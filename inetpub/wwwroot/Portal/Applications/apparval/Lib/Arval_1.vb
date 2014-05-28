Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Configuration
Imports CKG.Base.Business
Imports CKG.Base.Kernel

<Serializable()> Public Class Arval_1
    REM § Lese-/Schreibfunktion, Kunde: Arval, 
    REM § Show - BAPI: Z_M_ZULF_FZGE_ARVAL,
    REM § Change - BAPI: ?.

    Inherits BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private m_strHaendlernummer As String
    Private m_strHalterNummer As String
    Private m_strStandortNummer As String
    Private m_strZielFirma As String
    Private m_strZielFirma2 As String
    Private m_strZielStrasse As String
    Private m_strZielHNr As String
    Private m_strZielPLZ As String
    Private m_strZielOrt As String
    Private m_strZielLand As String
    Private m_blnArvalZulassung As Boolean
    Private m_strSucheFgstNr As String
    Private m_strSucheLvNr As String'
    Private m_kbanr As String
    Private m_zulkz As String
    Private m_afnam As String
    Private m_Fahrzeuge As Int32
    Private m_tableGrund As DataTable
    Private m_versandadr As String
    Private m_versandadrtext As String
    Private m_versicherung As String
    Private m_material As String
    Private m_schein As String
    Private m_abckz As String
    Private m_equ As String
    Private m_kennz As String
    Private m_tidnr As String
    Private m_liznr As String
    Private m_versgrund As String
    Private m_versgrundText As String
    Private strAuftragsstatus As String
    Private strAuftragsnummer As String
    Private dataArray As ArrayList
    Private m_blnAbwVersandadresse As Boolean
    Private rowToChange As DataRow
    Private m_strBemerkung As String
    Private m_tblLaender As DataTable

#End Region

#Region " Properties"
    Public Property Bemerkung() As String
        Get
            Return m_strBemerkung
        End Get
        Set(ByVal Value As String)
            m_strBemerkung = Value
        End Set
    End Property

    Public Property Versicherung() As String
        Get
            Return m_versicherung
        End Get
        Set(ByVal Value As String)
            m_versicherung = Value
        End Set
    End Property

    Public Property AbwVersandadresse() As Boolean
        Get
            Return m_blnAbwVersandadresse
        End Get
        Set(ByVal Value As Boolean)
            m_blnAbwVersandadresse = Value
        End Set
    End Property


    Public Property VersandGrundText() As String
        Get
            Return m_versgrundText
        End Get
        Set(ByVal Value As String)
            m_versgrundText = Value
        End Set
    End Property

    Public Property rowChange() As DataRow
        Get
            Return rowToChange
        End Get
        Set(ByVal Value As DataRow)
            rowToChange = Value
        End Set
    End Property

    Public Property objData() As ArrayList
        Get
            Return dataArray
        End Get
        Set(ByVal Value As ArrayList)
            dataArray = Value
        End Set
    End Property

    Public Property Auftragsstatus() As String
        Get
            Return strAuftragsstatus
        End Get
        Set(ByVal Value As String)
            strAuftragsstatus = Value
        End Set
    End Property

    Public Property Auftragsnummer() As String
        Get
            Return strAuftragsnummer
        End Get
        Set(ByVal Value As String)
            strAuftragsnummer = Value
        End Set
    End Property

    Public Property VersandGrund() As String
        Get
            Return m_versgrund
        End Get
        Set(ByVal Value As String)
            m_versgrund = Value
        End Set
    End Property

    Public Property LizenzNr() As String
        Get
            Return m_liznr
        End Get
        Set(ByVal Value As String)
            m_liznr = Value
        End Set
    End Property

    Public Property TIDNr() As String
        Get
            Return m_tidnr
        End Get
        Set(ByVal Value As String)
            m_tidnr = Value
        End Set
    End Property

    Public Property Kennzeichen() As String
        Get
            Return m_kennz
        End Get
        Set(ByVal Value As String)
            m_kennz = Value
        End Set
    End Property

    Public Property Equimpent() As String
        Get
            Return m_equ
        End Get
        Set(ByVal Value As String)
            m_equ = Value
        End Set
    End Property

    Public Property Versandart() As String
        Get
            Return m_abckz
        End Get
        Set(ByVal Value As String)
            m_abckz = Value
        End Set
    End Property

    Public Property ScheinSchildernummer() As String
        Get
            Return m_schein
        End Get
        Set(ByVal Value As String)
            m_schein = Value
        End Set
    End Property

    Public Property Materialnummer() As String
        Get
            Return m_material
        End Get
        Set(ByVal Value As String)
            m_material = Value
        End Set
    End Property

    Public Property VersandAdresse() As String
        Get
            Return m_versandadr
        End Get
        Set(ByVal Value As String)
            m_versandadr = Value
        End Set
    End Property

    Public Property VersandAdresseText() As String
        Get
            Return m_versandadrtext
        End Get
        Set(ByVal Value As String)
            m_versandadrtext = Value
        End Set
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblResult
        End Get
        Set(ByVal Value As DataTable)
            m_tblResult = Value
        End Set
    End Property
    Public ReadOnly Property GrundTabelle() As DataTable
        Get
            Return m_tableGrund
        End Get
    End Property

    Public Property SucheLvNr() As String
        Get
            Return m_strSucheLvNr
        End Get
        Set(ByVal Value As String)
            m_strSucheLvNr = Value
        End Set
    End Property

    Public Property SucheFgstNr() As String
        Get
            Return m_strSucheFgstNr
        End Get
        Set(ByVal Value As String)
            m_strSucheFgstNr = Value
        End Set
    End Property

    Public Property ArvalZulassung() As Boolean
        Get
            Return m_blnArvalZulassung
        End Get
        Set(ByVal Value As Boolean)
            m_blnArvalZulassung = Value
        End Set
    End Property

    Public Property ZielLand() As String
        Get
            Return m_strZielLand
        End Get
        Set(ByVal Value As String)
            m_strZielLand = Value
        End Set
    End Property

    Public Property ZielOrt() As String
        Get
            Return m_strZielOrt
        End Get
        Set(ByVal Value As String)
            m_strZielOrt = Value
        End Set
    End Property

    Public Property ZielPLZ() As String
        Get
            Return m_strZielPLZ
        End Get
        Set(ByVal Value As String)
            m_strZielPLZ = Value
        End Set
    End Property

    Public Property ZielHNr() As String
        Get
            Return m_strZielHNr
        End Get
        Set(ByVal Value As String)
            m_strZielHNr = Value
        End Set
    End Property

    Public Property ZielStrasse() As String
        Get
            Return m_strZielStrasse
        End Get
        Set(ByVal Value As String)
            m_strZielStrasse = Value
        End Set
    End Property

    Public Property ZielFirma2() As String
        Get
            Return m_strZielFirma2
        End Get
        Set(ByVal Value As String)
            m_strZielFirma2 = Value
        End Set
    End Property

    Public Property ZielFirma() As String
        Get
            Return m_strZielFirma
        End Get
        Set(ByVal Value As String)
            m_strZielFirma = Value
        End Set
    End Property

    Public Property Haendlernummer() As String
        Get
            Return m_strHaendlernummer
        End Get
        Set(ByVal Value As String)
            m_strHaendlernummer = Value
        End Set
    End Property

    Public Property HalterNummer() As String
        Get
            Return m_strHalterNummer
        End Get
        Set(ByVal Value As String)
            m_strHalterNummer = Value
        End Set
    End Property

    Public Property StandortNummer() As String
        Get
            Return m_strStandortNummer
        End Get
        Set(ByVal Value As String)
            m_strStandortNummer = Value
        End Set
    End Property

    Public ReadOnly Property Laender() As DataTable
        Get
            Return m_tblLaender
        End Get
    End Property


#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_blnArvalZulassung = False

        getLaender()
    End Sub

    Public Sub checkZulassungsdokumente(ByVal strAppID As String, ByVal strSessionID As String, ByRef table As DataTable,
                                        ByVal HalterName As String, ByVal HalterPlz As String)

        ClearError()

        Try

            S.AP.InitExecute("Z_M_Zuldokumente_Arval", "I_AG,I_ZHNAME,I_ZHPLZ,I_UNVOLLSTAENDIG", KUNNR, HalterName, HalterPlz, "X")

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Zuldokumente_Arval", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", KUNNR)
            'myProxy.setImportParameter("I_ZHNAME", HalterName)
            'myProxy.setImportParameter("I_ZHPLZ", HalterPlz)
            'myProxy.setImportParameter("I_UNVOLLSTAENDIG", "X")

            'myProxy.callBapi()

            table = S.AP.GetExportTable("T_ZULDOKUMENTE") 'myProxy.getExportTable("T_ZULDOKUMENTE")
            
            WriteLogEntry(True, "KUNNR=" & KUNNR, m_tblResult)
        Catch ex As Exception
            RaiseError("-9999", "ERR_ZULDOKU")
            WriteLogEntry(False, "KUNNR=" & KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        Finally
            m_blnGestartet = False
        End Try

    End Sub
    
    Public Overrides Sub Show()
        m_strClassAndMethod = "Arval_1.Show"

        ClearError()

        If Not m_blnGestartet Then
            m_blnGestartet = True
            
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                NewResultTable()
                ClearError()

                Dim haendler As String

                If m_strHaendlernummer <> String.Empty Then
                    haendler = Right("0000000000" & m_strHaendlernummer, 10)    'Händler: nur eigene KFZ!
                Else
                    haendler = String.Empty                                     'Mitarbeiter: alle KFZ!
                End If
                
                S.AP.Init("Z_M_Zulf_Fzge_Arval")

                S.AP.SetImportParameter("I_AG", m_strKUNNR)
                S.AP.SetImportParameter("I_ZF", haendler)
                S.AP.SetImportParameter("I_LVNR", m_strSucheLvNr)
                S.AP.SetImportParameter("I_FZGID", m_strSucheFgstNr)


                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Zulf_Fzge_Arval", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
               
                S.AP.Execute()

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("T_FZGE")

                If tblTemp2.Rows.Count > 0 Then
                    Dim rowNew As DataRow
                    Dim tempRow As DataRow
                    For Each tempRow In tblTemp2.Rows
                        rowNew = m_tblResult.NewRow
                        rowNew("Ausgewaehlt") = False
                        rowNew("Kennzeichenserie") = False
                        rowNew("Vorreserviert") = False
                        rowNew("HaendlerNummer") = tempRow("Za_Kunnr").ToString
                        rowNew("LeasingNummer") = tempRow("Liznr").ToString
                        rowNew("FhgstNummer") = tempRow("Chassis_Num").ToString
                        rowNew("EquipmentNummer") = tempRow("Equnr").ToString
                        rowNew("DatumBriefeingang") = tempRow("Erdat").ToString
                        rowNew("Halter") = tempRow("Zh_Name1").ToString.Trim & "<br>" & tempRow("Zh_Plz").ToString.Trim & "&nbsp;" & tempRow("Zh_Ort").ToString.Trim
                        rowNew("HalterKunnr") = tempRow("Zh_Kunnr").ToString
                        rowNew("HalterName1") = tempRow("Zh_Name1").ToString
                        rowNew("HalterName2") = tempRow("Zh_Name2").ToString
                        rowNew("HalterStr") = tempRow("Zh_Strasse").ToString
                        rowNew("HalterNr") = tempRow("Zh_Hnr").ToString
                        rowNew("HalterPlz") = tempRow("Zh_Plz").ToString
                        rowNew("HalterOrt") = tempRow("Zh_Ort").ToString
                        'ZulStelle holen (Halter)
                        m_zulkz = ""
                        Dim strStatus As String = String.Empty
                        If tempRow("Zh_Plz").ToString.Length > 0 Or tempRow("Zh_Ort").ToString.Length > 0 Then
                            getZulStelle(m_strAppID, m_strSessionID, tempRow("Zh_Plz").ToString, tempRow("Zh_Ort").ToString, strStatus)
                        End If
                        rowNew("Zulstelle") = m_zulkz
                        rowNew("Standort") = tempRow("Za_Name1").ToString & "<br>" & tempRow("Za_Plz").ToString & "&nbsp;" & tempRow("Za_Ort").ToString
                        rowNew("StandortKunnr") = tempRow("Za_Kunnr").ToString
                        rowNew("StandortName1") = tempRow("Za_Name1").ToString
                        rowNew("StandortName2") = tempRow("Za_Name2").ToString
                        rowNew("StandortStr") = tempRow("Za_Strasse").ToString
                        rowNew("StandortNr") = tempRow("Za_Hnr").ToString
                        rowNew("StandortPlz") = tempRow("Za_Plz").ToString
                        rowNew("StandortOrt") = tempRow("Za_Ort").ToString

                        rowNew("Haendler") = tempRow("Zf_Name1").ToString & ", " & tempRow("Zf_Plz").ToString & "&nbsp;" & tempRow("Zf_Ort").ToString
                        rowNew("HaendlerKunnr") = tempRow("Zf_Kunnr").ToString
                        rowNew("HaendlerName1") = tempRow("Zf_Name1").ToString
                        rowNew("HaendlerName2") = tempRow("Zf_Name2").ToString
                        rowNew("HaendlerStr") = tempRow("Zf_Strasse").ToString
                        rowNew("HaendlerNr") = tempRow("Zf_Hnr").ToString
                        rowNew("HaendlerPlz") = tempRow("Zf_Plz").ToString
                        rowNew("HaendlerOrt") = tempRow("Zf_Ort").ToString
                        rowNew("Zulassungsstelle") = m_afnam
                        rowNew("Evbnummer") = tempRow("ZEVBNR").ToString
                        rowNew("EVB_TXT") = tempRow("ZEVBTXT").ToString
                        rowNew("DatumZulassung") = SuggestionDay(tempRow("Chassis_Num").ToString, m_afnam)

                        If (ArvalZulassung = False) Then        'Keine Arval-Zulassung
                            m_tblResult.Rows.Add(rowNew)
                        Else
                            If (tempRow("Zh_Kunnr").ToString) = m_strKUNNR Then       'Arval-Zulassung
                                m_tblResult.Rows.Add(rowNew)
                            End If
                        End If
                    Next
                Else
                    RaiseError("-2202", "Fehler: Keine Kontingentinformationen vorhanden.")
                End If

                For Each col As DataColumn In m_tblResult.Columns
                    col.ReadOnly = False
                Next

                If (ArvalZulassung And m_tblResult.Rows.Count = 0) Then
                    RaiseError("-3331", "Keine Daten gefunden.")
                End If

                WriteLogEntry(True, "KUNNR=" & Right(m_strCustomer, 5), m_tblResult)
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace(" Execution failed", "")
                    Case "ERR_NO_DATA"      'Änderung                      
                        RaiseError("-3331", "Keine Daten gefunden.")
                    Case "ERR_INV_AG"
                        RaiseError("-3332", "Allgemeiner Lesefehler.")
                    Case "ERR_INV_ZF"
                        RaiseError("-3333", "Ungültiger Händler """ & m_strHaendlernummer & """.")
                    Case Else
                        RaiseError("-9999", ex.Message)
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & Right(m_strCustomer, 5) & " , " & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String)
        'Fahrzeuge zulassen

        ClearError()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = "Vorgang OK"
                'Halter, Standort, Schein & Schilder, Brief. Pro Zulassung je ein Satz!
                '###Halter

                S.AP.Init("Z_M_Web_Zulassung_Arval")

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Web_Zulassung_Arval", m_objApp, m_objUser, page)

                Dim table As DataTable = S.AP.GetImportTable("T_PARTNERS") 'myProxy.getImportTable("T_PARTNERS")
                
                Dim tableRow As DataRow
                tableRow = table.NewRow
                tableRow("Kunnr") = Right("0000000000" & rowToChange("HalterKunnr").ToString, 10)
                tableRow("Parvw") = "ZH"
                If rowToChange("HalterKunnr").ToString = String.Empty Then
                    tableRow("Name1") = rowToChange("HalterName1").ToString
                    tableRow("Name2") = rowToChange("HalterName2").ToString
                    tableRow("Street") = rowToChange("HalterStr").ToString
                    tableRow("House_Num1") = rowToChange("HalterNr").ToString
                    tableRow("Post_Code1") = rowToChange("HalterPlz").ToString
                    tableRow("City1") = rowToChange("HalterOrt").ToString
                    tableRow("State") = "DE"
                End If
                table.Rows.Add(tableRow) 'Zeile hinzufügen

                '###Standort
                tableRow = table.NewRow

                If (rowToChange("StandortKunnr").ToString <> String.Empty) Then
                    tableRow("Kunnr") = Right("0000000000" & rowToChange("StandortKunnr").ToString, 10)
                Else
                    tableRow("Kunnr") = ""
                End If

                tableRow("Parvw") = "ZA"

                If rowToChange("StandortKunnr").ToString = String.Empty Then
                    tableRow("Name1") = rowToChange("StandortName1").ToString
                    tableRow("Name2") = rowToChange("StandortName2").ToString
                    tableRow("Street") = rowToChange("StandortStr").ToString
                    tableRow("House_Num1") = rowToChange("StandortNr").ToString
                    tableRow("Post_Code1") = rowToChange("StandortPlz").ToString
                    tableRow("City1") = rowToChange("StandortOrt").ToString
                    tableRow("State") = "DE"
                End If

                If (rowToChange("StandortKunnr").ToString <> "") Then
                    table.Rows.Add(tableRow) 'Zeile hinzufügen
                End If

                '###Schein & Schilder 
                tableRow = table.NewRow

                If rowToChange("HaendlerKunnr").ToString <> String.Empty Then
                    tableRow("Kunnr") = Right("0000000000" & rowToChange("HaendlerKunnr").ToString, 10)
                End If

                If rowToChange("Versandadresse").ToString = "X" Then
                    tableRow("Kunnr") = "0000390051"     'Kunnr für abweichende Adresse Schein & Schilder...
                    tableRow("Name1") = rowToChange("HaendlerName1").ToString
                    tableRow("Name2") = rowToChange("HaendlerName2").ToString
                    tableRow("Street") = rowToChange("HaendlerStr").ToString
                    tableRow("House_Num1") = rowToChange("HaendlerNr").ToString
                    tableRow("Post_Code1") = Right("00000" & rowToChange("HaendlerPlz").ToString, 5)
                    tableRow("City1") = rowToChange("HaendlerOrt").ToString
                    tableRow("State") = "DE"
                End If

                tableRow("Parvw") = "ZE"

                If (rowToChange("HaendlerKunnr").ToString <> "") Then
                    table.Rows.Add(tableRow) 'Zeile hinzufügen
                End If

                '###Briefempfänger (immer DAD)
                tableRow = table.NewRow

                tableRow("Kunnr") = "0000000001"
                tableRow("Parvw") = "ZS"

                table.Rows.Add(tableRow) 'Zeile hinzufügen
                table.AcceptChanges()

                S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & rowToChange("HaendlerKunnr").ToString, 10)) 'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & rowToChange("HaendlerKunnr").ToString, 10))
                S.AP.SetImportParameter("I_EQUNR", rowToChange("EquipmentNummer").ToString) 'myProxy.setImportParameter("I_EQUNR", rowToChange("EquipmentNummer").ToString)
                S.AP.SetImportParameter("I_AG", m_strKUNNR) 'myProxy.setImportParameter("I_AG", m_strKUNNR)
                S.AP.SetImportParameter("I_ERNAM", Left(m_objUser.UserName, 12)) 'myProxy.setImportParameter("I_ERNAM", Left(m_objUser.UserName, 12))
                If rowToChange("DatumZulassung").ToString <> String.Empty AndAlso IsDate(rowToChange("DatumZulassung").ToString) Then
                    S.AP.SetImportParameter("I_ZULDAT", rowToChange("DatumZulassung").ToString) ' myProxy.setImportParameter("I_ZULDAT", rowToChange("DatumZulassung").ToString)
                End If
                S.AP.SetImportParameter("I_ZULST", m_kbanr) 'myProxy.setImportParameter("I_ZULST", m_kbanr)
                S.AP.SetImportParameter("I_KENNZ", rowToChange("Zulstelle").ToString & "-") 'myProxy.setImportParameter("I_KENNZ", rowToChange("Zulstelle").ToString & "-")
                S.AP.SetImportParameter("I_WUK", rowToChange("Wunschkennzeichen").ToString) 'myProxy.setImportParameter("I_WUK", rowToChange("Wunschkennzeichen").ToString)
                S.AP.SetImportParameter("I_VERS", m_versicherung) 'myProxy.setImportParameter("I_VERS", m_versicherung)
                S.AP.SetImportParameter("I_EVBNR", rowToChange("Evbnummer").ToString) 'myProxy.setImportParameter("I_EVBNR", rowToChange("Evbnummer").ToString)
                If rowToChange("Evb_Von").ToString <> String.Empty AndAlso IsDate(rowToChange("Evb_Von").ToString) Then
                    S.AP.SetImportParameter("I_EVBVON", rowToChange("Evb_Von").ToString) 'myProxy.setImportParameter("I_EVBVON", rowToChange("Evb_Von").ToString)
                End If

                If rowToChange("Evb_bis").ToString <> String.Empty AndAlso IsDate(rowToChange("Evb_bis").ToString) Then
                    S.AP.SetImportParameter("I_EVBBIS", rowToChange("Evb_bis").ToString) 'myProxy.setImportParameter("I_EVBBIS", rowToChange("Evb_bis").ToString)
                End If

                S.AP.Execute()
                'myProxy.callBapi()
                
                rowToChange("Status") = m_strMessage 'Meldung merken

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace(" Execution failed", "")
                    Case "NO_AUFTRAG"
                        RaiseError("-3331", "Kein Auftrag angelegt.")
                    Case "NO_ZDADVERSAND"
                        RaiseError("-3332", "Keine Einträge für die Versanddatei erstellt.")
                    Case "ERR_INV_KUNNR"
                        RaiseError("-3333", "Unbekannte Kundennr.")
                    Case "ERR_NO_ZULDAT"
                        RaiseError("-3334", "Kein Zulassungsdatum angegeben.")
                    Case "ERR_INV_ZULDAT"
                        RaiseError("-3335", "Ungültiges Zulassungsdatum.")
                    Case "ERR_NO_TRANSNAME"
                        RaiseError("-3336", "Kein Transaktionsname angegeben.")
                    Case "ERR_INV_EQUNR"
                        RaiseError("-3337", "Ungültige Equipmentnr.")
                    Case "ERR_NO_LIF"
                        RaiseError("-3338", "Kein Zulassungsdienst gefunden.")
                    Case "ERR_INV_PARVW"
                        RaiseError("-3339", "Ungültige Partnerrolle.")
                    Case "ERR_INV_ZF"
                        RaiseError("-3340", "Ungültige Kundennr. für Händler.")
                    Case "ERR_INV_ZF_ABWADR"
                        RaiseError("-3341", "Fehlende Information zu abw. Adresse für Händler.")
                    Case "ERR_INV_ZH"
                        RaiseError("-3342", "Ungültige Kundennr. für Halter.")
                    Case "ERR_INV_ZH_ABWADR"
                        RaiseError("-3343", "Fehlende Information zu abw. Adresse für Halter.")
                    Case "ERR_INV_ZS"
                        RaiseError("-3344", "Ungültige Kundennr. für Empf. Brief.")
                    Case "ERR_INV_ZS_ABWADR"
                        RaiseError("-3345", "Fehlende Information zu abw. Adresse für Empf. Brief.")
                    Case "ERR_INV_ZE"
                        RaiseError("-3346", "Ungültige Kundennr. für Empf. Schein und Schilder.")
                    Case "ERR_INV_ZE_ABWADR"
                        RaiseError("-3347", "Fehlende Information zu abw. Adresse für Empf. Schein und Schilder.")
                    Case "ERR_INV_ZA"
                        RaiseError("-3348", "Ungültige Kundennr. für Empf. Standort.")
                    Case "ERR_INV_ZA_ABWADR"
                        RaiseError("-3348", "Fehlende Information zu abw. Adresse für Empf. Standort.")
                    Case "ERR_SAVE"
                        RaiseError("-3350", "Fehler beim Speichern.")
                    Case "ERR_NO_ZF"
                        RaiseError("-3351", "Kein Händler angegeben.")
                    Case "ERR_NO_ZH"
                        RaiseError("-3352", "Kein Halter angegeben.")
                    Case "ERR_NO_ZS"
                        RaiseError("-3353", "Kein Empfänger Brief angegeben.")
                    Case "ERR_NO_ZE"
                        RaiseError("-3354", "Kein Empfänger Schein und Schilder angegeben.")
                    Case "ERR_NO_EQUI"
                        RaiseError("-3355", "Kein Equipment zur Fahrgestellnr. gefunden.")
                    Case "ERR_NO_RE"
                        RaiseError("-3356", "Kein Rechnungsempfänger angegeben.")
                    Case "ERR_NO_RG"
                        RaiseError("-3357", "Kein Regulierer angegeben.")
                    Case "ERR_INV_RE"
                        RaiseError("-3358", "Ungültiger Rechnungsempfänger.")
                    Case "ERR_INV_RG"
                        RaiseError("-3359", "Ungültiger Regulierer.")
                    Case Else
                        RaiseError("-9999", ex.Message)
                End Select
            Finally
                rowToChange("Status") = m_strMessage 'Meldung merken
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()
    End Sub
    
    Public Function SuggestionDay(ByVal fahrgestellnr As String, ByVal afnam As String) As DateTime
        Dim datTemp As DateTime = CType(Now.ToShortDateString, Date)
        Dim intAddDays As Int32 = 0

        '§§§ 02.02.06: Nur 1 Tag!
        Do While (datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday Or intAddDays < 1)
            datTemp = datTemp.AddDays(1)
            If Not (datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday) Then
                intAddDays += 1
            End If
        Loop

        '§§§ 02.02.06: 1 Tag dazu wenn nicht München.
        If (afnam <> ConfigurationManager.AppSettings("ArvalAuftragMunchenLand")) Then
            datTemp = datTemp.Add(New TimeSpan(1, 0, 0, 0))
            '§§§ 02.02.06: WoEnde prüfen!
            If datTemp.DayOfWeek = DayOfWeek.Saturday Then
                datTemp = datTemp.Add(New TimeSpan(2, 0, 0, 0))
            End If
            If datTemp.DayOfWeek = DayOfWeek.Sunday Then
                datTemp = datTemp.Add(New TimeSpan(1, 0, 0, 0))
            End If
        End If
        '§§§ 02.02.06: 1 Tag dazu!
        If (fahrgestellnr = String.Empty) Then
            datTemp = datTemp.Add(New TimeSpan(1, 0, 0, 0))
        Else
            If (Now.Hour >= CType(ConfigurationManager.AppSettings("ArvalAuftragsEnde"), Double)) Then   'Nach 15 Uhr 1 Tag mehr...
                datTemp = datTemp.Add(New TimeSpan(1, 0, 0, 0))
            End If
        End If

        'WoEnde prüfen!
        If datTemp.DayOfWeek = DayOfWeek.Saturday Then
            datTemp = datTemp.Add(New TimeSpan(2, 0, 0, 0))
        End If
        If datTemp.DayOfWeek = DayOfWeek.Sunday Then
            datTemp = datTemp.Add(New TimeSpan(1, 0, 0, 0))
        End If

        Return datTemp
    End Function

    Public Sub ClearResultTable()
        m_tblResult = Nothing
    End Sub

    Private Sub NewResultTable()
        If m_tblResult Is Nothing Then
            m_tblResult = New DataTable()
            With m_tblResult.Columns
                .Add("Ausgewaehlt", System.Type.GetType("System.Boolean"))
                .Add("HaendlerNummer", System.Type.GetType("System.String"))
                .Add("EquipmentNummer", System.Type.GetType("System.String"))
                .Add("LeasingNummer", System.Type.GetType("System.String"))
                .Add("FhgstNummer", System.Type.GetType("System.String"))
                .Add("DatumBriefeingang", System.Type.GetType("System.DateTime"))
                .Add("DatumZulassung", System.Type.GetType("System.DateTime"))
                .Add("Wuk01_Buchstaben", System.Type.GetType("System.String"))
                .Add("Wuk01_Ziffern", System.Type.GetType("System.String"))
                .Add("Wuk02_Buchstaben", System.Type.GetType("System.String"))
                .Add("Wuk02_Ziffern", System.Type.GetType("System.String"))
                .Add("Wuk03_Buchstaben", System.Type.GetType("System.String"))
                .Add("Wuk03_Ziffern", System.Type.GetType("System.String"))
                .Add("Kennzeichenserie", System.Type.GetType("System.Boolean"))
                .Add("Vorreserviert", System.Type.GetType("System.Boolean"))
                .Add("Reservierungsdaten", System.Type.GetType("System.String"))
                .Add("Ergebnis", System.Type.GetType("System.String"))
                .Add("Halter", System.Type.GetType("System.String")) 'Name+PLZ+Ort als Anzeige im WEB
                .Add("HalterKunnr", System.Type.GetType("System.String"))
                .Add("HalterName1", System.Type.GetType("System.String"))
                .Add("HalterName2", System.Type.GetType("System.String"))
                .Add("HalterStr", System.Type.GetType("System.String"))
                .Add("HalterNr", System.Type.GetType("System.String"))
                .Add("HalterPlz", System.Type.GetType("System.String"))
                .Add("HalterOrt", System.Type.GetType("System.String"))

                .Add("Standort", System.Type.GetType("System.String")) ''Name+PLZ+Ort als Anzeige im WEB
                .Add("StandortKunnr", System.Type.GetType("System.String"))
                .Add("StandortName1", System.Type.GetType("System.String"))
                .Add("StandortName2", System.Type.GetType("System.String"))
                .Add("StandortStr", System.Type.GetType("System.String"))
                .Add("StandortNr", System.Type.GetType("System.String"))
                .Add("StandortPlz", System.Type.GetType("System.String"))
                .Add("StandortOrt", System.Type.GetType("System.String"))

                .Add("Haendler", System.Type.GetType("System.String"))
                .Add("HaendlerKunnr", System.Type.GetType("System.String"))
                .Add("HaendlerName1", System.Type.GetType("System.String"))
                .Add("HaendlerName2", System.Type.GetType("System.String"))
                .Add("HaendlerStr", System.Type.GetType("System.String"))
                .Add("HaendlerNr", System.Type.GetType("System.String"))
                .Add("HaendlerPlz", System.Type.GetType("System.String"))
                .Add("HaendlerOrt", System.Type.GetType("System.String"))

                .Add("Wunschkennzeichen", System.Type.GetType("System.String"))
                .Add("Status", System.Type.GetType("System.String")) 'Für Statusmeldung nach Speichern
                .Add("FUnterlagen", System.Type.GetType("System.String")) 'Für Statusmeldung Zulassungsunterlagen vollständig
                .Add("Zulstelle", System.Type.GetType("System.String"))
                .Add("Versandadresse", System.Type.GetType("System.String"))    'Für Abweichende Versandadresse
                .Add("Zulassungsstelle", System.Type.GetType("System.String"))
                .Add("Evbnummer", System.Type.GetType("System.String"))
                .Add("EVB_VON", System.Type.GetType("System.String"))
                .Add("EVB_BIS", System.Type.GetType("System.String"))
                .Add("EVB_TXT", System.Type.GetType("System.String"))
            End With
        End If
    End Sub

    Private Sub getZulStelle(ByVal strAppId As String, ByVal strSessionId As String, ByVal plz As String, ByVal ort As String, ByRef status As String)

        m_strAppID = strAppId
        m_strSessionID = SessionID

        ClearError()

        Dim table As DataTable
        'Dim tblOrte As DataTable

        Try
            S.AP.InitExecute("Z_Get_Zulst_By_Plz", "I_PLZ,I_ORT", plz, ort)

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", m_objApp, m_objUser, Page)

            'myProxy.setImportParameter("I_PLZ", plz)
            'myProxy.setImportParameter("I_ORT", ort)


            'myProxy.callBapi()

            table = S.AP.GetExportTable("T_ZULST") 'myProxy.getExportTable("T_ZULST")
            'tblOrte = myProxy.getExportTable("T_ORTE")

            If (table.Rows.Count > 1) Then
                'Mehr als ein Eintrag gefunden! Bei Arval möglich?? 'TODO
                status = "PLZ nicht eindeutig. Mehrere Treffer gefunden."
            End If

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
            End If
            m_kbanr = table.Rows(0)("KBANR").ToString
            m_zulkz = table.Rows(0)("ZKFZKZ").ToString
            m_afnam = table.Rows(0)("AFNAM").ToString

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace(" Execution failed", "")
                Case "ERR_INV_PLZ"
                    RaiseError("-1118", "Ungültige Postleitzahl.")
                Case Else
                    RaiseError("-9999", "Unbekannter Fehler.")
            End Select
            status = m_strMessage
        End Try
    End Sub

    Public Function giveResultStructure(ByVal strAppId As String, ByVal strSessionId As String) As DataTable

        m_strAppID = strAppId
        m_strSessionID = strSessionId

        Dim tbl As DataTable

        Try
            S.AP.Init("Z_M_Unangefordert_Arval")
            tbl = S.AP.GetImportTable("GT_WEB")
            tbl.Columns.Add("STATUS", GetType(System.String))
            tbl.Columns.Add("ART", GetType(System.String))
        Catch ex As Exception
            RaiseError("-9999", "Importtabelle konnte nicht erstellt werden!")
        End Try


        'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Unangefordert_Arval", m_objApp, m_objUser, page)
        'Dim tbl As DataTable = myProxy.getImportTable("GT_WEB")
        'tbl.Columns.Add("STATUS", GetType(System.String))
        'tbl.Columns.Add("ART", GetType(System.String))
        Return tbl

    End Function

    Public Sub GiveCars(ByVal strAppId As String, ByVal strSessionId As String)

        m_strAppID = strAppId
        m_strSessionID = strSessionId

        Dim tableHide As New DataTable()
        Dim status As String = ""
        Dim row As DataRow
        Dim rowResult As DataRow()

        ClearError()

        readAllAuthorizationSets(tableHide, status) 'Daten, die zur Autorisierung einstehen

        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1


            Try
                S.AP.InitExecute("Z_M_Unangefordert_Arval", "I_KONZS,I_LICENSE_NUM,I_LIZNR,I_VKORG", m_strKUNNR, m_strSucheFgstNr, m_strSucheLvNr, "1510")

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Unangefordert_Arval", m_objApp, m_objUser, Page)

                'myProxy.setImportParameter("I_KONZS", m_strKUNNR)
                'myProxy.setImportParameter("I_LICENSE_NUM", m_strSucheFgstNr)
                'myProxy.setImportParameter("I_LIZNR", m_strSucheLvNr)
                'myProxy.setImportParameter("I_VKORG", "1510")

                'myProxy.callBapi()

                'm_tableGrund = New DataTable()
                m_tableGrund = S.AP.GetExportTable("GT_GRU") 'myProxy.getExportTable("GT_GRU")
                m_tblResult = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

                m_tblResult.Columns.Add("STATUS")

                For Each row In m_tblResult.Rows
                    row("STATUS") = ""
                    If CStr(row("ZZSTATUS_IABG")) = "X" Then
                        row("STATUS") = "In Abmeldung"
                    End If
                    If CStr(row("ZZSTATUS_ABG")) = "X" Then
                        row("STATUS") = "Abgemeldet"
                    End If
                    If CStr(row("ZZSTATUS_ZUG")) = "X" Then
                        row("STATUS") = "Zugelassen"
                    End If
                    rowResult = tableHide.Select("EQUIPMENT = '" & row("EQUNR").ToString & "'")
                    If Not (rowResult.Length = 0) Then
                        row("STATUS") = row("STATUS").ToString & "(*)"
                    End If
                Next

                If (m_tblResult Is Nothing) OrElse (m_tblResult.Rows.Count = 0) Then
                    RaiseError("-3331", "Keine Daten gefunden.")
                End If

                WriteLogEntry(True, "HAENDLER=" & m_strHaendlernummer & ", LVNr.=" & m_strSucheLvNr & ", KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace(" Execution failed", "")
                    Case "NO_DATA"
                        RaiseError("-3331", "Keine Daten gefunden.")
                    Case "NO_HAENDLER"
                        RaiseError("-3332", "Keine oder falsche Haendlernummer.")
                    Case Else
                        RaiseError("-9999", ex.Message)
                End Select
                WriteLogEntry(False, "HAENDLER=" & m_strHaendlernummer & ", LVNr.=" & m_strSucheLvNr & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub getZulassungsdienste(ByVal strAppID As String, ByVal strSessionID As String, ByRef tblSTVA As DataTable, ByRef status As String)

        ClearError()

        Try
            S.AP.InitExecute("Z_Get_Zulst_By_Plz", "I_PLZ,I_ORT", "", "")
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_PLZ", "")
            'myProxy.setImportParameter("I_ORT", "")

            'myProxy.callBapi()

            tblSTVA = S.AP.GetExportTable("T_ZULST") 'myProxy.getExportTable("T_ZULST")

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace(" Execution failed", "")
                Case "ERR_INV_PLZ"
                    RaiseError("-1118", "Ungültige Postleitzahl.")
                Case Else
                    RaiseError("-9999", "Unbekannter Fehler.")
            End Select
            status = m_strMessage
        End Try
    End Sub

    Private Sub AnfordernSAP()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            ClearError()

            Try
                m_strZielLand = RemoveSingleSpace(m_strZielLand)
                If m_strZielLand = "" Then
                    m_strZielLand = "DE"
                End If

                S.AP.Init("Z_M_Briefanforderung_Arval")

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Briefanforderung_Arval", m_objApp, m_objUser, Page)

                S.AP.SetImportParameter("I_KUNNR_AG", m_strKUNNR) 'myProxy.setImportParameter("I_KUNNR_AG", m_strKUNNR)
                S.AP.SetImportParameter("I_KUNNR_ZF", m_strHaendlernummer) 'myProxy.setImportParameter("I_KUNNR_ZF", m_strHaendlernummer)
                S.AP.SetImportParameter("I_KUNNR_ZH", "") 'myProxy.setImportParameter("I_KUNNR_ZH", "")
                S.AP.SetImportParameter("I_KUNNR_ZE", m_versandadr) 'myProxy.setImportParameter("I_KUNNR_ZE", m_versandadr)
                S.AP.SetImportParameter("I_KUNNR_ZS", "") 'myProxy.setImportParameter("I_KUNNR_ZS", "")
                S.AP.SetImportParameter("I_ABCKZ", m_abckz) 'myProxy.setImportParameter("I_ABCKZ", m_abckz)
                S.AP.SetImportParameter("I_EQUNR", m_equ) 'myProxy.setImportParameter("I_EQUNR", m_equ)
                S.AP.SetImportParameter("I_ERNAM", Left(m_objUser.UserName, 12)) 'myProxy.setImportParameter("I_ERNAM", Left(m_objUser.UserName, 12))
                S.AP.SetImportParameter("I_CHASSIS_NUM", m_strSucheFgstNr) 'myProxy.setImportParameter("I_CHASSIS_NUM", m_strSucheFgstNr)
                S.AP.SetImportParameter("I_LICENSE_NUM", m_kennz) 'myProxy.setImportParameter("I_LICENSE_NUM", m_kennz)
                S.AP.SetImportParameter("I_TIDNR", m_tidnr) 'myProxy.setImportParameter("I_TIDNR", m_tidnr)
                S.AP.SetImportParameter("I_LIZNR", m_liznr) 'myProxy.setImportParameter("I_LIZNR", m_liznr)
                S.AP.SetImportParameter("I_MATNR", Right("000000000000000000" & m_material, 18)) 'myProxy.setImportParameter("I_MATNR", Right("000000000000000000" & m_material, 18))
                S.AP.SetImportParameter("I_ZZVGRUND", m_versgrund) 'myProxy.setImportParameter("I_ZZVGRUND", m_versgrund)
                S.AP.SetImportParameter("I_NAME1", m_strZielFirma) 'myProxy.setImportParameter("I_NAME1", m_strZielFirma)
                S.AP.SetImportParameter("I_NAME2", Replace(m_strZielFirma2, "&nbsp;", "")) 'myProxy.setImportParameter("I_NAME2", Replace(m_strZielFirma2, "&nbsp;", ""))
                S.AP.SetImportParameter("I_PSTLZ", m_strZielPLZ) 'myProxy.setImportParameter("I_PSTLZ", m_strZielPLZ)
                S.AP.SetImportParameter("I_ORT01", m_strZielOrt) 'myProxy.setImportParameter("I_ORT01", m_strZielOrt)
                S.AP.SetImportParameter("I_STR01", m_strZielStrasse) 'myProxy.setImportParameter("I_STR01", m_strZielStrasse)
                S.AP.SetImportParameter("I_HOUSE", m_strZielHNr) 'myProxy.setImportParameter("I_HOUSE", m_strZielHNr)
                S.AP.SetImportParameter("I_LAND1", m_strZielLand) 'myProxy.setImportParameter("I_LAND1", m_strZielLand)
                S.AP.SetImportParameter("I_ZZBETREFF", m_strBemerkung) 'myProxy.setImportParameter("I_ZZBETREFF", m_strBemerkung)

                S.AP.Execute()
                'myProxy.callBapi()

                strAuftragsnummer = S.AP.GetExportParameter("E_VBELN") 'myProxy.getExportParameter("E_VBELN")
                strAuftragsnummer = strAuftragsnummer.TrimStart("0"c)
                strAuftragsstatus = "Vorgang OK"
                If strAuftragsnummer.Length = 0 Then
                    RaiseError("-2100", "Ihre Anforderung konnte im System nicht erstellt werden.")
                    strAuftragsstatus = "Fehler: Anforderung konnte nicht erstellt werden."
                End If
            Catch ex As Exception
                strAuftragsstatus = "Fehler"
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace(" Execution failed", "")
                    Case "ZCREDITCONTROL_ENTRY_LOCKED"
                        RaiseError("-1111", "System ausgelastet. Bitte clicken Sie erneut auf ""Absenden"".")
                    Case "NO_UPDATE_EQUI"
                        RaiseError("-1112", "Fehler bei der Datenspeicherung (EQUI-UPDATE)")
                    Case "NO_AUFTRAG"
                        RaiseError("-1113", "Kein Auftrag angelegt")
                    Case "NO_ZDADVERSAND"
                        RaiseError("-1114", "Keine Einträge für die Versanddatei erstellt")
                    Case "NO_MODIFY_ILOA"
                        RaiseError("-1115", "ILOA-MODIFY-Fehler")
                    Case "NO_BRIEFANFORDERUNG"
                        RaiseError("-1116", "Brief bereits angefordert")
                    Case "NO_EQUZ"
                        RaiseError("-1117", "Kein Brief vorhanden (EQUZ)")
                    Case "NO_ILOA"
                        RaiseError("-1118", "Kein Brief vorhanden (ILOA)")
                    Case Else
                        RaiseError("-9999", ex.Message)
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Private Sub AddResultTableHeader()
        If (m_tblResult Is Nothing) Then
            m_tblResult = New DataTable()
        End If
        With m_tblResult.Columns
            .Add("id")
            .Add("Erstellt")
            .Add("Benutzer")
            .Add("Equipment")
            .Add("Fahrgestellnr")
            .Add("Versandadresse")
            .Add("VersandadresseName1")
            .Add("VersandadresseName2")
            .Add("VersandadresseStr")
            .Add("VersandadresseNr")
            .Add("VersandadressePlz")
            .Add("VersandadresseOrt")
            .Add("VersandadresseLand")
            .Add("Lvnr")
            .Add("Haendlernummer")
            .Add("Versandart")
            .Add("Kennzeichen")
            .Add("TIDNr")
            .Add("LIZNr")
            .Add("Materialnummer")
            .Add("VersandartShow")
            .Add("Status")
        End With

    End Sub

    Private Sub AddResultTableRow(ByVal id As String, ByVal tstamp As String, ByVal equi As String, ByVal user As String, ByVal objData As ArrayList)
        Dim row As DataRow

        row = m_tblResult.NewRow
        row("id") = id
        row("Erstellt") = tstamp
        row("Benutzer") = user
        row("Equipment") = equi
        If objData(12) Is Nothing Then
            row("Lvnr") = ""
        Else
            row("Lvnr") = objData(12)
        End If
        If objData(10) Is Nothing Then
            row("Fahrgestellnr") = ""
        Else
            row("Fahrgestellnr") = objData(11)
        End If
        If objData(3) Is Nothing Then
            row("VersandadresseName1") = ""
            row("Versandadresse") = ""
        Else
            row("VersandadresseName1") = objData(3)
            row("Versandadresse") = objData(3).ToString
        End If
        If objData(4) Is Nothing Then
            row("VersandadresseName2") = ""
        Else
            row("VersandadresseName2") = objData(4)
        End If
        If objData(5) Is Nothing Then
            row("VersandadresseStr") = ""
            row("Versandadresse") = CStr(row("Versandadresse")) & "<br>" & " "
        Else
            row("VersandadresseStr") = objData(5)
            row("Versandadresse") = CStr(row("Versandadresse")) & "<br>" & objData(5).ToString
        End If
        If objData(6) Is Nothing Then
            row("VersandadresseNr") = ""
            row("Versandadresse") = CStr(row("Versandadresse")) & "&nbsp;" & " "
        Else
            row("VersandadresseNr") = objData(6)
            row("Versandadresse") = CStr(row("Versandadresse")) & "&nbsp;" & objData(6).ToString
        End If
        If objData(7) Is Nothing Then
            row("VersandadressePlz") = ""
            row("Versandadresse") = CStr(row("Versandadresse")) & ", " & " "
        Else
            row("VersandadressePlz") = objData(7)
            row("Versandadresse") = CStr(row("Versandadresse")) & ", " & objData(7).ToString
        End If
        If objData(8) Is Nothing Then
            row("VersandadresseOrt") = ""
            row("Versandadresse") = CStr(row("Versandadresse")) & "&nbsp;" & " "
        Else
            row("VersandadresseOrt") = objData(8)
            row("Versandadresse") = CStr(row("Versandadresse")) & "&nbsp;" & objData(8).ToString
        End If

        If objData(9) Is Nothing Then
            row("VersandadresseLand") = ""
            row("Versandadresse") = CStr(row("Versandadresse"))
        Else
            row("VersandadresseLand") = objData(9)
            row("Versandadresse") = CStr(row("Versandadresse")) & ", " & objData(9).ToString
        End If


        'row("Versandadresse") = objData(3).ToString & "<br>" & objData(5).ToString & "&nbsp;" & objData(6).ToString & "," & objData(7).ToString & "&nbsp;" & objData(8).ToString
        If objData(0) Is Nothing Then
            row("Haendlernummer") = ""
        Else
            row("Haendlernummer") = objData(0)
        End If
        If objData(23) Is Nothing Then
            row("Kennzeichen") = ""
        Else
            row("Kennzeichen") = objData(23)
        End If
        If objData(24) Is Nothing Then
            row("TIDNr") = ""
        Else
            row("TIDNr") = objData(24)
        End If
        row("LIZNr") = objData(25)
        If objData(19) Is Nothing Then
            row("Materialnummer") = "1391"
        Else
            row("Materialnummer") = objData(19)
        End If
        Select Case CStr(row("Materialnummer"))
            Case "1391"
                row("VersandartShow") = "innerhalb von 24 bis 48 h"
            Case "1385"
                row("VersandartShow") = "vor 9:00 Uhr"
            Case "1389"
                row("VersandartShow") = "vor 10:00 Uhr"
            Case "1390"
                row("VersandartShow") = "vor 12:00 Uhr"
        End Select
        row("Versandart") = "2"                 'Endgültig
        m_tblResult.Rows.Add(row)
        m_tblResult.AcceptChanges()
    End Sub

    Public Sub getAutorizationData(ByRef status As String)
        Dim formatter As System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim cn As New SqlClient.SqlConnection
        Dim cmdOutPut As SqlClient.SqlCommand
        Dim objData As ArrayList
        Dim ms As MemoryStream
        Dim drAppData As SqlClient.SqlDataReader
        Dim intI As Int32 = 4

        addResultTableHeader()

        Try
            cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            cn.Open()

            cmdOutPut = New SqlClient.SqlCommand()
            cmdOutPut.Connection = cn
            cmdOutPut.CommandText = "SELECT * FROM AuthorizationARVAL"
            drAppData = cmdOutPut.ExecuteReader

            While drAppData.Read()
                If Not drAppData Is Nothing Then
                    If Not TypeOf drAppData(intI) Is System.DBNull Then
                        ' 1. Daten als bytearray aus der DB lesen:
                        Dim bytData(CInt(drAppData.GetBytes(intI, 0, Nothing, 0, Integer.MaxValue - 1) - 1)) As Byte
                        drAppData.GetBytes(intI, 0, bytData, 0, bytData.Length)
                        ' Dataset über einen Memory Stream aus dem ByteArray erzeugen:
                        Dim stmAppData As MemoryStream
                        stmAppData = New MemoryStream(bytData)
                        ms = stmAppData
                        formatter = New BinaryFormatter()

                        objData = DirectCast(formatter.Deserialize(ms), ArrayList)
                        addResultTableRow(CType(drAppData("id"), String), CType(drAppData("tstamp"), String), CType(drAppData("equipment"), String),
                                          CType(drAppData("username"), String), objData)
                    End If
                End If
            End While
        Catch ex As Exception
            Throw ex
        Finally
            cn.Close()
            cn.Dispose()
        End Try
    End Sub

    Private Sub writedb(ByVal username As String, ByVal equipment As String, ByVal objectData As MemoryStream, ByRef status As String)
        Dim connection As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim para As SqlClient.SqlParameter

        Dim command As New SqlClient.SqlCommand()
        Dim sqlInsert As String
        Dim b As Byte()
       
        Try
            sqlInsert = "INSERT INTO AuthorizationARVAL (username,equipment, data) VALUES (@user,@equi,@data)"

            With command
                .Connection = connection
                .CommandType = CommandType.Text
                .CommandText = sqlInsert
                .Parameters.Clear()
            End With

            b = objectData.ToArray
            para = New SqlClient.SqlParameter("@data", SqlDbType.Image, b.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, b)
            With command.Parameters
                .AddWithValue("@user", username)
                .AddWithValue("@equi", equipment)
                .Add(para)
            End With
            connection.Open()
            command.ExecuteNonQuery()
            status = String.Empty
        Catch ex As Exception
            status = ex.Message
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Sub

    Private Sub anfordernSQL()
        Dim formatter As System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim ms As MemoryStream
        dataArray = New ArrayList()

        With dataArray
            .Add(m_strHaendlernummer)       '0
            .Add(m_strHalterNummer)         '1
            .Add(m_strStandortNummer)       '2
            .Add(m_strZielFirma)            '3
            .Add(m_strZielFirma2)           '4
            .Add(m_strZielStrasse)          '5
            .Add(m_strZielHNr)              '6
            .Add(m_strZielPLZ)              '7
            .Add(m_strZielOrt)              '8
            .Add(m_strZielLand)
            .Add(m_blnArvalZulassung)       '9
            .Add(m_strSucheFgstNr)          '10
            .Add(m_liznr)                   '11
            .Add(m_kbanr)                   '12
            .Add(m_zulkz)                   '13
            .Add(m_Fahrzeuge)               '14
            .Add(m_tableGrund)              '15
            .Add(m_versandadr)              '16
            .Add(m_versandadrtext)          '17
            .Add(m_material)                '18
            .Add(m_schein)                  '19
            .Add(m_abckz)                   '20
            .Add(m_equ)                     '21
            .Add(m_kennz)                   '22
            .Add(m_tidnr)                   '23
            .Add(m_liznr)                   '24
            .Add(m_versgrund)               '25
            .Add(strAuftragsstatus)         '26
            .Add(strAuftragsnummer)         '27
        End With

        ms = New MemoryStream()
        formatter = New BinaryFormatter()
        formatter.Serialize(ms, dataArray)

        writedb(m_objUser.UserName, m_equ, ms, strAuftragsstatus)
        If (strAuftragsstatus = String.Empty) Then
            strAuftragsstatus = "Auftrag zur Freigabe gespeichert."
            strAuftragsnummer = "OK"
        Else
            strAuftragsnummer = ""
        End If
    End Sub

    Public Sub ClearDb(ByVal id As Int32, ByRef status As String)
        Dim connection As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As New SqlClient.SqlCommand()
        Dim sqlInsert As String
        
        Try
            sqlInsert = "DELETE FROM AuthorizationARVAL WHERE id = @id"

            With command
                .Connection = connection
                .CommandType = CommandType.Text
                .CommandText = sqlInsert
                .Parameters.Clear()
            End With

            With command.Parameters
                .AddWithValue("@id", id)
            End With
            connection.Open()
            command.ExecuteScalar()
            status = String.Empty
        Catch ex As Exception
            status = ex.Message
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Sub

    Private Sub readAllAuthorizationSets(ByRef resultTable As DataTable, ByRef status As String)
        Dim connection As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim sqlInsert As String

        Try
            sqlInsert = "SELECT * FROM AuthorizationARVAL"

            With command
                .Connection = connection
                .CommandType = CommandType.Text
                .CommandText = sqlInsert
                .Parameters.Clear()
            End With

            connection.Open()
            adapter.SelectCommand = command
            adapter.Fill(resultTable)

            status = String.Empty
        Catch ex As Exception
            status = ex.Message
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Sub

    Public Sub Anfordern(ByVal art As String)
        If (art = "TEMP") Then
            AnfordernSAP()
        Else
            anfordernSQL()
        End If
    End Sub

    Private Sub getLaender()

        Dim intId As Int32 = -1
        ClearError()
        
        Try
           
            S.AP.Init("Z_M_Land_Plz_001")

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            intId = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Land_Plz_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

            S.AP.Execute()

            m_objLogApp.WriteEndDataAccessSAP(intId, True)

            m_tblLaender = S.AP.GetExportTable("GT_WEB")

            m_tblLaender.Columns.Add("Beschreibung", System.Type.GetType("System.String"))
            m_tblLaender.Columns.Add("FullDesc", System.Type.GetType("System.String"))

            Dim rowTemp As DataRow
            For Each rowTemp In m_tblLaender.Rows
                If CInt(rowTemp("LNPLZ")) > 0 Then
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx")) & " (" & CStr(CInt(rowTemp("LNPLZ"))) & ")"
                Else
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx"))
                End If
                rowTemp("FullDesc") = CStr(rowTemp("Land1")) & " " & CStr(rowTemp("Beschreibung"))
            Next
        Catch ex As Exception
            m_objLogApp.WriteEndDataAccessSAP(intId, False, ex.Message)
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace(" Execution failed", "")
                Case "ERR_INV_PLZ"
                    RaiseError("-1118", "Ungültige Postleitzahl.")
                Case Else
                    RaiseError("-9999", "Unbekannter Fehler.")
            End Select
        End Try

    End Sub

    Private Function RemoveSingleSpace(ByVal strIn As String) As String
        Dim strReturn As String = ""
        Try
            Dim strOut As String = strIn.Trim(" "c)
            If strOut = "&nbsp;" Then
                strOut = ""
            End If
            strReturn = strOut
        Catch
        End Try
        Return strReturn
    End Function

#End Region

End Class

' ************************************************
' $History: Arval_1.vb $
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 30.06.09   Time: 11:28
' Updated in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 29.04.09   Time: 15:54
' Updated in $/CKAG/Applications/apparval/Lib
' Warnungen
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 11.03.09   Time: 11:36
' Updated in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 21.10.08   Time: 9:01
' Updated in $/CKAG/Applications/apparval/Lib
' Bugfix leeres Datum abgefangen!OR
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 25.08.08   Time: 17:11
' Updated in $/CKAG/Applications/apparval/Lib
' ITA: 1859
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 25.08.08   Time: 15:13
' Updated in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 1.07.08    Time: 17:07
' Updated in $/CKAG/Applications/apparval/Lib
' Bugfix
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 6.12.07    Time: 13:29
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' ITA 1440
' 
' *****************  Version 11  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 10  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' 
' ************************************************
