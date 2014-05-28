Option Explicit On
Option Strict On

Public Class CWSDLZieladresseKfz1
    Inherits TranslatedUserControl
    Implements ICollapsibleWizardStep

    Private _fzg As Fahrzeug

    Protected ReadOnly Property TransferPage As ITransferPage
        Get
            Return DirectCast(Page, ITransferPage)
        End Get
    End Property

    Protected Overrides Sub OnInit(e As EventArgs)
        MyBase.OnInit(e)

        AddHandler TransferPage.Dal.PropertyChanged, AddressOf OnDalPropertyChanged
        AddFzgPropertyChanged(TransferPage.Dal.Fzg1)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        Dim dal = TransferPage.Dal
        If Services.MustInitSelectbox AndAlso Not dal.Fzg1 Is Nothing Then
            Dim fahrt As Fahrt = dal.Fzg1.Zielfahrt

            If Not fahrt Is Nothing Then
                Services.InitSelectbox(dal.GetDienstleistungen(fahrt.TransporttypCode))
            End If
        End If

        Dim protokollArten = dal.ProtokollArten
        If ProtocolUpload.NeedsInitialization Then
            If protokollArten IsNot Nothing Then
                Dim initialProtokolle = protokollArten.Select("Fahrt='1' OR Fahrt=''").Select(Function(r) Protokoll.FromRow(r, "1")).ToList
                ProtocolUpload.Protokolle = initialProtokolle
            Else
                ProtocolUpload.Protokolle = New List(Of Protokoll)
            End If    
            ProtocolUpload.FillGrid()
        End If
    End Sub

    Protected Overrides Sub OnUnload(e As EventArgs)
        RemoveFzgPropertyChanged()
        RemoveHandler TransferPage.Dal.PropertyChanged, AddressOf OnDalPropertyChanged

        MyBase.OnUnload(e)
    End Sub

    Private Sub OnDalPropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        If e.PropertyName.Equals("Fzg1", StringComparison.Ordinal) Then
            RemoveFzgPropertyChanged()
            AddFzgPropertyChanged(TransferPage.Dal.Fzg1)
        End If
    End Sub

    Private Sub AddFzgPropertyChanged(newFzg As Fahrzeug)
        _fzg = newFzg

        If _fzg IsNot Nothing Then
            AddHandler _fzg.PropertyChanged, AddressOf OnFzgPropertyChanged
        End If
    End Sub

    Private Sub RemoveFzgPropertyChanged()
        If _fzg IsNot Nothing Then
            RemoveHandler _fzg.PropertyChanged, AddressOf OnFzgPropertyChanged
            _fzg = Nothing
        End If
    End Sub

    Private Sub OnFzgPropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        If e.PropertyName.Equals("Zielfahrt", StringComparison.Ordinal) Then
            Dim fahrt As Fahrt = TransferPage.Dal.Fzg1.Zielfahrt
            Services.Dienstleistungen = From dienstleistung As Dienstleistung In fahrt.Dienstleistungen _
                                                Select dienstleistung.Nummer
            Services.InitSelectbox(TransferPage.Dal.GetDienstleistungen(fahrt.TransporttypCode))
        End If
    End Sub

    ' Express ~ Fahrt innerhalb von weniger als 'ExpressDays' 
    Protected Function IsExpressDelivery(f As Fahrt) As Boolean
        Dim dal = TransferPage.Dal

        If Not (dal.IsExpress AndAlso f.Datum.HasValue) Then Return False

        Dim skipWeekend = Function(d1 As Date) As Date
                              If d1.DayOfWeek = DayOfWeek.Saturday Then Return d1.AddDays(2)
                              If d1.DayOfWeek = DayOfWeek.Sunday Then Return d1.AddDays(1)
                              Return d1
                          End Function

        Dim tmpDate = skipWeekend(DateTime.Today)
        For i = 0 To dal.ExpressDays - 1
            tmpDate = skipWeekend(tmpDate.AddDays(1))
        Next

        Return tmpDate >= f.Datum.Value
    End Function

    ' Vorholung ~ Es finden Fahrten an unterschiedlichen Tagen statt
    Protected Function IsVorholung(ByVal ParamArray fahrten As Fahrt()) As Boolean
        Dim dal = TransferPage.Dal
        Dim dates = fahrten.Where(Function(f) f.Datum.HasValue).Select(Function(f) f.Datum.Value).ToList

        If Not (dal.IsVorholung AndAlso dates.Count > 1) Then Return False

        Return dates.Distinct().Count() > 1
    End Function

    ' Samstagsfahrt
    Protected Function IsSamstag(ByVal ParamArray fahrten As Fahrt()) As Boolean
        Dim dal = TransferPage.Dal
        Dim dates = fahrten.Where(Function(f) f.Datum.HasValue).Select(Function(f) f.Datum.Value).ToList

        Return dal.IsSamstag AndAlso dates.Any(Function(d) d.DayOfWeek = DayOfWeek.Saturday)
    End Function

    ' Sonstiges
    Protected Function IsSonstiges(ByVal ParamArray fahrten As Fahrt()) As Boolean
        Dim dal = TransferPage.Dal

        Return dal.IsSonstiges AndAlso fahrten.Any(Function(f) Not String.IsNullOrEmpty(f.Bemerkung))
    End Function

    Protected Sub OnValidateExpress(sender As Object, e As ServerValidateEventArgs)
        Dim dal = TransferPage.Dal
        Dim dienstleistungen = Services.Dienstleistungen
        Dim dienstleistungenArr = If(TryCast(dienstleistungen, String()), dienstleistungen.ToArray())
        Dim messages = New List(Of String)

        Services.ResetPreselection()
        If Not ProtocolUpload.ValidateUploads() Then
            e.IsValid = False
            messages.Add("Der Upload einer oder mehrerer Dateien war nicht erfolgreich. Bitte laden Sie diese ggf. erneut hoch.")
        ElseIf IsExpressDelivery(dal.Fzg1.Abholfahrt) AndAlso Not dienstleistungenArr.Contains(dal.ExpressDienstleistung.Nummer) Then
            e.IsValid = False
            messages.Add("Das Wunschlieferdatum liegt weniger als " & dal.ExpressDays & " Werktag(e) in der Zukunft. Bitte wählen Sie aus den verfügbaren Dienstleistungen „Expressauslieferung"".")
            Services.Preselect(dal.ExpressDienstleistung.Nummer)
        ElseIf IsVorholung({dal.Fzg1.Abholfahrt, dal.Fzg1.Zielfahrt}.Union(dal.Fzg1.Zusatzfahrten).ToArray) AndAlso _
            Not dienstleistungenArr.Contains(dal.VorholungDienstleistung.Nummer) Then
            e.IsValid = False
            messages.Add("Die Fahrten liegen an unterschiedlichen Tagen. Sie beauftragen hiermit eine Vorholung am " & dal.Fzg1.Abholfahrt.Datum.Value.ToShortDateString & ". Bitte wählen Sie aus den verfügbaren Dienstleistungen „Vorholung Fahrzeug"".")
            Services.Preselect(dal.VorholungDienstleistung.Nummer)
        ElseIf IsSamstag({dal.Fzg1.Abholfahrt, dal.Fzg1.Zielfahrt}.Union(dal.Fzg1.Zusatzfahrten).ToArray) AndAlso _
            Not dienstleistungenArr.Contains(dal.SamstagDienstleistung.Nummer) Then
            e.IsValid = False
            messages.Add("Sie haben einen Samstag als Wunschtermin eingegeben. Bitte wählen Sie aus den verfügbaren Dienstleistungen „Samstagsauslieferung oder Abholung"".")
            Services.Preselect(dal.SamstagDienstleistung.Nummer)
        ElseIf IsSonstiges({dal.Fzg1.Abholfahrt, dal.Fzg1.Zielfahrt}.Union(dal.Fzg1.Zusatzfahrten).ToArray) AndAlso _
            Not dienstleistungenArr.Contains(dal.SonstigesDienstleistung.Nummer) Then
            Services.Dienstleistungen = Services.Dienstleistungen.Union({dal.SonstigesDienstleistung.Nummer}).ToList
        End If

        cvExpress.ErrorMessage = String.Join("<br />", messages.ToArray)
    End Sub

    Public Sub Save() Implements ICollapsibleWizardStep.Save
        Dim fzg = Me.TransferPage.Dal.Fzg1
        Dim fahrt As Fahrt = fzg.Zielfahrt
        fahrt.Dienstleistungen.Clear()

        Dim services As New HashSet(Of String)(Me.Services.Dienstleistungen)
        Dim dienstleistungen As IEnumerable(Of Dienstleistung) = From d In Me.TransferPage.Dal.GetDienstleistungen(fahrt.TransporttypCode)
                                                                 Where services.Contains(d.Nummer)
                                                                 Select d

        For Each dienstleistung As Dienstleistung In dienstleistungen
            fahrt.Dienstleistungen.Add(dienstleistung)
        Next

        fzg.Protokolle.Clear()
        For Each p As Protokoll In ProtocolUpload.Protokolle
            fzg.Protokolle.Add(p)
        Next

        fahrt.Bemerkung = Me.Services.Bemerkung
    End Sub

    Public Sub Validate() Implements ICollapsibleWizardStep.Validate
        Me.Page.Validate("CWSDLZieladresseKfz1")
    End Sub

    Public Event WizardStepError(sender As Object, e As ErrorEventArgs) Implements ICollapsibleWizardStep.WizardStepError
End Class