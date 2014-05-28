Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security
Imports DHLWebService
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Net
Imports System.IO
Imports System.Text

Public Class DHLRetourenLabel
    Inherits System.Web.UI.Page

    Private m_User As CKG.Base.Kernel.Security.User
    Private m_App As CKG.Base.Kernel.Security.App
    Private ews As RetourenLabel
    Private downloadPath As String = String.Empty

    Private dhlUser As String = String.Empty
    Private dhlPWD As String = String.Empty
    Private dhlDeliver As String = String.Empty
    Private dhlPortlID As String = String.Empty


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lblError.Visible = False
        ShowFile.Visible = False

        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        m_App = New App(m_User)
        Common.GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        downloadPath = CKG.Base.Kernel.Common.Common.GetCustomerSetting(m_User.Customer, Convert.ToString(Session("AppID")), "DownloadPath")

        If (String.IsNullOrEmpty(downloadPath)) Then
            ews.ReturnPath = downloadPath
        End If

        dhlUser = CKG.Base.Kernel.Common.Common.GetCustomerSetting(m_User.Customer, Convert.ToString(Session("AppID")), "DHLUser")
        If (String.IsNullOrEmpty(downloadPath)) Then
            ews.User = dhlUser
        End If

        dhlPWD = CKG.Base.Kernel.Common.Common.GetCustomerSetting(m_User.Customer, Convert.ToString(Session("AppID")), "DHLPasswort")
        If (String.IsNullOrEmpty(downloadPath)) Then
            ews.Password = dhlPWD
        End If
        dhlDeliver = CKG.Base.Kernel.Common.Common.GetCustomerSetting(m_User.Customer, Convert.ToString(Session("AppID")), "DHLDeliveryName")
        If (String.IsNullOrEmpty(downloadPath)) Then
            ews.DeliveryName = dhlDeliver
        End If
        dhlPortlID = CKG.Base.Kernel.Common.Common.GetCustomerSetting(m_User.Customer, Convert.ToString(Session("AppID")), "DHLPortalID")
        If (String.IsNullOrEmpty(downloadPath)) Then
            ews.PortalID = dhlPortlID
        End If
        dhlPortlID = CKG.Base.Kernel.Common.Common.GetCustomerSetting(m_User.Customer, Convert.ToString(Session("AppID")), "DHLPortalID")
        If (String.IsNullOrEmpty(downloadPath)) Then
            ews.PortalID = dhlPortlID
        End If


        If Not IsPostBack Then



            'prüfen ob ein Internetexplorer < Version 7 verwendet wird
            Dim version As String = Request.Browser.Version.Replace(".", ",")
            Dim ver As Double = 0.0
            If (Double.TryParse(version, ver) And Request.Browser.Browser.ToUpper().Equals("IE")) Then

                If (ver < 8.0) Then

                    lblError.Text = "Achtung!</br>Für eine optimale Darstellung wird ein Internet Explorer ab der Version 8.0 empfohlen."
                    lblError.Text += "</br>Der Kompatibilitätsmodus sollte ebenfalls ausgeschaltet sein."
                    lblError.Visible = True

                End If
            End If
        End If
    End Sub

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload

        Common.SetEndASPXAccess(Me)
    End Sub

#Region "Control Events"

    Protected Sub ibtNewSearch_Click(sender As Object, e As ImageClickEventArgs)

        If (divSelection.Visible) Then

            divSelection.Visible = False
            lb_Search.Visible = False
            divSelection.Visible = False
            ibtNewSearch.ImageUrl = "../../../Images/queryArrowUp.gif"

        Else

            lb_Search.Visible = True
            divSelection.Visible = True
            ibtNewSearch.ImageUrl = "../../../Images/queryArrow.gif"
        End If
    End Sub

    Protected Sub SearchClick(sender As Object, e As EventArgs)


        ExecuteDHLService()

    End Sub

    Protected Sub txtDatum_TextChanged(sender As Object, e As EventArgs)
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

#End Region

#Region "methods"

    Private Sub ExecuteDHLService()

        ews = New RetourenLabel()

        ews.Name1 = txt_FirstName.Text.Trim
        ews.Name2 = txt_SecondName.Text.Trim
        ews.Street = txt_Street.Text.Trim
        ews.StreetNumber = txt_StreetNumber.Text.Trim
        ews.PostalCode = txt_PostalCode.Text.Trim
        ews.City = txt_City.Text.Trim
        ews.Phone = txt_Phone.Text
        ews.Email = txt_Mail.Text.Trim

        ews.ReturnPath = downloadPath

        If String.IsNullOrEmpty(ews.Name1) Then
            lblError.Text = lbl_FirstName.Text + " muss angegeben werden!"
            txt_FirstName.Focus()
            lblError.Visible = True
            Return
        End If

        If String.IsNullOrEmpty(ews.Street) Then
            lblError.Text = lbl_Street.Text + " muss angegeben werden!"
            txt_Street.Focus()
            lblError.Visible = True
            Return
        End If

        If String.IsNullOrEmpty(ews.StreetNumber) Then
            lblError.Text = lbl_Street.Text + " muss angegeben werden!"
            txt_StreetNumber.Focus()
            lblError.Visible = True
            Return
        End If


        If String.IsNullOrEmpty(ews.PostalCode) Then
            lblError.Text = lbl_Street.Text + " muss angegeben werden!"
            txt_PostalCode.Focus()
            lblError.Visible = True
            Return
        End If

        If String.IsNullOrEmpty(ews.City) Then
            lblError.Text = lbl_Street.Text + " muss angegeben werden!"
            txt_City.Focus()
            lblError.Visible = True
            Return
        End If

        Dim sError As String = String.Empty
        Dim pathToLabel As String = ews.DoService(sError)

        If (String.IsNullOrEmpty(sError)) Then


            Dim ret As String = DownloadPDF(pathToLabel)

            If Not String.IsNullOrEmpty(ret) Then

                lblError.Text = ret
                lblError.Visible = True
            End If


        Else

            lblError.Text = sError
            lblError.Visible = True
        End If


    End Sub


    Private Function DownloadPDF(path As String) As String

        Try

            If (File.Exists(path)) Then

                Dim fi As FileInfo = New FileInfo(path)
                Response.ContentType = "Application/pdf"
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fi.Name)
                Dim FilePath As String = path
                Response.WriteFile(FilePath)
                Return String.Empty

            Else

                Return "Die Datei '" + path + "' wurde nicht gefunden"
            End If


        Catch ex As Exception

            Return ex.Message

        End Try

    End Function

#End Region


End Class