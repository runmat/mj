Imports CKG.Base.Kernel.Security
'Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Public Class Change05s
    Inherits System.Web.UI.Page
    Private m_App As App
    Private m_User As User

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        m_App = New App(m_User)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        If Not IsPostBack Then
            InitialForm()
            If Not Request.QueryString.Item("Ident") Is Nothing Then
                SetAdress()
            End If
        End If

    End Sub


    Private Sub InitialForm()
        Dim sprache As String

        Dim bv As Briefversand = New Briefversand(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        'Länder DLL füllen
        ddlLand.DataSource = bv.Laender

        ddlLand.DataTextField = "FullDesc"
        ddlLand.DataValueField = "Land1"
        ddlLand.DataBind()
        'vorbelegung der Länderddl auf Grund der im Browser eingestellten erstsprache JJ2007.12.06
        Dim tmpstr() As String
        If Request("HTTP_ACCEPT_Language").IndexOf(",") = -1 Then
            'es ist nur eine sprache ausgewählt
            sprache = Request("HTTP_ACCEPT_Language")
        Else
            'es gibt eine erst und eine zweitsprache
            sprache = Request("HTTP_ACCEPT_Language").Substring(0, Request("HTTP_ACCEPT_Language").IndexOf(","))
        End If

        tmpstr = sprache.Split(CChar("-"))
        'Länderkennzeichen setzen sich aus Region und Sprache zusammen. de-ch, de-at usw. leider werden bei Regionen in denen die Sprache das selbe Kürzel hat nur einfache Kürzel geschrieben, z.b. bei "de"
        If tmpstr.Length > 1 Then
            sprache = tmpstr(1).ToUpper
        Else
            sprache = tmpstr(0).ToUpper
        End If
        ddlLand.Items.FindByValue(sprache).Selected = True


        Dim AdressPool As Adresspflege = New Adresspflege(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        AdressPool.GetKennung(Me.Page)

        If AdressPool.TableKennung.Rows.Count > 0 Then
            ddlKennung.DataSource = AdressPool.TableKennung
            ddlKennung.DataTextField = "KEY_ADR_POOL"
            ddlKennung.DataValueField = "ANWENDUNG"
            ddlKennung.DataBind()

        End If

        AdressPool = Nothing

        lblKunnrShow.Text = m_User.KUNNR

    End Sub


    Protected Sub ibtSuchen_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtSuchen.Click


        Session("Adressen") = Nothing

        Dim SearchString As String = ""

        SearchString = txtReferenz.Text &
                       txtName1.Text &
                       txtName2.Text &
                       txtStrasse.Text &
                       txtPLZ.Text &
                       txtOrt.Text

        If SearchString.Replace("*", "").Length > 0 Then




            Dim AdressPool As Adresspflege = New Adresspflege(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

            AdressPool.Referenz = txtReferenz.Text
            AdressPool.Name1 = txtName1.Text
            AdressPool.Name2 = txtName2.Text
            AdressPool.Strasse = txtStrasse.Text
            AdressPool.PLZ = txtPLZ.Text
            AdressPool.Ort = txtOrt.Text

            AdressPool.EqTyp = ddlKennung.SelectedValue

            AdressPool.GetAdressenandZulStellen(Me.Page)

            If AdressPool.TableAdressen.Rows.Count = 0 Then
                lblError.Text = "Es wurden keine Adressen gefunden."
            ElseIf AdressPool.TableAdressen.Rows.Count > 50 Then
                lblError.Text = "Es wurden mehr als 50 Adressen gefunden. Bitte schränken Sie Ihre Suche ein."
            Else
                Dim tmpItem As ListItem
                Dim dv As New DataView
                dv = AdressPool.TableAdressen.DefaultView
                dv.Sort = "NAME1 asc"



                Dim i As Int32 = 0
                ddlAdresse.Items.Clear()
                Do While i < dv.Count
                    tmpItem = New ListItem(dv.Item(i)("NAME1").ToString & " " & dv.Item(i)("NAME2").ToString & " - " & dv.Item(i)("STREET").ToString & ", " & dv.Item(i)("CITY1").ToString, dv.Item(i)("IDENT").ToString)
                    ddlAdresse.Items.Add(tmpItem)
                    i += 1
                Loop
                tmpItem = New ListItem("- bitte auswählen -", "0000")
                ddlAdresse.Items.Insert(0, tmpItem)

                ddlAdresse.Enabled = True

                chkLoeschkennzeichen.Enabled = True

                Session("Adressen") = AdressPool.TableAdressen

                ibtSave.Visible = True


            End If


        Else
            lblError.Text = "Bitte geben Sie mindestens 1 Suchkriterium an."
        End If




    End Sub

    Protected Sub ddlAdresse_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAdresse.SelectedIndexChanged


        If ddlAdresse.SelectedValue = "0000" Then Exit Sub

        Dim AdrRow As DataRow = CType(Session("Adressen"), DataTable).Select("IDENT = '" + ddlAdresse.SelectedValue + "'")(0)

        txtReferenz.Text = ConvertNull(AdrRow("POS_TEXT"))
        txtName1.Text = ConvertNull(AdrRow("NAME1"))
        txtName2.Text = ConvertNull(AdrRow("NAME2"))
        txtStrasse.Text = ConvertNull(AdrRow("STREET"))
        txtPLZ.Text = ConvertNull(AdrRow("POST_CODE1"))
        txtOrt.Text = ConvertNull(AdrRow("CITY1"))
        ddlLand.SelectedValue = ConvertNull(AdrRow("COUNTRY"))
        txtTelefon.Text = ConvertNull(AdrRow("TELEFON"))
        txtMail.Text = ConvertNull(AdrRow("EMAIL"))
        txtFax.Text = ConvertNull(AdrRow("FAX"))

    End Sub

    Private Function ConvertNull(str As Object) As String

        If IsDBNull(str) Then
            str = ""
        End If

        Return str.ToString

    End Function

    Private Sub ClearFields()
        txtReferenz.Text = ""
        txtName1.Text = ""
        txtName2.Text = ""
        txtStrasse.Text = ""
        txtPLZ.Text = ""
        txtOrt.Text = ""
        ddlLand.SelectedValue = "DE"
        ddlAdresse.Items.Clear()
        ddlAdresse.Enabled = False
        txtTelefon.Text = ""
        txtMail.Text = ""
        txtFax.Text = ""
        chkLoeschkennzeichen.Checked = False
    End Sub

    Private Sub SetErrorFrame(ByVal ctrl As Control)

        Dim txt As TextBox
        Dim rdb As RadioButtonList

        If TypeOf ctrl Is TextBox Then
            txt = ctrl
            'txt.BorderWidth = 1
            txt.BorderColor = Drawing.Color.Red
        ElseIf TypeOf ctrl Is RadioButtonList Then
            rdb = ctrl
            rdb.BorderStyle = BorderStyle.Solid
            rdb.BorderWidth = 1
            rdb.BorderColor = Drawing.Color.Red
        End If

    End Sub

    Private Sub ResetErrorFrame()

        txtName1.BorderColor = Drawing.Color.Empty
        txtStrasse.BorderColor = Drawing.Color.Empty
        txtPLZ.BorderColor = Drawing.Color.Empty
        txtOrt.BorderColor = Drawing.Color.Empty

    End Sub


    Protected Sub ibtSave_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtSave.Click

        Dim booError As Boolean = False

        If txtName1.Text.Length = 0 Then
            SetErrorFrame(txtName1) : booError = True
        End If

        If txtStrasse.Text.Length = 0 Then
            SetErrorFrame(txtStrasse) : booError = True
        End If

        If txtPLZ.Text.Length = 0 Then
            SetErrorFrame(txtPLZ) : booError = True
        End If

        If txtOrt.Text.Length = 0 Then
            SetErrorFrame(txtOrt) : booError = True
        End If


        Dim AdressPool As Adresspflege = New Adresspflege(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        AdressPool.Referenz = txtReferenz.Text
        AdressPool.Name1 = txtName1.Text
        AdressPool.Name2 = txtName2.Text
        AdressPool.Strasse = txtStrasse.Text
        AdressPool.PLZ = txtPLZ.Text
        AdressPool.Ort = txtOrt.Text
        AdressPool.Land = ddlLand.SelectedValue
        AdressPool.Telefon = txtTelefon.Text
        AdressPool.Mail = txtMail.Text
        AdressPool.Fax = txtFax.Text


        If ddlAdresse.Enabled = True Then
            If ddlAdresse.SelectedValue <> "00000" Then
                AdressPool.Ident = ddlAdresse.SelectedValue

                If chkLoeschkennzeichen.Checked = True Then
                    AdressPool.Verarbeitungskennzeichen = "D"
                Else
                    AdressPool.Verarbeitungskennzeichen = "U"
                End If

            Else
                lblError.Text = "Bitte suchen Sie zunächst eine Adresse, die Sie ändern wollen."
                AdressPool = Nothing
                Exit Sub
            End If
        Else
            AdressPool.Verarbeitungskennzeichen = "N"
        End If


        If booError = True Then
            lblError.Text = "Bitte füllen Sie die Pflichtfelder aus."
            AdressPool = Nothing
            Exit Sub
        End If

        AdressPool.Kennung = ddlKennung.SelectedItem.Text

        If Not Request.QueryString.Item("Ident") Is Nothing Then
            AdressPool.Verarbeitungskennzeichen = "U"
            AdressPool.Ident = Request.QueryString.Item("Ident").ToString
        End If

        AdressPool.ChangeNew(Me.Page)

        If AdressPool.Status <> 0 Then
            lblError.Text = AdressPool.Message
        Else

            If Request.QueryString.Item("Ident") Is Nothing Then
                lblMessage.Text = "Die Adresse wurde gespeichert."
                ibtSave.Visible = False
                ibtNeuanlage.Visible = True
                ibtSuchen.Visible = True
            Else


                Response.Redirect(Session("ReturnURL") & "&mode=success")
            End If

            
        End If

       
        

    End Sub

    Protected Sub ibtCancel_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtCancel.Click

        'If Session("ReturnURL") Is Nothing Then
        If Request.QueryString.Item("ident") Is Nothing Then
            Session("ReturnURL") = Nothing
            Session("me") = Nothing
            ClearFields()
            ResetErrorFrame()
            ibtSuchen.Visible = True
            ibtNeuanlage.Visible = True
            ibtSave.Visible = True
        Else
            Response.Redirect(Session("ReturnURL") & "&mode=cancel")
        End If

    End Sub

    Protected Sub ibtNeuanlage_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles ibtNeuanlage.Click
        ClearFields()
        ResetErrorFrame()
        chkLoeschkennzeichen.Enabled = False
        ibtNeuanlage.Visible = False
        ibtSuchen.Visible = False
        ibtSave.Visible = True
    End Sub

    Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub SetAdress()

        Session("ReturnURL") = Me.Context.Request.UrlReferrer.ToString
        Session("ReturnURL") = Session("ReturnURL").ToString.Replace("&mode=cancel", "")
        Session("ReturnURL") = Session("ReturnURL").ToString.Replace("&mode=success", "")




        lbBack.Text = " "
        lbBack.Enabled = False

        ibtNeuanlage.Visible = False
        ibtSuchen.Visible = False
        ddlKennung.Enabled = False

        Dim Name As String
        Dim Ident As String
        Dim eqtyp As String
        Name = Request.QueryString.Item("name").ToString
        Ident = Request.QueryString.Item("ident").ToString
        eqtyp = Request.QueryString.Item("eqtyp").ToString

        Dim AdressPool As Adresspflege = New Adresspflege(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")

        AdressPool.Name1 = Name
        AdressPool.EqTyp = eqtyp

        AdressPool.GetAdressenandZulStellen(Me.Page)


        Dim AdrRow As DataRow = AdressPool.TableAdressen.Select("IDENT = '" + Ident + "'")(0)

        ddlKennung.SelectedValue = eqtyp
        txtReferenz.Text = ConvertNull(AdrRow("POS_TEXT"))
        txtName1.Text = ConvertNull(AdrRow("NAME1"))
        txtName2.Text = ConvertNull(AdrRow("NAME2"))
        txtStrasse.Text = ConvertNull(AdrRow("STREET"))
        txtPLZ.Text = ConvertNull(AdrRow("POST_CODE1"))
        txtOrt.Text = ConvertNull(AdrRow("CITY1"))
        ddlLand.SelectedValue = ConvertNull(AdrRow("COUNTRY"))
        txtTelefon.Text = ConvertNull(AdrRow("TELEFON"))
        txtMail.Text = ConvertNull(AdrRow("EMAIL"))
        txtFax.Text = ConvertNull(AdrRow("FAX"))


    End Sub

    Protected Sub ddlKennung_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlKennung.SelectedIndexChanged
        ClearFields()
        ResetErrorFrame()
        ibtSuchen.Visible = True
        ibtNeuanlage.Visible = True
        ibtSave.Visible = True
    End Sub
End Class