Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Globalization

Public Class Report82
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

    Private ReadOnly m_StatusAll As String = "A"
    Private ReadOnly m_StatusOpen As String = "O"
    Private ReadOnly m_StatusClosed As String = "G"


    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents btnCal1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents btnCal2 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents calBis As System.Web.UI.WebControls.Calendar
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trCreate As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents rbAll As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbOpen As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbClosed As System.Web.UI.WebControls.RadioButton
    Protected WithEvents txtVorhaben As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUebergabeVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUebergabeBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblUebergabeVon As System.Web.UI.WebControls.Label
    Protected WithEvents lblUebergabeBis As System.Web.UI.WebControls.Label
    Protected WithEvents lblVorhaben As System.Web.UI.WebControls.Label
    Protected WithEvents lblStatus As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        lblError.Text = String.Empty

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                Session("MultiResult") = Nothing
                txtUebergabeVon.Text = Date.Now.AddMonths(-1).ToShortDateString()
                txtUebergabeBis.Text = Date.Now.ToShortDateString()
                rbOpen.Checked = True
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim m_Report As New VW_03(m_User, m_App, "")
        Dim b As Boolean

        Try
            Dim datUebergabeVon As DateTime
            Dim datUebergabeBis As DateTime
            Dim strUebergabeVon As String = ""
            Dim strUebergabeBis As String = ""
            Dim strVorhaben As String

            b = True
            If txtVorhaben.Text.Length = 0 Then
                strVorhaben = ""
            Else
                strVorhaben = txtVorhaben.Text
            End If

            txtUebergabeVon.Text = txtUebergabeVon.Text.Trim(" "c)
            If Len(txtUebergabeVon.Text) > 0 Then
                If IsDate(txtUebergabeVon.Text) Then
                    datUebergabeVon = CDate(txtUebergabeVon.Text)
                    strUebergabeVon = txtUebergabeVon.Text
                Else
                    lblError.Text = "Übergabe Von - Muss einen Datumswert beinhalten."
                    b = False
                End If
            End If
            txtUebergabeBis.Text = txtUebergabeBis.Text.Trim(" "c)
            If Len(txtUebergabeBis.Text) > 0 Then
                If IsDate(txtUebergabeBis.Text) Then
                    datUebergabeBis = CDate(txtUebergabeBis.Text)
                    strUebergabeBis = txtUebergabeBis.Text
                Else
                    lblError.Text = "Übergabe Bis - Muss einen Datumswert beinhalten."
                    b = False
                End If
            End If

            If txtUebergabeVon.Text.Length + txtUebergabeBis.Text.Length + txtVorhaben.Text.Length = 0 Then
                lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
                b = False
            Else
                If b Then
                    Dim tmpStatus As String = GetStatus()

                    m_Report.FillHaendlerstatus(Session("AppID").ToString, Session.SessionID.ToString, strUebergabeVon, strUebergabeBis, strVorhaben, tmpStatus)

                    Session("ResultTable") = m_Report.History
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            b = False
        End Try

        If b Then
            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If (m_Report.Result Is Nothing) OrElse (m_Report.Result.Rows.Count = 0) Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    Dim tmpTable As DataTable = CreateDataTableForExcelReport(m_Report.Result)

                    Session("ResultTable") = tmpTable
                    CreateExcelReportForSelectedHandlerstatus(tmpTable)
                    Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        End If

        Session("ShowLink") = "False"
    End Sub

    Private Sub btnCal1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal1.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Private Sub btnCal2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCal2.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtUebergabeVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtUebergabeBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub

    'Prüft, ob ein Text exakt ein Datum im Format "TT.MM.JJJJ" darstellt
    Private Function IsTextDateValid(ByVal pTextDate As String) As Boolean
        Dim baseDate As Date

        If pTextDate.Trim().Length <> 10 Then Return False
        If Not (pTextDate.Chars(2) = ".") Or Not (pTextDate.Chars(5) = ".") Then Return False

        Try
            baseDate = Date.ParseExact(pTextDate, "dd.MM.yyyy", New CultureInfo("de-DE"))
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    'Erzeugen des Excel-Sheets für die Händlerstatus
    Private Sub CreateExcelReportForSelectedHandlerstatus(ByVal pTblHandlerstatus As DataTable)
        Dim strfilename As String = ConfigurationManager.AppSettings("ExcelPath") & Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

        Try
            Excel.ExcelExport.WriteExcel(pTblHandlerstatus, strfilename)
            Session("lnkExcel") = strfilename
        Catch
        End Try
    End Sub

    Private Function CreateDataTableForExcelReport(ByVal pTable As DataTable) As DataTable
        Dim tmpRow As DataRow
        Dim tmpTable As DataTable
        Dim newRow As DataRow

        tmpTable = New DataTable()


        With tmpTable.Columns
            .Add("Vorhaben", System.Type.GetType("System.String"))
            .Add("IKZ", System.Type.GetType("System.String"))
            .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
            .Add("Kennzeichen", System.Type.GetType("System.String"))
            .Add("Übergabe soll", System.Type.GetType("System.DateTime"))
            'Bei Status 'Offen' existiert kein Übergabedatum für Lieferscheine
            'und soll daher auch nicht ausgegeben werden
            If GetStatus() <> "O" Then
                .Add("Übergabe ist", System.Type.GetType("System.DateTime"))
                .Add("Übergabe Differenz", System.Type.GetType("System.Int32"))
            End If
            .Add("Zulassungsdatum", System.Type.GetType("System.DateTime"))
            'Bei Status 'Offen' existiert kein Eingangsdatum für Lieferscheine
            'und soll daher auch nicht ausgegeben werden
            If GetStatus() <> "O" Then
                .Add("Eingang Lieferschein", System.Type.GetType("System.DateTime"))
            End If
            .Add("Mahndatum", System.Type.GetType("System.DateTime"))
            .Add("Mahnstufe", System.Type.GetType("System.String"))
        End With

        For Each tmpRow In pTable.Rows
            newRow = tmpTable.NewRow()
            newRow("Vorhaben") = tmpRow("ZZREFERENZ1")
            newRow("IKZ") = tmpRow("Liznr")
            newRow("Fahrgestellnummer") = tmpRow("Chassis_Num")
            newRow("Kennzeichen") = tmpRow("License_Num")
            If IsDateSAPDate(tmpRow("ZZFAEDT")) Then
                newRow("Übergabe soll") = ConvertSAPDateToDate(tmpRow("ZZFAEDT"))
            End If
            'Bei Status 'Offen' existiert kein Eingangsdatum für Lieferscheine
            'und soll daher auch nicht ausgegeben werden
            If GetStatus() <> "O" Then
                If IsDateSAPDate(tmpRow("Auldt")) Then
                    newRow("Übergabe ist") = ConvertSAPDateToDate(tmpRow("Auldt"))
                End If
                If IsNumericSAP(tmpRow("DIFF_TAGE")) Then
                    newRow("Übergabe Differenz") = CInt(tmpRow("DIFF_TAGE"))
                End If
            End If
            If IsDateSAPDate(tmpRow("Repla_Date")) Then
                newRow("Zulassungsdatum") = ConvertSAPDateToDate(tmpRow("Repla_Date"))
            End If
            'Bei Status 'Offen' existiert kein Eingangsdatum für Lieferscheine
            'und soll daher auch nicht ausgegeben werden
            If GetStatus() <> "O" Then
                If IsDateSAPDate(tmpRow("Zzabrdt")) Then
                    newRow("Eingang Lieferschein") = ConvertSAPDateToDate(tmpRow("Zzabrdt"))
                End If
            End If
            If IsDateSAPDate(tmpRow("Zzmadat")) Then
                newRow("Mahndatum") = ConvertSAPDateToDate(tmpRow("Zzmadat"))
            End If
            newRow("Mahnstufe") = tmpRow("Zzmahns")
            tmpTable.Rows.Add(newRow)
        Next

        tmpTable.AcceptChanges()
        Return tmpTable
    End Function

    'Ein Datum aus SAP liegt im Format YYYYMMDD vor. Wenn das Datum nicht gefüllt ist,
    'wird 00000000 geliefert
    Private Function IsSAPDateEmpty(ByVal pSAPDate As String) As Boolean
        If pSAPDate = "00000000" Or pSAPDate = String.Empty Then
            Return True
        End If
        Return False
    End Function

    Private Function IsNumericSAP(ByVal pSAPInteger As String) As Boolean
        Dim tmpInteger As Int32

        Try
            tmpInteger = CInt(pSAPInteger)
            Return True
        Catch
            Return False
        End Try
    End Function

    Private Function IsDateSAPDate(ByVal pSAPDate As String) As Boolean
        Dim tmpDate As Date

        Try
            tmpDate = Date.ParseExact(pSAPDate, "yyyyMMdd", CultureInfo.InvariantCulture)
            Return True
        Catch
            Return False
        End Try
    End Function

    Private Function ConvertSAPDateToDate(ByVal pSAPDate As String) As Date
        Dim tmpDate As Date

        Try
            tmpDate = Date.ParseExact(pSAPDate, "yyyyMMdd", CultureInfo.InvariantCulture)
            Return tmpDate
        Catch
            Return Date.MinValue
        End Try
    End Function

    Public Function GetStatus() As String
        If rbAll.Checked Then Return m_StatusAll
        If rbOpen.Checked Then Return m_StatusOpen
        If rbClosed.Checked Then Return m_StatusClosed
        Throw New System.Exception("Unbekannter Status.")
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report82.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:14
' Updated in $/CKAG/Applications/appvw/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:48
' Created in $/CKAG/Applications/appvw/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 12.09.07   Time: 11:25
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Kleine Änderung in Report82 (andere Texte) - dabei Formular zur
' Übersetzung vorbereitet
' 
' *****************  Version 5  *****************
' User: Uha          Date: 13.08.07   Time: 17:03
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Bugfixing in "Lieferschein-Handling" lt. ITA 1125
' 
' *****************  Version 4  *****************
' User: Uha          Date: 13.08.07   Time: 14:04
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Lieferschein-Handling lt. ITA 1125 geändert
' 
' *****************  Version 3  *****************
' User: Uha          Date: 20.06.07   Time: 14:32
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 2  *****************
' User: Uha          Date: 22.05.07   Time: 10:27
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 1  *****************
' User: Uha          Date: 21.05.07   Time: 15:52
' Created in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Änderungen im Vergleich zur Startapplikation zum Stand 21.05.2007
' 
' ************************************************
