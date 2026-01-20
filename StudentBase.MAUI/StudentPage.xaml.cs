using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace StudentBase.MAUI;
public partial class StudentPage : ContentPage
{
    // список студентов для заполнения CollectionView
    private ObservableCollection<StudentEntity> Students;
    private readonly IStudentRepository _studentRepository;
    public StudentPage()
    {
        InitializeComponent();

        Students = new ObservableCollection<StudentEntity>();
        _studentRepository = App.Services!.GetRequiredService<IStudentRepository>();
    }

    // подгрузка списка каждый раз при открытии окна
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadStudents();
    }
    private async void LoadStudents()
    {
        var students = await _studentRepository.GetAllAsync();
        if (students == null) return;
        Students.Clear();
        foreach (var student in students)
        {
            Students.Add(student);
        }
        studentsList.ItemsSource = Students;
    }

    private async void Button_Add_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NewStudentModalWindow(null));
    }

    private  async void Button_Edit_Clicked(object sender, EventArgs e)
    {
        // получаем выбранный объект
        StudentEntity? selectedStudent = studentsList.SelectedItem as StudentEntity;
        if (selectedStudent == null)
        {
            await DisplayAlert("Ошибка", "Выберите студента для изменения", "ОК");
            return;
        }
        await Navigation.PushModalAsync(new NewStudentModalWindow(selectedStudent));
    }

    private async void Button_Delete_Clicked(object sender, EventArgs e)
    {
       // получаем выбранный объект
        StudentEntity? student = studentsList.SelectedItem as StudentEntity;
        if (student == null)
        {
            await DisplayAlert("Ошибка", "Не выбран объект", "ОК");
            return;
        }
        // проверка ответа пользователя
        if (await DisplayAlert("Удаление", $"Вы действительно хотите удалить студента {student.Name} ?", "Да", "Нет"))
        {
            if (!await _studentRepository.DeleteAsync(student.Id))
            {
                await DisplayAlert("Ошибка", "Не получилось удалить объект", "ОК");
                return;
            }
        }
        else return;
    }
    // заполнение данных второго окна при выборе объекта
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
        nameGroup_Label.Text = student.GroupName;
        idProgram_Label.Text = student.ProgramId.ToString();
    }
}