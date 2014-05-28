Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common
Public Class ec_20
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_tblHistory As DataTable
    Private m_Fill As String = "----------"
#End Region

#Region " Properties"
    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
    End Sub


    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "EC_20.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Status_Bestand", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Ec_Avm_Status_Bestand", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            Dim drow As DataRow
            Dim NewTable As DataTable
            Dim NewRow As DataRow
            Dim i As Integer

            NewTable = tblTemp2.Clone()

            For Each drow In tblTemp2.Rows
                If CStr(drow("ZNAME1")) = String.Empty And CStr(drow("ZZCARPORT")) <> "GESAMT" And CStr(drow("ZZCARPORT")) <> "Summe PKW" And CStr(drow("ZZCARPORT")) <> "Summe LKW" Then
                    For i = 0 To 5
                        drow.Item(i) = m_Fill
                    Next
                    'Neue Zeile einfügen
                    NewRow = NewTable.NewRow()
                    NewTable.Rows.Add(NewRow)
                    For i = 0 To 13
                        NewRow.Item(i) = drow.Item(i)
                    Next
                    'Leerzeile einfügen
                    NewRow = NewTable.NewRow()
                    NewTable.Rows.Add(NewRow)
                Else
                    NewRow = NewTable.NewRow()
                    NewTable.Rows.Add(NewRow)

                    For i = 0 To 13
                        NewRow.Item(i) = drow.Item(i)
                    Next
                End If
            Next
            tblTemp2.AcceptChanges()

            CreateOutPut(NewTable, strAppID)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_FLEET"
                    m_strMessage = "Keine Fleet Daten vorhanden."
                Case "NO_WEB"
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        End Try
    End Sub


#End Region
End Class