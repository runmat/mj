Option Explicit On
Option Strict On

Imports System
'Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Data.SqlClient
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class SIXT_PDI
    REM § Lese-/Schreibfunktion, Kunde: SIXT, 
    REM § New - BAPI: Z_M_Meldungen_Pdi,
    REM § Show - BAPI: Z_M_Meldungen_Fahrzeuge,
    REM § ShowDatenABE - BAPI: Z_M_Abezufzg,
    REM § SelectCars - BAPI: ,
    REM § Change - BAPI: Z_Massenzulassung, Z_M_Zulassungssperre, Z_M_Pdiwechsel.

    Inherits Base.Business.BankBase  'FDD_Bank_Base

#Region " Declarations"
    Private m_intErrorCount As Int32
    Private m_objABE_Daten As Base.Business.ABE2FHZG
    Private m_dsPDI_Data As DataSet
    Private m_dsPDI_Data_Selected As DataSet
    Private m_tblPDIs As DataTable
    Private m_tblAllPDIs As DataTable
    Private m_tblModelle As DataTable
    Private m_tblFahrzeuge As DataTable
    Private m_tblSaveCars As DataTable
    Private m_tblFahrgestellNr As DataTable
    Private m_strPDINummer As String
    Private m_strPDINummerSuche As String
    Private m_strModellCode As String
    Private m_strFahrgestellnummer As String
    Private m_intLastID As Int32
    Private m_strTask As String
    Private m_intSelectedCars As Int32
    Private m_blnShowBelegnummer As Boolean
    Private m_intFahrzeugeGesamtZulassungsf As Int32
    Private m_intFahrzeugeGesamtGesperrt As Int32
    Private m_tblErledigt As DataTable
    Private data As ArrayList
    Private returnList As ArrayList
    '§§§ JVE 26.10.2006
    Private m_intSucheFarbe As Integer
    Private m_strSucheFahrgestellnummer As String
#End Region

#Region " Properties"
    Public ReadOnly Property FahrzeugeGesamt() As DataTable '§§§ JVE 26.10.2006
        Get
            Return m_tblSaveCars
        End Get
    End Property

    Public Property PSucheFarbe() As Integer '§§§ JVE 26.10.2006
        Get
            Return m_intSucheFarbe
        End Get
        Set(ByVal Value As Integer)
            m_intSucheFarbe = Value
        End Set
    End Property

    Public ReadOnly Property ErrorCount() As Int32
        Get
            Return m_intErrorCount
        End Get
    End Property

    Public ReadOnly Property Erledigt() As DataTable
        Get
            Return m_tblErledigt
        End Get
    End Property

    Public ReadOnly Property FahrzeugeGesamtZulassungsf() As Int32
        Get
            Return m_intFahrzeugeGesamtZulassungsf
        End Get
    End Property

    Public ReadOnly Property FahrzeugeGesamtGesperrt() As Int32
        Get
            Return m_intFahrzeugeGesamtGesperrt
        End Get
    End Property

    Public Property ABE_Daten() As Base.Business.ABE2FHZG
        Get
            Return m_objABE_Daten
        End Get
        Set(ByVal Value As Base.Business.ABE2FHZG)
            m_objABE_Daten = Value
        End Set
    End Property

    Public ReadOnly Property AllPDIs() As DataTable
        Get
            Return m_tblAllPDIs
        End Get
    End Property

    Public ReadOnly Property PDI_Data_Selected() As DataSet
        Get
            Return m_dsPDI_Data_Selected
        End Get
    End Property

    Public ReadOnly Property ShowBelegnummer() As Boolean
        Get
            Return m_blnShowBelegnummer
        End Get
    End Property

    Public ReadOnly Property PDI_Data() As DataSet
        Get
            Return m_dsPDI_Data
        End Get
    End Property

    Public ReadOnly Property Task() As String
        Get
            Return m_strTask
        End Get
    End Property

    Public Property PDINummer() As String
        Get
            Return m_strPDINummer
        End Get
        Set(ByVal Value As String)
            m_strPDINummer = Value
        End Set
    End Property

    Public Property PDINummerSuche() As String
        Get
            Return m_strPDINummerSuche
        End Get
        Set(ByVal Value As String)
            m_strPDINummerSuche = Value
        End Set
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return m_strFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummer = Value
        End Set
    End Property

    Public Property ModellCode() As String
        Get
            Return m_strModellCode
        End Get
        Set(ByVal Value As String)
            m_strModellCode = Value
        End Set
    End Property

    Public ReadOnly Property SelectedCars() As Int32
        Get
            Return m_intSelectedCars
        End Get
    End Property

    Public ReadOnly Property confirmList() As ArrayList
        Get
            Return returnList
        End Get
    End Property

    Public Property FahrgestellNr() As DataTable
        Get
            Return m_tblFahrgestellNr
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrgestellNr = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, ByRef strCustomer As String, ByRef strCreditControlArea As String, ByRef strPDINummerSuche As String, ByRef strTask As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        If Not m_blnGestartet Then
            m_blnGestartet = True
            m_intErrorCount = 0
            m_intLastID = -1
            m_strModellCode = ""
            m_strFahrgestellnummer = ""
            m_strPDINummer = ""
            m_intSelectedCars = 0
            m_blnShowBelegnummer = False
            m_strCustomer = Right("0000000000" & strCustomer, 10)
            m_strCreditControlArea = strCreditControlArea
            m_strPDINummerSuche = strPDINummerSuche
            m_strTask = strTask
            m_objABE_Daten = New Base.Business.ABE2FHZG()

            '--- Alle PDIs ------------------------------------------------------
            m_tblAllPDIs = New DataTable("AllPDIs")
            With m_tblAllPDIs.Columns
                .Add("PDI_Nummer", System.Type.GetType("System.String"))
                .Add("Kunden_PDI_Nummer", System.Type.GetType("System.String"))
                .Add("PDI_Name", System.Type.GetType("System.String"))
                .Add("Zulassungsf_Fahrzeuge", System.Type.GetType("System.Int32"))
                .Add("Gesperrte_Fahrzeuge", System.Type.GetType("System.Int32"))
                .Add("Details", System.Type.GetType("System.Boolean"))
                .Add("Loaded", System.Type.GetType("System.Boolean"))
            End With

            '--- PDIs -----------------------------------------------------------
            m_dsPDI_Data = New DataSet()
            m_tblPDIs = New DataTable("PDIs")

            With m_tblPDIs.Columns
                .Add("PDI_Nummer", System.Type.GetType("System.String"))
                .Add("Kunden_PDI_Nummer", System.Type.GetType("System.String"))
                .Add("PDI_Name", System.Type.GetType("System.String"))
                .Add("Zulassungsf_Fahrzeuge", System.Type.GetType("System.Int32"))
                .Add("Gesperrte_Fahrzeuge", System.Type.GetType("System.Int32"))
                .Add("Details", System.Type.GetType("System.Boolean"))
                .Add("Loaded", System.Type.GetType("System.Boolean"))
            End With

            '--- Modelle ---------------------------------------------------------
            m_tblModelle = New DataTable("Modelle")

            With m_tblModelle.Columns
                .Add("ID", System.Type.GetType("System.Int32"))
                .Add("PDI_Nummer", System.Type.GetType("System.String"))
                .Add("Kunden_PDI_Nummer", System.Type.GetType("System.String"))
                .Add("PDI_Name", System.Type.GetType("System.String"))
                .Add("Hersteller", System.Type.GetType("System.String"))
                .Add("Modellbezeichnung", System.Type.GetType("System.String"))
                .Add("Schaltung", System.Type.GetType("System.String"))
                .Add("Ausfuehrung", System.Type.GetType("System.String"))
                .Add("Antrieb", System.Type.GetType("System.String"))
                .Add("Bereifung", System.Type.GetType("System.String"))
                .Add("Navigation", System.Type.GetType("System.String"))
                .Add("Beklebung", System.Type.GetType("System.Boolean"))
                .Add("BeklebungAlsText", System.Type.GetType("System.String"))
                .Add("Sperre", System.Type.GetType("System.Boolean"))
                .Add("SperreAlsText", System.Type.GetType("System.String"))
                .Add("Limo", System.Type.GetType("System.Boolean"))
                .Add("Anzahl_alt", System.Type.GetType("System.Int32"))
                .Add("Anzahl_neu", System.Type.GetType("System.Int32"))
                .Add("Bemerkung", System.Type.GetType("System.String"))
                '---------------------------------------------------------------------
                '§§§ JVE 03.05.2006: Neues Bemerkungsfeld.
                .Add("BemerkungDatum", System.Type.GetType("System.DateTime"))
                .Add("Fehler", System.Type.GetType("System.String"))
                '---------------------------------------------------------------------
                .Add("DatumErstzulassung", System.Type.GetType("System.DateTime"))
                .Add("ZielPDI", System.Type.GetType("System.String"))
                .Add("OrtsKennz", System.Type.GetType("System.String"))
                .Add("WK1", System.Type.GetType("System.String"))
                .Add("WK2", System.Type.GetType("System.String"))
                .Add("WK3", System.Type.GetType("System.String"))
                .Add("Task", System.Type.GetType("System.String"))
            End With

            '--- FAHRZEUGE ---------------------------------------------------------
            m_tblFahrzeuge = New DataTable("Fahrzeuge")

            With m_tblFahrzeuge.Columns
                .Add("Modell_ID", System.Type.GetType("System.Int32"))
                .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                .Add("Meldungsnummer", System.Type.GetType("System.String"))
                .Add("Equipmentnummer", System.Type.GetType("System.String"))
                .Add("Eingangsdatum", System.Type.GetType("System.DateTime"))
                .Add("Ausgewaehlt", System.Type.GetType("System.Boolean"))
                .Add("Limo", System.Type.GetType("System.Boolean"))
                .Add("Kennzeichen2zeilig", System.Type.GetType("System.Boolean"))
                .Add("Bemerkung", System.Type.GetType("System.String"))
                '---------------------------------------------------------------------
                '§§§ JVE 03.05.2006: Neues Bemerkungsfeld.
                .Add("BemerkungDatum", System.Type.GetType("System.DateTime"))
                .Add("Fehler", System.Type.GetType("System.String"))
                '---------------------------------------------------------------------
                .Add("DatumErstzulassung", System.Type.GetType("System.DateTime"))
                .Add("ZielPDI", System.Type.GetType("System.String"))
                .Add("OrtsKennz", System.Type.GetType("System.String"))
                .Add("Briefnr", System.Type.GetType("System.String"))
                .Add("WK1", System.Type.GetType("System.String"))
                .Add("WK2", System.Type.GetType("System.String"))
                .Add("WK3", System.Type.GetType("System.String"))
                .Add("DADPDI", System.Type.GetType("System.String"))
                .Add("Belegnummer", System.Type.GetType("System.String"))
            End With

            m_dsPDI_Data.Tables.Add(m_tblPDIs)
            m_dsPDI_Data.Tables.Add(m_tblModelle)
            m_dsPDI_Data.Tables.Add(m_tblFahrzeuge)

            Dim dc1 As DataColumn
            Dim dc2 As DataColumn
            'Relation Author => Title
            dc1 = m_dsPDI_Data.Tables("PDIs").Columns("PDI_Nummer")
            dc2 = m_dsPDI_Data.Tables("Modelle").Columns("PDI_Nummer")
            Dim dr As DataRelation = New DataRelation("PDI_Modell", dc1, dc2, False)
            m_dsPDI_Data.Relations.Add(dr)

            'Relation Title => Sales
            dc1 = m_dsPDI_Data.Tables("Modelle").Columns("ID")
            dc2 = m_dsPDI_Data.Tables("Fahrzeuge").Columns("Modell_ID")
            dr = New DataRelation("Modell_Fahrzeug", dc1, dc2, False)
            m_dsPDI_Data.Relations.Add(dr)

            m_blnGestartet = False
        End If
    End Sub

    Public Sub ShowPDIs(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "SIXT_PDI.ShowPDIs"
        Dim intCounter As Integer
        Dim intFarbe As Integer

        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intErrorCount = 0
            m_intLastID = -1


            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""
                m_intFahrzeugeGesamtZulassungsf = 0
                m_intFahrzeugeGesamtGesperrt = 0

                If CheckCustomerData() Then
                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_MELDUNGEN_PDI1", m_objApp, m_objUser, page)

                    'myProxy.setImportParameter("I_KUNNR", m_strCustomer)
                    'myProxy.setImportParameter("I_VKORG", "1510")
                    'myProxy.setImportParameter("I_ZZCARPORT", m_strPDINummerSuche)

                    S.AP.Init("Z_M_MELDUNGEN_PDI1", "I_KUNNR, I_VKORG, I_ZZCARPORT", m_strCustomer, "1510", m_strPDINummerSuche)

                    If Not m_tblFahrgestellNr Is Nothing Then
                        If m_tblFahrgestellNr.Rows.Count > 0 Then
                            'Dim tblIN As DataTable = myProxy.getImportTable("I_ZZFAHRG")
                            Dim tblIN As DataTable = S.AP.GetImportTable("I_ZZFAHRG")
                            For Each tblRow As DataRow In m_tblFahrgestellNr.Rows
                                Dim RowNeu As DataRow = tblIN.NewRow
                                RowNeu("ZZFAHRG") = tblRow("ZZFAHRG").ToString
                                tblIN.Rows.Add(RowNeu)
                            Next
                        End If
                    End If

                    'myProxy.callBapi()
                    S.AP.Execute()

                    'PDIs
                    Dim i As Int32
                    Dim rowCount As Int32
                    'Dim tblTemp As DataTable = myProxy.getExportTable("GT_WEB")
                    Dim tblTemp As DataTable = S.AP.GetExportTable("GT_WEB")

                    tblTemp.Columns.Add("SortPDIs", System.Type.GetType("System.Int32"))
                    rowCount = tblTemp.Rows.Count - 1
                    For i = rowCount To 0 Step -1       'Fahrzeuge, die nicht dargestellt werden sollen, entfernen...
                        If (tblTemp.Rows(i)("ZZAKTSPERRE").ToString.Trim = "D") Then
                            tblTemp.Rows(i).Delete()
                        Else
                            Dim strTmp As String = CStr(tblTemp.Rows(i)("KUNPDI"))
                            If IsNumeric(tblTemp.Rows(i)("KUNPDI")) Then
                                tblTemp.Rows(i)("SortPDIs") = CInt(strTmp)
                            Else
                                Try
                                    tblTemp.Rows(i)("SortPDIs") = 10000 + CInt(Right(strTmp, strTmp.Length - 3))
                                Catch
                                    tblTemp.Rows(i)("SortPDIs") = 99999
                                End Try
                            End If
                        End If
                    Next

                    '§§§ JVE 26.10.2006: Evtl. Farbe filtern    
                    If (PSucheFarbe >= 0) Then
                        For intCounter = tblTemp.Rows.Count - 1 To 0 Step -1
                            Try
                                intFarbe = CInt(tblTemp.Rows(intCounter)("ZZFARBE"))
                                If (intFarbe <> PSucheFarbe) Then
                                    tblTemp.Rows.Remove(tblTemp.Rows(intCounter))
                                End If
                            Catch ex As Exception
                                'Wenn keine Farbinformation ermittelt werden konnte, Fahrzeug entfernen...
                                tblTemp.Rows.Remove(tblTemp.Rows(intCounter))
                            End Try
                        Next
                    End If

                    Dim vwTemp As DataView = tblTemp.DefaultView
                    vwTemp.Sort = "SortPDIs"

                    Dim intZul As Int32 = 0
                    Dim intGsp As Int32 = 0
                    Dim rowNew2 As DataRow

                    If (tblTemp.Rows.Count > 0) Then       'Nur wenn noch Fahrzeuge vorhanden, weiter...

                        rowNew2 = m_tblAllPDIs.NewRow
                        rowNew2("PDI_Nummer") = vwTemp(0)("DADPDI")
                        rowNew2("Kunden_PDI_Nummer") = vwTemp(0)("KUNPDI")
                        rowNew2("PDI_Name") = vwTemp(0)("DADPDI_NAME1")
                        If CStr(vwTemp(0)("ZZAKTSPERRE")) = "X" Then
                            intGsp = 1
                            intZul = 0
                        Else
                            intGsp = 0
                            intZul = 1
                        End If
                        Dim strPDIName As String = CStr(vwTemp(0)("DADPDI"))

                        For i = 1 To vwTemp.Count - 1
                            If (Not strPDIName = CStr(vwTemp(i)("DADPDI"))) Then
                                m_intFahrzeugeGesamtZulassungsf += intZul
                                m_intFahrzeugeGesamtGesperrt += intGsp
                                rowNew2("Zulassungsf_Fahrzeuge") = intZul
                                rowNew2("Gesperrte_Fahrzeuge") = intGsp
                                rowNew2("Details") = False
                                rowNew2("Loaded") = False
                                m_tblAllPDIs.Rows.Add(rowNew2)
                                If intZul > 0 Or intGsp > 0 Then
                                    Dim rowNew As DataRow = m_tblPDIs.NewRow
                                    rowNew("PDI_Nummer") = rowNew2("PDI_Nummer")
                                    rowNew("Kunden_PDI_Nummer") = rowNew2("Kunden_PDI_Nummer")
                                    rowNew("PDI_Name") = rowNew2("PDI_Name")
                                    rowNew("Zulassungsf_Fahrzeuge") = rowNew2("Zulassungsf_Fahrzeuge")
                                    rowNew("Gesperrte_Fahrzeuge") = rowNew2("Gesperrte_Fahrzeuge")
                                    If m_strTask = "Entsperren" Then
                                        If intGsp > 0 Then
                                            rowNew("Details") = True
                                        Else
                                            rowNew("Details") = False
                                        End If
                                    Else
                                        If intZul > 0 Then
                                            rowNew("Details") = True
                                        Else
                                            rowNew("Details") = False
                                        End If
                                    End If
                                    rowNew("Loaded") = False
                                    m_tblPDIs.Rows.Add(rowNew)
                                End If

                                rowNew2 = m_tblAllPDIs.NewRow
                                rowNew2("PDI_Nummer") = vwTemp(i)("DADPDI")
                                rowNew2("Kunden_PDI_Nummer") = vwTemp(i)("KUNPDI")
                                rowNew2("PDI_Name") = vwTemp(i)("DADPDI_NAME1")

                                If CStr(vwTemp(i)("ZZAKTSPERRE")) = "X" Then
                                    intGsp = 1
                                    intZul = 0
                                Else
                                    intGsp = 0
                                    intZul = 1
                                End If

                                strPDIName = CStr(vwTemp(i)("DADPDI"))
                            Else
                                If CStr(vwTemp(i)("ZZAKTSPERRE")) = "X" Then
                                    intGsp += 1
                                Else
                                    intZul += 1
                                End If
                            End If
                        Next
                        m_intFahrzeugeGesamtZulassungsf += intZul
                        m_intFahrzeugeGesamtGesperrt += intGsp
                        rowNew2("Zulassungsf_Fahrzeuge") = intZul
                        rowNew2("Gesperrte_Fahrzeuge") = intGsp
                        rowNew2("Details") = False
                        rowNew2("Loaded") = False
                        m_tblAllPDIs.Rows.Add(rowNew2)
                        If intZul > 0 Or intGsp > 0 Then
                            Dim rowNew As DataRow = m_tblPDIs.NewRow
                            rowNew("PDI_Nummer") = rowNew2("PDI_Nummer")
                            rowNew("Kunden_PDI_Nummer") = rowNew2("Kunden_PDI_Nummer")
                            rowNew("PDI_Name") = rowNew2("PDI_Name")
                            rowNew("Zulassungsf_Fahrzeuge") = rowNew2("Zulassungsf_Fahrzeuge")
                            rowNew("Gesperrte_Fahrzeuge") = rowNew2("Gesperrte_Fahrzeuge")
                            rowNew("Details") = False
                            rowNew("Loaded") = False
                            m_tblPDIs.Rows.Add(rowNew)
                            If m_strTask = "Entsperren" Then
                                If intGsp > 0 Then
                                    rowNew("Details") = True
                                Else
                                    rowNew("Details") = False
                                End If
                            Else
                                If intZul > 0 Then
                                    rowNew("Details") = True
                                Else
                                    rowNew("Details") = False
                                End If
                            End If
                            rowNew("Loaded") = False
                        End If

                        '2. Fahrzeuge und Modelle

                        'Dim tblMeldungen As DataTable = myProxy.getExportTable("GT_TXT")
                        Dim tblMeldungen As DataTable = S.AP.GetExportTable("GT_TXT")

                        tblTemp.Columns.Add("Bemerkung", System.Type.GetType("System.String"))
                        '----------------------------------------------------------------------
                        '§§§ JVE 03.05.2006: Bemerkung Datum
                        tblTemp.Columns.Add("BemerkungDatum", System.Type.GetType("System.DateTime"))
                        '----------------------------------------------------------------------

                        tblTemp.Columns.Add("Limo", System.Type.GetType("System.Boolean"))

                        If tblTemp.Rows.Count > 0 Then
                            Dim objZulassungsVorbelegung As New Base.Business.ZulassungsVorbelegung(Now, m_objApp.Connectionstring)

                            vwTemp = tblMeldungen.DefaultView
                            vwTemp.Sort = "QMNUM, LFDNUM"

                            For i = tblTemp.Rows.Count - 1 To 0 Step -1
                                'tblTemp.AcceptChanges()
                                Dim blnRemove As Boolean = False
                                If m_strTask = "Entsperren" Then
                                    If Not CStr(tblTemp.Rows(i)("ZZAKTSPERRE")) = "X" Then
                                        blnRemove = True
                                    End If
                                Else
                                    If CStr(tblTemp.Rows(i)("ZZAKTSPERRE")) = "X" Then
                                        blnRemove = True
                                    End If
                                End If

                                If blnRemove Then
                                    tblTemp.Rows(i).Delete()
                                Else
                                    'Aus Ordernummer VM -> j/n ermitteln
                                    objZulassungsVorbelegung.Find(CStr(tblTemp.Rows(i)("ZZFAHRG")))
                                    'tblTemp.Rows(i)("VM") = objZulassungsVorbelegung.CheckVMBoolean(CStr(tblTemp.Rows(i)("ZZREF1")), CStr(tblTemp.Rows(i)("FLEET_VIN")))
                                    'Aus Fahrgestellnummer Limo und Kennzeichen2zeilig -> j/n ermitteln
                                    tblTemp.Rows(i)("Limo") = objZulassungsVorbelegung.Limo
                                    'tblTemp.Rows(i)("Kennzeichen2zeilig") = objZulassungsVorbelegung.Kennzeichen2zeilig

                                    vwTemp.RowFilter = "QMNUM = '" & tblTemp.Rows(i)("QMNUM").ToString & "'"

                                    tblTemp.Rows(i)("Bemerkung") = ""
                                    If Not vwTemp.Count = 0 Then
                                        Dim j As Int32
                                        For j = 0 To vwTemp.Count - 1
                                            tblTemp.Rows(i)("Bemerkung") = tblTemp.Rows(i)("Bemerkung").ToString & vwTemp(j)("TDLINE").ToString & " "
                                        Next
                                    End If

                                    tblTemp.AcceptChanges()
                                End If

                            Next
                            objZulassungsVorbelegung.CloseDB()
                            vwTemp.RowFilter = ""
                        End If

                        m_tblSaveCars = tblTemp

                        WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZCARPORT=" & m_strPDINummerSuche, m_tblPDIs)
                    End If

                End If
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = 0
                        m_strMessage = "Es wurden keine Daten gefunden."
                    Case "NO_WEB"
                        m_intStatus = 0
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZCARPORT=" & m_strPDINummerSuche & " , " & Replace(m_strMessage, "<br>", " "), m_tblPDIs)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub ShowPDIsAll(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "SIXT_PDI.ShowPDIsAll"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intErrorCount = 0
            m_intLastID = -1

            'Create start data
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""
                m_intFahrzeugeGesamtZulassungsf = 0
                m_intFahrzeugeGesamtGesperrt = 0

                If CheckCustomerData() Then

                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_MELDUNGEN_PDI1", m_objApp, m_objUser, page)

                    'myProxy.setImportParameter("I_KUNNR", m_strCustomer)
                    'myProxy.setImportParameter("I_VKORG", "1510")
                    'myProxy.setImportParameter("I_ZZCARPORT", m_strPDINummerSuche)

                    'myProxy.callBapi()

                    S.AP.InitExecute("Z_M_MELDUNGEN_PDI1", "I_KUNNR, I_VKORG, I_ZZCARPORT", m_strCustomer, "1510", m_strPDINummerSuche)

                    'Dim tblTemp As DataTable = myProxy.getExportTable("GT_WEB")
                    Dim tblTemp As DataTable = S.AP.GetExportTable("GT_WEB")

                    Dim vwTemp As DataView = tblTemp.DefaultView
                    Dim i As Int32

                    With m_tblPDIs.Columns
                        .Add("Fahrgestellnr", System.Type.GetType("System.String"))
                        .Add("Liznr", System.Type.GetType("System.String"))
                        .Add("Hersteller", System.Type.GetType("System.String"))
                        .Add("QMNUM", System.Type.GetType("System.String"))
                        .Add("Modellbezeichnung", System.Type.GetType("System.String"))
                        .Add("Schaltung", System.Type.GetType("System.String"))

                        .Add("Ausfuehrung", System.Type.GetType("System.String"))
                        .Add("Antrieb", System.Type.GetType("System.String"))
                        .Add("Bereifung", System.Type.GetType("System.String"))
                        .Add("Navigation", System.Type.GetType("System.String"))
                        .Add("Beklebung", System.Type.GetType("System.String"))
                        .Add("Identifikation", System.Type.GetType("System.String"))
                        .Add("Bemerkungsdatum", System.Type.GetType("System.String"))
                    End With

                    For i = 0 To vwTemp.Count - 1
                        Dim rowNew As DataRow = m_tblPDIs.NewRow
                        rowNew("PDI_Nummer") = vwTemp(i)("DADPDI")
                        rowNew("Kunden_PDI_Nummer") = vwTemp(i)("KUNPDI")
                        rowNew("PDI_Name") = vwTemp(i)("DADPDI_NAME1")
                        rowNew("Zulassungsf_Fahrzeuge") = CInt(vwTemp(i)("ANZAHL_ZUL"))
                        rowNew("Gesperrte_Fahrzeuge") = CInt(vwTemp(i)("ANZAHL_GSP"))
                        rowNew("Details") = False
                        rowNew("Loaded") = False

                        rowNew("Fahrgestellnr") = vwTemp(i)("ZZFAHRG")
                        rowNew("Liznr") = vwTemp(i)("ZZREF1")
                        rowNew("Hersteller") = vwTemp(i)("HERST_K")

                        rowNew("Modellbezeichnung") = vwTemp(i)("ZZBEZEI")
                        rowNew("Schaltung") = vwTemp(i)("ZZSIPP3")

                        rowNew("Ausfuehrung") = vwTemp(i)("ZZAUSF")
                        rowNew("Antrieb") = vwTemp(i)("ZZANTR")
                        rowNew("Bereifung") = vwTemp(i)("ZZREIFEN")
                        rowNew("Navigation") = vwTemp(i)("ZZNAVI")

                        If vwTemp(i)("ZZKLEBE").ToString = "X" Then
                            rowNew("Beklebung") = "Y"
                        Else
                            rowNew("Beklebung") = "N"
                        End If
                        rowNew("QMNUM") = vwTemp(i)("QMNUM")
                        If Not (vwTemp(i)("ZZAKTSPERRE").ToString = "X") Then   'Nur nicht gesperrte KFZ einfügen
                            m_tblPDIs.Rows.Add(rowNew)
                        End If
                        rowNew("Identifikation") = vwTemp(i)("FLEET_VIN")
                        rowNew("Bemerkungsdatum") = vwTemp(i)("ZZDATBEM")
                        'End If
                    Next

                    WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZCARPORT=" & m_strPDINummerSuche, m_tblPDIs)
                End If
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = 0
                        m_strMessage = "Es wurden keine Daten gefunden."
                    Case "NO_WEB"
                        m_intStatus = 0
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZCARPORT=" & m_strPDINummerSuche & " , " & Replace(m_strMessage, "<br>", " "), m_tblPDIs)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Show()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim vwTemp As DataView = m_tblModelle.DefaultView
                vwTemp.RowFilter = "PDI_Nummer = '" & m_strPDINummer & "'"
                Dim intTemp As Int32 = vwTemp.Count
                vwTemp.RowFilter = ""

                If intTemp = 0 Then
                    Dim tblTemp As DataTable = m_tblSaveCars
                    If Not AddData(tblTemp) Then
                        Exit Sub
                    End If

                    Dim blnDetails As Boolean = False

                    If m_tblFahrzeuge.Rows.Count = 0 Then
                        m_intStatus = 0
                        m_strMessage = "Es wurden keine Daten gefunden."
                    Else
                        blnDetails = True
                    End If
                    Dim i As Int32
                    For i = 0 To m_tblPDIs.Rows.Count - 1
                        If m_tblPDIs.Rows(i)("PDI_Nummer").ToString = m_strPDINummer Then
                            m_tblPDIs.Rows(i)("Loaded") = True
                            m_tblPDIs.Rows(i)("Details") = blnDetails
                        End If
                    Next
                End If
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = ex.Message
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub ShowAll()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim vwTemp As DataView = m_tblModelle.DefaultView
                vwTemp.RowFilter = "PDI_Nummer = '" & m_strPDINummer & "'"
                Dim intTemp As Int32 = vwTemp.Count
                vwTemp.RowFilter = ""

                If intTemp = 0 Then
                    Dim tblTemp As DataTable

                    'Hier könnten die anderen Tasks separat betrachtet werden
                    Select Case m_strTask
                        Case "Entsperren"
                            'Gesperrte Fahrzeuge
                            tblTemp = GetDetailsSingleAll(True)

                            If Not m_intStatus = 0 Then
                                Exit Sub
                            End If
                        Case Else
                            'Nicht gesperrte Fahrzeuge
                            tblTemp = GetDetailsSingleAll(False)

                            If Not m_intStatus = 0 Then
                                Exit Sub
                            End If
                    End Select

                    Dim blnDetails As Boolean = False

                    If m_tblFahrzeuge.Rows.Count = 0 Then
                        m_intStatus = 0
                        m_strMessage = "Es wurden keine Daten gefunden."
                    Else
                        blnDetails = True
                    End If
                End If
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = ex.Message
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Private Function AddData(ByVal tblInput As DataTable) As Boolean
        Try
            Dim rowNew As DataRow

            Dim strDADPDI As String
            Dim strKUNPDI As String
            Dim strNAME1 As String
            Dim strHERST_K As String
            Dim strZZBEZEI As String
            Dim strZZSIPP3 As String
            Dim strZZAUSF As String
            Dim strZZANTR As String
            Dim strZZREIFEN As String
            Dim strZZNAVI As String
            Dim strZZAKTSPERRE As String
            Dim strZZKLEBE As String
            'Dim blnVM As Boolean
            Dim blnLimo As Boolean

            Dim intAnzahl As Int32

            If tblInput IsNot Nothing AndAlso tblInput.Rows.Count > 0 Then
                m_intLastID += 1
                Dim i As Int32

                Dim tmpView As DataView = tblInput.DefaultView
                tmpView.RowFilter = "DADPDI = '" & m_strPDINummer & "'"
                tmpView.Sort = "DADPDI, KUNPDI, DADPDI_NAME1, HERST_K, ZZBEZEI, ZZSIPP3, ZZAUSF, ZZANTR, ZZREIFEN, ZZNAVI, ZZKLEBE, Limo, ZZAKTSPERRE, ZZDAT_EIN"

                strDADPDI = tmpView(0)("DADPDI").ToString
                strKUNPDI = tmpView(0)("KUNPDI").ToString
                strNAME1 = tmpView(0)("DADPDI_NAME1").ToString
                strHERST_K = tmpView(0)("HERST_K").ToString
                strZZBEZEI = tmpView(0)("ZZBEZEI").ToString
                strZZSIPP3 = tmpView(0)("ZZSIPP3").ToString
                strZZAUSF = tmpView(0)("ZZAUSF").ToString
                strZZANTR = tmpView(0)("ZZANTR").ToString
                strZZREIFEN = tmpView(0)("ZZREIFEN").ToString
                strZZNAVI = tmpView(0)("ZZNAVI").ToString
                strZZAKTSPERRE = tmpView(0)("ZZAKTSPERRE").ToString
                strZZKLEBE = tmpView(0)("ZZKLEBE").ToString
                'blnVM = CType(tmpView(0)("VM"), Boolean)
                blnLimo = CType(tmpView(0)("Limo"), Boolean)

                'Modelle ermitteln
                'intAnzahl = 0
                For i = 0 To tmpView.Count - 1

                    If ((strDADPDI = tmpView(i)("DADPDI").ToString) And _
                    (strKUNPDI = tmpView(i)("KUNPDI").ToString) And _
                    (strNAME1 = tmpView(i)("DADPDI_NAME1").ToString) And _
                    (strHERST_K = tmpView(i)("HERST_K").ToString) And _
                    (strZZBEZEI = tmpView(i)("ZZBEZEI").ToString) And _
                    (strZZSIPP3 = tmpView(i)("ZZSIPP3").ToString) And _
                    (strZZAUSF = tmpView(i)("ZZAUSF").ToString) And _
                    (strZZANTR = tmpView(i)("ZZANTR").ToString) And _
                    (strZZREIFEN = tmpView(i)("ZZREIFEN").ToString) And _
                    (strZZNAVI = tmpView(i)("ZZNAVI").ToString) And _
                    (strZZKLEBE = tmpView(i)("ZZKLEBE").ToString) And _
                    (blnLimo = CType(tmpView(i)("Limo"), Boolean)) And _
                    (strZZAKTSPERRE = tmpView(i)("ZZAKTSPERRE").ToString)) Then
                        'intAnzahl += 1
                    Else
                        rowNew = m_tblModelle.NewRow
                        rowNew("ID") = m_intLastID
                        rowNew("PDI_Nummer") = strDADPDI
                        rowNew("Kunden_PDI_Nummer") = strKUNPDI
                        rowNew("PDI_Name") = strNAME1
                        rowNew("Hersteller") = strHERST_K
                        rowNew("Modellbezeichnung") = strZZBEZEI
                        rowNew("Schaltung") = strZZSIPP3
                        rowNew("Ausfuehrung") = strZZAUSF
                        rowNew("Antrieb") = strZZANTR
                        rowNew("Bereifung") = strZZREIFEN
                        rowNew("Navigation") = strZZNAVI
                        'rowNew("VM") = blnVM
                        rowNew("Limo") = blnLimo

                        rowNew("BeklebungAlsText") = strZZKLEBE
                        If strZZKLEBE = "X" Then
                            rowNew("Beklebung") = True
                        Else
                            rowNew("Beklebung") = False
                        End If
                        rowNew("SperreAlsText") = strZZAKTSPERRE
                        If strZZAKTSPERRE = "X" Then
                            rowNew("Sperre") = True
                        Else
                            rowNew("Sperre") = False
                        End If
                        'rowNew("Anzahl_alt") = intAnzahl
                        rowNew("Anzahl_alt") = 0
                        rowNew("Anzahl_neu") = 0
                        rowNew("Task") = m_strTask
                        m_tblModelle.Rows.Add(rowNew)

                        'intAnzahl = 1
                        m_intLastID += 1

                        strDADPDI = tmpView(i)("DADPDI").ToString
                        strKUNPDI = tmpView(i)("KUNPDI").ToString
                        strNAME1 = tmpView(i)("DADPDI_NAME1").ToString
                        strHERST_K = tmpView(i)("HERST_K").ToString
                        strZZBEZEI = tmpView(i)("ZZBEZEI").ToString
                        strZZSIPP3 = tmpView(i)("ZZSIPP3").ToString

                        strZZAUSF = tmpView(i)("ZZAUSF").ToString
                        strZZANTR = tmpView(i)("ZZANTR").ToString
                        strZZREIFEN = tmpView(i)("ZZREIFEN").ToString
                        strZZNAVI = tmpView(i)("ZZNAVI").ToString
                        strZZKLEBE = tmpView(i)("ZZKLEBE").ToString

                        strZZAKTSPERRE = tmpView(i)("ZZAKTSPERRE").ToString
                        'blnVM = CType(tmpView(i)("VM"), Boolean)
                        blnLimo = CType(tmpView(i)("Limo"), Boolean)
                    End If
                Next

                rowNew = m_tblModelle.NewRow
                rowNew("ID") = m_intLastID
                rowNew("PDI_Nummer") = strDADPDI
                rowNew("Kunden_PDI_Nummer") = strKUNPDI
                rowNew("PDI_Name") = strNAME1
                rowNew("Hersteller") = strHERST_K
                rowNew("Modellbezeichnung") = strZZBEZEI
                rowNew("Schaltung") = strZZSIPP3
                rowNew("Ausfuehrung") = strZZAUSF
                rowNew("Antrieb") = strZZANTR
                rowNew("Bereifung") = strZZREIFEN
                rowNew("Navigation") = strZZNAVI
                'rowNew("VM") = blnVM
                rowNew("Limo") = blnLimo
                rowNew("BeklebungAlsText") = strZZKLEBE
                If strZZKLEBE = "X" Then
                    rowNew("Beklebung") = True
                Else
                    rowNew("Beklebung") = False
                End If
                rowNew("SperreAlsText") = strZZAKTSPERRE
                If strZZAKTSPERRE = "X" Then
                    rowNew("Sperre") = True
                Else
                    rowNew("Sperre") = False
                End If
                'rowNew("Anzahl_alt") = intAnzahl
                rowNew("Anzahl_alt") = 0
                rowNew("Anzahl_neu") = 0
                rowNew("Task") = m_strTask
                m_tblModelle.Rows.Add(rowNew)


                'Fahrzeuge hinzufügen
                Dim j As Int32
                Dim vwTemp As DataView = m_tblModelle.DefaultView
                vwTemp.RowFilter = "PDI_Nummer = '" & m_strPDINummer & "'"
                For j = 0 To vwTemp.Count - 1
                    tmpView.RowFilter = _
                    "DADPDI = '" & vwTemp.Item(j)("PDI_Nummer").ToString & "' AND " & _
                    "KUNPDI = '" & vwTemp.Item(j)("Kunden_PDI_Nummer").ToString & "' AND " & _
                    "DADPDI_NAME1 = '" & vwTemp.Item(j)("PDI_Name").ToString & "' AND " & _
                    "HERST_K = '" & vwTemp.Item(j)("Hersteller").ToString & "' AND " & _
                    "ZZBEZEI = '" & vwTemp.Item(j)("Modellbezeichnung").ToString & "' AND " & _
                    "ZZSIPP3 = '" & vwTemp.Item(j)("Schaltung").ToString & "' AND " & _
                    "ZZAUSF = '" & vwTemp.Item(j)("Ausfuehrung").ToString & "' AND " & _
                    "ZZANTR = '" & vwTemp.Item(j)("Antrieb").ToString & "' AND " & _
                    "ZZREIFEN = '" & vwTemp.Item(j)("Bereifung").ToString & "' AND " & _
                    "ZZNAVI = '" & vwTemp.Item(j)("Navigation").ToString & "' AND " & _
                    "ZZKLEBE = '" & vwTemp.Item(j)("BeklebungAlsText").ToString & "' AND " & _
                    "Limo = " & vwTemp.Item(j)("Limo").ToString & " AND " & _
                    "ZZAKTSPERRE = '" & vwTemp.Item(j)("SperreAlsText").ToString & "'"

                    intAnzahl = 0
                    For i = 0 To tmpView.Count - 1
                        rowNew = m_tblFahrzeuge.NewRow
                        rowNew("DADPDI") = vwTemp.Item(j)("PDI_Nummer")
                        rowNew("Modell_ID") = vwTemp.Item(j)("ID")
                        rowNew("Fahrgestellnummer") = tmpView(i)("ZZFAHRG")
                        rowNew("Meldungsnummer") = tmpView(i)("QMNUM")
                        rowNew("Equipmentnummer") = tmpView(i)("EQUNR")
                        'rowNew("VM") = tmpView(i)("VM")
                        rowNew("Limo") = tmpView(i)("Limo")
                        rowNew("Briefnr") = tmpView(i)("ZZBRIEF")
                        'rowNew("Kennzeichen2zeilig") = tmpView(i)("Kennzeichen2zeilig")
                        rowNew("Bemerkung") = Replace(CStr(tmpView(i)("Bemerkung")), "==", "")
                        rowNew("Eingangsdatum") = tmpView(i)("ZZDAT_EIN").ToString
                        rowNew("Ausgewaehlt") = False
                        '--------------------------------------------------------------------------
                        '§§§ JVE 04.05.2006: Bemerkung Datum
                        If (tmpView(i)("ZZDATBEM").ToString <> String.Empty) AndAlso (tmpView(i)("ZZDATBEM").ToString.Trim("0"c) <> String.Empty) Then
                            rowNew("BemerkungDatum") = tmpView(i)("ZZDATBEM").ToString
                        End If
                        '--------------------------------------------------------------------------
                        m_tblFahrzeuge.Rows.Add(rowNew)
                        intAnzahl += 1
                    Next
                    For i = 0 To m_tblModelle.Rows.Count - 1
                        If m_tblModelle.Rows(i)("ID").ToString = vwTemp.Item(j)("ID").ToString Then
                            m_tblModelle.Rows(i)("Anzahl_alt") = intAnzahl
                        End If
                    Next
                Next
                tmpView.RowFilter = ""
                vwTemp.RowFilter = ""
            End If

            Return True
        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = ex.Message
            Return False
        End Try
    End Function

    Private Function GetDetailsSingle(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, ByVal blnSperre As Boolean) As DataTable
        m_strClassAndMethod = "SIXT_PDI.GetDetailsSingle"

        Dim strSperre As String = " "
        If blnSperre Then
            strSperre = "X"
        End If

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If
        m_intIDSAP = -1

        Dim tblReturn As DataTable = Nothing
        Try
            m_intStatus = 0

            If CheckCustomerData() Then
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_MELDUNGEN_PDI1", m_objApp, m_objUser, Page)

                'myProxy.setImportParameter("I_KUNNR", m_strCustomer)
                'myProxy.setImportParameter("I_VKORG", "1510")
                'myProxy.setImportParameter("I_ZZCARPORT", m_strPDINummerSuche)

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_MELDUNGEN_PDI1", "I_KUNNR, I_VKORG, I_ZZCARPORT", m_strCustomer, "1510", m_strPDINummerSuche)

                'tblReturn = myProxy.getExportTable("GT_WEB")
                'Dim tblMeldungen As DataTable = myProxy.getExportTable("GT_TXT")

                tblReturn = S.AP.GetExportTable("GT_WEB")
                Dim tblMeldungen As DataTable = S.AP.GetExportTable("GT_TXT")

                tblReturn.Columns.Add("Bemerkung", System.Type.GetType("System.String"))
                tblReturn.Columns.Add("Limo", System.Type.GetType("System.Boolean"))
                tblReturn.Columns.Add("Kennzeichen2zeilig", System.Type.GetType("System.Boolean"))

                If tblReturn.Rows.Count > 0 Then
                    Dim objZulassungsVorbelegung As Base.Business.ZulassungsVorbelegung
                    Dim i As Int32

                    For i = tblReturn.Rows.Count - 1 To 0 Step -1
                        tblReturn.AcceptChanges()

                        Dim blnRemove As Boolean = False
                        If blnSperre Then
                            If Not CStr(tblReturn.Rows(i)("ZZAKTSPERRE")) = "X" Then
                                blnRemove = True
                            End If
                        Else
                            If Not CStr(tblReturn.Rows(i)("ZZAKTSPERRE")) = " " Then
                                blnRemove = True
                            End If
                        End If

                        If blnRemove Then
                            tblReturn.Rows(i).Delete()
                        Else
                            'Aus Ordernummer VM -> j/n ermitteln
                            objZulassungsVorbelegung = New Base.Business.ZulassungsVorbelegung(CStr(tblReturn.Rows(i)("ZZFAHRG")), Now, m_objApp.Connectionstring)
                            'Aus Fahrgestellnummer Limo und Kennzeichen2zeilig -> j/n ermitteln
                            tblReturn.Rows(i)("Limo") = objZulassungsVorbelegung.Limo
                            tblReturn.Rows(i)("Kennzeichen2zeilig") = objZulassungsVorbelegung.Kennzeichen2zeilig
                        End If

                        tblReturn.AcceptChanges()
                    Next

                    Dim vwTemp As DataView = tblMeldungen.DefaultView
                    vwTemp.Sort = "QMNUM, LFDNUM"
                    For i = 0 To tblReturn.Rows.Count - 1
                        vwTemp.RowFilter = "QMNUM = '" & tblReturn.Rows(i)("QMNUM").ToString & "'"
                        tblReturn.AcceptChanges()
                        tblReturn.Rows(i)("Bemerkung") = ""
                        If Not vwTemp.Count = 0 Then
                            Dim j As Int32
                            For j = 0 To vwTemp.Count - 1
                                tblReturn.Rows(i)("Bemerkung") = tblReturn.Rows(i)("Bemerkung").ToString & vwTemp(j)("TDLINE").ToString & " "
                            Next
                        End If
                        tblReturn.AcceptChanges()
                    Next
                    vwTemp.RowFilter = ""
                End If

                WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZCARPORT=" & m_strPDINummer & ", ZZAKTSPERRE=" & strSperre, tblReturn)
            End If
        Catch ex As Exception
            Select Case ex.Message
                Case "NO_DATA"
                Case "NO_WEB"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
            tblReturn = Nothing
            WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZCARPORT=" & m_strPDINummer & ", ZZAKTSPERRE=" & strSperre & " , " & Replace(m_strMessage, "<br>", " "), tblReturn)
        Finally
        End Try

        Return tblReturn
    End Function

    Private Function GetDetailsSingleAll(ByVal blnSperre As Boolean) As DataTable
        m_strClassAndMethod = "SIXT_PDI.GetDetailsSingle"

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If
        m_intIDSAP = -1
        Dim strSperre As String
        If blnSperre Then
            strSperre = "X"
        Else
            strSperre = " "
        End If

        Dim tblReturn As DataTable = Nothing
        Try
            m_intStatus = 0

            If CheckCustomerData() Then

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                tblReturn = m_tblPDIs

                tblReturn.Columns.Add("Bemerkung", System.Type.GetType("System.String"))
                tblReturn.Columns.Add("Limo", System.Type.GetType("System.Boolean"))
                tblReturn.Columns.Add("Kennzeichen2zeilig", System.Type.GetType("System.Boolean"))

                If tblReturn.Rows.Count > 0 Then
                    Dim objZulassungsVorbelegung As Base.Business.ZulassungsVorbelegung
                    Dim i As Int32

                    For i = 0 To tblReturn.Rows.Count - 1
                        tblReturn.AcceptChanges()

                        'Aus Ordernummer VM -> j/n ermitteln
                        If CStr(tblReturn.Rows(i)("Liznr")) = "0523501122" Then
                            'i = i + 1 - 1          'BUG!!!!!!!!!!!
                        End If

                        objZulassungsVorbelegung = New Base.Business.ZulassungsVorbelegung(CStr(tblReturn.Rows(i)("Fahrgestellnr")), Now, m_objApp.Connectionstring)
                        tblReturn.Rows(i)("Limo") = objZulassungsVorbelegung.Limo
                        tblReturn.Rows(i)("Kennzeichen2zeilig") = objZulassungsVorbelegung.Kennzeichen2zeilig

                        tblReturn.AcceptChanges()
                    Next

                    Dim vwTemp As DataView = tblReturn.DefaultView
                    vwTemp.Sort = "QMNUM"
                End If

                WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZCARPORT=" & m_strPDINummer & ", ZZAKTSPERRE=" & strSperre, tblReturn)
            End If
        Catch ex As Exception
            Select Case ex.Message
                Case "NO_DATA"
                Case "NO_WEB"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
            End If
            tblReturn = Nothing
            WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZCARPORT=" & m_strPDINummer & ", ZZAKTSPERRE=" & strSperre & " , " & Replace(m_strMessage, "<br>", " "), tblReturn)
        Finally
            If m_intIDSAP > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
            End If
        End Try

        Return tblReturn
    End Function

    Public Function SelectCars() As Int32
        Try
            m_dsPDI_Data_Selected = m_dsPDI_Data.Copy
            Dim i As Int32
            Dim vwTemp As DataView = m_dsPDI_Data_Selected.Tables("Fahrzeuge").DefaultView
            vwTemp.RowFilter = "Ausgewaehlt = True"
            Dim intReturn As Int32 = vwTemp.Count
            vwTemp.RowFilter = ""

            If intReturn > 0 Then
                For i = m_dsPDI_Data_Selected.Tables("Fahrzeuge").Rows.Count - 1 To 0 Step -1
                    If Not CType(m_dsPDI_Data_Selected.Tables("Fahrzeuge").Rows(i)("Ausgewaehlt"), Boolean) Then
                        m_dsPDI_Data_Selected.Tables("Fahrzeuge").AcceptChanges()
                        m_dsPDI_Data_Selected.Tables("Fahrzeuge").Rows(i).Delete()
                        m_dsPDI_Data_Selected.Tables("Fahrzeuge").AcceptChanges()
                    End If
                Next
                For i = m_dsPDI_Data_Selected.Tables("Modelle").Rows.Count - 1 To 0 Step -1
                    vwTemp = m_dsPDI_Data_Selected.Tables("Fahrzeuge").DefaultView
                    vwTemp.RowFilter = "Modell_ID = " & m_dsPDI_Data_Selected.Tables("Modelle").Rows(i)("ID").ToString
                    If vwTemp.Count = 0 Then
                        m_dsPDI_Data_Selected.Tables("Modelle").AcceptChanges()
                        m_dsPDI_Data_Selected.Tables("Modelle").Rows(i).Delete()
                        m_dsPDI_Data_Selected.Tables("Modelle").AcceptChanges()
                    Else
                        m_dsPDI_Data_Selected.Tables("Modelle").Rows(i)("Anzahl_neu") = vwTemp.Count
                    End If
                    vwTemp.RowFilter = ""
                Next
                For i = m_dsPDI_Data_Selected.Tables("PDIs").Rows.Count - 1 To 0 Step -1
                    vwTemp = m_dsPDI_Data_Selected.Tables("Modelle").DefaultView
                    vwTemp.RowFilter = "PDI_Nummer = '" & m_dsPDI_Data_Selected.Tables("PDIs").Rows(i)("PDI_Nummer").ToString & "'"
                    If vwTemp.Count = 0 Then
                        m_dsPDI_Data_Selected.Tables("PDIs").AcceptChanges()
                        m_dsPDI_Data_Selected.Tables("PDIs").Rows(i).Delete()
                        m_dsPDI_Data_Selected.Tables("PDIs").AcceptChanges()
                    End If
                    vwTemp.RowFilter = ""
                Next
            End If

            m_intSelectedCars = intReturn
        Catch ex As Exception
            m_intSelectedCars = -1
        End Try
        Return m_intSelectedCars
    End Function

    Public Overrides Sub Change()

    End Sub

    Public Sub setZulassung(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        'Zulassen
        m_strClassAndMethod = "SIXT_PDI.setZulassung"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        Dim m_zulOutPutTable As DataTable
        FahrzeugeSperren(Result, m_strAppID, SessionID, page)

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Massenzulassung", m_objApp, m_objUser, page)
            S.AP.Init("Z_Massenzulassung")

            'Dim zulSpeicherTable As DataTable = myProxy.getImportTable("INTERNTAB")
            Dim zulSpeicherTable As DataTable = S.AP.GetImportTable("INTERNTAB")

            Dim zulSpeicher As DataRow

            Dim zulOutPut As DataRow
            'Alle FAhrzeuge durchgehen.....
            m_intStatus = 0
            m_intErrorCount = 0
            m_strMessage = ""
            m_blnShowBelegnummer = False

            Dim i As Int32
            Dim vwTemp As DataView = m_tblFahrzeuge.DefaultView
            vwTemp.RowFilter = "Ausgewaehlt = True"

            If vwTemp.Count > 0 Then
                For i = 0 To m_tblFahrzeuge.Rows.Count - 1
                    If CType(m_tblFahrzeuge.Rows(i)("Ausgewaehlt"), Boolean) Then
                        Dim datDate As DateTime = CType(m_tblFahrzeuge.Rows(i)("DatumErstzulassung"), DateTime)
                        Dim objZulassungsvorbelegung As Base.Business.ZulassungsVorbelegung
                        m_tblFahrzeuge.Rows(i).AcceptChanges()

                        objZulassungsvorbelegung = New Base.Business.ZulassungsVorbelegung(m_tblFahrzeuge.Rows(i)("Fahrgestellnummer").ToString, datDate, m_objApp.Connectionstring)
                        'End If
                        If objZulassungsvorbelegung.Halter_SAPNr.Length = 0 Then
                            m_intErrorCount += 1
                            m_tblFahrzeuge.Rows(i)("Belegnummer") = "Fehler beim Speichern.<br>Konnte Halter nicht ermitteln."
                        Else
                            If objZulassungsvorbelegung.Versicherer_SAPNr.Length = 0 Then
                                m_intErrorCount += 1
                                m_tblFahrzeuge.Rows(i)("Belegnummer") = "Fehler beim Speichern.<br>Konnte Versicherer nicht ermitteln."
                            Else
                                If objZulassungsvorbelegung.KBANR.Length = 0 Then
                                    m_intErrorCount += 1
                                    m_tblFahrzeuge.Rows(i)("Belegnummer") = "Fehler beim Speichern.<br>Konnte Zusatzinformation nicht ermitteln."
                                Else
                                    Dim strZZSONDER As String = " "
                                    If objZulassungsvorbelegung.Kennzeichen2zeilig Then
                                        strZZSONDER = "Z"
                                    End If
                                    If objZulassungsvorbelegung.Limo Then
                                        strZZSONDER = "L"
                                    End If
                                    Try
                                        zulSpeicher = zulSpeicherTable.NewRow
                                        With zulSpeicher
                                            .Item("I_Kunnr_Ag") = m_strCustomer
                                            .Item("I_Zzfahrg") = m_tblFahrzeuge.Rows(i)("Fahrgestellnummer").ToString
                                            .Item("I_Edatu") = Format(datDate, "dd.MM.yyyy")
                                            .Item("I_Kunnr_Zv") = objZulassungsvorbelegung.Versicherer_SAPNr.PadLeft(10, "0"c)
                                            .Item("I_Zzkennz") = String.Empty
                                            .Item("I_Kunnr_Zh") = objZulassungsvorbelegung.Halter_SAPNr.PadLeft(10, "0"c)
                                            .Item("I_Kunnr_Za") = String.Empty
                                            .Item("I_Zzsonder") = strZZSONDER
                                            .Item("I_Kbanr") = objZulassungsvorbelegung.KBANR
                                            .Item("I_Zzcarport") = m_tblFahrzeuge.Rows(i)("DADPDI").ToString
                                        End With
                                        zulSpeicherTable.Rows.Add(zulSpeicher)
                                        m_tblFahrzeuge.Rows(i)("Belegnummer") = "Daten wurden gesendet."
                                    Catch ex As Exception
                                        'Fehler...
                                        Select Case ex.Message
                                            Case "SCHON_ZUGELASSEN"
                                                m_intStatus = -1234
                                                m_strMessage = "Fehler! Fahrzeug bereits zugelassen."
                                            Case "GESPERRT"
                                                m_intStatus = -1235
                                                m_strMessage = "Fehler! Fahrzeug gesperrt"
                                            Case "VERSCHOBEN"
                                                m_intStatus = -1236
                                                m_strMessage = "Fehler! Fahrzeug verschoben."
                                            Case Else
                                                m_intStatus = -9999
                                                m_strMessage = "Fehler! Unbekannter Fehler."
                                        End Select
                                        m_intErrorCount += 1
                                        m_tblFahrzeuge.Rows(i)("Belegnummer") = "Fehler beim Speichern. (" & ex.Message & ")"
                                        WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
                                    End Try
                                End If
                            End If
                        End If
                    End If
                Next

            End If
            If zulSpeicherTable.Rows.Count > 0 Then
                Try
                    Dim anzahl As String = ""
                    Dim sReturn As String = ""

                    'myProxy.callBapi()
                    S.AP.Execute()

                    'sReturn = myProxy.getExportParameter("RETURN")
                    'anzahl = myProxy.getExportParameter("ANZAHL")
                    'm_zulOutPutTable = myProxy.getExportTable("OUTPUT")

                    sReturn = S.AP.GetExportParameter("RETURN")
                    anzahl = S.AP.GetExportParameter("ANZAHL")
                    m_zulOutPutTable = S.AP.GetExportTable("OUTPUT")

                    If sReturn = "OK" Then
                        m_intStatus = 0
                        m_tblFahrzeuge.AcceptChanges()
                        For Each zulOutPut In m_zulOutPutTable.Rows
                            m_tblFahrzeuge.Select("Fahrgestellnummer='" & zulOutPut("Id").ToString & "'")(0)("Belegnummer") = zulOutPut("Message").ToString
                            If zulOutPut("Message").ToString.Length > 0 Then
                                m_intErrorCount += 1
                            End If
                        Next
                        m_tblFahrzeuge.AcceptChanges()
                        m_strMessage = String.Format("Es wurden {0} Fahrzeuge zugelassen.", anzahl.Trim)
                    Else
                        m_intStatus = -9999
                        For i = 0 To m_tblFahrzeuge.Rows.Count - 1
                            If CType(m_tblFahrzeuge.Rows(i)("Ausgewaehlt"), Boolean) Then
                                m_intErrorCount += 1
                                m_tblFahrzeuge.Rows(i)("Belegnummer") = "Fehler beim Speichern."
                            End If
                        Next
                        m_tblFahrzeuge.AcceptChanges()
                        m_strMessage = "Es ist ein Fehler aufgetreten.<br>Daten wurden nicht übermittelt."
                    End If

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = String.Concat("Es ist ein Fehler aufgetreten.<br>", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                End Try
            End If
            CreateErledigtTable()
            m_blnShowBelegnummer = True
            vwTemp.RowFilter = ""
        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = "Unbekannter Fehler."
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub

    Public Sub setSperre(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page, ByVal strSperre As String)
        'Sperren/Entsperren
        m_strClassAndMethod = "SIXT_PDI.setSperre"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Dim strEquNr As String = ""
        Dim strQmNr As String = ""
        Dim strResult As String

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Zulassungssperre", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_Zulassungssperre")

            'Dim tblTextSAP As DataTable = myProxy.getImportTable("GT_TEXTE")
            Dim tblTextSAP As DataTable = S.AP.GetImportTable("GT_TEXTE")

            Dim rowTextSAP As DataRow

            Dim fahrzeugRows() As DataRow = m_tblFahrzeuge.Select("Ausgewaehlt = True")

            If fahrzeugRows.Length > 0 Then

                For Each tmpRow As DataRow In fahrzeugRows
                    Try
                        rowTextSAP = tblTextSAP.NewRow
                        rowTextSAP("Tdline") = tmpRow("Bemerkung").ToString
                        tblTextSAP.Rows.Add(rowTextSAP)
                        'myProxy.setImportParameter("I_ZZFAHRG", tmpRow("Fahrgestellnummer").ToString)
                        'myProxy.setImportParameter("I_ZZCARPORT", tmpRow("DADPDI").ToString)
                        'myProxy.setImportParameter("I_ZZDATBEM", tmpRow("BemerkungDatum").ToString)
                        'myProxy.setImportParameter("I_KUNNR", m_strCustomer)
                        'myProxy.setImportParameter("I_ZZAKTSPERRE", strSperre)

                        'myProxy.callBapi()

                        S.AP.SetImportParameter("I_ZZFAHRG", tmpRow("Fahrgestellnummer").ToString)
                        S.AP.SetImportParameter("I_ZZCARPORT", tmpRow("DADPDI").ToString)
                        S.AP.SetImportParameter("I_ZZDATBEM", tmpRow("BemerkungDatum").ToString)
                        S.AP.SetImportParameter("I_KUNNR", m_strCustomer)
                        S.AP.SetImportParameter("I_ZZAKTSPERRE", strSperre)

                        S.AP.Execute()

                        'strResult = myProxy.getExportParameter("E_SUBRC")
                        'strEquNr = myProxy.getExportParameter("E_EQUNR")
                        'strQmNr = myProxy.getExportParameter("E_QMNUM")

                        strResult = S.AP.GetExportParameter("E_SUBRC")
                        strEquNr = S.AP.GetExportParameter("E_EQUNR")
                        strQmNr = S.AP.GetExportParameter("E_QMNUM")

                        If strResult.Trim = "0" Then
                            If strSperre = "X" Then
                                tmpRow("Belegnummer") = "Fahrzeug gesperrt."
                            Else
                                tmpRow("Belegnummer") = "Fahrzeug entsperrt."
                            End If

                        Else
                            m_intErrorCount += 1
                            tmpRow("Belegnummer") = "Fehler beim Speichern."
                        End If
                    Catch ex As Exception
                        m_intErrorCount += 1
                        tmpRow("Belegnummer") = "Fehler beim Speichern. (" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                    End Try

                Next
                m_tblFahrzeuge.AcceptChanges()
            Else
                m_intStatus = -9999
                m_strMessage = "Keine Fahrzeuge ausgewählt."
            End If


            CreateErledigtTable()
            m_blnShowBelegnummer = True

        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = "Unbekannter Fehler."
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub

    Public Sub Verschieben(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        'Sperren/Entsperren
        m_strClassAndMethod = "SIXT_PDI.setSperre"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Dim strEquNr As String = ""
        Dim strQmNr As String = ""

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_PDIWECHSEL", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_PDIWECHSEL")

            'Dim tblTextSAP As DataTable = myProxy.getImportTable("ZZBEMERKUNG")
            Dim tblTextSAP As DataTable = S.AP.GetImportTable("ZZBEMERKUNG")

            Dim rowTextSAP As DataRow

            Dim fahrzeugRows() As DataRow = m_tblFahrzeuge.Select("Ausgewaehlt = True")

            If fahrzeugRows.Length > 0 Then

                For Each tmpRow As DataRow In fahrzeugRows
                    Try
                        rowTextSAP = tblTextSAP.NewRow
                        rowTextSAP("Tdline") = tmpRow("Bemerkung").ToString
                        tblTextSAP.Rows.Add(rowTextSAP)

                        'myProxy.setImportParameter("ZZQMNUM", tmpRow("Meldungsnummer").ToString)
                        'myProxy.setImportParameter("ZZCARPORT", tmpRow("ZielPDI").ToString)
                        'myProxy.setImportParameter("I_ZZCARPORT", tmpRow("DADPDI").ToString)
                        'myProxy.setImportParameter("I_ZZDATBEM", tmpRow("BemerkungDatum").ToString)
                        'myProxy.setImportParameter("ZZKUNNR", m_strCustomer)

                        'myProxy.callBapi()

                        S.AP.SetImportParameter("ZZQMNUM", tmpRow("Meldungsnummer").ToString)
                        S.AP.SetImportParameter("ZZCARPORT", tmpRow("ZielPDI").ToString)
                        S.AP.SetImportParameter("I_ZZCARPORT", tmpRow("DADPDI").ToString)
                        S.AP.SetImportParameter("I_ZZDATBEM", tmpRow("BemerkungDatum").ToString)
                        S.AP.SetImportParameter("ZZKUNNR", m_strCustomer)

                        S.AP.Execute()

                        tmpRow("Belegnummer") = "Fahrzeug verschoben."

                    Catch ex As Exception
                        m_intErrorCount += 1
                        tmpRow("Belegnummer") = "Fehler beim Speichern. (" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                    End Try


                Next
                m_tblFahrzeuge.AcceptChanges()
            Else
                m_intStatus = -9999
                m_strMessage = "Keine Fahrzeuge ausgewählt."
            End If


            CreateErledigtTable()
            m_blnShowBelegnummer = True

        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = "Unbekannter Fehler."
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub

    Private Sub CreateErledigtTable()
        Dim i As Integer
        m_tblErledigt = New DataTable()
        m_tblErledigt.Columns.Add("Aufgabe", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("PDI Nummer", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("PDI Name", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("Hersteller", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("Modellbezeichnung", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("Schaltung", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("Ausfuehrung", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("Antrieb", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("Bereifung", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("Navigation", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("Beklebung", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("Eingangsdatum", System.Type.GetType("System.DateTime"))
        m_tblErledigt.Columns.Add("Bemerkung", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("Datum Erstzulassung", System.Type.GetType("System.DateTime"))
        m_tblErledigt.Columns.Add("Ziel PDI", System.Type.GetType("System.String"))
        m_tblErledigt.Columns.Add("Belegnummer", System.Type.GetType("System.String"))

        For i = 0 To m_tblFahrzeuge.Rows.Count - 1
            If CType(m_tblFahrzeuge.Rows(i)("Ausgewaehlt"), Boolean) Then

                Dim rowTemp As DataRow
                rowTemp = m_tblErledigt.NewRow
                rowTemp("Aufgabe") = m_strTask
                rowTemp("Belegnummer") = m_tblFahrzeuge.Rows(i)("Belegnummer")
                rowTemp("Ziel PDI") = m_tblFahrzeuge.Rows(i)("ZielPDI")
                rowTemp("Datum Erstzulassung") = m_tblFahrzeuge.Rows(i)("DatumErstzulassung")
                rowTemp("Bemerkung") = m_tblFahrzeuge.Rows(i)("Bemerkung")
                rowTemp("Eingangsdatum") = m_tblFahrzeuge.Rows(i)("Eingangsdatum")
                rowTemp("Fahrgestellnummer") = m_tblFahrzeuge.Rows(i)("Fahrgestellnummer")

                Dim vwTemp2 As DataView = m_tblModelle.DefaultView
                vwTemp2.RowFilter = "ID = " & m_tblFahrzeuge.Rows(i)("Modell_ID").ToString
                rowTemp("PDI Nummer") = vwTemp2.Item(0)("PDI_Nummer")

                rowTemp("PDI Name") = vwTemp2.Item(0)("PDI_Name")
                rowTemp("Hersteller") = vwTemp2.Item(0)("Hersteller")
                rowTemp("Modellbezeichnung") = vwTemp2.Item(0)("Modellbezeichnung")
                rowTemp("Schaltung") = vwTemp2.Item(0)("Schaltung")
                rowTemp("Ausfuehrung") = vwTemp2.Item(0)("Ausfuehrung")
                rowTemp("Antrieb") = vwTemp2.Item(0)("Antrieb")
                rowTemp("Bereifung") = vwTemp2.Item(0)("Bereifung")
                rowTemp("Navigation") = vwTemp2.Item(0)("Navigation")
                If CType(vwTemp2.Item(0)("Beklebung"), Boolean) Then
                    rowTemp("Beklebung") = "Ja"
                Else
                    rowTemp("Beklebung") = "Nein"
                End If

                vwTemp2.RowFilter = ""

                m_tblErledigt.Rows.Add(rowTemp)
            End If
        Next
    End Sub

    Public Sub FahrzeugeSperren(ByVal tblFahrzeuge As DataTable, ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        Dim rows As DataRow()
        Dim i As Integer

        Try
            m_intStatus = 0
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Warenkorb_Sperre", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_Warenkorb_Sperre")

            'Dim tblFahrzeugeSAP As DataTable = myProxy.getImportTable("GT_IN")
            Dim tblFahrzeugeSAP As DataTable = S.AP.GetImportTable("GT_IN")

            Dim tblFahrzeugeSAPRow As DataRow


            'Fahrzeuge übernehmen
            rows = tblFahrzeuge.Select("Ausgewaehlt = True")

            For i = 0 To rows.Length - 1
                tblFahrzeugeSAPRow = tblFahrzeugeSAP.NewRow
                tblFahrzeugeSAPRow("Equnr") = CType(rows(i)("EQUNR"), String)
                tblFahrzeugeSAP.Rows.Add(tblFahrzeugeSAPRow)
            Next

            'SAP-Aufruf
            'myProxy.callBapi()
            S.AP.Execute()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, tblFahrzeuge)
        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & m_strMessage, tblFahrzeuge)
        End Try
    End Sub

    Public Sub setKOPFDATEN(ByVal arrKopfdaten As ArrayList)
        Dim tempRow As DataRow
        Dim strKennz As String = CType(arrKopfdaten(32), String)

        data = arrKopfdaten
        For Each tempRow In m_tblModelle.Rows
            tempRow("OrtsKennz") = strKennz
        Next
        For Each tempRow In m_tblFahrzeuge.Rows
            tempRow("OrtsKennz") = strKennz
        Next
    End Sub

    Public Shared Function getFlag(ByVal id_group As Int32) As Int32
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As New SqlClient.SqlCommand()
        Dim sql_select As String
        Dim flag As Int32

        sql_select = "SELECT min(flag1) FROM Flags WHERE id_group = @id"

        command.Parameters.AddWithValue("@id", id_group)
        command.Connection = conn
        command.CommandText = sql_select
        Try
            conn.Open()
            flag = CType(command.ExecuteScalar(), Int32) ' .ExecuteScalar()
        Catch ex As Exception
            flag = -1
        Finally
            conn.Close()
        End Try
        Return flag
    End Function
#End Region

End Class

' ************************************************
' $History: SIXT_PDI.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:38
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 29  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 28  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
