Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class kruell_03
    REM § Lese-/Schreibfunktion, Kunde: Kruell, 
    REM § Show - BAPI: Z_M_AUFTRDAT_UEBERF_001
    REM § Change - BAPI: Z_M_AUFTRDAT_UEBERF_002

    Inherits CKG.Base.Business.DatenimportBase

#Region " Declarations"
    Dim strSelMitStoerungsmeldung As String = ""
    Dim strSelMitEingangKMC As String = ""
    Dim strSelOhneEingangKMC As String = ""
    Dim strSelOrderNummer As String = ""
    Dim strSelFahrgestellnummer As String = ""

    Dim strOrderNummer As String = ""
    Dim strFahrgestellnummer As String = ""
    Dim strStoerungsmeldung As String = ""
    Dim strEingangAm As String = ""
    Dim strErledigt As String = ""
    Dim strGeplantesFertigstellungsdatum As String = ""
    Dim strDatStoerungsmeldung As String = ""




#End Region

#Region " Properties"
    Public Property mitStoerungsmeldung() As String
        Get
            Return strSelMitStoerungsmeldung
        End Get
        Set(ByVal Value As String)
            strSelMitStoerungsmeldung = Value
        End Set
    End Property



    Public Property Eingang() As String
        Get
            Return strEingangAm
        End Get
        Set(ByVal Value As String)
            strEingangAm = Value
        End Set
    End Property

    Public Property Erledigt() As String
        Get
            Return strErledigt
        End Get
        Set(ByVal Value As String)
            strErledigt = Value
        End Set
    End Property


    Public Property Stoerungsmeldung() As String
        Get
            Return strStoerungsmeldung
        End Get
        Set(ByVal Value As String)
            strStoerungsmeldung = Value
        End Set
    End Property

    Public Property mitEingangKMC() As String
        Get
            Return strSelMitEingangKMC
        End Get
        Set(ByVal Value As String)
            strSelMitEingangKMC = Value
        End Set
    End Property

    Public Property ohneEingangKMC() As String
        Get
            Return strSelOhneEingangKMC
        End Get
        Set(ByVal Value As String)
            strSelOhneEingangKMC = Value
        End Set
    End Property

    Public Property ordernummer() As String
        Get
            Return strOrderNummer
        End Get
        Set(ByVal Value As String)
            If Not Value Is Nothing Then
                strOrderNummer = Value.Trim(" "c).ToUpper
            End If
        End Set
    End Property


    Public Property fahrgestellnummer() As String
        Get
            Return strFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            strFahrgestellnummer = Value
        End Set
    End Property

    Public Property selOrdernummer() As String
        Get
            Return strSelOrderNummer
        End Get
        Set(ByVal Value As String)
            If Not Value Is Nothing Then
                strSelOrderNummer = Value.Trim(" "c).ToUpper
            End If
        End Set
    End Property


    Public Property selFahrgestellnummer() As String
        Get
            Return strSelFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            strSelFahrgestellnummer = Value
        End Set
    End Property


    Public Property geplFertigstellungsdatum() As String
        Get
            Return strGeplantesFertigstellungsdatum
        End Get
        Set(ByVal Value As String)
            strGeplantesFertigstellungsdatum = Value
        End Set
    End Property


    Public Property datumStoerungsmeldung() As String
        Get
            Return strDatStoerungsmeldung
        End Get
        Set(ByVal Value As String)
            strDatStoerungsmeldung = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App)
        MyBase.New(objUser, objApp, "")

    End Sub

    'Public Overloads Overrides Sub Fill()

    '    m_strClassAndMethod = "kruell_03.fill"
    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True

    '        'alte Resulttabelle Clearen, da bei einer exception noch daten darin enthalten bleiben JJ2008.1.14
    '        If Not Result Is Nothing Then
    '            Result.Clear()
    '        End If


    '        Dim objSAP As New SAPProxy_Kruell.SAPProxy_Kruell()

    '        'MakeDestination()
    '        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
    '        objSAP.Connection.Open()

    '        If m_objLogApp Is Nothing Then
    '            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
    '        End If

    '        m_intStatus = -1

    '        Try
    '            m_intStatus = 0
    '            m_strMessage = ""


    '            Dim sapTable As New SAPProxy_Kruell.ZDAD_AUFTR_UEBERF_001Table()
    '            m_intStatus = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Abweich_abrufgrund", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
    '            objSAP.Z_M_Auftrdat_Ueberf_001(selFahrgestellnummer, Right("0000000000" & m_objUser.Customer.KUNNR, 10), mitEingangKMC, mitStoerungsmeldung, ohneEingangKMC, selOrdernummer, sapTable)
    '            objSAP.CommitWork()
    '            If m_intStatus > -1 Then
    '                m_objLogApp.WriteEndDataAccessSAP(m_intStatus, True)
    '            End If


    '            CreateOutPut(sapTable.ToADODataTable, Me.AppID)

    '            'WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", LICENSE_NUM=" & m_strINKennzeichen.ToUpper & ", LIZNR=" & m_strINVertragsnummer.ToUpper & ", CHASSIS_NUM=" & m_strINFahrgestellnummer.ToUpper, m_tblResult)
    '        Catch ex As Exception
    '            Select Case ex.Message
    '                Case "NO_DATA"
    '                    m_intStatus = -8711
    '                    m_strMessage = "Keine Daten vorhanden"
    '                Case Else
    '                    m_intStatus = -9999
    '                    m_strMessage = ex.Message
    '            End Select
    '            If m_intStatus > -1 Then
    '                m_objLogApp.WriteEndDataAccessSAP(m_intStatus, False, m_strMessage)
    '            End If
    '            'WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", LICENSE_NUM=" & m_strINKennzeichen.ToUpper & ", LIZNR=" & m_strINVertragsnummer.ToUpper & ", CHASSIS_NUM=" & m_strINFahrgestellnummer.ToUpper & " , " & Replace(m_strMessage, "<br>", " "), m_tblResult)
    '        Finally
    '            If m_intStatus > -1 Then
    '                m_objLogApp.WriteStandardDataAccessSAP(m_intStatus)
    '            End If

    '            objSAP.Connection.Close()
    '            objSAP.Dispose()

    '            m_blnGestartet = False
    '        End Try
    '    End If
    'End Sub
    Public Sub Fill()

    End Sub
    Public Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        m_intStatus = 0

        Try
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Auftrdat_Ueberf_001", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.Customer.KUNNR, 10))
            myProxy.setImportParameter("I_ORDER_NR", selOrdernummer)
            myProxy.setImportParameter("I_CHASSIS_NUM", selFahrgestellnummer)
            myProxy.setImportParameter("I_OHNE_EINGDAT", ohneEingangKMC)
            myProxy.setImportParameter("I_MIT_EINGDAT", mitEingangKMC)
            myProxy.setImportParameter("I_MIT_STOERMEL", mitStoerungsmeldung)


            myProxy.callBapi()


            Dim tempTable As DataTable = myProxy.getExportTable("GT_WEB")

            CreateOutPut(tempTable, Me.AppID)

        Catch ex As Exception
            m_intStatus = -9999
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Daten vorhanden. "
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
        End Try

    End Sub


    Public Sub Change()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_Kruell.SAPProxy_Kruell()

            'MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intStatus = -1

            Try
                m_intStatus = 0
                m_strMessage = ""


                m_intStatus = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_ueberf_002", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                Dim sapOutputTable As New SAPProxy_Kruell.ZDAD_AUFTR_UEBERF_002Table()
                Dim sapOutputTableRow As New SAPProxy_Kruell.ZDAD_AUFTR_UEBERF_002()

                'werte setzen

                If Not Eingang.Trim Is String.Empty Then
                    sapOutputTableRow.Dat_Fzgeing_Kmc = MakeDateSAP(Eingang)
                End If

                If Not geplFertigstellungsdatum.Trim Is String.Empty Then
                    sapOutputTableRow.Gepl_Dat_Smel_Ab = MakeDateSAP(geplFertigstellungsdatum)
                End If

                If Not datumStoerungsmeldung.Trim Is String.Empty Then
                    sapOutputTableRow.Dat_Smel_Aufber = MakeDateSAP(datumStoerungsmeldung)
                End If

                sapOutputTableRow.Order_Nr = ordernummer
                sapOutputTableRow.Wert_Smel_Aufbe = Stoerungsmeldung

                sapOutputTable.Add(sapOutputTableRow)

                objSAP.Z_M_Auftrdat_Ueberf_002(Right("0000000000" & m_objUser.KUNNR, 10), sapOutputTable)
                objSAP.CommitWork()

                If m_intStatus > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intStatus, True)
                End If

                m_strMessage = "Vorgang für Ordernummer: <b>" & ordernummer & "</b> erfolgreich abgeschlossen"
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_UPDATE_EQUI"
                        m_intStatus = -8703
                        m_strMessage = "Fehler bei der Datenspeicherung "
                    Case "NO_DATA"
                        m_intStatus = -8704
                        m_strMessage = "Keine Equipmentdaten gefunden"
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intStatus > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intStatus, False, m_strMessage)
                End If
            Finally
                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_intStatus = 0

        Try
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_AUFTRDAT_UEBERF_002", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.Customer.KUNNR, 10))


            Dim tempTable As DataTable = myProxy.getImportTable("GT_WEB")

            Dim newRow As DataRow = tempTable.NewRow

            newRow("ORDER_NR") = ordernummer
            newRow("DAT_FZGEING_KMC") = Eingang
            newRow("DAT_SMEL_AUFBER") = datumStoerungsmeldung
            newRow("WERT_SMEL_AUFBE") = Stoerungsmeldung
            newRow("GEPL_DAT_SMEL_AB") = geplFertigstellungsdatum

            myProxy.callBapi()

            m_strMessage = "Vorgang für Ordernummer: <b>" & ordernummer & "</b> erfolgreich abgeschlossen"

        Catch ex As Exception
            m_intStatus = -9999
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_UPDATE_EQUI"
                    m_intStatus = -8703
                    m_strMessage = "Fehler bei der Datenspeicherung "
                Case "NO_DATA"
                    m_intStatus = -8704
                    m_strMessage = "Keine Equipmentdaten gefunden"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
        End Try
    End Sub
#End Region
End Class
' ************************************************
' $History: kruell_03.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:37
' Created in $/CKAG/Applications/AppKruell/Lib
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 7.03.08    Time: 10:22
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' Ordernummer in Großbuchstaben umsetzen
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 23.01.08   Time: 14:32
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 23.01.08   Time: 14:02
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' testfertig ITA 1580
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 16.01.08   Time: 18:10
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' in bearbeitung
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 16.01.08   Time: 16:48
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' in bearbeitung
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 16.01.08   Time: 11:01
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' in bearbeitung
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 15.01.08   Time: 17:31
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' in bearbeitung
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 15.01.08   Time: 15:15
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' in bearbeitung
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 15.01.08   Time: 11:46
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' Script erzeugen?
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 15.01.08   Time: 10:46
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' ITA 1580, in bearbeitung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 14.01.08   Time: 17:11
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 9.01.08    Time: 14:33
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 9.01.08    Time: 14:29
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' ITA 1580 Torso
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 9.01.08    Time: 13:57
' Created in $/CKG/Applications/AppKruell/AppKruellWeb/Lib
' ITA 1580 Report01 hinzugefügt, SS History Bodys hinzugefügt
' ************************************************
