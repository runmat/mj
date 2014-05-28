Option Explicit On
Option Infer On
Option Strict On

Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel.Common

Public Class Briefanforderung

    Inherits Base.Business.BankBase

#Region " Declarations"
   
    Private mName1 As String = ""
    Private mName2 As String = ""
    Private mName3 As String = ""
    Private mCity As String = ""
    Private mPostcode As String = ""
    Private mStreet As String = ""
    Private mHouseNum As String = ""
    Private mLaenderKuerzel As String = ""

    Private mSucheLeasingvertragsnummer As String = ""
    Private mSucheKennzeichen As String = ""
    Private mSucheSuchname As String = ""
    Private mSucheFahrgestellnummer As String = ""


    Private mVersandAdressValue As String = ""
    Private mMaterialNummer As String = "" 'Versandart
    Private mTextZumMaterial As String = "" 'Beschreibungs Text der Versandart
    Private mVersandEmpfaengerArt As String = ""
    Private mGeschaeftsstellenValue As String = ""
    Private mPartnerAdressValue As String = ""
    Private mVersandgrund As String = ""
    Private mVersandgrundZusatzText As String = ""
    Private mEigentumsvorbehalt As String = ""
    Private mBemerkung As String = ""

    Private mFahrzeuge As DataTable
    Private mVersandAdressen As New DataTable
    Private mPartnerAdressen As New DataTable
    Private mAbrufgruende As DataTable
    Private mLaender As DataTable

    Private mZusatzanschreiben() As String = {"", "", "", ""}











#End Region

#Region " Properties"



    Public ReadOnly Property Zusatzanschreiben() As String()
        Get
            Return mZusatzanschreiben
        End Get
    End Property


    Public Property SucheLeasingvertragsnummer() As String
        Get
            Return mSucheLeasingvertragsnummer
        End Get
        Set(ByVal Value As String)
            If Not Value Is Nothing Then
                mSucheLeasingvertragsnummer = Value.Replace(" ", "")
            Else
                mSucheLeasingvertragsnummer = ""
            End If

        End Set
    End Property

    Public Property Versandgrund() As String
        Get
            Return mVersandgrund
        End Get
        Set(ByVal value As String)
            mVersandgrund = value
        End Set
    End Property


    Public Property VersandgrundZusatztext() As String
        Get
            Return mVersandgrundZusatzText
        End Get
        Set(ByVal value As String)
            mVersandgrundZusatzText = value
        End Set
    End Property


    Public Property VersandEmpfängerArt() As String
        Get
            Return mVersandEmpfaengerArt
        End Get
        Set(ByVal value As String)
            mVersandEmpfaengerArt = value
        End Set
    End Property

    Public Property Geschaefsstelle() As String
        Get
            Return mGeschaeftsstellenValue
        End Get
        Set(ByVal value As String)
            mGeschaeftsstellenValue = value
        End Set
    End Property

    Public Property Partner() As String
        Get
            Return mPartnerAdressValue
        End Get
        Set(ByVal value As String)
            mPartnerAdressValue = value
        End Set
    End Property

    Public Property SucheKennzeichen() As String
        Get
            Return mSucheKennzeichen
        End Get
        Set(ByVal Value As String)
            If Not Value Is Nothing Then
                mSucheKennzeichen = Value.Replace(" ", "")
            Else
                mSucheKennzeichen = ""
            End If
        End Set
    End Property

    Public Property SucheFahrgestellnummer() As String
        Get
            Return mSucheFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            If Not Value Is Nothing Then
                mSucheFahrgestellnummer = Value.Replace(" ", "")
            Else
                mSucheFahrgestellnummer = ""
            End If
        End Set
    End Property


    Public Property SucheSuchname() As String
        Get
            Return mSucheSuchname
        End Get
        Set(ByVal Value As String)
            If Not Value Is Nothing Then
                mSucheSuchname = Value.Replace(" ", "")
            Else
                mSucheSuchname = ""
            End If
        End Set
    End Property

    Public ReadOnly Property Fahrzeuge() As DataTable
        Get
            Return mFahrzeuge
        End Get
    End Property

    Public Property MaterialText() As String
        Get
            Return mTextZumMaterial
        End Get
        Set(ByVal value As String)
            mTextZumMaterial = value
        End Set
    End Property

    Public Property MaterialNummer() As String
        Get
            Return mMaterialNummer
        End Get
        Set(ByVal Value As String)
            mMaterialNummer = Value
        End Set
    End Property

    Public Property Bemerkung() As String
        Get
            Return mBemerkung
        End Get
        Set(ByVal Value As String)
            mBemerkung = Value
        End Set
    End Property

    Public Property VersandAdressValue() As String
        Get
            Return mVersandAdressValue
        End Get
        Set(ByVal Value As String)
            mVersandAdressValue = Value
        End Set
    End Property

    Public ReadOnly Property VersandAdressText() As String
        Get
            Dim tmpStr As String = ""
            Dim tmpRow As DataRow
            Select Case mVersandEmpfaengerArt
                Case "Anschrift"
                    tmpStr = Name1 & " " & Name2 & "<br>" & Street & " " & HouseNum & "<br>" & LaenderKuerzel & " " & PostCode & " " & City & "<br>"
                Case "Partner"
                    tmpRow = PartnerAdressen.Select("EX_KUNNR='" & Partner & "'")(0)
                    tmpStr = tmpRow("SORTL").ToString & " " & tmpRow("Name1").ToString & " " & tmpRow("Name2").ToString & "<br>" & tmpRow("STREET").ToString & " " & tmpRow("HOUSE_NUM1").ToString & "<br>" & tmpRow("POST_CODE1").ToString & " " & tmpRow("CITY1").ToString
                Case "Geschaeft"
                    tmpRow = VersandAdressen.Select("KUNNR='" & Geschaefsstelle & "'")(0)
                    tmpStr = tmpRow("SORTL").ToString & " " & tmpRow("Name1").ToString & " " & tmpRow("Name2").ToString & "<br>" & tmpRow("STREET").ToString & " " & tmpRow("HOUSE_NUM1").ToString & "<br>" & tmpRow("POST_CODE1").ToString & " " & tmpRow("CITY1").ToString
            End Select

            Return tmpStr
        End Get
    End Property


    Public ReadOnly Property VersandAdressen() As DataTable
        Get
            Return mVersandAdressen
        End Get
    End Property


    Public ReadOnly Property PartnerAdressen() As DataTable
        Get
            Return mPartnerAdressen
        End Get
    End Property

    Public ReadOnly Property Abrufgruende() As DataTable
        Get
            If mAbrufgruende Is Nothing Then
                Dim cn As SqlClient.SqlConnection
                Dim cmdAg As SqlClient.SqlCommand
                Dim dsAg As DataSet
                Dim adAg As SqlClient.SqlDataAdapter
                cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Try

                    cn.Open()

                    dsAg = New DataSet()

                    adAg = New SqlClient.SqlDataAdapter()

                    cmdAg = New SqlClient.SqlCommand("SELECT " & _
                                                    "[WebBezeichnung]," & _
                                                    "[SapWert]," & _
                                                    "[MitZusatzText]," & _
                                                    "[Zusatzbemerkung], " & _
                                                    "[AbrufTyp] " & _
                                                    "FROM CustomerAbrufgruende " & _
                                                    "WHERE " & _
                                                    "CustomerID = " & "72" & _
                                                    " AND GroupID = " & "272" _
                                                    , cn)
                    cmdAg.CommandType = CommandType.Text
                    'AbrufTyp: 'temp' oder 'endg'

                    adAg.SelectCommand = cmdAg
                    adAg.Fill(dsAg, "Abrufgruende")

                    If dsAg.Tables("Abrufgruende") Is Nothing OrElse dsAg.Tables("Abrufgruende").Rows.Count = 0 Then
                        Throw New Exception("Keine Abrufgründe für den Kunden hinterlegt.")
                    End If

                    mAbrufgruende = dsAg.Tables("Abrufgruende")
                Catch ex As Exception
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End If

            Return mAbrufgruende
        End Get


    End Property


    Public Property Name1() As String
        Get
            Return mName1
        End Get
        Set(ByVal Value As String)
            mName1 = Value
        End Set

    End Property

    Public Property Name2() As String
        Get
            Return mName2
        End Get
        Set(ByVal Value As String)
            mName2 = Value
        End Set

    End Property

    Public Property City() As String
        Get
            Return mCity
        End Get
        Set(ByVal Value As String)
            mCity = Value
        End Set
    End Property

    Public Property PostCode() As String
        Get
            Return mPostcode
        End Get
        Set(ByVal Value As String)
            mPostcode = Value
        End Set
    End Property

    Public Property Street() As String
        Get
            Return mStreet
        End Get
        Set(ByVal Value As String)
            mStreet = Value
        End Set
    End Property
    Public Property HouseNum() As String
        Get
            Return mHouseNum
        End Get
        Set(ByVal Value As String)
            mHouseNum = Value
        End Set
    End Property
    Public ReadOnly Property Laender() As DataTable
        Get
            If mLaender Is Nothing Then
                getLaender()
            End If
            Return mLaender
        End Get
    End Property

    Public Property LaenderKuerzel() As String
        Get
            Return mLaenderKuerzel
        End Get
        Set(ByVal Value As String)
            mLaenderKuerzel = Value
        End Set

    End Property

    Public Property Eigentumsvorbehalt() As String
        Get
            Return mEigentumsvorbehalt
        End Get
        Set(ByVal Value As String)
            mEigentumsvorbehalt = Value
        End Set

    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

    End Sub

    Public Sub callBapiForAdressen()
        m_strClassAndMethod = "Briefanforderung.callBapiForAdressen"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            m_intStatus = 0


            Try
                Dim proxy = DynSapProxy.getProxy("Z_M_GET_VERSANDADDR_UC", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                'befüllen der Importparameter
                proxy.setImportParameter("I_AG", Right("0000000000" & "345728", 10)) 'unicredit
                Dim mImportTable = proxy.getImportTable("GT_EQUIS")

                For Each tmpRow As DataRow In Fahrzeuge.Select("Anfordern='X'")
                    Dim newRow As DataRow = mImportTable.NewRow
                    newRow(0) = tmpRow("EQUNR")
                    mImportTable.Rows.Add(newRow)
                Next

                mImportTable.AcceptChanges()


                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_UNANGEFORDERT_UC", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                proxy.callBapi()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                mPartnerAdressen = proxy.getExportTable("GT_ADDR")
                mVersandAdressen = proxy.getExportTable("GT_KUNDEN")
                If Not mPartnerAdressen Is Nothing Then HelpProcedures.killAllDBNullValuesInDataTable(mPartnerAdressen)
                If Not mVersandAdressen Is Nothing Then HelpProcedures.killAllDBNullValuesInDataTable(mVersandAdressen)
            Catch ex As Exception
                mVersandAdressen = Nothing
                mPartnerAdressen = Nothing
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_EQUIES"
                        m_strMessage = "Keine Briefdaten übergeben. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                m_blnGestartet = False
            End Try
        End If

    End Sub

    Public Overloads Overrides Sub Show()
        m_strClassAndMethod = "Briefanforderung.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            m_intStatus = 0

            Try
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_UNANGEFORDERT_UC", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim proxy = DynSapProxy.getProxy("Z_M_UNANGEFORDERT_UC", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                'befüllen der Importparameter
                proxy.setImportParameter("I_AG", Right("0000000000" & "345728", 10)) 'unicredit
                proxy.setImportParameter("I_CHASSIS_NUM", mSucheFahrgestellnummer.ToUpper)
                proxy.setImportParameter("I_LIZNR", mSucheLeasingvertragsnummer.ToUpper)
                proxy.setImportParameter("I_LICENSE_NUM", mSucheKennzeichen.ToUpper)
                proxy.setImportParameter("I_SORTL", mSucheSuchname.ToUpper)

                proxy.callBapi()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                'auswerten der exportparameter
                mFahrzeuge = proxy.getExportTable("GT_WEB")
                If Not mFahrzeuge Is Nothing Then
                    mFahrzeuge.Columns.Add("Anfordern", GetType(String))
                    HelpProcedures.killAllDBNullValuesInDataTable(mFahrzeuge)
                    mFahrzeuge = CreateOutPut(mFahrzeuge, AppID)

                    mFahrzeuge.AcceptChanges()
                End If

            Catch ex As Exception
                mFahrzeuge = Nothing
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case "NO_HAENDLER"
                        m_strMessage = "Händler nicht vorhanden. "
                    Case "ENNG_VERSAND"
                        m_strMessage = "ZBII bereits endgültig versendet. "
                    Case "TEMP_VERSAND"
                        m_strMessage = "ZBII bereits temporär versendet. "
                    Case "ENDG_ANGEF"
                        m_strMessage = "ZBII bereits endgültig angefordert. "
                    Case "TEMP_ANGEF"
                        m_strMessage = "ZBII bereits temporär angefordert. "
                    Case "HAENDLER_NOT_FOUND"
                        m_strMessage = "Händler nicht gefunden. "
                    Case "HIST_CHECK"
                        m_strMessage = "Bitte in Historie prüfen. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                m_blnGestartet = False
            End Try
        End If
    End Sub
    Public Overrides Sub change()

    End Sub


    Public Overloads Sub Change(ByVal abcKZ As String)
        m_strClassAndMethod = "Briefanforderung.Change"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            m_intStatus = 0
            Try

                mFahrzeuge.Columns.Add("ErrorMessage", String.Empty.GetType)
                mFahrzeuge.AcceptChanges()

                For Each tmptRow As DataRow In mFahrzeuge.Select("Anfordern='X'")
                    If m_objLogApp Is Nothing Then
                        m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                    End If
                    intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_BRIEFANFORDERUNG_UC", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                    Dim proxy = DynSapProxy.getProxy("Z_M_BRIEFANFORDERUNG_UC", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                    'befüllen der Importparameter
                    proxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & "345728", 10)) 'unicredit
                    proxy.setImportParameter("I_ERNAM", Left(m_objUser.UserName, 12))
                    proxy.setImportParameter("I_MATNR", mMaterialNummer)
                    Select Case abcKZ.ToUpper
                        Case "TEMP"
                            proxy.setImportParameter("I_ABCKZ", "1")
                        Case "ENDG"
                            proxy.setImportParameter("I_ABCKZ", "2")
                        Case Else
                            Throw New Exception("Briefanforderung verfügt über kein gültiges ABCKZ")
                    End Select
                    proxy.setImportParameter("I_ZZVGRUND", mVersandgrund)
                    proxy.setImportParameter("I_VSST1", mEigentumsvorbehalt)
                    proxy.setImportParameter("I_S1", mZusatzanschreiben(0))
                    proxy.setImportParameter("I_S2", mZusatzanschreiben(1))
                    proxy.setImportParameter("I_S3", mZusatzanschreiben(2))
                    proxy.setImportParameter("I_S4", mZusatzanschreiben(3))
                    proxy.setImportParameter("I_ZZBETREFF", mBemerkung)
                    proxy.setImportParameter("I_EQUNR", tmptRow("EQUNR").ToString())

                    Select Case mVersandEmpfaengerArt
                        Case "Anschrift"
                            proxy.setImportParameter("I_NAME1", mName1)
                            proxy.setImportParameter("I_NAME2", mName2)
                            proxy.setImportParameter("I_STRAS", mStreet)
                            proxy.setImportParameter("I_LAND1", mLaenderKuerzel)
                            proxy.setImportParameter("I_ORT01", mCity)
                            proxy.setImportParameter("I_PSTLZ", mPostcode)
                            proxy.setImportParameter("I_HOUSE_NUM", mHouseNum)
                        Case "Partner"
                            proxy.setImportParameter("I_EX_KUNNR", Partner)
                        Case "Geschaeft"
                            proxy.setImportParameter("I_KUNNR_ZS", Geschaefsstelle)
                    End Select

                    proxy.callBapi()

                    If intID > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(intID, True)
                    End If

                    Dim eFehltxt = proxy.getExportParameter("E_FEHLTXT")
                    tmptRow("ErrorMessage") = If(eFehltxt Is Nothing, String.Empty, eFehltxt.Trim())
                Next

            Catch ex As Exception
                mFahrzeuge = Nothing
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_UPDATE_EQUI"
                        m_strMessage = "EQUI-UPDATE-Fehler. "
                    Case "NO_AUFTRAG"
                        m_strMessage = "Kein Auftrag angelegt. "
                    Case "NO_ZDADVERSAND"
                        m_strMessage = "Keine Einträge für die Versanddatei erstellt. "
                    Case "NO_UPDATE_ILOA"
                        m_strMessage = "ILOA-Update-Fehler. "
                    Case "NO_BRIEFANFORDERUNG"
                        m_strMessage = "Brief bereits angefordert. "
                    Case "NO_EQUZ_ILOA"
                        m_strMessage = "Kein Brief vorhanden (EQUZ+ILOA). "
                    Case "EQUI_SPERRE"
                        m_strMessage = "Equi ist gesperrt. "
                    Case "NO_INSERT_ADRESSE"
                        m_strMessage = "Keine Adresse angelegt. "
                    Case "NO_ZS_KUNNR"
                        m_strMessage = "ZS-Kunde nicht angelegt. "
                    Case Else
                        m_strMessage = "Beim der Übertragung ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Private Sub getLaender()
        Dim intID As Int32 = -1
        Try
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Land_Plz_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

            Dim proxy = DynSapProxy.getProxy("Z_M_Land_Plz_001", m_objApp, m_objUser, PageHelper.GetCurrentPage())

            proxy.callBapi()

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            mLaender = proxy.getExportTable("GT_WEB")

            mLaender.Columns.Add("Beschreibung", GetType(String))
            mLaender.Columns.Add("FullDesc", GetType(String))

            For Each rowTemp As DataRow In mLaender.Rows
                If CInt(rowTemp("LNPLZ")) > 0 Then
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx")) & " (" & CStr(CInt(rowTemp("LNPLZ"))) & ")"
                Else
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx"))
                End If
                rowTemp("FullDesc") = CStr(rowTemp("Land1")) & " " & CStr(rowTemp("Beschreibung"))
            Next
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
        Finally
            If m_intIDSAP > -1 Then
                m_objLogApp.LogStandardIdentity = m_intStandardLogID
                m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
            End If
        End Try


    End Sub


#End Region

End Class

' ************************************************
' $History: Briefanforderung.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.02.09   Time: 13:34
' Updated in $/CKAG2/Applications/AppGenerali/lib
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.02.09   Time: 17:58
' Updated in $/CKAG2/Applications/AppGenerali/lib
' ka
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 23.02.09   Time: 14:50
' Created in $/CKAG2/Applications/AppGenerali/lib
' briefanforderung
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 3.12.08    Time: 16:26
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2446 testfertig
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 10.11.08   Time: 15:34
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2367 testfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 30.10.08   Time: 13:41
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' testfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 30.10.08   Time: 11:51
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' weiterentwicklung
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 29.10.08   Time: 16:27
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2284 unfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 27.10.08   Time: 17:20
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2286 Änderungen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.10.08   Time: 11:15
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2284 Weiterentwicklung
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 21.10.08   Time: 17:11
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2284 weiterentwicklung
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 17.10.08   Time: 10:44
' Created in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2284 torso
' 
' ************************************************