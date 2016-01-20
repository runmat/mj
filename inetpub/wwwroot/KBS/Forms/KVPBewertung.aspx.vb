Imports System
Imports KBS.KBS_BASE
Imports Telerik.Web.UI

Public Class KVPBewertung
    Inherits Page

#Region "Enumeratoren"

    Private Enum ViewStatus
        Bedienerkarte
        KVPBewertungsliste
        KVPBewertung
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
        lblNoData.Text = ""
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

                If mObjKVP.ErrorOccured Then
                    ShowPopUp("Fehler!", mObjKVP.ErrorMessage, MessageType.ErrorText)
                Else
                    ViewControl(ViewStatus.KVPBewertungsliste)
                    LadeBewertungen()
                End If

                Session("ObjKVP") = mObjKVP

            Catch ex As Exception
                ShowPopUp("Fehler!", ex.Message, MessageType.ErrorText)
            End Try

        End If
    End Sub

    Private Sub LadeBewertungen()
        mObjKVP.LoadBewertungen()
        Session("ObjKVP") = mObjKVP

        If Not mObjKVP.ErrorOccured Then
            FillGrid()
        Else
            lblError.Text = "Fehler beim Laden der Bewertungsliste: " & mObjKVP.ErrorMessage
        End If
    End Sub

    Private Sub FillGrid()
        If mObjKVP.Vorschlagsliste IsNot Nothing AndAlso mObjKVP.Vorschlagsliste.Rows.Count > 0 Then
            rgGrid1.Visible = True
            rgGrid1.Rebind()
            'Setzen der DataSource geschieht durch das NeedDataSource-Event
        Else
            rgGrid1.Visible = False
            lblNoData.Text = "Keine Daten gefunden"
        End If
    End Sub

    Protected Sub rgGrid1_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles rgGrid1.NeedDataSource
        If mObjKVP.Vorschlagsliste IsNot Nothing Then
            rgGrid1.DataSource = mObjKVP.Vorschlagsliste.DefaultView
        Else
            rgGrid1.DataSource = Nothing
        End If
    End Sub

    Protected Sub rgGrid1_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles rgGrid1.ItemCommand
        If TypeOf e.Item Is GridDataItem Then
            Dim gridRow As GridDataItem = CType(e.Item, GridDataItem)

            If e.CommandName = "bewerten" Then
                mObjKVP.SelectKVPForBewertung(gridRow("KVPID").Text)
                LadeKVP()
                Session("ObjKVP") = mObjKVP

                ViewControl(ViewStatus.KVPBewertung)
            End If
        End If
    End Sub

    Protected Sub btnLike_Click(sender As Object, e As EventArgs) Handles btnLike.Click
        mObjKVP.BewertungPositiv = True
        mObjKVP.BewertungNegativ = False
        Session("ObjKVP") = mObjKVP
        lblBewertung.Text = btnLike.Text
        mpeConfirmBewertung.Show()
    End Sub

    Protected Sub btnDontLike_Click(sender As Object, e As EventArgs) Handles btnDontLike.Click
        mObjKVP.BewertungPositiv = False
        mObjKVP.BewertungNegativ = True
        Session("ObjKVP") = mObjKVP
        lblBewertung.Text = btnDontLike.Text
        mpeConfirmBewertung.Show()
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
                Bewertungsliste.Visible = False
                Bewertung.Visible = False
                txtBedienerkarte.Focus()

            Case ViewStatus.KVPBewertungsliste
                tblBedienerkarte.Visible = False
                Bewertungsliste.Visible = True
                Bewertung.Visible = False

            Case ViewStatus.KVPBewertung
                tblBedienerkarte.Visible = False
                Bewertungsliste.Visible = False
                Bewertung.Visible = True

        End Select

    End Sub

    Private Sub LadeKVP()
        With mObjKVP
            .LoadKVP(.AktuelleKVPId, True)

            If Not .ErrorOccured Then
                txtBeschreibung.Text = .Kurzbeschreibung
                txtSituation.Text = .SituationText
                txtVeraenderung.Text = .VeraenderungText
                txtVorteil.Text = .VorteilText
                txtBewertungsfrist.Text = .Bewertungsfrist
                btnLike.Enabled = True
                btnDontLike.Enabled = True
            Else
                btnLike.Enabled = False
                btnDontLike.Enabled = False
                lblError.Text = "Fehler beim KVP-Laden: " & .ErrorMessage
            End If
        End With
    End Sub

    Private Sub BewerteKVP()
        Try
            mObjKVP.SaveBewertung()

            If Not mObjKVP.ErrorOccured Then
                btnLike.Enabled = False
                btnDontLike.Enabled = False
                ClearKVPFields()
                lblError.Text = "Bewertung erfolgreich gespeichert"
                Session("ObjKVP") = mObjKVP
                ViewControl(ViewStatus.KVPBewertungsliste)
                LadeBewertungen()
            Else
                lblError.Text = "Fehler beim Speichern der Bewertung: " & mObjKVP.ErrorMessage
            End If

        Catch ex As Exception
            lblError.Text = "Fehler beim Speichern der Bewertung: " & ex.Message
        End Try
    End Sub

    Private Sub ClearKVPFields()
        txtBeschreibung.Text = ""
        txtSituation.Text = ""
        txtVeraenderung.Text = ""
        txtVorteil.Text = ""
        txtBewertungsfrist.Text = ""
    End Sub

    Protected Sub btnPanelConfirmBewertungOK_Clicked(ByVal sender As Object, ByVal e As EventArgs) Handles btnPanelConfirmBewertungOK.Click
        mpeConfirmBewertung.Hide()
        BewerteKVP()
    End Sub

    Protected Sub btnPanelConfirmBewertungCancel_Clicked(ByVal sender As Object, ByVal e As EventArgs) Handles btnPanelConfirmBewertungCancel.Click
        mpeConfirmBewertung.Hide()
        mObjKVP.BewertungPositiv = False
        mObjKVP.BewertungNegativ = False
        Session("ObjKVP") = mObjKVP
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