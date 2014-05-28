Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common
Imports System.Data.SqlClient

Public Class ALDMailversand
    Inherits System.Web.UI.Page
    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles

#Region " Declarations"
    Protected WithEvents cmdCreate As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents txtVon As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents txtBis As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
#End Region

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
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        ucStyles.TitleText = ""
        'AdminAuth(Me, m_User, AdminLevel.Organization)

        GetAppIDFromQueryString(Me)

        If Not IsPostBack Then
            txtVon.Text = Now.ToShortDateString
            txtBis.Text = Now.ToShortDateString
        End If

        Try
            m_App = New Base.Kernel.Security.App(m_User)
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreate.Click
        Dim table As DataTable
        Dim status As String = ""

        table = getData(status)
        If (status = String.Empty) Then
            If Session("ResultTable") Is Nothing Then
                Session.Add("ResultTable", table)
            Else
                Session("ResultTable") = table
            End If
            Session("lnkExcel") = ""
            Response.Redirect("/Portal/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & CStr(Session("AppID")))
        Else
            lblError.Text = status
        End If
    End Sub

    Private Function getData(ByRef status As String) As DataTable
        Dim sqlCommand As SqlClient.SqlCommand
        Dim sqlConnection As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim adapter As SqlClient.SqlDataAdapter
        Dim table As New DataTable()
        Dim tableOutput As New DataTable()
        Dim datVon As Date
        Dim datBis As Date
        Dim row As DataRow
        Dim rowOut As DataRow

        status = String.Empty
        Try
            datVon = CType(txtVon.Text, Date)
            datVon = datVon.Subtract(New TimeSpan(1, 0, 0, 0))
            datBis = CType(txtBis.Text, Date)
            datBis = datBis.Add(New TimeSpan(1, 0, 0, 0))
        Catch ex As Exception
            status = "Falsche Eingabeparameter."
            Return Nothing
            Exit Function
        End Try

        sqlCommand = New SqlClient.SqlCommand()
        With sqlCommand
            .CommandText = "SELECT * FROM Abmeldebescheinigungen WHERE Bemerkung<>'Vorgank OK.' AND DatensatzAnlage BETWEEN CAST(@DatumVon as DATETIME) AND CAST(@DatumBis as DATETIME)"
            .Parameters.AddWithValue("@DatumVon", datVon.ToShortDateString)
            .Parameters.AddWithValue("@DatumBis", datBis.ToShortDateString)
            .Connection = sqlConnection
        End With

        adapter = New SqlClient.SqlDataAdapter(sqlCommand)
        Try
            sqlConnection.Open()
            adapter.Fill(table)

            With table.Columns
                .Remove("ID")
                .Remove("SMTP_ADDR")
                .Remove("AnhangPfad")
            End With

            For Each row In table.Rows
                If (row("Bemerkung").ToString = "Vorgank OK.") Then
                    row("Bemerkung") = "Vorgang OK."
                End If
            Next
            table.AcceptChanges()

            'Ausgabetabelle erstellen
            '1. Spalten erzeugen
            With tableOutput.Columns
                .Add("Erstellung", GetType(System.String))
                .Add("Kunden-Nr.", GetType(System.String))
                .Add("Kennzeichen", GetType(System.String))
                .Add("LV-Nr.", GetType(System.String))
                .Add("AnhangGesucht", GetType(System.String))
                .Add("AnhangGefunden", GetType(System.String))
                .Add("SendeVersuch", GetType(System.String))
                .Add("SendeErfolg", GetType(System.String))
                .Add("SAPQuittung", GetType(System.String))
                .Add("Status", GetType(System.String))
            End With
            tableOutput.AcceptChanges()

            '2. Zeilen anfügen
            For Each row In table.Rows
                rowOut = tableOutput.NewRow()
                rowOut("Erstellung") = row("DatensatzAnlage")
                rowOut("Kunden-Nr.") = row("ZZKUNNR_ZK")
                rowOut("Kennzeichen") = row("ZZKENN")
                rowOut("LV-Nr.") = row("LIZNR")
                If row("AnhangGesucht").ToString = "True" Then
                    rowOut("AnhangGesucht") = "&#215;"
                End If
                If row("AnhangGefunden").ToString = "True" Then
                    rowOut("AnhangGefunden") = "&#215;"
                End If
                If row("SendeVersuch").ToString = "True" Then
                    rowOut("SendeVersuch") = "&#215;"
                End If
                If row("SendeErfolg").ToString = "True" Then
                    rowOut("SendeErfolg") = "&#215;"
                End If
                If row("SAPQuittung").ToString = "True" Then
                    rowOut("SAPQuittung") = "&#215;"
                End If
                rowOut("Status") = row("Bemerkung")

                tableOutput.Rows.Add(rowOut)
                tableOutput.AcceptChanges()
            Next

            table = tableOutput

        Catch ex As Exception
            lblError.Text = "Fehler beim Lesen der Daten!"
        Finally
            sqlConnection.Close()
            sqlConnection.Dispose()
        End Try
        Return table
    End Function

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: ALDMailversand.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 14:58
' Updated in $/CKAG/Applications/appALD/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:15
' Created in $/CKAG/Applications/appald/Forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 6.12.07    Time: 13:15
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' ITA: 1440
' 
' *****************  Version 3  *****************
' User: Uha          Date: 20.06.07   Time: 15:25
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 2  *****************
' User: Uha          Date: 23.05.07   Time: 10:43
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Forms
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 1  *****************
' User: Uha          Date: 15.03.07   Time: 13:08
' Created in $/CKG/Applications/AppALD/AppALDWeb/Forms
' ALDMailversand hinzugefügt
' 
' ************************************************
