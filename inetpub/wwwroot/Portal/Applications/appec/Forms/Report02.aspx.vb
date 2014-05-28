Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report02
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents lblInputReq As System.Web.UI.WebControls.Label
    Protected WithEvents btnVon As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private m_User As Base.Kernel.Security.User
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents txt_ErfassungsdatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents txt_ErfassungsdatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Bis As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ddl_Hersteller As System.Web.UI.WebControls.DropDownList
    Private m_App As Base.Kernel.Security.App
    Dim m_Report As ec_04

    Private errorText As String

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
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)


            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

            If Session("Report") Is Nothing Then
                m_Report = New ec_04(m_User, m_App, strFileName)
            Else
                m_Report = CType(Session("Report"), ec_04)
            End If

            If ddl_Hersteller.Items.Count <= 0 Then
                fillHerstellerddl()
            End If


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub fillHerstellerddl()

        If m_Report Is Nothing Then
            m_Report = CType(Session("Report"), ec_04)
        End If

        m_Report.FillHersteller(Session("AppID").ToString, Session.SessionID.ToString, Me)

        Dim row As DataRow
        row = m_Report.Result.NewRow()
        Dim item As ListItem

        item = New ListItem("keine Auswahl", "")
        ddl_Hersteller.Items.Add(item)

        For Each row In m_Report.Result.Rows
            item = New ListItem(row(1), row(0))
            ddl_Hersteller.Items.Add(item)
        Next

    End Sub

    Private Sub DoSubmit()
        Dim checkInput As Boolean = True
        Session("lnkExcel") = ""
        Try

            If m_Report Is Nothing Then
                m_Report = CType(Session("Report"), ec_04)
            End If

            Dim strHerstellerID As String = ""

            If ddl_Hersteller.SelectedIndex > 0 Then
                strHerstellerID = ddl_Hersteller.SelectedItem.Value.ToString()
            End If

            errorText = ""
            lblError.Text = ""

            checkInput = HelpProcedures.checkDate(txt_ErfassungsdatumVon, txt_ErfassungsdatumBis, errorText, False)

            'weil checkDate leider nur Jahresbedingte Zeitspannen erlaubt, und diese auch schlecht änderbar ist, weil Sie von anderen Reports bereits verwendet wird JJ2007.11.14
            If checkInput = True Then
                If Not DateDiff(DateInterval.DayOfYear, CDate(txt_ErfassungsdatumVon.Text), CDate(txt_ErfassungsdatumBis.Text)) < 31 Then
                    checkInput = False
                    errorText = "Die Zeitspanne darf maximal 31 Tage betragen"
                End If
            End If
            lblError.Text = errorText

            If checkInput Then
                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, txt_ErfassungsdatumVon.Text, txt_ErfassungsdatumBis.Text, strHerstellerID, Me)

                Session("ResultTable") = m_Report.Result

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else
                        Try
                            Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & m_Report.FileName)
                        Catch ex As Exception
                            Throw New Exception("Excel-Datei konnte nicht erzeugt werden.")
                        End Try

                        Dim AbsoluterPfadZumVirituellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
                        Session("lnkExcel") = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirituellenVerz, "") & m_Report.FileName & "".Replace("/", "\")
                        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub btnVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVon.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub



    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txt_ErfassungsdatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txt_ErfassungsdatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub



    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lb_Bis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lb_Bis.Click
        calBis.Visible = True
        calVon.Visible = False
    End Sub
End Class

' ************************************************
' $History: Report02.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 26.06.09   Time: 14:53
' Updated in $/CKAG/Applications/appec/Forms
' ITA 2918, Z_M_Ec_Avm_Zulauf, Z_V_FAHRZEUG_STATUS_001,
' Z_M_EC_AVM_STATUS_BESTAND, Z_M_ABMBEREIT_LAUFAEN,
' Z_M_ABMBEREIT_LAUFZEIT, Z_M_Brief_Temp_Vers_Mahn_001,
' Z_M_SCHLUE_SET_MAHNSP_001, Z_M_SCHLUESSELDIFFERENZEN,
' Z_M_SCHLUESSELVERLOREN, Z_M_SCHLUE_TEMP_VERS_MAHN_001,
' Z_M_Ec_Avm_Status_Zul,  Z_M_ECA_TAB_BESTAND
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.06.09   Time: 16:04
' Updated in $/CKAG/Applications/appec/Forms
' ITA 2918 Z_M_Herstellergroup, Z_M_EC_AVM_BATCH_update,
' Z_M_EC_AVM_HERST_VWZWECK_MODID, Z_M_EC_AVM_BATCH_INSERT,
' Z_M_EC_AVM_BATCH_ANSICHT
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
' *****************  Version 14  *****************
' User: Jungj        Date: 21.12.07   Time: 15:06
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' batchreporting ITA 1358
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 14.12.07   Time: 13:45
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Anpassung Excel Links, wegen Webconfig Änderung, jetzt Variabel ab
' Virtuellem Verzeichnis
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 21.11.07   Time: 9:34
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 21.11.07   Time: 9:24
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 20.11.07   Time: 15:51
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 14.11.07   Time: 14:07
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 14.11.07   Time: 14:07
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 23.05.07   Time: 9:47
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 22.05.07   Time: 13:31
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
