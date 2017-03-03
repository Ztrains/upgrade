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
    public class ListAdapter : BaseAdapter<string>
    {
        string[] items;
        Activity context;
        public ListAdapter(Activity context, string[] items) : base()
        {
            this.context = context;
            this.items = items;
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
            View view = convertView;
            if(view == null)
            {
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
               
            }
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position];
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = "50 Students";
            return view;
        }
    }
} 
