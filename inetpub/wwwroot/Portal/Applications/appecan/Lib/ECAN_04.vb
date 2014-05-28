Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class ECAN_04
    REM § Status-Report, Kunde: ECAN, BAPI: Z_M_Versandfreigabe_Brief,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub
    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "ECAN_03.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_VERSANDFREIGABE_BRIEF", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

                myProxy.callBapi()

                Dim tblTemp2 As DataTable = myProxy.getExportTable("OT_FAHRZG")
                CreateOutPut(tblTemp2, strAppID)

                Dim tmpRow As DataRow
                If m_tblResult.Columns.Count > 0 Then
                    m_tblResult.Columns("Versandart").MaxLength = 10
                End If
                For Each tmpRow In m_tblResult.Rows
                    If CStr(tmpRow("Versandart")) = "1" Then
                        tmpRow("Versandart") = "temporär"
                    ElseIf CStr(tmpRow("Versandart")) = "2" Then
                        tmpRow("Versandart") = "endgültig"
                    End If
                Next

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "ERR_NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Fahrzeuge gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: ECAN_04.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.02.10    Time: 16:49
' Updated in $/CKAG/Applications/appecan/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 10:57
' Updated in $/CKAG/Applications/appecan/Lib
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:07
' Created in $/CKAG/Applications/appecan/Lib
' 
' *****************  Version 1  *****************
' User: Uha          Date: 9.07.07    Time: 17:12
' Created in $/CKG/Applications/AppECAN/AppECANWeb/Lib
' Report "Auction Report ohne Vorlage ZBII" hinzugefügt
' 
' ************************************************
