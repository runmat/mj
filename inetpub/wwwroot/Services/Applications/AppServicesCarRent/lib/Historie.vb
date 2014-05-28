Option Explicit On
Option Strict On
Option Infer On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel.Security

Public Class Historie
    Inherits DatenimportBase

#Region " Declarations"
    Private m_strBriefnummer As String
    Private m_strFahrgestellnummer As String
    Private m_strAmtlKennzeichen As String
    Private m_strOrdernummer As String
    Private m_ArchivName As String

    Private m_tblHistory As DataTable
    Private m_tblResultModelle As DataTable
    Private m_tblResultStueckliste As DataTable
    Private m_tblQMEL_DATENTable As DataTable
    Private m_tblQMEL_TueteTable As DataTable
    Private m_tblQMMIDATENTable As DataTable
    Private mGT_ADDR As DataTable
    Private mGT_divFahrzeuge As DataTable
    Private mGT_TEXT As DataTable
    Private m_tblOptArchive As DataTable


    Private m_Links As HistorieLinks

#End Region

#Region " Properties"
    Public ReadOnly Property ResultModelle() As DataTable
        Get
            Return m_tblResultModelle
        End Get
    End Property

    Public ReadOnly Property ResultOptArchiv() As DataTable
        Get
            Return m_tblOptArchive
        End Get
    End Property

    Public ReadOnly Property ResultStueckliste() As DataTable
        Get
            Return m_tblResultStueckliste
        End Get
    End Property

    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property

    Public ReadOnly Property QMMIDATENTable() As DataTable
        Get
            Return m_tblQMMIDATENTable
        End Get
    End Property

    Public ReadOnly Property QMEL_TueteTable() As DataTable
        Get
            Return m_tblQMEL_TueteTable
        End Get
    End Property

    Public ReadOnly Property QMEL_DATENTable() As DataTable
        Get
            Return m_tblQMEL_DATENTable
        End Get
    End Property

    Public ReadOnly Property diverseFahrzeuge() As DataTable
        Get
            Return mGT_divFahrzeuge
        End Get
    End Property


    Public Property Kennzeichen() As String
        Get
            Return m_strAmtlKennzeichen
        End Get
        Set(ByVal Value As String)
            m_strAmtlKennzeichen = Value
        End Set
    End Property

    Public Property FahrgestellNr() As String
        Get
            Return m_strFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummer = Value
        End Set
    End Property
    Public Property Briefnummer() As String
        Get
            Return m_strBriefnummer
        End Get
        Set(ByVal Value As String)
            m_strBriefnummer = Value
        End Set
    End Property
    Public Property Ordernummer() As String
        Get
            Return m_strOrdernummer
        End Get
        Set(ByVal Value As String)
            m_strOrdernummer = Value
        End Set
    End Property
    'Public ReadOnly Property Archivname() As String
    '    Get
    '        Return m_ArchivName
    '    End Get
    'End Property
    Public ReadOnly Property Links As HistorieLinks
        Get
            Return m_Links
        End Get
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As User, ByVal objApp As CKG.Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page, ByVal strFahrgestellnummer As String)
        m_strClassAndMethod = "Historie.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            Try
                m_blnGestartet = True
                m_intStatus = 0

                Dim myProxy = DynSapProxy.getProxy("Z_DPM_LEBENSLAUF_TUETE_005", m_objApp, m_objUser, page)
                myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("I_VKORG", "1510")
                myProxy.setImportParameter("I_ZZFAHRG", UCase(strFahrgestellnummer))
                myProxy.callBapi()

                m_tblResult = myProxy.getExportTable("GT_WEB")
                m_tblQMEL_TueteTable = myProxy.getExportTable("GT_QMEL")
            Catch ex As Exception
                m_intStatus = -2222
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten vorhanden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overloads Sub FILLStueckliste(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page, ByVal strFahrgestellnummer As String, ByVal strKennzeichen As String)
        m_strClassAndMethod = "Historie.FILLStueckliste"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            Try
                m_blnGestartet = True
                m_intStatus = 0

                Dim myProxy = DynSapProxy.getProxy("Z_DPM_LEBENSLAUF_TUETENKOMP", m_objApp, m_objUser, page)
                myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("I_VKORG", "1510")
                If Not String.IsNullOrEmpty(strFahrgestellnummer) Then
                    myProxy.setImportParameter("I_ZZFAHRG", UCase(strFahrgestellnummer.Trim()))
                End If
                If Not String.IsNullOrEmpty(strKennzeichen) Then
                    myProxy.setImportParameter("I_LICENSE_NUM", UCase(strKennzeichen.Trim()))
                End If

                myProxy.callBapi()

                m_tblResultStueckliste = myProxy.getExportTable("GT_WEB")
            Catch ex As Exception
                m_intStatus = -2222
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten vorhanden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Füllt alle Daten aus dem Brieflebenslauf
    ''' </summary>
    ''' <param name="strAppID">AppID</param>
    ''' <param name="strSessionID">SessionID</param>
    ''' <param name="page">page-Objekt</param>
    Public Sub FillHistory(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Historie.FillHistory"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            Try
                m_blnGestartet = True
                m_intStatus = 0

                Dim myProxy = DynSapProxy.getProxy("Z_DPM_BRIEFLEBENSLAUF_005", m_objApp, m_objUser, page)
                myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("I_ZZBRIEF", UCase(m_strBriefnummer))
                myProxy.setImportParameter("I_VKORG", "1510")
                myProxy.setImportParameter("I_ZZKENN", UCase(m_strAmtlKennzeichen))
                myProxy.setImportParameter("I_ZZFAHRG", UCase(m_strFahrgestellnummer))
                'myProxy.setImportParameter("I_ZZREF1", UCase(m_strOrdernummer))
                myProxy.setImportParameter("I_LIZNR", UCase(m_strOrdernummer))
                myProxy.callBapi()

                m_tblHistory = myProxy.getExportTable("GT_WEB")
                m_tblQMMIDATENTable = myProxy.getExportTable("GT_QMMA")
                m_tblQMEL_DATENTable = myProxy.getExportTable("GT_QMEL")

                For Each dr As DataRow In m_tblQMEL_DATENTable.Rows
                    If dr("QMCOD").ToString() = "EFZS" Then
                        dr("NAME1_Z5") = dr("NAME1_ZB1")
                        dr("NAME2_Z5") = dr("NAME2_ZB1")
                        dr("STREET_Z5") = dr("STREET_ZB1")
                        dr("HOUSE_NUM1_Z5") = dr("HOUSE_NUM1_ZB1")
                        dr("POST_CODE1_Z5") = dr("POST_CODE1_ZB1")
                        dr("CITY1_Z5") = dr("CITY1_ZB1")
                    End If
                Next

                mGT_divFahrzeuge = myProxy.getExportTable("GT_FAHRG")
            Catch ex As Exception
                m_intStatus = -2222
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten vorhanden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Füllt die <b>Links</b> Eigenschaft des Objekts
    ''' </summary>
    ''' <param name="fahrgestNr">Fahrgestellnummer</param>
    ''' <param name="strAppID">AppID</param>
    ''' <param name="strSessionID">SessionID</param>
    ''' <param name="page">Page</param>
    ''' <returns>HistorieLinks-Objekt der <b>Links</b> Eigenschaft</returns>
    ''' <remarks></remarks>
    Public Function FillLinks(ByVal fahrgestNr As String, ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page) As HistorieLinks
        m_strClassAndMethod = "Historie.FillLinks"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_ArchivName = String.Empty

        If Not m_blnGestartet Then
            Try
                m_blnGestartet = True
                m_intStatus = 0

                Dim myProxy = DynSapProxy.getProxy("Z_DPM_READ_MB_ARCHIV_01", m_objApp, m_objUser, page)
                myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("I_FIN", fahrgestNr)
                myProxy.callBapi()

                Dim E_SUBRC = myProxy.getExportParameter("E_SUBRC").ToString()
                Dim E_MESSAGE = myProxy.getExportParameter("E_MESSAGE").ToString()

                If E_SUBRC = "0" Then
                    m_tblResult = myProxy.getExportTable("GT_OUT")

                    If m_tblResult.Rows.Count > 0 Then
                        m_ArchivName = m_tblResult.Rows(0)("ARCHNAME").ToString()
                        Dim lagerort = m_tblResult.Rows(0)("EQFNR").ToString() 'SIC
                        m_Links = New HistorieLinks(fahrgestNr, m_ArchivName, lagerort)

                        Dim ExistingFiles = m_Links.GetFileList()

                        'Dim i As Integer = 0
                        Dim limit As Integer = m_tblResult.Rows.Count - 1
                        For i = 0 To limit
                            ' Vorhandene Typen durchlaufen
                            For k = 0 To ExistingFiles.GetUpperBound(0)
                                If m_tblResult.Rows(i)("DOKUMENTENART").ToString() = ExistingFiles(k, 3) Then
                                    GoTo Nexti
                                End If
                            Next
                            ' DocType nicht vorhanden
                            m_tblResult.Rows(i).Delete()
                            
                            'i -= 1 ' nochmal durchlaufen
                            'limit = m_tblResult.Rows.Count - 1 'Grenze neu setzen weil Satz gelöscht
Nexti:                  Next
                        m_tblResult.AcceptChanges()
                    End If

                    m_tblOptArchive = m_tblResult
                Else
                    Throw New Exception("Fehlercode: " & E_SUBRC & " " & E_MESSAGE)
                End If
            Catch ex As Exception
                m_intStatus = -2222
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten vorhanden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If

        Return m_Links
    End Function
#End Region
End Class