using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using System.Collections.ObjectModel;
using System.Formats.Tar;

namespace StudentBase.MAUI;

public partial class NewStudentModalWindow : ContentPage
{
    private readonly StudentEntity? currentStudent;
    private readonly IStudentRepository _studentRepository;
    public NewStudentModalWindow(StudentEntity? student = null)
    {
        InitializeComponent();
        _studentRepository = App.Services!.GetRequiredService<IStudentRepository>();

        currentStudent = student;

        if (currentStudent == null || currentStudent.Id == 0 || string.IsNullOrWhiteSpace(currentStudent.Name))
        {
            nameEntry.Text = string.Empty;
            phoneEntry.Text = string.Empty;
            emailEntry.Text = string.Empty;
            dateOfBirthEntry.Text = string.Empty;
            dateOfReceiptEntry.Text = string.Empty;
            genderEntry.Text = string.Empty;
            groupIdEntry.Text = string.Empty;
            programIdEntry.Text = string.Empty;
        }
        else
        {
            nameEntry.Text = currentStudent.Name;
            phoneEntry.Text = currentStudent.Phone;
            emailEntry.Text = currentStudent.Email;
            dateOfBirthEntry.Text = currentStudent.DateOfBirth.ToString();
            dateOfReceiptEntry.Text = currentStudent.DateOfReceipt;
            genderEntry.Text = currentStudent.Gender;
            groupIdEntry.Text = currentStudent.GroupId.ToString();
            programIdEntry.Text = currentStudent.ProgramId.ToString();
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (currentStudent == null)
        {
            var student = new StudentEntity
            {
                Name = nameEntry.Text,
                Phone = phoneEntry.Text,
                Email = emailEntry.Text,
                DateOfBirth = DateOnly.Parse(dateOfBirthEntry.Text),
                DateOfReceipt = dateOfReceiptEntry.Text,
                Gender = genderEntry.Text,
                GroupId = Int32.Parse(groupIdEntry.Text),
                ProgramId = Int32.Parse(programIdEntry.Text)
            };
            var id = await _studentRepository.CreateAsync(student);
        }
        else
        {
            currentStudent.Name = nameEntry.Text;
            currentStudent.Phone = phoneEntry.Text;
            currentStudent.Email = emailEntry.Text;
            currentStudent.DateOfBirth = DateOnly.Parse(dateOfBirthEntry.Text);
            currentStudent.DateOfReceipt = dateOfReceiptEntry.Text;
            currentStudent.Gender = genderEntry.Text;
            currentStudent.GroupId = Int32.Parse(groupIdEntry.Text);
            currentStudent.ProgramId = Int32.Parse(programIdEntry.Text);

            if (!await _studentRepository.UpdateAsync(currentStudent))
            {
                await DisplayAlert("Ошибка", "Ошибка при попытке изменить данные", "ОК");
                return;
            }
        }
        await Navigation.PopModalAsync();
    }
}