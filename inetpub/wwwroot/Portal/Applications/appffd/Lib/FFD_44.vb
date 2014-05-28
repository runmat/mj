Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class FFD_44

    Inherits Base.Business.DatenimportBase



#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        Dim intID As Int32 = -1

        'Kundennummer ermitteln und in SAP-Format wandeln
        Dim strKUNNR As String = Right("0000000000" & Me.m_objUser.Customer.KUNNR, 10)


        Try

            m_intStatus = 0
            m_strMessage = ""
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ffdimports_Anzeigen", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_WEB_REPORT", "Report44.aspx")

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Ffdimports_Anzeigen", "I_KUNNR,I_WEB_REPORT", Right("0000000000" & m_objUser.KUNNR, 10), "Report44.aspx")
            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            Dim row As DataRow
            Dim strZeit As String

            For Each row In tblTemp2.Rows
                Try
                    row.BeginEdit()
                    tblTemp2.Columns("UZEIT").MaxLength = 8
                    strZeit = CStr(row("UZEIT"))
                    If strZeit <> String.Empty Then
                        strZeit = Left(strZeit, 2) & ":" & strZeit.Substring(2, 2) & ":" & Right(strZeit, 2)
                    End If
                    row("UZEIT") = strZeit

                    row("ZDATENSAETZE") = CStr(row("ZDATENSAETZE")).TrimStart("0"c)

                    tblTemp2.Columns("ERFOLG").MaxLength = 50
                    Select Case row("ERFOLG").ToString
                        Case "X"
                            row("ERFOLG") = "<img src=""/Portal/Images/erfolg.gif"">"
                        Case "W"
                            row("ERFOLG") = "<img src=""/Portal/Images/warnung.gif"">"
                        Case "F"
                            row("ERFOLG") = "<img src=""/Portal/Images/fehler.gif"">"
                    End Select
                Catch ex As Exception
                    row("ERFOLG") = "Fehler."
                End Try
                row.AcceptChanges()
                row.EndEdit()
            Next

            CreateOutPut(tblTemp2, strAppID)
            WriteLogEntry(True, "KUNNR=" & strKUNNR, m_tblResult, False)

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                    m_intStatus = 999
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try


    End Sub
#End Region
End Class

' ************************************************
' $History: FFD_44.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.06.09   Time: 11:35
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 6  *****************
' User: Uha          Date: 12.03.07   Time: 16:32
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Imagepfade von Customize auf Images geändert
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' ************************************************
