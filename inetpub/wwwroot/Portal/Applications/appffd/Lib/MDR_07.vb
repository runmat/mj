Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class MDR_07
    Inherits Base.Business.DatenimportBase
    Private strfahrgestellnummer As String
    Private strkennzeichen As String
    Private strordernummer As String

#Region " Properties "

    Public Property kennzeichen() As String
        Get
            Return strkennzeichen
        End Get
        Set(ByVal value As String)
            strkennzeichen = value
        End Set
    End Property

    Public Property fahrgestellnummer() As String
        Get
            Return strfahrgestellnummer
        End Get
        Set(ByVal value As String)
            strfahrgestellnummer = value
        End Set
    End Property

    Public Property Ordernummer() As String
        Get
            Return strordernummer
        End Get
        Set(ByVal value As String)
            strordernummer = value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "MDR_7.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Historie_Zb2", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_IDENTNR", strfahrgestellnummer.ToUpper)
                'myProxy.setImportParameter("I_KENNZEICHEN", strkennzeichen.ToUpper)
                'myProxy.setImportParameter("I_ORDERNR", strordernummer.ToUpper)

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_Historie_Zb2", "I_IDENTNR,I_KENNZEICHEN,I_ORDERNR", strfahrgestellnummer.ToUpper, strkennzeichen.ToUpper, strordernummer.ToUpper)

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_HISTORIE") 'myProxy.getExportTable("GT_HISTORIE")

                CreateOutPut(tblTemp2, m_strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NOT_FOUND"
                        m_intStatus = -1111
                        m_strMessage = "Keine Fahrzeuge gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region


End Class

