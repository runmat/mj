Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change11
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
    Private objFahrzeuge As CSC_Sperrliste

    Protected WithEvents Hyperlink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkLogout As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            DoSubmit()
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
        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        objFahrzeuge = New CSC_Sperrliste(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
        objFahrzeuge.Customer = m_User.KUNNR
        objFahrzeuge.Show(Session("AppID").ToString, Session.SessionID, Me)
        If Not objFahrzeuge.Status = 0 Then
            lblError.Text = objFahrzeuge.Message
            lblError.Visible = True
        Else
            If objFahrzeuge.Fahrzeuge.Rows.Count = 0 Then
                lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
            Else
                Dim tblTemp As DataTable
                tblTemp = New DataTable()
                tblTemp.Columns.Add("Kontonummer", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Briefnummer", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Kennzeichen", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Label", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Modellbezeichnung", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Datum Sperrliste", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Datum Briefeingang", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Datum Versand", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Problem", System.Type.GetType("System.String"))
                Dim i As Int32
                Dim rowNew As DataRow
                For i = 0 To objFahrzeuge.Fahrzeuge.Rows.Count - 1
                    rowNew = tblTemp.NewRow
                    rowNew("Kontonummer") = objFahrzeuge.Fahrzeuge.Rows(i)("Kontonummer")
                    rowNew("Fahrgestellnummer") = objFahrzeuge.Fahrzeuge.Rows(i)("Fahrgestellnummer")
                    rowNew("Briefnummer") = objFahrzeuge.Fahrzeuge.Rows(i)("Briefnummer")
                    rowNew("Kennzeichen") = objFahrzeuge.Fahrzeuge.Rows(i)("Kennzeichen")
                    rowNew("Label") = objFahrzeuge.Fahrzeuge.Rows(i)("Label")
                    rowNew("Modellbezeichnung") = objFahrzeuge.Fahrzeuge.Rows(i)("Modellbezeichnung")
                    If IsDate(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Sperrliste")) Then
                        rowNew("Datum Sperrliste") = CType(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Sperrliste"), DateTime).ToShortDateString
                    End If
                    If IsDate(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Briefeingang")) Then
                        rowNew("Datum Briefeingang") = CType(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Briefeingang"), DateTime).ToShortDateString
                    End If
                    If IsDate(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Versand")) Then
                        rowNew("Datum Versand") = CType(objFahrzeuge.Fahrzeuge.Rows(i)("Datum_Versand"), DateTime).ToShortDateString
                    End If
                    rowNew("Problem") = objFahrzeuge.Fahrzeuge.Rows(i)("Problem")
                    tblTemp.Rows.Add(rowNew)
                Next

                Dim objExcelExport As New Excel.ExcelExport()
                Try
                    Excel.ExcelExport.WriteExcel(tblTemp, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                Catch
                End Try
                Session("objFahrzeuge") = objFahrzeuge
                Response.Redirect("Change11_2.aspx?AppID=" & Session("AppID").ToString)
            End If
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
' $History: Change11.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.03.10   Time: 13:34
' Updated in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 2.05.08    Time: 10:37
' Updated in $/CKAG/Applications/appcsc/Forms
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:37
' Created in $/CKAG/Applications/appcsc/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 10:42
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.03.07    Time: 9:28
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Forms
' 
' ************************************************
