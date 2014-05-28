Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports AppUeberf.UeberfgStandard_01

Public Class Ueberfg_03
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private clsUeberf As UeberfgStandard_01
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeAnsprechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugdaten As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdRight1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtHerstTyp As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtVin As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents txtRef As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents rdbZugelassen As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents txtAbName As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbAnsprechpartner As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents txtAbNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbTelefon As System.Web.UI.WebControls.TextBox
    Protected WithEvents rdbBereifung As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents ucStyles As Styles
    Protected WithEvents cmdBack As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents drpRetour As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents txtAbTelefon2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAbFax As System.Web.UI.WebControls.TextBox
    Protected WithEvents ucHeader As Header
    Protected WithEvents txtKennzeichen2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents rdbFahrzeugklasse As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents rdbAnZulKCL As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents TRllbAnZulKCL As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents TRrdbAnZulKCL As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ctrlAddressSearchRueckliefer As Controls.AddressSearchInputControl

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
        Me.ctrlAddressSearchRueckliefer.ResultDropdownList = Me.drpRetour

    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        If IsPostBack = False Then
            GetData()
        Else
            If clsUeberf Is Nothing Then
                clsUeberf = Session("Ueberf")
            End If
            SetData()
        End If


    End Sub

    Private Sub GetData()

        If Session("Ueberf") Is Nothing Then
            clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
        Else
            clsUeberf = Session("Ueberf")
        End If


        dv = Session("DataView")

        dv.RowFilter = "TYP = 'ZR'"
        Session(drpRetour.ID) = dv.Table 'Damit man beim Selektieren wieder auf die Daten zugreifen kann

        drpRetour.AutoPostBack = True

        Dim e As Long

        e = 0

        With drpRetour
            .Items.Add(New ListItem("Keine Auswahl", "0"))
            Do While e < dv.Count

                Dim addrRow As DataSets.AddressDataSet.ADDRESSERow = CType(dv.Item(e).Row, DataSets.AddressDataSet.ADDRESSERow)
                .Items.Add(New ListItem(addrRow.NAME + ", " + addrRow.ORT, addrRow.ID))

                e = e + 1
            Loop


        End With


        With clsUeberf

            Me.txtAbName.Text = .ReName
            Me.txtAbStrasse.Text = .ReStrasse
            Me.txtAbNr.Text = .ReNr
            Me.txtAbPLZ.Text = .RePlz
            Me.txtAbOrt.Text = .ReOrt
            Me.txtAbAnsprechpartner.Text = .ReAnsprechpartner
            Me.txtAbTelefon.Text = .ReTelefon
            Me.txtAbTelefon2.Text = .ReTelefon2
            Me.txtAbFax.Text = .ReFax

            Me.txtHerstTyp.Text = .ReHerst
            Me.txtKennzeichen1.Text = .ReKenn1
            Me.txtKennzeichen2.Text = .ReKenn2
            Me.txtVin.Text = .ReVin
            Me.txtRef.Text = .ReRef

            If .ReFzgZugelassen <> "" Then
                rdbZugelassen.Items.FindByValue(.ReFzgZugelassen).Selected = True
            End If
            If .ReSomWin <> "" Then
                rdbBereifung.Items.FindByValue(.ReSomWin).Selected = True
            End If

            If .ReFahrzeugklasse <> "" Then
                rdbFahrzeugklasse.Items.FindByValue(.ReFahrzeugklasse).Selected = True
            End If

            If Not .AdressStatusRuecklieferung = AdressStatus.Frei Then
                Me.txtAbName.Enabled = False
                Me.txtAbStrasse.Enabled = False
                Me.txtAbNr.Enabled = False
                Me.txtAbPLZ.Enabled = False
                Me.txtAbOrt.Enabled = False
                'Me.txtAbAnsprechpartner.Enabled = False
                'Me.txtAbTelefon.Enabled = False
            Else
                Me.txtAbName.Enabled = True
                Me.txtAbStrasse.Enabled = True
                Me.txtAbNr.Enabled = True
                Me.txtAbPLZ.Enabled = True
                Me.txtAbOrt.Enabled = True
                Me.txtAbAnsprechpartner.Enabled = True
                Me.txtAbTelefon.Enabled = True

            End If


        End With
    End Sub

    Private Sub SetData()

        With clsUeberf

            .ReName = Me.txtAbName.Text
            .ReStrasse = Me.txtAbStrasse.Text
            .ReNr = Me.txtAbNr.Text
            .RePlz = Me.txtAbPLZ.Text
            .ReOrt = Me.txtAbOrt.Text
            .ReAnsprechpartner = Me.txtAbAnsprechpartner.Text
            .ReTelefon = Me.txtAbTelefon.Text
            .ReTelefon2 = Me.txtAbTelefon2.Text
            .ReFax = Me.txtAbFax.Text


            .ReHerst = Me.txtHerstTyp.Text
            .ReKenn1 = Me.txtKennzeichen1.Text
            .ReKenn2 = Me.txtKennzeichen2.Text
            .ReVin = Me.txtVin.Text
            .ReRef = Me.txtRef.Text


            If Not Me.txtAbName.Enabled OrElse Me.txtAbName.ReadOnly Then
                .AdressStatusRuecklieferung = UeberfgStandard_01.AdressStatus.Gesperrt
            Else
                .AdressStatusRuecklieferung = UeberfgStandard_01.AdressStatus.Frei
            End If



        End With
    End Sub


    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        clsUeberf.Modus = 2

        Session("Ueberf") = clsUeberf
        Response.Redirect("Ueberfg_02.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub rdbZugelassen_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbZugelassen.SelectedIndexChanged
        clsUeberf.ReFzgZugelassen = rdbZugelassen.SelectedItem.Value
    End Sub

    Private Sub rdbBereifung_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbBereifung.SelectedIndexChanged
        clsUeberf.ReSomWin = rdbBereifung.SelectedItem.Value
    End Sub

    Private Sub cmdRight1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRight1.Click
        Dim booErr As Boolean
        Dim strErr As String


        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If




        strErr = "Bitte füllen Sie alle Pflichtfelder aus: <br>"

        If txtAbName.Text = "" Then
            booErr = True
            strErr = "Firma / Name <br>"
        End If
        If Me.txtAbStrasse.Text = "" Then
            booErr = True
            strErr = strErr & "Straße <br>"
        End If
        If Me.txtAbNr.Text = "" Then
            booErr = True
            strErr = strErr & "Nr. <br>"
        End If

        If txtAbPLZ.Text = "" Then
            booErr = True
            strErr = strErr & "PLZ <br>"
        ElseIf Len(txtAbPLZ.Text) <> 5 Then
            booErr = True
            strErr = strErr & "Bitte geben Sie die Postleitzahl fünfstellig ein. <br>"
        ElseIf IsNumeric(Me.txtAbPLZ.Text) = False Then
            booErr = True
            strErr = strErr & "Bitte geben Sie numerische Werte für die Postleitzahl ein. <br>"
        End If

        If Me.txtAbOrt.Text = "" Then
            booErr = True
            strErr = strErr & "Ort <br>"
        End If
        If txtAbAnsprechpartner.Text = "" Then
            booErr = True
            strErr = strErr & "Ansprechpartner <br>"
        End If
        If txtAbTelefon.Text = "" Then
            booErr = True
            strErr = strErr & "Tel. <br>"
        End If

        With clsUeberf

            If .Herst = "" Then
                booErr = True
                strErr = strErr & "Hersteller / Typ <br>"
            End If

            If .Kenn1 = "" Or .Kenn2 = "" Then
                booErr = True
                strErr = strErr & "Kennzeichen <br>"
            End If

            If .FzgZugelassen = "" Then
                booErr = True
                strErr = strErr & "Fahrzeug zugelassen und fahrbereit? <br>"
            End If

            If .ReFahrzeugklasse = "" Then
                booErr = True
                strErr = strErr & "Fahrzeugklasse "
            End If


            If .SomWin = "" Then
                booErr = True
                strErr = strErr & "Bereifung "
            End If

            'If .An_KCL_Zulassen = "" Then
            '    booErr = True
            '    strErr = strErr & "Zulassung durch KCL?  <br>"
            'End If

            .SelRetour = drpRetour.SelectedItem.Value


        End With

        SetData()

        Session("Ueberf") = clsUeberf

        If booErr = False Then
            If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
                Response.Redirect("Zulg_UebBest.aspx?AppID=" & Session("AppID").ToString)
            Else
                Response.Redirect("Ueberfg_04.aspx?AppID=" & Session("AppID").ToString)
            End If
        Else
            Me.lblError.Text = strErr
        End If
    End Sub

    Private Sub drpRetour_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpRetour.SelectedIndexChanged
        dv = Session("DataView")

        If drpRetour.SelectedIndex = -1 OrElse drpRetour.SelectedItem.Value = "" OrElse drpRetour.SelectedItem.Value = "0" Then
            Me.txtAbName.Text = ""
            Me.txtAbName.Enabled = True
            Me.txtAbStrasse.Text = ""
            Me.txtAbStrasse.Enabled = True
            Me.txtAbNr.Text = ""
            Me.txtAbNr.Enabled = True
            Me.txtAbPLZ.Text = ""
            Me.txtAbPLZ.Enabled = True
            Me.txtAbOrt.Text = ""
            Me.txtAbOrt.Enabled = True
            Me.txtAbAnsprechpartner.Text = ""
            Me.txtAbAnsprechpartner.Enabled = True
            Me.txtAbTelefon.Text = ""
            Me.txtAbTelefon.Enabled = True
            Me.txtAbTelefon2.Text = ""
            Me.txtAbFax.Text = ""

        Else
            Dim row As DataSets.AddressDataSet.ADDRESSERow = CType(CType(Session(drpRetour.ID), DataTable).Select("ID='" + drpRetour.SelectedItem.Value + "'")(0), DataSets.AddressDataSet.ADDRESSERow)
            Me.txtAbName.Text = row.NAME
            Me.txtAbName.Enabled = False
            Me.txtAbStrasse.Text = row.STRASSE
            Me.txtAbStrasse.Enabled = False
            Me.txtAbNr.Text = row.HAUSNUMMER
            Me.txtAbNr.Enabled = False
            Me.txtAbPLZ.Text = row.PLZ
            Me.txtAbPLZ.Enabled = False
            Me.txtAbOrt.Text = row.ORT
            Me.txtAbOrt.Enabled = False
            Me.txtAbAnsprechpartner.Text = row.NAME2
            'Me.txtAbAnsprechpartner.Enabled = False
            Me.txtAbTelefon.Text = row.TELEFON1
            'Me.txtAbTelefon.Enabled = False
            Me.txtAbTelefon2.Text = row.TELEFON2
            Me.txtAbFax.Text = row.FAX
        End If
    End Sub

    Private Sub Refill()
        With clsUeberf

            drpRetour.Items.FindByValue(.SelRetour).Selected = True

            Me.txtAbName.Text = .ReName
            Me.txtAbStrasse.Text = .ReStrasse
            Me.txtAbNr.Text = .ReNr
            Me.txtAbPLZ.Text = .RePlz
            Me.txtAbOrt.Text = .ReOrt
            Me.txtAbAnsprechpartner.Text = .ReAnsprechpartner
            Me.txtAbTelefon.Text = .ReTelefon

            If .SelRetour <> "0" Then
                Me.txtAbName.Enabled = False
                Me.txtAbStrasse.Enabled = False
                Me.txtAbNr.Enabled = False
                Me.txtAbPLZ.Enabled = False
                Me.txtAbOrt.Enabled = False
                'Me.txtAbAnsprechpartner.Enabled = False
                'Me.txtAbTelefon.Enabled = False
            Else
                Me.txtAbName.Enabled = True
                Me.txtAbStrasse.Enabled = True
                Me.txtAbNr.Enabled = True
                Me.txtAbPLZ.Enabled = True
                Me.txtAbOrt.Enabled = True
                Me.txtAbAnsprechpartner.Enabled = True
                Me.txtAbTelefon.Enabled = True
            End If


        End With



    End Sub

    Private Sub rdbFahrzeugklasse_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbFahrzeugklasse.SelectedIndexChanged
        clsUeberf.ReFahrzeugklasse = rdbFahrzeugklasse.SelectedItem.Value
        clsUeberf.ReFahrzeugklasseTxt = rdbFahrzeugklasse.SelectedItem.Text & " to"
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub rdbAnZulKCL_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbAnZulKCL.SelectedIndexChanged
        'clsUeberf.An_KCL_Zulassen = rdbAnZulKCL.SelectedItem.Value
    End Sub
End Class

' ************************************************
' $History: Ueberfg_03.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 29.08.07   Time: 13:51
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' "Durch KCL zulassen"  ( ITA:1246)  in der Anschlussfahrt wieder raus
' genommen
'  
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 23.08.07   Time: 12:57
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1246 
' 
' *****************  Version 10  *****************
' User: Uha          Date: 3.07.07    Time: 19:42
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' FormAuth in diverse Seiten wieder eingefügt
' 
' *****************  Version 9  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 22.06.07   Time: 16:22
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 20.06.07   Time: 16:21
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 19.06.07   Time: 15:11
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 12.06.07   Time: 17:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 4.06.07    Time: 14:21
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Ungereimtheiten bzgl. PARVW->TYP, KUNNR->KUNDENNUMMER etc. zwischen
' Dataset und Code der *.aspx.vb-Dateien beseitigt. Richtige BAPI in lib
' Ueberf_01.vb eingetragen.
' 
' *****************  Version 3  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
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
