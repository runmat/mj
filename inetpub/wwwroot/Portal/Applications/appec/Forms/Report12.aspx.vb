Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report12
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist f�r den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist f�r den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtPDI As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtZulassungsdatumVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtZulassungsdatumBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents btnCal1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LinkButton1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents lblInputReq As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblError As System.Web.UI.WebControls.Label

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

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim checkInput As Boolean = True
        Dim checkDate As Boolean = True
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New ec_12(m_User, m_App, strFileName)
            Dim datZulassungsdatumVon As DateTime
            Dim datZulassungsdatumBis As DateTime

            lblError.Text = ""

            If (txtZulassungsdatumVon.Text.Trim = String.Empty) And (txtZulassungsdatumBis.Text.Trim = String.Empty) Then
                'Datumsfelder leer oder falsches Format
                lblError.Text = "Bitte geben Sie ein g�ltiges Eingangsdatum (von,bis) ein!"
                checkInput = False
            End If
            'Datumsfelder sind gef�llt. Jetzt Format und Werte pr�fen.
            If checkInput Then
                Try
                    datZulassungsdatumVon = CDate(txtZulassungsdatumVon.Text)
                    datZulassungsdatumBis = CDate(txtZulassungsdatumBis.Text)

                    If (datZulassungsdatumVon > datZulassungsdatumBis) Then
                        checkInput = False
                        lblError.Text = "Eingangsdatum (von) mu� kleiner oder gleich Eingangsdatum (bis) sein!<br>"
                    End If

                Catch ex As Exception
                    lblError.Text = "Falsches Datumsformat."
                    checkInput = False
                End Try
            End If

            If checkInput Then
                m_Report.PPDI = txtPDI.Text.Trim
                m_Report.PEingangsdatumVon = datZulassungsdatumVon
                m_Report.PEingangsdatumBis = datZulassungsdatumBis

                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

                Session("ResultTable") = m_Report.Result

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse f�r die gew�hlten Kriterien."
                    Else

                        Try
                            Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Catch
                        End Try

                        Dim AbsoluterPfadZumVirituellenVerz As String = ConfigurationManager.AppSettings("ReplaceExcelPath")
                        Session("lnkExcel") = ConfigurationManager.AppSettings("ExcelPath").Replace(AbsoluterPfadZumVirituellenVerz, "") & strFileName & "".Replace("/", "\")
                        Response.Redirect("Report12_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub btnCal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal1.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub LinkButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtZulassungsdatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtZulassungsdatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report12.aspx.vb $
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
' *****************  Version 8  *****************
' User: Jungj        Date: 14.12.07   Time: 13:45
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Anpassung Excel Links, wegen Webconfig �nderung, jetzt Variabel ab
' Virtuellem Verzeichnis
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingef�hrt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 13:31
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
