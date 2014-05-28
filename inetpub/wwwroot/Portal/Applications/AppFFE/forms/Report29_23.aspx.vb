Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Report29_23
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As FFE_Search
    Private objHaendler As FFE_Haendler
    Private objFDDBank As FFE_BankBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User)

        ucHeader.Visible = False
        Session("SelectedDealer") = Request.QueryString("Kunnr").ToString

        Try
            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Report29.aspx?AppID=" & Session("AppID").ToString)
            End If

            lblHead.Text = GetTextForLabelHead(Request.UrlReferrer.ToString())

            objSuche = CType(Session("objSuche"), FFE_Search)
            If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
                Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
                Dim strTemp As String = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    strTemp &= "<br>" & objSuche.NAME_2
                End If
                Kopfdaten1.HaendlerName = strTemp
                Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
            Else
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            End If

            If (Session("objHaendler") Is Nothing) OrElse (Not IsPostBack) Then
                objHaendler = New FFE_Haendler(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", "60" & objSuche.REFERENZ, , True)
            Else
                objHaendler = CType(Session("objHaendler"), FFE_Haendler)
            End If

            If objHaendler.Status = 0 Then
                If Not IsPostBack Then
                    Kopfdaten1.Kontingente = objHaendler.Kontingente
                End If
            Else
                lblError.Text = "Fehler bei der Ermittlung der Kontingentdaten.<br>(" & objHaendler.Message & ")"
            End If

            Session("objHaendler") = objHaendler
            objFDDBank = CType(Session("objFDDBank"), FFE_BankBase)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Function GetTextForLabelHead(ByVal urlReferrer As String) As String
        If urlReferrer.IndexOf("AppFFD/Forms/Report30_01.aspx") >= 0 Then
            Return "Händlerstatus"
        Else
            Return "Überzogene Kontingente (Details)"
        End If
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
' ************************************************
' $History: Report29_23.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
