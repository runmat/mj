Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Drawing


Partial Public Class Change03_1
    Inherits Page

    Private m_User As Base.Kernel.Security.User
    Private mObjUploadZulassung As UploadZulassung

    Protected WithEvents lblError As Label
    Protected WithEvents lblNoData As Label
    Protected WithEvents DataGrid1 As DataGrid
    Protected WithEvents lblPageTitle As Label
    Protected WithEvents lblHead As Label
    Protected WithEvents lbBack As LinkButton

    Protected WithEvents lbweiter As LinkButton
    Protected WithEvents imgbExcel As ImageButton

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            lblError.Text = ""

            If Not IsPostBack Then
                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

            If mObjUploadZulassung Is Nothing Then
                If Session("mObjUploadZulassungSession") Is Nothing Then
                    Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
                Else
                    mObjUploadZulassung = CType(Session("mObjUploadZulassungSession"), UploadZulassung)
                End If
            End If

            FillGrid(0)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If mObjUploadZulassung.Status = 0 Then

            If IsNothing(mObjUploadZulassung.ZulassungsTabelle) OrElse mObjUploadZulassung.ZulassungsTabelle.Rows.Count = 0 Then

                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                DataGrid1.Visible = False
                lblNoData.Visible = True
            Else

                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView(mObjUploadZulassung.ZulassungsTabelle)

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

                DataGrid1.CurrentPageIndex = intTempPageIndex
                DataGrid1.DataSource = tmpDataView
                DataGrid1.DataBind()

                DataGrid1.Columns(4).Visible = (mObjUploadZulassung.ArtDerZulassung = UploadZulassung.Zulassungstyp.Planzulassung)

                For Each tmpItem As DataGridItem In DataGrid1.Items
                    Dim tmpLabel As Label
                    tmpLabel = CType(tmpItem.FindControl("lblMessage"), Label)

                    If lbweiter.Enabled = False Then 'wenn fertig oder fehler 
                        CType(tmpItem.FindControl("lbdelete"), LinkButton).Visible = False
                    End If

                    Dim tmpStr As String = mObjUploadZulassung.ZulassungsTabelle.Select("Fahrgestellnummer='" & tmpItem.Cells(0).Text.Replace("&nbsp;", "") & "'")(0)("STATUS").ToString

                    If Not tmpStr.Trim(" "c) = "" Then
                        If tmpStr = "Für Zulassung bereit" Then
                            tmpLabel.Text = "<img src=../../../Images/erfolg.gif border=0> " & tmpStr
                            tmpLabel.ForeColor = Color.Green
                        ElseIf tmpStr.Contains("Zulassung erfolgreich beauftragt.") Then
                            tmpLabel.Text = "<img src=../../../Images/erfolg.gif border=0> " & tmpStr
                            tmpLabel.ForeColor = Color.Green
                        ElseIf tmpStr = "Fehlerhafte Uploaddatei, Fahrzeug wird ignoriert" Then
                            tmpLabel.Text = "<img src=../../../Images/fehler.gif border=0> " & tmpStr
                            tmpLabel.ForeColor = Color.Red
                        Else
                            tmpLabel.Text = "<img src=../../../Images/fehler.gif border=0> " & tmpStr
                            tmpLabel.ForeColor = Color.Red
                        End If
                    End If
                Next

            End If
        Else
            lblError.Text = mObjUploadZulassung.Message
            Exit Sub
        End If

    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles DataGrid1.ItemCommand

        If e.CommandName = "Delete" Then
            mObjUploadZulassung.ZulassungsTabelle.Select("Fahrgestellnummer='" & e.CommandArgument.ToString & "'")(0).Delete()
            mObjUploadZulassung.ZulassungsTabelle.AcceptChanges()
            FillGrid(0)
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("Change03.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub lbweiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbweiter.Click

        If lbweiter.Text = "Daten ergänzen" Then
            mObjUploadZulassung.Show()
            If mObjUploadZulassung.Status = 0 Then
                FillGrid(0)
                lbweiter.Text = "Zulassung beauftragen"
                imgbExcel.Visible = True
            Else
                lblError.Text = mObjUploadZulassung.Message
            End If

        ElseIf lbweiter.Text = "Zulassung beauftragen" Then
            mObjUploadZulassung.change()
            If mObjUploadZulassung.Status = 0 Then
                FillGrid(0)
            Else
                lblError.Text = mObjUploadZulassung.Message
            End If

        End If

    End Sub

    Protected Sub imgbExcel_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles imgbExcel.Click
        Try
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim sColName As String

            Dim AppURL As String = Replace(Request.Url.LocalPath, "/Portal", "..")
            Dim tblTranslations As DataTable = CType(Session(AppURL), DataTable)
            Dim tblTemp As DataTable = mObjUploadZulassung.ZulassungsTabelle.Copy
            For Each col As DataGridColumn In DataGrid1.Columns
                For i As Integer = tblTemp.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper OrElse col2.ColumnName.ToUpper = col.HeaderText.ToUpper.Replace("COL_", "") Then
                        sColName = TranslateColLbtn(DataGrid1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            tblTemp.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                    'EQUNR nicht mit in Excel ausgeben, wird meist nur als boundcolumn versteckt als schlüssel verwendet JJU2008.10.23
                    If col2.ColumnName.ToUpper = "EQUNR" Then
                        tblTemp.Columns.Remove(col2)
                    End If
                    'wenn keine Planzulassung, Verarbeitungsdatum nicht mit ausgeben
                    If col2.ColumnName.ToUpper = "VERARBEITUNGSDATUM" AndAlso mObjUploadZulassung.ArtDerZulassung = UploadZulassung.Zulassungstyp.Zulassung Then
                        tblTemp.Columns.Remove(col2)
                    End If
                Next
                tblTemp.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Page)
        Catch ex As Exception
            lblError.Text = "Beim erstellen des Exceldatei ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

End Class

' ************************************************
' $History: Change03_1.aspx.vb $
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 29.05.11   Time: 15:33
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 30.10.09   Time: 11:14
' Updated in $/CKAG/Applications/AppAvis/forms
' ita�s: 3216, 3155
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 28.04.09   Time: 17:09
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 31.03.09   Time: 11:48
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA 2758
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 20.03.09   Time: 15:33
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA 2739 unfertig
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 11.03.09   Time: 13:16
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 21.11.08   Time: 14:54
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA 2412 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 21.11.08   Time: 9:59
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA 2412 testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 20.11.08   Time: 14:08
' Created in $/CKAG/Applications/AppAvis/forms
' ITa 2412 torso
' 
' ************************************************