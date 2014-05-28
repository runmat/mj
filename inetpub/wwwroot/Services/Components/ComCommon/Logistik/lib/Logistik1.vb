Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common

Namespace Logistik

    Public Class Logistik1
        Inherits BankBase

#Region "Declarations"
        Private m_strName As String = ""
        Private m_strStrasse As String = ""
        Private m_strTelefon As String = ""
        Private m_strHausnummer As String = ""
        Private m_strPostleitzahl As String = ""
        Private m_strOrt As String = ""
        Private m_strAPartner As String = ""
        Private m_blnExpress As Boolean
        Private m_blnStandard As Boolean

        Private m_strLiefName As String = ""
        Private m_strLiefStrasse As String = ""
        Private m_strLiefTelefon As String = ""
        Private m_strLiefHausnummer As String = ""
        Private m_strLiefPostleitzahl As String = ""
        Private m_strLiefOrt As String = ""
        Private m_strLiefAPartner As String = ""
        Private m_strFixFlag As String = ""
        Private m_strFixTermin As String = ""

        Private m_GeoCodeDaten As DataTable
        Private m_Daten As DataTable
        Private m_LiefAdressen As DataTable

        Private mErrMessage As String

        Private m_strFahrgestellnummer As String = ""
        Private m_strKennzeichen As String = ""
        Private m_strMietendvon As String = ""
        Private m_strMietendbis As String = ""
        Private m_strEntfernung As String = ""
        Private mE_SUBRC As String = ""
        Private mE_MESSAGE As String = ""
        Private E_FEHLER_ST As String = ""
        Private E_FEHLER_ZI As String = ""

        Private mE_VBELN As String = ""
        Private mE_VBELN_1510 As String = ""
        Private mE_QMNUM As String = ""
        Private m_strWE_Nr As String = ""
        Private m_strSORTL As String = ""
        Private m_strSAPNr As String = ""
        Private m_blnConfirm As Boolean
#End Region

#Region " Properties"


        Public Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                m_strName = Value
            End Set
        End Property

        Public Property Strasse() As String
            Get
                Return m_strStrasse
            End Get
            Set(ByVal Value As String)
                m_strStrasse = Value
            End Set
        End Property

        Public Property Telefon() As String
            Get
                Return m_strTelefon
            End Get
            Set(ByVal Value As String)
                m_strTelefon = Value
            End Set
        End Property

        Public Property Hausnummer() As String
            Get
                Return m_strHausnummer
            End Get
            Set(ByVal Value As String)
                m_strHausnummer = Value
            End Set
        End Property

        Public Property Postleitzahl() As String
            Get
                Return m_strPostleitzahl
            End Get
            Set(ByVal Value As String)
                m_strPostleitzahl = Value
            End Set
        End Property

        Public Property Ort() As String
            Get
                Return m_strOrt
            End Get
            Set(ByVal Value As String)
                m_strOrt = Value
            End Set
        End Property

        Public Property APartner() As String
            Get
                Return m_strAPartner
            End Get
            Set(ByVal Value As String)
                m_strAPartner = Value
            End Set
        End Property

        Public Property Express() As Boolean
            Get
                Return m_blnExpress
            End Get
            Set(ByVal Value As Boolean)
                m_blnExpress = Value
            End Set
        End Property

        Public Property Standard() As Boolean
            Get
                Return m_blnStandard
            End Get
            Set(ByVal Value As Boolean)
                m_blnStandard = Value
            End Set
        End Property


        Public Property LiefName() As String
            Get
                Return m_strLiefName
            End Get
            Set(ByVal Value As String)
                m_strLiefName = Value
            End Set
        End Property

        Public Property LiefStrasse() As String
            Get
                Return m_strLiefStrasse
            End Get
            Set(ByVal Value As String)
                m_strLiefStrasse = Value
            End Set
        End Property

        Public Property LiefTelefon() As String
            Get
                Return m_strLiefTelefon
            End Get
            Set(ByVal Value As String)
                m_strLiefTelefon = Value
            End Set
        End Property

        Public Property LiefHausnummer() As String
            Get
                Return m_strLiefHausnummer
            End Get
            Set(ByVal Value As String)
                m_strLiefHausnummer = Value
            End Set
        End Property

        Public Property LiefPostleitzahl() As String
            Get
                Return m_strLiefPostleitzahl
            End Get
            Set(ByVal Value As String)
                m_strLiefPostleitzahl = Value
            End Set
        End Property

        Public Property LiefOrt() As String
            Get
                Return m_strLiefOrt
            End Get
            Set(ByVal Value As String)
                m_strLiefOrt = Value
            End Set
        End Property
        Public Property LiefAPartner() As String
            Get
                Return m_strLiefAPartner
            End Get
            Set(ByVal Value As String)
                m_strLiefAPartner = Value
            End Set
        End Property
        Public Property GeoCodeDaten() As DataTable
            Get
                Return m_GeoCodeDaten
            End Get
            Set(ByVal Value As DataTable)
                m_GeoCodeDaten = Value
            End Set
        End Property

        Public Property ErrMessage() As String
            Get
                Return mErrMessage
            End Get
            Set(ByVal Value As String)
                mErrMessage = Value
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

        Public Property Kennzeichen() As String
            Get
                Return m_strKennzeichen
            End Get
            Set(ByVal Value As String)
                m_strKennzeichen = Value
            End Set
        End Property

        Public Property MietendVon() As String
            Get
                Return m_strMietendvon
            End Get
            Set(ByVal Value As String)
                m_strMietendvon = Value
            End Set
        End Property

        Public Property Mietendbis() As String
            Get
                Return m_strMietendbis
            End Get
            Set(ByVal Value As String)
                m_strMietendbis = Value
            End Set
        End Property

        Public Property Daten() As DataTable
            Get
                Return m_Daten
            End Get
            Set(ByVal Value As DataTable)
                m_Daten = Value
            End Set
        End Property
        Public Property LiefAdressen() As DataTable
            Get
                Return m_LiefAdressen
            End Get
            Set(ByVal Value As DataTable)
                m_LiefAdressen = Value
            End Set
        End Property
        Public Property Entfernung() As String
            Get
                Return m_strEntfernung
            End Get
            Set(ByVal Value As String)
                m_strEntfernung = Value
            End Set
        End Property
        Public Property Confirm() As Boolean
            Get
                Return m_blnConfirm
            End Get
            Set(ByVal Value As Boolean)
                m_blnConfirm = Value
            End Set
        End Property
        Public Property VBELN() As String
            Get
                Return mE_VBELN
            End Get
            Set(ByVal Value As String)
                mE_VBELN = Value
            End Set
        End Property

        Public Property WE_Nr() As String
            Get
                Return m_strWE_Nr
            End Get
            Set(ByVal Value As String)
                m_strWE_Nr = Value
            End Set
        End Property
        Public Property SORTL() As String
            Get
                Return m_strSORTL
            End Get
            Set(ByVal Value As String)
                m_strSORTL = Value
            End Set
        End Property
        Public Property SAPNr() As String
            Get
                Return m_strSAPNr
            End Get
            Set(ByVal Value As String)
                m_strSAPNr = Value
            End Set
        End Property

        Public Property FixFlag() As String
            Get
                Return m_strFixFlag
            End Get
            Set(ByVal Value As String)
                m_strFixFlag = Value
            End Set
        End Property

        Public Property FixTermin() As String
            Get
                Return m_strFixTermin
            End Get
            Set(ByVal Value As String)
                m_strFixTermin = Value
            End Set
        End Property
#End Region


        Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
            MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

            m_strName = ""
            m_strStrasse = ""
            m_strHausnummer = ""
            m_strPostleitzahl = ""
            m_strOrt = ""
            m_strName = ""
            m_strStrasse = ""
            m_strHausnummer = ""
            m_strPostleitzahl = ""
            m_strOrt = ""
            m_strAPartner = ""
            m_strTelefon = ""
            m_strLiefAPartner = ""
            m_strLiefName = ""
            m_strLiefStrasse = ""
            m_strLiefHausnummer = ""
            m_strLiefPostleitzahl = ""
            m_strLiefOrt = ""
            m_strLiefAPartner = ""
            m_strLiefTelefon = ""
            m_blnExpress = False
            m_blnStandard = False
            mErrMessage = ""
            m_strSORTL = ""
        End Sub


        Public Overrides Sub Change()

        End Sub

        Public Overrides Sub Show()

        End Sub

        Public Overloads Sub Show(ByVal strAppID As String, _
                            ByVal strSessionID As String, _
                            ByVal page As Web.UI.Page)

            m_strClassAndMethod = "Logistik1.Show"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            m_intStatus = 0
            m_strMessage = ""
            If Not m_blnGestartet Then
                m_blnGestartet = True
                Dim intID As Int32 = -1

                Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

                Try

                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_EQUI_ZUM_ZC", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                    myProxy.setImportParameter("I_EX_KUNNR", m_objUser.Reference)
                    myProxy.setImportParameter("I_LICENSE_NUM ", Kennzeichen)
                    myProxy.setImportParameter("I_CHASSIS_NUM", Fahrgestellnr)
                    myProxy.setImportParameter("I_MIETENDE_VON", MietendVon)
                    myProxy.setImportParameter("I_MIETENDE_BIS", Mietendbis)


                    myProxy.callBapi()

                    m_Daten = New DataTable

                    mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                    mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

                    m_Daten = myProxy.getExportTable("GT_DATEN")
                    m_Daten.Columns.Add("Express", GetType(System.String))
                    m_Daten.Columns.Add("Standard", GetType(System.String))
                    m_Daten.Columns("STATUS").MaxLength = 25
                    m_Daten.AcceptChanges()

                    Dim tmpRow As DataRow
                    For Each tmpRow In m_Daten.Rows
                        If tmpRow("STATUS").ToString = "B" Then
                            tmpRow("STATUS") = "beauftragt"
                        End If
                    Next
                    m_Daten.AcceptChanges()

                    If mE_SUBRC <> "0" Then
                        m_intStatus = CInt(mE_SUBRC)
                        m_strMessage = mE_MESSAGE
                    End If

                Catch ex As Exception
                    m_Daten = Nothing
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Overloads Sub Change(ByVal strAppID As String, _
                    ByVal strSessionID As String, _
                    ByVal page As Web.UI.Page)
            m_strClassAndMethod = "Logistik1.Change"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            m_intStatus = 0
            m_strMessage = ""
            If Not m_blnGestartet Then
                m_blnGestartet = True
                Dim intID As Int32 = -1

                Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

                Try

                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Order_Ueberfuehrung_Create", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("KUNNR", strKUNNR)
                    myProxy.setImportParameter("VKORG", "1510")
                    myProxy.setImportParameter("MATNR", Right("000000000000000000" & 13930, 18))
                    myProxy.setImportParameter("ZZKENN", Kennzeichen)
                    myProxy.setImportParameter("ZZFAHRG", Fahrgestellnr)
                    myProxy.setImportParameter("I_WMENG", Entfernung)
                    myProxy.setImportParameter("AUGRU", "Z00")
                    myProxy.setImportParameter("VDATU", m_strFixTermin)
                    myProxy.setImportParameter("FIX", m_strFixFlag)
                    If Express = True Then
                        myProxy.setImportParameter("EXPRESS_VERSAND", "J")
                    End If

                    Dim tblPartner As DataTable = myProxy.getImportTable("PARTNER")
                    Dim Partnerrow As DataRow = tblPartner.NewRow


                    'Auftrageber
                    Partnerrow("PARTN_ROLE") = "AG"
                    Partnerrow("PARTN_NUMB") = strKUNNR
                    Partnerrow("ITM_NUMBER") = "000000"
                    tblPartner.Rows.Add(Partnerrow)


                    'Rechnungsempfänger
                    Partnerrow = tblPartner.NewRow
                    Partnerrow("PARTN_ROLE") = "RE"
                    Partnerrow("PARTN_NUMB") = Right("0000000000" & m_strSAPNr, 10)
                    Partnerrow("ITM_NUMBER") = "000000"
                    tblPartner.Rows.Add(Partnerrow)

                    'Regulierer
                    Partnerrow = tblPartner.NewRow
                    Partnerrow("PARTN_ROLE") = "RG"
                    Partnerrow("PARTN_NUMB") = Right("0000000000" & m_strSAPNr, 10)
                    Partnerrow("ITM_NUMBER") = "000000"
                    tblPartner.Rows.Add(Partnerrow)

                    Partnerrow = tblPartner.NewRow
                    Partnerrow("PARTN_ROLE") = "ZB"
                    Partnerrow("PARTN_NUMB") = Right("0000000000" & m_strSAPNr, 10)
                    Partnerrow("ITM_NUMBER") = "000000"
                    If Name.Trim.Length > 35 Then
                        Partnerrow("Name") = Left(Name, 35)
                        Dim strTemp As String
                        strTemp = Mid(Name, 35)
                        If strTemp.Trim.Length > 35 Then
                            Dim SubStr As String
                            Partnerrow("Name_2") = Left(strTemp, 35)
                            SubStr = Mid(strTemp, 35)
                            If SubStr.Trim.Length > 35 Then
                                Dim SubStr2 As String
                                Partnerrow("Name_3") = Left(SubStr, 35)
                                SubStr2 = Mid(SubStr, 35)

                            Else
                                Partnerrow("Name_3") = strTemp
                            End If
                        Else
                            Partnerrow("Name_2") = strTemp
                        End If
                    Else
                        Partnerrow("Name") = Name.Trim
                    End If

                    '
                    Partnerrow("Street") = Strasse & " " & Hausnummer
                    Partnerrow("Postl_Code") = Postleitzahl
                    Partnerrow("City") = Ort
                    Partnerrow("Telephone") = Telefon
                    tblPartner.Rows.Add(Partnerrow)

                    'Warenempfänger
                    Partnerrow = tblPartner.NewRow
                    Partnerrow("PARTN_ROLE") = "WE"
                    If m_strWE_Nr <> "" Then
                        Partnerrow("PARTN_NUMB") = m_strWE_Nr
                    Else
                        Partnerrow("PARTN_NUMB") = "0000390051"
                    End If

                    Partnerrow("ITM_NUMBER") = "000000"
                    If LiefName.Trim.Length > 35 Then
                        Partnerrow("Name") = Left(LiefName, 35)
                        Dim strTemp As String
                        strTemp = Mid(LiefName, 35)
                        If strTemp.Trim.Length > 35 Then
                            Dim SubStr As String
                            Partnerrow("Name_2") = Left(strTemp, 35)
                            SubStr = Mid(strTemp, 35)
                            If SubStr.Trim.Length > 35 Then
                                Dim SubStr2 As String
                                Partnerrow("Name_3") = Left(SubStr, 35)
                                SubStr2 = Mid(SubStr, 35)

                            Else
                                Partnerrow("Name_3") = strTemp
                            End If
                        Else
                            Partnerrow("Name_2") = strTemp
                        End If
                    Else
                        Partnerrow("Name") = LiefName.Trim
                    End If
                    'Partnerrow("Name_2") = ""
                    Partnerrow("Street") = LiefStrasse & " " & LiefHausnummer
                    Partnerrow("Postl_Code") = LiefPostleitzahl
                    Partnerrow("City") = LiefOrt
                    Partnerrow("Telephone") = LiefTelefon




                    tblPartner.Rows.Add(Partnerrow)

                    myProxy.callBapi()

                    mE_VBELN = myProxy.getExportParameter("VBELN")
                    mE_VBELN_1510 = myProxy.getExportParameter("VBELN_1510")
                    mE_QMNUM = myProxy.getExportParameter("QMNUM")

                Catch ex As Exception
                    m_intStatus = -5555
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case "NO_DATA"
                            m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                        Case "NO_PARAMETER"
                            m_strMessage = "Eingabedaten nicht ausreichend."
                        Case "NO_ASL"
                            m_strMessage = "Falsche Kundennr."
                        Case "NO_LANGTEXT"
                            m_strMessage = ""
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                    End Select
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub FillLiefAdressen(ByVal strAppID As String, _
                            ByVal strSessionID As String, _
                            ByVal page As Web.UI.Page)

            m_strClassAndMethod = "Logistik1.FillLiefAdressen"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            If Not m_blnGestartet Then
                m_blnGestartet = True
                Dim intID As Int32 = -1

                Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

                Try

                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_ADESSEN_001", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_VKORG", "1510")
                    myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                    myProxy.setImportParameter("I_SORTL", m_strSORTL)



                    myProxy.callBapi()

                    m_LiefAdressen = New DataTable
                    m_LiefAdressen = myProxy.getExportTable("GT_WEB")

                    mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                    mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

                    If mE_SUBRC <> "0" Then
                        m_intStatus = CInt(mE_SUBRC)
                        m_strMessage = mE_MESSAGE
                    End If

                Catch ex As Exception
                    m_GeoCodeDaten = Nothing
                Finally
                    m_blnGestartet = False
                End Try
            End If

        End Sub


        Public Sub GeoAdressen(ByVal strAppID As String, _
                            ByVal strSessionID As String, _
                            ByVal page As Web.UI.Page, Optional ByVal Lief As String = "")

            m_strClassAndMethod = "Logistik1.GeoAdressen"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            If Not m_blnGestartet Then
                m_blnGestartet = True
                Dim intID As Int32 = -1

                Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

                Try

                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_CHECK_ADRESS", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_LAND", "DE")
                    If Lief = "" Then
                        myProxy.setImportParameter("I_STRASSE", Strasse)
                        myProxy.setImportParameter("I_HAUSNR", Hausnummer)
                        myProxy.setImportParameter("I_POSTLTZ", Postleitzahl)
                        myProxy.setImportParameter("I_ORT", Ort)
                    Else
                        myProxy.setImportParameter("I_STRASSE", LiefStrasse)
                        myProxy.setImportParameter("I_HAUSNR", LiefHausnummer)
                        myProxy.setImportParameter("I_POSTLTZ", LiefPostleitzahl)
                        myProxy.setImportParameter("I_ORT", LiefOrt)
                    End If



                    myProxy.callBapi()

                    m_GeoCodeDaten = New DataTable
                    m_GeoCodeDaten = myProxy.getExportTable("GT_GEO")


                Catch ex As Exception
                    m_GeoCodeDaten = Nothing
                Finally
                    m_blnGestartet = False
                End Try
            End If


        End Sub


        Public Sub GetGeoEntfernung(ByVal strAppID As String, _
                            ByVal strSessionID As String, _
                            ByVal page As Web.UI.Page)

            m_strClassAndMethod = "Logistik1.GetGeoEntfernung"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            If Not m_blnGestartet Then
                m_blnGestartet = True
                Dim intID As Int32 = -1
                Try

                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_GEOKODIERUNG_001", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_AKTION", "E")
                    myProxy.setImportParameter("I_COUNTRY_ST", "DE")
                    myProxy.setImportParameter("I_POST_CODE1_ST", Postleitzahl)
                    myProxy.setImportParameter("I_CITY1_ST", Ort)
                    myProxy.setImportParameter("I_STREET_ST", Strasse)
                    myProxy.setImportParameter("I_HOUSE_NUM1_ST", Hausnummer)
                    myProxy.setImportParameter("I_COUNTRY_ZI", "DE")
                    myProxy.setImportParameter("I_POST_CODE1_ZI", LiefPostleitzahl)
                    myProxy.setImportParameter("I_CITY1_ZI", LiefOrt)
                    myProxy.setImportParameter("I_STREET_ZI", LiefStrasse)
                    myProxy.setImportParameter("I_HOUSE_NUM1_ZI", LiefHausnummer)

                    myProxy.callBapi()



                    mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                    mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")
                    m_strEntfernung = myProxy.getExportParameter("E_ENTFERNUNG")
                    E_FEHLER_ZI = myProxy.getExportParameter("E_FEHLER_ZI")
                    E_FEHLER_ST = myProxy.getExportParameter("E_FEHLER_ST")

                    If mE_SUBRC <> "0" Then
                        m_intStatus = CInt(mE_SUBRC)
                        m_strMessage = mE_MESSAGE
                    End If

                Catch ex As Exception

                Finally
                    m_blnGestartet = False
                End Try
            End If


        End Sub
    End Class


End Namespace
