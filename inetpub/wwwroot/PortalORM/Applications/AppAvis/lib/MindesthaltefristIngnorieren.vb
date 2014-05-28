Option Explicit On 
Option Strict On
Imports CKG
Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Imports SapORM.Contracts

Public Class MindesthaltefristIngnorieren

    Inherits Base.Business.BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private m_tblFahrzeuge As DataTable
#End Region

#Region " Properties"
    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblFahrzeuge
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrzeuge = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub


    Public Overrides Sub Show()

    End Sub


    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)
        '----------------------------------------------------------------------
        ' Methode: Show
        ' Autor: JJU
        ' Beschreibung: Ruft das Bapi Z_M_ABM_LAUFZEIT_001 auf 
        ' Erstellt am: 10.03.2009
        ' ITA: 2657
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABM_LAUFZEIT_001", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()

            'Dim CarDetailTable As New DataTable

            'CarDetailTable = myProxy.getExportTable("GT_WEB")

            m_tblFahrzeuge = New DataTable()
            With m_tblFahrzeuge.Columns

                .Add("Equipmentnummer", System.Type.GetType("System.String"))
                .Add("Kennzeichen", System.Type.GetType("System.String"))
                .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                .Add("HerstellerID", System.Type.GetType("System.String"))
                .Add("Hersteller", System.Type.GetType("System.String"))
                .Add("TypID", String.Empty.GetType)
                .Add("Modellbezeichnung", String.Empty.GetType)
                .Add("OwnerCode", String.Empty.GetType)
                .Add("Lieferant", String.Empty.GetType)
                .Add("Zulassungsdatum", System.Type.GetType("System.DateTime"))
                .Add("DatumHaltefrist", System.Type.GetType("System.DateTime"))
                .Add("Bemerkung", System.Type.GetType("System.String"))
                .Add("ActionNOTHING", System.Type.GetType("System.Boolean"))
                .Add("ActionDELE", System.Type.GetType("System.Boolean"))
                .Add("Action", System.Type.GetType("System.String"))
            End With

            Dim CarDetailTable As DataTable = S.AP.GetExportTableWithInitExecute("Z_M_ABM_LAUFZEIT_001.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())


            If CarDetailTable.Rows.Count > 0 Then
                Dim rowNew As DataRow
                For Each tmpRow As DataRow In CarDetailTable.Rows
                    rowNew = m_tblFahrzeuge.NewRow
                    rowNew("Bemerkung") = ""
                    rowNew("Action") = ""
                    rowNew("ActionNOTHING") = True
                    rowNew("ActionDELE") = False

                    rowNew("Equipmentnummer") = tmpRow("Equnr")
                    rowNew("Fahrgestellnummer") = tmpRow("CHASSIS_NUM")
                    rowNew("Kennzeichen") = tmpRow("KENNZEICHEN")
                    Dim datTemp As DateTime
                    rowNew("Modellbezeichnung") = tmpRow("BESCHREIBUNG")
                    datTemp = Nothing
                    rowNew("Hersteller") = tmpRow("MAKE_CODE")
                    rowNew("HerstellerID") = tmpRow("HERST_NUMMER")
                    rowNew("Lieferant") = tmpRow("LIEF_KURZNAME")
                    rowNew("OwnerCode") = tmpRow("OWNER_CODE")
                    rowNew("TypID") = tmpRow("TYP")
                    rowNew("DatumHaltefrist") = tmpRow("FRUEHST_ABM_DAT")
                    rowNew("Zulassungsdatum") = tmpRow("ZULASS_DAT")
                    'berechnung der tage bis die mindesthaltefrist erreicht wäre, JJU20090216
                    'rowNew("TageZuMindesthaltefrist") = (CDate(tmpRow("REPLA_DATE")) - Today).Days
                    m_tblFahrzeuge.Rows.Add(rowNew)
                Next
            Else
                m_intStatus = -2202
                m_strMessage = "Keine Vorgangsinformationen vorhanden."
            End If

            Dim col As DataColumn
            For Each col In m_tblFahrzeuge.Columns
                col.ReadOnly = False
            Next
        Catch ex As Exception
            m_intStatus = -9999
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
        End Try
    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)
        '----------------------------------------------------------------------
        ' Methode: Change
        ' Autor: JJU
        ' Beschreibung: führt das ignorieren der Haltefristen im SAP aus
        ' Erstellt am: 16.02.2009
        ' ITA: 2586
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_SAVE_ABMDAT_FZG_001", m_objApp, m_objUser, page)
            'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tmpSAPTable As DataTable = S.AP.GetExportTableWithInitExecute("Z_M_SAVE_ABMDAT_FZG_001.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

            Dim strActionText As String

            strActionText = "Löschen"
            Dim strBemerkung As String = "Haltefrist ignoriert"

            Dim tmpDataView As DataView
            tmpDataView = m_tblFahrzeuge.DefaultView
            tmpDataView.RowFilter = "ActionNOTHING = 0"
            'Dim tmpSAPTable As DataTable = myProxy.getImportTable("GT_WEB")
            Dim tmpRowSAP As DataRow
            For i = 0 To tmpDataView.Count - 1
                tmpRowSAP = tmpSAPTable.NewRow
                tmpRowSAP("EQUNR") = tmpDataView.Item(i)("Equipmentnummer").ToString
                tmpDataView.Item(i)("Action") = strActionText
                tmpDataView.Item(i)("Bemerkung") = strBemerkung
                tmpSAPTable.Rows.Add(tmpRowSAP)
            Next
            'myProxy.callBapi()
            'tmpSAPTable = myProxy.getExportTable("GT_WEB")

            For Each tmpRow As DataRow In tmpSAPTable.Rows
                Dim tmpRows As DataRow()
                tmpRows = m_tblFahrzeuge.Select("Equipmentnummer='" & tmpRow("EQUNR").ToString & "'")
                If tmpRows.Length > 0 Then
                    tmpRows(0).BeginEdit()
                    tmpRows(0).Item("Bemerkung") = tmpRow("RETUR_BEM").ToString
                    tmpRows(0).EndEdit()
                    m_tblFahrzeuge.AcceptChanges()
                End If
            Next
        Catch ex As Exception
            m_intStatus = -9999
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case Else
                    m_strMessage = "Fehler bei der Speicherung der Vorgänge.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
        End Try

    End Sub
#End Region
End Class

' ************************************************
' $History: MindesthaltefristIngnorieren.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 10.03.09   Time: 13:34
' Created in $/CKAG/Applications/AppAvis/lib
' ITA 2657 testfertig
' 
' ************************************************
