Option Explicit Off
Option Strict Off

Imports System.Configuration
Imports ERPConnect
Public Class App

    Public Message As String
    Public BooError As Boolean

    Public Sub New()
 
    End Sub

    Private Shared Function ErpConnection() As R3Connection

        Dim con As R3Connection

        If ConfigurationManager.AppSettings("ProdSAP").ToString.ToUpper = "TRUE" Then
            con = New R3Connection(ConfigurationManager.AppSettings("SAPAppServerHost"), _
                      CShort(ConfigurationManager.AppSettings("SAPSystemNumber")), _
                      ConfigurationManager.AppSettings("SAPUsername"), _
                      ConfigurationManager.AppSettings("SAPPassword"), _
                     "DE", CShort(ConfigurationManager.AppSettings("SAPClient")))
        Else
            con = New R3Connection(ConfigurationManager.AppSettings("TESTSAPAppServerHost"), _
                     CShort(ConfigurationManager.AppSettings("TESTSAPSystemNumber")), _
                     ConfigurationManager.AppSettings("TESTSAPUsername"), _
                     ConfigurationManager.AppSettings("TESTSAPPassword"), _
                    "DE", CShort(ConfigurationManager.AppSettings("TESTSAPClient")))
        End If

        LIC.SetLic(ConfigurationManager.AppSettings("ErpConnectLicense"))

        Return con

    End Function



    Public Sub SetFreigabe(ByVal agenturnr As String, ByVal auftragsnummer As String)

        Dim con As R3Connection = ErpConnection()

        Try

            con.Open(False)

            Dim func = con.CreateFunction("Z_M_VERS_FREIGABE_BST_001")

            func.Exports("I_KUNNR_AG").ParamValue = "0000324044"
            func.Exports("I_AGENTUR").ParamValue = agenturnr
            func.Exports("I_SPERRE_WEB_ID").ParamValue = Auftragsnummer

            func.Execute()


            If func.Imports("O_OK").ToString() = "X" Then
                Message = "Ihr Auftrag wurde freigegeben."
            Else
                Message = "Ihr Auftrag konnte nicht freigegeben werden."
            End If

        Catch ex As Exception
            Message = "Ihr Auftrag konnte nicht freigegeben werden."
            BooError = True
        Finally
            con.Close()
        End Try

    End Sub



End Class
