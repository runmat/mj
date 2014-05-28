Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports CKG.Base.Business

Partial Public Class Change05_2
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Private m_change As Freigabe
    Private objHaendler As Haendler
    Private objBank As AppF2BankBaseCredit
    Private objSuche As AppF2.Search
    Private CurrentIndex As Integer
    Protected widestData As Integer

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
        widestData = 0


        'Username + Datum + 2 Leerzeichen
        txtStornotext.MaxLength = 108 - (m_User.UserName.Length + 12)

        If IsPostBack = False Then

            'objekt wiederherstellen
            m_change = CType(Session("m_change"), Freigabe)
            If Not Session("objSuche") Is Nothing Then
                objSuche = CType(Session("objSuche"), AppF2.Search)
            Else
                objSuche = New AppF2.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            End If



            If m_change.Haendlernummer Is Nothing OrElse m_change.Haendlernummer.Trim Is String.Empty Then
                'wenn kein konkreter Händler angegeben worden ist, alle offenen Anforderungen anzeigen
                'somit entfällt das füllen der Kopfdaten
                Kopfdaten1.Visible = False

            Else
                'wenn ein Konkreter Händler ausgwählt, dann kopfdaten und Kontingente füllen
                Kopfdaten1.Visible = True

                If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, m_change.Haendlernummer, Me) Then
                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                Else
                    Session("objSuche") = objSuche
                End If



                'Kopfdatenfüllen
                Kopfdaten1.UserReferenz = m_User.Reference
                Kopfdaten1.HaendlerNummer = m_change.Haendlernummer
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
            'm_change.show(Me.Request.QueryString.Item("HEZ"), Session("AppID").ToString, Session.SessionID, Me)
            If m_change.Status = 0 Then
                Session("m_change") = m_change
                FillGrid(0)
            Else
                lblError.Text = m_change.Message
            End If


            If Request.QueryString("HDL") = 1 Then
                Session("AppShowNot") = True
            End If
        Else 'wenn postback
            If m_change Is Nothing Then
                m_change = CType(Session("m_change"), Freigabe)
            End If

            If objBank Is Nothing Then
                objBank = CType(Session("objBank"), AppF2BankBaseCredit)
            End If

        End If
    End Sub



    Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand


        If Not e.CommandName = "Sort" Then
            Dim index As Integer = CType(e.CommandArgument, Integer)

            CurrentIndex = index

            Dim row As GridViewRow = GridView1.Rows(index)


            If e.CommandName = "Freigabe" Then

                'Dim resultRows() As DataRow = m_change.Result.Select("EQUNR = '" + e.CommandArgument.ToString + "'")
                'Dim resultRow As DataRow

                'If resultRows.Length = 1 Then
                '    resultRow = resultRows(0)
                'Else
                '    lblError.Text = "Fehler beim Freigeben der Kontingentüberschreitung! Equipment nicht gefunden!"
                '    Return
                'End If

                m_change.Storno = ""

                Dim lbl As Label

                lbl = CType(row.FindControl("lblHaendlernummer"), Label)

                m_change.Haendlernummer = lbl.Text


                lbl = CType(row.FindControl("lblFahrgestellnummer"), Label)

                m_change.Fahrgestellnr = lbl.Text




                lbl = CType(row.Cells(15).FindControl("lblVbeln"), Label)
                m_change.VBELN = lbl.Text

                lbl = CType(row.Cells(16).FindControl("lblequinr"), Label)
                m_change.Equinr = lbl.Text



                Select Case m_change.Result.Rows(index).Item("Kontingentart").ToString
                    Case "Standard temporär"
                        m_change.Kontingentart = "0001"
                    Case "Standard endgültig"
                        m_change.Kontingentart = "0002"
                    Case "Händler Zulassung"
                        m_change.Kontingentart = "0005"
                End Select


                'logging
                Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                logApp.CollectDetails("Händlernummer", CType(m_change.Haendlernummer, Object), True)
                logApp.CollectDetails("Vertragsnummer", CType(row.Cells(3).Text, Object))
                logApp.CollectDetails("Fahrgestellnummer", CType(m_change.Fahrgestellnr, Object))
                logApp.CollectDetails("Abrufgrund", CType(row.Cells(9).Text, Object))
                logApp.CollectDetails("Storno Grund", CType("Freigabe", Object))

                logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)



                m_change.stornoorderfreigabe(m_change.Storno)


                If Not m_change.Status = 0 Then

                    If m_change.Status = -1111 Then
                        'auftrag wurde währendessen schonmal freigegeben von einem anderen benutzer
                        logApp.UpdateEntry("ERR", Session("AppID").ToString, "Freigabe: Vorgang wurde schon bearbeitet", logApp.InputDetails)
                        lblNoData.Text = "Vorgang wurde schon bearbeitet von " & m_change.FreigabeUser & " am " & m_change.FreigabeDatum & " um " & m_change.FreigabeUhrzeit
                        lblNoData.Visible = True
                        fakeTheGrid(e.CommandArgument, m_change.Storno, m_change.Fahrgestellnr, True)
                    Else
                        logApp.UpdateEntry("ERR", Session("AppID").ToString, "Freigabe: Speichern fehlgeschlagen", logApp.InputDetails)
                        lblError.Text = m_change.Message
                    End If
                Else
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Freigabe", logApp.InputDetails)
                    fakeTheGrid(m_change.Equinr, m_change.Storno, m_change.Fahrgestellnr)
                End If
            End If
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
        Response.Redirect("Change05.aspx?AppID=" & Page.Session("AppID"))
    End Sub

    Private Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing
        Dim row As GridViewRow = GridView1.Rows(e.NewEditIndex)

    End Sub



    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        txtStornotext.Text = String.Empty
        ModalPopupExtender2.Show()
    End Sub



    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Dim row As GridViewRow = GridView1.Rows(CurrentIndex)

        m_change.StornoGrund1 = String.Empty
        m_change.StornoGrund2 = String.Empty
        m_change.StornoGrund3 = String.Empty
        m_change.StornoGrund4 = String.Empty


        m_change.Storno = "X"

        Dim lbl As Label

        lbl = CType(row.FindControl("lblHaendlernummer"), Label)

        m_change.Haendlernummer = lbl.Text


        lbl = CType(row.FindControl("lblFahrgestellnummer"), Label)

        m_change.Fahrgestellnr = lbl.Text



        lbl = CType(row.Cells(15).FindControl("lblVbeln"), Label)
        m_change.VBELN = lbl.Text

        lbl = CType(row.Cells(16).FindControl("lblequinr"), Label)
        m_change.Equinr = lbl.Text



        Select Case m_change.Result.Rows(CurrentIndex).Item("Kontingentart").ToString
            Case "Standard temporär"
                m_change.Kontingentart = "0001"
            Case "Standard endgültig"
                m_change.Kontingentart = "0002"
            Case "Händler Zulassung"
                m_change.Kontingentart = "0005"
        End Select




        Dim x As Int32 = 0
        Dim i As Int32 = 1

        txtStornotext.Text = m_User.UserName & " " & _
                            Today.ToShortDateString & " " & _
                            txtStornotext.Text


        'logging
        Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.CollectDetails("Händlernummer", CType(m_change.Haendlernummer, Object), True)
        logApp.CollectDetails("Vertragsnummer", CType(row.Cells(3).Text, Object))
        logApp.CollectDetails("Fahrgestellnummer", CType(m_change.Fahrgestellnr, Object))
        logApp.CollectDetails("Abrufgrund", CType(row.Cells(9).Text, Object))
        logApp.CollectDetails("Storno Grund", CType(txtStornotext.Text, Object))

        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)


        Dim cTmp As Char
        For Each cTmp In txtStornotext.Text.ToCharArray
            If i Mod 27 = 0 Then
                x = x + 1
            End If
            Select Case x
                Case 0
                    m_change.StornoGrund1 = m_change.StornoGrund1 & cTmp
                Case 1
                    m_change.StornoGrund2 = m_change.StornoGrund2 & cTmp
                Case 2
                    m_change.StornoGrund3 = m_change.StornoGrund3 & cTmp
                Case 3
                    m_change.StornoGrund4 = m_change.StornoGrund4 & cTmp
                Case Else
                    Exit For
            End Select
            i = i + 1
        Next

        m_change.stornoorderfreigabe(m_change.Storno)

        If Not m_change.Status = 0 Then



            If m_change.Status = -1111 Then
                'auftrag wurde währendessen schonmal freigegeben von einem anderen benutzer
                logApp.UpdateEntry("ERR", Session("AppID").ToString, "Freigabe Storno: Vorgang wurde schon bearbeitet", logApp.InputDetails)
                lblNoData.Text = "Vorgang wurde schon bearbeitet von " & m_change.FreigabeUser & " am " & m_change.FreigabeDatum & " um " & m_change.FreigabeUhrzeit
                lblNoData.Visible = True
                fakeTheGrid(CurrentIndex, m_change.Storno, m_change.Fahrgestellnr, True)
            Else
                logApp.UpdateEntry("ERR", Session("AppID").ToString, "Freigabe Storno: Speichern fehlgeschlagen", logApp.InputDetails)
                lblError.Text = m_change.Message
            End If
        Else
            logApp.UpdateEntry("APP", Session("AppID").ToString, "Freigabe Storno", logApp.InputDetails)
            fakeTheGrid(m_change.Equinr, m_change.Storno, m_change.Fahrgestellnr)
        End If

        ModalPopupExtender2.Hide()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


#End Region

#Region "Methods"

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_change.Status = 0 OrElse m_change.Status = -1111 Then
            If m_change.Result.Rows.Count = 0 Then
                GridView1.Visible = False
                lblNoData.Visible = True
                'Label1.Visible = False
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            Else
                GridView1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = m_change.Result.DefaultView

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


                GridView1.PageIndex = intTempPageIndex

                GridView1.DataSource = tmpDataView
                GridView1.DataBind()

                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Fahrzeuge gefunden."
                lblNoData.Visible = True


                'für AutorisierungsAnwendung, Einzelautorisierung
                If m_change.Result.Rows.Count = 1 Then
                    If Not m_change.Result.Rows(0).Item("Status") = "" Then
                        'lb_Autorisierung.Visible = True

                        'Freigabe/Storno Button
                        GridView1.Columns(10).Visible = False
                        GridView1.Columns(11).Visible = False
                        'Auto/Loesch Button
                        GridView1.Columns(12).Visible = True
                        GridView1.Columns(13).Visible = True
                        If m_change.Storno = "X" Then
                            lblNoData.Text = "Es wird um eine Autorisierung für die Stornierung dieser Freigabe gebeten"
                        Else
                            lblNoData.Text = "Es wird um eine Autorisierung für diese Freigabe gebeten"
                        End If
                    End If
                End If

                If GridView1.PageCount > 1 Then
                    GridView1.PagerStyle.CssClass = "PagerStyle"
                    GridView1.DataBind()
                    'GridView1.PagerStyle.Visible = True
                Else
                    'GridView1.PagerStyle.Visible = False
                End If
            End If
        Else
            GridView1.Visible = False
            'Label1.Visible = False
            lblError.Text = m_change.Message
            lblNoData.Visible = True
        End If
    End Sub

    Private Sub fakeTheGrid(ByVal EQUNR As String, ByVal storno As String, ByVal fahrgestellnummer As String, Optional ByVal keineAnzeige As Boolean = False)
        m_change.Result.Select("EQUNR='" & EQUNR & "'")(0).Delete()
        m_change.Result.AcceptChanges()
        FillGrid(GridView1.PageIndex)

        If Not keineAnzeige Then
            'keine Anzeige wenn der datensatz ohne "todo" text aus grid entfernt werden soll
            If storno = "X" Then
                lblNoData.Text = "Der Auftrag mit der Fahrgestellnummer:  " & fahrgestellnummer & " wurde erfolgreich storniert"
            Else
                lblNoData.Text = "Der Auftrag mit der Fahrgestellnummer:  " & fahrgestellnummer & " wurde erfolgreich freigegeben"
            End If
            lblNoData.Visible = True
        End If
    End Sub
#End Region

End Class

' ************************************************
' $History: Change05_2.aspx.vb $
' 
' *****************  Version 11  *****************
' User: Dittbernerc  Date: 5.04.11    Time: 16:30
' Updated in $/CKAG2/Applications/AppF2/forms
' FixGridViewCols:
' 
' Elemente wie Textboxen, Dropdownlisten, und Checkboxen werden in der
' Berechnung mit ber�cksichtigt.
' Die Headrow wird nur noch einmalig abgefragt nicht pro abgefragter Row
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 10.09.09   Time: 10:15
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 3.09.09    Time: 11:27
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 2.09.09    Time: 17:47
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 20.08.09   Time: 13:38
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 19.08.09   Time: 17:18
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 17.08.09   Time: 10:00
' Updated in $/CKAG2/Applications/AppF2/forms
' ITA: 3071
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 17.08.09   Time: 9:08
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 17.08.09   Time: 0:05
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 16.08.09   Time: 9:34
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 13.08.09   Time: 17:36
' Created in $/CKAG2/Applications/AppF2/forms
' 