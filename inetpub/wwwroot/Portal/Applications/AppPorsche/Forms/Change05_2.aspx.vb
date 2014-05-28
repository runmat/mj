Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Imports System
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization


Public Class Change05_2
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

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private objSuche As Search
    Private objPorscheBank As Porsche_05
    Private objPorsche As Porsche_051

    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ucHeader As Header
    Protected WithEvents Literal1 As System.Web.UI.WebControls.Literal
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lnkVertragssuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents lnkExcel As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblDownloadTip As System.Web.UI.WebControls.Label
    Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ucStyles As Styles
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Kopfdaten1 As Kopfdaten
    Protected WithEvents trInanspruchnahme As HtmlTableRow
    Protected WithEvents lnkInanspruchnahme As HyperLink
    Protected WithEvents InanspruchnahmeText As Label

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmdSave.Enabled = False
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        Try
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 2

                lnkVertragssuche.NavigateUrl = "Change05.aspx?AppID=" & Session("AppID").ToString

                objPorsche = CType(Session("objFDDBank2"), Porsche_051)     'Auftragsdaten holen

                objSuche = New Search(m_App, m_User, Session.SessionID, Session("AppID").ToString)
                objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, objPorsche.Filiale)

                If objSuche.ErrorMessage = String.Empty Then
                    Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
                    Dim strTemp As String = objSuche.NAME
                    If objSuche.NAME_2.Length > 0 Then
                        strTemp &= "<br>" & objSuche.NAME_2
                    End If
                    Kopfdaten1.HaendlerName = strTemp
                    Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
                    Session("objSuche") = objSuche
                Else
                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten."
                    Exit Sub
                End If

                objPorscheBank = New Porsche_05(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", "60" & objPorsche.Filiale, Page:=Me.Page)

                If objPorscheBank.Status = 0 Then
                    Kopfdaten1.Kontingente = objPorscheBank.Kontingente
                Else
                    lblError.Text = "Fehler bei der Ermittlung der Kontingentdaten."
                    Exit Sub
                End If
                FillGrid(0)

                'inanspruchnahmeverlinkung
                Dim dvAppLinks As DataView = m_User.Applications.DefaultView
                dvAppLinks.RowFilter = "APPURL='../Components/ComCommon/Finance/Change54.aspx'"
                If Not dvAppLinks.Count = 1 Then
                    lblError.Text = "Sie haben keine Berechtigungen für den die 'Inanspruchnahme'"
                    lblError.Visible = True
                    trInanspruchnahme.Visible = False
                Else
                    Dim strParameter As String = ""
                    HelpProcedures.getAppParameters(dvAppLinks.Item(0).Item("AppID"), strParameter, ConfigurationManager.AppSettings("Connectionstring"))
                    lnkInanspruchnahme.NavigateUrl = "../../../Components/ComCommon/Finance/Change54.aspx?AppID=" & dvAppLinks.Item(0).Item("AppID") & strParameter & "&Haendlernummer=" & Right("60" & objSuche.REFERENZ, 7)
                End If
            Else
                objSuche = CType(Session("objSuche"), Search)               'Händlerdaten
                objPorscheBank = CType(Session("objFDDBank"), Porsche_05)  'Kreditlimitdaten
                objPorsche = CType(Session("objFDDBank2"), Porsche_051)             'Auftragsdaten
            End If

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        objPorsche = CType(Session("objFDDBank2"), Porsche_051)


        If objPorsche.Auftraege.Rows.Count = 0 Then
            DataGrid1.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "Keine Daten zur Anzeige gefunden."
            ShowScript.Visible = False
        Else
            DataGrid1.Visible = True
            lblNoData.Visible = False
            ddlPageSize.Visible = True

            Dim tmpDataView As New DataView()
            tmpDataView = objPorsche.Auftraege.DefaultView

            tmpDataView.RowFilter = "KUNNR_ZF='" & Right("000000000060" & objPorsche.Filiale, 10) & "'"    'Nur den selektierten Händler anzeigen..

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

            lblNoData.Text = "Es wurden " & tmpDataView.Count.ToString & " gesperrte Aufträge gefunden."
            lblNoData.Visible = True

            If DataGrid1.PageCount > 1 Then
                DataGrid1.PagerStyle.CssClass = "PagerStyle"
                DataGrid1.DataBind()
                DataGrid1.PagerStyle.Visible = True
            Else
                DataGrid1.PagerStyle.Visible = False
            End If


            Dim item As DataGridItem

            Dim button As LinkButton
            Dim button2 As LinkButton
         
            Dim row As DataRow

            For Each item In DataGrid1.Items
                button = item.FindControl("Linkbutton3")
                button2 = item.FindControl("Linkbutton1")
                row = objPorsche.Auftraege.Select("EQUNR = '" & item.Cells(0).Text & "'")(0)
                If row("Status") = "Vorgang OK." Then
                    button.Enabled = False
                    button2.Enabled = False
                Else
                    button.Enabled = True
                    button2.Enabled = True
                End If

           
            Next
           
        End If
    End Sub

  
    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim row As DataRow
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        If (e.CommandName = "Freigabe") Or (e.CommandName = "Storno") Then
            objPorsche = CType(Session("objFDDBank2"), Porsche_051)

            objPorsche.EquipmentNummer = e.Item.Cells(0).Text       'Equipmentnr
            objPorsche.AuftragsNummer = e.Item.Cells(1).Text        'Auftragsnr


            Dim cell As TableCell
            Dim label As Label
            Dim objcontrol As Control

            'FALSCH, so wird die kontingentart des letzten Vorgangs genommen!! JJU20090702
            '-------------------------------------------------
            'For Each item In DataGrid1.Items
            '    Dim blnDetailsExist As Boolean = False
            '    cell = item.Cells(11) 'Art 

            '    For Each objcontrol In cell.Controls
            '        If TypeOf objcontrol Is Label Then
            '            label = CType(objcontrol, Label)
            '            If label.ID = "Label5" Then
            '                objPorsche.Kunde = Right("0000" & label.Text, 4)
            '            End If
            '        End If
            '    Next
            'Next
            '-------------------------------------------------
            cell = e.Item.Cells(11) 'Art 
            For Each objcontrol In cell.Controls
                If TypeOf objcontrol Is Label Then
                    label = CType(objcontrol, Label)
                    If label.ID = "Label5" Then
                        objPorsche.Kunde = Right("0000" & label.Text, 4)
                    End If
                End If
            Next



                objPorsche.Storno = ""

                If e.CommandName = "Storno" Then
                    objPorsche.Storno = "X"
                End If
            objPorsche.Change(Me)

                row = objPorsche.Auftraege.Select("EQUNR = '" & objPorsche.EquipmentNummer & "'")(0)

                If objPorsche.Status <> 0 Then
                    'Fehler
                    row("Status") = objPorsche.Message
                    logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, e.CommandName, "Freigabe gesperrter Anforderungen (Benutzer: " & m_User.UserName & ", Aktion:" & e.CommandArgument & ", Auftragsnr.:" & objPorsche.AuftragsNummer & ") nicht erfolgreich (Fehler:" & objPorsche.Status & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser)
                Else
                    'OK
                    '§§§ JVE 22.08.2006: Erfolgs-Meldung nach Aktion unterscheiden...
                    'row("Status") = "Vorgang OK."
                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, e.CommandName, "Freigabe gesperrter Anforderungen (Benutzer: " & m_User.UserName & ", Aktion:" & e.CommandName & ", Auftragsnr.:" & objPorsche.AuftragsNummer & ") erfolgreich!", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser)
                    If e.CommandName = "Freigabe" Then
                        row("Status") = "Freigabe OK."
                    Else
                        row("Status") = "Storno OK."
                    End If
                End If
                objPorsche.Auftraege.AcceptChanges()
                Session("objFDDBank2") = objPorsche

                FillGrid(0)
        End If

    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid1.SelectedIndexChanged
        DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)
        FillGrid(0)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

   
End Class

' ************************************************
' $History: Change05_2.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 23.03.10   Time: 16:47
' Updated in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 11.03.10   Time: 14:42
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA: 2918
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 10.03.10   Time: 15:53
' Updated in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 3.07.09    Time: 15:37
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA 2918 Z_M_BRIEFANFORDERUNG_PORSCHE, Z_M_BRIEF_OHNE_DATEN_PORSCHE,
' Z_M_CREDITLIMIT_CHANGE_PORSCHE, Z_M_CUST_GET_CHILDREN_PORSCHE,
' Z_M_DATEN_OHNE_BRIEF_PORSCHE, Z_M_FREIGEBEN_AUFTRAG_PORSCHE,
' Z_M_GESPERRT_AUFTRAG_PORSCHE, Z_M_HAENDLERBESTAND_PORSCHE
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 2.07.09    Time: 13:15
' Updated in $/CKAG/Applications/AppPorsche/Forms
' Bugfix zur richtigen übergabe der Kontingentart
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.05.09    Time: 9:21
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA: 2837
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 18.11.08   Time: 13:54
' Updated in $/CKAG/Applications/AppPorsche/Forms
' ITA 2263 testfertig
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:28
' Created in $/CKAG/Applications/AppPorsche/Forms
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 10.03.08   Time: 10:15
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' *****************  Version 12  *****************
' User: Uha          Date: 2.07.07    Time: 13:27
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 11  *****************
' User: Uha          Date: 5.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' *****************  Version 10  *****************
' User: Uha          Date: 5.03.07    Time: 12:50
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Forms
' 
' ************************************************
