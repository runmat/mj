Imports System
Imports KBSBase

''' <summary>
''' Taggleiche Meldung DAD
''' </summary>
''' <remarks></remarks>
Public Class MeldungDAD
    Inherits ErrorHandlingClass

#Region "Properties"

    Public Property Kostenstelle As String
    Public Property Benutzer As String
    Public Property IDSuche As String
    Public Property FahrgestellnummerSuche As String
    Public Property BriefnummerSuche As String
    Public Property ID As String
    Public Property Bestellnummer As String
    Public Property Frachtbriefnummer As String
    Public Property Fahrgestellnummer As String
    Public Property Briefnummer As String
    Public Property Zulassungsdatum As String
    Public Property Kennzeichen As String
    Public Property Gebuehr As String
    Public Property Auslieferung As Boolean

#End Region

#Region "Methods"

    Public Sub New(ByVal kst As String, ByVal user As String)
        Kostenstelle = kst
        Benutzer = user
    End Sub

    ''' <summary>
    ''' Vorgang zur ID laden. Bapi: Z_ZLD_FIND_DAD_SD_ORDER
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadVorgang()
        ClearErrorState()

        Try
            S.AP.Init("Z_ZLD_FIND_DAD_SD_ORDER", "I_VKORG, I_VKBUR", "1510", Kostenstelle)

            If Not String.IsNullOrEmpty(IDSuche) Then
                S.AP.SetImportParameter("I_VBELN", IDSuche.PadLeft(10, "0"c))
            End If

            If Not String.IsNullOrEmpty(FahrgestellnummerSuche) Then
                S.AP.SetImportParameter("I_FAHRG", FahrgestellnummerSuche)
            End If

            If Not String.IsNullOrEmpty(BriefnummerSuche) Then
                S.AP.SetImportParameter("I_BRIEF", BriefnummerSuche)
            End If

            Dim tmpTable As DataTable = S.AP.GetExportTableWithExecute("E_VBAK")

            If S.AP.ResultCode = 0 Then
                Dim row As DataRow = tmpTable.Rows(0)
                ID = row("VBELN").ToString()
                Bestellnummer = row("EBELN").ToString()
                Frachtbriefnummer = row("ZZSEND2").ToString()
                Fahrgestellnummer = row("ZZFAHRG").ToString()
                Briefnummer = row("ZZBRIEF").ToString()
                Zulassungsdatum = row("VDATU").ToString()
                Kennzeichen = row("ZZKENN").ToString()
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub ClearFields()
        ID = ""
        Bestellnummer = ""
        Frachtbriefnummer = ""
        Fahrgestellnummer = ""
        Briefnummer = ""
        Zulassungsdatum = ""
        Kennzeichen = ""
    End Sub


    ''' <summary>
    ''' Vorgang speichern. Bapi: Z_ZLD_SAVE_TAGGLEICHE_MELDUNG
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SaveVorgang()
        ClearErrorState()

        Try
            Dim tmpTable As DataTable = S.AP.GetImportTableWithInit("Z_ZLD_SAVE_TAGGLEICHE_MELDUNG.IS_TG_MELDUNG")

            Dim newRow As DataRow = tmpTable.NewRow()
            newRow("VKORG") = "1510"
            newRow("VBELN") = ID.PadLeft(10, "0"c)
            newRow("EBELN") = Bestellnummer
            newRow("FAHRG") = Fahrgestellnummer
            newRow("BRIEF") = Briefnummer
            If Common.IsDate(Zulassungsdatum) Then newRow("ZZZLDAT") = Zulassungsdatum
            newRow("ZZKENN") = Kennzeichen
            newRow("GEB_AMT") = Gebuehr
            newRow("AUSLIEF") = Common.BoolToX(Auslieferung)
            newRow("ZZSEND2") = Frachtbriefnummer
            newRow("ERNAM") = Benutzer
            newRow("SAVE_STATUS") = "A"
            tmpTable.Rows.Add(newRow)

            S.AP.Execute()

            If Not S.AP.ResultCode = 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

#End Region

End Class
