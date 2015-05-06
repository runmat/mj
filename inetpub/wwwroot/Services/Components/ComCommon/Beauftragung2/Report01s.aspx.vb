Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel

Namespace Beauftragung2

    Partial Public Class Report01s
        Inherits Page

#Region "Declarations"

        Private m_App As App
        Private m_User As User
        Private mBeauftragung As Beauftragung2

#End Region

#Region "Events"

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
            Try
                m_User = GetUser(Me)
                FormAuth(Me, m_User)

                GetAppIDFromQueryString(Me)
                m_App = New App(m_User)

                lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
                lblError.Text = ""

                If Session("mBeauftragung2") IsNot Nothing Then
                    mBeauftragung = CType(Session("mBeauftragung2"), Beauftragung2)
                Else
                    mBeauftragung = New Beauftragung2(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                    FillBeauftragung()
                    Session("mBeauftragung2") = mBeauftragung
                End If

                InitLargeDropdowns()
                InitJava()

            Catch ex As Exception
                lblError.Text = "Beim Initialisieren der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Try
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                InitControls()

            Catch ex As Exception
                lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Try
        End Sub

        Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
            DoSubmit()
        End Sub

        Private Sub btnDummy_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDummy.Click
            DoSubmit()
        End Sub

        Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
            Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

#End Region

#Region "Methods"

        Private Sub FillBeauftragung()
            With mBeauftragung
                .Gruppe = m_User.Groups(0).GroupName
                .Verkaufsbuero = Right(m_User.Reference, 4)
                .Verkaufsorganisation = Left(m_User.Reference, 4)

                'Stammdaten laden
                .Fill(Me)
            End With
        End Sub

        ''' <summary>
        ''' Dropdowns mit großen Datenmengen (ohne ViewState!)
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitLargeDropdowns()

            'Kunde füllen
            ddlKunde.Items.Clear()
            ddlKunde.Items.Add(New ListItem("- Keine Auswahl -", "0"))
            For Each dRow As DataRow In mBeauftragung.Kunden.Rows
                ddlKunde.Items.Add(New ListItem(dRow("NAME1").ToString(), dRow("KUNNR").ToString().TrimStart("0"c)))
            Next

        End Sub

        Private Sub InitJava()
            txtKunnr.Attributes.Add("onkeyup", "FilterItems(this.value," + ddlKunde.ClientID + ")")
            txtKunnr.Attributes.Add("onblur", "SetItemText(" + ddlKunde.ClientID + ",this)")
            ddlKunde.Attributes.Add("onchange", "SetItemText(" + ddlKunde.ClientID + "," + txtKunnr.ClientID + ")")
        End Sub

        Private Sub InitControls()

            txtKunnr.BorderColor = Drawing.Color.Empty
            lblKundeInfo.Text = ""

            txtZulDatumVon.BorderColor = Drawing.Color.Empty
            lblZulDatumVonInfo.Text = ""

            txtZulDatumBis.BorderColor = Drawing.Color.Empty
            lblZulDatumBisInfo.Text = ""
        End Sub

        Private Sub DoSubmit()

            If ValidateError() Then
                Return
            End If

            mBeauftragung.SelKundennr = ddlKunde.SelectedValue
            mBeauftragung.SelKennzeichen = txtKennzeichen.Text
            mBeauftragung.SelReferenz = txtReferenz.Text

            If rbG.Checked = True Then
                mBeauftragung.SelLoeschkennzeichen = "G"
            End If
            If rbA.Checked = True Then
                mBeauftragung.SelLoeschkennzeichen = "A"
            End If

            mBeauftragung.SelZuldatVon = txtZulDatumVon.Text
            mBeauftragung.SelZuldatBis = txtZulDatumBis.Text

            mBeauftragung.FillUebersicht2(Me)
            Session("ResultTable") = mBeauftragung.Result

            If Not mBeauftragung.Status = 0 Then
                lblError.Text = "Fehler: " & mBeauftragung.Message
            Else
                If mBeauftragung.Result.Rows.Count = 0 Then
                    lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
                    Session("ResultTable") = Nothing
                Else
                    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
                    Try
                        Excel.ExcelExport.WriteExcel(mBeauftragung.Result, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Catch ex As Exception
                        lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                    End Try
                    Session("lnkExcel") = "/Services/Temp/Excel/" & strFileName
                    Response.Redirect("Report01s_2.aspx?AppID=" & Session("AppID").ToString, False)
                End If
            End If

        End Sub

        Private Function ValidateError() As Boolean
            Dim booError = False

            'Kunde prüfen (Pflichtfeld)
            If ddlKunde.SelectedValue = "0" Then
                SetErrBehavior(txtKunnr, lblKundeInfo, "Ungültige Kundenauswahl.")
                booError = True
            End If

            'Zulassungsdatum prüfen
            If txtZulDatumVon.Text.Length > 0 Then
                If IsDate(txtZulDatumVon.Text) = False Then
                    SetErrBehavior(txtZulDatumVon, lblZulDatumVonInfo, "Ungültiges Datum.")
                    booError = True
                ElseIf txtZulDatumBis.Text.Length = 0 Then
                    SetErrBehavior(txtZulDatumBis, lblZulDatumBisInfo, "Datum nicht gesetzt.")
                    booError = True
                End If
            End If

            If txtZulDatumBis.Text.Length > 0 Then
                If IsDate(txtZulDatumBis.Text) = False Then
                    SetErrBehavior(txtZulDatumBis, lblZulDatumBisInfo, "Ungültiges Datum.")
                    booError = True
                ElseIf txtZulDatumVon.Text.Length = 0 Then
                    SetErrBehavior(txtZulDatumVon, lblZulDatumVonInfo, "Datum nicht gesetzt.")
                    booError = True
                End If
            End If

            If IsDate(txtZulDatumVon.Text) AndAlso IsDate(txtZulDatumBis.Text) Then

                If CDate(txtZulDatumVon.Text) > CDate(txtZulDatumBis.Text) Then
                    SetErrBehavior(txtZulDatumBis, lblZulDatumBisInfo, "Datum bis größer als Datum von.")
                    booError = True
                ElseIf DateDiff(DateInterval.Day, CDate(txtZulDatumVon.Text), CDate(txtZulDatumBis.Text)) > 60 Then
                    SetErrBehavior(txtZulDatumVon, lblZulDatumVonInfo, "Zeitraum von 60 Tagen überschritten.")
                    SetErrBehavior(txtZulDatumBis, lblZulDatumBisInfo, "Zeitraum von 60 Tagen überschritten.")
                    booError = True
                End If

            End If

            Return booError

        End Function

        Private Sub SetErrBehavior(ByVal txtcontrol As TextBox, ByVal lblControl As Label, ByVal ErrText As String)

            txtcontrol.BorderColor = Drawing.Color.Red

            lblControl.Text = ErrText

        End Sub

#End Region

    End Class
End Namespace
