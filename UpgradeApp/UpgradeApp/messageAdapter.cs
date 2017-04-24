
using Android.App;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UpgradeApp
{
    public class messageAdapter : BaseAdapter<chatClass>
    {
        //string[] items;
        Activity context;
        List<chatClass> chatList;
        LinearLayout layout;
        public messageAdapter(Activity context, List<chatClass> chatList) : base()
        {
            this.context = context;
            this.chatList = chatList;
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
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.bubbler, parent, false);
                 
            }
            TextView message = (TextView)view.FindViewById(Resource.Id.message_text);
            layout = (LinearLayout)view.FindViewById(Resource.Id.bubble_layout);
            //LinearLayout parent_layout = (LinearLayout)view.FindViewById(Resource.Id.bubble_layout_parent);
            message.Text = chatObject.message;
            if(chatObject.direction)
            {
                layout.SetGravity(GravityFlags.Left);
            }
            else
            {
                layout.SetGravity(GravityFlags.Right);
            }
            //layout.SetGravity(chatObject.direction ? GravityFlags.Left : GravityFlags.Right);

            //message.SetText();
            
            return view;
        }
    }
}
