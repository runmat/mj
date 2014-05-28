Option Strict On
Option Explicit On

Public Class Change05
    Inherits System.Web.UI.Page

    Private Shared _returnTable As DataTable

    Private Shared ReadOnly Property ReturnTable As DataTable
        Get
            If Change05._returnTable Is Nothing Then
                Dim table As New DataTable()
                table.Columns.Add("Fahrgestellnummer", GetType(String)).ExtendedProperties.Add("SearchString", "Fahrgestell")
                table.Columns.Add("Kennzeichen", GetType(String)).ExtendedProperties.Add("SearchString", "Kennz")
                table.Columns.Add("Carportschlüssel", GetType(String)).ExtendedProperties.Add("SearchString", "Carport")
                table.Columns.Add("Carporttext", GetType(String))
                table.Columns.Add("Logistikpartner", GetType(String)).ExtendedProperties.Add("SearchString", "Logistik")
                table.Columns.Add("Bereitstellungsdatum", GetType(DateTime)).ExtendedProperties.Add("SearchString", "Bereit")
                table.Columns.Add("Bemerkungstext", GetType(String)).ExtendedProperties.Add("SearchString", "Bemerkung")
                table.Columns.Add("Dienstleistung1", GetType(String)).ExtendedProperties.Add("SearchString", "Dienstleistung1")
                table.Columns.Add("Dienstleistung1Text", GetType(String))
                table.Columns.Add("Dienstleistung2", GetType(String)).ExtendedProperties.Add("SearchString", "Dienstleistung2")
                table.Columns.Add("Dienstleistung2Text", GetType(String))
                table.Columns.Add("Dienstleistung3", GetType(String)).ExtendedProperties.Add("SearchString", "Dienstleistung3")
                table.Columns.Add("Dienstleistung3Text", GetType(String))
                table.Columns.Add("Dienstleistung4", GetType(String)).ExtendedProperties.Add("SearchString", "Dienstleistung4")
                table.Columns.Add("Dienstleistung4Text", GetType(String))
                table.Columns.Add("Dienstleistung5", GetType(String)).ExtendedProperties.Add("SearchString", "Dienstleistung5")
                table.Columns.Add("Dienstleistung5Text", GetType(String))
                table.Columns.Add("Status", GetType(Status))
                table.Columns.Add("Statustext", GetType(String))

                Change05._returnTable = table
            End If

            Return Change05._returnTable.Clone()
        End Get
    End Property

    Protected GridNavigation1 As CKG.Services.GridNavigation

    Private _user As Base.Kernel.Security.User
    Private _app As Base.Kernel.Security.App

    Protected Property Sort As String
        Get
            Return DirectCast(Me.ViewState("Sort"), String)
        End Get
        Set(value As String)
            Me.ViewState("Sort") = value
        End Set
    End Property

    Protected Property Direction As String
        Get
            Return DirectCast(Me.ViewState("Direction"), String)
        End Get
        Set(value As String)
            Me.ViewState("Direction") = value
        End Set
    End Property

    Protected Property Uploaddaten As DataTable
        Get
            Return DirectCast(Me.Session("Uploaddaten"), DataTable)
        End Get
        Set(value As DataTable)
            Me.Session("Uploaddaten") = value
        End Set
    End Property

    Protected Property Carports As IEnumerable(Of ListItem)
        Get
            Return DirectCast(Me.Session("Carports"), IEnumerable(Of ListItem))
        End Get
        Set(value As IEnumerable(Of ListItem))
            Me.Session("Carports") = value
        End Set
    End Property

    Protected Property LogistikPartner As IEnumerable(Of ListItem)
        Get
            Return DirectCast(Me.Session("LogistikPartner"), IEnumerable(Of ListItem))
        End Get
        Set(value As IEnumerable(Of ListItem))
            Me.Session("LogistikPartner") = value
        End Set
    End Property

    Protected Property Dienstleistungen As IEnumerable(Of ListItem)
        Get
            Return DirectCast(Me.Session("Dienstleistungen"), IEnumerable(Of ListItem))
        End Get
        Set(value As IEnumerable(Of ListItem))
            Me.Session("Dienstleistungen") = value
        End Set
    End Property

    Protected Overrides Sub OnLoad(e As System.EventArgs)
        MyBase.OnLoad(e)

        Me.GridNavigation1.setGridElment(Me.GridView1)

        Me._user = Base.Kernel.Common.Common.GetUser(Me)

        Base.Kernel.Common.Common.FormAuth(Me, Me._user)
        Base.Kernel.Common.Common.GetAppIDFromQueryString(Me)

        lblHead.Text = Me._user.Applications.Select("AppID = '" & Session("AppID").ToString() & "'")(0)("AppFriendlyName").ToString()
        Me._app = New Base.Kernel.Security.App(Me._user)

        If Not Me.IsPostBack Then
            Dim s As New CarportBeauftragung(Me._user, Me._app, Me)
            Me.Carports = s.GetCarports()
            Me.LogistikPartner = s.GetLogistikPartner()
            Me.Dienstleistungen = s.GetServices()
            Dim array As ListItem() = Me.Dienstleistungen.ToArray()
            Me.ddDienstleistung1.Items.AddRange(array)
            Me.ddDienstleistung2.Items.AddRange(array)
            Me.ddDienstleistung3.Items.AddRange(array)
            Me.ddDienstleistung4.Items.AddRange(array)
            Me.ddDienstleistung5.Items.AddRange(array)
            Me.GridNavigation1.PageSizeIndex = 0
        End If
    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        Me.CheckErrors()

        MyBase.OnPreRender(e)

        Base.Kernel.Common.Common.SetEndASPXAccess(Me)
    End Sub

    Protected Overrides Sub OnUnload(e As System.EventArgs)
        MyBase.OnUnload(e)

        Base.Kernel.Common.Common.SetEndASPXAccess(Me)
    End Sub

    Protected Sub OnPagerChanged(ByVal PageIndex As Integer)
        GridView1.PageIndex = PageIndex
        FillGrid(PageIndex)
    End Sub

    Protected Sub OnPageSizeChanged()
        FillGrid(0)
    End Sub

    Protected Sub OnHochladenClick(sender As Object, e As EventArgs)
        Me.Result.Visible = False
        Me.lbBeauftragungSpeichern.Visible = False
        Me.lbPrüfen.Visible = False

        If Me.fuRückmeldungen.HasFile Then
            Dim ext As String = System.IO.Path.GetExtension(Me.fuRückmeldungen.FileName)

            If ext.Equals(".xls", StringComparison.OrdinalIgnoreCase) OrElse ext.Equals(".xlsx", StringComparison.OrdinalIgnoreCase) Then

                Dim tempFilename As String = System.IO.Path.GetTempFileName()
                tempFilename = System.IO.Path.ChangeExtension(tempFilename, ext)

                Try
                    Me.fuRückmeldungen.SaveAs(tempFilename)

                    Me.Uploaddaten = New ExcelImporter().ExceldatenLaden(tempFilename, Change05.ReturnTable)

                    If Me.Uploaddaten Is Nothing Then
                        Me.lblError.Text = "Die Datei enthält keine gültigen Daten."
                    Else
                        Me.Uploaddaten = Me.PrepareData(Me.Uploaddaten)
                        Me.Uploaddaten = Me.CheckData(Me.Uploaddaten)
                        Me.FillGrid()
                        Me.lbPrüfen.Visible = Me.Uploaddaten.AsEnumerable().Any(Function(dr) dr.Field(Of Status)("Status") = Status.New)
                    End If
                Finally
                    System.IO.File.Delete(tempFilename)
                End Try
            Else
                Me.lblError.Text = "Das Dateiformat ist ungültig."
            End If
        Else
            Me.lblError.Text = "Wählen Sie bitte eine Datei aus."
        End If
    End Sub

    Private Function ExtractDienstleistungen(dr As DataRow) As IEnumerable(Of String)
        Dim ret As New List(Of String)()
        ret.Add(dr.Field(Of String)("Dienstleistung1"))
        ret.Add(dr.Field(Of String)("Dienstleistung2"))
        ret.Add(dr.Field(Of String)("Dienstleistung3"))
        ret.Add(dr.Field(Of String)("Dienstleistung4"))
        ret.Add(dr.Field(Of String)("Dienstleistung5"))
        ret.Add(Me.ddDienstleistung1.SelectedValue)
        ret.Add(Me.ddDienstleistung2.SelectedValue)
        ret.Add(Me.ddDienstleistung3.SelectedValue)
        ret.Add(Me.ddDienstleistung4.SelectedValue)
        ret.Add(Me.ddDienstleistung5.SelectedValue)
        Return ret.Where(Function(s) Not String.IsNullOrEmpty(s))
    End Function

    Protected Sub OnBeauftragungSpeichernClick(sender As Object, e As EventArgs)
        Dim zuVerbuchen As IEnumerable(Of DataRow) = From dr In Me.Uploaddaten.AsEnumerable() _
                                                     Where dr.Field(Of Status)("Status") = Status.Ready
                                                     Select dr

        Dim c As New CarportBeauftragung(Me._user, Me._app, Me)

        For Each dr In zuVerbuchen

            If dr.Field(Of String)("Statustext") <> "B" Then
                c.SaveRückmeldung(dr.Field(Of String)("Fahrgestellnummer"), _
                                             dr.Field(Of String)("Kennzeichen"), _
                                             dr.Field(Of String)("Carportschlüssel"), _
                                             dr.Field(Of String)("Logistikpartner"), _
                                             dr.Field(Of DateTime)("Bereitstellungsdatum"), _
                                             dr.Field(Of String)("Bemerkungstext"), _
                                             Me.ExtractDienstleistungen(dr))
            End If


        Next

        For Each dr In Me.Uploaddaten.AsEnumerable()
            If dr.Field(Of Status)("Status") = Status.Ready Then
                dr.SetField("Status", Status.Saved)
                If dr.Field(Of String)("Statustext") <> "B" Then
                    dr.SetField("Statustext", "Carportbeauftragung erfolgt")
                End If

            End If
        Next

        Me.FillGrid()

        Me.lbBeauftragungSpeichern.Visible = False
    End Sub

    Protected Sub OnPrüfenClick(sender As Object, e As EventArgs)
        Dim zuPrüfen As IEnumerable(Of String) = From dr In Me.Uploaddaten.AsEnumerable() _
                                             Where dr.Field(Of Status)("Status") = Status.New
                                             Select dr.Field(Of String)("Fahrgestellnummer")

        Dim c As New CarportBeauftragung(Me._user, Me._app, Me)
        Dim stat As IEnumerable(Of String) = c.GetAuftragsstatus(zuPrüfen)

        Using [enum] As IEnumerator(Of String) = stat.GetEnumerator()
            For Each dr In Me.Uploaddaten.AsEnumerable()
                If dr.Field(Of Status)("Status") = status.New Then
                    [enum].MoveNext()
                    dr.SetField("Status", status.Ready)
                    dr.SetField("Statustext", [enum].Current)
                End If
            Next
        End Using

        Me.FillGrid()

        Me.lbPrüfen.Visible = False
        Me.lbBeauftragungSpeichern.Visible = Me.Uploaddaten.AsEnumerable().Any(Function(dr) dr.Field(Of Status)("Status") = Status.Ready)
    End Sub

    Private Function PrepareData(daten As DataTable) As DataTable
        Dim x = From d In daten.AsEnumerable() _
                Group Join l In Me.Carports _
                On d.Field(Of String)("Carportschlüssel") Equals l.Value _
                Into g = Group _
                Select New Pair(d, g.SingleOrDefault())

        Dim y = x.Select(Function(p As Pair) As DataRow
                             Dim dr As DataRow = DirectCast(p.First, DataRow)
                             Dim la As ListItem = DirectCast(p.Second, ListItem)
                             If la Is Nothing Then
                                 dr.SetField(Of String)("Carporttext", Nothing)
                                 dr.SetField("Status", Status.Error)
                                 dr.SetField("Statustext", "Problem mit dem Carport")
                             Else
                                 dr.SetField("Carporttext", la.Text)
                                 dr.SetField("Status", Status.New)
                                 dr.SetField(Of String)("Statustext", Nothing)
                             End If

                             Return dr
                         End Function)

        x = From d In y _
            Group Join p In Me.LogistikPartner _
            On d.Field(Of String)("Logistikpartner") Equals p.Value _
            Into g = Group _
            Select New Pair(d, g.SingleOrDefault())

        y = x.Select(Function(p As Pair) As DataRow
                         Dim dr As DataRow = DirectCast(p.First, DataRow)
                         Dim la As ListItem = DirectCast(p.Second, ListItem)
                         If la Is Nothing Then
                             If Not String.IsNullOrEmpty(dr.Field(Of String)("Logistikpartner")) Then
                                 dr.SetField(Of String)("Logistikpartner", Nothing)
                                 dr.SetField("Status", Status.Error)
                                 dr.SetField("Statustext", String.Concat(dr.Field(Of String)("Statustext"), Environment.NewLine, "Problem mit dem Logistikpartner"))
                             End If
                         Else
                             dr.SetField("Logistikpartner", la.Text)
                         End If

                         Return dr
                     End Function)

        y = Me.PrepareDienstlesitung("Dienstleistung1", y)
        y = Me.PrepareDienstlesitung("Dienstleistung2", y)
        y = Me.PrepareDienstlesitung("Dienstleistung3", y)
        y = Me.PrepareDienstlesitung("Dienstleistung4", y)
        daten = Me.PrepareDienstlesitung("Dienstleistung5", y).CopyToDataTable()

        Return daten
    End Function

    Private Function PrepareDienstlesitung(fieldName As String, daten As IEnumerable(Of DataRow)) As IEnumerable(Of DataRow)
        For Each dr In daten
            Dim value As String = dr.Field(Of String)(fieldName)

            If Not String.IsNullOrEmpty(value) Then
                dr.SetField(fieldName, value.PadLeft(18, "0"c))
            End If
        Next

        Dim x = From d In daten _
                Group Join l In Me.Dienstleistungen _
                On d.Field(Of String)(fieldName) Equals l.Value _
                Into g = Group _
                Select New Pair(d, g.SingleOrDefault())

        Return x.Select(Function(p As Pair) As DataRow
                            Dim dr As DataRow = DirectCast(p.First, DataRow)
                            Dim la As ListItem = DirectCast(p.Second, ListItem)
                            If la Is Nothing Then
                                If Not String.IsNullOrEmpty(dr.Field(Of String)(fieldName)) Then
                                    dr.SetField(Of String)(String.Concat(fieldName, "Text"), Nothing)
                                    dr.SetField("Status", Status.Error)
                                    dr.SetField("Statustext", String.Concat(dr.Field(Of String)("Statustext"), Environment.NewLine, "Problem mit der ", fieldName))
                                End If
                            Else
                                dr.SetField(fieldName, la.Value)
                                dr.SetField(String.Concat(fieldName, "Text"), la.Text)
                            End If

                            Return dr
                        End Function)
    End Function

    Private Function CheckData(daten As DataTable) As DataTable
        'Dim idents = From d In daten.AsEnumerable() _
        '             Where d.Field(Of Status)("Status") = Status.New _
        '             Select d.Field(Of String)("Fahrgestellnummer")

        'Dim r As New Rückmeldung(Me._user, Me._app, Me)
        'Dim details As IEnumerable(Of Rückmeldungsdetails) = r.GetRückmeldungsdetails(idents)

        'Dim x = From d In daten _
        '        Group Join l In details _
        '        On d.Field(Of String)("Fahrgestellnummer") Equals l.Fahrgestellnummer _
        '        And d.Field(Of String)("LeistungsartIntern") Equals l.LeistungsartIntern _
        '        Into g = Group _
        '        Select New Pair(d, g.FirstOrDefault(Function(ll) ll.Endrückmeldung AndAlso Not ll.Abgeschlossen))

        'daten = x.Select(Function(p As Pair) As DataRow
        '                     Dim dr As DataRow = DirectCast(p.First, DataRow)
        '                     Dim rd As Rückmeldungsdetails = DirectCast(p.Second, Rückmeldungsdetails)

        '                     If dr.Field(Of Status)("Status") = Status.New Then
        '                         If rd Is Nothing Then
        '                             dr.SetField("Status", Status.Ready)
        '                             dr.SetField("Statustext", "Rückmeldung kann erfolgen")
        '                         Else
        '                             dr.SetField("Status", Status.Error)
        '                             dr.SetField(Of String)("Statustext", "Rückmeldung schon durchgeführt")
        '                         End If
        '                     End If

        '                     Return dr
        '                 End Function).CopyToDataTable()

        Return daten
    End Function

    Private Sub CheckErrors()
        Dim daten As DataTable = Me.Uploaddaten

        If daten IsNot Nothing AndAlso daten.AsEnumerable().Any(Function(d) d.Field(Of Status)("Status") = Status.Error) Then
            Me.lblErrorVorhanden.Visible = True
        End If
    End Sub

    Private Sub FillGrid(Optional ByVal pageIndex As Integer = 0, Optional ByVal sort As String = "")
        Dim direction As String = ""
        Dim tmpDataView As DataView = Me.Uploaddaten.DefaultView
        tmpDataView.RowFilter = ""

        GridView1.Visible = True
        Result.Visible = True

        If Not String.IsNullOrEmpty(sort) Then
            pageIndex = 0
            sort = sort.Trim()

            If (Me.Sort Is Nothing) OrElse Me.Sort.Equals(sort) Then
                direction = If(Me.Direction Is Nothing, "desc", Me.Direction)
            Else
                direction = "desc"
            End If

            If direction = "asc" Then
                direction = "desc"
            Else
                direction = "asc"
            End If

            Me.Sort = sort
            Me.Direction = direction
        Else
            If Me.Sort IsNot Nothing Then
                sort = Me.Sort

                direction = If(Me.Direction Is Nothing, "asc", Me.Direction)
                Me.Direction = direction
            End If
        End If

        If Not String.IsNullOrEmpty(sort) Then
            tmpDataView.Sort = sort & " " & direction
        End If

        GridView1.DataSource = tmpDataView
        GridView1.DataBind()
        GridView1.PageIndex = pageIndex
    End Sub

    Protected Sub GridRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Dim row As GridViewRow = e.Row

        If row.RowType = DataControlRowType.DataRow Then
            Dim status As Status = DirectCast(DataBinder.Eval(row.DataItem, "Status"), Status)
            Dim lbl As Label
            Dim bereitstellungsdatum As DateTime
            Dim lblBereitstellungsdatum As Label

            Dim bgColour As Drawing.Color?

            If status = status.Error Then
                bgColour = Drawing.Color.Pink
            ElseIf status = ComCommon.Status.Ready Then
                bgColour = Drawing.Color.LightGoldenrodYellow
            ElseIf status = ComCommon.Status.Saved Then
                bgColour = Drawing.Color.LightGreen
            End If

            lbl = CType(row.FindControl("lblStatustext"), Label)

            If lbl.Text = "B" Then
                bgColour = Drawing.Color.LightSalmon
                lblBeauftragt.Visible = True
            End If

            lblBereitstellungsdatum = CType(row.FindControl("lblBereitstellungsdatum"), Label)

            Dim okay As Boolean = DateTime.TryParse(lblBereitstellungsdatum.Text, bereitstellungsdatum)

            If okay = False OrElse CDate(lblBereitstellungsdatum.Text) <= Today Then

                bgColour = Drawing.Color.LightBlue
                lblErrBereitstellungsdatum.Visible = True

            End If

            If bgColour.HasValue Then
                For Each cell As TableCell In row.Cells
                    cell.BackColor = bgColour.Value
                Next
            End If
        End If
    End Sub

    Private NotInheritable Class PairIgnoreCaseComparer
        Implements IEqualityComparer(Of Pair)
        Private ReadOnly comparer As IEqualityComparer(Of String) = StringComparer.OrdinalIgnoreCase

        Public Function Equals1(x As Pair, y As Pair) As Boolean _
            Implements System.Collections.Generic.IEqualityComparer(Of Pair).Equals
            Return comparer.Equals(DirectCast(x.First, String), DirectCast(y.First, String)) _
                AndAlso comparer.Equals(DirectCast(x.Second, String), DirectCast(y.Second, String))
        End Function

        Public Function GetHashCode1(obj As Pair) As Integer _
            Implements System.Collections.Generic.IEqualityComparer(Of Pair).GetHashCode
            Return comparer.GetHashCode(If(DirectCast(obj.First, String), String.Empty)) Xor (comparer.GetHashCode(If(DirectCast(obj.Second, String), String.Empty)) >> 3)
        End Function
    End Class

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Sub Change05_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        CKG.Base.Kernel.Common.Common.SetEndASPXAccess(Me)
        CKG.Base.Business.HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

   
    Private Sub Change05_Unload(sender As Object, e As System.EventArgs) Handles Me.Unload
        CKG.Base.Kernel.Common.Common.SetEndASPXAccess(Me)
    End Sub
End Class
