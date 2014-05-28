Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class UnangeforderteEquipments
    Inherits BankBase

    Private Const BAPI_Unangefordert As String = "Z_M_UNANGEFORDERT_STD"

    Sub New(ByVal user As User, ByVal app As App, ByVal appId As String, ByVal sessionID As String)
        MyBase.New(user, app, appId, sessionID, "")

        KUNNR = user.KUNNR
    End Sub

    Private lastFIN As String
    Private lastZB2 As String

    Public Sub LoadData(ByVal haendlerNr As String, ByVal fin As String, ByVal zb2 As String, ByRef page As Page)
        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod

        If lastFIN = fin AndAlso lastZB2 = zb2 AndAlso Not Result Is Nothing Then Exit Sub

        m_intStatus = 0
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name

        Try
            Dim oldSelection As DataRow() = Nothing
            If Not Result Is Nothing Then oldSelection = Result.Select("Selected")

            Dim proxy = DynSapProxy.getProxy(BAPI_Unangefordert, m_objApp, m_objUser, page)

            proxy.setImportParameter("I_AG", KUNNR)
            proxy.setImportParameter("I_HAENDLER_EX", haendlerNr)
            If Not String.IsNullOrEmpty(fin) Then proxy.setImportParameter("I_CHASSIS_NUM", fin.ToUpper)
            If Not String.IsNullOrEmpty(zb2) Then proxy.setImportParameter("I_ZB2", zb2.ToUpper)

            proxy.callBapi()

            Dim r = proxy.getExportTable("GT_WEB")

            Dim selCol = New DataColumn("Selected", GetType(Boolean))
            selCol.DefaultValue = False
            r.Columns.Add(selCol)

            Dim augruCol = New DataColumn("AUGRU", GetType(String))
            augruCol.DefaultValue = String.Empty
            r.Columns.Add(augruCol)

            Dim subjectCol = New DataColumn("Subject", GetType(String))
            subjectCol.DefaultValue = String.Empty
            r.Columns.Add(subjectCol)

            Dim commentCol = New DataColumn("COMMENT", GetType(String))
            commentCol.DefaultValue = String.Empty
            r.Columns.Add(commentCol)

            Dim vbelnCol = New DataColumn("VBELN", GetType(String))
            vbelnCol.DefaultValue = String.Empty
            r.Columns.Add(vbelnCol)

            ' Re-Add old selection to new table?!
            If Not oldSelection Is Nothing Then
                For Each row In oldSelection
                    Dim sFin = row("CHASSIS_NUM")
                    Dim newRow = r.Select(String.Format("CHASSIS_NUM='{0}'", sFin)).FirstOrDefault
                    If Not newRow Is Nothing Then newRow("Selected") = True
                Next
            End If

            r.AcceptChanges()

            m_tblResult = r
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_HAENDLER"
                    m_intStatus = -10
                    m_strMessage = "Händler nicht vorhanden."
                Case "NO_CREDITSTEUER"
                    m_intStatus = -11
                    m_strMessage = "Keinen Eintrag in ZDADCREDITSTEUER für den AG."
                Case "NO_DATA"
                    m_intStatus = -12
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select

            m_tblResult = Nothing
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & m_strMessage.Replace("<br>", " "), Nothing)
        End Try
    End Sub


    Public Overrides Sub Change()
        Throw New NotImplementedException
    End Sub

    Public Overrides Sub Show()
        Throw New NotImplementedException
    End Sub
End Class
