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
        string nameOf;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
			// Use Class list layout
            SetContentView(Resource.Layout.ClassesScreen);
			// Create variables to access screen objects
            EditText searchText = FindViewById<EditText>(Resource.Id.searchBoxC);
            Button searchButton = FindViewById<Button>(Resource.Id.searchButtonC);
            Button request = FindViewById<Button>(Resource.Id.requestClassesAdd);
            nameOf = Intent.GetStringExtra("name");
			// Get information from server
			classes = HTTPHandler.classListRequest();
			// Sort the classes alphabetically
			Array.Sort(classes.classes, (x, y) => (string.Compare(x, y)));
			// Populate listView from server received information
			listView = FindViewById<ListView>(Resource.Id.classList);
			ListAdapter adapt = new ListAdapter(this, classes.classes);
            listView.Adapter = adapt;

            listView.ItemClick += ListView_ItemClick;

			// When search button is clicked
            searchButton.Click += (Sender, e) =>
            {
                if (!searchText.Text.Equals(""))
                {
					// Filter the classes based on the search text entered
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

			// When request button is pressed, open the request page
            request.Click += (Sender, e) =>
            {
                var intent = new Intent(this, typeof(AddRequestActivity));
                StartActivity(intent);
            };

        }

		// When a class is pressed, open the student list for the selected class
        public void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
			// Send the class name to the new screen
			var intent = new Android.Content.Intent(this, typeof(StudentListActivity));
			intent.PutExtra("className", classes.classes[e.Position]);
            intent.PutExtra("theClassName", classes.classes[e.Position]);
            intent.PutExtra("name", nameOf);
			StartActivity(intent);

        }
    }
}