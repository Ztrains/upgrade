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
            SetTheme(Android.Resource.Style.ThemeMaterialLight);
            
            SetContentView(Resource.Layout.messaging);
            ActionBar.Title = Intent.GetStringExtra("name");
            //View view = LayoutInflater.Inflate(Resource.Layout.messaging, null);
			Button sendButton = FindViewById<Button>(Resource.Id.sendButton);
			TextView msgTextView = FindViewById<TextView>(Resource.Id.msg);
            listView = FindViewById<ListView>(Resource.Id.message);
            chats = new List<chatClass>();
            

            string uid = Intent.GetStringExtra("uid");
			string cid = Intent.GetStringExtra("cid");

			Messages ms = HTTPHandler.getMessages(cid);
			if (ms != null) {
				for (int i = 0; i < ms.messages.Length; i++) {
					bool direction = (ms.messages[i].sender.Equals(uid));
					chatClass c = new chatClass(direction, ms.messages[i].message);
					chats.Add(c);
				}
			}
            messageAdapter adapt = new messageAdapter(this, chats);
            listView.Adapter = adapt;
            //Chats should be replaced by stuff from the server. 

            
			sendButton.Click += (object Sender, EventArgs e) => {
				HTTPHandler.sendMessage(cid, msgTextView.Text);
				//msgTextView.Text = "";
                chatClass chatter = new chatClass(false, msgTextView.Text);
                chats.Add(chatter);
                msgTextView.Text = "";
                adapt = new messageAdapter(this, chats);
                listView.Adapter = null;
                listView.Adapter = adapt;
                //Refreshes when sent, but will not update when a message is received
            };
			

        }
    }
}