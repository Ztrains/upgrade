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

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
           // SetTheme(Android.Resource.Style.ThemeHoloLightNoActionBar);
            // Create your application here
            SetContentView(Resource.Layout.EditProfileScreen);

			TextView nameLabelTextView = FindViewById<TextView>(Resource.Id.NameLabelTextView);
			EditText nameEditText = FindViewById<EditText>(Resource.Id.NameEditText);
			TextView contactLabelTextView = FindViewById<TextView>(Resource.Id.ContactLabelTextView);
			EditText emailEditText = FindViewById<EditText>(Resource.Id.EmailEditText);
			EditText otherContactMethodsEditText = FindViewById<EditText>(Resource.Id.OtherContactMethodsEditText);
			TextView aboutLabelTextView = FindViewById<TextView>(Resource.Id.AboutLabelTextView);
			EditText aboutEditText = FindViewById<EditText>(Resource.Id.AboutEditText);
			TextView iTutorLabelTextView = FindViewById<TextView>(Resource.Id.ITutorLabelTextView);
			TextView iWantToStudyLabelTextView = FindViewById<TextView>(Resource.Id.IWantToStudyTextView);
			TextView freeTimeLabelTextView = FindViewById<TextView>(Resource.Id.FreeTimeLabelTextView);
			EditText freeTimeTextView = FindViewById<EditText>(Resource.Id.FreeTimeEditText);
			TextView pricesLabelTextView = FindViewById<TextView>(Resource.Id.PricesLabelTextView);
			EditText pricesEditText = FindViewById<EditText>(Resource.Id.PricesEditText);
			Button submitButton = FindViewById<Button>(Resource.Id.SubmitButton);

			if (Intent.GetStringExtra("name") != null) {
				nameEditText.Text = Intent.GetStringExtra("name");
			}
			if (Intent.GetStringExtra("contact") != null) {
				otherContactMethodsEditText.Text = Intent.GetStringExtra("contact");
			}
			if (Intent.GetStringExtra("email") != null) {
				emailEditText.Text = Intent.GetStringExtra("email");
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






			submitButton.Click += (sender, e) => {
				string newName = nameEditText.Text;
				string newEmail = emailEditText.Text;
				string newContact = otherContactMethodsEditText.Text;
				string newAbout = aboutEditText.Text;
				string freeTime = freeTimeTextView.Text;
				string prices = pricesEditText.Text;
				
				// Send server the changes
				// Needs to have other fields fixed
				HTTPHandler.updateProfile(newName, newEmail, newContact, -5, newAbout, null, null, freeTime, prices);

                var intent = new Android.Content.Intent(this, typeof(ProfileActivity));
                intent.PutExtra("name", newName);
                intent.PutExtra("email", newEmail);
                intent.PutExtra("contact", newContact);
				intent.PutExtra("about", newAbout);
                intent.PutExtra("freeTime", freeTime);
                intent.PutExtra("prices", prices);
                StartActivity(intent);
            };

		}
	}
}