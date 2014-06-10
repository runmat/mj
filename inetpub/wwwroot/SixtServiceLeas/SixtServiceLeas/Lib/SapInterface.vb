Imports System.Configuration
Imports System.Net.Mail

Public Class SapInterface

    Public Function InsertFreisetzung_Zul(ByVal VehicleRegistrations As VehicleRegs_Zul) As Errors
        Dim VehErrors As New Errors()
        Dim strKUNNR As String = "0000300997"

        Try
            Dim impTable As DataTable = S.AP.GetImportTableWithInit("Z_M_IMP_SERVICE_AUFTR_001.GT_WEB", "I_KUNNR", strKUNNR)

            For Each item As VehicleRegistrationZul In VehicleRegistrations
                With item
                    Try
                        Dim dr As DataRow = impTable.NewRow()

                        If String.IsNullOrEmpty(.Mandant) Then Throw New Exception("Mandant: Pflichtfeld enthaelt keinen Wert.")
                        dr("KUNMANDT") = .Mandant
                        dr("MANDT") = ""
                        dr("KUNNR_AG") = ""
                        If Len(Trim(.Auftragsgrund)) = 0 Then Throw New Exception("Auftragsgrund: Pflichtfeld enthaelt keinen Wert.")
                        dr("AUFGRUND") = .Auftragsgrund
                        If Len(Trim(.Aenderungskennzeichen)) = 0 Then Throw New Exception("Änderungskennzeichen: Pflichtfeld enthaelt keinen Wert.")
                        dr("AEKNZ") = .Aenderungskennzeichen
                        dr("CHASSIS_NUM") = .FahrzeugIdent
                        If Len(Trim(.Datum_Zulassung_Vorgabe)) = 0 Then Throw New Exception("Datum_Zulassung_Vorgabe: Pflichtfeld enthaelt keinen Wert.")
                        If Not IsDate(.Datum_Zulassung_Vorgabe) Then Throw New Exception("Datum_Zulassung_Vorgabe: Falsches Format.")
                        dr("ZDATUMZULASSUNG") = DateTime.Parse(.Datum_Zulassung_Vorgabe)
                        dr("TIDNR") = .Briefnummer
                        If Len(Trim(.Referenz1)) = 0 Then Throw New Exception("Referenz1: Pflichtfeld enthaelt keinen Wert.")
                        dr("REFERENZ1") = .Referenz1
                        dr("REFERENZ_2") = .Referenz2
                        If Len(Trim(.Versandart)) = 0 Then Throw New Exception("Versandart: Pflichtfeld enthaelt keinen Wert.")
                        dr("VERSART") = .Versandart
                        If Len(Trim(.Zul_Kreis)) = 0 Then Throw New Exception("Zul_Kreis: Pflichtfeld enthaelt keinen Wert.")
                        dr("ZULKREIS") = .Zul_Kreis
                        dr("WKENNZ_V1") = .WKZ
                        dr("WKENNZ_V2") = .WKZ_2
                        dr("WKENNZ_V3") = .WKZ_3
                        dr("WKENNZ_RES_AUF") = .WKZ_reserviert_auf
                        dr("BEMERKUNG") = .Bemerkung
                        dr("FEINSTAUB") = .Feinstaub
                        dr("VERSTRAEGER") = .Versicherungstraeger
                        dr("VERSMAKLER") = .Versicherungsmakler
                        dr("CODE_VSN") = .Vnehmer_Code
                        dr("NAME1_VSN") = .Vnehmer_Name1
                        dr("NAME2_VSN") = .Vnehmer_Name2
                        dr("STR_VSN") = .Vnehmer_Strasse
                        dr("HAUSNUMM_VSN") = .Vnehmer_Hausnummer
                        dr("PLZ_VSN") = .Vnehmer_PLZ
                        dr("ORT_VSN") = .Vnehmer_Ort
                        dr("LAND_VSN") = .Vnehmer_Land
                        If Len(Trim(.eVB_Nummer)) = 0 Then Throw New Exception("eVB_Nummer: Pflichtfeld enthaelt keinen Wert.")
                        If Len(Trim(.eVB_Nummer)) <> 7 Then Throw New Exception("eVB_Nummer: Pflichtfeld nicht 7stellig.")
                        dr("EVB_NUMMER") = .eVB_Nummer
                        If IsDate(.eVB_gueltig_bis) Then
                            dr("EVB_GUELTIG_BIS") = DateTime.Parse(.eVB_gueltig_bis)
                        End If

                        If IsDate(.eVB_gueltig_von) Then
                            dr("EVB_GUELTIG_VON") = DateTime.Parse(.eVB_gueltig_von)
                        End If
                        If Len(Trim(.ZulAuf)) = 0 Then Throw New Exception("ZulAuf: Pflichtfeld enthaelt keinen Wert.")
                        dr("ZULAUF") = .ZulAuf
                        If Len(Trim(.Halter_Code)) = 0 Then Throw New Exception("Halter_Code: Pflichtfeld enthaelt keinen Wert.")
                        dr("HALTER") = .Halter_Code
                        If Len(Trim(.Halter_Name1)) = 0 Then Throw New Exception("Halter_Name1: Pflichtfeld enthaelt keinen Wert.")
                        dr("NAME1_HA") = .Halter_Name1
                        dr("NAME2_HA") = .Halter_Name2
                        If Len(Trim(.Halter_Strasse)) = 0 Then Throw New Exception("Halter_Strasse: Pflichtfeld enthaelt keinen Wert.")
                        dr("STRASSE_HA") = .Halter_Strasse
                        dr("HAUSNR_HA") = .Halter_Hausnummer
                        If Len(Trim(.Halter_PLZ)) = 0 Then Throw New Exception("Halter_PLZ: Pflichtfeld enthaelt keinen Wert.")
                        If Len(Trim(.Halter_PLZ)) <> 5 Then Throw New Exception("Halter_PLZ: Pflichtfeld nicht 5stellig.")
                        dr("PSTLZ_HA") = .Halter_PLZ
                        If Len(Trim(.Halter_Ort)) = 0 Then Throw New Exception("Halter_Ort: Pflichtfeld enthaelt keinen Wert.")
                        dr("ORT_HA") = .Halter_Ort
                        dr("LAND_HA") = .Halter_Land
                        If Len(Trim(.Empf_SuS_Code)) = 0 Then Throw New Exception("Empf_SuS_Code: Pflichtfeld enthaelt keinen Wert.")
                        dr("EMPF_SUS") = .Empf_SuS_Code
                        If Len(Trim(.Empf_SuS_Name1)) = 0 Then Throw New Exception("Empf_SuS_Name1: Pflichtfeld enthaelt keinen Wert.")
                        dr("NAME1_SUS") = .Empf_SuS_Name1
                        dr("NAME2_SUS") = .Empf_SuS_Name2
                        If Len(Trim(.Empf_SuS_Strasse)) = 0 Then Throw New Exception("Empf_SuS_Strasse: Pflichtfeld enthaelt keinen Wert.")
                        dr("STRASSE_SUS") = .Empf_SuS_Strasse
                        dr("HAUSNR_SUS") = .Empf_SuS_Hausnummer
                        If Len(Trim(.Empf_SuS_PLZ)) = 0 Then Throw New Exception("Empf_SuS_PLZ: Pflichtfeld enthaelt keinen Wert.")
                        If Len(Trim(.Empf_SuS_PLZ)) <> 5 Then Throw New Exception("Empf_SuS_PLZ: Pflichtfeld nicht 5stellig.")
                        dr("PSTLZ_SUS") = .Empf_SuS_PLZ
                        If Len(Trim(.Empf_SuS_Ort)) = 0 Then Throw New Exception("Empf_SuS_Ort: Pflichtfeld enthaelt keinen Wert.")
                        dr("ORT_SUS") = .Empf_SuS_Ort
                        dr("LAND_SUS") = .Empf_SuS_Land
                        If Len(Trim(.Empf_Brief_Code)) = 0 Then Throw New Exception("Empf_Brief_Code: Pflichtfeld enthaelt keinen Wert.")
                        dr("EMPF_BRIEF") = .Empf_Brief_Code
                        If .Empf_Brief_Code = "1" Then
                            dr("NAME1_BF") = "DAD Deutscher Auto Dienst GmbH"
                            dr("NAME2_BF") = ""
                            dr("STRASSE_BF") = "Ladestraße"
                            dr("HAUSNR_BF") = "1"
                            dr("PSTLZ_BF") = "22926"
                            dr("ORT_BF") = "Ahrensburg"
                            dr("LAND_BF") = "DE"
                        Else
                            If Len(Trim(.Empf_Brief_Name1)) = 0 Then Throw New Exception("Empf_Brief_Name1: Pflichtfeld enthaelt keinen Wert.")
                            dr("NAME1_BF") = .Empf_Brief_Name1
                            dr("NAME2_BF") = .Empf_Brief_Name2
                            If Len(Trim(.Empf_Brief_Strasse)) = 0 Then Throw New Exception("Empf_Brief_Strasse: Pflichtfeld enthaelt keinen Wert.")
                            dr("STRASSE_BF") = .Empf_Brief_Strasse
                            dr("HAUSNR_BF") = .Empf_Brief_Hausnummer
                            If Len(Trim(.Empf_Brief_PLZ)) = 0 Then Throw New Exception("Empf_Brief_PLZ: Pflichtfeld enthaelt keinen Wert.")
                            If Len(Trim(.Empf_Brief_PLZ)) <> 5 Then Throw New Exception("Empf_Brief_PLZ: Pflichtfeld nicht 5stellig.")
                            dr("PSTLZ_BF") = .Empf_Brief_PLZ
                            If Len(Trim(.Empf_Brief_Ort)) = 0 Then Throw New Exception("Empf_Brief_Ort: Pflichtfeld enthaelt keinen Wert.")
                            dr("ORT_BF") = .Empf_Brief_Ort
                            dr("LAND_BF") = .Empf_Brief_Land
                        End If

                        dr("EQUNR") = ""
                        dr("ZFCODE") = ""

                        impTable.Rows.Add(dr)

                    Catch InnerEx As Exception
                        Dim VehRegErr As New _Error()

                        VehRegErr.id = .Referenz1
                        VehRegErr.message = InnerEx.Message
                        VehErrors.Add(VehRegErr)

                    End Try
                End With
            Next

            Dim expTable As DataTable = S.AP.GetExportTableWithExecute("GT_WEB")

            VerifyResult(impTable, expTable)

            Dim errTable As DataTable = GetErrors(expTable)

            If errTable IsNot Nothing Then
                For Each dRow As DataRow In errTable.Rows
                    Dim VehRegErr As New _Error()
                    VehRegErr.id = dRow("id").ToString
                    VehRegErr.message = dRow("Message").ToString
                    VehErrors.Add(VehRegErr)
                Next
            End If

        Catch ex As Exception
            Throw New Exception("WMInsertFreisetzung_Zul, Fehler beim Import:  " & ex.Message)
        End Try

        Return VehErrors
    End Function

    Public Function InsertFreisetzung_SonstDL(ByVal VehicleRegistrations As VehicleRegs_Sonst) As Errors
        Dim VehErrors As New Errors()
        Dim strKUNNR As String = "0000300997"

        Try
            Dim impTable As DataTable = S.AP.GetImportTableWithInit("Z_M_IMP_SERVICE_AUFTR_001.GT_WEB", "I_KUNNR", strKUNNR)

            For Each item As VehicleRegistrationSonst In VehicleRegistrations
                With item
                    Try
                        Dim dr As DataRow = impTable.NewRow()

                        If String.IsNullOrEmpty(.Mandant) Then Throw New Exception("Mandant: Pflichtfeld enthaelt keinen Wert.")
                        dr("KUNMANDT") = .Mandant
                        dr("MANDT") = ""
                        dr("KUNNR_AG") = ""
                        If Len(Trim(.Auftragsgrund)) = 0 Then Throw New Exception("Auftragsgrund: Pflichtfeld enthaelt keinen Wert.")
                        dr("AUFGRUND") = .Auftragsgrund
                        If Len(Trim(.Aenderungskennzeichen)) = 0 Then Throw New Exception("Änderungskennzeichen: Pflichtfeld enthaelt keinen Wert.")
                        dr("AEKNZ") = .Aenderungskennzeichen
                        dr("CHASSIS_NUM") = .FahrzeugIdent
                        If Len(Trim(.Datum_Durchfuehrung)) = 0 Then Throw New Exception("Datum_Durchfuehrung: Pflichtfeld enthaelt keinen Wert.")
                        If Not IsDate(.Datum_Durchfuehrung) Then Throw New Exception("Datum_Durchfuehrung: Falsches Format.")
                        dr("ZDATUMZULASSUNG") = DateTime.Parse(.Datum_Durchfuehrung)
                        dr("TIDNR") = .Briefnummer
                        If Len(Trim(.Referenz1)) = 0 Then Throw New Exception("Referenz1: Pflichtfeld enthaelt keinen Wert.")
                        dr("REFERENZ1") = .Referenz1
                        dr("REFERENZ_2") = .Referenz2
                        If Len(Trim(.Versandart)) = 0 Then Throw New Exception("Versandart: Pflichtfeld enthaelt keinen Wert.")
                        dr("VERSART") = .Versandart
                        If Len(Trim(.Zul_Kreis)) = 0 Then Throw New Exception("Zul_Kreis: Pflichtfeld enthaelt keinen Wert.")
                        dr("ZULKREIS") = .Zul_Kreis
                        dr("WKENNZ_V1") = .WKZ
                        dr("WKENNZ_V2") = .WKZ_2
                        dr("WKENNZ_V3") = .WKZ_3
                        dr("WKENNZ_RES_AUF") = .WKZ_reserviert_auf
                        dr("BEMERKUNG") = .Bemerkung
                        dr("FEINSTAUB") = .Feinstaub
                        dr("VERSTRAEGER") = .Versicherungstraeger
                        dr("VERSMAKLER") = .Versicherungsmakler
                        If Len(Trim(.eVB_Nummer)) = 0 Then Throw New Exception("eVB_Nummer: Pflichtfeld enthaelt keinen Wert.")
                        If Len(Trim(.eVB_Nummer)) <> 7 Then Throw New Exception("eVB_Nummer: Pflichtfeld nicht 7stellig.")
                        dr("EVB_NUMMER") = .eVB_Nummer
                        If IsDate(.eVB_gueltig_bis) Then
                            dr("EVB_GUELTIG_BIS") = DateTime.Parse(.eVB_gueltig_bis)
                        End If

                        If IsDate(.eVB_gueltig_von) Then
                            dr("EVB_GUELTIG_VON") = DateTime.Parse(.eVB_gueltig_von)
                        End If
                        If Len(Trim(.ZulAuf)) = 0 Then Throw New Exception("ZulAuf: Pflichtfeld enthaelt keinen Wert.")
                        dr("ZULAUF") = .ZulAuf
                        If Len(Trim(.Halter_Code)) = 0 Then Throw New Exception("Halter_Code: Pflichtfeld enthaelt keinen Wert.")
                        dr("HALTER") = .Halter_Code
                        If Len(Trim(.Halter_Name1)) = 0 Then Throw New Exception("Halter_Name1: Pflichtfeld enthaelt keinen Wert.")
                        dr("NAME1_HA") = .Halter_Name1
                        dr("NAME2_HA") = .Halter_Name2
                        If Len(Trim(.Halter_Strasse)) = 0 Then Throw New Exception("Halter_Strasse: Pflichtfeld enthaelt keinen Wert.")
                        dr("STRASSE_HA") = .Halter_Strasse
                        dr("HAUSNR_HA") = .Halter_Hausnummer
                        If Len(Trim(.Halter_PLZ)) = 0 Then Throw New Exception("Halter_PLZ: Pflichtfeld enthaelt keinen Wert.")
                        If Len(Trim(.Halter_PLZ)) <> 5 Then Throw New Exception("Halter_PLZ: Pflichtfeld nicht 5stellig.")
                        dr("PSTLZ_HA") = .Halter_PLZ
                        If Len(Trim(.Halter_Ort)) = 0 Then Throw New Exception("Halter_Ort: Pflichtfeld enthaelt keinen Wert.")
                        dr("ORT_HA") = .Halter_Ort
                        dr("LAND_HA") = .Halter_Land
                        If Len(Trim(.Empf_SuS_Code)) = 0 Then Throw New Exception("Empf_SuS_Code: Pflichtfeld enthaelt keinen Wert.")
                        dr("EMPF_SUS") = .Empf_SuS_Code
                        If Len(Trim(.Empf_SuS_Name1)) = 0 Then Throw New Exception("Empf_SuS_Name1: Pflichtfeld enthaelt keinen Wert.")
                        dr("NAME1_SUS") = .Empf_SuS_Name1
                        dr("NAME2_SUS") = .Empf_SuS_Name2
                        If Len(Trim(.Empf_SuS_Strasse)) = 0 Then Throw New Exception("Empf_SuS_Strasse: Pflichtfeld enthaelt keinen Wert.")
                        dr("STRASSE_SUS") = .Empf_SuS_Strasse
                        dr("HAUSNR_SUS") = .Empf_SuS_Hausnummer
                        If Len(Trim(.Empf_SuS_PLZ)) = 0 Then Throw New Exception("Empf_SuS_PLZ: Pflichtfeld enthaelt keinen Wert.")
                        If Len(Trim(.Empf_SuS_PLZ)) <> 5 Then Throw New Exception("Empf_SuS_PLZ: Pflichtfeld nicht 5stellig.")
                        dr("PSTLZ_SUS") = .Empf_SuS_PLZ
                        If Len(Trim(.Empf_SuS_Ort)) = 0 Then Throw New Exception("Empf_SuS_Ort: Pflichtfeld enthaelt keinen Wert.")
                        dr("ORT_SUS") = .Empf_SuS_Ort
                        dr("LAND_SUS") = .Empf_SuS_Land
                        If Len(Trim(.Empf_Brief_Code)) = 0 Then Throw New Exception("Empf_Brief_Code: Pflichtfeld enthaelt keinen Wert.")
                        dr("EMPF_BRIEF") = .Empf_Brief_Code
                        If .Empf_Brief_Code = "1" Then
                            dr("NAME1_BF") = "DAD Deutscher Auto Dienst GmbH"
                            dr("NAME2_BF") = ""
                            dr("STRASSE_BF") = "Ladestraße"
                            dr("HAUSNR_BF") = "1"
                            dr("PSTLZ_BF") = "22926"
                            dr("ORT_BF") = "Ahrensburg"
                            dr("LAND_BF") = "DE"
                        Else
                            If Len(Trim(.Empf_Brief_Name1)) = 0 Then Throw New Exception("Empf_Brief_Name1: Pflichtfeld enthaelt keinen Wert.")
                            dr("NAME1_BF") = .Empf_Brief_Name1
                            dr("NAME2_BF") = .Empf_Brief_Name2
                            If Len(Trim(.Empf_Brief_Strasse)) = 0 Then Throw New Exception("Empf_Brief_Strasse: Pflichtfeld enthaelt keinen Wert.")
                            dr("STRASSE_BF") = .Empf_Brief_Strasse
                            dr("HAUSNR_BF") = .Empf_Brief_Hausnummer
                            If Len(Trim(.Empf_Brief_PLZ)) = 0 Then Throw New Exception("Empf_Brief_PLZ: Pflichtfeld enthaelt keinen Wert.")
                            If Len(Trim(.Empf_Brief_PLZ)) <> 5 Then Throw New Exception("Empf_Brief_PLZ: Pflichtfeld nicht 5stellig.")
                            dr("PSTLZ_BF") = .Empf_Brief_PLZ
                            If Len(Trim(.Empf_Brief_Ort)) = 0 Then Throw New Exception("Empf_Brief_Ort: Pflichtfeld enthaelt keinen Wert.")
                            dr("ORT_BF") = .Empf_Brief_Ort
                            dr("LAND_BF") = .Empf_Brief_Land
                        End If

                        dr("EQUNR") = ""
                        dr("ZFCODE") = ""

                        impTable.Rows.Add(dr)

                    Catch InnerEx As Exception
                        Dim VehRegErr As New _Error()

                        VehRegErr.id = .Referenz1
                        VehRegErr.message = InnerEx.Message
                        VehErrors.Add(VehRegErr)

                    End Try
                End With
            Next

            Dim expTable As DataTable = S.AP.GetExportTableWithExecute("GT_WEB")

            VerifyResult(impTable, expTable)

            Dim errTable As DataTable = GetErrors(expTable)

            If errTable IsNot Nothing Then
                For Each dRow As DataRow In errTable.Rows
                    Dim VehRegErr As New _Error()
                    VehRegErr.id = dRow("id").ToString
                    VehRegErr.message = dRow("Message").ToString
                    VehErrors.Add(VehRegErr)
                Next
            End If

        Catch ex As Exception
            Throw New Exception("WMInsertFreisetzung_SonstDL, Fehler beim Import:  " & ex.Message)
        End Try

        Return VehErrors
    End Function

    Public Function InsertFreisetzung_EndgVers(ByVal VehicleRegistrations As VehicleRegs_EndgVers) As Errors
        Dim VehErrors As New Errors()
        Dim strKUNNR As String = "0000300997"

        Try
            Dim impTable As DataTable = S.AP.GetImportTableWithInit("Z_M_IMP_SERVICE_AUFTR_001.GT_WEB", "I_KUNNR", strKUNNR)

            For Each item As VehicleRegistrationEndgVers In VehicleRegistrations
                With item
                    Try
                        Dim dr As DataRow = impTable.NewRow()

                        If String.IsNullOrEmpty(.Mandant) Then Throw New Exception("Mandant: Pflichtfeld enthaelt keinen Wert.")
                        dr("KUNMANDT") = .Mandant
                        dr("MANDT") = ""
                        dr("KUNNR_AG") = ""
                        dr("AUFGRUND") = "EVE"
                        dr("AEKNZ") = "N"
                        dr("WKENNZ_V1") = .Akt_Kennzeichen
                        If Len(Trim(.FahrzeugIdent)) = 0 Then Throw New Exception("Fahrgestellnummer: Pflichtfeld enthaelt keinen Wert.")
                        If Len(Trim(.FahrzeugIdent)) <> 17 Then Throw New Exception("Fahrgestellnummer: Feld ist nicht 17stellig. ")
                        dr("CHASSIS_NUM") = .FahrzeugIdent
                        If Len(Trim(.Referenznummer)) = 0 Then Throw New Exception("Referenz1: Pflichtfeld enthaelt keinen Wert.")
                        dr("REFERENZ1") = .Referenznummer
                        If Len(Trim(.Haendler_Code)) = 0 Then Throw New Exception("Haendler_Code: Pflichtfeld enthaelt keinen Wert.")
                        dr("EMPF_BRIEF") = .Haendler_Code
                        If Len(Trim(.Empf_Name1)) = 0 Then Throw New Exception("Empf_Name1: Pflichtfeld enthaelt keinen Wert.")
                        dr("NAME1_BF") = .Empf_Name1
                        dr("NAME2_BF") = .Empf_Name2
                        If Len(Trim(.Empf_Strasse)) = 0 Then Throw New Exception("Empf_Strasse: Pflichtfeld enthaelt keinen Wert.")
                        dr("STRASSE_BF") = .Empf_Strasse
                        dr("HAUSNR_BF") = .Empf_Hausnummer
                        If Len(Trim(.Empf_PLZ)) = 0 Then Throw New Exception("Empf_PLZ: Pflichtfeld enthaelt keinen Wert.")
                        If Len(Trim(.Empf_PLZ)) <> 5 Then Throw New Exception("Empf_PLZ: Pflichtfeld nicht 5stellig.")
                        If Len(Trim(.Empf_Ort)) = 0 Then Throw New Exception("Empf_Ort: Pflichtfeld enthaelt keinen Wert.")
                        dr("PSTLZ_BF") = .Empf_PLZ
                        dr("ORT_BF") = .Empf_Ort
                        dr("LAND_BF") = .Empf_Land
                        dr("VERSART") = .Zustellart

                        dr("EQUNR") = ""
                        dr("ZFCODE") = ""

                        impTable.Rows.Add(dr)

                    Catch InnerEx As Exception
                        Dim VehRegErr As New _Error()

                        VehRegErr.id = .Referenznummer
                        VehRegErr.message = InnerEx.Message
                        VehErrors.Add(VehRegErr)

                    End Try
                End With
            Next

            Dim expTable As DataTable = S.AP.GetExportTableWithExecute("GT_WEB")

            VerifyResult(impTable, expTable)

            Dim errTable As DataTable = GetErrors(expTable)

            If errTable IsNot Nothing Then
                For Each dRow As DataRow In errTable.Rows
                    Dim VehRegErr As New _Error()
                    VehRegErr.id = dRow("id").ToString
                    VehRegErr.message = dRow("Message").ToString
                    VehErrors.Add(VehRegErr)
                Next
            End If

        Catch ex As Exception
            Throw New Exception("WMInsertFreisetzung_EndgVers, Fehler beim Import:  " & ex.Message)
        End Try

        Return VehErrors
    End Function

    Public Function InsertFreisetzung_TempVers(ByVal VehicleRegistrations As VehicleRegs_TempVers) As Errors
        Dim VehErrors As New Errors()
        Dim strKUNNR As String = "0000300997"

        Try
            Dim impTable As DataTable = S.AP.GetImportTableWithInit("Z_M_IMP_SERVICE_AUFTR_001.GT_WEB", "I_KUNNR", strKUNNR)

            For Each item As VehicleRegistrationTempVers In VehicleRegistrations
                With item
                    Try
                        Dim dr As DataRow = impTable.NewRow()

                        If String.IsNullOrEmpty(.Mandant) Then Throw New Exception("Mandant: Pflichtfeld enthaelt keinen Wert.")
                        dr("KUNMANDT") = .Mandant
                        dr("MANDT") = ""
                        dr("KUNNR_AG") = ""
                        If Not .Versandgrund = String.Empty AndAlso Not .Versandgrund = "12" Then
                            dr("AUFGRUND") = "TVE_" & .Versandgrund
                        Else
                            dr("AUFGRUND") = "TVE"
                        End If
                        dr("AEKNZ") = "N"
                        dr("WKENNZ_V1") = .Akt_Kennzeichen
                        If Len(Trim(.FahrzeugIdent)) = 0 Then Throw New Exception("Fahrgestellnummer: Pflichtfeld enthaelt keinen Wert.")
                        If Len(Trim(.FahrzeugIdent)) <> 17 Then Throw New Exception("Fahrgestellnummer: Feld ist nicht 17stellig.")
                        dr("CHASSIS_NUM") = .FahrzeugIdent
                        dr("TIDNR") = .Briefnummer
                        If Len(Trim(.Referenznummer)) = 0 Then Throw New Exception("Referenz1: Pflichtfeld enthaelt keinen Wert.")
                        dr("REFERENZ1") = .Referenznummer
                        dr("BEMERKUNG") = .Bemerkung
                        If Len(Trim(.Halter_Code)) = 0 Then Throw New Exception("Halter_Code: Pflichtfeld enthaelt keinen Wert.")
                        dr("HALTER") = .Halter_Code
                        If Len(Trim(.Halter_Name1)) = 0 Then Throw New Exception("Halter_Name1: Pflichtfeld enthaelt keinen Wert.")
                        dr("NAME1_HA") = .Halter_Name1
                        dr("NAME2_HA") = .Halter_Name2
                        If Len(Trim(.Halter_Strasse)) = 0 Then Throw New Exception("Halter_Strasse: Pflichtfeld enthaelt keinen Wert.")
                        dr("STRASSE_HA") = .Halter_Strasse
                        dr("HAUSNR_HA") = .Halter_Hausnummer
                        If Len(Trim(.Halter_PLZ)) = 0 Then Throw New Exception("Halter_PLZ: Pflichtfeld enthaelt keinen Wert.")
                        If Len(Trim(.Halter_PLZ)) <> 5 Then Throw New Exception("Halter_PLZ: Pflichtfeld nicht 5stellig.")
                        dr("PSTLZ_HA") = .Halter_PLZ
                        If Len(Trim(.Halter_Ort)) = 0 Then Throw New Exception("Halter_Ort: Pflichtfeld enthaelt keinen Wert.")
                        dr("ORT_HA") = .Halter_Ort
                        dr("LAND_HA") = .Halter_Land
                        If Len(Trim(.Haendler_Code)) = 0 Then Throw New Exception("Haendler_Code: Pflichtfeld enthaelt keinen Wert.")
                        dr("EMPF_BRIEF") = .Haendler_Code
                        If Len(Trim(.Empf_Name1)) = 0 Then Throw New Exception("Empf_Name1: Pflichtfeld enthaelt keinen Wert.")
                        dr("NAME1_BF") = .Empf_Name1
                        dr("NAME2_BF") = .Empf_Name2
                        If Len(Trim(.Empf_Strasse)) = 0 Then Throw New Exception("Empf_Strasse: Pflichtfeld enthaelt keinen Wert.")
                        dr("STRASSE_BF") = .Empf_Strasse
                        dr("HAUSNR_BF") = .Empf_Hausnummer
                        If Len(Trim(.Empf_PLZ)) = 0 Then Throw New Exception("Empf_PLZ: Pflichtfeld enthaelt keinen Wert.")
                        If Len(Trim(.Empf_PLZ)) <> 5 Then Throw New Exception("Empf_PLZ: Pflichtfeld nicht 5stellig.")
                        dr("PSTLZ_BF") = .Empf_PLZ
                        If Len(Trim(.Empf_Ort)) = 0 Then Throw New Exception("Empf_Ort: Pflichtfeld enthaelt keinen Wert.")
                        dr("ORT_BF") = .Empf_Ort
                        dr("LAND_BF") = .Empf_Land
                        dr("VERSART") = .Zustellart

                        dr("EQUNR") = ""
                        dr("ZFCODE") = ""

                        impTable.Rows.Add(dr)

                    Catch InnerEx As Exception
                        Dim VehRegErr As New _Error()

                        VehRegErr.id = .Referenznummer
                        VehRegErr.message = InnerEx.Message
                        VehErrors.Add(VehRegErr)

                    End Try
                End With
            Next

            Dim expTable As DataTable = S.AP.GetExportTableWithExecute("GT_WEB")

            VerifyResult(impTable, expTable)

            Dim errTable As DataTable = GetErrors(expTable)

            If errTable IsNot Nothing Then
                For Each dRow As DataRow In errTable.Rows
                    Dim VehRegErr As New _Error()
                    VehRegErr.id = dRow("id").ToString
                    VehRegErr.message = dRow("Message").ToString
                    VehErrors.Add(VehRegErr)
                Next
            End If

        Catch ex As Exception
            Throw New Exception("WMInsertFreisetzung_TempVers, Fehler beim Import:  " & ex.Message)
        End Try

        Return VehErrors
    End Function

    Private Sub VerifyResult(ByVal impTable As DataTable, ByVal expTable As DataTable)
        Dim blnError As Boolean = False
        Dim strMessage As String = ""

        If impTable.Rows.Count <> expTable.Rows.Count Then
            blnError = True
            strMessage &= "Z_M_IMP_SERVICE_AUFTR_001: Die Anzahl der Exportdatensätze stimmt nicht mit der Anzahl der Importdatensätze überein!" & vbNewLine
        Else
            For i As Integer = 0 To impTable.Rows.Count - 1
                Dim impRow As DataRow = impTable.Rows(i)
                Dim expRow As DataRow = expTable.Rows(i)

                For j As Integer = 0 To impRow.ItemArray.Length
                    If Not String.IsNullOrEmpty(impRow.ItemArray(j).ToString()) AndAlso impRow.ItemArray(j) <> expRow.ItemArray(j) Then
                        blnError = True
                        strMessage &= "Z_M_IMP_SERVICE_AUFTR_001: Exportdaten weichen von Importdaten ab (Import: " & impRow.ItemArray(j).ToString() & ", Export: " & expRow.ItemArray(j).ToString() & ")!" & vbNewLine
                    End If
                Next
            Next
        End If

        If blnError Then
            SendErrorMail(strMessage)
        End If

    End Sub

    Private Sub SendErrorMail(ByVal message As String)
        Dim mail As New MailMessage()
        mail.From = New MailAddress(ConfigurationManager.AppSettings("ErrorMailAbsender"))
        mail.Subject = "SixtLeasing-Webservice: Fehler beim Datenimport"
        mail.Body = message
        mail.To.Add(ConfigurationManager.AppSettings("ErrorMailEmpfaenger").Replace(";"c, ","c))

        Dim mailserver As New SmtpClient(ConfigurationManager.AppSettings("SmtpServer"))
        mailserver.Send(mail)
    End Sub

    Private Function GetErrors(ByVal exportTable As DataTable) As DataTable
        Dim datRows() As DataRow = exportTable.Select("ZFCODE<>''")
        Dim tmpTable As DataTable = Nothing

        If datRows.Length > 0 Then

            tmpTable = New DataTable
            tmpTable.Columns.Add("id", Type.GetType("System.String"))
            tmpTable.Columns.Add("Message", Type.GetType("System.String"))

            For Each dRow As DataRow In datRows
                Dim newRow As DataRow = tmpTable.NewRow()
                Select Case dRow("ZFCODE").ToString
                    Case "001"
                        newRow("id") = dRow("REFERENZ1").ToString
                        newRow("Message") = "Es existiert kein Datensatz zu diesem Vertrag - keine Datenänderung /-löschung"
                        tmpTable.Rows.Add(newRow)
                    Case "002"
                        newRow("id") = dRow("REFERENZ1").ToString
                        newRow("Message") = "Zum Vertrag existiert noch ein unerledigter Datensatz mit gleichem Auftragsgrund - keine Datenübernahme"
                        tmpTable.Rows.Add(newRow)
                    Case "003"
                        newRow("id") = dRow("REFERENZ1").ToString
                        newRow("Message") = "Änderungskennzeichen unbekannt - keine Datenübernahme"
                        tmpTable.Rows.Add(newRow)
                    Case "008"
                        newRow("id") = dRow("REFERENZ1").ToString
                        newRow("Message") = "Fehler bei Insert  - keine Datenübernahme"
                        tmpTable.Rows.Add(newRow)
                    Case "009"
                        newRow("id") = dRow("REFERENZ1").ToString
                        newRow("Message") = "Auftrag kann nicht mehr storniert werden"
                        tmpTable.Rows.Add(newRow)
                End Select
            Next
        End If

        Return tmpTable
    End Function

    Public Function WMGetFreisetzungStatus() As String
        Dim strKUNNR As String = "0000300997"
        Dim strTest As String = ConfigurationManager.AppSettings("ISTEST")

        Try
            S.AP.Init("Z_M_STATUS_SIXT_LS_001", "I_KUNNR, I_TEST", strKUNNR, strTest)

            Return S.AP.GetExportParameterWithExecute("E_XML")

        Catch ex As Exception
            Select Case CastSapBizTalkErrorMessage(ex.Message)

                Case "NO_DATA"
                    Throw New Exception("WMInsertFreisetzung_Status, Fehler beim Export: Keine Daten gefunden!")
                Case Else
                    Throw New Exception("WMInsertFreisetzung_Status, Fehler beim Export:  " & ex.Message)
            End Select

        End Try
    End Function

    Public Shared Function CastSapBizTalkErrorMessage(ByVal errorMessage As String) As String
        If errorMessage.Contains("SapErrorMessage") = True Then

            Dim errMessage As String = Mid(errorMessage, errorMessage.IndexOf("SapErrorMessage") + 17, _
                        errorMessage.Substring((errorMessage.IndexOf("SapErrorMessage") + 16)).IndexOf("."))

            errMessage = Replace(errMessage, "EXCEPTION", "")
            errMessage = Replace(errMessage, "RAISED", "").Trim

            Return errMessage
        Else
            Return errorMessage

        End If
    End Function

End Class