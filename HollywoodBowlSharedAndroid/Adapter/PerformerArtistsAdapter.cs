using System;
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using LAPhil.Events;

namespace HollywoodBowl.Droid
{
    public class PerformerArtistsAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ArtistsItemClick;
        public List<Performer> PerformerList;
        public Context mContext;
        public PerformerArtistsAdapter(List<Performer> _PerformerList, Context context)
        {
            this.PerformerList = _PerformerList;
            this.mContext = context;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // instantiate/inflate a view
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ArtistsCell, parent, false);

            var viewHolder = new PerformerArtistsViewHolder(itemView, OnClick, mContext);
            return viewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as PerformerArtistsViewHolder;
            // assign values to the views' text properties
            if (viewHolder == null) return;

            Performer performer = PerformerList[position];

            if (performer.Name != null && performer.Name != "")
            { 
                viewHolder.display_name.Text = performer.Name;
             }

            if (performer.Role != null && performer.Role != "")
            {
                viewHolder.artistsRole.Text = ", "+performer.Role;
            }
        }

        // Return the number of items
        public override int ItemCount => PerformerList.Count;

        void OnClick(int position)
        {
            ArtistsItemClick?.Invoke(this, position);
        }

    }
    internal class PerformerArtistsViewHolder : RecyclerView.ViewHolder
    {
        public View PerformerRow { get; }
        public TextView artistsRole { get; }
        public TextView display_name { get; }

        public PerformerArtistsViewHolder(View itemView, Action<int> listener, Context context) : base(itemView)
        {
            PerformerRow = itemView;

            artistsRole = PerformerRow.FindViewById<TextView>(Resource.Id.lblArtistsRole);
            display_name = PerformerRow.FindViewById<TextView>(Resource.Id.lblArtistsName);
          

            display_name.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);
            artistsRole.SetTypeface(Utility.ItalicTypeface(context), TypefaceStyle.Italic);

           itemView.Click += (sender, e) => listener(base.LayoutPosition);
            //  LabelTextView = ConcertsRow.FindViewById<TextView>(Resource.Id.jobTitleTextView);
        }
    }
}