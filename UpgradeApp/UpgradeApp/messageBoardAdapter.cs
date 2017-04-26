
using Android.App;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UpgradeApp
{
    public class messageBoardAdapter : BaseAdapter<chatClass>
    {
        //Again everything needs to be adjust for messageBoard object with text, direction, and username
        //string[] items;
        Activity context;
        List<chatClass> chatList;
        LinearLayout layout;
        string[] names;
        public messageBoardAdapter(Activity context, List<chatClass> chatList, string[] names) : base()
        {
            this.context = context;
            this.chatList = chatList;
            this.names = names;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override chatClass this[int position]
        {
            get { return chatList[position]; }
        }
        public override int Count
        {
            get { return chatList.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
      
            chatClass chatObject = chatList[position];
            View view = convertView;
            string name = names[position];
            int layoutRes = 0;
            if(chatObject.direction)
            {
                layoutRes = Resource.Layout.bubblerLB;
            }
            else
            {
                layoutRes = Resource.Layout.bubblerB;
            }
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(layoutRes, parent, false);
                 
            }
            TextView message = (TextView)view.FindViewById(Resource.Id.message_text);
            TextView namer = (TextView)view.FindViewById(Resource.Id.userName);
            namer.Text = name;

            message.Text = chatObject.message;
            
            return view;
        }
    }
}
