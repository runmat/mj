Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class ChangeStdText
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private clsUeberTexte As UeberfgStandard_01

    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lbtnconfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lvGruppe As System.Web.UI.WebControls.ListBox
    Protected WithEvents trlvGruppe As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents dgSearchResult As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents lvUntergruppe As System.Web.UI.WebControls.ListBox
    Protected WithEvents TRlbtnNew As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents TRlbtnGruppe As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents TRlbtnCancel As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents TRlbtnSave As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents TRlbtnUgruppe As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbtnEditGruppe As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnNeuGruppe As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnEditUGruppe As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnNeuUGruppe As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtGruppe As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents txtUGruppe As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents txtStdTextName As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtBemerkung As CKG.Portal.PageElements.TextAreaControl
    Protected WithEvents lbtnSaveGruppe As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trGruppe As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trUntergruppe As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trStdTextName As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trEdit As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtDummy As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lbtnBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnTexte As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnGruppen As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trlvGruppelabel As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbuttons As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents TRResult As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trHauptgruppe As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trButtons As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents txtHauptID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtUnterID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtTextID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtSave As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lbtnNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbtnCancel As System.Web.UI.WebControls.LinkButton
    Protected WithEvents txtVorschau As CKG.Portal.PageElements.TextAreaControl
    Protected WithEvents lbtnDelete As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucStyles As Styles

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)
            If IsPostBack = False Then
                GetTexte()
            End If
            If Not Session("App_FromPage") Is Nothing Then
                lbtnTexte.Visible = False
                lbtnGruppen.Visible = False
                lbtnBack.Visible = True
                lbtnconfirm.Visible = True
                TRResult.Visible = True
                dgSearchResult.Visible = True
                trlvGruppelabel.Visible = True
                trlvGruppe.Visible = True
                trEdit.Visible = False
                dgSearchResult.Columns(5).Visible = False
                dgSearchResult.Columns(6).Visible = False
                dgSearchResult.Columns(7).Visible = True
                dgSearchResult.Columns(8).Visible = True
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub GetTexte()
        Dim tblTexte As DataTable
        Dim tblHaupt As DataTable
        Dim tblUnter As DataTable
        Dim vwGr1 As DataView
        'Dim sValueold As String = ""
        Dim sTemp As String = ""
        Dim sTemp2 As String = ""

        If Session("App_Ueberftexte") Is Nothing Then
            clsUeberTexte = New UeberfgStandard_01(m_User, m_App, "")
            tblTexte = clsUeberTexte.getStdTexte(tblHaupt, tblUnter)

            If tblTexte IsNot Nothing Then
                tblHaupt.DefaultView.Sort = "Name"
                Session("App_tblHaupt") = tblHaupt
                tblUnter.DefaultView.Sort = "Name"
                Session("App_tblUnter") = tblUnter
                tblTexte.DefaultView.Sort = "Name"
                Session("App_Standardtext") = tblTexte
            End If
        Else
            clsUeberTexte = Session("App_Ueberftexte")
            tblHaupt = Session("App_tblHaupt")
            tblUnter = Session("App_tblUnter")
            tblTexte = Session("App_Standardtext")
        End If


        If Not IsNothing(tblHaupt) Then
            lvGruppe.DataSource = tblHaupt
            lvGruppe.DataTextField = "Name"
            lvGruppe.DataValueField = "Haupt_ID"
            lvGruppe.DataBind()
            If lvGruppe.Items.Count > 0 AndAlso IsNothing(Session("AppRowHaupt")) Then
                lvGruppe.Items(0).Selected = True
            ElseIf lvGruppe.Items.Count > 0 Then
                lvGruppe.Items(Session("AppRowHaupt")).Selected = True
            End If
        End If

        If lvGruppe.SelectedIndex >= 0 Then
            sTemp = lvGruppe.SelectedItem.Value
        End If

        FillUnterGruppe(tblUnter, sTemp)

        If lvUntergruppe.SelectedIndex >= 0 Then
            sTemp2 = lvUntergruppe.SelectedItem.Value
        ElseIf tblUnter Is Nothing OrElse tblUnter.Rows.Count = 0 Then
            sTemp2 = "0000000000"
        End If

        If tblTexte IsNot Nothing AndAlso tblTexte.Rows.Count > 0 Then
            vwGr1 = New DataView
            If Not sTemp2 = String.Empty AndAlso Not sTemp = String.Empty Then
                vwGr1 = tblTexte.DefaultView 'Select("Sort1='" & sTemp & "' And Sort2='" & sTemp2 & "'")
                vwGr1.RowFilter = "Haupt_ID='" & sTemp & "' And Unter_ID='" & sTemp2 & "'"
            End If
            dgSearchResult.DataSource = vwGr1
            dgSearchResult.DataBind()
        Else
            lbtnNew.Visible = False
            TRResult.Visible = True
            trEdit.Visible = True
            txtDummy.Value = "Neu"
            If Not Request.QueryString("A#") Is Nothing Then lblMessage.Text = "Es wurden noch keine Texte eingepflegt!"
        End If

    End Sub

    Private Sub FillUnterGruppe(ByVal tblUGruppe As DataTable, ByVal strSort1 As String)

        If tblUGruppe IsNot Nothing Then
            Dim vwGr1 As DataView

            vwGr1 = tblUGruppe.DefaultView
            vwGr1.RowFilter = "Haupt_ID='" & strSort1 & "' "

            lvUntergruppe.DataSource = vwGr1
            lvUntergruppe.DataValueField = "Unter_ID"
            lvUntergruppe.DataTextField = "Name"
            lvUntergruppe.SelectionMode = ListSelectionMode.Single
            lvUntergruppe.DataBind()

            If lvUntergruppe.Items.Count > 0 AndAlso IsNothing(Session("AppRowUnter")) Then
                lvUntergruppe.Items(0).Selected = True
            ElseIf lvUntergruppe.Items.Count > 0 Then
                lvUntergruppe.Items(Session("AppRowUnter")).Selected = True
            End If
        End If

    End Sub

    Private Sub lbtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnCancel.Click
        If Left(txtSave.Value, 1) = "T" Then
            lbtnNew.Visible = True
            trEdit.Visible = False
            trStdTextName.Visible = False
            txtDummy.Value = ""
            txtBemerkung.Text = ""
            txtStdTextName.Text = ""
            lbtnGruppen.Visible = True
        Else
            trUntergruppe.Visible = False
            trHauptgruppe.Visible = False
            txtGruppe.Enabled = True
            txtGruppe.Text = ""
            txtHauptID.Value = ""
            txtUGruppe.Text = ""
            txtUnterID.Value = ""
            lbtnNew.Visible = False
            lbtnGruppen.Visible = True
        End If
        trButtons.Visible = False
        txtSave.Value = ""
    End Sub

    Private Sub lbtnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNew.Click
        lbtnNew.Visible = False
        TRResult.Visible = True
        trEdit.Visible = True
        trButtons.Visible = True
        trStdTextName.Visible = True
        txtDummy.Value = ""
        If Not IsNothing(lvGruppe.SelectedItem) Then
            txtHauptID.Value = lvGruppe.SelectedItem.Value
        ElseIf lvGruppe.Rows = 0 Then
            lblMessage.Text = ("Wählen Sie mindestens eine Hauptgruppe aus bzw. legen Sie eine an!")
        Else
            txtUnterID.Value = "0000000000"
        End If

        If Not IsNothing(lvUntergruppe.SelectedItem) Then
            txtUnterID.Value = lvUntergruppe.SelectedItem.Value
        Else
            txtUnterID.Value = "0000000000"
        End If
        txtSave.Value = "T2"
    End Sub


    Private Sub lbtnconfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnconfirm.Click
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim ctl As Control
        Dim chk1 As CheckBox
        Dim Str As String = ""
        Try
            For Each item In dgSearchResult.Items
                For Each cell In item.Cells
                    For Each ctl In cell.Controls
                        If TypeOf ctl Is CheckBox Then
                            chk1 = CType(ctl, CheckBox)
                            If chk1.Checked Then
                                Str = Str & " " & item.Cells(4).Text
                            End If
                        End If
                    Next
                Next
            Next
            Session("App_UebTexte") = Str

            If Session("App_FromPage").ToString = "UploadEdit03.aspx" Then
                Response.Redirect("UploadEdit03.aspx?AppID=" & Session("AppID").ToString)
            ElseIf Session("App_FromPage").ToString = "Ueberfg_02.aspx" Then
                Response.Redirect("Ueberfg_02.aspx?AppID=" & Session("AppID").ToString)
            End If



        Catch ex As Exception
            lblError.Text = "Fehler beim übernehmen der Standadtexte."
        End Try

    End Sub



    Private Sub Fillgrid(ByVal tblTexte As DataTable, ByVal sTemp As String, ByVal sTemp2 As String)
        Dim vwGr1 As DataView

        If tblTexte.Rows.Count > 0 Then
            vwGr1 = New DataView
            If Not sTemp2 = String.Empty AndAlso Not sTemp = String.Empty Then
                vwGr1 = tblTexte.DefaultView
                vwGr1.RowFilter = "Haupt_ID='" & sTemp & "' And Unter_ID='" & sTemp2 & "'"
            ElseIf sTemp2 = String.Empty AndAlso Not sTemp = String.Empty Then
                vwGr1 = tblTexte.DefaultView
                vwGr1.RowFilter = "Haupt_ID='" & sTemp & "'"
            End If

            dgSearchResult.DataSource = vwGr1
            dgSearchResult.DataBind()
            If vwGr1.Table.Rows.Count < 1 Then
                lbtnNew.Visible = False
                TRResult.Visible = True
                trEdit.Visible = True
                dgSearchResult.Visible = False
                txtDummy.Value = vwGr1.Table.Rows.Item(0).Item("ID")
                lblMessage.Text = "Es sind bisher noch keine Standardtexte angelegt!"
            End If
            'Else
            '    lbtnNew.Visible = False
            '    TRResult.Visible = True
            '    trEdit.Visible = True
            '    txtDummy.Value = "Neu"
            '    lblMessage.Text = "Es sind bisher noch keine Standardtexte angelegt!"
        End If
    End Sub
    Private Sub lbtnGruppe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        lbtnNew.Visible = False
        TRResult.Visible = False
        trEdit.Visible = False
        dgSearchResult.Visible = False
        trHauptgruppe.Visible = True
        'lbtnGruppe.Visible = False
    End Sub

    Private Sub IbtnSaveGroup_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim dTable As DataTable
        Dim dRow As DataRow
        Dim dTableSave As DataTable
        Dim clsTexte As New UeberfgStandard_01(m_User, m_App, "")

        If txtGruppe.Text = String.Empty AndAlso txtUGruppe.Text = String.Empty Then
            lblMessage.Text = "Bitte Geben Sie eine Gruppe sowie ein Untergruppe ein!"
            Exit Sub
        End If

        If txtGruppe.Text = String.Empty Then
            lblMessage.Text = "Bitte Geben Sie eine Gruppe ein!"
            Exit Sub
        End If
        If txtGruppe.Text = String.Empty AndAlso txtUGruppe.Text = String.Empty Then
            lblMessage.Text = "Bitte Geben Sie eine Untergruppe ein!"
            Exit Sub
        End If
        dTable = Session("App_Standardtext")
        dRow = dTable.NewRow()
        dRow("TEXT") = ""
        dRow("Sort1") = txtGruppe.Text.Trim
        dRow("Sort2") = txtUGruppe.Text
        dRow("ID") = dTable.Rows.Count + 1
        dTable.Rows.Add(dRow)
        dTableSave = dTable.Copy
        'clsTexte.saveStdTexte(dTableSave)
        Session("App_Standardtext") = dTable
        GetTexte()

        lbtnNew.Visible = True
        TRResult.Visible = True
        trEdit.Visible = True
        dgSearchResult.Visible = True
        trlvGruppe.Visible = True
        trHauptgruppe.Visible = False
        'lbtnGruppe.Visible = True
    End Sub


    Private Sub lvGruppe_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvGruppe.SelectedIndexChanged
        Dim tblTexte As DataTable
        Dim tblUnter As DataTable
        Dim sTemp As String = ""
        Dim sTemp2 As String = ""

        If Not Session("App_Standardtext") Is Nothing Then
            tblTexte = Session("App_Standardtext")
            tblUnter = Session("App_tblUnter")
            If lvGruppe.SelectedIndex >= 0 Then
                sTemp = lvGruppe.SelectedItem.Value
            End If

            FillUnterGruppe(tblUnter, sTemp)
            If lvUntergruppe.SelectedIndex >= 0 Then
                sTemp2 = lvUntergruppe.SelectedItem.Value
            End If
            invisibleControls()
            Fillgrid(tblTexte, sTemp, sTemp2)
        End If
    End Sub


    Private Sub lbtnUGruppe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        lbtnNew.Visible = False
        TRResult.Visible = False
        trEdit.Visible = False
        dgSearchResult.Visible = False
        trUntergruppe.Visible = True
        trHauptgruppe.Visible = True
        txtGruppe.Enabled = True
        'lbtnGruppe.Visible = False
    End Sub

    Private Sub lbtnEditGruppe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnEditGruppe.Click
        If Not IsNothing(lvGruppe.SelectedItem) Then
            lbtnNew.Visible = False
            TRResult.Visible = False
            lbtnDelete.Visible = True
            trEdit.Visible = False
            trHauptgruppe.Visible = True
            trUntergruppe.Visible = False
            txtUGruppe.Text = ""
            txtUnterID.Value = ""
            trButtons.Visible = True
            dgSearchResult.Visible = False
            txtGruppe.Enabled = True
            txtGruppe.Text = lvGruppe.SelectedItem.Text
            txtHauptID.Value = lvGruppe.SelectedItem.Value
            txtSave.Value = "H2"
        End If
    End Sub

    Private Sub lbtnNeuGruppe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNeuGruppe.Click

        If Not IsNothing(lvGruppe.SelectedItem) Then lvGruppe.SelectedItem.Selected = False
        trHauptgruppe.Visible = True
        trButtons.Visible = True
        txtGruppe.Enabled = True
        lbtnSaveGruppe.Visible = True
        txtGruppe.Text = ""
        txtHauptID.Value = ""
        txtSave.Value = "H1"
        trUntergruppe.Visible = False
        txtUGruppe.Text = ""
        txtUnterID.Value = ""
        lbtnDelete.Visible = False

    End Sub

    Private Sub lbtnEditUGruppe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnEditUGruppe.Click
        If Not IsNothing(lvUntergruppe.SelectedItem) Then
            trUntergruppe.Visible = True
            trHauptgruppe.Visible = True
            lbtnDelete.Visible = True
            trButtons.Visible = True
            txtGruppe.Text = lvGruppe.SelectedItem.Text
            txtHauptID.Value = lvGruppe.SelectedItem.Value
            txtGruppe.Enabled = False
            txtUGruppe.Text = lvUntergruppe.SelectedItem.Text
            txtUnterID.Value = lvUntergruppe.SelectedItem.Value
            txtSave.Value = "U2"
        End If
    End Sub

    Private Sub lbtnGruppen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnGruppen.Click
        TRResult.Visible = False
        lbuttons.Visible = True
        trlvGruppelabel.Visible = True
        trlvGruppe.Visible = True
        trHauptgruppe.Visible = False
        trUntergruppe.Visible = False
        trStdTextName.Visible = False
        trEdit.Visible = False
        lbtnNew.Visible = False
        trButtons.Visible = False
        Session.Remove("AppRowUnter")
        Session.Remove("AppRowHaupt")
    End Sub

    Private Sub lbtnNeuUGruppe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnNeuUGruppe.Click

        If Not IsNothing(lvGruppe.SelectedItem) Then
            txtGruppe.Text = lvGruppe.SelectedItem.Text
            trUntergruppe.Visible = True
            trHauptgruppe.Visible = True
            trButtons.Visible = True
            txtGruppe.Enabled = False
            txtUGruppe.Text = ""
            lbtnDelete.Visible = False
            txtSave.Value = "U1"
        Else
            lblMessage.Text = "Bitte legen Sie zuerst eine Hauptgruppe an!"
        End If


    End Sub

    Private Sub lbtnSaveGruppe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnSaveGruppe.Click
        Dim dTableTexte As DataTable
        Dim dTableHaupt As DataTable
        Dim dTableUnter As DataTable
        Dim vwGr1 As DataView
        Dim dRow As DataRow
        Dim iID As Integer
        Dim sID As String = ""
        Dim clsTexte As New UeberfgStandard_01(m_User, m_App, "")

        Try
            dTableTexte = Session("App_Standardtext")
            dTableHaupt = Session("App_tblHaupt")
            dTableUnter = Session("App_tblUnter")
            If txtSave.Value = "H1" Then
                vwGr1 = dTableHaupt.DefaultView
                vwGr1.RowFilter = "Haupt_ID>='0000000000'"
                vwGr1.Sort = "Haupt_ID"
                If vwGr1.Table.Rows.Count > 0 Then
                    iID = vwGr1.Table.Rows(vwGr1.Table.Rows.Count - 1).Item("Haupt_ID")
                End If
                iID = iID + 1
                sID = Right("0000000000" & iID, 10)
                dRow = dTableHaupt.NewRow()
                dRow("Name") = txtGruppe.Text
                dRow("Haupt_ID") = sID
                dTableHaupt.Rows.Add(dRow)
                Session("App_tblHaupt") = dTableHaupt
            ElseIf txtSave.Value = "H2" Then
                For Each dRow In dTableHaupt.Rows
                    If txtHauptID.Value = dRow("Haupt_ID") Then
                        dRow.BeginEdit()
                        dRow("Name") = txtGruppe.Text
                        dRow.EndEdit()
                        dRow.AcceptChanges()
                        Exit For
                    End If
                Next
                Session("App_tblHaupt") = dTableHaupt
                trButtons.Visible = False
                trHauptgruppe.Visible = False
                trUntergruppe.Visible = False
            End If

            If txtSave.Value = "U1" Then
                vwGr1 = dTableUnter.DefaultView
                vwGr1.RowFilter = "Unter_ID>='0000000000' AND Haupt_ID='" & lvGruppe.SelectedItem.Value & "'"
                vwGr1.Sort = "Unter_ID"
                If vwGr1.Table.Rows.Count > 0 Then
                    iID = vwGr1.Table.Rows(vwGr1.Table.Rows.Count - 1).Item("Unter_ID")
                End If
                iID = iID + 1
                sID = Right("0000000000" & iID, 10)
                dRow = dTableUnter.NewRow()
                dRow("Name") = txtUGruppe.Text
                dRow("Unter_ID") = sID
                dRow("Haupt_ID") = lvGruppe.SelectedItem.Value
                dTableUnter.Rows.Add(dRow)
                Session("App_tblUnter") = dTableUnter
            ElseIf txtSave.Value = "U2" Then
                dRow = dTableUnter.Select("Unter_ID='" & txtUnterID.Value & "' and Haupt_ID='" & txtHauptID.Value & "'")(0)
                dRow.BeginEdit()
                dRow("Name") = txtUGruppe.Text
                dRow.EndEdit()
                dRow.AcceptChanges()
                Session("App_tblUnter") = dTableUnter
            End If
            If txtSave.Value = "T1" Then
                If txtStdTextName.Text = String.Empty Then Throw New Exception("Bitte geben Sie einen Namen für Text ein!")
                If txtBemerkung.Text = String.Empty Then Throw New Exception("Bitte geben Sie einen Text ein!")
                If IsNumeric(txtDummy.Value) Then
                    dRow = dTableTexte.Select("TEXT_ID='" & txtDummy.Value & "' and Unter_ID='" & txtUnterID.Value & "' and Haupt_ID='" & txtHauptID.Value & "'")(0)
                    dRow.BeginEdit()
                    dRow("TEXT") = txtBemerkung.Text
                    dRow("Name") = txtStdTextName.Text
                    dRow.EndEdit()
                    dRow.AcceptChanges()
                End If
            ElseIf txtSave.Value = "T2" Then
                If txtStdTextName.Text = String.Empty Then Throw New Exception("Bitte geben Sie einen Namen für Text ein!")
                If txtBemerkung.Text = String.Empty Then Throw New Exception("Bitte geben Sie einen Text ein!")
                vwGr1 = dTableTexte.DefaultView
                vwGr1.RowFilter = "Text_ID>='0000000000'"
                vwGr1.Sort = "Text_ID"
                If vwGr1.Table.Rows.Count > 0 Then
                    iID = vwGr1.Table.Rows(vwGr1.Table.Rows.Count - 1).Item("Text_ID")
                End If
                iID = iID + 1
                sID = Right("0000000000" & iID, 10)
                dRow = dTableTexte.NewRow()
                dRow("TEXT") = txtBemerkung.Text
                dRow("Name") = txtStdTextName.Text
                dRow("TEXT_ID") = sID
                dRow("Haupt_ID") = txtHauptID.Value
                dRow("Unter_ID") = txtUnterID.Value
                dTableTexte.Rows.Add(dRow)
                lbtnNew.Visible = True
            End If
            clsTexte.saveStdTexte(dTableTexte, dTableHaupt, dTableUnter)
            If Not lvGruppe.SelectedIndex = -1 Then Session("AppRowHaupt") = lvGruppe.SelectedIndex
            If Not lvUntergruppe.SelectedIndex = -1 Then Session("AppRowUnter") = lvUntergruppe.SelectedIndex
            GetTexte()
            trButtons.Visible = False
            trEdit.Visible = False
            trStdTextName.Visible = False
            trHauptgruppe.Visible = False
            trUntergruppe.Visible = False

        Catch ex As Exception
            lblError.Text = "Fehler beim Speichern der Daten! " & ex.Message
        End Try
    End Sub

    Private Sub lbtnTexte_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnTexte.Click
        trlvGruppelabel.Visible = True
        trlvGruppe.Visible = True
        TRResult.Visible = True
        dgSearchResult.Visible = True
        lbuttons.Visible = False
        trStdTextName.Visible = False
        trEdit.Visible = False
        trButtons.Visible = False
        trUntergruppe.Visible = False
        lbtnNew.Visible = True
        trHauptgruppe.Visible = False
        trUntergruppe.Visible = False
        Session.Remove("AppRowUnter")
        Session.Remove("AppRowHaupt")
    End Sub

    Private Sub lvUntergruppe_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvUntergruppe.SelectedIndexChanged
        Dim tblTexte As DataTable
        Dim sTemp As String = ""
        Dim sTemp2 As String = ""

        If Not Session("App_Standardtext") Is Nothing Then
            tblTexte = Session("App_Standardtext")

            If lvGruppe.SelectedIndex >= 0 Then
                sTemp = lvGruppe.SelectedItem.Value
            End If

            If lvUntergruppe.SelectedIndex >= 0 Then
                sTemp2 = lvUntergruppe.SelectedItem.Value
            End If

            invisibleControls()

            Fillgrid(tblTexte, sTemp, sTemp2)
        End If
    End Sub
    Private Sub invisibleControls()
        trEdit.Visible = False
        trHauptgruppe.Visible = False
        trUntergruppe.Visible = False
        trButtons.Visible = False
        trStdTextName.Visible = False
        txtBemerkung.Text = ""
        txtDummy.Value = ""
        txtHauptID.Value = ""
        txtGruppe.Text = ""
        txtUnterID.Value = ""
        txtUGruppe.Text = ""
        txtVorschau.Visible = False
        txtVorschau.Text = ""
    End Sub

    Private Sub dgSearchResult_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgSearchResult.ItemCommand
        Dim dRow As DataRow
        Dim dTable As DataTable
        Dim dTableHaupt As DataTable
        Dim dTableUnter As DataTable
        Dim sID As String
        Dim sHauptID As String
        Dim sUnterID As String
        Dim clsTexte As New UeberfgStandard_01(m_User, m_App, "")

        Try
            If e.CommandName = "Edit" Then
                trStdTextName.Visible = True
                trEdit.Visible = True
                trButtons.Visible = True
                txtDummy.Value = e.Item.Cells(0).Text
                txtBemerkung.Text = e.Item.Cells(4).Text
                txtStdTextName.Text = e.Item.Cells(3).Text
                lbtnDelete.Visible = False
                lbtnNew.Visible = False
                trStdTextName.Visible = True
                trEdit.Visible = True
                trButtons.Visible = True
                txtHauptID.Value = lvGruppe.SelectedItem.Value
                If Not IsNothing(lvUntergruppe.SelectedItem) Then
                    txtUnterID.Value = lvUntergruppe.SelectedItem.Value
                Else
                    txtUnterID.Value = "0000000000"
                End If
                txtSave.Value = "T1"
            ElseIf e.CommandName = "Delete" Then
                dTable = Session("App_Standardtext")
                dTableHaupt = Session("App_tblHaupt")
                dTableUnter = Session("App_tblUnter")
                sID = e.Item.Cells(0).Text
                sHauptID = e.Item.Cells(1).Text
                sUnterID = e.Item.Cells(2).Text

                dRow = dTable.Select("TEXT_ID='" & sID & "' and Haupt_ID='" & sHauptID & "' And Unter_ID ='" & sUnterID & "'")(0)

                dTable.Rows.Remove(dRow)
                clsTexte.saveStdTexte(dTable, dTableHaupt, dTableUnter)
                Session("App_Standardtext") = dTable
                dgSearchResult.DataSource = dTable
                dgSearchResult.DataBind()
                txtBemerkung.Text = ""
                txtStdTextName.Text = ""
            ElseIf e.CommandName = "Vorschau" Then
                txtVorschau.Visible = True
                txtVorschau.Text = e.Item.Cells(4).Text
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub lbtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnDelete.Click
        Dim dRow As DataRow
        Dim dTable As DataTable
        Dim dTableHaupt As DataTable
        Dim dTableUnter As DataTable
        Dim sHauptID As String
        Dim sUnterID As String
        Dim clsTexte As New UeberfgStandard_01(m_User, m_App, "")

        If txtHauptID.Value <> String.Empty AndAlso txtUnterID.Value = String.Empty Then
            dTable = Session("App_Standardtext")
            dTableHaupt = Session("App_tblHaupt")
            dTableUnter = Session("App_tblUnter")
            sHauptID = txtHauptID.Value
            dRow = dTableHaupt.Select("Haupt_ID='" & sHauptID & "'")(0)
            dTableHaupt.Rows.Remove(dRow)
            If dTable.Rows.Count > 0 Then
                dRow = dTable.Select("Haupt_ID='" & sHauptID & "'")(0)
                dTable.Rows.Remove(dRow)
            End If
            If dTableUnter.Rows.Count > 0 Then
                dRow = dTableUnter.Select("Haupt_ID='" & sHauptID & "'")(0)
                dTableUnter.Rows.Remove(dRow)
            End If
            clsTexte.saveStdTexte(dTable, dTableHaupt, dTableUnter)
            Session("App_Standardtext") = dTable
            Session("App_tblUnter") = dTableUnter
            Session("App_tblHaupt") = dTableHaupt
        ElseIf txtHauptID.Value <> String.Empty AndAlso txtUnterID.Value <> String.Empty Then
            dTable = Session("App_Standardtext")
            dTableHaupt = Session("App_tblHaupt")
            dTableUnter = Session("App_tblUnter")
            sHauptID = txtHauptID.Value
            sUnterID = txtUnterID.Value
            If dTable.Rows.Count > 0 Then
                dRow = dTable.Select("Haupt_ID='" & sHauptID & "' AND Unter_ID='" & sUnterID & "'")(0)
                dTable.Rows.Remove(dRow)
            End If
            dRow = dTableUnter.Select("Haupt_ID='" & sHauptID & "' AND Unter_ID='" & sUnterID & "'")(0)
            dTableUnter.Rows.Remove(dRow)
            clsTexte.saveStdTexte(dTable, dTableHaupt, dTableUnter)
            Session("App_Standardtext") = dTable
            Session("App_tblUnter") = dTableUnter
            Session("App_tblHaupt") = dTableHaupt

        End If
        GetTexte()
        trButtons.Visible = False
        trEdit.Visible = False
        trStdTextName.Visible = False
        trHauptgruppe.Visible = False
        trUntergruppe.Visible = False
    End Sub

    Private Sub lbtnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnBack.Click
        If Session("App_FromPage").ToString = "UploadEdit03.aspx" Then
            Response.Redirect("UploadEdit03.aspx?AppID=" & Session("AppID").ToString)
        ElseIf Session("App_FromPage").ToString = "Ueberfg_02.aspx" Then
            Response.Redirect("Ueberfg_02.aspx?AppID=" & Session("AppID").ToString)
        End If
    End Sub
End Class
