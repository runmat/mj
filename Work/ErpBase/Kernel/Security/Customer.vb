Imports System.Web
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Configuration
Imports GeneralTools.Models

Namespace Kernel.Security
    <Serializable()> Public Class Customer
        REM § Dient der Haltung und Bearbeitung von Kundendaten aus der SQL DB

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_intCustomerID As Integer
        Private m_strCustomerName As String
        Private m_strDocuPath As String
        Private m_strKUNNR As String
        Private m_blnReadDealer As Boolean
        Private m_selfAdministration As Integer
        Private m_selfAdministrationInfo As String
        Private m_selfAdministrationContact As String
        Private m_Locked As Boolean
        Private m_blnMaster As Boolean
        Private m_blnAllowMultipleLogin As Boolean
        Private m_blnAllowUrlRemoteLogin As Boolean
        Private m_CustomerContact As Contact
        Private m_CustomerPasswordRules As PasswordRules
        Private m_CustomerLoginRules As LoginRules
        Private m_CustomerStyle As Style
        Private m_intMaxUser As Integer
        Private m_blnShowOrganization As Boolean
        Private m_blnShowDistrikte As Boolean
        Private m_strHeaderBackgroundPath As String
        Private m_strLogoPath2 As String
        Private m_strLogoPath As String
        Private m_blnOrgAdminRestrictToCustomerGroup As Boolean
        Private m_blnNameInputOptional As Boolean
        Private m_blnPwdDontSendEmail As Boolean
        <NonSerialized()> Private m_blnForcePasswordQuestion As Boolean
        <NonSerialized()> Private m_blnIpRestriction As Boolean
        <NonSerialized()> Private m_strIpStandardUser As String
        <NonSerialized()> Private m_intAccountingArea As Integer
        <NonSerialized()> Private m_tblIpAddresses As DataTable
        Private m_intDaysUntilLock As Integer
        Private m_intDaysUntilDelete As Integer
        Private m_UrCustomerUsernameRules As UsernameRules = New UsernameRules(True)
        'Private m_blnUsernameDontSendEmail As Boolean = False
        Private m_blnTVShow As Boolean
        Private m_intLoginLinkID As Integer
        Private m_DcDatCredentials As DatCredentials
        Private m_PortalType As String
        Private m_MvcSelectionUrl As String
        Private m_MvcSelectionType As String
        Private m_ReferenceType1 As String
        Private m_ReferenceType2 As String
        Private m_ReferenceType3 As String
        Private m_ReferenceType4 As String
        Private m_ReferenceType1Name As String
        Private m_ReferenceType2Name As String
        Private m_ReferenceType3Name As String
        Private m_ReferenceType4Name As String
        Private m_blnForceSpecifiedLoginLink As Boolean
        Private m_strLogoutLink As String
        Private m_strLoginLink As String

#End Region

#Region " Constructor "

        Public Sub New(ByVal intCustomerID As Integer)
            m_intCustomerID = intCustomerID
        End Sub

        Public Sub New(ByVal intCustomerID As Integer, _
                               ByVal strCustomerName As String, _
                               ByVal strKUNNR As String, _
                               ByVal blnMaster As Boolean, _
                               ByVal blnReadDealer As Boolean, _
                               ByVal strCName As String, _
                               ByVal strCAddress As String, _
                               ByVal strCMailDisplay As String, _
                               ByVal strCMail As String, _
                               ByVal strKundePostf As String, _
                               ByVal strKundeHotl As String, _
                               ByVal strKundeFax As String, _
                               ByVal strCWebDisplay As String, _
                               ByVal strCWeb As String, _
                               ByVal intNewPwdAfterNDays As Integer, _
                               ByVal intLockedAfterNLogins As Integer, _
                               ByVal intPwdNNumeric As Integer, _
                               ByVal intPwdLength As Integer, _
                               ByVal intPwdNCapitalLetter As Integer, _
                               ByVal intPwdNSpecialCharacter As Integer, _
                               ByVal intPwdHistoryNEntries As Integer, _
                               ByVal strLogoPath As String, _
                               ByVal strLogoPath2 As String, _
                               ByVal strHeaderBackgroundPath As String, _
                               ByVal strDocuPath As String, _
                               ByVal strCssPath As String, _
                               ByVal blnAllowMultipleLogin As Boolean, _
                               ByVal blnAllowUrlRemoteLogin As Boolean, _
                               ByVal intMaxUser As Integer, _
                               ByVal blnShowOrganization As Boolean, _
                               ByVal blnOrgAdminRestrictToCustomerGroup As Boolean, _
                               ByVal blnPwdDontSendEmail As Boolean, _
                               ByVal blnNameInputOptional As Boolean, _
                               ByVal blnShowDistrikte As Boolean, _
                               ByVal blnForcePasswordQuestion As Boolean, _
                               ByVal blnIpRestriction As Boolean, _
                               ByVal strIpStandardUser As String, _
                               ByVal intAccountingArea As Integer, _
                               ByVal intSelfAdministration As Integer, _
                               ByVal strSelfAdministrationInfo As String, _
                               ByVal strSelfAdministrationContact As String, _
                               ByVal blnLocked As Boolean, _
                               ByVal blnTVShow As Boolean, _
                               ByVal blnUsernameDontSendEmail As Boolean, _
                               ByVal intLoginLinkID As Integer, _
                               ByVal strPortalType As String, _
                               ByVal blnForceSpecifiedLoginLink As Boolean, _
                               ByVal strLogoutLink As String, _
                               ByVal strReferenzTyp1 As String, _
                               ByVal strReferenzTyp2 As String, _
                               ByVal strReferenzTyp3 As String, _
                               ByVal strReferenzTyp4 As String, _
                               Optional ByVal intDaysUntilLock As Integer = 90, _
                               Optional ByVal intDaysUntilDelete As Integer = 9999, _
                               Optional ByVal strSDCustomerNumber As String = "", _
                               Optional ByVal strSDUserName As String = "", _
                               Optional ByRef strSDPassword As String = "", _
                               Optional ByVal strSDUserLogin As String = "", _
                               Optional ByRef strSDSignatur As String = "", _
                               Optional ByRef strSDSignatur2 As String = "", _
                               Optional ByRef strMvcSelectionUrl As String = "", _
                               Optional ByRef strMvcSelectionType As String = "")

            m_intCustomerID = intCustomerID
            m_strCustomerName = strCustomerName
            m_strDocuPath = strDocuPath
            m_strKUNNR = strKUNNR
            m_blnReadDealer = blnReadDealer
            m_blnMaster = blnMaster
            m_CustomerContact = New Contact(strCName, strCAddress, strCMailDisplay, strCMail, strCWebDisplay, strCWeb, strKundePostf, strKundeHotl, strKundeFax)
            m_CustomerPasswordRules = New PasswordRules(intPwdNNumeric, intPwdLength, intPwdNCapitalLetter, intPwdNSpecialCharacter, intPwdHistoryNEntries, blnPwdDontSendEmail, blnNameInputOptional)
            m_CustomerLoginRules = New LoginRules(intLockedAfterNLogins, intNewPwdAfterNDays)
            m_CustomerStyle = New Style(strLogoPath, strCssPath)
            m_strLogoPath2 = strLogoPath2
            m_strHeaderBackgroundPath = strHeaderBackgroundPath
            m_blnAllowMultipleLogin = blnAllowMultipleLogin
            m_blnAllowUrlRemoteLogin = blnAllowUrlRemoteLogin
            m_intMaxUser = intMaxUser
            m_blnShowOrganization = blnShowOrganization
            m_blnOrgAdminRestrictToCustomerGroup = blnOrgAdminRestrictToCustomerGroup
            m_blnPwdDontSendEmail = blnPwdDontSendEmail
            m_blnNameInputOptional = blnNameInputOptional
            m_blnShowDistrikte = blnShowDistrikte
            m_blnForcePasswordQuestion = blnForcePasswordQuestion
            m_blnIpRestriction = blnIpRestriction
            m_strIpStandardUser = strIpStandardUser
            m_intAccountingArea = intAccountingArea
            m_selfAdministration = intSelfAdministration
            m_selfAdministrationInfo = Left(strSelfAdministrationInfo, 200)
            m_selfAdministrationContact = Left(strSelfAdministrationContact, 400)
            m_Locked = blnLocked
            m_intDaysUntilLock = intDaysUntilLock
            m_intDaysUntilDelete = intDaysUntilDelete
            m_blnTVShow = blnTVShow
            m_UrCustomerUsernameRules = New UsernameRules(blnUsernameDontSendEmail)
            m_intLoginLinkID = intLoginLinkID
            m_DcDatCredentials = New DatCredentials(strSDCustomerNumber, strSDUserName, strSDPassword, strSDUserLogin, strSDSignatur, strSDSignatur2)
            m_PortalType = strPortalType
            m_blnForceSpecifiedLoginLink = blnForceSpecifiedLoginLink
            m_strLogoutLink = strLogoutLink
            m_ReferenceType1 = strReferenzTyp1
            m_ReferenceType2 = strReferenzTyp2
            m_ReferenceType3 = strReferenzTyp3
            m_ReferenceType4 = strReferenzTyp4
            m_MvcSelectionUrl = strMvcSelectionUrl
            m_MvcSelectionType = strMvcSelectionType
        End Sub

        Public Sub New(ByVal intCustomerID As Integer, ByVal _user As User)
            Me.New(intCustomerID, _user.App.Connectionstring)
        End Sub

        Public Sub New(ByVal intCustomerID As Integer, ByVal strConnectionString As String)
            Me.New(intCustomerID, New SqlClient.SqlConnection(strConnectionString))
        End Sub

        Public Sub New(ByVal intCustomerID As Integer, ByVal cn As SqlClient.SqlConnection)
            Dim blnCloseOnEnd As Boolean = False
            m_intCustomerID = intCustomerID
            If cn.State = ConnectionState.Closed Then
                cn.Open()
                blnCloseOnEnd = True
            End If
            GetCustomer(cn)
            If m_intLoginLinkID > 0 Then
                GetPortalLoginLink(cn)
            End If
            If blnCloseOnEnd Then
                cn.Close()
            End If
        End Sub

#End Region

#Region " Properties "

        Public Property AccountingArea() As Integer
            Get
                Return m_intAccountingArea
            End Get
            Set(ByVal value As Integer)
                m_intAccountingArea = value
            End Set
        End Property

        Public Property IpAddresses() As DataTable
            Get
                Return m_tblIpAddresses
            End Get
            Set(ByVal Value As DataTable)
                m_tblIpAddresses = Value
            End Set
        End Property

        Public Property IpStandardUser() As String
            Get
                Return m_strIpStandardUser
            End Get
            Set(ByVal Value As String)
                m_strIpStandardUser = Value
            End Set
        End Property

        Public Property IpRestriction() As Boolean
            Get
                Return m_blnIpRestriction
            End Get
            Set(ByVal Value As Boolean)
                m_blnIpRestriction = Value
            End Set
        End Property

        Public Property ForcePasswordQuestion() As Boolean
            Get
                Return m_blnForcePasswordQuestion
            End Get
            Set(ByVal Value As Boolean)
                m_blnForcePasswordQuestion = Value
            End Set
        End Property

        Public ReadOnly Property PwdDontSendEmail() As Boolean
            Get
                Return m_blnPwdDontSendEmail
            End Get
        End Property

        Public ReadOnly Property NameInputOptional() As Boolean
            Get
                Return m_blnNameInputOptional
            End Get
        End Property

        Public ReadOnly Property LogoPath2() As String
            Get
                Return m_strLogoPath2
            End Get
        End Property

        Public ReadOnly Property HeaderBackgroundPath() As String
            Get
                Return m_strHeaderBackgroundPath
            End Get
        End Property

        Public ReadOnly Property LogoPath() As String
            Get
                Return m_strLogoPath
            End Get
        End Property

        Public ReadOnly Property LogoImage() As IO.MemoryStream
            Get
                Dim fileName As String = HttpContext.Current.Server.MapPath(LogoPath2)
                If File.Exists(fileName) Then
                    Return New MemoryStream(File.ReadAllBytes(fileName))
                Else
                    Dim ms As MemoryStream = New MemoryStream()
                    Dim blank As Image = New Bitmap(1, 1)
                    With Graphics.FromImage(blank)
                        .FillRectangle(Brushes.White, 0, 0, 1, 1)
                    End With
                    blank.Save(ms, ImageFormat.Jpeg)
                    Return ms
                End If
            End Get
        End Property

        Public ReadOnly Property LogoImage2() As IO.MemoryStream
            Get

                Dim fs As New IO.FileStream("C:\Inetpub\wwwroot" & Replace(LogoPath, "/", "\"), IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
                Dim br As New IO.BinaryReader(fs)
                Dim ms As New IO.MemoryStream(br.ReadBytes(fs.Length))
                br.Close()
                fs.Close()
                Return ms
            End Get
        End Property

        Public ReadOnly Property MvcLayout() As String
            Get
                Return m_PortalType
            End Get
        End Property

        Public ReadOnly Property MvcLayoutAsWebFormsInline() As Boolean
            Get
                Return (Not String.IsNullOrEmpty(m_PortalType)) And (m_PortalType.ToLower() <> "mvc")
            End Get
        End Property

        Public ReadOnly Property HasMvcApplicationsOnly() As Boolean
            Get
                Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                Dim countNoMvcApps As Integer
                Try
                    If cn.State = ConnectionState.Closed Then
                        cn.Open()
                    End If
                    Dim cmd As New SqlClient.SqlCommand("select COUNT(cr.AppID) from CustomerRights cr left outer join Application app on cr.AppID = app.AppID and (app.AppURL like 'mvc/%' or app.AppURL like 'http%') where cr.CustomerID = @CustomerID and app.AppID is null", cn)
                    cmd.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
                    countNoMvcApps = CInt(cmd.ExecuteScalar)

                Finally
                    If cn.State <> ConnectionState.Closed Then
                        cn.Close()
                    End If
                End Try

                Return countNoMvcApps = 0
            End Get
        End Property

        Public ReadOnly Property DocuPath() As String
            Get
                Return m_strDocuPath
            End Get
        End Property

        Public ReadOnly Property CustomerId() As Integer
            Get
                Return m_intCustomerID
            End Get
        End Property

        Public ReadOnly Property Selfadministration() As Integer
            Get
                Return m_selfAdministration
            End Get
        End Property

        Public ReadOnly Property Locked() As Boolean
            Get
                Return m_Locked
            End Get
        End Property

        Public ReadOnly Property SelfadministrationInfo() As String
            Get
                Return m_selfAdministrationInfo
            End Get
        End Property

        Public ReadOnly Property SelfadministrationContact() As String
            Get
                Return m_selfAdministrationContact
            End Get
        End Property

        Public ReadOnly Property CustomerName() As String
            Get
                Return m_strCustomerName
            End Get
        End Property

        Public ReadOnly Property KUNNR() As String
            Get
                Return m_strKUNNR
            End Get
        End Property

        Public ReadOnly Property ReadDealer() As Boolean
            Get
                Return m_blnReadDealer
            End Get
        End Property

        Public ReadOnly Property IsMaster() As Boolean
            Get
                Return m_blnMaster
            End Get
        End Property

        Public ReadOnly Property AllowMultipleLogin() As Boolean
            Get
                Return m_blnAllowMultipleLogin
            End Get
        End Property

        Public ReadOnly Property AllowUrlRemoteLogin() As Boolean
            Get
                Return m_blnAllowUrlRemoteLogin
            End Get
        End Property

        Public ReadOnly Property CustomerContact() As Contact
            Get
                Return m_CustomerContact
            End Get
        End Property

        Public ReadOnly Property CustomerPasswordRules() As PasswordRules
            Get
                Return m_CustomerPasswordRules
            End Get
        End Property

        Public ReadOnly Property CustomerUsernameRules() As UsernameRules
            Get
                Return m_UrCustomerUsernameRules
            End Get
        End Property

        Public ReadOnly Property CustomerLoginRules() As LoginRules
            Get
                Return m_CustomerLoginRules
            End Get
        End Property

        Public ReadOnly Property CustomerStyle() As Style
            Get
                Return m_CustomerStyle
            End Get
        End Property

        Public ReadOnly Property MaxUser() As Integer
            Get
                Return m_intMaxUser
            End Get
        End Property

        Public ReadOnly Property ShowOrganization() As Boolean
            Get
                Return m_blnShowOrganization
            End Get
        End Property

        Public ReadOnly Property ShowDistrikte() As Boolean
            Get
                Return m_blnShowDistrikte
            End Get
        End Property

        Public ReadOnly Property OrgAdminRestrictToCustomerGroup() As Boolean
            Get
                Return m_blnOrgAdminRestrictToCustomerGroup
            End Get
        End Property

        Public Property DaysUntilLock As Integer
            Get
                Return m_intDaysUntilLock
            End Get
            Set(value As Integer)
                m_intDaysUntilLock = value
            End Set
        End Property

        Public Property DaysUntilDelete As Integer
            Get
                Return m_intDaysUntilDelete
            End Get
            Set(value As Integer)
                m_intDaysUntilDelete = value
            End Set
        End Property

        Public Property ShowsTeamViewer As Boolean
            Get
                Return m_blnTVShow
            End Get
            Set(value As Boolean)
                m_blnTVShow = value
            End Set
        End Property

        Public Property LoginLinkID As Integer
            Get
                Return m_intLoginLinkID
            End Get
            Set(value As Integer)
                m_intLoginLinkID = value
            End Set
        End Property

        Public Property PortalType As String
            Get
                Return m_PortalType
            End Get
            Set(value As String)
                m_PortalType = value
            End Set
        End Property

        Public ReadOnly Property SilverDATCredentials As DatCredentials
            Get
                Return m_DcDatCredentials
            End Get
        End Property

        Public ReadOnly Property MvcSelectionUrl As String
            Get
                Return m_MvcSelectionUrl
            End Get
        End Property

        Public ReadOnly Property MvcSelectionType As String
            Get
                Return m_MvcSelectionType
            End Get
        End Property

        Public ReadOnly Property ReferenceType1 As String
            Get
                Return m_ReferenceType1
            End Get
        End Property

        Public ReadOnly Property ReferenceType2 As String
            Get
                Return m_ReferenceType2
            End Get
        End Property

        Public ReadOnly Property ReferenceType3 As String
            Get
                Return m_ReferenceType3
            End Get
        End Property

        Public ReadOnly Property ReferenceType4 As String
            Get
                Return m_ReferenceType4
            End Get
        End Property

        Public ReadOnly Property ReferenceType1Name As String
            Get
                Return m_ReferenceType1Name
            End Get
        End Property

        Public ReadOnly Property ReferenceType2Name As String
            Get
                Return m_ReferenceType2Name
            End Get
        End Property

        Public ReadOnly Property ReferenceType3Name As String
            Get
                Return m_ReferenceType3Name
            End Get
        End Property

        Public ReadOnly Property ReferenceType4Name As String
            Get
                Return m_ReferenceType4Name
            End Get
        End Property

        Public Property ForceSpecifiedLoginLink As Boolean
            Get
                Return m_blnForceSpecifiedLoginLink
            End Get
            Set(value As Boolean)
                m_blnForceSpecifiedLoginLink = value
            End Set
        End Property

        Public Property LogoutLink As String
            Get
                Return m_strLogoutLink
            End Get
            Set(value As String)
                m_strLogoutLink = value
            End Set
        End Property

        Public Property LoginLink As String
            Get
                Return m_strLoginLink
            End Get
            Set(value As String)
                m_strLoginLink = value
            End Set
        End Property

#End Region

#Region " Functions "

        Private Sub SetTVShowInGroups(ByVal blnShow As Boolean, ByVal constr As String)

            Dim cn As New SqlClient.SqlConnection(constr)

            Dim cmdGroups As New SqlClient.SqlCommand("UPDATE WebGroup SET TVShow=@TVShow where CustomerID=@CustomerID", cn)
            cmdGroups.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
            cmdGroups.Parameters.AddWithValue("@TVShow", CInt(blnShow))

            Try
                cn.Open()
                cmdGroups.ExecuteNonQuery()
            Catch ex As Exception

            Finally
                cn.Close()
            End Try
        End Sub

        Private Sub GetCustomer(ByVal cn As SqlClient.SqlConnection)

            Dim cmdGetCustomer As New SqlClient.SqlCommand("SELECT * FROM Customer WHERE CustomerID=@CustomerID", cn)
            cmdGetCustomer.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
            Dim dr As SqlClient.SqlDataReader = cmdGetCustomer.ExecuteReader
            Try
                While dr.Read
                    m_strCustomerName = dr("Customername").ToString
                    m_strDocuPath = dr("DocuPath").ToString

                    Try
                        m_PortalType = dr("PortalType").ToString
                    Catch
                        m_PortalType = ""
                    End Try
                    Try
                        m_ReferenceType1 = dr("Userreferenzfeld1").ToString()
                        m_ReferenceType2 = dr("Userreferenzfeld2").ToString()
                        m_ReferenceType3 = dr("Userreferenzfeld3").ToString()
                        m_ReferenceType4 = dr("Userreferenzfeld4").ToString()
                    Catch
                        m_ReferenceType1 = ""
                        m_ReferenceType2 = ""
                        m_ReferenceType3 = ""
                        m_ReferenceType4 = ""
                    End Try
                    Try
                        m_MvcSelectionUrl = dr("MvcSelectionUrl").ToString
                    Catch
                        m_MvcSelectionUrl = ""
                    End Try
                    Try
                        m_MvcSelectionType = dr("MvcSelectionType").ToString
                    Catch
                        m_MvcSelectionType = ""
                    End Try


                    'ITA 2156 JJU20090219
                    '---------------------------------------------------------------
                    If dr("SelfAdministration") Is DBNull.Value Then
                        m_selfAdministration = 0
                    Else
                        m_selfAdministration = CInt((dr("SelfAdministration")))
                    End If

                    If dr("SelfAdministrationInfo") Is DBNull.Value Then
                        m_selfAdministrationInfo = ""
                    Else
                        m_selfAdministrationInfo = dr("SelfAdministrationInfo")
                    End If

                    If dr("SelfAdministrationContact") Is DBNull.Value Then
                        m_selfAdministrationContact = ""
                    Else
                        m_selfAdministrationContact = dr("SelfAdministrationContact")
                    End If

                    If dr("Locked") Is DBNull.Value Then
                        m_Locked = False
                    Else
                        m_Locked = CBool((dr("Locked")))
                    End If

                    If dr("AllowUrlRemoteLogin") Is DBNull.Value Then
                        m_blnAllowUrlRemoteLogin = False
                    Else
                        m_blnAllowUrlRemoteLogin = CBool((dr("AllowUrlRemoteLogin")))
                    End If
                    '---------------------------------------------------------------


                    m_blnForcePasswordQuestion = CBool(dr("ForcePasswordQuestion"))
                    m_strHeaderBackgroundPath = dr("HeaderBackgroundPath").ToString
                    m_strLogoPath2 = dr("LogoPath2").ToString
                    m_strKUNNR = dr("KUNNR").ToString
                    m_blnReadDealer = CBool(dr("ReadDealer"))
                    m_blnMaster = CBool(dr("Master"))
                    m_blnAllowMultipleLogin = CBool(dr("AllowMultipleLogin"))
                    '
                    ' Null Reference Check a couple of rows above...
                    'm_blnAllowUrlRemoteLogin = CBool(dr("AllowUrlRemoteLogin"))

                    m_CustomerContact = New Contact(dr("CName").ToString, _
                                                    dr("CAddress").ToString, _
                                                    dr("CMailDisplay").ToString, _
                                                    dr("CMail").ToString, _
                                                    dr("CWebDisplay").ToString, _
                                                    dr("CWeb").ToString, _
                                                    dr("KundePostfach").ToString, _
                                                    dr("KundeHotline").ToString, _
                                                    dr("KundeFax").ToString)
                    m_CustomerPasswordRules = New PasswordRules(CInt(dr("PwdNNumeric")), _
                                                                CInt(dr("PwdLength")), _
                                                                CInt(dr("PwdNCapitalLetters")), _
                                                                CInt(dr("PwdNSpecialCharacter")), _
                                                                CInt(dr("PwdHistoryNEntries")), _
                                                                CBool(dr("PwdDontSendEmail")), _
                                                                CBool(dr("NameInputOptional")))

                    m_CustomerLoginRules = New LoginRules(CInt(dr("LockedAfterNLogins")), _
                                                          CInt(dr("NewPwdAfterNDays")))
                    m_CustomerStyle = New Style(dr("LogoPath").ToString, dr("CssPath").ToString)
                    m_intMaxUser = CInt(dr("MaxUser"))
                    If TypeOf dr("AccountingArea") Is System.DBNull Then
                        m_intAccountingArea = -1
                    Else
                        m_intAccountingArea = CInt(dr("AccountingArea"))
                    End If
                    m_blnShowOrganization = CBool(dr("ShowOrganization"))
                    m_blnOrgAdminRestrictToCustomerGroup = CBool(dr("OrgAdminRestrictToCustomerGroup"))
                    m_blnPwdDontSendEmail = CBool(dr("PwdDontSendEmail"))
                    m_blnIpRestriction = CBool(dr("IpRestriction"))
                    If Not TypeOf dr("IpStandardUser") Is System.DBNull Then
                        m_strIpStandardUser = CStr(dr("IpStandardUser"))
                    Else
                        m_strIpStandardUser = ""
                    End If
                    m_blnNameInputOptional = CBool(dr("NameInputOptional"))
                    m_blnShowDistrikte = CBool(dr("Distrikte"))


                    m_strHeaderBackgroundPath = dr("HeaderBackgroundPath").ToString
                    m_strLogoPath2 = dr("LogoPath2").ToString
                    m_strLogoPath = dr("LogoPath").ToString

                    If Not TypeOf dr("DaysUntilLock") Is System.DBNull Then
                        m_intDaysUntilLock = CInt(dr("DaysUntilLock"))
                    End If
                    If Not TypeOf dr("DaysUntilDelete") Is System.DBNull Then
                        m_intDaysUntilDelete = CInt(dr("DaysUntilDelete"))
                    End If
                    If Not TypeOf dr("TVShow") Is System.DBNull Then
                        m_blnTVShow = CBool(dr("TVShow"))
                    Else
                        m_blnTVShow = False
                    End If
                    If Not TypeOf dr("UserDontSendEmail") Is System.DBNull Then
                        m_UrCustomerUsernameRules = New UsernameRules(CBool(dr("UserDontSendEmail")))
                    Else
                        m_UrCustomerUsernameRules = New UsernameRules(True)
                    End If
                    If Not TypeOf dr("LoginLinkID") Is System.DBNull Then
                        m_intLoginLinkID = CInt(dr("LoginLinkID"))
                    Else
                        m_intLoginLinkID = 0
                    End If
                    If Not TypeOf dr("ForceSpecifiedLoginLink") Is System.DBNull Then
                        m_blnForceSpecifiedLoginLink = CBool(dr("ForceSpecifiedLoginLink"))
                    Else
                        m_blnForceSpecifiedLoginLink = False
                    End If
                    m_strLogoutLink = dr("LogoutLink").ToString
                End While
            Catch ex As Exception
                Throw ex
            Finally
                dr.Close()
            End Try

            GetIpAddresses(cn)
            GetSDCredentials(cn)
            GetReferenceTypeNames(cn)
        End Sub

        Private Sub GetPortalLoginLink(ByVal cn As SqlClient.SqlConnection)
            Dim cmdGetLink As New SqlClient.SqlCommand("SELECT Text FROM WebUserUploadLoginLink WHERE ID=@ID", cn)
            cmdGetLink.Parameters.AddWithValue("@ID", m_intLoginLinkID)
            m_strLoginLink = cmdGetLink.ExecuteScalar().ToString()
        End Sub

        Public Sub GetIpAddresses(ByVal cn As SqlClient.SqlConnection)
            Dim da As New SqlClient.SqlDataAdapter()
            Dim tblReturn As New DataTable()
            Try
                Dim cmdGetAddresse As New SqlClient.SqlCommand("SELECT IpAddress FROM IpAddresses WHERE CustomerID=@CustomerID ORDER BY IpAddress", cn)
                cmdGetAddresse.Parameters.AddWithValue("@CustomerID", m_intCustomerID)

                da.SelectCommand = cmdGetAddresse
                da.Fill(tblReturn)
            Catch ex As Exception
                Throw ex
            Finally
                da.Dispose()
            End Try

            m_tblIpAddresses = tblReturn
        End Sub

        Public Sub GetSDCredentials(ByVal cn As SqlClient.SqlConnection)
            Dim cmdSDCredentials As New SqlClient.SqlCommand("SELECT Kundennummer, Zugangsname, Passwort, LoginNameBankenLinie, SignaturBankenLinie, Signatur2BankenLinie FROM CustomerDAT WHERE CustomerID=@CustomerID", cn)
            cmdSDCredentials.Parameters.AddWithValue("@CustomerID", m_intCustomerID)

            Using dr As SqlClient.SqlDataReader = cmdSDCredentials.ExecuteReader()
                If dr.HasRows Then
                    dr.Read()
                    m_DcDatCredentials = New DatCredentials(dr.GetString(0), dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetString(4), dr.GetString(5))
                Else
                    m_DcDatCredentials = New DatCredentials("", "", "", "", "", "")
                End If
            End Using
        End Sub

        Public Sub GetReferenceTypeNames(ByVal cn As SqlClient.SqlConnection)
            Dim tmpTable As New DataTable()

            Dim daReferenceTypeNames As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter("SELECT * FROM ReferenzTypen", cn)
            daReferenceTypeNames.Fill(tmpTable)

            If Not String.IsNullOrEmpty(m_ReferenceType1) Then
                m_ReferenceType1Name = tmpTable.Select("ReferenzTyp = '" & ReferenceType1 & "'")(0)("ReferenzTypName").ToString()
                If String.IsNullOrEmpty(m_ReferenceType1Name) Then
                    m_ReferenceType1Name = m_ReferenceType1
                End If
            End If
            If Not String.IsNullOrEmpty(m_ReferenceType2) Then
                m_ReferenceType2Name = tmpTable.Select("ReferenzTyp = '" & ReferenceType2 & "'")(0)("ReferenzTypName").ToString()
                If String.IsNullOrEmpty(m_ReferenceType2Name) Then
                    m_ReferenceType2Name = m_ReferenceType2
                End If
            End If
            If Not String.IsNullOrEmpty(m_ReferenceType3) Then
                m_ReferenceType3Name = tmpTable.Select("ReferenzTyp = '" & ReferenceType3 & "'")(0)("ReferenzTypName").ToString()
                If String.IsNullOrEmpty(m_ReferenceType3Name) Then
                    m_ReferenceType3Name = m_ReferenceType3
                End If
            End If
            If Not String.IsNullOrEmpty(m_ReferenceType4) Then
                m_ReferenceType4Name = tmpTable.Select("ReferenzTyp = '" & ReferenceType4 & "'")(0)("ReferenzTypName").ToString()
                If String.IsNullOrEmpty(m_ReferenceType4Name) Then
                    m_ReferenceType4Name = m_ReferenceType4
                End If
            End If
        End Sub

        Public Sub Delete(ByVal strConnectionString As String)
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Delete(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen des Kunden!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub

        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strDeleteGroupApps As String = "DELETE " & _
                                                   "FROM Rights " & _
                                                   "WHERE GroupID IN (SELECT DISTINCT Rights.GroupID " & _
                                                                     "FROM Rights INNER JOIN WebGroup " & _
                                                                       "ON Rights.GroupID = WebGroup.GroupID " & _
                                                                     "WHERE (WebGroup.CustomerID = @CustomerID))"

                Dim strDeleteGroup As String = "DELETE " & _
                                               "FROM WebGroup " & _
                                               "WHERE CustomerID=@CustomerID"

                Dim strDeleteCustomerApps As String = "DELETE " & _
                                                      "FROM CustomerRights " & _
                                                      "WHERE CustomerID=@CustomerID"

                Dim strDeleteCustomerDAT As String = "DELETE " & _
                                               "FROM CustomerDAT " & _
                                               "WHERE CustomerID=@CustomerID"

                Dim strDeleteCustomerOrganization As String = "DELETE " & _
                                                "FROM Organization " & _
                                                "WHERE CustomerID=@CustomerID"

                Dim strDeleteCustomer As String = "DELETE " & _
                                                  "FROM Customer " & _
                                                  "WHERE CustomerID=@CustomerID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@CustomerID", m_intCustomerID)

                'Group-Application-Verknuepfungen loeschen
                cmd.CommandText = strDeleteGroupApps
                cmd.ExecuteNonQuery()

                'Group loeschen
                cmd.CommandText = strDeleteGroup
                cmd.ExecuteNonQuery()

                'Application-Verknuepfungen loeschen
                cmd.CommandText = strDeleteCustomerApps
                cmd.ExecuteNonQuery()

                'SilverDAT Zugang loeschen
                cmd.CommandText = strDeleteCustomerDAT
                cmd.ExecuteNonQuery()

                'Organization loeschen
                cmd.CommandText = strDeleteCustomerOrganization
                cmd.ExecuteNonQuery()

                'Customer loeschen
                cmd.CommandText = strDeleteCustomer
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen des Kunden!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal strConnectionString As String)
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Save(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern des Kunden!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub

        Public Sub Save(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strInsert As String = "INSERT INTO Customer(Customername, " & _
                                               "KUNNR, " & _
                                               "ReadDealer, " & _
                                               "CName, " & _
                                               "CAddress, " & _
                                               "CMailDisplay, " & _
                                               "CMail, " & _
                                               "CWebDisplay, " & _
                                               "CWeb, " & _
                                               "KundePostfach, " & _
                                               "KundeHotline, " & _
                                               "KundeFax, " & _
                                               "Master, " & _
                                               "NewPwdAfterNDays, " & _
                                               "LockedAfterNLogins, " & _
                                               "PwdNNumeric, " & _
                                               "PwdLength, " & _
                                               "PwdNCapitalLetters, " & _
                                               "PwdNSpecialCharacter, " & _
                                               "PwdHistoryNEntries, " & _
                                               "LogoPath, " & _
                                               "LogoPath2, " & _
                                               "HeaderBackgroundPath, " & _
                                               "DocuPath, " & _
                                               "CssPath, " & _
                                               "AllowMultipleLogin, " & _
                                               "AllowUrlRemoteLogin, " & _
                                               "MaxUser, " & _
                                               "ShowOrganization, " & _
                                               "OrgAdminRestrictToCustomerGroup, " & _
                                               "PwdDontSendEmail, " & _
                                               "NameInputOptional, " & _
                                               "ForcePasswordQuestion, " & _
                                               "Distrikte, " & _
                                               "IpRestriction, " & _
                                               "IpStandardUser, " & _
                                               "AccountingArea, " & _
                                               "SelfAdministration, " & _
                                               "SelfAdministrationInfo, " & _
                                               "SelfAdministrationContact, " & _
                                               "DaysUntilLock, " & _
                                               "DaysUntilDelete, " & _
                                               "TVShow, " & _
                                               "UserDontSendEmail, " & _
                                               "LoginLinkID, " & _
                                               "PortalType, " & _
                                               "ForceSpecifiedLoginLink, " & _
                                               "LogoutLink, " & _
                                               "Userreferenzfeld1, " & _
                                               "Userreferenzfeld2, " & _
                                               "Userreferenzfeld3, " & _
                                               "Userreferenzfeld4, " & _
                                               "MvcSelectionUrl, " & _
                                               "MvcSelectionType)" & _
                          "VALUES(@Customername, " & _
                                 "@KUNNR, " & _
                                 "@ReadDealer, " & _
                                 "@CName, " & _
                                 "@CAddress, " & _
                                 "@CMailDisplay, " & _
                                 "@CMail, " & _
                                 "@CWebDisplay, " & _
                                 "@CWeb, " & _
                                 "@KundePostfach, " & _
                                 "@KundeHotline, " & _
                                 "@KundeFax, " & _
                                 "@Master, " & _
                                 "@NewPwdAfterNDays, " & _
                                 "@LockedAfterNLogins, " & _
                                 "@PwdNNumeric, " & _
                                 "@PwdLength, " & _
                                 "@PwdNCapitalLetters, " & _
                                 "@PwdNSpecialCharacter, " & _
                                 "@PwdHistoryNEntries, " & _
                                 "@LogoPath, " & _
                                 "@LogoPath2, " & _
                                 "@HeaderBackgroundPath, " & _
                                 "@DocuPath, " & _
                                 "@CssPath, " & _
                                 "@AllowMultipleLogin, " & _
                                 "@AllowUrlRemoteLogin, " & _
                                 "@MaxUser, " & _
                                 "@ShowOrganization, " & _
                                 "@OrgAdminRestrictToCustomerGroup, " & _
                                 "@PwdDontSendEmail, " & _
                                 "@NameInputOptional," & _
                                 "@ForcePasswordQuestion," & _
                                 "@Distrikte," & _
                                 "@IpRestriction," & _
                                 "@IpStandardUser," & _
                                 "@AccountingArea," & _
                                 "@SelfAdministration," & _
                                 "@SelfAdministrationInfo," & _
                                 "@SelfAdministrationContact," & _
                                 "@DaysUntilLock, " & _
                                 "@DaysUntilDelete, " & _
                                 "@TVShow, " & _
                                 "@UserDontSendEmail, " & _
                                 "@LoginLinkID, " & _
                                 "@PortalType, " & _
                                 "@ForceSpecifiedLoginLink, " & _
                                 "@LogoutLink, " & _
                                 "@Userreferenzfeld1, " & _
                                 "@Userreferenzfeld2, " & _
                                 "@Userreferenzfeld3, " & _
                                 "@Userreferenzfeld4, " & _
                                 "@MvcSelectionUrl, " & _
                                 "@MvcSelectionType); " & _
                          "SELECT SCOPE_IDENTITY()"

                Dim strUpdate As String = "UPDATE Customer " & _
                                          "SET Customername=@Customername, " & _
                                              "KUNNR=@KUNNR," & _
                                              "ReadDealer=@ReadDealer, " & _
                                              "CName=@CName, " & _
                                              "CAddress=@CAddress, " & _
                                              "CMailDisplay=@CMailDisplay, " & _
                                              "CMail=@CMail, " & _
                                              "CWebDisplay=@CWebDisplay, " & _
                                              "CWeb=@CWeb, " & _
                                              "KundePostfach=@Kundepostfach, " & _
                                              "KundeHotline=@KundeHotline, " & _
                                              "KundeFax=@KundeFax, " & _
                                              "Master=@Master, " & _
                                              "NewPwdAfterNDays=@NewPwdAfterNDays, " & _
                                              "LockedAfterNLogins=@LockedAfterNLogins," & _
                                              "PwdNNumeric=@PwdNNumeric, " & _
                                              "PwdLength=@PwdLength, " & _
                                              "PwdNCapitalLetters=@PwdNCapitalLetters, " & _
                                              "PwdNSpecialCharacter=@PwdNSpecialCharacter, " & _
                                              "PwdHistoryNEntries=@PwdHistoryNEntries, " & _
                                              "LogoPath=@LogoPath, " & _
                                              "LogoPath2=@LogoPath2, " & _
                                              "HeaderBackgroundPath=@HeaderBackgroundPath, " & _
                                              "DocuPath=@DocuPath, " & _
                                              "CssPath=@CssPath, " & _
                                              "AllowMultipleLogin=@AllowMultipleLogin, " & _
                                              "AllowUrlRemoteLogin=@AllowUrlRemoteLogin, " & _
                                              "MaxUser=@MaxUser, " & _
                                              "ShowOrganization=@ShowOrganization, " & _
                                              "OrgAdminRestrictToCustomerGroup=@OrgAdminRestrictToCustomerGroup, " & _
                                              "PwdDontSendEmail=@PwdDontSendEmail, " & _
                                              "NameInputOptional=@NameInputOptional, " & _
                                              "ForcePasswordQuestion=@ForcePasswordQuestion, " & _
                                              "Distrikte=@Distrikte, " & _
                                              "IpRestriction=@IpRestriction, " & _
                                              "IpStandardUser=@IpStandardUser, " & _
                                              "AccountingArea=@AccountingArea, " & _
                                              "SelfAdministration=@SelfAdministration, " & _
                                              "SelfAdministrationInfo=@SelfAdministrationInfo, " & _
                                              "SelfAdministrationContact=@SelfAdministrationContact, " & _
                                              "Locked=@Locked, " & _
                                              "DaysUntilLock=@DaysUntilLock, " & _
                                              "DaysUntilDelete=@DaysUntilDelete, " & _
                                              "TVShow=@TVShow, " & _
                                              "UserDontSendEmail=@UserDontSendEmail, " & _
                                              "LoginLinkID=@LoginLinkID, " & _
                                              "PortalType=@PortalType, " & _
                                              "ForceSpecifiedLoginLink=@ForceSpecifiedLoginLink, " & _
                                              "LogoutLink=@LogoutLink, " & _
                                              "Userreferenzfeld1=@Userreferenzfeld1, " & _
                                              "Userreferenzfeld2=@Userreferenzfeld2, " & _
                                              "Userreferenzfeld3=@Userreferenzfeld3, " & _
                                              "Userreferenzfeld4=@Userreferenzfeld4, " & _
                                              "MvcSelectionUrl=@MvcSelectionUrl, " & _
                                              "MvcSelectionType=@MvcSelectionType " & _
                                          "WHERE CustomerID=@CustomerID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                'Speichern
                If m_intCustomerID = -1 Then
                    cmd.CommandText = strInsert

                    Dim cmdCheckUserExist As New SqlClient.SqlCommand("SELECT COUNT(CustomerID) FROM Customer WHERE Customername=@Customername", cn)
                    cmdCheckUserExist.Parameters.AddWithValue("@Customername", m_strCustomerName)
                    If cmdCheckUserExist.ExecuteScalar.ToString <> "0" Then
                        Throw New Exception("Es existiert bereits ein Kunde  mit dieser Firmenbezeichnung im System! Bitte wählen sie eine andere Bezeichnung!")
                    End If
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@CustomerID", m_intCustomerID)

                    'TeamViewer Flag in den Groups setzen falls nötig
                    Dim ReaderCon As New SqlClient.SqlConnection(cn.ConnectionString)
                    Dim cmdTVShow As New SqlClient.SqlCommand("SELECT TVShow FROM Customer WHERE CustomerID=@CustomerID", ReaderCon)
                    cmdTVShow.Parameters.AddWithValue("@CustomerID", m_intCustomerID)

                    ReaderCon.Open()
                    Dim SQLRead As SqlClient.SqlDataReader = cmdTVShow.ExecuteReader()
                    Dim blnTVChanged As Boolean = False

                    'Prüfen ob sich TVShow geändert hat
                    While SQLRead.Read()
                        If TypeOf SQLRead("TVShow") Is DBNull Then
                            blnTVChanged = True
                        ElseIf m_blnTVShow <> CBool(SQLRead("TVShow")) Then
                            blnTVChanged = True
                        End If
                    End While

                    ReaderCon.Close()
                    ' Falls geändert, dann Groups auch anpassen
                    If blnTVChanged Then
                        SetTVShowInGroups(m_blnTVShow, cn.ConnectionString)
                    End If
                End If
                With cmd.Parameters
                    .AddWithValue("@Customername", m_strCustomerName)
                    .AddWithValue("@KUNNR", m_strKUNNR)
                    .AddWithValue("@ReadDealer", m_blnReadDealer)
                    .AddWithValue("@CName", m_CustomerContact.Name)
                    .AddWithValue("@CAddress", m_CustomerContact.Address)
                    .AddWithValue("@CMailDisplay", m_CustomerContact.MailDisplay)
                    .AddWithValue("@CMail", m_CustomerContact.Mail)
                    .AddWithValue("@CWebDisplay", m_CustomerContact.WebDisplay)
                    .AddWithValue("@CWeb", m_CustomerContact.Web)
                    .AddWithValue("@KundePostfach", m_CustomerContact.Kundenpostfach)
                    .AddWithValue("@KundeHotline", m_CustomerContact.Kundenhotline)
                    .AddWithValue("@KundeFax", m_CustomerContact.Kundenfax)

                    .AddWithValue("@AccountingArea", m_intAccountingArea)
                    .AddWithValue("@Master", m_blnMaster)
                    .AddWithValue("@NewPwdAfterNDays", m_CustomerLoginRules.NewPasswordAfterNDays)
                    .AddWithValue("@LockedAfterNLogins", m_CustomerLoginRules.LockedAfterNLogins)
                    .AddWithValue("@PwdNNumeric", m_CustomerPasswordRules.Numeric)
                    .AddWithValue("@PwdLength", m_CustomerPasswordRules.Length)
                    .AddWithValue("@PwdNCapitalLetters", m_CustomerPasswordRules.CapitalLetters)
                    .AddWithValue("@PwdNSpecialCharacter", m_CustomerPasswordRules.SpecialCharacter)
                    .AddWithValue("@PwdHistoryNEntries", m_CustomerPasswordRules.PasswordHistoryEntries)
                    .AddWithValue("@LogoPath", m_CustomerStyle.LogoPath)
                    .AddWithValue("@LogoPath2", m_strLogoPath2)
                    .AddWithValue("@HeaderBackgroundPath", m_strHeaderBackgroundPath)
                    .AddWithValue("@DocuPath", m_strDocuPath)
                    .AddWithValue("@CssPath", m_CustomerStyle.CssPath)
                    .AddWithValue("@AllowMultipleLogin", m_blnAllowMultipleLogin)
                    .AddWithValue("@AllowUrlRemoteLogin", m_blnAllowUrlRemoteLogin)
                    .AddWithValue("@MaxUser", m_intMaxUser)
                    .AddWithValue("@ShowOrganization", m_blnShowOrganization)
                    .AddWithValue("@OrgAdminRestrictToCustomerGroup", OrgAdminRestrictToCustomerGroup)
                    .AddWithValue("@PwdDontSendEmail", m_CustomerPasswordRules.DontSendEmail)
                    .AddWithValue("@NameInputOptional", m_CustomerPasswordRules.NameInputOptional)
                    .AddWithValue("@ForcePasswordQuestion", m_blnForcePasswordQuestion)
                    .AddWithValue("@Distrikte", m_blnShowDistrikte)
                    .AddWithValue("@IpRestriction", m_blnIpRestriction)
                    .AddWithValue("@IpStandardUser", m_strIpStandardUser)
                    .AddWithValue("@SelfAdministration", m_selfAdministration)
                    .AddWithValue("@SelfAdministrationInfo", m_selfAdministrationInfo)
                    .AddWithValue("@SelfAdministrationContact", m_selfAdministrationContact)
                    .AddWithValue("@Locked", m_Locked)
                    .AddWithValue("@DaysUntilLock", m_intDaysUntilLock)
                    .AddWithValue("@DaysUntilDelete", m_intDaysUntilDelete)
                    .AddWithValue("@TVShow", m_blnTVShow)
                    .AddWithValue("@UserDontSendEmail", m_UrCustomerUsernameRules.DontSendEmail)
                    .AddWithValue("@LoginLinkID", m_intLoginLinkID)
                    .AddWithValue("@PortalType", m_PortalType)
                    .AddWithValue("@ForceSpecifiedLoginLink", m_blnForceSpecifiedLoginLink)
                    .AddWithValue("@LogoutLink", m_strLogoutLink)
                    .AddWithValue("@Userreferenzfeld1", m_ReferenceType1)
                    .AddWithValue("@Userreferenzfeld2", m_ReferenceType2)
                    .AddWithValue("@Userreferenzfeld3", m_ReferenceType3)
                    .AddWithValue("@Userreferenzfeld4", m_ReferenceType4)
                    .AddWithValue("@MvcSelectionUrl", m_MvcSelectionUrl)
                    .AddWithValue("@MvcSelectionType", m_MvcSelectionType)

                End With


                If m_intCustomerID = -1 Then
                    'Wenn Customer neu ist dann ID ermitteln, damit bei nachfolgendem Fehler und erneutem Speichern Datensatz nicht doppelt angelegt wird.
                    m_intCustomerID = CInt(cmd.ExecuteScalar)

                    'Standard-Organisation anlegen
                    cmd = New SqlClient.SqlCommand()
                    cmd.Connection = cn

                    cmd.CommandText = "INSERT INTO Organization(OrganizationName, " & _
                                                                   "CustomerID, " & _
                                                                   "OrganizationReference) " & _
                                              "VALUES(@OrganizationName, " & _
                                                     "@CustomerID, " & _
                                                     "@OrganizationReference)"
                    With cmd.Parameters
                        .AddWithValue("@OrganizationName", "Standard")
                        .AddWithValue("@CustomerID", m_intCustomerID)
                        .AddWithValue("@OrganizationReference", "999")
                    End With
                    cmd.ExecuteNonQuery()

                    'TeamViewer Flag in den Groups setzen
                    SetTVShowInGroups(m_blnTVShow, cn.ConnectionString)
                Else
                    cmd.ExecuteNonQuery()
                End If

                SaveSDCredentials(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern des Kunden!", ex)
            End Try
        End Sub

        Private Sub SaveSDCredentials(ByVal cn As SqlClient.SqlConnection)
            Dim cmdSDCredentials As New SqlClient.SqlCommand("SELECT COUNT(Kundennummer) FROM CustomerDAT WHERE CustomerID=@CustomerID", cn)
            cmdSDCredentials.Parameters.AddWithValue("@CustomerID", m_intCustomerID)

            If Convert.ToInt32(cmdSDCredentials.ExecuteScalar()) = 0 Then
                cmdSDCredentials.CommandText = "INSERT INTO CustomerDAT(CustomerID, Kundennummer, Zugangsname, Passwort, LoginNameBankenLinie, SignaturBankenLinie, Signatur2BankenLinie) " & _
                                               "VALUES(@CustomerID, @Kundennummer, @Zugangsname, @Passwort, @LoginNameBankenLinie, @SignaturBankenLinie, @Signatur2BankenLinie)"
            Else
                cmdSDCredentials.CommandText = "UPDATE CustomerDAT " & _
                                               "SET Kundennummer = @Kundennummer, " & _
                                                   "Zugangsname = @Zugangsname, " & _
                                                   "Passwort = @Passwort, " & _
                                                   "LoginNameBankenLinie = @LoginNameBankenLinie, " & _
                                                   "SignaturBankenLinie = @SignaturBankenLinie, " & _
                                                   "Signatur2BankenLinie = @Signatur2BankenLinie " & _
                                               "WHERE CustomerID = @CustomerID"
            End If

            With cmdSDCredentials.Parameters
                .AddWithValue("@Kundennummer", SilverDATCredentials.CustomerNumber)
                .AddWithValue("@Zugangsname", SilverDATCredentials.UserName)
                .AddWithValue("@Passwort", SilverDATCredentials.Password)
                .AddWithValue("@LoginNameBankenLinie", SilverDATCredentials.LoginName)
                .AddWithValue("@SignaturBankenLinie", SilverDATCredentials.Signatur)
                .AddWithValue("@Signatur2BankenLinie", SilverDATCredentials.Signatur2)
            End With
            cmdSDCredentials.ExecuteNonQuery()
        End Sub

        Public Function HasUser(ByVal strConnectionString As String) As Boolean
            Dim cn As New SqlClient.SqlConnection(strConnectionString)
            Try
                Return HasUser(cn)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try

        End Function

        Public Function HasUser(ByVal cn As SqlClient.SqlConnection) As Boolean
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim cmd As New SqlClient.SqlCommand("SELECT COUNT(UserID) FROM WebUser WHERE CustomerID=@CustomerID", cn)
            cmd.Parameters.AddWithValue("@CustomerID", m_intCustomerID)
            If CInt(cmd.ExecuteScalar) > 0 Then
                Return True
            End If
            Return False
        End Function

#End Region

#Region " SubClasses "
        <Serializable()> Class PasswordRules
#Region " Membervariables "
            Private m_intPwdNNumeric As Integer
            Private m_intPwdLength As Integer
            Private m_intPwdNCapitalLetters As Integer
            Private m_intPwdNSpecialCharacter As Integer
            Private m_intPwdHistoryNEntries As Integer
            Private m_blnPwdDontSendEmail As Boolean
            Private m_blnNameInputOptional As Boolean
            Private m_strErrorMessage As String
            Private m_blnUsernameDontSendEmail As Boolean
#End Region

#Region " Constructor "
            Friend Sub New(ByVal intNumeric As Integer, _
                           ByVal intPwdLength As Integer, _
                           ByVal intPwdNCapitalLetters As Integer, _
                           ByVal intSpecialCharacter As Integer, _
                           ByVal intPwdHistoryNEntries As Integer, _
                           ByVal blnPwdDontSendEmail As Boolean, _
                           ByVal blnNameInputOptional As Boolean)
                m_intPwdNNumeric = intNumeric
                m_intPwdLength = intPwdLength
                m_intPwdNCapitalLetters = intPwdNCapitalLetters
                m_intPwdNSpecialCharacter = intSpecialCharacter
                m_intPwdHistoryNEntries = intPwdHistoryNEntries
                m_blnPwdDontSendEmail = blnPwdDontSendEmail
                m_blnNameInputOptional = blnNameInputOptional
            End Sub
#End Region

#Region " Properties "
            Public ReadOnly Property Numeric() As Integer
                Get
                    Return m_intPwdNNumeric
                End Get
            End Property

            Public ReadOnly Property Length() As Integer
                Get
                    Return m_intPwdLength
                End Get
            End Property

            Public ReadOnly Property CapitalLetters() As Integer
                Get
                    Return m_intPwdNCapitalLetters
                End Get
            End Property

            Public ReadOnly Property SpecialCharacter() As Integer
                Get
                    Return m_intPwdNSpecialCharacter
                End Get
            End Property

            Public ReadOnly Property PasswordHistoryEntries() As Integer
                Get
                    Return m_intPwdHistoryNEntries
                End Get
            End Property

            Public ReadOnly Property DontSendEmail() As Boolean
                Get
                    Return m_blnPwdDontSendEmail
                End Get
            End Property

            Public ReadOnly Property NameInputOptional() As Boolean
                Get
                    Return m_blnNameInputOptional
                End Get
            End Property

            Public ReadOnly Property ErrorMessage() As String
                Get
                    Return m_strErrorMessage
                End Get
            End Property
#End Region

#Region " Functions "
            Public Function PasswordIsValid(ByVal strPassword As String) As Boolean
                m_strErrorMessage = ""

                If strPassword.Length < m_intPwdLength Then
                    m_strErrorMessage &= String.Format("<li>Das Passwort muss mindestens {0} Zeichen lang sein.", m_intPwdLength)
                End If

                Dim intNNumeric As Integer
                Dim intNLetters As Integer
                Dim intNCapitalLetters As Integer
                Dim intNSpecialCharacter As Integer
                Dim chrPassword() As Char = strPassword.ToCharArray
                Dim _chr As Char
                Dim _int As Integer
                For Each _chr In chrPassword
                    _int = Asc(_chr)
                    If (_int >= 97) AndAlso (_int <= 122) Then
                        'Kleinbuchstabe
                        intNLetters += 1
                    ElseIf _int >= 65 AndAlso _int <= 90 Then
                        'Grossbuchstabe
                        intNCapitalLetters += 1
                    ElseIf _int >= 48 AndAlso _int <= 57 Then
                        'Zahl
                        intNNumeric += 1
                    Else
                        'Sonderzeichen
                        intNSpecialCharacter += 1
                    End If
                Next

                If m_intPwdNNumeric > intNNumeric Then
                    m_strErrorMessage &= String.Format("<li>Das Passwort muss mindestens {0} numerischen Zeichen enthalten.", m_intPwdNNumeric)
                End If
                If m_intPwdNCapitalLetters > intNCapitalLetters Then
                    m_strErrorMessage &= String.Format("<li>Das Passwort muss mindestens {0} alphanumerischen Zeichen (gross) enthalten.", m_intPwdNCapitalLetters)
                End If
                If m_intPwdNSpecialCharacter > intNSpecialCharacter Then
                    m_strErrorMessage &= String.Format("<li>Das Passwort muss mindestens {0} Sonderzeichen enthalten.", m_intPwdNSpecialCharacter)
                End If

                If m_strErrorMessage.Length = 0 Then
                    Return True
                Else
                    Return False
                End If
            End Function

            Public Function CreateNewPasswort(ByRef errorMessage As String) As String
                Dim pw As String = String.Empty
                Dim status As String = String.Empty

                pw = Crypto.RandomPassword(Length, Numeric, CapitalLetters, SpecialCharacter, status)

                If status <> String.Empty Then
                    errorMessage = status
                    Return String.Empty
                Else
                    Return pw
                End If
            End Function
#End Region
        End Class

        <Serializable()> Class UsernameRules
#Region " Membervariables "
            Private m_strErrorMessage As String
            Private m_blnUsernameDontSendEmail As Boolean
#End Region

#Region " Constructor "
            Friend Sub New(ByVal blnUsernameDontSendEmail As Boolean)
                m_blnUsernameDontSendEmail = blnUsernameDontSendEmail
            End Sub
#End Region

#Region " Properties "
            Public ReadOnly Property DontSendEmail() As Boolean
                Get
                    Return m_blnUsernameDontSendEmail
                End Get
            End Property

            Public ReadOnly Property ErrorMessage() As String
                Get
                    Return m_strErrorMessage
                End Get
            End Property
#End Region

        End Class

        <Serializable()> Class LoginRules
#Region " Membervariables "
            Private m_intNewPwdAfterNDays As Integer
            Private m_intLockedAfterNLogins As Integer
#End Region

#Region " Constructor "
            Friend Sub New(ByVal intLockedAfterNLogins As Integer, _
                           ByVal intNewPasswordAfterNDays As Integer)
                m_intNewPwdAfterNDays = intNewPasswordAfterNDays
                m_intLockedAfterNLogins = intLockedAfterNLogins
            End Sub
#End Region

#Region " Properties "
            Public ReadOnly Property NewPasswordAfterNDays() As Integer
                Get
                    Return m_intNewPwdAfterNDays
                End Get
            End Property

            Public ReadOnly Property LockedAfterNLogins() As Integer
                Get
                    Return m_intLockedAfterNLogins
                End Get
            End Property
#End Region
        End Class

        <Serializable()> Class Style
#Region " Membervariables "
            Private m_strLogoPath As String
            Private m_strCssPath As String
#End Region

#Region " Constructor "
            Friend Sub New(ByVal strLogoPath As String, _
                           ByVal strCssPath As String)
                m_strLogoPath = strLogoPath
                m_strCssPath = strCssPath
            End Sub
#End Region

#Region " Properties "
            Public ReadOnly Property LogoPath() As String
                Get
                    Return m_strLogoPath
                End Get
            End Property

            Public ReadOnly Property CssPath() As String
                Get
                    Return m_strCssPath
                End Get
            End Property
#End Region
        End Class

        <Serializable()> Class DatCredentials
#Region " Membervariables "
            Private m_strCustomerNumber As String
            Private m_strUserName As String
            Private m_strPassword As String
            Private m_strLoginName As String
            Private m_strSignatur As String
            Private m_strSignatur2 As String
#End Region

#Region " Constructor "
            Friend Sub New(ByVal strCustomerNumber As String, _
                           ByVal strUserName As String, _
                           ByRef strPassword As String, ByRef strLoginName As String, ByRef strSignatur As String, ByRef strSignatur2 As String)
                m_strCustomerNumber = strCustomerNumber
                m_strUserName = strUserName
                m_strPassword = strPassword
                m_strLoginName = strLoginName
                m_strSignatur = strSignatur
                m_strSignatur2 = strSignatur2
            End Sub
#End Region

#Region " Properties "
            Public ReadOnly Property CustomerNumber() As String
                Get
                    Return m_strCustomerNumber
                End Get
            End Property

            Public ReadOnly Property UserName() As String
                Get
                    Return m_strUserName
                End Get
            End Property

            Public ReadOnly Property Password() As String
                Get
                    Return m_strPassword
                End Get
            End Property
            Public ReadOnly Property LoginName() As String
                Get
                    Return m_strLoginName
                End Get
            End Property
            Public ReadOnly Property Signatur() As String
                Get
                    Return m_strSignatur
                End Get
            End Property
            Public ReadOnly Property Signatur2() As String
                Get
                    Return m_strSignatur2
                End Get
            End Property
#End Region
        End Class
#End Region

    End Class
End Namespace

' ************************************************
' $History: Customer.vb $
' 
' *****************  Version 8  *****************
' User: Dittbernerc  Date: 9.05.11    Time: 13:39
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 8  *****************
' User: Dittbernerc  Date: 3.05.11    Time: 10:55
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 26.10.09   Time: 11:44
' Updated in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 19.03.09   Time: 16:25
' Updated in $/CKAG/Base/Kernel/Security
' ITA 2156 testfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 19.03.09   Time: 11:27
' Updated in $/CKAG/Base/Kernel/Security
' ITA 2156 fertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 20.02.09   Time: 14:37
' Updated in $/CKAG/Base/Kernel/Security
' ITA 2588
' 
' *****************  Version 3  *****************
' User: Hartmannu    Date: 9.09.08    Time: 13:42
' Updated in $/CKAG/Base/Kernel/Security
' ITA 2152 und 2158
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/Security
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 13  *****************
' User: Uha          Date: 21.01.08   Time: 18:09
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA 1644: Ermöglicht Login nur mit IP und festgelegtem Benutzer
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA:1440
' 
' *****************  Version 11  *****************
' User: Uha          Date: 30.08.07   Time: 12:36
' Updated in $/CKG/Base/Base/Kernel/Security
' ITA 1280: Paßwortversand im Web auf Benutzerwunsch
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 13.06.07   Time: 14:28
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' *****************  Version 9  *****************
' User: Uha          Date: 23.05.07   Time: 12:45
' Updated in $/CKG/Base/Base/Kernel/Security
' TESTSAPUsername und SAPUsername aus Tabelle Customer entfernt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 15.05.07   Time: 11:04
' Updated in $/CKG/Base/Base/Kernel/Security
' Änderungen aus StartApplication vom 11.05.2007 (Aspose-Tool,
' DataTableHelper.vb, Customer.vb)
' 
' *****************  Version 7  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' ************************************************