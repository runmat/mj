Option Explicit On
Option Infer On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel.Common

Public Class Versand
    Inherits BankBase
#Region " Declarations"

    Private configurationAppSettings As System.Configuration.AppSettingsReader
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

    Private m_strAuf As String
    Private m_strBetreff As String
    Private m_blnLeaseplanZulassung As Boolean
    Private m_strSucheFahrgestellNr As String
    Private m_strSucheKennzeichen As String
    Private m_strSucheLeasingvertragsNr As String
    Private m_strSucheNummerZB2 As String
    Private m_kbanr As String
    Private m_zulkz As String
    Private m_Fahrzeuge As Int32
    Private m_tableGrund As DataTable
    Private m_versandadr_ZE As String
    Private m_versandadr_ZS As String
    Private m_versandadrtext As String
    Private m_versicherung As String
    Private m_material As String
    Private m_schein As String
    Private m_abckz As String
    Private m_equ As String
    Private m_kennz As String
    Private m_tidnr As String
    Private m_liznr As String
    Private m_unitnr As String
    Private m_versgrund As String
    Private m_strListenart As String
    Private m_versgrundText As String
    Private strAuftragsstatus As String
    Private strAuftragsnummer As String
    Private dataArray As ArrayList
    Private rowToChange As DataRow
    Private m_IsCustomer As Boolean
    Private m_tblAdressen As DataTable
    Private strMaterialnummernBezeichnung As String
    Private m_strAdressNummer As String
    Private m_tblLaender As DataTable
    Private m_EquiTyp As String

    Private mE_SUBRC As String = ""
    Private mE_MESSAGE As String = ""
#End Region

#Region " Properties"
    Public ReadOnly Property Laender() As DataTable
        Get
            If m_tblLaender Is Nothing Then
                getLaender()
            End If
            Return m_tblLaender
        End Get
    End Property

    Public ReadOnly Property AdressNummer() As String
        Get
            Return m_strAdressNummer
        End Get
    End Property

    Public Property MaterialBezeichnung() As String
        Get
            Return strMaterialnummernBezeichnung
        End Get
        Set(ByVal Value As String)
            strMaterialnummernBezeichnung = Value
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

    Public Property IsBooCustomerGroup() As Boolean
        Get
            Return m_IsCustomer
        End Get
        Set(ByVal Value As Boolean)
            m_IsCustomer = Value
        End Set
    End Property


    Public Property Betreff() As String
        Get
            Return m_strBetreff
        End Get
        Set(ByVal Value As String)
            m_strBetreff = Value
        End Set
    End Property

    Public ReadOnly Property Adressen() As DataTable
        Get
            Return m_tblAdressen
        End Get
    End Property

    Public Property Versicherung() As String
        Get
            Return m_versicherung
        End Get
        Set(ByVal Value As String)
            m_versicherung = Value
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

    Public Property SucheKennzeichen() As String
        Get
            Return m_strSucheKennzeichen
        End Get
        Set(ByVal Value As String)
            m_strSucheKennzeichen = Value
        End Set
    End Property

    Public Property SucheLeasingvertragsNr() As String
        Get
            Return m_strSucheLeasingvertragsNr
        End Get
        Set(ByVal Value As String)
            m_strSucheLeasingvertragsNr = Value
        End Set
    End Property

    Public Property SucheNummerZB2() As String
        Get
            Return m_strSucheNummerZB2
        End Get
        Set(ByVal Value As String)
            m_strSucheNummerZB2 = Value
        End Set
    End Property

    Public Property SucheFahrgestellNr() As String
        Get
            Return m_strSucheFahrgestellNr
        End Get
        Set(ByVal Value As String)
            m_strSucheFahrgestellNr = Value
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

    Public Property EquiTyp() As String
        Get
            Return m_EquiTyp
        End Get
        Set(ByVal Value As String)
            m_EquiTyp = Value
        End Set
    End Property

    Public Property UnitNr() As String
        Get
            Return m_unitnr
        End Get
        Set(ByVal Value As String)
            m_unitnr = Value
        End Set
    End Property
    Public Property Listenart() As String
        Get
            Return m_strListenart
        End Get
        Set(ByVal Value As String)
            m_strListenart = Value
        End Set
    End Property
#End Region
#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_blnLeaseplanZulassung = False
        'If objUser.Reference = String.Empty Then
        '    m_blnLeaseplanZulassung = True
        'End If
    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overrides Sub Change()

    End Sub


    Private Function SuggestionDay() As DateTime
        Dim datTemp As DateTime = Now
        Dim intAddDays As Int32 = 0
        Do While datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday Or intAddDays < 3
            datTemp = datTemp.AddDays(1)
            If Not (datTemp.DayOfWeek = DayOfWeek.Saturday Or datTemp.DayOfWeek = DayOfWeek.Sunday) Then
                intAddDays += 1
            End If
        Loop
        Return datTemp
    End Function

    Public Sub ClearResultTable()
        m_tblResult = Nothing
    End Sub

    Public Sub GiveCars(ByVal strAppID As String, _
                            ByVal strSessionID As String, _
                            ByVal page As Page)

        m_strClassAndMethod = "GiveCars"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_UNANGEFORDERT_005", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR", strKUNNR)
                myProxy.setImportParameter("I_EQTYP", m_EquiTyp)
                myProxy.setImportParameter("I_LICENSE_NUM ", m_strSucheKennzeichen)
                myProxy.setImportParameter("I_CHASSIS_NUM", m_strSucheFahrgestellNr)
                myProxy.setImportParameter("I_ZZREFERENZ1", m_unitnr)
                myProxy.callBapi()

                m_tblResult = New DataTable

                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

                m_tableGrund = myProxy.getExportTable("GT_GRU")

                m_tblResult = myProxy.getExportTable("GT_WEB")
                m_tblResult.Columns.Add("STATUS", GetType(System.String))
                m_tblResult.Columns("STATUS").MaxLength = 25
                m_tblResult.AcceptChanges()
                Dim tmpRow As DataRow
                For Each tmpRow In m_tblResult.Rows
                    If tmpRow("STATUS").ToString = "B" Then
                        tmpRow("STATUS") = "beauftragt"
                    Else
                        tmpRow("STATUS") = ""
                    End If
                Next
                m_tblResult.AcceptChanges()

                If mE_SUBRC <> "0" Then
                    m_intStatus = CInt(mE_SUBRC)
                    m_strMessage = "Keine Daten gefunden!"
                End If

            Catch ex As Exception
                m_tblResult = Nothing
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
    Public Sub getZulassungsdienste(ByVal strAppID As String, _
                            ByVal strSessionID As String, _
                            ByVal page As Web.UI.Page, _
                            ByRef tblSTVA As DataTable, _
                            ByRef status As String)


        m_strClassAndMethod = "getZulassungsdienste"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", m_objApp, m_objUser, page)

                myProxy.callBapi()

                tblSTVA = myProxy.getExportTable("T_ZULST")


            Catch ex As Exception
                m_tblResult = Nothing
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub
    Private Sub getLaender()
        configurationAppSettings = New AppSettingsReader()

        Dim intID As Int32 = -1
        Try

            If m_objLogApp Is Nothing Then
                m_objLogApp = New CKG.Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_Get_Zulst_By_Plz", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

            Dim proxy = DynSapProxy.getProxy("Z_M_LAND_PLZ_001", m_objApp, m_objUser, PageHelper.GetCurrentPage())

            proxy.callBapi()

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

            m_tblLaender = proxy.getExportTable("GT_WEB")
            m_tblLaender.Columns.Add("Beschreibung", GetType(String))
            m_tblLaender.Columns.Add("FullDesc", GetType(String))

            For Each rowTemp As DataRow In m_tblLaender.Rows
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
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "FahrgestellNr=" & m_strSucheFahrgestellNr & ", LVNr.=" & m_strSucheLeasingvertragsNr & ", KfzKz.=" & m_strSucheKennzeichen & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            m_blnGestartet = False
        End Try
    End Sub
    Public Sub Anfordern(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Try
                m_intStatus = 0
                m_strMessage = ""
                strAuftragsstatus = ""
                strAuftragsnummer = ""
                Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BRIEFANFORDERUNG_ALLG", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                myProxy.setImportParameter("I_KUNNR_ZF", "")
                myProxy.setImportParameter("I_KUNNR_ZH", strKUNNR)
                myProxy.setImportParameter("I_KUNNR_ZE", m_versandadr_ZE)
                myProxy.setImportParameter("I_ABCKZ", m_abckz)
                myProxy.setImportParameter("I_EQUNR", m_equ)
                myProxy.setImportParameter("I_ERNAM", Left(m_objUser.UserName, 13))
                myProxy.setImportParameter("I_CHASSIS_NUM", m_strSucheFahrgestellNr)
                myProxy.setImportParameter("I_LICENSE_NUM", m_kennz)
                myProxy.setImportParameter("I_TIDNR", m_tidnr)
                myProxy.setImportParameter("I_LIZNR", m_liznr)
                myProxy.setImportParameter("I_MATNR", Right("000000000000000000" & m_material, 18))
                myProxy.setImportParameter("I_ZZVGRUND", m_versgrund)
                myProxy.setImportParameter("I_LISTENART", m_strListenart)
                myProxy.setImportParameter("I_KUNNR_ZS", m_versandadr_ZS)
                myProxy.setImportParameter("I_NAME1", m_strZielFirma)
                myProxy.setImportParameter("I_NAME2", m_strZielFirma2)
                myProxy.setImportParameter("I_PSTLZ", m_strZielPLZ)
                myProxy.setImportParameter("I_ORT01", m_strZielOrt)
                myProxy.setImportParameter("I_STR01", m_strZielStrasse)
                myProxy.setImportParameter("I_HOUSE", m_strZielHNr)
                myProxy.setImportParameter("I_LAND1", "DE")
                myProxy.callBapi()

                strAuftragsnummer = myProxy.getExportParameter("E_VBELN")
                strAuftragsnummer = strAuftragsnummer.TrimStart("0"c)
                If strAuftragsstatus.Length = 0 Then
                    strAuftragsstatus = "Vorgang OK"
                End If

                If strAuftragsnummer.Length = 0 Then
                    m_intStatus = -2100
                    m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
                    strAuftragsstatus = "Fehler"
                End If
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "ZCREDITCONTROL_ENTRY_LOCKED"
                        m_strMessage = "System ausgelastet. Bitte clicken Sie erneut auf ""Absenden""."
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
                    Case "EQUI_SPERRE"
                        m_strMessage = "Vorgang in Bearbeitung"
                        m_intStatus = -1119
                    Case Else
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        m_intStatus = -9999
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
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

    Public Sub LeseAdressenSAP(ByVal strAppID As String, _
                            ByVal strSessionID As String, _
                            ByVal page As Page, Optional ByVal SORTL As String = "ADAC_T")

        m_strClassAndMethod = "Versankeys.LeseAdressenSAP"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_CUSTOMER_GET_CHILDREN", m_objApp, m_objUser, page)

                myProxy.setImportParameter("CUSTHITYP", "A")
                myProxy.setImportParameter("CUSTOMERNO", Right("0000000000" & m_objUser.Customer.KUNNR, 10))
                myProxy.setImportParameter("NODE_LEVEL", "00")
                myProxy.setImportParameter("I_SORTL", SORTL)

                myProxy.callBapi()

                m_tblAdressen = myProxy.getExportTable("GT_ADRESSEN")

                m_tblAdressen.Columns.Add("ADDRESSNUMBER", GetType(String))
                m_tblAdressen.Columns.Add("DISPLAY_ADDRESS", GetType(String))


                For Each row As DataRow In m_tblAdressen.Rows

                    Dim strTemp = row("NAME1").ToString

                    If row("NAME2").ToString.Length > 0 Then
                        strTemp &= ", " & row("NAME2").ToString
                    End If
                    If row("NAME3").ToString.Length > 0 Then
                        strTemp &= ", " & row("NAME3").ToString
                    End If
                    If row("NAME4").ToString.Length > 0 Then
                        strTemp &= ", " & row("NAME4").ToString
                    End If

                    row("DISPLAY_ADDRESS") = strTemp & _
                                                            ", " & row("COUNTRYISO").ToString & _
                                                            " - " & row("POST_CODE1").ToString & _
                                                            " " & row("CITY1").ToString & _
                                                            ", " & row("STREET").ToString & _
                                                            " " & row("HOUSE_NUM1").ToString

                    row("ADDRESSNUMBER") = row("KUNNR").ToString
                    m_tblAdressen.AcceptChanges()
                Next


                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

                If mE_SUBRC <> "0" Then
                    m_intStatus = CInt(mE_SUBRC)
                    m_strMessage = mE_MESSAGE
                End If

            Catch ex As Exception
                m_tblAdressen = Nothing
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub
    Public Function GiveResultStructure() As DataTable
        Dim tblTemp As DataTable
        tblTemp = m_tblResult.Clone
        Return tblTemp
    End Function
#End Region


End Class
