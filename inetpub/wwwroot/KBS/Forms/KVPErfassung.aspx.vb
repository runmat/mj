Imports System
Imports KBS.KBS_BASE

Public Class KVPErfassung
    Inherits Page

#Region "Enumeratoren"

    Private Enum ViewStatus
        Bedienerkarte
        KVPErfassung
    End Enum

#End Region

    Private mObjKasse As Kasse
    Private mObjKVP As KVP
    Private strBedienernummer As String

    Private mpKBS As KBS

    Protected WithEvents btnOKmaster As LinkButton
    Protected WithEvents btnCancelmaster As LinkButton

    Protected Sub Page_Init() Handles Me.Init
        'PopUp aus Masterpage laden
        btnOKmaster = Master.FindControl("btnPanelErrorOk")
        btnCancelmaster = Master.FindControl("btnPanelErrorCancel")
        mpKBS = Master
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)
        If Not Session("mKasse") Is Nothing Then
            mObjKasse = Session("mKasse")
        End If

        lblError.Text = ""
        Title = lblHead.Text

        If Not IsPostBack Then
            ViewControl(ViewStatus.Bedienerkarte)
        Else
            Dim eventArg As String = Request("__EVENTARGUMENT")
            If eventArg = "MyCustomArgument" Then
                txtBedienerkarte_TextChanged()
            End If

            If Session("ObjKVP") IsNot Nothing Then
                mObjKVP = CType(Session("ObjKVP"), KVP)
            End If

        End If

        txtBedienerkarte.Attributes.Add("onkeyup", "javascript:ControlField(this);")
        Session("LastPage") = Me
    End Sub

    Private Function CheckBedienerKarte() As Boolean

        If txtBedienerkarte.Text = String.Empty Then
            lblBedienError.Text = "Bitte lesen Sie die Bedienerkarte ein!"
            Return False
        ElseIf txtBedienerkarte.Text.Length <> 15 Then
            lblBedienError.Text = "Fehler beim einlesen der Bedienerkarte. Barcode hat die falsche Länge!"
            Return False
        Else
            Try
                Dim strCode As String
                Dim strBediener As String
                strCode = Left(txtBedienerkarte.Text, 14)
                strCode = Right(strCode, 13)
                strBediener = strCode.Substring(3, 1)
                strBediener &= strCode.Substring(6, 1)
                strBediener &= strCode.Substring(8, 1)
                strBediener &= strCode.Substring(11, 1)
                strBedienernummer = strBediener
                Return True
            Catch ex As Exception
                lblBedienError.Text = "Fehler beim einlesen der Bedienerkarte. Versuchen Sie es nochmal!"
                Return False
            End Try
        End If

    End Function

    Public Sub txtBedienerkarte_TextChanged()
        mObjKVP = New KVP()

        If CheckBedienerKarte() Then
            Try
                mObjKVP.KVPLogin(mObjKasse.Lagerort, strBedienernummer)

                With mObjKVP
                    If .ErrorOccured Then
                        ShowPopUp("Fehler!", .ErrorMessage, MessageType.ErrorText)
                    Else
                        txtVorschlagVon.Text = .Benutzername
                        txtStandort.Text = .Standort
                        txtVorgesetzter.Text = .Vorgesetzter
                        txtKST.Text = .Kostenstelle

                        ViewControl(ViewStatus.KVPErfassung)

                        If Not String.IsNullOrEmpty(.GeparkteKVPId) AndAlso .GeparkteKVPId.Trim().Length > 0 Then
                            .LoadKVP(.GeparkteKVPId)

                            If Not .ErrorOccured Then
                                txtBeschreibung.Text = .Kurzbeschreibung
                                txtSituation.Text = .SituationText
                                txtVeraenderung.Text = .VeraenderungText
                                txtVorteil.Text = .VorteilText
                                btnVerwerfen.Visible = True
                            Else
                                ShowPopUp("Fehler!", "Fehler beim KVP-Laden: " & .ErrorMessage, MessageType.ErrorText)
                            End If
                        End If
                    End If
                End With

                Session("ObjKVP") = mObjKVP

            Catch ex As Exception
                ShowPopUp("Fehler!", ex.Message, MessageType.ErrorText)
            End Try

        End If
    End Sub

    Protected Sub btnParken_Click(sender As Object, e As EventArgs) Handles btnParken.Click
        If SaveKVP(True) Then
            Session("ObjKVP") = Nothing
            Response.Redirect("../Selection.aspx")
        End If
    End Sub

    Protected Sub btnVerwerfen_Click(sender As Object, e As EventArgs) Handles btnVerwerfen.Click
        mpeConfirmDelete.Show()
    End Sub

    Protected Sub btnSenden_Click(sender As Object, e As EventArgs) Handles btnSenden.Click
        SaveKVP()
    End Sub

    ''' <summary>
    ''' Ansichtssteuerung für alle Elemente der Seite
    ''' </summary>
    ''' <param name="VS">Das anzuzeigende Seiten-Layout</param>
    ''' <remarks></remarks>
    Private Sub ViewControl(ByVal VS As ViewStatus)
        Select Case VS

            Case ViewStatus.Bedienerkarte
                tblBedienerkarte.Visible = True
                Erfassung.Visible = False
                txtBedienerkarte.Focus()

            Case ViewStatus.KVPErfassung
                tblBedienerkarte.Visible = False
                Erfassung.Visible = True
                txtBeschreibung.Focus()

        End Select

    End Sub

    Private Function SaveKVP(Optional ByVal nurParken As Boolean = False) As Boolean
        Try
            If Not nurParken Then
                'Zum Absenden müssen alle Textfelder gefüllt sein
                If String.IsNullOrEmpty(txtBeschreibung.Text) OrElse String.IsNullOrEmpty(txtSituation.Text) OrElse _
                        String.IsNullOrEmpty(txtVeraenderung.Text) OrElse String.IsNullOrEmpty(txtVorteil.Text) Then
                    ShowPopUp("Fehler!", "Bitte füllen Sie alle Felder aus!", MessageType.ErrorText)
                    Return False
                End If
            End If

            mObjKVP.Kurzbeschreibung = txtBeschreibung.Text
            mObjKVP.SituationText = txtSituation.Text
            mObjKVP.VeraenderungText = txtVeraenderung.Text
            mObjKVP.VorteilText = txtVorteil.Text

            mObjKVP.SaveKVP(nurParken)

            If nurParken Then
                If Not mObjKVP.ErrorOccured Then
                    lblError.Text = "KVP erfolgreich geparkt"
                    btnVerwerfen.Visible = True
                Else
                    ShowPopUp("Fehler!", "Fehler beim KVP-Parken: " & mObjKVP.ErrorMessage, MessageType.ErrorText)
                End If
            Else
                If Not mObjKVP.ErrorOccured Then
                    ClearKVPFields()
                    ShowPopUp("Info", "Ihr KVP wurde erfolgreich versendet!", MessageType.NotificationText)
                Else
                    ShowPopUp("Fehler!", "Fehler beim KVP-Senden: " & mObjKVP.ErrorMessage, MessageType.ErrorText)
                End If
            End If

            Session("ObjKVP") = mObjKVP

            Return (Not mObjKVP.ErrorOccured)

        Catch ex As Exception
            If nurParken Then
                ShowPopUp("Fehler!", "Fehler beim KVP-Parken: " & ex.Message, MessageType.ErrorText)
            Else
                ShowPopUp("Fehler!", "Fehler beim KVP-Senden: " & ex.Message, MessageType.ErrorText)
            End If
            Return False
        End Try
    End Function

    Private Sub DeleteKVP()
        Try
            mObjKVP.DeleteKVP()

            If Not mObjKVP.ErrorOccured Then
                ClearKVPFields()
                lblError.Text = "KVP erfolgreich verworfen"
            Else
                ShowPopUp("Fehler!", "Fehler beim KVP-Verwerfen: " & mObjKVP.ErrorMessage, MessageType.ErrorText)
            End If

            Session("ObjKVP") = mObjKVP

        Catch ex As Exception
            ShowPopUp("Fehler!", "Fehler beim KVP-Verwerfen: " & ex.Message, MessageType.ErrorText)
        End Try
    End Sub

    Private Sub ClearKVPFields()
        txtBeschreibung.Text = ""
        txtSituation.Text = ""
        txtVeraenderung.Text = ""
        txtVorteil.Text = ""
    End Sub

    Protected Sub btnPanelQuestionOK_Clicked(ByVal sender As Object, ByVal e As EventArgs) Handles btnPanelConfirmDeleteOK.Click
        mpeConfirmDelete.Hide()
        DeleteKVP()
    End Sub

    Protected Sub btnPanelQuestionCancel_Clicked(ByVal sender As Object, ByVal e As EventArgs) Handles btnPanelConfirmDeleteCancel.Click
        mpeConfirmDelete.Hide()
    End Sub

    Protected Sub lb_zurueck_Click(sender As Object, e As EventArgs) Handles lb_zurueck.Click
        Session("ObjKVP") = Nothing
        Response.Redirect("../Selection.aspx")
    End Sub

    Private Sub ShowPopUp(ByVal HeaderText As String, ByVal Text As String, Optional ByVal TextType As MessageType = MessageType.FlatText)
        mpKBS.ShowErrorPopUp(HeaderText, Text, TextType)
    End Sub

    Protected Sub btnOK_Click() Handles btnOKmaster.Click
        Session("bPanelCancel") = False
        Session("bPanelOK") = True
    End Sub

    Protected Sub btnCancel_Click() Handles btnCancelmaster.Click
        Session("bPanelCancel") = True
        Session("bPanelOK") = False
    End Sub

End Class