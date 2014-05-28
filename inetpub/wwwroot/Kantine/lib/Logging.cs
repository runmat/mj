using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;

namespace Kantine
{
	public class Logging
	{
		string PathText = "C:/Inetpub/wwwroot/Kantine/Logs/Log_"+ DateTime.Today.Month.ToString() + DateTime.Today.Year + ".txt";
		string PathXML = "C:/Inetpub/wwwroot/Kantine/Logs/Log_" + DateTime.Today.Month.ToString() + DateTime.Today.Year + ".xml";

		public string LogfilePathText
		{
			get {return PathText;}
			set{PathText = value;}
		}

		public string LogfilePathXML
		{
			get { return PathXML; }
			set { PathXML = value; }
		}

		public void NewTextEntry(string [] Daten) 
		{
			FileStream FSOpen = File.Open(LogfilePathText, FileMode.OpenOrCreate);
			FSOpen.Close();
			FileStream FS = new FileStream(LogfilePathText, FileMode.Append, FileAccess.Write, FileShare.Write);
			StreamWriter SW = new StreamWriter(FS);

			for (int i = 0; i < Daten.GetLength(0);i++)
			{
				if (i == Daten.GetLength(0) - 1)
				{						
					SW.Write( Daten[i]+SW.NewLine);
					
				}
				else
				{
					SW.Write(Daten[i] + " | ");
					
				}
			}
			SW.Close();
		}

		public void NewXMLEntry(string[,] Daten)
		{
			XmlDocument XMLDoc = new XmlDocument();

			XmlTextWriter myXmlTextWriter = new XmlTextWriter(LogfilePathXML,
                                                   System.Text.Encoding.UTF8);
			myXmlTextWriter.Formatting = Formatting.Indented;
			myXmlTextWriter.WriteStartDocument();
			
			myXmlTextWriter.WriteComment("Logfile für den " + DateTime.Today.Month + "." + DateTime.Today.Year);
 
			myXmlTextWriter.WriteStartElement(DateTime.Today.Date.ToShortDateString());

			for (int i = 0; i < Daten.GetLength(0); i++)
			{
				myXmlTextWriter.WriteElementString(Daten[i,0].ToString(), Daten[i,1].ToString());
			}	
			 
			myXmlTextWriter.WriteEndElement(); 
 
			myXmlTextWriter.Flush();
			myXmlTextWriter.Close();
		}

	}
}
