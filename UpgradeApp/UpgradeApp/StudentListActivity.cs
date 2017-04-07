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
		//string[] items = { "Bob Ross", "Curtis Maves", "Mitch Daniels" };

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			string c = Intent.GetStringExtra("className");
			// Get information from server
			students = HTTPHandler.studentListRequest(c);

			SetContentView(Resource.Layout.listOfStudents);

            listView = FindViewById<ListView>(Resource.Id.students);
            listView.Adapter = new StudentAdapter(this, students.students);
			listView.ItemClick += ListView_ItemClick;
        }

		public void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e) {

			// Send the student name to the new screen
			var intent = new Android.Content.Intent(this, typeof(StudentListActivity));
			intent.PutExtra("studentName", students.students[e.Position].name); //students.students[e.Position]
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