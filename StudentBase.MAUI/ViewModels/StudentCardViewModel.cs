using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Application.Interfaces;
using StudentBase.Domain;
using StudentBase.Domain.Entities;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class StudentCardViewModel(IDataService dataService) : ViewModelBase
    {
        private readonly IDataService _dataService = dataService;

        [ObservableProperty]
        public partial ObservableCollection<StudentTransferEntity> Transfers { get; set; } = [];

        [ObservableProperty]
        public partial bool HasTransfers { get; set; } = false;

        [ObservableProperty]
        public partial int Id { get; set; }

        [ObservableProperty]
        public partial string Name { get;set; }

        [ObservableProperty]
        public partial string Phone { get; set; }

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
        public partial int GroupId { get; set; }

        [ObservableProperty]
        public partial string? GroupName { get; set; }

        [ObservableProperty]
        public partial int ProgramId { get; set; }

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


        // заполнение полей
        public async Task UploadData(StudentEntity s)
        {
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
            DateOfBirth = s.DateOfBirth;
            DateOfReceipt = s.DateOfReceipt;
            EducationLevel = s.EducationLevel;
            FormOfEducation = s.FormOfEducation;
            DurationTraining = s.DurationTraining;
            GroupId = s.CurrentGroupId;
            GroupName = s.EducationalGroup.Name;
            ProgramId = s.EducationalGroup.ProgramId;
            ProgramSpecialty = s.EducationalGroup.EducationalProgram.Specialty;
            ProgramQualification = s.EducationalGroup.EducationalProgram.Qualification;
            Status = s.Status;
        }

    }
}
