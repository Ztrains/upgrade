using Android.App;
using Android.Widget;
using Android.OS;
using System.Net.Http;

namespace UpgradeApp {
	[Activity(Label = "UpgradeApp", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity {
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);  // Change Main to Login screen //

			// Code for testing purposes
			HTTPHandler.Testfn();

		}
	}
}

