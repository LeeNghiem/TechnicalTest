using System;

using Android.Views;
using Android.Content;
using Android.Util;
using Android.Widget;
using Android.Graphics;

using TechnicalTest.Android.App.Extension;
using TechnicalTest.Share.Core;
using TechnicalTest.Android.App.CustomView;

namespace TechnicalTest.Android.App.CustomLayout
{
    /// <summary>
    /// This layout is single custom layout view for Technical Test Android Application.
    /// It will add ShapeView at clicked coordinate
    /// </summary>
    public class ShapeLayout : RelativeLayout
    {
		private readonly IPatternCache _patternCache;
        private const int FILL_THRESHOLD = 3;

        public ShapeLayout(Context context, IAttributeSet attr)
            : base(context, attr)
        {
            this.Touch += ShapeController_Touch;
			this._patternCache = new PatternCache (FILL_THRESHOLD);
        }

        private void ShapeController_Touch(object sender, TouchEventArgs e)
        {
            // Add ShapeView when clicked.
			if (e.Event.Action == MotionEventActions.Down) {

				var clickedX = Convert.ToInt32 (e.Event.GetX ());
				var clickedY = Convert.ToInt32 (e.Event.GetY ());

				var shapeView = new ShapeView (this.Context, this._patternCache);
				shapeView.SetLayoutCoordinate(clickedX, clickedY, shapeView.Model.Size);

                this.AddView (shapeView);

				e.Handled = true;
			}
		}

    }
}

