using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using LAPhil.Droid;
using LAPhil.User;

namespace SharedLibraryAndroid
{ 
    public class TicketsBarCodePagerAdapter : PagerAdapter
    {
        LayoutInflater layoutInflater;
        public Context mContext;
        private TicketDetail[] myTicketDetail;
        private String ticketName;
        public TicketsBarCodePagerAdapter(Context context,TicketDetail[] myTicketDetail,String ticketsName)
        {
            this.myTicketDetail = myTicketDetail;
            this.mContext = context;
            this.ticketName = ticketsName;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            layoutInflater = (LayoutInflater)Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService);
            View view = layoutInflater.Inflate(Resource.Layout.TicketsDetailCell, container, false);
            ImageView image = (ImageView)view.FindViewById(Resource.Id.imageBarCode);
            TextView ticketsCount = (TextView)view.FindViewById(Resource.Id.ticketsCount);
            TextView ticketsName = (TextView)view.FindViewById(Resource.Id.ticketsName);
            ticketsName.SetTypeface(Utility.BoldTypeface(mContext),TypefaceStyle.Bold);
            ticketsCount.SetTypeface(Utility.RegularTypeface(mContext), TypefaceStyle.Normal);

            ticketsCount.Text = position + 1 + " of " + myTicketDetail.Length + " tickets";
            ticketsName.Text = ticketName;
            try
            {
                if (myTicketDetail[position].BarcodeUrl != null && myTicketDetail[position].BarcodeUrl != "")
                {
                    Glide.With(mContext).Load(myTicketDetail[position].BarcodeUrl).Thumbnail(0.1f).Into(image);

                }
                else
                {
                    ticketsName.Text = "Ticket Currently Unavailable\n" + ticketName;
                }
            }
            catch (Exception)
            { }
            container.AddView(view);
            return view;
        }

        public override int Count
        {
            get
            {
                return myTicketDetail.Length;
            }
        }
        public override bool IsViewFromObject(View view, Java.Lang.Object objectValue)
        {
            return view == objectValue;
        }
        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object objectValue)
        {
            View view = (View)objectValue;
            container.RemoveView(view);
        }
    }
}