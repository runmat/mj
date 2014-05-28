Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Imports SapORM.Contracts


'----------------------------------------------------------------------
' Class: Zulassungsreport
' Autor: O.Rudolph
' Beschreibung: füllen der Ausgabetabelle für Zulassungsreport (Report08.aspx)
' Erstellt am: 23.01.2009
' ITA: 2452
'----------------------------------------------------------------------

Public Class Zulassungsreport
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_tblExcel As DataTable
    Private m_tblVerwendung As DataTable
#End Region

#Region " Properties"
    Public Property ExcelResult() As DataTable
        Get
            Return m_tblExcel
        End Get
        Set(ByVal value As DataTable)
            m_tblExcel = value
        End Set
    End Property
    Public Property Verwendung() As System.Data.DataTable
        Get
            Return Me.m_tblVerwendung
        End Get
        Set(ByVal value As System.Data.DataTable)
            Me.m_tblVerwendung = value
        End Set
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    '----------------------------------------------------------------------
    ' Methode: Fill
    ' Autor: O.Rudolph
    ' Beschreibung: füllen der Ausgabetabelle für Zulassungsreport (Report08.aspx)
    ' Erstellt am: 23.01.2009
    ' ITA: 2452
    '----------------------------------------------------------------------

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page, _
                              ByVal datZuldatumVon As String, ByVal datZuldatumBis As String, ByVal strVerwendung As String, _
                              ByVal datFreisdatumVon As String, ByVal datFreisdatumBis As String)
        Dim strKunnr As String = ""
        m_strClassAndMethod = "Zulassungsreport.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim intID As Int32 = -1

        Try

            S.AP.InitExecute("Z_M_READ_ZUL_001",
                            "I_KUNNR_AG, I_ZULDAT_VON, I_ZULDAT_BIS, I_DAT_FREIS_ZUL_VON, I_DAT_FREIS_ZUL_BIS, I_VERWENDUNGSZWECK",
                            m_objUser.KUNNR.ToSapKunnr(), datZuldatumVon, datZuldatumBis, datFreisdatumVon, datFreisdatumBis, strVerwendung)

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_READ_ZUL_001", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'If IsDate(datZuldatumVon) Then
            '    myProxy.setImportParameter("I_ZULDAT_VON", datZuldatumVon)
            'End If

            'If IsDate(datZuldatumBis) Then
            '    myProxy.setImportParameter("I_ZULDAT_BIS", datZuldatumBis)
            'End If

            'If IsDate(datFreisdatumVon) Then
            '    myProxy.setImportParameter("I_DAT_FREIS_ZUL_VON", datFreisdatumVon)
            'End If
            'If IsDate(datFreisdatumBis) Then
            '    myProxy.setImportParameter("I_DAT_FREIS_ZUL_BIS", datFreisdatumBis)
            'End If

            'myProxy.setImportParameter("I_VERWENDUNGSZWECK", strVerwendung)
            'myProxy.callBapi()

            'm_tblVerwendung = myProxy.getExportTable("GT_WEB")
            m_tblVerwendung = S.AP.GetExportTable("GT_WEB")

            CreateOutPut(m_tblVerwendung, strAppID)
            m_tblExcel = m_tblResult.Copy

            With m_tblResult
                .Columns.Remove("Name")
                .Columns.Remove("Name2")
                .Columns.Remove("Make Code")
                .Columns.Remove("Modellbezeichnung")
                .Columns.Remove("Geplanter Liefertermin")
                .Columns.Remove("Antriebsart")
                .Columns.Remove("Anzahl Navi CD´s")
                .Columns.Remove("Farbe Deutsch")
                .Columns.Remove("Vermietgruppe")
                .Columns.Remove("Fahrzeugart")
                .Columns.Remove("Aufbauart")
                .Columns.Remove("Bezahltkennzeichen")
                .Columns.Remove("Lieferant")
                .Columns.Remove("Lieferantenkurzname")
                .Columns.Remove("Einkaufsindikator")
                .Columns.Remove("Navigation")
                .Columns.Remove("Reifengröße")
                .Columns.Remove("Sperrvermerk")
                .Columns.Remove("Owner Code")
            End With

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", I_ZULDAT_VON=" & datZuldatumVon & ", I_ZULDAT_BIS=" & datZuldatumBis, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", I_ZULDAT_VON=" & datZuldatumVon & ", I_ZULDAT_BIS=" & datZuldatumBis & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            m_blnGestartet = False
        End Try
    End Sub

    Public Sub GetVerwendung(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)
        '----------------------------------------------------------------------
        ' Methode: GetVerwendung
        ' Autor: ORU
        ' Beschreibung: fragt die Vorgabewerte(Verwendungszweck) im SAP ab
        ' Erstellt am: 07.07.2009
        ' ITA: 2962
        '----------------------------------------------------------------------


        m_intStatus = 0
        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_VORGABEWERTE_001", m_objApp, m_objUser, page)
            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_KENNUNG", "VERWENDUNG")
            'myProxy.callBapi()

            m_tblVerwendung = S.AP.GetExportTableWithInitExecute("Z_M_VORGABEWERTE_001.GT_WEB",
                                                                 "I_KUNNR, I_KENNUNG",
                                                                  m_objUser.KUNNR.ToSapKunnr(), "VERWENDUNG")

        Catch ex As Exception
            m_intStatus = -9999
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Verwendungzwecke vorhanden. "
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try

    End Sub
#End Region


End Class
