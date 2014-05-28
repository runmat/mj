Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Web.UI.WebControls

Partial Public Class UploadEdit02
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private clsUeberf As UeberfgStandard_01

    Private Sub UploadEdit02_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Suchmasken mit Dropdownlisten verbinden, damit die Ergebnisse angezeigt werden
        Me.ctrlAddressSearchAbholung.ResultDropdownList = drpAbholung
        Me.ctrlAddressSearchAnlieferung.ResultDropdownList = drpAnlieferung
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)
        'Dim imgHeaderDeco As New Image()
        'imgHeaderDeco.ImageUrl = Page.ClientScript.GetWebResourceUrl( _
        '                 Type.GetType(DialogBox1), "WebControls.Resources.header_decoration.gif")

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            'DoSubmit()
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try

        If Request.UrlReferrer.ToString.IndexOf("Selection.aspx") = 0 Then
            Session("App_Ueberf") = Nothing
            clsUeberf = Nothing
        End If


        If IsPostBack = False Then
            'FillfromUpload()
            FillControlls()
        End If


        If IsNothing(dv) Then
            dv = Session("DataView")
        End If

        If clsUeberf Is Nothing Then
            clsUeberf = Session("App_Ueberf")
            If clsUeberf.Modus = 1 Then
                Refill()
            End If
        Else
            If clsUeberf.Modus = 1 Or clsUeberf.HoldData = True Then
                Refill()
            End If
        End If
        If Not Session("App_ScrollX") Is Nothing Then
            xCoordHolder.Value = Session("App_ScrollX")
        End If
        If Not Session("App_ScrollY") Is Nothing Then
            yCoordHolder.Value = Session("App_ScrollY")
        End If

        cmdBack0.Attributes.Add("onclick", "return ShowMessage()")
    End Sub
    Private Sub FillControlls()
        Dim tblPartner As DataTable

        If Session("App_Ueberf") Is Nothing Then
            clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
        Else
            clsUeberf = Session("App_Ueberf")
        End If


        tblPartner = clsUeberf.getPartner(m_User.KUNNR)

        dv = tblPartner.DefaultView

        If (Session("DataView")) Is Nothing Then
            Session.Add("DataView", dv)
        Else
            Session("DataView") = dv
        End If



        If (Session("App_Ueberf")) Is Nothing Then
            Session.Add("App_Ueberf", clsUeberf)
        Else
            Session("App_Ueberf") = clsUeberf
        End If

    End Sub

    Private Sub FillfromUpload()
        Dim dataRows() As DataRow
        Dim dataRow As DataRow

        If Session("App_Ueberf") Is Nothing Then
            clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
        Else
            clsUeberf = Session("App_Ueberf")
        End If
        With clsUeberf
            ' schon in der UploadUeberf füllen alles!!!!

            dataRows = .TabPartnerSel.Select("PARTN_ROLE = 'ZB'") 
            For Each dataRow In dataRows
                'Abholadresse
                If Not IsDBNull(dataRows(0)("PARTN_NUMB")) Then
                    clsUeberf.SelAbholung = dataRows(0)("PARTN_NUMB")
                End If


                If Not IsDBNull(dataRows(0)("NAME_2")) Then
                    Me.txtAbAnsprechpartner.Text = dataRows(0)("NAME_2")
                    .AbAnsprechpartner = Me.txtAbAnsprechpartner.Text
                End If

                If Not IsDBNull(dataRows(0)("TELEPHONE")) Then
                    Me.txtAbTelefon.Text = dataRows(0)("TELEPHONE")
                    .AbTelefon = dataRows(0)("TELEPHONE")
                End If
                If Not IsDBNull(dataRows(0)("NAME")) Then
                    Me.txtAbName.Text = dataRows(0)("NAME")
                    .AbName = Me.txtAbName.Text
                End If
                If Not IsDBNull(dataRows(0)("CITY")) Then
                    Me.txtAbOrt.Text = dataRows(0)("CITY")
                    .AbOrt = Me.txtAbOrt.Text
                End If

                If Not IsDBNull(dataRows(0)("POSTL_CODE")) Then
                    Me.txtAbPLZ.Text = dataRows(0)("POSTL_CODE")
                    .AbPlz = Me.txtAbPLZ.Text
                End If

                If Not IsDBNull(dataRows(0)("STREET")) Then
                    Me.txtAbStrasse.Text = dataRows(0)("STREET")
                    .AbStrasse = Me.txtAbStrasse.Text
                End If

                If Not IsDBNull(dataRows(0)("TELEPHONE2")) Then
                    Me.txtAbTelefon2.Text = dataRows(0)("TELEPHONE2")

                    .AbTelefon2 = Me.txtAbTelefon2.Text
                End If
                If Not IsDBNull(dataRows(0)("FAX_NUMBER")) Then
                    Me.txtAbFax.Text = dataRows(0)("FAX_NUMBER")
                    .AbFax = Me.txtAbFax.Text
                End If

            Next
            'Anlieferadresse

            dataRows = .TabPartnerSel.Select("PARTN_ROLE = 'WE'")
            For Each dataRow In dataRows
                If Not IsDBNull(dataRows(0)("PARTN_NUMB")) Then
                    clsUeberf.SelAnlieferung = dataRows(0)("PARTN_NUMB")
                End If

                If Not IsDBNull(dataRows(0)("NAME_2")) Then
                    Me.txtAnAnsprechpartner.Text = dataRows(0)("NAME_2")
                    .AnAnsprechpartner = Me.txtAnAnsprechpartner.Text
                End If
                If Not IsDBNull(dataRows(0)("NAME")) Then
                    Me.txtAnName.Text = dataRows(0)("NAME")
                    .AnName = Me.txtAnName.Text
                End If

                If Not IsDBNull(dataRows(0)("CITY")) Then
                    Me.txtAnOrt.Text = dataRows(0)("CITY")
                    .AnOrt = Me.txtAnOrt.Text
                End If

                If Not IsDBNull(dataRows(0)("POSTL_CODE")) Then
                    Me.txtAnPLZ.Text = dataRows(0)("POSTL_CODE")
                    .AnPlz = Me.txtAnPLZ.Text
                End If

                If Not IsDBNull(dataRows(0)("STREET")) Then
                    Me.txtAnStrasse.Text = dataRows(0)("STREET")
                    .AnStrasse = Me.txtAnStrasse.Text
                End If

                If Not IsDBNull(dataRows(0)("TELEPHONE")) Then
                    Me.txtAnTelefon.Text = dataRows(0)("TELEPHONE")
                    .AnTelefon = Me.txtAnTelefon.Text
                End If
                If Not IsDBNull(dataRows(0)("TELEPHONE2")) Then
                    Me.txtAnTelefon2.Text = dataRows(0)("TELEPHONE2")

                    .AnTelefon2 = Me.txtAnTelefon2.Text
                End If
                If Not IsDBNull(dataRows(0)("FAX_NUMBER")) Then
                    Me.txtAnFax.Text = dataRows(0)("FAX_NUMBER")
                    .AnFax = Me.txtAnFax.Text
                End If
            Next
        End With

        Session("App_Ueberf") = clsUeberf

    End Sub

    Private Sub drpAbholung_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpAbholung.SelectedIndexChanged

        If drpAbholung.SelectedItem.Value = "" OrElse drpAbholung.SelectedItem.Value = "0" Then
            Me.txtAbName.Text = ""
            Me.txtAbName.Enabled = True
            Me.txtAbStrasse.Text = ""
            Me.txtAbStrasse.Enabled = True
            Me.txtAbPLZ.Text = ""
            Me.txtAbPLZ.Enabled = True
            Me.txtAbOrt.Text = ""
            Me.txtAbOrt.Enabled = True
            Me.txtAbAnsprechpartner.Text = ""
            Me.txtAbAnsprechpartner.Enabled = True
            Me.txtAbTelefon.Text = ""
            Me.txtAbTelefon.Enabled = True
            Me.txtAbTelefon2.Text = ""
            Me.txtAbTelefon2.Enabled = True
        Else
            Dim row As DataSets.AddressDataSet.ADDRESSERow = CType(CType(Session(drpAbholung.ID), DataTable).Select("ID='" + drpAbholung.SelectedItem.Value + "'")(0), DataSets.AddressDataSet.ADDRESSERow)

            Me.txtAbName.Text = row.NAME
            Me.txtAbName.Enabled = False
            Me.txtAbStrasse.Text = row.STRASSE & " " & row.HAUSNUMMER
            Me.txtAbStrasse.Enabled = False
            'Me.txtAbNr.Enabled = False
            Me.txtAbPLZ.Text = row.PLZ
            Me.txtAbPLZ.Enabled = False
            Me.txtAbOrt.Text = row.ORT
            Me.txtAbOrt.Enabled = False
            Me.txtAbAnsprechpartner.Text = row.NAME2
            'Me.txtAbAnsprechpartner.Enabled = False
            Me.txtAbTelefon.Text = row.TELEFON1
            'Me.txtAbTelefon.Enabled = False
            Me.txtAbTelefon2.Text = row.TELEFON2
            Me.txtAbFax.Text = row.FAX
            clsUeberf.SelAbholung = row.KUNDENNUMMER

        End If

    End Sub

    Private Sub drpAnlieferung_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpAnlieferung.SelectedIndexChanged
        If drpAnlieferung.SelectedItem.Value = "" OrElse drpAnlieferung.SelectedItem.Value = "0" Then
            Me.txtAnName.Text = ""
            Me.txtAnName.Enabled = True
            Me.txtAnStrasse.Text = ""
            Me.txtAnStrasse.Enabled = True
            Me.txtAnPLZ.Text = ""
            Me.txtAnPLZ.Enabled = True
            Me.txtAnOrt.Text = ""
            Me.txtAnOrt.Enabled = True
            Me.txtAnAnsprechpartner.Text = ""
            Me.txtAnAnsprechpartner.Enabled = True
            Me.txtAnTelefon.Text = ""
            Me.txtAnTelefon.Enabled = True
            Me.txtAnTelefon2.Text = ""
            Me.txtAnTelefon2.Enabled = True
            Me.txtAnFax.Text = ""
        Else
            Dim row As DataSets.AddressDataSet.ADDRESSERow = CType(CType(Session(drpAnlieferung.ID), DataTable).Select("ID='" + drpAnlieferung.SelectedItem.Value + "'")(0), DataSets.AddressDataSet.ADDRESSERow)
            Me.txtAnName.Text = row.NAME
            Me.txtAnName.Enabled = False
            Me.txtAnStrasse.Text = row.STRASSE & " " & row.HAUSNUMMER
            Me.txtAnStrasse.Enabled = False
            'Me.txtAnNr.Text = row.HAUSNUMMER
            'Me.txtAnNr.Enabled = False
            Me.txtAnPLZ.Text = row.PLZ
            Me.txtAnPLZ.Enabled = False
            Me.txtAnOrt.Text = row.ORT
            Me.txtAnOrt.Enabled = False
            Me.txtAnAnsprechpartner.Text = row.NAME2
            'Me.txtAnAnsprechpartner.Enabled = False
            Me.txtAnTelefon.Text = row.TELEFON1
            'Me.txtAnTelefon.Enabled = False
            Me.txtAnTelefon2.Text = row.TELEFON2
            Me.txtAnFax.Text = row.FAX
            clsUeberf.SelAnlieferung = row.KUNDENNUMMER
        End If
        Session("App_ScrollY") = yCoordHolder.Value
        Session("App_ScrollX") = xCoordHolder.Value
    End Sub

    Private Sub cmdRight1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRight1.Click
        Weiter()

    End Sub
    Private Sub Weiter()
        Dim strErrAbholung As String = ""
        Dim strErrAnlieferung As String = ""
        Dim booErrAbholung As Boolean
        Dim booErrAnlieferung As Boolean


        If txtAbName.Text = "" Then
            booErrAbholung = True
            strErrAbholung = "Firma / Name <br>"
            txtAbName.BorderColor = Color.Red
            txtAbName.BorderStyle = BorderStyle.Solid
        End If
        If Me.txtAbStrasse.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Straße <br>"
            txtAbStrasse.BorderColor = Color.Red
            txtAbStrasse.BorderStyle = BorderStyle.Solid
        End If
        'If Me.txtAbNr.Text = "" Then
        '    booErrAbholung = True
        '    strErrAbholung = strErrAbholung & "Nr. <br>"
        'End If
        If txtAbPLZ.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "PLZ <br>"
            txtAbPLZ.BorderColor = Color.Red
            txtAbPLZ.BorderStyle = BorderStyle.Solid
        ElseIf Len(txtAbPLZ.Text) <> 5 Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Bitte geben Sie die Postleitzahl fünfstellig ein. <br>"
            txtAbPLZ.BorderColor = Color.Red
            txtAbPLZ.BorderStyle = BorderStyle.Solid
        ElseIf IsNumeric(Me.txtAbPLZ.Text) = False Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Bitte geben Sie numerische Werte für die Postleitzahl ein. <br>"
            txtAbPLZ.BorderStyle = BorderStyle.Solid
            txtAbPLZ.BorderColor = Color.Red
            txtAbPLZ.BorderStyle = BorderStyle.Solid
        End If
        If Me.txtAbOrt.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Ort <br>"
            txtAbOrt.BorderStyle = BorderStyle.Solid
            txtAbOrt.BorderColor = Color.Red
        End If
        If txtAbAnsprechpartner.Text = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Ansprechpartner <br>"
            txtAbAnsprechpartner.BorderColor = Color.Red
            txtAbAnsprechpartner.BorderStyle = BorderStyle.Solid
        End If
        If txtAbTelefon.Text.Trim = "" Then
            booErrAbholung = True
            strErrAbholung = strErrAbholung & "Tel. <br>"
            txtAbTelefon.BorderColor = Color.Red
            txtAbTelefon.BorderStyle = BorderStyle.Solid
        End If

        If txtAnName.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = "Firma / Name <br>"
            txtAnPLZ.BorderStyle = BorderStyle.Solid
            txtAnPLZ.BorderColor = Color.Red
        End If
        If Me.txtAnStrasse.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Straße <br>"
            txtAnPLZ.BorderStyle = BorderStyle.Solid
            txtAnPLZ.BorderColor = Color.Red
        End If
        'If Me.txtAnNr.Text = "" Then
        '    booErrAnlieferung = True
        '    strErrAnlieferung = strErrAnlieferung & "Nr. <br>"
        'End If
        If txtAnPLZ.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAbholung & "PLZ <br>"
            txtAnPLZ.BorderColor = Color.Red
            txtAnPLZ.BorderStyle = BorderStyle.Solid
        ElseIf Len(txtAnPLZ.Text) <> 5 Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Bitte geben Sie die Postleitzahl fünfstellig ein. <br>"
            txtAnPLZ.BorderColor = Color.Red
            txtAnPLZ.BorderStyle = BorderStyle.Solid
        ElseIf IsNumeric(Me.txtAnPLZ.Text) = False Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Bitte geben Sie numerische Werte für die Postleitzahl ein. <br>"
            txtAnPLZ.BorderStyle = BorderStyle.Solid
            txtAnPLZ.BorderColor = Color.Red
        End If
        If Me.txtAnOrt.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Ort <br>"
            txtAbOrt.BorderStyle = BorderStyle.Solid
            txtAnOrt.BorderColor = Color.Red
        End If

        If txtAnAnsprechpartner.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Ansprechpartner <br>"
            txtAnAnsprechpartner.BorderStyle = BorderStyle.Solid
            txtAnAnsprechpartner.BorderColor = Color.Red
        End If

        If txtAnTelefon.Text = "" Then
            booErrAnlieferung = True
            strErrAnlieferung = strErrAnlieferung & "Tel. <br>"
            txtAnTelefon.BorderStyle = BorderStyle.Solid
            txtAnTelefon.BorderColor = Color.Red
        End If

        If booErrAbholung = True Or booErrAnlieferung = True Then

            strError = "Bitte füllen Sie alle Pflichtfelder korrekt aus. <br>"

            If booErrAbholung = True Then
                strError = strError & "Abholung:  <br>" & strErrAbholung & " <br>"
            End If

            If booErrAnlieferung = True Then
                strError = strError & "Anlieferung:  <br>" & strErrAnlieferung & " <br>"
            End If

            lblError.Text = strError

        Else
            'Daten aus der Seite in die Properties der Klasse eintragen
            With clsUeberf
                'Abholadresse
                .AbName = Me.txtAbName.Text
                .AbStrasse = Me.txtAbStrasse.Text
                '.AbNr = Me.txtAbNr.Text
                .AbPlz = Me.txtAbPLZ.Text
                .AbOrt = Me.txtAbOrt.Text
                .AbAnsprechpartner = Me.txtAbAnsprechpartner.Text
                .AbTelefon = Me.txtAbTelefon.Text
                .AbTelefon2 = Me.txtAbTelefon2.Text
                .AbFax = Me.txtAbFax.Text

                'Anlieferadresse
                .AnName = Me.txtAnName.Text
                .AnStrasse = Me.txtAnStrasse.Text
                '.AnNr = Me.txtAnNr.Text
                .AnPlz = Me.txtAnPLZ.Text
                .AnOrt = Me.txtAnOrt.Text
                .AnAnsprechpartner = Me.txtAnAnsprechpartner.Text
                .AnTelefon = Me.txtAnTelefon.Text
                .AnTelefon2 = Me.txtAnTelefon2.Text
                .AnFax = Me.txtAnFax.Text

                ''Einträge in den Dropdownlists
                '.AdressStatusAbholung = drpAbholung.SelectedItem.Value
                '.AdressStatusAnlieferung = drpAnlieferung.SelectedItem.Value

                If Not txtAbName.Enabled OrElse txtAbName.ReadOnly Then
                    .AdressStatusAbholung = UeberfgStandard_01.AdressStatus.Gesperrt
                Else
                    .AdressStatusAbholung = UeberfgStandard_01.AdressStatus.Frei
                End If

                If Not txtAnName.Enabled OrElse txtAnName.ReadOnly Then
                    .AdressStatusAnlieferung = UeberfgStandard_01.AdressStatus.Gesperrt
                Else
                    .AdressStatusAnlieferung = UeberfgStandard_01.AdressStatus.Frei
                End If
                clsUeberf = Nothing
                Response.Redirect("UploadEdit03.aspx?AppID=" & Session("AppID").ToString)
            End With

        End If
    End Sub
    Private Sub Refill()
        With clsUeberf

            Me.txtAbName.Text = .AbName
            Me.txtAbStrasse.Text = .AbStrasse
            'Me.txtAbNr.Text = .AbNr
            Me.txtAbPLZ.Text = .AbPlz
            Me.txtAbOrt.Text = .AbOrt
            Me.txtAbAnsprechpartner.Text = .AbAnsprechpartner
            Me.txtAbTelefon.Text = .AbTelefon
            Me.txtAbTelefon2.Text = .AbTelefon2
            Me.txtAbFax.Text = .AbFax

            If .AdressStatusAbholung <> UeberfgStandard_01.AdressStatus.Frei Then
                Me.txtAbName.Enabled = False
                Me.txtAbStrasse.Enabled = False
                'Me.txtAbNr.Enabled = False
                Me.txtAbPLZ.Enabled = False
                Me.txtAbOrt.Enabled = False
                'Me.txtAbAnsprechpartner.Enabled = False
                'Me.txtAbTelefon.Enabled = False

            Else
                Me.txtAbName.Enabled = True
                Me.txtAbStrasse.Enabled = True
                'Me.txtAbNr.Enabled = True
                Me.txtAbPLZ.Enabled = True
                Me.txtAbOrt.Enabled = True
                Me.txtAbTelefon.Enabled = True
            End If

            Me.txtAnName.Text = .AnName
            Me.txtAnStrasse.Text = .AnStrasse
            'Me.txtAnNr.Text = .AnNr
            Me.txtAnPLZ.Text = .AnPlz
            Me.txtAnOrt.Text = .AnOrt
            Me.txtAnAnsprechpartner.Text = .AnAnsprechpartner
            Me.txtAnTelefon.Text = .AnTelefon
            Me.txtAnTelefon2.Text = .AnTelefon2
            Me.txtAnFax.Text = .AnFax


            If .AdressStatusAnlieferung <> UeberfgStandard_01.AdressStatus.Frei Then
                Me.txtAnName.Enabled = False
                Me.txtAnStrasse.Enabled = False
                'Me.txtAnNr.Enabled = False
                Me.txtAnPLZ.Enabled = False
                Me.txtAnOrt.Enabled = False
                'Me.txtAnAnsprechpartner.Enabled = False
                'Me.txtAnTelefon.Enabled = False
            Else
                Me.txtAnName.Enabled = True
                Me.txtAnStrasse.Enabled = True
                ' Me.txtAnNr.Enabled = True
                Me.txtAnPLZ.Enabled = True
                Me.txtAnOrt.Enabled = True
                Me.txtAnAnsprechpartner.Enabled = True
                Me.txtAnTelefon.Enabled = True
            End If

            .Modus = 0
        End With



    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        If clsUeberf Is Nothing Then
            clsUeberf = Session("Ueberf")
        End If

        clsUeberf.Modus = 1

        Session("App_Ueberf") = clsUeberf
        Response.Redirect("UploadEdit01.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub btnConfOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfOK.Click
        Dim RetTable As DataTable
        Dim strErr As String = ""

        If clsUeberf Is Nothing Then
            clsUeberf = Session("App_Ueberf")
        End If

        Weiter()

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
    End Sub

    Private Sub btnConfCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfCancel.Click
        Response.Redirect("UploadUeberf.aspx?AppID=" & Session("AppID").ToString)
    End Sub

End Class
' ************************************************
' $History: UploadEdit02.aspx.vb $
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 13.11.08   Time: 12:55
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2340
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 13.11.08   Time: 11:33
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 6.11.08    Time: 13:04
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2368
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 24.09.08   Time: 15:18
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 10.09.08   Time: 17:58
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2197
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 8.09.08    Time: 11:51
' Updated in $/CKAG/Applications/AppUeberf/Forms
' ITA: 2197
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 25.08.08   Time: 8:28
' Updated in $/CKAG/Applications/AppUeberf/Forms
' 
' ************************************************
