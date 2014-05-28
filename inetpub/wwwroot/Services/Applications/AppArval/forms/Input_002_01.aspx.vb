Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG
Imports CKG.Services.PageElements
Imports CKG.Base.Kernel.Common.Common


Partial Public Class Input_002_01
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                Session("ShowLink") = "False"
                Session("ResultTable") = Nothing
            End If

            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        rbSelect.SelectedItem.Value = "H"
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Arval_2(m_User, m_App, strFileName)
            '---> Eingabedaten
            Dim status As String

            With m_Report
                .PKennzeichenVon = txtKennzeichenVon.Text
                .PKennzeichenBis = txtKennzeichenBis.Text
                .PFahrgestellVon = txtFahrgestellVon.Text
                .PFahrgestellBis = txtFahrgestellBis.Text
                .PLeasingNrVon = txtLeasVVon.Text
                .PLeasingNrBis = txtLeasVBis.Text

                .PKundenNr = txtKundennr.Text
                .PKlaerfall = lblKF.Checked
            End With

            If (rbSelect.SelectedItem.Value = "H") Then
                status = rbStatus.SelectedItem.Value.ToString
            Else
                status = rbMahnung.SelectedItem.Value.ToString
            End If

            '---> BAPI-Aufruf, Excel-Export und Darstellung (Report01_2)
            'If checkInput() = True Then

            m_Report.FILL(Session("AppID").ToString, Session.SessionID.ToString, status, rbSelect.SelectedItem.Value)
            Session("ResultTable") = m_Report.Result
            Session("ResultTableNative") = m_Report.getNativeData() 'Alle Spalten merken 

            If Not m_Report.Status = 0 Then
                lblError.Text = "Fehler: " & m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    Session("ExcelTable") = m_Report.Result
                    Response.Redirect("Report_002_01.aspx?AppID=" & Session("AppID").ToString & "&typ=" & rbSelect.SelectedItem.Value)
                End If
            End If
            'End If

        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub


    Private Sub lnkAuswahl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If (rbSelect.SelectedItem.Value = "H") Then
            rbStatus.Visible = True
            rbMahnung.Visible = False
            'lblTitel.Text = "Historie"
        Else
            rbStatus.Visible = False
            rbMahnung.Visible = True
            'lblTitel.Text = "Mahnstufen"
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
' $History: Input_002_01.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 22.04.09   Time: 12:34
' Updated in $/CKAG2/Applications/AppArval/forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 21.04.09   Time: 17:25
' Created in $/CKAG2/Applications/AppArval/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 6.03.07    Time: 15:30
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' GetAppIDFromQueryString(Me) hinzugefügt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************
