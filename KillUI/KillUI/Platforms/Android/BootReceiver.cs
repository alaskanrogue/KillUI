using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KillUI
{
	internal class BootReceiver : BroadcastReceiver 
	{
		public void onReceive(Context context, Intent intent)
		{
			if (intent != null)
			{
				if ((intent.Action == "ACTION_BOOT_COMPLETED") || (intent.Action == "ACTION_REBOOT_COMPLETED"))
				{
					new KillUI.App();
				}
			}
		}

		public override void OnReceive(Context context, Intent intent)
		{
			throw new NotImplementedException();
		}
	}
}
