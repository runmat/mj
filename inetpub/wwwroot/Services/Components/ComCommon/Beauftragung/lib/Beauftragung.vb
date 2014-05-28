Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common
Imports System.Security.Cryptography
Imports System.IO

Public Class Beauftragung
    Inherits DatenimportBase

#Region "Declarations"
    Private connection As SqlClient.SqlConnection
    Private mVerkaufsorganisation As String
    Private mVerkaufsbuero As String
    Private mGruppe As String
    Private mKunden As DataTable
    Private mDienstleistungen As DataTable
    Private mKreise As DataTable
    Private mTypdatenMessage As String
    Private mKunnr As String
    Private mKundenname As String
    Private mHalterAnrede As String
    Private mHaltername1 As String
    Private mHaltername2 As String
    Private mName As String
    Private mGeburtsname As String
    Private mGeburtstag As String
    Private mGeburtsort As String
    Private mHalterStrasse As String
    Private mHalterHausnr As String
    Private mHalterHausnrZusatz As String
    Private mHalterPLZ As String
    Private mHalterOrt As String
    Private mGrosskundennr As String
    Private mStVA As String
    Private mStVANr As String
    Private mReferenz As String
    Private mVerkKuerz As String
    Private mKiReferenz As String
    Private mNotiz As String
    Private mHersteller As String
    Private mTyp As String
    Private mVarianteVersion As String
    Private mTypPruef As String
    Private mFahrgestellnummer As String
    Private mFahrgestellnummerPruef As String
    Private mDienstleistungsnr As String
    Private mDienstleistung As String
    Private mEVB As String
    Private mZulassungsdatum As String
    Private mKennzeichen As String
    Private mBemerkung As String
    Private mEinzelkennzeichen As Boolean
    Private mKrad As Boolean
    Private mKennzeichenTyp As String
    Private mFeinstaubplakette As Boolean
    Private mWunschkennzeichen As Boolean
    Private mReserviert As Boolean
    Private mReservierungsnr As String
    Private mStatustext As String
    Private mMaterialnummer As String
    Private mHalterReferenz As String
    Private mBriefnummer As String
    Private mZuldatVon As String
    Private mZuldatBis As String
    Private mLoeschkennzeichen As String
    Private mBLZ As String
    Private mKontonr As String
    Private mEinzug As String
    Private mHe As String
    Private mFr As String
    Private mFi As String
    Private mNPaUsed As Boolean = False
    Private mBestellnummer As String
    Private mGrosskunde As String
    Private mErrText As String

    Private mBankdatenNeeded As Char = "N"c
    Private mEvBNeeded As Char = "N"c
    Private mGutachtenNeeded As Char
    Private mNaechsteHUNeeded As Char
    Private mHalterNeeded As Char
    Private mTypDatenNeeded As Char

    Private mNaechsteHU As String
    Private mArtGenehmigung As String
    Private mPrueforganisation As String
    Private mGutachtenNummer As String

    Private mGenehmigungArten As DataTable
    Private mPrueforganisationen As DataTable

    Private mVerkaeufer As String
    Private mKundenreferenz As String
    Private mKundennotiz As String

#End Region

#Region " Properties"

    Public Property Verkaeufer() As String
        Get
            Return mVerkaeufer
        End Get
        Set(ByVal value As String)
            mVerkaeufer = value
        End Set
    End Property

    Private Property Kundenreferenz() As String
        Get
            Return mKundenreferenz
        End Get
        Set(ByVal value As String)
            mKundenreferenz = value
        End Set
    End Property

    Private Property Kundennotiz() As String
        Get
            Return mKundennotiz
        End Get
        Set(ByVal value As String)
            mKundennotiz = value
        End Set
    End Property

    Public ReadOnly Property GenehmigungsArten() As DataTable
        Get
            Return mGenehmigungArten
        End Get
    End Property

    Public ReadOnly Property Prueforganisationen() As DataTable
        Get
            Return mPrueforganisationen
        End Get
    End Property

    Public Property EvBNeeded() As Char
        Get
            Return mEvBNeeded
        End Get
        Set(ByVal value As Char)
            mEvBNeeded = value
        End Set
    End Property
    Public Property GutachtenNeeded() As Char
        Get
            Return mGutachtenNeeded
        End Get
        Set(ByVal value As Char)
            mGutachtenNeeded = value
        End Set
    End Property
    Public Property NaechsteHUNeeded() As Char
        Get
            Return mNaechsteHUNeeded
        End Get
        Set(ByVal value As Char)
            mNaechsteHUNeeded = value
        End Set
    End Property
    Public Property BankdatenNeeded() As Char
        Get
            Return mBankdatenNeeded
        End Get
        Set(ByVal value As Char)
            mBankdatenNeeded = value
        End Set
    End Property

    Public Property NaechsteHU() As String
        Get
            Return mNaechsteHU
        End Get
        Set(ByVal value As String)
            mNaechsteHU = value
        End Set
    End Property
    Public Property ArtGenehmigung() As String
        Get
            Return mArtGenehmigung
        End Get
        Set(ByVal value As String)
            mArtGenehmigung = value
        End Set
    End Property
    Public Property Prueforganisation() As String
        Get
            Return mPrueforganisation
        End Get
        Set(ByVal value As String)
            mPrueforganisation = value
        End Set
    End Property
    Public Property GutachtenNummer() As String
        Get
            Return mGutachtenNummer
        End Get
        Set(ByVal value As String)
            mGutachtenNummer = value
        End Set
    End Property

    Public Property Verkaufsorganisation() As String
        Get
            Return mVerkaufsorganisation
        End Get
        Set(ByVal value As System.String)
            mVerkaufsorganisation = value
        End Set
    End Property
    Public Property Verkaufsbuero() As String
        Get
            Return mVerkaufsbuero
        End Get
        Set(ByVal value As System.String)
            mVerkaufsbuero = value
        End Set
    End Property
    Public Property Gruppe() As String
        Get
            Return mGruppe
        End Get
        Set(ByVal value As System.String)
            mGruppe = value
        End Set
    End Property

    Public Property TypdatenMessage() As String
        Get
            Return mTypdatenMessage
        End Get
        Set(ByVal value As System.String)
            mTypdatenMessage = value
        End Set
    End Property

    Public Property Kunden() As DataTable
        Get
            Return mKunden
        End Get
        Set(ByVal value As DataTable)
            mKunden = value
        End Set
    End Property

    Public Property Dienstleistungen() As DataTable
        Get
            Return mDienstleistungen
        End Get
        Set(ByVal value As DataTable)
            mDienstleistungen = value
        End Set
    End Property

    Public Property Kreise() As DataTable
        Get
            Return mKreise
        End Get
        Set(ByVal value As DataTable)
            mKreise = value
        End Set
    End Property

    Public Property Kundennr() As String
        Get
            Return mKunnr
        End Get
        Set(ByVal value As String)
            mKunnr = value
        End Set
    End Property

    Public Property Kundenname() As String
        Get
            Return mKundenname
        End Get
        Set(ByVal value As String)
            mKundenname = value
        End Set
    End Property

    Public Property Grosskundennr() As String
        Get
            Return mGrosskundennr
        End Get
        Set(ByVal value As String)
            mGrosskundennr = value
        End Set
    End Property

    Public Property HalterAnrede() As String
        Get
            Return mHalterAnrede
        End Get
        Set(ByVal value As String)
            mHalterAnrede = value
        End Set
    End Property

    Public Property Haltername1() As String
        Get
            Return mHaltername1
        End Get
        Set(ByVal value As String)
            mHaltername1 = value
        End Set
    End Property
    Public Property Haltername2() As String
        Get
            Return mHaltername2
        End Get
        Set(ByVal value As String)
            mHaltername2 = value
        End Set
    End Property


    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property
    Public Property Geburtsname() As String
        Get
            Return mGeburtsname
        End Get
        Set(ByVal value As String)
            mGeburtsname = value
        End Set
    End Property

    Public Property Geburtstag() As String
        Get
            Return mGeburtstag
        End Get
        Set(ByVal value As String)
            mGeburtstag = value
        End Set
    End Property

    Public Property Geburtsort() As String
        Get
            Return mGeburtsort
        End Get
        Set(ByVal value As String)
            mGeburtsort = value
        End Set
    End Property

    Public Property HalterStrasse() As String
        Get
            Return mHalterStrasse
        End Get
        Set(ByVal value As String)
            mHalterStrasse = value
        End Set
    End Property

    Public Property HalterHausnr() As String
        Get
            Return mHalterHausnr
        End Get
        Set(ByVal value As String)
            mHalterHausnr = value
        End Set
    End Property

    Public Property HalterHausnrZusatz() As String
        Get
            Return mHalterHausnrZusatz
        End Get
        Set(ByVal value As String)
            mHalterHausnrZusatz = value
        End Set
    End Property


    Public Property HalterPLZ() As String
        Get
            Return mHalterPLZ
        End Get
        Set(ByVal value As String)
            mHalterPLZ = value
        End Set
    End Property

    Public Property HalterOrt() As String
        Get
            Return mHalterOrt
        End Get
        Set(ByVal value As String)
            mHalterOrt = value
        End Set
    End Property

    Public Property Referenz() As String
        Get
            Return mReferenz
        End Get
        Set(ByVal value As String)
            mReferenz = value
        End Set
    End Property

    Public Property VerkKuerz() As String
        Get
            Return mVerkKuerz
        End Get
        Set(ByVal value As String)
            mVerkKuerz = value
        End Set
    End Property

    Public Property KiReferenz() As String
        Get
            Return mKiReferenz
        End Get
        Set(ByVal value As String)
            mKiReferenz = value
        End Set
    End Property

    Public Property Notiz() As String
        Get
            Return mNotiz
        End Get
        Set(ByVal value As String)
            mNotiz = value
        End Set
    End Property

    Public Property Hersteller() As String
        Get
            Return mHersteller
        End Get
        Set(ByVal value As String)
            mHersteller = value
        End Set
    End Property

    Public Property Typ() As String
        Get
            Return mTyp
        End Get
        Set(ByVal value As String)
            mTyp = value
        End Set
    End Property

    Public Property VarianteVersion() As String
        Get
            Return mVarianteVersion
        End Get
        Set(ByVal value As String)
            mVarianteVersion = value
        End Set
    End Property

    Public Property TypPruef() As String
        Get
            Return mTypPruef
        End Get
        Set(ByVal value As String)
            mTypPruef = value
        End Set
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return mFahrgestellnummer
        End Get
        Set(ByVal value As String)
            mFahrgestellnummer = value
        End Set
    End Property

    Public Property FahrgestellnummerPruef() As String
        Get
            Return mFahrgestellnummerPruef
        End Get
        Set(ByVal value As String)
            mFahrgestellnummerPruef = value
        End Set
    End Property

    Public Property StVA() As String
        Get
            Return mStVA
        End Get
        Set(ByVal value As String)
            mStVA = value
        End Set
    End Property

    Public Property StVANr() As String
        Get
            Return mStVANr
        End Get
        Set(ByVal value As String)
            mStVANr = value
        End Set
    End Property

    Public Property Dienstleistungsnr() As String
        Get
            Return mDienstleistungsnr
        End Get
        Set(ByVal value As String)
            mDienstleistungsnr = value
        End Set
    End Property

    Public Property Dienstleistung() As String
        Get
            Return mDienstleistung
        End Get
        Set(ByVal value As String)
            mDienstleistung = value
        End Set
    End Property

    Public Property EVB() As String
        Get
            Return mEVB
        End Get
        Set(ByVal value As String)
            mEVB = value
        End Set
    End Property

    Public Property Zulassungsdatum() As String
        Get
            Return mZulassungsdatum
        End Get
        Set(ByVal value As String)
            mZulassungsdatum = value
        End Set
    End Property

    Public Property Kennzeichen() As String
        Get
            Return mKennzeichen
        End Get
        Set(ByVal value As String)
            mKennzeichen = value
        End Set
    End Property

    Public Property Bemerkung() As String
        Get
            Return mBemerkung
        End Get
        Set(ByVal value As String)
            mBemerkung = value
        End Set
    End Property

    Public Property Einzelkennzeichen() As Boolean
        Get
            Return mEinzelkennzeichen
        End Get
        Set(ByVal value As Boolean)
            mEinzelkennzeichen = value
        End Set
    End Property
   
    Public Property Krad() As Boolean
        Get
            Return mKrad
        End Get
        Set(ByVal value As Boolean)
            mKrad = value
        End Set
    End Property

    Public Property KennzeichenTyp() As String
        Get
            Return mKennzeichenTyp
        End Get
        Set(ByVal value As String)
            mKennzeichenTyp = value
        End Set
    End Property

    Public Property Feinstaubplakette() As Boolean
        Get
            Return mFeinstaubplakette
        End Get
        Set(ByVal value As Boolean)
            mFeinstaubplakette = value
        End Set
    End Property

    Public Property Wunschkennzeichen() As Boolean
        Get
            Return mWunschkennzeichen
        End Get
        Set(ByVal value As Boolean)
            mWunschkennzeichen = value
        End Set
    End Property

    Public Property Reserviert() As Boolean
        Get
            Return mReserviert
        End Get
        Set(ByVal value As Boolean)
            mReserviert = value
        End Set
    End Property

    Public Property Reservierungsnr() As String
        Get
            Return mReservierungsnr
        End Get
        Set(ByVal value As String)
            mReservierungsnr = value
        End Set
    End Property

    Public Property Statustext() As String
        Get
            Return mStatustext
        End Get
        Set(ByVal value As String)
            mStatustext = value
        End Set
    End Property

    Public Property Materialnummer() As String
        Get
            Return mMaterialnummer
        End Get
        Set(ByVal value As String)
            mMaterialnummer = value
        End Set
    End Property
    Public Property HalterReferenz() As String
        Get
            Return mHalterReferenz
        End Get
        Set(ByVal value As String)
            mHalterReferenz = value
        End Set
    End Property
    Public Property Bestellnummer() As String
        Get
            Return mBestellnummer
        End Get
        Set(ByVal value As String)
            mBestellnummer = value
        End Set
    End Property
    Public Property Briefnummer() As String
        Get
            Return mBriefnummer
        End Get
        Set(ByVal value As String)
            mBriefnummer = value
        End Set
    End Property

    Public Property ZuldatVon() As String
        Get
            Return mZuldatVon
        End Get
        Set(ByVal value As String)
            mZuldatVon = value
        End Set
    End Property
    Public Property ZuldatBis() As String
        Get
            Return mZuldatBis
        End Get
        Set(ByVal value As String)
            mZuldatBis = value
        End Set
    End Property

    Public Property Loeschkennzeichen() As String
        Get
            Return mLoeschkennzeichen
        End Get
        Set(ByVal value As String)
            mLoeschkennzeichen = value
        End Set
    End Property

    Public Property BLZ() As String
        Get
            Return mBLZ
        End Get
        Set(ByVal value As String)
            mBLZ = value
        End Set
    End Property

    Public Property Kontonummer() As String
        Get
            Return mKontonr
        End Get
        Set(ByVal value As String)
            mKontonr = value
        End Set
    End Property

    Public Property Einzug() As String
        Get
            Return mEinzug
        End Get
        Set(ByVal value As String)
            mEinzug = value
        End Set
    End Property

    Public Property He() As String
        Get
            Return mHe
        End Get
        Set(ByVal value As String)
            mHe = value
        End Set
    End Property

    Public Property Fr() As String
        Get
            Return mFr
        End Get
        Set(ByVal value As String)
            mFr = value
        End Set
    End Property

    Public Property Fi() As String
        Get
            Return mFi
        End Get
        Set(ByVal value As String)
            mFi = value
        End Set
    End Property

    Public Property nPaUsed() As Boolean
        Get
            Return mNPaUsed
        End Get
        Set(ByVal value As Boolean)
            mNPaUsed = value
        End Set
    End Property

    Public Property Grosskunde() As String
        Get
            Return mGrosskunde
        End Get
        Set(ByVal value As String)
            mGrosskunde = value
        End Set
    End Property


    Public Property ErrorText() As String
        Get
            Return mErrText
        End Get
        Set(ByVal value As String)
            mErrText = value
        End Set
    End Property

    Public Property HalterNeeded As Char
        Get
            Return mHalterNeeded
        End Get
        Set(value As Char)
            mHalterNeeded = value
        End Set
    End Property

    Public Property TypDatenNeeded As Char
        Get
            Return mHalterNeeded
        End Get
        Set(value As Char)
            mHalterNeeded = value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal AppID As String, ByVal SessionID As String, ByVal FileName As String)
        MyBase.New(objUser, objApp, FileName)
    End Sub

    Public Overloads Sub Fill(ByVal AppID As String, ByVal SessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Beauftragung.FILL"
        m_strAppID = AppID
        m_strSessionID = SessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1


            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_F_CK_GRUPPENDATEN", m_objApp, m_objUser, page)


                myProxy.setImportParameter("I_VKORG", mVerkaufsorganisation)
                myProxy.setImportParameter("I_VKBUR", mVerkaufsbuero)
                myProxy.setImportParameter("I_GRUPPE", mGruppe)


                myProxy.callBapi()


                mKunden = myProxy.getExportTable("GT_KUNNR")

                'mDienstleistungen = myProxy.getExportTable("GT_MATNR")

                mKreise = myProxy.getExportTable("GT_KREISKZ")


                For Each Row As DataRow In mKunden.Rows
                    Row("NAME1") = Row("NAME1").ToString & " ~ " & Right("00000000" & Row("KUNNR").ToString, 8)

                    If Row("DATLT").ToString <> String.Empty Then
                        Row("NAME1") = Row("NAME1").ToString & " / " & Row("DATLT").ToString
                    End If
                Next

                mKunden.AcceptChanges()

                'StVA füllen            
                For Each Row As DataRow In mKreise.Rows
                    Row("KREISBEZ") = Left(Row("KREISKZ").ToString & "....", 4) & Row("KREISBEZ").ToString
                Next
                mKreise.AcceptChanges()


            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    'ToDo ErrMessage
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub


    Public Overloads Sub FillUebersicht(ByVal AppID As String, ByVal SessionID As String, ByVal page As Web.UI.Page)

        m_strClassAndMethod = "Beauftragung.FillUebersicht"
        m_strAppID = AppID
        m_strSessionID = SessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1


            Try


                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_ZULASSUNGSDATEN_EKFZ", m_objApp, m_objUser, page)


                myProxy.setImportParameter("KUNNR", Kundennr.PadLeft(10, "0"c))
                myProxy.setImportParameter("ZZREFNR", Referenz)
                myProxy.setImportParameter("ZZKENN", Kennzeichen)
                myProxy.setImportParameter("ZZZLDAT_VON", ZuldatVon)
                myProxy.setImportParameter("ZZZLDAT_BIS", ZuldatBis)
                myProxy.setImportParameter("ZZLOESCH", Loeschkennzeichen)
                myProxy.setImportParameter("ZVERKAEUFER", Verkaeufer)
                myProxy.setImportParameter("ZKUNDREF", Kundenreferenz)
                myProxy.setImportParameter("ZKUNDNOTIZ", Kundennotiz)

                myProxy.callBapi()

                Dim TempTable As DataTable
                TempTable = myProxy.getExportTable("EXTAB")

                TempTable.Columns("ZZSTATUSUHRZEIT").MaxLength = 8
                TempTable.AcceptChanges()

                For Each dr As DataRow In TempTable.Rows


                    If dr("ZZSTATUSDATUM").ToString.Length = 0 Then
                        dr("ZZSTATUSUHRZEIT") = ""

                    End If

                    TempTable.AcceptChanges()
                Next

                CreateOutPut(TempTable, AppID)

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    'ToDo ErrMessage
                    Case "NO_DATA"
                        m_strMessage = "Es konnte keine Daten ermittelt werden."
                    Case "NO_INTERVAL"
                        m_strMessage = "Ungültiger Zeitraum."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overloads Sub FillZulassung(ByVal AppID As String, ByVal SessionID As String, ByVal page As Web.UI.Page, ByVal datumAb As Date, ByVal datumBis As Date)

        m_strClassAndMethod = "Beauftragung.FillZulassung"
        m_strAppID = AppID
        m_strSessionID = SessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1


            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_ONLSTAT_001", m_objApp, m_objUser, page)


                myProxy.setImportParameter("I_KREIS", "B")
                myProxy.setImportParameter("I_VKORG", "1010")
                myProxy.setImportParameter("I_VKBUR", "4837")

                myProxy.setImportParameter("I_VON", HelpProcedures.MakeDateSAP(datumAb.ToShortDateString))
                myProxy.setImportParameter("I_BIS", HelpProcedures.MakeDateSAP(datumBis.ToShortDateString))

                myProxy.callBapi()



                Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_ONL_ZUL")

                For Each dr As DataRow In tblTemp2.Rows

                    If dr("ZZSTATUSUHRZEIT").ToString = "000000" Then
                        dr("ZZSTATUSUHRZEIT") = ""
                    Else
                        dr("ZZSTATUSUHRZEIT") = Left(dr("ZZSTATUSUHRZEIT").ToString, 2) & ":" & Mid(dr("ZZSTATUSUHRZEIT").ToString, 3, 2)
                    End If

                Next


                ResultTable = tblTemp2

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub


    Public Function Save(ByVal strAppID As String, _
                    ByVal strSessionID As String, _
                    ByVal page As Web.UI.Page) As Boolean

        m_strClassAndMethod = "Beauftragung.Save"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""

        Dim Success As Boolean = False
        Dim EncryptData As String = ""


        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_VORERFASSUNG_EKFZ", m_objApp, m_objUser, page)

                Dim BelegTable As DataTable = myProxy.getImportTable("IT_BELEG")
                Dim BelegRow As DataRow = BelegTable.NewRow

                BelegRow("VKORG") = m_objUser.Reference.Substring(0, 4)
                BelegRow("VKBUR") = m_objUser.Reference.Substring(4, 4)
                BelegRow("KUNNR") = Kundennr.PadLeft(10, "0"c)
                BelegRow("ZZKENN") = Kennzeichen

                If Zulassungsdatum Is Nothing Then
                    BelegRow("ZZZLDAT") = System.DBNull.Value
                Else
                    BelegRow("ZZZLDAT") = Zulassungsdatum
                End If

                BelegRow("USERID") = m_objUser.UserID.ToString
                BelegRow("USERNAME") = m_objUser.UserName
                BelegRow("RESERVIERT") = IIf(Reserviert, "X", "")
                BelegRow("RESERVID") = Reservierungsnr
                BelegRow("KENNZTYP") = KennzeichenTyp
                BelegRow("EINKZ") = IIf(Einzelkennzeichen, "X", "")
                BelegRow("ZZWUNSCH") = IIf(Wunschkennzeichen, "X", "")
                BelegRow("ZKUNDREF") = KiReferenz
                BelegRow("ZKUNDNOTIZ") = Notiz
                BelegRow("ZFEINSTAUB_KZ") = IIf(Feinstaubplakette, "X", "")
                BelegRow("ZKRAD_KZ") = IIf(Krad, "X", "")
                BelegRow("ZZFAHRG") = Bestellnummer
                BelegRow("KREIS") = StVANr
                BelegRow("MATNR") = Materialnummer
                BelegRow("ZZHALTER") = HalterReferenz
                BelegRow("ZVERKAEUFER") = VerkKuerz
                BelegRow("ZZTEXT") = Bemerkung
                BelegRow("BANKL") = BLZ
                BelegRow("BANKN") = Kontonummer
                BelegRow("XEZER") = Einzug


                Dim SapID As Integer
                Dim ReturnCode As String = ""

                SapID = GiveNewZulassungsID(ReturnCode)

                If ReturnCode.Length > 0 Then
                    Success = False
                    Return Success
                End If

                SetNewZulassungsID(SapID)

                BelegRow("ID") = SapID.ToString

                BelegTable.Rows.Add(BelegRow)
                BelegTable.AcceptChanges()

                Dim VorerfassungTable As DataTable = myProxy.getImportTable("I_VORERFASSUNG_EKFZ_01")
                Dim VorerfassungRow As DataRow = VorerfassungTable.NewRow

                VorerfassungRow("ID") = SapID.ToString
                VorerfassungRow("VORG") = Dienstleistungen.Select("MATNR='" & Materialnummer & "'")(0)("VORG")
                VorerfassungRow("AKZ") = Kennzeichen
                VorerfassungRow("FGNU") = Fahrgestellnummer
                VorerfassungRow("ANR") = HalterAnrede
                VorerfassungRow("VNAM") = Haltername1
                VorerfassungRow("RNAM") = Haltername2
                VorerfassungRow("STRN") = HalterStrasse
                VorerfassungRow("STRH") = HalterHausnr
                VorerfassungRow("STRB") = HalterHausnrZusatz
                VorerfassungRow("PLZ") = HalterPLZ
                VorerfassungRow("ORT") = HalterOrt

                If Zulassungsdatum Is Nothing Then
                    VorerfassungRow("ZUDA") = System.DBNull.Value
                Else
                    VorerfassungRow("ZUDA") = Zulassungsdatum
                End If

                VorerfassungRow("HERS") = Hersteller
                VorerfassungRow("TYP") = Typ
                VorerfassungRow("VVS") = VarianteVersion
                VorerfassungRow("TYPZ") = TypPruef
                VorerfassungRow("FGPZ") = FahrgestellnummerPruef
                VorerfassungRow("ZZREFERENZCODE") = Referenz
                VorerfassungRow("ZZGROSSKUNDNR") = Grosskundennr
                VorerfassungRow("ZZEVB") = EVB
                VorerfassungRow("BRNR") = Briefnummer
                VorerfassungRow("BLZ") = BLZ
                VorerfassungRow("KONTO") = Kontonummer

                If mNaechsteHUNeeded = "H"c Or mNaechsteHUNeeded = "G"c Or mNaechsteHUNeeded = "B"c Then
                    VorerfassungRow("NAHU") = NaechsteHU
                Else
                    VorerfassungRow("NAHU") = System.DBNull.Value
                End If

                VorerfassungRow("ARTGE") = ArtGenehmigung
                VorerfassungRow("PRUEFORG") = Prueforganisation
                VorerfassungRow("NRGUT") = GutachtenNummer

                If Geburtstag Is Nothing Then
                    VorerfassungRow("GEBDAT") = System.DBNull.Value
                Else
                    VorerfassungRow("GEBDAT") = Geburtstag
                End If

                VorerfassungRow("GEBORT") = Geburtsort


                VorerfassungTable.Rows.Add(VorerfassungRow)
                VorerfassungTable.AcceptChanges()


                'Importparameter

                If nPaUsed = True Then

                    Dim StringToEncrypt As String

                    StringToEncrypt = Haltername1 & "|"
                    StringToEncrypt &= Haltername2 & "|"
                    StringToEncrypt &= HalterStrasse & "|"
                    StringToEncrypt &= HalterPLZ & "|"
                    StringToEncrypt &= HalterOrt & "|"
                    StringToEncrypt &= Geburtstag & "|"
                    StringToEncrypt &= Geburtsort

                    EncryptData = EncrData(StringToEncrypt)

                End If


                myProxy.setImportParameter("I_STRING", EncryptData)


                myProxy.callBapi()

                Dim ErrorTable As DataTable = myProxy.getExportTable("ET_FEHLER")

                If ErrorTable.Rows.Count > 0 Then

                    Success = False
                    ErrorText = ErrorTable.Rows(0)("FEHLERTEXT").ToString

                Else
                    ErrorText = EncryptData
                    Success = True
                End If


            Catch ex As Exception
                m_intStatus = -5555
                Success = False
                ErrorText = ex.Message
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Speichern.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False

            End Try
        End If

        Return Success


    End Function


    Public Function CheckVin(ByVal Fahrgestellnummer As String, ByVal Pruefziffer As String, ByVal page As Web.UI.Page) As Integer
        m_strClassAndMethod = "Beauftragung.CheckVin"
        m_strAppID = AppID
        m_strSessionID = SessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_PRUEF_FIN_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_FGNU", Fahrgestellnummer)
                myProxy.setImportParameter("I_FGPZ", Pruefziffer)

                myProxy.callBapi()

                Return CInt(myProxy.getExportParameter("E_STATUS"))

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    'ToDo ErrMessage
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Function


    Public Function FillTypdaten(ByVal Hersteller As String, _
                                 ByVal Typschluessel As String, _
                                 ByVal VVS As String, _
                                 ByVal Pruefziffer As String, _
                                 ByVal page As Web.UI.Page) As DataTable

        m_strClassAndMethod = "Beauftragung.FillTypdaten"
        m_strAppID = AppID
        m_strSessionID = SessionID

        Dim TempTable As New DataTable

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_TYPDATEN_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_ZZHERSTELLER_SCH", Hersteller)
                myProxy.setImportParameter("I_ZZTYP_SCHL", Typschluessel)
                myProxy.setImportParameter("I_ZZVVS_SCHLUESSEL", VVS)
                myProxy.setImportParameter("I_ZZTYP_VVS_PRUEF", Pruefziffer)


                myProxy.callBapi()

                mTypdatenMessage = myProxy.getExportParameter("E_MESSAGE")

                TempTable = myProxy.getExportTable("GT_WEB")


            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    'ToDo ErrMessage
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If

        Return TempTable

    End Function

    Public Function CheckGrosskundennummer(ByVal KBA As String, ByVal Grosskundennummer As String, ByVal page As Web.UI.Page) As String
        m_strClassAndMethod = "Beauftragung.CheckGrosskundennummer"
        m_strAppID = AppID
        m_strSessionID = SessionID
        Dim ReturnValue As String = ""

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_GROSSKUNDEN_PRUEF", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_ZKBA1", KBA)
                myProxy.setImportParameter("I_ZZGROSSKUNDNR", Grosskundennummer)
                myProxy.setImportParameter("I_ZKUNNR", Kundennr.PadLeft(10, "0"c))



                myProxy.callBapi()

                Dim TempTable As DataTable = myProxy.getExportTable("GT_OUT")

                If TempTable.Rows.Count > 0 Then
                    ReturnValue = TempTable.Rows(0)("ZNAME1").ToString
                End If

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    'ToDo ErrMessage
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

            Finally
                m_blnGestartet = False
            End Try
        End If

        Return ReturnValue

    End Function

    Public Function CheckBarcode(ByVal Barcode As String, ByVal page As Web.UI.Page) As String
        m_strClassAndMethod = "Beauftragung.CheckBarcode"
        m_strAppID = AppID
        m_strSessionID = SessionID
        Dim ReturnValue As String = ""

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_STATUS_REFCODE", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_ZZREFERENZCODE", Barcode)

                myProxy.callBapi()

                ReturnValue = myProxy.getExportParameter("E_RETURNCODE")
                mStatustext = myProxy.getExportParameter("E_ZZSTATUS")

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    'ToDo ErrMessage
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

            Finally
                m_blnGestartet = False
            End Try
        End If

        Return ReturnValue

    End Function


    Private Function GiveNewZulassungsID(ByRef returnStatus As String) As Int32
        returnStatus = ""
        Try
            openConnection()
            'Save Data
            Return DBGiveNewZulassungsID()
        Catch ex As Exception
            returnStatus = ex.Message
        Finally
            closeConnection()
        End Try

    End Function

    Private Function DBGiveNewZulassungsID() As Int32
        Dim command As New SqlClient.SqlCommand()
        With command
            .Connection = connection
            .CommandType = CommandType.Text
        End With
        command.CommandText = "SELECT PValue FROM Parameters WHERE  (PName = 'HoechsteZulassungsID')"
        Return CType(command.ExecuteScalar, System.Int32) + 1
    End Function

    Public Function SetNewZulassungsID(ByVal id_sap As Int32) As Int32
        Dim command2 As New SqlClient.SqlCommand()
        openConnection()
        With command2
            .Connection = connection
            .CommandType = CommandType.Text
            .Parameters.Clear()
        End With
        command2.CommandText = "SELECT PValue FROM Parameters WHERE  (PName = 'HoechsteZulassungsID')"
        If id_sap > CType(command2.ExecuteScalar, System.Int32) Then
            command2.CommandText = "UPDATE Parameters SET PValue = " & id_sap.ToString & " WHERE  (PName = 'HoechsteZulassungsID')"
            command2.ExecuteNonQuery()
        End If
        closeConnection()
    End Function

    Private Sub openConnection()
        connection = New SqlClient.SqlConnection()
        connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
        connection.Open()
    End Sub

    Private Sub closeConnection()
        connection.Close()
        connection.Dispose()
    End Sub

    Public Sub FillDienstleistungen(ByVal Amt As String, ByVal VKORG As String, ByVal page As Web.UI.Page)

        mStVA = Amt
        mVerkaufsorganisation = VKORG

        If Not mVerkaufsorganisation Is Nothing Then
            m_strClassAndMethod = "Beauftragung.FillDienstleistungen"
            m_strAppID = AppID
            m_strSessionID = SessionID

            Dim TempTable As New DataTable

            If Not m_blnGestartet Then
                m_blnGestartet = True
                Dim intID As Int32 = -1

                Try

                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_EXPORT_ONLMAT", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_AMT", mStVA)
                    myProxy.setImportParameter("I_VKORG", mVerkaufsorganisation)

                    myProxy.callBapi()

                    mTypdatenMessage = myProxy.getExportParameter("E_SUBRC")
                    mTypdatenMessage = myProxy.getExportParameter("E_MESSAGE")


                    mDienstleistungen = myProxy.getExportTable("GT_ONLMAT")



                    If mDienstleistungen.Rows.Count > 0 Then
                        Dim tblTemp As New DataTable
                        tblTemp = mDienstleistungen.Clone

                        Dim NewRow As DataRow = tblTemp.NewRow

                        NewRow("MATNR") = ""
                        NewRow("MAKTX") = "--- Auswahl ---"

                        tblTemp.Rows.Add(NewRow)

                        For Each dr As DataRow In mDienstleistungen.Rows

                            NewRow = tblTemp.NewRow

                            For i = 0 To dr.Table.Columns.Count - 1

                                NewRow(i) = dr(i)

                            Next

                            tblTemp.Rows.Add(NewRow)

                        Next

                        mDienstleistungen = tblTemp

                    End If






                Catch ex As Exception
                    m_intStatus = -9999
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        'ToDo ErrMessage
                        Case Else
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                    End Select
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End If
    End Sub

    Public Sub FillPrueforganisation(ByVal page As Web.UI.Page)

        m_strClassAndMethod = "Beauftragung.FillPrueforganisation"
        m_strAppID = AppID
        m_strSessionID = SessionID

        Dim TempTable As New DataTable

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_DOMAENEN_WERTE", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_DOMNAME", "ZZLD_PRUEFORG")

                myProxy.callBapi()

                mTypdatenMessage = myProxy.getExportParameter("E_SUBRC")
                mTypdatenMessage = myProxy.getExportParameter("E_MESSAGE")

                mPrueforganisationen = myProxy.getExportTable("GT_WERTE")

                If mPrueforganisationen.Rows.Count > 0 Then
                    Dim tblTemp As New DataTable
                    tblTemp = mPrueforganisationen.Clone

                    Dim NewRow As DataRow = tblTemp.NewRow

                    NewRow("DOMVALUE_L") = ""
                    NewRow("DDTEXT") = "--- Auswahl ---"

                    tblTemp.Rows.Add(NewRow)

                    For Each dr As DataRow In mPrueforganisationen.Rows

                        NewRow = tblTemp.NewRow

                        For i = 0 To dr.Table.Columns.Count - 1

                            NewRow(i) = dr(i)

                        Next

                        tblTemp.Rows.Add(NewRow)

                    Next

                    mPrueforganisationen = tblTemp

                End If


            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    'ToDo ErrMessage
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

    Public Sub FillArtGenehmigung(ByVal page As Web.UI.Page)

        m_strClassAndMethod = "Beauftragung.FillArtGenehmigung"
        m_strAppID = AppID
        m_strSessionID = SessionID

        Dim TempTable As New DataTable

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_DOMAENEN_WERTE", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_DOMNAME", "ZZLD_ARTGE")

                myProxy.callBapi()

                mTypdatenMessage = myProxy.getExportParameter("E_SUBRC")
                mTypdatenMessage = myProxy.getExportParameter("E_MESSAGE")

                mGenehmigungArten = myProxy.getExportTable("GT_WERTE")

                If mGenehmigungArten.Rows.Count > 0 Then
                    Dim tblTemp As New DataTable
                    tblTemp = mGenehmigungArten.Clone

                    Dim NewRow As DataRow = tblTemp.NewRow

                    NewRow("DOMVALUE_L") = ""
                    NewRow("DDTEXT") = "--- Auswahl ---"

                    tblTemp.Rows.Add(NewRow)

                    For Each dr As DataRow In mGenehmigungArten.Rows

                        NewRow = tblTemp.NewRow

                        For i = 0 To dr.Table.Columns.Count - 1

                            NewRow(i) = dr(i)

                        Next

                        tblTemp.Rows.Add(NewRow)

                    Next

                    mGenehmigungArten = tblTemp

                End If




            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    'ToDo ErrMessage
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
       
    End Sub

    Private Function EncrData(ByVal TextToEncrypt As String) As String

        Dim EncText As String
        Dim rd As New RijndaelManaged

        Dim md5 As New MD5CryptoServiceProvider
        Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes("@S33wolf"))

        md5.Clear()
        rd.Key = key
        rd.GenerateIV()

        Dim iv() As Byte = rd.IV
        Dim ms As New MemoryStream

        ms.Write(iv, 0, iv.Length)

        Dim cs As New CryptoStream(ms, rd.CreateEncryptor, CryptoStreamMode.Write)
        Dim data() As Byte = System.Text.Encoding.UTF8.GetBytes(TextToEncrypt)

        cs.Write(data, 0, data.Length)
        cs.FlushFinalBlock()

        Dim encdata() As Byte = ms.ToArray()
        EncText = Convert.ToBase64String(encdata)
        cs.Close()
        rd.Clear()

        Return EncText


    End Function


    'Public Sub GetPruefOrganisationen(ByVal page As Page)

    '    m_strClassAndMethod = "Beauftragung.GetMaterialien"
    '    m_strAppID = AppID
    '    m_strSessionID = SessionID

    '    Dim TempTable As New DataTable

    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True
    '        Dim intID As Int32 = -1

    '        Try

    '            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ONLMAT_BAPI", m_objApp, m_objUser, page)

    '            myProxy.setImportParameter("I_AMT", mStVA)
    '            myProxy.setImportParameter("I_VKORG", mVerkaufsorganisation)

    '            myProxy.callBapi()

    '            mTypdatenMessage = myProxy.getExportParameter("E_MESSAGE")

    '            mDienstleistungen = myProxy.getExportTable("GT_WEB")
    '        Catch ex As Exception
    '            m_intStatus = -9999
    '            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
    '                'ToDo ErrMessage
    '                Case Else
    '                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
    '            End Select
    '        Finally
    '            m_blnGestartet = False
    '        End Try
    '    End If
    'End Sub
#End Region

End Class
' ************************************************
' $History: Beauftragung.vb $
' 
' *****************  Version 38  *****************
' User: Fassbenders  Date: 12.05.11   Time: 15:04
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 37  *****************
' User: Fassbenders  Date: 12.05.11   Time: 10:00
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 36  *****************
' User: Fassbenders  Date: 11.05.11   Time: 16:34
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 35  *****************
' User: Fassbenders  Date: 10.05.11   Time: 17:12
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 34  *****************
' User: Fassbenders  Date: 10.05.11   Time: 16:44
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 33  *****************
' User: Dittbernerc  Date: 10.05.11   Time: 16:41
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 32  *****************
' User: Fassbenders  Date: 9.05.11    Time: 19:25
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 31  *****************
' User: Fassbenders  Date: 6.05.11    Time: 14:59
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 30  *****************
' User: Fassbenders  Date: 6.05.11    Time: 14:12
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 29  *****************
' User: Dittbernerc  Date: 5.05.11    Time: 15:24
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 28  *****************
' User: Dittbernerc  Date: 21.04.11   Time: 11:36
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 27  *****************
' User: Fassbenders  Date: 15.04.11   Time: 13:53
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 26  *****************
' User: Fassbenders  Date: 2.03.11    Time: 8:53
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 25  *****************
' User: Fassbenders  Date: 22.02.11   Time: 17:34
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 24  *****************
' User: Fassbenders  Date: 31.01.11   Time: 15:15
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 23  *****************
' User: Fassbenders  Date: 31.01.11   Time: 8:53
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 22  *****************
' User: Fassbenders  Date: 12.01.11   Time: 14:50
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 21  *****************
' User: Fassbenders  Date: 10.01.11   Time: 15:07
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 20  *****************
' User: Fassbenders  Date: 6.10.10    Time: 14:53
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 19  *****************
' User: Fassbenders  Date: 17.06.10   Time: 11:18
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 18  *****************
' User: Fassbenders  Date: 10.06.10   Time: 15:04
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 17  *****************
' User: Fassbenders  Date: 26.02.10   Time: 16:45
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 16  *****************
' User: Fassbenders  Date: 23.02.10   Time: 14:45
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 15  *****************
' User: Fassbenders  Date: 17.02.10   Time: 13:12
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 14  *****************
' User: Fassbenders  Date: 17.02.10   Time: 10:19
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 13  *****************
' User: Fassbenders  Date: 3.02.10    Time: 19:34
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 12  *****************
' User: Fassbenders  Date: 14.12.09   Time: 15:02
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 11  *****************
' User: Fassbenders  Date: 9.12.09    Time: 14:22
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' ITA: 3383
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 7.12.09    Time: 12:49
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 2.12.09    Time: 14:21
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 1.12.09    Time: 13:26
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 26.11.09   Time: 14:44
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 25.11.09   Time: 16:24
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 20.11.09   Time: 14:32
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 17.11.09   Time: 17:56
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 17.11.09   Time: 8:35
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 13.11.09   Time: 15:52
' Updated in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 11.11.09   Time: 13:21
' Created in $/CKAG2/Services/Components/ComCommon/Beauftragung/lib
' 