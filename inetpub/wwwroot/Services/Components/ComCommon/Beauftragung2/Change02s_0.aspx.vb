Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security

Namespace Beauftragung2

    Public Class Change02s_0
        Inherits Page

#Region "Declarations"

        Private m_App As App
        Private m_User As User
        Private mBeauftragung As Beauftragung2

#End Region

#Region "Events"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                m_User = GetUser(Me)
                FormAuth(Me, m_User)

                GetAppIDFromQueryString(Me)
                m_App = New App(m_User)

                GridNavigation1.setGridElment(GridView1)

                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                lblError.Text = ""

                If Session("mBeauftragung2") IsNot Nothing Then
                    mBeauftragung = CType(Session("mBeauftragung2"), Beauftragung2)
                Else
                    mBeauftragung = New Beauftragung2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    FillBeauftragung()
                    Session("mBeauftragung2") = mBeauftragung
                End If

                If Not IsPostBack Then
                    LadeVorgaenge()
                End If

            Catch ex As Exception
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Try
        End Sub

        Private Sub btnDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDummy.Click
            LadeVorgaenge()
        End Sub

        Protected Sub lbSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbSearch.Click
            LadeVorgaenge()
        End Sub

        Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
            Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub

        Protected Sub GridNavigation1_PageIndexChanged(pageindex As Integer) Handles GridNavigation1.PagerChanged
            FillGrid(pageindex, "")
        End Sub

        Protected Sub GridNavigation1_PageSizeChanged() Handles GridNavigation1.PageSizeChanged
            FillGrid(0, "")
        End Sub

        Protected Sub GridView1_Sorting(sender As Object, e As GridViewSortEventArgs) Handles GridView1.Sorting
            FillGrid(GridView1.PageIndex, e.SortExpression)
        End Sub

        Private Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridView1.RowCommand
            Select Case e.CommandName

                Case "Bearbeiten"
                    mBeauftragung.LoadAutohausVorgangDetails(e.CommandArgument, Me.Page)
                    If mBeauftragung.Status = 0 Then
                        Session("mBeauftragung2") = mBeauftragung
                        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Components/ComCommon/Beauftragung2/Change02s.aspx?AppID=" & Session("AppID").ToString)
                    Else
                        lblError.Text = "Fehler beim Laden des Vorgangs (" & mBeauftragung.Message & ")"
                    End If

                Case "Zuruecksetzen"
                    mBeauftragung.ResetAutohausVorgang(e.CommandArgument, Me.Page)
                    If mBeauftragung.Status = 0 Then
                        LadeVorgaenge()
                    Else
                        lblError.Text = "Fehler beim Zurücksetzen des Vorgangs (" & mBeauftragung.Message & ")"
                    End If

            End Select
        End Sub

#End Region

#Region "Methods"

        Private Sub FillBeauftragung()
            With mBeauftragung
                .Gruppe = m_User.Groups(0).GroupName
                .Verkaufsbuero = Right(m_User.Reference, 4)
                .Verkaufsorganisation = Left(m_User.Reference, 4)

                'Stammdaten laden
                .Fill(Session("AppID").ToString, Session.SessionID.ToString, Me)
                .FillFarben(Me)
            End With
        End Sub

        Private Sub LadeVorgaenge()
            mBeauftragung.LoadAutohausVorgangListe(txtID.Text, Me.Page)

            If mBeauftragung.Status = 0 Then
                FillGrid(0, "")
            Else
                lblError.Text = mBeauftragung.Message
            End If

            Session("mBeauftragung2") = mBeauftragung
        End Sub

        Private Sub FillGrid(intPageIndex As Integer, strSort As String)

            If mBeauftragung.AutohausVorgangListe Is Nothing OrElse mBeauftragung.AutohausVorgangListe.Rows.Count = 0 Then
                lblError.Text = "Keine Daten zur Anzeige gefunden."

                Result.Visible = False

            Else
                Dim tmpDataview = mBeauftragung.AutohausVorgangListe.DefaultView
                tmpDataview.Sort = "ZZZLDAT DESC"

                Result.Visible = True

                Dim strTempSort As String = ""
                Dim strDirection As String = ""
                Dim intTempPageIndex As Integer = intPageIndex

                If strSort.Trim().Length > 0 Then
                    strTempSort = strSort.Trim()

                    If (ViewState("Sort") Is Nothing) OrElse (DirectCast(ViewState("Sort"), String) = strTempSort) Then
                        If ViewState("Direction") Is Nothing Then
                            strDirection = "desc"
                        Else
                            strDirection = DirectCast(ViewState("Direction"), String)
                        End If
                    Else
                        strDirection = "desc"
                    End If

                    If (strDirection = "asc") Then
                        strDirection = "desc"
                    Else
                        strDirection = "asc"
                    End If

                    ViewState("Sort") = strTempSort
                    ViewState("Direction") = strDirection
                Else
                    If ViewState("Sort") IsNot Nothing Then
                        strTempSort = DirectCast(ViewState("Sort"), String)

                        If ViewState("Direction") Is Nothing Then
                            strDirection = "asc"
                            ViewState("Direction") = strDirection
                        Else
                            strDirection = DirectCast(ViewState("Direction"), String)
                        End If
                    End If
                End If

                If strTempSort.Length <> 0 Then
                    tmpDataview.Sort = strTempSort & " " & strDirection
                End If

                GridView1.PageIndex = intTempPageIndex
                GridView1.DataSource = tmpDataview
                GridView1.DataBind()

            End If
        End Sub

#End Region

    End Class

End Namespace