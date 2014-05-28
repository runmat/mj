Option Explicit On
Option Infer On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel.Common

Namespace Zulassung
    Public Class Zulassung1
        Inherits Base.Business.DatenimportBase

#Region "Declarations"

        Private mE_SUBRC As String = ""
        Private mE_MESSAGE As String = ""
        Private m_strFahrgestellnummer As String = ""
        Private m_strNummerZBII As String = ""
        Private m_strVertragsnummer As String = ""
        Private m_tblFahrzeuge As DataTable
        Private m_tblFINs As DataTable
        Private m_tblAdressen As DataTable
        '***Halter***
        Private mHalterName As String
        Private mHalterName2 As String
        Private mHalterStrasse As String
        Private mHalterPLZ As String
        Private mHalterOrt As String
        Private mHalterLand As String

        '***Versicherung***
        Private mVersGesellschaft As String
        Private mEVBNummer As String
        Private mEVBVon As String
        Private mEVBBis As String
        Private mVersNehmer As String
        Private mVersNehmerArt As String
        Private mVersNehmerStrasse As String
        Private mVersNehmerPLZ As String
        Private mVersNehmerOrt As String
        Private mVersNehmerLand As String

        '***Zulassung***
        Private mEquiNr As String
        Private mZulassungsdatum As String
        Private mWunschkennzeichen1 As String
        Private mWunschkennzeichen2 As String
        Private mWunschkennzeichen3 As String
        Private mZulassungskreis As String
        Private mResNummer As String
        Private mResName As String
        Private mFeinstaub As String
        Private mTblZulassungsart As DataTable
        Private mZulassungsart As String = "3"
        Private mZulassungsartText As String

        '***VersandSchein und Schilder***
        Private mVssName As String
        Private mVssName2 As String
        Private mVssStrasse As String
        Private mVssPLZ As String
        Private mVssOrt As String
        Private mVssLAND As String
        'Länder
        Private m_tblLaender As DataTable

        'Zulassungskreise
        Public m_tblKreis As DataTable
        Private m_ZulCount As Integer

        Private m_AuftragNr As String

#End Region

#Region " Properties"

        Public Property Vertragsnummer() As String
            Get
                Return m_strVertragsnummer
            End Get
            Set(ByVal Value As String)
                m_strVertragsnummer = Value
            End Set
        End Property

        Public Property Fahrgestellnr() As String
            Get
                Return m_strFahrgestellnummer
            End Get
            Set(ByVal Value As String)
                m_strFahrgestellnummer = Value
            End Set
        End Property

        Public Property NummerZBII() As String
            Get
                Return m_strNummerZBII
            End Get
            Set(ByVal Value As String)
                m_strNummerZBII = Value
            End Set
        End Property

        Public Property Zulassungsarttext() As String
            Get
                Return mZulassungsartText
            End Get
            Set(ByVal Value As String)
                mZulassungsartText = Value
            End Set
        End Property

        Public Property Fahrzeuge() As DataTable
            Get
                Return m_tblFahrzeuge
            End Get
            Set(ByVal Value As DataTable)
                m_tblFahrzeuge = Value
            End Set
        End Property
        Public Property FINs() As DataTable
            Get
                Return m_tblFINs
            End Get
            Set(ByVal Value As DataTable)
                m_tblFINs = Value
            End Set
        End Property
        Public Property Laender() As DataTable
            Get
                Return m_tblLaender
            End Get
            Set(ByVal Value As DataTable)
                m_tblLaender = Value
            End Set
        End Property
        Public Property Kreis() As DataTable
            Get
                Return m_tblKreis
            End Get
            Set(ByVal Value As DataTable)
                m_tblKreis = Value
            End Set
        End Property
        Public Property Adressen() As DataTable
            Get
                Return m_tblAdressen
            End Get
            Set(ByVal Value As DataTable)
                m_tblAdressen = Value
            End Set
        End Property
        '***Halter***
        Public Property Halter() As String
            Get
                Return mHalterName
            End Get
            Set(ByVal Value As String)
                mHalterName = Value
            End Set
        End Property
        Public Property Halter2() As String
            Get
                Return mHalterName2
            End Get
            Set(ByVal Value As String)
                mHalterName2 = Value
            End Set
        End Property
        Public Property HalterStrasse() As String
            Get
                Return mHalterStrasse
            End Get
            Set(ByVal Value As String)
                mHalterStrasse = Value
            End Set
        End Property

        Public Property HalterPLZ() As String
            Get
                Return mHalterPLZ
            End Get
            Set(ByVal Value As String)
                mHalterPLZ = Value
            End Set
        End Property

        Public Property HalterOrt() As String
            Get
                Return mHalterOrt
            End Get
            Set(ByVal Value As String)
                mHalterOrt = Value
            End Set
        End Property

        Public Property HalterLand() As String
            Get
                Return mHalterLand
            End Get
            Set(ByVal Value As String)
                mHalterLand = Value
            End Set
        End Property

        '***Versicherung***
        Public Property VersGesellschaft() As String
            Get
                Return mVersGesellschaft
            End Get
            Set(ByVal Value As String)
                mVersGesellschaft = Value
            End Set
        End Property
        Public Property EVBNummer() As String
            Get
                Return mEVBNummer
            End Get
            Set(ByVal Value As String)
                mEVBNummer = Value
            End Set
        End Property

        Public Property EVBVon() As String
            Get
                Return mEVBVon
            End Get
            Set(ByVal Value As String)
                mEVBVon = Value
            End Set
        End Property
        Public Property EVBBis() As String
            Get
                Return mEVBBis
            End Get
            Set(ByVal Value As String)
                mEVBBis = Value
            End Set
        End Property

        Public Property VersNehmerArt() As String
            Get
                Return mVersNehmerArt
            End Get
            Set(ByVal Value As String)
                mVersNehmerArt = Value
            End Set
        End Property

        Public Property VersNehmer() As String
            Get
                Return mVersNehmer
            End Get
            Set(ByVal Value As String)
                mVersNehmer = Value
            End Set
        End Property

        Public Property VersNehmerStrasse() As String
            Get
                Return mVersNehmerStrasse
            End Get
            Set(ByVal Value As String)
                mVersNehmerStrasse = Value
            End Set
        End Property

        Public Property VersNehmerPLZ() As String
            Get
                Return mVersNehmerPLZ
            End Get
            Set(ByVal Value As String)
                mVersNehmerPLZ = Value
            End Set
        End Property

        Public Property VersNehmerOrt() As String
            Get
                Return mVersNehmerOrt
            End Get
            Set(ByVal Value As String)
                mVersNehmerOrt = Value
            End Set
        End Property

        Public Property VersNehmerLand() As String
            Get
                Return mVersNehmerLand
            End Get
            Set(ByVal Value As String)
                mVersNehmerLand = Value
            End Set
        End Property
        Public Property Zulassungsdatum() As String
            Get
                Return mZulassungsdatum
            End Get
            Set(ByVal Value As String)
                mZulassungsdatum = Value
            End Set
        End Property

        Public Property Wunschkennzeichen1() As String
            Get
                Return mWunschkennzeichen1
            End Get
            Set(ByVal Value As String)
                mWunschkennzeichen1 = Value
            End Set
        End Property

        Public Property Wunschkennzeichen2() As String
            Get
                Return mWunschkennzeichen2
            End Get
            Set(ByVal Value As String)
                mWunschkennzeichen2 = Value
            End Set
        End Property

        Public Property Wunschkennzeichen3() As String
            Get
                Return mWunschkennzeichen3
            End Get
            Set(ByVal Value As String)
                mWunschkennzeichen3 = Value
            End Set
        End Property

        Public Property Zulassungskreis() As String
            Get
                Return mZulassungskreis
            End Get
            Set(ByVal Value As String)
                mZulassungskreis = Value
            End Set
        End Property

        Public Property ResNummer() As String
            Get
                Return mResNummer
            End Get
            Set(ByVal Value As String)
                mResNummer = Value
            End Set
        End Property

        Public Property ResName() As String
            Get
                Return mResName
            End Get
            Set(ByVal Value As String)
                mResName = Value
            End Set
        End Property

        Public Property Feinstaub() As String
            Get
                Return mFeinstaub
            End Get
            Set(ByVal Value As String)
                mFeinstaub = Value
            End Set
        End Property
        Public Property ZulCount() As Integer
            Get
                Return m_ZulCount
            End Get
            Set(ByVal Value As Integer)
                m_ZulCount = Value
            End Set
        End Property

        Public Property EquiNr() As String
            Get
                Return mEquiNr
            End Get
            Set(ByVal Value As String)
                mEquiNr = Value
            End Set
        End Property

        Public Property AuftragsNr() As String
            Get
                Return m_AuftragNr
            End Get
            Set(ByVal value As String)
                m_AuftragNr = value
            End Set
        End Property

        '***Versand Schein und Schilder***
        Public Property VssName() As String
            Get
                Return mVssName
            End Get
            Set(ByVal Value As String)
                mVssName = Value
            End Set
        End Property
        Public Property VssName2() As String
            Get
                Return mVssName2
            End Get
            Set(ByVal Value As String)
                mVssName2 = Value
            End Set
        End Property
        Public Property VssStrasse() As String
            Get
                Return mVssStrasse
            End Get
            Set(ByVal Value As String)
                mVssStrasse = Value
            End Set
        End Property

        Public Property VssPLZ() As String
            Get
                Return mVssPLZ
            End Get
            Set(ByVal Value As String)
                mVssPLZ = Value
            End Set
        End Property

        Public Property VssOrt() As String
            Get
                Return mVssOrt
            End Get
            Set(ByVal Value As String)
                mVssOrt = Value
            End Set
        End Property
        Public Property VssLand() As String
            Get
                Return mVssLAND
            End Get
            Set(ByVal Value As String)
                mVssLAND = Value
            End Set
        End Property

        Public Property Zulassungsart() As String
            Get
                Return mZulassungsart
            End Get
            Set(ByVal Value As String)
                mZulassungsart = Value
            End Set
        End Property

        Public ReadOnly Property tblZulassungsart() As DataTable
            Get
                Return mTblZulassungsart
            End Get
        End Property

#End Region

        Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
            MyBase.New(objUser, objApp, strFilename)
        End Sub



        Public Overloads Sub Show(ByVal strAppID As String, _
                     ByVal strSessionID As String, _
                     ByVal page As Web.UI.Page)

            m_strClassAndMethod = "Zulassung.Show"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            m_intStatus = 0
            m_strMessage = ""
            If Not m_blnGestartet Then
                m_blnGestartet = True
                Dim intID As Int32 = -1
                Dim TempFin As DataTable
                Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

                Try

                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_EQUI_002", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                    If m_strNummerZBII.Length > 0 Then
                        myProxy.setImportParameter("I_TIDNR ", m_strNummerZBII)
                    End If

                    If Vertragsnummer.Length > 0 Then
                        myProxy.setImportParameter("I_LIZNR ", Vertragsnummer)
                    End If

                    TempFin = New DataTable

                    TempFin = myProxy.getImportTable("GT_FIN")
                    Dim FinRow As DataRow
                    If m_tblFINs Is Nothing Then
                        FinRow = TempFin.NewRow
                        FinRow("CHASSIS_NUM") = m_strFahrgestellnummer
                        TempFin.Rows.Add(FinRow)
                    ElseIf m_tblFINs.Rows.Count > 0 Then
                        For Each tempRow As DataRow In m_tblFINs.Rows
                            FinRow = TempFin.NewRow
                            FinRow("CHASSIS_NUM") = tempRow(0).ToString
                            TempFin.Rows.Add(FinRow)
                        Next
                    End If

                    myProxy.callBapi()

                    m_tblFahrzeuge = New DataTable

                    mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                    mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

                    m_tblFahrzeuge = myProxy.getExportTable("GT_OUT")
                    m_tblFahrzeuge.Columns.Add("AUSWAHL", GetType(System.String))
                    m_tblFahrzeuge.Columns.Add("ZulKreis", GetType(System.String))
                    m_tblFahrzeuge.Columns.Add("Wunschkennz1", GetType(System.String))
                    m_tblFahrzeuge.Columns.Add("Wunschkennz2", GetType(System.String))
                    m_tblFahrzeuge.Columns.Add("Wunschkennz3", GetType(System.String))
                    m_tblFahrzeuge.Columns.Add("ResNr", GetType(System.String))
                    m_tblFahrzeuge.Columns.Add("ResName", GetType(System.String))
                    m_tblFahrzeuge.Columns.Add("Auftragsnr", GetType(System.String))

                    For Each Row As DataRow In m_tblFahrzeuge.Rows
                        Row("Auswahl") = ""
                    Next

                    m_tblFahrzeuge.AcceptChanges()


                    If mE_SUBRC <> "0" Then
                        m_intStatus = CInt(mE_SUBRC)
                        m_strMessage = mE_MESSAGE
                    End If

                Catch ex As Exception
                    m_tblFahrzeuge = Nothing
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Overloads Sub Change(ByVal strAppID As String, _
                     ByVal strSessionID As String, _
                     ByVal page As Web.UI.Page)
            m_strClassAndMethod = "Zulassung.Change"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            m_intStatus = 0
            m_strMessage = ""


            If Not m_blnGestartet Then
                m_blnGestartet = True
                Dim intID As Int32 = -1
                Dim ImportZul As New DataTable
                Dim PartnerZul As New DataTable
                Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

                Try

                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ORDER_LN", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("AG", strKUNNR)
                    myProxy.setImportParameter("EQUNR", mEquiNr)
                    myProxy.setImportParameter("WEB_USER", m_objUser.UserName)
                    ' myProxy.setImportParameter("AUGRU", "ZFB")
                    myProxy.setImportParameter("VDATU", Zulassungsdatum)

                    ImportZul = myProxy.getImportTable("ZUL")
                    PartnerZul = myProxy.getImportTable("PARTNER_ZUL")

                    '### Zulassung ###
                    Dim ImportRow As DataRow
                    ImportRow = ImportZul.NewRow
                    Dim WunschkennzeichenData As String = String.Empty

                    If Wunschkennzeichen1 <> String.Empty Then
                        WunschkennzeichenData = Wunschkennzeichen1
                        If Wunschkennzeichen2 <> String.Empty Then
                            WunschkennzeichenData = WunschkennzeichenData & "," & Wunschkennzeichen2
                            If Wunschkennzeichen3 <> String.Empty Then
                                WunschkennzeichenData = WunschkennzeichenData & "," & Wunschkennzeichen3

                            End If
                        End If

                    End If
                    'Reservierungsname und -nummer in das Wunschkennzeichen eintragen
                    If ResNummer <> String.Empty Then
                        If WunschkennzeichenData <> String.Empty Then
                            WunschkennzeichenData = WunschkennzeichenData & ", " & ResNummer
                        Else
                            WunschkennzeichenData = ResNummer
                        End If
                    End If

                    If ResName <> String.Empty Then
                        If WunschkennzeichenData <> String.Empty Then
                            WunschkennzeichenData = WunschkennzeichenData & ", " & ResName
                        Else
                            WunschkennzeichenData = ResName
                        End If
                    End If

                    If Zulassungsarttext <> String.Empty Then
                        If WunschkennzeichenData <> String.Empty Then
                            WunschkennzeichenData = Zulassungsarttext & ", " & WunschkennzeichenData
                        Else
                            WunschkennzeichenData = Zulassungsarttext
                        End If
                    End If

                    ImportRow("ZZFAHRG") = Fahrgestellnr
                    ImportRow("ZZBRIEF") = NummerZBII
                    ImportRow("ZULDAT") = Zulassungsdatum
                    ImportRow("EVBNR") = EVBNummer
                    If IsDate(EVBVon) Then
                        ImportRow("EVBVONDAT") = EVBVon
                    End If
                    If IsDate(EVBBis) Then
                        ImportRow("EVBBISDAT") = EVBBis
                    End If
                    ImportRow("WUNSCHKENNZ") = WunschkennzeichenData
                    ImportRow("VERSICHERUNG") = VersGesellschaft
                    ImportRow("TERMINHINWEIS") = ""
                    ImportRow("FEINSTAUBPL") = Feinstaub
                    ImportRow("STEUERN") = ""
                    ImportRow("EXKUNNR_ZL") = ""
                    ImportRow("KVGR3") = Zulassungsart

                    ImportZul.Rows.Add(ImportRow)


                    '### Partner Zulassung ###
                    Dim PartnerZulRow As DataRow = PartnerZul.NewRow

                    'Rechnungsemfänger
                    PartnerZulRow = PartnerZul.NewRow

                    PartnerZulRow("PARTN_ROLE") = "RE"
                    PartnerZulRow("PARTN_NUMB") = strKUNNR

                    PartnerZul.Rows.Add(PartnerZulRow)

                    'Regulierer
                    PartnerZulRow = PartnerZul.NewRow

                    PartnerZulRow("PARTN_ROLE") = "RG"
                    PartnerZulRow("PARTN_NUMB") = strKUNNR

                    PartnerZul.Rows.Add(PartnerZulRow)

                    PartnerZulRow = PartnerZul.NewRow

                    'Halter
                    PartnerZulRow("PARTN_ROLE") = "ZH"
                    PartnerZulRow("NAME") = Halter
                    PartnerZulRow("STREET") = HalterStrasse
                    PartnerZulRow("POSTL_CODE") = HalterPLZ
                    PartnerZulRow("CITY") = HalterOrt
                    PartnerZulRow("COUNTRY") = HalterLand

                    PartnerZul.Rows.Add(PartnerZulRow)

                    PartnerZulRow = PartnerZul.NewRow
                    ' Versicherungsnehmer
                    PartnerZulRow("PARTN_ROLE") = "ZC"
                    PartnerZulRow("NAME") = VersNehmer
                    PartnerZulRow("STREET") = VersNehmerStrasse
                    PartnerZulRow("POSTL_CODE") = VersNehmerPLZ
                    PartnerZulRow("CITY") = VersNehmerOrt
                    PartnerZulRow("COUNTRY") = VersNehmerLand
                    PartnerZulRow("FLGDEFAULT") = "X"

                    PartnerZul.Rows.Add(PartnerZulRow)

                    'Versand Schein und Schilder
                    PartnerZulRow = PartnerZul.NewRow
                    PartnerZulRow("PARTN_ROLE") = "ZE"
                    PartnerZulRow("NAME") = VssName
                    PartnerZulRow("NAME_2") = VssName2
                    PartnerZulRow("STREET") = VssStrasse
                    PartnerZulRow("POSTL_CODE") = VssPLZ
                    PartnerZulRow("CITY") = VssOrt
                    PartnerZulRow("COUNTRY") = "DE"

                    PartnerZul.Rows.Add(PartnerZulRow)


                    myProxy.callBapi()

                    m_AuftragNr = String.Empty
                    m_AuftragNr = myProxy.getExportParameter("VBELN_ZUL")

                    WriteLogEntry(True, "I_AG=" & m_objUser.KUNNR, m_tblResult)

                Catch ex As Exception
                    m_intStatus = -5555

                    Select Case Base.Business.HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case "NO_VKORG"
                            m_strMessage = "VKORG konnte nicht ermittelt werden."
                        Case "STATUS_ERROR"
                            m_strMessage = "Fehler beim Statusändern des ÜberfOrders."
                        Case "NO_MAT"
                            m_strMessage = "Kein Material für AG gefunden."
                        Case "ZH_NOT_FOUND"
                            m_strMessage = "Halter nicht gefunden."
                        Case "ZH_UNVOLLSTAENDIG"
                            m_strMessage = "Halteradresse ist unvollständig."
                        Case "ZV_NOT_FOUND"
                            m_strMessage = "Versicherer nicht gefunden."
                        Case "ZE_UNVOLLSTAENDIG"
                            m_strMessage = "Empfängeradresse Schein ist unvollständig."
                        Case "ZC_NOT_FOUND"
                            m_strMessage = "abw.Versicherungsnehmer nicht gefunden."
                        Case "ZC_UNVOLLSTAENDIG"
                            m_strMessage = "abw.Versicherungsnehmer ist unvollständig."
                        Case "RE_NOT_FOUND"
                            m_strMessage = "Rechnungsempfänger nicht gefunden."
                        Case "RG_NOT_FOUND"
                            m_strMessage = "Regulierer nicht gefunden."
                        Case "NO_ZH"
                            m_strMessage = "Halter nicht angegeben."
                        Case "NO_ZE"
                            m_strMessage = "Empfänger Scheinr nicht angegeben."
                        Case "NO_RE"
                            m_strMessage = "Rechnungsempfänger nicht angegeben."
                        Case "NO_RG"
                            m_strMessage = "Regulierer nicht angegeben."
                        Case "NO_ZS_ZUM_RE"
                            m_strMessage = "Zum Rechnungsempfänger konnte kein Empfänger des Briefes gefunden werden."
                        Case "INV_PLZ"
                            m_strMessage = "Postleitzahl ZH falsch."
                        Case "NO_ZULST"
                            m_strMessage = "Keine Zulassungsstelle gefunden."
                        Case "SMTP_ERROR"
                            m_strMessage = "E-Mail Adresse fehlerhaft."
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Fehler beim Speichern.<br>(" & ex.Message & ")"
                    End Select

                    If m_intStatus <> -9999 Then
                        m_strMessage = m_strMessage & " Bitte korrigieren Sie Ihre Eingaben."
                    End If

                    If intID > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                    End If

                    WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
                Finally
                    m_blnGestartet = False
                End Try
            End If

        End Sub
        Public Function GetAdresse(ByVal Kennung As String, _
                                   ByVal Name As String, _
                                   ByVal PLZ As String, _
                                   ByVal Ort As String) As DataTable


            Dim intID As Int32 = -1

            Dim exportTable As New DataTable
            m_intStatus = 0

            Try
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, _
                                                            "Z_M_IMP_AUFTRDAT_007", m_strAppID, m_strSessionID, _
                                                            m_objUser.CurrentLogAccessASPXID)

                Dim proxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                proxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                proxy.setImportParameter("I_KENNUNG", Kennung)
                proxy.setImportParameter("I_NAME1", Name)
                proxy.setImportParameter("I_PSTLZ", PLZ)

                proxy.callBapi()

                m_tblResult = proxy.getExportTable("GT_WEB")

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                With exportTable
                    .Columns.Add("POS_TEXT", GetType(String))
                    .Columns.Add("POS_KURZTEXT", GetType(String))
                    .Columns.Add("Adresse", GetType(String))
                    .Columns.Add("Name1", GetType(String))
                    .Columns.Add("Name2", GetType(String))
                    .Columns.Add("STRAS", GetType(String))
                    .Columns.Add("PSTLZ", GetType(String))
                    .Columns.Add("ORT01", GetType(String))
                    .Columns.Add("EMAIL", GetType(String))
                    .Columns.Add("LAND1", GetType(String))
                    .Columns.Add("TELNR", GetType(String))
                End With

                Dim i As Long = 0

                For Each dataRow As DataRow In m_tblResult.Rows

                    Dim newRow = exportTable.NewRow

                    newRow("POS_TEXT") = dataRow("POS_TEXT").ToString
                    newRow("POS_KURZTEXT") = dataRow("POS_KURZTEXT").ToString
                    newRow("Name1") = dataRow("Name1").ToString
                    newRow("Name2") = dataRow("Name2").ToString
                    newRow("STRAS") = dataRow("STRAS").ToString
                    newRow("PSTLZ") = dataRow("PSTLZ").ToString
                    newRow("ORT01") = dataRow("ORT01").ToString
                    newRow("EMAIL") = dataRow("EMAIL").ToString
                    newRow("LAND1") = dataRow("LAND1").ToString
                    newRow("TELNR") = dataRow("TELNR").ToString

                    'Adresse für Ausgabe in Dropdown verketten
                    Dim adresse = dataRow("Name1").ToString & "|"
                    If Kennung = "EVB" Then
                        adresse = adresse & dataRow("POS_TEXT").ToString
                    Else
                        adresse = adresse & dataRow("STRAS").ToString & "|"
                        adresse = adresse & dataRow("PSTLZ").ToString & "|"
                        adresse = adresse & dataRow("ORT01").ToString
                    End If


                    newRow("Adresse") = adresse

                    exportTable.Rows.Add(newRow)
                    i = i + 1
                Next

                WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR & ",I_KENNUNG=" & Kennung, m_tblResult)

            Catch ex As Exception
                m_intStatus = -5555

                Select Case Base.Business.HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."

                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Fehler beim Abfragen der Leasingnehmerdaten.<br>(" & ex.Message & ")"

                End Select

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

                Throw New Exception(m_strMessage)

            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If
            End Try


            Return exportTable
        End Function

        Public Sub GetLaender()
            Dim intID As Int32 = -1

            Try

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Land_Plz_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim proxy = DynSapProxy.getProxy("Z_M_LAND_PLZ_001", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                proxy.callBapi()

                m_objLogApp.WriteEndDataAccessSAP(intID, True)

                m_tblLaender = proxy.getExportTable("GT_WEB")

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
        Public Sub getSTVA(ByRef page As Web.UI.Page, ByVal strPLZ As String)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", m_objApp, m_objUser, Page)

                myProxy.setImportParameter("I_PLZ", strPLZ)
                myProxy.setImportParameter("I_ORT", "")

                myProxy.callBapi()

                Dim table As DataTable = myProxy.getExportTable("T_ZULST")

                m_tblKreis = table.Copy

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

                    Case "ERR_INV_PLZ"
                        m_strMessage = "Ungültige Postleitzahl."
                        m_intStatus = -1118
                    Case Else
                        m_strMessage = "Unbekannter Fehler."
                        m_intStatus = -9999
                End Select

            End Try

        End Sub

        Public Sub getZulassungsart(ByRef page As Web.UI.Page)
            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_GET_TVV3T", m_objApp, m_objUser, page)

                myProxy.callBapi()

                mTblZulassungsart = myProxy.getExportTable("GT_OUT")

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_strMessage = "Unbekannter Fehler."
                        m_intStatus = -9999
                End Select
            End Try
        End Sub
    End Class
End Namespace
