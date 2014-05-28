Imports CKG.Base.Kernel

Partial Public Class Kopfdatenhaendler
    Inherits System.Web.UI.UserControl
    Private objUser As Security.User
    Private objApp As Security.App

    Private m_strHaendlerNummer As String
    Private m_strHaendlerName As String
    Private m_strAdresse As String
    Private m_tblKontingente As DataTable
    Private m_strMessage As String
    Private m_blnGesperrt As Boolean
    Private m_strUserReferenz As String


    Public Property UserReferenz() As String
        Get
            If Not m_strUserReferenz Is Nothing Then
                Return m_strUserReferenz
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            m_strUserReferenz = Value
        End Set
    End Property

    Public Property HaendlerNummer() As String
        Get
            If Not m_strHaendlerNummer Is Nothing Then
                Return m_strHaendlerNummer
            Else
                Return ""
            End If
        End Get
        Set(ByVal Value As String)
            m_strHaendlerNummer = Value
            lblHaendlerNummer.Text = m_strHaendlerNummer
        End Set
    End Property

    Public Property Message() As String
        Get
            Return m_strMessage
        End Get
        Set(ByVal Value As String)
            m_strMessage = Value
            lblMessage.Text = m_strMessage & "<br>"
            lblMessage.CssClass = "TextNormal"      'HEZ?
        End Set
    End Property

    Public Property MessageError() As String
        Get
            Return m_strMessage
        End Get
        Set(ByVal Value As String)
            m_strMessage = Value
            lblMessage.Text = m_strMessage & "<br>"
            lblMessage.CssClass = "TextError"
        End Set
    End Property

    Public Property HaendlerName() As String
        Get
            Return m_strHaendlerName
        End Get
        Set(ByVal Value As String)
            m_strHaendlerName = Value
            lblHaendlerName.Text = m_strHaendlerName
        End Set
    End Property

    Public Property Adresse() As String
        Get
            Return m_strAdresse
        End Get
        Set(ByVal Value As String)
            m_strAdresse = Value
            lblAdresse.Text = m_strAdresse
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class
' ************************************************
' $History: Kopfdatenhaendler.ascx.vb $
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 18.06.08   Time: 14:33
' Created in $/CKAG/Applications/AppFFE/PageElements
' Ausblenden Händler Kontingente
' 
' ************************************************
