using Android.Views;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;

using TechnicalTest.Share.Core.Model;
using TechnicalTest.Android.App.Extension;
using TechnicalTest.Share.Core;

namespace TechnicalTest.Android.App.CustomView
{
    public class ShapeView : View
    {
        private const int MIN_SIZE = 100;
        private const int MAX_SIZE = 500;
        private const int FILL_THRESHOLD = 10;
        private const int DOUBLE_CLICK_TIME = 1000; //1 sec

        public TechnicalTest.Share.Core.Model.Shape Model
        {
            get
            {
                return _shapeModel;
            }
        }

        private Share.Core.Model.Shape _shapeModel;
        private ShapeFactory _shapeFactory;
        private Paint _paint;
        private ShapeDrawable _drawableShape;
        private float _viewX;
        private float _viewY;
        private System.Timers.Timer _timer;

        public ShapeView(Context context, IPatternCache cache)
            : base(context)
        {

            this._shapeFactory = new ShapeFactory(FILL_THRESHOLD, MIN_SIZE, MAX_SIZE, cache);
            this._shapeModel = this._shapeFactory.CreateShapeRandomly();
            this._timer = new System.Timers.Timer(DOUBLE_CLICK_TIME);
            this._timer.Elapsed += (sender, e) => { this._timer.Stop(); };
            this._drawableShape = this._shapeModel is Circle ? new ShapeDrawable(new OvalShape()) : new ShapeDrawable(new RectShape());
            this._drawableShape.SetBounds(0, 0, this._shapeModel.Size, this._shapeModel.Size);

            this.Touch += ShapeView_Touch;

            this._paint = new Paint();
            this._paint.SetStyle(Paint.Style.Fill);
            RepaintShapeColor();
        }

        private void ShapeView_Touch(object sender, TouchEventArgs e)
        {

            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    if (!this._timer.Enabled)
                    {
                        this._timer.Start();
						_viewX = e.Event.GetX();
						_viewY = e.Event.GetY();
                    }
                    else
                    {
                        this._timer.Stop();
                        OnDoubleClicked();
                    }

                    break;

                case MotionEventActions.Move:
                    //Drag along with cursor
                    var left = (int)(e.Event.RawX - _viewX);
                    var top = (int)(e.Event.RawY - _viewY + this.Height / 2);
                    this.SetLayoutCoordinate(left, top, this._shapeModel.Size);
                    break;
            }

            e.Handled = true;
        }

        protected override void OnDraw(Canvas canvas)
        {
            this._drawableShape.Draw(canvas);
        }

        //Handle when view get double clicked
        private void OnDoubleClicked()
        {
            this._shapeFactory.RefillShapePattern(this._shapeModel);
            RepaintShapeColor();
        }

        //Paint view base on pattern of shapeModel
        private void RepaintShapeColor()
        {
            if (this._shapeModel.Pattern.PatternType == PatternType.Color)
            {
                _paint.SetARGB(this._shapeModel.Pattern.ColorARBG.Item1, this._shapeModel.Pattern.ColorARBG.Item2,
                    this._shapeModel.Pattern.ColorARBG.Item3, this._shapeModel.Pattern.ColorARBG.Item4);
            }
            else
            {
                Bitmap bitmap = BitmapFactory.DecodeByteArray(this._shapeModel.Pattern.ImageBytes, 0, this._shapeModel.Pattern.ImageBytes.Length);
                BitmapShader shader = new BitmapShader(bitmap, Shader.TileMode.Repeat, Shader.TileMode.Repeat);
                this._paint.SetShader(shader);
            }

            this._drawableShape.Paint.Set(_paint);
            this.Invalidate();
        }

    }
}