Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports Microsoft.Data.SAPClient
Imports CKG.Base.Common

Public Class ZulFahrzeuge
    Inherits DatenimportBase

#Region " Declarations"
    Private m_DatumVon As Date
    Private m_DatumBis As Date
    Private m_Versandart As String
    Private m_Fahrzeuge As DataTable
    Private m_ShowLizFzg As Boolean
#End Region

#Region " Properties"
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
    Public Property ShowLizFahrzeuge() As Boolean
        Get
            Return Me.m_ShowLizFzg
        End Get
        Set(ByVal value As Boolean)
            Me.m_ShowLizFzg = value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "ZulFahrzeuge.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ZUGELASSENE_FAHRZEUGE", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR", strKUNNR)
                myProxy.setImportParameter("I_VKORG", "1510")
                myProxy.setImportParameter("I_VDATU_VON", CStr(m_DatumVon))
                myProxy.setImportParameter("I_VDATU_BIS", CStr(m_DatumBis))

                If m_ShowLizFzg Then
                    myProxy.setImportParameter("I_LIZFZG", "X")
                End If

                myProxy.callBapi()
                m_Fahrzeuge = New DataTable
                m_Fahrzeuge = myProxy.getExportTable("GT_WEB")
                m_Fahrzeuge.Columns.Add("ORT", GetType(System.String))
                m_Fahrzeuge.Columns.Add("HERSTMOD", GetType(System.String))

                For Each row As DataRow In m_Fahrzeuge.Rows
                    row("ORT") = row("KUNPDI").ToString & " " & row("DADPDI_NAME1").ToString
                    row("HERSTMOD") = row("HERST_K").ToString & " " & row("ZZBEZEI").ToString
                Next
                m_Fahrzeuge.AcceptChanges()
                CreateOutPut(m_Fahrzeuge, strAppID)

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Versendungen im gewählten Zeitraum gefunden."
                    Case "DAT_ERROR"
                        m_strMessage = "Selektions-Zeitraum fehlerhaft."
                    Case "HAENDLER_NOT_FOUND"
                        m_intStatus = -2502
                        m_strMessage = "Händler nicht vorhanden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region
End Class
