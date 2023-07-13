using moviesAPI.Models;

namespace moviesAPI.Interfaces
{
    public interface IPdfTransformService
    {
        public MemoryStream TransformTicketToPdf(PdfTicketModel ticket);
    }
}
