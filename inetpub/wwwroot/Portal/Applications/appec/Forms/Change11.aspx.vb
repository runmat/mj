Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Data
Imports System.Data.OleDb

Partial Public Class Change11
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objHaendler As ec_17

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                objHaendler = New ec_17(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                If Session("objModel") Is Nothing Then
                    Session.Add("objModel", objHaendler)
                Else
                    Session("objModel") = objHaendler
                End If

                FillMarken()
            Else
                objHaendler = CType(Session("objModel"), ec_17)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub cmdContinue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdContinue.Click
        If (Not upFile.PostedFile Is Nothing) AndAlso (Not (upFile.PostedFile.FileName = String.Empty)) Then
            If Right(upFile.PostedFile.FileName.ToUpper, 4) <> ".XLS" AndAlso Right(upFile.PostedFile.FileName.ToUpper, 5) <> ".XLSX" Then

                lblError.Text = "Es können nur Dateien im .XLS oder .XLSX-Format verarbeitet werden."
                Exit Sub
            End If
        Else

            lblError.Text = "Keine Datei ausgewählt!"
            Exit Sub
        End If


        upload(upFile.PostedFile)
    End Sub

    Private Sub FillMarken()
        Dim Marken() As String = {"Volkswagen", "Sonstige"}

        ddlMarke.DataSource = Marken
        ddlMarke.DataBind()
    End Sub

    Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)

        Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
        Dim filename As String = ""
        Dim info As System.IO.FileInfo

        'Dateiname: User_yyyyMMddhhmmss.xls
        If Right(upFile.PostedFile.FileName.ToUpper, 4) = ".XLS" Then
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"
        End If
        If Right(upFile.PostedFile.FileName.ToUpper, 5) = ".XLSX" Then
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xlsx"
        End If

        Try
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

                Dim objCmdSelect As OleDbCommand

                Select Case ddlMarke.SelectedValue
                    Case "Volkswagen"
                        objCmdSelect = New OleDbCommand("SELECT * FROM [Bestellungen$]", objConn)
                    Case "Sonstige"
                        objCmdSelect = New OleDbCommand("SELECT * FROM [Tabelle1$]", objConn)
                End Select

                Dim objAdapter1 As New OleDbDataAdapter()
                objAdapter1.SelectCommand = objCmdSelect

                Dim objDataset1 As New DataSet()
                objAdapter1.Fill(objDataset1, "XLData")


                Dim TempTable As DataTable = objDataset1.Tables(0)

                objConn.Close()

                objHaendler.Model = New DataTable

                objHaendler.Model.Columns.Add("Fahrgestellnummer")
                objHaendler.Model.Columns.Add("Model")

                objHaendler.Model.AcceptChanges()

                Dim NewRow As DataRow

                Select Case ddlMarke.SelectedValue
                    Case "Volkswagen"
                        For Each dr As DataRow In TempTable.Rows

                            NewRow = objHaendler.Model.NewRow

                            If dr(9).ToString.Length = 0 Then Exit For

                            If dr(15).ToString.Length > 0 Then
                                NewRow("Fahrgestellnummer") = dr(13).ToString
                                NewRow("Model") = dr(8).ToString

                                objHaendler.Model.Rows.Add(NewRow)

                                objHaendler.Model.AcceptChanges()
                            End If
                        Next
                    Case "Sonstige"
                        For Each dr As DataRow In TempTable.Rows

                            NewRow = objHaendler.Model.NewRow

                            If dr(0).ToString.Length > 0 Then
                                NewRow("Fahrgestellnummer") = dr(0).ToString
                                NewRow("Model") = dr(1).ToString

                                objHaendler.Model.Rows.Add(NewRow)

                                objHaendler.Model.AcceptChanges()
                            End If
                        Next
                End Select

                objHaendler.SetAvis(Me.Page)

                Session("objModel") = objHaendler

                FillGrid(0)


            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim hochladen der Datei! " & ex.Message
        End Try

    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Try

            Dim tblTemp As DataTable = CType(Session("objModel"), ec_17).Result

            If tblTemp.Rows.Count = 0 Then
                GridView1.Visible = False
            Else
                GridView1.Visible = True
                ExcelCell.Visible = True
                Dim tmpDataView As New DataView()
                tmpDataView = tblTemp.DefaultView

                Dim intTempPageIndex As Int32 = intPageIndex
                Dim strTempSort As String = ""
                Dim strDirection As String = ""

                If strSort.Trim(" "c).Length > 0 Then
                    intTempPageIndex = 0
                    strTempSort = strSort.Trim(" "c)
                    If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "desc"
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    Else
                        strDirection = "desc"
                    End If

                    If strDirection = "asc" Then
                        strDirection = "desc"
                    Else
                        strDirection = "asc"
                    End If

                    ViewState("Sort") = strTempSort
                    ViewState("Direction") = strDirection
                Else
                    If Not ViewState("Sort") Is Nothing Then
                        strTempSort = ViewState("Sort").ToString
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "asc"
                            ViewState("Direction") = strDirection
                        Else
                            strDirection = ViewState("Direction").ToString
                        End If
                    End If
                End If

                If Not strTempSort.Length = 0 Then
                    tmpDataView.Sort = strTempSort & " " & strDirection
                End If

                GridView1.DataSource = tmpDataView
                GridView1.DataBind()


            End If
            If GridView1.PageCount > 1 Then
                GridView1.PagerStyle.CssClass = "PagerStyle"
                GridView1.PageIndex = intPageIndex
                GridView1.DataBind()
            End If
                

        Catch ex As Exception
            lblError.Text = "Daten konnten nicht geladen werden!"
        End Try
    End Sub

    Private Sub GridView1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging

        FillGrid(e.NewPageIndex)

    End Sub


    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click

        Dim tblExcel As DataTable = CType(Session("objModel"), ec_17).Result.Copy

        tblExcel.Columns(0).ColumnName = "Fahrgestellnummer"
        tblExcel.Columns(1).ColumnName = "Model ID"
        tblExcel.Columns(2).ColumnName = "Status"

        Try
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName

            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblExcel, Me.Page)

        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen der Excel-Datei: " + ex.Message
        End Try
    End Sub
End Class