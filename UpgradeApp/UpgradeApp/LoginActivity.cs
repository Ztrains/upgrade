using Android.App;
using Android.Widget;
using Android.OS;

namespace UpgradeApp {
	[Activity(Label = "UpgradeApp", MainLauncher = true, Icon = "@drawable/icon")]
	public class LoginActivity : Activity {
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Login);  // Change Main to Login screen //

            EditText username = FindViewById<EditText>(Resource.Id.username);
            EditText password = FindViewById<EditText>(Resource.Id.password);
            ImageView upgradeLogo = FindViewById<ImageView>(Resource.Id.upgradeLogo);
            Button loginButton = FindViewById<Button>(Resource.Id.loginButton);

		}
	}
}

