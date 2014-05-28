Option Infer On

Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common


Partial Public Class Report20
    Inherits Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private m_objTrace As Base.Kernel.Logging.Trace
    Private objSuche As Base.Business.Search
    Protected WithEvents tr_Filialen As HtmlTableRow
    Protected WithEvents TRdistrikte As HtmlTableRow


    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Session("ShowLink") = "False"
        Session("ShowOtherString") = ""
        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        Common.NoDealer(Me, m_User)

        Common.GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

        m_App = New Base.Kernel.Security.App(m_User)


        If Not IsPostBack Then

            txtAbDatum.Text = Today.ToShortDateString

            txtBisDatum.Text = Today.ToShortDateString

            Dim appAssigned As New Base.Kernel.Admin.ApplicationList(m_User.Customer.CustomerId, m_User.App.Connectionstring)
            appAssigned.GetAssigned()

            Dim dvLinks = appAssigned.DefaultView
            dvLinks.RowFilter = "AppType='Change' AND AppInMenu=1"
            dvLinks.Sort = "AppRank"

            Dim tblApplikationen = New DataTable("")
            tblApplikationen.Columns.Add("AppID", GetType(Int32))
            tblApplikationen.Columns.Add("Applikation", GetType(String))

            For i As Integer = 0 To dvLinks.Count - 1
                Dim rowTemp = tblApplikationen.NewRow
                rowTemp("AppID") = dvLinks.Item(i)("AppID")
                Dim strTemp As String
                If Not TypeOf dvLinks.Item(i)("AppComment") Is DBNull Then
                    strTemp = CStr(dvLinks.Item(i)("AppComment"))
                Else
                    strTemp = ""
                End If
                If strTemp.Length = 0 Then
                    rowTemp("Applikation") = CStr(dvLinks.Item(i)("AppFriendlyName"))
                Else
                    rowTemp("Applikation") = CStr(dvLinks.Item(i)("AppFriendlyName")) & " - " & strTemp
                End If
                tblApplikationen.Rows.Add(rowTemp)
            Next


            optChange.DataTextField = "Applikation"
            optChange.DataValueField = "AppID"
            optChange.DataSource = tblApplikationen
            optChange.DataBind()
            ''########### O.Rudolph ITA:946 Distriktstruktur 13.04.2007
            objSuche = New Base.Business.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            Dim districtCount = ReadDistricts()
            Session("objSuche") = objSuche
            If districtCount > 0 Then
                tr_Filialen.Visible = False
                TRdistrikte.Visible = True
                If districtCount = 1 Then
                    ddlDistrikte.Visible = False
                    lblDistrikte.Text = ddlDistrikte.SelectedItem.Text
                End If
            ElseIf districtCount = 0 Then '####### alte Filialstruktur beibehalten 18.04.2007
                TRdistrikte.Visible = False
                tr_Filialen.Visible = True
                'lblError.Text = "Ihnen wurde bisher noch kein Distrikt zugeordnet!" & vbCrLf & _
                ' "Bitte wenden Sie sich an Ihren Administrator!"
                lblFiliale.Text = m_User.Organization.OrganizationName
                Dim dtOrganizations As New Base.Kernel.Admin.OrganizationList(m_User.Customer.CustomerId, m_User.App.Connectionstring, m_User.Customer.AccountingArea)

                dtOrganizations.AddAllNone(True, False)
                With ddlFiliale
                    .Items.Clear()
                    Dim dv = dtOrganizations.DefaultView
                    dv.Sort = "OrganizationName"
                    .DataSource = dv
                    .DataTextField = "OrganizationName"
                    .DataValueField = "OrganizationID"
                    .DataBind()
                    If IsNumeric(m_User.Organization.OrganizationId) Then
                        Dim li = .Items.FindByValue(m_User.Organization.OrganizationId.ToString)
                        If Not (li Is Nothing) Then
                            li.Selected = True
                        End If
                    End If
                End With
                ddlFiliale.Visible = m_User.Organization.AllOrganizations OrElse m_User.HighestAdminLevel > Base.Kernel.Security.AdminLevel.Customer

                lblFiliale.Visible = Not ddlFiliale.Visible
            End If
            'lblFiliale.Text = m_User.Organization.OrganizationName
            'Dim dtOrganizations As New OrganizationList(m_User.Customer.CustomerId, m_User.App.Connectionstring)
            'dtOrganizations.AddAllNone(True, False)
            'With ddlFiliale
            '    .Items.Clear()
            '    Dim dv As DataView = dtOrganizations.DefaultView
            '    dv.Sort = "OrganizationName"
            '    .DataSource = dv
            '    .DataTextField = "OrganizationName"
            '    .DataValueField = "OrganizationID"
            '    .DataBind()
            '    If IsNumeric(m_User.Organization.OrganizationId) Then
            '        Dim _li As ListItem = .Items.FindByValue(m_User.Organization.OrganizationId.ToString)
            '        If Not (_li Is Nothing) Then
            '            _li.Selected = True
            '        End If
            '    End If
            'End With
            'ddlFiliale.Visible = m_User.Organization.AllOrganizations OrElse m_User.HighestAdminLevel > AdminLevel.Customer
            'lblFiliale.Visible = Not ddlFiliale.Visible
        End If
    End Sub

    Private Sub cmdCreate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdcreate.Click
        Session("ShowLink") = "True"
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        Session("ShowOtherString") = ""
        Try
            lblError.Text = ""
            If Not IsDate(txtAbDatum.Text) Then
                If Not Common.IsStandardDate(txtAbDatum.Text) Then
                    If Not Common.IsSAPDate(txtAbDatum.Text) Then
                        lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                    End If
                End If
            End If
            If Not IsDate(txtBisDatum.Text) Then
                If Not Common.IsStandardDate(txtBisDatum.Text) Then
                    If Not Common.IsSAPDate(txtBisDatum.Text) Then
                        lblError.Text = "Geben Sie bitte ein gültiges Datum ein!<br>"
                    End If
                End If
            End If
            Dim datAb As Date = CDate(txtAbDatum.Text)
            Dim datBis As Date = CDate(txtBisDatum.Text)
            If datAb > datBis Then
                lblError.Text = "Das Enddatum darf nicht vor dem Anfangsdatum liegen!<br>"
            End If
            If lblError.Text.Length > 0 Then
                Exit Sub
            End If

            m_objTrace = New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP)

            Dim intSelectedOrganizationId As Integer = 0
            Dim districtCount As Integer = CInt(Session("DistrictCount"))
            If districtCount = 0 Then '####### alte Filialstruktur beibehalten 18.04.2007
                If Not ddlFiliale.SelectedItem Is Nothing AndAlso IsNumeric(ddlFiliale.SelectedItem.Value) Then
                    intSelectedOrganizationId = CInt(ddlFiliale.SelectedItem.Value)
                End If
            Else
                ''########### O.Rudolph ITA:946 Distriktstruktur 13.04.2007
                If Not ddlDistrikte.SelectedItem Is Nothing AndAlso IsNumeric(ddlDistrikte.SelectedItem.Value) Then
                    intSelectedOrganizationId = CInt(ddlDistrikte.SelectedItem.Value)
                End If
            End If
            Dim tblResult As DataTable
            tblResult = m_objTrace.GetLogOverViewPerSource(optChange.SelectedItem.Value, txtAbDatum.Text, CDate(txtBisDatum.Text).AddDays(1).ToShortDateString, m_User.Customer.CustomerId, intSelectedOrganizationId)

            Session("ResultTable") = tblResult

            If tblResult.Rows.Count = 0 Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                Dim strFileName = String.Format("{0:yyyyMMdd_HHmmss}_{1}.xls", DateTime.Now, m_User.UserName)

                Excel.ExcelExport.WriteExcel(tblResult, ConfigurationManager.AppSettings("ExcelPath") & strFileName)

                Session("lnkExcel") = "/Services/Temp/Excel/" & strFileName
                Session("ShowOtherString") = optChange.SelectedItem.Text
                Response.Redirect("/Services/(S(" & Session.SessionID & "))/Shared/Report01_2.aspx?AppID=" & Session("AppID").ToString)
            End If
        Catch ex As Exception
            If ex.Message = "NO_DATA" Or ex.Message = "NO_WEB" Then
                lblError.Text = "Keine Ergebnisse für die gewählten Kriterien."
            Else
                lblError.Text = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End If
        End Try
    End Sub

    Private Function ReadDistricts() As Integer
        'diesen ganzen block nur einmal, da die dropdown sich eigentlich die werte über einen postback hinaus merken sollte.
        Dim districtCount As Integer

        'Hier Zugriff auf neue BAPI....
        Dim appId As Integer = CInt(Session("AppID"))
        districtCount = objSuche.ReadDistrictSAP(appId.ToString, Session.SessionID)
        If districtCount > 0 Then
            With ddlDistrikte
                .Items.Clear()
                'dropdown füllen:
                .DataSource = objSuche.District
                .DataTextField = "NAME1"
                .DataValueField = "DISTRIKT"
                .DataBind()
                'vorbelegten distrikt suchen
                objSuche.District.RowFilter = "VORBELEGT='X'"
                Dim drv As DataRowView
                For Each drv In objSuche.District
                    Dim li As ListItem = .Items.FindByValue(drv("DISTRIKT").ToString)
                    If Not li Is Nothing Then
                        If Not .SelectedItem Is Nothing Then
                            .SelectedItem.Selected = False ' falls schon ein anderer selektiert, dann deselektieren.
                        End If
                        li.Selected = True
                    End If
                    Exit For ' nach dem ersten aussteigen, da nur einer selektiert sein darf!!!
                Next
            End With
        End If
        Session("DistrictCount") = districtCount
        Return districtCount
    End Function



    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        Common.SetEndASPXAccess(Me)
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" & Session.SessionID & "))/Start/Selection.aspx?AppID=" & Session("AppID").ToString, False)
    End Sub
End Class

' ************************************************
' $History: Report20.aspx.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 11.05.11   Time: 20:48
' Updated in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 23.03.10   Time: 16:43
' Updated in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 11.03.10   Time: 15:53
' Updated in $/CKAG2/Services/Components/ComCommon
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 21.07.09   Time: 16:53
' Created in $/CKAG2/Services/Components/ComCommon
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 5.06.09    Time: 15:58
' Updated in $/CKAG/Components/ComCommon
' mögliche try catches entfernt
' 
' *****************  Version 5  *****************
' User: Hartmannu    Date: 9.09.08    Time: 16:36
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 11.04.08   Time: 13:43
' Updated in $/CKAG/Components/ComCommon
' Migration
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon
' Migration
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 10.04.08   Time: 12:52
' Updated in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:47
' Created in $/CKAG/Services/Components/ComCommon
' 
' *****************  Version 17  *****************
' User: Jungj        Date: 12.02.08   Time: 8:17
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Fieldtranslation ermöglicht
' 
' *****************  Version 16  *****************
' User: Uha          Date: 12.07.07   Time: 9:28
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 20.06.07   Time: 13:46
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 15.06.07   Time: 10:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 15.06.07   Time: 10:55
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 15.06.07   Time: 10:52
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 15.06.07   Time: 10:51
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' 
' *****************  Version 10  *****************
' User: Uha          Date: 3.05.07    Time: 18:27
' Updated in $/CKG/Components/ComCommon/ComCommonWeb
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************