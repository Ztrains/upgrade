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
            SetContentView(Resource.Layout.PasswordRecoverySet);

            EditText question = FindViewById<EditText>(Resource.Id.recoveryQuestion);
			EditText answer = FindViewById<EditText>(Resource.Id.recoveryAnswer);
            Button submit = FindViewById<Button>(Resource.Id.submitButton);

            submit.Click += (object sender, EventArgs e) =>
            {
				// Ask server to send recovery email
				
				// Return to login page
				var intent = new Android.Content.Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
        }
    }
}