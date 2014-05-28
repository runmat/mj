Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports System.Data.OleDb

Public Class FMS_90
    REM § Lese-/Schreibfunktion, Kunde: Arval, 
    REM § Show - BAPI: Z_M_ZULF_FZGE_ARVAL,
    REM § Change - BAPI: ?.

    Inherits BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private strUploadFile As String
    Private m_ExcelTable As DataTable
#End Region

#Region " Properties"

    Public Property PUploadfile() As String
        Get
            Return strUploadFile
        End Get
        Set(ByVal Value As String)
            strUploadFile = Value
        End Set
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblResult
        End Get
        Set(ByVal Value As DataTable)
            m_tblResult = Value
        End Set
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overrides Sub Show()
    End Sub

    Public Overrides Sub Change()
    End Sub

    Public Sub GiveCars()
        Dim row As DataRow
        Dim tblTemp As DataTable

        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intIDSAP = -1

            Try
                S.AP.Init("Z_M_Fahrzeugabgleich")

                Dim tblFahrzeugUpload As DataTable = S.AP.GetImportTable("IT_CHASSIS_NUM")
                Dim rowFahrzeuge As DataRow

                S.AP.SetImportParameter("I_KUNNR", m_strKUNNR)
                S.AP.SetImportParameter("I_WEB_REPORT", "Report90.aspx")

                For Each rowData As DataRow In m_ExcelTable.Rows
                    If Not (TypeOf rowData(0) Is System.DBNull) Then
                        rowFahrzeuge = tblFahrzeugUpload.NewRow

                        rowFahrzeuge("CHASSIS_NUM") = CStr(rowData(0)).Trim(" "c)
                        tblFahrzeugUpload.Rows.Add(rowFahrzeuge)
                    End If
                Next

                S.AP.Execute()

                tblTemp = S.AP.GetExportTable("GT_WEB")

                m_tblResult = CreateOutPut(tblTemp, m_strAppID)

                m_intStatus = 0

                If (m_tblResult Is Nothing) OrElse (m_tblResult.Rows.Count = 0) Then
                    m_intStatus = -3331
                    m_strMessage = "Keine Daten gefunden."
                Else
                    For Each row In m_tblResult.Rows
                        If CStr(row("Status")) = String.Empty Then
                            row("Status") = "ok."
                        Else
                            row("Status") = "unbekannt."
                        End If
                    Next
                    m_tblResult.AcceptChanges()
                End If

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -3331
                        m_strMessage = "Keine Daten gefunden."
                    Case "NO_BAPI"
                        m_intStatus = -3332
                        m_strMessage = "Keine Eintrag in Prüftabelle vorhanden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub setMatch()

        Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                 "Data Source=" & strUploadFile & ";" & _
                 "Extended Properties=""Excel 8.0;HDR=NO;"""

        Dim objConn As New OleDbConnection(sConnectionString)
        objConn.Open()

        Dim objCmdSelect As New OleDbCommand("SELECT * FROM [Tabelle1$]", objConn)

        Dim objAdapter1 As New OleDbDataAdapter()
        objAdapter1.SelectCommand = objCmdSelect

        Dim objDataset1 As New DataSet()
        objAdapter1.Fill(objDataset1, "XLData")

        Dim logApp As New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        logApp.InitEntry(m_objUser.UserName, m_strSessionID, CInt(m_strAppID), m_objUser.Applications.Select("AppID = '" & m_strAppID & "'")(0)("AppFriendlyName").ToString, m_objUser.CustomerName, m_objUser.Customer.CustomerId, m_objUser.IsTestUser, 0)

        m_ExcelTable = objDataset1.Tables(0)

        objConn.Close()
        GiveCars()
    End Sub

#End Region

End Class

' ************************************************
' $History: FMS_90.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 15:58
' Updated in $/CKAG/Applications/appfms/Lib
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:44
' Created in $/CKAG/Applications/appfms/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 17:52
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 13:11
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' 
' ******************************************
