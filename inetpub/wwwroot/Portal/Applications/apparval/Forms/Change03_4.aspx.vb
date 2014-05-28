Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Public Class Change03_4
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
    'Private m_App As Base.Kernel.Security.App
    Private objZulassung As Arval_1

    Protected WithEvents cmdSave As LinkButton
    Protected WithEvents ucHeader As Header
    Protected WithEvents DataGrid1 As DataGrid
    Protected WithEvents Button1 As Button
    Protected WithEvents lblMessage As Label
    Protected WithEvents ucStyles As Styles
    Protected WithEvents btnPrint As HyperLink
    Protected WithEvents lblError As Label
    Protected WithEvents txtVersicherung As TextBox
    Protected WithEvents lblPageTitle As Label
    Protected WithEvents lblHead As Label
    Protected WithEvents lnkFahrzeugAuswahl As HyperLink
    Protected WithEvents lnkAdressAuswahl As HyperLink
    Protected WithEvents Label3 As Label

    Private blnVollst As Boolean

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        lnkFahrzeugAuswahl.NavigateUrl = "Change03_2.aspx?AppID=" & Session("AppID").ToString

        'lnkFahrzeugsuche.NavigateUrl = "Change03.aspx?AppID=" & Session("AppID").ToString
        'lnkAdressAuswahl.NavigateUrl = "Change03_3.aspx?AppID=" & Session("AppID").ToString

        GetAppIDFromQueryString(Me)
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            'm_App = New Base.Kernel.Security.App(m_User)

            If Session("objZulassung") Is Nothing Then
                Response.Redirect("Change03.aspx?AppID=" & Session("AppID").ToString)
            Else
                objZulassung = CType(Session("objZulassung"), Arval_1)
            End If

            If Not IsPostBack Then
                InitialLoad()
            Else
                CheckGrid()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub InitialLoad()
        Dim row As DataRow
        Dim table As DataTable
        Dim strWuk As String

        table = objZulassung.Result
        strWuk = String.Empty

        For Each row In table.Rows
            If (row("Wuk01_Buchstaben").ToString <> String.Empty) And (row("Wuk02_Buchstaben").ToString <> String.Empty) Then
                strWuk = CStr(row("Wuk01_Buchstaben").ToString.ToUpper & row("Wuk01_Ziffern").ToString & "," &
                    row("Wuk02_Buchstaben").ToString.ToUpper & row("Wuk02_Ziffern").ToString)
            Else
                If row("Wuk01_Buchstaben").ToString = String.Empty Then
                    strWuk = CStr(row("Wuk02_Buchstaben").ToString.ToUpper & row("Wuk02_Ziffern").ToString)
                End If
                If row("Wuk02_Buchstaben").ToString = String.Empty Then
                    strWuk = CStr(row("Wuk01_Buchstaben").ToString.ToUpper & row("Wuk01_Ziffern").ToString)
                End If
            End If

            If (strWuk <> String.Empty) And (row("Reservierungsdaten").ToString <> String.Empty) Then
                strWuk &= ";(Res.Info.:" & row("Reservierungsdaten").ToString & ")"
            End If

            row("Wunschkennzeichen") = strWuk
            If txtVersicherung.Text.Length = 0 AndAlso row("EVB_TXT").ToString.Length > 0 Then
                txtVersicherung.Text = row("EVB_TXT").ToString
            End If
        Next
        table.AcceptChanges()

        FillGrid()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        DoSubmit()
    End Sub

    Private Sub disableAll()
        cmdSave.Enabled = False
        DataGrid1.Enabled = False
        txtVersicherung.Enabled = False
    End Sub
    
    Private Sub DoSubmit()
        Dim row As DataRow
        Dim logApp As Base.Kernel.Logging.Trace
        logApp = CType(Session("objLog"), Base.Kernel.Logging.Trace)

        If Not blnVollst Then
            lblError.Text = "Angaben für abweichende Versandanschrift unvollständig."
            FillGrid()
        Else
            objZulassung = CType(Session("objZulassung"), Arval_1)
            objZulassung.Versicherung = txtVersicherung.Text
            disableAll()

            For Each row In objZulassung.Result.Rows
                If CType(row("Ausgewaehlt"), Boolean) = True Then
                    objZulassung.rowChange = row

                    If objZulassung.AbwVersandadresse = True Then

                        row("HaendlerKunnr") = String.Empty
                        row.AcceptChanges()

                    End If
                    objZulassung.Change(Session("AppID").ToString, Session.SessionID.ToString)
                    If objZulassung.Message <> "Vorgang OK" Then
                        logApp.UpdateEntry("ERR", Session("AppID").ToString, "Fehler in <Zulassung>. Fehler: " & objZulassung.Message & ")")
                    Else
                        logApp.UpdateEntry("APP", Session("AppID").ToString, "Zulassung")
                    End If
                End If
            Next

            FillGrid()
            btnPrint.Enabled = True
        End If
    End Sub

    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    DoSubmit()
    'End Sub

    Private Sub CheckGrid()
        Dim row As DataRow
        Dim i As Integer
        Dim j As Integer
        Dim AvName As String
        Dim AvStr As String
        Dim AvNr As String
        Dim AvPlz As String
        Dim AvOrt As String

        Dim equ As String
        Dim equipment As String

        blnVollst = True

        For Each row In objZulassung.Result.Rows
            equ = row("EquipmentNummer").ToString

            For i = 0 To Request.Form.Count - 1
                'Gesetzte Checkbox suchen...
                If InStr(Request.Form.Keys.Item(i), equ & ".cbx") > 0 Then
                    equipment = Right(Request.Form.Keys.Item(i), 21)
                    equipment = Left(equipment, equipment.IndexOf("."))
                    row("Versandadresse") = "X"
                    'Werte suchen...
                    For j = 0 To Request.Form.Count - 1
                        If InStr(Request.Form.Keys.Item(j), equipment & ".name") > 0 Then
                            AvName = Request.Form.Item(j).ToString()
                            row("HaendlerName1") = AvName
                            row.AcceptChanges()
                            If AvName.Trim.Length = 0 Then
                                blnVollst = False
                            End If
                        End If
                        If InStr(Request.Form.Keys.Item(j), equipment & ".str") > 0 Then
                            AvStr = Request.Form.Item(j).ToString()
                            row("HaendlerStr") = AvStr
                            row.AcceptChanges()
                            If AvStr.Trim.Length = 0 Then
                                blnVollst = False
                            End If
                        End If
                        If InStr(Request.Form.Keys.Item(j), equipment & ".nr") > 0 Then
                            AvNr = Request.Form.Item(j).ToString()
                            row("HaendlerNr") = AvNr
                            row.AcceptChanges()
                            If AvNr.Trim.Length = 0 Then
                                blnVollst = False
                            End If
                        End If
                        If InStr(Request.Form.Keys.Item(j), equipment & ".plz") > 0 Then
                            AvPlz = Request.Form.Item(j).ToString()
                            row("HaendlerPlz") = AvPlz
                            row.AcceptChanges()
                            If AvPlz.Trim.Length = 0 Then
                                blnVollst = False
                            End If
                        End If
                        If InStr(Request.Form.Keys.Item(j), equipment & ".ort") > 0 Then
                            AvOrt = Request.Form.Item(j).ToString()
                            row("HaendlerOrt") = AvOrt
                            row.AcceptChanges()
                            If AvOrt.Trim.Length = 0 Then
                                blnVollst = False
                            End If
                        End If
                    Next
                End If
            Next
        Next
    End Sub

    Private Sub FillGrid(Optional ByVal strSort As String = "")
        Dim tmpDataView As DataView = objZulassung.Result.DefaultView

        If Not strSort.Trim(" "c).Length = 0 Then
            Dim strDirection As String
            If ViewState("Direction") Is Nothing Then
                strDirection = "desc"
            Else
                strDirection = ViewState("Direction").ToString
            End If

            If strDirection = "asc" Then
                strDirection = "desc"
            Else
                strDirection = "asc"
            End If

            tmpDataView.Sort = strSort & " " & strDirection
            ViewState("Direction") = strDirection
        End If

        tmpDataView.RowFilter = "Ausgewaehlt = 1"
        DataGrid1.DataSource = tmpDataView
        DataGrid1.DataBind()

        DataGrid1.PagerStyle.Visible = False

        '####################################################################
        Dim item As DataGridItem

        'UH 06.06.05
        Dim strTemp As String
        Dim strHelp As String
        Dim blnHelp As Boolean

        For Each item In DataGrid1.Items

            strHelp = item.Cells(0).Text
            If objZulassung.Result.Select("EquipmentNummer='" & item.Cells(0).Text & "'")(0)("Versandadresse").ToString = "X" Then
                blnHelp = True
            Else
                blnHelp = False
            End If
            strTemp = "</TD></TR><TR><TD></TD><TD colspan=""" & item.Cells.Count - 2 & """>" & vbCrLf

            'Auswahl abw. Halteradresse
            strTemp &= "<input id=""" & strHelp & ".cbx"" type=""checkbox"" name=""" & strHelp & ".cbx"" "
            If blnHelp Then
                strTemp &= "checked "
            End If
            strTemp &= " onClick=""javascript:SI" & strHelp & "();"" onBlur=""javascript:SI" & strHelp & "();"" />" & vbCrLf
            strTemp &= "Abweichende Versandadresse Schein & Schilder:<br>" & vbCrLf

            'Name
            strTemp &= "&nbsp;&nbsp;Name: <input name=""" & strHelp & ".name"" type=""text"" value=""" &
                objZulassung.Result.Select("EquipmentNummer='" & item.Cells(0).Text & "'")(0)("HaendlerName1").ToString & """ id=""" &
                strHelp & ".name"" style=""width:200px;"" />" & vbCrLf

            'Strasse
            strTemp &= "&nbsp;Strasse: <input name=""" & strHelp & ".str"" type=""text"" value=""" &
                objZulassung.Result.Select("EquipmentNummer='" & item.Cells(0).Text & "'")(0)("HaendlerStr").ToString &
                """ id=""" & strHelp & ".str"" style=""width:200px;"" />" & vbCrLf

            'Nr
            strTemp &= "&nbsp;Nr.: <input name=""" & strHelp & ".nr"" type=""text"" value=""" &
                objZulassung.Result.Select("EquipmentNummer='" & item.Cells(0).Text & "'")(0)("HaendlerNr").ToString &
                """ id=""" & strHelp & ".nr"" style=""width:30px;"" />" & vbCrLf

            'Plz
            strTemp &= "&nbsp;Plz: <input name=""" & strHelp & ".plz"" type=""text"" value=""" &
                objZulassung.Result.Select("EquipmentNummer='" & item.Cells(0).Text & "'")(0)("HaendlerPlz").ToString &
                """ id=""" & strHelp & ".plz"" style=""width:50px;"" />" & vbCrLf

            'Ort
            strTemp &= "&nbsp;Ort: <input name=""" & strHelp & ".ort"" type=""text"" value=""" &
                objZulassung.Result.Select("EquipmentNummer='" & item.Cells(0).Text & "'")(0)("HaendlerOrt").ToString &
                """ id=""" & strHelp & ".ort"" style=""width:100px;"" />" & vbCrLf

            strTemp &= "<SCRIPT language=""JavaScript"" type=""text/javascript"">" & vbCrLf
            strTemp &= "<!--" & vbCrLf
            strTemp &= "	function SI" & strHelp & "() {" & vbCrLf
            'strTemp &= "        alert(document.getElementById(""" & strHelp & ".cbx"").checked);" & vbCrLf
            strTemp &= "		if (document.getElementById(""" & strHelp & ".cbx"").checked == true)" & vbCrLf
            strTemp &= "		    {" & vbCrLf
            strTemp &= "		        document.getElementById(""" & strHelp & ".name"").disabled = false;" & vbCrLf
            strTemp &= "		        document.getElementById(""" & strHelp & ".str"").disabled = false;" & vbCrLf
            strTemp &= "		        document.getElementById(""" & strHelp & ".nr"").disabled = false;" & vbCrLf
            strTemp &= "		        document.getElementById(""" & strHelp & ".plz"").disabled = false;" & vbCrLf
            strTemp &= "		        document.getElementById(""" & strHelp & ".ort"").disabled = false;" & vbCrLf
            strTemp &= "		    }" & vbCrLf
            strTemp &= "		    else" & vbCrLf
            strTemp &= "		    {" & vbCrLf
            strTemp &= "		        document.getElementById(""" & strHelp & ".name"").disabled = true;" & vbCrLf
            strTemp &= "		        document.getElementById(""" & strHelp & ".str"").disabled = true;" & vbCrLf
            strTemp &= "		        document.getElementById(""" & strHelp & ".nr"").disabled = true;" & vbCrLf
            strTemp &= "		        document.getElementById(""" & strHelp & ".plz"").disabled = true;" & vbCrLf
            strTemp &= "		        document.getElementById(""" & strHelp & ".ort"").disabled = true;" & vbCrLf
            strTemp &= "		    }" & vbCrLf
            strTemp &= "	}" & vbCrLf
            strTemp &= "	SI" & strHelp & "();" & vbCrLf
            strTemp &= "// -->" & vbCrLf
            strTemp &= "</SCRIPT>" & vbCrLf

            Dim newLiteral As New Literal()

            newLiteral.Text = strTemp
            item.Cells(item.Cells.Count - 1).Controls.Add(newLiteral)

        Next
    End Sub
    
    Private Sub DataGrid1_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles DataGrid1.SortCommand
        FillGrid(e.SortExpression)
    End Sub

    Private Sub DataGrid1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid1.ItemCommand
        Dim equ As String
        Dim row As DataRow()
        Dim tmpDataView As DataView

        If e.CommandName = "delete" Then
            equ = e.Item.Cells(0).Text
            DataGrid1.SelectedIndex = e.Item.ItemIndex

            row = objZulassung.Result.Select("EquipmentNummer='" & equ & "'")
            objZulassung.Result.Rows.Remove(row(0)) 'Löschen
            FillGrid()

            tmpDataView = objZulassung.Result.DefaultView
            tmpDataView.RowFilter = "Ausgewaehlt = 1"
            If (tmpDataView.Count = 0) Then
                cmdSave.Enabled = False
            End If
        End If
    End Sub

    'Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Response.Redirect("Change03_4.aspx?AppID=" & Session("AppID").ToString)
    'End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change03_4.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 30.06.09   Time: 13:03
' Updated in $/CKAG/Applications/apparval/Forms
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 25.08.08   Time: 17:11
' Updated in $/CKAG/Applications/apparval/Forms
' ITA: 1859
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 7  *****************
' User: Uha          Date: 6.03.07    Time: 15:30
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' GetAppIDFromQueryString(Me) hinzugefügt
' 
' *****************  Version 6  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************
