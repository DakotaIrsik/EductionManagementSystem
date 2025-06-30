using SkiaSharp;
using System;

namespace SilverLeaf.Screener.Admin.Views.Renderers
{
    public interface IRenderer
    {
        void PaintSurface(SKSurface surface, SKImageInfo info);

        event EventHandler RefreshRequested;

        void Refresh(int percentageAlreadyCompleted, int recentlyCompleted, int toBeCompleted);
    }
}
