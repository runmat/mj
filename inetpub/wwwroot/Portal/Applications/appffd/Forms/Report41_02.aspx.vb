Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business
Imports System.Data.SqlClient.SqlDataReader
Imports System.Web.UI.WebControls.Label
Imports System
Imports System.IO

Public Class Report41_02
    Inherits System.Web.UI.Page

    Private objSuche As Search
    Private m_User As Base.Kernel.Security.User
    Private objApp As Base.Kernel.Security.App
    Private objFDDBank As BankBaseCredit
    Private objFDDBank2 As FDD_Bank_2
    Private objFDDBank3 As FDD_Zahlungsfrist
    Private objDistrikt As FFD_Bank_Distrikt
    Private objtblKontingente As DataTable
    Private m_strMessage As String
    Protected WithEvents ucHeader As Header
    Protected WithEvents ucStyles As Styles
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents lblPageTitle As System.Web.UI.WebControls.Label
    Protected WithEvents cmdSelect As System.Web.UI.WebControls.LinkButton
    Protected WithEvents cmdBack As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents trHaendlernummer As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trName As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblHDNummer As System.Web.UI.WebControls.Label
    Protected WithEvents lblName As System.Web.UI.WebControls.Label
    Protected WithEvents lblDistrikt As System.Web.UI.WebControls.Label
    Protected WithEvents trOrt As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trDistrikt As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblAdresse As System.Web.UI.WebControls.Label
    Protected WithEvents CheckBox3 As System.Web.UI.WebControls.CheckBox
    Protected WithEvents lblseit As System.Web.UI.WebControls.Label
    Protected WithEvents Tr2 As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Datagrid2 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Datagrid4 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Datagrid3 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid
    Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink
    Protected WithEvents trHdAuswahl As System.Web.UI.HtmlControls.HtmlTableRow

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
        FormAuth(Me, m_User)

        Try
            objApp = New Base.Kernel.Security.App(m_User)

            If (Not IsPostBack) Or (Session("objFDDBank2") Is Nothing) Then
                objSuche = CType(Session("objSuche"), Search)
                If objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString) Then
                    lblHDNummer.Text = objSuche.REFERENZ
                    Dim strTemp As String = objSuche.NAME
                    If objSuche.NAME_2.Length > 0 Then
                        strTemp &= "<br>" & objSuche.NAME_2
                    End If
                    lblName.Text = strTemp
                    lblAdresse.Text = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
                Else
                    lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
                End If

                'Daten aus SAP laden
                ' Kontingente
                objFDDBank2 = New FDD_Bank_2(m_User, objApp, Session("AppID").ToString, Session.SessionID.ToString, "")
                objFDDBank2.AppID = Session("AppID").ToString
                objFDDBank2.CreditControlArea = "ZDAD"
                objFDDBank2.Filiale = objSuche.HaendlerFiliale
                objFDDBank2.Customer = m_User.KUNNR
                objFDDBank2.Fill(Session("AppID").ToString, Session.SessionID, Me)
                objFDDBank2.Haendler = Session("SelectedDealer").ToString

                objFDDBank = New BankBaseCredit(m_User, objApp, Session("AppID").ToString, Session.SessionID.ToString, "")
                objFDDBank.CreditControlArea = "ZDAD"
                objFDDBank.Customer = Session("SelectedDealer").ToString
                objFDDBank.Show()
                Kontingente(objFDDBank.Kontingente, "1")
                Kontingente(objFDDBank.Kontingente, "3")
                '---

                ' Fälligkeit
                objFDDBank3 = New FDD_Zahlungsfrist(m_User, objApp, Session("AppID").ToString, Session.SessionID.ToString, "")
                objFDDBank3.CreditControlArea = "ZDAD"
                objFDDBank3.Customer = "60" & Session("SelectedDealer").ToString
                objFDDBank3.Show(Session("AppID").ToString, Session.SessionID, Me)
                Session("objFDDBank3") = objFDDBank3

                Datagrid2.DataSource = objFDDBank3.Zahlungsfristen
                If objFDDBank3.Zahlungsfristen.Rows.Count > 0 Then Datagrid2.DataBind()
                ' ---

                ' Regionalbüro
                objDistrikt = New FFD_Bank_Distrikt(m_User, objApp, Session("AppID").ToString, Session.SessionID.ToString)
                objDistrikt.Haendler = Session("SelectedDealer")
                objDistrikt.Show(Session("AppID").ToString, Session.SessionID, Me)
                lblDistrikt.Text = objDistrikt.Distrikt
                Session("App_objDistrikt") = objDistrikt
                ' ---

                ' gesamt berechnen
                calcSum()
                '---
            ElseIf Not Session("objFDDBank2") Is Nothing AndAlso Not Session("objFDDBank") Is Nothing _
                   AndAlso Not Session("objFDDBank3") AndAlso Not Session("App_objDistrikt") Then
                objFDDBank2 = CType(Session("objFDDBank2"), FDD_Bank_2)
                objFDDBank = CType(Session("objFDDBank"), BankBaseCredit)
                objFDDBank3 = CType(Session("objFDDBank3"), FDD_Zahlungsfrist)
                objDistrikt = CType(Session("App_objDistrikt"), FFD_Bank_Distrikt)
                lblHDNummer.Text = objSuche.REFERENZ

                Dim strTemp As String = objSuche.NAME
                If objSuche.NAME_2.Length > 0 Then
                    strTemp &= "<br>" & objSuche.NAME_2
                End If

                lblName.Text = strTemp
                lblAdresse.Text = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
                Kontingente(objFDDBank.Kontingente, "1")
                Kontingente(objFDDBank.Kontingente, "3")
                Datagrid2.DataSource = objFDDBank3.Zahlungsfristen
                Datagrid2.DataBind()
                lblDistrikt.Text = objDistrikt.Distrikt
                calcSum()
            Else
                lblError.Text = "Fehler beim Bereitstellen der Daten!"
            End If


        Catch ex As Exception
            lblError.Text = "Fehler beim Bereitstellen der Daten!"
        End Try
    End Sub

    Private Sub calcSum()


        Dim intKreditlimit As Int32
        Dim intAusschoepfung As Int32
        Dim item As DataGridItem
        Dim cell As TableCell
        Dim label As Label
        Dim control As Control
        Try
            For Each item In DataGrid1.Items
                cell = item.Cells(2)                            'Spalte Kontingent
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        If label.Visible = True Then      '
                            intKreditlimit += CInt(label.Text)
                        End If
                    End If
                Next
                intAusschoepfung += CInt(item.Cells(3).Text) 'Spalte Inanspruchname
            Next
            For Each item In Datagrid3.Items
                cell = item.Cells(2)                            'Spalte Kontingent
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        If label.Visible = True Then
                            intKreditlimit += CInt(label.Text)
                        End If
                    End If
                Next
                intAusschoepfung += CInt(item.Cells(3).Text) 'Spalte Inanspruchname
            Next

            Dim SumTable As New DataTable()

            SumTable.Columns.Add("Gesamt", System.Type.GetType("System.String"))
            SumTable.Columns.Add("Kontingente", System.Type.GetType("System.String"))
            SumTable.Columns.Add("Inanspruchnahme", System.Type.GetType("System.String"))
            SumTable.Columns.Add("DummyCol1", System.Type.GetType("System.String"))
            SumTable.Columns.Add("DummyCol2", System.Type.GetType("System.String"))
            Dim NewRow As DataRow
            NewRow = SumTable.NewRow

            NewRow("Gesamt") = ""
            NewRow("Kontingente") = intKreditlimit
            NewRow("Inanspruchnahme") = intAusschoepfung
            SumTable.Rows.Add(NewRow)

            Datagrid4.DataSource = SumTable
            Datagrid4.DataBind()
            Session("App_GridGesamt") = Datagrid4
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Kontingente(ByVal Value As DataTable, ByVal sKKber As String)
        Dim strTemp As String = ""
        Dim temptbl As DataTable
        Dim darows() As DataRow
        Dim darow As DataRow
        Dim TempDatagrid As New DataGrid()
        objtblKontingente = Value
        temptbl = objtblKontingente.Copy

        If sKKber = "1" Then
            darows = temptbl.Select("Kreditkontrollbereich<>'0001' AND Kreditkontrollbereich<>'0002'")
            TempDatagrid = DataGrid1
            For Each darow In darows
                temptbl.Rows.Remove(darow)
            Next
        ElseIf sKKber = "3" Then
            darows = temptbl.Select("Kreditkontrollbereich='0001' OR Kreditkontrollbereich='0002'")
            TempDatagrid = Datagrid3
            For Each darow In darows
                temptbl.Rows.Remove(darow)
            Next
        End If

        If (Not (temptbl Is Nothing)) AndAlso (Not (temptbl.Rows.Count = 0)) Then
            lblMessage.CssClass = "LabelExtraLarge"

            TempDatagrid.DataSource = temptbl
            TempDatagrid.DataBind()

            Dim intKreditlimit As Int32
            Dim intAusschoepfung As Int32
            Dim blnGesperrt As Boolean

            Dim item As DataGridItem
            Dim cell As TableCell
            Dim chkBox As CheckBox
            Dim label As Label
            Dim control As Control

            For Each item In TempDatagrid.Items                    'Zeilen des DataGrids durchgehen...
                Dim blnZeigeKontingent As Boolean
                cell = item.Cells(0)                            'Erste Spalte holen = CheckBox (nicht sichtbar)
                For Each control In cell.Controls               'bestimmen, ob die jew. Kontingentart gezeigt werden soll...
                    If TypeOf control Is CheckBox Then
                        chkBox = CType(control, CheckBox)
                        blnZeigeKontingent = chkBox.Checked     'Die checkBox wird bereits durch DataBinding gesetzt, in Abhängigkeit des Wertes in "ZeigeKontingentArt"!
                    End If
                Next
                cell = item.Cells(2)                            'Spalte Kontingent
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        If label.ID = "Label1" And blnZeigeKontingent Then      'Label1 = nimmt Wert für Kontingent_Alt auf
                            label.Visible = True
                            intKreditlimit = CInt(label.Text)

                        Else
                            label.Visible = False
                            If label.ID = "Label2" And Not blnZeigeKontingent Then    'Label2 = nimmt Wert für Richtwert_Alt auf
                                intKreditlimit = CInt(label.Text)
                                If m_User.Reference.Length = 0 Then
                                    label.Visible = True
                                Else
                                    label.Visible = False
                                    If intKreditlimit = 0 Then
                                        item.Visible = False
                                    End If
                                End If
                            Else
                                label.Visible = False
                            End If
                        End If
                    End If

                Next
                intAusschoepfung = CInt(item.Cells(3).Text)
                cell = item.Cells(4)
                For Each control In cell.Controls
                    If TypeOf control Is Label Then
                        label = CType(control, Label)
                        If label.ID = "Label3" And blnZeigeKontingent Then
                            label.Visible = True
                        Else
                            label.Visible = False
                        End If
                    End If
                Next

                cell = item.Cells(5)
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chkBox = CType(control, CheckBox)
                        blnGesperrt = chkBox.Checked
                        If blnZeigeKontingent Then
                            chkBox.Visible = True
                        Else
                            chkBox.Visible = False
                        End If
                    End If
                Next

                If blnGesperrt Or (intAusschoepfung > intKreditlimit) And blnZeigeKontingent Then
                    For Each cell In item.Cells
                        cell.ForeColor = System.Drawing.Color.Red
                    Next
                End If
            Next
        End If

        darows = temptbl.Select("Lastschrift<>0")
        If darows.Length > 0 Then
            CheckBox3.Checked = True
            Session("AppLastschrift") = True
        Else
            Session("AppLastschrift") = False
        End If
        lblMessage.Text = m_strMessage

        If Not strTemp.Length = 0 Then
            If Not lblMessage.Text.Length = 0 Then
                lblMessage.Text &= "<br>"
            End If
            lblMessage.Text &= strTemp
        End If
        If sKKber = "1" Then
            DataGrid1 = TempDatagrid
            Session("App_GridKont1") = TempDatagrid
        ElseIf sKKber = "3" Then
            Datagrid3 = TempDatagrid
            Session("App_GridKont3") = TempDatagrid
        End If
    End Sub

    Private Sub cmdSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelect.Click
        Response.Redirect("Report41_03.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
        Response.Redirect("Report41.aspx?AppID=" & Session("AppID").ToString)
    End Sub
End Class
