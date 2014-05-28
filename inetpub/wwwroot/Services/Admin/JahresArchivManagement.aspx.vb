Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common.Common
Imports Admin.Kernel
Imports System.Data.SqlClient

Partial Public Class JahresArchivManagement
    Inherits Page

#Region " Membervariables "
    Private m_User As User
    Private m_App As App
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_User = GetUser(Me)
        lblHead.Text = "Jahresarchiv-Verwaltung"
        AdminAuth(Me, m_User, AdminLevel.Master)
        'wenn SuperUser und übergeordnete Firma
        If m_User.Customer.AccountingArea = -1 Then
            lnkAppManagement.Visible = True
        End If


        Try
            m_App = New App(m_User)

            lblError.Text = ""
            lblMessage.Text = ""

            If Not IsPostBack Then
                InitDestiantionYearDate()
            End If

        Catch ex As Exception
            lblError.Text = ex.ToString
            m_App.WriteErrorText(1, m_User.UserName, "JahresArchivManagement", "PageLoad", lblError.Text)
        End Try
    End Sub

#Region " Data and Function "

    Private Sub InitDestiantionYearDate()
        Dim today As Date = DateTime.Today
        Dim year As Int32 = today.Year + 1
        If (today.Month <= 1) Then
            year = today.Year
        End If

        tbDestinationYear.Text = year.ToString()
    End Sub

#End Region

#Region " Events "

    Private Sub BtnCopyYearArchivesClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyYearArchives.Click

        Dim cn As New SqlClient.SqlConnection(m_User.App.Connectionstring)

        Try
            cn.Open()

            Dim destinationYear As String
            destinationYear = tbDestinationYear.Text
            If (destinationYear.Length = 4) Then
                destinationYear = destinationYear.Substring(2, 2)
            End If
            Dim sourceYear As String = (Int32.Parse(destinationYear) - 1).ToString()

            Dim archivesMatchingList As List(Of String) = Archiv.GetArchivesForYearMatch(cn, sourceYear)

            divStart.Visible = False
            divResults.Visible = True
            If (Not archivesMatchingList.Any()) Then
                lblResult.Text = "Hinweis: Es wurde keine passenden Archive zum Kopieren gefunden!"
                Return
            End If

            lblResult.Text = "<b>Kopiervorgang Jahres-Archive abgeschlossen!</b><br /><br />Folgende " & archivesMatchingList.Count & " Archive wurden verarbeitet: <br /> <br />"
            For Each sourceArchiveName As String In archivesMatchingList
                Dim sc As New SqlCommand("JahresArchivKopieren", cn)
                sc.CommandType = CommandType.StoredProcedure
                sc.Parameters.AddWithValue("@srcArchiveName", sourceArchiveName)
                sc.Parameters.AddWithValue("@srcYY", sourceYear)
                sc.Parameters.AddWithValue("@dstYY", destinationYear)
                Dim returnValue As SqlParameter = sc.Parameters.Add("RetValue", SqlDbType.Int)
                returnValue.Direction = ParameterDirection.ReturnValue
                sc.ExecuteNonQuery()

                If (returnValue.Value = 0) Then
                    lblResult.Text += " - <font color=green>Archiv '" & sourceArchiveName & "' => '" & sourceArchiveName.Replace(sourceYear, destinationYear) & "' wurde erfolgreich kopiert!</font><br />"
                Else
                    lblResult.Text += " - <font color=orange>Archiv '" & sourceArchiveName & "' => '" & sourceArchiveName.Replace(sourceYear, destinationYear) & "' wurde nicht kopiert da Ziel-Archiv u. U. bereits vorhanden!</font><br />"
                End If
            Next

        Catch ex As Exception
            divStart.Visible = False
            divResults.Visible = True
            lblResult.Text = "Es ist ein Fehler aufgetreten: " & ex.Message

        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try


    End Sub

#End Region

End Class