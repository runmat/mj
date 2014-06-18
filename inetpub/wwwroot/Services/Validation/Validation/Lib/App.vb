Imports Microsoft.Data.SAPClient
Imports System.Configuration
Public Class App

    Private m_strSAPAppServerHost As String
    Private m_shortSAPSystemNumber As Short
    Private m_shortSAPClient As Short
    Private m_strSAPUsername As String
    Private m_strSAPPassword As String
    Private m_BizTalkSapConnectionString As String
    Public m_Message As String
    Public booError As Boolean

    Public Sub New(ByVal IsTest As Boolean)
        If Not CBool(ConfigurationManager.AppSettings("IsTest")) Then
            m_strSAPAppServerHost = ConfigurationManager.AppSettings("SAPAppServerHost")
            m_shortSAPSystemNumber = CShort(ConfigurationManager.AppSettings("SAPSystemNumber"))
            m_shortSAPClient = CShort(ConfigurationManager.AppSettings("SAPClient"))
            m_strSAPUsername = ConfigurationManager.AppSettings("SAPUsername")
            m_strSAPPassword = ConfigurationManager.AppSettings("SAPPassword")
        Else
            m_strSAPAppServerHost = ConfigurationManager.AppSettings("TESTSAPAppServerHost")
            m_shortSAPSystemNumber = CShort(ConfigurationManager.AppSettings("TESTSAPSystemNumber"))
            m_shortSAPClient = CShort(ConfigurationManager.AppSettings("TESTSAPClient"))
            m_strSAPUsername = ConfigurationManager.AppSettings("TESTSAPUsername")
            m_strSAPPassword = ConfigurationManager.AppSettings("TESTSAPPassword")
        End If

        m_BizTalkSapConnectionString = "ASHOST=" & m_strSAPAppServerHost & _
                                ";CLIENT=" & m_shortSAPClient & _
                                ";SYSNR=" & m_shortSAPSystemNumber & _
                                ";USER=" & m_strSAPUsername & _
                                ";PASSWD=" & m_strSAPPassword & _
                                ";LANG=DE"


    End Sub


    Public Sub SetFreigabe(ByVal Agenturnr As String, ByVal Auftragsnummer As String)

        Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        con.Open()

        Dim cmd As New SAPCommand()
        cmd.Connection = con

        Dim strCom As String

        strCom = "EXEC Z_M_VERS_FREIGABE_BST_001 @I_KUNNR_AG=@pI_KUNNR_AG,@I_AGENTUR=@pI_AGENTUR,"
        strCom &= "@I_SPERRE_WEB_ID=@pI_SPERRE_WEB_ID,@O_OK=@pO_OK OUTPUT OPTION 'disabledatavalidation'"

        cmd.CommandText = strCom

        Try


            Dim pI_KUNNR_AG As New SAPParameter("@pI_KUNNR_AG", ParameterDirection.Input)
            Dim pI_AGENTUR As New SAPParameter("@pI_AGENTUR", ParameterDirection.Input)
            Dim pI_SPERRE_WEB_ID As New SAPParameter("@pI_SPERRE_WEB_ID", ParameterDirection.Input)


            cmd.Parameters.Add(pI_KUNNR_AG)
            cmd.Parameters.Add(pI_AGENTUR)
            cmd.Parameters.Add(pI_SPERRE_WEB_ID)

            pI_KUNNR_AG.Value = "0000324044"
            pI_AGENTUR.Value = Agenturnr
            pI_SPERRE_WEB_ID.Value = Auftragsnummer



            'exportparameter
            Dim pE_O_OK As New SAPParameter("@pO_OK", ParameterDirection.Output)


            'exportparameter hinzufügen
            cmd.Parameters.Add(pE_O_OK)


            cmd.ExecuteNonQuery()

            If pE_O_OK.Value.ToString = "X" Then
                m_Message = "Ihr Auftrag wurde freigegeben."
            Else
                m_Message = "Ihr Auftrag konnte nicht freigegeben werden."
            End If


        Catch ex As Exception
            m_Message = "Ihr Auftrag konnte nicht freigegeben werden."
            booError = True
        Finally
            con.Close()
        End Try

    End Sub



End Class
