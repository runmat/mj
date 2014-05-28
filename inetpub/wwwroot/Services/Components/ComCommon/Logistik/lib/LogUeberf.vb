Option Explicit On
Option Infer On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class LogUeberf
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
    Private mProtokoll As DataTable
    Private mUploadedProtokoll As DataTable
    Private Const fileNameDelimiter As Char = "-"c
    Private Const fileNameDelimiterNew As Char = "_"c
    Private Const fileExtension As String = ".JPG"
    Private Const strThumbPrefix As String = "THUMB_"
    Private Const strPattern_01 As String = "0" & fileNameDelimiter & "\d{4}" & fileNameDelimiter & fileNameDelimiter & "0" & fileExtension
    Private Const strPattern_02 As String = "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])" & fileExtension
    Private Const strPattern_03 As String = "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])*" & fileNameDelimiter & "([0-9])P" & fileExtension
    Private Const strPattern_04 As String = "\d*" & fileNameDelimiter & "\d\d\d\d" & fileNameDelimiter & "\d*" & fileNameDelimiter & "\d" & fileExtension
    Private Const strPattern_05 As String = "([0-9])*" & fileNameDelimiter & "\d" & fileNameDelimiter & "*" & fileExtension
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
    Public Property ProtokollArten() As DataTable
        Get
            Return mProtokoll
        End Get
        Set(ByVal value As DataTable)
            mProtokoll = value
        End Set
    End Property
    Public Property UploadedProtokolle() As DataTable
        Get
            Return mUploadedProtokoll
        End Get
        Set(ByVal value As DataTable)
            mUploadedProtokoll = value
        End Set
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


    Private Shared Function ThumbnailCallback() As Boolean
        Return False
    End Function

    Private Shared Function ThumbNailAbort() As Boolean
        'Do Nothing Here
    End Function

    Public Function readGroupData(ByVal page As Web.UI.Page, ByVal Kundennr As String) As DataView
        Dim dt As New DataTable()
        dt.Columns.Add("KUNNR", GetType(String))
        dt.Columns.Add("Anzeige", GetType(String))

        Try

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_Kcl_Gruppendaten", m_objApp, m_objUser, page)

            Dim Gruppe As String = m_objUser.Groups(0).GroupName
            Dim Kunnr As String = Kundennr.PadLeft(10, "0"c)

            myProxy.setImportParameter("KUNNR", Kunnr)
            myProxy.setImportParameter("GRUPPE", Gruppe)

            myProxy.callBapi()

            Dim Zzgruppendaten As DataTable = myProxy.getExportTable("ZZGRUPPENDATEN")

            Dim dr As DataRow

            For Each dRow As DataRow In Zzgruppendaten.Rows
                dr = dt.NewRow
                dr("KUNNR") = dRow("Zfiliale").ToString
                dr("Anzeige") = String.Concat(String.Concat(dRow("Name1").ToString, ", ", dRow("Ort01").ToString, ", "), dRow("Stras").ToString)
                dt.Rows.Add(dr)
                dt.AcceptChanges()
            Next

        Catch ex As Exception
            Select Case ex.Message
                Case "NO_DATA"
                    m_intStatus = -2201
                    m_strMessage = "Keine Daten zur Selektion vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
            End If
            WriteLogEntry(False, "KUNNR=" & m_objUser.Customer.CustomerId, dt)
        Finally
            If m_intIDSAP > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
            End If

        End Try

        dt.DefaultView.Sort = "Anzeige"
        Return dt.DefaultView
    End Function

    ''nicht sinnlos wird von anderen Reports z.B. Renault überführung genutzt JJ2007.11.19
    'Public Sub readSAPAuftraege(ByVal txtAuftrag As String, ByVal txtAuftragdatumVon As String, ByVal txtAuftragdatumBis As String, ByVal txtReferenz As String, ByVal txtKennzeichen As String, ByVal txtUeberfuehrungdatumVon As String, ByVal txtUeberfuehrungdatumBis As String, ByVal auftragart As String)
    '    'readSAPAuftraege2(txtAuftrag, txtAuftragdatumVon, txtAuftragdatumBis, txtReferenz, txtKennzeichen, txtUeberfuehrungdatumVon, txtUeberfuehrungdatumBis, auftragart, KUNNR, "", "")
    'End Sub


    Public Sub readSAPAuftraege2(ByVal txtAuftrag As String, ByVal txtAuftragdatumVon As String, _
                                 ByVal txtAuftragdatumBis As String, ByVal txtReferenz As String, _
                                 ByVal txtKennzeichen As String, ByVal txtUeberfuehrungdatumVon As String, _
                                 ByVal txtUeberfuehrungdatumBis As String, ByVal auftragart As String, _
                                 ByVal KUNNR As String, ByVal strLeasingGesellschaft As String, ByVal strLeasingKunde As String, _
                                 ByVal strAppID As String, _
                                 ByVal strSessionID As String, _
                                 ByVal page As Web.UI.Page, ByVal Kundennr As String)

        m_strClassAndMethod = "LogUeberf.Read"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_Ueberf_Auftr_Kund_Port", m_objApp, m_objUser, page)

                Dim rowIn As DataRow
                Dim sapTableIn As DataTable
                sapTableIn = myProxy.getImportTable("T_SELECT")
                rowIn = sapTableIn.NewRow

                'Importtabelle füllen
                rowIn("Aufnr") = Right("0000000000" & txtAuftrag, 10)
                If txtAuftragdatumVon <> String.Empty Then
                    rowIn("Erdat") = txtAuftragdatumVon
                End If

                If txtAuftragdatumBis <> String.Empty Then
                    rowIn("Erdat_Bis") = txtAuftragdatumBis
                End If

                If txtUeberfuehrungdatumVon <> String.Empty Then
                    rowIn("Vdatu") = txtUeberfuehrungdatumVon
                End If

                If txtUeberfuehrungdatumBis <> String.Empty Then
                    rowIn("Vdatu_Bis") = txtUeberfuehrungdatumBis
                End If
                rowIn("Zzrefnr") = txtReferenz
                rowIn("Zzkenn") = txtKennzeichen

                If String.IsNullOrEmpty(KUNNR) Then
                    rowIn("Kunnr_Ag") = Right("0000000000" & Kundennr, 10)
                Else
                    rowIn("Kunnr_Ag") = Right("0000000000" & KUNNR, 10)
                End If

                If m_objUser.Organization.AllOrganizations Then
                    rowIn("Zorgadmin") = "X"
                Else
                    rowIn("Zorgadmin") = String.Empty
                End If

                'hinzufügen von Selektionsparametern für Krüll JJ 2007.11.08
                If strLeasingGesellschaft <> String.Empty Then
                    rowIn("Name_Lg") = strLeasingGesellschaft
                End If

                If strLeasingKunde <> String.Empty Then
                    rowIn("Name_Ln") = strLeasingKunde
                End If


                If Not (m_objUser.Store = "AUTOHAUS" AndAlso m_objUser.Reference.Trim(" "c).Length > 0 AndAlso m_objUser.KUNNR = "261510") Then
                    If m_objUser.Reference <> String.Empty Then
                        rowIn("EX_KUNNR") = m_objUser.Reference
                    End If
                End If


                rowIn("Wbstk") = auftragart

                sapTableIn.Rows.Add(rowIn)
                sapTableIn.AcceptChanges()

                myProxy.callBapi()

                m_tblResult = myProxy.getExportTable("T_AUFTRAEGE")       'Ergebnis
                m_tblResult.Columns.Add("Counter", GetType(System.String))
                m_tblResult.Columns.Add("URL", GetType(System.String))

                m_tblResult.Columns.Add("Dokumente", GetType(System.String))
                Dim count As Integer = 1                        'Key (laufende Nr.) einbauen...
                Dim Row As DataRow
                For Each Row In m_tblResult.Rows
                    Row("AUFNR") = Row("Aufnr").ToString.TrimStart("0"c)
                    Row("Counter") = CStr(count)
                    Row("URL") = "Report02_1.aspx?REF=" & count
                    Row("Dokumente") = ""
                    Dim str As String = Row("ZZPROTOKOLL").ToString()
                    count += 1
                Next

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.Customer.CustomerId, m_tblResult)
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = -2201
                        m_strMessage = "Keine Daten zur Selektion vorhanden."
                    Case "U_INTERVAL"
                        m_intStatus = -9999
                        m_strMessage = "Range max. 3 Monate."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub getProtokollarten(ByVal kdNr As String, ByVal page As Web.UI.Page)
        m_intStatus = 0
        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_TAB_PROT_01", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR_AG", kdNr.PadLeft(10, "0"c).ToString)

            myProxy.callBapi()

            mProtokoll = myProxy.getExportTable("GT_OUT")

            mProtokoll.Columns.Add("ID", GetType(System.String))
            mProtokoll.Columns.Add("Filename", GetType(System.String))
            mProtokoll.Columns.Add("Filepath", GetType(System.String))
            mProtokoll.Columns.Add("Fahrt", GetType(System.String))
            mProtokoll.Columns.Add("Auftr", GetType(System.String))
            mProtokoll.Columns.Add("LVORM", GetType(System.String))
            Dim i As Integer = 1
            For Each Row As DataRow In mProtokoll.Rows
                Row("ID") = i
                Row("Filename") = ""
                Row("Filepath") = ""
                Row("Fahrt") = ""
                Row("Auftr") = ""
                Row("LVORM") = ""
                i += 1
                mProtokoll.AcceptChanges()
            Next

            mUploadedProtokoll = mProtokoll.Clone

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Es konnten keine Protokollarten ermittelt werden."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
        End Try
    End Sub

    Public Sub UpdateProtokollarten(ByVal objUser As CKG.Base.Kernel.Security.User, ByVal page As Web.UI.Page, ByVal RowAktion As DataRow)
        m_intStatus = 0
        Try

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_SET_KATEGORIE_01", m_objApp, m_objUser, page)

            myProxy.setImportParameter("WEB_USER", objUser.UserName.ToString)
            Dim tblUpdate As DataTable = myProxy.getImportTable("GT_IN")


            Dim UpdateRow As DataRow = tblUpdate.NewRow
            UpdateRow("VBELN") = Right("0000000000" & RowAktion("Auftr").ToString, 10)
            If RowAktion("Fahrt").ToString = "1" Then
                UpdateRow("FAHRT") = "H"
            ElseIf RowAktion("Fahrt").ToString = "2" Then
                UpdateRow("FAHRT") = "R"
            End If

            UpdateRow("ZZKATEGORIE") = RowAktion("ZZKATEGORIE").ToString
            UpdateRow("ZZPROTOKOLLART") = RowAktion("ZZPROTOKOLLART").ToString
            UpdateRow("LVORM") = RowAktion("LVORM").ToString
            If RowAktion("LVORM").ToString = "N" Then UpdateRow("LVORM") = ""
            tblUpdate.Rows.Add(UpdateRow)


            myProxy.callBapi()

            Dim tblReturn As DataTable = myProxy.getExportTable("GT_RET")

            For Each ReturnRow As DataRow In tblReturn.Rows
                If ReturnRow("RET_BEM").ToString <> "" Then
                    m_strMessage = ReturnRow("RET_BEM").ToString
                    m_intStatus = -9999
                End If
            Next

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
        End Try
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
        Dim auftrag As String

        auftrag = filename
        If Delimiter = fileNameDelimiter Then
            auftrag = Left(auftrag, auftrag.IndexOf(fileNameDelimiter))
        Else
            auftrag = Left(auftrag, auftrag.IndexOf(Delimiter))
        End If

        Return auftrag
    End Function

    Public Shared Function getTourFromFilename2(ByVal filename As String, Optional ByVal Delimiter As Char = fileNameDelimiter) As String
        Dim tour As String

        tour = filename
        If Delimiter = fileNameDelimiter Then
            tour = tour.Split(fileNameDelimiter)(1)
        Else
            tour = tour.Split(Delimiter)(1)
        End If

        Return tour
    End Function

#End Region
End Class
