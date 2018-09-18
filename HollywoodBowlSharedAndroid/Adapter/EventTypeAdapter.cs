using System;
using System.Collections.Generic;
using System.Text;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using LAPhil.Events;

namespace HollywoodBowl.Droid
{
    public class EventTypeAdapter : RecyclerView.Adapter
    {
        public Context mContext;
        public event EventHandler<int> ItemClick;
        List<String> eventType;
        public EventTypeAdapter(List<String> eventType, Context mContext)
        {
            this.eventType = eventType;
            this.mContext = mContext;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // instantiate/inflate a view
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.Event_Type_cell, parent, false);

            var viewHolder = new EventTypeViewHolder(itemView, OnClick);
            return viewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as EventTypeViewHolder;
            // assign values to the views' text properties
            if (viewHolder == null) return;

            viewHolder.EventTypeRow.Tag = position;
            viewHolder.NameTextView.SetTypeface(Utility.BoldTypeface(mContext), TypefaceStyle.Bold);
            viewHolder.NameTextView.Text = eventType[position];

        }

        // Return the number of items
        public override int ItemCount => eventType.Count;

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

    }
    internal class EventTypeViewHolder : RecyclerView.ViewHolder
    {
        public View EventTypeRow { get; }

        public TextView NameTextView { get; }

        public TextView DateTextView { get; }

        public ImageView LblImageView { get; }
        public SparseBooleanArray selectedItems = new SparseBooleanArray();
        public EventTypeViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            EventTypeRow = itemView;
            NameTextView = EventTypeRow.FindViewById<TextView>(Resource.Id.lblEventName);
            EventTypeRow.Click += (sender, e) => {
                listener(base.LayoutPosition);

                if (selectedItems.Get(base.LayoutPosition, false))
                {
                    selectedItems.Delete(base.LayoutPosition);
                    itemView.Selected = false;
                }
                else
                {
                    selectedItems.Put(base.LayoutPosition, true);
                    itemView.Selected = true;
                }

            };

        }
    }
}