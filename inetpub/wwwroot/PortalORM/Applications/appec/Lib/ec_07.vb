Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common
Public Class ec_07

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_tblHistory As DataTable
    'Private m_Fill As String = "---------------"
    Private m_Fill As String = "----------"    '§§§ JVE 02.10.2006
    Private m_BriefCount As String
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

    Public Sub GetCountBrief(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "EC_07.GetCountBrief"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_BriefCount = "0"
        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Nur_Brief_Vorh", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()

            'Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")

            S.AP.InitExecute("Z_M_Ec_Avm_Nur_Brief_Vorh", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")
            m_BriefCount = CStr(tblTemp2.Rows.Count)

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_FLEET"
                    m_strMessage = "Keine Fleet Daten vorhanden."
                Case "NO_WEB"
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case "NO_DATA"
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "EC_07.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Status_Einsteuerung", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Ec_Avm_Status_Einsteuerung", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            Dim drow As DataRow
            Dim NewTable As DataTable
            Dim NewRow As DataRow
            Dim i As Integer
            Dim intCountGesperrt As Integer

            NewTable = tblTemp2.Clone()

            For Each drow In tblTemp2.Rows
                'ausgabe mit Summe PKW und LKW, ausgebessert JJ2007.12.14
                If CStr(drow("ZNAME1")) = String.Empty And CStr(drow("ZZCARPORT")) <> "GESAMT" And CStr(drow("ZZCARPORT")) <> "Summe PKW" And CStr(drow("ZZCARPORT")) <> "Summe LKW" Then
                    For i = 0 To 5
                        drow.Item(i) = m_Fill
                    Next
                    'Neue Zeile einfügen
                    NewRow = NewTable.NewRow()
                    NewTable.Rows.Add(NewRow)
                    For i = 0 To 19
                        NewRow.Item(i) = drow.Item(i)
                    Next
                    'Leerzeile einfügen
                    NewRow = NewTable.NewRow()
                    NewTable.Rows.Add(NewRow)
                Else
                    NewRow = NewTable.NewRow()
                    NewTable.Rows.Add(NewRow)

                    For i = 0 To 19
                        NewRow.Item(i) = drow.Item(i)
                    Next

                    If CStr(drow("ZZCARPORT")) = "GESAMT" Then
                        intCountGesperrt = CInt(drow.Item(20))
                    End If
                End If
            Next
            NewRow = NewTable.NewRow()
            NewTable.Rows.Add(NewRow)

            NewRow = NewTable.NewRow()
            NewRow(0) = "ZBII ohne Fahrzeug"
            NewRow(1) = m_BriefCount
            NewTable.Rows.Add(NewRow)
            NewRow = NewTable.NewRow()
            NewRow(0) = "Gesperrte Fahrzeuge"
            NewRow(1) = intCountGesperrt
            NewTable.Rows.Add(NewRow)

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

' ************************************************
' $History: ec_07.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 9.02.11    Time: 14:49
' Updated in $/CKAG/Applications/appec/Lib
' 219853
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 26.06.09   Time: 11:29
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918
' Z_M_EC_AVM_BRIEFLEBENSLAUF,Z_M_Ec_Avm_Fzg_M_Dfs_O_Zul,Z_M_EC_AVM_FZG_OH
' NE_BRIEF,Z_M_Ec_Avm_Fzg_Ohne_Unitnr,Z_M_Ec_Avm_Nur_Brief_Vorh,
' Z_M_EC_AVM_OFFENE_ZAHLUNGEN,  Z_M_EC_AVM_PDI_BESTAND,
' Z_M_EC_AVM_STATUS_EINSTEUERUNG,  Z_M_EC_AVM_STATUS_GREENWAY,
' Z_M_Ec_Avm_Status_Zul, Z_M_EC_AVM_ZULASSUNGEN, Z_M_Ec_Avm_Zulassungen_2
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
' *****************  Version 11  *****************
' User: Jungj        Date: 14.12.07   Time: 14:18
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' ITA 1353/1354 Statusberichte (Summen PKW/LKW hinzugefügt)
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 16.10.07   Time: 14:50
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 17:23
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 8  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' ************************************************
