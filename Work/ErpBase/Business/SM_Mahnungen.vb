
Namespace Business
    Public Class SM_Mahnungen
        Inherits StartMethodBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
        Public Sub New(ByRef frmWebForm As System.Web.UI.Page, ByVal objUser As Kernel.Security.User, ByVal strFilename As String)
            MyBase.New(frmWebForm, objUser, strFilename)
            RetrieveAppInfo("Report33")
            Run()
        End Sub

        Public Overrides Sub Run()
            If m_objuser.Reference = String.Empty Then
                SetRunFlag()
            End If
            If NotRunYet Then
                If Not m_blnGestartet Then
                    m_blnGestartet = True

                    Dim m_Report As TempMahn

                    m_Report = New TempMahn(m_objUser, m_objApp, m_strFileName)
                    m_Report.Fill(m_intAppID.ToString)

                    If Not m_Report.Status = 0 Then
                        m_strMessage = m_Report.Message
                        Exit Sub
                    Else
                        If m_Report.Result.Rows.Count = 0 Then
                            m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                            Exit Sub
                        Else
                            m_frmWebForm.Session.Add("Mahnungen", m_Report.Result)
                            m_blnGestartet = False
                            SetRunFlag()
                            Redirect()
                        End If
                    End If
                    m_blnGestartet = False
                    SetRunFlag()
                End If
            End If
        End Sub
#End Region

    End Class
End Namespace

' ************************************************
' $History: SM_Mahnungen.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Base/Business
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 4  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Base/Base/Business
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Business
' 
' ************************************************