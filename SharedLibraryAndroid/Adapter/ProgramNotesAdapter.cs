using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using LAPhil.Droid;
using LAPhil.Events;

namespace SharedLibraryAndroid
{
    class ProgramNotesAdapter : RecyclerView.Adapter
    {
        public event EventHandler<int> ItemClick;
        public List<Piece> ProgramList;
        public Context mContext;
        public ProgramNotesAdapter(List<Piece> _ProgramList, Context context)
        {
            this.ProgramList = _ProgramList;
            this.mContext = context;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // instantiate/inflate a view
            var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ProgramNotesCell, parent, false);

            var viewHolder = new ProgramViewHolder(itemView, OnClick, mContext);
            return viewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as ProgramViewHolder;
            // assign values to the views' text properties
            if (viewHolder == null) return;

            Piece piece = ProgramList[position];

            if (piece.Description != null && piece.Description != "")
            {
                viewHolder.ReadMore.Visibility = ViewStates.Visible;
                viewHolder.PieceDesc.Visibility = ViewStates.Visible;
                viewHolder.DescTitle.Visibility = ViewStates.Visible;

                if (((int)Build.VERSION.SdkInt) >= 24)
                {
                    viewHolder.PieceDesc.TextFormatted = Html.FromHtml(piece.Description, FromHtmlOptions.ModeLegacy);
                }
                else
                {
                    viewHolder.PieceDesc.TextFormatted = Html.FromHtml(piece.Description);
                }
            }
            else
            {
                viewHolder.ReadMore.Visibility = ViewStates.Gone;
                viewHolder.PieceDesc.Visibility = ViewStates.Gone;
                viewHolder.DescTitle.Visibility = ViewStates.Gone;
            }

            if (piece.ComposerName != null && piece.ComposerName != "")
            {
                viewHolder.ComposerTitle.Visibility = ViewStates.Visible;
                viewHolder.btnAboutArtist.Visibility = ViewStates.Visible;
                viewHolder.btnAboutArtist.Text = piece.ComposerName;
            }
            else
            {
                viewHolder.ComposerTitle.Visibility = ViewStates.Gone;
                viewHolder.btnAboutArtist.Visibility = ViewStates.Gone;
            }

            if (piece.Name != null && piece.Name != "")
            { 
                viewHolder.PieceName.Visibility = ViewStates.Visible; 
                viewHolder.PieceName.Text = piece.Name;
            }
            else
            { viewHolder.PieceName.Visibility = ViewStates.Gone; }

            if (piece.Duration != null && piece.Duration != "")
            {
                viewHolder.PieceLength.Visibility = ViewStates.Visible;
                viewHolder.LenghtTitle.Visibility = ViewStates.Visible;
                viewHolder.PieceLength.Text = piece.Duration;
            }
            else
            {
                viewHolder.PieceLength.Visibility = ViewStates.Gone;
                viewHolder.LenghtTitle.Visibility = ViewStates.Gone;
            }

            int pieceNumber = position + 1;
            viewHolder.PieceCount.Text = "Piece " + pieceNumber;
            //viewHolder.PieceDesc.Text = piece.Description;
        }

        // Return the number of items
        public override int ItemCount => ProgramList.Count;

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

    }
    internal class ProgramViewHolder : RecyclerView.ViewHolder
    {
        public View ProgramRow { get; }
        public TextView OrchesText { get; }
        public TextView IntermissionText { get; }
        public TextView PieceCount { get; }
        public TextView btnAboutArtist { get; }
        public TextView PieceName { get; }
        public TextView PieceDesc { get; }
        public TextView PerformanceName { get; }
        public TextView PieceLength { get; }
        public TextView ReadMore { get; }

        public TextView ComposerTitle { get; }
        public TextView PerformTitle { get; }
        public TextView LenghtTitle { get; }
        public TextView DescTitle { get; }
        public TextView OrchesTitle { get; }
       
        public ProgramViewHolder(View itemView, Action<int> listener, Context context) : base(itemView)
        {
            ProgramRow = itemView;
            btnAboutArtist = ProgramRow.FindViewById<TextView>(Resource.Id.btnAboutArtist);
         
            //  
            PieceCount = ProgramRow.FindViewById<TextView>(Resource.Id.pieceCount);
            OrchesText = ProgramRow.FindViewById<TextView>(Resource.Id.tv_orches);
            PieceName = ProgramRow.FindViewById<TextView>(Resource.Id.tv_piece);
            IntermissionText = ProgramRow.FindViewById<TextView>(Resource.Id.tvIntermission);
            PieceDesc = ProgramRow.FindViewById<TextView>(Resource.Id.tv_description);
            PieceLength = ProgramRow.FindViewById<TextView>(Resource.Id.tv_length);
            PerformanceName = ProgramRow.FindViewById<TextView>(Resource.Id.tv_perform);
            ReadMore = ProgramRow.FindViewById<TextView>(Resource.Id.tv_readMore);

            DescTitle = ProgramRow.FindViewById<TextView>(Resource.Id.desTitle);
            ComposerTitle = ProgramRow.FindViewById<TextView>(Resource.Id.composerTitle);
            PerformTitle = ProgramRow.FindViewById<TextView>(Resource.Id.performTitle);
            LenghtTitle = ProgramRow.FindViewById<TextView>(Resource.Id.lengthTitle);
            OrchesTitle = ProgramRow.FindViewById<TextView>(Resource.Id.orchesTitle);

            OrchesText.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);
            PieceCount.SetTypeface(Utility.RegularTypeface(context), TypefaceStyle.Normal);
            btnAboutArtist.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);
            IntermissionText.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);
            PieceDesc.SetTypeface(Utility.RegularTypeface(context), TypefaceStyle.Normal);
            PieceName.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);
            PerformanceName.SetTypeface(Utility.RegularTypeface(context), TypefaceStyle.Normal);
            PieceLength.SetTypeface(Utility.RegularTypeface(context), TypefaceStyle.Normal);

            DescTitle.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);
            ComposerTitle.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);
            PerformTitle.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);
            LenghtTitle.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);
            OrchesTitle.SetTypeface(Utility.BoldTypeface(context), TypefaceStyle.Bold);
            ReadMore.SetTypeface(Utility.RegularTypeface(context), TypefaceStyle.Normal);

            ReadMore.Click += (sender, e) => listener(base.LayoutPosition);
            //  LabelTextView = ConcertsRow.FindViewById<TextView>(Resource.Id.jobTitleTextView);
        }
    }
}