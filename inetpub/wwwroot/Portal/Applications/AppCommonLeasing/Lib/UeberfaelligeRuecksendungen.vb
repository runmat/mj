Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
'Imports Microsoft.Data.SAPClient
Imports CKG.Base.Business

Public Class UeberfaelligeRuecksendungen

    Inherits Base.Business.BankBase

#Region " Declarations"
    Private mVorgaenge As DataTable
    Private mFaelligkeit As String

#End Region

#Region " Properties"

    Public ReadOnly Property Vorgaenge() As DataTable
        Get
            Return mVorgaenge
        End Get
    End Property

    Public Property Faelligkeit() As String
        Get
            Return mFaelligkeit
        End Get
        Set(ByVal value As String)
            mFaelligkeit = value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
     
    End Sub

    Public Overloads Overrides Sub Show()

        m_strClassAndMethod = "UeberfaelligeRuecksendungen.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            m_intStatus = 0


            'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            'con.Open()
            Try

                'Dim cmd As New SAPCommand()
                'cmd.Connection = con

                'Dim strCom As String

                'strCom = "EXEC Z_M_FAELLIGE_EQUI_LP @I_KUNNR=@pI_KUNNR,@I_FAELLIGKEIT=@pI_FAELLIGKEIT,@GT_WEB=@pI_GT_WEB, "
                'strCom = strCom & "@GT_WEB=@pE_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                'cmd.CommandText = strCom

                S.AP.InitExecute("Z_M_FAELLIGE_EQUI_LP", "I_KUNNR,I_FAELLIGKEIT", m_strKUNNR, Faelligkeit)

                ''importparameter
                'Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)
                'Dim pI_FAELLIGKEIT As New SAPParameter("@pI_FAELLIGKEIT", ParameterDirection.Input)
                'Dim pI_GT_WEB As New SAPParameter("@pI_GT_WEB", New DataTable)

                ''exportParameter
                'Dim pE_GT_WEB As New SAPParameter("@pE_GT_WEB", ParameterDirection.Output)

                ''Importparameter hinzufügen
                'cmd.Parameters.Add(pI_KUNNR)
                'cmd.Parameters.Add(pI_FAELLIGKEIT)
                'cmd.Parameters.Add(pI_GT_WEB)


                ''exportparameter hinzugfügen
                'cmd.Parameters.Add(pE_GT_WEB)

                ''befüllen der Importparameter
                'pI_KUNNR.Value = Right("0000000000" & m_objUser.KUNNR, 10)
                'pI_FAELLIGKEIT.Value = Faelligkeit

                'If m_objLogApp Is Nothing Then
                '    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                'End If

                'intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_GET_UeberfaelligeRuecksendungen", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                'cmd.ExecuteNonQuery()

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                'End If

                'auswerten der exportparameter
                'If Not pE_GT_WEB.Value Is DBNull.Value Then

                mVorgaenge = S.AP.GetExportTable("GT_WEB") 'DirectCast(pE_GT_WEB.Value, DataTable)

                'HelpProcedures.killAllDBNullValuesInDataTable(mVorgaenge)

                mVorgaenge = CreateOutPut(mVorgaenge, AppID)
                mVorgaenge.AcceptChanges()

                For Each tmprow As DataRow In mVorgaenge.Rows
                    tmprow("Faelligkeitsdatum") = tmprow("Faelligkeitsdatum")
                Next

                mVorgaenge.AcceptChanges()
                'End If

            Catch ex As Exception
                mVorgaenge = Nothing
                m_intStatus = -9999

                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                'End If
            Finally
                'If intID > -1 Then
                '    m_objLogApp.WriteStandardDataAccessSAP(intID)
                'End If

                'con.Close()

                m_blnGestartet = False
            End Try
        End If

    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Change(ByVal EQUNR As String, ByVal text As String, ByVal Verlaengerung As String)
        m_strClassAndMethod = "UeberfaelligeRuecksendungen.Change"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            m_intStatus = 0

            'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            'con.Open()
            Try

                S.AP.InitExecute("Z_M_FAELLIGE_EQUI_UPDATE_LP", "I_KUNNR,I_EQUNR,I_FALLIG_VERLAENGERN,I_TEXT200",
                                 m_strKUNNR, EQUNR, Verlaengerung, text)
            

                'Dim cmd As New SAPCommand()
                'cmd.Connection = con

                'Dim strCom As String

                'strCom = "EXEC Z_M_FAELLIGE_EQUI_UPDATE_LP @I_KUNNR=@pI_KUNNR,@I_EQUNR=@pI_EQUNR,"
                'strCom = strCom & "@I_TEXT200=@pI_TEXT200,@I_FALLIG_VERLAENGERN=@pI_FALLIG_VERLAENGERN"

                'cmd.CommandText = strCom


                ''importparameter
                'Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)
                'Dim pI_EQUNR As New SAPParameter("@pI_EQUNR", ParameterDirection.Input)
                'Dim pI_TEXT200 As New SAPParameter("@pI_TEXT200", ParameterDirection.Input)
                'Dim pI_FALLIG_VERLAENGERN As New SAPParameter("@pI_FALLIG_VERLAENGERN", ParameterDirection.Input)


                ''Importparameter hinzufügen
                'cmd.Parameters.Add(pI_KUNNR)
                'cmd.Parameters.Add(pI_EQUNR)
                'cmd.Parameters.Add(pI_TEXT200)
                'cmd.Parameters.Add(pI_FALLIG_VERLAENGERN)



                ''befüllen der Importparameter
                'pI_KUNNR.Value = Right("0000000000" & m_objUser.KUNNR, 10)
                'pI_EQUNR.Value = EQUNR
                'pI_FALLIG_VERLAENGERN.Value = Verlaengerung
                'pI_TEXT200.Value = text

                'If m_objLogApp Is Nothing Then
                '    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                'End If

                'intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_GET_UeberfaelligeRuecksendungen", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                'cmd.ExecuteNonQuery()

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                'End If
            Catch ex As Exception
                m_intStatus = -9999

                Select Case ex.Message
                    Case Else
                        m_strMessage = "Beim Update ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                'End If
            Finally
                'If intID > -1 Then
                '    m_objLogApp.WriteStandardDataAccessSAP(intID)
                'End If

                'con.Close()

                m_blnGestartet = False
            End Try
        End If

    End Sub

#End Region

End Class

' ************************************************
' $History: UeberfaelligeRuecksendungen.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 20.10.08   Time: 8:35
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' 2286 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 15.10.08   Time: 9:50
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2286 und hinzufügen von JBC,FW,Unicredit -styles
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 14.10.08   Time: 13:58
' Created in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2286 unfertig
' ************************************************