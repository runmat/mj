Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class Sixt_B15

    Inherits CKG.Base.Business.DatenimportBase

#Region "Declarations"
    Private Enum Blocken
        Anlegen = 1
        Freigeben = 2
        Loeschen = 3
        Anzeigen = 4
        Vorschlaege = 5
    End Enum

    Private m_dsBlocken_Data_Selected As DataSet
    Private m_dsBlocken_Data As DataSet
    Private m_Ausfuehrung As DataTable
    Private m_Farben As DataTable
    Private m_Hersteller As DataTable
    Private m_Navi As DataTable
    Private m_Reifen As DataTable
    Private m_Master As DataTable
    Private m_Detail As DataTable
    Private m_Antrieb As DataTable
    Private m_Halter As DataTable
    Private m_Versicherer As DataTable
    Private m_GroupRegel As DataTable
    Private m_tblModelle As DataTable
    Private m_tblFahrzeuge As DataTable
    Private m_Action As Int32
    Private m_strRegelname As String
    Private m_strLizLim As String
    Private m_strHersteller As String
    Private m_strPDI As String
    Private m_strReferenz As String
    Private m_strFIN As String
    Private m_strHandelsname As String
    Private m_Mail As String
    Private m_UserName As String
    Private m_FirstName As String
    Private m_LastName As String
    Private m_strModell As String
    Private m_strAntrieb As String
    Private m_strLeistKW As String
    Private m_strFarbe As String
    Private m_strKstLN As String
    Private m_strNameLN As String
    Private m_strVersicherung As String
    Private m_strAnzahl As String
    Private m_strZulSixt As String
    Private m_strGeplStation As String
    Private m_strNavi As String
    Private m_strReifen As String
    Private m_strAusfuehrung As String
    Private m_ErrMessage As String
    Private m_pIndex As Int32
    Private m_DataCount As Int32
    Private m_intSelectedCars As Int32
    Private m_CountErr As Boolean

#End Region

#Region "Properties"

    Public ReadOnly Property Blocken_Data_Selected() As DataSet
        Get
            Return m_dsBlocken_Data_Selected
        End Get
    End Property
    Public ReadOnly Property Blocken_Data() As DataSet
        Get
            Return m_dsBlocken_Data
        End Get
    End Property

    Public Property pIndex() As Int32
        Get
            Return m_pIndex
        End Get
        Set(ByVal Value As Int32)
            m_pIndex = Value
        End Set
    End Property

    Public Property CountErr() As Boolean
        Get
            Return m_CountErr
        End Get
        Set(ByVal Value As Boolean)
            m_CountErr = Value
        End Set
    End Property

    Public Property ErrMessage() As String
        Get
            Return m_ErrMessage
        End Get
        Set(ByVal Value As String)
            m_ErrMessage = Value
        End Set
    End Property


    Public Property AusfuehrungValue() As String
        Get
            Return m_strAusfuehrung
        End Get
        Set(ByVal Value As String)
            m_strAusfuehrung = Value
        End Set
    End Property

    Public Property Regelname() As String
        Get
            Return m_strRegelname
        End Get
        Set(ByVal Value As String)
            m_strRegelname = Value
        End Set
    End Property

    Public Property Mail() As String
        Get
            Return m_Mail
        End Get
        Set(ByVal Value As String)
            m_Mail = Value
        End Set
    End Property


    Public Property UserName() As String
        Get
            Return m_UserName
        End Get
        Set(ByVal Value As String)
            m_UserName = Value
        End Set
    End Property


    Public Property FirstName() As String
        Get
            Return m_FirstName
        End Get
        Set(ByVal Value As String)
            m_FirstName = Value
        End Set
    End Property

    Public Property LastName() As String
        Get
            Return m_LastName
        End Get
        Set(ByVal Value As String)
            m_LastName = Value
        End Set
    End Property


    Public Property ReifenValue() As String
        Get
            Return m_strReifen
        End Get
        Set(ByVal Value As String)
            m_strReifen = Value
        End Set
    End Property

    Public Property NaviValue() As String
        Get
            Return m_strNavi
        End Get
        Set(ByVal Value As String)
            m_strNavi = Value
        End Set
    End Property


    Public Property GeplStation() As String
        Get
            Return m_strGeplStation
        End Get
        Set(ByVal Value As String)
            m_strGeplStation = Value
        End Set
    End Property

    Public Property ZulSixt() As String
        Get
            Return m_strZulSixt
        End Get
        Set(ByVal Value As String)
            m_strZulSixt = Value
        End Set
    End Property

    Public Property Anzahl() As String
        Get
            Return m_strAnzahl
        End Get
        Set(ByVal Value As String)
            m_strAnzahl = Value
        End Set
    End Property

    Public Property Versicherung() As String
        Get
            Return m_strVersicherung
        End Get
        Set(ByVal Value As String)
            m_strVersicherung = Value
        End Set
    End Property

    Public Property KstName() As String
        Get
            Return m_strNameLN
        End Get
        Set(ByVal Value As String)
            m_strNameLN = Value
        End Set
    End Property

    Public Property KstLN() As String
        Get
            Return m_strKstLN
        End Get
        Set(ByVal Value As String)
            m_strKstLN = Value
        End Set
    End Property

    Public Property Farbe() As String
        Get
            Return m_strFarbe
        End Get
        Set(ByVal Value As String)
            m_strFarbe = Value
        End Set
    End Property

    Public Property LeistKW() As String
        Get
            Return m_strLeistKW
        End Get
        Set(ByVal Value As String)
            m_strLeistKW = Value
        End Set
    End Property

    Public Property AntriebValue() As String
        Get
            Return m_strAntrieb
        End Get
        Set(ByVal Value As String)
            m_strAntrieb = Value
        End Set
    End Property

    Public Property Modell() As String
        Get
            Return m_strModell
        End Get
        Set(ByVal Value As String)
            m_strModell = Value
        End Set
    End Property

    Public Property Handelsname() As String
        Get
            Return m_strHandelsname
        End Get
        Set(ByVal Value As String)
            m_strHandelsname = Value
        End Set
    End Property

    Public Property FIN() As String
        Get
            Return m_strFIN
        End Get
        Set(ByVal Value As String)
            m_strFIN = Value
        End Set
    End Property

    Public Property Referenz() As String
        Get
            Return m_strReferenz
        End Get
        Set(ByVal Value As String)
            m_strReferenz = Value
        End Set
    End Property

    Public Property PDI() As String
        Get
            Return m_strPDI
        End Get
        Set(ByVal Value As String)
            m_strPDI = Value
        End Set
    End Property

    Public Property HerstellerValue() As String
        Get
            Return m_strHersteller
        End Get
        Set(ByVal Value As String)
            m_strHersteller = Value
        End Set
    End Property

    Public Property LizLim() As String
        Get
            Return m_strLizLim
        End Get
        Set(ByVal Value As String)
            m_strLizLim = Value
        End Set
    End Property


    Public Property Ausfuehrung() As DataTable
        Get
            Return m_Ausfuehrung
        End Get
        Set(ByVal Value As DataTable)
            m_Ausfuehrung = Value
        End Set
    End Property

    Public Property Versicherer() As DataTable
        Get
            Return m_Versicherer
        End Get
        Set(ByVal Value As DataTable)
            m_Versicherer = Value
        End Set
    End Property

    Public Property Halter() As DataTable
        Get
            Return m_Halter
        End Get
        Set(ByVal Value As DataTable)
            m_Halter = Value
        End Set
    End Property


    Public Property Farben() As DataTable
        Get
            Return m_Farben
        End Get
        Set(ByVal Value As DataTable)
            m_Farben = Value
        End Set
    End Property

    Public Property Hersteller() As DataTable
        Get
            Return m_Hersteller
        End Get
        Set(ByVal Value As DataTable)
            m_Hersteller = Value
        End Set
    End Property

    Public Property Navi() As DataTable
        Get
            Return m_Navi
        End Get
        Set(ByVal Value As DataTable)
            m_Navi = Value
        End Set
    End Property

    Public Property Reifen() As DataTable
        Get
            Return m_Reifen
        End Get
        Set(ByVal Value As DataTable)
            m_Reifen = Value
        End Set
    End Property

    Public Property Antrieb() As DataTable
        Get
            Return m_Antrieb
        End Get
        Set(ByVal Value As DataTable)
            m_Antrieb = Value
        End Set
    End Property


    Public Property Master() As DataTable
        Get
            Return m_Master
        End Get
        Set(ByVal Value As DataTable)
            m_Master = Value
        End Set
    End Property

    Public Property Detail() As DataTable
        Get
            Return m_Detail
        End Get
        Set(ByVal Value As DataTable)
            m_Detail = Value
        End Set
    End Property

    Public Property Aktion() As Int32
        Get
            Return m_Action
        End Get
        Set(ByVal Value As Int32)
            m_Action = Value
        End Set
    End Property

    Public ReadOnly Property SelectedCars() As Int32
        Get
            Return m_intSelectedCars
        End Get
    End Property

#End Region


#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)

        If Not m_blnGestartet Then
            m_blnGestartet = True

            '--- Regeln -----------------------------------------------------------
            m_dsBlocken_Data = New DataSet()
            m_GroupRegel = New DataTable("Regeln")

            With m_GroupRegel.Columns
                .Add("RegelID", System.Type.GetType("System.Int32"))
                .Add("Regelname", System.Type.GetType("System.String"))
                .Add("Erstellungsdatum", System.Type.GetType("System.String"))
                .Add("Regelersteller", System.Type.GetType("System.String"))
                .Add("Regelgruppe", System.Type.GetType("System.String"))
                .Add("Anzahl", System.Type.GetType("System.Int32"))
                .Add("Details", System.Type.GetType("System.Boolean"))
                .Add("Loaded", System.Type.GetType("System.Boolean"))
            End With

            '--- Modelle ---------------------------------------------------------
            m_tblModelle = New DataTable("Modelle")

            With m_tblModelle.Columns
                .Add("RegelID", System.Type.GetType("System.Int32"))
                .Add("PDI_Name", System.Type.GetType("System.String"))
                .Add("Modell_ID", System.Type.GetType("System.String"))
                .Add("BezeichnungltBrief", System.Type.GetType("System.String"))
                .Add("SummeVorschlaege", System.Type.GetType("System.Int32"))
            End With

            '--- FAHRZEUGE ---------------------------------------------------------
            m_tblFahrzeuge = New DataTable("Fahrzeuge")

            With m_tblFahrzeuge.Columns
                .Add("RegelID", System.Type.GetType("System.Int32"))
                .Add("Modell_ID", System.Type.GetType("System.String"))
                .Add("PDI", System.Type.GetType("System.String"))
                .Add("Referenz", System.Type.GetType("System.String"))
                .Add("FIN", System.Type.GetType("System.String"))
                .Add("Kraftstoff", System.Type.GetType("System.String"))
                .Add("Hersteller", System.Type.GetType("System.String"))
                .Add("LeistungKW", System.Type.GetType("System.String"))
                .Add("Navi", System.Type.GetType("System.String"))
                .Add("Bereifung", System.Type.GetType("System.String"))
                .Add("Farbe", System.Type.GetType("System.String"))
                .Add("Ausfuehrung", System.Type.GetType("System.String"))
                .Add("Ausgewaehlt", System.Type.GetType("System.Boolean"))
                .Add("Posnr", System.Type.GetType("System.String"))
                .Add("Regelname", System.Type.GetType("System.String"))
                .Add("EQUNR", System.Type.GetType("System.String"))
            End With

            m_dsBlocken_Data.Tables.Add(m_GroupRegel)
            m_dsBlocken_Data.Tables.Add(m_tblModelle)
            m_dsBlocken_Data.Tables.Add(m_tblFahrzeuge)

            Dim dc1 As DataColumn
            Dim dc2 As DataColumn
            'Relation Author => Title
            dc1 = m_dsBlocken_Data.Tables("Regeln").Columns("RegelID")
            dc2 = m_dsBlocken_Data.Tables("Modelle").Columns("RegelID")
            Dim dr As DataRelation = New DataRelation("Regeln_Modell", dc1, dc2, False)
            m_dsBlocken_Data.Relations.Add(dr)

            'Relation Title => Sales
            dc1 = m_dsBlocken_Data.Tables("Modelle").Columns("Modell_ID")
            dc2 = m_dsBlocken_Data.Tables("Fahrzeuge").Columns("Modell_ID")
            dr = New DataRelation("Modell_Fahrzeug", dc1, dc2, False)
            m_dsBlocken_Data.Relations.Add(dr)

            m_blnGestartet = False
        End If

    End Sub

    Public Sub Save(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)
            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Blocken_Anlegen", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("ANZAHL", Anzahl)

                S.AP.Init("Z_M_Blocken_Anlegen", "ANZAHL", Anzahl)

                'Dim tblSap As DataTable = myProxy.getImportTable("INPUT")
                Dim tblSap As DataTable = S.AP.GetImportTable("INPUT")

                Dim DtRow As DataRow
                DtRow = tblSap.NewRow
                DtRow("Regelname") = Me.Regelname
                DtRow("Mail") = m_objUser.Email
                DtRow("Chassis_Num") = Me.FIN
                DtRow("Costl") = Me.KstLN
                DtRow("Kennnz_Li") = Me.LizLim
                DtRow("Kunpdi") = Me.PDI
                DtRow("Liznr") = Me.Referenz
                DtRow("Name") = Me.KstName
                DtRow("Station") = Me.GeplStation
                DtRow("Userid") = m_objUser.UserName
                DtRow("Vorname") = m_objUser.FirstName
                DtRow("Name1") = m_objUser.LastName

                If Me.Farbe <> "0000" Then
                    DtRow("Zfarbe") = Me.Farbe
                End If


                If Me.AusfuehrungValue <> "0000" Then
                    DtRow("Zzausf") = Me.AusfuehrungValue
                End If

                DtRow("Zzbezei") = Me.Modell
                DtRow("Zzhandelsname") = Me.Handelsname
                DtRow("Zzherst_Text") = Me.HerstellerValue


                If Me.AntriebValue <> "Keine Auswahl" Then
                    DtRow("Zzkraftstoff_Txt") = Me.AntriebValue
                End If

                If Me.Versicherung <> "Keine Auswahl" Then
                    DtRow("Versicherer") = Left(Me.Versicherung, Me.Versicherung.IndexOf("-")).TrimEnd(" "c)
                End If

                If Me.ZulSixt <> "Keine Auswahl" Then
                    DtRow("Name") = Me.ZulSixt
                End If


                If Me.NaviValue <> "0000" Then
                    DtRow("Zznavi") = Me.NaviValue
                End If

                DtRow("Zznennleistung") = Me.LeistKW

                If Me.ReifenValue <> "0000" Then
                    DtRow("Zzreifen") = Me.ReifenValue
                End If

                tblSap.Rows.Add(DtRow)

                'myProxy.callBapi()
                S.AP.Execute()

                WriteLogEntry(True, "Anlegen.", m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -5555
                m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                ErrMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                WriteLogEntry(False, "Fehler beim Anlegen, BAPI: Z_M_BLOCKEN_ANLEGEN", m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub SaveData(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID



        Dim strSapKunnr As String
        Dim strBapiName As String = ""

        Dim vwTemp As DataView = m_tblFahrzeuge.DefaultView
        Dim i As Int32


        strSapKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        vwTemp.RowFilter = "Ausgewaehlt = True"

        Dim intID As Int32 = -1

        Try
            If Me.Aktion = Sixt_B15.Blocken.Vorschlaege Then

                strBapiName = "Z_M_BLOCKEN_BLOCKEN"
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_BLOCKEN", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("KUNNR", strSapKunnr)

                S.AP.Init(strBapiName, "KUNNR", strSapKunnr)

                'Dim tblSap As DataTable = myProxy.getImportTable("INPUT")
                Dim tblSap As DataTable = S.AP.GetImportTable("INPUT")

                Dim DtRow As DataRow
                For i = 0 To vwTemp.Count - 1
                    DtRow = tblSap.NewRow
                    DtRow("Regel_Id") = Right("0000000000" & vwTemp(i)("RegelID").ToString, 10)
                    DtRow("Regelname") = vwTemp(i)("Regelname").ToString
                    DtRow("Chassis_Num") = vwTemp(i)("Fin").ToString
                    DtRow("Kunnr") = strSapKunnr

                    tblSap.Rows.Add(DtRow)

                Next

                'myProxy.callBapi()
                S.AP.Execute()
            Else
                strBapiName = "Z_M_BLOCKEN_FREIGEBEN"

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_FREIGEBEN", m_objApp, m_objUser, page)
                S.AP.Init(strBapiName)

                'Dim tblSapFreigeben As DataTable = myProxy.getImportTable("INPUT")
                Dim tblSapFreigeben As DataTable = S.AP.GetImportTable("INPUT")

                Dim strucBlocken As DataRow

                For i = 0 To vwTemp.Count - 1
                    strucBlocken = tblSapFreigeben.NewRow
                    strucBlocken("Regel_Id") = Right("0000000000" & vwTemp(i)("RegelID").ToString, 10)
                    strucBlocken("Posnr") = vwTemp(i)("Posnr").ToString
                    strucBlocken("Equnr") = vwTemp(i)("EQUNR").ToString

                    tblSapFreigeben.Rows.Add(strucBlocken)
                Next


                'myProxy.callBapi()
                S.AP.Execute()

            End If

            WriteLogEntry(True, strBapiName, m_tblResult, False)

        Catch ex As Exception
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            ErrMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "Fehler beim Anlegen, BAPI: " & strBapiName, m_tblResult, False)
        Finally

        End Try

    End Sub

    Public Sub GiveZurFreigabe(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strClassAndMethod = "Sixt_B15.GiveZurFreigabe"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)
            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_ANZEIGEN_FREI", m_objApp, m_objUser, page)

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_BLOCKEN_ANZEIGEN_FREI")

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("OUTPUT")
                Dim tblTemp2 As DataTable = S.AP.GetExportTable("OUTPUT")

                Master = tblTemp2

                Master.Columns.Add("LoeFrei", System.Type.GetType("System.String"))

                WriteLogEntry(True, "Geblockte Fahrzeuge zur Freigabe ermitteln.", m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -5555
                m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "Fehler beim Ermitteln der Freigaben, BAPI: Z_M_Blocken_Anzeigen_Frei", m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try

        End If

    End Sub

    Public Sub GiveData(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Dim strKunnr As String
        Dim strBapiName As String = ""
        Dim tblSapVorschlaege As DataTable
        Dim strucBlocken As DataTable
        Dim tblSap As DataTable

        strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim intID As Int32 = -1
        m_intStatus = 0

        Try
            Select Case Aktion
                Case Blocken.Freigeben
                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_ANZEIGEN_FREI", m_objApp, m_objUser, page)
                    S.AP.Init("Z_M_BLOCKEN_ANZEIGEN_FREI")

                    'strucBlocken = myProxy.getImportTable("INPUT")
                    strucBlocken = S.AP.GetImportTable("INPUT")

                    'myProxy.callBapi()

                    'tblSap = myProxy.getExportTable("OUTPUT")

                    tblSap = S.AP.GetExportTableWithExecute("OUTPUT")

                Case Blocken.Anzeigen
                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_ANZEIGEN", m_objApp, m_objUser, page)
                    S.AP.Init("Z_M_BLOCKEN_ANZEIGEN")

                    'strucBlocken = myProxy.getImportTable("INPUT")
                    strucBlocken = S.AP.GetImportTable("INPUT")

                    'myProxy.callBapi()

                    'tblSap = myProxy.getExportTable("OUTPUT")

                    tblSap = S.AP.GetExportTableWithExecute("OUTPUT")

                Case Blocken.Vorschlaege
                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_ANZEIGEN_VOR", m_objApp, m_objUser, page)

                    'myProxy.callBapi()

                    S.AP.InitExecute("Z_M_BLOCKEN_ANZEIGEN_VOR")

                    'tblSapVorschlaege = myProxy.getExportTable("OUTPUT")
                    tblSapVorschlaege = S.AP.GetExportTable("OUTPUT")

            End Select

            WriteLogEntry(True, "Vorschlagsliste ermitteln.", m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "Fehler beim Ermitteln der Fahrzeuge, BAPI: Z_M_Blocken_Anzeigen_...", m_tblResult, False)
            Exit Sub
        Finally

        End Try


        Dim tblTemp As DataTable

        If Me.Aktion = Blocken.Vorschlaege Then
            tblTemp = tblSapVorschlaege.Copy
        Else
            tblTemp = tblSap.Copy
        End If


        Dim drTemp As DataRow
        Dim drRegel As DataRow
        Dim drModell As DataRow
        Dim drFahrzeug As DataRow
        Dim RegelID As String = ""
        Dim RegelIDModell As String = ""
        Dim strModell As String = ""


        drTemp = tblTemp.NewRow



        Dim vwTemp As DataView = tblTemp.DefaultView



        For Each drTemp In tblTemp.Rows



            'Tabelle m_GroupRegel füllen
            If RegelID <> drTemp.Item("Regel_ID").ToString Then

                drRegel = m_GroupRegel.NewRow


                RegelID = drTemp.Item("Regel_ID").ToString


                drRegel.Item("RegelID") = RegelID
                drRegel.Item("Regelname") = drTemp.Item("Regelname").ToString
                If IsDate(drTemp.Item("Erdat").ToString) Then
                    drRegel.Item("Erstellungsdatum") = CDate(drTemp.Item("Erdat").ToString).ToShortDateString
                End If

                drRegel.Item("Regelersteller") = drTemp.Item("Userid").ToString
                drRegel.Item("Regelgruppe") = drTemp.Item("KENNNZ_LI").ToString


                GiveRegel(strAppID, strSessionID, RegelID, page, True)

                drRegel.Item("Anzahl") = m_DataCount

                drRegel.Item("Details") = False
                drRegel.Item("Loaded") = False

                m_GroupRegel.Rows.Add(drRegel)

            End If

            'Tabelle m_tblModelle füllen
            If RegelIDModell <> drTemp.Item("Regel_ID").ToString OrElse strModell <> (drTemp.Item("ZZBEZEI").ToString & drTemp.Item("ZZHANDELSNAME").ToString) Then



                RegelIDModell = drTemp.Item("Regel_ID").ToString

                strModell = drTemp.Item("ZZBEZEI").ToString & drTemp.Item("ZZHANDELSNAME").ToString

                drModell = m_tblModelle.NewRow
                drModell.Item("RegelID") = RegelID
                drModell.Item("PDI_Name") = drTemp.Item("ZZBEZEI").ToString
                drModell.Item("BezeichnungltBrief") = drTemp.Item("ZZHANDELSNAME").ToString
                drModell.Item("Modell_ID") = RegelID & drTemp.Item("ZZBEZEI").ToString & drTemp.Item("ZZHANDELSNAME").ToString


                vwTemp.RowFilter = "Regel_ID = '" & RegelID & "' AND ZZBEZEI = '" & drTemp.Item("ZZBEZEI").ToString & "' AND ZZHANDELSNAME = '" & _
                                    drTemp.Item("ZZHANDELSNAME").ToString & "'"


                drModell.Item("SummeVorschlaege") = vwTemp.Count

                m_tblModelle.Rows.Add(drModell)

            End If


            'Tabelle m_tblFahrzeuge füllen

            drFahrzeug = m_tblFahrzeuge.NewRow

            drFahrzeug.Item("RegelID") = RegelID
            drFahrzeug.Item("Modell_ID") = RegelID & drTemp.Item("ZZBEZEI").ToString & drTemp.Item("ZZHANDELSNAME").ToString
            drFahrzeug.Item("PDI") = drTemp.Item("KUNPDI").ToString
            drFahrzeug.Item("Referenz") = drTemp.Item("LIZNR").ToString
            drFahrzeug.Item("FIN") = drTemp.Item("CHASSIS_NUM").ToString
            drFahrzeug.Item("Kraftstoff") = drTemp.Item("ZZKRAFTSTOFF_TXT").ToString
            drFahrzeug.Item("Hersteller") = drTemp.Item("ZZHERST_TEXT").ToString
            drFahrzeug.Item("LeistungKW") = drTemp.Item("ZZNENNLEISTUNG").ToString
            drFahrzeug.Item("Navi") = drTemp.Item("ZZNAVI").ToString
            drFahrzeug.Item("Bereifung") = drTemp.Item("ZZREIFEN").ToString
            drFahrzeug.Item("Farbe") = drTemp.Item("ZFARBE").ToString
            drFahrzeug.Item("Ausfuehrung") = drTemp.Item("ZZAUSF").ToString
            drFahrzeug.Item("ausgewaehlt") = False
            drFahrzeug.Item("Posnr") = drTemp.Item("POSNR").ToString
            drFahrzeug.Item("EQUNR") = drTemp.Item("EQUNR").ToString

            m_tblFahrzeuge.Rows.Add(drFahrzeug)
        Next



    End Sub

    Public Sub GiveZumAnzeigen(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Dim strKunnr As String
        Dim strBapiName As String = ""
        Dim strucBlocken As DataTable

        strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim intID As Int32 = -1

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_ANZEIGEN_FREI", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_BLOCKEN_ANZEIGEN_FREI")

            'strucBlocken = myProxy.getImportTable("INPUT")
            strucBlocken = S.AP.GetImportTable("INPUT")

            'myProxy.callBapi()

            'Master = myProxy.getExportTable("OUTPUT")

            Master = S.AP.GetExportTableWithExecute("OUTPUT")

            Master.Columns.Add("LoeFrei", System.Type.GetType("System.String"))

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            WriteLogEntry(True, "Geblockte Fahrzeuge ermitteln.", m_tblResult, False)
        Catch ex As Exception
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "Fehler beim Ermitteln geblockten Fahrzeuge, BAPI: Z_M_Blocken_Anzeigen", m_tblResult, False)
        Finally


        End Try

    End Sub

    Public Sub GiveZumLoeschen(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, ByRef strRegelID As String)

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_DataCount = 0

        Dim tblSap As DataTable
        Dim strucBlocken As DataTable

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_ANZEIGEN_LOESCHEN", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_BLOCKEN_ANZEIGEN_LOESCHEN")

            'strucBlocken = myProxy.getImportTable("INPUT")
            strucBlocken = S.AP.GetImportTable("INPUT")

            If strRegelID <> String.Empty Then
                Dim dRow As DataRow
                dRow = strucBlocken.NewRow

                dRow("Regel_Id") = strRegelID
                strucBlocken.Rows.Add(drow)
            End If

            'myProxy.callBapi()

            'tblSap = myProxy.getExportTable("OUTPUT")

            tblSap = S.AP.GetExportTableWithExecute("OUTPUT")

            WriteLogEntry(True, "Geblockte Fahrzeuge zum Löschen ermitteln.", m_tblResult, False)
        Catch ex As Exception
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            WriteLogEntry(False, "Fehler beim Ermitteln der Löschvorgänge, BAPI: Z_M_Blocken_Anzeigen_Loeschen", m_tblResult, False)
        End Try

        If Not (tblSap) Is Nothing Then

            Dim tblTemp As DataTable

            tblTemp = tblSap.Copy

            m_DataCount = tblTemp.Rows.Count

            If strRegelID <> String.Empty Then Exit Sub


            tblTemp.Columns.Add("Anzahl", System.Type.GetType("System.Int32"))
            tblTemp.Columns.Add("AnzahlLoeschen", System.Type.GetType("System.String"))
            tblTemp.Columns.Add("Status", System.Type.GetType("System.String"))


            Dim vwTemp As DataView = tblTemp.DefaultView


            Detail = tblTemp
            Master = tblTemp.Copy

            Master.Clear()

            Dim dr As DataRow
            Dim drMaster As DataRow
            Dim RegelID As String = ""

            dr = tblTemp.NewRow

            For Each dr In tblTemp.Rows

                If RegelID <> dr.Item("Regel_ID").ToString Then

                    drMaster = Master.NewRow

                    drMaster.Item("Regel_ID") = dr.Item("Regel_ID")
                    drMaster.Item("Regelname") = dr.Item("Regelname")
                    If IsDate(dr.Item("Erdat").ToString) Then
                        drMaster.Item("Erdat") = CDate(dr.Item("Erdat").ToString).ToShortDateString
                    End If

                    drMaster.Item("UserID") = dr.Item("UserID")
                    drMaster.Item("ZZHERST_TEXT") = dr.Item("ZZHERST_TEXT")



                    RegelID = dr.Item("Regel_ID").ToString

                    vwTemp.RowFilter = "Regel_ID = '" & RegelID & "'"

                    drMaster.Item("Anzahl") = vwTemp.Count

                    Master.Rows.Add(drMaster)


                End If

            Next
        End If
    End Sub

    Public Sub GiveRegel(ByVal strAppID As String, ByVal strSessionID As String, ByRef strRegelID As String, ByVal page As Web.UI.Page, _
                         Optional ByRef booAnzahl As Boolean = False)

        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Dim strucBlocken As DataTable

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_ANZEIGEN_AUSWAHL", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_REGEL_ID", strRegelID)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_BLOCKEN_ANZEIGEN_AUSWAHL", "I_REGEL_ID", strRegelID)

            'strucBlocken = myProxy.getExportTable("OUT")
            'Anzahl = myProxy.getExportParameter("O_ANZAHL")

            strucBlocken = S.AP.GetExportTable("OUT")
            Anzahl = S.AP.GetExportParameter("O_ANZAHL")

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            WriteLogEntry(True, "Regel ermitteln.", m_tblResult, False)
        Catch ex As Exception
            m_strMessage = ex.Message

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "Fehler beim Ermitteln der Regel, BAPI: Z_M_Blocken_Anzeigen_Auswahl", m_tblResult, False)
        Finally

        End Try

        If booAnzahl = True Then
            m_DataCount = CInt(Anzahl)
            Exit Sub
        End If

        Anzahl = m_DataCount.ToString
        If Not (strucBlocken) Is Nothing Then
            If strucBlocken.Rows.Count > 0 Then
                Me.Regelname = strucBlocken.Rows(0)("Regelname").ToString
                Me.Mail = strucBlocken.Rows(0)("Mail").ToString
                Me.FIN = strucBlocken.Rows(0)("Chassis_Num").ToString
                Me.KstLN = strucBlocken.Rows(0)("Costl").ToString
                Me.LizLim = strucBlocken.Rows(0)("Kennnz_Li").ToString
                Me.PDI = strucBlocken.Rows(0)("Kunpdi").ToString
                Me.Referenz = strucBlocken.Rows(0)("Liznr").ToString
                Me.KstName = strucBlocken.Rows(0)("Name").ToString
                Me.GeplStation = strucBlocken.Rows(0)("Station").ToString
                Me.Versicherung = strucBlocken.Rows(0)("Versicherer").ToString
                Me.UserName = strucBlocken.Rows(0)("Userid").ToString
                Me.FirstName = strucBlocken.Rows(0)("Vorname").ToString
                Me.LastName = strucBlocken.Rows(0)("Name1").ToString
                Me.Farbe = strucBlocken.Rows(0)("Zfarbe").ToString
                Me.AusfuehrungValue = strucBlocken.Rows(0)("Zzausf").ToString

                Me.Modell = strucBlocken.Rows(0)("Zzbezei").ToString
                Me.Handelsname = strucBlocken.Rows(0)("Zzhandelsname").ToString
                Me.HerstellerValue = strucBlocken.Rows(0)("Zzherst_Text").ToString

                Me.AntriebValue = strucBlocken.Rows(0)("Zzkraftstoff_Txt").ToString


                Me.Versicherung = strucBlocken.Rows(0)("Versicherer").ToString

                Me.ZulSixt = strucBlocken.Rows(0)("Name").ToString

                Me.NaviValue = strucBlocken.Rows(0)("Zznavi").ToString


                Me.LeistKW = strucBlocken.Rows(0)("Zznennleistung").ToString

                Me.ReifenValue = strucBlocken.Rows(0)("Zzreifen").ToString
            End If
        End If

    End Sub

    Public Sub GeblockteLoeschen(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Dim strucBlocken As DataTable
        Dim row As DataRow
        Dim rowDetail As DataRow
        Dim rows() As DataRow
        Dim strExp As String
        Dim e As Int32

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_LOESCHEN", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_BLOCKEN_LOESCHEN")

            'strucBlocken = myProxy.getImportTable("INPUT")
            strucBlocken = S.AP.GetImportTable("INPUT")

            For Each row In Master.Rows

                If row.Item("AnzahlLoeschen").ToString <> String.Empty Then

                    e = CInt(row.Item("AnzahlLoeschen"))

                    strExp = "Regel_ID = '" & row.Item("Regel_ID").ToString & "'"

                    rows = Detail.Select(strExp)

                    For Each rowDetail In rows
                        Dim NewRow As DataRow
                        NewRow = strucBlocken.NewRow
                        With strucBlocken

                            NewRow("REGEL_ID") = rowDetail("REGEL_ID").ToString
                            NewRow("POSNR") = rowDetail("POSNR").ToString
                        End With

                        strucBlocken.Rows.Add(NewRow)

                        e = e - 1

                        If e = 0 Then Exit For

                    Next

                End If
            Next

            'myProxy.callBapi()
            S.AP.Execute()

            WriteLogEntry(True, "Geblockte Fahrzeuge löschen.", m_tblResult, False)
        Catch ex As Exception
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "Fehler beim Löschen, BAPI: Z_M_BLOCKEN_LOESCHEN", m_tblResult, True)
        Finally
        End Try

    End Sub

    Public Sub GeblockteFreigeben(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID


        Dim strucBlocken As DataTable


        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_FREIGEBEN", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_BLOCKEN_FREIGEBEN")

            'strucBlocken = myProxy.getImportTable("INPUT")
            strucBlocken = S.AP.GetImportTable("INPUT")

            Dim row As DataRow
            Dim rows() As DataRow

            rows = Master.Select("LoeFrei = 'x'")

            For Each row In rows
                Dim NewRow As DataRow
                NewRow = strucBlocken.NewRow
                With strucBlocken

                    NewRow("REGEL_ID") = row("REGEL_ID").ToString
                    NewRow("POSNR") = row("POSNR").ToString
                End With

                strucBlocken.Rows.Add(NewRow)

            Next

            'myProxy.callBapi()
            S.AP.Execute()

            WriteLogEntry(True, "Geblockte Fahrzeuge freigeben.", m_tblResult, False)
        Catch ex As Exception
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "Fehler beim Freigeben, BAPI: Z_M_BLOCKEN_FREIGEBEN", m_tblResult, True)
        Finally
        End Try

    End Sub

    Public Sub GiveAusfuehrung(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID


        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_AUSFUEHRUNG", m_objApp, m_objUser, page)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_BLOCKEN_AUSFUEHRUNG")

            'Ausfuehrung = myProxy.getExportTable("ZZAUSF")
            Ausfuehrung = S.AP.GetExportTable("ZZAUSF")

            WriteLogEntry(True, "Ausführungsdaten ermittelt.", Ausfuehrung, False)
        Catch ex As Exception
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            WriteLogEntry(False, "Fehler beim Ermitteln der Ausführung, BAPI: Z_M_Blocken_Ausfuehrung", m_tblResult, False)
        Finally

        End Try

    End Sub

    Public Sub GiveAntrieb(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_ANTRIEB", m_objApp, m_objUser, page)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_BLOCKEN_ANTRIEB")

            'Antrieb = myProxy.getExportTable("ANTRIEB")
            Antrieb = S.AP.GetExportTable("ANTRIEB")

            WriteLogEntry(True, "Antriebsarten ermittelt.", Antrieb, False)
        Catch ex As Exception
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "Fehler beim Ermitteln der Ausführung, BAPI: Z_M_BLOCKEN_ANTRIEB", m_tblResult, False)
        Finally

        End Try


    End Sub

    Public Sub GiveFarben(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_FARBEN", m_objApp, m_objUser, Page)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_BLOCKEN_FARBEN")

            'Farben = myProxy.getExportTable("FARBE")
            Farben = S.AP.GetExportTable("FARBE")

            WriteLogEntry(True, "FARBEN ermittelt.", Antrieb, False)
        Catch ex As Exception
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "Fehler beim Ermitteln der Farben, BAPI: Z_M_BLOCKEN_FARBEN", m_tblResult, False)
        Finally

        End Try


    End Sub

    Public Sub GiveHersteller(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, ByVal KunNr As String)

        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_HERSTELLER", m_objApp, m_objUser, page)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_BLOCKEN_HERSTELLER")

            'Hersteller = myProxy.getExportTable("HERSTELLER")
            Hersteller = S.AP.GetExportTable("HERSTELLER")

            WriteLogEntry(True, "HERSTELLER ermittelt.", Antrieb, False)
        Catch ex As Exception
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "Fehler beim Ermitteln der Farben, BAPI: Z_M_BLOCKEN_HERSTELLER", m_tblResult, False)
        Finally

        End Try

    End Sub

    Public Sub GiveNavi(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_NAVI", m_objApp, m_objUser, page)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_BLOCKEN_NAVI")

            'Navi = myProxy.getExportTable("ZZNAVI")
            Navi = S.AP.GetExportTable("ZZNAVI")

            WriteLogEntry(True, "Navi ermittelt.", Antrieb, False)
        Catch ex As Exception
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "Fehler beim Ermitteln der Navi, BAPI: Z_M_BLOCKEN_NAVI", m_tblResult, False)
        Finally

        End Try
    End Sub

    Public Sub GiveReifen(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_REIFEN", m_objApp, m_objUser, page)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_BLOCKEN_REIFEN")

            'Reifen = myProxy.getExportTable("ZZREIFEN")
            Reifen = S.AP.GetExportTable("ZZREIFEN")

            WriteLogEntry(True, "Reifen ermittelt.", Antrieb, False)
        Catch ex As Exception
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "Fehler beim Ermitteln der Reifen, BAPI: Z_M_BLOCKEN_REIFEN", m_tblResult, False)
        Finally

        End Try

    End Sub

    Public Sub GiveVersicherer(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_VERSICHERER", m_objApp, m_objUser, page)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_BLOCKEN_VERSICHERER")

            'Versicherer = myProxy.getExportTable("VERSICHERER")
            Versicherer = S.AP.GetExportTable("VERSICHERER")

            WriteLogEntry(True, "Versicherer ermittelt.", Antrieb, False)
        Catch ex As Exception
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "Fehler beim Ermitteln der Versicherer, BAPI: Z_M_BLOCKEN_VERSICHERER", m_tblResult, False)
        Finally

        End Try


    End Sub

    Public Sub GiveHalter(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BLOCKEN_HALTER", m_objApp, m_objUser, page)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_BLOCKEN_HALTER")

            'Halter = myProxy.getExportTable("HALTER")
            Halter = S.AP.GetExportTable("HALTER")

            WriteLogEntry(True, "HALTER ermittelt.", Antrieb, False)
        Catch ex As Exception
            m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "Fehler beim Ermitteln der HALTER, BAPI: Z_M_BLOCKEN_HALTER", m_tblResult, False)
        Finally

        End Try


    End Sub

    Public Function SelectCars() As Int32
        Try
            m_dsBlocken_Data_Selected = m_dsBlocken_Data.Copy
            Dim i As Int32
            Dim vwTemp As DataView = m_dsBlocken_Data_Selected.Tables("Fahrzeuge").DefaultView
            vwTemp.RowFilter = "Ausgewaehlt = True"
            Dim intReturn As Int32 = vwTemp.Count
            vwTemp.RowFilter = ""

            If intReturn > 0 Then
                For i = m_dsBlocken_Data_Selected.Tables("Fahrzeuge").Rows.Count - 1 To 0 Step -1
                    If Not CType(m_dsBlocken_Data_Selected.Tables("Fahrzeuge").Rows(i)("Ausgewaehlt"), Boolean) Then
                        m_dsBlocken_Data_Selected.Tables("Fahrzeuge").AcceptChanges()
                        m_dsBlocken_Data_Selected.Tables("Fahrzeuge").Rows(i).Delete()
                        m_dsBlocken_Data_Selected.Tables("Fahrzeuge").AcceptChanges()
                    End If
                Next
                For i = m_dsBlocken_Data_Selected.Tables("Modelle").Rows.Count - 1 To 0 Step -1
                    vwTemp = m_dsBlocken_Data_Selected.Tables("Fahrzeuge").DefaultView
                    vwTemp.RowFilter = "Modell_ID = '" & m_dsBlocken_Data_Selected.Tables("Modelle").Rows(i)("Modell_ID").ToString & "'"
                    If vwTemp.Count = 0 Then
                        m_dsBlocken_Data_Selected.Tables("Modelle").AcceptChanges()
                        m_dsBlocken_Data_Selected.Tables("Modelle").Rows(i).Delete()
                        m_dsBlocken_Data_Selected.Tables("Modelle").AcceptChanges()
                    Else
                        'm_dsBlocken_Data_Selected.Tables("Modelle").Rows(i)("Anzahl_neu") = vwTemp.Count
                    End If
                    vwTemp.RowFilter = ""
                Next
                For i = m_dsBlocken_Data_Selected.Tables("Regeln").Rows.Count - 1 To 0 Step -1
                    vwTemp = m_dsBlocken_Data_Selected.Tables("Modelle").DefaultView
                    vwTemp.RowFilter = "RegelID = '" & m_dsBlocken_Data_Selected.Tables("Regeln").Rows(i)("RegelID").ToString & "'"
                    If vwTemp.Count = 0 Then
                        m_dsBlocken_Data_Selected.Tables("Regeln").AcceptChanges()
                        m_dsBlocken_Data_Selected.Tables("Regeln").Rows(i).Delete()
                        m_dsBlocken_Data_Selected.Tables("Regeln").AcceptChanges()
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

    Public Function CheckAnzahlFahrzeuge() As Boolean
        Try
            m_dsBlocken_Data_Selected = m_dsBlocken_Data.Copy
            Dim i As Int32
            Dim vwTemp As DataView = m_dsBlocken_Data_Selected.Tables("Fahrzeuge").DefaultView
            Dim rows() As DataRow
            Dim strTablename As String = "Regeln"


            CountErr = False

            vwTemp.RowFilter = "Ausgewaehlt = True"
            Dim intReturn As Int32 = vwTemp.Count
            vwTemp.RowFilter = ""

            For i = m_dsBlocken_Data_Selected.Tables("Modelle").Rows.Count - 1 To 0 Step -1
                vwTemp = m_dsBlocken_Data_Selected.Tables("Fahrzeuge").DefaultView
                vwTemp.RowFilter = "RegelID = '" & m_dsBlocken_Data_Selected.Tables("Modelle").Rows(i)("RegelID").ToString & "' AND Ausgewaehlt = 'True'"


                rows = m_dsBlocken_Data_Selected.Tables("Regeln").Select("RegelID = '" & m_dsBlocken_Data_Selected.Tables("Modelle").Rows(i)("RegelID").ToString & "'")


                If vwTemp.Count > CInt(rows(0)("Anzahl")) Then

                    CountErr = True

                End If
                vwTemp.RowFilter = ""
            Next


        Catch ex As Exception

        End Try
        Return CountErr
    End Function


#End Region
End Class

' ************************************************
' $History: Sixt_B15.vb $
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
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 26.06.07   Time: 11:18
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' *****************  Version 3  *****************
' User: Uha          Date: 21.05.07   Time: 15:28
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Änderungen im Vergleich zur Startapplikation zum Stand 21.05.2007
' 
' *****************  Version 2  *****************
' User: Uha          Date: 15.05.07   Time: 17:40
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Änderungen aus StartApplication vom 11.05.2007
' 
' ************************************************
