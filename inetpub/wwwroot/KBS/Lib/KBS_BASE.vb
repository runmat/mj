Option Explicit On
Option Strict On

Public Class KBS_BASE
    Private Shared mIPtoKassen As DataTable
    Private Shared connectedKassen As New Generic.List(Of Kasse)
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
    Private Shared WithEvents mTimer As New Timers.Timer(3600000) '1 stunde=3600000

    Private Shared m_intGroupID As Int32
    Private Shared m_intUserId As Int32 = -1
    Private Shared m_bolTestUser As Boolean = False

#Region "Properties"

    Public ReadOnly Property UserID() As Int32
        Get
            Return m_intUserId
        End Get
    End Property

    Public ReadOnly Property GroupID() As Int32
        Get
            Return m_intGroupID
        End Get
    End Property

    Protected Friend Shared ReadOnly Property SAPConnectionString() As String
        Get
            Dim conStr As String = ""

            If Not m_bolTestUser Then
                'prod
                conStr = "ASHOST=" & ConfigurationManager.AppSettings("SAPAppServerHost") & _
                                        ";CLIENT=" & CShort(ConfigurationManager.AppSettings("SAPClient")) & _
                                        ";SYSNR=" & CShort(ConfigurationManager.AppSettings("SAPSystemNumber")) & _
                                        ";USER=" & ConfigurationManager.AppSettings("SAPUsername") & _
                                        ";PASSWD=" & ConfigurationManager.AppSettings("SAPPassword") & _
                                        ";LANG=DE"
            ElseIf m_bolTestUser Then
                'vm,test,entwicklung
                conStr = "ASHOST=" & ConfigurationManager.AppSettings("TESTSAPAppServerHost") & _
                                    ";CLIENT=" & CShort(ConfigurationManager.AppSettings("TESTSAPClient")) & _
                                    ";SYSNR=" & CShort(ConfigurationManager.AppSettings("TESTSAPSystemNumber")) & _
                                    ";USER=" & ConfigurationManager.AppSettings("TESTSAPUsername") & _
                                    ";PASSWD=" & ConfigurationManager.AppSettings("TESTSAPPassword") & _
                                    ";LANG=DE"
            Else
                Throw New Exception("DynProxy kann SQL-ConnectionString nicht erkennen")
            End If

            Return conStr
        End Get
    End Property

    Public Shared ReadOnly Property IPtoKassen() As DataTable
        Get
            If mIPtoKassen Is Nothing Then
                mTimer.AutoReset = False
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

    Private Shared Sub ClearKassen() Handles mTimer.Elapsed
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
                                    Dim StdUser As String = ""
                                    StdUser = GiveIpStandardUser(CInt(tmpkasseObj.KUNNR), tmpkasseObj.Firma)
                                    GiveGroupIDByUsername(StdUser)
                                    GiveTestuserByUsername(StdUser)
                                    tmpkasseObj.SetApps(GetApplications)
                                    Return tmpkasseObj

                                End If
                            End If
                        End If

                    End If
                Next

                Dim tmpKasse As DataRow = IPtoKassen.Select("IP='" & page.Request.UserHostAddress & "'")(0)
                Dim StandardUser As String = ""
                StandardUser = GiveIpStandardUser(CInt(tmpKasse("Kunnr")), tmpKasse("Firma").ToString)
                GiveGroupIDByUsername(StandardUser)
                GiveTestuserByUsername(StandardUser)
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

    Private Shared Sub GiveGroupIDByUsername(ByVal strUsername As String)

        'Ermittele IpStandardUser des Kunden
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As SqlClient.SqlCommand

        Try
            conn.Open()

            command = New SqlClient.SqlCommand("SELECT dbo.WebUser.UserID, dbo.WebMember.GroupID " & _
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
                m_intGroupID = CType(drUser("GroupID").ToString, Int32)
            Next
        Catch ex As Exception
            EventLog.WriteEntry("KBS_BASE.GiveGroupIDByUsername(" & strUsername & ") SQL-Zugriff-Fehler", ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Shared Sub GiveTestuserByUsername(ByVal strUsername As String)

        'Ermittele IpStandardUser des Kunden
        Dim conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
        Dim command As SqlClient.SqlCommand

        Try
            conn.Open()

            command = New SqlClient.SqlCommand("SELECT dbo.WebUser.TestUser " & _
                                                "FROM dbo.WebUser " & _
                                                "WHERE (dbo.WebUser.Username = '" & strUsername.ToString & "')", _
                                                conn)

            Dim daUser As New SqlClient.SqlDataAdapter(command)
            Dim dtUser As New DataTable()

            daUser.Fill(dtUser)

            Dim drUser As DataRow
            For Each drUser In dtUser.Rows
                m_bolTestUser = CBool(drUser("TestUser").ToString())
            Next
        Catch ex As Exception
            EventLog.WriteEntry("KBS_BASE.GiveTestuserByUsername(" & strUsername & ") SQL-Zugriff-Fehler", ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Shared Function GetApplications() As DataTable
        Dim blnReturn As Boolean
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

            blnReturn = True
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
                Return True
            End If
        End If
    End Function

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


' ************************************************
' $History: KBS_BASE.vb $
' 
' *****************  Version 28  *****************
' User: Dittbernerc  Date: 18.03.11   Time: 13:22
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 27  *****************
' User: Rudolpho     Date: 1.12.10    Time: 11:30
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 26  *****************
' User: Rudolpho     Date: 17.11.10   Time: 10:04
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 25  *****************
' User: Rudolpho     Date: 11.11.10   Time: 10:00
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 24  *****************
' User: Rudolpho     Date: 1.11.10    Time: 16:16
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 23  *****************
' User: Rudolpho     Date: 30.04.10   Time: 12:41
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 22  *****************
' User: Rudolpho     Date: 20.04.10   Time: 18:09
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 21  *****************
' User: Rudolpho     Date: 7.04.10    Time: 14:28
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 20  *****************
' User: Rudolpho     Date: 6.04.10    Time: 17:15
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 19  *****************
' User: Rudolpho     Date: 22.03.10   Time: 17:13
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 22.03.10   Time: 14:30
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 12.03.10   Time: 9:49
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 16.02.10   Time: 17:28
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 12.02.10   Time: 16:29
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 12.02.10   Time: 13:47
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 10.02.10   Time: 17:53
' Updated in $/CKAG2/KBS/Lib
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 3.06.09    Time: 15:43
' Updated in $/CKAG2/KBS/Lib
' nachbesserungen
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 12.05.09   Time: 16:17
' Updated in $/CKAG2/KBS/Lib
' ITA 2808
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 7.05.09    Time: 13:51
' Updated in $/CKAG2/KBS/Lib
' Basisgeschichten
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 7.05.09    Time: 13:08
' Updated in $/CKAG2/KBS/Lib
' ITA 2808 
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 30.04.09   Time: 11:44
' Updated in $/CKAG2/KBS/Lib
' ITA 2838 unfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 24.04.09   Time: 15:48
' Updated in $/CKAG2/KBS/Lib
' ITA 2808
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 24.04.09   Time: 10:35
' Updated in $/CKAG2/KBS/Lib
' from server.transfer back wegen js
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 23.04.09   Time: 17:50
' Updated in $/CKAG2/KBS/Lib
' ITA 2808
' 
'
' ************************************************