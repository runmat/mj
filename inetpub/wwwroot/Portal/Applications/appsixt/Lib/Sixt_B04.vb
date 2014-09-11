Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class Sixt_B04
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Zugelassene_Fahrzeuge,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_strBriefnummer As String
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_strFahrgestellnummer As String
    Private m_strPDI As String
    Private m_strHaendlerID As String
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

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page _
                              , ByVal datEingangsdatumVon As DateTime, ByVal datEingangsdatumBis As DateTime, _
                                ByVal strPDI As String, ByVal strFahrgestellnummer As String)
        m_strClassAndMethod = "Sixt_B04.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)
            Dim strDatTempVon As String
            If IsDate(datEingangsdatumVon) Then
                strDatTempVon = CDate(datEingangsdatumVon).ToString
            Else
                strDatTempVon = ""
            End If
            Dim strDatTempBis As String
            If IsDate(datEingangsdatumBis) Then
                strDatTempBis = CDate(datEingangsdatumBis).ToString
            Else
                strDatTempBis = ""
            End If

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ZUGELASSENE_FAHRZEUGE", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", strKUNNR)
                'myProxy.setImportParameter("I_VKORG", "1510")
                'myProxy.setImportParameter("I_VDATU_VON", CStr(strDatTempVon))
                'myProxy.setImportParameter("I_VDATU_BIS", CStr(strDatTempBis))

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_ZUGELASSENE_FAHRZEUGE", "I_KUNNR, I_VKORG, I_VDATU_VON, I_VDATU_BIS", strKUNNR, "1510", strDatTempVon, strDatTempBis)

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")
                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")

                CreateOutPut(tblTemp2, strAppID)

                Dim rowTemp As DataRow
                Dim str As String
                Dim dtVersicherer As Admin.VersichererList
                Dim cn As New SqlClient.SqlConnection(m_objUser.App.Connectionstring)
                cn.Open()
                For Each rowTemp In m_tblResult.Rows
                    If (Not TypeOf rowTemp("Versicherer") Is System.DBNull) AndAlso (Not CStr(rowTemp("Versicherer")).Length = 0) Then
                        dtVersicherer = New Admin.VersichererList(CStr(rowTemp("Versicherer")), "%", cn, CInt(m_objUser.KUNNR))
                        If (dtVersicherer.Rows.Count = 0) Then
                            rowTemp("Versicherer") = "?"
                        Else
                            'Es soll der komplette Versicherungskürzel angezeigt werden, nicht nur die 1. Stelle..
                            str = CType(dtVersicherer.Rows(0)("Name1"), String)
                            If str.IndexOf("-") >= 0 Then
                                rowTemp("Versicherer") = Left(str, str.IndexOf("-") - 1)
                            Else
                                rowTemp("Versicherer") = str
                            End If
                        End If

                    End If
                Next
                cn.Close()
                WriteLogEntry(True, "VDATU_VON=" & datEingangsdatumVon.ToShortDateString & ", VDATU_BIS=" & datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZCARPORT=" & strPDI & ", ZZFAHRG=" & strFahrgestellnummer, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_PARAMETER"
                        m_strMessage = "Kein Suchkriterium gefüllt."
                    Case "NO_DATA"
                        m_strMessage = "Keine Ausgabedaten gefunden."
                    Case "NO_KORREKTPARAMETER"
                        m_strMessage = "Wenn nach Fahrgestellnummer gesucht wird, kann nicht gleichzeitig nach PDI und/oder Zulassungsdatum gesucht werden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "VDATU_VON=" & datEingangsdatumVon.ToShortDateString & ", VDATU_BIS=" & datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZCARPORT=" & strPDI & ", ZZFAHRG=" & strFahrgestellnummer & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Sixt_B04.vb $
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
' *****************  Version 8  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
