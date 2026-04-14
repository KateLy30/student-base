using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Application.Interfaces;
using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class StudentCardViewModel(IDataService dataService,
                                                ICustomFieldRepository customFieldRepository,
                                                IDynamicFieldRepository dynamicFieldRepository) : ViewModelBase
    {
        private readonly IDataService _dataService = dataService;
        private readonly ICustomFieldRepository _customFieldRepository = customFieldRepository;
        private readonly IDynamicFieldRepository _dynamicFieldRepository = dynamicFieldRepository;

        public class CustomFieldDisplay
        {
            public string DisplayName { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
        }


        [ObservableProperty]
        public partial ObservableCollection<CustomFieldDisplay>? CustomFieldsWithValues { get; set; } = new();
        [ObservableProperty]
        public partial ObservableCollection<StudentTransferEntity>? Transfers { get; set; } = [];

        [ObservableProperty]
        public partial ObservableCollection<PaymentEntity>? Payments { get; set; } = new();

        [ObservableProperty]
        public partial bool HasTransfers { get; set; } = false;

        [ObservableProperty]
        public partial bool HasCustomField { get; set; } = false;

        [ObservableProperty]
        public partial bool HasPayments { get; set; } = false;

        [ObservableProperty]
        public partial int Id { get; set; }

        [ObservableProperty]
        public partial string? Name { get;set; }

        [ObservableProperty]
        public partial string? Phone { get; set; }

        [ObservableProperty]
        public partial string? Email { get; set; }

        [ObservableProperty]
        public partial string? PassportNumber { get; set; }

        [ObservableProperty]
        public partial string? Snils { get; set; }

        [ObservableProperty]
        public partial DateTime DateOfBirth { get; set;}

        [ObservableProperty]
        public partial DateTime DateOfReceipt { get; set; }

        [ObservableProperty]
        public partial LevelsOfEducation EducationLevel { get; set; }

        [ObservableProperty]
        public partial TermsOfStudy DurationTraining { get; set; }

        [ObservableProperty]
        public partial FormsOfEducation FormOfEducation { get;set; }

        [ObservableProperty]
        public partial int? GroupId { get; set; }

        [ObservableProperty]
        public partial string? GroupName { get; set; }

        [ObservableProperty]
        public partial int? ProgramId { get; set; }

        [ObservableProperty]
        public partial string? ProgramSpecialty { get; set; }

        [ObservableProperty]
        public partial string? ProgramQualification { get; set; }

        [ObservableProperty]
        public partial StatusStudents Status {  get; set; }


        [RelayCommand]
        public static async Task ExitAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        public async Task LoadCustomFields(int studentId)
        {
            var customField = await _customFieldRepository.GetAllAsync();
            if (customField == null) return;
            var dynamicFields = await _dynamicFieldRepository.GetAllByEntityIdAsync(studentId);
            if (dynamicFields == null) return;

            CustomFieldsWithValues.Clear();

            foreach (var field in customField)
            {
                var value = dynamicFields.FirstOrDefault(df => df.CustomFieldId == field.Id)?.Value ?? "-";
                CustomFieldsWithValues.Add(new CustomFieldDisplay
                {
                    DisplayName = field.DisplayName,
                    Value = value,
                });
            }
            HasCustomField = true;
        }
        //public async Task LoadPayments(int studentId)
        //{
        //    var payments = await _dataService.PaymentsService.GetAllPaymentsByStudentAsync(studentId);
        //    if (payments == null) return;

        //    Payments.Clear();

        //    foreach( var pay in payments)
        //    {
        //        Payments.Add(pay);
        //    }
        //    HasPayments = true;
        //}

        // заполнение полей
        public async Task UploadData(StudentEntity s)
        {
            if (s is null) return;
            await LoadCustomFields(s.Id);
           // await LoadPayments(s.Id);
            if (s.StudentTransfers.Count == 0) HasTransfers = false;
            else
            {
                HasTransfers = true;
                var list = await _dataService.StudentTransferService.GetAllStudentTransfersByStudentAsync(s.Id);
                if (list == null) return;
                Transfers.Clear();
                foreach (var item in list) 
                    Transfers.Add(item);

            }
            Id = s.Id;
            Name = s.Name;
            Phone = s.Phone;
            Email = s.Email ?? "-";
            PassportNumber = s.PassportNumber ?? "-";
            Snils = s.Snils ?? "-";
            DateOfBirth = s.DateOfBirth;
            DateOfReceipt = s.DateOfReceipt;
            EducationLevel = s.EducationLevel;
            FormOfEducation = s.FormOfEducation;
            DurationTraining = s.DurationTraining;

            if (s.EducationalGroup != null && s.CurrentGroupId != null && s.CurrentGroupId != 0)
            {
                GroupId = s.CurrentGroupId;
                GroupName = s.EducationalGroup?.Name ?? "-";
                ProgramId = s.EducationalGroup?.ProgramId;
                ProgramSpecialty = s.EducationalGroup?.EducationalProgram?.Specialty ?? "-";
                ProgramQualification = s.EducationalGroup?.EducationalProgram?.Qualification ?? "-";
            } 
           
            Status = s.Status;
        }

    }
}
