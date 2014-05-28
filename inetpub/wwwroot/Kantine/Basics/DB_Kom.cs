using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Printing;

namespace Basics.Database
{
	public class DB_Kom
	{
		/// <summary>
		///		Die Klasse stellt unterstützende Methoden und Funktionen für Datenbankzugriffe bereit.
		/// </summary>
		protected SqlConnection Con = new SqlConnection();

		protected String Except;

		protected int Verbindungsanforderungen = 0;

		public void crypt()
		{
			AesManaged CryptAES = new AesManaged();
			CryptAES.CreateDecryptor();
		}

		#region "Properties"

		/// <summary>
		/// Enthält eine Fehlerbeschreibung für den Fall das eine Methode oder Funktion eine Ausnahme verursacht.
		/// </summary>
		
		public string Exception
		{
			get { return Except; }
		}

		/// <summary>
		/// Liefert oder setzt die SQL connection.
		/// </summary>
		/// <value>
		/// The SQL connection.
		/// </value>
		public SqlConnection SQLConnection
		{
			get	{return Con;}
			set	{Con = value;}
		}

		#endregion

	#region "Methods / Functions"

		public DB_Kom(string ConnectionString)
		{
			SQLConnection.ConnectionString = ConnectionString;
		}

		protected void ConnectionÖffnen()
		{
			Verbindungsanforderungen++;

			if (SQLConnection.State != ConnectionState.Open)
			{
				SQLConnection.Open();
			}
		}

		protected void ConnectionSchließen()
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


		#region "Update"

        // Updates mit string IDWert

		public void UpdateUniversal(string Tabellenname, string IDFeld, string IDWert, string Feld, string Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;

			if (Wert != null)
			{
				Com.CommandText = "UPDATE " + Tabellenname +
								  " SET " + Feld + " = '" + Wert + "' " +
								  " WHERE " + IDFeld + " = '" + IDWert+"'";
			}
			else
			{
				Com.CommandText = "UPDATE " + Tabellenname +
									  " SET " + Feld + " = Null " +
									  " WHERE " + IDFeld + " = '" + IDWert + "'";
			}
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

		public void UpdateUniversal(string Tabellenname, string IDFeld, string IDWert, string Feld, int Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE " + Tabellenname +
							  " SET " + Feld + " = " + Wert +
							  " WHERE " + IDFeld + " = '" + IDWert + "'";
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

        public void UpdateUniversal(string Tabellenname, string IDFeld, string IDWert, string Feld, long Wert)
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

        public void UpdateUniversal(string Tabellenname, string IDFeld, string IDWert, string Feld, uint Wert)
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

		public void UpdateUniversal(string Tabellenname, string IDFeld, string IDWert, string Feld, double Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE " + Tabellenname +
							  " SET " + Feld + " = " + Convert.ToString(Wert).Replace(',', '.') +
							  " WHERE " + IDFeld + " = '" + IDWert + "'";
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

		public void UpdateUniversal(string Tabellenname, string IDFeld, string IDWert, string Feld, decimal Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE " + Tabellenname +
							  " SET " + Feld + " = " + Convert.ToString(Wert).Replace(',', '.') +
							  " WHERE " + IDFeld + " = '" + IDWert + "'";
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

		public void UpdateUniversal(string Tabellenname, string IDFeld, string IDWert, string Feld, bool Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE " + Tabellenname +
							  " SET " + Feld + " = " + Convert.ToInt16(Wert) +
							  " WHERE " + IDFeld + " = '" + IDWert + "'";
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

		public void UpdateUniversal(string Tabellenname, string IDFeld, string IDWert, string Feld, char Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE " + Tabellenname +
							  " SET " + Feld + " = '" + Wert + "'" +
							  " WHERE " + IDFeld + " = '" + IDWert + "'";
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

		public void UpdateUniversal(string Tabellenname, string IDFeld, string IDWert, string Feld, DateTime Wert)
		{
			Except = "";

			ConnectionÖffnen();

			SqlCommand Com = new SqlCommand();

			Com.Connection = SQLConnection;
			Com.CommandText = "UPDATE " + Tabellenname +
							  " SET " + Feld + " = '" + Wert + "'" +
							  " WHERE " + IDFeld + " = '" + IDWert + "'";
			Com.ExecuteNonQuery();

			ConnectionSchließen();
		}

        // Updates mit Int IDWert

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

        public void UpdateUniversal(string Tabellenname, string IDFeld, int IDWert, string Feld, long Wert)
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

        public void UpdateUniversal(string Tabellenname, string IDFeld, int IDWert, string Feld, uint Wert)
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

		
	#endregion
	}
}
