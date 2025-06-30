using SkiaSharp;
using System;

namespace SilverLeaf.Screener.Admin.Views.Renderers
{
    class ProgressRenderer : IRenderer
    {
        public int PercentageAlreadyCompleted { get; set; }

        public int RecentlyCompleted { get; set; }

        public int ToBeCompleted { get; set; }

        public int NewPercentageCompleted
        {
            get
            {
                var percentage = (int)Math.Floor(((double)RecentlyCompleted / (double)ToBeCompleted) * 100);
                return percentage;

            }
        }

        private int Width { get; set; }

        private int Height { get; set; }

        public ProgressRenderer()
        {

        }

        public void Refresh(int percentageAlreadyCompleted, int recentlyCompleted, int toBeCompleted)
        {
            PercentageAlreadyCompleted = percentageAlreadyCompleted;
            RecentlyCompleted = recentlyCompleted;
            ToBeCompleted = toBeCompleted;
            RefreshRequested?.Invoke(null, new EventArgs());
        }

        public void PaintSurface(SKSurface surface, SKImageInfo info)
        {
            Width = info.Width;
            Height = info.Height;

            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            SKPaint blueFill = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = FillColor
            };

            SKPaint background = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = new SKColor(0, 0, 0)
            };

            SKPaint greenFill = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = new SKColor(0, 255, 0)
            };
            var alreadyFilled = PercentageAlreadyCompleted * (Width / 100);
            var leftToFill = Width - alreadyFilled;
            int toBeFilled = 0;
            if (NewPercentageCompleted != 0 && NewPercentageCompleted != int.MinValue)
            {
                toBeFilled = (int)Math.Floor((double)NewPercentageCompleted / (double)100 * (double)leftToFill);
            }


            canvas.DrawRect(1, 1, alreadyFilled, Height, blueFill);
            //canvas.DrawRect(1, 1, Width, Height, background);
            canvas.DrawRect(alreadyFilled, 1, toBeFilled, Height, greenFill);
        }

        SKColor _fillColor = new SKColor(0, 0, 255);
        public SKColor FillColor
        {
            get => _fillColor;
            set
            {
                if (_fillColor != value)
                {
                    _fillColor = value;
                    RefreshRequested?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler RefreshRequested;
    }
}
