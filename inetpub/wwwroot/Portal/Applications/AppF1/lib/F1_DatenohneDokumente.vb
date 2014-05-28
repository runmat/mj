Option Explicit On
Option Strict On

Imports CKG.Base
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common

Public Class F1_DatenohneDokumente
    Inherits BankBase

#Region " Declarations"
    Private m_strHaendler As String
    Private mDokumente As DataTable
#End Region

#Region " Properties"
    Public Property Haendler() As String
        Get
            Return m_strHaendler
        End Get
        Set(ByVal Value As String)
            m_strHaendler = Value
        End Set
    End Property
    Public ReadOnly Property tblDokumente() As DataTable
        Get
            Return mDokumente
        End Get
    End Property
#End Region

#Region "Methods"

    Public Sub New(ByRef objUser As User, ByRef objApp As App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_strHaendler = ""
    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String)

        m_strClassAndMethod = "F1_DokumenteohneDaten.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_DATEN_OHNE_BRIEF", m_objApp, m_objUser, Page)

                'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.Init("Z_DPM_DATEN_OHNE_BRIEF", "I_KUNNR_AG", m_strKUNNR)

                If Not m_strHaendler = String.Empty Then
                    'myProxy.setImportParameter("I_KUNNR_ZF", m_strHaendler)
                    S.AP.SetImportParameter("I_KUNNR_ZF", m_strHaendler)
                End If


                m_intStatus = 0
                m_strMessage = ""

                'myProxy.callBapi()
                S.AP.Execute()

                mDokumente = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
                mDokumente.Columns.Remove("ZZFINART")
                mDokumente.Columns.Remove("NAME1_ZF")
                mDokumente.Columns.Remove("LIZNR")
                mDokumente.Columns.Remove("ORIGBETRAG")
                mDokumente.Columns.Remove("CURR")
                mDokumente.Columns.Remove("ZZFAEDT")
                mDokumente.Columns.Remove("ZZABRDT")

            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -1402
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select

                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ",   " & Replace(m_strMessage, "<br>", " "), Nothing)
            End Try

        End If
        m_blnGestartet = False
    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overrides Sub Show()

    End Sub
#End Region


End Class
