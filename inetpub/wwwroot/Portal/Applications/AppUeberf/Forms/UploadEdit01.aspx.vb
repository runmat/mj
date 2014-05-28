Imports CKG.Base.Kernel.Common.Common

Partial Public Class UploadEdit01
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private clsUeberf As UeberfgStandard_01


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        If Request.UrlReferrer.ToString.IndexOf("Selection.aspx") = 0 Then
            Session("Ueberf") = Nothing
            clsUeberf = Nothing
        End If


        If IsPostBack = False Then
            FillControlls()
        End If


        If IsNothing(dv) Then
            dv = Session("DataViewRG")
        End If

        If clsUeberf Is Nothing Then
            clsUeberf = Session("App_Ueberf")
            If clsUeberf.Modus = 1 Then
                Refill()
            End If
        Else
            If clsUeberf.Modus = 1 Then
                Refill()
            End If
        End If

        'Beaufragungsart "Reine Überführung"
        If clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ReineUeberfuehrung Or _
                    clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung Or _
                    clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL Or _
                    clsUeberf.Beauftragung = Ueberf_01.Beauftragungsart.UeberfuehrungKCL Then

            cmdBack.Visible = True
        End If

        cmdBack.Attributes.Add("onclick", "return ShowMessage()")
    End Sub

    Private Sub FillControlls()
        Try
            Dim tblPartner As DataTable
            Dim booRegDefault As Boolean
            Dim booRechDefault As Boolean


            If Session("App_Ueberf") Is Nothing Then
                clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
            Else
                clsUeberf = Session("App_Ueberf")
            End If



            tblPartner = clsUeberf.getPartnerStandard(m_User.KUNNR)

            dv = tblPartner.DefaultView


            dv.RowFilter = "PARVW = 'RG'"

            drpRegulierer.AutoPostBack = True

            Dim e As Long

            e = 0
            Do While e < dv.Count
                e = e + 1
            Loop
            e = 0

            With drpRegulierer

                If booRegDefault = False Then
                    .Items.Add(New ListItem("Bitte auswählen", "0"))
                End If

                Do While e < dv.Count
                    .Items.Add(New ListItem(dv.Item(e)("NAME1"), dv.Item(e)("KUNNR")))
                    If clsUeberf.SelRegulierer = dv.Item(e)("KUNNR") Then
                        .SelectedIndex = .Items.Count - 1
                    End If
                    e = e + 1
                Loop


            End With

            e = 0

            dv.RowFilter = "PARVW = 'RE'"

            drpRechnungsempf.AutoPostBack = True

            Do While e < dv.Count
                e = e + 1
            Loop

            e = 0

            With drpRechnungsempf
                If booRechDefault = False Then
                    .Items.Add(New ListItem("Bitte auswählen", "0"))
                End If

                Do While e < dv.Count
                    .Items.Add(New ListItem(dv.Item(e)("NAME1"), dv.Item(e)("KUNNR")))
                    If clsUeberf.SelRechnungsempf = dv.Item(e)("KUNNR") Then
                        .SelectedIndex = .Items.Count - 1
                    End If
                    e = e + 1
                Loop
            End With


            If (Session("DataViewRG")) Is Nothing Then
                Session.Add("DataViewRG", dv)
            Else
                Session("DataViewRG") = dv
            End If

            If clsUeberf.HoldData = True Then
                Refill()
            End If
        Catch ex As Exception
            lblError.Text = "Es ist ein Fehler aufgetreten! Funktion FillControlls meldet: " + ex.Message
        End Try

    End Sub

    Private Sub cmdRight1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRight1.Click

        If Weiter() = False Then
            clsUeberf = Nothing
            Response.Redirect("UploadEdit02.aspx?AppID=" & Session("AppID").ToString)
        End If

    End Sub

    Private Function Weiter() As Boolean
        Dim strErrAbholung As String = ""
        Dim strErrAnlieferung As String = ""
        Dim booErrAbholung As Boolean
        Dim booErrAnlieferung As Boolean


        If txtAbName.Text = "" Then
            booErrAbholung = True
            strErrAbholung = "Firma / Name <br>"
        End If

        If txtAnName.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = "Firma / Name <br>"
        End If


        If booErrAbholung = True Or booErrAnlieferung = True Then

            strError = "Bitte füllen Sie alle Pflichtfelder korrekt aus. <br>"

            If booErrAbholung = True Then
                strError = strError & "Regulierer:  <br>" & strErrAbholung & " <br>"
            End If

            If booErrAnlieferung = True Then
                strError = strError & "Rechnungsempfänger:  <br>" & strErrAnlieferung & " <br>"
            End If

            lblError.Text = strError
            Return True
        Else
            'Daten aus der Seite in die Properties der Klasse eintragen
            With clsUeberf

                .SelRegulierer = drpRegulierer.SelectedItem.Value
                .SelRechnungsempf = drpRechnungsempf.SelectedItem.Value
            End With


        End If
    End Function

    'Private Sub FillfromUpload()

    '    Dim dv As DataView
    '    Dim dataRows() As DataRow

    '    dv = Session("DataViewRG")
    '    If clsUeberf Is Nothing Then
    '        clsUeberf = Session("App_Ueberf")
    '    End If
    '    With clsUeberf
    '        'Regulierer
    '        dataRows = .TabPartnerSel.Select("PARTN_ROLE = 'RG'")


    '        clsUeberf.SelRegulierer = dataRows(0)("PARTN_NUMB")

    '        If Not IsDBNull(dataRows(0)("NAME_2")) Then
    '            txtAbAnsprechpartner.Text = dataRows(0)("NAME_2")
    '        End If

    '        If Not IsDBNull(dataRows(0)("TELEPHONE")) Then
    '            txtAbTelefon.Text = dataRows(0)("TELEPHONE")
    '        End If
    '        If Not IsDBNull(dataRows(0)("NAME")) Then
    '            txtAbName.Text = dataRows(0)("NAME")
    '        End If


    '        If Not IsDBNull(dataRows(0)("CITY")) Then
    '            txtAbOrt.Text = dataRows(0)("CITY")
    '        End If

    '        If Not IsDBNull(dataRows(0)("POSTL_CODE")) Then
    '            txtAbPLZ.Text = dataRows(0)("POSTL_CODE")
    '        End If

    '        If Not IsDBNull(dataRows(0)("STREET")) Then
    '            txtAbStrasse.Text = dataRows(0)("STREET")
    '        End If



    '        'Rechnungsempfänger

    '        dataRows = .TabPartnerSel.Select("PARTN_ROLE = 'RE'")

    '        clsUeberf.SelRechnungsempf = dataRows(0)("PARTN_NUMB")

    '        If Not IsDBNull(dataRows(0)("NAME_2")) Then
    '            txtAnAnsprechpartner.Text = dataRows(0)("NAME2")
    '        End If
    '        If Not IsDBNull(dataRows(0)("NAME")) Then
    '            txtAnName.Text = dataRows(0)("NAME")
    '        End If

    '        If Not IsDBNull(dataRows(0)("CITY")) Then
    '            txtAnOrt.Text = dataRows(0)("CITY")
    '        End If

    '        If Not IsDBNull(dataRows(0)("POSTL_CODE")) Then
    '            txtAnPLZ.Text = dataRows(0)("POSTL_CODE")
    '        End If

    '        If Not IsDBNull(dataRows(0)("STREET")) Then
    '            txtAnStrasse.Text = dataRows(0)("STREET")
    '        End If

    '        If Not IsDBNull(dataRows(0)("TELEPHONE")) Then
    '            txtAnTelefon.Text = dataRows(0)("TELEPHONE")
    '        End If
    '        .Modus = 0
    '    End With

    '    Session("Ueberf") = clsUeberf

    'End Sub

    Private Sub Refill()

        Dim dv As DataView


        dv = Session("DataViewRG")


        With clsUeberf

            dv.RowFilter = "PARVW = 'RG' AND KUNNR = '" & .SelRegulierer & "'"

            If dv.Count > 0 Then

                drpRegulierer.Items.FindByValue(.SelRegulierer).Selected = True

                txtAbAnsprechpartner.Text = dv.Item(0)("NAME2")
                txtAbName.Text = dv.Item(0)("NAME1")
                txtAbNr.Text = dv.Item(0)("HOUSE_NUM1")
                txtAbOrt.Text = dv.Item(0)("CITY1")
                txtAbPLZ.Text = dv.Item(0)("POST_CODE1")
                txtAbStrasse.Text = dv.Item(0)("STREET")
                txtAbTelefon.Text = dv.Item(0)("TEL_NUMBER")

            Else
                lblError.Text = "Angegebener Regulierer nicht vorhanden. Bitte auswählen.<br>"
                drpRegulierer.BackColor = Color.Red
            End If



            dv.RowFilter = "PARVW = 'RE' AND KUNNR = '" & .SelRechnungsempf & "'"

            If dv.Count > 0 Then


                drpRechnungsempf.Items.FindByValue(.SelRechnungsempf).Selected = True

                txtAnAnsprechpartner.Text = dv.Item(0)("NAME2")
                txtAnName.Text = dv.Item(0)("NAME1")
                txtAnNr.Text = dv.Item(0)("HOUSE_NUM1")
                txtAnOrt.Text = dv.Item(0)("CITY1")
                txtAnPLZ.Text = dv.Item(0)("POST_CODE1")
                txtAnStrasse.Text = dv.Item(0)("STREET")
                txtAnTelefon.Text = dv.Item(0)("TEL_NUMBER")
            Else
                lblError.Text = lblError.Text & "Angegebener Rechnungsempfänger nicht vorhanden. Bitte auswählen.<br>"
                drpRechnungsempf.BackColor = Color.Red
            End If

            .Modus = 0
        End With
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        'Beaufragungsart "Reine Überführung" oder "Zulassung und Überführung"
        Response.Redirect("UploadUeberf.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub drpRegulierer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpRegulierer.SelectedIndexChanged

        If drpRegulierer.SelectedItem.Value = "0" Then
            txtAbName.Text = ""
            txtAbStrasse.Text = ""
            txtAbNr.Text = ""
            txtAbPLZ.Text = ""
            txtAbOrt.Text = ""
            txtAbAnsprechpartner.Text = ""
            txtAbTelefon.Text = ""
        Else
            dv.RowFilter = "PARVW = 'RG' AND KUNNR = '" & drpRegulierer.SelectedItem.Value() & "'"
            txtAbName.Text = dv.Item(0)("NAME1")
            txtAbStrasse.Text = dv.Item(0)("STREET") & " " & dv.Item(0)("HOUSE_NUM1")
            txtAbPLZ.Text = dv.Item(0)("POST_CODE1")
            txtAbOrt.Text = dv.Item(0)("CITY1")
            txtAbAnsprechpartner.Text = dv.Item(0)("NAME2")
            txtAbTelefon.Text = dv.Item(0)("TEL_NUMBER")
            clsUeberf.SelRegulierer = drpRegulierer.SelectedItem.Value()

        End If
        drpRegulierer.BackColor = Color.White
    End Sub

    Private Sub drpRechnungsempf_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpRechnungsempf.SelectedIndexChanged

        If drpRechnungsempf.SelectedItem.Value = "0" Then
            txtAnName.Text = ""
            txtAnStrasse.Text = ""
            txtAnNr.Text = ""
            txtAnPLZ.Text = ""
            txtAnOrt.Text = ""
            txtAnAnsprechpartner.Text = ""
            txtAnTelefon.Text = ""
        Else
            dv.RowFilter = "PARVW = 'RE' AND KUNNR = '" & drpRechnungsempf.SelectedItem.Value() & "'"
            txtAnName.Text = dv.Item(0)("NAME1")
            txtAnStrasse.Text = dv.Item(0)("STREET") & " " & dv.Item(0)("HOUSE_NUM1")
            txtAnPLZ.Text = dv.Item(0)("POST_CODE1")
            txtAnOrt.Text = dv.Item(0)("CITY1")
            txtAnAnsprechpartner.Text = dv.Item(0)("NAME2")
            txtAnTelefon.Text = dv.Item(0)("TEL_NUMBER")
        End If
        drpRechnungsempf.BackColor = Color.White
    End Sub

    Private Sub btnConfOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfOK.Click
        Dim RetTable As DataTable
        'Dim strErr As String = ""

        If clsUeberf Is Nothing Then
            clsUeberf = Session("App_Ueberf")
        End If

        If Weiter() = False Then

            clsUeberf.AuftragsStatus = "B"
            If clsUeberf.NewDataSet = False Then
                RetTable = clsUeberf.Update()
            Else
                clsUeberf.AufIDSel = "0000000000"
                clsUeberf.MakeAuftragsTable()
                clsUeberf.MakePartnerTable()
                clsUeberf.NewAuftrag()
                RetTable = clsUeberf.Update("N")
            End If

            If clsUeberf.Message.Length > 0 Then
                lblError.Text = clsUeberf.Message
            Else

                Dim Drow As DataRow
                Dim Nrow As DataRow
                Dim DataTblChecked As New DataTable
                DataTblChecked.Columns.Add("Auf_ID", System.Type.GetType("System.String"))
                DataTblChecked.Columns.Add("Ok", System.Type.GetType("System.Boolean"))
                DataTblChecked.Columns.Add("Del", System.Type.GetType("System.Boolean"))
                DataTblChecked.Columns.Add("NoSel", System.Type.GetType("System.Boolean"))

                If clsUeberf.NewDataSet = False Then
                    For Each Drow In clsUeberf.TabUpload.Rows
                        Nrow = DataTblChecked.NewRow
                        Nrow("Auf_ID") = Drow("Auf_ID")
                        Nrow("Ok") = Drow("Ok")
                        Nrow("Del") = Drow("Del")
                        Nrow("NoSel") = Drow("NoSel")
                        DataTblChecked.Rows.Add(Nrow)
                    Next
                    DataTblChecked.AcceptChanges()
                    Session("App_SelData") = DataTblChecked
                End If

                Session("App_Ueberf") = Nothing
                clsUeberf.CleanClass()
                Response.Redirect("UploadUeberf.aspx?AppID=" & Session("AppID").ToString)

            End If
        End If
    End Sub

    Private Sub btnConfCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfCancel.Click
        Response.Redirect("UploadUeberf.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class
' ************************************************
' $History: UploadEdit01.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 13.11.08   Time: 11:33
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 10.09.08   Time: 17:58
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2197
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 25.08.08   Time: 8:28
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' ************************************************
