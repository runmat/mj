Imports System.Globalization
Imports KBSBase
Imports GeneralTools.Models

''' <summary>
''' Arbeitsklasse, die Daten aus SAP holt und aufbereitet für den Report Filialziele
''' </summary>
''' <remarks></remarks>
Public Class Filialvergleich
    Inherits ErrorHandlingClass

    Private dtFilialauswertung As DataTable
    Private dtFilialvergleich As DataTable
    Private dblRahmenquote As Double
    Private strLFB As String = ""
    Private strDatumFilialVergleich As String = ""
    Private strDatumFilialAuswertung As String = ""

#Region "Propertys"

    ''' <summary>
    ''' Tabelle mit Filialauswertungsdaten
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property FilialauswertungTabelle As DataTable
        Get
            Return dtFilialauswertung
        End Get
    End Property

    ''' <summary>
    ''' Tabelle mit Filialvergleichsdaten
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property FilialvergleichTabelle As DataTable
        Get
            Return dtFilialvergleich
        End Get
    End Property

    ''' <summary>
    ''' Monatl. Rahmenquote
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property Rahmenquote As Double
        Get
            Return dblRahmenquote
        End Get
    End Property

    ''' <summary>
    ''' Zuständiger Leiterfilialbetrieb
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property LFB As String
        Get
            Return strLFB
        End Get
    End Property

    ''' <summary>
    ''' Datum der Erzeugung der Filialvergleichsdaten
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property DatumFilialVergleich As String
        Get
            Return strDatumFilialVergleich
        End Get
    End Property

    ''' <summary>
    ''' Datum der Erzeugung der Filialauswertungsdaten
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public ReadOnly Property DatumFilialAuswertung As String
        Get
            Return strDatumFilialAuswertung
        End Get
    End Property

#End Region

    ''' <summary>
    ''' Holt Daten zur Anzeige aus SAP und füllt diese aufbereitet in die Eigenschaftensfelder
    ''' </summary>
    ''' <param name="VkBur">Kostenstelle für die die Daten abgeglichen werdne sollen</param>
    ''' <param name="Kundengruppe">PK = Privatkunde, GK = Geschäftskunde</param>
    Public Sub getDataFromSAP(ByVal VkBur As String, ByVal Kundengruppe As String, ByVal jahrWoche As String)
        ClearErrorState()

        Try
            ' Filialauswertung Daten holen
            S.AP.Init("Z_ADS_FIL_AUSW_001")

            S.AP.SetImportParameter("I_VKBUR", VkBur)
            S.AP.SetImportParameter("I_WOCHE", jahrWoche)
            S.AP.SetImportParameter("I_KNDGRP", Kundengruppe)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                dblRahmenquote = S.AP.GetExportParameter("E_RAHMENQUOTE").ToDouble(0)
                strDatumFilialAuswertung = S.AP.GetExportParameter("E_DATE")
                If strDatumFilialAuswertung = "00000000" Then
                    strDatumFilialAuswertung = ""
                Else
                    Dim datAuswertung As Date = DateTime.ParseExact(strDatumFilialAuswertung, "yyyyMMdd", CultureInfo.CurrentCulture)
                    strDatumFilialAuswertung = datAuswertung.ToShortDateString()
                End If

                dtFilialauswertung = S.AP.GetExportTable("GT_WEB")
                dtFilialauswertung.Columns.Add("WocheDiffColor")
                dtFilialauswertung.Columns.Add("MonatDiffColor")
                dtFilialauswertung.Columns.Add("AmpelUrl")
                dtFilialauswertung.Columns.Add("Rahmenquote")

                For Each row As DataRow In dtFilialauswertung.Rows
                    row("WocheDiffColor") = getColor(row("WOCHE_ABW").ToString())
                    row("MonatDiffColor") = getColor(row("MONAT_ABW").ToString())
                    row("AmpelUrl") = getAmpel(row("Ampel").ToString())
                    row("Rahmenquote") = Rahmenquote.ToString("F2")
                Next

                dtFilialauswertung.AcceptChanges()

                ' Filialvergleich Daten holen
                S.AP.Init("Z_ADS_FIL_VGL_001")

                S.AP.SetImportParameter("I_VKBUR", VkBur)
                S.AP.SetImportParameter("I_WOCHE", jahrWoche)
                S.AP.SetImportParameter("I_KNDGRP", Kundengruppe)

                S.AP.Execute()

                If S.AP.ResultCode = 0 Then
                    dblRahmenquote = S.AP.GetExportParameter("E_RAHMENQUOTE").ToDouble(0)

                    strDatumFilialVergleich = S.AP.GetExportParameter("E_DATE")
                    If strDatumFilialVergleich = "00000000" Then
                        strDatumFilialVergleich = ""
                    Else
                        Dim datVergleich As Date = DateTime.ParseExact(strDatumFilialVergleich, "yyyyMMdd", CultureInfo.CurrentCulture)
                        strDatumFilialVergleich = datVergleich.ToShortDateString()
                    End If

                    Dim dtRahmenquote As DataTable = S.AP.GetExportTable("GT_RAHMENQUOTE")

                    dtFilialvergleich = S.AP.GetExportTable("GT_WEB")
                    dtFilialvergleich.Columns.Add("Ort")
                    dtFilialvergleich.Columns.Add("WocheDiffColor")
                    dtFilialvergleich.Columns.Add("MonatDiffColor")
                    dtFilialvergleich.Columns.Add("Rahmenquote")

                    For Each row As DataRow In dtFilialvergleich.Rows
                        row("WocheDiffColor") = getColor(row("WOCHE_ABW").ToString())
                        row("MonatDiffColor") = getColor(row("MONAT_ABW").ToString())
                        If row("MATNR_PRODH") = "0001300001" Then
                            row("Rahmenquote") = dtRahmenquote.Select("VKBUR = '" & row("VKBUR").ToString() & "'")(0)("RAHMENQUOTE").ToString() & " %"
                        Else
                            row("Rahmenquote") = ""
                        End If
                    Next

                    dtFilialvergleich.AcceptChanges()
                Else
                    RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
                End If
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Liefert die zu verwendende CSS-Klasse eines String-Wertes
    ''' Value enthält + == Grüne Farbklasse 
    ''' Value enthält - == Rote Farbklasse
    ''' sonst == inherit 
    ''' </summary>
    ''' <param name="Value">zu prüfender String</param>
    ''' <returns>CSS-Klassenname</returns>
    Private Function getColor(ByVal value As String) As String
        If value.Contains("+") Then
            Return "Green"
        ElseIf value.Contains("-") Then
            Return "Red"
        Else
            Return "inherit"
        End If
    End Function

    ''' <summary>
    ''' Liefert die ImageUrl zum angegebenen Wert
    ''' </summary>
    ''' <param name="Value">zu prüfender Wert</param>
    ''' <returns>ImageUrl als String</returns>
    ''' <remarks></remarks>
    Private Function getAmpel(ByVal value As String) As String
        Select Case value
            Case "4"
                Return "..\Images\Gruener Smiley.png"
            Case "3"
                Return "..\Images\Gruener Kreis.png"
            Case "2"
                Return "..\Images\Gelber Kreis.png"
            Case Else
                Return "..\Images\Roter Kreis.png"
        End Select
    End Function

End Class
