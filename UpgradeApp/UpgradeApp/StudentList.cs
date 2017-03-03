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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.listOfStudents);

            string[] items = { "Bob Ross", "Curtis Maves", "Mitch Daniels" };
            listView = FindViewById<ListView>(Resource.Id.students);
            listView.Adapter = new StudentAdapter(this, items);
        }
    }
}