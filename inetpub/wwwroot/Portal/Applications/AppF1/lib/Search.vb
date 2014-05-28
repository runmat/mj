Option Explicit On
Option Strict On

Imports CKG
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

<Serializable()> Public Class Search
    Inherits F1_BankBase

#Region " Definitions"
    Private m_strHaendlerReferenzNummer As String
    Private m_strSapInterneHaendlerReferenzNummer As String
    Private m_strHaendlerName As String
    Private m_strHaendlerOrt As String
    Private m_strHaendlerFiliale As String
    Private m_tblHaendler As DataTable
    Private m_tblFilialen As DataTable
    Private m_districtTable As DataTable
    Private m_strErrorMessage As String

    'Private mKontingenttabelle As DataTable


    'Private m_objApp As Base.Kernel.Security.App
    'Private m_objUser As Base.Kernel.Security.User

    'Private m_strCUSTOMER As String
    Private m_strNAME As String
    Private m_strNAME_2 As String
    Private m_strCOUNTRYISO As String
    Private m_strPOSTL_CODE As String
    Private m_strCITY As String
    Private m_strSTREET As String

    Private m_strREFERENZ As String
    'Private m_strFILIALE As String

    'Private m_blnGestartet As Boolean
    Private m_tblSearchResult As DataTable

    '<NonSerialized()> Private m_strSessionID As String
    '<NonSerialized()> Private m_strAppID As String
    <NonSerialized()> Private m_vwHaendler As DataView
    <NonSerialized()> Private m_districtView As DataView
    <NonSerialized()> Private m_vwFilialen As DataView
    '<NonSerialized()> Protected m_objLogApp As Base.Kernel.Logging.Trace
    '<NonSerialized()> Protected m_intIDSAP As Int32

    'für sucheHaendler Control
    '---------------------------------
    Private aStrHaendlernummer() As String
    Private aStrName1() As String
    Private aStrName2() As String
    Private aStrOrt() As String
    Private aStrPLZ() As String
    Private aStrStrasse() As String
    Private inthaendlerTreffer As Int32
    Private strZeigeAlle As String = ""
    Private strSucheHaendlerNr As String
    Private strSuchePLZ As String
    Private strSucheOrt As String
    Private strSucheName1 As String
    Private strSucheName2 As String
    '---------------------------------

#End Region

#Region " Public Properties"

    Public ReadOnly Property SearchResult() As DataTable
        Get
            Return m_tblSearchResult
        End Get
    End Property

    Public Property HaendlerReferenzNummer() As String
        Get
            Return m_strHaendlerReferenzNummer
        End Get
        Set(ByVal Value As String)
            m_strHaendlerReferenzNummer = Value
        End Set
    End Property

    Public Property SapInterneHaendlerReferenzNummer() As String
        Get
            Return m_strSapInterneHaendlerReferenzNummer
        End Get
        Set(ByVal Value As String)
            m_strSapInterneHaendlerReferenzNummer = Value
        End Set
    End Property

    Public Property HaendlerName() As String
        Get
            Return m_strHaendlerName
        End Get
        Set(ByVal Value As String)
            m_strHaendlerName = Value
        End Set
    End Property

    Public Property HaendlerOrt() As String
        Get
            Return m_strHaendlerOrt
        End Get
        Set(ByVal Value As String)
            m_strHaendlerOrt = Value
        End Set
    End Property

    Public Property HaendlerFiliale() As String
        Get
            Return m_strHaendlerFiliale
        End Get
        Set(ByVal Value As String)
            m_strHaendlerFiliale = Value
        End Set
    End Property

    Public ReadOnly Property Haendler() As DataView
        Get
            Return m_vwHaendler
        End Get
    End Property

    Public ReadOnly Property Filialen() As DataView
        Get
            Return m_tblFilialen.DefaultView
        End Get
    End Property

    Public ReadOnly Property District() As DataView
        Get
            Return m_districtTable.DefaultView
        End Get
    End Property

    Public ReadOnly Property ErrorMessage() As String
        Get
            Return m_strErrorMessage
        End Get
    End Property

    Public ReadOnly Property NAME() As String
        Get
            Return m_strNAME
        End Get
    End Property

    Public ReadOnly Property NAME_2() As String
        Get
            Return m_strNAME_2
        End Get
    End Property

    Public ReadOnly Property COUNTRYISO() As String
        Get
            Return m_strCOUNTRYISO
        End Get
    End Property

    Public ReadOnly Property POSTL_CODE() As String
        Get
            Return m_strPOSTL_CODE
        End Get
    End Property

    Public ReadOnly Property CITY() As String
        Get
            Return m_strCITY
        End Get
    End Property

    Public ReadOnly Property STREET() As String
        Get
            Return m_strSTREET
        End Get
    End Property

    Public ReadOnly Property REFERENZ() As String
        Get
            Return m_strREFERENZ
        End Get
    End Property

    Public Property sucheHaendlerNr() As String
        Get
            Return strSucheHaendlerNr
        End Get
        Set(ByVal Value As String)
            strSucheHaendlerNr = Value
        End Set
    End Property

    Public Property suchePLZ() As String
        Get
            Return strSuchePLZ
        End Get
        Set(ByVal Value As String)
            strSuchePLZ = Value
        End Set
    End Property

    Public Property sucheOrt() As String
        Get
            Return strSucheOrt
        End Get
        Set(ByVal Value As String)
            strSucheOrt = Value
        End Set
    End Property

    Public Property sucheName1() As String
        Get
            Return strSucheName1
        End Get
        Set(ByVal Value As String)
            strSucheName1 = Value
        End Set
    End Property

    Public Property sucheName2() As String
        Get
            Return strSucheName2
        End Get
        Set(ByVal Value As String)
            strSucheName2 = Value
        End Set
    End Property

    Public ReadOnly Property aHaendlernummer() As String()
        Get
            Return aStrHaendlernummer
        End Get
    End Property

    Public ReadOnly Property aName1() As String()
        Get
            Return aStrName1
        End Get
    End Property

    Public ReadOnly Property aName2() As String()
        Get
            Return aStrName2
        End Get
    End Property

    Public ReadOnly Property aOrt() As String()
        Get
            Return aStrOrt
        End Get
    End Property

    Public ReadOnly Property aPLZ() As String()
        Get
            Return aStrPLZ
        End Get
    End Property

    Public ReadOnly Property aStrasse() As String()
        Get
            Return aStrStrasse
        End Get
    End Property

    Public ReadOnly Property anzahlHaendlerTreffer() As Integer
        Get
            Return inthaendlerTreffer
        End Get
    End Property

#End Region

#Region " Public Methods"

    Public Sub New(ByRef objApp As Base.Kernel.Security.App, ByRef objUser As Base.Kernel.Security.User, ByVal strSessionID As String, ByVal strAppID As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, "")

        m_objApp = objApp
        m_objUser = objUser

        m_strHaendlerReferenzNummer = ""
        m_strHaendlerName = ""
        m_strHaendlerOrt = ""
        m_strHaendlerFiliale = ""
        m_strSapInterneHaendlerReferenzNummer = ""

        m_strSessionID = strSessionID
        m_strAppID = strAppID

    End Sub

    Public Sub fillHaendlerData(ByVal strAppID As String, ByVal strSessionID As String, ByVal Haendlernummer As String)
        '----------------------------------------------------------------------
        ' Methode: fillHaendlerData
        ' Autor: JJU
        ' Beschreibung: gibt für einen Händler das Kontingent und dessen adresse zurück
        ' Erstellt am: 04.03.2009
        ' ITA: 2661
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_HAENDLER_KONTINGENT_STD", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_HAENDLER_EX", Haendlernummer)


            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_HAENDLER_KONTINGENT_STD", "I_AG,I_HAENDLER_EX", Right("0000000000" & m_objUser.KUNNR, 10), Haendlernummer)

            m_tblKontingente = S.AP.GetExportTable("GT_LIMIT") 'myProxy.getExportTable("GT_LIMIT")
            Dim tblAdresseTemp As DataTable = S.AP.GetExportTable("EX_ADRS") 'myProxy.getExportTable("EX_ADRS")

            For Each tmpRow As DataRow In m_tblKontingente.Rows
                'translate Kontingentarten
                Select Case tmpRow.Item("RECART").ToString
                    Case "G"
                        tmpRow.Item("RECART") = "Gruppe"
                    Case "H"
                        tmpRow.Item("RECART") = "Händler"
                    Case "S"
                        tmpRow.Item("RECART") = "Summe"
                        'bei summe nur frei als summe ausgeben
                        tmpRow.Item("KLIMK") = DBNull.Value
                        tmpRow.Item("SKFOR") = DBNull.Value
                    Case Else
                        tmpRow.Item("") = "unbekannte Kontingentart"
                End Select
            Next

            m_tblKontingente.AcceptChanges()

            m_strCustomer = tblAdresseTemp(0)("AG").ToString
            m_strREFERENZ = tblAdresseTemp(0)("HAENDLER_EX").ToString
            m_strNAME = tblAdresseTemp(0)("NAME1").ToString
            m_strNAME_2 = tblAdresseTemp(0)("NAME2").ToString
            m_strCITY = tblAdresseTemp(0)("ORT01").ToString
            m_strPOSTL_CODE = tblAdresseTemp(0)("PSTLZ").ToString
            m_strSTREET = tblAdresseTemp(0)("STRAS").ToString
            m_strCOUNTRYISO = tblAdresseTemp(0)("LAND1").ToString
            m_strSapInterneHaendlerReferenzNummer = tblAdresseTemp(0)("HAENDLER").ToString

        Catch ex As Exception
            m_intStatus = -9999
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case Else
                    m_strMessage = ex.Message
            End Select
        End Try
    End Sub

    Private Sub ResetHaendlerTabelle()
        m_tblHaendler = New DataTable()

        m_tblHaendler.Columns.Add("CUSTOMER", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("REFERENZ", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("FILIALE", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("NAME", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("NAME_2", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("CITY", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("POSTL_CODE", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("STREET", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("COUNTRYISO", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("DISPLAY", System.Type.GetType("System.String"))
        m_tblHaendler.Columns.Add("DISPLAY_ADDRESS", System.Type.GetType("System.String"))
    End Sub

    Public Function LeseAdressenSAP(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Web.UI.Page, ByVal strParentNode As String, Optional ByVal adressart As String = "A", Optional ByVal nodelevel As String = "99") As Int32 'HEZ: "B" oder "C"

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            m_tblSearchResult = New DataTable()
            m_tblSearchResult.Columns.Add("ADDRESSNUMBER", System.Type.GetType("System.String"))
            m_tblSearchResult.Columns.Add("DISPLAY_ADDRESS", System.Type.GetType("System.String"))

            m_tblSearchResult.Columns.Add("POSTL_CODE", System.Type.GetType("System.String"))
            m_tblSearchResult.Columns.Add("STREET", System.Type.GetType("System.String"))
            m_tblSearchResult.Columns.Add("COUNTRYISO", System.Type.GetType("System.String"))
            m_tblSearchResult.Columns.Add("CITY", System.Type.GetType("System.String"))
            m_tblSearchResult.Columns.Add("NAME", System.Type.GetType("System.String"))
            m_tblSearchResult.Columns.Add("NAME_2", System.Type.GetType("System.String"))


            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Bapi_Customer_Get_Children", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("VALID_ON", Today.ToShortDateString)
            'myProxy.setImportParameter("NODE_LEVEL", nodelevel)
            'myProxy.setImportParameter("CUSTOMERNO", Right("0000000000" & strParentNode, 10))
            'myProxy.setImportParameter("CUSTHITYP", adressart)

            'myProxy.callBapi()

            S.AP.InitExecute("Bapi_Customer_Get_Children", "VALID_ON,NODE_LEVEL,CUSTOMERNO,CUSTHITYP",
                             Today.ToShortDateString, nodelevel, Right("0000000000" & strParentNode, 10), adressart)

            Dim newDealerDetailRow As DataRow

            Dim SAPReturnTableNODE_LIST As DataTable = S.AP.GetExportTable("NODE_LIST") 'myProxy.getExportTable("NODE_LIST")

            For Each tmpRow1 As DataRow In SAPReturnTableNODE_LIST.Rows
                If tmpRow1("NODE_LEVEL").ToString.TrimStart("0"c) = "1" Then

                    'Die Detaildaten zu den Händlern in die Tabelle m_tblHaendler schreiben

                    'myProxy = DynSapProxy.getProxy("Bapi_Customer_Getdetail2", m_objApp, m_objUser, page)

                    'myProxy.setImportParameter("CUSTOMERNO", tmpRow1("CUSTOMER").ToString)


                    'myProxy.callBapi()
                    S.AP.InitExecute("Bapi_Customer_Getdetail2", "CUSTOMERNO", tmpRow1("CUSTOMER").ToString)

                    Dim SAPReturnValue As DataTable = S.AP.GetExportTable("RETURN") 'myProxy.getExportTable("RETURN")
                    Dim SAPReturnCustomerDetail As DataTable = S.AP.GetExportTable("CUSTOMERGENERALDETAIL") 'myProxy.getExportTable("CUSTOMERGENERALDETAIL")
                    Dim SAPReturnCustomerAdress As DataTable = S.AP.GetExportTable("CUSTOMERADDRESS") 'myProxy.getExportTable("CUSTOMERADDRESS")

                    If SAPReturnValue(0)("TYPE").ToString.Trim(" "c) = "" Or SAPReturnValue(0)("TYPE").ToString = "S" Or SAPReturnValue(0)("TYPE").ToString = "I" Then

                        If (Not SAPReturnCustomerDetail(0)("Groupkey").ToString = m_objUser.Reference) Or (SAPReturnCustomerDetail(0)("Groupkey").ToString.Length = 0 And m_objUser.Reference.Length = 0) Then

                            newDealerDetailRow = m_tblSearchResult.NewRow
                            Dim strTemp As String = SAPReturnCustomerAdress(0)("Name").ToString

                            If SAPReturnCustomerAdress(0)("Name_2").ToString.Length > 0 Then
                                strTemp &= ", " & SAPReturnCustomerAdress(0)("Name_2").ToString
                            End If

                            If SAPReturnCustomerAdress(0)("Name_3").ToString.Length > 0 Then
                                strTemp &= ", " & SAPReturnCustomerAdress(0)("Name_3").ToString
                            End If

                            If SAPReturnCustomerAdress(0)("Name_4").ToString.Length > 0 Then
                                strTemp &= ", " & SAPReturnCustomerAdress(0)("Name_4").ToString
                            End If

                            newDealerDetailRow("DISPLAY_ADDRESS") = strTemp & ", " & SAPReturnCustomerAdress(0)("Countryiso").ToString & " - " & SAPReturnCustomerAdress(0)("Postl_Code").ToString & " " & SAPReturnCustomerAdress(0)("City").ToString & ", " & SAPReturnCustomerAdress(0)("Street").ToString
                            newDealerDetailRow("ADDRESSNUMBER") = SAPReturnCustomerAdress(0)("Customer").ToString

                            If SAPReturnCustomerAdress(0)("Postl_Code").ToString.Length > 0 Then
                                newDealerDetailRow("POSTL_CODE") = SAPReturnCustomerAdress(0)("Postl_Code").ToString
                            End If

                            If SAPReturnCustomerAdress(0)("Street").ToString.Length > 0 Then
                                newDealerDetailRow("STREET") = SAPReturnCustomerAdress(0)("Street").ToString
                            End If

                            If SAPReturnCustomerAdress(0)("Countryiso").ToString.Length > 0 Then
                                newDealerDetailRow("COUNTRYISO") = SAPReturnCustomerAdress(0)("Countryiso").ToString
                            End If

                            If SAPReturnCustomerAdress(0)("City").ToString.Length > 0 Then
                                newDealerDetailRow("CITY") = SAPReturnCustomerAdress(0)("City").ToString
                            End If

                            If SAPReturnCustomerAdress(0)("Name").ToString.Length > 0 Then
                                newDealerDetailRow("NAME") = SAPReturnCustomerAdress(0)("Name").ToString
                            End If

                            If SAPReturnCustomerAdress(0)("Name_2").ToString.Length > 0 Then
                                newDealerDetailRow("NAME_2") = SAPReturnCustomerAdress(0)("Name_2").ToString
                            End If

                            m_tblSearchResult.Rows.Add(newDealerDetailRow)
                        End If
                    End If
                End If
            Next

            Return m_tblSearchResult.Rows.Count

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case Else
                    m_strErrorMessage = "Es ist ein Fehler aufgetreten: " & ex.Message
                    m_intStatus = -3
            End Select
        End Try
    End Function

    Public Sub LeseHaendlerForSucheHaendlerControl(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Web.UI.Page, Optional ByVal InputReferenz As String = "")

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ADRESSDATEN_STD", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_HAENDLER_EX", sucheHaendlerNr)
            'myProxy.setImportParameter("I_NAME", sucheName1) 'ist auch mit name 2
            'myProxy.setImportParameter("I_ORT", sucheOrt)
            'myProxy.setImportParameter("I_PSTLZ", suchePLZ)
            'myProxy.setImportParameter("I_MAX", "2000")

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_ADRESSDATEN_STD", "I_AG,I_HAENDLER_EX,I_NAME,I_ORT,I_PSTLZ,I_MAX",
                             Right("0000000000" & m_objUser.KUNNR, 10), sucheHaendlerNr, sucheName1, sucheOrt, suchePLZ, 2000)

            Dim SAPReturnTable As DataTable = S.AP.GetExportTable("GT_ADRS") 'myProxy.getExportTable("GT_ADRS")


            If SAPReturnTable.Rows.Count = 0 Then
                m_strErrorMessage = "Kein Suchergebnis."
                m_vwHaendler = Nothing
            Else

                Dim newDealerDetailRow As DataRow

                'arrays Dimensionieren
                ReDim aStrHaendlernummer(SAPReturnTable.Rows.Count - 1)
                ReDim aStrName1(SAPReturnTable.Rows.Count - 1)
                ReDim aStrName2(SAPReturnTable.Rows.Count - 1)
                ReDim aStrOrt(SAPReturnTable.Rows.Count - 1)
                ReDim aStrPLZ(SAPReturnTable.Rows.Count - 1)
                ReDim aStrStrasse(SAPReturnTable.Rows.Count - 1)
                ResetHaendlerTabelle()

                For I = 0 To SAPReturnTable.Rows.Count - 1

                    'ArraysBefüllen
                    '-------------------------------------
                    aStrHaendlernummer(I) = stringZeichenEntfernen(SAPReturnTable(I)("HAENDLER_EX").ToString)
                    aStrName1(I) = stringZeichenEntfernen(SAPReturnTable(I)("NAME1").ToString)
                    aStrName2(I) = stringZeichenEntfernen(SAPReturnTable(I)("NAME2").ToString)
                    aStrOrt(I) = stringZeichenEntfernen(SAPReturnTable(I)("ORT01").ToString)
                    aStrPLZ(I) = stringZeichenEntfernen(SAPReturnTable(I)("PSTLZ").ToString)
                    aStrStrasse(I) = stringZeichenEntfernen(SAPReturnTable(I)("STRAS").ToString)
                    '-------------------------------------

                    newDealerDetailRow = m_tblHaendler.NewRow()
                    newDealerDetailRow("REFERENZ") = SAPReturnTable(I)("HAENDLER_EX").ToString
                    newDealerDetailRow("NAME") = SAPReturnTable(I)("NAME1").ToString
                    newDealerDetailRow("NAME_2") = SAPReturnTable(I)("NAME2").ToString
                    newDealerDetailRow("CITY") = SAPReturnTable(I)("ORT01").ToString
                    newDealerDetailRow("POSTL_CODE") = SAPReturnTable(I)("PSTLZ").ToString
                    newDealerDetailRow("STREET") = SAPReturnTable(I)("STRAS").ToString
                    newDealerDetailRow("COUNTRYISO") = SAPReturnTable(I)("LAND1").ToString
                    '-------------------------------------------------------
                    'newDealerDetailRow("DISPLAY") = SAPReturnTableRow.Kunnr.TrimStart("0"c) & " - " & SAPReturnTableRow.Name1 & ", " & SAPReturnTableRow.Ort01
                    'es soll mit tastendruck ein händler aus der dll ausgewählt werden, das geht nur wenn der 1. wert in der DDL das suchkriterium ist, hier der name des Händlers-
                    'Händler nummer soll in anzeige komplett verschwinden> JJU//Rothe 2008.03.03
                    'Neuer Aufbau laut Rothe 2008.03.04: Name1,Name2,Str, Ort,.
                    '-------------------------------------------------------
                    newDealerDetailRow("DISPLAY") = SAPReturnTable(I)("NAME1").ToString & " " & SAPReturnTable(I)("NAME2").ToString & "  -  " & SAPReturnTable(I)("STRAS").ToString & ", " & SAPReturnTable(I)("ORT01").ToString
                    newDealerDetailRow("DISPLAY_ADDRESS") = SAPReturnTable(I)("NAME1").ToString

                    If Not SAPReturnTable(I)("NAME2").ToString.Trim(" "c).Length = 0 Then
                        newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTable(I)("NAME2").ToString & ", " & SAPReturnTable(I)("LAND1").ToString & "-" & SAPReturnTable(I)("PSTLZ").ToString & " " & SAPReturnTable(I)("ORT01").ToString & ", " & SAPReturnTable(I)("STRAS").ToString
                    Else
                        newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTable(I)("LAND1").ToString & "-" & SAPReturnTable(I)("PSTLZ").ToString & " " & SAPReturnTable(I)("ORT01").ToString & ", " & SAPReturnTable(I)("STRAS").ToString
                    End If

                    m_tblHaendler.Rows.Add(newDealerDetailRow)

                Next

                m_vwHaendler = m_tblHaendler.DefaultView

                If SAPReturnTable.Rows.Count > 1 Then
                    'Weitere Auswahl entspechend Name und/oder Ort
                    Dim filterExp As String = ""

                    If m_strHaendlerReferenzNummer.Trim(" "c).Length = 0 Then
                        If (Len(Trim(m_strHaendlerName)) > 0) And (Len(Trim(m_strHaendlerOrt)) > 0) Then
                            filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "' AND CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
                        ElseIf Len(Trim(m_strHaendlerName)) > 0 Then
                            filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "'"
                        ElseIf Len(Trim(m_strHaendlerOrt)) > 0 Then
                            filterExp = "CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
                        End If
                    End If

                    If filterExp.Length > 0 Then
                        filterExp = Replace(filterExp, "*", "%")
                        m_vwHaendler.RowFilter = filterExp
                    End If
                End If
                m_vwHaendler.Sort = "NAME"
            End If
            inthaendlerTreffer = CInt(S.AP.GetExportParameter("E_REC_ANZ")) 'myProxy.getExportParameter("E_REC_ANZ")

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_WEB"
                    m_strErrorMessage = "Keine Web-Tabelle erstellt."
                    m_intStatus = -1
                Case "NO_DATA"
                    m_strErrorMessage = "Es wurden keine Händler gefunden"
                    inthaendlerTreffer = 0
                    m_intStatus = -2
                Case Else
                    m_strErrorMessage = "Es ist ein Fehler aufgetreten: " & ex.Message
                    m_intStatus = -3
            End Select
        End Try

    End Sub

    Private Function stringZeichenEntfernen(ByVal zeichenkette As String) As String
        If zeichenkette Is Nothing OrElse zeichenkette Is String.Empty Then
            zeichenkette = "-"
        Else
            If Not zeichenkette.IndexOf("'") = -1 Then
                zeichenkette = zeichenkette.Replace("'", "")
            End If
            If Not zeichenkette.IndexOf("""") = -1 Then
                zeichenkette = zeichenkette.Replace("""", "")
            End If
        End If

        Return zeichenkette
    End Function

#End Region

End Class

' ************************************************
' $History: Search.vb $
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 20.04.09   Time: 10:39
' Updated in $/CKAG/Applications/AppF1/lib
' suche H�ndlerControl  Bugfixes
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 20.04.09   Time: 8:50
' Updated in $/CKAG/Applications/AppF1/lib
' bugfix hndlersuche
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 20.04.09   Time: 8:42
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 20.04.09   Time: 8:41
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 17.04.09   Time: 9:41
' Updated in $/CKAG/Applications/AppF1/lib
' auskommentierte codes entfernt
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 17.04.09   Time: 9:11
' Updated in $/CKAG/Applications/AppF1/lib
' Anpassung GMAC Dokumentenversand
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 11.03.09   Time: 11:35
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 9.03.09    Time: 14:51
' Updated in $/CKAG/Applications/AppF1/lib
' 2664 testfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 5.03.09    Time: 17:22
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 5.03.09    Time: 15:52
' Updated in $/CKAG/Applications/AppF1/lib
' ita 2664
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 4.03.09    Time: 17:30
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 4.03.09    Time: 11:12
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 3.03.09    Time: 16:53
' Updated in $/CKAG/Applications/AppF1/lib
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 3.03.09    Time: 13:22
' Updated in $/CKAG/Applications/AppF1/lib
' 
' ************************************************
