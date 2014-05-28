Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports System.Threading

Partial Public Class Change02_1
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As Search
    Private objFDDBank As F1_BankBase
    Private objFDDBank2 As F1_Bank_2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)

            If m_User.Organization.AllOrganizations Then
                lnkKreditlimit.Visible = True
            Else
                lnkKreditlimit.Visible = False
            End If


            If (Request.QueryString("ShowAll") = "True") And (Not IsPostBack) Then
                Session("objSuche") = Nothing
            End If


            If (Not IsPostBack) Or (Session("objFDDBank2") Is Nothing) Then
                'Daten aus SAP laden
                objFDDBank2 = New F1_Bank_2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                objFDDBank2.AppID = Session("AppID").ToString
                objFDDBank2.CreditControlArea = "ZDAD"
                objFDDBank2.Customer = m_User.KUNNR

                objFDDBank2.Show(Session("AppID").ToString, Session.SessionID)

                If Not Session("objSuche") Is Nothing Then
                    objSuche = CType(Session("objSuche"), Search)
                    objFDDBank2.Haendler = objSuche.REFERENZ
                End If


                Select Case objFDDBank2.Status
                    Case 0
                        FillGrid(objFDDBank2, 0)
                        Session("objFDDBank2") = objFDDBank2
                    Case -9999
                        trDataGrid1.Visible = False
                        lblError.Text = "Fehler bei der Ermittlung der gesperrten Aufträge.<br>(" & objFDDBank2.Message & ")"
                    Case Else
                        trDataGrid1.Visible = False
                        lblError.Text = objFDDBank2.Message
                End Select
            Else
                objFDDBank2 = CType(Session("objFDDBank2"), F1_Bank_2)
            End If

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2
            End If


            If Session("objSuche") Is Nothing Then
                'noch kein Händler ausgewählt
                trKopfdaten.Visible = False
                trVorgangsArt.Visible = False
                trDataGrid1.Visible = True
                cmdSave.Visible = False

            Else
                objSuche = CType(Session("objSuche"), Search)
                'händler ausgewählt


                trKopfdaten.Visible = True
                trVorgangsArt.Visible = True        'Hier wird die Zeile der Vorgangsarten eingeblendet!
                trDataGrid1.Visible = False
                cmdSave.Visible = True

                objFDDBank = CType(Session("objFDDBank"), F1_BankBase)
                If objFDDBank.Status = 0 Then
                    Kopfdaten1.Kontingente = objSuche.Kontingente
                    cmdSave.Enabled = True
                    Session("objFDDBank") = objFDDBank
                Else
                    ddlPageSize.Visible = False
                    lblError.Text = objFDDBank.Message
                End If

                If Not IsPostBack Then

                    Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
                    Dim strTemp As String = objSuche.NAME
                    If objSuche.NAME_2.Length > 0 Then
                        strTemp &= "<br>" & objSuche.NAME_2
                    End If
                    Kopfdaten1.HaendlerName = strTemp
                    Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET

                End If

                objFDDBank2 = CType(Session("objFDDBank2"), F1_Bank_2)
                Dim vwTemp As DataView = objFDDBank2.AuftragsUebersicht.DefaultView
                vwTemp.RowFilter = "Händlernummer = '" & Session("SelectedDealer").ToString & "'"
                Dim item As ListItem
                Dim str As String
                str = String.Empty

                If (objFDDBank2.ZeigeAlle) Then
                    item = New ListItem()
                    item.Value = "0"
                    item.Text = ""
                    ddlKontingentart.Items.Add(item)
                End If

                If (objFDDBank2.ZeigeStandard) Then
                    item = New ListItem()
                    item.Value = "1"
                    item.Text = CStr(CInt(vwTemp(0)(2)) + CInt(vwTemp(0)(3)) + CInt(vwTemp(0)(6))) & " Vorgänge 'Standard'"
                    str &= "<u><b>" & CStr(CInt(vwTemp(0)(2)) + CInt(vwTemp(0)(3)) + CInt(vwTemp(0)(6))) & "</b></u> Vorgänge 'Standard'<br>"
                    ddlKontingentart.Items.Add(item)
                End If

                If (objFDDBank2.ZeigeFlottengeschaeft) Then
                    item = New ListItem()
                    item.Value = "2"
                    item.Text = CStr(vwTemp(0)(4)) & " Vorgänge 'erweitertes Zahlungsziel' (Delayed Payment)"
                    str &= "<u><b>" & CStr(vwTemp(0)(4)) & "</b></u> Vorgänge 'Erweitertes Zahlungsziel' (Delayed Payment)<br>"
                    ddlKontingentart.Items.Add(item)
                End If

                If (objFDDBank2.ZeigeHEZ) Then
                    item = New ListItem()
                    item.Value = "3"
                    item.Text = CStr(vwTemp(0)(5)) & " Vorgänge 'HEZ'" 'HEZ
                    str &= "<u><b>" & CStr(vwTemp(0)(5)) & "</b></u> Vorgänge 'HEZ'<br><br>"
                    ddlKontingentart.Items.Add(item)
                End If
              
                lblAnzeige.Text = str
                checkFastForward()
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        doSubmit()
    End Sub

    Private Sub checkFastForward()
        'wenn nur ein eintrag in der DDL ist diesen nehmen und sofort weiterspringen
        If ddlKontingentart.Items.Count = 1 Then
            doSubmit()
        End If
    End Sub

    Private Sub doSubmit()
        Dim ddlVal As String

        objFDDBank2.ZeigeAlle = False
        objFDDBank2.ZeigeFlottengeschaeft = False
        objFDDBank2.ZeigeHEZ = False
        objFDDBank2.ZeigeStandard = False


        ddlVal = ddlKontingentart.SelectedItem.Value
        Select Case ddlVal
            Case "1"
                'Standard
                objFDDBank2.ZeigeStandard = True 'rbStandard.Checked  'HEZ
            Case "2"
                'Flottengeschäft
                objFDDBank2.ZeigeFlottengeschaeft = True
            Case "3"
                'HEZ
                objFDDBank2.ZeigeHEZ = True
            Case "4"
                'KF/KL
                objFDDBank2.ZeigeKFKL = True
        End Select

        objFDDBank2.Haendler = Session("SelectedDealer").ToString

        Session("objFDDBank2") = objFDDBank2
        Response.Redirect("Change02Edit.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub FillGrid(ByVal objBank As F1_Bank_2, ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        If objBank.Status = 0 Then
            If objBank.AuftragsUebersicht.Rows.Count = 0 Then
                trDataGrid1.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                ShowScript.Visible = False
            Else
                trDataGrid1.Visible = True
                lblNoData.Visible = False

                Dim tmpDataView As New DataView()
                tmpDataView = objBank.AuftragsUebersicht.DefaultView

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

                lblNoData.Text = "Es wurden " & objBank.AuftraegeAlle.Rows.Count.ToString & " gesperrte Aufträge gefunden."
                lblNoData.Visible = True

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If
            End If
        Else
            lblError.Text = objBank.Message
            lblNoData.Visible = True
            ShowScript.Visible = False
        End If
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand

        If Not e.CommandName = "Page" AndAlso Not e.CommandName = "Sort" Then

            If Not e.Item.Cells(1).Text.Length = 0 Then
                Dim strRedirectURL As String = "Change02_2.aspx?AppID=" & Session("AppID").ToString

                objSuche = New Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
                objSuche.fillHaendlerData(Session("AppID").ToString, Session.SessionID.ToString, e.Item.Cells(1).Text)
                If Not objSuche.Status = 0 Then
                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                    Exit Sub
                End If
                Session("objSuche") = objSuche
                objFDDBank2.Haendler = objSuche.REFERENZ 'für die einschränkung auf einen händler und füllen der auftragstabelle

                objFDDBank = New F1_BankBase(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", True)
                objFDDBank.CreditControlArea = "ZDAD"
                objFDDBank.Customer = objSuche.REFERENZ
                Session("objFDDBank") = objFDDBank

                Dim int3 As Int32
                Dim int4 As Int32
                Dim int5 As Int32
                Dim int55 As Int32
                Dim int6 As Int32
                Dim counter As Integer = 3 'Anzahl Kontingentarten - 1: Standard, Delayed Payment, HEZ

                Dim label As Label
                Dim control As Control
                Dim cell As TableCell

                cell = e.Item.Cells(4)
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        int3 = CInt(label.Text)                 'Standard endgültig
                    End If
                Next

                cell = e.Item.Cells(5)
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        int4 = CInt(label.Text)                 'Standard temporär
                    End If
                Next

                cell = e.Item.Cells(6)
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        int5 = CInt(label.Text)                 'Delayed Payment
                    End If
                Next

                cell = e.Item.Cells(7)
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        int6 = CInt(label.Text)                 'KF/KL
                    End If
                Next

                cell = e.Item.Cells(8)
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        int55 = CInt(label.Text)                 'HEZ
                    End If
                Next


                objFDDBank2.ZeigeStandard = True
                objFDDBank2.ZeigeFlottengeschaeft = True
                objFDDBank2.ZeigeHEZ = True
                objFDDBank2.ZeigeKFKL = True

                If int3 + int4 + int6 = 0 Then                             'Standard endgültig + temporär = 0
                    objFDDBank2.ZeigeStandard = False
                    counter -= 1
                End If

                If int5 = 0 Then                                    'Delayed Payment = 0, dann verberge 
                    objFDDBank2.ZeigeFlottengeschaeft = False
                    counter -= 1
                End If

                If int55 = 0 Then                                   'HEZ = 0 , dann verberge
                    objFDDBank2.ZeigeHEZ = False
                    counter -= 1
                End If

                
                '------------------------------
                If (counter = 0) Then
                    'Nur eine Kontingentart enthält Daten. Direkt nach Change02Edit.aspx gehen!
                    objFDDBank2.Haendler = Session("SelectedDealer").ToString
                    Session("objFDDBank2") = objFDDBank2
                    strRedirectURL = "Change02Edit.aspx?AppID=" & Session("AppID").ToString
                End If
                '------------------------------
                Session("objFDDBank2") = objFDDBank2
                Response.Redirect(strRedirectURL)
            End If
        End If
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(objFDDBank2, e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(objFDDBank2, DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(objFDDBank2, 0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
' ************************************************
' $History: Change02_1.aspx.vb $
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 30.04.09   Time: 10:26
' Updated in $/CKAG/Applications/AppF1/forms
' ITa 2840
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 25.03.09   Time: 10:21
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2741, 2670
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 17.03.09   Time: 11:38
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2675
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 13.03.09   Time: 14:18
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2675 unfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 13.03.09   Time: 9:18
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2675 unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 10.03.09   Time: 10:48
' Updated in $/CKAG/Applications/AppF1/forms
' ITA 2675 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 9.03.09    Time: 17:49
' Created in $/CKAG/Applications/AppF1/forms
' ITA 2675 unfertig
'
' ************************************************
