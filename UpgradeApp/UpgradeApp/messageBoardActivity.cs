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
            ActionBar.Title = Intent.GetStringExtra("theClass");
            GetChatID cid = HTTPHandler.startABoard(Intent.GetStringExtra("theClass"));
            Messages ms = HTTPHandler.getMessages(cid._id);
            
            Button sendButton = FindViewById<Button>(Resource.Id.sendButton);
            TextView msgTextView = FindViewById<TextView>(Resource.Id.msg);
            TextView userName = FindViewById<TextView>(Resource.Id.userName);
            listView = FindViewById<ListView>(Resource.Id.message);
            chats = new List<chatClass>();
            if (ms != null)
            {
                for (int i = 0; i < ms.messages.Length; i++)
                {
                    bool direction = true;
                    chatClass c = new chatClass(direction, ms.messages[i].message);
                    chats.Add(c);
                }
            }
            else
            {
                chatClass chatter = new chatClass(true, "Be the first to send a message");
                chats.Add(chatter);
            }
            messageAdapter adapt = new messageAdapter(this, chats);
            listView.Adapter = adapt;


            sendButton.Click += (object Sender, EventArgs e) => {
                HTTPHandler.sendMessageBoard(cid._id, Intent.GetStringExtra("className"), msgTextView.Text);
                chatClass chatter = new chatClass(true, msgTextView.Text);
                chats.Add(chatter);
               // msgTextView.Text = "";
                adapt = new messageAdapter(this, chats);
                listView.Adapter = null;
                listView.Adapter = adapt;
            };
            // Create your application here
        }
    }
}