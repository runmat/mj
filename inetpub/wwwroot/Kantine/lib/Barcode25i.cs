using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Media;

namespace BarcodeCreator.lib
{
	enum BarcodeTypen {Code2of5Interleaved }

	class Barcode25i
	{
		enum StrichLänge {KeinStrich, Kurz, Lang }

		public Bitmap Generate(string Daten)
		{
			int Länge = Daten.Length;
			decimal Teilung = Länge / 2;

			//bool Gerade;
			int SignsNeeded;

			if (System.Math.Truncate(Teilung) * 2 == Länge)
			{
				//Gerade = true;
				SignsNeeded = Länge;
			}
			else 
			{
				//Gerade = false;
				SignsNeeded = Länge + 1;
				Daten = "0" + Daten;
			}

			//Länge Startzeichen + Länge DatenZeichen + Länge Stoppzeichen
			int Gesamtlänge = 8 + SignsNeeded*16 + 9;
			int Höhe = 100;

			Bitmap bmp = new Bitmap(Gesamtlänge, Höhe);

			for (int i = 0; i < 8; i++)
			{
				bool bBlack = false;
				switch (i)
				{ 
					case 0:
						bBlack = true;
						break;
					case 1:
						bBlack = true;
						break;
					case 2:
						bBlack = false;
						break;
					case 3:
						bBlack = false;
						break;
					case 4:
						bBlack = true;
						break;
					case 5:
						bBlack = true;
						break;
					case 6:
						bBlack = false;
						break;
					case 7:
						bBlack = false;
						break;
				}
				if (bBlack)
				{
					for (int k = 0; k < Höhe; k++)
					{						
						bmp.SetPixel(i, k, Color.Black);
					}
				}
				else 
				{
					for (int k = 0; k < Höhe; k++)
					{
						bmp.SetPixel(i, k, Color.White);						
					}
				}
				
			}

			//int Blockzähler = 0;

			for (int Blockzähler = 0; Blockzähler < SignsNeeded / 2; Blockzähler++ )//(int i = 8; i < Gesamtlänge - 7; i++)
			{
				//bool bBlack = false;

				string Strich1 = Daten.Substring(2*Blockzähler, 1);
				string Strich2 = Daten.Substring(2*Blockzähler + 1, 1);
				//bool curStrichIs1 = true;

				StrichLänge[] SL = new StrichLänge[10];

				for (int l = 0; l < 5; l++)
				{
					//string StrichtoDraw = "";

					//switch (curStrichIs1)
					//{
					//    case true:
					//        SL[l * 2] = GetStrichLänge(Strich1, l);
					//        curStrichIs1 = false;
					//        break;
					//    case false:
					//        SL[l * 2 + 1] = GetStrichLänge(Strich2,l);
					//        curStrichIs1 = true;
					//        break;
					//}
					SL[l * 2] = GetStrichLänge(Strich1, l);
					SL[l * 2 + 1] = GetStrichLänge(Strich2, l);
				}
				int BlockPos = 0;

				for (int l = 0; l < 10; l++)
				{


					Color col = Color.Black;
					switch (l)
					{
						case 0:
							col = Color.Black;
							break;
						case 1:
							col = Color.White;
							break;
						case 2:
							col = Color.Black;
							break;
						case 3:
							col = Color.White;
							break;
						case 4:
							col = Color.Black;
							break;
						case 5:
							col = Color.White;
							break;
						case 6:
							col = Color.Black;
							break;
						case 7:
							col = Color.White;
							break;
						case 8:
							col = Color.Black;
							break;
						case 9:
							col = Color.White;
							break;
					}
					if (SL[l] == StrichLänge.Kurz)
					{
						for (int m = 0; m < 2; m++)
						{
							DrawCurrentStrich(8 + 32 * Blockzähler + BlockPos, Höhe, col, ref bmp);
							BlockPos++;
						}
					}
					else if (SL[l] == StrichLänge.Lang)
					{
						for (int m = 0; m < 5; m++)
						{
							DrawCurrentStrich(8 + 32 * Blockzähler + BlockPos, Höhe, col, ref bmp);
							BlockPos++;
						}
					}
				}
				//Blockzähler++;
				BlockPos = 0;
			}

			for (int i = Gesamtlänge - 9; i < Gesamtlänge; i++)
			{
				bool bBlack = true;
				if(i < Gesamtlänge - 4 || i>Gesamtlänge-2)
				{
					bBlack = true;
				}
				else if (i < Gesamtlänge - 2)
				{
					bBlack = false;
				}
						
						
				if (bBlack)
				{
					for (int k = 0; k < Höhe; k++)
					{
						bmp.SetPixel(i, k, Color.Black);
					}
				}
				else
				{
					for (int k = 0; k < Höhe; k++)
					{
						bmp.SetPixel(i, k, Color.White);
					}
				}
			}

			return bmp;
		}

		private StrichLänge GetStrichLänge(string Zeichen,int Position)
		{
			StrichLänge SL = StrichLänge.KeinStrich;

			switch (Zeichen)
			{
				case "0":
					switch (Position)
					{
						case 0:
							SL = StrichLänge.Kurz;
							break;
						case 1:
							SL = StrichLänge.Kurz;
							break;
						case 2:
							SL = StrichLänge.Lang;
							break;
						case 3:
							SL = StrichLänge.Lang;
							break;
						case 4:
							SL = StrichLänge.Kurz;
							break;
						default:
							SL = StrichLänge.KeinStrich;
							break;
					}
					break;
				case "1":
					switch (Position)
					{
						case 0:
							SL = StrichLänge.Lang;
							break;
						case 1:
							SL = StrichLänge.Kurz;
							break;
						case 2:
							SL = StrichLänge.Kurz;
							break;
						case 3:
							SL = StrichLänge.Kurz;
							break;
						case 4:
							SL = StrichLänge.Lang;
							break;
						default:
							SL = StrichLänge.KeinStrich;
							break;
					}
					break;
				case "2":
					switch (Position)
					{
						case 0:
							SL = StrichLänge.Kurz;
							break;
						case 1:
							SL = StrichLänge.Lang;
							break;
						case 2:
							SL = StrichLänge.Kurz;
							break;
						case 3:
							SL = StrichLänge.Kurz;
							break;
						case 4:
							SL = StrichLänge.Lang;
							break;
						default:
							SL = StrichLänge.KeinStrich;
							break;
					}
					break;
				case "3":
					switch (Position)
					{
						case 0:
							SL = StrichLänge.Lang;
							break;
						case 1:
							SL = StrichLänge.Lang;
							break;
						case 2:
							SL = StrichLänge.Kurz;
							break;
						case 3:
							SL = StrichLänge.Kurz;
							break;
						case 4:
							SL = StrichLänge.Kurz;
							break;
						default:
							SL = StrichLänge.KeinStrich;
							break;
					}
					break;
				case "4":
					switch (Position)
					{
						case 0:
							SL = StrichLänge.Kurz;
							break;
						case 1:
							SL = StrichLänge.Kurz;
							break;
						case 2:
							SL = StrichLänge.Lang;
							break;
						case 3:
							SL = StrichLänge.Kurz;
							break;
						case 4:
							SL = StrichLänge.Lang;
							break;
						default:
							SL = StrichLänge.KeinStrich;
							break;
					}
					break;
				case "5":
					switch (Position)
					{
						case 0:
							SL = StrichLänge.Lang;
							break;
						case 1:
							SL = StrichLänge.Kurz;
							break;
						case 2:
							SL = StrichLänge.Lang;
							break;
						case 3:
							SL = StrichLänge.Kurz;
							break;
						case 4:
							SL = StrichLänge.Kurz;
							break;
						default:
							SL = StrichLänge.KeinStrich;
							break;
					}
					break;
				case "6":
					switch (Position)
					{
						case 0:
							SL = StrichLänge.Kurz;
							break;
						case 1:
							SL = StrichLänge.Lang;
							break;
						case 2:
							SL = StrichLänge.Lang;
							break;
						case 3:
							SL = StrichLänge.Kurz;
							break;
						case 4:
							SL = StrichLänge.Kurz;
							break;
						default:
							SL = StrichLänge.KeinStrich;
							break;
					}
					break;
				case "7":
					switch (Position)
					{
						case 0:
							SL = StrichLänge.Kurz;
							break;
						case 1:
							SL = StrichLänge.Kurz;
							break;
						case 2:
							SL = StrichLänge.Kurz;
							break;
						case 3:
							SL = StrichLänge.Lang;
							break;
						case 4:
							SL = StrichLänge.Lang;
							break;
						default:
							SL = StrichLänge.KeinStrich;
							break;
					}
					break;
				case "8":
					switch (Position)
					{
						case 0:
							SL = StrichLänge.Lang;
							break;
						case 1:
							SL = StrichLänge.Kurz;
							break;
						case 2:
							SL = StrichLänge.Kurz;
							break;
						case 3:
							SL = StrichLänge.Lang;
							break;
						case 4:
							SL = StrichLänge.Kurz;
							break;
						default:
							SL = StrichLänge.KeinStrich;
							break;
					}
					break;
				case "9":
					switch (Position)
					{
						case 0:
							SL = StrichLänge.Kurz;
							break;
						case 1:
							SL = StrichLänge.Lang;
							break;
						case 2:
							SL = StrichLänge.Kurz;
							break;
						case 3:
							SL = StrichLänge.Lang;
							break;
						case 4:
							SL = StrichLänge.Kurz;
							break;
						default:
							SL = StrichLänge.KeinStrich;
							break;
					}
					break;
			}
			return SL;
		}

		private void DrawCurrentStrich(int Position,int Höhe,Color Col,ref Bitmap bmp)
		{
			for (int k = 0; k < Höhe; k++)
			{
				bmp.SetPixel(Position, k, Col);
			}
		}
	}
}
