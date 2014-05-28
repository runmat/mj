Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports Microsoft.Data.SAPClient
Imports CKG.Base.Common

Public Class ZBII_Ersteingang
    Inherits DatenimportBase

#Region "Declarations"
    Private m_DatumVon As Date
    Private m_DatumBis As Date
    Private m_Fahrzeuge As DataTable
    Private mE_SUBRC As String = ""
    Private mE_MESSAGE As String = ""
#End Region

#Region "Properties"
    Public Property Fahrzeuge() As DataTable
        Get
            Return Me.m_Fahrzeuge
        End Get
        Set(ByVal value As DataTable)
            Me.m_Fahrzeuge = value
        End Set
    End Property

    Public Property DatumVon() As Date
        Get
            Return Me.m_DatumVon
        End Get
        Set(ByVal value As System.DateTime)
            Me.m_DatumVon = value
        End Set
    End Property
    Public Property DatumBis() As Date
        Get
            Return Me.m_DatumBis
        End Get
        Set(ByVal value As System.DateTime)
            Me.m_DatumBis = value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "ZBII_Ersteingang.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_ZBII_EINGANG_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                myProxy.setImportParameter("I_VON_ERDAT", CStr(m_DatumVon))
                myProxy.setImportParameter("I_BIS_ERDAT", CStr(m_DatumBis))


                myProxy.callBapi()
                m_Fahrzeuge = New DataTable
                m_Fahrzeuge = myProxy.getExportTable("GT_OUT")

                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

                m_Fahrzeuge.Columns.Add("Absender", GetType(System.String))
                For Each dRow As DataRow In m_Fahrzeuge.Rows
                    dRow("Absender") = dRow("NAME1_ZP").ToString & " " & dRow("ORT01_ZP").ToString
                Next
                CreateOutPut(m_Fahrzeuge, strAppID)

                If mE_SUBRC <> "0" Then
                    m_intStatus = CInt(mE_SUBRC)
                    m_strMessage = "Keine Daten gefunden!"
                End If

            Catch ex As Exception
                m_tblResult = Nothing
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region
End Class
