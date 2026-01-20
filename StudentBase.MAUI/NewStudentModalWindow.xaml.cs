using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using System.Collections.ObjectModel;
using System.Formats.Tar;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StudentBase.MAUI;

public partial class NewStudentModalWindow : ContentPage
{
    private GroupEntity? SelectedGroup;

    // список групп для Picker
    public ObservableCollection<GroupEntity> Groups { get; }
    private readonly StudentEntity? currentStudent;
    private readonly IGroupRepository _groupRepository;
    private readonly IStudentRepository _studentRepository;
    public NewStudentModalWindow(StudentEntity? student = null)
    {
        InitializeComponent();

        Groups = new ObservableCollection<GroupEntity>();
        _studentRepository = App.Services!.GetRequiredService<IStudentRepository>();
        _groupRepository = App.Services!.GetRequiredService<IGroupRepository>();

        // инициализация переменной
        currentStudent = student;

        // очищаем поля, если окно используется для добавления нового объекта
        // и заполняем, если существующий объект редактируется
        PrepareFields();

        // загружаем список групп
        LoadGroups();
        picker_groups.ItemsSource = Groups;
    }

    private async void Button_Clicked_Accept(object sender, EventArgs e)
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
                GroupId = SelectedGroup!.Id,
                GroupName = SelectedGroup.Name,
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
            currentStudent.GroupId = SelectedGroup!.Id;
            currentStudent.GroupName = SelectedGroup.Name;
            currentStudent.ProgramId = Int32.Parse(programIdEntry.Text);

            if (!await _studentRepository.UpdateAsync(currentStudent))
            {
                await DisplayAlert("Ошибка", "Ошибка при попытке изменить данные", "ОК");
                return;
            }
        }
        await Navigation.PopModalAsync();
    }
    private async void LoadGroups()
    {
        var groupsFromDb = await _groupRepository.GetAllAsync();
        if (groupsFromDb == null) return;
        Groups.Clear();
        foreach (var g in groupsFromDb)
            Groups.Add(g);
        picker_groups.ItemsSource = Groups;
    }

    private async void Button_Clicked_Cancel(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();   
    }

    private void picker_groups_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectedGroup = (GroupEntity)picker_groups.SelectedItem;
    }

    private void PrepareFields()
    {
        if (currentStudent == null || currentStudent.Id == 0 || string.IsNullOrWhiteSpace(currentStudent.Name))
        {
            nameEntry.Text = string.Empty;
            phoneEntry.Text = string.Empty;
            emailEntry.Text = string.Empty;
            dateOfBirthEntry.Text = string.Empty;
            dateOfReceiptEntry.Text = string.Empty;
            genderEntry.Text = string.Empty;
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
            picker_groups.SelectedItem = currentStudent.GroupName;//TODO
            programIdEntry.Text = currentStudent.ProgramId.ToString();
        }
    }
}