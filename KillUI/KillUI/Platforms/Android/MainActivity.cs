using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Xamarin.Google;

namespace KillUI
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
	public class MainActivity : MauiAppCompatActivity
	{
		public static MainActivity ActivityCurrent { get; set; }

		internal static readonly string TAG = typeof(MainActivity).FullName;

		internal static Notification notification;
		internal static readonly int NOTIFICATION_ID = 1111;
		internal static readonly string CHANNEL_ID = "Killnotifications";

		internal static Intent serviceIntent;
		internal static Intent startServiceIntent;
		internal static Intent stopServiceIntent;
		internal static bool isStarted = false;

		internal static Broadcast receiver;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			try
			{
				base.OnCreate(savedInstanceState);

				receiver = new Broadcast();

				ActivityCurrent = this;

				if (savedInstanceState == null)
				{
					startServiceIntent = new Intent(this, typeof(ForegroundService));
					startServiceIntent.SetAction("START_SERVICE");

					stopServiceIntent = new Intent(this, typeof(ForegroundService));
					stopServiceIntent.SetAction("STOP_SERVICE");

					StartService(MainActivity.startServiceIntent);
					StartForegroundService(startServiceIntent);

					OnNewIntent(this.Intent);

					if (savedInstanceState != null)
					{
						isStarted = savedInstanceState.GetBoolean(Constants.SERVICE_STARTED_KEY, false);
					}
				}
			}
			catch (Exception ex)
			{
				if (App.debugging) { throw; };
			}
		}

		protected override void OnNewIntent(Intent intent)
		{
			try
			{
				if (intent == null)
				{
					return;
				}

				ForegroundService foregroundService = new ForegroundService();
				foregroundService.OnCreate();

				var bundle = intent.Extras;
				if (bundle != null)
				{
					if (bundle.ContainsKey(Constants.SERVICE_STARTED_KEY))
					{
						isStarted = true;
					}
				}
			}
			catch (Exception ex)
			{
				if (App.debugging) { throw; };
			}
		}

		protected override void OnSaveInstanceState(Bundle outState)
		{
			try
			{
				outState.PutBoolean(Constants.SERVICE_STARTED_KEY, isStarted);
				base.OnSaveInstanceState(outState);
			}
			catch (Exception ex)
			{
				if (App.debugging) { throw; };
			}
		}

		protected override void OnRestoreInstanceState(Bundle savedInstanceState)
		{
			try
			{
				base.OnRestoreInstanceState(savedInstanceState);
			}
			catch (Exception ex)
			{
				if (App.debugging) { throw; };
			}
		}
	}
}
