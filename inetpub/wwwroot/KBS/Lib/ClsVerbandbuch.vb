Imports System.Data
Imports KBSBase

Public Class ClsVerbandbuch
    Inherits ErrorHandlingClass

    Private dtEntries As DataTable

    Private strID As String = ""
    Private strVkbur As String = ""

    Private strNameVerl As String = ""
    Private strNameZeug As String = ""
    Private strNameErstHelf As String = ""
    Private strVerletzung As String = ""
    Private strOrt As String = ""
    Private datUnfall As String
    Private datErstHelf As String
    Private ZeitUnfall As String = ""
    Private ZeitErstHelf As String = ""
    Private UnfallHergang As String = ""
    Private ErstHelfHandlung As String = ""



#Region "Accessoren"

    Public ReadOnly Property Entries() As DataTable
        Get
            If dtEntries Is Nothing Then
                dtEntries = New DataTable
                dtEntries.TableName = "Eintrag"
                dtEntries.Columns.Add("ID", String.Empty.GetType)
                dtEntries.Columns.Add("VKBUR", String.Empty.GetType)
                dtEntries.Columns.Add("NAME_VERL", String.Empty.GetType)
                dtEntries.Columns.Add("NAME_ZEUG", String.Empty.GetType)
                dtEntries.Columns.Add("ART_VERL", String.Empty.GetType)
                dtEntries.Columns.Add("ORT", String.Empty.GetType)
                dtEntries.Columns.Add("DATUM_UNF", String.Empty.GetType)
                dtEntries.Columns.Add("ZEIT_UNF", String.Empty.GetType)
                dtEntries.Columns.Add("DATUM_HILF", String.Empty.GetType)
                dtEntries.Columns.Add("ZEIT_HILF", String.Empty.GetType)
                dtEntries.Columns.Add("HERGANG", String.Empty.GetType)
                dtEntries.Columns.Add("ART_HILF", String.Empty.GetType)
                dtEntries.Columns.Add("NAME_HELFER", String.Empty.GetType)
            End If
            Return dtEntries
        End Get
    End Property


    Public Property ID() As String
        Get
            Return strID
        End Get
        Set(value As String)
            strID = value
        End Set
    End Property


    Public Property VKBUR() As String
        Get
            Return strVkbur
        End Get
        Set(value As String)
            strVkbur = value
        End Set
    End Property


    Public Property NameVerl() As String
        Get
            Return strNameVerl
        End Get
        Set(value As String)
            strNameVerl = value
        End Set
    End Property

    Public Property NameZeug() As String
        Get
            Return strNameZeug
        End Get
        Set(value As String)
            strNameZeug = value
        End Set
    End Property

    Public Property Verletzung() As String
        Get
            Return strVerletzung
        End Get
        Set(value As String)
            strVerletzung = value
        End Set
    End Property

    Public Property NameHelf() As String
        Get
            Return strNameErstHelf
        End Get
        Set(value As String)
            strNameErstHelf = value
        End Set
    End Property

    Public Property Ort() As String
        Get
            Return strOrt
        End Get
        Set(value As String)
            strOrt = value
        End Set
    End Property

    Public Property DatumUnfall() As Date
        Get
            Return datUnfall
        End Get
        Set(value As DateTime)
            datUnfall = value
        End Set
    End Property

    Public Property DatumHilfe() As Date
        Get
            Return datErstHelf
        End Get
        Set(value As DateTime)
            datErstHelf = value
        End Set
    End Property


    Public Property ZeitUnf() As String
        Get
            Return ZeitUnfall
        End Get
        Set(value As String)
            ZeitUnfall = value
        End Set
    End Property

    Public Property ZeitHilf() As String
        Get
            Return ZeitErstHelf
        End Get
        Set(value As String)
            ZeitErstHelf = value
        End Set
    End Property

    Public Property UnfallHer() As String
        Get
            Return UnfallHergang
        End Get
        Set(value As String)
            UnfallHergang = value
        End Set
    End Property

    Public Property Hilfeleistung() As String
        Get
            Return ErstHelfHandlung
        End Get
        Set(value As String)
            ErstHelfHandlung = value
        End Set
    End Property
#End Region


    Public Sub New()

    End Sub


    Public Sub SaveSAP()

        Try

            Dim tmpTable As DataTable = S.AP.GetImportTableWithInit("Z_VB_IMPORT_FALL.GT_VERBANDBUCH")

            Dim newRow As DataRow = tmpTable.NewRow()

            newRow("VKBUR") = VKBUR
            newRow("NAME_VERL") = NameVerl
            newRow("ZEIT_UNF") = ZeitUnf
            newRow("DATUM_UNF") = DatumUnfall
            newRow("ORT") = Ort
            newRow("HERGANG") = UnfallHergang
            newRow("NAME_ZEUG") = NameZeug
            newRow("ART_VERL") = Verletzung
            newRow("NAME_HELFER") = NameHelf
            newRow("ZEIT_HILF") = ZeitHilf
            newRow("DATUM_HILF") = DatumHilfe
            newRow("ART_HILF") = Hilfeleistung

            tmpTable.Rows.Add(newRow)

            S.AP.Execute()

            If Not S.AP.ResultCode = 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

    End Sub


    Public Sub GetSAP(impVkbur As String)

        ClearErrorState()

        Try
            Entries.Clear()

            S.AP.Init("Z_VB_EXPORT_FAELLE", "I_VKBUR", impVkbur)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            Dim SapTable As DataTable = S.AP.GetExportTable("GT_VERBANDBUCH")
            If SapTable.Rows.Count > 0 Then
                For Each row As DataRow In SapTable.Rows
                    Dim tmprow As DataRow = Entries.NewRow



                    tmprow("ID") = CInt(row("ID")).ToString()
                    tmprow("VKBUR") = row("VKBUR").ToString()
                    tmprow("NAME_VERL") = row("NAME_VERL").ToString()
                    tmprow("DATUM_UNF") = row("DATUM_UNF").ToString().Replace("00:00:00", " ") & row("ZEIT_UNF").Insert(2, ":").Remove(5)
                    tmprow("ORT") = row("ORT").ToString()
                    tmprow("HERGANG") = row("HERGANG").ToString
                    tmprow("NAME_ZEUG") = row("NAME_ZEUG").ToString
                    tmprow("ART_VERL") = row("ART_VERL").ToString
                    tmprow("DATUM_HILF") = row("DATUM_HILF").ToString().Replace("00:00:00", " ") & row("ZEIT_HILF").Insert(2, ":").Remove(5)
                    tmprow("ART_HILF") = row("ART_HILF").ToString
                    tmprow("NAME_HELFER") = row("NAME_HELFER").ToString
                    Entries.Rows.Add(tmprow)
                Next
            Else
                RaiseError("-1", "Keine Positionen vorhanden.")
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try


    End Sub


    


End Class
