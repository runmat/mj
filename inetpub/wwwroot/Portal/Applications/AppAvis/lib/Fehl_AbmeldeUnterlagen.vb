Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts


Public Class Fehl_AbmeldeUnterlagen
    Inherits Base.Business.DatenimportBase
#Region " Declarations"
    Private m_tblSperren As DataTable
    Private m_tblEntsperren As DataTable

#End Region

#Region " Properties"
    Public Property Sperren() As DataTable
        Get
            Return m_tblSperren
        End Get
        Set(ByVal value As DataTable)
            m_tblSperren = value
        End Set
    End Property
    Public Property Entsperren() As DataTable
        Get
            Return m_tblEntsperren
        End Get
        Set(ByVal value As DataTable)
            m_tblEntsperren = value
        End Set
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String)

        m_strClassAndMethod = "Fehl_AbmeldeUnterlagen.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            S.AP.InitExecute("Z_M_ABM_FEHL_UNTERL_001", "KUNNR", m_objUser.KUNNR.ToSapKunnr())

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("AUSGABE")

            CreateOutPut(tblTemp2, strAppID)

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally

            m_blnGestartet = False
        End Try
    End Sub


#End Region
End Class
