Option Explicit On
Option Strict On

Imports System
Imports System.Configuration

Namespace Business
    Public MustInherit Class StartMethodBase
        REM § Basisklasse fuer StartMethod, Run-Methoden jeweils individuell.

#Region " Declarations"
        Protected m_objUser As Base.Kernel.Security.User
        Protected m_objApp As Base.Kernel.Security.App
        Protected m_objLogApp As Base.Kernel.Logging.Trace
        Protected m_intStatus As Int32
        Protected m_strMessage As String
        Protected m_blnGestartet As Boolean
        Protected m_frmWebForm As System.Web.UI.Page
        Protected m_intAppID As Integer
        Protected m_strSessionID As String
        Protected m_strAppUrl As String
        Protected m_strFileName As String
        Private Shared m_strAppKey As String = ConfigurationManager.AppSettings("ApplicationKey")
#End Region

#Region " Properties"
        Public ReadOnly Property Status() As Int32
            Get
                Return m_intStatus
            End Get
        End Property

        Public ReadOnly Property Message() As String
            Get
                Return m_strMessage
            End Get
        End Property

        Friend ReadOnly Property Gestartet() As Boolean
            Get
                Return m_blnGestartet
            End Get
        End Property

        Friend ReadOnly Property NotRunYet() As Boolean
            Get
                If m_frmWebForm.Session(Me.GetType.Name) Is Nothing Then
                    Return True
                End If
                Return False
            End Get
        End Property
#End Region

#Region " Methods"
        Public Sub New(ByRef frmWebForm As Web.UI.Page, ByRef objUser As Base.Kernel.Security.User, ByVal strFileName As String)
            REM § Constructor. Übernimmt WebForm, User und Applikationsobjekt.
            m_objUser = objUser
            m_strSessionID = objUser.SessionID
            m_objApp = objUser.App
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            m_strFileName = strFileName
         
            m_intStatus = 0
            m_strMessage = ""

            m_frmWebForm = frmWebForm
        End Sub

        Public MustOverride Sub Run()

        'Friend Sub WriteScript(ByVal strText As String)
        '    Dim strScript As String
        '    strScript = "<script type=""text/javascript"" language=""JavaScript"">" & vbNewLine
        '    strScript &= "alert('{0}');" & vbNewLine
        '    strScript &= "</script>" & vbNewLine
        '    m_frmWebForm.RegisterStartupScript("SM_Mahnungen", String.Format(strScript, strText))
        '    m_frmWebForm.Session.Add(Me.GetType.Name, Now)
        'End Sub

        Friend Sub SetRunFlag()
            m_frmWebForm.Session.Add(Me.GetType.Name, Now)
        End Sub

        Friend Sub RetrieveAppInfo(ByVal strAppName As String)
            Dim cn As New SqlClient.SqlConnection(m_objApp.Connectionstring)
            cn.Open()
            Dim cmd As New SqlClient.SqlCommand("SELECT * FROM Application WHERE AppName=@AppName", cn)
            cmd.Parameters.AddWithValue("@AppName", strAppName)
            Dim dr As SqlClient.SqlDataReader
            dr = cmd.ExecuteReader
            While dr.Read
                m_strAppUrl = CStr(dr("AppURL"))
                m_intAppID = CInt(dr("AppID"))
            End While
            dr.Close()
            cn.Close()
        End Sub

        Friend Sub Redirect()
            If m_strAppUrl <> String.Empty AndAlso m_intAppID > 0 Then
                Try
                    m_frmWebForm.Session.Add("StartMethodeRedirect", Now)
                    m_frmWebForm.Response.Redirect(String.Format("{0}?AppID={1}", m_strAppUrl, m_intAppID.ToString), False)
                Catch ex As Exception
                End Try
            End If
        End Sub

        Friend Function MakeDateSAP(ByVal datInput As Date) As String
            REM $ Formt Date-Input in String YYYYMMDD um
            Return Year(datInput) & Right("0" & Month(datInput), 2) & Right("0" & Day(datInput), 2)
        End Function

        Friend Function MakeDateStandard(ByVal strInput As String) As Date
            REM § Formt String-Input im SAP-Format in Standard-Date um. Gibt "01.01.1900" zurück, wenn Umwandlung nicht möglich ist.
            Dim strTemp As String = Right(strInput, 2) & "." & Mid(strInput, 5, 2) & "." & Left(strInput, 4)
            If IsDate(strTemp) Then
                Return CDate(strTemp)
            Else
                Return CDate("01.01.1900")
            End If
        End Function

        Public Sub WriteLogEntry(ByVal blnSuccess As Boolean, ByVal strComment As String, ByRef tblConsidered As DataTable)
            Try
                Dim p_strType As String = "ERR"
                Dim p_strComment As String = strComment
                If blnSuccess Then
                    p_strType = "APP"
                    If m_strFileName.Trim(" "c).Length > 0 And tblConsidered.Rows.Count > 0 Then
                        p_strComment = strComment & " (<a href=""/" & m_strAppKey & "/Temp/Excel/" & m_strFileName & """>Excel</a>)"
                    End If
                End If
                m_objLogApp.WriteEntry(p_strType, m_objUser.UserName, m_strSessionID, m_intAppID, m_objUser.Applications.Select("AppID = '" & m_intAppID.ToString & "'")(0)("AppFriendlyName").ToString, "Report", p_strComment, m_objUser.CustomerName, m_objUser.Customer.CustomerId, m_objUser.IsTestUser, 0)
            Catch ex As Exception
                m_objApp.WriteErrorText(1, m_objUser.UserName, "DADReports", "WriteLogEntry", ex.ToString)
            End Try
        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: StartMethodBase.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 27.07.09   Time: 9:25
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Business
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 5  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Base/Base/Business
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Business
' 
' ************************************************