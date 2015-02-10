
Partial Public Class Modell
    Inherits UserControl
    Private objCarports As Zul_Sperr_Entsperr

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objCarports = CType(Session("App_Report"), Zul_Sperr_Entsperr)

        SetTaskProperties()
    End Sub

    Private Sub TitleList_DataBind(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.DataBinding
        Dim dgi As DataGridItem = CType(Me.BindingContainer, DataGridItem)
        Dim ds As DataSet = CType(dgi.DataItem, DataSet)

        HG1.DataSource = ds
        HG1.DataMember = "Modelle"
        HG1.DataBind()

        objCarports = CType(Session("App_Report"), Zul_Sperr_Entsperr)

        SetTaskProperties()
    End Sub

    Private Sub SetTaskProperties()

        HG1.Columns(16).HeaderText = "Datum <br/> " & objCarports.ArtDerZulassung.ToString()

        Select Case objCarports.Task
            Case "Zulassen"
                HG1.Columns(13).Visible = False         'Bemerkung
                HG1.Columns(14).Visible = False         'Bemerkung Datum 
                HG1.Columns(15).Visible = True          'Datum Erstzulassung
                HG1.Columns(16).Visible = False         'Datum ZieCarport
            Case "Ausgabe"
                HG1.Columns(13).Visible = False         'Bemerkung
                HG1.Columns(14).Visible = False         'Bemerkung Datum 
                HG1.Columns(15).Visible = True          'Datum Erstzulassung
                HG1.Columns(16).Visible = False
                HG1.Columns(17).Visible = False
                HG1.Columns(18).Visible = False
        End Select

    End Sub

    Private Sub HG1_ItemCommand(ByVal source As Object, ByVal e As DataGridCommandEventArgs) Handles HG1.ItemCommand
        Dim i As Integer
        Dim txtBox As TextBox
        Dim AnzOld As String
        Dim AnzVorh As String
        Dim txtZul As TextBox
        Dim zudat As String = ""
        Dim c As DataGrid
        Dim chk As CheckBox
        Dim j As Integer = 0
        Dim iplus As Integer = 0
        Dim iCountforGesamt As Integer = 0
        Dim errMsg As String = ""

        If e.CommandName.ToString = "Edit" Then
            c = CType(e.Item.Cells(1).FindControl("DCP").FindControl("Panel_Modell_Fahrzeug"). _
            FindControl("ChildTemplate_Modell_Fahrzeug").FindControl("DG1"),  _
                        DataGrid)

            txtBox = CType(e.Item.Cells(18).FindControl("Anzahl_neu"), TextBox)
            AnzOld = e.Item.Cells(17).Text
            AnzVorh = e.Item.Cells(18).Text
            If AnzOld = "" Then AnzOld = "0"
            If AnzVorh = "" Then AnzVorh = "0"
            If Not Session("App_ZulDat") Is Nothing Then
                zudat = Session("App_ZulDat").ToString
                txtZul = CType(e.Item.Cells(15).FindControl("txtDatumErstzulassung"), TextBox)
                txtZul.Text = zudat
            Else
                txtZul = CType(e.Item.Cells(15).FindControl("txtDatumErstzulassung"), TextBox)
                zudat = txtZul.Text
            End If
            If IsNumeric(txtBox.Text) AndAlso IsNumeric(AnzOld) Then
                If CInt(txtBox.Text) <= CInt(AnzVorh) Then
                    If CInt(txtBox.Text) >= 0 Then

                        If CInt(AnzOld) > CInt(txtBox.Text) Then
                            j = CInt(AnzOld) - CInt(txtBox.Text)
                            i = c.Items.Count - 1
                            e.Item.Cells(17).Text = txtBox.Text
                            Dim CountBack As Integer = 1
                            Do While i >= 0
                                If CountBack <= j Then
                                    txtZul = CType(c.Items(i + iplus).Cells(9).FindControl("txtDatumErstzulassung"), TextBox)
                                    If txtZul.Enabled = True Then
                                        txtZul.Text = ""
                                        chk = CType(c.Items(i + iplus).Cells(11).FindControl("chkAuswahl"), CheckBox)
                                        If chk.Checked = True Then
                                            chk.Checked = False
                                            iCountforGesamt = iCountforGesamt - 1
                                            CountBack += 1
                                        End If

                                    End If
                                    i = i - 1
                                Else
                                    Exit Do
                                End If

                            Loop
                        Else
                            If IsNumeric(txtBox.Text) Then
                                If CInt(txtBox.Text) > 0 Then
                                    j = CInt(txtBox.Text)
                                    e.Item.Cells(17).Text = txtBox.Text
                                    Do While i <= j - 1
                                        If i + iplus <= c.Items.Count - 1 Then
                                            txtZul = CType(c.Items(i + iplus).Cells(9).FindControl("txtDatumErstzulassung"), TextBox)
                                            If txtZul.Enabled = True Then
                                                txtZul.Text = zudat
                                                chk = CType(c.Items(i + iplus).Cells(11).FindControl("chkAuswahl"), CheckBox)
                                                If chk.Checked = False Then
                                                    chk.Checked = True
                                                    iCountforGesamt += 1
                                                End If
                                                i += 1
                                            Else
                                                iplus += 1
                                            End If
                                        Else : Exit Do
                                        End If
                                    Loop
                                ElseIf CInt(txtBox.Text) = 0 Then
                                    Do While i <= c.Items.Count - 1
                                        If i + iplus <= c.Items.Count - 1 Then
                                            txtZul = CType(c.Items(i + iplus).Cells(9).FindControl("txtDatumErstzulassung"), TextBox)
                                            If txtZul.Enabled = True Then
                                                txtZul.Text = ""
                                                chk = CType(c.Items(i + iplus).Cells(11).FindControl("chkAuswahl"), CheckBox)
                                                If chk.Checked = True Then
                                                    chk.Checked = False

                                                    iCountforGesamt = iCountforGesamt - 1
                                                End If
                                                i += 1
                                            Else
                                                iplus += 1
                                            End If
                                        Else : Exit Do
                                        End If
                                    Loop
                                End If
                            End If

                        End If
                    Else : errMsg = "Der Zahlenwert für die Anzahl ist zu klein."
                    End If
                Else : errMsg = "Der Zahlenwert für die Anzahl ist zu groß."
                End If
            Else : errMsg = "Geben Sie ein Zahlenwert ein."
            End If

            Dim obj As Control
            Dim obj1 As HtmlInputText
            Dim lbl As Label
            Dim strText As String
            For Each obj In Me.Parent.Page.Controls
                strText = obj.UniqueID
                If strText = "Form1" Then
                    If errMsg.Length = 0 Then
                        If iCountforGesamt <> 0 Then
                            obj1 = CType(obj.FindControl("AktuelleAnzahl"), HtmlInputText)
                            If Not obj1 Is Nothing Then
                                If IsNumeric(obj1.Value) Then
                                    obj1.Value = (iCountforGesamt + CInt(obj1.Value)).ToString
                                Else
                                    obj1.Value = iCountforGesamt.ToString
                                End If
                            End If
                            obj1 = CType(obj.FindControl("AktuelleSumme"), HtmlInputText)
                            If IsNumeric(obj1.Value) Then
                                obj1.Value = (iCountforGesamt + CInt(obj1.Value)).ToString
                            Else
                                obj1.Value = iCountforGesamt.ToString
                            End If
                        Else : Exit For
                        End If
                    Else
                        lbl = CType(obj.FindControl("lblError"), Label)
                        If Not lbl Is Nothing Then
                            lbl.Text = errMsg
                            Exit For
                        End If
                    End If

                End If
            Next
        Else
            SelectAll(source, e)
        End If

    End Sub

    Private Sub SelectAll(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
        Dim c As DataGrid
        Dim GridItem As DataGridItem
        Dim Grid As DBauer.Web.UI.WebControls.HierarGrid
        Dim i As Integer
        Dim txtBox As TextBox
        Dim AnzOld As String
        Dim AnzVorh As String
        Dim txtZul As TextBox
        Dim zudat As String = ""
        Dim chk As CheckBox
        Dim j As Integer = 0
        Dim iplus As Integer = 0
        Dim iCountforGesamt As Integer = 0
        Dim errMsg As String = ""
        Grid = CType(source, DBauer.Web.UI.WebControls.HierarGrid)

        For Each GridItem In Grid.Items
            j = 0
            i = 0
            c = CType(GridItem.Cells(1).FindControl("DCP").FindControl("Panel_Modell_Fahrzeug"). _
                FindControl("ChildTemplate_Modell_Fahrzeug").FindControl("DG1"),  _
                DataGrid)
            txtBox = CType(GridItem.Cells(18).FindControl("Anzahl_neu"), TextBox)
            AnzOld = GridItem.Cells(17).Text
            AnzVorh = GridItem.Cells(18).Text
            If AnzOld = "" Then AnzOld = "0"
            If AnzVorh = "" Then AnzVorh = "0"
            If Not Session("App_ZulDat") Is Nothing Then
                zudat = Session("App_ZulDat").ToString
                txtZul = CType(GridItem.Cells(15).FindControl("txtDatumErstzulassung"), TextBox)
                If CType(txtBox.Text, Integer) > 0 Then
                    txtZul.Text = zudat
                Else
                    txtZul.Text = ""
                End If

            Else
                If CType(txtBox.Text, Integer) > 0 Then
                    txtZul = CType(GridItem.Cells(15).FindControl("txtDatumErstzulassung"), TextBox)
                    txtZul.Text = zudat
                    txtZul.Text = ""
                End If
            End If
            If IsNumeric(txtBox.Text) AndAlso IsNumeric(AnzOld) Then
                If CInt(txtBox.Text) <= CInt(AnzVorh) Then
                    If CInt(txtBox.Text) >= 0 Then

                        If CInt(AnzOld) > CInt(txtBox.Text) Then
                            j = CInt(AnzOld) - CInt(txtBox.Text)
                            i = c.Items.Count - 1
                            GridItem.Cells(17).Text = txtBox.Text
                            Dim CountBack As Integer = 1
                            Do While i >= 0
                                If CountBack <= j Then
                                    txtZul = CType(c.Items(i + iplus).Cells(9).FindControl("txtDatumErstzulassung"), TextBox)
                                    If txtZul.Enabled = True Then
                                        txtZul.Text = ""
                                        chk = CType(c.Items(i + iplus).Cells(11).FindControl("chkAuswahl"), CheckBox)
                                        If chk.Checked = True Then
                                            chk.Checked = False
                                            iCountforGesamt = iCountforGesamt - 1
                                            CountBack += 1
                                        End If

                                    End If
                                    i = i - 1
                                Else
                                    Exit Do
                                End If

                            Loop
                        Else
                            If IsNumeric(txtBox.Text) Then
                                If CInt(txtBox.Text) > 0 Then
                                    j = CInt(txtBox.Text)
                                    GridItem.Cells(17).Text = txtBox.Text
                                    Do While i <= j - 1
                                        If i + iplus <= c.Items.Count - 1 Then
                                            txtZul = CType(c.Items(i + iplus).Cells(9).FindControl("txtDatumErstzulassung"), TextBox)
                                            If txtZul.Enabled = True Then
                                                txtZul.Text = zudat
                                                chk = CType(c.Items(i + iplus).Cells(11).FindControl("chkAuswahl"), CheckBox)
                                                If chk.Checked = False Then
                                                    chk.Checked = True
                                                    iCountforGesamt += 1
                                                End If
                                                i += 1
                                            Else
                                                iplus += 1
                                            End If
                                        Else : Exit Do
                                        End If
                                    Loop
                                ElseIf CInt(txtBox.Text) = 0 Then
                                    Do While i <= c.Items.Count - 1
                                        If i + iplus <= c.Items.Count - 1 Then
                                            txtZul = CType(c.Items(i + iplus).Cells(9).FindControl("txtDatumErstzulassung"), TextBox)
                                            If txtZul.Enabled = True Then
                                                txtZul.Text = ""
                                                chk = CType(c.Items(i + iplus).Cells(11).FindControl("chkAuswahl"), CheckBox)
                                                If chk.Checked = True Then
                                                    chk.Checked = False

                                                    iCountforGesamt = iCountforGesamt - 1
                                                End If
                                                i += 1
                                            Else
                                                iplus += 1
                                            End If
                                        Else : Exit Do
                                        End If
                                    Loop
                                End If
                            End If

                        End If
                    Else : errMsg = "Der Zahlenwert für die Anzahl ist zu klein."
                    End If
                Else : errMsg = "Der Zahlenwert für die Anzahl ist zu groß."
                End If
            Else : errMsg = "Geben Sie ein Zahlenwert ein."
            End If

        Next
        Dim obj As Control
        Dim obj1 As HtmlInputText
        Dim lbl As Label
        Dim strText As String
        For Each obj In Me.Parent.Page.Controls
            strText = obj.UniqueID
            If strText = "Form1" Then
                If errMsg.Length = 0 Then
                    If iCountforGesamt <> 0 Then
                        obj1 = CType(obj.FindControl("AktuelleAnzahl"), HtmlInputText)
                        If Not obj1 Is Nothing Then
                            If IsNumeric(obj1.Value) Then
                                obj1.Value = (iCountforGesamt + CInt(obj1.Value)).ToString
                            Else
                                obj1.Value = iCountforGesamt.ToString
                            End If
                        End If
                        obj1 = CType(obj.FindControl("AktuelleSumme"), HtmlInputText)
                        If IsNumeric(obj1.Value) Then
                            obj1.Value = (iCountforGesamt + CInt(obj1.Value)).ToString
                        Else
                            obj1.Value = iCountforGesamt.ToString
                        End If
                    Else : Exit For
                    End If
                Else
                    lbl = CType(obj.FindControl("lblError"), Label)
                    If Not lbl Is Nothing Then
                        lbl.Text = errMsg
                        Exit For
                    End If
                End If

            End If
        Next
    End Sub

    Private Sub HG1_TemplateSelection(ByVal sender As Object, ByVal e As DBauer.Web.UI.WebControls.HierarGridTemplateSelectionEventArgs) Handles HG1.TemplateSelection

        Select Case (e.Row.Table.TableName)
            Case "Fahrzeuge"
                e.TemplateFilename = "..\Templates\Fahrzeug.ascx"
            Case Else
                Throw New NotImplementedException("Unexpected child row in TemplateSelection event")
        End Select
    End Sub

End Class