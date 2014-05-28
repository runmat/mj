Imports System.Web.Services
Imports System.Configuration
Imports System.IO
Imports System.Diagnostics

<WebService(Namespace:="http://kroschke.de/")> _
Public Class DADService
    Inherits System.Web.Services.WebService

#Region " Vom Webdienst-Designer generierter Code "

    Public Sub New()
        MyBase.New()

        'Dieser Aufruf ist für den Webdienst-Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Ihren eigenen Initialisierungscode hinter dem InitializeComponent()-Aufruf ein

    End Sub

    'Für den Webdienst-Designer erforderlich.
    Private components As System.ComponentModel.IContainer

    'HINWEIS: Die folgende Prozedur ist für den Webdienst-Designer erforderlich
    'Sie kann mit dem Webdienst-Designer modifiziert werden.
    'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
    Friend WithEvents EventLog1 As System.Diagnostics.EventLog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.EventLog1 = New System.Diagnostics.EventLog()
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'EventLog1
        '
        Me.EventLog1.Log = "Application"
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        'CODEGEN: Diese Prozedur ist für den Webdienst-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

#End Region

    ' WEBDIENSTBEISPIEL
    ' Der Beispieldienst HelloWorld() gibt die Zeichenfolge Hello World zurück.
    ' Zum Erstellen, entfernen Sie die Kommentare der folgenden Zeilen. Speichern und erstellen Sie dann das Projekt.
    ' Zum Testen des Webdiensts, stellen Sie sicher, dass die .asmx-Datei die Startseite ist,
    ' und drücken Sie auf F5.
    '
    '<WebMethod()> Public Function HelloWorld() As String
    '	HelloWorld = "Hello World"
    ' End Function

    Private strErr As String


    <WebMethod()> Public Function WMGetFahrzeug(ByVal User As String, ByVal Password As String, ByVal VIN As String, ByVal Date_From As String, ByVal Date_to As String) As String


        Dim strXml As String


        Try
            'Login überprüfen
            If CheckLogin(User, Password) = False Then

                Throw New Exception("WMGetFahrzeug, User oder Password nicht korrekt.")
            End If

            'Datumsparameter überprüfen
            If Date_From.Length > 0 Then
                If Date_From.Length <> 8 Then
                    Throw New Exception("WMGetFahrzeug, Date_From ist nicht 8-stellig(yyyymmdd).")
                End If

                If IsDate(Right(Date_From, 2) & "." & Mid(Date_From, 5, 2) & "." & Left(Date_From, 4)) = False Then
                    Throw New Exception("WMGetFahrzeug, Date_From - " & Date_From & " - ist kein güliges Datum.")
                End If

            End If

            If Date_to.Length > 0 Then
                If Date_to.Length <> 8 Then
                    Throw New Exception("WMGetFahrzeug, Date_to ist nicht 8-stellig(yyyymmdd).")
                End If

                If IsDate(Right(Date_to, 2) & "." & Mid(Date_to, 5, 2) & "." & Left(Date_to, 4)) = False Then
                    Throw New Exception("WMGetFahrzeug, Date_to - " & Date_to & " - ist kein güliges Datum.")
                End If

            End If


            If Date_From.Length = 8 And Date_to.Length = 8 Then

                If CDate(Right(Date_to, 2) & "." & Mid(Date_to, 5, 2) & "." & Left(Date_to, 4)) < _
                    CDate(Right(Date_From, 2) & "." & Mid(Date_From, 5, 2) & "." & Left(Date_From, 4)) Then
                    Throw New Exception("WMGetFahrzeug, Date_From ist größer als Date_to.")
                End If

            End If

            ' Auf Normales Datum umformen für ERP
            Date_From = Right(Date_From, 2) & "." & Mid(Date_From, 5, 2) & "." & Left(Date_From, 4)
            Date_to = Right(Date_to, 2) & "." & Mid(Date_to, 5, 2) & "." & Left(Date_to, 4)

            Dim GetData As  New Sixt1()

            strXml = GetData.GetBriefdaten(VIN, Date_From, Date_to)

        Catch ex As Exception
            'Error in das Eventlog schreiben
            EventLog1.WriteEntry("SixtService", "WMGetFahrzeug: " & Err.Description, EventLogEntryType.Warning)

            Throw ex
            Return strXml = String.Empty
        End Try

        
        Return strXml

    End Function


    <WebMethod()> Public Function WMInsertFreisetzung(ByVal User As String, ByVal Password As String, ByVal VehicleRegistrations As VehicleRegistrations) As Errors


        'Dim objSAP As New SAPProxy_SixtService.SAPProxy_SixtService()

        Try

            'Login überprüfen
            If CheckLogin(User, Password) = False Then
                Throw New Exception("WMInsertFreisetzung, User oder Password nicht korrekt.")
            End If

            Dim SetData As New Sixt1()

            Dim SapTable As DataTable = SetData.GetZulAuftrTable() 'New SAPProxy_SixtService.ZDAD_ZUL_AUFTR_1Table()
            Dim dr As DataRow 'New SAPProxy_SixtService.ZDAD_ZUL_AUFTR_1()

            Dim i As Int32
            Dim ErrCount As Int32 = 0


            Dim VehErrors As New Errors()



            For i = 0 To VehicleRegistrations.Count - 1

                With VehicleRegistrations.Item(i)

                    dr = SapTable.NewRow() 'New SAPProxy_SixtService.ZDAD_ZUL_AUFTR_1()

                    Try
                        'dr.Antriebkuerzel = CheckValue(.vehicleFuel, "vehicleFuel")
                        'dr.Ausfuerungkuerz = CheckValue(.vehicleFormat, "vehicleFormat")
                        'dr.Dat_Imp = Format(Today, "yyyyMMdd")
                        'If Len(Trim(.registrationDate)) = 0 Then Throw New Exception("registrationdate: Pflichtfeld enthaelt keinen Wert.")
                        'If IsDate(.registrationDate) = False Then Throw New Exception("registrationdate: Falsches Format.")
                        'dr.Datumzulassung = .registrationDate
                        'dr.Fahrzeugident = CheckValue(.vehicleID, "vehicleID")
                        'If Len(.vehicleID) <> 17 Then Throw New Exception("vehicleID: Nicht siebzehnstellig.")
                        'dr.Getriebekuerzel = CheckValue(.vehicleGear, "vehicleGear")
                        'dr.Haltercode = CheckValue(.registeredKeeperId, "registeredKeeperId")
                        'dr.Halterhausnumme = .registeredKeeperStreetNo
                        'dr.Halterland = .registeredKeeperCountry
                        'dr.Haltername1 = CheckValue(.registeredKeeperName1, "registeredKeeperName1")
                        'dr.Haltername2 = .registeredKeeperName2
                        'dr.Halterort = CheckValue(.registeredKeeperCity, "registeredKeeperCity")
                        'dr.Halterplz = CheckValue(.registeredKeeperPostCode, "registeredKeeperPostCode")
                        'dr.Halterstr = CheckValue(.registeredKeeperStreet, "registeredKeeperStreet")
                        'dr.Kennzeichenvorg = CheckValue(.vehicleLicenceNo, "vehicleLicenceNo")
                        'dr.Kunnr_Ag = "0000312680"
                        'dr.Modelbezeichnung = CheckValue(.vehicleType, "vehicleType")
                        'dr.Navikuerzel = CheckValue(.vehicleNavi, "vehicleNavi")
                        'dr.Nutzungartcode = CheckValue(.vehicleUsage, "vehicleUsage")
                        'dr.Reifenartkuerze = CheckValue(.vehicleTyre, "vehicleTyre")
                        'dr.Station = CheckValue(.registrationDestination, "registrationDestination")
                        'dr.Stationhausnumm = .registrationDestStreetNo
                        'dr.Stationland = CheckValue(.registrationDestCountry, "registrationDestCountry")
                        'dr.Stationname1 = CheckValue(.registrationDestName1, "registrationDestName1")
                        'dr.Stationname2 = .registrationDestName2
                        'dr.Stationort = CheckValue(.registrationDestCity, "registrationDestCity")
                        'dr.Stationplz = CheckValue(.registrationDestPostCode, "registrationDestPostCode")
                        'dr.Stationstr = .registrationDestStreet
                        'dr.Versicherungcod = CheckValue(.insuranceName, "insuranceName")

                        'If Len(Trim(.registeredLicenseeID)) > 0 Then
                        '    dr.Vnehmerhausnumm = .registeredLicenseeStreetNo
                        '    dr.Vnehmerland = .registeredLicenseeCountry
                        '    dr.Vnehmername1 = CheckValue(.registeredLicenseeName1, "registeredLicenseeName1")
                        '    dr.Vnehmername2 = .registeredLicenseeName2
                        '    dr.Vnehmerort = CheckValue(.registeredLicenseeCity, "registeredLicenseeCity")
                        '    dr.Vnehmerplz = CheckValue(.registeredLicenseePostCode, "registeredLicenseePostCode")
                        'End If

                        'dr.Vnehmercode = .registeredLicenseeID
                        'dr.Vnehmerhausnumm = .registeredLicenseeStreetNo
                        'dr.Vnehmerland = .registeredLicenseeCountry
                        'dr.Vnehmername1 = .registeredLicenseeName1
                        'dr.Vnehmername2 = .registeredLicenseeName2
                        'dr.Vnehmerort = .registeredLicenseeCity
                        'dr.Vnehmerplz = .registeredLicenseePostCode
                        'dr.Vnehmerstr = .registeredLicenseeStreet
                        'dr.Zeit_Imp = Format(TimeOfDay, "hhmmss")
                        'dr.Zulassungartco = .vehicleLicenceType
                        'dr.Zulortvorgabe = Right("0000000000" & CheckValue(.registrationLocationID, "registrationLocationID"), 10)

                        dr("Antriebkuerzel") = CheckValue(.vehicleFuel, "vehicleFuel")
                        dr("Ausfuerungkuerz") = CheckValue(.vehicleFormat, "vehicleFormat")
                        dr("Dat_Imp") = Today 'Format(Today, "yyyyMMdd")
                        If Len(Trim(.registrationDate)) = 0 Then Throw New Exception("registrationdate: Pflichtfeld enthaelt keinen Wert.")
                        If IsDate(.registrationDate) = False Then Throw New Exception("registrationdate: Falsches Format.")
                        dr("Datumzulassung") = .registrationDate
                        dr("Fahrzeugident") = CheckValue(.vehicleID, "vehicleID")
                        If Len(.vehicleID) <> 17 Then Throw New Exception("vehicleID: Nicht siebzehnstellig.")
                        dr("Getriebekuerzel") = CheckValue(.vehicleGear, "vehicleGear")
                        dr("Haltercode") = CheckValue(.registeredKeeperId, "registeredKeeperId")
                        dr("Halterhausnumme") = .registeredKeeperStreetNo
                        dr("Halterland") = .registeredKeeperCountry
                        dr("Haltername1") = CheckValue(.registeredKeeperName1, "registeredKeeperName1")
                        dr("Haltername2") = .registeredKeeperName2
                        dr("Halterort") = CheckValue(.registeredKeeperCity, "registeredKeeperCity")
                        dr("Halterplz") = CheckValue(.registeredKeeperPostCode, "registeredKeeperPostCode")
                        dr("Halterstr") = CheckValue(.registeredKeeperStreet, "registeredKeeperStreet")
                        dr("Kennzeichenvorg") = CheckValue(.vehicleLicenceNo, "vehicleLicenceNo")
                        dr("Kunnr_Ag") = "0000312680"
                        dr("Modelbezeichnung") = CheckValue(.vehicleType, "vehicleType")
                        dr("Navikuerzel") = CheckValue(.vehicleNavi, "vehicleNavi")
                        dr("Nutzungartcode") = CheckValue(.vehicleUsage, "vehicleUsage")
                        dr("Reifenartkuerze") = CheckValue(.vehicleTyre, "vehicleTyre")
                        dr("Station") = CheckValue(.registrationDestination, "registrationDestination")
                        dr("Stationhausnumm") = .registrationDestStreetNo
                        dr("Stationland") = CheckValue(.registrationDestCountry, "registrationDestCountry")
                        dr("Stationname1") = CheckValue(.registrationDestName1, "registrationDestName1")
                        dr("Stationname2") = .registrationDestName2
                        dr("Stationort") = CheckValue(.registrationDestCity, "registrationDestCity")
                        dr("Stationplz") = CheckValue(.registrationDestPostCode, "registrationDestPostCode")
                        dr("Stationstr") = .registrationDestStreet
                        dr("Versicherungcod") = CheckValue(.insuranceName, "insuranceName")

                        If Len(Trim(.registeredLicenseeID)) > 0 Then
                            dr("Vnehmerhausnumm") = .registeredLicenseeStreetNo
                            dr("Vnehmerland") = .registeredLicenseeCountry
                            dr("Vnehmername1") = CheckValue(.registeredLicenseeName1, "registeredLicenseeName1")
                            dr("Vnehmername2") = .registeredLicenseeName2
                            dr("Vnehmerort") = CheckValue(.registeredLicenseeCity, "registeredLicenseeCity")
                            dr("Vnehmerplz") = CheckValue(.registeredLicenseePostCode, "registeredLicenseePostCode")
                        End If

                        dr("Vnehmercode") = .registeredLicenseeID
                        dr("Vnehmerhausnumm") = .registeredLicenseeStreetNo
                        dr("Vnehmerland") = .registeredLicenseeCountry
                        dr("Vnehmername1") = .registeredLicenseeName1
                        dr("Vnehmername2") = .registeredLicenseeName2
                        dr("Vnehmerort") = .registeredLicenseeCity
                        dr("Vnehmerplz") = .registeredLicenseePostCode
                        dr("Vnehmerstr") = .registeredLicenseeStreet
                        dr("Zeit_Imp") = Format(TimeOfDay, "hhmmss")
                        dr("Zulassungartco") = .vehicleLicenceType
                        dr("Zulortvorgabe") = Right("0000000000" & CheckValue(.registrationLocationID, "registrationLocationID"), 10)


                        SapTable.Rows.Add(dr)

                    Catch InnerEx As Exception
                        Dim VehRegErr As New _Error()

                        VehRegErr.id = .vehicleID
                        VehRegErr.message = InnerEx.Message
                        VehErrors.Add(VehRegErr)

                    End Try


                End With

            Next




            SetData.SetZulAuftr(SapTable)

            Return VehErrors

        Catch
            EventLog.WriteEntry("SixtService", "WMInsertFreisetzung, Fehler beim Import: " & Err.Description, EventLogEntryType.Warning)
            Throw New Exception("WMInsertFreisetzung, Fehler beim Import:  " & Err.Number & ", " & Err.Description)
            'Finally
            'If IsNothing(objSAP.Connection) = False Then
            '    objSAP.Connection.Close()
            '    objSAP.Dispose()
            'End If
        End Try

    End Function


    <WebMethod()> Public Function WMInsertBrieffreigabe(ByVal User As String, ByVal Password As String, ByVal Brieffreigaben As Brieffreigaben) As Errors

      
        Try

            'Login überprüfen
            If CheckLogin(User, Password) = False Then

                Throw New Exception("WMInsertBrieffreigabe, User oder Password nicht korrekt.")
            End If

            Dim i As Int32
            Dim ErrCount As Int32 = 0


            Dim FreigabeErrors As New Errors()
            Dim SetData As New Sixt1()

            Dim TempTable As DataTable = SetData.GetSaveBrieffreigabeTable()
            Dim dr As DataRow

            'With TempTable
            '    .TableName = "GT_LIST"
            '    .Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
            '    .Columns.Add("LICENSE_NUM", System.Type.GetType("System.String"))
            '    .Columns.Add("KUNNR_R", System.Type.GetType("System.String"))
            '    .Columns.Add("NAME1_R", System.Type.GetType("System.String"))
            '    .Columns.Add("NAME2_R", System.Type.GetType("System.String"))
            '    .Columns.Add("STRASSE_R", System.Type.GetType("System.String"))
            '    .Columns.Add("PLZ_R", System.Type.GetType("System.String"))
            '    .Columns.Add("ORT_R", System.Type.GetType("System.String"))
            '    .Columns.Add("LANDX50_R", System.Type.GetType("System.String"))
            '    .Columns.Add("KUNNR_B", System.Type.GetType("System.String"))
            '    .Columns.Add("NAME1_B", System.Type.GetType("System.String"))
            '    .Columns.Add("NAME2_B", System.Type.GetType("System.String"))
            '    .Columns.Add("STRASSE_B", System.Type.GetType("System.String"))
            '    .Columns.Add("PLZ_B", System.Type.GetType("System.String"))
            '    .Columns.Add("ORT_B", System.Type.GetType("System.String"))
            '    .Columns.Add("LANDX50_B", System.Type.GetType("System.String"))
            '    .Columns.Add("KUNNR_S", System.Type.GetType("System.String"))
            '    .Columns.Add("NAME1_S", System.Type.GetType("System.String"))
            '    .Columns.Add("NAME2_S", System.Type.GetType("System.String"))
            '    .Columns.Add("STRASSE_S", System.Type.GetType("System.String"))
            '    .Columns.Add("PLZ_S", System.Type.GetType("System.String"))
            '    .Columns.Add("ORT_S", System.Type.GetType("System.String"))
            '    .Columns.Add("LANDX50_S", System.Type.GetType("System.String"))
            '    .Columns.Add("BRIEFVERS", System.Type.GetType("System.String"))
            '    .Columns.Add("BRIEFVERSBED", System.Type.GetType("System.String"))
            '    .Columns.Add("ABMELDUNG", System.Type.GetType("System.String"))
            '    'Neue Felder in der Struktur
            '    .Columns.Add("CODE_STO", System.Type.GetType("System.String"))
            '    .Columns.Add("NAME1_STO", System.Type.GetType("System.String"))
            '    .Columns.Add("NAME2_STO", System.Type.GetType("System.String"))
            '    .Columns.Add("ANSPP_STO", System.Type.GetType("System.String"))
            '    .Columns.Add("STRAS_STO", System.Type.GetType("System.String"))
            '    .Columns.Add("PSTLZ_STO", System.Type.GetType("System.String"))
            '    .Columns.Add("ORT01_STO", System.Type.GetType("System.String"))
            '    .Columns.Add("TEL_STO", System.Type.GetType("System.String"))
            '    .Columns.Add("FAX_STO", System.Type.GetType("System.String"))
            '    .Columns.Add("EMAIL_STO", System.Type.GetType("System.String"))


            'End With


            For i = 0 To Brieffreigaben.Count - 1

                With Brieffreigaben.Item(i)

                    dr = TempTable.NewRow

                    Try

                        dr("CHASSIS_NUM") = CheckValue(.fahrgestellNr, "fahrgestellNr")
                        dr("LICENSE_NUM") = CheckValue(.amtKennz, "amtKennz")
                        dr("KUNNR_R") = .rechKdnr
                        dr("NAME1_R") = .rechName
                        dr("NAME2_R") = .rechName2
                        dr("STRASSE_R") = .rechStrasse
                        dr("PLZ_R") = .rechPlz
                        dr("ORT_R") = .rechOrt
                        dr("LANDX50_R") = .rechLand

                        dr("KUNNR_B") = CheckValue(.briefKdnr, "briefKdnr")
                        dr("NAME1_B") = CheckValue(.briefName, "briefName")
                        dr("NAME2_B") = .briefName2
                        dr("STRASSE_B") = CheckValue(.briefStrasse, "briefStrasse")
                        dr("PLZ_B") = CheckValue(.briefPlz, "briefPlz")
                        dr("ORT_B") = CheckValue(.briefOrt, "briefOrt")
                        dr("LANDX50_B") = CheckValue(.briefLand, "briefLand")

                        If .schluesselKdnr.Length > 0 Then
                            dr("KUNNR_S") = CheckValue(.schluesselKdnr, "schluesselKdnr")
                            dr("NAME1_S") = CheckValue(.schluesselName, "schluesselName")
                            dr("NAME2_S") = .schluesselName2
                            dr("STRASSE_S") = CheckValue(.schluesselStrasse, "schluesselStrasse")
                            dr("PLZ_S") = CheckValue(.schluesselPlz, "schluesselPlz")
                            dr("ORT_S") = CheckValue(.schluesselOrt, "schluesselOrt")
                            dr("LANDX50_S") = CheckValue(.schluesselLand, "schluesselLand")
                        Else
                            dr("KUNNR_S") = .schluesselKdnr
                            dr("NAME1_S") = .schluesselName
                            dr("NAME2_S") = .schluesselName2
                            dr("STRASSE_S") = .schluesselStrasse
                            dr("PLZ_S") = .schluesselPlz
                            dr("ORT_S") = .schluesselOrt
                            dr("LANDX50_S") = .schluesselLand
                        End If


                        dr("BRIEFVERS") = CheckValue(.briefVersand, "briefVersand")
                        dr("BRIEFVERSBED") = CheckValue(.briefVersandBed, "briefVersandBed")

                        'If Len(Trim(.checkInDatum)) = 0 Then Throw New Exception("registrationdate: Pflichtfeld enthaelt keinen Wert.")
                        'If IsDate(.checkInDatum) = False Then Throw New Exception("registrationdate: Falsches Format.")
                        dr("ABMELDUNG") = .checkInDatum

                        TempTable.Rows.Add(dr)

                    Catch InnerEx As Exception
                        Dim BrieffreigabeErr As New _Error()

                        BrieffreigabeErr.id = .fahrgestellNr
                        BrieffreigabeErr.message = InnerEx.Message
                        FreigabeErrors.Add(BrieffreigabeErr)

                    End Try

                End With

                TempTable.AcceptChanges()

            Next

            SetData.SaveBrieffreigabe(TempTable)

            Return FreigabeErrors

        Catch

            EventLog.WriteEntry("SixtService", "WMInsertBrieffreigabe: " & Err.Description, EventLogEntryType.Warning)
            Throw New Exception("WMInsertBrieffreigabe, Fehler beim Import:  " & Err.Number & ", " & Err.Description)

        End Try
    End Function



    <WebMethod()> Public Function WMUpdateFahrzeug(ByVal User As String, ByVal Password As String, ByVal XmlString As String) As String

        Dim strXml As String


        Try

            'Login überprüfen
            If CheckLogin(User, Password) = False Then

                Throw New Exception("WMUpdateFahrzeug, User oder Password nicht korrekt.")
            End If


            'XML-String gefüllt?
            If XmlString.Length = 0 Then
                Throw New Exception("WMUpdateFahrzeug, XML-String nicht gefüllt.")
            End If


            Dim SetData As New Sixt1()


            strXml = SetData.GetVehicleChanges(XmlString)

        Catch ex As Exception
            'Error in das Eventlog schreiben

            EventLog1.WriteEntry("SixtService", "WMUpdateFahrzeug: " & Err.Description, EventLogEntryType.Warning)

            Throw ex
            Return strXml = String.Empty
        End Try

        Return strXml

    End Function

    <WebMethod()> Public Function WMGetFreisetzung(ByVal User As String, ByVal Password As String, ByVal DateZul As String) As String

        Dim strXml As String

        Try

            If CheckLogin(User, Password) = False Then

                'Error in das Eventlog schreiben
                EventLog1.WriteEntry("SixtService", "WMGetFreisetzung: " & Err.Description, EventLogEntryType.Warning)

                Throw New Exception("WMGetFreisetzung, User oder Password nicht korrekt")
            End If

            'Datumsparameter überprüfen
            If DateZul.Length > 0 Then
                If DateZul.Length <> 8 Then
                    Throw New Exception("WMGetFreisetzung, DateZul ist nicht 8-stellig(yyyymmdd).")
                End If

                If IsDate(Right(DateZul, 2) & "." & Mid(DateZul, 5, 2) & "." & Left(DateZul, 4)) = False Then
                    Throw New Exception("WMGetFreisetzung, Date_From - " & DateZul & " - ist kein güliges Datum.")
                End If
            Else
                Throw New Exception("WMGetFreisetzung, DateZul wurde nicht angegeben.")
            End If

            ' Auf Normales Datum umformen für ERP
            DateZul = Right(DateZul, 2) & "." & Mid(DateZul, 5, 2) & "." & Left(DateZul, 4)

            Dim SetData As  New Sixt1()

            strXml = SetData.GetZulFahrzeuge(DateZul)

            Return strXml

        Catch ex As Exception
            'Error in das Eventlog schreiben
            EventLog1.WriteEntry("SixtService", "WMGetFreisetzung: " & Err.Description, EventLogEntryType.Warning)

            Throw ex
            Return strXml = String.Empty
        End Try

        Return strXml

    End Function

    <WebMethod()> Public Function WMGetAbmeldung(ByVal User As String, ByVal Password As String, ByVal vonDatum As String, ByVal bisDatum As String) As VehicleDocuments

        Try

            If CheckLogin(User, Password) = False Then

                'Error in das Eventlog schreiben
                EventLog.WriteEntry("SixtService", "WMGetAbmeldung: " & Err.Description, EventLogEntryType.Warning)

                Throw New Exception("WMGetAbmeldung, User oder Password nicht korrekt")
            End If


            'Datumsparameter überprüfen
            If vonDatum.Length > 0 Then
                If vonDatum.Length <> 8 Then
                    Throw New Exception("WMGetAbmeldung, vonDatum ist nicht 8-stellig(yyyymmdd).")
                End If

                If IsDate(Right(vonDatum, 2) & "." & Mid(vonDatum, 5, 2) & "." & Left(vonDatum, 4)) = False Then
                    Throw New Exception("WMGetAbmeldung, vonDatum - " & vonDatum & " - ist kein güliges Datum.")
                End If

            End If

            If bisDatum.Length > 0 Then
                If bisDatum.Length <> 8 Then
                    Throw New Exception("WMGetAbmeldung, bisDatum ist nicht 8-stellig(yyyymmdd).")
                End If

                If IsDate(Right(bisDatum, 2) & "." & Mid(bisDatum, 5, 2) & "." & Left(bisDatum, 4)) = False Then
                    Throw New Exception("WMGetAbmeldung, bisDatum - " & bisDatum & " - ist kein güliges Datum.")
                End If

            End If


            If vonDatum.Length = 8 And bisDatum.Length = 8 Then

                If CDate(Right(bisDatum, 2) & "." & Mid(bisDatum, 5, 2) & "." & Left(bisDatum, 4)) < _
                    CDate(Right(vonDatum, 2) & "." & Mid(vonDatum, 5, 2) & "." & Left(vonDatum, 4)) Then
                    Throw New Exception("WMGetAbmeldung, vonDatum ist größer als bisDatum.")
                End If

            End If

            ' Auf Normales Datum umformen für ERP
            vonDatum = Right(vonDatum, 2) & "." & Mid(vonDatum, 5, 2) & "." & Left(vonDatum, 4)
            bisDatum = Right(bisDatum, 2) & "." & Mid(bisDatum, 5, 2) & "." & Left(bisDatum, 4)

            Dim SetData As New Sixt1
            Dim VehicleDocs As VehicleDocuments

            VehicleDocs = SetData.GetAbmeldungen(vonDatum, bisDatum)
            Return VehicleDocs

        Catch ex As Exception
            'Error in das Eventlog schreiben
            EventLog.WriteEntry("SixtService", "WMGetAbmeldung: " & Err.Description, EventLogEntryType.Warning)

            Throw ex

        End Try

    End Function


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

End Class
