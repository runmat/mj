Option Explicit On 
Option Strict On

Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports System.Drawing

Public Class Change01_1
    Inherits System.Web.UI.Page

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label

    Protected WithEvents tbl_EingabeMaske As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents blubb As System.Web.UI.HtmlControls.HtmlTable

    Dim mObjVertragsdaten As Vertragsdaten
    Dim alControls As New ArrayList()

    Dim tmpControl As New Control()
    Dim tmpTxt As New TextBox()
    Dim tmpCb As New CheckBox()
    Dim tmpDTG As New DataGrid()
    Dim tmpLBX As New ListBox()
    Dim tmpDDL As New DropDownList()

    Private m_User As Base.Kernel.Security.User 'gefüllt bei GetUser()

    Protected WithEvents lb_Speichern As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lb_Verwerfen As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
   
    Protected WithEvents tableblabla As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable

    Protected WithEvents txt_D_LN_KUNNR As TextBox
    Protected WithEvents txt_D_LN_NAME1 As TextBox
    Protected WithEvents txt_D_LN_NAME2 As TextBox
    Protected WithEvents txt_D_LN_PSTLZ As TextBox
    Protected WithEvents txt_D_LN_STRAS As TextBox
    Protected WithEvents txt_D_LN_ORT01 As TextBox
    Protected WithEvents txt_D_HD_PSTLZ As TextBox
    Protected WithEvents txt_D_GST_KURZ As TextBox
    Protected WithEvents txt_D_GST_NAME As TextBox

    Protected WithEvents ddl_D_LN_LAND1 As DropDownList
    Protected WithEvents ddl_D_HD_LAND1 As DropDownList
    Protected WithEvents ddl_S_Geschaeftsstelle As DropDownList

    Protected WithEvents ibtRefresh As ImageButton
    Protected WithEvents ibtSearchLN As ImageButton
    Protected WithEvents ibtSearchKB As ImageButton
    
    'Private m_App As Base.Kernel.Security.App 'ist ein neues Objekt der klasse Base.Kernel.Security.App


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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            m_User = GetUser(Me)
            'm_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 

            ucHeader.InitUser(m_User)
            FormAuth(Me, m_User)
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            If mObjVertragsdaten Is Nothing Then
                If Session("objVertragsdatenSession") Is Nothing Then
                    Throw New Exception("benötigtiges SessionObjekt nicht vorhanden")
                Else
                    mObjVertragsdaten = CType(Session("objVertragsdatenSession"), Vertragsdaten)
                End If

            End If

            alControls.Clear() 'bei jedem postback
            fillArrayListWithControls(FindControl("tbl_EingabeMaske")) 'bei jedem postback

            If Not IsPostBack Then
                fillLaenderDLL(ddl_D_HD_LAND1)
                fillLaenderDLL(ddl_D_LN_LAND1)
                mObjVertragsdaten.FillControlElementsOrSAPTable(alControls, False)
                lb_Verwerfen.Attributes.Add("onclick", "return confirm('Ihre Änderungen sind noch nicht gespeichert, möchten Sie trotzdem fortfahren?');")
            Else 'wenn postback
                'SFA: Leider nicht immer gefüllt.
                'speichern der DDL-werte in die Unsichtbaren Textfelder
                If Not ddl_S_Geschaeftsstelle Is Nothing Then
                    If ddl_S_Geschaeftsstelle.Items.Count > 0 Then
                        txt_D_GST_NAME.Text = ddl_S_Geschaeftsstelle.SelectedItem.Text
                        txt_D_GST_KURZ.Text = Right(ddl_S_Geschaeftsstelle.SelectedValue, 6)
                    End If

                End If
            End If
        Catch ex As Exception
            lblError.Text = "Fehler beim laden der Seite: " & ex.Message.ToString
        End Try
    End Sub

    Private Sub setRequBorders()
        
        Dim width As New Unit(2)
        Dim bs As BorderStyle = BorderStyle.Solid
        Dim bc As Color = Color.Red
       

        With txt_D_LN_NAME1
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With


        With txt_D_LN_ORT01
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With

        With txt_D_LN_PSTLZ
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With

        With txt_D_LN_STRAS
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With

        With txt_D_LN_KUNNR
            .BorderColor = bc
            .BorderStyle = bs
            .BorderWidth = width
        End With


    End Sub

    Private Sub fillArrayListWithControls(ByVal mothercontrol As Control)
        'alle Contorls(txt,cb,DV, LBX) auf der seite werden in eine ArrayListe geschrieben, 
        'Dies muss jedes mal erneut passieren, weil komplette Objekte in der AL gesichert sind und keine Referenzen, 
        'so kann die AL auch nicht in einer Session abgelegt werden, weil sonst der Wert von den Objekt bezogen wird,
        ' wie Anfangs in der SessionArraylist hinzugefügt wurden JJ2007.11.15

        For Each tmpControl In mothercontrol.Controls
            'wenn das dataGrid eine zeile enthält würde es nicht in das ControlArray gesichert werden, dadurch könnte es auch nicht bei der Erfassung berücksichtigt werden. JJ2007.11.26
            If tmpControl.HasControls = True And (tmpControl.GetType Is tmpDTG.GetType) = False Then
                fillArrayListWithControls(tmpControl)
            Else
                If (tmpControl.GetType Is tmpCb.GetType()) = True OrElse
                    (tmpControl.GetType Is tmpTxt.GetType()) = True OrElse
                    (tmpControl.GetType Is tmpDTG.GetType) = True OrElse
                    (tmpControl.GetType Is tmpLBX.GetType) = True OrElse
                    (tmpControl.GetType Is tmpDDL.GetType) Then
                    alControls.Add(tmpControl)
                End If

            End If
        Next
    End Sub

    Private Sub lb_Verwerfen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lb_Verwerfen.Click
        Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Function plausibilityCheck(ByRef meldung As String) As Boolean
        meldung = ""

        If Not txt_D_HD_PSTLZ.Text.Trim(" "c).Length = 0 Then
            If CInt(mObjVertragsdaten.Laender.Select("Land1='" & ddl_D_HD_LAND1.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(mObjVertragsdaten.Laender.Select("Land1='" & ddl_D_HD_LAND1.SelectedItem.Value & "'")(0)("Lnplz")) = txt_D_HD_PSTLZ.Text.Trim(" "c).Length Then
                    meldung = meldung & "Postleitzahl des Händlers hat die falsche Länge." & "\n"
                End If
            End If
        End If

        If Not txt_D_LN_PSTLZ.Text.Trim(" "c).Length = 0 Then
            If CInt(mObjVertragsdaten.Laender.Select("Land1='" & ddl_D_LN_LAND1.SelectedItem.Value & "'")(0)("Lnplz")) > 0 Then
                If Not CInt(mObjVertragsdaten.Laender.Select("Land1='" & ddl_D_LN_LAND1.SelectedItem.Value & "'")(0)("Lnplz")) = txt_D_LN_PSTLZ.Text.Trim(" "c).Length Then
                    meldung = meldung & "Postleitzahl des Leasingnehmers hat die falsche Länge." & "\n"
                End If
            End If
        End If
        
        If Not meldung.Trim(" "c).Length = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Function checkRequiredFields() As Boolean
        If txt_D_LN_NAME1.Text Is String.Empty OrElse txt_D_LN_ORT01.Text Is String.Empty OrElse txt_D_LN_PSTLZ.Text Is String.Empty OrElse txt_D_LN_KUNNR.Text Is String.Empty OrElse txt_D_LN_STRAS.Text Is String.Empty Then
            Return False
        End If
        Return True
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Protected Sub lb_Speichern_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_Speichern.Click
        Try

            Dim meldung As String = ""
            
            If checkRequiredFields() = True Then
                If plausibilityCheck(meldung) = True Then

                    'leeren der aktuellen arrayliste 
                    alControls.Clear()
                    fillArrayListWithControls(FindControl("tbl_EingabeMaske"))
                    mObjVertragsdaten.FillControlElementsOrSAPTable(alControls, True)
                    mObjVertragsdaten.sendDataToSap(Session("AppID").ToString, Session.SessionID)
                    If Not mObjVertragsdaten.Status = 0 Then
                        lblError.Text = mObjVertragsdaten.Message
                        lblError.Visible = True
                    Else
                        Response.Redirect("Change01.aspx?AppID=" & Session("AppID").ToString)
                    End If
                Else
                    Response.Write("<script language='javascript'>alert('" & meldung & "');</script>")
                End If

            Else
                Response.Write("<script language='javascript'>alert('Füllen Sie bitte alle benötigten Felder');</script>")
                setRequBorders()
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

    Private Sub fillLaenderDLL(ByRef ddl_Land As DropDownList)
        Dim sprache As String
        'Länder DLL füllen
        ddl_Land.DataSource = mObjVertragsdaten.Laender
        'ddlLand.DataTextField = "Beschreibung"
        ddl_Land.DataTextField = "FullDesc"
        ddl_Land.DataValueField = "Land1"
        ddl_Land.DataBind()
        'vorbelegung der Länderddl auf Grund der im Browser eingestellten erstsprache JJ2007.12.06
        Dim tmpstr() As String
        If Request("HTTP_ACCEPT_Language").IndexOf(",") = -1 Then
            'es ist nur eine sprache ausgewählt
            sprache = Request("HTTP_ACCEPT_Language")
        Else
            'es gibt eine erst und eine zweitsprache
            sprache = Request("HTTP_ACCEPT_Language").Substring(0, Request("HTTP_ACCEPT_Language").IndexOf(","))
        End If

        tmpstr = sprache.Split(CChar("-"))
        'Länderkennzeichen setzen sich aus Region und Sprache zusammen. de-ch, de-at usw. leider werden bei Regionen in denen die Sprache das selbe Kürzel hat nur einfache Kürzel geschrieben, z.b. bei "de"
        If tmpstr.Length > 1 Then
            sprache = tmpstr(1).ToUpper
        Else
            sprache = tmpstr(0).ToUpper
        End If
        If mObjVertragsdaten.Vertragsdaten(0)(ddl_Land.ID.Substring(ddl_Land.ID.IndexOf("_", ddl_Land.ID.IndexOf("_") + 1) + 1)).ToString = "" Then
            ddl_Land.Items.FindByValue(sprache).Selected = True
        Else
            ddl_Land.Items.FindByValue(mObjVertragsdaten.Vertragsdaten(0)(ddl_Land.ID.Substring(ddl_Land.ID.IndexOf("_", ddl_Land.ID.IndexOf("_") + 1) + 1)).ToString).Selected = True
        End If
    End Sub

    Protected Sub ibtRefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtRefresh.Click
     
        Dim adressdaten As DataTable
        mObjVertragsdaten.GetLnKunde(txt_D_LN_KUNNR.Text.Trim(" "c))

        If mObjVertragsdaten.Status = 0 OrElse mObjVertragsdaten.Status = -1111 Then
            adressdaten = mObjVertragsdaten.Adressdaten
            If Not adressdaten Is Nothing Then
                With adressdaten.Rows(0)
                    txt_D_LN_NAME1.Text = .Item("NAME1_ZL").ToString
                    txt_D_LN_ORT01.Text = .Item("ORT01_ZL").ToString
                    txt_D_LN_PSTLZ.Text = .Item("PSTLZ_ZL").ToString
                    txt_D_LN_STRAS.Text = .Item("STRAS_ZL").ToString
                    ddl_D_LN_LAND1.SelectedValue = .Item("LAND1_ZL").ToString
                End With
                txt_D_LN_KUNNR.ForeColor = Color.Black
            Else
                txt_D_LN_KUNNR.ForeColor = Color.Red
            End If
        Else
            lblError.Text = mObjVertragsdaten.Message
            Exit Sub
        End If
    End Sub

    Protected Sub ibtSearchLN_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtSearchLN.Click
        mObjVertragsdaten.FillControlElementsOrSAPTable(alControls, True)
        Response.Redirect("Change01_Suchhilfe.aspx?AppID=" & Session("AppID").ToString & "&Kennung=HAENDLER")
    End Sub

    Protected Sub ibtSearchLN0_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtSearchKB.Click
        mObjVertragsdaten.FillControlElementsOrSAPTable(alControls, True)
        Response.Redirect("Change01_Suchhilfe.aspx?AppID=" & Session("AppID").ToString & "&Kennung=KB")
    End Sub

End Class
' ************************************************
' $History: Change01_1.aspx.vb $
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 29.04.09   Time: 15:26
' Updated in $/CKAG/Applications/AppAlphabet/forms
' 
' *****************  Version 11  *****************
' User: Fassbenders  Date: 12.12.08   Time: 11:13
' Updated in $/CKAG/Applications/AppAlphabet/forms
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 1.10.08    Time: 15:56
' Updated in $/CKAG/Applications/AppAlphabet/forms
' ITA 2254 nachbesserung
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 1.10.08    Time: 13:04
' Updated in $/CKAG/Applications/AppAlphabet/forms
' ITA 2254 testfertig
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 30.09.08   Time: 9:40
' Updated in $/CKAG/Applications/AppAlphabet/forms
' ITA 2254 unfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 20.08.08   Time: 15:43
' Updated in $/CKAG/Applications/AppAlphabet/forms
' Nachbesserungen
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 6.08.08    Time: 13:34
' Updated in $/CKAG/Applications/AppAlphabet/forms
' ITA 2133 nachbesserungen
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 6.08.08    Time: 10:48
' Updated in $/CKAG/Applications/AppAlphabet/forms
' ITA 2133 fertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.08.08    Time: 18:47
' Updated in $/CKAG/Applications/AppAlphabet/forms
' 2133 testfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.08.08    Time: 13:37
' Updated in $/CKAG/Applications/AppAlphabet/forms
' 2133 weiterentwicklung
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 4.08.08    Time: 13:26
' Updated in $/CKAG/Applications/AppAlphabet/forms
' Weiterentwiclung
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 4.08.08    Time: 11:52
' Created in $/CKAG/Applications/AppAlphabet/forms
' ITA 2133 unfertig
' 
' ************************************************
