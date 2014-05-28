Imports System.Security.Cryptography
Imports System.IO
Imports System.Xml
Imports System.Text
Public Class InsConfirm
    Inherits System.Web.UI.Page

    Private ValKey As String = String.Empty

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString.Item("key") Is Nothing Then
            lblError.Text = "Diese Seite kann nur mit einem entsprechenden Schlüssel aufgerufen werden."
        Else

            Dim KeyUrl As String = Request.QueryString.Item("key").ToString

            If IsPostBack = False Then
                ValidateKey(KeyUrl)
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
    Private Sub ValidateKey(ByVal KeyValue As String)

        Dim Decrypt As New Security


        KeyValue = Decrypt.psDecrypt(KeyValue)


        If KeyValue.Length = 15 Then
            lblInfo.Visible = True
            lblVermittlernummer.Visible = True
            cmdConfirm.Visible = True
            ValKey = KeyValue
            Session.Add("ValKey", ValKey)
        Else
            lblAusgabe.Visible = False
            txtVermittlernummer.Visible = False
            cmdConfirm.Visible = False
            lblError.Text = "Ihr Link enthält einen ungültigen Schlüssel."
        End If

    End Sub
 
#End Region

    Protected Sub cmdConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdConfirm.Click

        lblError.Text = ""
        Dim Vermittlernummer As String = ""
        Dim sNummerTrenn As String = ""

        For i As Integer = 1 To txtVermittlernummer.Text.Length


            If IsNumeric(Mid(txtVermittlernummer.Text, i, 1)) = True Then
                sNummerTrenn = txtVermittlernummer.Text
                Vermittlernummer &= Mid(txtVermittlernummer.Text, i, 1)
            End If

        Next
        ValKey = Session("ValKey").ToString
        If Vermittlernummer.Length <> 9 Then
            lblError.Text = "Die Agenturnummer wurde nicht 9-stellig eingegeben."
        Else
            If Vermittlernummer <> Left(ValKey, 9) Then
                lblError.Text = "Die Agenturnummer ist nicht korrekt."
            Else
                'ToDo Zugriff auf SAP

                Dim Freigabe As New App(True)

                Freigabe.SetFreigabe(Vermittlernummer, Right(ValKey, 6))

                If Freigabe.booError = True Then
                    'lblVermittlernummer.Visible = False
                    'txtVermittlernummer.Visible = False
                    'cmdConfirm.Visible = False
                    lblError.Text = Freigabe.m_Message
                Else
                    lblInfo.Text = Freigabe.m_Message
                    lblVermittlernummer.Visible = False
                    txtVermittlernummer.Visible = False
                    cmdConfirm.Visible = False
                End If

            End If
        End If
    End Sub
End Class
' ************************************************
' $History: InsConfirm.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 24.11.09   Time: 13:05
' Updated in $/Validation/Validation/Forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 23.11.09   Time: 17:42
' Updated in $/Validation/Validation/Forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 22.10.09   Time: 14:24
' Updated in $/Validation/Validation/Forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 8.10.09    Time: 8:42
' Updated in $/Validation/Validation/Forms
' ITA: 3132
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 6.10.09    Time: 17:18
' Created in $/Validation/Validation/Forms
' ITA: 3132
'