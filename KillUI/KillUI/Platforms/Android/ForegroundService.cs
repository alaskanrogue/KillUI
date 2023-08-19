using System;
using System.Threading;
using Android.App;
using Android.Content;
using AndroidX.Core.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Java.Lang;
using Android.Graphics.Drawables;

namespace KillUI
{
	[Service]
	public class ForegroundService : Service
	{
		static readonly string TAG = typeof(ForegroundService).FullName;

		static readonly string CHANNEL_ID = "Killnotifications";

		internal bool isStarted;
		internal Action runnable;

		public override void OnCreate()
		{
			base.OnCreate();

			runnable = new Action(() =>
			{
				App killui = new App();
			});
		}

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			if (intent.Action == "START_SERVICE")
			{
				if (isStarted)
				{

				}
				else
				{
					RegisterForegroundService();
					isStarted = true;
				}
			}
			else if (intent.Action == "STOP_SERVICE")
			{
				StopForeground(StopForegroundFlags.Detach);
				StopSelf();
				isStarted = false;
			}
			else if (intent.Action.Equals("RESTART_SERVICE"))
			{

			}

			// This tells Android to start the service if it is killed to reclaim resources.
			return StartCommandResult.Sticky;
		}


		public override IBinder OnBind(Intent intent)
		{
			// Return null because this is a pure started service. A hybrid service would return a binder that would
			// allow access to the GetFormattedStamp() method.
			return null;
		}


		public override void OnDestroy()
		{
			// Remove the notification from the status bar.
			var notificationManager = (NotificationManager)GetSystemService(NotificationService);
			notificationManager.Cancel(Constants.NOTIFICATION_CHANNEL_ID);

			isStarted = false;
			base.OnDestroy();
		}

		void RegisterForegroundService()
		{
			if (Build.VERSION.SdkInt < BuildVersionCodes.O)
			{
				// Notification channels are new in API 26 (and not a part of the
				// support library). There is no need to create a notification
				// channel on older versions of Android.
				return;
			}

			NotificationChannel channel = new NotificationChannel("98765", "KillUI", NotificationImportance.Max)
			{
				Description = "UI Management"
			};

			NotificationManager manager = (NotificationManager)MainActivity.ActivityCurrent.GetSystemService(Context.NotificationService);
			manager.CreateNotificationChannel(channel);

			Notification notification = new Notification.Builder(MainActivity.ActivityCurrent, "98765")
				.SetContentTitle("UI Management")
				.SetSmallIcon(Resource.Drawable.icon)
				.SetContentIntent(BuildIntentToShowMainActivity())
				.SetOngoing(true)
				.SetAutoCancel(false)
				.AddAction(BuildRestartForegroundServiceAction())
				.Build();

			notification.Visibility = NotificationVisibility.Public;

			MainActivity.notification = notification;

			// Enlist this instance of the service as a foreground service
			StartForeground(Constants.NOTIFICATION_CHANNEL_ID, notification);
		}

		/// <summary>
		/// Builds a PendingIntent that will display the main activity of the app. This is used when the 
		/// user taps on the notification; it will take them to the main activity of the app.
		/// </summary>
		/// <returns>The content intent.</returns>
		PendingIntent BuildIntentToShowMainActivity()
		{
			var notificationIntent = new Intent(MainActivity.ActivityCurrent, typeof(MainActivity));
			notificationIntent.SetAction("MAIN_ACTIVITY");
			notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask | ActivityFlags.ReceiverForeground);
			notificationIntent.PutExtra(Constants.SERVICE_STARTED_KEY, true);
			notificationIntent.SetClassName(Context.NotificationService, "Requests");

			MainActivity.ActivityCurrent.Intent = notificationIntent;

			var pendingIntent = PendingIntent.GetActivity(MainActivity.ActivityCurrent, 0, notificationIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
			return pendingIntent;
		}

		/// <summary>
		/// Builds a Notification.Action that will instruct the service to restart the timer.
		/// </summary>
		/// <returns>The restart timer action.</returns>
		Notification.Action BuildRestartForegroundServiceAction()
		{
			var restartServicesIntent = new Intent(MainActivity.ActivityCurrent, GetType());
			restartServicesIntent.SetAction("RESTART_SERVICE");
			restartServicesIntent.SetClassName(Context.NotificationService, "RestartRequests");
			var restartServicesPendingIntent = PendingIntent.GetService(MainActivity.ActivityCurrent, 0, restartServicesIntent, PendingIntentFlags.Immutable);

			ICharSequence reboot = new Java.Lang.String("Reboot");

			var builder = new Notification.Action.Builder(Resource.Drawable.icon,
											  reboot,
											  restartServicesPendingIntent);

			return builder.Build();
		}

		/// <summary>
		/// Builds the Notification.Action that will allow the user to stop the service via the
		/// notification in the status bar
		/// </summary>
		/// <returns>The stop service action.</returns>
		Notification.Action BuildStopForegroundServiceAction()
		{
			var stopServiceIntent = new Intent(MainActivity.ActivityCurrent, GetType());
			stopServiceIntent.SetClassName(Context.NotificationService, "StopRequests");
			stopServiceIntent.SetAction("STOP_SERVICE");
			var stopServicePendingIntent = PendingIntent.GetService(MainActivity.ActivityCurrent, 0, stopServiceIntent, PendingIntentFlags.Immutable);

			var builder = new Notification.Action.Builder(Android.Resource.Drawable.IcMediaPause,
														  "Stopping",
														  stopServicePendingIntent);
			return builder.Build();
		}
	}
}
