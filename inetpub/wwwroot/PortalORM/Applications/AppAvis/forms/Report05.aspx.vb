Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Report05
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

    '-----------------------------------------------------------------------
    ' Methode: DoSubmit
    ' Autor: J.Jung
    ' Beschreibung: Aufruf der Methode ReservierteKennzeichen.Fill, 
    '               Prüfung und Übergabe der Selektionsparameter
    ' Erstellt am: 24.11.2008
    ' ITA: 2421
    '-----------------------------------------------------------------------
    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim mObjReservierteKennzeichen As New ReservierteKennzeichen(m_User, m_App, strFileName)


            lblError.Text = ""
            Dim tmpErrorText As String = ""

            If Not HelpProcedures.checkDate(txtEingangsdatumVon, txtEingangsdatumBis, tmpErrorText, True, 0) Then
                lblError.Text = tmpErrorText
            End If

            mObjReservierteKennzeichen.Fahrgestellnummer = txtFahrgestellnummer.Text.Trim(" "c)
            mObjReservierteKennzeichen.Fahrzeugmodell = txtFahrzeugmodell.Text.Trim(" "c)
            mObjReservierteKennzeichen.EingangBis = txtEingangsdatumBis.Text
            mObjReservierteKennzeichen.EingangVon = txtEingangsdatumVon.Text
            mObjReservierteKennzeichen.PDI = txtPDI.Text.Trim(" "c)




            If lblError.Text.Length = 0 Then
                mObjReservierteKennzeichen.Fill(Session("AppID").ToString, Session.SessionID.ToString)

                Session("ResultTable") = mObjReservierteKennzeichen.Result

                If Not mObjReservierteKennzeichen.Status = 0 Then
                    lblError.Text = mObjReservierteKennzeichen.Message
                Else
                    Dim objExcelExport As New Excel.ExcelExport()
                    Try
                        Excel.ExcelExport.WriteExcel(mObjReservierteKennzeichen.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Catch
                    End Try
                    Session("lnkExcel") = "/PortalORM/Temp/Excel/" & strFileName
                    Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtEingangsdatumVon.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub

    Private Sub calBis_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBis.SelectionChanged
        txtEingangsdatumBis.Text = calBis.SelectedDate.ToShortDateString
        calBis.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub ImageButton2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Protected Sub btnCal1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCal1.Click
        calVon.Visible = True
        calBis.Visible = False
    End Sub

    Protected Sub btnCal2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCal2.Click
        calVon.Visible = False
        calBis.Visible = True
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("/PortalORM/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub
End Class

' ************************************************
' $History: Report05.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 28.04.09   Time: 17:09
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 24.11.08   Time: 13:33
' Updated in $/CKAG/Applications/AppAvis/forms
' ITa 2421 nachbesserung
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.11.08   Time: 13:15
' Updated in $/CKAG/Applications/AppAvis/forms
' ITA 2421 nachbesserungen
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 24.11.08   Time: 9:35
' Created in $/CKAG/Applications/AppAvis/forms
' ITA 2421 testfertig
'
' ************************************************