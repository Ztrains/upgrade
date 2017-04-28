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
    public class StudentListActivity : Activity
    {

		StudentList students;
		ListView listView;
        string nameOf;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            string c = Intent.GetStringExtra("className");
            string theClassName = Intent.GetStringExtra("theClassName");
            // Get information from server
            students = HTTPHandler.studentListRequest(c);
			// Sort the students alphabetically
			try {
				Array.Sort(students.students, (x, y) => (string.Compare(x.name, y.name)));
			} catch (Exception e) {
				// Don't sort if empty list
			}
            // Use student list layout
            SetContentView(Resource.Layout.listOfStudents);
            nameOf = Intent.GetStringExtra("name");
			// Create variables for screen objects
            EditText searchBox = FindViewById<EditText>(Resource.Id.searchBox);
            Button searchButton = FindViewById<Button>(Resource.Id.searchButton);
            Button boardButton = FindViewById<Button>(Resource.Id.boardButton);
			// Populate and set up list
			StudentAdapter adapt;
				try {
				listView = FindViewById<ListView>(Resource.Id.students);
				adapt = new StudentAdapter(this, students.students);
				listView.Adapter = adapt;
				listView.ItemClick += ListView_ItemClick;
			} catch (Exception e) {
				// Do nothing if empty
			}

			// If search button is pressed, filter results
            searchButton.Click += (Sender, e) =>
            {
                if (!searchBox.Text.Equals(""))
                {
                    StudentList filtered = ClientHelper.filterStudents(ref students, searchBox.Text);
					if (filtered != null) {
						listView.Adapter = null;
						if (filtered.students.Length != 0) {
							listView.Adapter = new StudentAdapter(this, filtered.students);
						}
					}
					else {
						Toast toaster = Toast.MakeText(this, "No students!", ToastLength.Short);
						toaster.Show();
					}
                }
                else
                {
                    Toast toaster = Toast.MakeText(this, "Please enter in something", ToastLength.Short);
                    toaster.Show();
                    listView.Adapter = null;
                    adapt = new StudentAdapter(this, students.students);
                    listView.Adapter = adapt;
                }
            };

			// If message board button is pressed, open this class's message board
            boardButton.Click += (Sender, e) =>
            {
                var intent = new Intent(this, typeof(messageBoardActivity));
                intent.PutExtra("theClass", theClassName);
                intent.PutExtra("nameOf", nameOf);
                StartActivity(intent);
            };
        }

		public void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e) {
			// Send the student name to the new screen
			var intent = new Android.Content.Intent(this, typeof(ProfileActivity));
			intent.PutExtra("studentName", students.students[e.Position].name);
            intent.PutExtra("nameOf", nameOf);
            StartActivity(intent);

		}

	}
}