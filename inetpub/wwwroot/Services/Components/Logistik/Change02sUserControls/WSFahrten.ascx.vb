Option Strict On
Option Explicit On

Public Class WSFahrten
    Inherits WSBase

    Protected ReadOnly Property TransferPage As ITransferPage
        Get
            Return DirectCast(Me.Page, ITransferPage)
        End Get
    End Property

    Protected Overrides Sub OnInit(e As System.EventArgs)
        MyBase.OnInit(e)
        ' Greife auf cwSteps.Controls zu um das Erzeugen der einzelnen Schritten
        ' zu erzwingen. Sie können nun selber auf PropertChanged reagieren
        Dim i As Integer = Me.cwcSteps.Controls.Count
        AddHandler Me.TransferPage.Dal.PropertyChanged, AddressOf Me.OnDalPropertyChanged
    End Sub

    Private Sub OnDalPropertyChanged(sender As Object, e As ComponentModel.PropertyChangedEventArgs)
        If e.PropertyName.Equals("Fzg2", StringComparison.Ordinal) Then
            Me.cwcSteps.Steps(3).Enabled = Me.TransferPage.Dal.Fzg2 IsNot Nothing
            Me.cwcSteps.Steps(4).Enabled = Me.TransferPage.Dal.Fzg2 IsNot Nothing
        End If
    End Sub

    Protected Overrides Sub OnUnload(e As System.EventArgs)
        RemoveHandler Me.TransferPage.Dal.PropertyChanged, AddressOf Me.OnDalPropertyChanged

        MyBase.OnUnload(e)
    End Sub

    Protected Overrides Sub OnCompleted(e As System.EventArgs)
        If Me.CheckDates() Then
            MyBase.OnCompleted(e)
        End If
    End Sub

    Private Function AreAscendingOrEmptyDates(values As IList(Of Date?)) As Boolean
        Dim filled = values.Where(Function(d) d.HasValue).Select(Function(d) d.Value).ToList
        If filled.Count = 0 Then Return True

        Dim current = filled.First
        For Each d In filled.Skip(1)
            If (current > d) Then Return False
            current = d
        Next

        Return True
    End Function

    Private Function AreDatesInFuture(values As IList(Of Date?)) As Boolean
        Dim filled = values.Where(Function(d) d.HasValue).Select(Function(d) d.Value).ToList
        If filled.Count = 0 Then Return True

        For Each d In filled
            If (d.Date < Now.Date) Then Return False
        Next

        Return True
    End Function

    Private Function CheckDates() As Boolean
        Dim ct As TransferDal = Me.TransferPage.Dal
        
        Dim fzg = ct.Fzg1
        Dim dates = {fzg.Abholfahrt.Datum}.Union(fzg.Zusatzfahrten.Select(Function(f) f.Datum)).Union({fzg.Zielfahrt.Datum}).ToList
        Dim hasDate = dates.Any(Function(d) d.HasValue)
        'Dim allDates = dates.All(Function(d) d.HasValue)
        Dim hasSunday = dates.Any(Function(d) d.HasValue AndAlso d.Value.DayOfWeek = DayOfWeek.Sunday)
        Dim datesSorted = AreAscendingOrEmptyDates(dates)
        Dim datesInFuture = AreDatesInFuture(dates)

        If ct.Fzg2 IsNot Nothing Then
            fzg = ct.Fzg2
            dates = {ct.Fzg1.Abholfahrt.Datum}.Union(fzg.Zusatzfahrten.Select(Function(f) f.Datum)).Union({fzg.Zielfahrt.Datum}).ToList
            hasDate = hasDate OrElse dates.Any(Function(d) d.HasValue)
            'allDates = allDates OrElse dates.All(Function(d) d.HasValue)
            hasSunday = hasSunday OrElse dates.Any(Function(d) d.HasValue AndAlso d.Value.DayOfWeek = DayOfWeek.Sunday)
            datesSorted = datesSorted AndAlso AreAscendingOrEmptyDates(dates)
            datesInFuture = datesInFuture AndAlso AreDatesInFuture(dates)
        End If

        If hasDate Then
            If hasSunday Then
                lblErrorStamm.Text = "Sie haben einen Sonntag als Wunschtermin eingegeben.<br />Bitte prüfen Sie die Daten."
                lblErrorStamm.Visible = True

                Return False
            End If

            'If Not allDates Then
            '    lblErrorStamm.Text = "Bitte geben Sie für jede Fahrt ein Datum an, oder lassen Sie alle frei."
            '    lblErrorStamm.Visible = True

            '    Return False
            'End If

            'Dim abholfahrtInvalid = Function(abholfahrt As Fahrt, f As Fahrzeug) As Boolean
            '                            Dim fahrten = f.Zusatzfahrten.Select(Function(fahrt) fahrt.Datum.Value).Union({f.Zielfahrt.Datum.Value})
            '                            Return fahrten.Any(Function(datum) datum < abholfahrt.Datum.Value)
            '                        End Function
            'If abholfahrtInvalid(ct.Fzg1.Abholfahrt, ct.Fzg1) OrElse (Not ct.Fzg2 Is Nothing AndAlso abholfahrtInvalid(ct.Fzg1.Abholfahrt, ct.Fzg2)) Then
            'lblErrorStamm.Text = "Bitte beachten Sie! Der Termin der ersten Abholung darf nicht größer als die folgenden Termine sein."

            If Not datesSorted Then
                lblErrorStamm.Text = "Bitte beachten Sie! Termine für Folgefahrten müssen hinter vorausgehenden liegen."
                lblErrorStamm.Visible = True

                Return False
            End If

            If Not datesInFuture Then
                lblErrorStamm.Text = "Bitte beachten Sie! Wunschtermine dürfen nicht in der Vergangenheit liegen."
                lblErrorStamm.Visible = True

                Return False
            End If
        End If

        Return True
    End Function
End Class