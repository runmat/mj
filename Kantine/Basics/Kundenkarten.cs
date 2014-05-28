using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Printing;
using System.IO;

namespace Basics.Kundenkarten
{
	public class Kundenkarte
	{
		string m_Kunde;
		string m_Kundennummer;
		int m_Kartennummer;

		public string Kunde 
		{
			get { return m_Kunde; }
		}

		public string Kundennummer 
		{
			get { return m_Kundennummer; }
		}

		public int Kartennummer 
		{
			get { return m_Kartennummer; }
		}

		public Kundenkarte(string Kunde,string Kundennummer,int Kartennummer)
		{
			m_Kunde = Kunde;
			m_Kundennummer = Kundennummer;
			m_Kartennummer = Kartennummer;
		}

		public void Drucken(string Druckername)
		{ 
			string Barcodedaten = m_Kundennummer.ToString();

			if (m_Kartennummer<10)
			{
				Barcodedaten += "0" + m_Kartennummer;
			}
			else
			{
				Barcodedaten += m_Kartennummer;
			}
			
			string Command = (char)27 + "MASTER" + (char)13 +
									 (char)27 + "SXY 1" + (char)13 +
									 (char)27 + "+C 5" + (char)13 +
									 (char)27 + "F" + (char)13 +
									 (char)27 + "T 150 300 0 0 0 70 1 " + m_Kunde.Trim() + (char)13 +
									 (char)27 + "B 500 550 4 1 2 4 200 0 " + Barcodedaten.Trim() + (char)13 +	
									 (char)27 + "I" + (char)13 +
									 (char)27 + "MO" + (char)13;

//+++++++++++++++++++++++++++++++ Settings Start +++++++++++++++++++++++++++++++++++++
//	Barcode +
//+++++++++++

//			Syntax <Esc>B p1 p2 p3 p4 p5 p6 p7 p8 data
//			
//			Parameters p1 = Horizontal (X-axis) Start Position, in dots
//				p2 = Vertical (Y-axis) Start Position, in dots
//				p3 = Rotation:
//						
//						Value Description Origin
//						0 No rotation Lower Left
//						1 90 degrees Lower Left
//						2 180 degrees Lower Left
//						3 270 degrees Lower Left
//						4 No rotation Centered
//						5 90 degrees Centered
//						6 180 degrees Centered
//						7 270 degrees Centered

//				p4 = Bar Code selection
//
//					0 = Code 39 (3 of 9—Alphanumeric)
//					1 = 2/5 Interleaved (Numeric, Even No Count
//					2 = 2/5 Industrial (Numeric) no Check Digit
//					3 = EAN8 (Numeric, 12 digits en coded)
//					4 = EAN13 (Numeric, 12 digits en coded)
//					5 = UPC - A (Numeric, 12 digits en coded)
//					6 = Re served for MON ARCH
//					7 = Code 128 C w/o Check Digits* (Numeric only, Even Number Printed)
//					8 = Code 128 B w/o Check Digits* (Alphanumeric)
//		            107 = Code 128 C w/Check Digits* (Numeric only, Even Number printed)
//					108 = Code 128 B w/Check Digits* (Alphanumeric)
//					
//					* Not supported in some Monochrome Printer
				
//				p5 = Bar Width Ratio

//					Value Narrow Bar Wide Bar Ra tio
//						0 - 1 dot 2 dots 2:1
//						1 - 1 dot 3 dots 3:1
//						2 - 2 dots 5 dots 2.5:1 or 2:5

//				NOTE: Some bar code types have a selectable barcode width ratio. 

//				p6 = Bar Code Bar Width Multiplier. Range 3~9 for all
//					Zebra card barcodes except UPC-A, EAN-8 and EAN-13 which have a range of 4~7. 
			//		For a selected barwidth ratio of 2:5, the range is 2~4.

			//	p7 = BarCode Height in dots

//				Note: Each Bar Code Type has an industry specified minimum height standard. 

			//	p8 = Print Text version of BarCode under BarCode
//					1 = yes
//					0 = no

//++++++++++++++++++++++++++++++++++
//Draw Text (Mono chrome/Overlay)  +
//++++++++++++++++++++++++++++++++++

			//	Syntax <Esc>T p1 p2 p3 p4 p5 p6 p7 data
			//			vT p1 p2 p3 p4 p5 p6 p7 data

			//	Parameters
 
			//	p1 = Horizontal (X) Start Position in dots

			//	p2 = Vertical (Y) Start Position in dots (position of lower case descender, if used)

			//	p3 = Rotation & Origin

			//		Value Description Origin
			//		0 - No rotation Lower Left
			//		1 - 90 degrees Lower Left
			//		2 - 180 degrees Lower Left
			//		3 - 270 degrees Lower Left
			//		4 - No rotation Centered
			//		5 - 90 degrees Centered
			//		6 - 180 degrees Centered
			//		7 - 270 degrees Centered

			//	p4 = Font selection

			//		0 = 100 points Normal
			//		1 = 100 points Bold
			
			//	p5 = Horizontal (X-axis) Width (before rotation) of Text
			//		(data string) Graphic in dots. If the value is zero the
			//		text maintains normal font proportions and scales
			//		according to the value of the Y-axis (p6) value.
			
			//	p6 = Vertical (Y-axis) Height (before rotation) of Text (data string)
			//		Graphic in dots as measured from top of ascender to bottom of decender
			
			//		Examples:
			//			For 28-point normal, p6 = 104
			//			For 28-point bold, p6 = 140

			//NOTE: With p5 a “0,” fonts maintain normal proportions, and just p6 determines font size.
			
			//	p7 = Graphic Mode:

			//		0 = Clear Print Area and load Reverse BitMap Image
			//		1 = Clear Print Area and load Standard BitMap Image
			//		2 = Overwrite Background BitMap Image in Printable Dot Locations, 
			//			leaving Non-Printing Dot Locations alone

//+++++++++++++++++++++++++++++++ Settings End ++++++++++++++++++++++++++++++++++++++++++++++++++++++

			char[] Characters = Command.ToCharArray();
			byte[] bytes = new byte[Characters.Length];
			for (int i = 0; i < Characters.Length; i++)
			{
				bytes[i] = Convert.ToByte(Characters[i]);
			}

			System.Printing.PrintServer PS = new LocalPrintServer();
			PS.NetPopup = true;
			PrintQueue PQ = PS.GetPrintQueue(Druckername);//"Zebra Performance Class Network2"
			PrintSystemJobInfo PSInfo = PQ.AddJob();
			Stream str = PSInfo.JobStream;

			BinaryWriter BW = new BinaryWriter(str, Encoding.ASCII);
			BW.Write(bytes);
			BW.Close();
			str.Close();

			PQ.Commit();

			////Testfile mit Druckerdaten
			//FileStream FS2 = new FileStream("C:/Binär.txt",FileMode.OpenOrCreate);
			//BinaryWriter BW2 = new BinaryWriter(FS2, Encoding.ASCII);
			//BW2.Write(bytes);
			//BW2.Close();
			//FS2.Close();
		}


        /// <summary>
        /// <b>Experimentell und noch nicht Releasefähig!</b>
        /// </summary>
        /// <param name="Druckername">Name des Zieldruckers</param>
        /// <param name="Printserver">Name des Ziel Printserver</param>        
        public void Drucken(string Druckername,string Printserver)
		{ 
			string Barcodedaten = m_Kundennummer.ToString();

			if (m_Kartennummer<10)
			{
				Barcodedaten += "0" + m_Kartennummer;
			}
			else
			{
				Barcodedaten += m_Kartennummer;
			}
			
			string Command = (char)27 + "MASTER" + (char)13 +
									 (char)27 + "SXY 1" + (char)13 +
									 (char)27 + "+C 5" + (char)13 +
									 (char)27 + "F" + (char)13 +
									 (char)27 + "T 150 300 0 0 0 70 1 " + m_Kunde.Trim() + (char)13 +
									 (char)27 + "B 500 550 4 1 2 4 200 0 " + Barcodedaten.Trim() + (char)13 +	
									 (char)27 + "I" + (char)13 +
									 (char)27 + "MO" + (char)13;

//+++++++++++++++++++++++++++++++ Settings Start +++++++++++++++++++++++++++++++++++++
//	Barcode +
//+++++++++++

//			Syntax <Esc>B p1 p2 p3 p4 p5 p6 p7 p8 data
//			
//			Parameters p1 = Horizontal (X-axis) Start Position, in dots
//				p2 = Vertical (Y-axis) Start Position, in dots
//				p3 = Rotation:
//						
//						Value Description Origin
//						0 No rotation Lower Left
//						1 90 degrees Lower Left
//						2 180 degrees Lower Left
//						3 270 degrees Lower Left
//						4 No rotation Centered
//						5 90 degrees Centered
//						6 180 degrees Centered
//						7 270 degrees Centered

//				p4 = Bar Code selection
//
//					0 = Code 39 (3 of 9—Alphanumeric)
//					1 = 2/5 Interleaved (Numeric, Even No Count
//					2 = 2/5 Industrial (Numeric) no Check Digit
//					3 = EAN8 (Numeric, 12 digits en coded)
//					4 = EAN13 (Numeric, 12 digits en coded)
//					5 = UPC - A (Numeric, 12 digits en coded)
//					6 = Re served for MON ARCH
//					7 = Code 128 C w/o Check Digits* (Numeric only, Even Number Printed)
//					8 = Code 128 B w/o Check Digits* (Alphanumeric)
//		            107 = Code 128 C w/Check Digits* (Numeric only, Even Number printed)
//					108 = Code 128 B w/Check Digits* (Alphanumeric)
//					
//					* Not supported in some Monochrome Printer
				
//				p5 = Bar Width Ratio

//					Value Narrow Bar Wide Bar Ra tio
//						0 - 1 dot 2 dots 2:1
//						1 - 1 dot 3 dots 3:1
//						2 - 2 dots 5 dots 2.5:1 or 2:5

//				NOTE: Some bar code types have a selectable barcode width ratio. 

//				p6 = Bar Code Bar Width Multiplier. Range 3~9 for all
//					Zebra card barcodes except UPC-A, EAN-8 and EAN-13 which have a range of 4~7. 
			//		For a selected barwidth ratio of 2:5, the range is 2~4.

			//	p7 = BarCode Height in dots

//				Note: Each Bar Code Type has an industry specified minimum height standard. 

			//	p8 = Print Text version of BarCode under BarCode
//					1 = yes
//					0 = no

//++++++++++++++++++++++++++++++++++
//Draw Text (Mono chrome/Overlay)  +
//++++++++++++++++++++++++++++++++++

			//	Syntax <Esc>T p1 p2 p3 p4 p5 p6 p7 data
			//			vT p1 p2 p3 p4 p5 p6 p7 data

			//	Parameters
 
			//	p1 = Horizontal (X) Start Position in dots

			//	p2 = Vertical (Y) Start Position in dots (position of lower case descender, if used)

			//	p3 = Rotation & Origin

			//		Value Description Origin
			//		0 - No rotation Lower Left
			//		1 - 90 degrees Lower Left
			//		2 - 180 degrees Lower Left
			//		3 - 270 degrees Lower Left
			//		4 - No rotation Centered
			//		5 - 90 degrees Centered
			//		6 - 180 degrees Centered
			//		7 - 270 degrees Centered

			//	p4 = Font selection

			//		0 = 100 points Normal
			//		1 = 100 points Bold
			
			//	p5 = Horizontal (X-axis) Width (before rotation) of Text
			//		(data string) Graphic in dots. If the value is zero the
			//		text maintains normal font proportions and scales
			//		according to the value of the Y-axis (p6) value.
			
			//	p6 = Vertical (Y-axis) Height (before rotation) of Text (data string)
			//		Graphic in dots as measured from top of ascender to bottom of decender
			
			//		Examples:
			//			For 28-point normal, p6 = 104
			//			For 28-point bold, p6 = 140

			//NOTE: With p5 a “0,” fonts maintain normal proportions, and just p6 determines font size.
			
			//	p7 = Graphic Mode:

			//		0 = Clear Print Area and load Reverse BitMap Image
			//		1 = Clear Print Area and load Standard BitMap Image
			//		2 = Overwrite Background BitMap Image in Printable Dot Locations, 
			//			leaving Non-Printing Dot Locations alone

//+++++++++++++++++++++++++++++++ Settings End ++++++++++++++++++++++++++++++++++++++++++++++++++++++

			char[] Characters = Command.ToCharArray();
			byte[] bytes = new byte[Characters.Length];
			for (int i = 0; i < Characters.Length; i++)
			{
				bytes[i] = Convert.ToByte(Characters[i]);
			}

			System.Printing.PrintServer PS = new PrintServer(@"\\"+Printserver,PrintSystemDesiredAccess.UsePrinter);
			PS.NetPopup = true;
			PrintQueue PQ = PS.GetPrintQueue(Druckername);//"Zebra Performance Class Network2"
			PrintSystemJobInfo PSInfo = PQ.AddJob();
			Stream str = PSInfo.JobStream;

			BinaryWriter BW = new BinaryWriter(str, Encoding.ASCII);
			BW.Write(bytes);
			BW.Close();
			str.Close();

			PQ.Commit();

			////Testfile mit Druckerdaten
			//FileStream FS2 = new FileStream("C:/Binär.txt",FileMode.OpenOrCreate);
			//BinaryWriter BW2 = new BinaryWriter(FS2, Encoding.ASCII);
			//BW2.Write(bytes);
			//BW2.Close();
			//FS2.Close();
		} 
      
        /// <summary>
        /// Liefert eine Lsite aller Drucker auf einem beliebigen erreichbaren Printserver
        /// </summary>
        /// <param name="Printserver">Der zu verwendende Printserver</param>
        /// <returns>Liste aller Drucker</returns>
        public List<string> GetDrucker(string Printserver)
        {
            List<string> list = new List<string>();
            
            PrintServer PS = new PrintServer(@"\\" + Printserver + @"\IT_Kyo_1020_P128",PrintSystemDesiredAccess.None);
			PS.NetPopup = true;
			PrintQueueCollection PQC = PS.GetPrintQueues();

            foreach (PrintQueue PQ in PQC)
            {
                list.Add(PQ.FullName);
            }

            return list;
        }
    }
}
