Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Imports SapORM.Contracts

Partial Public Class Change04_4
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

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
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

        End With

        Dim TempTable As DataTable

        TempTable = clsBlocken.SearchRegeln

        If TempTable Is Nothing Then
            TempTable = New DataTable
        End If

        If TempTable.Rows.Count > 0 Then

            'Dim Row As DataRow

            'For Each Row In TempTable.Rows

            '    If Row("Carport").ToString = String.Empty Then
            '        Row("Carport") = "Auswahl"
            '    End If



            'Next


            pnlAuswahl.Visible = False


            grvRegeln.Visible = True


            'cmdCreate.Text.Replace("Suchen", "Absenden")

            cmdCreate.Text = cmdCreate.Text.Replace("Suchen", "Absenden")

            grvRegeln.DataSource = TempTable
            grvRegeln.DataBind()


            Dim Drp As DropDownList
            Dim DrpModell As DropDownList
            Dim lbl As Label
            Dim lblModellCheck As Label

            Dim GridRow As GridViewRow


            For Each GridRow In grvRegeln.Rows

                'Carport
                lbl = CType(GridRow.FindControl("lblCarport"), Label)
                Drp = CType(GridRow.FindControl("drpCarportGrid"), DropDownList)

                If lbl.Text.Length = 0 Then

                    Dim Item As ListItem

                    For Each Item In drpCarport.Items

                        Drp.Items.Add(Item)

                    Next

                    lbl.Visible = False

                    Drp.Visible = True
                Else
                    lbl.Visible = True
                    Drp.Visible = False
                End If

                'Farbe

                lbl = CType(GridRow.FindControl("lblFarbe"), Label)
                Drp = CType(GridRow.FindControl("drpFarbeGrid"), DropDownList)

                If lbl.Text.Length = 0 Then

                    Dim Item As ListItem

                    For Each Item In drpFarbe.Items

                        Drp.Items.Add(Item)

                    Next

                    lbl.Visible = False


                    Drp.Visible = True
                Else
                    lbl.Visible = True
                    Drp.Visible = False
                End If

                'Liefermonat

                lbl = CType(GridRow.FindControl("lblLiefermonat"), Label)
                Drp = CType(GridRow.FindControl("drpLiefermonatGrid"), DropDownList)

                If lbl.Text.Length = 0 Then

                    Dim Item As ListItem

                    For Each Item In drpLiefermonat.Items

                        Drp.Items.Add(Item)

                    Next

                    lbl.Visible = False


                    Drp.Visible = True
                Else
                    lbl.Visible = True
                    Drp.Visible = False
                End If


                'Hersteller
                lbl = CType(GridRow.FindControl("lblHersteller"), Label)
                Drp = CType(GridRow.FindControl("drpHerstellerGrid"), DropDownList)

                If lbl.Text.Length = 0 Then

                    Dim Item As ListItem

                    For Each Item In drpHersteller.Items

                        Drp.Items.Add(Item)

                    Next

                    lbl.Visible = False


                    Drp.Visible = True
                Else
                    lbl.Visible = True
                    Drp.Visible = False
                End If

                'Modellgruppe
                lblModellCheck = CType(GridRow.FindControl("lblModellgruppe"), Label)
                DrpModell = CType(GridRow.FindControl("drpModellgruppeGrid"), DropDownList)

                If lblModellCheck.Text.Length = 0 Then

                    'Hersteller-Zustand prüfen
                    If lbl.Text.Length = 0 Then
                        'Hersteller kann ausgewählt werden
                        DrpModell.Enabled = False

                    Else
                        'Hersteller hat einen Wert; Modellgruppe entsprechend füllen
                        Dim Item As New ListItem

                        DrpModell.Enabled = True

                        Item.Text = "Auswahl"
                        Item.Value = "Auswahl"

                        DrpModell.Items.Add(Item)

                        Dim dv As DataView = clsBlocken.DropDownTable.DefaultView

                        dv.RowFilter = "KENNUNG = 'MODELLGRUPPE' and POS_TEXT = '" & _
                                                                    lbl.Text & "'"


                        'Modellgruppe
                        DrpModell.DataSource = dv

                        DrpModell.DataTextField = "POS_KURZTEXT"
                        DrpModell.DataValueField = "POS_TEXT"

                        DrpModell.DataBind()


                    End If


                Else
                    lblModellCheck.Visible = True
                    DrpModell.Visible = False
                End If

                'Kraftstoff
                lbl = CType(GridRow.FindControl("lblKraftstoff"), Label)
                Drp = CType(GridRow.FindControl("drpKraftstoffGrid"), DropDownList)

                If lbl.Text.Length = 0 Then

                    Dim Item As ListItem

                    For Each Item In drpKraftstoff.Items

                        Drp.Items.Add(Item)

                    Next

                    lbl.Visible = False


                    Drp.Visible = True
                Else
                    lbl.Visible = True
                    Drp.Visible = False
                End If


                'Navi
                lbl = CType(GridRow.FindControl("lblNavi"), Label)
                Drp = CType(GridRow.FindControl("drpNaviGrid"), DropDownList)

                If lbl.Text.Length = 0 Then

                    Dim Item As ListItem

                    For Each Item In drpNavi.Items

                        Drp.Items.Add(Item)

                    Next

                    lbl.Visible = False


                    Drp.Visible = True
                Else
                    lbl.Visible = True
                    Drp.Visible = False
                End If


                'Bereifung
                lbl = CType(GridRow.FindControl("lblBereifung"), Label)
                Drp = CType(GridRow.FindControl("drpBereifungGrid"), DropDownList)

                If lbl.Text.Length = 0 Then

                    Dim Item As ListItem

                    For Each Item In drpReifenart.Items

                        Drp.Items.Add(Item)

                    Next

                    lbl.Visible = False


                    Drp.Visible = True
                Else
                    lbl.Visible = True
                    Drp.Visible = False
                End If

                'Aufbauart
                lbl = CType(GridRow.FindControl("lblAufbauart"), Label)
                Drp = CType(GridRow.FindControl("drpAufbauartGrid"), DropDownList)

                If lbl.Text.Length = 0 Then

                    Dim Item As ListItem

                    For Each Item In drpAufbauart.Items

                        Drp.Items.Add(Item)

                    Next

                    lbl.Visible = False


                    Drp.Visible = True
                Else
                    lbl.Visible = True
                    Drp.Visible = False
                End If


            Next


        Else

            lblErrMessage.Text = "Es wurden keine Daten gefunden."
            'cmdCreate.Visible = False

        End If

        'clsBlocken.SaveRegel()

        'Session("clsBlocken") = Nothing


    End Sub

    Private Sub Save()

        'Dim SAPTable As New DataTable



        'With SAPTable

        '    .Columns.Add("ID_BLOCK_RG", System.Type.GetType("System.String"))
        '    .Columns.Add("DAT_STOR_BREG", System.Type.GetType("System.String"))
        '    .Columns.Add("UZEIT_STOR_BREG", System.Type.GetType("System.String"))
        '    .Columns.Add("FARBE_DE", System.Type.GetType("System.String"))
        '    .Columns.Add("CARPORT", System.Type.GetType("System.String"))
        '    .Columns.Add("GEPL_LIEFTERMIN", System.Type.GetType("System.String"))
        '    .Columns.Add("HERST_NUMMER", System.Type.GetType("System.String"))
        '    .Columns.Add("MODELLGRUPPE", System.Type.GetType("System.String"))
        '    .Columns.Add("KRAFTSTOFF", System.Type.GetType("System.String"))
        '    .Columns.Add("NAVIGATION", System.Type.GetType("System.String"))
        '    .Columns.Add("REIFENART", System.Type.GetType("System.String"))
        '    .Columns.Add("AUFBAUART", System.Type.GetType("System.String"))
        '    .Columns.Add("TYP", System.Type.GetType("System.String"))
        '    .Columns.Add("DAT_SPERR_AB", System.Type.GetType("System.String"))
        '    .Columns.Add("DAT_SPERR_BIS", System.Type.GetType("System.String"))
        '    .Columns.Add("SPERRVERMERK", System.Type.GetType("System.String"))
        '    .Columns.Add("ANZ_FZG", System.Type.GetType("System.String"))
        '    .Columns.Add("WEB_USER", System.Type.GetType("System.String"))
        '    .Columns.Add("RETUR_BEM", System.Type.GetType("System.String"))
        '    .Columns.Add("LIEFERANT", System.Type.GetType("System.String"))
        'End With

        Dim SAPTable As DataTable = clsBlocken.GetSaveTable()


        Dim GridRow As GridViewRow


        For Each GridRow In grvRegeln.Rows

            If GridRow.Enabled = True Then

                Dim Row As DataRow = SAPTable.NewRow


                Row("ID_BLOCK_RG") = Right("0000000000" & GridRow.Cells(0).Text, 10)

                Dim chk As CheckBox

                chk = CType(GridRow.FindControl("chkLoeschen"), CheckBox)

                If chk.Checked = True Then
                    Row("DAT_STOR_BREG") = ChangeDate(CStr((Date.Today)))
                    Row("UZEIT_STOR_BREG") = CStr(Format(TimeOfDay, "HHmmss"))
                    Row("WEB_USER") = m_User.UserName

                    GridRow.Enabled = False

                Else


                    '******************Controls*************

                    Dim lbl As Label
                    Dim drp As DropDownList
                    Dim txt As TextBox

                    'Carport
                    lbl = CType(GridRow.FindControl("lblCarport"), Label)
                    drp = CType(GridRow.FindControl("drpCarportGrid"), DropDownList)

                    If lbl.Text.Length = 0 Then

                        If drp.SelectedItem.Text <> "Auswahl" Then
                            Row("CARPORT") = drp.SelectedItem.Text
                        End If
                    Else
                        Row("CARPORT") = lbl.Text
                    End If

                    'Farbe

                    lbl = CType(GridRow.FindControl("lblFarbe"), Label)
                    drp = CType(GridRow.FindControl("drpFarbeGrid"), DropDownList)

                    If lbl.Text.Length = 0 Then

                        If drp.SelectedItem.Text <> "Auswahl" Then
                            Row("FARBE_DE") = drp.SelectedItem.Text
                        End If
                    Else
                        Row("FARBE_DE") = lbl.Text
                    End If

                    'Liefermonat

                    lbl = CType(GridRow.FindControl("lblLiefermonat"), Label)
                    drp = CType(GridRow.FindControl("drpLiefermonatGrid"), DropDownList)

                    If lbl.Text.Length = 0 Then

                        If drp.SelectedItem.Text <> "Auswahl" Then
                            Row("GEPL_LIEFTERMIN") = drp.SelectedItem.Text
                        End If
                    Else
                        Row("GEPL_LIEFTERMIN") = lbl.Text

                    End If


                    'Hersteller
                    lbl = CType(GridRow.FindControl("lblHersteller"), Label)
                    drp = CType(GridRow.FindControl("drpHerstellerGrid"), DropDownList)

                    If lbl.Text.Length = 0 Then

                        If drp.SelectedItem.Text <> "Auswahl" Then
                            Row("HERST_NUMMER") = drp.SelectedItem.Text
                        End If
                    Else
                        Row("HERST_NUMMER") = lbl.Text
                    End If

                    'Modellgruppe
                    lbl = CType(GridRow.FindControl("lblModellgruppe"), Label)
                    drp = CType(GridRow.FindControl("drpModellgruppeGrid"), DropDownList)

                    If lbl.Text.Length = 0 AndAlso drp.Visible = True AndAlso drp.Enabled = True Then

                        If drp.SelectedItem.Text <> "Auswahl" Then
                            Row("MODELLGRUPPE") = drp.SelectedItem.Text
                        End If
                    Else
                        Row("MODELLGRUPPE") = lbl.Text

                    End If

                    'Kraftstoff
                    lbl = CType(GridRow.FindControl("lblKraftstoff"), Label)
                    drp = CType(GridRow.FindControl("drpKraftstoffGrid"), DropDownList)

                    If lbl.Text.Length = 0 Then

                        If drp.SelectedItem.Text <> "Auswahl" Then
                            Row("KRAFTSTOFF") = drp.SelectedItem.Text
                        End If
                    Else
                        Row("KRAFTSTOFF") = lbl.Text
                    End If


                    'Navi
                    lbl = CType(GridRow.FindControl("lblNavi"), Label)
                    drp = CType(GridRow.FindControl("drpNaviGrid"), DropDownList)

                    If lbl.Text.Length = 0 Then

                        If drp.SelectedItem.Text <> "Auswahl" Then
                            Row("NAVIGATION") = drp.SelectedItem.Text
                        End If
                    Else
                        Row("NAVIGATION") = lbl.Text
                    End If


                    'Bereifung
                    If lbl.Text.Length = 0 Then

                        If drp.SelectedItem.Text <> "Auswahl" Then
                            Row("REIFENART") = drp.SelectedItem.Text
                        End If
                    Else
                        Row("REIFENART") = lbl.Text
                    End If

                    'Aufbauart
                    lbl = CType(GridRow.FindControl("lblAufbauart"), Label)
                    drp = CType(GridRow.FindControl("drpAufbauartGrid"), DropDownList)

                    If lbl.Text.Length = 0 Then

                        If drp.SelectedItem.Text <> "Auswahl" Then
                            Row("AUFBAUART") = drp.SelectedItem.Text
                        End If
                    Else
                        Row("AUFBAUART") = lbl.Text
                    End If

                    txt = CType(GridRow.FindControl("txtTyp"), TextBox)
                    lbl = CType(GridRow.FindControl("lblTyp"), Label)

                    If txt.Text.Length > 0 Then
                        Row("TYP") = txt.Text
                    Else
                        Row("TYP") = lbl.Text
                    End If

                    txt = CType(GridRow.FindControl("txtSoll"), TextBox)

                    Row("ANZ_FZG") = txt.Text


                    txt = CType(GridRow.FindControl("txtSperreAb"), TextBox)
                    lbl = CType(GridRow.FindControl("lblSperreAb"), Label)

                    If txt.Text.Length > 0 Then
                        Row("DAT_SPERR_AB") = ChangeDate(txt.Text)
                    Else
                        Row("DAT_SPERR_AB") = ChangeDate(lbl.Text)
                    End If

                    txt = CType(GridRow.FindControl("txtSperreBis"), TextBox)
                    lbl = CType(GridRow.FindControl("lblSperreBis"), Label)

                    If txt.Text.Length > 0 Then
                        Row("DAT_SPERR_BIS") = ChangeDate(txt.Text)
                    Else
                        Row("DAT_SPERR_BIS") = ChangeDate(lbl.Text)
                    End If

                    Row("SPERRVERMERK") = Replace(GridRow.Cells(16).Text.Trim, "&nbsp;", "")
                    Row("LIEFERANT") = Replace(GridRow.Cells(13).Text.Trim, "&nbsp;", "")
                    Row("WEB_USER") = Replace(GridRow.Cells(17).Text.Trim, "&nbsp;", "")



                End If

                SAPTable.Rows.Add(Row)

            End If

        Next


        Dim ReturnTable As DataTable = clsBlocken.ChangeRegel(SAPTable)


        If ReturnTable.Rows.Count = 0 Then
            lblErrMessage.Text = "Daten gesichert."
        Else

            lblErrMessage.Text = "Es sind Fehler aufgetreten."

            Dim RetRow As DataRow
            Dim i As Integer


            For Each RetRow In ReturnTable.Rows

                If RetRow("RETUR_BEM").ToString <> String.Empty Then
                    grvRegeln.Columns(18).Visible = True
                    grvRegeln.Rows(i).Cells(18).Text = RetRow("RETUR_BEM").ToString
                End If


            Next
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
            Response.Redirect("Change04_4.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub

    Protected Sub drpHerstellerGrid_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)


        Dim drpHersteller As DropDownList

        drpHersteller = CType(sender, DropDownList)


        Dim row As GridViewRow
        Dim drp As DropDownList
        'Dim drpHersteller As DropDownList
        Dim drpModell As DropDownList

        For Each row In grvRegeln.Rows

            drp = CType(row.FindControl("drpHerstellerGrid"), DropDownList)

            If drpHersteller.ClientID = drp.ClientID Then

                drpModell = CType(row.FindControl("drpModellgruppeGrid"), DropDownList)


                If drpHersteller.SelectedItem.Text <> "Auswahl" Then


                    drpModell.Items.Clear()

                    Dim Item As New ListItem

                    drpModell.Enabled = True

                    Item.Text = "Auswahl"
                    Item.Value = "Auswahl"

                    drpModell.Items.Add(Item)

                    Dim dv As DataView = clsBlocken.DropDownTable.DefaultView

                    dv.RowFilter = "KENNUNG = 'MODELLGRUPPE' and POS_TEXT = '" & _
                                                                drpHersteller.SelectedItem.Text & "'"


                    'Modellgruppe
                    drpModell.DataSource = dv

                    drpModell.DataTextField = "POS_KURZTEXT"
                    drpModell.DataValueField = "POS_TEXT"

                    drpModell.DataBind()

                Else
                    drpModell.Items.Clear()
                    drpModell.Enabled = False

                End If




                Exit For
            End If
        Next

    End Sub

    Private Function ChangeDate(ByVal Datum As String) As Object
        'Dim Temp As String = String.Empty
        'If Datum = Nothing Then
        '    Temp = "00000000"
        'Else
        '    If Datum.Length = 10 Then
        '        Temp = Datum.Replace(".", "")
        '        Temp = Right(Temp, 4) & Mid(Temp, 3, 2) & Left(Temp, 2)
        '    Else
        '        Temp = "00000000"
        '    End If
        'End If

        'Return Temp

        Return Datum.NotEmptyOrDbNull

    End Function


End Class

' ************************************************
' $History: Change04_4.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 20.07.10   Time: 15:53
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 19.07.10   Time: 11:40
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA: 3966 (KGa)
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 25.11.08   Time: 15:20
' Updated in $/CKAG/Applications/AppAvis/forms
' 