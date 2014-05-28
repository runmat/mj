Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common

Public Class OffeneAnforderungenGMAC
    Inherits BankBase

    Private Const BAPI_offeneAnforderungen As String = "Z_M_OFFENE_ANFORDERUNGEN_STD"
    Private Const BAPI_storno As String = "Z_M_OFFENE_ANFORDER_STORNO_STD"


    Sub New(ByVal user As User, ByVal app As App, ByVal appId As String, ByVal sessionID As String)
        MyBase.New(user, app, appId, sessionID, "")

        KUNNR = user.KUNNR
    End Sub

    Public Sub LoadData(ByRef page As Page, ByVal haendler_ex As String)
        m_intStatus = 0

        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name

        Try
            Dim proxy = DynSapProxy.getProxy(BAPI_offeneAnforderungen, m_objApp, m_objUser, page)

            proxy.setImportParameter("I_AG", KUNNR)
            proxy.setImportParameter("I_HDGRP", "")
            proxy.setImportParameter("I_HAENDLER_EX", haendler_ex)
            proxy.setImportParameter("I_KKBER", "")

            proxy.callBapi()

            Dim result = proxy.getExportTable("GT_WEB")

            result.Columns.Add("KONTART", System.Type.GetType("System.String"))
            result.AcceptChanges()

            Dim rowTemp As DataRow
            For Each rowTemp In result.Rows
                Select Case rowTemp("ZZKKBER").ToString
                    Case "0001"
                        rowTemp("KONTART") = "Standard temporär"
                    Case "0002"
                        rowTemp("KONTART") = "Standard endgültig"
                    Case "0003"
                        rowTemp("KONTART") = "Retail"
                    Case "0004"
                        rowTemp("KONTART") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                    Case "0005"
                        rowTemp("KONTART") = "Händlereigene Zulassung"
                    Case "0006"
                        rowTemp("KONTART") = "KF/KL"
                End Select
                If CStr(rowTemp("Status")) = "G" Then
                    rowTemp("Status") = "X"
                Else
                    rowTemp("Status") = ""
                End If
            Next
            result.AcceptChanges()

            If result.Rows.Count = 0 Then
                m_intStatus = -10
                m_strMessage = "Keine Daten gefunden."
            End If

            m_tblResult = result

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "EXCEPTION NO_DATA RAISED"
                    m_intStatus = -1402
                    m_strMessage = "Keine Daten gefunden."
                Case "NO_DATA"
                    m_intStatus = -1402
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select

            m_tblResult = Nothing
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & m_strMessage.Replace("<br>", " "), Nothing)
        End Try
    End Sub

    Public Sub Storno(ByRef page As Page, ByVal haendler_ex As String, ByVal auftragsnummer As String, ByVal equinummer As String)
        m_intStatus = 0

        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name

        Try
            Dim proxy = DynSapProxy.getProxy(BAPI_storno, m_objApp, m_objUser, page)

            proxy.setImportParameter("I_AG", KUNNR)
            proxy.setImportParameter("I_HAENDLER_EX", haendler_ex)
            proxy.setImportParameter("I_EQUNR", equinummer)
            proxy.setImportParameter("I_VBELN", auftragsnummer)

            proxy.setImportParameter("I_ERNAM", m_objUser.UserName.Substring(0, Math.Min(12, m_objUser.UserName.Length)))

            proxy.callBapi()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, Nothing)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_UPDATE"
                    m_intStatus = -3501
                    m_strMessage = "Kein EQUI-UPDATE."
                Case "NO_ZDADVERSAND"
                    m_intStatus = -3502
                    m_strMessage = "Kein ZDADVERSAND-STORNO."
                Case "NO_UPDATE_SALESDOCUMENT"
                    m_intStatus = -3503
                    m_strMessage = "Keine Auftragsänderung."
                Case "ZVERSAND_SPERRE"
                    m_intStatus = -3504
                    m_strMessage = "ZVERSAND vom DAD gesperrt."
                Case "NO_PICKLISTE"
                    m_intStatus = -3505
                    m_strMessage = "Kein Picklisteneintrag gefunden."
                Case "NO_ZCREDITCONTROL"
                    m_intStatus = -3506
                    m_strMessage = "Kein Creditcontroleintrag gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & m_strMessage.Replace("<br>", " "), Nothing)
        End Try
    End Sub

    Public Overrides Sub Show()
        Throw New NotImplementedException
    End Sub

    Public Overrides Sub Change()
        Throw New NotImplementedException
    End Sub

End Class
