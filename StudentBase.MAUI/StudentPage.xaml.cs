using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using StudentBase.Infrastructure.EntityFramework.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
namespace StudentBase.MAUI;
public partial class StudentPage : ContentPage
{
    private ObservableCollection<StudentEntity> _students;
    private readonly IStudentRepository _studentRepository;
    public StudentPage()
    {
        InitializeComponent();

        _students = new ObservableCollection<StudentEntity>();
        _studentRepository = App.Services!.GetRequiredService<IStudentRepository>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadStudents();
    }
    private async void LoadStudents()
    {
        var students = await _studentRepository.GetAllAsync();
        _students.Clear();
        foreach (var student in students)
        {
            _students.Add(student);
        }
        studentsList.ItemsSource = _students;
    }
    private async void Button_Add_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NewStudentModalWindow(null));
    }

    private  async void Button_Edit_Clicked(object sender, EventArgs e)
    {
        StudentEntity? selectedStudent = studentsList.SelectedItem as StudentEntity;
        if (selectedStudent == null)
        {
            await DisplayAlert("Ошибка", "Выберите студента для изменения", "ОК");
            return;
        }

        await Navigation.PushModalAsync(new NewStudentModalWindow(selectedStudent));
    }
    private  void Button_Delete_Clicked(object sender, EventArgs e)
    {

    }

    private void studentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        StudentEntity? student = studentsList.SelectedItem as StudentEntity;
        if (student == null) return;

        id_Label.Text = student.Id.ToString();
        name_Label.Text = student.Name;
        phone_Label.Text = student.Phone;
        email_Label.Text = student.Email;
        dateOfBirth_Label.Text = student.DateOfBirth.ToString();
        dateOfReceipt_Label.Text = student.DateOfReceipt;
        gender_Label.Text = student.Gender;
        idGroup_Label.Text = student.GroupId.ToString();
        idProgram_Label.Text = student.ProgramId.ToString();
    }
}