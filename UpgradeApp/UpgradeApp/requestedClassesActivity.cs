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
    [Activity(Label = "requestedClassesActivity")]
    public class requestedClassesActivity : Activity
    {
		string[] requestedClasses; //Replaced by requested classes from the server
        int location = 0;
        string[] items; //All the class names are placed into this 
		string[] checkedClasses; // The list of classes being accepted
        

        void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
			// If an item is checked, add it to the checked array
			if (location < checkedClasses.Length) {
				string a = items[e.Position];
				checkedClasses[location] = a;
				location++;
			}

		}

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.requestedClassScreen);
            Button submit = FindViewById<Button>(Resource.Id.submitClassButtonR);
            ListView listView = FindViewById<ListView>(Resource.Id.requestedClasses);
			requestedClasses = HTTPHandler.getClassAdditionRequests();

			items = requestedClasses;
			checkedClasses = new string[items.Length];
            ArrayAdapter lister = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemChecked, items);
            listView.Adapter = lister;
            listView.ChoiceMode = ChoiceMode.Multiple;
            listView.ItemClick += ListView_ItemClick;



            submit.Click += (object sender, EventArgs e) =>
            {
                var intent = new Android.Content.Intent(this, typeof(AdminActivity));
                // Updating the server with the new classes 
				foreach (string s in checkedClasses) {
					if (s != null) {
						HTTPHandler.createClass(s);
					}
				}
				// and remove the rest of the classes from the requested class list
				HTTPHandler.emptyClassRequestList();
				Recreate();
            };

        }
    }
}