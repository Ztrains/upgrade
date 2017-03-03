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
	[Activity(Label = "ProfileActivity")]
	public class ProfileActivity : Activity {
		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
            //SetTheme(Android.Resource.Style.ThemeHoloLightNoActionBar);
            SetContentView(Resource.Layout.ProfileScreen);

			TextView nameTextView = FindViewById<TextView>(Resource.Id.NameTextView);
			ImageView avatarImageView = FindViewById<ImageView>(Resource.Id.AvatarImageView);
			Button sendMessageButton = FindViewById<Button>(Resource.Id.SendMessageButton);
			TextView contactMethodsLabelTextView = FindViewById<TextView>(Resource.Id.ContactMethodsLabelTextView);
			TextView contactMethodsTextView = FindViewById<TextView>(Resource.Id.ContactMethodsTextView);
			Button editButton = FindViewById<Button>(Resource.Id.EditButton);
			Button blockButton = FindViewById<Button>(Resource.Id.BlockButton);
			Button reportButton = FindViewById<Button>(Resource.Id.ReportButton);
			TextView aboutLabelTextView = FindViewById<TextView>(Resource.Id.AboutLabelTextView);
			TextView aboutTextView = FindViewById<TextView>(Resource.Id.AboutTextView);
			TextView iTutorLabelTextView = FindViewById<TextView>(Resource.Id.ITutorLabelTextView);
			TextView iTutorTextView = FindViewById<TextView>(Resource.Id.ITutorTextView);
			TextView iNeedATutorLabelTextView = FindViewById<TextView>(Resource.Id.INeedATutorLabelTextView);
			TextView iNeedATutorTextView = FindViewById<TextView>(Resource.Id.INeedATutorTextView);
			TextView availabilityLabelTextView = FindViewById<TextView>(Resource.Id.AvailabilityLabelTextView);
			TextView availabilityTextView = FindViewById<TextView>(Resource.Id.AvailabilityTextView);
			TextView pricesLabelTextView = FindViewById<TextView>(Resource.Id.PricesLabelTextView);
			TextView pricesTextView = FindViewById<TextView>(Resource.Id.PricesTextView);
            Button classListView = FindViewById<Button>(Resource.Id.classButton);

            classListView.Click += (object Sender, EventArgs e) =>
            {
                var intent = new Android.Content.Intent(this, typeof(ClassList));
                StartActivity(intent);
            };
			

			


		}
	}
}