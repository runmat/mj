Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Components.ComCommon.Treuhand
Imports System.Data.OleDb
Namespace Treuhand
    Partial Public Class Change100s
        Inherits System.Web.UI.Page

#Region "Declarations"
        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private CustomerObject As SperreFreigabe
        Private treuKey As New Dictionary(Of String, String)

#End Region

#Region "Events"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

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

        Protected Sub rdbCustomer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdbCustomer.SelectedIndexChanged
            trUpload.Visible = True
            'trTabName.Visible = True nicht gewünscht
            trAktion.Visible = True


            Dim name As String
            For xAGS As Integer = 0 To CustomerObject.Result.Rows.Count - 1

                If CustomerObject.Result.Rows(xAGS)("ZSELECT").ToString = "TG" Then
                    name = CustomerObject.Result.Rows(xAGS)("NAME1_AG").ToString
                Else
                    name = CustomerObject.Result.Rows(xAGS)("NAME1_TG").ToString
                End If

                If name = rdbCustomer.SelectedItem.Text Then
                    If CustomerObject.Result.Rows(xAGS)("TREUH_KEY").ToString = "F" Then
                        InfoText.Text = "Bitte nehmen Sie den Excel Upload mit der</br> Fahrgestellnummer vor"
                    Else
                        InfoText.Text = "Bitte nehmen Sie den Excel Upload mit der</br> Vertragsnummer vor"
                    End If

                    Exit For
                End If

            Next

            pnInfo.Visible = True


        End Sub

#End Region

#Region "Methods"

        Private Sub SetRadioButtonCustomer()

            CustomerObject = New SperreFreigabe(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

            CustomerObject.GetCustomer(Me.Page, Session("AppID").ToString, Session.SessionID.ToString)

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

                    rdbValue = CustomerObject.Result.Rows(xAGS)("AG").ToString & "|" & CustomerObject.Result.Rows(xAGS)("TREU").ToString & "|" & CustomerObject.Result.Rows(xAGS)("ZSELECT").ToString

                    rdvitem.Text = rdbText
                    rdvitem.Value = rdbValue

                    rdbCustomer.Items.Add(rdvitem)

                Next
            End If

            Session("CustomerObject") = CustomerObject
        End Sub
        Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)

            Dim objConn As OleDbConnection = Nothing

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

                    objConn = New OleDbConnection(sConnectionString)
                    objConn.Open()

                    Dim tab As String = "Tabelle1$"
                    ' '''''''''''''
                    'Dim dt As DataTable
                    'Dim ds As DataSet = New DataSet()
                    'dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})

                    'If dt Is Nothing Then

                    '    Return
                    'End If

                    'Dim sheetCount As Integer = dt.Rows.Count - 1
                    'Dim excelSheets(sheetCount) As String

                    'Dim i As Integer = 0

                    'For Each row As DataRow In dt.Rows

                    '    excelSheets(i) = row("TABLE_NAME").ToString()
                    '    i += 1
                    'Next

                    'tab = excelSheets(0)



                    ''Wenn TAbellenname wählbar sei sollte (Momentan aukommentiertin ASPX und code behind)
                    'tab As String = Trim(txtTabName.Text)
                    'If String.IsNullOrEmpty(tab) Then
                    '    tab = "Tabelle1"
                    'End If

                    ' Dim objCmdSelect As New OleDbCommand("SELECT * FROM [Tabelle1$]", objConn)

                    Dim objAdapter1 As New OleDbDataAdapter()


                    Dim objCmdSelect As New OleDbCommand("SELECT * FROM [" + tab + "]", objConn)
                    objAdapter1.SelectCommand = objCmdSelect

                    Dim objDataset1 As New DataSet()
                    objAdapter1.Fill(objDataset1, "XLData")

                    CheckInputTable(objDataset1.Tables(0))

                    objConn.Close()

                    If Not CustomerObject.tblUpload.Rows Is Nothing AndAlso CustomerObject.tblUpload.Rows.Count > 0 Then

                        Dim tmp As String() = rdbCustomer.SelectedValue.Split("|"c)
                        CustomerObject.Treugeber = tmp(1).ToString

                        CustomerObject.AG = tmp(0).ToString
                        CustomerObject.TREU = tmp(1).ToString
                        CustomerObject.ZSelect = tmp(2).ToString

                        If rb_Sperren.Checked Then
                            CustomerObject.IsSperren = True
                        Else
                            CustomerObject.IsSperren = False
                        End If


                        Session("CustomerObject") = CustomerObject
                        Response.Redirect("Change100s_2.aspx?AppID=" & Session("AppID").ToString)
                    Else
                        lblError.Text = "Datei enthielt keine verwendbaren Daten."
                    End If
                End If
            Catch ex As Exception
                lblError.Text = "Bitte Format der Importdatei und den Tabellennamen (Tabelle1) prüfen."
                lblError.Visible = True

                If objConn Is Nothing Then
                    objConn.Close()
                End If

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

                rowNew("ID") = CustomerObject.tblUpload.Rows.Count + 1

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
                rowNew("ERROR") = ""


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



        Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
            If rdbCustomer.SelectedIndex = -1 Then
                lblError.Visible = True
                lblError.Text = "Bitte wählen Sie einen Treunehmer aus!"
            Else
                'Prüfe Fehlerbedingung
                If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
                    If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                        lblError.Visible = True
                        lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                        Exit Sub
                    End If
                Else
                    lblError.Visible = True
                    lblError.Text = "Keine Datei ausgewählt!"
                    Exit Sub
                End If

                'Lade Datei
                upload(upFile.PostedFile)
            End If

        End Sub

        Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
            Session("CustomerObject") = Nothing
            Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub
    End Class
End Namespace

' ************************************************
' $History: Change100s.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 9.07.10    Time: 12:18
' Updated in $/CKAG2/Services/Components/ComCommon/Treuhand
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 9.07.10    Time: 11:26
' Updated in $/CKAG2/Services/Components/ComCommon/Treuhand
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 2.07.10    Time: 15:59
' Updated in $/CKAG2/Services/Components/ComCommon/Treuhand
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 4.05.10    Time: 16:06
' Updated in $/CKAG2/Services/Components/ComCommon/Treuhand
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 29.03.10   Time: 13:56
' Updated in $/CKAG2/Services/Components/ComCommon/Treuhand
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 22.03.10   Time: 11:51
' Updated in $/CKAG2/Services/Components/ComCommon/Treuhand
' ITA: 3539
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 16.03.10   Time: 16:56
' Created in $/CKAG2/Services/Components/ComCommon/Treuhand
' 