using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FCEngine;

namespace CkgAbbyy
{
    public class OcrServiceRg
    {
        const string RootFolder = @"C:\Backup\ABBYY\Lieferantenrechnung";

        public static void CreateDefinitionFromTrainingImages()
        {
            var batchFolder = Path.Combine(RootFolder, "_TrainingBatch");
            if (Directory.Exists(batchFolder))
                Directory.Delete(batchFolder, true);

            var engineLoader = new InprocLoader();
            var engine = engineLoader.Load(FceConfig.GetDeveloperSn(), "");

            ITrainingBatch trainingBatch = engine.CreateTrainingBatch(batchFolder, "German");
            try
            {
                ITrainingDefinition newDefinition = trainingBatch.Definitions.AddNew("Lieferantenrechnung");

                var trainingImagFileNames = new[]
                {
                    "89657.jpg",
                    "90067.jpg",
                    "90413.jpg",
                    "89724.jpg",
                    "90150.jpg",
                };
                trainingImagFileNames.ToList().ForEach(f => trainingBatch.AddImageFile(Path.Combine(RootFolder, f)));

                var dict = new Dictionary<string, OcrField>
                {
                    { "RG_NR", new OcrField() },
                    { "KD_NR", new OcrField() },
                    { "RG_Datum", new OcrField() },
                    { "UstIdentNr", new OcrField() },
                    { "Steuerbetrag", new OcrField() },
                    { "Endbetrag", new OcrField() },
                    { "KD_Kurzname", new OcrField() },
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

                    if (text != null && text == "89657")
                    {
                        // We want to extact this field. Create a data field and define its geometry on the current page.
                        fieldKey = "RG_NR";
                        var entry = dict[fieldKey];
                        if (entry.Image == null)
                            entry.Image = obj;
                        entry.Ofset = new Rectangle(0,0,0,0);
                        entry.Sort = 10;
                    }
                    if (text != null && text.StartsWith("129"))
                    {
                        // We want to extact this field. Create a data field and define its geometry on the current page.
                        fieldKey = "KD_NR";
                        var entry = dict[fieldKey];
                        if (entry.Image == null)
                            entry.Image = obj;
                        entry.Ofset = new Rectangle(0, 0, 180, 0);
                        entry.Sort = 20;
                    }
                    if (text != null && text.ToLower().StartsWith("evels"))
                    {
                        // We want to extact this field. Create a data field and define its geometry on the current page.
                        fieldKey = "KD_Kurzname";
                        var entry = dict[fieldKey];
                        if (entry.Image == null)
                            entry.Image = obj;
                        entry.Ofset = new Rectangle(-100, 0, 400, 0);
                        entry.Sort = 30;
                    }
                    if (text != null && text.ToLower().Contains("21.05.2015"))
                    {
                        // We want to extact this field. Create a data field and define its geometry on the current page.
                        fieldKey = "RG_Datum";
                        var entry = dict[fieldKey];
                        if (entry.Image == null)
                            entry.Image = obj;
                        entry.Ofset = new Rectangle(0, 0, 0, 0);
                        entry.Sort = 40;
                    }
                    if (text != null && text.ToLower().Contains("de811425"))
                    {
                        // We want to extact this field. Create a data field and define its geometry on the current page.
                        fieldKey = "UstIdentNr";
                        var entry = dict[fieldKey];
                        if (entry.Image == null)
                            entry.Image = obj;
                        entry.Ofset = new Rectangle(0, 0, 0, 0);
                        entry.Sort = 50;
                    }
                    if (text != null && text == "530,10")
                    {
                        // We want to extact this field. Create a data field and define its geometry on the current page.
                        fieldKey = "Steuerbetrag";
                        var entry = dict[fieldKey];
                        if (entry.Image == null)
                            entry.Image = obj;
                        entry.Ofset = new Rectangle(0, 0, 0, 0);
                        entry.Sort = 60;
                    }
                    if (text != null && text == "3.320,10")
                    {
                        // We want to extact this field. Create a data field and define its geometry on the current page.
                        fieldKey = "Endbetrag";
                        var entry = dict[fieldKey];
                        if (entry.Image == null)
                            entry.Image = obj;
                        entry.Ofset = new Rectangle(0, 0, 0, 0);
                        entry.Sort = 70;
                    }
                }
                foreach (ITrainingImageObject obj in firstPage.ImageObjects)
                {
                    var text = obj.RecognizedText;

                    if (text != null && text.ToLower() == "rechnung")
                    {
                        // We want to use this text for reference. Create a reference element and define its geometry on the current page.
                        fieldKey = "RG_NR";
                        var entry = dict[fieldKey];
                        if (entry.RefImage == null)
                            entry.RefImage = obj;
                    }
                    if (text != null && text.ToLower() == "kunden")
                    {
                        // We want to use this text for reference. Create a reference element and define its geometry on the current page.
                        fieldKey = "KD_NR";
                        var entry = dict[fieldKey];
                        if (entry.RefImage == null)
                            entry.RefImage = obj;
                    }
                    if (text != null && (text.ToLower() == "sys" || text.ToLower() == "gs"))
                    {
                        // We want to use this text for reference. Create a reference element and define its geometry on the current page.
                        fieldKey = "RG_Datum";
                        var entry = dict[fieldKey];
                        if (entry.RefImage == null)
                            entry.RefImage = obj;
                    }
                    if (text != null && text.ToLower().Contains("mwst"))
                    {
                        // We want to use this text for reference. Create a reference element and define its geometry on the current page.
                        fieldKey = "Steuerbetrag";
                        var entry = dict[fieldKey];
                        if (entry.RefImage == null)
                            entry.RefImage = obj;
                    }
                    if (text != null && text.ToLower().Contains("ident"))
                    {
                        // We want to use this text for reference. Create a reference element and define its geometry on the current page.
                        fieldKey = "UstIdentNr";
                        var entry = dict[fieldKey];
                        if (entry.RefImage == null)
                            entry.RefImage = obj;
                    }
                    if (text != null && text.ToLower() == "endbetrag")
                    {
                        // We want to use this text for reference. Create a reference element and define its geometry on the current page.
                        fieldKey = "Endbetrag";
                        var entry = dict[fieldKey];
                        if (entry.RefImage == null)
                            entry.RefImage = obj;
                    }
                    if (text != null && text.ToLower().StartsWith("versand"))
                    {
                        // We want to use this text for reference. Create a reference element and define its geometry on the current page.
                        fieldKey = "KD_Kurzname";
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

                var documentDefinitionFileName = Path.Combine(RootFolder, "Lieferantenrechnung.afl");

                newDefinition.ExportToAFL(documentDefinitionFileName);
            }
            finally
            {
                trainingBatch.Close();
            }
        }

        public static void ParseImagesFromDefinition(string key)
        {
            var documentDefinitionFileName = Path.Combine(RootFolder, "Lieferantenrechnung.afl");

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
