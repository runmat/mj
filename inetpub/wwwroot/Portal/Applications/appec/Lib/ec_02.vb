Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class ec_02
    REM § Status-Report, Kunde: ALD, BAPI: Z_V_Ueberf_Auftr_Kund_Port,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private tblBatche As DataTable
    Private strModelId As String
    Private strHerstellerBezeichnung As String
    Private strBatchId As String
    Private strLoeschvermerk As String
    Private strUnitNr As String
    Private strSelectionCriteria As String
    Private boolWithUnitNumbers As Boolean = False
    Dim SAPTable As DataTable
    'Dim myProxy As DynSapProxyObj
    Dim tblHersteller As DataTable
#End Region

#Region " Properties"


    Property Hersteller() As DataTable
        Get
            Return tblHersteller
        End Get
        Set(ByVal Value As DataTable)
            tblHersteller = Value
        End Set
    End Property

    Property ModelID() As String
        Get
            Return strModelId
        End Get
        Set(ByVal Value As String)
            strModelId = Value
        End Set
    End Property

    Property UnitNr() As String
        Get
            Return strUnitNr
        End Get
        Set(ByVal Value As String)
            strUnitNr = Value
        End Set
    End Property
    Property HerstellerBezeichnung() As String
        Get
            Return strHerstellerBezeichnung
        End Get
        Set(ByVal Value As String)
            strHerstellerBezeichnung = Value
        End Set
    End Property

    Property withUnitNumbers() As Boolean
        Get
            Return boolWithUnitNumbers
        End Get
        Set(ByVal Value As Boolean)
            boolWithUnitNumbers = Value
        End Set
    End Property

    Property SelectionCriteria() As String
        Get
            Return strSelectionCriteria
        End Get
        Set(ByVal Value As String)
            strSelectionCriteria = Value
        End Set
    End Property

    Property BarchId() As String
        Get
            Return strBatchId
        End Get
        Set(ByVal Value As String)
            strBatchId = Value
        End Set
    End Property

    Property Loeschvermerk() As String
        Get
            Return strLoeschvermerk
        End Get
        Set(ByVal Value As String)
            strLoeschvermerk = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        'myProxy = DynSapProxy.getProxy("Z_M_Ec_Avm_Batch_Update", m_objApp, m_objUser, New Page)

        'SAPTable = myProxy.getImportTable("GT_WEB")

        S.AP.Init("Z_M_Ec_Avm_Batch_Update")
        SAPTable = S.AP.GetImportTable("GT_WEB")
    End Sub

    Public Sub fillHerstellerDDL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strAppID = AppID
        m_strSessionID = SessionID

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Herstellergroup", m_objApp, m_objUser, Page)
            'myProxy.callBapi()
            'tblHersteller = myProxy.getExportTable("T_HERST")

            S.AP.InitExecute("Z_M_Herstellergroup")
            tblHersteller = S.AP.GetExportTable("T_HERST")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Hersteller gefunden"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try

    End Sub

    Public Sub comitSapWorkAndCloseConnection(ByVal strAppID As String, ByVal strSessionID As String)

        Try

            'myProxy.callBapi()
            S.AP.Init("Z_M_Ec_Avm_Batch_Update")
            Dim tblCommit As DataTable = S.AP.GetImportTable("GT_WEB")
            'Dim commitRow As DataRow

            'For Each dr As DataRow In SAPTable.Rows

            '    commitRow = tblCommit.NewRow()

            '    For Each col As DataColumn In tblCommit.Columns
            '        commitRow(col.ToString().ToUpper()) = dr(col.ToString().ToUpper())
            '    Next

            '    tblCommit.Rows.Add(commitRow)
            'Next

            tblCommit.Merge(SAPTable)

            'tblCommit = SAPTable
            S.AP.Execute()

            tblCommit.Dispose()
            SAPTable.Clear()

            'Report-Logeintrag (ok)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_UPDATE"
                    m_intStatus = -1234
                    m_strMessage = "Fehler bei der Aktualisierung."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub
    
    Public Sub saveDataInSAPRow(ByVal strUnitNr As String, ByVal strGesperrt As String, ByVal strLaufzeitbindung As String, ByVal strSperrBemerkung As String, ByVal strBemerkung As String)
        Dim SAPTableRow As DataRow = SAPTable.NewRow

        With SAPTableRow
            .Item("ZLOEVM") = strGesperrt
            .Item("ZUNIT_NR") = strUnitNr
            If Not strLaufzeitbindung.Contains("X") Then
                .Item("ZLZBINDUNG") = ""
            Else
                .Item("ZLZBINDUNG") = strLaufzeitbindung
            End If
            .Item("ZBEM_SPERR") = strSperrBemerkung
            .Item("ZUSER_SPERR") = m_objUser.UserName
            .Item("ZDAT_SPERR") = Today.Date
            'Bemerkung
            .Item("ZBEMERKUNG") = strBemerkung
        End With
        SAPTable.Rows.Add(SAPTableRow)

    End Sub

    Public Sub getData(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "ec_02.getData"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        'Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Batch_Ansicht", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_ZMAKE", strHerstellerBezeichnung)
            'myProxy.setImportParameter("I_ZMODEL_ID", strModelId)
            'myProxy.setImportParameter("I_ZBATCH_ID", strBatchId)
            'myProxy.setImportParameter("I_ZLOEVM", strLoeschvermerk)
            'myProxy.setImportParameter("I_UNIT_NR", UnitNr)

            'myProxy.callBapi()

            ''Tabellen formatieren            
            'tblBatche = myProxy.getExportTable("GT_WEB")

            S.AP.InitExecute("Z_M_Ec_Avm_Batch_Ansicht", "I_ZMAKE,I_ZMODEL_ID,I_ZBATCH_ID,I_ZLOEVM,I_UNIT_NR", _
                             strHerstellerBezeichnung, strModelId, strBatchId, strLoeschvermerk, UnitNr)

            tblBatche = S.AP.GetExportTable("GT_WEB")
            CreateOutPut(tblBatche, strAppID)

            With m_tblResult.Columns
                .Add("RowID", GetType(System.String))
                .Add("Status", GetType(System.String))
                .Add("AuftragsNrVonBis", GetType(System.String))
                .Add("UnitNrVonBis", GetType(System.String))
                .Add("LoeschNeu", GetType(System.String))
            End With

            Dim row As DataRow
            Dim intCounter As Integer = 0

            For Each row In m_tblResult.Rows
                intCounter += 1
                row("AuftragsNrVon") = CType(row("AuftragsNrVon"), String).TrimStart("0"c)
                row("AuftragsNrBis") = CType(row("AuftragsNrBis"), String).TrimStart("0"c)
                row("Anzahl") = CType(row("Anzahl"), String).TrimStart("0"c)
                row("RowID") = m_objUser.UserName & "." & intCounter
                row("AuftragsNrVonBis") = CType(row("AuftragsNrVon"), String) & "<br>" & CType(row("AuftragsNrBis"), String)
                row("UnitNrVonBis") = CType(row("UnitNrVon"), String) & "<br>" & CType(row("UnitNrBis"), String)
                row("LoeschNeu") = CType(row("Loesch"), String)
                row("unitNR") = CStr(row("unitNR"))
            Next
            m_tblResult.AcceptChanges()

            'Report-Logeintrag (ok)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1234
                    m_strMessage = "Keine Daten gefunden!"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

#End Region

End Class

' ************************************************
' $History: ec_02.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 25.06.09   Time: 17:20
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918 tests und anpassungen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.06.09   Time: 16:04
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918 Z_M_Herstellergroup, Z_M_EC_AVM_BATCH_update,
' Z_M_EC_AVM_HERST_VWZWECK_MODID, Z_M_EC_AVM_BATCH_INSERT,
' Z_M_EC_AVM_BATCH_ANSICHT
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Lib
' Warnungen entfernt!
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 7.01.08    Time: 8:57
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Auswahl nach Herstellergruppen ermöglichen ITA 1358 Batch reporting
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 21.12.07   Time: 15:39
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' ITA 1358 vorbereitet für DDL mit Herstellergruppen
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 21.12.07   Time: 15:06
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' batchreporting ITA 1358
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 25.10.07   Time: 18:15
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 24.10.07   Time: 18:54
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 23.10.07   Time: 18:00
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 17:23
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 6  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' ************************************************
