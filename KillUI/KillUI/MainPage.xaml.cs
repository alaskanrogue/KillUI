namespace KillUI
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void OnStartClicked(object sender, EventArgs e)
		{
			App.SAPP.MainPage = new NavigationPage(new MasterPage());
		}
	}

}
