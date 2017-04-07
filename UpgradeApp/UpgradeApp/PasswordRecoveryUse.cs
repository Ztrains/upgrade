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
    public class PasswordRecoveryUse : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            ImageView upgradeLogo = FindViewById<ImageView>(Resource.Id.upgradeLogo);
            TextView recoveryQuestion = FindViewById<TextView>(Resource.Id.RecoveryQuestion);
            EditText recoveryAnswer = FindViewById<EditText>(Resource.Id.recoverAnswer);
            Button submitButton = FindViewById<Button>(Resource.Id.submitButton);
            EditText email = FindViewById<EditText>(Resource.Id.email);

            //Add some HTTP handler to get the question and set the TextView recvoeryQuestion 
            
			//recoveryQuestion.SetText(some string)

            SetContentView(Resource.Layout.PasswordRecoveryUseScreen);


            submitButton.Click += (object sender, EventArgs e) =>
            {
                 
                // Return to login page
                var intent = new Android.Content.Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };
        }
    }
}