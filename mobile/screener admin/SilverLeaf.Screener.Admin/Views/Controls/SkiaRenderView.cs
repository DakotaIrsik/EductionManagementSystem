using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.Views.Controls
{
    class SkiaRenderView : SKCanvasView
    {
        public static readonly BindableProperty RendererProperty = BindableProperty.Create(
             nameof(Renderer),
             typeof(Renderers.IRenderer),
             typeof(SkiaRenderView),
             null,
             defaultBindingMode: BindingMode.TwoWay,
             propertyChanged: (bindable, oldValue, newValue) =>
             {
                 ((SkiaRenderView)bindable).RendererChanged((Renderers.IRenderer)oldValue, (Renderers.IRenderer)newValue);
             });

        void RendererChanged(Renderers.IRenderer currentRenderer, Renderers.IRenderer newRenderer)
        {
            if (currentRenderer != newRenderer)
            {
                if (currentRenderer != null)
                    currentRenderer.RefreshRequested -= Renderer_RefreshRequested;

                if (newRenderer != null)
                    newRenderer.RefreshRequested += Renderer_RefreshRequested;

                InvalidateSurface();
            }
        }

        void Renderer_RefreshRequested(object sender, EventArgs e)
        {
            InvalidateSurface();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            Renderer.PaintSurface(e.Surface, e.Info);
        }

        public Renderers.IRenderer Renderer
        {
            get { return (Renderers.IRenderer)GetValue(RendererProperty); }
            set { SetValue(RendererProperty, value); }
        }
    }
}
