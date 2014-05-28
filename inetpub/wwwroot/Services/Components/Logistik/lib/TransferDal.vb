Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.IO

Public Class TransferDal
    Implements INotifyPropertyChanged

    Private ReadOnly _transfer As Transfer
    Private _fzg1 As Fahrzeug
    Private _fzg2 As Fahrzeug
    Private _halterAdresse As Adresse

    Public Property HalterAdresse As Adresse
        Get
            Return _halterAdresse
        End Get
        Set(value As Adresse)
            _halterAdresse = value
        End Set
    End Property

    Public Property Fzg1 As Fahrzeug
        Get
            Return _fzg1
        End Get
        Set(value As Fahrzeug)
            If Not EqualityComparer(Of Fahrzeug).Default.Equals(value, _fzg1) Then
                _fzg1 = value

                Dim e As New PropertyChangedEventArgs("Fzg1")
                RaiseEvent PropertyChanged(Me, e)
            End If
        End Set
    End Property

    Public Property Fzg2 As Fahrzeug
        Get
            Return _fzg2
        End Get
        Set(value As Fahrzeug)
            If Not EqualityComparer(Of Fahrzeug).Default.Equals(value, _fzg2) Then
                _fzg2 = value

                Dim e As New PropertyChangedEventArgs("Fzg2")
                RaiseEvent PropertyChanged(Me, e)
            End If
        End Set
    End Property

    Public ReadOnly Property IsExpress As Boolean
        Get
            Return _transfer.Express
        End Get
    End Property

    Public ReadOnly Property ExpressDays As Integer
        Get
            Return _transfer.ExpressDays
        End Get
    End Property

    Public ReadOnly Property ExpressDienstleistung As Dienstleistung
        Get
            Return New Dienstleistung() With {.Nummer = _transfer.ExpressDlNummer}
        End Get
    End Property

    Public ReadOnly Property IsVorholung As Boolean
        Get
            Return _transfer.Vorholung
        End Get
    End Property

    Public ReadOnly Property VorholungDienstleistung As Dienstleistung
        Get
            Return New Dienstleistung() With {.Nummer = _transfer.VorholDlNummer}
        End Get
    End Property

    Public ReadOnly Property IsSamstag As Boolean
        Get
            Return _transfer.Samstag
        End Get
    End Property

    Public ReadOnly Property SamstagDienstleistung As Dienstleistung
        Get
            Return New Dienstleistung() With {.Nummer = _transfer.SamstagDlNummer}
        End Get
    End Property

    Public ReadOnly Property IsSonstiges As Boolean
        Get
            Return _transfer.Sonstiges
        End Get
    End Property

    Public ReadOnly Property SonstigesDienstleistung As Dienstleistung
        Get
            Return New Dienstleistung() With {.Nummer = _transfer.SonstigesDlNummer}
        End Get
    End Property

    Public Sub New(transfer As Transfer)
        _transfer = transfer
    End Sub

    Public Property ProtokollArten As DataTable
        Get
            Return _transfer.ProtokollArten
        End Get
        Set(value As DataTable)
            _transfer.ProtokollArten = value
        End Set
    End Property

    Public Function GetStandarddienstleistungen(transporttyp As String) As IEnumerable(Of Dienstleistung)
        Return GetDienstleistungen(transporttyp, True)
    End Function

    Public Function GetDienstleistungen(transporttyp As String) As IEnumerable(Of Dienstleistung)
        Return GetDienstleistungen(transporttyp, False)
    End Function

    Private Function GetDienstleistungen(transporttyp As String, standard As Boolean) As IEnumerable(Of Dienstleistung)
        _transfer.DienstAuswahl.DefaultView.RowFilter = "EXTGROUP='" & transporttyp & "' AND ASNUM <> '' AND KTEXT1_H2 = ''" & If(standard, " AND VW_AG = 'X'", "")

        Return From r As DataRowView In _transfer.DienstAuswahl.DefaultView.Cast(Of DataRowView)() _
               Select New Dienstleistung() With {.Nummer = DirectCast(r("ASNUM"), String), .Text = DirectCast(r("ASKTX"), String), .MatNummer = DirectCast(r("EAN11"), String)}
    End Function

    Public Sub RefillSubmitData()
        _transfer.Fahrzeuge.Rows.Clear()
        _transfer.Fahrten.Rows.Clear()
        _transfer.Adressen.Rows.Clear()
        _transfer.Bemerkungen.Rows.Clear()
        _transfer.Dienstleistungen.Rows.Clear()
        If _transfer.ProtokollArten IsNot Nothing Then _transfer.ProtokollArten.Rows.Clear()

        AddFahrzeugrow("1", Fzg1)
        If Fzg2 IsNot Nothing Then
            AddFahrzeugrow("2", Fzg2)
        End If
        AddHalterAdresse()

        _transfer.Fahrzeuge.AcceptChanges()
        _transfer.Fahrten.AcceptChanges()
        _transfer.Adressen.AcceptChanges()
        _transfer.Bemerkungen.AcceptChanges()
        _transfer.Dienstleistungen.AcceptChanges()
        If _transfer.ProtokollArten IsNot Nothing Then _transfer.ProtokollArten.AcceptChanges()
    End Sub

    Public Function Submit(user As Base.Kernel.Security.User, page As Page, kunnr As String) As List(Of String)
        RefillSubmitData()

        Dim result = New List(Of String)
        Dim vorgangsnummer = _transfer.GetVorgangsnummer(user, page)
        If _transfer.Status <> 0 Then
            result.Add(_transfer.Message)
        Else
            _transfer.Save(user, page, kunnr, vorgangsnummer)

            If _transfer.Status <> 0 Then result.Add(_transfer.Message)

            Dim sapMessages = _transfer.ReturnTable.Rows.Cast(Of DataRow).Where(Function(r) String.IsNullOrEmpty(CStr(r("VBELN")))).Select(Function(r) CStr(r("BEMERKUNG"))).ToList()
            result.AddRange(sapMessages)
        End If

        Return result

        '' Success if no empty VBELN returned
        'Return Me._transfer.Status = 0 AndAlso Not (From r In Me._transfer.ReturnTable.AsEnumerable() _
        '                           Where String.IsNullOrEmpty(r.Field(Of String)("VBELN"))
        '                           Select r).Any()
    End Function

    Private Function GetFahrtindex() As Integer
        Return If(_transfer.Fahrten.AsEnumerable().Any(), _transfer.Fahrten.AsEnumerable().Max(Function(r As DataRow) CInt(r.Field(Of String)("FAHRT"))) + 1, 0)
    End Function

    Private Sub AddFahrzeugrow(index As String, fzg As Fahrzeug)
        Dim row = _transfer.Fahrzeuge.NewRow()

        row.SetField("FAHRZEUG", index)
        row.SetField("ZZFAHRZGTYP", fzg.Typ)
        row.SetField("ZZKENN", fzg.Kennzeichen)
        row.SetField("FZGART", fzg.Klasse)
        row.SetField("ZULGE", fzg.Zugelassen)
        row.SetField("ZUL_BEI_CK_DAD", fzg.Beauftragt)
        row.SetField("SOWI", fzg.Bereifung)
        row.SetField("AUGRU", fzg.Wert)
        row.SetField("ZZREFNR", fzg.Referenznummer)
        row.SetField("ZZFAHRG", fzg.Fahrgestellnummer)

        _transfer.Fahrzeuge.Rows.Add(row)

        Dim fahrtindex As Integer = GetFahrtindex()

        If fzg.Abholfahrt IsNot Nothing Then
            AddFahrtrow(fahrtindex, index, fzg.Abholfahrt)
            fahrtindex = fahrtindex + 1
        End If

        For Each fahrt As Fahrt In fzg.Zusatzfahrten
            AddFahrtrow(fahrtindex, index, fahrt)
            fahrtindex = fahrtindex + 1
        Next

        AddFahrtrow(fahrtindex, index, fzg.Zielfahrt)

        For Each protokoll As Protokoll In fzg.Protokolle
            AddProtokollrow(index, protokoll)
        Next
    End Sub

    Private Sub AddFahrtrow(index As Integer, fzg As String, fahrt As Fahrt)
        Dim row = _transfer.Fahrten.NewRow()
        Dim strIndex = CStr(index)

        row.SetField("FAHRT", strIndex)
        row.SetField("FAHRZEUG", fzg)
        row.SetField("REIHENFOLGE", index)
        row.SetField("TRANSPORTTYP", fahrt.Transporttyp)
        row.SetField("TRANSPORTTYPNR", fahrt.TransporttypCode)
        row.SetField("VDATU", fahrt.Datum)
        row.SetField("AT_TIM_VON", fahrt.ZeitVon)
        row.SetField("AT_TIM_BIS", fahrt.ZeitBis)
        row.SetField("KENNZ_ZUS_FAHT", "")

        _transfer.Fahrten.Rows.Add(row)

        AddAdresserow(strIndex, fahrt.Adresse)
        AddBemerkungrows(strIndex, If(fahrt.Bemerkung, String.Empty))

        For Each dienstleistung As Dienstleistung In fahrt.Dienstleistungen
            AddDienstleistungrow(strIndex, dienstleistung)
        Next
    End Sub

    Private Sub AddHalterAdresse()
        If Not HalterAdresse Is Nothing Then
            AddAdresserow(New String("9"c, 10), HalterAdresse)
        End If
    End Sub

    Private Sub AddAdresserow(fahrt As String, adresse As Adresse)
        Dim row = _transfer.Adressen.NewRow()

        row.SetField("FAHRT", fahrt)
        row.SetField("PARTN_NUMB", adresse.DebitorNr)
        row.SetField("NAME", adresse.Name)
        row.SetField("NAME_2", adresse.Ansprechpartner)
        row.SetField("STREET", adresse.Straße)
        row.SetField("POSTL_CODE", adresse.Postleitzahl)
        row.SetField("CITY", adresse.Ort)
        row.SetField("TELEPHONE", adresse.Telefon)
        row.SetField("COUNTRY", adresse.Land)
        row.SetField("SMTP_ADDR", adresse.EMail)

        _transfer.Adressen.Rows.Add(row)
    End Sub

    Private Sub AddBemerkungrows(fahrt As String, bemerkung As String)
        For i As Integer = 0 To bemerkung.Length - 1 Step 40
            Dim row = _transfer.Bemerkungen.NewRow()

            row.SetField("FAHRT", fahrt)
            row.SetField("TEXT_ID", "0018")
            row.SetField("BEMERKUNG", bemerkung.Substring(i, Math.Min(40, bemerkung.Length - i)))

            _transfer.Bemerkungen.Rows.Add(row)
        Next
    End Sub

    Private Sub AddDienstleistungrow(fahrt As String, dienstleistung As Dienstleistung)
        Dim row = _transfer.Dienstleistungen.NewRow()

        row.SetField("FAHRT", fahrt)
        row.SetField("DIENSTL_NR", dienstleistung.Nummer)
        row.SetField("DIENSTL_TEXT", dienstleistung.Text)
        row.SetField("MATNR", dienstleistung.MatNummer)

        _transfer.Dienstleistungen.Rows.Add(row)
    End Sub

    Private Sub AddProtokollrow(fahrt As String, protokoll As Protokoll)
        If String.IsNullOrEmpty(protokoll.Tempfilename) Then Return

        Dim row = _transfer.ProtokollArten.NewRow()

        row("FAHRT") = fahrt
        row("ZZKATEGORIE") = protokoll.Kategorie
        row("ZZPROTOKOLLART") = protokoll.Protokollart
        row("Tempfilename") = protokoll.Tempfilename
        row("Filename") = protokoll.Filename

        _transfer.ProtokollArten.Rows.Add(row)
    End Sub

    Public Function ValidateUploads() As Boolean
        Dim blnFileNotFound As Boolean = False

        Try
            'prüfen, ob hochgeladene Dateien auch wirklich auf dem Server vorhanden sind
            Dim alleProtokolle As New List(Of Protokoll)

            If Me.Fzg1.Protokolle IsNot Nothing Then
                alleProtokolle.AddRange(Me.Fzg1.Protokolle)
            End If
            If Me.Fzg2 IsNot Nothing AndAlso Me.Fzg2.Protokolle IsNot Nothing Then
                alleProtokolle.AddRange(Me.Fzg2.Protokolle)
            End If

            For Each p As Protokoll In alleProtokolle
                If p IsNot Nothing AndAlso Not String.IsNullOrEmpty(p.Tempfilename) AndAlso Not File.Exists(p.Tempfilename) Then
                    blnFileNotFound = True
                    p.Tempfilename = ""
                End If
            Next

            If blnFileNotFound Then
                Return False
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class