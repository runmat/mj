Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel
Partial Public Class Report01s
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Private mBeauftragung As Beauftragung

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            m_App = New App(m_User)

            lblError.Text = ""


            If Not IsPostBack Then

                Session("Kunden") = Nothing
                Session("Kreise") = Nothing
                InitControls()

            End If


            ClearControls()


        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub
    Private Sub txtKunde_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtKunde.TextChanged
        txtReferenz.Focus()
    End Sub
    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        DoSubmit()
    End Sub

    Private Sub btnDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDummy.Click
        DoSubmit()
    End Sub
    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        ClearControls()
        SetEndASPXAccess(Me)
    End Sub

#End Region


#Region "Methods"

    Private Sub InitControls()


        mBeauftragung = New Beauftragung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        With mBeauftragung
            .Gruppe = m_User.Groups(0).GroupName
            .Verkaufsbuero = Right(m_User.Reference, 4)
            .Verkaufsorganisation = Left(m_User.Reference, 4)
        End With


        mBeauftragung.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

        Dim arrKunden() As String
        Dim i As Integer = 0

        For Each Row As DataRow In mBeauftragung.Kunden.Rows

            ReDim Preserve arrKunden(i)

            arrKunden(i) = Row("NAME1").ToString
            i += 1
        Next

        Session.Add("Kunden", arrKunden)
    End Sub

    Private Sub ClearControls()


        txtKunde.BorderColor = Drawing.Color.Empty
        lblKundeInfo.Text = ""

        txtZulDatumVon.BorderColor = Drawing.Color.Empty
        lblZulDatumVonInfo.Text = ""

        txtZulDatumBis.BorderColor = Drawing.Color.Empty
        lblZulDatumBisInfo.Text = ""
    End Sub

    Private Sub DoSubmit()

        If ValidateError() = True Then Return

        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"

        mBeauftragung = New Beauftragung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)

        mBeauftragung.Kundennr = Mid(txtKunde.Text, txtKunde.Text.IndexOf(" ~ ") + 4, 8)

        mBeauftragung.Kennzeichen = txtKennzeichen.Text
        mBeauftragung.Referenz = txtReferenz.Text

        If rbG.Checked = True Then
            mBeauftragung.Loeschkennzeichen = "G"
        End If
        If rbA.Checked = True Then
            mBeauftragung.Loeschkennzeichen = "A"
        End If

        mBeauftragung.ZuldatVon = txtZulDatumVon.Text
        mBeauftragung.ZuldatBis = txtZulDatumBis.Text


        mBeauftragung.FillUebersicht(Session("AppID").ToString, Session.SessionID.ToString, Me.Page)
        Session("ResultTable") = mBeauftragung.Result

        If Not mBeauftragung.Status = 0 Then
            lblError.Text = "Fehler: " & mBeauftragung.Message
        Else
            If mBeauftragung.Result.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Session("ResultTable") = Nothing
            Else

                Dim objExcelExport As New Excel.ExcelExport()
                Try
                    'objExcelExport.WriteExcel(mBeauftragung.Result, sPath)
                    Excel.ExcelExport.WriteExcel(mBeauftragung.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                Catch ex As Exception
                    lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Try
                Session("lnkExcel") = "/Services/Temp/Excel/" & strFileName
                'Response.Redirect("/Services/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
                Response.Redirect("Report01s_2.aspx?AppID=" & Session("AppID").ToString, False)
            End If
        End If

    End Sub

    Private Function ValidateError() As Boolean

        'Kunden-Pflichtfeld prüfen
        Dim Kundenliste() As String = CType(Session("Kunden"), String())
        Dim booError = False

        Dim i As Integer = Array.IndexOf(Kundenliste, txtKunde.Text)

        If i = -1 Then
            SetErrBehavior(txtKunde, lblKundeInfo, "Ungültige Kundenauswahl.")
            booError = True
        End If

        If txtZulDatumVon.Text.Length > 0 Then
            If IsDate(txtZulDatumVon.Text) = False Then
                SetErrBehavior(txtZulDatumVon, lblZulDatumVonInfo, "Ungültiges Datum.")
                booError = True
            ElseIf txtZulDatumBis.Text.Length = 0 Then
                SetErrBehavior(txtZulDatumBis, lblZulDatumBisInfo, "Datum nicht gesetzt.")
                booError = True
            End If
        End If

        If txtZulDatumBis.Text.Length > 0 Then
            If IsDate(txtZulDatumBis.Text) = False Then
                SetErrBehavior(txtZulDatumBis, lblZulDatumBisInfo, "Ungültiges Datum.")
                booError = True
            ElseIf txtZulDatumVon.Text.Length = 0 Then
                SetErrBehavior(txtZulDatumVon, lblZulDatumVonInfo, "Datum nicht gesetzt.")
                booError = True
            End If
        End If

        If IsDate(txtZulDatumVon.Text) AndAlso IsDate(txtZulDatumBis.Text) Then

            If CDate(txtZulDatumVon.Text) > CDate(txtZulDatumBis.Text) Then
                SetErrBehavior(txtZulDatumBis, lblZulDatumBisInfo, "Datum bis größer als Datum von.")
                booError = True
            ElseIf DateDiff(DateInterval.Day, CDate(txtZulDatumVon.Text), CDate(txtZulDatumBis.Text)) > 60 Then
                SetErrBehavior(txtZulDatumVon, lblZulDatumVonInfo, "Zeitraum von 60 Tagen überschritten.")
                SetErrBehavior(txtZulDatumBis, lblZulDatumBisInfo, "Zeitraum von 60 Tagen überschritten.")
                booError = True
            End If

        End If


        Return booError

    End Function

    Private Sub SetErrBehavior(ByVal txtcontrol As TextBox, ByVal lblControl As Label, ByVal ErrText As String)

        txtcontrol.BorderColor = Drawing.Color.Red

        lblControl.Text = ErrText

    End Sub

#End Region

    ' ************************************************
    ' $History: Report01s.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 4.05.11    Time: 17:15
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 18.06.10   Time: 11:10
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 14.05.10   Time: 11:33
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 14.12.09   Time: 11:01
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 9.12.09    Time: 17:37
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.12.09    Time: 14:22
' Created in $/CKAG2/Services/Components/ComCommon/Beauftragung
' ITA: 3383
    ' 

End Class