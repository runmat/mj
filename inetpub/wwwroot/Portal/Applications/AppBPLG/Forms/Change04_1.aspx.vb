Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary


Public Class Change04_1
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Haendlersuche As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trPageSize As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents trDataGrid1 As System.Web.UI.HtmlControls.HtmlTableRow

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents lb_Autorisierung As System.Web.UI.WebControls.LinkButton
    Protected WithEvents xCoordHolder As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents yCoordHolder As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents trVorgangsArt As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents divYCoordHolder As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents btnMassenfreigabe As System.Web.UI.WebControls.LinkButton

    Dim mObjFreigabe As Freigabe

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
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte 
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

            FormAuth(Me, m_User)
            ucHeader.InitUser(m_User)
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            lblError.Text = ""

            If mObjFreigabe Is Nothing Then
                mObjFreigabe = CType(Session("mObjFreigabeSession"), Freigabe)
            End If

            If IsPostBack = False Then
                FillGrid(0)
            End If

        Catch ex As Exception
            lblError.Text = "Fehler beim laden der Seite: " & ex.Message.ToString
        End Try
        
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If mObjFreigabe.Status = 0 OrElse mObjFreigabe.Status = -1111 Then
            If mObjFreigabe.Result.Rows.Count = 0 Then
                DataGrid1.Visible = False
                lblNoData.Visible = True
                Label1.Visible = False
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            Else
                DataGrid1.Visible = True
                lblNoData.Visible = False

                If IsPostBack = True Then
                    saveMassenFreigabeAuswahl() 'angeklickte checkboxen im grid in Tabelle sichern bevor grid neu befüllt wird JJU2008.08.22
                End If

                Dim tmpDataView As DataView = mObjFreigabe.Result.DefaultView

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

                lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " Fahrzeuge gefunden."
                lblNoData.Visible = True


                'für AutorisierungsAnwendung, Einzelautorisierung
                If mObjFreigabe.Result.Rows.Count = 1 Then
                    If Not mObjFreigabe.Result.Rows(0).Item("Status").ToString = "" Then
                        lb_Autorisierung.Visible = True
                        'Freigabe/Storno Button
                        DataGrid1.Columns(10).Visible = False
                        DataGrid1.Columns(11).Visible = False
                        'Auto/Loesch Button
                        DataGrid1.Columns(12).Visible = True
                        DataGrid1.Columns(13).Visible = True
                        If mObjFreigabe.Storno = "X" Then
                            lblNoData.Text = "Es wird um eine Autorisierung für die Stornierung dieser Freigabe gebeten"
                        Else
                            lblNoData.Text = "Es wird um eine Autorisierung für diese Freigabe gebeten"
                        End If
                    End If
                End If

                Dim item As DataGridItem
                Dim cell As TableCell
                Dim button As LinkButton
                Dim label As Label
                Dim control As Control
                Dim blnSameUser As Boolean = False


                For Each item In DataGrid1.Items 'ergänzung, anscheinend darf der user der den brief angefodert hat diesen nicht freigeben, darum auch massenfreigabecheckbox ausblenden! JJU2008.08.22

                    cell = item.Cells(1)
                    For Each control In cell.Controls
                        Dim control1 = TryCast(control, Label)
                        If (control1 IsNot Nothing) Then
                            label = control1
                            If label.ID = "Label4" Then
                                If label.Text = m_User.UserName Then
                                    blnSameUser = True
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                    cell = item.Cells(9)
                    For Each control In cell.Controls
                        Dim linkButton = TryCast(control, LinkButton)
                        If (linkButton IsNot Nothing) Then
                            button = linkButton
                            If button.ID = "lbFreigabe" Then
                                If blnSameUser = True Then
                                    button.Visible = False
                                    item.FindControl("chkMassenfreigabeAuswahl").Visible = False
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                Next

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If
            End If
        Else
            DataGrid1.Visible = False
            Label1.Visible = False
            lblError.Text = mObjFreigabe.Message
            lblNoData.Visible = True
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        
        Try
            If Not e.CommandName = "Sort" Then

                'Autorisierungsoptionen, durchführung und löschung des eintrages in der Autorisierungstabelle
                If e.CommandName = "Autho" Then
                    If Session("AuthorizationID") Is Nothing Then
                        lblError.Text = "Es wurde keine Autorisierungsvorgang in der Session gefunden, Vorgang wird abgebrochen"
                    Else
                        mObjFreigabe.change()
                        DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
                        lblNoData.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
                    End If
                    DataGrid1.Visible = False
                    Exit Sub
                ElseIf e.CommandName = "Loesch" Then
                    If Session("AuthorizationID") Is Nothing Then
                        lblError.Text = "Es wurde keine Autorisierungsvorgang in der Session gefunden, Vorgang wird abgebrochen"
                    Else
                        DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
                        lblNoData.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
                    End If
                    DataGrid1.Visible = False
                    Exit Sub
                End If


                'attribute löschen
                mObjFreigabe.Fahrgestellnr = ""
                mObjFreigabe.Equinr = ""
                mObjFreigabe.VBELN = ""
                mObjFreigabe.StornoGrund1 = ""
                mObjFreigabe.StornoGrund2 = ""
                mObjFreigabe.StornoGrund3 = ""
                mObjFreigabe.StornoGrund4 = ""


                lblError.Text = ""
                '!!!!!!!!!WICHTIG,  EQUINR MUSS EINDEUTIG SEIN UND AN LETZTER STELLE EINES DATAGRID ITEMS STEHEN! JJU2008.03.04
                '------------------------------------------------------------------------------------
                'geht nicht wenn sich eine Zeile im edit-Modus befindet, kann irgendwie nicht die item cells zugreifen! JJU2008.03.04
                '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                'mObjFreigabe.Haendlernummer = mObjFreigabe.Result.Select("EQUNR='" & e.Item.Cells(e.Item.Cells.Count - 1).Text & "'")(0).Item("Haendlernummer")
                'mObjFreigabe.VBELN = mObjFreigabe.Result.Select("EQUNR='" & e.Item.Cells(e.Item.Cells.Count - 1).Text & "'")(0).Item("VBELN")
                'mObjFreigabe.Equinr = mObjFreigabe.Result.Select("EQUNR='" & e.Item.Cells(e.Item.Cells.Count - 1).Text & "'")(0).Item("EQUNR")
                'mObjFreigabe.Fahrgestellnr = mObjFreigabe.Result.Select("EQUNR='" & e.Item.Cells(e.Item.Cells.Count - 1).Text & "'")(0).Item("Fahrgestellnummer")
                '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                ' mObjFreigabe.Haendlernummer = mObjFreigabe.Result.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0).Item("Haendlernummer").ToString
                mObjFreigabe.VBELN = mObjFreigabe.Result.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0).Item("VBELN").ToString
                mObjFreigabe.Equinr = mObjFreigabe.Result.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0).Item("EQUNR").ToString
                mObjFreigabe.Fahrgestellnr = mObjFreigabe.Result.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0).Item("Fahrgestellnummer").ToString

                '------------------------------------------------------------------------------------

                Select Case mObjFreigabe.Result.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0).Item("Abrufart").ToString
                    Case "Standard temporär"
                        mObjFreigabe.Kontingentart = "0001"
                    Case "Standard endgültig"
                        mObjFreigabe.Kontingentart = "0002"
                    Case "Händler Zulassung"
                        mObjFreigabe.Kontingentart = "0005"
                End Select


                If e.CommandName = "Storno" Then

                    'wenn einmal auf Storno geklickt wird, zeile Editieren für Storno Grund sonst ins SAP Schießen, bzw Autorisierung
                    If DataGrid1.EditItemIndex = e.Item.ItemIndex Then
                        mObjFreigabe.Storno = "X"
                        'StornoText auf 4 SAP Felder a 27 Zeichen aufteilen
                        Dim txtStornoGrund As TextBox = CType(e.Item.FindControl("txtStornoGrund"), TextBox)
                        If txtStornoGrund.Text.Length > 108 Then
                            Dim script As String
                            script = "<" & "script language='javascript'>" & _
                                      "alert('Es sind Maximal 108 Zeichen erlaubt, Ihr Text enthält " & txtStornoGrund.Text.Length & " Zeichen' );" & _
                                  "</" & "script>"
                            Response.Write(script)
                            Exit Sub
                        End If

                        Dim x As Int32 = 0
                        Dim i As Int32 = 1

                        Dim cTmp As Char
                        For Each cTmp In txtStornoGrund.Text.ToCharArray
                            If i Mod 27 = 0 Then
                                x = x + 1
                            End If
                            Select Case x
                                Case 0
                                    mObjFreigabe.StornoGrund1 = mObjFreigabe.StornoGrund1 & cTmp
                                Case 1
                                    mObjFreigabe.StornoGrund2 = mObjFreigabe.StornoGrund2 & cTmp
                                Case 2
                                    mObjFreigabe.StornoGrund3 = mObjFreigabe.StornoGrund3 & cTmp
                                Case 3
                                    mObjFreigabe.StornoGrund4 = mObjFreigabe.StornoGrund4 & cTmp
                                Case Else
                                    Exit For
                            End Select
                            i = i + 1
                        Next

                        DataGrid1.EditItemIndex = -1
                    Else

                        DataGrid1.EditItemIndex = e.Item.ItemIndex
                        FillGrid(DataGrid1.CurrentPageIndex)
                        Exit Sub
                    End If
                Else
                    mObjFreigabe.Storno = ""
                End If
                

                'logging
                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                'logApp.CollectDetails("Händlernummer", CType(mObjFreigabe.Haendlernummer, Object), True)
                logApp.CollectDetails("Storno", CType(mObjFreigabe.Storno, Object))
                logApp.CollectDetails("Vertragsnummer", CType(mObjFreigabe.Result.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0).Item("Vertragsnummer"), Object))
                logApp.CollectDetails("Fahrgestellnummer", CType(mObjFreigabe.Result.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0).Item("Fahrgestellnummer"), Object))


                If (CInt(m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel")) > 0) Then


                    Dim DetailArray(1, 2) As Object
                    Dim ms As MemoryStream
                    Dim formatter As BinaryFormatter
                    Dim b() As Byte


                    ''Pruefen, ob schon in der Autorisierung.
                    Dim strInitiator As String = ""
                    Dim intAuthorizationID As Int32

                    m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, mObjFreigabe.Result.Rows(e.Item.ItemIndex).Item("Endkundennummer").ToString, mObjFreigabe.Result.Rows(e.Item.ItemIndex).Item("Fahrgestellnummer").ToString, m_User.IsTestUser, strInitiator, intAuthorizationID)
                    If Not strInitiator.Length = 0 Then
                        'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                        lblError.Text = "Diese Briefanforderung wurde schon einmal freigegeben und liegt bereits zur Autorisierung vor!"
                        Exit Sub
                    End If

                    'status des Datensatzes setzen
                    mObjFreigabe.Result.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0).Item("Status") = "inAutorisierung"
                    Dim tmpRow As DataRow
                    Dim x() As Object
                    'tmpRow = mObjFreigabe.Result.NewRow()
                    tmpRow = mObjFreigabe.Result.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0)
                    If tmpRow IsNot Nothing Then
                        x = tmpRow.ItemArray
                    End If

                    mObjFreigabe.Result.Rows.Clear()
                    mObjFreigabe.Result.AcceptChanges()
                    mObjFreigabe.Result.Rows.Add(x)
                    mObjFreigabe.Result.AcceptChanges()


                    'erst nach änderung in memoryStream umwandeln, sonst sind diese logischerweise nicht da
                    ms = New MemoryStream()
                    formatter = New BinaryFormatter()
                    formatter.Serialize(ms, mObjFreigabe)
                    b = ms.ToArray
                    ms = New MemoryStream(b)
                    DetailArray(0, 0) = ms
                    DetailArray(0, 1) = "mObjFreigabe"

                    'System.Diagnostics.Debug.WriteLine("" + CStr(mObjFreigabe.Result.Rows(0).Item("Haendlernummer")))
                    'System.Diagnostics.Debug.WriteLine("" + CStr(mObjFreigabe.Result.Rows(0).Item("Fahrgestellnummer")))
                    'System.Diagnostics.Debug.WriteLine("" + CStr(mObjFreigabe.Result.Rows.Count))

                    WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, mObjFreigabe.Result.Rows(0).Item("Endkundennummer").ToString, mObjFreigabe.Result.Rows(0).Item("Fahrgestellnummer").ToString, "", "", m_User.IsTestUser, DetailArray)
                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, mObjFreigabe.Result.Rows(0).Item("Fahrgestellnummer").ToString, "Brieffreigabe für Endkunden " & mObjFreigabe.Result.Rows(0).Item("Endkundennummer").ToString & " erfolgreich initiiert.", m_User.CustomerName.ToString, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)

                    Label1.Text = "Die Freigabe des Briefes mit der Fahrgestellnummer: " & mObjFreigabe.Result.Rows(0).Item("Fahrgestellnummer").ToString & " liegt zur Autorisierung vor"

                    'Grid neu laden
                    mObjFreigabe.show()
                    FillGrid(DataGrid1.CurrentPageIndex)
                Else
                    'Anwendung erfordert keine Autorisierung (Level=0)

                    mObjFreigabe.stornoorderfreigabe(mObjFreigabe.Storno)


                    If Not mObjFreigabe.Status = 0 Then

                        If mObjFreigabe.Status = -1111 Then
                            'auftrag wurde währendessen schonmal freigegeben von einem anderen benutzer
                            Label1.Text = "Vorgang wurde schon bearbeitet von " & mObjFreigabe.FreigabeUser & " am " & mObjFreigabe.FreigabeDatum & " um " & mObjFreigabe.FreigabeUhrzeit
                            Label1.Visible = True
                            fakeTheGrid(e.CommandArgument.ToString, mObjFreigabe.Storno, mObjFreigabe.Fahrgestellnr, True)
                        Else
                            lblError.Text = mObjFreigabe.Message
                        End If
                    Else
                        'logging
                        'für benutzeraktivitäten, nimmt sich die werte aus der logApp.CollectDetails(die Weiter oben gefüllt wurde) in der Funktion logApp.InputDetails um den die BenutzerHistory tabelle zu erstellen
                        logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, mObjFreigabe.Result.Rows(0).Item("Fahrgestellnummer").ToString, "Brieffreigabe für Endkunde " & mObjFreigabe.Result.Rows(0).Item("Endkundennummer").ToString & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, )
                        fakeTheGrid(e.CommandArgument.ToString, mObjFreigabe.Storno, mObjFreigabe.Fahrgestellnr)
                    End If
                End If
            End If

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub fakeTheGrid(ByVal EQUNR As String, ByVal storno As String, ByVal fahrgestellnummer As String, Optional ByVal keineAnzeige As Boolean = False)
        mObjFreigabe.Result.Select("EQUNR='" & EQUNR & "'")(0).Delete()
        mObjFreigabe.Result.AcceptChanges()
        FillGrid(DataGrid1.CurrentPageIndex)

        If Not keineAnzeige Then
            'keine Anzeige wenn der datensatz ohne "todo" text aus grid entfernt werden soll
            If storno = "X" Then
                Label1.Text = "Der Auftrag mit der Fahrgestellnummer:  " & fahrgestellnummer & " wurde erfolgreich storniert"
            Else
                Label1.Text = "Der Auftrag mit der Fahrgestellnummer:  " & fahrgestellnummer & " wurde erfolgreich freigegeben"
            End If
            Label1.Visible = True
        End If
    End Sub

    Private Sub lb_Haendlersuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Haendlersuche.Click
        Response.Redirect("Change04.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub lb_Autorisierung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Autorisierung.Click
        Response.Redirect("Change48.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub saveMassenFreigabeAuswahl()
        '----------------------------------------------------------------------
        ' Methode: saveMassenFreigabeAuswahl
        ' Autor: JJU
        ' Beschreibung: speichert die Gridauswahl (MassenfreigabeCheckboxen) in die Datentabelle um nach einem Gridneuladen diesen Zustand wieder herzustellen
        ' Erstellt am: 2008.08.22
        ' ITA: 2189
        '----------------------------------------------------------------------

        If mObjFreigabe Is Nothing Then
            mObjFreigabe = CType(Session("mObjFreigabeSession"), Freigabe)
        End If
        For Each tmpItem As DataGridItem In DataGrid1.Items
            If CType(tmpItem.FindControl("chkMassenfreigabeAuswahl"), CheckBox).Checked Then
                If Not mObjFreigabe.Result.Select("EQUNR='" & tmpItem.Cells(tmpItem.Cells.Count - 1).Text & "'").Length = 0 Then 'kann passiern wenn bei der Freigabe die datensätze aus der tabelle gelöscht wurden JJU2008.08.22
                    mObjFreigabe.Result.Select("EQUNR='" & tmpItem.Cells(tmpItem.Cells.Count - 1).Text & "'")(0).Item("Massenfreigabe") = "True"
                End If
            Else
                If Not mObjFreigabe.Result.Select("EQUNR='" & tmpItem.Cells(tmpItem.Cells.Count - 1).Text & "'").Length = 0 Then
                    mObjFreigabe.Result.Select("EQUNR='" & tmpItem.Cells(tmpItem.Cells.Count - 1).Text & "'")(0).Item("Massenfreigabe") = "False"
                End If
            End If
        Next
        mObjFreigabe.Result.AcceptChanges()
        Session("mObjFreigabeSession") = mObjFreigabe
    End Sub

    Protected Sub btnMassenfreigabe_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMassenfreigabe.Click
        '----------------------------------------------------------------------
        ' Methode: btnMassenfreigabe_Click
        ' Autor: JJU
        ' Beschreibung: Ruft das freigabebapi so oft auf wie Ausgewählte Freigabesätze im Grid existieren
        '               entfernt alle durchgelaufenen Sätze aus der Tabelle und füllt das Grid neu
        ' Erstellt am: 2008.08.22
        ' ITA: 2189
        '----------------------------------------------------------------------
        If mObjFreigabe Is Nothing Then
            mObjFreigabe = CType(Session("mObjFreigabeSession"), Freigabe)
        End If
        Try
            For Each tmpItem As DataGridItem In DataGrid1.Items
                If CType(tmpItem.FindControl("chkMassenfreigabeAuswahl"), CheckBox).Checked Then

                    lblError.Text = ""
                    'attribute löschen
                    mObjFreigabe.Fahrgestellnr = ""
                    mObjFreigabe.Equinr = ""
                    mObjFreigabe.VBELN = ""
                    mObjFreigabe.StornoGrund1 = ""
                    mObjFreigabe.StornoGrund2 = ""
                    mObjFreigabe.StornoGrund3 = ""
                    mObjFreigabe.StornoGrund4 = ""

                    mObjFreigabe.VBELN = mObjFreigabe.Result.Select("EQUNR='" & tmpItem.Cells(tmpItem.Cells.Count - 1).Text & "'")(0).Item("VBELN").ToString
                    mObjFreigabe.Equinr = mObjFreigabe.Result.Select("EQUNR='" & tmpItem.Cells(tmpItem.Cells.Count - 1).Text & "'")(0).Item("EQUNR").ToString
                    mObjFreigabe.Fahrgestellnr = mObjFreigabe.Result.Select("EQUNR='" & tmpItem.Cells(tmpItem.Cells.Count - 1).Text & "'")(0).Item("Fahrgestellnummer").ToString


                    Select Case mObjFreigabe.Result.Select("EQUNR='" & tmpItem.Cells(tmpItem.Cells.Count - 1).Text & "'")(0).Item("Abrufart").ToString
                        Case "Standard temporär"
                            mObjFreigabe.Kontingentart = "0001"
                        Case "Standard endgültig"
                            mObjFreigabe.Kontingentart = "0002"
                        Case "Händler Zulassung"
                            mObjFreigabe.Kontingentart = "0005"
                    End Select


                    mObjFreigabe.stornoorderfreigabe("") 'freigabe
                    If Not mObjFreigabe.Status = 0 Then
                        lblError.Text = mObjFreigabe.Message
                        Exit Sub
                    End If
                    mObjFreigabe.Result.Select("EQUNR='" & tmpItem.Cells(tmpItem.Cells.Count - 1).Text & "'")(0).Delete()

                End If
            Next
            mObjFreigabe.Result.AcceptChanges()
        Catch ex As Exception
            lblError.Text = "Es ist ein Fehler in der Massenfreigabe aufgetreten: " & ex.Message
            Exit Sub
        End Try
        Label1.Text = "alle ausgewählten Aufträge wurden erfolgreich freigegeben."
        FillGrid(DataGrid1.CurrentPageIndex)
        Session("mObjFreigabeSession") = mObjFreigabe
    End Sub


End Class
' ************************************************
' $History: Change04_1.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 22.08.08   Time: 15:45
' Updated in $/CKAG/Applications/AppBPLG/Forms
' Methodenkommentare hinzugefügt
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 22.08.08   Time: 13:05
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2189 nachbesserung
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 22.08.08   Time: 12:39
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2189 fertig
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 11.08.08   Time: 17:54
' Updated in $/CKAG/Applications/AppBPLG/Forms
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 30.07.08   Time: 16:16
' Updated in $/CKAG/Applications/AppBPLG/Forms
' Nachbesserung BPLG
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.07.08   Time: 16:57
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2100 TestFertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.07.08   Time: 10:31
' Updated in $/CKAG/Applications/AppBPLG/Forms
' ITA 2100 ungetestet
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 23.07.08   Time: 13:56
' Created in $/CKAG/Applications/AppBPLG/Forms
' ITA 2100 erstellung Rohversion
' 
' ************************************************
