using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Widget;

namespace HollywoodBowl.Droid
{
   public class NearDiningView : Fragment
    {
        private Context mContext;
        private NearDiningAdapter _Adapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.NearDiningView, container, false);
            mContext = this.Activity;
            _Adapter = new NearDiningAdapter();
            RecyclerView diningRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.rv_dining);
            var btnBack  = view.FindViewById<ImageView>(Resource.Id.btnBack);
            btnBack.Click += (sender,e) => 
            { 
                FragmentManager.PopBackStack();
            };
            // instantiate the layout manager
            LinearLayoutManager layoutManager = new LinearLayoutManager(mContext);
            diningRecyclerView.SetLayoutManager(layoutManager);

            // set RecyclerView's adapter
            diningRecyclerView.SetAdapter(_Adapter);
            return view.RootView;

        }
    }

}
