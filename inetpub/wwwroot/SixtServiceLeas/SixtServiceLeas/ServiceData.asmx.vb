Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

<System.Web.Services.WebService(Namespace:="http://kroschke.de/")> _
Public Class ServiceData
    Inherits System.Web.Services.WebService

    Friend WithEvents EventLog1 As System.Diagnostics.EventLog

    <WebMethod()> Public Function WMInsertFreisetzung_Zul(ByVal User As String, ByVal Password As String, ByVal VehicleRegistrations As VehicleRegs_Zul) As Errors

        Dim VehErrors As New Errors()

        Try

            'Login überprüfen
            If CheckLogin(User, Password) = False Then
                Throw New Exception("WMInsertFreisetzung_Zul, User oder Password nicht korrekt.")
            End If

            Dim SetData As New SapInterface()

            Dim SapTable As New DataTable
            Dim dr As DataRow

            Dim i As Int32
            Dim ErrCount As Int32 = 0


            SapTable = SetData.TabellenSpaltenZul()

            For i = 0 To VehicleRegistrations.Count - 1

                With VehicleRegistrations.Item(i)

                    dr = SapTable.NewRow

                    Try
                        dr("MANDT") = ""
                        dr("KUNNR_AG") = ""
                        If Len(Trim(.Auftragsgrund)) = 0 Then Throw New Exception("Auftragsgrund: Pflichtfeld enthaelt keinen Wert.")
                        dr("AUFGRUND") = .Auftragsgrund
                        If Len(Trim(.Aenderungskennzeichen)) = 0 Then Throw New Exception("Änderungskennzeichen: Pflichtfeld enthaelt keinen Wert.")
                        dr("AEKNZ") = .Aenderungskennzeichen
                        dr("CHASSIS_NUM") = .FahrzeugIdent
                        If Len(Trim(.Datum_Zulassung_Vorgabe)) = 0 Then Throw New Exception("Datum_Zulassung_Vorgabe: Pflichtfeld enthaelt keinen Wert.")
                        If IsDate(.Datum_Zulassung_Vorgabe) = False Then Throw New Exception("Datum_Zulassung_Vorgabe: Falsches Format.")
                        dr("ZDATUMZULASSUNG") = .Datum_Zulassung_Vorgabe 'MakeDateSAP(.Datum_Zulassung_Vorgabe)
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
                            dr("EVB_GUELTIG_BIS") = .eVB_gueltig_bis 'MakeDateSAP(.eVB_gueltig_bis)
                        Else
                            dr("EVB_GUELTIG_BIS") = DBNull.Value '"00000000"
                        End If

                        If IsDate(.eVB_gueltig_von) Then
                            dr("EVB_GUELTIG_VON") = .eVB_gueltig_von 'MakeDateSAP(.eVB_gueltig_von)
                        Else
                            dr("EVB_GUELTIG_VON") = DBNull.Value '"00000000"
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

                        dr("DAT_ERL") = DBNull.Value '""
                        dr("DAT_IMP") = DBNull.Value '""
                        dr("DAT_AEND") = DBNull.Value '""
                        dr("EQUNR") = ""
                        dr("ZFCODE") = ""

                        SapTable.Rows.Add(dr)


                    Catch InnerEx As Exception
                        Dim VehRegErr As New _Error()

                        VehRegErr.id = .Referenz1
                        VehRegErr.message = InnerEx.Message
                        VehErrors.Add(VehRegErr)

                    End Try

                End With

            Next


            Dim errTable As DataTable
            errTable = SetData.InsertFreisetzungZul(SapTable)

            If Not errTable Is Nothing Then

                Dim row As DataRow
                For Each row In errTable.Rows
                    Dim VehRegErr As New _Error()
                    VehRegErr.id = row("id").ToString
                    VehRegErr.message = row("Message").ToString
                    VehErrors.Add(VehRegErr)
                Next

            End If
            Return VehErrors

        Catch

            EventLog.WriteEntry("SixtServiceLeas", "WMInsertFreisetzung_Zul: " & Err.Description, EventLogEntryType.Warning)
            Return VehErrors
        End Try

    End Function

    <WebMethod()> Public Function WMInsertFreisetzung_SonstDL(ByVal User As String, ByVal Password As String, ByVal VehicleRegistrations As VehicleRegs_Sonst) As Errors

        Dim VehErrors As New Errors()

        Try

            'Login überprüfen
            If CheckLogin(User, Password) = False Then
                Throw New Exception("WMInsertFreisetzung_SonstDL, User oder Password nicht korrekt.")
            End If

            Dim SetData As New SapInterface()

            Dim SapTable As New DataTable
            Dim dr As DataRow

            Dim i As Int32
            Dim ErrCount As Int32 = 0


            SapTable = SetData.TabellenSpaltenZul()

            SapTable.TableName = ""

            For i = 0 To VehicleRegistrations.Count - 1

                With VehicleRegistrations.Item(i)

                    dr = SapTable.NewRow

                    Try
                        dr("MANDT") = ""
                        dr("KUNNR_AG") = ""
                        If Len(Trim(.Auftragsgrund)) = 0 Then Throw New Exception("Auftragsgrund: Pflichtfeld enthaelt keinen Wert.")
                        dr("AUFGRUND") = .Auftragsgrund
                        If Len(Trim(.Aenderungskennzeichen)) = 0 Then Throw New Exception("Änderungskennzeichen: Pflichtfeld enthaelt keinen Wert.")
                        dr("AEKNZ") = .Aenderungskennzeichen
                        dr("CHASSIS_NUM") = .FahrzeugIdent
                        If Len(Trim(.Datum_Durchfuehrung)) = 0 Then Throw New Exception("Datum_Durchfuehrung: Pflichtfeld enthaelt keinen Wert.")
                        If IsDate(.Datum_Durchfuehrung) = False Then Throw New Exception("Datum_Durchfuehrung: Falsches Format.")
                        dr("ZDATUMZULASSUNG") = .Datum_Durchfuehrung'MakeDateSAP(.Datum_Durchfuehrung)
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
                            dr("EVB_GUELTIG_BIS") = .eVB_gueltig_bis 'MakeDateSAP(.eVB_gueltig_bis)
                        Else
                            dr("EVB_GUELTIG_BIS") = DBNull.Value '"00000000"
                        End If

                        If IsDate(.eVB_gueltig_von) Then
                            dr("EVB_GUELTIG_VON") = .eVB_gueltig_von 'MakeDateSAP(.eVB_gueltig_von)
                        Else
                            dr("EVB_GUELTIG_VON") = DBNull.Value '"00000000"
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

                        dr("DAT_IMP") = DBNull.Value '""
                        dr("DAT_AEND") = DBNull.Value '""
                        dr("EQUNR") = ""
                        dr("ZFCODE") = ""
                        SapTable.Rows.Add(dr)


                    Catch InnerEx As Exception
                        Dim VehRegErr As New _Error()
                        VehRegErr.id = .Referenz1
                        VehRegErr.message = InnerEx.Message
                        VehErrors.Add(VehRegErr)
                    End Try
                End With
            Next

            Dim errTable As DataTable
            errTable = SetData.InsertFreisetzungZul(SapTable)

            If Not errTable Is Nothing Then
                Dim row As DataRow
                For Each row In errTable.Rows
                    Dim VehRegErr As New _Error()
                    VehRegErr.id = row("id").ToString
                    VehRegErr.message = row("Message").ToString
                    VehErrors.Add(VehRegErr)
                Next

            End If
            Return VehErrors

        Catch

            EventLog.WriteEntry("SixtServiceLeas", "WMInsertFreisetzung_SonstDL: " & Err.Description, EventLogEntryType.Warning)
            Return VehErrors
        End Try

    End Function



    <WebMethod()> Public Function WMInsertFreisetzung_EndgVers(ByVal User As String, ByVal Password As String, ByVal VehicleRegistrations As VehicleRegs_EndgVers) As Errors

        Try

            'Login überprüfen
            If CheckLogin(User, Password) = False Then
                Throw New Exception("WMInsertFreisetzung_EngVers, User oder Password nicht korrekt.")
            End If

            Dim SetData As  New SapInterface()

            Dim SapTable As New DataTable
            Dim dr As DataRow

            Dim i As Int32
            Dim ErrCount As Int32 = 0


            Dim VehErrors As New Errors()

            SapTable = SetData.TabellenSpaltenZul()

            SapTable.TableName = ""

            For i = 0 To VehicleRegistrations.Count - 1

                With VehicleRegistrations.Item(i)

                    dr = SapTable.NewRow

                    Try
                        dr("MANDT") = ""
                        dr("KUNNR_AG") = ""
                        dr("AUFGRUND") = "EVE"
                        dr("AEKNZ") = "N"
                        dr("WKENNZ_V1") = .Akt_Kennzeichen
                        If Len(Trim(.FahrzeugIdent)) = 0 Then Throw New Exception("Fahrgestellnummer: Pflichtfeld enthaelt keinen Wert.")
                        If Len(Trim(.FahrzeugIdent)) <> 17 Then Throw New Exception("Fahrgestellnummer: Feld ist nicht 17stellig. ")
                        dr("CHASSIS_NUM") = .FahrzeugIdent
                        dr("ZDATUMZULASSUNG") = DBNull.Value '"00000000"
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

                        dr("DAT_IMP") = DBNull.Value
                        dr("DAT_AEND") = DBNull.Value
                        dr("EQUNR") = ""
                        dr("ZFCODE") = ""
                        SapTable.Rows.Add(dr)


                    Catch InnerEx As Exception
                        Dim VehRegErr As New _Error()

                        VehRegErr.id = .Referenznummer
                        VehRegErr.message = InnerEx.Message
                        VehErrors.Add(VehRegErr)

                    End Try
                End With
            Next

            
            Dim errTable As DataTable
            errTable = SetData.InsertFreisetzungZul(SapTable)

            If Not errTable Is Nothing Then
                Dim row As DataRow
                For Each row In errTable.Rows
                    Dim VehRegErr As New _Error()
                    VehRegErr.id = row("id").ToString
                    VehRegErr.message = row("Message").ToString
                    VehErrors.Add(VehRegErr)
                Next

            End If
            Return VehErrors
        Catch
            Throw New Exception("WMInsertFreisetzung_EngVers, Fehler beim Import:  " & Err.Number & ", " & Err.Description)
            'Finally
            'If IsNothing(objSAP.Connection) = False Then
            '    objSAP.Connection.Close()
            '    objSAP.Dispose()
            'End If
        End Try

    End Function

    <WebMethod()> Public Function WMInsertFreisetzung_TempVers(ByVal User As String, ByVal Password As String, ByVal VehicleRegistrations As VehicleRegs_TempVers) As Errors

        Try

            'Login überprüfen
            If CheckLogin(User, Password) = False Then
                Throw New Exception("WMInsertFreisetzung_TempVers, User oder Password nicht korrekt.")
            End If

            Dim SetData As New SapInterface()

            Dim SapTable As New DataTable
            Dim dr As DataRow

            Dim i As Int32
            Dim ErrCount As Int32 = 0


            Dim VehErrors As New Errors()

            SapTable = SetData.TabellenSpaltenZul()

            SapTable.TableName = ""

            For i = 0 To VehicleRegistrations.Count - 1

                With VehicleRegistrations.Item(i)

                    dr = SapTable.NewRow

                    Try
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
                        dr("ZDATUMZULASSUNG") = DBNull.Value '"00000000"
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
                        dr("DAT_IMP") = DBNull.Value '""
                        dr("DAT_AEND") = DBNull.Value '""
                        dr("EQUNR") = ""
                        dr("ZFCODE") = ""
                        SapTable.Rows.Add(dr)

                    Catch InnerEx As Exception
                        Dim VehRegErr As New _Error()
                        VehRegErr.message = InnerEx.Message
                        VehRegErr.id = .Referenznummer
                        VehErrors.Add(VehRegErr)
                    End Try
                End With
            Next

           
            Dim errTable As DataTable
            errTable = SetData.InsertFreisetzungZul(SapTable)

            If Not errTable Is Nothing Then
                Dim row As DataRow
                For Each row In errTable.Rows
                    Dim VehRegErr As New _Error()
                    VehRegErr.id = row("id").ToString
                    VehRegErr.message = row("Message").ToString
                    VehErrors.Add(VehRegErr)
                Next

            End If

            Return VehErrors
        Catch
            Throw New Exception("WMInsertFreisetzung_TempVers, Fehler beim Import:  " & Err.Number & ", " & Err.Description)
            'Finally
            'If IsNothing(objSAP.Connection) = False Then
            '    objSAP.Connection.Close()
            '    objSAP.Dispose()
            'End If
        End Try

    End Function

    Private Sub InitializeComponent()
        Me.EventLog1 = New System.Diagnostics.EventLog
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).BeginInit()
      
        Me.EventLog1.Log = "Application"
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Private Function CheckLogin(ByVal User As String, ByVal Password As String) As Boolean

        If User <> ConfigurationManager.AppSettings("Username") OrElse Password <> ConfigurationManager.AppSettings("Password") Then
            Return False
        Else
            Return True
        End If

    End Function

    Private Function CheckValue(ByVal strValue As String, ByVal strFeldname As String) As String

        If Len(Trim(strValue)) = 0 Then
            Throw New Exception(strFeldname & ": Pflichtfeld enthaelt keinen Wert.")
        Else
            CheckValue = strValue
        End If

    End Function

    'Private Function TabellenSpaltenZul() As DataTable

    'Dim TempTable As New DataTable

    'TempTable.Columns.Add("MANDT", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("KUNNR_AG", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("REFERENZ1", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("AUFGRUND", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("DAT_IMP", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("AEKNZ", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("ZDATUMZULASSUNG", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("TIDNR", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("REFERENZ_2", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("VERSART", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("ZULKREIS", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("WKENNZ_V1", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("WKENNZ_V2", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("WKENNZ_V3", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("WKENNZ_RES_AUF", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("BEMERKUNG", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("FEINSTAUB", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("VERSTRAEGER", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("VERSMAKLER", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("CODE_VSN", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("NAME1_VSN", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("NAME2_VSN", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("STR_VSN", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("HAUSNUMM_VSN", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("PLZ_VSN", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("ORT_VSN", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("LAND_VSN", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("EVB_NUMMER", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("EVB_GUELTIG_VON", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("EVB_GUELTIG_BIS", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("ZULAUF", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("HALTER", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("NAME1_HA", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("NAME2_HA", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("STRASSE_HA", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("HAUSNR_HA", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("PSTLZ_HA", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("ORT_HA", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("LAND_HA", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("EMPF_SUS", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("NAME1_SUS", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("NAME2_SUS", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("STRASSE_SUS", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("HAUSNR_SUS", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("PSTLZ_SUS", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("ORT_SUS", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("LAND_SUS", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("EMPF_BRIEF", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("NAME1_BF", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("NAME2_BF", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("STRASSE_BF", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("HAUSNR_BF", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("PSTLZ_BF", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("ORT_BF", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("LAND_BF", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("DAT_AEND", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("DAT_ERL", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("EQUNR", System.Type.GetType("System.String"))
    'TempTable.Columns.Add("ZFCODE", System.Type.GetType("System.String"))



    ' Return TempTable

    'End Function


    <WebMethod()> Public Function WMGetFreisetzung_Status(ByVal User As String, ByVal Password As String) As String
        Dim strXml As String


        Try

            'Login überprüfen
            If CheckLogin(User, Password) = False Then

                Throw New Exception("WMGetFreisetzung_Status, User oder Password nicht korrekt.")
            End If

            Dim SetData As New SapInterface()

            strXml = SetData.WMGetFreisetzungStatus()

        Catch ex As Exception
            'Error in das Eventlog schreiben
            EventLog.WriteEntry("SixtServiceLeas", "WMGetFreisetzung_Status: " & Err.Description, EventLogEntryType.Warning)

            Throw ex
            Return strXml = String.Empty
        End Try

        Return strXml
    End Function


    Public Function MakeDateSAP(ByVal datInput As Date) As String
        REM $ Formt Date-Input in String YYYYMMDD um
        Return Year(datInput) & Right("0" & Month(datInput), 2) & Right("0" & Day(datInput), 2)
    End Function

    Public Sub New()
        MyBase.New()

        'Dieser Aufruf ist für den Webdienst-Designer erforderlich.
        InitializeComponent()
        ' Fügen Sie Ihren eigenen Initialisierungscode hinter dem InitializeComponent()-Aufruf ein

    End Sub

End Class