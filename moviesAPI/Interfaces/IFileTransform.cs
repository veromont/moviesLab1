using moviesAPI.Models.db;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace moviesAPI.Interfaces
{
    public interface IFileTransform
    {
        public MemoryStream TransformTicketToPdf(Ticket ticket);
    }
}
