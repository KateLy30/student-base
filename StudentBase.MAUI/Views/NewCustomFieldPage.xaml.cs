using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class NewCustomFieldPage : ContentPage
{
	private readonly NewCustomFieldViewModel _viewModel;
	public NewCustomFieldPage(NewCustomFieldViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}
}