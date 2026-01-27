
using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI;

public partial class NewStudentModalWindow : ContentPage
{
    private readonly NewStudentViewModel _newStudentViewModel;
    public NewStudentModalWindow(NewStudentViewModel newStudentViewModel )
    {
        InitializeComponent();
        _newStudentViewModel = newStudentViewModel;
        BindingContext = _newStudentViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _newStudentViewModel.LoadGroupsAsync();
        await _newStudentViewModel.LoadProgramsAsync();
    }
}