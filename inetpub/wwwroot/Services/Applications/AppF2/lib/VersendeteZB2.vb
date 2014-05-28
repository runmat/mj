
Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Common

Public Class VersendeteZB2
    Inherits DatenimportBase
#Region " Declarations"
    Private m_DatumVon As Date
    Private m_DatumBis As Date
    Private m_Haendlernr As String
    Private m_Haendlername As String
    Private m_Versandart As String
    Private m_Abrufgrund As String
    Private m_Versendungen As DataTable
    Private m_Wiedereingang As Boolean
#End Region

#Region " Properties"
    Public Property Versendungen() As DataTable
        Get
            Return Me.m_Versendungen
        End Get
        Set(ByVal value As DataTable)
            Me.m_Versendungen = value
        End Set
    End Property
    Public Property Haendlernr() As String
        Get
            Return Me.m_Haendlernr
        End Get
        Set(ByVal value As System.String)
            Me.m_Haendlernr = value
        End Set
    End Property
    Public Property Versandart() As String
        Get
            Return Me.m_Versandart
        End Get
        Set(ByVal value As System.String)
            Me.m_Versandart = value
        End Set
    End Property
    Public Property Haendlername() As String
        Get
            Return Me.m_Haendlername
        End Get
        Set(ByVal value As System.String)
            Me.m_Haendlername = value
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
    Public Property Abrufgrund() As String
        Get
            Return Me.m_Abrufgrund
        End Get
        Set(ByVal value As String)
            Me.m_Abrufgrund = value
        End Set
    End Property

    Public Property Wiedereingang() As Boolean
        Get
            Return Me.m_Wiedereingang
        End Get
        Set(ByVal value As Boolean)
            Me.m_Wiedereingang = value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "VersendeteZB2.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)


            Try


                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_VERSENDETE_DOKUMENTE_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("AG", strKUNNR)

                'If Versandart <> "1" Then
                myProxy.setImportParameter("VON", m_DatumVon.ToShortDateString)
                myProxy.setImportParameter("BIS", m_DatumBis.ToShortDateString)
                'End If

                myProxy.setImportParameter("HAENDLER", m_Haendlernr)
                myProxy.setImportParameter("ABCKZ", m_Versandart)
                myProxy.setImportParameter("NAME1", m_Haendlername)
                myProxy.setImportParameter("ZZVGRUND", m_Abrufgrund)


                myProxy.callBapi()
                m_Versendungen = New DataTable
                m_Versendungen = myProxy.getExportTable("GT_WEB")


                If Wiedereingang = True AndAlso m_Versendungen.Rows.Count > 0 Then
                    m_Versendungen.DefaultView.RowFilter = "ZZWEDAT IS NOT NULL"
                    m_Versendungen = m_Versendungen.DefaultView.ToTable
                End If





                m_Versendungen.Columns.Add("NAME", System.Type.GetType("System.String"))
                For Each row As DataRow In m_Versendungen.Rows

                    row("NAME") = row("ZS_NAME1").ToString & "<br />" & row("ZS_STREET").ToString & " " & row("ZS_HOUSE_NUM1").ToString & _
                    "<br />" & row("ZS_POST_CODE1").ToString & " " & row("ZS_CITY1").ToString

                Next

                CreateOutPut(m_Versendungen, strAppID)

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
