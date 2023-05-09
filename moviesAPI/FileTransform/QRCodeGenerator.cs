using PdfSharp.Drawing;
using QRCoder;
using System.Drawing;

namespace moviesAPI.FileTransform
{
    public class QRGenerator
    {
        public static XImage getQRCode(string info)
        {
            // Generate QR code using QRCoder
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(info, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            // Convert QR code to XImage
            Bitmap bitmap = qrCode.GetGraphic(30);
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                var image = XImage.FromStream(stream);
                return image;
            }
        }
    }
}
