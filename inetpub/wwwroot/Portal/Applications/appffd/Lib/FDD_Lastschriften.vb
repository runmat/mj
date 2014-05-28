Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business


<Serializable()> _
Public Class FDD_Lastschriften
    Inherits CKG.Base.Business.DatenimportBase

#Region "Declarations"

    Private m_dteBaseDate As Date

#End Region

#Region "Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, ByVal dteBaseDate As Date)
        Dim intID As Int32 = -1

        Dim strKUNNR As String = Right("0000000000" & Me.m_objUser.Customer.KUNNR, 10)

        Try

            m_intStatus = 0
            m_strMessage = ""
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_Lastschrift_Get", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("KUNNR", strKUNNR)
            'myProxy.setImportParameter("DATUM", CStr(dteBaseDate.Date))

            'myProxy.callBapi()
            S.AP.InitExecute("Z_V_Lastschrift_Get", "KUNNR,DATUM", strKUNNR, CStr(dteBaseDate.Date))

            Dim tblTemp2 As DataTable
            tblTemp2 = S.AP.GetExportTable("OUT_TAB") 'myProxy.getExportTable("OUT_TAB")


            CreateOutPut(tblTemp2, strAppID)

            WriteLogEntry(True, "KUNNR=" & strKUNNR, m_tblResult, False)
            'myProxy = Nothing
        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NRF"
                    m_intStatus = -1402
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -5555
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & strKUNNR & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try


    End Sub

#End Region


End Class

' ************************************************
' $History: FDD_Lastschriften.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 12.05.10   Time: 18:23
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.06.09   Time: 11:35
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 15.06.09   Time: 17:08
' Updated in $/CKAG/Applications/appffd/Lib
' ITA 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 3  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 2  *****************
' User: Uha          Date: 22.05.07   Time: 13:20
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
