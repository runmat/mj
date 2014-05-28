Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports System.Configuration
Imports System.IO
Imports System.Linq
Imports System.Collections.Generic


Public Class Ueberfuehrung
    REM § Lese-/Schreibfunktion, Kunde: CKAG, 
    REM § Show - BAPI: Z_V_Ueberf_Auftr_Fahrer,
    REM § Change - BAPI: .

    Inherits BankBase

#Region "Declarations"
    Protected m_Fahrernummer As String
    Protected m_Fahrername As String
    Protected m_Ort As String
    Protected m_Plz As String
    Protected m_StrNr As String

    Protected Shared mEmails As DataTable


    Protected Const fileNameDelimiter As Char = "-"c
    Protected Const fileExtension As String = ".PDF"
    Protected Const fileExtensionjpg As String = ".JPG"
    Protected Const strThumbPrefix As String = "THUMB_"
    Protected Const strPattern_01 As String = "0" & fileNameDelimiter & "\d{4}" & fileNameDelimiter & fileNameDelimiter & "0" & fileExtension
    Protected Const strPattern_02 As String = "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])" & fileExtension
    Protected Const strPattern_03 As String = "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])P" & fileExtension
    Protected Const strPattern_04 As String = "\d*" & fileNameDelimiter & "\d\d\d\d" & fileNameDelimiter & "\d*" & fileNameDelimiter & "\d" & fileExtension
    Protected Const strPattern_05 As String = "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])" & fileExtensionjpg

#End Region

    Public Shared ReadOnly Property fileDelimiter() As String
        Get
            Return fileNameDelimiter
        End Get
    End Property

    Public Shared ReadOnly Property fileExt() As String
        Get
            Return fileExtension
        End Get
    End Property


    Public Shared ReadOnly Property fileExtJPG() As String
        Get
            Return fileExtensionjpg
        End Get
    End Property

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overrides Sub show()
        'no function
    End Sub

    Public Overrides Sub Change()
        'no function
    End Sub


    Public Sub getAuftraege(ByVal strAppID As String, ByVal strSessionID As String, ByVal fahrer As String)
        '§§§ JVE 16.01.2006 Paramater "fahrer" eingeführt, da es möglich sein soll, auch "fremde" Aufträge zu sehen...
        m_strClassAndMethod = "Ueberfuehrung.getAuftraege()"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'm_intIDSAP = -1

            Try
                ClearError()

                S.AP.InitExecute("Z_V_UEBERF_AUFTR_FAHRER", "I_FAHRER", Right("0000000000" & fahrer, 10))

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_UEBERF_AUFTR_FAHRER", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_FAHRER", Right("0000000000" & fahrer, 10))


                'myProxy.callBapi()
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
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c), m_tblResult)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub getAuftragsDatenByReferenz(ByVal referenz As String, ByVal auftrag As String)
        m_strClassAndMethod = "Ueberfuehrung.getAuftagsDaten()"
        If Not m_blnGestartet Then
            m_blnGestartet = True
            
            Try
                ClearError()

                S.AP.InitExecute("Z_V_Ueberf_Auftr_Referenz", "AUFNR, REFNR", auftrag, referenz)

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                m_tblResult = S.AP.GetExportTable("T_AUFTRAEGE") 'DirectCast(pE_T_AUFTRAEGE.Value, DataTable)

                'der biztalk gibt keine führenden Nullen mit 
                'diese applikation rechnet aber mit einer 10 stelligen auftragsnummer daher nullen 
                'manuell hinzufügen! JJU20081202

                For Each row As DataRow In m_tblResult.Rows
                    row("AUFNR") = Right("0000000000" & row("AUFNR").ToString, 10)
                Next

                mEmails = S.AP.GetExportTable("T_SMTP")

                WriteLogEntry(True, "", m_tblResult)
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        RaiseError("-9999", "Keine Daten vorhanden. ")
                    Case "NO_PROTOKOLL"
                        RaiseError("-9999", "Zum Auftrag wurde bereits ein Protokoll archiviert. ")
                    Case Else
                        RaiseError("-9999", "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")")
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    'Public Sub getAuftragsDaten(ByVal table As DataTable)
    '    m_strClassAndMethod = "Ueberfuehrung.getAuftagsDaten()"

    '    Try
    '        ClearError()

    '        S.AP.Init("Z_V_Ueberf_Auftr_Liste")
    '        Dim AuftragTable As DataTable = S.AP.GetImportTable("T_AUFTRAEGE")

    '        'Dim myProxy As DynSapProxyObj

    '        'myProxy = DynSapProxy.getProxy("Z_V_Ueberf_Auftr_Liste", m_objApp, m_objUser, Page)

    '        'Dim AuftragTable As DataTable = myProxy.getImportTable("T_AUFTRAEGE")
    '        Dim row As DataRow
    '        Dim auftrag As DataRow

    '        For Each row In table.Rows
    '            auftrag = AuftragTable.NewRow
    '            auftrag.Item("Aufnr") = Right("0000000000" & row("Auftrag").ToString, 10)
    '            auftrag.Item("Fahrer") = Right("0000000000" & row("Fahrer").ToString, 10)
    '            auftrag.Item("Fahrtnr") = row("Tour").ToString
    '            AuftragTable.Rows.Add(auftrag)
    '        Next

    '        S.AP.Execute()
    '        'myProxy.callBapi()

    '        m_tblResult = S.AP.getExportTable("T_AUFTRAEGE") 'myProxy.getExportTable("T_AUFTRAEGE")

    '        WriteLogEntry(True, "", m_tblResult)
    '    Catch ex As Exception
    '        Select Case ex.Message.Replace("Execution failed", "").Trim()
    '            Case "NO_DATA"
    '                RaiseError("-2201", "Keine Daten gefunden.")
    '            Case Else
    '                RaiseError("-9999", ex.Message)
    '        End Select
    '        WriteLogEntry(False, "KUNNR=" & m_objUser.Customer.CustomerId, m_tblResult)
    '    End Try
    'End Sub

    Public Function readThumbsFromServer() As DataTable
        Dim table As New DataTable()
        Dim row As DataRow
        Dim column As DataColumn
        Dim files As String()
        Dim info As FileInfo
        Dim i As Integer
        Dim fname As String

        column = New DataColumn("Zeit", Type.GetType("System.DateTime"))
        table.Columns.Add(column)
        column = New DataColumn("Serverpfad", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("Filename", Type.GetType("System.String"))
        table.Columns.Add(column)
        '#1003 02.05.07 TB - Filename als Primärschlüssel setzen
        table.PrimaryKey = New DataColumn() {column}
        column = New DataColumn("Status", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("FilenameNew", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Auftrag", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Tour", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Save", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Fahrer", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Info", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("Abgabedatum", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("FilenameOld", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("Kunnr", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("Kategorie", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("KategorieValue", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("VKORG", Type.GetType("System.String"))
        table.Columns.Add(column)

        Dim Stemp As String
        Stemp = ConfigurationManager.AppSettings("UploadpathLocalProtokoll")



        files = Directory.GetFiles(Stemp, "*" & fileExtension)


        For i = 0 To files.Length - 1
            info = New FileInfo(files.GetValue(i).ToString)

            fname = files.GetValue(i).ToString

            row = table.NewRow()
            row("Zeit") = info.CreationTime

            row("Serverpfad") = ConfigurationManager.AppSettings("UploadpathLocalProtokoll")
            row("Filename") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)

            row("Status") = String.Empty

            table.Rows.Add(row)
        Next

        Return table
    End Function

    Public Function CreateUploadTable() As DataTable
        Dim table As New DataTable()
        Dim column As DataColumn

        column = New DataColumn("Zeit", Type.GetType("System.DateTime"))
        table.Columns.Add(column)
        column = New DataColumn("Serverpfad", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("Filename", Type.GetType("System.String"))
        table.Columns.Add(column)

        table.PrimaryKey = New DataColumn() {column}
        column = New DataColumn("Status", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("FilenameNew", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("Filedata", Type.GetType("System.Object"))       'Die Datei
        table.Columns.Add(column)




        column = New DataColumn("Auftrag", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Tour", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Save", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Fahrer", Type.GetType("System.String"))
        table.Columns.Add(column)
        column = New DataColumn("Info", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("Abgabedatum", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("FilenameOld", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("Kunnr", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("Kategorie", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("KategorieValue", Type.GetType("System.String"))
        table.Columns.Add(column)

        column = New DataColumn("VKORG", Type.GetType("System.String"))
        table.Columns.Add(column)

        Return table
    End Function

    Public Shared Function checkFilename(ByVal fname As String, ByVal pattern As String) As Boolean
        'Prüft, ob der Dateiname dem Muster Ziffernfolge(=Auftragsnr),Unterstrich,4 Ziffern(=Zähler),Unterstrich,Ziffernfolge(=Fahrernr.),Unterstrich,Ziffer(=Tour),Extension

        Dim exp As String
        Dim ret As Boolean = False

        exp = pattern

        Dim regEx As New Text.RegularExpressions.Regex(exp)

        If regEx.IsMatch(fname.ToUpper) Then
            ret = True
        End If

        Return ret
    End Function

    Public Function readDataFromServer(Optional ByVal auftrag As String = "", Optional ByVal tournr As String = "",
                                       Optional ByVal booJPG As Boolean = False) As DataTable
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
        '#1003 02.05.07 TB - Filename als Primärschlüssel setzen
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

        column = New DataColumn("Kunnr", Type.GetType("System.String"))
        table.Columns.Add(column)

        If booJPG = True Then
            strExtension = fileExtensionjpg
            pattern = strPattern_05
        Else
            strExtension = fileExtension
            pattern = strPattern_02
        End If


        files = Directory.GetFiles(ConfigurationManager.AppSettings("UploadPathLocal"), "*" & strExtension)


        For i = 0 To files.Length - 1
            info = New FileInfo(files.GetValue(i).ToString)

            fname = files.GetValue(i).ToString

            If checkFilename(Right(fname, fname.Length - fname.LastIndexOf("\") - 1), pattern) Then 'Nur eigene Dateien auswählen...
                anr = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                anr = Left(anr, anr.IndexOf(fileNameDelimiter))

                tour = Right(fname, fname.Length - fname.LastIndexOf(fileNameDelimiter) - 1)
                tour = Left(tour, 1)

                If (auftrag = String.Empty And tournr = String.Empty) Then
                    'Alle Aufträge lesen (Liste für Kontrolleur)
                    If (info.Extension.ToUpper = strExtension) Then
                        row = table.NewRow()
                        row("Zeit") = info.CreationTime

                        row("Serverpfad") = ConfigurationManager.AppSettings("UploadPath") 'Dateien liegen auf gemapptem LW. = Lokal!
                        row("Filename") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                        row("FilenameOld") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                        row("Status") = "OK"
                        row("Auftrag") = anr
                        row("Tour") = tour

                        table.Rows.Add(row)
                        table.AcceptChanges()
                    End If
                Else
                    'Nur spezielle Auftrags-Nr. lesen (Fahrer)
                    If (auftrag <> String.Empty And tour <> String.Empty) Then
                        If (info.Extension.ToUpper = strExtension) And (anr = auftrag) And (tour = tournr) Then
                            row = table.NewRow()
                            row("Zeit") = info.CreationTime
                            row("Serverpfad") = ConfigurationManager.AppSettings("UploadPath")
                            row("Filename") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                            row("FilenameOld") = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                            row("Status") = "OK"
                            row("Auftrag") = anr
                            row("Tour") = tour

                            table.Rows.Add(row)
                            table.AcceptChanges()
                        End If
                    Else
                        table = Nothing
                    End If
                End If
            End If
        Next

        Return table
    End Function

    Public Function leseFahrerDatenSAP(ByVal strAppID As String, ByVal strSessionID As String, ByRef status As String, ByVal fahrer As String) As DataTable
        Dim table As DataTable
        Dim result As New DataTable()
        Dim row As DataRow
        Dim rowResult As DataRow
        Dim strItem As String

        getAuftraege(strAppID, strSessionID, fahrer)

        status = m_strMessage
        If status <> String.Empty Then
            'Fehler 
        Else
            result.Columns.Add("Daten", Type.GetType("System.String"))
            table = m_tblResult
            If table.Rows.Count > 0 Then
                'Ok, Liste mit den Tourdaten füllen....
                For Each row In table.Rows
                    strItem = String.Empty

                    If Not (row("AUFNR") Is Nothing) AndAlso (row("AUFNR").ToString.Length > 0) Then           'Auftragsnr
                        strItem &= row("AUFNR").ToString.TrimStart("0"c)
                    End If
                    If Not (row("FAHRTNR") Is Nothing) AndAlso (row("FAHRTNR").ToString.Length > 0) Then         'Fahrtnr
                        strItem &= "." & row("FAHRTNR").ToString
                    End If
                    If Not (row("FAHRTVON") Is Nothing) AndAlso (row("FAHRTVON").ToString.Length > 0) Then         'Von
                        strItem &= "." & row("FAHRTVON").ToString
                    End If
                    If Not (row("FAHRTNACH") Is Nothing) AndAlso (row("FAHRTNACH").ToString.Length > 0) Then         'Nach
                        strItem &= "->" & row("FAHRTNACH").ToString
                    End If
                    If Not (row("ZZKENN") Is Nothing) AndAlso (row("ZZKENN").ToString.Length > 0) Then       'Kennzeichen
                        strItem &= "." & row("ZZKENN").ToString
                    End If
                    If Not (row("ZZBEZEI") Is Nothing) AndAlso (row("ZZBEZEI").ToString.Length > 0) Then         'Fahrzeugtyp
                        strItem &= "." & row("ZZBEZEI").ToString
                    End If
                    rowResult = result.NewRow()
                    rowResult("Daten") = strItem
                    result.Rows.Add(rowResult)
                    result.AcceptChanges()

                Next
            End If
        End If
        Return result
    End Function

    Public Shared Function getMaxFileIndex(ByVal path As String, ByVal pattern As String, ByVal auftrag As String, ByVal tour As String,
                                           Optional ByVal booJPG As Boolean = False) As Integer
        Dim maxIndex As Integer
        Dim view As DataView
        Dim viewRow As DataRowView
        Dim row As DataRow
        Dim fileName As String
        Dim index As String
        Dim files As String()
        Dim i As Integer
        Dim info As FileInfo
        Dim fname As String
        Dim table As New DataTable()
        Dim tourTemp As String
        Dim strExtension As String

        With table.Columns
            table.Columns.Add("Auftrag", Type.GetType("System.String"))
            table.Columns.Add("Tour", Type.GetType("System.String"))
            table.Columns.Add("Filename", Type.GetType("System.String"))
        End With


        If booJPG = True Then
            strExtension = fileExtensionjpg
        Else
            strExtension = fileExtension
        End If


        files = Directory.GetFiles(path, "*" & strExtension)

        If files.Length > 0 Then
            For i = 0 To files.Length - 1
                info = New FileInfo(files.GetValue(i).ToString)
                fname = files.GetValue(i).ToString

                If checkFilename(fname, pattern) Then
                    row = table.NewRow
                    row("Filename") = fname

                    tourTemp = Left(Right(fname, fname.Length - fname.LastIndexOf(fileNameDelimiter) - 1), 1)

                    fname = Right(fname, fname.Length - fname.LastIndexOf("\") - 1)
                    fname = Left(fname, fname.IndexOf(fileNameDelimiter))

                    row("Auftrag") = fname
                    row("Tour") = tourTemp
                    table.Rows.Add(row)
                    table.AcceptChanges()
                End If
            Next

            view = table.DefaultView
            view.RowFilter = "Auftrag = '" & auftrag & "' AND Tour = '" & tour & "'"

            If view.Count > 0 Then
                view.Sort = "Filename desc"
                viewRow = view.Item(0)

                fileName = viewRow.Row("Filename").ToString
                index = Right(fileName, fileName.Length - fileName.IndexOf(fileNameDelimiter) - 1)
                index = Left(index, index.IndexOf(fileNameDelimiter))
                maxIndex = 1 + CInt(index)
            Else
                maxIndex = 0
            End If
        Else
            maxIndex = -1
        End If
        Return maxIndex
    End Function

    ''' <summary>
    ''' Mailversand
    ''' </summary>
    ''' <param name="row"></param>
    ''' <param name="status"></param>
    ''' <param name="Client"></param>
    ''' <remarks></remarks>
    Protected Shared Sub sendMail(ByVal row As DataRow, ByRef status As String, Optional ByVal Client As Boolean = False)
        Dim Mail As Net.Mail.MailMessage
        Dim mailAdresse As String
        Dim filename As String
        Dim file As Net.Mail.Attachment
        Dim referenz As String
        Dim mailBody As String

        status = String.Empty

        Try

            If row("Save").ToString = "X" Then
                For Each tmpRow As DataRow In mEmails.Select("AUFNR='" & Right("0000000000" & row("Auftrag").ToString, 10) &
                                                             "' AND FAHRTNR='" & row("Info").ToString.Substring(row("Info").ToString.IndexOf(":") + 1, 1) & "'")


                    referenz = row("Info").ToString
                    referenz = Left(referenz, referenz.IndexOf(":"))
                    referenz = Right(referenz, referenz.Length - referenz.IndexOf(".") - 1)

                    mailAdresse = tmpRow("SMTP_ADDR").ToString
                    row("Info") = row("Info").ToString + mailAdresse
                    If Client = True Then
                        filename = row("Filename").ToString
                    Else
                        filename = row("FilenameNew").ToString
                    End If



                    file = New Net.Mail.Attachment(filename)

                    If row("VKORG").ToString = "1060" Then
                        mailBody = ConfigurationManager.AppSettings("MailBodyKcl").ToString.Replace("§", vbCrLf)
                    Else
                        mailBody = ConfigurationManager.AppSettings("MailBody").ToString.Replace("§", vbCrLf)
                    End If

                    Dim smtpMailSender As String = ConfigurationManager.AppSettings("SmtpMailSender")
                    Dim smtpMailServer As String = ConfigurationManager.AppSettings("SmtpMailServer")
                    
                    Mail = New Net.Mail.MailMessage(smtpMailSender, mailAdresse.Trim, "Bestandsnummer: " & referenz, mailBody)
                    Mail.Attachments.Add(file)
                    Dim mailclient As New Net.Mail.SmtpClient(smtpMailServer)
                    mailclient.Send(Mail)
                    Mail.Attachments.Dispose()
                    Mail.Dispose()
                    file.Dispose()
                Next
            End If
            
        Catch ex As Exception
            row("Status") = "Fehler bei eMail-Versand:<br>" & ex.Message
            status = ex.Message
            row.AcceptChanges()
        End Try
    End Sub

    Public Overridable Sub renameFilesProtocol(ByVal table As DataTable, ByRef status As String, ByVal IsTestUser As Boolean)
        Dim row As DataRow
        Dim fname As String
        Dim fnameNew As String
        Dim fnameTemp As String
        Dim auftrag As String
        Dim fahrer As String
        Dim tour As String
        Dim path As String
        Dim file As FileInfo
        Dim maxIndex As Integer
        Dim pattern As String
        Dim fehler As String
        Dim kategorieValue As String

        status = String.Empty
        fehler = String.Empty
        pattern = strPattern_03

        Try
            For Each row In table.Rows
                If (row("Save").ToString = "X") Then
                    path = ConfigurationManager.AppSettings("UploadPathLocalProtokoll")

                    auftrag = row("Auftrag").ToString
                    fahrer = row("Fahrer").ToString
                    tour = row("Tour").ToString
                    kategorieValue = row("kategorieValue").ToString

                    If tour.Trim.Length = 0 Then
                        tour = "000000"
                    End If
                    
                    If (maxIndex = -1) Then
                        maxIndex = 0
                    End If

                    fnameNew = ConfigurationManager.AppSettings("UploadPathLocalProtokoll") & auftrag & fileNameDelimiter & Right("0000" & kategorieValue, 4) &
                        fileNameDelimiter & fahrer & fileNameDelimiter & tour & "P" & fileExtension
                    fnameTemp = ConfigurationManager.AppSettings("ExcelPath") & auftrag & fileNameDelimiter & Right("0000" & kategorieValue, 4) &
                        fileNameDelimiter & fahrer & fileNameDelimiter & tour & "P" & fileExtension

                    row("FilenameNew") = fnameNew
                    table.AcceptChanges()

                    fname = path & row("Filename").ToString
                    fnameNew = row("FilenameNew").ToString
                    file = New FileInfo(fname)
                    file.CopyTo(fnameNew, True)
                    file.CopyTo(fnameTemp, True)
                    'Umbenennen (=verschieben)
                    
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

    Public Sub renameFilesProtocolUpload(ByVal table As DataTable, ByRef status As String, ByVal IsTestUser As Boolean)
        Dim row As DataRow
        Dim fname As String
        Dim fnameNew As String
        Dim auftrag As String
        Dim fahrer As String
        Dim tour As String
        Dim path As String
        Dim maxIndex As Integer
        Dim pattern As String
        Dim fehler As String
        Dim kategorieValue As String
        Dim UpFile As HttpPostedFile
        status = String.Empty
        fehler = String.Empty

        pattern = strPattern_03

        Try
            For Each row In table.Rows
                If (row("Save").ToString = "X") Then


                    auftrag = row("Auftrag").ToString
                    fahrer = row("Fahrer").ToString
                    tour = row("Tour").ToString
                    kategorieValue = row("kategorieValue").ToString

                    If (maxIndex = -1) Then
                        maxIndex = 0
                    End If
                    fname = auftrag & fileNameDelimiter & Right("0000" & kategorieValue, 4) & fileNameDelimiter & fahrer &
                        fileNameDelimiter & tour & "P" & fileExtension
                    path = ConfigurationManager.AppSettings("UploadPathLocal")
                    fnameNew = ConfigurationManager.AppSettings("UploadPathLocalProtokoll") & fname



                    UpFile = CType(row("Filedata"), System.Web.HttpPostedFile)
                    UpFile.SaveAs(path & fname)
                    row("Filename") = path & fname
                    'Hochladen um an die Statusmail als Attachment angehängt zu werden

                    row("FilenameNew") = fnameNew
                    table.AcceptChanges()
                   
                    sendMail(row, status, True)
                    If (status <> String.Empty) Then
                        fehler &= status
                    End If
                    transferFileProtocolUpload(row, status, IsTestUser)
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
    
    Public Shared Function ThumbnailCallback() As Boolean
        Return False
    End Function

    Public Shared Function ThumbNailAbort() As Boolean
        'Do Nothing Here
        Return False
    End Function
    
    Public Shared Sub createThumb(ByVal sourceFile As String, ByVal strKunnr As String, ByVal strAuftrag As String)
        Dim destFile As String

        '------------------
        Dim MyImage As New Bitmap(sourceFile)
        Dim MyThumbNail As System.Drawing.Image


        destFile = ConfigurationManager.AppSettings("UploadPathSambaArchive") & Right("0000000000" & strKunnr, 10) & "\" &
            Right("0000000000" & strAuftrag, 10) & "\" & strThumbPrefix & Right(sourceFile, sourceFile.Length - sourceFile.LastIndexOf("\") - 1)
        '--------------------
        MyThumbNail = MyImage.GetThumbnailImage(75, 75, AddressOf ThumbNailAbort, IntPtr.Zero)

        MyThumbNail.Save(destFile, System.Drawing.Imaging.ImageFormat.Jpeg)


        MyImage.Dispose()

    End Sub

    Public Sub transferFileProtocol(ByRef row As DataRow, ByRef status As String, ByVal IsTestUser As Boolean)
        'Upload der Protokolldateien auf SAMBA-Server
        Dim source As String
        Dim targetArchiv As String
        Dim file As FileInfo
        Dim fileSource As FileInfo
        Dim count As Integer
        Dim strAuftrag As String
        Dim dirDirInfo As DirectoryInfo

        status = String.Empty
        count = 0

        Try
            If (row("Save").ToString = "X") Then
                source = row("FilenameNew").ToString
                source = Right(source, source.Length - source.LastIndexOf("\") - 1)

                strAuftrag = Right("0000000000" & Left(source, source.IndexOf(fileNameDelimiter)), 10)

                targetArchiv = ConfigurationManager.AppSettings("UploadPathSambaArchive") & CStr(row("Kunnr")) & "\" & Right("0000000000" & CStr(row("Auftrag")), 10)
                file = New FileInfo(ConfigurationManager.AppSettings("UploadPathLocalProtokoll") & source)
               
                dirDirInfo = New DirectoryInfo(targetArchiv)
                If Not dirDirInfo.Exists Then
                    dirDirInfo.Create()
                End If

                file.CopyTo(targetArchiv & "\" & source, False)        'Kopieren
               
                'Checken, ob Datei vorhanden...
                fileSource = New FileInfo(ConfigurationManager.AppSettings("UploadPathLocalProtokoll") & row("Filename").ToString)


                file = New FileInfo(ConfigurationManager.AppSettings("UploadPathLocalProtokoll") & source)
                If ((fileSource.Exists) AndAlso (fileSource.Length > 0)) Or (IsTestUser) Then

                    If file.Exists Then file.Delete() 'Quelldatei(en) löschen
                    fileSource.Delete()
                    count += 1
                    row("Status") = count & "#" & ConfigurationManager.AppSettings("UploadPathSamba") & "#" & fileSource.Name & "#" &
                        fileSource.CreationTime.ToString & "*" & Right(row("Info").ToString, row("Info").ToString.Length - row("Info").ToString.LastIndexOf("~") - 1)

                    status = String.Empty
                Else
                    status = "Datei konnte nicht kopiert werden."
                End If
            End If
        Catch ex As Exception
            row("Status") = ex.Message
            status = ex.Message
        End Try
    End Sub
    
    Public Sub transferFileProtocolUpload(ByRef row As DataRow, ByRef status As String, ByVal IsTestUser As Boolean)
        'Upload der Protokolldateien auf SAMBA-Server
        Dim source As String
        Dim target As String
        Dim targetArchiv As String
        Dim file As FileInfo
        Dim fileTarget As FileInfo
        Dim count As Integer
        Dim strAuftrag As String
        Dim dirDirInfo As DirectoryInfo
        Dim UpFile As HttpPostedFile

        status = String.Empty
        count = 0

        Try
            If (row("Save").ToString = "X") Then
                source = row("FilenameNew").ToString
                source = Right(source, source.Length - source.LastIndexOf("\") - 1)

                target = ConfigurationManager.AppSettings("UploadPathSamba") & source

                strAuftrag = Right("0000000000" & Left(source, source.IndexOf(fileNameDelimiter)), 10)

                targetArchiv = ConfigurationManager.AppSettings("UploadPathSambaArchive") & CStr(row("Kunnr")) & "\" & Right("0000000000" & CStr(row("Auftrag")), 10)
                '---------------------------------------------------------------------------
                file = New FileInfo(ConfigurationManager.AppSettings("UploadPathLocalProtokoll") & source)

                UpFile = CType(row("Filedata"), System.Web.HttpPostedFile)


                dirDirInfo = New DirectoryInfo(targetArchiv)
                If Not dirDirInfo.Exists Then
                    dirDirInfo.Create()
                End If

                UpFile.SaveAs(targetArchiv & "\" & source)
                
                'Checken, ob Datei vorhanden...
                fileTarget = New FileInfo(targetArchiv & "\" & source)
                
                If ((fileTarget.Exists) AndAlso (fileTarget.Length > 0)) Then
                    count += 1
                    status = String.Empty
                    row("Status") = count & "#" & Left(UpFile.FileName, UpFile.FileName.LastIndexOf("\")) & "#" & fileTarget.Name & "#" &
                        fileTarget.CreationTime.ToString & "*" & Right(row("Info").ToString, row("Info").ToString.Length - row("Info").ToString.LastIndexOf("~") - 1)
                Else
                    status = "Datei konnte nicht kopiert werden."
                End If
            End If
        Catch ex As Exception
            row("Status") = ex.Message
            status = ex.Message
        End Try
    End Sub

    Public Sub transferFiles(ByRef table As DataTable, ByRef status As String, ByVal IsTestUser As Boolean)
        'Übertragen der Fotos auf den SAMBA-Server
        Dim row As DataRow
        Dim source As String
        Dim targetArchiv As String
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
                    source = ConfigurationManager.AppSettings("UploadPathLocal") & row("Filename").ToString
                   
                    targetArchiv = ConfigurationManager.AppSettings("UploadPathSambaArchive") & CStr(row("Kunnr")) & "\" & Right("0000000000" & CStr(row("Auftrag")), 10)

                    'Evtl. Verzeichnisse anlegen...
                    strFilename = row("Filename").ToString
                    file = New FileInfo(source)           'Quelldatei
                    
                    dirInfo = New DirectoryInfo(targetArchiv)

                    If Not dirInfo.Exists Then
                        dirInfo.Create()
                    End If

                    targetArchiv = targetArchiv & "\" & strFilename
                    file.CopyTo(targetArchiv, True)         'Ins Archiv - Verzeichnis immer (auch bei Test - User)

                    createThumb(source, CStr(row("Kunnr")), CStr(row("Auftrag")))            'Thumbnail erzeugen
                    'Checken, ob Datei vorhanden...
                    fileTarget = New FileInfo(targetArchiv)
                    If ((fileTarget.Exists) AndAlso (fileTarget.Length > 0)) Or (IsTestUser) Then
                        file.Delete()                           'Quelldatei löschen        
                        count += 1
                        status = String.Empty
                        row("Status") = count & "§" & ConfigurationManager.AppSettings("UploadPathSambaArchive") & "§" & fileTarget.Name & "§" &
                            fileTarget.CreationTime.ToString
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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strAppID"></param>
    ''' <param name="strSessionID"></param>
    ''' <param name="table"></param>
    ''' <param name="status"></param>
    ''' <param name="VerArt"></param>
    ''' <remarks></remarks>
    Public Sub writeSAPProtocol(ByVal strAppID As String, ByVal strSessionID As String, ByRef table As DataTable,
                                ByRef status As String, Optional ByVal VerArt As String = "")
        'Kontrolle der Protokolle durch Kontrolleur: Nach Upload auf Easy-Server entsprechende Aufträge in SAP "abschließen"
        m_strClassAndMethod = "Ueberfuehrung.writeSAPProtocol()"

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim row As DataRow


            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Dim auftrag As String = ""
            Dim tour As String = ""
            Dim ret As String = ""
            Dim abgabedatum As String
            Dim kategorie As String


            Try
                ClearError()
                
                'm_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_V_Ueberf_Auftr_Protokoll_Ab",
                '                                                 m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                For Each row In table.Rows
                    If row("Save").ToString = "X" Then
                        auftrag = Right("0000000000" & row("Auftrag").ToString, 10)
                        tour = row("Tour").ToString
                        kategorie = row("kategorie").ToString
                        abgabedatum = row("Abgabedatum").ToString
                        If (abgabedatum = String.Empty) Then
                            abgabedatum = "00000000"
                        End If

                        S.AP.Init("Z_V_Ueberf_Auftr_Protokoll_Ab")
                        'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_Ueberf_Auftr_Protokoll_Ab", m_objApp, m_objUser, Page)

                        S.AP.setImportParameter("AUFNR", auftrag)
                        S.AP.setImportParameter("FAHRTNR", tour)
                        S.AP.setImportParameter("WADAT_IST", abgabedatum)
                        S.AP.setImportParameter("ZZKATEGORIE", kategorie)
                        S.AP.setImportParameter("VERARBEITUNG", VerArt)

                        S.AP.Execute()
                        'myProxy.callBapi()
                        ret = S.AP.getExportParameter("UPDATE")
                    End If
                Next

                WriteLogEntry(True, "KUNNR=" & m_objUser.Customer.CustomerId, m_tblResult)
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_CHANGE"
                        m_strMessage = "Auftrag konnte nicht geändert werden."
                        RaiseError("-2201", m_strMessage)
                        status = m_strMessage & "Auftrags-Nr.=" & auftrag
                    Case "NO_DATA"
                        m_strMessage = "Auftrag nicht vorhanden."
                        RaiseError("-2202", m_strMessage)
                        status = m_strMessage & "Auftrags-Nr.=" & auftrag
                    Case "NO_ARCHIV"
                        m_strMessage = "Archivierung nicht vorgesehen."
                        RaiseError("-2203", m_strMessage)
                        status = m_strMessage & "Auftrags-Nr.=" & auftrag
                    Case Else
                        RaiseError("-9999", ex.Message)
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.Customer.CustomerId, m_tblResult)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
   
    Public Sub writeSAP(ByVal strAppID As String, ByVal strSessionID As String, ByVal table As DataTable, ByRef status As String)
        'Kontrolle der Fotos durch Kontrolleur: Nach Upload auf Easy-Server entsprechende Aufträge in SAP "abschließen"
        m_strClassAndMethod = "Ueberfuehrung.writeSAP()"

        If Not m_blnGestartet Then
            m_blnGestartet = True
            m_intIDSAP = -1

            Dim auftrag As String = ""
            Dim tour As String = ""
            Dim ret As String = ""
            Dim row As DataRow

            Try
                ClearError()

                For Each row In table.Rows
                    If row("Save").ToString = "X" Then
                        auftrag = Right("0000000000" & row("Auftrag").ToString, 10)
                        tour = row("Tour").ToString

                        S.AP.InitExecute("Z_V_UEBERF_AUFTR_UPLOAD_ABSCHL", "AUFNR,FAHRTNR", auftrag, tour)

                        'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_UEBERF_AUFTR_UPLOAD_ABSCHL", m_objApp, m_objUser, page)

                        'myProxy.setImportParameter("AUFNR", auftrag)
                        'myProxy.setImportParameter("FAHRTNR", tour)

                        'myProxy.callBapi()

                        ret = S.AP.GetExportParameter("UPDATE")
                    End If
                Next
                
                WriteLogEntry(True, "KUNNR=" & m_objUser.Customer.CustomerId, m_tblResult)
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_CHANGE"
                        m_strMessage = "Auftrag konnte nicht geändert werden."
                        RaiseError("-2201", m_strMessage)
                        status = m_strMessage & "Auftrags-Nr.=" & auftrag
                    Case Else
                        RaiseError("-9999", ex.Message)
                End Select
                
                WriteLogEntry(False, "KUNNR=" & m_objUser.Customer.CustomerId, m_tblResult)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Shared Sub moveFiles(ByVal table As DataTable)
        Dim row As DataRow
        Dim fileName As String
        Dim fileNameNew As String
        Dim filePath As String
        Dim auftrag As String
        Dim tour As String
        Dim fahrer As String
        Dim maxCount As Integer
        Dim pattern As String

        pattern = strPattern_04

        For Each row In table.Rows
            filePath = ConfigurationManager.AppSettings("UploadPathLocal")

            fahrer = getFahrerFromFilename(row("Filename").ToString)

            fileName = row("Filename").ToString
            fileName = Left(fileName, fileName.IndexOf(fileNameDelimiter)) & "." & Left(Right(fileName, fileName.Length - fileName.LastIndexOf(fileNameDelimiter) - 1), 1)
            fileNameNew = row("FilenameNew").ToString

            'Alten Dateinamen merken...
            row("FilenameOld") = Right(fileName, fileName.Length - fileName.LastIndexOf("\") - 1)

            'Nur ändern, wenn unterschiedlich....
            If (fileNameNew <> String.Empty) AndAlso (fileName <> fileNameNew) Then
                auftrag = Left(fileNameNew, fileNameNew.IndexOf(".")) ' row("Auftrag").ToString
                tour = Right(fileNameNew, 1)

                maxCount = getMaxFileIndex(ConfigurationManager.AppSettings("UploadpathLocal"), pattern, auftrag, tour)

                'Neuen Dateinamen zusammenbasteln...
                fileNameNew = filePath & auftrag & fileNameDelimiter & Right("0000" & CType(maxCount, String), 4) & fileNameDelimiter & fahrer & fileNameDelimiter &
                    tour & fileExtension
                fileName = filePath & row("Filename").ToString
                'Datei umbenennen
                Try
                    File.Move(fileName, fileNameNew)
                    row("Filename") = auftrag & fileNameDelimiter & Right("0000" & CType(maxCount, String), 4) & fileNameDelimiter & fahrer & fileNameDelimiter &
                        tour & fileExtension ' fileNameNewServer   'Neuen Dateinamen setzen
                    row("Auftrag") = auftrag
                    row("Tour") = tour
                Catch ex As Exception
                    row("Status") = ex.Message      'Fehler anzeigen
                End Try
            End If
        Next
    End Sub

    Public Shared Sub writeFileForImport(ByVal table As DataTable, ByRef status As String)
        Dim writer As StreamWriter
        Dim fileLock As FileInfo
        Dim fileInfo As FileInfo
        Dim fileName As String
        Dim rowTxt As String
        Dim row As DataRow
        Dim counter As String

        status = String.Empty

        Try
            fileLock = New FileInfo(ConfigurationManager.AppSettings("UploadPathSamba") & ConfigurationManager.AppSettings("EasyLockFile"))
            If fileLock.Exists Then
                status = "System ausgelastet. Bitte versuchen Sie es in einigen Minuten noch einmal."
            End If
        Catch ex As Exception
            status = ex.Message
        End Try
        'Ok, kopieren möglich...
        If status = String.Empty Then
            fileName = ConfigurationManager.AppSettings("UploadPathSamba") & ConfigurationManager.AppSettings("EasyImportFile")
            writer = New StreamWriter(fileName, True, New Text.ASCIIEncoding())
            'Header
            Try

                fileInfo = New FileInfo(ConfigurationManager.AppSettings("UploadPathSamba") & ConfigurationManager.AppSettings("EasyImportFile"))

                If (fileInfo.Length = 0) Then  '****** Kopfzeile nur speichern, wenn neue Datei
                    writer.WriteLine("@IMPHDR,^Ueberfuehrung ALD^,1001,1002,1003,1004,2001")
                    writer.Flush()
                End If
                'Sätze
                For Each row In table.Rows
                    '****** Inhalt der Spalten
                    If (row("Save").ToString = "X") Then
                        fileName = row("FilenameNew").ToString

                        'Auftragsnr
                        rowTxt = "^" & row("Auftrag").ToString

                        'Bild oder Protokoll
                        If (Right(fileName, 5) = ("P" & fileExtension)) Then
                            rowTxt &= "^,^Protokoll^"
                        Else
                            rowTxt &= "^,^Bild^"
                        End If

                        'Lfd. Nr
                        counter = Right(fileName, fileName.Length - fileName.IndexOf(fileNameDelimiter) - 1)
                        counter = Left(counter, counter.IndexOf(fileNameDelimiter))
                        rowTxt &= ",^" & CStr(CInt(counter)) & "^"

                        'Fahrtnr
                        Dim tour As String = row("Tour").ToString
                        If tour.Trim.Length = 0 Then
                            tour = "000000"
                        End If
                        rowTxt &= ",^" & tour & "^"

                        'Dateiname Bild
                        fileName = Right(fileName, fileName.Length - fileName.LastIndexOf("\") - 1)
                        rowTxt &= ",^" & ConfigurationManager.AppSettings("EasyImportPath") & fileName & "^"

                        writer.WriteLine(rowTxt)
                        writer.Flush()
                    End If
                Next
            Catch ex As Exception
                status = ex.Message
            Finally
                Try
                    writer.Close()
                Catch ex As Exception
                    status = String.Empty
                End Try
            End Try
        End If
    End Sub

    Public Shared Function getFahrerFromFilename(ByVal filename As String) As String
        Dim fahrer As String

        fahrer = filename
        fahrer = Right(fahrer, fahrer.Length - fahrer.IndexOf(fileNameDelimiter) - 1)
        fahrer = Right(fahrer, fahrer.Length - fahrer.IndexOf(fileNameDelimiter) - 1)
        fahrer = Left(fahrer, fahrer.IndexOf(fileNameDelimiter))

        Return fahrer
    End Function

    Public Shared Function getTourFromFilename(ByVal filename As String) As String
        Dim tour As String
        tour = filename
        tour = Right(tour, tour.Length - tour.LastIndexOf(fileNameDelimiter) - 1)
        tour = Left(tour, 1)

        Return tour
    End Function

    Public Shared Function getAuftragFromFilename(ByVal filename As String) As String
        Dim auftrag As String
        auftrag = filename
        auftrag = Left(auftrag, auftrag.IndexOf(fileNameDelimiter))

        Return auftrag
    End Function

#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub


    Public Sub ReadAuftragsdaten(ByVal auftraege As List(Of ReviewAuftrag))
        m_strClassAndMethod = "Ueberfuehrung.ReadAuftragsdaten()"

        ClearError()

        Try

            S.AP.Init("Z_V_Ueberf_Auftr_Liste")

            Dim AuftragTable As DataTable = S.AP.getImportTable("T_AUFTRAEGE")
            'Dim myProxy As DynSapProxyObj

            'myProxy = DynSapProxy.getProxy("Z_V_Ueberf_Auftr_Liste", m_objApp, m_objUser, Page)

            'Dim AuftragTable As DataTable = myProxy.getImportTable("T_AUFTRAEGE")
            
            For Each auftrag In auftraege
                Dim row = AuftragTable.NewRow
                row("Aufnr") = auftrag.AuftragsNummer.ToString("0000000000")
                row("Fahrer") = auftrag.FahrerID.ToString("0000000000")
                row("Fahrtnr") = auftrag.Fahrt.ToString
                AuftragTable.Rows.Add(row)
            Next

            S.AP.Execute()
            'myProxy.callBapi()

            m_tblResult = S.AP.getExportTable("T_AUFTRAEGE")

            For Each row As DataRow In m_tblResult.Rows
                Dim aNr = Integer.Parse(row("Aufnr").ToString)
                Dim fID = Integer.Parse(row("Fahrer").ToString)
                Dim fNr = Integer.Parse(row("Fahrtnr").ToString)

                Dim auftrag = auftraege.FirstOrDefault(Function(a) a.AuftragsNummer = aNr AndAlso a.FahrerID = fID AndAlso a.Fahrt = fNr)
                If Not auftrag Is Nothing Then auftrag.AddInfo(row)
            Next

            WriteLogEntry(True, "", m_tblResult)
        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    RaiseError("-2201", "Keine Daten gefunden.")
                Case Else
                    RaiseError("-9999", ex.Message)
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.Customer.CustomerId, m_tblResult)
        End Try
    End Sub

    Public Sub AuftragAbschließen(ByVal auftrag As ReviewAuftrag)
        'Kontrolle der Fotos durch Kontrolleur: Nach Upload auf Easy-Server entsprechende Aufträge in SAP "abschließen"
        m_strClassAndMethod = "Ueberfuehrung.AuftragAbschließen()"
        
        Try
            ClearError()

            S.AP.InitExecute("Z_V_UEBERF_AUFTR_UPLOAD_ABSCHL", "AUFNR,FAHRTNR", auftrag.AuftragsNummer.ToString("0000000000"), auftrag.Fahrt.ToString)
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_UEBERF_AUFTR_UPLOAD_ABSCHL", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("AUFNR", auftrag.AuftragsNummer.ToString("0000000000"))
            'myProxy.setImportParameter("FAHRTNR", auftrag.Fahrt.ToString)

            'myProxy.callBapi()
            
            WriteLogEntry(True, "KUNNR=" & m_objUser.Customer.CustomerId, m_tblResult)
        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_CHANGE"
                    RaiseError("-2201", "Auftrag konnte nicht geändert werden.")
                Case Else
                    RaiseError("-9999", ex.Message)
            End Select
         
            WriteLogEntry(False, "KUNNR=" & m_objUser.Customer.CustomerId, m_tblResult)
        End Try
    End Sub
End Class

' ************************************************
' $History: Ueberfuehrung.vb $
' 
' *****************  Version 19  *****************
' User: Rudolpho     Date: 23.09.10   Time: 15:33
' Updated in $/CKAG/Applications/appdcl/Lib
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 4.02.10    Time: 14:38
' Updated in $/CKAG/Applications/appdcl/Lib
' ITA: 2918
' 
' *****************  Version 17  *****************
' User: Rudolpho     Date: 6.11.09    Time: 11:56
' Updated in $/CKAG/Applications/appdcl/Lib
' 
' *****************  Version 16  *****************
' User: Rudolpho     Date: 22.10.09   Time: 14:37
' Updated in $/CKAG/Applications/appdcl/Lib
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 12.10.09   Time: 12:32
' Updated in $/CKAG/Applications/appdcl/Lib
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 8.10.09    Time: 14:30
' Updated in $/CKAG/Applications/appdcl/Lib
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 9.07.09    Time: 10:13
' Updated in $/CKAG/Applications/appdcl/Lib
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 2.07.09    Time: 8:34
' Updated in $/CKAG/Applications/appdcl/Lib
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 23.06.09   Time: 16:29
' Updated in $/CKAG/Applications/appdcl/Lib
' ITA: 2885
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 30.04.09   Time: 9:25
' Updated in $/CKAG/Applications/appdcl/Lib
' ITA: 2837
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 27.04.09   Time: 11:14
' Updated in $/CKAG/Applications/appdcl/Lib
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 21.04.09   Time: 16:46
' Updated in $/CKAG/Applications/appdcl/Lib
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 12.01.09   Time: 15:13
' Updated in $/CKAG/Applications/appdcl/Lib
' ITA 2528
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 11.12.08   Time: 14:18
' Updated in $/CKAG/Applications/appdcl/Lib
' Fehlerexception hinzugefügt
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 2.12.08    Time: 14:52
' Updated in $/CKAG/Applications/appdcl/Lib
' ITA 2377 testfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 2.12.08    Time: 11:41
' Updated in $/CKAG/Applications/appdcl/Lib
' ita 2377 unfertig
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 20.08.08   Time: 15:53
' Updated in $/CKAG/Applications/appdcl/Lib
' Bugfix Mailversand Schlender
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 15.05.08   Time: 14:54
' Updated in $/CKAG/Applications/appdcl/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:00
' Created in $/CKAG/Applications/appdcl/Lib
' 
' *****************  Version 23  *****************
' User: Fassbenders  Date: 21.02.08   Time: 17:05
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Lib
' 
' *****************  Version 22  *****************
' User: Fassbenders  Date: 21.02.08   Time: 10:06
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Lib
' 
' *****************  Version 21  *****************
' User: Rudolpho     Date: 8.08.07    Time: 9:57
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Lib
' 
' *****************  Version 20  *****************
' User: Rudolpho     Date: 25.07.07   Time: 16:06
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Lib
' 
' *****************  Version 19  *****************
' User: Uha          Date: 2.07.07    Time: 16:55
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 18  *****************
' User: Rudolpho     Date: 18.06.07   Time: 13:42
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Lib
' 
' *****************  Version 17  *****************
' User: Uha          Date: 22.05.07   Time: 13:38
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Lib
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 16  *****************
' User: Uha          Date: 15.05.07   Time: 15:58
' Updated in $/CKG/Applications/AppDCL/AppDCLWeb/Lib
' Änderungen aus StartApplication vom 11.05.2007
' 
' ************************************************