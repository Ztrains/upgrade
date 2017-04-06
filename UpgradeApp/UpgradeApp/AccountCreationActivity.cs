using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Net.Http;

namespace UpgradeApp
{
    [Activity(Label = "AccountCreation")]
    public class AccountCreationActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetTheme(Android.Resource.Style.ThemeHoloLightNoActionBar);
            SetContentView(Resource.Layout.AccountCreationScreen);

            ImageView upgradeLogo = FindViewById<ImageView>(Resource.Id.upgradeLogo);
            EditText userEmail = FindViewById<EditText>(Resource.Id.email);
            EditText userPassword = FindViewById<EditText>(Resource.Id.password);
            Button subButton = FindViewById<Button>(Resource.Id.submitButton);

            subButton.Click += (object sender, EventArgs e) => {
				
				Toast toast = Toast.MakeText(this, "Attempting registration...", ToastLength.Short);
				toast.Show();
				// Send information to the server
				int status = HTTPHandler.registerRequest(userEmail.Text, userPassword.Text);
				if (status == 1) {
					toast = Toast.MakeText(this, "Registration successful!", ToastLength.Short);
					toast.Show();
					// Now login to new account
					status = HTTPHandler.loginRequest(userEmail.Text, userPassword.Text);
					// Change screen
					var intent = new Android.Content.Intent(this, typeof(EditProfileActivity));
					StartActivity(intent);
				}
				else if (status == -2) {
					toast = Toast.MakeText(this, "Database error.", ToastLength.Short);
					toast.Show();
				}
				else if (status == -3) {
					toast = Toast.MakeText(this, "Account already exists with that email.", ToastLength.Short);
					toast.Show();
				}
				else if (status == -1) {
					toast = Toast.MakeText(this, "Unknown error.", ToastLength.Short);
					toast.Show();
				}
				
            };

        }
    }
}