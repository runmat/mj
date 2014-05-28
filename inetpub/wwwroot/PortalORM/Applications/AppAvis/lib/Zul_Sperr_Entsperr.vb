Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Imports SapORM.Contracts

'#################################################################
' Klasse für das Zulassen, Sperren und Entsperren von Fahrzeugen
' Change : Carport Bestände (Change02)
'#################################################################

Public Class Zul_Sperr_Entsperr
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_strPDI As String
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_datMeldungsdatumVon As DateTime
    Private m_datMeldungsdatumBis As DateTime
    Private m_strModell As String
    Private m_strFlag_Vorh As String
    Private m_tblResultPDIs As DataTable
    Private m_tblHersteller As DataTable
    Private m_tblLiefermonat As DataTable
    Private m_tblBereifung As DataTable
    Private m_tblGetriebe As DataTable
    Private m_tblKraftstoff As DataTable
    Private m_tblNavi As DataTable
    Private m_tblFarbe As DataTable
    Private m_tblVermiet As DataTable
    Private m_tblFzgArt As DataTable
    Private m_tblAufbauArt As DataTable
    Private m_tblHaendlernr As DataTable
    Private m_tblHandlername As DataTable
    Private m_tblEKIndikator As DataTable
    Private m_tblVerwZweck As DataTable
    Private m_tblHOwnerCode As DataTable
    Private m_tblSperrdat As DataTable
    Private m_tblZulKreis As DataTable
    Private m_dsCarPort_Data As DataSet
    Private m_dsCarPort_Data_Selected As DataSet
    Private m_tblCarports As DataTable
    Private m_tblAllCarports As DataTable
    Private m_tblModelle As DataTable
    Private m_tblFahrzeuge As DataTable
    Private m_intErrorCount As Int32
    Private m_tblSaveCars As DataTable
    Private m_strCarportNr As String
    Private m_strCarportNrSuche As String
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
    Private m_strFilter As String
    Private m_intFehlerCount As Integer
    Private m_tblZulCount As DataTable
    Private m_DezZul As Boolean
#End Region

#Region " Properties"
    Public ReadOnly Property CarPort_Data_Selected() As DataSet
        Get
            Return m_dsCarPort_Data_Selected
        End Get
    End Property

    Public ReadOnly Property Task() As String
        Get
            Return m_strTask
        End Get
    End Property

    Public ReadOnly Property CarPort_Data() As DataSet
        Get
            Return m_dsCarPort_Data
        End Get
    End Property
    Public ReadOnly Property ResultPDIs() As DataTable
        Get
            Return m_tblResultPDIs
        End Get
    End Property
    Public ReadOnly Property Hersteller() As DataTable
        Get
            Return m_tblHersteller
        End Get
    End Property
    Public ReadOnly Property Bereifung() As DataTable
        Get
            Return m_tblBereifung
        End Get
    End Property
    Public ReadOnly Property Liefermonat() As DataTable
        Get
            Return m_tblLiefermonat
        End Get
    End Property
    Public ReadOnly Property Getriebe() As DataTable
        Get
            Return m_tblGetriebe
        End Get
    End Property

    Public ReadOnly Property Kraftstoff() As DataTable
        Get
            Return m_tblKraftstoff
        End Get
    End Property

    Public ReadOnly Property Navi() As DataTable
        Get
            Return m_tblNavi
        End Get
    End Property
    Public ReadOnly Property Farbe() As DataTable
        Get
            Return m_tblFarbe
        End Get
    End Property
    Public ReadOnly Property Vermiet() As DataTable
        Get
            Return m_tblVermiet
        End Get
    End Property

    Public ReadOnly Property FzgArt() As DataTable
        Get
            Return m_tblFzgArt
        End Get
    End Property

    Public ReadOnly Property AufbauArt() As DataTable
        Get
            Return m_tblAufbauArt
        End Get
    End Property
    Public ReadOnly Property Haendlernr() As DataTable
        Get
            Return m_tblHaendlernr
        End Get
    End Property

    Public ReadOnly Property Handlername() As DataTable
        Get
            Return m_tblHandlername
        End Get
    End Property

    Public ReadOnly Property EKIndikator() As DataTable
        Get
            Return m_tblEKIndikator
        End Get
    End Property
    Public ReadOnly Property VerwZweck() As DataTable
        Get
            Return m_tblVerwZweck
        End Get
    End Property
    Public ReadOnly Property HOwnerCode() As DataTable
        Get
            Return m_tblHOwnerCode
        End Get
    End Property
    Public ReadOnly Property Sperrdat() As DataTable
        Get
            Return m_tblSperrdat
        End Get
    End Property
    Public ReadOnly Property ZulKreis() As DataTable
        Get
            Return m_tblZulKreis
        End Get
    End Property

    Public Property EingangsdatumVon() As DateTime
        Get
            Return m_datEingangsdatumVon
        End Get
        Set(ByVal value As DateTime)
            m_datEingangsdatumVon = value
        End Set
    End Property
    Public Property EingangsdatumBis() As DateTime
        Get
            Return m_datEingangsdatumBis
        End Get
        Set(ByVal value As DateTime)
            m_datEingangsdatumBis = value
        End Set
    End Property
    Public Property MeldungsdatumVon() As DateTime
        Get
            Return m_datMeldungsdatumVon
        End Get
        Set(ByVal value As DateTime)
            m_datMeldungsdatumVon = value
        End Set
    End Property
    Public Property MeldungsdatumBis() As DateTime
        Get
            Return m_datMeldungsdatumBis
        End Get
        Set(ByVal value As DateTime)
            m_datMeldungsdatumBis = value
        End Set
    End Property
    Public Property PDI() As String
        Get
            Return m_strPDI
        End Get
        Set(ByVal value As String)
            m_strPDI = value
        End Set
    End Property

    Public Property Modell() As String
        Get
            Return m_strModell
        End Get
        Set(ByVal value As String)
            m_strModell = value
        End Set
    End Property
    Public Property Flag_Vorh() As String
        Get
            Return m_strFlag_Vorh
        End Get
        Set(ByVal value As String)
            m_strFlag_Vorh = value
        End Set
    End Property
    Public Property Carportnummer() As String
        Get
            Return m_strCarportNr
        End Get
        Set(ByVal value As String)
            m_strCarportNr = value
        End Set
    End Property
    Public Property FilterString() As String
        Get
            Return m_strFilter
        End Get
        Set(ByVal value As String)
            m_strFilter = value
        End Set
    End Property
    Public ReadOnly Property SelectedCars() As Int32
        Get
            Return m_intSelectedCars
        End Get
    End Property
    Public ReadOnly Property FehlerCount() As Int32
        Get
            Return m_intFehlerCount
        End Get
    End Property
    Public ReadOnly Property Erledigt() As DataTable
        Get
            Return m_tblErledigt
        End Get
    End Property
    Public ReadOnly Property ZulCount() As DataTable
        Get
            Return m_tblZulCount
        End Get
    End Property

    Public Property DezZul() As Boolean
        Get
            Return m_DezZul
        End Get
        Set(ByVal value As Boolean)
            m_DezZul = value
        End Set
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        If Not m_blnGestartet Then
            m_blnGestartet = True
            m_intErrorCount = 0
            m_intLastID = -1
            m_strModellCode = ""
            m_strFahrgestellnummer = ""
            m_strCarportNr = ""
            m_intSelectedCars = 0
            m_blnShowBelegnummer = False
            'm_strCustomer = Right("0000000000" & strCustomer, 10)
            'm_strCreditControlArea = strCreditControlArea
            'm_strPDINummerSuche = strPDINummerSuche
            'm_strTask = strTask

            '--- Alle PDIs ------------------------------------------------------
            m_tblAllCarports = New DataTable("AllCarports")
            With m_tblAllCarports.Columns
                .Add("Carportnr", System.Type.GetType("System.String"))
                .Add("Carport", System.Type.GetType("System.String"))
                .Add("Zulassungsf_Fahrzeuge", System.Type.GetType("System.Int32"))
                .Add("Gesperrte_Fahrzeuge", System.Type.GetType("System.Int32"))
                .Add("Details", System.Type.GetType("System.Boolean"))
                .Add("Loaded", System.Type.GetType("System.Boolean"))
            End With

            '--- PDIs -----------------------------------------------------------
            m_dsCarPort_Data = New DataSet()
            m_tblCarports = New DataTable("Carports")

            With m_tblCarports.Columns
                .Add("Carportnr", System.Type.GetType("System.String"))
                .Add("Carport", System.Type.GetType("System.String"))
                .Add("Zulassungsf_Fahrzeuge", System.Type.GetType("System.Int32"))
                .Add("Gesperrte_Fahrzeuge", System.Type.GetType("System.Int32"))
                .Add("Details", System.Type.GetType("System.Boolean"))
                .Add("Loaded", System.Type.GetType("System.Boolean"))
            End With

            '--- Modelle ---------------------------------------------------------
            m_tblModelle = New DataTable("Modelle")

            With m_tblModelle.Columns
                .Add("ID", System.Type.GetType("System.Int32"))
                .Add("Carportnr", System.Type.GetType("System.String"))
                .Add("Carport", System.Type.GetType("System.String"))
                .Add("Hersteller_ID_Avis", System.Type.GetType("System.String"))
                .Add("Herstellername", System.Type.GetType("System.String"))
                .Add("Typ_ID_Avis", System.Type.GetType("System.String"))
                .Add("Modellbezeichnung", System.Type.GetType("System.String"))
                .Add("Reifenart", System.Type.GetType("System.String"))
                .Add("Liefermonat", System.Type.GetType("System.String"))
                .Add("Kraftstoffart", System.Type.GetType("System.String"))
                .Add("Navigation", System.Type.GetType("System.String"))
                .Add("Farbe", System.Type.GetType("System.String"))
                .Add("Vermietgruppe", System.Type.GetType("System.String"))
                .Add("Fahrzeugart", System.Type.GetType("System.String"))
                .Add("Bezahltkennzeichen", System.Type.GetType("System.Boolean"))
                .Add("HaendlerId", System.Type.GetType("System.String"))
                .Add("Haendler_Kurzname", System.Type.GetType("System.String"))
                .Add("Zulassungsort", System.Type.GetType("System.String"))
                .Add("Einkaufsindikator", System.Type.GetType("System.String"))
                .Add("DatumErstzulassung", System.Type.GetType("System.DateTime"))
                .Add("Anzahl_alt", System.Type.GetType("System.Int32"))
                .Add("Anzahl_neu", System.Type.GetType("System.Int32"))
                .Add("ZZCARPORT", System.Type.GetType("System.String"))
                .Add("ZielCarport", System.Type.GetType("System.String"))
                .Add("Bemerkung", System.Type.GetType("System.String"))
                .Add("BemerkungDatum", System.Type.GetType("System.DateTime"))
                .Add("EQUNR", System.Type.GetType("System.String"))
                .Add("QMNUM", System.Type.GetType("System.String"))
                .Add("Task", System.Type.GetType("System.String"))
            End With

            '--- FAHRZEUGE ---------------------------------------------------------
            m_tblFahrzeuge = New DataTable("Fahrzeuge")

            With m_tblFahrzeuge.Columns
                .Add("Modell_ID", System.Type.GetType("System.Int32"))
                .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                .Add("Eingangsdatum", System.Type.GetType("System.DateTime"))
                .Add("Ausgewaehlt", System.Type.GetType("System.Boolean"))
                .Add("DatumErstzulassung", System.Type.GetType("System.DateTime"))
                .Add("Datum_zur_Sperre", System.Type.GetType("System.String"))
                .Add("Sperrvermerk", System.Type.GetType("System.String"))
                .Add("Sperren", System.Type.GetType("System.String"))
                .Add("ZielCarport", System.Type.GetType("System.String"))
                .Add("ZZCARPORT", System.Type.GetType("System.String"))
                .Add("EQUNR", System.Type.GetType("System.String"))
                .Add("QMNUM", System.Type.GetType("System.String"))
                .Add("Zulassungsort", System.Type.GetType("System.String"))
                .Add("Reifenart", System.Type.GetType("System.String"))
                .Add("Verwendungszweck", System.Type.GetType("System.String"))
                .Add("neuGesperrt", System.Type.GetType("System.String"))
                .Add("neuEntsperrt", System.Type.GetType("System.String"))
                .Add("Ergebnis", System.Type.GetType("System.String"))
                .Add("Owner_Code", System.Type.GetType("System.String"))
            End With

            m_dsCarPort_Data.Tables.Add(m_tblCarports)
            m_dsCarPort_Data.Tables.Add(m_tblModelle)
            m_dsCarPort_Data.Tables.Add(m_tblFahrzeuge)

            Dim dc1 As DataColumn
            Dim dc2 As DataColumn
            'Relation Author => Title
            dc1 = m_dsCarPort_Data.Tables("Carports").Columns("Carportnr")
            dc2 = m_dsCarPort_Data.Tables("Modelle").Columns("Carportnr")
            Dim dr As DataRelation = New DataRelation("Carport_Modell", dc1, dc2, False)
            m_dsCarPort_Data.Relations.Add(dr)

            'Relation Title => Sales
            dc1 = m_dsCarPort_Data.Tables("Modelle").Columns("ID")
            dc2 = m_dsCarPort_Data.Tables("Fahrzeuge").Columns("Modell_ID")
            dr = New DataRelation("Modell_Fahrzeug", dc1, dc2, False)
            m_dsCarPort_Data.Relations.Add(dr)

            m_blnGestartet = False
        End If
    End Sub

    '----------------------------------------------------------------------
    ' Methode: Fill
    ' Autor: O.Rudolph
    ' Beschreibung: füllen der Selektiondaten(Change02)
    ' Erstellt am: 18.11.2008
    ' ITA: 2359
    '----------------------------------------------------------------------

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        'Dim strKunnr As String = ""
        m_strClassAndMethod = "Zul_Sperr_Entsperr.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        'strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim tblTemp2 As DataTable
        Dim intID As Int32 = -1

        Try
            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_READ_FZGPOOL_ZUL_FZG_006", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

            tblTemp2 = S.AP.GetExportTableWithInitExecute("Z_M_READ_FZGPOOL_ZUL_FZG_006.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

            'strCom = "EXEC Z_M_READ_FZGPOOL_ZUL_FZG_006 @I_KUNNR_AG='" & strKunnr & "', " _
            '                                          & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom


            'Dim pSAPTableAUFTRAG As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTableAUFTRAG)

            'cmd.ExecuteNonQuery()

            'tblTemp2 = DirectCast(pSAPTableAUFTRAG.Value, DataTable)

            m_tableResult = tblTemp2
            CreateOutPut(tblTemp2, strAppID)


            Dim dv As DataView

            dv = m_tblResult.DefaultView
            dv.Sort = "Carportnr"

            Dim row As DataRow
            Dim e As Int32


            Dim sCarport As String = ""
            dv.Sort = "Carportnr"
            e = 0
            m_tblResultPDIs = New DataTable
            m_tblResultPDIs.Columns.Add("Carportnr", System.Type.GetType("System.String"))
            m_tblResultPDIs.Columns.Add("Carport", System.Type.GetType("System.String"))
            m_tblResultPDIs = FillFirstRow(m_tblResultPDIs, "Carportnr", "Carport")

            Do While e < dv.Count
                row = m_tblResultPDIs.NewRow
                If sCarport <> dv.Item(e)("Carportnr").ToString Then
                    row(0) = dv.Item(e)("Carportnr")
                    row(1) = dv.Item(e)("Carportnr").ToString & " - " & dv.Item(e)("Carport").ToString
                    sCarport = dv.Item(e)("Carportnr").ToString
                    m_tblResultPDIs.Rows.Add(row)
                End If
                e = e + 1
            Loop


            dv.Sort = "Hersteller_ID_Avis"
            e = 0
            Dim HerrCode As String = ""
            m_tblHersteller = FillFirstRow(m_tblHersteller, "HerstellerID", "Hersteller_ID_Avis")
            row = m_tblHersteller.NewRow
            Do While e < dv.Count
                row = m_tblHersteller.NewRow
                If HerrCode <> dv.Item(e)("Hersteller_ID_Avis").ToString Then
                    row(0) = dv.Item(e)("Hersteller_ID_Avis")
                    row(1) = dv.Item(e)("Hersteller_ID_Avis")
                    m_tblHersteller.Rows.Add(row)
                    HerrCode = dv.Item(e)("Hersteller_ID_Avis").ToString
                End If

                e = e + 1
            Loop


            m_tblLiefermonat = FillFirstRow(m_tblLiefermonat, "ID", "Liefermonat")
            BoundViews(dv, m_tblLiefermonat, "Liefermonat")

            m_tblBereifung = FillFirstRow(m_tblBereifung, "ID", "REIFENART")
            BoundViews(dv, m_tblBereifung, "REIFENART")

            m_tblGetriebe = FillFirstRow(m_tblGetriebe, "ID", "ANTRIEBSART")
            BoundViews(dv, m_tblGetriebe, "ANTRIEBSART")


            m_tblKraftstoff = FillFirstRow(m_tblKraftstoff, "ID", "Kraftstoffart")
            BoundViews(dv, m_tblKraftstoff, "Kraftstoffart")


            m_tblNavi = FillFirstRow(m_tblNavi, "ID", "Navigation")
            dv.Sort = "Navigation"
            e = 0
            Dim sNavi As String = ""
            row = m_tblNavi.NewRow
            Do While e < dv.Count
                row = m_tblNavi.NewRow
                If sNavi <> dv.Item(e)("Navigation").ToString Then
                    row(0) = e
                    row(1) = dv.Item(e)("Navigation")
                    If dv.Item(e)("Navigation").ToString = "J" Then
                        row(1) = "Ja"
                    Else
                        row(1) = "Nein"
                    End If

                    m_tblNavi.Rows.Add(row)
                    sNavi = dv.Item(e)("Navigation").ToString
                End If
                e = e + 1
            Loop


            m_tblFarbe = FillFirstRow(m_tblFarbe, "ID", "FARBE")
            BoundViews(dv, m_tblFarbe, "FARBE")


            m_tblVermiet = FillFirstRow(m_tblVermiet, "ID", "Vermietgruppe")
            BoundViews(dv, m_tblVermiet, "Vermietgruppe")


            m_tblFzgArt = FillFirstRow(m_tblFzgArt, "ID", "Fahrzeugart")
            BoundViews(dv, m_tblFzgArt, "Fahrzeugart")


            m_tblAufbauArt = FillFirstRow(m_tblAufbauArt, "ID", "AUFBAUART")
            BoundViews(dv, m_tblAufbauArt, "AUFBAUART")


            m_tblHaendlernr = FillFirstRow(m_tblHaendlernr, "ID", "HaendlerId")
            BoundViews(dv, m_tblHaendlernr, "HaendlerId")


            m_tblHandlername = FillFirstRow(m_tblHandlername, "ID", "Haendler_Kurzname")
            BoundViews(dv, m_tblHandlername, "Haendler_Kurzname")


            m_tblEKIndikator = FillFirstRow(m_tblEKIndikator, "ID", "Einkaufsindikator")
            BoundViews(dv, m_tblEKIndikator, "Einkaufsindikator")

            m_tblVerwZweck = FillFirstRow(m_tblVerwZweck, "ID", "VERWENDUNGSZWECK")
            BoundViews(dv, m_tblVerwZweck, "VERWENDUNGSZWECK")

            m_tblHOwnerCode = FillFirstRow(m_tblHOwnerCode, "ID", "Owner_Code")
            BoundViews(dv, m_tblHOwnerCode, "Owner_Code")

            m_tblZulKreis = FillFirstRow(m_tblZulKreis, "ID", "ZULASSUNGSORT")
            BoundViews(dv, m_tblZulKreis, "ZULASSUNGSORT")

            dv.Sort = "Datum_zur_Sperre"
            e = 0
            Dim Sperrdate As String = ""
            m_tblSperrdat = FillFirstRow(m_tblSperrdat, "ID", "Datum_zur_Sperre")
            row = m_tblSperrdat.NewRow
            Do While e < dv.Count
                row = m_tblSperrdat.NewRow
                If Sperrdate <> dv.Item(e)("Datum_zur_Sperre").ToString AndAlso dv.Item(e)("Datum_zur_Sperre").ToString <> "00000000" Then
                    row(0) = e
                    row(1) = FormatDateTime(CDate(dv.Item(e)("Datum_zur_Sperre")), DateFormat.ShortDate)
                    m_tblSperrdat.Rows.Add(row)
                    Sperrdate = dv.Item(e)("Datum_zur_Sperre").ToString
                End If

                e = e + 1
            Loop



            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", I_EING_DAT_BIS=" & m_datEingangsdatumBis.ToShortDateString, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'cmd.Dispose()
            'con.Close()
            'con.Dispose()
            m_blnGestartet = False
        End Try
    End Sub

    Private Sub BoundViews(ByVal View As DataView, ByVal Table As DataTable, ByVal Text As String)

        Dim sTemp As String = ""
        Dim row As DataRow
        Dim e As Int32

        View.Sort = Text
        e = 0
        Do While e < View.Count
            row = Table.NewRow
            If sTemp <> View.Item(e)(Text).ToString Then
                row(0) = e
                row(1) = View.Item(e)(Text)
                Table.Rows.Add(row)
                sTemp = View.Item(e)(Text).ToString
            End If
            e = e + 1
        Loop
    End Sub

    Private Function FillFirstRow(ByVal Table As DataTable, ByVal ID As String, ByVal ColumnName As String) As DataTable
        Dim row As DataRow

        Table = New DataTable
        Table.Columns.Add(ID, System.Type.GetType("System.String"))
        Table.Columns.Add(ColumnName, System.Type.GetType("System.String"))

        row = Table.NewRow
        row(0) = "-1"
        row(1) = "- keine Auswahl -"
        Table.Rows.Add(row)

        Return Table
    End Function

    Public Function Filter(ByVal FilterString As String) As Integer

        Dim dv As DataView

        dv = Result.DefaultView
        dv.RowFilter = FilterString
        If dv.Count > 0 Then

            dv.Sort = "Carportnr"

            Dim row As DataRow
            Dim e As Int32

            e = 0
            m_tblResultPDIs = New DataTable
            m_tblResultPDIs.Columns.Add("Carportnr", System.Type.GetType("System.String"))
            m_tblResultPDIs.Columns.Add("Carport", System.Type.GetType("System.String"))
            m_tblResultPDIs = FillFirstRow(m_tblResultPDIs, "Carportnr", "Carport")
            Dim sCarport As String = ""
            Do While e < dv.Count
                row = m_tblResultPDIs.NewRow
                If sCarport <> dv.Item(e)("Carportnr").ToString Then
                    row(0) = dv.Item(e)("Carportnr")
                    row(1) = dv.Item(e)("Carportnr").ToString & " - " & dv.Item(e)("Carport").ToString
                    sCarport = dv.Item(e)("Carportnr").ToString
                    m_tblResultPDIs.Rows.Add(row)
                End If
                e = e + 1
            Loop


            dv.Sort = "Hersteller_ID_Avis"
            e = 0
            Dim HerrCode As String = ""
            m_tblHersteller = FillFirstRow(m_tblHersteller, "HerstellerID", "Hersteller_ID_Avis")
            row = m_tblHersteller.NewRow
            Do While e < dv.Count
                row = m_tblHersteller.NewRow
                If HerrCode <> dv.Item(e)("Hersteller_ID_Avis").ToString Then
                    row(0) = dv.Item(e)("Hersteller_ID_Avis")
                    row(1) = dv.Item(e)("Hersteller_ID_Avis")
                    m_tblHersteller.Rows.Add(row)
                    HerrCode = dv.Item(e)("Hersteller_ID_Avis").ToString
                End If

                e = e + 1
            Loop


            m_tblLiefermonat = FillFirstRow(m_tblLiefermonat, "ID", "Liefermonat")
            BoundViews(dv, m_tblLiefermonat, "Liefermonat")

            m_tblBereifung = FillFirstRow(m_tblBereifung, "ID", "REIFENART")
            BoundViews(dv, m_tblBereifung, "REIFENART")

            m_tblGetriebe = FillFirstRow(m_tblGetriebe, "ID", "ANTRIEBSART")
            BoundViews(dv, m_tblGetriebe, "ANTRIEBSART")


            m_tblKraftstoff = FillFirstRow(m_tblKraftstoff, "ID", "Kraftstoffart")
            BoundViews(dv, m_tblKraftstoff, "Kraftstoffart")


            m_tblNavi = FillFirstRow(m_tblNavi, "ID", "Navigation")
            dv.Sort = "Navigation"
            e = 0
            Dim sNavi As String = ""
            row = m_tblNavi.NewRow
            Do While e < dv.Count
                row = m_tblNavi.NewRow
                If sNavi <> dv.Item(e)("Navigation").ToString Then
                    row(0) = e
                    row(1) = dv.Item(e)("Navigation")
                    If dv.Item(e)("Navigation").ToString = "J" Then
                        row(1) = "Ja"
                    Else
                        row(1) = "Nein"
                    End If

                    m_tblNavi.Rows.Add(row)
                    sNavi = dv.Item(e)("Navigation").ToString
                End If
                e = e + 1
            Loop


            m_tblFarbe = FillFirstRow(m_tblFarbe, "ID", "FARBE")
            BoundViews(dv, m_tblFarbe, "FARBE")


            m_tblVermiet = FillFirstRow(m_tblVermiet, "ID", "Vermietgruppe")
            BoundViews(dv, m_tblVermiet, "Vermietgruppe")


            m_tblFzgArt = FillFirstRow(m_tblFzgArt, "ID", "Fahrzeugart")
            BoundViews(dv, m_tblFzgArt, "Fahrzeugart")


            m_tblAufbauArt = FillFirstRow(m_tblAufbauArt, "ID", "AUFBAUART")
            BoundViews(dv, m_tblAufbauArt, "AUFBAUART")


            m_tblHaendlernr = FillFirstRow(m_tblHaendlernr, "ID", "HaendlerId")
            BoundViews(dv, m_tblHaendlernr, "HaendlerId")


            m_tblHandlername = FillFirstRow(m_tblHandlername, "ID", "Haendler_Kurzname")
            BoundViews(dv, m_tblHandlername, "Haendler_Kurzname")


            m_tblEKIndikator = FillFirstRow(m_tblEKIndikator, "ID", "Einkaufsindikator")
            BoundViews(dv, m_tblEKIndikator, "Einkaufsindikator")

            m_tblVerwZweck = FillFirstRow(m_tblVerwZweck, "ID", "VERWENDUNGSZWECK")
            BoundViews(dv, m_tblVerwZweck, "VERWENDUNGSZWECK")

            m_tblHOwnerCode = FillFirstRow(m_tblHOwnerCode, "ID", "Owner_Code")
            BoundViews(dv, m_tblHOwnerCode, "Owner_Code")

            m_tblZulKreis = FillFirstRow(m_tblZulKreis, "ID", "ZULASSUNGSORT")
            BoundViews(dv, m_tblZulKreis, "ZULASSUNGSORT")

            dv.Sort = "Datum_zur_Sperre"
            e = 0
            Dim Sperrdate As String = ""
            m_tblSperrdat = FillFirstRow(m_tblSperrdat, "ID", "Datum_zur_Sperre")
            row = m_tblSperrdat.NewRow
            Do While e < dv.Count
                row = m_tblSperrdat.NewRow
                If Sperrdate <> dv.Item(e)("Datum_zur_Sperre").ToString AndAlso dv.Item(e)("Datum_zur_Sperre").ToString <> "00000000" Then
                    row(0) = e
                    row(1) = FormatDateTime(CDate(dv.Item(e)("Datum_zur_Sperre")), DateFormat.ShortDate)
                    m_tblSperrdat.Rows.Add(row)
                    Sperrdate = dv.Item(e)("Datum_zur_Sperre").ToString
                End If

                e = e + 1
            Loop
            Return dv.Count
        Else
            Return 0
        End If
    End Function

    Public Function FilterCarports() As String

        If m_strFilter <> "" Then
            Dim carportrow As DataRow
            Dim intZul As Int32 = 0
            Dim intGsp As Int32 = 0
            Dim dv As DataView
            Dim dv2 As DataView
            dv = m_tblResult.DefaultView
            dv2 = m_tblResult.DefaultView
            dv2.Sort = "Carportnr"
            dv.RowFilter = m_strFilter
            dv.Sort = "Carportnr"

            Dim e As Int32
            Dim iCount As Int32

            m_strTask = "Zulassen"
            Dim sCarport As String = ""
            e = 0
            Do While e < dv.Count
                intGsp = 0
                intZul = 0
                With m_tblCarports
                    If sCarport <> dv.Item(e)("Carportnr").ToString Then
                        carportrow = .NewRow
                        carportrow("Carportnr") = dv.Item(e)("Carportnr")
                        carportrow("Carport") = dv.Item(e)("Carport")
                        carportrow("Details") = True
                        carportrow("Loaded") = False
                        sCarport = dv.Item(e)("Carportnr").ToString
                        dv2.RowFilter = "Carportnr= '" & sCarport & "' and " & m_strFilter
                        intGsp = 0
                        intZul = 0
                        iCount = 0
                        Do While iCount < dv2.Count
                            If dv2.Item(iCount)("Datum_zur_Sperre").ToString <> "" Then
                                intGsp += 1
                                carportrow("Gesperrte_Fahrzeuge") = intGsp
                                carportrow("Zulassungsf_Fahrzeuge") = intZul
                            Else
                                intZul += 1
                                carportrow("Gesperrte_Fahrzeuge") = intGsp
                                carportrow("Zulassungsf_Fahrzeuge") = intZul

                            End If
                            iCount = iCount + 1
                        Loop

                        .Rows.Add(carportrow)
                        dv.RowFilter = m_strFilter
                    End If

                End With
                e = e + 1
            Loop

        End If
        Return ""
    End Function

    Public Sub Show()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim vwTemp As DataView = m_tblModelle.DefaultView
                vwTemp.RowFilter = "Carportnr = '" & m_strCarportNr & "'"
                Dim intTemp As Int32 = vwTemp.Count
                vwTemp.RowFilter = ""

                If intTemp = 0 Then
                    'Dim tblTemp As DataTable = m_tblSaveCars
                    If Not AddModelle() Then
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
                    For i = 0 To m_tblCarports.Rows.Count - 1
                        If m_tblCarports.Rows(i)("Carportnr").ToString = m_strCarportNr Then
                            m_tblCarports.Rows(i)("Loaded") = True
                            m_tblCarports.Rows(i)("Details") = blnDetails
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

    Private Function AddModelle() As Boolean
        Try
            Dim rowNew As DataRow
            Dim rowNewFzg As DataRow
            Dim CarportRows() As DataRow
            Dim row As DataRow
            Dim rowCar As DataRow
            Dim rowMod As DataRow
            Dim oldID As Integer = 0
            Dim countFzg As Integer = 0
            Dim TypID As String = ""
            Dim CarportNr As String = ""
            Dim sDate As String
            'Dataview!!!!!!!!!!!'
            RemoveRows()

            For Each row In m_tblResult.Rows
                If CarportNr <> row("Carportnr").ToString Then
                    CarportNr = row("Carportnr").ToString
                    CarportRows = m_tblResult.Select("Carportnr = '" & CarportNr & "' And " & m_strFilter)
                    'CarportRows.Sort(Sort)
                    TypID = ""
                    For Each rowCar In CarportRows

                        If TypID <> rowCar("Typ_ID_Avis").ToString Then

                            rowNew = m_tblModelle.NewRow
                            m_intLastID += 1
                            rowNew("ID") = m_intLastID
                            rowNew("Carportnr") = rowCar("Carportnr")
                            rowNew("Carport") = rowCar("Carport")
                            rowNew("Hersteller_ID_Avis") = rowCar("Hersteller_ID_Avis")
                            rowNew("Herstellername") = rowCar("Herstellername")
                            rowNew("Typ_ID_Avis") = rowCar("Typ_ID_Avis")
                            rowNew("Modellbezeichnung") = rowCar("Modellbezeichnung")
                            rowNew("Reifenart") = rowCar("Reifenart")
                            rowNew("Liefermonat") = rowCar("Liefermonat")
                            rowNew("Kraftstoffart") = rowCar("Kraftstoffart")
                            rowNew("Navigation") = rowCar("Navigation")
                            If rowCar("Navigation").ToString = "J" Then
                                rowNew("Navigation") = "Ja"
                            ElseIf rowCar("Navigation").ToString = "N" Then
                                rowNew("Navigation") = "Nein"
                            End If
                            rowNew("Farbe") = rowCar("Farbe")
                            rowNew("Vermietgruppe") = rowCar("Vermietgruppe")
                            rowNew("Fahrzeugart") = rowCar("Fahrzeugart")
                            rowNew("Bezahltkennzeichen") = False
                            If row("Bezahltkennzeichen").ToString = "1" Then
                                rowNew("Bezahltkennzeichen") = True
                            End If

                            rowNew("HaendlerId") = rowCar("HaendlerId")
                            rowNew("Haendler_Kurzname") = rowCar("Haendler_Kurzname")
                            rowNew("Zulassungsort") = rowCar("Zulassungsort")
                            rowNew("Einkaufsindikator") = rowCar("Einkaufsindikator")
                            rowNew("Anzahl_alt") = 0
                            rowNew("Anzahl_neu") = 0
                            rowNew("ZZCARPORT") = rowCar("ZZCARPORT")
                            rowNew("Task") = "Zulassen"
                            rowNew("EQUNR") = rowCar("EQUNR")
                            rowNew("QMNUM") = rowCar("QMNUM")
                            TypID = rowCar("Typ_ID_Avis").ToString
                            m_tblModelle.Rows.Add(rowNew)
                        End If
                    Next
                End If
            Next
            TypID = ""
            CarportNr = ""
            For Each rowMod In m_tblModelle.Rows
                TypID = rowMod("Typ_ID_Avis").ToString
                Dim sTypFilter As String = "AND Typ_ID_Avis='" & TypID & "'"
                CarportNr = rowMod("Carportnr").ToString
                If m_strFilter <> "" Then
                    CarportRows = m_tblResult.Select("Carportnr = '" & CarportNr & "' And " & m_strFilter & " " & sTypFilter)
                Else
                    CarportRows = m_tblResult.Select("Carportnr = '" & CarportNr & "' " & sTypFilter)
                End If

                For Each rowCar In CarportRows
                    rowNewFzg = m_tblFahrzeuge.NewRow
                    rowNewFzg("Modell_ID") = rowMod("ID")
                    rowNewFzg("Fahrgestellnummer") = rowCar("Fahrgestellnummer")
                    rowNewFzg("Eingangsdatum") = rowCar("Eingangsdatum")
                    rowNewFzg("Verwendungszweck") = rowCar("Verwendungszweck")
                    rowNewFzg("Owner_Code") = rowCar("Owner_Code")
                    rowNewFzg("Ausgewaehlt") = False

                    sDate = rowCar("Datum_zur_Sperre").ToString
                    If IsDate(sDate) Then
                        rowNewFzg("Datum_zur_Sperre") = CDate(sDate).ToShortDateString
                    Else
                        rowNewFzg("Datum_zur_Sperre") = rowCar("Datum_zur_Sperre")
                        rowMod("Anzahl_alt") = CInt(rowMod("Anzahl_alt")) + 1
                    End If
                    rowNewFzg("Sperrvermerk") = rowCar("Sperrvermerk")
                    rowNewFzg("ZZCARPORT") = rowCar("ZZCARPORT")
                    rowNewFzg("EQUNR") = rowCar("EQUNR")
                    rowNewFzg("QMNUM") = rowCar("QMNUM")
                    rowNewFzg("Reifenart") = rowCar("Reifenart")
                    rowNewFzg("Verwendungszweck") = rowCar("Verwendungszweck")
                    rowNewFzg("Zulassungsort") = rowCar("Zulassungsort")
                    oldID = m_intLastID

                    m_tblFahrzeuge.Rows.Add(rowNewFzg)
                Next
            Next

            Return True
        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = ex.Message
            Return False
        End Try
    End Function

    Private Function AddColumns() As DataTable
        Dim TableCars As New DataTable

        With TableCars.Columns
            .Add("AKTION", System.Type.GetType("System.String"))
            .Add("CHASSIS_NUM", System.Type.GetType("System.String"))
            .Add("LICENSE_NUM", System.Type.GetType("System.String"))
            .Add("ZULASSUNGSORT", System.Type.GetType("System.String"))
            .Add("VERWENDUNGSZWECK", System.Type.GetType("System.String"))
            .Add("REIFENART", System.Type.GetType("System.String"))
            .Add("ZZCARPORT", System.Type.GetType("System.String"))
            .Add("EQUNR", System.Type.GetType("System.String"))
            .Add("QMNUM", System.Type.GetType("System.String"))
            .Add("ZULDAT", System.Type.GetType("System.String"))
            .Add("DAT_SPERRE", System.Type.GetType("System.String"))
            .Add("SPERRVERMERK", System.Type.GetType("System.String"))
            .Add("WEB_USER", System.Type.GetType("System.String"))
            .Add("FEHLER", System.Type.GetType("System.String"))
        End With

        Return TableCars
    End Function

    Private Sub RemoveRows()
        Dim count As Integer

        With m_tblFahrzeuge
            For count = .Rows.Count - 1 To 0 Step -1
                m_tblFahrzeuge.Rows.Remove(m_tblFahrzeuge.Rows(count))
            Next
        End With
        With m_tblModelle
            For count = .Rows.Count - 1 To 0 Step -1
                .Rows.Remove(.Rows(count))
            Next
        End With
    End Sub

    Public Sub SaveData(ByVal strAppID As String, ByVal strSessionID As String)

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String

        'Dim strKunnr As String = ""
        Dim TableCars As DataTable
        Dim DatRow As DataRow
        Dim i As Integer
        Dim sAktion As String = ""
        m_strClassAndMethod = "Zul_Sperr_Entsperr.SaveData"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        'strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        TableCars = S.AP.GetImportTableWithInit("Z_M_MASSENZULASSUNG_006.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

        Dim intID As Int32 = -1
        m_intFehlerCount = 0
        Try

            'TableCars = AddColumns()
            'TableCars.TableName = "GT_WEB"

            Dim vwTemp As DataView = m_tblFahrzeuge.DefaultView
            vwTemp.RowFilter = "Ausgewaehlt = True"

            For i = 0 To vwTemp.Count - 1
                If vwTemp(i)("NeuGesperrt").ToString = "1" Then
                    sAktion = "S"
                End If
                If vwTemp(i)("NeuEntsperrt").ToString = "1" Then
                    sAktion = "E"
                End If
                If vwTemp(i)("NeuEntsperrt").ToString <> "1" AndAlso vwTemp(i)("NeuGesperrt").ToString <> "1" Then
                    sAktion = "Z"
                ElseIf vwTemp(i)("NeuEntsperrt").ToString = "1" AndAlso vwTemp(i)("DatumErstzulassung").ToString <> "" Then
                    'Erst entsperren dann zulassen!!!!
                    DatRow = TableCars.NewRow
                    DatRow("AKTION") = "E"
                    DatRow("CHASSIS_NUM") = vwTemp(i)("Fahrgestellnummer").ToString
                    'DatRow("CHASSIS_NUM") = "ADWDHIUH6848218"
                    DatRow("LICENSE_NUM") = ""
                    DatRow("ZULASSUNGSORT") = vwTemp(i)("Zulassungsort").ToString
                    DatRow("VERWENDUNGSZWECK") = vwTemp(i)("Verwendungszweck").ToString
                    DatRow("REIFENART") = vwTemp(i)("Reifenart").ToString
                    DatRow("ZZCARPORT") = vwTemp(i)("ZZCARPORT").ToString
                    DatRow("EQUNR") = vwTemp(i)("EQUNR").ToString
                    DatRow("QMNUM") = vwTemp(i)("QMNUM").ToString
                    DatRow("ZULDAT") = Nothing
                    'If IsDate(vwTemp(i)("Datum_zur_Sperre").ToString) Then
                    '    DatRow("DAT_SPERRE") = MakeDateSAP(vwTemp(i)("Datum_zur_Sperre").ToString)
                    'Else
                    '    DatRow("DAT_SPERRE") = "00000000"
                    'End If

                    DatRow("SPERRVERMERK") = vwTemp(i)("Sperrvermerk").ToString
                    DatRow("WEB_USER") = Left(m_objUser.UserName, 15)
                    DatRow("FEHLER") = ""
                    TableCars.Rows.Add(DatRow)
                    sAktion = "Z"
                End If
                DatRow = TableCars.NewRow
                DatRow("AKTION") = sAktion
                DatRow("CHASSIS_NUM") = vwTemp(i)("Fahrgestellnummer").ToString
                'DatRow("CHASSIS_NUM") = "ADWDHIUH6848218"
                DatRow("LICENSE_NUM") = ""
                DatRow("ZULASSUNGSORT") = vwTemp(i)("Zulassungsort").ToString
                DatRow("VERWENDUNGSZWECK") = vwTemp(i)("Verwendungszweck").ToString
                DatRow("REIFENART") = vwTemp(i)("Reifenart").ToString
                DatRow("ZZCARPORT") = vwTemp(i)("ZZCARPORT").ToString
                DatRow("EQUNR") = vwTemp(i)("EQUNR").ToString
                DatRow("QMNUM") = vwTemp(i)("QMNUM").ToString

                DatRow("ZULDAT") = vwTemp(i)("DatumErstzulassung").ToString.NotEmptyOrDbNull
                'If IsDate(vwTemp(i)("DatumErstzulassung").ToString) Then
                '    DatRow("ZULDAT") = MakeDateSAP(vwTemp(i)("DatumErstzulassung").ToString)
                'Else
                '    DatRow("ZULDAT") = "00000000"
                'End If
                DatRow("DAT_SPERRE") = vwTemp(i)("Datum_zur_Sperre").ToString.NotEmptyOrDbNull
                'If IsDate(vwTemp(i)("Datum_zur_Sperre").ToString) Then
                '    DatRow("DAT_SPERRE") = MakeDateSAP(vwTemp(i)("Datum_zur_Sperre").ToString)
                'Else
                '    DatRow("DAT_SPERRE") = "00000000"
                'End If

                DatRow("SPERRVERMERK") = vwTemp(i)("Sperrvermerk").ToString
                DatRow("WEB_USER") = Left(m_objUser.UserName, 15)
                DatRow("FEHLER") = ""
                TableCars.Rows.Add(DatRow)

            Next

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_MASSENZULASSUNG_006", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

            'strCom = "EXEC Z_M_MASSENZULASSUNG_006 @I_KUNNR_AG='" & strKunnr & "', " _
            '                & " @I_WEB_USER='" & Left(m_objUser.UserName, 40) & "', " _
            '                & " @GT_WEB=@pSAPTableAUFTRAG," _
            '                & " @GT_WEB=@pSAPTableAUFTRAGOUT OUTPUT OPTION 'disabledatavalidation' "
            'cmd.Connection = con
            'cmd.CommandText = strCom

            'Dim pSAPTableAUFTRAG As New SAPParameter("@pSAPTableAUFTRAG", TableCars)
            'Dim pSAPTableAUFTRAGOut As New SAPParameter("@pSAPTableAUFTRAGOUT", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTableAUFTRAG)
            'cmd.Parameters.Add(pSAPTableAUFTRAGOut)

            'cmd.ExecuteNonQuery()

            S.AP.Execute()

            Dim TableBack As New DataTable
            TableBack = S.AP.GetExportTable("GT_WEB")
            'TableBack = DirectCast(pSAPTableAUFTRAGOut.Value, DataTable)

            If TableBack.Rows.Count > 0 Then
                For i = 0 To TableBack.Rows.Count - 1
                    Dim strTemp As String = TableBack.Rows(0)("FEHLER").ToString
                    vwTemp(i)("Ergebnis") = strTemp
                    m_intFehlerCount += 1
                Next
            Else
                For i = 0 To vwTemp.Count - 1
                    vwTemp(i)("Ergebnis") = "Daten gesendet!"
                Next
            End If
            m_strTask = "Ausgabe"
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            m_tblErledigt = New DataTable
            With m_tblErledigt.Columns
                .Add("Carportnr", System.Type.GetType("System.String"))
                .Add("Carport", System.Type.GetType("System.String"))
                .Add("Herstellername", System.Type.GetType("System.String"))
                .Add("Typ ID Avis", System.Type.GetType("System.String"))
                .Add("Modellbezeichnung", System.Type.GetType("System.String"))
                .Add("Reifenart", System.Type.GetType("System.String"))
                .Add("Kraftstoffart", System.Type.GetType("System.String"))
                .Add("Navigation", System.Type.GetType("System.String"))
                .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                .Add("Zulassungsort", System.Type.GetType("System.String"))
                .Add("Datum Erstzulassung", System.Type.GetType("System.String"))
                .Add("Datum_zur_Sperre", System.Type.GetType("System.String"))
                .Add("Sperrvermerk", System.Type.GetType("System.String"))
                .Add("Ergebnis", System.Type.GetType("System.String"))
            End With
            For i = 0 To m_tblFahrzeuge.Rows.Count - 1
                If CType(m_tblFahrzeuge.Rows(i)("Ausgewaehlt"), Boolean) Then

                    Dim rowTemp As DataRow
                    rowTemp = m_tblErledigt.NewRow
                    rowTemp("Fahrgestellnummer") = m_tblFahrzeuge.Rows(i)("Fahrgestellnummer")
                    rowTemp("Zulassungsort") = m_tblFahrzeuge.Rows(i)("Zulassungsort")
                    rowTemp("Datum Erstzulassung") = m_tblFahrzeuge.Rows(i)("DatumErstzulassung")
                    If IsDate(m_tblFahrzeuge.Rows(i)("DatumErstzulassung")) Then
                        rowTemp("Datum Erstzulassung") = CDate(m_tblFahrzeuge.Rows(i)("DatumErstzulassung")).ToShortDateString
                    End If
                    If IsDate(m_tblFahrzeuge.Rows(i)("Datum_zur_Sperre")) Then
                        rowTemp("Datum_zur_Sperre") = CDate(m_tblFahrzeuge.Rows(i)("Datum_zur_Sperre")).ToShortDateString
                    End If
                    rowTemp("Sperrvermerk") = m_tblFahrzeuge.Rows(i)("Sperrvermerk")
                    rowTemp("Ergebnis") = m_tblFahrzeuge.Rows(i)("Ergebnis")
                    Dim vwTemp2 As DataView = m_tblModelle.DefaultView
                    vwTemp2.RowFilter = "ID = " & m_tblFahrzeuge.Rows(i)("Modell_ID").ToString
                    rowTemp("Carportnr") = vwTemp2.Item(0)("Carportnr")
                    rowTemp("Carport") = vwTemp2.Item(0)("Carport")
                    rowTemp("Herstellername") = vwTemp2.Item(0)("Herstellername")
                    rowTemp("Typ ID Avis") = vwTemp2.Item(0)("Typ_ID_Avis")
                    rowTemp("Modellbezeichnung") = vwTemp2.Item(0)("Modellbezeichnung")
                    rowTemp("Reifenart") = vwTemp2.Item(0)("Reifenart")
                    rowTemp("Kraftstoffart") = vwTemp2.Item(0)("Kraftstoffart")
                    rowTemp("Navigation") = vwTemp2.Item(0)("Navigation")
                    vwTemp2.RowFilter = ""

                    m_tblErledigt.Rows.Add(rowTemp)
                End If
            Next


            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", I_EING_DAT_BIS=" & m_datEingangsdatumBis.ToShortDateString, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'cmd.Dispose()
            'con.Close()
            'con.Dispose()
            m_blnGestartet = False
        End Try
    End Sub

    Public Sub SaveDataDez(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        'Dim cmd As New SAPCommand()
        'Dim strKunnr As String = ""
        Dim TableCars As DataTable
        Dim DatRow As DataRow
        Dim i As Integer
        Dim sAktion As String = ""
        m_strClassAndMethod = "Zul_Sperr_Entsperr.SaveDataDez"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        'strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim intID As Int32 = -1
        m_intFehlerCount = 0
        Try
            Dim vwTemp As DataView = m_tblFahrzeuge.DefaultView
            vwTemp.RowFilter = "Ausgewaehlt = True"

            m_intStatus = 0
            m_strMessage = ""


            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_DEZ_ZUL_001", m_objApp, m_objUser, Page)
            'myProxy.setImportParameter("I_KUNNR_AG", strKunnr)

            'TableCars = myProxy.getImportTable("GT_WEB")

            TableCars = S.AP.GetImportTableWithInit("Z_DPM_DEZ_ZUL_001.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

            For i = 0 To vwTemp.Count - 1


                If vwTemp(i)("NeuGesperrt").ToString = "1" Then
                    sAktion = "S"
                End If
                If vwTemp(i)("NeuEntsperrt").ToString = "1" Then
                    sAktion = "E"
                End If
                If vwTemp(i)("NeuEntsperrt").ToString <> "1" AndAlso vwTemp(i)("NeuGesperrt").ToString <> "1" Then
                    sAktion = "Z"
                ElseIf vwTemp(i)("NeuEntsperrt").ToString = "1" AndAlso vwTemp(i)("DatumErstzulassung").ToString <> "" Then
                    'Erst entsperren dann zulassen!!!!
                    DatRow = TableCars.NewRow
                    DatRow("AKTION") = "E"
                    DatRow("CHASSIS_NUM") = vwTemp(i)("Fahrgestellnummer").ToString
                    'DatRow("CHASSIS_NUM") = "ADWDHIUH6848218"
                    DatRow("LICENSE_NUM") = ""
                    DatRow("ZULASSUNGSORT") = vwTemp(i)("Zulassungsort").ToString
                    DatRow("VERWENDUNGSZWECK") = vwTemp(i)("Verwendungszweck").ToString
                    DatRow("REIFENART") = vwTemp(i)("Reifenart").ToString
                    DatRow("ZZCARPORT") = vwTemp(i)("ZZCARPORT").ToString
                    DatRow("EQUNR") = vwTemp(i)("EQUNR").ToString
                    DatRow("QMNUM") = vwTemp(i)("QMNUM").ToString
                    If IsDate(vwTemp(i)("Datum_zur_Sperre").ToString) Then
                        DatRow("DAT_SPERRE") = vwTemp(i)("Datum_zur_Sperre").ToString
                    End If

                    DatRow("SPERRVERMERK") = vwTemp(i)("Sperrvermerk").ToString
                    DatRow("WEB_USER") = Left(m_objUser.UserName, 15)
                    DatRow("FEHLER") = ""
                    TableCars.Rows.Add(DatRow)
                    sAktion = "Z"
                End If
                DatRow = TableCars.NewRow
                DatRow("AKTION") = sAktion
                DatRow("CHASSIS_NUM") = vwTemp(i)("Fahrgestellnummer").ToString
                'DatRow("CHASSIS_NUM") = "ADWDHIUH6848218"
                DatRow("LICENSE_NUM") = ""
                DatRow("ZULASSUNGSORT") = vwTemp(i)("Zulassungsort").ToString
                DatRow("VERWENDUNGSZWECK") = vwTemp(i)("Verwendungszweck").ToString
                DatRow("REIFENART") = vwTemp(i)("Reifenart").ToString
                DatRow("ZZCARPORT") = vwTemp(i)("ZZCARPORT").ToString
                DatRow("EQUNR") = vwTemp(i)("EQUNR").ToString
                DatRow("QMNUM") = vwTemp(i)("QMNUM").ToString

                DatRow("ZULDAT") = vwTemp(i)("DatumErstzulassung").ToString.NotEmptyOrDbNull

                DatRow("DAT_SPERRE") = vwTemp(i)("Datum_zur_Sperre").ToString.NotEmptyOrDbNull

                DatRow("SPERRVERMERK") = vwTemp(i)("Sperrvermerk").ToString
                DatRow("WEB_USER") = Left(m_objUser.UserName, 15)
                DatRow("FEHLER") = ""
                TableCars.Rows.Add(DatRow)

            Next

            'myProxy.callBapi()
            S.AP.Execute()

            'Dim tblTemp As DataTable = myProxy.getExportTable("GT_WEB")
            Dim tblTemp As DataTable = S.AP.GetExportTable("GT_WEB")

            Dim TableBack As New DataTable
            TableBack = tblTemp
            If TableBack.Rows.Count > 0 Then
                For i = 0 To TableBack.Rows.Count - 1
                    Dim strTemp As String = TableBack.Rows(0)("FEHLER").ToString
                    vwTemp(i)("Ergebnis") = strTemp
                    m_intFehlerCount += 1
                Next
            Else
                For i = 0 To vwTemp.Count - 1
                    vwTemp(i)("Ergebnis") = "Daten gesendet!"
                Next
            End If
            m_strTask = "Ausgabe"
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            m_tblErledigt = New DataTable
            With m_tblErledigt.Columns
                .Add("Carportnr", System.Type.GetType("System.String"))
                .Add("Carport", System.Type.GetType("System.String"))
                .Add("Herstellername", System.Type.GetType("System.String"))
                .Add("Typ ID Avis", System.Type.GetType("System.String"))
                .Add("Modellbezeichnung", System.Type.GetType("System.String"))
                .Add("Reifenart", System.Type.GetType("System.String"))
                .Add("Kraftstoffart", System.Type.GetType("System.String"))
                .Add("Navigation", System.Type.GetType("System.String"))
                .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                .Add("Zulassungsort", System.Type.GetType("System.String"))
                .Add("Datum Erstzulassung", System.Type.GetType("System.String"))
                .Add("Datum_zur_Sperre", System.Type.GetType("System.String"))
                .Add("Sperrvermerk", System.Type.GetType("System.String"))
                .Add("Ergebnis", System.Type.GetType("System.String"))
            End With
            For i = 0 To m_tblFahrzeuge.Rows.Count - 1
                If CType(m_tblFahrzeuge.Rows(i)("Ausgewaehlt"), Boolean) Then

                    Dim rowTemp As DataRow
                    rowTemp = m_tblErledigt.NewRow
                    rowTemp("Fahrgestellnummer") = m_tblFahrzeuge.Rows(i)("Fahrgestellnummer")
                    rowTemp("Zulassungsort") = m_tblFahrzeuge.Rows(i)("Zulassungsort")
                    rowTemp("Datum Erstzulassung") = m_tblFahrzeuge.Rows(i)("DatumErstzulassung")
                    If IsDate(m_tblFahrzeuge.Rows(i)("DatumErstzulassung")) Then
                        rowTemp("Datum Erstzulassung") = CDate(m_tblFahrzeuge.Rows(i)("DatumErstzulassung")).ToShortDateString
                    End If
                    If IsDate(m_tblFahrzeuge.Rows(i)("Datum_zur_Sperre")) Then
                        rowTemp("Datum_zur_Sperre") = CDate(m_tblFahrzeuge.Rows(i)("Datum_zur_Sperre")).ToShortDateString
                    End If
                    rowTemp("Sperrvermerk") = m_tblFahrzeuge.Rows(i)("Sperrvermerk")
                    rowTemp("Ergebnis") = m_tblFahrzeuge.Rows(i)("Ergebnis")
                    Dim vwTemp2 As DataView = m_tblModelle.DefaultView
                    vwTemp2.RowFilter = "ID = " & m_tblFahrzeuge.Rows(i)("Modell_ID").ToString
                    rowTemp("Carportnr") = vwTemp2.Item(0)("Carportnr")
                    rowTemp("Carport") = vwTemp2.Item(0)("Carport")
                    rowTemp("Herstellername") = vwTemp2.Item(0)("Herstellername")
                    rowTemp("Typ ID Avis") = vwTemp2.Item(0)("Typ_ID_Avis")
                    rowTemp("Modellbezeichnung") = vwTemp2.Item(0)("Modellbezeichnung")
                    rowTemp("Reifenart") = vwTemp2.Item(0)("Reifenart")
                    rowTemp("Kraftstoffart") = vwTemp2.Item(0)("Kraftstoffart")
                    rowTemp("Navigation") = vwTemp2.Item(0)("Navigation")
                    vwTemp2.RowFilter = ""

                    m_tblErledigt.Rows.Add(rowTemp)
                End If
            Next


            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", I_EING_DAT_BIS=" & m_datEingangsdatumBis.ToShortDateString, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        Finally
            m_blnGestartet = False
        End Try
    End Sub

    Public Function SelectCars() As Int32

        Try
            m_dsCarPort_Data_Selected = m_dsCarPort_Data.Copy
            Dim i As Int32
            Dim vwTemp As DataView = m_dsCarPort_Data_Selected.Tables("Fahrzeuge").DefaultView
            vwTemp.RowFilter = "Ausgewaehlt = True"

            Dim intReturn As Int32 = vwTemp.Count
            vwTemp.RowFilter = ""

            If intReturn > 0 Then
                For i = m_dsCarPort_Data_Selected.Tables("Fahrzeuge").Rows.Count - 1 To 0 Step -1
                    If Not CType(m_dsCarPort_Data_Selected.Tables("Fahrzeuge").Rows(i)("Ausgewaehlt"), Boolean) Then
                        m_dsCarPort_Data_Selected.Tables("Fahrzeuge").AcceptChanges()
                        m_dsCarPort_Data_Selected.Tables("Fahrzeuge").Rows(i).Delete()
                        m_dsCarPort_Data_Selected.Tables("Fahrzeuge").AcceptChanges()
                    End If
                Next
                For i = m_dsCarPort_Data_Selected.Tables("Modelle").Rows.Count - 1 To 0 Step -1
                    vwTemp = m_dsCarPort_Data_Selected.Tables("Fahrzeuge").DefaultView
                    vwTemp.RowFilter = "Modell_ID = " & m_dsCarPort_Data_Selected.Tables("Modelle").Rows(i)("ID").ToString
                    If vwTemp.Count = 0 Then
                        m_dsCarPort_Data_Selected.Tables("Modelle").AcceptChanges()
                        m_dsCarPort_Data_Selected.Tables("Modelle").Rows(i).Delete()
                        m_dsCarPort_Data_Selected.Tables("Modelle").AcceptChanges()
                    Else
                        m_dsCarPort_Data_Selected.Tables("Modelle").Rows(i)("Anzahl_neu") = vwTemp.Count
                    End If
                    vwTemp.RowFilter = ""
                Next
                For i = m_dsCarPort_Data_Selected.Tables("Carports").Rows.Count - 1 To 0 Step -1
                    vwTemp = m_dsCarPort_Data_Selected.Tables("Modelle").DefaultView
                    vwTemp.RowFilter = "Carportnr = '" & m_dsCarPort_Data_Selected.Tables("Carports").Rows(i)("Carportnr").ToString & "'"
                    If vwTemp.Count = 0 Then
                        m_dsCarPort_Data_Selected.Tables("Carports").AcceptChanges()
                        m_dsCarPort_Data_Selected.Tables("Carports").Rows(i).Delete()
                        m_dsCarPort_Data_Selected.Tables("Carports").AcceptChanges()
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

    Public Sub createZulassungCount(ByVal Zuldatum As String)
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String
        'Dim strKunnr As String = ""
        m_strClassAndMethod = "Zul_Sperr_Entsperr.createZulassungCount"
        'strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim intID As Int32 = -1
        Dim tblTemp2 As DataTable

        'If Zuldatum = "" Then
        '    Zuldatum = "00000000"
        'Else
        '    Zuldatum = MakeDateSAP(Zuldatum)
        'End If

        Try
            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_ANZ_BEAUFTR_ZUL_006", "", "", m_objUser.CurrentLogAccessASPXID)

            tblTemp2 = S.AP.GetExportTableWithInitExecute("Z_M_ANZ_BEAUFTR_ZUL_006.GT_WEB",
                                                         "I_KUNNR, I_ZULDAT",
                                                         m_objUser.KUNNR.ToSapKunnr(), Zuldatum)

            'strCom = "EXEC Z_M_ANZ_BEAUFTR_ZUL_006 @I_KUNNR='" & strKunnr & "', " _
            '                                    & "@I_ZULDAT='" & Zuldatum & "', " _
            '                                    & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom


            'Dim pSAPTableAUFTRAG As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTableAUFTRAG)

            'cmd.ExecuteNonQuery()

            'tblTemp2 = DirectCast(pSAPTableAUFTRAG.Value, DataTable)

            m_tblZulCount = tblTemp2

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, tblTemp2, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'cmd.Dispose()
            'con.Close()
            'con.Dispose()
            m_blnGestartet = False
        End Try

    End Sub
#End Region
End Class
' ************************************************
' $History: Zul_Sperr_Entsperr.vb $
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 18.11.09   Time: 14:08
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 30.10.09   Time: 11:14
' Updated in $/CKAG/Applications/AppAvis/lib
' ita�s: 3216, 3155
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 10.09.09   Time: 13:15
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 28.04.09   Time: 17:09
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 13.01.09   Time: 13:14
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 6.01.09    Time: 14:15
' Updated in $/CKAG/Applications/AppAvis/lib
' Anpassungen
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 30.12.08   Time: 15:50
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 11.12.08   Time: 14:48
' Updated in $/CKAG/Applications/AppAvis/lib
' AddModelle verndert: nur ungesperrte Fahrzeugen z�hlen  "Anzahl_alt"
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 1.12.08    Time: 16:13
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA: 2359 & kleinere Anpassungen
' 
' ************************************************
