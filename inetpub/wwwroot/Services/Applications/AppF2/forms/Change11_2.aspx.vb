Imports System.IO
Imports System.Net.Mail
Imports System.Reflection
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Logging
Imports CKG.Base.Kernel.DocumentGeneration
Imports CKG.Base.Kernel.Security
Imports Telerik.Web.UI

Public Class Change11_2
    Inherits System.Web.UI.Page
    Implements IHttpHandler

    Private m_User As User
    Private m_App As App

    Private m_AG As Abrufgruende
    Private m_Adr As Adressen
    Private m_Baf As Briefanforderung
    Private m_UE As UnangeforderteEquipments
    Private m_VS As Versandwege
    Private m_L As Laender
    Private m_Zls As Zulassungstellen

    Private m_State As States

    Protected kopfdaten As CKG.Services.PageElements.Kopfdaten

    Private Enum States
        VersandartWahl
        VersandConfirm
        HaendlerZulassung
        EndkundenZulassung

        Done

    End Enum

    Public Overrides Sub ProcessRequest(ByVal context As HttpContext)
        If context.Request.QueryString.AllKeys.Contains("Download") Then
            Dim f = CStr(context.Session("DLFile"))

            If Not String.IsNullOrEmpty(f) AndAlso File.Exists(f) Then
                Dim downloadFile As FileInfo = New FileInfo(f)
                Dim fileName = Path.GetFileNameWithoutExtension(f).Split("_")(0) & Path.GetExtension(f)

                context.Response.Clear()
                context.Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", fileName))
                context.Response.AddHeader("Content-Length", downloadFile.Length.ToString())
                context.Response.ContentType = "application/octet-stream"
                context.Response.WriteFile(downloadFile.FullName)
                context.Response.Flush()
                context.Response.End()
            End If
        Else
            MyBase.ProcessRequest(context)
        End If
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        MyBase.OnLoad(e)

        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        m_App = New App(m_User)
        Common.GetAppIDFromQueryString(Me)
        Dim appId = CStr(Session("AppID"))

        lblHead.Text = m_User.Applications.Select("AppID = '" & appId & "'")(0)("AppFriendlyName").ToString

        Try
            m_AG = Common.GetOrCreateObject("AG", Function() New Abrufgruende(m_User))
            m_Adr = Common.GetOrCreateObject("Adr", Function() New Adressen(m_User, m_App, appId, Session.SessionID))
            m_Baf = Common.GetOrCreateObject("Baf", Function() New Briefanforderung(m_User, m_App, appId, Session.SessionID))
            m_Baf.Haendler_Ex = CStr(Session("HAENDLER_EX"))
            m_L = Common.GetOrCreateObject("Lae", Function() New Laender(m_User, m_App, appId, Session.SessionID))
            m_UE = Common.GetOrCreateObject("UE", Function() New UnangeforderteEquipments(m_User, m_App, appId, Session.SessionID))
            m_VS = Common.GetOrCreateObject("VS", Function() New Versandwege(m_User, m_App, appId, Session.SessionID))
            m_Zls = Common.GetOrCreateObject("Zls", Function() New Zulassungstellen(m_User, m_App, appId, Session.SessionID))

            augruSource.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            augruSource.SelectParameters.Clear()
            augruSource.SelectParameters.Add("cID", m_User.Customer.CustomerId)
            augruSource.SelectParameters.Add("gID", m_User.GroupID)

            kopfdaten.UserReferenz = m_User.Reference
            kopfdaten.HaendlerNummer = Session("HAENDLER_EX")
            kopfdaten.HaendlerName = Session("HAENDLER_NAME")
            kopfdaten.Adresse = Session("HAENDLER_ADDR")

            If Not IsPostBack Then
                Dim augrus = m_UE.Result.Select("Selected").Select(Function(r) CStr(r("AUGRU"))).Distinct.ToList
                If augrus.Count = 1 Then
                    If augrus(0) = "169" Then
                        m_State = States.HaendlerZulassung
                    ElseIf augrus(0) = "168" Then
                        m_State = States.EndkundenZulassung
                    End If
                Else
                    m_State = States.VersandartWahl
                End If
            ElseIf Not Session("CurrentState") Is Nothing AndAlso TypeOf Session("CurrentState") Is States Then
                m_State = CType(Session("CurrentState"), States)
            End If
            Session("CurrentState") = m_State

            If m_State <> States.Done Then
                CheckVersandberechtigung()
                SetVersandModus()
                BindDropDowns()
            End If

            SyncToState()
        Catch ex As Exception
            lblError.Text = ex.Message
            lblError.Visible = True
            views.Visible = False
            cmdSave.Visible = False
            Array.ForEach({"AG", "Adr", "Baf", "Lae", "UE", "VS", "Zls"}, Sub(s) Session.Remove(s))
        End Try
    End Sub

    Private Sub SyncToState()
        Select Case m_State
            Case States.VersandartWahl
                views.SetActiveView(versandartView)
            Case States.VersandConfirm
                views.SetActiveView(versandConfirm)

                lblVersandart.Text = m_Baf.VersandText
                lblMaterialNummer.Text = m_Baf.VersandMaterialNummer

                lblAddress.Text = m_Baf.DeliveryText

                LoadData()

                cmdSave.Text = "» Bestätigen"
            Case States.HaendlerZulassung
                views.SetActiveView(haendlerZulView)
            Case States.EndkundenZulassung
                views.SetActiveView(halterInfoView)
            Case States.Done
                Array.ForEach({"AG", "Adr", "Baf", "Lae", "UE", "VS", "Zls"}, Sub(s) Session.Remove(s))
                lbBack.Text = "Weitere Dokumente anfordern"
                views.SetActiveView(doneView)
        End Select
    End Sub

    Protected Sub GridNeedDataSource(ByVal sender As Object, e As GridNeedDataSourceEventArgs)
        LoadData(False)
    End Sub

    Private Sub LoadData(Optional ByVal rebind As Boolean = True)
        Dim view = New DataView(m_UE.Result, "Selected", "CHASSIS_NUM", DataViewRowState.CurrentRows)
        selFzgGrid.DataSource = view
        If rebind Then selFzgGrid.Rebind()
    End Sub

    Protected Overrides Sub OnUnload(e As System.EventArgs)
        MyBase.OnUnload(e)

        Common.TranslateTelerikColumns(selFzgGrid)
        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        MyBase.OnPreRender(e)

        Common.TranslateTelerikColumns(selFzgGrid)
        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub CheckVersandberechtigung()

        Dim authRight = m_User.Groups(0).Authorizationright

        Dim augrus = m_UE.Result.Select("Selected").Cast(Of DataRow).Select(Function(r) CStr(r("AUGRU"))).Distinct.ToList()
        Dim eingeschraenkt = 0
        For Each augru In augrus
            Dim r = m_AG.Result.Select("SapWert='" & augru & "'").FirstOrDefault
            If r Is Nothing OrElse r("Eingeschraenkt") Is DBNull.Value Then Continue For
            Dim i = CInt(r("Eingeschraenkt"))
            If i > 0 AndAlso (i < eingeschraenkt Or eingeschraenkt = 0) Then
                eingeschraenkt = i
            End If
        Next

        Dim berechtigung = authRight
        If eingeschraenkt > 0 AndAlso eingeschraenkt < authRight Then
            berechtigung = eingeschraenkt
        End If

        Select Case berechtigung
            Case 0
                trZeigeManuelleAdresse.Visible = False
                trZeigeVorgegebeneAdressen.Visible = False
                trZeigeZULST.Visible = False
                cmdSave.Enabled = False
                Throw New Exception("Sie sind zu einer Versendung nicht berechtigt. (Gruppenautorisierungslvl)")
            Case 1
                trZeigeManuelleAdresse.Visible = False
                trZeigeVorgegebeneAdressen.Visible = False
                trZeigeZULST.Visible = True
                'tbl_Adresse.Visible = False
                'cmbZulassungstellen.Visible = True
                'cmbZweigstellen.Visible = False
                rb_Manuell.Checked = False
                rb_Zulassungsstellen.Checked = True
                rb_Zweigstellen.Checked = False
            Case 2
                trZeigeManuelleAdresse.Visible = False
                trZeigeVorgegebeneAdressen.Visible = True
                trZeigeZULST.Visible = True
                'tbl_Adresse.Visible = False
                'cmbZulassungstellen.Visible = False
                'cmbZweigstellen.Visible = True
                'rb_Manuell.Checked = False
                'rb_Zulassungsstellen.Checked = False
                'rb_Zweigstellen.Checked = True
                If rb_Manuell.Checked Then
                    rb_Manuell.Checked = False
                    rb_Zulassungsstellen.Checked = False
                    rb_Zweigstellen.Checked = True
                End If
            Case 3
                trZeigeZULST.Visible = True
                trZeigeVorgegebeneAdressen.Visible = True
                trZeigeManuelleAdresse.Visible = True
        End Select
    End Sub

    Private Sub SetVersandModus()
        If rb_Manuell.Checked = True Then
            cmbZulassungstellen.Visible = False
            tbl_Adresse.Visible = True
            cmbZweigstellen.Visible = False
        ElseIf rb_Zulassungsstellen.Checked Then
            cmbZulassungstellen.Visible = True
            tbl_Adresse.Visible = False
            cmbZweigstellen.Visible = False
        ElseIf rb_Zweigstellen.Checked Then
            cmbZulassungstellen.Visible = False
            tbl_Adresse.Visible = False
            cmbZweigstellen.Visible = True
        End If
    End Sub

    Private Sub BindDropDowns()
        If IsPostBack Then Return

        RebindLaender()
        RebindZulassungstellen()
        RebindZweigstellen()
    End Sub

    Private Sub RebindLaender()
        m_L.LoadData(Me)
        ddl_Land.DataSource = m_L.Result
        ddl_Land.DataTextField = "FullDesc"
        ddl_Land.DataValueField = "Land1"
        ddl_Land.DataBind()

        Dim languages = Request("HTTP_ACCEPT_Language").Split(","c).First
        Dim region = languages.Split("-"c).Last().ToUpper
        ddl_Land.Items.FindByValue(region).Selected = True
    End Sub

    Private Sub RebindZulassungstellen()
        m_Zls.LoadData(Me)

        Dim view = m_Zls.Result.DefaultView
        view.Sort = "DISPLAY"
        With cmbZulassungstellen
            .DataSource = view
            .DataTextField = "DISPLAY"
            .DataValueField = "KBANR"
            .DataBind()
        End With
    End Sub

    Private Sub RebindZweigstellen()
        Dim haendler = CStr(Session("HAENDLER"))
        If String.IsNullOrEmpty(haendler) Then
            Throw New Exception("Haendler nicht gesetzt.")
        End If

        m_Adr.LoadData(Me, haendler)

        cmbZweigstellen.DataSource = Nothing
        cmbZweigstellen.Items.Clear()

        cmbZweigstellen.Items.Add(New ListItem(kopfdaten.HaendlerName & ", " & Replace(kopfdaten.Adresse, "<br />", ", "), haendler))
        cmbZweigstellen.AppendDataBoundItems = True

        Dim view = m_Adr.Result.DefaultView
        view.Sort = "DISPLAY_ADDRESS"
        With cmbZweigstellen
            .DataSource = view
            .DataTextField = "DISPLAY_ADDRESS"
            .DataValueField = "ADDRESSNUMBER"
            .DataBind()
        End With
    End Sub

    Protected Sub SendClick(ByVal sender As Object, ByVal e As EventArgs)
        Select Case m_State
            Case States.VersandartWahl
                VersandartWaehlen()
            Case States.VersandConfirm
                VersandAnfordern()
            Case States.EndkundenZulassung
                EndkundenZulassung()
            Case States.HaendlerZulassung
                HaendlerZulassung()
        End Select
    End Sub

    Private Sub VersandartWaehlen()
        lblError.Text = String.Empty
        m_Baf.Name1 = String.Empty
        m_Baf.Name2 = String.Empty
        m_Baf.Street = String.Empty
        m_Baf.HouseNum = String.Empty
        m_Baf.PostCode = String.Empty
        m_Baf.City = String.Empty

        If rb_Manuell.Checked Then
            CheckTextNotEmpty(txt_Name, "Name")
            CheckTextNotEmpty(txt_PLZ, "PLZ")
            CheckTextNotEmpty(txt_Ort, "Ort")
            CheckTextNotEmpty(txt_Strasse, "Strasse")
            CheckTextNotEmpty(txt_Nummer, "Nummer")
            If Not String.IsNullOrEmpty(txt_PLZ.Text.Trim) Then
                Dim landRow = m_L.Result.Select("Land1='" & ddl_Land.SelectedItem.Value & "'").FirstOrDefault
                If Not landRow Is Nothing AndAlso CInt(landRow("Lnplz")) <> txt_PLZ.Text.Trim.Length Then
                    lblError.Text &= "Postleitzahl hat falsche Länge.<br>&nbsp;"
                End If
            End If
            If Not String.IsNullOrEmpty(lblError.Text) Then
                lblError.Visible = True
                Exit Sub
            End If

            m_Baf.Name1 = txt_Name.Text.Trim
            m_Baf.Name2 = txt_Name2.Text.Trim
            m_Baf.Street = txt_Strasse.Text.Trim
            m_Baf.HouseNum = txt_Nummer.Text.Trim
            m_Baf.PostCode = txt_PLZ.Text.Trim
            m_Baf.City = txt_Ort.Text.Trim

            m_Baf.DeliveryText = String.Format("{0}, {1} - {2} {3}, {4} {5}", m_Baf.Name1, ddl_Land.SelectedItem.Value, m_Baf.PostCode, m_Baf.City, m_Baf.Street, m_Baf.HouseNum)
            m_Baf.DeliveryValue = CStr(Session("HAENDLER_EX"))
            m_Baf.Kbanr = String.Empty
            m_Baf.NewAdress = True
        ElseIf rb_Zulassungsstellen.Checked Then
            m_Baf.DeliveryValue = cmbZulassungstellen.SelectedItem.Value
            m_Baf.DeliveryText = "Zulassungsstelle " & cmbZulassungstellen.SelectedItem.Text
            m_Baf.Kbanr = m_Baf.DeliveryValue
            m_Baf.NewAdress = True
        ElseIf rb_Zweigstellen.Checked Then
            m_Baf.DeliveryValue = cmbZweigstellen.SelectedItem.Value
            m_Baf.DeliveryText = cmbZweigstellen.SelectedItem.Text
            m_Baf.Kbanr = String.Empty
            m_Baf.NewAdress = False
        End If

        If rb_VersandStandard.Checked Then
            m_Baf.VersandText = rb_VersandStandard.Text
            m_Baf.VersandMaterialNummer = "1391"
            m_Baf.VersandDienstleister = String.Empty
        ElseIf rb_0900.Checked Then
            m_Baf.VersandText = VisibleText(rb_0900, lbl_0900)
            m_Baf.VersandMaterialNummer = "1385"
            m_Baf.VersandDienstleister = "1"
        ElseIf rb_0900TNT.Checked Then
            m_Baf.VersandText = VisibleText(rb_0900TNT, lbl_0900TNT)
            m_Baf.VersandMaterialNummer = "1385"
            m_Baf.VersandDienstleister = "2"
        ElseIf rb_1000.Checked Then
            m_Baf.VersandText = VisibleText(rb_1000, lbl_1000)
            m_Baf.VersandMaterialNummer = "1389"
            m_Baf.VersandDienstleister = "1"
        ElseIf rb_1000TNT.Checked Then
            m_Baf.VersandText = VisibleText(rb_1000TNT, lbl_1000TNT)
            m_Baf.VersandMaterialNummer = "1389"
            m_Baf.VersandDienstleister = "2"
        ElseIf rb_1200.Checked Then
            m_Baf.VersandText = VisibleText(rb_1200, lbl_1200)
            m_Baf.VersandMaterialNummer = "1390"
            m_Baf.VersandDienstleister = "1"
        ElseIf rb_1200TNT.Checked Then
            m_Baf.VersandText = VisibleText(rb_1200TNT, lbl_1200TNT)
            m_Baf.VersandMaterialNummer = "1390"
            m_Baf.VersandDienstleister = "2"
        ElseIf rb_Sendungsverfolgt.Checked Then
            m_Baf.VersandText = VisibleText(rb_Sendungsverfolgt, lbl_Sendungsverfolgt)
            m_Baf.VersandMaterialNummer = "5530"
            m_Baf.VersandDienstleister = "2"
        End If

        m_State = States.VersandConfirm
        Session("CurrentState") = m_State
        SyncToState()
    End Sub

    Private Sub VersandAnfordern(Optional ByVal switchStates As Boolean = True)
        If switchStates Then
            rfvTEXT50.Enabled = True
            rfvTEXT50.Validate()

            If Not rfvTEXT50.IsValid Then Return
        End If

        Dim log = New Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        log.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), lblHead.Text, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        Dim hasErrors = False
        Dim rows = m_UE.Result.Select("Selected")
        For Each row In rows
            m_Baf.Anfordern(Me, row, txtTEXT50.Text)

            If m_Baf.Status < 0 Then
                hasErrors = True
                If Not String.IsNullOrEmpty(m_Baf.Message) Then row("COMMENT") = "Fehler: " & m_Baf.Message
                Exit For
            End If
        Next

        If Not hasErrors Then
            Dim logTable As New DataTable()
            Array.ForEach({"Händlernr.", "Fahrgestellnr.", "Kundenreferenz", _
                           "Nummer ZBII", "Kennzeichen", "Kundenreferenz 2", _
                           "Equipmentnr.", "COC", "Auftragsnr.", "Kommentar", _
                           "Kontingentart", "Abrufgrund"}, Sub(s) logTable.Columns.Add(s, GetType(String)))
            Dim augru2Text = m_AG.Result.Rows.Cast(Of DataRow).ToDictionary(Function(r) CStr(r("SapWert")), Function(r) CStr(r("WebBezeichnung")))

            For Each row In rows
                Dim logRow = logTable.NewRow
                logRow("Händlernr.") = row("HAENDLER_EX")
                logRow("Fahrgestellnr.") = row("CHASSIS_NUM")
                logRow("Kundenreferenz") = row("LIZNR")
                logRow("Nummer ZBII") = row("TIDNR")
                logRow("Kennzeichen") = row("LICENSE_NUM")
                logRow("Kundenreferenz 2") = row("ZZREFERENZ1")
                logRow("Equipmentnr.") = row("EQUNR")
                logRow("COC") = row("ZZCOCKZ")
                logRow("Auftragsnr.") = row("VBELN")
                logRow("Kommentar") = row("COMMENT")
                logRow("Kontingentart") = "Standard endgültig"
                If augru2Text.ContainsKey(CStr(row("AUGRU"))) Then logRow("Abrufgrund") = augru2Text(CStr(row("AUGRU")))

                logTable.Rows.Add(logRow)
            Next

            log.UpdateEntry("APP", Session("AppID"), "Dokumentenanforderung zu Händler-Nr. " & kopfdaten.HaendlerNummer & " erfolgreich durchgeführt.", logTable)
        End If

        LoadData()

        rfvTEXT50.Enabled = False

        If switchStates AndAlso Not hasErrors Then
            cmdSave.Enabled = False
            lbl_Message.Text = "Versand wurde angefordert."
            m_State = States.Done
            Session("CurrentState") = m_State
            SyncToState()
        End If
    End Sub

    Private Sub EndkundenZulassung()
        Dim validators As BaseValidator() = {cv_HTitle, rfv_HLastName, rfv_HStreet, rfv_HPostCode, rfv_HCity, rfv_HZulDatum, rfv_HZulOrt, cv_HZulDatum}
        If validators.All(Function(v)
                              v.Validate()
                              Return v.IsValid
                          End Function) Then

            Dim head = New DataTable("Kopf")
            Dim headColumns = {"Username", "HalterAnrede", "HalterVorname", "HalterName", _
                               "HalterStrasseHausnummer", "HalterPLZOrt", _
                               "Zulassungsort", "Perso", "Vollmacht", "ECCopy", "EVBNr", _
                               "WunschKennz", "Comment", "HaendlerEx", "HaendlerName", "HaendlerAdr"}
            Array.ForEach(headColumns, Function(c) head.Columns.Add(c, GetType(String)))
            head.Columns.Add("Datum", GetType(DateTime))
            head.Columns.Add("Zulassungsdatum", GetType(DateTime))
            Dim headRow = head.NewRow
            headRow("Username") = m_User.UserName
            headRow("Datum") = DateTime.Today
            headRow("HaendlerEx") = kopfdaten.HaendlerNummer
            headRow("HaendlerName") = kopfdaten.HaendlerName.Replace("<br />", Environment.NewLine & ControlChars.Tab)
            headRow("HaendlerAdr") = kopfdaten.Adresse.Replace("<br />", Environment.NewLine & ControlChars.Tab)
            headRow("HalterAnrede") = ddl_HTitle.SelectedValue & " "
            headRow("HalterVorname") = IIf(String.IsNullOrEmpty(txt_HFirstName.Text), String.Empty, txt_HFirstName.Text & " ")
            headRow("HalterName") = txt_HLastName.Text
            headRow("HalterStrasseHausnummer") = txt_HStreet.Text
            headRow("HalterPLZOrt") = String.Join(" ", {txt_HPostCode.Text.Trim, txt_HCity.Text.Trim})
            headRow("Zulassungsort") = txt_HZulOrt.Text
            headRow("Zulassungsdatum") = DateTime.Parse(txt_HZulDatum.Text)
            headRow("Perso") = IIf(chk_HPerso.Checked, "ja", "nein")
            headRow("Vollmacht") = IIf(chk_HVollmacht.Checked, "ja", "nein")
            headRow("ECCopy") = IIf(chk_HECCopy.Checked, "ja", "nein")
            headRow("EVBNr") = IIf(chk_HEVBNr.Checked, "ja", "nein")
            headRow("WunschKennz") = IIf(chk_HWunschKennz.Checked, "ja", "nein")
            headRow("Comment") = txt_HComment.Text
            head.Rows.Add(headRow)
            head.AcceptChanges()

            Dim ds = CreateFahrzeugDS()
            Dim reportName = System.IO.Path.Combine(System.IO.Path.GetTempPath, "Endkundenzulassung_" & Guid.NewGuid.ToString)

            ' Cleanup old generated files
            Dim oldFileLimit = DateTime.Now.AddMinutes(-15)
            Dim oldFiles = System.IO.Directory.GetFiles(System.IO.Path.GetTempPath, "Endkundenzulassung_*.pdf").Where(Function(f) New System.IO.FileInfo(f).CreationTime < oldFileLimit).ToArray
            Array.ForEach(oldFiles, Sub(f)
                                        Try
                                            File.Delete(f)
                                        Catch ex As Exception
                                        End Try
                                    End Sub)

            Dim wdf = New WordDocumentFactory(New System.Data.DataTable("dt"), Nothing)
            wdf.CreateDocumentDatasetandSave(reportName, Me, "\Applications\appF2\docu\Endkundenzulassung.doc", head, ds)

            Dim reportFileName = reportName & ".pdf"

            If File.Exists(reportFileName) Then
                ' TODO: Empfänger über Web.Config? setzen
                SendDocument(New MailAddress("Web-Administrator@dad.de,gmac-daten@dad.de"), reportFileName, "Endkundenzulassung.pdf", "Neue Endkundenzulassung angefordert, siehe Anhang", "[GMAC] Endkundenzulassung")

                cmdDownload.Visible = True
                Session("DLFile") = reportFileName
            End If

            SetDADVersand()
            VersandAnfordern(False)

            cmdSave.Enabled = False

            m_State = States.Done
            Session("CurrentState") = m_State
            lbl_Message.Text = "Ihr Zulassungsauftrag wurde angelegt."
            SyncToState()
        End If
    End Sub

    Private Sub HaendlerZulassung()
        rfv_Zulassungsdatum.Validate()

        If rfv_Zulassungsdatum.IsValid Then
            Dim zulDatum = DateTime.Parse(txt_Zulassungsdatum.Text)
            Dim abmDatum As Nullable(Of DateTime) = Nothing
            If chk_Abmeldung.Checked Then
                rfv_Abmeldung.Validate()
                If Not rfv_Abmeldung.IsValid Then
                    Return
                Else
                    abmDatum = DateTime.Parse(txt_Abmeldung.Text)
                End If
            End If

            Dim head = New DataTable("Kopf")
            Dim headColumns = {"Username", "Abmeldung", "HaendlerEx", "HaendlerName", "HaendlerAdr"}
            Array.ForEach(headColumns, Function(c) head.Columns.Add(c, GetType(String)))
            head.Columns.Add("Datum", GetType(DateTime))
            head.Columns.Add("Zulassungsdatum", GetType(DateTime))
            Dim headRow = head.NewRow
            headRow("Username") = m_User.UserName
            headRow("Datum") = DateTime.Today
            headRow("HaendlerEx") = kopfdaten.HaendlerNummer
            headRow("HaendlerName") = kopfdaten.HaendlerName.Replace("<br />", Environment.NewLine & ControlChars.Tab)
            headRow("HaendlerAdr") = kopfdaten.Adresse.Replace("<br />", Environment.NewLine & ControlChars.Tab)
            headRow("Zulassungsdatum") = zulDatum
            If abmDatum.HasValue Then headRow("Abmeldung") = String.Format("Automatische Abmeldung am {0:dd.MM.yyyy}", abmDatum.Value)
            head.Rows.Add(headRow)
            head.AcceptChanges()

            Dim ds = CreateFahrzeugDS()
            Dim reportName = System.IO.Path.Combine(System.IO.Path.GetTempPath, "Haendlerzulassung_" & Guid.NewGuid.ToString)

            ' Cleanup old generated files
            Dim oldFileLimit = DateTime.Now.AddMinutes(-15)
            Dim oldFiles = System.IO.Directory.GetFiles(System.IO.Path.GetTempPath, "Haendlerzulassung_*.pdf").Where(Function(f) New System.IO.FileInfo(f).CreationTime < oldFileLimit).ToArray
            Array.ForEach(oldFiles, Sub(f)
                                        Try
                                            File.Delete(f)
                                        Catch ex As Exception
                                        End Try
                                    End Sub)


            Dim wdf = New WordDocumentFactory(New System.Data.DataTable("dt"), Nothing)
            wdf.CreateDocumentDatasetandSave(reportName, Me, "\Applications\appF2\docu\Haendlerzulassung.doc", head, ds)

            Dim reportFileName = reportName & ".pdf"

            If File.Exists(reportFileName) Then
                ' TODO: Empfänger über Web.Config? setzen
                SendDocument(New MailAddress("Web-Administrator@dad.de,gmac-daten@dad.de"), reportFileName, "Händlerzulassung.pdf", "Neue Händlerzulassung angefordert, siehe Anhang", "[GMAC] Händlerzulassung")

                cmdDownload.Visible = True
                Session("DLFile") = reportFileName
            End If

            SetDADVersand()
            VersandAnfordern(False)

            cmdSave.Enabled = False

            m_State = States.Done
            Session("CurrentState") = m_State
            lbl_Message.Text = "Ihr Auftrag zur händlereigenen Zulassung wurde angelegt."
            SyncToState()
        End If
    End Sub

    Private Sub SetDADVersand()
        m_Baf.Name1 = "DAD Deutscher Autodienst"
        m_Baf.Name2 = "GMAC-Zulassung"
        m_Baf.Street = "Ladestraße"
        m_Baf.HouseNum = "1"
        m_Baf.PostCode = "22926"
        m_Baf.City = "Ahrensburg"

        m_Baf.DeliveryText = String.Format("{0}, {1} - {2} {3}, {4} {5}", m_Baf.Name1, "DE", m_Baf.PostCode, m_Baf.City, m_Baf.Street, m_Baf.HouseNum)
        m_Baf.DeliveryValue = CStr(Session("HAENDLER_EX"))
        m_Baf.Kbanr = String.Empty
        m_Baf.NewAdress = True
    End Sub

    Private Function CreateFahrzeugDS() As DataSet
        Dim ds = New DataSet()
        Dim fzg = ds.Tables.Add("Fahrzeuge")
        Dim fzgColumns = {"Fahrgestellnummer", "ZBII", "COC", "Kennzeichen", "Bezahlt", "Referenz"}
        Array.ForEach(fzgColumns, Function(c) fzg.Columns.Add(c, GetType(String)))
        fzg.Columns.Add("Vertragsdatum", GetType(DateTime))
        Dim selected = m_UE.Result.Select("Selected")
        For Each f In selected
            Dim fzgRow = fzg.NewRow()
            fzgRow("Fahrgestellnummer") = f("CHASSIS_NUM")
            fzgRow("ZBII") = f("TIDNR")
            fzgRow("COC") = IIf("X".Equals(f("ZZCOCKZ")), "ja", "nein")
            fzgRow("Kennzeichen") = f("LICENSE_NUM")
            fzgRow("Bezahlt") = IIf("X".Equals(f("ZZBEZAHLT")), "ja", "nein")
            fzgRow("Referenz") = IIf(m_State = States.EndkundenZulassung, txt_HReference.Text, txt_Reference.Text)

            Dim vdStr = CStr(f("ERDAT"))
            Dim vd As DateTime
            fzgRow("Vertragsdatum") = IIf(DateTime.TryParse(vdStr, vd), vd, DBNull.Value)
            fzg.Rows.Add(fzgRow)
        Next
        fzg.AcceptChanges()

        Return ds
    End Function

    Private Sub SendDocument(ByVal dest As MailAddress, ByVal tempDocumentFile As String, ByVal documentName As String, ByVal body As String, ByVal subject As String)
        Dim s = File.OpenRead(tempDocumentFile)
        Dim att = New Attachment(s, Path.GetFileName(documentName))

        Dim sender = New MailAddress(ConfigurationManager.AppSettings("SmtpMailSender"))
        Dim msg = New MailMessage(sender, dest)

        msg.Attachments.Add(att)
        msg.Body = body
        msg.Subject = subject

        Dim mta = ConfigurationManager.AppSettings("SmtpMailServer")
        Dim client = New SmtpClient(mta)
        client.Send(msg)

        s.Close()
    End Sub

    Private Sub CheckTextNotEmpty(ByVal txt As TextBox, ByVal name As String)
        If String.IsNullOrEmpty(txt.Text.Trim) Then
            lblError.Text &= String.Format("Bitte ""{0}"" eingeben.<br>&nbsp;", name)
        End If
    End Sub

    Private Function VisibleText(ByVal rbtn As RadioButton, ByVal lbl As Label) As String
        If lbl.Visible Then Return rbtn.Text & " " & lbl.Text
        Return rbtn.Text
    End Function

    Protected Sub rb_VersandExpress_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_VersandExpress.CheckedChanged
        rb_VersandStandard.Checked = False
        trInfTxt.Visible = False

        m_VS.LoadData(Me)

        Dim versWege = m_VS.Result.Rows.Cast(Of DataRow).Select(Function(r) New With {.VersWeg = CStr(r("VERSANDWEG")), .Default = r("FLAG_DEFAULT").Equals("X")}).ToList()

        If versWege.Count > 0 Then
            trHinweis.Visible = True

            Dim dhl = versWege.FirstOrDefault(Function(w) w.VersWeg = "1")
            If Not dhl Is Nothing Then
                trDHL.Visible = True
                If dhl.Default Then rb_0900.Checked = True
            End If

            Dim tnt = versWege.FirstOrDefault(Function(w) w.VersWeg = "2")
            If Not tnt Is Nothing Then
                trTNT.Visible = True
                If tnt.Default Then rb_0900TNT.Checked = True
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

    Protected Sub AbmeldungChanged(ByVal sender As Object, ByVal e As EventArgs)
        txt_Abmeldung.Enabled = chk_Abmeldung.Checked
        lbl_Abmeldung.Enabled = chk_Abmeldung.Checked
        rfv_Abmeldung.Enabled = chk_Abmeldung.Checked
    End Sub

    'Protected Sub NavigateBack(ByVal sender As Object, ByVal e As EventArgs)
    '    If m_State = States.Done Then
    '        Response.Redirect("Change11.aspx" + Request.Url.Query)
    '    Else
    '        Response.Redirect("Change11_1.aspx" + Request.Url.Query)
    '    End If
    'End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        If m_State = States.Done Then
            Response.Redirect("Change11.aspx" + Request.Url.Query)
        Else
            Response.Redirect("Change11_1.aspx" + Request.Url.Query)
        End If
    End Sub

    Protected Sub ValidateZulassungsDatum(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        Dim zulassung = DateTime.Parse(e.Value).Date
        If zulassung < DateTime.Today Then
            e.IsValid = False
        ElseIf zulassung.DayOfWeek = DayOfWeek.Sunday OrElse zulassung.DayOfWeek = DayOfWeek.Saturday Then
            e.IsValid = False
        Else
            e.IsValid = True
        End If
    End Sub

    Protected Sub ValidateTitle(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        e.IsValid = Not String.IsNullOrEmpty(ddl_HTitle.SelectedValue)
    End Sub
End Class