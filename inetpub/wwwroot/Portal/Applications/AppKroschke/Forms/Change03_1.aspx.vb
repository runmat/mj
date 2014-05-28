Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.IO

Partial Public Class Change03_1
    Inherits System.Web.UI.Page


    Private m_User As Base.Kernel.Security.User
    'Private m_App As Base.Kernel.Security.App

    Private mObjFahrerAuftraege As FahrerAuftraege

#Region "Properties"

    Private Property Refferer() As String
        Get
            If Not Session.Item(Request.Url.LocalPath & "Refferer") Is Nothing Then
                Return Session.Item(Request.Url.LocalPath & "Refferer").ToString()
            Else : Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            Session.Item(Request.Url.LocalPath & "Refferer") = value
        End Set
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        m_User = GetUser(Me)
        'm_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)
        lblError.Text = ""


        If mObjFahrerAuftraege Is Nothing Then
            If Not Session("mObjFahrerAuftraegeSession") Is Nothing Then
                mObjFahrerAuftraege = CType(Session("mObjFahrerAuftraegeSession"), FahrerAuftraege)
            Else
                Throw New Exception("Benötigtes Session Objekt nicht vorhanden")
            End If
        End If



        If Not IsPostBack Then
            If Refferer Is Nothing Then
                If Not Request.UrlReferrer Is Nothing Then
                    Refferer = Request.UrlReferrer.ToString
                Else
                    Refferer = ""
                End If
            End If

            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            If mObjFahrerAuftraege.Auswahl = "" Then
                lblHead.Text &= " (Neue Aufträge)"
                legende.Visible = True
            Else
                lblHead.Text &= " (Angenommene Aufträge)"
                legende.Visible = False
            End If
            ucStyles.TitleText = lblHead.Text

            'seitenspezifisch
            FillAuftragsGrid(0, "VDATU", "Asc")
        End If
        Dim tmpDataView As New DataView(mObjFahrerAuftraege.Auftraege)
        tmpDataView.RowFilter = "Fahrer_Status = 'OK'"
        If tmpDataView.Count > 0 Then
            LoadDocuments()
        End If

    End Sub

    Private Sub LoadDocuments()
        Dim files As String()
        Dim info As FileInfo
        Dim i As Integer
        Dim fname As String

        For Each auftraegeRow As DataGridItem In dgAuftraege.Items
            Dim sFahrer_Status As String = auftraegeRow.Cells(0).Text
            If sFahrer_Status = "OK" Then
                Dim sVBELN As String = auftraegeRow.Cells(1).Text.PadLeft(10, "0"c).ToString
                Dim sKunnr As String = auftraegeRow.Cells(12).Text

                'Dim sPath As String = "D:\Dokumente\" + sKunnr.PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                Dim sPath As String = ConfigurationManager.AppSettings("UploadPathSambaArchive") + sKunnr.PadLeft(10, "0"c).ToString + "\" + sVBELN + "\Vertraege\"
                If Directory.Exists(sPath) Then
                    files = Directory.GetFiles(sPath, sVBELN & "*")
                    For i = 0 To files.Length - 1
                        info = New FileInfo(files.GetValue(i).ToString)
                        fname = info.Name
                        If Left(fname, 10) = sVBELN Then
                            Dim ibtn As New ImageButton
                            ibtn.ID = "ibtnProtokoll" & i
                            ibtn.ImageUrl = "../../../Images/pdf.gif"
                            ibtn.Width = Unit.Pixel(20)
                            ibtn.Height = Unit.Pixel(20)
                            ibtn.ToolTip = fname
                            ibtn.CommandName = "PrintProtokolle"
                            ibtn.CommandArgument = info.FullName
                            auftraegeRow.Cells(2).Controls.Add(ibtn)
                        End If
                    Next
                End If
            End If
            
        Next
    End Sub

    Private Sub responseBack()
        If Refferer = "" Then
            Dim strLinkPrefix As String = "/" & ConfigurationManager.AppSettings("WebAppPath") & "/"
            Response.Redirect(strLinkPrefix & "Start/Selection.aspx")
        Else
            Response.Redirect(Refferer)
        End If
    End Sub

    Protected Sub cmdBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
        responseBack()
    End Sub

    Private Sub FillAuftragsGrid(ByVal intPageIndex As Int32, Optional ByVal strSort As String = "", Optional ByVal direction As String = "")

        If IsNothing(mObjFahrerAuftraege.Auftraege) OrElse mObjFahrerAuftraege.Auftraege.Rows.Count = 0 Then

            dgAuftraege.Visible = False
            lblInfo.Text = "Anzahl: 0"
        Else

            dgAuftraege.Visible = True

            Dim tmpDataView As New DataView(mObjFahrerAuftraege.Auftraege)


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
                    If ViewState("Direction") Is Nothing Then
                        strDirection = "asc"
                        ViewState("Direction") = strDirection
                    Else
                        strDirection = ViewState("Direction").ToString
                    End If
                End If
            End If

            If Not strTempSort.Length = 0 Then
                If direction.Length > 0 Then
                    strDirection = direction
                End If
                tmpDataView.Sort = strTempSort & " " & strDirection
            End If

            lblInfo.Text = "Anzahl: " & tmpDataView.Count

            dgAuftraege.CurrentPageIndex = intTempPageIndex

            dgAuftraege.DataSource = tmpDataView

            dgAuftraege.DataBind()

            If mObjFahrerAuftraege.Auswahl = "" Then
                For Each tmpItem As DataGridItem In dgAuftraege.Items
                    Select Case tmpItem.Cells(0).Text
                        Case "NO"
                            tmpItem.BackColor = Color.FromArgb(255, 255, 163, 0)
                        Case "OK"
                            tmpItem.BackColor = Color.YellowGreen
                    End Select
                Next
            End If
        End If

    End Sub

    Private Sub dgAuftraege_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAuftraege.ItemCommand
        If e.CommandName = "annehmen" Then
            mObjFahrerAuftraege.Change(CStr(e.CommandArgument), "OK", Session("AppID").ToString, Session.SessionID)
            FillAuftragsGrid(dgAuftraege.CurrentPageIndex)
            LoadDocuments()

        ElseIf e.CommandName = "ablehnen" Then
            mObjFahrerAuftraege.Change(CStr(e.CommandArgument), "NO", Session("AppID").ToString, Session.SessionID)
            FillAuftragsGrid(dgAuftraege.CurrentPageIndex)
        End If
        
        If e.CommandName = "getPDF" Then
            Dim filename As String = getPDF(CStr(e.CommandArgument))
            If Not filename Is Nothing Then
                Dim openScript As String
                openScript = _
                   "<" & "script language=""JavaScript"">" & _
                      "var win = window.open('../../../Temp/Excel/" & filename & "','unerheblich'," & _
                         "'width=670,height=700,left=20,top=20,resizable=YES, scrollbars=YES,menubar=NO');" & _
                   "</" & "script>"
                Response.Write(openScript)
            End If
        End If

        If e.CommandName = "PrintProtokolle" Then
            Session("App_ContentType") = "Application/pdf"
            Session("App_Filepath") = e.CommandArgument
            'Dim bc As HttpBrowserCapabilities
            'bc = Request.Browser
           
            Dim openScript As String
            openScript = _
               "<" & "script language=""JavaScript"">" & _
                  "window.open(""Printpdf.aspx"", ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" & _
               "</" & "script>"
            Response.Write(openScript)

        End If

    End Sub
    
    Private Sub dgAuftraege_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgAuftraege.SortCommand
        FillAuftragsGrid(dgAuftraege.CurrentPageIndex, e.SortExpression)
    End Sub

    Private Function getPDF(ByVal Auftragsnummer As String) As String
        Dim pdfBin As Byte() = mObjFahrerAuftraege.getPDFBinaryFromSAP(Auftragsnummer, Session("AppID").ToString, Session.SessionID)
        If mObjFahrerAuftraege.Status = 0 Then
            If Not pdfBin Is Nothing Then
                Dim strFilename As String = "FahrerAuftrag" & Now.ToString.Replace("."c, "").Replace(" ", "").Replace(":", "") & Auftragsnummer & ".pdf"
                Dim strFilePath As String = ConfigurationManager.AppSettings("ExcelPath") & "FahrerAuftrag" &
                    Now.ToString.Replace("."c, "").Replace(" ", "").Replace(":", "") & Auftragsnummer & ".pdf"
                File.WriteAllBytes(strFilePath, pdfBin)
                Return strFilename
            Else
                Return Nothing
            End If
        Else
            lblError.Text = mObjFahrerAuftraege.Message
            Return Nothing
        End If

    End Function

End Class

' ************************************************
' $History: Change03_1.aspx.vb $
' 
' *****************  Version 11  *****************
' User: Fassbenders  Date: 26.05.11   Time: 9:17
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 25.05.11   Time: 22:00
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 16.02.11   Time: 13:51
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 8.02.11    Time: 15:06
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 7.02.11    Time: 15:10
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 3.02.11    Time: 10:06
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 2.02.11    Time: 9:00
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 28.07.09   Time: 13:27
' Updated in $/CKAG/Applications/AppKroschke/Forms
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 8.07.09    Time: 15:30
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA 2641 response ohne page html
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 2.07.09    Time: 15:24
' Updated in $/CKAG/Applications/AppKroschke/Forms
' ITA 2641 Testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 1.07.09    Time: 16:45
' Created in $/CKAG/Applications/AppKroschke/Forms
' ITA 2641 unfertig
'
' ************************************************
