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
        //string[] items = { "Bob Ross", "Curtis Maves", "Mitch Daniels" };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            string c = Intent.GetStringExtra("className");
            string theClassName = Intent.GetStringExtra("theClassName");
            // Get information from server
            students = HTTPHandler.studentListRequest(c);
            Array.Sort(students.students, (x, y) => (string.Compare(x.name, y.name)));
            
            SetContentView(Resource.Layout.listOfStudents);
            nameOf = Intent.GetStringExtra("name");
            EditText searchBox = FindViewById<EditText>(Resource.Id.searchBox);
            Button searchButton = FindViewById<Button>(Resource.Id.searchButton);
            Button boardButton = FindViewById<Button>(Resource.Id.boardButton);

            listView = FindViewById<ListView>(Resource.Id.students);
            StudentAdapter adapt = new StudentAdapter(this, students.students);
            listView.Adapter = adapt;
            listView.ItemClick += ListView_ItemClick;
            searchButton.Click += (Sender, e) =>
            {
                if (!searchBox.Text.Equals(""))
                {
                    StudentList filtered = ClientHelper.filterStudents(ref students, searchBox.Text);
                    listView.Adapter = null;
                    if (filtered.students.Length != 0)
                    {
                        listView.Adapter = new StudentAdapter(this, filtered.students);
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

			/*if (items[e.Position] == "Bob Ross") {
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
			*/
		}

	}
}