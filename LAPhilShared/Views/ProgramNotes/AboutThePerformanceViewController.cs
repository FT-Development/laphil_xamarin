using UIKit;
using Foundation;
using System;
using System.Linq;
using LAPhil.Events;
using LAPhil.Platform;
using Xamarin.Forms;


namespace LAPhil.iOS
{
    [Register("AboutThePerformanceViewController")]
    public class AboutThePerformanceViewController: UIViewController
    {
        public Event Event { get; set; }

        [Outlet]
        public UILabel TitleLabel { get; set; }

        [Outlet]
        public UIButton SeriesButton { get; set; }

        [Outlet]
        public UIView SeriesView{ get; set; }

        [Outlet]
        public UIStackView Components { get; set; }

        [Outlet]
        // https://stackoverflow.com/a/45070888/1060314
        // Let it grow...
        public UITextView DescriptionTextView { get; set; }

        [Outlet]
        public NSLayoutConstraint DescriptionTextViewHeight { get; set; }

        public AboutThePerformanceViewController(IntPtr handle): base(handle)
        {
            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.ConfigureDefaultBackButton();

            // Available Font Names: 
            // ApercuPro-BoldItalic
            // ApercuPro-LightItalic
            // ApercuPro-BlackItalic
            // ApercuPro-Mono
            // ApercuPro-MonoBold
            // ApercuPro-MediumItalic
            // ApercuPro-Black
            // ApercuPro-Medium
            // ApercuPro-Regular
            // ApercuPro-Light
            // ApercuPro-Italic
            // ApercuPro-Bold
            TitleLabel.AttributedText = ($"<b>{Event.Program.Name}</b>")
                .HtmlAttributedString(matchingLabel: TitleLabel);

            var attributedString = Event
                .Description
                .HtmlAttributedString(matchingTextView: DescriptionTextView);

            DescriptionTextView.AttributedText = attributedString;

            SeriesView.Hidden = true;
            Components.RemoveArrangedSubview(SeriesView);

            if (Event.Series != null && string.IsNullOrEmpty(Event.Series.WebUrl) == false)
            {
                //SeriesButton.NumberOfLines = 0;
                SeriesButton.LineBreakMode = UILineBreakMode.WordWrap;
                SeriesButton.SetAttributedTitle(
                    $"PART OF {Event.Series.Name.ToUpper()}"
                    .HtmlAttributedString(
                        matchingButton: SeriesButton,
                        controlState: UIControlState.Normal
                    )
                    , UIControlState.Normal);
                SeriesButton.SizeToFit();
                SeriesView.Hidden = false;
                Components.InsertArrangedSubview(SeriesView, stackIndex: 1);
            }
        }

        [Action("OnLearnMoreAboutTheSeries")]
        public void OnLearnMoreAboutTheSeries()
        {
            if (Event.Series != null && string.IsNullOrEmpty(Event.Series.WebUrl) == false)
            {
                Device.OpenUri(new Uri(Event.Series.WebUrl));
            }
        }
    }
}
