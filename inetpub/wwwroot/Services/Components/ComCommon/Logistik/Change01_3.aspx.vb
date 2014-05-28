Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports System.Drawing

Namespace Logistik
    Partial Public Class Change01_3
        Inherits System.Web.UI.Page
        Private m_App As App
        Private m_User As User
        Private m_change As Logistik.Logistik1

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me)
            FormAuth(Me, m_User)
            m_App = New App(m_User)
            GetAppIDFromQueryString(Me)
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            lnkSuche.NavigateUrl = "Change01.aspx?AppID=" & Session("AppID").ToString
            lnkFahrzeug.NavigateUrl = "Change01_2.aspx?AppID=" & Session("AppID").ToString

            If IsPostBack = False Then
                If Session("AppChange") Is Nothing Then
                    m_change = New Logistik.Logistik1(m_User, m_App, CStr(Request.QueryString("AppID")), Session.SessionID.ToString, "")
                    Session("AppChange") = m_change
                Else
                    m_change = CType(Session("AppChange"), Logistik.Logistik1)
                End If
                FillAbholAdresse()
                FillLieferAdresse()
            End If
        End Sub
        Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
            'txtStornotext.Text = String.Empty
            ModalPopupExtender2.Show()
        End Sub

        Protected Sub chkManuell_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkManuell.CheckedChanged
            If chkManuell.Checked = True Then
                txtOrt.Enabled = True
                txtAnsprechpartner.Enabled = True
                txtName.Enabled = True
                txtStrasse.Enabled = True
                txtPostleitzahl.Enabled = True
                txtTel.Enabled = True
                txtNummer.Enabled = True
            Else
                txtOrt.Enabled = False
                txtAnsprechpartner.Enabled = False
                txtName.Enabled = False
                txtStrasse.Enabled = False
                txtPostleitzahl.Enabled = False
                txtTel.Enabled = False
                txtNummer.Enabled = False
            End If
        End Sub
        Private Sub cmdConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdConfirm.Click
            If m_change Is Nothing Then
                m_change = CType(Session("AppChange"), Logistik.Logistik1)
            End If
            m_change.GetGeoEntfernung(CStr(Request.QueryString("AppID")), Session.SessionID.ToString, Me.Page)
            If Not m_change.Entfernung Is Nothing Then
                If ddlAuswahl.SelectedIndex > 0 Then
                    m_change.WE_Nr = ddlAuswahl.SelectedItem.Value
                Else
                    m_change.WE_Nr = ""
                End If
                Session("AppHaendler") = m_change
                Response.Redirect("Change01_4.aspx?AppID=" & Session("AppID").ToString)
            End If
        End Sub
        Protected Sub cmdContinue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdContinue.Click
            If cmdContinue.Text.Contains("Weiter") Then
                Dim bShow As Boolean = False
                lblError.Text = ""

                If CheckGEOAbholAdr() Then
                    ddlAlternativAdressen.Visible = True
                    lblAdressMessage.Visible = True
                    chkAltenativ.Visible = True
                    bShow = True
                Else
                    ddlAlternativAdressen.Visible = False
                    lblAdressMessage.Visible = False
                    chkAltenativ.Visible = False
                End If
                If CheckGEOLiefAdr() Then
                    ddlAlternativAdressen2.Visible = True
                    lblAdressMessage2.Visible = True
                    chkLiefAltenativ.Visible = True
                    bShow = True
                Else
                    ddlAlternativAdressen2.Visible = False
                    lblAdressMessage2.Visible = False
                    chkLiefAltenativ.Visible = False
                End If
                chkAltenativ.Checked = False
                chkLiefAltenativ.Checked = False
                If bShow = True Then
                    ModalPopupExtender2.Show()
                ElseIf lblError.Text = "" Then
                    ConfirmMode(False)
                Else
                    ConfirmMode(True)
                End If

            ElseIf cmdContinue.Text.Contains("Ändern") Then
                ConfirmMode(True)
            End If


        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

        Private Sub FillAbholAdresse()
            Dim tmpRow() As DataRow
            tmpRow = m_change.Daten.Select("Express = 'X' OR Standard = 'X'")

            If tmpRow.Length > 0 Then
                txtFahrgestellnr.Text = tmpRow(0)("CHASSIS_NUM").ToString.Trim
                txtKennz.Text = tmpRow(0)("LICENSE_NUM").ToString.Trim
                m_change.Fahrgestellnr = tmpRow(0)("CHASSIS_NUM").ToString.Trim
                m_change.Kennzeichen = tmpRow(0)("LICENSE_NUM").ToString.Trim
                txtName.Text = tmpRow(0)("NAME1_ZC").ToString.Trim
                txtStrasse.Text = tmpRow(0)("STREET_ZC").ToString.Trim
                txtNummer.Text = tmpRow(0)("HOUSE_NUM1_ZC").ToString.Trim
                txtPostleitzahl.Text = tmpRow(0)("POST_CODE1_ZC").ToString.Trim
                txtOrt.Text = tmpRow(0)("CITY1_ZC").ToString.Trim
                txtTel.Text = tmpRow(0)("TEL_NUMBER_ZC").ToString.Trim
                txtAnsprechpartner.Text = tmpRow(0)("NAME3_ZC").ToString.Trim
            End If


        End Sub

        Private Sub FillLieferAdresse()
            m_change.SORTL = "ES_RUECK"
            m_change.FillLiefAdressen(Session("AppID").ToString, Session.SessionID, Me)
            If m_change.Status = 0 Then
                Session.Add("AppChange", m_change)
                If m_change.LiefAdressen.Rows.Count > 0 Then
                    With ddlAuswahl
                        .Items.Clear()
                        Dim LiItem As New ListItem
                        LiItem.Text = "- Bitte wählen -"
                        LiItem.Value = "0"
                        .Items.Add(LiItem)

                        For i As Integer = 0 To m_change.LiefAdressen.Rows.Count - 1
                            LiItem = New ListItem
                            LiItem.Text = m_change.LiefAdressen.Rows(i)("NAME1").ToString
                            LiItem.Value = m_change.LiefAdressen.Rows(i)("KUNNR").ToString
                            .Items.Add(LiItem)
                        Next
                    End With
                End If

            Else
                lblError.Text = m_change.Message
                Exit Sub
            End If
        End Sub


        Private Function CheckGEOAbholAdr() As Boolean

            Dim PopupExtenderShow As Boolean = False

            If m_change Is Nothing Then
                m_change = CType(Session("AppChange"), Logistik.Logistik1)
            End If


            If Not CheckAbholAdr() Then
                If chkManuell.Checked = True Then
                    Dim AdressTable As New DataTable

                    m_change.GeoAdressen(CStr(Request.QueryString("AppID")), Session.SessionID.ToString, Me.Page)

                    AdressTable = m_change.GeoCodeDaten
                    If IsNothing(AdressTable) = False Then
                        If AdressTable.Rows.Count > 0 Then
                            ddlAlternativAdressen.Items.Clear()


                            If AdressTable.Rows.Count = 1 Then
                                With m_change


                                    Dim CompareStringSource As String

                                    CompareStringSource = .Postleitzahl & _
                                                          .Ort & _
                                                          .Strasse & _
                                                          .Hausnummer

                                    Dim CompareStringResult As String = AdressTable.Rows(0)("ADRESSE").ToString

                                    CompareStringResult = CompareStringResult.Replace(" ", "")
                                    CompareStringResult = CompareStringResult.Replace(",", "")
                                    CompareStringResult = Mid(CompareStringResult, 2)


                                    If Replace(CompareStringSource, " ", "").ToUpper <> Replace(CompareStringResult, " ", "").ToUpper Then

                                        ddlAlternativAdressen.DataSource = AdressTable

                                        ddlAlternativAdressen.DataTextField = "ADRESSE"
                                        ddlAlternativAdressen.DataValueField = ""

                                        ddlAlternativAdressen.DataBind()

                                        ddlAlternativAdressen.Enabled = False

                                        PopupExtenderShow = True

                                    End If

                                End With
                            Else
                                Dim ID As New DataColumn("ID", GetType(Int32))
                                Dim Adresse As New DataColumn("ADRESSE", GetType(String))
                                ID.ColumnName = "ID"
                                ID.AutoIncrement = True
                                ID.AutoIncrementSeed = CLng(ID.AutoIncrementStep = 1)

                                Dim TempTable As New DataTable
                                Dim NewRow As DataRow

                                TempTable.Columns.Add(ID)
                                TempTable.Columns.Add(Adresse)

                                TempTable.AcceptChanges()

                                For Each Row As DataRow In AdressTable.Rows
                                    NewRow = TempTable.NewRow

                                    NewRow("ADRESSE") = Row("ADRESSE")

                                    TempTable.Rows.Add(NewRow)

                                Next
                                TempTable.AcceptChanges()


                                ddlAlternativAdressen.DataSource = TempTable

                                ddlAlternativAdressen.DataValueField = "ID"
                                ddlAlternativAdressen.DataTextField = "ADRESSE"

                                ddlAlternativAdressen.DataBind()

                                PopupExtenderShow = True
                            End If


                        End If
                    End If
                End If
                Return PopupExtenderShow
            Else
                Return False
                lblError.Text = "Bitte überprüfen Sie Ihre Eingaben!"
            End If
            Return PopupExtenderShow
        End Function
        Private Function CheckGEOLiefAdr() As Boolean

            Dim PopupExtenderShow As Boolean = False
            If m_change Is Nothing Then
                m_change = CType(Session("AppChange"), Logistik.Logistik1)
            End If


            If Not CheckLiefAdr() Then

                If chkManuellLief.Checked = True Then
                    Dim AdressTable As New DataTable

                    m_change.GeoAdressen(CStr(Request.QueryString("AppID")), Session.SessionID.ToString, Me.Page, "X")

                    AdressTable = m_change.GeoCodeDaten
                    If IsNothing(AdressTable) = False Then
                        If AdressTable.Rows.Count > 0 Then
                            ddlAlternativAdressen2.Items.Clear()


                            If AdressTable.Rows.Count = 1 Then
                                With m_change
                                    Dim CompareStringSource As String

                                    CompareStringSource = .LiefPostleitzahl & _
                                                          .LiefOrt & _
                                                          .LiefStrasse & _
                                                          .LiefHausnummer

                                    Dim CompareStringResult As String = AdressTable.Rows(0)("ADRESSE").ToString

                                    CompareStringResult = CompareStringResult.Replace(" ", "")
                                    CompareStringResult = CompareStringResult.Replace(",", "")
                                    CompareStringResult = Mid(CompareStringResult, 2)


                                    If Replace(CompareStringSource, " ", "").ToUpper <> Replace(CompareStringResult, " ", "").ToUpper Then

                                        ddlAlternativAdressen2.DataSource = AdressTable

                                        ddlAlternativAdressen2.DataTextField = "ADRESSE"
                                        ddlAlternativAdressen2.DataValueField = ""

                                        ddlAlternativAdressen2.DataBind()

                                        ddlAlternativAdressen2.Enabled = False

                                        PopupExtenderShow = True

                                    End If

                                End With
                            Else
                                Dim ID As New DataColumn("ID", GetType(Int32))
                                Dim Adresse As New DataColumn("ADRESSE", GetType(String))
                                ID.ColumnName = "ID"
                                ID.AutoIncrement = True
                                ID.AutoIncrementSeed = CLng(ID.AutoIncrementStep = 1)

                                Dim TempTable As New DataTable
                                Dim NewRow As DataRow

                                TempTable.Columns.Add(ID)
                                TempTable.Columns.Add(Adresse)

                                TempTable.AcceptChanges()

                                For Each Row As DataRow In AdressTable.Rows
                                    NewRow = TempTable.NewRow

                                    NewRow("ADRESSE") = Row("ADRESSE")

                                    TempTable.Rows.Add(NewRow)

                                Next
                                TempTable.AcceptChanges()


                                ddlAlternativAdressen2.DataSource = TempTable

                                ddlAlternativAdressen2.DataValueField = "ID"
                                ddlAlternativAdressen2.DataTextField = "ADRESSE"

                                ddlAlternativAdressen2.DataBind()

                                PopupExtenderShow = True
                            End If


                        End If
                    End If
                End If
                Return PopupExtenderShow
                Return False
            Else
                lblError.Text = "Bitte überprüfen Sie Ihre Eingaben!"
            End If

        End Function

        Private Sub ddlAuswahl_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAuswahl.SelectedIndexChanged
            With ddlAuswahl
                If .SelectedValue <> "0" Then
                    If Session("AppChange") Is Nothing Then
                        m_change = New Logistik.Logistik1(m_User, m_App, CStr(Request.QueryString("AppID")), Session.SessionID.ToString, "")
                        Session("AppChange") = m_change
                    Else
                        m_change = CType(Session("AppChange"), Logistik.Logistik1)
                    End If
                    Dim drAdressen() As DataRow
                    drAdressen = m_change.LiefAdressen.Select("KUNNR = " & .SelectedValue)
                    If drAdressen.Length > 0 Then
                        ClearLiefFields()
                        txtNamelief.Text = drAdressen(0)("NAME1").ToString.Trim
                        If drAdressen(0)("NAME2").ToString.Trim.Length > 0 Then txtNamelief.Text &= " " & drAdressen(0)("NAME2").ToString.Trim
                        txtStrasseLief.Text = drAdressen(0)("STREET").ToString.Trim
                        txtLiefNummer.Text = drAdressen(0)("HOUSE_NUM1").ToString.Trim
                        txtPostleitzahllief.Text = drAdressen(0)("POST_CODE1").ToString.Trim
                        txtOrtlief.Text = drAdressen(0)("CITY1").ToString.Trim
                        txtTelLief.Text = drAdressen(0)("TEL_NUMBER").ToString.Trim
                        txtLiefAnsprechpartner.Text = drAdressen(0)("NAME3").ToString.Trim
                    End If
                End If
            End With
        End Sub

        Private Sub ClearLiefFields()

            txtNamelief.Text = ""
            txtStrasseLief.Text = ""
            txtLiefNummer.Text = ""
            txtPostleitzahllief.Text = ""
            txtOrtlief.Text = ""
            txtTelLief.Text = ""
            txtLiefAnsprechpartner.Text = ""
        End Sub

        Private Sub chkManuellLief_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkManuellLief.CheckedChanged
            If chkManuellLief.Checked = True Then
                txtOrtlief.Enabled = True
                txtLiefAnsprechpartner.Enabled = True
                txtNamelief.Enabled = True
                txtStrasseLief.Enabled = True
                txtPostleitzahllief.Enabled = True
                txtTelLief.Enabled = True
                txtLiefNummer.Enabled = True
            Else
                txtOrtlief.Enabled = False
                txtLiefAnsprechpartner.Enabled = False
                txtNamelief.Enabled = False
                txtStrasseLief.Enabled = False
                txtPostleitzahllief.Enabled = False
                txtTelLief.Enabled = False
                txtLiefNummer.Enabled = False
            End If
        End Sub
        Private Function CheckLiefAdr() As Boolean
            Dim Bstyle As BorderStyle = BorderStyle.Solid
            Dim Bcolor As Color = Color.Maroon
            Dim bError As Boolean

            If txtNamelief.Text.Trim(" "c).Length = 0 Then
                bError = True
                txtNamelief.BorderColor = Bcolor
                txtNamelief.BorderStyle = Bstyle
            Else
                txtNamelief.BorderStyle = BorderStyle.NotSet
                txtNamelief.BorderColor = Nothing
                m_change.LiefName = txtNamelief.Text
            End If
            If txtStrasseLief.Text.Trim(" "c).Length = 0 Then
                bError = True
                txtStrasseLief.BorderColor = Bcolor
                txtStrasseLief.BorderStyle = Bstyle
            Else
                txtStrasseLief.BorderColor = Nothing
                txtStrasseLief.BorderStyle = BorderStyle.NotSet
                m_change.LiefStrasse = txtStrasseLief.Text
            End If
            If txtLiefNummer.Text.Trim(" "c).Length = 0 Then
                bError = True
                txtLiefNummer.BorderColor = Bcolor
                txtLiefNummer.BorderStyle = Bstyle
            Else
                txtLiefNummer.BorderStyle = BorderStyle.NotSet
                txtLiefNummer.BorderColor = Nothing

                m_change.LiefHausnummer = txtLiefNummer.Text
            End If
            If txtPostleitzahllief.Text.Trim(" "c).Length = 0 Then
                bError = True
                txtPostleitzahllief.BorderColor = Bcolor
                txtPostleitzahllief.BorderStyle = Bstyle
            Else
                txtPostleitzahllief.BorderStyle = BorderStyle.NotSet
                txtPostleitzahllief.BorderColor = Nothing

                m_change.LiefPostleitzahl = txtPostleitzahllief.Text
            End If
            If txtOrtlief.Text.Trim(" "c).Length = 0 Then
                bError = True
                txtOrtlief.BorderColor = Bcolor
                txtOrtlief.BorderStyle = Bstyle
            Else
                txtOrtlief.BorderStyle = BorderStyle.NotSet
                txtOrtlief.BorderColor = Nothing

                m_change.LiefOrt = txtOrtlief.Text
            End If
            If txtTelLief.Text.Trim(" "c).Length = 0 Then
                bError = True
                txtTelLief.BorderColor = Bcolor
                txtTelLief.BorderStyle = Bstyle
            Else
                txtTelLief.BorderStyle = BorderStyle.NotSet
                txtTelLief.BorderColor = Nothing

                m_change.LiefTelefon = txtTelLief.Text
            End If
            Return bError
        End Function

        Private Function CheckAbholAdr() As Boolean
            Dim Bstyle As BorderStyle = BorderStyle.Solid
            Dim Bcolor As Color = Color.Maroon
            Dim bError As Boolean

            If txtName.Text.Trim(" "c).Length = 0 Then
                bError = True
                txtName.BorderColor = Bcolor
                txtName.BorderStyle = Bstyle
            Else
                txtName.BorderStyle = BorderStyle.NotSet
                txtName.BorderColor = Nothing
                m_change.Name = txtName.Text
            End If
            If txtStrasse.Text.Trim(" "c).Length = 0 Then
                bError = True
                txtStrasse.BorderColor = Bcolor
                txtStrasse.BorderStyle = Bstyle
            Else
                txtStrasse.BorderColor = Nothing
                txtStrasse.BorderStyle = BorderStyle.NotSet
                m_change.Strasse = txtStrasse.Text
            End If
            If txtNummer.Text.Trim(" "c).Length = 0 Then
                bError = True
                txtNummer.BorderColor = Bcolor
                txtNummer.BorderStyle = Bstyle
            Else
                txtNummer.BorderStyle = BorderStyle.NotSet
                txtNummer.BorderColor = Nothing

                m_change.Hausnummer = txtNummer.Text
            End If
            If txtPostleitzahl.Text.Trim(" "c).Length = 0 Then
                bError = True
                txtPostleitzahl.BorderColor = Bcolor
                txtPostleitzahl.BorderStyle = Bstyle
            Else
                txtPostleitzahl.BorderStyle = BorderStyle.NotSet
                txtPostleitzahl.BorderColor = Nothing

                m_change.Postleitzahl = txtPostleitzahl.Text
            End If
            If txtOrt.Text.Trim(" "c).Length = 0 Then
                bError = True
                txtOrt.BorderColor = Bcolor
                txtOrt.BorderStyle = Bstyle
            Else
                txtOrt.BorderStyle = BorderStyle.NotSet
                txtOrt.BorderColor = Nothing

                m_change.Ort = txtOrt.Text
            End If
            If txtTel.Text.Trim(" "c).Length = 0 Then
                bError = True
                txtTel.BorderColor = Bcolor
                txtTel.BorderStyle = Bstyle
            Else
                txtTel.BorderStyle = BorderStyle.NotSet
                txtTel.BorderColor = Nothing

                m_change.Telefon = txtTel.Text
            End If
            If txtFixTermin.Text.Trim(" "c).Length > 0 Then
                m_change.FixTermin = txtFixTermin.Text
                m_change.FixFlag = "X"
            End If

            Return bError
        End Function

        Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
            If m_change Is Nothing Then
                m_change = CType(Session("AppChange"), Logistik1)
            End If

            With m_change
                If chkAltenativ.Checked = True Then
                    Dim Adresse As String = ddlAlternativAdressen.SelectedItem.Text

                    Dim arrAdressen() As String = Split(ddlAlternativAdressen.SelectedItem.Text, ",").ToArray


                    .Postleitzahl = arrAdressen(1)
                    txtPostleitzahl.Text = .Postleitzahl

                    .Ort = arrAdressen(2)
                    txtOrt.Text = .Ort

                    .Strasse = arrAdressen(3)
                    txtStrasse.Text = .Strasse

                    If arrAdressen.Length > 4 Then
                        .Hausnummer = arrAdressen(4)
                        txtNummer.Text = .Hausnummer

                    Else
                        .Hausnummer = ""
                        txtNummer.Text = ""
                        txtNummer.Enabled = True
                    End If

                End If
                If chkLiefAltenativ.Checked = True Then
                    Dim Adresse As String = ddlAlternativAdressen2.SelectedItem.Text

                    Dim arrAdressen() As String = Split(ddlAlternativAdressen2.SelectedItem.Text, ",").ToArray


                    .LiefPostleitzahl = arrAdressen(1)
                    txtPostleitzahllief.Text = .LiefPostleitzahl

                    .LiefOrt = arrAdressen(2)
                    txtOrtlief.Text = .LiefOrt

                    .LiefStrasse = arrAdressen(3)
                    txtStrasseLief.Text = .LiefStrasse

                    If arrAdressen.Length > 4 Then
                        .LiefHausnummer = arrAdressen(4)
                        txtLiefNummer.Text = .LiefHausnummer

                    Else
                        .LiefHausnummer = ""
                        txtLiefNummer.Text = ""
                        txtLiefNummer.Enabled = True
                    End If
                End If
            End With

            lblError.Text = m_change.ErrMessage
            m_change.Confirm = True
            Session("AppChange") = m_change

            ModalPopupExtender2.Hide()

            ConfirmMode(False)

        End Sub


        Public Sub ConfirmMode(ByVal enabled As Boolean)
            If enabled Then
                cmdContinue.Text = "&nbsp;&#187;Weiter"
            Else
                cmdContinue.Text = "&nbsp;&#187;Ändern"
                cmdConfirm.Visible = True
            End If


            cmdConfirm.Visible = Not enabled
            If Trim(txtNummer.Text).Length > 0 Then
                txtNummer.Enabled = enabled
            Else
                txtNummer.Enabled = Not enabled
            End If
            txtFahrgestellnr.Enabled = enabled
            txtKennz.Enabled = enabled
            txtAnsprechpartner.Enabled = enabled
            txtName.Enabled = enabled
            txtOrt.Enabled = enabled
            txtPostleitzahl.Enabled = enabled
            txtStrasse.Enabled = enabled
            txtTel.Enabled = enabled
            txtFixTermin.Enabled = enabled
            chkManuell.Enabled = enabled

            ddlAuswahl.Enabled = enabled
            txtNamelief.Enabled = enabled
            txtLiefAnsprechpartner.Enabled = enabled
            txtOrtlief.Enabled = enabled
            txtPostleitzahllief.Enabled = enabled
            txtStrasseLief.Enabled = enabled
            txtTelLief.Enabled = enabled
            chkManuellLief.Enabled = enabled
            If Trim(txtLiefNummer.Text).Length > 0 Then
                txtLiefNummer.Enabled = enabled
            Else
                txtLiefNummer.Enabled = Not enabled
            End If
        End Sub


        Protected Sub btnCancel2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel2.Click
            ConfirmMode(False)
        End Sub
    End Class



End Namespace
