using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class NewStudentPage : ContentPage
{
    private readonly NewStudentViewModel _newStudentViewModel;
    public NewStudentPage(NewStudentViewModel newStudentViewModel )
    {
        InitializeComponent();
        _newStudentViewModel = newStudentViewModel;
        BindingContext = _newStudentViewModel;
    }
}