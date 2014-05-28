Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG


Public Class Change07_3
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As Search
    Private objAddressList As Search
    Private objHaendler As F1_Haendler

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents Kopfdaten1 As Kopfdaten

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Kopfdaten1.Message = ""
        lnkFahrzeugAuswahl.NavigateUrl = "Change07_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change07_1.aspx?AppID=" & Session("AppID").ToString

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change07_1.aspx?AppID=" & Session("AppID").ToString)
            End If

            If (Session("objSuche") Is Nothing) Then
                Response.Redirect("Change07_1.aspx?AppID=" & Session("AppID").ToString)
            Else
                objSuche = CType(Session("objSuche"), Search)
            End If

            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
            Kopfdaten1.SapInterneHaendlerNummer = objSuche.SapInterneHaendlerReferenzNummer
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
            Kopfdaten1.Kontingente = objSuche.Kontingente
            Session("objSuche") = objSuche

            objHaendler = CType(Session("objHaendler"), F1_Haendler)

            Dim rowsTemp() As DataRow = objHaendler.Fahrzeuge.Select("MANDT='4'")
            If rowsTemp.GetLength(0) > 0 Then
                ZeigeTEXT50.Visible = True
                RequiredFieldValidator1.Enabled = True
                If IsPostBack Then
                    Dim r As Integer
                    For r = 0 To rowsTemp.GetLength(0) - 1
                        rowsTemp(r)("TEXT50") = txtTEXT50.Text
                    Next
                End If
            Else
                ZeigeTEXT50.Visible = False
                RequiredFieldValidator1.Enabled = False
            End If


            If Not IsPostBack Then
                getZulStellen()
                fillLaenderDLL()
                setVersandadressenberechtigung()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        DoSubmit()
    End Sub

    Private Sub setVersandadressenberechtigung()

        Dim tmpRows() As DataRow
        tmpRows = objHaendler.Fahrzeuge.Select("MANDT <> 0")

        Dim arrRow As DataRow
        Dim iEingeschraenkt As Integer = 0
        Dim iBerechtigung As Integer = 0
        Dim iAutRight As Integer = m_User.Groups(0).Authorizationright

        For Each arrRow In tmpRows
            Dim sAugru As String
            sAugru = CType(arrRow("AUGRU"), String)
            Dim tmpEingeschraenkt As Integer
            tmpEingeschraenkt = objHaendler.GeteingeschraenkteAbrufgruende(sAugru)
            If iEingeschraenkt = 0 AndAlso Not tmpEingeschraenkt = 0 Then
                iEingeschraenkt = tmpEingeschraenkt
            ElseIf iEingeschraenkt > 0 AndAlso Not tmpEingeschraenkt = 0 Then
                If iEingeschraenkt > tmpEingeschraenkt Then
                    iEingeschraenkt = tmpEingeschraenkt
                End If
            End If
        Next

        If iEingeschraenkt <> 0 Then
            If iAutRight < iEingeschraenkt Then
                iBerechtigung = iAutRight
            Else
                iBerechtigung = iEingeschraenkt
            End If

        Else
            iBerechtigung = iAutRight
        End If

        Select Case iBerechtigung
            Case 0
                trZeigeManuelleAdresse.Visible = False
                trZeigeVorgegebeneAdressen.Visible = False
                trZeigeZULST.Visible = False
                cmdSave.Enabled = False
                Throw New Exception("Sie sind zu einer Versendung nicht berechtigt. (Gruppenautorisierungslvl)")
            Case 1
                trZeigeVorgegebeneAdressen.Visible = False
                trZeigeManuelleAdresse.Visible = False
                trZeigeZULST.Visible = True
                cmbZuslassungstellen.Visible = True
                tbl_Adresse.Visible = False
                cmbZweigstellen.Visible = False
                chkZweigstellen.Checked = False
                rb_Manuell.Checked = False
                chkZulassungsstellen.Checked = True
            Case 2
                trZeigeZULST.Visible = True
                trZeigeVorgegebeneAdressen.Visible = True
                trZeigeManuelleAdresse.Visible = False
                chkZweigstellen.Checked = True
                cmbZuslassungstellen.Visible = False
                tbl_Adresse.Visible = False
                rb_Manuell.Checked = False
                chkZulassungsstellen.Checked = False
                cmbZweigstellen.Visible = True
            Case 3
                trZeigeZULST.Visible = True
                trZeigeVorgegebeneAdressen.Visible = True
                trZeigeManuelleAdresse.Visible = True
        End Select

        Dim booZulassungsstelle As Boolean = False
        Dim booDAD As Boolean = False
        Dim booHaendler As Boolean = False

        If objHaendler.Fahrzeuge.Select("AUGRU = '013'").Length > 0 Then booZulassungsstelle = True
        If objHaendler.Fahrzeuge.Select("AUGRU = '014'").Length > 0 Then booZulassungsstelle = True
        If objHaendler.Fahrzeuge.Select("AUGRU = '015'").Length > 0 Then booZulassungsstelle = True
        If objHaendler.Fahrzeuge.Select("AUGRU = '120'").Length > 0 Then booZulassungsstelle = True
        If objHaendler.Fahrzeuge.Select("AUGRU = '129'").Length > 0 Then booZulassungsstelle = True
        If objHaendler.Fahrzeuge.Select("AUGRU = '130'").Length > 0 Then booZulassungsstelle = True

        If booZulassungsstelle = True Then
            trZeigeZULST.Visible = True
            cmbZuslassungstellen.Visible = True
            chkZulassungsstellen.Checked = True
            chkZweigstellen.Checked = False
            trZeigeVorgegebeneAdressen.Visible = False
            trZeigeManuelleAdresse.Visible = False
        End If

        If objHaendler.Fahrzeuge.Select("AUGRU = '162'").Length > 0 Then booDAD = True
        If objHaendler.Fahrzeuge.Select("AUGRU = '167'").Length > 0 Then booDAD = True

        If booDAD = True Then
            trZeigeZULST.Visible = False
            cmbZuslassungstellen.Visible = False
            chkZulassungsstellen.Checked = False
            chkZweigstellen.Checked = False
            trZeigeVorgegebeneAdressen.Visible = True
            trZeigeManuelleAdresse.Visible = False

            Dim tmpIntValue As Int32 = objSuche.LeseAdressenSAP(Session("AppId").ToString, Session.SessionID, Me, objSuche.SapInterneHaendlerReferenzNummer)
            If tmpIntValue > 0 Then
                Dim cmbItem As New System.Web.UI.WebControls.ListItem()

                For Each tmpRow As DataRow In objSuche.SearchResult.Rows
                    cmbItem = New System.Web.UI.WebControls.ListItem()
                    If tmpRow("DISPLAY_ADDRESS").ToString.ToUpper.Contains("DAD") AndAlso _
                        tmpRow("DISPLAY_ADDRESS").ToString.Contains("22926") AndAlso _
                        tmpRow("DISPLAY_ADDRESS").ToString.ToUpper.Contains("AHRENSBURG") Then
                        cmbItem.Value = tmpRow("ADDRESSNUMBER").ToString
                        cmbItem.Text = tmpRow("DISPLAY_ADDRESS").ToString
                        cmbZweigstellen.Items.Add(cmbItem)
                    End If
                Next

                If Not cmbZweigstellen.Items.Count = 0 Then
                    chkZweigstellen.Checked = True
                    cmbZweigstellen.Visible = True
                    cmbZweigstellen.SelectedIndex = 0
                Else
                    chkZweigstellen.Checked = True
                    lblError.Text = "Es wurden keine Adressen für den DAD gepflegt."
                End If

            End If

        End If

        If objHaendler.Fahrzeuge.Select("AUGRU = '128'").Length > 0 Then booHaendler = True
        If objHaendler.Fahrzeuge.Select("AUGRU = '133'").Length > 0 Then booHaendler = True
        If objHaendler.Fahrzeuge.Select("AUGRU = '159'").Length > 0 Then booHaendler = True
        If objHaendler.Fahrzeuge.Select("AUGRU = '160'").Length > 0 Then booHaendler = True

        If booHaendler = True Then
            trZeigeZULST.Visible = False
            cmbZuslassungstellen.Visible = False
            chkZulassungsstellen.Checked = False
            chkZweigstellen.Checked = True
            trZeigeVorgegebeneAdressen.Visible = True
            trZeigeManuelleAdresse.Visible = False

            Dim tmpIntValue As Int32 = objSuche.LeseAdressenSAP(Session("AppId").ToString, Session.SessionID, Me, objSuche.SapInterneHaendlerReferenzNummer)
            If tmpIntValue > 0 Then
                Dim cmbItem As New System.Web.UI.WebControls.ListItem()

                cmbItem.Value = Kopfdaten1.SapInterneHaendlerNummer
                cmbItem.Text = Kopfdaten1.HaendlerName & ", " & Replace(Kopfdaten1.Adresse, "<br>", ", ")
                cmbZweigstellen.Items.Add(cmbItem)

                Dim tmpRow As DataRow
                For Each tmpRow In objSuche.SearchResult.Rows
                    cmbItem = New System.Web.UI.WebControls.ListItem()
                    If Not (tmpRow("DISPLAY_ADDRESS").ToString.ToUpper.Contains("DAD") AndAlso _
                        tmpRow("DISPLAY_ADDRESS").ToString.Contains("22926") AndAlso _
                        tmpRow("DISPLAY_ADDRESS").ToString.ToUpper.Contains("AHRENSBURG")) Then
                        cmbItem.Value = tmpRow("ADDRESSNUMBER").ToString
                        cmbItem.Text = tmpRow("DISPLAY_ADDRESS").ToString
                        cmbZweigstellen.Items.Add(cmbItem)
                    End If

                Next

                cmbZweigstellen.Visible = True
                chkZweigstellen.Checked = True

            End If
        End If

    End Sub

    Private Sub DoSubmit()

        Dim strAdresse As String = ""
        If chkZweigstellen.Checked Then
            Session("SelectedDeliveryValue") = cmbZweigstellen.SelectedItem.Value
            Session("SelectedDeliveryText") = cmbZweigstellen.SelectedItem.Text
            objHaendler.kbanr = String.Empty
            objHaendler.NewAdress = False
        ElseIf chkZulassungsstellen.Checked Then
            Session("SelectedDeliveryText") = "Zulassungsstelle " & cmbZuslassungstellen.SelectedItem.Text
            Session("SelectedDeliveryValue") = cmbZuslassungstellen.SelectedItem.Value
            objHaendler.kbanr = cmbZuslassungstellen.SelectedItem.Value
            objHaendler.NewAdress = True
        Else
            If txt_Name.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Name"" eingeben.<br>&nbsp;"
            Else
                objHaendler.Name1 = txt_Name.Text.Trim(" "c)
                strAdresse = txt_Name.Text.Trim(" "c) & ", "
            End If
            If txt_PLZ.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""PLZ"" eingeben.<br>&nbsp;"
            Else
                If CInt(objHaendler.Laender.Select("Land1='" & ddl_Land.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                    If Not CInt(objHaendler.Laender.Select("Land1='" & ddl_Land.SelectedItem.Value & "'")(0)("Lnplz")) = txt_PLZ.Text.Trim(" "c).Length Then
                        lblError.Text = "Postleitzahl hat falsche Länge."
                    Else
                        objHaendler.PostCode = txt_PLZ.Text.Trim(" "c) & " "
                        strAdresse = strAdresse & ddl_Land.SelectedItem.Value & "-" & txt_PLZ.Text.Trim(" "c) & " "
                    End If
                End If

            End If

            If txt_Ort.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Ort"" eingeben.<br>&nbsp;"
            Else
                objHaendler.City = txt_Ort.Text.Trim(" "c)
                strAdresse = strAdresse & txt_Ort.Text.Trim(" "c) & ", "
            End If

            If txt_Strasse.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Strasse"" eingeben.<br>&nbsp;"
            Else
                objHaendler.Street = txt_Strasse.Text.Trim(" "c)
                strAdresse = strAdresse & txt_Strasse.Text.Trim(" "c) & " "
            End If

            If txt_Nummer.Text.Trim(" "c).Length = 0 Then
                lblError.Text &= "Bitte ""Nummer"" eingeben.<br>&nbsp;"
            Else
                objHaendler.HouseNum = txt_Nummer.Text.Trim(" "c)
                strAdresse = strAdresse & txt_Nummer.Text.Trim(" "c)
            End If

            Session("SelectedDeliveryValue") = Kopfdaten1.HaendlerNummer
            Session("SelectedDeliveryText") = strAdresse
            objHaendler.NewAdress = True
            objHaendler.kbanr = String.Empty
            objHaendler.Name2 = txt_Name2.Text.Trim(" "c)
        End If

        Session("Materialnummer") = ""
        Session("VersandArtText") = ""

        If rb_VersandStandard.Checked Then
            objHaendler.DienstleisterDetail = ""
            Session("Materialnummer") = "1391"
            Session.Add("VersandArtText", rb_VersandStandard.Text)
        ElseIf rb_0900.Checked Then
            objHaendler.DienstleisterDetail = "1"
            Session("Materialnummer") = "1385"

            If lbl_0900.Visible Then
                Session.Add("VersandArtText", rb_0900.Text & " " & lbl_0900.Text)
            Else
                Session.Add("VersandArtText", rb_0900.Text)
            End If
        ElseIf rb_1000.Checked Then
            objHaendler.DienstleisterDetail = "1"
            Session("Materialnummer") = "1389"

            If lbl_1000.Visible Then
                Session.Add("VersandArtText", rb_1000.Text & " " & lbl_1000.Text)
            Else
                Session.Add("VersandArtText", rb_1000.Text)
            End If
        ElseIf rb_1200.Checked Then
            objHaendler.DienstleisterDetail = "1"
            Session("Materialnummer") = "1390"

            If lbl_1200.Visible Then
                Session.Add("VersandArtText", rb_1200.Text & " " & lbl_1200.Text)
            Else
                Session.Add("VersandArtText", rb_1200.Text)
            End If
        ElseIf rb_0900TNT.Checked Then
            objHaendler.DienstleisterDetail = "2"
            Session("Materialnummer") = "1385"

            If lbl_900TNT.Visible Then
                Session.Add("VersandArtText", rb_0900TNT.Text & " " & lbl_900TNT.Text)
            Else
                Session.Add("VersandArtText", rb_0900TNT.Text)
            End If
        ElseIf rb_1000TNT.Checked Then
            objHaendler.DienstleisterDetail = "2"
            Session("Materialnummer") = "1389"

            If lbl_1000TNT.Visible Then
                Session.Add("VersandArtText", rb_1000TNT.Text & " " & lbl_1000TNT.Text)
            Else
                Session.Add("VersandArtText", rb_1000TNT.Text)
            End If
        ElseIf rb_1200TNT.Checked Then
            objHaendler.DienstleisterDetail = "2"
            Session("Materialnummer") = "1390"

            If lbl_1200TNT.Visible Then
                Session.Add("VersandArtText", rb_1200TNT.Text & " " & lbl_1200TNT.Text)
            Else
                Session.Add("VersandArtText", rb_1200TNT.Text)
            End If
        ElseIf rb_Sendungsverfolgt.Checked Then
            objHaendler.DienstleisterDetail = "2"
            Session("Materialnummer") = "5530"

            If lbl_Sendungsverfolgt.Visible Then
                Session.Add("VersandArtText", rb_Sendungsverfolgt.Text & " " & lbl_Sendungsverfolgt.Text)
            Else
                Session.Add("VersandArtText", rb_Sendungsverfolgt.Text)
            End If
        End If

        If Not lblError.Text.Length > 0 Then
            Session("AppHaendler") = objHaendler
            Response.Redirect("Change07_4.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Private Sub getZulStellen()
        Dim status As String = String.Empty
        Dim view As DataView
        Dim row As DataRow
        Dim tblTemp As DataTable
        Dim newRow As DataRow
        Dim counter As Integer


        'Zulassungsdienste holen
        objHaendler.getZulStelle("", "", status, Session("AppId").ToString, Session.SessionID)

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
                    newRow("KBANR") = row("KBANR").ToString
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
        With cmbZuslassungstellen
            .DataSource = view
            .DataTextField = "DISPLAY"
            .DataValueField = "KBANR"
            .DataBind()
        End With

    End Sub

    Private Sub fillLaenderDLL()

        objHaendler.getLaender(Session("AppID").ToString, Session.SessionID)

        Dim sprache As String
        ddl_Land.DataSource = objHaendler.Laender
        ddl_Land.DataTextField = "FullDesc"
        ddl_Land.DataValueField = "Land1"
        ddl_Land.DataBind()

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
        ddl_Land.Items.FindByValue(sprache).Selected = True

    End Sub

    Private Sub chkZulassungsstellen_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkZulassungsstellen.CheckedChanged
        If chkZulassungsstellen.Checked = True Then
            cmbZuslassungstellen.Visible = True
            tbl_Adresse.Visible = False
            cmbZweigstellen.Visible = False
        End If
    End Sub

    Private Sub rb_Manuell_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_Manuell.CheckedChanged
        If rb_Manuell.Checked = True Then
            cmbZuslassungstellen.Visible = False
            tbl_Adresse.Visible = True
            cmbZweigstellen.Visible = False
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub chkZweigstellen_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkZweigstellen.CheckedChanged
        If chkZweigstellen.Checked = True Then
            cmbZuslassungstellen.Visible = False
            tbl_Adresse.Visible = False
            cmbZweigstellen.Visible = True
        End If
    End Sub

    Protected Sub rb_VersandExpress_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_VersandExpress.CheckedChanged
        objHaendler = CType(Session("objHaendler"), F1_Haendler)

        rb_VersandStandard.Checked = False
        trInfTxt.Visible = False

        If Not objHaendler.Dienstleister Is Nothing Then

            If objHaendler.Dienstleister.Rows.Count > 0 Then

                trHinweis.Visible = True

                For Each dr As DataRow In objHaendler.Dienstleister.Rows

                    Select Case dr("VERSANDWEG").ToString

                        Case "1"
                            trDHL.Visible = True
                            If dr("FLAG_DEFAULT").ToString = "X" Then
                                rb_0900.Checked = True
                            End If
                        Case "2"
                            trTNT.Visible = True
                            If dr("FLAG_DEFAULT").ToString = "X" Then
                                rb_0900TNT.Checked = True
                            End If
                    End Select

                Next
            End If
        End If

    End Sub

    Protected Sub rb_VersandStandard_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_VersandStandard.CheckedChanged
        rb_VersandExpress.Checked = False
        trHinweis.Visible = False
        trDHL.Visible = False
        trTNT.Visible = False
        trInfTxt.Visible = True
    End Sub
End Class