Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

Public Class VW_01
    REM § Status-Report, Kunde: VW, BAPI: Z_M_KLAERFAELLEVW,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    'Public Overloads Overrides Sub Fill()
    '    Fill(m_strAppID, m_strSessionID)
    'End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "VW_01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim objSAP As New SAPProxy_VW.SAPProxy_VW()
            'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            'objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Klaerfaellevw", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                'Dim SAPTable As New SAPProxy_VW.ZDAD_M_WEB_KLAERFALLTable()
                Dim tblTemp2 As DataTable = S.AP.GetExportTableWithInitExecute("Z_M_Klaerfaellevw.GT_WEB", "I_KUNNR", m_objUser.KUNNR.ToSapKunnr)

                'objSAP.Z_M_Klaerfaellevw(Right("0000000000" & m_objUser.KUNNR, 10), SAPTable)
                'objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                'Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

                Dim tmpRow As DataRow
                Dim tmpString As String = String.Empty
                For Each tmpRow In tblTemp2.Rows
                    If TypeOf tmpRow("TDLINE1") Is System.String Then
                        tmpString = Trim(CStr(tmpRow("TDLINE1")))
                    End If
                    If TypeOf tmpRow("TDLINE2") Is System.String Then
                        tmpString &= " " & Trim(CStr(tmpRow("TDLINE2")))
                    End If
                    If TypeOf tmpRow("TDLINE3") Is System.String Then
                        tmpString &= " " & Trim(CStr(tmpRow("TDLINE3")))
                    End If
                    If TypeOf tmpRow("TDLINE4") Is System.String Then
                        tmpString &= " " & Trim(CStr(tmpRow("TDLINE4")))
                    End If
                    If TypeOf tmpRow("TDLINE5") Is System.String Then
                        tmpString &= " " & Trim(CStr(tmpRow("TDLINE5")))
                    End If
                    tmpRow("TDLINE1") = tmpString.Trim(" "c)
                Next
                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Fahrzeuge gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                'objSAP.Connection.Close()
                'objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: VW_01.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:14
' Updated in $/CKAG/Applications/appvw/Lib
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:49
' Created in $/CKAG/Applications/appvw/Lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 3.07.07    Time: 9:13
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 15:06
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 
' ************************************************
