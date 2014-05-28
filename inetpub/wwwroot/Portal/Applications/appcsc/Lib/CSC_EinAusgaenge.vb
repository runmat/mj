Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class CSC_EinAusgaenge
    REM § Status-Report, Kunde: CNC, BAPI: Zz_Csc_Daten_Einaus_Report_001,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' BankBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_blnAll As Boolean
    Private m_datAbDatum As Date
    Private m_datBisDatum As Date
    Private m_strAction As String
#End Region

#Region " Properties"

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal datAbDatum As Date, ByVal datBisDatum As Date, ByVal strAction As String, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_blnAll = False
        m_datAbDatum = datAbDatum
        m_datBisDatum = datBisDatum
        m_strAction = strAction
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "CSC_EinAusgaenge.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try
                S.AP.Init("ZZ_CSC_DATEN_EINAUS_REPORT_001")

                S.AP.SetImportParameter("KUNNR", strKUNNR)
                S.AP.SetImportParameter("ACTION", m_strAction)
                S.AP.SetImportParameter("DATANF", m_datAbDatum.ToString)
                S.AP.SetImportParameter("DATEND", m_datBisDatum.ToString)

                S.AP.Execute()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("EINNEU")

                CreateOutPut(tblTemp2, strAppID)

            Catch ex As Exception
                m_intStatus = -1111
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: CSC_EinAusgaenge.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.03.10   Time: 13:34
' Updated in $/CKAG/Applications/appcsc/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:37
' Created in $/CKAG/Applications/appcsc/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 16:46
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 9:28
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Lib
' 
' ************************************************
