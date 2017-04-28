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
        List<chatClass> chats;
        string[] names;
        GetChatID cid;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //SetTheme(Android.Resource.Style.ThemeMaterialLight);

            SetContentView(Resource.Layout.messageBoard);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbarMB);
            SetActionBar(toolbar);
            ActionBar.Title = Intent.GetStringExtra("theClass");
            cid = HTTPHandler.startABoard(Intent.GetStringExtra("theClass"));
            Messages ms = HTTPHandler.getMessages(cid._id);
            string nameOfStudent = Intent.GetStringExtra("nameOf");

            Button sendButton = FindViewById<Button>(Resource.Id.sendButton);
            TextView msgTextView = FindViewById<TextView>(Resource.Id.msg);
            TextView userName = FindViewById<TextView>(Resource.Id.userName);
            listView = FindViewById<ListView>(Resource.Id.message);
            chats = new List<chatClass>();

            if (ms != null && ms.messages != null)
            {
                for (int i = 0; i < ms.messages.Length; i++)
                {
                    bool direction = true;
                    if (nameOfStudent.Equals(HTTPHandler.getName(ms.messages[i].sender)))
                    {
                        direction = false;
                    }
                    chatClass c = new chatClass(direction, ms.messages[i].message);
                    chats.Add(c);
                }
                names = new string[ms.messages.Length];
                for (int i = 0; i < ms.messages.Length; i++)
                {
                    names[i] = HTTPHandler.getName(ms.messages[i].sender);
                }
            }
            else
            {
                chatClass chatter = new chatClass(true, "Be the first to send a message");
                chats.Add(chatter);
                names = new string[1];
                names[0] = "Geo";
                HTTPHandler.sendMessageBoard(cid._id, Intent.GetStringExtra("theClass"), chatter.message);
            }

            messageBoardAdapter adapt = new messageBoardAdapter(this, chats, names);
            listView.Adapter = adapt;


            sendButton.Click += (object Sender, EventArgs e) =>
            {
                if (!msgTextView.Text.Equals(""))
                {
                    HTTPHandler.sendMessageBoard(cid._id, Intent.GetStringExtra("theClass"), msgTextView.Text);
                    chatClass chatter = new chatClass(false, msgTextView.Text);
                    chats.Add(chatter);
                    msgTextView.Text = "";
                    ms = HTTPHandler.getMessages(cid._id);
                    if (ms != null)
                    {
                        names = new string[ms.messages.Length];
                        for (int i = 0; i < ms.messages.Length; i++)
                        {
                            names[i] = HTTPHandler.getName(ms.messages[i].sender);
                        }
                    }
                    adapt = new messageBoardAdapter(this, chats, names);
                    listView.Adapter = null;
                    listView.Adapter = adapt;
                }
            };
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.messageBoardToolbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item) //Passed in the menu item that was selected
        {
            Messages ms = HTTPHandler.getMessages(cid._id);
            if (ms != null && ms.messages != null)
            {
                names = new string[ms.messages.Length];
                for (int i = 0; i < ms.messages.Length; i++)
                {
                    names[i] = HTTPHandler.getName(ms.messages[i].sender);
                }
            }
            messageBoardAdapter adapt = new messageBoardAdapter(this, chats, names);
            listView.Adapter = null;
            listView.Adapter = adapt;
            return base.OnOptionsItemSelected(item);
        }
    }
}
