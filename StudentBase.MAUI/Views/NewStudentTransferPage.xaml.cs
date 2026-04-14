using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class NewStudentTransferPage : ContentPage
{
	private readonly NewStudentTransferViewModel _newStudentTransferViewModel;
	public NewStudentTransferPage(NewStudentTransferViewModel newStudentTransferViewModel)
	{
		InitializeComponent();
		_newStudentTransferViewModel = newStudentTransferViewModel;
		BindingContext = _newStudentTransferViewModel;
	}

  //  protected override async void OnAppearing()
  //  {
  //      base.OnAppearing();
		//await _newStudentTransferViewModel.LoadGroupsAsync();
		//await _newStudentTransferViewModel.LoadStudentsAsync();
  //  }

}