Option Explicit On
Option Strict On

Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change02_2
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

    Protected WithEvents lb_weiter As LinkButton
    Protected WithEvents lb_zurueck As LinkButton
    Protected WithEvents lb_Suche As LinkButton
    Protected WithEvents lb_NeuSuche As LinkButton
    Protected WithEvents lb_Auswahl As System.Web.UI.WebControls.LinkButton

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App


    Protected WithEvents rb_Geschaeftsstelle As RadioButton
    Protected WithEvents rb_Partneradresse As RadioButton
    Protected WithEvents rb_Versandanschrift As RadioButton
    Protected WithEvents rb_1391 As RadioButton
    Protected WithEvents rb_1385 As RadioButton
    Protected WithEvents rb_1390 As RadioButton
    Protected WithEvents rb_1389 As RadioButton
    Protected WithEvents rb_5530 As RadioButton

    Protected WithEvents ddlPartneradressen As DropDownList
    Protected WithEvents ddlGeschaeftsstelle As DropDownList
    Protected WithEvents ddlLand As DropDownList




    Protected WithEvents tblSuche As HtmlTable
    Protected WithEvents tr_Partneradresse As HtmlTableRow
    Protected WithEvents tr_Geschaeftsstelle As HtmlTableRow
    Protected WithEvents tr_Versandanschrift As HtmlTableRow
    Protected WithEvents trVersandanschriftValue As HtmlTableRow
    Protected WithEvents trVersandart As HtmlTableRow
    Protected WithEvents trVersandartValue As HtmlTableRow
   

    Protected WithEvents txtName1 As TextBox
    Protected WithEvents txtName2 As TextBox
    Protected WithEvents txtStrasse As TextBox
    Protected WithEvents txtOrt As TextBox
    Protected WithEvents txtHausnummer As TextBox
    Protected WithEvents txtPostleitzahl As TextBox

    Protected WithEvents txtNameSuche As TextBox
    Protected WithEvents txtOrtSuche As TextBox
    Protected WithEvents txtPostleitzahlSuche As TextBox
    Protected WithEvents lblSuche As Label

    Private mObjBriefanforderung As Briefanforderung

#Region "Properties"


    Private Property Refferer() As String
        Get
            Dim tmpStr As String = ""
            HelpProcedures.getAppParameters(Session("AppID").ToString, tmpStr, ConfigurationManager.AppSettings.Item("Connectionstring"))
            If Not Session.Item(Me.Request.Url.LocalPath & tmpStr & "Refferer") Is Nothing Then
                Return Session.Item(Me.Request.Url.LocalPath & tmpStr & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Dim tmpStr As String = ""
            HelpProcedures.getAppParameters(Session("AppID").ToString, tmpStr, ConfigurationManager.AppSettings.Item("Connectionstring"))
            Session.Item(Me.Request.Url.LocalPath & tmpStr & "Refferer") = value
        End Set
    End Property



#End Region



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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User)
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            lblError.Text = ""

            If Not IsPostBack Then
                If Refferer Is Nothing Then
                    If Not Me.Request.UrlReferrer Is Nothing Then
                        Refferer = Me.Request.UrlReferrer.ToString
                    Else
                        Refferer = ""
                    End If
                End If
                GetAppIDFromQueryString(Me)
                Select Case Me.Request.QueryString.Item("Art").ToUpper
                    Case "ENDG"
                        lblHead.Text = "Endgültiger Dokumentenversand"
                    Case "TEMP"
                        lblHead.Text = "Temporärer Dokumentenversand"

                    Case Else
                        Throw New Exception("Fehlender Querystringparameter 'Art' Vorgang wird abgebrochen.")
                End Select
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text
            End If

            If mObjBriefanforderung Is Nothing Then
                If Session("mObjBriefanforderungSession") Is Nothing Then
                    Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
                Else
                    mObjBriefanforderung = CType(Session("mObjBriefanforderungSession"), Briefanforderung)
                End If
            End If
            'seitenbezifische PageLoads
            If Not IsPostBack Then
                ClearAddress()
                setVersandadressenberechtigung()
                mObjBriefanforderung.VersandEmpfängerArt = "Anschrift"
                fillLaenderDDL()
                fillPartneradressenDDL()
                fillGeschaeftsstellenDDL()

                fillFormular()
            End If

            checkVisibleVersandAdressArtSection()

        Catch ex As Exception
            lblError.Text = "Beim laden der Seite ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub


    Private Sub setVersandadressenberechtigung()

        'ausblenden verschiedener versandadres möglichkeiten nach gruppenLVL
        'ita 2561 Rothe
        'JJU20090128
        Select Case m_User.Groups(0).Authorizationright

            Case 0
                rb_Geschaeftsstelle.Visible = False
                rb_Partneradresse.Visible = False
                rb_Versandanschrift.Visible = False
                Throw New Exception("Sie sind zu einer Versendung nicht berechtigt. (Gruppenautorisierungslvl)")
            Case 1
                rb_Geschaeftsstelle.Visible = True
                rb_Partneradresse.Visible = False
                rb_Versandanschrift.Visible = False
            Case 2
                rb_Geschaeftsstelle.Visible = True
                rb_Partneradresse.Visible = True
                rb_Versandanschrift.Visible = False
            Case 3
                rb_Geschaeftsstelle.Visible = True
                rb_Partneradresse.Visible = True
                rb_Versandanschrift.Visible = True
        End Select


    End Sub

    Private Sub fillGeschaeftsstellenDDL()
        Dim tmpItem As ListItem

        Dim dv As New DataView(mObjBriefanforderung.VersandAdressen)
        dv.Sort = "Name1 asc"
        Dim e As Int32 = 0
        Do While e < dv.Count
            tmpItem = New ListItem(dv.Item(e)("Name1").ToString & " " & dv.Item(e)("Name2").ToString & ", " & dv.Item(e)("POST_CODE1").ToString & " " & dv.Item(e)("CITY1").ToString, dv.Item(e)("KUNNR").ToString)
            ddlGeschaeftsstelle.Items.Add(tmpItem)
            e += 1
        Loop
        tmpItem = New ListItem("- keine Auswahl -", "")
        ddlGeschaeftsstelle.Items.Insert(0, tmpItem)
        ddlGeschaeftsstelle.Visible = False
        lblSuche.Visible = True
        tblSuche.Visible = True
        lblSuche.Text = ""
    End Sub
    Private Sub ClearAddress()
        With mObjBriefanforderung
            .Street = ""
            .Name1 = ""
            .Name2 = ""
            .HouseNum = ""
            .City = ""
            .PostCode = ""
            .LaenderKuerzel = ""
            .MaterialNummer = ""
            .VersandEmpfängerArt = ""
            .Partner = ""
            .Geschaefsstelle = ""
        End With

    End Sub

    Private Sub fillPartneradressenDDL()
        Dim tmpItem As ListItem
        For Each tmpRow As DataRow In mObjBriefanforderung.PartnerAdressen.Rows
            tmpItem = New ListItem(tmpRow("SORTL").ToString & " " & tmpRow("Name1").ToString & " " & tmpRow("Name2").ToString & ", " & tmpRow("POST_CODE1").ToString & " " & tmpRow("CITY1").ToString, tmpRow("EX_KUNNR").ToString)
            ddlPartneradressen.Items.Add(tmpItem)
        Next
        tmpItem = New ListItem("- keine Auswahl -", "")
        ddlPartneradressen.Items.Insert(0, tmpItem)
    End Sub



    Private Sub lb_Weiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_weiter.Click
        doSubmit()

    End Sub


    Private Sub fillFormular()
        If Not mObjBriefanforderung.VersandEmpfängerArt = "" Then
            'also muss er schonmal das formular ausgefüllt haben. ->wiederherstellen
            With mObjBriefanforderung

                'erstmal alle unchecked setzen, da durch aspxChecked Fest wert
                'diese noch immer checked ist.
                rb_Partneradresse.Checked = False
                rb_Geschaeftsstelle.Checked = False
                rb_Versandanschrift.Checked = False

                Select Case .VersandEmpfängerArt
                    Case "Partner"
                        rb_Partneradresse.Checked = True
                    Case "Geschaeft"
                        rb_Geschaeftsstelle.Checked = True
                    Case "Anschrift"
                        rb_Versandanschrift.Checked = True
                End Select

                txtStrasse.Text = .Street
                txtName1.Text = .Name1
                txtName2.Text = .Name2
                txtHausnummer.Text = .HouseNum
                txtOrt.Text = .City
                txtPostleitzahl.Text = .PostCode
                ddlLand.SelectedValue = .LaenderKuerzel

                Select Case .MaterialNummer
                    Case "1385"
                        rb_1385.Checked = True
                    Case "1390"
                        rb_1390.Checked = True
                    Case "1391"
                        rb_1391.Checked = True
                    Case "1389"
                        rb_1389.Checked = True
                    Case "5530"
                        rb_5530.Checked = True
                End Select

                ddlGeschaeftsstelle.SelectedValue = .Geschaefsstelle
                ddlPartneradressen.SelectedValue = .Partner
                'checkVisibleVersandAdressArtSection()
            End With
        End If
    End Sub


    Private Sub checkVisibleVersandAdressArtSection()
        If rb_Geschaeftsstelle.Checked And rb_Geschaeftsstelle.Visible = True Then
            ddlGeschaeftsstelle.Visible = True
            If ddlGeschaeftsstelle.SelectedValue = "" Then
                ddlGeschaeftsstelle.Visible = False
                lblSuche.Visible = True
                tblSuche.Visible = True
            End If
            ddlPartneradressen.Visible = False
            trVersandanschriftValue.Visible = False

        ElseIf rb_Partneradresse.Checked And rb_Partneradresse.Visible = True Then
        ddlGeschaeftsstelle.Visible = False
        ddlPartneradressen.Visible = True
        trVersandanschriftValue.Visible = False
        lblSuche.Visible = False
        tblSuche.Visible = False

        ElseIf rb_Versandanschrift.Checked And rb_Versandanschrift.Visible = True Then
        ddlGeschaeftsstelle.Visible = False
        ddlPartneradressen.Visible = False
        trVersandanschriftValue.Visible = True
        lblSuche.Visible = False
        tblSuche.Visible = False

        End If
    End Sub


    Private Sub responseBack()
        Dim urlReffer As String
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            urlReffer = Refferer
            Refferer = Nothing
            Response.Redirect(urlReffer)
        End If
    End Sub

    Private Sub setVersandart()
        If rb_1385.Checked And rb_1385.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "1385"
            mObjBriefanforderung.MaterialText = rb_1385.Text
        ElseIf rb_1390.Checked And rb_1390.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "1390"
            mObjBriefanforderung.MaterialText = rb_1390.Text
        ElseIf rb_1391.Checked And rb_1391.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "1391"
            mObjBriefanforderung.MaterialText = rb_1391.Text
        ElseIf rb_1389.Checked And rb_1389.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "1389"
            mObjBriefanforderung.MaterialText = rb_1389.Text
        ElseIf rb_5530.Checked And rb_5530.Visible = True Then
            mObjBriefanforderung.MaterialNummer = "5530"
            mObjBriefanforderung.MaterialText = rb_5530.Text


        End If
    End Sub
    Private Function checkAndSetManuelleAdresseingabe() As Boolean
        If txtName1.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Name"" eingeben.<br>&nbsp;"
            Return False
        Else
            mObjBriefanforderung.Name1 = txtName1.Text.Trim(" "c)
        End If
        If txtPostleitzahl.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""PLZ"" eingeben.<br>&nbsp;"
            Return False
        Else
            If CInt(mObjBriefanforderung.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(mObjBriefanforderung.Laender.Select("Land1='" & ddlLand.SelectedItem.Value & "'")(0)("Lnplz")) = txtPostleitzahl.Text.Trim(" "c).Length Then
                    lblError.Text = "Postleitzahl hat falsche Länge."
                    Return False
                Else
                    mObjBriefanforderung.PostCode = txtPostleitzahl.Text.Trim(" "c) & " "
                End If
            End If

        End If
        If txtOrt.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Ort"" eingeben.<br>&nbsp;"
            Return False
        Else
            mObjBriefanforderung.City = txtOrt.Text.Trim(" "c)
        End If
        If txtStrasse.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Strasse"" eingeben.<br>&nbsp;"
            Return False
        Else
            mObjBriefanforderung.Street = txtStrasse.Text.Trim(" "c)
        End If
        If txtHausnummer.Text.Trim(" "c).Length = 0 Then
            lblError.Text &= "Bitte ""Hausnummer"" eingeben.<br>&nbsp;"
            Return False
        Else
            mObjBriefanforderung.HouseNum = txtHausnummer.Text.Trim(" "c)
        End If

        mObjBriefanforderung.Name2 = txtName2.Text.Trim(" "c)
        mObjBriefanforderung.LaenderKuerzel = ddlLand.SelectedItem.Value
        Return True

    End Function

    Private Sub fillLaenderDDL()
        Dim sprache As String
        If mObjBriefanforderung.Laender Is Nothing Then
            mObjBriefanforderung.getLaender(Session("AppID").ToString, Session.SessionID, Me)
        End If
        'Länder DLL füllen
        ddlLand.DataSource = mObjBriefanforderung.Laender
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


    Private Sub lb_zurueck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub doSubmit()
        If checkAndWriteEmpfaengerAndOptions() Then
            setVersandart()

            Dim Parameterlist As String = "&Art=Endg"
            Response.Redirect("Change02_3.aspx?AppID=" & Session("AppID").ToString & Parameterlist)
        End If
    End Sub

    Private Function checkAndWriteEmpfaengerAndOptions() As Boolean
        'prüfung und schreiben des Empfängers
        With mObjBriefanforderung

            If rb_Partneradresse.Checked Then
                .VersandEmpfängerArt = "Partner"
                If ddlPartneradressen.SelectedValue = "" Then
                    lblError.Text = "Wählen Sie einen Empfänger"
                    Return False
                End If
                .Partner = ddlPartneradressen.SelectedValue
            ElseIf rb_Geschaeftsstelle.Checked Then
                .VersandEmpfängerArt = "Geschaeft"
                If ddlGeschaeftsstelle.SelectedValue = "" Then
                    lblError.Text = "Wählen Sie einen Empfänger"
                    Return False
                End If
                .Geschaefsstelle = ddlGeschaeftsstelle.SelectedValue
            ElseIf rb_Versandanschrift.Checked Then
                .VersandEmpfängerArt = "Anschrift"
                If Not checkAndSetManuelleAdresseingabe() Then
                    Return False
                End If
            End If
            Return True
        End With
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lb_Suche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Suche.Click
        Dim tmpItem As ListItem

        If txtOrtSuche.Text.Length + txtPostleitzahlSuche.Text.Length + txtNameSuche.Text.Length > 0 Then

            Dim sQuery As String = ""

            If txtNameSuche.Text.Length > 0 Then
                sQuery += "Name1 LIKE '" & txtNameSuche.Text.Trim & "' AND "
            End If

            If txtOrtSuche.Text.Length > 0 Then
                sQuery += "CITY1 LIKE '" & txtOrtSuche.Text.Trim & "' AND "
            End If

            If txtPostleitzahlSuche.Text.Length > 0 Then
                sQuery += "POST_CODE1 LIKE '" & txtPostleitzahlSuche.Text.Trim & "' AND "
            End If

            sQuery = Left(sQuery, sQuery.Length - 4)


            Dim dv As New DataView(mObjBriefanforderung.VersandAdressen)
            dv.RowFilter = sQuery
            dv.Sort = "Name1 asc"
            Dim i As Int32 = 0
            ddlGeschaeftsstelle.Items.Clear()
            Do While i < dv.Count
                tmpItem = New ListItem(dv.Item(i)("Name1").ToString & " " & dv.Item(i)("Name2").ToString & ", " & dv.Item(i)("POST_CODE1").ToString & " " & dv.Item(i)("CITY1").ToString, dv.Item(i)("KUNNR").ToString)
                ddlGeschaeftsstelle.Items.Add(tmpItem)
                i += 1
            Loop
            tmpItem = New ListItem("- keine Auswahl -", "")
            ddlGeschaeftsstelle.Items.Insert(0, tmpItem)
            If dv.Count > 0 Then
                If dv.Count = 1 Then ddlGeschaeftsstelle.SelectedIndex = 1

                If ddlGeschaeftsstelle.Items.Count > 20 Then

                    ddlGeschaeftsstelle.Visible = False
                    lblSuche.Visible = True
                    tblSuche.Visible = True
                    lblSuche.Text = "Bitte über die Suchkriterien genauer eingrenzen!"

                Else
                    ddlGeschaeftsstelle.Visible = True
                    lb_NeuSuche.Visible = True
                    lblSuche.Visible = False
                    tblSuche.Visible = False
                End If
            Else
                lblSuche.ForeColor = Drawing.Color.Red
                lblSuche.Text = "Keine Geschäftstelle gefunden!"
                lblSuche.Visible = True
            End If


        Else
            txtNameSuche.BorderColor = Drawing.Color.Red
            txtOrtSuche.BorderColor = Drawing.Color.Red
            txtPostleitzahlSuche.BorderColor = Drawing.Color.Red
            lblSuche.ForeColor = Drawing.Color.Red
            lblSuche.Text = "Kein Suchkriterium gefüllt!"
        End If




    End Sub

    Protected Sub lb_NeuSuche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_NeuSuche.Click
        ddlGeschaeftsstelle.Visible = False
        lblSuche.Visible = True
        tblSuche.Visible = True
        lb_NeuSuche.Visible = False
        lblSuche.ForeColor = Drawing.Color.Black
        lblSuche.Text = ""
    End Sub
End Class
' ************************************************
' $History: Change02_2.aspx.vb $
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 24.06.09   Time: 10:00
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2939
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 23.06.09   Time: 15:05
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 16.06.09   Time: 17:07
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA:2804
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 2.06.09    Time: 14:19
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2804
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 20.05.09   Time: 17:01
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 9.03.09    Time: 17:25
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA: 2671
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 6.03.09    Time: 16:18
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 3.03.09    Time: 17:30
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' Bugfix partneradressen 
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 19.02.09   Time: 17:33
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 19.02.09   Time: 17:29
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2596
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 13.02.09   Time: 13:51
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2596 / 2589
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 12.02.09   Time: 17:40
' Created in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2596 im test
' 
' ************************************************