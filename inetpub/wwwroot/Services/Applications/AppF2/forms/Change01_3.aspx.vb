Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Partial Public Class Change01_3
    Inherits System.Web.UI.Page

    Private objSuche As AppF2.Search
    Private objAddressList As AppF2.Search
    Private objHaendler As Haendler
    Private mstrHDL As String


#Region "Declarations"
    Private m_App As App
    Private m_User As User
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        m_App = New App(m_User)

        lnkFahrzeugAuswahl.NavigateUrl = "Change01_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        mstrHDL = ""

        If CType(Session("AppShowNot"), Boolean) = True Then
            tr_Adresse.Visible = False
        End If

        If Session("AppHaendler") Is Nothing Then
            Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
        End If

        If (Session("objSuche") Is Nothing) Then
            Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
        Else
            objSuche = CType(Session("objSuche"), AppF2.Search)
        End If

        If objHaendler Is Nothing Then
            objHaendler = CType(Session("AppHaendler"), Haendler)
        End If



        If Not IsPostBack Then

            If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, objHaendler.Customer, Me) Then
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            End If

            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.STREET & "<br>" & objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY
            objHaendler.Show(Session("AppID").ToString, Session.SessionID, Me) 'kontingente füllen
            Kopfdaten1.Kontingente = objHaendler.Kontingente

            Dim cmbItem As New System.Web.UI.WebControls.ListItem()
            cmbItem.Value = objSuche.REFERENZ
            cmbItem.Text = strTemp & ", " & objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & ",  " & objSuche.STREET
            ddlPartneradressen.Items.Add(cmbItem)


            objSuche.zeigeAlle = True
            Dim tmpIntValue As Int32 = objSuche.LeseHaendlerForSucheHaendlerControl(Session("AppID").ToString, Session.SessionID.ToString, Me.Page)
            If tmpIntValue = 0 Then
                Dim tmpRow As DataRow
                For Each tmpRow In objSuche.Haendler.Table.Rows
                    cmbItem = New System.Web.UI.WebControls.ListItem()
                    cmbItem.Value = tmpRow("REFERENZ").ToString
                    cmbItem.Text = tmpRow("REFERENZ").ToString
                    ddlPartneradressen.Items.Add(cmbItem)
                Next

            End If
            lblSuche.Visible = True
            tblSuche.Visible = True
            lblSuche.Text = ""
            ddlPartneradressen.SelectedIndex = 0
            ddlPartneradressen.Visible = False
            Session("objSuche") = objSuche

            fillLaenderDLL()
            getZulStellen()
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region


    Private Sub fillLaenderDLL()
        Dim sprache As String
        'Länder DLL füllen
        ddlLand.DataSource = objHaendler.Laender
        'ddlLand.DataTextField = "Beschreibung"
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

    End Sub

    Private Sub DoSubmit()
        Dim strAdresse As String = ""
        objHaendler.kbanr = ""
        lblError.Text = ""
        If rb_Haendler.Checked Then
            Session("SelectedDeliveryValue") = ddlPartneradressen.SelectedItem.Value
            Session("SelectedDeliveryText") = ddlPartneradressen.SelectedItem.Text
            objHaendler.neueAdresse = False
            objHaendler.FlagVersand = "1" ' Händler
        ElseIf rb_BCA.Checked Then

            objHaendler.kbanr = ddlGeschaeftsstelle.SelectedItem.Value
            Session("SelectedDeliveryText") = "Zulassungsstelle " & ddlGeschaeftsstelle.SelectedItem.Text
            Session("SelectedDeliveryValue") = objSuche.REFERENZ
            objHaendler.neueAdresse = True
            objHaendler.FlagVersand = "4" ' Zulassungsstellen
        Else
            objHaendler.FlagVersand = "2" ' freie Adresse
            objHaendler.neueAdresse = True
            If txtName1.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Name"" eingeben.<br>&nbsp;"
            Else
                objHaendler.Name1 = txtName1.Text.Trim(" "c) & ", "
                strAdresse = txtName1.Text.Trim(" "c) & ", "
            End If
            If txtPostleitzahl.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""PLZ"" eingeben.<br>&nbsp;"
            Else
                If CInt(objHaendler.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                    If Not CInt(objHaendler.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) = txtPostleitzahl.Text.Trim(" "c).Length Then
                        lblError.Text = "Postleitzahl hat falsche Länge."
                    Else
                        objHaendler.PostCode = txtPostleitzahl.Text.Trim(" "c) & " "
                        strAdresse = strAdresse & ddlLand.SelectedItem.Value & "-" & txtPostleitzahl.Text.Trim(" "c) & " "
                    End If
                End If

            End If
            If txtOrt.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Ort"" eingeben.<br>&nbsp;"
            Else
                objHaendler.City = txtOrt.Text.Trim(" "c)
                strAdresse = strAdresse & txtOrt.Text.Trim(" "c) & ", "
            End If
            If txtStrasse.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Strasse"" eingeben.<br>&nbsp;"
            Else
                objHaendler.Street = txtStrasse.Text.Trim(" "c)
                strAdresse = strAdresse & txtStrasse.Text.Trim(" "c) & " "
            End If
            If txtHausnummer.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Nummer"" eingeben.<br>&nbsp;"
            Else
                objHaendler.HouseNum = txtHausnummer.Text.Trim(" "c)
                strAdresse = strAdresse & txtHausnummer.Text.Trim(" "c)
            End If
            objHaendler.Name2 = txtName2.Text.Trim(" "c)
            objHaendler.laenderKuerzel = ddlLand.SelectedItem.Value
            Session("SelectedDeliveryValue") = objSuche.REFERENZ
            Session("SelectedDeliveryText") = strAdresse
        End If

        If rb_VersandStandard.Checked And rb_VersandStandard.Visible = True Then
            Session("Materialnummer") = "1391"
            'nix wird hard codiert als standard auf der folgeseite angezeigt
        ElseIf rb_0900.Checked And rb_0900.Visible = True Then
            Session("Materialnummer") = "1385"
            objHaendler.VersandArtText = rb_0900.Text & " " & lbl_0900.Text
        ElseIf rb_1000.Checked And rb_1000.Visible = True Then
            Session("Materialnummer") = "1389"
            objHaendler.VersandArtText = rb_1000.Text & " " & lbl_1000.Text
        ElseIf rb_1200.Checked And rb_1200.Visible = True Then
            Session("Materialnummer") = "1390"
            objHaendler.VersandArtText = rb_1200.Text & " " & lbl_1200.Text
        ElseIf rb_SendungsVerfolgt.Checked And rb_SendungsVerfolgt.Visible = True Then
            Session("Materialnummer") = "5530"
            objHaendler.VersandArtText = rb_SendungsVerfolgt.Text & " " & lbl_SendungsVerfolgt.Text
        End If

        If Not lblError.Text.Length > 0 Then
            Session("AppHaendler") = objHaendler
            Response.Redirect("Change01_4.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub



    Protected Sub rb_freieAdresse_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_freieAdresse.CheckedChanged
        If rb_freieAdresse.Checked = True Then
            ddlPartneradressen.Visible = False
            tr_Partneradresse.Visible = False
            tr_Adresse.Visible = True
            tr_ZulStelle.Visible = False
        End If
    End Sub

    Protected Sub rb_Haendler_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Haendler.CheckedChanged
        If rb_Haendler.Checked = True Then
            ddlPartneradressen.Visible = False
            tr_Partneradresse.Visible = True
            tr_Adresse.Visible = False
            tr_ZulStelle.Visible = False
        End If
    End Sub

    Protected Sub rb_BCA_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_BCA.CheckedChanged
        If rb_BCA.Checked = True Then
            ddlPartneradressen.Visible = False
            tr_Partneradresse.Visible = False
            ddlGeschaeftsstelle.Visible = False
            tr_ZulStelle.Visible = True
            tr_Adresse.Visible = False
            tr_Adresse.Visible = False
        End If
    End Sub

    Protected Sub lb_Suche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Suche.Click
        Dim tmpItem As ListItem
        If Not (Session("objSuche") Is Nothing) Then
            objSuche = CType(Session("objSuche"), AppF2.Search)

            If txtOrtSuche.Text.Length + txtPostleitzahlSuche.Text.Length + txtNameSuche.Text.Length + txtHaendlerNr.Text.Length > 0 Then

                Dim sQuery As String = ""

                If txtNameSuche.Text.Length > 0 Then
                    sQuery += "NAME LIKE '" & txtNameSuche.Text.Trim & "' AND "
                End If

                If txtHaendlerNr.Text.Length > 0 Then
                    sQuery += "REFERENZ LIKE '" & txtHaendlerNr.Text.Trim & "' AND "
                End If

                If txtOrtSuche.Text.Length > 0 Then
                    sQuery += "CITY LIKE '" & txtOrtSuche.Text.Trim & "' AND "
                End If

                If txtPostleitzahlSuche.Text.Length > 0 Then
                    sQuery += "POSTL_CODE LIKE '" & txtPostleitzahlSuche.Text.Trim & "' AND "
                End If

                sQuery = Left(sQuery, sQuery.Length - 4)


                Dim dv As New DataView
                dv = objSuche.Haendler
                dv.RowFilter = sQuery
                dv.Sort = "NAME asc"

                If dv.Count > 0 Then
                    Dim i As Int32 = 0
                    ddlPartneradressen.Items.Clear()
                    Do While i < dv.Count
                        tmpItem = New ListItem(dv.Item(i)("NAME").ToString & " " & dv.Item(i)("NAME_2").ToString & ", " & dv.Item(i)("POSTL_CODE").ToString & " " & dv.Item(i)("CITY").ToString, dv.Item(i)("REFERENZ").ToString)
                        ddlPartneradressen.Items.Add(tmpItem)
                        i += 1
                    Loop
                    tmpItem = New ListItem("- bitte auswählen -", "")
                    ddlPartneradressen.Items.Insert(0, tmpItem)


                    If dv.Count = 1 Then ddlPartneradressen.SelectedIndex = 1

                    If ddlPartneradressen.Items.Count > 20 Then

                        ddlPartneradressen.Visible = False
                        lblSuche.Visible = True
                        tblSuche.Visible = True
                        lblSuche.Text = "Bitte über die Suchkriterien genauer eingrenzen!"

                    Else
                        ddlPartneradressen.Visible = True
                        lb_NeuSuche.Visible = True
                        lblSuche.Visible = False
                        tblSuche.Visible = False
                    End If
                Else
                    dv.RowFilter = ""
                    lblSuche.ForeColor = Drawing.Color.Red
                    lblSuche.Text = "Kein Händler gefunden!"
                    lblSuche.Visible = True
                End If


            Else
                txtNameSuche.BorderColor = Drawing.Color.Red
                txtOrtSuche.BorderColor = Drawing.Color.Red
                txtHaendlerNr.BorderColor = Drawing.Color.Red
                txtPostleitzahlSuche.BorderColor = Drawing.Color.Red
                lblSuche.ForeColor = Drawing.Color.Red
                lblSuche.Text = "Kein Suchkriterium gefüllt!"
            End If

        End If
    End Sub


    Protected Sub lb_NeuSuche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_NeuSuche.Click
        ddlPartneradressen.Items.Clear()
        ddlPartneradressen.Visible = False
        tr_Partneradresse.Visible = True
        lblSuche.Visible = True
        tblSuche.Visible = True
        tr_Adresse.Visible = False
        tr_ZulStelle.Visible = False
        lb_NeuSuche.Visible = False
    End Sub


    Private Sub getZulStellen()
        Dim status As String = ""
        Dim view As DataView
        Dim row As DataRow
        Dim tblTemp As DataTable
        Dim newRow As DataRow
        Dim counter As Integer

        'Zulassungsdienste holen
        objHaendler.getZulStelle(Me.Page, "", "", status)

        tblTemp = objHaendler.ZulStellen.Clone
        tblTemp.Columns.Add("DISPLAY")

        For Each row In objHaendler.ZulStellen.Rows
            If row("ORT01").ToString <> String.Empty And row("LIFNR").ToString <> String.Empty Then
                Dim intTemp As Integer = InStr(row("ORT01").ToString, "ZLS")
                Dim intTemp2 As Integer = InStr(row("ORT01").ToString, "geschl")
                If Not (intTemp > 0 AndAlso intTemp2 > intTemp) Then
                    newRow = tblTemp.NewRow
                    newRow("DISPLAY") = row("PSTLZ").ToString & " - " & row("ORT01").ToString & " - " & row("STRAS").ToString
                    newRow("LIFNR") = row("LIFNR").ToString
                    newRow("ORT01") = row("ORT01").ToString
                    newRow("STRAS") = row("STRAS").ToString
                    newRow("PSTLZ") = row("PSTLZ").ToString
                    newRow("PSTL2") = CType(counter, String) 'Lfd. Nr. (für spätere Suche!)
                    newRow("KBANR") = row("KBANR").ToString 'ZKFZKZ
                    newRow("ZKFZKZ") = row("ZKFZKZ").ToString
                    tblTemp.Rows.Add(newRow)
                    tblTemp.AcceptChanges()
                    counter += 1
                End If
            End If
        Next

        objHaendler.ZulStellen = tblTemp.Copy
        objHaendler.ZulStellen.AcceptChanges()
        tblTemp = Nothing

        Session.Add("ZulDienste", objHaendler.ZulStellen)

        view = objHaendler.ZulStellen.DefaultView
        view.Sort = "DISPLAY"
        With ddlGeschaeftsstelle
            .DataSource = view
            .DataTextField = "DISPLAY"
            .DataValueField = "KBANR"
            .DataBind()
        End With

    End Sub

    Protected Sub lbt_SucheGeschNeu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbt_SucheGeschNeu.Click
        ddlGeschaeftsstelle.Items.Clear()
        ddlGeschaeftsstelle.Visible = False
        tr_Partneradresse.Visible = False
        Table1.Visible = True
        tr_Adresse.Visible = False
        tr_ZulStelle.Visible = True
        lbt_SucheGeschNeu.Visible = False
    End Sub

    Protected Sub lb_SucheGesch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_SucheGesch.Click
        If Not (Session("ZulDienste") Is Nothing) Then
            Dim tblZul As DataTable = Session("ZulDienste")

            If txtOrtSucheGe.Text.Length + txtPLZSucheGe.Text.Length + txtKennzKreis.Text.Length > 0 Then

                Dim sQuery As String = ""

                If txtOrtSucheGe.Text.Length > 0 Then
                    sQuery += "ORT01 LIKE '" & txtOrtSucheGe.Text.Trim & "' AND "
                End If

                If txtPLZSucheGe.Text.Length > 0 Then
                    sQuery += "PSTLZ LIKE '" & txtPLZSucheGe.Text.Trim & "' AND "
                End If

                If txtKennzKreis.Text.Length > 0 Then
                    sQuery += "ZKFZKZ LIKE '" & txtKennzKreis.Text.Trim & "' AND "
                End If

                sQuery = Left(sQuery, sQuery.Length - 4)


                Dim dv As New DataView(tblZul)
                dv.RowFilter = sQuery
                dv.Sort = "DISPLAY asc"
                Dim i As Int32 = 0
                ddlPartneradressen.Items.Clear()

                dv.Sort = "DISPLAY"
                With ddlGeschaeftsstelle
                    .DataSource = dv
                    .DataTextField = "DISPLAY"
                    .DataValueField = "KBANR"
                    .DataBind()
                End With

                If dv.Count > 0 Then
                    If dv.Count = 1 Then ddlGeschaeftsstelle.SelectedIndex = 0

                    If ddlGeschaeftsstelle.Items.Count > 20 Then

                        ddlGeschaeftsstelle.Visible = False
                        lblSucheGeschaeft.Visible = True
                        Table1.Visible = True
                        lblSucheGeschaeft.Text = "Bitte über die Suchkriterien genauer eingrenzen!"

                    Else
                        ddlGeschaeftsstelle.Visible = True
                        lbt_SucheGeschNeu.Visible = True
                        lblSucheGeschaeft.Visible = False
                        lblSucheGeschaeft.Visible = False
                    End If
                Else
                    lblSucheGeschaeft.ForeColor = Drawing.Color.Red
                    lblSucheGeschaeft.Text = "Keine Zulassungsstelle gefunden!"
                    lblSucheGeschaeft.Visible = True
                End If


            Else
                txtOrtSucheGe.BorderColor = Drawing.Color.Red
                txtPLZSucheGe.BorderColor = Drawing.Color.Red
                lblSucheGeschaeft.ForeColor = Drawing.Color.Red
                lblSucheGeschaeft.Text = "Kein Suchkriterium gefüllt!"
            End If

        End If
    End Sub
End Class
