using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace StudentBase.MAUI;

public partial class GroupPage : ContentPage
{
    private ObservableCollection<GroupEntity> _groups;
    private readonly IGroupRepository _groupRepository;
	public GroupPage()
	{
		InitializeComponent();
        _groupRepository = App.Services!.GetRequiredService<IGroupRepository>();
        _groups = new ObservableCollection<GroupEntity>();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadGroups();
    }
    private async void LoadGroups()
    {
        var groups = await _groupRepository.GetAllAsync();
        _groups.Clear();
        foreach (var group in groups)
        {
            _groups.Add(group);
        }
        groupsList.ItemsSource = _groups;
    }
    private void groupsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        GroupEntity? group = groupsList.SelectedItem as GroupEntity;
        if (group == null) return;

        id_Label.Text = group.Id.ToString();
        programId_Label.Text = group.ProgramId.ToString();
        name_Label.Text = group.Name;
        year_Label.Text = group.YearOfEntry.ToString();
        duration_Label.Text = group.DurationYears.ToString();
        status_Label.Text = group.Status.ToString();
    }

    private async void Button_Add_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NewGroupModalWindow(null));
    }

    private async void Button_Edit_Clicked(object sender, EventArgs e)
    {
        GroupEntity? selectedGroup = groupsList.SelectedItem as GroupEntity;
        if (selectedGroup == null)
        {
            await DisplayAlert("Ошибка", "Выберите студента для изменения", "ОК");
            return;
        }

        await Navigation.PushModalAsync(new NewGroupModalWindow(selectedGroup));
    }

    private void Button_Delete_Clicked(object sender, EventArgs e)
    {

    }
}