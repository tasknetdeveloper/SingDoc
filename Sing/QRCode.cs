using QRCoder;
using System.Drawing;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using LoggerSpace;
namespace SingSpace
{

    public class QRCodeUtil
    {
        private Log log = new(true);
        public QRCodeUtil(Log log)
        {
            this.log = log;
        }

        //https://www.e-iceblue.com/Tutorials/Spire.PDF/Spire.PDF-Program-Guide/PDF-Image-Draw-PDF-Image-in-C-VB.NET.html
        public bool AddCodeToFile(string path, string[] text, int x,int y)
        {
            var result=false;
            
            if (text == null || string.IsNullOrEmpty(path))
                return result;

            try
            {
                var img = CreateQRCode(text);
                if (img == null) return result;

                var doc = new PdfDocument();
                doc.LoadFromFile(path);

                //adjust image size
                /*
                int width = img.Width;
                int height = img.Height;
                float schale = 1.5f;

                Size size = new Size((int)(width * schale), (int)(height * schale));
                Bitmap schaleImage = new Bitmap(img, size);
                if(schaleImage==null)return result;
                */
                //insert image into the first PDF page at specific position
                PdfImage pdfImage = PdfImage.FromImage(img);//(schaleImage);
                PdfPageBase page0 = doc.Pages[0];

                PointF position = new PointF(x, y);///((page0.Canvas.ClientSize.Width - schaleImage.Width) / 2, 210);
                page0.Canvas.SetTransparency(0.5f);

                page0.Canvas.DrawImage(pdfImage, position);

                page0.Canvas.SetTransparency(1.0f);
                doc.SaveToFile(path);
            }
            catch (Exception exp)
            {
                log.Error(exp.Message);
            }
            return result;
        }

        private Bitmap? CreateQRCode(string[] text)
        {
            Bitmap? result = null;
            if (text == null)
                return result;
            var s = "";
            foreach (var item in text)
            {
                s = s + $"\r\n{item}";
            }

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(s, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            result = qrCode.GetGraphic(20);
            return result;
        }
    }
}
