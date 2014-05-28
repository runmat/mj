Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports CKG.Services

Partial Public Class Change02
    Inherits System.Web.UI.Page
#Region "Declarations"
    Private m_User As Security.User
    Private m_App As Security.App
    Private m_change As Versand
    Private versandart As String
    Private booError As Boolean
#End Region


    Private ReadOnly Property FormSearch() As CKG.Services.SearchForm
        Get
            Return DirectCast(FrmSearch, CKG.Services.SearchForm)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New Security.App(m_User)
        versandart = Request.QueryString.Item("art").ToString
        lblError.Text = ""
        Try
            If Session("AppChange") Is Nothing Then
                m_change = New Versand(m_User, m_App, CStr(Request.QueryString("AppID")), Session.SessionID.ToString, "")
                Session("AppChange") = m_change
            Else
                m_change = CType(Session("AppChange"), Versand)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten:<br>" & ex.Message
        End Try
    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click

        Try
            If Not FormSearch.KennzeichenText = String.Empty Then
                'FormSearch.KennzeichenText = Replace(FormSearch.KennzeichenText, " ", "").Trim(","c)
                'If FormSearch.KennzeichenText.Length = 0 Then
                'FormSearch.KennzeichenText = String.Empty
                'End If
            End If
            'If FormSearch.KennzeichenText = String.Empty AndAlso FormSearch.FahrgestellText = String.Empty AndAlso FormSearch.Referenz1Text = String.Empty Then
            If FormSearch.IsEmpty() Then
                lblError.Text = "Bitte geben Sie mindestens ein Suchkriterium an."
                Exit Sub
            End If
            DoSubmit()
        Catch ex As Exception
            'lblerror.Text = Err.Description
        End Try
    End Sub

    Private Sub DoSubmit()
        Dim b As Boolean
        lblError.Text = ""
        b = True
        If FormSearch.FahrgestellText.Length = 0 Then
            m_change.SucheFahrgestellNr = ""
        Else
            m_change.SucheFahrgestellNr = FormSearch.FahrgestellText
            'If m_change.SucheFahrgestellNr.Length < 17 Then
            '    If m_change.SucheFahrgestellNr.Length > 4 Then
            '        FormSearch.FahrgestellText = "*" & m_change.SucheFahrgestellNr
            '        m_change.SucheFahrgestellNr = "%" & m_change.SucheFahrgestellNr
            '    Else
            '        lblError.Text = "Bitte geben Sie die Fahrgestellnummer mindestens 8-stellig ein."
            '        b = False
            '    End If
            'End If
        End If

        If FormSearch.KennzeichenText.Length = 0 Then
            m_change.SucheKennzeichen = ""
        Else
            'FormSearch.KennzeichenText = Replace(FormSearch.KennzeichenText.Trim(" "c), " ", "")
            m_change.SucheKennzeichen = FormSearch.KennzeichenText
            ''prüfe auf Eingabeformat Kreis und ein Buchstabe JJU2008.04.07
            'Dim tmpaKennzeichen As String()
            'tmpaKennzeichen = FormSearch.KennzeichenText.Split(",".ToCharArray)
            'Dim tmpStr As String
            'Dim tmpStr2 As String
            'For Each tmpStr2 In tmpaKennzeichen
            '    tmpStr = tmpStr2.Replace("*", "")
            '    If Not tmpStr.IndexOf("-") = -1 Then
            '        If Not tmpStr.Length > tmpStr.IndexOf("-") + 1 OrElse tmpStr.IndexOf("-") = 0 OrElse tmpStr.Length < 3 Then
            '            lblError.Text = "Bitte beachten Sie das Eingabeformat für Kennzeichen."
            '            b = False
            '            Exit For
            '        End If
            '    Else
            '        lblError.Text = "Bitte beachten Sie das Eingabeformat für Kennzeichen."
            '        b = False
            '        Exit For
            '    End If

            'Next
        End If
        If FormSearch.Referenz1Text.Length = 0 Then
            m_change.UnitNr = ""
        Else
            m_change.UnitNr = FormSearch.Referenz1Text
        End If
        If b Then
            m_change.EquiTyp = "T"
            'm_change.UnitNr = ""
            m_change.GiveCars(Session("AppID").ToString, Session.SessionID, Me)
            Dim blnGo As Boolean = False
            If Not m_change.Status = 0 Then
                lblError.Text = m_change.Message
                lblError.Visible = True
            Else
                If m_change.Result.Rows.Count = 0 Then
                    lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                Else
                    blnGo = True
                End If
            End If

            If blnGo Then
                Session("AppChange") = m_change
                Response.Redirect("Change02_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
            End If
        End If
    End Sub

    Protected Sub btndefault_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btndefault.Click
        lbCreate_Click(sender, e)
    End Sub

    Private Sub lbBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


End Class