using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Graphics;
using LAPhil.Events;
using System.Collections.Generic;
using System;
using Com.Bumptech.Glide;
using Android.Support.V4.App;
using LAPhil.Application;
using System.Threading.Tasks;
using Android.OS;
using Android.Text;
using Android.Text.Format;

namespace HollywoodBowl.Droid
{
    public class ConcertsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;
        public Context mContext;
        //private Event[] eventList;
        List<Event> eventList;
        public event EventHandler<int> ItemonClick;
        EventService eventService = ServiceContainer.Resolve<EventService>();

        public ConcertsAdapter(List<Event> eventList, Context mContext)
        {
            this.eventList = eventList;
            this.mContext = mContext;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // instantiate/inflate a view
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ConcertCell, parent, false);

            var viewHolder = new ConcertsViewHolder(itemView, OnClick, OnbuyNowClick);
            return viewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as ConcertsViewHolder;
            // assign values to the views' text properties
            if (viewHolder == null) return;

            viewHolder.ConcertsRow.Tag = position;
            var eventData = eventList[position] as Event;

            var localTime = TimeZoneInfo.ConvertTimeFromUtc(eventData.StartTime.DateTime, System.TimeZoneInfo.Local);
            var date1 = localTime.ToString("MMM dd");
            var date2 = localTime.ToString($"ddd - {TimeFormat}");

            //var myDateTime = eventData.StartTime.ToString("yyyyMMddTHHmmssZ");
            //DateTime d = DateTime.ParseExact(myDateTime, "yyyyMMdd'T'HHmmss'Z'", null);
            //var date1 = d.ToString("MMM dd");  //d.ToString("ddd / MMM dd, yyyy /H:mm tt");
            //var date2 = d.ToString("ddd - H:mm tt");
            viewHolder.DateTextView.Text = date1;
            viewHolder.DayTimeTextView.Text = date2;


            viewHolder.DateTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            viewHolder.DayTimeTextView.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);
            viewHolder.NameTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);

            viewHolder.btnSeeDetail.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            viewHolder.btnbuynow.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            viewHolder.lblSpecialRule.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);

            if (((int)Build.VERSION.SdkInt) >= 24)
            {
                viewHolder.NameTextView.TextFormatted = Html.FromHtml(eventList[position].Program.Name, FromHtmlOptions.ModeLegacy);
            }
            else
            {
                viewHolder.NameTextView.TextFormatted = Html.FromHtml(eventList[position].Program.Name);
            }

            if (eventData.Program.ExtraMessage != null)
            {
                viewHolder.lblSpecialRule.Text = eventData.Program.ExtraMessage;
            }else{
                viewHolder.lblSpecialRule.Text = "";
            }

            try
            {
                if (eventData.Image3x2Url != null && eventData.Image3x2Url != "")
                {
                    Glide.With(mContext).Load(eventData.Image3x2Url).Thumbnail(0.1f).Into(viewHolder.ImageConcerts);
                }
                else
                {
                    Glide.With(mContext).Load(eventData.ImageUrl).Thumbnail(0.1f).Into(viewHolder.ImageConcerts);
                }
            }
            catch (Exception)
            { }
           
        }
        // Return the number of items
        public override int ItemCount => eventList.Count;
        void OnClick(int position)
        {
            ItemClick(this, position);
        }
        void OnbuyNowClick(int position)
        {
            ItemonClick(this, position);
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

    internal class ConcertsViewHolder : RecyclerView.ViewHolder
    {

        public View ConcertsRow { get; }
        public TextView NameTextView { get; }
        public TextView DateTextView { get; }
        public TextView LabelTextView { get; }
        public TextView DayTimeTextView { get; }
        public TextView btnSeeDetail { get; }
        public ImageView ImageConcerts { get; }
        public TextView btnbuynow { get; }
        public TextView lblSpecialRule { get; }

        public ConcertsViewHolder(View itemView, Action<int> listener, Action<int> buyNowListner) : base(itemView)
        {
            ConcertsRow = itemView;
            DateTextView = ConcertsRow.FindViewById<TextView>(Resource.Id.lblDateTextView);
            DayTimeTextView = ConcertsRow.FindViewById<TextView>(Resource.Id.lblDayTime);
            NameTextView = ConcertsRow.FindViewById<TextView>(Resource.Id.lblName);
            btnSeeDetail = ConcertsRow.FindViewById<TextView>(Resource.Id.btn_see_details);
            btnbuynow = ConcertsRow.FindViewById<TextView>(Resource.Id.btn_buy_now);
            ImageConcerts = ConcertsRow.FindViewById<ImageView>(Resource.Id.imageConcerts);
            lblSpecialRule = ConcertsRow.FindViewById<TextView>(Resource.Id.lblSpecialRule);

            ImageConcerts.Click += (sender, e) => listener(base.LayoutPosition);
            btnSeeDetail.Click += (sender, e) => listener(base.LayoutPosition);
            btnbuynow.Click += (sender, e) => buyNowListner(base.LayoutPosition);
        }

    }
}
