using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Printing;
using System.IO;
using System.Text;

namespace Kantine
{
	public enum Zeitraum { aktuellerMonat, letzerMonat };

	public class DB_Kom
	{
		SqlConnection Con = new SqlConnection();

		String Except;

		int Verbindungsanforderungen = 0;

		public void crypt()
		{
			AesManaged CryptAES = new AesManaged();
			CryptAES.CreateDecryptor();
		}

		#region "Properties"

		public string Exception
		{
			get { return Except; }
			set { Except = value; }
		}

		public SqlConnection SQLConnection
		{
			get	{return Con;}
			set	{Con = value;}
		}

		#endregion

	#region "Methods / Functions"
		public DB_Kom()
		{
			SQLConnection.ConnectionString = "Data Source=192.168.10.69;Initial Catalog=Kantine;User ID=spyITUser;" +
												"Password=spyIT0815;persist security info=true"; //HHSRVW2KBCK019
		}

		internal void ConnectionÖffnen()
		{
			Verbindungsanforderungen++;

			if (SQLConnection.State != ConnectionState.Open)
			{
				SQLConnection.Open();
			}
		}

		internal void ConnectionSchließen()
		{
			if (Verbindungsanforderungen > 0)
			{
				Verbindungsanforderungen--;
			}
			if (Verbindungsanforderungen == 0 && SQLConnection.State != ConnectionState.Closed)
			{				
				SQLConnection.Close();
			}
		}

		private int KundennummerGenerieren()
		{
			int Nummer = -1;
			Random rdm = new Random();
			bool unique = false;
			DataTable dtBenutzer = GetAllBenutzer();

			while (!unique)
			{
				Nummer = rdm.Next(100000000);
				if (GetBenutzerAllByKundennummer(Nummer).Rows.Count == 0)
				{ unique = true; }
			}
			return Nummer;
		}

		public int KartennummerNeuGenerieren(int Kundennummer)
		{
			DataTable dt;
			int KtNew = -1;

			dt = GetBenutzerAllByKundennummer(Kundennummer);

			if (dt.Rows.Count > 0)
			{
				KtNew = (int)dt.Rows[0]["Kartennummer"]+1;
			}
			return KtNew;
		}

		public void PrintBarcode(int Nummer)
		{
			string Command = (char)27 + "MASTER" + (char)13 +
							 (char)27 + "SXY 1" + (char)13 +
							 (char)27 + "+C 5" + (char)13 +		//Kontrast
							 (char)27 + "F" + (char)13 +
							 (char)27 + "B 500 450 4 1 2 4 200 0 " + Nummer.ToString() + (char)13 +	//Barcode
							 (char)27 + "I" + (char)13 +
							 (char)27 + "MO" + (char)13;

			char[] Characters = Command.ToCharArray();
			byte[] bytes = new byte[Characters.Length];
			for (int i = 0; i < Characters.Length; i++)
			{
				bytes[i] = Convert.ToByte(Characters[i]);
			}

			System.Printing.PrintServer PS = new LocalPrintServer();
			PS.NetPopup = true;
			PrintQueue PQ = PS.GetPrintQueue("Zebra Performance Class Network2");
			PrintSystemJobInfo PSInfo = PQ.AddJob();
			Stream str = PSInfo.JobStream;

			BinaryWriter BW = new BinaryWriter(str, Encoding.ASCII);
			BW.Write(bytes);
			BW.Close();
			str.Close();

			PQ.Commit();
		}		

		public void Buchen(int Kundennummer, decimal Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE Benutzer " +
							  " SET Guthaben = Guthaben - " + Wert.ToString().Replace(',','.') +
							  " WHERE Kundennummer = " + Kundennummer;
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

		
		#region "Add"


		public void AddBenutzer(string Benutzername,string Nachname)
		{
			string strSelect;
			int Benutzer = -1;
			DataTable dt = new DataTable();
			int Kundennummer = KundennummerGenerieren();
			// Prüfen ob bereits angelegt
			strSelect = "SELECT BenutzerID,Deleted FROM Benutzer WHERE Benutzername LIKE '" + Benutzername + "'";
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);
			// Anlegen
			if (dt.Rows.Count == 0)
			{
				try
				{
					ConnectionÖffnen();
					SqlCommand Com = new SqlCommand();
					Com.Connection = SQLConnection;
					Com.CommandText = "INSERT INTO Benutzer(Benutzername,Nachname,Kundennummer,Passwort,Guthaben,Deleted) " +
									"VALUES ('" + Benutzername + "',' " + Nachname + "'," + Kundennummer + ",'',0.00,0)";
					Com.ExecuteNonQuery();
					ConnectionSchließen();
				}
				catch (Exception e)
				{
					Except = e.Message;
				}
			}
			else
			{
				if ((bool)dt.Rows[0]["Deleted"] == false)
				{
					Except = "Ein Benutzer mit diesem Namen existiert bereits.";
				}
				else
				{	//Except = "Es existiert ein nicht entgültig gelöscht Benutzer mit diesem Namen. "+
					//"Löschen oder reaktivieren Sie diesen, um den gewünschten Namen verwenden zu können.";
					Benutzer = (int)dt.Rows[0]["BenutzerID"];
					UpdateUniversal("Benutzer", "BenutzerID", Benutzer, "Deleted", false);
					UpdateUniversal("Benutzer", "BenutzerID", Benutzer, "Nachname", Nachname);
					UpdateUniversal("Benutzer", "BenutzerID", Benutzer, "Vorname", null);
					UpdateUniversal("Benutzer", "BenutzerID", Benutzer, "Verkäufer", false);
					UpdateUniversal("Benutzer", "BenutzerID", Benutzer, "Kundennummer", Kundennummer);
					UpdateUniversal("Benutzer", "BenutzerID", Benutzer, "Passwort", "''");
					UpdateUniversal("Benutzer", "BenutzerID", Benutzer, "Guthaben", "0.00");	
				}
			}
		}

		public void AddArtikel(string Bezeichnung)
		{
			string strSelect;
			int Artikel = -1;
			DataTable dt = new DataTable();
			
			// Prüfen ob bereits angelegt
			strSelect = "SELECT ArtikelID FROM Artikel WHERE Artikelbezeichnung LIKE '" + Bezeichnung + "'";
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);
			// Anlegen
			if (dt.Rows.Count == 0)
			{
				try
				{
					ConnectionÖffnen();
					SqlCommand Com = new SqlCommand();
					Com.Connection = SQLConnection;
					Com.CommandText = "INSERT INTO Artikel(Artikelbezeichnung) " +
									"VALUES ('" + Bezeichnung + "')";
					Com.ExecuteNonQuery();
					ConnectionSchließen();
				}
				catch (Exception e)
				{
					Except = e.Message;
				}

				// Suchen des neuen Datensatzes
				try
				{
					strSelect = "SELECT ArtikelID FROM Artikel WHERE Artikelbezeichnung LIKE '" + Bezeichnung + "'";

					SqlDataAdapter da2 = new SqlDataAdapter(strSelect, SQLConnection);
					da.Fill(dt);

					Artikel = (int)dt.Rows[0]["ArtikelID"];
				}
				catch (Exception e)
				{
					Except = e.Message;
				}
			}
		}

		public void AddArtikel(string Bezeichnung, int EAN,int WarengruppeID,decimal Preis)
		{
			string strSelect;
			int Artikel = -1;
			DataTable dt = new DataTable();
			
			// Prüfen ob bereits angelegt
			strSelect = "SELECT ArtikelID FROM Artikel WHERE Artikelbezeichnung LIKE '" + Bezeichnung + "'";
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);
			// Anlegen
			if (dt.Rows.Count == 0)
			{
				try
				{
					ConnectionÖffnen();
					SqlCommand Com = new SqlCommand();
					Com.Connection = SQLConnection;
					Com.CommandText = "INSERT INTO Artikel(Artikelbezeichnung,EAN,WarengruppeID,Preis) " +
									"VALUES ('" + Bezeichnung + "'," + EAN + ", " + WarengruppeID + ", " + Convert.ToString(Preis).Replace(',', '.') + ")";
					Com.ExecuteNonQuery();
					ConnectionSchließen();
				}
				catch (Exception e)
				{
					Except = e.Message;
				}

				// Suchen des neuen Datensatzes
				try
				{
					strSelect = "SELECT ArtikelID FROM Artikel WHERE Artikelbezeichnung LIKE '" + Bezeichnung + "'";

					SqlDataAdapter da2 = new SqlDataAdapter(strSelect, SQLConnection);
					da.Fill(dt);

					Artikel = (int)dt.Rows[0]["ArtikelID"];
				}
				catch (Exception e)
				{
					Except = e.Message;
				}
			}
		}

		public void AddWarengruppe(string Bezeichnung)
		{
			string strSelect;
			int Warengruppe = -1;
			DataTable dt = new DataTable();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT WarengruppeID FROM Warengruppen WHERE BezeichnungWarengruppe LIKE '" + Bezeichnung + "'";
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);
			// Anlegen
			if (dt.Rows.Count == 0)
			{
				try
				{
					ConnectionÖffnen();
					SqlCommand Com = new SqlCommand();
					Com.Connection = SQLConnection;
					Com.CommandText = "INSERT INTO Warengruppen(BezeichnungWarengruppe) " +
									"VALUES ('" + Bezeichnung + "')";
					Com.ExecuteNonQuery();
					ConnectionSchließen();
				}
				catch (Exception e)
				{
					Except = e.Message;
				}

				// Suchen des neuen Datensatzes
				try
				{
					strSelect = "SELECT WarengruppeID FROM Warengruppen WHERE BezeichnungWarengruppe LIKE '" + Bezeichnung + "'";

					SqlDataAdapter da2 = new SqlDataAdapter(strSelect, SQLConnection);
					da.Fill(dt);

					Warengruppe = (int)dt.Rows[0]["WarengruppeID"];
				}
				catch (Exception e)
				{
					Except = e.Message;
				}
			}
		}

		public void AddVerkaufsLogEntry(DateTime Datum,int Kundennummer ,string Kassierer,string Aktion,string Artikel,decimal Betrag)
		{
			// Anlegen
			try
			{
				ConnectionÖffnen();
				SqlCommand Com = new SqlCommand();
				Com.Connection = SQLConnection;
				Com.CommandText = "INSERT INTO Verkaufslog(Datum,Kundennummer,Kassierer,Aktion,Artikel,Betrag) " +
								"VALUES ('" + Datum + "', " + Kundennummer + ", '" + Kassierer + "','" + Aktion + "','" 
								+ Artikel + "'," + Convert.ToString(Betrag).Replace(',', '.') + ")";
				Com.ExecuteNonQuery();
				ConnectionSchließen();
			}
			catch (Exception e)
			{
				Except = e.Message;
			}
		}

		#endregion

		#region "Update"


		public void UpdateUniversal(string Tabellenname, string IDFeld, int IDWert, string Feld, string Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;

			if (Wert != null)
			{
				Com.CommandText = "UPDATE " + Tabellenname +
								  " SET " + Feld + " = '" + Wert + "' " +
								  " WHERE " + IDFeld + " = " + IDWert;
			}
			else
			{
				Com.CommandText = "UPDATE " + Tabellenname +
									  " SET " + Feld + " = Null " +
									  " WHERE " + IDFeld + " = " + IDWert;
			}
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

		public void UpdateUniversal(string Tabellenname, string IDFeld, int IDWert, string Feld, int Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE " + Tabellenname +
							  " SET " + Feld + " = " + Wert +
							  " WHERE " + IDFeld + " = " + IDWert;
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

		public void UpdateUniversal(string Tabellenname, string IDFeld, int IDWert, string Feld, double Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE " + Tabellenname +
							  " SET " + Feld + " = " + Convert.ToString(Wert).Replace(',', '.') +
							  " WHERE " + IDFeld + " = " + IDWert;
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

		public void UpdateUniversal(string Tabellenname, string IDFeld, int IDWert, string Feld, decimal Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE " + Tabellenname +
							  " SET " + Feld + " = " + Convert.ToString(Wert).Replace(',', '.') +
							  " WHERE " + IDFeld + " = " + IDWert;
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

		public void UpdateUniversal(string Tabellenname, string IDFeld, int IDWert, string Feld, bool Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE " + Tabellenname +
							  " SET " + Feld + " = " + Convert.ToInt16(Wert) +
							  " WHERE " + IDFeld + " = " + IDWert;
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

		public void UpdateUniversal(string Tabellenname, string IDFeld, int IDWert, string Feld, char Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE " + Tabellenname +
							  " SET " + Feld + " = '" + Wert + "'" +
							  " WHERE " + IDFeld + " = " + IDWert;
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

		public void UpdateUniversal(string Tabellenname, string IDFeld, int IDWert, string Feld, DateTime Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE " + Tabellenname +
							  " SET " + Feld + " = '" + Wert + "'" +
							  " WHERE " + IDFeld + " = " + IDWert;
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}


		#endregion

		#region "Delete"


		public void DeleteBenutzer(int Kundennummer,bool final)
		{
			if (final)
			{
				ConnectionÖffnen();

				try
				{
					SqlCommand Com = new SqlCommand();
					Com.Connection = Con;
					Com.CommandText = "DELETE FROM Benutzer WHERE Kundennummer = " + Kundennummer;
					Com.ExecuteNonQuery();
				}
				finally
				{
					ConnectionSchließen();
				}
			}
			else
			{
				UpdateUniversal("Benutzer", "Kundennummer", Kundennummer, "Deleted", true);
			}
		}

		public void DeleteArtikel(int ArtikelID, bool final)
		{
			if (final)
			{
				ConnectionÖffnen();

				try
				{
					SqlCommand Com = new SqlCommand();
					Com.Connection = Con;
					Com.CommandText = "DELETE FROM Artikel WHERE ArtikelID = " + ArtikelID;
					Com.ExecuteNonQuery();
				}
				finally
				{
					ConnectionSchließen();
				}
			}
			else
			{
				UpdateUniversal("Artikel", "ArtikelID", ArtikelID, "Deleted", true);
			}
		}

		public void DeleteWarengruppe(int WarengruppeID, bool final)
		{
			if (final)
			{
				ConnectionÖffnen();

				try
				{
					SqlCommand Com = new SqlCommand();
					Com.Connection = Con;
					Com.CommandText = "DELETE FROM Warengruppen WHERE WarengruppeID = " + WarengruppeID;
					Com.ExecuteNonQuery();
				}
				finally
				{
					ConnectionSchließen();
				}
			}
			else
			{
				UpdateUniversal("Warengruppen", "WarengruppeID", WarengruppeID, "Deleted", true);
			}
		}

		#endregion

		#region "Get"


		#region "Benutzer"

		public DataTable GetAllBenutzer()
		{		
			string strSelect;
			DataTable dt = new DataTable();

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT * FROM Benutzer";
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;		
		}

		public DataTable GetAllBenutzerWithoutBC()
		{
			string strSelect;
			DataTable dt = new DataTable();

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT BenutzerID, Benutzername, Nachname, Vorname, Guthaben, Passwort, Verkäufer, Kundennummer, Deleted, Gesperrt " +
						"FROM Benutzer";
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

		public DataTable GetBenutzerAllByID(int BenutzerID)
		{
			string strSelect;
			DataTable dt = new DataTable();

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT * FROM Benutzer WHERE BenutzerID = " + BenutzerID;
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

		public DataTable GetBenutzerAllByKundennummer(int Kundennummer)
		{
			string strSelect;
			DataTable dt = new DataTable();

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT * FROM Benutzer WHERE Kundennummer = " + Kundennummer;
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

		public DataTable GetBenutzerAllByBenutzername(string Benutzername)
		{
			string strSelect;
			DataTable dt = new DataTable();

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT * FROM Benutzer WHERE Benutzername LIKE '" + Benutzername + "'";
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

		#endregion

		#region "Warengruppen"


		public DataTable GetAllWarengruppen()
		{
			string strSelect;
			DataTable dt = new DataTable();

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT * FROM Warengruppen";
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

		public DataTable GetWarengruppeAllByID(int WarengruppeID)
		{
			string strSelect;
			DataTable dt = new DataTable();

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT * FROM Warengruppen WHERE WarengruppeID = " + WarengruppeID;
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

		public DataTable GetWarengruppeAllByBezeichnung(string Bezeichnung)
		{
			string strSelect;
			DataTable dt = new DataTable();

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT * FROM Warengruppen WHERE WarengruppenBezeichnung LIKE '" + Bezeichnung + "'"; ;
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

		#endregion

		#region "Artikel"


		public DataTable GetAllArtikel()
		{
			string strSelect;
			DataTable dt = new DataTable();

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT * FROM Artikel LEFT OUTER JOIN Warengruppen ON Artikel.WarengruppeID = Warengruppen.WarengruppeID";
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

		public DataTable GetArtikelAllByID(int ArtikelID)
		{
			string strSelect;
			DataTable dt = new DataTable();

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT * FROM Artikel WHERE ArtikelID = " + ArtikelID;
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

		public DataTable GetArtikelAllByBezeichnung(string Bezeichnung)
		{
			string strSelect;
			DataTable dt = new DataTable();

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT * FROM Artikel WHERE Artikelbezeichnung LIKE '" + Bezeichnung + "'"; ;
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

		public DataTable GetArtikelAllByWarengruppeID(int WarengruppeID)
		{
			string strSelect;
			DataTable dt = new DataTable();

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT * FROM Artikel WHERE WarengruppeID = " + WarengruppeID;
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

		#endregion

		#region "Kontoauszug"

		public DataTable GetKontoauszug(int Kundennummer,Zeitraum Monat)
		{

			string strSelect;
			DataTable dt = new DataTable();
			int Month = 0;
			//Monat berechnen

			switch (Monat)
			{ 
				case Zeitraum.aktuellerMonat:
					Month = DateTime.Today.Month;
					break;
				case Zeitraum.letzerMonat:
					switch (DateTime.Today.Month - 1)
					{
						case 1:
							Month = 12;
							break;
						default:
							Month = DateTime.Today.Month - 1;
							break;
					}
					break;
			}

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT * FROM Verkaufslog WHERE (Kundennummer = " + Kundennummer + ") AND (Month(Datum) = " + Month +")";
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

		public DataTable GetKontoauszug(string Kassierer, Zeitraum Monat)
		{

			string strSelect;
			DataTable dt = new DataTable();
			int Month = 0;
			//Monat berechnen

			switch (Monat)
			{
				case Zeitraum.aktuellerMonat:
					Month = DateTime.Today.Month;
					break;
				case Zeitraum.letzerMonat:
					switch (DateTime.Today.Month - 1)
					{
						case 1:
							Month = 12;
							break;
						default:
							Month = DateTime.Today.Month - 1;
							break;
					}
					break;
			}

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT Verkaufslog.*, Benutzer.Benutzername FROM Verkaufslog INNER JOIN Benutzer ON Verkaufslog.Kundennummer = Benutzer.Kundennummer" +
						" WHERE (Kassierer = '" + Kassierer + "') AND (Month(Datum) = " + Month + ")";
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

		#endregion


		#endregion


	#endregion
	}
}
