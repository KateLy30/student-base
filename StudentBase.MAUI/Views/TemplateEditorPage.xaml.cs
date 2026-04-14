using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class TemplateEditorPage : ContentPage
{
	private readonly NewTemplateViewModel _newTemplateViewModel;
	public TemplateEditorPage(NewTemplateViewModel newTemplateViewModel)
	{
		InitializeComponent();
		_newTemplateViewModel = newTemplateViewModel;
		BindingContext = _newTemplateViewModel;
	}
}