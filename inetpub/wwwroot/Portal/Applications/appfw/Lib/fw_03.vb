Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports Microsoft.Data.SAPClient

Public Class fw_03

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"

    Private m_datAbDatum As Date
    Private m_datBisDatum As Date
    Private m_strFilename2 As String
#End Region

#Region " Properties"



    Public Property datVON() As Date
        Get
            Return m_datAbDatum
        End Get
        Set(ByVal Value As Date)
            m_datAbDatum = Value
        End Set
    End Property

    Public Property datBIS() As Date
        Get
            Return m_datBisDatum
        End Get
        Set(ByVal Value As Date)
            m_datBisDatum = Value
        End Set
    End Property

    Public ReadOnly Property FileName() As String
        Get
            Return m_strFilename2
        End Get
    End Property

#End Region

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_strFilename2 = strFilename
    End Sub



    Public Overloads Sub Fill1()
        Fill1(m_strAppID, m_strsessionid)
    End Sub


    Public Overloads Sub Fill2()
        Fill2(m_strAppID, m_strsessionid)
    End Sub

    Public Overloads Sub FILL1(ByVal strAppID As String, ByVal strSessionID As String)
        'vollständig
        m_strClassAndMethod = "fw_03.FILL1"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_strMessage = ""
        m_intStatus = 0
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            con.Open()
            Try

                Dim cmd As New SAPCommand()
                cmd.Connection = con

                Dim strCom As String

                strCom = "EXEC Z_M_Dad_Fw_Dok_Vollst_001 @I_KUNNR=@pI_KUNNR,@I_DATUM_VON=@pI_DATUM_VON,@I_DATUM_BIS=@pI_DATUM_BIS,@GT_WEB=@pI_GT_WEB,"
                strCom = strCom & "@GT_WEB=@pE_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                cmd.CommandText = strCom

                'importparameter
                Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)
                Dim pI_DATUM_VON As New SAPParameter("@pI_DATUM_VON", ParameterDirection.Input)
                Dim pI_DATUM_BIS As New SAPParameter("@pI_DATUM_BIS", ParameterDirection.Input)
                Dim pI_GT_WEB As New SAPParameter("@pI_GT_WEB", New DataTable)

                'outputparameter
                Dim pE_GT_WEB As New SAPParameter("@pE_GT_WEB", ParameterDirection.Output)

                'Importparameter hinzufügen
                cmd.Parameters.Add(pI_KUNNR)
                cmd.Parameters.Add(pI_DATUM_VON)
                cmd.Parameters.Add(pI_DATUM_BIS)
                cmd.Parameters.Add(pI_GT_WEB)

                'exportparameter hinzufügen
                cmd.Parameters.Add(pE_GT_WEB)

                'befüllen der Importparameter
                pI_KUNNR.Value = Right("0000000000" & m_objUser.KUNNR, 10)
                pI_DATUM_VON.Value = HelpProcedures.MakeDateSAP(m_datAbDatum)
                pI_DATUM_BIS.Value = HelpProcedures.MakeDateSAP(m_datBisDatum)

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Dad_Fw_Dok_Vollst_001", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                cmd.ExecuteNonQuery()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If
                Dim tblTemp2 As DataTable = DirectCast(pE_GT_WEB.Value, DataTable)

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "DATANF=" & m_datAbDatum.ToShortDateString & ", DATEND=" & m_datBisDatum.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                Select Case (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "keine Ergebnisse gefunden"
                    Case Else
                        m_intStatus = -2222
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "DATANF=" & m_datAbDatum.ToShortDateString & " DATEND=" & m_datBisDatum.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                con.Close()
                m_blnGestartet = False
            End Try
        End If
    End Sub


    Public Overloads Sub FILL2(ByVal strAppID As String, ByVal strSessionID As String)
        'unvollständig
        m_strClassAndMethod = "fw_03.FILL2"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_strMessage = ""
        m_intStatus = 0
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            con.Open()
            Try

                Dim cmd As New SAPCommand()
                cmd.Connection = con

                Dim strCom As String

                strCom = "EXEC Z_M_Dad_Fw_Dok_Unvollst_001 @I_KUNNR=@pI_KUNNR,@GT_WEB=@pI_GT_WEB,"
                strCom = strCom & "@GT_WEB=@pE_GT_WEB OUTPUT OPTION 'disabledatavalidation'"
                cmd.CommandText = strCom

                'importparameter
                Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)
                Dim pI_GT_WEB As New SAPParameter("@pI_GT_WEB", New DataTable)

                'outputparameter
                Dim pE_GT_WEB As New SAPParameter("@pE_GT_WEB", ParameterDirection.Output)

                'Importparameter hinzufügen
                cmd.Parameters.Add(pI_KUNNR)
                cmd.Parameters.Add(pI_GT_WEB)

                'exportparameter hinzufügen
                cmd.Parameters.Add(pE_GT_WEB)

                'befüllen der Importparameter
                pI_KUNNR.Value = Right("0000000000" & m_objUser.KUNNR, 10)

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Dad_Fw_Dok_Vollst_001", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                cmd.ExecuteNonQuery()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If
                Dim tblTemp2 As DataTable = DirectCast(pE_GT_WEB.Value, DataTable)

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                Select Case (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "keine Ergebnisse gefunden"
                    Case Else
                        m_intStatus = -2222
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                con.Close()
                m_blnGestartet = False
            End Try
        End If
    End Sub
End Class
