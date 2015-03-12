Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class change_06

    Inherits Base.Business.BankBase

#Region " Declarations"
    Private _mTblFahrzeuge As DataTable
#End Region

#Region " Properties"
    Public Property Fahrzeuge() As DataTable
        Get
            Return _mTblFahrzeuge
        End Get
        Set(ByVal value As DataTable)
            _mTblFahrzeuge = value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppId As String, ByVal strSessionId As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppId, strSessionId, strFileName)
    End Sub

    Public Overrides Sub Show()
    End Sub


    Public Overloads Sub Show(ByVal page As Page)
        m_strClassAndMethod = "Change_06.Show"

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If
        Try
            _mTblFahrzeuge = New DataTable()
            With _mTblFahrzeuge.Columns
                .Add("Equipmentnummer", Type.GetType("System.String"))
                .Add("Kennzeichen", Type.GetType("System.String"))
                .Add("Fahrgestellnummer", Type.GetType("System.String"))
                .Add("Versandanschrift", Type.GetType("System.String"))
                .Add("Versanddatum", Type.GetType("System.String"))
                .Add("ActionNOTHING", Type.GetType("System.Boolean"))
                .Add("ActionDELE", Type.GetType("System.Boolean"))
                .Add("Action", Type.GetType("System.String"))
                .Add("Bemerkung", Type.GetType("System.String"))
            End With

            m_intStatus = 0
            m_strMessage = ""

            S.AP.InitExecute("Z_M_Schlue_Temp_Vers_Mahn_001", "I_KUNNR", m_strKUNNR)

            Dim carDetail As DataRow
            Dim carDetailTable As DataTable = S.AP.GetExportTable("GT_WEB")

            If carDetailTable.Rows.Count > 0 Then
                Dim rowNew As DataRow
                For Each carDetail In carDetailTable.Rows

                    rowNew = _mTblFahrzeuge.NewRow
                    rowNew("Equipmentnummer") = carDetail("Equnr")
                    rowNew("Fahrgestellnummer") = carDetail("Chassis_Num")
                    rowNew("Versandanschrift") = carDetail("Name1").ToString & "," & carDetail("Street").ToString & "," & carDetail("Post_Code1").ToString & "," & carDetail("City1").ToString
                    rowNew("Kennzeichen") = carDetail("License_Num")
                    Dim datTemp As DateTime
                    'Versanddatum
                    If Not carDetail("Zztmpdt") Is DBNull.Value AndAlso IsNumeric(carDetail("Zztmpdt")) Then
                        datTemp = CDate(carDetail("Zztmpdt"))
                        If datTemp > CDate("01.01.1900") Then
                            rowNew("Versanddatum") = Left(CStr(datTemp), 10)
                        End If
                    End If

                    rowNew("Action") = ""
                    rowNew("ActionNOTHING") = True
                    rowNew("ActionDELE") = False
                    rowNew("Bemerkung") = ""
                    _mTblFahrzeuge.Rows.Add(rowNew)

                Next
            Else
                m_intStatus = -2202
                m_strMessage = "Keine Vorgangsinformationen vorhanden."
            End If

            Dim col As DataColumn
            For Each col In _mTblFahrzeuge.Columns
                col.ReadOnly = False
            Next

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, _mTblFahrzeuge)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -1234
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Unbekannter Fehler."
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & " , " & m_strMessage, _mTblFahrzeuge)
        End Try
    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Change(ByVal strAppId As String, ByVal strSessionId As String, ByVal page As Page)


        m_intStatus = 0
        m_strMessage = ""

        Dim tmpDataView As DataView
        tmpDataView = _mTblFahrzeuge.DefaultView

        tmpDataView.RowFilter = "ActionNOTHING = 0"



        For i As Int32 = 0 To tmpDataView.Count - 1
            Dim strBemerkung As String = "Vorgang erfolgreich"
            Dim strActionText As String = "Löschen"

            Try

                S.AP.InitExecute("Z_M_Schlue_Set_Mahnsp_001", "I_KUNNR,I_EQUNR,I_ZZMANSP", m_strKUNNR, CType(tmpDataView.Item(i)("Equipmentnummer"), String), "X")

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = -1234
                        m_strMessage = "Equipment nicht gefunden."
                    Case "NO_UPDATE"
                        m_intStatus = -4554
                        m_strMessage = "Fehler bei der Aktualisierung."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Unbekannter Fehler."
                End Select
                strBemerkung = m_strMessage
            End Try

            _mTblFahrzeuge.AcceptChanges()
            Dim tmpRows As DataRow()

            tmpRows = _mTblFahrzeuge.Select("Fahrgestellnummer = '" & CType(tmpDataView.Item(i)("Fahrgestellnummer"), String) & "'")
            If tmpRows.Length > 0 Then
                tmpRows(0).BeginEdit()
                tmpRows(0).Item("Action") = strActionText
                tmpRows(0).Item("Bemerkung") = strBemerkung
                tmpRows(0).EndEdit()
                _mTblFahrzeuge.AcceptChanges()
            End If
        Next

    End Sub

#End Region
End Class