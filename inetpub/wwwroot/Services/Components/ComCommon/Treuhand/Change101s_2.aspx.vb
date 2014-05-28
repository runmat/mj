Option Strict On
Option Explicit On

Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business
Imports Telerik.Web.UI
Imports Telerik.Web.UI.GridExcelBuilder
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Namespace Treuhand
    Partial Public Class Change101_2s
        Inherits System.Web.UI.Page

        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private isExcelExportConfigured As Boolean
        Dim m_report As Treuhandsperre

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            m_User = Common.GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            Common.FormAuth(Me, m_User)

            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            lblError.Text = ""

            If Not Session("FreigabeObject") Is Nothing Then
                m_report = CType(Session("FreigabeObject"), Treuhandsperre)
            End If

            If Not IsPostBack Then
                Common.TranslateTelerikColumns(rgGrid1)

                doSubmit()
            End If
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            Common.SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            Common.SetEndASPXAccess(Me)
        End Sub

        Private Sub doSubmit()
            FillGrid()
        End Sub

        Private Sub FillGrid()

            m_report.Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)
            Session("FreigabeObject") = m_report

            If m_report.Status < 0 Then
                lblError.Text = m_report.Message
            Else
                If m_report.Result.Rows.Count = 0 Then
                    SearchMode()
                    lblError.Text = "Keine Daten gefunden."
                Else
                    SearchMode(False)

                    rgGrid1.Rebind()
                    'Setzen der DataSource geschieht durch das NeedDataSource-Event

                    CheckgridForAutorisierung()
                End If
            End If

        End Sub

        Private Sub SearchMode(Optional search As Boolean = True)
            Result.Visible = Not search
            cmdFreigabe.Visible = Not search
            cmdAblehnen.Visible = Not search
        End Sub

        Protected Sub rgGrid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
            If m_report IsNot Nothing Then
                rgGrid1.DataSource = m_report.Result.DefaultView
            Else
                rgGrid1.DataSource = Nothing
            End If
        End Sub

        Private Sub CheckgridForAutorisierung()
            Dim label As Label
            Dim strInitiator As String = ""
            Dim strFin As String
            Dim strTreunehmer As String
            Dim chkBox As CheckBox
            Dim txtBox As TextBox

            Dim intAutID As Int32 = 0

            For Each item As GridDataItem In rgGrid1.Items
                strFin = item("CHASSIS_NUM").Text
                strTreunehmer = item("NAME1_AG").Text
                m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, strTreunehmer, strFin, m_User.IsTestUser, strInitiator, intAutID)
                If Not strInitiator.Length = 0 Then
                    label = CType(item.FindControl("lblInAut"), WebControls.Label)
                    label.Visible = True
                    chkBox = CType(item.FindControl("chkSperre"), CheckBox)
                    chkBox.Checked = False
                    chkBox.Visible = False
                    txtBox = CType(item.FindControl("txtAblehnungsgrund"), TextBox)
                    txtBox.Enabled = False
                End If
            Next

        End Sub

        Private Function Checkgrid(ByVal FreigabeFlag As String) As Boolean
            Dim chkBox As CheckBox
            Dim bchecked As Boolean
            Dim txtBox As TextBox
            bchecked = False

            For Each item As GridDataItem In rgGrid1.Items
                chkBox = CType(item.FindControl("chkSperre"), CheckBox)
                If chkBox.Checked = True Then
                    Dim dRow As DataRow = m_report.Result.Select("BELNR='" & item("BELNR").Text & "'")(0)
                    dRow("zurFreigabe") = "X"
                    If FreigabeFlag = "A" Then
                        txtBox = CType(item.FindControl("txtAblehnungsgrund"), TextBox)
                        If txtBox.Text.Trim.Length > 0 Then
                            dRow("NICHT_FREIG_GRU") = txtBox.Text
                            bchecked = True
                        Else
                            item("CHASSIS_NUM").BackColor = System.Drawing.Color.FromArgb(196, 0, 0)
                            lblError.Text = "Bitte geben Sie für die markierten Zeilen einen Ablehnungsgrund an!"
                            bchecked = False
                        End If
                    Else
                        bchecked = True
                    End If

                    m_report.Result.AcceptChanges()

                End If
            Next

            Session("ResultTable") = m_report.Result

            If lblError.Text <> "" Then
                Return False
            Else
                Return bchecked
            End If

        End Function

        Protected Sub cmdFreigabe_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdFreigabe.Click
            If Checkgrid("F") Then
                Dim aktionAlt As String = m_report.Aktion
                m_report.Aktion = "F"
                Dim iGroupLevel As Integer = m_User.Groups(0).Authorizationright
                Dim iAppLevel As Integer = CInt(m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AuthorizationLevel"))

                If m_User.Groups.ItemByID(m_User.GroupID).Authorizationright > 0 Then
                    For Each tmpRow As DataRow In m_report.Result.Rows
                        If tmpRow("zurFreigabe").ToString = "X" Then
                            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                            logApp.CollectDetails("Treunehmer", CType(tmpRow("NAME1_AG").ToString, Object), True)
                            logApp.CollectDetails("Angefordert am", CType(Now.ToShortDateString, Object))
                            logApp.CollectDetails("Fahrgestellnummer", CType(tmpRow("CHASSIS_NUM").ToString, Object))
                            logApp.CollectDetails("Kennzeichen", CType(tmpRow("LICENSE_NUM").ToString, Object))
                            logApp.CollectDetails("Freigabe", "X")
                            logApp.CollectDetails("Abgelehnt", "")
                            m_report.FinforAut = tmpRow("Fahrgestellnummer").ToString
                            'Anwendung erfordert Autorisierung (Level>0)
                            Dim DetailArray(1, 2) As Object
                            Dim ms As MemoryStream
                            Dim formatter As BinaryFormatter
                            Dim b() As Byte

                            ms = New MemoryStream()
                            formatter = New BinaryFormatter()
                            formatter.Serialize(ms, m_report)
                            b = ms.ToArray
                            ms = New MemoryStream(b)
                            DetailArray(0, 0) = ms
                            DetailArray(0, 1) = "objReport"

                            'Pruefen, ob schon in der Autorisierung.
                            Dim strInitiator As String = ""
                            Dim intAuthorizationID As Int32

                            m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, m_User.KUNNR, tmpRow("CHASSIS_NUM").ToString, m_User.IsTestUser, strInitiator, intAuthorizationID)
                            If Not strInitiator.Length = 0 Then
                                'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                                lblError.Text = "Diese Anforderung wurde schon einmal freigegeben und liegt bereits zur Autorisierung vor!"
                                Exit Sub
                            End If

                            intAuthorizationID = Common.WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, tmpRow("NAME1_AG").ToString, tmpRow("CHASSIS_NUM").ToString, "Freigabe", "", m_User.IsTestUser, DetailArray)
                            tmpRow("SPERRSTATUS") = "Aut."
                            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, tmpRow("NAME1_AG").ToString, "Freigabe für Treunehmer " & tmpRow("NAME1_AG").ToString & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                            CheckgridForAutorisierung()
                        End If
                    Next
                Else
                    'Anwendung erfordert keine Autorisierung (Level=0)
                    m_report.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)
                    If m_report.Status < 0 Then
                        lblError.Text = m_report.Message
                        SearchMode()
                    Else
                        'ursprünglichen Suchfilter wiederherstellen
                        m_report.Aktion = aktionAlt
                        doSubmit()
                    End If
                End If
            End If
        End Sub

        Protected Sub cmdAblehnen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdAblehnen.Click
            If Checkgrid("A") Then
                Dim aktionAlt As String = m_report.Aktion
                m_report.Aktion = "A"
                If m_User.Groups.ItemByID(m_User.GroupID).Authorizationright > 0 Then
                    For Each tmpRow As DataRow In m_report.Result.Rows
                        If tmpRow("zurFreigabe").ToString = "X" Then
                            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                            logApp.CollectDetails("Treunehmer", CType(tmpRow("NAME1_AG").ToString, Object), True)
                            logApp.CollectDetails("Angefordert am", CType(Now.ToShortDateString, Object))
                            logApp.CollectDetails("Fahrgestellnummer", CType(tmpRow("CHASSIS_NUM").ToString, Object))
                            logApp.CollectDetails("Kennzeichen", CType(tmpRow("LICENSE_NUM").ToString, Object))
                            logApp.CollectDetails("Freigabe", "")
                            logApp.CollectDetails("Abgelehnt", "X")
                            m_report.FinforAut = tmpRow("CHASSIS_NUM").ToString
                            'Anwendung erfordert Autorisierung (Level>0)
                            Dim DetailArray(1, 2) As Object
                            Dim ms As MemoryStream
                            Dim formatter As BinaryFormatter
                            Dim b() As Byte

                            ms = New MemoryStream()
                            formatter = New BinaryFormatter()
                            formatter.Serialize(ms, m_report)
                            b = ms.ToArray
                            ms = New MemoryStream(b)
                            DetailArray(0, 0) = ms
                            DetailArray(0, 1) = "objReport"

                            'Pruefen, ob schon in der Autorisierung.
                            Dim strInitiator As String = ""
                            Dim intAuthorizationID As Int32

                            m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, m_User.KUNNR, tmpRow("CHASSIS_NUM").ToString, m_User.IsTestUser, strInitiator, intAuthorizationID)
                            If Not strInitiator.Length = 0 Then
                                'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                                lblError.Text = "Diese Anforderung wurde schon einmal freigegeben und liegt bereits zur Autorisierung vor!"
                                Exit Sub
                            End If

                            intAuthorizationID = Common.WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, tmpRow("NAME1_AG").ToString, tmpRow("CHASSIS_NUM").ToString, "Ablehnung", "", m_User.IsTestUser, DetailArray)
                            tmpRow("SPERRSTATUS") = "Aut."
                            logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, tmpRow("NAME1_AG").ToString, "Ablehnung für Treunehmer " & tmpRow("NAME1_AG").ToString & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                            CheckgridForAutorisierung()
                        End If
                    Next
                Else
                    'm_report.Treunehmer = m_User.Reference
                    m_report.Aktion = "A"
                    m_report.Change(Session("AppID").ToString, Session.SessionID.ToString, Me)
                    If m_report.Status < 0 Then
                        lblError.Text = m_report.Message
                        SearchMode()
                    Else
                        'ursprünglichen Suchfilter wiederherstellen
                        m_report.Aktion = aktionAlt
                        doSubmit()
                    End If
                End If

            End If

        End Sub

        Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
            Response.Redirect("Change101s.aspx?AppID=" & Session("AppID").ToString)
        End Sub

        Protected Sub rgGrid1_ItemCommand(sender As Object, e As GridCommandEventArgs)
            Select Case e.CommandName

                Case RadGrid.ExportToExcelCommandName
                    Dim eSettings As GridExportSettings = rgGrid1.ExportSettings
                    eSettings.ExportOnlyData = True

                    eSettings.FileName = String.Format("TreuhandServices_{0:yyyyMMdd}", DateTime.Now)
                    eSettings.HideStructureColumns = True
                    eSettings.IgnorePaging = True
                    eSettings.OpenInNewWindow = True
                    ' hide non display columns from excel export
                    For Each col As GridColumn In rgGrid1.MasterTableView.Columns
                        If TypeOf col Is GridEditableColumn AndAlso Not col.Display Then
                            col.Visible = False
                        End If
                    Next
                    rgGrid1.Rebind()
                    rgGrid1.MasterTableView.ExportToExcel()

            End Select
        End Sub

        Protected Sub rgGrid1_ExcelMLExportRowCreated(sender As Object, e As GridExportExcelMLRowCreatedArgs)

            If e.RowType = GridExportExcelMLRowType.DataRow Then

                If Not isExcelExportConfigured Then
                    e.Worksheet.Name = "Seite 1"

                    'Set Page options
                    Dim layout As PageLayoutElement = e.Worksheet.WorksheetOptions.PageSetup.PageLayoutElement
                    layout.IsCenteredVertical = True
                    layout.IsCenteredHorizontal = True
                    layout.PageOrientation = PageOrientationType.Landscape
                    Dim margins As PageMarginsElement = e.Worksheet.WorksheetOptions.PageSetup.PageMarginsElement
                    margins.Left = 0.5
                    margins.Top = 0.5
                    margins.Right = 0.5
                    margins.Bottom = 0.5

                    'Freeze panes
                    Dim wso As WorksheetOptionsElement = e.Worksheet.WorksheetOptions
                    wso.AllowFreezePanes = True
                    wso.LeftColumnRightPaneNumber = 1
                    wso.TopRowBottomPaneNumber = 1
                    wso.SplitHorizontalOffset = 1
                    wso.SplitVerticalOffest = 1
                    wso.ActivePane = 2

                    isExcelExportConfigured = True
                End If

            End If

        End Sub

        Protected Sub rgGrid1_ExcelMLExportStylesCreated(sender As Object, e As GridExportExcelMLStyleCreatedArgs)

            'Add currency and percent styles
            Dim priceStyle As New StyleElement("priceItemStyle")
            priceStyle.NumberFormat.FormatType = NumberFormatType.Currency
            priceStyle.FontStyle.Color = System.Drawing.Color.Red
            e.Styles.Add(priceStyle)

            Dim alternatingPriceStyle As New StyleElement("alternatingPriceItemStyle")
            alternatingPriceStyle.NumberFormat.FormatType = NumberFormatType.Currency
            alternatingPriceStyle.FontStyle.Color = System.Drawing.Color.Red
            e.Styles.Add(alternatingPriceStyle)

            Dim percentStyle As New StyleElement("percentItemStyle")
            percentStyle.NumberFormat.FormatType = NumberFormatType.Percent
            percentStyle.FontStyle.Italic = True
            e.Styles.Add(percentStyle)

            Dim alternatingPercentStyle As New StyleElement("alternatingPercentItemStyle")
            alternatingPercentStyle.NumberFormat.FormatType = NumberFormatType.Percent
            alternatingPercentStyle.FontStyle.Italic = True
            e.Styles.Add(alternatingPercentStyle)

            'Apply background colors 
            For Each style As StyleElement In e.Styles
                If style.Id = "headerStyle" Then
                    style.InteriorStyle.Pattern = InteriorPatternType.Solid
                    style.InteriorStyle.Color = System.Drawing.Color.Gray
                End If
                If style.Id = "alternatingItemStyle" Or style.Id = "alternatingPriceItemStyle" Or style.Id = "alternatingPercentItemStyle" Or style.Id = "alternatingDateItemStyle" Then
                    style.InteriorStyle.Pattern = InteriorPatternType.Solid
                    style.InteriorStyle.Color = System.Drawing.Color.LightGray
                End If
                If style.Id.Contains("itemStyle") Or style.Id = "priceItemStyle" Or style.Id = "percentItemStyle" Or style.Id = "dateItemStyle" Then
                    style.InteriorStyle.Pattern = InteriorPatternType.Solid
                    style.InteriorStyle.Color = System.Drawing.Color.White
                End If
            Next

        End Sub

    End Class

End Namespace
