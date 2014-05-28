Option Explicit On 
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Change01_2
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label


  
    Protected WithEvents tbl_EingabeMaske As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents blubb As System.Web.UI.HtmlControls.HtmlTable

    Dim m_change As kruell_01
    Dim alControls As New ArrayList()

    Dim tmpControl As New Control()
    Dim tmpTxt As New TextBox()
    Dim tmpCb As New CheckBox()
    Dim tmpDTG As New DataGrid()
    Dim tmpLBX As New ListBox()
    Dim tmpDDL As New DropDownList()
   
    Private m_User As Base.Kernel.Security.User 'gefüllt bei GetUser()
    Protected WithEvents lb_Aktuallisieren As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lb_Anzeige As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Imagebutton5 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label05 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Table3 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Table4 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    Protected WithEvents Table5 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents Label22 As System.Web.UI.WebControls.Label
    Protected WithEvents Label23 As System.Web.UI.WebControls.Label
    Protected WithEvents Label24 As System.Web.UI.WebControls.Label
    Protected WithEvents Label25 As System.Web.UI.WebControls.Label
    Protected WithEvents Table6 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tableblabla As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Label111 As System.Web.UI.WebControls.Label
    Protected WithEvents Label27 As System.Web.UI.WebControls.Label
    Protected WithEvents Label28 As System.Web.UI.WebControls.Label
    Protected WithEvents Label29 As System.Web.UI.WebControls.Label
    Protected WithEvents Label30 As System.Web.UI.WebControls.Label
    Protected WithEvents Label31 As System.Web.UI.WebControls.Label
    Protected WithEvents Label26 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents label119 As System.Web.UI.WebControls.Label
    Protected WithEvents txt_D_KUNNR_LN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_DAT_ERF_AUFTR As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_NAME_LN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_AP_NAME1_LN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_AP_NAME2_LN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_STR_HNR_LN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_HANDY_LN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_PLZ_LN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_SONST_LN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_TEL_LN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_VERS As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_FAX_LN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_CAR_TYPE As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_ORDER_NR As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_KUNNR_LG As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_NAME_LG As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_CHASSIS_NUM As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_CAR_BESTNR As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBX_B_AUFBER_ART As System.Web.UI.WebControls.ListBox
    Protected WithEvents cb_D_CAR_TANK As System.Web.UI.WebControls.CheckBox
    Protected WithEvents TXT_B_FA_ZUSEINBAUTß1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_FA_ZUSEINBAUTß2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_FA_ZUSEINBAUTß3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_FA_ZUSEINBAUTß4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_FA_ZUSEINBAUTß5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_ZUL_ART As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_ABW_HALTERß1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_ABW_HALTERß2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_ABW_HALTERß3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_ABW_HALTERß4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_ABW_HALTERß5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_W_KENNZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents cb_D_W_KENNZ_RES As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txt_D_GEW_VERBR_DAT As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_ABW_LIEF_ADRß1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_ABW_LIEF_ADRß3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_ABW_LIEF_ADRß4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_ABW_LIEF_ADRß5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_CAR_BEM As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_CAR_RET_ADRß1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_CAR_RET_ADRß2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_CAR_RET_ADRß3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_CAR_RET_ADRß4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_CAR_RET_ADRß5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_CAR_RET_BESCH As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_LIC_NUM_RETURN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_S_BEM_SUZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_S_AUFBER_ART As System.Web.UI.WebControls.TextBox
    Protected WithEvents IBTN_S_AUFBER_ART1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IBTN_S_AUFBER_ART2 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents TXT_S_AUSSTATT As System.Web.UI.WebControls.TextBox
    Protected WithEvents IBTN_S_AUSSTATT1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IBTN_S_AUSSTATT2 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IBTN_S_AUSSTATT3 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IBTN_S_EINBAU1 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IBTN_S_EINBAU2 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents cb_D_CAR_RETURN As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txt_D_STADT_LN As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_SONST_OPT As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBX_S_EINBAU As System.Web.UI.WebControls.ListBox
    Protected WithEvents TXT_B_FA_AUFBERß1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_FA_AUFBERß2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_FA_AUFBERß3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_FA_AUFBERß4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_FA_AUFBERß5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_ABW_LIEF_ADRß2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBX_S_AUFBEREITUNGSART As System.Web.UI.WebControls.ListBox
    Protected WithEvents LBX_S_AUSSTATTUNG As System.Web.UI.WebControls.ListBox
    Protected WithEvents LBX_B_AUSSTATT As System.Web.UI.WebControls.ListBox
    Protected WithEvents DDL_S_ZULASSUNGSDIENST As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDL_S_LIEFERUNGDURCH As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DTG_S_SONDEINB_POS As System.Web.UI.WebControls.DataGrid
    Protected WithEvents TXT_S_EINBAU As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_S_AUFBER_POS As System.Web.UI.WebControls.TextBox
    Protected WithEvents xCoordHolder As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents yCoordHolder As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txt_D_ANGABEN_LG As System.Web.UI.WebControls.TextBox
    Protected WithEvents txt_D_BEM_RUECKFZG As System.Web.UI.WebControls.TextBox
    Protected WithEvents DDL_S_STANDORT As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDL_S_EINBAUFIRMA As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDL_S_FIRMA_WINTER As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXT_B_FA_WINTERRADß1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_FA_WINTERRADß2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_FA_WINTERRADß3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_FA_WINTERRADß4 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_B_FA_WINTERRADß5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXT_S_WINTERRAD_POS As System.Web.UI.WebControls.TextBox
    Protected WithEvents LBX_S_WINTERRAD_POS As System.Web.UI.WebControls.ListBox
    Protected WithEvents IBTN_S_WINTERRAD_POS As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IBTN_S_WINTERRAD_POS2 As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IBTN_S_WINTERRAD_POS_Loesch As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LBX_B_WINTERRAD_POS As System.Web.UI.WebControls.ListBox


    Private m_App As Base.Kernel.Security.App 'ist ein neues Objekt der klasse Base.Kernel.Security.App


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
        Try

            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            'objekt aus Session holen
            m_change = CType(Session("objChange"), kruell_01)



            If Not Me.IsPostBack Then
                'wenn eine zeile im DataGrid ausgewählt wurde, wird die RowID übergeben und somit ist eine Bearbeitung eines vorhandenen Datensatzes gewünscht, dessen Daten jetzt in die maske geladen werden. JJ2007.11.15
                'muss auch ausgeführt werden wenn eine neue Row Angelegt wird, um die Vorbelegungsfelder zu füllen, dabei ist Row ID-1
                'If checkRowId() >= 0 Then
                alControls.Clear()
                fillArrayListWithControls(Me.FindControl("tbl_EingabeMaske"))
                m_change.FillControlElementsOrSAPTable(checkRowId, alControls, False)

                'wenn eine eine Einbaufirma bzw eine Aubereitungsfirma aus der DDL ausgewählt wurde, die dazugehörtigen Textfelder Deaktivieren.
                'dies muss beim Wiederaufrufen des auftrags geschehen, sonst läuft das über IndexChanged der DDL JJU2008.07.04
                deaktivateAdressFields()



                'End If

                If checkRowId() = -1 Then 'wenn neuer Auftrag
                    setRequBorders()
                Else
                    Try
                        If m_change.DatenTabelle.Rows.Find(checkRowId()).Item("WEB_USER_FREIG") Is "" Or m_change.DatenTabelle.Rows.Find(checkRowId()).Item("WEB_USER_FREIG") Is " " Then
                            'wenn noch nicht freigegeben dann Requ Border setzten
                            setRequBorders()
                        End If

                        If CStr(m_change.DatenTabelle.Rows.Find(checkRowId()).Item("ABGEARB")) = "X" Then
                            Response.Write("<script language='javascript'>alert('Dieser Auftrag ist ""abgearbeitet"", es werden keine Änderungen berücksichtigt! ');</script>")
                        End If

                    Catch
                    End Try
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim laden der Seite: " & ex.Message.ToString
        End Try
    End Sub


    Private Sub deaktivateAdressFields()
        If Not DDL_S_EINBAUFIRMA.SelectedIndex = 0 Then '0=keine auswahl
            TXT_B_FA_ZUSEINBAUTß1.Enabled = False
            TXT_B_FA_ZUSEINBAUTß2.Enabled = False
            TXT_B_FA_ZUSEINBAUTß3.Enabled = False
            TXT_B_FA_ZUSEINBAUTß4.Enabled = False
            TXT_B_FA_ZUSEINBAUTß5.Enabled = False
        End If

        If Not DDL_S_STANDORT.SelectedIndex = 0 Then '0=keine auswahl
            TXT_B_FA_AUFBERß1.Enabled = False
            TXT_B_FA_AUFBERß2.Enabled = False
            TXT_B_FA_AUFBERß3.Enabled = False
            TXT_B_FA_AUFBERß4.Enabled = False
            TXT_B_FA_AUFBERß5.Enabled = False
        End If

        If Not DDL_S_FIRMA_WINTER.SelectedIndex = 0 Then '0=keine auswahl
            TXT_B_FA_WINTERRADß1.Enabled = False
            TXT_B_FA_WINTERRADß2.Enabled = False
            TXT_B_FA_WINTERRADß3.Enabled = False
            TXT_B_FA_WINTERRADß4.Enabled = False
            TXT_B_FA_WINTERRADß5.Enabled = False
        End If

    End Sub

    Private Sub setRequBorders()


        Dim width As New Unit(2)
        Dim bs As New BorderStyle()
        Dim bc As New Color()
        bc = Color.Red
        bs = BorderStyle.Solid


        With txt_D_ORDER_NR
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With

        With txt_D_KUNNR_LN
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With

        With txt_D_CHASSIS_NUM
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With

        With txt_D_NAME_LN
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With

        With txt_D_STR_HNR_LN
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With

        With txt_D_PLZ_LN
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With

        With txt_D_STADT_LN
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With

        With txt_D_CAR_TYPE
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With

        With txt_D_NAME_LG
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With
    End Sub

    Private Sub fillArrayListWithControls(ByVal mothercontrol As Control)
        'alle Contorls(txt,cb,DV, LBX) auf der seite werden in eine ArrayListe geschrieben, 
        'Dies muss jedes mal erneut passieren, weil komplette Objekte in der AL gesichert sind und keine Referenzen, 
        'so kann die AL auch nicht in einer Session abgelegt werden, weil sonst der Wert von den Objekt bezogen wird,
        ' wie Anfangs in der SessionArraylist hinzugefügt wurden JJ2007.11.15



        For Each tmpControl In mothercontrol.Controls
            'wenn das dataGrid eine zeile enthält würde es nicht in das ControlArray gesichert werden, dadurch könnte es auch nicht bei der Erfassung berücksichtigt werden. JJ2007.11.26
            If tmpControl.HasControls = True And tmpControl.GetType.Equals(tmpDTG.GetType) = False Then
                fillArrayListWithControls(tmpControl)
            Else
                If tmpControl.GetType.Equals(tmpCb.GetType()) = True OrElse tmpControl.GetType.Equals(tmpTxt.GetType()) = True OrElse tmpControl.GetType.Equals(tmpDTG.GetType) = True OrElse tmpControl.GetType.Equals(tmpLBX.GetType) = True OrElse tmpControl.GetType.Equals(tmpDDL.GetType) Then
                    System.Diagnostics.Debug.WriteLine(tmpControl.ID)
                    alControls.Add(tmpControl)
                End If

            End If
        Next
        System.Diagnostics.Debug.WriteLine(alControls.Count)
    End Sub

    Private Sub lb_Anzeige_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Anzeige.Click
        Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Function plausibilityCheck() As Boolean


        'prüfung Fahrzeugaufbereitung werte<>Adresse
        Dim EintragFirmaAufbereitung As Boolean
        Dim EintragAufbereitungen As Boolean
        If Not TXT_B_FA_AUFBERß1.Text.Trim(" "c) = "" OrElse Not TXT_B_FA_AUFBERß2.Text.Trim(" "c) = "" OrElse Not TXT_B_FA_AUFBERß3.Text.Trim(" "c) = "" OrElse Not TXT_B_FA_AUFBERß4.Text.Trim(" "c) = "" OrElse Not TXT_B_FA_AUFBERß5.Text.Trim(" "c) = "" Then
            EintragFirmaAufbereitung = True
        Else
            EintragFirmaAufbereitung = False
        End If


        If Not LBX_B_AUFBER_ART.Items.Count = 0 Then
            EintragAufbereitungen = True
        Else
            EintragAufbereitungen = False
        End If

        If Not EintragAufbereitungen = EintragFirmaAufbereitung Then
            Response.Write("<script language='javascript'>alert('Plausibilitätsprüfung (Fahrzeugaufbereitung):\n Eine Aufbereitung benötigt immer einen Aufbereitungsauftrag und eine Aufbereitungsfirma.');</script>")
            Return False
        End If

        'prüfung zusätzliche Einbauten werte<>Adresse
        Dim EintragFirmaZEinbauten As Boolean
        Dim EintragZEinbauten As Boolean

        If Not TXT_B_FA_ZUSEINBAUTß1.Text.Trim(" "c) = "" OrElse Not TXT_B_FA_ZUSEINBAUTß2.Text.Trim(" "c) = "" OrElse Not TXT_B_FA_ZUSEINBAUTß3.Text.Trim(" "c) = "" OrElse Not TXT_B_FA_ZUSEINBAUTß4.Text.Trim(" "c) = "" OrElse Not TXT_B_FA_ZUSEINBAUTß5.Text.Trim(" "c) = "" Then
            EintragFirmaZEinbauten = True
        Else
            EintragFirmaZEinbauten = False
        End If


        If Not DTG_S_SONDEINB_POS.Items.Count = 0 Then
            EintragZEinbauten = True
        Else
            EintragZEinbauten = False
        End If


        If Not EintragZEinbauten = EintragFirmaZEinbauten Then
            Response.Write("<script language='javascript'>alert('Plausibilitätsprüfung (zusätzliche Einbauten):\n Zusätzliche Einbauten benötigt immer einen Einbauauftrag und eine Einbaufirma');</script>")
            Return False
        End If


        'prüfung Winterräder werte<>Adresse
        Dim EintragFirmaWinterR As Boolean
        Dim EintragWinterR As Boolean

        If Not TXT_B_FA_WINTERRADß1.Text.Trim(" "c) = "" OrElse Not TXT_B_FA_WINTERRADß2.Text.Trim(" "c) = "" OrElse Not TXT_B_FA_WINTERRADß3.Text.Trim(" "c) = "" OrElse Not TXT_B_FA_WINTERRADß4.Text.Trim(" "c) = "" OrElse Not TXT_B_FA_WINTERRADß5.Text.Trim(" "c) = "" Then
            EintragFirmaWinterR = True
        Else
            EintragFirmaWinterR = False
        End If


        If Not LBX_B_WINTERRAD_POS.Items.Count = 0 Then
            EintragWinterR = True
        Else
            EintragWinterR = False
        End If


        If Not EintragWinterR = EintragFirmaWinterR Then
            Response.Write("<script language='javascript'>alert('Plausibilitätsprüfung (Winterräder):\n Winterräder benötigt immer eine Position und eine Firma');</script>")
            Return False
        End If



        Return True
    End Function

    Private Sub lb_aktuallisieren_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Aktuallisieren.Click

        Try


            If m_change Is Nothing Then
                m_change = CType(Session("objChange"), kruell_01)
            End If
            If plausibilityCheck() Then


                'wenn neuer Auftrag und wenn noch nicht freigegeben muss Required Fields Prüfung erfolgen JJ2007.11.29 
                If Not checkRowId() = -1 Then
                    If m_change.DatenTabelle.Rows.Find(checkRowId()).Item("WEB_USER_FREIG") Is "" Or m_change.DatenTabelle.Rows.Find(checkRowId()).Item("WEB_USER_FREIG") Is " " Then
                        'wenn auftrag nicht neu aber noch nicht freigegeben
                        If checkRequiredFields() = True Then
                            'leeren der aktuellen arrayliste 
                            alControls.Clear()
                            fillArrayListWithControls(Me.FindControl("tbl_EingabeMaske"))
                            m_change.FillControlElementsOrSAPTable(checkRowId, alControls, True)
                            Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
                        Else
                            Response.Write("<script language='javascript'>alert('Füllen Sie bitte alle benötigten Felder');</script>")
                        End If
                    Else
                        'wenn auftrag bereits freigegebeen ist
                        'leeren der aktuellen(arrayliste)
                        alControls.Clear()
                        fillArrayListWithControls(Me.FindControl("tbl_EingabeMaske"))
                        m_change.FillControlElementsOrSAPTable(checkRowId, alControls, True)
                        Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
                    End If
                Else
                    'wenn auftrag neu
                    If checkRequiredFields() = True Then
                        'leeren der aktuellen arrayliste 
                        alControls.Clear()
                        fillArrayListWithControls(Me.FindControl("tbl_EingabeMaske"))
                        m_change.FillControlElementsOrSAPTable(checkRowId, alControls, True)
                        Response.Redirect("Change01_1.aspx?AppID=" & Session("AppID").ToString)
                    Else
                        Response.Write("<script language='javascript'>alert('Füllen Sie bitte alle benötigten Felder');</script>")
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub


    Private Function checkRequiredFields() As Boolean
        If txt_D_ORDER_NR.Text Is String.Empty OrElse txt_D_KUNNR_LN.Text Is String.Empty OrElse txt_D_CHASSIS_NUM.Text Is String.Empty OrElse txt_D_NAME_LN.Text Is String.Empty OrElse txt_D_STR_HNR_LN.Text Is String.Empty OrElse txt_D_PLZ_LN.Text Is String.Empty OrElse txt_D_STADT_LN.Text Is String.Empty OrElse txt_D_CAR_TYPE.Text Is String.Empty OrElse txt_D_NAME_LG.Text Is String.Empty Then
            Return False
            Exit Function
        End If
        Return True
    End Function

    Private Function checkRowId() As Integer
        Dim rowId As Int32
        Try
            If Page.Request.QueryString.Item("SourceRow") = "" OrElse Page.Request.QueryString.Item("SourceRow") Is String.Empty Then
                rowId = -1
            Else
                rowId = CInt(Page.Request.QueryString.Item("SourceRow"))
            End If

        Catch
            rowId = -1
        End Try

        Return rowId
    End Function




    Private Sub IBTN_S_AUFBER_ART1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBTN_S_AUFBER_ART1.Click
        LBX_B_AUFBER_ART.Items.Add(TXT_S_AUFBER_ART.Text)
        TXT_S_AUFBER_ART.Text = ""
    End Sub

    Private Sub IBTN_S_AUFBER_ART2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBTN_S_AUFBER_ART2.Click
        Dim item2 As New ListItem()
        Dim item As ListItem

        For Each item2 In LBX_S_AUFBEREITUNGSART.Items
            If item2.Selected = True Then
                item = New ListItem(item2.Text, item2.Value)
                LBX_B_AUFBER_ART.Items.Add(item)
            End If
        Next
        LBX_S_AUFBEREITUNGSART.SelectedIndex = -1
    End Sub

    Private Sub IBTN_S_AUSSTATT1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBTN_S_AUSSTATT1.Click
        LBX_B_AUSSTATT.Items.Add(TXT_S_AUSSTATT.Text())
        TXT_S_AUSSTATT.Text = ""
    End Sub

    Private Sub IBTN_S_AUSSTATT2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBTN_S_AUSSTATT2.Click
        Dim item2 As New ListItem()
        Dim item As ListItem

        For Each item2 In LBX_S_AUSSTATTUNG.Items
            If item2.Selected = True Then
                item = New ListItem(item2.Text, item2.Value)
                LBX_B_AUSSTATT.Items.Add(item)
            End If

        Next
        LBX_S_AUSSTATTUNG.SelectedIndex = -1
    End Sub

    Private Sub Imagebutton5_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Imagebutton5.Click
        LBX_B_AUFBER_ART.Items.Remove(LBX_B_AUFBER_ART.SelectedItem)
    End Sub

    Private Sub IBTN_S_AUSSTATT3_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBTN_S_AUSSTATT3.Click
        LBX_B_AUSSTATT.Items.Remove(LBX_B_AUSSTATT.SelectedItem)
    End Sub

    Private Sub IBTN_S_EINBAU1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBTN_S_EINBAU1.Click
        Dim bla(1) As Object
        bla(0) = TXT_S_EINBAU.Text
        bla(1) = TXT_S_AUFBER_POS.Text
        m_change.Einbau.Rows.Add(bla)
        DTG_S_SONDEINB_POS.DataSource = m_change.Einbau
        DTG_S_SONDEINB_POS.DataBind()
        TXT_S_EINBAU.Text = ""
        TXT_S_AUFBER_POS.Text = ""
    End Sub

    Private Sub IBTN_S_EINBAU2_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBTN_S_EINBAU2.Click
        Dim bla(1) As Object
        If Not LBX_S_EINBAU.SelectedIndex = -1 Then

            bla(0) = LBX_S_EINBAU.SelectedItem.Text
            bla(1) = TXT_S_AUFBER_POS.Text
            m_change.Einbau.Rows.Add(bla)
            DTG_S_SONDEINB_POS.DataSource = m_change.Einbau
            DTG_S_SONDEINB_POS.DataBind()
            TXT_S_AUFBER_POS.Text = ""
        End If
    End Sub



    Private Sub DTG_S_SONDEINB_POS_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DTG_S_SONDEINB_POS.ItemCommand
        If m_change Is Nothing Then
            m_change = CType(Session("objChange"), kruell_01)
        End If
        m_change.Einbau.Rows.RemoveAt(e.Item.ItemIndex)
        DTG_S_SONDEINB_POS.DataSource = m_change.Einbau
        DTG_S_SONDEINB_POS.DataBind()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub DDL_S_STANDORT_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_S_STANDORT.SelectedIndexChanged
        'füllen der Adressfelder "durch Firma" mit den Werten aus der VorschlagswerteTabelle JJU2008.05.29
        Dim tmpRow As DataRow
        Dim tmpRows() As DataRow
        tmpRows = m_change.Vorschlagswerte.Select("KENNUNG='STANDORT' AND POS_KURZTEXT='" & DDL_S_STANDORT.SelectedValue() & "'")

        If Not tmpRows.Length = 1 Then
            If tmpRows.Length > 1 Then
                Throw New Exception("Es wurden mehr als eine Adresse in den Vorschlagswerten zu dem POS_KURZText: " & DDL_S_STANDORT.SelectedValue & " gefunden")
            Else
                If Not DDL_S_STANDORT.SelectedIndex = 0 Then 'index 0 = -Keine Auswahl- Felder leeren
                    Throw New Exception("Es wurden keine Adresse in den Vorschlagswerten zu dem POS_KURZText: " & DDL_S_STANDORT.SelectedValue & " gefunden")
                Else

                    TXT_B_FA_AUFBERß1.Enabled = True
                    TXT_B_FA_AUFBERß2.Enabled = True
                    TXT_B_FA_AUFBERß3.Enabled = True
                    TXT_B_FA_AUFBERß4.Enabled = True
                    TXT_B_FA_AUFBERß5.Enabled = True

                    TXT_B_FA_AUFBERß1.Text = String.Empty
                    TXT_B_FA_AUFBERß2.Text = String.Empty
                    TXT_B_FA_AUFBERß3.Text = String.Empty
                    TXT_B_FA_AUFBERß4.Text = String.Empty
                    TXT_B_FA_AUFBERß5.Text = String.Empty
                End If
            End If
        Else
            tmpRow = tmpRows(0)
            TXT_B_FA_AUFBERß1.Text = tmpRow("Name1").ToString
            TXT_B_FA_AUFBERß2.Text = tmpRow("Name2").ToString
            TXT_B_FA_AUFBERß3.Text = tmpRow("STRAS").ToString
            TXT_B_FA_AUFBERß4.Text = tmpRow("PSTLZ").ToString
            TXT_B_FA_AUFBERß5.Text = tmpRow("ORT01").ToString

            TXT_B_FA_AUFBERß1.Enabled = False
            TXT_B_FA_AUFBERß2.Enabled = False
            TXT_B_FA_AUFBERß3.Enabled = False
            TXT_B_FA_AUFBERß4.Enabled = False
            TXT_B_FA_AUFBERß5.Enabled = False

        End If

    End Sub

    Private Sub DDL_S_EINBAUFIRMA_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDL_S_EINBAUFIRMA.SelectedIndexChanged
        'füllen der Adressfelder "durch Firma" mit den Werten aus der VorschlagswerteTabelle JJU2008.05.29
        Dim tmpRow As DataRow
        Dim tmpRows() As DataRow
        tmpRows = m_change.Vorschlagswerte.Select("KENNUNG='EINBAUFIRMA' AND POS_KURZTEXT='" & DDL_S_EINBAUFIRMA.SelectedValue() & "'")

        If Not tmpRows.Length = 1 Then
            If tmpRows.Length > 1 Then
                Throw New Exception("Es wurden mehr als eine Adresse in den Vorschlagswerten zu dem POS_KURZText: " & DDL_S_EINBAUFIRMA.SelectedValue & " gefunden")
            Else
                If Not DDL_S_EINBAUFIRMA.SelectedIndex = 0 Then 'index 0 = -Keine Auswahl- Felder leeren
                    Throw New Exception("Es wurden keine Adresse in den Vorschlagswerten zu dem POS_KURZText: " & DDL_S_EINBAUFIRMA.SelectedValue & " gefunden")
                Else
                    TXT_B_FA_ZUSEINBAUTß1.Text = String.Empty
                    TXT_B_FA_ZUSEINBAUTß2.Text = String.Empty
                    TXT_B_FA_ZUSEINBAUTß3.Text = String.Empty
                    TXT_B_FA_ZUSEINBAUTß4.Text = String.Empty
                    TXT_B_FA_ZUSEINBAUTß5.Text = String.Empty

                    TXT_B_FA_ZUSEINBAUTß1.Enabled = True
                    TXT_B_FA_ZUSEINBAUTß2.Enabled = True
                    TXT_B_FA_ZUSEINBAUTß3.Enabled = True
                    TXT_B_FA_ZUSEINBAUTß4.Enabled = True
                    TXT_B_FA_ZUSEINBAUTß5.Enabled = True

                End If
            End If
        Else
            tmpRow = tmpRows(0)
            TXT_B_FA_ZUSEINBAUTß1.Text = tmpRow("Name1").ToString
            TXT_B_FA_ZUSEINBAUTß2.Text = tmpRow("Name2").ToString
            TXT_B_FA_ZUSEINBAUTß3.Text = tmpRow("STRAS").ToString
            TXT_B_FA_ZUSEINBAUTß4.Text = tmpRow("PSTLZ").ToString
            TXT_B_FA_ZUSEINBAUTß5.Text = tmpRow("ORT01").ToString

            TXT_B_FA_ZUSEINBAUTß1.Enabled = False
            TXT_B_FA_ZUSEINBAUTß2.Enabled = False
            TXT_B_FA_ZUSEINBAUTß3.Enabled = False
            TXT_B_FA_ZUSEINBAUTß4.Enabled = False
            TXT_B_FA_ZUSEINBAUTß5.Enabled = False
        End If

    End Sub

    Protected Sub DDL_S_FIRMA_WINTER_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DDL_S_FIRMA_WINTER.SelectedIndexChanged
        'füllen der Adressfelder "Firma Winterräder" mit den Werten aus der VorschlagswerteTabelle JJU2008.05.29
        Dim tmpRow As DataRow
        Dim tmpRows() As DataRow
        tmpRows = m_change.Vorschlagswerte.Select("KENNUNG='FIRMA_WINTER' AND POS_KURZTEXT='" & DDL_S_FIRMA_WINTER.SelectedValue() & "'")

        If Not tmpRows.Length = 1 Then
            If tmpRows.Length > 1 Then
                Throw New Exception("Es wurden mehr als eine Adresse in den Vorschlagswerten zu dem POS_KURZText: " & DDL_S_FIRMA_WINTER.SelectedValue & " gefunden")
            Else
                If Not DDL_S_FIRMA_WINTER.SelectedIndex = 0 Then 'index 0 = -Keine Auswahl- Felder leeren
                    Throw New Exception("Es wurden keine Adresse in den Vorschlagswerten zu dem POS_KURZText: " & DDL_S_FIRMA_WINTER.SelectedValue & " gefunden")
                Else
                    TXT_B_FA_WINTERRADß1.Text = String.Empty
                    TXT_B_FA_WINTERRADß2.Text = String.Empty
                    TXT_B_FA_WINTERRADß3.Text = String.Empty
                    TXT_B_FA_WINTERRADß4.Text = String.Empty
                    TXT_B_FA_WINTERRADß5.Text = String.Empty

                    TXT_B_FA_WINTERRADß1.Enabled = True
                    TXT_B_FA_WINTERRADß2.Enabled = True
                    TXT_B_FA_WINTERRADß3.Enabled = True
                    TXT_B_FA_WINTERRADß4.Enabled = True
                    TXT_B_FA_WINTERRADß5.Enabled = True

                End If
            End If
        Else
            tmpRow = tmpRows(0)
            TXT_B_FA_WINTERRADß1.Text = tmpRow("Name1").ToString
            TXT_B_FA_WINTERRADß2.Text = tmpRow("Name2").ToString
            TXT_B_FA_WINTERRADß3.Text = tmpRow("STRAS").ToString
            TXT_B_FA_WINTERRADß4.Text = tmpRow("PSTLZ").ToString
            TXT_B_FA_WINTERRADß5.Text = tmpRow("ORT01").ToString

            TXT_B_FA_WINTERRADß1.Enabled = False
            TXT_B_FA_WINTERRADß2.Enabled = False
            TXT_B_FA_WINTERRADß3.Enabled = False
            TXT_B_FA_WINTERRADß4.Enabled = False
            TXT_B_FA_WINTERRADß5.Enabled = False
        End If
    End Sub

    Protected Sub IBTN_S_WINTERRAD_POS_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBTN_S_WINTERRAD_POS.Click
        LBX_B_WINTERRAD_POS.Items.Add(TXT_S_WINTERRAD_POS.Text)
        TXT_S_WINTERRAD_POS.Text = ""
    End Sub

    Protected Sub IBTN_S_WINTERRAD_POS2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBTN_S_WINTERRAD_POS2.Click
        Dim item2 As New ListItem()
        Dim item As ListItem

        For Each item2 In LBX_S_WINTERRAD_POS.Items
            If item2.Selected = True Then
                item = New ListItem(item2.Text, item2.Value)
                LBX_B_WINTERRAD_POS.Items.Add(item)
            End If
        Next
        LBX_S_WINTERRAD_POS.SelectedIndex = -1
    End Sub

    Protected Sub IBTN_S_WINTERRAD_POS_Loesch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IBTN_S_WINTERRAD_POS_Loesch.Click
        LBX_B_WINTERRAD_POS.Items.Remove(LBX_B_WINTERRAD_POS.SelectedItem)
    End Sub
End Class
' ************************************************
' $History: Change01_2.aspx.vb $
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 22.09.08   Time: 14:36
' Updated in $/CKAG/Applications/AppKruell/Forms
' ITA 2129
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 21.08.08   Time: 8:39
' Updated in $/CKAG/Applications/AppKruell/Forms
' ITA 2128 fertig
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 18.08.08   Time: 12:52
' Updated in $/CKAG/Applications/AppKruell/Forms
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 16.07.08   Time: 18:15
' Updated in $/CKAG/Applications/AppKruell/Forms
' ITA 2026 Bugfixes
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 15.07.08   Time: 13:07
' Updated in $/CKAG/Applications/AppKruell/Forms
' ITA 2071
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 7.07.08    Time: 8:24
' Updated in $/CKAG/Applications/AppKruell/Forms
' ITA 2026
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 25.06.08   Time: 8:26
' Updated in $/CKAG/Applications/AppKruell/Forms
' ITA 2025
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 29.05.08   Time: 10:54
' Updated in $/CKAG/Applications/AppKruell/Forms
' ITA 1955
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 21.05.08   Time: 14:53
' Updated in $/CKAG/Applications/AppKruell/Forms
' ITA 1923
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:37
' Created in $/CKAG/Applications/AppKruell/Forms
' 
' *****************  Version 36  *****************
' User: Jungj        Date: 9.01.08    Time: 13:57
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' ITA 1580 Report01 hinzugefügt, SS History Bodys hinzugefügt
' ************************************************

