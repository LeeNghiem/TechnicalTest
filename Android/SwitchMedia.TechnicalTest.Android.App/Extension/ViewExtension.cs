using Android.Views;
using Android.Widget;

namespace TechnicalTest.Android.App.Extension
{
    public static class ViewExtension
    {
        //Set coordinate of the view in parent layout    
		public static void SetLayoutCoordinate(this View view,int x, int y, int size)
        {
            var param = new RelativeLayout.LayoutParams(size, size);
			param.SetMargins(x - size/2 + view.Width/2 , y - view.Height - size/2 , 0, 0);
            view.LayoutParameters = param;
        }
    }
}
