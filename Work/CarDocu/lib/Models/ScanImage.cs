using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Xml.Serialization;
using CarDocu.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using System.ComponentModel.DataAnnotations;

namespace CarDocu.Models
{
    public class ScanImage : ModelBase
    {
        #region Properties

        [Key]
        public string ImageID { get; set; }

        public string ParentDocumentID { get { return ParentDocument.DocumentID; } }

        public string ImageDocumentTypeCode { get; set; }

        public string ImageDocumentTypeCodeSAP { get; set; }

        [XmlIgnore]
        public DocumentType ImageDocumentType
        {
            get { return DomainService.Repository.GetImageDocumentType(ImageDocumentTypeCode); }
        }

        public int Sort { get; set; }

        public int TempImageCounter { get; set; }

        [XmlIgnore]
        public ScanDocument ParentDocument { get; set; }

        //private ImageSource _imageSource; 
        [XmlIgnore]
        public ImageSource ImageSource 
        { 
            get
            {
                //if (_imageSource != null) return _imageSource;

                //_imageSource = GetChachedImageSource(true);

                //return _imageSource;

                return GetChachedImageSource(true);
            }
        }

        [XmlIgnore]
        public static ImageFormat ImageFormat { get { return ImageFormat.Jpeg; } }

        #endregion

        public string GetCachedImageFileName(bool useThumbnail)
        {
            return Path.Combine(ParentDocument.GetDocumentPrivateDirectoryName(),
                                string.Format("{0}{1}{2}.{3}",
                                ImageID, 
                                (TempImageCounter == 0 ? "" : "_" + TempImageCounter), 
                                (useThumbnail ? "_tb" : ""), 
                                ImagingService.GetExtensionByImageFormat(ImageFormat)));
        }

        public ImageSource GetChachedImageSource(bool useThumbnail)
        {
            return ImagingService.ImageSourceFromUri(new Uri(GetCachedImageFileName(useThumbnail)));
        }

        public void SetImageAndStoreToFileCache(Bitmap bitmap)
        {
            var tempFileName = GetCachedImageFileName(false);
            var tempThumbFileName = GetCachedImageFileName(true);

            if (File.Exists(tempFileName))
                File.Delete(tempFileName);
            if (File.Exists(tempThumbFileName))
                File.Delete(tempThumbFileName);

            var imgBytes = ImagingService.BytesFromImage(bitmap, ImageFormat);
            var bitmapThumb = ImagingService.ImageFromBytes(ImagingService.ScaleImage(imgBytes, 800, 1200));

            bitmap.Save(tempFileName, ImageFormat);
            bitmap.Dispose();

            bitmapThumb.Save(tempThumbFileName, ImageFormat);
            bitmapThumb.Dispose();

            //_imageSource = null;
        }

        public void Rotate()
        {
            var bitmap = Image.FromFile(GetCachedImageFileName(false));
            bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
            var clonedBitmap = new Bitmap(bitmap);
            clonedBitmap.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
            bitmap.Dispose();

            TempImageCounter++;
            SetImageAndStoreToFileCache(clonedBitmap);
        }

        public void SendPropertyChangedImageSource()
        {
            SendPropertyChanged("ImageSource");
        }
    }
}
