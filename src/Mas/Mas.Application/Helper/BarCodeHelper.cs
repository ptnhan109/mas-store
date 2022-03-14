using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.QrCode;

namespace Mas.Application.Helper
{
    public class BarCodeHelper
    {
        public static async Task GenerateBarCode(string data)
        {
            var barcodeData = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new QrCodeEncodingOptions
                {
                    Height = 90,
                    Width = 240,
                    Margin = 6,
                    CharacterSet = data
                }
            };

            var pixelData = barcodeData.Write(data);

            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                using (var memoryStream = new MemoryStream())
                {
                    var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                    try
                    {
                        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                    }
                    finally
                    {
                        bitmap.UnlockBits(bitmapData);
                    }
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                    string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","assets", "barcode", "barcode.png");

                    await File.WriteAllBytesAsync(path, memoryStream.ToArray());
                }
            }
        }
    }
}
