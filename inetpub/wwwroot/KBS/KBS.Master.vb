Public Enum MessageType
    FlatText
    ErrorText
    NotificationText
End Enum

Partial Public Class KBS
    Inherits MasterPage
    Dim bc As HttpBrowserCapabilities

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender

        'Aktuelles Jahr ins Copyright setzen.
        lblCopyright.Text = lblCopyright.Text.Replace("year", DateTime.Now.Year.ToString)
        For Each Control As Control In Form1.Controls
            If TypeOf (Control) Is LiteralControl Then
                Dim litControl = CType(Control, LiteralControl)
                If litControl.Text.Contains("id=""header""") Then
                    If Not Session("mKasse") Is Nothing Then
                        Dim tmpFirma As String = CType(Session("mKasse"), Kasse).Firma
                        Dim HeaderUrl As String = ""
                        Select Case tmpFirma
                            Case "KBS"
                                HeaderUrl = " style=""background-image: url(../Images/headerKBS2.jpg);"""
                            Case "Kroschke"
                                HeaderUrl = " style=""background-image: url(../Images/headerCK2.jpg);"""
                            Case "Kroschke Partner"
                                HeaderUrl = " style=""background-image: url(../Images/headerCK2.jpg);"""
                            Case "nicht Kroschke"
                                HeaderUrl = " style=""background-image: url(../Images/header.jpg);"""
                                lblCopyright.Visible = False
                            Case Else
                                Throw New Exception("Die Kasse " & CType(Session("mKasse"), Kasse).IP & "  gehört zu keiner gültigen Firma")
                        End Select
                        litControl.Text = litControl.Text.Insert(litControl.Text.IndexOf("id=""header""") + 11, HeaderUrl)
                        Exit For
                    Else
                        Throw New Exception("benötigtes Session Objekt nicht vorhanden")
                    End If
                End If
            End If
        Next

        bc = Request.Browser
        Dim strCSSLink As String = ""
        With bc
            If .Type = "IE6" Then
                strCSSLink &= " <link href=""../Styles/defaultIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                strCSSLink &= "<link href=""../Styles/KBSIE6.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"

            Else
                strCSSLink &= " <link href=""../Styles/default.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"
                strCSSLink &= "<link href=""../Styles/KBS.css"" media=""screen, projection"" type=""text/css"" rel=""stylesheet"" />"

            End If
        End With
        Head1.Controls.Add(New LiteralControl(strCSSLink))

        If HttpContext.Current.Request.UserAgent IsNot Nothing AndAlso HttpContext.Current.Request.UserAgent.ToLower().Contains("msie 10") Then
            Head1.Controls.AddAt(0, New LiteralControl("<META content=""IE=9,chrome=1"" http-equiv=""X-UA-Compatible"">"))
        End If
    End Sub

    Public Sub ShowErrorPopUp(ByVal Headline As String, ByVal Text As String, Optional ByVal TextType As MessageType = MessageType.FlatText)
        lblPanelErrorHeadline.Text = Headline
        lblPanelError.Text = Text

        Select Case TextType
            Case MessageType.FlatText
                'ShowElements
                btnPanelErrorCancel.Visible = True

                'ElementsLook
                lblPanelErrorHeadline.ForeColor = Drawing.Color.Black
            Case MessageType.ErrorText
                'ShowElements
                btnPanelErrorCancel.Visible = False

                'ElementsLook
                lblPanelErrorHeadline.ForeColor = Drawing.Color.Red
            Case MessageType.NotificationText
                'ShowElements
                btnPanelErrorCancel.Visible = False

                'ElementsLook
                lblPanelErrorHeadline.ForeColor = Drawing.Color.DarkGoldenrod
        End Select

        mpeError.Show()
    End Sub

End Class

' ************************************************
' $History: KBS.Master.vb $
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 28.03.11   Time: 14:03
' Updated in $/CKAG2/KBS
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 3.08.10    Time: 16:46
' Updated in $/CKAG2/KBS
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 7.04.10    Time: 14:28
' Updated in $/CKAG2/KBS
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 6.04.10    Time: 17:15
' Updated in $/CKAG2/KBS
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 6.04.10    Time: 13:01
' Updated in $/CKAG2/KBS
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 3.06.09    Time: 11:38
' Updated in $/CKAG2/KBS
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 7.05.09    Time: 13:08
' Updated in $/CKAG2/KBS
' ITA 2808 
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 7.05.09    Time: 10:17
' Updated in $/CKAG2/KBS
' Header Bild Variabel nach firma
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 24.04.09   Time: 9:59
' Updated in $/CKAG2/KBS
' ITA 2808
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 23.04.09   Time: 17:50
' Updated in $/CKAG2/KBS
' ITA 2808
' 
'
' ************************************************