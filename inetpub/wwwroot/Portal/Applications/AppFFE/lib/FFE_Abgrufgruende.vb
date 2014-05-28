Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports Microsoft.Data.SAPClient

Public Class FFE_Abgrufgruende
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_datAbDatum As String
    Private m_datBisDatum As String
    Private m_strSAPWert As String = ""
    Private m_strKunnr As String = ""
    Private tblGruende As DataTable
#End Region

#Region " Properties"

    Public Property Abrufgruende() As DataTable
        Get
            Return tblGruende
        End Get
        Set(ByVal Value As DataTable)
            tblGruende = Value
        End Set
    End Property

    Public Property datVON() As String
        Get
            Return m_datAbDatum
        End Get
        Set(ByVal Value As String)
            m_datAbDatum = Value
        End Set
    End Property

    Public Property datBIS() As String
        Get
            Return m_datBisDatum
        End Get
        Set(ByVal Value As String)
            m_datBisDatum = Value
        End Set
    End Property


    Public Property SAPWert() As String
        Get
            Return m_strSAPWert
        End Get
        Set(ByVal Value As String)
            m_strSAPWert = Value
        End Set
    End Property
    Public Property Kunnr() As String
        Get
            Return m_strKunnr
        End Get
        Set(ByVal Value As String)
            m_strKunnr = Value
        End Set
    End Property
#End Region


    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strsessionid)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)

      
        Dim cmd As New SAPCommand()
        Dim strCom As String
        Dim intID As Int32 = -1
        'DirectCast(DirectCast(DirectCast(ex, Microsoft.Data.SAPClient.SAPException).InnerException, System.Exception), Microsoft.Adapters.SAP.RFCException).SapErrorMessage()
        Dim strKUNNR As String = Right("0000000000" & Me.m_objUser.Customer.KUNNR, 10)

        Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        con.Open()
        Try
            If Not m_blnGestartet Then
                m_blnGestartet = True
                Dim tmpDateab As Date
                Dim tmpDatebis As Date
                If IsDate(m_datAbDatum) Then
                    tmpDateab = CDate(m_datAbDatum)
                    m_datAbDatum = MakeDateSAP(tmpDateab)
                Else
                    m_datAbDatum = "00000000"
                End If
                If IsDate(m_datBisDatum) Then
                    tmpDatebis = CDate(m_datBisDatum)
                    m_datBisDatum = MakeDateSAP(tmpDatebis)
                Else
                    m_datBisDatum = "00000000"
                End If
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Lastschrift_Haendler_Get", Me.m_strAppID, Me.m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                strCom = "EXEC Z_M_VERSAND_JE_ABRUFGRUND_FCE @I_AG='" & strKUNNR & "'," _
                                                       & "@I_DAT_VON='" & m_datAbDatum & "'," _
                                                       & "@I_DAT_BIS='" & m_datBisDatum & "'," _
                                                       & "@I_VSGRUND='" & m_strSAPWert & "'," _
                                                       & "@I_KUNNR_ZF='" & m_strKunnr & "'," _
                                                       & "@GT_VERSAND=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"



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
                Case "NO_DATA"
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
    Public Sub DBAbrufgrund(ByVal strGrundTyp As String)
        Dim cn As New SqlClient.SqlConnection
        Try
            m_intStatus = 0
            cn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            cn.Open()

            Dim da As New SqlClient.SqlDataAdapter("SELECT  TOP 100 PERCENT SapWert, WebBezeichnung " & _
                                            " FROM dbo.CustomerAbrufgruende " & _
                                            " WHERE (CustomerID = " & m_objUser.Customer.CustomerId & ") " & _
                                            " AND (SapWert <> '000') " & _
                                            " GROUP BY SapWert, WebBezeichnung, AbrufTyp " & _
                                            " HAVING (AbrufTyp = '" & strGrundTyp & "')" & _
                                            " ORDER BY WebBezeichnung", cn)
            tblGruende = New DataTable
            da.Fill(tblGruende)

        Catch ex As Exception
            m_intStatus = -99
            m_strMessage = ex.Message

        Finally
            cn.Close()
        End Try
    End Sub

End Class
' ************************************************
' $History: FFE_Abgrufgruende.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 30.04.09   Time: 15:42
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2837
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 30.07.08   Time: 14:34
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA:2091
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 7.07.08    Time: 10:49
' Updated in $/CKAG/Applications/AppFFE/lib
' Historie hinzugefgt
' 
' ************************************************