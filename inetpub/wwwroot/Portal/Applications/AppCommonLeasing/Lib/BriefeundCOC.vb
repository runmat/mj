Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
'Imports Microsoft.Data.SAPClient
Imports CKG.Base.Common

Public Class BriefeundCOC
    Inherits Base.Business.DatenimportBase

#Region "Declarations"
    Private mE_SUBRC As String
    Private mE_MESSAGE As String
    Private mErdatVon As String
    Private mErdatBis As String
#End Region

#Region "Properties"
    Public Property E_SUBRC() As String
        Get
            Return mE_SUBRC
        End Get
        Set(ByVal Value As String)
            mE_SUBRC = Value
        End Set
    End Property

    Public Property E_MESSAGE() As String
        Get
            Return mE_MESSAGE
        End Get
        Set(ByVal Value As String)
            mE_MESSAGE = Value
        End Set
    End Property

    Public Property ErdatVon() As String
        Get
            Return mErdatVon
        End Get
        Set(ByVal Value As String)
            mErdatVon = Value
        End Set
    End Property
    Public Property ErdatBis() As String
        Get
            Return mErdatBis
        End Get
        Set(ByVal Value As String)
            mErdatBis = Value
        End Set
    End Property
#End Region

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL_COCohneTypenschein(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "BriefeundCOC.FILL_COCohneTypenschein"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_COC_OHNE_TYPENSCHEIN", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()

            S.AP.InitExecute("Z_DPM_COC_OHNE_TYPENSCHEIN", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tblTemp As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            CreateOutPut(tblTemp, m_strAppID)

            mE_SUBRC = S.AP.GetExportParameter("E_SUBRC") 'myProxy.getExportParameter("E_SUBRC")
            mE_MESSAGE = S.AP.GetExportParameter("E_MESSAGE") 'myProxy.getExportParameter("E_MESSAGE")

            If mE_SUBRC <> "0" Then
                m_intStatus = CInt(mE_SUBRC)
                m_strMessage = mE_MESSAGE
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            End If

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case ex.Message
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Ergebnisse zu den Kriterien gefunden."
                Case "NO_DATA"
                    m_intStatus = -12
                    m_strMessage = "Keine Dokumente gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Public Overloads Sub FILL_TypenscheinOhneCOC(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "BriefeundCOC.FILL_TypenscheinOhneCOC"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_TYPENSCHEIN_OHNE_COC", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_ERDAT_VON", mErdatVon)
            'myProxy.setImportParameter("I_ERDAT_BIS", mErdatBis)
            'myProxy.callBapi()

            S.AP.Init("Z_DPM_TYPENSCHEIN_OHNE_COC")

            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            S.AP.SetImportParameter("I_ERDAT_VON", mErdatVon)
            S.AP.SetImportParameter("I_ERDAT_BIS", mErdatBis)

            S.AP.Execute()

            Dim tblTemp As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            CreateOutPut(tblTemp, m_strAppID)

            ResultTable = Result.Copy

            mE_SUBRC = S.AP.GetExportParameter("E_SUBRC") 'myProxy.getExportParameter("E_SUBRC")
            mE_MESSAGE = S.AP.GetExportParameter("E_MESSAGE") 'myProxy.getExportParameter("E_MESSAGE")

            If mE_SUBRC <> "0" Then
                m_intStatus = CInt(mE_SUBRC)
                m_strMessage = mE_MESSAGE
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            End If

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case ex.Message
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Ergebnisse zu den Kriterien gefunden."
                Case "NO_DATA"
                    m_intStatus = -12
                    m_strMessage = "Keine Dokumente gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Public Overloads Sub SetBezahlt(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "BriefeundCOC.SetBezahlt"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_TYPENSCHEIN_SET_BEZAHLT", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            S.AP.Init("Z_DPM_TYPENSCHEIN_SET_BEZAHLT", "KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tblTemp As DataTable = S.AP.GetImportTable("GT_WEB") 'myProxy.getImportTable("GT_WEB")
            Dim NewRow As DataRow

            For Each dr As DataRow In ResultTable.DefaultView.ToTable.Rows
                NewRow = tblTemp.NewRow

                NewRow("EQUNR") = dr("EQUNR")
                NewRow("CHASSIS_NUM") = dr("Fahrgestellnummer")

                tblTemp.Rows.Add(NewRow)
            Next

            'myProxy.callBapi()
            S.AP.Execute()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = "Beim Speichern ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

End Class
