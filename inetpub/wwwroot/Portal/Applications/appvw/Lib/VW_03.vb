Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

Public Class VW_03
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_tblHistory As DataTable
    Private m_intResultCount As Integer
#End Region

#Region " Properties"
    Public ReadOnly Property ResultCount() As Integer
        Get
            Return m_intResultCount
        End Get
    End Property

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
        FILL(m_strAppID, m_strSessionID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)

    End Sub

    Public Sub FillHaendlerstatus(ByVal strAppID As String, ByVal strSessionID As String, ByVal pUebergabeVon As String, ByVal pUebergabeBis As String, ByVal pVorhaben As String, ByVal pStatus As String)
        m_strClassAndMethod = "VW_03.FillHaendlerstatus"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim objSAP As New SAPProxy_VW.SAPProxy_VW()
            'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            'objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                'Dim SAPTable As New SAPProxy_VW.ZCS_VWNUTZ_S001Table()

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_V_Zdad_V_Vwnutz_001", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                m_tblResult = S.AP.GetExportTableWithInitExecute("Z_V_Zdad_V_Vwnutz_001.VWNUTZ_TAB",
                                                                                  "KUNNR, STATUS, UEBERGDAT_VON, UEBERGDAT_BIS, VORHABEN",
                                                                                  m_objUser.KUNNR.ToSapKunnr, pStatus, pUebergabeVon.ToDateTimeOrNull, pUebergabeBis.ToDateTimeOrNull, pVorhaben)


                'If pUebergabeBis.Length = 0 Or pUebergabeVon.Length = 0 Then
                '    objSAP.Z_V_Zdad_V_Vwnutz_001(Right("0000000000" & m_objUser.KUNNR, 10), pStatus, "00000000", "00000000", pVorhaben, SAPTable)
                'Else
                '    objSAP.Z_V_Zdad_V_Vwnutz_001(Right("0000000000" & m_objUser.KUNNR, 10), pStatus, MakeDateSAP(pUebergabeBis), MakeDateSAP(pUebergabeVon), pVorhaben, SAPTable)
                'End If
                'objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                'm_tblResult = SAPTable.ToADODataTable
                m_intResultCount = m_tblResult.Rows.Count

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", Übergabe Von=" & pUebergabeVon & ", Übergabe Bis=" & pUebergabeBis & ", Vorhaben=" & pVorhaben & ", Status=" & UCase(pStatus) & ", Count=" & m_intResultCount.ToString, m_tblHistory, False)
            Catch ex As Exception
                m_intResultCount = 0
                m_intStatus = -2222
                Select Case ex.Message
                    Case "NRF"
                        m_strMessage = "Keine Daten vorhanden."
                    Case "FALSCHER_STATUS"
                        m_strMessage = "Falscher Status"
                    Case Else
                        m_strMessage = ex.Message
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", Übergabe Von=" & pUebergabeVon & ", Übergabe Bis=" & pUebergabeBis & ", Vorhaben=" & pVorhaben & ", Status=" & UCase(pStatus) & ", Count=" & m_intResultCount.ToString, m_tblHistory, False)
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
' $History: VW_03.vb $
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
' *****************  Version 5  *****************
' User: Uha          Date: 13.08.07   Time: 17:03
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' Bugfixing in "Lieferschein-Handling" lt. ITA 1125
' 
' *****************  Version 4  *****************
' User: Uha          Date: 13.08.07   Time: 14:04
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' Lieferschein-Handling lt. ITA 1125 geändert
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 3.07.07    Time: 9:13
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 19.06.07   Time: 16:24
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' History-Eintrag eingefügt
' 
' ************************************************