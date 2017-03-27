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
			TextView emailTextView = FindViewById<TextView>(Resource.Id.EmailTextView);
			TextView contactMethodsTextView = FindViewById<TextView>(Resource.Id.ContactMethodsTextView);
			Button editButton = FindViewById<Button>(Resource.Id.EditButton);
			Button blockButton = FindViewById<Button>(Resource.Id.BlockButton);
			Button reportButton = FindViewById<Button>(Resource.Id.ReportButton);
			TextView ratingTextView = FindViewById<TextView>(Resource.Id.RatingTextView);
			Button rateButton = FindViewById<Button>(Resource.Id.RateButton);
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

            if(Intent.GetStringExtra("name") != null)
            {
                nameTextView.Text = Intent.GetStringExtra("name");
            }
			if (Intent.GetStringExtra("email") != null) {
				emailTextView.Text = Intent.GetStringExtra("email");
			}
				
			if (Intent.GetStringExtra("contact") != null)
            {
                contactMethodsTextView.Text = Intent.GetStringExtra("contact");
            }
			if (Intent.GetStringExtra("about") != null) {
				aboutTextView.Text = Intent.GetStringExtra("about");
			}
			if (Intent.GetStringExtra("freeTime") != null)
            {
                availabilityTextView.Text = Intent.GetStringExtra("freeTime");
            }
            if (Intent.GetStringExtra("prices") != null)
            {
                pricesTextView.Text = Intent.GetStringExtra("prices");
            }


            classListView.Click += (object Sender, EventArgs e) =>
            {
                var intent = new Android.Content.Intent(this, typeof(ClassList));
                StartActivity(intent);
            };

            editButton.Click += (Sender, e) =>
            {
                var intent = new Android.Content.Intent(this, typeof(EditProfileActivity));

				intent.PutExtra("name", nameTextView.Text);
				intent.PutExtra("email", emailTextView.Text);
				intent.PutExtra("contact", contactMethodsTextView.Text);
				intent.PutExtra("about", aboutTextView.Text);
				intent.PutExtra("freeTime", availabilityTextView.Text);
				intent.PutExtra("prices", pricesTextView.Text);
				StartActivity(intent);

            };

			rateButton.Click += (Sender, e) => {
				// Implement rate button clicking here
			};

			


		}
	}
}