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

using System.Net.Http;

namespace UpgradeApp
{
    [Activity(Label = "ClassList")]
    public class ClassListActivity: Activity
    {
		

        ListView listView;
		ClassList classes;
		//string[] items = { "CS 307", "MA 265", "CS 180" }; will use classes.Classes now

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ClassesScreen);

			// Get information from server
			classes = HTTPHandler.classListRequest();

			// Populate listView from server received information
			listView = FindViewById<ListView>(Resource.Id.classList);
			listView.Adapter = new ListAdapter(this, classes.classes);

            listView.ItemClick += ListView_ItemClick;     
        }

        public void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
			// Send the class name to the new screen
			var intent = new Android.Content.Intent(this, typeof(StudentListActivity));
			intent.PutExtra("className", classes.classes[e.Position]);
			StartActivity(intent);


			/*
			if (classes.Classes[e.Position] == "CS 307")
            {
                var intent = new Android.Content.Intent(this, typeof(StudentListActivity));
                StartActivity(intent);
            }
            if (classes.Classes[e.Position] == "MA 265")
            {
                var intent = new Android.Content.Intent(this, typeof(StudentListActivity));
                StartActivity(intent);
            }
            if (classes.Classes[e.Position] == "CS 180")
            {
                var intent = new Android.Content.Intent(this, typeof(StudentListActivity));
                StartActivity(intent);
            }
			*/
        }
    }
}