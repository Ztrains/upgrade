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
    public class StudentAdapter : BaseAdapter<string>
    {
        Student[] items;
        Activity context;
        public StudentAdapter(Activity context, Student[] items) : base()
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
            get { return items[position].name; } // this didn't have the .name but idk what it needs tbh
        }
        public override int Count
        {
            get { return items.Length; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.TwoLineListItem, null);

            }
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position].name;
			view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = items[position].type;
			return view;
        }
    }
}
