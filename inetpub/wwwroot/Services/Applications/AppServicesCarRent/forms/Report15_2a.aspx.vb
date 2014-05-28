Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Services
Imports CKG.Base.Business
Imports CKG.EasyAccess
Imports CKG.Base.Kernel.DocumentGeneration
Imports Telerik.Web.UI

Partial Public Class Report15_2a
    Inherits Page

#Region "Declarations"

    Private _app As Security.App
    Private _user As Security.User
    Private _historie As Historie

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        _user = GetUser(Me)
        FormAuth(Me, _user)

        Try
            _app = New Security.App(_user)

            lblHead.Text = _user.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString

            If Not IsPostBack Then

                'Fülle Schlüsselinformationen
                '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                FillKeyInfo()

            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

#End Region

    Private Sub FillKeyInfo()

        If Not Session("objReport") Is Nothing Then
            _historie = CType(Session("objReport"), Historie)

            With _historie
                If Not String.IsNullOrEmpty(_historie.FahrgestellNr) Then
                    lblFahrzeugIdTitel.Text = "Fahrgestellnummer:"
                    lblFahrzeugId.Text = _historie.FahrgestellNr
                Else
                    lblFahrzeugIdTitel.Text = "Kennzeichen:"
                    lblFahrzeugId.Text = _historie.Kennzeichen
                End If
                Session("AppResultStueckliste") = .ResultStueckliste
                'Befüllen des Grids läuft über das "NeedDataSource"-Event
            End With
        End If

    End Sub

    Private Function createUebermittlungsTable() As DataTable
        Dim uebersicht As New DataTable
        uebersicht.Columns.Add("Komponentennr.", GetType(String))
        uebersicht.Columns.Add("Komponententyp", GetType(String))
        uebersicht.Columns.Add("Aktivität", GetType(String))
        uebersicht.Columns.Add("Sachbearbeiter", GetType(String))
        uebersicht.Columns.Add("Datum", GetType(String))
        uebersicht.Columns.Add("Uhrzeit", GetType(String))
        uebersicht.Columns.Add("Versandadresse", GetType(String))

        Dim tueteTable As DataTable = CType(Session("AppResultStueckliste"), DataTable)
        If Not tueteTable Is Nothing Then
            For Each row In tueteTable.Rows

                Dim newRow = uebersicht.NewRow()

                newRow.Item("Komponentennr.") = row("STLKN").ToString
                newRow.Item("Komponententyp") = row("MAKTX").ToString
                newRow.Item("Aktivität") = row("AKTI_BEZEI").ToString
                newRow.Item("Sachbearbeiter") = row("UNAME").ToString
                newRow.Item("Datum") = CType(row("DATUM"), Date).ToString("dd.MM.yyyy")
                newRow.Item("Uhrzeit") = getTimeString(row("UZEIT").ToString, row("AKTI_BEZEI").ToString)
                If String.IsNullOrEmpty(row("NAME1").ToString) AndAlso String.IsNullOrEmpty(row("NAME2").ToString) AndAlso String.IsNullOrEmpty(row("STREET").ToString) Then
                    newRow.Item("Versandadresse") = ""
                Else
                    newRow.Item("Versandadresse") = row("NAME1").ToString & " " & row("NAME2").ToString & " / " & row("STREET").ToString & " " & row("HOUSE_NUM1").ToString & " / " & row("POST_CODE1").ToString & " " & row("CITY1").ToString & " " & row("COUNTRY").ToString
                End If

                uebersicht.Rows.Add(newRow)
                uebersicht.AcceptChanges()
            Next
        End If

        uebersicht.TableName = "Komponenten"
        Return uebersicht
    End Function

    Private Sub lbBack_Click(sender As Object, e As EventArgs) Handles lbBack.Click
        Response.Redirect("Report15.aspx?AppID=" & Session("AppID").ToString())
    End Sub

    Protected Sub rgLebenslaufTueteStkl_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgLebenslaufTueteStkl.NeedDataSource
        If Not (Session("AppResultStueckliste") Is Nothing) Then
            rgLebenslaufTueteStkl.DataSource = CType(Session("AppResultStueckliste"), DataTable).DefaultView
        End If
    End Sub

    Protected Sub ibtNewSearch_Click(sender As Object, e As ImageClickEventArgs) Handles ibtNewSearch.Click
        Response.Redirect("./Report15.aspx?AppID=" & Session("AppID").ToString() & "&B=true")
    End Sub

    ''' <summary>
    ''' Formatierung eines Zeit-Strings
    ''' </summary>
    ''' <param name="timetext"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getTimeString(timetext As String, aktivitaet As String) As String
        Dim erg As String = timetext

        'Bei Ersteingang Zeit nicht gesetzt
        If Not String.IsNullOrEmpty(aktivitaet) AndAlso aktivitaet.ToUpper = "ERSTEINGANG" Then
            erg = ""
        ElseIf Not String.IsNullOrEmpty(timetext) Then
            If timetext.Length = 6 Then
                If timetext.StartsWith(" ") Then
                    erg = "0" & timetext.Substring(1, 1) & ":" & timetext.Substring(2, 2) & ":" & timetext.Substring(4, 2)
                Else
                    erg = timetext.Substring(0, 2) & ":" & timetext.Substring(2, 2) & ":" & timetext.Substring(4, 2)
                End If
            ElseIf timetext.Length = 5 Then
                erg = "0" & timetext.Substring(0, 1) & ":" & timetext.Substring(1, 2) & ":" & timetext.Substring(3, 2)
            End If
        End If

        Return erg
    End Function

End Class
