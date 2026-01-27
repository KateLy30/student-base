
using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI;

public partial class NewGroupModalWindow : ContentPage
{
    private readonly NewGroupViewModel _newGroupViewModel;
    public NewGroupModalWindow(NewGroupViewModel newGroupViewModel)
	{
		InitializeComponent();
        _newGroupViewModel = newGroupViewModel;
        BindingContext = _newGroupViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _newGroupViewModel.LoadProgramsAsync();
    }
}