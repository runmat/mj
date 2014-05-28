Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common

Public Class Klaerfaelle
    Inherits BankBase

    Const BAPI_read_kf As String = "Z_DPM_READ_MOFA_KF_01"
    Const BAPI_save_kf As String = "Z_DPM_SAVE_MOFA_KF_01"
    Dim m_Versicherungsinfos As DataTable

    Sub New(ByVal user As User, ByVal app As CKG.Base.Kernel.Security.App, ByVal appId As String, ByVal sessionID As String)
        MyBase.New(user, app, appId, sessionID, "")

        KUNNR = user.KUNNR
    End Sub

    Public Sub LoadData(ByRef page As Page)
        m_intStatus = 0

        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name

        Try
            Dim proxy = DynSapProxy.getProxy(BAPI_read_kf, m_objApp, m_objUser, page)

            proxy.setImportParameter("I_AG", KUNNR)

            proxy.callBapi()

            Dim result = proxy.getExportTable("GT_KF")

            Dim ivCol = result.Columns.Add("INFO_VERS", GetType(String))
            ivCol.DefaultValue = String.Empty

            
            If result.Rows.Count = 0 Then
                m_intStatus = -10
                m_strMessage = "Keine Klärfälle."
            Else
                Dim r = New Regex("(\d{4})(\d{4})(\d)", RegexOptions.Compiled)
                For Each row In result.Rows
                    Dim value = CStr(row("EIKTO_VM"))
                    If (r.IsMatch(value)) Then row("EIKTO_VM") = r.Replace(value, "$1-$2-$3")
                Next
            End If

            result.AcceptChanges()

            m_Versicherungsinfos = proxy.getExportTable("GT_IV")
            m_tblResult = result

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -12
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & m_strMessage.Replace("<br>", " "), m_tblResult)
        End Try
    End Sub

    Public Sub Save(ByRef page As Page)
        m_intStatus = 0

        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name

        Try
            Dim proxy = DynSapProxy.getProxy(BAPI_save_kf, m_objApp, m_objUser, page)

            proxy.setImportParameter("I_AG", KUNNR)

            Dim gt_in = proxy.getImportTable("GT_IN")

            For Each row In m_tblResult.Select("INFO_VERS <> ''")
                Dim row_in = gt_in.NewRow
                row_in("KUNDE") = row("KUNDE")
                row_in("EIKTO_VM") = Replace(row("EIKTO_VM"), "-", "")
                row_in("VERS_JAHR") = row("VERS_JAHR")
                row_in("SERNR") = row("SERNR")
                row_in("ERDAT") = row("ERDAT")
                row_in("INFO_VERS") = row("INFO_VERS")
                row_in("DAT_ERL") = DateTime.Now.ToShortDateString
                gt_in.Rows.Add(row_in)
            Next
            gt_in.AcceptChanges()

            proxy.callBapi()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -12
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & m_strMessage.Replace("<br>", " "), m_tblResult)
        End Try
    End Sub

    Public ReadOnly Property Versicherungsinfos As DataTable
        Get
            Return m_Versicherungsinfos
        End Get
    End Property


    Public Overrides Sub Show()
        Throw New NotImplementedException
    End Sub

    Public Overrides Sub Change()
        Throw New NotImplementedException
    End Sub



End Class
