Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel

Public Class Fahrzeugbestand
    Inherits BankBase

    Private Const BAPI_READ = "Z_DPM_FAHRZEUGBESTAND_001"
    Private Const BAPI_CHANGE = "Z_DPM_CHANGE_EQUI_BNP_001"

    Public Property FilterFIN As String
    Public Property FilterHaendlernummer As String ' Equi Partnerrolle ZF
    Public Property FilterKundennummer As String ' Equi Partnerrolle ZS
    Public Property FilterBranding As String ' Equi-Label/Marke

    Public Property SelectedFIN As String

    Public Property ChangedHaendlernummer As String ' I_HAENDLER
    Public Property ChangedKundennummer As String ' I_KUNNR_ZS
    Public Property ChangedVertragsnummer As String ' I_VERTRAGNR
    Public Property ChangedBranding As String ' I_BRANDING

    Public Sub New(user As User, app As App, appId As String)
        MyBase.New(user, app, appId, HttpContext.Current.Session.SessionID, String.Empty)
    End Sub

    Public Overrides Sub Show()
        ClearError()

        S.AP.Init(BAPI_READ)

        S.AP.SetImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, "0"c))
        S.AP.SetImportParameter("I_KUNNR_ZS", FilterKundennummer)
        S.AP.SetImportParameter("I_HAENDLER", FilterHaendlernummer)
        S.AP.SetImportParameter("I_BRANDING", FilterBranding)
        S.AP.SetImportParameter("I_FIN", FilterFIN)

        Try
            S.AP.Execute()

            m_strMessage = CStr(S.AP.GetExportParameter("E_MESSAGE"))
            m_intStatus = CInt(S.AP.GetExportParameter("E_SUBRC"))

            Select Case m_intStatus
                Case 101 'no data
                    RaiseError("-1", "Es wurden keine entsprechenden Daten gefunden!")
                Case 102 ' no kunnr
                    RaiseError("-2", "Diese Kunden-Nr haben wir in unserem System nicht gefunden!")
            End Select

            m_tblResult = S.AP.GetExportTable("GT_OUT")

        Catch ex As Exception
            RaiseError("-999", ex.Message)
            m_tblResult = Nothing
        End Try
    End Sub

    Public Overrides Sub Change()
        ClearError()

        S.AP.Init(BAPI_CHANGE)

        S.AP.SetImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, "0"c))
        S.AP.SetImportParameter("I_FIN", SelectedFIN)
        S.AP.SetImportParameter("I_USER", m_objUser.UserName)
        S.AP.SetImportParameter("I_KUNNR_ZS", ChangedKundennummer)
        S.AP.SetImportParameter("I_HAENDLER", ChangedHaendlernummer)
        S.AP.SetImportParameter("I_BRANDING", ChangedBranding)
        S.AP.SetImportParameter("I_VERTRAGNR", ChangedVertragsnummer)

        Try
            S.AP.Execute()
            Dim myMagicNumber = CInt(S.AP.GetExportParameter("E_SUBRC"))

            m_strMessage = S.AP.GetExportParameter("E_MESSAGE")
            Select Case myMagicNumber
                Case 101, 102, 103, 105 ' we have a problem here..
                    m_intStatus = 100 - myMagicNumber
                Case 104
                    m_intStatus = 0 ' 104! Success!
                    m_strMessage = String.Empty
            End Select

        Catch ex As Exception
            m_intStatus = -99
            m_strMessage = ex.ToString
        End Try
    End Sub
End Class
