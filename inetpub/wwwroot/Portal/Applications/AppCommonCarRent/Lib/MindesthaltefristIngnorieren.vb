Option Explicit On 
Option Strict On
Imports CKG
Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class MindesthaltefristIngnorieren
    REM § Show - BAPI: Z_M_ABMBEREIT_LAUFZEIT,
    REM § Change - BAPI: Z_M_ABMBEREIT_LAUFAEN.

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
        ' Beschreibung: Ruft das Bapi Z_M_ABMBEREIT_LAUFZEIT auf 
        ' Erstellt am: 16.02.2009
        ' ITA: 2586
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABMBEREIT_LAUFZEIT", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_ABMBEREIT_LAUFZEIT", "KUNNR", m_strKUNNR)

            Dim CarDetailTable As DataTable = S.AP.GetExportTable("AUSGABE") 'myProxy.getExportTable("AUSGABE")

            m_tblFahrzeuge = New DataTable()
            With m_tblFahrzeuge.Columns
                .Add("Equipmentnummer", System.Type.GetType("System.String"))
                .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                .Add("Briefnummer", System.Type.GetType("System.String"))
                .Add("Kennzeichen", System.Type.GetType("System.String"))
                .Add("Abmeldedatum", System.Type.GetType("System.DateTime"))
                .Add("ActionNOTHING", System.Type.GetType("System.Boolean"))
                .Add("ActionDELE", System.Type.GetType("System.Boolean"))
                .Add("Bemerkung", System.Type.GetType("System.String"))
                .Add("Action", System.Type.GetType("System.String"))
                .Add("Belegnr", System.Type.GetType("System.String"))
                .Add("Hersteller", System.Type.GetType("System.String"))
                .Add("Laufzeit", System.Type.GetType("System.String"))
                .Add("Zulassungsdatum", System.Type.GetType("System.DateTime"))
                .Add("Modellbezeichnung", String.Empty.GetType)
                .Add("TageZuMindesthaltefrist", String.Empty.GetType)
            End With


            If CarDetailTable.Rows.Count > 0 Then
                Dim rowNew As DataRow
                For Each tmpRow As DataRow In CarDetailTable.Rows
                    rowNew = m_tblFahrzeuge.NewRow
                    rowNew("Belegnr") = tmpRow("Kunpdi")
                    rowNew("Equipmentnummer") = tmpRow("Equnr")
                    rowNew("Fahrgestellnummer") = tmpRow("Zzfahrg")
                    rowNew("Briefnummer") = tmpRow("Zzbrief")
                    rowNew("Kennzeichen") = tmpRow("Zzkenn")
                    rowNew("Abmeldedatum") = tmpRow("Repla_Date")
                    rowNew("Modellbezeichnung") = tmpRow("Zzbezei")
                    rowNew("Bemerkung") = ""
                    rowNew("Action") = ""
                    rowNew("ActionNOTHING") = True
                    rowNew("ActionDELE") = False
                    rowNew("Hersteller") = tmpRow("Zzherst_Text")
                    rowNew("Zulassungsdatum") = tmpRow("Vdatu")
                    rowNew("Laufzeit") = tmpRow("ZZLAUFZEIT")
                    rowNew("TageZuMindesthaltefrist") = (CDate(tmpRow("REPLA_DATE")) - Today).Days

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
            Select Case ex.Message
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
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

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Abmbereit_Laufaen", m_objApp, m_objUser, page)

            Dim strActionText As String

            strActionText = "Löschen"
            Dim strBemerkung As String = "Haltefrist ignoriert"

            Dim tmpDataView As DataView
            tmpDataView = m_tblFahrzeuge.DefaultView
            tmpDataView.RowFilter = "ActionNOTHING = 0"

            For i = 0 To tmpDataView.Count - 1
                S.AP.InitExecute("Z_M_Abmbereit_Laufaen", "KUNNR,ZZKENN,ZZFAHRG,KUNPDI",
                          m_strKUNNR,
                          tmpDataView.Item(i)("Kennzeichen").ToString,
                          tmpDataView.Item(i)("Fahrgestellnummer").ToString,
                          tmpDataView.Item(i)("Belegnr").ToString)

                'myProxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("ZZKENN", CType(tmpDataView.Item(i)("Kennzeichen"), String))
                'myProxy.setImportParameter("ZZFAHRG", CType(tmpDataView.Item(i)("Fahrgestellnummer"), String))
                'myProxy.setImportParameter("KUNPDI", CType(tmpDataView.Item(i)("Belegnr"), String))

                'myProxy.callBapi()


                'm_tblFahrzeuge.AcceptChanges()
                Dim tmpRows As DataRow()

                tmpRows = m_tblFahrzeuge.Select("Fahrgestellnummer = '" & tmpDataView.Item(i)("Fahrgestellnummer").ToString & "'")
                If tmpRows.Length > 0 Then
                    tmpRows(0).BeginEdit()
                    tmpRows(0).Item("Action") = strActionText
                    tmpRows(0).Item("Bemerkung") = strBemerkung
                    tmpRows(0).EndEdit()
                    m_tblFahrzeuge.AcceptChanges()
                End If
            Next

        Catch ex As Exception
            m_intStatus = -9999
            Select Case ex.Message
                Case Else
                    m_strMessage = "Fehler bei der Speicherung der Vorgänge.<br>(" & ex.Message & ")"
            End Select
        End Try

    End Sub
#End Region

End Class

' ************************************************
' $History: MindesthaltefristIngnorieren.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:53
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' Warnungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 19.02.09   Time: 16:52
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2586
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 19.02.09   Time: 10:18
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2586/ 2588
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 16.02.09   Time: 14:19
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITa 2586/2588 unfertig
' 
'
' ************************************************
