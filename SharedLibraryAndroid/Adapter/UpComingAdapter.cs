using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using LAPhil.Droid;
using System;
using LAPhil.Events;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using LAPhil.User;
using Android.Text.Format;
using System.Linq;
using Android.OS;
using Android.Text;

namespace SharedLibraryAndroid
{
    class UpComingAdapter : RecyclerView.Adapter
    {
        public Ticket[] ProgramList;
        public event EventHandler<int> ItemClick;
        public Context mContext;
        public Event[] events;
        public UpComingAdapter(Ticket[] _ProgramList, Context context,Event[] events)
        {
            this.ProgramList = _ProgramList;
            this.mContext = context;
            this.events = events;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        // instantiate/inflate a view
        var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.UpcomingCell, parent, false);

            var viewHolder = new UpComingViewHolder(itemView,OnClick);
        return viewHolder;
    }

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        var viewHolder = holder as UpComingViewHolder;
        // assign values to the views' text properties
        if (viewHolder == null) return;

            viewHolder.DateTextView.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
            viewHolder.NameTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            var timeString = ProgramList[position].Date;
  

            var date = timeString.DateTime.ToString($"ddd / MMM dd, yyyy / {TimeFormat}");

            viewHolder.DateTextView.Text = date;

            if (events != null)
            {
                var selectedEvent = events.Where(x => x.TicketingSystemId == ProgramList[position].PerformanceId).FirstOrDefault();
                if (selectedEvent != null)
                {
                    viewHolder.NameTextView.Text = selectedEvent.Program.Name;
                }
                else
                {
                    viewHolder.NameTextView.Text = ProgramList[position].Name;
                }
            }
            else
            {
                viewHolder.NameTextView.Text = ProgramList[position].Venue + " " + ProgramList[position].SectionDescription;
            }

            if (((int)Build.VERSION.SdkInt) >= 24)
            {
                viewHolder.NameTextView.TextFormatted = Html.FromHtml(viewHolder.NameTextView.Text, FromHtmlOptions.ModeLegacy);
            }
            else
            {
                viewHolder.NameTextView.TextFormatted = Html.FromHtml(viewHolder.NameTextView.Text);
            }

            viewHolder.UpcomingRow.Tag = position;
    }

    // Return the number of items
        public override int ItemCount => ProgramList.Length;

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }
        public string TimeFormat
        {
            get
            {
                if (DateFormat.Is24HourFormat(mContext))
                    return "H:mm tt";

                return "h:mm tt";
            }
        }
   }
   internal class UpComingViewHolder : RecyclerView.ViewHolder
   {
    public View UpcomingRow { get; set; }

    public TextView NameTextView { get; set; }

    public TextView DateTextView { get; set; }

    public LinearLayout BtnShowDetail { get; }



        public UpComingViewHolder(View itemView,Action<int> listener) : base(itemView)
    {
            UpcomingRow = itemView;

         NameTextView = UpcomingRow.FindViewById<TextView>(Resource.Id.tv_name);
         DateTextView = UpcomingRow.FindViewById<TextView>(Resource.Id.tv_date);
         BtnShowDetail = UpcomingRow.FindViewById<LinearLayout>(Resource.Id.lytUpcoming);
         BtnShowDetail.Click+=delegate 
             {
                listener(base.LayoutPosition);
             };
    }
}
}
