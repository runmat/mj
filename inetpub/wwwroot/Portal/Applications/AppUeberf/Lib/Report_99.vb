Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Public Class Report_99
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Abm_Abgemeldete_Kfz,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private strKennzeichen As String
#End Region

#Region " Properties"
    Property PKennzeichen() As String
        Get
            Return strKennzeichen
        End Get
        Set(ByVal Value As String)
            strKennzeichen = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByRef User As Base.Kernel.Security.User, ByRef APP As CKG.Base.Kernel.Security.App, ByVal page As Web.UI.Page, _
                              ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "Porsche_04.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True
            
            Dim intID As Int32 = -1

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Zgbs_Ben_Zulassungsunt", APP, User, page)
                S.AP.Init("Z_M_Zgbs_Ben_Zulassungsunt")

                'Importparameter
                S.AP.SetImportParameter("I_ZKBA1", "")
                S.AP.SetImportParameter("I_ZKBA2", "")
                S.AP.SetImportParameter("I_ZKFZKZ", strKennzeichen)
                S.AP.SetImportParameter("I_AUSWAHL", "")

                'myProxy.callBapi()
                S.AP.Execute()

                m_tblResult = S.AP.GetExportTable("GT_WEB")

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -5555
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
             
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: Report_99.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 23.01.08   Time: 11:43
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' ITA:1371
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 16:40
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' SAPProxy_Ueberf erzeugt und wo nötig eingesetzt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 6.03.07    Time: 16:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' 
' ************************************************
