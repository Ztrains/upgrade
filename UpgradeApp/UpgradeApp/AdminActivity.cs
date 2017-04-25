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
    [Activity(Label = "AdminActivity")]
    public class AdminActivity : Activity
    {
        Student[] students;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AdminScreen);

            ListView listView = FindViewById<ListView>(Resource.Id.bannedUsers);
            listView.Adapter = new BannedAdapter(this, students);//Needs to be replaced with students from the server

            // Create your application here
        }
    }
}