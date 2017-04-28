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

			// Use recovery screen layout
			SetContentView(Resource.Layout.PasswordRecoveryUseScreen);
			// Create variables for screen objects for usage below
			ImageView upgradeLogo = FindViewById<ImageView>(Resource.Id.upgradeLogo);
            TextView recoveryQuestion = FindViewById<TextView>(Resource.Id.RecoveryQuestion);
            EditText recoveryAnswer = FindViewById<EditText>(Resource.Id.recoverAnswer);
            Button submitButton = FindViewById<Button>(Resource.Id.submitButton);
            EditText email = FindViewById<EditText>(Resource.Id.email);
            Button submitEmailButton = FindViewById<Button>(Resource.Id.submitEmailButton);
			EditText recoveryPassEditText = FindViewById<EditText>(Resource.Id.recoveryPassEditText);
			Button recoveryPassButton = FindViewById<Button>(Resource.Id.recoveryPassButton);
			// Set initial states of screen objects
			submitButton.Enabled = false;
			recoveryAnswer.Enabled = false;
			recoveryQuestion.Enabled = false;
			recoveryPassEditText.Enabled = false;
			recoveryPassButton.Enabled = false;

			// When submit username button is pressed, update the screen objects
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

			// When the submit answer button is pressed, update the screen objects
			submitButton.Click += (object sender, EventArgs e) =>
            {
				string status = HTTPHandler.checkRecoveryAnswer(email.Text, recoveryAnswer.Text);
				// If successful, enable the next objects
				if (status.Equals("Password change successful")) {
					Toast toast = Toast.MakeText(this, "Correct.  Please reset your password.", ToastLength.Short);
					toast.Show();
					recoveryPassEditText.Enabled = true;
					recoveryPassButton.Enabled = true;
					recoveryAnswer.Enabled = false;
					submitButton.Enabled = false;
				}
				else { // Otherwise display an incorrect message
					Toast toast = Toast.MakeText(this, "Incorrect.", ToastLength.Short);
					toast.Show();
				}
            };

			// When the recover password button has been pressed, change the server password to the new one provided
			recoveryPassButton.Click += (object sender, EventArgs e) => {
				Toast toast = Toast.MakeText(this, "Password has been reset.", ToastLength.Short);
				toast.Show();
				HTTPHandler.updatePassword(null, recoveryPassEditText.Text, "true", email.Text);
				// Then return to the login page
				var intent = new Android.Content.Intent(this, typeof(MainActivity));
				StartActivity(intent);
			};


		}
    }
}