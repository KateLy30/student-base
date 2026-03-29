
using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class NewGroupPage : ContentPage
{
    private readonly NewGroupViewModel _newGroupViewModel;
    public NewGroupPage(NewGroupViewModel newGroupViewModel)
	{
		InitializeComponent();
        _newGroupViewModel = newGroupViewModel;
        BindingContext = _newGroupViewModel;
    }
    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();
    //    await _newGroupViewModel.LoadProgramsAsync();
    //}
}