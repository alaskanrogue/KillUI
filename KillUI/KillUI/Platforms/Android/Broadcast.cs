using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;

namespace KillUI
{
	internal class Broadcast : BroadcastReceiver
	{
		internal static Intent serviceIntent;
		internal static Intent startServiceIntent;

		public override void OnReceive(Context context, Intent intent)
		{
			if (intent == null)
			{
				return;
			}

			startServiceIntent = new Intent(context, typeof(ForegroundService));
			startServiceIntent.SetAction("START_SERVICE");

			context.StartService(MainActivity.startServiceIntent);
			context.StartForegroundService(startServiceIntent);
		}
	}
}
