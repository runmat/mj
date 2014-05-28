Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports AppUeberf.UeberfgStandard_01

Partial Public Class UploadEdit03_2
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    Private strError As String
    Private clsUeberf As UeberfgStandard_01

    Private Sub UploadEdit03_2_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Suchmasken mit Dropdownlisten verbinden, damit die Ergebnisse angezeigt werden
        Me.ctrlAddressSearchRueckliefer.ResultDropdownList = Me.drpRetour

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
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

        If IsPostBack = False Then
            GetData()
        Else
            If clsUeberf Is Nothing Then
                clsUeberf = Session("App_Ueberf")
            End If
            SetData()
        End If

        cmdBack0.Attributes.Add("onclick", "return ShowMessage()")
    End Sub

    Private Sub GetData()

        If Session("App_Ueberf") Is Nothing Then
            clsUeberf = New UeberfgStandard_01(m_User, m_App, "")
        Else
            clsUeberf = Session("App_Ueberf")
        End If


        dv = Session("DataView")

        dv.RowFilter = "TYP = 'ZR'"
        Session(drpRetour.ID) = dv.Table 'Damit man beim Selektieren wieder auf die Daten zugreifen kann

        drpRetour.AutoPostBack = True

        Dim e As Long

        e = 0

        With drpRetour
            .Items.Add(New ListItem("Keine Auswahl", "0"))
            Do While e < dv.Count

                Dim addrRow As DataSets.AddressDataSet.ADDRESSERow = CType(dv.Item(e).Row, DataSets.AddressDataSet.ADDRESSERow)
                .Items.Add(New ListItem(addrRow.NAME + ", " + addrRow.ORT, addrRow.ID))

                e = e + 1
            Loop


        End With


        With clsUeberf

            Me.txtAbName.Text = .ReName
            Me.txtAbStrasse.Text = .ReStrasse
            Me.txtAbNr.Text = .ReNr
            Me.txtAbPLZ.Text = .RePlz
            Me.txtAbOrt.Text = .ReOrt
            Me.txtAbAnsprechpartner.Text = .ReAnsprechpartner
            Me.txtAbTelefon.Text = .ReTelefon
            Me.txtAbTelefon2.Text = .ReTelefon2
            Me.txtAbFax.Text = .ReFax

            Me.txtHerstTyp.Text = .ReHerst
            Me.txtKennzeichen1.Text = .ReKenn1
            Me.txtKennzeichen2.Text = .ReKenn2
            Me.txtVin.Text = .ReVin
            Me.txtRef.Text = .ReRef

            If .ReFzgZugelassen <> "" Then
                rdbZugelassen.Items.FindByValue(.ReFzgZugelassen).Selected = True
            End If
            If .ReSomWin <> "" Then
                rdbBereifung.Items.FindByValue(.ReSomWin).Selected = True
            End If

            If .ReFahrzeugklasse <> "" Then
                rdbFahrzeugklasse.Items.FindByValue(.ReFahrzeugklasse).Selected = True
            End If

            If Not .AdressStatusRuecklieferung = AdressStatus.Frei Then
                Me.txtAbName.Enabled = False
                Me.txtAbStrasse.Enabled = False
                Me.txtAbNr.Enabled = False
                Me.txtAbPLZ.Enabled = False
                Me.txtAbOrt.Enabled = False
                'Me.txtAbAnsprechpartner.Enabled = False
                'Me.txtAbTelefon.Enabled = False
            Else
                Me.txtAbName.Enabled = True
                Me.txtAbStrasse.Enabled = True
                Me.txtAbNr.Enabled = True
                Me.txtAbPLZ.Enabled = True
                Me.txtAbOrt.Enabled = True
                Me.txtAbAnsprechpartner.Enabled = True
                Me.txtAbTelefon.Enabled = True

            End If


        End With
    End Sub

    Private Sub SetData()

        With clsUeberf

            .ReName = Me.txtAbName.Text
            .ReStrasse = Me.txtAbStrasse.Text
            .ReNr = Me.txtAbNr.Text
            .RePlz = Me.txtAbPLZ.Text
            .ReOrt = Me.txtAbOrt.Text
            .ReAnsprechpartner = Me.txtAbAnsprechpartner.Text
            .ReTelefon = Me.txtAbTelefon.Text
            .ReTelefon2 = Me.txtAbTelefon2.Text
            .ReFax = Me.txtAbFax.Text


            .ReHerst = Me.txtHerstTyp.Text
            .ReKenn1 = Me.txtKennzeichen1.Text
            .ReKenn2 = Me.txtKennzeichen2.Text
            .ReVin = Me.txtVin.Text
            .ReRef = Me.txtRef.Text


            If Not Me.txtAbName.Enabled OrElse Me.txtAbName.ReadOnly Then
                .AdressStatusRuecklieferung = UeberfgStandard_01.AdressStatus.Gesperrt
            Else
                .AdressStatusRuecklieferung = UeberfgStandard_01.AdressStatus.Frei
            End If



        End With
    End Sub


    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click

        If clsUeberf Is Nothing Then
            clsUeberf = Session("App_Ueberf")
        End If

        clsUeberf.Modus = 2

        Session("App_Ueberf") = clsUeberf
        Response.Redirect("UploadEdit03.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub rdbZugelassen_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbZugelassen.SelectedIndexChanged
        clsUeberf.ReFzgZugelassen = rdbZugelassen.SelectedItem.Value
    End Sub

    Private Sub rdbBereifung_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbBereifung.SelectedIndexChanged
        clsUeberf.ReSomWin = rdbBereifung.SelectedItem.Value
    End Sub

    Private Sub cmdRight1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdRight1.Click

        If Weiter() = False Then
            clsUeberf.Anschluss = True
            Response.Redirect("UploadEdit04.aspx?AppID=" & Session("AppID").ToString)
        End If



    End Sub
    Private Function Weiter() As Boolean
        Dim booErr As Boolean
        Dim strErr As String


        If clsUeberf Is Nothing Then
            clsUeberf = Session("App_Ueberf")
        End If




        strErr = "Bitte füllen Sie alle Pflichtfelder aus: <br>"

        If txtAbName.Text = "" Then
            booErr = True
            strErr = "Firma / Name <br>"
            txtAbName.BorderStyle = BorderStyle.Solid
            txtAbName.BorderColor = Color.Red
        End If
        If Me.txtAbStrasse.Text = "" Then
            booErr = True
            strErr = strErr & "Straße <br>"
            txtAbStrasse.BorderStyle = BorderStyle.Solid
            txtAbStrasse.BorderColor = Color.Red
        End If

        If txtAbPLZ.Text = "" Then
            booErr = True
            strErr = strErr & "PLZ <br>"
            txtAbPLZ.BorderStyle = BorderStyle.Solid
            txtAbPLZ.BorderColor = Color.Red
        ElseIf Len(txtAbPLZ.Text) <> 5 Then
            booErr = True
            strErr = strErr & "Bitte geben Sie die Postleitzahl fünfstellig ein. <br>"
            txtAbPLZ.BorderStyle = BorderStyle.Solid
            txtAbPLZ.BorderColor = Color.Red
        ElseIf IsNumeric(Me.txtAbPLZ.Text) = False Then
            booErr = True
            strErr = strErr & "Bitte geben Sie numerische Werte für die Postleitzahl ein. <br>"
            txtAbPLZ.BorderStyle = BorderStyle.Solid
            txtAbPLZ.BorderColor = Color.Red
        End If

        If Me.txtAbOrt.Text = "" Then
            booErr = True
            strErr = strErr & "Ort <br>"
            txtAbOrt.BorderStyle = BorderStyle.Solid
            txtAbOrt.BorderColor = Color.Red
        End If
        If txtAbAnsprechpartner.Text = "" Then
            booErr = True
            strErr = strErr & "Ansprechpartner <br>"
            txtAbAnsprechpartner.BorderStyle = BorderStyle.Solid
            txtAbAnsprechpartner.BorderColor = Color.Red
        End If
        If txtAbTelefon.Text = "" Then
            booErr = True
            strErr = strErr & "Tel. <br>"
            txtAbTelefon.BorderStyle = BorderStyle.Solid
            txtAbTelefon.BorderColor = Color.Red
        End If

        With clsUeberf

            If .ReHerst = "" Then
                booErr = True
                strErr = strErr & "Hersteller / Typ <br>"
                txtHerstTyp.BorderStyle = BorderStyle.Solid
                txtHerstTyp.BorderColor = Color.Red
            End If

            If .ReKenn1 = "" Or .ReKenn2 = "" Then
                booErr = True
                strErr = strErr & "Kennzeichen <br>"
                txtKennzeichen1.BorderStyle = BorderStyle.Solid
                txtKennzeichen1.BorderColor = Color.Red
                txtKennzeichen2.BorderStyle = BorderStyle.Solid
                txtKennzeichen2.BorderColor = Color.Red
            End If

            If .ReFzgZugelassen = "" Then
                booErr = True
                strErr = strErr & "Fahrzeug zugelassen und fahrbereit? <br>"
                rdbZugelassen.BorderStyle = BorderStyle.Solid
                rdbZugelassen.BorderColor = Color.Red
            End If

            If .ReFahrzeugklasse = "" Then
                booErr = True
                strErr = strErr & "Fahrzeugklasse "
                rdbFahrzeugklasse.BorderStyle = BorderStyle.Solid
                rdbFahrzeugklasse.BorderColor = Color.Red
            End If


            If .ReSomWin = "" Then
                booErr = True
                strErr = strErr & "Bereifung "
                rdbBereifung.BorderStyle = BorderStyle.Solid
                rdbBereifung.BorderColor = Color.Red
            End If

            'If .An_KCL_Zulassen = "" Then
            '    booErr = True
            '    strErr = strErr & "Zulassung durch KCL?  <br>"
            'End If

            '.SelRetour = drpRetour.SelectedItem.Value


        End With

        SetData()

        Session("App_Ueberf") = clsUeberf

        If booErr = False Then
            Return booErr
            'Response.Redirect("UploadEdit04.aspx?AppID=" & Session("AppID").ToString)
        Else
            Return True
            Me.lblError.Text = strErr
        End If
    End Function
    Private Sub drpRetour_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles drpRetour.SelectedIndexChanged
        dv = Session("DataView")

        If drpRetour.SelectedIndex = -1 OrElse drpRetour.SelectedItem.Value = "" OrElse drpRetour.SelectedItem.Value = "0" Then
            Me.txtAbName.Text = ""
            Me.txtAbName.Enabled = True
            Me.txtAbStrasse.Text = ""
            Me.txtAbStrasse.Enabled = True
            Me.txtAbNr.Text = ""
            Me.txtAbNr.Enabled = True
            Me.txtAbPLZ.Text = ""
            Me.txtAbPLZ.Enabled = True
            Me.txtAbOrt.Text = ""
            Me.txtAbOrt.Enabled = True
            Me.txtAbAnsprechpartner.Text = ""
            Me.txtAbAnsprechpartner.Enabled = True
            Me.txtAbTelefon.Text = ""
            Me.txtAbTelefon.Enabled = True
            Me.txtAbTelefon2.Text = ""
            Me.txtAbFax.Text = ""

        Else
            Dim row As DataSets.AddressDataSet.ADDRESSERow = CType(CType(Session(drpRetour.ID), DataTable).Select("ID='" + drpRetour.SelectedItem.Value + "'")(0), DataSets.AddressDataSet.ADDRESSERow)
            Me.txtAbName.Text = row.NAME
            Me.txtAbName.Enabled = False
            Me.txtAbStrasse.Text = row.STRASSE
            Me.txtAbStrasse.Enabled = False
            Me.txtAbNr.Text = row.HAUSNUMMER
            Me.txtAbNr.Enabled = False
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
        End If
    End Sub

    Private Sub Refill()
        With clsUeberf

            drpRetour.Items.FindByValue(.SelRetour).Selected = True

            Me.txtAbName.Text = .ReName
            Me.txtAbStrasse.Text = .ReStrasse
            Me.txtAbNr.Text = .ReNr
            Me.txtAbPLZ.Text = .RePlz
            Me.txtAbOrt.Text = .ReOrt
            Me.txtAbAnsprechpartner.Text = .ReAnsprechpartner
            Me.txtAbTelefon.Text = .ReTelefon

            If .SelRetour <> "0" Then
                Me.txtAbName.Enabled = False
                Me.txtAbStrasse.Enabled = False
                Me.txtAbNr.Enabled = False
                Me.txtAbPLZ.Enabled = False
                Me.txtAbOrt.Enabled = False
                'Me.txtAbAnsprechpartner.Enabled = False
                'Me.txtAbTelefon.Enabled = False
            Else
                Me.txtAbName.Enabled = True
                Me.txtAbStrasse.Enabled = True
                Me.txtAbNr.Enabled = True
                Me.txtAbPLZ.Enabled = True
                Me.txtAbOrt.Enabled = True
                Me.txtAbAnsprechpartner.Enabled = True
                Me.txtAbTelefon.Enabled = True
            End If


        End With



    End Sub

    Private Sub rdbFahrzeugklasse_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbFahrzeugklasse.SelectedIndexChanged
        clsUeberf.ReFahrzeugklasse = rdbFahrzeugklasse.SelectedItem.Value
        clsUeberf.ReFahrzeugklasseTxt = rdbFahrzeugklasse.SelectedItem.Text & " to"
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub rdbAnZulKCL_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbAnZulKCL.SelectedIndexChanged
        'clsUeberf.An_KCL_Zulassen = rdbAnZulKCL.SelectedItem.Value
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
' $History: UploadEdit03_2.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 13.11.08   Time: 11:33
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
