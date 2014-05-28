Imports System
Imports CKG.Base.Kernel
Imports Microsoft.Data.SAPClient
Imports CKG.Base.Business
Imports CKG.Base
Imports CKG.Base.Common


Public Class UploadSperr_Entsperr
    Inherits DatenimportBase
#Region " Declarations"
    Private m_tblUpload As DataTable
    Private m_tblEntsperren As DataTable
    Private m_tblErledigt As DataTable
    Private mSAPTabelleZulassung As DataTable
    Private m_intFehlerCount As Integer
    Private m_strTask As String
    Private mE_SUBRC As String = ""
    Private mE_MESSAGE As String = ""
#End Region
#Region " Properties"
    Public Property tbUpload() As DataTable
        Get
            If m_tblUpload Is Nothing Then
                m_tblUpload = New DataTable
                With m_tblUpload
                    .Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
                    .Columns.Add("SPERRVERMERK", System.Type.GetType("System.String"))
                End With
                m_tblUpload.AcceptChanges()
            End If
            Return m_tblUpload
        End Get
        Set(ByVal value As DataTable)
            m_tblUpload = value
        End Set
    End Property
    Public Property Erledigt() As DataTable
        Get
            Return m_tblErledigt
        End Get
        Set(ByVal value As DataTable)
            m_tblErledigt = value
        End Set
    End Property
    Public Property Task() As String
        Get
            Return m_strTask
        End Get
        Set(ByVal value As String)
            m_strTask = value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Kernel.Security.User, ByRef objApp As Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

    Public Overloads Sub save(ByVal strAppID As String, _
                            ByVal strSessionID As String, _
                            ByVal page As Web.UI.Page)
        m_strClassAndMethod = "UploadSperr_Entsperr.Change"

        Dim intID As Int32 = -1
        m_intStatus = 0


        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_ZULASSUNGSSPERRE_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                myProxy.setImportParameter("I_ACTION", m_strTask)

                Dim SAPTable As DataTable = myProxy.getImportTable("GT_IN")
                Dim tblSAPRow As DataRow


                For Each uploadRow As DataRow In m_tblUpload.Rows
                    tblSAPRow = SAPTable.NewRow
                    tblSAPRow("CHASSIS_NUM") = uploadRow("CHASSIS_NUM").ToString
                    tblSAPRow("SPERRVERMERK") = uploadRow("SPERRVERMERK").ToString
                    SAPTable.Rows.Add(tblSAPRow)
                Next

                myProxy.callBapi()

                m_tblErledigt = myProxy.getExportTable("GT_OUT")
                m_tblErledigt.Columns.Add("SPERRVERMERK", System.Type.GetType("System.String"))



                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")
                For Each row In m_tblErledigt.Rows
                    Dim drow() As DataRow = m_tblUpload.Select("CHASSIS_NUM='" & row("CHASSIS_NUM").ToString & "'")
                    If drow.Length = 1 Then
                        row("SPERRVERMERK") = drow(0)("SPERRVERMERK").ToString
                    Else
                        row("SPERRVERMERK") = ""
                    End If
                Next

                If mE_SUBRC <> "0" Then
                    m_intStatus = CInt(mE_SUBRC)
                    m_strMessage = mE_MESSAGE
                End If

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub generateUploadTable(ByVal tmpTable As DataTable)

        Dim tmpNewRow As DataRow

        If Not tbUpload Is Nothing Then
            tbUpload.Clear()
        End If
        Try
            For Each tmpRow As DataRow In tmpTable.Rows
                tmpNewRow = tbUpload.NewRow

                If Not tmpTable.Rows(0) Is tmpRow Then
                    'nur nicht die überschriftstzeile bitte
                    If tmpRow(0).ToString.Length > 0 Then
                        tmpNewRow("CHASSIS_NUM") = tmpRow(0).ToString
                        If tmpTable.Columns.Count = 2 Then
                            tmpNewRow("SPERRVERMERK") = tmpRow(1).ToString
                        End If

                        tbUpload.Rows.Add(tmpNewRow)
                    Else
                        Exit For
                    End If

                End If
            Next
            tbUpload.AcceptChanges()
            HelpProcedures.killAllDBNullValuesInDataTable(tbUpload)
        Catch ex As Exception
            m_intStatus = -111
            m_strMessage = "Die Übergabetabelle konnte nicht generiert werden, überprüfen Sie Ihre Exceldatei."
        End Try
    End Sub
#End Region
End Class
