using PdfSharp.Drawing;

namespace moviesAPI.FileTransform
{
    public class Cell
    {
        public Cell(XRect rectangle, string value, XFont font, XBrush color)
        {
            Rectangle = rectangle;
            Value = value;
            Font = font;
            Color = color;
        }

        public XRect Rectangle { get; set; }
        public string Value { get; set; }
        public XFont Font { get; set; }
        public XBrush Color { get; set; }
    }
}
