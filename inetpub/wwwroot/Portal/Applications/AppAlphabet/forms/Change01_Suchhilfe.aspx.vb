Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change01_Suchhilfe
    Inherits System.Web.UI.Page

#Region "Declarations"

    Private m_User As Base.Kernel.Security.User
    'Private m_App As Base.Kernel.Security.App
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Private mObjVertragsdaten As Vertragsdaten


    Protected WithEvents btnSuchen As Button

    Protected WithEvents lbGet As LinkButton
    Protected WithEvents lbCancel As LinkButton

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label

    Protected WithEvents txtName As TextBox
    Protected WithEvents txtPLZ As TextBox
    Protected WithEvents txtOrt As TextBox

    Protected WithEvents ddlAdresse As DropDownList

#End Region


#Region "Properties"

    Private Property Refferer() As String
        Get
            Return ViewState.Item("refferer").ToString
        End Get
        Set(ByVal value As String)
            ViewState.Item("refferer") = value
        End Set
    End Property
#End Region


#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            m_User = GetUser(Me)
            'If Not IsPostBack Then
            '    m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            'End If
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)

            If Not IsPostBack Then
                If Not Request.UrlReferrer Is Nothing Then
                    Refferer = Request.UrlReferrer.ToString
                Else
                    Refferer = ""
                End If
                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If


            If mObjVertragsdaten Is Nothing Then
                If Session("objVertragsdatenSession") Is Nothing Then
                    Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
                Else
                    mObjVertragsdaten = CType(Session("objVertragsdatenSession"), Vertragsdaten)
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub btnSuchen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSuchen.Click
        lblError.Text = String.Empty
        ddlAdresse.Items.Clear()
        lbGet.Enabled = False
        Try
            If Replace(txtName.Text, "*", "") = "" AndAlso _
                Replace(txtPLZ.Text, "*", "") = "" AndAlso _
                Replace(txtOrt.Text, "*", "") = "" Then

                lblError.Text = "Bitte geben Sie wenigstens ein Suchkriterium an."
                Exit Sub
            End If

            mObjVertragsdaten.GetAdresse(Request.QueryString("Kennung").ToString, txtName.Text, _
                                               txtPLZ.Text, _
                                               txtOrt.Text)



            If mObjVertragsdaten.Status = 0 Then
                ddlAdresse.DataSource = mObjVertragsdaten.Adressdaten
                ddlAdresse.DataTextField = "Adresse"
                ddlAdresse.DataValueField = "POS_KURZTEXT"
                ddlAdresse.DataBind()
                lbGet.Enabled = True
            Else
                lblError.Text = mObjVertragsdaten.Message
                Exit Sub
            End If


        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Protected Sub lbGet_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbGet.Click

        Dim tmpRow As DataRow = mObjVertragsdaten.Adressdaten.Select("POS_KURZTEXT='" & ddlAdresse.SelectedValue & "'")(0)

        Dim tmpRow2 As DataRow = mObjVertragsdaten.Vertragsdaten(0)

        Select Case Request.QueryString("Kennung").ToString

            Case "HAENDLER"
                tmpRow2("HD_KUNNR") = tmpRow("POS_KURZTEXT").ToString
                tmpRow2("HD_NAME1") = tmpRow("NAME1").ToString
                tmpRow2("HD_NAME2") = tmpRow("NAME2").ToString
                tmpRow2("HD_STRAS") = tmpRow("STRAS").ToString
                tmpRow2("HD_ORT01") = tmpRow("ORT01").ToString
                tmpRow2("HD_TELEF") = tmpRow("TELNR").ToString
                tmpRow2("HD_LAND1") = tmpRow("LAND1").ToString
                tmpRow2("HD_PSTLZ") = tmpRow("PSTLZ").ToString

            Case "KB"
                tmpRow2("KZ_INTERN") = tmpRow("POS_KURZTEXT").ToString
                tmpRow2("KB_VNAME") = tmpRow("NAME1").ToString
                tmpRow2("KB_NNAME") = tmpRow("NAME2").ToString
                tmpRow2("KB_ORT01") = tmpRow("ORT01").ToString
                tmpRow2("KB_TELEFON") = tmpRow("TELNR").ToString
                tmpRow2("KB_EMAIL") = tmpRow("EMAIL").ToString
        End Select
       
        tmpRow2.AcceptChanges()

        responseBack()
    End Sub

    Protected Sub lbCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCancel.Click
        responseBack()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub responseBack()

        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If

    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub ddlAdresse_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAdresse.SelectedIndexChanged
        lbGet.Enabled = True
    End Sub

#End Region
    
End Class

' ************************************************
' $History: Change01_Suchhilfe.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 1.10.08    Time: 13:04
' Created in $/CKAG/Applications/AppAlphabet/forms
' ITA 2254 testfertig
' 
' ************************************************