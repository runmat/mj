Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

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

    Public Function GetLangText(ByVal strAppId As String, ByVal strSessionId As String, ByVal equi As String) As DataTable
        Dim datTable As New DataTable

        If Not m_blnGestartet Then
            m_blnGestartet = True

          Try
                S.AP.InitExecute("Z_M_Asl_Sis_Historie_Langtext", "I_KUNNR,I_EQUNR", Right("0000000000" & m_objUser.KUNNR, 10), equi)

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Asl_Sis_Historie_Langtext", m_objApp, m_objUser, Page)

                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_EQUNR", equi)

                'myProxy.callBapi()

                datTable = S.AP.GetExportTable("GT_WEB_LANGTEXT") 'myProxy.getExportTable("GT_WEB_LANGTEXT")
                
            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
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

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
                Return datTable
            Finally
                m_blnGestartet = False
                GetLangText = datTable
            End Try
        Else : Return Nothing
        End If

    End Function

    Public Overloads Sub FILL(ByVal strAppId As String, ByVal strSessionId As String, ByVal status As String, ByVal type As String, Optional ByRef mahn As Boolean = False)
        m_strClassAndMethod = "Report_002_01.FILL"
        m_strAppID = strAppId
        m_strSessionID = strSessionId

        If Not m_blnGestartet Then
            m_blnGestartet = True
            
            Try

                Dim strKlaer As String

                If blnKlaerfall = True Then
                    strKlaer = "X"
                Else
                    strKlaer = String.Empty
                End If

                S.AP.Init("Z_M_Asl_Sis_Historie")

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Asl_Sis_Historie", m_objApp, m_objUser, Page)

                S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10)) 'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_LIZNR_HIGH", strLeasingVertrNrBis) 'myProxy.setImportParameter("I_LIZNR_HIGH", strLeasingVertrNrBis)
                S.AP.SetImportParameter("I_LIZNR_LOW", strLeasingVertrNrVon) 'myProxy.setImportParameter("I_LIZNR_LOW", strLeasingVertrNrVon)
                S.AP.SetImportParameter("I_STATUS", status) 'myProxy.setImportParameter("I_STATUS", status)
                S.AP.SetImportParameter("I_KLAERFALL", strKlaer) 'myProxy.setImportParameter("I_KLAERFALL", strKlaer)
                S.AP.SetImportParameter("I_KONZS_ZL", strkundennr) 'myProxy.setImportParameter("I_KONZS_ZL", strkundennr)
                S.AP.SetImportParameter("I_KONZS_ZO", strkonzernnr) 'myProxy.setImportParameter("I_KONZS_ZO", strkonzernnr)
                S.AP.SetImportParameter("I_LICENSE_NUM_LOW", strKennzeichenVon) 'myProxy.setImportParameter("I_LICENSE_NUM_LOW", strKennzeichenVon)
                S.AP.SetImportParameter("I_LICENSE_NUM_HIGH", strKennzeichenBis) 'myProxy.setImportParameter("I_LICENSE_NUM_HIGH", strKennzeichenBis)
                S.AP.SetImportParameter("I_CHASSIS_NUM_LOW", strFahrgestellVon) 'myProxy.setImportParameter("I_CHASSIS_NUM_LOW", strFahrgestellVon)
                S.AP.SetImportParameter("I_CHASSIS_NUM_HIGH", strFahrgestellBis) 'myProxy.setImportParameter("I_CHASSIS_NUM_HIGH", strFahrgestellBis)

                S.AP.Execute()
                'myProxy.callBapi()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

                If Mahn = True Then
                    tblTemp2.DefaultView.RowFilter = "STORT <> '0001' AND STAT <> 'I0320'"

                    tblTemp2 = tblTemp2.DefaultView.ToTable
                End If

                CreateOutPut(tblTemp2, strAppId)    'Spalten übersetzen

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
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace(" Execution failed", "")
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
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "KennzeichenVon=" & strKennzeichenVon & ", KennzeichenBis=" & strKennzeichenBis & ", LeasingVertrNrVon=" & strLeasingVertrNrVon & ", LeasingVertrNrBis=" & strLeasingVertrNrBis & ", Status=" & status, m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub SaveComments(ByVal strAppId As String, ByVal strSessionId As String, ByVal equi As String, ByVal c1 As String,
                            ByVal c2 As String, ByVal c3 As String, ByVal c4 As String)

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try

                S.AP.Init("Z_M_Fleet_Bem_Update_001")

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Fleet_Bem_Update_001", m_objApp, m_objUser, Page)

                S.AP.SetImportParameter("I_EQUNR", equi) 'myProxy.setImportParameter("I_EQUNR", equi)
                S.AP.SetImportParameter("I_ZBE04", c1) 'myProxy.setImportParameter("I_ZBE04", c1)
                S.AP.SetImportParameter("I_ZBE05", c2) 'myProxy.setImportParameter("I_ZBE05", c2)
                S.AP.SetImportParameter("I_ZBE06", c3) 'myProxy.setImportParameter("I_ZBE06", c3)
                S.AP.SetImportParameter("I_ZBE07", c4) 'myProxy.setImportParameter("I_ZBE07", c4)


                S.AP.Execute()
                'myProxy.callBapi()

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case "NO_UPDATE"
                        m_strMessage = "Fehler bei Update."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Shared Function ToShortDateString(ByVal dat As String) As String
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
' $History: Arval_2.vb $
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 5.08.10    Time: 14:47
' Updated in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 5.08.10    Time: 8:49
' Updated in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 4.08.10    Time: 12:55
' Updated in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 30.06.09   Time: 13:03
' Updated in $/CKAG/Applications/apparval/Lib
' ITA: 2918
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
