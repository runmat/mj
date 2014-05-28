Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change06
    Inherits System.Web.UI.Page

#Region "Declarations"
    
    Private m_App As Security.App
    Private m_User As Security.User

    Private objReport05_objFDDBank2 As F1_OffeneAnforderungen
#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        ucStyles.TitleText = lblHead.Text

        m_App = New Security.App(m_User)


        Try
            m_App = New Security.App(m_User)

            If Not IsPostBack Then
                ddlPageSize.Items.Add("10")
                ddlPageSize.Items.Add("20")
                ddlPageSize.Items.Add("50")
                ddlPageSize.Items.Add("100")
                ddlPageSize.Items.Add("200")
                ddlPageSize.Items.Add("500")
                ddlPageSize.Items.Add("1000")
                ddlPageSize.SelectedIndex = 1


                SucheHaendler1.Visible = True
                SucheHaendler1.initialize()

                cmdSearch.Visible = True

            End If

            tdExcel.Visible = False

        Catch ex As Exception
            lblError.Text = "Bei der Ermittlung der Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim ctrl As Control
        Dim Label1 As Label
        Dim hLink As HyperLink
        Dim sAuftrag As String = ""
        Dim sEqui As String = ""
        Dim sFIN As String = ""
        Dim sHaendlerReferenz As String = String.Empty
        If e.CommandName = "Storno" Then

            If objReport05_objFDDBank2 Is Nothing Then
                objReport05_objFDDBank2 = CType(Session("objFDDBank2"), F1_OffeneAnforderungen)
            End If

            For Each ctrl In e.Item.Cells(0).Controls
                Try
                    Label1 = DirectCast(ctrl, Label)
                    If Label1.ID = "lblHaendlerEx" Then
                        sHaendlerReferenz = Label1.Text
                        Exit For
                    End If
                Catch
                End Try
            Next

            For Each ctrl In e.Item.Cells(1).Controls
                Try
                    Label1 = DirectCast(ctrl, Label)
                    If Label1.ID = "lblAuftragsnummer" Then
                        sAuftrag = Label1.Text
                        Exit For
                    End If
                Catch
                End Try
            Next

            For Each ctrl In e.Item.Cells(11).Controls
                Try
                    Label1 = DirectCast(ctrl, Label)
                    If Label1.ID = "lblEquinummer" Then
                        sEqui = Label1.Text
                        Exit For
                    End If
                Catch
                End Try
            Next

            For Each ctrl In e.Item.Cells(4).Controls
                Try
                    hLink = DirectCast(ctrl, HyperLink)
                    If hLink.ID = "lnkHistorie" Then
                        sFIN = hLink.Text
                        Exit For
                    End If
                Catch
                End Try
            Next

            If Not sAuftrag.Length = 0 Then
                objReport05_objFDDBank2.AuftragsNummer = sAuftrag
                objReport05_objFDDBank2.Equinr = sEqui
                objReport05_objFDDBank2.Haendler = sHaendlerReferenz
                objReport05_objFDDBank2.Change(Session("AppID").ToString, Session.SessionID.ToString)

                Dim logApp As New Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)
                If Not objReport05_objFDDBank2.Status = 0 Then
                    lblError.Text = objReport05_objFDDBank2.Message
                    logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.Reference, "Fehler bei der Stornierung durch Bank (Vorgang: " & objReport05_objFDDBank2.AuftragsNummer.TrimStart("0"c) & ", FIN: " & sFIN & ", Fehler: " & objReport05_objFDDBank2.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
                Else
                    lblError.Text = ""
                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, m_User.Reference, "Stornierung durch Bank (Vorgang: " & objReport05_objFDDBank2.AuftragsNummer.TrimStart("0"c) & ", FIN: " & sFIN & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
                End If
                logApp.WriteStandardDataAccessSAP(objReport05_objFDDBank2.IDSAP)

                If SucheHaendler1.giveHaendlernummer Is Nothing Then
                    objReport05_objFDDBank2.Haendler = String.Empty
                Else
                    objReport05_objFDDBank2.Haendler = SucheHaendler1.giveHaendlernummer
                End If


                objReport05_objFDDBank2.Show(Session("AppID").ToString, Session.SessionID.ToString)

                Session("objFDDBank2") = objReport05_objFDDBank2

                If Not objReport05_objFDDBank2.Status = 0 Then
                    lblError.Text = objReport05_objFDDBank2.Message
                End If

                FillGrid(DataGrid1.CurrentPageIndex)

                Dim fahrg As String = ""
                Dim HyperLink1 As HyperLink

                For Each ctrl In e.Item.Cells(4).Controls
                    Try
                        HyperLink1 = DirectCast(ctrl, HyperLink)
                        If HyperLink1.ID = "lnkHistorie" Then
                            fahrg = HyperLink1.Text
                            Exit For
                        End If
                    Catch
                    End Try
                Next

                If (fahrg <> String.Empty) Then
                    DelAuthEntry(fahrg, sHaendlerReferenz)
                End If

            End If
        End If

    End Sub

    Protected Sub lnkCreateExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCreateExcel.Click
        Try
            Dim m_datatable As DataTable = CType(Session("lnkExcel"), DataTable)
            Dim col As DataGridColumn
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim i As Integer
            Dim sColName As String

            Dim AppURL As String = Replace(Request.Url.LocalPath, "/Portal", "..")
            Dim tblTranslations As DataTable = CType(Session(AppURL), DataTable)

            For Each col In DataGrid1.Columns
                For i = m_datatable.Columns.Count - 1 To 0 Step -1
                    bVisibility = 0
                    col2 = m_datatable.Columns(i)
                    If col2.ColumnName = col.SortExpression Then
                        sColName = TranslateColLbtn(DataGrid1, tblTranslations, col.HeaderText, bVisibility)
                        If bVisibility = 0 Then
                            m_datatable.Columns.Remove(col2)
                        ElseIf sColName.Length > 0 Then
                            col2.ColumnName = sColName
                        End If
                    End If
                Next
                m_datatable.AcceptChanges()
            Next

            m_datatable.Columns.RemoveAt(m_datatable.Columns.Count - 1)
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, m_datatable, Me)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub lbSelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbSelektionZurueckSetzen.Click
        objReport05_objFDDBank2 = Nothing
       
        SucheHaendler1.SelektionZuruecksetzen()

        SucheHaendler1.Visible = True
        tdHaendlersuche.Visible = True
        lbSelektionZurueckSetzen.Visible = False
        DataGrid1.Visible = False
        DataGrid1.Dispose()
        tdExcel.Visible = False
        cmdSearch.Visible = True
        lblNoData.Visible = False
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
        FillGrid(e.NewPageIndex)
    End Sub

    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(DataGrid1.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub ddlPageSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlPageSize.SelectedIndexChanged
        FillGrid(0)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region

#Region "Methods"

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        If objReport05_objFDDBank2 Is Nothing Then
            objReport05_objFDDBank2 = CType(Session("objFDDBank2"), F1_OffeneAnforderungen)
        End If

        If objReport05_objFDDBank2.Status = 0 Then

            If IsNothing(objReport05_objFDDBank2.Auftraege) OrElse objReport05_objFDDBank2.Auftraege.Rows.Count = 0 Then
                tdExcel.Visible = False
                lblNoData.Visible = True
                lblNoData.Text = "Keine Daten zur Anzeige gefunden."
                DataGrid1.Visible = False
                lblNoData.Visible = True
            Else
                lnkCreateExcel.Visible = True
                DataGrid1.Visible = True
                lblNoData.Visible = False
                tdExcel.Visible = True

                DataGrid1.PageSize = CInt(ddlPageSize.SelectedItem.Value)

                tdHaendlersuche.Visible = False
                SucheHaendler1.Visible = False

                Dim tmpDataView As DataView = objReport05_objFDDBank2.Auftraege.DefaultView
                Session("lnkExcel") = objReport05_objFDDBank2.Auftraege

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
                'Anzahl der gesperrten Aufträge ermitteln
                Dim view As New DataView(tmpDataView.Table)
                view.RowFilter = "Status <> ''"
                '----------------------------------------
                lblNoData.Text = "Anzahl offene Anforderungen: " & tmpDataView.Count.ToString & "<br>Davon in Bearbeitung Bank: " & view.Count & "<br>"
                lblNoData.Visible = True

                If DataGrid1.PageCount > 1 Then
                    DataGrid1.PagerStyle.CssClass = "PagerStyle"
                    DataGrid1.DataBind()
                    DataGrid1.PagerStyle.Visible = True
                Else
                    DataGrid1.PagerStyle.Visible = False
                End If

                Dim item As DataGridItem
                Dim cell As TableCell
                Dim button As LinkButton
                Dim hyperlink As HyperLink
                Dim Label As Label
                Dim control As Control
                Dim blnScriptFound As Boolean = False
                Dim strAuftragsnummer As String = ""
                Dim strVertragsnummer As String = ""
                Dim strAngefordertAm As String = ""
                Dim strFahrgestellnummer As String = ""
                Dim strKontingentart As String = ""
                Dim strBriefnummer As String = ""

                For Each item In DataGrid1.Items

                    cell = item.Cells(1)
                    For Each control In cell.Controls
                        Dim control1 = TryCast(control, Label)
                        If (control1 IsNot Nothing) Then
                            Label = control1
                            strAuftragsnummer = Label.Text
                        End If
                    Next

                    cell = item.Cells(2)
                    For Each control In cell.Controls
                        Dim control1 = TryCast(control, Label)
                        If (control1 IsNot Nothing) Then
                            Label = control1
                            strVertragsnummer = Label.Text.Trim
                        End If
                    Next

                    cell = item.Cells(3)
                    For Each control In cell.Controls
                        Dim control1 = TryCast(control, Label)
                        If (control1 IsNot Nothing) Then
                            Label = control1
                            strAngefordertAm = Label.Text
                        End If
                    Next

                    cell = item.Cells(4)
                    For Each control In cell.Controls
                        Dim link = TryCast(control, HyperLink)
                        If (link IsNot Nothing) Then
                            hyperlink = link
                            strFahrgestellnummer = hyperlink.Text
                        End If
                    Next

                    cell = item.Cells(5)
                    For Each control In cell.Controls
                        Dim control1 = TryCast(control, Label)
                        If (control1 IsNot Nothing) Then
                            Label = control1
                            strBriefnummer = Label.Text
                        End If
                    Next

                    cell = item.Cells(6)
                    For Each control In cell.Controls
                        Dim control1 = TryCast(control, Label)
                        If (control1 IsNot Nothing) Then
                            Label = control1
                            strKontingentart = Label.Text
                        End If
                    Next

                    cell = item.Cells(8)
                    For Each control In cell.Controls
                        Dim linkButton = TryCast(control, LinkButton)
                        If (linkButton IsNot Nothing) Then
                            button = linkButton
                            If button.CommandName = "Storno" Then
                                button.Attributes.Add("onClick", "if (!StornoConfirm('" & strAuftragsnummer & "','" & strVertragsnummer & "','" &
                                                      strAngefordertAm & "','" & strFahrgestellnummer & "','" & strBriefnummer & "','" &
                                                      strKontingentart & "')) return false;")
                                button.Attributes.Add("class", "StandardButtonTable")
                                blnScriptFound = True
                            End If
                        End If
                    Next
                Next

                If blnScriptFound Then
                    ShowScript.Visible = True
                End If

                For Each itemX As DataGridItem In DataGrid1.Items

                    If Not itemX.FindControl("lnkHistorie") Is Nothing Then
                        If Not m_User.Applications.Select("AppName = 'Report46'").Count = 0 Then

                            CType(itemX.FindControl("lnkHistorie"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" &
                                m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" &
                                CType(itemX.FindControl("lnkHistorie"), HyperLink).Text

                        End If
                    End If

                Next
            End If
        Else
            lblNoData.Text = objReport05_objFDDBank2.Message
            lblNoData.Visible = True
            cmdSearch.Visible = True
            lbSelektionZurueckSetzen.Visible = False
        End If
    End Sub

    '----------------------------------------------------------------------
    ' Methode:      DoSubmit
    ' Autor:        SFa
    ' Beschreibung: Gibt anhand der Parameter Kunden- und Händlernummer
    '               die Daten für offene Anforderungen zurück
    ' Erstellt am:  13.05.2009
    ' ITA:          2849
    '----------------------------------------------------------------------
    Private Sub DoSubmit()

        lblError.Text = ""
        lblError.Visible = False


        Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        objReport05_objFDDBank2 = New F1_OffeneAnforderungen(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
        objReport05_objFDDBank2.AppID = Session("AppID").ToString
        objReport05_objFDDBank2.Customer = m_User.Customer.KUNNR
        objReport05_objFDDBank2.CreditControlArea = "ZDAD"

        If SucheHaendler1.giveHaendlernummer Is Nothing Then
            objReport05_objFDDBank2.Haendler = String.Empty
        Else
            objReport05_objFDDBank2.Haendler = SucheHaendler1.giveHaendlernummer
        End If
        objReport05_objFDDBank2.Show(Session("AppID").ToString, Session.SessionID.ToString)

        Session("objFDDBank2") = objReport05_objFDDBank2

        cmdSearch.Visible = False
        lbSelektionZurueckSetzen.Visible = True

        FillGrid(0)

    End Sub

    Private Sub DelAuthEntry(ByVal fahrgestellnr As String, ByVal HaendlerExReference As String)

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        Try
            cn.Open()

            Dim strDeleteSQL As String = "DELETE " & _
                                          "FROM [Authorization] " & _
                                          "WHERE CustomerReference=@cr AND ProcessReference=@pr"

            Dim cmd As New SqlClient.SqlCommand()
            cmd.Connection = cn
            cmd.Parameters.AddWithValue("@cr", HaendlerExReference)
            cmd.Parameters.AddWithValue("@pr", fahrgestellnr)

            cmd.CommandText = strDeleteSQL
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "AutorisierungenLoeschen", "lbtnDelete_Click", ex.ToString)

            lblError.Text = ex.Message
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
                cn.Dispose()
            End If
        End Try
    End Sub

#End Region

End Class

' ************************************************
' $History: Change06.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 20.09.10   Time: 11:04
' Updated in $/CKAG/Applications/AppF1/forms
' ITA: 4104
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 14.05.09   Time: 16:11
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 14.05.09   Time: 11:45
' Updated in $/CKAG/Applications/AppF1/forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 14.05.09   Time: 9:25
' Created in $/CKAG/Applications/AppF1/forms
' 