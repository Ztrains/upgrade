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
    [Activity(Label = "Activity1")]
    public class messagingActivity : Activity
    {
        ListView listView;
        List<chatClass> chats;//Placeholder, chat objects contain messages, and direction (true for left (so not your message) and false for right (your message))

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.messaging);
			string cid = Intent.GetStringExtra("cid");
			HTTPHandler.getMessages(cid);
            listView = FindViewById<ListView>(Resource.Layout.messaging);
            listView.Adapter = new messageAdapter(this, chats);//Chats should be replaced by stuff from the server. 
            // Create your application here
        }
    }
}