Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Imports SapORM.Contracts


Public Class FehlendeCD
    Inherits Base.Business.DatenimportBase



#Region " Declarations"

    Private dtSAPHersteller As DataTable
    Private m_Filename As String


#End Region

#Region " Properties"
    Public ReadOnly Property FileName() As String
        Get
            Return m_Filename
        End Get
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub
    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "FehlendeCD.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_FZG_OHNE_NAVI_CD_001", m_objApp, m_objUser, page)
            'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.callBapi()

            S.AP.InitExecute("Z_DPM_FZG_OHNE_NAVI_CD_001", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

            Dim tblTemp As DataTable
            'tblTemp = myProxy.getExportTable("GT_WEB")
            tblTemp = S.AP.GetExportTable("GT_WEB")

            CreateOutPut(tblTemp, strAppID)


            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            Select Case ex.Message
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Ergebnisse zu den Kriterien gefunden."
                Case "NO_DATA"
                    m_intStatus = -12
                    m_strMessage = "Keine Fahrzeuge gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Public Sub DelDocuments(ByVal strAppID As String, ByVal strSessionID As String, ByVal strTidNR As String, ByVal page As Page)
        m_strClassAndMethod = "DatenOhneZB2.DelDocuments"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_DATEN_OHNE_BRIEF_DEL", m_objApp, m_objUser, page)
            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_TidNR", strTidNR)
            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_DATEN_OHNE_BRIEF_DEL", "I_AG, I_TidNR", m_objUser.KUNNR.ToSapKunnr(), strTidNR)

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case ex.Message
                Case "NOT_DEL"
                    m_intStatus = -1111
                    m_strMessage = "Eintrag konnte nicht gelöscht werden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

#End Region


End Class
