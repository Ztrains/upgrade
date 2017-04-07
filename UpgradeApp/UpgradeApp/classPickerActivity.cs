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
    [Activity(Label = "classPickerActivity")]
    public class classPickerActivity : Activity
    {
        ListView list;
        ClassList classes; 
        string[] items;
        string[] returner;
        int location = 0;

		void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e) {
			if (location < returner.Length) {
				returner[location] = items[e.Position];
				location++;
			}
		}

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            classes = HTTPHandler.classListRequest();
            items = classes.classes;
            returner = new string[items.Length];
            SetContentView(Resource.Layout.classPickerScreen);
            Button submit = FindViewById<Button>(Resource.Id.submitClassButton);
            list = FindViewById<ListView>(Resource.Id.classPicker);
            ArrayAdapter lister = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItemChecked, items);
            list.Adapter = lister;
            list.ItemClick += ListView_ItemClick;
            list.ChoiceMode = ChoiceMode.Multiple;
            classes = HTTPHandler.classListRequest();
            //Trying to fix errors
            

            submit.Click += (object sender, EventArgs e) =>
            {
                var intent = new Android.Content.Intent(this, typeof(EditProfileActivity));
                string returnString = string.Join(" ", returner);
                if (Intent.GetBooleanExtra("study", true))
                {
                    intent.PutExtra("studyClasses", returnString);
					intent.PutExtra("tutorClasses", Intent.GetStringExtra("tutorClasses"));
                }
                else
                {
                    intent.PutExtra("tutorClasses", returnString);
					intent.PutExtra("studyClasses", Intent.GetStringExtra("studyClasses"));
				}

				//intent.PutExtra("name", Intent.GetStringExtra("name"));
				intent.PutExtra("studentName", Intent.GetStringExtra("studentName"));
				intent.PutExtra("name", Intent.GetStringExtra("studentName"));
				intent.PutExtra("email", Intent.GetStringExtra("email"));
				intent.PutExtra("contact", Intent.GetStringExtra("contact"));
				intent.PutExtra("about", Intent.GetStringExtra("about"));
				intent.PutExtra("freeTime", Intent.GetStringExtra("freeTime"));
				intent.PutExtra("prices", Intent.GetStringExtra("prices"));

				StartActivity(intent);
            };


        //list.ItemClick += List_ItemClick;
		}
    }
}