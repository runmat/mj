Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Zulg_01
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private clsUeberf As UeberfgStandard_01


    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents calVon As System.Web.UI.WebControls.Calendar
    Protected WithEvents lblKundeName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeAnsprechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents txtRef As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents txtAbgabetermin As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnVon As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cmdRight1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtHalter As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents txtHalterPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennZusatz1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennZusatz2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtKennZusatz3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnZulkreis As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents txtZulkreis As System.Web.UI.WebControls.TextBox
    Protected WithEvents ucStyles As Styles


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

        lblError.Text = ""
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try


        If IsPostBack = False Then
            FillControlls()
            GetData()
        Else
            If clsUeberf Is Nothing Then
                clsUeberf = Session("Ueberf")
            End If
            SetData()
        End If

    End Sub


    Private Sub cmdRight1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRight1.Click
        Dim booErr As Boolean
        Dim strErr As String


        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        'Daten in die Klasse schreiben
        SetData()



        With clsUeberf


            strErr = "Bitte füllen Sie alle Pflichtfelder aus: <br>"

            If .ZulHaltername = "" Then
                booErr = True
                strErr = strErr & "Haltername <br>"
            End If


            If .Kenn1 = "" Then
                booErr = True
                strErr = strErr & "Zulassungskreis <br>"
            End If

            If .Ref = "" Then
                booErr = True
                strErr = strErr & "Referenz <br>"
            End If
            If IsNothing(clsUeberf.tblKreis) OrElse clsUeberf.tblKreis.Rows.Count = 0 Then
                booErr = True
                strErr = strErr & "Für die eingegebene PLZ konnte kein Zulassungskreis ermittelt werden. <br>"
            ElseIf Me.txtZulkreis.Text <> clsUeberf.tblKreis.Rows(0)("ZKFZKZ") Then
                booErr = True
                strErr = strErr & "Die PLZ wurde geändert. <br>"
                strErr = strErr & "Bitte ermitteln Sie den Zulassungskeis erneut. <br>"
            End If



        End With


        Session("Ueberf") = clsUeberf

        If booErr = False Then
            If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungKCL Then
                Response.Redirect("Zulg_Best01.aspx?AppID=" & Session("AppID").ToString)
            Else
                Response.Redirect("Ueberfg_00.aspx?AppID=" & Session("AppID").ToString)
            End If
        Else
            Me.lblError.Text = strErr
        End If

    End Sub

    Private Sub SetData()


        With clsUeberf

            .Kenn1 = Me.txtZulkreis.Text
            .Kenn2 = Me.txtKennZusatz1.Text

            If Not Trim(Me.txtKennZusatz1.Text) = String.Empty Then
                .Wunschkennzeichen1 = Me.txtKennzeichen1.Text & "-" & Me.txtKennZusatz1.Text
            End If

            If Not Trim(Me.txtKennZusatz2.Text) = String.Empty Then
                .Wunschkennzeichen2 = Me.txtKennzeichen2.Text & "-" & Me.txtKennZusatz2.Text
            End If


            If Not Trim(Me.txtKennZusatz2.Text) = String.Empty Then
                .Wunschkennzeichen2 = Me.txtKennzeichen2.Text & "-" & Me.txtKennZusatz2.Text
            End If


            .Ref = Me.txtRef.Text
            .Zulassungsdatum = Me.txtAbgabetermin.Text
            'Vorschlagswert für die Überführung
            'Soll laut IT-Anforderung 715 nicht mehr übernommen werden
            '.DatumUeberf = Me.txtAbgabetermin.Text

            .ZulHaltername = Me.txtHalter.Text
            .ZulPLZ = Me.txtHalterPLZ.Text
            .KundeName = Me.lblKundeName1.Text
            .KundeStrasse = Me.lblKundeStrasse.Text
            .KundeOrt = Me.lblKundePlzOrt.Text
            .KundeAnsprechpartner = Me.lblKundeAnsprechpartner.Text

        End With

        Session("Ueberf") = clsUeberf

    End Sub

    Private Sub GetData()

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If


        With clsUeberf

            Me.lblKundeName1.Text = .KundeName
            Me.lblKundeStrasse.Text = .KundeStrasse
            Me.lblKundePlzOrt.Text = .KundeOrt
            Me.lblKundeAnsprechpartner.Text = .KundeAnsprechpartner
            'Me.txtKennzeichen1.Text = .Kenn1
            'Me.txtKennzeichen2.Text = .Kenn2

            Me.txtZulkreis.Text = .Kenn1
            Me.txtKennzeichen1.Text = .Kenn1
            Me.txtKennzeichen2.Text = .Kenn1
            Me.txtKennzeichen3.Text = .Kenn1

            Me.txtKennZusatz1.Text = Mid(.Wunschkennzeichen1, (Len(.Wunschkennzeichen1) + 1) - InStr(.Wunschkennzeichen1, "-"))
            Me.txtKennZusatz2.Text = Mid(.Wunschkennzeichen2, (Len(.Wunschkennzeichen2) + 1) - InStr(.Wunschkennzeichen2, "-"))
            Me.txtKennZusatz3.Text = Mid(.Wunschkennzeichen3, (Len(.Wunschkennzeichen3) + 1) - InStr(.Wunschkennzeichen3, "-"))


            Me.txtRef.Text = .Ref
            Me.txtAbgabetermin.Text = .Zulassungsdatum
            Me.txtHalter.Text = .ZulHaltername
            Me.txtHalterPLZ.Text = .ZulPLZ

        End With

    End Sub



    Private Sub FillControlls()
        Dim tblPartner As DataTable

        If Session("Ueberf") Is Nothing Then
            clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
        Else
            clsUeberf = Session("Ueberf")
        End If

        tblPartner = clsUeberf.getPartner(m_User.KUNNR)
        dv = tblPartner.DefaultView
        dv.RowFilter = "TYP = 'AG'"

        With dv
            lblKundeName1.Text = .Item(0)("NAME")
            lblKundeStrasse.Text = .Item(0)("STRASSE") & " " & .Item(0)("HAUSNUMMER")
            lblKundePlzOrt.Text = .Item(0)("PLZ") & " " & .Item(0)("ORT")
            lblKundeAnsprechpartner.Text = .Item(0)("NAME2")
        End With

        If (Session("DataView")) Is Nothing Then
            Session.Add("DataView", dv)
        Else
            Session("DataView") = dv
        End If

        If (Session("Ueberf")) Is Nothing Then
            Session.Add("Ueberf", clsUeberf)
        Else
            Session("Ueberf") = clsUeberf
        End If

    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If


        Session("Ueberf") = clsUeberf
        Response.Redirect("Ueberfg_ZulStart.aspx?AppID=" & Session("AppID").ToString)
    End Sub


    Private Sub btnVon_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVon.Click
        calVon.Visible = True
    End Sub
    Private Sub calVon_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles calVon.SelectionChanged
        txtAbgabetermin.Text = calVon.SelectedDate.ToShortDateString
        calVon.Visible = False
    End Sub


    'Private Sub drpKreis_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpKreis.SelectedIndexChanged
    '    Me.txtKennzeichen1.Text = drpKreis.SelectedItem.Value
    'End Sub

    Private Sub btnZulkreis_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZulkreis.Click
        If Len(Me.txtHalterPLZ.Text) <> 5 Then
            lblError.Text = "Bitte geben Sie eine 5-stellige PLZ ein."
        Else
            clsUeberf.getSTVA(Me.txtHalterPLZ.Text)

            If Not clsUeberf.tblKreis Is Nothing Then

                If clsUeberf.tblKreis.Rows.Count > 0 Then
                    Me.txtZulkreis.Text = clsUeberf.tblKreis.Rows(0)("ZKFZKZ")
                    Me.txtKennzeichen1.Text = clsUeberf.tblKreis.Rows(0)("ZKFZKZ")
                    Me.txtKennzeichen2.Text = clsUeberf.tblKreis.Rows(0)("ZKFZKZ")
                    Me.txtKennzeichen3.Text = clsUeberf.tblKreis.Rows(0)("ZKFZKZ")
                Else
                    Me.lblError.Text = "Für die eingegebene PLZ konnte kein Zulassungskreis ermittelt werden."
                    Me.txtZulkreis.Text = ""
                    Me.txtKennzeichen1.Text = ""
                    Me.txtKennzeichen2.Text = ""
                    Me.txtKennzeichen3.Text = ""
                End If
            End If
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Zulg_01.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 12.07.07   Time: 16:03
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' In Zulg_01 leere Zulassungskreisabfrage abgefangen
' 
' *****************  Version 7  *****************
' User: Uha          Date: 3.07.07    Time: 19:42
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' FormAuth in diverse Seiten wieder eingefügt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 21.06.07   Time: 16:27
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 19.06.07   Time: 15:11
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 4.06.07    Time: 14:21
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Ungereimtheiten bzgl. PARVW->TYP, KUNNR->KUNDENNUMMER etc. zwischen
' Dataset und Code der *.aspx.vb-Dateien beseitigt. Richtige BAPI in lib
' Ueberf_01.vb eingetragen.
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
