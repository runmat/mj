Imports System
Imports KBS.KBS_BASE
Imports Telerik.Web.UI

Public Class ReportLetztePlatinenbestellungen
    Inherits Page

    Private mObjKasse As Kasse
    Private mObjPlatinen As Platinen

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)
        lblError.Text = ""
        Title = lblHead.Text
        If mObjKasse Is Nothing Then
            If Not Session("mKasse") Is Nothing Then
                mObjKasse = CType(Session("mKasse"), Kasse)
            Else
                Throw New Exception("benötigtes Session Objekt nicht vorhanden")
            End If
        End If

        If mObjPlatinen Is Nothing Then
            If Session("mPlatinen") IsNot Nothing Then
                mObjPlatinen = CType(Session("mPlatinen"), Platinen)
            Else
                mObjPlatinen = mObjKasse.Platinen(Me)
                Session("mPlatinen") = mObjPlatinen
            End If
        End If

        If Not IsPostBack Then
            If mObjKasse.Master Then
                txtKST.Enabled = True
                ddlLieferant.Enabled = False
                lbSearch.Enabled = False
                SetFocus(txtKST)
            Else
                txtKST.Enabled = False
                txtKST.Text = mObjKasse.Lagerort
                ApplyKst()
            End If
        End If
    End Sub

    Protected Sub txtKST_TextChanged(sender As Object, e As EventArgs) Handles txtKST.TextChanged
        If mObjKasse.Master Then
            If Not String.IsNullOrEmpty(txtKST.Text) Then
                ApplyKst(True)
            End If
        Else
            SetFocus(txtKST)
            lblKSTText.Visible = False
            lblKSTText.Text = ""
            ddlLieferant.Enabled = False
            lbSearch.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Dropdown der Lieferanten füllen
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub fillDropdown()
        mObjPlatinen.getLieferantenERP(mObjKasse.Master, mObjKasse.Firma)

        If Not mObjPlatinen.ErrorOccured Then

            ddlLieferant.DataSource = mObjPlatinen.Lieferanten
            ddlLieferant.DataValueField = "LIFNR"
            ddlLieferant.DataTextField = "NAME1"
            ddlLieferant.DataBind()

            Dim hauptLiefRows() As DataRow = mObjPlatinen.Lieferanten.Select("HAUPT = 'X'")
            If hauptLiefRows.Length > 0 Then
                ddlLieferant.SelectedValue = hauptLiefRows(0)("LIFNR")
            End If

            mObjPlatinen.Lieferantennr = ddlLieferant.SelectedValue

        Else
            lblError.Text = mObjPlatinen.ErrorMessage '"Es konnten keine Lieferanten geladen werden!"
        End If

        Session("mPlatinen") = mObjPlatinen
    End Sub

    Protected Sub lbSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbSearch.Click

        mObjPlatinen.Lieferantennr = ddlLieferant.SelectedValue

        If String.IsNullOrEmpty(mObjPlatinen.KostStelle) Then
            lblError.Text = "Bitte geben Sie eine Kostenstelle an!"
            Exit Sub
        End If

        If String.IsNullOrEmpty(mObjPlatinen.Lieferantennr) Then
            lblError.Text = "Bitte wählen Sie einen Lieferanten aus!"
            Exit Sub
        End If

        LadeLetzteBestellungen()
    End Sub

    Private Sub LadeLetzteBestellungen()
        mObjPlatinen.getLetzteBestellungenERP()
        Session("mPlatinen") = mObjPlatinen

        If Not mObjPlatinen.ErrorOccured Then
            FillGrid()
        Else
            lblError.Text = "Fehler beim Laden der letzten Bestellungen: " & mObjPlatinen.ErrorMessage
            rgGrid1.Visible = False
        End If
    End Sub

    Private Sub FillGrid()
        If mObjPlatinen.LetzteBestellungenKopf IsNot Nothing AndAlso mObjPlatinen.LetzteBestellungenKopf.Rows.Count > 0 Then
            rgGrid1.Visible = True
            rgGrid1.Rebind()
            'Setzen der DataSource geschieht durch das NeedDataSource-Event
        Else
            rgGrid1.Visible = False
            lblNoData.Text = "Keine Daten gefunden"
        End If
    End Sub

    Protected Sub rgGrid1_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles rgGrid1.NeedDataSource
        If Not e.IsFromDetailTable Then
            If mObjPlatinen.LetzteBestellungenKopf IsNot Nothing Then
                rgGrid1.DataSource = mObjPlatinen.LetzteBestellungenKopf.DefaultView
            Else
                rgGrid1.DataSource = Nothing
            End If
        End If
    End Sub

    Protected Sub rgGrid1_DetailTableDataBind(ByVal sender As Object, ByVal e As GridDetailTableDataBindEventArgs) Handles rgGrid1.DetailTableDataBind
        Dim bestellNr As String = e.DetailTableView.ParentItem.GetDataKeyValue("BSTNR").ToString()

        If Not String.IsNullOrEmpty(bestellNr) AndAlso mObjPlatinen.LetzteBestellungenPos IsNot Nothing Then
            Dim dv As New DataView(mObjPlatinen.LetzteBestellungenPos)
            dv.RowFilter = "BSTNR = '" & bestellNr & "'"
            e.DetailTableView.DataSource = dv
        End If
    End Sub

    Protected Sub lb_zurueck_Click(sender As Object, e As EventArgs) Handles lb_zurueck.Click
        Response.Redirect("../Selection.aspx")
    End Sub

    Private Function ApplyKst(Optional ByVal skipIfNotChanged As Boolean = False) As Boolean
        With mObjPlatinen

            If skipIfNotChanged AndAlso .KostStelle = txtKST.Text.Trim Then
                'Kst nicht geändert
                Return True
            End If

            .CheckKostStelleERP(txtKST.Text.Trim)

            If .ErrorOccured Then
                lblError.Text = .ErrorMessage
                lblKSTText.Visible = False
                lblKSTText.Text = ""
                ddlLieferant.Enabled = False
                lbSearch.Enabled = False
                SetFocus(txtKST)
                Return False
            End If

            .KostStelle = txtKST.Text.Trim()
            lblKSTText.Visible = True
            lblKSTText.Text = .KostText
            fillDropdown()
            ddlLieferant.Enabled = True
            lbSearch.Enabled = True
            SetFocus(ddlLieferant)

        End With

        Session("mPlatinen") = mObjPlatinen

        Return True
    End Function

End Class