using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace Kantine
{
    public enum UserLevel { Nachmittagsmodus,Benutzersteuerung, Seller,Administration };
	
    public class KantinenBenutzer
	{
		#region "Global Declarations" 
		
		private string mName;
        private UserLevel mHighestUserLevel;

        private bool mIsAdmin;
        private bool mIsSeller;
        private bool mIsUseradmin;
        private bool mIsNachmittag;

		#endregion

		#region "Properties"

        public UserLevel HighestUserLevel
        {
            get { return mHighestUserLevel; }
        }

        public bool IsAdmin 
        {
            get {return mIsAdmin; }
        }

        public bool IsSeller
        {
            get { return mIsSeller; }
        }

        public bool IsUseradmin
        {
            get { return mIsUseradmin; }
        }

        public bool IsNachmittag 
        {
            get { return mIsNachmittag; }
        }

		public string Name
		{
			get { return mName; }
			set { mName = value; }
		}

		#endregion

		#region "Methods"
		
        /// <summary>
        /// Erzeugt ein Objekt vom Typ <c>KantinenBenutzer</c> der Benutzer ist standardmässig nur mit den einfachsten Berechtigungen als
        /// Nachmittagsbenutzer ausgestattet. Wird eine der anderen Berechtigungen gesetzt, verfallen die Nachmittagsbenutzerrechte
        /// </summary>
        /// <param name="name">Name des Benutzers</param>
        /// <param name="isadmin">Benutzer erhält Administratorrechte</param>
        /// <param name="isseller">Benutzer erhält Verkäuferrechte</param>
        /// <param name="isuseradmin">Benutzer erhält Benutzerverwaltungsrechte</param>
		public KantinenBenutzer(string name, bool isadmin, bool isseller, bool isuseradmin)
		{
            mName = name;
            mIsAdmin = isadmin;
            mIsSeller = isseller;
            mIsUseradmin = isuseradmin;
            mIsNachmittag = false;

            if (isadmin)
            {
                mHighestUserLevel = UserLevel.Administration;
            }
            else if(isuseradmin)
            {
                mHighestUserLevel= UserLevel.Benutzersteuerung;
            }
            else if (isseller)
            {
                mHighestUserLevel = UserLevel.Seller;
            }
            else 
            {
                mHighestUserLevel = UserLevel.Nachmittagsmodus;
                mIsNachmittag = true;
            }
		}

        public static string PasswortVerschlüsseln(string PW)
        {
            //SHA256 CryptService = SHA256CryptoServiceProvider.Create(); //HashWert ist mit 95 Zeichen zu lang für Passwortfeld
            MD5 CryptService = MD5CryptoServiceProvider.Create();
            Byte[] inputBytes = Encoding.UTF8.GetBytes(PW);
            Byte[] hashedBytes = CryptService.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);           
        }

		#endregion
	}
}
