Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Namespace Kernel.Common


    <Serializable()> Public Class GetMailTexte

#Region "Declarations"

        Private m_objApp As Base.Kernel.Security.App
        Private m_objUser As Base.Kernel.Security.User

        Private mtblMailings As DataTable

        Private m_strMailBody As String = ""
        Private m_strMailAdress As String = ""
        Private m_strMailAdressCC As String = ""
        Private m_strBetreff As String = ""

        Protected m_intStatus As Int32
        Protected m_strMessage As String

        <NonSerialized()> Private m_strSessionID As String
        <NonSerialized()> Private m_strAppID As String
#End Region

#Region "Public Properties"


        Public Property Mailings() As DataTable
            Get
                Return mtblMailings
            End Get
            Set(ByVal Value As DataTable)
                mtblMailings = Value
            End Set
        End Property

        Public Property MailBody() As String
            Get
                Return m_strMailBody
            End Get
            Set(ByVal Value As String)
                m_strMailBody = Value
            End Set
        End Property

        Public Property MailAdress() As String
            Get
                Return m_strMailAdress
            End Get
            Set(ByVal Value As String)
                m_strMailAdress = Value
            End Set
        End Property

        Public Property MailAdressCC() As String
            Get
                Return m_strMailAdressCC
            End Get
            Set(ByVal Value As String)
                m_strMailAdressCC = Value
            End Set
        End Property


        Public Property Betreff() As String
            Get
                Return m_strBetreff
            End Get
            Set(ByVal Value As String)
                m_strBetreff = Value
            End Set
        End Property


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
#End Region

        Public Sub New(ByRef objApp As Base.Kernel.Security.App, ByRef objUser As Base.Kernel.Security.User, ByVal strSessionID As String, ByVal strAppID As String)
            m_objApp = objApp
            m_objUser = objUser
            m_strSessionID = strSessionID
            m_strAppID = strAppID

        End Sub


        Public Sub LeseMailTexte(ByVal InputVorgang As String)

            Dim strTempVorgang As String = InputVorgang

            m_intStatus = 0
            m_strMessage = ""
            mtblMailings = New DataTable

            Try
                Dim cn As New SqlClient.SqlConnection(m_objUser.App.Connectionstring)
                cn.Open()

                Dim da As New SqlClient.SqlDataAdapter("SELECT * FROM vwGetMailTexte WHERE KundenID=@KundenID " & _
                                                                                    "AND Vorgangsnummer Like @Vorgangsnummer " & _
                                                                                    "AND Aktiv=1", cn)

                da.SelectCommand.Parameters.AddWithValue("@KundenID", m_objUser.Customer.CustomerId)
                da.SelectCommand.Parameters.AddWithValue("@Vorgangsnummer", strTempVorgang)

                da.Fill(mtblMailings)

                If mtblMailings.Rows.Count > 0 Then
                    Dim dRow As DataRow

                    For Each dRow In mtblMailings.Rows
                        m_strMailBody = dRow("Text").ToString
                        m_strBetreff = dRow("Betreff").ToString
                        If CBool(dRow("CC")) Then
                            If m_strMailAdressCC.Length = 0 Then
                                m_strMailAdressCC = dRow("EmailAdresse").ToString
                            Else
                                m_strMailAdressCC &= ";" & dRow("EmailAdresse").ToString
                            End If
                        ElseIf m_strMailAdress.Length = 0 Then
                            m_strMailAdress = dRow("EmailAdresse").ToString
                        Else
                            m_strMailAdress &= ";" & dRow("EmailAdresse").ToString
                        End If
                    Next
                Else
                    m_intStatus = -9999
                End If

                cn.Close()
            Catch ex As Exception
                m_strMessage = "Keine Mailvorlagen für diesen Kunden <br>(" & ex.Message & ")."
                m_intStatus = -9999
            End Try
        End Sub

        Public Sub LeseMailAdressCC(ByVal InputVorgang As String)

            Dim strTempVorgang As String = InputVorgang

            Dim intReturn As Int32

            Try
                Dim cn As New SqlClient.SqlConnection(m_objUser.App.Connectionstring)
                cn.Open()

                Dim da As New SqlClient.SqlDataAdapter("SELECT * FROM vwGetMailTexte WHERE KundenID=@KundenID " & _
                                                                                    "AND Vorgangsnummer=@Vorgangsnummer " & _
                                                                                    "AND CC=0", cn)
                da.SelectCommand.Parameters.AddWithValue("@KundenID", m_objUser.Customer.CustomerId)
                da.SelectCommand.Parameters.AddWithValue("@Vorgangsnummer", strTempVorgang)
                da.Fill(mtblMailings)

                If mtblMailings.Rows.Count > 0 Then
                    Dim dRow As DataRow

                    For Each dRow In mtblMailings.Rows

                    Next

                End If

                cn.Close()
                intReturn = mtblMailings.Rows.Count
            Catch ex As Exception
                m_strMessage = "Keine Filialen für diesen Kunden <br>(" & ex.Message & ")."
                intReturn = 0
            End Try
        End Sub

    End Class


End Namespace