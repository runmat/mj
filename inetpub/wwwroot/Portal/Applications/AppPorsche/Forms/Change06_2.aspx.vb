Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Imports System
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization


Public Class Change06_2
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

    Private m_context As HttpContext = HttpContext.Current
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As Search
    'Private objChange01_objFDDBank As BankBaseCredit
    Private objPorsche As Porsche_01
    Private objHaendler As Porsche_05
    Private m_strInitiator As String

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents FocusScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblScript As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdReset As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents cmdAuthorisize As System.Web.UI.WebControls.LinkButton
    'Protected WithEvents cmdBack2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerName As System.Web.UI.WebControls.Label
    Protected WithEvents lblAdresse As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblPageTitle.Text = "Werte ändern"
        lnkKreditlimit.NavigateUrl = "Change06.aspx?AppID=" & Session("AppID").ToString

        Try
            m_User = GetUser(Me)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)

            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                If Session("objFDDBank") Is Nothing Then
                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten."
                Else
                    'Händleradresse suchen...
                    objPorsche = CType(Session("objFDDBank"), Porsche_01)
                    objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                    Session("objSuche") = objSuche

                    If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, objPorsche.PHaendler) Then
                        lblHaendlerNummer.Text = objSuche.REFERENZ
                        lblHaendlerName.Text = objSuche.NAME
                        If objSuche.NAME_2.Length > 0 Then
                            lblHaendlerName.Text &= "<br>" & objSuche.NAME_2
                        End If
                        lblAdresse.Text = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
                    Else
                        lblError.Text = "Fehler bei der Ermittlung der Händlerdaten."
                    End If
                    lnkKreditlimit.NavigateUrl = "Change06.aspx?AppID=" & Session("AppID").ToString & "&ID=" & Request.QueryString("ID")
                    lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                    ucStyles.TitleText = lblHead.Text

                    'Kontingentdaten
                    objHaendler = New Porsche_05(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", "60" & objPorsche.PHaendler, Page:=Me.Page)
                    objHaendler.Customer = "60" & objPorsche.PHaendler
                    Session("objHaendler") = objHaendler

                    If objHaendler.Status = 0 Then
                        StartLoadData()
                    Else
                        lblError.Text = "Fehler bei der Ermittlung der Kontingentdaten."
                    End If
                End If
            End If
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change06_2", "Page_Load", ex.ToString)

            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub FillGrid()
        DataGrid1.DataSource = objHaendler.Kontingente
        DataGrid1.DataBind()
    End Sub

    Private Sub StartLoadData()
        FocusScript.Visible = True

        cmdSave.Visible = True
        cmdReset.Visible = False

        If (objHaendler.Kontingente Is Nothing) OrElse (objHaendler.Kontingente.Rows.Count = 0) Then
            lblError.Text = "Fehler: Es konnten keine Kontingentdaten ermittelt werden."
            FocusScript.Visible = False
            lblError.CssClass = "TextError"
        Else
            lblError.CssClass = "LabelExtraLarge"
            FillGrid()
        End If
    End Sub

    Private Sub disableGrid()
        Dim item As DataGridItem
        Dim blnGesperrtAlt As Boolean
        Dim blnGesperrtNeu As Boolean
        Dim cbxGesperrtAlt As CheckBox
        Dim cbxGesperrtNeu As CheckBox
        Dim row As DataRow

        For Each item In DataGrid1.Items
            cbxGesperrtAlt = CType(item.Cells(5).FindControl("Gesperrt_Alt"), CheckBox)
            cbxGesperrtNeu = CType(item.Cells(6).FindControl("chkGesperrt_Neu"), CheckBox)

            blnGesperrtAlt = cbxGesperrtAlt.Checked
            blnGesperrtNeu = cbxGesperrtNeu.Checked

            row = objHaendler.Kontingente.Select("Kreditkontrollbereich=" & item.Cells(0).Text)(0)

            cbxGesperrtAlt.Enabled = False
            cbxGesperrtNeu.Enabled = False

            If blnGesperrtAlt <> blnGesperrtNeu Then
                item.Cells(5).BackColor = System.Drawing.Color.FromArgb(200, 255, 200)
                item.Cells(6).BackColor = System.Drawing.Color.FromArgb(200, 255, 200)
                row("Gesperrt_Alt") = blnGesperrtAlt
                row("Gesperrt_Neu") = blnGesperrtNeu
                objHaendler.Kontingente.AcceptChanges()
            End If
        Next
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Dim objChange As Porsche_06c

        If (cmdReset.Visible = False) Then
            GetChanges()
            FillGrid()
            disableGrid()
            cmdReset.Visible = True
            lblError.Text = "Zum endgültigen Absenden der Änderungen klicken Sie bitte erneut auf 'Sichern'."
        Else
            objHaendler = CType(Session("objHaendler"), Porsche_05)

            objChange = New Porsche_06c(m_User, m_App, Session("AppID").ToString, Session.SessionID, "")
            objChange.PKontingente = objHaendler.Kontingente
            objChange.Customer = objHaendler.Customer
            objChange.Change(Me)
            If objChange.Status <> 0 Then
                lblError.Text = objChange.Message
            Else
                lblError.Text = "Werte erfolgreich gespeichert."
                disableGrid()
                cmdReset.Enabled = False
                cmdSave.Enabled = False
            End If
        End If
    End Sub

    Private Sub cmdReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReset.Click
        objHaendler = CType(Session("objHaendler"), Porsche_05)

        'objHaendler.ShowStandard()
        StartLoadData()
    End Sub

    Private Sub GetChanges()
        Dim item As DataGridItem
        Dim cbxGesperrtAlt As CheckBox
        Dim cbxGesperrtNeu As CheckBox
        Dim blnGesperrtAlt As Boolean
        Dim blnGesperrtNeu As Boolean
        Dim row As DataRow

        objHaendler = Session("objHaendler")

        For Each item In DataGrid1.Items
            cbxGesperrtAlt = CType(item.Cells(5).FindControl("Gesperrt_Alt"), CheckBox)
            cbxGesperrtNeu = CType(item.Cells(6).FindControl("chkGesperrt_Neu"), CheckBox)

            blnGesperrtAlt = cbxGesperrtAlt.Checked
            blnGesperrtNeu = cbxGesperrtNeu.Checked

            row = objHaendler.Kontingente.Select("Kreditkontrollbereich=" & item.Cells(0).Text)(0)

            If blnGesperrtAlt <> blnGesperrtNeu Then
                row("Gesperrt_Alt") = blnGesperrtAlt
                row("Gesperrt_Neu") = blnGesperrtNeu
                objHaendler.Kontingente.AcceptChanges()
            End If
        Next
        Session("objHaendler") = objHaendler
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Response.Redirect("Change06.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Change14'")(0).Item("AppID")), False)
        Catch
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change06_2.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:47
' Updated in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 11.03.10   Time: 14:42
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 10.03.10   Time: 15:53
' Updated in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 3.07.09    Time: 15:37
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA 2918 Z_M_BRIEFANFORDERUNG_PORSCHE, Z_M_BRIEF_OHNE_DATEN_PORSCHE,
' Z_M_CREDITLIMIT_CHANGE_PORSCHE, Z_M_CUST_GET_CHILDREN_PORSCHE,
' Z_M_DATEN_OHNE_BRIEF_PORSCHE, Z_M_FREIGEBEN_AUFTRAG_PORSCHE,
' Z_M_GESPERRT_AUFTRAG_PORSCHE, Z_M_HAENDLERBESTAND_PORSCHE
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 9:21
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:28
' Created in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 13:27
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 5.03.07    Time: 12:50
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' ************************************************
