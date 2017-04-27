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
using Android.Graphics;
using System.Threading.Tasks;
using System.Net.Http;
using FFImageLoading;
using Square.Picasso;

namespace UpgradeApp {
	[Activity(Label = "ProfileActivity")]
	public class ProfileActivity : Activity {

		bool justLoggedIn;
        bool admin = false;
		

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			//SetTheme(Android.Resource.Style.ThemeHoloLightNoActionBar);
			SetContentView(Resource.Layout.ProfileScreen);
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

			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);

			InvalidateOptionsMenu();

			// Get profile information from the server
			Profile p;
			if (Intent.GetStringExtra("studentName") != null)
				p = HTTPHandler.getProfileByName(Intent.GetStringExtra("studentName"));
			else {
				p = HTTPHandler.getProfile(Intent.GetStringExtra("email"));
			}
			//nameTextView.Text = p.name;
			emailTextView.Text = p.email;
			ratingTextView.Text = p.rating.ToString();
			aboutTextView.Text = p.about;
            ActionBar.Title = p.name;

            iTutorTextView.Text = "";
			iNeedATutorTextView.Text = "";
            if (p.classesIn != null)
            {
                foreach (classInfo ci in p.classesIn)
                {
                    if (ci.type == "tutor")
                    {
                        iTutorTextView.Text += ci.className;
                        iTutorTextView.Text += " ";
                    }
                    else if (ci.type == "student")
                    {
                        iNeedATutorTextView.Text += ci.className;
                        iNeedATutorTextView.Text += " ";
                    }
                }
            }

			Picasso.With(this).Load(p.avatar).Into(avatarImageView);

			availabilityTextView.Text = p.time;
			pricesTextView.Text = p.price;

			if (emailTextView.Text.Equals(HTTPHandler.emailLoggedIn)) {
				sendMessageButton.Enabled = false;
				reportButton.Enabled = false;
				blockButton.Enabled = false;
				editButton.Enabled = true;
				rateButton.Enabled = false;
			}
			else if (p.admin != null && p.admin.Equals("true")) {
				sendMessageButton.Enabled = true;
				reportButton.Enabled = true;
				blockButton.Enabled = true;
				editButton.Enabled = true;
                admin = true;
			}
			else {
				sendMessageButton.Enabled = true;
				reportButton.Enabled = true;
				blockButton.Enabled = true;
				editButton.Enabled = false;
			}

			string uid = HTTPHandler.getProfile(HTTPHandler.emailLoggedIn)._id;
			if (p.usersUpvoted != null) {
				foreach (upvotedID u in p.usersUpvoted) {
					if (u._id.Equals(uid))
						rateButton.Enabled = false;
				}
			}

			if (Intent.GetStringExtra("justLoggedIn") != null && Intent.GetStringExtra("justLoggedIn").Equals("true"))
				justLoggedIn = true;
			else justLoggedIn = false;

			bool isBlocked = false;
			blockedID[] bids = HTTPHandler.getProfile(HTTPHandler.emailLoggedIn).blockedUsers;
			if (bids != null) {
				foreach (blockedID bid in bids) {
					if (p._id == bid.id)
						isBlocked = true;
				}
			}
			if (isBlocked) blockButton.Text = "Unblock";


			classListView.Click += (object Sender, EventArgs e) => {
				var intent = new Android.Content.Intent(this, typeof(ClassListActivity));
				StartActivity(intent);
			};

			editButton.Click += (Sender, e) => {
				var intent = new Android.Content.Intent(this, typeof(EditProfileActivity));

				intent.PutExtra("name", ActionBar.Title);
				//intent.PutExtra("email", emailTextView.Text);
				intent.PutExtra("contact", contactMethodsTextView.Text);
				intent.PutExtra("about", aboutTextView.Text);
				intent.PutExtra("freeTime", availabilityTextView.Text);
				intent.PutExtra("prices", pricesTextView.Text);
				intent.PutExtra("avatar", p.avatar);
				//intent.PutExtra("studyClasses", iTutorTextView.Text);
				//intent.PutExtra("tutorClasses", iNeedATutorTextView.Text);
				StartActivity(intent);

			};

			rateButton.Click += (Sender, e) => {
				HTTPHandler.upvoteProfile(ActionBar.Title);
				Toast toast = Toast.MakeText(this, "Thanks for your input!", ToastLength.Short);
				toast.Show();
				rateButton.Enabled = false;
			};

			reportButton.Click += (Sender, e) => {
				HTTPHandler.reportProfile(p._id, p.name, "Reported.");
				Toast toast = Toast.MakeText(this, "User has been reported.", ToastLength.Short);
				toast.Show();

			};

			blockButton.Click += (Sender, e) => {
				if (isBlocked) {
					HTTPHandler.unblockProfile(p._id);
					Toast toast = Toast.MakeText(this, "User has been unblocked.", ToastLength.Short);
					toast.Show();
					blockButton.Text = "Block";
					isBlocked = false;
				} else {
					HTTPHandler.blockProfile(p._id);
					Toast toast = Toast.MakeText(this, "User has been blocked.", ToastLength.Short);
					toast.Show();
					blockButton.Text = "Unblock";
					isBlocked = true;
				}
			};

			sendMessageButton.Click += (Sender, e) => {
				GetChatID cid = HTTPHandler.startAChat(p._id);
				var intent = new Android.Content.Intent(this, typeof(messagingActivity));
				intent.PutExtra("cid", cid._id);
				intent.PutExtra("uid", p._id);
                intent.PutExtra("name", ActionBar.Title);
				StartActivity(intent);
			};
		}

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.topmenus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

		public override bool OnPrepareOptionsMenu(IMenu menu) {
			//MenuInflater.Inflate(Resource.Menu.topmenus, menu);
			//if (!admin) {
			//	menu.RemoveItem(menu.GetItem(0).ItemId);
			//}
			//return true;
			return base.OnCreateOptionsMenu(menu);
		}


		public override bool OnOptionsItemSelected(IMenuItem item) //Passed in the menu item that was selected
        {
			if (item.TitleFormatted.ToString().Equals("Admin Options")) {
				if (admin) {
					var intent = new Intent(this, typeof(AdminActivity));
					StartActivity(intent);
				}
			}
			else if (item.TitleFormatted.ToString().Equals("Logout")) {
				var intent = new Intent(this, typeof(MainActivity));
				HTTPHandler.emailLoggedIn = "";
				StartActivity(intent);
			}
			
            
            return base.OnOptionsItemSelected(item);
        }
        // Disables the back button on this page
        public override void OnBackPressed() {
			if (justLoggedIn == false)
				base.OnBackPressed();
		}

	}
}