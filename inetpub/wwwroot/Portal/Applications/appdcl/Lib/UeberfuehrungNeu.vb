Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common
Imports System.IO
Imports CKG.Base.Kernel

Public Class UeberfuehrungNeu

    REM § Lese-/Schreibfunktion, Kunde: CKAG, 
    REM § Show - BAPI: Z_V_Ueberf_Auftr_Fahrer,
    REM § Change - BAPI: .

    Inherits AppDCL.Ueberfuehrung

#Region "Declarations"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String,
                   ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Shadows Sub getAuftraege(ByVal strAppID As String, ByVal strSessionID As String, ByVal fahrer As String)
        '§§§ JVE 16.01.2006 Paramater "fahrer" eingeführt, da es möglich sein soll, auch "fremde" Aufträge zu sehen...
        m_strClassAndMethod = "Ueberfuehrung.getAuftraege()"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intIDSAP = -1

            Try
                ClearError()

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_UEBERF_AUFTR_FAHRER", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_FAHRER", Right("0000000000" & fahrer, 10))

                'myProxy.callBapi()

                S.AP.InitExecute("Z_V_UEBERF_AUFTR_FAHRER", "I_FAHRER", Right("0000000000" & fahrer, 10))

                m_Fahrernummer = S.AP.GetExportParameter("E_FAHRER")
                m_Fahrername = S.AP.GetExportParameter("NAME1")
                m_Ort = S.AP.GetExportParameter("CITY1")
                m_Plz = S.AP.GetExportParameter("PSTCD1")
                m_StrNr = S.AP.GetExportParameter("STREET")
                m_tblResult = S.AP.GetExportTable("T_AUFTRAEGE")

                WriteLogEntry(True, "KUNNR=" & m_objUser.Customer.CustomerId, m_tblResult)
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        RaiseError("-2201", "Keine Daten gefunden.")
                    Case Else
                        RaiseError("-9999", ex.Message)
                End Select
                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c), m_tblResult)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Diese Funktion verwendet den Biztalk-Adapter über die DynSapProxy-Klasse, um Auftragsdaten über eine <paramref name="referenz"></paramref> zu ermitteln.
    ''' </summary>
    ''' <param name="referenz">20-stelliger Referenzwert</param>
    ''' <param name="auftrag">10-stellige Auftragsnummer</param>
    ''' <param name="page"><c>System.Web.UI.Page</c> Objekt das beim Aufruf dem DynSapProxy mitgegeben wird.</param>
    ''' <remarks></remarks>
    Public Shadows Sub getAuftragsDatenByReferenz(ByVal referenz As String, ByVal auftrag As String)
        m_strClassAndMethod = "Ueberfuehrung.getAuftagsDaten()"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim intID As Int32 = -1
            m_intStatus = 0

            Try
                ClearError()

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_UEBERF_AUFTR_REFERENZ", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("AUFNR", Right("0000000000" & auftrag, 10))
                'myProxy.setImportParameter("REFNR", Right("00000000000000000000" & referenz, 20))

                'myProxy.callBapi()

                S.AP.InitExecute("Z_V_UEBERF_AUFTR_REFERENZ", "AUFNR,REFNR", Right("0000000000" & auftrag, 10), Right("00000000000000000000" & referenz, 20))

                m_tblResult = S.AP.GetExportTable("T_AUFTRAEGE")
                'HelpProcedures.killAllDBNullValuesInDataTable(m_tblResult)
                mEmails = S.AP.GetExportTable("T_SMTP")
                'HelpProcedures.killAllDBNullValuesInDataTable(mEmails)

                For Each row As DataRow In m_tblResult.Rows
                    row("AUFNR") = Right("0000000000" & row("AUFNR").ToString, 10)
                Next

                WriteLogEntry(True, "", m_tblResult)
            Catch ex As Exception
                m_intStatus = -9999
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case "NO_PROTOKOLL"
                        m_strMessage = "Zum Auftrag wurde bereits ein Protokoll archiviert. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                'End If
            Finally
                'If intID > -1 Then
                '    m_objLogApp.WriteStandardDataAccessSAP(intID)
                'End If

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Shadows Sub getAuftragsDaten(ByVal table As DataTable)
        m_strClassAndMethod = "Ueberfuehrung.getAuftagsDaten()"
        m_intStatus = 0
        Try
            'Dim myProxy As DynSapProxyObj

            'myProxy = DynSapProxy.getProxy("Z_V_UEBERF_AUFTR_LISTE", m_objApp, m_objUser, page)

            Dim AuftragTable As DataTable = S.AP.GetImportTableWithInit("Z_V_UEBERF_AUFTR_LISTE.T_AUFTRAEGE") 'myProxy.getImportTable("T_AUFTRAEGE")

            Dim row As DataRow
            Dim auftrag As DataRow

            AuftragTable.Clear()
            For Each row In table.Rows
                auftrag = AuftragTable.NewRow
                auftrag.Item("Aufnr") = Right("0000000000" & row("Auftrag").ToString, 10)
                auftrag.Item("Fahrer") = Right("0000000000" & row("Fahrer").ToString, 10)
                auftrag.Item("Fahrtnr") = row("Tour").ToString
                AuftragTable.Rows.Add(auftrag)
            Next

            'myProxy.callBapi()
            S.AP.Execute()

            m_tblResult = S.AP.GetExportTable("T_AUFTRAEGE")

            WriteLogEntry(True, "", m_tblResult)
        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_intStatus = -2201
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.Customer.CustomerId, m_tblResult)
        End Try
    End Sub

    Public Shared Function CreateDirectory(ByVal Auftrag As String, ByVal KunNr As String) As Boolean
        Dim DirExist As Boolean = True
        If Not Directory.Exists(ConfigurationManager.AppSettings("UploadPathLocal") & KunNr & "\" & Auftrag & "\") Then
            Try
                Directory.CreateDirectory(ConfigurationManager.AppSettings("UploadPathLocal") & KunNr & "\" & Auftrag & "\")
                If Directory.Exists(ConfigurationManager.AppSettings("UploadPathLocal") & KunNr & "\" & Auftrag & "\") Then
                    DirExist = True
                Else
                    DirExist = False
                End If
            Catch ex As Exception
                DirExist = False
            End Try
        End If

        Return DirExist
    End Function

    Public Shared Function RemoveEmptyDirectories() As Integer
        Dim counter As Integer = 0

        Dim DirInfo As New DirectoryInfo(ConfigurationManager.AppSettings("UploadPathLocal"))
        Dim ExistingDirs() As DirectoryInfo = DirInfo.GetDirectories("*", SearchOption.AllDirectories)

        For Each Dir As DirectoryInfo In ExistingDirs
            If Dir.GetFiles().Length = 0 And Dir.GetDirectories().Length = 0 Then
                Dir.Delete()
                counter += 1
            End If
        Next

        Return counter
    End Function

    Public Shadows Function readDataFromServer(ByVal page As Page) As DataTable
        Dim table As New DataTable()
        Dim row As DataRow
        Dim column As DataColumn
        Dim files As String()
        Dim info As FileInfo
        Dim i As Integer
        Dim fname As String
        Dim anr As String
        Dim tour As String
        Dim pattern As String
        Dim strExtension As String

        column = New DataColumn("Zeit", Type.GetType("System.DateTime"))
        table.Columns.Add(column)
        column = New DataColumn("Serverpfad", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("Filename", Type.GetType("System.String"))
        table.Columns.Add(column)

        'Filename als Primärschlüssel setzen
        table.PrimaryKey = New DataColumn() {column}

        column = New DataColumn("FilenameNew", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Auftrag", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Tour", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Status", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Save", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("FilenameOld", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("Kunnr", Type.GetType("System.String"))      '§§§ JVE 25.09.2006
        table.Columns.Add(column)

        'If booJPG = True Then
        '    strExtension = fileExtensionjpg
        '    pattern = strPattern_05
        'Else
        strExtension = fileExtension
        pattern = strPattern_02
        'End If


        files = Directory.GetFiles(ConfigurationManager.AppSettings("UploadPathLocal"), "*" & strExtension, SearchOption.AllDirectories)


        '§§§ JVE 20061102
        'pattern = "(1|2|3|4|5|6|7|8|9)\d{0,9}" & fileNameDelimiter & "\d{4}" & fileNameDelimiter & "\d{4}" & fileNameDelimiter & "(1|2|3|4|5|6|7|8|9)" & fileExtension

        For i = 0 To files.Length - 1
            info = New FileInfo(files.GetValue(i).ToString)

            fname = files.GetValue(i).ToString

            If checkFilename(Right(fname, fname.Length - fname.LastIndexOf("\") - 1), pattern) Then 'Nur eigene Dateien auswählen...
                anr = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                anr = Left(anr, anr.IndexOf(fileNameDelimiter))

                tour = Right(fname, fname.Length - fname.LastIndexOf(fileNameDelimiter) - 1)
                tour = Left(tour, 1)

                Dim kunnr As String = getKunNr(anr, tour)
                'Alle Aufträge lesen (Liste für Kontrolleur)
                If (info.Extension.ToUpper = strExtension) Then
                    row = table.NewRow()
                    'fname = files.GetValue(i).ToString
                    row("Zeit") = info.CreationTime

                    row("Serverpfad") = ConfigurationManager.AppSettings("UploadPath") & kunnr & "\" & anr & "\" 'Dateien liegen auf gemapptem LW. = Lokal!
                    row("Filename") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                    row("FilenameOld") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                    row("Status") = "OK"
                    row("Auftrag") = anr
                    row("Tour") = tour

                    table.Rows.Add(row)
                    table.AcceptChanges()
                End If
            End If
        Next
        Return table
    End Function

    Public Shadows Function readDataFromServer(ByVal auftrag As String, ByVal tournr As String, ByVal kunnr As String, Optional ByVal booJPG As Boolean = False) As DataTable
        Dim table As New DataTable()
        Dim row As DataRow
        Dim column As DataColumn
        Dim files(0) As String
        Dim info As FileInfo
        Dim i As Integer
        Dim fname As String
        Dim anr As String
        Dim tour As String
        Dim pattern As String
        Dim strExtension As String

        column = New DataColumn("Zeit", Type.GetType("System.DateTime"))
        table.Columns.Add(column)
        column = New DataColumn("Serverpfad", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("Filename", Type.GetType("System.String"))
        table.Columns.Add(column)
        'Filename als Primärschlüssel setzen
        table.PrimaryKey = New DataColumn() {column}

        column = New DataColumn("FilenameNew", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Auftrag", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Tour", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Status", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Save", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("FilenameOld", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("Kunnr", Type.GetType("System.String"))      '§§§ JVE 25.09.2006
        table.Columns.Add(column)

        If booJPG = True Then
            strExtension = fileExtensionjpg
            pattern = strPattern_05
        Else
            strExtension = fileExtension
            pattern = strPattern_02
        End If

        If Directory.Exists(ConfigurationManager.AppSettings("UploadPathLocal")) Then

            If auftrag <> "" And tournr <> "" And kunnr <> "" Then
                If Directory.Exists(ConfigurationManager.AppSettings("UploadPathLocal") & kunnr & "\" & auftrag & "\") Then
                    files = Directory.GetFiles(ConfigurationManager.AppSettings("UploadPathLocal") & kunnr & "\" & auftrag & "\", "*" & strExtension)

                    '§§§ JVE 20061102
                    'pattern = "(1|2|3|4|5|6|7|8|9)\d{0,9}" & fileNameDelimiter & "\d{4}" & fileNameDelimiter & "\d{4}" & fileNameDelimiter & "(1|2|3|4|5|6|7|8|9)" & fileExtension

                    For i = 0 To files.Length - 1
                        If Not files Is Nothing Then

                            info = New FileInfo(files.GetValue(i).ToString)

                            fname = files.GetValue(i).ToString

                            If checkFilename(Right(fname, fname.Length - fname.LastIndexOf("\") - 1), pattern) Then 'Nur eigene Dateien auswählen...
                                anr = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                                anr = Left(anr, anr.IndexOf(fileNameDelimiter))

                                tour = Right(fname, fname.Length - fname.LastIndexOf(fileNameDelimiter) - 1)
                                tour = Left(tour, 1)

                                If (auftrag = String.Empty And tournr = String.Empty) Then
                                    kunnr = getKunNr(anr, tour)
                                    'Alle Aufträge lesen (Liste für Kontrolleur)
                                    If (info.Extension.ToUpper = strExtension) Then
                                        row = table.NewRow()
                                        'fname = files.GetValue(i).ToString
                                        row("Zeit") = info.CreationTime

                                        row("Serverpfad") = ConfigurationManager.AppSettings("UploadPath") & kunnr & "\" & anr & "\" 'Dateien liegen auf gemapptem LW. = Lokal!
                                        row("Filename") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                                        row("FilenameOld") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                                        row("Status") = "OK"
                                        row("Auftrag") = anr
                                        row("Tour") = tour
                                        row("Kunnr") = kunnr

                                        table.Rows.Add(row)
                                        table.AcceptChanges()
                                    End If
                                Else
                                    'Nur spezielle Auftrags-Nr. lesen (Fahrer)
                                    If (auftrag <> String.Empty And tour <> String.Empty) Then
                                        kunnr = getKunNr(auftrag, tour)
                                        'Beide Kriterien müssen gefüllt sein...
                                        If (info.Extension.ToUpper = strExtension) And (anr = auftrag) And (tour = tournr) Then
                                            row = table.NewRow()
                                            'fname = files.GetValue(i).ToString
                                            row("Zeit") = info.CreationTime
                                            row("Serverpfad") = ConfigurationManager.AppSettings("UploadPath") & kunnr & "\" & auftrag & "\"
                                            row("Filename") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                                            row("FilenameOld") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                                            row("Status") = "OK"
                                            row("Auftrag") = anr
                                            row("Tour") = tour
                                            row("Kunnr") = kunnr

                                            table.Rows.Add(row)
                                            table.AcceptChanges()
                                        End If
                                    Else
                                        table = Nothing
                                        '...sonst Fehler!
                                    End If
                                End If
                            End If
                        Else
                            table = Nothing
                        End If
                    Next
                Else
                    table = Nothing
                    '...sonst Fehler!
                End If
            Else
                Dim Dirs() As String = Directory.GetDirectories(ConfigurationManager.AppSettings("UploadPathLocal"), "*", SearchOption.AllDirectories)
                Dim Pat As String = "\d{0,10}(\\)(1|2|3|4|5|6|7|8|9)\d{0,9}" '(\-)(1|2|3|4|5|6|7|8|9)

                For Each Direc As String In Dirs
                    Dim blExit As Boolean = False

                    If checkFilename(Direc, Pat) Then
                        Dim tmpFiles() As String = Directory.GetFiles(Direc)
                        Dim k As Integer = files.GetLength(0)
                        If k = 1 Then
                            If files(0) = Nothing Then
                                k = 0
                            End If
                        End If

                        Dim l As Integer = tmpFiles.GetLength(0)
                        If l = 1 Then
                            If tmpFiles(0) = Nothing Then
                                l = 0
                            End If
                        End If

                        If l > 0 And k = 0 Then
                            ReDim files(l - 1)
                        ElseIf l > 0 And k > 0 Then
                            Dim tmp((k + l - 1)) As String
                            For i = 0 To k - 1
                                tmp(i) = files(i)
                            Next
                            files = tmp
                        Else
                            blExit = True
                        End If

                        If blExit = False Then
                            For i = 0 To tmpFiles.GetLength(0) - 1
                                files.SetValue(tmpFiles(i), (k + i))
                            Next
                        End If
                    End If
                Next

                For i = 0 To files.Length - 1
                    If Not files(0) Is Nothing Then


                        info = New System.IO.FileInfo(files.GetValue(i).ToString)

                        fname = files.GetValue(i).ToString

                        If checkFilename(Right(fname, fname.Length - fname.LastIndexOf("\") - 1), pattern) Then 'Nur eigene Dateien auswählen...
                            anr = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                            anr = Left(anr, anr.IndexOf(fileNameDelimiter))

                            tour = Right(fname, fname.Length - fname.LastIndexOf(fileNameDelimiter) - 1)
                            tour = Left(tour, 1)

                            If (auftrag = String.Empty And tournr = String.Empty) Then
                                kunnr = getKunNr(anr, tour)
                                'Alle Aufträge lesen (Liste für Kontrolleur)
                                If (info.Extension.ToUpper = strExtension) Then
                                    row = table.NewRow()
                                    'fname = files.GetValue(i).ToString
                                    row("Zeit") = info.CreationTime
                                    row("Serverpfad") = ConfigurationManager.AppSettings("UploadPath") & kunnr & "\" & anr & "\" 'Dateien liegen auf gemapptem LW. = Lokal!
                                    row("Filename") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                                    row("FilenameOld") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                                    row("Status") = "OK"
                                    row("Auftrag") = anr
                                    row("Tour") = tour
                                    row("Kunnr") = kunnr

                                    table.Rows.Add(row)
                                    table.AcceptChanges()
                                End If
                            Else
                                'Nur spezielle Auftrags-Nr. lesen (Fahrer)
                                If (auftrag <> String.Empty And tour <> String.Empty) Then
                                    kunnr = getKunNr(auftrag, tour)
                                    'Beide Kriterien müssen gefüllt sein...
                                    If (info.Extension.ToUpper = strExtension) And (anr = auftrag) And (tour = tournr) Then
                                        row = table.NewRow()
                                        'fname = files.GetValue(i).ToString
                                        row("Zeit") = info.CreationTime
                                        row("Serverpfad") = ConfigurationManager.AppSettings("UploadPath") & kunnr & "\" & auftrag & "\"
                                        row("Filename") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                                        row("FilenameOld") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                                        row("Status") = "OK"
                                        row("Auftrag") = anr
                                        row("Tour") = tour
                                        row("Kunnr") = kunnr

                                        table.Rows.Add(row)
                                        table.AcceptChanges()
                                    End If
                                Else
                                    table = Nothing
                                    '...sonst Fehler!
                                End If
                            End If
                        End If
                    Else
                        table = Nothing
                    End If
                Next
            End If

        Else
            table = Nothing
            '...sonst Fehler!
        End If

        Return table
    End Function

    ''' <summary>
    ''' Benennt Protokolldateien um und archiviert diese.
    ''' </summary>
    ''' <param name="table"></param>
    ''' <param name="status"></param>
    ''' <param name="IsTestUser"></param>
    ''' <remarks></remarks>
    Public Overrides Sub renameFilesProtocol(ByVal table As DataTable, ByRef status As String, ByVal IsTestUser As Boolean)
        Dim row As DataRow
        Dim fname As String
        Dim fnameNew As String
        Dim fnameTemp As String
        Dim auftrag As String
        Dim fahrer As String
        Dim tour As String
        Dim path As String
        Dim file As IO.FileInfo
        Dim maxIndex As Integer
        Dim pattern As String
        Dim fehler As String
        Dim kategorieValue As String
        Dim Protokollart As String

        status = String.Empty
        fehler = String.Empty
        '§§§JVE20061102
        'pattern = "(0|1|2|3|4|5|6|7|8|9)\d{0,9}" & fileNameDelimiter & "\d{4}" & fileNameDelimiter & "\d{4}" & fileNameDelimiter & "(0|1|2|3|4|5|6|7|8|9)P" & fileExtension
        pattern = strPattern_03

        Try
            For Each row In table.Rows
                If (row("Save").ToString = "X") Then
                    fname = row("Filename").ToString
                    path = ConfigurationManager.AppSettings("UploadPathLocalProtokoll")

                    auftrag = row("Auftrag").ToString
                    fahrer = row("Fahrer").ToString
                    tour = row("Tour").ToString
                    kategorieValue = row("kategorieValue").ToString
                    Protokollart = row("kategorie").ToString

                    If (Protokollart = String.Empty) Then
                        Protokollart = "PA"
                    End If

                    If tour.Trim.Length = 0 Then
                        tour = "1" 'tour = "000000"
                    End If

                    'counter = Right(fname, fname.Length - fname.IndexOf(fileNameDelimiter) - 1)
                    'counter = Left(counter, counter.IndexOf(fileNameDelimiter))

                    'maxIndex = getMaxFileIndex(ConfigurationManager.AppSettings("UploadPathSamba"), pattern, row("Auftrag").ToString, row("Tour").ToString)
                    '§§§ JVE 15.12.2005 <begin>
                    If (maxIndex = -1) Then
                        maxIndex = 0
                    End If
                    '§§§ JVE 15.12.2005 <end>
                    'fnameNew = ConfigurationManager.AppSettings("UploadpathLocal") & auftrag & fileNameDelimiter & Right("0000" & maxIndex, 4) & fileNameDelimiter & fahrer & fileNameDelimiter & tour & "P" & fileExtension
                    '§§§ CDI 16.11.2011
                    'fnameNew = ConfigurationManager.AppSettings("UploadPathLocalProtokoll") & auftrag & fileNameDelimiter & Right("0000" & kategorieValue, 4) & fileNameDelimiter & fahrer & fileNameDelimiter & tour & "P" & fileExtension
                    fnameNew = ConfigurationManager.AppSettings("UploadPathLocalProtokoll") & auftrag & fileNameDelimiter & tour & fileNameDelimiter & Protokollart & fileExtension

                    'fnameTemp = ConfigurationManager.AppSettings("ExcelPath") & auftrag & fileNameDelimiter & Right("0000" & kategorieValue, 4) & fileNameDelimiter & fahrer & fileNameDelimiter & tour & "P" & fileExtension
                    fnameTemp = ConfigurationManager.AppSettings("ExcelPath") & auftrag & fileNameDelimiter & tour & fileNameDelimiter & Protokollart & fileExtension

                    row("FilenameNew") = fnameNew
                    table.AcceptChanges()

                    fname = path & row("Filename").ToString
                    fnameNew = row("FilenameNew").ToString
                    file = New FileInfo(fname)
                    file.CopyTo(fnameNew, True)
                    file.CopyTo(fnameTemp, True)
                    'Umbenennen (=verschieben)
                    'file = Nothing
                    row("FilenameNew") = fnameTemp
                    sendMail(row, status)
                    row("FilenameNew") = fnameNew
                    If (status <> String.Empty) Then
                        fehler &= status
                    End If
                    transferFileProtocol(row, status, IsTestUser)
                    If (status <> String.Empty) Then
                        fehler &= status
                    End If
                End If
            Next
        Catch ex As Exception
            fehler &= ex.Message
            status = fehler
        End Try
    End Sub

    Public Shadows Sub transferFiles(ByRef table As DataTable, ByRef status As String, ByVal IsTestUser As Boolean)
        'Übertragen der Fotos auf den SAMBA-Server
        Dim row As DataRow
        Dim source As String
        'Dim target As String
        Dim targetArchiv As String  '§§§ JVE 13.06.2006
        Dim file As FileInfo
        Dim fileTarget As FileInfo
        Dim count As Integer
        Dim dirInfo As DirectoryInfo
        Dim strFilename As String

        status = String.Empty
        count = 0

        For Each row In table.Rows
            Try
                If (row("Save").ToString = "X") Then
                    Dim Kunnr As String = CStr(row("Kunnr"))
                    Dim Auftrag As String = CStr(row("Auftrag"))

                    'source = ConfigurationManager.AppSettings("UploadPathLocal") & CStr(row("Kunnr")) & "\" & CStr(row("Auftrag")) & "\" & row("Filename").ToString
                    source = ConfigurationManager.AppSettings("UploadPathLocal") & Kunnr & "\" & Auftrag & "\" & row("Filename").ToString

                    'Anpassen auf neue Verzeichnisstruktur! --------
                    targetArchiv = ConfigurationManager.AppSettings("UploadPathSambaArchive") & CStr(row("Kunnr")) & "\" & Right("0000000000" & CStr(row("Auftrag")), 10)
                    'Evtl. Verzeichnisse anlegen...
                    strFilename = row("Filename").ToString
                    file = New FileInfo(source)           'Quelldatei

                    dirInfo = New DirectoryInfo(targetArchiv)

                    If Not dirInfo.Exists Then
                        'Dim ds As New System.Security.AccessControl.DirectorySecurity("access", System.Security.AccessControl.AccessControlSections.All)
                        dirInfo.Create()
                        'System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings("UploadPathLocal") & KUNNR & "\" & Auftrag & "\")
                        CreateDirectory(Auftrag, Kunnr)
                    End If

                    targetArchiv = targetArchiv & "\" & strFilename
                    file.CopyTo(targetArchiv, True)

                    createThumb(source, CStr(row("Kunnr")), CStr(row("Auftrag")))            'Thumbnail erzeugen
                    'Checken, ob Datei vorhanden...
                    fileTarget = New FileInfo(targetArchiv)
                    If ((fileTarget.Exists) AndAlso (fileTarget.Length > 0)) Or (IsTestUser) Then
                        file.Delete()                           'Quelldatei löschen        
                        count += 1
                        status = String.Empty
                        row("Status") = count & "#" & ConfigurationManager.AppSettings("UploadPathSamba") & "#" & fileTarget.Name & "#" & fileTarget.CreationTime.ToString
                    Else
                        status = "Datei konnte nicht kopiert werden."
                    End If
                End If

            Catch ex As Exception
                row("Status") = ex.Message
                status = ex.Message
                Exit For
            End Try
        Next



    End Sub
    
#End Region

    ''' <summary>
    ''' Versucht den Zugriff auf einen Auftrag zu erlangen und sperrt diesen für andere Benutzer
    ''' </summary>
    ''' <param name="AuftragsNr">Auftragsnummer</param>
    ''' <param name="TourNr">Tour</param>
    ''' <param name="UserID">UserID</param>
    ''' <param name="SessionID">SessionID</param>
    ''' <param name="ConStr">SQL-Connectionstring</param>
    ''' <returns>liefert True zurück, wenn der Zugriff gewährt wurde</returns>
    ''' <remarks></remarks>
    Shared Function GetAuftragAccess(ByVal AuftragsNr As String, ByVal TourNr As String, ByVal UserID As String, ByVal SessionID As String, ByVal ConStr As String) As Boolean
        Dim con As New SqlClient.SqlConnection()
        con.ConnectionString = ConStr
        Try
            con.Open()

            Dim com As String = "SELECT * FROM [ArchivUse] WHERE Auftragsnummer = '" & AuftragsNr & "' AND Tour = '" & TourNr & "'"
            Dim SDA As New SqlClient.SqlDataAdapter(com, con)

            Dim table As New DataTable
            SDA.Fill(table)

            If table.Rows.Count = 0 Then
                com = "INSERT INTO [ArchivUse](Auftragsnummer, Tour, InUse, UserID, SessionID)" & _
                " VALUES('" & AuftragsNr & "', '" & TourNr & "', '1', '" & UserID & "', '" & SessionID & "')"
                Dim SQC As New SqlClient.SqlCommand(com, con)
                SQC.ExecuteNonQuery()
                Return True
            Else
                If CBool(table.Rows(0)("InUse")) Then
                    If CStr(table.Rows(0)("UserID")) <> UserID Then
                        Return False
                    Else
                        Return True
                    End If
                Else
                    com = "UPDATE [ArchivUse] SET InUse = 1, UserID = '" & UserID & "', SessionID='" & SessionID & "' WHERE Auftragsnummer = '" & AuftragsNr & _
                        "' AND Tour = '" & TourNr & "'"
                    Dim SQC As New SqlClient.SqlCommand(com, con)
                    SQC.ExecuteNonQuery()
                    Return True
                End If
            End If
        Catch ex As Exception
            Return False
        Finally
            con.Close()
        End Try
    End Function

    ''' <summary>
    ''' Gibt einen bestimmten Auftrag frei der alle Parameter erfüllt
    ''' </summary>
    ''' <param name="AuftragsNr">Auftragsnummer</param>
    ''' <param name="TourNr">Tour</param>
    ''' <param name="UserID">UserID</param>
    ''' <param name="ConStr">SQL-Connectionstring</param>
    ''' <remarks></remarks>
    Shared Sub DropAuftragAccess(ByVal AuftragsNr As String, ByVal TourNr As String, ByVal UserID As String, ByVal ConStr As String)
        Dim con As New SqlClient.SqlConnection()
        con.ConnectionString = ConStr

        Try
            con.Open()
            Dim com As String = "UPDATE [ArchivUse] SET InUse = 0 WHERE Auftragsnummer = '" & AuftragsNr & "' AND Tour = '" & TourNr & "' AND UserID = '" & UserID & "'"
            Dim SQC As New SqlClient.SqlCommand(com, con)
            SQC.ExecuteNonQuery()
        Catch ex As Exception

        Finally
            con.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Gibt alle vom Benutzer belegten Aufträge frei
    ''' </summary>
    ''' <param name="UserID">UserID des Benutzer des Aufträge freigegeben werden sollen.</param>
    ''' <param name="ConStr">SQL-Connectionstring</param>
    Shared Sub DropAuftragAccess(ByVal UserID As String, ByVal ConStr As String)
        Dim con As New SqlClient.SqlConnection()
        con.ConnectionString = ConStr

        Try
            con.Open()
            Dim com As String = "UPDATE [ArchivUse] SET InUse = 0 WHERE UserID = '" & UserID & "'"
            Dim SQC As New SqlClient.SqlCommand(com, con)
            SQC.ExecuteNonQuery()
        Catch ex As Exception

        Finally
            con.Close()
        End Try
    End Sub

    Shared Sub DeleteAuftragFromDB(ByVal AuftragsNr As String, ByVal TourNr As String, ByVal UserID As String, ByVal ConStr As String)
        Dim con As New SqlClient.SqlConnection()
        con.ConnectionString = ConStr

        Try
            con.Open()
            Dim com As String = "DELETE [ArchivUse] WHERE Auftragsnummer = '" & AuftragsNr & "' AND Tour = '" & TourNr & "' AND UserID = '" & UserID & "'"
            Dim SQC As New SqlClient.SqlCommand(com, con)
            SQC.ExecuteNonQuery()
        Catch ex As Exception

        Finally
            con.Close()
        End Try
    End Sub

    Public Function getKunNr(ByVal AuftragsNr As String, ByVal Tour As String) As String
        Dim kunnr As String = "Error"

        If AuftragsNr <> "" And Tour <> "" Then
            Try
                Dim obju As String
                If m_objUser Is Nothing Then
                    obju = "Nothing"
                Else
                    obju = "UserObjekt"
                End If

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_UEBERF_AUFTR_LISTE", m_objApp, m_objUser, page)

                'Dim table As DataTable = myProxy.getImportTable("T_AUFTRAEGE")
                'table.Clear()

                Dim table As DataTable = S.AP.GetImportTableWithInit("Z_V_UEBERF_AUFTR_LISTE.T_AUFTRAEGE")

                Dim row As DataRow = table.NewRow()

                row.Item("AUFNR") = AuftragsNr
                row.Item("FAHRTNR") = Tour
                table.Rows.Add(row)

                'myProxy.callBapi()
                S.AP.Execute()

                Dim tblResult As DataTable = S.AP.GetExportTable("T_AUFTRAEGE")

                kunnr = tblResult.Rows(0)("ZZKUNNR").ToString()

            Catch ex As Exception
                ' Create an EventLog instance and assign its source.
                Dim myLog As Diagnostics.EventLog = New Diagnostics.EventLog()
                myLog.Source = "Application"

                ' Write an informational entry to the event log.    
                myLog.WriteEntry("Catched: Kunnr: " & kunnr & " Aufnr: " & AuftragsNr & " Tour: " & Tour & " " & ex.Message)

                kunnr = "0000000001"

            End Try
        Else
            Return "0000000000"
        End If

        Return kunnr
    End Function

End Class
