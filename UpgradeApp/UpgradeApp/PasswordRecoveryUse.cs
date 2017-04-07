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

			SetContentView(Resource.Layout.PasswordRecoveryUseScreen);

			ImageView upgradeLogo = FindViewById<ImageView>(Resource.Id.upgradeLogo);
            TextView recoveryQuestion = FindViewById<TextView>(Resource.Id.RecoveryQuestion);
            EditText recoveryAnswer = FindViewById<EditText>(Resource.Id.recoverAnswer);
            Button submitButton = FindViewById<Button>(Resource.Id.submitButton);
            EditText email = FindViewById<EditText>(Resource.Id.email);
            Button submitEmailButton = FindViewById<Button>(Resource.Id.submitEmailButton);

            //Add some HTTP handler to get the question and set the TextView recvoeryQuestion 
            
			//recoveryQuestion.SetText(some string)

            

			submitButton.Enabled = false;
			recoveryAnswer.Enabled = false;
			recoveryQuestion.Enabled = false;

			

			submitButton.Click += (object sender, EventArgs e) =>
            {
				// Talk to server and return to main page
				HTTPHandler.checkRecoveryAnswer(email.Text, recoveryAnswer.Text);
                var intent = new Android.Content.Intent(this, typeof(MainActivity));
                StartActivity(intent);
            };

			submitEmailButton.Click += (object sender, EventArgs e) => {
				// Update w/ question
				email.Enabled = false;
				submitEmailButton.Enabled = false;
				submitButton.Enabled = true;
				recoveryQuestion.Enabled = true;
				recoveryAnswer.Enabled = true;
				Toast toast = Toast.MakeText(this, "Retrieving question...", ToastLength.Short);
				toast.Show();
				Question q = HTTPHandler.getRecoveryQuestion(email.Text);
				recoveryQuestion.Text = q.question;
			};
		}
    }
}