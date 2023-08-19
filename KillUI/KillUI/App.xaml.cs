namespace KillUI
{
	public partial class App : Microsoft.Maui.Controls.Application
	{
		internal static App SAPP;
		internal static MasterPage mainpage;

		internal static bool alertsRunning;

		internal static bool debugging;
		internal static bool debugLogging;

		// <summary>
		// Lifecycle
		// </summary
		internal static bool inBackground = false;

		public App()
		{
			InitializeComponent();

			MainPage = new AppShell();

			// Create a reference to this application
			SAPP = this;
		}

		internal void StartManagingAlerts()
		{
			Thread alerts = new Thread(() => ManageAlerts());
			alerts.Start();
		}

		public async void ManageAlerts()
		{
			bool started = false;
			bool end = false;

			while (!end)
			{
				if (started)
				{
					await Task.Delay(60000);
				}
				else
				{
					await Task.Delay(15000);
					started = true;
				}

				App.mainpage.TriggerAlert();
			}
		}

		// <summary>
		// Error Reporting
		// </summary>

		internal static bool errorReportComplete;

		public static void ReportError(string comment)
		{
			string np = "<p>";
			string cr = "</p>";

			string error = np + comment + cr;

			System.Diagnostics.Debug.WriteLine(error);
		}
	}
}
