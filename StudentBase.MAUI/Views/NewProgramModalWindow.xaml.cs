using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class NewProgramModalWindow : ContentPage
{
	private readonly NewProgramViewModel _newProgramViewModel;
	public NewProgramModalWindow(NewProgramViewModel newProgramViewModel)
	{
		InitializeComponent();
		_newProgramViewModel = newProgramViewModel;
		BindingContext = _newProgramViewModel;
	}
}