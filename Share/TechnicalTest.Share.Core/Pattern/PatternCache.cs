using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalTest.Share.Core.Model;

namespace TechnicalTest.Share.Core
{
    public class PatternCache : IPatternCache
    {    
        private Queue<Pattern> _patternImages;
        private Queue<Pattern> _patternColors;
        private readonly Random _colorRandomGen;
        private int _fillThrehold;


        public PatternCache(int fillThreshold)
        {
            this._patternImages = new Queue<Pattern>();
            this._patternColors = new Queue<Pattern>();
            this._colorRandomGen = new Random();
            this._fillThrehold = fillThreshold;

            FillQueues();
        }

        private void FillQueues()
        {
            Task.Factory.StartNew(() => PreFillColorQueue());
            Task.Factory.StartNew(() => PreFillImageQueue());
        }

        private void PreFillColorQueue()
        {
            Parallel.For(0, this._fillThrehold - this._patternColors.Count(), i =>
            {
                var colorHttpDownloader = new MyHttpClient();
                var colorPattern = colorHttpDownloader.DownloadColorAsync();
                this._patternColors.Enqueue(colorPattern.Result);
            });
        }

        private void PreFillImageQueue()
        {
            Parallel.For(0, this._fillThrehold - this._patternImages.Count(), i =>
            {
                var imageHttpDownloader = new MyHttpClient();
                var imagePattern = imageHttpDownloader.DownloadPatternAsync();

                this._patternImages.Enqueue(imagePattern.Result);
            });
        }

        public Pattern GetColorPattern()
        {
            if (!this._patternColors.Any())
                return new Pattern()
                {
                    PatternType = PatternType.Color,
                    ImageBytes = null,
                    ColorARBG = new Tuple<int, int, int, int>(_colorRandomGen.Next(255), _colorRandomGen.Next(255), _colorRandomGen.Next(255), _colorRandomGen.Next(255))
                };

            var q = this._patternColors.Dequeue();
            FillQueues();

            return q;
        }

        public Pattern GetImagePattern()
        {
            if (!this._patternImages.Any())
                return new Pattern()
                {
                    PatternType = PatternType.Color,
                    ImageBytes = null,
                    ColorARBG = new Tuple<int, int, int, int>(_colorRandomGen.Next(255), _colorRandomGen.Next(255), _colorRandomGen.Next(255), _colorRandomGen.Next(255))
                };

            var q = this._patternImages.Dequeue();
            FillQueues();

            return q;
        }

    }
}
