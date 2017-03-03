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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ClassesScreen);

            string[] items = { "CS 307" , "MA 265" , "CS 180"};
            listView = FindViewById<ListView>(Resource.Id.classList);
            listView.Adapter = new ListAdapter(this, items);
        }
    }
}