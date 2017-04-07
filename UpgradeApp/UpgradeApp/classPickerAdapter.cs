using System;
using Android.App;
using Android.Views;
using Android.Widget;

namespace UpgradeApp
{
    public class classListAdapter : BaseAdapter<string>
    {
        string[] items;
        Activity context;
        public classListAdapter (Activity context, string[] items) : base()
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
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItemChecked, null);

            }
            //Postion is a place holder
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position];
            return view;
        }
    }
}
