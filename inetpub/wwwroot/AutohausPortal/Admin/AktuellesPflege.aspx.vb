
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common

Partial Public Class AktuellesPflege
    Inherits System.Web.UI.Page

    Private m_User As User
    Private m_App As App
    Private mObjAktuelles As Aktuelles
    Private mNichtSpeichern As Boolean

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        AdminAuth(Me, m_User, AdminLevel.Organization)
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)
        cn.Open()

        If Not Session("AppAktuelles") Is Nothing Then
            mObjAktuelles = Session("AppAktuelles")
            If IsPostBack Then
                lblError.Visible = False
            Else
                If mObjAktuelles.IsNew = True Then
                    NeuOderEditieren(True)
                Else
                    NeuOderEditieren(False)
                End If
            End If
        End If
    End Sub

    Protected Sub btnBack_click(ByVal sender As Object, ByVal e As EventArgs) Handles Back.Click
        Response.Redirect("AktuellePflegeUebersicht.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Sub ddlKundenliste_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlKundenliste.SelectedIndexChanged
        mNichtSpeichern = True
    End Sub

    Protected Sub btnSpeichern_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSpeichern.Click
        If Speichern() Then
            ' Wechsel in den Editiermodus
            mObjAktuelles.IsNew = False
            Session("AppAktuelles") = mObjAktuelles
            If mObjAktuelles.IsNew = True Then
                NeuOderEditieren(True)
            Else
                NeuOderEditieren(False)
            End If
        End If
    End Sub

    Protected Sub chkBildbehalten_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkBildbehalten.CheckedChanged
        BildbehaltenControlWechsel()
    End Sub

#End Region
#Region "Methods"
    Sub BildbehaltenControlWechsel()
        If chkBildbehalten.Checked Then
            trFileupload.Visible = False
        Else
            trFileupload.Visible = True
        End If
    End Sub
    Sub FillKunden()
        Dim dtKunden As Kernel.CustomerList
        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)

        dtKunden = New Kernel.CustomerList(m_User.Customer.AccountingArea, cn)

        Try
            With ddlKundenliste
                Dim dv As DataView = dtKunden.DefaultView
                dv.Sort = "Customername"
                .DataSource = dv
                .DataTextField = "Customername"
                .DataValueField = "CustomerID"
                .DataBind()
                .Items.FindByValue(mObjAktuelles.CustomerID).Selected = True
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

    Sub NeuOderEditieren(ByVal Neu As Boolean)
        If Neu = False Then
            lblKundenliste.Visible = True
            ddlKundenliste.Visible = False
            trBildbehalten.Visible = True
            trFileupload.Visible = False

            BeitragEditieren()
        Else
            lblKundenliste.Visible = False
            ddlKundenliste.Visible = True
            trBildbehalten.Visible = False
            trFileupload.Visible = True

            Felderleeren()
            FillKunden()
        End If
    End Sub

    Function TextKonvertieren(ByVal Text As String, ByVal Reverse As Boolean) As String
        If Reverse = False Then
            Text = Text.Replace("<", "{")
            Text = Text.Replace(">", "}")
            Text = Text.Replace("'", "´")
            Text.Trim()
            Return Text
        Else
            Text = Text.Replace("{", "<")
            Text = Text.Replace("}", ">")
            Text = Text.Replace("´", "'")
            Text.Trim()
            Return Text
        End If
    End Function

    Sub FillVorschau(ByVal Bildlink As String, ByVal Text As String)
        ltlVorschau.Text = TextKonvertieren(Text, True)
        imgVorschau.ImageUrl = Bildlink
    End Sub

    Function GetFileString() As String
        Dim fileuploaded As Boolean = False
        Dim filestring As String
        Dim filename As String = fileUpload.FileName.Replace("-", "_")
        If fileUpload.HasFile Then
            fileUpload.SaveAs("C:\Inetpub\wwwroot\Services\bilder\news\" & filename)
            fileuploaded = True
        End If

        If fileuploaded Then
            filestring = "../bilder/news/" & filename
        Else
            filestring = "../bilder/responsible/NoPicture.JPG"
        End If
        GetFileString = filestring
    End Function

    Sub BeitragEditieren()
        Try
            ' Beitragtext aus DB holen
            Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)

            Dim da As New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT " & _
                                                "   CKGPortal.dbo.Customer.Customername, " & _
                                                   "	CKGPortal.dbo.CustomerNews.ID, " & _
                                                   "	CKGPortal.dbo.CustomerNews.CustomerID, " & _
                                                   "	CKGPortal.dbo.CustomerNews.BeitragName, " & _
                                                   "	CKGPortal.dbo.CustomerNews.Text, " & _
                                                   "	CKGPortal.dbo.CustomerNews.ImagePath, " & _
                                                   "	CKGPortal.dbo.CustomerNews.GueltigBis, " & _
                                                   "   CKGPortal.dbo.CustomerNews.aktiv " & _
                                                   " FROM CKGPortal.dbo.CustomerNews INNER JOIN CKGPortal.dbo.Customer " & _
                                                   "   ON CKGPortal.dbo.CustomerNews.CustomerID = CKGPortal.dbo.Customer.CustomerID " & _
                                                   " WHERE ID = " & mObjAktuelles.Id, cn)
            Dim tblBeitraegsinhalt As New DataTable
            Dim KonvertierterText As String
            Dim Bildpfad As String

            da.Fill(tblBeitraegsinhalt)

            txtBeitrag.Text = CStr(tblBeitraegsinhalt.Rows(0)("BeitragName")).Trim
            lblKundenliste.Text = tblBeitraegsinhalt.Rows(0)("CustomerName").ToString.Trim 'Row("CustomerName")
            txtDateBis.Text = CDate(tblBeitraegsinhalt.Rows(0)("GueltigBis"))
            chkAktiv.Checked = CBool(tblBeitraegsinhalt.Rows(0)("aktiv"))
            KonvertierterText = TextKonvertieren(CStr(tblBeitraegsinhalt.Rows(0)("Text")), True)
            Editor1.Content = KonvertierterText

            If tblBeitraegsinhalt.Rows(0)("ImagePath").trim Is Nothing Then
                Bildpfad = "../bilder/responsible/NoPicture.JPG"
            Else
                Bildpfad = tblBeitraegsinhalt.Rows(0)("ImagePath").Trim
            End If

            FillVorschau(Bildpfad, KonvertierterText)
        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = ex.Message
        End Try
    End Sub

#End Region

#Region "Functions"
    Function Speichern() As Boolean
        If Not IsDate(Me.txtDateBis.Text) Then
            Me.lblError.Text = "Das Feld ""Gültig Bis"" ist nicht korrekt befüllt."
            lblError.Visible = True
            Speichern = False
            Exit Function
        ElseIf Editor1.Content.Trim.Length > 499 Then
            Me.lblError.Text = "Der Text inkl. HTML-Tags enthält mehr als 500 Zeichen und kann deshalb nicht gespeichert werden."
            lblError.Visible = True
            Speichern = False
            Exit Function
        Else
            Try
                Dim cn As SqlClient.SqlConnection = New SqlClient.SqlConnection(m_User.App.Connectionstring)
                cn.Open()   'SQL Connection öffnen

                If mObjAktuelles.IsNew = True Then
                    Dim sc As New SqlClient.SqlCommand("INSERT INTO CustomerNews (CustomerID,BeitragName,GueltigBis,aktiv,Text,ImagePath)" & _
                                                            "VALUES (" & ddlKundenliste.SelectedValue & "," & _
                                                            "'" & txtBeitrag.Text & "'," & _
                                                            "'" & CDate(txtDateBis.Text) & "'," & _
                                                            CInt(chkAktiv.Checked) & ", " & _
                                                            "'" & TextKonvertieren(Editor1.Content, False) & "'," & _
                                                            "'" & GetFileString() & "')", cn)
                    sc.ExecuteNonQuery()

                    'ID für neuen Beitrag an Session übergeben
                    Dim da As New SqlClient.SqlDataAdapter("SELECT TOP 100 PERCENT ID, CustomerID " & _
                                                           "FROM dbo.CustomerNews " & _
                                                           "WHERE  ID = (SELECT MAX(ID) FROM CustomerNews)", cn)

                    Dim tblBeitraegsinhalt As New DataTable
                    da.Fill(tblBeitraegsinhalt)

                    mObjAktuelles.Id = CInt(tblBeitraegsinhalt.Rows(0)("ID"))
                    mObjAktuelles.CustomerID = CInt(tblBeitraegsinhalt.Rows(0)("CustomerID"))
                    Session("AppAktuelles") = mObjAktuelles

                ElseIf mObjAktuelles.IsNew = False Then
                    If chkBildbehalten.Checked Then
                        Dim sc As New SqlClient.SqlCommand("UPDATE CustomerNews SET" & _
                                                                                    "   CustomerID = " & CInt(mObjAktuelles.CustomerID) & ", " & _
                                                                                    "   BeitragName = '" & CStr(txtBeitrag.Text) & "', " & _
                                                                                    "   GueltigBis = '" & CDate(txtDateBis.Text) & "', " & _
                                                                                    "   aktiv = " & CInt(chkAktiv.Checked) & ", " & _
                                                                                    "   Text = '" & TextKonvertieren(Editor1.Content, False) & "'" & _
                                                                                    "   WHERE ID = " & mObjAktuelles.Id, cn)
                        sc.ExecuteNonQuery()

                    Else
                        Dim sc As New SqlClient.SqlCommand("UPDATE CustomerNews SET" & _
                                                                                    "   CustomerID = " & CInt(mObjAktuelles.CustomerID) & ", " & _
                                                                                    "   BeitragName = '" & CStr(txtBeitrag.Text) & "', " & _
                                                                                    "   GueltigBis = '" & CDate(txtDateBis.Text) & "', " & _
                                                                                    "   aktiv = " & CInt(chkAktiv.Checked) & ", " & _
                                                                                    "   Text = '" & TextKonvertieren(Editor1.Content, False) & "', " & _
                                                                                    "   ImagePath = '" & GetFileString() & "'" & _
                                                                                    "   WHERE ID = " & mObjAktuelles.Id, cn)
                        sc.ExecuteNonQuery()
                    End If
                End If
                chkBildbehalten.Checked = True
                cn.Close()  'SQL Connection schließen
            Catch ex As Exception
                lblError.Text = ex.Message
                lblError.Visible = True
                Speichern = False
            End Try
        End If
        Speichern = True
    End Function
#End Region
End Class

