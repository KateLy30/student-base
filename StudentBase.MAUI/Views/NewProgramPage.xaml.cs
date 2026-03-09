using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class NewProgramPage : ContentPage
{
	private readonly NewProgramViewModel _newProgramViewModel;
	public NewProgramPage(NewProgramViewModel newProgramViewModel)
	{
		InitializeComponent();
		_newProgramViewModel = newProgramViewModel;
		BindingContext = _newProgramViewModel;
	}
}