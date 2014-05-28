Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Public Class Report02
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

    Protected WithEvents txtDateVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDateBis As System.Web.UI.WebControls.TextBox

    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents cldSelect As System.Web.UI.WebControls.Calendar
    Protected WithEvents reqFieldValVersJahr As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents imgbSelectDateVon As ImageButton
    Protected WithEvents imgbSelectDatebis As ImageButton


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
        Session("ShowOtherString") = Nothing

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
                    If IsDate(objhandler.DatumVon) Then txtDateVon.Text = objhandler.DatumVon
                    If IsDate(objhandler.DatumBis) Then txtDateBis.Text = objhandler.DatumBis

                    objhandler = Nothing
                    Session("objVFS02") = Nothing
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

    End Sub


    Private Sub fill()
        'txtOrgNr.Text = "1*"
        drpVJahr.Items.Add(New ListItem(Now.Year, Now.Year))
        drpVJahr.Items.Add(New ListItem(Now.Year - 1, Now.Year - 1))
        drpVJahr.Items.Add(New ListItem(Now.Year - 2, Now.Year - 2))
        'txtDateVon.Text = Date.Now.ToShortDateString()
        'txtDateBis.Text = txtDateVon.Text
        'txtKennzeichen.Text = String.Empty
    End Sub


    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click

        If Not IsPageValid() Then Exit Sub

        'Bei Eingabe eines Kennzeichens wird direkt die Liste der Kennzeichen angezeigt.
        'If txtKennzeichen.Text.Trim().Length > 0 Then
        '    ShowReportKennzeichenliste()
        '    Exit Sub
        'End If

        objhandler = New VFS02(m_User, m_App, "")

        objhandler.Versicherungsjahr = drpVJahr.SelectedItem.Text.Trim()
        objhandler.OrgNr = txtOrgNr.Text.Trim()
        objhandler.Kennzeichen = txtKennzeichen.Text.Trim().ToUpper

        objhandler.DatumVon = txtDateVon.Text.Trim
        objhandler.DatumBis = txtDateBis.Text.Trim

     
        objhandler.GiveData(Session("AppID").ToString, Session.SessionID)

        Session("objVFS02") = objhandler

        objhandler = Nothing

        Response.Redirect("Report02_1.aspx?AppID=" & Session("AppID").ToString)
    End Sub

   
    'Es wurde ein Kennzeichen eingegeben. Daher wird direkt der Report für
    'das Kennzeichen aufgerufen
    Private Sub ShowReportKennzeichenliste()
        Dim tmpTable As DataTable

        objhandler = New VFS02(m_User, m_App, "")

        objhandler.Versicherungsjahr = drpVJahr.SelectedItem.Text.Trim()
        objhandler.Kennzeichen = txtKennzeichen.Text.Trim.ToUpper

        If txtOrgNr.Text.Trim().Length > 0 Then
            objhandler.OrgNr = txtOrgNr.Text.Trim()
            objhandler.GiveDataByKennzeichen(Session("AppID").ToString, Session.SessionID)
        Else
            objhandler.GiveDataByOrgNrAndKennzeichen(Session("AppID").ToString, Session.SessionID)
        End If
        Session("objVFS02") = objhandler

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
            .Add("Versand am", System.Type.GetType("System.DateTime"))
            .Add("VD-Bezirk", System.Type.GetType("System.Int64"))
            .Add("Name2", System.Type.GetType("System.String"))
        End With

        For Each tmpRow In pDetailTable.Rows
            newRow = tmpTable.NewRow()
            newRow("Kennzeichen") = tmpRow("Kennzeichen")
            newRow("Versand am") = tmpRow("Versand am")
            newRow("VD-Bezirk") = tmpRow("VD-Bezirk")
            newRow("Name2") = tmpRow("Name2")
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


    'Auswählen eines Datums und übernehmen in Textbox
    Private Sub cldSelect_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cldSelect.SelectionChanged
        If imgbSelectDateVon.CommandArgument <> String.Empty Then
            Me.txtDateVon.Text = cldSelect.SelectedDate.ToShortDateString()
            imgbSelectDateVon.CommandArgument = String.Empty
            If Me.txtDateBis.Text = String.Empty Then
                txtDateBis.Text = txtDateVon.Text
            End If
        Else
            Me.txtDateBis.Text = cldSelect.SelectedDate.ToShortDateString()
            Me.imgbSelectDateVon.CommandArgument = String.Empty
        End If
        Me.cldSelect.Visible = False
    End Sub

    Private Function IsTextDateValid(ByVal pTextDate As String) As Boolean
        Dim baseDate As Date

        Try
            baseDate = Date.Parse(pTextDate)
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Private Function IsPageValid() As Boolean

        'Wenn die Suche über eingegebene Kennzeichen stattfinden soll
        'werden die anderen Eingaben nicht mehr geprüft
        If txtKennzeichen.Text.Trim().Length > 0 Then
            Return True
        End If

        If txtOrgNr.Text.Trim().Length = 0 Then
            lblError.Text = "Eingabe von mindestens 2 Zeichen inkl. * für die Organistionsnummer erforderlich."
            Return False
        End If

        Dim tmpStr As String = ""
        If Not HelpProcedures.checkDate(txtDateVon, txtDateBis, tmpStr, True) Then
            lblError.Text = tmpStr
            Return False
        End If


        Return True
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub imgbSelectDateVon_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSelectDateVon.Click
        Try
            Try
                Me.cldSelect.SelectedDate = Date.Parse(txtDateVon.Text)
            Catch ex As Exception
                Me.cldSelect.SelectedDate = Date.Today
            End Try
            Me.cldSelect.Visible = True
            imgbSelectDateVon.CommandArgument = "DateVon"
            imgbSelectDateBis.CommandArgument = String.Empty
        Catch ex As Exception
            lblError.Text = "Beim Öffnen der Datumsauswahl ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub imgbSelectDateBis_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbSelectDateBis.Click
        Try
            Try
                Me.cldSelect.SelectedDate = Date.Parse(txtDateBis.Text)
            Catch ex As Exception
                Me.cldSelect.SelectedDate = Date.Today
            End Try

            Me.cldSelect.Visible = True
            imgbSelectDatebis.CommandArgument = "DateBis"
            imgbSelectDateVon.CommandArgument = String.Empty
        Catch ex As Exception
            lblError.Text = "Beim Öffnen der Datumsauswahl ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
End Class

' ************************************************
' $History: Report02.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:47
' Updated in $/CKAG/Applications/appvfs/Forms
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
' *****************  Version 7  *****************
' User: Rudolpho     Date: 19.07.07   Time: 13:54
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Forms
' ITA: 1140
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 2.07.07    Time: 16:00
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
