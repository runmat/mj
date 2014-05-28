Option Strict On
Option Explicit On

Imports CKG.Base.Kernel

Public Class WSÜbersicht
    Inherits TranslatedUserControl
    Implements IWizardStep

    Protected ReadOnly Property TransferPage As ITransferPage
        Get
            Return DirectCast(Page, ITransferPage)
        End Get
    End Property

    Protected Overrides Sub OnPreRender(e As EventArgs)
        Dim dal = TransferPage.Dal

        lFzg1.Text = dal.Fzg1.Kennzeichen
        gvFzg1.DataSource = ConvertFahrten(dal.Fzg1)
        gvFzg1.DataBind()

        If Not dal.Fzg2 Is Nothing Then
            trFzg2Header.Visible = True
            trFzg2Data.Visible = True
            lFzg2.Text = dal.Fzg2.Kennzeichen
            gvFzg2.DataSource = ConvertFahrten(dal.Fzg2)
            gvFzg2.DataBind()
        End If

        Dim dr = TransferPage.Transfer.Partner.Select(String.Format("PARVW = 'RG' AND KUNNR = '{0}'", TransferPage.Transfer.RG)).First()
        RechnungszahlerFirma.Text = dr.Field(Of String)("NAME1")
        RechnungszahlerStrasse.Text = String.Format("{0} {1}", dr.Field(Of String)("STREET"), dr.Field(Of String)("HOUSE_NUM1"))
        RechnungszahlerPlz.Text = dr.Field(Of String)("POST_CODE1")
        RechnungszahlerOrt.Text = dr.Field(Of String)("CITY1")

        dr = TransferPage.Transfer.Partner.Select(String.Format("PARVW = 'RE' AND KUNNR = '{0}'", TransferPage.Transfer.RE)).First()
        RechnungsempfängerFirma.Text = dr.Field(Of String)("NAME1")
        RechnungsempfängerStrasse.Text = String.Format("{0} {1}", dr.Field(Of String)("STREET"), dr.Field(Of String)("HOUSE_NUM1"))
        RechnungsempfängerFirmaPlz.Text = dr.Field(Of String)("POST_CODE1")
        RechnungsempfängerFirmaOrt.Text = dr.Field(Of String)("CITY1")

        MyBase.OnPreRender(e)
    End Sub

    Private Function ConvertFahrten(fzg As Fahrzeug) As IEnumerable
        Dim adjust As Integer = If(fzg.Abholfahrt IsNot Nothing, 0, -1)
        Return From f In Enumerable.Repeat(fzg.Abholfahrt, 1).
                                    Concat(fzg.Zusatzfahrten).
                                    Concat(Enumerable.Repeat(fzg.Zielfahrt, 1)).
                                    Where(Function(f) f IsNot Nothing).
                         Select(Function(f, i)
                                    Return New With
                                           {
                                               .Typ = If(i = adjust, "Abholadresse:", _
                                                             If(i = fzg.Zusatzfahrten.Count + 1 + adjust, _
                                                                "Zieladresse:", _
                                                                String.Format("Zusatzfahrt {0} / {1}:", i - adjust, f.Transporttyp))),
                                               .Name = f.Adresse.Name,
                                               .Adresse = f.Adresse.Straße,
                                               .PLZOrt = String.Format("{0} - {1} / {2}", f.Adresse.Land, f.Adresse.Postleitzahl, f.Adresse.Ort),
                                               .Termin = If(f.Datum.HasValue, f.Datum.Value.ToShortDateString(), String.Empty)
                                           }
                                End Function)
    End Function

    Protected Sub OnSubmitClick(sender As Object, e As EventArgs)
        RaiseEvent Completed(Me, EventArgs.Empty)
    End Sub

    Protected Sub OnPreviousClick(sender As Object, e As EventArgs)
        RaiseEvent NavigateBack(Me, EventArgs.Empty)
    End Sub

    Protected Sub BackToStartClick(sender As Object, e As EventArgs)
        Session.Remove("Transfer")
        Session.Remove("TransferDal")
        Response.Redirect("Change02s.aspx?AppID=" & Session("AppID").ToString, True)
    End Sub

    Protected Sub NewOrderSameAdress(ByVal sender As Object, ByVal e As EventArgs)
        Dim dal = TransferPage.Dal

        CleanFzg(dal.Fzg1)
        If Not dal.Fzg2 Is Nothing Then CleanFzg(dal.Fzg2)

        Session("KeepAdress") = "1"
        Response.Redirect("Change02s.aspx?AppID=" & Session("AppID").ToString(), False)
    End Sub

    Private Sub CleanFzg(ByVal fahrzeug As Fahrzeug)
        fahrzeug.Beauftragt = String.Empty
        fahrzeug.Bereifung = String.Empty
        fahrzeug.Fahrgestellnummer = String.Empty
        fahrzeug.Kennzeichen = String.Empty
        fahrzeug.Klasse = String.Empty
        fahrzeug.Protokolle.Clear()
        fahrzeug.Referenznummer = String.Empty
        fahrzeug.Typ = String.Empty
        fahrzeug.Wert = String.Empty
        fahrzeug.Zugelassen = String.Empty
        fahrzeug.Zusatzfahrten.Clear()
    End Sub

    Public Event Completed(sender As Object, e As EventArgs) Implements IWizardStep.Completed

    Public Event NavigateBack(sender As Object, e As EventArgs) Implements IWizardStep.NavigateBack

    Protected Sub CreatePDFClick(sender As Object, e As EventArgs)
        Dim pdfDataSet = New DataSet()
        Dim ct = TransferPage.Transfer

        Dim headTable = New DataTable
        headTable.TableName = "Kopf"
        headTable.Columns.Add("Datum", GetType(String))
        headTable.Columns.Add("User", GetType(String))
        Dim headRow = headTable.NewRow
        headRow("Datum") = Now.ToShortDateString
        headRow("User") = TransferPage.CSKUser.UserName
        headTable.Rows.Add(headRow)

        pdfDataSet.Tables.Add(CreateTableStammdaten())
        pdfDataSet.Tables.Add(CreateTableFahrten())
        pdfDataSet.Tables.Add(CreateTableDienstleistungen())
        pdfDataSet.Tables.Add(CreateTableProtokolle())

        Dim auftragsNrTable = New DataTable
        auftragsNrTable.TableName = "Auftragsnummer"
        auftragsNrTable.Columns.Add("ID", GetType(String))
        auftragsNrTable.Columns.Add("Auftragsnummer", GetType(String))

        Dim ilfdnr As Int32 = 1
        For Each row As DataRow In ct.ReturnTable.Rows
            Dim auftragsNrRow = auftragsNrTable.NewRow
            auftragsNrRow("ID") = ilfdnr
            auftragsNrRow("Auftragsnummer") = row("VBELN")
            ilfdnr += 1
            auftragsNrTable.Rows.Add(auftragsNrRow)
        Next

        pdfDataSet.Tables.Add(auftragsNrTable)
        Dim imageHt = New Hashtable()
        imageHt.Add("Logo", TransferPage.CSKUser.Customer.LogoImage)

        Dim docFactory As New DocumentGeneration.WordDocumentFactory(Nothing, imageHt)
        docFactory.CreateDocumentDataset("UebersichtUeberfuehrung", Page, "Components\Logistik\Dokumente\UebersichtUeberfuehrung.doc", headTable, pdfDataSet)
    End Sub

    Private Function CreateTableStammdaten() As DataTable
        Dim tmptable As New DataTable
        tmptable.TableName = "Stammdaten"
        tmptable.Columns.Add("FIN1", GetType(String))
        tmptable.Columns.Add("Kennz1", GetType(String))
        tmptable.Columns.Add("Typ1", GetType(String))
        tmptable.Columns.Add("RefNr1", GetType(String))
        tmptable.Columns.Add("FzgWert1", GetType(String))
        tmptable.Columns.Add("FzgZulBereit1", GetType(String))
        tmptable.Columns.Add("FzgZulDAD1", GetType(String))
        tmptable.Columns.Add("Reifen1", GetType(String))
        tmptable.Columns.Add("FzgKlasse1", GetType(String))
        tmptable.Columns.Add("FIN2", GetType(String))
        tmptable.Columns.Add("Kennz2", GetType(String))
        tmptable.Columns.Add("Typ2", GetType(String))
        tmptable.Columns.Add("RefNr2", GetType(String))
        tmptable.Columns.Add("FzgWert2", GetType(String))
        tmptable.Columns.Add("FzgZulBereit2", GetType(String))
        tmptable.Columns.Add("FzgZulDAD2", GetType(String))
        tmptable.Columns.Add("Reifen2", GetType(String))
        tmptable.Columns.Add("FzgKlasse2", GetType(String))
        tmptable.Columns.Add("NameRG", GetType(String))
        tmptable.Columns.Add("StrasseRG", GetType(String))
        tmptable.Columns.Add("PLZ_OrtRG", GetType(String))
        tmptable.Columns.Add("APartnerRG", GetType(String))
        tmptable.Columns.Add("TelefonRG", GetType(String))
        tmptable.Columns.Add("NameRE", GetType(String))
        tmptable.Columns.Add("StrasseRE", GetType(String))
        tmptable.Columns.Add("PLZ_OrtRE", GetType(String))
        tmptable.Columns.Add("APartnerRE", GetType(String))
        tmptable.Columns.Add("TelefonRE", GetType(String))

        Dim fzg1 = TransferPage.Dal.Fzg1
        Dim fzg2 = TransferPage.Dal.Fzg2

        Dim newRowStamm = tmptable.NewRow

        newRowStamm("FIN1") = fzg1.Fahrgestellnummer
        newRowStamm("Kennz1") = fzg1.Kennzeichen
        newRowStamm("Typ1") = fzg1.Typ
        newRowStamm("RefNr1") = fzg1.Referenznummer
        newRowStamm("FzgWert1") = fzg1.Wert
        newRowStamm("FzgZulBereit1") = fzg1.Zugelassen
        newRowStamm("FzgZulDAD1") = fzg1.Beauftragt
        newRowStamm("Reifen1") = fzg1.Bereifung
        newRowStamm("FzgKlasse1") = fzg1.Klasse
        If Not fzg2 Is Nothing Then
            newRowStamm("FIN2") = fzg2.Fahrgestellnummer
            newRowStamm("Kennz2") = fzg2.Kennzeichen
            newRowStamm("Typ2") = fzg2.Typ
            newRowStamm("RefNr2") = fzg2.Referenznummer
            newRowStamm("FzgWert2") = fzg2.Wert
            newRowStamm("FzgZulBereit2") = fzg2.Zugelassen
            newRowStamm("FzgZulDAD2") = fzg2.Beauftragt
            newRowStamm("Reifen2") = fzg2.Bereifung
            newRowStamm("FzgKlasse2") = fzg2.Klasse
        Else
            newRowStamm("FIN2") = ""
            newRowStamm("Kennz2") = ""
            newRowStamm("Typ2") = ""
            newRowStamm("RefNr2") = ""
            newRowStamm("FzgWert2") = ""
            newRowStamm("FzgZulBereit2") = ""
            newRowStamm("FzgZulDAD2") = ""
            newRowStamm("Reifen2") = ""
            newRowStamm("FzgKlasse2") = ""
        End If

        Dim dr As DataRow = TransferPage.Transfer.Partner.Select("PARVW = 'RG' AND KUNNR = '" & TransferPage.Transfer.RG & "'").First()
        newRowStamm("NameRG") = dr.Field(Of String)("NAME1")
        newRowStamm("StrasseRG") = dr.Field(Of String)("STREET") & " " & dr.Field(Of String)("HOUSE_NUM1")
        newRowStamm("PLZ_OrtRG") = dr.Field(Of String)("POST_CODE1") & " " & dr.Field(Of String)("CITY1")
        newRowStamm("APartnerRG") = dr.Field(Of String)("NAME2")
        newRowStamm("TelefonRG") = dr.Field(Of String)("TEL_NUMBER")

        dr = TransferPage.Transfer.Partner.Select("PARVW = 'RE' AND KUNNR = '" & TransferPage.Transfer.RE & "'").First()
        newRowStamm("NameRE") = dr.Field(Of String)("NAME1")
        newRowStamm("StrasseRE") = dr.Field(Of String)("STREET") & " " & dr.Field(Of String)("HOUSE_NUM1")
        newRowStamm("PLZ_OrtRE") = dr.Field(Of String)("POST_CODE1") & " " & dr.Field(Of String)("CITY1")
        newRowStamm("APartnerRE") = dr.Field(Of String)("NAME2")
        newRowStamm("TelefonRE") = dr.Field(Of String)("TEL_NUMBER")

        tmptable.Rows.Add(newRowStamm)

        Return tmptable
    End Function

    Private Function CreateTableFahrten() As DataTable

        Dim tmptable As New DataTable
        tmptable.TableName = "Fahrten"
        tmptable.Columns.Add("Adresstyp", GetType(String))
        tmptable.Columns.Add("Adresse", GetType(String))
        tmptable.Columns.Add("APartner", GetType(String))
        tmptable.Columns.Add("Telefon", GetType(String))
        tmptable.Columns.Add("Datum", GetType(String))
        tmptable.Columns.Add("Uhrzeit", GetType(String))
        tmptable.Columns.Add("Fahrzeug", GetType(String))
        tmptable.Columns.Add("KM", GetType(String))

        Dim fzg = TransferPage.Dal.Fzg1
        Dim fahrten = {fzg.Abholfahrt}.Union(fzg.Zusatzfahrten).Union({fzg.Zielfahrt}).Where(Function(f) Not f Is Nothing).ToList

        AddFahrten(fahrten, tmptable, "Fahrzeug 1", True, 0)

        fzg = TransferPage.Dal.Fzg2
        If Not fzg Is Nothing Then
            Dim offset = fahrten.Count
            fahrten = fzg.Zusatzfahrten.Union({fzg.Zielfahrt}).Where(Function(f) Not f Is Nothing).ToList
            AddFahrten(fahrten, tmptable, "Fahrzeug 2", False, offset)
        End If

        tmptable.AcceptChanges()

        Return tmptable
    End Function

    Private Sub AddFahrten(fahrten As List(Of Fahrt), tmptable As DataTable, fahrzeug As String, hasAbholadresse As Boolean, kmOffset As Integer)
        Dim entfernungen = TransferPage.Transfer.Entfernungen

        For Each i In Enumerable.Range(0, fahrten.Count)
            Dim fahrt = fahrten(i)
            Dim row = tmptable.NewRow()
            row("Fahrzeug") = fahrzeug
            row("Adresstyp") = If(hasAbholadresse AndAlso i = 0, "Abholadresse", fahrt.Transporttyp)

            If Not entfernungen Is Nothing Then
                Dim eRow = entfernungen.Select("FAHRT = '" & (i + kmOffset) & "'").FirstOrDefault
                If Not eRow Is Nothing Then row("KM") = eRow("KM")
            End If

            Dim a = fahrt.Adresse
            If Not a Is Nothing Then
                row("Adresse") = String.Join(Environment.NewLine, {a.Name, a.Straße, a.Postleitzahl & " " & a.Ort, a.Land})
                row("APartner") = a.Ansprechpartner
                row("Telefon") = a.Telefon
            End If

            If fahrt.Datum.HasValue Then
                row("Datum") = fahrt.Datum.Value.ToShortDateString
            End If

            If Not String.IsNullOrEmpty(fahrt.ZeitVon) Then
                row("Uhrzeit") = fahrt.ZeitVon.Substring(0, 2) & ":" & fahrt.ZeitVon.Substring(2, 2) & " - " & fahrt.ZeitBis.Substring(0, 2) & ":" & fahrt.ZeitBis.Substring(2, 2)
            End If
            tmptable.Rows.Add(row)
        Next
    End Sub

    Private Function CreateTableDienstleistungen() As DataTable

        Dim tmptable As New DataTable
        tmptable.TableName = "Dienstleistungen"
        tmptable.Columns.Add("Adresstyp", GetType(String))
        tmptable.Columns.Add("Dienstleistungen", GetType(String))
        tmptable.Columns.Add("Bemerkung", GetType(String))

        Dim fzg = TransferPage.Dal.Fzg1
        Dim fahrten = fzg.Zusatzfahrten.Union({fzg.Zielfahrt}).ToList
        fzg = TransferPage.Dal.Fzg2
        If Not fzg Is Nothing Then
            fahrten = fahrten.Union(fzg.Zusatzfahrten).Union({fzg.Zielfahrt}).ToList
        End If
        fahrten = fahrten.Where(Function(f) Not f Is Nothing).ToList

        For Each f In fahrten
            Dim row = tmptable.NewRow()

            row("Adresstyp") = f.Transporttyp
            row("Dienstleistungen") = String.Join(Environment.NewLine, f.Dienstleistungen.Select(Function(d) d.Text).ToArray)
            row("Bemerkung") = f.Bemerkung
            tmptable.Rows.Add(row)
        Next
        tmptable.AcceptChanges()

        Return tmptable
    End Function

    Private Function CreateTableProtokolle() As DataTable

        Dim tmptable As New DataTable
        tmptable.TableName = "Protokolle"
        tmptable.Columns.Add("Adresstyp", GetType(String))
        tmptable.Columns.Add("Protokolle", GetType(String))

        Dim fzg = TransferPage.Dal.Fzg1
        Dim protokolle = fzg.Protokolle.Where(Function(p) Not p Is Nothing AndAlso Not String.IsNullOrEmpty(p.Tempfilename)).ToList

        If protokolle.Count > 0 Then
            Dim r = tmptable.NewRow
            r("Adresstyp") = fzg.Zielfahrt.Transporttyp
            r("Protokolle") = String.Join(Environment.NewLine, protokolle.Select(Function(p) p.Filename).ToArray)
            tmptable.Rows.Add(r)
        End If

        fzg = TransferPage.Dal.Fzg2
        If Not fzg Is Nothing Then
            protokolle = fzg.Protokolle.Where(Function(p) Not p Is Nothing AndAlso Not String.IsNullOrEmpty(p.Tempfilename)).ToList

            If protokolle.Count > 0 Then
                Dim r = tmptable.NewRow
                r("Adresstyp") = fzg.Zielfahrt.Transporttyp
                r("Protokolle") = String.Join(Environment.NewLine, protokolle.Select(Function(p) p.Filename).ToArray)
                tmptable.Rows.Add(r)
            End If
        End If

        tmptable.AcceptChanges()

        Return tmptable
    End Function

    Public Property IsWizardComplete As Boolean
        Get
            Return lbBackToStart.Visible
        End Get
        Set(value As Boolean)
            lbPrevious.Enabled = Not value
            lbSubmit.Enabled = Not value
            lbBackToStart.Visible = value
            lbNewOrderSameAdress.Visible = value
            ibtnCreatePDF.Visible = value
            lblPDFPrint.Visible = value
        End Set
    End Property


End Class