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
    [Activity(Label = "reportedUsersActivity")]
    public class reportedUsersActivity : Activity
    {
        Reports students; // The list of accounts which have been reported

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.reportedUsersScreen);

			students = HTTPHandler.getReports();
			// Populate screen with report listings
            ListView listView = FindViewById<ListView>(Resource.Id.reportedUsers);
            ReportedAdapter adapt = new ReportedAdapter(this, students.reportedUsers);
            listView.Adapter = adapt;
 
        }
    }
}