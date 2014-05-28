Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common

Partial Public Class _Report01s
    Inherits System.Web.UI.Page

    Private strDocument As String
    Private m_App As Security.App
    Private m_User As Security.User
    Private showCheckbox As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        FormAuthNoReferrer(Me, m_User)

        strDocument = Request.QueryString("I")
        If (strDocument Is Nothing) OrElse (strDocument = String.Empty) Then
            lblError.Text = "Kein Dokument gefunden!"
            Exit Sub
        End If
        Try
            m_App = New Security.App(m_User)
            If Not IsPostBack Then
                loadForm(strDocument)
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub loadForm(ByVal strDocument As String)
        Dim easy As EasyAccess.EasyAccess
        'Dim archives As EasyAccess.EasyArchive = easy.getArchives()
        Dim ids As String()
        Dim strArcLoc As String
        Dim strArcNam As String
        Dim strDocID As String
        Dim strDocVer As String
        'Rückgabeparameter
        Dim lngLaenge As Long
        Dim strErstellDat As String = ""
        Dim strAenderDat As String = ""
        Dim strTitel As String = ""
        Dim intFelderGes As Integer
        Dim intTextFelder As Integer
        Dim intBildFelder As Integer
        Dim intBlobFelder As Integer
        Dim status As String = ""

        easy = CType(Session("EasyAccess"), EasyAccess.EasyAccess)

        Try
            ids = strDocument.Split("."c)
            strArcLoc = ids(0)
            strArcNam = ids(1)
            strDocID = ids(2)
            strDocVer = ids(3)

            easy.getDocumentInfo(strArcLoc, strArcNam, strDocID, strDocVer, lngLaenge, strErstellDat, strAenderDat, strTitel, intFelderGes, intTextFelder, intBildFelder, intBlobFelder, status)

            If (status <> String.Empty) Then
                lblError.Text = "Fehler beim Laden des Dokumentes."
            Else
                txtAenderDatum.Text = strAenderDat
                txtErstellDatum.Text = strErstellDat
                txtFelderBild.Text = intBildFelder.ToString
                txtFelderBlob.Text = intBlobFelder.ToString
                txtFelderGesamt.Text = intFelderGes.ToString
                txtFelderText.Text = intTextFelder.ToString
                txtLAenge.Text = lngLaenge.ToString
                txtTitel.Text = strTitel
            End If

        Catch ex As Exception
            lblError.Text = "Fehler beim Laden der Daten."
            Exit Sub
        End Try

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class