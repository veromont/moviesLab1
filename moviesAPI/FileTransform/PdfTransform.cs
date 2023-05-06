using moviesAPI.Interfaces;
using moviesAPI.Models.db;
using moviesAPI.Models.dbContext;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace moviesAPI.FileTransform
{
    public class PdfTransform : IFileTransform
    {
        public PdfTransform() { }

        public MemoryStream TransformTicketToPdf(Ticket ticket)
        {
            using (var ms = new MemoryStream())
            {
                var document = new PdfDocument();
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                document.Save(ms, false);
                var bytes = ms.ToArray();
                return new MemoryStream(bytes);
            }
        }
    }
}
