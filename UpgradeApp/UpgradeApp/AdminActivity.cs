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
    [Activity(Label = "AdminActivity")]
    public class AdminActivity : Activity
    {
        Reports reportedUsers;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.adminScreen);

            Button reportedUserButton = FindViewById<Button>(Resource.Id.reportButton);
            Button requestClassButton = FindViewById<Button>(Resource.Id.requestClassButton);

			// If reported users button is pressed, view the reports page
            reportedUserButton.Click += (Sender, e) =>
            {
                var intent = new Intent(this, typeof(reportedUsersActivity));
                StartActivity(intent);
            };
			// If the class request list button is pressed, view the requests page
            requestClassButton.Click += (Sender, e) =>
            {
                var intent = new Intent(this, typeof(requestedClassesActivity));
                StartActivity(intent);
            };
            
        }
    }
}