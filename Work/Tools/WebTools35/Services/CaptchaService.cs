using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;

namespace WebTools.Services
{
    public class CaptchaService
    {
        private static int _width = 310;
        private static readonly int CharSpace = 40;

        /// <summary>
        /// method for create captcha image
        /// </summary>
        public static byte[] CreateCaptchaImage(string captchaText)
        {
            var bitmap = new Bitmap(_width, 65, PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(bitmap);
            var fontColor = new SolidBrush(Color.FromArgb(120, 120, 120));
            var counter = 0;
            g.Clear(Color.FromArgb(120, 255, 255, 255));

            var rand = new Random();
            for (var i = 0; i < captchaText.Length; i++)
            {
                g.DrawString(captchaText[i].ToString(), new Font("Arial", 14 + rand.Next(1, 15), FontStyle.Italic), fontColor, new PointF(10 + counter, 10));
                counter += (CharSpace - 2);
            }
            //DrawRandomLines(g);

            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            var bmpBytes = ms.GetBuffer();
            ms.Close();


            g.Dispose();
            bitmap.Dispose();

            return bmpBytes;
        }

        /// <summary>
        /// Method for generating random text of n charactersas captcha code
        /// </summary>
        /// <returns></returns>
        public static string GetRandomText(int length)
        {
            var randomText = new StringBuilder();
            var alphabets = "12345679AEFGHMNRabdefghijhmnoqr";
            var r = new Random();
            for (var j = 0; j <= length; j++)
                randomText.Append(alphabets[r.Next(alphabets.Length)]);

            return randomText.ToString();
        }

        public static string GetSessionCaptchaText()
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session["Captcha"] == null)
                return "";

            return HttpContext.Current.Session["Captcha"].ToString();
        }

        public static string GetAndSetSessionCaptchaText(int length)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
                return "";

            var captchaText = GetRandomText(length);
            _width = captchaText.Length * CharSpace;
            HttpContext.Current.Session["Captcha"] = captchaText;
            
            return captchaText;
        }
    }
}
