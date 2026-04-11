using StudentBase.MAUI.ViewModels;
using System.Threading.Tasks;

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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
		await _paymentPageViewModel.LoadAsync();
    }
}