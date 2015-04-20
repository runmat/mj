Option Explicit On
Option Strict On

Public Class KBS_BASE
    Private Shared mIPtoKassen As DataTable
    Private Shared connectedKassen As New List(Of Kasse)
    Public Const CHANGE01_GLOBALOBJHANDLING As Boolean = True
    Public Const CHANGE02_GLOBALOBJHANDLING As Boolean = True
    Public Const CHANGE03_GLOBALOBJHANDLING As Boolean = True
    Public Const CHANGE04_GLOBALOBJHANDLING As Boolean = True
    Public Const CHANGE05_GLOBALOBJHANDLING As Boolean = True
    Public Const CHANGE06_GLOBALOBJHANDLING As Boolean = True
    Public Const CHANGE07_GLOBALOBJHANDLING As Boolean = True
    Public Const CHANGE08_GLOBALOBJHANDLING As Boolean = True
    Public Const REPORT01_GLOBALOBJHANDLING As Boolean = True
    Public Const CHANGE12_GLOBALOBJHANDLING As Boolean = True
    Private Shared WithEvents mTimer As Timers.Timer

    Private Shared m_intUserId As Int32 = -1

#Region "Properties"

    Public Shared ReadOnly Property IPtoKassen() As DataTable
        Get
            If mIPtoKassen Is Nothing Then
                If mTimer Is Nothing Then
                    Dim tmpMinutes As Integer = 60
                    If IsNumeric(ConfigurationManager.AppSettings("IPDataRefreshTimerMinutes")) Then
                        tmpMinutes = Int32.Parse(ConfigurationManager.AppSettings("IPDataRefreshTimerMinutes"))
                    End If
                    mTimer = New Timers.Timer(60000 * tmpMinutes)
                    mTimer.AutoReset = False
                    AddHandler mTimer.Elapsed, AddressOf ClearKassen
                End If
                mTimer.Start()
                mIPtoKassen = New DataTable
                fillIpToKassenTable()
                CompareConnectedKassen()
            End If
            Return mIPtoKassen
        End Get
    End Property

#End Region

#Region "Methods"

    Private Shared Sub ClearKassen(ByVal sender As Object, ByVal e As Timers.ElapsedEventArgs)
        mIPtoKassen = Nothing
        IPtoKassen.AcceptChanges()
    End Sub

    Private Shared Sub fillIpToKassenTable()
        Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Try
            cn.Open()
            Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand("SELECT * FROM KBS_EFA", cn)

            Dim adKassen As New SqlClient.SqlDataAdapter(cmd)
            adKassen.Fill(mIPtoKassen)
        Catch ex As Exception
            EventLog.WriteEntry("KBS_BASE.fillIpToKassenTable() SQL-Zugriff-Fehler", ex.Message)
            Throw ex
        Finally
            If cn.State <> ConnectionState.Closed Then
                cn.Close()
            End If
        End Try
    End Sub

    Private Shared Sub CompareConnectedKassen()
        'gleich die Connected Kassen von den Daten in der SQL DB ab, unberechtigte kassen werden entfernt
        'dies dient dazu um bei IP Änderungen diese herauszunehmen
        'JJU20090507
        Try

            Dim reDo As Boolean = False

            For Each tmpkasseObj In connectedKassen
                If Not IPtoKassen.Select("IP='" & tmpkasseObj.IP & "'").Count = 0 Then
                    Dim tmpKassenRow As DataRow = IPtoKassen.Select("IP='" & tmpkasseObj.IP & "'")(0)
                    If Not tmpKassenRow("WERKS").ToString = tmpkasseObj.Werk OrElse Not tmpKassenRow("LGORT").ToString = tmpkasseObj.Lagerort OrElse Not tmpKassenRow("Firma").ToString = tmpkasseObj.Firma Then
                        tmpkasseObj.ChangeBasedValues(tmpKassenRow)
                    End If
                Else
                    'kasse existiert nicht mehr
                    connectedKassen.Remove(tmpkasseObj)
                    reDo = True
                    Exit For
                End If
            Next

            If reDo Then
                CompareConnectedKassen()
            End If

        Catch ex As Exception
            EventLog.WriteEntry("KBS_BASE.CompareConnectedKassen() SQL-Zugriff-Fehler", ex.Message)
        End Try
    End Sub

    Private Shared Function checkIP(ByVal page As Page) As Kasse

        Try

            If IPtoKassen.Select("IP='" & page.Request.UserHostAddress & "'").Count = 1 Then
                Dim tmpkasseObj As Kasse

                For Each tmpkasseObj In connectedKassen
                    If tmpkasseObj.IP = page.Request.UserHostAddress Then
                        Dim kasseDB As DataTable = ProofDBandObject(tmpkasseObj.IP)
                        If kasseDB Is Nothing Then
                            Return Nothing
                        Else

                            If Not IPtoKassen.Select("IP='" & tmpkasseObj.IP & "'").Count = 0 Then
                                Dim tmpKassenRow As DataRow = kasseDB.Select("IP='" & tmpkasseObj.IP & "'")(0)
                                If Not tmpKassenRow("WERKS").ToString = tmpkasseObj.Werk OrElse
                                    Not tmpKassenRow("LGORT").ToString = tmpkasseObj.Lagerort OrElse
                                    Not tmpKassenRow("Firma").ToString = tmpkasseObj.Firma OrElse
                                        Not CBool(tmpKassenRow("Master")) = tmpkasseObj.Master Then
                                    tmpkasseObj.ChangeBasedValues(tmpKassenRow)
                                    Dim tmpKasseold As DataRow = mIPtoKassen.Select("IP='" & tmpkasseObj.IP & "'")(0)
                                    tmpKasseold("WERKS") = tmpKassenRow("WERKS").ToString
                                    tmpKasseold("LGORT") = tmpKassenRow("LGORT").ToString
                                    tmpKasseold("Firma") = tmpKassenRow("Firma").ToString
                                    tmpKasseold("Kunnr") = tmpKassenRow("Kunnr").ToString
                                    tmpKasseold("CustomerID") = tmpKassenRow("CustomerID").ToString
                                    tmpKasseold("Master") = tmpKassenRow("Master")
                                    connectedKassen.Remove(tmpkasseObj)
                                    Exit For
                                Else
                                    Dim StdUser As String = GiveIpStandardUser(CInt(tmpkasseObj.KUNNR), tmpkasseObj.Firma)
                                    GiveUserIDByUsername(StdUser)
                                    tmpkasseObj.SetApps(GetApplications)
                                    Return tmpkasseObj

                                End If
                            End If
                        End If

                    End If
                Next

                Dim tmpKasse As DataRow = IPtoKassen.Select("IP='" & page.Request.UserHostAddress & "'")(0)
                Dim StandardUser As String = GiveIpStandardUser(CInt(tmpKasse("Kunnr")), tmpKasse("Firma").ToString)
                GiveUserIDByUsername(StandardUser)
                Dim tblApps As DataTable
                tblApps = GetApplications()

                tmpkasseObj = New Kasse(page.Request.UserHostAddress, tmpKasse("WERKS").ToString, _
                                        tmpKasse("LGORT").ToString, tmpKasse("Firma").ToString, tmpKasse("Kunnr").ToString, tblApps, _
                                        tmpKasse("CustomerID").ToString, CBool(tmpKasse("Master")))
                connectedKassen.Clear()
                connectedKassen.Add(tmpkasseObj)

                Return tmpkasseObj

            Else : Return Nothing
            End If
        Catch ex As Exception
            EventLog.WriteEntry("KBS_BASE.checkIP(" & page.ToString & ") SQL-Zugriff-Fehler", ex.Message)
        End Try

        Return Nothing
    End Function

    Private Shared Function ProofDBandObject(ByVal strIP As String) As DataTable
        'Änderungen 

        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As SqlClient.SqlCommand
        Dim proofKassen As New DataTable

        Try
            conn.Open()

            command = New SqlClient.SqlCommand("SELECT * FROM KBS_EFA" & _
                    " WHERE" & _
                    " IP = '" & strIP & "'", _
                    conn)


            Dim adKassen As New SqlClient.SqlDataAdapter(command)
            adKassen.Fill(proofKassen)

        Catch ex As Exception
            EventLog.WriteEntry("KBS_BASE.ProofDBandObject(" & strIP & ") SQL-Zugriff-Fehler", ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        Return proofKassen
    End Function

    Private Shared Function GiveIpStandardUser(ByVal intCust As Integer, ByVal strFirma As String) As String
        'Ermittele IpStandardUser des Kunden
        Dim result As Object
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As SqlClient.SqlCommand
        Dim strReturn As String = ""

        Try
            conn.Open()

            command = New SqlClient.SqlCommand("SELECT dbo.WebUser.Username" & _
                        " FROM dbo.Customer INNER JOIN" & _
                        " dbo.WebUser ON dbo.Customer.CustomerID = dbo.WebUser.CustomerID " & _
                        " WHERE (dbo.Customer.KUNNR = '" & intCust.ToString & _
                        "') AND (dbo.WebUser.Reference = '" & strFirma & "')", conn)

            result = command.ExecuteScalar
            If Not result Is Nothing Then
                strReturn = CStr(result)
            End If
        Catch ex As Exception
            EventLog.WriteEntry("KBS_BASE.GiveIpStandardUser(" & intCust.ToString() & "," & strFirma & ") SQL-Zugriff-Fehler", ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        Return strReturn
    End Function

    Private Shared Sub GiveUserIDByUsername(ByVal strUsername As String)

        'Ermittele IpStandardUser des Kunden
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As SqlClient.SqlCommand

        Try
            conn.Open()

            command = New SqlClient.SqlCommand("SELECT dbo.WebUser.UserID " & _
                                                "FROM dbo.WebUser INNER JOIN " & _
                                                "dbo.WebMember ON dbo.WebUser.UserID = dbo.WebMember.UserID " & _
                                                "WHERE     (dbo.WebUser.Username = '" & strUsername.ToString & "')", _
                                                conn)

            Dim daUser As New SqlClient.SqlDataAdapter(command)
            Dim dtUser As New DataTable()

            daUser.Fill(dtUser)

            Dim drUser As DataRow
            For Each drUser In dtUser.Rows
                m_intUserId = CType(drUser("UserID").ToString, Int32)
            Next
        Catch ex As Exception
            EventLog.WriteEntry("KBS_BASE.GiveGroupIDByUsername(" & strUsername & ") SQL-Zugriff-Fehler", ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Shared Function GetApplications() As DataTable
        Dim tmpTable As DataTable

        Try

            Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))

            Dim command As SqlClient.SqlCommand = New SqlClient.SqlCommand("SELECT * " & _
                                                      "FROM vwApplicationWebUser " & _
                                                      "WHERE UserID = @UserID AND AppInMenu = 1 " & _
                                                      "ORDER BY AppRank ASC", conn)
            command.Parameters.AddWithValue("@UserID", m_intUserId)
            Dim adApplication As New SqlClient.SqlDataAdapter(command)
            tmpTable = New DataTable
            adApplication.Fill(tmpTable)

        Catch ex As Exception
            Return Nothing
        End Try
        Return tmpTable

    End Function

    Public Shared Sub FormAuth(ByVal page As Page)
        If page.Session.IsNewSession Then
            Dim tmpKassenObj As Kasse = checkIP(page)
            If tmpKassenObj Is Nothing Then
                page.Response.Redirect("/KBS/Login.aspx")
            Else
                page.Session("mKasse") = tmpKassenObj
                SetLoggingInfos(page, tmpKassenObj)
            End If
        End If
    End Sub

    Public Shared Function login(ByVal page As Page) As Boolean
        If page.Session.IsNewSession Then
            Dim tmpKassenObj As Kasse = checkIP(page)
            If tmpKassenObj Is Nothing Then
                Return False
            Else
                page.Session("mKasse") = tmpKassenObj
                SetLoggingInfos(page, tmpKassenObj)
                Return True
            End If
        End If
    End Function

    Private Shared Sub SetLoggingInfos(ByVal page As Page, ByVal tmpKassenObj As Kasse)
        page.Session("LastAppID") = 0
        page.Session("LastUserID") = 0
        page.Session("LastCustomerID") = tmpKassenObj.CustomerID
        page.Session("LastKunnr") = tmpKassenObj.KUNNR
        page.Session("LastPortalType") = 4
    End Sub

    Public Shared Function CastSapBizTalkErrorMessage(ByVal errorMessage As String) As String
        If errorMessage.Contains("SapErrorMessage") = True Then
            Return Mid(errorMessage, errorMessage.IndexOf("SapErrorMessage") + 17, _
                        errorMessage.Substring((errorMessage.IndexOf("SapErrorMessage") + 16)).IndexOf("."))
        Else
            Return errorMessage
        End If
    End Function

    Public Shared Function MakeDateStandard(ByVal strInput As String) As Date
        REM § Formt String-Input im SAP-Format in Standard-Date um. Gibt "01.01.1900" zurück, wenn Umwandlung nicht möglich ist.
        Dim strTemp As String = Right(strInput, 2) & "." & Mid(strInput, 5, 2) & "." & Left(strInput, 4)
        If IsDate(strTemp) Then
            Return CDate(strTemp)
        Else
            Return CDate("01.01.1900")
        End If
    End Function

    Public Shared Sub killAllDBNullValuesInDataTable(ByRef datentabelle As DataTable)
        '----------------------------------------------------------------------
        'Methode:       killAllDBNullValuesInDataTable
        'Autor:         Julian Jung
        'Beschreibung:  wandelt alle dbnull werte in einer datatable in leere strings ums
        'Erstellt am:   17.7.2008
        '----------------------------------------------------------------------
        For Each tmpRow As DataRow In datentabelle.Rows
            For i As Int32 = 0 To tmpRow.ItemArray.Length - 1
                If tmpRow(i) Is DBNull.Value Then
                    tmpRow(i) = String.Empty
                End If
                If TypeOf (tmpRow(i)) Is DataTable Then
                    killAllDBNullValuesInDataTable(CType(tmpRow(i), DataTable))
                End If
            Next
        Next
        datentabelle.AcceptChanges()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Shared Function GetConnectedKassen(ByVal objIP As String) As Kasse
        GetConnectedKassen = Nothing
        For Each tmpkasseObj In connectedKassen
            If IPtoKassen.Select("IP='" & tmpkasseObj.IP & "'").Count = 1 Then
                GetConnectedKassen = tmpkasseObj
            Else
                Exit For
            End If
        Next
        Return GetConnectedKassen
    End Function

#End Region

End Class
