Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Partial Public Class Report29
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_User As Security.User
    Private m_App As Security.App

    Private Suche As Search

    Protected WithEvents ucStyles As Global.CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As Global.CKG.Portal.PageElements.Header
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Security.App(m_User)


            If IsPostBack = False Then

                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                tdExcel.Visible = False

            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub


    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
        If Page.IsValid = False Then Exit Sub

        If Len(txtKennzeichen.Text.Trim & txtDatumVon.Text.Trim & txtDatumBis.Text.Trim) = 0 Then
            lblPanelError.Visible = True
            Exit Sub
        End If

        DoSubmit()
    End Sub


    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub


    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Try
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim AppURL As String
            Dim TempTable As DataTable = CType(Session("BestandTable"), DataTable).Clone
            Dim AusgabeTable As DataTable = CType(Session("BestandTable"), DataTable).Copy
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim sColName As String = ""

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)


            Dim FieldControl As DataControlField
            Dim Found As Boolean = False


            For Each col2 In TempTable.Columns

                Found = False


                For Each FieldControl In grvAusgabe.Columns

                    If col2.ColumnName.ToUpper = FieldControl.SortExpression Then
                        Found = True

                        sColName = TranslateColLbtn(grvAusgabe, tblTranslations, FieldControl.HeaderText, bVisibility)

                        AusgabeTable.Columns(col2.ColumnName).ColumnName = sColName
                        If FieldControl.Visible = False Then Found = False

                        Exit For

                    End If


                Next

                If Found = False Then
                    AusgabeTable.Columns.Remove(AusgabeTable.Columns(col2.ColumnName))

                End If

                AusgabeTable.AcceptChanges()
            Next

            If AusgabeTable.Columns.Count > 0 Then
                Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
                excelFactory.CreateDocumentAndSendAsResponse(strFileName, AusgabeTable, Me.Page)
            Else
                Err.Raise(-1, , "Feldübersetzung prüfen.")
            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub Image1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Image1.Click

        If pnlSuche.Visible = True Then
            pnlSuche.Visible = False
            Image1.ImageUrl = "/Portal/images/expand.jpg"
            lblOpenClose.Text = Replace(lblOpenClose.Text, "Schließen", "Öffnen")
        Else
            pnlSuche.Visible = True
            Image1.ImageUrl = "/Portal/images/collapse.jpg"
            lblOpenClose.Text = Replace(lblOpenClose.Text, "Öffnen", "Schließen")

        End If


    End Sub


    Private Sub grvAusgabe_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grvAusgabe.PageIndexChanging
        FillGrid(e.NewPageIndex, String.Empty)
    End Sub

    Private Sub grvAusgabe_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grvAusgabe.Sorting
        FillGrid(grvAusgabe.PageIndex, e.SortExpression)
    End Sub


    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        grvAusgabe.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0, String.Empty)
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region


#Region "Methods"

    '----------------------------------------------------------------------
    ' Methode:      LoadRFIDBestand
    ' Autor:        SFa
    ' Beschreibung: Ruft die Methode FillHaendlerbestand auf, um die Tabelle
    '               mit den Bestandsdaten zu füllen.
    ' Erstellt am:  14.05.2009
    ' ITA:          2814
    '----------------------------------------------------------------------

    Private Sub DoSubmit()

        Dim Bestand As New SixtRFID(m_User, m_App, "")
        Bestand.FillRFIDbestand(Me.Page, txtDatumVon.Text, txtDatumBis.Text, txtKennzeichen.Text)

        If IsNothing(Session("BestandTable")) = True Then
            Session.Add("BestandTable", Bestand.BestandTable)
        Else
            Session("BestandTable") = Bestand.BestandTable
        End If

        FillGrid(0, "")

    End Sub

    '----------------------------------------------------------------------
    ' Methode:      FillGrid
    ' Autor:        SFa
    ' Beschreibung: Füllt das gridview grvAusgabe aus der Bestandstabelle.
    '               Je nach Übergabeparameter findet eine Sortierung oder
    '               Seitenanzeige statt
    ' Erstellt am:  14.05.2009
    ' ITA:          2814
    '----------------------------------------------------------------------
    Private Sub FillGrid(ByVal PageIndex As Int32, Optional ByVal Sort As String = "")


        Dim BestandTable As New DataTable
        Dim Direction As String = String.Empty

        If IsNothing(Session("BestandTable")) = False Then

            BestandTable = CType(Session("BestandTable"), DataTable)

            If BestandTable.Rows.Count > 0 Then

                'If m_User.Reference Is Nothing OrElse m_User.Reference.Trim Is String.Empty OrElse m_User.Reference.Trim = "" Then
                '    grvAusgabe.Columns(0).Visible = True
                'End If


                grvAusgabe.Visible = True
                ddlPageSize.Visible = True

                tdExcel.Visible = True

                Dim TempPageIndex As Int32 = PageIndex

                If Sort.Trim(" "c).Length > 0 Then
                    PageIndex = 0
                    Sort = Sort.Trim(" "c)
                    If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = Sort) Then
                        If ViewState("Direction") Is Nothing Then
                            Direction = "desc"
                        Else
                            Direction = ViewState("Direction").ToString
                        End If
                    Else
                        Direction = "desc"
                    End If

                    If Direction = "asc" Then
                        Direction = "desc"
                    Else
                        Direction = "asc"
                    End If

                    ViewState("Sort") = Sort
                    ViewState("Direction") = Direction
                Else
                    If Not ViewState("Sort") Is Nothing Then
                        Sort = ViewState("Sort").ToString
                        If ViewState("Direction") Is Nothing Then
                            Direction = "asc"
                            ViewState("Direction") = Direction
                        Else
                            Direction = ViewState("Direction").ToString
                        End If
                    End If
                End If

                If Not Sort.Length = 0 Then
                    BestandTable.DefaultView.Sort = Sort & " " & Direction
                End If

                grvAusgabe.PageIndex = PageIndex

                grvAusgabe.DataSource = BestandTable.DefaultView
                grvAusgabe.DataBind()

                lnkCreateExcel.Visible = True

                lblNoData.Text = "Es wurden " & BestandTable.Rows.Count & " Fahrzeuge gefunden."
                lblNoData.Visible = True

            Else
                lblNoData.Visible = True
                lblNoData.Text = "Es wurden keine Fahrzeuge gefunden."

            End If
        Else
            grvAusgabe.Visible = False
            ddlPageSize.Visible = False

            tdExcel.Visible = False
            lblNoData.Text = "Es wurden keine Fahrzeuge gefunden."
            lblNoData.Visible = True
        End If



    End Sub



#End Region



 
End Class

' ************************************************
' $History: Report29.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 19.05.09   Time: 16:36
' Updated in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 19.05.09   Time: 10:20
' Updated in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 15.05.09   Time: 16:17
' Created in $/CKAG/Applications/appsixt/Forms
' ITA: 2814
' 