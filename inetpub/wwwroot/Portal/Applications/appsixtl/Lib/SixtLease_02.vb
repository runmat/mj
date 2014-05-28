Option Explicit On 
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business

<Serializable()> Public Class SixtLease_02

    Inherits BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private m_strHaendlernummer As String
    Private m_strHalterNummer As String
    Private m_strStandortNummer As String
    Private m_strZielFirma As String
    Private m_strZielFirma2 As String
    Private m_strZielStrasse As String
    Private m_strZielHNr As String
    Private m_strZielPLZ As String
    Private m_strZielOrt As String
    Private m_strZielLand As String
    Private m_blnArvalZulassung As Boolean
    Private m_strErrorMessage As String

    Private m_strSucheFgstNr As String
    Private m_strSucheLvNr As String
    '
    Private m_kbanr As String
    Private m_zulkz As String
    '§§§ JVE 22.12.2005 neues Feld
    Private m_afnam As String
    '
    Private m_Fahrzeuge As Int32
    Private m_tableGrund As DataTable
    Private m_versandadr As String
    Private m_versandadrtext As String
    Private m_versicherung As String
    Private m_material As String
    Private m_schein As String
    Private m_abckz As String
    Private m_equ As String
    Private m_kennz As String
    Private m_tidnr As String
    Private m_liznr As String
    Private m_versgrund As String
    Private m_versgrundText As String
    Private strAuftragsstatus As String
    Private strAuftragsnummer As String
    Private dataArray As ArrayList
    Private m_blnAbwVersandadresse As Boolean
    Private rowToChange As DataRow
    Private m_strBemerkung As String
    Private m_tblAdressen As DataTable
    Private m_strBetreff As String
    Private m_versandadr_ZE As String
    Private m_versandadr_ZS As String
    Private m_strAuf As String
#End Region

#Region " Properties"

    Public ReadOnly Property Adressen() As DataTable
        Get
            Return m_tblAdressen
        End Get
    End Property

    Public Property Betreff() As String
        Get
            Return m_strBetreff
        End Get
        Set(ByVal Value As String)
            m_strBetreff = Value
        End Set
    End Property

    Public Property Auf() As String
        Get
            Return m_strAuf
        End Get
        Set(ByVal Value As String)
            m_strAuf = Value
        End Set
    End Property

    Public Property Bemerkung() As String
        Get
            Return m_strBemerkung
        End Get
        Set(ByVal Value As String)
            m_strBemerkung = Value
        End Set
    End Property

    Public Property Versicherung() As String
        Get
            Return m_versicherung
        End Get
        Set(ByVal Value As String)
            m_versicherung = Value
        End Set
    End Property

    Public Property AbwVersandadresse() As Boolean
        Get
            Return m_blnAbwVersandadresse
        End Get
        Set(ByVal Value As Boolean)
            m_blnAbwVersandadresse = Value
        End Set
    End Property


    Public Property VersandGrundText() As String
        Get
            Return m_versgrundText
        End Get
        Set(ByVal Value As String)
            m_versgrundText = Value
        End Set
    End Property

    Public Property rowChange() As DataRow
        Get
            Return rowToChange
        End Get
        Set(ByVal Value As DataRow)
            rowToChange = Value
        End Set
    End Property

    Public Property objData() As ArrayList
        Get
            Return dataArray
        End Get
        Set(ByVal Value As ArrayList)
            dataArray = Value
        End Set
    End Property

    Public Property Auftragsstatus() As String
        Get
            Return strAuftragsstatus
        End Get
        Set(ByVal Value As String)
            strAuftragsstatus = Value
        End Set
    End Property

    Public Property Auftragsnummer() As String
        Get
            Return strAuftragsnummer
        End Get
        Set(ByVal Value As String)
            strAuftragsnummer = Value
        End Set
    End Property

    Public Property VersandGrund() As String
        Get
            Return m_versgrund
        End Get
        Set(ByVal Value As String)
            m_versgrund = Value
        End Set
    End Property

    Public Property LizenzNr() As String
        Get
            Return m_liznr
        End Get
        Set(ByVal Value As String)
            m_liznr = Value
        End Set
    End Property

    Public Property TIDNr() As String
        Get
            Return m_tidnr
        End Get
        Set(ByVal Value As String)
            m_tidnr = Value
        End Set
    End Property

    Public Property Kennzeichen() As String
        Get
            Return m_kennz
        End Get
        Set(ByVal Value As String)
            m_kennz = Value
        End Set
    End Property

    Public Property Equimpent() As String
        Get
            Return m_equ
        End Get
        Set(ByVal Value As String)
            m_equ = Value
        End Set
    End Property

    Public Property Versandart() As String
        Get
            Return m_abckz
        End Get
        Set(ByVal Value As String)
            m_abckz = Value
        End Set
    End Property

    Public Property ScheinSchildernummer() As String
        Get
            Return m_schein
        End Get
        Set(ByVal Value As String)
            m_schein = Value
        End Set
    End Property

    Public Property Materialnummer() As String
        Get
            Return m_material
        End Get
        Set(ByVal Value As String)
            m_material = Value
        End Set
    End Property

    Public Property VersandAdresse_ZE() As String
        Get
            Return m_versandadr_ZE
        End Get
        Set(ByVal Value As String)
            m_versandadr_ZE = Value
        End Set
    End Property

    Public Property VersandAdresse_ZS() As String
        Get
            Return m_versandadr_ZS
        End Get
        Set(ByVal Value As String)
            m_versandadr_ZS = Value
        End Set
    End Property

    Public Property VersandAdresse() As String
        Get
            Return m_versandadr
        End Get
        Set(ByVal Value As String)
            m_versandadr = Value
        End Set
    End Property

    Public Property VersandAdresseText() As String
        Get
            Return m_versandadrtext
        End Get
        Set(ByVal Value As String)
            m_versandadrtext = Value
        End Set
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblResult
        End Get
        Set(ByVal Value As DataTable)
            m_tblResult = Value
        End Set
    End Property
    Public ReadOnly Property GrundTabelle() As DataTable
        Get
            Return m_tableGrund
        End Get
    End Property

    Public Property SucheLvNr() As String
        Get
            Return m_strSucheLvNr
        End Get
        Set(ByVal Value As String)
            m_strSucheLvNr = Value
        End Set
    End Property

    Public Property SucheFgstNr() As String
        Get
            Return m_strSucheFgstNr
        End Get
        Set(ByVal Value As String)
            m_strSucheFgstNr = Value
        End Set
    End Property

    Public Property ArvalZulassung() As Boolean
        Get
            Return m_blnArvalZulassung
        End Get
        Set(ByVal Value As Boolean)
            m_blnArvalZulassung = Value
        End Set
    End Property

    Public Property ZielLand() As String
        Get
            Return m_strZielLand
        End Get
        Set(ByVal Value As String)
            m_strZielLand = Value
        End Set
    End Property

    Public Property ZielOrt() As String
        Get
            Return m_strZielOrt
        End Get
        Set(ByVal Value As String)
            m_strZielOrt = Value
        End Set
    End Property

    Public Property ZielPLZ() As String
        Get
            Return m_strZielPLZ
        End Get
        Set(ByVal Value As String)
            m_strZielPLZ = Value
        End Set
    End Property

    Public Property ZielHNr() As String
        Get
            Return m_strZielHNr
        End Get
        Set(ByVal Value As String)
            m_strZielHNr = Value
        End Set
    End Property

    Public Property ZielStrasse() As String
        Get
            Return m_strZielStrasse
        End Get
        Set(ByVal Value As String)
            m_strZielStrasse = Value
        End Set
    End Property

    Public Property ZielFirma2() As String
        Get
            Return m_strZielFirma2
        End Get
        Set(ByVal Value As String)
            m_strZielFirma2 = Value
        End Set
    End Property

    Public Property ZielFirma() As String
        Get
            Return m_strZielFirma
        End Get
        Set(ByVal Value As String)
            m_strZielFirma = Value
        End Set
    End Property

    Public Property Haendlernummer() As String
        Get
            Return m_strHaendlernummer
        End Get
        Set(ByVal Value As String)
            m_strHaendlernummer = Value
        End Set
    End Property

    Public Property HalterNummer() As String
        Get
            Return m_strHalterNummer
        End Get
        Set(ByVal Value As String)
            m_strHalterNummer = Value
        End Set
    End Property

    Public Property StandortNummer() As String
        Get
            Return m_strStandortNummer
        End Get
        Set(ByVal Value As String)
            m_strStandortNummer = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String,
                   ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

        m_blnArvalZulassung = False

    End Sub

    Public Overrides Sub Show()
    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Function LeseAdressenSAP(ByVal strAppID As String, ByVal strSessionID As String, ByVal strParentNode As String,
                                    Optional ByVal adressart As String = "A", Optional ByVal nodelevel As String = "99") As Int32 'HEZ: "B" oder "C"
        
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            m_tblAdressen = New DataTable()
            m_tblAdressen.Columns.Add("ADDRESSNUMBER", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("DISPLAY_ADDRESS", System.Type.GetType("System.String"))

            m_tblAdressen.Columns.Add("POSTL_CODE", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("STREET", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("COUNTRYISO", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("CITY", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("NAME", System.Type.GetType("System.String"))
            m_tblAdressen.Columns.Add("NAME_2", System.Type.GetType("System.String"))

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Bapi_Customer_Get_Children", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("VALID_ON", Today.ToShortDateString)
            'myProxy.setImportParameter("NODE_LEVEL", nodelevel)
            'myProxy.setImportParameter("CUSTOMERNO", Right("0000000000" & strParentNode, 10))
            'myProxy.setImportParameter("CUSTHITYP", adressart)

            'myProxy.callBapi()

            S.AP.InitExecute("Bapi_Customer_Get_Children", "VALID_ON, NODE_LEVEL, CUSTOMERNO, CUSTHITYP",
                             Today.ToShortDateString(), nodelevel, Right("0000000000" & strParentNode, 10), adressart)

            Dim newDealerDetailRow As DataRow

            'Dim SAPReturnTableNODE_LIST As DataTable = myProxy.getExportTable("NODE_LIST")
            Dim SAPReturnTableNODE_LIST As DataTable = S.AP.GetExportTable("NODE_LIST")

            For Each tmpRow1 As DataRow In SAPReturnTableNODE_LIST.Rows
                If tmpRow1("NODE_LEVEL").ToString.TrimStart("0"c) = "1" Then

                    'Die Detaildaten zu den Händlern in die Tabelle m_tblHaendler schreiben

                    'myProxy = DynSapProxy.getProxy("Bapi_Customer_Getdetail2", m_objApp, m_objUser, page)

                    'myProxy.setImportParameter("CUSTOMERNO", tmpRow1("CUSTOMER").ToString)

                    'myProxy.callBapi()

                    S.AP.InitExecute("Bapi_Customer_Getdetail2", "CUSTOMERNO", tmpRow1("CUSTOMER").ToString)

                    'Dim SAPReturnValue As DataTable = myProxy.getExportTable("RETURN")
                    'Dim SAPReturnCustomerDetail As DataTable = myProxy.getExportTable("CUSTOMERGENERALDETAIL")
                    'Dim SAPReturnCustomerAdress As DataTable = myProxy.getExportTable("CUSTOMERADDRESS")

                    Dim SAPReturnValue As DataTable = S.AP.GetExportTable("RETURN")
                    Dim SAPReturnCustomerDetail As DataTable = S.AP.GetExportTable("CUSTOMERGENERALDETAIL")
                    Dim SAPReturnCustomerAdress As DataTable = S.AP.GetExportTable("CUSTOMERADDRESS")

                    If SAPReturnValue.Rows(0)("TYPE").ToString.Trim(" "c) = "" Or SAPReturnValue.Rows(0)("TYPE").ToString = "S" Or SAPReturnValue.Rows(0)("TYPE").ToString = "I" Then

                        If (Not SAPReturnCustomerDetail.Rows(0)("Groupkey").ToString = m_objUser.Reference) Or (SAPReturnCustomerDetail.Rows(0)("Groupkey").ToString.Length = 0 And m_objUser.Reference.Length = 0) Then

                            newDealerDetailRow = m_tblAdressen.NewRow

                            Dim strTemp As String = SAPReturnCustomerAdress.Rows(0)("Name").ToString
                            If SAPReturnCustomerAdress.Rows(0)("Name_2").ToString.Length > 0 Then
                                strTemp &= ", " & SAPReturnCustomerAdress.Rows(0)("Name_2").ToString
                            End If
                            If SAPReturnCustomerAdress.Rows(0)("Name_3").ToString.Length > 0 Then
                                strTemp &= ", " & SAPReturnCustomerAdress.Rows(0)("Name_3").ToString
                            End If
                            If SAPReturnCustomerAdress.Rows(0)("Name_4").ToString.Length > 0 Then
                                strTemp &= ", " & SAPReturnCustomerAdress.Rows(0)("Name_4").ToString
                            End If

                            newDealerDetailRow("DISPLAY_ADDRESS") = strTemp & ", " & SAPReturnCustomerAdress.Rows(0)("Countryiso").ToString & " - " & SAPReturnCustomerAdress.Rows(0)("Postl_Code").ToString & " " & SAPReturnCustomerAdress.Rows(0)("City").ToString & ", " & SAPReturnCustomerAdress.Rows(0)("Street").ToString
                            newDealerDetailRow("ADDRESSNUMBER") = SAPReturnCustomerAdress.Rows(0)("Customer").ToString
                            If SAPReturnCustomerAdress.Rows(0)("Postl_Code").ToString.Length > 0 Then
                                newDealerDetailRow("POSTL_CODE") = SAPReturnCustomerAdress.Rows(0)("Postl_Code").ToString
                            End If
                            If SAPReturnCustomerAdress.Rows(0)("Street").ToString.Length > 0 Then
                                newDealerDetailRow("STREET") = SAPReturnCustomerAdress.Rows(0)("Street").ToString
                            End If
                            If SAPReturnCustomerAdress.Rows(0)("Countryiso").ToString.Length > 0 Then
                                newDealerDetailRow("COUNTRYISO") = SAPReturnCustomerAdress.Rows(0)("Countryiso").ToString
                            End If
                            If SAPReturnCustomerAdress.Rows(0)("City").ToString.Length > 0 Then
                                newDealerDetailRow("CITY") = SAPReturnCustomerAdress.Rows(0)("City").ToString
                            End If
                            If SAPReturnCustomerAdress.Rows(0)("Name").ToString.Length > 0 Then
                                newDealerDetailRow("NAME") = SAPReturnCustomerAdress.Rows(0)("Name").ToString
                            End If
                            If SAPReturnCustomerAdress.Rows(0)("Name_2").ToString.Length > 0 Then
                                newDealerDetailRow("NAME_2") = SAPReturnCustomerAdress.Rows(0)("Name_2").ToString
                            End If
                            m_tblAdressen.Rows.Add(newDealerDetailRow)
                        End If
                    End If
                End If
            Next
            Return m_tblAdressen.Rows.Count


        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case Else
                    m_strErrorMessage = "Es ist ein Fehler aufgetreten: " & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    m_intStatus = -3
            End Select
        End Try
    End Function
    
    Public Sub ClearResultTable()
        m_tblResult = Nothing
    End Sub
    
    'Public Sub getZulStelle(ByVal plz As String, ByVal ort As String, ByRef status As String)

    '    Try

    '        'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", m_objApp, m_objUser, Page)

    '        'myProxy.setImportParameter("I_PLZ", plz)
    '        'myProxy.setImportParameter("I_ORT", ort)

    '        'myProxy.callBapi()

    '        S.AP.InitExecute("Z_Get_Zulst_By_Plz", "I_PLZ, I_ORT", plz, ort)

    '        'Dim table As DataTable = myProxy.getExportTable("T_ZULST")
    '        Dim table As DataTable = S.AP.GetExportTable("T_ZULST")

    '        If (table.Rows.Count > 1) Then
    '            'Mehr als ein Eintrag gefunden! Darf nicht sein!
    '            status = "PLZ nicht eindeutig. Mehrere Treffer gefunden."
    '        End If

    '        'If m_intIDSAP > -1 Then
    '        '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
    '        'End If

    '        m_kbanr = table.Rows(0)("KBANR").ToString
    '        m_zulkz = table.Rows(0)("ZKFZKZ").ToString
    '        m_afnam = table.Rows(0)("AFNAM").ToString

    '    Catch ex As Exception
    '        Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

    '            Case "ERR_INV_PLZ"
    '                m_strMessage = "Ungültige Postleitzahl."
    '                m_intStatus = -1118
    '            Case Else
    '                m_strMessage = "Unbekannter Fehler."
    '                m_intStatus = -9999
    '        End Select
    '        status = m_strMessage
    '        m_kbanr = ""
    '    End Try

    'End Sub

    Public Sub GiveCars()
        'Dim tableGrund As New DataTable()
        'Dim tableFahrzeuge As New DataTable()
        Dim tableHide As New DataTable()
        Dim status As String = ""
        Dim row As DataRow
        Dim rowResult As DataRow()

        readAllAuthorizationSets(tableHide, status) 'Daten, die zur Autorisierung einstehen

        If Not m_blnGestartet Then
            m_blnGestartet = True

            'If m_objLogApp Is Nothing Then
            '    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            'End If
            m_intIDSAP = -1

            'Dim intID As Int32 = -1

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_M_Unangefordert_Arval", m_objApp, m_objUser)

                'myProxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_LICENSE_NUM", m_strSucheFgstNr)
                'myProxy.setImportParameter("I_LIZNR", m_strSucheLvNr)
                'myProxy.setImportParameter("I_VKORG", "1510")

                'intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Unangefordert_Arval", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                'If intID > -1 Then
                '    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                'End If

                S.AP.InitExecute("Z_M_Unangefordert_Arval", "I_KONZS, I_LICENSE_NUM, I_LIZNR, I_VKORG",
                                 Right("0000000000" & m_objUser.KUNNR, 10), m_strSucheFgstNr, m_strSucheLvNr, "1510")

                'm_tableGrund = myProxy.getExportTable("GT_GRU")
                'm_tblResult = myProxy.getExportTable("GT_WEB")

                m_tableGrund = S.AP.GetExportTable("GT_GRU")
                m_tblResult = S.AP.GetExportTable("GT_WEB")

                m_tblResult.Columns.Add("STATUS", GetType(System.String))
                m_tblResult.Columns.Add("ART", GetType(System.String))
                m_intStatus = 0

                If (m_tblResult Is Nothing) OrElse (m_tblResult.Rows.Count = 0) Then
                    m_intStatus = -3331
                    m_strMessage = "Keine Daten gefunden."
                Else
                    For Each row In m_tblResult.Rows
                        row("ART") = m_abckz
                        row("STATUS") = String.Empty
                        If CStr(row("ZZSTATUS_IABG")) = "X" Then
                            row("STATUS") = "In Abmeldung"
                        End If
                        If CStr(row("ZZSTATUS_ABG")) = "X" Then
                            row("STATUS") = "Abgemeldet"
                        End If
                        If CStr(row("ZZSTATUS_ZUG")) = "X" Then
                            row("STATUS") = "Zugelassen"
                        End If
                        rowResult = tableHide.Select("EQUIPMENT = '" & row("EQUNR").ToString & "'")
                        If Not (rowResult.Length = 0) Then
                            row("STATUS") = row("STATUS").ToString & "(*)"
                        End If
                    Next
                    m_tblResult.AcceptChanges()
                End If

                WriteLogEntry(True, "HAENDLER=" & m_strHaendlernummer & ", LVNr.=" & m_strSucheLvNr & ", KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = -3331
                        m_strMessage = "Keine Daten gefunden."
                    Case "NO_HAENDLER"
                        m_intStatus = -3332
                        m_strMessage = "Keine oder falsche Haendlernummer."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                'If intID > -1 Then
                '    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                'End If
                WriteLogEntry(False, "HAENDLER=" & m_strHaendlernummer & ", LVNr.=" & m_strSucheLvNr & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
            Finally
                'If intID > -1 Then
                '    m_objlogApp.WriteStandardDataAccessSAP(intID)
                'End If

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Function giveResultStructure() As DataTable
        Dim tbl As DataTable
        'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_M_Unangefordert_Arval", m_objApp, m_objUser)
        S.AP.Init("Z_M_Unangefordert_Arval")

        'tbl = myProxy.getImportTable("GT_WEB")
        tbl = S.AP.GetImportTable("GT_WEB")

        tbl.Columns.Add("STATUS", GetType(System.String))
        tbl.Columns.Add("ART", GetType(System.String))
        Return tbl

    End Function

    Public Sub getZulassungsdienste(ByRef tblSTVA As DataTable, ByRef status As String)

        Try
            'If m_objLogApp Is Nothing Then
            '    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            'End If

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", m_objApp, m_objUser, page)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_Get_Zulst_By_Plz")

            'tblSTVA = myProxy.getExportTable("T_ZULST")
            tblSTVA = S.AP.GetExportTable("T_ZULST")

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
            status = m_strMessage

        End Try
    End Sub

    Private Function RemoveSingleSpace(ByVal strIn As String) As String
        Dim strReturn As String = ""
        Try
            Dim strOut As String = strIn.Trim(" "c)
            If strOut = "&nbsp;" Then
                strOut = ""
            End If
            strReturn = strOut
        Catch
        End Try
        Return strReturn
    End Function

    Private Sub AnfordernSAP()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'If m_objLogApp Is Nothing Then
            '    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            'End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_M_Briefanforderung_Allg", m_objApp, m_objUser)

                'Dim strDaten As String = "abckz=" & m_abckz & _
                '   ", strSucheFahrgestellNr=" & m_strSucheFgstNr & _
                '   ", equ=" & m_equ & _
                '   ", strZielHNr=" & m_strZielHNr & _
                '   ", KUNNR=" & KUNNR & _
                '   ", versandadr_ZE=" & m_versandadr_ZE & _
                '   ", versandadr_ZS=" & m_versandadr_ZS & _
                '   ", strZielLand=" & m_strZielLand & _
                '   ", kennz=" & m_kennz & _
                '   ", liznr=" & m_liznr & _
                '   ", material=" & m_material & _
                '   ", strZielFirma=" & m_strZielFirma & _
                '   ", strZielFirma2=" & m_strZielFirma2 & _
                '   ", strZielOrt=" & m_strZielOrt & _
                '   ", strZielPLZ=" & m_strZielPLZ & _
                '   ", strZielStrasse=" & m_strZielStrasse & _
                '   ", tidnr=" & m_tidnr & _
                '   ", versgrund=" & m_versgrund & _
                '   ", strAuf=" & m_strAuf & _
                '   ", strBetreff=" & m_strBetreff

                'm_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Briefanforderung_Allg", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                m_strZielLand = "DE"

                'myProxy.setImportParameter("I_KUNNR_AG", KUNNR)
                'myProxy.setImportParameter("I_KUNNR_ZE", RemoveSingleSpace(m_versandadr_ZE))
                'myProxy.setImportParameter("I_KUNNR_ZS", RemoveSingleSpace(m_versandadr_ZS))
                'myProxy.setImportParameter("I_ABCKZ", m_abckz)
                'myProxy.setImportParameter("I_EQUNR", RemoveSingleSpace(m_equ))
                'myProxy.setImportParameter("I_ERNAM", m_objUser.UserName)
                'myProxy.setImportParameter("I_CHASSIS_NUM", RemoveSingleSpace(m_strSucheFgstNr))
                'myProxy.setImportParameter("I_LICENSE_NUM", RemoveSingleSpace(m_kennz))
                'myProxy.setImportParameter("I_TIDNR", RemoveSingleSpace(m_tidnr))
                'myProxy.setImportParameter("I_LIZNR", RemoveSingleSpace(m_liznr))
                'myProxy.setImportParameter("I_MATNR", Right("000000000000000000" & m_material, 18))
                'myProxy.setImportParameter("I_ZZVGRUND", RemoveSingleSpace(m_versgrund))
                'myProxy.setImportParameter("I_NAME1", RemoveSingleSpace(m_strZielFirma))
                'myProxy.setImportParameter("I_NAME2", RemoveSingleSpace(m_strZielFirma2))
                'myProxy.setImportParameter("I_PSTLZ", RemoveSingleSpace(m_strZielPLZ))
                'myProxy.setImportParameter("I_ORT01", RemoveSingleSpace(m_strZielOrt))
                'myProxy.setImportParameter("I_STR01", RemoveSingleSpace(m_strZielStrasse))
                'myProxy.setImportParameter("I_HOUSE", RemoveSingleSpace(m_strZielHNr))
                'myProxy.setImportParameter("I_LAND1", RemoveSingleSpace(m_strZielLand))
                'myProxy.setImportParameter("I_ZZBETREFF", RemoveSingleSpace(m_strBetreff))
                'myProxy.setImportParameter("I_ZZNAME_ZH", RemoveSingleSpace(m_strAuf))
                'myProxy.setImportParameter("I_LISTENART", "C6")

                'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                'If m_intIDsap > -1 Then
                '    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                'End If

                S.AP.Init("Z_M_Briefanforderung_Allg")


                S.AP.SetImportParameter("I_KUNNR_AG", KUNNR)
                S.AP.SetImportParameter("I_KUNNR_ZE", RemoveSingleSpace(m_versandadr_ZE))
                S.AP.SetImportParameter("I_KUNNR_ZS", RemoveSingleSpace(m_versandadr_ZS))
                S.AP.SetImportParameter("I_ABCKZ", m_abckz)
                S.AP.SetImportParameter("I_EQUNR", RemoveSingleSpace(m_equ))
                S.AP.SetImportParameter("I_ERNAM", m_objUser.UserName)
                S.AP.SetImportParameter("I_CHASSIS_NUM", RemoveSingleSpace(m_strSucheFgstNr))
                S.AP.SetImportParameter("I_LICENSE_NUM", RemoveSingleSpace(m_kennz))
                S.AP.SetImportParameter("I_TIDNR", RemoveSingleSpace(m_tidnr))
                S.AP.SetImportParameter("I_LIZNR", RemoveSingleSpace(m_liznr))
                S.AP.SetImportParameter("I_MATNR", Right("000000000000000000" & m_material, 18))
                S.AP.SetImportParameter("I_ZZVGRUND", RemoveSingleSpace(m_versgrund))
                S.AP.SetImportParameter("I_NAME1", RemoveSingleSpace(m_strZielFirma))
                S.AP.SetImportParameter("I_NAME2", RemoveSingleSpace(m_strZielFirma2))
                S.AP.SetImportParameter("I_PSTLZ", RemoveSingleSpace(m_strZielPLZ))
                S.AP.SetImportParameter("I_ORT01", RemoveSingleSpace(m_strZielOrt))
                S.AP.SetImportParameter("I_STR01", RemoveSingleSpace(m_strZielStrasse))
                S.AP.SetImportParameter("I_HOUSE", RemoveSingleSpace(m_strZielHNr))
                S.AP.SetImportParameter("I_LAND1", RemoveSingleSpace(m_strZielLand))
                S.AP.SetImportParameter("I_ZZBETREFF", RemoveSingleSpace(m_strBetreff))
                S.AP.SetImportParameter("I_ZZNAME_ZH", RemoveSingleSpace(m_strAuf))
                S.AP.SetImportParameter("I_LISTENART", "C6")

                S.AP.Execute()

                'strAuftragsnummer = myProxy.getExportParameter("E_VBELN")
                strAuftragsnummer = S.AP.GetExportParameter("E_VBELN")

                strAuftragsnummer = strAuftragsnummer.TrimStart("0"c)
                strAuftragsstatus = "Vorgang OK"

                'If m_intIDsap > -1 Then
                '    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True, Left(strDaten, 500))
                'End If
            Catch ex As Exception
                strAuftragsstatus = "Fehler"
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "ZCREDITCONTROL_ENTRY_LOCKED"
                        m_strMessage = "System ausgelastet. Bitte klicken Sie erneut auf ""Absenden""."
                        m_intStatus = -1111
                    Case "NO_UPDATE_EQUI"
                        m_strMessage = "Fehler bei der Datenspeicherung (EQUI-UPDATE)"
                        m_intStatus = -1112
                    Case "NO_AUFTRAG"
                        m_strMessage = "Kein Auftrag angelegt"
                        m_intStatus = -1113
                    Case "NO_ZDADVERSAND"
                        m_strMessage = "Keine Einträge für die Versanddatei erstellt"
                        m_intStatus = -1114
                    Case "NO_MODIFY_ILOA"
                        m_strMessage = "ILOA-MODIFY-Fehler"
                        m_intStatus = -1115
                    Case "NO_BRIEFANFORDERUNG"
                        m_strMessage = "Brief bereits angefordert"
                        m_intStatus = -1116
                    Case "NO_EQUZ"
                        m_strMessage = "Kein Brief vorhanden (EQUZ)"
                        m_intStatus = -1117
                    Case "NO_ILOA"
                        m_strMessage = "Kein Brief vorhanden (ILOA)"
                        m_intStatus = -1118
                    Case Else
                        m_strMessage = ex.Message
                        m_intStatus = -9999
                End Select
                'If m_intIDsap > -1 Then
                '    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                'End If
            Finally
                'If m_intIDsap > -1 Then
                '    m_objLogApp.LogStandardIdentity = m_intStandardLogID
                '    m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
                'End If

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Private Sub addResultTableHeader()
        If (m_tblResult Is Nothing) Then
            m_tblResult = New DataTable()
        End If
        With m_tblResult.Columns
            .Add("id")
            .Add("Erstellt")
            .Add("Benutzer")
            .Add("Equipment")
            .Add("Fahrgestellnr")
            .Add("Versandadresse")
            .Add("VersandadresseName1")
            .Add("VersandadresseName2")
            .Add("VersandadresseStr")
            .Add("VersandadresseNr")
            .Add("VersandadressePlz")
            .Add("VersandadresseOrt")
            .Add("Lvnr")
            .Add("Haendlernummer")
            .Add("Versandart")
            .Add("Kennzeichen")
            .Add("TIDNr")
            .Add("LIZNr")
            .Add("Materialnummer")
            .Add("VersandartShow")
            .Add("Status")
        End With

    End Sub

    Private Sub addResultTableRow(ByVal id As String, ByVal tstamp As String, ByVal equi As String, ByVal user As String, ByVal objData As ArrayList)
        Dim row As DataRow

        row = m_tblResult.NewRow
        row("id") = id
        row("Erstellt") = tstamp
        row("Benutzer") = user
        row("Equipment") = equi
        If objData(11) Is Nothing Then
            row("Lvnr") = ""
        Else
            row("Lvnr") = objData(11)
        End If
        If objData(10) Is Nothing Then
            row("Fahrgestellnr") = ""
        Else
            row("Fahrgestellnr") = objData(10)
        End If
        If objData(3) Is Nothing Then
            row("VersandadresseName1") = ""
            row("Versandadresse") = ""
        Else
            row("VersandadresseName1") = objData(3)
            row("Versandadresse") = objData(3).ToString
        End If
        If objData(4) Is Nothing Then
            row("VersandadresseName2") = ""
        Else
            row("VersandadresseName2") = objData(4)
        End If
        If objData(5) Is Nothing Then
            row("VersandadresseStr") = ""
            row("Versandadresse") = CStr(row("Versandadresse")) & "<br>" & " "
        Else
            row("VersandadresseStr") = objData(5)
            row("Versandadresse") = CStr(row("Versandadresse")) & "<br>" & objData(5).ToString
        End If
        If objData(6) Is Nothing Then
            row("VersandadresseNr") = ""
            row("Versandadresse") = CStr(row("Versandadresse")) & "&nbsp;" & " "
        Else
            row("VersandadresseNr") = objData(6)
            row("Versandadresse") = CStr(row("Versandadresse")) & "&nbsp;" & objData(6).ToString
        End If
        If objData(7) Is Nothing Then
            row("VersandadressePlz") = ""
            row("Versandadresse") = CStr(row("Versandadresse")) & ", " & " "
        Else
            row("VersandadressePlz") = objData(7)
            row("Versandadresse") = CStr(row("Versandadresse")) & ", " & objData(7).ToString
        End If
        If objData(8) Is Nothing Then
            row("VersandadresseOrt") = ""
            row("Versandadresse") = CStr(row("Versandadresse")) & "&nbsp;" & " "
        Else
            row("VersandadresseOrt") = objData(8)
            row("Versandadresse") = CStr(row("Versandadresse")) & "&nbsp;" & objData(8).ToString
        End If
        'row("Versandadresse") = objData(3).ToString & "<br>" & objData(5).ToString & "&nbsp;" & objData(6).ToString & "," & objData(7).ToString & "&nbsp;" & objData(8).ToString
        If objData(0) Is Nothing Then
            row("Haendlernummer") = ""
        Else
            row("Haendlernummer") = objData(0)
        End If
        If objData(22) Is Nothing Then
            row("Kennzeichen") = ""
        Else
            row("Kennzeichen") = objData(22)
        End If
        If objData(23) Is Nothing Then
            row("TIDNr") = ""
        Else
            row("TIDNr") = objData(23)
        End If
        row("LIZNr") = objData(24)
        If objData(18) Is Nothing Then
            row("Materialnummer") = "1391"
        Else
            row("Materialnummer") = objData(18)
        End If
        Select Case CStr(row("Materialnummer"))
            Case "1391"
                row("VersandartShow") = "innerhalb von 24 bis 48 h"
            Case "1385"
                row("VersandartShow") = "vor 9:00 Uhr"
            Case "1389"
                row("VersandartShow") = "vor 10:00 Uhr"
            Case "1390"
                row("VersandartShow") = "vor 12:00 Uhr"
        End Select
        row("Versandart") = "2"                 'Endgültig
        m_tblResult.Rows.Add(row)
        m_tblResult.AcceptChanges()
    End Sub

    Public Sub getAutorizationData(ByRef status As String)
        Dim formatter As System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim cn As New SqlClient.SqlConnection
        Dim cmdOutPut As SqlClient.SqlCommand
        Dim objData As ArrayList
        Dim ms As MemoryStream
        Dim drAppData As SqlClient.SqlDataReader
        Dim intI As Int32 = 4

        addResultTableHeader()

        Try
            cn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            cn.Open()

            cmdOutPut = New SqlClient.SqlCommand()
            cmdOutPut.Connection = cn
            cmdOutPut.CommandText = "SELECT * FROM AuthorizationARVAL"
            drAppData = cmdOutPut.ExecuteReader

            While drAppData.Read()
                If Not drAppData Is Nothing Then
                    If Not TypeOf drAppData(intI) Is System.DBNull Then
                        ' 1. Daten als bytearray aus der DB lesen:
                        Dim bytData(CInt(drAppData.GetBytes(intI, 0, Nothing, 0, Integer.MaxValue - 1) - 1)) As Byte
                        drAppData.GetBytes(intI, 0, bytData, 0, bytData.Length)
                        ' Dataset über einen Memory Stream aus dem ByteArray erzeugen:
                        Dim stmAppData As MemoryStream
                        stmAppData = New MemoryStream(bytData)
                        ms = stmAppData
                        formatter = New BinaryFormatter()
                        objData = DirectCast(formatter.Deserialize(ms), ArrayList)
                        addResultTableRow(CType(drAppData("id"), String), CType(drAppData("tstamp"), String), CType(drAppData("equipment"), String),
                                          CType(drAppData("username"), String), objData)
                    Else
                        ms = Nothing
                    End If
                Else
                    ms = Nothing
                End If
            End While
        Catch ex As Exception
            Throw ex
        Finally
            cn.Close()
            cn.Dispose()
        End Try
    End Sub

    'Private Sub writedb(ByVal username As String, ByVal equipment As String, ByVal objectData As MemoryStream, ByRef status As String)
    '    Dim connection As New SqlClient.SqlConnection
    '    Dim para As SqlClient.SqlParameter

    '    Dim command As New SqlClient.SqlCommand()
    '    Dim sqlInsert As String
    '    Dim b As Byte()

    '    Try
    '        connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

    '        sqlInsert = "INSERT INTO AuthorizationARVAL (username,equipment, data) VALUES (@user,@equi,@data)"

    '        With command
    '            .Connection = connection
    '            .CommandType = CommandType.Text
    '            .CommandText = sqlInsert
    '            .Parameters.Clear()
    '        End With

    '        b = objectData.ToArray
    '        para = New SqlClient.SqlParameter("@data", SqlDbType.Image, b.Length, ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, b)
    '        With command.Parameters
    '            .AddWithValue("@user", username)
    '            .AddWithValue("@equi", equipment)
    '            .Add(para)
    '        End With
    '        connection.Open()
    '        command.ExecuteNonQuery()
    '        status = String.Empty
    '    Catch ex As Exception
    '        status = ex.Message
    '    Finally
    '        connection.Close()
    '        connection.Dispose()
    '    End Try
    'End Sub

    Public Sub clearDB(ByVal id As Int32, ByRef status As String)
        Dim connection As New SqlClient.SqlConnection
        Dim command As New SqlClient.SqlCommand()
        Dim sqlInsert As String

        Try
            connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")

            sqlInsert = "DELETE FROM AuthorizationARVAL WHERE id = @id"

            With command
                .Connection = connection
                .CommandType = CommandType.Text
                .CommandText = sqlInsert
                .Parameters.Clear()
            End With

            With command.Parameters
                .AddWithValue("@id", id)
            End With
            connection.Open()
            command.ExecuteScalar()
            status = String.Empty
        Catch ex As Exception
            status = ex.Message
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Sub

    Public Sub readAllAuthorizationSets(ByRef resultTable As DataTable, ByRef status As String)
        Dim connection As New SqlClient.SqlConnection
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim sqlInsert As String

        Try
            connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            sqlInsert = "SELECT * FROM AuthorizationARVAL"

            With command
                .Connection = connection
                .CommandType = CommandType.Text
                .CommandText = sqlInsert
                .Parameters.Clear()
            End With

            connection.Open()
            adapter.SelectCommand = command
            adapter.Fill(resultTable)

            status = String.Empty
        Catch ex As Exception
            status = ex.Message
        Finally
            connection.Close()
            connection.Dispose()
        End Try
    End Sub

    Public Sub Anfordern(ByVal art As String)

        AnfordernSAP()

    End Sub
#End Region
End Class

' ************************************************
' $History: SixtLease_02.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 11.03.10   Time: 14:52
' Updated in $/CKAG/Applications/appsixtl/Lib
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 5.03.10    Time: 11:24
' Updated in $/CKAG/Applications/appsixtl/Lib
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 4.03.10    Time: 12:59
' Updated in $/CKAG/Applications/appsixtl/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:47
' Updated in $/CKAG/Applications/appsixtl/Lib
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:27
' Created in $/CKAG/Applications/appsixtl/Lib
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 6.12.07    Time: 14:46
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' ITA : 1440
' 
' *****************  Version 11  *****************
' User: Uha          Date: 3.07.07    Time: 9:34
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 10  *****************
' User: Uha          Date: 8.03.07    Time: 13:42
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' 
' ************************************************
