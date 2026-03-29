using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class PaymentPage : ContentPage
{
	private readonly PaymentPageViewModel _paymentPageViewModel;
	public PaymentPage(PaymentPageViewModel paymentPageViewModel)
	{
		InitializeComponent();
		_paymentPageViewModel = paymentPageViewModel;
		BindingContext = _paymentPageViewModel;
	}
}