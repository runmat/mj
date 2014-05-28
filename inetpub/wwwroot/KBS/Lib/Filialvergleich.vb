
''' <summary>
''' Arbeitsklasse, die Daten aus SAP holt und aufbereitet für den Report Filialziele
''' </summary>
''' <remarks></remarks>
Public Class Filialvergleich

    Private dtFilialauswertung As DataTable
    Private dtFilialvergleich As DataTable
    Private dblRahmenquote As Double
    Private ConStr As String = ""
    Private strLFB As String = ""
    Private strError As String = ""
    Private strDatumFilialVergleich As String = ""
    Private strDatumFilialAuswertung As String = ""

    Public Sub New()
        ConStr = KBS_BASE.SAPConnectionString
    End Sub

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

    Public ReadOnly Property ErrorText As String
        Get
            Return strError
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
        strError = String.Empty

        Dim SapExc As SAPExecutor.SAPExecutor = New SAPExecutor.SAPExecutor(ConStr)

        ' # Filialauswertung Daten holen

        Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

        ' Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object, Feldlänge (0=unbestimmt)}
        ' Import-Parameter
        dt.Rows.Add(New Object() {"I_VKBUR", False, VkBur})
        dt.Rows.Add(New Object() {"I_WOCHE", False, jahrWoche})
        dt.Rows.Add(New Object() {"I_KNDGRP", False, Kundengruppe})
        ' Export-Parameter
        dt.Rows.Add(New Object() {"E_RAHMENQUOTE", True})
        dt.Rows.Add(New Object() {"E_DATE", True})
        dt.Rows.Add(New Object() {"GT_WEB", True})

        Try
            SapExc.ExecuteERP("Z_ADS_FIL_AUSW_001", dt)

            If SapExc.ErrorOccured Then
                strError += SapExc.E_SUBRC & ": " & SapExc.E_MESSAGE + Environment.NewLine
            Else
                dblRahmenquote = dt.Select("Fieldname = 'E_RAHMENQUOTE'")(0)("Data")
                strDatumFilialAuswertung = dt.Select("Fieldname = 'E_DATE'")(0)("Data")
                If strDatumFilialAuswertung = "00000000" Then
                    strDatumFilialAuswertung = ""
                Else
                    Dim datAuswertung As Date = SAPExecutor.SAPExecutor.MakeDateTime(strDatumFilialAuswertung)
                    strDatumFilialAuswertung = datAuswertung.ToShortDateString()
                End If

                dtFilialauswertung = dt.Select("Fieldname = 'GT_WEB'")(0)("Data")
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

            End If
        Catch ex As Exception
            strError += ex.Message & Environment.NewLine
        End Try

        ' #

        ' # Filialvergleich Daten holen

        Dim dt2 As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

        ' Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object, Feldlänge (0=unbestimmt)}
        ' Import-Parameter
        dt2.Rows.Add(New Object() {"I_VKBUR", False, VkBur})
        dt2.Rows.Add(New Object() {"I_WOCHE", False, jahrWoche})
        dt2.Rows.Add(New Object() {"I_KNDGRP", False, Kundengruppe})
        ' Export-Parameter
        dt2.Rows.Add(New Object() {"E_DATE", True})
        dt2.Rows.Add(New Object() {"GT_WEB", True})
        dt2.Rows.Add(New Object() {"GT_RAHMENQUOTE", True})

        Try
            SapExc.ExecuteERP("Z_ADS_FIL_VGL_001", dt2)

            If SapExc.ErrorOccured Then
                strError += SapExc.E_SUBRC & ": " & SapExc.E_MESSAGE + Environment.NewLine
            Else
                strDatumFilialVergleich = dt2.Select("Fieldname = 'E_DATE'")(0)("Data").ToString()
                If strDatumFilialVergleich = "00000000" Then
                    strDatumFilialVergleich = ""
                Else
                    Dim datVergleich As Date = SAPExecutor.SAPExecutor.MakeDateTime(strDatumFilialVergleich)
                    strDatumFilialVergleich = datVergleich.ToShortDateString()
                End If

                dtFilialvergleich = dt2.Select("Fieldname = 'GT_WEB'")(0)("Data")
                Dim dtRahmenquote As DataTable = dt2.Select("Fieldname = 'GT_RAHMENQUOTE'")(0)("Data")

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
            End If
        Catch ex As Exception
            strError += ex.Message
        End Try

        ' #
    End Sub

    ''' <summary>
    ''' Liefert die zuverwendende CSS-Klasse eines String-Wertes
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
