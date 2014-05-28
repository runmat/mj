Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

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

    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ddlHersteller As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Private m_Hersteller As ec_03

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
                initialload()
            Else
                'postbackload()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten."
        End Try
    End Sub

    Private Sub initialload()
        'Hersteller-Tabelle lesen

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim tblHersteller As DataTable
        Dim tblHerstellerOutput As New DataTable()
        Dim rowNew As DataRow
        Dim rowExists As DataRow()
        Dim vwData As DataView
        Dim intLoop As Integer

        m_Hersteller = New ec_03(m_User, m_App, strFileName)
        m_Hersteller.getHersteller(Me.Page, Session("AppID"), Session.SessionID)

        tblHerstellerOutput.Columns.Add("DOMVALUE_L", GetType(System.String))
        tblHerstellerOutput.Columns.Add("View", GetType(System.String))

        If (m_Hersteller.Status = 0) Then
            tblHersteller = m_Hersteller.PHerstellerListe

            vwData = tblHersteller.DefaultView
            vwData.Sort = "ZHERST asc"

            rowNew = tblHerstellerOutput.NewRow
            rowNew("DOMVALUE_L") = "0"
            rowNew("View") = "-alle-"
            tblHerstellerOutput.Rows.Add(rowNew)



            For intLoop = 0 To vwData.Count - 1 ' tblHersteller.Rows
                rowExists = tblHerstellerOutput.Select("View='" & CType(vwData.Item(intLoop)("View"), String) & "'")
                If (rowExists.Length = 0) Then
                    'Zeile nicht vorhanden, einfügen
                    rowNew = tblHerstellerOutput.NewRow
                    rowNew("DOMVALUE_L") = CStr(vwData.Item(intLoop)("DOMVALUE_L"))
                    rowNew("View") = CStr(vwData.Item(intLoop)("View"))
                    tblHerstellerOutput.Rows.Add(rowNew)
                End If
            Next
            tblHerstellerOutput.AcceptChanges()

            With ddlHersteller
                .DataSource = tblHerstellerOutput
                .DataValueField = "DOMVALUE_L"
                .DataTextField = "View"
                .DataBind()
            End With
        Else
            lblError.Text = "Fehler beim Lesen der Halterdaten."
            cmdCreate.Enabled = False
        End If
        'm_Hersteller.ResultTable = tblHerstellerOutput
        'Session.Add("objSuche", m_Hersteller)
    End Sub

    'Private Sub postbackload()
    '    'Dim intIndex As Integer
    '    'm_Hersteller = CType(Session("objSuche"), ec_01)

    '    'intIndex = ddlHersteller.SelectedIndex

    '    'With ddlHersteller
    '    '    .DataSource = m_Hersteller.ResultTable
    '    '    .DataValueField = "VALPOS"
    '    '    .DataTextField = "ZHERST"
    '    '    .DataBind()
    '    'End With

    '    'ddlHersteller.Items(intIndex).Selected = True
    'End Sub

    'Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
    '    Session("ShowLink") = "True"
    '    DoSubmit()
    'End Sub

    Private Sub DoSubmit()
        Dim checkInput As Boolean = True
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New ec_03(m_User, m_App, strFileName)

            lblError.Text = ""

            If checkInput Then
                If ddlHersteller.SelectedItem.Text = "-alle-" Then
                    m_Report.PHersteller = String.Empty
                Else
                    m_Report.PHersteller = ddlHersteller.SelectedItem.Value
                End If

                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

                Session("ResultTable") = m_Report.Result

                If Not m_Report.Status = 0 Then
                    lblError.Text = m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else

                        Try
                            Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Catch
                        End Try

                        Dim AbsoluterPfadZumVirituellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
                        Session("lnkExcel") = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirituellenVerz, "") & strFileName & "".Replace("/", "\")

                        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
        End Try
    End Sub


    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
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
' User: Fassbenders  Date: 9.03.10    Time: 10:56
' Updated in $/CKAG/Applications/appec/Forms
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 26.06.09   Time: 11:29
' Updated in $/CKAG/Applications/appec/Forms
' ITA 2918
' Z_M_EC_AVM_BRIEFLEBENSLAUF,Z_M_Ec_Avm_Fzg_M_Dfs_O_Zul,Z_M_EC_AVM_FZG_OH
' NE_BRIEF,Z_M_Ec_Avm_Fzg_Ohne_Unitnr,Z_M_Ec_Avm_Nur_Brief_Vorh,
' Z_M_EC_AVM_OFFENE_ZAHLUNGEN,  Z_M_EC_AVM_PDI_BESTAND,
' Z_M_EC_AVM_STATUS_EINSTEUERUNG,  Z_M_EC_AVM_STATUS_GREENWAY,
' Z_M_Ec_Avm_Status_Zul, Z_M_EC_AVM_ZULASSUNGEN, Z_M_Ec_Avm_Zulassungen_2
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Forms
' Warnungen entfernt!
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 14.12.07   Time: 13:45
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Anpassung Excel Links, wegen Webconfig Änderung, jetzt Variabel ab
' Virtuellem Verzeichnis
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 23.05.07   Time: 9:47
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 8  *****************
' User: Uha          Date: 22.05.07   Time: 13:31
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 7  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
