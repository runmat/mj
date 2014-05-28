Option Explicit On
Option Strict On
Option Compare Binary

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class AppDokVerw_03
    Inherits Base.Business.BankBase

#Region " Declarations"
    Private m_tblAuftraege As DataTable
    Private m_tblRaw As DataTable
    Private m_blnChangeMemo As Boolean
    Private m_blnChangeFaelligkeit As Boolean
    Private m_i_Kunnr As String
    Private configurationAppSettings As System.Configuration.AppSettingsReader
#End Region

#Region " Properties"
    Public ReadOnly Property Auftraege() As DataTable
        Get
            Return m_tblAuftraege
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_blnChangeFaelligkeit = False
        m_blnChangeMemo = False
        m_i_Kunnr = objUser.KUNNR.PadLeft(10, "0"c)
    End Sub

    Public Overrides Sub Show()
        m_strClassAndMethod = "AppDokVerw_03.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            configurationAppSettings = New System.Configuration.AppSettingsReader()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1
            Dim i As Int32

            Try
                m_intStatus = 0
                m_strMessage = ""

                S.AP.InitExecute("Z_M_Faellige_Equi_Lp", "I_KUNNR", m_i_Kunnr)

                m_tblRaw = S.AP.GetExportTable("GT_WEB")

                Dim j As Integer

                For j = m_tblRaw.Rows.Count - 1 To 0 Step -1
                    If (TypeOf (m_tblRaw.Rows(j)("ZZTMPDT")) Is System.DBNull) OrElse (m_tblRaw.Rows(j)("ZZTMPDT") Is Nothing) Then
                        m_tblRaw.Rows.RemoveAt(j)
                    End If
                Next

                m_tblAuftraege = CreateOutPut(m_tblRaw, m_strAppID)
                m_tblResultExcel = m_tblAuftraege.Copy
                m_tblResultExcel.Columns.Remove("EQUNR")
                m_tblAuftraege.Columns.Add("Memo alt", System.Type.GetType("System.String"))
                m_tblAuftraege.Columns.Add("MemoString", System.Type.GetType("System.String"))
                m_tblAuftraege.Columns.Add("DatumAendern", System.Type.GetType("System.Boolean"))
                Dim rowTemp2 As DataRow
                For i = m_tblAuftraege.Rows.Count - 1 To 0 Step -1
                    rowTemp2 = m_tblAuftraege.Rows(i)
                    If Not rowTemp2("Memo").ToString = String.Empty Then
                        rowTemp2("MemoString") = "Ändern"
                    Else
                        rowTemp2("MemoString") = "Erfassen"
                    End If
                Next
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_WEB"
                        m_intStatus = -1401
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_DATA"
                        m_intStatus = -1
                        m_strMessage = "Keine Daten gefunden."
                    Case "NO_HAENDLER"
                        m_intStatus = -1402
                        m_strMessage = "Händler nicht vorhanden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()
        If Not m_blnGestartet Then
            m_blnGestartet = True
            configurationAppSettings = New System.Configuration.AppSettingsReader()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try

                Dim strMemo_alt As String
                Dim strMemo As String
                Dim strEQUNR As String

                Dim tmpRow As DataRow
                For Each tmpRow In m_tblAuftraege.Rows
                    tmpRow.AcceptChanges()
                    strEQUNR = tmpRow("EQUNR").ToString

                    If tmpRow("Memo alt").ToString = tmpRow("Memo").ToString Then
                        strMemo_alt = tmpRow("Memo alt").ToString
                        strMemo = strMemo_alt
                        m_blnChangeMemo = False
                    Else
                        strMemo_alt = tmpRow("Memo alt").ToString
                        strMemo = tmpRow("Memo").ToString
                        m_blnChangeMemo = True
                    End If

                    If m_blnChangeMemo Then
                        m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Faellige_Equi_Update_Lp", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                        m_intStatus = 0
                        m_strMessage = ""

                        S.AP.InitExecute("Z_M_Faellige_Equi_Update_Lp", "I_KUNNR, I_EQUNR, I_TEXT200", m_i_Kunnr, strEQUNR, strMemo)
                    End If

                    If m_blnChangeMemo Then
                        tmpRow("Memo alt") = tmpRow("Memo")
                        If tmpRow("Memo alt").ToString.Length = 0 Then
                            tmpRow("MemoString") = "Erfassen"
                        Else
                            tmpRow("MemoString") = "Ändern"
                        End If
                    End If

                    tmpRow.AcceptChanges()
                Next
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_UPDATE"
                        m_intStatus = -1401
                        m_strMessage = "Kein EQUI-UPDATE."
                    Case "NO_TEXTAENDERUNG"
                        m_intStatus = -1402
                        m_strMessage = "Fehler bei der Textänderung."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region

End Class

' ************************************************
' $History: AppDokVerw_03.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/AppDokVerw/Lib
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 15.07.08   Time: 14:20
' Created in $/CKAG/Components/ComCommon/AppDokVerw/Lib
' ITA: 2081
' 
' ************************************************
