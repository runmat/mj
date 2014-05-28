Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Ueberfg_01
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private clsUeberf As UeberfgStandard_01
    Private Kundennummer As String

    Protected WithEvents lblKundePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeAnsprechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeName1 As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents drpAbholung As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtAbName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbAnsprechpartner As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbTelefon As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnAnsprechpartner As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnTelefon As System.Web.UI.WebControls.TextBox
    Protected WithEvents drpAnlieferung As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtAbNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents cmdRight1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents cmdBack As System.Web.UI.WebControls.ImageButton
    Protected WithEvents txtAbTelefon2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAnTelefon2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents ctrlAddressSearchAbholung As Controls.AddressSearchInputControl
    Protected WithEvents ctrlAddressSearchAnlieferung As Controls.AddressSearchInputControl

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()

        'Suchmasken mit Dropdownlisten verbinden, damit die Ergebnisse angezeigt werden
        ctrlAddressSearchAbholung.ResultDropdownList = drpAbholung
        ctrlAddressSearchAnlieferung.ResultDropdownList = drpAnlieferung

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

            If Session("NLKunnr") IsNot Nothing Then
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
            dv = Session("DataView")
        End If

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
            If clsUeberf.Modus = 1 Then
                Refill()
            End If
        Else
            If clsUeberf.Modus = 1 Or clsUeberf.HoldData = True Then
                Refill()
            End If
        End If

        ''Beaufragungsart "Reine Überführung"
        'If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ReineUeberfuehrung Or _
        '            clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung Or _
        '            clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Or _
        '            clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.UeberfuehrungKCL Then

        '    cmdBack.Visible = True
        'End If

    End Sub

    Private Sub FillControlls()
        Dim tblPartner As DataTable

        If Session("Ueberf") Is Nothing Then
            clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
        Else
            clsUeberf = Session("Ueberf")
        End If


        tblPartner = clsUeberf.getPartner(Kundennummer)

        dv = tblPartner.DefaultView
        dv.RowFilter = "TYP = 'AG'"

        With dv
            lblKundeName1.Text = .Item(0)("NAME")
            lblKundeStrasse.Text = .Item(0)("STRASSE") & " " & .Item(0)("HAUSNUMMER")
            lblKundePlzOrt.Text = .Item(0)("PLZ") & " " & .Item(0)("ORT")
            lblKundeAnsprechpartner.Text = .Item(0)("NAME2")
        End With

        dv.RowFilter = "TYP = 'ZB'"

        drpAbholung.AutoPostBack = True

        Dim e As Long

        e = 0

        With drpAbholung
            .Items.Add(New ListItem("Keine Auswahl", "0"))
            Do While e < dv.Count

                .Items.Add(New ListItem(dv.Item(e)("NAME"), dv.Item(e)("ID")))

                e = e + 1
            Loop


        End With

        e = 0

        dv.RowFilter = "TYP = 'WE'"

        drpAnlieferung.AutoPostBack = True

        With drpAnlieferung
            .Items.Add(New ListItem("Keine Auswahl", "0"))
            Do While e < dv.Count

                .Items.Add(New ListItem(dv.Item(e)("NAME"), dv.Item(e)("ID")))

                e = e + 1
            Loop


        End With


        'If (Session("DataView")) Is Nothing Then
        '    Session.Add("DataView", dv)
        'Else

        'dv.RowFilter = ""
        'Session("DataView") = dv
        Session("tblPartner") = tblPartner

        'End If

        'If (Session("Ueberf")) Is Nothing Then
        '    Session.Add("Ueberf", clsUeberf)
        'Else
        Session("Ueberf") = clsUeberf
        'End If

    End Sub

    Private Sub drpAbholung_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpAbholung.SelectedIndexChanged

        If drpAbholung.SelectedItem.Value = "" OrElse drpAbholung.SelectedItem.Value = "0" Then
            txtAbName.Text = ""
            txtAbName.Enabled = True
            txtAbStrasse.Text = ""
            txtAbStrasse.Enabled = True
            txtAbNr.Text = ""
            txtAbNr.Enabled = True
            txtAbPLZ.Text = ""
            txtAbPLZ.Enabled = True
            txtAbOrt.Text = ""
            txtAbOrt.Enabled = True
            txtAbAnsprechpartner.Text = ""
            txtAbAnsprechpartner.Enabled = True
            txtAbTelefon.Text = ""
            txtAbTelefon.Enabled = True
            txtAbTelefon2.Text = ""
            txtAbTelefon2.Enabled = True
        Else
            Dim tbl As DataSets.AddressDataSet.ADDRESSEDataTable = Session("tblPartner") 'CType(Session("DataView"), DataView).ToTable()
            Dim row As DataSets.AddressDataSet.ADDRESSERow = CType(tbl.Select("ID='" + drpAbholung.SelectedItem.Value + "'")(0), DataSets.AddressDataSet.ADDRESSERow)

            txtAbName.Text = row.NAME
            txtAbName.Enabled = False
            txtAbStrasse.Text = row.STRASSE
            txtAbStrasse.Enabled = False
            txtAbNr.Text = row.HAUSNUMMER
            txtAbNr.Enabled = False
            txtAbPLZ.Text = row.PLZ
            txtAbPLZ.Enabled = False
            txtAbOrt.Text = row.ORT
            txtAbOrt.Enabled = False
            txtAbAnsprechpartner.Text = row.NAME2
            'txtAbAnsprechpartner.Enabled = False
            txtAbTelefon.Text = row.TELEFON1
            'txtAbTelefon.Enabled = False
            txtAbTelefon2.Text = row.TELEFON2
            txtAbFax.Text = row.FAX
            clsUeberf.SelAbholung = row.KUNDENNUMMER

        End If

    End Sub

    Private Sub drpAnlieferung_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpAnlieferung.SelectedIndexChanged
        If drpAnlieferung.SelectedItem.Value = "" OrElse drpAnlieferung.SelectedItem.Value = "0" Then
            txtAnName.Text = ""
            txtAnName.Enabled = True
            txtAnStrasse.Text = ""
            txtAnStrasse.Enabled = True
            txtAnNr.Text = ""
            txtAnNr.Enabled = True
            txtAnPLZ.Text = ""
            txtAnPLZ.Enabled = True
            txtAnOrt.Text = ""
            txtAnOrt.Enabled = True
            txtAnAnsprechpartner.Text = ""
            txtAnAnsprechpartner.Enabled = True
            txtAnTelefon.Text = ""
            txtAnTelefon.Enabled = True
            txtAnTelefon2.Text = ""
            txtAnTelefon2.Enabled = True
            txtAnFax.Text = ""
        Else
            Dim tbl As DataSets.AddressDataSet.ADDRESSEDataTable = Session("tblPartner") 'CType(Session("DataView"), DataView).ToTable()
            Dim row As DataSets.AddressDataSet.ADDRESSERow = CType(tbl.Select("ID='" + drpAnlieferung.SelectedItem.Value + "'")(0), DataSets.AddressDataSet.ADDRESSERow)

            txtAnName.Text = row.NAME
            txtAnName.Enabled = False
            txtAnStrasse.Text = row.STRASSE
            txtAnStrasse.Enabled = False
            txtAnNr.Text = row.HAUSNUMMER
            txtAnNr.Enabled = False
            txtAnPLZ.Text = row.PLZ
            txtAnPLZ.Enabled = False
            txtAnOrt.Text = row.ORT
            txtAnOrt.Enabled = False
            txtAnAnsprechpartner.Text = row.NAME2
            'txtAnAnsprechpartner.Enabled = False
            txtAnTelefon.Text = row.TELEFON1
            'txtAnTelefon.Enabled = False
            txtAnTelefon2.Text = row.TELEFON2
            txtAnFax.Text = row.FAX
            clsUeberf.SelAnlieferung = row.KUNDENNUMMER
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
        If txtAbStrasse.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Straße <br>"
        End If
        If txtAbNr.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Nr. <br>"
        End If
        If txtAbPLZ.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "PLZ <br>"
        ElseIf Len(txtAbPLZ.Text) <> 5 Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Bitte geben Sie die Postleitzahl fünfstellig ein. <br>"
        ElseIf IsNumeric(txtAbPLZ.Text) = False Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Bitte geben Sie numerische Werte für die Postleitzahl ein. <br>"
        End If
        If txtAbOrt.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Ort <br>"
        End If
        If txtAbAnsprechpartner.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Ansprechpartner <br>"
        End If
        If txtAbTelefon.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Tel. <br>"
        End If

        If txtAnName.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = "Firma / Name <br>"
        End If
        If txtAnStrasse.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Straße <br>"
        End If
        If txtAnNr.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Nr. <br>"
        End If
        If txtAnPLZ.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAbholung & "PLZ <br>"
        ElseIf Len(txtAnPLZ.Text) <> 5 Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Bitte geben Sie die Postleitzahl fünfstellig ein. <br>"
        ElseIf IsNumeric(txtAnPLZ.Text) = False Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Bitte geben Sie numerische Werte für die Postleitzahl ein. <br>"
        End If
        If txtAnOrt.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Ort <br>"
        End If

        If txtAnAnsprechpartner.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Ansprechpartner <br>"
        End If

        If txtAnTelefon.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Tel. <br>"
        End If

        If booErrAbholung = True Or booErrAnlieferung = True Then

            strError = "Bitte füllen Sie alle Pflichtfelder korrekt aus. <br>"

            If booErrAbholung = True Then
                strError = strError & "Abholung:  <br>" & strErrAbholung & " <br>"
            End If

            If booErrAnlieferung = True Then
                strError = strError & "Anlieferung:  <br>" & strErrAnlieferung & " <br>"
            End If

            lblError.Text = strError

        Else
            'Daten aus der Seite in die Properties der Klasse eintragen
            With clsUeberf
                'Aktueller Kunde
                .KundeName = lblKundeName1.Text
                .KundeStrasse = lblKundeStrasse.Text
                .KundeAnsprechpartner = lblKundeAnsprechpartner.Text
                .KundeOrt = lblKundePlzOrt.Text

                'Abholadresse
                .AbName = txtAbName.Text
                .AbStrasse = txtAbStrasse.Text
                .AbNr = txtAbNr.Text
                .AbPlz = txtAbPLZ.Text
                .AbOrt = txtAbOrt.Text
                .AbAnsprechpartner = txtAbAnsprechpartner.Text
                .AbTelefon = txtAbTelefon.Text
                .AbTelefon2 = txtAbTelefon2.Text
                .AbFax = txtAbFax.Text

                'Anlieferadresse
                .AnName = txtAnName.Text
                .AnStrasse = txtAnStrasse.Text
                .AnNr = txtAnNr.Text
                .AnPlz = txtAnPLZ.Text
                .AnOrt = txtAnOrt.Text
                .AnAnsprechpartner = txtAnAnsprechpartner.Text
                .AnTelefon = txtAnTelefon.Text
                .AnTelefon2 = txtAnTelefon2.Text
                .AnFax = txtAnFax.Text

                ''Einträge in den Dropdownlists
                '.AdressStatusAbholung = drpAbholung.SelectedItem.Value
                '.AdressStatusAnlieferung = drpAnlieferung.SelectedItem.Value

                If Not txtAbName.Enabled OrElse txtAbName.ReadOnly Then
                    .AdressStatusAbholung = UeberfgStandard_01.AdressStatus.Gesperrt
                Else
                    .AdressStatusAbholung = UeberfgStandard_01.AdressStatus.Frei
                End If

                If Not txtAnName.Enabled OrElse txtAnName.ReadOnly Then
                    .AdressStatusAnlieferung = UeberfgStandard_01.AdressStatus.Gesperrt
                Else
                    .AdressStatusAnlieferung = UeberfgStandard_01.AdressStatus.Frei
                End If

                'Einträge in den Dropdownlists
                '.SelAbholung = drpAbholung.SelectedItem.Value
                '.SelAnlieferung = drpAnlieferung.SelectedItem.Value
            End With

            clsUeberf = Nothing

            Response.Redirect("Ueberfg_02.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Private Sub Refill()
        With clsUeberf

            txtAbName.Text = .AbName
            txtAbStrasse.Text = .AbStrasse
            txtAbNr.Text = .AbNr
            txtAbPLZ.Text = .AbPlz
            txtAbOrt.Text = .AbOrt
            txtAbAnsprechpartner.Text = .AbAnsprechpartner
            txtAbTelefon.Text = .AbTelefon
            txtAbTelefon2.Text = .AbTelefon2
            txtAbFax.Text = .AbFax

            If .AdressStatusAbholung <> UeberfgStandard_01.AdressStatus.Frei Then
                txtAbName.Enabled = False
                txtAbStrasse.Enabled = False
                txtAbNr.Enabled = False
                txtAbPLZ.Enabled = False
                txtAbOrt.Enabled = False
                'txtAbAnsprechpartner.Enabled = False
                'txtAbTelefon.Enabled = False

            Else
                txtAbName.Enabled = True
                txtAbStrasse.Enabled = True
                txtAbNr.Enabled = True
                txtAbPLZ.Enabled = True
                txtAbOrt.Enabled = True
                txtAbTelefon.Enabled = True
            End If

            txtAnName.Text = .AnName
            txtAnStrasse.Text = .AnStrasse
            txtAnNr.Text = .AnNr
            txtAnPLZ.Text = .AnPlz
            txtAnOrt.Text = .AnOrt
            txtAnAnsprechpartner.Text = .AnAnsprechpartner
            txtAnTelefon.Text = .AnTelefon
            txtAnTelefon2.Text = .AnTelefon2
            txtAnFax.Text = .AnFax


            If .AdressStatusAnlieferung <> UeberfgStandard_01.AdressStatus.Frei Then
                txtAnName.Enabled = False
                txtAnStrasse.Enabled = False
                txtAnNr.Enabled = False
                txtAnPLZ.Enabled = False
                txtAnOrt.Enabled = False
                'txtAnAnsprechpartner.Enabled = False
                'txtAnTelefon.Enabled = False
            Else
                txtAnName.Enabled = True
                txtAnStrasse.Enabled = True
                txtAnNr.Enabled = True
                txtAnPLZ.Enabled = True
                txtAnOrt.Enabled = True
                txtAnAnsprechpartner.Enabled = True
                txtAnTelefon.Enabled = True
            End If

            .Modus = 0
        End With



    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        ''Beaufragungsart "Reine Überführung" oder "Zulassung und Überführung"
        'If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ReineUeberfuehrung Then
        '    Response.Redirect("Ueberfg_05.aspx?AppID=" & Session("AppID").ToString)
        'ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung Then
        '    Response.Redirect("../../AppARVAL/Forms/ChangeZulUe02.aspx?AppID=" & Session("AppID").ToString)
        'ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
        '    If clsUeberf.AnName <> "" Then
        '        clsUeberf.Modus = 1
        '    End If
        '    Response.Redirect("Zulg_01.aspx?AppID=" & Session("AppID").ToString)
        'ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.UeberfuehrungKCL Then
        '    Response.Redirect("Ueberfg_ZulStart.aspx?AppID=" & Session("AppID").ToString)
        'End If
        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        clsUeberf.Modus = 1

        Session("Ueberf") = clsUeberf
        Response.Redirect("Ueberfg_00.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Ueberfg_01.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 5.05.09    Time: 11:55
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 11  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 20.06.07   Time: 16:21
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 20.06.07   Time: 15:55
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 20.06.07   Time: 15:47
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 20.06.07   Time: 10:01
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 12.06.07   Time: 17:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 5  *****************
' User: Uha          Date: 4.06.07    Time: 14:21
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Ungereimtheiten bzgl. PARVW->TYP, KUNNR->KUNDENNUMMER etc. zwischen
' Dataset und Code der *.aspx.vb-Dateien beseitigt. Richtige BAPI in lib
' Ueberf_01.vb eingetragen.
' 
' *****************  Version 4  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' *****************  Version 3  *****************
' User: Uha          Date: 5.04.07    Time: 17:04
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Verlinkung korrigiert.
' 
' *****************  Version 2  *****************
' User: Uha          Date: 5.04.07    Time: 11:14
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Verlinkung der Formulare untereinander korrigiert.
' 
' *****************  Version 1  *****************
' User: Uha          Date: 7.03.07    Time: 16:56
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Überführungs-ASPX-Seiten aus Shared-Bereich übernommen
' 
' ******************************************
