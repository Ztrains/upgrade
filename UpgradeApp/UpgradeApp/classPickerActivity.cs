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
        classInfo[] newClasses;
        int location = 0;
		bool isAStudent = false;

		void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e) {
			if (location < newClasses.Length) {
				classInfo ci = new classInfo();
				ci.className = items[e.Position];

				if (isAStudent)
					ci.type = "student";
				else ci.type = "tutor";
				newClasses[location] = ci;
				location++;
			}
		}

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			if (Intent.GetStringExtra("study").Equals("true"))
				isAStudent = true;
			else isAStudent = false;

			classes = HTTPHandler.classListRequest();
            items = classes.classes;
            newClasses = new classInfo[items.Length];

            SetContentView(Resource.Layout.classPickerScreen);
            Button submit = FindViewById<Button>(Resource.Id.submitClassButton);
            list = FindViewById<ListView>(Resource.Id.classPicker);
            ArrayAdapter lister = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemChecked, items);
            list.Adapter = lister;
            list.ItemClick += ListView_ItemClick;
            list.ChoiceMode = ChoiceMode.Multiple;

            submit.Click += (object sender, EventArgs e) =>
            {
                var intent = new Android.Content.Intent(this, typeof(EditProfileActivity));
				//string returnString = string.Join(" ", returner);

				// Update classes chosen by the user
				Profile p = HTTPHandler.getProfile(HTTPHandler.emailLoggedIn);
				if (p.classesIn != null) {
					foreach (classInfo c in p.classesIn) {
						if (isAStudent == true && c.type.Equals("student"))
							HTTPHandler.leaveClass(c.className, c.type);
						else if (isAStudent == false && c.type.Equals("tutor"))
							HTTPHandler.leaveClass(c.className, c.type);
					}
				}
				for (int i = 0; i < newClasses.Length; i++) {
					if (newClasses[i] != null && newClasses[i].className != "")
						HTTPHandler.joinClass(newClasses[i].className, newClasses[i].type);
				}

				/*if (Intent.GetBooleanExtra("study", true))
                {
                    intent.PutExtra("studyClasses", returnString);
					intent.PutExtra("tutorClasses", Intent.GetStringExtra("tutorClasses"));
                }
                else
                {
                    intent.PutExtra("tutorClasses", returnString);
					intent.PutExtra("studyClasses", Intent.GetStringExtra("studyClasses"));
				}*/

				//intent.PutExtra("name", Intent.GetStringExtra("name"));
				intent.PutExtra("studentName", Intent.GetStringExtra("studentName"));
				intent.PutExtra("name", Intent.GetStringExtra("studentName"));
				intent.PutExtra("email", Intent.GetStringExtra("email"));
				intent.PutExtra("contact", Intent.GetStringExtra("contact"));
				intent.PutExtra("about", Intent.GetStringExtra("about"));
				intent.PutExtra("freeTime", Intent.GetStringExtra("freeTime"));
				intent.PutExtra("prices", Intent.GetStringExtra("prices"));
				intent.PutExtra("avatar", Intent.GetStringExtra("avatar"));

				StartActivity(intent);
            };


        //list.ItemClick += List_ItemClick;
		}
    }
}