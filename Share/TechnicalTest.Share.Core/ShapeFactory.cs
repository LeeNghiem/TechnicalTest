using System;
using TechnicalTest.Share.Core.Model;

namespace TechnicalTest.Share.Core
{
    public class ShapeFactory
    {
        private readonly Random _randomGen;
        private int _minimumSize;
        private int _maximumSize;

        private IPatternCache _patternCache;

        public ShapeFactory(int fillThreshold, int minimumSize, int maximumSize, IPatternCache patternCache)
        {
            this._randomGen = new Random();
            this._minimumSize = minimumSize;
            this._maximumSize = maximumSize;
            this._patternCache = patternCache;
        }

        public Shape CreateShapeRandomly()
        {
            int randHeadTail = this._randomGen.Next(0, 2);
            Shape result;
            if (randHeadTail == 0)
            {
                result = new Circle() { Pattern = this._patternCache.GetColorPattern() };
            }
            else
            {
                result = new Square() { Pattern = this._patternCache.GetImagePattern() };
            }

            result.Size = this._randomGen.Next(this._minimumSize, this._maximumSize);

            return result;
        }

        public void RefillShapePattern(Shape shape)
        {
            shape.Pattern = shape is Circle ? this._patternCache.GetColorPattern() : this._patternCache.GetImagePattern();
        }
    }
}
