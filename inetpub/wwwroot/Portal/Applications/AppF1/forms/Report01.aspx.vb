Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Partial Public Class Report01
    Inherits System.Web.UI.Page

#Region "Declarations"
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
                    Kopfdaten1.Visible = False

                    cmdSearch.Visible = True

                Else    'fall wenn user mit User-Referenz 

                    FillKontingent()


                    SucheHaendler1.Visible = False
                    lbl_AnzeigeHaendlerSuche.Visible = False
                    cmdBack.Visible = True

                    LoadHaendlerBestand(m_User.Reference.ToString)

                End If

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub


    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Protected Sub lbSelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbSelektionZurueckSetzen.Click
        SucheHaendler1.SelektionZuruecksetzen()

        SucheHaendler1.Visible = True
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

    ''' <summary>
    ''' Ermittelt die Händlerdaten und sucht die entsprechenden Fahrzeuge
    ''' </summary>
    ''' <remarks>
    ''' Autor:        SFa
    ''' Erstellt am:  16.03.2009
    ''' ITA:          2673 und 2667
    ''' </remarks>
    Private Sub DoSubmit()

        lblError.Text = ""
        lblError.Visible = False


        If Not SucheHaendler1.giveHaendlernummer = "" Then

            Suche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            Suche.fillHaendlerData(Session("AppID").ToString, Session.SessionID.ToString, SucheHaendler1.giveHaendlernummer)
            If Not Suche.Status = 0 Then
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & Suche.ErrorMessage & ")"
                Exit Sub
            End If
            Session("Suche") = Suche
            LoadHaendlerBestand(Suche.REFERENZ)
        Else
            'Keinen Händler ausgewählt
            If SucheHaendler1.txtNummer.Text.Length > 0 Then
                LoadHaendlerBestand(SucheHaendler1.txtNummer.Text.ToUpper & "*")
            End If

        End If

        cmdSearch.Visible = False
        lbSelektionZurueckSetzen.Visible = True

    End Sub

    ''' <summary>
    ''' Füllt das Kopfdaten-ascx mit den Kontingenten zur User-Referenz
    ''' </summary>
    ''' <remarks>
    ''' Autor:        SFa
    ''' Erstellt am:  16.03.2009
    ''' ITA:          2673 und 2667
    ''' </remarks>
    Private Sub FillKontingent()

        Try

            Suche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            Suche.fillHaendlerData(Session("AppID").ToString, Session.SessionID.ToString, m_User.Reference)
            If Not Suche.Status = 0 Then
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & Suche.Message & ")"
                Exit Sub
            End If


            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = Suche.REFERENZ
            Dim strTemp As String = Suche.NAME
            If Suche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & Suche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = Suche.COUNTRYISO & " - " & Suche.POSTL_CODE & " " & Suche.CITY & "<br>" & Suche.STREET
            Kopfdaten1.Kontingente = Suche.Kontingente


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try


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
        Bestand.FillHaendlerbestand(UserReference)

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

                If m_User.Reference Is Nothing OrElse m_User.Reference.Trim Is String.Empty OrElse m_User.Reference.Trim = "" Then
                    grvAusgabe.Columns(0).Visible = True
                End If


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

                Dim GridRow As GridViewRow

                For Each GridRow In grvAusgabe.Rows

                    If Not GridRow.FindControl("lnkHistorie") Is Nothing Then

                        If Not m_User.Applications.Select("AppName = 'Report46'").Count = 0 Then

                            CType(GridRow.FindControl("lnkHistorie"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(GridRow.FindControl("lnkHistorie"), HyperLink).Text

                        End If

                    End If

                Next


                SucheHaendler1.Visible = False
                lbl_AnzeigeHaendlerSuche.Visible = False
                lnkCreateExcel.Visible = True

                lblNoData.Text = "Es wurden " & BestandTable.Rows.Count & " Fahrzeuge gefunden."
                lblNoData.Visible = True
                lblHinweis.Text = "Stand: " & Date.Today.ToShortDateString & " - 8:00 Uhr"
            Else
                lblNoData.Visible = True
                lblNoData.Text = "Es wurden keine Fahrzeuge gefunden."
                cmdSearch.Visible = True
                lbSelektionZurueckSetzen.Visible = False

            End If
        Else
            lblNoData.Visible = True
        End If

    End Sub

#End Region

End Class

' ************************************************
' $History: Report01.aspx.vb $
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 16.03.10   Time: 14:32
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 10.09.09   Time: 12:56
' Updated in $/CKAG/Applications/AppF1/forms
' ITA: 3098
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 22.06.09   Time: 15:37
' Updated in $/CKAG/Applications/AppF1/forms
' ITA: 2938
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 30.04.09   Time: 11:39
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2837
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 3.04.09    Time: 11:50
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 25.03.09   Time: 16:24
' Updated in $/CKAG/Applications/AppF1/forms
' Link auf Fahrzeughistorie ITA 2673, 2667
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 24.03.09   Time: 9:21
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 16.03.09   Time: 15:16
' Updated in $/CKAG/Applications/AppF1/forms
' ITAs: 2667 und 2673
' 