Option Strict On
Option Explicit On

Public Class Change03
    Inherits System.Web.UI.Page

    Private Shared _returnTable As DataTable

    Private Shared ReadOnly Property ReturnTable As DataTable
        Get
            If Change03._returnTable Is Nothing Then
                Dim table As New DataTable()
                table.Columns.Add("Fahrgestellnummer", GetType(String)).ExtendedProperties.Add("SearchString", "Fahrgestell")
                table.Columns.Add("Leistungsart", GetType(String)).ExtendedProperties.Add("SearchString", "Leistung")
                table.Columns.Add("LeistungsartIntern", GetType(String))
                table.Columns.Add("LeistungsartText", GetType(String))
                table.Columns.Add("PrognoseArbeitsende", GetType(DateTime)).ExtendedProperties.Add("SearchString", "Prognose")
                table.Columns.Add("Endrückmeldung", GetType(String)).ExtendedProperties.Add("SearchString", "Meldung")
                table.Columns.Add("Bemerkungstext", GetType(String)).ExtendedProperties.Add("SearchString", "Bemerkung")
                table.Columns.Add("Status", GetType(Status))
                table.Columns.Add("Statustext", GetType(String))

                Change03._returnTable = table
            End If

            Return Change03._returnTable.Clone()
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

    Protected Property Leistungsarten As IEnumerable(Of Leistungsart)
        Get
            Return DirectCast(Me.Session("Leistungsarten"), IEnumerable(Of Leistungsart))
        End Get
        Set(value As IEnumerable(Of Leistungsart))
            Me.Session("Leistungsarten") = value
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
            Dim s As New Rückmeldung(Me._user, Me._app, Me)
            Me.Leistungsarten = s.GetBerechtigteLeistungsarten()
            Me.rMöglicheLeistungen.DataSource = Me.Leistungsarten
            Me.rMöglicheLeistungen.DataBind()
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
        Me.lbRückmeldungenVerbuchen.Visible = False

        If Me.fuRückmeldungen.HasFile Then
            Dim ext As String = System.IO.Path.GetExtension(Me.fuRückmeldungen.FileName)

            If ext.Equals(".xls", StringComparison.OrdinalIgnoreCase) OrElse ext.Equals(".xlsx", StringComparison.OrdinalIgnoreCase) Then

                Dim tempFilename As String = System.IO.Path.GetTempFileName()
                tempFilename = System.IO.Path.ChangeExtension(tempFilename, ext)

                Try
                    Me.fuRückmeldungen.SaveAs(tempFilename)

                    Me.Uploaddaten = New ExcelImporter().ExceldatenLaden(tempFilename, Change03.ReturnTable)

                    If Me.Uploaddaten Is Nothing Then
                        Me.lblError.Text = "Die Datei enthält keine gültige Daten."
                    Else
                        Me.Uploaddaten = Me.PrepareData(Me.Uploaddaten)
                        Me.Uploaddaten = Me.CheckData(Me.Uploaddaten)
                        Me.FillGrid()
                        Me.lbRückmeldungenVerbuchen.Visible = Me.Uploaddaten.AsEnumerable().Any(Function(dr) dr.Field(Of Status)("Status") = Status.Ready)
                    End If
                Finally
                    System.IO.File.Delete(tempFilename)
                End Try
            Else
                Me.lblError.Text = "Der Dateiformat ist ungültig."
            End If
        Else
            Me.lblError.Text = "Wählen Sie bitte eine Datei aus."
        End If
    End Sub

    Protected Sub OnRückmeldungenVerbuchenClick(sender As Object, e As EventArgs)
        Dim zuVerbuchen As IEnumerable(Of DataRow) = From dr In Me.Uploaddaten.AsEnumerable() _
                                                     Where dr.Field(Of Status)("Status") = Status.Ready
                                                     Select dr

        Dim r As New Rückmeldung(Me._user, Me._app, Me)
        Dim errors As IEnumerable(Of Rückmeldungsfehler) = r.SaveRückmeldungen(zuVerbuchen)

        Me.Uploaddaten = Me.Uploaddaten.AsEnumerable().GroupJoin(errors, _
                                                                 Function(d) New Pair(d.Field(Of String)("Fahrgestellnummer"), d.Field(Of String)("LeistungsartIntern")), _
                                                                 Function(er) New Pair(er.Fahrgestellnummer, er.LeistungsartIntern), _
            Function(dr As DataRow, es As IEnumerable(Of Rückmeldungsfehler)) As DataRow
                Dim fehler As Rückmeldungsfehler = es.FirstOrDefault()
                If dr.Field(Of Status)("Status") = Status.Ready Then
                    If fehler Is Nothing Then
                        dr.SetField("Status", Status.Saved)
                        dr.SetField("Statustext", "Rückmeldung erfolgreich")
                    Else
                        dr.SetField("Status", Status.Error)
                        dr.SetField("Statustext", String.Concat(fehler.Meldungstyp, ": ", fehler.Meldung))
                    End If
                End If
                Return dr
            End Function, _
            New PairIgnoreCaseComparer()).CopyToDataTable()

        Me.FillGrid()

        Me.lbRückmeldungenVerbuchen.Visible = False
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub

    Private Function PrepareData(daten As DataTable) As DataTable
        Dim x = From d In daten _
                Group Join l In Me.Leistungsarten _
                On d.Field(Of String)("Leistungsart") Equals l.ExterneKennung _
                Into g = Group _
                Select New Pair(d, g.SingleOrDefault())

        daten = x.Select(Function(p As Pair) As DataRow
                             Dim dr As DataRow = DirectCast(p.First, DataRow)
                             Dim la As Leistungsart = DirectCast(p.Second, Leistungsart)
                             If la Is Nothing Then
                                 dr.SetField(Of String)("LeistungsartIntern", Nothing)
                                 dr.SetField(Of String)("LeistungsartText", Nothing)
                                 dr.SetField("Status", Status.Error)
                                 dr.SetField("Statustext", "Problem mit der Leistungsart")
                             Else
                                 dr.SetField(Of String)("LeistungsartIntern", la.InterneKennung)
                                 dr.SetField("LeistungsartText", la.Leistungsart)
                                 dr.SetField("Status", Status.New)
                                 dr.SetField(Of String)("Statustext", Nothing)
                             End If

                             Return dr
                         End Function).CopyToDataTable()

        Return daten
    End Function

    Private Function CheckData(daten As DataTable) As DataTable
        Dim idents = From d In daten.AsEnumerable() _
                     Where d.Field(Of Status)("Status") = Status.New _
                     Select d.Field(Of String)("Fahrgestellnummer")

        Dim r As New Rückmeldung(Me._user, Me._app, Me)
        Dim details As IEnumerable(Of Rückmeldungsdetails) = r.GetRückmeldungsdetails(idents)

        Dim x = From d In daten _
                Group Join l In details _
                On d.Field(Of String)("Fahrgestellnummer") Equals l.Fahrgestellnummer _
                And d.Field(Of String)("LeistungsartIntern") Equals l.LeistungsartIntern _
                Into g = Group _
                Select New Pair(d, g.FirstOrDefault(Function(ll) ll.Endrückmeldung AndAlso Not ll.Abgeschlossen))

        daten = x.Select(Function(p As Pair) As DataRow
                             Dim dr As DataRow = DirectCast(p.First, DataRow)
                             Dim rd As Rückmeldungsdetails = DirectCast(p.Second, Rückmeldungsdetails)

                             If dr.Field(Of Status)("Status") = Status.New Then
                                 If rd Is Nothing Then
                                     dr.SetField("Status", Status.Ready)
                                     dr.SetField("Statustext", "Rückmeldung kann erfolgen")
                                 Else
                                     dr.SetField("Status", Status.Error)
                                     dr.SetField(Of String)("Statustext", "Rückmeldung schon durchgeführt")
                                 End If
                             End If

                             Return dr
                         End Function).CopyToDataTable()

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

            Dim bgColour As Drawing.Color?

            If status = status.Error Then
                bgColour = Drawing.Color.Pink
            ElseIf status = ComCommon.Status.Ready Then
                bgColour = Drawing.Color.LightGoldenrodYellow
            ElseIf status = ComCommon.Status.Saved Then
                bgColour = Drawing.Color.LightGreen
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
End Class
