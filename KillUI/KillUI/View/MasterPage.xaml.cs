#if ANDROID
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Android.App;
using Android.Util;
using Android.Content;
using Android.OS;
#endif

using CommunityToolkit.Maui.Views;

namespace KillUI;

public partial class MasterPage : ContentPage
{
	public MasterPage()
	{
		InitializeComponent();

		App.mainpage = this;

		if (!App.alertsRunning)
		{
			App.alertsRunning = true;
			App.SAPP.StartManagingAlerts();
		}
	}

	private void OnStartClicked(object sender, EventArgs e)
	{
		App.SAPP.MainPage = new NavigationPage(new AlertPage());
	}

	internal void TriggerAlert()
	{
		try
		{
			Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(async () =>
			{
				AlertPopup popup = new AlertPopup();

				if (App.inBackground)
				{
					popup.OnStartAlert();
				}
				else
				{
					await this.ShowPopupAsync(popup);
				}
			});
		}
		catch (Exception ex)
		{
			if (App.debugging) { throw; };
			if (App.debugLogging) { App.ReportError(ex.ToString()); };
		}
	}
}