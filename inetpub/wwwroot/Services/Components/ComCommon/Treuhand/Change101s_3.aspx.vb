Option Strict On
Option Explicit On

Imports CKG.Base.Kernel.Common
Imports CKG.Components.ComCommon.Treuhand
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business
Imports Telerik.Web.UI
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO

Namespace Treuhand

    Partial Public Class Change101s_3
        Inherits System.Web.UI.Page

#Region "Declarations"
        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private CustomerObject As SperreFreigabe
        Private tblDaten As DataTable
#End Region

#Region "PageEvents"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            m_User = Common.GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            Common.FormAuth(Me, m_User)

            lblMessage.Text = ""

            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            Common.GetAppIDFromQueryString(Me) 'füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            CustomerObject = CType(Session("SperrObject"), SperreFreigabe)
            If Session("tblDaten") IsNot Nothing Then
                tblDaten = CType(Session("tblDaten"), DataTable)
            End If

            If Not IsPostBack Then
                Common.TranslateTelerikColumns(rgGrid1)
                CheckUpload()
                'Info weitergeben, ob CheckUpload die Anzeige des Senden-Buttons zulässt
                FillGrid(cmdSave.Visible)
            End If

        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            Common.SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            Common.SetEndASPXAccess(Me)
        End Sub

#End Region

#Region "Methods"

        ''' <summary>
        ''' Gridview prüfen
        ''' </summary>
        ''' <returns>Anzahl der Gridviewzeilen</returns>
        ''' <remarks></remarks>
        Private Function CheckGrid() As Int32
            Dim chbox As CheckBox
            Dim intReturn As Int32 = 0
            Dim tmpRows As DataRow()

            For Each item As GridDataItem In rgGrid1.Items
                Dim strZZFAHRG As String = ""
                strZZFAHRG = "EQUI_KEY = '" & item("EQUI_KEY").Text & "'"
                tmpRows = CustomerObject.tblUpload.Select(strZZFAHRG)
                If (tmpRows.Length > 0) Then
                    tmpRows(0).BeginEdit()
                    chbox = CType(item.FindControl("chkAnfordern"), CheckBox)
                    If chbox.Checked Then           'anfordern
                        tmpRows(0).Item("SELECT") = "99"
                        intReturn += 1
                    End If

                    tmpRows(0).EndEdit()
                    CustomerObject.tblUpload.AcceptChanges()
                End If
            Next

            Session("CustomerObject") = CustomerObject
            Return intReturn
        End Function

        Private Sub FillGrid(Optional blnAllowShowSaveButton As Boolean = True)

            If CustomerObject.tblUpload.DefaultView.Count = 0 Then
                SearchMode(True, blnAllowShowSaveButton)
                lblError.Text = "Keine Daten zur Anzeige gefunden."
            Else
                lblError.Text = ""
                lblMessage.Text = "Bitte wählen Sie die Vorgänge ab, die Sie nicht absenden wollen."
                SearchMode(False, blnAllowShowSaveButton)

                If CustomerObject.tblUpload.Select("Message <> ''").Length > 0 Then
                    rgGrid1.Columns.FindByUniqueName("ERROR").Visible = True
                End If

                tblDaten = CustomerObject.tblUpload
                Session("tblDaten") = tblDaten
                rgGrid1.Rebind()
                'Setzen der DataSource geschieht durch das NeedDataSource-Event
            End If

        End Sub

        Private Sub FillGrid2()

            If CustomerObject.Fahrzeuge.DefaultView.Count = 0 Then
                SearchMode()
                lblError.Text = "Keine Daten zur Anzeige gefunden."
            Else
                lblError.Text = ""

                SearchMode(False)

                rgGrid1.Columns.FindByUniqueName("SELECT").Visible = False
                rgGrid1.Columns.FindByUniqueName("SPERRDAT").Visible = False
                rgGrid1.Columns.FindByUniqueName("TREUH_VGA").Visible = True

                tblDaten = CustomerObject.Fahrzeuge
                Session("tblDaten") = tblDaten
                rgGrid1.Rebind()
                'Setzen der DataSource geschieht durch das NeedDataSource-Event
            End If

        End Sub

        Private Sub SearchMode(Optional search As Boolean = True, Optional blnAllowShowSaveButton As Boolean = True)
            Result.Visible = Not search
            cmdCheck.Visible = Not search
            cmdSave.Visible = blnAllowShowSaveButton And Not search
        End Sub

        Protected Sub rgGrid1_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs)
            If tblDaten IsNot Nothing Then
                rgGrid1.DataSource = tblDaten.DefaultView
            Else
                rgGrid1.DataSource = Nothing
            End If
        End Sub

        Protected Sub rgGrid1_ItemDataBound(sender As Object, e As GridItemEventArgs)

            If TypeOf (e.Item) Is GridDataItem Then
                Dim item As GridDataItem = CType(e.Item, GridDataItem)

                'Standardstyle setzen
                item.Style.Item("background-color") = "transparent !Important;"

                Dim equikey As String = item("EQUI_KEY").Text

                Dim rows As DataRow() = CustomerObject.tblUpload.Select("EQUI_KEY='" & equikey & "'")

                If rows IsNot Nothing AndAlso rows.Length > 0 Then

                    Dim errText As String = rows(0)("ERROR").ToString()

                    If Not String.IsNullOrEmpty(errText) Then
                        If errText.StartsWith("FEHLER: ") Then
                            item("ERROR").Text = errText.Replace("FEHLER: ", "")
                            item.Style.Item("background-color") = "#F08080 !Important;"
                            item.BorderStyle = BorderStyle.Solid
                            item.BorderWidth = 1
                            item.BorderColor = Drawing.Color.LightGray

                        ElseIf errText.StartsWith("WARNUNG: ") Then
                            item("ERROR").Text = errText.Replace("WARNUNG: ", "")
                            item.Style.Item("background-color") = "#FFFFE0 !Important;"
                            item.BorderStyle = BorderStyle.Solid
                            item.BorderWidth = 1
                            item.BorderColor = Drawing.Color.LightGray

                        End If
                    End If

                End If

            End If

        End Sub

        Private Function CheckUpload() As Boolean

            Dim selRowCount As Integer = 0
            Dim isWarning As Boolean = False
            Dim isError As Boolean = False
            Dim sperrPruef As SperrPruefung

            lblError.Text = ""

            Try
                sperrPruef = New SperrPruefung(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
                sperrPruef.Treuhandgeber = CustomerObject.TREU
                sperrPruef.Auftraggeber = CustomerObject.AG
                sperrPruef.IsSperren = CustomerObject.IsSperren

                'Zurücksetzen der Formatierung für neue prüfung
                rgGrid1.Columns.FindByUniqueName("ERROR").Visible = False

                For Each row As DataRow In CustomerObject.tblUpload.Rows
                    If row("SELECT").Equals("99") Then
                        selRowCount += 1
                    End If
                Next

                'verhalten wenn nichts selektiert
                If selRowCount <= 0 Then
                    InfoHead.Text = "Information"
                    rgGrid1.Columns.FindByUniqueName("ERROR").Visible = False
                    InfoText.Text = "Bitte wählen Sie erst Fahrzeuge aus."
                    InfoText.ForeColor = Drawing.Color.Black
                    cmdSave.Visible = False
                    cmdCheck.Visible = True
                    Return False
                End If

                'GESAMTE TABELLE gegen Bapi Prüfen
                Dim pruefErg As SperrPruefung.UploadTableStatus = sperrPruef.CheckAllTable(Me, CustomerObject.tblUpload)

                If pruefErg = SperrPruefung.UploadTableStatus.Fehler Then
                    isError = True
                ElseIf pruefErg = SperrPruefung.UploadTableStatus.Warnung Then
                    isWarning = True
                End If

                'Setzen der Information je nach Fehlertyp 
                If isError Then
                    rgGrid1.Columns.FindByUniqueName("ERROR").Visible = True
                    InfoHead.Text = "Achtung!"
                    InfoText.ForeColor = Drawing.Color.Red
                    InfoText.Text = "Der Upload enthält Fehler"
                    cmdSave.Visible = False
                    Return False
                ElseIf isWarning Then
                    rgGrid1.Columns.FindByUniqueName("ERROR").Visible = True
                    InfoHead.Text = "Information"
                    InfoText.ForeColor = Drawing.Color.Black
                    InfoText.Text = "Der Upload enthält Warnungen"
                    cmdSave.Visible = True
                    Return True
                Else
                    rgGrid1.Columns.FindByUniqueName("ERROR").Visible = False
                    InfoHead.Text = "Information"
                    InfoText.ForeColor = Drawing.Color.DarkGreen
                    InfoText.Text = "Prüfung erfolgreich!"
                    cmdSave.Visible = True
                    Return True
                End If

            Catch ex As Exception
                lblError.Text = ex.Message
                Return False
            End Try

        End Function

#End Region

#Region "Events"

        Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

            'Workaround, um (versehentliches) Mehrfachabsenden zu verhindern
            If Session("Sent_Change101s_3") IsNot Nothing Then
                Dim letztesAbsenden As DateTime = CType(Session("Sent_Change101s_3"), DateTime)
                If (DateTime.Now - letztesAbsenden).TotalSeconds < 5 Then
                    lblError.Text = Session("SentMessage_Change101s_3").ToString()
                    pnInfo.Visible = False
                    cmdCheck.Visible = False
                    cmdSave.Visible = False
                    rgGrid1.Columns.FindByUniqueName("SELECT").Visible = False
                    rgGrid1.Columns.FindByUniqueName("ERROR").Visible = False
                    Exit Sub
                End If
            End If

            Dim iSelCount As Integer
            iSelCount = CheckGrid()

            lblError.Text = ""

            If iSelCount = 0 Then
                lblError.Text = "Bitte wählen Sie erst Fahrzeuge aus."
                FillGrid()
            Else
                If m_User.Groups.ItemByID(m_User.GroupID).Authorizationright > 0 Then
                    For Each tmpRow As DataRow In CustomerObject.tblUpload.Rows
                        If tmpRow("SELECT").ToString = "99" Then

                            Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

                            logApp.CollectDetails("Vertrags. - / Fahrgestellnr.", CType(tmpRow("EQUI_KEY").ToString, Object), True)
                            logApp.CollectDetails("Treunehmer", CType(CustomerObject.Treunehmer, Object))
                            logApp.CollectDetails("Sachbearbeiter", CType(tmpRow("ERNAM").ToString, Object))
                            logApp.CollectDetails("Sperrdatum", CType(tmpRow("SPERRDAT").ToString, Object))
                            logApp.CollectDetails("Datum", CType(tmpRow("ERDAT").ToString, Object))
                            CustomerObject.ReferenceforAut = tmpRow("EQUI_KEY").ToString

                            If tmpRow("TREUH_VGA").ToString = "S" Then
                                logApp.CollectDetails("Sperren", "X")
                                logApp.CollectDetails("Entsperren", "")
                            Else
                                logApp.CollectDetails("Sperren", "")
                                logApp.CollectDetails("Entsperren", "X")
                            End If

                            'CustomerObject.FinforAut = tmpRow("Fahrgestellnummer").ToString
                            'Anwendung erfordert Autorisierung (Level>0)
                            Dim DetailArray(1, 2) As Object
                            Dim ms As MemoryStream
                            Dim formatter As BinaryFormatter
                            Dim b() As Byte

                            ms = New MemoryStream()
                            formatter = New BinaryFormatter()
                            formatter.Serialize(ms, CustomerObject)
                            b = ms.ToArray
                            ms = New IO.MemoryStream(b)
                            DetailArray(0, 0) = ms
                            DetailArray(0, 1) = "CustomerObject"

                            'Pruefen, ob schon in der Autorisierung.
                            Dim strInitiator As String = ""
                            Dim intAuthorizationID As Int32

                            m_App.CheckForPendingAuthorization(CInt(Session("AppID")), m_User.Organization.OrganizationId, m_User.KUNNR, tmpRow("EQUI_KEY").ToString, m_User.IsTestUser, strInitiator, intAuthorizationID)
                            If Not strInitiator.Length = 0 Then
                                'Fahrzeug wurde schon mal freigegeben und liegt zur Autorisierung vor
                                tmpRow("MESSAGE") = "liegt zur Autorisierung vor"

                            Else
                                If tmpRow("TREUH_VGA").ToString = "S" Then
                                    intAuthorizationID = Common.WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, CustomerObject.Treunehmer, tmpRow("EQUI_KEY").ToString, "Sperren", "", m_User.IsTestUser, DetailArray)
                                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CustomerObject.Treunehmer, "Sperrung für Treunehmer " & CustomerObject.Treunehmer & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)

                                Else
                                    intAuthorizationID = Common.WriteAuthorization(m_App.Connectionstring, CInt(Session("AppID")), m_User.UserName, m_User.Organization.OrganizationId, CustomerObject.Treunehmer, tmpRow("EQUI_KEY").ToString, "Entsperren", "", m_User.IsTestUser, DetailArray)
                                    logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, CustomerObject.Treunehmer, "Entsperrung für Treunehmer " & CustomerObject.Treunehmer & " erfolgreich initiiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                                End If

                                tmpRow("MESSAGE") = "Aut."
                            End If
                        End If
                    Next
                    FillGrid()
                Else
                    CustomerObject.GiveCars(Me.Page, Session("AppID").ToString, Session.SessionID.ToString)
                    FillGrid2()
                    Dim ErrCount As Integer = 0
                    Dim RefCount As Integer = 0
                    Dim rows As DataRow() = CustomerObject.Fahrzeuge.Select("SUBRC <> 0")
                    ErrCount = rows.Length
                    For Each row As DataRow In rows
                        If row("MESSAGE").ToString.StartsWith("W") Then
                            ErrCount -= 1
                        ElseIf row("MESSAGE").ToString.StartsWith("I") Then
                            ErrCount -= 1
                            RefCount += 1
                        End If
                    Next

                    lblError.Text = "Es wurden " & iSelCount - ErrCount & " Fahrzeuge erfolgreich " & CustomerObject.SperrEnsperr & "!"
                    If ErrCount > 0 Then
                        lblError.Text &= "<br /> " & ErrCount & " Fahrzeuge mit Fehler!"
                    End If
                    If RefCount > 0 Then
                        lblError.Text &= "<br /> " & RefCount & " Referenznummern aktualisiert!"
                    End If

                    Session("CustomerObject") = CustomerObject

                    cmdCheck.Visible = False
                    pnInfo.Visible = False

                    Session("Sent_Change101s_3") = DateTime.Now
                    Session("SentMessage_Change101s_3") = lblError.Text
                End If

                cmdSave.Visible = False

                rgGrid1.Columns.FindByUniqueName("SELECT").Visible = False
                rgGrid1.Columns.FindByUniqueName("ERROR").Visible = False

            End If

        End Sub

        Protected Sub chkAnfordern_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

            If cmdSave.Visible Then
                cmdSave.Visible = False
                cmdCheck.Visible = True
            End If

            Dim tmpGridRow As GridDataItem = CType(CType(sender, CheckBox).Parent.Parent, GridDataItem)

            If CType(sender, CheckBox).Checked Then
                CustomerObject.tblUpload.Select("ID='" & tmpGridRow("ID").Text & "'")(0)("SELECT") = "99"
            Else
                CustomerObject.tblUpload.Select("ID='" & tmpGridRow("ID").Text & "'")(0)("SELECT") = ""
            End If

        End Sub

        Protected Sub cmdCheck_Click(sender As Object, e As EventArgs) Handles cmdCheck.Click
            CheckUpload()
            rgGrid1.Rebind()
        End Sub

        Protected Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
            Response.Redirect("Change101s.aspx?AppID=" & Session("AppID").ToString)
        End Sub

#End Region

    End Class

End Namespace