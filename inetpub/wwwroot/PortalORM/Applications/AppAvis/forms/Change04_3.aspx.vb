Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change04_3
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
            lblErrMessage.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Fill()
        If clsBlocken Is Nothing = True Then
            clsBlocken = New AvisChange04(m_User, m_App, "")
        End If

        clsBlocken.FillDropdown()

        Dim TemTable As DataTable = clsBlocken.DropDownTable


        Dim Text As String = "POS_TEXT"
        Dim Value As String = "POS_KURZTEXT"

        Dim dv As DataView = TemTable.DefaultView


        'Einbaufirma
        dv.RowFilter = "KENNUNG = 'EINBAUFIRMA'"
        drpEinbau.DataSource = dv

        drpEinbau.DataTextField = Text
        drpEinbau.DataValueField = Value

        drpEinbau.DataBind()

        'Ausrüstung
        dv.RowFilter = "KENNUNG = 'AUSSTATTUNG'"
        drpAusruest.DataSource = dv

        drpAusruest.DataTextField = Text
        drpAusruest.DataValueField = Value

        drpAusruest.DataBind()

        'User
        'dv.RowFilter = "KENNUNG = 'USER_ANL_BREG'"
        'drpUser.DataSource = dv

        'drpUser.DataTextField = Text
        'drpUser.DataValueField = Value

        'drpUser.DataBind()

        'Carports
        dv.RowFilter = "KENNUNG = 'Carport'"
        drpCarport.DataSource = dv

        drpCarport.DataTextField = Text
        drpCarport.DataValueField = Value

        drpCarport.DataBind()

        'Hersteller
        dv.RowFilter = "KENNUNG = 'HERST_NUMMER'"
        drpHersteller.DataSource = dv

        drpHersteller.DataTextField = Text
        drpHersteller.DataValueField = Value

        drpHersteller.DataBind()


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

    Protected Sub drpHersteller_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpHersteller.SelectedIndexChanged
        If drpHersteller.SelectedItem.Text <> "Auswahl" Then
            drpModellgruppe.Enabled = True
            If clsBlocken Is Nothing = True Then
                clsBlocken = New AvisChange04(m_User, m_App, "")
            End If

            Dim TemTable As DataTable = clsBlocken.DropDownTable


            drpModellgruppe.Items.Clear()

            Dim Item As New ListItem

            Item.Text = "Auswahl"
            Item.Value = "Auswahl"

            drpModellgruppe.Items.Add(Item)

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

    Private Sub Search()

        Dim Text As String = "Auswahl"

        lblErrMessage.Text = String.Empty

        With clsBlocken

            'User
            'If drpUser.SelectedItem.Text <> Text Then
            '    .UserRegel = drpUser.SelectedItem.Text
            'Else
            '    .UserRegel = String.Empty
            'End If

            'Carports
            If drpCarport.SelectedItem.Text <> Text Then
                .Carport = drpCarport.SelectedItem.Text
            Else
                .Carport = String.Empty
            End If

            'Hersteller
            If drpHersteller.SelectedItem.Text <> Text Then
                .Hersteller = drpHersteller.SelectedItem.Text
            Else
                .Hersteller = String.Empty
            End If

            'Modellgruppe
            If drpModellgruppe.SelectedItem.Text <> Text Then
                .Modellgruppe = drpModellgruppe.SelectedItem.Text
            Else
                .Modellgruppe = String.Empty
            End If

            'Kraftstoff
            If drpKraftstoff.SelectedItem.Text <> Text Then
                .Kraftstoff = drpKraftstoff.SelectedItem.Text
            Else
                .Kraftstoff = String.Empty
            End If

            'NAVI
            If drpNavi.SelectedItem.Text <> Text Then
                .NAVI = drpNavi.SelectedItem.Text
            Else
                .NAVI = String.Empty
            End If

            'REIFENART
            If drpReifenart.SelectedItem.Text <> Text Then
                .Reifenart = drpReifenart.SelectedItem.Text
            Else
                .Reifenart = String.Empty
            End If

            'Aufbauart
            If drpAufbauart.SelectedItem.Text <> Text Then
                .Aufbauart = drpAufbauart.SelectedItem.Text
            Else
                .Aufbauart = String.Empty
            End If

            If Trim(txtTyp.Text) <> String.Empty Then
                .Typ = txtTyp.Text
            Else
                .Typ = String.Empty
            End If

            If Trim(txtHaendler.Text) <> String.Empty Then
                .Haendlernr = txtHaendler.Text
            Else
                .Haendlernr = String.Empty
            End If


        End With

        Dim TempTable As DataTable

        TempTable = clsBlocken.SearchGesperrteFahrzeuge

        If TempTable Is Nothing Then
            TempTable = New DataTable
        End If

        If TempTable.Rows.Count > 0 Then
            pnlAuswahl.Visible = False

            lblAusruest.Visible = True
            lblEinbau.Visible = True
            drpEinbau.Visible = True
            drpAusruest.Visible = True

            grvFahrzeuge.Visible = True



            cmdCreate.Text = cmdCreate.Text.Replace("Suchen", "Absenden")

            grvFahrzeuge.DataSource = TempTable
            grvFahrzeuge.DataBind()


        Else

            lblErrMessage.Text = "Es wurden keine Daten gefunden."

        End If


    End Sub

    Private Sub Save()

        Dim Freisetzung As String = String.Empty
        Dim Ausruest As String = String.Empty
        Dim Einbau As String = String.Empty


        lblErrMessage.Text = String.Empty

        If drpEinbau.SelectedItem.Text <> "Auswahl" AndAlso _
            drpAusruest.SelectedItem.Text = "Auswahl" Then
            lblErrMessage.Text = "Bitte geben Sie eine Ausrüstung an."
            Exit Sub
        End If

        If drpEinbau.SelectedItem.Text = "Auswahl" AndAlso _
            drpAusruest.SelectedItem.Text <> "Auswahl" Then
            lblErrMessage.Text = "Bitte geben Sie eine Einbaufirma an."
            Exit Sub
        End If

        If drpEinbau.SelectedItem.Text <> "Auswahl" AndAlso _
            drpAusruest.SelectedItem.Text <> "Auswahl" Then
            Einbau = drpEinbau.SelectedItem.Value
            Ausruest = drpAusruest.SelectedItem.Text
        End If


        If drpEinbau.SelectedItem.Text = "Auswahl" AndAlso _
            drpAusruest.SelectedItem.Text = "Auswahl" Then

            Freisetzung = "X"

        End If


        Dim SAPTable As DataTable = clsBlocken.GetFreigabenSaveTable()

        'Dim SAPTable As New DataTable
        'With SAPTable
        '    .Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
        '    .Columns.Add("AUSRUESTUNG", System.Type.GetType("System.String"))
        '    .Columns.Add("EINBAUFIRMA", System.Type.GetType("System.String"))
        '    .Columns.Add("FLAG_FREISETZUNG", System.Type.GetType("System.String"))
        '    .Columns.Add("RETUR_BEM", System.Type.GetType("System.String"))
        'End With


        Dim GridRow As GridViewRow
        Dim chk As CheckBox

        For Each GridRow In grvFahrzeuge.Rows
            chk = CType(GridRow.FindControl("chkFreigabe"), CheckBox)

            If chk.Checked = True Then
                Dim Row As DataRow = SAPTable.NewRow

                Row("CHASSIS_NUM") = Trim(GridRow.Cells(2).Text)
                Row("AUSRUESTUNG") = Ausruest
                Row("EINBAUFIRMA") = Einbau
                Row("FLAG_FREISETZUNG") = Freisetzung

                SAPTable.Rows.Add(Row)
            End If

        Next

        If SAPTable.Rows.Count = 0 Then
            lblErrMessage.Text = "Keine Fahrzeuge ausgewählt."
        Else
            cmdCreate.Visible = False

            clsBlocken.SaveFreigaben()

            lblErrMessage.Text = "Daten gespeichert."

        End If

    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCreate.Click
        If cmdCreate.Text.Contains("Suchen") Then
            Search()
        Else
            Save()
        End If

    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        clsBlocken = Nothing
        Session("clsBlocken") = Nothing
        If pnlAuswahl.Visible = True Then
            Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
        Else
            Response.Redirect("Change04_3.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class

' ************************************************
' $History: Change04_3.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 19.07.10   Time: 11:40
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 3966 (KGa)
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 8.12.08    Time: 15:43
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 8.12.08    Time: 15:04
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 25.11.08   Time: 15:20
' Updated in $/CKAG/Applications/AppAvis/forms
' 