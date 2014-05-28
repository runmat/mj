Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Public Class Change09_4
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
    'Private objSuche As Search
    'Private objAddressList As Search
    Private objHaendler As Straub_09c

    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lnkFahrzeugsuche As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lnkFahrzeugAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents cmdSave As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lnkAdressAuswahl As System.Web.UI.WebControls.HyperLink
    Protected WithEvents lblVersandart As System.Web.UI.WebControls.Label
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblMaterialNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents txtOrt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPLZ As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNummer As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtStrasse As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtName2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtName1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents ucStyles As Styles

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        lnkFahrzeugAuswahl.NavigateUrl = "Change09_2.aspx?AppID=" & Session("AppID").ToString
        lnkFahrzeugsuche.NavigateUrl = "Change09.aspx?AppID=" & Session("AppID").ToString
        lnkAdressAuswahl.NavigateUrl = "Change09_3.aspx?AppID=" & Session("AppID").ToString
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            m_App = New Base.Kernel.Security.App(m_User)

            If Session("objHaendler") Is Nothing Then
                Response.Redirect("Change09.aspx?AppID=" & Session("AppID").ToString)
            End If

            objHaendler = CType(Session("objHaendler"), Straub_09c)

            If Not IsPostBack Then
                InitialLoad()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub InitialLoad()
        'Kopfdaten1.Kontingente = objHaendler.Kontingente

        lnkAdressAuswahl.Visible = True
        lnkFahrzeugAuswahl.Visible = True

        Dim strNullen As String = "00000000000000"

        Select Case objHaendler.MaterialNummer
            Case "1391"
                lblMaterialNummer.Text = strNullen & "1391"
                lblVersandart.Text = "Standard"
            Case "1385"
                lblMaterialNummer.Text = strNullen & "1385"
                lblVersandart.Text = "Express (vor 9:00 Uhr) (28,20 Euro Netto)"
            Case "1389"
                lblMaterialNummer.Text = strNullen & "1389"
                lblVersandart.Text = "vor 10:00 Uhr (23,00 Euro Netto)"
            Case "1390"
                lblMaterialNummer.Text = strNullen & "1390"
                lblVersandart.Text = "vor 12:00 Uhr (17,80 Euro Netto)"
        End Select

        'Versandadresse füllen
        txtName1.Text = objHaendler.PName
        txtName2.Text = objHaendler.PName1
        txtStrasse.Text = objHaendler.PStrasse
        txtNummer.Text = objHaendler.PHausnummer
        txtPLZ.Text = objHaendler.PPlz
        txtOrt.Text = objHaendler.POrt

        FillGrid()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        cmdSave.Enabled = False
        lnkFahrzeugsuche.Visible = False
        lnkFahrzeugAuswahl.Visible = False
        lnkAdressAuswahl.Visible = False
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
        logApp.InitEntry(m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0)

        lblMessage.Text = String.Empty
        objHaendler.StandardLogID = logApp.LogStandardIdentity
        Try
            Dim tmpDataView As New DataView()
            tmpDataView = objHaendler.Fahrzeuge.DefaultView
            tmpDataView.RowFilter = "MANDT <> '0'"

            Dim intItemCounter As Int32
            Dim blnPerformedWithoutError As Boolean = True
            For intItemCounter = 0 To tmpDataView.Count - 1

                objHaendler.ZZFAHRG = tmpDataView.Item(intItemCounter)("EQUNR").ToString
                'objHaendler.KUNNR = m_User.KUNNR
                objHaendler.Anfordern()

                'If objHaendler.Status = -1111 Then
                '    blnPerformedWithoutError = False
                '    lblError.Text = objHaendler.Message
                '    Exit For
                'End If

                'Dim sngStart As Single = CSng(Timer)
                'Dim intStart As Int32 = 0
                'Do While CSng(Timer) < sngStart + 1
                '    intStart += 1
                'Loop

                If objHaendler.Auftragsnummer.Length = 0 Then
                    tmpDataView.Item(intItemCounter)("COMMENT") = "Fehler: " & objHaendler.Message
                Else
                    tmpDataView.Item(intItemCounter)("COMMENT") = objHaendler.Message
                End If
            Next
            FillGrid()
        Catch ex As Exception
            lblError.Text = "Fehler: " & ex.Message
            logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler bei der Briefanforderung zu Adresse: " & objHaendler.PName & "," & objHaendler.PStrasse & "," & objHaendler.PHausnummer & "," & objHaendler.PPlz & "," & objHaendler.POrt & ", Fehler: " & ex.Message & ")")
        End Try
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = objHaendler.Fahrzeuge.DefaultView

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

        tmpDataView.RowFilter = "MANDT <> '0'"
        DataGrid1.DataSource = tmpDataView
        DataGrid1.DataBind()

        Dim item As DataGridItem
        Dim cell As TableCell
        Dim chkBox As CheckBox
        Dim control As Control
        Dim blnScriptFound As Boolean = False
        Dim intZaehl0001 As Int32 = 0
        Dim intZaehl0002 As Int32 = 0
        'Dim intZaehl0003 As Int32 = 0
        'Dim intZaehl0004 As Int32 = 0
        Dim blnBezahlt As Boolean
        Dim strKKB As String
        Dim blnGesperrteAnforderungen As Boolean = False

        For Each item In DataGrid1.Items
            blnBezahlt = False
            strKKB = item.Cells(2).Text     'Temp. oder Endg.
            For Each cell In item.Cells
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chkBox = CType(control, CheckBox)
                        If chkBox.ID = "Bezahlt" And chkBox.Checked Then
                            blnBezahlt = True
                        End If
                    End If
                Next
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chkBox = CType(control, CheckBox)
                        If Not chkBox.ID = "Bezahlt" Then
                            Select Case chkBox.ID
                                Case "chk0001"
                                    If strKKB = "1" Then
                                        chkBox.Checked = True
                                        If Not blnBezahlt Then
                                            intZaehl0001 += 1
                                        End If
                                    Else
                                        chkBox.Checked = False
                                    End If
                                Case "chk0002"
                                    If strKKB = "2" Then
                                        chkBox.Checked = True
                                        If Not blnBezahlt Then
                                            intZaehl0002 += 1
                                        End If
                                    Else
                                        chkBox.Checked = False
                                    End If
                            End Select
                        End If
                    End If
                Next
            Next
        Next

        lblMessage.Text = ""
        lblMessage.Text = lblMessage.Text & "<br>Zur Aktivierung der ausgewählten Vorgänge klicken Sie auf [Absenden].<br>"
        DataGrid1.PagerStyle.Visible = False
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs)
        FillGrid(e.SortExpression)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change09_4.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:38
' Created in $/CKAG/Applications/AppSTRAUB/Forms
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 14:28
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 14:18
' Updated in $/CKG/Applications/AppSTRAUB/AppSTRAUBWeb/Forms
' 
' ************************************************
