Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class Transfer
    Inherits DatenimportBase

    Private mFahrzeuge As DataTable
    Private mFahrten As DataTable
    Private mAdressen As DataTable
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
    Private mSamstagDlNummer As String
    Private mEntfernungen As DataTable
    Private mVorholDlNummer As String
    Private mVorholung As Boolean = False

    Private mE_SUBRC As String
    Private mE_MESSAGE As String

#Region "Properties"

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

    Public Property SamstagDlNummer() As String
        Get
            Return mSamstagDlNummer
        End Get
        Set(ByVal value As String)
            mSamstagDlNummer = value
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

#End Region


#Region "Constructure"

    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal AppID As String, ByVal SessionID As String, ByVal FileName As String)
        MyBase.New(objUser, objApp, FileName)
    End Sub

#End Region


    Public Sub FillTables(ByVal objUser As CKG.Base.Kernel.Security.User, ByVal page As Web.UI.Page, ByVal Kundennr As String)

        m_strClassAndMethod = "Transfer.FillTables"
        m_strAppID = AppID
        m_strSessionID = SessionID

        Dim TempTable As New DataTable

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_LV_001", m_objApp, m_objUser, page)


                myProxy.setImportParameter("I_VWAG", "X")

                Dim tblAG As DataTable
                Dim tblProzess As DataTable
                Dim RowAG As DataRow
                Dim RowProzess As DataRow

                tblAG = myProxy.getImportTable("GT_IN_AG")
                tblProzess = myProxy.getImportTable("GT_IN_PROZESS")

                RowAG = tblAG.NewRow
                RowAG("AG") = Kundennr.ToString.PadLeft(10, "0"c)

                tblAG.Rows.Add(RowAG)

                RowProzess = tblProzess.NewRow
                RowProzess("SORT1") = 7
                tblProzess.Rows.Add(RowProzess)

                myProxy.callBapi()


                TempTable = myProxy.getExportTable("GT_OUT_DL")


                DienstAuswahl = TempTable.Copy

                mSamstagDlNummer = "0"

                For Each Row As DataRow In DienstAuswahl.Rows
                    If Row("USERF2_TXT").ToString = "EXPRESS" Then
                        mExpress = True
                        mExpressDlNummer = Mid(Row("ASNUM").ToString, 9)
                        If IsNumeric(Row("USERF1_NUM").ToString) Then
                            mExpressDays = CInt(Row("USERF1_NUM"))
                        Else
                            mExpress = False
                            mExpressDlNummer = "0"
                        End If
                    End If

                    If Row("USERF2_TXT").ToString.ToUpper = "SAMSTAG" Then
                        mSamstagDlNummer = Mid(Row("ASNUM").ToString, 9)
                    End If

                    If Row("USERF2_TXT").ToString.ToUpper = "VORHOL" Then
                        mVorholung = True
                        mVorholDlNummer = Mid(Row("ASNUM").ToString, 9)
                    End If


                    Row("ASNUM") = Mid(Row("ASNUM").ToString, 9)

                    DienstAuswahl.AcceptChanges()
                Next


                DienstAuswahl.Columns.Add("Description", GetType(System.String))
                DienstAuswahl.Columns("Description").DefaultValue = String.Empty

                For Each dr As DataRow In DienstAuswahl.Rows
                    dr("Description") = ""
                Next

                Dim LangTextTable As DataTable = myProxy.getExportTable("GT_OUT_ESLL_LTXT")

                If LangTextTable.Rows.Count > 0 Then

                    Dim strText As String = ""

                    For Each dr As DataRow In DienstAuswahl.Rows

                        strText = ""

                        LangTextTable.DefaultView.RowFilter = "SRVPOS = '" & dr("ASNUM").ToString.PadLeft(18, "0"c) & "'"

                        If LangTextTable.DefaultView.Count > 0 Then

                            For i = 0 To LangTextTable.DefaultView.Count - 1
                                strText &= LangTextTable.DefaultView.Item(i)("TDLINE").ToString & " "
                            Next

                        End If

                        dr("Description") = strText

                    Next

                End If

                Dim tblAusgabe As New DataTable

                tblAusgabe.Columns.Add("ID", GetType(System.String))
                tblAusgabe.Columns.Add("Text", GetType(System.String))

                tblAusgabe.AcceptChanges()

                Dim AusgabeRow As DataRow


                AusgabeRow = tblAusgabe.NewRow

                AusgabeRow("ID") = "00"
                AusgabeRow("Text") = "Transporttyp wählen"

                tblAusgabe.Rows.Add(AusgabeRow)


                For Each dr As DataRow In TempTable.Rows

                    If String.IsNullOrEmpty(dr("ASNUM").ToString) = True AndAlso String.IsNullOrEmpty(dr("KTEXT1_H2").ToString) = True Then
                        AusgabeRow = tblAusgabe.NewRow

                        AusgabeRow("ID") = dr("EXTGROUP")
                        AusgabeRow("Text") = RTrim(dr("KTEXT1_H1").ToString)
                        tblAusgabe.Rows.Add(AusgabeRow)

                    End If

                Next

                tblAusgabe.AcceptChanges()

                Transporttyp = tblAusgabe

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
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

        Dim TempTable As New DataTable

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_UEB_CREATE_ORDER_01", m_objApp, m_objUser, page)

                Fahrzeuge = myProxy.getImportTable("GT_FZG")
                Fahrten = myProxy.getImportTable("GT_FAHRTEN")

                Adressen = myProxy.getImportTable("GT_ADRESSEN")
                Dienstleistungen = myProxy.getImportTable("GT_DIENSTLSTGN")
                Bemerkungen = myProxy.getImportTable("GT_BEM")

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
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
            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
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

        Dim TempTable As New DataTable

        'Neu initialisieren
        Select Case Kennung

            Case "Abholadresse"
                Abholadresse = TempTable
            Case "Auslieferung"
                Zieladresse = TempTable
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
                    Case "Rückholung"
                        ZieladresseRueck = TempTable
                    Case "Z_WEB_UEB_LAND"
                        LaenderKunde = TempTable
                End Select

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
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
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Save(ByVal objUser As CKG.Base.Kernel.Security.User, ByVal page As Web.UI.Page, ByVal Kundennr As String)

        m_strClassAndMethod = "Transfer.Save"
        m_strAppID = AppID
        m_strSessionID = SessionID

        Dim TempTable As New DataTable
        Dim Vorgangsnummer As String

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_UEB_CREATE_ORDER_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("AG", Kundennr.PadLeft(10, "0"c))
                myProxy.setImportParameter("WEB_USER", objUser.UserName)
                If Not String.IsNullOrEmpty(m_objUser.Email) Then
                    myProxy.setImportParameter("EMAIL_WEB_USER", m_objUser.Email)
                End If
                If Not String.IsNullOrEmpty(m_objUser.Store) Then
                    myProxy.setImportParameter("INFO_ZUM_AG", m_objUser.Store)
                End If
                myProxy.setImportParameter("RE", RE)
                myProxy.setImportParameter("RG", RG)

                Dim ImpFahrten As DataTable = myProxy.getImportTable("GT_FAHRTEN")
                Dim ImpAdressen As DataTable = myProxy.getImportTable("GT_ADRESSEN")
                Dim ImpDienstleistungen As DataTable = myProxy.getImportTable("GT_DIENSTLSTGN")
                Dim ImpFahrzeuge As DataTable = myProxy.getImportTable("GT_FZG")
                Dim ImpBemerkungen As DataTable = myProxy.getImportTable("GT_BEM")
                Dim ImpProtokolle As DataTable = myProxy.getImportTable("GT_PROT")


                Vorgangsnummer = GetVorgangsnummer(objUser, page)

                For Each Row As DataRow In Fahrten.Rows

                    Row("VORGANG") = Vorgangsnummer

                    If String.IsNullOrEmpty(Row("VTIMEU").ToString) = False Then
                        Row("VTIMEU") = Row("VTIMEU").ToString.PadRight(6, "0"c)
                    End If

                Next
                Fahrten.AcceptChanges()

                Dim NewRow As DataRow

                'Fahrten
                For Each Row As DataRow In Fahrten.Rows
                    NewRow = ImpFahrten.NewRow


                    For Each Col As DataColumn In Fahrten.Columns

                        If Col.ColumnName = "TRANSPORTTYP" Then
                            If Row("FAHRT").ToString <> "0" Then
                                NewRow(Col.ColumnName) = Transporttyp.Select("ID = '" & Row("TRANSPORTTYP").ToString & "'")(0)("TEXT")

                            End If

                        Else
                            NewRow(Col.ColumnName) = Row(Col.ColumnName)
                        End If
                        If Col.ColumnName = "KENNZ_ZUS_FAHT" Then
                            NewRow(Col.ColumnName) = Row(Col.ColumnName).ToString.Replace(" ", "")
                        End If

                        If Col.ColumnName = "TRANSPORTTYPNR" Then
                            NewRow("TRANSPORTTYPNR") = Row("TRANSPORTTYP")
                        End If

                    Next
                    ImpFahrten.Rows.Add(NewRow)
                    ImpFahrten.AcceptChanges()
                Next

                'Adressen
                For Each Row As DataRow In Adressen.Rows
                    NewRow = ImpAdressen.NewRow
                    For Each Col As DataColumn In Adressen.Columns
                        NewRow(Col.ColumnName) = Row(Col.ColumnName)
                    Next
                    ImpAdressen.Rows.Add(NewRow)
                    ImpAdressen.AcceptChanges()
                Next


                ' Web-User Daten als weitere Adresse mit FAHRT="AP" übergeben:
                Dim dataRow = ImpAdressen.NewRow
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
                    NewRow = ImpDienstleistungen.NewRow
                    For Each Col As DataColumn In Dienstleistungen.Columns

                        NewRow(Col.ColumnName) = Row(Col.ColumnName)
                    Next

                    If String.IsNullOrEmpty(NewRow("MATNR").ToString) = True Then
                        NewRow("FLAG_TEXT") = "X"
                    End If

                    ImpDienstleistungen.Rows.Add(NewRow)
                    ImpDienstleistungen.AcceptChanges()
                Next

                'Fahrzeuge
                For Each Row As DataRow In Fahrzeuge.Rows
                    NewRow = ImpFahrzeuge.NewRow
                    For Each Col As DataColumn In Fahrzeuge.Columns
                        If Col.ColumnName = "ZULGE" OrElse Col.ColumnName = "ZUL_BEI_CK_DAD" Then
                            If Row(Col.ColumnName).ToString = "N" Then
                                NewRow(Col.ColumnName) = ""
                            Else
                                NewRow(Col.ColumnName) = Row(Col.ColumnName)
                            End If
                        Else
                            NewRow(Col.ColumnName) = Row(Col.ColumnName)
                        End If
                        If Col.ColumnName = "ZZKENN" Then
                            NewRow(Col.ColumnName) = Row(Col.ColumnName).ToString.Replace(" ", "")
                        End If

                        If Col.ColumnName = "FZGART" Then
                            NewRow(Col.ColumnName) = Row(Col.ColumnName).ToString
                        End If


                    Next
                    ImpFahrzeuge.Rows.Add(NewRow)
                    ImpFahrzeuge.AcceptChanges()
                Next

                'Bemerkungen
                For Each Row As DataRow In Bemerkungen.Rows
                    NewRow = ImpBemerkungen.NewRow
                    For Each Col As DataColumn In Bemerkungen.Columns
                        NewRow(Col.ColumnName) = Row(Col.ColumnName)
                    Next
                    ImpBemerkungen.Rows.Add(NewRow)
                    ImpBemerkungen.AcceptChanges()
                Next

                'Protokolle
                If Not mProtokoll Is Nothing Then
                    If mProtokoll.Rows.Count > 0 Then
                        For Each Row As DataRow In mProtokoll.Rows
                            If Row("Filename").ToString.Length > 0 Then
                                NewRow = ImpProtokolle.NewRow
                                NewRow("FAHRT") = Row("FAHRT")
                                NewRow("ZZKATEGORIE") = Row("ZZKATEGORIE")
                                NewRow("ZZPROTOKOLLART") = Row("ZZPROTOKOLLART")
                                ImpProtokolle.Rows.Add(NewRow)
                                ImpProtokolle.AcceptChanges()
                            End If

                        Next

                    End If
                End If

                myProxy.callBapi()
                ReturnTable = myProxy.getExportTable("GT_RET")

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

    Public Function GetVorgangsnummer(ByVal objUser As CKG.Base.Kernel.Security.User, ByVal page As Web.UI.Page) As String
        m_strClassAndMethod = "Transfer.GetVorgangsnummer"
        m_strAppID = AppID
        m_strSessionID = SessionID
        Dim Vorgangsnummer As String = ""

        Dim intID As Int32 = -1

        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_UEB_NEXT_NUMBER_VORGANG_01", m_objApp, m_objUser, page)

            myProxy.callBapi()

            Vorgangsnummer = myProxy.getExportParameter("E_VORGANG")

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

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Land_Plz_001", m_objApp, m_objUser, page)

            myProxy.callBapi()

            mLaender = myProxy.getExportTable("GT_WEB")


            mLaender.Columns.Add("Beschreibung", System.Type.GetType("System.String"))
            mLaender.Columns.Add("FullDesc", System.Type.GetType("System.String"))
            Dim rowTemp As DataRow
            For Each rowTemp In mLaender.Rows
                If CInt(rowTemp("LNPLZ")) > 0 Then
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx")) & " (" & CStr(CInt(rowTemp("LNPLZ"))) & ")"
                Else
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx"))
                End If
                rowTemp("FullDesc") = CStr(rowTemp("Land1")) & " " & CStr(rowTemp("Beschreibung"))

            Next

            mLaender.AcceptChanges()

            If LaenderKunde.Rows.Count > 0 Then

                Dim TempTable As DataTable = mLaender.Clone
                Dim NewRow As DataRow
                Dim dr As DataRow()

                For Each Row As DataRow In LaenderKunde.Rows

                    dr = mLaender.Select("LAND1 = '" & Row("POS_KURZTEXT").ToString & "'")

                    If dr.Length > 0 Then

                        NewRow = TempTable.NewRow

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

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_TAB_PROT_01", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR_AG", Kundennr.PadLeft(10, "0"c).ToString)

            myProxy.callBapi()
            Dim ReturnTable As DataTable

            ReturnTable = myProxy.getExportTable("GT_OUT")

            Dim ReturnRow() As DataRow

            ReturnRow = ReturnTable.Select("WEB_UPLOAD = 'X'")
            mProtokoll = New DataTable

            mProtokoll = ReturnTable.Clone

            mProtokoll.Columns.Add("ID", System.Type.GetType("System.String"))
            mProtokoll.Columns.Add("Filename", System.Type.GetType("System.String"))
            mProtokoll.Columns.Add("Filepath", System.Type.GetType("System.String"))
            mProtokoll.Columns.Add("Fahrt", System.Type.GetType("System.String"))

            Dim i As Integer = 1
            Dim NewRow As DataRow
            For Each Row As DataRow In ReturnRow
                NewRow = mProtokoll.NewRow
                NewRow("ID") = i
                NewRow("Filename") = ""
                NewRow("Filepath") = ""
                NewRow("Fahrt") = ""
                NewRow("ZZKUNNR") = Row("ZZKUNNR")
                NewRow("ZZKATEGORIE") = Row("ZZKATEGORIE")
                NewRow("ZZPROTOKOLLART") = Row("ZZPROTOKOLLART")
                NewRow("WEB_UPLOAD") = Row("WEB_UPLOAD")
                i += 1
                mProtokoll.Rows.Add(NewRow)
                mProtokoll.AcceptChanges()
            Next
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


End Class
