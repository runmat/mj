Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Imports SapORM.Contracts


Public Class Rechnungsdatenreport
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_tblRechnungsdaten As DataTable
    Private m_tblExcel As DataTable
#End Region

#Region " Properties"
    Property Rechnungsdaten() As DataTable
        Get
            Return m_tblRechnungsdaten
        End Get
        Set(ByVal value As DataTable)
            m_tblRechnungsdaten = value
        End Set
    End Property
    Property ExcelResult() As DataTable
        Get
            Return m_tblExcel
        End Get
        Set(ByVal value As DataTable)
            m_tblExcel = value
        End Set
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, _
                              ByRef page As Page, _
                              ByVal strRechnungsnummer As String, _
                              ByVal strFahrgestellnummer As String, _
                              ByVal strAusgabeart As String, _
                              ByVal strRechnungsdatumVon As String, _
                              ByVal strRechnungsdatumBis As String, _
                              ByVal strLeistungsdatumVon As String, _
                              ByVal strLeistungsdatumBis As String, _
                              ByVal strLeistungsart As String, _
                              ByVal strSpediteur As String)
        'Dim strKunnr As String = ""
        m_strClassAndMethod = "Rechnungsdatenreport.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        'strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Try
            m_tblRechnungsdaten = S.AP.GetExportTableWithInitExecute("Z_DPM_AVIS_EXP_REDAT.GT_OUT",
                "I_RECH_NR, I_FAHRGNR, I_SEL_ART, I_REDAT_VON, I_REDAT_BIS, I_LEIST_DATE_VON, I_LEIST_DATE_BIS, I_LEIST_CODE, I_SPEDI",
                strRechnungsnummer, strFahrgestellnummer, strAusgabeart, strRechnungsdatumVon, strRechnungsdatumBis, strLeistungsdatumVon, strLeistungsdatumBis, strLeistungsart, strSpediteur)

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_AVIS_EXP_REDAT", m_objApp, m_objUser, Page)

            'If Not String.IsNullOrEmpty(strRechnungsnummer) Then
            '    myProxy.setImportParameter("I_RECH_NR", strRechnungsnummer)
            'End If
            'If Not String.IsNullOrEmpty(strFahrgestellnummer) Then
            '    myProxy.setImportParameter("I_FAHRGNR", strFahrgestellnummer)
            'End If
            'myProxy.setImportParameter("I_SEL_ART", strAusgabeart)
            'If Not String.IsNullOrEmpty(strRechnungsdatumVon) Then
            '    myProxy.setImportParameter("I_REDAT_VON", strRechnungsdatumVon)
            'End If
            'If Not String.IsNullOrEmpty(strRechnungsdatumBis) Then
            '    myProxy.setImportParameter("I_REDAT_BIS", strRechnungsdatumBis)
            'End If
            'If Not String.IsNullOrEmpty(strLeistungsdatumVon) Then
            '    myProxy.setImportParameter("I_LEIST_DATE_VON", strLeistungsdatumVon)
            'End If
            'If Not String.IsNullOrEmpty(strLeistungsdatumBis) Then
            '    myProxy.setImportParameter("I_LEIST_DATE_BIS", strLeistungsdatumBis)
            'End If
            'If Not String.IsNullOrEmpty(strLeistungsart) Then
            '    myProxy.setImportParameter("I_LEIST_CODE", strLeistungsart)
            'End If
            'If Not String.IsNullOrEmpty(strSpediteur) Then
            '    myProxy.setImportParameter("I_SPEDI", strSpediteur)
            'End If

            'myProxy.callBapi()

            'm_tblRechnungsdaten = myProxy.getExportTable("GT_OUT")

            CreateOutPut(m_tblRechnungsdaten, strAppID)
            m_tblExcel = m_tblResult.Copy

            With m_tblResult
                .Columns.Remove("Rechnungs-ID")
                .Columns.Remove("Auftraggeber")
                .Columns.Remove("Erstellungsdatum")
                .Columns.Remove("Anzahl")
                .Columns.Remove("Dienstleistungstext")
                .Columns.Remove("Carport")
                .Columns.Remove("PLZ von")
                .Columns.Remove("Ort von")
                .Columns.Remove("PLZ nach")
                .Columns.Remove("Ort nach")
                .Columns.Remove("SAP SD")
                .Columns.Remove("Hersteller")
                .Columns.Remove("Vertrag")
                .Columns.Remove("Carport-Bezeichnung")
                .Columns.Remove("Leasing")
                .Columns.Remove("Herstellernummer")
                .Columns.Remove("Make Code")
                .Columns.Remove("Aufbauart")
                .Columns.Remove("MVA-Nummer")
                .Columns.Remove("Owner Code")
                .Columns.Remove("Lieferant")
                .Columns.Remove("Kennzeichen")
                .Columns.Remove("Zulassungsdatum")
                .Columns.Remove("Außerbetriebsetzung")
            End With

            WriteLogEntry(True, "I_RECH_NR=" & strRechnungsnummer & ", I_FAHRGNR=" & strFahrgestellnummer & ", I_SEL_ART=" & strAusgabeart & ", I_REDAT_VON=" & strRechnungsdatumVon & ", I_REDAT_BIS=" & strRechnungsdatumBis & ", I_LEIST_DATE_VON=" & strLeistungsdatumVon & ", I_LEIST_DATE_BIS=" & strLeistungsdatumBis & ", I_LEIST_CODE=" & strLeistungsart & ", I_SPEDI=" & strSpediteur, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            'Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            WriteLogEntry(False, "I_RECH_NR=" & strRechnungsnummer & ", I_FAHRGNR=" & strFahrgestellnummer & ", I_SEL_ART=" & strAusgabeart & ", I_REDAT_VON=" & strRechnungsdatumVon & ", I_REDAT_BIS=" & strRechnungsdatumBis & ", I_LEIST_DATE_VON=" & strLeistungsdatumVon & ", I_LEIST_DATE_BIS=" & strLeistungsdatumBis & ", I_LEIST_CODE=" & strLeistungsart & ", I_SPEDI=" & strSpediteur & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            m_blnGestartet = False
        End Try
    End Sub
#End Region
End Class

