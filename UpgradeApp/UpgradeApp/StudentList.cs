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
    [Activity(Label = "StudentList")]
    public class StudentList : Activity
    {
        ListView listView;
		string[] items = { "Bob Ross", "Curtis Maves", "Mitch Daniels" };
	protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.listOfStudents);

            listView = FindViewById<ListView>(Resource.Id.students);
            listView.Adapter = new StudentAdapter(this, items);
			listView.ItemClick += ListView_ItemClick;
        }

		public void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e) {
			if (items[e.Position] == "Bob Ross") {
				var intent = new Android.Content.Intent(this, typeof(ProfileActivity));
				StartActivity(intent);
			}
			if (items[e.Position] == "Curtis Maves") {
				var intent = new Android.Content.Intent(this, typeof(ProfileActivity));
				StartActivity(intent);
			}
			if (items[e.Position] == "Mitch Daniels") {
				var intent = new Android.Content.Intent(this, typeof(ProfileActivity));
				StartActivity(intent);
			}
		}

	}
}