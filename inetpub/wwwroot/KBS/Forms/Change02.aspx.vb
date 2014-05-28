Imports KBS.KBS_BASE

Partial Public Class Change02
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjWareneingangspruefung As Wareneingangspruefung

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        FormAuth(Me)
        lblError.Text = ""

        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        Title = lblHead.Text

        If mObjWareneingangspruefung Is Nothing Then
            mObjWareneingangspruefung = mObjKasse.Wareneingangspruefung(Me)
        End If

        If Not IsPostBack Then
            mObjWareneingangspruefung.currentApplikationPage = Me
            mObjWareneingangspruefung.getErwarteteLieferungenFromSAPERP()
            If Not mObjWareneingangspruefung.SAPStatus = 0 Then

                Select Case mObjWareneingangspruefung.SAPStatus
                    Case -1
                        lblError.Text = mObjWareneingangspruefung.SAPStatusText
                    Case Else
                        lblError.Text = "Es ist ein Fehler aufgetreten <br> " & mObjWareneingangspruefung.SAPStatusText
                End Select
                lbWeiter.Visible = False

            Else
                lbWeiter.Visible = True
                lbxBestellungen.DataSource = New DataView(mObjWareneingangspruefung.ErwarteteLieferungen)
                lbxBestellungen.DataBind()
            End If
        End If
    End Sub

    Private Sub responseBack()
        mObjWareneingangspruefung.currentApplikationPage = Nothing
        Response.Redirect("../Selection.aspx")
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub RefillFillBestellungen(ByVal sender As Object, ByVal e As EventArgs) Handles txtBestellnummer.TextChanged, txtLieferantName.TextChanged
        '----------------------------------------------------------------------
        'Methode:      RefillFillBestellungen
        'Autor:         Julian Jung
        'Beschreibung:  Listbox der Bestellungen anhand der Suchkriterien neu befüllen
        'Erstellt am:   05.05.2009
        '----------------------------------------------------------------------

        mObjWareneingangspruefung.BestellnummerSelection = txtBestellnummer.Text.Replace(" ", "")
        mObjWareneingangspruefung.LieferantSelection = txtLieferantName.Text.Replace(" ", "")

        Dim tmpDataView As DataView = New DataView(mObjWareneingangspruefung.ErwarteteLieferungen)
        tmpDataView.RowFilter = mObjWareneingangspruefung.SelectionString

        lbxBestellungen.DataSource = tmpDataView
        lbxBestellungen.DataBind()

    End Sub

    Protected Sub lbWeiter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbWeiter.Click
        If Not lbxBestellungen.SelectedIndex = -1 Then

            Dim drow() As DataRow
            drow = mObjWareneingangspruefung.ErwarteteLieferungen.Select("Bestellnummer='" & lbxBestellungen.SelectedValue & "'")
            If drow.Length > 0 Then
                If drow(0)("IstUmlagerung").ToString = "X" Then
                    mObjWareneingangspruefung.IstUmlagerung = "X"
                    mObjWareneingangspruefung.UmlNummer = lbxBestellungen.SelectedValue
                    mObjWareneingangspruefung.getUmlPositionenFromSAPERP(lbxBestellungen.SelectedValue)
                Else
                    mObjWareneingangspruefung.getLieferungsPositionenFromSAPERP(lbxBestellungen.SelectedValue)
                End If
            Else
                lblError.Text = "Fehler beim lesen der Bestellungdetails!"
            End If

            Select Case mObjWareneingangspruefung.SAPStatus
                Case 0
                    mObjWareneingangspruefung.LiefantAnzeige = lbxBestellungen.SelectedItem.Text

                    mObjWareneingangspruefung.currentApplikationPage = Nothing
                    Response.Redirect("Change02_1.aspx")

                Case -1
                    lblError.Text = mObjWareneingangspruefung.SAPStatusText
                Case Else
                    lblError.Text = "Es ist ein Fehler aufgetreten: <br>" & mObjWareneingangspruefung.SAPStatusText
            End Select
        Else
            lblError.Text = "Wählen Sie bitte eine Bestellung aus"
        End If
    End Sub

    Private Sub Change02_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        lblBestellungsAnzahl.Text = lbxBestellungen.Items.Count.ToString
    End Sub

End Class

' ************************************************
' $History: Change02.aspx.vb $
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 3.08.10    Time: 16:46
' Updated in $/CKAG2/KBS/Forms
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 12.03.10   Time: 9:49
' Updated in $/CKAG2/KBS/Forms
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 24.02.10   Time: 17:59
' Updated in $/CKAG2/KBS/Forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 18.02.10   Time: 9:18
' Updated in $/CKAG2/KBS/Forms
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 3.06.09    Time: 16:53
' Updated in $/CKAG2/KBS/Forms
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 5.05.09    Time: 10:12
' Updated in $/CKAG2/KBS/Forms
' ITA 2838 kommentare 
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 4.05.09    Time: 17:35
' Updated in $/CKAG2/KBS/Forms
' ITA 2838
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 4.05.09    Time: 11:44
' Updated in $/CKAG2/KBS/Forms
' ITA 2838 unfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 30.04.09   Time: 16:56
' Updated in $/CKAG2/KBS/Forms
' ITA 2838 unfertig
'
' ************************************************