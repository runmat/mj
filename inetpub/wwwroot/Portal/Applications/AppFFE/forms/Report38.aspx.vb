Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Report38
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Dim m_Report As FFE_Lastschriften
    Dim sURL As String
    Dim bRetail As Boolean
    Dim bhaendler As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        Session("ShowOtherString") = ""
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        If m_User.Reference.Trim(" "c).Length > 0 Then
            bhaendler = True
        End If
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Request.Params("art") = "Retail" Then
                bRetail = True
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
        'Initiales befüllen
        If Not IsPostBack Then
            sURL = "Report38_2.aspx?AppID=" & Session("AppID").ToString
            txtBisDatum.Text = Today
            txtAbDatum.Text = Today
        End If
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Try
            lblError.Text = ""
            If txtAbDatum.Text.Length > 0 Then
                If Not IsDate(txtAbDatum.Text) Then
                    If Not IsStandardDate(txtAbDatum.Text) Then
                        If Not IsSAPDate(txtAbDatum.Text) Then
                            lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                            Exit Sub
                        End If
                    End If
                End If
            End If
            If txtBisDatum.Text.Length > 0 Then
                If Not IsDate(txtBisDatum.Text) Then
                    If Not IsStandardDate(txtBisDatum.Text) Then
                        If Not IsSAPDate(txtBisDatum.Text) Then
                            lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                            Exit Sub
                        End If
                    End If
                End If
            End If
            If txtBisDatum.Text.Length + txtAbDatum.Text.Length > 0 Then
                Dim datAb As Date = CDate(txtAbDatum.Text)
                Dim datBis As Date = CDate(txtBisDatum.Text)
                If datAb > datBis Then
                    lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
                    Exit Sub
                End If
            End If

            Dim erledigt As String = ""

            erledigt = "A"
            If rdo_Erledigt.Checked = True Then erledigt = "E"
            If rdo_Offen.Checked = True Then erledigt = "O"

            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
         
            m_Report = New FFE_Lastschriften(m_User, m_App, strFileName)
            m_Report.FlagErledigt = erledigt
            m_Report.datVON = txtAbDatum.Text
            m_Report.datBIS = txtBisDatum.Text
            If bhaendler = True Then
                m_Report.Haendler = Right("0000000000" & 60 & m_User.Reference, 10)
            End If
            'm_Report.Haendler = Right("0000000000" & 60 & "34199", 10)
            m_Report.Fill(Session("AppID").ToString, Session.SessionID.ToString)

            Session("m_report") = m_Report
            Session("ResultTable") = m_Report.Result

            If Not m_Report.Status = 0 Then
                lblError.Text = m_Report.Message
            Else
                If m_Report.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    Dim tmpRows As DataRow()
                    Dim i As Integer
                    For i = 1 To 4

                        tmpRows = m_Report.Result.Select("Kontingentart = '000" & i & "'")
                        If tmpRows.Length > 0 Then
                            Dim i2 As Integer

                            For i2 = 0 To tmpRows.Length - 1
                                Dim iLoeschFlag As Integer = 0
                                tmpRows(0).BeginEdit()

                                If tmpRows(i2).Item("Kontingentart") = "0001" Then
                                    tmpRows(i2).Item("Kontingentart") = "Standard Temporär"
                                    If bRetail = True Then iLoeschFlag = 1
                                End If
                                If tmpRows(i2).Item("Kontingentart") = "0002" Then
                                    tmpRows(i2).Item("Kontingentart") = "Standard endgültig"
                                    If bRetail = True Then iLoeschFlag = 1
                                End If
                                If tmpRows(i2).Item("Kontingentart") = "0003" Then
                                    tmpRows(i2).Item("Kontingentart") = "Retail"
                                    If Not bRetail = True Then iLoeschFlag = 1
                                End If
                                If tmpRows(i2).Item("Kontingentart") = "0004" Then
                                    tmpRows(i2).Item("Kontingentart") = "Delayed Payment"
                                    If bRetail = True Then iLoeschFlag = 1
                                End If
                                If iLoeschFlag = 1 Then
                                    tmpRows(i2).Delete()
                                End If
                                tmpRows(i2).EndEdit()
                                m_Report.Result.AcceptChanges()
                            Next i2
                        End If
                    Next

                    tmpRows = m_Report.Result.Select("Kontingentart = '0006'")
                    If tmpRows.Length > 0 Then
                        Dim i2 As Integer
                        For i2 = 0 To tmpRows.Length - 1
                            Dim iLoeschFlag As Integer = 0
                            tmpRows(0).BeginEdit()
                            If tmpRows(i2).Item("Kontingentart") = "0006" Then
                                tmpRows(i2).Item("Kontingentart") = "KF/KL"
                                If bRetail = True Then iLoeschFlag = 1
                            End If
                            If iLoeschFlag = 1 Then
                                tmpRows(i2).Delete()
                            End If
                            tmpRows(i2).EndEdit()
                            m_Report.Result.AcceptChanges()
                        Next i2
                    End If


                    Session("ResultTable") = m_Report.Result
                    Session("ShowOtherString") = "Es wurden " & m_Report.Result.Rows.Count.ToString & " Einträge gefunden."

                    Try
                        Excel.ExcelExport.WriteExcel(m_Report.Result, System.Configuration.ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Catch
                    End Try
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                    Response.Redirect("Report38_2.aspx?AppID=" & Session("AppID").ToString)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
        End Try
    End Sub

    Private Sub btnOpenSelectAb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectAb.Click
        calAbDatum.Visible = True
    End Sub

    Private Sub btnOpenSelectBis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSelectBis.Click
        calBisDatum.Visible = True
    End Sub

    Private Sub calAbDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calAbDatum.SelectionChanged
        calAbDatum.Visible = False
        txtAbDatum.Text = calAbDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub calBisDatum_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calBisDatum.SelectionChanged
        calBisDatum.Visible = False
        txtBisDatum.Text = calBisDatum.SelectedDate.ToShortDateString
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class
' ************************************************
' $History: Report38.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 16.07.08   Time: 11:34
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 18.06.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' Ausblenden Hndler Kontingente
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.06.08    Time: 18:43
' Updated in $/CKAG/Applications/AppFFE/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
