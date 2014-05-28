Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements

Partial Public Class Change05
    Inherits System.Web.UI.Page

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Protected WithEvents ucStyles As Global.CKG.Portal.PageElements.Styles
    Protected WithEvents ucHeader As Global.CKG.Portal.PageElements.Header

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

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click

        lblError.Text = String.Empty

        If Len(Trim(txtFahrgestellnummer.Text)) = 0 AndAlso _
            Len(Trim(txtKennzeichen.Text)) = 0 AndAlso _
            Len(Trim(txtOrdernummer.Text)) = 0 Then

            lblError.Text = "Bitte geben Sie mindestens ein Kriterium an."
            Exit Sub
        End If

        txtKennzeichen.Text = txtKennzeichen.Text.ToUpper

        Dim Search As New AvisChange05(m_User, m_App, "")

        Dim TempTable As DataTable = Search.SearchABMDat(Trim(txtFahrgestellnummer.Text), _
                                                         Trim(txtKennzeichen.Text), _
                                                         Trim(txtOrdernummer.Text))

        If TempTable Is Nothing Then
            TempTable = New DataTable
        End If

        If TempTable.Rows.Count > 0 Then

            lblPFahrgestellnummer.Text = TempTable.Rows(0)("CHASSIS_NUM").ToString
            lblPDatumGeplAbmeldung.Text = TempTable.Rows(0)("ZZABMDAT").ToString
            lblPDatumZul.Text = TempTable.Rows(0)("REPLA_DATE").ToString
            lblPHersteller.Text = TempTable.Rows(0)("MAKE_CODE").ToString
            lblPKennzeichen.Text = TempTable.Rows(0)("LICENSE_NUM").ToString
            lblPModellbezeichnung.Text = TempTable.Rows(0)("MODELL").ToString
            lblPTyp.Text = TempTable.Rows(0)("TYP").ToString
            lblPEqui.Text = TempTable.Rows(0)("EQUNR").ToString

            pnlAusgabe.Visible = True
            cmdFreigeben.Visible = True
        Else
            lblError.Text = "Es wurden keine Daten gefunden."
        End If



    End Sub

    Protected Sub cmdFreigeben_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdFreigeben.Click


        Dim Save As New AvisChange05(m_User, m_App, "")
        Dim SAPTable As DataTable = Save.GetImportTableForSave()

        'Dim SAPTable As New DataTable
        'With SAPTable
        '    .Columns.Add("EQUNR", System.Type.GetType("System.String"))
        '    .Columns.Add("RETUR_BEM", System.Type.GetType("System.String"))
        'End With

        Dim Row As DataRow = SAPTable.NewRow
        Row("EQUNR") = Right("000000000000000000" & lblPEqui.Text, 18)
        SAPTable.Rows.Add(Row)


        Dim ReturnTable As DataTable = Save.SaveABMDat()


        If ReturnTable.Rows.Count = 0 Then
            lblError.Text = "Daten gesichert."
            pnlAusgabe.Visible = False
            cmdFreigeben.Visible = False
        Else

            lblError.Text = "Fehler beim Speichern."
        End If



    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Start/Selection.aspx")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class

' ************************************************
' $History: Change05.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 25.11.08   Time: 15:20
' Updated in $/CKAG/Applications/AppAvis/forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 25.11.08   Time: 13:23
' Created in $/CKAG/Applications/AppAvis/forms
'