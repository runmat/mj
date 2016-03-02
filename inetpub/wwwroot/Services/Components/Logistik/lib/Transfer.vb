Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Common

Public Class Transfer
    Inherits DatenimportBase

    Private configurationAppSettings As System.Configuration.AppSettingsReader

    Private mFahrzeuge As DataTable
    Private mFahrten As DataTable
    Private mAdressen As DataTable
    Private mHalter As DataTable
    Private mDienstleistungen As DataTable
    Private mBemerkungen As DataTable
    Private mTransporttyp As DataTable
    Private mDienstAuswahl As DataTable
    Private mReturnTable As DataTable
    Private mPartner As DataTable
    Private mSuchFahrzeuge As DataTable
    Private mSuchFahrzeugestellnummer As String = ""
    Private mSuchkennzeichen As String = ""
    Private mSuchReferenz As String = ""

    Private mRE As String
    Private mRG As String
    Private mEditFahrt As String = ""
    Private mEditRueckFahrt As String = ""
    Private mZuActiveFahrt As String = ""
    Private mZuRueckActiveFahrt As String = ""
    Private mReUseAdresses As Boolean = False
    Private mAbholadresse As DataTable
    Private mZieladresse As DataTable
    Private mZieladresseRueck As DataTable
    Private mLaender As DataTable
    Private mLaenderKunde As DataTable
    Private mProtokoll As DataTable
    Private mExpress As Boolean = False
    Private mExpressDays As Integer = 0
    Private mExpressDlNummer As String
    Private mSamstag As Boolean = False
    Private mSamstagDlNummer As String
    Private mSonstiges As Boolean = False
    Private mSonstigesDlNummer As String
    Private mEntfernungen As DataTable
    Private mVorholDlNummer As String
    Private mVorholung As Boolean = False

    Private mE_SUBRC As String
    Private mE_MESSAGE As String

    Public Property Fahrzeuge() As DataTable
        Get
            Return mFahrzeuge
        End Get
        Set(ByVal value As DataTable)
            mFahrzeuge = value
        End Set
    End Property

    Public Property Fahrten() As DataTable
        Get
            Return mFahrten
        End Get
        Set(ByVal value As DataTable)
            mFahrten = value
        End Set
    End Property

    Public Property Adressen() As DataTable
        Get
            Return mAdressen
        End Get
        Set(ByVal value As DataTable)
            mAdressen = value
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

    Public Property Bemerkungen() As DataTable
        Get
            Return mBemerkungen
        End Get
        Set(ByVal value As DataTable)
            mBemerkungen = value
        End Set
    End Property

    Public Property ReturnTable() As DataTable
        Get
            Return mReturnTable
        End Get
        Set(ByVal value As DataTable)
            mReturnTable = value
        End Set
    End Property

    Public Property Transporttyp() As DataTable
        Get
            Return mTransporttyp
        End Get
        Set(ByVal value As DataTable)
            mTransporttyp = value
        End Set
    End Property

    Public Property DienstAuswahl() As DataTable
        Get
            Return mDienstAuswahl
        End Get
        Set(ByVal value As DataTable)
            mDienstAuswahl = value
        End Set
    End Property

    Public Property Partner() As DataTable
        Get
            Return mPartner
        End Get
        Set(ByVal value As DataTable)
            mPartner = value
        End Set
    End Property


    Public Property Entfernungen() As DataTable
        Get
            Return mEntfernungen
        End Get
        Set(ByVal value As DataTable)
            mEntfernungen = value
        End Set
    End Property


    Public Property RG() As String
        Get
            Return mRG
        End Get
        Set(ByVal value As String)
            mRG = value
        End Set
    End Property

    Public Property RE() As String
        Get
            Return mRE
        End Get
        Set(ByVal value As String)
            mRE = value
        End Set
    End Property

    Public Property EditFahrt() As String
        Get
            Return mEditFahrt
        End Get
        Set(ByVal value As String)
            mEditFahrt = value
        End Set
    End Property

    Public Property EditRueckFahrt() As String
        Get
            Return mEditRueckFahrt
        End Get
        Set(ByVal value As String)
            mEditRueckFahrt = value
        End Set
    End Property

    Public Property ZuActiveFahrt() As String
        Get
            Return mZuActiveFahrt
        End Get
        Set(ByVal value As String)
            mZuActiveFahrt = value
        End Set
    End Property
    Public Property ZuRueckActiveFahrt() As String
        Get
            Return mZuRueckActiveFahrt
        End Get
        Set(ByVal value As String)
            mZuRueckActiveFahrt = value
        End Set
    End Property

    Public Property Abholadresse() As DataTable
        Get
            Return mAbholadresse
        End Get
        Set(ByVal value As DataTable)
            mAbholadresse = value
        End Set
    End Property

    Public Property Halter As DataTable
        Get
            Return mHalter
        End Get
        Set(value As DataTable)
            mHalter = value
        End Set
    End Property

    Public Property Zieladresse() As DataTable
        Get
            Return mZieladresse
        End Get
        Set(ByVal value As DataTable)
            mZieladresse = value
        End Set
    End Property

    Public Property ZieladresseRueck() As DataTable
        Get
            Return mZieladresseRueck
        End Get
        Set(ByVal value As DataTable)
            mZieladresseRueck = value
        End Set
    End Property

    Public Property Laender() As DataTable
        Get
            Return mLaender
        End Get
        Set(ByVal value As DataTable)
            mLaender = value
        End Set
    End Property

    Public Property LaenderKunde() As DataTable
        Get
            Return mLaenderKunde
        End Get
        Set(ByVal value As DataTable)
            mLaenderKunde = value
        End Set
    End Property

    Public Property SuchFahrzeuge() As DataTable
        Get
            Return mSuchFahrzeuge
        End Get
        Set(ByVal value As DataTable)
            mSuchFahrzeuge = value
        End Set
    End Property

    Public Property SuchFahrzeugestellnummer() As String
        Get
            Return mSuchFahrzeugestellnummer
        End Get
        Set(ByVal value As String)
            mSuchFahrzeugestellnummer = value
        End Set
    End Property

    Public Property Suchkennzeichen() As String
        Get
            Return mSuchkennzeichen
        End Get
        Set(ByVal value As String)
            mSuchkennzeichen = value
        End Set
    End Property

    Public Property SuchReferenz() As String
        Get
            Return mSuchReferenz
        End Get
        Set(ByVal value As String)
            mSuchReferenz = value
        End Set
    End Property

    Public Property ReUseAdresses() As Boolean
        Get
            Return mReUseAdresses
        End Get
        Set(ByVal value As Boolean)
            mReUseAdresses = value
        End Set
    End Property

    Public Property ProtokollArten() As DataTable
        Get
            Return mProtokoll
        End Get
        Set(ByVal value As DataTable)
            mProtokoll = value
        End Set
    End Property


    Public Property Express() As Boolean
        Get
            Return mExpress
        End Get
        Set(ByVal value As Boolean)
            mExpress = value
        End Set
    End Property

    Public Property ExpressDays() As Integer
        Get
            Return mExpressDays
        End Get
        Set(ByVal value As Integer)
            mExpressDays = value
        End Set
    End Property

    Public Property ExpressDlNummer() As String
        Get
            Return mExpressDlNummer
        End Get
        Set(ByVal value As String)
            mExpressDlNummer = value
        End Set
    End Property

    Public Property Samstag As Boolean
        Get
            Return mSamstag
        End Get
        Set(value As Boolean)
            mSamstag = value
        End Set
    End Property

    Public Property SamstagDlNummer() As String
        Get
            Return mSamstagDlNummer
        End Get
        Set(ByVal value As String)
            mSamstagDlNummer = value
        End Set
    End Property

    Public Property Sonstiges As Boolean
        Get
            Return mSonstiges
        End Get
        Set(value As Boolean)
            mSonstiges = value
        End Set
    End Property

    Public Property SonstigesDlNummer As String
        Get
            Return mSonstigesDlNummer
        End Get
        Set(value As String)
            mSonstigesDlNummer = value
        End Set
    End Property

    Public Property VorholDlNummer() As String
        Get
            Return mVorholDlNummer
        End Get
        Set(ByVal value As String)
            mVorholDlNummer = value
        End Set
    End Property
    Public Property Vorholung() As Boolean
        Get
            Return mVorholung
        End Get
        Set(ByVal value As Boolean)
            mVorholung = value
        End Set
    End Property

    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal AppID As String, ByVal SessionID As String, ByVal FileName As String)
        MyBase.New(objUser, objApp, FileName)
    End Sub




    Public Sub FillTables(ByVal objUser As CKG.Base.Kernel.Security.User, ByVal page As Web.UI.Page, ByVal Kundennr As String)
        m_strClassAndMethod = "Transfer.FillTables"
        m_strAppID = AppID
        m_strSessionID = SessionID

        m_intStatus = 0

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID = -1

            Try
                Dim proxy = DynSapProxy.getProxy("Z_DPM_READ_LV_001", m_objApp, m_objUser, page)
                proxy.setImportParameter("I_VWAG", "X")

                Dim tblAG = proxy.getImportTable("GT_IN_AG")
                Dim newRow = tblAG.NewRow
                newRow("AG") = Kundennr.ToString.PadLeft(10, "0"c)
                tblAG.Rows.Add(newRow)

                Dim tblProzess = proxy.getImportTable("GT_IN_PROZESS")
                newRow = tblProzess.NewRow
                newRow("SORT1") = 7
                tblProzess.Rows.Add(newRow)

                proxy.callBapi()

                Dim tmpTable = proxy.getExportTable("GT_OUT_DL")
                DienstAuswahl = tmpTable.Copy

                mSamstagDlNummer = "0"

                For Each row As DataRow In DienstAuswahl.Rows
                    Dim userf2_txt = CStr(row("USERF2_TXT")).ToUpper

                    Select Case userf2_txt
                        Case "EXPRESS"
                            mExpress = True
                            mExpressDlNummer = Mid(row("ASNUM").ToString, 9)
                            If IsNumeric(row("USERF1_NUM").ToString) Then
                                mExpressDays = CInt(row("USERF1_NUM"))
                            Else
                                mExpress = False
                                mExpressDlNummer = "0"
                            End If

                        Case "SAMSTAG"
                            mSamstag = True
                            mSamstagDlNummer = Mid(row("ASNUM").ToString, 9)

                        Case "VORHOL"
                            mVorholung = True
                            mVorholDlNummer = Mid(row("ASNUM").ToString, 9)

                        Case "SONSTIGES"
                            mSonstiges = True
                            mSonstigesDlNummer = Mid(row("ASNUM").ToString, 9)
                    End Select

                    row("ASNUM") = Mid(row("ASNUM").ToString, 9)
                Next

                Dim descCol = DienstAuswahl.Columns.Add("Description", GetType(System.String))
                descCol.DefaultValue = String.Empty
                'For Each dr As DataRow In DienstAuswahl.Rows
                '    dr("Description") = ""
                'Next
                DienstAuswahl.AcceptChanges()

                Dim langTextTable = proxy.getExportTable("GT_OUT_ESLL_LTXT")

                If langTextTable.Rows.Count > 0 Then
                    For Each dr As DataRow In DienstAuswahl.Rows
                        Dim text = ""

                        langTextTable.DefaultView.RowFilter = "SRVPOS = '" & dr("ASNUM").ToString.PadLeft(18, "0"c) & "'"

                        If langTextTable.DefaultView.Count > 0 Then
                            For i = 0 To langTextTable.DefaultView.Count - 1
                                text &= langTextTable.DefaultView.Item(i)("TDLINE").ToString & " "
                            Next
                        End If

                        dr("Description") = text
                    Next
                End If

                Dim tblAusgabe As New DataTable
                tblAusgabe.Columns.Add("ID", GetType(System.String))
                tblAusgabe.Columns.Add("Text", GetType(System.String))
                tblAusgabe.AcceptChanges()

                newRow = tblAusgabe.NewRow
                newRow("ID") = "00"
                newRow("Text") = "Transporttyp wählen"

                tblAusgabe.Rows.Add(newRow)

                For Each dr As DataRow In tmpTable.Rows
                    If String.IsNullOrEmpty(dr("ASNUM").ToString) = True AndAlso String.IsNullOrEmpty(dr("KTEXT1_H2").ToString) = True Then
                        newRow = tblAusgabe.NewRow

                        newRow("ID") = dr("EXTGROUP")
                        newRow("Text") = RTrim(dr("KTEXT1_H1").ToString)
                        tblAusgabe.Rows.Add(newRow)
                    End If
                Next
                tblAusgabe.AcceptChanges()

                Transporttyp = tblAusgabe

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

    Public Sub InitTables(ByVal objUser As CKG.Base.Kernel.Security.User, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Transfer.InitTables"
        m_strAppID = AppID
        m_strSessionID = SessionID

        m_intStatus = 0

        Dim TempTable As New DataTable

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy = DynSapProxy.getProxy("Z_UEB_CREATE_ORDER_01", m_objApp, m_objUser, page)

                Fahrzeuge = myProxy.getImportTable("GT_FZG")
                Fahrten = myProxy.getImportTable("GT_FAHRTEN")

                Adressen = myProxy.getImportTable("GT_ADRESSEN")
                Dienstleistungen = myProxy.getImportTable("GT_DIENSTLSTGN")
                Bemerkungen = myProxy.getImportTable("GT_BEM")

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


    Public Sub FillPartner(ByVal objUser As CKG.Base.Kernel.Security.User, ByVal page As Web.UI.Page, ByVal Kundennr As String)
        m_strClassAndMethod = "Transfer.FillPartner"
        m_strAppID = AppID
        m_strSessionID = SessionID

        m_intStatus = 0

        Dim TempTable As New DataTable

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_PARTNER_AUS_KNVP_LESEN", m_objApp, m_objUser, page)

                myProxy.setImportParameter("KUNNR", Kundennr.PadLeft(10, "0"c).ToString)


                If Not (objUser.Store = "AUTOHAUS" AndAlso objUser.Reference.Trim(" "c).Length > 0 AndAlso objUser.KUNNR = "261510") Then
                    If objUser.Reference.Trim.Length > 0 Then
                        myProxy.setImportParameter("EIKTO", objUser.Reference.Trim)
                    End If
                End If


                myProxy.callBapi()


                Partner = myProxy.getExportTable("AUSGABE")
                Partner.Columns.Add("DDLFELD", GetType(String), "NAME1 + ', ' + CITY1")

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


    Public Sub FillAdressen(ByVal objUser As CKG.Base.Kernel.Security.User, ByVal page As Web.UI.Page, _
                            ByVal Kennung As String, _
                            ByVal Name As String, _
                            ByVal PLZ As String, _
                            ByVal Ort As String, _
                            ByVal Kundennr As String)

        m_strClassAndMethod = "Transfer.FillAdressen"
        m_strAppID = AppID
        m_strSessionID = SessionID

        m_intStatus = 0

        Dim TempTable As New DataTable

        'Neu initialisieren
        Select Case Kennung
            Case "Abholadresse"
                Abholadresse = TempTable
            Case "Auslieferung"
                Zieladresse = TempTable
            Case "Halter"
                Halter = TempTable
            Case "Rückholung"
                ZieladresseRueck = TempTable
            Case "Z_WEB_UEB_LAND"
                LaenderKunde = TempTable
        End Select

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", m_objApp, m_objUser, page)

                If Name.Length > 0 AndAlso Name.Contains("*") = False Then
                    Name = "*" & Name & "*"
                End If

                myProxy.setImportParameter("I_KUNNR", Kundennr.PadLeft(10, "0"c).ToString)
                myProxy.setImportParameter("I_KENNUNG", Kennung.ToUpper)
                myProxy.setImportParameter("I_NAME1", Name)
                myProxy.setImportParameter("I_PSTLZ", PLZ)
                myProxy.setImportParameter("I_ORT01", Ort)

                myProxy.callBapi()

                TempTable = myProxy.getExportTable("GT_WEB")

                TempTable.Columns.Add("Adresse", GetType(System.String))
                TempTable.AcceptChanges()

                Dim i As Integer = 1
                For Each Row As DataRow In TempTable.Rows
                    Row("KUNNR") = i.ToString
                    i += 1
                    TempTable.AcceptChanges()
                Next

                Select Case Kennung
                    Case "Abholadresse"
                        Abholadresse = TempTable
                    Case "Auslieferung"
                        Zieladresse = TempTable
                    Case "Halter"
                        Halter = TempTable
                    Case "Rückholung"
                        ZieladresseRueck = TempTable
                    Case "Z_WEB_UEB_LAND"
                        LaenderKunde = TempTable
                End Select
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

    Public Sub FillFahrzeuge(ByVal objUser As CKG.Base.Kernel.Security.User, ByVal page As Web.UI.Page, ByVal Kundennr As String)
        m_strClassAndMethod = "Transfer.FillFahrzeuge"
        m_strAppID = AppID
        m_strSessionID = SessionID
        m_intStatus = 0
        Dim TempTable As New DataTable

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_EQUI_003", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", Kundennr.PadLeft(10, "0"c).ToString)
                myProxy.setImportParameter("I_EQTYP", "B")
                myProxy.setImportParameter("I_CHASSIS_NUM", mSuchFahrzeugestellnummer)
                myProxy.setImportParameter("I_LICENSE_NUM", mSuchkennzeichen)
                myProxy.setImportParameter("I_LIZNR", mSuchReferenz)
                myProxy.setImportParameter("I_TIDNR", "")
                myProxy.setImportParameter("I_ZZREFERENZ1", "")
                myProxy.setImportParameter("I_ZZREFERENZ2", "")
                myProxy.callBapi()


                SuchFahrzeuge = myProxy.getExportTable("GT_OUT")
                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")
                If IsNumeric(mE_SUBRC) Then
                    m_intStatus = CInt(mE_SUBRC)
                    If mE_MESSAGE.Length > 0 Then m_strMessage = mE_MESSAGE
                End If
                If SuchFahrzeuge.Rows.Count = 0 Then
                    m_intStatus = -9999
                    m_strMessage = "Fahrzeug nicht gefunden!"
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


    Public Sub FillEntfernungen(ByVal objUser As CKG.Base.Kernel.Security.User, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Transfer.FillEntfernungen"
        m_strAppID = AppID
        m_strSessionID = SessionID
        m_intStatus = 0
        Dim TempTable As New DataTable

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_KCL_GEOKODIERUNG_001", m_objApp, m_objUser, page)

                Dim myProxy As DynSapProxyObj
                Dim KM As String = ""
                Dim e As Integer


                e = Entfernungen.Rows.Count - 2


                For i As Integer = 0 To e

                    KM = " - "

                    myProxy = DynSapProxy.getProxy("Z_DPM_GEOKODIERUNG_001", m_objApp, m_objUser, page)

                    'Startort
                    myProxy.setImportParameter("I_AKTION", "E")
                    myProxy.setImportParameter("I_COUNTRY_ST", Entfernungen.Rows(i)("COUNTRY").ToString)
                    myProxy.setImportParameter("I_POST_CODE1_ST", Entfernungen.Rows(i)("POSTL_CODE").ToString)
                    myProxy.setImportParameter("I_CITY1_ST", Entfernungen.Rows(i)("CITY").ToString)
                    myProxy.setImportParameter("I_STREET_ST", Entfernungen.Rows(i)("STREET").ToString)

                    'Zielort
                    myProxy.setImportParameter("I_COUNTRY_ZI", Entfernungen.Rows(i + 1)("COUNTRY").ToString)
                    myProxy.setImportParameter("I_POST_CODE1_ZI", Entfernungen.Rows(i + 1)("POSTL_CODE").ToString)
                    myProxy.setImportParameter("I_CITY1_ZI", Entfernungen.Rows(i + 1)("CITY").ToString)
                    myProxy.setImportParameter("I_STREET_ZI", Entfernungen.Rows(i + 1)("STREET").ToString)

                    myProxy.callBapi()


                    If myProxy.getExportParameter("E_ENTFERNUNG") <> "" Then
                        KM = myProxy.getExportParameter("E_ENTFERNUNG")
                    End If

                    Entfernungen.Rows(i + 1)("KM") = KM

                Next



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

    Public Sub Save(ByVal objUser As CKG.Base.Kernel.Security.User, ByVal page As Web.UI.Page, ByVal kundennr As String, ByVal vorgangsnummer As String)
        m_strClassAndMethod = "Transfer.Save"
        m_strAppID = AppID
        m_strSessionID = SessionID

        m_intStatus = 0
        m_strMessage = String.Empty

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try
                Dim myProxy = DynSapProxy.getProxy("Z_UEB_CREATE_ORDER_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("AG", kundennr.PadLeft(10, "0"c))
                myProxy.setImportParameter("WEB_USER", objUser.UserName)
                If m_objUser.Email <> "" Then
                    myProxy.setImportParameter("EMAIL_WEB_USER", objUser.Email)
                End If
                If Not String.IsNullOrEmpty(m_objUser.Store) Then
                    myProxy.setImportParameter("INFO_ZUM_AG", m_objUser.Store)
                End If
                myProxy.setImportParameter("RE", RE)
                myProxy.setImportParameter("RG", RG)

                'System.Diagnostics.Trace.WriteLine("AG=" & kundennr.PadLeft(10, "0"c))
                'System.Diagnostics.Trace.WriteLine("WEB_USER=" & objUser.UserName)
                'System.Diagnostics.Trace.WriteLine("RE=" & RE)
                'System.Diagnostics.Trace.WriteLine("RG=" & RG)

                Dim ImpFahrten As DataTable = myProxy.getImportTable("GT_FAHRTEN")
                Dim ImpAdressen As DataTable = myProxy.getImportTable("GT_ADRESSEN")
                Dim ImpDienstleistungen As DataTable = myProxy.getImportTable("GT_DIENSTLSTGN")
                Dim ImpFahrzeuge As DataTable = myProxy.getImportTable("GT_FZG")
                Dim ImpBemerkungen As DataTable = myProxy.getImportTable("GT_BEM")
                Dim ImpProtokolle As DataTable = myProxy.getImportTable("GT_PROT")

                For Each Row As DataRow In Fahrten.Rows
                    Row("VORGANG") = vorgangsnummer

                    If String.IsNullOrEmpty(Row("VTIMEU").ToString) = False Then
                        Row("VTIMEU") = Row("VTIMEU").ToString.PadRight(6, "0"c)
                    End If
                Next
                Fahrten.AcceptChanges()

                'Fahrten
                For Each Row As DataRow In Fahrten.Rows
                    Dim NewRow = ImpFahrten.NewRow
                    For Each Col As DataColumn In Fahrten.Columns

                        If Col.ColumnName = "KENNZ_ZUS_FAHT" Then
                            NewRow(Col.ColumnName) = Row(Col.ColumnName).ToString.Replace(" ", "")
                        ElseIf Col.ColumnName = "REIHENFOLGE" Then
                            NewRow(Col.ColumnName) = Row(Col.ColumnName).ToString.PadLeft(6, "0"c)
                        Else
                            NewRow(Col.ColumnName) = DBNullToEmpty(Row, Col)
                        End If
                    Next
                    ImpFahrten.Rows.Add(NewRow)
                    ImpFahrten.AcceptChanges()
                Next
                'DumpTable(ImpFahrten)

                'Adressen
                For Each Row As DataRow In Adressen.Rows
                    Dim NewRow = ImpAdressen.NewRow
                    For Each Col As DataColumn In Adressen.Columns
                        NewRow(Col.ColumnName) = DBNullToEmpty(Row, Col)
                    Next
                    ImpAdressen.Rows.Add(NewRow)
                    ImpAdressen.AcceptChanges()
                Next
                'DumpTable(ImpAdressen)


                ' Web-User Daten als weitere Adresse mit FAHRT="AP" übergeben:
                Dim dataRow = ImpAdressen.NewRow
                For Each column As DataColumn In Adressen.Columns
                    dataRow(column.ColumnName) = DBNullToEmpty(dataRow, column)
                Next
                dataRow("FAHRT") = "AP"
                dataRow("PARTN_NUMB") = Kundennr.PadLeft(10, "0"c).ToString
                dataRow("NAME") = objUser.FirstName
                dataRow("NAME_2") = objUser.LastName
                dataRow("TELEPHONE") = objUser.Telephone
                dataRow("SMTP_ADDR") = objUser.Email
                ImpAdressen.Rows.Add(dataRow)
                ImpAdressen.AcceptChanges()


                'Dienstleistungen
                For Each Row As DataRow In Dienstleistungen.Rows
                    Dim NewRow = ImpDienstleistungen.NewRow
                    For Each Col As DataColumn In Dienstleistungen.Columns
                        NewRow(Col.ColumnName) = DBNullToEmpty(Row, Col)
                    Next

                    If String.IsNullOrEmpty(NewRow("MATNR").ToString) Then
                        NewRow("FLAG_TEXT") = "X"
                    End If

                    ImpDienstleistungen.Rows.Add(NewRow)
                    ImpDienstleistungen.AcceptChanges()
                Next
                'DumpTable(ImpDienstleistungen)

                'Fahrzeuge
                For Each Row As DataRow In Fahrzeuge.Rows
                    Dim NewRow = ImpFahrzeuge.NewRow
                    For Each Col As DataColumn In Fahrzeuge.Columns
                        Select Case Col.ColumnName
                            Case "ZULGE", "ZUL_BEI_CK_DAD"
                                If Row(Col.ColumnName).ToString = "N" Then
                                    NewRow(Col.ColumnName) = ""
                                Else
                                    NewRow(Col.ColumnName) = DBNullToEmpty(Row, Col)
                                End If
                            Case "ZZKENN"
                                NewRow(Col.ColumnName) = Row(Col.ColumnName).ToString.Replace(" ", "")
                            Case "FZGART"
                                NewRow(Col.ColumnName) = Row(Col.ColumnName)
                            Case Else
                                NewRow(Col.ColumnName) = DBNullToEmpty(Row, Col)
                        End Select
                    Next
                    ImpFahrzeuge.Rows.Add(NewRow)
                    ImpFahrzeuge.AcceptChanges()
                Next
                'DumpTable(ImpFahrzeuge)

                'Bemerkungen
                For Each Row As DataRow In Bemerkungen.Rows
                    Dim NewRow = ImpBemerkungen.NewRow
                    For Each Col As DataColumn In Bemerkungen.Columns
                        NewRow(Col.ColumnName) = DBNullToEmpty(Row, Col)
                    Next
                    ImpBemerkungen.Rows.Add(NewRow)
                    ImpBemerkungen.AcceptChanges()
                Next
                'DumpTable(ImpBemerkungen)

                'Protokolle
                If ProtokollArten IsNot Nothing Then
                    For Each Row As DataRow In ProtokollArten.Rows
                        If Not String.IsNullOrEmpty(CStr(Row("Tempfilename"))) Then
                            Dim NewRow = ImpProtokolle.NewRow
                            For Each Col As DataColumn In ImpProtokolle.Columns
                                NewRow(Col.ColumnName) = DBNullToEmpty(Row, Col)
                            Next
                            ImpProtokolle.Rows.Add(NewRow)
                            ImpProtokolle.AcceptChanges()
                        End If
                    Next
                End If
                
                'DumpTable(ImpProtokolle)

                myProxy.callBapi()

                'DumpTable(myProxy.Import)
                'DumpTable(myProxy.Export)

                ReturnTable = myProxy.getExportTable("GT_RET")
                'DumpTable(ReturnTable)

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            Finally
                m_blnGestartet = False
            End Try
        End If


    End Sub

    Private Function DBNullToEmpty(row As DataRow, col As DataColumn) As Object
        Dim value = row(col.ColumnName)
        If DBNull.Value.Equals(value) AndAlso GetType(String).Equals(col.DataType) Then Return String.Empty
        Return value
    End Function

    Private Sub DumpTable(table As DataTable)
        If table Is Nothing Then Return

        System.Diagnostics.Trace.WriteLine(table.TableName)

        For Each col As DataColumn In table.Columns
            PrintCell(col.ColumnName)
        Next
        System.Diagnostics.Trace.Write(Environment.NewLine)

        Dim cc = table.Columns.Count
        For Each row As DataRow In table.Rows
            For c = 0 To cc - 1
                PrintCell(row(c))
            Next
            System.Diagnostics.Trace.Write(Environment.NewLine)
        Next
        System.Diagnostics.Trace.WriteLine("------------------------------")
    End Sub

    Private Sub PrintCell(value As Object)
        Const length As Integer = 20

        If DBNull.Value.Equals(value) Then
            System.Diagnostics.Trace.Write("<DBNull>".PadRight(length).Substring(0, length))
        ElseIf value Is Nothing Then
            System.Diagnostics.Trace.Write("<Null>".PadRight(length).Substring(0, length))
        Else
            System.Diagnostics.Trace.Write(value.ToString().PadRight(length).Substring(0, length))
        End If
        System.Diagnostics.Trace.Write("  ")
    End Sub

    Public Function GetVorgangsnummer(ByVal objUser As CKG.Base.Kernel.Security.User, ByVal page As Web.UI.Page) As String
        m_strClassAndMethod = "Transfer.GetVorgangsnummer"
        m_strAppID = AppID
        m_strSessionID = SessionID

        m_intStatus = 0

        Dim Vorgangsnummer As String = ""

        Dim intID As Int32 = -1

        Try
            Dim proxy = DynSapProxy.getProxy("Z_UEB_NEXT_NUMBER_VORGANG_01", m_objApp, m_objUser, page)
            proxy.callBapi()

            Vorgangsnummer = proxy.getExportParameter("E_VORGANG")

        Catch ex As Exception
            m_intStatus = -9999
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                'ToDo ErrMessage
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            Return ""
        End Try

        Return Vorgangsnummer
    End Function


    Public Sub getLaender(ByVal page As Web.UI.Page)
        m_intStatus = 0
        Try
            Dim proxy = DynSapProxy.getProxy("Z_M_Land_Plz_001", m_objApp, m_objUser, page)

            proxy.callBapi()

            mLaender = proxy.getExportTable("GT_WEB")
            mLaender.Columns.Add("Beschreibung", System.Type.GetType("System.String"))
            mLaender.Columns.Add("FullDesc", System.Type.GetType("System.String"))
            For Each row As DataRow In mLaender.Rows
                If CInt(row("LNPLZ")) > 0 Then
                    row("Beschreibung") = CStr(row("Landx")) & " (" & CStr(CInt(row("LNPLZ"))) & ")"
                Else
                    row("Beschreibung") = CStr(row("Landx"))
                End If
                row("FullDesc") = CStr(row("Land1")) & " " & CStr(row("Beschreibung"))
            Next
            mLaender.AcceptChanges()

            If LaenderKunde.Rows.Count > 0 Then
                Dim TempTable = mLaender.Clone

                For Each Row As DataRow In LaenderKunde.Rows
                    Dim dr = mLaender.Select("LAND1 = '" & Row("POS_KURZTEXT").ToString & "'")

                    If dr.Length > 0 Then
                        Dim NewRow = TempTable.NewRow

                        NewRow(0) = dr(0)(0).ToString
                        NewRow(1) = dr(0)(1).ToString
                        NewRow(2) = dr(0)(2).ToString
                        NewRow(3) = dr(0)(3).ToString
                        NewRow(4) = dr(0)(4).ToString
                        NewRow(5) = dr(0)(5).ToString

                        TempTable.Rows.Add(NewRow)
                    End If
                Next

                TempTable.AcceptChanges()
                mLaender = TempTable
            End If
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

    Public Sub getProtokollarten(ByVal objUser As CKG.Base.Kernel.Security.User, ByVal page As Web.UI.Page, ByVal Kundennr As String)
        m_intStatus = 0
        Try

            Dim proxy = DynSapProxy.getProxy("Z_DPM_READ_TAB_PROT_01", m_objApp, m_objUser, page)
            proxy.setImportParameter("I_KUNNR_AG", Kundennr.PadLeft(10, "0"c).ToString)

            proxy.callBapi()

            Dim gt_out = proxy.getExportTable("GT_OUT")

            Dim webUploadRows = gt_out.Select("WEB_UPLOAD = 'X'")
            mProtokoll = gt_out.Clone
            mProtokoll.Columns.Add("ID", System.Type.GetType("System.String"))
            mProtokoll.Columns.Add("Tempfilename", System.Type.GetType("System.String"))
            mProtokoll.Columns.Add("Filename", System.Type.GetType("System.String"))
            mProtokoll.Columns.Add("Fahrt", System.Type.GetType("System.String"))

            Dim i = 1
            For Each r As DataRow In webUploadRows
                Dim newRow = mProtokoll.NewRow
                newRow("ID") = i
                newRow("Tempfilename") = ""
                newRow("Filename") = ""
                newRow("Fahrt") = ""
                newRow("ZZKUNNR") = r("ZZKUNNR")
                newRow("ZZKATEGORIE") = r("ZZKATEGORIE")
                newRow("ZZPROTOKOLLART") = r("ZZPROTOKOLLART")
                newRow("WEB_UPLOAD") = r("WEB_UPLOAD")
                i += 1
                mProtokoll.Rows.Add(newRow)
            Next
            mProtokoll.AcceptChanges()

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Es konnten keine Protokollarten ermittelt werden."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
        End Try
    End Sub


    Private Function GetKundennummer() As String

        Dim Kundennummer As String = m_objUser.KUNNR

        If m_objUser.Store = "AUTOHAUS" AndAlso m_objUser.Reference.Trim(" "c).Length > 0 AndAlso m_objUser.KUNNR = "261510" Then
            Kundennummer = m_objUser.Reference
        End If

        Return Kundennummer

    End Function


    Public Function CheckFahrzeugStandort(ByVal page As Web.UI.Page, ByVal CarportAutohausAdresse As String, ByVal Fahrgestellnummer As String, ByRef returnMessage As String, ByRef returnMessageErfolg As String) As String
        Dim Kundennr = GetKundennummer()
        Kundennr = Kundennr.PadLeft(10, "0"c).ToString

        m_intStatus = 0

        'check ob geprüft werden soll
        Dim CheckTableRows As Integer = 0
        Try
            Dim myProxyCheck As DynSapProxyObj = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", m_objApp, m_objUser, page)
            Dim KennungCheck As String = "CARPORTPRUEFUNGEN"
            myProxyCheck.setImportParameter("I_KUNNR", Kundennr.PadLeft(10, "0"c).ToString)
            myProxyCheck.setImportParameter("I_KENNUNG", KennungCheck.ToUpper)
            myProxyCheck.callBapi()
            Dim CheckTable = myProxyCheck.getExportTable("GT_WEB")
            CheckTableRows = CheckTable.Rows.Count
        Catch
        End Try

        If CheckTableRows > 0 Then





            returnMessage = "Fahrzeug nicht avisiert und auch nicht am Standort eingetroffen!"
            Dim m_Daten = New DataTable
            Dim m_Daten_gefiltert() As DataRow
            Dim m_Daten2 = New DataTable
            Dim m_Daten3 = New DataTable
            Dim m_Daten4 = New DataTable
            Dim m_Daten5 = New DataTable
            Dim m_Daten6 = New DataTable



            Dim CarportAuto As String

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_COIH_READ_AUF_POS_02", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", Kundennr)
                myProxy.setImportParameter("I_AUART", "")
                myProxy.setImportParameter("I_PLNAL", "22")
                myProxy.setImportParameter("I_ETEXT1", "")
                myProxy.setImportParameter("I_TERMIN_ANFANG", "")
                myProxy.setImportParameter("I_TERMIN_ENDE", "")
                myProxy.setImportParameter("I_ANLZU", "")
                myProxy.setImportParameter("I_ETEXT4", "")

                myProxy.callBapi()

                m_Daten = New DataTable


                m_Daten = myProxy.getExportTable("GT_VORG")
                m_Daten2 = myProxy.getExportTable("GT_AUF")
                m_Daten3 = myProxy.getExportTable("GT_LSTART")
                m_Daten4 = myProxy.getExportTable("GT_VORG")
                m_Daten5 = myProxy.getExportTable("GT_LEISTUNGEN")
                m_Daten6 = myProxy.getExportTable("GT_TEXTE")


                'Fahrgestellnummer filtern und Leistungsart angeben
                m_Daten_gefiltert = m_Daten.Select("LSTAR = 'UAFZCA' and CHASSIS_NUM = '" & Fahrgestellnummer & "'")

                'Vergleich von Carport mit ETEXT1 und CHASSIS_NUM mit Fahrgestellnummer  WDB12222222288991
                For Each xrow As DataRow In m_Daten_gefiltert
                    CarportAuto = xrow("ETEXT1").ToString()

                    If returnMessage = "Fahrzeug nicht avisiert und auch nicht am Standort eingetroffen!" Then
                        returnMessage = ""
                    End If
                    If xrow("ETEXT1").ToString() = CarportAutohausAdresse Then
                        'Carport ist übereinstimmend
                        'returnMessage = "Fahrzeug ist am Standort " & Carport & " bekannt."
                        returnMessage = ""
                    Else
                        'Carport ist nicht übereinstimmend
                        'Adresse des Standortes finden
                        Dim myProxy2 As DynSapProxyObj = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", m_objApp, m_objUser, page)
                        Dim Kennung As String = "Abholadresse"
                        myProxy2.setImportParameter("I_KUNNR", Kundennr.PadLeft(10, "0"c).ToString)
                        myProxy2.setImportParameter("I_KENNUNG", Kennung.ToUpper)
                        myProxy2.callBapi()
                        Dim TempTable2 = myProxy2.getExportTable("GT_WEB")

                        For Each StandortRow As DataRow In TempTable2.Rows
                            If StandortRow("POS_TEXT").ToString() = CarportAuto Then
                                returnMessage += "Fahrzeug ist an diesem Standort nicht bekannt. Fahrzeug ist am Standort:<br>"
                                returnMessage += StandortRow("NAME1").ToString() & StandortRow("NAME2").ToString() & "<br>"
                                returnMessage += StandortRow("STRAS").ToString() & "<br>"
                                returnMessage += StandortRow("PSTLZ").ToString() & " " & StandortRow("ORT01").ToString() & "<br>"
                                returnMessage += "Telefon: " & StandortRow("TELNR").ToString() & "<br>"
                            End If
                        Next
                    End If
                Next

                If returnMessage = "" Then
                    'If 1 = 1 Then
                    'Eingang am Carport prüfen

                    Dim myProxy3 As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_COIH_READ_AUFTRAG_001", m_objApp, m_objUser, page)

                    Dim ImportParameter As DataTable = myProxy3.getImportTable("GS_IN")

                    'Parameter füllen
                    Dim dr As DataRow = ImportParameter.NewRow
                    dr("KUNNR_AG") = Kundennr.PadLeft(10, "0"c).ToString
                    dr.SetField("VAGRP", "4")
                    dr.SetField("PLNAL", "22")
                    dr.SetField("STATU", "3")
                    dr.SetField("AUART", "ZS02")
                    dr.SetField("AUTYP", "30")
                    dr.SetField("BUKRS", "1510")
                    dr.SetField("WERKS", "1510")
                    ImportParameter.Rows.Add(dr)
                    ImportParameter.AcceptChanges()

                    'Werte an Tabellen übergeben
                    Dim ImportTable1 As DataTable = myProxy3.getImportTable("GT_IN_LARNT")
                    Dim dr2 As DataRow = ImportTable1.NewRow
                    dr2(0) = "UAFZEI"
                    ImportTable1.Rows.Add(dr2)
                    Dim dr3 As DataRow = ImportTable1.NewRow
                    dr3(0) = "UAFZBE"
                    ImportTable1.Rows.Add(dr3)
                    ImportTable1.AcceptChanges()

                    If Fahrgestellnummer.Length > 0 Then
                        Dim IdentsTable As DataTable = myProxy3.getImportTable("GT_IN_IDENTS")
                        Dim drIdents As DataRow = IdentsTable.NewRow
                        drIdents("CHASSIS_NUM") = Fahrgestellnummer
                        IdentsTable.Rows.Add(drIdents)
                        IdentsTable.AcceptChanges()
                    End If


                    myProxy3.callBapi()

                    Dim TempTable2 = myProxy3.getExportTable("GT_AUF_RUECK")
                    Dim ist_vorhanden As Boolean = False
                    Dim ist_bereit As Boolean = False

                    If TempTable2.Rows.Count = 0 Then
                        returnMessage += "Fahrzeug ist am Standort noch nicht eingegangen!"
                    Else
                        For Each Row2 As DataRow In TempTable2.Rows
                            If Row2("LEARR").ToString() = "UAFZEI" And Row2("AUERU").ToString() = "X" Then
                                ist_vorhanden = True
                            End If
                        Next
                        For Each Row2 As DataRow In TempTable2.Rows
                            If Row2("LEARR").ToString() = "UAFZBE" And Row2("AUERU").ToString() = "X" Then
                                ist_bereit = True
                            End If
                        Next
                    End If
                    If ist_vorhanden Then
                        returnMessageErfolg += "Fahrzeug ist am Standort eingegangen!"
                    Else
                        returnMessage += "Fahrzeug ist am Standort noch nicht eingegangen!"
                    End If

                    If ist_bereit Then
                        returnMessageErfolg += "Fahrzeug ist am Standort Bereit gemeldet. "
                    Else
                        returnMessage += "Fahrzeug ist am Standort nicht Bereit gemeldet. "
                    End If


                End If

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Es konnten keine Protokollarten ermittelt werden."
                        m_intStatus = -1118
                    Case Else
                        m_strMessage = "Unbekannter Fehler."
                        m_intStatus = -9999
                End Select


                m_Daten = Nothing
            Finally
                'm_blnGestartet = False
            End Try


            Return returnMessage

        End If
    End Function

End Class
