Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Change07_2
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdNew As System.Web.UI.WebControls.LinkButton
    Protected WithEvents drpLizLim As System.Web.UI.WebControls.DropDownList
    Protected WithEvents drpHersteller As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtPDI As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtReferenz As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFIN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtHandelsname As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLeistKW As System.Web.UI.WebControls.TextBox
    Protected WithEvents drpFarbe As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtKstLN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNameLN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnzahl As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtGeplStation As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles


    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_RegelID As String
    Protected WithEvents drpAusfuehrung As System.Web.UI.WebControls.DropDownList
    Protected WithEvents drpBereifung As System.Web.UI.WebControls.DropDownList
    Protected WithEvents drpNavi As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblErrMessage As System.Web.UI.WebControls.Label
    Protected WithEvents txtRegelname As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtModell As System.Web.UI.WebControls.TextBox
    Protected WithEvents drpAntrieb As System.Web.UI.WebControls.DropDownList
    Protected WithEvents drpVersicherer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents drpHalter As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Private clsBlock As Sixt_B15

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
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        m_App = New Base.Kernel.Security.App(m_User)

        If Not Request.QueryString("RegelID") Is Nothing Then
            m_RegelID = Request.QueryString("RegelID")
            cmdNew.Visible = False
            cmdCreate.Visible = False
            cmdBack.Visible = False
            lnkFahrzeugsuche.Visible = True
            Fill()
            DisableControls()
        Else
            If IsPostBack = False Then
                FillDropdowns()
            End If
        End If


        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillDropdowns()

        If Session("clsBlock") Is Nothing Then
            clsBlock = New Sixt_B15(m_User, m_App, "")
        Else
            clsBlock = Session("clsBlock")
        End If

        Dim tmpView As DataView
        Dim e As Long


        clsBlock.GiveAusfuehrung(Session("AppID").ToString, Session.SessionID.ToString, Me)


        '****Dropdown Ausführung füllen
        tmpView = clsBlock.Ausfuehrung.DefaultView


        e = 0

        With drpAusfuehrung
            .Items.Add(New ListItem("Keine Auswahl", "0000"))
            Do While e < tmpView.Count

                .Items.Add(New ListItem(tmpView.Item(e)("DDTEXT"), tmpView.Item(e)("DOMVALUE_L")))

                e = e + 1
            Loop

        End With



        clsBlock.GiveAntrieb(Session("AppID").ToString, Session.SessionID.ToString, Me)


        '****Dropdown Antrieb füllen
        tmpView = clsBlock.Antrieb.DefaultView


        e = 0

        With drpAntrieb
            .Items.Add(New ListItem("Keine Auswahl", "0000"))
            Do While e < tmpView.Count

                .Items.Add(New ListItem(tmpView.Item(e)("DDTEXT"), tmpView.Item(e)("DOMVALUE_L")))

                e = e + 1
            Loop

        End With



        clsBlock.GiveFarben(Session("AppID").ToString, Session.SessionID.ToString, Me)


        '****Dropdown Farben füllen
        tmpView = clsBlock.Farben.DefaultView

        e = 0

        With drpFarbe
            .Items.Add(New ListItem("Keine Auswahl", "0000"))
            Do While e < tmpView.Count

                .Items.Add(New ListItem(tmpView.Item(e)("DDTEXT"), tmpView.Item(e)("DOMVALUE_L")))

                e = e + 1
            Loop

        End With

        clsBlock.GiveNavi(Session("AppID").ToString, Session.SessionID.ToString, Me)


        '****Dropdown Navi füllen
        tmpView = clsBlock.Navi.DefaultView

        e = 0

        With drpNavi
            .Items.Add(New ListItem("Keine Auswahl", "0000"))
            Do While e < tmpView.Count

                .Items.Add(New ListItem(tmpView.Item(e)("DDTEXT"), tmpView.Item(e)("DOMVALUE_L")))

                e = e + 1
            Loop

        End With

        clsBlock.GiveReifen(Session("AppID").ToString, Session.SessionID.ToString, Me)


        '****Dropdown Bereifung füllen
        tmpView = clsBlock.Reifen.DefaultView

        e = 0

        With drpBereifung
            .Items.Add(New ListItem("Keine Auswahl", "0000"))
            Do While e < tmpView.Count

                .Items.Add(New ListItem(tmpView.Item(e)("DDTEXT"), tmpView.Item(e)("DOMVALUE_L")))

                e = e + 1
            Loop

        End With


        clsBlock.GiveHersteller(Session("AppID").ToString, Session.SessionID.ToString, Me, m_User.KUNNR)


        '****Dropdown Hersteller füllen
        tmpView = clsBlock.Hersteller.DefaultView


        tmpView.Sort = "ZZHERST_TEXT"

        e = 0

        With drpHersteller
            .Items.Add(New ListItem("Keine Auswahl", "0000"))
            Do While e < tmpView.Count

                .Items.Add(New ListItem(tmpView.Item(e)("ZZHERST_TEXT"), tmpView.Item(e)("ZZHERSTELLER_SCH")))

                e = e + 1
            Loop

        End With


        clsBlock.GiveVersicherer(Session("AppID").ToString, Session.SessionID.ToString, Me)


        '****Dropdown Versicherer füllen
        tmpView = clsBlock.Versicherer.DefaultView


        tmpView.Sort = "Name1"

        e = 0

        With drpVersicherer
            .Items.Add(New ListItem("Keine Auswahl", "0000"))
            Do While e < tmpView.Count

                .Items.Add(New ListItem(tmpView.Item(e)("Name1"), tmpView.Item(e)("ID")))

                e = e + 1
            Loop

        End With



        clsBlock.GiveHalter(Session("AppID").ToString, Session.SessionID.ToString, Me)


        '****Dropdown Halter füllen
        tmpView = clsBlock.Halter.DefaultView


        tmpView.Sort = "Name1"

        e = 0

        With drpHalter
            .Items.Add(New ListItem("Keine Auswahl", "0000"))
            Do While e < tmpView.Count

                .Items.Add(New ListItem(tmpView.Item(e)("Name1"), tmpView.Item(e)("ID")))

                e = e + 1
            Loop

        End With





        Session("clsBlock") = clsBlock

    End Sub



    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        If checkData() = True Then
            clsBlock.Save(Session("AppID").ToString, Session.SessionID.ToString, Me)

            If clsBlock.ErrMessage <> String.Empty Then
                lblError.Text = "Beim Speichern der Daten ist ein Fehler aufgetreten."
                clsBlock.ErrMessage = String.Empty
            Else
                lblError.Text = "Die Daten wurden gespeichert."
                DisableControls()
                clsBlock = Nothing
                cmdCreate.Visible = False
                cmdNew.Visible = True
            End If
        End If




    End Sub


    Private Sub Fill()

        Dim objBlocken As Sixt_B15


        objBlocken = Session("clsBlock")


        objBlocken.GiveRegel(Session("AppID").ToString, Session.SessionID.ToString, m_RegelID, Me)

        With objBlocken

            drpLizLim.SelectedItem.Selected = False

            drpLizLim.Items.FindByValue(.LizLim).Selected = True


            Me.txtRegelname.Text = .Regelname.ToString
            Me.txtPDI.Text = .PDI.ToString
            Me.txtReferenz.Text = .Referenz.ToString
            Me.txtFIN.Text = .FIN.ToString
            Me.txtHandelsname.Text = .Handelsname.ToString
            Me.txtModell.Text = .Modell.ToString
            Me.drpAntrieb.Items.Add(New ListItem(.AntriebValue.ToString, "0"))
            Me.drpHersteller.Items.Add(New ListItem(.HerstellerValue.ToString, "0"))
            Me.txtLeistKW.Text = .LeistKW.ToString
            Me.drpNavi.Items.Add(New ListItem(.NaviValue.ToString, "0"))
            Me.drpBereifung.Items.Add(New ListItem(.ReifenValue.ToString, "0"))
            Me.drpFarbe.Items.Add(New ListItem(.Farbe.ToString, "0"))
            Me.drpAusfuehrung.Items.Add(New ListItem(.AusfuehrungValue.ToString, "0"))
            Me.txtKstLN.Text = .KstLN.ToString
            Me.txtNameLN.Text = .KstName.ToString
            Me.drpVersicherer.Items.Add(New ListItem(.Versicherung.ToString, "0"))
            Me.txtAnzahl.Text = .Anzahl.ToString
            Me.drpHalter.Items.Add(New ListItem(.ZulSixt.ToString, "0"))
            Me.txtGeplStation.Text = .GeplStation.ToString


        End With

    End Sub



    Private Function checkData() As Boolean
        Dim booErr As Boolean
        Dim strErr As String


        clsBlock = Session("clsBlock")



        strErr = "Bitte füllen Sie alle Pflichtfelder aus: <br>"

        With clsBlock

            .LizLim = drpLizLim.SelectedItem.Value

            If drpLizLim.SelectedItem.Value = "Auswahl" Then
                booErr = True
                strErr = strErr & "LI/LZ <br>"
            End If

            .Regelname = Trim(txtRegelname.Text)

            If Trim(txtRegelname.Text) = String.Empty Then
                booErr = True
                strErr = strErr & "Regelname <br>"
            End If


            .PDI = Trim(txtPDI.Text)

            .Referenz = Trim(txtReferenz.Text)

            .FIN = Trim(txtFIN.Text)

            .Anzahl = Trim(txtAnzahl.Text)

            If Trim(txtFIN.Text) <> String.Empty Or (txtReferenz.Text) <> String.Empty Then

                If Trim(txtFIN.Text) <> String.Empty Then
                    If txtFIN.Text.Length < 17 Then
                        booErr = True
                        strErr = strErr & "Bitte geben Sie die FIN 17-stellig ein. <br>"
                    End If
                End If

                If Trim(txtAnzahl.Text) <> "1" Then
                    booErr = True
                    strErr = strErr & "Bei Eingabe einer FIN oder Referenz tragen Sie bitte in das Feld Anzahl eine 1 ein. <br>"
                End If
            Else
                If Trim(txtAnzahl.Text) <> String.Empty Then

                    If IsNumeric(Trim(txtAnzahl.Text)) = False Then
                        booErr = True
                        strErr = strErr & "Bitte geben Sie nur Ziffern in das Feld Anzahl ein. <br>"
                    End If
                Else

                    booErr = True
                    strErr = strErr & "Anzahl <br>"
                End If
            End If

            .Handelsname = Trim(txtHandelsname.Text)

            .Modell = Trim(txtModell.Text)

            'If Trim(txtModell.Text) = String.Empty Then
            '    booErr = True
            '    strErr = strErr & "Model-Bezeichnung <br>"
            'End If

            .AntriebValue = drpAntrieb.SelectedItem.Text

            'If Trim(txtAntrieb.Text) = String.Empty Then
            '    booErr = True
            '    strErr = strErr & "Antrieb <br>"
            'End If

            .HerstellerValue = drpHersteller.SelectedItem.Text

            If drpHersteller.SelectedItem.Value = "0000" Then
                booErr = True
                strErr = strErr & "Hersteller  <br>"
            End If

            .LeistKW = Trim(txtLeistKW.Text)

            .NaviValue = drpNavi.SelectedItem.Value

            .ReifenValue = drpBereifung.SelectedItem.Value

            .Farbe = drpFarbe.SelectedItem.Value

            .AusfuehrungValue = drpAusfuehrung.SelectedItem.Value

            .Versicherung = drpVersicherer.SelectedItem.Text

            .ZulSixt = drpHalter.SelectedItem.Text


            .KstLN = Trim(txtKstLN.Text)

            .KstName = Trim(txtNameLN.Text)

            If drpLizLim.SelectedItem.Value = "LZ" Then

                If Trim(txtKstLN.Text) = String.Empty Then
                    booErr = True
                    strErr = strErr & "Kst. LN  <br>"
                End If

                If Trim(txtNameLN.Text) = String.Empty Then
                    booErr = True
                    strErr = strErr & "Name LN  <br>"
                End If
            End If

            .GeplStation = Trim(txtGeplStation.Text)



        End With


        If booErr = False Then
            checkData = True
        Else
            lblErrMessage.Text = strErr
        End If

        Session("clsBlock") = clsBlock


    End Function

    Private Sub DisableControls()
        txtRegelname.Enabled = False
        'txtAntrieb.Enabled = False
        txtAnzahl.Enabled = False
        txtFIN.Enabled = False
        txtGeplStation.Enabled = False
        txtHandelsname.Enabled = False
        txtKstLN.Enabled = False
        txtLeistKW.Enabled = False
        txtModell.Enabled = False
        txtNameLN.Enabled = False
        txtPDI.Enabled = False
        txtReferenz.Enabled = False

        drpAntrieb.Enabled = False
        drpAusfuehrung.Enabled = False
        drpBereifung.Enabled = False
        drpFarbe.Enabled = False
        drpHersteller.Enabled = False
        drpVersicherer.Enabled = False
        drpHalter.Enabled = False
        drpLizLim.Enabled = False
        drpNavi.Enabled = False

    End Sub



    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        clsBlock = Nothing
        Response.Redirect("Change07.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
        'FillDropdowns()
        'Me.cmdNew.Visible = False
        'Me.cmdCreate.Visible = True
        Response.Redirect("Change07_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change07_2.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 2.07.07    Time: 14:09
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 2  *****************
' User: Uha          Date: 21.05.07   Time: 15:28
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Änderungen im Vergleich zur Startapplikation zum Stand 21.05.2007
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 11:23
' Created in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Änderungen aus der StartApplication zum Stand 02.05.2007 Mittags
' übernommen
' 
' ************************************************