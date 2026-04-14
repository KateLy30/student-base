using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Application.Interfaces;
using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Extensions;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class NewPaymentViewModel(IDataService dataService) : ViewModelBase
    {
        private readonly IDataService _dataService = dataService;
        private PaymentEntity _payment = new();

        [ObservableProperty]
        public partial ObservableCollection<StudentEntity> Students { get; set; } = new();

        [ObservableProperty]
        public partial StudentEntity SelectedStudent { get; set; }

        [ObservableProperty]
        public partial bool ChangedStudent { get; set; } = true;

        [ObservableProperty]
        public partial ObservableCollection<SemesterItem> AvailableSemesters { get; set; } = new();

        [ObservableProperty]
        public partial PaymentType SelectedPaymentType { get; set; } = PaymentType.Cash;  // По умолчанию наличные

        [ObservableProperty]
        public partial bool IsStudentSelected { get; set; }

        [ObservableProperty]
        public partial bool HasSelectedSemester { get; set; }

        [ObservableProperty]
        public partial int SelectedSemesterCount { get; set; }

        [ObservableProperty]
        public partial decimal TotalAmount { get; set; }

        [ObservableProperty]
        public partial bool IsDiscounted { get; set; }

        [ObservableProperty]
        public partial string DiscountReason { get; set; }

        [ObservableProperty]
        public partial string DiscountedAmountText { get; set; } = string.Empty;

        [ObservableProperty]
        public partial decimal DiscountedAmount { get; set; }

        [ObservableProperty]
        public partial string Comment { get; set; }

        [ObservableProperty]
        public partial bool IsSemesterSelectionEnabled { get; set; } = true;


        // Текст кнопки оплаты
        public string PaymentButtonText
        {
            get
            {
                if (!IsStudentSelected) return "Выберите студента";
                if (!HasSelectedSemester) return "Выберите семестр(ы)";
                if (IsDiscounted)
                {
                    if (DiscountedAmount <= 0 
                        || string.IsNullOrWhiteSpace(DiscountReason)) return "Введите причину льготы и сумму";
                    return "Оплатить по льготе";
                }
                return SelectedSemesterCount > 1 ? $"Оплатить {SelectedSemesterCount} семестр(ов)" : "Оплатить семестр";
            }
        }

        // можно ли создать оплату
        public bool CanCreatePayment
        {
            get
            {
                if (!HasSelectedSemester) return false;
                if (IsDiscounted)
                {
                    if (DiscountedAmount <= 0 || string.IsNullOrWhiteSpace(DiscountReason)) return false;
                }
                if (!IsStudentSelected || SelectedStudent == null) return false;

                return true;
            }
        }

        // при изменении льготной суммы
        partial void OnDiscountedAmountTextChanged(string value)
        {
            if (decimal.TryParse(value, out decimal result))
                DiscountedAmount = result;
            else
                DiscountedAmount = 0;

            OnPropertyChanged(nameof(CanCreatePayment));
            OnPropertyChanged(nameof(PaymentButtonText));
        }

        // При изменении причины льготы
        partial void OnDiscountReasonChanged(string value)
        {
            OnPropertyChanged(nameof(CanCreatePayment));
            OnPropertyChanged(nameof(PaymentButtonText));
        }

        // при изменении выбранного студента
        partial void OnSelectedStudentChanged(StudentEntity value)
        {
            IsStudentSelected = value != null;
            if (value != null)
            {
                _ = LoadAvailableSemesterAsync(value);
            }
            else
            {
                AvailableSemesters.Clear();
                HasSelectedSemester = false;
                SelectedSemesterCount = 0;
                TotalAmount = 0;
            }
            OnPropertyChanged(nameof(CanCreatePayment));
        }

        // при изменении льготы
        async partial void OnIsDiscountedChanged(bool value)
        {
            if (value)
            {
                IsSemesterSelectionEnabled = false;
                await UpdateSelectionInfo();
            }
            else
            {
                DiscountReason = string.Empty;
                IsSemesterSelectionEnabled = true;
                DiscountedAmount = 0;
            }

            OnPropertyChanged(nameof(PaymentButtonText));
            OnPropertyChanged(nameof(CanCreatePayment));
        }

        // обновление информации о выбранных семестрах
        private async Task UpdateSelectionInfo()
        {
            if (AvailableSemesters.Count == 0 ||
                AvailableSemesters == null)
            {
                HasSelectedSemester = false;
                SelectedSemesterCount = 0;
                TotalAmount = 0;
                OnPropertyChanged(nameof(CanCreatePayment));
                return;
            }

            var selected = AvailableSemesters.Where(s => s.IsSelected).ToList();
            SelectedSemesterCount = selected.Count;
            TotalAmount = selected.Sum(s => s.Cost);
            HasSelectedSemester = SelectedSemesterCount > 0;

            if (IsDiscounted && SelectedSemesterCount > 1)
            {
                var firstSelected = selected.First();
                foreach (var semester in selected.Skip(1))
                    semester.IsSelected = false;

                firstSelected.IsSelected = true;

                await Shell.Current.DisplayAlert(
                    "Внимание",
                    "При льготе можно оплатить только один семестр",
                    "OK");
            }

            OnPropertyChanged(nameof(PaymentButtonText));
            OnPropertyChanged(nameof(CanCreatePayment));
        }

        // загрузка доступных (неоплаченных) семестров
        private async Task LoadAvailableSemesterAsync(StudentEntity student)
        {
            try
            {
                IsBusy = true;
                var paidSemester = await _dataService.PaymentsService.GetAllPaymentsByStudentAsync(student.Id);
                if (paidSemester == null) return;
                // Получаем список номеров оплаченных семестров
                var paidSemesterNumbers = paidSemester.Select(p => p.PaidSemester).ToList();
                AvailableSemesters.Clear();

                for (int i = 1; i <= (int)student.DurationTraining; i++)
                {
                    if (!paidSemesterNumbers.Contains(i))
                    {
                        var semester = new SemesterItem
                        {
                            SemesterNumber = i,
                            Cost = student.EducationalGroup.EducationalProgram.CostPerSemester,
                            EnrollmentDate = student.DateOfReceipt,
                            IsSelected = false
                        };

                        semester.PropertyChanged += async (s, e) =>
                        {
                            if (e.PropertyName == nameof(SemesterItem.IsSelected))
                            {
                                await UpdateSelectionInfo();
                            }
                        };

                        AvailableSemesters.Add(semester);
                    }
                }
                await UpdateSelectionInfo();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"{ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        // загрузка списка студентов
        public async Task LoadStudentsAsync()
        {
            try
            {
                IsBusy = true;
                var list = await _dataService.StudentService.GetAllStudentsAsync();
                if (list == null) return;
                Students.Clear();
                foreach (var s in list)
                    Students.Add(s);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"{ex.Message}", "ОК");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private static async Task CancelAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }



        [RelayCommand]
        private async Task CreatePaymentAsync()
        {
            try
            {
                if (!CanCreatePayment) return;
                var selectedSemester = AvailableSemesters.Where(s => s.IsSelected).ToList();
                if (selectedSemester.Count == 0) return;

                if (IsDiscounted)
                {
                    if (selectedSemester.Count > 1)
                    {
                        await Shell.Current.DisplayAlert(
                            "Ошибка",
                            "При льготе можно оплатить только один семестер",
                            "OK");
                        return;
                    }
                    else if (DiscountedAmount <= 0)
                    {
                        await Shell.Current.DisplayAlert(
                            "Ошибка",
                            "Введите сумму оплаты",
                            "OK");
                        return;
                    }
                    else if (string.IsNullOrWhiteSpace(DiscountReason))
                    {
                        await Shell.Current.DisplayAlert(
                           "Ошибка",
                           "Укажите причину льготы",
                           "OK");
                        return;
                    }
                }
                OnPropertyChanged(nameof(PaymentButtonText));

                int createCount = 0;
                foreach (var semester in selectedSemester)
                {
                    var payment = new PaymentEntity
                    {
                        StudentId = SelectedStudent.Id,
                        Student = SelectedStudent,
                        Amount = (decimal)(IsDiscounted ? DiscountedAmount! : semester.Cost),
                        PaymentDate = DateTime.Now,
                        PaidSemester = semester.SemesterNumber,
                        IsDiscount = IsDiscounted,
                        ReasonDiscount = IsDiscounted ? DiscountReason : null,
                        PaymentType = SelectedPaymentType,
                        Comment = string.IsNullOrWhiteSpace(Comment) ? null : Comment
                    };

                    await _dataService.PaymentsService.CreatePaymentAsync(payment);
                    createCount++;
                }
                var message = IsDiscounted ? $"Создана квитанция на сумму {DiscountedAmount:N0} рублей."
                    : $"Создано {createCount} квитанций на сумму {TotalAmount:N0} рублей.";

                await Shell.Current.DisplayAlert("Успех", message, "OK");

                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(
                    "Ошибка",
                    $"Не удалось создать оплату: {ex.Message}",
                    "OK");
            }
            finally
            {
                OnPropertyChanged(nameof(PaymentButtonText));
            }
        }

        public void LoadForm(StudentEntity? student)
        {
            SelectedStudent = Students.FirstOrDefault(s => s.Id == student.Id);
            IsStudentSelected = true;
            ChangedStudent = false;
        }


        // ==================== Вложенный класс SemesterItem ====================

        public partial class SemesterItem : ObservableObject
        {
            private int _semesterNumber;
            private bool _isSelected;
            private decimal _cost;
            private DateTime _enrollmentDate;

            public int SemesterNumber
            {
                get => _semesterNumber;
                set => SetProperty(ref _semesterNumber, value);
            }

            public bool IsSelected
            {
                get => _isSelected;
                set => SetProperty(ref _isSelected, value);
            }

            public decimal Cost
            {
                get => _cost;
                set => SetProperty(ref _cost, value);
            }

            public DateTime EnrollmentDate
            {
                get => _enrollmentDate;
                set => SetProperty(ref _enrollmentDate, value);
            }

            // Вычисляемое свойство для отображения
            public string DisplayName => SemesterNumber.ToSemesterDisplay(EnrollmentDate);
        }
    }
}
