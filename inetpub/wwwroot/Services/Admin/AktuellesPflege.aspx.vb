
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common
Imports System.Data.SqlClient
Imports System.IO

Partial Public Class AktuellesPflege
    Inherits Page

    Private _user As User
    Private _aktuelles As Aktuelles

    Private Const ImageDirectory As String = "/Services/bilder/Customize/"
    Private Const FileNameFormat As String = "{0}_{1}"

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        _user = Common.GetUser(Me)
        Common.AdminAuth(Me, _user, AdminLevel.Organization)
        'Dim cn As New SqlConnection(_user.App.Connectionstring)
        'cn.Open()

        lblError.Text = ""

        If Not Session("AppAktuelles") Is Nothing Then
            _aktuelles = Session("AppAktuelles")
            If Not IsPostBack Then
                If _aktuelles.IsNew = True Then
                    NeuOderEditieren(True)
                Else
                    NeuOderEditieren(False)
                End If
            End If
        End If

        FillImagesList(_aktuelles.CustomerID)
    End Sub


    Protected Sub btnBack_click(ByVal sender As Object, ByVal e As EventArgs) Handles Back.Click
        Response.Redirect("AktuellePflegeUebersicht.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Sub ddlKundenliste_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles ddlKundenliste.SelectedIndexChanged
        'mNichtSpeichern = True
    End Sub

    Protected Sub btnSpeichern_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSpeichern.Click
        If Speichern() Then
            ' Wechsel in den Editiermodus
            _aktuelles.IsNew = False
            Session("AppAktuelles") = _aktuelles
            NeuOderEditieren(_aktuelles.IsNew)
        End If
    End Sub

    Protected Sub AddImageClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn = CType(sender, LinkButton)
        Dim file = btn.CommandArgument

        If Not String.IsNullOrEmpty(file) Then AddImage(file)
    End Sub

    Protected Sub AddNewImageClick(ByVal sender As Object, ByVal e As EventArgs)
        If Not fileUpload.HasFile Then Return

        Dim customerId = _aktuelles.CustomerID

        'absoluten Pfad ermitteln
        Dim physPath = HttpContext.Current.Server.MapPath(ImageDirectory)

        If Not Directory.Exists(physPath) Then
            Directory.CreateDirectory(physPath)
        End If

        Dim filename = String.Format(FileNameFormat, customerId, fileUpload.FileName.Replace("-", "_"))
        fileUpload.SaveAs(Path.Combine(physPath, filename))

        FillImagesList(customerId)
        AddImage(Path.Combine(ImageDirectory, filename))
    End Sub

#End Region

#Region "Methods"

    Private Sub AddImage(virtualImagePath As String)
        Editor1.Content += String.Format("<img src='{0}' />", virtualImagePath)
    End Sub

    Private Sub FillImagesList(ByVal customerId As Integer)
        Dim physPath = Server.MapPath(ImageDirectory)
        Dim files = Directory.GetFiles(physPath, String.Format(FileNameFormat, customerId, "*"))

        Dim virtFiles = files.Select(Function(f) Path.Combine(ImageDirectory, Path.GetFileName(f))).ToArray()
        customerImages.DataSource = virtFiles
        customerImages.DataBind()
    End Sub


    Sub FillKunden()
        Dim dtKunden As Kernel.CustomerList
        Dim cn As New SqlConnection(_user.App.Connectionstring)

        dtKunden = New Kernel.CustomerList(_user.Customer.AccountingArea, cn)

        Try
            With ddlKundenliste
                Dim dv As DataView = dtKunden.DefaultView
                dv.Sort = "Customername"
                .DataSource = dv
                .DataTextField = "Customername"
                .DataValueField = "CustomerID"
                .DataBind()
                .Items.FindByValue(_aktuelles.CustomerID).Selected = True
            End With

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Sub Felderleeren()
        txtDateBis.Text = ""
        txtBeitrag.Text = ""
        chkAktiv.Checked = False
        Editor1.Content = ""
        fileUpload.Dispose()
    End Sub

    Sub NeuOderEditieren(ByVal neu As Boolean)

        ddlKundenliste.Visible = neu
        'trFileupload.Visible = neu

        lblKundenliste.Visible = Not neu
        'trBildbehalten.Visible = Not neu

        If neu Then
            Felderleeren()
            FillKunden()
        Else
            BeitragEditieren()
        End If
    End Sub

    Function TextKonvertieren(ByVal text As String, ByVal reverse As Boolean) As String
        If Not reverse Then
            text = text.Replace("<", "{")
            text = text.Replace(">", "}")
            text = text.Replace("'", "´")
            Return text.Trim()
        Else
            text = text.Replace("{", "<")
            text = text.Replace("}", ">")
            text = text.Replace("´", "'")
            Return text.Trim()
        End If
    End Function

    Sub FillVorschau(ByVal bildlink As String, ByVal text As String)
        ltlVorschau.Text = TextKonvertieren(text, True)
        'imgVorschau.ImageUrl = bildlink
        'imgVorschau.Visible = Not String.IsNullOrEmpty(bildlink)
    End Sub

    Sub BeitragEditieren()
        Try
            ' Beitragtext aus DB holen
            Dim cn As New SqlConnection(_user.App.Connectionstring)

            Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT " &
                                         "   Customer.Customername, " &
                                         "	CustomerNews.ID, " &
                                         "	CustomerNews.CustomerID, " &
                                         "	CustomerNews.BeitragName, " &
                                         "	CustomerNews.Text, " &
                                         "	CustomerNews.ImagePath, " &
                                         "	CustomerNews.GueltigBis, " &
                                         "   CustomerNews.aktiv " &
                                         " FROM CustomerNews INNER JOIN Customer " &
                                         "   ON CustomerNews.CustomerID = Customer.CustomerID " &
                                         " WHERE ID = " & _aktuelles.Id, cn)

            Dim tblBeitraegsinhalt As New DataTable
            da.Fill(tblBeitraegsinhalt)

            txtBeitrag.Text = CStr(tblBeitraegsinhalt.Rows(0)("BeitragName")).Trim
            lblKundenliste.Text = tblBeitraegsinhalt.Rows(0)("CustomerName").ToString.Trim
            txtDateBis.Text = CDate(tblBeitraegsinhalt.Rows(0)("GueltigBis"))
            chkAktiv.Checked = CBool(tblBeitraegsinhalt.Rows(0)("aktiv"))
            Dim konvertierterText As String = TextKonvertieren(CStr(tblBeitraegsinhalt.Rows(0)("Text")), True)
            Editor1.Content = konvertierterText

            Dim bildpfad As String = TryCast(tblBeitraegsinhalt.Rows(0)("ImagePath"), String)
            'chkBildbehalten.Checked = Not String.IsNullOrEmpty(bildpfad)
            'BildbehaltenControlWechsel()

            'If String.IsNullOrEmpty(bildpfad) Then
            '    bildpfad = "../bilder/responsible/NoPicture.JPG"
            'End If

            FillVorschau(bildpfad, konvertierterText)
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

#End Region

#Region "Functions"

    Function Speichern() As Boolean
        If Not IsDate(txtDateBis.Text) Then
            lblError.Text = "Das Feld ""Gültig Bis"" ist nicht korrekt befüllt."
            Return False
        ElseIf Editor1.Content.Trim.Length > 1000 Then
            lblError.Text =
                "Der Text inkl. HTML-Tags enthält mehr als 1000 Zeichen und kann deshalb nicht gespeichert werden."
            Return False
        Else
            Try
                Dim cn = New SqlConnection(_user.App.Connectionstring)

                'SQL Connection öffnen
                cn.Open()

                Dim newContent = TextKonvertieren(Editor1.Content, False)

                If _aktuelles.IsNew Then
                    Dim sc As New SqlCommand(
                                String.Format(
                                    "INSERT INTO CustomerNews (CustomerID,BeitragName,GueltigBis,aktiv,Text)" &
                                    "VALUES ({0}, '{1}', '{2}', {3}, '{4}')",
                                    ddlKundenliste.SelectedValue, txtBeitrag.Text, CDate(txtDateBis.Text),
                                    CInt(chkAktiv.Checked), newContent),
                                cn)
                    sc.ExecuteNonQuery()

                    'ID für neuen Beitrag an Session übergeben
                    Dim da As New SqlDataAdapter("SELECT TOP 100 PERCENT ID, CustomerID " &
                                                 "FROM dbo.CustomerNews " &
                                                 "WHERE  ID = (SELECT MAX(ID) FROM CustomerNews)", cn)

                    Dim tblBeitraegsinhalt As New DataTable
                    da.Fill(tblBeitraegsinhalt)

                    _aktuelles.Id = CInt(tblBeitraegsinhalt.Rows(0)("ID"))
                    _aktuelles.CustomerID = CInt(tblBeitraegsinhalt.Rows(0)("CustomerID"))
                    Session("AppAktuelles") = _aktuelles

                Else
                    Dim sc As New SqlCommand(
                                String.Format(
                                    "UPDATE CustomerNews SET CustomerID = {0}, BeitragName = '{1}', GueltigBis = '{2}', " &
                                    "aktiv = {3}, Text = '{4}' WHERE ID = {5}",
                                    CInt(_aktuelles.CustomerID), CStr(txtBeitrag.Text), CDate(txtDateBis.Text),
                                    CInt(chkAktiv.Checked), newContent, _aktuelles.Id),
                                cn)
                    sc.ExecuteNonQuery()
                End If

                'SQL Connection schließen
                cn.Close()

            Catch ex As Exception
                lblError.Text = ex.Message
                Return False
            End Try
        End If
        Return True
    End Function

#End Region


    
End Class

