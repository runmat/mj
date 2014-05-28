Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Business
Imports System.Data.OleDb

Partial Public Class Change03
    Inherits System.Web.UI.Page
#Region "Declarations"
    Private m_User As Security.User
    Private m_App As Security.App
    Private m_change As Versand
    Private versandart As String
    Private booError As Boolean
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        GetAppIDFromQueryString(Me)

        versandart = Request.QueryString.Item("art").ToString

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        If m_User.Organization.OrganizationAdmin Then
            lblHead.Text = Replace(lblHead.Text, " (Beauftragung)", "")
        End If

        m_App = New Security.App(m_User)

        If (Session("AppChange") Is Nothing) OrElse (Not IsPostBack) Then
            m_change = New Versand(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
        Else
            m_change = CType(Session("AppChange"), Versand)
        End If

        'Für den Upload
        Me.Form.Enctype = "multipart/form-data"
        CheckAuswahl()

        Session("AppChange") = m_change

    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        'Response.Redirect("../../../Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


#End Region

#Region "Methods"
    Private Sub DoSubmit()
        Dim b As Boolean
        lblerror.Text = ""

        b = True

        'Keine Platzhaltersuche -> Werfe Platzhalter 'raus
        If Not cbxPlatzhaltersuche.Checked Then
            txtOrdernummer.Text = Replace(txtOrdernummer.Text, "*", "")
            txtOrdernummer.Text = Replace(txtOrdernummer.Text, "%", "")

            txtAmtlKennzeichen.Text = Replace(txtAmtlKennzeichen.Text, "*", "")
            txtAmtlKennzeichen.Text = Replace(txtAmtlKennzeichen.Text, "%", "")

            txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text, "*", "")
            txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text, "%", "")
        End If

        'Briefnummer generell ohne Platzhalter
        txtNummerZB2.Text = Replace(txtNummerZB2.Text, "*", "")
        txtNummerZB2.Text = Replace(txtNummerZB2.Text, "%", "")


        If Not chk_alle.Checked Then
            If txtNummerZB2.Text.Length = 0 Then
                m_change.SucheNummerZB2 = ""
            Else
                m_change.SucheNummerZB2 = txtNummerZB2.Text.Replace(" ", "")
            End If


            If txtOrdernummer.Text.Length = 0 Then
                m_change.SucheLeasingvertragsNr = ""
            Else
                m_change.SucheLeasingvertragsNr = txtOrdernummer.Text.Replace(" ", "")
            End If

            txtFahrgestellnummer.Text = Replace(txtFahrgestellnummer.Text.Trim(" "c).Trim("*"c), " ", "")
            If txtFahrgestellnummer.Text.Length = 0 Then
                m_change.SucheFahrgestellNr = ""
            Else
                m_change.SucheFahrgestellNr = txtFahrgestellnummer.Text
                If m_change.SucheFahrgestellNr.Length < 17 Then
                    If m_change.SucheFahrgestellNr.Length > 4 Then
                        txtFahrgestellnummer.Text = "*" & m_change.SucheFahrgestellNr
                        m_change.SucheFahrgestellNr = "%" & m_change.SucheFahrgestellNr
                    Else
                        lblerror.Text = "Bitte geben Sie die Fahrgestellnummer mindestens 8-stellig ein."
                        b = False
                    End If
                End If
            End If
            If txtZZREFERENZ1.Text.Length = 0 Then
                m_change.UnitNr = ""
            Else
                m_change.UnitNr = txtZZREFERENZ1.Text
            End If

            If txtAmtlKennzeichen.Text.Length = 0 Then
                m_change.SucheKennzeichen = ""
            Else
                txtAmtlKennzeichen.Text = Replace(txtAmtlKennzeichen.Text.Trim(" "c), " ", "")
                m_change.SucheKennzeichen = txtAmtlKennzeichen.Text
                'prüfe auf Eingabeformat Kreis und ein Buchstabe JJU2008.04.07
                Dim tmpaKennzeichen As String()
                tmpaKennzeichen = txtAmtlKennzeichen.Text.Split(",".ToCharArray)
                Dim tmpStr As String
                Dim tmpStr2 As String
                For Each tmpStr2 In tmpaKennzeichen
                    tmpStr = tmpStr2.Replace("*", "")
                    If Not tmpStr.IndexOf("-") = -1 Then
                        If Not tmpStr.Length > tmpStr.IndexOf("-") + 1 OrElse tmpStr.IndexOf("-") = 0 OrElse tmpStr.Length < 3 Then
                            lblerror.Text = "Bitte beachten Sie das Eingabeformat für Kennzeichen."
                            b = False
                            Exit For
                        End If
                    Else
                        lblerror.Text = "Bitte beachten Sie das Eingabeformat für Kennzeichen."
                        b = False
                        Exit For
                    End If

                Next
            End If
        Else
            b = True
        End If
        If b Then
            m_change.EquiTyp = "B"
            m_change.GiveCars(Session("AppID").ToString, Session.SessionID, Me)
            Dim blnGo As Boolean = False
            If Not m_change.Status = 0 Then
                lblerror.Text = m_change.Message
                lblerror.Visible = True
            Else
                If m_change.Result.Rows.Count = 0 Then
                    lblerror.Text = "Zu den gewählten Kriterien wurden keine Ergebnisse gefunden."
                Else
                    blnGo = True
                End If
            End If


            If blnGo Then
                Session("AppChange") = m_change
                Response.Redirect("Change03_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
            End If
        End If
    End Sub


    Private Sub upload(ByVal uFile As System.Web.HttpPostedFile)
        'Try
        Dim filepath As String = ConfigurationManager.AppSettings("ExcelPath")
        Dim filename As String
        Dim info As System.IO.FileInfo

        'Dateiname: User_yyyyMMddhhmmss.xls
        filename = m_User.UserName & "_" & Format(Now, "yyyyMMddhhmmss") & ".xls"

        If Not (uFile Is Nothing) Then
            uFile.SaveAs(ConfigurationManager.AppSettings("ExcelPath") & filename)
            info = New System.IO.FileInfo(filepath & filename)
            If Not (info.Exists) Then
                lblerror.Text = "Fehler beim Speichern."
                Exit Sub
            End If

            'Datei gespeichert -> Auswertung
            Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
             "Data Source=" & filepath & filename & ";" & _
             "Extended Properties=""Excel 8.0;HDR=YES;"""

            Dim objConn As New OleDbConnection(sConnectionString)
            objConn.Open()

            Dim objCmdSelect As New OleDbCommand("SELECT * FROM [Tabelle1$]", objConn)

            Dim objAdapter1 As New OleDbDataAdapter()
            objAdapter1.SelectCommand = objCmdSelect

            Dim objDataset1 As New DataSet()
            objAdapter1.Fill(objDataset1, "XLData")

            Dim tblTemp As DataTable = CheckInputTable(objDataset1.Tables(0))

            objConn.Close()

            If IsNothing(tblTemp) Then
                Exit Sub
            End If

            If Not tblTemp.Rows Is Nothing AndAlso tblTemp.Rows.Count > 0 Then
                m_change.Fahrzeuge = tblTemp
                Session("AppChange") = m_change
                Response.Redirect("Change03_2.aspx?AppID=" & Session("AppID").ToString & "&art=" & versandart)
            Else
                lblerror.Text = "Datei enthielt keine verwendbaren Daten."
            End If
        End If
        'Catch
        '    Throw New Exception(Err.Description)
        'End Try
    End Sub

    Private Function CheckInputTable(ByVal tblInput As DataTable) As DataTable

        Dim i As Integer = 0
        Dim rowData As DataRow
        Dim tblReturn As DataTable = Nothing

        For Each rowData In tblInput.Rows
            i += 1
            If TypeOf rowData(0) Is System.DBNull Then Exit For

            Dim Fahrgestellnummer As String = ""
            If Not TypeOf rowData(0) Is System.DBNull Then
                Fahrgestellnummer = CStr(rowData(0)).Trim(" "c)
            End If
            'Dim strKennzeichen As String = ""
            'If Not TypeOf rowData(1) Is System.DBNull Then
            '    strKennzeichen = CStr(rowData(1)).Trim(" "c)
            'End If

            If Fahrgestellnummer.Length = 0 Then Exit For


            m_change.SucheKennzeichen = ""
            m_change.SucheLeasingvertragsNr = ""
            m_change.SucheFahrgestellNr = Fahrgestellnummer
            m_change.Haendlernummer = ""
            m_change.KUNNR = ""

            m_change.EquiTyp = "B"
            m_change.GiveCars(Session("AppID").ToString, Session.SessionID, Me)

            If IsNothing(m_change.Fahrzeuge) Then
                If i = 1 Then
                    lblerror.Text = "Fahrzeug nicht gefunden."
                    booError = True
                    Return Nothing
                End If
            End If


            If tblReturn Is Nothing Then
                tblReturn = m_change.GiveResultStructure
            End If

            Dim ColumnCounter As Integer = tblReturn.Columns.Count - 1
            Dim j As Integer
            Dim rowNew As DataRow
            rowNew = tblReturn.NewRow
            If m_change.Status = 0 Then
                Dim CarRow() As DataRow
                CarRow = m_change.Fahrzeuge.Select("CHASSIS_NUM ='" & m_change.SucheFahrgestellNr & "'")
                rowNew("MANDT") = CarRow(0)("MANDT").ToString
                rowNew("EQUNR") = CarRow(0)("EQUNR").ToString
                rowNew("LIZNR") = CarRow(0)("LIZNR").ToString
                rowNew("CHASSIS_NUM") = m_change.SucheFahrgestellNr
                rowNew("TIDNR") = CarRow(0)("TIDNR").ToString
                rowNew("LICENSE_NUM") = CarRow(0)("LICENSE_NUM").ToString
                rowNew("ZZREFERENZ1") = CarRow(0)("ZZREFERENZ1").ToString
                rowNew("STATUS") = m_change.Message
                rowNew("ZZZLDAT") = CarRow(0)("ZZZLDAT")
                tblReturn.Rows.Add(rowNew)
            Else
                If m_change.Fahrzeuge.Rows.Count = 0 Then
                    rowNew("MANDT") = "010"
                    rowNew("LIZNR") = ""
                    rowNew("CHASSIS_NUM") = m_change.SucheFahrgestellNr
                    rowNew("TIDNR") = ""
                    rowNew("LICENSE_NUM") = ""
                    rowNew("ZZREFERENZ1") = ""
                    rowNew("ZZCOCKZ") = ""
                    rowNew("STATUS") = "Keine Daten gefunden."
                Else
                    For j = 0 To ColumnCounter
                        rowNew(j) = m_change.Fahrzeuge.Rows(0)(j)
                    Next
                    rowNew("MANDT") = "010"
                End If
                tblReturn.Rows.Add(rowNew)
            End If
 
        Next

        Return tblReturn
    End Function

    Private Sub CheckAuswahl()

        If rb_Einzelauswahl.Checked = True Then
            tr_Leasingvertragsnummer.Visible = True
            tr_Kennzeichen.Visible = True
            tr_KennzeichenZusatz.Visible = True
            tr_Fahrgestellnummer.Visible = True
            tr_FahrgestellnummerZusatz.Visible = True
            tr_NummerZB2.Visible = True
            tr_Platzhaltersuche.Visible = True
            tr_Alle.Visible = True
            tr_ZZREFERENZ1.Visible = True
            tr_upload.Visible = False
        Else
            tr_Leasingvertragsnummer.Visible = False
            tr_Kennzeichen.Visible = False
            tr_KennzeichenZusatz.Visible = False
            tr_Fahrgestellnummer.Visible = False
            tr_FahrgestellnummerZusatz.Visible = False
            tr_NummerZB2.Visible = False
            tr_ZZREFERENZ1.Visible = False
            tr_Platzhaltersuche.Visible = False
            tr_Alle.Visible = False
            tr_upload.Visible = True
        End If

    End Sub


#End Region


    Protected Sub rb_Upload_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Upload.CheckedChanged
        CheckAuswahl()
    End Sub

    Protected Sub rb_Einzelauswahl_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rb_Einzelauswahl.CheckedChanged
        CheckAuswahl()
    End Sub

    Protected Sub lbCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreate.Click
        Try

            If rb_Upload.Checked = True Then

                'Prüfe Fehlerbedingung
                If (Not upFile1.PostedFile Is Nothing) AndAlso (Not (upFile1.PostedFile.FileName = String.Empty)) Then
                    'lblExcelfile.Text = upFile1.PostedFile.FileName
                    If Right(upFile1.PostedFile.FileName.ToUpper, 4) <> ".XLS" Then
                        lblerror.Text = "Es können nur Dateien im .XLS - Format verarbeitet werden."
                        Exit Sub
                    End If
                Else
                    lblerror.Text = "Keine Datei ausgewählt"
                    Exit Sub
                End If

                booError = False

                'Lade Datei
                upload(upFile1.PostedFile)

                If booError = True Then Exit Sub



            Else

                If Not txtAmtlKennzeichen.Text = String.Empty Then
                    txtAmtlKennzeichen.Text = Replace(txtAmtlKennzeichen.Text, " ", "").Trim(","c)
                    If txtAmtlKennzeichen.Text.Length = 0 Then
                        txtAmtlKennzeichen.Text = String.Empty
                    End If
                End If
                If Not chk_alle.Checked Then
                    If (txtZZREFERENZ1.Text = String.Empty And txtAmtlKennzeichen.Text = String.Empty And _
                        txtOrdernummer.Text = String.Empty And txtFahrgestellnummer.Text = String.Empty And _
                                                                        txtNummerZB2.Text = String.Empty) Then
                        lblerror.Text = "Bitte geben Sie mindestens ein Suchkriterium an."
                        Exit Sub
                    End If
                End If

            End If

            DoSubmit()
        Catch ex As Exception
            'lblerror.Text = Err.Description
        End Try

    End Sub
End Class