using Android.App;
using Android.Widget;
using Android.OS;
using System.Net.Http;
using System;

using Stormpath.SDK;
using Stormpath.SDK.Client;
using Stormpath.SDK.Error;

namespace UpgradeApp {
	[Activity(Label = "UpgradeApp", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity {

		/*public async void stormPathMain() {
			var client = Clients.Builder()
				.SetApiKeyFilePath("./keys.txt")
				.Build();
			var myApp = await client.GetApplications()
				.Where(x => x.Name == "My Application")
				.SingleAsync();
		}*/

		protected override void OnCreate(Bundle bundle) {
            //SetTheme(Android.Resource.Style.ThemeHoloLightNoActionBar);
            base.OnCreate(bundle);

			//stormPathMain();

			//HTTPHandler.launchStormPath();
			//HTTPHandler.classListRequest();


			//Console.WriteLine("test");

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Login);  // Change Main to Login screen //
           

            EditText email = FindViewById<EditText>(Resource.Id.email);
			EditText password = FindViewById<EditText>(Resource.Id.password);
			ImageView upgradeLogo = FindViewById<ImageView>(Resource.Id.upgradeLogo);
			Button loginButton = FindViewById<Button>(Resource.Id.loginButton);
			Button newAccountButton = FindViewById<Button>(Resource.Id.createAccountButton);
            Button recoverAccountButton = FindViewById<Button>(Resource.Id.recoverButton);
            EditText firstName = FindViewById<EditText>(Resource.Id.firstName);
            EditText lastName = FindViewById<EditText>(Resource.Id.lastName);

			// Code for testing purposes
			//HTTPHandler.Testfn();

			loginButton.Click += (object sender, EventArgs e) => {
				Toast toast = Toast.MakeText(this, "Attempting login...", ToastLength.Short);
				toast.Show();
				// Send information to the server
				int status = HTTPHandler.loginRequest(email.Text, password.Text);
				//HTTPHandler.testLogin();

				if (status == 1) {
					toast = Toast.MakeText(this, "Login successful", ToastLength.Short);
					toast.Show();
					var intent = new Android.Content.Intent(this, typeof(ProfileActivity));
					intent.PutExtra("email", email.Text);
					intent.PutExtra("justLoggedIn", "true");
					HTTPHandler.emailLoggedIn = email.Text;
					StartActivity(intent);
				}
				else if (status == -2) {
					toast = Toast.MakeText(this, "Email/Password incorrect.  Try again.", ToastLength.Short);
					toast.Show();
				}
				else if (status == -1) {
					toast = Toast.MakeText(this, "Unknown error.", ToastLength.Short);
					toast.Show();
				}
				/*
                if (!email.Text.Equals("myers259@purdue.edu") || !password.Text.Equals("Thegm_97"))
                {
                    Toast toast = Toast.MakeText(this, "Invalid Username or Password", ToastLength.Short);
                    toast.Show();
                }
                else
                {
                    var intent = new Android.Content.Intent(this, typeof(ProfileActivity));
                    StartActivity(intent);
                }
				*/
            };

			newAccountButton.Click += (sender, e) => {
				var intent = new Android.Content.Intent(this, typeof(AccountCreationActivity));
				StartActivity(intent);
			};

            recoverAccountButton.Click += (sender, e) =>
            {
                var intent = new Android.Content.Intent(this, typeof(PasswordRecoveryUse));
                StartActivity(intent);
            };

		}
	}
}

