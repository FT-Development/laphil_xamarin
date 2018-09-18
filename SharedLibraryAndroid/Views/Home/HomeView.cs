using System;
using Android.App;
using Android.OS;
using Android.Views;
using LAPhil.Droid;

namespace SharedLibraryAndroid
{
    public class HomeView : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.HomeView, container, false);
        }

    }
}
