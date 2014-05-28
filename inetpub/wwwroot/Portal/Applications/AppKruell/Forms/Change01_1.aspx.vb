Option Explicit On 
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business


Public Class Change01_1
    Inherits System.Web.UI.Page
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Protected WithEvents lb_neuerAuftrag As System.Web.UI.WebControls.LinkButton
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lb_Auswahl As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lb_Senden As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lbl_SAPResultat As System.Web.UI.WebControls.Label
    Protected WithEvents Datagrid2 As System.Web.UI.WebControls.DataGrid
    Dim m_change As kruell_01

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
            If m_change Is Nothing Then
                m_change = CType(Session("objChange"), kruell_01)
            End If
            If Not m_change.DatenTabelle Is Nothing Then
                If m_change.dataChanged = True Then
                    lb_Auswahl.Attributes.Add("onclick", "return confirm('Ihre Änderungen sind noch nicht gesendet, möchten Sie trotzdem fortfahren?');")
                Else
                    lb_Auswahl.Attributes.Clear()
                End If
                FillGrid()
            End If



        Catch ex As Exception
            lblError.Text = "Fehler beim laden der Seite: " & ex.Message.ToString
        End Try


    End Sub




    Sub DataGrid1_ItemCreated(ByVal Sender As Object, ByVal e As DataGridItemEventArgs) Handles DataGrid1.ItemCreated
        Try

        
        Select Case e.Item.ItemType
            Case ListItemType.Item
                    Dim myDeleteButton As LinkButton

                    myDeleteButton = CType(e.Item.FindControl("lb_delete"), LinkButton)
                    myDeleteButton.Attributes.Add("onclick", "return confirm('Sind Sie sicher dass Sie den Auftrag löschen möchten?');")

                    Dim myAbgearbeitetButton As LinkButton
                    myAbgearbeitetButton = CType(e.Item.FindControl("lb_Abgearbeitet"), LinkButton)
                    myAbgearbeitetButton.Attributes.Add("onclick", "return confirm('Sind Sie sicher dass der Auftrag ""abgearbeitet"" ist?');")

            Case ListItemType.AlternatingItem
                Dim myDeleteButton As LinkButton
                myDeleteButton = CType(e.Item.FindControl("lb_delete"), LinkButton)
                    myDeleteButton.Attributes.Add("onclick", "return confirm('Sind Sie Sicher dass Sie den Auftrag löschen möchten?');")

                    Dim myAbgearbeitetButton As LinkButton
                    myAbgearbeitetButton = CType(e.Item.FindControl("lb_Abgearbeitet"), LinkButton)
                    myAbgearbeitetButton.Attributes.Add("onclick", "return confirm('Sind Sie sicher dass der Auftrag ""abgearbeitet"" ist?');")

            End Select
        Catch
            lblError.Text = "Vorsicht: Bestätigungsabfragen konnten nicht generiert werden"
        End Try
    End Sub


    Private Sub FillGrid(Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = m_change.DatenTabelle.DefaultView

        If Not strSort.Trim(" "c).Length = 0 Then
            Dim strDirection As String
            If ViewState("Direction") Is Nothing Then
                strDirection = "desc"
            Else
                strDirection = ViewState("Direction").ToString
            End If

            If strDirection = "asc" Then
                strDirection = "desc"
            Else
                strDirection = "asc"
            End If

            tmpDataView.Sort = strSort & " " & strDirection
            ViewState("Direction") = strDirection
        End If


        tmpDataView.RowFilter = ""
        DataGrid1.DataSource = tmpDataView
        DataGrid1.DataBind()

        DataGrid1.PagerStyle.Visible = False
    End Sub


    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub
  

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        System.Diagnostics.Debug.Write(e.CommandName)

        If m_change Is Nothing Then
            m_change = CType(Session("objChange"), kruell_01)
        End If

        Select Case e.CommandName
            Case "Delete"

                'm_change.DatenTabelle.Rows.Find(e.Item.Cells(0).Text).Delete()
                'm_change.DatenTabelle.AcceptChanges()
                'RowState setzten, da übersicht besser erhalten wird und SAP Datensätze nicht einfach "entfernt" werden können, Problem ist wenn Row gelöscht wird, wird sie im Grid nicht mehr angezeigt JJ2007.11.19
                m_change.DatenTabelle.Rows.Find(e.Item.Cells(0).Text).Item("Status") = "gelöscht"
                FillGrid()

            Case "Select"
                System.Diagnostics.Debug.WriteLine("Dataset Index eines Items= " & e.Item.DataSetIndex())
                System.Diagnostics.Debug.WriteLine("Item Index eines Items= " & e.Item.ItemIndex)

                Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString & "&SourceRow=" & e.Item.Cells(0).Text, False)

            Case "Freigabe"
                m_change.DatenTabelle.Rows.Find(e.Item.Cells(0).Text).Item("WEB_USER_FREIG") = m_User.UserName
                m_change.DatenTabelle.Rows.Find(e.Item.Cells(0).Text).Item("DAT_FREIG") = Now.ToShortDateString
                FillGrid()

            Case "abgearbeitet"
                m_change.DatenTabelle.Rows.Find(e.Item.Cells(0).Text).Item("Status") = "abgearbeitet"
                m_change.DatenTabelle.Rows.Find(e.Item.Cells(0).Text).Item("ABGEARB") = "X"
                FillGrid()
            Case "Drucken"
                'es soll möglich sein auch schon bei der anzeige eine druckausgabe zu erzeugen, ich verwende die jungfräuliche SAP Tabelle dafür
                'die alte druckmethode wird dann mit der passenden Row aus der jungfräulichen sap tabelle gefüttert. JJU2008.06.12

                If m_change.Vorschlagswerte Is Nothing Then
                    'nun da nur noch die values ins sap übertragen werden und die anzeige trotzdem dem text 
                    'enthalten soll muss diese tabelle jetzt gefüllt werden um aus dem Value den Text zu ermitteln
                    m_change.fillVorschlagswerte(CStr(Session("AppID")), Me.Session.SessionID)
                End If
               

                Dim strHtmlCodeAndLink() As String = m_change.createAenderungsFormular(m_change.SAPgetSAPRowForEarlyPrinting(e.CommandArgument.ToString), "", True)

                Dim objStreamWriter As System.IO.StreamWriter


                objStreamWriter = New System.IO.StreamWriter("C:\Inetpub\wwwroot" & strHtmlCodeAndLink(1), False)
                objStreamWriter.Write(strHtmlCodeAndLink(0))
                objStreamWriter.Close()


                Dim openScript As String
                'HTML ausgabe in neuem Fenster öffnen, da ein Linkbutton und kein Hyperlink muss javascript genutzt werden JJ2007.11.30
                openScript = _
                   "<" & "script language=""JavaScript"">" & _
                      "var win = window.open('" & strHtmlCodeAndLink(1) & "','unerheblich'," & _
                         "'width=670,height=700,left=20,top=20,resizable=YES, scrollbars=YES,menubar=NO');" & _
                   "</" & "script>"
                Response.Write(openScript)


        End Select
    End Sub


    Private Sub DataGrid2_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles Datagrid2.ItemCommand
        System.Diagnostics.Debug.Write(e.CommandName)
        If m_change Is Nothing Then
            m_change = CType(Session("objChange"), kruell_01)
        End If

        Select Case e.CommandName
            Case "Drucken"


                Dim objStreamWriter As System.IO.StreamWriter


                objStreamWriter = New System.IO.StreamWriter("C:\Inetpub\wwwroot" & CStr(m_change.DatenTabelle.Rows.Find(e.Item.Cells(0).Text).Item("LinkURL")), False)
                objStreamWriter.Write(CStr(m_change.DatenTabelle.Rows.Find(e.Item.Cells(0).Text).Item("PrintString")))
                objStreamWriter.Close()


                Dim openScript As String
                'HTML ausgabe in neuem Fenster öffnen, da ein Linkbutton und kein Hyperlink muss javascript genutzt werden JJ2007.11.30
                openScript = _
                   "<" & "script language=""JavaScript"">" & _
                      "var win = window.open('" & CStr(m_change.DatenTabelle.Rows.Find(e.Item.Cells(0).Text).Item("LinkURL")) & "','unerheblich'," & _
                         "'width=670,height=700,left=20,top=20,resizable=YES, scrollbars=YES,menubar=NO');" & _
                   "</" & "script>"
                Response.Write(openScript)
        End Select

    End Sub



    Private Sub lb_neuerAuftrag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_neuerAuftrag.Click

        Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString & "&SourceRow=-1")
    End Sub

    Private Sub lb_Auswahl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Auswahl.Click
        Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub lb_Senden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Senden.Click
        If m_change Is Nothing Then
            m_change = CType(Session("objChange"), kruell_01)
        End If

        If m_change.Vorschlagswerte Is Nothing Then
            'nun da nur noch die values ins sap übertragen werden und die anzeige trotzdem dem text 
            'enthalten soll muss diese tabelle jetzt gefüllt werden um aus dem Value den Text zu ermitteln
            m_change.fillVorschlagswerte(CStr(Session("AppID")), Me.Session.SessionID)
        End If

        m_change.SendDataToSAP(m_change.AppID, m_change.SessionID)

        'Dim strHeaderString As String
        'strHeaderString = "<p align=""center""><b><u><font size=""15""> SAP Rückmeldung:</font><br><br><br></u> </b></p>"


        lbl_SAPResultat.Text = m_change.SAPResultat
        lblError.Text = m_change.Message







        'FillGrid()
        'Bearbeitungsgrid ausblenden
        DataGrid1.DataSource = Nothing
        DataGrid1.DataBind()
        DataGrid1.Visible = False
        'confirm Frage löschen
        lb_Auswahl.Attributes.Clear()
        lb_neuerAuftrag.Enabled = False
        lb_Senden.Enabled = False

        'PrintGridFüllen
        Datagrid2.DataSource = m_change.DatenTabelle
        Datagrid2.DataBind()

    End Sub

End Class
' ************************************************
' $History: Change01_1.aspx.vb $
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 30.04.09   Time: 17:24
' Updated in $/CKAG/Applications/AppKruell/Forms
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 18.07.08   Time: 11:50
' Updated in $/CKAG/Applications/AppKruell/Forms
' ITA 2026 verbesserungen
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 16.07.08   Time: 18:15
' Updated in $/CKAG/Applications/AppKruell/Forms
' ITA 2026 Bugfixes
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 7.07.08    Time: 8:24
' Updated in $/CKAG/Applications/AppKruell/Forms
' ITA 2026
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 12.06.08   Time: 14:47
' Updated in $/CKAG/Applications/AppKruell/Forms
' ITA 1999
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 21.05.08   Time: 15:07
' Updated in $/CKAG/Applications/AppKruell/Forms
' ITA 1923
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
' *****************  Version 34  *****************
' User: Jungj        Date: 9.01.08    Time: 13:57
' Updated in $/CKG/Applications/AppKruell/AppKruellWeb/Forms
' ITA 1580 Report01 hinzugefügt, SS History Bodys hinzugefügt
' ************************************************

