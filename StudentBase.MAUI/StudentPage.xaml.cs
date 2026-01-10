using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using System.Collections.ObjectModel;
namespace StudentBase.MAUI;
public partial class StudentPage : ContentPage
{
    private ObservableCollection<StudentEntity> _students;
    public StudentPage()
    {
        InitializeComponent();

        _students = new ObservableCollection<StudentEntity>();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadStudents();
    }
    private async void LoadStudents()
    {
        var repo = App.Services.GetRequiredService<IStudentRepository>();
        var students = await repo.GetAllAsync();
        _students.Clear();
        foreach (var student in students)
        {
            _students.Add(student);
        }
        studentsList.ItemsSource = _students;
    }
    private async void Button_Add_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NewStudentModalWindow());
    }

    private  void Button_Edit_Clicked(object sender, EventArgs e)
    {

    }
    private  void Button_Delete_Clicked(object sender, EventArgs e)
    {

    }

    private void studentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}