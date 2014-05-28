Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Business.BankBase
Imports CKG.Base.Common


Public Class Porsche_01
    REM § BAPI: Z_M_Fahrzeugbriefhistorie (FillHistory),
    REM § Direkte Rückgabe (FillHistory).

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_strGueltgkeitsdatum As String
    Private m_strHierarchietyp As String
    Private m_strKnotenlevel As String
    Private m_strKundennr As String
    Private m_strHaendler As String
    Private m_strStichtag As String
    Private selectedValue As String
    Private tblResult As DataTable
#End Region

#Region " Properties"
    Public Property PResult() As DataTable
        Set(ByVal Value As DataTable)
            tblResult = Value
        End Set
        Get
            Return tblResult
        End Get
    End Property

    Public Property PSelection() As String
        Set(ByVal Value As String)
            selectedValue = Value
        End Set
        Get
            Return selectedValue
        End Get
    End Property

    Public Property PHaendler() As String
        Set(ByVal Value As String)
            m_strHaendler = Value
        End Set
        Get
            Return m_strHaendler
        End Get
    End Property

    Public Property Stichtag() As String
        Set(ByVal Value As String)
            m_strStichtag = CDate(Value).ToShortDateString
        End Set
        Get
            Return m_strStichtag
        End Get
    End Property

    Public Property PKnotenlevel() As String
        Set(ByVal Value As String)
            m_strKnotenlevel = Value
        End Set
        Get
            Return m_strKnotenlevel
        End Get
    End Property

    Public Property PKundennr() As String
        Set(ByVal Value As String)
            m_strKundennr = Value
        End Set
        Get
            Return m_strKundennr
        End Get
    End Property

    Public Property PGueltigkeit() As String
        Set(ByVal Value As String)
            m_strGueltgkeitsdatum = Value
        End Set
        Get
            Return m_strGueltgkeitsdatum
        End Get
    End Property

    Public Property PHierarchie() As String
        Set(ByVal Value As String)
            m_strHierarchietyp = Value
        End Set
        Get
            Return m_strHierarchietyp
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Sub getHaendler(ByVal strHaendlerNummer As String, ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Porsche_01.getHaendler"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Cust_Get_Children_Porsche", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            myProxy.setImportParameter("I_WEB_REPORT", "Report01.aspx")

            myProxy.callBapi()

            m_tblResult = myProxy.getExportTable("GT_WEB")
            m_tblResult.Columns.Add("Addresse", GetType(System.String))

            tblResult = m_tblResult

            Dim row As DataRow
            Dim strAddresse As String
            Dim intCount As Integer
            Dim strHaendler As String

            strAddresse = String.Empty

            For Each row In m_tblResult.Rows
                strAddresse = CType(row("NAME1"), String)
                If CType(row("NAME2"), String) <> String.Empty Then
                    strAddresse &= " - " & CType(row("NAME2"), String)
                End If
                If CType(row("ORT01"), String) <> String.Empty Then
                    strAddresse &= " - " & CType(row("ORT01"), String)
                End If
                row("Addresse") = strAddresse
            Next

            If Not (strHaendlerNummer = String.Empty) Then
                For intCount = tblResult.Rows.Count - 1 To 0 Step -1
                    strHaendler = Right(CStr(tblResult.Rows(intCount)("KUNNR_ZF")), 7)
                    If strHaendler <> ("60" & strHaendlerNummer) Then
                        tblResult.Rows.Remove(tblResult.Rows(intCount))
                    End If
                Next
            End If

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", VALID_ON=" & UCase(m_strGueltgkeitsdatum) & ", CUSTHITYP=" & UCase(m_strHierarchietyp) & ", CUSTOMERNO=" & UCase(m_strKundennr), m_tblResult, False)

        Catch ex As Exception

            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1234
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler bei der Ermittlung der Händlerdaten."
            End Select
           
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", VALID_ON=" & UCase(m_strGueltgkeitsdatum) & ", CUSTHITYP=" & UCase(m_strHierarchietyp) & ", CUSTOMERNO=" & UCase(m_strKundennr), m_tblResult, False)
        End Try
    End Sub

    'Public Sub getHaendler(ByVal strHaendlerNummer As String, ByVal strAppID As String, ByVal strSessionID As String)
    '    m_strClassAndMethod = "Porsche_01.getHaendler"
    '    m_strAppID = strAppID
    '    m_strSessionID = strSessionID
    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True

    '        Dim objSAP As New SAPProxy_PORSCHE.SAPProxy_PORSCHE()
    '        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
    '        objSAP.Connection.Open()

    '        Dim intID As Int32 = -1

    '        Try
    '            Dim SAPTable As New SAPProxy_PORSCHE.ZDAD_M_WEB_CUSTOMERTable()

    '            intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Cust_Get_Children_Porsche", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
    '            objSAP.Z_M_Cust_Get_Children_Porsche(Right("0000000000" & m_objUser.KUNNR, 10), "Report01.aspx", SAPTable)
    '            objSAP.CommitWork()
    '            If intID > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(intID, True)
    '            End If

    '            m_tblResult = SAPTable.ToADODataTable
    '            m_tblResult.Columns.Add("Addresse", GetType(System.String))

    '            tblResult = m_tblResult

    '            Dim row As DataRow
    '            Dim strAddresse As String
    '            Dim intCount As Integer
    '            Dim strHaendler As String

    '            strAddresse = String.Empty

    '            For Each row In m_tblResult.Rows
    '                strAddresse = CType(row("NAME1"), String)
    '                If CType(row("NAME2"), String) <> String.Empty Then
    '                    strAddresse &= " - " & CType(row("NAME2"), String)
    '                End If
    '                If CType(row("ORT01"), String) <> String.Empty Then
    '                    strAddresse &= " - " & CType(row("ORT01"), String)
    '                End If
    '                row("Addresse") = strAddresse
    '            Next

    '            If Not (strHaendlerNummer = String.Empty) Then
    '                For intCount = tblResult.Rows.Count - 1 To 0 Step -1
    '                    strHaendler = Right(CStr(tblResult.Rows(intCount)("KUNNR_ZF")), 7)
    '                    If strHaendler <> ("60" & strHaendlerNummer) Then
    '                        tblResult.Rows.Remove(tblResult.Rows(intCount))
    '                    End If
    '                Next
    '            End If

    '            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", VALID_ON=" & UCase(m_strGueltgkeitsdatum) & ", CUSTHITYP=" & UCase(m_strHierarchietyp) & ", CUSTOMERNO=" & UCase(m_strKundennr), m_tblResult, False)

    '        Catch ex As Exception

    '            Select Case ex.Message
    '                Case "NO_DATA"
    '                    m_intStatus = -1234
    '                    m_strMessage = "Keine Daten gefunden."
    '                Case Else
    '                    m_intStatus = -9999
    '                    m_strMessage = "Fehler bei der Ermittlung der Händlerdaten."
    '            End Select
    '            If intID > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
    '            End If
    '            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", VALID_ON=" & UCase(m_strGueltgkeitsdatum) & ", CUSTHITYP=" & UCase(m_strHierarchietyp) & ", CUSTOMERNO=" & UCase(m_strKundennr), m_tblResult, False)
    '        Finally
    '            If intID > -1 Then
    '                m_objlogApp.WriteStandardDataAccessSAP(intID)
    '            End If

    '            objSAP.Connection.Close()
    '            objSAP.Dispose()

    '            m_blnGestartet = False
    '        End Try
    '    End If
    'End Sub


    Public Sub getEquiStichtag(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Porsche_01.getBriefe"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Equis_Zu_Stichtag", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            myProxy.setImportParameter("I_KUNNR_ZF", m_strKundennr)
            myProxy.setImportParameter("I_STICHTAG", m_strStichtag)

            myProxy.callBapi()

            m_tblResult = myProxy.getExportTable("GT_EXPORT")

            CreateOutPut(m_tblResult, strAppID)

            Dim row As DataRow
            m_tblResult.Columns("Kontingentart").MaxLength = 25
            m_tblResult.AcceptChanges()
            For Each row In m_tblResult.Rows
                If row("Kontingentart").ToString = "1" Then
                    row("Kontingentart") = "Standard temporär"
                End If
                If row("Kontingentart").ToString = "2" Then
                    row("Kontingentart") = "Standard endgültig"
                End If
            Next
            m_tblResult.AcceptChanges()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", KUNNR_ZF=" & UCase(m_strKundennr), m_tblResult, False)

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1234
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", KUNNR_ZF=" & UCase(m_strKundennr), m_tblResult, False)
        End Try
    End Sub

    'Public Sub getEquiStichtag(ByVal strAppID As String, ByVal strSessionID As String)
    '    m_strClassAndMethod = "Porsche_01.getBriefe"
    '    m_strAppID = strAppID
    '    m_strSessionID = strSessionID
    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True

    '        Dim objSAP As New SAPProxy_Porsche.SAPProxy_Porsche()
    '        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
    '        objSAP.Connection.Open()

    '        Dim intID As Int32 = -1

    '        Try
    '            Dim SAPTable As New SAPProxy_Porsche.ZDAD_M_EQUI_EXPORT_T()

    '            intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Equis_Zu_Stichtag", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
    '            objSAP.Z_M_Equis_Zu_Stichtag(Right("0000000000" & m_objUser.KUNNR, 10), m_strKundennr, m_strStichtag, SAPTable)

    '            objSAP.CommitWork()
    '            If intID > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(intID, True)
    '            End If

    '            m_tblResult = SAPTable.ToADODataTable

    '            CreateOutPut(m_tblResult, strAppID)

    '            Dim row As DataRow

    '            For Each row In m_tblResult.Rows
    '                If row("Kontingentart").ToString = "1" Then
    '                    row("Kontingentart") = "Standard temporär"
    '                End If
    '                If row("Kontingentart").ToString = "2" Then
    '                    row("Kontingentart") = "Standard endgültig"
    '                End If
    '            Next
    '            m_tblResult.AcceptChanges()

    '            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", KUNNR_ZF=" & UCase(m_strKundennr), m_tblResult, False)

    '        Catch ex As Exception

    '            Select Case ex.Message
    '                Case "NO_DATA"
    '                    m_intStatus = -1234
    '                    m_strMessage = "Keine Daten gefunden."
    '                Case Else
    '                    m_intStatus = -9999
    '                    m_strMessage = ex.Message
    '            End Select
    '            If intID > -1 Then
    '                m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
    '            End If
    '            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", KUNNR_ZF=" & UCase(m_strKundennr), m_tblResult, False)
    '        Finally
    '            If intID > -1 Then
    '                m_objlogApp.WriteStandardDataAccessSAP(intID)
    '            End If

    '            objSAP.Connection.Close()
    '            objSAP.Dispose()

    '            m_blnGestartet = False
    '        End Try
    '    End If
    'End Sub

    Public Sub getBriefe(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Porsche_01.getBriefe"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Haendlerbestand_Porsche", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            myProxy.setImportParameter("I_KUNNR_ZF", m_strKundennr)
            myProxy.setImportParameter("I_WEB_REPORT", "Report01.aspx")

            myProxy.callBapi()

            m_tblResult = myProxy.getExportTable("GT_WEB")
            CreateOutPut(m_tblResult, strAppID)
            m_tblResult.Columns("Fahrzeugklasse").MaxLength = 30
            m_tblResult.AcceptChanges()
            Dim row As DataRow
            Dim strFahrzeugklasse As String

            For Each row In m_tblResult.Rows
                strFahrzeugklasse = CStr(row("Fahrzeugklasse"))
                Select Case strFahrzeugklasse
                    Case "10"
                        row("Fahrzeugklasse") = "Neuwagen (Kunde)"
                    Case "11"
                        row("Fahrzeugklasse") = "Neuwagen (Lager)"
                    Case "12"
                        row("Fahrzeugklasse") = "Vorführwagen"
                    Case "13"
                        row("Fahrzeugklasse") = "Gebrauchtwagen"
                    Case Else
                        row("Fahrzeugklasse") = "-unbekannt-"
                End Select
            Next
            m_tblResult.AcceptChanges()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", KUNNR_ZF=" & UCase(m_strKundennr), m_tblResult, False)

        Catch ex As Exception

            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1234
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
          
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", KUNNR_ZF=" & UCase(m_strKundennr), m_tblResult, False)
        End Try
    End Sub

    'Public Sub getBriefe(ByVal strAppID As String, ByVal strSessionID As String)
    '    m_strClassAndMethod = "Porsche_01.getBriefe"
    '    m_strAppID = strAppID
    '    m_strSessionID = strSessionID
    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True

    '        Dim objSAP As New SAPProxy_PORSCHE.SAPProxy_PORSCHE()
    '        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
    '        objSAP.Connection.Open()

    '        Dim intID As Int32 = -1

    '        Try
    '            Dim SAPTable As New SAPProxy_PORSCHE.ZDAD_M_WEB_HAENDLERBESTANDTable()

    '            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Haendlerbestand_Porsche", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
    '            objSAP.Z_M_Haendlerbestand_Porsche(Right("0000000000" & m_objUser.KUNNR, 10), m_strKundennr, "Report01.aspx", SAPTable)

    '            objSAP.CommitWork()
    '            If intID > -1 Then
    '                m_objLogApp.WriteEndDataAccessSAP(intID, True)
    '            End If

    '            m_tblResult = SAPTable.ToADODataTable

    '            CreateOutPut(m_tblResult, strAppID)

    '            Dim row As DataRow
    '            Dim strFahrzeugklasse As String

    '            For Each row In m_tblResult.Rows
    '                strFahrzeugklasse = CStr(row("Fahrzeugklasse"))
    '                Select Case strFahrzeugklasse
    '                    Case "10"
    '                        row("Fahrzeugklasse") = "Neuwagen (Kunde)"
    '                    Case "11"
    '                        row("Fahrzeugklasse") = "Neuwagen (Lager)"
    '                    Case "12"
    '                        row("Fahrzeugklasse") = "Vorführwagen"
    '                    Case "13"
    '                        row("Fahrzeugklasse") = "Gebrauchtwagen"
    '                    Case Else
    '                        row("Fahrzeugklasse") = "-unbekannt-"
    '                End Select
    '            Next
    '            m_tblResult.AcceptChanges()

    '            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", KUNNR_ZF=" & UCase(m_strKundennr), m_tblResult, False)

    '        Catch ex As Exception

    '            Select Case ex.Message
    '                Case "NO_DATA"
    '                    m_intStatus = -1234
    '                    m_strMessage = "Keine Daten gefunden."
    '                Case Else
    '                    m_intStatus = -9999
    '                    m_strMessage = ex.Message
    '            End Select
    '            If intID > -1 Then
    '                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
    '            End If
    '            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", KUNNR_ZF=" & UCase(m_strKundennr), m_tblResult, False)
    '        Finally
    '            If intID > -1 Then
    '                m_objLogApp.WriteStandardDataAccessSAP(intID)
    '            End If

    '            objSAP.Connection.Close()
    '            objSAP.Dispose()

    '            m_blnGestartet = False
    '        End Try
    '    End If
    'End Sub
#End Region
End Class

' ************************************************
' $History: Porsche_01.vb $
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 9.07.09    Time: 14:25
' Updated in $/CKAG/Applications/AppPorsche/Lib
' ITA 2918 nachbesserungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 9.07.09    Time: 10:22
' Updated in $/CKAG/Applications/AppPorsche/Lib
' ITA 2918 Z_M_OFFENEANFOR_PORSCHE, Z_M_OFFENEANFOR_STORNO_PORSCHE,
' Z_M_UNANGEFORDERT_PORSCHE, Z_M_UNB_HAENDLER_PORSCHE,
' Z_M_EQUIS_ZU_STICHTAG
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 3.07.09    Time: 15:37
' Updated in $/CKAG/Applications/AppPorsche/Lib
' ITA 2918 Z_M_BRIEFANFORDERUNG_PORSCHE, Z_M_BRIEF_OHNE_DATEN_PORSCHE,
' Z_M_CREDITLIMIT_CHANGE_PORSCHE, Z_M_CUST_GET_CHILDREN_PORSCHE,
' Z_M_DATEN_OHNE_BRIEF_PORSCHE, Z_M_FREIGEBEN_AUFTRAG_PORSCHE,
' Z_M_GESPERRT_AUFTRAG_PORSCHE, Z_M_HAENDLERBESTAND_PORSCHE
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 9:21
' Updated in $/CKAG/Applications/AppPorsche/Lib
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:28
' Created in $/CKAG/Applications/AppPorsche/Lib
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 13.02.08   Time: 9:45
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Lib
' ITA: 1653
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 18:24
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 5.03.07    Time: 12:43
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Lib
' 
' ************************************************