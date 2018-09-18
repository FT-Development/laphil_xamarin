using System;
using UIKit;
using Foundation;
using System.Collections.Generic;

namespace LAPhil.iOS
{
    [Register("Program_ActionViewController")]
    public class Program_ActionViewController: ProgramViewController
    {
        [Outlet]
        UIButton btnOnAboutThePerformance { get; set; }

        [Outlet]
        UIView AboutThePerformanceView { get; set; }


        public Program_ActionViewController(IntPtr handle): base(handle)
        {
        }

        protected override void ConfigureView()
        {
            base.ConfigureView();

            // only show if we have a Event.Description
            AboutThePerformanceView.Hidden = true;

            if (Event.Description == null || Event.Description == string.Empty)
            {
                Components.RemoveArrangedSubview(AboutThePerformanceView);
                AboutThePerformanceView.Hidden = true;
            }
            else
            {
                AboutThePerformanceView.Hidden = false;
            }
        }

        [Action("OnAboutThePerformance")]
        void OnAboutThePerformance()
        {
            ShowAboutThePerformance();
        }

        void ShowAboutThePerformance()
        {
            var vc = (AboutThePerformanceViewController)Storyboard.InstantiateViewController("AboutThePerformanceViewController");
            vc.Event = Event;
            NavigationController.PushViewController(vc, animated: true);

            Dictionary<object, object> parameters = new Dictionary<object, object>();
            parameters.Add("Program", Event.Program.Name);
            Firebase.Analytics.Analytics.LogEvent("AboutPerformance", parameters);

        }

    }
}
