using moviesAPI.Interfaces;
using moviesAPI.Models.db;
using moviesAPI.Models.dbContext;
using NPOI.SS.Formula.Functions;
using NPOI.XWPF.UserModel;
using Org.BouncyCastle.Asn1.Pkcs;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SixLabors.ImageSharp;
using System.Drawing;
using System.Drawing.Text;

namespace moviesAPI.FileTransform
{
    public class PdfTransform : IFileTransform
    {
        public PdfTransform() { }
        private void AddTableRow(pdfContext info)
        {
            XGraphics gfx = info.gfx;
            pdfCoordinates coordinates = info.coordinates;
            XFont font = info.font;
            XSolidBrush color = info.color;
            double tableX = coordinates.tableX;
            double tableY = coordinates.tableY;
            double tableWidth = coordinates.tableWidth;
            double rowHeight = coordinates.rowHeight;
            double columnWidth = coordinates.columnWidth;

            XRect ticketNumberRect = new XRect(tableX, tableY, columnWidth, rowHeight);
            XRect ticketNumberValueRect = new XRect(ticketNumberRect.Right, tableY, columnWidth, rowHeight);
            gfx.DrawRectangle(color, ticketNumberRect);
            gfx.DrawRectangle(XPens.Black, ticketNumberRect);
            gfx.DrawRectangle(XPens.Black, ticketNumberValueRect);
            gfx.DrawString("Ticket Number", font, XBrushes.Black, ticketNumberRect, XStringFormats.Center);
            gfx.DrawString("1", font, XBrushes.Black, ticketNumberValueRect, XStringFormats.Center);
        }
        public MemoryStream TransformTicketToPdf(Ticket ticket)
        {
            using (var ms = new MemoryStream())
            {
                var document = new PdfDocument();
                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Constantia", 24, XFontStyleEx.Bold);

                // Draw the "Ticket" header at the top center of the page
                gfx.DrawString("Ticket", font, XBrushes.Black,
                    new XRect(0, 20, page.Width, 50),
                    XStringFormats.Center);

                var pdfCoordinates = new pdfCoordinates();
                double tableX = 50;
                double tableY = 80;
                double tableWidth = page.Width - (tableX * 2);
                double rowHeight = 30;
                double columnWidth = tableWidth / 2;
                pdfCoordinates.columnWidth = columnWidth;
                pdfCoordinates.rowHeight = rowHeight;
                pdfCoordinates.tableWidth = tableWidth;
                pdfCoordinates.tableX = tableX;
                pdfCoordinates.tableY = tableY;

                pdfContext context = new pdfContext();
                context.font = font;
                context.gfx = gfx;
                context.color = XBrushes.LightGray;
                context.coordinates = pdfCoordinates;
                AddTableRow(context);
                XRect passengerNameRect = new XRect(tableX, tableY + rowHeight, columnWidth, rowHeight);
                gfx.DrawRectangle(XBrushes.WhiteSmoke, passengerNameRect);
                gfx.DrawString("Passenger Name", font, XBrushes.Black, passengerNameRect, XStringFormats.Center);
                gfx.DrawString("John", font, XBrushes.Black, new XRect(passengerNameRect.Right, passengerNameRect.Top, columnWidth, rowHeight), XStringFormats.Center);
                XRect departureTimeRect = new XRect(tableX, passengerNameRect.Bottom, columnWidth, rowHeight);
                gfx.DrawRectangle(XBrushes.LightGray, departureTimeRect);
                gfx.DrawString("Departure Time", font, XBrushes.Black, departureTimeRect, XStringFormats.Center);
                gfx.DrawString("some info", font, XBrushes.Black, new XRect(departureTimeRect.Right, departureTimeRect.Top, columnWidth, rowHeight), XStringFormats.Center);

                /////
                document.Save(ms, false);
                var bytes = ms.ToArray();
                return new MemoryStream(bytes);
            }
        }
    }
}
