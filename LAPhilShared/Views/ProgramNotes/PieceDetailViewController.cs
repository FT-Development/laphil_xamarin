using UIKit;
using Foundation;
using CoreGraphics;
using System;
using LAPhil.Events;
using LAPhil.Platform;

namespace LAPhil.iOS
{
    [Register("PieceDetailViewController")]
    public class PieceDetailViewController: UIViewController
    {
        public Piece Piece { get; set; }


        [Outlet]
        public UILabel TitleLabel { get; set; }

        [Outlet]
        public UILabel ComposerLabel { get; set; }

        [Outlet]
        public UITextView DescriptionTextView { get; set; }


        [Outlet]
        public UIView TitleView{ get; set; }

        [Outlet]
        public UIView ComposerView { get; set; }

        [Outlet]
        // https://stackoverflow.com/a/45070888/1060314
        // Let it grow...
        public UIView DescriptionView { get; set; }

        [Outlet]
        public UIStackView Components { get; set; }



        public PieceDetailViewController(IntPtr handle): base(handle)
        {
        
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.ConfigureDefaultBackButton();
            ConfigureView();
        }

        public void ConfigureView()
        {
            TitleLabel.AttributedText = $"<b>{Piece.Name}</b>"
                .HtmlAttributedString(matchingLabel: TitleLabel);

            ComposerLabel.Text = Piece.ComposerName;

            var attributedString = Piece
                .Description
                .HtmlAttributedString(matchingTextView: DescriptionTextView);

            DescriptionTextView.AttributedText = attributedString;
                
        }
    }
}
