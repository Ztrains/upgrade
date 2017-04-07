
using Android.App;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;

namespace UpgradeApp
{
    public class messageAdapter : BaseAdapter<string>
    {
        string[] items;
        Activity context;
        ArrayList chatList;
        public messageAdapter(Activity context, ArrayList chatList) : base()
        {
            this.context = context;
            this.chatList = chatList;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override string this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Length; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            chatClass chatObject = (chatClass)chatList[position];
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.bubbler, null);

            }
            TextView message = (TextView)view.FindViewById(Resource.Id.message_text);
            //message.SetText();
            
            return view;
        }
    }
}
