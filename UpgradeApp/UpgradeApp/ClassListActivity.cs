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
            EditText searchText = FindViewById<EditText>(Resource.Id.searchBoxC);
            Button searchButton = FindViewById<Button>(Resource.Id.searchButtonC);
            Button boardButton = FindViewById<Button>(Resource.Id.boardButton);

			// Get information from server
			classes = HTTPHandler.classListRequest();
			Array.Sort(classes.classes, (x, y) => (string.Compare(x, y)));

			// Populate listView from server received information
			listView = FindViewById<ListView>(Resource.Id.classList);
			ListAdapter adapt = new ListAdapter(this, classes.classes);
            listView.Adapter = adapt;

            listView.ItemClick += ListView_ItemClick;

            searchButton.Click += (Sender, e) =>
            {
                if (!searchText.Text.Equals(""))
                {
                    ClassList filtered = ClientHelper.filterClasses(classes, searchText.Text);
                    listView.Adapter = null;
                    if (filtered.classes.Length != 0)
                    {
                        listView.Adapter = new ListAdapter(this, filtered.classes);
                    }
                }
                else
                {
                    Toast toaster = Toast.MakeText(this, "Please enter in something", ToastLength.Short);
                    toaster.Show();
                    listView.Adapter = null;
                    adapt = new ListAdapter(this, classes.classes);
                    listView.Adapter = adapt;
                }
            };

           
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