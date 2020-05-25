using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace StarIndiaHoliday.Models
{
    public class ImageUpload
    {
        public static void CreateThumbnails(HttpPostedFileBase pfFileInput, string dest, int thumbWidth)
        {
            //System.Drawing.Image image = System.Drawing.Image.FromFile(src);
            HttpPostedFileBase jpeg_image_upload = pfFileInput;
            System.Drawing.Image original_image = System.Drawing.Image.FromStream(jpeg_image_upload.InputStream);

            float srcWidth = original_image.Width;
            float srcHeight = original_image.Height;

            if (srcWidth < thumbWidth)
            {
                thumbWidth = original_image.Width;
            }


            int thumbHeight = (int)((srcHeight / srcWidth) * (float)thumbWidth);

            Bitmap bmp = new Bitmap(thumbWidth, thumbHeight);

            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight);
            gr.DrawImage(original_image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);

            bmp.Save(dest);

            bmp.Dispose();
            original_image.Dispose();
        }
    }
}