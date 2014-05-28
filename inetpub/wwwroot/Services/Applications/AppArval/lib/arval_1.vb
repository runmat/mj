Option Explicit On
Option Strict On
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Common

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
    Private m_strSucheLvNr As String
    '
    Private m_kbanr As String
    Private m_zulkz As String
    '§§§ JVE 22.12.2005 neues Feld
    Private m_afnam As String
    '
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
        'If objUser.Reference = String.Empty Then
        '    m_blnArvalZulassung = True
        'End If

        getLaender()

    End Sub

    Public Sub checkZulassungsdokumente(ByRef table As DataTable, ByVal HalterName As String, ByVal HalterPlz As String)
        Dim intID As Int32 = -1

        Try
            Dim proxy = DynSapProxy.getProxy("Z_M_Zuldokumente_Arval", m_objApp, m_objUser, GetCurrentPage())

            proxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            proxy.setImportParameter("I_UNVOLLSTAENDIG", "X")
            proxy.setImportParameter("I_ZHNAME", HalterName)
            proxy.setImportParameter("I_ZHPLZ", HalterPlz)

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Zuldokumente_Arval", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

            proxy.callBapi()

            table = proxy.getExportTable("T_ZULDOKUMENTE")

            For Each row As DataRow In table.Rows
                If IsDate(row("Evb_bis").ToString) AndAlso row("Evb_bis").ToString <> "0000000000" Then
                    row("Evb_bis") = MakeDateStandard(row("Evb_bis").ToString)
                Else
                    row("Evb_bis") = ""
                End If
                If IsDate(row("Evb_von").ToString) AndAlso row("Evb_von").ToString <> "0000000000" Then
                    row("Evb_von") = MakeDateStandard(row("Evb_von").ToString)
                Else
                    row("Evb_von") = ""
                End If
            Next

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = "ERR_ZULDOKU"

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If

            m_blnGestartet = False
        End Try

    End Sub

    Public Overrides Sub Show()
        m_strClassAndMethod = "Arval_1.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                NewResultTable()
                m_intStatus = 0
                m_strMessage = ""
                Dim haendler As String

                If m_strHaendlernummer <> String.Empty Then
                    haendler = Right("0000000000" & m_strHaendlernummer, 10)    'Händler: nur eigene KFZ!
                Else
                    haendler = String.Empty                                     'Mitarbeiter: alle KFZ!
                End If


                Dim proxy = DynSapProxy.getProxy("Z_M_ZULF_FZGE_ARVAL", m_objApp, m_objUser, GetCurrentPage())
                proxy.setImportParameter("I_AG", Customer)
                proxy.setImportParameter("I_ZF", haendler)
                proxy.setImportParameter("I_LVNR", m_strSucheLvNr)
                proxy.setImportParameter("I_FZGID", m_strSucheFgstNr)

                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Zulf_Fzge_Arval", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                proxy.callBapi()

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If
                Dim tblTemp2 As DataTable = proxy.getExportTable("T_FZGE")

                If tblTemp2.Rows.Count > 0 Then
                    Dim rowNew As DataRow
                    Dim TempRow As DataRow
                    For Each TempRow In tblTemp2.Rows
                        rowNew = m_tblResult.NewRow
                        rowNew("Ausgewaehlt") = False
                        rowNew("Kennzeichenserie") = False
                        rowNew("Vorreserviert") = False
                        rowNew("HaendlerNummer") = TempRow("Za_Kunnr").ToString
                        rowNew("LeasingNummer") = TempRow("Liznr").ToString
                        rowNew("FhgstNummer") = TempRow("Chassis_Num").ToString
                        rowNew("EquipmentNummer") = TempRow("Equnr").ToString
                        rowNew("DatumBriefeingang") = MakeDateStandard(TempRow("Erdat").ToString)
                        rowNew("Halter") = TempRow("Zh_Name1").ToString.Trim & "<br>" & TempRow("Zh_Plz").ToString.Trim & "&nbsp;" & TempRow("Zh_Ort").ToString.Trim
                        rowNew("HalterKunnr") = TempRow("Zh_Kunnr").ToString
                        rowNew("HalterName1") = TempRow("Zh_Name1").ToString
                        rowNew("HalterName2") = TempRow("Zh_Name2").ToString
                        rowNew("HalterStr") = TempRow("Zh_Strasse").ToString
                        rowNew("HalterNr") = TempRow("Zh_Hnr").ToString
                        rowNew("HalterPlz") = TempRow("Zh_Plz").ToString
                        rowNew("HalterOrt") = TempRow("Zh_Ort").ToString
                        'ZulStelle holen (Halter)
                        m_zulkz = ""
                        Dim strStatus As String = String.Empty
                        If TempRow("Zh_Plz").ToString.Length > 0 Or TempRow("Zh_Ort").ToString.Length > 0 Then
                            getZulStelle(TempRow("Zh_Plz").ToString, TempRow("Zh_Ort").ToString, strStatus)
                        End If
                        rowNew("Zulstelle") = m_zulkz
                        rowNew("Standort") = TempRow("Za_Name1").ToString & "<br>" & TempRow("Za_Plz").ToString & "&nbsp;" & TempRow("Za_Ort").ToString
                        rowNew("StandortKunnr") = TempRow("Za_Kunnr").ToString
                        rowNew("StandortName1") = TempRow("Za_Name1").ToString
                        rowNew("StandortName2") = TempRow("Za_Name2").ToString
                        rowNew("StandortStr") = TempRow("Za_Strasse").ToString
                        rowNew("StandortNr") = TempRow("Za_Hnr").ToString
                        rowNew("StandortPlz") = TempRow("Za_Plz").ToString
                        rowNew("StandortOrt") = TempRow("Za_Ort").ToString

                        rowNew("Haendler") = TempRow("Zf_Name1").ToString & ", " & TempRow("Zf_Plz").ToString & "&nbsp;" & TempRow("Zf_Ort").ToString
                        rowNew("HaendlerKunnr") = TempRow("Zf_Kunnr").ToString
                        rowNew("HaendlerName1") = TempRow("Zf_Name1").ToString
                        rowNew("HaendlerName2") = TempRow("Zf_Name2").ToString
                        rowNew("HaendlerStr") = TempRow("Zf_Strasse").ToString
                        rowNew("HaendlerNr") = TempRow("Zf_Hnr").ToString
                        rowNew("HaendlerPlz") = TempRow("Zf_Plz").ToString
                        rowNew("HaendlerOrt") = TempRow("Zf_Ort").ToString
                        rowNew("Zulassungsstelle") = m_afnam
                        rowNew("Evbnummer") = TempRow("ZEVBNR").ToString
                        rowNew("EVB_TXT") = TempRow("ZEVBTXT").ToString
                        rowNew("DatumZulassung") = SuggestionDay(TempRow("Chassis_Num").ToString, m_afnam)

                        If (ArvalZulassung = False) Then        'Keine Arval-Zulassung
                            m_tblResult.Rows.Add(rowNew)
                        Else
                            If (TempRow("Zh_Kunnr").ToString) = Right("0000000000" & m_objUser.KUNNR, 10) Then       'Arval-Zulassung
                                m_tblResult.Rows.Add(rowNew)
                            End If
                        End If
                    Next
                Else
                    m_intStatus = -2202
                    m_strMessage = "Fehler: Keine Kontingentinformationen vorhanden."
                End If

                Dim col As DataColumn
                For Each col In m_tblResult.Columns
                    col.ReadOnly = False
                Next
                If (ArvalZulassung And m_tblResult.Rows.Count = 0) Then
                    m_intStatus = -3331
                    m_strMessage = "Keine Daten gefunden."
                End If
                WriteLogEntry(True, "KUNNR=" & Right(m_strCustomer, 5), m_tblResult)
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "ERR_NO_DATA"      'Änderung
                        m_intStatus = -3331
                        m_strMessage = "Keine Daten gefunden."
                    Case "ERR_INV_AG"
                        m_intStatus = -3332
                        m_strMessage = "Allgemeiner Lesefehler."
                    Case "ERR_INV_ZF"
                        m_intStatus = -3333
                        m_strMessage = "Ungültiger Händler """ & m_strHaendlernummer & """."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
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
    Public Overrides Sub Change()
        'Fahrzeuge zulassen
        Dim counter As Integer = 0
        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1
            Try
                Dim proxy = DynSapProxy.getProxy("Z_M_WEB_ZULASSUNG_ARVAL", m_objApp, m_objUser, GetCurrentPage())

                Dim partners = proxy.getImportTable("T_PARTNERS")

                m_intStatus = 0
                m_strMessage = "Vorgang OK"
                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Web_Zulassung_Arval", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                m_strMessage = "Vorgang OK"

                'Halter, Standort, Schein & Schilder, Brief. Pro Zulassung je ein Satz!

                '###Halter
                Dim row = partners.NewRow()
                row("Kunnr") = Right("0000000000" & rowToChange("HalterKunnr").ToString, 10)
                row("Parvw") = "ZH"
                If rowToChange("HalterKunnr").ToString = String.Empty Then
                    row("Name1") = rowToChange("HalterName1").ToString
                    row("Name2") = rowToChange("HalterName2").ToString
                    row("Street") = rowToChange("HalterStr").ToString
                    row("House_Num1") = rowToChange("HalterNr").ToString
                    row("Post_Code1") = rowToChange("HalterPlz").ToString
                    row("City1") = rowToChange("HalterOrt").ToString
                    row("State") = "DE"
                End If
                partners.Rows.Add(row) 'Zeile hinzufügen

                '###Standort
                row = partners.NewRow()

                If (rowToChange("StandortKunnr").ToString <> String.Empty) Then
                    row("Kunnr") = Right("0000000000" & rowToChange("StandortKunnr").ToString, 10)
                Else
                    row("Kunnr") = ""
                End If
                row("Parvw") = "ZA"
                If rowToChange("StandortKunnr").ToString = String.Empty Then
                    row("Name1") = rowToChange("StandortName1").ToString
                    row("Name2") = rowToChange("StandortName2").ToString
                    row("Street") = rowToChange("StandortStr").ToString
                    row("House_Num1") = rowToChange("StandortNr").ToString
                    row("Post_Code1") = rowToChange("StandortPlz").ToString
                    row("City1") = rowToChange("StandortOrt").ToString
                    row("State") = "DE"
                End If
                If (rowToChange("StandortKunnr").ToString <> "") Then
                    partners.Rows.Add(row) 'Zeile hinzufügen
                End If

                '###Schein & Schilder 
                row = partners.NewRow()
                If rowToChange("HaendlerKunnr").ToString <> String.Empty Then
                    row("Kunnr") = Right("0000000000" & rowToChange("HaendlerKunnr").ToString, 10)
                End If
                If rowToChange("Versandadresse").ToString = "X" Then
                    row("Kunnr") = Right("0000000000" & "390051", 10)     'Kunnr für abweichende Adresse Schein & Schilder...
                    row("Name1") = rowToChange("HaendlerName1").ToString
                    row("Name2") = rowToChange("HaendlerName2").ToString
                    row("Street") = rowToChange("HaendlerStr").ToString
                    row("House_Num1") = rowToChange("HaendlerNr").ToString
                    row("Post_Code1") = Right("00000" & rowToChange("HaendlerPlz").ToString, 5)
                    row("City1") = rowToChange("HaendlerOrt").ToString
                    row("State") = "DE"
                End If
                row("Parvw") = "ZE"
                If (rowToChange("HaendlerKunnr").ToString <> "") Then
                    partners.Rows.Add(row) 'Zeile hinzufügen
                End If

                '###Briefempfänger (immer DAD)
                row = partners.NewRow()
                row("Kunnr") = "0000000001"
                row("Parvw") = "ZS"
                partners.Rows.Add(row) 'Zeile hinzufügen

                Dim tmpDate As DateTime
                Dim sEvbBis As String = ""
                If DateTime.TryParse(rowToChange("Evb_bis").ToString, tmpDate) Then
                    sEvbBis = tmpDate.ToShortDateString()
                End If
                Dim sEvbVon As String = ""
                If DateTime.TryParse(rowToChange("Evb_Von").ToString, tmpDate) Then
                    sEvbVon = tmpDate.ToShortDateString()
                End If
                Dim sZulDate As String = ""
                If DateTime.TryParse(rowToChange("DatumZulassung").ToString, tmpDate) Then
                    sZulDate = tmpDate.ToShortDateString()
                End If

                proxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                proxy.setImportParameter("I_EQUNR", rowToChange("EquipmentNummer").ToString)
                proxy.setImportParameter("I_ERNAM", m_objUser.UserName)
                proxy.setImportParameter("I_EVBBIS", sEvbBis)
                proxy.setImportParameter("I_EVBNR", rowToChange("Evbnummer").ToString)
                proxy.setImportParameter("I_EVBVON", sEvbVon)
                proxy.setImportParameter("I_KENNZ", rowToChange("Zulstelle").ToString & "-")
                proxy.setImportParameter("I_KUNNR", Right("0000000000" & rowToChange("HaendlerKunnr").ToString, 10))
                proxy.setImportParameter("I_VERS", m_versicherung)
                proxy.setImportParameter("I_WUK", rowToChange("Wunschkennzeichen").ToString)
                proxy.setImportParameter("I_ZULDAT", sZulDate)
                proxy.setImportParameter("I_ZULST", m_kbanr)

                proxy.callBapi()

                ' TODO: exportParameter "O_VBELN" verarbeiten?

                'Alles OK... 
                rowToChange("Status") = m_strMessage 'Meldung merken

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_AUFTRAG"
                        m_intStatus = -3331
                        m_strMessage = "Kein Auftrag angeleg."
                    Case "NO_ZDADVERSAND"
                        m_intStatus = -3332
                        m_strMessage = "Keine Einträge für die Versanddatei erstellt."
                    Case "ERR_INV_KUNNR"
                        m_intStatus = -3333
                        m_strMessage = "Unbekannte Kundennr."
                    Case "ERR_NO_ZULDAT"
                        m_intStatus = -3334
                        m_strMessage = "Kein Zulassungsdatum angegeben."
                    Case "ERR_INV_ZULDAT"
                        m_intStatus = -3335
                        m_strMessage = "Ungültiges Zulassungsdatum."
                    Case "ERR_NO_TRANSNAME"
                        m_intStatus = -3336
                        m_strMessage = "Kein Transaktionsname angegeben."
                    Case "ERR_INV_EQUNR"
                        m_intStatus = -3337
                        m_strMessage = "Ungültige Equipmentnr."
                    Case "ERR_NO_LIF"
                        m_intStatus = -3338
                        m_strMessage = "Kein Zulassungsdienst gefunden."
                    Case "ERR_INV_PARVW"
                        m_intStatus = -3339
                        m_strMessage = "Ungültige Partnerrolle."
                    Case "ERR_INV_ZF"
                        m_intStatus = -3340
                        m_strMessage = "Ungültige Kundennr. für Händler."
                    Case "ERR_INV_ZF_ABWADR"
                        m_intStatus = -3341
                        m_strMessage = "Fehlende Information zu abw. Adresse für Händler."
                    Case "ERR_INV_ZH"
                        m_intStatus = -3342
                        m_strMessage = "Ungültige Kundennr. für Halter."
                    Case "ERR_INV_ZH_ABWADR"
                        m_intStatus = -3343
                        m_strMessage = "Fehlende Information zu abw. Adresse für Halter."
                    Case "ERR_INV_ZS"
                        m_intStatus = -3344
                        m_strMessage = "Ungültige Kundennr. für Empf. Brief."
                    Case "ERR_INV_ZS_ABWADR"
                        m_intStatus = -3345
                        m_strMessage = "Fehlende Information zu abw. Adresse für Empf. Brief."
                    Case "ERR_INV_ZE"
                        m_intStatus = -3346
                        m_strMessage = "Ungültige Kundennr. für Empf. Schein und Schilder."
                    Case "ERR_INV_ZE_ABWADR"
                        m_intStatus = -3347
                        m_strMessage = "Fehlende Information zu abw. Adresse für Empf. Schein und Schilder."
                    Case "ERR_INV_ZA"
                        m_intStatus = -3348
                        m_strMessage = "Ungültige Kundennr. für Empf. Standort."
                    Case "ERR_INV_ZA_ABWADR"
                        m_intStatus = -3348
                        m_strMessage = "Fehlende Information zu abw. Adresse für Empf. Standort."
                    Case "ERR_SAVE"
                        m_intStatus = -3350
                        m_strMessage = "Fehler beim Speichern."
                    Case "ERR_NO_ZF"
                        m_intStatus = -3351
                        m_strMessage = "Kein Händler angegeben."
                    Case "ERR_NO_ZH"
                        m_intStatus = -3352
                        m_strMessage = "Kein Halter angegeben."
                    Case "ERR_NO_ZS"
                        m_intStatus = -3353
                        m_strMessage = "Kein Empfänger Brief angegeben."
                    Case "ERR_NO_ZE"
                        m_intStatus = -3354
                        m_strMessage = "Kein Empfänger Schein und Schilder angegeben."
                    Case "ERR_NO_EQUI"
                        m_intStatus = -3355
                        m_strMessage = "Kein Equipment zur Fahrgestellnr. gefunden."
                    Case "ERR_NO_RE"
                        m_intStatus = -3356
                        m_strMessage = "Kein Rechnungsempfänger angegeben."
                    Case "ERR_NO_RG"
                        m_intStatus = -3357
                        m_strMessage = "Kein Regulierer angegeben."
                    Case "ERR_INV_RE"
                        m_intStatus = -3358
                        m_strMessage = "Ungültiger Rechnungsempfänger."
                    Case "ERR_INV_RG"
                        m_intStatus = -3359
                        m_strMessage = "Ungültiger Regulierer."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                rowToChange("Status") = m_strMessage 'Meldung merken
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Function ErfasseBrief(ByVal strLeasingNummer As String, ByVal strFhgstNummer As String, ByVal strPZ As String, ByVal strHaendlerNummer As String) As Boolean
        'Dim rowNew As DataRow
        'Dim blnReturn As Boolean = False
        'Dim blnTest As Boolean = False

        ''Test, ob schon in Kollection
        'If m_tblResult Is Nothing Then
        '    'Noch leer, d.h. auf keinen Fall doppelt
        '    blnTest = True
        'Else
        '    Dim rowsTemp As DataRow() = m_tblResult.Select("FhgstNummer='" & strFhgstNummer & "'")
        '    If rowsTemp.GetLength(0) = 0 Then
        '        'Nicht gefunden, d.h. auch o.k.
        '        blnTest = True
        '    End If
        'End If

        'If blnTest Then
        '    '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
        '    ' Hier SAP-Aufruf
        '    '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
        '    If strLeasingNummer = "4711" Then
        '        ' Mißerfolg -> Fehlermeldung
        '        m_strMessage = "Fehler bei der Datenerfassung (" & "Bla" & ")"
        '    Else
        '        ' Erfolg -> Übernahme in Kollektion
        '        NewResultTable()
        '        rowNew = m_tblResult.NewRow
        '        rowNew("Ausgewaehlt") = True
        '        rowNew("Kennzeichenserie") = False
        '        rowNew("Vorreserviert") = False
        '        rowNew("HaendlerNummer") = strHaendlerNummer
        '        rowNew("LeasingNummer") = strLeasingNummer
        '        rowNew("FhgstNummer") = strFhgstNummer
        '        rowNew("DatumZulassung") = SuggestionDay()
        '        m_tblResult.Rows.Add(rowNew)
        '        blnReturn = True
        '    End If
        'Else
        '    m_strMessage = "Fehler: Doppelerfassung"
        'End If

        'Return blnReturn
    End Function

    Public Function SuggestionDay(ByVal fahrgestellnr As String, ByVal afnam As String) As DateTime
        Dim datTemp As DateTime = CType(Now.ToShortDateString, Date)
        Dim intAddDays As Int32 = 0
        '§§§ JVE 19.12.2005 <begin>
        'Do While datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday Or intAddDays < 3
        '    datTemp = datTemp.AddDays(1)
        '    If Not (datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday) Then
        '        intAddDays += 1
        '    End If
        'Loop


        'Nur 2 Tage in der Zukunft!
        'Do While datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday Or intAddDays < 2
        '    datTemp = datTemp.AddDays(1)
        '    If Not (datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday) Then
        '        intAddDays += 1
        '    End If
        'Loop
        '§§§ 02.02.06: Nur 1 Tag!
        Do While (datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday Or intAddDays < 1)
            datTemp = datTemp.AddDays(1)
            If Not (datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday) Then
                intAddDays += 1
            End If
        Loop
        '§§§ 02.02.06: Nur 1 Tag!
        '§§ JVE 22.12.2005 <begin>

        'If (afnam = ConfigurationManager.AppSettings("ArvalAuftragMunchenLand")) Then
        '    datTemp = datTemp.Subtract(New TimeSpan(1, 0, 0, 0))
        'End If
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
        '§§ JVE 22.12.2005 <end>

        'WoEnde prüfen!
        If datTemp.DayOfWeek = DayOfWeek.Saturday Then
            datTemp = datTemp.Add(New TimeSpan(2, 0, 0, 0))
        End If
        If datTemp.DayOfWeek = DayOfWeek.Sunday Then
            datTemp = datTemp.Add(New TimeSpan(1, 0, 0, 0))
        End If

        'Do While datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday Or intAddDays < 2
        '    datTemp = datTemp.AddDays(1)
        '    If Not (datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday) Then
        '        intAddDays += 1
        '    End If
        'Loop
        '§§§ JVE 19.12.2005 <end>
        Return datTemp
    End Function

    Public Sub ClearResultTable()
        m_tblResult = Nothing
    End Sub

    Private Sub NewResultTable()
        If m_tblResult Is Nothing Then
            m_tblResult = New DataTable()
            With m_tblResult.Columns
                .Add("Ausgewaehlt", GetType(Boolean))
                .Add("HaendlerNummer", GetType(String))
                .Add("EquipmentNummer", GetType(String))
                .Add("LeasingNummer", GetType(String))
                .Add("FhgstNummer", GetType(String))
                .Add("DatumBriefeingang", GetType(DateTime))
                .Add("DatumZulassung", GetType(DateTime))
                .Add("Wuk01_Buchstaben", GetType(String))
                .Add("Wuk01_Ziffern", GetType(String))
                .Add("Wuk02_Buchstaben", GetType(String))
                .Add("Wuk02_Ziffern", GetType(String))
                .Add("Wuk03_Buchstaben", GetType(String))
                .Add("Wuk03_Ziffern", GetType(String))
                .Add("Kennzeichenserie", GetType(Boolean))
                .Add("Vorreserviert", GetType(Boolean))
                .Add("Reservierungsdaten", GetType(String))
                .Add("Ergebnis", GetType(String))
                .Add("Halter", GetType(String)) 'Name+PLZ+Ort als Anzeige im WEB
                .Add("HalterKunnr", GetType(String))
                .Add("HalterName1", GetType(String))
                .Add("HalterName2", GetType(String))
                .Add("HalterStr", GetType(String))
                .Add("HalterNr", GetType(String))
                .Add("HalterPlz", GetType(String))
                .Add("HalterOrt", GetType(String))

                .Add("Standort", GetType(String)) ''Name+PLZ+Ort als Anzeige im WEB
                .Add("StandortKunnr", GetType(String))
                .Add("StandortName1", GetType(String))
                .Add("StandortName2", GetType(String))
                .Add("StandortStr", GetType(String))
                .Add("StandortNr", GetType(String))
                .Add("StandortPlz", GetType(String))
                .Add("StandortOrt", GetType(String))

                .Add("Haendler", GetType(String))
                .Add("HaendlerKunnr", GetType(String))
                .Add("HaendlerName1", GetType(String))
                .Add("HaendlerName2", GetType(String))
                .Add("HaendlerStr", GetType(String))
                .Add("HaendlerNr", GetType(String))
                .Add("HaendlerPlz", GetType(String))
                .Add("HaendlerOrt", GetType(String))

                .Add("Wunschkennzeichen", GetType(String))
                .Add("Status", GetType(String)) 'Für Statusmeldung nach Speichern
                '.Add("FUFlag", GetType(String)) 'Für Flag, ob Zulassungsunterlagen vollständig
                .Add("FUnterlagen", GetType(String)) 'Für Statusmeldung Zulassungsunterlagen vollständig
                .Add("Zulstelle", GetType(String))
                .Add("Versandadresse", GetType(String))    'Für Abweichende Versandadresse
                '§§§ JVE 23.12.2005 neue Spalte "AFNAM"
                .Add("Zulassungsstelle", GetType(String))
                .Add("Evbnummer", GetType(String))
                .Add("EVB_VON", GetType(String))
                .Add("EVB_BIS", GetType(String))
                .Add("EVB_TXT", GetType(String))
            End With
        End If
    End Sub

    Public Sub getZulStelle(ByVal plz As String, ByVal ort As String, ByRef status As String)
        Try
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            Dim proxy = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", m_objApp, m_objUser, GetCurrentPage())

            proxy.setImportParameter("I_PLZ", plz)
            proxy.setImportParameter("I_ORT", ort)

            proxy.callBapi()

            Dim objZul = proxy.getExportTable("T_ZULST")

            Dim table = objZul
            If (table.Rows.Count > 1) Then
                'Mehr als ein Eintrag gefunden! Bei Arval möglich?? 'TODO
                status = "PLZ nicht eindeutig. Mehrere Treffer gefunden."
            End If

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
            End If

            m_kbanr = table.Rows(0)("KBANR").ToString
            m_zulkz = table.Rows(0)("ZKFZKZ").ToString
            '§§§ JVE 22.12.2005 
            m_afnam = table.Rows(0)("AFNAM").ToString
            '
        Catch ex As Exception
            Select Case ex.Message
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
            status = m_strMessage
        Finally
            If m_intIDSAP > -1 Then
                m_objLogApp.LogStandardIdentity = m_intStandardLogID
                m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
            End If
        End Try
    End Sub

    'Public Function giveResultStructure() As DataTable
    '    Dim tbl As DataTable

    '    tbl = New SAPProxy_ARVAL.ZDAD_M_WEB_EQUIDATENTable().ToADODataTable  ' .ZDAD_M_WEB_EQUIDATENTable().ToADODataTable
    '    tbl.Columns.Add("STATUS", GetType(System.String))
    '    tbl.Columns.Add("ART", GetType(System.String))
    '    Return tbl

    'End Function


    Public Sub GiveCars()
        Dim tableHide As New DataTable()
        Dim status As String = ""
        Dim row As DataRow
        Dim rowResult As DataRow()

        readAllAuthorizationSets(tableHide, status) 'Daten, die zur Autorisierung einstehen

        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Dim intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Unangefordert_Arval", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

            Try
                Dim proxy = DynSapProxy.getProxy("Z_M_UNANGEFORDERT_ARVAL", m_objApp, m_objUser, GetCurrentPage())
                proxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.KUNNR, 10))
                proxy.setImportParameter("I_LICENSE_NUM", m_strSucheFgstNr)
                proxy.setImportParameter("I_LIZNR", m_strSucheLvNr)
                proxy.setImportParameter("I_VKORG", "1510")

                proxy.callBapi()

                Dim tblGrund = proxy.getExportTable("GT_GRU")
                Dim tblFahrzeuge = proxy.getExportTable("GT_WEB")

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                m_tableGrund = tblGrund
                m_tblResult = tblFahrzeuge
                m_tblResult.Columns.Add("STATUS")
                m_intStatus = 0

                'Zur Autorisierung gespeicherte Daten entfernen!
                'If tableHide.Rows.Count > 0 Then
                '    For Each row In tableHide.Rows
                '        rowResult = m_tblResult.Select("EQUNR = '" & row("EQUIPMENT").ToString & "'")
                '        If Not (rowResult.Length = 0) Then
                '            m_tblResult.Rows.Remove(rowResult(0))
                '        End If
                '    Next
                'End If

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
                    m_intStatus = -3331
                    m_strMessage = "Keine Daten gefunden."
                End If

                WriteLogEntry(True, "HAENDLER=" & m_strHaendlernummer & ", LVNr.=" & m_strSucheLvNr & ", KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -3331
                        m_strMessage = "Keine Daten gefunden."
                    Case "NO_HAENDLER"
                        m_intStatus = -3332
                        m_strMessage = "Keine oder falsche Haendlernummer."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "HAENDLER=" & m_strHaendlernummer & ", LVNr.=" & m_strSucheLvNr & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub getZulassungsdienste(ByRef tblSTVA As DataTable, ByRef status As String)

        Try


            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            Dim proxy = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", m_objApp, m_objUser, GetCurrentPage)
            proxy.callBapi()


            tblSTVA = proxy.getExportTable("T_ZULST")

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
            End If

        Catch ex As Exception
            Select Case ex.Message
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
            status = m_strMessage
        Finally
            If m_intIDSAP > -1 Then
                m_objLogApp.LogStandardIdentity = m_intStandardLogID
                m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
            End If
        End Try
    End Sub

    Private Sub AnfordernSAP()
        If Not m_blnGestartet Then
            m_blnGestartet = True


            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                m_strZielLand = RemoveSingleSpace(m_strZielLand)
                If m_strZielLand = "" Then
                    m_strZielLand = "DE"
                End If

                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Briefanforderung_Arval", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim proxy = DynSapProxy.getProxy("Z_M_Briefanforderung_Arval", m_objApp, m_objUser, GetCurrentPage())
                proxy.setImportParameter("I_ABCKZ", m_abckz)
                proxy.setImportParameter("I_CHASSIS_NUM", m_strSucheFgstNr)
                proxy.setImportParameter("I_EQUNR", m_equ)
                proxy.setImportParameter("I_ERNAM", m_objUser.UserName)
                proxy.setImportParameter("I_HOUSE", m_strZielHNr)
                proxy.setImportParameter("I_KUNNR_AG", KUNNR)
                proxy.setImportParameter("I_KUNNR_ZE", m_versandadr)
                proxy.setImportParameter("I_KUNNR_ZF", m_strHaendlernummer)
                proxy.setImportParameter("I_KUNNR_ZH", "")
                proxy.setImportParameter("I_KUNNR_ZS", "")
                proxy.setImportParameter("I_LAND1", m_strZielLand)
                proxy.setImportParameter("I_LICENSE_NUM", m_kennz)
                proxy.setImportParameter("I_LIZNR", m_liznr)
                proxy.setImportParameter("I_MATNR", Right("000000000000000000" & m_material, 18))
                proxy.setImportParameter("I_NAME1", m_strZielFirma)
                proxy.setImportParameter("I_NAME2", Replace(m_strZielFirma2, "&nbsp;", ""))
                proxy.setImportParameter("I_ORT01", m_strZielOrt)
                proxy.setImportParameter("I_PSTLZ", m_strZielPLZ)
                proxy.setImportParameter("I_STR01", m_strZielStrasse)
                proxy.setImportParameter("I_TIDNR", m_tidnr)
                proxy.setImportParameter("I_ZZBETREFF", m_strBemerkung)
                proxy.setImportParameter("I_ZZVGRUND", m_versgrund)

                proxy.callBapi()

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If
                strAuftragsnummer = proxy.getExportParameter("E_VBELN").TrimStart("0"c)
                strAuftragsstatus = "Vorgang OK"
                If String.IsNullOrEmpty(strAuftragsnummer) Then
                    m_intStatus = -2100
                    m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                    strAuftragsstatus = "Fehler: Anforderung konnte nicht erstellt werden."
                End If
            Catch ex As Exception
                strAuftragsstatus = "Fehler"
                Select Case ex.Message
                    Case "ZCREDITCONTROL_ENTRY_LOCKED"
                        m_strMessage = "System ausgelastet. Bitte clicken Sie erneut auf ""Absenden""."
                        m_intStatus = -1111
                    Case "NO_UPDATE_EQUI"
                        m_strMessage = "Fehler bei der Datenspeicherung (EQUI-UPDATE)"
                        m_intStatus = -1112
                    Case "NO_AUFTRAG"
                        m_strMessage = "Kein Auftrag angelegt"
                        m_intStatus = -1113
                    Case "NO_ZDADVERSAND"
                        m_strMessage = "Keine Einträge für die Versanddatei erstellt"
                        m_intStatus = -1114
                    Case "NO_MODIFY_ILOA"
                        m_strMessage = "ILOA-MODIFY-Fehler"
                        m_intStatus = -1115
                    Case "NO_BRIEFANFORDERUNG"
                        m_strMessage = "Brief bereits angefordert"
                        m_intStatus = -1116
                    Case "NO_EQUZ"
                        m_strMessage = "Kein Brief vorhanden (EQUZ)"
                        m_intStatus = -1117
                    Case "NO_ILOA"
                        m_strMessage = "Kein Brief vorhanden (ILOA)"
                        m_intStatus = -1118
                    Case Else
                        m_strMessage = ex.Message
                        m_intStatus = -9999
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                If m_intIDSAP > -1 Then
                    m_objLogApp.LogStandardIdentity = m_intStandardLogID
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Private Sub addResultTableHeader()
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

    Private Sub addResultTableRow(ByVal id As String, ByVal tstamp As String, ByVal equi As String, ByVal user As String, ByVal objData As ArrayList)
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
        Dim ms As New MemoryStream()
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
                        objData = New ArrayList()
                        objData = DirectCast(formatter.Deserialize(ms), ArrayList)
                        addResultTableRow(CType(drAppData("id"), String), CType(drAppData("tstamp"), String), CType(drAppData("equipment"), String), CType(drAppData("username"), String), objData)
                    Else
                        ms = Nothing
                    End If
                Else
                    ms = Nothing
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
        Dim connection As SqlClient.SqlConnection
        Dim para As SqlClient.SqlParameter

        Dim command As New SqlClient.SqlCommand()
        Dim sqlInsert As String
        Dim b As Byte()
        connection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
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

    Public Sub clearDB(ByVal id As Int32, ByRef status As String)
        Dim connection As SqlClient.SqlConnection
        Dim command As New SqlClient.SqlCommand()
        Dim sqlInsert As String
        connection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
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

    Public Sub readAllAuthorizationSets(ByRef resultTable As DataTable, ByRef status As String)
        Dim connection As New SqlClient.SqlConnection
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim sqlInsert As String

        Try
            connection = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
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

        Dim intID As Int32 = -1

        Try
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Land_Plz_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

            Dim proxy = DynSapProxy.getProxy("Z_M_LAND_PLZ_001", m_objApp, m_objUser, GetCurrentPage())
            proxy.callBapi()


            m_objLogApp.WriteEndDataAccessSAP(intID, True)

            m_tblLaender = proxy.getExportTable("GT_WEB")

            m_tblLaender.Columns.Add("Beschreibung", GetType(String))
            m_tblLaender.Columns.Add("FullDesc", GetType(String))

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

            m_objLogApp.WriteEndDataAccessSAP(intID, False, ex.Message)
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
        Finally
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

    Private Function GetCurrentPage() As Page
        If HttpContext.Current Is Nothing OrElse HttpContext.Current.Handler Is Nothing Then Return Nothing
        Return TryCast(HttpContext.Current.Handler, Page)
    End Function
#End Region
End Class

' ************************************************
' $History: arval_1.vb $
' 
' *****************  Version 2  *****************
' User: Dittbernerc  Date: 30.07.09   Time: 15:05
' Updated in $/CKAG2/Applications/AppArval/lib
' Anlegen Report203_2
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 16.04.09   Time: 11:30
' Created in $/CKAG2/Applications/AppArval/lib
' Arval
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
