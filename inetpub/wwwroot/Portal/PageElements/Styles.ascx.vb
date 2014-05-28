Imports CKG.Base.Kernel

Namespace PageElements
    Public MustInherit Class Styles
        Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

#Region " Membervariables "
        Private m_User As Security.User
        Private m_strTitleText As String
#End Region

        Public Property TitleText() As String
            Get
                Return m_strTitleText
            End Get
            Set(ByVal Value As String)
                m_strTitleText = Value
            End Set
        End Property

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Put user code to initialize the page here
            If Not Session("objUser") Is Nothing Then
                m_User = CType(Session("objUser"), Security.User)
            End If

            If (Not m_strTitleText Is Nothing) AndAlso (Not m_strTitleText.Trim(" "c).Length = 0) Then
                If Not m_User Is Nothing AndAlso Me.Page.User.Identity.IsAuthenticated Then
                    m_strTitleText = "<TITLE>" & m_User.Customer.CustomerName & " - " & m_strTitleText & "</TITLE>"
                Else
                    m_strTitleText = "<TITLE>" & m_strTitleText & "</TITLE>"
                End If
            End If
            Me.Controls.Add(New LiteralControl(m_strTitleText))

            Dim strCSSLink As String = ""

            If Not m_User Is Nothing AndAlso Me.Page.User.Identity.IsAuthenticated Then
                If m_User.GroupID > 0 Then
                    strCSSLink = m_User.Organization.CssPath
                End If
                If strCSSLink = String.Empty Then
                    strCSSLink = m_User.Customer.CustomerStyle.CssPath

                    'UH 05.03.2007: Entf‰llt, Pfad absolut zu Portal-Verzeichnis
                    'If Request.PhysicalPath.ToUpper.IndexOf("\APPLICATIONS") >= 0 Then
                    '    'Bei Kind-Anwendung eine Ebene tiefer...
                    '    strCSSLink = "../Styles/Styles.css"
                    'End If
                End If
                If Not strCSSLink = String.Empty Then
                    strCSSLink = String.Format("<LINK rel=""stylesheet"" type=""text/css"" href=""{0}"">", strCSSLink)
                Else
                    strCSSLink = "<LINK rel=""stylesheet"" type=""text/css"" href=""../Styles/Styles.css"">"
                End If
            Else
                strCSSLink = "<LINK rel=""stylesheet"" type=""text/css"" href=""../Styles/Styles.css"">"
            End If
            'strCSSLink = vbCrLf & strCSSLink & vbCrLf & "        <STYLE>" & vbCrLf
            'strCSSLink = strCSSLink & "          .tableMain { BEHAVIOR: url(../shared/scroll.htc); FONT-FAMILY: Verdana }" & vbCrLf
            ''strCSSLink = strCSSLink & "          .tableMain { FONT-SIZE: 0.8em; BEHAVIOR: url(scroll.htc); FONT-FAMILY: Verdana }" & vbCrLf
            'strCSSLink = strCSSLink & "          .tableHeader { BACKGROUND-COLOR:#FFFFFF }" & vbCrLf
            'strCSSLink = strCSSLink & "          .tableBody { COLOR:#000000; BACKGROUND-COLOR:#FFFFFF }" & vbCrLf
            ''strCSSLink = strCSSLink & "          .tableBody { COLOR: darkblue; BACKGROUND-COLOR:#EEEEEE }" & vbCrLf
            'strCSSLink = strCSSLink & "        </STYLE>" & vbCrLf
            Me.Controls.Add(New LiteralControl(strCSSLink))

            If HttpContext.Current.Request.UserAgent IsNot Nothing AndAlso HttpContext.Current.Request.UserAgent.ToLower().Contains("msie 10") Then
                Me.Controls.AddAt(0, New LiteralControl("<META content=""IE=9,chrome=1"" http-equiv=""X-UA-Compatible"">"))
            End If
        End Sub

    End Class
End Namespace

' ************************************************
' $History: Styles.ascx.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 15.05.11   Time: 20:22
' Updated in $/CKAG/portal/PageElements
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 15.04.11   Time: 8:55
' Updated in $/CKAG/portal/PageElements
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 28.10.10   Time: 18:03
' Updated in $/CKAG/portal/PageElements
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 22.10.10   Time: 10:21
' Updated in $/CKAG/portal/PageElements
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/portal/PageElements
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:19
' Created in $/CKAG/portal/PageElements
' 
' *****************  Version 6  *****************
' User: Uha          Date: 5.03.07    Time: 14:44
' Updated in $/CKG/Portal/PageElements
' Setzen des Pfades angepasﬂt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 5.03.07    Time: 13:54
' Updated in $/CKG/Portal/PageElements
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:38
' Updated in $/CKG/Portal/PageElements
' 
' ************************************************