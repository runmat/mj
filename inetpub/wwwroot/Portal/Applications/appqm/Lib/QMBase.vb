Imports System

Public Class QMBase

    Inherits CKG.Base.Business.DatenimportBase

    Private connection As SqlClient.SqlConnection

    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByVal objApp As CKG.Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub


    Public Sub dbWrite(ByVal InsertErfassung As QMErfassung)
        Dim command As New SqlClient.SqlCommand()
        Dim sqlInsert As String
        'Dim returnID As Long

        sqlInsert = "INSERT INTO QMErfassung (" & _
                            "Erfasser,VKORG,VKBUR,ErfasstAm,ErfasstUm,Referenz,AnzPos,Meldedatum,Kundennr,Kundenname,Ansprechpartner," & _
                            "Kontaktdaten,Kundenreklamation,Prozess,Fehler,Fehlerbeschreibung,FehlerverursacherFirma, " & _
                            "FehlerverursacherName,Klaerungsverantwortlicher,Status) VALUES (" & _
                            "@Erfasser,@VKORG,@VKBUR,@ErfasstAm,@ErfasstUm,@Referenz,@AnzPos,@Meldedatum,@Kundennr,@Kundenname,@Ansprechpartner," & _
                            "@Kontaktdaten,@Kundenreklamation,@Prozess,@Fehler,@Fehlerbeschreibung,@FehlerverursacherFirma," & _
                            "@FehlerverursacherName,@Klaerungsverantwortlicher,@Status)"


        openConnection()


        With command
            .Connection = connection
            .CommandType = CommandType.Text
            .Parameters.Clear()
        End With

        'INSERT
        command.CommandText = sqlInsert

        With command.Parameters
            .AddWithValue("@Erfasser", InsertErfassung.Erfasser)
            .AddWithValue("@VKORG", InsertErfassung.VKORG)
            .AddWithValue("@VKBUR", InsertErfassung.VKBUR)
            .AddWithValue("@ErfasstAm", InsertErfassung.ErfasstAm)
            .AddWithValue("@ErfasstUm", Left(InsertErfassung.ErfasstUm, 8))
            .AddWithValue("@Referenz", InsertErfassung.Referenz)
            .AddWithValue("@AnzPos", InsertErfassung.AnzPos)
            .AddWithValue("@Meldedatum", Left(InsertErfassung.Meldedatum, 2) & "." & Mid(InsertErfassung.Meldedatum, 3, 2) & ".20" & Right(InsertErfassung.Meldedatum, 2))
            .AddWithValue("@Kundennr", InsertErfassung.Kunnr)
            .AddWithValue("@kundenname", InsertErfassung.Kundenname)
            .AddWithValue("@Ansprechpartner", InsertErfassung.Ansprechpartner)
            .AddWithValue("@Kontaktdaten", InsertErfassung.Kontaktdaten)
            .AddWithValue("@Kundenreklamation", InsertErfassung.Kundenreklamation)
            .AddWithValue("@Prozess", InsertErfassung.Prozess)
            .AddWithValue("@Fehler", InsertErfassung.Fehler)
            .AddWithValue("@Fehlerbeschreibung", InsertErfassung.Fehlerbeschreibung)
            .AddWithValue("@FehlerverursacherFirma", InsertErfassung.FehlerverursacherFirma)
            .AddWithValue("@FehlerverursacherName", InsertErfassung.FehlerverursacherName)
            .AddWithValue("@Klaerungsverantwortlicher", InsertErfassung.KlaerungsverantwortlicherName)
            .AddWithValue("@Status", InsertErfassung.Status)

        End With

        command.ExecuteNonQuery()

        closeConnection()

    End Sub


    Public Function getTablesForDropdowns() As DataSet

        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim dtProzess As DataTable
        Dim dtFehler As DataTable
        Dim dtStatus As DataTable

        Dim str As String
        Dim dsDropdownTables As New DataSet()

        Try

            openConnection()
            command.Connection = connection

            'Prozesse
            str = "SELECT * FROM QMProzess order by Prozesstext"
            command.CommandText = str
            adapter.SelectCommand = command
            dtProzess = dsDropdownTables.Tables.Add("dtProzess")
            adapter.Fill(dsDropdownTables.Tables("dtProzess"))


            'Fehler
            str = "SELECT * FROM QMFehler order by Fehlertext"
            command.CommandText = str
            adapter.SelectCommand = command
            dtFehler = dsDropdownTables.Tables.Add("dtFehler")
            adapter.Fill(dsDropdownTables.Tables("dtFehler"))

            'Status
            str = "SELECT * FROM QMStatus order by Statustext"
            command.CommandText = str
            adapter.SelectCommand = command
            dtStatus = dsDropdownTables.Tables.Add("dtStatus")
            adapter.Fill(dsDropdownTables.Tables("dtStatus"))


        Catch ex As Exception
            Throw ex
        Finally
            closeConnection()
        End Try
        Return dsDropdownTables
    End Function

    Public Function ErrDescPflicht(ByVal Fehlernr As String) As Boolean
        Dim command As New SqlClient.SqlCommand()
        Dim adapter As New SqlClient.SqlDataAdapter()
        Dim dtFehler As New DataTable()

        openConnection()

        command.Connection = connection

        command.CommandText = "Select Fehlernr From qmfehler where Fehlernr = '" & Fehlernr & "' and FehlerbeschreibungPflicht = 1"

        adapter.SelectCommand = command

        adapter.Fill(dtFehler)

        If IsNothing(dtFehler) = False Then
            If dtFehler.Rows.Count = 0 Then
                Return True
            Else
                Return False
            End If
        Else
            Return True
        End If


    End Function



    Public Overloads Sub FILL_QM_Auswertung(ByVal strAppID As String, ByVal strSessionID As String, _
                                            ByVal datMeldedatumVon As String, ByVal datMeldedatumBis As String, _
                                            ByVal Kunnr As String, ByVal VKORG As String, ByVal VKBUR As String, ByVal Status As String)

        m_strClassAndMethod = "QMBase.FILL_QM_Auswertung"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True


            Try
                Dim tblTemp2 As New DataTable()

                Dim command As New SqlClient.SqlCommand()
                Dim adapter As New SqlClient.SqlDataAdapter()

                Dim str As String
                'Dim dsDropdownTables As New DataSet()


                openConnection()
                command.Connection = connection

                'Query
                Dim strWhere As String = String.Empty

                If datMeldedatumVon <> String.Empty And datMeldedatumBis <> String.Empty Then

                    strWhere = " where Meldedatum BETWEEN '" & datMeldedatumVon & "' and '" & datMeldedatumBis & "' "

                    If Kunnr <> String.Empty Then
                        strWhere = strWhere & "and Kunnr = '" & Kunnr & "' "
                    End If


                ElseIf datMeldedatumVon <> String.Empty OrElse datMeldedatumBis <> String.Empty Then
                    If datMeldedatumVon <> String.Empty Then
                        strWhere = " where Meldedatum >= '" & datMeldedatumVon & "' "
                    Else
                        strWhere = " where Meldedatum <= '" & datMeldedatumBis & "' "
                    End If

                    If Kunnr <> String.Empty Then
                        strWhere = strWhere & "and Kundennr = '" & Kunnr & "' "
                    End If
                Else
                    If Kunnr <> String.Empty Then
                        strWhere = strWhere & " Where Kundennr = '" & Kunnr & "' "
                    End If

                End If

                If VKORG <> String.Empty Then
                    If strWhere <> String.Empty Then
                        strWhere = strWhere & " and VKORG = '" & VKORG & "' "
                    Else
                        strWhere = " Where VKORG = '" & VKORG & "' "
                    End If
                End If

                If VKBUR <> String.Empty Then
                    If strWhere <> String.Empty Then
                        strWhere = strWhere & " and VKBUR = '" & VKBUR & "' "
                    Else
                        strWhere = " Where VKBUR = '" & VKBUR & "' "
                    End If
                End If


                If Status <> String.Empty Then
                    If strWhere <> String.Empty Then
                        strWhere = strWhere & " and Status = '" & Status & "' "
                    Else
                        strWhere = " Where Status = '" & Status & "' "
                    End If
                End If

                str = "SELECT * FROM QMErfassung " & strWhere & " order by Meldedatum, Kundennr, ErfasstAm"

                command.CommandText = str
                adapter.SelectCommand = command
                adapter.Fill(tblTemp2)



                CreateOutPut(tblTemp2, strAppID)

            Catch ex As Exception

                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

            Finally
                closeConnection()
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Function FILL_QM_AuswertungSingle(ByVal ID As String, ByVal strAppID As String, ByVal strSessionID As String) As DataTable

        m_strClassAndMethod = "QMBase.FILL_QM_AuswertungSingle"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        Dim tblTemp2 As New DataTable

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Try

                Dim command As New SqlClient.SqlCommand()
                Dim adapter As New SqlClient.SqlDataAdapter()

                Dim str As String = "SELECT * FROM QMErfassung where id = " & CInt(ID)


                openConnection()
                command.Connection = connection

                command.CommandText = str
                adapter.SelectCommand = command
                adapter.Fill(tblTemp2)


            Catch ex As Exception

                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

            Finally
                closeConnection()
                m_blnGestartet = False
            End Try

        End If
        Return tblTemp2
    End Function
    
    Public Sub Updatesql(ByVal ID As String, ByVal InsertErfassung As QMErfassung)
        Dim command As New SqlClient.SqlCommand()
        Dim sqlUpdate As String

        'sqlUpdate = "UPDATE Zulassung SET zulassung.username = webuser.username FROM webuser INNER JOIN zulassung ON zulassung.id_user = webuser.userid AND zulassung.username IS NULL"


        sqlUpdate = "Update QMErfassung " & _
                            "set VKORG=@VKORG,VKBUR=@VKBUR,Referenz=@Referenz,AnzPos=@AnzPos,Meldedatum=@Meldedatum,Kundennr=@Kundennr,Kundenname=@Kundenname,Ansprechpartner=@Ansprechpartner," & _
                            "Kontaktdaten=@Kontaktdaten,Kundenreklamation=@Kundenreklamation,Prozess=@Prozess,Fehler=@Fehler,Fehlerbeschreibung=@Fehlerbeschreibung,FehlerverursacherFirma=@FehlerverursacherFirma, " & _
                            "FehlerverursacherName=@FehlerverursacherName,Klaerungsverantwortlicher=@Klaerungsverantwortlicher,Status=@Status" & _
                            " where id = " & CInt(ID)


        openConnection()


        With command
            .Connection = connection
            .CommandType = CommandType.Text
            .Parameters.Clear()
        End With

        command.CommandText = sqlUpdate


        With command.Parameters
            '.Add("@Erfasser", InsertErfassung.Erfasser)
            .AddWithValue("@VKORG", InsertErfassung.VKORG)
            .AddWithValue("@VKBUR", InsertErfassung.VKBUR)
            '.Add("@ErfasstAm", InsertErfassung.ErfasstAm)
            '.Add("@ErfasstUm", Left(InsertErfassung.ErfasstUm, 8))
            .AddWithValue("@Referenz", InsertErfassung.Referenz)
            .AddWithValue("@AnzPos", InsertErfassung.AnzPos)
            .AddWithValue("@Meldedatum", Left(InsertErfassung.Meldedatum, 2) & "." & Mid(InsertErfassung.Meldedatum, 3, 2) & ".20" & Right(InsertErfassung.Meldedatum, 2))
            .AddWithValue("@Kundennr", InsertErfassung.Kunnr)
            .AddWithValue("@kundenname", InsertErfassung.Kundenname)
            .AddWithValue("@Ansprechpartner", InsertErfassung.Ansprechpartner)
            .AddWithValue("@Kontaktdaten", InsertErfassung.Kontaktdaten)
            .AddWithValue("@Kundenreklamation", InsertErfassung.Kundenreklamation)
            .AddWithValue("@Prozess", InsertErfassung.Prozess)
            .AddWithValue("@Fehler", InsertErfassung.Fehler)
            .AddWithValue("@Fehlerbeschreibung", InsertErfassung.Fehlerbeschreibung)
            .AddWithValue("@FehlerverursacherFirma", InsertErfassung.FehlerverursacherFirma)
            .AddWithValue("@FehlerverursacherName", InsertErfassung.FehlerverursacherName)
            .AddWithValue("@Klaerungsverantwortlicher", InsertErfassung.KlaerungsverantwortlicherName)
            .AddWithValue("@Status", InsertErfassung.Status)

        End With

        command.ExecuteNonQuery()

        closeConnection()


    End Sub

    Private Sub openConnection()
        connection = New SqlClient.SqlConnection()
        connection.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
        connection.Open()
    End Sub

    Private Sub closeConnection()
        connection.Close()
        connection.Dispose()
    End Sub

End Class
