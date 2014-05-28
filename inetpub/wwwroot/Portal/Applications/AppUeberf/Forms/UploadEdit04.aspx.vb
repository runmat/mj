Imports CKG.Base.Kernel.Common.Common

Partial Public Class UploadEdit04
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    'Private m_App As Base.Kernel.Security.App
    Private dv As DataView
    'Private strError As String
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
            'm_App = New Base.Kernel.Security.App(m_User)

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
        End If

        If clsUeberf.Anschluss = False Then
            lblSchritt.Text = "Schritt 4 von 4"
        Else
            lblSchritt.Text = "Schritt 5 von 5"
        End If
        cmdBack0.Attributes.Add("onclick", "return ShowMessage()")
    End Sub

    Private Sub GetData()

        If clsUeberf Is Nothing Then
            clsUeberf = Session("App_Ueberf")
        End If

        With clsUeberf


            dv = Session("DataViewRG")

            If Not dv Is Nothing Then
                dv.RowFilter = "PARVW = 'RG' AND KUNNR = '" & .SelRegulierer & "'"

                .RegName = dv.Item(0)("NAME1") & " " & dv.Item(0)("NAME2")
                .RegStrasse = dv.Item(0)("STREET") & " " & dv.Item(0)("HOUSE_NUM1")
                .RegOrt = dv.Item(0)("POST_CODE1") & " " & dv.Item(0)("CITY1")

                lblRegName.Text = .RegName
                lblRegStrasse.Text = .RegStrasse
                lblRegOrt.Text = .RegOrt

                dv.RowFilter = "PARVW = 'RE' AND KUNNR = '" & .SelRechnungsempf & "'"

                .RechName = dv.Item(0)("NAME1") & " " & dv.Item(0)("NAME2")
                .RechStrasse = dv.Item(0)("STREET") & " " & dv.Item(0)("HOUSE_NUM1")
                .RechOrt = dv.Item(0)("POST_CODE1") & " " & dv.Item(0)("CITY1")
            End If

            lblRechName.Text = .RechName
            lblRechStrasse.Text = .RechStrasse
            lblRechOrt.Text = .RechOrt

            lblAbName.Text = .AbName
            lblAbStrasse.Text = .AbStrasse & " " & .AbNr
            lblAbOrt.Text = .AbPlz & " " & .AbOrt
            lblAbAnsprechpartner.Text = .AbAnsprechpartner
            lblAbTelefon.Text = .AbTelefon
            lblAbTelefon2.Text = .AbTelefon2
            lblAbFax.Text = .AbFax

            lblAnName.Text = .AnName
            lblAnStrasse.Text = .AnStrasse & " " & .AnNr
            lblAnOrt.Text = .AnPlz & " " & .AnOrt
            lblAnAnspechpartner.Text = .AnAnsprechpartner
            lblAnTelefon.Text = .AnTelefon
            lblAnTelefon2.Text = .AnTelefon2
            lblAnFax.Text = .AnFax

            lblHerst.Text = .Herst
            lblKennzeichen.Text = .Kenn1 & "-" & .Kenn2
            lblFahrzeugklasse.Text = .FahrzeugklasseTxt
            lblVin.Text = .Vin
            lblRef.Text = .Ref
            lblBem.Text = .Bemerkung

            lblZugelassen.Text = .FzgZugelassen
            lblBereifung.Text = .SomWin
            lblExpress.Text = .Express
            lblHinZulKCL.Text = .Hin_KCL_Zulassen

            lblDatumUeberf.Text = .DatumUeberf

            lblFahrzeugwert.Text = .FahrzeugwertTxt

            If .Tanken = True Then
                lblTanken.Text = "Ja"
            Else
                lblTanken.Text = "Nein"
            End If

            If .FzgEinweisung = True Then
                lblEinw.Text = "Ja"
            Else
                lblEinw.Text = "Nein"
            End If

            If .Waesche = True Then
                lblWW.Text = "Ja"
            Else
                lblWW.Text = "Nein"
            End If

            If .RotKenn = True Then
                lblRotKenn.Text = "Ja"
            Else
                lblRotKenn.Text = "Nein"
            End If


            If .Anschluss = False Then
                Table5.Visible = False
            Else
                lbl2ReName.Text = .ReName
                lbl2ReStrasse.Text = .ReStrasse & " " & .ReNr
                lbl2RePlzOrt.Text = .RePlz & " " & .ReOrt
                lbl2ReAnsprech.Text = .ReAnsprechpartner
                lbl2ReTelefon.Text = .ReTelefon
                lbl2ReTelefon2.Text = .ReTelefon2
                lbl2ReFax.Text = .ReFax
                lbl2ReHerst.Text = .ReHerst
                lbl2ReKenn.Text = .ReKenn1 & "-" & .ReKenn2
                lbl2ReFahrzeugklasse.Text = .ReFahrzeugklasseTxt
                lbl2ReVin.Text = .ReVin
                lbl2ReRef.Text = .ReRef
                lbl2ReZugelassen.Text = .ReFzgZugelassen
                lbl2ReBereif.Text = .ReSomWin
                'lblAnZulKCL.Text = .An_KCL_Zulassen
            End If


        End With
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack.Click
        If clsUeberf Is Nothing Then
            clsUeberf = Session("App_Ueberf")
        End If
        clsUeberf.Modus = 1
        Session("App_UebTexte") = Nothing
        Session("App_Ueberf") = clsUeberf
        Response.Redirect("UploadEdit03.aspx?AppID=" & Session("AppID").ToString)

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Dim RetTable As DataTable
        Dim strErr As String = ""

        If clsUeberf Is Nothing Then
            clsUeberf = Session("App_Ueberf")
        End If


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
            lblInfo.Text = clsUeberf.Message
            cmdSave.Visible = False
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

    Private Sub SetNothing()
        clsUeberf = Nothing
        Session("App_Ueberf") = Nothing
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click

        Try
            Dim imageHt As New Hashtable()
            imageHt.Add("Logo", m_User.Customer.LogoImage)

            clsUeberf.getVKBueroFooter(m_User.KUNNR)

            Dim tblData As DataTable = Base.Kernel.Common.DataTableHelper.ObjectToDataTable(clsUeberf)
            tblData.Rows(0).BeginEdit()
            Dim dr As DataRow
            dr = tblData.Rows(0)
            dr("Vbeln") = clsUeberf.Vbeln.TrimStart("0"c)
            tblData.Rows(0).EndEdit()
            tblData.Columns.Add("Datum", GetType(String))
            tblData.Rows(0).BeginEdit()
            dr = tblData.Rows(0)
            dr("Datum") = Today.ToShortDateString
            tblData.Rows(0).EndEdit()

            Dim docFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(tblData, imageHt)

            docFactory.CreateDocument(clsUeberf.Ref + "_Ueb", Me, "\Applications\AppUeberf\Documents\ÜberführungStandardPortal.doc")


        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
    Private Sub btnConfOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfOK.Click
        cmdSave_Click(sender, e)
    End Sub

    Private Sub btnConfCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfCancel.Click
        Response.Redirect("UploadUeberf.aspx?AppID=" & Session("AppID").ToString)
    End Sub
    Protected Sub cmdBack0_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdBack0.Click
        Response.Redirect("UploadUeberf.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class
' ************************************************
' $History: UploadEdit04.aspx.vb $
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


