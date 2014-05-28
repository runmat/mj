Namespace Kernel.Security
    <Serializable()> Public Class Contact
        REM § Enthält Kontaktinformationen zur Anzeige für den angemeldeten Benutzer, die aus Kunde bzw. Gruppe ermittelt werden. 

#Region " Membervariables "
        Private m_strName As String
        Private m_strAddress As String
        Private m_strMailDisplay As String
        Private m_strMail As String
        Private m_strWebDisplay As String
        Private m_strWeb As String
        Private m_strKundenpostfach As String
        Private m_strKundenhotline As String
        Private m_strKundenfax As String
#End Region

#Region " Constructor "
        Public Sub New(ByVal strName As String, _
                       ByVal strAddress As String, _
                       ByVal strMailDisplay As String, _
                       ByVal strMail As String, _
                       ByVal strWebDisplay As String, _
                       ByVal strWeb As String, _
                       ByVal strKundePost As String, _
                       ByVal strKundehotline As String, _
                       ByVal strKundefax As String)
            m_strName = strName
            m_strAddress = strAddress
            m_strMailDisplay = strMailDisplay
            m_strMail = strMail
            m_strWebDisplay = strWebDisplay
            m_strWeb = strWeb
            m_strKundenpostfach = strKundePost
            m_strKundenhotline = strKundehotline
            m_strKundenfax = strKundefax
        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property Name() As String
            Get
                Return m_strName
            End Get
        End Property

        Public ReadOnly Property Address() As String
            Get
                Return m_strAddress
            End Get
        End Property

        Public ReadOnly Property MailDisplay() As String
            Get
                Return m_strMailDisplay
            End Get
        End Property

        Public ReadOnly Property Mail() As String
            Get
                Return m_strMail
            End Get
        End Property
        Public ReadOnly Property WebDisplay() As String
            Get
                Return m_strWebDisplay
            End Get
        End Property

        Public ReadOnly Property Web() As String
            Get
                Return m_strWeb
            End Get
        End Property

        Public ReadOnly Property Kundenpostfach() As String
            Get
                Return m_strKundenpostfach
            End Get
        End Property

        Public ReadOnly Property Kundenhotline() As String
            Get
                Return m_strKundenhotline
            End Get
        End Property

        Public ReadOnly Property Kundenfax() As String
            Get
                Return m_strKundenfax
            End Get
        End Property
#End Region

#Region " Functions "
        Public Function GetMailHyperLink() As System.Web.UI.WebControls.HyperLink
            Dim _lnk As New System.Web.UI.WebControls.HyperLink()
            _lnk.Text = m_strMailDisplay
            _lnk.NavigateUrl = m_strMail
            _lnk.Target = "_blank"
            Return _lnk
        End Function

        Public Function GetWebHyperLink() As System.Web.UI.WebControls.HyperLink
            Dim _lnk As New System.Web.UI.WebControls.HyperLink()
            _lnk.Text = m_strWebDisplay
            _lnk.NavigateUrl = m_strWeb
            _lnk.Target = "_blank"
            Return _lnk
        End Function

        Public Function CombineWith(ByVal _cIn As Contact) As Contact
            Dim strName As String = Name
            Dim strAddress As String = Address
            Dim strMailDisplay As String = MailDisplay
            Dim strMail As String = Mail
            Dim strWebDisplay As String = WebDisplay
            Dim strWeb As String = Web
            Dim strKundePostf As String = Kundenpostfach
            Dim strKundeHotl As String = Kundenhotline
            Dim strKundeFax As String = Kundenfax
            With _cIn
                If Not .Name = String.Empty Then
                    strName = .Name
                End If
                If Not .Address = String.Empty Then
                    strAddress = .Address
                End If
                If Not .MailDisplay = String.Empty Then
                    strMailDisplay = .MailDisplay
                End If
                If Not .Mail = String.Empty Then
                    strMail = .Mail
                End If
                If Not .WebDisplay = String.Empty Then
                    strWebDisplay = .WebDisplay
                End If
                If Not .Web = String.Empty Then
                    strWeb = .Web
                End If
                If Not .Kundenpostfach = String.Empty Then
                    strKundePostf = .Kundenpostfach
                End If
                If Not .Kundenhotline = String.Empty Then
                    strKundeHotl = .Kundenhotline
                End If
                If Not .Kundenfax = String.Empty Then
                    strKundeFax = .Kundenfax
                End If
            End With

            Return New Contact(strName, strAddress, strMailDisplay, strMail, strWebDisplay, strWeb, strKundePostf, strKundeHotl, strKundeFax)
        End Function
#End Region

    End Class
End Namespace

' ************************************************
' $History: Contact.vb $
' 
' *****************  Version 3  *****************
' User: Dittbernerc  Date: 9.05.11    Time: 13:39
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 3  *****************
' User: Dittbernerc  Date: 3.05.11    Time: 10:55
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 26.10.09   Time: 11:44
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' ************************************************