using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class GroupCardModalWindow : ContentPage
{
	private readonly GroupCardViewModel _groupCardViewModel;
	public GroupCardModalWindow(GroupCardViewModel groupCardViewModel)
	{
		InitializeComponent();
		_groupCardViewModel = groupCardViewModel;
		BindingContext = _groupCardViewModel;
	}
}