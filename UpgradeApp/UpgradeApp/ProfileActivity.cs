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

		// Activity variables used outside of the create function
		bool justLoggedIn;
        bool admin = false;
		Profile p;
		

		protected override void OnCreate(Bundle savedInstanceState) {
			base.OnCreate(savedInstanceState);
			//SetTheme(Android.Resource.Style.ThemeHoloLightNoActionBar);
			// Use profile layout
			SetContentView(Resource.Layout.ProfileScreen);
			// Variables for screen objects
			ImageView avatarImageView = FindViewById<ImageView>(Resource.Id.AvatarImageView);
			Button sendMessageButton = FindViewById<Button>(Resource.Id.SendMessageButton);
			TextView emailTextView = FindViewById<TextView>(Resource.Id.EmailTextView);
			TextView contactMethodsTextView = FindViewById<TextView>(Resource.Id.ContactMethodsTextView);
			Button editButton = FindViewById<Button>(Resource.Id.EditButton);
			Button blockButton = FindViewById<Button>(Resource.Id.BlockButton);
			Button reportButton = FindViewById<Button>(Resource.Id.ReportButton);
			TextView ratingTextView = FindViewById<TextView>(Resource.Id.RatingTextView);
			Button rateButton = FindViewById<Button>(Resource.Id.RateButton);
			TextView aboutTextView = FindViewById<TextView>(Resource.Id.AboutTextView);
			TextView iTutorTextView = FindViewById<TextView>(Resource.Id.ITutorTextView);
			TextView iNeedATutorTextView = FindViewById<TextView>(Resource.Id.INeedATutorTextView);
			TextView availabilityTextView = FindViewById<TextView>(Resource.Id.AvailabilityTextView);
			TextView pricesTextView = FindViewById<TextView>(Resource.Id.PricesTextView);
			Button classListView = FindViewById<Button>(Resource.Id.classButton);
            string messageStudent = Intent.GetStringExtra("nameOf");
			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetActionBar(toolbar);
			// Force toolbar refresh
			InvalidateOptionsMenu();

			string n = Intent.GetStringExtra("studentName");

			// Get profile information from the server
			if (n != null && n != "")
				// For profiles besides your own
				p = HTTPHandler.getProfileByName(n);
			else {
				// For your own profile
				p = HTTPHandler.getProfile(Intent.GetStringExtra("email"));
			}


			// Set screen fields based on profile information retrieved
			//nameTextView.Text = p.name;
			emailTextView.Text = p.email;
			ratingTextView.Text = p.rating.ToString();
			aboutTextView.Text = p.about;
            ActionBar.Title = p.name;
			availabilityTextView.Text = p.time;
			pricesTextView.Text = p.price;
			// Set tutor fields
			iTutorTextView.Text = "";
			iNeedATutorTextView.Text = "";
            if (p.classesIn != null)
            {
                foreach (classInfo ci in p.classesIn)
                {
					// Add to tutor section for tutor labeled classes
                    if (ci.type == "tutor")
                    {
                        iTutorTextView.Text += ci.className;
                        iTutorTextView.Text += ", "; // separate by commas
                    }
					// Or student section if labeled student
                    else if (ci.type == "student")
                    {
                        iNeedATutorTextView.Text += ci.className;
                        iNeedATutorTextView.Text += ", ";
                    }
                }
            }

			// Load the avatar with Picasso
			Picasso.With(this).Load(p.avatar).Into(avatarImageView);

			// Check permissions of the user viewing the profile
			Profile u = HTTPHandler.getProfile(HTTPHandler.emailLoggedIn);

			// If it is the own user's profile
			if (p.email != null && p.email.Equals(HTTPHandler.emailLoggedIn)) {
				sendMessageButton.Enabled = false;
				reportButton.Enabled = false;
				blockButton.Enabled = false;
				editButton.Enabled = true;
				rateButton.Enabled = false;
				if (u.admin != null && u.admin.Equals("true"))
					admin = true;
			}
			// If it is not their profile, but they are an admin
			else if (u.admin != null && u.admin.Equals("true")) {
				sendMessageButton.Enabled = true;
				reportButton.Enabled = true;
				blockButton.Enabled = true;
				editButton.Enabled = true;
                admin = true;
			}
			// Or if it is not their profile, and they are a regular user
			else {
				sendMessageButton.Enabled = true;
				reportButton.Enabled = true;
				blockButton.Enabled = true;
				editButton.Enabled = false;
			}

			// Check if the user has upvoted this profile already, disable upvoting if true
			string uid = u._id;
			if (p.usersUpvoted != null) {
				foreach (upvotedID ui in p.usersUpvoted) {
					if (ui._id.Equals(uid))
						rateButton.Enabled = false;
				}
			}
			// Check if the profile has blocked the user, disable messaging if true
			 if (p.blockedUsers != null) {
				foreach (blockedID bid in p.blockedUsers) {
					if (bid.id.Equals(uid))
						sendMessageButton.Enabled = false;
				}
			}

			if (Intent.GetStringExtra("justLoggedIn") != null && Intent.GetStringExtra("justLoggedIn").Equals("true"))
				justLoggedIn = true;
			else justLoggedIn = false;

			// Check if the profile has been blocked
			bool isBlocked = false;
			blockedID[] bids = HTTPHandler.getProfile(HTTPHandler.emailLoggedIn).blockedUsers;
			if (bids != null) {
				foreach (blockedID bid in bids) {
					if (p._id == bid.id)
						isBlocked = true;
				}
			}
			// Set block button text to unblock if true
			if (isBlocked) blockButton.Text = "Unblock";

			// If Class List button is pressed
			classListView.Click += (object Sender, EventArgs e) => {
				// Change to class list page
				var intent = new Android.Content.Intent(this, typeof(ClassListActivity));
                intent.PutExtra("name", p.name);
				StartActivity(intent);
			};

			// If edit profie button is pressed
			editButton.Click += (Sender, e) => {
				// Change to edit profile page
				var intent = new Android.Content.Intent(this, typeof(EditProfileActivity));
				intent.PutExtra("name", ActionBar.Title);
				intent.PutExtra("email", p.email);
				intent.PutExtra("contact", contactMethodsTextView.Text);
				intent.PutExtra("about", aboutTextView.Text);
				intent.PutExtra("freeTime", availabilityTextView.Text);
				intent.PutExtra("prices", pricesTextView.Text);
				intent.PutExtra("avatar", p.avatar);
				//intent.PutExtra("studyClasses", iTutorTextView.Text);
				//intent.PutExtra("tutorClasses", iNeedATutorTextView.Text);
				StartActivity(intent);

			};

			// If rate button is pressed
			rateButton.Click += (Sender, e) => {
				// Upvote the profile
				HTTPHandler.upvoteProfile(ActionBar.Title);
				Toast toast = Toast.MakeText(this, "Thanks for your input!", ToastLength.Short);
				toast.Show();
				rateButton.Enabled = false;
			};

			// If report button is pressed
			reportButton.Click += (Sender, e) => {
				// Add user to the report list for admins
				HTTPHandler.reportProfile(p._id, p.name, "Reported.");
				Toast toast = Toast.MakeText(this, "User has been reported.", ToastLength.Short);
				toast.Show();
			};

			// If block button is pressed
			blockButton.Click += (Sender, e) => {
				// If user is blocked, unblock
				if (isBlocked) {
					HTTPHandler.unblockProfile(p._id);
					Toast toast = Toast.MakeText(this, "User has been unblocked.", ToastLength.Short);
					toast.Show();
					blockButton.Text = "Block";
					isBlocked = false;
				} else {
				// If user is not blocked, block
					HTTPHandler.blockProfile(p._id);
					Toast toast = Toast.MakeText(this, "User has been blocked.", ToastLength.Short);
					toast.Show();
					blockButton.Text = "Unblock";
					isBlocked = true;
				}
			};

			// If the send message button is pressed
			sendMessageButton.Click += (Sender, e) => {
				// Open the message page btwn the two users
				GetChatID cid = HTTPHandler.startAChat(p._id);
				var intent = new Android.Content.Intent(this, typeof(messagingActivity));
				intent.PutExtra("cid", cid._id);
				intent.PutExtra("uid", p._id);
                intent.PutExtra("name", ActionBar.Title);
                intent.PutExtra("uName", messageStudent);
				StartActivity(intent);
			};
		}

		// Create variables for the menu bar items
		IMenuItem logoutItem, adminItem, banItem;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.topmenus, menu);
			// Initialize variables
			logoutItem = menu.GetItem(0);
			adminItem = menu.GetItem(1);
			banItem = menu.GetItem(2);
			return base.OnCreateOptionsMenu(menu);
        }

		public override bool OnPrepareOptionsMenu(IMenu menu) {
			base.OnCreateOptionsMenu(menu);
			// Disable admin buttons for non-admins
			if (!admin) {
				adminItem.SetVisible(false);
				banItem.SetVisible(false);
			}
			return true;
		}

		// When a menu item was pressed
		public override bool OnOptionsItemSelected(IMenuItem item) 
        {
			// If it was the admin button, go to the admin page
			if (item.TitleFormatted.ToString().Equals("Admin Options")) {
				if (admin) {
					var intent = new Intent(this, typeof(AdminActivity));
					StartActivity(intent);
				}
			}
			// If it was the logout button, return to sign-in page
			else if (item.TitleFormatted.ToString().Equals("Logout")) {
				var intent = new Intent(this, typeof(MainActivity));
				HTTPHandler.emailLoggedIn = "";
				StartActivity(intent);
			}
			// If it was the ban button, have the server ban the given user
			else if (item.TitleFormatted.ToString().Equals("Ban User")) {
				if (admin) {
					HTTPHandler.banUser(p.email);
					Toast toast = Toast.MakeText(this, "User has been banned.", ToastLength.Short);
					toast.Show();
				}
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