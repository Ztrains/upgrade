
using Android.App;
using Android.Content.Res;
using Android.Views;
using Android.Widget;

namespace UpgradeApp
{
    public class messageAdapter : BaseAdapter<string>
    {
        string[] items;
        Activity context;
        public messageAdapter(Activity context, string[] items) : base()
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
            chatClass chatObject = (chatClass)GetItem(position);
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.bubbler, null);

            }
            TextView message = (TextView)view.FindViewById(Resource.Id.message_text);
            
            return view;
        }
    }
}
