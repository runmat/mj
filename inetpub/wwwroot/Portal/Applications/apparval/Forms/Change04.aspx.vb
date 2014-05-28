Imports CKG.Base.Kernel.Common.Common

Public Class Change04
    Inherits System.Web.UI.Page
    
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents ucHeader As CKG.Portal.PageElements.Header
    Protected WithEvents ucStyles As CKG.Portal.PageElements.Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        DoSubmit()
    End Sub

    Private Sub DoSubmit()

        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Try


            If Len(txtFahrgestellnummer.Text & txtKennzeichen.Text & txtVertragsnummer.Text) = 0 Then
                lblError.Text = "Es wurden keine Suchkriterien eingegeben."
                Return
            End If

            ClearVertragsdaten()

            Dim vertrag As New Vertragsdaten(m_User, m_App, "")

            lblError.Text = ""


            vertrag.Fahrgestellnummer = txtFahrgestellnummer.Text
            vertrag.Kennzeichen = txtKennzeichen.Text
            vertrag.Vertragsnummer = txtVertragsnummer.Text

            vertrag.Fill()

            If vertrag.Status = 0 Then

                If vertrag.VertragTable.Rows.Count > 0 Then


                    If Session("ArvalVertragsdaten") Is Nothing Then
                        Session.Add("ArvalVertragsdaten", vertrag.VertragTable)
                    Else
                        Session("ArvalVertragsdaten") = vertrag.VertragTable
                    End If

                    txtFahrgestellnummer.Text = ""
                    txtKennzeichen.Text = ""
                    txtVertragsnummer.Text = ""

                    cmdSave.Visible = True
                    
                    FillVertragsdaten(vertrag.VertragTable)
                Else
                    ResetPage()
                    
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Return
                End If

                If vertrag.Adressen.Rows.Count > 0 Then

                    If Session("LnAdressen") Is Nothing Then
                        Dim arrAdressen() As String
                        Dim i As Integer = 0

                        For Each row As DataRow In vertrag.Adressen.Rows

                            ReDim Preserve arrAdressen(i)

                            arrAdressen(i) = row("NAME1").ToString
                            i += 1
                        Next

                        Session.Add("LnAdressen", arrAdressen)
                        Session.Add("ArvalAdressen", vertrag.Adressen)
                    End If
                End If
            Else
                lblError.Text = "Fehler: " & vertrag.Message

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

    Protected Sub txtLeasingnehmer_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtLeasingnehmer.TextChanged

        If txtLeasingnehmer.Text.Length = 0 OrElse txtLeasingnehmer.Text = "Geben Sie mindestens 3 Zeichen ein um eine Auswahlliste zu erhalten." Then Return

        If Session("LnAdressen") Is Nothing Then Return

        Dim adressliste() As String = CType(Session("LnAdressen"), String())

        Dim i As Integer = Array.IndexOf(adressliste, txtLeasingnehmer.Text)

        If i = -1 Then
            lblError.Text = "Ungültige Auswahl des Leasingnehmers."

            txtLeasingnehmer.Text = ""
        End If

    End Sub

    Private Sub FillVertragsdaten(ByVal TempTable As DataTable)

        lblVetragsnummer.Text = TempTable.Rows(0)("LIZNR")
        lblFahrgestellnummer.Text = TempTable.Rows(0)("CHASSIS_NUM")
        lblKennzeichen.Text = TempTable.Rows(0)("LICENSE_NUM")
        lblHalterName1.Text = TempTable.Rows(0)("NAME1_ZH")
        lblHalterOrt.Text = TempTable.Rows(0)("CITY1_ZH")
        lblName1.Text = TempTable.Rows(0)("NAME1_ZL")
        lblName2.Text = TempTable.Rows(0)("NAME2_ZL")
        lblStrasse.Text = TempTable.Rows(0)("STREET_ZL")
        lblPlzOrt.Text = TempTable.Rows(0)("POST_CODE1_ZL") & " " & TempTable.Rows(0)("CITY1_ZL")

        tblAdressenAnzeige.Visible = True
        tblEdit.Visible = True
        
    End Sub
    
    Private Sub ClearVertragsdaten()

        txtNewVertragsnummer.Text = ""
        txtLeasingnehmer.Text = ""
        lblVetragsnummer.Text = ""
        lblFahrgestellnummer.Text = ""
        lblKennzeichen.Text = ""
        lblHalterName1.Text = ""
        lblHalterOrt.Text = ""
        lblName1.Text = ""
        lblName2.Text = ""
        lblStrasse.Text = ""
        lblPlzOrt.Text = ""

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click

        Dim booError As Boolean = False
        lblError.Text = ""

        Dim vertrag As New Vertragsdaten(m_User, m_App, "")
        Dim tempTable As DataTable = CType(Session("ArvalVertragsdaten"), DataTable)


        vertrag.Equinr = tempTable.Rows(0)("EQUNR").ToString


        If txtNewVertragsnummer.Text.Length > 0 Then
            If IsNumeric(txtNewVertragsnummer.Text) = False Then
                lblError.Text = "Bitte geben Sie eine numerische Vertragsnummer ein."
                booError = True
            Else
                vertrag.Vertragsnummer = txtNewVertragsnummer.Text
            End If
        Else
            vertrag.Vertragsnummer = tempTable.Rows(0)("LIZNR").ToString
        End If
        
        If txtLeasingnehmer.Text.Length > 0 Then
            vertrag.KunnrZL = Mid(txtLeasingnehmer.Text, txtLeasingnehmer.Text.IndexOf("~") + 3)
        Else
            vertrag.KunnrZL = tempTable.Rows(0)("KUNNR_ZL").ToString
        End If

        If booError = False Then

            vertrag.Change()

            If vertrag.Status = 0 Then
                lblInfo.Visible = True
                lblInfo.Text = "Die Daten wurden gespeichert."
                ClearVertragsdaten()
                ResetPage()
            Else
                lblError.Text = "Fehler: " & vertrag.Message
            End If

        End If

    End Sub

    Private Sub ResetPage()

        cmdSave.Visible = False
        tblAdressenAnzeige.Visible = False
        tblEdit.Visible = False

        Session("ArvalVertragsdaten") = Nothing
        Session("LnAdressen") = Nothing
        Session("ArvalAdressen") = Nothing

    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

End Class

' ************************************************
' $History: Change04.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 21.01.10   Time: 12:51
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 20.01.10   Time: 16:34
' Updated in $/CKAG/Applications/apparval/Forms
' ITA: 3339
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 19.01.10   Time: 17:19
' Created in $/CKAG/Applications/apparval/Forms
'