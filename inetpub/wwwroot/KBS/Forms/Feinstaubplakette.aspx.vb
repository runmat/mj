Imports System.IO
Imports KBS.KBS_BASE

Public Class Feinstaubplakette
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)

        lblError.Text = ""
        Title = lblHead.Text
    End Sub

    Protected Sub cmdCreate_Click(sender As Object, e As EventArgs) Handles cmdCreate.Click

        Try
            If String.IsNullOrEmpty(txtKennzeichen.Text) Then
                lblError.Text = "Bitte geben Sie ein Kennzeichen an, das gedruckt werden soll"
                Exit Sub
            End If

            Dim strServerIp As String = ConfigurationManager.AppSettings("FSPDruckerIP")
            Dim strUsername As String = ConfigurationManager.AppSettings("FSPDruckerUsername")
            Dim strPassword As String = ConfigurationManager.AppSettings("FSPDruckerPassword")
            Dim strLabelFile As String = ConfigurationManager.AppSettings("FSPLabelFilePath")

            If String.IsNullOrEmpty(strServerIp) Then
                ' wenn Drucker-IP nicht angegeben -> Annahme, dass Drucker-IP = xxx.xxx.xxx.40 im Subnet des Clients
                Dim adressTeile() As String = Request.UserHostAddress.Split(":"c)
                strServerIp = adressTeile(0).PadLeft(1, "0"c) & ":" & adressTeile(1).PadLeft(1, "0"c) & ":" & adressTeile(2).PadLeft(1, "0"c) & ":40"
            End If

            Dim strServerUrl As String = "ftp://" & strUsername & ":" & strPassword & "@" & strServerIp

            ' Labeldatei erzeugen
            Using writer As New StreamWriter(strLabelFile, False)
                writer.WriteLine("mm")
                writer.WriteLine("zO")
                writer.WriteLine("J")
                writer.WriteLine("H 40,0,T")
                writer.WriteLine("O R,T,S,P")
                writer.WriteLine("S l2;0,0,96,96,100")
                writer.WriteLine("T 80,22,180,3,10.02,b,q75;" & txtKennzeichen.Text.ToUpper() & "[J:c60]")
                writer.WriteLine("A1")
                writer.Close()
            End Using

            ' Label-Datei an Drucker senden
            Dim sh As New Shell32.Shell()
            Dim folder = sh.NameSpace(strServerUrl)
            folder.CopyHere(strLabelFile)

            lblError.Text = "Ihr Druckauftrag wurde an den Drucker gesendet."

        Catch ex As Exception
            lblError.Text = "Fehler beim Senden des Druckauftrags (Client-IP: " & Request.UserHostAddress & "): " & ex.Message

        End Try

    End Sub

End Class