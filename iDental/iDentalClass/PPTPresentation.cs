using iDental.Class;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace iDental.iDentalClass
{
    public class PPTPresentation
    {
        public bool CreatePPTExport(ObservableCollection<Templates_Images> observableCollection, string fileName, string pptTitle)
        {
            try
            {
                Application pptApplication = new Application();

                Slides slides;
                _Slide slide;
                TextRange objText;

                // Create the Presentation File
                Presentation pptPresentation = pptApplication.Presentations.Add(MsoTriState.msoTrue);

                CustomLayout customLayout = pptPresentation.SlideMaster.CustomLayouts[PpSlideLayout.ppLayoutText];

                for (int i = 0; i < observableCollection.Count(); i++)
                {
                    // Create new Slide
                    slides = pptPresentation.Slides;
                    slide = slides.AddSlide(i + 1, customLayout);

                    // Add title
                    objText = slide.Shapes[1].TextFrame.TextRange;
                    objText.Text = pptTitle;
                    objText.Font.Name = "Arial";
                    objText.Font.Size = 32;

                    PowerPoint.Shape shape = slide.Shapes[2];
                    slide.Shapes.AddPicture(observableCollection[i].Image_Path, MsoTriState.msoFalse, MsoTriState.msoTrue, shape.Left, shape.Top, shape.Width, shape.Height);

                    slide.NotesPage.Shapes[2].TextFrame.TextRange.Text = "This document is created by DigiDental.";
                }
                pptPresentation.SaveAs(fileName, PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoTrue);
                //pptApplication.Quit();

                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.ErrorMessageOutput(ex.ToString());
                return false;
            }
        }
    }
}
