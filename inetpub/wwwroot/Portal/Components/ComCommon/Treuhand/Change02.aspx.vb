Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.OleDb

Namespace Treuhand

    Partial Public Class Change02
        Inherits System.Web.UI.Page

#Region "Declarations"
        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private CustomerObject As SperreFreigabe
#End Region

#Region "Events"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            GetAppIDFromQueryString(Me)

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)


            If IsPostBack = False Then

                SetRadioButtonCustomer()

            ElseIf Not (Session("CustomerObject") Is Nothing) Then
                CustomerObject = CType(Session("CustomerObject"), SperreFreigabe)
            End If

        End Sub
        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
            If rb_Sperren.Visible Then
                rb_Sperren.Checked = True
            ElseIf rb_Entsperren.Visible = True Then
                rb_Entsperren.Checked = True
            End If
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

#End Region

#Region "Methods"

        Private Sub SetRadioButtonCustomer()

            CustomerObject = New SperreFreigabe(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

            CustomerObject.GetCustomer(Session("AppID").ToString, Session.SessionID.ToString)

            If CustomerObject.Result.Rows.Count > 0 Then

                Dim rdbText As String
                Dim rdbValue As String


                For xAGS As Integer = 0 To CustomerObject.Result.Rows.Count - 1
                    Dim rdvitem As New ListItem

                    If CustomerObject.Result.Rows(xAGS)("ZSELECT").ToString = "TG" Then
                        rdbText = CustomerObject.Result.Rows(xAGS)("NAME1_AG").ToString
                    Else
                        rdbText = CustomerObject.Result.Rows(xAGS)("NAME1_TG").ToString
                    End If

                    rdbValue = CustomerObject.Result.Rows(xAGS)("AG").ToString & "|" & CustomerObject.Result.Rows(xAGS)("TREU").ToString

                    rdvitem.Text = rdbText
                    rdvitem.Value = rdbValue
                    rdbCustomer.Items.Add(rdvitem)
                Next
            End If
            Session("CustomerObject") = CustomerObject
        End Sub
        Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)
            Try
                Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
                Dim filename As String
                Dim info As System.IO.FileInfo

                'Dateiname: User_yyyyMMddhhmmss.xls
                filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

                If Not (uFile Is Nothing) Then
                    uFile.SaveAs(ConfigurationManager.AppSettings("ExcelPath") & filename)
                    info = New System.IO.FileInfo(filepath & filename)
                    If Not (info.Exists) Then
                        lblError.Text = "Fehler beim Speichern."
                        Exit Sub
                    End If

                    'Datei gespeichert -> Auswertung
                    Dim sConnectionString As String = ""
                    If Right(upFile.PostedFile.FileName.ToUpper, 4) = ".XLS" Then
                        sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                         "Data Source=" & filepath & filename & ";" & _
                         "Extended Properties=""Excel 8.0;HDR=YES;"""
                    Else
                        sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filepath + filename + _
                        ";Extended Properties=""Excel 12.0 Xml;HDR=YES"""
                    End If


                    Dim objConn As New OleDbConnection(sConnectionString)
                    objConn.Open()

                    Dim objCmdSelect As New OleDbCommand("SELECT * FROM [Tabelle1$]", objConn)

                    Dim objAdapter1 As New OleDbDataAdapter()
                    objAdapter1.SelectCommand = objCmdSelect

                    Dim objDataset1 As New DataSet()
                    objAdapter1.Fill(objDataset1, "XLData")


                    CheckInputTable(objDataset1.Tables(0))

                    objConn.Close()

                    If Not CustomerObject.tblUpload.Rows Is Nothing AndAlso CustomerObject.tblUpload.Rows.Count > 0 Then
                        CustomerObject.Treugeber = rdbCustomer.SelectedValue.Split("|"c)(1).ToString
                        Session("CustomerObject") = CustomerObject
                        Response.Redirect("Change02_2.aspx?AppID=" & Session("AppID").ToString)
                    Else
                        lblError.Text = "Datei enthielt keine verwendbaren Daten."
                    End If
                End If
            Catch ex As Exception
                lblError.Text = "Datei enthielt keine verwendbaren Daten."
            End Try
        End Sub
        Private Sub CheckInputTable(ByVal tblInput As DataTable)
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            Session.Add("logObj", logApp)

            Dim i As Integer = 0
            Dim rowData As DataRow
            CustomerObject.GiveResultStructure()
            For Each rowData In tblInput.Rows
                i += 1
                If TypeOf rowData(0) Is System.DBNull And TypeOf rowData(1) Is System.DBNull Then Exit For

                Dim strEquiKey As String = ""
                If Not TypeOf rowData(0) Is System.DBNull Then
                    strEquiKey = CStr(rowData(0)).Trim(" "c)
                End If
                Dim strDate As String = ""
                If Not TypeOf rowData(1) Is System.DBNull Then
                    strDate = CStr(rowData(1)).Trim(" "c)
                End If
                Dim Referenznummer As String = ""

                If rowData.Table.Columns.Count > 2 Then
                    If Not TypeOf rowData(2) Is System.DBNull Then
                        Referenznummer = CStr(rowData(2)).Trim(" "c)
                    End If
                End If

                Dim rowNew As DataRow
                rowNew = CustomerObject.tblUpload.NewRow

                rowNew("AG") = rdbCustomer.SelectedValue.Split("|"c)(0).ToString
                rowNew("EQUI_KEY") = strEquiKey
                rowNew("ERNAM") = m_User.UserName
                rowNew("ERDAT") = Now.ToShortDateString
                If IsDate(strDate) Then
                    rowNew("SPERRDAT") = strDate
                Else
                    rowNew("SPERRDAT") = Now.ToShortDateString
                End If
                If rb_Sperren.Checked = True Then
                    rowNew("TREUH_VGA") = "S"
                    CustomerObject.SperrEnsperr = "gesperrt"
                Else
                    rowNew("TREUH_VGA") = "F"
                    CustomerObject.SperrEnsperr = "entsperrt"
                End If
                rowNew("SUBRC") = ""
                rowNew("ZZREFERENZ2") = Referenznummer



                'Pruefen, ob schon in der Autorisierung.
                Dim strInitiator As String = ""
                Dim intAuthorizationID As Int32
                m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, rdbCustomer.SelectedItem.Text, rowNew("EQUI_KEY").ToString, m_User.IsTestUser, strInitiator, intAuthorizationID)
                If Not strInitiator.Length = 0 Then
                    'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                    rowNew("MESSAGE") = "Aut."
                    rowNew("SELECT") = ""
                Else
                    rowNew("MESSAGE") = ""
                    rowNew("SELECT") = "99"
                End If

                CustomerObject.tblUpload.Rows.Add(rowNew)
            Next
            CustomerObject.Treunehmer = rdbCustomer.SelectedItem.Text
        End Sub
#End Region

        Protected Sub rdbCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbCustomer.SelectedIndexChanged
            tblUpload.Visible = True

        End Sub

        Protected Sub cmdWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdWeiter.Click
            If rdbCustomer.SelectedIndex = -1 Then
                lblError.Text = "Bitte wählen Sie einen Treunehmer aus!"
            Else
                'Prüfe Fehlerbedingung
                If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
                    If (Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" AndAlso Right(upFile.PostedFile.FileName.ToUpper, 5) <> ".XLSX") Then

                        lblError.Text = "Es können nur Dateien im .XLS oder .XLSX-Format verarbeitet werden."
                        Exit Sub
                    End If
                Else

                    lblError.Text = "Keine Datei ausgewählt!"
                    Exit Sub
                End If

                'Lade Datei
                upload(upFile.PostedFile)
            End If

        End Sub
    End Class

End Namespace
