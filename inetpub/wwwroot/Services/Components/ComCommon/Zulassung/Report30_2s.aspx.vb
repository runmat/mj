Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security

Partial Public Class Report30_2s
    Inherits System.Web.UI.Page
    Private m_User As User
    Private m_App As App

    Private objZLDSuche As ZLD_Suche

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        'FormAuth(Me, m_User)
        m_App = New App(m_User)

        Dim tblResultTableRaw As New DataTable()
        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If (Session("ResultTableRaw") Is Nothing) Then
                lblError.Text = "Fehler: Die Seite wurde ohne Kontext aufgerufen."
            Else
                tblResultTableRaw = CType(Session("ResultTableRaw"), DataTable)
            End If

            If Not tblResultTableRaw Is Nothing Then
                If (Request.QueryString("ID") Is Nothing) OrElse (Request.QueryString("ID").ToString.Length = 0) Then
                    lblError.Text = "Feher: Die Seite wurde ohne Angaben zum Zulassungsdienst aufgerufen."
                Else
                    Dim rows() As DataRow = tblResultTableRaw.Select("ID = " & Request.QueryString("ID").ToString)
                    Label1.Text = rows(0)("NAME1").ToString
                    Label2.Text = rows(0)("ANSPRECHPARTNER").ToString
                    Label3.Text = rows(0)("NAME1").ToString
                    Label4.Text = rows(0)("NAME2").ToString
                    Label5.Text = rows(0)("STREET").ToString
                    Label6.Text = rows(0)("HOUSE_NUM1").ToString
                    Label7.Text = rows(0)("POST_CODE1").ToString
                    Label8.Text = rows(0)("CITY1").ToString
                    Label9.Text = rows(0)("TELE1").ToString
                    Label10.Text = rows(0)("TELE2").ToString
                    Label11.Text = rows(0)("TELE3").ToString
                    Label12.Text = rows(0)("FAX_NUMBER").ToString
                    Label13.Text = rows(0)("SMTP_ADDR").ToString
                    Label14.Text = rows(0)("ZTXT1").ToString
                    Label15.Text = rows(0)("ZTXT2").ToString
                    Label16.Text = rows(0)("ZTXT3").ToString
                    Label17.Text = rows(0)("BEMERKUNG").ToString
                End If
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

End Class