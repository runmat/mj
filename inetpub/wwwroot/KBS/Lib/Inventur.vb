Imports KBSBase
Imports GeneralTools.Models

Public Class Inventur
    Inherits ErrorHandlingClass

#Region "Declarations"

    Protected mProduktierarchie As DataTable
    Protected mInvMaterialien As DataTable
    Protected mMatEAN As DataTable
    Protected mZaehlung As DataTable
    Protected mMyKasse As Kasse
    Protected mProdHNr As String
    Protected mMaterialNr As String
    Protected mErfTyp As String
    Protected mProdHBezeichnung As String
    Protected mInvBelegNr As String
    Protected mInvTyp As String
    Protected mInvDat As String
    Protected mFilepath As String
    Protected mstrKunnrUtschFiliale As String
    Dim mHistorie As DataTable
    Dim mHistorieLength As Integer

#End Region

#Region "Properties"

    Public Property KunnrUtschFiliale() As String
        Get
            Return mstrKunnrUtschFiliale
        End Get
        Set(ByVal value As String)
            mstrKunnrUtschFiliale = value
        End Set
    End Property

    Public Property Produktierarchie() As DataTable
        Get
            Return mProduktierarchie
        End Get
        Set(ByVal Value As DataTable)
            mProduktierarchie = Value
        End Set
    End Property

    Public Property InvMaterialien() As DataTable
        Get
            Return mInvMaterialien
        End Get
        Set(ByVal Value As DataTable)
            mInvMaterialien = Value
        End Set
    End Property

    Public Property MaterialNr() As String
        Get
            Return mMaterialNr
        End Get
        Set(ByVal Value As String)
            mMaterialNr = Value
        End Set
    End Property

    Public Property Zaehlung() As DataTable
        Get
            Return mZaehlung
        End Get
        Set(ByVal Value As DataTable)
            mZaehlung = Value
        End Set
    End Property

    Public Property ProdHNr() As String
        Get
            Return mProdHNr
        End Get
        Set(ByVal Value As String)
            mProdHNr = Value
        End Set
    End Property

    Public Property ErfTyp() As String
        Get
            Return mErfTyp
        End Get
        Set(ByVal Value As String)
            mErfTyp = Value
        End Set
    End Property

    Public Property ProdHBezeichnung() As String
        Get
            Return mProdHBezeichnung
        End Get
        Set(ByVal Value As String)
            mProdHBezeichnung = Value
        End Set
    End Property

    Public Property Filepath() As String
        Get
            Return mFilepath
        End Get
        Set(ByVal Value As String)
            mFilepath = Value
        End Set
    End Property

    Public Property InvBelegNr() As String
        Get
            Return mInvBelegNr
        End Get
        Set(ByVal Value As String)
            mInvBelegNr = Value
        End Set
    End Property

    Public Property InvTyp() As String
        Get
            Return mInvTyp
        End Get
        Set(ByVal Value As String)
            mInvTyp = Value
        End Set
    End Property

    Public Property tblHistorie() As DataTable
        Get
            Return mHistorie
        End Get
        Set(ByVal Value As DataTable)
            mHistorie = Value
        End Set
    End Property

#End Region

    Public Sub AddHistorieEntry(ByVal EAN As String, ByVal MATBEZ As String, ByVal MATNR As String, ByVal Menge_Gesamt As Integer, ByVal Menge As Integer, ByVal Status As String)

        Do Until mHistorie.Rows.Count < mHistorieLength
            mHistorie.Rows(0).Delete()
        Loop

        Dim insertedRow() As DataRow = mHistorie.Select("MATNR = '" + MATNR + "'")

        If insertedRow.Length = 1 Then

            insertedRow(0)("Menge_Gesamt") = Menge_Gesamt
            insertedRow(0)("Menge_erfasst") = Menge
            insertedRow(0)("Status") = Status

        ElseIf insertedRow.Length = 0 Then
            Dim NRow As DataRow = mHistorie.NewRow()

            NRow("Index") = 99
            NRow("MAKTX") = MATBEZ
            NRow("EAN") = EAN
            NRow("MATNR") = MATNR
            NRow("Menge_Gesamt") = Menge_Gesamt
            NRow("Menge_erfasst") = Menge
            NRow("Status") = Status

            mHistorie.Rows.Add(NRow)
        End If
        Dim Count As Integer = 1

        For Each dRow As DataRow In mHistorie.Rows
            dRow("Index") = Count
            Count += 1
        Next

    End Sub

    Public Sub ClearHistorie()
        mHistorie.Rows.Clear()
    End Sub

    Public Sub New(ByRef Kasse As Kasse)
        mMyKasse = Kasse

        If mMyKasse.Werk = "1030" Then
            mHistorie = New DataTable()
            mHistorie.Columns.Add("Index", GetType(Integer))
            mHistorie.Columns.Add("MAKTX", GetType(String))
            mHistorie.Columns.Add("EAN", GetType(String))
            mHistorie.Columns.Add("MATNR", GetType(String))
            mHistorie.Columns.Add("Menge_Gesamt", GetType(Integer))
            mHistorie.Columns.Add("Menge_erfasst", GetType(Integer))
            mHistorie.Columns.Add("Status", GetType(String))
            mHistorieLength = 20
        End If

    End Sub

    Public Function getArtikel(ByVal EAN As String, ByRef Materialnummer As String, ByRef Artikelbezeichnung As String) As Boolean

        Dim tmpRows As DataRow() = InvMaterialien.Select("EAN11='" & EAN & "'")

        If tmpRows IsNot Nothing AndAlso tmpRows.Count > 0 Then
            Materialnummer = tmpRows(0)("MATNR")
            Artikelbezeichnung = tmpRows(0)("MAKTX")
            Return True
        Else
            If getEANFromSAPERP(EAN, Materialnummer, Artikelbezeichnung) Then
                Return Not ErrorOccured
            Else
                Return False
            End If
        End If
    End Function

    Public Function Create_ZaehlungTab() As DataTable
        Dim tblSAP As New DataTable()
        tblSAP.Columns.Add("MATNR", String.Empty.GetType)
        tblSAP.Columns.Add("ERFMG", String.Empty.GetType)
        Return tblSAP
    End Function

    Public Sub FillProdukthierarchieERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_INV_GET_PRODH")

            S.AP.SetImportParameter("I_VKORG", mMyKasse.Werk)
            S.AP.SetImportParameter("I_VKBUR", mMyKasse.Lagerort)
            S.AP.SetImportParameter("I_INVDAT", DateTime.Now.ToShortDateString())

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            mProduktierarchie = S.AP.GetExportTable("GT_PRODH")
            mInvTyp = S.AP.GetExportParameter("E_INVTYP")
            mInvBelegNr = S.AP.GetExportParameter("E_INVBELNR")
            mInvDat = S.AP.GetExportParameter("E_INVDAT")

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub FillInventurMaterialienERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_INV_GET_MATERIAL")

            S.AP.SetImportParameter("I_VKORG", mMyKasse.Werk)
            S.AP.SetImportParameter("I_VKBUR", mMyKasse.Lagerort)
            S.AP.SetImportParameter("I_PRODH", mProdHNr)
            S.AP.SetImportParameter("I_INVBELNR", mInvBelegNr)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            mInvMaterialien = S.AP.GetExportTable("GT_MAT")
            mInvMaterialien.Columns.Add("Delete")
            For Each drow As DataRow In mInvMaterialien.Rows
                drow("MATNR") = drow("MATNR").ToString.TrimStart("0"c)
                If IsNumeric(drow("ERFMG").ToString) Then
                    drow("ERFMG") = CInt(drow("ERFMG")).ToString
                Else
                    drow("ERFMG") = "0"
                End If
                drow("Delete") = False
            Next

            mstrKunnrUtschFiliale = S.AP.GetExportParameter("E_KUNNR")

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub SetMengeMaterialERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_INV_SET_MNG")

            S.AP.SetImportParameter("I_VKORG", mMyKasse.Werk)
            S.AP.SetImportParameter("I_VKBUR", mMyKasse.Lagerort)
            S.AP.SetImportParameter("I_INVBELNR", mInvBelegNr)

            Dim tblSap As DataTable = S.AP.GetImportTable("GT_ZAEHLUNG")

            For Each dRow As DataRow In mZaehlung.Rows
                Dim newRow As DataRow = tblSap.NewRow()
                newRow("MATNR") = dRow("MATNR")
                newRow("ERFMG") = dRow("ERFMG")
                tblSap.Rows.Add(newRow)
            Next

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Private Function getEANFromSAPERP(ByVal EAN As String, ByRef Materialnummer As String, ByRef Artikelbezeichnung As String) As Boolean
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_READ_MATNR_001")

            S.AP.SetImportParameter("I_EAN11", Left(EAN, 13))
            S.AP.SetImportParameter("I_KOSTL", mMyKasse.Lagerort)
            S.AP.SetImportParameter("I_PRODH", ProdHNr)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            If ErrorCode = "103" Then
                RaiseError("103", "Artikel kann nicht hinzugefügt werden, da er für Ihre Filiale gelöscht oder nicht angelegt ist. " & _
                            "Bitte wenden Sie sich an die Firmenzentrale, Abteilung Einkauf.")
            End If

            Materialnummer = S.AP.GetExportParameter("E_MATNR")
            If Not String.IsNullOrEmpty(Materialnummer) Then
                Artikelbezeichnung = S.AP.GetExportParameter("E_MAKTX")
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
            Return False
        End Try
    End Function

    Public Sub SetInventurEndERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_INV_SET_INV")

            S.AP.SetImportParameter("I_INVBELNR", mInvBelegNr)
            S.AP.SetImportParameter("I_VKORG", mMyKasse.Werk)
            S.AP.SetImportParameter("I_VKBUR", mMyKasse.Lagerort)
            S.AP.SetImportParameter("I_INVDAT", DateTime.Now.ToShortDateString())

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Shadows Sub SetMengeMaterialKBSERP(ByVal MATNR As String, ByVal Menge As Integer, ByVal ADD As String)
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_EFA_INV_SET_MNG_KBS")

            S.AP.SetImportParameter("I_VKORG", mMyKasse.Werk)
            S.AP.SetImportParameter("I_VKBUR", mMyKasse.Lagerort)
            S.AP.SetImportParameter("I_INVBELNR", mInvBelegNr)
            S.AP.SetImportParameter("I_MATNR", MATNR)
            S.AP.SetImportParameter("I_ERFMG", Menge)
            S.AP.SetImportParameter("I_ADD", ADD)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Shadows Function getEANFromSAPKBSERP(ByVal EAN As String, ByRef Materialnummer As String, ByRef Artikelbezeichnung As String, ByRef Menge As String) As Boolean
        ClearErrorState()

        Try
            S.AP.Init("Z_FIL_READ_MATNR_KBS")

            S.AP.SetImportParameter("I_EAN11", EAN.TrimStart("0"c))
            S.AP.SetImportParameter("I_KOSTL", mMyKasse.Lagerort)
            S.AP.SetImportParameter("I_PRODH", ProdHNr)
            S.AP.SetImportParameter("I_INVBELNR", mInvBelegNr)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            Materialnummer = S.AP.GetExportParameter("E_MATNR")
            If Not String.IsNullOrEmpty(Materialnummer) Then
                Artikelbezeichnung = S.AP.GetExportParameter("E_MAKTX")
                Menge = S.AP.GetExportParameter("E_ERFMG")
                If Not Menge.IsNumeric Then
                    Menge = "0"
                End If
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
            Return False
        End Try
    End Function

End Class
