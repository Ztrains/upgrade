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

			// Create your application here
			SetContentView(Resource.Layout.EditProfileScreen);

			TextView nameLabelTextView = FindViewById<TextView>(Resource.Id.NameLabelTextView);
			TextView nameTextView = FindViewById<TextView>(Resource.Id.NameTextView);
			TextView contactLabelTextView = FindViewById<TextView>(Resource.Id.ContactLabelTextView);
			TextView emailTextView = FindViewById<TextView>(Resource.Id.EmailTextView);
			TextView otherContactMethodsTextView = FindViewById<TextView>(Resource.Id.OtherContactMethodsTextView);
			TextView iTutorLabelTextView = FindViewById<TextView>(Resource.Id.ITutorLabelTextView);
			TextView iWantToStudyLabelTextView = FindViewById<TextView>(Resource.Id.IWantToStudyTextView);
			TextView freeTimeLabelTextView = FindViewById<TextView>(Resource.Id.FreeTimeLabelTextView);
			TextView freeTimeTextView = FindViewById<TextView>(Resource.Id.FreeTimeTextView);
			Button submitButton = FindViewById<Button>(Resource.Id.SubmitButton);



		}
	}
}