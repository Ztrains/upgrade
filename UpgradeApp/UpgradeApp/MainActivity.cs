using Android.App;
using Android.Widget;
using Android.OS;
using System.Net.Http;
using System;

using Stormpath.SDK;
using Stormpath.SDK.Client;
using Stormpath.SDK.Error;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Android.Gms.Common;

namespace UpgradeApp {
	[Activity(Label = "UpgradeApp", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity {

		const string TAG = "MainActivity";

		public bool IsPlayServicesAvailable() {
			string t = "";
			int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
			if (resultCode != ConnectionResult.Success) {
				if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
					t = GoogleApiAvailability.Instance.GetErrorString(resultCode);
				else {
					Toast toast = Toast.MakeText(this, "device not supported", ToastLength.Short);
					toast.Show();
					//msgText.Text = "This device is not supported";
					Finish();
				}
				return false;
			}
			else {
				//msgText.Text = "Google Play Services is available.";
				Toast toast = Toast.MakeText(this, "Google Play Services available", ToastLength.Short);
				toast.Show();
				
				return true;
			}
		}


		protected override void OnCreate(Bundle bundle) {
			//SetTheme(Android.Resource.Style.ThemeHoloLightNoActionBar);
			base.OnCreate(bundle);

			//stormPathMain();

			//HTTPHandler.launchStormPath();
			//HTTPHandler.classListRequest();


			//Console.WriteLine("test");

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Login);  // Change Main to Login screen //

			// Check if play services is working
			IsPlayServicesAvailable();
			Firebase.FirebaseApp.InitializeApp(this);
			//HTTPHandler.registerDevice(FirebaseInstanceId.Instance.Token);
			//Log.Debug(TAG, "google app id: " + GetString(Resource.String.google_app_id));

			if (Intent.Extras != null) {
				foreach (var key in Intent.Extras.KeySet()) {
					var value = Intent.Extras.GetString(key);
					Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
				}
			}




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

		// Disables the back button on this page
		public override void OnBackPressed() {
			// disabled
		}
	}
}

