Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.SqlClient
Imports CKG.Base.Common

Public Class CarRent01
    Inherits BankBase

#Region " Declarations"
    Private m_intErrorCount As Int32
    Private m_objABE_Daten As ABE2FHZG
    Private m_dsPDI_Data As DataSet
    Private m_dsPDI_Data_Selected As DataSet
    Private m_tblPDIs As DataTable
    Private m_tblAllPDIs As DataTable
    Private m_tblAllMODs As DataTable
    Private m_tblModelle As DataTable
    Private m_tblFahrzeuge As DataTable
    Private m_zulOutPutTable As DataTable
    Private m_tblSaveCars As DataTable
    Private m_tblVersicherer As DataTable
    Private m_tblDeckungskarten As DataTable
    Private m_strPDINummer As String
    Private m_strPDINummerSuche As String
    Private m_strModellCode As String
    Private m_strFahrgestellnummer As String
    Private m_intItemIndex As Int32
    Private m_intLastID As Int32
    Private m_strTask As String
    Private m_intSelectedCars As Int32
    Private m_blnShowBelegnummer As Boolean
    Private m_intFahrzeugeGesamtZulassungsf As Int32
    Private m_intFahrzeugeGesamtGesperrt As Int32
    Private m_tblErledigt As DataTable
    Private data As ArrayList
    Private m_pdiListe As DataTable
    Private returnList As ArrayList

    Private m_tblKennzeichenserie As DataTable
    Private m_tblVerwendung As DataTable
    Private m_tblFahrzeugeText As DataTable
    Private gridMain As DataGrid
    Private m_strPDISuche As String
    Private m_phase As Char

    Private m_selectedPDI As String
    Private m_selectedMOD As String

    Private m_blnSelectedPDI As Boolean
    Private m_blnSelectedMOD As Boolean
    Private m_Sperre As Char

    Private m_ZulArt As Integer
    Private m_Zulassungskreise As DataTable
    Private m_ZulKreis As String
    Private m_Zentral As Boolean = True
    Private m_ZulKreisID As String
    Private Const strTaskZulassen As String = "Zulassen"
    Private Const strTaskSperren As String = "Sperren"
    Private Const strTaskEntsperren As String = "Entsperren"
    Private Const strTaskVerschieben As String = "Verschieben"



    Private Enum Zulassungsart As Integer

        UngueltigeAuswahl = 0
        Zentral = 1
        Dezentral = 2
        Beides = 3
    End Enum

    'DEZENTRAL


#End Region

#Region " Properties"
    Public Property PDIListe() As DataTable
        Get
            Return m_pdiListe
        End Get
        Set(ByVal Value As DataTable)
            m_pdiListe = Value
        End Set
    End Property

    Public ReadOnly Property PTexte() As DataTable
        Get
            Return m_tblFahrzeugeText
        End Get
    End Property

    Public Property PSperre() As Char
        Get
            Return m_Sperre
        End Get
        Set(ByVal Value As Char)
            m_Sperre = Value
        End Set
    End Property

    Public Property PPhase() As Char
        Get
            Return m_phase
        End Get
        Set(ByVal Value As Char)
            m_phase = Value
        End Set
    End Property

    Public Property PSelectionPDI() As Boolean
        Get
            Return m_blnSelectedPDI
        End Get
        Set(ByVal Value As Boolean)
            m_blnSelectedPDI = Value
        End Set
    End Property

    Public Property PSelectionMOD() As Boolean
        Get
            Return m_blnSelectedMOD
        End Get
        Set(ByVal Value As Boolean)
            m_blnSelectedMOD = Value
        End Set
    End Property

    Public Property PSelectedMOD() As String
        Get
            Return m_selectedMOD
        End Get
        Set(ByVal Value As String)
            m_selectedMOD = Value
        End Set
    End Property


    Public Property PSelectedPDI() As String
        Get
            Return m_selectedPDI
        End Get
        Set(ByVal Value As String)
            m_selectedPDI = Value
        End Set
    End Property

    Public Property PPDISuche() As String
        Get
            Return m_strPDISuche
        End Get
        Set(ByVal Value As String)
            m_strPDISuche = Value
        End Set
    End Property


    Public Property PGrid() As DataGrid
        Get
            Return gridMain
        End Get
        Set(ByVal Value As DataGrid)
            gridMain = Value
        End Set
    End Property

    Public ReadOnly Property PVerwendung() As DataTable
        Get
            Return m_tblVerwendung
        End Get
    End Property

    Public ReadOnly Property PKennzeichenSerie() As DataTable
        Get
            Return m_tblKennzeichenserie
        End Get
    End Property

    Public Property OutPutTable() As DataTable
        Get
            Return m_zulOutPutTable
        End Get
        Set(ByVal Value As DataTable)
            m_zulOutPutTable = Value
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

    Public Property ABE_Daten() As ABE2FHZG
        Get
            Return m_objABE_Daten
        End Get
        Set(ByVal Value As ABE2FHZG)
            m_objABE_Daten = Value
        End Set
    End Property

    Public ReadOnly Property AllPDIs() As DataTable
        Get
            Return m_tblAllPDIs
        End Get
    End Property

    Public ReadOnly Property AllMODs() As DataTable
        Get
            Return m_tblAllMODs
        End Get
    End Property


    Public ReadOnly Property Versicherer() As DataTable
        Get
            Return m_tblVersicherer
        End Get
    End Property

    Public ReadOnly Property Deckungskarten() As DataTable
        Get
            Return m_tblDeckungskarten
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

    Public Property Task() As String
        Get
            Return m_strTask
        End Get
        Set(ByVal Value As String)
            m_strTask = Value
        End Set
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


    Public Property intItemIndex() As Int32
        Get
            Return m_intItemIndex
        End Get
        Set(ByVal Value As Int32)
            m_intItemIndex = Value
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

    Public Property ZulArt() As Integer
        Get
            Return m_ZulArt
        End Get
        Set(ByVal Value As Integer)
            m_ZulArt = Value
        End Set
    End Property

    Public Property Zulassungskreise() As DataTable
        Get
            Return m_Zulassungskreise
        End Get
        Set(ByVal Value As DataTable)
            m_Zulassungskreise = Value
        End Set
    End Property

    Public Property Zentral() As Boolean
        Get
            Return m_Zentral
        End Get
        Set(ByVal Value As Boolean)
            m_Zentral = Value
        End Set
    End Property

    Public Property ZulKreis() As String
        Get
            Return m_ZulKreis
        End Get
        Set(ByVal Value As String)
            m_ZulKreis = Value
        End Set
    End Property
    Public Property ZulKreisID() As String
        Get
            Return m_ZulKreisID
        End Get
        Set(ByVal Value As String)
            m_ZulKreisID = Value
        End Set
    End Property
#End Region

    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

    End Sub

    Public Overrides Sub Show()

    End Sub
    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub getCars(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        Try
            Dim row As DataRow
            m_intStatus = 0
            m_strMessage = ""
            'weil bei bankbase führende nullen angehängt werden, verträgt das bapi aber nicht wenn ohne Händlernummer gesucht wird.
            Dim m_strKUNNR As String
            m_strKUNNR = Right("0000000000" & m_objUser.Customer.KUNNR, 10)


            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_MELD_PDI_ZENTR_ZUL_001", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR", m_strKUNNR)
            myProxy.setImportParameter("I_VKORG", "1510")

            myProxy.callBapi()

            Dim tblFahrzeugeSAP As DataTable = myProxy.getExportTable("GT_WEB")
            m_tblVersicherer = myProxy.getExportTable("GT_VERS")

            m_tblFahrzeuge = New DataTable()
            m_tblFahrzeuge = tblFahrzeugeSAP.Copy

            With m_tblFahrzeuge.Columns
                .Add("Art", GetType(System.String))   'Gruppierungstyp: PDI, Modell, Fahrzeug
                .Add("Anzahl", GetType(System.Int32))
                .Add("RowID", GetType(System.String))   'ID der Zeile
                .Add("SelectedDate", GetType(System.String))            'Eingetragenes Zulassungsdatum
                .Add("SelectedEinzel", GetType(System.Boolean))           'Eingetragene Fahrzeuganzahl
                .Add("SelectedAnzahl", GetType(System.String))           'Eingetragene Fahrzeuganzahl
                .Add("PDIAnzahl", GetType(System.String))
                .Add("MODAnzahl", GetType(System.String))
                .Add("SelectedAlle", GetType(System.String))             'Werte übernehmen?
                .Add("Status", GetType(System.String))             'Werte übernehmen?
                .Add("SelectedZielPDI", GetType(System.String))
                .Add("SelectedZielPDIText", GetType(System.String))
                .Add("Eingangsdatum", GetType(System.DateTime))
                .Add("Versicherer", GetType(System.String))
                .Add("Belegnummer", GetType(System.String))
            End With

            m_tblFahrzeuge.AcceptChanges()

            For Each row In m_tblFahrzeuge.Rows
                row("RowID") = CStr(row("EQUNR"))
                row("Art") = "CAR"
                row("SelectedDate") = String.Empty                        'Eingetragenes Zulassungsdatum
                row("SelectedEinzel") = False                                 'Eingentragene Fahrzeuganzahl
                row("SelectedAlle") = ""
                row("PDIAnzahl") = String.Empty
                row("MODAnzahl") = String.Empty
                row("SelectedAnzahl") = ""
                row("Status") = String.Empty
                row("SelectedZielPDI") = String.Empty
                row("SelectedZielPDIText") = String.Empty
                row("Eingangsdatum") = row("ZZDAT_EIN").ToString
                row("Versicherer") = String.Empty
                m_tblFahrzeuge.AcceptChanges()
            Next
            m_tblResult = m_tblFahrzeuge

            m_intStatus = 0


        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -2501
                    m_strMessage = "Es wurden keine Daten gefunden."
                Case "NO_WEB"
                    m_intStatus = -2502
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select


        End Try
    End Sub

    Public Overloads Sub getCarsDezentral(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page, ByVal Kreis As String)

        Try
            Dim row As DataRow
            m_intStatus = 0
            m_strMessage = ""
            'weil bei bankbase führende nullen angehängt werden, verträgt das bapi aber nicht wenn ohne Händlernummer gesucht wird.
            Dim m_strKUNNR As String
            m_strKUNNR = Right("0000000000" & m_objUser.Customer.KUNNR, 10)


            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_MELD_PDI_DEZ_ZUL_001", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR", m_strKUNNR)
            myProxy.setImportParameter("I_VKORG", "1510")

            myProxy.callBapi()

            Dim tblFahrzeugeSAP As DataTable = myProxy.getExportTable("GT_WEB")
            m_tblVersicherer = myProxy.getExportTable("GT_VERS")

            m_tblFahrzeuge = New DataTable()
            m_tblFahrzeuge = tblFahrzeugeSAP.Copy


            m_tblFahrzeuge.DefaultView.RowFilter = "ZKFZKZ='" & Kreis & "'"


            m_tblFahrzeuge = m_tblFahrzeuge.DefaultView.ToTable

            With m_tblFahrzeuge.Columns
                .Add("Art", GetType(System.String))   'Gruppierungstyp: PDI, Modell, Fahrzeug
                .Add("Anzahl", GetType(System.Int32))
                .Add("RowID", GetType(System.String))   'ID der Zeile
                .Add("SelectedDate", GetType(System.String))            'Eingetragenes Zulassungsdatum
                .Add("SelectedEinzel", GetType(System.Boolean))           'Eingetragene Fahrzeuganzahl
                .Add("SelectedAnzahl", GetType(System.String))           'Eingetragene Fahrzeuganzahl
                .Add("PDIAnzahl", GetType(System.String))
                .Add("MODAnzahl", GetType(System.String))
                .Add("SelectedAlle", GetType(System.String))             'Werte übernehmen?
                .Add("Status", GetType(System.String))             'Werte übernehmen?
                .Add("SelectedZielPDI", GetType(System.String))
                .Add("SelectedZielPDIText", GetType(System.String))
                .Add("Eingangsdatum", GetType(System.DateTime))
                .Add("Versicherer", GetType(System.String))
                .Add("Belegnummer", GetType(System.String))
            End With

            m_tblFahrzeuge.AcceptChanges()

            'm_Zulassungskreise.Columns.Add("ID", GetType(System.Int32))
            'm_Zulassungskreise.Columns.Add("Kreis", GetType(System.String))

            'm_Zulassungskreise.AcceptChanges()

            'Dim KreisRow As DataRow = m_Zulassungskreise.NewRow

            'KreisRow("ID") = 0
            'KreisRow("Kreis") = "--Auswahl--"

            'm_Zulassungskreise.AcceptChanges()

            'Dim booFound As Boolean = False
            'Dim i As Integer = 0

            For Each row In m_tblFahrzeuge.Rows
                row("RowID") = CStr(row("EQUNR"))
                row("Art") = "CAR"
                row("SelectedDate") = String.Empty                        'Eingetragenes Zulassungsdatum
                row("SelectedEinzel") = False                                 'Eingentragene Fahrzeuganzahl
                row("SelectedAlle") = ""
                row("PDIAnzahl") = String.Empty
                row("MODAnzahl") = String.Empty
                row("SelectedAnzahl") = ""
                row("Status") = String.Empty
                row("SelectedZielPDI") = String.Empty
                row("SelectedZielPDIText") = String.Empty
                row("Eingangsdatum") = row("ZZDAT_EIN").ToString
                row("Versicherer") = String.Empty
                m_tblFahrzeuge.AcceptChanges()

                ''Kreise hinzufügen

                'booFound = False

                'For Each dr As DataRow In m_Zulassungskreise.Rows

                '    If dr("Kreis").ToString = row("ZKFZKZ").ToString Then
                '        booFound = True
                '        Exit For
                '    End If

                'Next

                'If booFound = False Then
                '    i += 1
                '    KreisRow = m_Zulassungskreise.NewRow

                '    KreisRow("ID") = i
                '    KreisRow("Kreis") = row("ZKFZKZ").ToString

                '    m_Zulassungskreise.Rows.Add(KreisRow)
                '    m_Zulassungskreise.AcceptChanges()
                'End If


            Next

            m_tblResult = m_tblFahrzeuge

            m_intStatus = 0


        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -2501
                    m_strMessage = "Es wurden keine Daten gefunden."
                Case "NO_WEB"
                    m_intStatus = -2502
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select


        End Try
    End Sub


    Public Overloads Sub getKreise(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        Try
            Dim row As DataRow
            m_intStatus = 0
            m_strMessage = ""
            'weil bei bankbase führende nullen angehängt werden, verträgt das bapi aber nicht wenn ohne Händlernummer gesucht wird.
            Dim m_strKUNNR As String
            m_strKUNNR = Right("0000000000" & m_objUser.Customer.KUNNR, 10)


            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_MELD_PDI_DEZ_ZUL_001", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR", m_strKUNNR)
            myProxy.setImportParameter("I_VKORG", "1510")

            myProxy.callBapi()

            Dim tblFahrzeugeSAP As DataTable = myProxy.getExportTable("GT_WEB")

            m_tblFahrzeuge = New DataTable()
            m_tblFahrzeuge = tblFahrzeugeSAP.Copy

            m_Zulassungskreise = New DataTable

            m_Zulassungskreise.Columns.Add("ID", GetType(System.Int32))
            m_Zulassungskreise.Columns.Add("Kreis", GetType(System.String))

            m_Zulassungskreise.AcceptChanges()

            Dim KreisRow As DataRow = m_Zulassungskreise.NewRow

            KreisRow("ID") = 0
            KreisRow("Kreis") = "--Auswahl--"

            m_Zulassungskreise.Rows.Add(KreisRow)
            m_Zulassungskreise.AcceptChanges()

            Dim booFound As Boolean = False
            Dim i As Integer = 0

            For Each row In m_tblFahrzeuge.Rows

                'Kreise hinzufügen
                booFound = False

                For Each dr As DataRow In m_Zulassungskreise.Rows

                    If dr("Kreis").ToString = row("ZKFZKZ").ToString Then
                        booFound = True
                        Exit For
                    End If

                Next

                If booFound = False Then
                    i += 1
                    KreisRow = m_Zulassungskreise.NewRow

                    KreisRow("ID") = i
                    KreisRow("Kreis") = row("ZKFZKZ").ToString

                    m_Zulassungskreise.Rows.Add(KreisRow)
                    m_Zulassungskreise.AcceptChanges()
                End If


            Next

            m_tblResult = m_tblFahrzeuge

            m_intStatus = 0


        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -2501
                    m_strMessage = "Es wurden keine Daten gefunden."
                Case "NO_WEB"
                    m_intStatus = -2502
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select


        End Try
    End Sub


    Public Sub getZulassungsarten(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        m_ZulArt = Zulassungsart.UngueltigeAuswahl

        Try

            m_intStatus = 0
            m_strMessage = ""


            Dim m_strKUNNR As String
            m_strKUNNR = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_MELD_PDI_ZENTR_ZUL_001", m_objApp, m_objUser, page)

            Dim tblZentral As New DataTable

            Try


                myProxy.setImportParameter("I_KUNNR", m_strKUNNR)
                myProxy.setImportParameter("I_VKORG", "1510")

                myProxy.callBapi()

                tblZentral = myProxy.getExportTable("GT_WEB")
            Catch ex As Exception
                'Weitermachen
            Finally
                myProxy = Nothing
            End Try



            Dim myNewProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_MELD_PDI_DEZ_ZUL_001", m_objApp, m_objUser, page)

            Dim tblDezentral As New DataTable

            Try
                myNewProxy.setImportParameter("I_KUNNR", m_strKUNNR)
                myNewProxy.setImportParameter("I_VKORG", "1510")

                myNewProxy.callBapi()

                tblDezentral = myNewProxy.getExportTable("GT_WEB")

            Catch ex As Exception

            Finally
                myNewProxy = Nothing
            End Try


            If tblDezentral.Rows.Count > 0 AndAlso tblZentral.Rows.Count > 0 Then

                m_ZulArt = Zulassungsart.Beides

            Else

                If tblZentral.Rows.Count > 0 Then
                    m_ZulArt = Zulassungsart.Zentral
                ElseIf tblDezentral.Rows.Count > 0 Then
                    m_ZulArt = Zulassungsart.Dezentral
                End If


            End If

            m_intStatus = 0


        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -2501
                    m_strMessage = "Es wurden keine Daten gefunden."
                Case "NO_WEB"
                    m_intStatus = -2502
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
        End Try



    End Sub




    Public Sub getPDIs(ByRef strStatus As String)
        Dim row As DataRow
        Dim rowPDIs As DataRow()
        Dim rowNew As DataRow

        strStatus = String.Empty

        Try
            m_tblAllPDIs = New DataTable()
            With m_tblAllPDIs.Columns
                .Add("DADPDI", GetType(System.String))
                .Add("PDIAnzahl", GetType(System.String))
                .Add("PDIName", GetType(System.String))
            End With

            For Each row In m_tblFahrzeuge.Rows
                rowPDIs = m_tblAllPDIs.Select("DADPDI='" & CType(row("DADPDI"), String) & "'")
                If rowPDIs.Length = 0 Then  'PDI noch nicht gefunden
                    rowNew = m_tblAllPDIs.NewRow
                    rowNew("PDIAnzahl") = row("DADPDI")
                    rowNew("DADPDI") = row("DADPDI")
                    rowNew("PDIName") = row("DADPDI_NAME1")
                    m_tblAllPDIs.Rows.Add(rowNew)
                    m_tblAllPDIs.AcceptChanges()
                End If
            Next
        Catch ex As Exception
            strStatus = "Fehler bei der Ermittlung der PDIs."
        End Try
    End Sub

    Public Sub getMODs(ByRef strStatus As String)
        Dim row As DataRow
        Dim rowPdi As DataRow
        Dim rowPDIs As DataRow()
        Dim rowMODs As DataRow()
        Dim rowNew As DataRow

        strStatus = String.Empty

        If m_tblAllPDIs Is Nothing Then
            getPDIs(strStatus)
        End If

        Dim lngTimeStart As DateTime
        Dim lngTimeEnd As DateTime

        lngTimeStart = Now

        Try
            If strStatus = String.Empty Then

                m_tblAllMODs = New DataTable()
                With m_tblAllMODs.Columns
                    .Add("ZZHERST_TEXT", GetType(System.String))
                    .Add("ZZHANDELSNAME", GetType(System.String))
                    .Add("HandelsnameAnzahl", GetType(System.String))
                    .Add("DADPDI", GetType(System.String))
                    .Add("Handelsname", GetType(System.String))
                End With

                For Each row In m_tblAllPDIs.Rows
                    rowPDIs = m_tblFahrzeuge.Select("DADPDI='" & CType(row("DADPDI"), String) & "'")



                    For Each rowPdi In rowPDIs      'PDIs durchgehen
                        rowPdi("ZZHANDELSNAME") = Replace(rowPdi("ZZHANDELSNAME").ToString, "'", "")

                        rowMODs = m_tblAllMODs.Select("DADPDI='" & CType(rowPdi("DADPDI"), String) & "' AND ZZHANDELSNAME ='" & CType(rowPdi("ZZHANDELSNAME"), String) & "'")
                        If rowMODs.Length = 0 Then          'Modell noch nicht gefunden
                            rowNew = m_tblAllMODs.NewRow
                            'rowNew("ZZHERST_TEXT") = rowPdi("ZZHERST_TEXT")
                            rowNew("HandelsnameAnzahl") = rowPdi("ZZHERST_TEXT").ToString & "/" & rowPdi("ZZHANDELSNAME").ToString
                            rowNew("ZZHANDELSNAME") = rowPdi("ZZHANDELSNAME")
                            rowNew("DADPDI") = rowPdi("DADPDI")
                            rowNew("Handelsname") = rowPdi("ZZHANDELSNAME")
                            m_tblAllMODs.Rows.Add(rowNew)
                            m_tblAllMODs.AcceptChanges()
                        End If
                    Next
                Next
            End If
        Catch ex As Exception
            strStatus = "Fehler bei der Ermittlung der Modelle."
        End Try

        lngTimeEnd = Now

    End Sub

    Public Sub setZulassung(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        'Zulassen
        Dim rowsZulassung As DataRow()
        Dim strZuldatum As String
        Dim strKBANR As String
        Dim strKunnr As String
        Dim strHalter As String
        Dim strVersicherer As String
        Dim strPDI As String
        Dim strFahrgestell As String


        m_strClassAndMethod = "Change_01.setZulassung"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        FahrzeugeSperren(Result, m_strAppID, SessionID, page)

        Try
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Massenzulassung", m_objApp, m_objUser, page)


            Dim zulSpeicherTable As DataTable = myProxy.getImportTable("INTERNTAB")
            Dim zulSpeicher As DataRow

            Dim zulOutPut As DataRow

            Dim intCount As Integer
            Dim booKunnrZa = False


            For Each dc As DataColumn In Result.Columns

                If dc.ColumnName = "I_KUNNR_ZA" Then
                    booKunnrZa = True
                    Exit For
                End If


            Next



            'Tabellenzeilen identifizieren
            rowsZulassung = Result.Select("SelectedEinzel=True AND Art='CAR'")

            'Alle FAhrzeuge durchgehen.....
            For intCount = 0 To rowsZulassung.Length - 1
                'Inputarameter füllen ......
                strZuldatum = Format(CDate(rowsZulassung(intCount)("SelectedDate")), "dd.MM.yyyy")
                strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)
                'strHalter = "200098574"
                strHalter = rowsZulassung(intCount)("HALTER").ToString
                strVersicherer = Right(CStr(rowsZulassung(intCount)("Versicherer")), 10)

                'strKBANR = "0641400"
                strKBANR = Right(rowsZulassung(intCount)("KBANR").ToString, 7)

                strPDI = rowsZulassung(intCount)("DADPDI").ToString
                strFahrgestell = CStr(rowsZulassung(intCount)("ZZFAHRG"))
                Try
                    zulSpeicher = zulSpeicherTable.NewRow
                    With zulSpeicher
                        .Item("I_Kunnr_Ag") = strKunnr
                        .Item("I_Zzfahrg") = strFahrgestell
                        .Item("I_Edatu") = strZuldatum
                        .Item("I_Kunnr_Zv") = strVersicherer
                        .Item("I_Zzkennz") = String.Empty
                        .Item("I_Kunnr_Zh") = strHalter



                        If booKunnrZa = True Then
                            If String.IsNullOrEmpty(rowsZulassung(intCount)("I_KUNNR_ZA").ToString) = False Then
                                .Item("I_Kunnr_Za") = rowsZulassung(intCount)("I_KUNNR_ZA")
                            End If

                        Else
                            .Item("I_Kunnr_Za") = String.Empty

                        End If


                        .Item("I_Kbanr") = strKBANR
                        .Item("I_Zzcarport") = strPDI
                    End With
                    zulSpeicherTable.Rows.Add(zulSpeicher)
                    rowsZulassung(intCount)("Status") = "Vorgang OK: Daten wurden gesendet."
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
                    rowsZulassung(intCount)("Status") = m_strMessage
                    WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
                End Try

            Next

            If zulSpeicherTable.Rows.Count > 0 Then
                Try
                    Dim anzahl As String = ""
                    Dim sReturn As String = ""

                    myProxy.callBapi()

                    sReturn = myProxy.getExportParameter("RETURN")
                    anzahl = myProxy.getExportParameter("ANZAHL")
                    m_zulOutPutTable = myProxy.getExportTable("OUTPUT")


                    If sReturn = "OK" Then
                        m_intStatus = 0
                        Result.AcceptChanges()
                        For Each zulOutPut In m_zulOutPutTable.Rows
                            Result.Select("Fahrgestellnummer='" & zulOutPut.Item("Id").ToString & "'")(0)("Belegnummer") = zulOutPut.Item("Message")
                            If zulOutPut.Item("Message").ToString.Length > 0 Then
                                m_intErrorCount += 1
                            End If
                        Next
                        Result.AcceptChanges()
                        m_strMessage = String.Format("Es wurden {0} Fahrzeuge zugelassen.", anzahl.Trim)
                    Else
                        m_intStatus = -9999

                        For intCount = 0 To rowsZulassung.Length - 1
                            m_intErrorCount += 1
                            rowsZulassung(intCount)("Status") = "Fehler beim Speichern."
                        Next
                        m_strMessage = "Es ist ein Fehler aufgetreten.<br>Daten wurden nicht übermittelt."
                    End If

                Catch ex As Exception
                    m_intStatus = -9999

                    m_strMessage = String.Concat("Es ist ein Fehler aufgetreten.<br>", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                End Try
            End If

        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = "Unbekannter Fehler."
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub


    Public Sub setZulassungDezentral(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        'Zulassen
        Dim rowsZulassung As DataRow()
        Dim strZuldatum As String
        Dim strKBANR As String
        Dim strKunnr As String
        Dim strHalter As String
        Dim strVersicherer As String
        Dim strPDI As String
        Dim strFahrgestell As String
        Dim strEquiNr As String
        Dim strZulassungsort As String



        m_strClassAndMethod = "CarRent01.setZulassungDezentral"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        FahrzeugeSperren(Result, m_strAppID, SessionID, page)

        Try
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_IMP_DEZ_ZUL_001", m_objApp, m_objUser, page)


            myProxy.setImportParameter("I_KUNNR_AG", m_strKUNNR)

            Dim zulSpeicherTable As DataTable = myProxy.getImportTable("GT_WEB")
            Dim zulSpeicher As DataRow


            Dim intCount As Integer

            'Tabellenzeilen identifizieren
            rowsZulassung = Result.Select("SelectedEinzel=True AND Art='CAR'")

            'Alle FAhrzeuge durchgehen.....
            For intCount = 0 To rowsZulassung.Length - 1
                'Inputarameter füllen ......
                strZuldatum = Format(CDate(rowsZulassung(intCount)("SelectedDate")), "dd.MM.yyyy")
                strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

                strHalter = rowsZulassung(intCount)("HALTER").ToString
                strVersicherer = Right(CStr(rowsZulassung(intCount)("Versicherer")), 10)


                strKBANR = rowsZulassung(intCount)("KBANR").ToString

                strPDI = rowsZulassung(intCount)("DADPDI").ToString
                strFahrgestell = CStr(rowsZulassung(intCount)("ZZFAHRG"))

                strEquiNr = CStr(rowsZulassung(intCount)("EQUNR"))
                strZulassungsort = CStr(rowsZulassung(intCount)("ZKFZKZ"))


                'Try
                zulSpeicher = zulSpeicherTable.NewRow
                With zulSpeicher
                    '.Item("I_Kunnr_Ag") = strKunnr
                    .Item("ZZFAHRG") = strFahrgestell
                    .Item("EDATU") = strZuldatum
                    .Item("KUNNR_ZV") = strVersicherer
                    '.Item("I_Zzkennz") = String.Empty
                    .Item("KUNNR_ZH") = strHalter
                    .Item("I_Kunnr_Za") = rowsZulassung(intCount)("DADPDI").ToString
                    .Item("KBANR") = strKBANR
                    .Item("ZZCARPORT") = strPDI
                    .Item("EQUNR") = strEquiNr
                    .Item("ZULASSUNGSORT") = strZulassungsort
                    .Item("ZULDAT") = strZuldatum

                End With
                zulSpeicherTable.Rows.Add(zulSpeicher)
                rowsZulassung(intCount)("Status") = "Vorgang OK: Daten wurden gesendet."
                '        Catch ex As Exception
                '    'Fehler...
                '    Select Case ex.Message
                '        Case "SCHON_ZUGELASSEN"
                '            m_intStatus = -1234
                '            m_strMessage = "Fehler! Fahrzeug bereits zugelassen."
                '        Case "GESPERRT"
                '            m_intStatus = -1235
                '            m_strMessage = "Fehler! Fahrzeug gesperrt"
                '        Case "VERSCHOBEN"
                '            m_intStatus = -1236
                '            m_strMessage = "Fehler! Fahrzeug verschoben."
                '        Case Else
                '            m_intStatus = -9999
                '            m_strMessage = "Fehler! Unbekannter Fehler."
                '    End Select
                '    rowsZulassung(intCount)("Status") = m_strMessage
                '    WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
                'End Try

            Next

            If zulSpeicherTable.Rows.Count > 0 Then
                Try
                    Dim anzahl As String = zulSpeicherTable.Rows.Count.ToString
                    Dim sReturn As String = ""

                    myProxy.callBapi()

                    'sReturn = myProxy.getExportParameter("RETURN")
                    'anzahl = myProxy.getExportParameter("ANZAHL")
                    'm_zulOutPutTable = myProxy.getExportTable("OUTPUT")


                    'If sReturn = "OK" Then
                    '    m_intStatus = 0
                    '    Result.AcceptChanges()
                    '    For Each zulOutPut In m_zulOutPutTable.Rows
                    '        Result.Select("Fahrgestellnummer='" & zulOutPut.Item("Id").ToString & "'")(0)("Belegnummer") = zulOutPut.Item("Message")
                    '        If zulOutPut.Item("Message").ToString.Length > 0 Then
                    '            m_intErrorCount += 1
                    '        End If
                    '    Next
                    '    Result.AcceptChanges()
                    '    m_strMessage = String.Format("Es wurden {0} Fahrzeuge zugelassen.", anzahl.Trim)
                    'Else
                    '    m_intStatus = -9999

                    '    For intCount = 0 To rowsZulassung.Length - 1
                    '        m_intErrorCount += 1
                    '        rowsZulassung(intCount)("Status") = "Fehler beim Speichern."
                    '    Next
                    '    m_strMessage = "Es ist ein Fehler aufgetreten.<br>Daten wurden nicht übermittelt."
                    'End If

                Catch ex As Exception
                    m_intStatus = -9999

                    m_strMessage = String.Concat("Es ist ein Fehler aufgetreten.<br>", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                End Try
            End If

        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = "Unbekannter Fehler."
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub






    Public Sub FahrzeugeSperren(ByVal tblFahrzeuge As DataTable, ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        Dim rows As DataRow()
        Dim i As Integer

        Try
            m_intStatus = 0
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Warenkorb_Sperre_001", m_objApp, m_objUser, page)

            Dim tblFahrzeugeSAP As DataTable = myProxy.getImportTable("GT_IN")
            Dim tblFahrzeugeSAPRow As DataRow


            'Fahrzeuge übernehmen
            rows = tblFahrzeuge.Select("SelectedEinzel=True AND Art='CAR'")

            For i = 0 To rows.Length - 1
                tblFahrzeugeSAPRow = tblFahrzeugeSAP.NewRow
                tblFahrzeugeSAPRow("Equnr") = CType(rows(i)("EQUNR"), String)
                tblFahrzeugeSAP.Rows.Add(tblFahrzeugeSAPRow)
            Next

            'SAP-Aufruf
            myProxy.callBapi()


            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, tblFahrzeuge)
        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & m_strMessage, tblFahrzeuge)
        End Try
    End Sub
    Public Sub setResultRowClear()
        Dim row As DataRow
        Dim intIndex As Integer
        Dim blnSelectedEinzel As Boolean
        m_tblResultExcel = New DataTable()

        'Nicht ausgewählte Zeilen löschen
        Try
            For intIndex = m_tblResult.Rows.Count - 1 To 0 Step -1
                row = m_tblResult.Rows(intIndex)

                blnSelectedEinzel = CBool(row("SelectedEinzel"))
                If blnSelectedEinzel = False Then
                    m_tblResult.Rows.Remove(row)
                End If
            Next
            m_tblResult.AcceptChanges()

            'Tabelle für Excel-Export neu aufbauen...
            With m_tblResultExcel.Columns
                .Add("Unitnr.", GetType(System.String))
                .Add("Eingangsdatum", GetType(System.String))
                .Add("Fahrgestellnummer", GetType(System.String))
                .Add("Zulassungsdatum", GetType(System.String))
                .Add("Status", GetType(System.String))
            End With
            'Werte füllen
            Dim rowNew As DataRow
            For Each row In m_tblResult.Rows
                rowNew = m_tblResultExcel.NewRow
                rowNew("Unitnr.") = CStr(row("ZZREFERENZ1"))
                rowNew("Eingangsdatum") = CStr(row("ZZDAT_EIN"))
                rowNew("Fahrgestellnummer") = CStr(row("ZZFAHRG"))
                rowNew("Zulassungsdatum") = CStr(row("SelectedDate"))
                rowNew("Status") = CStr(row("Status"))
                m_tblResultExcel.Rows.Add(rowNew)
            Next
            m_tblResultExcel.AcceptChanges()

        Catch ex As Exception
            m_tblResult = Nothing
            m_tblResultExcel = Nothing
        End Try
    End Sub
End Class
