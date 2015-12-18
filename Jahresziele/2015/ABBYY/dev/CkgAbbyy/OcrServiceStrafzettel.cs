using System.IO;
using System.Linq;
using FCEngine;

namespace CkgAbbyy
{
    public class OcrServiceStrafzettel : IOcrService
    {
        const string RootFolder = @"C:\Backup\ABBYY\Strafzettel";

        public void CreateDefinitionFromTrainingImages()
        {
            string batchFolder = RootFolder + @"\_TrainingBatch";
            if (Directory.Exists(batchFolder))
                Directory.Delete(batchFolder, true);

            var engineLoader = new InprocLoader();
            var engine = engineLoader.Load(FceConfig.GetDeveloperSn(), "");

            ITrainingBatch trainingBatch = engine.CreateTrainingBatch(batchFolder, "German");
            try
            {
                ITrainingDefinition newDefinition = trainingBatch.Definitions.AddNew("AZ");

                var trainingImagFileNames = new[]
                {
                    "STUT_505-52-122567-4.jpg",
                    "BB_140-11-0027822-1.jpg",
                    "VAIHI_505-79-231367-6.jpg",
                    //"KS_966-523472-7.jpg",
                    "KARL_505-27-012371-0.jpg",
                    //"OSNABR_515-49-104859-0.jpg",
                    //"LEASEPLAN_01-11241-101779-9.jpg",
                };
                trainingImagFileNames.ToList().ForEach(f => trainingBatch.AddImageFile(Path.Combine(RootFolder, f)));

                ITrainingPage firstPage = trainingBatch.Pages[0];
                // Each page must be prepared before trying to work with its layout. At this stage the page is analyzed
                // and primitive image objects are extracted (which can be used as helpers in user imterface). 
                // Than an attempt is made to predict the page layout based on the layout of verified pages if any.
                firstPage.PrepareLayout();
                // At this point the user must draw boxes for fields and references. In this sample we try to emulate this 
                // behavior by 'looking' for known text strings and 'drawing' fields around them.
                for (int i = 0; i < firstPage.ImageObjects.Count; i++)
                {
                    var obj = firstPage.ImageObjects[i];

                    string text = obj.RecognizedText;
                    if (text != null)
                    {
                        if (Helper.KeyValueMatchesFileNames(trainingImagFileNames, text))
                        {
                            // We want to extact this field. Create a data field and define its geometry on the current page.
                            ITrainingField isbnField = newDefinition.Fields.AddNew("AZ", TrainingFieldTypeEnum.TFT_Field);
                            firstPage.SetFieldBlock(isbnField, obj.Region);
                            break;
                        }
                    }
                }
                foreach (ITrainingImageObject obj in firstPage.ImageObjects)
                {
                    string text = obj.RecognizedText;
                    if (text != null && text.ToLower() == "aktenzeichen:")
                    {
                        // We want to use this text for reference. Create a reference element and define its geometry on the current page.
                        ITrainingField isbnTag = newDefinition.Fields.AddNew("AZ_Ref", TrainingFieldTypeEnum.TFT_ReferenceText);
                        firstPage.SetFieldBlock(isbnTag, obj.Region);
                        break;
                    }
                }
                // Now that we are done with this page, mark it as verified and ready for training.
                trainingBatch.SubmitPageForTraining(firstPage);

                for (var i = 1; i < trainingBatch.Pages.Count; i++)
                {
                    ITrainingPage page = trainingBatch.Pages[i];
                    page.PrepareLayout();
                    // At this point the user must verify and correct the computed layout. In this sample we assume that 
                    // the computed layout is correct, so we just mark the page as verified and ready for training.
                    trainingBatch.SubmitPageForTraining(page);
                }

                var documentDefinitionFileName = Path.Combine(RootFolder, "Strafzettel.afl");

                newDefinition.ExportToAFL(documentDefinitionFileName);
            }
            finally
            {
                trainingBatch.Close();
            }
        }

        public void ParseImagesFromDefinition()
        {
            const string key = "_AZ;_AZ_Alternating";

            var documentDefinitionFileName = Path.Combine(RootFolder, "Strafzettel.afl");

            var engineLoader = new InprocLoader();
            var engine = engineLoader.Load(FceConfig.GetDeveloperSn(), "");

            IDocumentDefinition documentDefinition = engine.CreateDocumentDefinitionFromAFL(documentDefinitionFileName, "German");
            var customStorage = documentDefinition as ICustomStorage;
            if (customStorage == null)
                return;

            var processor = engine.CreateFlexiCaptureProcessor();
            processor.AddDocumentDefinition(documentDefinition);

            var imageSource = new CustomPreprocessingImageSource(engine);
            var imageFiles = Directory.GetFiles(RootFolder, "*.jpg");
            for (var i = 0; i < imageFiles.Length; i++)
                imageSource.AddImageFile(imageFiles[i]);

            processor.SetCustomImageSource(imageSource);
            var pos = 0;
            while (true)
            {
                var document = processor.RecognizeNextDocument();
                if (document == null)
                {
                    var error = processor.GetLastProcessingError();
                    break;
                }

                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFiles[pos]);
                var xmlFileName = Path.Combine(RootFolder, "export", fileNameWithoutExtension + ".xml");
                processor.ExportDocumentEx(document, Path.Combine(RootFolder, "export"), fileNameWithoutExtension);

                if (!Helper.KeyIsValid(xmlFileName, key))
                {
                    var errorXmlFileName = Path.Combine(RootFolder, "export", "_ERROR_" + fileNameWithoutExtension + ".xml");
                    if (File.Exists(errorXmlFileName))
                        File.Delete(errorXmlFileName);
                    File.Move(xmlFileName, errorXmlFileName);
                }

                pos++;
            }
        }
    }
}
