Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class Sixt_B66
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Abm_Abgemeldete_Kfz,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_equtyp As Integer = 0
    '0=Alles
    '1=Brief
    '2=Schlüssel
#End Region

#Region " Properties"
    Public Property Typ() As Integer
        Get
            Return m_equtyp
        End Get
        Set(ByVal Value As Integer)
            m_equtyp = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub


    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strClassAndMethod = "Sixt_B66.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_VERSAUFTR_FEHLERHAFTE", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_VERSAUFTR_FEHLERHAFTE", "I_KUNNR", strKUNNR)

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")
                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")

                Dim row As DataRow
                Dim i As Integer

                tblTemp2.Columns.Add("EQTYP", GetType(System.String))

                If (m_equtyp = 1) Then    'Nur Briefe
                    For i = tblTemp2.Rows.Count - 1 To 0 Step -1
                        If CType(tblTemp2.Rows(i)("ZZBRFVERS"), String) = "0" Then
                            tblTemp2.Rows.RemoveAt(i)
                        End If
                    Next
                End If

                If (m_equtyp = 2) Then    'Nur Schlüssel
                    For i = tblTemp2.Rows.Count - 1 To 0 Step -1
                        If CType(tblTemp2.Rows(i)("ZZSCHLVERS"), String) = "0" Then
                            tblTemp2.Rows.RemoveAt(i)
                        End If
                    Next
                End If

                For Each row In tblTemp2.Rows
                    tblTemp2.Columns("ZZBRFVERS").MaxLength = 15
                    tblTemp2.Columns("ZZSCHLVERS").MaxLength = 15
                    tblTemp2.Columns("ZZNAME1_ZS").MaxLength = 120
                    If CType(row("ZZBRFVERS"), String).Trim = "1" Then
                        row("EQTYP") = "Brief"
                    End If
                    If CType(row("ZZSCHLVERS"), String).Trim = "1" Then
                        row("EQTYP") = "Schlüssel"
                    End If
                    row("ZZNAME1_ZS") = CType(row("ZZNAME1_ZS"), String) & "<br>" & CType(row("ZZSTRAS_ZS"), String) & "," & CType(row("ZZPSTLZ_ZS"), String) & "&nbsp;" & CType(row("ZZORT01_ZS"), String)
                Next
                tblTemp2.AcceptChanges()

                CreateOutPut(tblTemp2, strAppID)
                '§§§ JVE 27.10.2006
                m_tblResult.Columns.Add("Delete", GetType(System.Boolean))
                m_tblResult.Columns.Add("Status", GetType(System.String))
                m_tblResult.AcceptChanges()

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub


    Public Sub Delete(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, _
                      ByVal kennz As String, ByVal fahrg As String, ByVal brief As String, ByVal schluess As String)

        m_strClassAndMethod = "Sixt_B66.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_VERSAUFTR_FEHLERHAFTE_DEL", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_LICENSE_NUM", kennz)
                'myProxy.setImportParameter("I_CHASSIS_NUM", fahrg)
                'myProxy.setImportParameter("I_ZZBRFVERS", brief)
                'myProxy.setImportParameter("I_ZZSCHLVERS", schluess)

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_VERSAUFTR_FEHLERHAFTE_DEL", "I_KUNNR, I_LICENSE_NUM, I_CHASSIS_NUM, I_ZZBRFVERS, I_ZZSCHLVERS", strKUNNR, kennz, fahrg, brief, schluess)

            Catch ex As Exception
                m_intStatus = -9999

                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DELETE"
                        m_strMessage = "Datensatz konnte nicht gelöscht werden."
                    Case "NO_DATA"
                        m_strMessage = "Datensatz nicht gefunden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, Nothing, False)

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Sixt_B66.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 9  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 8  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
