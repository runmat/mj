Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Imports AppUeberf.Controls.ProgressControl

Public Class Ueberf04
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    'Private m_App As Base.Kernel.Security.App
    'Private dv As DataView
    'Private strError As String
    Private clsUeberf As Ueberf_01
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
    Protected WithEvents lblAnStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbOrt As System.Web.UI.WebControls.Label
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
    Protected WithEvents lblReBemerkung As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label29 As System.Web.UI.WebControls.Label
    Protected WithEvents lblAbTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label30 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReTelefon1 As System.Web.UI.WebControls.Label
    Protected WithEvents lbl2ReTelefon2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label31 As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblUeberfDatumFix As System.Web.UI.WebControls.Label
    Protected WithEvents lblWinterText As System.Web.UI.WebControls.Label
    Protected WithEvents Label39 As System.Web.UI.WebControls.Label
    Protected WithEvents pnlPlaceholder As System.Web.UI.WebControls.Panel
    Protected WithEvents lbl_Auftragsnr As System.Web.UI.WebControls.Label
    Protected WithEvents ProgressControl1 As Controls.ProgressControl

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

        ProgressControl1.Fill(Source.Ueber04, clsUeberf)

    End Sub

    Private Sub GetData()

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        With clsUeberf

            lblAbName.Text = .AbName
            lblAbStrasse.Text = .AbStrasse & " " & .AbNr
            lblAbOrt.Text = .AbPlz & " " & .AbOrt
            lblAbAnsprechpartner.Text = .AbAnsprechpartner
            lblAbTelefon.Text = .AbTelefon1
            lblAbTelefon2.Text = .AbTelefon2
            lblAnName.Text = .AnName
            lblAnStrasse.Text = .AnStrasse & " " & .AnNr
            lblAnOrt.Text = .AnPlz & " " & .AnOrt
            lblAnAnspechpartner.Text = .AnAnsprechpartner
            lblAnTelefon.Text = .AnTelefon1
            lblAnTelefon2.Text = .AnTelefon2

            lblHerst.Text = .Herst
            lblKennzeichen.Text = .Kenn1 & "-" & .Kenn2
            lblVin.Text = .Vin
            lblRef.Text = .Ref
            lblBem.Text = .Bemerkung

            If .FixDatumUeberfuehrung Then
                lblUeberfDatumFix.Text = "Fix"
            Else
                lblUeberfDatumFix.Text = ""
            End If
            If .WinterHandling Then
                lblWinterText.Text = .WinterText
            Else
                lblWinterText.Text = ""
            End If

            lblZugelassen.Text = .FzgZugelassen
            lblBereifung.Text = .SomWin

            lblDatumUeberf.Text = .DatumUeberf

            lblFahrzeugwert.Text = .FahrzeugwertTxt

            If .Tanken = True Then
                lblTanken.Text = "Ja"
            Else
                lblTanken.Text = "Nein"
            End If

            If .FzgEinweisung = True Then
                lblEinw.Text = "Ja"
            Else
                lblEinw.Text = "Nein"
            End If

            If .Waesche = True Then
                lblWW.Text = "Ja"
            Else
                lblWW.Text = "Nein"
            End If

            If .RotKenn = True Then
                lblRotKenn.Text = "Ja"
            Else
                lblRotKenn.Text = "Nein"
            End If


            If .Anschluss = False Then
                Table5.Visible = False
            Else
                lbl2ReName.Text = .ReName
                lbl2ReStrasse.Text = .ReStrasse & " " & .ReNr
                lbl2RePlzOrt.Text = .RePlz & " " & .ReOrt
                lbl2ReAnsprech.Text = .ReAnsprechpartner
                lbl2ReTelefon1.Text = .ReTelefon1
                lbl2ReTelefon2.Text = .ReTelefon2
                lbl2ReHerst.Text = .ReHerst
                lbl2ReKenn.Text = .ReKenn1 & "-" & .ReKenn2
                lbl2ReVin.Text = .ReVin
                lbl2ReRef.Text = .ReRef
                lblReBemerkung.Text = .ReBemerkung
                lbl2ReZugelassen.Text = .ReFzgZugelassen
                lbl2ReBereif.Text = .ReSomWin
            End If


        End With
    End Sub



    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If


        If clsUeberf.Vbeln1510 <> "" OrElse clsUeberf.Vbeln <> "" Then
            'Wenn Auftragsnummer vorhanden, dann direkt zum Anfang zurück
            SetNothing()
            Response.Redirect("UeberfZulStart.aspx?AppID=" & Session("AppID").ToString)

        Else
            'Es wurde noch kein Auftrag erzeugt
            clsUeberf.Modus = 2

            Session("Ueberf") = clsUeberf
            If clsUeberf.Anschluss = False Then
                Response.Redirect("Ueberf02.aspx?AppID=" & Session("AppID").ToString)
            Else
                Response.Redirect("Ueberf03.aspx?AppID=" & Session("AppID").ToString)
            End If
        End If

    End Sub


    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim RetTable As DataTable
        Dim strErr As String = ""

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If


        RetTable = clsUeberf.Save()

        cmdSave.Visible = False

        If clsUeberf.Vbeln = "" Then

            strErr = "Die Überführung konnte wegen eines Fehlers nicht beauftragt werden! <br>"

            lblError.Text = strErr

        Else
            lblError.Text = "Ihr Auftrag wurde unter der Auftragsnummer " & clsUeberf.Vbeln.TrimStart("0"c) & " in unserem System erfasst!"
            lbl_Auftragsnr.Text = "Unsere Überführungsauftragsnr.: " & clsUeberf.Vbeln.TrimStart("0"c) & "  vom " & Today.ToShortDateString()
            lbl_Auftragsnr.Visible = True
            cmdPrint.Visible = True
        End If


    End Sub

    Private Sub SetNothing()
        clsUeberf = Nothing
        Session("Ueberf") = Nothing
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        'Response.Write("<script>window.open('UeberfPrint.aspx?AppID=" & Session("AppID").ToString & "')</script>")
        Try
            Dim imageHt As New Hashtable()
            imageHt.Add("Logo", m_User.Customer.LogoImage)

            clsUeberf.getVKBueroFooter(m_User.KUNNR)


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

            docFactory.CreateDocument(clsUeberf.Ref + "_Ueb", Page, "\Applications\AppUeberf\Documents\ÜberführungLeasing.doc")

        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Ueberf04.aspx.vb $
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
' *****************  Version 12  *****************
' User: Rudolpho     Date: 27.07.07   Time: 9:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 27.07.07   Time: 8:18
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Bugfixing drucken
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 11.07.07   Time: 10:37
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' ITA: 1126
' 
' *****************  Version 9  *****************
' User: Uha          Date: 3.07.07    Time: 19:42
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' FormAuth in diverse Seiten wieder eingefügt
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 22.05.07   Time: 10:48
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 6  *****************
' User: Uha          Date: 21.05.07   Time: 11:47
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen im Vergleich zur Startapplikation zum Stand 11.05.2007
' 
' *****************  Version 5  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************