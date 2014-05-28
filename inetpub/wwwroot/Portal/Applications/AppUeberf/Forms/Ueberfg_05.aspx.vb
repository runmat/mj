Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Ueberfg_05
    Inherits System.Web.UI.Page

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents txtKennzeichen As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtReferenz As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents cmdSearch As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblOutput As System.Web.UI.WebControls.Label
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
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private clsUeberf As UeberfgStandard_01

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

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        If IsPostBack = False Then


            Session("Ueberf") = Nothing
            clsUeberf = Nothing

            clsUeberf = New UeberfgStandard_01(m_User, m_App, "")


            'Beaufragungsart auf "Reine Überführung" setzen
            clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ReineUeberfuehrung
            Session("Ueberf") = clsUeberf
        Else
            'Beauftragen-Button einblenden
            Me.cmdCreate.Visible = True

            clsUeberf = Session("Ueberf")
        End If

        'Wenn vorhanden: Ausgabedaten anzeigen
        FillOutput()

    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Response.Redirect("Ueberfg_01.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click

        'Felder und Werte zurücksetzen
        InitialValues()
        'Eingabefelder auf Werte überprüfen
        If CheckInputErr() = True Then Exit Sub


        'Anhand des Kennzeichens und/oder der Referenznr die Fahrzeugdaten ermitteln
        clsUeberf.SetVehicleData(m_User.KUNNR, txtKennzeichen.Text, txtReferenz.Text)

        'Fahrzeug gefunden: Ausgabedaten anzeigen
        If clsUeberf.FahrzeugVorhanden = True Then
            FillOutput()

        Else
            lblError.Text = "Das Fahrzeug wurde nicht gefunden. Möchten Sie dennoch beauftragen?"

        End If

        'Beauftragen-Button einblenden
        Me.cmdCreate.Visible = True

        'Session Variable aktualisieren
        Session("Ueberf") = clsUeberf
    End Sub

    Private Function CheckInputErr() As Boolean
        'Kennzeichen oder Referenz müssen gefüllt sein
        If Trim(Me.txtKennzeichen.Text) = String.Empty And Trim(Me.txtReferenz.Text) = String.Empty Then
            lblError.Text = "Bitte geben Sie ein Kennzeichen und/oder eine Kundenreferenz ein."
            CheckInputErr = True
        End If
    End Function

    Private Sub FillOutput()

        If clsUeberf.FahrzeugVorhanden = True Then

            Dim strOutput As String

            With clsUeberf
                'Fahrzeugdaten ausgeben
                strOutput = "Folgende Daten konnten ermittelt werden: <br>"
                strOutput = strOutput & "<TABLE id=TableAusgabe cellSpacing=1 cellPadding=1 width=400 border=0 height=23>"
                strOutput = strOutput & "<TR><TD width=130>Kennzeichen:</TD><TD>" & .Kenn1 & "-" & .Kenn2 & "</TD></TR>"
                strOutput = strOutput & "<TR><TD width=130>Hersteller/Typ:</TD><TD>" & .Herst & "</TD></TR>"
                strOutput = strOutput & "<TR><TD width=130>Fahrgestellnummer:</TD><TD>" & .Vin & "</TD></TR>"
                strOutput = strOutput & "<TR><TD width=130>Referenz:</TD><TD>" & .Ref & "</TD></TR>"
                strOutput = strOutput & "</TABLE>"

                'strOutput = strOutput & "Kennzeichen:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & .Kenn1 & "-" & .Kenn2 & "<br>"
                'strOutput = strOutput & "Hersteller/Typ:&nbsp;&nbsp;&nbsp;&nbsp;" & .Herst & "<br>"
                'strOutput = strOutput & "Fahrgestellnummer:&nbsp;" & .Vin & "<br>"
                'strOutput = strOutput & "Referenz:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & .Ref & "<br>"
            End With

            lblOutput.Text = strOutput
            lblInfo.Text = ""
        Else
            If Trim(txtKennzeichen.Text) <> String.Empty And txtKennzeichen.Text.Contains("-") Then
                clsUeberf.Kenn1 = Left(txtKennzeichen.Text, InStr(txtKennzeichen.Text, "-") - 1)
                clsUeberf.Kenn2 = Mid(txtKennzeichen.Text, InStr(txtKennzeichen.Text, "-") + 1)
            ElseIf Trim(txtReferenz.Text) <> String.Empty Then
                clsUeberf.Ref = txtReferenz.Text
            End If
        End If
    End Sub

    Private Sub InitialValues()
        Me.lblOutput.Text = ""
        With clsUeberf
            .FahrzeugVorhanden = False
            .Kenn1 = ""
            .Kenn2 = ""
            .Herst = ""
            .Vin = ""
            .Ref = ""
            .Equipment = ""
        End With
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Ueberfg_05.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 3.07.07    Time: 19:42
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' FormAuth in diverse Seiten wieder eingefügt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 12.06.07   Time: 17:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' 
' *****************  Version 1  *****************
' User: Uha          Date: 7.03.07    Time: 16:56
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Überführungs-ASPX-Seiten aus Shared-Bereich übernommen
' 
' ******************************************
