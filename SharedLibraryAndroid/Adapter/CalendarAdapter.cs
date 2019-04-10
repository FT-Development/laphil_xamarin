using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text.Format;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using LAPhil.Droid;
using LAPhil.Events;

namespace SharedLibraryAndroid
{
    class CalendarAdapter : RecyclerView.Adapter
    {
        public Context mContext;
        public event EventHandler<int> ItemClick;
        List<Event> eventList;
        public CalendarAdapter(List<Event> eventList, Context mContext)
        {
            this.eventList = eventList;
            this.mContext = mContext;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // instantiate/inflate a view
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.Calendar_cell, parent, false);

            var viewHolder = new CalendarViewHolder(itemView,OnClick);
            return viewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as CalendarViewHolder;
            // assign values to the views' text properties
            if (viewHolder == null) return;

            viewHolder.CalendarRow.Tag = position;
            viewHolder.DateTextView.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
            viewHolder.NameTextView.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);

            var eventData = eventList[position] as Event;

            var  localTime=  TimeZoneInfo.ConvertTimeFromUtc(eventData.StartTime.DateTime, System.TimeZoneInfo.Local);
            var date = localTime.ToString($"ddd, MMM dd - {TimeFormat}");
           

            viewHolder.DateTextView.Text = date;

            if (((int)Android.OS.Build.VERSION.SdkInt) >= 24)
            {
                viewHolder.NameTextView.TextFormatted = Android.Text.Html.FromHtml(eventList[position].Program.Name, Android.Text.FromHtmlOptions.ModeLegacy);
            }
            else
            {
                viewHolder.NameTextView.TextFormatted = Android.Text.Html.FromHtml(eventList[position].Program.Name);
            }

            try
            {
                if (eventData.Image1x1Url != null && eventData.Image1x1Url != "") {
                    Glide.With(mContext).Load(eventData.Image1x1Url).Thumbnail(0.1f).Into(viewHolder.LblImageView);    
                } else {
                    Glide.With(mContext).Load(eventData.ImageUrl).Thumbnail(0.1f).Into(viewHolder.LblImageView);
                }
            }
            catch (Exception)
            { }

        }

        // Return the number of items
        public override int ItemCount => eventList.Count;

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
    internal class CalendarViewHolder : RecyclerView.ViewHolder
    {
        public View CalendarRow { get; }

        public TextView NameTextView { get; }

        public TextView DateTextView { get; }

        public ImageView LblImageView { get; }

        public CalendarViewHolder(View itemView,Action<int> listener) : base(itemView)
        {
            CalendarRow = itemView;

            NameTextView = CalendarRow.FindViewById<TextView>(Resource.Id.lblName);
            DateTextView = CalendarRow.FindViewById<TextView>(Resource.Id.lblDatetime);
            LblImageView = CalendarRow.FindViewById<ImageView>(Resource.Id.lblImageView);
            CalendarRow.Click += (sender, e) => listener(base.LayoutPosition);

        }
    }


}