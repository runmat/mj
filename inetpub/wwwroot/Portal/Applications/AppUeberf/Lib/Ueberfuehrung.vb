Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Public Class Ueberfuehrung
    REM § Lese-/Schreibfunktion, Kunde: CKAG, 
    REM § Show - BAPI: Z_V_Ueberf_Auftr_Fahrer,
    REM § Change - BAPI: .

    Inherits BankBase

#Region "Declarations"
    Private m_Fahrernummer As String
    Private m_Fahrername As String
    Private m_Ort As String
    Private m_Plz As String
    Private m_StrNr As String

    Private Const fileNameDelimiter As Char = "-"c
    Private Const fileExtension As String = ".JPG"
    Private Const strThumbPrefix As String = "THUMB_"
    Private Const strPattern_01 As String = "0" & fileNameDelimiter & "\d{4}" & fileNameDelimiter & fileNameDelimiter & "0" & fileExtension
    Private Const strPattern_02 As String = "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])" & fileExtension
    Private Const strPattern_03 As String = "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])P" & fileExtension
    Private Const strPattern_04 As String = "\d*" & fileNameDelimiter & "\d\d\d\d" & fileNameDelimiter & "\d*" & fileNameDelimiter & "\d" & fileExtension
#End Region

#Region " Properties"
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
#End Region

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
    
    'Private Shared Function ThumbnailCallback() As Boolean
    '    Return False
    'End Function

    'Private Shared Function ThumbNailAbort() As Boolean
    '    'Do Nothing Here
    'End Function

    Public Function readGroupData(ByVal page As Page) As DataView
        Dim dt As New DataTable()
        dt.Columns.Add("KUNNR", GetType(String))
        dt.Columns.Add("Anzeige", GetType(String))


        Try
            Dim Gruppe As String = m_objUser.Groups(0).GroupName

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_KCL_GRUPPENDATEN", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("KUNNR", m_strKUNNR)
            'myProxy.setImportParameter("GRUPPE", Gruppe)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_V_KCL_GRUPPENDATEN", "KUNNR,GRUPPE", m_strKUNNR, Gruppe)

            Dim groupData As DataTable = S.AP.GetExportTable("ZZGRUPPENDATEN")

            For Each dr As DataRow In groupData.Rows
                Dim NewRow As DataRow = dt.NewRow()


                NewRow("KUNNR") = dr("ZFILIALE")
                NewRow("Anzeige") = String.Concat(String.Concat(dr("NAME1"), ", ", dr("ORT01"), ", "), dr("STRAS"))
                dt.Rows.Add(NewRow)
            Next

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_intStatus = -2201
                    m_strMessage = "Keine Daten zur Selektion vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.Customer.CustomerId, dt)
    
        End Try

        dt.DefaultView.Sort = "Anzeige"

        Return dt.DefaultView
    End Function

    Public Sub readSAPAuftraege(ByVal txtAuftrag As String, ByVal txtAuftragdatumVon As String, ByVal txtAuftragdatumBis As String, ByVal txtReferenz As String,
                                ByVal txtKennzeichen As String, ByVal txtUeberfuehrungdatumVon As String, ByVal txtUeberfuehrungdatumBis As String,
                                ByVal auftragart As String)
        readSAPAuftraege2(txtAuftrag, txtAuftragdatumVon, txtAuftragdatumBis, txtReferenz, txtKennzeichen, txtUeberfuehrungdatumVon, txtUeberfuehrungdatumBis,
                          auftragart, KUNNR, "", "")
    End Sub

    Public Sub readSAPAuftraege2(ByVal txtAuftrag As String, ByVal txtAuftragdatumVon As String, ByVal txtAuftragdatumBis As String, ByVal txtReferenz As String,
                                 ByVal txtKennzeichen As String, ByVal txtUeberfuehrungdatumVon As String, ByVal txtUeberfuehrungdatumBis As String,
                                 ByVal auftragart As String, ByVal KUNNR As String, ByVal strLeasingGesellschaft As String, ByVal strLeasingKunde As String)
        'Auslesen der Aufträge zur Anzeige im Report und Verlinkung mit dem Easy-Archiv...
        m_strClassAndMethod = "Ueberfuehrung.writeSAPAuftraege()"

        ClearError()

        If Not m_blnGestartet Then
            Try
                m_blnGestartet = True

                Dim sapTableIn As DataTable
                Dim rowIn As DataRow
                Dim row As DataRow
                'Dim myProxy As DynSapProxyObj

                If auftragart = "N" Then
                    'myProxy = DynSapProxy.getProxy("Z_V_Ueberf_Auftr_Kund_Klaerf", m_objApp, m_objUser, Page)
                    S.AP.Init("Z_V_Ueberf_Auftr_Kund_Klaerf")
                Else
                    'myProxy = DynSapProxy.getProxy("Z_V_Ueberf_Auftr_Kund_Port", m_objApp, m_objUser, Page)
                    S.AP.Init("Z_V_Ueberf_Auftr_Kund_Port")
                End If

                sapTableIn = S.AP.GetImportTable("T_SELECT")
                rowIn = sapTableIn.NewRow()

                'Importtabelle(füllen)
                rowIn("Aufnr") = Right("0000000000" & txtAuftrag, 10)
                rowIn("Erdat") = DBNull.Value

                If txtAuftragdatumVon <> String.Empty Then
                    rowIn("Erdat") = CType(txtAuftragdatumVon, Date)
                End If

                rowIn("Erdat_Bis") = DBNull.Value

                If txtAuftragdatumBis <> String.Empty Then
                    rowIn("Erdat_Bis") = CType(txtAuftragdatumBis, Date)
                End If

                rowIn("Vdatu") = DBNull.Value

                If txtUeberfuehrungdatumVon <> String.Empty Then
                    rowIn("Vdatu") = CType(txtUeberfuehrungdatumVon, Date)
                End If

                rowIn("Vdatu_Bis") = DBNull.Value

                If txtUeberfuehrungdatumBis <> String.Empty Then
                    rowIn("Vdatu_Bis") = CType(txtUeberfuehrungdatumBis, Date)
                End If

                rowIn("Zzrefnr") = txtReferenz
                rowIn("Zzkenn") = txtKennzeichen

                If KUNNR = String.Empty Then
                    rowIn("Kunnr_Ag") = m_strKUNNR
                Else
                    rowIn("Kunnr_Ag") = KUNNR.PadLeft(10, "0"c)
                End If

                If m_objUser.Organization.AllOrganizations Then
                    rowIn("Zorgadmin") = "X"
                Else
                    rowIn("Zorgadmin") = String.Empty
                End If

                If strLeasingGesellschaft <> String.Empty Then
                    rowIn("Name_Lg") = strLeasingGesellschaft
                End If

                If strLeasingKunde <> String.Empty Then
                    rowIn("Name_Ln") = strLeasingKunde
                End If

                rowIn("Wbstk") = auftragart

                sapTableIn.Rows.Add(rowIn)
                sapTableIn.AcceptChanges()

                ' Leere Funktion -- CDI 31.07.2013 auskommentiert
                'MakeDestination()

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                
                'myProxy.callBapi()
                S.AP.Execute()

                m_tblResult = S.AP.GetExportTable("T_AUFTRAEGE")        'Ergebnis
                m_tblResult.Columns.Add("Counter", Type.GetType("System.String"))
                m_tblResult.Columns.Add("URL", Type.GetType("System.String"))

                Dim count As Integer = 1                        'Key (laufende Nr.) einbauen...

                For Each row In m_tblResult.Rows
                    row("AUFNR") = row("Aufnr").ToString.TrimStart("0"c)
                    row("Counter") = CStr(count)
                    row("URL") = "_Report041.aspx?REF=" & count
                    count += 1
                Next

                WriteLogEntry(True, "KUNNR=" & m_objUser.Customer.CustomerId, m_tblResult)
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        RaiseError("-2201", "Keine Daten zur Selektion vorhanden.")
                    Case Else
                        RaiseError("-9999", ex.Message)
                End Select

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Shared Function checkFilename(ByVal fname As String, ByVal pattern As String) As Boolean
        'Prüft, ob der Dateiname dem Muster Ziffernfolge(=Auftragsnr),Unterstrich,4 Ziffern(=Zähler),Unterstrich,Ziffernfolge(=Fahrernr.),Unterstrich,Ziffer(=Tour),Extension

        Dim exp As String
        Dim ret As Boolean = False

        exp = pattern

        Dim regEx As New System.Text.RegularExpressions.Regex(exp)

        If regEx.IsMatch(fname.ToUpper) Then
            ret = True
        End If

        Return ret
    End Function

    Public Shared Function getAuftragFromFilename(ByVal filename As String) As String
        Dim auftrag As String

        auftrag = filename
        auftrag = Left(auftrag, auftrag.IndexOf(fileNameDelimiter))
        Return auftrag
    End Function

    Public Shared Function getTourFromFilename(ByVal filename As String) As String
        Dim tour As String

        tour = filename
        tour = Right(tour, tour.Length - tour.LastIndexOf(fileNameDelimiter) - 1)
        tour = Left(tour, 1)
        Return tour
    End Function

    Public Shared Function getAuftragFromFilename2(ByVal filename As String, Optional ByVal Delimiter As Char = fileNameDelimiter) As String
        Return Left(filename, filename.IndexOf(Delimiter))
    End Function

    Public Shared Function getTourFromFilename2(ByVal filename As String, Optional ByVal Delimiter As Char = fileNameDelimiter) As String
        Return filename.Split(Delimiter)(1)
    End Function

#End Region

End Class

' ************************************************
' $History: Ueberfuehrung.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Lib
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 19.11.07   Time: 16:51
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 19.11.07   Time: 16:50
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 8.11.07    Time: 13:04
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 8.11.07    Time: 11:19
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 8.11.07    Time: 10:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 8.11.07    Time: 10:27
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 7.08.07    Time: 8:55
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' ITA: 1197
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 19.06.07   Time: 15:54
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' 
' *****************  Version 2  *****************
' User: Uha          Date: 7.03.07    Time: 15:38
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' Ueberfuehrung-Klasse für AppHYUNDAI erweitert
' 
' *****************  Version 1  *****************
' User: Uha          Date: 7.03.07    Time: 15:10
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' Klasse "Ueberfuehrung" (AppUeberf) und BAPI
' "Z_V_UEBERF_AUFTR_KUND_PORT" (SAPProxy_Ueberf) hinzugefügt
' 
' ************************************************
