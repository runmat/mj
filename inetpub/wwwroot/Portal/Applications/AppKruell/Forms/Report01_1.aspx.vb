Option Explicit On 
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Report01_1
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles


    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Auswahl As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblnodata As System.Web.UI.WebControls.Label
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lbl_SAPResultat As System.Web.UI.WebControls.Label

    Dim m_report As kruell_03

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
          
            If IsPostBack = False Then

                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text

                If Not Session("objReport") Is Nothing Then
                    m_report = CType(Session("objReport"), kruell_03)
                Else
                    lblError.Text = "Es wurde kein Session Objekt für den Report gefunden, Vorgang ist ungültig"
                    Exit Sub
                End If


                'ddlpagesize füllen
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                FillGrid(0)
            Else
                If m_report Is Nothing Then
                    m_report = CType(Session("objReport"), kruell_03)
                End If

            End If

        Catch ex As Exception
            lblError.Text = "Fehler beim laden der Seite: " & ex.Message.ToString
        End Try
    End Sub



    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        If m_report Is Nothing Then
            m_report = CType(Session("objChange"), kruell_03)
        End If


        If m_report.Result Is Nothing OrElse m_report.Result.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblnodata.Visible = True
            ddlPageSize.Visible = False
            lblnodata.Text = "Keine Daten zur Anzeige gefunden."
        Else
            DataGrid1.Visible = True
            lblnodata.Visible = False

            Dim tmpDataView As New DataView()
            tmpDataView = m_report.Result.DefaultView

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            Else
                If Not ViewState("Sort") Is Nothing Then
                    strTempSort = ViewState("Sort").ToString
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            DataGrid1.CurrentPageIndex = intTempPageIndex

            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            'splitten der Aufbereitungsmöglichkeiten nach | für die anzeige
            Dim item As DataGridItem
            Dim cell As TableCell

            For Each item In DataGrid1.Items
                If item.Cells(12).Text = "X" Then
                    item.BackColor = Color.Red
                End If
                For Each cell In item.Cells
                    If Not cell.Text.IndexOf("|") = -1 Then
                        cell.Text = "&bull;&nbsp;" & cell.Text
                        cell.Text = cell.Text.Replace("|", "<br>&bull;&nbsp;")
                    End If
                Next
            Next


            lblnodata.Text = "Funde: " & tmpDataView.Count.ToString & " "
            lblnodata.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If
        End If
    End Sub



    Public Function changeIcon2(ByVal eingangkmc As Object) As String
        'für das Eingegangen ICON im Grid
        'wenn eingang verzeichnet, erfolgicon anzeigen, sonst text
        If Not eingangkmc Is System.DBNull.Value Then
            Return "<img src=../../../Images/erfolg.gif border=0> " & CStr(eingangkmc)
        Else
            Return "buchen"
        End If

    End Function


    Private Sub DataGrid1_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.UpdateCommand
        If m_report Is Nothing Then
            m_report = CType(Session("objChange"), kruell_03)
        End If
        Try



        Dim txtDatFertigstellung As TextBox = CType(e.Item.FindControl("txtFertigstellungsdatum"), TextBox)
            Dim txtmemo As TextBox = CType(e.Item.Cells(8).Controls(1), TextBox)


        'prüfung ob text und datum gefüllt 

        If (txtmemo.Text.Trim Is String.Empty And txtDatFertigstellung.Text.Trim Is String.Empty) = False And (txtmemo.Text.Trim Is String.Empty = False And txtDatFertigstellung.Text.Trim Is String.Empty = False) = False Then
                lblError.Text = "Fertigstellungsdatum und Störungsmeldung sind nur in Kombination zulässig, verwenden Sie den Lösch-Button"
            Exit Sub
        End If

        'prüfen auf die länge des errortextes
        If txtmemo.Text.Length > 100 Then
            Dim script As String
            script = "<" & "script language='javascript'>" & _
                      "alert('Es sind Maximal 100 Zeichen erlaubt, Ihr Text enthält " & txtmemo.Text.Length & " Zeichen' );" & _
                  "</" & "script>"
            Response.Write(script)
            Exit Sub
        End If


        'prüfen und eintragen des geplanten fertigstellungsdatums
            Dim strErrorText As String = ""
        If Not HelpProcedures.checkDate(txtDatFertigstellung, strErrorText, True) Then
            lblError.Text = strErrorText
            Exit Sub
        End If


            'datumsvalidierung
            If IsDate(txtDatFertigstellung.Text) Then
                If CDate(txtDatFertigstellung.Text) < Today Then
                    lblError.Text = "gepl. Fertigstellungsdatum darf nicht in der Vergangenheit liegen"
                    Exit Sub
                End If
            End If



            m_report.Stoerungsmeldung = txtmemo.Text

            If txtmemo.Text.Trim() Is String.Empty Then
                m_report.datumStoerungsmeldung = "" 'datum der Störungsmeldung löschen
            Else
                'If m_report.Result.Rows(e.Item.ItemIndex).Item("Stoerungsmeldung") Is DBNull.Value Or m_report.Result.Rows(e.Item.ItemIndex).Item("Stoerungsmeldung") Is String.Empty Then
                '    m_report.Stoerungsmeldung = txtmemo.Text
                '    m_report.datumStoerungsmeldung = Today.ToShortDateString
                'Else
                    If Not txtmemo.Text = CStr(m_report.Result.Rows(e.Item.ItemIndex).Item("Stoerungsmeldung")) Then
                        m_report.datumStoerungsmeldung = Today.ToShortDateString
                    Else
                        m_report.datumStoerungsmeldung = CStr(m_report.Result.Rows(e.Item.ItemIndex).Item("Stoerungsmeldungsdatum"))
                    End If
                'End If
            End If

            m_report.geplFertigstellungsdatum = txtDatFertigstellung.Text


            If Not m_report.Result.Rows(e.Item.ItemIndex).Item("EingangKMC") Is DBNull.Value Then
                m_report.Eingang = CStr(m_report.Result.Rows(e.Item.ItemIndex).Item("EingangKMC"))
            End If
            m_report.ordernummer = CStr(m_report.Result.Rows(e.Item.ItemIndex).Item("Ordernummer"))
            m_report.Change()
            lbl_SAPResultat.Text = m_report.Message
            m_report.Fill()
            DataGrid1.EditItemIndex = -1

            'Eingegangen Button Spalte
            DataGrid1.Columns(0).Visible = True
            'löschStörungsmeldungs spalte
            DataGrid1.Columns(10).Visible = False
            'DatumAuftragserteilungsspalte
            DataGrid1.Columns(7).Visible = True

            FillGrid(DataGrid1.CurrentPageIndex)
        Catch ex As Exception
            lbl_SAPResultat.Text = ""
            lblError.Text = m_report.Message & " " & ex.Message
        End Try

    End Sub

    Public Function FormatDateBoundColumns(ByVal datum As Object) As String
        If datum Is DBNull.Value Then
            Return ""
        Else
            Return CStr(FormatDateTime(CDate(datum), DateFormat.GeneralDate))
        End If

    End Function


    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Try
            If e.CommandName = "Eingegangen" Then

                If m_report Is Nothing Then
                    m_report = CType(Session("objChange"), kruell_03)
                End If

                'weil auch vor eingang eine störungsmeldung aufgegeben werden kann
                If Not m_report.Result.Rows(e.Item.ItemIndex).Item("Stoerungsmeldung") Is DBNull.Value Then
                    m_report.Stoerungsmeldung = CStr(m_report.Result.Rows(e.Item.ItemIndex).Item("Stoerungsmeldung"))
                End If

                If Not m_report.Result.Rows(e.Item.ItemIndex).Item("Stoerungsmeldungsdatum") Is DBNull.Value Then
                    m_report.datumStoerungsmeldung = CStr(m_report.Result.Rows(e.Item.ItemIndex).Item("Stoerungsmeldungsdatum"))
                End If


                m_report.Eingang = Today.ToShortDateString
                m_report.ordernummer = CStr(m_report.Result.Rows(e.Item.ItemIndex).Item("Ordernummer"))
                m_report.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)
                lbl_SAPResultat.Text = m_report.Message
                m_report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)
                lblError.Text = m_report.Message
                Me.FillGrid(DataGrid1.CurrentPageIndex)
            End If

        Catch
            lbl_SAPResultat.Text = ""
            lblError.Text = m_report.Message
        End Try

        If e.CommandName = "LoeschMeldung" Then
            Dim txtDatFertigstellung As TextBox = CType(e.Item.FindControl("txtFertigstellungsdatum"), TextBox)
            Dim txtmemo As TextBox = CType(e.Item.Cells(8).Controls(1), TextBox)
            Dim tmpCal As Calendar = CType(e.Item.FindControl("calFertigstellung"), Calendar)
            txtDatFertigstellung.Text = String.Empty
            txtmemo.Text = String.Empty
            tmpCal.SelectedDate = Nothing
        End If
    End Sub

    Private Sub removeAllValues()
        If m_report Is Nothing Then
            m_report = CType(Session.Item("objChange"), kruell_03)
        End If
        With m_report
            .datumStoerungsmeldung = ""
            .Eingang = ""
            .Erledigt = ""
            .fahrgestellnummer = ""
            .geplFertigstellungsdatum = ""
            .mitEingangKMC = ""
            .mitStoerungsmeldung = ""
            .ohneEingangKMC = ""
            .ordernummer = ""
            .Stoerungsmeldung = ""
        End With
    End Sub

    Public Function checkDate100(ByVal wert As Object) As Date
        If wert Is DBNull.Value OrElse wert Is String.Empty Then
            Return Nothing
        Else
            Return CDate(wert)
        End If
    End Function

    Public Sub Calendar_DateChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim tmpCal As Calendar = CType(sender, Calendar)
        Dim item As DataGridItem
        item = DataGrid1.Items(DataGrid1.EditItemIndex)
        Dim txtDatum As TextBox = CType(item.FindControl("txtFertigstellungsdatum"), TextBox)
        txtDatum.Text = tmpCal.SelectedDate.ToShortDateString
    End Sub


    Private Sub DataGrid1_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.EditCommand
        DataGrid1.EditItemIndex = e.Item.ItemIndex

        If m_report Is Nothing Then
            m_report = CType(Session("objChange"), kruell_03)
        End If

        'Eingegangen Button Spalte
        DataGrid1.Columns(0).Visible = False
        'DatumAuftragserteilungsspalte
        DataGrid1.Columns(7).Visible = False

        'wenn ein Störungsmeldung vorliegt, löschcolumn eiblenden,

        If viewstate.Item("enabled") Is Nothing Then
            viewstate.Add("enabled", True)
        End If
        If Not m_report.Result.Rows(e.Item.ItemIndex).Item("Stoerungsmeldungsdatum") Is System.DBNull.Value AndAlso Not m_report.Result.Rows(e.Item.ItemIndex).Item("Stoerungsmeldungsdatum") Is String.Empty Then
            'auftraglöschbutton Spalte
            DataGrid1.Columns(10).Visible = True
            'viewstate für disablen der Datumstextbox setzen, wird beim itemcreate disabled, da hier nicht zu finden
            viewstate.Add("enabled", False)
        Else
            viewstate.Add("enabled", True)
        End If

        FillGrid(DataGrid1.CurrentPageIndex)

    End Sub

    Private Sub DataGrid1_CancelCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.CancelCommand
        DataGrid1.EditItemIndex = -1

        'Eingegangen Button Spalte
        DataGrid1.Columns(0).Visible = True
        'löschStörungsmeldungs Spalte
        DataGrid1.Columns(10).Visible = False

        'DatumAuftragserteilungsspalte
        DataGrid1.Columns(7).Visible = True

        FillGrid(DataGrid1.CurrentPageIndex)
    End Sub


    Sub DataGrid1_ItemCreated(ByVal Sender As Object, ByVal e As DataGridItemEventArgs) Handles DataGrid1.ItemCreated
        Try
            Select Case e.Item.ItemType
                Case ListItemType.Item
                    Dim myDeleteButton As LinkButton

                    myDeleteButton = CType(e.Item.FindControl("lbEingegangen"), LinkButton)
                    myDeleteButton.Attributes.Add("onclick", "return confirm('Eingang des Fahrzeuges bestätigen?');")
                Case ListItemType.AlternatingItem

                    Dim myDeleteButton As LinkButton
                    myDeleteButton = CType(e.Item.FindControl("lbEingegangen"), LinkButton)

                    myDeleteButton.Attributes.Add("onclick", "return confirm('Eingang des Fahrzeuges bestätigen?');")

                Case ListItemType.EditItem
                    Dim txtDatum1 As TextBox
                    Dim myDeleteButton2 As LinkButton
                    txtDatum1 = CType(e.Item.FindControl("txtFertigstellungsdatum"), TextBox)
                    myDeleteButton2 = CType(e.Item.FindControl("lbStoerungLoeschen"), LinkButton)

                    If Not txtDatum1 Is Nothing Then
                        If CBool(viewState.Item("enabled")) = False Then
                            txtDatum1.Enabled = False
                        Else
                            txtDatum1.Enabled = True
                        End If
                    End If

                    '                    If Not myDeleteButton2 Is Nothing Then
                    myDeleteButton2.Attributes.Add("onclick", "return confirm('Störungsmeldung löschen?');")

                    '                   End If
            End Select
        Catch
            lblError.Text = "Vorsicht: Bestätigungsabfragen konnten nicht generiert werden"
        End Try
    End Sub


    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(DataGrid1.CurrentPageIndex)
    End Sub






    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub lb_Auswahl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Auswahl.Click
        Response.Redirect("Report01.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class

' ************************************************
' $History: Report01_1.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 17:24
' Updated in $/CKAG/Applications/AppKruell/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:37
' Created in $/CKAG/Applications/AppKruell/Forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 23.01.08   Time: 14:02
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' testfertig ITA 1580
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 21.01.08   Time: 10:01
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 17.01.08   Time: 12:32
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' in bearbeitung
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 16.01.08   Time: 18:10
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' in bearbeitung
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 16.01.08   Time: 16:48
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' in bearbeitung
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 16.01.08   Time: 11:01
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' in bearbeitung
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 15.01.08   Time: 17:31
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' in bearbeitung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 15.01.08   Time: 15:15
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' in bearbeitung
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 15.01.08   Time: 11:46
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' Script erzeugen?
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 15.01.08   Time: 10:46
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' ITA 1580, in bearbeitung
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 9.01.08    Time: 14:29
' Created in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' ITA 1580 Torso
' ************************************************
