Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements


Imports System
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization


Public Class Change45_1
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


    Dim m_change As fin_11

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

        m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte 
        m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

        FormAuth(Me, m_User)
        ucHeader.InitUser(m_User)
        GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        lblError.Text = ""

        If m_change Is Nothing Then
            m_change = CType(Session("m_change"), fin_11)
        End If

        If IsPostBack = False Then
            FillGrid(0)
        End If


    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If m_change.Status = 0 OrElse m_change.Status = -1111 Then
            If m_change.Result.Rows.Count = 0 Then
                DataGrid1.Visible = False
                lblNoData.Visible = True
                Label1.Visible = False
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            Else
                DataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = m_change.Result.DefaultView

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
                If m_change.Result.Rows.Count = 1 Then
                    If Not m_change.Result.Rows(0).Item("Status") = "" Then
                        lb_Autorisierung.Visible = True
                        'Freigabe/Storno Button
                        DataGrid1.Columns(10).Visible = False
                        DataGrid1.Columns(11).Visible = False
                        'Auto/Loesch Button
                        DataGrid1.Columns(12).Visible = True
                        DataGrid1.Columns(13).Visible = True
                        If m_change.Storno = "X" Then
                            lblNoData.Text = "Es wird um eine Autorisierung für die Stornierung dieser Freigabe gebeten"
                        Else
                            lblNoData.Text = "Es wird um eine Autorisierung für diese Freigabe gebeten"
                        End If
                    End If
                End If

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
            lblError.Text = m_change.Message
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

        If Not e.CommandName = "Sort" Then

            'Autorisierungsoptionen, durchführung und löschung des eintrages in der Autorisierungstabelle
            If e.CommandName = "Autho" Then
                If Session("AuthorizationID") = Nothing Then
                    lblError.Text = "Es wurde keine Autorisierungsvorgang in der Session gefunden, Vorgang wird abgebrochen"
                Else
                    Dim logAppFreigabe As Base.Kernel.Logging.Trace
                    logAppFreigabe = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                    logAppFreigabe.CollectDetails("Händlernummer", CType(m_change.Haendlernummer, Object), True)
                    logAppFreigabe.CollectDetails("EQUINummer", CType(m_change.Equinr, Object))
                    logAppFreigabe.CollectDetails("Storno", CType(m_change.Storno, Object))
                    logAppFreigabe.CollectDetails("Fahrgestellnummer", CType(m_change.Fahrgestellnr, Object))
                    m_change.AutorisierungUser = m_User.UserName
                    m_change.stornoorderfreigabe(Session("AppID").ToString, Session.SessionID, m_change.Storno, "X")
                    If m_change.Status = 0 Then
                        logAppFreigabe.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_change.Result.Rows(0).Item("Fahrgestellnummer"), "Brieffreigabe(Autorisierung) für Händler " & m_change.Haendlernummer & " erfolgreich durchgeführt.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logAppFreigabe.InputDetails)
                        DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
                    Else
                        logAppFreigabe.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_change.Result.Rows(0).Item("Fahrgestellnummer"), "Fehler bei der Brieffreigabe für Händler " & m_change.Haendlernummer & " , (Fehler: " & m_change.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logAppFreigabe.InputDetails)
                        lblError.Text = "Beim Speichern Ihrer Daten sind Fehler aufgetreten."
                    End If

                    lblNoData.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
                End If
                DataGrid1.Visible = False
                Exit Sub
            ElseIf e.CommandName = "Loesch" Then
                If Session("AuthorizationID") = Nothing Then
                    lblError.Text = "Es wurde keine Autorisierungsvorgang in der Session gefunden, Vorgang wird abgebrochen"
                Else
                    DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
                    lblNoData.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
                End If
                DataGrid1.Visible = False
                Exit Sub
            End If


            lblError.Text = ""
            '!!!!!!!!!WICHTIG,  EQUINR MUSS EINDEUTIG SEIN UND AN LETZTER STELLE EINES DATAGRID ITEMS STEHEN! JJU2008.03.04
            '------------------------------------------------------------------------------------
            'geht nicht wenn sich eine Zeile im edit-Modus befindet, kann irgendwie nicht die item cells zugreifen! JJU2008.03.04
            '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            'm_change.Haendlernummer = m_change.Result.Select("EQUNR='" & e.Item.Cells(e.Item.Cells.Count - 1).Text & "'")(0).Item("Haendlernummer")
            'm_change.VBELN = m_change.Result.Select("EQUNR='" & e.Item.Cells(e.Item.Cells.Count - 1).Text & "'")(0).Item("VBELN")
            'm_change.Equinr = m_change.Result.Select("EQUNR='" & e.Item.Cells(e.Item.Cells.Count - 1).Text & "'")(0).Item("EQUNR")
            'm_change.Fahrgestellnr = m_change.Result.Select("EQUNR='" & e.Item.Cells(e.Item.Cells.Count - 1).Text & "'")(0).Item("Fahrgestellnummer")
            '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            m_change.Haendlernummer = m_change.Result.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("Haendlernummer")
            m_change.VBELN = m_change.Result.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("VBELN")
            m_change.Equinr = m_change.Result.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("EQUNR")
            m_change.Fahrgestellnr = m_change.Result.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("Fahrgestellnummer")

            '------------------------------------------------------------------------------------

            Select Case m_change.Result.Rows(e.Item.ItemIndex).Item("Kontingentart").ToString
                Case "Standard temporär"
                    m_change.Kontingentart = "0001"
                Case "Standard endgültig"
                    m_change.Kontingentart = "0002"
                Case "Händler Zulassung"
                    m_change.Kontingentart = "0005"
            End Select


            If e.CommandName = "Storno" Then

                'wenn einmal auf Storno geklickt wird, zeile Editieren für Storno Grund sonst ins SAP Schießen, bzw Autorisierung
                If DataGrid1.EditItemIndex = e.Item.ItemIndex Then
                    m_change.Storno = "X"
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
                                m_change.StornoGrund1 = m_change.StornoGrund1 & cTmp
                            Case 1
                                m_change.StornoGrund2 = m_change.StornoGrund2 & cTmp
                            Case 2
                                m_change.StornoGrund3 = m_change.StornoGrund3 & cTmp
                            Case 3
                                m_change.StornoGrund4 = m_change.StornoGrund4 & cTmp
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
                m_change.Storno = ""
            End If



            'logging
            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            logApp.CollectDetails("Händlernummer", CType(m_change.Haendlernummer, Object), True)
            logApp.CollectDetails("Storno", CType(m_change.Storno, Object))
            logApp.CollectDetails("Vertragsnummer", CType(m_change.Result.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("Vertragsnummer"), Object))
            logApp.CollectDetails("Fahrgestellnummer", CType(m_change.Result.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("Fahrgestellnummer"), Object))




            If (CInt(m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel")) > 0) Then


                Dim DetailArray(1, 2) As Object
                Dim ms As MemoryStream
                Dim formatter As BinaryFormatter
                Dim b() As Byte


                ''Pruefen, ob schon in der Autorisierung.
                Dim strInitiator As String = ""
                Dim intAuthorizationID As Int32

                m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, m_change.Result.Rows(e.Item.ItemIndex).Item("Haendlernummer"), m_change.Result.Rows(e.Item.ItemIndex).Item("Fahrgestellnummer"), m_User.IsTestUser, strInitiator, intAuthorizationID)
                If Not strInitiator.Length = 0 Then
                    'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                    lblError.Text = "Diese Briefanforderung wurde schon einmal freigegeben und liegt bereits zur Autorisierung vor!"
                    Exit Sub
                End If

                'status des Datensatzes setzen
                m_change.Result.Select("EQUNR='" & e.CommandArgument & "'")(0).Item("Status") = "inAutorisierung"
                Dim tmpRow As DataRow
                Dim x() As Object
                tmpRow = m_change.Result.NewRow()
                tmpRow = m_change.Result.Select("EQUNR='" & e.CommandArgument & "'")(0)
                x = tmpRow.ItemArray
                m_change.Result.Rows.Clear()
                m_change.Result.AcceptChanges()
                m_change.Result.Rows.Add(x)
                m_change.Result.AcceptChanges()


                'erst nach änderung in memoryStream umwandeln, sonst sind diese logischerweise nicht da
                ms = New MemoryStream()
                formatter = New BinaryFormatter()
                formatter.Serialize(ms, m_change)
                b = ms.ToArray
                ms = New MemoryStream(b)
                DetailArray(0, 0) = ms
                DetailArray(0, 1) = "m_change"

                'System.Diagnostics.Debug.WriteLine("" + CStr(m_change.Result.Rows(0).Item("Haendlernummer")))
                'System.Diagnostics.Debug.WriteLine("" + CStr(m_change.Result.Rows(0).Item("Fahrgestellnummer")))
                'System.Diagnostics.Debug.WriteLine("" + CStr(m_change.Result.Rows.Count))
                intAuthorizationID = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, m_change.Result.Rows(0).Item("Haendlernummer"), m_change.Result.Rows(0).Item("Fahrgestellnummer"), "", "", m_User.IsTestUser, DetailArray)
                logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_change.Result.Rows(0).Item("Fahrgestellnummer"), "Brieffreigabe für Händler " & m_change.Result.Rows(0).Item("Haendlernummer") & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)

                Label1.Text = "Die Freigabe des Briefes mit der Fahrgestellnummer: " & m_change.Result.Rows(0).Item("Fahrgestellnummer") & " liegt zur Autorisierung vor"

                'attribute löschen
                m_change.Fahrgestellnr = ""
                m_change.Equinr = ""
                m_change.VBELN = ""
                m_change.StornoGrund1 = ""
                m_change.StornoGrund2 = ""
                m_change.StornoGrund3 = ""
                m_change.StornoGrund4 = ""


                'Grid neu laden
                m_change.show(Session("AppID").ToString, Session.SessionID)
                FillGrid(DataGrid1.CurrentPageIndex)
            Else
                'Anwendung erfordert keine Autorisierung (Level=0)

                'logging
                'für benutzeraktivitäten, nimmt sich die werte aus der logApp.CollectDetails(die Weiter oben gefüllt wurde) in der Funktion logApp.InputDetails um den die BenutzerHistory tabelle zu erstellen


                m_change.stornoorderfreigabe(Session("AppID").ToString, Session.SessionID, m_change.Storno)


                If Not m_change.Status = 0 Then

                    If m_change.Status = -1111 Then
                        'auftrag wurde währendessen schonmal freigegeben von einem anderen benutzer
                        Label1.Text = "Vorgang wurde schon bearbeitet von " & m_change.FreigabeUser & " am " & m_change.FreigabeDatum & " um " & m_change.FreigabeUhrzeit
                        Label1.Visible = True
                        fakeTheGrid(e.CommandArgument, m_change.Storno, m_change.Fahrgestellnr, True)
                    Else
                        lblError.Text = m_change.Message
                    End If
                Else
                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_change.Result.Rows(0).Item("Fahrgestellnummer"), "Brieffreigabe für Händler " & m_change.Result.Rows(0).Item("Haendlernummer") & " erfolgreich durchgeführt.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                    fakeTheGrid(e.CommandArgument, m_change.Storno, m_change.Fahrgestellnr)
                End If
            End If
        End If
    End Sub

    Private Sub fakeTheGrid(ByVal EQUNR As String, ByVal storno As String, ByVal fahrgestellnummer As String, Optional ByVal keineAnzeige As Boolean = False)
        m_change.Result.Select("EQUNR='" & EQUNR & "'")(0).Delete()
        m_change.Result.AcceptChanges()
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
        Response.Redirect("Change45.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub lb_Autorisierung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Autorisierung.Click
        Response.Redirect("Change48.aspx?AppID=" & Session("AppID").ToString)
    End Sub


End Class
' ************************************************
' $History: Change45_1.aspx.vb $
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 3.03.10    Time: 13:52
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 2.03.10    Time: 14:35
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA: 3509, 3515, 3522
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 22.06.09   Time: 16:58
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918  Z_M_Gesperrte_Auftraege_001
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 10.07.08   Time: 11:02
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2062
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 11.06.08   Time: 13:30
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 23.05.08   Time: 15:24
' Updated in $/CKAG/Components/ComCommon/Finance
' Bugfixing RTFS Bank Portal
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance
' 
' *****************  Version 22  *****************
' User: Jungj        Date: 13.03.08   Time: 10:42
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Anpassungen Offene Anforderungen Bank/offene Anforderungen Händler
' 
' *****************  Version 21  *****************
' User: Jungj        Date: 5.03.08    Time: 9:13
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Änderungen ITA 1733
' 
' *****************  Version 20  *****************
' User: Jungj        Date: 4.03.08    Time: 18:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF ANPASSUNGEN ITA 1733
' 
' *****************  Version 19  *****************
' User: Jungj        Date: 28.02.08   Time: 12:43
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733
' 
' *****************  Version 18  *****************
' User: Jungj        Date: 28.02.08   Time: 12:42
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733
' 
' *****************  Version 17  *****************
' User: Jungj        Date: 25.02.08   Time: 15:05
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ita 1733
' 
' *****************  Version 16  *****************
' User: Jungj        Date: 14.02.08   Time: 12:53
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' akf änderungen
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 11.02.08   Time: 15:11
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 11.02.08   Time: 15:09
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Logging für benutzeraktivitäten bei Freigabe hinzugefügt
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 4.02.08    Time: 8:19
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' AKF Änderungen
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 2.02.08    Time: 13:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' akf Änderungen
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 9.01.08    Time: 9:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
'  bugfix Sammelautorisierung bei Freigabe gesperrter Aufträge
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 8.01.08    Time: 18:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Autorisierung
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 8.01.08    Time: 16:19
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Einzelautorisierung bei der Freigabe Briefanforderungen
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 8.01.08    Time: 12:49
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Authorisierung Freigabe gesperrter Aufträge
' 
' ************************************************
