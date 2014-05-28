Imports CKG
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports CKG.Base.Kernel


Partial Public Class Report01
    Inherits System.Web.UI.Page


    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Dim mObjFristablauf As Fristablauf


#Region " methoden "

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString


            If Page.IsPostBack = False Then
                Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                mObjFristablauf = New Fristablauf(m_User, m_App, strFileName)
                Session.Add("mObjFristablaufSession", mObjFristablauf)
                mObjFristablauf.SessionID = Me.Session.SessionID
                mObjFristablauf.AppID = CStr(Session("AppID"))

                doSubmit()
            End If



        Catch ex As Exception
            lblError.Text = "Fehler beim laden der Seite: " & ex.Message.ToString
        End Try
    End Sub

    Private Sub doSubmit()

        mObjFristablauf.Fill()
        If mObjFristablauf.Status < 0 Then
            lblError.Text = mObjFristablauf.Message
        Else
            If mObjFristablauf.Result.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                'wiso bekomme ich den filename den ich beim erzeugen des Reportobjektes übergeben musste nicht wieder zurück? Property Eingebaut in EC_21 JJ2007.11.21

                Dim objExcelExport As New Excel.ExcelExport()
                Try
                    Excel.ExcelExport.WriteExcel(mObjFristablauf.Result, ConfigurationSettings.AppSettings("ExcelPath") & mObjFristablauf.FileName)
                Catch
                End Try

                Session("ResultTable") = mObjFristablauf.Result
                Session("lnkExcel") = "/Services/Temp/Excel/" & mObjFristablauf.FileName
                Response.Redirect("/Services/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region

End Class

' ************************************************
' $History: Report01.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 15.04.09   Time: 12:38
' Updated in $/CKAG2/Applications/AppAkf/forms
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 15.04.09   Time: 11:01
' Created in $/CKAG2/Applications/AppAkf/forms
'
' ************************************************
