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
    [Activity(Label = "ClassList")]
    public class ClassList: Activity
    {
        ListView listView;
        string[] items = { "CS 307", "MA 265", "CS 180" };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ClassesScreen);

           
            listView = FindViewById<ListView>(Resource.Id.classList);
            listView.Adapter = new ListAdapter(this, items);

            listView.ItemClick += ListView_ItemClick;
          
        }

        public void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (items [e.Position] == "CS 307")
            {
                var intent = new Android.Content.Intent(this, typeof(StudentList));
                StartActivity(intent);
            }
            if (items[e.Position] == "MA 265")
            {
                var intent = new Android.Content.Intent(this, typeof(StudentList));
                StartActivity(intent);
            }
            if (items[e.Position] == "CS 180")
            {
                var intent = new Android.Content.Intent(this, typeof(StudentList));
                StartActivity(intent);
            }
        }
    }
}