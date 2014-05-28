Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel

Partial Public Class Report02
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Private objBestand As Bestand
#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)
        GridNavigation1.setGridElment(gvBestand)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New App(m_User)


            lblError.Text = ""
            If IsPostBack Then
                If Not Session("Bestand") Is Nothing Then
                    objBestand = CType(Session("Bestand"), Bestand)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        DoSubmit()
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
    Private Sub GridNavigation1_PagerChanged(ByVal PageIndex As Integer) Handles GridNavigation1.PagerChanged
        gvBestand.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Private Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
    Private Sub gvBestand_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvBestand.Sorting
        FillGrid(gvBestand.PageIndex, e.SortExpression)
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As DataTable = CType(Session("App_ResultTable"), DataTable)

        Try
            If reportExcel.Columns.Contains("EQUNR") Then reportExcel.Columns.Remove("EQUNR")
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, reportExcel, Me)

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub NewSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles NewSearch.Click
        lbCreate.Visible = Not lbCreate.Visible
        tab1.Visible = Not tab1.Visible
        Queryfooter.Visible = Not Queryfooter.Visible
    End Sub


    Protected Sub btnEmpty_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEmpty.Click
        DoSubmit()
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
#End Region

#Region "Methods"
    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objBestand.Result.DefaultView

        tmpDataView.RowFilter = ""

        If tmpDataView.Count = 0 Then
            Result.Visible = False
        Else
            Result.Visible = True
            lbCreate.Visible = False
            tab1.Visible = False
            Queryfooter.Visible = False
            Dim strTempSort As String = ""
            Dim strDirection As String = ""
            Dim intTempPageIndex As Int32 = intPageIndex

            If strSort.Trim(" "c).Length > 0 Then
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
            gvBestand.PageIndex = intTempPageIndex
            gvBestand.DataSource = tmpDataView
            gvBestand.DataBind()
            Session("Bestand") = objBestand
            objBestand = Nothing



        End If

    End Sub


    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try

            'Prüfen, ob eines der Felder einen Wert enthält
            If CheckIsEmpty() = False Then

                lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
                Exit Sub

            End If

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            objBestand = New Bestand(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)

            If txtFahrgestellnummer.Text.Length > 0 OrElse _
            txtKennzeichen.Text.Length > 0 OrElse _
            txtAutovermieter.Text.Length > 0 Then

                objBestand.Fahrgestellnummer = txtFahrgestellnummer.Text
                objBestand.Kennzeichen = txtKennzeichen.Text
                objBestand.Autovermieter = txtAutovermieter.Text
            Else
                If checkDate() Then

                    'Prüfen, ob die Zeiträume von/bis korrekt gefüllt wurden
                    If txtDatumVon.Text.Length > 0 AndAlso txtDatumBis.Text.Length = 0 Then

                        lblError.Text = "Bitte geben Sie nur Zeiträume ""von/bis"" an!"
                        Exit Sub

                    End If

                    If txtDatumVon.Text.Length = 0 AndAlso txtDatumBis.Text.Length > 0 Then

                        lblError.Text = "Bitte geben Sie nur Zeiträume ""von/bis"" an!"
                        Exit Sub

                    End If

                    If txtDatumVonAus.Text.Length > 0 AndAlso txtDatumBisAus.Text.Length = 0 Then

                        lblError.Text = "Bitte geben Sie nur Zeiträume ""von/bis"" an!"
                        Exit Sub

                    End If

                    If txtDatumVonAus.Text.Length = 0 AndAlso txtDatumBisAus.Text.Length > 0 Then

                        lblError.Text = "Bitte geben Sie nur Zeiträume ""von/bis"" an!"
                        Exit Sub

                    End If

                    If txtDatumVon.Text.Length > 0 Then objBestand.DatumVon = txtDatumVon.Text
                    If txtDatumBis.Text.Length > 0 Then objBestand.DatumBis = txtDatumBis.Text
                    If txtDatumVonAus.Text.Length > 0 Then objBestand.DatumVonAus = txtDatumVonAus.Text
                    If txtDatumBisAus.Text.Length > 0 Then objBestand.DatumBisAus = txtDatumBisAus.Text

                Else
                    Exit Sub

                End If

            End If

            'Fremdbestand
            objBestand.Bestandsart = "F"

            objBestand.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

            Session("App_ResultTable") = objBestand.Result

            If Not objBestand.Status = 0 Then
                lblError.Text = objBestand.Message
            ElseIf objBestand.Result.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                FillGrid(0)
            End If


        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.(" & ex.Message & ")"
        End Try
    End Sub

    Private Function CheckIsEmpty() As Boolean

        Dim col As New Collection

        With col
            .Add(txtDatumBis)
            .Add(txtDatumBisAus)
            .Add(txtDatumVon)
            .Add(txtDatumVonAus)
            .Add(txtFahrgestellnummer)
            .Add(txtKennzeichen)
            .Add(txtAutovermieter)

        End With

        Dim booFilled As Boolean = False

        For Each txt As TextBox In col

            If Len(Trim(txt.Text)) > 0 Then
                booFilled = True
                Exit For
            End If

        Next

        Return booFilled

    End Function

    Private Function checkDate() As Boolean
        Dim tmpbool As Boolean = True
        For Each ctrl As BaseValidator In Me.Validators
            If ctrl.IsValid = False Then
                tmpbool = False
            End If
        Next
        Return tmpbool
    End Function
#End Region

End Class
' ************************************************
' $History: Report02.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 9.04.10    Time: 10:27
' Updated in $/CKAG2/Applications/AppChrysler/AppChrysler/forms
' ITA: 3624
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 29.09.09   Time: 12:46
' Updated in $/CKAG2/Applications/AppChrysler/AppChrysler/forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 16.09.09   Time: 11:03
' Created in $/CKAG2/Applications/AppChrysler/AppChrysler/forms
' 