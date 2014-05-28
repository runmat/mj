Option Explicit On
Option Strict On

Imports CKG
Imports CKG.Base.Common
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class BzKennzeichen_Sperren
    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Sub SetBezKennz(ByVal strAppID As String, ByVal strSessionID As String, _
                           ByRef page As Page, ByVal Fahrzeuge As DataTable)
        m_strClassAndMethod = "BzKennzeichen_Sperren.SetBezKennz"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_FLAG_SPERRE_BEZAHLT_001", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            S.AP.Init("Z_M_FLAG_SPERRE_BEZAHLT_001", "I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim sapImportTable As DataTable = S.AP.GetImportTable("GT_WEB") 'myProxy.getImportTable("GT_WEB")
            Dim newSapRow As DataRow
            Dim tmpRow As DataRow

            For Each tmpRow In Fahrzeuge.Rows
                newSapRow = sapImportTable.NewRow
                newSapRow("EQUNR") = tmpRow("EQUNR").ToString
                newSapRow("ZZBEZAHLT") = tmpRow("Bezahltkennzeichen").ToString
                newSapRow("AEND_ZZBEZAHLT") = "X"
                sapImportTable.Rows.Add(newSapRow)
            Next

            'myProxy.callBapi()
            S.AP.Execute()

            Dim SAPResultTable As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            If SAPResultTable.Rows.Count > 0 Then
                For Each tmpRow In Fahrzeuge.Rows
                    tmpRow("ErrorMessage") = SAPResultTable.Select("EQUNR='" & tmpRow("EQUNR").ToString & "'")(0)("RETUR_BEM")
                Next
            End If

        Catch ex As Exception
            Fahrzeuge = Nothing
            m_intStatus = -9999

            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "keine Eingabedaten"
                Case Else
                    m_strMessage = "Beim der Übertragung ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try
    End Sub

    Public Sub SetSperre(ByVal strAppID As String, ByVal strSessionID As String, _
                           ByRef page As Page, ByVal Fahrzeuge As DataTable)
        m_strClassAndMethod = "BzKennzeichen_Sperren.SetBezKennz"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_FLAG_SPERRE_BEZAHLT_001", m_objApp, m_objUser, Page)

            'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            S.AP.Init("Z_M_FLAG_SPERRE_BEZAHLT_001", "I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim sapImportTable As DataTable = S.AP.GetImportTable("GT_WEB") 'myProxy.getImportTable("GT_WEB")
            Dim newSapRow As DataRow
            Dim tmpRow As DataRow

            For Each tmpRow In Fahrzeuge.Rows
                newSapRow = sapImportTable.NewRow
                newSapRow("EQUNR") = tmpRow("EQUNR").ToString
                newSapRow("ZZAKTSPERRE") = tmpRow("Versandsperre").ToString
                newSapRow("AEND_ZZAKTSPERRE") = "X"
                sapImportTable.Rows.Add(newSapRow)
            Next

            'myProxy.callBapi()
            S.AP.Execute()

            Dim SAPResultTable As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            If SAPResultTable.Rows.Count > 0 Then
                For Each tmpRow In Fahrzeuge.Rows
                    tmpRow("ErrorMessage") = SAPResultTable.Select("EQUNR='" & tmpRow("EQUNR").ToString & "'")(0)("RETUR_BEM")
                Next
            End If


        Catch ex As Exception
            Fahrzeuge = Nothing
            m_intStatus = -9999
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "keine Eingabedaten"
                Case Else
                    m_strMessage = "Beim der Übertragung ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try

    End Sub


#End Region

End Class
