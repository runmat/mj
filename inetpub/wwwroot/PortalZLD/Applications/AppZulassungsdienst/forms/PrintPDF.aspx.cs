﻿using System;
using System.IO;
namespace AppZulassungsdienst.forms
{   
    /// <summary>
    /// Senden der PDF als Response.BinaryWrite Stream
    /// </summary>
    public partial class PrintPDF : System.Web.UI.Page
    {   
        /// <summary>
        /// Auslesen der Werte aus den Session. Datei in Stream wandeln.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["App_Filepath"] != null)
            {
                String sPfad = Session["App_Filepath"].ToString();
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = Session["App_ContentType"].ToString();
                Session["App_Filepath"] = null;
                Session["App_ContentType"] = null;
       
                //Get the physical path to the file.
                String FilePath  = sPfad;
                //Write the file directly to the HTTP output stream.
                String fname = sPfad.PadRight(sPfad.Length - sPfad.LastIndexOf("\\") - 1);

                FileStream MyFileStream = new FileStream(FilePath, FileMode.Open);
                long FileSize = MyFileStream.Length;

                byte[] pdfStream = new byte[MyFileStream.Length + 1];
                MyFileStream.Read(pdfStream, 0, Convert.ToInt32(FileSize));

                MyFileStream.Close();

                Response.AddHeader("Accept-Header", FileSize.ToString());
                Response.AddHeader("Content-Disposition", "inline; filename=" + fname);
                Response.AddHeader("Expires", "0");
                Response.AddHeader("Pragma", "cache");
                Response.AddHeader("Cache-Control", "private");
                Response.BinaryWrite(pdfStream);
                if (Response.IsClientConnected)
                {
                    Response.Flush();
                }
                Response.End();
                if (Session["App_FileDelete"] != null)
                {
                    File.Delete(sPfad);
                }
                Session["App_FileDelete"] = null;
            }
            else if (Session["PDFXString"] != null)
            {
                byte[] pdfBytes = (byte[])Session["PDFXString"];

                // Header bereinigen
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "inline; filename=" + "Beleg.pdf");
                Response.AddHeader("Expires", "0");
                Response.AddHeader("Pragma", "cache");
                Response.AddHeader("Cache-Control", "private");
                Response.BinaryWrite(pdfBytes);
                if (Response.IsClientConnected)
                {
                    Response.Flush();
                }
                Response.End();
                Session["PDFXString"] = null;
            }
        }
    }
}
