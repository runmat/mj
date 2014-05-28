Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change02
    Inherits System.Web.UI.Page

#Region " Vom Web Form Designer generierter Code "

    'Dieser Aufruf ist für den Web Form-Designer erforderlich.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
        'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
        InitializeComponent()
    End Sub

#End Region

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtModelId As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBatchid As System.Web.UI.WebControls.TextBox
    Protected WithEvents trSelectDropdown As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents ddlHersteller As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ucStyles As Styles
    Private objBatch As ec_01
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents txtUnitNr As System.Web.UI.WebControls.TextBox
    Protected WithEvents rblBatch As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents cbxShowUnitNummern As System.Web.UI.WebControls.CheckBox
    Private objBatch2 As ec_02

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                'objBatch2 = New ec_02(m_User, m_App, "")
                'Session.Add("objSuche", objBatch)
                Initialload()
            Else
                'objBatch = CType(Session("objSuche"), ec_01)
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Initialload()
        Dim item As ListItem


        'alte Hersteller Auswahl JJ2008.1.7
        '------------------
        'objBatch = New ec_01(m_User, m_App, "")
        'objBatch.getData(Session("AppID").ToString, Session.SessionID)
        '------------------

        'Neue Herstellerauswahl nach z_m_Herstellergroup Bapi JJ2008.1.7
        objBatch2 = New ec_02(m_User, m_App, "")
        objBatch2.fillHerstellerDDL(Session("AppID").ToString, Session.SessionID, Me)


        If objBatch2.Status <> 0 Then
            lblError.Text = objBatch2.Message
        Else


            Dim tmpRow As DataRow
            Dim tmpStr As String

            item = New ListItem()
            item.Text = "-alle-"
            item.Value = 0


            ddlHersteller.Items.Add(item)

            For Each tmpRow In objBatch2.Hersteller.Rows
                'Herstellername ab öffnender Klammer abschneiden bei Übergabe "(" laut Achim JJ2008.1.7
                tmpStr = tmpRow(1)
                If Not tmpStr.IndexOf("(") = -1 Then
                    tmpStr = tmpStr.Remove(tmpStr.IndexOf("("), tmpStr.Length - tmpStr.IndexOf("("))
                End If
                item = New ListItem(tmpStr, tmpStr)
                ddlHersteller.Items.Add(item)
            Next

            'ursprüngliche befüllung der Hersteller DDL JJ2008.1.7
            '---------------------------------------
            'ddlHersteller.Items.Add(item)
            'tblHersteller = CType(objBatch.HerstellerAuswahl, DataTable)

            ''1. Hersteller füllen
            'vwHersteller = objBatch.HerstellerAuswahl.DefaultView
            'vwHersteller.Sort = "ZHERST asc"

            'item = New ListItem()
            'item.Text = "-alle-"
            'item.Value = 0

            'ddlHersteller.Items.Add(item)

            'For intIndex = 0 To objBatch.HerstellerAuswahl.Rows.Count - 1
            '    item = New ListItem()
            '    item.Value = CType(vwHersteller.Item(intIndex)("VALPOS"), String)
            '    item.Text = CType(vwHersteller.Item(intIndex)("ZHERST"), String)
            '    ddlHersteller.Items.Add(item)
            'Next
            '---------------------------------------

        End If
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        lblError.Text = ""
        'Selektionskriterienprüfung

        If rblBatch.SelectedIndex = 0 Or rblBatch.SelectedIndex = 1 Or rblBatch.SelectedIndex = 2 Then
            If txtBatchid.Text = "" And txtModelId.Text = "" And txtUnitNr.Text = "" And ddlHersteller.SelectedItem.Value = "0" Then
                lblError.Text = "Geben Sie mindestens ein weiteres Selektionskriterium an"
            Else
                doRealSubmit()
            End If
        Else
            doRealSubmit()
        End If



    End Sub

    Private Sub doRealSubmit()
        Dim objBatch2 As New ec_02(m_User, m_App, "")

        'Eingabewerte lesen...
        objBatch2.BarchId = txtBatchid.Text
        objBatch2.ModelID = txtModelId.Text
        objBatch2.UnitNr = txtUnitNr.Text

        If ddlHersteller.SelectedItem.Value = "0" Then
            objBatch2.HerstellerBezeichnung = String.Empty
        Else

            objBatch2.HerstellerBezeichnung = ddlHersteller.SelectedItem.Text

        End If

        'evtl später redundant wenns ums zurückschreiben ins sap geht
        'If (rblBatch.Items(1).Selected = True Or rblBatch.Items(5).Selected = True) Then
        '    objBatch2.Loeschvermerk = "X"
        'Else
        '    objBatch2.Loeschvermerk = String.Empty
        'End If


        objBatch2.SelectionCriteria = rblBatch.SelectedItem.Value
        objBatch2.withUnitNumbers = cbxShowUnitNummern.Checked


        objBatch2.getData(Session("AppID").ToString, Session.SessionID, Me)

        'Sperrdatum in typ Date wandeln
        If Not objBatch2.Result Is Nothing Then
            Dim alDatum As New ArrayList()
            alDatum.Add("Sperrdatum")
            objBatch2.ResultTable = HelpProcedures.FormatGridDateColumn(alDatum, objBatch2.Result)
            alDatum = Nothing
        End If



        If objBatch2.Status <> 0 Then
            lblError.Text = objBatch2.Message
            Exit Sub
        End If

        Session.Add("objSuche", objBatch2)

        Response.Redirect("Change02_2.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class

' ************************************************
' $History: Change02.aspx.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.06.09   Time: 16:04
' Updated in $/CKAG/Applications/appec/Forms
' ITA 2918 Z_M_Herstellergroup, Z_M_EC_AVM_BATCH_update,
' Z_M_EC_AVM_HERST_VWZWECK_MODID, Z_M_EC_AVM_BATCH_INSERT,
' Z_M_EC_AVM_BATCH_ANSICHT
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Forms
' Warnungen entfernt!
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Forms
' 
' *****************  Version 16  *****************
' User: Jungj        Date: 7.01.08    Time: 11:23
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' ITA 1358 Umstellung der Herstellerauswahl auf z_m_Herstellergroup
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 7.01.08    Time: 8:57
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Auswahl nach Herstellergruppen ermöglichen ITA 1358 Batch reporting
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 21.12.07   Time: 15:38
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' ITA 1358 vorbereitet für DDL mit Herstellergruppen
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 21.12.07   Time: 15:06
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' batchreporting ITA 1358
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 30.10.07   Time: 15:55
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 30.10.07   Time: 9:55
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 25.10.07   Time: 18:15
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 24.10.07   Time: 18:54
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 23.10.07   Time: 18:00
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 12:29
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 13:31
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
