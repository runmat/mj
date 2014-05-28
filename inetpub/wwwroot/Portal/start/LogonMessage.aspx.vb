Public Enum LogonMsgType As Integer
    INFO = 1
    ALERT = 2
    ERR = 0
End Enum


Public Structure LogonMsgData

    Public MsgType As LogonMsgType
    Public MsgTitle As String
    Public MsgText As String

End Structure 'MyStruct 

Public Class LogonMessage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim data As LogonMsgData = CType(Session("LOGONMSGDATA"), LogonMsgData)
        Try

            Select Case data.MsgType
                Case 1
                    imgMessage.ImageUrl = "../Images/Info02_06.jpg"
                Case 2
                    imgMessage.ImageUrl = "../Images/Ausrufezeichen02_10.jpg"
                Case Else
                    imgMessage.ImageUrl = "../Images/Ausrufezeichen02_10.jpg"
            End Select

            lblTitle.Text = data.MsgTitle
            lblMessage.Text = data.MsgText
        Finally

        End Try
    End Sub

End Class
