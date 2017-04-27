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
            reportedUsers = HTTPHandler.getReports();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.adminScreen);
            Button reportedUserButton = FindViewById<Button>(Resource.Id.reportButton);
            Button requestClassButton = FindViewById<Button>(Resource.Id.requestClassButton);

            reportedUserButton.Click += (Sender, e) =>
            {
                var intent = new Intent(this, typeof(reportedUsersActivity));
                StartActivity(intent);
            };
            requestClassButton.Click += (Sender, e) =>
            {
                var intent = new Intent(this, typeof(requestedClassesActivity));
                StartActivity(intent);
            };
            

            // Create your application here
        }
    }
}