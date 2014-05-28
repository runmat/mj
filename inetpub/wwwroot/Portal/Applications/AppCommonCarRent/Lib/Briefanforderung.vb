Option Explicit On 
Option Strict On

Imports CKG
Imports CKG.Base.Common
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

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
    Private mSucheFahrgestellnummer As String = ""
    Private mSucheHaendlernr As String = ""

    Private mVersandAdressValue As String = ""
    Private mMaterialNummer As String = "" 'Versandart
    Private mMaterialTEXT As String = "" 'VersandartText
    Private mVersandEmpfaengerArt As String = ""
    Private mGeschaeftsstellenValue As String = ""
    Private mPartnerAdressValue As String = ""
    Private mBemerkung As String = ""

    Private mFahrzeuge As DataTable
    Private mVersandAdressen As New DataTable
    Private mPartnerAdressen As New DataTable
    Private mVersanddaten As DataTable
    Private mAbrufgruende As DataTable
    Private mLaender As DataTable
    Private mFahrzeugeChanged As DataTable

    Private mZusatzanschreiben() As String = {"", "", "", ""}
    Private mRetourdatVon As String = ""
    Private mRetourdatBis As String = ""
    Private mAbmeldeDatVon As String = ""
    Private mAbmeldeDatbis As String = ""
    Private mstrBezahlt As String = ""
    Private mstrGesperrt As String = ""
    Private mstrSelectedHaendler As String = ""
    Private mstrSelectedEqui As String = ""

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


    Public Property MaterialText() As String
        Get
            Return mMaterialTEXT
        End Get
        Set(ByVal value As String)
            mMaterialTEXT = value
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

    Public ReadOnly Property Fahrzeuge() As DataTable
        Get
            Return mFahrzeuge
        End Get
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
    Public Property FahrzeugeChanged() As System.Data.DataTable
        Get
            Return Me.mFahrzeugeChanged
        End Get
        Set(ByVal value As System.Data.DataTable)
            Me.mFahrzeugeChanged = value
        End Set
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
            Return mLaender
        End Get
    End Property

    Public Property RetourdatVon() As String
        Get
            Return mRetourdatVon
        End Get
        Set(ByVal Value As String)
            mRetourdatVon = Value
        End Set

    End Property
    Public Property RetourdatBis() As String
        Get
            Return mRetourdatBis
        End Get
        Set(ByVal Value As String)
            mRetourdatBis = Value
        End Set

    End Property
    Public Property LaenderKuerzel() As String
        Get
            Return mLaenderKuerzel
        End Get
        Set(ByVal Value As String)
            mLaenderKuerzel = Value
        End Set

    End Property
    Public Property AbmeldeDatVon() As String
        Get
            Return mAbmeldeDatVon
        End Get
        Set(ByVal Value As String)
            mAbmeldeDatVon = Value
        End Set

    End Property
    Public Property AbmeldeDatbis() As String
        Get
            Return mAbmeldeDatbis
        End Get
        Set(ByVal Value As String)
            mAbmeldeDatbis = Value
        End Set

    End Property
    Public Property Bezahlt() As String
        Get
            Return mstrBezahlt
        End Get
        Set(ByVal Value As String)
            mstrBezahlt = Value
        End Set

    End Property

    Public Property SucheHaendlernr() As String
        Get
            Return Me.mSucheHaendlernr
        End Get
        Set(ByVal value As String)
            Me.mSucheHaendlernr = value
        End Set
    End Property
    Public Property Versanddaten() As System.Data.DataTable
        Get
            Return Me.mVersanddaten
        End Get
        Set(ByVal value As System.Data.DataTable)
            Me.mVersanddaten = value
        End Set
    End Property
    Public Property SelectedHaendler() As System.String
        Get
            Return Me.mstrSelectedHaendler
        End Get
        Set(ByVal value As System.String)
            Me.mstrSelectedHaendler = value
        End Set
    End Property

    Public Property SelectedEqui() As System.String
        Get
            Return Me.mstrSelectedEqui
        End Get
        Set(ByVal value As System.String)
            Me.mstrSelectedEqui = value
        End Set
    End Property

    Public Property Gesperrt() As System.String
        Get
            Return Me.mstrGesperrt
        End Get
        Set(ByVal value As System.String)
            Me.mstrGesperrt = value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

    End Sub


    Public Overrides Sub Show()

    End Sub

    Public Sub callBapiForAdressenOverEqui(ByVal strAppID As String, ByVal strSessionID As String, ByVal strEqui As String, ByRef page As Page)
        '----------------------------------------------------------------------
        ' Methode: callBapiForAdressenOverEqui
        ' Autor: ORu
        ' Beschreibung: fragt die Versandadressen über Equinummer im SAP ab 
        ' Erstellt am: 14.05.2009
        ' ITA: 2851
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_GET_VERSANDADDR_AV_001", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            'Dim SapImportTable As DataTable = myProxy.getImportTable("GT_EQUIS")

            S.AP.Init("Z_M_GET_VERSANDADDR_AV_001", "I_AG", m_strKUNNR)
            Dim SapImportTable As DataTable = S.AP.GetImportTable("GT_EQUIS")

            Dim newSapImportRow As DataRow
            newSapImportRow = SapImportTable.NewRow
            newSapImportRow("EQUNR") = strEqui
            SapImportTable.Rows.Add(newSapImportRow)

            'myProxy.callBapi()
            S.AP.Execute()



            mPartnerAdressen = S.AP.GetExportTable("GT_ADDR") 'myProxy.getExportTable("GT_ADDR")
            mVersandAdressen = S.AP.GetExportTable("GT_KUNDEN") 'myProxy.getExportTable("GT_KUNDEN")


        Catch ex As Exception
            mVersandAdressen = Nothing
            mPartnerAdressen = Nothing
            m_intStatus = -9999

            Select Case ex.Message
                Case "NO_EQUIES"
                    m_strMessage = "Keine Briefdaten übergeben. "
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try

    End Sub

    Public Function callBapiForAdressen(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page) As Boolean

        '----------------------------------------------------------------------
        ' Methode: callBapiForAdressen
        ' Autor: JJU
        ' Beschreibung: fragt die Versandadressen im SAP ab
        ' Erstellt am: 12.02.2009
        ' ITA: 2596
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        Dim bAnfordern As Boolean = False

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_GET_VERSANDADDR_AV_001", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            'Dim SapImportTable As DataTable = myProxy.getImportTable("GT_EQUIS")

            S.AP.Init("Z_M_GET_VERSANDADDR_AV_001", "I_AG", m_strKUNNR)
            Dim SapImportTable As DataTable = S.AP.GetImportTable("GT_EQUIS")

            Dim newSapImportRow As DataRow
            For Each tmprow As DataRow In FahrzeugeChanged.Select("Anfordern='X'")
                newSapImportRow = SapImportTable.NewRow
                newSapImportRow("EQUNR") = tmprow("EQUNR")
                SapImportTable.Rows.Add(newSapImportRow)
                bAnfordern = True
            Next

            If bAnfordern = True Then
                'myProxy.callBapi()
                S.AP.Execute()

                mPartnerAdressen = S.AP.GetExportTable("GT_ADDR") 'myProxy.getExportTable("GT_ADDR")
                mVersandAdressen = S.AP.GetExportTable("GT_KUNDEN") 'myProxy.getExportTable("GT_KUNDEN")
            End If


            Return bAnfordern
        Catch ex As Exception
            mVersandAdressen = Nothing
            mPartnerAdressen = Nothing
            m_intStatus = -9999

            Select Case ex.Message
                Case "NO_EQUIES"
                    m_strMessage = "Keine Briefdaten übergeben. "
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            Return False
        End Try


    End Function

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)
        '----------------------------------------------------------------------
        ' Methode: Show
        ' Autor: JJU
        ' Beschreibung: gibt alle endgültig anforderbaren briefe zurück
        ' Erstellt am: 12.02.2009
        ' ITA: 2596
        '----------------------------------------------------------------------

        m_strClassAndMethod = "Briefanforderung.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_UNANGEFORDERT_AV_001", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.setImportParameter("I_CHASSIS_NUM", mSucheFahrgestellnummer.ToUpper)
            'myProxy.setImportParameter("I_LIZNR", mSucheLeasingvertragsnummer.ToUpper)
            'myProxy.setImportParameter("I_LICENSE_NUM", mSucheKennzeichen.ToUpper)
            'myProxy.setImportParameter("I_HAENDLER", mSucheHaendlernr.ToUpper)
            'myProxy.setImportParameter("I_RET_DAT_VON ", mRetourdatVon)
            'myProxy.setImportParameter("I_RET_DAT_BIS", mRetourdatBis)
            'myProxy.setImportParameter("I_EXPIRY_DATE_VON", mAbmeldeDatVon)
            'myProxy.setImportParameter("I_EXPIRY_DATE_BIS", mAbmeldeDatbis)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_UNANGEFORDERT_AV_001", "I_AG,I_CHASSIS_NUM,I_LIZNR,I_LICENSE_NUM,I_HAENDLER,I_RET_DAT_VON,I_RET_DAT_BIS,I_EXPIRY_DATE_VON,I_EXPIRY_DATE_BIS",
                                m_strKUNNR,
                                mSucheFahrgestellnummer.ToUpper,
                                mSucheLeasingvertragsnummer.ToUpper,
                                mSucheKennzeichen.ToUpper,
                                mSucheHaendlernr.ToUpper,
                                mRetourdatVon,
                                mRetourdatBis,
                                mAbmeldeDatVon,
                                mAbmeldeDatbis)

            mFahrzeuge = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
           
            If Not mstrBezahlt Is Nothing Then
                If mstrBezahlt = " " Then
                    Dim DatRows() As DataRow
                    DatRows = mFahrzeuge.Select("ZZBEZAHLT='X'")
                    If DatRows.Length > 0 Then
                        For Each tmpRow As DataRow In DatRows
                            mFahrzeuge.Rows.Remove(tmpRow)
                        Next
                    End If
                Else
                    Dim DatRows() As DataRow
                    DatRows = mFahrzeuge.Select("ZZBEZAHLT=' '")
                    If DatRows.Length > 0 Then
                        For Each tmpRow As DataRow In DatRows
                            mFahrzeuge.Rows.Remove(tmpRow)
                        Next
                    End If
                End If
            End If

            If Not mstrGesperrt Is Nothing Then
                If mstrGesperrt = " " Then
                    Dim DatRows() As DataRow
                    DatRows = mFahrzeuge.Select("ZZAKTSPERRE='X'")
                    If DatRows.Length > 0 Then
                        For Each tmpRow As DataRow In DatRows
                            mFahrzeuge.Rows.Remove(tmpRow)
                        Next
                    End If
                Else
                    Dim DatRows() As DataRow
                    DatRows = mFahrzeuge.Select("ZZAKTSPERRE=' '")
                    If DatRows.Length > 0 Then
                        For Each tmpRow As DataRow In DatRows
                            mFahrzeuge.Rows.Remove(tmpRow)
                        Next
                    End If
                End If
            End If


            Dim DatRowsFreigabe() As DataRow
            DatRowsFreigabe = mFahrzeuge.Select("FLAG_VERS_FREIG='X'")
            If DatRowsFreigabe.Length > 0 Then
                For Each tmpRow As DataRow In DatRowsFreigabe
                    mFahrzeuge.Rows.Remove(tmpRow)
                Next
            End If

            mFahrzeuge = CreateOutPut(mFahrzeuge, AppID)
            mFahrzeuge.AcceptChanges()

        Catch ex As Exception
            mFahrzeuge = Nothing
            m_intStatus = -9999

            Select Case ex.Message
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

        End Try

    End Sub

    Public Overrides Sub change()

    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page, ByVal abcKZ As String)
        '----------------------------------------------------------------------
        ' Methode: Change
        ' Autor: JJU
        ' Beschreibung: fordern briefe an
        ' Erstellt am: 12.02.2009
        ' ITA: 2596
        '----------------------------------------------------------------------

        m_strClassAndMethod = "Briefanforderung.Change"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BRIEFANFORDERUNG_AV_001", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            S.AP.Init("Z_M_BRIEFANFORDERUNG_AV_001", "I_KUNNR_AG", m_strKUNNR)


            Dim sapImportTable As DataTable = S.AP.GetImportTable("GT_ZDAD_FW_BEAUFTRAGUNG_001") 'myProxy.getImportTable("GT_ZDAD_FW_BEAUFTRAGUNG_001")
            Dim newSapRow As DataRow

            For Each tmpRow As DataRow In mFahrzeugeChanged.Rows
                newSapRow = sapImportTable.NewRow

                If tmpRow("Anfordern").ToString = "X" Then
                    Dim tmpRowAdr2 As DataRow
                   
                    Select Case mVersandEmpfaengerArt
                        Case "Partner"
                            tmpRowAdr2 = PartnerAdressen.Select("EX_KUNNR='" & Partner & "'")(0)
                            mName1 = tmpRowAdr2("Name1").ToString
                            mName2 = tmpRowAdr2("Name2").ToString
                            mStreet = tmpRowAdr2("STREET").ToString
                            mLaenderKuerzel = tmpRowAdr2("COUNTRY").ToString
                            mCity = tmpRowAdr2("CITY1").ToString
                            mPostcode = tmpRowAdr2("POST_CODE1").ToString
                            mHouseNum = tmpRowAdr2("HOUSE_NUM1").ToString
                        Case "Geschaeft"
                            tmpRowAdr2 = VersandAdressen.Select("KUNNR='" & Geschaefsstelle & "'")(0)
                            mName1 = tmpRowAdr2("Name1").ToString
                            mName2 = tmpRowAdr2("Name2").ToString
                            mStreet = tmpRowAdr2("STREET").ToString
                            mLaenderKuerzel = tmpRowAdr2("COUNTRY").ToString
                            mCity = tmpRowAdr2("CITY1").ToString
                            mPostcode = tmpRowAdr2("POST_CODE1").ToString
                            mHouseNum = tmpRowAdr2("HOUSE_NUM1").ToString
                    End Select
                Else
                    mName1 = ""
                    mName2 = ""
                    mStreet = ""
                    mLaenderKuerzel = ""
                    mCity = ""
                    mPostcode = ""
                    mHouseNum = ""
                    mMaterialNummer = ""
                End If


                newSapRow("CHASSIS_NUM") = tmpRow("Fahrgestellnummer")
                newSapRow("LICENSE_NUM") = tmpRow("Kennzeichen")

                newSapRow("ZZNAME1_ZS") = mName1
                newSapRow("ZZNAME2_ZS") = mName2
                newSapRow("ZZSTRAS_ZS") = mStreet
                newSapRow("ZZLAND1") = mLaenderKuerzel
                newSapRow("ZZORT01_ZS") = mCity
                newSapRow("ZZPSTLZ_ZS") = mPostcode
                newSapRow("ZZHAUSNR_ZS") = mHouseNum


                newSapRow("MATNR") = mMaterialNummer
                newSapRow("ERNAM") = Left(m_objUser.UserName, 12)

                newSapRow("ZZBEZAHLT") = tmpRow("Bezahltkennzeichen").ToString
                newSapRow("ZZAKTSPERRE") = tmpRow("Versandsperre").ToString
                newSapRow("ZZVSFREIGABE") = tmpRow("Anfordern").ToString

                If tmpRow("Abmelden").ToString = "" Then
                    newSapRow("ZZABMELD") = "X"
                Else
                    newSapRow("ZZABMELD") = ""
                End If

                sapImportTable.Rows.Add(newSapRow)
            Next

            If Not mFahrzeugeChanged.Columns.Contains("ErrorMessage") Then
                mFahrzeugeChanged.Columns.Add("ErrorMessage", String.Empty.GetType)
            End If

            mFahrzeugeChanged.AcceptChanges()

            'myProxy.callBapi()
            S.AP.Execute()

            Dim SAPResultTable As DataTable = S.AP.GetExportTable("GT_ZDAD_FW_BEAUFTRAGUNG_001") 'myProxy.getExportTable("GT_ZDAD_FW_BEAUFTRAGUNG_001")

            For Each tmpRow As DataRow In mFahrzeugeChanged.Rows
                tmpRow("ErrorMessage") = SAPResultTable.Select("CHASSIS_NUM='" & tmpRow("Fahrgestellnummer").ToString & "'")(0)("ZFEHLERTEXT")
            Next

        Catch ex As Exception
            mFahrzeuge = Nothing
            m_intStatus = -9999

            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "keine Eingabedaten"
                Case Else
                    m_strMessage = "Beim der Übertragung ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try

    End Sub

    Public Sub getLaender(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)

        '----------------------------------------------------------------------
        ' Methode: getLaender
        ' Autor: JJU
        ' Beschreibung: gibt die länder aus sap zurück
        ' Erstellt am: 12.02.2009
        ' ITA: 2596
        '----------------------------------------------------------------------

        m_strClassAndMethod = "Briefanforderung.getLaender"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Land_Plz_001", m_objApp, m_objUser, Page)
            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Land_Plz_001")

            mLaender = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

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

        Catch ex As Exception
            Select Case ex.Message
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
        End Try

    End Sub

    Public Sub CreateVersanddaten()
        mVersanddaten = New DataTable
        With Versanddaten.Columns
            .Add("EQUNR", Type.GetType("System.String"))
            .Add("EX_KUNNR", Type.GetType("System.String"))
            .Add("SORTL", Type.GetType("System.String"))
            .Add("NAME1", Type.GetType("System.String"))
            .Add("NAME2", Type.GetType("System.String"))
            .Add("STREET", Type.GetType("System.String"))
            .Add("HOUSE_NUM1", Type.GetType("System.String"))
            .Add("POST_CODE1", Type.GetType("System.String"))
            .Add("CITY1", Type.GetType("System.String"))
            .Add("COUNTRY", Type.GetType("System.String"))
            .Add("VersEmpfArt", Type.GetType("System.String"))
            .Add("MaterialNummer", Type.GetType("System.String"))
            .Add("MaterialText", Type.GetType("System.String"))
            .Add("Geschaefsstelle", Type.GetType("System.String"))
            .Add("Partner", Type.GetType("System.String"))
        End With
    End Sub

#End Region

End Class

' ************************************************
' $History: Briefanforderung.vb $
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 24.06.09   Time: 10:00
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA: 2939
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 23.06.09   Time: 15:05
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 18.06.09   Time: 12:34
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA: 2804
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 16.06.09   Time: 17:07
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA:2804
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 10.06.09   Time: 15:25
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA:2804
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 10.06.09   Time: 14:36
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA: 2804
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 20.05.09   Time: 17:01
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 13.02.09   Time: 13:51
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2596 / 2589
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 12.02.09   Time: 17:40
' Created in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2596 im test
' 
' ************************************************