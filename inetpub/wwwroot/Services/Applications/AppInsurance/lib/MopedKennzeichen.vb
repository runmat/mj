Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports Microsoft.Data.SAPClient
Imports CKG.Base.Common

Public Class MopedKennzeichen
    Inherits BankBase

    Private m_AG As String
    Private m_VersJahr As String
    Private m_SerialNumber As String
    Private m_Vermittler As String
    Private m_IsRueckLaufer As String = ""
    Private m_IsAgentur As String = ""
    Private m_returnData As DataTable
    Private i_ImportRow As String()

    Public ReadOnly Property ReturnTable() As DataTable
        Get
            Return m_returnData
        End Get
    End Property

    Public Property ImportRow() As String()
        Get
            Return i_ImportRow
        End Get
        Set(ByVal Value As String())
            i_ImportRow = Value
        End Set
    End Property

    Public Property VersJahr() As String
        Get
            Return m_VersJahr
        End Get
        Set(ByVal Value As String)
            m_VersJahr = Value
        End Set
    End Property

    Public Property SerialNumber() As String
        Get
            Return m_SerialNumber
        End Get
        Set(ByVal Value As String)
            m_SerialNumber = Value
        End Set
    End Property

    Public Property AG() As String
        Get
            Return m_AG
        End Get
        Set(ByVal Value As String)
            m_AG = Value
        End Set
    End Property

    Public Property Vermittler() As String
        Get
            Return m_Vermittler
        End Get
        Set(ByVal Value As String)
            m_Vermittler = Value
        End Set
    End Property

    Public Property IsRueckLaeufer() As String
        Get
            Return m_IsRueckLaufer
        End Get
        Set(ByVal Value As String)
            m_IsRueckLaufer = Value
        End Set
    End Property

    Public Property IsAgentur() As String
        Get
            Return m_IsAgentur
        End Get
        Set(ByVal Value As String)
            m_IsAgentur = Value
        End Set
    End Property

    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

    End Sub

    Public Overloads Overrides Sub show()
        'nur wegen bankbase
    End Sub

    Public Overloads Overrides Sub Change()
        'nur wegen bankbase
    End Sub


    Public Sub GetData(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strClassAndMethod = "MopedKennzeichen.GetData"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = m_objUser.Customer.KUNNR.PadLeft(10, "0"c)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_MOFA_DETAILS_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_AG", strKUNNR)
                myProxy.setImportParameter("I_VJAHR", m_VersJahr.ToString)
                myProxy.setImportParameter("I_SERNR", m_SerialNumber.ToString)
                myProxy.setImportParameter("I_VERM", m_Vermittler.ToString)
                myProxy.setImportParameter("I_RUECKL", m_IsRueckLaufer.ToString)
                myProxy.setImportParameter("I_AGENTUR", m_isAgentur.ToString)

                myProxy.callBapi()
                Me.m_tblResult = myProxy.getExportTable("GT_WEB")

                Dim tmpStr As String


                For Each row As DataRow In m_tblResult.Rows

                    tmpStr = row("EIKTO_VM").ToString
                    tmpStr = Left(tmpStr, 4) & "-" & Mid(tmpStr, 5, 4) & "-" & Right(tmpStr, 1)
                    row("EIKTO_VM") = tmpStr

                Next



                m_returnData = Me.m_tblResult


            Catch ex As Exception
                m_strMessage = "Keine Daten gefunden."

            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub



    Public Sub SetData(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page, ByVal inRows As String())

        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_SAVE_MOFA_DETAILS_01", m_objApp, m_objUser, page)
            Dim tabImport As DataTable = myProxy.getImportTable("GT_WEB")
            Dim tmp As String()
            'tabImport.Rows.Add(i_ImportRow)

            For Each inputRow As String In inRows
                tmp = inputRow.Split(New Char() {"|"c})
                tabImport.Rows.Add(tmp)

            Next


            myProxy.callBapi()

            tabImport = myProxy.getExportTable("GT_WEB")
            Dim err As String

            For Each row As DataRow In tabImport.Rows

                err = row.Item("ERROR").ToString()
            Next

        Catch ex As Exception
            Throw ex

        End Try

        Me.i_ImportRow = Nothing

    End Sub

End Class
