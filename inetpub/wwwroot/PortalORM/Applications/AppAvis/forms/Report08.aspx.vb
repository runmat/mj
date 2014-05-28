Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report08
    Inherits System.Web.UI.Page

#Region "Declarationen"
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents ucStyles As Global.CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As Global.CKG.Portal.PageElements.Header
#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)
            If Not IsPostBack Then
                FillVerwendung()
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Session("ResultTable") = Nothing
        Session("ExcelTable") = Nothing
        Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        If (checkInput()) Then
            DoSubmit()
        End If
    End Sub
#End Region

#Region "Methods"

    '-----------------------------------------------------------------------
    ' Methode: DoSubmit
    ' Autor: O.Rudolph
    ' Beschreibung: Aufruf der Methode Zulassungsreport.Fill, 
    '               Prüfung und Übergabe der Selektionsparameter
    ' Erstellt am: 23.01.2009
    ' ITA: 2542
    '-----------------------------------------------------------------------
    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Zulassungsreport(m_User, m_App, strFileName)



            lblError.Text = ""

            If lblError.Text.Length = 0 Then
                If checkDate() Then
                    Dim verwendung As String = ""
                    If ddlVerwendung.SelectedValue <> "NoSel" Then
                        verwendung = ddlVerwendung.SelectedValue
                    End If

                    m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me, txtZuldatumVon.Text, txtZuldatumBis.Text, _
                                    verwendung, txtFreisetdatvon.Text, txtFreisetdatbis.Text)

                    Session("ResultTable") = m_Report.Result
                    Session("ExcelTable") = m_Report.ExcelResult
                    If Not m_Report.Status = 0 Then
                        lblError.Text = m_Report.Message
                    Else
                        Session("lnkExcel") = "/PortalORM/Temp/Excel/" & strFileName
                        logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Zulassungsreport")
                        Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If

            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function checkDate() As Boolean
        Dim tmpbool As Boolean = True
        For Each ctrl As BaseValidator In Me.Validators
            If ctrl.IsValid = False Then
                tmpbool = False
            End If
        Next
        Return tmpbool
    End Function
    Private Sub FillVerwendung()
        Dim m_Report As New Zulassungsreport(m_User, m_App, "")
        m_Report.GetVerwendung(Session("AppID").ToString, Session.SessionID, Me)

        If m_Report.Status > 0 Then
            lblError.Text = m_Report.Message
        ElseIf Not m_Report.Verwendung Is Nothing Then

            ddlVerwendung.DataSource = m_Report.Verwendung
            ddlVerwendung.DataTextField = "POS_TEXT"
            ddlVerwendung.DataValueField = "POS_KURZTEXT"
            ddlVerwendung.DataBind()
            Dim ListItemVerw As New ListItem("- keine Auswahl -", "NoSel")
            ddlVerwendung.Items.Add(ListItemVerw)
            ddlVerwendung.SelectedIndex = ddlVerwendung.Items.Count - 1
        End If

    End Sub
    Private Function checkInput() As Boolean
        Dim DateVon As Date
        Dim DateBis As Date


        If txtZuldatumVon.Text.Length + txtZuldatumBis.Text.Length + txtFreisetdatbis.Text.Length + txtFreisetdatvon.Text.Length = 0 Then
            lblError.Text = "Es muss mindestens eine der beiden möglichen Datumseingrenzungen(von-bis)erfolgen."
            Return False
        End If


        If IsDate(txtZuldatumVon.Text) AndAlso IsDate(txtZuldatumBis.Text) Then
            DateVon = CType(txtZuldatumVon.Text, Date)
            DateBis = CType(txtZuldatumBis.Text, Date)
            If DateDiff(DateInterval.Day, DateVon, DateBis) > 90 Then
                lblError.Text = "Ausgewählter Zeitraum des Zulassungsdatums zu groß. Maximal 3 Monate."
                Return False
            End If
        ElseIf IsDate(txtZuldatumVon.Text) AndAlso Not IsDate(txtZuldatumBis.Text) Then
            lblError.Text = "Zulassungsdatum bis muss gefüllt sein!"
            Return False
        ElseIf Not IsDate(txtZuldatumVon.Text) AndAlso IsDate(txtZuldatumBis.Text) Then
            lblError.Text = "Zulassungsdatum von muss gefüllt sein!"
            Return False
        End If
        If IsDate(txtFreisetdatvon.Text) AndAlso IsDate(txtFreisetdatbis.Text) Then
            DateVon = CType(txtFreisetdatvon.Text, Date)
            DateBis = CType(txtFreisetdatbis.Text, Date)
            If DateDiff(DateInterval.Day, DateVon, DateBis) > 90 Then
                lblError.Text = "Ausgewählter Zeitraum des Freisetzungsdatums zu groß. Maximal 3 Monate."
                Return False
            End If
        ElseIf IsDate(txtFreisetdatvon.Text) AndAlso Not IsDate(txtFreisetdatbis.Text) Then
            lblError.Text = "Freisetzungssdatum bis muss gefüllt sein!"
            Return False
        ElseIf Not IsDate(txtFreisetdatvon.Text) AndAlso IsDate(txtFreisetdatbis.Text) Then
            lblError.Text = "Freisetzungssdatum von muss gefüllt sein!"
            Return False
        End If
        Return True
    End Function
#End Region

End Class

' ************************************************
' $History: Report08.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 8.07.09    Time: 14:44
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA:2962
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 28.01.09   Time: 8:46
' Created in $/CKAG/Applications/AppAvis/forms
' ITA: 2542
' 
' ************************************************
