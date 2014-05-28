Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Report200
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

    End Sub

    Private Function checkInput() As Boolean
        Dim datevon As Date
        Dim datebis As Date

        Try
            datevon = CType(txtAbmeldedatumVon.Text, Date)
            datebis = CType(txtAbmeldedatumBis.Text, Date)
        Catch ex As Exception
            Return False
        End Try

        If (datevon > datebis) Then
            Return False
        End If
        Return True
    End Function

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        If (checkInput()) Then
            DoSubmit()
        Else
            lblError.Text = "Falsche Eingabeparameter."
        End If
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)


        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        Dim m_Report As New ec_200(m_User, m_App, strFileName)
        Dim datAbmeldedatumVon As DateTime
        Dim datAbmeldedatumBis As DateTime

        If (txtAbmeldedatumVon.Text = String.Empty And txtAbmeldedatumBis.Text = String.Empty) Then
            lblError.Text = "Keine Abfragekriterien eingegeben."
            Exit Sub
        End If

        lblError.Text = ""

        If Not (txtAbmeldedatumVon.Text = String.Empty) Then
            If Not IsDate(txtAbmeldedatumVon.Text) Then
                If Not IsStandardDate(txtAbmeldedatumVon.Text) Then
                    If Not IsSAPDate(txtAbmeldedatumVon.Text) Then
                        lblError.Text = "Geben Sie bitte ein gültiges ""Abmeldedatum von"" ein!<br>"
                    Else
                        datAbmeldedatumVon = CDate(txtAbmeldedatumVon.Text)
                    End If
                Else
                    datAbmeldedatumVon = CDate(txtAbmeldedatumVon.Text)
                End If
            Else
                datAbmeldedatumVon = CDate(txtAbmeldedatumVon.Text)
            End If
        End If


        If Not (txtAbmeldedatumBis.Text = String.Empty) Then
            If Not IsDate(txtAbmeldedatumBis.Text) Then
                If Not IsStandardDate(txtAbmeldedatumBis.Text) Then
                    If Not IsSAPDate(txtAbmeldedatumBis.Text) Then
                        lblError.Text &= "Geben Sie bitte ein gültiges ""Abmeldedatum bis"" ein!<br>"
                    Else
                        datAbmeldedatumBis = CDate(txtAbmeldedatumBis.Text)
                    End If
                Else
                    datAbmeldedatumBis = CDate(txtAbmeldedatumBis.Text)
                End If
            Else
                datAbmeldedatumBis = CDate(txtAbmeldedatumBis.Text)
            End If
        End If

        If lblError.Text.Length = 0 Then
            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, datAbmeldedatumVon, datAbmeldedatumBis, Me.Page)

            Session("ResultTable") = m_Report.Result

            If Not m_Report.Status = 0 Then
                lblError.Text = "Fehler: " & m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else

                    Dim objExcelExport As New Excel.ExcelExport()

                    Excel.ExcelExport.WriteExcel(m_Report.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)

                    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                    logApp.UpdateEntry("APP", Session("AppID").ToString, "Report: Abgemeldete Fahrzeuge")
                    Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        End If

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
        txtAbmeldedatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtAbmeldedatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class