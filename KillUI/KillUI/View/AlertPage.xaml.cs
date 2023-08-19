namespace KillUI;

public partial class AlertPage : ContentPage
{
	public AlertPage()
	{
		InitializeComponent();
	}

	private void OnCancelClicked(object sender, EventArgs e)
	{
		App.SAPP.MainPage = new NavigationPage(new MasterPage());
	}
}