using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace GeneralTools.Services
{
    public class ImagingService
    {
        public static ImageSource ImageSourceFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.None;
            bitmap.EndInit();
            return bitmap;
        }
        
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
            var ms = new MemoryStream();
            image.Save(ms, imgFormat);
            ms.Seek(0, SeekOrigin.Begin);
            var imageBytes = new byte[ms.Length];
            ms.Read(imageBytes, 0, (int)ms.Length);
            return imageBytes;
        }

        public static byte[] CreateEmptyHintImage(string cultureEmptyImageHint, string textcolor, int width, int height)
        {
            var b = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(b);
            g.Clear(ColorTranslator.FromHtml("#F0F0F0"));
            g.DrawRectangle(Pens.DarkGray, new Rectangle(0, 0, b.Width - 1, b.Height - 1));
            var sf = new StringFormat(StringFormat.GenericDefault) {Alignment = StringAlignment.Center};

            //string cultureEmptyImageHint = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["hintEmptyImage"].ToString());
            var f = new Font("Verdana", 8.0f, FontStyle.Italic);
            var size = g.MeasureString(cultureEmptyImageHint, f, new SizeF(b.Width - 4, b.Height), sf);
            var br = new SolidBrush(ColorTranslator.FromHtml("#" + textcolor));
            g.DrawString(cultureEmptyImageHint, f, br, new RectangleF(2, b.Height / 2 - size.Height / 2, b.Width - 4, size.Height), sf);
            br.Dispose();

            g.Dispose();

            var ms = new MemoryStream();
            b.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public static Image ImageFromBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }
            var ms = new MemoryStream(bytes);
            Image img = null;
            try
            {
                img = Image.FromStream(ms);
            }
            catch { }
            return img;
        }

        public static byte[] ScaleImage(byte[] bytes, int maxWidth, int maxHeight)
        {
            return ScaleImage(bytes, maxWidth, maxHeight, ImageFormat.Jpeg);
        }

        public static byte[] ScaleImage(byte[] bytes, int maxWidth, int maxHeight, ImageFormat format)
        {
            try
            {
                using (var img = Image.FromStream(new MemoryStream(bytes)))
                {
                    if (format == null)
                    {
                        format = img.RawFormat;
                    }
                    if (img.Size.Width > maxWidth || img.Size.Height > maxHeight)
                    {
                        //resize the image to fit our website's required size
                        var newWidth = img.Size.Width;
                        var newHeight = img.Size.Height;
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
                            using (var g = Graphics.FromImage(newImage))
                            {
                                //if (indexed)
                                {
                                    g.FillRectangle(Brushes.Transparent, 0, 0, newWidth, newHeight);
                                }
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                g.CompositingQuality = CompositingQuality.HighQuality;

                                g.DrawImage(img, new Rectangle(-1, -1, newWidth + 1, newHeight + 1));
                            }
                            using (var ms = new MemoryStream())
                            {
                                newImage.Save(ms, format);
                                bytes = ms.ToArray();
                                newImage.Dispose();
                            }
                        }
                    }
                    else if (!Equals(img.RawFormat, format))
                    {
                        using (var ms = new MemoryStream())
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

        public static Bitmap CreateTextImage(int imageWidth, int imageHeight, string text)
        {
            var b = new Bitmap(imageWidth, imageHeight);

            var g = Graphics.FromImage(b);
            g.Clear(Color.Ivory); // ColorTranslator.FromHtml("#F0F8FF"));
            var f = new Font("Trebuchet,Verdana,Arial", 7.5f, FontStyle.Italic);
            var sf = new StringFormat {Alignment = StringAlignment.Near};
            g.DrawString(text, f, Brushes.Gray, new RectangleF(1, 0, b.Width, b.Height), sf);
            f.Dispose();
            g.Dispose();

            return b;
        }

        public static Bitmap CropImage(Bitmap img,int startX,int startY,int newWidth,int newHeight){
            var croppedBitmap = new Bitmap(newWidth,newHeight);

            for(int i = 0; i < img.Width;i++)
            {
                if(i >= startX ){                    
                    if(i < startX + newWidth){
                        for(int j = 0; j < img.Height;j++)
                        {
                            if(j >= startY ){
                                if(j < startY + newHeight){
                                    var x = i-startX;
                                    var y = j-startY;

                                    croppedBitmap.SetPixel(x,y,img.GetPixel(i,j));
                                }else{
                                    break;
                                }                                
                            }
                        }    
                    }else{
                        break;
                    }                   
                }
            }

            return croppedBitmap;
        }

        public static void ScaleAndSaveImage(string sourceImageFileName, string destinationImageFileName, int dimensions)
        {
            var bitmapSource = Image.FromFile(sourceImageFileName);
            var imgBytes = BytesFromImage(bitmapSource);
            var bitmapDestination = ImageFromBytes(ScaleImage(imgBytes, dimensions, dimensions));

            TryRotateImageDueToExifOrientationProperty(bitmapSource, bitmapDestination);

            bitmapDestination.Save(destinationImageFileName);

            bitmapSource.Dispose();
            bitmapDestination.Dispose();
        }

        public static void TryRotateImageDueToExifOrientationProperty(Image bitmapSource, Image bitmapDestination)
        {
            var imageProperties = bitmapSource.PropertyItems;
            var rotationType = RotateFlipType.RotateNoneFlipNone;
            foreach (var imageProperty in imageProperties)
            {
                if (imageProperty.Id == 274)
                {
                    var orientation = BitConverter.ToInt16(imageProperty.Value, 0);
                    switch (orientation)
                    {
                        case 1:
                            rotationType = RotateFlipType.RotateNoneFlipNone;
                            break;
                        case 3:
                            rotationType = RotateFlipType.Rotate180FlipNone;
                            break;
                        case 6:
                            rotationType = RotateFlipType.Rotate90FlipNone;
                            break;
                        case 8:
                            rotationType = RotateFlipType.Rotate270FlipNone;
                            break;
                    }
                }
            }

            if (rotationType != RotateFlipType.RotateNoneFlipNone)
                bitmapDestination.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }
    }

    public class ImageCacheEntry
    {
        private readonly Dictionary<Size, byte[]> _images = new Dictionary<Size, byte[]>();
        public byte[] OriginalImage;

        public string ContentType
        {
            get;
            private set;
        }

        public ImageCacheEntry(byte[] imageBytes)
        {
            using (var image = ImagingService.ImageFromBytes(imageBytes))
            {
                OriginalImage = imageBytes;

                if (image == null)
                    return;

                ContentType = ImagingService.GetContentTypeByImageFormat(image.RawFormat);
                _images.Add(new Size(image.Width, image.Height), imageBytes);
            }
        }

        public byte[] GetImage(int width, int height)
        {
            lock (this)
            {
                var size = new Size(width, height);
                byte[] result;
                if (!_images.TryGetValue(size, out result))
                {
                    result = ImagingService.ScaleImage(OriginalImage, width, height, null);
                    _images.Add(size, result);
                }
                return result;
            }
        }
    }
}