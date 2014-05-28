Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business

Partial Public Class Change03_2
    Inherits System.Web.UI.Page

#Region "Declarations"

    Private m_App As App
    Private m_User As User
    Private objHaendler As Haendler
    Private objBank As AppF2BankBaseCredit
    Private objSuche As AppF2.Search
    Private m_Change As OffeneAnforderungen

    Protected WithEvents GridNavigation1 As Global.CKG.Services.GridNavigation
    Protected WithEvents Kopfdaten1 As Global.CKG.Services.PageElements.Kopfdaten

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        m_App = New App(m_User)
        Kopfdaten1.Message = ""
        GridNavigation1.setGridElment(GridView1)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        If IsPostBack = False Then

            'objekt wiederherstellen
            m_Change = CType(Session("m_change"), OffeneAnforderungen)
            objSuche = CType(Session("objSuche"), AppF2.Search)


            If m_Change.Haendler Is Nothing OrElse m_Change.Haendler.Trim Is String.Empty Then
                'wenn kein konkreter Händler angegeben worden ist, alle offenen Anforderungen anzeigen
                'somit entfällt das füllen der Kopfdaten
                Kopfdaten1.Visible = False

            Else
                'wenn ein Konkreter Händler ausgwählt, dann kopfdaten und Kontingente füllen
                Kopfdaten1.Visible = True


                If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, m_Change.Haendler, Me) Then
                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                Else
                    Session("objSuche") = objSuche
                End If



                'Kopfdatenfüllen
                Kopfdaten1.UserReferenz = m_User.Reference
                Kopfdaten1.HaendlerNummer = m_Change.Haendler
                Dim strTemp As String = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    strTemp &= "<br>" & objSuche.NAME_2
                End If
                Kopfdaten1.HaendlerName = strTemp
                Kopfdaten1.Adresse = objSuche.STREET & "<br>" & objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY

                'bankObjekt für Kontingente instanziieren 
                objBank = New AppF2BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                objBank.Customer = objSuche.REFERENZ
                objBank.KUNNR = m_User.KUNNR
                objBank.CreditControlArea = "ZDAD"
                objBank.Show(Session("AppID").ToString, Session.SessionID, Me) 'kontingentetabelle füllen
                Session("objBank") = objBank
                Kopfdaten1.Kontingente = objBank.Kontingente 'kontingente anzeigen

            End If

            'Resultatstabelle füllen/objekt in Session schreiben und Excel erzeugen/Grid füllen

            If m_Change.Status = 0 Then
                Session("m_change") = m_Change
                FillGrid(0, "Anforderungsdatum", "desc")
            Else
                lblError.Text = m_Change.Message
            End If


            If Request.QueryString("HDL") = 1 Then
                Session("AppShowNot") = True
            End If
        Else 'wenn postback
            If m_Change Is Nothing Then
                m_Change = CType(Session("m_change"), OffeneAnforderungen)
            End If

            If objBank Is Nothing Then
                objBank = CType(Session("objBank"), AppF2BankBaseCredit)
            End If

        End If

    End Sub

    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        If e.CommandName = "Storno" Then
            If m_change Is Nothing Then
                m_change = CType(Session("m_change"), OffeneAnforderungen)
            End If


            m_change.EQUNR = m_change.Auftraege.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("EQUNR")
            m_change.VBELN = m_change.Auftraege.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("VBELN")
            m_change.StornoHaendler = m_change.Auftraege.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("Haendlernummer")
            m_change.Fahrgestellnummer = m_change.Auftraege.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("Fahrgestellnummer")
            m_change.Change(Session("AppID").ToString, Session.SessionID, Me)



            Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

            Dim Vertragsnummer As String = m_Change.Auftraege.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("Vertragsnummer")


            logApp.CollectDetails("Hdl.-Nr.", CType(m_Change.StornoHaendler, Object), True)
            logApp.CollectDetails("Fahrgestellnummer", CType(m_Change.Fahrgestellnummer, Object))
            logApp.CollectDetails("Vertragsnummer", CType(Vertragsnummer, Object))
            logApp.CollectDetails("Kommentar", CType("Storno", Object))

            logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)



            If Not m_Change.Status = 0 Then
                logApp.UpdateEntry("ERR", Session("AppID").ToString, "Offene Anforderungen: Storno fehlgeschlagen", logApp.InputDetails)
                lblError.Text = m_Change.Message
                Exit Sub
            Else
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Offene Anforderungen: Storno", logApp.InputDetails)
                fakeTheGrid(e.CommandArgument, "X", m_Change.Fahrgestellnummer)
            End If


            'Kontingente aktuallisieren, kann immer geschehen, wurde ja zu  anfangs visible=false/true gesetzt wenn kein spezieller Händler
            If objBank Is Nothing Then
                If Session.Item("objBank") Is Nothing Then

                    objBank = New AppF2BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    objBank.Customer = m_change.Haendler 'muss ja jetzt einen haben sonst ginge das storno nicht, 
                    objBank.KUNNR = m_User.KUNNR
                    objBank.CreditControlArea = "ZDAD"
                    objBank.Show(Session("AppID").ToString, Session.SessionID, Me) 'kontingentetabelle füllen
                    Session("objBank") = objBank
                Else
                    objBank = CType(Session.Item("objBank"), AppF2BankBaseCredit)
                    objBank.Show(Session("AppID").ToString, Session.SessionID, Me) 'kontingentetabelle füllen
                End If
            End If
            objBank.Show(Session("AppID").ToString, Session.SessionID, Me) 'kontingentetabelle füllen
            Kopfdaten1.Kontingente = objBank.Kontingente 'kontingente anzeigen
        End If
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(GridView1.PageIndex, e.SortExpression)
    End Sub

    Private Sub GridNavigation1_ddlPageSizeChanged() Handles GridNavigation1.PageSizeChanged
        FillGrid(0)
    End Sub
    Private Sub GridView1_PageIndexChanged(ByVal pageindex As Int32) Handles GridNavigation1.PagerChanged
        GridView1.EditIndex = -1
        FillGrid(pageindex)
    End Sub

    Protected Sub lbHaendlersuche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbHaendlersuche.Click
        Response.Redirect("Change03.aspx?AppID=" & Page.Session("AppID"))
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub timerHidePopup_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles timerHidePopup.Tick
        timerHidePopup.Enabled = False
        ModalPopupExtender1.Hide()
        FillGrid(GridView1.PageIndex)
    End Sub

#End Region

#Region "Methods"

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal Direction As String = "")
        If m_Change.Status = 0 Then

            If IsNothing(m_Change.Auftraege) OrElse m_Change.Auftraege.Rows.Count = 0 Then
                'lbExcel.Visible = False

                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                GridView1.Visible = False
                lblNoData.Visible = True
            Else
                'lbExcel.Visible = True

                GridView1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = m_Change.Auftraege.DefaultView

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

                    If Direction.Length > 0 Then
                        strDirection = Direction
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
                    Else
                        strDirection = "asc"
                    End If

                End If


                If Not strTempSort.Length = 0 Then
                    tmpDataView.Sort = strTempSort & " " & strDirection
                End If

                GridView1.PageIndex = intTempPageIndex

                GridView1.DataSource = tmpDataView
                GridView1.DataBind()
                'Anzahl der gesperrten Aufträge ermitteln
                Dim view As New DataView(tmpDataView.Table)
                view.RowFilter = "Gesperrt <> ''"
                '----------------------------------------
                lblNoData.Text = "Es wurde(n) " & tmpDataView.Count.ToString & " offene Anforderung(en) gefunden"
                lblNoData.Visible = True


                Dim cell As TableCell
                Dim control As Control
                Dim blnScriptFound As Boolean = False
                Dim tmpLabel As Label
                Dim strAngefordertAm As String = ""
                Dim strFahrgestellnummer As String = ""
                Dim strBriefnummer As String = ""
                Dim strKontingentart As String = ""
                Dim ConfirmButton As AjaxControlToolkit.ConfirmButtonExtender


                Dim Row As GridViewRow


                For Each Row In GridView1.Rows

                    control = Row.Cells(4).Controls(1)
                    If TypeOf control Is WebControls.Label Then
                        tmpLabel = CType(control, WebControls.Label)
                        strAngefordertAm = Replace(tmpLabel.Text, "&nbsp;", "")
                    End If


                    control = Row.Cells(5).Controls(1)
                    If TypeOf control Is WebControls.Label Then
                        tmpLabel = CType(control, WebControls.Label)
                        strFahrgestellnummer = Replace(tmpLabel.Text, "&nbsp;", "")
                    End If

                    control = Row.Cells(6).Controls(1)
                    If TypeOf control Is WebControls.Label Then
                        tmpLabel = CType(control, WebControls.Label)
                        strBriefnummer = Replace(tmpLabel.Text, "&nbsp;", "")
                    End If

                    control = Row.Cells(7).Controls(1)
                    If TypeOf control Is WebControls.Label Then
                        tmpLabel = CType(control, WebControls.Label)
                        strKontingentart = Replace(tmpLabel.Text, "&nbsp;", "")
                    End If


                    cell = Row.Cells(0)
                    For Each control In cell.Controls
                        If TypeOf control Is AjaxControlToolkit.ConfirmButtonExtender Then

                            ConfirmButton = CType(control, AjaxControlToolkit.ConfirmButtonExtender)

                            ConfirmButton.ConfirmText = "Wollen Sie diesen Auftrag wirklich stornieren?" & vbCrLf & _
                                                        "Angefordert am: " & strAngefordertAm & vbCrLf & _
                                                        "Fahrgestellnr:  " & strFahrgestellnummer & vbCrLf & _
                                                        "ZBII-Nummer:    " & strBriefnummer & vbCrLf & _
                                                        "Kontingentart:  " & strKontingentart


                        End If

                    Next

                Next

                If blnScriptFound Then
                    ShowScript.Visible = True
                End If
            End If
        Else
            lblError.Text = m_Change.Message
            lblNoData.Visible = True
        End If
    End Sub

    Private Sub fakeTheGrid(ByVal EQUNR As String, ByVal storno As String, ByVal fahrgestellnummer As String)

        m_change.Auftraege.Select("EQUNR='" & EQUNR & "'")(0).Delete()
        m_change.Auftraege.AcceptChanges()
        'FillGrid(GridView1.PageIndex)

        If storno = "X" Then
            'lblNoData.Text = "Der Auftrag mit der Fahrgestellnummer:  " & fahrgestellnummer & " wurde erfolgreich storniert"
            ModalPopupExtender1.Show()
            timerHidePopup.Enabled = True
        End If
        lblNoData.Visible = True
    End Sub



#End Region


End Class
' ************************************************
' $History: Change03_2.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Dittbernerc  Date: 5.04.11    Time: 16:30
' Updated in $/CKAG2/Applications/AppF2/forms
' FixGridViewCols:
' 
' Elemente wie Textboxen, Dropdownlisten, und Checkboxen werden in der
' Berechnung mit ber�cksichtigt.
' Die Headrow wird nur noch einmalig abgefragt nicht pro abgefragter Row
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 10.09.09   Time: 10:15
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 3.09.09    Time: 13:38
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 3.09.09    Time: 11:27
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 2.09.09    Time: 15:42
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 13.08.09   Time: 12:58
' Updated in $/CKAG2/Applications/AppF2/forms
' ITA: 3071
' 