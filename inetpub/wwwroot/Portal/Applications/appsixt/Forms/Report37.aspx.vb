Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Report37
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_blnUnvollstaendigeTuete As Boolean

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If (Not Request.QueryString("Ges") Is Nothing) AndAlso Request.QueryString("Ges").ToString = "1" Then
                m_blnUnvollstaendigeTuete = False
            Else
                m_blnUnvollstaendigeTuete = True
            End If

            DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            Dim m_Report As New Sixt_B11(m_User, m_App, strFileName)

            lblError.Text = ""

            If lblError.Text.Length = 0 Then
                m_Report.UnvollstaendigeTuete = m_blnUnvollstaendigeTuete
                m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)

                m_Report.Result.Columns.Add("Delete", GetType(System.Boolean))

                For Each dr As DataRow In m_Report.Result.Rows

                    dr("Delete") = False


                Next

                Session("ResultTable") = m_Report.Result
                Session("UnvollstaendigeTuete") = m_Report.UnvollstaendigeTuete

                If Not m_Report.Status = 0 Then
                    lblError.Text = "Fehler: " & m_Report.Message
                Else
                    If m_Report.Result.Rows.Count = 0 Then
                        lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Else

                        Dim tmpTbl As DataTable = m_Report.Result.Copy
                        tmpTbl.Columns.Remove("Equipmenttyp")
                        tmpTbl.Columns.Remove("Equipmentnummer")
                        tmpTbl.Columns.Remove("Delete")
                        If m_Report.UnvollstaendigeTuete Then
                            tmpTbl.Columns.Remove("Kennzeichen")
                            'tmpTbl.Columns.Remove("Erstzulassungsdatum")
                            tmpTbl.Columns.Remove("Briefnummer")
                            tmpTbl.Columns.Remove("PDI-Nr")
                        Else
                            tmpTbl.Columns.Remove("Ersatzsschlüssel")
                            tmpTbl.Columns.Remove("Carpass")
                            tmpTbl.Columns.Remove("Radiocodekarte")
                            tmpTbl.Columns.Remove("CD-Navi")
                            tmpTbl.Columns.Remove("Chipkarte")
                            tmpTbl.Columns.Remove("COC-Papier")
                            tmpTbl.Columns.Remove("Navi-Codekarte")
                            tmpTbl.Columns.Remove("Codekarte WFS")
                            tmpTbl.Columns.Remove("Ersatzfernbed Standh")
                            tmpTbl.Columns.Remove("Prüfbuch LKW")
                        End If
                        Try
                            Excel.ExcelExport.WriteExcel(tmpTbl, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                        Catch ex As Exception
                        End Try
                        Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                        Response.Redirect("Report37_2.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report37.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 7.01.11    Time: 8:49
' Updated in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 14:10
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 22.05.07   Time: 11:27
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 7  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' 
' ************************************************
