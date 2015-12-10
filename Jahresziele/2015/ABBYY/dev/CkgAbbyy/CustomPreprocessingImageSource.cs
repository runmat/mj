using System.Collections;
using FCEngine;

namespace CkgAbbyy
{
    class CustomPreprocessingImageSource : IImageSource
    {
        public CustomPreprocessingImageSource(IEngine engine)
        {
            fileNames = new Queue();
            imageTools = engine.CreateImageProcessingTools();
        }

        public void AddImageFile(string filePath)
        {
            fileNames.Enqueue(filePath);
        }

        // IImageSource ///////////////////
        public string GetName() { return "Custom Preprocessing Image Source"; }
        public IFileAdapter GetNextImageFile()
        {
            // You could apply an external tool to a file and return the reference
            // to the resulting image file. This sample source does not use this feature
            return null;
        }
        public IImage GetNextImage()
        {
            while (true)
            {
                if (imageFile == null)
                {
                    if (fileNames.Count > 0)
                    {
                        imageFile = imageTools.OpenImageFile((string)fileNames.Dequeue());
                        nextPageIndex = 0;
                    }
                    else
                    {
                        return null;
                    }
                }
                if (nextPageIndex < imageFile.PagesCount)
                {
                    IImage nextPage = imageFile.OpenImagePage(nextPageIndex++);

                    // Here you can apply any required preprocessing to the image in memory
                    // using built-in IImageProcessingTools (see description), your own algorithms or
                    // third-party components
                    // ...

                    return nextPage;
                }
                else
                {
                    imageFile = null;
                    continue;
                }
            }
        }

        #region IMPLEMENTATION

        IImageProcessingTools imageTools;

        Queue fileNames;
        IImageFile imageFile;
        int nextPageIndex;

        #endregion
    };
}