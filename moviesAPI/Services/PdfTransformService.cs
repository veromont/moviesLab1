﻿using moviesAPI.FileTransform;
using moviesAPI.Interfaces;
using moviesAPI.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace moviesAPI.Services
{
    public class PdfTransformService : IPdfTransformService
    {
        PdfBuilder builder;
        PdfDocument document;
        public PdfTransformService()
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

                builder.addTableRow(PdfTicketModel.TranslationMap[nameof(ticket.MovieTitle)], ticket.MovieTitle);
                builder.addTableRow(PdfTicketModel.TranslationMap[nameof(ticket.SeatNumber)], ticket.SeatNumber.ToString());
                builder.addTableRow(PdfTicketModel.TranslationMap[nameof(ticket.Price)], ticket.Price.ToString());
                builder.addTableRow(PdfTicketModel.TranslationMap[nameof(ticket.HallName)], ticket.HallName.ToString());
                builder.addTableRow(PdfTicketModel.TranslationMap[nameof(ticket.StartTime)], ticket.StartTime.ToString());

                builder.addQrCode($"Ticket id = {ticket.Id}\nSession id = {ticket.SessionId}");

                builder.addDateTimeSign();

                document.Save(ms, false);
                var bytes = ms.ToArray();
                return new MemoryStream(bytes);
            }
        }
    }
}