Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Services.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class Change03_5
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objZulassung As Arval_1

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_User = GetUser(Me)

        FormAuth(Me, m_User)
        lblHead.Text = "Druckansicht"
        'GetAppIDFromQueryString(Me)
        Try
            ' lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            'ucStyles.TitleText = lblHead.Text

            'm_App = New DADWebClass.App(m_User)

            If Session("objZulassung") Is Nothing Then
                Response.Redirect("Change03.aspx?AppID=" & Session("AppID").ToString)
            Else
                objZulassung = CType(Session("objZulassung"), Arval_1)
            End If

            If Not IsPostBack Then
                InitialLoad()
            Else
                'CheckGrid()
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub InitialLoad()
        Dim tmpDataView As New DataView()
        tmpDataView = objZulassung.Result.DefaultView

        Dim strContent As String = ""
        Dim strNum As Integer
        Dim strTemp As String = ""
        Dim bool As Boolean

        strContent &= "<br><br>"
        strContent &= Left("Datum" & ".........................", 25) & ".:&nbsp;<b>" & Now.ToString & "</b><br>"
        strContent &= Left("Benutzer" & ".........................", 25) & ".:&nbsp;<b>" & m_User.UserName & "</b><br><br>"

        For strNum = 0 To (tmpDataView.Count - 1)
            strContent &= "<br><u>Position:&nbsp;<b>" & Right("000" & CType(strNum + 1, String), 3) & "</b></u><br>"
            strContent &= Left("Leasingvertragsnummer" & ".........................", 25) & ".:&nbsp;<b>" & tmpDataView.Item(strNum)("LeasingNummer").ToString & "</b><br>"
            strContent &= Left("Halteranschrift" & ".........................", 25) & ".:&nbsp;<b>" & tmpDataView.Item(strNum)("HalterName1").ToString & "," & tmpDataView.Item(strNum)("HalterStr").ToString & "&nbsp;" & tmpDataView.Item(strNum)("HalterNr").ToString & "," & tmpDataView.Item(strNum)("HalterPlz").ToString & "&nbsp;" & tmpDataView.Item(strNum)("HalterOrt").ToString & "</b><br>"
            strContent &= Left("Versandadresse" & ".........................", 25) & ".:&nbsp;<b>" & tmpDataView.Item(strNum)("HaendlerName1").ToString & "," & tmpDataView.Item(strNum)("HaendlerStr").ToString & "&nbsp;" & tmpDataView.Item(strNum)("HaendlerNr").ToString & "," & tmpDataView.Item(strNum)("HaendlerPlz").ToString & "&nbsp;" & tmpDataView.Item(strNum)("HaendlerOrt").ToString & "</b><br>"
            strContent &= Left("Fahrgestellnummer" & ".........................", 25) & ".:&nbsp;<b>" & tmpDataView.Item(strNum)("FhgstNummer").ToString & "</b><br>"
            strContent &= Left("Datum Zulassung" & ".........................", 25) & ".:&nbsp;<b>" & CType(tmpDataView.Item(strNum)("DatumZulassung"), Date).ToShortDateString & "</b><br>"
            strContent &= Left("Wunschkennzeichen" & ".........................", 25) & ".:&nbsp;<b>" & tmpDataView.Item(strNum)("Wunschkennzeichen").ToString & "</b><br>"
            bool = CType(tmpDataView.Item(strNum)("Vorreserviert"), Boolean)
            strTemp = "Nein"
            If bool Then
                strTemp = "Ja"
            End If
            strContent &= Left("Vorreserviert" & ".........................", 25) & ".:&nbsp;<b>" & strTemp & "</b><br>"
            bool = CType(tmpDataView.Item(strNum)("Kennzeichenserie"), Boolean)
            strTemp = "Nein"
            If bool Then
                strTemp = "Ja"
            End If
            strContent &= Left("Aus Kennzeichenserie" & ".........................", 25) & ".:&nbsp;<b>" & strTemp & "</b><br>"
            strContent &= Left("OKZ Zulassung" & ".........................", 25) & ".:&nbsp;<b>" & tmpDataView.Item(strNum)("Zulstelle").ToString & "</b><br>"
            strContent &= Left("Versicherung" & ".........................", 25) & ".:&nbsp;<b>" & objZulassung.Versicherung.ToString & "</b><br>"
            strContent &= Left("Auftragsstatus" & ".........................", 25) & ".:&nbsp;<b>" & tmpDataView.Item(strNum)("Status").ToString & "</b><br>"
        Next

        lblContent.Text = strContent
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class

' ************************************************
' $History: Change03_5.aspx.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 16.04.09   Time: 17:06
' Updated in $/CKAG2/Applications/AppArval/forms
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Forms
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 10:05
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' Logging der Laufzeiten der ASPX-Seiten eingeführt
' 
' *****************  Version 5  *****************
' User: Uha          Date: 6.03.07    Time: 15:30
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' GetAppIDFromQueryString(Me) hinzugefügt
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Forms
' 
' ************************************************