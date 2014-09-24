
Public Class Inventur

#Region "Declarations"

    Protected mE_SUBRC As Integer
    Protected mE_MESSAGE As String
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
    Dim SAPExc As SAPExecutor.SAPExecutor

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

    Public Property E_SUBRC() As Integer
        Get
            Return mE_SUBRC
        End Get
        Set(ByVal Value As Integer)
            mE_SUBRC = Value
        End Set
    End Property

    Public Property E_MESSAGE() As String
        Get
            Return mE_MESSAGE
        End Get
        Set(ByVal Value As String)
            mE_MESSAGE = Value
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
                If E_MESSAGE.Length > 0 Then
                    Return False
                Else
                    Return True
                End If

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
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0
        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_VKORG", False, mMyKasse.Werk})
            dt.Rows.Add(New Object() {"I_VKBUR", False, mMyKasse.Lagerort})
            dt.Rows.Add(New Object() {"I_INVDAT", False, Now.ToShortDateString})
            dt.Rows.Add(New Object() {"I_COCKPIT", False, ""})
            'Exportparameter
            dt.Rows.Add(New Object() {"E_INVBELNR", True})
            dt.Rows.Add(New Object() {"E_INVTYP", True})
            dt.Rows.Add(New Object() {"E_INVDAT", True})
            dt.Rows.Add(New Object() {"GT_PRODH", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_INV_GET_PRODH", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If
            Dim retRows As DataRow = dt.Select("Fieldname='GT_PRODH'")(0)
            If Not retRows Is Nothing Then
                mProduktierarchie = DirectCast(retRows("Data"), DataTable)
            End If
            retRows = dt.Select("Fieldname='E_INVTYP'")(0)
            If Not retRows Is Nothing Then
                mInvTyp = retRows("Data").ToString
            End If
            retRows = dt.Select("Fieldname='E_INVBELNR'")(0)
            If Not retRows Is Nothing Then
                mInvBelegNr = retRows("Data").ToString
            End If
            retRows = dt.Select("Fieldname='E_INVDAT'")(0)
            If Not retRows Is Nothing Then
                mInvDat = retRows("Data").ToString
            End If
        Catch ex As Exception
            mE_SUBRC = -9999
            mE_MESSAGE = "Es ist ein Fehler aufgetreten: " & ex.Message
        Finally

        End Try
    End Sub

    Public Sub FillInventurMaterialienERP()

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0
        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_VKORG", False, mMyKasse.Werk})
            dt.Rows.Add(New Object() {"I_VKBUR", False, mMyKasse.Lagerort})
            dt.Rows.Add(New Object() {"I_PRODH", False, mProdHNr})
            dt.Rows.Add(New Object() {"I_INVBELNR", False, mInvBelegNr})
            'Exportparameter
            dt.Rows.Add(New Object() {"E_KUNNR", True})
            dt.Rows.Add(New Object() {"GT_MAT", True})

            SAPExc.ExecuteERP("Z_FIL_EFA_INV_GET_MATERIAL", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If
            Dim retRows As DataRow = dt.Select("Fieldname='GT_MAT'")(0)
            If Not retRows Is Nothing Then
                mInvMaterialien = DirectCast(retRows("Data"), DataTable)
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
            End If
            mstrKunnrUtschFiliale = ""
            retRows = dt.Select("Fieldname='E_KUNNR'")(0)
            If Not retRows Is Nothing Then
                mstrKunnrUtschFiliale = retRows("Data").ToString
            End If

        Catch ex As Exception
            mE_SUBRC = -9999
            mE_MESSAGE = "Es ist ein Fehler aufgetreten: " & ex.Message
        Finally

        End Try

    End Sub

    Public Sub SetMengeMaterialERP()
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0
        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_VKORG", False, mMyKasse.Werk})
            dt.Rows.Add(New Object() {"I_VKBUR", False, mMyKasse.Lagerort})
            dt.Rows.Add(New Object() {"I_INVBELNR", False, mInvBelegNr})
            dt.Rows.Add(New Object() {"GT_ZAEHLUNG", False, mZaehlung})

            SAPExc.ExecuteERP("Z_FIL_EFA_INV_SET_MNG", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If

        Catch ex As Exception
            mE_SUBRC = -9999
            mE_MESSAGE = "Es ist ein Fehler aufgetreten: " & ex.Message
        Finally

        End Try
    End Sub

    Private Function getEANFromSAPERP(ByVal EAN As String, ByRef Materialnummer As String, ByRef Artikelbezeichnung As String) As Boolean

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0
        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_EAN11", False, Left(EAN, 13)})
            dt.Rows.Add(New Object() {"I_KOSTL", False, mMyKasse.Lagerort})
            dt.Rows.Add(New Object() {"I_PRODH", False, ProdHNr})
            'Exportparameter
            dt.Rows.Add(New Object() {"E_MATNR", True})
            dt.Rows.Add(New Object() {"E_MAKTX", True})

            SAPExc.ExecuteERP("Z_FIL_READ_MATNR_001", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If
            If E_SUBRC = 103 Then
                E_MESSAGE = "Artikel kann nicht hinzugefügt werden, da er für Ihre Filiale gelöscht oder nicht angelegt ist. " & _
                            "Bitte wenden Sie sich an die Firmenzentrale, Abteilung Einkauf."
            End If
            Dim retRows As DataRow = dt.Select("Fieldname='E_MATNR'")(0)
            If Not retRows Is Nothing Then
                Materialnummer = retRows("Data").ToString
                If Not Materialnummer.Trim(" ") = String.Empty Then
                    retRows = dt.Select("Fieldname='E_MAKTX'")(0)
                    Artikelbezeichnung = retRows("Data").ToString
                    Return True
                Else
                    Return False
                End If
            End If

        Catch ex As Exception
            mE_SUBRC = -9999
            mE_MESSAGE = "Es ist ein Fehler aufgetreten: " & ex.Message
            Return False
        Finally

        End Try
    End Function

    Public Sub SetInventurEndERP()

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0
        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_INVBELNR", False, mInvBelegNr})
            dt.Rows.Add(New Object() {"I_VKORG", False, mMyKasse.Werk})
            dt.Rows.Add(New Object() {"I_VKBUR", False, mMyKasse.Lagerort})
            dt.Rows.Add(New Object() {"I_INVDAT", False, Now.ToShortDateString})

            SAPExc.ExecuteERP("Z_FIL_EFA_INV_SET_INV", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If

        Catch ex As Exception
            mE_SUBRC = -9999
            mE_MESSAGE = "Es ist ein Fehler aufgetreten: " & ex.Message
        Finally

        End Try
    End Sub

    Public Shadows Sub SetMengeMaterialKBSERP(ByVal MATNR As String, ByVal Menge As Integer, ByVal ADD As String)

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_VKORG", False, mMyKasse.Werk})
            dt.Rows.Add(New Object() {"I_VKBUR", False, mMyKasse.Lagerort})
            dt.Rows.Add(New Object() {"I_INVBELNR", False, mInvBelegNr})
            dt.Rows.Add(New Object() {"I_MATNR", False, MATNR})
            dt.Rows.Add(New Object() {"I_ERFMG", False, Menge})
            dt.Rows.Add(New Object() {"I_ADD", False, ADD})

            SAPExc.ExecuteERP("Z_FIL_EFA_INV_SET_MNG_KBS", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If

        Catch ex As Exception
            mE_SUBRC = -9999
            mE_MESSAGE = "Es ist ein Fehler aufgetreten: " & ex.Message
        End Try

    End Sub

    Public Shadows Function getEANFromSAPKBSERP(ByVal EAN As String, ByRef Materialnummer As String, ByRef Artikelbezeichnung As String, ByRef Menge As String) As Boolean

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_EAN11", False, EAN.TrimStart("0"c)})
            dt.Rows.Add(New Object() {"I_KOSTL", False, mMyKasse.Lagerort})
            dt.Rows.Add(New Object() {"I_PRODH", False, ProdHNr})
            dt.Rows.Add(New Object() {"I_INVBELNR", False, mInvBelegNr})
            'Exportparameter
            dt.Rows.Add(New Object() {"E_MATNR", True})
            dt.Rows.Add(New Object() {"E_MAKTX", True})
            dt.Rows.Add(New Object() {"E_ERFMG", True})

            SAPExc.ExecuteERP("Z_FIL_READ_MATNR_KBS", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If

            Dim retRows As DataRow = dt.Select("Fieldname='E_MATNR'")(0)

            If Not retRows Is Nothing Then
                Materialnummer = retRows("Data").ToString
                If Not Materialnummer.Trim(" ") = String.Empty Then
                    retRows = dt.Select("Fieldname='E_MAKTX'")(0)
                    Artikelbezeichnung = retRows("Data").ToString
                    retRows = dt.Select("Fieldname='E_ERFMG'")(0)
                    If IsNumeric(retRows("Data").ToString) Then
                        Menge = retRows("Data").ToString
                    Else
                        Menge = "0"
                    End If
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            mE_MESSAGE = "Fehler in der Funktion getEANFromSAPKBS!"
            Return False
        Finally
        End Try

    End Function

End Class
