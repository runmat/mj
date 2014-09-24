Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class Sixt_B09
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Zugelassene_Fahrzeuge_Fibu,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datErfassungsdatumVon As DateTime
    Private m_datErfassungsdatumBis As DateTime
    Private m_tblHistory As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub
    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, _
                              ByVal datErfassungsdatumVon As DateTime, ByVal datErfassungsdatumBis As DateTime)
        m_strClassAndMethod = "Sixt_B09.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try
                Dim strAnzahl As String = ""
                Dim strDatTempVon As String
                If IsDate(datErfassungsdatumVon) Then
                    strDatTempVon = datErfassungsdatumVon.ToShortDateString
                Else
                    strDatTempVon = ""
                End If
                Dim strDatTempBis As String
                If IsDate(datErfassungsdatumBis) Then
                    strDatTempBis = datErfassungsdatumBis.ToShortDateString
                Else
                    strDatTempBis = ""
                End If


                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ZUGELASSENE_FAHRZEUGE_FIBU", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_VKORG", "1510")
                'If strDatTempVon.Length > 0 Then
                '    myProxy.setImportParameter("I_ERDAT_LOW", strDatTempVon)
                'End If
                'If strDatTempBis.Length > 0 Then
                '    myProxy.setImportParameter("I_ERDAT_HIGH", strDatTempVon)
                'End If

                'myProxy.callBapi()

                S.AP.Init("Z_M_ZUGELASSENE_FAHRZEUGE_FIBU", "I_KUNNR, I_VKORG", Right("0000000000" & m_objUser.KUNNR, 10), "1510")

                If strDatTempVon.Length > 0 Then
                    S.AP.SetImportParameter("I_ERDAT_LOW", strDatTempVon)
                End If
                If strDatTempBis.Length > 0 Then
                    S.AP.SetImportParameter("I_ERDAT_HIGH", strDatTempVon)
                End If

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")
                Dim tblTemp2 As DataTable = S.AP.GetExportTableWithExecute("GT_WEB")

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & datErfassungsdatumVon.ToShortDateString & ", ERDAT_HIGH=" & datErfassungsdatumBis.ToShortDateString, m_tblResult, False)


            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten gefunden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Sixt_B09.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
