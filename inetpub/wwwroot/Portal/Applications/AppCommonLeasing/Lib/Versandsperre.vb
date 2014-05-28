Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common
Imports CKG.Base.Kernel

Public Class Versandsperre
    Inherits BankBase

    Public Sub New(ByVal user As User, ByVal app As App, ByVal appId As String)
        MyBase.new(user, app, appId, HttpContext.Current.Session.SessionID, String.Empty)

    End Sub

    Public Sub GetData(fahrgestellnummer As String, kennzeichen As String, vertragsnummer As String, objektnummer As String, von As DateTime?, bis As DateTime?)
        Try
            m_intStatus = 0
            m_strMessage = String.Empty
            m_tblResult = Nothing

            'Dim proxy = DynSapProxy.getProxy("Z_DPM_READ_VERS_SPERR_01", m_objApp, m_objUser, page)

            'proxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, "0"c))

            S.AP.Init("Z_DPM_READ_VERS_SPERR_01", "I_AG", m_strKUNNR)

            ' unused parameter??  VALUE(I_TG) TYPE  ZKUNNR_TG OPTIONAL
            If (Not String.IsNullOrEmpty(fahrgestellnummer)) Then S.AP.SetImportParameter("I_CHASSIS_NUM", fahrgestellnummer) 'proxy.setImportParameter("I_CHASSIS_NUM", fahrgestellnummer)
            If (Not String.IsNullOrEmpty(kennzeichen)) Then S.AP.SetImportParameter("I_LICENSE_NUM", kennzeichen) 'proxy.setImportParameter("I_LICENSE_NUM", kennzeichen)
            If (Not String.IsNullOrEmpty(vertragsnummer)) Then S.AP.SetImportParameter("I_LIZNR", vertragsnummer) 'proxy.setImportParameter("I_LIZNR", vertragsnummer)
            If (Not String.IsNullOrEmpty(objektnummer)) Then S.AP.SetImportParameter("I_ZZREFERENZ1", objektnummer) 'proxy.setImportParameter("I_ZZREFERENZ1", objektnummer)
            If (von.HasValue) Then S.AP.SetImportParameter("I_ZZTMPDT_VON", von.Value.ToShortDateString()) 'proxy.setImportParameter("I_ZZTMPDT_VON", von.Value.ToShortDateString())
            If (bis.HasValue) Then S.AP.SetImportParameter("I_ZZTMPDT_BIS", bis.Value.ToShortDateString()) 'proxy.setImportParameter("I_ZZTMPDT_BIS", bis.Value.ToShortDateString())

            'proxy.callBapi()
            S.AP.Execute()

            Dim tmpResult = S.AP.GetExportTable("GT_OUT") 'proxy.getExportTable("GT_OUT")
            Dim statusCol = New DataColumn With {.ColumnName = "Status", .DataType = GetType(String), .DefaultValue = "-"}
            tmpResult.Columns.Add(statusCol)
            tmpResult.AcceptChanges()

            m_tblResult = tmpResult

        Catch ex As Exception

            Select Case ex.Message
                Case "NO_DATA"
                    m_intStatus = -3000
                    m_strMessage = "Keine gesperrten Versandänderungen gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = String.Format("Beim Erstellen des Reports ist ein Fehler aufgetreten ({0}).", ex.Message)
            End Select
        End Try
    End Sub

    Public Sub Entsperren()
        Try
            m_intStatus = 0
            m_strMessage = String.Empty

            'Dim proxy = DynSapProxy.getProxy("Z_DPM_EQUI_DEL_SPERR_01", m_objApp, m_objUser, page)
            'proxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, "0"c))

            S.AP.Init("Z_DPM_EQUI_DEL_SPERR_01", "I_AG", m_strKUNNR)

            Dim fzg = S.AP.GetImportTable("GT_FZG") 'proxy.getImportTable("GT_FZG")
            Dim equnrs = Result.Select("Status='X'").Select(Function(r) CStr(r("EQUNR"))).ToList()
            equnrs.ForEach(Sub(e)
                               Dim r = fzg.NewRow()
                               r("EQUNR") = e
                               fzg.Rows.Add(r)
                           End Sub)
            fzg.AcceptChanges()

            'proxy.callBapi()
            S.AP.Execute()

            Dim tmpResult = S.AP.GetExportTable("GT_FZG") 'proxy.getExportTable("GT_FZG")
            For Each r As DataRow In tmpResult.Rows
                Dim equnr = CStr(r("EQUNR"))
                Dim r2 = Result.Select(String.Format("EQUNR='{0}'", equnr)).FirstOrDefault()
                If Not r2 Is Nothing Then r2("Status") = CStr(r("BEM"))
            Next
        Catch ex As Exception
            Select Case ex.Message
                Case Else
                    m_intStatus = -9999
                    m_strMessage = String.Format("Beim Erstellen des Reports ist ein Fehler aufgetreten ({0}).", ex.Message)
            End Select
        End Try
    End Sub

    Public Overrides Sub Show()
        Throw New NotImplementedException()
    End Sub

    Public Overrides Sub Change()
        Throw New NotImplementedException()
    End Sub
End Class
