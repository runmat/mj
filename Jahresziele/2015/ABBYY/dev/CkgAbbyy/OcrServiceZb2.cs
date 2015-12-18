using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using FCEngine;
using GeneralTools.Models;

namespace CkgAbbyy
{
    public class OcrServiceZb2 : IOcrService
    {
        const string RootFolder = @"C:\Backup\ABBYY\ZBII";

        public void CreateDefinitionFromTrainingImages()
        {
            var batchFolder = Path.Combine(RootFolder, "_TrainingBatch");
            if (Directory.Exists(batchFolder))
                Directory.Delete(batchFolder, true);

            var engineLoader = new InprocLoader();
            var engine = engineLoader.Load(FceConfig.GetDeveloperSn(), "");

            ITrainingBatch trainingBatch = engine.CreateTrainingBatch(batchFolder, "German");
            try
            {
                ITrainingDefinition newDefinition = trainingBatch.Definitions.AddNew("ZBII");

                var trainingImagFileNames = new[]
                {
                    "VF1FDC1HH38365655.jpg",
                    "JMZBK14Z261351772.jpg",
                    "JTKLC16420J001979.jpg",
                    //"VF1JPOCO532739479.jpg",
                    //"TMADB81CACJ076808.jpg",
                    //"0007-a.jpg",
                };
                trainingImagFileNames.ToList().ForEach(f => trainingBatch.AddImageFile(Path.Combine(RootFolder, f)));

                var dict = new Dictionary<string, OcrField>
                {
                    { "VIN", new OcrField() },
                };
                string fieldKey;

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

                    var text = obj.RecognizedText;

                    // Data
                    if (text != null && text.ToUpper() == trainingImagFileNames[0].Replace(".jpg", ""))
                    {
                        fieldKey = "VIN";
                        var entry = dict[fieldKey];
                        if (entry.Image == null)
                            entry.Image = obj;
                        entry.Ofset = new Rectangle(0,-10,50,20);
                        entry.Sort = 10;
                    }

                    // References
                    text = obj.RecognizedText;
                    if (text != null && text == "E")
                    {
                        fieldKey = "VIN";
                        var entry = dict[fieldKey];
                        if (entry.RefImage == null)
                            entry.RefImage = obj;
                    }
                }


                foreach (var entry in dict.OrderBy(e => e.Value.Sort))
                {
                    var key = entry.Key;
                    var obj = entry.Value.Image;
                    var objRef = entry.Value.RefImage;
                    var ofs = entry.Value.Ofset;

                    if (obj == null)
                        continue;

                    // Data field:
                    ITrainingField field = newDefinition.Fields.AddNew(key, TrainingFieldTypeEnum.TFT_Field);
                    IRegion r = obj.Region;
                    IRegion newRegion = engine.CreateRegion();
                    newRegion.AddRect(r.get_Left(0) + ofs.X, r.get_Top(0) + ofs.Y, r.get_Right(0) + ofs.Width, r.get_Bottom(0) + ofs.Height);
                    firstPage.SetFieldBlock(field, newRegion);

                    // Reference-Field:
                    if (objRef != null)
                    {
                        ITrainingField tag = newDefinition.Fields.AddNew(key + "_Ref", TrainingFieldTypeEnum.TFT_ReferenceText);
                        firstPage.SetFieldBlock(tag, objRef.Region);
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

                var documentDefinitionFileName = Path.Combine(RootFolder, "ZBII.afl");

                newDefinition.ExportToAFL(documentDefinitionFileName);
            }
            finally
            {
                trainingBatch.Close();
            }
        }

        public void ParseImagesFromDefinition()
        {
            const string key = "_VIN";

            var documentDefinitionFileName = Path.Combine(RootFolder, "ZBII.afl");

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
            for (var i = 0; i < imageFiles.Length - 0; i++)
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

                var valid = Helper.KeyIsValid(xmlFileName, key, VinMatches);
                var fName = Path.GetFileNameWithoutExtension(xmlFileName).NotNullOrEmpty().ToLower();
                var errorPrefix = (_vinMatchDict.ContainsKey(fName) && _vinMatchDict[fName]) ? "_WARNING_" : "_ERROR_";

                if (!valid || errorPrefix == "_WARNING_")
                {
                    var errorXmlFileName = Path.Combine(RootFolder, "export", errorPrefix + fileNameWithoutExtension + ".xml");
                    if (File.Exists(errorXmlFileName))
                        File.Delete(errorXmlFileName);
                    File.Move(xmlFileName, errorXmlFileName);
                }

                pos++;
            }
        }

        readonly Dictionary<string, bool> _vinMatchDict = new Dictionary<string, bool>();

        bool VinMatches(string fileName, string keyValue)
        {
            keyValue = keyValue.Replace("o", "0");

            var val = fileName.Equals(keyValue, StringComparison.InvariantCultureIgnoreCase);
            _vinMatchDict.Add(fileName, val);

            return val;
        }
    }
}
