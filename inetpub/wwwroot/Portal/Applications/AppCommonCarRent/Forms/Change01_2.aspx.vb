Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change01_2
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private mObjZulassung As Zulassung

#Region "Properties"

    Private Property Refferer() As String
        Get
            If Not Session.Item(Me.Request.Url.LocalPath & "Refferer") Is Nothing Then
                Return Session.Item(Me.Request.Url.LocalPath & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Session.Item(Me.Request.Url.LocalPath & "Refferer") = value
        End Set
    End Property

#End Region

    Private Sub Change01_2_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            m_User = GetUser(Me)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            lblError.Text = ""


            If mObjZulassung Is Nothing Then
                If Not Session("mObjZulassungSession") Is Nothing Then
                    mObjZulassung = CType(Session("mObjZulassungSession"), Zulassung)
                Else
                    Throw New Exception("benötigtes Session Objekt nicht vorhanden")
                End If
            End If


            If Not IsPostBack AndAlso Not scriptmanager1.IsInAsyncPostBack Then
                If Refferer Is Nothing Then
                    If Not Me.Request.UrlReferrer Is Nothing Then
                        Refferer = Me.Request.UrlReferrer.ToString
                    Else
                        Refferer = ""
                    End If
                End If

                GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                ucStyles.TitleText = lblHead.Text

                If Not mObjZulassung.Status = 0 Then
                    lblError.Text = mObjZulassung.Message
                    Exit Sub
                End If

                FillGrid(0)

            End If


        Catch ex As Exception
            lblError.Text = "Beim laden der Seite ist ein Fehler aufgetreten: " & ex.Message
        End Try


    End Sub

    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")

        Dim tmpDataView As New DataView(mObjZulassung.Fahrzeuge)
        tmpDataView.RowFilter = "Uebernommen='X'"

        If tmpDataView.Count = 0 Then
            gvFahrzeuge.Visible = False
            lblGvFahrzeugeNoData.Visible = True
        Else
            gvFahrzeuge.Visible = True
            lblGvFahrzeugeNoData.Visible = False

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


            gvFahrzeuge.PageIndex = intTempPageIndex

            gvFahrzeuge.DataSource = tmpDataView

            gvFahrzeuge.DataBind()


            For Each item As GridViewRow In gvFahrzeuge.Rows


                If Not item.FindControl("lnkFahrgestellnummer") Is Nothing Then
                    If Not m_User.Applications.Select("AppName = 'Report46'").Length = 0 Then
                        CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).NavigateUrl = "../../../Components/ComCommon/Finance/Report46.aspx?AppID=" & m_User.Applications.Select("AppName = 'Report46'")(0)("AppID").ToString & "&VIN=" & CType(item.FindControl("lnkFahrgestellnummer"), HyperLink).Text
                    End If
                End If

                Dim tmpLit As Literal = CType(item.FindControl("lvl2GridStart"), Literal)

                If item.RowState = DataControlRowState.Alternate Then
                    tmpLit.Text = "</td></tr><tr Class='GridTableAlternate'><td colspan='" & item.Cells.Count - 1 & "'>"
                Else
                    tmpLit.Text = "</td></tr><tr><td colspan='" & item.Cells.Count - 1 & "'>"
                End If

            Next
        End If

    End Sub

    Private Sub gvFahrzeuge_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvFahrzeuge.PageIndexChanging
        FillGrid(e.NewPageIndex)
    End Sub


    Public Sub gvFahrzeuge_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFahrzeuge.RowCommand
        If Not e.CommandName = "Sort" Then
            If e.CommandName = "loeschen" Then
                mObjZulassung.Fahrzeuge.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0)("Uebernommen") = ""
                mObjZulassung.Fahrzeuge.Select("EQUNR='" & e.CommandArgument.ToString & "'")(0)("Ausgewaehlt") = ""
            End If
        End If
        FillGrid(gvFahrzeuge.PageIndex)
    End Sub

    Private Sub gvFahrzeuge_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvFahrzeuge.Sorting
        FillGrid(gvFahrzeuge.PageIndex, e.SortExpression)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        showAnzahl()
    End Sub

    Private Sub showAnzahl()
        If Not mObjZulassung Is Nothing Then 'wenn session objekt nach zulassen gekillt
            lblAnzeigeAnzahlAusgewaehlt.Text = "Anzahl zuzulassender Fahrzeuge: " & mObjZulassung.Fahrzeuge.Select("Uebernommen='X'").Count
        End If

    End Sub

    Protected Sub lb_Zulassen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Zulassen.Click

        'KBA-Nummer aus Steuertabelle holen
        mObjZulassung.fillKbaNummer(Session("AppID").ToString, Me.Session.SessionID, Me)
        If Not mObjZulassung.Status = 0 Then
            lblError.Text = "Fehler: KBA-Nummer konnte nicht ermittelt werden: " & mObjZulassung.Message
            Exit Sub
        End If

        'Verwendungszweck aus Steuertabelle holen
        mObjZulassung.fillVerwendungszweck(Session("AppID").ToString, Me.Session.SessionID, Me)
        If Not mObjZulassung.Status = 0 Then
            lblError.Text = "Fehler: Verwendungszweck konnte nicht ermittelt werden: " & mObjZulassung.Message
            Exit Sub
        End If

        mObjZulassung.Zulassen(Session("AppID").ToString, Me.Session.SessionID, Me)
        If Not mObjZulassung.Status = 0 Then
            lblError.Text = mObjZulassung.Message
        Else
            FillGrid(gvFahrzeuge.PageIndex)
        End If

        For Each tmpRow As GridViewRow In gvFahrzeuge.Rows
            tmpRow.FindControl("lblStatus").Visible = True
            tmpRow.FindControl("imgbloesch").Visible = False
            tmpRow.FindControl("lblZulassungskennzeichen").Visible = True
        Next

        lb_Zulassen.Enabled = False
        lb_zurueck.Enabled = False
        'so ich muss das sessionObjekt der zulassung löschen, muss aber vorher noch für eine 
        'excel ausgabe die modifiziert tabelle in die session schreiben
        '-------------------------
        Dim tbltemp As DataTable
        tbltemp = mObjZulassung.Fahrzeuge.Copy
        'natürlich nur die aktuell zuzulassenden Fahrzeuge
        For Each tmpRow As DataRow In tbltemp.Select("Uebernommen<>'X'")
            tmpRow.Delete()
        Next
        tbltemp.AcceptChanges()
        Session.Add("ExcelTable", tbltemp)
        '-------------------------
        Session("mObjZulassungSession") = Nothing
    End Sub

    Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Protected Sub imgbExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbExcel.Click
        Try
            Dim control As New Control
            Dim tblTranslations As New DataTable()
            Dim tblTemp As New DataTable()
            Dim AppURL As String
            Dim col As TableCell
            Dim col2 As DataColumn
            Dim bVisibility As Integer
            Dim isInTranslation As Boolean
            Dim i As Integer
            Dim sColName As String = ""

            AppURL = Replace(Me.Request.Url.LocalPath, "/Portal", "..")
            tblTranslations = CType(Me.Session(AppURL), DataTable)

            If Not mObjZulassung Is Nothing Then
                tblTemp = mObjZulassung.Fahrzeuge.Copy
                'natürlich nur die aktuell zuzulassenden Fahrzeuge
                For Each tmpRow As DataRow In tblTemp.Select("Uebernommen<>'X'")
                    tmpRow.Delete()
                Next
                tblTemp.AcceptChanges()
            Else
                tblTemp = CType(Session("ExcelTable"), DataTable)
            End If


            For i = tblTemp.Columns.Count - 1 To 0 Step -1
                isInTranslation = False
                For Each col In gvFahrzeuge.HeaderRow.Cells

                    bVisibility = 0
                    col2 = tblTemp.Columns(i)

                    For Each control In col.Controls
                        If Not control.ID Is Nothing Then
                            If col2.ColumnName.ToUpper = control.ID.ToUpper OrElse col2.ColumnName.ToUpper = control.ID.ToUpper.Replace("COL_", "") Then
                                sColName = TranslateColLbtn(gvFahrzeuge, tblTranslations, control.ID.Replace("COL_", ""), bVisibility)
                                isInTranslation = True
                                If bVisibility = 0 Then
                                    tblTemp.Columns.Remove(col2)
                                ElseIf sColName.Length > 0 Then
                                    col2.ColumnName = sColName
                                End If
                            End If
                        End If

                    Next
                Next

                If Not isInTranslation Then
                    'wenn eine Spalte nicht übersetzt wurde, dann aus exceltabelle entfernen
                    'da die tabelle noch viele hilfsfelder enthält die beim excel nicht benötigt werden
                    'aber da es hier diverse ausnahmen gibt, da nicht alle spalten gleichnamiges HeaderElement haben, muss hier nochmal selektiert werden
                    If Not col2.ColumnName = "Status" AndAlso Not col2.ColumnName = "Zulassungskennzeichen" AndAlso Not col2.ColumnName = "Absender" AndAlso Not col2.ColumnName = "Standort" AndAlso Not col2.ColumnName = "AbwScheinSchilderTEXT" Then
                        tblTemp.Columns.Remove(col2)
                    End If
                End If
                tblTemp.AcceptChanges()

            Next
            Dim excelFactory As New DocumentGeneration.ExcelDocumentFactory()
            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            excelFactory.CreateDocumentAndSendAsResponse(strFileName, tblTemp, Me.Page)
        Catch ex As Exception
            lblError.Text = "Beim erstellen des Exceldatei ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

End Class
' ************************************************
' $History: Change01_2.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:53
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' Warnungen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 12.02.09   Time: 10:51
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 11.02.09   Time: 17:31
' Updated in $/CKAG/Applications/AppCommonCarRent/Forms
' ita 2537
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 10.02.09   Time: 17:32
' Created in $/CKAG/Applications/AppCommonCarRent/Forms
' ITA 2537
' 
' ************************************************