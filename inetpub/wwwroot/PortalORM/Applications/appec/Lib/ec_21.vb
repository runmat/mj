Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class ec_21
    REM § Status-Report, Kunde: ALD, BAPI: Z_V_Ueberf_Auftr_Kund_Port,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    
    Private strHerstellerBezeichnung As String
    Private dtSAPHersteller As DataTable
    Private m_strFilename2 As String
    Private mGesamtdaten As DataTable
#End Region

#Region " Properties"


    Public ReadOnly Property FileName() As String
        Get
            Return m_strFilename2
        End Get
    End Property

    Property HerstellerBezeichnung() As String
        Get
            Return strHerstellerBezeichnung
        End Get
        Set(ByVal Value As String)
            strHerstellerBezeichnung = Value
        End Set
    End Property

    Property SAPHerstellertabelle() As DataTable
        Get
            Return dtSAPHersteller
        End Get
        Set(ByVal Value As DataTable)
            dtSAPHersteller = Value
        End Set
    End Property

    Public Property Gesamtdaten() As DataTable
        Get
            Return mGesamtdaten
        End Get
        Set(ByVal Value As DataTable)
            mGesamtdaten = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_strFilename2 = strFilename
    End Sub

    Public Overloads Overrides Sub Fill()
    End Sub


    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        m_intStatus = 0
        m_strClassAndMethod = "EC_21.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            Dim SAPTable As DataTable
            Dim saprow As DataRow

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Eca_Tab_Bestand", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_HERST", strHerstellerBezeichnung)
            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Eca_Tab_Bestand", "I_KUNNR,I_HERST", Right("0000000000" & m_objUser.KUNNR, 10), strHerstellerBezeichnung)

            SAPTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            SAPTable.Columns("Abckz").MaxLength = 25
            SAPTable.AcceptChanges()
            For Each saprow In SAPTable.Rows
                If saprow("Abckz").ToString = "1" Then
                    saprow("Abckz") = "temporär versendet"
                Else
                    saprow("Abckz") = "Im Bestand DAD"
                End If
            Next
            CreateOutPut(SAPTable, m_strAppID)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Fahrzeuge gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub


    Public Sub fillHersteller(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strAppID = AppID
        m_strSessionID = SessionID
        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Herstellergroup", m_objApp, m_objUser, page)
            'myProxy.callBapi()
            S.AP.InitExecute("Z_M_Herstellergroup")

            dtSAPHersteller = S.AP.GetExportTable("T_HERST") 'myProxy.getExportTable("T_HERST")

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


    Public Sub GetAllData(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_intStatus = 0
        m_strClassAndMethod = "EC_21.GetAllData"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Eca_Tab_Bestand", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Eca_Tab_Bestand", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            Gesamtdaten = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Fahrzeuge gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub


#End Region
End Class