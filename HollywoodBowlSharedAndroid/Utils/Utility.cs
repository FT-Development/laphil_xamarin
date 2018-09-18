using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Android.Content;
using Android.Graphics;
using Java.Lang.Reflect;
using AndroidContext = Android.App;

namespace HollywoodBowl.Droid
{
   public class Utility
    {
        public  void SetStringSharedPreference(string key, string value)
        {
            var prefs = AndroidContext.Application.Context.GetSharedPreferences(name: "LAPhil", mode: FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString(key, value);
            prefEditor.Commit();
        }
        public static String GetStringSharedPreference(string key, string defValue = null)
        {
            var prefs = AndroidContext.Application.Context.GetSharedPreferences(name: "LAPhil", mode: FileCreationMode.Private);
            var settingValue = prefs.GetString(key, defValue);
            return settingValue;
        }
        public static void SetBooleanSharedPreference(string key, Boolean value)
        {
            var prefs = AndroidContext.Application.Context.GetSharedPreferences(name: "LAPhil", mode: FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutBoolean(key, value);
            prefEditor.Commit();
        }
        public static Boolean GetBooleanSharedPreference(string key)
        {
            var prefs = AndroidContext.Application.Context.GetSharedPreferences(name: "LAPhil", mode: FileCreationMode.Private);
            var booleanValue = prefs.GetBoolean(key, false);
            return booleanValue;
        }
       
        public static Typeface RegularTypeface(Context context)
        {
            var customfont = Typeface.CreateFromAsset(context.Assets, "fonts/apercu-regular-pro.otf");
            return customfont;
        }
        public static Typeface BoldTypeface(Context context)
        {
            var customfont = Typeface.CreateFromAsset(context.Assets, "fonts/apercu-bold-pro.otf");
            return customfont;
        }
        public static Typeface MediumTypeface(Context context)
        {
            var customfont = Typeface.CreateFromAsset(context.Assets, "fonts/apercu-medium-pro.otf");
            return customfont;
        }
        public static Typeface ItalicTypeface(Context context)
        {
            var customfont = Typeface.CreateFromAsset(context.Assets, "fonts/apercu-italic-pro.otf");
            return customfont;
        }
        public static Boolean IsValidEmail(String email)
        {
            Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }

    }
}
