Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change04_3
    Inherits System.Web.UI.Page
    Private m_User As Security.User
    Private m_App As Security.App
    Private objSuche As Search
    Private objAddressList As Search
    Private objHaendler As Porsche_05

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        Kopfdaten1.Message = ""
        lnkFahrzeugAuswahl.NavigateUrl = "Change04_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change04.aspx?AppID=" & Session("AppID").ToString

        GetAppIDFromQueryString(Me)

        idPreis1.Visible = False
        idPreis2.Visible = False

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            m_App = New Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
            End If

            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), Search)
            End If

            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br />" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br />" & objSuche.STREET

            Session("objSuche") = objSuche

            objHaendler = CType(Session("objHaendler"), Porsche_05)

            cmdSearch.Visible = False

            If Not IsPostBack Then
                Kopfdaten1.Kontingente = objHaendler.Kontingente

                Dim cmbItem As New System.Web.UI.WebControls.ListItem()
                cmbItem.Value = Kopfdaten1.HaendlerNummer
                cmbItem.Text = Kopfdaten1.HaendlerName & ", " & Replace(Kopfdaten1.Adresse, "<br />", ", ")
                cmbZweigstellen.Items.Add(cmbItem)

                objAddressList = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)

                Dim tmpIntValue As Int32 = objAddressList.LeseAdressenSAP(m_User.Reference)
                If tmpIntValue > 0 Then
                    Dim tmpRow As DataRow
                    For Each tmpRow In objAddressList.SearchResult.Rows
                        cmbItem = New System.Web.UI.WebControls.ListItem()
                        cmbItem.Value = tmpRow("ADDRESSNUMBER").ToString
                        cmbItem.Text = tmpRow("DISPLAY_ADDRESS").ToString
                        cmbZweigstellen.Items.Add(cmbItem)
                    Next
                End If

                cmbZweigstellen.SelectedIndex = 0

            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

    End Sub

    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
    '    DoSubmit()
    'End Sub

    Private Sub DoSubmit()
        Session("SelectedDeliveryValue") = cmbZweigstellen.SelectedItem.Value
        Session("SelectedDeliveryText") = cmbZweigstellen.SelectedItem.Text
        If chkVersandStandard.Checked Then
            Session("Materialnummer") = "1391"
        ElseIf chk0900.Checked Then
            Session("Materialnummer") = "1385"
        ElseIf chk1000.Checked Then
            Session("Materialnummer") = "1389"
        ElseIf chk1200.Checked Then
            Session("Materialnummer") = "1390"
        End If
        Response.Redirect("Change04_4.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class