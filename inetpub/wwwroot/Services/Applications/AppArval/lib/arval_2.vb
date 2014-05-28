Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Common

Public Class Arval_2
    REM § Status-Report, Kunde: ASL, BAPI: Z_M_Abm_Abgemeldete_Kfz,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

    Private tableNative As DataTable

    Public Function getNativeData() As DataTable
        Return tableNative
    End Function

#Region "Declarations"
    Private strKennzeichenVon As String
    Private strKennzeichenBis As String
    Private strFahrgestellVon As String
    Private strFahrgestellBis As String
    Private strLeasingVertrNrVon As String
    Private strLeasingVertrNrBis As String
    Private strkonzernnr As String
    Private strkundennr As String
    Private blnKlaerfall As Boolean
    Private url As String
#End Region

#Region "Properties"
    Public Property PKennzeichenVon() As String
        Get
            Return strKennzeichenVon
        End Get
        Set(ByVal Value As String)
            strKennzeichenVon = Value
        End Set
    End Property

    Public Property PKennzeichenBis() As String
        Get
            Return strKennzeichenBis
        End Get
        Set(ByVal Value As String)
            strKennzeichenBis = Value
        End Set
    End Property

    Public Property PFahrgestellVon() As String
        Get
            Return strFahrgestellVon
        End Get
        Set(ByVal Value As String)
            strFahrgestellVon = Value
        End Set
    End Property

    Public Property PFahrgestellBis() As String
        Get
            Return strFahrgestellBis
        End Get
        Set(ByVal Value As String)
            strFahrgestellBis = Value
        End Set
    End Property

    Public Property PLeasingNrVon() As String
        Get
            Return strLeasingVertrNrVon
        End Get
        Set(ByVal Value As String)
            strLeasingVertrNrVon = Value
        End Set
    End Property

    Public Property PLeasingNrBis() As String
        Get
            Return strLeasingVertrNrBis
        End Get
        Set(ByVal Value As String)
            strLeasingVertrNrBis = Value
        End Set
    End Property

    Public Property PKundenNr() As String
        Get
            Return strkundennr
        End Get
        Set(ByVal Value As String)
            strkundennr = Value
        End Set
    End Property

    Public Property PKonzernNr() As String
        Get
            Return strkonzernnr
        End Get
        Set(ByVal Value As String)
            strkonzernnr = Value
        End Set
    End Property

    Public Property PKlaerfall() As Boolean
        Get
            Return blnKlaerfall
        End Get
        Set(ByVal Value As Boolean)
            blnKlaerfall = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Private Function checkInput() As Boolean
        Try
        Catch ex As Exception
        End Try
    End Function

    Public Function getLangText(ByVal equi As String) As DataTable
        Dim DatTable As New DataTable
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Asl_Sis_Historie_Langtext", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim proxy = DynSapProxy.getProxy("Z_M_Asl_Sis_Historie_Langtext", m_objApp, m_objUser, GetCurrentPage())

                proxy.setImportParameter("I_EQUNR", equi)
                proxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                proxy.callBapi()

                Dim tmpDatTable = proxy.getExportTable("GT_WEB_LANGTEXT")

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
                DatTable = tmpDatTable

            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case "NO_PARAMETER"
                        m_strMessage = "Eingabedaten nicht ausreichend."
                    Case "NO_ASL"
                        m_strMessage = "Falsche Kundennr."
                    Case "NO_LANGTEXT"
                        m_strMessage = ""
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
                Return DatTable
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If
                m_blnGestartet = False
                getLangText = DatTable
            End Try
        Else : Return Nothing
        End If

    End Function

    Private Function GetCurrentPage() As Page
        If HttpContext.Current Is Nothing OrElse HttpContext.Current.Handler Is Nothing Then Return Nothing
        Return TryCast(HttpContext.Current.Handler, Page)
    End Function

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal status As String, ByVal type As String)
        m_strClassAndMethod = "Report_002_01.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            Try

                Dim strKlaer = If(blnKlaerfall, "X", String.Empty)

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Asl_Sis_Historie", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim proxy = DynSapProxy.getProxy("Z_M_ASL_SIS_HISTORIE", m_objApp, m_objUser, GetCurrentPage())
                proxy.setImportParameter("I_CHASSIS_NUM_HIGH", strFahrgestellBis)
                proxy.setImportParameter("I_CHASSIS_NUM_LOW", strFahrgestellVon)
                proxy.setImportParameter("I_KLAERFALL", strKlaer)
                proxy.setImportParameter("I_KONZS_ZL", strkundennr)
                proxy.setImportParameter("I_KONZS_ZO", strkonzernnr)
                proxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                proxy.setImportParameter("I_LICENSE_NUM_HIGH", strKennzeichenBis)
                proxy.setImportParameter("I_LICENSE_NUM_LOW", strKennzeichenVon)
                proxy.setImportParameter("I_LIZNR_HIGH", strLeasingVertrNrBis)
                proxy.setImportParameter("I_LIZNR_LOW", strLeasingVertrNrVon)
                proxy.setImportParameter("I_STATUS", status)
                proxy.callBapi()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 = proxy.getExportTable("GT_WEB")
                CreateOutPut(tblTemp2, strAppID)    'Spalten übersetzen

                tableNative = m_tblResult.Copy
                tblTemp2 = m_tblResult.Copy

                '#### Überflüssige Spalten löschen

                Dim col As DataColumn
                Dim row As DataRow
                'LVNr.,Angelegt,Beginn,Ende,VersandLN,RückgabeLN,VersandVG,RückgabeVG,Versicherung,Status,Klärfall,Info,Equipment merken
                If type = "H" Then      'Historie

                    For Each col In tblTemp2.Columns
                        If (col.ColumnName <> "LVNr" And _
                                col.ColumnName <> "Angelegt" And _
                                col.ColumnName <> "Beginn" And _
                                col.ColumnName <> "Gepl Ende" And _
                                col.ColumnName <> "Versand" And _
                                col.ColumnName <> "Rückgabe_LN" And _
                                col.ColumnName <> "Versand_VG" And _
                                col.ColumnName <> "Rückgabe_VG" And _
                                col.ColumnName <> "Versicherung" And _
                                col.ColumnName <> "Status" And _
                                col.ColumnName <> "Klärfall" And _
                                col.ColumnName <> "Info" And _
                                col.ColumnName <> "Equipment") Then
                            m_tblResult.Columns.Remove(col.ColumnName)
                        End If
                    Next
                End If

                'LVNr.,Angelegt,Beginn,Ende,VersandLN,VersandVG,MahnstufeLN,Mahnstufe-VG,Mahndatum-LN*,Mahndatum-VG*,Status,Klärfall,Info,Equipment merken
                If (type = "M") Then      'Mahnungen
                    For Each col In tblTemp2.Columns
                        If (col.ColumnName <> "LVNr" And _
                                col.ColumnName <> "Angelegt" And _
                                col.ColumnName <> "Beginn" And _
                                col.ColumnName <> "Gepl Ende" And _
                                col.ColumnName <> "Versand" And _
                                col.ColumnName <> "Versand_LN" And _
                                col.ColumnName <> "Versand_VG" And _
                                col.ColumnName <> "Mahnstufe_LN" And _
                                col.ColumnName <> "Mahnstufe_VG" And _
                                col.ColumnName <> "Mahndatum_LN" And _
                                col.ColumnName <> "Mahndatum_VG" And _
                                col.ColumnName <> "Status" And _
                                col.ColumnName <> "Klärfall" And _
                                col.ColumnName <> "Info" And _
                                col.ColumnName <> "Equipment") Then
                            m_tblResult.Columns.Remove(col.ColumnName)
                        End If
                    Next
                End If

                If (type = "HM") Then      'Klärfälle
                    For Each col In tblTemp2.Columns
                        If (col.ColumnName <> "LVNr" And _
                                col.ColumnName <> "Angelegt" And _
                                col.ColumnName <> "Beginn" And _
                                col.ColumnName <> "Gepl Ende" And _
                                col.ColumnName <> "Versand_LN" And _
                                col.ColumnName <> "Kundennummer" And _
                                col.ColumnName <> "Kundenname" And _
                                col.ColumnName <> "Kundenbetreuer" And _
                                col.ColumnName <> "Klärfall" And _
                                col.ColumnName <> "Schlüsselnummer Klärfall" And _
                                col.ColumnName <> "Equipment") Then
                            m_tblResult.Columns.Remove(col.ColumnName)
                        End If
                    Next
                End If

                '#### Spalte Status neu formatieren (ohne jedesmal "Sicherungsschein" davor..)
                If (type <> "HM") Then
                    For Each row In m_tblResult.Rows
                        row("Status") = row("Status").ToString.Replace("Sicherungsschein", "")
                    Next
                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & "KennzeichenVon=" & strKennzeichenVon & ", KennzeichenBis=" & strKennzeichenBis & ", LeasingVertrNrVon=" & strLeasingVertrNrVon & ", LeasingVertrNrBis=" & strLeasingVertrNrBis & ", Status=" & status, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -1234
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case "NO_PARAMETER"
                        m_intStatus = -3333
                        m_strMessage = "Eingabedaten nicht ausreichend."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "KennzeichenVon=" & strKennzeichenVon & ", KennzeichenBis=" & strKennzeichenBis & ", LeasingVertrNrVon=" & strLeasingVertrNrVon & ", LeasingVertrNrBis=" & strLeasingVertrNrBis & ", Status=" & status, m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub saveComments(ByVal equi As String, ByVal c1 As String, ByVal c2 As String, ByVal c3 As String, ByVal c4 As String)
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Fleet_Bem_Update_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim proxy = DynSapProxy.getProxy("Z_M_FLEET_BEM_UPDATE_001", m_objApp, m_objUser, GetCurrentPage())
                proxy.setImportParameter("I_EQUNR", equi)
                proxy.setImportParameter("I_ZBE04", c1)
                proxy.setImportParameter("I_ZBE05", c2)
                proxy.setImportParameter("I_ZBE06", c3)
                proxy.setImportParameter("I_ZBE07", c4)
                proxy.callBapi()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case "NO_UPDATE"
                        m_strMessage = "Fehler bei Update."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Shared Function toShortDateString(ByVal dat As String) As String
        Dim result As String
        Try
            result = CType(dat, Date).ToShortDateString
            Return result
        Catch
            Return String.Empty
        End Try
    End Function
#End Region
End Class

' ************************************************
' $History: arval_2.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 21.04.09   Time: 17:25
' Created in $/CKAG2/Applications/AppArval/lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' 
' ************************************************
