
Namespace Business
    Public Class ZulassungsVorbelegung
        REM § Enthält Daten einer einzelnen ZulassungsVorbelegung.
        REM § (Zuordnung von Fahrgestellnummer zu Versicherer, Halter etc)

#Region " Membervariables "
        Private m_strConnectionstring As String
        Private m_intZVID As Integer
        Private m_strVonFZN1_3 As String
        Private m_strVonFZN4_17 As String
        Private m_strBisFZN1_3 As String
        Private m_strBisFZN4_17 As String
        Private m_strHalter_SAPNr As String
        Private m_strVersicherer_SAPNr As String
        Private m_datAbDatum As Date
        Private m_datBisDatum As Date
        Private m_strModell As String
        Private m_strFahrgestellNr As String
        Private m_datZulassungsDatum As Date
        Private m_blnKennzeichen2zeilig As Boolean
        Private m_blnLimo As Boolean
        Private m_strKBANR As String
        Private m_strHersteller As String
        Private m_cn As SqlClient.SqlConnection
        Private m_cmd As SqlClient.SqlCommand
        Private m_da As SqlClient.SqlDataAdapter
        Private m_datatable As DataTable
        Private m_CustomerID As Integer '§§§ JVE 19.10.2006
#End Region

#Region " Constructor "
        Public Sub New(ByVal intZVID As Integer)
            m_intZVID = intZVID
        End Sub

        Public Sub New(ByVal intZVID As Integer, _
                       ByVal strVonFZN1_3 As String, _
                       ByVal strVonFZN4_17 As String, _
                       ByVal strBisFZN1_3 As String, _
                       ByVal strBisFZN4_17 As String, _
                       ByVal strHalter_SAPNr As String, _
                       ByVal strVersicherer_SAPNr As String, _
                       ByVal datAbDatum As Date, _
                       ByVal datBisDatum As Date, _
                       ByVal strModell As String, _
                       ByVal blnKennzeichen2zeilig As Boolean, _
                       ByVal blnLimo As Boolean, _
                       ByVal strHersteller As String, ByVal intCustomerID As Integer) '§§§ JVE 19.10.2006: CustomerID eingefügt.
            m_intZVID = intZVID
            m_strVonFZN1_3 = Left(strVonFZN1_3 & "000", 3)
            m_strVonFZN4_17 = Left(strVonFZN4_17 & "00000000000000", 14)
            m_strBisFZN1_3 = Left(strBisFZN1_3 & "ZZZ", 3)
            m_strBisFZN4_17 = Left(strBisFZN4_17 & "ZZZZZZZZZZZZZZ", 14)
            m_strHalter_SAPNr = strHalter_SAPNr
            m_strVersicherer_SAPNr = strVersicherer_SAPNr
            m_datAbDatum = datAbDatum
            m_datBisDatum = datBisDatum
            m_strModell = strModell
            m_blnKennzeichen2zeilig = blnKennzeichen2zeilig
            m_blnLimo = blnLimo
            m_strHersteller = strHersteller
            m_CustomerID = intCustomerID   '§§§ JVE 19.10.2006
        End Sub
        Public Sub New(ByVal intZVID As Integer, ByVal _user As Kernel.Security.User)
            Me.New(intZVID, _user.App.Connectionstring)
        End Sub
        Public Sub New(ByVal intZVID As Integer, ByVal strConnectionString As String)
            Me.New(intZVID, New SqlClient.SqlConnection(strConnectionString))
        End Sub
        Public Sub New(ByVal intZVID As Integer, ByVal cn As SqlClient.SqlConnection)
            m_intZVID = intZVID
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            GetData(cn)
        End Sub
        Public Sub New(ByVal strFahrgestellNr As String, ByVal datZulassungsDatum As Date, ByVal strCn As String)
            Me.new(strFahrgestellNr, datZulassungsDatum, New SqlClient.SqlConnection(strCn))
            m_strConnectionstring = strCn
        End Sub
        Public Sub New(ByVal strFahrgestellNr As String, ByVal datZulassungsDatum As Date, ByVal cn As SqlClient.SqlConnection)
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            m_strFahrgestellNr = strFahrgestellNr
            m_datZulassungsDatum = datZulassungsDatum
            FindRecord(cn)
        End Sub
        Public Sub New(ByVal datZulassungsdatum As Date, ByVal strCn As String)
            m_cn = New SqlClient.SqlConnection(strCn)
            m_datZulassungsDatum = datZulassungsdatum
        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property Kennzeichen2zeilig() As Boolean
            Get
                Return m_blnKennzeichen2zeilig
            End Get
        End Property

        Public ReadOnly Property Limo() As Boolean
            Get
                Return m_blnLimo
            End Get
        End Property

        Public ReadOnly Property ZVID() As Integer
            Get
                Return m_intZVID
            End Get
        End Property

        Public ReadOnly Property VonFZN1_3() As String
            Get
                Return m_strVonFZN1_3
            End Get
        End Property

        Public ReadOnly Property VonFZN4_17() As String
            Get
                Return m_strVonFZN4_17
            End Get
        End Property

        Public ReadOnly Property BisFZN1_3() As String
            Get
                Return m_strBisFZN1_3
            End Get
        End Property

        Public ReadOnly Property BisFZN4_17() As String
            Get
                Return m_strBisFZN4_17
            End Get
        End Property

        Public ReadOnly Property Halter_SAPNr() As String
            Get
                If m_strHalter_SAPNr Is Nothing Then
                    Return ""
                Else
                    Return m_strHalter_SAPNr
                End If
            End Get
        End Property

        Public ReadOnly Property Versicherer_SAPNr() As String
            Get
                If m_strVersicherer_SAPNr Is Nothing Then
                    Return ""
                Else
                    Return m_strVersicherer_SAPNr
                End If
            End Get
        End Property

        Public ReadOnly Property AbDatum() As Date
            Get
                Return m_datAbDatum
            End Get
        End Property

        Public ReadOnly Property BisDatum() As Date
            Get
                Return m_datBisDatum
            End Get
        End Property

        Public ReadOnly Property Modell() As String
            Get
                If m_strModell Is Nothing Then
                    Return "unbekannt"
                Else
                    Return m_strModell
                End If
            End Get
        End Property
        Public ReadOnly Property KBANR() As String
            Get
                If m_strKBANR Is Nothing Then
                    Return "0000000"
                Else
                    Return Right("0000000" & m_strKBANR, 7)
                End If
            End Get
        End Property

        Public ReadOnly Property Hersteller() As String
            Get
                Return m_strHersteller
            End Get
        End Property
#End Region

#Region " Functions "
        Public Sub Find(ByVal strFahrgestellNr As String)
            m_strFahrgestellNr = strFahrgestellNr
            If m_cn.State = ConnectionState.Closed Then
                m_cn.Open()
                Dim strSQL As String = "SELECT [VonFZN1_3], [BisFZN1_3], [VonFZN4_17], [BisFZN4_17], Kennzeichen2zeilig, Limo " & _
                                       "FROM ZulassungsVorbelegungen " & _
                                       "WHERE (@ZLDat BETWEEN AbDatum AND BisDatum) AND (Limo=@Limo)"
                m_cmd = New SqlClient.SqlCommand(strSQL, m_cn)
                With m_cmd
                    .Parameters.Add("@ZLDat", SqlDbType.DateTime)
                    .Parameters("@ZLDat").Value = m_datZulassungsDatum
                    .Parameters.Add("@Limo", SqlDbType.Bit)
                    .Parameters("@Limo").Value = True
                End With
                m_da = New SqlClient.SqlDataAdapter(m_cmd)
                m_datatable = New DataTable()
                m_da.Fill(m_datatable)
            End If

            m_blnKennzeichen2zeilig = False
            m_blnLimo = False
            If (Not m_datatable Is Nothing) AndAlso (m_datatable.Rows.Count > 0) Then
                Dim str1_3 As String = Mid(m_strFahrgestellNr, 1, 3)
                Dim str4_17 As String = Mid(m_strFahrgestellNr, 4, 14)

                Dim tmpRows As DataRow() = m_datatable.Select("(NOT '" & str1_3 & "' < VonFZN1_3) AND (NOT '" & str1_3 & "' > BisFZN1_3) AND (NOT '" & str4_17 & "' < VonFZN4_17) AND (NOT '" & str4_17 & "' > BisFZN4_17)")
                If tmpRows.Length > 0 Then
                    m_blnKennzeichen2zeilig = CType(tmpRows(0)("Kennzeichen2zeilig"), Boolean)
                    m_blnLimo = CType(tmpRows(0)("Limo"), Boolean)
                End If
            End If
        End Sub

        'Public Sub Find(ByVal strFahrgestellNr As String)
        '    m_strFahrgestellNr = strFahrgestellNr
        '    If m_cn.State = ConnectionState.Closed Then
        '        m_cn.Open()

        '        Dim strSQL As String = "SELECT Limo " & _
        '                               "FROM ZulassungsVorbelegungen " & _
        '                               "WHERE (@FZN1_3 BETWEEN VonFZN1_3 AND BisFZN1_3) " & _
        '                                 "AND (@FZN4_17 BETWEEN VonFZN4_17 AND BisFZN4_17) " & _
        '                                 "AND (@ZLDat BETWEEN AbDatum AND BisDatum) AND (Limo=@Limo)"
        '        m_cmd = New SqlClient.SqlCommand(strSQL, m_cn)

        '        With m_cmd
        '            .Parameters.Add("@ZLDat", SqlDbType.DateTime)
        '            .Parameters.Add("@Limo", SqlDbType.Bit)
        '            .Parameters.Add("@FZN1_3", SqlDbType.NVarChar, 3)
        '            .Parameters.Add("@FZN4_17", SqlDbType.NVarChar, 14)
        '        End With
        '    End If

        '    m_blnKennzeichen2zeilig = False
        '    m_blnLimo = False

        '    Dim str1_3 As String = Mid(m_strFahrgestellNr, 1, 3)
        '    Dim str4_17 As String = Mid(m_strFahrgestellNr, 4, 14)
        '    With m_cmd
        '        .Parameters("@ZLDat").Value = m_datZulassungsDatum
        '        .Parameters("@Limo").Value = True
        '        .Parameters("@FZN1_3").Value = str1_3
        '        .Parameters("@FZN4_17").Value = str4_17
        '    End With

        '    If m_cmd.ExecuteScalar Then
        '        m_blnLimo = True
        '    End If
        'End Sub

        Public Sub CloseDB()
            If Not m_cn.State = ConnectionState.Closed Then
                m_cn.Close()
            End If
        End Sub

        Private Sub FindRecord(ByVal cn As SqlClient.SqlConnection)
            Try

                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                Dim str1_3 As String = Mid(m_strFahrgestellNr, 1, 3)
                Dim str4_17 As String = Mid(m_strFahrgestellNr, 4, 14)
                Dim strSQL As String = "SELECT [Halter_SAP-Nr], [Versicherer_SAP-Nr], Modell, Kennzeichen2zeilig, Limo " & _
                                       "FROM ZulassungsVorbelegungen " & _
                                       "WHERE (@FZN1_3 BETWEEN VonFZN1_3 AND BisFZN1_3) " & _
                                         "AND (@FZN4_17 BETWEEN VonFZN4_17 AND BisFZN4_17) " & _
                                         "AND (@ZLDat BETWEEN AbDatum AND BisDatum)"
                Dim cmd As New SqlClient.SqlCommand(strSQL, cn)
                With cmd
                    'Parameter mit Groesse definieren um Fehler zu vermeiden.
                    .Parameters.Add("@FZN1_3", SqlDbType.NVarChar, 3)
                    .Parameters.Add("@FZN4_17", SqlDbType.NVarChar, 14)
                    .Parameters.Add("@ZLDat", SqlDbType.DateTime)
                    .Parameters("@FZN1_3").Value = str1_3
                    .Parameters("@FZN4_17").Value = str4_17
                    .Parameters("@ZLDat").Value = m_datZulassungsDatum
                End With
                Dim dr As SqlClient.SqlDataReader = cmd.ExecuteReader
                While dr.Read
                    m_strHalter_SAPNr = dr("Halter_SAP-Nr").ToString
                    m_strVersicherer_SAPNr = dr("Versicherer_SAP-Nr").ToString
                    m_strModell = dr("Modell").ToString
                    m_blnKennzeichen2zeilig = CType(dr("Kennzeichen2zeilig"), Boolean)
                    m_blnLimo = CType(dr("Limo"), Boolean)
                End While
                dr.Close()

                strSQL = "SELECT KBANR " & _
                                       "FROM Halter " & _
                                       "WHERE [SAP-Nr] = '" & m_strHalter_SAPNr & "'"
                cmd = New SqlClient.SqlCommand(strSQL, cn)
                dr = cmd.ExecuteReader
                While dr.Read
                    m_strKBANR = dr("KBANR").ToString
                End While
                dr.Close()
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try

        End Sub

        Private Sub GetData(ByVal cn As SqlClient.SqlConnection)
            Dim cmdGet As New SqlClient.SqlCommand("SELECT * FROM ZulassungsVorbelegungen WHERE ZVID=@ZVID", cn)
            Dim dr As SqlClient.SqlDataReader = cmdGet.ExecuteReader()
            Try
                cmdGet.Parameters.AddWithValue("@ZVID", ZVID)

                While dr.Read
                    m_intZVID = CInt(dr("ZVID"))
                    m_strVonFZN1_3 = CStr(dr("VonFZN1_3"))
                    m_strVonFZN4_17 = CStr(dr("VonFZN4_17"))
                    m_strBisFZN1_3 = CStr(dr("BisFZN1_3"))
                    m_strBisFZN4_17 = CStr(dr("BisFZN4_17"))
                    m_strHalter_SAPNr = CStr(dr("Halter_SAP-Nr"))
                    m_strVersicherer_SAPNr = CStr(dr("Versicherer_SAP-Nr"))
                    m_datAbDatum = CDate(dr("AbDatum"))
                    m_datBisDatum = CDate(dr("BisDatum"))
                    m_strModell = dr("Modell").ToString
                    m_blnKennzeichen2zeilig = CType(dr("Kennzeichen2zeilig"), Boolean)
                    m_blnLimo = CType(dr("Limo"), Boolean)
                    m_strHersteller = CStr(dr("Hersteller"))
                End While

            Catch ex As Exception
                dr.Close()
                cn.Close()
                Throw ex
            Finally
                dr.Close()
                cn.Close()
            End Try
        End Sub

        Public Sub Delete(ByVal strConnectionString As String)
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Delete(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der Vorbelegung!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub Delete(ByVal cn As SqlClient.SqlConnection)
            Try
                Dim strDeleteSQL As String = "DELETE " & _
                                              "FROM ZulassungsVorbelegungen " & _
                                              "WHERE ZVID=@ZVID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn
                cmd.Parameters.AddWithValue("@ZVID", m_intZVID)

                cmd.CommandText = strDeleteSQL
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("Fehler beim Löschen der Vorbelegung!", ex)
            End Try
        End Sub

        Public Sub Save(ByVal strConnectionString As String)
            m_strConnectionstring = strConnectionString
            Dim cn As New SqlClient.SqlConnection(m_strConnectionstring)
            Try
                cn.Open()
                Save(cn)
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Vorbelegung!", ex)
            Finally
                If cn.State <> ConnectionState.Closed Then
                    cn.Close()
                End If
            End Try
        End Sub
        Public Sub Save(ByVal cn As SqlClient.SqlConnection)
            Try     '§§§ JVE 19.10.2006: CustomerID eingefügt!
                Dim strInsert As String = "INSERT INTO ZulassungsVorbelegungen(VonFZN1_3, " & _
                                                      "VonFZN4_17, " & _
                                                      "BisFZN1_3, " & _
                                                      "BisFZN4_17, " & _
                                                      "[Halter_SAP-Nr], " & _
                                                      "[Versicherer_SAP-Nr], " & _
                                                      "AbDatum, " & _
                                                      "BisDatum, " & _
                                                      "Modell, " & _
                                                      "Kennzeichen2zeilig, " & _
                                                      "Limo, " & _
                                                      "Hersteller,CustomerID) " & _
                                          "VALUES(@VonFZN1_3, " & _
                                                 "@VonFZN4_17, " & _
                                                 "@BisFZN1_3, " & _
                                                 "@BisFZN4_17, " & _
                                                 "@Halter_SAPNr, " & _
                                                 "@Versicherer_SAPNr, " & _
                                                 "@AbDatum, " & _
                                                 "@BisDatum, " & _
                                                 "@Modell, " & _
                                                 "@Kennzeichen2zeilig, " & _
                                                 "@Limo, " & _
                                                 "@Hersteller,@CustomerID); " & _
                                          "SELECT SCOPE_IDENTITY()"

                Dim strUpdate As String = "UPDATE ZulassungsVorbelegungen " & _
                                          "SET VonFZN1_3=@VonFZN1_3, " & _
                                              "VonFZN4_17=@VonFZN4_17, " & _
                                              "BisFZN1_3=@BisFZN1_3, " & _
                                              "BisFZN4_17=@BisFZN4_17, " & _
                                              "[Halter_SAP-Nr]=@Halter_SAPNr, " & _
                                              "[Versicherer_SAP-Nr]=@Versicherer_SAPNr, " & _
                                              "AbDatum=@AbDatum, " & _
                                              "BisDatum=@BisDatum, " & _
                                              "Modell=@Modell, " & _
                                              "Kennzeichen2zeilig=@Kennzeichen2zeilig, " & _
                                              "Limo=@Limo, " & _
                                              "Hersteller=@Hersteller, CustomerID=@CustomerID " & _
                                           "WHERE ZVID=@ZVID"

                Dim cmd As New SqlClient.SqlCommand()
                cmd.Connection = cn

                'Speichern
                If m_intZVID = -1 Then
                    cmd.CommandText = strInsert
                Else
                    cmd.CommandText = strUpdate
                    cmd.Parameters.AddWithValue("@ZVID", m_intZVID)
                End If
                With cmd.Parameters
                    .AddWithValue("@VonFZN1_3", m_strVonFZN1_3.ToUpper)
                    .AddWithValue("@VonFZN4_17", m_strVonFZN4_17.ToUpper)
                    .AddWithValue("@BisFZN1_3", m_strBisFZN1_3.ToUpper)
                    .AddWithValue("@BisFZN4_17", m_strBisFZN4_17.ToUpper)
                    .AddWithValue("@Halter_SAPNr", m_strHalter_SAPNr)
                    .AddWithValue("@Versicherer_SAPNr", m_strVersicherer_SAPNr)
                    .AddWithValue("@AbDatum", m_datAbDatum)
                    .AddWithValue("@BisDatum", m_datBisDatum)
                    .AddWithValue("@Modell", m_strModell)
                    .AddWithValue("@Kennzeichen2zeilig", m_blnKennzeichen2zeilig)
                    .AddWithValue("@Limo", m_blnLimo)
                    .AddWithValue("@Hersteller", m_strHersteller)
                    .AddWithValue("@CustomerID", m_CustomerID)   '§§§ JVE 19.10.2006: CustomerID
                End With

                If m_intZVID = -1 Then
                    m_intZVID = CInt(cmd.ExecuteScalar())
                Else
                    cmd.ExecuteNonQuery()
                End If
            Catch ex As SqlClient.SqlException
                If ex.Number = 2627 Then
                    Throw New Exception("Diese Vorbelegung existiert schon!")
                Else
                    Throw New Exception("Fehler beim Speichern der Vorbelegung!", ex)
                End If
            Catch ex As Exception
                Throw New Exception("Fehler beim Speichern der Vorbelegung!", ex)
            End Try
        End Sub

        Public Function CheckVMBoolean(ByVal strOrdernummer As String, ByVal strZiffer2 As String, Optional ByVal strCheckValue As String = "217", Optional ByVal strCheckValue2 As String = "0710,0999", Optional ByVal strCheckValue3 As String = "14226,15226,16226,17226,54226,55226,74226,75226") As Boolean
            Dim blnReturn As Boolean = False

            If strZiffer2.Length < 4 Then
                blnReturn = False
            Else
                Dim strCheckValues2() As String = Split(strCheckValue2, ",")
                Dim str As String
                For Each str In strCheckValues2
                    If strZiffer2 = str Then
                        blnReturn = True
                        Exit For
                    End If
                Next
            End If

            If blnReturn Then
                If strOrdernummer.Length < 5 Then
                    blnReturn = False
                Else
                    Dim strCheckValues() As String = Split(strCheckValue, ",")
                    Dim str As String
                    Dim strOrderNrPart As String = strOrdernummer.Substring(2, 3)
                    For Each str In strCheckValues
                        If str = strOrderNrPart Then
                            blnReturn = False
                            Exit For
                        End If
                    Next
                    If blnReturn Then
                        strCheckValues = Split(strCheckValue3, ",")
                        strOrderNrPart = strOrdernummer.Substring(0, 5)
                        For Each str In strCheckValues
                            If str = strOrderNrPart Then
                                blnReturn = False
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If

            If blnReturn Then
                Dim cn As SqlClient.SqlConnection
                cn = New SqlClient.SqlConnection(m_strConnectionstring)
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If
                m_strFahrgestellNr = "_VMVERMITT_FHRZG"
                FindRecord(cn)
            End If
            Return blnReturn
        End Function
#End Region

    End Class
End Namespace

' ************************************************
' $History: ZulassungsVorbelegung.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Business
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 6.12.07    Time: 12:47
' Updated in $/CKG/Base/Base/Business
' ITA:1440
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Base/Base/Business
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Business
' 
' ************************************************