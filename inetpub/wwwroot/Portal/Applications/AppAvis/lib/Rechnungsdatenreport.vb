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
    Private _mTblSap As DataTable
    Private _mTblSapKum As DataTable
    Private _mTblSpediteur As DataTable
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
    Property SapTable() As DataTable
        Get
            Return _mTblSap
        End Get
        Set(ByVal value As DataTable)
            _mTblSap = value
        End Set
    End Property
    Property SapTableKum() As DataTable
        Get
            Return _mTblSapKum
        End Get
        Set(ByVal value As DataTable)
            _mTblSapKum = value
        End Set
    End Property
    Property TableSpediteur() As DataTable
        Get
            Return _mTblSpediteur
        End Get
        Set(ByVal value As DataTable)
            _mTblSpediteur = value
        End Set
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, _
                              ByRef page As Page, _
                              ByVal strRechnungsnummer As String,
                              ByVal strRechnungsnummerBis As String, _
                              ByVal strFahrgestellnummer As String, _
                              ByVal strAusgabeart As String, _
                              ByVal strRechnungsdatumVon As String, _
                              ByVal strRechnungsdatumBis As String, _
                              ByVal strLeistungsdatumVon As String, _
                              ByVal strLeistungsdatumBis As String, _
                              ByVal strLeistungscodeVon As String, _
                              ByVal strLeistungscodeBis As String,
                              ByVal strAbgearbeitet As String, _
                              ByVal strGrunddatenvorlage As String, _
                              ByVal strSpediteur As String)

        m_strClassAndMethod = "Rechnungsdatenreport.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID


        Try
            m_tblRechnungsdaten = S.AP.GetExportTableWithInitExecute("Z_DPM_AVIS_EXP_REDAT.GT_OUT",
                "I_RECH_NR_VON,I_RECH_NR_BIS, I_FAHRGNR, I_SEL_ART, I_REDAT_VON, I_REDAT_BIS, I_LEIST_DATE_VON, I_LEIST_DATE_BIS, " & _
                "I_LEIST_CODE_VON, I_LEIST_CODE_BIS, I_SPEDI, I_ABGEARB,I_NUR_OHNE_GRUNDDAT",
                strRechnungsnummer, strRechnungsnummerBis, strFahrgestellnummer, strAusgabeart, strRechnungsdatumVon, strRechnungsdatumBis, strLeistungsdatumVon, strLeistungsdatumBis, _
                strLeistungscodeVon, strLeistungscodeBis, strSpediteur, strAbgearbeitet, strGrunddatenvorlage)

            SapTable = m_tblRechnungsdaten.Copy()

            SapTableKum = New DataTable()
            SapTableKum.Columns.Add("RECH_NR", GetType(String))
            SapTableKum.Columns.Add("SPEDI", GetType(String))
            SapTableKum.Columns.Add("ABGEARB_FLAG", GetType(String))
            SapTableKum.Columns.Add("ABGEARB_USER", GetType(String))

            Dim nRow As DataRow

            For Each row As DataRow In m_tblRechnungsdaten.Rows
                nRow = SapTableKum.NewRow()

                nRow("RECH_NR") = row("RECH_NR")
                nRow("SPEDI") = row("SPEDI")
                nRow("ABGEARB_FLAG") = row("ABGEARB_FLAG")
                nRow("ABGEARB_USER") = row("ABGEARB_USER")

                If SapTableKum.Select("RECH_NR = '" & row("RECH_NR").ToString() & "'").Length = 0 Then
                    SapTableKum.Rows.Add(nRow)
                End If

            Next

            CreateOutPut(m_tblRechnungsdaten, strAppID)
            m_tblExcel = m_tblResult.Copy

            WriteLogEntry(True, "I_RECH_NR=" & strRechnungsnummer & ", I_FAHRGNR=" & strFahrgestellnummer & ", I_SEL_ART=" & strAusgabeart & ", I_REDAT_VON=" & strRechnungsdatumVon & ", I_REDAT_BIS=" & strRechnungsdatumBis & ", I_LEIST_DATE_VON=" & strLeistungsdatumVon & ", I_LEIST_DATE_BIS=" & strLeistungsdatumBis & ", I_LEIST_CODE_VON=" & strLeistungscodeVon & ", I_SPEDI=" & strSpediteur, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            WriteLogEntry(False, "I_RECH_NR=" & strRechnungsnummer & ", I_FAHRGNR=" & strFahrgestellnummer & ", I_SEL_ART=" & strAusgabeart & ", I_REDAT_VON=" & strRechnungsdatumVon & ", I_REDAT_BIS=" & strRechnungsdatumBis & ", I_LEIST_DATE_VON=" & strLeistungsdatumVon & ", I_LEIST_DATE_BIS=" & strLeistungsdatumBis & ", I_LEIST_CODE_VON=" & strLeistungscodeVon & ", I_SPEDI=" & strSpediteur & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            m_blnGestartet = False
        End Try
    End Sub

    Public Overloads Sub FillSpediteur(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)

        m_strClassAndMethod = "Rechnungsdatenreport.FillSpediteur"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            _mTblSpediteur = S.AP.GetExportTableWithInitExecute("Z_DPM_READ_AUFTR_006.GT_OUT",
                "I_KUNNR,I_KENNUNG", m_objUser.KUNNR.ToSapKunnr(), "SPEDITEUR")

        Catch ex As Exception
            m_intStatus = -5555

            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        Finally
            m_blnGestartet = False
        End Try
    End Sub

    Public Sub Save(ByVal strAppId As String, ByVal strSessionId As String, ByVal saveTable As DataTable, ByVal compareTable As DataTable)

        m_strClassAndMethod = "Rechnungsdatenreport.Save"
        m_strAppID = strAppId
        m_strSessionID = strSessionId

        Try

            S.AP.Init("Z_DPM_AVIS_SET_ABGEARBEITET")

            Dim impTable As DataTable = S.AP.GetImportTable("GT_IN")

            Dim sapTableRow As DataRow
            Dim strAbgearbeitet As String

            For Each nRow As DataRow In saveTable.Rows

                sapTableRow = impTable.NewRow()

                strAbgearbeitet = compareTable.Select("RECH_NR = '" & nRow("RECH_NR").ToString() & "'")(0)("ABGEARB_FLAG").ToString()

                If strAbgearbeitet <> nRow("ABGEARB_FLAG").ToString() Then

                    sapTableRow("RECH_NR") = nRow("RECH_NR")
                    sapTableRow("ABGEARB_FLAG") = nRow("ABGEARB_FLAG")

                    If nRow("ABGEARB_FLAG").ToString() = "X" Then

                        sapTableRow("ABGEARB_USER") = nRow("ABGEARB_USER")

                    End If

                    impTable.Rows.Add(sapTableRow)
                End If

            Next

            S.AP.Execute()

            'Report-Logeintrag (ok)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception

            m_intStatus = -9999
            m_strMessage = "Fehler beim Speichern.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

#End Region
End Class

