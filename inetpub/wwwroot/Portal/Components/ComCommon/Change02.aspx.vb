Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.OleDb
Imports System.Data

Public Class Change02
    Inherits System.Web.UI.Page

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

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objHandler As Change_01

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Weiter As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lb_Confirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trWeiter As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trConfirm As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Region As System.Web.UI.WebControls.Label
    Protected WithEvents txtRegion As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_Wldat As System.Web.UI.WebControls.Label
    Protected WithEvents txtWldat As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnOpenSelectWldat As System.Web.UI.WebControls.Button
    Protected WithEvents calWldat As System.Web.UI.WebControls.Calendar
    Protected WithEvents tr_Region As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Wldat As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_Haendler As System.Web.UI.WebControls.Label
    Protected WithEvents ddlHaendler As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lbl_Dienstleistung As System.Web.UI.WebControls.Label
    Protected WithEvents rb_Buchpruefung As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_KoerperlichePruefung As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_Vollpruefung As System.Web.UI.WebControls.RadioButton
    Protected WithEvents tr_Haendler As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Dienstleistung As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents rb_Monat As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_Woche As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rb_Tag As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lb_Back As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trBack As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lb_New As System.Web.UI.WebControls.LinkButton
    Protected WithEvents trNew As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHaendlerShow As System.Web.UI.WebControls.Label
    Protected WithEvents lblRegionShow As System.Web.UI.WebControls.Label
    Protected WithEvents lblWldatShow As System.Web.UI.WebControls.Label
    Protected WithEvents Literal2 As System.Web.UI.WebControls.Literal
    Protected WithEvents Literal3 As System.Web.UI.WebControls.Literal
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If (Session("objHandler") Is Nothing) Then
            BuildObject()
        Else
            objHandler = CType(Session("objHandler"), Change_01)
        End If

        If Not IsPostBack Then
            Literal1.Text = "		<script language=""JavaScript"">" & vbCrLf
            Literal1.Text &= "			<!-- //" & vbCrLf
            Literal1.Text &= "			window.document.Form1.txtRegion.focus();" & vbCrLf
            Literal1.Text &= "			//-->" & vbCrLf
            Literal1.Text &= "		</script>" & vbCrLf

            InitButtons()
        Else
            Literal1.Text = ""
        End If
      
    End Sub

    Private Sub InitButtons()
        lb_Weiter.Visible = True
        lb_Confirm.Visible = False
        lb_Back.Visible = False
        lb_New.Visible = False
    End Sub

    Private Sub BuildObject()
        objHandler = New Change_01(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        objHandler.Show()

        Dim dvHaendler As DataView = objHandler.Result.DefaultView
        dvHaendler.Sort = "Name1"
        Dim tblHaendler As New DataTable()
        tblHaendler.Columns.Add("Kunnr", System.Type.GetType("System.String"))
        tblHaendler.Columns.Add("Beschreibung", System.Type.GetType("System.String"))
        Dim tmpRow As DataRow
        tmpRow = tblHaendler.NewRow
        tmpRow("Kunnr") = "-1"
        tmpRow("Beschreibung") = " - keine Auswahl - "
        tblHaendler.Rows.Add(tmpRow)
        Dim i As Integer
        For i = 0 To dvHaendler.Count - 1
            tmpRow = tblHaendler.NewRow
            tmpRow("Kunnr") = CStr(dvHaendler(i)("Kunnr"))
            tmpRow("Beschreibung") = CStr(dvHaendler(i)("Kunnr")) & " - " & CStr(dvHaendler(i)("Name1")) & " " & CStr(dvHaendler(i)("Name2")) & ", " & CStr(dvHaendler(i)("Pstlz")) & " " & CStr(dvHaendler(i)("Ort01"))
            tblHaendler.Rows.Add(tmpRow)
        Next
        ddlHaendler.DataSource = tblHaendler
        ddlHaendler.DataValueField = "Kunnr"
        ddlHaendler.DataTextField = "Beschreibung"
        ddlHaendler.DataBind()

        Session("objHandler") = objHandler
    End Sub

    Private Function CheckInput() As Change_02
        Dim objReturn As New Change_02(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        If Not ddlHaendler.SelectedItem.Value = "-1" Then
            objReturn.Kunnr_I = ddlHaendler.SelectedItem.Value
            objHandler.Result.DefaultView.RowFilter = "KUNNR = '" & ddlHaendler.SelectedItem.Value & "'"
            objReturn.Region = objHandler.Result.DefaultView.Item(0)("PSTLZ")
            objHandler.Result.DefaultView.RowFilter = ""
        Else
            objReturn.Kunnr_I = " "
            If txtRegion.Text.Trim(" "c).Length = 0 Then
                Throw New Exception("Wählen Sie einen Händler aus oder geben Sie eine Region ein.")
            Else
                If Not txtRegion.Text.Trim(" "c).Length = 5 Then
                    Throw New Exception("Die Region muss eine fünfstellige Zahl sein.")
                End If
                If Not IsNumeric(txtRegion.Text.Trim(" "c)) Then
                    Throw New Exception("Die Region muss eine fünfstellige Zahl sein.")
                End If
                objReturn.Region = txtRegion.Text.Trim(" "c)
            End If
        End If

        If rb_Buchpruefung.Checked Then
            objReturn.Dienstleistung = "B"
        ElseIf rb_KoerperlichePruefung.Checked Then
            objReturn.Dienstleistung = "K"
        ElseIf rb_Vollpruefung.Checked Then
            objReturn.Dienstleistung = "V"
        End If

        txtWldat.Text = txtWldat.Text.Trim(" "c)
        If txtWldat.Text.Length = 0 Then
            Throw New Exception("Bitte geben Sie ein Datum ein.")
        End If
        If rb_Tag.Checked Then
            If Not IsDate(txtWldat.Text) Then
                Throw New Exception("Datum hat ungültiges Format.")
            End If
            objReturn.Wldat_Typ = "T"
        ElseIf rb_Woche.Checked Then
            CheckWeekAndMonth()
            If CInt(Left(txtWldat.Text, 2)) < 1 Or CInt(Left(txtWldat.Text, 2)) > 53 Then
                Throw New Exception("Datum hat ungültiges Format.")
            End If
            objReturn.Wldat_Typ = "W"
        ElseIf rb_Monat.Checked Then
            CheckWeekAndMonth()
            If CInt(Left(txtWldat.Text, 2)) < 1 Or CInt(Left(txtWldat.Text, 2)) > 12 Then
                Throw New Exception("Datum hat ungültiges Format.")
            End If
            objReturn.Wldat_Typ = "M"
        End If

        objReturn.Wldat = txtWldat.Text

        Return objReturn
    End Function

    Private Sub DoSubmit()

        lb_Weiter.Visible = False
        lb_Confirm.Visible = False
        lb_Back.Visible = False
        lb_New.Visible = False

        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        Session.Add("logObj", logApp)

        lblError.Text = ""

        Dim objHandlerWrite As Change_02 = CheckInput()

        objHandlerWrite.Change()

        If Not objHandlerWrite.Status = 0 Then
            lblError.Text = objHandlerWrite.Message
            lblError.Visible = True
        Else
            lblError.Text = "Auftrag mit der Nummer """ & objHandlerWrite.Vbeln & """ wurde angelegt."
        End If

        lb_New.Visible = True
       
    End Sub

    Private Sub CheckWeekAndMonth()
        If Not Len(txtWldat.Text) = 7 Then
            Throw New Exception("Datum hat ungültiges Format.")
        End If
        If Not InStr(txtWldat.Text, ".") = 3 Then
            Throw New Exception("Datum hat ungültiges Format.")
        End If
        If Not IsNumeric(Left(txtWldat.Text, 2)) Then
            Throw New Exception("Datum hat ungültiges Format.")
        End If
        If Not IsNumeric(Right(txtWldat.Text, 4)) Then
            Throw New Exception("Datum hat ungültiges Format.")
        End If
        If CInt(Right(txtWldat.Text, 4)) < Now.Year Then
            Throw New Exception("Datum hat ungültiges Format.")
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        If lb_Weiter.Visible = True Then
            ddlHaendler.Visible = True
            lblHaendlerShow.Visible = False

            txtRegion.Visible = True
            lblRegionShow.Visible = False

            rb_Tag.Visible = True
            rb_Woche.Visible = True
            rb_Monat.Visible = True
            txtWldat.Visible = True
            btnOpenSelectWldat.Visible = True
            lblWldatShow.Visible = False

            rb_Buchpruefung.Visible = True
            rb_KoerperlichePruefung.Visible = True
            rb_Vollpruefung.Visible = True
            Literal2.Text = "<br>"
            Literal3.Text = "<br>"
        End If
        SetEndASPXAccess(Me)
        If lb_Weiter.Visible = False Then
            ddlHaendler.Visible = False
            lblHaendlerShow.Visible = True
            lblHaendlerShow.Text = ddlHaendler.SelectedItem.Text

            txtRegion.Visible = False
            lblRegionShow.Visible = True
            lblRegionShow.Text = txtRegion.Text

            rb_Tag.Visible = rb_Tag.Checked
            rb_Woche.Visible = rb_Woche.Checked
            rb_Monat.Visible = rb_Monat.Checked
            lblWldatShow.Visible = True
            lblWldatShow.Text = txtWldat.Text
            txtWldat.Visible = False
            btnOpenSelectWldat.Visible = False

            rb_Buchpruefung.Visible = rb_Buchpruefung.Checked
            rb_KoerperlichePruefung.Visible = rb_KoerperlichePruefung.Checked
            rb_Vollpruefung.Visible = rb_Vollpruefung.Checked
            Literal2.Text = ""
            Literal3.Text = ""
        End If
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lb_Weiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Weiter.Click

        Dim objHandlerWrite As Change_02 = CheckInput()

        lblError.Text = "Wollen Sie oben stehende Dienstleistung beauftragen?"
        lb_Weiter.Visible = False
        lb_Confirm.Visible = True
        lb_Back.Visible = True
        lb_New.Visible = False
       
    End Sub

    Private Sub lb_Confirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Confirm.Click
        DoSubmit()
    End Sub

    Private Sub btnOpenSelectWldat_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOpenSelectWldat.Click
        calWldat.Visible = True
    End Sub

    Private Sub calWldat_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calWldat.SelectionChanged
        calWldat.Visible = False
        txtWldat.Text = calWldat.SelectedDate.ToShortDateString
    End Sub

    Private Sub rb_Tag_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rb_Tag.CheckedChanged
        If rb_Tag.Checked Then
            btnOpenSelectWldat.Enabled = True
        End If
    End Sub

    Private Sub rb_Woche_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rb_Woche.CheckedChanged
        If rb_Woche.Checked Then
            btnOpenSelectWldat.Enabled = False
            calWldat.Visible = False
        End If
    End Sub

    Private Sub rb_Monat_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rb_Monat.CheckedChanged
        If rb_Monat.Checked Then
            btnOpenSelectWldat.Enabled = False
            calWldat.Visible = False
        End If
    End Sub

    Private Sub lb_Back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Back.Click
        InitButtons()
    End Sub

    Private Sub lb_New_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_New.Click
        InitButtons()
        txtRegion.Text = ""
        txtWldat.Text = ""
        ddlHaendler.ClearSelection()
        ddlHaendler.Items.FindByValue("-1").Selected = True
        rb_Tag.Checked = True
        rb_Woche.Checked = False
        rb_Monat.Checked = False
        rb_Buchpruefung.Checked = True
        rb_KoerperlichePruefung.Checked = False
        rb_Vollpruefung.Checked = False
    End Sub
End Class

' ************************************************
' $History: Change02.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 5.06.09    Time: 13:51
' Updated in $/CKAG/Components/ComCommon
' Try Catch entfernt wenn möglich
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/portal/Components/ComCommon
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 16.10.07   Time: 9:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' *****************  Version 4  *****************
' User: Uha          Date: 27.09.07   Time: 16:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1238 Bugfixing
' 
' *****************  Version 3  *****************
' User: Uha          Date: 27.09.07   Time: 10:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Controlnamen für Feldübersetzungen geändert
' 
' *****************  Version 2  *****************
' User: Uha          Date: 26.09.07   Time: 13:22
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Bugfixing in ITA 1237, 1181 und 1238 (Alle Floorcheck)
' 
' *****************  Version 1  *****************
' User: Uha          Date: 24.09.07   Time: 17:01
' Created in $/CKG/Components/ComCommon/ComCommonWeb
' ITA 1238: Anlage Floorcheckauftrag - Testversion
' 
' ************************************************
