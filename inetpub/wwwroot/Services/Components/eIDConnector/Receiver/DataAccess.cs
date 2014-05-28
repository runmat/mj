using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Data;

namespace Receiver
{
    public class DataAccess
    {

        public void SaveXml(String EncryptedXml, String SessionID)
        {

            String cnString = System.Configuration.ConfigurationManager.AppSettings["Connectionstring"];

            System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(cnString);

           

            try
            {
                cn.Open();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Delete from nPaData where SessionID = '" + SessionID + "'",cn);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "Insert into nPaData(SessionID, XmlData) Values(@SessionID,@XmlData)";

                cmd.Parameters.AddWithValue("@SessionID", SessionID);
                cmd.Parameters.AddWithValue("@XmlData", EncryptedXml);
                cmd.ExecuteNonQuery();
               

            }
            catch
            { }
            finally
            {
                cn.Close();
                cn.Dispose();
               
            }



        }


    }
}
