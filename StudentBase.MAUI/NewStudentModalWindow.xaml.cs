using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using System.Threading.Tasks;
using StudentBase.Infrastructure.EntityFramework.Repositories;

namespace StudentBase.MAUI;

public partial class NewStudentModalWindow : ContentPage
{
	private readonly IStudentRepository _studentRepository;
	public NewStudentModalWindow()
	{
		InitializeComponent();
		_studentRepository = App.Services.GetRequiredService<IStudentRepository>();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		var student = new StudentEntity
		{
			Name = nameEntry.Text,
			Phone = phoneEntry.Text,
			Email = emailEntry.Text,
			DateOfReceipt = dateOfReceiptEntry.Text,
			Gender = genderEntry.Text
		};
		var id = await _studentRepository.CreateAsync(student);
		await Navigation.PopModalAsync();
    }
}