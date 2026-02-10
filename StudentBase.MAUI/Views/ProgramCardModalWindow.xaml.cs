using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class ProgramCardModalWindow : ContentPage
{
	private readonly ProgramCardViewModel _viewModel;
	public ProgramCardModalWindow(ProgramCardViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}
}