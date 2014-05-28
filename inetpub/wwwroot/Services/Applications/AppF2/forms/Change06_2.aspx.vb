Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Business

Partial Public Class Change06_2
    Inherits System.Web.UI.Page

#Region "Declarations"

    Private m_App As App
    Private m_User As User
    Private objHaendler As Haendler
    Private objBank As AppF2BankBaseCredit
    Private objSuche As AppF2.Search
    Private m_Change As AppF2BankBaseCredit
    Private m_context As HttpContext = HttpContext.Current

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuth(Me, m_User)
        m_App = New App(m_User)
        Kopfdaten1.Message = ""

        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        objSuche = CType(Session("objSuche"), AppF2.Search)

        If IsPostBack = False Then

            
            m_Change = CType(Session("m_change"), AppF2BankBaseCredit)

            'wenn ein Konkreter Händler ausgwählt, dann kopfdaten und Kontingente füllen
            Kopfdaten1.Visible = True


            If Not objSuche.LeseHaendlerSAP_Einzeln(Session("AppID").ToString, Session.SessionID.ToString, m_Change.Customer, Me) Then
                lblError.Text = "Fehler bei der Ermittlung der Händlerdaten.<br>(" & objSuche.ErrorMessage & ")"
            Else
                Session("objSuche") = objSuche
            End If

            'bankObjekt für Kontingente instanziieren 
            objBank = New AppF2BankBaseCredit(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "")
            objBank.Customer = objSuche.REFERENZ
            objBank.KUNNR = m_User.KUNNR
            objBank.CreditControlArea = "ZDAD"
            objBank.Show(Session("AppID").ToString, Session.SessionID, Me) 'kontingentetabelle füllen
            Session("objBank") = objBank
            Kopfdaten1.Kontingente = objBank.Kontingente 'kontingente anzeigen

            'Kopfdatenfüllen
            Kopfdaten1.UserReferenz = m_User.Reference
            Kopfdaten1.HaendlerNummer = m_Change.Customer
            Dim strTemp As String = objSuche.NAME
            If objSuche.NAME_2.Length > 0 Then
                strTemp &= "<br>" & objSuche.NAME_2
            End If
            Kopfdaten1.HaendlerName = strTemp
            Kopfdaten1.Adresse = objSuche.STREET & "<br>" & objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY

            If objBank.Status = 0 Then
                If Not IsPostBack Then
                    StartLoadData()
                End If

                cmdSave.Enabled = True
                cmdConfirm.Enabled = True
                cmdReset.Enabled = True
            Else
                lblError.Text = objBank.Message

            End If

            m_context.Cache.Insert("objBank", objBank, New System.Web.Caching.CacheDependency(Server.MapPath("Change06_2.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)

        Else 'wenn postback

            objBank = CType(m_context.Cache("objBank"), AppF2BankBaseCredit)
            Kopfdaten1.Kontingente = objBank.Kontingente
        End If

    End Sub

    Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
        DoSubmit1()
    End Sub

    Protected Sub cmdConfirm_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdConfirm.Click
        DoSubmit2()
    End Sub

    Protected Sub cmdReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdReset.Click
        objBank.Show(Session("AppID").ToString, Session.SessionID, Me)
        StartLoadData()
        m_context.Cache.Insert("objBank", objBank, New System.Web.Caching.CacheDependency(Server.MapPath("Change06_2.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
    End Sub

    Protected Sub lbHaendlersuche_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbHaendlersuche.Click
        Response.Redirect("Change06.aspx?AppID=" & Page.Session("AppID"))
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
        HelpProcedures.FixedGridViewCols(GridView1)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region

#Region "Methods"

    Private Sub FillGrid()
        GridView1.DataSource = objBank.Kontingente
        GridView1.DataBind()

        Dim intKreditlimit As Int32
        Dim intAusschoepfung As Int32
        Dim blnGesperrt As Boolean

        Dim cell As TableCell
        Dim chkBox As CheckBox
        Dim label As Label
        Dim textbox As TextBox
        Dim control As Control
        Dim Row As GridViewRow


        For Each Row In GridView1.Rows
            Dim blnZeigeKontingentart As Boolean
            cell = Row.Cells(Row.Cells.Count - 1)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    blnZeigeKontingentart = chkBox.Checked
                End If
            Next
            cell = Row.Cells(2)
            For Each control In cell.Controls
                If TypeOf control Is Label Then
                    label = CType(control, Label)
                    If label.ID = "lblKontingent_Alt" And blnZeigeKontingentart Then
                        label.Visible = True
                        intKreditlimit = CInt(label.Text)
                    Else
                        label.Visible = False
                        If label.ID = "lblRichtwert_Alt" And (Not blnZeigeKontingentart) Then
                            label.Visible = True
                            intKreditlimit = CInt(label.Text)
                        Else
                            label.Visible = False
                        End If
                    End If
                End If
            Next

            intAusschoepfung = CInt(Row.Cells(3).Text)

            cell = Row.Cells(4)
            For Each control In cell.Controls
                If TypeOf control Is Label Then
                    label = CType(control, Label)
                    If Not blnZeigeKontingentart Then
                        label.Visible = False
                    End If
                End If
            Next

            cell = Row.Cells(5)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    blnGesperrt = chkBox.Checked
                    If Not blnZeigeKontingentart Then
                        chkBox.Visible = False
                    End If
                End If
            Next

            cell = Row.Cells(6)
            For Each control In cell.Controls
                If TypeOf control Is TextBox Then
                    textbox = CType(control, TextBox)
                    If textbox.ID = "txtKontingent_Neu" And blnZeigeKontingentart Then
                        textbox.Visible = True
                    Else
                        textbox.Visible = False
                        If textbox.ID = "txtRichtwert_Neu" And (Not blnZeigeKontingentart) Then
                            textbox.Visible = True
                        Else
                            textbox.Visible = False
                        End If
                    End If
                End If
            Next

            cell = Row.Cells(7)
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    If Not blnZeigeKontingentart Then
                        chkBox.Visible = False
                    End If
                End If
            Next

            If blnZeigeKontingentart Then
                If blnGesperrt Then
                    For Each cell In Row.Cells
                    Next
                Else
                    'If Not objChange40.ZeigeGesperrt Then
                    If (intAusschoepfung > intKreditlimit) Then
                        cell.ForeColor = System.Drawing.Color.Red
                    End If
                    'End If
                End If
            End If
        Next
    End Sub

    Private Sub DoSubmit1()
        Dim intKreditlimit_Alt As Int32
        Dim intKreditlimit_Neu As Int32
        Dim intRichtwert_Alt As Int32
        Dim intRichtwert_Neu As Int32
        Dim intAusschoepfung As Int32
        Dim blnGesperrt_Alt As Boolean
        Dim blnGesperrt_Neu As Boolean
        Dim strChangeMessage As String = ""

        Dim cell As TableCell
        Dim chkBox As CheckBox
        Dim textbox As TextBox
        Dim image As System.Web.UI.WebControls.Image
        Dim control As Control
        Dim blnChanged As Boolean = False

        Dim i As Int32 = 0

        Dim Row As GridViewRow

        For Each Row In GridView1.Rows
            'Werte ermitteln

            'Alt
            Dim blnZeigeKontingentart As Boolean = CBool(objBank.Kontingente.Rows(i)("ZeigeKontingentart"))
            If blnZeigeKontingentart Then
                intKreditlimit_Alt = CInt(objBank.Kontingente.Rows(i)("Kontingent_Alt"))
                intRichtwert_Alt = CInt(objBank.Kontingente.Rows(i)("Richtwert_Alt"))
                intRichtwert_Neu = CInt(objBank.Kontingente.Rows(i)("Richtwert_Neu"))
                blnGesperrt_Alt = CBool(objBank.Kontingente.Rows(i)("Gesperrt_Alt"))
            Else
                intKreditlimit_Alt = CInt(objBank.Kontingente.Rows(i)("Kontingent_Alt"))
                intKreditlimit_Neu = CInt(objBank.Kontingente.Rows(i)("Kontingent_Neu"))
                intRichtwert_Alt = CInt(objBank.Kontingente.Rows(i)("Richtwert_Alt"))
                blnGesperrt_Alt = CBool(objBank.Kontingente.Rows(i)("Gesperrt_Alt"))
                blnGesperrt_Neu = CBool(objBank.Kontingente.Rows(i)("Gesperrt_Neu"))
            End If
            intAusschoepfung = CInt(objBank.Kontingente.Rows(i)("Ausschoepfung"))
            i += 1

            'Neu
            cell = Row.Cells(6)
            For Each control In cell.Controls
                If TypeOf control Is TextBox Then
                    textbox = CType(control, TextBox)
                    If IsNumeric(textbox.Text) AndAlso (textbox.Text.Length < 5) AndAlso (Not CInt(textbox.Text) < 0) Then

                        If textbox.ID = "txtKontingent_Neu" And blnZeigeKontingentart Then
                            intKreditlimit_Neu = CInt(textbox.Text)
                        Else
                            If textbox.ID = "txtRichtwert_Neu" And (Not blnZeigeKontingentart) Then
                                intRichtwert_Neu = CInt(textbox.Text)
                            End If
                        End If

                    Else
                        strChangeMessage &= "Bitte geben Sie numerische, positive und max. vierstellige Kontigentwerte ein.<br>"
                    End If
                End If
            Next

            cell = Row.Cells(7)
            blnGesperrt_Neu = False
            For Each control In cell.Controls
                If TypeOf control Is CheckBox Then
                    chkBox = CType(control, CheckBox)
                    blnGesperrt_Neu = chkBox.Checked
                End If
            Next

            cell = Row.Cells(6)
            If (Not (intKreditlimit_Alt = intKreditlimit_Neu)) Or (Not (intRichtwert_Alt = intRichtwert_Neu)) Then
                For Each control In cell.Controls
                    If TypeOf control Is System.Web.UI.WebControls.Image Then
                        image = CType(control, System.Web.UI.WebControls.Image)
                        image.ImageUrl = "/Services/Images/Pfeil_vor_01.jpg"
                    End If
                Next
                blnChanged = True
            Else
                For Each control In cell.Controls
                    If TypeOf control Is System.Web.UI.WebControls.Image Then
                        image = CType(control, System.Web.UI.WebControls.Image)
                        image.ImageUrl = "/Services/Images/empty.gif"
                    End If
                Next
            End If

            cell = Row.Cells(7)
            If blnGesperrt_Alt = blnGesperrt_Neu Then
                For Each control In cell.Controls
                    If TypeOf control Is System.Web.UI.WebControls.Image Then
                        image = CType(control, System.Web.UI.WebControls.Image)
                        image.ImageUrl = "/Services/Images/empty.gif"
                    End If
                Next
            Else
                For Each control In cell.Controls
                    If TypeOf control Is System.Web.UI.WebControls.Image Then
                        image = CType(control, System.Web.UI.WebControls.Image)
                        image.ImageUrl = "/Services/Images/Pfeil_vor_01.jpg"
                    End If
                Next
                blnChanged = True
            End If

            For Each cell In Row.Cells
                cell.ForeColor = System.Drawing.Color.Black
            Next
            If blnZeigeKontingentart Then
                If blnGesperrt_Neu Then
                    For Each cell In Row.Cells
                        cell.ForeColor = System.Drawing.Color.Red
                    Next
                Else

                    If (intAusschoepfung > intKreditlimit_Neu) Then
                        For Each cell In Row.Cells
                            cell.ForeColor = System.Drawing.Color.Red
                        Next
                    End If

                End If
            End If
        Next

        If blnChanged Then
            If strChangeMessage.Length = 0 Then
                For Each Row In GridView1.Rows
                    cell = Row.Cells(6)
                    For Each control In cell.Controls
                        If TypeOf control Is TextBox Then
                            textbox = CType(control, TextBox)
                            textbox.Enabled = False
                        End If
                    Next
                    cell = Row.Cells(7)
                    For Each control In cell.Controls
                        If TypeOf control Is CheckBox Then
                            chkBox = CType(control, CheckBox)
                            chkBox.Enabled = False
                        End If
                    Next
                Next
            End If

            cmdSave.Visible = False
            cmdConfirm.Visible = True
            cmdReset.Visible = True

        Else
            strChangeMessage &= "Die Werte wurden nicht geändert."
        End If
        lblNoData.Text = strChangeMessage
        lblError.Text = strChangeMessage

        m_context.Cache.Insert("objBank", objBank, New System.Web.Caching.CacheDependency(Server.MapPath("Change06_2.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
    End Sub


    Private Sub DoSubmit2()
        Dim logApp As New CKG.Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

        Try

            Dim strKontingentart As String = ""
            Dim intKreditlimit_Alt As Int32
            Dim intKreditlimit_Neu As Int32
            Dim intRichtwert_Alt As Int32
            Dim intRichtwert_Neu As Int32
            Dim intAusschoepfung As Int32
            Dim blnGesperrt_Alt As Boolean
            Dim blnGesperrt_Neu As Boolean

            Dim cell As TableCell
            Dim chkBox As CheckBox
            Dim textbox As TextBox
            Dim control As Control
            Dim i As Int32 = 0

            Dim Row As GridViewRow

            For Each Row In GridView1.Rows
                'Werte ermitteln

                'Alt
                Dim blnZeigeKontingentart As Boolean = CBool(objBank.Kontingente.Rows(i)("ZeigeKontingentart"))
                If blnZeigeKontingentart Then
                    intKreditlimit_Alt = CInt(objBank.Kontingente.Rows(i)("Kontingent_Alt"))
                    intRichtwert_Alt = CInt(objBank.Kontingente.Rows(i)("Richtwert_Alt"))
                    intRichtwert_Neu = CInt(objBank.Kontingente.Rows(i)("Richtwert_Neu"))
                    blnGesperrt_Alt = CBool(objBank.Kontingente.Rows(i)("Gesperrt_Alt"))
                Else
                    intKreditlimit_Alt = CInt(objBank.Kontingente.Rows(i)("Kontingent_Alt"))
                    intKreditlimit_Neu = CInt(objBank.Kontingente.Rows(i)("Kontingent_Neu"))
                    intRichtwert_Alt = CInt(objBank.Kontingente.Rows(i)("Richtwert_Alt"))
                    blnGesperrt_Alt = CBool(objBank.Kontingente.Rows(i)("Gesperrt_Alt"))
                    blnGesperrt_Neu = CBool(objBank.Kontingente.Rows(i)("Gesperrt_Neu"))
                End If
                intAusschoepfung = CInt(objBank.Kontingente.Rows(i)("Ausschoepfung"))
                strKontingentart = CStr(objBank.Kontingente.Rows(i)("Kontingentart"))
                i += 1

               
                cell = Row.Cells(6)
                For Each control In cell.Controls
                    If TypeOf control Is TextBox Then
                        textbox = CType(control, TextBox)
                        If textbox.ID = "txtKontingent_Neu" And blnZeigeKontingentart Then
                            intKreditlimit_Neu = CInt(textbox.Text)
                        Else
                            If textbox.ID = "txtRichtwert_Neu" And (Not blnZeigeKontingentart) Then
                                intRichtwert_Neu = CInt(textbox.Text)
                            End If
                        End If
                    End If
                Next


                cell = Row.Cells(7)
                blnGesperrt_Neu = False
                For Each control In cell.Controls
                    If TypeOf control Is CheckBox Then
                        chkBox = CType(control, CheckBox)
                        blnGesperrt_Neu = chkBox.Checked
                    End If
                Next

                If Not ((intKreditlimit_Alt = intKreditlimit_Neu) And (intRichtwert_Alt = intRichtwert_Neu) And (blnGesperrt_Alt = blnGesperrt_Neu)) Then
                    objBank.Kontingente.AcceptChanges()
                    Dim tmpRows As DataRow()
                    tmpRows = objBank.Kontingente.Select("Kontingentart = '" & strKontingentart & "'")
                    tmpRows(0).BeginEdit()
                    tmpRows(0).Item("Gesperrt_Neu") = blnGesperrt_Neu

                    tmpRows(0).Item("Kontingent_Neu") = intKreditlimit_Neu
                    tmpRows(0).Item("Richtwert_Neu") = intRichtwert_Neu
                    
                    tmpRows(0).EndEdit()
                    objBank.Kontingente.AcceptChanges()

                End If
            Next

            Dim tblLogDetails As DataTable = GetChanges()


            'Anwendung erfordert keine Autorisierung (Level=0)

            objBank.Change(Session("AppID"), Session.SessionID, Me)
            If objBank.Status = 0 Then

                logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Kontingent von Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ") erfolgreich geändert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, tblLogDetails)
                lblNoData.Text = "<b>Ihre Daten wurden gespeichert.</b><br>&nbsp;"
                lblNoData.Visible = True

            Else
                logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Fehler bei der Kontingentänderung von Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ", Fehler: " & objBank.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
                lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & objBank.Message & ")"

            End If
            logApp.WriteStandardDataAccessSAP(objBank.IDSAP)
            objBank.Show(Session("AppID").ToString, Session.SessionID, Me)
            StartLoadData()
           
        Catch ex As Exception
            m_App.WriteErrorText(1, m_User.UserName, "Change06_2", "DoSubmit2", ex.ToString)

            logApp.WriteEntry("ERR", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Session("SelectedDealer").ToString, "Fehler bei der Kontingentänderung von Händler " & objSuche.REFERENZ & " (DAD: " & Session("SelectedDealer").ToString & ", Fehler: " & ex.Message & ")", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
            lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            lblError.CssClass = "TextError"

            objBank.Show(Session("AppID").ToString, Session.SessionID, Me)
            Throw ex
        End Try
        m_context.Cache.Insert("objBank", objBank, New System.Web.Caching.CacheDependency(Server.MapPath("Change06_2.aspx")), DateTime.Now.AddMinutes(20), TimeSpan.Zero)
    End Sub


    Private Sub StartLoadData()

        cmdSave.Visible = True
        cmdConfirm.Visible = False
        cmdReset.Visible = False

        If (objBank.Kontingente Is Nothing) OrElse (objBank.Kontingente.Rows.Count = 0) Then
            lblError.Text = "Fehler: Es konnten keine Kontingentdaten ermittelt werden."

        Else
            Kopfdaten1.Kontingente = objBank.Kontingente
            FillGrid()
        End If
    End Sub

    Private Function GetChanges() As DataTable
        Dim m_tblKontingenteChanged As DataTable
        m_tblKontingenteChanged = New DataTable()
        m_tblKontingenteChanged.Columns.Add("Status", System.Type.GetType("System.String"))
        m_tblKontingenteChanged.Columns.Add("Händler", System.Type.GetType("System.String"))
        m_tblKontingenteChanged.Columns.Add("Kontingentart", System.Type.GetType("System.String"))
        m_tblKontingenteChanged.Columns.Add("Kontingent", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Richtwert", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Ausschoepfung", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Frei", System.Type.GetType("System.Int32"))
        m_tblKontingenteChanged.Columns.Add("Gesperrt", System.Type.GetType("System.Boolean"))

        Dim rowTemp As DataRow
        For Each rowTemp In objBank.Kontingente.Rows
            Dim tmpRow2 As DataRow
            tmpRow2 = m_tblKontingenteChanged.NewRow
            tmpRow2("Status") = "Alt"
            tmpRow2("Händler") = objSuche.REFERENZ
            tmpRow2("Kontingentart") = rowTemp("Kontingentart")
            tmpRow2("Kontingent") = rowTemp("Kontingent_Alt")
            tmpRow2("Richtwert") = rowTemp("Richtwert_Alt")
            tmpRow2("Ausschoepfung") = rowTemp("Ausschoepfung")
            tmpRow2("Frei") = CInt(rowTemp("Kontingent_Alt")) - CInt(rowTemp("Ausschoepfung"))
            tmpRow2("Gesperrt") = rowTemp("Gesperrt_Alt")
            m_tblKontingenteChanged.Rows.Add(tmpRow2)
            If (Not CInt(rowTemp("Kontingent_Alt")) = CInt(rowTemp("Kontingent_Neu"))) Or (Not CInt(rowTemp("Richtwert_Alt")) = CInt(rowTemp("Richtwert_Neu"))) Or (Not CBool(rowTemp("Gesperrt_Alt")) = CBool(rowTemp("Gesperrt_Neu"))) Then
                tmpRow2 = m_tblKontingenteChanged.NewRow
                tmpRow2("Status") = "Neu"
                tmpRow2("Händler") = objSuche.REFERENZ
                tmpRow2("Kontingentart") = rowTemp("Kontingentart")
                tmpRow2("Kontingent") = rowTemp("Kontingent_Neu")
                tmpRow2("Richtwert") = rowTemp("Richtwert_Neu")
                tmpRow2("Ausschoepfung") = rowTemp("Ausschoepfung")
                tmpRow2("Frei") = CInt(rowTemp("Kontingent_Neu")) - CInt(rowTemp("Ausschoepfung"))
                tmpRow2("Gesperrt") = rowTemp("Gesperrt_Neu")
                m_tblKontingenteChanged.Rows.Add(tmpRow2)
            End If
        Next
        Return m_tblKontingenteChanged
    End Function


#End Region


End Class
' ************************************************
' $History: Change06_2.aspx.vb $
' 
' *****************  Version 6  *****************
' User: Dittbernerc  Date: 5.04.11    Time: 16:30
' Updated in $/CKAG2/Applications/AppF2/forms
' FixGridViewCols:
' 
' Elemente wie Textboxen, Dropdownlisten, und Checkboxen werden in der
' Berechnung mit ber�cksichtigt.
' Die Headrow wird nur noch einmalig abgefragt nicht pro abgefragter Row
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 10.09.09   Time: 10:15
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 3.09.09    Time: 11:27
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 18.08.09   Time: 16:55
' Updated in $/CKAG2/Applications/AppF2/forms
' ITA: 3077
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 18.08.09   Time: 11:33
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.08.09   Time: 16:49
' Created in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 13.08.09   Time: 12:58
' Updated in $/CKAG2/Applications/AppF2/forms
' ITA: 3071
' 