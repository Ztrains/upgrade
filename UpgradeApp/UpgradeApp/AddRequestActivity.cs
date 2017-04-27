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
    [Activity(Label = "AddRequestActivity")]
    public class AddRequestActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.addClassesScreen);
            Button submit = FindViewById<Button>(Resource.Id.submitButtonAdd);
            EditText requestedClass = FindViewById<EditText>(Resource.Id.inputedRequest);//Contains requested classs

            submit.Click += (Sender, e) =>
            {
				//get requested class and store on database
				if (requestedClass.Text != null) {
					HTTPHandler.requestClass(requestedClass.Text);
					Toast toast = Toast.MakeText(this, "Class has been requested!", ToastLength.Short);
					toast.Show();
					requestedClass.Text = "";
				}
					
				
            };
        }
    }
}