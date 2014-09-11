Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Threading

Public Class Change07_3
    Inherits System.Web.UI.Page

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
    Private objPDIs As SIXT_PDI
    Private objPDIsThread As SIXT_PDI
    Private objBlocken As Sixt_B15
    Private m_blnPDIError As Boolean

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkKreditlimit As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucHeader As Header
    <CLSCompliant(False)> Protected WithEvents HG1 As DBauer.Web.UI.WebControls.HierarGrid
    Protected WithEvents cmdWeiter As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdAbsenden As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdVerwerfen As System.Web.UI.WebControls.LinkButton
    Protected WithEvents litAddScript As System.Web.UI.WebControls.Literal
    Protected WithEvents cmdZurueck As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents rowExcelLink As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents cmdWeitereAuswahl As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ZZFAHRG_WDB2112611A592199 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Dim thread As SixtThread

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        lnkKreditlimit.NavigateUrl = "Change07.aspx?AppID=" & Session("AppID").ToString
        litAddScript.Text = ""
        Try
            'If (Session("objPDIs") Is Nothing) Then
            '    Response.Redirect(Request.UrlReferrer.ToString)
            'Else
            '    objPDIs = CType(Session("objPDIs"), SIXT_PDI)
            'End If

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text



            m_App = New Base.Kernel.Security.App(m_User)

            'Dim logApp As New Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
            'logApp.WriteEntry("DBG", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "Page_Load Change06_2 gestartet.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            If Not IsPostBack Then

                rowExcelLink.Visible = False

                If IsNothing(objBlocken) = True Then
                    objBlocken = Session("clsBlock")
                End If

                objBlocken.GiveData(CInt(Session("AppID")), Session.SessionID, Me)


                If objBlocken.Aktion = 4 Then
                    cmdWeiter.Visible = False
                    cmdAbsenden.Visible = False
                    cmdVerwerfen.Visible = False
                    cmdWeitereAuswahl.Visible = False
                    cmdZurueck.Visible = True
                    DisableAllInput()
                End If


                If objBlocken.Status = 0 Then
                    FillGrid(-1)
                Else
                    lblError.Text = objBlocken.Message
                    cmdWeiter.Visible = False
                    cmdAbsenden.Visible = False
                    cmdVerwerfen.Visible = False
                    cmdWeitereAuswahl.Visible = False
                    cmdZurueck.Visible = True
                End If

                'Select Case objPDIs.Task
                '    Case "Entsperren"
                '        lblMessage.Text = "Es stehen " & objPDIs.FahrzeugeGesamtGesperrt.ToString & " Fahrzeuge zum " & objPDIs.Task & " zur Verfügung."
                '    Case Else
                '        lblMessage.Text = "Es stehen " & objPDIs.FahrzeugeGesamtZulassungsf.ToString & " Fahrzeuge zum " & objPDIs.Task & " zur Verfügung."
                'End Select
            Else
                Dim blnInputPerformed As Boolean
                If Request.Form("__EVENTTARGET").ToString = "cmdAbsenden" Or Request.Form("__EVENTTARGET").ToString = "cmdWeitereAuswahl" Then
                    blnInputPerformed = False
                Else
                    blnInputPerformed = CheckInput()
                End If

                If blnInputPerformed Then
                    FillGrid(-1)
                End If


            End If


            'logApp.WriteEntry("DBG", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "Page_Load Change06_2 beendet.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub HG1_TemplateSelection(ByVal sender As Object, ByVal e As DBauer.Web.UI.WebControls.HierarGridTemplateSelectionEventArgs) Handles HG1.TemplateSelection
        Select Case (e.Row.Table.TableName)
            Case "Modelle"
                e.TemplateFilename = "..\Templates\BlkModell.ascx"
            Case Else
                Throw New NotImplementedException("Unexpected child row in TemplateSelection event")
        End Select
    End Sub

    Private Sub FillGrid(ByVal intExpand As Int32, Optional ByVal blnSelectedOnly As Boolean = False)

        If IsNothing(objBlocken) = True Then
            objBlocken = Session("clsBlock")
        End If

        Select Case objBlocken.Aktion
            Case 2
                lblPageTitle.Text = "(Geblockte Fahrzeuge freigeben)"
            Case 4
                lblPageTitle.Text = "(Alle offenen Regeln)"
            Case 5
                lblPageTitle.Text = "(Vorschlagsliste bearbeiten)"
        End Select


        'bind the DataSet to the HierarGrid
        If blnSelectedOnly Then
            HG1.DataSource = objBlocken.Blocken_Data_Selected
        Else
            HG1.DataSource = objBlocken.Blocken_Data
        End If
        HG1.DataMember = "Regeln"
        HG1.DataBind()


        Dim item As DataGridItem
        Dim cell As TableCell
        Dim control As Control
        Dim checkbox As CheckBox
        Dim intItem As Int32 = 0

        For Each item In HG1.Items
            'Dim strPDI_Nummer As String = item.Cells(1).Text
            'Dim intValidCounter As Int32
            'If objPDIs.Task = "Entsperren" Then
            '    intValidCounter = CInt(item.Cells(6).Text)
            'Else
            '    intValidCounter = CInt(item.Cells(5).Text)
            'End If
            Dim blnDetails As Boolean = False
            Dim blnLoaded As Boolean = False
            cell = item.Cells(1)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    checkbox = CType(control, CheckBox)
                    If checkbox.ID = "chkDetails" And checkbox.Checked Then
                        blnDetails = True
                    End If
                    If checkbox.ID = "chkLoaded" And checkbox.Checked Then
                        blnLoaded = True
                    End If
                End If
            Next

            cell = item.Cells(0)
            cell.Width = New Unit("100px")
            cell.HorizontalAlign = HorizontalAlign.Center

            If Not blnLoaded Then
                'If intValidCounter > 0 Then
                'Dim button As New ImageButton()
                'cell.Controls.Add(button)
                'button.ImageUrl = "../Images/plus.gif"
                'button.Attributes.Add("onClick", "DetailsSuchen('" & strPDI_Nummer.Trim(","c) & "'," & intItem & ",'" & objPDIs.Task & "');")
                'Else
                '    Dim image As New System.Web.UI.WebControls.Image()
                '    cell.Controls.Add(image)
                '    image.ImageUrl = "../Images/empty.gif"
                '    'Dim literal As New LiteralControl()
                '    'cell.Controls.Add(literal)
                '    'literal.Text = "Keine Fahrzeuge"
                '    If intExpand = intItem Then
                '        intExpand = -1
                '    End If
                'End If
            Else
                If Not blnDetails Then
                    Dim image As New System.Web.UI.WebControls.Image()
                    cell.Controls.Add(image)
                    image.ImageUrl = "../Images/empty.gif"
                    'Dim literal As New LiteralControl()
                    'cell.Controls.Add(literal)
                    'literal.Text = "Keine Fahrzeuge"
                    If intExpand = intItem Then
                        intExpand = -1
                    End If
                End If
            End If

            If blnSelectedOnly Then
                HG1.RowExpanded(intItem) = True
            End If

            intItem += 1
        Next

        If intExpand > -1 Then
            HG1.RowExpanded(intExpand) = True
        End If

        Session("clsBlock") = objBlocken
    End Sub

    Private Sub cmdWeiter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWeiter.Click
        If objBlocken.SelectCars > 0 Then

            If objBlocken.CheckAnzahlFahrzeuge = True Then
                lblError.Text = "Die Anzahl der ausgewählten Fahrzeuge für die Regel ist zu hoch."
                FillGrid(-1)
                Exit Sub
            End If

            FillGrid(-1, True)
            cmdWeiter.Visible = False
            cmdAbsenden.Visible = True
            cmdVerwerfen.Visible = True
            cmdWeitereAuswahl.Visible = True
            cmdZurueck.Visible = False
            DisableAllInput()
            lblMessage.Text = "Anzahl Fahrzeuge: " & objBlocken.SelectedCars.ToString
        Else
            lblError.Text = "Keine Fahrzeuge ausgewählt."
            FillGrid(-1)
        End If
    End Sub

    Private Sub cmdAbsenden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAbsenden.Click

        If IsNothing(objBlocken) = True Then
            objBlocken = Session("clsBlock")
        End If


        lblPageTitle.Text = "Bestätigung der Datenübernahme"

        objBlocken.SaveData(CInt(Session("AppID")), Session.SessionID, Me)

        'Dim logApp As New Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        lblMessage.Text = objBlocken.SelectedCars.ToString & " Fahrzeuge übernommen."



        'Dim objExcelExport As New Excel.ExcelExport()
        'Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        'Try
        '    objExcelExport.WriteExcel(objPDIs.Erledigt, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
        '    lnkExcel.NavigateUrl = "../../../Temp/Excel/" & strFileName
        '    rowExcelLink.Visible = True
        'Catch
        'End Try

        If objBlocken.SelectCars > 0 Then
            FillGrid(-1, True)
        End If

        cmdWeiter.Visible = False
        cmdAbsenden.Visible = False
        cmdVerwerfen.Visible = False
        cmdWeitereAuswahl.Visible = False
        cmdZurueck.Visible = True
        DisableAllInput()
    End Sub

    Private Sub cmdVerwerfen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdVerwerfen.Click
        Dim i As Int32
        For i = 0 To objBlocken.Blocken_Data.Tables("Fahrzeuge").Rows.Count - 1
            objBlocken.Blocken_Data.Tables("Fahrzeuge").Rows(i)("Ausgewaehlt") = False
        Next

        objBlocken.SelectCars()
        lblMessage.Text = ""
        FillGrid(-1)

        cmdWeiter.Visible = True
        cmdWeitereAuswahl.Visible = False
        cmdAbsenden.Visible = False
        cmdVerwerfen.Visible = False
        cmdZurueck.Visible = False
    End Sub

    Private Sub DisableAllInput()
        Dim strTemp As String = "			<script language=""JavaScript"">" & vbCrLf
        strTemp &= "		<!-- //" & vbCrLf
        strTemp &= "		  DisableControls();" & vbCrLf
        strTemp &= "		//-->" & vbCrLf
        strTemp &= "			</script>"
        litAddScript.Text = strTemp
        lblPageTitle.Text = "(Kontrolle der Fahrzeugauswahl)"
    End Sub

    Private Function CheckInput() As Boolean
        Dim blnReturn As Boolean = False

        'Dim logApp As New Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        'logApp.WriteEntry("DBG", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "CheckInput gestartet.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        If IsNothing(objBlocken) = True Then
            objBlocken = Session("clsBlock")
        End If


        Try
            Dim i As Int32
            Dim j As Int32
            For i = 0 To Request.Form.Count - 1
                If InStr(Request.Form.Keys.Item(i), "ZZFAHRG_") > 0 Then
                    Dim strFahrgestellNummer As String = (Right(Request.Form.Keys.Item(i), 17))
                    Dim strAuswahl_alt As String = CStr(Request.Form(i))
                    Dim intPos As Int32 = InStr(strAuswahl_alt, ",")
                    If intPos > 0 Then
                        strAuswahl_alt = Left(strAuswahl_alt, intPos - 1)
                    End If
                    Dim blnAuswahl_alt As Boolean = CType(strAuswahl_alt, Boolean)
                    Dim blnAuswahl_neu As Boolean = False

                    If i + 1 = Request.Form.Count Then Exit For


                    If UCase(Request.Form(i + 1).ToString) = "ON" And InStr(Request.Form.Keys.Item(i + 1), "chkAuswahl") > 0 Then
                        blnAuswahl_neu = True

                    End If
                    'If Not (blnAuswahl_alt = blnAuswahl_neu) Then
                    blnReturn = True
                    For j = 0 To objBlocken.Blocken_Data.Tables("Fahrzeuge").Rows.Count - 1
                        If objBlocken.Blocken_Data.Tables("Fahrzeuge").Rows(j)("FIN").ToString = strFahrgestellNummer Then
                            objBlocken.Blocken_Data.Tables("Fahrzeuge").Rows(j)("Ausgewaehlt") = blnAuswahl_neu

                        End If
                    Next

                End If

            Next

            blnReturn = True

            'ToDo: Wenn die Anzahl der gewählten Fahrzeuge größer als die Anzahl der Regeln ist, dann Fehler





        Catch ex As Exception
            'logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "Fehler bei der Prüfung der Dateneingaben. (" & ex.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
            lblError.Text = "Fehler bei der Prüfung der Dateneingaben.<br>(" & ex.Message & ")"
            blnReturn = False
        End Try
        'logApp.WriteEntry("DBG", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CStr(Session("AppID")), "CheckInput beendet.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)
        Return blnReturn
    End Function

    Private Sub cmdZurueck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdZurueck.Click
        'Session("ShowLink") = "True"
        Response.Redirect("Change07.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub cmdWeitereAuswahl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdWeitereAuswahl.Click
        lblMessage.Text = ""
        FillGrid(-1)

        cmdWeiter.Visible = True
        cmdWeitereAuswahl.Visible = False
        cmdAbsenden.Visible = False
        cmdVerwerfen.Visible = False
        cmdZurueck.Visible = False
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change07_3.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:16
' Updated in $/CKAG/Applications/appsixt/Forms
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Forms
' 
' *****************  Version 3  *****************
' User: Uha          Date: 2.07.07    Time: 14:09
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 2  *****************
' User: Uha          Date: 21.05.07   Time: 15:28
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Änderungen im Vergleich zur Startapplikation zum Stand 21.05.2007
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 11:23
' Created in $/CKG/Applications/AppSIXT/AppSIXTWeb/Forms
' Änderungen aus der StartApplication zum Stand 02.05.2007 Mittags
' übernommen
' 
' ************************************************