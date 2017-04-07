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
            

            submit.Click += (object sender, EventArgs e) =>
            {
                var intent = new Android.Content.Intent(this, typeof(EditProfileActivity));
                string returnString = string.Join(" ", returner);
                intent.PutExtra("classes", returnString);
                StartActivity(intent);
            };

            void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
            {
                if (location < returner.Length)
                {
                    returner[location] = items[e.Position];
                    location++;
                }
            }
        //list.ItemClick += List_ItemClick;
    }
    }
}