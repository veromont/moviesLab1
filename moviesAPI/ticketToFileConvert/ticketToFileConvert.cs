using NPOI.XWPF.UserModel;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace moviesAPI.ticketToFileConvert
{
    public class TicketToFileConvert
    {
        public TicketToFileConvert() { }
        public byte[] GeneratePdf()
        {
            using (var ms = new MemoryStream())
            {
                var document = new PdfDocument();
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);

                // Draw some text
                var font = new XFont("Arial", 12);
                gfx.DrawString("Hello, world!", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

                // Save the document
                document.Save(ms, true);
                return ms.ToArray();
            }
        }
    }
}
