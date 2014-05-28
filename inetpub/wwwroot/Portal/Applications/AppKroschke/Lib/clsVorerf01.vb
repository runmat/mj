Imports System.Configuration
Imports CKG.Base.Kernel

Public Class clsVorerf01

#Region "Structs & Vars"
    Public id_record As Int32
    Public id_usr As Int32
    Public id_sapp As Int32
    Public id_sess As String
    Public kunden_nr As String
    Public kunden_name As String
    Public halter_name As String
    Public internebem As String
    Public stvamt As String
    Public stva_nr As String
    Public wunsch_flag As Boolean
    Public wunsch_kennz As String
    Public wunsch_kennzABC As String
    Public wunsch_kennzNR As String
    Public kztyp As String
    Public str_wunschk As String
    Public dienstleistung_nr As String
    Public dienstleistung_name As String
    Public zulassungs_datum As Date
    Public preisstva As Decimal
    Public preiszulassung As Decimal
    Public preiskennz As Decimal
    Public preispauschal As Decimal
    Public preiskasse As Decimal
    Public to_Delete As String
    Public to_Save As Boolean
    Public issaved As Boolean
    Public check_2 As Boolean
    Public check_3 As Boolean
    Public free_1 As String
    Public free_2 As String
    Public free_3 As String
    Public kundennr_alt As String
    Public tour As String
    Public reserv As Boolean
    Public reservid As String
    Public einkz As Boolean
    Public fremdbest As Boolean
    Public bar As Boolean
    Public sonst_dienst As String
    Public preis_sonstdienst As Decimal
    Public fahrgestell As String
    Public name_1 As String
    Public strasse1 As String
    Public plz1 As String
    Public ort1 As String
    Public sVerkkurz As String
    Public sKIReferenz As String
    Public sNotiz As String
    Public filKunde As String
    Public filSTVA As String
    Public filID As String
    Public filDatum As String

    Private connection As SqlClient.SqlConnection
    Private m_user As Base.Kernel.Security.User
    'Private m_app As Base.Kernel.Security.App
    Private blnVorerfassung As Boolean
    Protected m_SapConnectionString As String
    Private tblKunde As DataTable
    Private tblMat As DataTable
    Private tblStva As DataTable
    Private vko As String
    Private vkb As String

    Private strBarcodeKundennr As String
    Private strBarcodeReferenz1 As String
    Private strBarcodeReferenz2 As String
    Private strBarcodeDienstleistung As String
    Private strBarcodeStva As String
    Private strZUFUnr As String
    Private strAUART As String
    Private bKrad As Boolean
    Private bFeinstaub As Boolean
    Public AufIDSel As String
    Public selectedUser As String
    Private tblUpload As DataTable
    Public Message As String
#End Region

#Region "Properties"

    Public ReadOnly Property PBarcodeKundennr() As String
        Get
            Return strBarcodeKundennr
        End Get
    End Property

    Public ReadOnly Property PBarcodeRef1() As String
        Get
            Return strBarcodeReferenz1
        End Get
    End Property

    Public ReadOnly Property PBarcodeRef2() As String
        Get
            Return strBarcodeReferenz2
        End Get
    End Property

    Public ReadOnly Property PBarcodeStva() As String
        Get
            Return strBarcodeStva
        End Get
    End Property

    Public ReadOnly Property PBarcodeDienst() As String
        Get
            Return strBarcodeDienstleistung
        End Get
    End Property

    Public Property VKOrg() As String
        Get
            Return vko
        End Get
        Set(ByVal Value As String)
            vko = Value
        End Set
    End Property

    Public Property VKBur() As String
        Get
            Return vkb
        End Get
        Set(ByVal Value As String)
            vkb = Value
        End Set
    End Property

    Public Property FilterDatum() As String
        Get
            Return filDatum
        End Get
        Set(ByVal Value As String)
            filDatum = Value
        End Set
    End Property

    Public Property FilterID() As String
        Get
            Return filID
        End Get
        Set(ByVal Value As String)
            filID = Value
        End Set
    End Property

    Public Property FilterKunde() As String
        Get
            Return filKunde
        End Get
        Set(ByVal Value As String)
            filKunde = Value
        End Set
    End Property

    Public Property FilterSTVA() As String
        Get
            Return filSTVA
        End Get
        Set(ByVal Value As String)
            filSTVA = Value
        End Set
    End Property


    Public ReadOnly Property Kundentabelle() As DataTable
        Get
            Return tblKunde
        End Get
    End Property

    Public ReadOnly Property Materialtabelle() As DataTable
        Get
            Return tblMat
        End Get
    End Property

    Public ReadOnly Property Stvatabelle() As DataTable
        Get
            Return tblStva
        End Get
    End Property

    Public Property Vorerfassung() As Boolean
        Get
            Return blnVorerfassung
        End Get
        Set(ByVal Value As Boolean)
            blnVorerfassung = Value
        End Set
    End Property

    Public Property Kassengebuehr() As Decimal
        Get
            Return preiskasse
        End Get
        Set(ByVal Value As Decimal)
            preiskasse = Value
        End Set
    End Property

    Public Property preisSonstDienst() As Decimal
        Get
            Return preis_sonstdienst
        End Get
        Set(ByVal Value As Decimal)
            preis_sonstdienst = Value
        End Set
    End Property

    Public Property internebemerkung() As String
        Get
            Return internebem
        End Get
        Set(ByVal Value As String)
            internebem = Value
        End Set
    End Property

    Public Property fahrgestellnr() As String
        Get
            Return fahrgestell
        End Get
        Set(ByVal Value As String)
            fahrgestell = Value
        End Set
    End Property

    Public Property sonstDienst() As String
        Get
            Return sonst_dienst
        End Get
        Set(ByVal Value As String)
            sonst_dienst = Value
        End Set
    End Property

    Public Property barkunde() As Boolean
        Get
            Return bar
        End Get
        Set(ByVal Value As Boolean)
            bar = Value
        End Set
    End Property

    Public Property fremdbestand() As Boolean
        Get
            Return fremdbest
        End Get
        Set(ByVal Value As Boolean)
            fremdbest = Value
        End Set
    End Property

    Public Property kennztyp() As String
        Get
            Return kztyp
        End Get
        Set(ByVal Value As String)
            kztyp = Value
        End Set
    End Property

    Public Property einkennz() As Boolean
        Get
            Return einkz
        End Get
        Set(ByVal Value As Boolean)
            einkz = Value
        End Set
    End Property

    Public Property reserviertid() As String
        Get
            Return reservid
        End Get
        Set(ByVal Value As String)
            reservid = Value
        End Set
    End Property

    Public Property wunschkennzflag() As Boolean
        Get
            Return wunsch_flag
        End Get
        Set(ByVal Value As Boolean)
            wunsch_flag = Value
        End Set
    End Property

    Public Property reserviert() As Boolean
        Get
            Return reserv
        End Get
        Set(ByVal Value As Boolean)
            reserv = Value
        End Set
    End Property

    Public Property kundennralt() As String
        Get
            Return kundennr_alt
        End Get
        Set(ByVal Value As String)
            kundennr_alt = Value
        End Set
    End Property

    Public Property tournr() As String
        Get
            Return tour
        End Get
        Set(ByVal Value As String)
            tour = Value
        End Set
    End Property

    Public Property id_recordset() As Int32
        Get
            Return id_record
        End Get
        Set(ByVal Value As Int32)
            id_record = Value
        End Set
    End Property
    Public Property id_sap() As Int32
        Get
            Return id_sapp
        End Get
        Set(ByVal Value As Int32)
            id_sapp = Value
        End Set
    End Property
    Public Property id_user() As Int32
        Get
            Return id_usr
        End Get
        Set(ByVal Value As Int32)
            id_usr = Value
        End Set
    End Property

    Public Property id_session() As String
        Get
            Return id_sess
        End Get
        Set(ByVal Value As String)
            id_sess = Value
        End Set
    End Property

    Public Property kundennr() As String
        Get
            Return kunden_nr
        End Get
        Set(ByVal Value As String)
            kunden_nr = Value
        End Set
    End Property

    Public Property kundenname() As String
        Get
            Return kunden_name
        End Get
        Set(ByVal Value As String)
            kunden_name = Value
        End Set
    End Property

    Public Property haltername() As String
        Get
            Return halter_name
        End Get
        Set(ByVal Value As String)
            halter_name = Value
        End Set
    End Property

    Public Property stva() As String
        Get
            Return stvamt
        End Get
        Set(ByVal Value As String)
            stvamt = Value
        End Set
    End Property

    Public Property stvanr() As String
        Get
            Return stva_nr
        End Get
        Set(ByVal Value As String)
            stva_nr = Value
        End Set
    End Property

    Public Property wunschennz() As String
        Get
            Return wunsch_kennz
        End Get
        Set(ByVal Value As String)
            wunsch_kennz = Value
        End Set
    End Property

    Public Property wunschennzABC() As String
        Get
            Return wunsch_kennzABC
        End Get
        Set(ByVal Value As String)
            wunsch_kennzABC = Value
        End Set
    End Property

    Public Property wunschennzNR() As String
        Get
            Return wunsch_kennzNR
        End Get
        Set(ByVal Value As String)
            wunsch_kennzNR = Value
        End Set
    End Property

    Public Property str_wunschkennz() As String
        Get
            Return str_wunschk
        End Get
        Set(ByVal Value As String)
            str_wunschk = Value
        End Set
    End Property

    Public Property dienstleistungnr() As String
        Get
            Return dienstleistung_nr
        End Get
        Set(ByVal Value As String)
            dienstleistung_nr = Value
        End Set
    End Property

    Public Property dienstleistung() As String
        Get
            Return dienstleistung_name
        End Get
        Set(ByVal Value As String)
            dienstleistung_name = Value
        End Set
    End Property

    Public Property zulassungsdatum() As Date
        Get
            Return zulassungs_datum
        End Get
        Set(ByVal Value As Date)
            zulassungs_datum = Value
        End Set
    End Property

    Public Property preis_stva() As Decimal
        Get
            Return preisstva
        End Get
        Set(ByVal Value As Decimal)
            preisstva = Value
        End Set
    End Property

    Public Property preis_zulassung() As Decimal
        Get
            Return preiszulassung
        End Get
        Set(ByVal Value As Decimal)
            preiszulassung = Value
        End Set
    End Property
    Public Property preis_kennz() As Decimal
        Get
            Return preiskennz
        End Get
        Set(ByVal Value As Decimal)
            preiskennz = Value
        End Set
    End Property
    Public Property preis_pauschal() As Decimal
        Get
            Return preispauschal
        End Get
        Set(ByVal Value As Decimal)
            preispauschal = Value
        End Set
    End Property

    Public Property saved() As Boolean
        Get
            Return issaved
        End Get
        Set(ByVal Value As Boolean)
            issaved = Value
        End Set
    End Property

    Public Property tosave() As Boolean
        Get
            Return to_Save
        End Get
        Set(ByVal Value As Boolean)
            to_Save = Value
        End Set
    End Property

    Public Property toDelete() As String
        Get
            Return to_Delete
        End Get
        Set(ByVal Value As String)
            to_Delete = Value
        End Set
    End Property

    Public Property check2() As Boolean
        Get
            Return check_2
        End Get
        Set(ByVal Value As Boolean)
            check_2 = Value
        End Set
    End Property

    Public Property check3() As Boolean
        Get
            Return check_3
        End Get
        Set(ByVal Value As Boolean)
            check_3 = Value
        End Set
    End Property

    Public Property free1() As String
        Get
            Return free_1
        End Get
        Set(ByVal Value As String)
            free_1 = Value
        End Set
    End Property

    Public Property free2() As String
        Get
            Return free_2
        End Get
        Set(ByVal Value As String)
            free_2 = Value
        End Set
    End Property

    Public Property free3() As String
        Get
            Return free_3
        End Get
        Set(ByVal Value As String)
            free_3 = Value
        End Set
    End Property

    Public Property name1() As String
        Get
            Return name_1
        End Get
        Set(ByVal Value As String)
            name_1 = Value
        End Set
    End Property

    Public Property strasse() As String
        Get
            Return strasse1
        End Get
        Set(ByVal Value As String)
            strasse1 = Value
        End Set
    End Property

    Public Property plz() As String
        Get
            Return plz1
        End Get
        Set(ByVal Value As String)
            plz1 = Value
        End Set
    End Property

    Public Property ort() As String
        Get
            Return ort1
        End Get
        Set(ByVal Value As String)
            ort1 = Value
        End Set
    End Property
    Public Property Verkkurz() As String
        Get
            Return sVerkkurz
        End Get
        Set(ByVal Value As String)
            sVerkkurz = Value
        End Set
    End Property
    Public Property KIReferenz() As String
        Get
            Return sKIReferenz
        End Get
        Set(ByVal Value As String)
            sKIReferenz = Value
        End Set
    End Property
    Public Property Notiz() As String
        Get
            Return sNotiz
        End Get
        Set(ByVal Value As String)
            sNotiz = Value
        End Set
    End Property
    Public Property PZUEFUE() As String
        Get
            Return strZUFUnr
        End Get
        Set(ByVal Value As String)
            strZUFUnr = Value
        End Set
    End Property

    Public Property PAUART() As String
        Get
            Return strAUART
        End Get
        Set(ByVal Value As String)
            strAUART = Value
        End Set
    End Property
    Public Property Krad() As Boolean
        Get
            Return bKrad
        End Get
        Set(ByVal Value As Boolean)
            bKrad = Value
        End Set
    End Property
    Public Property Feinstaub() As Boolean
        Get
            Return bFeinstaub
        End Get
        Set(ByVal Value As Boolean)
            bFeinstaub = Value
        End Set
    End Property

    Public Property SelAufID() As String
        Get
            Return AufIDSel
        End Get
        Set(ByVal Value As String)
            AufIDSel = Value
        End Set
    End Property

    Public Property SelUser() As String
        Get
            Return selectedUser
        End Get
        Set(ByVal Value As String)
            selectedUser = Value
        End Set
    End Property
    Public Property TabUpload() As DataTable
        Get
            Return tblUpload
        End Get
        Set(ByVal Value As DataTable)
            tblUpload = Value
        End Set
    End Property
#End Region

    Public Sub New(ByVal aUser As Base.Kernel.Security.User, ByVal bVorerfassung As Boolean)
        'm_app = New Base.Kernel.Security.App(aUser)

        m_user = aUser
        blnVorerfassung = bVorerfassung

        tblKunde = New DataTable()
        tblStva = New DataTable()
        tblMat = New DataTable()
        filSTVA = String.Empty
        filDatum = String.Empty
        filKunde = String.Empty
        filID = String.Empty
        Message = String.Empty

    End Sub

    Private Sub openConnection()
        connection = New SqlClient.SqlConnection()
        connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
        connection.Open()
    End Sub

    Private Sub closeConnection()
        connection.Close()
        connection.Dispose()
    End Sub

    Public Sub dbClear(ByVal ctext As String, ByRef status As String)
        Dim command As New SqlClient.SqlCommand()
        Try
            openConnection()
            With command
                .Connection = connection
                .CommandType = CommandType.Text
                .CommandText = "DELETE FROM Zulassung WHERE id_sap IN (" & ctext & ") OR (toDelete = 'X' AND vorerfassung = 1)"  'Daten aus SQL löschen
            End With
            If ctext <> String.Empty Then
                command.ExecuteNonQuery()
            End If
        Catch ex As Exception
            status = ex.Message
        Finally
            closeConnection()
        End Try
    End Sub

    Public Function toSAPDate(ByVal datInput As Date) As String
        Return Year(datInput) & Right("0" & Month(datInput), 2) & Right("0" & Day(datInput), 2)
    End Function

    Public Sub writeZTable(ByVal table As DataTable, ByRef status As String, ByVal userid As String, ByRef deleteID As String)
        'Z-Tabelle füllen (Vorerfassung)
        Dim row As DataRow
        Dim rowTable As DataRow()
        Dim tBelege As DataTable
        Dim fehlerTabelle As DataTable

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_F_CK_BELEGVORERFASSUNG", m_app, m_user, page)
            'tBelege = myProxy.getImportTable("IT_BELEG")

            tBelege = S.AP.GetImportTableWithInit("Z_F_CK_BELEGVORERFASSUNG.IT_BELEG")

            For Each row In table.Rows
                If (CType(row("toSave"), Int16) <> 0) AndAlso (CType(row("toDelete"), String) <> "L") Then

                    With tBelege
                        Dim tmpRow As DataRow = .NewRow()

                        tmpRow("ID") = Right("0000000000" & row("id_sap").ToString, 10)
                        tmpRow("VKORG") = VKOrg
                        tmpRow("VKBUR") = VKBur

                        tmpRow("KREIS") = row("STVANR").ToString
                        tmpRow("KUNNR") = row("KUNDENNR").ToString
                        tmpRow("MATNR") = row("DIENSTLEISTUNGNR").ToString
                        tmpRow("MENGE") = 1
                        tmpRow("PREISDL") = 0
                        tmpRow("GEBAUSL") = row("preis_stva").ToString.Replace(","c, "."c)
                        tmpRow("ZZKENN") = row("STR_WUNSCHKENNZ").ToString
                        tmpRow("ZZHALTER") = row("HALTERNAME").ToString
                        tmpRow("ZZTEXT") = row("INTERNEBEMERKUNG").ToString
                        tmpRow("ZZFAHRG") = row("FAHRGESTELLNR").ToString
                        tmpRow("ZZZLDAT") = CType(row("zulassungsdatum"), Date)
                        tmpRow("ZZSPERR") = ""
                        tmpRow("USERID") = Right("0000000000" & userid, 10)
                        tmpRow("USERNAME") = m_user.UserName

                        If CType(row("RESERV"), Boolean) = True Then
                            tmpRow("RESERVIERT") = "X"
                        Else
                            tmpRow("RESERVIERT") = ""
                        End If

                        If CType(row("WUNSCHKENNZFLAG"), Boolean) = True Then
                            tmpRow("ZZWUNSCH") = "X"
                        Else
                            tmpRow("ZZWUNSCH") = ""
                        End If

                        tmpRow("RESERVID") = row("RESERVID").ToString
                        tmpRow("KVGR4") = row("TOUR").ToString
                        If row("KUNDENNR_ALT").ToString.Length > 10 Then
                            tmpRow("ALTKN") = row("KUNDENNR_ALT").ToString.Substring(0, 10)
                        Else
                            tmpRow("ALTKN") = row("KUNDENNR_ALT").ToString
                        End If
                        If CType(row("EINKZ"), Boolean) = True Then
                            tmpRow("EINKZ") = "X"
                        Else
                            tmpRow("EINKZ") = ""
                        End If
                        tmpRow("KENNZTYP") = row("KENNZTYP").ToString
                        If CType(row("FREMDBESTAND"), Boolean) = True Then
                            tmpRow("KENNZFREMD") = "X"
                        Else
                            tmpRow("KENNZFREMD") = ""
                        End If
                        If CType(row("BARKUNDE"), Boolean) = True Then
                            tmpRow("KALKS") = "X"
                        Else
                            tmpRow("KALKS") = ""
                        End If
                        'Anlieferadresse
                        tmpRow("NAME1") = row("NAME1").ToString
                        tmpRow("STRAS") = row("STRASSE").ToString
                        tmpRow("PSTLZ") = row("PLZ").ToString
                        tmpRow("ORT01") = row("ORT").ToString
                        tmpRow("ZVERKAEUFER") = row("Vkkurz").ToString
                        tmpRow("ZKUNDNOTIZ") = row("Notiz").ToString
                        tmpRow("ZKUNDREF") = row("KIReferenz").ToString

                        Dim sAuart As String = row("AUART").ToString.Trim
                        If sAuart = "ZÜFÜ" Then
                            tmpRow("ZVERBVBELN") = row("VerbVbeln").ToString.Trim
                            tmpRow("FAKSK") = "04"
                        End If

                        If CType(row("krad"), Boolean) = True Then
                            tmpRow("ZKRAD_KZ") = "X"
                        End If

                        If CType(row("Feinstaub"), Boolean) = True Then
                            tmpRow("ZFEINSTAUB_KZ") = "X"
                        End If

                        .Rows.Add(tmpRow)
                    End With
                End If
            Next

            tBelege.AcceptChanges()

            If tBelege.Rows.Count > 0 Then
                'myProxy.callBapi()
                S.AP.Execute()

                fehlerTabelle = S.AP.GetExportTable("ET_FEHLER")

                For Each row In fehlerTabelle.Rows
                    rowTable = table.Select("id_sap = " & row("Id").ToString)
                    If row("Fehlertext").ToString = String.Empty Then
                        rowTable(0)("status") = "Vorgang OK"
                        deleteID &= rowTable(0)("id_sap").ToString & ","
                    Else
                        rowTable(0)("status") = "Fehler: " & row("Fehlertext").ToString
                    End If
                Next

                If deleteID <> String.Empty Then
                    deleteID = Left(deleteID, deleteID.Length - 1)  'Das letzte Komma entfernen
                End If
            End If
        Catch ex As Exception
            status = ex.Message
        End Try
    End Sub

    Public Sub writeUploadTable(ByVal table As DataTable, ByRef status As String, ByVal userid As String, ByRef deleteID As String)
        'Z-Tabelle füllen (Vorerfassung)
        Dim row As DataRow
        Dim rowTable As DataRow()
        Dim tBelege As DataTable
        Dim fehlerTabelle As DataTable

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_F_Ck_Belegvorerfassung", m_app, m_user, page)
            'tBelege = myProxy.getImportTable("IT_BELEG")

            tBelege = S.AP.GetImportTableWithInit("Z_F_CK_BELEGVORERFASSUNG.IT_BELEG")

            For Each row In table.Rows
                If (CType(row("OK"), Boolean) = True) AndAlso (CType(row("Del"), Boolean) = False) Then

                    With tBelege
                        Dim tmpRow As DataRow = .NewRow()

                        tmpRow("ID") = Right("0000000000" & row("id_sap").ToString, 10)
                        id_sap = row("id_sap").ToString
                        tmpRow("VKORG") = VKOrg
                        tmpRow("VKBUR") = VKBur
                        tmpRow("KREIS") = row("KREIS").ToString
                        tmpRow("KUNNR") = Right("0000000000" & row("KUNNR_AG").ToString, 10)
                        tmpRow("MATNR") = row("MATNR").ToString
                        tmpRow("MENGE") = 1
                        tmpRow("PREISDL") = 0
                        tmpRow("GEBAUSL") = CType(row("Gebausl"), Decimal)
                        tmpRow("ZZKENN") = row("ZZKENN").ToString
                        tmpRow("ZZHALTER") = row("ZZHALTER").ToString
                        tmpRow("ZZTEXT") = row("ZZTEXT").ToString
                        tmpRow("ZZFAHRG") = row("ZZFAHRG").ToString
                        tmpRow("ZZZLDAT") = row("ZZZLDAT") 'toSAPDate(CType(row("ZZZLDAT"), Date))
                        tmpRow("ZZSPERR") = ""
                        tmpRow("USERID") = Right("0000000000" & userid, 10)
                        tmpRow("USERNAME") = m_user.UserName
                        tmpRow("ZZLOESCH") = ""
                        tmpRow("RESERVIERT") = row("RESERVIERT")
                        tmpRow("ZZWUNSCH") = row("ZZWUNSCH")
                        tmpRow("RESERVID") = row("RESERVID").ToString
                        tmpRow("KVGR4") = ""
                        tmpRow("ALTKN") = ""
                        tmpRow("EINKZ") = row("EINKZ")
                        tmpRow("KENNZTYP") = row("KENNZTYP").ToString
                        tmpRow("KENNZFREMD") = row("KENNZFREMD").ToString
                        tmpRow("KALKS") = row("KALKS")

                        'Anlieferadresse
                        tmpRow("NAME1") = row("NAME1").ToString
                        tmpRow("STRAS") = row("STRAS").ToString
                        tmpRow("PSTLZ") = row("PSTLZ").ToString
                        tmpRow("ORT01") = row("ORT01").ToString
                        tmpRow("ZVERKAEUFER") = row("ZVERKAEUFER").ToString
                        tmpRow("ZKUNDNOTIZ") = row("ZKUNDNOTIZ").ToString
                        tmpRow("ZKUNDREF") = row("ZKUNDREF").ToString

                        tmpRow("ZKRAD_KZ") = row("ZKRAD_KZ")
                        tmpRow("ZFEINSTAUB_KZ") = row("ZFEINSTAUB_KZ")

                        .Rows.Add(tmpRow)
                    End With
                End If
            Next

            tBelege.AcceptChanges()

            If tBelege.Rows.Count > 0 Then

                'myProxy.callBapi()
                S.AP.Execute()

                fehlerTabelle = S.AP.GetExportTable("ET_FEHLER")

                For Each row In fehlerTabelle.Rows
                    rowTable = table.Select("id_sap = " & row("Id").ToString)
                    If row("Fehlertext").ToString = String.Empty Then
                        rowTable(0)("status") = "Vorgang OK"
                        deleteID &= rowTable(0)("id_sap").ToString & ","
                    Else
                        rowTable(0)("status") = "Fehler: " & row("Fehlertext").ToString
                    End If
                Next

                If deleteID <> String.Empty Then
                    deleteID = Left(deleteID, deleteID.Length - 1)  'Das letzte Komma entfernen
                End If

                SetNewZulassungsID()

            End If
        Catch ex As Exception
            status = ex.Message
        End Try
    End Sub

    Public Sub writeSDTable(ByVal table As DataTable, ByRef status As String, ByVal userid As String, ByRef deleteID As String, ByRef barcode As String, ByRef page As Page)
        'SD-Aufträge anlegen (Nacherfassung)
        Dim row As DataRow
        Dim rowsToSave As DataRow()
        Dim rowTable As DataRow()
        Dim tBelege As DataTable
        Dim fehlerTabelle As DataTable
        Dim m_objLogApp As Base.Kernel.Logging.Trace = New Base.Kernel.Logging.Trace(m_user.App.Connectionstring, True, m_user.App.LogLevel)
        Dim intID As Long


        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("", m_app, m_user, page)
            'tBelege = myProxy.getImportTable("IT_BELEG_SD")

            tBelege = S.AP.GetImportTableWithInit("Z_F_Ck_Belegvorerfassung.IT_BELEG_SD")

            rowsToSave = table.Select("toSave <> 0")
            For Each row In rowsToSave
                With tBelege
                    Dim newRow As DataRow = tBelege.NewRow()
                    newRow("Vkorg") = VKOrg
                    newRow("Vkbur") = VKBur

                    newRow("Id") = Right("0000000000" & row("id_sap").ToString, 10)
                    newRow("Kreis") = row("STVANR").ToString
                    newRow("Kunnr") = row("KUNDENNR").ToString
                    newRow("Matnr") = row("DIENSTLEISTUNGNR").ToString
                    newRow("Menge") = "1"
                    newRow("Userid") = Right("0000000000" & userid, 10)
                    newRow("Preisdl") = CType(row("PREIS_ZULASSUNG"), Decimal)
                    newRow("Gebausl") = CType(row("PREIS_STVA"), Decimal)
                    newRow("Preiskz") = CType(row("PREIS_KENNZ"), Decimal)
                    newRow("Preispausch") = CType(row("PREIS_PAUSCHAL"), Decimal)
                    newRow("Zzkenn") = row("STR_WUNSCHKENNZ").ToString
                    newRow("Zzhalter") = row("HALTERNAME").ToString
                    newRow("Zztext") = row("INTERNEBEMERKUNG").ToString
                    newRow("Zzfahrg") = row("FAHRGESTELLNR").ToString
                    newRow("Zzzldat") = toSAPDate(CType(row("ZULASSUNGSDATUM"), Date))
                    newRow("Zzloesch") = row("TODELETE").ToString
                    newRow("Kennztyp") = row("KENNZTYP").ToString
                    newRow("Kvgr4") = row("TOUR").ToString
                    newRow("Kalks") = ""
                    If row("PREIS_KASSE") Is DBNull.Value Then
                        newRow("Zzkassengba") = 0
                    Else
                        newRow("Zzkassengba") = CType(row("PREIS_KASSE"), Decimal)
                    End If
                    newRow("Username") = m_user.UserName
                    If CType(row("BARKUNDE"), Boolean) = True Then
                        newRow("Kalks") = "X"
                    End If
                    If CType(row("WUNSCHKENNZFLAG"), Boolean) = True Then
                        newRow("Zzwunsch") = "X"
                    Else
                        newRow("Zzwunsch") = ""
                    End If
                    If CType(row("FREMDBESTAND"), Boolean) = True Then
                        newRow("Kennzfremd") = "X"
                    End If
                    If CType(row("EINKZ"), Boolean) = True Then
                        newRow("Einkz") = "X"
                    Else
                        newRow("Einkz") = ""
                    End If
                    newRow("Sonstdl") = row("SONST_DIENST").ToString
                    newRow("Preissonst") = CType(row("PREIS_SONSTDIENST"), Decimal)
                    'Anlieferadresse
                    newRow("Name1") = row("NAME1").ToString
                    newRow("Stras") = row("STRASSE").ToString
                    newRow("Pstlz") = row("PLZ").ToString
                    newRow("Ort01") = row("ORT").ToString
                    If CType(row("Feinstaub"), Boolean) = True Then
                        newRow("Zfeinstaub_Kz") = "X"
                    End If
                    If CType(row("Krad"), Boolean) = True Then
                        newRow("Zkrad_Kz") = "X"
                    End If
                    newRow("Reserviert") = ""
                    If CType(row("reserv"), Boolean) = True Then
                        newRow("Reserviert") = "X"
                    End If

                    tBelege.Rows.Add(newRow)
                End With

            Next

            tBelege.AcceptChanges()

            'intID = m_objLogApp.WriteStartDataAccessSAP(m_user.UserName, m_user.IsTestUser, "Z_F_Ck_Beleg_Sd_Anlegen", "", "", m_user.CurrentLogAccessASPXID)

            S.AP.Execute() 'myProxy.callBapi()

            'If intID > -1 Then
            '    m_objLogApp.WriteEndDataAccessSAP(intID, True)
            'End If

            fehlerTabelle = S.AP.GetExportTable("GT_VBELN")

            For Each row In table.Rows
                rowTable = fehlerTabelle.Select("Id = '" & Right("000000000" & row("id_sap").ToString, 10) & "'")
                If rowTable.GetUpperBound(0) > -1 Then
                    If rowTable(0)("Fehlertext").ToString = String.Empty Then
                        row("status") = "Vorgang OK"
                        deleteID &= row("id_sap").ToString & ","
                    Else
                        row("status") = rowTable(0)("Fehlertext").ToString
                    End If
                Else
                    row("status") = "Vorgang OK"

                    deleteID &= row("id_sap").ToString & ","
                End If
            Next

            If deleteID <> String.Empty Then
                deleteID = Left(deleteID, deleteID.Length - 1)  'Das letzte Komma entfernen
            End If

        Catch ex As Exception
            status = ex.Message
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, status)
            End If
        End Try
    End Sub

    Private Sub dbWrite(ByVal type As String, ByRef returnID As String)
        Dim command As New SqlClient.SqlCommand()
        Dim command2 As New SqlClient.SqlCommand()
        Dim sqlInsert As String
        Dim sqlUpdate As String

        sqlInsert = "INSERT INTO Zulassung (" & _
                            "testuser,vorerfassung,internebemerkung,fahrgestellnr,id_sap,id_user,id_session,username,kundennr,kundenname,haltername,stva,stvanr,wunschkennzflag,wunschkennz," & _
                            "wunschkennzABC,wunschkennzNR,str_wunschkennz,dienstleistungnr,dienstleistung,zulassungsdatum,preis_stva,preis_zulassung, " & _
                            "preis_kennz,preis_pauschal,saved,check2,check3,free1,free2,free3,toSave,toDelete,kennztyp,einkz,reservid,reserv,kundennr_alt,tour,fremdbestand,barkunde,sonst_dienst,preis_sonstdienst,name1,strasse,plz,ort,preis_kasse,Vkkurz,KIReferenz,Notiz,Auart,VerbVbeln,Feinstaub,krad) VALUES (" & _
                            "@testuser,@vorerfassung,@internebemerkung,@fahrgestellnr,@id_sap,@id_user,@id_session,@username,@kundennr,@kundenname,@haltername,@stva,@stvanr,@wunschkennzflag,@wunschkennz," & _
                            "@wunschkennzABC,@wunschkennzNR,@str_wunschkennz,@dienstleistungnr,@dienstleistung,@zulassungsdatum,@preis_stva,@preis_zulassung," & _
                            "@preis_kennz,@preis_pauschal,@saved,@check2,@check3,@free1,@free2,@free3,@toSave,@toDelete,@kennztyp,@einkz,@reservid,@reserv,@kundennr_alt,@tour,@fremdbestand,@barkunde,@sonst_dienst,@preis_sonstdienst,@name1,@strasse,@plz,@ort,@preis_kasse,@Vkkurz,@KIReferenz,@Notiz,@Auart,@VerbVbeln,@Feinstaub,@krad);SELECT SCOPE_IDENTITY()"

        sqlUpdate = "UPDATE Zulassung SET " & _
                            "testuser=@testuser,vorerfassung=@vorerfassung,internebemerkung=@internebemerkung,fahrgestellnr=@fahrgestellnr,id_sap=@id_sap,id_user=@id_user,id_session=@id_session,username=@username,kundennr=@kundennr,kundenname=@kundenname,haltername=@haltername,stva=@stva,stvanr=@stvanr,wunschkennzflag=@wunschkennzflag,wunschkennz=@wunschkennz," & _
                            "wunschkennzABC=@wunschkennzABC,wunschkennzNR=@wunschkennzNR,str_wunschkennz=@str_wunschkennz,dienstleistungnr=@dienstleistungnr,dienstleistung=@dienstleistung,zulassungsdatum=@zulassungsdatum,preis_stva=@preis_stva,preis_zulassung=@preis_zulassung," & _
                            "preis_kennz=@preis_kennz,preis_pauschal=@preis_pauschal,saved=@saved,check2=@check2,check3=@check3,free1=@free1,free2=@free2,free3=@free3,toSave=@toSave,toDelete=@toDelete, " & _
                            "kennztyp=@kennztyp,einkz=@einkz,reservid=@reservid,reserv=@reserv,kundennr_alt=@kundennr_alt,tour=@tour,fremdbestand=@fremdbestand,barkunde=@barkunde,sonst_dienst=@sonst_dienst,preis_sonstdienst=@preis_sonstdienst,name1=@name1,strasse=@strasse,plz=@plz,ort=@ort,preis_kasse=@preis_kasse, " & _
                            "Vkkurz=@Vkkurz,KIReferenz=@KIReferenz,Notiz=@Notiz,Feinstaub=@Feinstaub,krad=@krad " & _
                            "WHERE id = @id_recordset"

        With command
            .Connection = connection
            .CommandType = CommandType.Text
            .Parameters.Clear()
        End With
        With command2
            .Connection = connection
            .CommandType = CommandType.Text
            .Parameters.Clear()
        End With

        If (type = "UPDATE") Then
            'UPDATE
            command.CommandText = sqlUpdate
            command.Parameters.AddWithValue("@id_recordset", id_recordset)
        Else
            'INSERT
            command.CommandText = sqlInsert
        End If

        With command.Parameters
            .AddWithValue("@testuser", m_user.IsTestUser)
            .AddWithValue("@vorerfassung", blnVorerfassung)
            .AddWithValue("@internebemerkung", internebemerkung)
            .AddWithValue("@id_sap", id_sap)
            .AddWithValue("@id_user", id_user)
            .AddWithValue("@id_session", id_session)
            .AddWithValue("@username", m_user.UserName)
            .AddWithValue("@kundennr", kundennr)
            .AddWithValue("@kundenname", kundenname)
            .AddWithValue("@haltername", haltername)
            .AddWithValue("@fahrgestellnr", fahrgestellnr)
            .AddWithValue("@stva", stva)
            .AddWithValue("@stvanr", stvanr)
            .AddWithValue("@wunschkennzflag", wunschkennzflag)
            .AddWithValue("@wunschkennz", wunschennz)
            .AddWithValue("@wunschkennzABC", wunschennzABC)
            .AddWithValue("@wunschkennzNR", wunschennzNR)
            .AddWithValue("@str_wunschkennz", str_wunschkennz)
            .AddWithValue("@dienstleistungnr", dienstleistungnr)
            .AddWithValue("@dienstleistung", dienstleistung)
            .AddWithValue("@zulassungsdatum", zulassungsdatum.ToShortDateString)
            .AddWithValue("@preis_stva", preis_stva)
            .AddWithValue("@preis_zulassung", preis_zulassung)
            .AddWithValue("@preis_kennz", preis_kennz)
            .AddWithValue("@preis_pauschal", preis_pauschal)
            .AddWithValue("@saved", saved)
            .AddWithValue("@check2", check2)
            .AddWithValue("@check3", check3)
            .AddWithValue("@free1", free1)
            .AddWithValue("@free2", free2)
            .AddWithValue("@free3", m_user.Reference)    'User-Daten!
            .AddWithValue("@toSave", tosave)
            .AddWithValue("@toDelete", toDelete)
            .AddWithValue("@kennztyp", kztyp)
            .AddWithValue("@einkz", einkz)
            .AddWithValue("@reservid", reservid)
            .AddWithValue("@reserv", reserv)
            .AddWithValue("@kundennr_alt", kundennr_alt)
            .AddWithValue("@tour", tour)
            .AddWithValue("@fremdbestand", fremdbest)
            .AddWithValue("@barkunde", bar)
            .AddWithValue("@sonst_dienst", sonst_dienst)
            .AddWithValue("@preis_sonstdienst", preis_sonstdienst)
            .AddWithValue("@name1", name1)
            .AddWithValue("@strasse", strasse)
            .AddWithValue("@plz", plz)
            .AddWithValue("@ort", ort)
            .AddWithValue("@preis_kasse", preiskasse)
            .AddWithValue("@Vkkurz", sVerkkurz)
            .AddWithValue("@KIReferenz", sKIReferenz)
            .AddWithValue("@Notiz", sNotiz)
            If Not PAUART Is Nothing Then
                .AddWithValue("@Auart", PAUART)
            Else : .AddWithValue("@Auart", "")
            End If
            If Not PZUEFUE Is Nothing Then
                .AddWithValue("@VerbVbeln", PZUEFUE)
            Else : .AddWithValue("@VerbVbeln", "")
            End If
            .AddWithValue("@Feinstaub", Feinstaub)
            .AddWithValue("@krad", Krad)
        End With

        returnID = CInt(command.ExecuteScalar).ToString
        If (type = "UPDATE") Then
            returnID = id_recordset.ToString
        End If
        
        If (type = "INSERT") Then
            command2.CommandText = "SELECT PValue FROM Parameters WHERE  (PName = 'HoechsteZulassungsID')"
            If id_sap > CType(command2.ExecuteScalar, System.Int32) Then
                command2.CommandText = "UPDATE Parameters SET PValue = " & id_sap.ToString & " WHERE  (PName = 'HoechsteZulassungsID')"
                command2.ExecuteNonQuery()
            End If
        End If
    End Sub

    Public Function GiveNewZulassungsID(ByRef returnStatus As String) As Int32
        returnStatus = ""
        Try
            openConnection()
            'Save Data
            Return DBGiveNewZulassungsID()
        Catch ex As Exception
            returnStatus = ex.Message
        Finally
            closeConnection()
        End Try

    End Function

    Private Function DBGiveNewZulassungsID() As Int32
        Dim command As New SqlClient.SqlCommand()
        With command
            .Connection = connection
            .CommandType = CommandType.Text
        End With
        command.CommandText = "SELECT PValue FROM Parameters WHERE  (PName = 'HoechsteZulassungsID')"
        Return CType(command.ExecuteScalar, System.Int32) + 1
    End Function

    Public Sub recordset_Save(ByRef returnStatus As String, ByRef returnID As String)
        returnStatus = ""
        Try
            openConnection()
            'Save Data
            dbWrite("INSERT", returnID)
        Catch ex As Exception
            returnStatus = ex.Message
        Finally
            closeConnection()
        End Try
    End Sub

    Public Sub recordset_Update(ByRef returnStatus As String, ByRef returnID As String)
        returnStatus = ""
        Try
            openConnection()
            'Update Data
            dbWrite("UPDATE", returnID)
        Catch ex As Exception
            returnStatus = ex.Message
        Finally
            closeConnection()
        End Try
    End Sub

    Public Function getSQLDaten(ByRef status As String, ByVal stva As String, ByVal kunnr As String, ByVal qryID As String, ByVal zulDatum As String,
                                ByVal endabgerechnet As Boolean) As DataTable
        'Für Nacherfassung
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim result As New DataTable()
        Dim str As String

        str = "SELECT * FROM vwZulassung WHERE free3 = @org"
        str &= " AND testuser = @testuser"
        'str &= " AND zulassungsdatum = @zd"
        str &= " AND UserID = @UserID"

        If endabgerechnet = True Then
            str &= " AND toSave = 1"
        End If

        '§§§ JVE 14.06.2006: Sortierung eingefügt
        'str &= " ORDER BY kundenname asc, haltername asc, str_wunschkennz"
        '-----------------------------------------------------------------

        Try
            'command.Parameters.Add("@stvanr", stva)
            command.Parameters.AddWithValue("@org", m_user.Reference)
            'command.Parameters.AddWithValue("@zd", zulDatum)
            command.Parameters.AddWithValue("@UserID", m_user.UserID)

            'If blnVorerfassung Then                         'Nur bei der Vorerfassung nach eigenen Datensätzen filtern!
            '    str &= " AND id_User = @id_User"
            '    command.Parameters.AddWithValue("@id_User", m_user.UserID)
            'End If

            command.Parameters.AddWithValue("@testuser", m_user.IsTestUser)

            If Not zulDatum = String.Empty Then
                str &= " AND zulassungsdatum = @zd"
                command.Parameters.AddWithValue("@zd", zulDatum)
            End If

            If Not stva = String.Empty Then
                str &= " AND stvanr = @stvanr"
                command.Parameters.AddWithValue("@stvanr", stva)
            End If

            If Not (kunnr = String.Empty) Then
                str &= " AND kundennr = @kundennr"
                '§§§ JVE 31.07.2006: Kundennr mit führenden Nullen!
                command.Parameters.AddWithValue("@kundennr", Right("0000000000" & kunnr, 10))
            End If
            If Not (qryID = String.Empty) Then
                str &= " AND id_sap = @id_sap"
                command.Parameters.AddWithValue("@id_sap", qryID)
            End If
            '§§§ JVE 31.07.2006: Sortierung eingefügt...
            str &= " ORDER BY kundenname asc, haltername asc, str_wunschkennz"

            command.CommandText = str

            openConnection()
            command.Connection = connection
            command.CommandType = CommandType.Text
            adapter.SelectCommand = command
            adapter.Fill(result)
        Catch ex As Exception
            status = ex.Message
        Finally
            closeConnection()
        End Try
        Return result
    End Function

    Public Function getSQLDaten(ByRef status As String, ByVal user As Int32, ByVal dat As String, ByVal endabgerechnet As Boolean) As DataTable
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim result As New DataTable()
        Dim strTemp As String

        strTemp = "SELECT * FROM Zulassung WHERE id_User = @id_User"
        strTemp &= " AND vorerfassung = @vorerfassung"
        strTemp &= " AND testuser = @testuser"

        If endabgerechnet = True Then
            strTemp &= " AND toSave = 1"
        End If

        Try
            If Not ((dat = String.Empty) Or dat = "ALLE") Then
                command.CommandText = strTemp & " AND (convert(varchar,tstamp,104) = '" & dat & "') ORDER BY kundenname asc, haltername asc, str_wunschkennz"
            Else
                If (dat = String.Empty) Then
                    dat = Now.ToShortDateString
                    command.CommandText = strTemp & " AND (convert(varchar,tstamp,104) = '" & dat & "') ORDER BY kundenname asc, haltername asc, str_wunschkennz"
                End If

                If (dat = "ALLE") Then
                    command.CommandText = strTemp & " ORDER BY kundenname asc, haltername asc, str_wunschkennz"
                End If
            End If

            command.Parameters.AddWithValue("@id_User", user)
            command.Parameters.AddWithValue("@vorerfassung", blnVorerfassung)
            command.Parameters.AddWithValue("@testuser", m_user.IsTestUser)

            openConnection()
            command.Connection = connection
            command.CommandType = CommandType.Text
            adapter.SelectCommand = command
            adapter.Fill(result)
        Catch ex As Exception
            status = ex.Message
        Finally
            closeConnection()
        End Try
        Return result
    End Function

    Public Function loadRecordset(ByVal id As Int32, ByRef status As String) As DataTable
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim result As New DataTable()

        Try
            command.CommandText = "SELECT * FROM Zulassung WHERE id_sap = @id AND vorerfassung = @vorerfassung AND testuser = @testuser"

            command.Parameters.AddWithValue("@id", id)
            command.Parameters.AddWithValue("@vorerfassung", blnVorerfassung)
            command.Parameters.AddWithValue("@testuser", m_user.IsTestUser)
            openConnection()
            command.Connection = connection
            command.CommandType = CommandType.Text
            adapter.SelectCommand = command
            adapter.Fill(result)
        Catch ex As Exception
            status = ex.Message
        Finally
            closeConnection()
        End Try
        Return result
    End Function

    Private Sub insertSQLAddParams(ByRef command As SqlClient.SqlCommand)
        With command.Parameters
            .AddWithValue("@testuser", System.DBNull.Value)
            .AddWithValue("@vorerfassung", System.DBNull.Value)
            .AddWithValue("@barkunde", System.DBNull.Value)
            .AddWithValue("@toSave", System.DBNull.Value)
            .AddWithValue("@id_user", System.DBNull.Value)
            .AddWithValue("@kundennr", System.DBNull.Value)
            .AddWithValue("@haltername", System.DBNull.Value)
            .AddWithValue("@internebemerkung", System.DBNull.Value)
            .AddWithValue("@fahrgestellnr", System.DBNull.Value)
            .AddWithValue("@stvanr", System.DBNull.Value)
            .AddWithValue("@str_wunschkennz", System.DBNull.Value)
            .AddWithValue("@preis_stva", System.DBNull.Value)
            .AddWithValue("@preis_zulassung", System.DBNull.Value)
            .AddWithValue("@preis_kennz", System.DBNull.Value)
            .AddWithValue("@dienstleistungnr", System.DBNull.Value)
            .AddWithValue("@zulassungsdatum", System.DBNull.Value)
            .AddWithValue("@id_sap", System.DBNull.Value)
            .AddWithValue("@wunschkennzflag", System.DBNull.Value)
            .AddWithValue("@kennztyp", System.DBNull.Value)
            .AddWithValue("@einkz", System.DBNull.Value)
            .AddWithValue("@reservid", System.DBNull.Value)
            .AddWithValue("@reserv", System.DBNull.Value)
            .AddWithValue("@kundennr_alt", System.DBNull.Value)
            .AddWithValue("@tour", System.DBNull.Value)
            .AddWithValue("@name1", System.DBNull.Value)
            .AddWithValue("@strasse", System.DBNull.Value)
            .AddWithValue("@plz", System.DBNull.Value)
            .AddWithValue("@ort", System.DBNull.Value)
            .AddWithValue("@preis_kasse", System.DBNull.Value)
            .AddWithValue("@free1", System.DBNull.Value)
            .AddWithValue("@free2", System.DBNull.Value)
            .AddWithValue("@free3", System.DBNull.Value)
        End With
    End Sub

    Private Function insertSQL(ByVal t As DataTable, ByRef status As String) As Boolean
        'Einfügen der aus SAP hochgeladenen Daten zur Nacherfassung....
        Dim command As New SqlClient.SqlCommand()
        Dim sqlInsert As String
        Dim row As DataRow
        Dim inserted As Boolean = False

        sqlInsert = "INSERT INTO Zulassung (testuser,vorerfassung,barkunde,toSave,id_user,kundennr,internebemerkung,haltername,fahrgestellnr,stvanr,str_wunschkennz,dienstleistungnr,zulassungsdatum,id_sap,preis_stva,preis_kennz,preis_zulassung,kennztyp,einkz,reservid,reserv,kundennr_alt,tour,wunschkennzflag,name1,strasse,plz,ort,preis_kasse,free1,free2,free3)" _
                & " VALUES (@testuser,@vorerfassung,@barkunde,@toSave,@id_user,@kundennr,@internebemerkung,@haltername,@fahrgestellnr,@stvanr,@str_wunschkennz,@dienstleistungnr,@zulassungsdatum,@id_sap,@preis_stva,@preis_kennz,@preis_zulassung,@kennztyp,@einkz,@reservid,@reserv,@kundennr_alt,@tour,@wunschkennzflag,@name1,@strasse,@plz,@ort,@preis_kasse,@free1,@free2,@free3)"

        With command
            .Connection = connection
            .CommandType = CommandType.Text
            .Parameters.Clear()
        End With
        command.CommandText = sqlInsert
        insertSQLAddParams(command)
        For Each row In t.Rows
            Try
                With command
                    .Parameters("@testuser").Value = m_user.IsTestUser
                    .Parameters("@vorerfassung").Value = blnVorerfassung
                    .Parameters("@internebemerkung").Value = row("ZZTEXT").ToString
                    .Parameters("@toSave").Value = True
                    .Parameters("@id_user").Value = row("USERID").ToString
                    .Parameters("@kundennr").Value = Right("0000000000" & CType(row("KUNNR"), Int32), 10)
                    .Parameters("@haltername").Value = row("ZZHALTER").ToString
                    .Parameters("@fahrgestellnr").Value = row("ZZFAHRG").ToString
                    .Parameters("@stvanr").Value = row("KREIS").ToString
                    .Parameters("@str_wunschkennz").Value = row("ZZKENN").ToString
                    .Parameters("@dienstleistungnr").Value = row("MATNR").ToString
                    .Parameters("@zulassungsdatum").Value = row("ZZZLDAT").ToString
                    .Parameters("@id_sap").Value = CType(row("ID"), Int32)
                    .Parameters("@preis_stva").Value = CType(row("GEBAUSL"), Decimal)
                    .Parameters("@preis_kennz").Value = CType(row("PREISKZ"), Decimal)
                    .Parameters("@preis_zulassung").Value = CType(row("PREISDL"), Decimal)
                    .Parameters("@preis_kasse").Value = CType(row("ZZKASSENGBA"), Decimal)
                    .Parameters("@kennztyp").Value = CType(row("KENNZTYP"), String)
                    If row("EINKZ").ToString = "X" Then
                        .Parameters("@einkz").Value = 1
                    Else
                        .Parameters("@einkz").Value = 0
                    End If
                    .Parameters("@reservid").Value = CType(row("RESERVID"), String)
                    If row("RESERVIERT").ToString = "X" Then
                        .Parameters("@reserv").Value = 1
                    Else
                        .Parameters("@reserv").Value = 0
                    End If
                    If row("KALKS").ToString = "X" Then
                        .Parameters("@barkunde").Value = 1
                    Else
                        .Parameters("@barkunde").Value = 0
                    End If
                    If row("ZZWUNSCH").ToString = "X" Then
                        .Parameters("@wunschkennzflag").Value = 1
                    Else
                        .Parameters("@wunschkennzflag").Value = 0
                    End If
                    .Parameters("@kundennr_alt").Value = CType(row("ALTKN"), String)
                    .Parameters("@tour").Value = CType(row("KVGR4"), String)

                    .Parameters("@name1").Value = CType(row("NAME1"), String)
                    .Parameters("@strasse").Value = CType(row("STRAS"), String)
                    .Parameters("@plz").Value = CType(row("PSTLZ"), String)
                    .Parameters("@ort").Value = CType(row("ORT01"), String)
                    .Parameters("@free1").Value = CType(Now.ToShortDateString, Date)

                    'Nicht Pauschalkunde:
                    .Parameters("@free2").Value = CType(row("ZZSPERR"), String)
                    .Parameters("@free3").Value = CType(row("VKORG"), String) & CType(row("VKBUR"), String)
                End With
                command.ExecuteNonQuery()
                status = String.Empty
                inserted = True
            Catch ex As Exception
                status = ex.Message
            End Try
        Next
        Return inserted
    End Function

    Public Sub DeleteRecordVE(ByVal strRecordID As String)
        Dim command As New SqlClient.SqlCommand()
        Dim sqlUpdate As String

        Try
            openConnection()
            sqlUpdate = "DELETE FROM Zulassung WHERE id=" & strRecordID
            With command
                .Connection = connection
                .CommandType = CommandType.Text
            End With
            command.CommandText = sqlUpdate
            command.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            closeConnection()
        End Try
    End Sub

    Public Sub DeleteRecordNE(ByVal strRecordID As String, ByVal strtoDelete As String)
        Dim command As New SqlClient.SqlCommand()
        Dim sqlUpdate As String
        Dim strHelp As String = "'L', preis_stva=0, preis_kasse=0"

        Try
            openConnection()

            If strtoDelete = "L" Then
                strHelp = "''"
            End If

            sqlUpdate = "UPDATE Zulassung SET toDelete=" & strHelp & " WHERE id=" & strRecordID
            With command
                .Connection = connection
                .CommandType = CommandType.Text
            End With
            command.CommandText = sqlUpdate
            command.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            closeConnection()
        End Try
    End Sub

    Private Sub updatesql()
        'Username hinzufügen
        Dim command As New SqlClient.SqlCommand()
        Dim sqlUpdate As String

        sqlUpdate = "UPDATE Zulassung SET zulassung.username = webuser.username FROM webuser INNER JOIN zulassung ON zulassung.id_user = webuser.userid AND zulassung.username IS NULL"
        With command
            .Connection = connection
            .CommandType = CommandType.Text
        End With
        command.CommandText = sqlUpdate
        command.ExecuteNonQuery()
    End Sub

    Private Function getID(ByVal command As SqlClient.SqlCommand, ByVal id As Int32) As Object
        Dim sqlGet As String
        Dim i As Int32

        sqlGet = "SELECT count(id_sap) FROM Zulassung WHERE id_sap = " & id
        command.Connection = connection
        command.CommandType = CommandType.Text
        command.CommandText = sqlGet
        i = CType(command.ExecuteScalar(), Int32)
        Return i
    End Function

    Private Sub clean(ByRef t As DataTable)
        'Public Shared Function Scalar(ByVal queryString As String) As Object
        '#Starts a query without a single scalar as result (i.e. SELECT count(*)...
        Dim command As New SqlClient.SqlCommand()
        Dim row As DataRow
        Dim rowDel As DataRow()
        Dim i As Int32

        Dim tmp As DataTable = t.Copy

        For Each row In t.Rows
            i = CType(getID(command, CType(row("id"), Int32)), Int32)
            If i > 0 Then
                rowDel = tmp.Select("id=" & CType(row("id"), String))
                tmp.Rows.Remove(rowDel(0))
                'SAP_ID bereits vorhanden...
            End If
        Next
        t = tmp.Copy
    End Sub

    Public Sub updateKunde(ByVal vkorg As String, ByVal vkbur As String, ByVal tblWeb As DataTable)
        'Aktualisiert Kundenname und Dienstleistungsname in der Darstellung
        Dim status As String
        Dim row As DataRow
        Dim rowWeb() As DataRow
        Dim strKunde As String

        status = String.Empty

        'getSAPDaten(vkorg, vkbur, tblKunde, tblStva, tblMat, status)
        For Each row In tblWeb.Rows
            rowWeb = Kundentabelle.Select("Kunnr=" & row("Kundennr").ToString)
            If rowWeb.Length > 0 Then
                strKunde = rowWeb(0)("Name1").ToString
                If (strKunde.IndexOf("~") >= 0) Then
                    row("Kundenname") = Left(strKunde, strKunde.IndexOf("~"))
                End If
                rowWeb = Materialtabelle.Select("Matnr='" & row("dienstleistungnr").ToString & "'")
                '§§§JVE 10.11.2005  If-Abfrage eingefügt, sonst Fehler wenn Dienstleistung nicht gefunden wird!
                If rowWeb.Length > 0 Then
                    row("dienstleistung") = rowWeb(0)("Maktx").ToString
                Else
                    '§§§JVE 10.11.2005  Wenn nicht gefunden
                    row("dienstleistung") = "???"
                End If
            Else
                '§§§JVE 10.11.2005  Wenn nicht gefunden
                row("Kundenname") = "???"
            End If
        Next
    End Sub

    Public Function getSAPDatenVorerfassung(ByVal tblKunden As DataTable, ByVal tblMaterial As DataTable, ByRef status As String, ByVal vkorg As String,
                                            ByVal vkbur As String, ByVal stva As String, ByVal lngID As String, ByVal zuldatum As String,
                                            Optional ByVal kunnr As String = "") As Integer
        'Daten zur Nacherfassung aus SAP lesen
        'Return 0: OK
        'Return 1: Keine Daten zur Nacherfassung in SAP gefunden
        'Return 2: Keine Daten in SQL eingefügt (da bereits alle vorhanden)

        Dim belege As DataTable
        Dim abr As DataTable
        Dim inserted As Boolean
        Dim i As Int32
        Dim returnvalue As Integer
        Dim orgdaten As String
        Dim m_objLogApp As Base.Kernel.Logging.Trace = New Base.Kernel.Logging.Trace(m_user.App.Connectionstring, True, m_user.App.LogLevel)
        Dim intID As Long

        status = String.Empty
        returnvalue = 0

        Try
            'intID = m_objLogApp.WriteStartDataAccessSAP(m_user.UserName, m_user.IsTestUser, "Z_F_Ck_Belegabrechnen", "", "", m_user.CurrentLogAccessASPXID)

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_F_Ck_Belegabrechnen", m_app, m_user, page)

            'belege = myProxy.getImportTable("IT_BELEGABR")

            belege = S.AP.GetImportTableWithInit("Z_F_Ck_Belegabrechnen.IT_BELEGABR")

            Dim row As DataRow = belege.NewRow()

            row("VKORG") = vkorg
            row("VKBUR") = vkbur
            row("Kreis") = stva

            If (lngID <> String.Empty) Then
                row("ID") = CType(lngID, Long)
            End If

            If kunnr = String.Empty Then
                row("KUNNR") = String.Empty
            Else
                row("KUNNR") = Right("0000000000" & kunnr, 10)
            End If

            row("ZZZLDAT") = zuldatum

            belege.Rows.Add(row)
            belege.AcceptChanges()

            'myProxy.callBapi()
            S.AP.Execute()

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            abr = S.AP.GetExportTable("GT_BELEGABR")

            openConnection()

            If abr.Rows.Count = 0 Then
                returnvalue = 1
            Else

                For i = abr.Rows.Count - 1 To 0 Step -1
                    If Not abr.Rows(i)("ZZLOESCH").ToString.Length = 0 Then
                        abr.Rows.RemoveAt(i)

                        orgdaten = CType(abr.Rows(i)("VKORG"), String) & CType(abr.Rows(i)("VKBUR"), String)
                        If (orgdaten <> m_user.Reference) Then
                            abr.Rows.RemoveAt(i)
                        End If
                    End If
                Next

                clean(abr) 'Alle Zeilen, die bereits in der SQL-DB drin sind, löschen...
                inserted = insertSQL(abr, status)
                updatesql()
                If Not inserted Then
                    returnvalue = 2
                End If
            End If

        Catch ex As Exception
            status = ex.Message
            'If intID > -1 Then
            '    m_objLogApp.WriteEndDataAccessSAP(intID, False, status)
            'End If
        Finally
            closeConnection()
        End Try

        Return returnvalue
    End Function

    ' ''' <summary>
    ' ''' Erstellen der Tabellestruktur für Biztalk
    ' ''' </summary>
    ' ''' <remarks>
    ' ''' Autor: O.Rudolph
    ' ''' Erstellt am: 19.01.2009
    ' ''' ITA: 2513
    ' ''' </remarks>
    'Public Sub MakeSapDatenTable()
    '    tblKunde = New DataTable
    '    tblKunde.TableName = "GT_KUNNR"
    '    tblKunde.Columns.Add("VKORG", System.Type.GetType("System.String"))
    '    tblKunde.Columns.Add("VKBUR", System.Type.GetType("System.String"))
    '    tblKunde.Columns.Add("KUNNR", System.Type.GetType("System.String"))
    '    tblKunde.Columns.Add("NAME1", System.Type.GetType("System.String"))
    '    tblKunde.Columns.Add("KVGR4", System.Type.GetType("System.String"))
    '    tblKunde.Columns.Add("ALTKN", System.Type.GetType("System.String"))
    '    tblKunde.Columns.Add("KALKS", System.Type.GetType("System.String"))
    '    tblKunde.Columns.Add("DATLT", System.Type.GetType("System.String"))
    '    tblKunde.Columns.Add("KATR1", System.Type.GetType("System.String"))
    '    tblKunde.Columns.Add("ZULUPLFICHT", System.Type.GetType("System.String"))

    '    tblMat = New DataTable
    '    tblMat.TableName = "GT_MATNR"
    '    tblMat.Columns.Add("VKORG", System.Type.GetType("System.String"))
    '    tblMat.Columns.Add("VKBUR", System.Type.GetType("System.String"))
    '    tblMat.Columns.Add("MATNR", System.Type.GetType("System.String"))
    '    tblMat.Columns.Add("MAKTX", System.Type.GetType("System.String"))
    '    tblMat.Columns.Add("DOKZUORD", System.Type.GetType("System.String"))

    '    tblMat = New DataTable
    '    tblMat.TableName = "GT_MATNR"
    '    tblMat.Columns.Add("KREISKZ", System.Type.GetType("System.String"))
    '    tblMat.Columns.Add("KREISBEZ", System.Type.GetType("System.String"))
    '    tblMat.Columns.Add("URL", System.Type.GetType("System.String"))
    'End Sub


    Public Sub getSAPDatenBiz(ByVal vkorg As String, ByVal vkbur As String, ByRef status As String)

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_F_CK_GRUPPENDATEN", m_app, m_user, page)
            S.AP.Init("Z_F_CK_GRUPPENDATEN")

            S.AP.SetImportParameter("I_VKORG", vkorg)
            S.AP.SetImportParameter("I_VKBUR", vkbur)
            S.AP.SetImportParameter("I_GRUPPE", m_user.Groups(0).GroupName)

            'myProxy.callBapi()
            S.AP.Execute()

            tblKunde = S.AP.GetExportTable("GT_KUNNR")
            tblMat = S.AP.GetExportTable("GT_MATNR")
            tblStva = S.AP.GetExportTable("GT_KREISKZ")

            If tblKunde.Rows.Count = 0 Then
                status = "FEHLER: Keine Kundendaten gefunden!"
            End If

            Dim dr As DataRow
            dr = tblKunde.NewRow
            dr("KUNNR") = "0000000000"
            dr("NAME1") = " - keine Auswahl - "
            tblKunde.Rows.Add(dr)

            'Kundenliste füllen
            For Each dr In tblKunde.Rows
                dr("NAME1") = dr("NAME1").ToString & " ~ " & Right("000000" & dr("KUNNR").ToString, 8)

                If (dr("ALTKN").ToString <> String.Empty) Then
                    dr("NAME1") = dr("NAME1").ToString & " ~ " & Right("000000" & dr("ALTKN").ToString, 8)
                End If

                If dr("DATLT").ToString <> String.Empty Then
                    dr("NAME1") = dr("NAME1").ToString & " / " & dr("DATLT").ToString
                End If
            Next
            tblKunde.AcceptChanges()

            'StVA füllen            
            For Each dr In tblStva.Rows
                dr("KREISBEZ") = Left(dr("KREISKZ").ToString & "....", 4) & dr("KREISBEZ").ToString
            Next
            tblStva.AcceptChanges()


            If tblStva.Rows.Count = 0 Then
                status = "FEHLER: Keine STVA-Daten gefunden!"
            End If

            dr = tblStva.NewRow
            dr("KREISBEZ") = " - keine Auswahl - "
            dr("KREISKZ") = ""
            tblStva.Rows.Add(dr)

            If tblMat.Rows.Count = 0 Then
                status = "FEHLER: Keine Materialdaten gefunden!"
            End If

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    selectedUser = "X"
                Case "NO_PARAMETER"
                    Message = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    Message = "Falsche Kundennr."
                Case "Message"
                    Message = ""
                Case Else
                    Message = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try
    End Sub

    Public Sub getSAPBarcodedaten(ByVal strVbeln As String, ByRef status As String)
        Dim stcReturn As DataTable

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_F_Ck_Vbeln_Daten", m_app, m_user, page)

            'myProxy.setImportParameter("I_VBELN", strVbeln)

            'myProxy.callBapi()

            'stcReturn = myProxy.getExportTable("GS_WEB")

            stcReturn = S.AP.GetExportTableWithInitExecute("Z_F_Ck_Vbeln_Daten.GS_WEB", "I_VBELN", strVbeln)

            If stcReturn.Rows.Count > 0 Then
                Dim row As DataRow = stcReturn.Rows(0)

                strBarcodeKundennr = row("KUNNR")
                strBarcodeReferenz2 = row("EBELN")
                strBarcodeDienstleistung = row("MATNR")
                strBarcodeStva = row("ZZKENN")
                strAUART = row("AUART")

                If strAUART = "ZÜFÜ" Then
                    strZUFUnr = strVbeln
                    strBarcodeReferenz2 = row("ZZREFNR")
                End If
                status = String.Empty

            End If

        Catch ex As Exception
            status = ex.Message
        End Try
    End Sub

    'Public Shared Function MakeDateSAP(ByVal datInput As String) As String
    '    REM $ Formt Date-Input in String YYYYMMDD um
    '    Dim dat As Date
    '    Dim returnStr As String

    '    returnStr = String.Empty

    '    If (datInput Is Nothing) Or (datInput = String.Empty) Then

    '    Else
    '        dat = CType(datInput, Date)
    '        returnStr = Year(dat) & Right("0" & Month(dat), 2) & Right("0" & Day(dat), 2)
    '    End If
    '    Return returnStr
    'End Function

    'Public Shared Function MakeDateStandart(ByVal datInput As String) As String
    '    REM $ Formt Date-Input in String YYYYMMDD um
    '    Dim returnStr As String = ""
    '    Dim strTemp As String = Nothing
    '    If (strTemp Is Nothing) Or (strTemp <> String.Empty) Then
    '        If Not Len(datInput) = 8 Then
    '            Return ""
    '        End If
    '        strTemp = Right(datInput, 2) & "." & Mid(datInput, 5, 2) & "." & Left(datInput, 4)
    '        If IsDate(strTemp) Then
    '            returnStr = strTemp
    '        End If
    '        Return returnStr
    '    Else
    '        Return String.Empty
    '    End If

    'End Function

    Public Shared Function toShortDateStr(ByVal dat As String) As String
        Dim datum As Date
        Try
            datum = CType(Left(dat, 2) & "." & dat.Substring(2, 2) & "." & Left(Year(Today).ToString, 2) & Right(dat, 2), Date)
        Catch ex As Exception
            Return String.Empty
        End Try
        Return datum.ToShortDateString
    End Function

    Public Sub clear()
        kundennr = String.Empty
        kundenname = String.Empty
        haltername = String.Empty
        internebemerkung = String.Empty
        stva = String.Empty
        stvanr = String.Empty
        wunschkennzflag = False
        wunschennz = String.Empty
        wunschennzABC = String.Empty
        wunschennzNR = String.Empty
        kennztyp = String.Empty
        str_wunschkennz = String.Empty
        dienstleistungnr = String.Empty
        dienstleistung = String.Empty
        preis_stva = 0
        preis_zulassung = 0
        preis_kennz = 0
        preis_pauschal = 0
        toDelete = String.Empty
        tosave = False
        saved = False
        check2 = False
        check3 = False
        free1 = String.Empty
        free2 = String.Empty
        free3 = String.Empty
        kundennralt = String.Empty
        tournr = String.Empty
        reserviert = False
        reserviertid = String.Empty
        einkennz = False
        fremdbestand = False
        barkunde = False
        sonstDienst = String.Empty
        preisSonstDienst = 0
        fahrgestellnr = String.Empty
        name1 = String.Empty
        strasse = String.Empty
        plz = String.Empty
        ort = String.Empty
        Feinstaub = 0
        Krad = 0
    End Sub

    'Public Overloads Sub FillData(ByRef page As Page)
    '    Dim strKunnr As String = ""
    '    Dim sDokZu As String = ""
    '    strKunnr = Right("0000000000" & m_user.KUNNR, 10)

    '    'Dim intID As Int32 = -1

    '    Try

    '        Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_KCL_READ_AUFTR_002", m_app, m_user, page)

    '        myProxy.setImportParameter("I_KUNNR_MASTER", strKunnr)

    '        myProxy.callBapi()

    '        tblUpload = myProxy.getExportTable("GT_AUFTRAG")

    '        With tblUpload
    '            .Columns.Add("Problem", System.Type.GetType("System.String"))
    '            .Columns.Add("Dienstleistung", System.Type.GetType("System.String"))
    '            .Columns.Add("Kunde", System.Type.GetType("System.String"))
    '            .Columns.Add("Url", System.Type.GetType("System.String"))
    '            .Columns.Add("Ok", System.Type.GetType("System.Boolean"))
    '            .Columns.Add("Del", System.Type.GetType("System.Boolean"))
    '            .Columns.Add("NoSel", System.Type.GetType("System.Boolean"))
    '            .Columns.Add("id_sap", System.Type.GetType("System.Int32"))
    '            .Columns.Add("VBELN", System.Type.GetType("System.String"))
    '            .Columns.Add("ZULUPLFICHT", System.Type.GetType("System.String"))
    '        End With

    '        Dim datarow As DataRow
    '        'Aufräge
    '        For Each datarow In tblUpload.Rows
    '            datarow("Problem") = "O"
    '            datarow("Ok") = False
    '            datarow("Del") = False
    '            datarow("NoSel") = True

    '            If datarow("Kreis").ToString = String.Empty Then
    '                datarow("Problem") = "X"
    '            End If

    '            If datarow("KUNNR_AG").ToString = String.Empty Then
    '                datarow("Problem") = "X"
    '            Else
    '                Dim KundeRow As DataRow
    '                KundeRow = tblKunde.Select("KUNNR='" & datarow("KUNNR_AG").ToString & "'")(0)
    '                datarow("KUNNR_AG") = datarow("KUNNR_AG").ToString.TrimStart("0"c)
    '                datarow("Kunde") = KundeRow("NAME1").ToString
    '                datarow("ZULUPLFICHT") = KundeRow("ZULUPLFICHT").ToString
    '            End If

    '            If datarow("MATNR").ToString = String.Empty Then
    '                datarow("Problem") = "X"
    '            Else
    '                Dim MatRow As DataRow
    '                MatRow = tblMat.Select("MATNR='" & datarow("MATNR").ToString & "'")(0)
    '                datarow("Dienstleistung") = MatRow("MAKTX").ToString
    '                sDokZu = MatRow("DOKZUORD").ToString
    '            End If

    '            If datarow("ZZZLDAT").ToString = String.Empty Or datarow("ZZZLDAT").ToString = "00000000" Then
    '                datarow("Problem") = "X"
    '                datarow("ZZZLDAT") = ""
    '            Else
    '                datarow("ZZZLDAT") = MakeDateStandart(datarow("ZZZLDAT").ToString)
    '            End If

    '            Dim sArray() As String = Split(datarow("ZZKenn").ToString, "-")
    '            Dim sKenn As String = ""
    '            Dim sABC As String = ""

    '            If sArray.Length = 2 Then
    '                sKenn = sArray(0)
    '                sABC = sArray(1)
    '            End If

    '            If sKenn = "" Then
    '                datarow("Problem") = "X"
    '            End If

    '            If datarow("ZULUPLFICHT").ToString = "P" And sDokZu <> "" Then
    '                If datarow("ZB1").ToString = "" Then
    '                    datarow("Problem") = "X"
    '                End If
    '                If datarow("ZB2").ToString = "" Then
    '                    datarow("Problem") = "X"
    '                End If
    '                If datarow("COC").ToString = "" Then
    '                    datarow("Problem") = "X"
    '                End If
    '                If datarow("ZDK").ToString = "" Then
    '                    datarow("Problem") = "X"
    '                End If
    '                If datarow("VM").ToString = "" Then
    '                    datarow("Problem") = "X"
    '                End If
    '                If datarow("PA").ToString = "" Then
    '                    datarow("Problem") = "X"
    '                End If
    '                If datarow("GEWA").ToString = "" Then
    '                    datarow("Problem") = "X"
    '                End If
    '                If datarow("HRA").ToString = "" Then
    '                    datarow("Problem") = "X"
    '                End If
    '                If datarow("LEV").ToString = "" Then
    '                    datarow("Problem") = "X"
    '                End If
    '            End If
    '        Next

    '        tblUpload.AcceptChanges()
    '        selectedUser = "X"
    '        HelpProcedures.killAllDBNullValuesInDataTable(tblUpload)
    '    Catch ex As Exception
    '        Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
    '            Case "NO_DATA"
    '                selectedUser = "X"
    '            Case "NO_PARAMETER"
    '                Message = "Eingabedaten nicht ausreichend."
    '            Case "NO_ASL"
    '                Message = "Falsche Kundennr."
    '            Case "Message"
    '                Message = ""
    '            Case Else
    '                Message = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
    '        End Select
    '    End Try
    'End Sub

    'Public Function UpdateUploadTable(ByVal Table As DataTable, ByVal Status As String, ByRef page As Page) As String
    '    Dim strTemp As String = ""
    '    Dim strKunnr As String = ""
    '    strKunnr = Right("0000000000" & m_user.KUNNR, 10)

    '    Try

    '        Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_KCL_CHANGE_AUFTR_002", m_app, m_user, page)

    '        Dim ImpTbl As DataTable = myProxy.getImportTable("GT_AUFTRAG")

    '        For Each row As DataRow In Table.Rows
    '            Dim newRow As DataRow = ImpTbl.NewRow()

    '            For Each col As DataColumn In ImpTbl.Columns
    '                newRow(col.ColumnName) = row(col.ColumnName)
    '            Next

    '            newRow("I_AKTION") = "B"
    '            newRow("STATUS") = Status
    '            newRow("E_FEHLER") = ""
    '            newRow("ZZZLDAT") = MakeDateSAP(newRow("ZZZLDAT").ToString)
    '            newRow("AEDAT") = Now
    '            newRow("AENAM") = m_user.UserName
    '            newRow("AEDAT") = MakeDateSAP(Now.ToString)
    '            newRow("Auf_ID") = Right("0000000000" & newRow("Auf_ID").ToString, 10)
    '            newRow("Kunnr_AG") = Right("0000000000" & newRow("Kunnr_AG").ToString, 10)

    '            ImpTbl.Rows.Add(newRow)
    '        Next

    '        ImpTbl.AcceptChanges()

    '        myProxy.callBapi()

    '        Dim ExpTbl As DataTable = myProxy.getExportTable("GT_AUFTRAG")

    '        strTemp = ExpTbl.Rows(0)("E_FEHLER").ToString()

    '        Return strTemp
    '    Catch ex As Exception
    '        strTemp = ex.Message
    '        Return strTemp
    '    End Try
    'End Function

    'Public Function InsertUploadTable(ByVal Table As DataTable, ByVal Status As String, _
    '                                    ByRef TableBack As DataTable, ByRef page As Page) As String
    '    Dim strTemp As String = ""
    '    Dim strKunnr As String = ""
    '    strKunnr = Right("0000000000" & m_user.KUNNR, 10)

    '    Try

    '        Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_KCL_CHANGE_AUFTR_002", m_app, m_user, Page)

    '        Dim ImpTbl As DataTable = myProxy.getImportTable("GT_AUFTRAG")

    '        For Each row As DataRow In Table.Rows
    '            Dim newRow As DataRow = ImpTbl.NewRow()

    '            For Each col As DataColumn In ImpTbl.Columns
    '                newRow(col.ColumnName) = row(col.ColumnName)
    '            Next

    '            newRow("MANDT") = "010"
    '            newRow("I_AKTION") = "N"
    '            newRow("STATUS") = Status
    '            newRow("E_FEHLER") = ""
    '            newRow("ZZZLDAT") = MakeDateSAP(newRow("ZZZLDAT").ToString)
    '            newRow("ERDAT") = MakeDateSAP(Now.ToString)
    '            newRow("Kunnr_AG") = Right("0000000000" & newRow("Kunnr_AG").ToString, 10)

    '            ImpTbl.Rows.Add(newRow)
    '        Next

    '        ImpTbl.AcceptChanges()

    '        myProxy.callBapi()

    '        Dim ExpTbl As DataTable = myProxy.getExportTable("GT_AUFTRAG")

    '        strTemp = ExpTbl.Rows(0)("E_FEHLER").ToString()

    '        Return strTemp
    '    Catch ex As Exception
    '        strTemp = ex.Message
    '        Return strTemp
    '    End Try
    'End Function

    'Public Function FilterEinzelSatz(ByVal ZulTable As DataTable, ByVal id_Anzeige As String) As DataTable
    '    Dim tmpTable As New DataTable
    '    Try
    '        tmpTable = TabUpload.Clone
    '        Dim tablerows As DataRow
    '        Dim newrow As DataRow
    '        Dim i As Integer
    '        newrow = tmpTable.NewRow
    '        tablerows = TabUpload.Select("Auf_ID='" & id_Anzeige & "'")(0)
    '        For i = 0 To tmpTable.Columns.Count - 1
    '            newrow(i) = tablerows(i)
    '        Next
    '        tmpTable.Rows.Add(newrow)

    '        tmpTable.AcceptChanges()
    '        Return tmpTable
    '    Catch ex As Exception
    '        Return tmpTable
    '    End Try
    'End Function

    Public Function SetNewZulassungsID() As Int32
        Dim command2 As New SqlClient.SqlCommand()
        openConnection()
        With command2
            .Connection = connection
            .CommandType = CommandType.Text
            .Parameters.Clear()
        End With
        command2.CommandText = "SELECT PValue FROM Parameters WHERE  (PName = 'HoechsteZulassungsID')"
        If id_sap > CType(command2.ExecuteScalar, System.Int32) Then
            command2.CommandText = "UPDATE Parameters SET PValue = " & id_sap.ToString & " WHERE  (PName = 'HoechsteZulassungsID')"
            command2.ExecuteNonQuery()
        End If
        closeConnection()
    End Function
End Class

' ************************************************
' $History: clsVorerf01.vb $
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 29.04.10   Time: 16:59
' Updated in $/CKAG/Applications/AppKroschke/Lib
' 
' *****************  Version 14  *****************
' User: Fassbenders  Date: 11.03.10   Time: 14:32
' Updated in $/CKAG/Applications/AppKroschke/Lib
' ITA: 2918
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 30.04.09   Time: 17:20
' Updated in $/CKAG/Applications/AppKroschke/Lib
' ITA: 2837
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 29.04.09   Time: 13:46
' Updated in $/CKAG/Applications/AppKroschke/Lib
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 26.01.09   Time: 10:54
' Updated in $/CKAG/Applications/AppKroschke/Lib
' ITA: 2513 Anpassungen
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 22.01.09   Time: 15:34
' Updated in $/CKAG/Applications/AppKroschke/Lib
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 22.01.09   Time: 13:01
' Updated in $/CKAG/Applications/AppKroschke/Lib
' ITA: 2513
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 5.12.08    Time: 9:08
' Updated in $/CKAG/Applications/AppKroschke/Lib
' ITA:2149
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 6.11.08    Time: 13:04
' Updated in $/CKAG/Applications/AppKroschke/Lib
' ITA: 2368
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 24.10.08   Time: 9:57
' Updated in $/CKAG/Applications/AppKroschke/Lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 21.10.08   Time: 17:48
' Updated in $/CKAG/Applications/AppKroschke/Lib
' ITA:2270
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 21.10.08   Time: 8:28
' Updated in $/CKAG/Applications/AppKroschke/Lib
' ITA: 2270
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 14.10.08   Time: 13:07
' Updated in $/CKAG/Applications/AppKroschke/Lib
' ITA: 2301 & Warnungen bereinigt
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 25.09.08   Time: 17:14
' Updated in $/CKAG/Applications/AppKroschke/Lib
' ITA 2213
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:30
' Created in $/CKAG/Applications/AppKroschke/Lib
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 30.11.07   Time: 10:59
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' ITA: 1347
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 13.11.07   Time: 16:38
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' ITA: 1374, 1404, 1433
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 21.08.07   Time: 17:40
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' ITA: 1242  Feinstaub und Krad beim nach Speichern/neuer Datensatz
' wieder rausgenommen
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 20.08.07   Time: 16:48
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' ITA: 1192 ,1242
' 
' *****************  Version 7  *****************
' User: Uha          Date: 5.07.07    Time: 9:52
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' Leerauswahl für ddlSTVA hinzugefügt (nach SAP-Lesen)
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 4.07.07    Time: 17:07
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.07.07    Time: 11:21
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 18:01
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 9:25
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Lib
' 
' ************************************************
