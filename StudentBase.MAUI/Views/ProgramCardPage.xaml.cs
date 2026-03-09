using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class ProgramCardPage : ContentPage
{
	private readonly ProgramCardViewModel _viewModel;
	public ProgramCardPage(ProgramCardViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}
}