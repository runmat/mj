<CLSCompliant(False)> Public Class __Report022
    Inherits System.Web.UI.Page
    Protected WithEvents lblDatum As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim str As String
        Dim strMain As String
        Dim lfdnr As String
        Dim fpath As String
        Dim fname As String
        Dim tstamp As String
        Dim mail As String
        Dim strOut As String

        mail = String.Empty
        If Not IsPostBack Then
            lblDatum.Text = Now.ToString

            If (Not Request.QueryString("USER") Is Nothing) Then
                lblDatum.Text = "<STRONG>" & lblDatum.Text & "</STRONG><br><STRONG>Benutzer:&nbsp;</STRONG>" & Request.QueryString("USER").ToString
            End If

            strOut = String.Empty
            Try
                'If Not (Request.QueryString("PAR") Is Nothing) Then
                If Not Session("Status") Is Nothing Then
                    'strMain = Request.QueryString("PAR").ToString
                    'strMain = strMain.Replace("'", "\")
                    strMain = Session("Status").ToString
                    strMain = strMain.Replace("'", "\")

                    While strMain.IndexOf(";") >= 0
                        str = Left(strMain, strMain.IndexOf(";"))

                        If (str.IndexOf("*") >= 0) Then
                            mail = Right(str, str.Length - str.LastIndexOf("*") - 1)
                            str = Left(str, str.IndexOf("*"))
                        End If

                        lfdnr = Right("0000" & Left(str, str.IndexOf("#")), 4)
                        str = Right(str, str.Length - str.IndexOf("#") - 1)
                        fpath = Left(str, str.IndexOf("#"))
                        str = Right(str, str.Length - str.IndexOf("#") - 1)
                        fname = Left(str, str.IndexOf("#"))
                        str = Right(str, str.Length - str.IndexOf("#") - 1)
                        tstamp = str

                        'strOut &= "<IMG height=""75"" width=""75"" src=""" & fpath & fname & """><br>" & lfdnr & "&nbsp;-&nbsp;" & tstamp & "&nbsp;-&nbsp;" & fname & "<br>"
                        strOut &= lfdnr & "&nbsp;-&nbsp;" & tstamp & "&nbsp;-&nbsp;" & fname & "<br>"
                        If (mail <> String.Empty) Then
                            strOut &= "-Mail versendet an:" & mail & "<br>"
                        End If

                        strMain = Right(strMain, strMain.Length - strMain.IndexOf(";") - 1)
                    End While
                End If
                lblInfo.Text = strOut
                Session("Status") = Nothing
            Catch ex As Exception
                lblError.Text = ex.Message
            End Try
        End If
    End Sub
End Class

' ************************************************
' $History: _Report022.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 23.09.10   Time: 15:33
' Updated in $/CKAG/Applications/appdcl/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 22.10.09   Time: 14:37
' Updated in $/CKAG/Applications/appdcl/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 9:25
' Updated in $/CKAG/Applications/appdcl/Forms
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:00
' Created in $/CKAG/Applications/appdcl/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 10:26
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Forms
' 
' ************************************************
