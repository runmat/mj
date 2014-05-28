Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports System.IO

Partial Public Class Change01_4
    Inherits System.Web.UI.Page
    Private m_App As App
    Private m_User As User
    Private m_change As Logistik.Logistik1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        m_App = New App(m_User)
        GetAppIDFromQueryString(Me)
        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        lnkFahrzeugsuche.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugAuswahl.NavigateUrl = "Change01_2.aspx?AppID=" & Session("AppID").ToString
        lnkAdressAuswahl.NavigateUrl = "Change01_3.aspx?AppID=" & Session("AppID").ToString

        If IsPostBack = False Then
            If Session("AppChange") Is Nothing Then
                m_change = New Logistik.Logistik1(m_User, m_App, CStr(Request.QueryString("AppID")), Session.SessionID.ToString, "")
                Session("AppChange") = m_change
            Else
                m_change = CType(Session("AppChange"), Logistik.Logistik1)
            End If
            FillFields()
            FillGrid(0)
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView()
        tmpDataView = m_change.Daten.DefaultView
        tmpDataView.RowFilter = "Express = 'X' OR Standard = 'X'"
        If tmpDataView.Count = 0 Then
            gvResult.Visible = False
            lblMessage.Visible = True
            lblMessage.Text = "Keine Daten zur Anzeige gefunden."
        Else
            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            Else
                If Not ViewState("Sort") Is Nothing Then
                    strTempSort = ViewState("Sort").ToString
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If
            gvResult.PageIndex = intTempPageIndex
            gvResult.DataSource = tmpDataView
            gvResult.DataBind()

        End If
    End Sub

    Private Sub FillFields()
        Dim tmpRow() As DataRow
        tmpRow = m_change.Daten.Select("Express = 'X' OR Standard = 'X'")

        If tmpRow.Length > 0 Then
            lblAbholortShow.Text = m_change.Name & " " & m_change.Strasse & " " & m_change.Hausnummer & ", " & m_change.Postleitzahl & " " & m_change.Ort
            lblAnlieferortShow.Text = m_change.LiefName & " " & m_change.LiefStrasse & " " & m_change.LiefHausnummer & ", " & m_change.LiefPostleitzahl & " " & m_change.LiefOrt
            If tmpRow(0).Item("Standard").ToString = "X" Then
                lblAuftragsartShow.Text = "Logistikgutachten.LogExpert Standard - Aholung innerhalb von 3 Werktagen(Mo-Fr), bei Beauftragung vor 15 Uhr"
            ElseIf tmpRow(0).Item("Express").ToString = "X" Then
                lblAuftragsartShow.Text = "Logistikgutachten.LogExpert Express - Aholung innerhalb von 1 Werktag(Mo-Fr), bei Beauftragung vor 14 Uhr"
            End If
            If m_change.Entfernung.Length > 0 Then
                lblEntfernung.Text = "Entfernung ca. " & m_change.Entfernung & "km <br />"
                lblEntfernung.Text &= "Preis gemäß aktueller km-Staffel:"
            Else
                lblEntfernung.Visible = False
                lnkPreisStaffel.Visible = False
            End If

        End If

        Try
            Dim dirInfo = New DirectoryInfo("C:\Inetpub\wwwroot\" & Me.TemplateSourceDirectory & "\Dokumente\Ruecknahme\")
            Dim fileInfo = dirInfo.GetFiles("*.pdf")
            Dim FileName As String = ""

            Dim LiItem As New ListItem
            LiItem.Text = "- Bitte wählen -"
            LiItem.Value = "0"
            ddlStandards.Items.Clear()
            ddlStandards.Items.Add(LiItem)
            Dim iCount As Integer = 1

            For Each oFileInfo In fileInfo
                FileName = oFileInfo.Name
                With ddlStandards
                    LiItem = New ListItem
                    LiItem.Text = FileName
                    LiItem.Value = iCount.ToString
                    .Items.Add(LiItem)
                    iCount += 1
                End With
            Next
        Catch ex As Exception
            lblerror.Text = "Rücknamestandards-Dateien konnten nicht gelesen werden!"
        End Try


    End Sub

    Protected Sub chkAgb_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkAgb.CheckedChanged
        cmdSend.Enabled = chkAgb.Checked

    End Sub

    Protected Sub ddlStandards_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlStandards.SelectedIndexChanged
        If ddlStandards.SelectedIndex > 0 Then
            lnkStandards.Visible = True
            lnkStandards.Attributes.Add("onclick", "window.open('Dokumente/Ruecknahme/" & ddlStandards.SelectedItem.Text & "')")
        Else
            lnkStandards.Visible = False
        End If
    End Sub

    Protected Sub cmdSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSend.Click
        DoSubmit()
    End Sub
    Private Sub DoSubmit()
        If m_change Is Nothing Then
             m_change = CType(Session("AppChange"), Logistik.Logistik1)
        End If
        Dim tmpRow() As DataRow
        tmpRow = m_change.Daten.Select("Express = 'X' OR Standard = 'X'")

        For i = 0 To tmpRow.Length - 1
            m_change.Fahrgestellnr = tmpRow(0)("CHASSIS_NUM").ToString.Trim
            m_change.Kennzeichen = tmpRow(0)("LICENSE_NUM").ToString.Trim
            If tmpRow(0)("Express").ToString.Trim = "X" Then
                m_change.Express = True
            End If
            m_change.Change(Session("AppID").ToString, Session.SessionID, Me)
            If m_change.VBELN = "" Then
                lblerror.Text = "Der Vorgang konnte wegen eines Fehlers nicht beauftragt werden! <br>"
                trAGB.Visible = False
                trButton.Visible = False
            Else
                tmpRow(0)("Status") = "beauftragt"
                m_change.Daten.AcceptChanges()
                FillGrid(0)
                trAGB.Visible = False
                trButton.Visible = False
            End If


        Next i

        lnkFahrzeugAuswahl.Visible = False
        lnkAdressAuswahl.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class