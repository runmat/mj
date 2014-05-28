Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report24
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
    Private objSuche As Search
    Private objReport23_objFDDBank As Base.Business.BankBaseCredit
    Private objReport23_objFDDBank3 As FDD_Bank_3

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents txtOrdernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVertragsnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkAlle As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkAngefordert As System.Web.UI.WebControls.RadioButton
    Protected WithEvents chkNichtAngefordert As System.Web.UI.WebControls.RadioButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents Kopfdaten1 As Kopfdaten
    'Private AppID As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If (Session("objSuche") Is Nothing) Then
                objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)

                If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, m_User.Reference) Then
                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                    cmdSearch.Visible = False
                End If
            Else
                objSuche = CType(Session("objSuche"), Search)
            End If

            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

            Session("objSuche") = objSuche

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If Not IsPostBack Then
                objReport23_objFDDBank = New Base.Business.BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                objReport23_objFDDBank.Customer = "60" & objSuche.REFERENZ
                objReport23_objFDDBank.CreditControlArea = "ZDAD"
                objReport23_objFDDBank.Show()
                If objReport23_objFDDBank.Status = 0 Then
                    Kopfdaten1.Kontingente = objReport23_objFDDBank.Kontingente
                    Session("objReport23_objFDDBank") = objReport23_objFDDBank
                    cmdSearch.Enabled = True
                Else
                    lblError.Text = objReport23_objFDDBank.Message
                    cmdSearch.Enabled = False
                End If
            Else
                If Not Session("objReport23_objFDDBank") Is Nothing Then
                    objReport23_objFDDBank = CType(Session("objReport23_objFDDBank"), Base.Business.BankBaseCredit)
                    If objReport23_objFDDBank.Status = 0 Then
                        cmdSearch.Enabled = True
                    Else
                        cmdSearch.Enabled = False
                        lblError.Text = objReport23_objFDDBank.Message
                    End If
                Else
                    Response.Redirect("../../../Start/Selection.aspx")
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        lblError.Text = ""
        lblError.Visible = False
        lblNoData.Text = ""

        If objReport23_objFDDBank.Status = 0 Then

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            objReport23_objFDDBank3 = New FDD_Bank_3(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
            objReport23_objFDDBank3.Customer = "60" & objSuche.REFERENZ
            objReport23_objFDDBank3.CreditControlArea = "ZDAD"
            objReport23_objFDDBank3.FahrgestellnummerSuche = txtFahrgestellnummer.Text
            objReport23_objFDDBank3.Ordernummer = txtOrdernummer.Text
            objReport23_objFDDBank3.Vertragsnummer = txtVertragsnummer.Text
            If chkAlle.Checked Then
                objReport23_objFDDBank3.Vorgaenge = "alle"
            End If
            If chkAngefordert.Checked Then
                objReport23_objFDDBank3.Vorgaenge = "angefordert"
            End If
            If chkNichtAngefordert.Checked Then
                objReport23_objFDDBank3.Vorgaenge = "nichtangefordert"
            End If
            objReport23_objFDDBank3.Report(Session("AppID").ToString, Session.SessionID, Me)
            If Not objReport23_objFDDBank3.Fahrzeuge Is Nothing AndAlso objReport23_objFDDBank3.Fahrzeuge.Rows.Count > 0 Then
                Session("objReport23_objFDDBank3") = objReport23_objFDDBank3

                Try
                    Excel.ExcelExport.WriteExcel(objReport23_objFDDBank3.FahrzeugeExcel, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                Catch
                End Try
                Response.Redirect("Report23_3.aspx?AppID=" & Session("AppID").ToString)


            Else
                lblError.Text = "Zu den gewählten Kriterien wurden keine Fahrzeuge gefunden."
                lblError.Visible = True
            End If
        Else
            lblError.Text = objReport23_objFDDBank.Message
            lblError.Visible = True
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
' $History: Report24.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:34
' Updated in $/CKAG/Applications/appffd/Forms
' Rückgängig: Dynproxy-Zugriff
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 11.03.10   Time: 12:36
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 10.03.10   Time: 15:22
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 19.06.09   Time: 16:48
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Forms
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' 
' ************************************************
