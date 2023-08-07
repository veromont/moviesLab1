using moviesAPI.FileTransform;
using moviesAPI.Interfaces;
using moviesAPI.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace moviesAPI.Services
{
    public class PdfTransform : IPdfTransformService
    {
        PdfBuilder builder;
        PdfDocument document;
        public PdfTransform()
        {
            document = new PdfDocument();
            builder = new PdfBuilder(document);
        }

        public MemoryStream TransformTicketToPdf(PdfTicketModel ticket)
        {
            using (var ms = new MemoryStream())
            {

                var headerFont = new XFont("Constantia", 24, XFontStyleEx.Bold);
                var regularFont = new XFont("Constantia", 20, XFontStyleEx.Regular);

                builder.addHeader(headerFont);

                builder.addTableRow("Назва фільму", ticket.MovieTitle);
                builder.addTableRow("Номер сидіння", ticket.SeatNumber.ToString());
                builder.addTableRow("Ціна", ticket.Price.ToString());
                builder.addTableRow("Зал", ticket.HallName.ToString());
                builder.addTableRow("Початок сеансу", ticket.StartTime.ToString());

                builder.addQrCode($"Ticket id = {ticket.Id}\nSession id = {ticket.SessionId}");

                builder.addDateTimeSign();

                document.Save(ms, false);
                var bytes = ms.ToArray();
                return new MemoryStream(bytes);
            }
        }
    }
}
