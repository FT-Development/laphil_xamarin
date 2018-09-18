using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace HollywoodBowl.Droid
{
   public class NearDiningAdapter : RecyclerView.Adapter
    {
       
        public Context mContext;
       // public NearDiningAdapter(List<Programs.Result> _ProgramList, Context context)
       // {
       //     this.ProgramList = _ProgramList;
       //     this.mContext = context;
        //}
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // instantiate/inflate a view
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.NearDining_cell, parent, false);

            var viewHolder = new NearDiningViewHolder(itemView);
            return viewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as NearDiningViewHolder;
            // assign values to the views' text properties
            if (viewHolder == null) return;

            viewHolder.NearDiningRow.Tag = position;
        }

        // Return the number of items
        public override int ItemCount => 10;
    }
    internal class NearDiningViewHolder : RecyclerView.ViewHolder
    {
        public View NearDiningRow { get; }

        public TextView NameTextView { get; }

        public TextView DateTextView { get; }

        public TextView LabelTextView { get; }

        public NearDiningViewHolder(View itemView) : base(itemView)
        {
            NearDiningRow = itemView;
            //  NameTextView = ProgramRow.FindViewById<TextView>(Resource.Id.nameTextView);
            // DateTextView = ProgramRow.FindViewById<TextView>(Resource.Id.companyTextView);
            //  LabelTextView = ConcertsRow.FindViewById<TextView>(Resource.Id.jobTitleTextView);
        }
    }
}
