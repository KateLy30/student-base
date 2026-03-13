using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class GroupCardPage : ContentPage
{
	private readonly GroupCardViewModel _groupCardViewModel;
	public GroupCardPage(GroupCardViewModel groupCardViewModel)
	{
		InitializeComponent();
		_groupCardViewModel = groupCardViewModel;
		BindingContext = _groupCardViewModel;
	}
}