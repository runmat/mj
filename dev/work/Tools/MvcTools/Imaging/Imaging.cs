using System;
using System.Web;
using System.Web.Caching;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using MvcTools.Web;

namespace MvcTools.Imaging
{
    public class Imaging
    {
        public static string GetExtensionByImageFormat(ImageFormat format)
        {
            string ctype = "jpg";

            if (format.Equals(ImageFormat.Gif))
            {
                ctype = "gif";
            }
            else if (format.Equals(ImageFormat.Jpeg))
            {
                ctype = "jpg";
            }
            else if (format.Equals(ImageFormat.Png))
            {
                ctype = "png";
            }
            else if (format.Equals(ImageFormat.Bmp) || format.Equals(ImageFormat.MemoryBmp))
            {
                ctype = "bmp";
            }
            else if (format.Equals(ImageFormat.Tiff))
            {
                ctype = "tif";
            }

            return ctype;
        }

        public static string GetContentTypeByImageFormat(ImageFormat format)
        {
            string ctype = "image/x-unknown";

            if (format.Equals(ImageFormat.Gif))
            {
                ctype = "image/gif";
            }
            else if (format.Equals(ImageFormat.Jpeg))
            {
                ctype = "image/jpeg";
            }
            else if (format.Equals(ImageFormat.Png))
            {
                ctype = "image/png";
            }
            else if (format.Equals(ImageFormat.Bmp) || format.Equals(ImageFormat.MemoryBmp))
            {
                ctype = "image/bmp";
            }
            else if (format.Equals(ImageFormat.Icon))
            {
                ctype = "image/x-icon";
            }
            else if (format.Equals(ImageFormat.Tiff))
            {
                ctype = "image/tiff";
            }

            return ctype;
        }

        public static ImageFormat GetImageFormatByContentType(string contentType)
        {
            ImageFormat format = null;

            if (contentType != null)
            {
                if (contentType.Equals("image/gif"))
                {
                    format = ImageFormat.Gif;
                }
                else if (contentType.Equals("image/jpeg") || contentType.Equals("image/pjpeg"))
                {
                    format = ImageFormat.Jpeg;
                }
                else if (contentType.Equals("image/png"))
                {
                    format = ImageFormat.Png;
                }
                else if (contentType.Equals("image/bmp"))
                {
                    format = ImageFormat.Bmp;
                }
                else if (contentType.Equals("image/x-icon"))
                {
                    format = ImageFormat.Icon;
                }
                else if (contentType.Equals("image/tiff"))
                {
                    format = ImageFormat.Tiff;
                }
            }

            return format;
        }

        public static string GetFileExtensionByContentType(string contentType)
        {
            string ext = "bin";

            if (contentType.Equals("image/gif"))
            {
                ext = "gif";
            }
            else if (contentType.Equals("image/jpeg") || contentType.Equals("image/pjpeg"))
            {
                ext = "jpg";
            }
            else if (contentType.Equals("image/png"))
            {
                ext = "png";
            }
            else if (contentType.Equals("image/bmp"))
            {
                ext = "bmp";
            }
            else if (contentType.Equals("image/x-icon"))
            {
                ext = "ico";
            }
            else if (contentType.Equals("image/tiff"))
            {
                ext = "tif";
            }

            return ext;
        }

        public static byte[] BytesFromImage(Image image)
        {
            return BytesFromImage(image, ImageFormat.Jpeg);
        }
        public static byte[] BytesFromImage(Image image, ImageFormat imgFormat)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, imgFormat);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] imageBytes = new byte[ms.Length];
            ms.Read(imageBytes, 0, (int)ms.Length);
            return imageBytes;
        }

        public static byte[] CreateEmptyHintImage(string cultureEmptyImageHint, string textcolor, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(b);
            g.Clear(ColorTranslator.FromHtml("#F0F0F0"));
            g.DrawRectangle(Pens.DarkGray, new Rectangle(0, 0, b.Width - 1, b.Height - 1));
            StringFormat sf = new StringFormat(StringFormat.GenericDefault);
            sf.Alignment = StringAlignment.Center;

            //string cultureEmptyImageHint = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["hintEmptyImage"].ToString());
            Font f = new Font("Verdana", 8.0f, FontStyle.Italic);
            SizeF size = g.MeasureString(cultureEmptyImageHint, f, new SizeF(b.Width - 4, b.Height), sf);
            SolidBrush br = new SolidBrush(ColorTranslator.FromHtml("#" + textcolor));
            g.DrawString(cultureEmptyImageHint, f, br, new RectangleF(2, b.Height / 2 - size.Height / 2, b.Width - 4, size.Height), sf);
            br.Dispose();

            g.Dispose();

            MemoryStream ms = new MemoryStream();
            b.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public static Image ImageFromBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }
            else
            {
                MemoryStream ms = new MemoryStream(bytes);
                Image img = null;
                try
                {
                    img = Image.FromStream(ms);
                }
                catch { }
                return img;
            }
        }

        public static byte[] ScaleImage(byte[] bytes, int maxWidth, int maxHeight)
        {
            return ScaleImage(bytes, maxWidth, maxHeight, ImageFormat.Jpeg);
        }

        public static byte[] ScaleImage(byte[] bytes, int maxWidth, int maxHeight, ImageFormat format)
        {
            try
            {
                using (Image img = Image.FromStream(new MemoryStream(bytes)))
                {
                    if (format == null)
                    {
                        format = img.RawFormat;
                    }
                    if (img.Size.Width > maxWidth || img.Size.Height > maxHeight)
                    {
                        //resize the image to fit our website's required size
                        int newWidth = img.Size.Width;
                        int newHeight = img.Size.Height;
                        if (newWidth > maxWidth)
                        {
                            newWidth = maxWidth;
                            newHeight = (int)(newHeight * ((float)newWidth / img.Size.Width));
                        }
                        if (newHeight > maxHeight)
                        {
                            newHeight = maxHeight;
                            newWidth = img.Size.Width;
                            newWidth = (int)(newWidth * ((float)newHeight / img.Size.Height));
                        }

                        //resize the image to fit in the allowed image size
                        //bool indexed;
                        Bitmap newImage;
                        if (img.PixelFormat == PixelFormat.Format1bppIndexed || img.PixelFormat == PixelFormat.Format4bppIndexed || img.PixelFormat == PixelFormat.Format8bppIndexed || img.PixelFormat == PixelFormat.Indexed)
                        {
                            //indexed = true;
                            newImage = new Bitmap(newWidth, newHeight);
                        }
                        else
                        {
                            //indexed = false;
                            newImage = new Bitmap(newWidth, newHeight, img.PixelFormat);
                        }
                        using (newImage)
                        {
                            using (Graphics g = Graphics.FromImage(newImage))
                            {
                                //if (indexed)
                                {
                                    g.FillRectangle(Brushes.Transparent, 0, 0, newWidth, newHeight);
                                }
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                g.CompositingQuality = CompositingQuality.HighQuality;

                                g.DrawImage(img, new Rectangle(-1, -1, newWidth + 1, newHeight + 1));
                            }
                            using (MemoryStream ms = new MemoryStream())
                            {
                                newImage.Save(ms, format);
                                bytes = ms.ToArray();
                            }
                        }
                    }
                    else if (img.RawFormat != format)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            img.Save(ms, format);
                            bytes = ms.ToArray();
                        }
                    }
                    return bytes;
                }
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        private static void WriteWtImage(HttpContext context)
        {
            Bitmap b = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(b);
            g.Clear(Color.Transparent);
            g.Dispose();
            byte[] ibytes = BytesFromImage(b, ImageFormat.Png);
            context.Response.BinaryWrite(ibytes);
            context.Response.Cache.SetNoStore();
            b.Dispose();
        }

        /// <summary>
        /// Text image
        /// </summary>
        /// <param name="context"></param>
        public static void ProcessRequest_TextImage(HttpContext context)
        {
            string rawText = context.Request.QueryString["text"].ToString();
            string text = context.Server.HtmlDecode(rawText);
            string cachekey = "text_" + text.Substring(0, (text.Length > 20 ? 20 : text.Length - 1));
            ImageCacheEntry images = (ImageCacheEntry)context.Cache[cachekey];

            int width = context.GetRequestInt("width", 200);
            int height = context.GetRequestInt("height", 16);

            if (images == null)
            {
                //get image bytes
                Bitmap b = CreateTextImage(width, height, text);
                byte[] bytes = BytesFromImage(b, ImageFormat.Png);

                images = new ImageCacheEntry(bytes);

                //cache image
                context.Cache.Add(cachekey, images, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(1), CacheItemPriority.Low, null);
            }
            if (images != null)
            {
                context.Response.ContentType = images.ContentType;
                byte[] img = images.GetImage(width, height);
                if (img != null)
                {
                    context.Response.BinaryWrite(img);
                    context.Response.Cache.SetNoStore();
                }
            }
        }

        public static Bitmap CreateTextImage(int imageWidth, int imageHeight, string text)
        {
            Bitmap b = new Bitmap(imageWidth, imageHeight);

            Graphics g = Graphics.FromImage(b);
            g.Clear(Color.Ivory); // ColorTranslator.FromHtml("#F0F8FF"));
            Font f = new Font("Trebuchet,Verdana,Arial", 7.5f, FontStyle.Italic);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            g.DrawString(text, f, Brushes.Gray, new RectangleF(1, 0, b.Width, b.Height), sf);
            f.Dispose();
            g.Dispose();

            return b;
        }

        public static void ProcessRequestImage(HttpContext context, byte[] imgBytes)
        {
            var width = context.GetRequestInt("width", 200);
            var height = context.GetRequestInt("height", 200);


            //byte[] imgBytes = (byte[])(row["FILE_IMAGE"]);
            imgBytes = Imaging.ScaleImage(imgBytes, width, height, null);
            var image = Imaging.ImageFromBytes(imgBytes);

            context.Response.Clear();
            context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(0));
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetLastModified(DateTime.Now);
            context.Response.Cache.SetMaxAge(new TimeSpan(0, 0, 0));
            context.Response.BufferOutput = false;
            context.Response.Cache.SetNoStore();
            

            context.Response.ContentType = Imaging.GetContentTypeByImageFormat(image.RawFormat);
            context.Response.BinaryWrite(imgBytes);

            context.Response.Cache.SetNoStore();

            context.Response.Cache.SetNoStore();

            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
    }

    public class ImageCacheEntry
    {
        private Dictionary<Size, byte[]> Images = new Dictionary<Size, byte[]>();
        public byte[] OriginalImage;

        public string ContentType
        {
            get;
            private set;
        }

        public ImageCacheEntry(byte[] imageBytes)
        {
            using (Image image = Imaging.ImageFromBytes(imageBytes))
            {
                OriginalImage = imageBytes;

                if (image == null)
                    return;

                ContentType = Imaging.GetContentTypeByImageFormat(image.RawFormat);
                Images.Add(new Size(image.Width, image.Height), imageBytes);
            }
        }

        public byte[] GetImage(int width, int height)
        {
            lock (this)
            {
                Size size = new Size(width, height);
                byte[] result;
                if (!Images.TryGetValue(size, out result))
                {
                    result = Imaging.ScaleImage(OriginalImage, width, height, null);
                    Images.Add(size, result);
                }
                return result;
            }
        }
    }
}