

Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

<Serializable()> Public Class FFE_Bank_Distrikt

    Inherits Base.Business.BankBase

    Dim tblRightsResult As DataTable
    Dim tblDistrictsResult As DataTable
    Dim m_sHaendler As String
    Dim m_sDistrikt As String
    Dim m_sKundNr As String
    Dim m_sKUNNR_HA As String
    Dim m_sDistriktID As String
    Dim m_sOrgaID As String
    Dim m_sOrgaName As String



#Region "Properties"
    Public Property Rights() As DataTable
        Get
            Return tblRightsResult
        End Get
        Set(ByVal Value As DataTable)
            tblRightsResult = Value
        End Set
    End Property

    Public Property Haendler() As String
        Get
            Return m_sHaendler
        End Get
        Set(ByVal Value As String)
            m_sHaendler = Value
        End Set
    End Property
    Public Property Districts() As DataTable
        Get
            Return tblDistrictsResult
        End Get
        Set(ByVal Value As DataTable)
            tblDistrictsResult = Value
        End Set
    End Property
    Public Property Distrikt() As String
        Get
            Return m_sDistrikt
        End Get
        Set(ByVal Value As String)
            m_sDistrikt = Value
        End Set
    End Property
    Public Property sDistriktID() As String
        Get
            Return m_sDistriktID
        End Get
        Set(ByVal Value As String)
            m_sDistriktID = Value
        End Set
    End Property
    Public Property sOrgaID() As String
        Get
            Return m_sOrgaID
        End Get
        Set(ByVal Value As String)
            m_sOrgaID = Value
        End Set
    End Property
    Public Property sOrgaName() As String
        Get
            Return m_sOrgaName
        End Get
        Set(ByVal Value As String)
            m_sOrgaName = Value
        End Set
    End Property
    Public ReadOnly Property KundNR() As String
        Get
            Return m_sKundNr
        End Get
    End Property
    Public ReadOnly Property KundNRHaendler() As String
        Get
            Return m_sKUNNR_HA
        End Get
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, "")
    End Sub

    Public Overrides Sub Show()
        m_strClassAndMethod = "FFD_Bank_Distrikt.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            Dim tblSapDistrikte As New SAPProxy_FFE.ZDAD_HAENDLER_S001Table()

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1
            Dim sKunnr As String = ""
            Dim sDistriktname As String = ""
            Dim sHaendler As String = Haendler
            Try
                m_intStatus = 0
                m_strMessage = ""

                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_V_Haendlerzuordnung_Get", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                objSAP.Z_V_Haendlerzuordnung_Get("1510", sHaendler, sKunnr, sDistriktname, tblSapDistrikte)
                objSAP.CommitWork()
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If
                Distrikt = sDistriktname

                'tblDistrictsResult = tblSapDistrikte.ToADODataTable

                'Dim dRow As DataRow
                'For Each dRow In tblDistrictsResult.Rows
                '    Dim sID As String
                '    sID = dRow("KUNNR_DI").ToString
                '    sID = sID.TrimStart("0"c)
                '    sID = Right(sID, sID.Length - 1)
                '    sID = sID.TrimStart("0"c)
                '    dRow("KUNNR_DI") = sID
                '    tblDistrictsResult.AcceptChanges()
                'Next

                WriteLogEntry(True, "Händlerdistrikt eingelesen", tblRightsResult)
            Catch ex As Exception
                Select Case ex.Message
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
                WriteLogEntry(False, "Fehler beim Einlesen des Händlerdistriktes", tblRightsResult)
            Finally
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
    Public Sub GiveReference()

        m_strClassAndMethod = "FFD_Bank_Distrikt.GiveReference"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            Dim tblSapDistrikte As New SAPProxy_FFE.ZDAD_HAENDLER_S001Table()

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1
            Dim sKunnr As String = ""
            Dim sDistriktname As String = ""
            Dim sHaendler As String = Haendler
            Try
                m_intStatus = 0
                m_strMessage = ""

                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_V_Haendlerzuordnung_Get", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                objSAP.Z_V_Haendlerzuordnung_Get("1510", sHaendler, sKunnr, sDistriktname, tblSapDistrikte)
                objSAP.CommitWork()
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If
                tblDistrictsResult = tblSapDistrikte.ToADODataTable

                Dim dRow As DataRow
                For Each dRow In tblDistrictsResult.Rows
                    Dim sID As String
                    sID = dRow("KUNNR_DI").ToString
                    sID = sID.TrimStart("0"c)
                    sID = Right(sID, sID.Length - 1)
                    sID = sID.TrimStart("0"c)
                    dRow("KUNNR_DI") = sID

                    tblDistrictsResult.AcceptChanges()
                    If sDistriktname = dRow("NAME1").ToString Then
                        m_sDistriktID = dRow("KUNNR_DI").ToString
                    End If
                Next
                WriteLogEntry(True, "Händlerdistrikt eingelesen", tblRightsResult)
            Catch ex As Exception
                Select Case ex.Message
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
                WriteLogEntry(False, "Fehler beim Einlesen des Händlerdistriktes", tblRightsResult)
            Finally
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
    Public Sub ShowOrgas()

        m_strClassAndMethod = "FFD_Bank_Distrikt.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            If tblDistrictsResult Is Nothing Then
                Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Dim cmdAg As SqlClient.SqlCommand
                Dim dsAg As DataSet
                Dim adAg As SqlClient.SqlDataAdapter

                Try

                    cn.Open()

                    dsAg = New DataSet()

                    adAg = New SqlClient.SqlDataAdapter()

                    cmdAg = New SqlClient.SqlCommand("SELECT " & _
                                                    "[OrganizationID]," & _
                                                    "[OrganizationName]," & _
                                                    "[OrganizationReference] " & _
                                                    "FROM Organization " & _
                                                    "WHERE " & _
                                                    "CustomerID = " & m_objUser.Customer.CustomerId.ToString & _
                                                    " AND OrganizationReference <>''" & _
                                                    " Order by OrganizationName" _
                                                    , cn)
                    cmdAg.CommandType = CommandType.Text
                    'AbrufTyp: 'temp' oder 'endg'

                    adAg.SelectCommand = cmdAg
                    adAg.Fill(dsAg, "Organization")

                    If dsAg.Tables("Organization") Is Nothing OrElse dsAg.Tables("Organization").Rows.Count = 0 Then
                        Throw New Exception("Keine Organization für den Kunden hinterlegt.")
                    End If

                    tblDistrictsResult = dsAg.Tables("Organization")

                Catch ex As Exception
                    Throw ex
                Finally
                    cn.Close()
                End Try
            End If

        End If
    End Sub
    Public Sub ShowUserOrg()

        m_strClassAndMethod = "FFD_Bank_Distrikt.Show"


        If tblDistrictsResult Is Nothing Then
            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmdAg As SqlClient.SqlCommand
            Dim dsAg As DataSet
            Dim adAg As SqlClient.SqlDataAdapter
            m_sOrgaName = ""
            Try

                cn.Open()

                dsAg = New DataSet()

                adAg = New SqlClient.SqlDataAdapter()

                cmdAg = New SqlClient.SqlCommand("SELECT " & _
                                                " dbo.Organization.OrganizationName,dbo.Organization.OrganizationReference " & _
                                                " FROM dbo.WebUser INNER JOIN " & _
                                                " dbo.OrganizationMember ON dbo.WebUser.UserID = dbo.OrganizationMember.UserID INNER JOIN" & _
                                                " dbo.Organization ON dbo.OrganizationMember.OrganizationID = dbo.Organization.OrganizationID " & _
                                                " WHERE dbo.WebUser.Reference = @Haendler", cn)
                cmdAg.CommandType = CommandType.Text
                cmdAg.Parameters.AddWithValue("@Haendler", Haendler)
                Dim dr As SqlClient.SqlDataReader = cmdAg.ExecuteReader

                While dr.Read
                    m_sOrgaName = dr("OrganizationName").ToString
                    m_sOrgaID = dr("OrganizationReference").ToString
                End While

            Catch ex As Exception
                Throw ex
            Finally
                cn.Close()
            End Try

        End If
    End Sub
    Public Overrides Sub Change()

        Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()


        MakeDestination()
        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        objSAP.Connection.Open()

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If
        m_intIDSAP = -1

        Try
            m_intStatus = 0
            m_strMessage = ""
            Dim ID As String = ""
            ID = Right("000000" & (sDistriktID), 6)
            ID = Right("0000000000" & ("6" & ID), 10)
            objSAP.Z_V_Haendlerzuordnung_Change("1510", ID, Haendler)
            objSAP.CommitWork()

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
            End If
        Catch ex As Exception
            Select Case ex.Message

                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
            End If
        Finally
            objSAP.Connection.Close()
            objSAP.Dispose()
            'm_blnGestartet = False
        End Try
    End Sub
    Public Sub Change2()

        Try
            m_intStatus = 0
            m_strMessage = ""
            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

            Dim strUpdate As String = "UPDATE dbo.OrganizationMember " & _
            " Set dbo.OrganizationMember.OrganizationID = " & sOrgaID & _
            " FROM dbo.WebUser INNER JOIN dbo.OrganizationMember ON  " & _
            " dbo.WebUser.UserID = dbo.OrganizationMember.UserID " & _
            " WHERE     (dbo.WebUser.Reference = '" & Haendler & " ') "
            cn.Open()
            Dim cmd As New SqlClient.SqlCommand()
            cmd.CommandText = strUpdate
            cmd.Connection = cn
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = ex.Message
        End Try
    End Sub
#End Region
End Class
' ************************************************
' $History: FFE_Bank_Distrikt.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 16.07.08   Time: 11:34
' Updated in $/CKAG/Applications/AppFFE/lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugefgt
' 
' ************************************************
