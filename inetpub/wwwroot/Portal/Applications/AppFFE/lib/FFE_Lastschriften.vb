Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports Microsoft.Data.SAPClient
Imports CKG.Base.Business


Public Class FFE_Lastschriften
    Inherits CKG.Base.Business.DatenimportBase
#Region "Declarations"

    Private m_vonDate As String
    Private m_bisDate As String
    Private m_strHaendler As String
    Private ErledigtFlag As String

#End Region


#Region "Properties"

    Public Property datVON() As String
        Get
            Return m_vonDate

        End Get
        Set(ByVal value As String)
            m_vonDate = value
        End Set
    End Property

    Public Property datBIS() As String
        Get
            Return m_bisDate

        End Get
        Set(ByVal value As String)
            m_bisDate = value
        End Set
    End Property

    Public Property Haendler() As String
        Get
            Return m_strHaendler

        End Get
        Set(ByVal value As String)
            m_strHaendler = value
        End Set
    End Property

    Public Property FlagErledigt() As String
        Get
            Return ErledigtFlag

        End Get
        Set(ByVal value As String)
            ErledigtFlag = value
        End Set
    End Property
#End Region

#Region "Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID)
    End Sub


    'Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
    '    Dim intID As Int32 = -1

    '    'Kundennummer ermitteln und in SAP-Format wandeln
    '    Dim strKUNNR As String = Right("0000000000" & Me.m_objUser.Customer.KUNNR, 10)
    '    ' Dim strHaendler As String = Right("0000000000" & 60 & Me.m_objUser.Reference, 10)
    '    If Not m_blnGestartet Then
    '        m_blnGestartet = True

    '        Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
    '        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
    '        objSAP.Connection.Open()

    '        Try
    '            Dim SAPTable As New SAPProxy_FFE.ZVLASTSCHRIFT_S002Table()
    '            Dim tmpDateab As Date
    '            Dim tmpDatebis As Date
    '            If IsDate(m_vonDate) Then
    '                tmpDateab = CDate(m_vonDate)
    '                m_vonDate = MakeDateSAP(tmpDateab)
    '            Else
    '                m_vonDate = Nothing
    '            End If
    '            If IsDate(m_bisDate) Then
    '                tmpDatebis = CDate(m_bisDate)
    '                m_bisDate = MakeDateSAP(tmpDatebis)
    '            Else
    '                m_bisDate = Nothing
    '            End If
    '            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_V_Lastschrift_Get", Me.m_strAppID, Me.m_strSessionID, m_objUser.CurrentLogAccessASPXID)
    '            objSAP.Z_M_Lastschrift_Haendler_Get(strKUNNR, m_bisDate, m_vonDate, m_strHaendler, SAPTable)
    '            objSAP.CommitWork()
    '            If intID > -1 Then
    '                m_objLogApp.WriteEndDataAccessSAP(intID, True)
    '            End If

    '            Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

    '            CreateOutPut(tblTemp2, strAppID)

    '            WriteLogEntry(True, "KUNNR=" & strKUNNR, m_tblResult, False)

    '        Catch ex As Exception
    '            Select Case ex.Message
    '                Case "NRF"
    '                    m_intStatus = -1402
    '                    m_strMessage = "Keine Daten gefunden."
    '                Case Else
    '                    m_intStatus = -5555
    '                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
    '            End Select
    '            If intID > -1 Then
    '                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
    '            End If
    '            WriteLogEntry(False, "KUNNR=" & strKUNNR & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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
    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)

        Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        con.Open()
        Dim cmd As New SAPCommand()
        Dim strCom As String
        Dim intID As Int32 = -1

        Dim strKUNNR As String = Right("0000000000" & Me.m_objUser.Customer.KUNNR, 10)
        Try
            If Not m_blnGestartet Then
                m_blnGestartet = True
                Dim tmpDateab As Date
                Dim tmpDatebis As Date
                If IsDate(m_vonDate) Then
                    tmpDateab = CDate(m_vonDate)
                    m_vonDate = MakeDateSAP(tmpDateab)
                Else
                    m_vonDate = "00000000"
                End If
                If IsDate(m_bisDate) Then
                    tmpDatebis = CDate(m_bisDate)
                    m_bisDate = MakeDateSAP(tmpDatebis)
                Else
                    m_bisDate = "00000000"
                End If
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Lastschrift_Haendler_Get", Me.m_strAppID, Me.m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                strCom = "EXEC Z_M_Lastschrift_Haendler_Get @AG='" & strKUNNR & "'," _
                                                       & "@DATUM_VON='" & m_vonDate & "'," _
                                                       & "@DATUM_BIS='" & m_bisDate & "'," _
                                                       & "@HAENDLER='" & m_strHaendler & "'," _
                                                       & "@ERLEDIGT='" & ErledigtFlag & "'," _
                                                       & "@OUT_TAB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"



                cmd.Connection = con
                cmd.CommandText = strCom

                Dim pSAPTable As New SAPParameter("@pSAPTable", ParameterDirection.Output)
                cmd.Parameters.Add(pSAPTable)


                cmd.ExecuteNonQuery()

                Dim SAPTable As DataTable = DirectCast(pSAPTable.Value, DataTable)

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.Copy

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & strKUNNR, m_tblResult, False)
            End If
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NRF"
                    m_intStatus = -1402
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -5555
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & strKUNNR & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            con.Close()
            con.Dispose()

            cmd.Dispose()


            m_blnGestartet = False
        End Try
    End Sub
    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal dteBaseDate As Date)
        Dim intID As Int32 = -1

        'Kundennummer ermitteln und in SAP-Format wandeln
        Dim strKUNNR As String = Right("0000000000" & Me.m_objUser.Customer.KUNNR, 10)

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Try
                Dim SAPTable As New SAPProxy_FFE.ZVLASTSCHRIFT_S001Table()

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_V_Lastschrift_Get", Me.m_strAppID, Me.m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_V_Lastschrift_Get(Me.MakeDateSAP(dteBaseDate.Date), strKUNNR, SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & strKUNNR, m_tblResult, False)

            Catch ex As Exception
                Select Case ex.Message
                    Case "NRF"
                        m_intStatus = -1402
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -5555
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & strKUNNR & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try

        End If

    End Sub
#End Region

End Class
' ************************************************
' $History: FFE_Lastschriften.vb $
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 16.07.08   Time: 11:34
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 10.06.08   Time: 17:24
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 6.06.08    Time: 9:21
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 5.06.08    Time: 13:12
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 5.06.08    Time: 13:04
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Applications/AppFFE/lib
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 9.04.08    Time: 13:32
' Created in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 9.04.08    Time: 13:06
' Updated in $/CKG/Applications/AppFFE/AppFFEWeb/lib
' ITA: 1790
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 3.04.08    Time: 11:19
' Created in $/CKG/Applications/AppFFE/AppFFEWeb/lib
' ITA 1790
' 
' ************************************************