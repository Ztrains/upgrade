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

			Button sendButton = FindViewById<Button>(Resource.Id.sendButton);
			TextView msgTextView = FindViewById<TextView>(Resource.Id.msg);

            SetContentView(Resource.Layout.messaging);
			string uid = Intent.GetStringExtra("uid");
			string cid = Intent.GetStringExtra("cid");
			chats = null;

			Messages ms = HTTPHandler.getMessages(cid);
			if (ms != null) {
				for (int i = 0; i < ms.messages.Length; i++) {
					bool direction = (ms.messages[i].sender.Equals(uid));
					chatClass c = new chatClass(direction, ms.messages[i].message);
					chats.Add(c);
				}
			}
			
            listView = FindViewById<ListView>(Resource.Id.message);
            listView.Adapter = new messageAdapter(this, chats); //Chats should be replaced by stuff from the server. 

			sendButton.Click += (Sender, e) => {
				HTTPHandler.sendMessage(cid, msgTextView.Text);
				msgTextView.Text = "";
				// Need to refresh the messages here probably
			};

		}
    }
}