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

namespace UpgradeApp {
	[Activity(Label = "EditProfileActivity")]
	public class EditProfileActivity : Activity {

		bool justLoggedIn;
		string newName = "";
		string newEmail = "";
		string newContact = "";
		string newAbout = "";
		string freeTime = "";
		string prices = "";
		string visible = ""; // TODO
		string avatar = "";
		string emaill = "";

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
           // SetTheme(Android.Resource.Style.ThemeHoloLightNoActionBar);
            // Use profile edit layout
            SetContentView(Resource.Layout.EditProfileScreen);

			// Create variables to access screen objects
			TextView nameLabelTextView = FindViewById<TextView>(Resource.Id.NameLabelTextView);
			EditText nameEditText = FindViewById<EditText>(Resource.Id.NameEditText);
			EditText otherContactMethodsEditText = FindViewById<EditText>(Resource.Id.OtherContactMethodsEditText);
			EditText aboutEditText = FindViewById<EditText>(Resource.Id.AboutEditText);
			EditText freeTimeTextView = FindViewById<EditText>(Resource.Id.FreeTimeEditText);
			EditText pricesEditText = FindViewById<EditText>(Resource.Id.PricesEditText);
			Button submitButton = FindViewById<Button>(Resource.Id.SubmitButton);
            Button studyButton = FindViewById<Button>(Resource.Id.studyButton);
            Button tutorButton = FindViewById<Button>(Resource.Id.tutorButton);
			EditText avatarEditText = FindViewById<EditText>(Resource.Id.avatarEditText);
			EditText passEditText = FindViewById<EditText>(Resource.Id.passEditText);
			EditText newPassEditText = FindViewById<EditText>(Resource.Id.newPassEditText);
			Button changePasswordButton = FindViewById<Button>(Resource.Id.updatePasswordButton);

			//string studyClasses = "";
			//string tutorClasses = "";

			

			// Check if just logged in
			if (Intent.GetStringExtra("justLoggedIn") != null)
				justLoggedIn = true;
			else justLoggedIn = false;
			
			// Populate existing fields from profile screen info
			if (Intent.GetStringExtra("name") != null) {
				nameEditText.Text = Intent.GetStringExtra("name");
			}
			if (Intent.GetStringExtra("contact") != null) {
				otherContactMethodsEditText.Text = Intent.GetStringExtra("contact");
			}
			if (Intent.GetStringExtra("email") != null) {
				emaill = Intent.GetStringExtra("email");
			}
			if (Intent.GetStringExtra("about") != null) {
				aboutEditText.Text = Intent.GetStringExtra("about");
			}
			if (Intent.GetStringExtra("freeTime") != null) {
				freeTimeTextView.Text = Intent.GetStringExtra("freeTime");
			}
			if (Intent.GetStringExtra("prices") != null) {
				pricesEditText.Text = Intent.GetStringExtra("prices");
			}
			if (Intent.GetStringExtra("avatar") != null) {
				avatarEditText.Text = Intent.GetStringExtra("avatar");
			}
			/*if (Intent.GetStringExtra("studyClasses") != null) {
				studyClasses = Intent.GetStringExtra("studyClasses");
			}
			if (Intent.GetStringExtra("tutorClasses") != null) {
				tutorClasses = Intent.GetStringExtra("tutorClasses");
			}*/


			// If study button pressed
			studyButton.Click += (sender, e) =>
            {
				string newName = nameEditText.Text;
				//string newEmail = emailEditText.Text;
				string newContact = otherContactMethodsEditText.Text;
				string newAbout = aboutEditText.Text;
				string freeTime = freeTimeTextView.Text;
				string prices = pricesEditText.Text;
				string avatar = avatarEditText.Text;
				//studyClasses = Intent.GetStringExtra("studyClasses");
				//tutorClasses = Intent.GetStringExtra("tutorClasses");

				// Save current fields for return
				var intent = new Android.Content.Intent(this, typeof(classPickerActivity));
                intent.PutExtra("study", "true");
				intent.PutExtra("tutor", "false");

				intent.PutExtra("name", newName);
				intent.PutExtra("studentName", newName);
				intent.PutExtra("email", newEmail);
				intent.PutExtra("contact", newContact);
				intent.PutExtra("about", newAbout);
				intent.PutExtra("freeTime", freeTime);
				intent.PutExtra("prices", prices);
				intent.PutExtra("avatar", avatar);
				//intent.PutExtra("studyClasses", studyClasses);
				//intent.PutExtra("tutorClasses", tutorClasses);

				newName = nameEditText.Text;
				//newEmail = emailEditText.Text;
				newContact = otherContactMethodsEditText.Text;
				newAbout = aboutEditText.Text;
				freeTime = freeTimeTextView.Text;
				prices = pricesEditText.Text;
				visible = ""; // TODO
				avatar = avatarEditText.Text;
				
				// Update the profile with existing information
				HTTPHandler.updateProfile(newName, emaill, newContact, newAbout, null, null, freeTime, prices, null, avatar);
				//HTTPHandler.emailLoggedIn = newEmail;
				// Change pages
				StartActivity(intent);
            };

			// If tutor button pressed
            tutorButton.Click += (sender, e) =>
            {
				string newName = nameEditText.Text;
				//string newEmail = emailEditText.Text;
				string newContact = otherContactMethodsEditText.Text;
				string newAbout = aboutEditText.Text;
				string freeTime = freeTimeTextView.Text;
				string prices = pricesEditText.Text;
				string avatar = avatarEditText.Text;
				//studyClasses = Intent.GetStringExtra("studyClasses");
				//tutorClasses = Intent.GetStringExtra("tutorClasses");

				// Save current fields for return 
				var intent = new Android.Content.Intent(this, typeof(classPickerActivity));
                intent.PutExtra("tutor", "true");
				intent.PutExtra("study", "false");

				intent.PutExtra("name", newName);
				intent.PutExtra("studentName", newName);
				intent.PutExtra("email", newEmail);
				intent.PutExtra("contact", newContact);
				intent.PutExtra("about", newAbout);
				intent.PutExtra("freeTime", freeTime);
				intent.PutExtra("prices", prices);
				intent.PutExtra("avatar", avatar);
				//intent.PutExtra("studyClasses", studyClasses);
				//intent.PutExtra("tutorClasses", tutorClasses);

				newName = nameEditText.Text;
				//newEmail = emailEditText.Text;
				newContact = otherContactMethodsEditText.Text;
				newAbout = aboutEditText.Text;
				freeTime = freeTimeTextView.Text;
				prices = pricesEditText.Text;
				visible = ""; // TODO
				avatar = avatarEditText.Text;

				// Update fields with existing information
				HTTPHandler.updateProfile(newName, emaill, newContact, newAbout, null, null, freeTime, prices, visible, avatar);
				//HTTPHandler.emailLoggedIn = newEmail;
				// Change pages
				StartActivity(intent);
            };

			// If change password button is pressed
			changePasswordButton.Click += (sender, e) => {
				// Update password with server
				string status = HTTPHandler.updatePassword(passEditText.Text, newPassEditText.Text, null, HTTPHandler.emailLoggedIn);
				passEditText.Text = "";
				newPassEditText.Text = "";
				// Confirm or deny attempt based on server response
				if (status.Equals("Password changed")) {
					Toast toast = Toast.MakeText(this, "Password has been changed.", ToastLength.Short);
					toast.Show();
				}
				else if (status.Equals("Wrong original password")) {
					Toast toast = Toast.MakeText(this, "Incorrect password.", ToastLength.Short);
					toast.Show();
				}
				else {
					Toast toast = Toast.MakeText(this, "Error.  Try again.", ToastLength.Short);
					toast.Show();
				}
					// 400 - bad request, missing password
					// 401 - wrong original password
					// 500 - database error
					// "Password changed"
			};


			// If the submit button is pressed
            submitButton.Click += (sender, e) => {
				newName = nameEditText.Text;
				//newEmail = emailEditText.Text;
				newContact = otherContactMethodsEditText.Text;
				newAbout = aboutEditText.Text;
				freeTime = freeTimeTextView.Text;
				prices = pricesEditText.Text;
				visible = ""; // TODO
				avatar = avatarEditText.Text;
				//studyClasses = Intent.GetStringExtra("studyClasses");
				//tutorClasses = Intent.GetStringExtra("tutorClasses");
				// Send server the changes
				// Needs to have other fields fixed

				// Communicate changes to server
				HTTPHandler.updateProfile(newName, emaill, newContact, newAbout, null, null, freeTime, prices, visible, avatar);
				//HTTPHandler.emailLoggedIn = newEmail;

                var intent = new Android.Content.Intent(this, typeof(ProfileActivity));
                //intent.PutExtra("name", newName);
				intent.PutExtra("studentName", newName);
                //intent.PutExtra("email", newEmail);
                //intent.PutExtra("contact", newContact);
				//intent.PutExtra("about", newAbout);
                //intent.PutExtra("freeTime", freeTime);
                //intent.PutExtra("prices", prices);
                //intent.PutExtra("studyClasses", studyClasses);
                //intent.PutExtra("tutorClasses", tutorClasses);
                StartActivity(intent);
            };

		}

		// Disables the back button on this page
		public override void OnBackPressed() {
			if (!justLoggedIn)
				base.OnBackPressed();
		}

	}
}