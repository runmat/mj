Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change03_4
    Inherits System.Web.UI.Page
#Region "Declarations"
    Private m_User As Security.User
    Private m_App As Security.App
    Private m_change As Versand
    Private versandart As String
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        versandart = Request.QueryString.Item("art")

        lnkFahrzeugAuswahl.NavigateUrl = "Change03_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        lnkFahrzeugsuche.NavigateUrl = "Change03.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart
        lnkAdressAuswahl.NavigateUrl = "Change03_3.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If m_User.Organization.OrganizationAdmin Then
            lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
        End If

        m_App = New Security.App(m_User)

        If Session("AppChange") Is Nothing Then
            Response.Redirect("Change03.aspx?AppID=" & Session("AppID").ToString & "art=" & versandart)
        End If
        m_change = CType(Session("AppChange"), Versand)

        If Not IsPostBack Then
            InitialLoad()
        End If

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        cmdSearch.Enabled = False
        lnkAdressAuswahl.Enabled = False
        lnkFahrzeugAuswahl.Enabled = False
        DoSubmit()
    End Sub

    Private Sub GridView1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        FillGrid(e.SortExpression)
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lnkCreateExcel1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel1.Click
        Dim reportExcel As DataTable = Session("App_ResultTable")
        Dim col2 As DataColumn
        Dim bVisibility As Integer
        Dim i As Integer
        Dim sColName As String = ""
        Dim tblTranslations As New DataTable()
        Dim tblTemp As New DataTable()
        Dim AppURL As String

        Try

            AppURL = Replace(Me.Request.Url.LocalPath, "/Services", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)
            tblTemp = reportExcel.Copy

            For Each col In GridView1.Columns
                For i = tblTemp.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = tblTemp.Columns(i)
                    If col2.ColumnName.ToUpper = col.SortExpression.ToUpper OrElse col2.ColumnName.ToUpper = col.HeaderText.ToUpper.Replace("COL_", "") Then
                        sColName = TranslateColLbtn(GridView1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            tblTemp.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                    'EQUNR nicht mit in Excel ausgeben, wird meist nur als boundcolumn versteckt als schlüssel verwendet JJU2008.10.23
                    If col2.ColumnName.ToUpper = "EQUNR" Then
                        tblTemp.Columns.Remove(col2)
                    End If
                Next
                tblTemp.AcceptChanges()
            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)

        Catch ex As Exception
            lblerror.Text = ex.Message
        End Try
    End Sub

#End Region



#Region "Methods"

    Private Sub InitialLoad()
        Dim str As String = String.Empty

        tr_Versandart.Visible = True
        lblXVersandartData.Text = m_change.MaterialBezeichnung
        lblXVersand.Text = m_change.VersandAdresseText  'Versandadresse
        If versandart = "TEMP" Then
            lbl_Versandgrund.Visible = True
            lblXVersandGrundData.Visible = True
            lblXVersandGrundData.Text = m_change.VersandGrundText
        Else
            tr_Versandgrund.Visible = False
        End If

        FillGrid()

    End Sub


    Private Sub FillGrid(Optional ByVal strSort As String = "")
        Dim tmpDataView As New DataView()
        tmpDataView = m_change.Fahrzeuge.DefaultView

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
        If (versandart = "TEMP") Then
            tmpDataView.RowFilter = "ABCKZ = '1'"
        Else
            tmpDataView.RowFilter = "ABCKZ = '2'"
        End If

        GridView1.DataSource = tmpDataView
        GridView1.DataBind()

    End Sub

    Private Sub DoSubmit()

        Try
            If (m_change.VersandAdresseText.Length = 0) Then
                Response.Redirect(lnkFahrzeugAuswahl.NavigateUrl)
            Else
                Dim tmpDataView As New DataView()
                tmpDataView = m_change.Fahrzeuge.DefaultView
                If (versandart = "TEMP") Then
                    tmpDataView.RowFilter = "ABCKZ = '1'"
                Else
                    tmpDataView.RowFilter = "ABCKZ = '2'"
                End If

                Dim intItemCounter As Int32
                Dim blnPerformedWithoutError As Boolean = True

                'angefordert = 0

                For intItemCounter = 0 To tmpDataView.Count - 1
                    'Daten sammeln
                    With m_change
                        .KUNNR = Right("0000000000" & m_User.KUNNR, 10)
                        .Haendlernummer = ""

                        .HalterNummer = ""                                              'Addressnr. Halter (wird nicht benötigt...)
                        .ScheinSchildernummer = ""                                      'Addressnr. Schein und Schilder (wird nicht benötigt...)
                        .VersandAdresse_ZS = m_change.VersandAdresse_ZS
                        If (versandart = "TEMP") Then
                            .Versandart = "1"                                               'Temporär!
                        Else
                            .Versandart = "2"                                               'Endgültig!
                        End If
                        .Equimpent = tmpDataView.Item(intItemCounter)("EQUNR").ToString
                        .SucheFahrgestellNr = tmpDataView.Item(intItemCounter)("CHASSIS_NUM").ToString
                        .Kennzeichen = tmpDataView.Item(intItemCounter)("LICENSE_NUM").ToString
                        .TIDNr = tmpDataView.Item(intItemCounter)("TIDNR").ToString
                        .LizenzNr = tmpDataView.Item(intItemCounter)("LIZNR").ToString
                        .Materialnummer = m_change.Materialnummer
                        .Listenart = "K1"
                    End With

                    m_change.Anfordern(Session("AppID").ToString, Session.SessionID, Me)

                    tmpDataView.Item(intItemCounter)("STATUS") = m_change.Auftragsstatus

                    tmpDataView.Table.AcceptChanges()
                    If m_change.Status = 0 Then
                        Dim ExcelResult As New DataTable
                        ExcelResult.Columns.Add("Kennzeichen", GetType(System.String))
                        ExcelResult.Columns.Add("Fahrgestellnummer", GetType(System.String))
                        ExcelResult.Columns.Add("Leasingvertragsnummer", GetType(System.String))
                        ExcelResult.Columns.Add("NummerZB2", GetType(System.String))
                        ExcelResult.Columns.Add("Ordernummer", GetType(System.String))
                        ExcelResult.Columns.Add("Status", GetType(System.String))
                        Dim dRow As DataRow
                        dRow = ExcelResult.NewRow
                        With m_change
                            dRow("Kennzeichen") = .Kennzeichen
                            dRow("Fahrgestellnummer") = .SucheFahrgestellNr
                            dRow("Leasingvertragsnummer") = .LizenzNr
                            dRow("NummerZB2") = .TIDNr
                            dRow("Ordernummer") = tmpDataView.Item(intItemCounter)("ZZREFERENZ1").ToString
                            dRow("Status") = .Status
                        End With
                        ExcelResult.Rows.Add(dRow)
                        ExcelResult.AcceptChanges()
                        Session("App_ResultTable") = ExcelResult
                    End If
                Next

                GridView1.DataSource = tmpDataView
                GridView1.DataBind()


            End If
            If m_change.Status <> 0 Then
                lblerror.Text = m_change.Message
            Else
                ExcelDiv.Visible = True
            End If

            Session("AppChange") = m_change

        Catch ex As Exception
            lblerror.Text = "Fehler: " & ex.Message
            Throw ex
        End Try
    End Sub

#End Region
End Class