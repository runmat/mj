Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports System.Reflection
Imports CKG.Base.Common
Imports CKG.Base.Kernel

Public Class NichtAktiveZulassungen
    Inherits ReportBase

    Public Sub New(user As User, app As App)
        MyBase.New(user, app, "")

    End Sub


    Public Overrides Sub Fill()
        Dim mi = MethodInfo.GetCurrentMethod()
        m_strClassAndMethod = mi.DeclaringType.Name + "." + mi.Name
        m_intStatus = 0
        m_strMessage = ""
        m_tblResult = Nothing

        Try
            'Dim page = DirectCast(HttpContext.Current.Handler, Page)
            Dim kunnr = m_objUser.KUNNR

            'Dim proxy = DynSapProxy.getProxy("Z_M_Briefbestand_001", m_objApp, m_objUser, page)

            'proxy.setImportParameter("I_KUNNR", kunnr.PadLeft(10, "0"c))

            'proxy.callBapi()

            S.AP.InitExecute("Z_M_Briefbestand_001", "I_KUNNR", kunnr.PadLeft(10, "0"c))

            Dim tmpResult = S.AP.GetExportTable("GT_DATEN") 'proxy.getExportTable("GT_DATEN")
            tmpResult.DefaultView.RowFilter = "EXPIRY_DATE IS NOT NULL"

            m_tblResult = tmpResult.DefaultView.ToTable()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case ex.Message
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

End Class
