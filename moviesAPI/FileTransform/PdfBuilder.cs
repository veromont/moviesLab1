﻿using moviesAPI.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace moviesAPI.FileTransform
{
    public class PdfBuilder
    {
        public XGraphics gfx;
        PdfPage page;
        PdfDocument document;
        XRect currentRect;
        bool isRowOdd;
        public PdfBuilder(PdfDocument document) 
        {
            this.document = document;
            page = document.AddPage();
            page.Width = new XUnit(200,XGraphicsUnit.Millimeter);
            page.Height = new XUnit(180, XGraphicsUnit.Millimeter);
            gfx = XGraphics.FromPdfPage(page);

            double tableX = 50;
            double tableY = 80;
            double tableWidth = page.Width - (tableX * 2);
            double rowHeight = 30;
            double columnWidth = tableWidth / 2;

            currentRect = new XRect(tableX, tableY, columnWidth, rowHeight);
            isRowOdd = true;
        }
        public void addHeader(XFont headerFont)
        {
            gfx.DrawString("Квиток", headerFont, XBrushes.Black,
                    new XRect(0, 20, page.Width, 50),
                    XStringFormats.Center);
        }
        public void addTableRow(string name, string value)
        {
            var nameFont = new XFont("Constantia", 20, XFontStyleEx.Regular);
            var valueFont = new XFont("Arial", 12, XFontStyleEx.Regular);
            var color = XBrushes.LightGray;
            if (!isRowOdd) 
                color = XBrushes.WhiteSmoke;
            Cell leftCell = new Cell(currentRect, name, nameFont, color);
            Cell rightCell = new Cell(rectangleOnRightSide(currentRect), value, valueFont, XBrushes.White);
            drawCells(gfx, leftCell, rightCell);

            currentRect = rectangleUnderThis(currentRect);
            isRowOdd = !isRowOdd;
        }
        public void drawCells(XGraphics gfx, Cell leftCell, Cell rightCell)
        {
            gfx.DrawRectangle(leftCell.Color, leftCell.Rectangle);
            gfx.DrawRectangle(rightCell.Color, rightCell.Rectangle);
            gfx.DrawRectangle(XPens.Black, leftCell.Rectangle);
            gfx.DrawRectangle(XPens.Black, rightCell.Rectangle);
            gfx.DrawString(leftCell.Value, leftCell.Font, XBrushes.Black, leftCell.Rectangle, XStringFormats.Center);
            gfx.DrawString(rightCell.Value, rightCell.Font, XBrushes.Black, rightCell.Rectangle, XStringFormats.Center);
        }
        public XRect rectangleOnRightSide(XRect leftRect)
        {
            return new XRect(leftRect.Right, leftRect.Y, leftRect.Width, leftRect.Height);
        }
        public XRect rectangleUnderThis(XRect topRect)
        {
            return new XRect(topRect.X, topRect.Bottom, topRect.Width, topRect.Height);
        }
        public void addQrCode(string info) 
        {
            double imageX = 100;
            double imageY = 320;
            double imageWidth = 150;
            double imageHeight = 150;

            XImage image = QRGenerator.getQRCode(info);
            gfx.DrawImage(image, imageX, imageY, imageWidth, imageHeight);
        }
        public void addDateTimeSign()
        {
            double imageX = 300;
            double imageY = 401;
            double imageWidth = 200;
            double imageHeight = 100;
            var font = new XFont("Arial", 10, XFontStyleEx.Regular);
            var rectangle = new XRect(imageX, imageY, imageWidth, imageHeight);
            var date = DateTime.UtcNow.ToLocalTime();
            gfx.DrawString(date.ToString(), font, XBrushes.Black, rectangle, XStringFormats.Center);
        }
    }
}
