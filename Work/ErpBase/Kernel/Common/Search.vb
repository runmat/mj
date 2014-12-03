Option Explicit On
Option Infer On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Namespace Kernel.Common
    <Serializable()> Public Class Search
        REM § Lese-/Schreibfunktion, Kunde: FFD, 
        REM § LeseHaendlerSAP_Einzeln, LeseHaendlerSAP, LeseFilialenSAP - BAPI: Z_M_Adressdaten.

#Region " Definitions"
        Private m_strHaendlerReferenzNummer As String
        Private m_strHaendlerName As String
        Private m_strHaendlerOrt As String
        Private m_strHaendlerFiliale As String
        Private m_tblHaendler As DataTable
        Private m_tblFilialen As DataTable
        Private m_districtTable As DataTable
        Private m_strErrorMessage As String

        Protected m_intStatus As Int32
        Protected m_strMessage As String

        Private m_objApp As Base.Kernel.Security.App
        Private m_objUser As Base.Kernel.Security.User

        Private m_strCUSTOMER As String
        Private m_strNAME As String
        Private m_strNAME_2 As String
        Private m_strCOUNTRYISO As String
        Private m_strPOSTL_CODE As String
        Private m_strCITY As String
        Private m_strSTREET As String

        Private m_strREFERENZ As String

        Private m_blnGestartet As Boolean
        Private m_tblSearchResult As DataTable

        Dim tblRightsResult As DataTable
        Dim tblDistrictsResult As DataTable

        <NonSerialized()> Private m_strSessionID As String
        <NonSerialized()> Private m_strAppID As String
        <NonSerialized()> Private m_vwHaendler As DataView
        <NonSerialized()> Protected m_objLogApp As Logging.Trace
        <NonSerialized()> Protected m_intIDSAP As Int32
        <NonSerialized()> Protected m_intStandardLogID As Int32
        <NonSerialized()> Protected m_strFileName As String
        <NonSerialized()> Protected m_strClassAndMethod As String

#End Region

#Region " Public Properties"
        Public Property SessionID() As String
            Get
                Return m_strSessionID
            End Get
            Set(ByVal Value As String)
                m_strSessionID = Value
            End Set
        End Property

        Public ReadOnly Property IDSAP() As Int32
            Get
                Return m_intIDSAP
            End Get
        End Property

        Public ReadOnly Property SearchResult() As DataTable
            Get
                Return m_tblSearchResult
            End Get
        End Property

        Public ReadOnly Property Gestartet() As Boolean
            Get
                Return m_blnGestartet
            End Get
        End Property

        Public Property HaendlerReferenzNummer() As String
            Get
                Return m_strHaendlerReferenzNummer
            End Get
            Set(ByVal Value As String)
                m_strHaendlerReferenzNummer = Value
            End Set
        End Property

        Public Property HaendlerName() As String
            Get
                Return m_strHaendlerName
            End Get
            Set(ByVal Value As String)
                m_strHaendlerName = Value
            End Set
        End Property

        Public Property HaendlerOrt() As String
            Get
                Return m_strHaendlerOrt
            End Get
            Set(ByVal Value As String)
                m_strHaendlerOrt = Value
            End Set
        End Property

        Public Property HaendlerFiliale() As String
            Get
                Return m_strHaendlerFiliale
            End Get
            Set(ByVal Value As String)
                m_strHaendlerFiliale = Value
            End Set
        End Property

        Public ReadOnly Property Haendler() As DataView
            Get
                Return m_vwHaendler
            End Get
        End Property

        Public ReadOnly Property Filialen() As DataView
            Get
                Return m_tblFilialen.DefaultView
            End Get
        End Property

        Public ReadOnly Property District() As DataView
            Get
                Return m_districtTable.DefaultView
            End Get
        End Property

        Public ReadOnly Property ErrorMessage() As String
            Get
                Return m_strErrorMessage
            End Get
        End Property

        Public ReadOnly Property CUSTOMER() As String
            Get
                Return m_strCUSTOMER.TrimStart("0"c)
            End Get
        End Property

        Public ReadOnly Property NAME() As String
            Get
                Return m_strNAME
            End Get
        End Property

        Public ReadOnly Property NAME_2() As String
            Get
                Return m_strNAME_2
            End Get
        End Property

        Public ReadOnly Property COUNTRYISO() As String
            Get
                Return m_strCOUNTRYISO
            End Get
        End Property

        Public ReadOnly Property POSTL_CODE() As String
            Get
                Return m_strPOSTL_CODE
            End Get
        End Property

        Public ReadOnly Property CITY() As String
            Get
                Return m_strCITY
            End Get
        End Property

        Public ReadOnly Property STREET() As String
            Get
                Return m_strSTREET
            End Get
        End Property

        Public ReadOnly Property REFERENZ() As String
            Get
                Return m_strREFERENZ
            End Get
        End Property
        Public Property Rights() As DataTable
            Get
                Return tblRightsResult
            End Get
            Set(ByVal Value As DataTable)
                tblRightsResult = Value
            End Set
        End Property

        Public ReadOnly Property Districts() As DataTable
            Get
                Return tblDistrictsResult
            End Get
        End Property
#End Region

#Region " Public Methods"
        Public Sub New(ByRef objApp As Security.App, ByRef objUser As Security.User, ByVal strSessionID As String, ByVal strAppID As String)
            m_objApp = objApp
            m_objUser = objUser

            m_strHaendlerReferenzNummer = ""
            m_strHaendlerName = ""
            m_strHaendlerOrt = ""
            m_strHaendlerFiliale = ""

            m_strSessionID = strSessionID
            m_strAppID = strAppID

            ResetFilialenTabelle()
        End Sub

        Public Sub WriteLogEntry(ByVal blnSuccess As Boolean, ByVal strComment As String)
            Try
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                Dim p_strType As String = "ERR"
                Dim p_strComment As String = strComment
                If blnSuccess Then
                    p_strType = "DBG"
                End If
                m_objLogApp.WriteEntry(p_strType, m_objUser.UserName, m_strSessionID, CInt(m_strAppID), m_objUser.Applications.Select("AppID = '" & m_strAppID & "'")(0)("AppFriendlyName").ToString, "Report", p_strComment, m_objUser.CustomerName, m_objUser.Customer.CustomerId, m_objUser.IsTestUser, 0)
            Catch ex As Exception
                m_objApp.WriteErrorText(1, m_objUser.UserName, "DADReports", "WriteLogEntry", ex.ToString)
            End Try
        End Sub

        Private Sub ResetFilialenTabelle()
            m_tblFilialen = New DataTable()

            m_tblFilialen.Columns.Add("CUSTOMER", GetType(String))
            m_tblFilialen.Columns.Add("FILIALE", GetType(String))
            m_tblFilialen.Columns.Add("NAME", GetType(String))
            m_tblFilialen.Columns.Add("NAME_2", GetType(String))
            m_tblFilialen.Columns.Add("CITY", GetType(String))
            m_tblFilialen.Columns.Add("POSTL_CODE", GetType(String))
            m_tblFilialen.Columns.Add("STREET", GetType(String))
            m_tblFilialen.Columns.Add("COUNTRYISO", GetType(String))
            m_tblFilialen.Columns.Add("DISPLAY_FILIALE", GetType(String))
        End Sub

        Public Function Show(ByVal sUserID As String, ByVal sKunnr As String) As Int32
            m_strClassAndMethod = "SAPProxy_Base.GetRights"
            If Not m_blnGestartet Then
                m_blnGestartet = True

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                Try
                    m_intStatus = 0
                    m_strMessage = ""

                    m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_Berechtigung_Anzeigen", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                    Dim proxy = DynSapProxy.getProxy("Z_BERECHTIGUNG_ANZEIGEN", m_objApp, m_objUser, PageHelper.GetCurrentPage())
                    proxy.setImportParameter("BENGRP", sUserID)
                    proxy.setImportParameter("KUNNR", sKunnr)

                    proxy.callBapi()
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                    End If

                    tblRightsResult = proxy.getExportTable("OUTPUT")
                    tblDistrictsResult = proxy.getExportTable("DISTRIKTE")

                    WriteLogEntry(True, "Berechtigungen eingelesen")
                Catch ex As Exception
                    Select Case ex.Message
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = ex.Message
                    End Select
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    End If
                    WriteLogEntry(False, "Fehler beim Einlesen der Berechtigungen")
                Finally
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                    End If

                    m_blnGestartet = False
                End Try
            End If
        End Function

        Public Function Change() As Int32

            Dim proxy = DynSapProxy.getProxy("Z_BERECHTIGUNG_ANLEGEN", m_objApp, m_objUser, PageHelper.GetCurrentPage())
            Dim sapTable = proxy.getImportTable("INPUT")


            Dim count As Integer = tblRightsResult.Rows.Count

            For Each row As DataRow In tblRightsResult.Rows

                Dim sapTableRow = sapTable.NewRow()
                sapTableRow("Anwendung") = CType(row.Item("Anwendung"), String)
                sapTableRow("Bengrp") = row.Item("Bengrp").ToString
                sapTableRow("Distrikt") = row.Item("Distrikt").ToString
                sapTableRow("Kunnr") = row.Item("Kunnr").ToString
                sapTableRow("Mandt") = row.Item("Mandt").ToString
                sapTableRow("Vorbelegt") = row.Item("Vorbelegt").ToString
                sapTableRow("Loekz") = row.Item("Loekz").ToString

                sapTable.Rows.Add(sapTableRow)
            Next

            count = sapTable.Rows.Count

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                proxy.callBapi()

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
                'm_blnGestartet = False
            End Try
        End Function
#End Region

        Public Sub New()

        End Sub
    End Class
End Namespace
