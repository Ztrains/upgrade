using Android.App;
using Android.Widget;
using Android.OS;
using System.Net.Http;
using System;

namespace UpgradeApp {
	[Activity(Label = "UpgradeApp", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity {
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Login);  // Change Main to Login screen //

			EditText email = FindViewById<EditText>(Resource.Id.email);
			EditText password = FindViewById<EditText>(Resource.Id.password);
			ImageView upgradeLogo = FindViewById<ImageView>(Resource.Id.upgradeLogo);
			Button loginButton = FindViewById<Button>(Resource.Id.loginButton);
			Button newAccountButton = FindViewById<Button>(Resource.Id.createAccountButton);

			// Code for testing purposes
			//HTTPHandler.Testfn();

			loginButton.Click += (object sender, EventArgs e) => {
				int success = HTTPHandler.loginRequest(email.Text, password.Text);
			};

			newAccountButton.Click += (sender, e) => {
				var intent = new Android.Content.Intent(this, typeof(AccountCreationActivity));
				StartActivity(intent);
			};

		}
	}
}

