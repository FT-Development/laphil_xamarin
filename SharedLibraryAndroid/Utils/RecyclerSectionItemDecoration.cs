using System;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using LAPhil.Events;
using LAPhil.Droid;
using System.Collections.Generic;
using Android.Content;

namespace SharedLibraryAndroid
{
    class RecyclerSectionItemDecoration : RecyclerView.ItemDecoration
    {
        private int headerOffset;
        private Boolean sticky;
        private View headerView;
        private TextView header;
        private List<Event> eventList;
        private Context mContext;
       
        public RecyclerSectionItemDecoration(int headerHeight, Boolean sticky, List<Event> eventList,Context mContext)
        {
            headerOffset = headerHeight;
            this.sticky = sticky;
            this.eventList = eventList;
            this.mContext = mContext;
        }

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            base.GetItemOffsets(outRect, view, parent, state);
            int pos = parent.GetChildAdapterPosition(view);
            if (isSection(pos))
            {
                outRect.Top = headerOffset;
            }
        }

		public override void OnDrawOver(Canvas c, RecyclerView parent, RecyclerView.State state)
		{
            base.OnDrawOver(c, parent, state);
            if (headerView == null)
            {
                headerView = inflateHeaderView(parent);
                header = (TextView)headerView.FindViewById(Resource.Id.list_section_text);
                header.SetTypeface(Utility.BoldTypeface(mContext),TypefaceStyle.Bold);
                fixLayoutSize(headerView, parent);
            }

            var previousHeader = "";
            for (int i = 0; i < parent.ChildCount; i++)
            {
                View child = parent.GetChildAt(i);
                int position = parent.GetChildAdapterPosition(child);

                var title = getSectionHeader(position);
                header.Text = title;
                if (!previousHeader.Equals(title) || isSection(position))
                {
                    drawHeader(c, child, headerView);
                    previousHeader = title;
                }
            }
		}

        private void drawHeader(Canvas c, View child, View headerView)
        {
            c.Save();
            if (sticky)
            {
                c.Translate(0, Math.Max(0, child.Top - headerView.Height));
            }
            else
            {
                c.Translate(0, child.Top - headerView.Height);
            }
            headerView.Draw(c);
            c.Restore();
        }

        private View inflateHeaderView(RecyclerView parent)
        {
            return LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RecyclerSectionHeader, parent, false);
        }

        private void fixLayoutSize(View view, ViewGroup parent)
        {
            int widthSpec = View.MeasureSpec.MakeMeasureSpec(parent.Width, MeasureSpecMode.Exactly);
            int heightSpec = View.MeasureSpec.MakeMeasureSpec(parent.Height, MeasureSpecMode.Unspecified);
            int childWidth = ViewGroup.GetChildMeasureSpec(widthSpec, parent.PaddingLeft + parent.PaddingRight, view.LayoutParameters.Width);
            int childHeight = ViewGroup.GetChildMeasureSpec(heightSpec, parent.PaddingTop + parent.PaddingBottom, view.LayoutParameters.Height);
            view.Measure(childWidth, childHeight);
            view.Layout(0, 0, view.MeasuredWidth, view.MeasuredHeight);
        }

       
        private  Boolean isSection(int position){
            if (eventList != null&&position>0&&eventList.Count>position)
            {
                
                var currentItemDate = eventList[position].StartTime.ToLocalTime().ToString("yyyy-MM");
                var previousItemDate = eventList[position - 1].StartTime.ToLocalTime().ToString("yyyy-MM");
                return position == 0 || currentItemDate != previousItemDate;

            }
            else
            {
                if (position == 0)
                {
                    return true;
                }else{
                    return false;
                }
            }
        }
        private  String getSectionHeader(int position){
            if (eventList != null&& eventList.Count > position)
            {
                return eventList[position].StartTime.ToLocalTime().ToString("MMMMMMMMMMMM yyyy");
            }
            else
            {
                return "";
            }
        }

	}
}
