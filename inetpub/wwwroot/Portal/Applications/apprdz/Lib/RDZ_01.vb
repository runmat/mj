Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class RDZ_01
    REM § Status-Report, Kunde: RDZ, BAPI: Z_M_Bapirdz,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_strKennzeichen As String
    Private m_strPLZ As String
    Private m_strZulassungspartner As String
    Private m_strZulassungspartnerNr As String
    Private m_tblResultRaw As DataTable
    Private m_Page As Page
#End Region

#Region " Properties"
    Public ReadOnly Property ResultRaw() As DataTable
        Get
            Return m_tblResultRaw
        End Get
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID, m_strKennzeichen, m_strPLZ, m_strZulassungspartner, m_strZulassungspartnerNr, m_Page)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strKennzeichen As String, ByVal strPLZ As String, ByVal strZulassungspartner As String, ByVal strZulassungspartnerNr As String, ByVal page As Page)
        m_strClassAndMethod = "RDZ_01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try

                S.AP.Init("Z_M_BAPIRDZ")

                S.AP.setImportParameter("IZKFZKZ", strKennzeichen)
                S.AP.setImportParameter("IPOST_CODE1", strPLZ)
                S.AP.setImportParameter("INAME1", strZulassungspartner)
                S.AP.setImportParameter("IREMARK", strZulassungspartnerNr)

                m_tblResultRaw = S.AP.GetExportTableWithExecute("ITAB")

                CreateOutPut(m_tblResultRaw, strAppID)
                m_tblResult.Columns.Add("Details", System.Type.GetType("System.String"))
                Dim tmpRow As DataRow
                For Each tmpRow In m_tblResult.Rows
                    tmpRow("Details") = "<a href=""../Applications/AppRDZ/Forms/Report30_3.aspx?ID=" & tmpRow("ID").ToString & """ Target=""_blank"">Anzeige</a>"
                Next
                m_tblResult.Columns.Remove("ID")
                WriteLogEntry(True, "ZKFZKZ=" & strKennzeichen & ", POST_CODE1=" & strPLZ & ", NAME1=" & strZulassungspartner & ", REMARK=" & strZulassungspartnerNr, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

                WriteLogEntry(False, "ZKFZKZ=" & strKennzeichen & ", POST_CODE1=" & strPLZ & ", NAME1=" & strZulassungspartner & ", REMARK=" & strZulassungspartnerNr & ", " & ex.Message, m_tblResult, False)
            Finally

                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: RDZ_01.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:59
' Created in $/CKAG/Applications/apprdz/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 3.07.07    Time: 9:09
' Updated in $/CKG/Applications/AppRDZ/AppRDZWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 11:14
' Updated in $/CKG/Applications/AppRDZ/AppRDZWeb/Lib
' 
' ************************************************
