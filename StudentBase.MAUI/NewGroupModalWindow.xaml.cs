using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using StudentBase.Infrastructure.EntityFramework.Repositories;
using System.Threading.Tasks;

namespace StudentBase.MAUI;

public partial class NewGroupModalWindow : ContentPage
{
	private readonly GroupEntity? currentGroup;
    private readonly IGroupRepository _groupRepository;
    public NewGroupModalWindow(GroupEntity? group)
	{
		InitializeComponent();
        _groupRepository = App.Services!.GetRequiredService<IGroupRepository>();
        
        currentGroup = group;
        PrepareFields();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (currentGroup == null || currentGroup.Id == 0 || string.IsNullOrWhiteSpace(currentGroup.Name))
        {
            var group = new GroupEntity
            {
                ProgramId = Int32.Parse(programIdEntry.Text),
                Name = nameEntry.Text,
                YearOfEntry = DateOnly.Parse(yearEntry.Text),
                //DurationYears = durationEntry.Text,
                //Status = statusEntry.Text
            };
            var id = await _groupRepository.CreateAsync(group);
        }
        else
        {
            currentGroup.ProgramId = Int32.Parse(programIdEntry.Text);
            currentGroup.Name = nameEntry.Text;
            currentGroup.YearOfEntry = DateOnly.Parse(yearEntry.Text);
            //currentGroup.DurationYears 
            //currentGroup.Status 

            if (!await _groupRepository.UpdateAsync(currentGroup))
            {
                await DisplayAlert("Ошибка", "Ошибка при попытке изменить данные", "ОК");
                return;
            }
        }
        await Navigation.PopModalAsync();
    }

    private void PrepareFields()
    {
        if (currentGroup == null || currentGroup.Id == 0 || string.IsNullOrWhiteSpace(currentGroup.Name))
        {
            programIdEntry.Text = string.Empty;
            nameEntry.Text = string.Empty;
            yearEntry.Text = string.Empty;
            durationEntry.Text = string.Empty;
            statusEntry.Text = string.Empty;
        }
        else
        {
            programIdEntry.Text = currentGroup.ProgramId.ToString();
            nameEntry.Text = currentGroup.Name;
            yearEntry.Text = currentGroup.YearOfEntry.ToString();
            durationEntry.Text = currentGroup.DurationYears.ToString();
            statusEntry.Text = currentGroup.Status.ToString();
        }
    }
}