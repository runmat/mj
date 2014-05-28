Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel

Public Class ec_22
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_tblResultZBII As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property ResultZBII() As DataTable
        Get
            Return m_tblResultZBII
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal datEingangsdatumVon As DateTime, ByVal datEingangsdatumBis As DateTime, ByVal page As Page)

        m_strClassAndMethod = "ec_22.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_ZB2_EING_ZUM_ZEITRAUM_01", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_ERDAT_EQUI_VON", datEingangsdatumVon.ToShortDateString)
            'myProxy.setImportParameter("I_ERDAT_EQUI_BIS", datEingangsdatumBis.ToShortDateString)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_DPM_ZB2_EING_ZUM_ZEITRAUM_01", "I_KUNNR_AG,I_ERDAT_EQUI_VON,I_ERDAT_EQUI_BIS", _
                             Right("0000000000" & m_objUser.KUNNR, 10), datEingangsdatumVon.ToShortDateString, datEingangsdatumBis.ToShortDateString)

            Dim tblTemp As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            tblTemp.AcceptChanges()

            ' Spaltenübersetzung
            CreateOutPut(tblTemp, strAppID)

            'ZBII - Summierung
            m_tblResultZBII = New DataTable()
            m_tblResultZBII.Columns.Add("Hersteller", System.Type.GetType("System.String"))
            m_tblResultZBII.Columns.Add("Anzahl", System.Type.GetType("System.Int32"))

            Dim rowResult As DataRow
            Dim rowZBII As DataRow()
            Dim rowNew As DataRow
            Dim intCount As Integer

            intCount = 0
            For Each rowResult In m_tblResult.Rows
                rowZBII = m_tblResultZBII.Select("Hersteller='" & CStr(rowResult("Hersteller")) & "'")
                If (rowZBII.Length = 0) Then
                    'Hersteller noch nicht gefunden
                    rowNew = m_tblResultZBII.NewRow
                    rowNew("Hersteller") = CStr(rowResult("Hersteller"))
                    rowNew("Anzahl") = 1
                    m_tblResultZBII.Rows.Add(rowNew)
                Else
                    'Hersteller gefunden, Anzahl hochsetzen
                    rowZBII(0)("Anzahl") = 1 + CInt(rowZBII(0)("Anzahl"))
                End If
            Next
            m_tblResultZBII.AcceptChanges()

        Catch ex As Exception
            m_intStatus = -4444
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                    WriteLogEntry(False, "Bei der Datenselektion (VON: " & datEingangsdatumVon.ToShortDateString & ", BIS: " & datEingangsdatumBis.ToShortDateString & ", KUNNR: " & m_objUser.KUNNR & ") ist folgender Fehler aufgetreten: " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            End Select
        End Try
    End Sub
#End Region
End Class
