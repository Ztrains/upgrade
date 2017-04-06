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
				// Send information to the server
				HTTPHandler.registerRequest(userEmail.Text, userPassword.Text);
				// Change screen
                var intent = new Android.Content.Intent(this, typeof(EditProfileActivity));
                StartActivity(intent);
            };

        }
    }
}