
Public Class InsConfirm
    Inherits Page

    Private _valKey As String = String.Empty

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Request.QueryString.Item("key") Is Nothing Then
            lblError.Text = "Diese Seite kann nur mit einem entsprechenden Schlüssel aufgerufen werden."
        Else

            Dim keyUrl As String = Request.QueryString.Item("key").ToString

            If IsPostBack = False Then
                ValidateKey(keyUrl)
            End If


        End If

    End Sub
#End Region

#Region "Methods"


    '----------------------------------------------------------------------
    ' Methode:      ValidateKey
    ' Autor:        Sfa
    ' Beschreibung: Überprüfung des in der URL übergebenen Schlüssels.
    '               Schlüssel gefunden: Mailversand an User, ansonsten Fehler
    '               meldung
    ' Erstellt am:  20.04.2009
    ' ITA:          2709
    '----------------------------------------------------------------------
    Private Sub ValidateKey(ByVal keyValue As String)

        Dim decrypt As New Security


        keyValue = decrypt.psDecrypt(keyValue)


        If KeyValue.Length = 15 Then
            lblInfo.Visible = True
            lblVermittlernummer.Visible = True
            cmdConfirm.Visible = True
            _valKey = KeyValue
            Session.Add("ValKey", _valKey)
        Else
            lblAusgabe.Visible = False
            txtVermittlernummer.Visible = False
            cmdConfirm.Visible = False
            lblError.Text = "Ihr Link enthält einen ungültigen Schlüssel."
        End If

    End Sub

#End Region

    Protected Sub CmdConfirmClick(ByVal sender As Object, ByVal e As EventArgs) Handles cmdConfirm.Click

        lblError.Text = ""
        Dim vermittlernummer As String = ""

        For i As Integer = 1 To txtVermittlernummer.Text.Length


            If IsNumeric(Mid(txtVermittlernummer.Text, i, 1)) = True Then
                vermittlernummer &= Mid(txtVermittlernummer.Text, i, 1)
            End If

        Next
        _valKey = Session("ValKey").ToString
        If vermittlernummer.Length <> 9 Then
            lblError.Text = "Die Agenturnummer wurde nicht 9-stellig eingegeben."
        Else
            If vermittlernummer <> Left(_valKey, 9) Then
                lblError.Text = "Die Agenturnummer ist nicht korrekt."
            Else


                Dim freigabe As New App()

                freigabe.SetFreigabe(vermittlernummer, Right(_valKey, 6))

                If freigabe.BooError = True Then
                    lblError.Text = freigabe.Message
                Else
                    lblInfo.Text = freigabe.Message
                    lblVermittlernummer.Visible = False
                    txtVermittlernummer.Visible = False
                    cmdConfirm.Visible = False
                End If

            End If
        End If
    End Sub
End Class
