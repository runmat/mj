Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Public Class Ueberfg_Off01
    Inherits System.Web.UI.Page
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblNoData As System.Web.UI.WebControls.Label
    Protected WithEvents ShowScript As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents ucStyles As Styles
    Protected WithEvents ddlPageSize As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ucHeader As Header

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
    Private clsUeberf As Ueberf_01


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            FillGrid(0)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        clsUeberf = New Ueberf_01(m_User, m_App, "")
        
        Dim tmpDataView As DataView = clsUeberf.GetUeberfuehrungsbuffer(m_User.KUNNR).DefaultView

        If tmpDataView.Count = 0 Then
            DataGrid1.Visible = False
            lblError.Text = "Keine Daten zur Anzeige gefunden."
            lblError.Visible = True
            ShowScript.Visible = False
        Else
            DataGrid1.DataSource = tmpDataView
            DataGrid1.DataBind()

            lblNoData.Visible = True
        End If
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand

        If e.CommandName = "ueberfuehren" Then

            With clsUeberf
                .Equipment = e.Item.Cells(0).Text
                .Auftragsnummer = e.Item.Cells(1).Text
                .Ref = e.Item.Cells(2).Text
                .Vin = e.Item.Cells(3).Text
                .Beauftragung = UeberfgStandard_01.Beauftragungsart.OffeneUeberfuehrung
                '.Herst = e.Item.Cells(4).Text & " " & e.Item.Cells(5).Text
            End With

            Session("Ueberf") = clsUeberf

            Response.Redirect("Ueberfg_01.aspx?AppID=" & Session("AppID").ToString)
            'Response.Write("<script>window.open('../../../Shared/Ueberfg_01.aspx?AppID=" & Session("AppID").ToString & "')</script>")
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Ueberfg_Off01.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 2  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 7.03.07    Time: 16:56
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Forms
' Überführungs-ASPX-Seiten aus Shared-Bereich übernommen
' 
' ******************************************
