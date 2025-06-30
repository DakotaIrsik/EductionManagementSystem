using PdfSharp.Drawing;

namespace SilverLeaf.Core.Objects
{
    public class PDFDrawableBox
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height => 0;

        public XRect Rect => new XRect(X, Y, Width, Height);

        public XFont Font { get; set; } = new XFont("Arial", 10, XFontStyle.Regular);

        public string Text { get; set; }

    }
}
