using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using CKG.Base.Kernel.Security;
using CKG.Base.Business;
using System.Data.SqlClient;

namespace AutohausPortal.lib
{
    public class InfoCenterData : BankBase
    {
        #region Declarations

        private DataTable m_tblDocumentTypes = new DataTable();
        private DataTable m_tblDocuments = new DataTable();
        private DataTable m_tblDocumentRights = new DataTable();
        private string m_uploadfile;

        #endregion

        #region Properties

        public DataTable DocumentTypes
        {
            get { return m_tblDocumentTypes; }
            set { m_tblDocumentTypes = value; }
        }

        public DataTable Documents
        {
            get { return m_tblDocuments; }
            set { m_tblDocuments = value; }
        }

        public DataTable DocumentRights
        {
            get { return m_tblDocumentRights; }
            set { m_tblDocumentRights = value; }
        }

        public string UploadFile
        {
            get { return m_uploadfile; }
            set { m_uploadfile = value; }
        }

        #endregion

        #region Methods

        public InfoCenterData(ref User objUser, ref App objApp, string strAppID, string strSessionID, string strFileName)
            :base(ref objUser, ref objApp, strAppID, strSessionID, strFileName)
        {
        }

        public void GetDocumentTypes()
        {
            using (SqlConnection cn = new SqlConnection(m_objApp.Connectionstring))
            {
                cn.Open();

                m_tblDocumentTypes.Clear();

                SqlDataAdapter daDocTypes = new SqlDataAdapter("SELECT * FROM DocumentType WHERE CustomerID = @CustomerID OR DocumentTypeId = @DocumentTypeId", cn);
                daDocTypes.SelectCommand.Parameters.AddWithValue("@CustomerID", m_objUser.Customer.CustomerId);
                daDocTypes.SelectCommand.Parameters.AddWithValue("@DocumentTypeId", 1);
                daDocTypes.Fill(m_tblDocumentTypes);

                cn.Close();
            }
        }

        /// <summary>
        /// Gibt alle Dokumente für eine Gruppe oder alle Dokumente des Kunden zurück
        /// </summary>
        /// <param name="groupId"></param>
        public void GetDocuments(int groupId = -1)
        {
            SqlDataAdapter daDocs;

            using (SqlConnection cn = new SqlConnection(m_objApp.Connectionstring))
            {
                cn.Open();

                m_tblDocuments.Clear();

                if (groupId == -1)
                {
                    daDocs = new SqlDataAdapter("SELECT * FROM vwDocumentWithType WHERE customerId = @customerId", cn);
                    daDocs.SelectCommand.Parameters.AddWithValue("@customerId", m_objUser.Customer.CustomerId);
                }
                else
                {
                    daDocs = new SqlDataAdapter("SELECT * FROM vwDocumentGroup WHERE groupId = @groupId", cn);
                    daDocs.SelectCommand.Parameters.AddWithValue("@groupId", groupId);
                }
                
                daDocs.Fill(m_tblDocuments);

                cn.Close();
            }
        }

        public void GetDocumentRights(int documentId)
        {
            using (SqlConnection cn = new SqlConnection(m_objApp.Connectionstring))
            {
                cn.Open();

                m_tblDocumentRights.Clear();

                SqlDataAdapter daDocs = new SqlDataAdapter("SELECT * FROM documentRights WHERE documentId = @documentId", cn);
                daDocs.SelectCommand.Parameters.AddWithValue("@documentId", documentId);
                daDocs.Fill(m_tblDocumentRights);

                cn.Close();
            }
        }

        public void SaveDocumentType(int docTypeId, string docType)
        {
            DataRow tablerow;
            DataRow[] rows;

            using (SqlConnection cn = new SqlConnection(m_objApp.Connectionstring))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;

                if (docTypeId == -1)
                {
                    cmd.CommandText = "INSERT INTO DocumentType (docTypeName,customerId) VALUES (@docTypeName,@customerId);SELECT SCOPE_IDENTITY() AS 'Identity'";
                    cmd.Parameters.AddWithValue("@docTypeName", docType);
                    cmd.Parameters.AddWithValue("@customerId", m_objUser.Customer.CustomerId);
                    int newId = Convert.ToInt32(cmd.ExecuteScalar());
                    tablerow = m_tblDocumentTypes.NewRow();
                    tablerow["documentTypeId"] = newId;
                    tablerow["docTypeName"] = docType;
                    tablerow["customerId"] = m_objUser.Customer.CustomerId;
                    m_tblDocumentTypes.Rows.Add(tablerow);
                }
                else
                {
                    cmd.CommandText = "UPDATE DocumentType SET docTypeName = @docTypeName WHERE documentTypeId = @documentTypeId;";
                    cmd.Parameters.AddWithValue("@docTypeName", docType);
                    cmd.Parameters.AddWithValue("@documentTypeId", docTypeId);
                    cmd.ExecuteNonQuery();
                    tablerow = m_tblDocumentTypes.Select("documentTypeId=" + docTypeId)[0];
                    tablerow["docTypeName"] = docType;
                    rows = m_tblDocuments.Select("docTypeId=" + docTypeId);
                    foreach (DataRow dr in rows)
                    {
                        dr["docTypeName"] = docType;
                    }
                }

                m_tblDocumentTypes.AcceptChanges();
                m_tblDocuments.AcceptChanges();

                cn.Close();
            }
        }

        public void SaveDocument(int documentId, int docTypeId, string fileName = "", string fileType = "", DateTime? fileLastEdited = null, long fileSize = 0)
        {
            DataRow tablerow;

            DateTime jetzt = DateTime.Now;

            using (SqlConnection cn = new SqlConnection(m_objApp.Connectionstring))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;

                if (documentId == -1)
                {
                    // prüfen, ob die Datei schon existiert, also nur überschrieben werden soll, oder wirklich DB-seitig neu angelegt werden muss
                    cmd.CommandText = "SELECT documentId FROM Document WHERE customerId = @CustomerId AND fileName = @fileName AND fileType = @fileType;";
                    cmd.Parameters.AddWithValue("@customerId", m_objUser.Customer.CustomerId);
                    cmd.Parameters.AddWithValue("@fileName", fileName);
                    cmd.Parameters.AddWithValue("@fileType", fileType);
                    object gefundeneId = cmd.ExecuteScalar();

                    if (gefundeneId != null)
                    {
                        cmd.CommandText = "UPDATE Document SET uploaded = @uploaded WHERE documentId = @documentId;";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@uploaded", jetzt);
                        cmd.Parameters.AddWithValue("@documentId", Convert.ToInt32(gefundeneId));
                        cmd.ExecuteNonQuery();
                        tablerow = m_tblDocuments.Select("documentId='" + Convert.ToInt32(gefundeneId) + "'")[0];
                        tablerow["lastEdited"] = jetzt;
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO Document (docTypeId,customerId,fileName,fileType,lastEdited,fileSize,uploaded) VALUES (@docTypeId,@customerId,@fileName,@fileType,@lastEdited,@fileSize,@uploaded);SELECT SCOPE_IDENTITY() AS 'Identity'";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@docTypeId", docTypeId);
                        cmd.Parameters.AddWithValue("@customerId", m_objUser.Customer.CustomerId);
                        cmd.Parameters.AddWithValue("@fileName", fileName);
                        cmd.Parameters.AddWithValue("@fileType", fileType);
                        cmd.Parameters.AddWithValue("@lastEdited", fileLastEdited);
                        cmd.Parameters.AddWithValue("@fileSize", fileSize);
                        cmd.Parameters.AddWithValue("@uploaded", jetzt);
                        int newId = Convert.ToInt32(cmd.ExecuteScalar());
                        tablerow = m_tblDocuments.NewRow();
                        tablerow["documentId"] = newId;
                        tablerow["docTypeId"] = docTypeId;
                        tablerow["docTypeName"] = m_tblDocumentTypes.Select("documentTypeId=" + docTypeId)[0]["docTypeName"];
                        tablerow["customerId"] = m_objUser.Customer.CustomerId;
                        tablerow["fileName"] = fileName;
                        tablerow["fileType"] = fileType;
                        tablerow["lastEdited"] = fileLastEdited;
                        m_tblDocuments.Rows.Add(tablerow);
                    }
                }
                else
                {
                    // aktuell nur Dokumententyp als veränderbarer Parameter vorgesehen
                    cmd.CommandText = "UPDATE Document SET docTypeId = @docTypeId WHERE documentId = @documentId;";
                    cmd.Parameters.AddWithValue("@docTypeId", docTypeId);
                    cmd.Parameters.AddWithValue("@documentId", documentId);
                    cmd.ExecuteNonQuery();
                    tablerow = m_tblDocuments.Select("documentId='" + documentId + "'")[0];
                    tablerow["docTypeId"] = docTypeId;
                    tablerow["docTypeName"] = m_tblDocumentTypes.Select("documentTypeId=" + docTypeId)[0]["docTypeName"];
                }

                m_tblDocuments.AcceptChanges();

                cn.Close();
            }
        }
    
        public void SaveDocumentRights(int documentId, List<int> groups)
        {
            DataRow tablerow;

            if ((groups != null) && (groups.Count > 0))
            {
                using (SqlConnection cn = new SqlConnection(m_objApp.Connectionstring))
                {
                    cn.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "INSERT INTO DocumentRights (documentId,groupId) VALUES (@documentId,@groupId);";

                    foreach (int gruppe in groups)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@documentId", documentId);
                        cmd.Parameters.AddWithValue("@groupId", gruppe);
                        cmd.ExecuteNonQuery();
                        tablerow = m_tblDocumentRights.NewRow();
                        tablerow["documentId"] = documentId;
                        tablerow["groupId"] = gruppe;
                        m_tblDocumentRights.Rows.Add(tablerow);
                    }

                    m_tblDocumentRights.AcceptChanges();

                    cn.Close();
                }
            }
        }

        public bool DeleteDocumentType(int docTypeId)
        {
            DataRow tablerow;

            using (SqlConnection cn = new SqlConnection(m_objApp.Connectionstring))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;

                // prüfen, ob DocType noch verwendet wird -> dann nicht löschen
                cmd.CommandText = "SELECT COUNT(*) FROM Document WHERE DocTypeId=" + docTypeId + " ;";
                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                {
                    return false;
                }

                cmd.CommandText = "DELETE FROM DocumentType WHERE DocumentTypeId=" + docTypeId + " ;";
                cmd.Parameters.Clear();
                cmd.ExecuteNonQuery();
                tablerow = m_tblDocumentTypes.Select("documentTypeId='" + docTypeId + "'")[0];
                tablerow.Delete();
                m_tblDocumentTypes.AcceptChanges();

                cn.Close();
            }

            return true;
        }

        public void DeleteDocument(int documentId)
        {
            DataRow tablerow;
            DataRow[] rows;

            using (SqlConnection cn = new SqlConnection(m_objApp.Connectionstring))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;

                // ggf. vorhandene Verknüpfungen entfernen
                cmd.CommandText = "DELETE FROM DocumentRights WHERE DocumentId=" + documentId + " ;";
                cmd.ExecuteNonQuery();
                if ((m_tblDocumentRights != null) && (m_tblDocumentRights.Rows.Count > 0))
                {
                    rows = m_tblDocumentRights.Select("documentId='" + documentId + "'");
                    foreach (DataRow dr in rows)
                    {
                        dr.Delete();
                    }
                    m_tblDocumentRights.AcceptChanges();
                }

                cmd.CommandText = "DELETE FROM Document WHERE DocumentId=" + documentId + " ;";
                cmd.Parameters.Clear();
                cmd.ExecuteNonQuery();
                tablerow = m_tblDocuments.Select("documentId='" + documentId + "'")[0];
                tablerow.Delete();
                m_tblDocuments.AcceptChanges();

                cn.Close();
            }
        }

        public void DeleteDocumentRights(int documentId, List<int> groups)
        {
            DataRow tablerow;

            if ((groups != null) && (groups.Count > 0))
            {
                using (SqlConnection cn = new SqlConnection(m_objApp.Connectionstring))
                {
                    cn.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "DELETE FROM DocumentRights WHERE documentId = @documentId AND groupId = @groupId;";

                    foreach (int gruppe in groups)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@documentId", documentId);
                        cmd.Parameters.AddWithValue("@groupId", gruppe);
                        cmd.ExecuteNonQuery();
                        tablerow = m_tblDocumentRights.Select("documentId='" + documentId + "' AND groupId='" + gruppe + "'")[0];
                        tablerow.Delete();
                    }

                    m_tblDocumentRights.AcceptChanges();

                    cn.Close();
                }
            }
        }

        public override void Show()
        {
        }

        public override void Change()
        {
        }

        #endregion

    }
}