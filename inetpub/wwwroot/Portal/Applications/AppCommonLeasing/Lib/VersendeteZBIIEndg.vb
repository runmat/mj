Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
'Imports Microsoft.Data.SAPClient

Public Class VersendeteZBIIEndg
    Inherits Base.Business.DatenimportBase

#Region "Deklarations"
    Private mDatumAb As String = ""
    Private mDatumBis As String = ""
    Private mLeasingvertragsnummer As String = ""
    Private mKennzeichen As String = ""
    Private mFahrgestellnummer As String = ""
#End Region

#Region "Properties"
    Public Property DatumAb() As String
        Get
            Return mDatumAb
        End Get
        Set(ByVal value As String)
            mDatumAb = value
        End Set
    End Property

    Public Property DatumBis() As String
        Get
            Return mDatumBis
        End Get
        Set(ByVal value As String)
            mDatumBis = value
        End Set
    End Property

    Public Property Leasingvertragsnummer() As String
        Get
            Return mLeasingvertragsnummer
        End Get
        Set(ByVal value As String)
            mLeasingvertragsnummer = value
        End Set
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return mFahrgestellnummer
        End Get
        Set(ByVal value As String)
            mFahrgestellnummer = value
        End Set
    End Property

    Public Property Kennzeichen() As String
        Get
            Return mKennzeichen
        End Get
        Set(ByVal value As String)
            mKennzeichen = value
        End Set
    End Property

#End Region

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String)
        m_strClassAndMethod = "VersendeteZBIIEndg.Fill"
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

                'strCom = "EXEC Z_M_VERSENDETE_ZB2_ENDG_LT "
                'strCom = strCom & "@I_KUNNR_AG=@pI_KUNNR_AG,@I_CHASSIS_NUM=@pI_CHASSIS_NUM,@I_LICENSE_NUM=@pI_LICENSE_NUM,@I_LIZNR=@pI_LIZNR,@I_ZZTMPDT_VON=@pI_ZZTMPDT_VON,@I_ZZTMPDT_BIS=@pI_ZZTMPDT_BIS,@GT_WEB=@pE_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                'cmd.CommandText = strCom

                S.AP.InitExecute("Z_M_VERSENDETE_ZB2_ENDG_LT ", "I_KUNNR_AG,I_CHASSIS_NUM,I_LICENSE_NUM,I_LIZNR,I_ZZTMPDT_VON,I_ZZTMPDT_BIS",
                                 Right("0000000000" & m_objUser.KUNNR, 10), mFahrgestellnummer, mKennzeichen, mLeasingvertragsnummer, mDatumAb, mDatumBis)

                ''importparameter

                'Dim pI_KUNNR_AG As New SAPParameter("@pI_KUNNR_AG", ParameterDirection.Input)
                'Dim pI_LIZNR As New SAPParameter("@pI_LIZNR", ParameterDirection.Input)
                'Dim pI_ZZTMPDT_VON As New SAPParameter("@pI_ZZTMPDT_VON", ParameterDirection.Input)
                'Dim pI_ZZTMPDT_BIS As New SAPParameter("@pI_ZZTMPDT_BIS", ParameterDirection.Input)
                'Dim pI_CHASSIS_NUM As New SAPParameter("@pI_CHASSIS_NUM", ParameterDirection.Input)
                'Dim pI_LICENSE_NUM As New SAPParameter("@pI_LICENSE_NUM", ParameterDirection.Input)

                ''exportParameter
                'Dim pE_GT_WEB As New SAPParameter("@pE_GT_WEB", ParameterDirection.Output)

                ''Importparameter hinzufügen
                'cmd.Parameters.Add(pI_KUNNR_AG)
                'cmd.Parameters.Add(pI_LIZNR)
                'cmd.Parameters.Add(pI_ZZTMPDT_VON)
                'cmd.Parameters.Add(pI_ZZTMPDT_BIS)
                'cmd.Parameters.Add(pI_CHASSIS_NUM)
                'cmd.Parameters.Add(pI_LICENSE_NUM)


                ''exportparameter hinzugfügen
                'cmd.Parameters.Add(pE_GT_WEB)


                ''befüllen der Importparameter
                'pI_KUNNR_AG.Value = Right("0000000000" & m_objUser.KUNNR, 10)
                'pI_LIZNR.Value = mLeasingvertragsnummer
                'pI_ZZTMPDT_BIS.Value = MakeDateSAP(mDatumBis)
                'pI_ZZTMPDT_VON.Value = MakeDateSAP(mDatumAb)
                'pI_CHASSIS_NUM.Value = mFahrgestellnummer
                'pI_LICENSE_NUM.Value = mKennzeichen



                'If m_objLogApp Is Nothing Then
                '    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                'End If

                'intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_VERSENDETE_ZB2_ENDG_LT ", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                'cmd.ExecuteNonQuery()

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                'End If

                'auswerten der exportparameter
                ResultTable = S.AP.GetExportTable("GT_WEB") 'DirectCast(pE_GT_WEB.Value, DataTable)

                ResultTable.Columns.Add("Versandadresse", String.Empty.GetType)

                'HelpProcedures.killAllDBNullValuesInDataTable(ResultTable)

                For Each tmpRow As DataRow In ResultTable.Rows
                    'versandadresse zusammenbauen
                    tmpRow("Versandadresse") = CStr(tmpRow("NAME1_ZS")) & " " & CStr(tmpRow("NAME2_ZS")) & "<br>" & CStr(tmpRow("STRAS_ZS")) & " " & CStr(tmpRow("HSNR_ZS")) & "<br>" & CStr(tmpRow("PSTLZ_ZS")) & " " & CStr(tmpRow("ORT01_ZS"))

                    'versandart
                    Select Case tmpRow("ABCKZ").ToString
                        Case "1"
                            tmpRow("ABCKZ") = "temporär versendet"
                        Case "2"
                            tmpRow("ABCKZ") = "endgültig versendet"
                        Case Else
                            tmpRow("ABCKZ") = "DAD"
                    End Select
                Next

                ResultTable.AcceptChanges()
                CreateOutPut(ResultTable, strAppID)

                WriteLogEntry(True, "", m_tblResult)
            Catch ex As Exception
                ResultTable = Nothing
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

End Class

' ************************************************
' $History: VersendeteZBIIEndg.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 4.11.08    Time: 16:58
' Created in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2354 testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 3.11.08    Time: 15:53
' Created in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2354 warte auf testdaten
' 
' ************************************************