Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common


Partial Public Class Change04_2
    Inherits System.Web.UI.Page
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User

    Private clsBlocken As AvisChange04

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)


            If IsPostBack = False Then
                Fill()
            End If

            clsBlocken = DirectCast(Session("clsBlocken"), AvisChange04)


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Fill()
        If clsBlocken Is Nothing = True Then
            clsBlocken = New AvisChange04(m_User, m_App, "")
        End If

        clsBlocken.FillDropdown()

        Dim TemTable As DataTable = clsBlocken.DropDownTable


        Dim Text As String = "POS_KURZTEXT"
        Dim Value As String = "POS_TEXT"

        Dim dv As DataView = TemTable.DefaultView

        dv.RowFilter = "KENNUNG = 'Carport'"
        'Carports
        drpCarport.DataSource = dv 'TemTable.Select("KENNUNG = 'Carport'")

        drpCarport.DataTextField = Text
        drpCarport.DataValueField = Value

        drpCarport.DataBind()

        'Farben
        dv.RowFilter = "KENNUNG = 'FARBE_DE'"
        drpFarbe.DataSource = dv

        drpFarbe.DataTextField = Text
        drpFarbe.DataValueField = Value

        drpFarbe.DataBind()

        'Liefermonate
        dv.RowFilter = "KENNUNG = 'GEPL_LIEFERTERMIN'"
        drpLiefermonat.DataSource = dv

        drpLiefermonat.DataTextField = Text
        drpLiefermonat.DataValueField = Value

        drpLiefermonat.DataBind()

        'Hersteller
        dv.RowFilter = "KENNUNG = 'HERST_NUMMER'"
        drpHersteller.DataSource = dv

        drpHersteller.DataTextField = Text
        drpHersteller.DataValueField = Value

        drpHersteller.DataBind()

        'Modellgruppe
        'drpModellgruppe.DataSource = TemTable.Select("KENNUNG = 'MODELLGRUPPE'")

        'drpModellgruppe.DataTextField = Text
        'drpModellgruppe.DataValueField = Value

        'Kraftstoff
        dv.RowFilter = "KENNUNG = 'KRAFTSTOFF'"
        drpKraftstoff.DataSource = dv

        drpKraftstoff.DataTextField = Text
        drpKraftstoff.DataValueField = Value

        drpKraftstoff.DataBind()

        'NAVI
        dv.RowFilter = "KENNUNG = 'NAVIGATION'"
        drpNavi.DataSource = dv

        drpNavi.DataTextField = Text
        drpNavi.DataValueField = Value

        drpNavi.DataBind()

        'REIFENART
        dv.RowFilter = "KENNUNG = 'REIFENART'"
        drpReifenart.DataSource = dv

        drpReifenart.DataTextField = Text
        drpReifenart.DataValueField = Value

        drpReifenart.DataBind()

        'Aufbauart
        dv.RowFilter = "KENNUNG = 'AUFBAUART'"
        drpAufbauart.DataSource = dv

        drpAufbauart.DataTextField = Text
        drpAufbauart.DataValueField = Value

        drpAufbauart.DataBind()

        Session("clsBlocken") = clsBlocken

    End Sub

    Protected Sub ibtDatumAb_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtDatumAb.Click
        calSperreAb.Visible = True
    End Sub

    Protected Sub ibtDatumBis_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtDatumBis.Click
        calSperreBis.Visible = True
    End Sub

    Private Sub Save()
        Dim CheckOK As Boolean = False
        Dim Text As String = "Auswahl"

        lblError.Text = String.Empty

        With clsBlocken
            'Carports
            If drpCarport.SelectedItem.Text <> Text Then
                .Carport = drpCarport.SelectedItem.Text
                CheckOK = True
            End If

            'Farben
            If drpFarbe.SelectedItem.Text <> Text Then
                .Farbe = drpFarbe.SelectedItem.Text
            End If

            'Liefermonate
            If drpLiefermonat.SelectedItem.Text <> Text Then
                .Liefermonat = drpLiefermonat.SelectedItem.Text
                CheckOK = True
            End If

            'Hersteller
            If drpHersteller.SelectedItem.Text <> Text Then
                .Hersteller = drpHersteller.SelectedItem.Text
                CheckOK = True
            End If

            'Modellgruppe
            If drpModellgruppe.SelectedItem.Text <> Text Then
                .Modellgruppe = drpModellgruppe.SelectedItem.Text
                CheckOK = True
            End If

            'Kraftstoff
            If drpKraftstoff.SelectedItem.Text <> Text Then
                .Kraftstoff = drpKraftstoff.SelectedItem.Text
            End If

            'NAVI
            If drpNavi.SelectedItem.Text <> Text Then
                .NAVI = drpNavi.SelectedItem.Text
            End If

            'REIFENART
            If drpReifenart.SelectedItem.Text <> Text Then
                .Reifenart = drpReifenart.SelectedItem.Text
                CheckOK = True
            End If

            'Aufbauart
            If drpAufbauart.SelectedItem.Text <> Text Then
                .Aufbauart = drpAufbauart.SelectedItem.Text
            End If

            If Trim(txtTyp.Text) <> String.Empty Then
                .Typ = txtTyp.Text
                CheckOK = True
            End If


            If CheckOK = False Then
                lblError.Text = "Bitte wählen Sie ein relevantes Blockkriterium aus."
                Exit Sub
            End If

            If Trim(txtAnzFahrzeuge.Text).Length = 0 Then
                lblError.Text = "Bitte geben Sie einen Wert für Anzahl Fahrzeuge ein."
                Exit Sub
            Else
                If IsNumeric(txtAnzFahrzeuge.Text) = False Then
                    lblError.Text = "Bitte geben Sie einen gültigen Wert für Anzahl Fahrzeuge ein."
                    Exit Sub
                Else
                    If txtAnzFahrzeuge.Text = "0" Then
                        lblError.Text = "Bitte geben Sie einen Wert größer 0 für Anzahl Fahrzeuge ein."
                        Exit Sub
                    Else
                        .AnzFzge = txtAnzFahrzeuge.Text
                    End If

                End If
            End If

            .Haendlernr = txtHaendler.Text

            .SperreAb = txtDatumAb.Text
            .SperreBis = txtDatumBis.Text
            .Sperrvermerk = txtSperrvermerk.Text

        End With

        Try
            clsBlocken.SaveRegel()
            lblError.Text = "Blockregel gespeichert."

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try



        cmdCreate.Visible = False

        clsBlocken = Nothing

        Session("clsBlocken") = Nothing


        cmdNew.Visible = True

    End Sub


    Protected Sub drpHersteller_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpHersteller.SelectedIndexChanged

        If drpHersteller.SelectedItem.Text <> "Auswahl" Then
            drpModellgruppe.Enabled = True
            If clsBlocken Is Nothing = True Then
                clsBlocken = New AvisChange04(m_User, m_App, "")
            End If

            drpModellgruppe.Items.Clear()

            Dim Item As New ListItem

            Item.Text = "Auswahl"
            Item.Value = "Auswahl"

            drpModellgruppe.Items.Add(Item)

            Dim TemTable As DataTable = clsBlocken.DropDownTable

            Dim dv As DataView = TemTable.DefaultView

            dv.RowFilter = "KENNUNG = 'MODELLGRUPPE' and POS_TEXT = '" & _
                                                        drpHersteller.SelectedItem.Text & "'"

            Dim Text As String = "POS_KURZTEXT"
            Dim Value As String = "POS_TEXT"

            'Modellgruppe
            drpModellgruppe.DataSource = dv

            drpModellgruppe.DataTextField = Text
            drpModellgruppe.DataValueField = Value

            drpModellgruppe.DataBind()

        Else
            drpModellgruppe.Items.Clear()

            Dim Item As New ListItem

            Item.Text = "Auswahl"
            Item.Value = "Auswahl"

            drpModellgruppe.Items.Add(Item)


            drpModellgruppe.Enabled = False

        End If

    End Sub

    Protected Sub cmdNew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdNew.Click

        If clsBlocken Is Nothing = True Then
            clsBlocken = New AvisChange04(m_User, m_App, "")
        End If

        Session("clsBlocken") = clsBlocken

        clsBlocken.Aktion = 1

        'cmdNew.Visible = False
        'cmdCreate.Visible = True

        Response.Redirect("Change04_2.aspx?AppID=" & Session("AppID").ToString)

        'Fill()
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click

        clsBlocken = Nothing
        Session("clsBlocken") = Nothing

        Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub calSperreAb_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calSperreAb.SelectionChanged
        txtDatumAb.Text = calSperreAb.SelectedDate.ToShortDateString
        calSperreAb.Visible = False

    End Sub

    Protected Sub calSperreBis_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calSperreBis.SelectionChanged
        txtDatumBis.Text = calSperreBis.SelectedDate.ToShortDateString
        calSperreBis.Visible = False
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        Save()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


End Class

' ************************************************
' $History: Change04_2.aspx.vb $
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 19.07.10   Time: 11:40
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 3966 (KGa)
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 5.12.08    Time: 10:38
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 3.12.08    Time: 9:32
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 2.12.08    Time: 16:14
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 25.11.08   Time: 15:20
' Updated in $/CKAG/Applications/AppAvis/forms
' 