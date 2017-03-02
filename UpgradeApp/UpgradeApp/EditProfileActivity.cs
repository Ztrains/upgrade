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
			EditText nameEditText = FindViewById<EditText>(Resource.Id.NameEditText);
			TextView contactLabelTextView = FindViewById<TextView>(Resource.Id.ContactLabelTextView);
			EditText emailEditText = FindViewById<EditText>(Resource.Id.EmailEditText);
			EditText otherContactMethodsEditText = FindViewById<EditText>(Resource.Id.OtherContactMethodsEditText);
			TextView iTutorLabelTextView = FindViewById<TextView>(Resource.Id.ITutorLabelTextView);
			TextView iWantToStudyLabelTextView = FindViewById<TextView>(Resource.Id.IWantToStudyTextView);
			TextView freeTimeLabelTextView = FindViewById<TextView>(Resource.Id.FreeTimeLabelTextView);
			EditText freeTimeTextView = FindViewById<EditText>(Resource.Id.FreeTimeEditText);
			TextView pricesLabelTextView = FindViewById<TextView>(Resource.Id.PricesLabelTextView);
			EditText pricesEditText = FindViewById<EditText>(Resource.Id.PricesEditText);
			Button submitButton = FindViewById<Button>(Resource.Id.SubmitButton);



		}
	}
}