Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports System.Drawing
Imports System.Data.OleDb

Partial Public Class Change01
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Private objHaendler As Haendler
    Private objSuche As AppF2.Search
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New App(m_User)

        If m_User.Reference.Trim(" "c).Length > 0 Then
            txtHaendlerNr.Text = m_User.Reference
        End If

        If (Not Request.QueryString("back") Is Nothing) AndAlso CStr(Request.QueryString("back")) = "1" Then
            'Session("AppHaendler") = Nothing
            If Not Session("objSuche") Is Nothing Then
                txtHaendlerNr.Text = CType(Session("objSuche"), AppF2.Search).REFERENZ
            End If
        End If

        If (Not Request.QueryString("Linked") Is Nothing) AndAlso CStr(Request.QueryString("Linked")) = "1" Then
            If Not Session("objSuche") Is Nothing Then
                txtHaendlerNr.Text = Session("AppHaendlerNr")
            End If
        End If


        If IsPostBack = False Then
            If objHaendler Is Nothing Then
                If Session.Item("AppHaendler") Is Nothing Then
                    objHaendler = New Haendler(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.Reference, m_User.KUNNR)
                    Session.Add("AppHaendler", objHaendler)
                Else
                    If TypeOf Session.Item("AppHaendler") Is Haendler Then
                        objHaendler = CType(Session("AppHaendler"), Haendler)
                    Else
                        objHaendler = New Haendler(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.Reference, m_User.KUNNR)
                        Session("AppHaendler") = objHaendler
                    End If
                End If
            End If
            If objSuche Is Nothing Then
                searchHaendlerInitial()
            End If

            tr_FahrgestellNr.Visible = True
            tr_TIDNR.Visible = True
            tr_ZZREFERENZ1.Visible = True
            tr_LIZNR.Visible = True


        Else 'wenn Postback
            If objHaendler Is Nothing Then
                objHaendler = CType(Session("AppHaendler"), Haendler)
            End If
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region

    Protected Sub lbSelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbSelektionZurueckSetzen.Click
        SelektionZuruecksetzen()
        txt_LIZNR.Text = ""
        txtFahrgestellNr.Text = ""
        txtTIDNR.Text = ""
        txtZZREFERENZ1.Text = ""
    End Sub


    Private Sub SelektionZuruecksetzen()
        'Zustand wie bei initial laden wiederherstellen
        txtName1.Text = ""
        txtName2.Text = ""
        txtPLZ.Text = ""
        txtNummer.Text = ""
        txtOrt.Text = ""
        lblSHistoryName1.Text = ""
        lblSHistoryName2.Text = ""
        lblSHistoryNR.Text = ""
        lblSHistoryOrt.Text = ""
        lblSHistoryPLZ.Text = ""

        'txtName2 Deaktivieren
        txtName2.Enabled = False
        txtName2.BackColor = System.Drawing.Color.FromKnownColor(KnownColor.InactiveCaptionText)


        lbHaendler.SelectedIndex = -1

        If objSuche Is Nothing Then
            objSuche = CType(Session("objNewHaendlerSuche"), AppF2.Search)
        End If

        With objSuche
            .sucheHaendlerNr = txtNummer.Text
            .sucheName1 = txtName1.Text
            .sucheName2 = txtName2.Text
            .sucheOrt = txtOrt.Text
            .suchePLZ = txtPLZ.Text
        End With

        Searchhaendler()
    End Sub

    Private Function pruefung() As Boolean

        With objHaendler
            If Not .SucheFahrgestellNr Is Nothing AndAlso Not .SucheFahrgestellNr.Trim = "" Then
                Return True
            End If

            If Not .SucheTIDNR Is Nothing AndAlso Not .SucheTIDNR.Trim = "" Then
                Return True
            End If


            If Not .SucheZZREFERENZ1 Is Nothing AndAlso Not .SucheZZREFERENZ1.Trim = "" Then
                Return True
            End If

            If Not .Customer Is Nothing AndAlso Not .Customer.Trim = "" AndAlso Not .Customer = "0000000000" Then
                Return True
            End If

            If Not .sucheLIZNR Is Nothing AndAlso Not .sucheLIZNR.Trim = "" Then
                Return True
            End If
            Return False
        End With
    End Function
    Private Sub DoSubmit()

        lblError.Text = ""
        lblError.Visible = False

        objHaendler.Customer = lblHaendlerDetailsNR.Text
        Dim uploadTable As DataTable
        uploadTable = LoadUploadFile()

        If Not uploadTable Is Nothing Then
            objHaendler.UploadTable = uploadTable
            objHaendler.GiveCarsUpload(Session("AppID").ToString, Session.SessionID, Me)

        Else

            objHaendler.SucheTIDNR = txtTIDNR.Text
            objHaendler.SucheZZREFERENZ1 = txtZZREFERENZ1.Text ' Ordernummer
            objHaendler.SucheFahrgestellNr = Replace(txtFahrgestellNr.Text, "%", "*")
            objHaendler.KUNNR = m_User.KUNNR
            objHaendler.sucheLIZNR = txt_LIZNR.Text

            If Not pruefung() Then
                lblError.Text = "Bitte geben Sie ein Suchkriterium ein."
                lblError.Visible = True
                lbSelektionZurueckSetzen.Visible = True
                Exit Sub
            End If
            objHaendler.GiveCars(Session("AppID").ToString, Session.SessionID, Me)
        End If





        If Not objHaendler.Status = 0 Then
            lblError.Text = objHaendler.Message
            lblError.Visible = True
            If objHaendler.Status = -2503 Then
                If m_User.Reference.Trim Is String.Empty Then
                    Session("AppFIN") = Replace(txtFahrgestellNr.Text, "%", "*")
                    Session("AppHaendlerNr") = objHaendler.getHaendlernummerByFin(Replace(txtFahrgestellNr.Text, "%", "*"))
                    If Not objHaendler.Status = 0 Then
                        lblError.Text = objHaendler.Message
                        lblError.Visible = True
                        Exit Sub
                    End If
                End If
            End If
        Else
            If objHaendler.Fahrzeuge.Rows.Count = 0 Then
                lblError.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                lbSelektionZurueckSetzen.Visible = True
                lblError.Visible = True
            Else

                'Bei Upload Anfordern vorbelegen
                If Not uploadTable Is Nothing Then
                    For Each dr As DataRow In objHaendler.Fahrzeuge.Rows
                        dr("MANDT") = "1"
                    Next
                End If

                Session("AppHaendler") = objHaendler
                Session("objSuche") = objSuche
                Response.Redirect("Change01_2.aspx?AppID=" & Session("AppID").ToString, False)
            End If
        End If
    End Sub
    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub
    Private Function LoadUploadFile() As DataTable
        'Prüfe Fehlerbedingung
        If (Not upFile1.PostedFile Is Nothing) AndAlso (Not (upFile1.PostedFile.FileName = String.Empty)) Then
            If Right(upFile1.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                lblError.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                Return Nothing
                Exit Function
            End If
            If (upFile1.PostedFile.ContentLength > CType(ConfigurationManager.AppSettings("MaxUploadSize"), Integer)) Then
                lblError.Text = "Datei '" & upFile1.PostedFile.FileName & "' ist zu gross (>300 KB)."
                Return Nothing
                Exit Function
            End If
            'Lade Datei
            Return getVertragsnummern(upFile1.PostedFile)
        Else
            Return Nothing
            Exit Function
        End If
    End Function

    Private Function getVertragsnummern(ByVal uFile As System.Web.HttpPostedFile) As DataTable
        Dim tmpTable As New DataTable
        Try
            Dim filepath As String = ConfigurationManager.AppSettings("UploadpathLocal")
            Dim filename As String
            Dim info As System.IO.FileInfo

            'Dateiname: User_yyyyMMddhhmmss.xls
            filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

            If Not (uFile Is Nothing) Then
                uFile.SaveAs(ConfigurationManager.AppSettings("UploadpathLocal") & filename)
                uFile = Nothing
                info = New System.IO.FileInfo(filepath & filename)
                If Not (info.Exists) Then
                    tmpTable = Nothing
                    Throw New Exception("Fehler beim Speichern")
                End If
                'Datei gespeichert -> Auswertung
                tmpTable = getDataTableFromExcel(filepath, filename)
            End If
        Catch ex As Exception
            lblError.Text = ex.Message
        Finally
            getVertragsnummern = tmpTable
        End Try

    End Function
    Private Function getDataTableFromExcel(ByVal filepath As String, ByVal filename As String) As DataTable
        '----------------------------------------------------------------------
        ' Methode: GetDataTable
        ' Autor: JJU 
        ' Beschreibung: extrahiert die Daten aus dem ersten Exceltabellen-Blatt in eine Datatable
        ' Erstellt am: 2008.09.22
        ' ITA: 1844
        '----------------------------------------------------------------------

        Dim objDataset1 As New DataSet()
        Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                         "Data Source=" & filepath & filename & ";Extended Properties=""Excel 8.0;HDR=No"""
        Dim objConn As New OleDbConnection(sConnectionString)
        objConn.Open()

        Dim schemaTable As DataTable
        Dim tmpObj() As Object = {Nothing, Nothing, Nothing, "Table"}
        schemaTable = objConn.GetOleDbSchemaTable(OleDb.OleDbSchemaGuid.Tables, tmpObj)

        For Each sheet As DataRow In schemaTable.Rows
            Dim tableName As String = sheet("Table_Name").ToString
            Dim objCmdSelect As New OleDbCommand("SELECT * FROM [" & tableName & "]", objConn)
            Dim objAdapter1 As New OleDbDataAdapter(objCmdSelect)
            objAdapter1.Fill(objDataset1, tableName)
        Next
        Dim tblTemp As DataTable = objDataset1.Tables(0)
        objConn.Close()
        Return tblTemp
    End Function
    Protected Sub lbHaendler_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lbHaendler.SelectedIndexChanged
        objSuche = Session("objNewHaendlerSuche")

        objSuche.Haendler.RowFilter = "REFERENZ = '" & lbHaendler.SelectedValue & "'"
        lblHaendlerDetailsNR.Text = objSuche.Haendler.Item(0).Item("REFERENZ")
        lblHaendlerDetailsName1.Text = objSuche.Haendler.Item(0).Item("Name")
        lblHaendlerDetailsName2.Text = objSuche.Haendler.Item(0).Item("NAME_2")
        lblHaendlerDetailsStrasse.Text = objSuche.Haendler.Item(0).Item("STREET")
        lblHaendlerDetailsPLZ.Text = objSuche.Haendler.Item(0).Item("POSTL_CODE")
        lblHaendlerDetailsOrt.Text = objSuche.Haendler.Item(0).Item("CITY")
    End Sub

    Private Sub searchHaendlerInitial()


        objSuche = New AppF2.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)


        With objSuche
            .sucheHaendlerNr = txtNummer.Text
            .sucheName1 = txtName1.Text
            .sucheName2 = txtName2.Text
            .sucheOrt = txtOrt.Text
            .suchePLZ = txtPLZ.Text
        End With

        Searchhaendler()
        If lbl_error.Text = "" AndAlso objSuche.anzahlHaendlerTreffer > 0 AndAlso objSuche.anzahlHaendlerTreffer <= 200 Then


            'Name 2 Textbox wieder aktivieren da ja jetzt von name1 sapseitig auch in name2 gesucht wird, clientseitig nicht möglich! JJ2008.04.23
            txtName2.Enabled = True
            txtName2.BackColor = Nothing



            'werte in SearchHistoryLabels Schreiben
            lblSHistoryName1.Text = txtName1.Text
            lblSHistoryName2.Text = txtName1.Text
            '------------------------------------
            'nein weil jetzt sap seitig in name 1 und 2 gesucht wird bei eingabe in Name 1
            'lblSHistoryName2.Text = txtName2.Text
            '-----------------------------------
            lblSHistoryNR.Text = txtNummer.Text
            lblSHistoryOrt.Text = txtOrt.Text
            lblSHistoryPLZ.Text = txtPLZ.Text



            'wenn nicht * am ende dann in historyLabel Schreiben sonst * entfernen
            If Not txtNummer.Text.Trim(" "c).Length = 0 AndAlso txtNummer.Text.IndexOf("*") < txtNummer.Text.Length - 1 Then
                txtNummer.Text = ""
            Else
                txtNummer.Text = txtNummer.Text.Replace("*", "")
            End If

            If Not txtName1.Text.Trim(" "c).Length = 0 AndAlso txtName1.Text.IndexOf("*") < txtName1.Text.Length - 1 Then
                txtName1.Text = ""
            Else
                '---------------------------------------------------------------
                'der Bapi sucht bei einer übergabe von Name1 auch in Name 2, daher muss immer die Eingabe gelöscht werden, da sonst die clientseitige Selektion nicht greift, 
                'dort geht nur die suche eine Eingabe-> ein Sucharray, da ja alle nicht zutreffenden rausgeschmissen werden. JJ2008.4.23
                'txtName1.Text = txtName1.Text.Replace("*", "")
                '---------------------------------------------------------------
                txtName1.Text = ""
            End If

            If Not txtName2.Text.Trim(" "c).Length = 0 AndAlso txtName2.Text.IndexOf("*") < txtName2.Text.Length - 1 Then
                txtName2.Text = ""
            Else
                txtName2.Text = txtName2.Text.Replace("*", "")
            End If

            If Not txtPLZ.Text.Trim(" "c).Length = 0 AndAlso txtPLZ.Text.IndexOf("*") < txtPLZ.Text.Length - 1 Then
                txtPLZ.Text = ""
            Else
                txtPLZ.Text = txtPLZ.Text.Replace("*", "")
            End If


            If Not txtOrt.Text.Trim(" "c).Length = 0 AndAlso txtOrt.Text.IndexOf("*") < txtOrt.Text.Length - 1 Then
                txtOrt.Text = ""
            Else
                txtOrt.Text = txtOrt.Text.Replace("*", "")
            End If



        End If
        Session("objNewHaendlerSuche") = objSuche
    End Sub
    Private Sub Searchhaendler()
        'neue Klassse erstellen für BAPI Aufruf

        Try

            lbl_error.Text = ""
            lbl_Message.Text = ""
            Dim intStatus As Int32
            intStatus = objSuche.LeseHaendlerForSucheHaendlerControl(Session("AppID").ToString, Session.SessionID.ToString, Me.Page)

            If Not intStatus = 0 Then
                lbl_error.Text = objSuche.ErrorMessage
                Exit Sub
            End If

            lbHaendler.DataSource = objSuche.Haendler
            lbHaendler.DataTextField = "DISPLAY"
            lbHaendler.DataValueField = "REFERENZ"
            lbHaendler.DataBind()

            If objSuche.anzahlHaendlerTreffer = 0 Then
                lbl_Message.Text = "keine Ergebnisse gefunden"

                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False
                lbl_Info.Visible = True
            ElseIf Not objSuche.anzahlHaendlerTreffer = 0 AndAlso objSuche.Haendler Is Nothing OrElse objSuche.Haendler.Count = 0 Then
                If anySearchEntrys() = True Then
                    lbl_Message.Text = "zu viele Ergebnisse gefunden (max 200 Treffer), weitere Einschränkungen benötigt "
                End If


                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbl_Info.Visible = True
            Else
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                tr_HaendlerAuswahl.Visible = True
                txtName2.Enabled = True
                txtName2.BackColor = Nothing
                lbl_Info.Visible = False
                lbl_Message.Text = ""
            End If

        Catch ex As Exception
            lbl_error.Text = "Es ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Protected Sub txtName1_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtName1.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtName1.Text, "*", "%")
        Search("NAME LIKE '%" & strFilter & "%'")
    End Sub

    Private Function anySearchEntrys() As Boolean

        If Not txtName1.Text.Trim(" "c) = "" OrElse Not txtName1.Text.Trim(" "c) = "" OrElse Not txtName2.Text.Trim(" "c) = "" OrElse Not txtOrt.Text.Trim(" "c) = "" OrElse Not txtPLZ.Text.Trim(" "c) = "" OrElse Not txtNummer.Text.Trim(" "c) = "" Then
            Return True
        Else
            Return False
        End If


    End Function

    Private Sub txtPLZ_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPLZ.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtPLZ.Text, "*", "%")
        Search("POSTL_CODE LIKE '%" & strFilter & "%'")
    End Sub

    Private Sub Search(ByVal Filter As String)

        If Session("objNewHaendlerSuche") Is Nothing Then
            objSuche = New AppF2.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            Session("objNewHaendlerSuche") = objSuche
            searchHaendlerInitial()
        Else
            objSuche = Session("objNewHaendlerSuche")
            If objSuche.Haendler Is Nothing Then
                searchHaendlerInitial()
            End If

        End If

        Try
            If objSuche.anzahlHaendlerTreffer = 0 Then
                lbl_Message.Text = "keine Ergebnisse gefunden"

                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False

                lbl_Info.Visible = True
            ElseIf Not objSuche.anzahlHaendlerTreffer = 0 AndAlso objSuche.Haendler Is Nothing OrElse objSuche.Haendler.Count = 0 Then
                If anySearchEntrys() = True Then
                    lbl_Message.Text = "zu viele Ergebnisse gefunden (max 200 Treffer), weitere Einschränkungen benötigt "
                End If


                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbl_Info.Visible = True
            Else
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                tr_HaendlerAuswahl.Visible = True
                txtName2.Enabled = True
                txtName2.BackColor = Nothing
                lbl_Info.Visible = False
                lbl_Message.Text = ""
                objSuche.Haendler.RowFilter = Filter
                lbHaendler.DataSource = objSuche.Haendler
                lbHaendler.DataTextField = "DISPLAY"
                lbHaendler.DataValueField = "REFERENZ"
                lbHaendler.DataBind()
            End If

        Catch ex As Exception
            lbl_error.Text = "Es ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Protected Sub txtNummer_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtNummer.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtNummer.Text, "*", "%")
        Search("REFERENZ LIKE '%" & strFilter & "%'")
    End Sub

    Protected Sub txtName2_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtName2.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtName2.Text, "*", "%")
        Search("NAME_2 LIKE '%" & strFilter & "%'")
    End Sub

    Protected Sub txtOrt_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtOrt.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtOrt.Text, "*", "%")
        Search("CITY LIKE '" & strFilter & "'")
    End Sub


    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    End Sub
End Class
