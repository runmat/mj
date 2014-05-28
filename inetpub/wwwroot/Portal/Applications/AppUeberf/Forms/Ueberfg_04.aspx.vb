Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Ueberfg_04
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private Kundennummer As String

    Private clsUeberf As UeberfgStandard_01
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents cmdBack As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblFahrzeugdaten As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbName As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbAnsprechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbTelefon As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnName As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnAnspechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnTelefon As System.Web.UI.WebControls.Label
    Protected WithEvents lblZugelassen As System.Web.UI.WebControls.Label
    Protected WithEvents lblHerst As System.Web.UI.WebControls.Label
    Protected WithEvents lblKennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents lblVin As System.Web.UI.WebControls.Label
    Protected WithEvents lblRef As System.Web.UI.WebControls.Label
    Protected WithEvents lblBereifung As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.Button
    Protected WithEvents lblSchritt As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundeAnsprechpartner As System.Web.UI.WebControls.Label
    Protected WithEvents lblKundePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents Label22 As System.Web.UI.WebControls.Label
    Protected WithEvents Label23 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Anschluss As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1FzgDaten As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Name As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReName As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Herst As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1StrNr As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Kenn As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1PLZOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2RePlzOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Ansprech As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReAnsprech As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Vin As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Telefon As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReTelefon As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Ref As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1FzgZugelassen As System.Web.UI.WebControls.Label
    Protected WithEvents lbl1Bereifung As System.Web.UI.WebControls.Label
    Protected WithEvents Label24 As System.Web.UI.WebControls.Label
    Protected WithEvents lblDatumUeberf As System.Web.UI.WebControls.Label
    Protected WithEvents lblTanken As System.Web.UI.WebControls.Label
    Protected WithEvents lblEinw As System.Web.UI.WebControls.Label
    Protected WithEvents lblBem As System.Web.UI.WebControls.Label
    Protected WithEvents Table5 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lbl2ReHerst As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReKenn As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReVin As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReRef As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReZugelassen As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReBereif As System.Web.UI.WebControls.Label
    Protected WithEvents lblWW As System.Web.UI.WebControls.Label
    Protected WithEvents Label25 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRotKenn As System.Web.UI.WebControls.Label
    Protected WithEvents Label26 As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugwert As System.Web.UI.WebControls.Label
    Protected WithEvents cmdPrint As System.Web.UI.WebControls.Button
    Protected WithEvents Label27 As System.Web.UI.WebControls.Label
    Protected WithEvents Label28 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label29 As System.Web.UI.WebControls.Label
    Protected WithEvents Label30 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbFax As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnFax As System.Web.UI.WebControls.Label
    Protected WithEvents Label31 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label32 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReFax As System.Web.UI.WebControls.Label
    Protected WithEvents Label33 As System.Web.UI.WebControls.Label
    Protected WithEvents Label34 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRegName As System.Web.UI.WebControls.Label
    Protected WithEvents Label36 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRegStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents Label37 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRegOrt As System.Web.UI.WebControls.Label
    Protected WithEvents Label38 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRechName As System.Web.UI.WebControls.Label
    Protected WithEvents Label39 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRechStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents Label41 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRechOrt As System.Web.UI.WebControls.Label
    Protected WithEvents Label43 As System.Web.UI.WebControls.Label
    Protected WithEvents cmdNewOrder As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdNewOrderHoldData As System.Web.UI.WebControls.LinkButton
    Protected WithEvents Label35 As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugklasse As System.Web.UI.WebControls.Label
    Protected WithEvents Label40 As System.Web.UI.WebControls.Label
    Protected WithEvents lblExpress As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReFahrzeugklasse As System.Web.UI.WebControls.Label
    Protected WithEvents Label44 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Auftragsnr As System.Web.UI.WebControls.Label
    Protected WithEvents lblHinZulKCL As System.Web.UI.WebControls.Label
    Protected WithEvents Label45 As System.Web.UI.WebControls.Label
    Protected WithEvents Label42 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnZulKCL As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header

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

        If IsPostBack = False Then
            GetData()
        Else
            If clsUeberf Is Nothing Then
                clsUeberf = Session("Ueberf")
            End If
        End If

        If clsUeberf.Anschluss = False Then
            Me.lblSchritt.Text = "Schritt 4 von 4"
        Else
            Me.lblSchritt.Text = "Schritt 5 von 5"
        End If

    End Sub

    Private Sub GetData()

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        With clsUeberf


            dv = Session("DataViewRG")

            If Not dv Is Nothing Then
                dv.RowFilter = "PARVW = 'RG' AND KUNNR = '" & .SelRegulierer & "'"

                .RegName = dv.Item(0)("NAME1") & " " & dv.Item(0)("NAME2")
                .RegStrasse = dv.Item(0)("STREET") & " " & dv.Item(0)("HOUSE_NUM1")
                .RegOrt = dv.Item(0)("POST_CODE1") & " " & dv.Item(0)("CITY1")

                Me.lblRegName.Text = .RegName
                Me.lblRegStrasse.Text = .RegStrasse
                Me.lblRegOrt.Text = .RegOrt

                dv.RowFilter = "PARVW = 'RE' AND KUNNR = '" & .SelRechnungsempf & "'"

                .RechName = dv.Item(0)("NAME1") & " " & dv.Item(0)("NAME2")
                .RechStrasse = dv.Item(0)("STREET") & " " & dv.Item(0)("HOUSE_NUM1")
                .RechOrt = dv.Item(0)("POST_CODE1") & " " & dv.Item(0)("CITY1")
            End If

            Me.lblRechName.Text = .RechName
            Me.lblRechStrasse.Text = .RechStrasse
            Me.lblRechOrt.Text = .RechOrt

            Me.lblKundeName1.Text = .KundeName
            Me.lblKundeStrasse.Text = .KundeStrasse
            Me.lblKundePlzOrt.Text = .KundeOrt
            Me.lblKundeAnsprechpartner.Text = .KundeAnsprechpartner

            Me.lblAbName.Text = .AbName
            Me.lblAbStrasse.Text = .AbStrasse & " " & .AbNr
            Me.lblAbOrt.Text = .AbPlz & " " & .AbOrt
            Me.lblAbAnsprechpartner.Text = .AbAnsprechpartner
            Me.lblAbTelefon.Text = .AbTelefon
            Me.lblAbTelefon2.Text = .AbTelefon2
            Me.lblAbFax.Text = .AbFax

            Me.lblAnName.Text = .AnName
            Me.lblAnStrasse.Text = .AnStrasse & " " & .AnNr
            Me.lblAnOrt.Text = .AnPlz & " " & .AnOrt
            Me.lblAnAnspechpartner.Text = .AnAnsprechpartner
            Me.lblAnTelefon.Text = .AnTelefon
            Me.lblAnTelefon2.Text = .AnTelefon2
            Me.lblAnFax.Text = .AnFax

            Me.lblHerst.Text = .Herst
            Me.lblKennzeichen.Text = .Kenn1 & "-" & .Kenn2
            Me.lblFahrzeugklasse.Text = .FahrzeugklasseTxt
            Me.lblVin.Text = .Vin
            Me.lblRef.Text = .Ref
            Me.lblBem.Text = .Bemerkung

            Me.lblZugelassen.Text = .FzgZugelassen
            Me.lblBereifung.Text = .SomWin
            Me.lblExpress.Text = .Express
            Me.lblHinZulKCL.Text = .Hin_KCL_Zulassen

            Me.lblDatumUeberf.Text = .DatumUeberf

            Me.lblFahrzeugwert.Text = .FahrzeugwertTxt

            If .Tanken = True Then
                Me.lblTanken.Text = "Ja"
            Else
                Me.lblTanken.Text = "Nein"
            End If

            If .FzgEinweisung = True Then
                Me.lblEinw.Text = "Ja"
            Else
                Me.lblEinw.Text = "Nein"
            End If

            If .Waesche = True Then
                Me.lblWW.Text = "Ja"
            Else
                Me.lblWW.Text = "Nein"
            End If

            If .RotKenn = True Then
                Me.lblRotKenn.Text = "Ja"
            Else
                Me.lblRotKenn.Text = "Nein"
            End If


            If .Anschluss = False Then
                Me.Table5.Visible = False
            Else
                lbl2ReName.Text = .ReName
                lbl2ReStrasse.Text = .ReStrasse & " " & .ReNr
                Me.lbl2RePlzOrt.Text = .RePlz & " " & .ReOrt
                Me.lbl2ReAnsprech.Text = .ReAnsprechpartner
                Me.lbl2ReTelefon.Text = .ReTelefon
                Me.lbl2ReTelefon2.Text = .ReTelefon2
                Me.lbl2ReFax.Text = .ReFax
                Me.lbl2ReHerst.Text = .ReHerst
                Me.lbl2ReKenn.Text = .ReKenn1 & "-" & .ReKenn2
                Me.lbl2ReFahrzeugklasse.Text = .ReFahrzeugklasseTxt
                Me.lbl2ReVin.Text = .ReVin
                Me.lbl2ReRef.Text = .ReRef
                Me.lbl2ReZugelassen.Text = .ReFzgZugelassen
                Me.lbl2ReBereif.Text = .ReSomWin
                'Me.lblAnZulKCL.Text = .An_KCL_Zulassen
            End If


        End With
    End Sub



    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        If clsUeberf.Vbeln1510 <> "" Then
            SetNothing

            Response.Redirect("Ueberfg_05.aspx?AppID=" & Session("AppID").ToString)
        ElseIf clsUeberf.Vbeln <> "" Then


            If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung Then
                SetNothing()
                Response.Redirect("../../AppARVAL/Forms/ChangeZulUe01.aspx?AppID=" & Session("AppID").ToString)
            ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.OffeneUeberfuehrung Then
                SetNothing()
                Response.Redirect("Ueberfg_Off01.aspx?AppID=" & Session("AppID").ToString)
            Else
                SetNothing()
                Response.Redirect("Ueberfg_00.aspx?AppID=" & Session("AppID").ToString)

            End If
            'ElseIf clsUeberf.Vbeln = "" And clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung And strerr = String.Empty Then
            '    SetNothing()
            '    Response.Redirect("../Applications/AppARVAL/Forms/ChangeZulUe01.aspx?AppID=" & Session("AppID").ToString)
        Else
            clsUeberf.Modus = 2

            Session("Ueberf") = clsUeberf
            If clsUeberf.Anschluss = False Then
                Response.Redirect("Ueberfg_02.aspx?AppID=" & Session("AppID").ToString)
            Else
                Response.Redirect("Ueberfg_03.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If

    End Sub
    
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim RetTable As DataTable
        Dim strErr As String = ""

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        If Session("NLKunnr") IsNot Nothing Then
            clsUeberf.KunnrNL = Session("NLKunnr")
        End If

        RetTable = clsUeberf.Save()

        cmdSave.Visible = False

        If clsUeberf.Vbeln = "" Then

            strErr = "Die Überführung konnte wegen eines Fehlers nicht beauftragt werden! " & clsUeberf.Message

            lblError.Text = strErr
        Else
            lblError.Text = "Ihr Auftrag wurde unter der Auftragsnummer " & clsUeberf.Vbeln.TrimStart("0"c) & " in unserem System erfasst!"
            lbl_Auftragsnr.Text = "Unsere Überführungsauftragsnr.: " & clsUeberf.Vbeln.TrimStart("0"c) & "  vom " & Today.ToShortDateString()
            lbl_Auftragsnr.Visible = True
            cmdPrint.Visible = True
            cmdBack.Visible = False
            cmdNewOrder.Visible = True
            cmdNewOrderHoldData.Visible = True
        End If

    End Sub

    Private Sub SetNothing()
        clsUeberf = Nothing
        Session("Ueberf") = Nothing
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        'Response.Write("<script>window.open('Ueberfg_Print.aspx?AppID=" & Session("AppID").ToString & "')</script>")

        Try
            Dim imageHt As New Hashtable()
            imageHt.Add("Logo", m_User.Customer.LogoImage)

            clsUeberf.getVKBueroFooter(Kundennummer)

            Dim tblData As DataTable = Base.Kernel.Common.DataTableHelper.ObjectToDataTable(clsUeberf)
            tblData.Rows(0).BeginEdit()
            Dim dr As DataRow
            dr = tblData.Rows(0)
            dr("Vbeln") = clsUeberf.Vbeln.TrimStart("0"c)
            tblData.Rows(0).EndEdit()
            tblData.Columns.Add("Datum", GetType(String))
            tblData.Rows(0).BeginEdit()
            dr = tblData.Rows(0)
            dr("Datum") = Today.ToShortDateString
            tblData.Rows(0).EndEdit()

            Dim docFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(tblData, imageHt)

            docFactory.CreateDocument(clsUeberf.Ref + "_Ueb", Me.Page, "\Applications\AppUeberf\Documents\ÜberführungStandardPortal.doc")


        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try

    End Sub


    Private Sub cmdNewOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNewOrder.Click
        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        If clsUeberf.Vbeln1510 <> "" Then
            SetNothing()

            Response.Redirect("Ueberfg_05.aspx?AppID=" & Session("AppID").ToString)
        ElseIf clsUeberf.Vbeln <> "" Then


            If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung Then
                SetNothing()
                Response.Redirect("../../AppARVAL/Forms/ChangeZulUe01.aspx?AppID=" & Session("AppID").ToString)
            ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.OffeneUeberfuehrung Then
                SetNothing()
                Response.Redirect("Ueberfg_Off01.aspx?AppID=" & Session("AppID").ToString)
            Else
                SetNothing()

                If Session("NLKunnr") Is Nothing = False Then
                    Response.Redirect("Ueberfg_00_NL.aspx?AppID=" & Session("AppID").ToString)
                Else
                    Response.Redirect("Ueberfg_00.aspx?AppID=" & Session("AppID").ToString)
                End If



            End If

            'Else
            '    clsUeberf.Modus = 2

            '    Session("Ueberf") = clsUeberf
            '    If clsUeberf.Anschluss = False Then
            '        Response.Redirect("Ueberfg_02.aspx?AppID=" & Session("AppID").ToString)
            '    Else
            '        Response.Redirect("Ueberfg_03.aspx?AppID=" & Session("AppID").ToString)
            '    End If
        End If
    End Sub

    Private Sub cmdNewOrderHoldData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNewOrderHoldData.Click
        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        If clsUeberf.Vbeln1510 <> "" Then


            Response.Redirect("Ueberfg_05.aspx?AppID=" & Session("AppID").ToString)
        ElseIf clsUeberf.Vbeln <> "" Then


            If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung Then
                Response.Redirect("../../AppARVAL/Forms/ChangeZulUe01.aspx?AppID=" & Session("AppID").ToString)
            ElseIf clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.OffeneUeberfuehrung Then
                SetNothing()
                Response.Redirect("Ueberfg_Off01.aspx?AppID=" & Session("AppID").ToString)
            Else
                clsUeberf.CleanClass()
                clsUeberf.HoldData = True
                clsUeberf.Anschluss = False
                Session("Ueberf") = clsUeberf
                Response.Redirect("Ueberfg_00.aspx?AppID=" & Session("AppID").ToString)

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
' $History: Ueberfg_04.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 16.03.11   Time: 17:11
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 7.05.09    Time: 10:32
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 5.05.09    Time: 11:55
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 4.06.08    Time: 16:42
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
' *****************  Version 18  *****************
' User: Rudolpho     Date: 15.10.07   Time: 8:25
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 23.08.07   Time: 12:57
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1246 
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 27.07.07   Time: 8:18
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Bugfixing drucken
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 11.07.07   Time: 13:08
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 11.07.07   Time: 11:19
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 11.07.07   Time: 10:37
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1126
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 11.07.07   Time: 9:20
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 11  *****************
' User: Uha          Date: 4.07.07    Time: 16:51
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Bugfixing: In Ueberfg_04 Verweis auf leeren dv = Session("DataViewRG")
' abgefangen.
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
' User: Fassbenders  Date: 20.06.07   Time: 15:47
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 19.06.07   Time: 15:11
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 13.06.07   Time: 11:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 12.06.07   Time: 17:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' *****************  Version 2  *****************
' User: Uha          Date: 5.04.07    Time: 17:04
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Verlinkung korrigiert.
' 
' *****************  Version 1  *****************
' User: Uha          Date: 7.03.07    Time: 16:56
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Überführungs-ASPX-Seiten aus Shared-Bereich übernommen
' 
' ******************************************
