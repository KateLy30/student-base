using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class NewPaymentPage : ContentPage
{
	private readonly NewPaymentViewModel _newPaymentViewModel;
	public NewPaymentPage(NewPaymentViewModel newPaymentViewModel)
	{
		InitializeComponent();
		_newPaymentViewModel = newPaymentViewModel;
		BindingContext = _newPaymentViewModel;
	}
}