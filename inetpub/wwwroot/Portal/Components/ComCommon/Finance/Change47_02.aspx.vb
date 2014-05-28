Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary


Public Class Change47_02
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As CKG.Components.ComCommon.Finance.Search
    Private objChange47_objRTFS As fin_09
    Private objHaendler As BankBaseCredit
    'Private logApp As Base.Kernel.Logging.Trace

    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents kopfdaten1 As Kopfdaten
    Protected WithEvents tableGrid As System.Web.UI.HtmlControls.HtmlTableRow
    Dim Aut As Boolean = False
    
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
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        kopfdaten1.Message = ""
        lnkKreditlimit.NavigateUrl = "Change47.aspx?AppID=" & Session("AppID").ToString & "&back=1"

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text
        m_App = New Base.Kernel.Security.App(m_User)

        If objChange47_objRTFS Is Nothing Then
            objChange47_objRTFS = CType(Session("AppHaendler09"), fin_09)
        End If

        'logApp = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        If Request.QueryString("Linked") = 1 Then
            lnkKreditlimit.Text = "Briefversand"
            lnkKreditlimit.NavigateUrl = Session.Item("URLReferenz")
            lblHead.Text = "Änderung Versandstatus (nachträglich endgültig)"
            ucStyles.TitleText = lblHead.Text
        End If

        'zurücklinken zu Fällige Vorgänge
        If Request.QueryString("Linked") = 2 Then
            lnkKreditlimit.Text = "Fällige Vorgänge"
            'lnkKreditlimit.NavigateUrl = "Change41_1.aspx?AppID=" & Session("AppID").ToString & "&Linked=2"
            lnkKreditlimit.NavigateUrl = Session.Item("URLReferenz")
            lblHead.Text = "Änderung Versandstatus (nachträglich endgültig)"
            ucStyles.TitleText = lblHead.Text
        End If


        If (Not Session("Authorization") Is Nothing) AndAlso CBool(Session("Authorization")) AndAlso _
            (Not Session("AuthorizationID") Is Nothing) AndAlso IsNumeric(Session("AuthorizationID")) Then
            Aut = True
            Dim OutPutStream As MemoryStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objSuche")
            If OutPutStream Is Nothing Then
                lblError.Text = "Keine Daten für den Vorgang vorhanden."
            Else
                Dim formatter As New BinaryFormatter()
                objSuche = New Finance.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                objSuche = DirectCast(formatter.Deserialize(OutPutStream), Finance.Search)
                Session("objSuche") = objSuche
                formatter = New BinaryFormatter()
                OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "objChange47_objRTFS")
                objChange47_objRTFS = DirectCast(formatter.Deserialize(OutPutStream), fin_09)
                Session("AppHaendler09") = objChange47_objRTFS

                cmdSave.Visible = True
                lnkKreditlimit.Text = "Übersicht Autorisierung"
                lnkKreditlimit.NavigateUrl = "Change48.aspx?AppID=" & Session("AppID").ToString
            End If
        End If
        If Session("AppHaendler09") Is Nothing AndAlso Aut = False Then
            Response.Redirect("Change42.aspx?AppID=" & Session("AppID").ToString)
        End If

        If Session("objSuche") Is Nothing AndAlso Aut = False Then
            Response.Redirect("Change42.aspx?AppID=" & Session("AppID").ToString)
        Else
            objSuche = CType(Session("objSuche"), Finance.Search)
            'kann passieren wenn verlinkung von Briefanforderung wenn Ohne Händlernummer gesucht wurde
            If objSuche.REFERENZ Is Nothing OrElse objSuche.REFERENZ Is String.Empty Then
                If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, objChange47_objRTFS.Customer) Then
                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                End If
            End If
        End If

        kopfdaten1.UserReferenz = m_User.Reference
        kopfdaten1.HaendlerNummer = objSuche.REFERENZ
        Dim strTemp As String = objSuche.NAME
        If objSuche.NAME_2.Length > 0 Then
            strTemp &= "<br>" & objSuche.NAME_2
        End If
        kopfdaten1.HaendlerName = strTemp
        kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
        Session("objSuche") = objSuche

        
        If Not IsPostBack Then

            objHaendler = New BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            objHaendler.Customer = objSuche.REFERENZ
            objHaendler.KUNNR = m_User.KUNNR
            objHaendler.CreditControlArea = "ZDAD"
            objHaendler.Show(Session("AppID").ToString, Session.SessionID)
            kopfdaten1.Kontingente = objHaendler.Kontingente
            FillGrid(objChange47_objRTFS, 0)
            Session("App_objHaendler") = objHaendler
        Else
            objHaendler = CType(Session("App_objHaendler"), BankBaseCredit)
            kopfdaten1.Kontingente = objHaendler.Kontingente
        End If
        
    End Sub

    Private Sub FillGrid(ByVal objBank As fin_09, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If Not objBank.Fahrzeuge Is Nothing Then
            If objBank.Status = 0 Then
                If objBank.Fahrzeuge.Rows.Count = 0 Then
                    'trDataGrid1.Visible = False
                    'trPageSize.Visible = False
                    lblNoData.Visible = True
                    lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                    DataGrid1.Visible = False
                    ShowScript.Visible = False
                Else
                    DataGrid1.Visible = True
                    lblNoData.Visible = False

                    Dim tmpDataView As New DataView(objBank.Fahrzeuge)
                    If Aut Then
                        tmpDataView.RowFilter = "Fahrgestellnummer='" & Request.QueryString("FIN") & "'"
                    End If


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

                    'lblNoData.Text = "Es wurden " & objBank.AuftraegeAlle.Rows.Count.ToString & " fällige Vorgänge gefunden."
                    lblNoData.Visible = True

                    If DataGrid1.PageCount > 1 Then
                        DataGrid1.PagerStyle.CssClass = "PagerStyle"
                        DataGrid1.DataBind()
                        DataGrid1.PagerStyle.Visible = True
                    Else
                        DataGrid1.PagerStyle.Visible = False
                    End If

                    'kontingente aktuallisieren
                    If objHaendler Is Nothing Then
                        objHaendler = CType(Session("App_objHaendler"), BankBaseCredit)
                    End If
                    objHaendler.Show(Session("AppID").ToString, Session.SessionID)
                    kopfdaten1.Kontingente = objHaendler.Kontingente

                    Dim strInitiator As String = ""
                    Dim intAuthorizationID As Int32
                    Dim item As DataGridItem
                    Dim cell As TableCell
                    Dim oLabel As Label
                    Dim control As Control
                    Dim button As LinkButton
                    For Each item In DataGrid1.Items
                        cell = item.Cells(2)

                        m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, objSuche.REFERENZ,
                                                           cell.Text, m_User.IsTestUser, strInitiator, intAuthorizationID)
                        If Not strInitiator.Length = 0 Then
                            'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                            cell = item.Cells(9)
                            For Each control In cell.Controls
                                Dim linkButton = TryCast(control, LinkButton)
                                If (linkButton IsNot Nothing) Then
                                    button = linkButton
                                    If button.ID = "lbStatus" Then
                                        button.Visible = False
                                    End If
                                End If
                                If Aut = False Then
                                    Dim label = TryCast(control, Label)
                                    If (label IsNot Nothing) Then
                                        oLabel = label
                                        If oLabel.ID = "lblStatus" Then
                                            oLabel.Visible = True
                                            oLabel.Text = "liegt zur Autorisierung vor"
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            Else
                lblError.Text = objBank.Message
                lblNoData.Visible = True
                ShowScript.Visible = False
            End If
        Else
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            DataGrid1.Visible = False
            lblNoData.Visible = True
        End If
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim mstr_FIN As String
        If Not e.Item.Cells(0).Text.Length = 0 Then
            objChange47_objRTFS.EQUNR = e.Item.Cells(0).Text
            mstr_FIN = e.Item.Cells(2).Text
            objChange47_objRTFS.Lastschrifterzeugung = CType(e.Item.FindControl("cboLastschrifterzeugung"), DropDownList).SelectedValue


            ' Autorisierungspflichtig?

            If (CInt(m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel")) > 0) Then
                Dim DetailArray(2, 1) As Object
                Dim ms As MemoryStream
                Dim formatter As BinaryFormatter
                Dim b() As Byte

                ms = New MemoryStream()
                formatter = New BinaryFormatter()
                formatter.Serialize(ms, objSuche)
                b = ms.ToArray
                ms = New MemoryStream(b)
                DetailArray(0, 0) = ms
                DetailArray(0, 1) = "objSuche"

                If objChange47_objRTFS.Fahrzeuge.Rows.Count > 1 Then
                    objChange47_objRTFS.Fahrzeuge.Select("EQUNR=" & objChange47_objRTFS.EQUNR)
                End If

                ms = New MemoryStream()
                formatter = New BinaryFormatter()
                formatter.Serialize(ms, objChange47_objRTFS)
                b = ms.ToArray
                ms = New MemoryStream(b)
                DetailArray(1, 0) = ms
                DetailArray(1, 1) = "objChange47_objRTFS"

                'Pruefen, ob schon in der Autorisierung.
                Dim strInitiator As String = ""
                Dim intAuthorizationID As Int32

                m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, objSuche.REFERENZ,
                                                   mstr_FIN, m_User.IsTestUser, strInitiator, intAuthorizationID)
                If Not strInitiator.Length = 0 Then
                    'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                    lblError.Text = "Der Status wurde bereits geändert und das Fahrzeug liegt zur Autorisierung vor!"
                    Exit Sub
                End If
                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                logApp.CollectDetails("Kundennummer", CType(objChange47_objRTFS.Customer.TrimStart("0"c), Object), True)
                logApp.CollectDetails("Rechnungsnummer", CType(e.Item.Cells(1).Text, Object))
                logApp.CollectDetails("Fahrgestellnummer", CType(mstr_FIN, Object))
                logApp.CollectDetails("Kennzeichen", CType(e.Item.Cells(3).Text, Object))
                logApp.CollectDetails("Versanddatum", CType(e.Item.Cells(4).Text, Object))
                logApp.CollectDetails("Abrufgrund", CType(e.Item.Cells(6).Text, Object))

                intAuthorizationID = WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName,
                                                        m_User.Organization.OrganizationId, objSuche.REFERENZ, mstr_FIN,
                                                        e.Item.Cells(3).Text, " ", m_User.IsTestUser, DetailArray)

                logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")),
                                  m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString,
                                  objSuche.REFERENZ, "Statusänderung für Händler " & objSuche.REFERENZ & " erfolgreich initiiert.",
                                  m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)

                lblError.Text = "Ihre Änderungen wurden zur Autorisierung gesichert."
            Else
                'nur wenn keine autorisierung
                objChange47_objRTFS.Change()
                Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                logApp.CollectDetails("Kundennummer", CType(objChange47_objRTFS.Customer.TrimStart("0"c), Object), True)
                logApp.CollectDetails("Rechnungsnummer", CType(e.Item.Cells(1).Text, Object))
                logApp.CollectDetails("Fahrgestellnummer", CType(mstr_FIN, Object))
                logApp.CollectDetails("Kennzeichen", CType(e.Item.Cells(3).Text, Object))
                logApp.CollectDetails("Versanddatum", CType(e.Item.Cells(4).Text, Object))
                logApp.CollectDetails("Abrufgrund", CType(e.Item.Cells(6).Text, Object))
                logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, objSuche.REFERENZ, "Statusänderung für Händler " & objSuche.REFERENZ & " erfolgreich durchgeführt.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)

            End If

            If Not objChange47_objRTFS.Status = 0 Then
                lblError.Text = objChange47_objRTFS.Message
                lblError.Visible = True
                Exit Sub
            End If


            Dim cell As TableCell
            Dim oLabel As Label
            Dim oLinkButton As LinkButton
            Dim control As Control
            Dim sMsg As String
            If objChange47_objRTFS.Message.Length > 0 Then
                sMsg = objChange47_objRTFS.Message
            Else
                sMsg = "Vorgang OK"
            End If
            cell = e.Item.Cells(9)
            For Each control In cell.Controls
                Dim label = TryCast(control, Label)
                If (label IsNot Nothing) Then
                    oLabel = label
                    If oLabel.ID = "lblStatus" Then
                        oLabel.Visible = True
                        oLabel.Text = sMsg
                    End If
                End If
                Dim linkButton = TryCast(control, LinkButton)
                If (linkButton IsNot Nothing) Then
                    oLinkButton = linkButton
                    If oLinkButton.ID = "lbStatus" Then
                        oLinkButton.Visible = False
                    End If
                End If
            Next
            CType(e.Item.FindControl("cboLastschrifterzeugung"), DropDownList).Visible = False
        End If

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim strInitiator As String = ""
        Dim strFahrgestellnummer As String = ""
        Dim strKennzeichen As String = ""
        Dim strVersanddatum As String = ""
        Dim strAbrufgrund As String = ""

        Dim intAuthorizationID As Int32
        Dim logApp As New Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        For Each item In DataGrid1.Items '
            cell = item.Cells(2)
            If Not cell.Text.Length = 0 Then
                strFahrgestellnummer = cell.Text
            End If
            cell = item.Cells(3)
            If Not cell.Text.Length = 0 Then
                strKennzeichen = cell.Text
            End If
            cell = item.Cells(4)
            If Not cell.Text.Length = 0 Then
                strVersanddatum = cell.Text
            End If
            cell = item.Cells(6)
            If Not cell.Text.Length = 0 Then
                strAbrufgrund = cell.Text
            End If
            m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, objSuche.REFERENZ, strFahrgestellnummer, m_User.IsTestUser, strInitiator, intAuthorizationID)
            If Not strInitiator.Length = 0 Then
                'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                cell = item.Cells(0)
                If Not cell.Text.Length = 0 Then
                    objChange47_objRTFS.EQUNR = cell.Text
                    objChange47_objRTFS.Change()
                    logApp.CollectDetails("Kundennummer", CType(objChange47_objRTFS.Customer.TrimStart("0"c), Object), True)
                    logApp.CollectDetails("Fahrgestellnummer", CType(strFahrgestellnummer, Object))
                    logApp.CollectDetails("Kennzeichen", CType(strKennzeichen, Object))
                    logApp.CollectDetails("Versanddatum", CType(strVersanddatum, Object))
                    logApp.CollectDetails("Abrufgrund", CType(strAbrufgrund, Object))
                End If
                
            End If
        Next


        If Not objChange47_objRTFS.Status = 0 Then
            lblError.Text = objChange47_objRTFS.Message
            ' WriteLog("Fehler bei Änderung Status für Fahrgestellnummer " & strFahrgestellnummer & " , (Fehler: " & objChange47_objRTFS.Message & ")", objChange47_objRTFS.KUNNR, "ERR")
            logApp.WriteEntry("ERR",
                              m_User.UserName,
                              Session.SessionID,
                              CInt(Session("AppID")),
                              m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString,
                              objSuche.REFERENZ,
                              "Statusänderung für Händler " & objSuche.REFERENZ & " nicht erfolgreich durchgeführt.",
                              m_User.CustomerName,
                              m_User.Customer.CustomerId,
                              m_User.IsTestUser,
                              0,
                              logApp.InputDetails)

            lblError.Visible = True
        Else
            logApp.WriteEntry("APP",
                              m_User.UserName,
                              Session.SessionID,
                              CInt(Session("AppID")),
                              m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString,
                              objSuche.REFERENZ,
                              "Statusänderung für Händler " & objSuche.REFERENZ & " erfolgreich durchgeführt.",
                              m_User.CustomerName,
                              m_User.Customer.CustomerId,
                              m_User.IsTestUser,
                              0,
                              logApp.InputDetails)

            DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
            Session("Authorization") = Nothing
            Session("AuthorizationID") = Nothing
            Session("objSuche") = objSuche
            lblError.Text = "Vorgang autorisiert!"
            DataGrid1.Visible = False
            'kontingente aktuallisieren
            If objHaendler Is Nothing Then
                objHaendler = CType(Session("App_objHaendler"), BankBaseCredit)
            End If
            objHaendler.Show(Session("AppID").ToString, Session.SessionID)
            kopfdaten1.Kontingente = objHaendler.Kontingente
        End If
    End Sub

    'Private Sub WriteLog(ByVal strMessage As String, ByVal strHaendler As String, Optional ByVal strType As String = "APP")
    '    logApp.WriteEntry(strType, m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(strHaendler, 5), strMessage, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
    'End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
' ************************************************
' $History: Change47_02.aspx.vb $
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 5.01.10    Time: 11:19
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 4.01.10    Time: 16:35
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 28.12.09   Time: 16:53
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 23.12.09   Time: 16:36
' Updated in $/CKAG/Components/ComCommon/Finance
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 22.06.09   Time: 13:18
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2918 Z_M_Creditlimit_Detail_001
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance
' ITa 2918 testfertig
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon/Finance
' mögliche try catches entfernt
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 16.03.09   Time: 16:55
' Updated in $/CKAG/Components/ComCommon/Finance
' autorisierungsänderung/Berichtung RTFS
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 18.09.08   Time: 13:19
' Updated in $/CKAG/Components/ComCommon/Finance
' ita 2242
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 17.09.08   Time: 12:39
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2242 fertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 28.08.08   Time: 15:14
' Updated in $/CKAG/Components/ComCommon/Finance
' ITA 2124 fertig
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
' *****************  Version 13  *****************
' User: Jungj        Date: 26.02.08   Time: 16:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1733
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 15.02.08   Time: 11:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA:1677
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 11.02.08   Time: 11:37
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1701/1702
' 
' *****************  Version 10  *****************
' User: Uha          Date: 4.02.08    Time: 10:05
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Page_PreRender und Page_Unload hinzugefügt
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 25.01.08   Time: 13:51
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Rothe Verbesserungen RTFS TEIL 2
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 24.01.08   Time: 14:30
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 18.01.08   Time: 10:57
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA 1624 fertig
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 17.01.08   Time: 12:38
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA: 1617
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 8.01.08    Time: 16:13
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' Bugfix RTFS
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 8.01.08    Time: 12:48
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance
' ITA:1515
' 
' ************************************************
