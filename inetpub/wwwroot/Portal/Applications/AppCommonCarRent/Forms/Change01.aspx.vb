Option Strict Off

Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change01
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
 
    Private mObjZulassung As Zulassung


#Region "Properties"

    Private Property Refferer() As String
        Get
            If Not Session.Item(Me.Request.Url.LocalPath & "Refferer") Is Nothing Then
                Return Session.Item(Me.Request.Url.LocalPath & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Session.Item(Me.Request.Url.LocalPath & "Refferer") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        lblError.Text = ""

        If Not IsPostBack AndAlso Not ScriptManager1.IsInAsyncPostBack Then
            If Refferer Is Nothing Then
                If Not Me.Request.UrlReferrer Is Nothing Then
                    Refferer = Me.Request.UrlReferrer.ToString
                Else
                    Refferer = ""
                End If
            End If

            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            'barrierefreies zurücknavigieren in der Anwendung
            If Session("mObjZulassungSession") Is Nothing Then
                mObjZulassung = New Zulassung(m_User, m_App, "")
                mObjZulassung.Fill(Session("AppID").ToString, Me.Session.SessionID, Me)
                If Not mObjZulassung.Status = 0 Then
                    lblError.Text = mObjZulassung.Message
                    Exit Sub
                End If
                Session("mObjZulassungSession") = mObjZulassung
            Else 'wenn zurücknavigation in der anwendung
                If mObjZulassung Is Nothing Then
                    If Not Session("mObjZulassungSession") Is Nothing Then
                        mObjZulassung = CType(Session("mObjZulassungSession"), Zulassung)
                    End If
                End If
                'dabei keine UserSelektionHistorie schreiben
                mObjZulassung.writeUserSelektion = False
                mObjZulassung.killFilter()
            End If

            FillSelektions()

        End If


        If mObjZulassung Is Nothing Then
            If Not Session("mObjZulassungSession") Is Nothing Then
                mObjZulassung = CType(Session("mObjZulassungSession"), Zulassung)
            End If
        End If


    End Sub

    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(mObjZulassung.Fahrzeuge)
        tmpDataView.RowFilter = mObjZulassung.Filter

        If tmpDataView.Count = 0 Then
            gvFahrzeuge.Visible = False
            lblGvFahrzeugeNoData.Visible = True
        Else
            gvFahrzeuge.Visible = True
            lblGvFahrzeugeNoData.Visible = False

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




            gvFahrzeuge.PageIndex = intTempPageIndex

            gvFahrzeuge.DataSource = tmpDataView

            gvFahrzeuge.DataBind()


            For Each item As GridViewRow In gvFahrzeuge.Rows

                If mObjZulassung.Fahrzeuge.Select("EQUNR='" & CType(item.FindControl("lblEQUNR"), Label).Text & "'")(0)("Uebernommen").ToString = "X" Then
                    CType(item.FindControl("chkAuswahl"), CheckBox).Enabled = False
                    Dim tmpImgb As ImageButton = CType(item.FindControl("imgbInsertOne"), ImageButton)
                    tmpImgb.Enabled = False
                    tmpImgb.ImageUrl = "../../../Images/selectgrey.gif"
                    item.BackColor = Drawing.Color.DarkGray
                End If


                If Not item.FindControl("lnkFahrgestellnummer") Is Nothing Then
                    If Not m_User.Applications.Select("AppName = 'Report46'").Length = 0 Then
                        CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).Text
                    End If
                End If
            Next
        End If

    End Sub

    Public Sub gvFahrzeuge_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFahrzeuge.RowCommand

        If Not e.CommandName = "Sort" Then
            If e.CommandName = "alleLoeschen" Then
                For Each tmprow As GridViewRow In gvFahrzeuge.Rows
                    mObjZulassung.Fahrzeuge.Select("EQUNR='" & CType(tmprow.FindControl("lblEQUNR"), Label).Text & "'")(0)("Ausgewaehlt") = ""
                Next
            ElseIf e.CommandName = "alleAuwaehlen" Then
                For Each tmprow As GridViewRow In gvFahrzeuge.Rows
                    If CType(tmprow.FindControl("chkAuswahl"), CheckBox).Enabled Then
                        mObjZulassung.Fahrzeuge.Select("EQUNR='" & CType(tmprow.FindControl("lblEQUNR"), Label).Text & "'")(0)("Ausgewaehlt") = "X"
                    End If
                Next
            ElseIf e.CommandName = "insertAll" Then
                For Each tmpRow As DataRow In mObjZulassung.Fahrzeuge.Select("Ausgewaehlt='X' AND Uebernommen<>'X'")
                    tmpRow("Uebernommen") = "X"
                    tmpRow("Ausgewaehlt") = ""
                Next
            ElseIf e.CommandName = "insertOne" Then
                mObjZulassung.Fahrzeuge.Select("EQUNR='" & e.CommandArgument & "'")(0)("Uebernommen") = "X"
                mObjZulassung.Fahrzeuge.Select("EQUNR='" & e.CommandArgument & "'")(0)("Ausgewaehlt") = ""
            End If


            FillGrid(gvFahrzeuge.PageIndex)
        End If

    End Sub

    Private Sub gvFahrzeuge_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvFahrzeuge.Sorting
        FillGrid(gvFahrzeuge.PageIndex, e.SortExpression)
    End Sub

    Private Sub SetFilter()

        '----------------------------------------------------------------------
        'Methode:       SetFilter
        'Autor:         Julian Jung
        'Beschreibung:  Ruft pro ddl die Filtermethode auf und übergibt den selektierten wert
        'Erstellt am:   26.01.2009
        '----------------------------------------------------------------------

        mObjZulassung.killFilter()

        If Not lbxStandort.SelectedIndex = -1 Then
            mObjZulassung.GenerateFilter("Standort", lbxStandort.SelectedValue)
           
        End If

        If Not lbxAbsender.SelectedIndex = -1 Then
            mObjZulassung.GenerateFilter("Absender", lbxAbsender.SelectedValue)
           
        End If


        If Not rblBereifung.SelectedIndex = -1 Then
            mObjZulassung.GenerateFilter("Bereifung", rblBereifung.SelectedValue)
        End If



        If Not ddlLiefermonat.SelectedIndex = -1 AndAlso Not ddlLiefermonat.SelectedIndex = 0 Then
            mObjZulassung.GenerateFilter("Liefermonat", ddlLiefermonat.SelectedValue)
        End If


        If Not lbxHersteller.SelectedIndex = -1 Then
            mObjZulassung.GenerateFilter("Hersteller", lbxHersteller.SelectedValue)
        End If

        If Not lbxTyp.SelectedIndex = -1 Then
            mObjZulassung.GenerateFilter("Typ", lbxTyp.SelectedValue)
        End If


        If Not lbxAufbauart.SelectedIndex = -1 Then
            mObjZulassung.GenerateFilter("Aufbauart", lbxAufbauart.SelectedValue)
        End If

        If Not ddlFarbe.SelectedIndex = -1 AndAlso Not ddlFarbe.SelectedIndex = 0 Then
            mObjZulassung.GenerateFilter("Farbe", ddlFarbe.SelectedValue)
            
        End If




        If Not rblGetriebe.SelectedIndex = -1 Then
            mObjZulassung.GenerateFilter("Getriebe", rblGetriebe.SelectedValue)
          
        End If

        If Not rblKraftstoffart.SelectedIndex = -1 Then
            mObjZulassung.GenerateFilter("Kraftstoffart", rblKraftstoffart.SelectedValue)
        End If




        If Not rblNavi.SelectedIndex = -1 Then
            mObjZulassung.GenerateFilter("Navi", rblNavi.SelectedValue)
        End If

    End Sub




    Private Sub FillSelektions()

        '----------------------------------------------------------------------
        'Methode:       FillSelektions
        'Autor:         Julian Jung
        'Beschreibung:  ruft die fill methode der controls auf und übergibt sich selbst+den spaltenname aus der sie gefüllt werden soll
        'Erstellt am:   26.01.2009
        '----------------------------------------------------------------------


        mObjZulassung.FillControls(lbxStandort, "Standort")
        mObjZulassung.FillControls(lbxHersteller, "Hersteller")
        mObjZulassung.FillControls(lbxAbsender, "Absender")
        mObjZulassung.FillControls(ddlLiefermonat, "Liefermonat")
        mObjZulassung.FillControls(lbxTyp, "Typ")
        mObjZulassung.FillControls(lbxAufbauart, "Aufbauart")
        mObjZulassung.FillControls(ddlFarbe, "Farbe")
        mObjZulassung.FillControls(rblBereifung, "Bereifung")
        mObjZulassung.FillControls(rblGetriebe, "Getriebe")
        mObjZulassung.FillControls(rblKraftstoffart, "Kraftstoffart")
        mObjZulassung.FillControls(rblNavi, "Navi")

        'selektion navigation handeln
        If mObjZulassung.SelektionStep > 0 Then
            lb_SelektionsschrittBack.Enabled = True
            lb_SelektionZurueckSetzen.Enabled = True
        Else
            lb_SelektionsschrittBack.Enabled = False
            lb_SelektionZurueckSetzen.Enabled = False
        End If


    End Sub


    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub fillSelektionInfo()

        lblDataSelektionAbsender.Text = lbxAbsender.SelectedValue
        lblDataSelektionBereifung.Text = rblBereifung.SelectedValue
        If Not ddlFarbe.SelectedValue = "-1" Then
            lblDataSelektionFarbe.Text = ddlFarbe.SelectedValue
        Else
            lblDataSelektionFarbe.Text = ""
        End If
        lblDataSelektionHersteller.Text = lbxHersteller.SelectedValue
        lblDataSelektionGetriebe.Text = rblGetriebe.SelectedValue
        lblDataSelektionKraftstoffart.Text = rblKraftstoffart.SelectedValue
        lblDataSelektionLiefermonat.Text = ddlLiefermonat.SelectedValue
        lblDataSelektionNavi.Text = rblNavi.SelectedValue
        lblDataSelektionStandort.Text = lbxStandort.SelectedValue
        lblDataSelektionTyp.Text = lbxTyp.SelectedValue
        lblDataSelektionAufbauart.Text = lbxAufbauart.SelectedValue
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_weiter.Click
        If Not mObjZulassung.Fahrzeuge.Select("Uebernommen='X'").Count = 0 Then
            Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
        Else
            lblError.Text = "Sie haben kein Fahrzeug selektiert"
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)


        If mObjZulassung.Fahrzeuge Is Nothing OrElse mObjZulassung.Fahrzeuge.Rows.Count = 0 Then
            lblError.Text = "keine zulassungsfähigen Fahrzeuge vorhanden"

        Else
            showAnzahlAuswahl()

            showAnzahlFahrzeuge()

            showAnzahlUebernommen()

        End If
      




    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub



    Private Sub ListItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLiefermonat.SelectedIndexChanged, lbxHersteller.SelectedIndexChanged, rblBereifung.SelectedIndexChanged, lbxStandort.SelectedIndexChanged, rblGetriebe.SelectedIndexChanged, rblKraftstoffart.SelectedIndexChanged, rblNavi.SelectedIndexChanged, lbxAbsender.SelectedIndexChanged, lbxTyp.SelectedIndexChanged, ddlFarbe.SelectedIndexChanged, lbxAufbauart.SelectedIndexChanged
        mObjZulassung.nextUserStep()
        SetFilter()
        FillSelektions()
    End Sub


    Public Sub chk_Auswahl_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If CType(sender, CheckBox).Checked Then
            mObjZulassung.Fahrzeuge.Select("EQUNR='" & CType(CType(CType(sender, CheckBox).Parent.Parent, GridViewRow).FindControl("lblEQUNR"), Label).Text & "'")(0)("Ausgewaehlt") = "X"
        Else
            mObjZulassung.Fahrzeuge.Select("EQUNR='" & CType(CType(CType(sender, CheckBox).Parent.Parent, GridViewRow).FindControl("lblEQUNR"), Label).Text & "'")(0)("Ausgewaehlt") = ""
        End If
    End Sub

    Protected Sub lbSelektionsschrittBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_SelektionsschrittBack.Click
        mObjZulassung.SelectionStepZurueck()
        mObjZulassung.killFilter()
        FillSelektions() 'erst die controls mit allen werten füllen, die ursprünglichen selektieren
        SetFilter() 'anhand der selektierten werten der controls den filter setzen
        FillSelektions() 'füllen wie immer mit ausgeschlossenen Items 


    End Sub

    Private Sub showAnzahlFahrzeuge()
        Dim tmpDV As New DataView(mObjZulassung.Fahrzeuge)
        tmpDV.RowFilter = mObjZulassung.Filter

        lblAnzeigeAnzahlErgebnisse.Text = "Anzahl gefundener Fahrzeuge: " & tmpDV.Count
    End Sub

    Private Sub showAnzahlAuswahl()
        lblAnzeigeAnzahlAuswahl.Text = "Anzahl ausgewählter Fahrzeuge: " & mObjZulassung.Fahrzeuge.Select("Ausgewaehlt='X'").Count
    End Sub

    Private Sub showAnzahlUebernommen()
        chkUebernahmeAusschluss.Enabled = Not mObjZulassung.Fahrzeuge.Select("Uebernommen='X'").Count = 0
        ' IIf(mObjZulassung.Fahrzeuge.Select("Uebernommen='X'").Count = 0, chkUebernahmeAusschluss.Enabled = False, ch)
        lblAnzeigeAnzahlUebernommen.Text = "Anzahl selektierter Fahrzeuge: " & mObjZulassung.Fahrzeuge.Select("Uebernommen='X'").Count
    End Sub


    Protected Sub lb_SelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_SelektionZurueckSetzen.Click
        mObjZulassung.SelectionStepZurueck() 'für das verhindern des schreibens in die userhistory
        mObjZulassung.SelektionStep = 0
        mObjZulassung.killFilter()
        FillSelektions()
    End Sub

    Private Sub fillGridUebernommen(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView(mObjZulassung.Fahrzeuge)
        tmpDataView.RowFilter = "Uebernommen='X'"

        If tmpDataView.Count = 0 Then
            gvUebernommen.Visible = False
            lblGvUebernommenNoData.Visible = True
        Else
            gvUebernommen.Visible = True
            lblGvUebernommenNoData.Visible = False

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




            gvUebernommen.PageIndex = intTempPageIndex

            gvUebernommen.DataSource = tmpDataView

            gvUebernommen.DataBind()


            For Each item As GridViewRow In gvUebernommen.Rows

                If Not item.FindControl("lnkFahrgestellnummer") Is Nothing Then
                    If Not m_User.Applications.Select("AppName = 'Report46'").Length = 0 Then
                        CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).Text
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub gvUebernommen_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvUebernommen.RowCommand
        If Not e.CommandName = "Sort" Then
            If e.CommandName = "delete" Then
                mObjZulassung.Fahrzeuge.Select("EQUNR='" & e.CommandArgument & "'")(0)("Uebernommen") = ""
            End If
            fillGridUebernommen(gvUebernommen.PageIndex)
        End If
    End Sub

    Private Sub gvUebernommen_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvUebernommen.RowDeleting
        'dummy
    End Sub

    Private Sub gvUebernommen_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvUebernommen.Sorting
        fillGridUebernommen(gvUebernommen.PageIndex, e.SortExpression)
    End Sub

    Private Sub chkUebernahmeAusschluss_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkUebernahmeAusschluss.CheckedChanged
        mObjZulassung.UebernahmeAusschluss = chkUebernahmeAusschluss.Checked
        lb_SelektionZurueckSetzen.Enabled = True
    End Sub

    Protected Sub lb_AnzeigeAuswahl_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_AnzeigeAuswahl.Click
        tableSelektion.Visible = False
        tableUebernommen.Visible = False
        tableSelektionInfo.Visible = True
        tableAuswahl.Visible = True
        FillGrid(0) 'auswahlGrid
        fillSelektionInfo()

    End Sub

    Protected Sub lb_AnzeigeSelektion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_AnzeigeSelektion.Click
        tableSelektion.Visible = True
        tableSelektionInfo.Visible = False
        tableAuswahl.Visible = False
        tableUebernommen.Visible = False

        For Each tmpRow As DataRow In mObjZulassung.Fahrzeuge.Rows
            tmpRow("Ausgewaehlt") = ""
            mObjZulassung.Fahrzeuge.AcceptChanges()
        Next

    End Sub

    Protected Sub lb_Uebernommen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Uebernommen.Click
        tableSelektion.Visible = False
        tableSelektionInfo.Visible = False
        tableAuswahl.Visible = False
        tableUebernommen.Visible = True
        fillGridUebernommen(0)
    End Sub
End Class
' ************************************************
' $History: Change01.aspx.vb $
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:53
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' Warnungen
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 27.03.09   Time: 15:07
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2760 unfertig
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 13.03.09   Time: 10:52
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITAn 2537 ergnzung
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 2.03.09    Time: 9:42
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2655
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 12.02.09   Time: 10:51
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 11.02.09   Time: 17:31
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ita 2537
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 30.01.09   Time: 15:20
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 29.01.09   Time: 17:29
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 28.01.09   Time: 17:00
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 28.01.09   Time: 11:11
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 27.01.09   Time: 17:31
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 27.01.09   Time: 10:20
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537
' 
'
' ************************************************
