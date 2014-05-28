Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report05
    Inherits System.Web.UI.Page
    '##### VW Report "Klärfallreport"
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

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtVorhaben As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFahrgestellnummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_Haendlernummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents ucStyles As Styles

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

            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        lblError.Text = ""
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        Try
            'Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            'Dim m_Report As New VW_05(m_User, m_App, strFileName)
            Dim m_Report As New VW_05(m_User, m_App, "")



            'Vorhaben als Pflichtfeld
            If txtVorhaben.Text.Length = 0 AndAlso txtFahrgestellnummer.Text.Length = 0 AndAlso txt_Haendlernummer.Text.Length = 0 Then
                lblError.Text = "Bitte geben Sie eine Vorhabennummer, Fahrgestellnummer oder eine Händlernummer ein."
            Else

                If txtFahrgestellnummer.Text.LastIndexOf("*") > 0 Then
                    If (txtFahrgestellnummer.Text.Substring(0, txtFahrgestellnummer.Text.IndexOf("*"))).Length < 11 And Not txtFahrgestellnummer.Text.Length = 0 Then
                        lblError.Text = "Das Selektionskriterium 'Fahrgestellnummer' muss mindestends 11 Zeichen enthalten"
                    End If
                Else

                    If txtFahrgestellnummer.Text.Length < 11 And Not txtFahrgestellnummer.Text.Length = 0 Then
                        lblError.Text = "Das Selektionskriterium 'Fahrgestellnummer' muss mindestends 11 Zeichen enthalten"
                    End If
                End If

                If (lblError.Text.Length = 0) Then
                    If txtFahrgestellnummer.Text.LastIndexOf("*") > 0 Then
                        m_Report.Fahrgestellnummer = txtFahrgestellnummer.Text.Substring(0, txtFahrgestellnummer.Text.IndexOf("*"))
                    Else
                        m_Report.Fahrgestellnummer = txtFahrgestellnummer.Text
                    End If
                    m_Report.Haendlernummer = txt_Haendlernummer.Text
                    m_Report.Vorhaben = txtVorhaben.Text
                End If

            End If

            If lblError.Text.Length = 0 Then
                m_Report.FILL(Session("AppID").ToString, Session.SessionID.ToString)

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Daten vorhanden."
                    Else
                        Session("ExcelTable") = m_Report.Result
                        Dim tmpTable As DataTable = m_Report.Result.Copy

                        'Überflüssige Spalten aus Darstellung entfernen
                        '(von hinten beginnend)

                        'ZIELBAHNHOF','Zielbahnhof',205
                        tmpTable.Columns.RemoveAt(37)

                        'RECHNUG_NR','Rechnungsnummer',200
                        tmpTable.Columns.RemoveAt(36)

                        'PROJEKT','Projektnummer',195
                        tmpTable.Columns.RemoveAt(35)

                        'KSKTXT','Kostenstelle',190
                        tmpTable.Columns.RemoveAt(34)

                        'DAT_AUSLIEF','Geplanter Liefertermin',185
                        'tmpTable.Columns.RemoveAt(33)

                        'HERST_SCHL','Hersteller Schlüsselnummer',180
                        tmpTable.Columns.RemoveAt(32)

                        'TIDNR','Nummer ZB2',175
                        tmpTable.Columns.RemoveAt(31)

                        'CHASSIS_NUM','Fahrgestellnummer',170
                        tmpTable.Columns.RemoveAt(30)

                        'WKENNZ_V1','Wunschkennzeichen',165
                        tmpTable.Columns.RemoveAt(29)

                        'TELEFON_POST','Post Übergabe Telefon',150
                        tmpTable.Columns.RemoveAt(28)

                        'NAME_POST','Post Übergabe Name',145
                        'tmpTable.Columns.RemoveAt(27)

                        'ORT_POST','Post Übergabeort',140
                        tmpTable.Columns.RemoveAt(26)

                        'TELEFON_SUS','Händler Telefon',135
                        tmpTable.Columns.RemoveAt(25)

                        'ORT_SUS','Händler Ort',130
                        'tmpTable.Columns.RemoveAt(24)

                        'PSTLZ_SUS','Händler PLZ',125
                        tmpTable.Columns.RemoveAt(23)

                        'STRASSE_SUS','Händlerstraße',120
                        tmpTable.Columns.RemoveAt(22)

                        'NAME2_SUS','Händlername',115
                        'tmpTable.Columns.RemoveAt(21)

                        'NAME1_SUS','Händleranrede',110
                        tmpTable.Columns.RemoveAt(20)

                        'NUMMER_SUS','Händler Nummer des Herstellers',105
                        tmpTable.Columns.RemoveAt(19)

                        'ID_SUS','Händler ID',100
                        tmpTable.Columns.RemoveAt(18)

                        'TELEFON_HA','Halter Telefon',95
                        tmpTable.Columns.RemoveAt(17)

                        'ORT_HA','Halter Ort',90
                        'tmpTable.Columns.RemoveAt(16)

                        'PSTLZ_HA','Halter PLZ',85
                        tmpTable.Columns.RemoveAt(15)

                        'STRASSE_HA','Halter Straße',80
                        tmpTable.Columns.RemoveAt(14)

                        'NAME1_HA','Halter Name',75
                        'tmpTable.Columns.RemoveAt(13)

                        'TELEFON_ESO','Einsatzort Telefon',70
                        tmpTable.Columns.RemoveAt(12)

                        'ORT_ESO','Einsatzort Ort',65
                        'tmpTable.Columns.RemoveAt(11)

                        'PSTLZ_ESO','Einsatzort PLZ',60
                        tmpTable.Columns.RemoveAt(10)

                        'STRASSE_ESO','Einsatzort Straße',55
                        tmpTable.Columns.RemoveAt(9)

                        'ANREDE_ESO','Einsatzort Anrede',50
                        tmpTable.Columns.RemoveAt(8)

                        'EINSATZORT_ID','Einsatzort ID',45
                        'tmpTable.Columns.RemoveAt(7)

                        'VARIANTE','Variante',40
                        tmpTable.Columns.RemoveAt(6)

                        'FZGTYP','Fahrzeugtyp',35
                        tmpTable.Columns.RemoveAt(5)

                        'HERST','Fahrzeughersteller',30
                        tmpTable.Columns.RemoveAt(4)

                        'REFERENZ2','Vorhabennummer',25
                        'tmpTable.Columns.RemoveAt(3)

                        'BSTPO','Bestellposition',20
                        tmpTable.Columns.RemoveAt(2)

                        'BSTNR','Bestellnummer',15
                        tmpTable.Columns.RemoveAt(1)

                        'REFERENZ1','IKZ',10
                        'tmpTable.Columns.RemoveAt(0)
                        Session("ResultTable") = tmpTable

                        'Dim objExcelExport As New Excel.ExcelExport()
                        'Try
                        '    objExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        'Catch
                        'End Try
                        'Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                        logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: " & lblHead.Text)

                        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString, False)
                        'Response.Redirect("/Portal/(" & Session.SessionID & ")/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString & "&legende=Report01")

                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            'logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler im Report <Klärfallreport>. Fehler: " & ex.Message)
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report05.aspx.vb $
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
' *****************  Version 8  *****************
' User: Jungj        Date: 6.11.07    Time: 14:32
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 6.11.07    Time: 14:00
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 6.11.07    Time: 13:33
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 6.11.07    Time: 12:58
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 12.09.07   Time: 17:03
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 22.08.07   Time: 17:52
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Bugfixing ITA 1120 und 1177
' 
' *****************  Version 2  *****************
' User: Uha          Date: 13.08.07   Time: 16:13
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Forms
' Vorhaben zu Pflichtfeld erklärt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 13.08.07   Time: 16:11
' Created in $/CKG/Applications/AppVW/AppVWWeb/Forms
' ITA 1125 "Werkstattzuordnungsliste" hinzugefügt
' 
' ************************************************
