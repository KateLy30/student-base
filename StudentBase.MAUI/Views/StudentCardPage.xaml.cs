using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class StudentCardPage : ContentPage
{
	private readonly StudentCardViewModel _studentCardViewModel;
	public StudentCardPage(StudentCardViewModel studentCardViewModel)
	{
		InitializeComponent();
		_studentCardViewModel = studentCardViewModel;
		BindingContext = _studentCardViewModel;
	}
}