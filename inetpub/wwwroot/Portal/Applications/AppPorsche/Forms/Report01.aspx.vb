Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report01
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

    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lstHaendler As System.Web.UI.WebControls.ListBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblTitel As System.Web.UI.WebControls.Label
    Protected WithEvents cbxAlle As System.Web.UI.WebControls.CheckBox
    Private objPorsche As Porsche_01

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                Initialload()
            Else

            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Refill()
        Dim vwHaendler As DataView
        objPorsche = CType(Session("objSuche"), Porsche_01)

        vwHaendler = objPorsche.PResult.DefaultView
        vwHaendler.Sort = "NAME1 asc"

        With lstHaendler
            .DataSource = vwHaendler
            .DataValueField = "KUNNR_ZF"
            .DataTextField = "Addresse"
            .DataBind()
        End With

        lstHaendler.Visible = True

        lstHaendler.Items.FindByValue(objPorsche.PSelection).Selected = True 'Eintrag auswählen
    End Sub

    Private Sub Initialload()
        Dim vwHaendler As DataView

        objPorsche = New Porsche_01(m_User, m_App, "")

        With objPorsche
            .PGueltigkeit = Now.ToShortDateString
            .PHierarchie = "A"
            .PKnotenlevel = "00"
            .PKundennr = Right("0000000000" & m_User.KUNNR, 10)
        End With

        objPorsche.getHaendler(m_User.Reference, Session("AppID").ToString, Session.SessionID, Me)

        If (objPorsche.Status <> 0) Then
            lblError.Text = objPorsche.Message
            lstHaendler.Visible = False
            lblTitel.Visible = False
            cmdCreate.Enabled = False
            Exit Sub
        End If

        If objPorsche.Result.Rows.Count = 0 Then
            lblError.Text = "Keine Händler gefunden."
            lstHaendler.Visible = False
            lblTitel.Visible = False
            cmdCreate.Enabled = False
            Exit Sub
        End If

        lstHaendler.Visible = True

        vwHaendler = objPorsche.Result.DefaultView
        vwHaendler.Sort = "NAME1 asc"

        With lstHaendler
            .DataSource = vwHaendler
            .DataValueField = "KUNNR_ZF"
            .DataTextField = "Addresse"
            .DataBind()
        End With

        lstHaendler.Items(0).Selected = True                        'Ersten Eintrag auswählen.

        objPorsche.PSelection = lstHaendler.SelectedItem.Value      '...und merken

        Session.Add("objSuche", objPorsche)
        'Wenn nur ein Händler gefunden wurde, gleich weiter....
        If (objPorsche.Result.Rows.Count = 1) Then
            DoSubmit()
        End If
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        objPorsche = CType(Session("objSuche"), Porsche_01)
        objPorsche.PSelection = lstHaendler.SelectedItem.Value
        objPorsche.PKundennr = CType(lstHaendler.SelectedItem.Value, String)
        objPorsche.PHaendler = m_User.Reference     'Händlernummer

        If (cbxAlle.Checked = True) Then
            objPorsche.PKundennr = String.Empty
        End If

        objPorsche.getBriefe(Session("AppID").ToString, Session.SessionID, Me)

        If (objPorsche.Status <> 0) Then
            lblError.Text = objPorsche.Message  'Fehler oder keine Daten vorhanden...
            Refill()
        Else
            If objPorsche.Result.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                'Ok, daten vorhanden...
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                Dim objExcelExport As New Excel.ExcelExport()

                Session("ResultTable") = objPorsche.Result

                Try
                    Excel.ExcelExport.WriteExcel(objPorsche.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                Catch
                End Try

                Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Händlerberstand")
                Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report01.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 3.07.09    Time: 15:37
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA 2918 Z_M_BRIEFANFORDERUNG_PORSCHE, Z_M_BRIEF_OHNE_DATEN_PORSCHE,
' Z_M_CREDITLIMIT_CHANGE_PORSCHE, Z_M_CUST_GET_CHILDREN_PORSCHE,
' Z_M_DATEN_OHNE_BRIEF_PORSCHE, Z_M_FREIGEBEN_AUFTRAG_PORSCHE,
' Z_M_GESPERRT_AUFTRAG_PORSCHE, Z_M_HAENDLERBESTAND_PORSCHE
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.05.09    Time: 9:21
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA: 2837
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:34
' Updated in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:28
' Created in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 12  *****************
' User: Uha          Date: 2.07.07    Time: 13:27
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 11  *****************
' User: Uha          Date: 23.05.07   Time: 9:15
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 10  *****************
' User: Uha          Date: 5.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' *****************  Version 9  *****************
' User: Uha          Date: 5.03.07    Time: 13:54
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' *****************  Version 8  *****************
' User: Uha          Date: 5.03.07    Time: 12:50
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' ************************************************
