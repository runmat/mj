Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Vorerf02_Print
    Inherits System.Web.UI.Page
    Protected WithEvents lblDate As System.Web.UI.WebControls.Label
    Protected WithEvents imgLogo As System.Web.UI.WebControls.Image
    Protected WithEvents dataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents btnPrintPDF As System.Web.UI.WebControls.Button
    Protected WithEvents btnPrint As System.Web.UI.HtmlControls.HtmlInputButton
    Private m_User As Base.Kernel.Security.User

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
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        Try
            btnPrint.Visible = False
            m_User = GetUser(Me)
            lblDate.Text = Now.ToShortDateString
            imgLogo.ImageUrl = m_User.Customer.CustomerStyle.LogoPath
            Dim ReportTable As DataTable = CType(Session("ReportTable"), DataTable)
            
            With dataGrid
                .DataSource = ReportTable
                .DataBind()
            End With
        Catch
            Dim scriptBuilder As New System.Text.StringBuilder()
            With scriptBuilder
                .Append("<script languange=""Javascript""><!--")
                .Append(ControlChars.CrLf)
                .Append("window.close();")
                .Append(ControlChars.CrLf)
                .Append("--></script>")
                .Append(ControlChars.CrLf)
            End With
            ClientScript.RegisterClientScriptBlock(Me.GetType, "CLOSE", scriptBuilder.ToString)
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

    Private Sub btnPrintPDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintPDF.Click

        'If clsUeberf.Beauftragung = clsUeberf.Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
        '    Response.Write("<script>window.open('ZulUebPrint.aspx?AppID=" & Session("AppID").ToString & "')</script>")
        'Else
        '    Response.Write("<script>window.open('UeberfPrint.aspx?AppID=" & Session("AppID").ToString & "')</script>")
        'End If
        Try
            Dim imageHt As New Hashtable()
            Dim ReportTable As DataTable = CType(Session("ReportTable"), DataTable)
            ReportTable.TableName = "Vorerfassung"
            Dim headTable As New DataTable("Kopf")
            'headTable.Columns.Add("customer", System.Type.GetType("System.String"), m_User.Customer.CustomerName.Trim)
            'headTable.Columns.Add("Date", System.Type.GetType("System.Date"), Today.ToShortTimeString)
            headTable.Columns.Add("customer", System.Type.GetType("System.String"))
            headTable.Columns.Add("Date", System.Type.GetType("System.String"))
            Dim dRow As DataRow = headTable.NewRow
            dRow("customer") = m_User.Customer.CustomerName.Trim
            dRow("Date") = Today.ToShortDateString
            headTable.Rows.Add(dRow)

            imageHt.Add("Logo", m_User.Customer.LogoImage)
            Dim docFactory As New Base.Kernel.DocumentGeneration.WordDocumentFactory(ReportTable, imageHt)
            docFactory.CreateDocumentTable("Vorerfassung Übersicht", Me.Page, "\Applications\AppKroschke\Documents\Vorerfassung_Übersicht.doc", headTable)

        Catch ex As Exception
            'lblError.Text = "Fehler beim Erstellen des Ausdrucks: " + ex.Message
        End Try

    End Sub
    Private Function CustomerLogoImage() As IO.MemoryStream
        Dim fs As New IO.FileStream("C:\Inetpub\wwwroot" & Replace(m_User.Customer.CustomerStyle.LogoPath, "/", "\"), IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
        Dim br As New IO.BinaryReader(fs)
        Dim ms As New IO.MemoryStream(br.ReadBytes(fs.Length))
        br.Close()
        fs.Close()
        Return ms
    End Function
End Class

' ************************************************
' $History: Vorerf02_Print.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 13:46
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 14:30
' Created in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 18.02.08   Time: 16:14
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' Bugfix ORUDO
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 9.07.07    Time: 15:17
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 6.07.07    Time: 14:29
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' ITA: 1130
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 13:07
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 14.03.07   Time: 15:49
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' IT 928 - Unterschriftsbalken hinzugefügt
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 9:25
' Updated in $/CKG/Applications/AppKroschke/AppKroschkeWeb/Forms
' 
' ************************************************
