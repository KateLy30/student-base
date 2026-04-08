using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Domain;
using StudentBase.Domain.Entities;

namespace StudentBase.MAUI.ViewModels
{
    public partial class GroupCardViewModel : ViewModelBase
    {

        [RelayCommand]
        public static async Task ExitAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        [ObservableProperty]
        public partial int Id { get; set; }

        [ObservableProperty]
        public partial string Name {  get; set; }

        [ObservableProperty]
        public partial int ProgramId { get; set; }

        [ObservableProperty]
        public partial string ProgramSpecialty { get; set; }

        [ObservableProperty]
        public partial string ProgramQualification { get; set; }

        [ObservableProperty]
        public partial DateTime DateOfCreation { get; set; }

        [ObservableProperty]
        public partial StatusGroups Status { get; set; }

        [ObservableProperty]
        public partial int StudentsCount { get; set; }



        // заполнение полей
        public void UploadData(GroupEntity g)
        {
            Id = g.Id;
            Name = g.Name;
            DateOfCreation = g.DateOfCreation;
            ProgramId = g.ProgramId;
            ProgramSpecialty = g.EducationalProgram.Specialty;
            ProgramQualification = g.EducationalProgram.Qualification;
            Status = g.Status;
            StudentsCount = g.Students.Count;
        }
    }
}
