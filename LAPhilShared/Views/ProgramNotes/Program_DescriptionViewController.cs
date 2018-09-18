using System;
using UIKit;
using Foundation;
using LAPhil.Platform;
using Xamarin.Forms;


namespace LAPhil.iOS
{
    [Register("Program_DescriptionViewController")]
    public class Program_DescriptionViewController: ProgramViewController
    {
        [Outlet]
        UITextView DescriptionTextView { get; set; }

        [Outlet]
        UIView DescriptionView { get; set; }

        [Outlet]
        protected UIButton SeriesButton { get; set; }

        [Outlet]
        protected UIStackView AboutThePerformanceComponents { get; set; }

        public Program_DescriptionViewController(IntPtr handle): base(handle)
        {
        }

        protected override void ConfigureView()
        {
            base.ConfigureView();
            SeriesButton.Hidden = true;
            AboutThePerformanceComponents.RemoveArrangedSubview(SeriesButton);

            if (Event == null) { return; }

            if (Event.Description == null || Event.Description == string.Empty)
            {
                Components.RemoveArrangedSubview(DescriptionView);
                DescriptionView.Hidden = true;
            } else {
                DescriptionTextView.AttributedText = Event
                .Description
                .HtmlAttributedString(matchingTextView: DescriptionTextView);
            }

            if (Event.Series != null && 
                Event.Series.WebUrl != null &&
                Event.Series.WebUrl != string.Empty)
            {
                SeriesButton.Hidden = false;
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
                AboutThePerformanceComponents.InsertArrangedSubview(SeriesButton, stackIndex: 1);
            }
        }

        [Action("OnLearnMoreAboutTheSeries")]
        public void OnLearnMoreAboutTheSeries()
        {
            if (Event.Series.WebUrl != null && Event.Series.WebUrl != string.Empty)
            {
                Device.OpenUri(new Uri(Event.Series.WebUrl));
            }
        }
    }
}
