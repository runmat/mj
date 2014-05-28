Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

Public Class VW_07

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_Mahnstufe As String
#End Region

#Region " Properties"
    Public Property Mahnstufe() As String
        Get
            Return m_Mahnstufe
        End Get
        Set(ByVal Value As String)
            m_Mahnstufe = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub


    Public Function FillMahnungen(ByVal strAppID As String, ByVal strSessionID As String) As DataTable
        m_strClassAndMethod = "VW_07.FillMahnungen"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim objSAP As New SAPProxy_VW.SAPProxy_VW()
            'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            'objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                'Dim SAPTable As New SAPProxy_VW.ZDAD_MAHNUNGENTable()


                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_Dad_Web_Mahnungen", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                m_tblResult = S.AP.GetExportTableWithInitExecute("Z_Dad_Web_Mahnungen.GT_MAHNUNGEN",
                                                                                  "I_KUNNR, I_ZZMAHNS",
                                                                                  m_objUser.KUNNR.ToSapKunnr, Mahnstufe)

                'objSAP.Z_Dad_Web_Mahnungen(Right("0000000000" & m_objUser.KUNNR, 10), Mahnstufe, SAPTable)
                'objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                'm_tblResult = SAPTable.ToADODataTable


                Dim dr As DataRow


                For Each dr In m_tblResult.Rows

                    dr.BeginEdit()


                    'If dr.Item("REPLA_DATE").ToString <> String.Empty Then

                    '    If dr.Item("REPLA_DATE").ToString.StartsWith("00000000") Then

                    '        dr.Item("REPLA_DATE") = String.Empty
                    '    Else
                    '        dr.Item("REPLA_DATE") = dr.Item("REPLA_DATE").ToString.Substring(6, 2) & "." & _
                    '                                dr.Item("REPLA_DATE").ToString.Substring(4, 2) & "." & _
                    '                                dr.Item("REPLA_DATE").ToString.Substring(0, 4)

                    '    End If
                    'End If

                    'If dr.Item("ZZMADAT").ToString <> String.Empty Then

                    '    If dr.Item("ZZMADAT").ToString.StartsWith("00000000") Then

                    '        dr.Item("ZZMADAT") = String.Empty
                    '    Else
                    '        dr.Item("ZZMADAT") = dr.Item("ZZMADAT").ToString.Substring(6, 2) & "." & _
                    '                                dr.Item("ZZMADAT").ToString.Substring(4, 2) & "." & _
                    '                                dr.Item("ZZMADAT").ToString.Substring(0, 4)

                    '    End If
                    'End If


                    If Not dr.Item("ZZVERZG") Is DBNull.Value AndAlso Not dr.Item("ZZVERZG") Is String.Empty Then
                        Select Case CInt(dr.Item("ZZVERZG"))
                            Case 1
                                dr.Item("ZZVERZG") = "Fahrzeug fehlt"
                            Case 2
                                dr.Item("ZZVERZG") = "Protokoll fehlt"
                            Case 3
                                dr.Item("ZZVERZG") = "DP: AP nicht erreichbar"
                            Case 4
                                dr.Item("ZZVERZG") = "DP: Abholung nicht erfolgt"
                            Case 5
                                dr.Item("ZZVERZG") = "AH: Keine Terminabsprache"
                            Case 6
                                dr.Item("ZZVERZG") = "AH: Protokoll bereits gefaxt"
                            Case 7
                                dr.Item("ZZVERZG") = "DAD: Rückruf AP Händler"
                            Case 8
                                dr.Item("ZZVERZG") = "DAD: Kein Kontakt AP Händler"
                            Case 9
                                dr.Item("ZZVERZG") = ""
                        End Select
                    End If


                    dr.EndEdit()
                    m_tblResult.AcceptChanges()
                Next





                'CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", Mahnstufe=" & Mahnstufe, m_tblResult, False)

                Return m_tblResult

            Catch ex As Exception

                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Mahnungen gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", Mahnstufe=" & Mahnstufe & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                'objSAP.Connection.Close()
                'objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
        Return m_tblResult
    End Function
#End Region
End Class

' ************************************************
' $History: VW_07.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 14.05.08   Time: 15:36
' Updated in $/CKAG/Applications/appvw/Lib
' ITA 1799
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:49
' Created in $/CKAG/Applications/appvw/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 25.09.07   Time: 11:46
' Created in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 

