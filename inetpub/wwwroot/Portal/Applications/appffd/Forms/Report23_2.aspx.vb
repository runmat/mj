Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
'Imports CKG.Base.Business.BankBaseCredit

Public Class Report23_2
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
    Private objReport23_objFDDBank As BankBaseCredit
    Private objReport23_objFDDBank3 As FDD_Bank_3

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        If (Session("objSuche") Is Nothing) OrElse Session("SelectedDealer").ToString.Length = 0 Then
            Response.Redirect("Report23.aspx?AppID=" & Session("AppID").ToString)
        Else
            objSuche = CType(Session("objSuche"), Search)
            If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
                Kopfdaten1.UserReferenz = m_User.Reference
                Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
                Dim strTemp As String = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    strTemp &= "<br>" & objSuche.NAME_2
                End If
                Kopfdaten1.HaendlerName = strTemp
                Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
            Else
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            End If
        End If
        lnkKreditlimit.NavigateUrl = "Report23.aspx?AppID=" & Session("AppID").ToString & "&Back=1"
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                objReport23_objFDDBank = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString)
                objReport23_objFDDBank.Customer = Session("SelectedDealer").ToString
                objReport23_objFDDBank.CreditControlArea = "ZDAD"
                objReport23_objFDDBank.Show()
                If objReport23_objFDDBank.Status = 0 Then
                    Kopfdaten1.Kontingente = objReport23_objFDDBank.Kontingente
                    Session("objReport23_objFDDBank") = objReport23_objFDDBank
                    cmdSearch.Enabled = True
                Else
                    cmdSearch.Enabled = False
                End If
            Else
                If Not Session("objReport23_objFDDBank") Is Nothing Then
                    objReport23_objFDDBank = CType(Session("objReport23_objFDDBank"), BankBaseCredit)
                    If objReport23_objFDDBank.Status = 0 Then
                        'Kopfdaten1.Kontingente = objReport23_objFDDBank.Kontingente
                        'Session("objReport23_objFDDBank") = objReport23_objFDDBank
                        cmdSearch.Enabled = True
                    Else
                        cmdSearch.Enabled = False
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

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        lblError.Text = ""
        lblError.Visible = False
        lblNoData.Text = ""

        If objReport23_objFDDBank.Status = 0 Then
            'Kopfdaten1.Kontingente = objReport23_objFDDBank.Kontingente

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            objReport23_objFDDBank3 = New FDD_Bank_3(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
            objReport23_objFDDBank3.Customer = "60" & Session("SelectedDealer").ToString
            objReport23_objFDDBank3.CreditControlArea = "ZDAD"
            Dim strTemp As String = Replace(txtFahrgestellnummer.Text, "%", "*")
            If strTemp Is Nothing Then
                strTemp = ""
            End If
            If strTemp.Length = 5 Then
                objReport23_objFDDBank3.FahrgestellnummerSuche = "*" & strTemp
            Else
                objReport23_objFDDBank3.FahrgestellnummerSuche = strTemp
            End If
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
                lblError.Text = "Zu den gewählten Kriterien wurden keine unbezahlten Fahrzeuge gefunden."
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
' $History: Report23_2.aspx.vb $
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
' *****************  Version 11  *****************
' User: Uha          Date: 2.07.07    Time: 12:52
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 8.06.07    Time: 15:36
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Abgleich Beyond Compare
' 
' *****************  Version 9  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************

