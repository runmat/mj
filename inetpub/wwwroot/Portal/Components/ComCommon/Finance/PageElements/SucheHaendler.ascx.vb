Imports CKG.Components.ComCommon.Finance
Imports CKG.Portal.PageElements

Public MustInherit Class SucheHaendler
    Inherits System.Web.UI.UserControl

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

#Region "Deklarations"
    'Dim int_MaxAnzahlErgebnisse As Int32

    Protected WithEvents ucStyles As Styles
    Private objSuche As Search
    Private m_objUser As Base.Kernel.Security.User
    Private objApp As Base.Kernel.Security.App
    Protected WithEvents lbl_ErgebnissAnzahl As System.Web.UI.WebControls.Label
    Private AppName As String
    Protected WithEvents hidden_Haendlernummer As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidden_Ort As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidden_Name1 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents lblprocessing As System.Web.UI.WebControls.Label
    Protected WithEvents hidden_Strasse As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidden_PLZ As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Tr1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_HaendlerNummer As System.Web.UI.WebControls.Label
    Protected WithEvents txtNummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtName1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtName2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbldirektInput As System.Web.UI.WebControls.Label
    Protected WithEvents lblErgebnissAnzahl As System.Web.UI.WebControls.Label
    Protected WithEvents lblwait As System.Web.UI.WebControls.Label
    Protected WithEvents lbHaendler As System.Web.UI.WebControls.ListBox
    Protected WithEvents lblHaendlerDetailsNR As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerDetailsName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerDetailsName2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerDetailsStrasse As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerDetailsPLZ As System.Web.UI.WebControls.Label
    Protected WithEvents lblHaendlerDetailsOrt As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Message As System.Web.UI.WebControls.Label
    Protected WithEvents lbl_Info As System.Web.UI.WebControls.Label
    Protected WithEvents txtAnzeigeInfo As System.Web.UI.WebControls.TextBox
    Protected WithEvents trHaendlernummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Name1 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Tr_Name2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_PLz As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Tr_Ort As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Tr_SelectionButton As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_HaendlerAuswahl As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents tr_Message As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_error As System.Web.UI.WebControls.Label
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents Tr2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lbl_ueberschrift As System.Web.UI.WebControls.Label
    Protected WithEvents lblSHistoryNR As System.Web.UI.WebControls.Label
    Protected WithEvents lblSHistoryName1 As System.Web.UI.WebControls.Label
    Protected WithEvents lblSHistoryName2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblSHistoryPLZ As System.Web.UI.WebControls.Label
    Protected WithEvents lblSHistoryOrt As System.Web.UI.WebControls.Label
    Protected WithEvents hidden_Name2 As System.Web.UI.HtmlControls.HtmlInputHidden



#End Region

#Region "Properties"

    Public ReadOnly Property giveHaendlernummer() As String
        Get
            'wenn zum ersten mal eine Auf suche des parentReport geklickt wird und eingaben für einen Händler getätigt wurden 
            If anySearchEntrys() And lbHaendler.Items.Count = 0 Then
                searchHaendlerInitial()
                If lbHaendler.Items.Count = 1 Then
                    Return lbHaendler.Items(0).Value
                Else
                    Return Nothing
                End If
                'wenn schon eine vorselektion vorliegt
            ElseIf Not lbHaendler.Items.Count = 0 Then
                If Not lbHaendler.SelectedIndex = -1 Then
                    Return lbHaendler.SelectedItem.Value
                    'wenn aus der vorselektion kein händler ausgewählt wurde
                Else
                    If lbHaendler.Items.Count = 1 Then
                        Return lbHaendler.Items(0).Value
                    Else
                        Return Nothing
                    End If
                End If
                'wenn eine Selektion ohne treffer getätigt wurde
            ElseIf lbHaendler.Items.Count = 0 Then
                Return Nothing
            Else
                Throw New Exception("keine Szenario der Händlersuche traf auf den Vorgang zu")
            End If
        End Get
    End Property

    Public Property user() As Base.Kernel.Security.User
        Get
            Return m_objUser
        End Get
        Set(ByVal Value As Base.Kernel.Security.User)
            m_objUser = Value
        End Set
    End Property
#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen

        AppName = Page.Request.Url.LocalPath
        AppName = Left(AppName, InStrRev(AppName, ".") - 1)
        AppName = Right(AppName, Len(AppName) - InStrRev(AppName, "/"))
        objApp = New Base.Kernel.Security.App(CType(Session("objUser"), Base.Kernel.Security.User))
        m_objUser = CType(Session("objUser"), Base.Kernel.Security.User)

        If Not IsPostBack Then

            If objSuche Is Nothing Then
                objSuche = New Search(objApp, m_objUser, Session.SessionID.ToString, Session("AppID").ToString)
                Session("objNewHaendlerSuche") = objSuche
            End If
            Searchhaendler()

            If lbl_error.Text.Length > 0 Then
                Exit Sub
            End If

            If objSuche.Haendler IsNot Nothing AndAlso objSuche.Haendler.Count > 0 Then
                'wenn beim initalladen das ergebniss nicht null ist, ist der maximale datenbestand schon geholt und es muss keine weitere SAP-Seitige selektion getätigt werden
                tr_HaendlerAuswahl.Visible = True
            End If
            addtheAtributes()
        Else
            If objSuche Is Nothing Then
                objSuche = CType(Session("objNewHaendlerSuche"), Search)
            End If


        End If


    End Sub

#Region "Methodes"

    Private Sub addtheAtributes()
        txtName1.Attributes.Add("onkeyup", "search(this);")
        txtName1.Attributes.Add("onkeydown", "showWaitInfo(this);")
        txtName2.Attributes.Add("onkeydown", "showWaitInfo(this);")
        txtName2.Attributes.Add("onKeyUp", "search(this);")
        txtNummer.Attributes.Add("onkeydown", "showWaitInfo(this);")
        txtNummer.Attributes.Add("onKeyUp", "search(this);")
        txtOrt.Attributes.Add("onkeydown", "showWaitInfo(this);")
        txtOrt.Attributes.Add("onKeyUp", "search(this);")
        txtPLZ.Attributes.Add("onkeydown", "showWaitInfo(this);")
        txtPLZ.Attributes.Add("onKeyUp", "search(this);")
    End Sub

    Private Sub Searchhaendler()
        'neue Klassse erstellen für BAPI Aufruf

        Try

            lbl_error.Text = ""
            lbl_Message.Text = ""
            Dim intStatus As Int32
            intStatus = objSuche.LeseHaendlerForSucheHaendlerControl(Session("AppID").ToString, Session.SessionID.ToString)

            If Not intStatus = 0 Then
                lbl_error.Text = objSuche.ErrorMessage
                Exit Sub
            End If

            lbHaendler.DataSource = objSuche.Haendler
            lbHaendler.DataTextField = "DISPLAY"
            lbHaendler.DataValueField = "REFERENZ"
            lbHaendler.DataBind()

            If objSuche.anzahlHaendlerTreffer = 0 Then
                lbl_Message.Text = "keine Ergebnisse gefunden"
                lblErgebnissAnzahl.ForeColor = Color.Red
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False
                deactivateClientSelection()
                lbl_Info.Visible = True
            ElseIf Not objSuche.anzahlHaendlerTreffer = 0 AndAlso objSuche.Haendler Is Nothing OrElse objSuche.Haendler.Count = 0 Then
                If anySearchEntrys() = True Then
                    lbl_Message.Text = "zu viele Ergebnisse gefunden (max 200 Treffer), weitere Einschränkungen benötigt "
                End If
                lblErgebnissAnzahl.ForeColor = Color.Red
                deactivateClientSelection()
                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbl_Info.Visible = True
                Else
                    lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                    lblErgebnissAnzahl.ForeColor = Color.Black
                    tr_HaendlerAuswahl.Visible = True
                    activateClientSelection()
                    txtName2.Enabled = True
                    txtName2.BackColor = Nothing
                    lbl_Info.Visible = False
                    lbl_Message.Text = ""
                End If

        Catch ex As Exception
            lbl_error.Text = "Es ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Private Sub activateClientSelection()

        hidden_Haendlernummer.Value = generateJSON_Array2(objSuche.aHaendlernummer)
        hidden_Name1.Value = generateJSON_Array2(objSuche.aName1)
        hidden_Name2.Value = generateJSON_Array2(objSuche.aName2)
        hidden_Strasse.Value = generateJSON_Array2(objSuche.aStrasse)
        hidden_Ort.Value = generateJSON_Array2(objSuche.aOrt)
        hidden_PLZ.Value = generateJSON_Array2(objSuche.aPLZ)
    End Sub

    Private Sub deactivateClientSelection()
        hidden_Haendlernummer.Value = String.Empty
        hidden_Name1.Value = String.Empty
        hidden_Name2.Value = String.Empty
        hidden_Strasse.Value = String.Empty
        hidden_Ort.Value = String.Empty
        hidden_PLZ.Value = String.Empty
    End Sub

    Private Function generateJSON_Array2(ByVal obj As Object()) As String

        Dim JSON As String = "'["
        Dim tmpObj As Object

        For Each tmpObj In obj
            JSON = JSON & """" & CStr(tmpObj) & """, "
        Next
        'letztes , entfernen
        JSON = JSON.Remove(JSON.Length - 2, 1)
        JSON = JSON & "]'"
        Return JSON
    End Function


    Public Sub SelektionZuruecksetzen()
        'Zustand wie bei initial laden wiederherstellen
        txtName1.Text = ""
        txtName2.Text = ""
        txtPLZ.Text = ""
        txtNummer.Text = ""
        txtOrt.Text = ""
        lblSHistoryName1.Text = ""
        lblSHistoryName2.Text = ""
        lblSHistoryNR.Text = ""
        lblSHistoryOrt.Text = ""
        lblSHistoryPLZ.Text = ""

        'txtName2 Deaktivieren
        txtName2.Enabled = False
        txtName2.BackColor = System.Drawing.Color.FromKnownColor(KnownColor.InactiveCaptionText)


        lbHaendler.SelectedIndex = -1

        If objSuche Is Nothing Then
            objSuche = CType(Session("objNewHaendlerSuche"), Search)
        End If

        With objSuche
            .sucheHaendlerNr = txtNummer.Text
            .sucheName1 = txtName1.Text
            .sucheName2 = txtName2.Text
            .sucheOrt = txtOrt.Text
            .suchePLZ = txtPLZ.Text
        End With

        Searchhaendler()
    End Sub



    Private Sub searchHaendlerInitial()

        If objSuche Is Nothing Then
            objSuche = CType(Session("objNewHaendlerSuche"), Search)
        End If
        With objSuche
            .sucheHaendlerNr = txtNummer.Text
            .sucheName1 = txtName1.Text
            .sucheName2 = txtName2.Text
            .sucheOrt = txtOrt.Text
            .suchePLZ = txtPLZ.Text
        End With

        Searchhaendler()
        If lbl_error.Text = "" AndAlso objSuche.anzahlHaendlerTreffer > 0 AndAlso objSuche.anzahlHaendlerTreffer <= 200 Then


            'Name 2 Textbox wieder aktivieren da ja jetzt von name1 sapseitig auch in name2 gesucht wird, clientseitig nicht möglich! JJ2008.04.23
            txtName2.Enabled = True
            txtName2.BackColor = Nothing



            'werte in SearchHistoryLabels Schreiben
            lblSHistoryName1.Text = txtName1.Text
            lblSHistoryName2.Text = txtName1.Text
            '------------------------------------
            'nein weil jetzt sap seitig in name 1 und 2 gesucht wird bei eingabe in Name 1
            'lblSHistoryName2.Text = txtName2.Text
            '-----------------------------------
            lblSHistoryNR.Text = txtNummer.Text
            lblSHistoryOrt.Text = txtOrt.Text
            lblSHistoryPLZ.Text = txtPLZ.Text



            'wenn nicht * am ende dann in historyLabel Schreiben sonst * entfernen
            If Not txtNummer.Text.Trim(" "c).Length = 0 AndAlso txtNummer.Text.IndexOf("*") < txtNummer.Text.Length - 1 Then
                txtNummer.Text = ""
            Else
                txtNummer.Text = txtNummer.Text.Replace("*", "")
            End If

            If Not txtName1.Text.Trim(" "c).Length = 0 AndAlso txtName1.Text.IndexOf("*") < txtName1.Text.Length - 1 Then
                txtName1.Text = ""
            Else
                '---------------------------------------------------------------
                'der Bapi sucht bei einer übergabe von Name1 auch in Name 2, daher muss immer die Eingabe gelöscht werden, da sonst die clientseitige Selektion nicht greift, 
                'dort geht nur die suche eine Eingabe-> ein Sucharray, da ja alle nicht zutreffenden rausgeschmissen werden. JJ2008.4.23
                'txtName1.Text = txtName1.Text.Replace("*", "")
                '---------------------------------------------------------------
                txtName1.Text = ""
            End If

            If Not txtName2.Text.Trim(" "c).Length = 0 AndAlso txtName2.Text.IndexOf("*") < txtName2.Text.Length - 1 Then
                txtName2.Text = ""
            Else
                txtName2.Text = txtName2.Text.Replace("*", "")
            End If

            If Not txtPLZ.Text.Trim(" "c).Length = 0 AndAlso txtPLZ.Text.IndexOf("*") < txtPLZ.Text.Length - 1 Then
                txtPLZ.Text = ""
            Else
                txtPLZ.Text = txtPLZ.Text.Replace("*", "")
            End If


            If Not txtOrt.Text.Trim(" "c).Length = 0 AndAlso txtOrt.Text.IndexOf("*") < txtOrt.Text.Length - 1 Then
                txtOrt.Text = ""
            Else
                txtOrt.Text = txtOrt.Text.Replace("*", "")
            End If



        End If
    End Sub

    Private Function anySearchEntrys() As Boolean

        If Not txtName1.Text.Trim(" "c) = "" OrElse Not txtName1.Text.Trim(" "c) = "" OrElse Not txtName2.Text.Trim(" "c) = "" OrElse Not txtOrt.Text.Trim(" "c) = "" OrElse Not txtPLZ.Text.Trim(" "c) = "" OrElse Not txtNummer.Text.Trim(" "c) = "" Then
            Return True
        Else
            Return False
        End If


    End Function

    Public Function isInUse() As Boolean

        If Not anySearchEntrys() And lbHaendler.Items.Count = 0 Then
            Return False
        Else
            Return True

        End If

    End Function
    Public Function NotInUseButinBorder() As Boolean

        If Not anySearchEntrys() AndAlso lbHaendler.Items.Count > 0 AndAlso lbHaendler.Items.Count < 200 Then
            Return True
        Else
            Return False

        End If
    End Function
#End Region




End Class
' ************************************************
' $History: SucheHaendler.ascx.vb $
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 10.07.09   Time: 17:09
' Updated in $/CKAG/Components/ComCommon/Finance/PageElements
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance/PageElements
' ITa 2918 testfertig
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 30.07.08   Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance/PageElements
' ITA 2119
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 30.07.08   Time: 9:52
' Updated in $/CKAG/Components/ComCommon/Finance/PageElements
' ITA 2119
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 29.07.08   Time: 13:09
' Updated in $/CKAG/Components/ComCommon/Finance/PageElements
' ITA 2119 Prototyp fertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 29.07.08   Time: 9:57
' Updated in $/CKAG/Components/ComCommon/Finance/PageElements
' ITA 2119 Änderungen
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 25.07.08   Time: 15:37
' Updated in $/CKAG/Components/ComCommon/Finance/PageElements
' ITA 2119 Prototyp
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 23.05.08   Time: 15:24
' Updated in $/CKAG/Components/ComCommon/Finance/PageElements
' Bugfixing RTFS Bank Portal
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 24.04.08   Time: 8:39
' Updated in $/CKAG/Components/ComCommon/Finance/PageElements
' Migration AKF-Entwicklungen
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 23.04.08   Time: 9:44
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' ITA 1850
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 8.04.08    Time: 9:08
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 8.04.08    Time: 8:30
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' HänderSuchControl fertig
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 2.04.08    Time: 13:44
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 2.04.08    Time: 13:07
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' ITA 1758
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 20.03.08   Time: 16:07
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' ITA 1758
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 20.03.08   Time: 8:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 13.03.08   Time: 16:28
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 11.03.08   Time: 8:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 11.03.08   Time: 7:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 11.03.08   Time: 7:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 7.03.08    Time: 13:56
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/PageElements
' UserControl zur HändlerSuche
' 
' ************************************************

