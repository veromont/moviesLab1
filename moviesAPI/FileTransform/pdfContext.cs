using PdfSharp.Drawing;

namespace moviesAPI.FileTransform
{
    public class pdfCoordinates
    {
        public double tableX { get; set; }
        public double tableY { get; set; }
        public double tableWidth { get; set; }
        public double rowHeight { get; set; }
        public double columnWidth { get; set; }
    }
    public class pdfContext
    {
        public XGraphics gfx { get; set; }
        public pdfCoordinates coordinates { get; set; }
        public XFont font { get; set; }
        public XSolidBrush color { get; set; }
    }
}
