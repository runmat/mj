Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report10
    Inherits System.Web.UI.Page

#Region "Declarations"

    Protected WithEvents ucStyles As CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As CKG.Portal.PageElements.Header

    Private m_User As Security.User
    Private m_App As Security.App

    Private Suche As Search
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

                If m_User.Reference Is Nothing OrElse m_User.Reference.Trim Is String.Empty OrElse m_User.Reference.Trim = "" Then 'standardfall wenn Bänker App betritt" 

                    SucheHaendler1.Visible = True
                    SucheHaendler1.initialize()

                    cmdSearch.Visible = True

                Else    'fall wenn user mit User-Referenz 

                    SucheHaendler1.Visible = False
                    lbl_AnzeigeHaendlerSuche.Visible = False

                    cmdBack.Visible = True

                    LoadHaendlerBestand(m_User.Reference.ToString)

                End If

                txtErfassungsdatumVon.Text = DateAdd(DateInterval.Day, -30, Date.Today).ToShortDateString
                txtErfassungsdatumBis.Text = Date.Today.ToShortDateString



            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub btnVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVon.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBis.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtErfassungsdatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtErfassungsdatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub


    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Protected Sub lbSelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbSelektionZurueckSetzen.Click
        SucheHaendler1.SelektionZuruecksetzen()

        SucheHaendler1.Visible = True
        trDatumVon.Visible = True
        trDatumBis.Visible = True

        lbl_AnzeigeHaendlerSuche.Visible = True
        lbSelektionZurueckSetzen.Visible = False
        grvAusgabe.Visible = False
        grvAusgabe.Dispose()
        tdExcel.Visible = False
        cmdSearch.Visible = True
        lblNoData.Visible = False
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


                        If FieldControl.Visible = False Then
                            Found = False
                        Else
                            AusgabeTable.Columns(col2.ColumnName).ColumnName = sColName
                        End If

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
    ' Methode:      DoSubmit
    ' Autor:        SFa
    ' Beschreibung: Ermittelt die Händlerdaten und sucht die entsprechenden
    '               Fahrzeuge
    ' Erstellt am:  16.03.2009
    ' ITA:          2673 und 2667
    '----------------------------------------------------------------------
    Private Sub DoSubmit()

        lblError.Text = ""

        Dim checkInput As Boolean = True


        Dim datErfassungsdatumVon As DateTime
        Dim datErfassungsdatumBis As DateTime

        lblError.Text = ""
        'Datumsfelder prüfen
        If (txtErfassungsdatumVon.Text.Length = 0) Or (txtErfassungsdatumBis.Text.Length = 0) Then
            checkInput = False
        End If
        If (Not IsDate(txtErfassungsdatumVon.Text)) Or (Not IsDate(txtErfassungsdatumBis.Text)) Then
            checkInput = False
        End If
        If Not checkInput Then
            'Datumsfelder leer oder falsches Format
            lblError.Text = "Bitte geben Sie ein gültiges Versanddatum (von,bis) ein!"
        End If
        'Datumsfelder sind gefüllt und haben das richtige Format. Jetzt Werte prüfen.
        If checkInput Then
            datErfassungsdatumVon = CDate(txtErfassungsdatumVon.Text)
            datErfassungsdatumBis = CDate(txtErfassungsdatumBis.Text)
            If (datErfassungsdatumVon > datErfassungsdatumBis) Then
                checkInput = False
                lblError.Text = "Versanddatum (von) muß kleiner oder gleich Versanddatum (bis) sein!<br>"
            End If
            If (datErfassungsdatumBis.Subtract(datErfassungsdatumVon).Days > 30) Then
                checkInput = False
                lblError.Text = "Der angegebene Zeitraum umfasst mehr als 30 Tage!<br>"
            End If
        End If

        If checkInput = False Then Exit Sub

        If Not SucheHaendler1.giveHaendlernummer = "" Then

            Suche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            Suche.fillHaendlerData(Session("AppID").ToString, Session.SessionID.ToString, SucheHaendler1.giveHaendlernummer)
            If Not Suche.Status = 0 Then
                lblError.Text = "Fehler bei der Ermittlung der Versendungen.<br>(" & Suche.ErrorMessage & ")"
                Exit Sub
            End If
            Session("Suche") = Suche
            LoadHaendlerBestand(Suche.REFERENZ)
        Else
           
            LoadHaendlerBestand("")

        End If

        cmdSearch.Visible = False
        lbSelektionZurueckSetzen.Visible = True

    End Sub

    '----------------------------------------------------------------------
    ' Methode:      LoadHaendlerBestand
    ' Autor:        SFa
    ' Beschreibung: Ruft die Methode FillHaendlerbestand auf, um die Tabelle
    '               mit den Bestandsdaten zu füllen.
    ' Erstellt am:  16.03.2009
    ' ITA:          2673 und 2667
    '----------------------------------------------------------------------

    Private Sub LoadHaendlerBestand(ByVal UserReference As String)

        Dim Bestand As New Haendlerbestand(m_User, m_App, "")
        Bestand.FillExpressversendungen(UserReference, txtErfassungsdatumVon.Text, txtErfassungsdatumBis.Text)

        If IsNothing(Session("BestandTable")) = True Then
            Session.Add("BestandTable", Bestand.HaendlerbestandTable)
        Else
            Session("BestandTable") = Bestand.HaendlerbestandTable
        End If

        FillGrid(0, "")

        If UserReference.Length > 0 Then
            cmdSearch.Visible = False
        End If

    End Sub

    '----------------------------------------------------------------------
    ' Methode:      FillGrid
    ' Autor:        SFa
    ' Beschreibung: Füllt das gridview grvAusgabe aus der Bestandstabelle.
    '               Je nach Übergabeparameter findet eine Sortierung oder
    '               Seitenanzeige statt
    ' Erstellt am:  16.03.2009
    ' ITA:          2673 und 2667
    '----------------------------------------------------------------------
    Private Sub FillGrid(ByVal PageIndex As Int32, Optional ByVal Sort As String = "")


        Dim BestandTable As New DataTable
        Dim Direction As String = String.Empty

        If IsNothing(Session("BestandTable")) = False Then

            BestandTable = CType(Session("BestandTable"), DataTable)

            If BestandTable.Rows.Count > 0 Then


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

                


                SucheHaendler1.Visible = False
                trDatumVon.Visible = False
                trDatumBis.Visible = False

                lbl_AnzeigeHaendlerSuche.Visible = False
                lnkCreateExcel.Visible = True

                lblNoData.Text = "Es wurden " & BestandTable.Rows.Count & " Versendungen gefunden."
                lblNoData.Visible = True

            Else
                lblError.Visible = True
                lblError.Text = "Es wurden keine Versendungen gefunden."
                cmdSearch.Visible = True
                lbSelektionZurueckSetzen.Visible = False

            End If
        Else
            lblNoData.Visible = True
            lblError.Visible = True
            lblError.Text = "Es wurden keine Versendungen gefunden."
        End If

    End Sub

#End Region

End Class