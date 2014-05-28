using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basics.Database;
using System.Data;
using System.Data.SqlClient;

namespace Kantine
{
	public enum Zeitraum { aktuellerMonat, letzerMonat };

	public enum LogFilter { Kassierer, Kundennummer }
	/// <summary>
	///		Klasse zur Unterstützung der Kommunikation mit dem Datenbankserver,
	///		Implementierung speziell für das Kantinenprojekt,
	///		erbt von DB_Kom
	/// </summary>
	public partial class DB_Kantine:DB_Kom
	{
		/// <summary>
		///		Klasse zur Unterstützung der Kommunikation mit dem Datenbankserver.
		///		Implementierung speziell für das Kantinenprojekt
		/// </summary>
		/// <param name="Connection">
		///		SQL-Connectionstring
		/// </param>
		/// 
		public DB_Kantine (string Connection): base(Connection)
		{
			
		}

		/// <summary>
		///		Generiert eine neue, eindeutige Kundennummer.
		/// </summary>
		public string KundennummerGenerieren()
		{
			string Nummer = "-1";
			Random rdm = new Random();
			bool unique = false;
			DataTable dtBenutzer = GetAllBenutzer();

			while (!unique)
			{
				Nummer = rdm.Next(100000000).ToString().PadLeft(8,'0');
				if (GetBenutzerAllByKundennummer(Nummer).Rows.Count == 0)
				{ unique = true; }
			}
			return Nummer;
		}

		/// <summary>
		///		Erhöht die aktuelle Kartennummer für eine Kundennummer.
		/// </summary>
		/// <param name="Kundennummer">Eine gültige Kundennummer.</param>
		/// <returns>gibt die neue Kartennummer zurück</returns>
		public int KartennummerNeuGenerieren(string Kundennummer)
		{
			DataTable dt;
			int KtNew = -1;

			dt = GetBenutzerAllByKundennummer(Kundennummer);

			if (dt.Rows.Count > 0)
			{
				KtNew = (int)dt.Rows[0]["Kartennummer"] + 1;
			}
			return KtNew;
		}

		public void Buchen(string Kundennummer, decimal Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE Benutzer " +
							  " SET Guthaben = Guthaben - " + Wert.ToString().Replace(',', '.') +
							  " WHERE Kundennummer = '" + Kundennummer+"'";
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

		#region "Add"


		public void AddBenutzer(string Benutzername, string Nachname)
		{
			string strSelect;
			int Benutzer = -1;
			DataTable dt = new DataTable();
			string Kundennummer = KundennummerGenerieren();
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
									"VALUES ('" + Benutzername + "','" + Nachname + "','" + Kundennummer + "','',0.00,0)";
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
                    UpdateUniversal("Benutzer", "BenutzerID", Benutzer, "Admin", false);
                    UpdateUniversal("Benutzer", "BenutzerID", Benutzer, "Useradmin", false);
					UpdateUniversal("Benutzer", "BenutzerID", Benutzer, "Seller", false);
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

		public void AddArtikel(string Bezeichnung, int EAN, int WarengruppeID, decimal Preis)
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

		public void AddVerkaufsLogEntry(DateTime Datum, string Kundennummer, string Kassierer, string Aktion, string Artikel, decimal Betrag)
		{
			// Anlegen
			try
			{
				ConnectionÖffnen();
				SqlCommand Com = new SqlCommand();
				Com.Connection = SQLConnection;
				Com.CommandText = "INSERT INTO Verkaufslog(Datum,Kundennummer,Kassierer,Aktion,Artikel,Betrag) " +
								"VALUES ('" + Datum + "', '" + Kundennummer + "', '" + Kassierer + "','" + Aktion + "','"
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


		#region "Delete"


		public void DeleteBenutzer(string Kundennummer, bool final)
		{
			if (final)
			{
				ConnectionÖffnen();

				try
				{
					SqlCommand Com = new SqlCommand();
					Com.Connection = Con;
					Com.CommandText = "DELETE FROM Benutzer WHERE Kundennummer = '" + Kundennummer+"'";
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
			strSelect = "SELECT BenutzerID, Benutzername, Nachname, Vorname, Guthaben, Passwort, Admin, Useradmin, Seller, Kundennummer, Deleted, Gesperrt " +
						"FROM Benutzer WHERE Deleted = 0";
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

		public DataTable GetBenutzerAllByKundennummer(string Kundennummer)
		{
			string strSelect;
			DataTable dt = new DataTable();

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			strSelect = "SELECT * FROM Benutzer WHERE Kundennummer = '" + Kundennummer +"'";
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

		//public DataTable GetKontoauszug(string Kundennummer, Zeitraum Monat)
		//{

		//    string strSelect;
		//    DataTable dt = new DataTable();
		//    int Month = 0;
		//    //Monat berechnen

		//    switch (Monat)
		//    {
		//        case Zeitraum.aktuellerMonat:
		//            Month = DateTime.Today.Month;
		//            break;
		//        case Zeitraum.letzerMonat:
		//            switch (DateTime.Today.Month - 1)
		//            {
		//                case 1:
		//                    Month = 12;
		//                    break;
		//                default:
		//                    Month = DateTime.Today.Month - 1;
		//                    break;
		//            }
		//            break;
		//    }

		//    ConnectionÖffnen();

		//    // Prüfen ob bereits angelegt
		//    strSelect = "SELECT * FROM Verkaufslog WHERE (Kundennummer = '" + Kundennummer + "') AND (Month(Datum) = " + Month + ")";
		//    SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
		//    da.Fill(dt);

		//    ConnectionSchließen();

		//    return dt;
		//}


        /// <summary>
        ///		Liefert eine Übersicht der Verkäufe in einem bestimmten Monat, abhängig vom Filter, 
        ///		zu einem bestimmten Verkäufer oder einem Kunden.
        /// </summary>
        /// <param name="Filterwert">Eine gültiger Benutzername oder eine Kundennummer, abhängig vom Wert des Filters</param>
        /// <param name="Monat">Der Monat der ausgewertet werden soll</param>
        /// <param name="Lf">Filter der angibt ob nach einer Kundennummer oder einem Benutzer gefiltert werden soll.</param>
        /// <returns>Liefert ein DataTable-Objekt mit allen Werten des gewählten Zeitraums</returns>
		public DataTable GetKontoauszug(string Filterwert, Zeitraum Monat,LogFilter Lf)
		{
			
			string strSelect = "";
			DataTable dt = new DataTable();
			int Month = 0;
            int Year = 0;
			//Datum berechnen

			switch (Monat)
			{
				case Zeitraum.aktuellerMonat:
					Month = DateTime.Today.Month;
                    Year = DateTime.Today.Year;
					break;
				case Zeitraum.letzerMonat:
					switch (DateTime.Today.Month - 1)
					{
						case 0:
							Month = 12;
                            Year = DateTime.Today.Year - 1;
							break;
						default:
							Month = DateTime.Today.Month - 1;
                            Year = DateTime.Today.Year;
							break;
					}
					break;
			}

			switch (Lf)
			{
				case LogFilter.Kassierer:
					
					switch (DateTime.Today.Month - 1)
					{
						case 0:
                            strSelect = "SELECT [Verkaufslog].*, [Benutzer].[Benutzername] " +
                                "FROM [Verkaufslog] INNER JOIN [Benutzer] " +
                                "ON [Verkaufslog].[Kundennummer] = [Benutzer].[Kundennummer] " +
                                "WHERE ([Verkaufslog].[Kassierer] = '" + Filterwert + "') AND (((Month([Datum]) >= " + Month + ") AND (Year([Datum]) = " + Year + ")) OR " +
                                "((Month([Datum]) >= 1) AND (Year([Datum]) = " + (Year + 1) + "))) ORDER BY [Datum]";
							break;
						default:
                            strSelect = "SELECT [Verkaufslog].*, [Benutzer].[Benutzername] " +
                                "FROM [Verkaufslog] INNER JOIN [Benutzer] " +
                                "ON [Verkaufslog].[Kundennummer] = [Benutzer].[Kundennummer] " +
                                "WHERE ([Verkaufslog].[Kassierer] = '" + Filterwert + "') AND (Month([Datum]) >= " + Month + ") AND (Year([Datum]) = " + Year + ") ORDER BY [Datum]";
							break;
					}
					break;                                
				case LogFilter.Kundennummer:
                    switch (DateTime.Today.Month - 1)
                    {
                        case 0:
                            strSelect = "SELECT * FROM Verkaufslog WHERE (Kundennummer = '" + Filterwert + "') AND (((Month([Datum]) >= " + Month + ") AND (Year([Datum]) = " + Year + ")) OR " +
                                "((Month([Datum]) >= 1) AND (Year([Datum]) = " + (Year + 1) + "))) ORDER BY [Datum]";
                            break;
                        default:
                            strSelect = "SELECT * FROM Verkaufslog WHERE (Kundennummer = '" + Filterwert + "') AND (Month([Datum]) >= " + Month + ") AND (Year([Datum]) = " + Year + ") ORDER BY [Datum]";
                            break;
                    }
                    break;
			}

			ConnectionÖffnen();

			// Prüfen ob bereits angelegt
			
			SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
			da.Fill(dt);

			ConnectionSchließen();

			return dt;
		}

        /// <summary>
        ///		Liefert eine Übersicht aller Verkäufe im gewählten Zeitraum.
        /// </summary>
        /// <param name="Von">Untere Datumsgrenze</param>
        /// <param name="Bis">Obere Datumsgrenze</param>
        /// <returns>Liefert ein DataTable-Objekt mit allen Werten des gewählten Zeitraums</returns>
        public DataTable GetKontoauszug(DateTime Von, DateTime Bis)
        {
            
            string strSelect = "";
            DataTable dt = new DataTable();
            string strVon = Von.ToShortDateString();
            string strBis = Bis.ToShortDateString();

            strSelect = "SELECT * FROM View_Verkaufslog WHERE (Datum >= '" + strVon + "') AND (Datum <= '" + strBis + "') ORDER BY [Datum]";
            
            ConnectionÖffnen();

            // Prüfen ob bereits angelegt

            SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
            da.Fill(dt);

            ConnectionSchließen();

            return dt;
        }

        /// <summary>
        ///		Liefert eine Übersicht aller Verkäufe im gewählten Zeitraum.
        /// </summary>
        /// <param name="Von">Untere Datumsgrenze</param>
        /// <param name="Bis">Obere Datumsgrenze</param>
        /// <returns>Liefert ein DataTable-Objekt mit allen Werten des gewählten Zeitraums</returns>
        public DataTable GetKontoauszug(DateTime Von, DateTime Bis,string Artikel)
        {

            string strSelect = "";
            DataTable dt = new DataTable();
            string strVon = Von.ToShortDateString();
            string strBis = Bis.ToShortDateString();

            strSelect = "SELECT * FROM View_Verkaufslog WHERE (Datum >= '" + strVon + "') AND (Datum <= '" + strBis + "') AND (Artikel = '" + Artikel + "') ORDER BY [Datum]";

            ConnectionÖffnen();

            // Prüfen ob bereits angelegt

            SqlDataAdapter da = new SqlDataAdapter(strSelect, SQLConnection);
            da.Fill(dt);

            ConnectionSchließen();

            return dt;
        }
		#endregion


		#endregion
	}
}
