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
    [Activity(Label = "messageBoardActivity")]
    public class messageBoardActivity : Activity
    {
        ListView listView;
        List<chatClass> chats;//Needs to be replaced with something new for message board so Object with text, direction, and Name of User
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetTheme(Android.Resource.Style.ThemeMaterialLight);

            SetContentView(Resource.Layout.messageBoard);
            ActionBar.Title = Intent.GetStringExtra("className");

            Button sendButton = FindViewById<Button>(Resource.Id.sendButton);
            TextView msgTextView = FindViewById<TextView>(Resource.Id.msg);
            TextView userName = FindViewById<TextView>(Resource.Id.userName);
            listView = FindViewById<ListView>(Resource.Id.message);
            chats = new List<chatClass>();

            sendButton.Click += (object Sender, EventArgs e) => {
                //Again Adjust when messageboard object is made
            };
            // Create your application here
        }
    }
}