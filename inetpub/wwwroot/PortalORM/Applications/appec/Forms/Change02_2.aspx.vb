Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common


Public Class Change02_2
    Inherits System.Web.UI.Page

    Protected WithEvents ucStyles As Styles
    Protected WithEvents ucHeader As Header
    Private m_App As Base.Kernel.Security.App
    Private m_User As Base.Kernel.Security.User
    Private ResultTable As DataTable
    Private highlightID As String
    Protected WithEvents lblHead As System.Web.UI.WebControls.Label
    Protected WithEvents dataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents lblError As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnzahl As System.Web.UI.WebControls.Label
    Protected WithEvents btnSaveSAP As System.Web.UI.WebControls.LinkButton
    Protected WithEvents lblTableTitle As System.Web.UI.WebControls.Label
    Protected WithEvents txtOrtsKzOld As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtFree2 As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents trSumme As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents Linkbutton1 As System.Web.UI.WebControls.LinkButton
  
    Private objBatch As ec_02
    Private alBatchIDCollectionCurret As New ArrayList()
    Private alBatchIDCollection As New ArrayList()


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
        Session.Add("AppID", Request.QueryString("AppID"))

        objBatch = CType(Session("objSuche"), ec_02)

        If Not IsPostBack Then
            initialLoad()
        Else
            'FillView()
        End If
    End Sub

    Private Sub initialLoad(Optional ByVal status As Int32 = 0)

        If status = 1 Then
            lblError.Text = "Aktion erfolgreich"
        ElseIf status = 2 Then
            lblError.Text = "keine Aktion getätigt"
        ElseIf status = 0 Then
            lblError.Text = ""
        End If



        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text


            If objBatch.withUnitNumbers = False And objBatch.UnitNr = "" Or objBatch.UnitNr = " " Then
                modifyDataSource() 'modifiziert den DataSource so, dass nur noch unterschiedliche BatchIDs angezeigt werden JJ 2007.10.24
            Else
                objBatch.Result.DefaultView.Sort = "UnitNR"
            End If

            'Dim tmpView As New DataView(objBatch.Result)
            'Dim tmpCheckBox As CheckBox
            Select Case objBatch.SelectionCriteria
                Case ("sperrbare/entsperrbare")
                    objBatch.Result.DefaultView.RowFilter = "EQUNR=''"
                Case ("zugeteilte")
                    objBatch.Result.DefaultView.RowFilter = "EQUNR<>''"
                Case ("sperrbare")
                    objBatch.Result.DefaultView.RowFilter = "EQUNR='' AND LOESCH=''"
                Case ("alle")

                Case ("entsperrbare")
                    objBatch.Result.DefaultView.RowFilter = "EQUNR='' AND LOESCH<>''"
            End Select

            Session("objSuche") = objBatch

            FillGrid(0)
        Catch ex As Exception
            lblError.Text = "Fehler beim Erstellen des Reports. " & ex.Message
        End Try
    End Sub

    Private Sub FillView()
        Dim item As DataGridItem
        Dim cbxControl As CheckBox
        Dim strUnitNr As String
        Dim tmpRows As DataRow()
        Dim resultTable As DataTable

        resultTable = objBatch.Result

        For Each item In dataGrid.Items
            'For Each cell In item.Cells
            strUnitNr = "UnitNr='" & item.Cells(0).Text & "'"
            cbxControl = CType(item.Cells(item.Cells.Count - 1).FindControl("cbxDelete"), CheckBox)
            tmpRows = resultTable.Select(strUnitNr)
            If cbxControl.Checked Then
                'If CType(tmpRows(0)("LoeschNeu"), String) = "X" Then
                tmpRows(0)("LoeschNeu") = "X"
            Else
                tmpRows(0)("LoeschNeu") = String.Empty
            End If
            'End If
        Next
    End Sub

    Private Sub FillGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "")
        Dim ResultTable As DataTable
        objBatch = CType(Session("objSuche"), ec_02)

        ResultTable = objBatch.Result

        If (ResultTable Is Nothing) OrElse (ResultTable.Rows.Count = 0) Then
            dataGrid.Visible = False

        Else


            dataGrid.Visible = True
            'lblNoData.Visible = False

            Dim tmpDataView As New DataView()
            tmpDataView = ResultTable.DefaultView

            Dim intTempPageIndex As Int32 = intPageIndex
            Dim strTempSort As String = ""
            Dim strDirection As String = ""

            If strSort.Trim(" "c).Length > 0 Then
                intTempPageIndex = 0
                strTempSort = strSort.Trim(" "c)
                If (ViewState("Sort") Is Nothing) OrElse (ViewState("Sort").ToString = strTempSort) Then
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "desc"
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                Else
                    strDirection = "desc"
                End If

                If strDirection = "asc" Then
                    strDirection = "desc"
                Else
                    strDirection = "asc"
                End If

                ViewState("Sort") = strTempSort
                ViewState("Direction") = strDirection
            Else
                If Not ViewState("Sort") Is Nothing Then
                    strTempSort = ViewState("Sort").ToString
                    'strTempSort = "free1"
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                tmpDataView.Sort = strTempSort & " " & strDirection
            Else
                tmpDataView.Sort = "modelid, modell"
            End If

            dataGrid.CurrentPageIndex = intTempPageIndex
            dataGrid.DataSource = tmpDataView
            dataGrid.DataBind()
            lblAnzahl.Text = tmpDataView.Count

            If dataGrid.PageCount > 1 Then
                dataGrid.PagerStyle.CssClass = "PagerStyle"
                dataGrid.DataBind()
                dataGrid.PagerStyle.Visible = True
            Else
                dataGrid.PagerStyle.Visible = False
            End If

            modifyGridInhalt(tmpDataView)

        End If
    End Sub

    Private Sub btnSaveSAP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSAP.Click

        Try
            writeToResultTable()

            Dim ResultTable As DataTable
            objBatch = CType(Session("objSuche"), ec_02)

            ResultTable = objBatch.Result

            Dim iStatus As Int32 = 2

            For Each dr As DataRow In ResultTable.Rows
                If dr("UnitNr") = "XXXXXXX" Then
                    saveToSAPbyArrayList(dr("BatchID"), dr("Sperrbemerkung"), dr("Loesch"), dr("Bemerkung"))
                Else
                    objBatch.saveDataInSAPRow(dr("UnitNr"), dr("Loesch"), dr("Laufzeitbindung"), dr("Sperrbemerkung"), dr("Bemerkung"))
                End If
                iStatus = 1
            Next

            objBatch.comitSapWorkAndCloseConnection(Session("AppID").ToString, Session.SessionID, Me)
            alBatchIDCollectionCurret.Clear()
            Session("BatchIDCollection") = alBatchIDCollectionCurret
            objBatch.getData(Session("AppID").ToString, Session.SessionID, Me)
            initialLoad(iStatus)
        Catch ex As Exception
            lblError.Text = "Beim Speichern in SAP ist ein Fehler aufgetreten: " & ex.Message
        End Try

    End Sub


    Private Sub saveToSAPbyArrayList(ByVal strBatchId As String, ByVal strSperrBemerkung As String, ByVal strLoesch As String, ByVal strBemerkung As String)
        Dim strCollection As String
        Dim aStrElements() As String
        Dim bBreak As Boolean = False

        If alBatchIDCollectionCurret.Count = 0 Then
            Try
                alBatchIDCollectionCurret = Session.Item("BatchIDCollection")
            Catch
                lblError.Text = "es wurde Item 'BatchIDCollection' in der Session gefunden"
                Exit Sub
            End Try
        End If

        For Each strCollection In alBatchIDCollectionCurret
            'aufbau:   strBatchID;strUnitNr;strgesperrt;strLaufzeitb   
            aStrElements = strCollection.Split(";")

            If aStrElements(0).Equals(strBatchId) Then

                bBreak = True
                objBatch.saveDataInSAPRow(aStrElements(1), strLoesch, aStrElements(3), strSperrBemerkung, strBemerkung)

            Else
                If bBreak = True Then
                    Exit For
                End If
            End If


        Next
    End Sub


    Private Sub saveToSAPbyDataGrid()
        Dim tmpcb As New CheckBox()
        Dim item As DataGridItem = Nothing
        tmpcb = item.FindControl("cbxDelete")


        If item.Cells(16).Text = "X" Then
            tmpcb.Checked = True
        End If
        If Not item.Cells(15).Text = "&nbsp;" Then
            tmpcb.Enabled = False

        End If




    End Sub

    Private Sub modifyGridInhalt(ByVal tmpDataView As DataView) 'je nach dem was in der Zeile Steht, die Checkbox modifizieren
        Dim tmpcb As New CheckBox()
        Dim tmptxt As New TextBox()

        Dim i As Int32
        Dim item As DataGridItem

        For Each item In dataGrid.Items

            tmpcb = item.FindControl("cbxDelete")

            If item.Cells(16).Text = "X" Or Not item.Cells(15).Text = "&nbsp;" Then 'cell für löschvermerk und cell für belegt
                tmpcb = item.FindControl("cbxDelete")

                If item.Cells(16).Text = "X" Then
                    tmpcb.Checked = True
                End If
                If Not item.Cells(15).Text = "&nbsp;" Then
                    tmpcb.Enabled = False
                End If

            End If

            System.Diagnostics.Debug.WriteLine(item.Cells(0).Text)
            System.Diagnostics.Debug.WriteLine(item.Cells(item.Cells.Count - 2).Text)
            System.Diagnostics.Debug.WriteLine(item.Cells(item.Cells.Count - 3).Text)
            System.Diagnostics.Debug.WriteLine(item.Cells(4).Text)
            System.Diagnostics.Debug.WriteLine("Count cells des Items= " & item.Cells.Count)
            i = i + 1
        Next
        System.Diagnostics.Debug.WriteLine("gesamte Items in DataGrid= " & i)
        System.Diagnostics.Debug.WriteLine("Count cells einer Item")

    End Sub

    Private Sub modifyDataSource()
        Dim i As Int32 = 0
        Dim strbatchIDold As String = "empty"
        Dim bGesperrt As Boolean = True
        Dim bBelegt As Boolean = True
        Dim intVergebeneUnits As Int32 = 0
        Dim intGesperrteUnits As Int32 = 0

        objBatch = CType(Session("objSuche"), ec_02)
        ResultTable = objBatch.Result

        System.Diagnostics.Debug.WriteLine(ResultTable.Rows.Count.ToString)

        Dim rows() As DataRow = ResultTable.Select("BatchID <> '0000000'", "BatchID") 'einzig und alleine die sortierung nach batch ID


        While i < rows.Length
            'mit feststellung für Batchstatus, ist eine unitNr in einem Batch nicht gesperrt oder frei ist das auch der ganze batch
            If Not strbatchIDold.Equals(rows(i)("BatchID")) Then
                If Not strbatchIDold.Equals("empty") Then
                    ViewState.Add(strbatchIDold, bGesperrt & ";" & bBelegt & ";" & intGesperrteUnits & ";" & intVergebeneUnits)
                    bGesperrt = True
                    bBelegt = True
                    intGesperrteUnits = 0
                    intVergebeneUnits = 0
                End If
                strbatchIDold = rows(i)("BatchID")
                If Not objBatch.SelectionCriteria = "unveraenderbar" Then
                    'createBatchIDCollection(rows(i)("UnitNR"), rows(i)("LOESCH"), rows(i)("BatchID"), rows(i)("UnitNrPruefziffer"), rows(i)("Laufzeitbindung"))
                    createBatchIDCollection(rows(i)("UnitNR"), rows(i)("LOESCH"), rows(i)("BatchID"), rows(i)("Laufzeitbindung"))


                    If rows(i)("LOESCH") = "X" Then
                        intGesperrteUnits = intGesperrteUnits + 1
                    End If

                    If rows(i)("EQUNR") = "" Then
                        If Not rows(i)("LOESCH") = "X" Then
                            'wenn eine nicht belegte Unitnummer nicht gesperrt ist, ist das ganze batch sperrbar, anderstherum heißt es das wenn alle nicht belegten unitnummern gesperrt sind, ist auch das batch gesperrt JJ20071220
                            bGesperrt = False
                        End If
                        bBelegt = False
                    Else
                        intVergebeneUnits = intVergebeneUnits + 1
                    End If
                End If
                rows(i).BeginEdit()
                rows(i)("UnitNR") = "XXXXXXX" '7X
                rows(i).EndEdit()
            Else
                If Not objBatch.SelectionCriteria = "unveraenderbar" Then
                    'createBatchIDCollection(rows(i)("UnitNR"), rows(i)("LOESCH"), rows(i)("BatchID"), rows(i)("UnitNrPruefziffer"), rows(i)("Laufzeitbindung"))
                    createBatchIDCollection(rows(i)("UnitNR"), rows(i)("LOESCH"), rows(i)("BatchID"), rows(i)("Laufzeitbindung"))
                    If rows(i)("LOESCH") = "X" Then
                        intGesperrteUnits = intGesperrteUnits + 1
                    End If
                    If rows(i)("EQUNR") = "" Then
                        If Not rows(i)("LOESCH") = "X" Then
                            'wenn eine nicht belegte Unitnummer nicht gesperrt ist, ist das ganze batch sperrbar, anderstherum heißt es das wenn alle nicht belegten unitnummern gesperrt sind, ist auch das batch gesperrt JJ20071220
                            bGesperrt = False
                        End If
                        bBelegt = False
                    Else
                        intVergebeneUnits = intVergebeneUnits + 1
                    End If
                End If
                rows(i).Delete()
            End If
            i = i + 1
        End While
        System.Diagnostics.Debug.Write("Wirkliche Anzahl von Durchläufen=" & i)
        ViewState.Add(strbatchIDold, bGesperrt & ";" & bBelegt & ";" & intGesperrteUnits & ";" & intVergebeneUnits) 'letzte BatchID speichern, da ansonsten nurn gespeichert wird, wenn sich die BatchID ändert

        Try
            Session.Item("BatchIDCollection") = alBatchIDCollection
        Catch
            Session.Add("BatchIDCollection", alBatchIDCollection)
        End Try
        ResultTable.AcceptChanges()
        objBatch.ResultTable = ResultTable
        modifyDataSourceForBatches()
    End Sub

    Private Sub modifyDataSourceForBatches()
        Dim row As DataRow

        'stats der einzelnen batches aus viewState auslesen und danach die Zeile modifizieren, danach view state löschen
        'den löschvermerk der Zeile für den batch anpassen, so das änderungen wahrgenommen werden bei liste abschicken

        For Each row In objBatch.ResultTable.Rows
            If row("UnitNR") = "XXXXXXX" Then
                Dim strBatchStatus As String
                Dim strEinzelStatus() As String

                Try

                    strBatchStatus = ViewState.Item(row("BatchID")) 'batchid aus Griditem holen
                    strEinzelStatus = strBatchStatus.Split(";") 'gesperrt;belegt;AnzahlGesperrt;anzahlVergeben
                    row.BeginEdit()
                    If strEinzelStatus(0).Equals("True") Then
                        'den löschvermerk der Zeile für den batch anpassen, so das änderungen wahrgenommen werden bei liste abschicken
                        row("LOESCH") = "X"
                    ElseIf strEinzelStatus(0).Equals("False") Then
                        'den löschvermerk der Zeile für den batch anpassen, so das änderungen wahrgenommen werden bei liste abschicken
                        row("LOESCH") = ""
                    End If

                    If strEinzelStatus(1).Equals("True") Then
                        row("EQUNR") = "not Empty"
                    ElseIf strEinzelStatus(1).Equals("False") Then
                        row("EQUNR") = ""
                    End If
                    ViewState.Remove(row("BatchID"))
                    row.EndEdit()
                    If objBatch.ResultTable.Rows.Count = 1 Then
                        lblError.Text = "Dieses Batch enthält " & strEinzelStatus(2) & " gesperrte / " & strEinzelStatus(3) & " zugeteilte Unit Nummern"
                    End If
                Catch
                    lblError.Text = "Fehler beim auslesen der ViewState für den BatchStatus. BatchNummer= " & row("BatchID")
                    Exit Sub
                End Try
            End If
        Next
        objBatch.ResultTable.AcceptChanges()
    End Sub



    Private Sub createBatchIDCollection(ByVal strUnitNr As String, ByVal strgesperrt As String, ByVal strBatchID As String, ByVal strLaufzeitb As String)
        Dim strBatchIdCollection As String

        strBatchIdCollection = strBatchID & ";" & strUnitNr & ";" & strgesperrt & ";" & strLaufzeitb
        'aufbau:   strBatchID;strUnitNr;strgesperrt;strLaufzeitb
        alBatchIDCollection.Add(strBatchIdCollection)
    End Sub

    Private Sub writeToResultTable()
        Dim ResultTable As DataTable
        objBatch = CType(Session("objSuche"), ec_02)

        ResultTable = objBatch.Result

        Dim tmpcb As CheckBox
        Dim tmptxtSperrvermerk As TextBox
        Dim tmptxtBemerkung As TextBox

        Dim Row As DataRow

        Dim item As DataGridItem

        For Each item In dataGrid.Items


            Row = ResultTable.Select("RowID = '" & item.Cells(3).Text & "'")(0)


            tmpcb = item.FindControl("cbxDelete")
            tmptxtSperrvermerk = item.FindControl("txtSperrvermerk")
            tmptxtBemerkung = item.FindControl("txtBemerkung")

            If tmpcb.Checked = True Then
                Row("Loesch") = "X"
            Else
                Row("Loesch") = ""
            End If

            Row("Sperrbemerkung") = tmptxtSperrvermerk.Text
            Row("Bemerkung") = tmptxtBemerkung.Text


        Next

        Session("objSuche") = objBatch

    End Sub


    Private Sub dataGrid_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dataGrid.SortCommand
        writeToResultTable()
        FillGrid(dataGrid.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Sub Linkbutton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Linkbutton1.Click
        Response.Redirect("Change02.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


    Private Sub dataGrid_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dataGrid.PageIndexChanged
        writeToResultTable()
        FillGrid(e.NewPageIndex)
    End Sub
End Class

' ************************************************
' $History: Change02_2.aspx.vb $
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
' *****************  Version 19  *****************
' User: Jungj        Date: 21.12.07   Time: 15:06
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' batchreporting ITA 1358
' 
' *****************  Version 18  *****************
' User: Jungj        Date: 7.12.07    Time: 9:17
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 17  *****************
' User: Jungj        Date: 30.10.07   Time: 15:56
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 16  *****************
' User: Jungj        Date: 30.10.07   Time: 9:55
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 29.10.07   Time: 16:48
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 29.10.07   Time: 8:51
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 26.10.07   Time: 12:32
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 26.10.07   Time: 9:11
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 25.10.07   Time: 18:15
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 25.10.07   Time: 10:05
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
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Forms
' 
' ************************************************
