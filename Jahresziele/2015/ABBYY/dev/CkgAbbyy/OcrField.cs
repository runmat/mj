using System.Drawing;
using FCEngine;

namespace CkgAbbyy
{
    public class OcrField
    {
        public ITrainingImageObject Image { get; set; }
        public Rectangle Ofset { get; set; }

        public ITrainingImageObject RefImage { get; set; }

        public int Sort { get; set; }
    }
}
