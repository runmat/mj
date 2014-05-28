Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common


Public Class Report201_3
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

    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private m_objTable As DataTable

    Protected WithEvents Hyperlink2 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents ucStyles As Styles
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label9 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugkennzeichen As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBox1 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CheckBox2 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label16 As System.Web.UI.WebControls.Label
    Protected WithEvents Label17 As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents lblFahrzeugUndAufbauart As System.Web.UI.WebControls.Label
    Protected WithEvents Label19 As System.Web.UI.WebControls.Label
    Protected WithEvents Label23 As System.Web.UI.WebControls.Label
    Protected WithEvents Label24 As System.Web.UI.WebControls.Label
    Protected WithEvents Label25 As System.Web.UI.WebControls.Label
    Protected WithEvents lblHersteller As System.Web.UI.WebControls.Label
    Protected WithEvents lblTypUndAusfuehrung As System.Web.UI.WebControls.Label
    Protected WithEvents lblFIN As System.Web.UI.WebControls.Label
    Protected WithEvents Label20 As System.Web.UI.WebControls.Label
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents Label22 As System.Web.UI.WebControls.Label
    Protected WithEvents Label26 As System.Web.UI.WebControls.Label
    Protected WithEvents Label27 As System.Web.UI.WebControls.Label
    Protected WithEvents Label28 As System.Web.UI.WebControls.Label
    Protected WithEvents Label29 As System.Web.UI.WebControls.Label
    Protected WithEvents Label30 As System.Web.UI.WebControls.Label
    Protected WithEvents Label31 As System.Web.UI.WebControls.Label
    Protected WithEvents Label32 As System.Web.UI.WebControls.Label
    Protected WithEvents Label33 As System.Web.UI.WebControls.Label
    Protected WithEvents Label34 As System.Web.UI.WebControls.Label
    Protected WithEvents Label35 As System.Web.UI.WebControls.Label
    Protected WithEvents lblName As System.Web.UI.WebControls.Label
    Protected WithEvents Label36 As System.Web.UI.WebControls.Label
    Protected WithEvents Label37 As System.Web.UI.WebControls.Label
    Protected WithEvents Label38 As System.Web.UI.WebControls.Label
    Protected WithEvents Label39 As System.Web.UI.WebControls.Label
    Protected WithEvents Label40 As System.Web.UI.WebControls.Label
    Protected WithEvents lblWohnhaft As System.Web.UI.WebControls.Label
    Protected WithEvents Label41 As System.Web.UI.WebControls.Label
    Protected WithEvents Label42 As System.Web.UI.WebControls.Label
    Protected WithEvents Label43 As System.Web.UI.WebControls.Label
    Protected WithEvents Label44 As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents Label45 As System.Web.UI.WebControls.Label
    Protected WithEvents Label46 As System.Web.UI.WebControls.Label
    Private strKennzeichen As String


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)

        Try
            Dim AppName As String = Me.Request.Url.LocalPath
            AppName = Left(AppName, InStrRev(AppName, ".") - 1)
            AppName = Right(AppName, Len(AppName) - InStrRev(AppName, "/"))
            AppName = "AppName='" & AppName & "'"
            If Not AppName = "Selection" AndAlso _
               m_User.Applications.Select(AppName).Length = 0 Then
                Me.lblError.Visible = True
                'Me.Response.Redirect("../../../Start/Selection.aspx")
            Else
                If (Request.QueryString.Item("strKennzeichen") Is Nothing) Then
                    Me.lblError.Visible = True
                    'Me.Response.Redirect("../../../Start/Selection.aspx")
                Else
                    strKennzeichen = Request.QueryString.Item("strKennzeichen")
                    If (Session("ResultTable") Is Nothing) Then
                        Me.lblError.Visible = True
                        'Me.Response.Redirect("../../../Start/Selection.aspx")
                    Else
                        m_objTable = CType(Session("ResultTable"), DataTable)

                        m_App = New Base.Kernel.Security.App(m_User)

                        If Not IsPostBack Then
                            Dim rows As DataRow() = m_objTable.Select("[Kfz-Kennzeichen]='" & strKennzeichen & "'")
                            If rows.Length = 1 Then
                                If Not rows(0)("Kfz-Kennzeichen") Is Nothing AndAlso CStr(rows(0)("Kfz-Kennzeichen")).Trim.Length > 0 Then
                                    lblFahrzeugkennzeichen.Text = CStr(rows(0)("Kfz-Kennzeichen"))
                                End If
                                If Not rows(0)("Fahrzeugart") Is Nothing AndAlso CStr(rows(0)("Fahrzeugart")).Trim.Length > 0 Then
                                    lblFahrzeugUndAufbauart.Text = CStr(rows(0)("Fahrzeugart"))
                                End If
                                If Not rows(0)("Hersteller") Is Nothing AndAlso CStr(rows(0)("Hersteller")).Trim.Length > 0 Then
                                    lblHersteller.Text = CStr(rows(0)("Hersteller"))
                                End If
                                If Not rows(0)("Typ Schlüssel") Is Nothing AndAlso CStr(rows(0)("Typ Schlüssel")).Trim.Length > 0 Then
                                    lblTypUndAusfuehrung.Text = "Typ Schlüssel: " & CStr(rows(0)("Typ Schlüssel"))
                                End If
                                If Not rows(0)("Ausführung") Is Nothing AndAlso CStr(rows(0)("Ausführung")).Trim.Length > 0 Then
                                    If Left(lblTypUndAusfuehrung.Text, 1) = "T" Then
                                        lblTypUndAusfuehrung.Text &= ", Ausführung: " & CStr(rows(0)("Ausführung"))
                                    Else
                                        lblTypUndAusfuehrung.Text = "Ausführung: " & CStr(rows(0)("Ausführung"))
                                    End If
                                End If
                                If Not rows(0)("Fahrgestellnummer") Is Nothing AndAlso CStr(rows(0)("Fahrgestellnummer")).Trim.Length > 0 Then
                                    lblFIN.Text = CStr(rows(0)("Fahrgestellnummer"))
                                End If
                                If Not rows(0)("Name") Is Nothing AndAlso CStr(rows(0)("Name")).Trim.Length > 0 Then
                                    lblName.Text = CStr(rows(0)("Name"))
                                End If
                                If Not rows(0)("Postleitzahl") Is Nothing AndAlso CStr(rows(0)("Postleitzahl")).Trim.Length > 0 Then
                                    lblWohnhaft.Text = CStr(rows(0)("Postleitzahl"))
                                End If
                                If Not rows(0)("Ort") Is Nothing AndAlso CStr(rows(0)("Ort")).Trim.Length > 0 Then
                                    lblWohnhaft.Text &= " " & CStr(rows(0)("Ort"))
                                End If
                                If Not rows(0)("Straße") Is Nothing AndAlso CStr(rows(0)("Straße")).Trim.Length > 0 Then
                                    lblWohnhaft.Text &= ", " & CStr(rows(0)("Straße"))
                                End If
                                '§§§ JVE 26.01.2006 Checkboxen automatisch setzen.
                                If (rows(0)("Anzahl Schilder").ToString.Trim = "V") Then
                                    CheckBox1.Checked = False
                                End If
                                If (rows(0)("Anzahl Schilder").ToString.Trim = "H") Then
                                    CheckBox2.Checked = False
                                End If
                                '---------------------------------------------------
                            Else
                                Me.lblError.Visible = True
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Me.lblError.Visible = True
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Report201_3.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:07
' Created in $/CKAG/Applications/appecan/Forms
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 12:32
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 11:16
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Forms
' 
' ************************************************
