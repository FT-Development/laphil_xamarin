using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Telephony;

namespace SharedLibraryAndroid
{
    public class AndroidInfo
    {
        private static bool IsNetworkAvailable()
        {
            bool isNetworkActive;// = false;
            Context context = Application.Context;//get the current application context
            ConnectivityManager cm = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            NetworkInfo activeConnection = cm.ActiveNetworkInfo;
            isNetworkActive = (activeConnection != null) && activeConnection.IsConnected;
            return isNetworkActive;
        }

        #region SystemInfo
        public bool IsInternetAvailble
        {
            get { return IsNetworkAvailable(); }
        }
        public string SystemFamily
        {
            get
            {
                string deviceType = "Mobile";
                var context = Application.Context;
                var manager = context.GetSystemService(Context.TelephonyService)
                       as TelephonyManager;

                if (manager == null)
                {
                    deviceType = "Unknown";
                }
                else if (manager.PhoneType == PhoneType.None)
                {
                    deviceType = "Tablet";
                }
                else
                {
                    deviceType = "Mobile";
                }
                return deviceType;
            }
        }
        public string SystemVersion
        {
            get
            {
                return Build.VERSION.Release;
            }
        }
        public string SystemArchitecture { get { return Build.CpuAbi; } }
        public string ApplicationName
        {
            get
            {
                var context = Application.Context;
                return context.PackageManager.GetPackageInfo(context.PackageName, 0).PackageName;
            }
        }
        public string ApplicationVersion
        {
            get
            {
                var context = Application.Context;
                return context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
            }
        }
        public string DeviceManufacturer
        {
            get
            {
                return Build.Manufacturer;
            }
        }
        public string DeviceModel { get { return Build.Model; } }
        public string DeviceType
        {
            get
            {
                string deviceType = "Mobile";
                var context = Application.Context;
                var manager = context.GetSystemService(Context.TelephonyService)
                       as TelephonyManager;
                if (manager == null)
                {
                    deviceType = "Unknown";
                }
                else if (manager.PhoneType == PhoneType.None)
                {
                    deviceType = "Tablet";
                }
                else
                {
                    deviceType = "Mobile";
                }
                return deviceType;
            }
        }
        public string FreeSpace()
        {
            var activityManager = (ActivityManager)Application.Context.GetSystemService(Context.ActivityService);
            ActivityManager.MemoryInfo memInfo = new ActivityManager.MemoryInfo();
            activityManager.GetMemoryInfo(memInfo);
            var free = (memInfo.AvailMem / 1024 / 1024);
            return free.ToString();
        }
        #endregion
    }
}
