Public Partial Class PM
    Inherits System.Web.UI.Page

    Protected m_dtLeft As DataTable
    Protected m_dtRight As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            getDataSource()
            fillGridViewLeft()
            fillGridViewRight()
            fillLaufSchriftFields()
        End If

    End Sub

    Private Sub fillGridViewLeft()

        gvLeft.DataSource = m_dtLeft
        gvLeft.DataBind()

    End Sub


    Private Sub fillGridViewRight()
        gvRight.DataSource = m_dtRight
        gvRight.DataBind()
    End Sub

    Private Sub getDataSource()
        Dim cn As SqlClient.SqlConnection
        Dim cmdAg As SqlClient.SqlCommand
        Dim ds As DataSet
        Dim ad As SqlClient.SqlDataAdapter
        cn = New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try

            cn.Open()

            ds = New DataSet()

            ad = New SqlClient.SqlDataAdapter()

            cmdAg = New SqlClient.SqlCommand("SELECT * FROM vwStatusmonitorDAD WHERE Platzierung='L' AND ServiceCenterID=4 ORDER BY Abteilung ", cn)
            cmdAg.CommandType = CommandType.Text

            ad.SelectCommand = cmdAg
            ad.Fill(ds, "Links")

            cmdAg.CommandText = "SELECT * FROM vwStatusmonitorDAD WHERE Platzierung='R' AND ServiceCenterID=4  ORDER BY Abteilung "
            ad.SelectCommand = cmdAg
            ad.Fill(ds, "Rechts")

            If ds.Tables("Links") Is Nothing OrElse ds.Tables("Rechts") Is Nothing Then
                Throw New Exception("Es wurden keine Datensätze gefunden")
            End If

            m_dtLeft = ds.Tables("Links")
            m_dtRight = ds.Tables("Rechts")

        Catch ex As Exception
            Throw ex
        Finally
            cn.Close()
        End Try

    End Sub

    Private Sub gvLeft_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvLeft.DataBinding

    End Sub

    Private Sub gvRight_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvRight.DataBinding

    End Sub

    Private Sub fillLaufSchriftFields()
        Dim tmpRow As DataRow
        Dim tmpRows() As DataRow

        'wollen einfache laufschrift JJU2008.06.19
        '------------------------------------------------------
        'hiddenLaufschriftLeft.Value = String.Empty
        'tmpRows = m_dtLeft.Select("Status='1' AND Infotext<>''")

        'For Each tmpRow In tmpRows
        '    Dim tmpStr As String
        '    tmpStr = tmpRow("Abteilung") & ": " & tmpRow("Infotext") & "|"
        '    hiddenLaufschriftLeft.Value = hiddenLaufschriftLeft.Value & tmpStr
        'Next


        'tmpRows = m_dtRight.Select("Status='1' AND Infotext<>''")
        'hiddenLaufschriftRight.Value = String.Empty

        'For Each tmpRow In tmpRows
        '    Dim tmpStr As String
        '    tmpStr = tmpRow("Abteilung") & ": " & tmpRow("Infotext") & "|"
        '    hiddenLaufschriftRight.Value = hiddenLaufschriftRight.Value & tmpStr
        'Next
        '------------------------------------------------------


        Dim tmpStr As String = String.Empty
        hiddenLaufschrift.Value = String.Empty

        tmpRows = m_dtRight.Select("Status='1' AND Infotext<>''")

        For Each tmpRow In tmpRows
            tmpStr = tmpStr & tmpRow("Abteilung") & ": " & tmpRow("Infotext") & "     **********     "
        Next

        tmpRows = m_dtLeft.Select("Status='1' AND Infotext<>''")

        For Each tmpRow In tmpRows
            tmpStr = tmpStr & tmpRow("Abteilung") & ": " & tmpRow("Infotext") & "     **********     "

        Next
        If Not tmpStr.Length = 0 Then
            hiddenLaufschrift.Value = tmpStr.Remove(tmpStr.Length - 20, 20)
        Else
            hiddenLaufschrift.Value = ""
        End If




    End Sub
    Private Sub timerRefresh_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles timerRefresh.Tick
        getDataSource()
        fillGridViewLeft()
        fillGridViewRight()
        fillLaufSchriftFields()
        lblStand.Font.Bold = True
        lblStand.Font.Size = 18
        'lblStand.Font.Name = "Arial"
        lblStand.Text = "Stand:   " & Now.ToLongTimeString & "   Uhr"
    End Sub
End Class