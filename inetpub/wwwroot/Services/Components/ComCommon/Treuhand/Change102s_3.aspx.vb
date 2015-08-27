Option Strict On
Option Explicit On

Imports CKG.Base.Kernel.Common
Imports System.Globalization
Imports Telerik.Web.UI
Imports Telerik.Web.UI.GridExcelBuilder

Namespace Treuhand

    Partial Public Class Change102s_3
        Inherits System.Web.UI.Page

#Region "Declarations"
        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private CustomerObject As SperreFreigabe
        Private isExcelExportConfigured As Boolean
#End Region

#Region "PageEvents"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            m_User = Common.GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            Common.FormAuth(Me, m_User)

            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(Me) 'füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            lblError.Text = ""
            lblMessage.Text = ""

            CustomerObject = CType(Session("SperrObject"), SperreFreigabe)

            If Not IsPostBack Then
                Common.TranslateTelerikColumns(rgGrid1)
                CheckUpload()
                FillGrid()
            End If

        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            Common.SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            Common.SetEndASPXAccess(Me)
        End Sub

#End Region

#Region "Methods"

        Private Sub FillGrid()

            If CustomerObject Is Nothing OrElse CustomerObject.tblUpload.DefaultView.Count = 0 Then
                SearchMode()
                lblError.Text = "Keine Daten zur Anzeige gefunden."
            Else
                lblMessage.Text = "Bitte wählen Sie die Vorgänge ab, die Sie nicht absenden wollen."
                SearchMode(False)

                If CustomerObject.tblUpload.Select("Message <> ''").Length > 0 Then
                    rgGrid1.Columns.FindByUniqueName("ERROR").Visible = True
                End If

                rgGrid1.Rebind()
                'Setzen der DataSource geschieht durch das NeedDataSource-Event
            End If

        End Sub

        Private Sub FillGrid2()

            If CustomerObject.Fahrzeuge.DefaultView.Count = 0 Then
                SearchMode()
                lblError.Text = "Keine Daten zur Anzeige gefunden."
            Else
                SearchMode(False)

                rgGrid1.Columns.FindByUniqueName("Loeschen").Visible = False
                rgGrid1.Columns.FindByUniqueName("SPERRDAT").Visible = False
                rgGrid1.Columns.FindByUniqueName("TREUH_VGA").Visible = True

                rgGrid1.Rebind()
                'Setzen der DataSource geschieht durch das NeedDataSource-Event
            End If

        End Sub

        Private Sub SearchMode(Optional search As Boolean = True)
            Result.Visible = Not search
            dataQueryFooter.Visible = Not search
        End Sub

        Private Function CheckUpload() As Boolean

            Dim isWarning As Boolean = False
            Dim isError As Boolean = False
            Dim sperrPruef As SperrPruefung

            Try
                sperrPruef = New SperrPruefung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                sperrPruef.Treuhandgeber = CustomerObject.TREU
                sperrPruef.Auftraggeber = CustomerObject.AG
                sperrPruef.IsSperren = CustomerObject.IsSperren

                'Zurücksetzen der Formatierung für neue Prüfung
                rgGrid1.Columns.FindByUniqueName("ERROR").Visible = False

                'verhalten wenn nichts selektiert
                If CustomerObject.tblUpload.Rows.Count <= 0 Then
                    InfoHead.Text = "Information"
                    rgGrid1.Columns.FindByUniqueName("ERROR").Visible = False
                    InfoText.Text = "Keine Fahrzeuge vorhanden."
                    InfoText.ForeColor = Drawing.Color.Black
                    cmdSave.Visible = False
                    cmdCheck.Visible = True
                    Return False
                End If

                'Daten im Upload-Table auf korrekte Formatierung prüfen
                If Not CheckData() Then
                    isError = True
                End If

                'GESAMTE TABELLE gegen Bapi Prüfen
                Dim pruefErg As SperrPruefung.UploadTableStatus = sperrPruef.CheckAllTable(Me, CustomerObject.tblUpload)

                If Not isError Then
                    If pruefErg = SperrPruefung.UploadTableStatus.Fehler Then
                        isError = True
                    Else If pruefErg = SperrPruefung.UploadTableStatus.Warnung Then
                        isWarning = True
                    End If
                End If

                rgGrid1.MasterTableView.SortExpressions.Clear()

                'Setzen der Information je nach Fehlertyp 
                If isError Then
                    rgGrid1.Columns.FindByUniqueName("ERROR").Visible = True
                    InfoHead.Text = "Achtung!"
                    InfoText.ForeColor = Drawing.Color.Red
                    InfoText.Text = "Der Upload enthält Fehler"
                    'Grid nach Fehler und ID sortieren
                    rgGrid1.MasterTableView.SortExpressions.AddSortExpression("ERROR DESC")
                    rgGrid1.MasterTableView.SortExpressions.AddSortExpression("ID ASC")
                    cmdSave.Visible = False
                    Return False
                ElseIf isWarning Then
                    rgGrid1.Columns.FindByUniqueName("ERROR").Visible = True
                    InfoHead.Text = "Information"
                    InfoText.ForeColor = Drawing.Color.Black
                    InfoText.Text = "Der Upload enthält Warnungen"
                    'Grid nach Fehler und ID sortieren
                    rgGrid1.MasterTableView.SortExpressions.AddSortExpression("ERROR DESC")
                    rgGrid1.MasterTableView.SortExpressions.AddSortExpression("ID ASC")
                    cmdSave.Visible = True
                    Return True
                Else
                    rgGrid1.Columns.FindByUniqueName("ERROR").Visible = False
                    InfoHead.Text = "Information"
                    InfoText.ForeColor = Drawing.Color.DarkGreen
                    InfoText.Text = "Prüfung erfolgreich!"
                    'Grid nach ID sortieren
                    rgGrid1.MasterTableView.SortExpressions.AddSortExpression("ID ASC")
                    cmdSave.Visible = True
                    Return True
                End If

            Catch ex As Exception
                lblError.Text = ex.Message
                Return False
            End Try

        End Function

        Private Sub SetGridError(ByVal ctrl As WebControl, ByVal fehler As Boolean, Optional ByVal message As String = "")
            If fehler Then
                ctrl.BorderColor = Drawing.Color.Red
                If String.IsNullOrEmpty(message) Then
                    ctrl.ToolTip = "Falsches Format"
                Else
                    ctrl.ToolTip = message
                End If
            Else
                ctrl.BorderColor = Drawing.Color.Empty
                ctrl.ToolTip = ""
            End If
        End Sub

        ''' <summary>
        ''' Aktualisieren der Daten im DataTable (nur aktuelle GridPage!)
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SaveGridPageData()

            For Each item As GridDataItem In rgGrid1.Items
                Dim tmpRows As DataRow() = CustomerObject.tblUpload.Select("ID = '" & item("ID").Text & "'")
                If (tmpRows.Length > 0) Then
                    tmpRows(0)("ZZREFERENZ2") = CType(item.FindControl("txtZZREFERENZ2"), TextBox).Text
                    tmpRows(0)("EQUI_KEY") = CType(item.FindControl("txtEQUI_KEY"), TextBox).Text
                    'Sperrdatum soll pauschal auf aktuelles Tagesdatum gesetzt werden, unabhängig von der Benutzereingabe
                    tmpRows(0)("SPERRDAT") = DateTime.Today.ToString("dd.MM.yyyy")
                    tmpRows(0)("VERTR_BEGINN") = CType(item.FindControl("txtVERTR_BEGINN"), TextBox).Text
                    tmpRows(0)("VERTR_ENDE") = CType(item.FindControl("txtVERTR_ENDE"), TextBox).Text
                    tmpRows(0)("Versanddatum") = CType(item.FindControl("txtVersanddatum"), TextBox).Text
                    tmpRows(0)("Haendlernummer") = CType(item.FindControl("txtHaendlernummer"), TextBox).Text
                    tmpRows(0)("Haendlername") = CType(item.FindControl("txtHaendlername"), TextBox).Text
                    tmpRows(0)("Name2") = CType(item.FindControl("txtName2"), TextBox).Text
                    tmpRows(0)("Strasse") = CType(item.FindControl("txtStrasse"), TextBox).Text
                    tmpRows(0)("PLZ") = CType(item.FindControl("txtPLZ"), TextBox).Text
                    tmpRows(0)("Ort") = CType(item.FindControl("txtOrt"), TextBox).Text
                    tmpRows(0)("Postfach") = CType(item.FindControl("txtPostfach"), TextBox).Text
                    tmpRows(0)("SELECT") = "99" 'immer selektiert, da keine Selektion per Checkbox mehr
                End If
            Next

            CustomerObject.tblUpload.AcceptChanges()
            Session("CustomerObject") = CustomerObject

        End Sub

        ''' <summary>
        ''' Prüfung auf Vollständigkeit und Format der Input-Daten
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckData() As Boolean
            Dim dummyDate As DateTime

            For Each dRow As DataRow In CustomerObject.tblUpload.Rows

                Dim strRef As String = ""
                Dim strEqui As String = ""
                Dim strVertragsbeginn As String = ""
                Dim strVertragsende As String = ""
                Dim strVersanddatum As String = ""
                Dim blnVersanddatumGesetzt As Boolean = False
                Dim strHaendlernummer As String = ""
                Dim strHaendlername As String = ""
                Dim strName2 As String = ""
                Dim strStrasse As String = ""
                Dim strPLZ As String = ""
                Dim strOrt As String = ""
                Dim strPostfach As String = ""

                'ACHTUNG: Reihenfolge der Prüfungen ist wichtig!

                'Vertragsnummer (Optional)
                strRef = dRow("ZZREFERENZ2").ToString()
                If Not String.IsNullOrEmpty(strRef) AndAlso (strRef.Trim().Length > 12 Or Not IsNumeric(strRef.Trim())) Then
                    Return False
                End If

                'Fahrgestellnummer (Pflicht)
                strEqui = dRow("EQUI_KEY").ToString()
                If String.IsNullOrEmpty(strEqui) OrElse strEqui.Trim().Length > 17 Then
                    Return False
                End If

                'Vertragsbeginn (Optional)
                strVertragsbeginn = dRow("VERTR_BEGINN").ToString()
                If Not String.IsNullOrEmpty(strVertragsbeginn) AndAlso _
                        (strVertragsbeginn.Trim().Length > 10 Or Not DateTime.TryParseExact(strVertragsbeginn.Trim(), "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, dummyDate)) Then
                    Return False
                End If

                'Vertragsende (Optional)
                strVertragsende = dRow("VERTR_ENDE").ToString()
                If Not String.IsNullOrEmpty(strVertragsende) AndAlso _
                        (strVertragsende.Trim().Length > 10 Or Not DateTime.TryParseExact(strVertragsende.Trim(), "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, dummyDate)) Then
                    Return False
                End If

                'Versanddatum (Optional)
                strVersanddatum = dRow("Versanddatum").ToString()
                If Not String.IsNullOrEmpty(strVersanddatum) AndAlso _
                        (strVersanddatum.Trim().Length > 10 Or Not DateTime.TryParseExact(strVersanddatum.Trim(), "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, dummyDate)) Then
                    Return False
                End If

                blnVersanddatumGesetzt = (Not String.IsNullOrEmpty(strVersanddatum))

                'Händlernummer (Optional)
                strHaendlernummer = dRow("Haendlernummer").ToString()
                If Not String.IsNullOrEmpty(strHaendlernummer) AndAlso (strHaendlernummer.Trim().Length > 40 Or Not IsNumeric(strHaendlernummer.Trim())) Then
                    Return False
                End If

                'Händlername (Pflicht, wenn Versanddatum erfasst)
                strHaendlername = dRow("Haendlername").ToString()
                If blnVersanddatumGesetzt Then
                    If String.IsNullOrEmpty(strHaendlername) OrElse strHaendlername.Trim().Length > 40 Then
                        Return False
                    End If
                Else
                    If Not String.IsNullOrEmpty(strHaendlername) AndAlso strHaendlername.Trim().Length > 40 Then
                        Return False
                    End If
                End If

                'Adresszusatz / Ansprechpartner (Optional)
                strName2 = dRow("Name2").ToString()
                If Not String.IsNullOrEmpty(strName2) AndAlso strName2.Trim().Length > 40 Then
                    Return False
                End If

                'Postfach (Strasse oder Postfach Pflicht, wenn Versanddatum erfasst -> Prüfung s.u.)
                strPostfach = dRow("Postfach").ToString()
                If Not String.IsNullOrEmpty(strPostfach) AndAlso strPostfach.Trim().Length > 40 Then
                    Return False
                End If

                'Strasse + Hausnummer (Strasse oder Postfach Pflicht, wenn Versanddatum erfasst)
                strStrasse = dRow("Strasse").ToString()
                If blnVersanddatumGesetzt Then
                    If (String.IsNullOrEmpty(strStrasse) AndAlso String.IsNullOrEmpty(strPostfach)) OrElse strStrasse.Trim().Length > 40 Then
                        Return False
                    End If
                Else
                    If Not String.IsNullOrEmpty(strStrasse) AndAlso strStrasse.Trim().Length > 40 Then
                        Return False
                    End If
                End If

                'PLZ (Pflicht, wenn Versanddatum erfasst)
                strPLZ = dRow("PLZ").ToString()
                If blnVersanddatumGesetzt Then
                    If String.IsNullOrEmpty(strPLZ) OrElse strPLZ.Trim().Length > 5 Or Not IsNumeric(strPLZ.Trim()) Then
                        Return False
                    End If
                Else
                    If Not String.IsNullOrEmpty(strPLZ) AndAlso (strPLZ.Trim().Length > 5 Or Not IsNumeric(strPLZ.Trim())) Then
                        Return False
                    End If
                End If

                'Ort (Pflicht, wenn Versanddatum erfasst)
                strOrt = dRow("Ort").ToString()
                If blnVersanddatumGesetzt Then
                    If String.IsNullOrEmpty(strOrt) OrElse strOrt.Trim().Length > 40 Then
                        Return False
                    End If
                Else
                    If Not String.IsNullOrEmpty(strOrt) AndAlso strOrt.Trim().Length > 40 Then
                        Return False
                    End If
                End If

            Next

            Return True

        End Function

        ''' <summary>
        ''' GridItem prüfen und Zellen ggf. entspr. formatieren
        ''' </summary>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Private Sub CheckDataItem(ByRef item As GridDataItem)

            Dim txtBox As TextBox
            Dim dummyDate As DateTime
            Dim strVersanddatum As String = ""
            Dim blnVersanddatumGesetzt As Boolean = False
            Dim strPostfach As String = ""

            'ACHTUNG: Reihenfolge der Prüfungen ist wichtig!

            'Vertragsnummer (Optional)
            txtBox = CType(item.FindControl("txtZZREFERENZ2"), TextBox)
            If String.IsNullOrEmpty(txtBox.Text) OrElse (txtBox.Text.Trim().Length <= 12 AndAlso IsNumeric(txtBox.Text.Trim())) Then
                SetGridError(txtBox, False)
            Else
                SetGridError(txtBox, True, "Falsches Format (nummerisch, max. 12 Stellen)")
            End If

            'Fahrgestellnummer (Pflicht)
            txtBox = CType(item.FindControl("txtEQUI_KEY"), TextBox)
            If Not String.IsNullOrEmpty(txtBox.Text) AndAlso txtBox.Text.Trim().Length <= 17 Then
                SetGridError(txtBox, False)
            Else
                SetGridError(txtBox, True, "Pflichtfeld (max. 17 Stellen)")
            End If

            'Vertragsbeginn (Optional)
            txtBox = CType(item.FindControl("txtVERTR_BEGINN"), TextBox)
            If String.IsNullOrEmpty(txtBox.Text) OrElse _
                    (txtBox.Text.Trim().Length <= 10 AndAlso DateTime.TryParseExact(txtBox.Text.Trim(), "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, dummyDate)) Then
                SetGridError(txtBox, False)
            Else
                SetGridError(txtBox, True, "Falsches Format (TT.MM.JJJJ)")
            End If

            'Vertragsende (Optional)
            txtBox = CType(item.FindControl("txtVERTR_ENDE"), TextBox)
            If String.IsNullOrEmpty(txtBox.Text) OrElse _
                    (txtBox.Text.Trim().Length <= 10 AndAlso DateTime.TryParseExact(txtBox.Text.Trim(), "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, dummyDate)) Then
                SetGridError(txtBox, False)
            Else
                SetGridError(txtBox, True, "Falsches Format (TT.MM.JJJJ)")
            End If

            'Versanddatum (Optional)
            txtBox = CType(item.FindControl("txtVersanddatum"), TextBox)
            If String.IsNullOrEmpty(txtBox.Text) OrElse _
                    (txtBox.Text.Trim().Length <= 10 AndAlso DateTime.TryParseExact(txtBox.Text.Trim(), "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, dummyDate)) Then
                strVersanddatum = txtBox.Text.Trim()
                SetGridError(txtBox, False)
            Else
                SetGridError(txtBox, True, "Falsches Format (TT.MM.JJJJ)")
            End If

            blnVersanddatumGesetzt = (Not String.IsNullOrEmpty(strVersanddatum))

            'Händlernummer (Optional)
            txtBox = CType(item.FindControl("txtHaendlernummer"), TextBox)
            If String.IsNullOrEmpty(txtBox.Text) OrElse (txtBox.Text.Trim().Length <= 40 AndAlso IsNumeric(txtBox.Text.Trim())) Then
                SetGridError(txtBox, False)
            Else
                SetGridError(txtBox, True, "Falsches Format (nummerisch, max. 40 Stellen)")
            End If

            'Händlername (Pflicht, wenn Versanddatum erfasst)
            txtBox = CType(item.FindControl("txtHaendlername"), TextBox)
            If blnVersanddatumGesetzt Then
                If Not String.IsNullOrEmpty(txtBox.Text) AndAlso txtBox.Text.Trim().Length <= 40 Then
                    SetGridError(txtBox, False)
                Else
                    SetGridError(txtBox, True, "Pflichtfeld (max. 40 Stellen)")
                End If
            Else
                If String.IsNullOrEmpty(txtBox.Text) OrElse txtBox.Text.Trim().Length <= 40 Then
                    SetGridError(txtBox, False)
                Else
                    SetGridError(txtBox, True, "Falsches Format (max. 40 Stellen)")
                End If
            End If

            'Adresszusatz / Ansprechpartner (Optional)
            txtBox = CType(item.FindControl("txtName2"), TextBox)
            If String.IsNullOrEmpty(txtBox.Text) OrElse txtBox.Text.Trim().Length <= 40 Then
                SetGridError(txtBox, False)
            Else
                SetGridError(txtBox, True, "Falsches Format (max. 40 Stellen)")
            End If

            'Postfach (Strasse oder Postfach Pflicht, wenn Versanddatum erfasst -> Prüfung s.u.)
            txtBox = CType(item.FindControl("txtPostfach"), TextBox)
            If String.IsNullOrEmpty(txtBox.Text) OrElse txtBox.Text.Trim().Length <= 40 Then
                strPostfach = txtBox.Text.Trim()
                SetGridError(txtBox, False)
            Else
                SetGridError(txtBox, True, "Falsches Format (max. 40 Stellen)")
            End If

            'Strasse + Hausnummer (Strasse oder Postfach Pflicht, wenn Versanddatum erfasst)
            txtBox = CType(item.FindControl("txtStrasse"), TextBox)
            If blnVersanddatumGesetzt Then
                If (Not String.IsNullOrEmpty(txtBox.Text) OrElse Not String.IsNullOrEmpty(strPostfach)) AndAlso txtBox.Text.Trim().Length <= 40 Then
                    SetGridError(txtBox, False)
                Else
                    SetGridError(txtBox, True, "Pflichtfeld (max. 40 Stellen)")
                End If
            Else
                If String.IsNullOrEmpty(txtBox.Text) OrElse txtBox.Text.Trim().Length <= 40 Then
                    SetGridError(txtBox, False)
                Else
                    SetGridError(txtBox, True, "Falsches Format (max. 40 Stellen)")
                End If
            End If

            'PLZ (Pflicht, wenn Versanddatum erfasst)
            txtBox = CType(item.FindControl("txtPLZ"), TextBox)
            If blnVersanddatumGesetzt Then
                If Not String.IsNullOrEmpty(txtBox.Text) AndAlso txtBox.Text.Trim().Length <= 5 AndAlso IsNumeric(txtBox.Text.Trim()) Then
                    SetGridError(txtBox, False)
                Else
                    SetGridError(txtBox, True, "Pflichtfeld (nummerisch, max. 5 Stellen)")
                End If
            Else
                If String.IsNullOrEmpty(txtBox.Text) OrElse (txtBox.Text.Trim().Length <= 5 AndAlso IsNumeric(txtBox.Text.Trim())) Then
                    SetGridError(txtBox, False)
                Else
                    SetGridError(txtBox, True, "Falsches Format (nummerisch, max. 5 Stellen)")
                End If
            End If

            'Ort (Pflicht, wenn Versanddatum erfasst)
            txtBox = CType(item.FindControl("txtOrt"), TextBox)
            If blnVersanddatumGesetzt Then
                If Not String.IsNullOrEmpty(txtBox.Text) AndAlso txtBox.Text.Trim().Length <= 40 Then
                    SetGridError(txtBox, False)
                Else
                    SetGridError(txtBox, True, "Pflichtfeld (max. 40 Stellen)")
                End If
            Else
                If String.IsNullOrEmpty(txtBox.Text) OrElse txtBox.Text.Trim().Length <= 40 Then
                    SetGridError(txtBox, False)
                Else
                    SetGridError(txtBox, True, "Falsches Format (max. 40 Stellen)")
                End If
            End If

        End Sub

#End Region

#Region "Events"

        Protected Sub rgGrid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
            If CustomerObject.tblUpload IsNot Nothing Then
                rgGrid1.DataSource = CustomerObject.tblUpload.DefaultView
            Else
                rgGrid1.DataSource = Nothing
            End If
        End Sub

        Protected Sub rgGrid1_ItemDataBound(sender As Object, e As GridItemEventArgs)

            If TypeOf (e.Item) Is GridDataItem Then
                Dim item As GridDataItem = CType(e.Item, GridDataItem)

                'Standardstyle setzen
                item.Style.Item("background-color") = "transparent !Important;"

                Dim txtEqui As TextBox = CType(item.FindControl("txtEQUI_KEY"), TextBox)
                Dim equikey As String = txtEqui.Text

                Dim rows As DataRow() = CustomerObject.tblUpload.Select("EQUI_KEY='" & equikey & "'")

                If rows IsNot Nothing AndAlso rows.Length > 0 Then

                    Dim errText As String = rows(0)("ERROR").ToString()

                    If Not String.IsNullOrEmpty(errText) Then
                        If errText.StartsWith("FEHLER: ") Then
                            item("ERROR").Text = errText.Replace("FEHLER: ", "")
                            item.Style.Item("background-color") = "#F08080 !Important;"
                            item.BorderStyle = BorderStyle.Solid
                            item.BorderWidth = 1
                            item.BorderColor = Drawing.Color.LightGray

                        ElseIf errText.StartsWith("WARNUNG: ") Then
                            item("ERROR").Text = errText.Replace("WARNUNG: ", "")
                            item.Style.Item("background-color") = "#FFFFE0 !Important;"
                            item.BorderStyle = BorderStyle.Solid
                            item.BorderWidth = 1
                            item.BorderColor = Drawing.Color.LightGray

                        End If
                    End If

                End If

                'Prüfung der Daten und entspr. Formatierung der Zellen
                CheckDataItem(item)

            End If

        End Sub

        Protected Sub cmdCheck_Click(sender As Object, e As EventArgs) Handles cmdCheck.Click
            SaveGridPageData()
            CheckUpload()
            rgGrid1.Rebind()
        End Sub

        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

            Dim iSelCount As Integer = CustomerObject.tblUpload.Rows.Count

            If iSelCount = 0 Then
                lblError.Text = "Keine Fahrzeuge vorhanden"
                FillGrid()

            Else
                'Bisher vorgenommene Änderungen sichern und prüfen
                SaveGridPageData()
                CheckData()

                CustomerObject.GiveCars(Me.Page, Session("AppID").ToString, Session.SessionID.ToString, True)
                CustomerObject.SaveAddressData(Me.Page, Session("AppID").ToString, Session.SessionID.ToString)

                Dim ErrCount As Integer = 0
                Dim RefCount As Integer = 0
                Dim rows As DataRow() = CustomerObject.Fahrzeuge.Select("SUBRC <> 0")
                ErrCount = rows.Length
                For Each row As DataRow In rows
                    CustomerObject.tblUpload.Select("EQUI_KEY = '" & row("EQUI_KEY").ToString() & "'")(0)("MESSAGE") = row("MESSAGE").ToString()

                    If row("MESSAGE").ToString.StartsWith("W") Then
                        ErrCount -= 1
                    ElseIf row("MESSAGE").ToString.StartsWith("I") Then
                        ErrCount -= 1
                        RefCount += 1
                    End If
                Next

                Session("CustomerObject") = CustomerObject

                FillGrid2()

                cmdSave.Visible = False
                cmdCheck.Visible = False
                pnInfo.Visible = False

                lblError.Text = "Es wurden " & iSelCount - ErrCount & " Fahrzeuge erfolgreich " & CustomerObject.SperrEnsperr & "!"
                If ErrCount > 0 Then
                    lblError.Text &= "<br /> " & ErrCount & " Fahrzeuge mit Fehler!"
                End If
                If RefCount > 0 Then
                    lblError.Text &= "<br /> " & RefCount & " Referenznummern aktualisiert!"
                End If

                rgGrid1.Columns.FindByUniqueName("ERROR").Visible = False

            End If

        End Sub

        Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
            Response.Redirect("Change102s.aspx?AppID=" & Session("AppID").ToString)
        End Sub

        Protected Sub rgGrid1_ItemCommand(sender As Object, e As GridCommandEventArgs)

            'Bisher vorgenommene Änderungen sichern (Event wird auch bei Seitenwechsel oder PageSize-Änderung ausgelöst)
            SaveGridPageData()

            Select Case e.CommandName

                Case RadGrid.ExportToExcelCommandName
                    'Spalten ausblenden, die nicht exportiert werden sollen
                    rgGrid1.MasterTableView.GetColumn("ID").Visible = False
                    rgGrid1.MasterTableView.GetColumn("Loeschen").Visible = False
                    rgGrid1.MasterTableView.GetColumn("ERNAM").Visible = False
                    rgGrid1.MasterTableView.GetColumn("ERDAT").Visible = False
                    rgGrid1.MasterTableView.GetColumn("TREUH_VGA").Visible = False
                    rgGrid1.MasterTableView.GetColumn("MESSAGE").Visible = False
                    rgGrid1.MasterTableView.GetColumn("ERROR").Visible = False

                    Dim eSettings As GridExportSettings = rgGrid1.ExportSettings
                    eSettings.ExportOnlyData = True
                    eSettings.FileName = String.Format("TreuhandServices_VWL_{0:yyyyMMdd}", DateTime.Now)
                    eSettings.HideStructureColumns = True
                    eSettings.IgnorePaging = True
                    eSettings.OpenInNewWindow = True
                    ' hide non display columns from excel export
                    For Each col As GridColumn In rgGrid1.MasterTableView.Columns
                        If TypeOf col Is GridEditableColumn AndAlso Not col.Display Then
                            col.Visible = False
                        End If
                    Next
                    rgGrid1.Rebind()
                    rgGrid1.MasterTableView.ExportToExcel()

                Case "Del"
                    Dim item As GridDataItem = CType(e.Item, GridDataItem)
                    Dim rows As DataRow() = CustomerObject.tblUpload.Select("ID = '" & item("ID").Text & "'")
                    If rows.Length > 0 Then
                        CustomerObject.tblUpload.Rows.Remove(rows(0))
                        CustomerObject.tblUpload.AcceptChanges()
                        Session("CustomerObject") = CustomerObject
                        rgGrid1.Rebind()
                    End If
            End Select
        End Sub

        Protected Sub rgGrid1_ExcelMLExportRowCreated(sender As Object, e As GridExportExcelMLRowCreatedArgs)

            If e.RowType = GridExportExcelMLRowType.DataRow Then

                If Not isExcelExportConfigured Then
                    'ACHTUNG: Der Name "Tabelle1" ist wichtig, da beim Wiedereinlesen der Datei beim Upload darauf abgefragt wird
                    e.Worksheet.Name = "Tabelle1"

                    'Set Page options
                    Dim layout As PageLayoutElement = e.Worksheet.WorksheetOptions.PageSetup.PageLayoutElement
                    layout.IsCenteredVertical = True
                    layout.IsCenteredHorizontal = True
                    layout.PageOrientation = PageOrientationType.Landscape
                    Dim margins As PageMarginsElement = e.Worksheet.WorksheetOptions.PageSetup.PageMarginsElement
                    margins.Left = 0.5
                    margins.Top = 0.5
                    margins.Right = 0.5
                    margins.Bottom = 0.5

                    'Freeze panes
                    Dim wso As WorksheetOptionsElement = e.Worksheet.WorksheetOptions
                    wso.AllowFreezePanes = True
                    wso.LeftColumnRightPaneNumber = 1
                    wso.TopRowBottomPaneNumber = 1
                    wso.SplitHorizontalOffset = 1
                    wso.SplitVerticalOffest = 1
                    wso.ActivePane = 2

                    isExcelExportConfigured = True
                End If

            End If

        End Sub

        Protected Sub rgGrid1_ExcelMLExportStylesCreated(sender As Object, e As GridExportExcelMLStyleCreatedArgs)

            'Add currency and percent styles
            Dim priceStyle As New StyleElement("priceItemStyle")
            priceStyle.NumberFormat.FormatType = NumberFormatType.Currency
            priceStyle.FontStyle.Color = System.Drawing.Color.Red
            e.Styles.Add(priceStyle)

            Dim alternatingPriceStyle As New StyleElement("alternatingPriceItemStyle")
            alternatingPriceStyle.NumberFormat.FormatType = NumberFormatType.Currency
            alternatingPriceStyle.FontStyle.Color = System.Drawing.Color.Red
            e.Styles.Add(alternatingPriceStyle)

            Dim percentStyle As New StyleElement("percentItemStyle")
            percentStyle.NumberFormat.FormatType = NumberFormatType.Percent
            percentStyle.FontStyle.Italic = True
            e.Styles.Add(percentStyle)

            Dim alternatingPercentStyle As New StyleElement("alternatingPercentItemStyle")
            alternatingPercentStyle.NumberFormat.FormatType = NumberFormatType.Percent
            alternatingPercentStyle.FontStyle.Italic = True
            e.Styles.Add(alternatingPercentStyle)

            'Apply background colors 
            For Each style As StyleElement In e.Styles
                If style.Id = "headerStyle" Then
                    style.InteriorStyle.Pattern = InteriorPatternType.Solid
                    style.InteriorStyle.Color = System.Drawing.Color.Gray
                End If
                If style.Id = "alternatingItemStyle" Or style.Id = "alternatingPriceItemStyle" Or style.Id = "alternatingPercentItemStyle" Or style.Id = "alternatingDateItemStyle" Then
                    style.InteriorStyle.Pattern = InteriorPatternType.Solid
                    style.InteriorStyle.Color = System.Drawing.Color.LightGray
                End If
                If style.Id.Contains("itemStyle") Or style.Id = "priceItemStyle" Or style.Id = "percentItemStyle" Or style.Id = "dateItemStyle" Then
                    style.InteriorStyle.Pattern = InteriorPatternType.Solid
                    style.InteriorStyle.Color = System.Drawing.Color.White
                End If
            Next

        End Sub

#End Region

    End Class

End Namespace