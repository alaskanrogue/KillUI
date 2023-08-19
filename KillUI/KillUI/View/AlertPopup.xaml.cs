using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace KillUI
{
	public partial class AlertPopup : Popup
	{
		public AlertPopup()
		{
			InitializeComponent();
		}

		public void OnContinueStartAlert(object? sender, EventArgs e) => OnStartAlert();

		public void OnStartAlert()
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				App.SAPP.MainPage = new NavigationPage(new AlertPage());
			});

			Close();
		}
	}
}