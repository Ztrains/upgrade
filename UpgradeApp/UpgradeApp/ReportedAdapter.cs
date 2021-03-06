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
    public class ReportedAdapter : BaseAdapter<string>
    {
        Report[] students;
        Activity context;
        public ReportedAdapter(Activity context, Report[] students) : base()
        {
            this.context = context;
            this.students = students;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override string this[int position]
        {
            get { return students[position].name; }
        }
        public override int Count
        {
            get { return students.Length; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.TwoLineListItem, null);

            }
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = students[position].name;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = students[position].reason;
            return view;
        }
    }
}
