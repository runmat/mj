Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Ueberfg_00
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeAnsprechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents txtAbName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbAnsprechpartner As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbTelefon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnAnsprechpartner As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnTelefon As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdBack As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdRight1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles


    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Protected WithEvents drpRegulierer As System.Web.UI.WebControls.DropDownList
    Protected WithEvents drpRechnungsempf As System.Web.UI.WebControls.DropDownList
    Private clsUeberf As UeberfgStandard_01
    Private Kundennummer As String

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
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Session("NLKunnr") Is Nothing = False Then
                Kundennummer = Session("NLKunnr")
            Else
                Kundennummer = m_User.KUNNR
            End If


            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        If Request.UrlReferrer.ToString.IndexOf("Selection.aspx") = 0 Then
            Session("Ueberf") = Nothing
            clsUeberf = Nothing
        End If


        If IsPostBack = False Then
            FillControlls()
        End If


        If IsNothing(dv) Then
            dv = Session("DataViewRG")
        End If

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
            If clsUeberf.Modus = 1 Then
                Refill()
            End If
        Else
            If clsUeberf.Modus = 1 Then
                Refill()
            End If
        End If

        'Beaufragungsart "Reine Überführung"
        If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ReineUeberfuehrung Or _
                    clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung Or _
                    clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Or _
                    clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.UeberfuehrungKCL Then

            cmdBack.Visible = True
        End If


    End Sub

    Private Sub FillControlls()
        Dim tblPartner As DataTable
        Dim booRegDefault As Boolean
        Dim booRechDefault As Boolean


        If Session("Ueberf") Is Nothing Then
            clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
        Else
            clsUeberf = Session("Ueberf")
        End If



        tblPartner = clsUeberf.getPartnerStandard(Kundennummer)

        dv = tblPartner.DefaultView
        dv.RowFilter = "PARVW = 'AG'"

        With dv
            If .Count > 0 Then
                Me.lblKundeName1.Text = .Item(0)("NAME1")
                Me.lblKundeStrasse.Text = .Item(0)("STREET") & " " & .Item(0)("HOUSE_NUM1")
                Me.lblKundePlzOrt.Text = .Item(0)("POST_CODE1") & " " & .Item(0)("CITY1")
                Me.lblKundeAnsprechpartner.Text = .Item(0)("NAME2")
            End If

        End With

        dv.RowFilter = "PARVW = 'RG'"

        drpRegulierer.AutoPostBack = True

        Dim e As Long

        e = 0


        Do While e < dv.Count
            If dv.Item(e)("DEFPA") = "X" Then
                booRegDefault = True
            End If

            e = e + 1
        Loop


        e = 0

        With drpRegulierer

            If booRegDefault = False Then
                .Items.Add(New ListItem("Bitte auswählen", "0"))
            End If

            Do While e < dv.Count

                .Items.Add(New ListItem(dv.Item(e)("NAME1") & ", " & dv.Item(e)("CITY1"), dv.Item(e)("KUNNR")))

                If dv.Item(e)("DEFPA") = "X" Then

                    Me.txtAbAnsprechpartner.Text = dv.Item(e)("NAME2")
                    Me.txtAbName.Text = dv.Item(e)("NAME1")
                    Me.txtAbNr.Text = dv.Item(e)("HOUSE_NUM1")
                    Me.txtAbOrt.Text = dv.Item(e)("CITY1")
                    Me.txtAbPLZ.Text = dv.Item(e)("POST_CODE1")
                    Me.txtAbStrasse.Text = dv.Item(e)("STREET")
                    Me.txtAbTelefon.Text = dv.Item(e)("TEL_NUMBER")
                End If

                e = e + 1
            Loop


        End With

        e = 0

        dv.RowFilter = "PARVW = 'RE'"

        drpRechnungsempf.AutoPostBack = True

        Do While e < dv.Count
            If dv.Item(e)("DEFPA") = "X" Then
                booRechDefault = True
            End If

            e = e + 1
        Loop

        e = 0

        With Me.drpRechnungsempf

            If booRechDefault = False Then
                .Items.Add(New ListItem("Bitte auswählen", "0"))
            End If

            Do While e < dv.Count

                .Items.Add(New ListItem(dv.Item(e)("NAME1") & ", " & dv.Item(e)("CITY1"), dv.Item(e)("KUNNR")))

                If dv.Item(e)("DEFPA") = "X" Then

                    Me.txtAnAnsprechpartner.Text = dv.Item(e)("NAME2")
                    Me.txtAnName.Text = dv.Item(e)("NAME1")
                    Me.txtAnNr.Text = dv.Item(e)("HOUSE_NUM1")
                    Me.txtAnOrt.Text = dv.Item(e)("CITY1")
                    Me.txtAnPLZ.Text = dv.Item(e)("POST_CODE1")
                    Me.txtAnStrasse.Text = dv.Item(e)("STREET")
                    Me.txtAnTelefon.Text = dv.Item(e)("TEL_NUMBER")
                End If

                e = e + 1
            Loop


        End With


        If (Session("DataViewRG")) Is Nothing Then
            Session.Add("DataViewRG", dv)
        Else
            Session("DataViewRG") = dv
        End If

        If (Session("Ueberf")) Is Nothing Then
            Session.Add("Ueberf", clsUeberf)
        Else
            Session("Ueberf") = clsUeberf
        End If

        If clsUeberf.HoldData = True Then
            Refill()
        End If

    End Sub


    Private Sub cmdRight1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRight1.Click
        Dim strErrAbholung As String = ""
        Dim strErrAnlieferung As String = ""
        Dim booErrAbholung As Boolean
        Dim booErrAnlieferung As Boolean


        If txtAbName.Text = "" Then
            booErrAbholung = True
            strErrAbholung = "Firma / Name <br>"
        End If

        If txtAnName.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = "Firma / Name <br>"
        End If


        If booErrAbholung = True Or booErrAnlieferung = True Then

            strError = "Bitte füllen Sie alle Pflichtfelder korrekt aus. <br>"

            If booErrAbholung = True Then
                strError = strError & "Regulierer:  <br>" & strErrAbholung & " <br>"
            End If

            If booErrAnlieferung = True Then
                strError = strError & "Rechnungsempfänger:  <br>" & strErrAnlieferung & " <br>"
            End If

            lblError.Text = strError

        Else
            'Daten aus der Seite in die Properties der Klasse eintragen
            With clsUeberf

                'Einträge in den Dropdownlists
                .SelRegulierer = drpRegulierer.SelectedItem.Value
                .SelRechnungsempf = drpRechnungsempf.SelectedItem.Value
            End With

            clsUeberf = Nothing

            Response.Redirect("Ueberfg_01.aspx?AppID=" & Session("AppID").ToString)
        End If


    End Sub

    Private Sub Refill()

        Dim dv As DataView


        dv = Session("DataViewRG")


        With clsUeberf

            drpRegulierer.Items.FindByValue(.SelRegulierer).Selected = True

            dv.RowFilter = "PARVW = 'RG' AND KUNNR = '" & .SelRegulierer & "'"


            Me.txtAbAnsprechpartner.Text = dv.Item(0)("NAME2")
            Me.txtAbName.Text = dv.Item(0)("NAME1")
            Me.txtAbNr.Text = dv.Item(0)("HOUSE_NUM1")
            Me.txtAbOrt.Text = dv.Item(0)("CITY1")
            Me.txtAbPLZ.Text = dv.Item(0)("POST_CODE1")
            Me.txtAbStrasse.Text = dv.Item(0)("STREET")
            Me.txtAbTelefon.Text = dv.Item(0)("TEL_NUMBER")



            drpRechnungsempf.Items.FindByValue(.SelRechnungsempf).Selected = True

            dv.RowFilter = "PARVW = 'RE' AND KUNNR = '" & .SelRegulierer & "'"


            Me.txtAnAnsprechpartner.Text = dv.Item(0)("NAME2")
            Me.txtAnName.Text = dv.Item(0)("NAME1")
            Me.txtAnNr.Text = dv.Item(0)("HOUSE_NUM1")
            Me.txtAnOrt.Text = dv.Item(0)("CITY1")
            Me.txtAnPLZ.Text = dv.Item(0)("POST_CODE1")
            Me.txtAnStrasse.Text = dv.Item(0)("STREET")
            Me.txtAnTelefon.Text = dv.Item(0)("TEL_NUMBER")


            .Modus = 0
        End With



    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        'Beaufragungsart "Reine Überführung" oder "Zulassung und Überführung"
        If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ReineUeberfuehrung Then
            Response.Redirect("Ueberfg_05.aspx?AppID=" & Session("AppID").ToString)
        ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung Then
            Response.Redirect("../Applications/AppARVAL/Forms/ChangeZulUe02.aspx?AppID=" & Session("AppID").ToString)
        ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
            If clsUeberf.AnName <> "" Then
                clsUeberf.Modus = 1
            End If
            Response.Redirect("Zulg_01.aspx?AppID=" & Session("AppID").ToString)
        ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.UeberfuehrungKCL Then
            Response.Redirect("UeberfZulStart.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Private Sub drpRegulierer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpRegulierer.SelectedIndexChanged

        If drpRegulierer.SelectedItem.Value = "0" Then
            Me.txtAbName.Text = ""
            Me.txtAbStrasse.Text = ""
            Me.txtAbNr.Text = ""
            Me.txtAbPLZ.Text = ""
            Me.txtAbOrt.Text = ""
            Me.txtAbAnsprechpartner.Text = ""
            Me.txtAbTelefon.Text = ""
        Else
            dv.RowFilter = "PARVW = 'RG' AND KUNNR = '" & drpRegulierer.SelectedItem.Value() & "'"
            Me.txtAbName.Text = dv.Item(0)("NAME1")
            Me.txtAbStrasse.Text = dv.Item(0)("STREET")
            Me.txtAbNr.Text = dv.Item(0)("HOUSE_NUM1")
            Me.txtAbPLZ.Text = dv.Item(0)("POST_CODE1")
            Me.txtAbOrt.Text = dv.Item(0)("CITY1")
            Me.txtAbAnsprechpartner.Text = dv.Item(0)("NAME2")
            Me.txtAbTelefon.Text = dv.Item(0)("TEL_NUMBER")
        End If
    End Sub

    Private Sub drpRechnungsempf_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpRechnungsempf.SelectedIndexChanged

        If drpRechnungsempf.SelectedItem.Value = "0" Then
            Me.txtAnName.Text = ""
            Me.txtAnStrasse.Text = ""
            Me.txtAnNr.Text = ""
            Me.txtAnPLZ.Text = ""
            Me.txtAnOrt.Text = ""
            Me.txtAnAnsprechpartner.Text = ""
            Me.txtAnTelefon.Text = ""
        Else
            dv.RowFilter = "PARVW = 'RE' AND KUNNR = '" & drpRechnungsempf.SelectedItem.Value() & "'"
            Me.txtAnName.Text = dv.Item(0)("NAME1")
            Me.txtAnStrasse.Text = dv.Item(0)("STREET")
            Me.txtAnNr.Text = dv.Item(0)("HOUSE_NUM1")
            Me.txtAnPLZ.Text = dv.Item(0)("POST_CODE1")
            Me.txtAnOrt.Text = dv.Item(0)("CITY1")
            Me.txtAnAnsprechpartner.Text = dv.Item(0)("NAME2")
            Me.txtAnTelefon.Text = dv.Item(0)("TEL_NUMBER")
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
