Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Report03
    Inherits System.Web.UI.Page

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents btnConfirm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private m_App As Base.Kernel.Security.App
    Protected WithEvents drpVJahr As System.Web.UI.WebControls.DropDownList
    Private m_User As Base.Kernel.Security.User
    Protected WithEvents txtOrgNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents reqFieldValVersJahr As System.Web.UI.WebControls.RequiredFieldValidator

    Private objhandler As VFS02


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

            If Not IsPostBack Then
                fill()

                If Not Session("objVFS02") Is Nothing Then
                    objhandler = CType(Session("objVFS02"), VFS02)

                    drpVJahr.Items.FindByText(objhandler.Versicherungsjahr).Selected = True
                    txtOrgNr.Text = objhandler.OrgNr
                    txtKennzeichen.Text = objhandler.Kennzeichen

                    objhandler = Nothing
                    Session("objVFS02") = Nothing
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub


    Private Sub fill()
        drpVJahr.Items.Add(New ListItem(Now.Year, Now.Year))
        drpVJahr.Items.Add(New ListItem(Now.Year - 1, Now.Year - 1))
        drpVJahr.Items.Add(New ListItem(Now.Year - 2, Now.Year - 2))
        'txtOrgNr.Text = "1*"
        txtKennzeichen.Text = String.Empty
    End Sub


    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click

        If Not IsPageValid() Then Exit Sub

        objhandler = New VFS02(m_User, m_App, "")

        objhandler.Versicherungsjahr = drpVJahr.SelectedItem.Text
        objhandler.OrgNr = txtOrgNr.Text.Trim()
        objhandler.Kennzeichen = txtKennzeichen.Text.Trim.ToUpper
        objhandler.GiveDataByOrgNrAndKennzeichen(Session("AppID").ToString, Session.SessionID)
        Session("objVFS02") = objhandler

        'Wenn Zeichen für Kennzeichensuche eingegeben wurde,
        'dann wird direkt der Report 'Kennzeichenliste' angezeigt
        If txtKennzeichen.Text.Trim().Length > 0 Then
            ShowReportKennzeichenliste()
            objhandler = Nothing
            Exit Sub
        Else
            objhandler = Nothing
            Response.Redirect("Report03_1.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    'Es wurde ein Kennzeichen eingegeben. Daher wird direkt der Report für
    'die Kennzeichenliste aufgerufen
    Private Sub ShowReportKennzeichenliste()
        Dim tmpTable As DataTable

        tmpTable = CreateDataTableKennzeichenliste(objhandler.DetailTable)
        Session("ResultTable") = tmpTable
        CreateExcelReportForSelectedKennzeichenliste(tmpTable)
        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString() & "&legende=AppVFS-KZL")

    End Sub

    'Hier wird aus der Gesamtmenge der Tabelle Details eine DataTable mit den
    'Datensätzen erzeugt, die den ausgewählten Werten für Vermittlernummer,
    'Auftragsnummer und Versanddatum entsprechen
    Private Function CreateDataTableKennzeichenliste(ByVal pDetailTable As DataTable) As DataTable
        Dim tmpRow As DataRow
        Dim tmpTable As DataTable
        Dim newRow As DataRow

        tmpTable = New DataTable()
        With tmpTable.Columns
            .Add("Kennzeichen", System.Type.GetType("System.String"))
            .Add("Vertragsnummer", System.Type.GetType("System.String"))
            .Add("Versand am", System.Type.GetType("System.DateTime"))
            .Add("Verkauf am", System.Type.GetType("System.DateTime"))
            .Add("Rücklauf am", System.Type.GetType("System.DateTime"))
            .Add("Verlust am", System.Type.GetType("System.DateTime"))
            .Add("VD-Bezirk", System.Type.GetType("System.Int64"))
            .Add("Name1", System.Type.GetType("System.String"))
            .Add("Name2", System.Type.GetType("System.String"))
        End With

        For Each tmpRow In pDetailTable.Rows
            newRow = tmpTable.NewRow()
            newRow("Kennzeichen") = tmpRow("Kennzeichen")
            newRow("Vertragsnummer") = tmpRow("Vertragsnummer")
            newRow("Versand am") = tmpRow("Versand am")
            newRow("VD-Bezirk") = tmpRow("VD-Bezirk")
            newRow("Rücklauf am") = tmpRow("Rücklauf am")
            newRow("Name1") = tmpRow("Name1")
            newRow("Name2") = tmpRow("Name2")
            newRow("Verlust am") = tmpRow("Verlust am")
            newRow("Verkauf am") = tmpRow("Verkauf am")
            tmpTable.Rows.Add(newRow)
        Next

        tmpTable.AcceptChanges()
        Return tmpTable
    End Function

    'Erzeugen des Excel-Sheets für die selektierte Kennzeichenliste eines Vermittlers
    Private Sub CreateExcelReportForSelectedKennzeichenliste(ByVal pTblKennzeichen As DataTable)
        Dim objExcelExport As New Excel.ExcelExport()
        Dim strfilename As String = ConfigurationManager.AppSettings("ExcelPath") & Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

        Try
            Excel.ExcelExport.WriteExcel(pTblKennzeichen, strfilename)
            Session("lnkExcel") = strfilename
        Catch
        End Try
    End Sub

    Private Function IsPageValid() As Boolean

        If txtOrgNr.Text.Trim().Length < 2 Then
            If txtKennzeichen.Text.Trim().Length < 3 Then
                lblError.Text = "Eingabe von mindestens 2 Zeichen  inkl * für die VD-Bezirk oder 3 Zeichen inkl. * für das Kennzeichen erforderlich."
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report03.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 8.01.09    Time: 8:44
' Updated in $/CKAG/Applications/appvfs/Forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 26.11.08   Time: 17:11
' Updated in $/CKAG/Applications/appvfs/Forms
' ITA 2317 unfertig
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.08   Time: 12:27
' Updated in $/CKAG/Applications/appvfs/Forms
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:43
' Created in $/CKAG/Applications/appvfs/Forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 19.07.07   Time: 13:45
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA: 1140
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 18.07.07   Time: 16:20
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA: 1140
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 11.07.07   Time: 9:20
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 25.06.07   Time: 14:26
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' bugfixing
' 
' *****************  Version 4  *****************
' User: Uha          Date: 21.06.07   Time: 18:45
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' Bugfixing VFS 2
' 
' *****************  Version 3  *****************
' User: Uha          Date: 20.06.07   Time: 18:58
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' Bug fixing 1
' 
' *****************  Version 2  *****************
' User: Uha          Date: 20.06.07   Time: 16:21
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 19:07
' Created in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
