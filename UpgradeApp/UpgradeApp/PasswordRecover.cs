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

namespace UpgradeApp
{
    [Activity(Label = "Activity1")]
    public class PasswordRecover : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PasswordRecovery);

            EditText userEmail = FindViewById<EditText>(Resource.Id.userEmail);
            Button submit = FindViewById<Button>(Resource.Id.submitButton);

            submit.Click += (object sender, EventArgs e) =>
            {
				// Ask server to send recovery email
				HTTPHandler.recoverPassword(userEmail.Text);
				// Return to login page
				var intent = new Android.Content.Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
        }
    }
}