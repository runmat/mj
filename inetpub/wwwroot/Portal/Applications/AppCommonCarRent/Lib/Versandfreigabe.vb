Option Explicit On
Option Strict On

Imports CKG
Imports System
Imports CKG.Base.Common
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class Versandfreigabe
    Inherits Base.Business.BankBase

#Region "Declarations"
    Private m_strEmpfaenger As String
    Private m_tblKunden As DataTable
#End Region

#Region "Properties"
    Public Property Empfaenger() As String
        Get
            Return m_strEmpfaenger
        End Get
        Set(ByVal Value As String)
            m_strEmpfaenger = Value
        End Set
    End Property

    Public Property Kunden() As DataTable
        Get
            Return m_tblKunden
        End Get
        Set(ByVal Value As DataTable)
            m_tblKunden = Value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

        'DataTable anlegen
        Me.m_tblKunden = New DataTable()
        With Me.m_tblKunden.Columns
            .Add("Kundennummer", System.Type.GetType("System.String"))
            .Add("Name", System.Type.GetType("System.String"))
            .Add("Anzahl", System.Type.GetType("System.Int64"))
        End With

        Customer = objUser.KUNNR
    End Sub

#End Region

#Region " Methods"

    Public Overrides Sub Show()

    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, _
                           ByRef page As Page)

        m_strClassAndMethod = "Versandfreigabe.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            m_intIDSAP = -1

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Versandfreigabe_Daten", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_Versandfreigabe_Daten", "I_KUNNR_AG", m_strKUNNR)

                m_tblResult = S.AP.GetExportTable("OT_FAHRZG") 'myProxy.getExportTable("OT_FAHRZG")
                m_tblResult.Columns.Add("Selected", System.Type.GetType("System.Boolean"))
                m_tblResult.Columns.Add("Anlagedatum", System.Type.GetType("System.DateTime"))

                Dim row As DataRow
                For Each row In m_tblResult.Rows
                    row("Selected") = False
                Next
                m_tblResult.AcceptChanges()

                Dim tmpVw As DataView = m_tblResult.DefaultView()
                tmpVw.Sort = "Zzkunnr_Zs,Chassis_Num"

                'DataTable leeren (falls gefüllt)
                Me.m_tblKunden.Clear()

                Dim intCount As Integer = 0
                Dim strKundennummer As String = "XXXXX"
                Dim strName As String = ""
                Dim intLoop As Integer

                For intLoop = 0 To tmpVw.Count - 1
                    If strKundennummer = CStr(tmpVw(intLoop)("Zzkunnr_Zs")) Then
                        intCount += 1
                    Else
                        If intCount > 0 Then
                            'Neue Row hinzufügen
                            row = Me.m_tblKunden.NewRow()
                            row("Kundennummer") = strKundennummer
                            row("Name") = strName
                            row("Anzahl") = intCount
                            m_tblKunden.Rows.Add(row)
                        End If
                        strKundennummer = CStr(tmpVw(intLoop)("Zzkunnr_Zs"))
                        strName = CStr(tmpVw(intLoop)("Name1"))
                        intCount = 1
                    End If
                Next

                If Not strKundennummer = "XXXXX" Then
                    'Neue Row hinzufügen
                    row = Me.m_tblKunden.NewRow()
                    row("Kundennummer") = strKundennummer
                    row("Name") = strName
                    row("Anzahl") = intCount
                    m_tblKunden.Rows.Add(row)
                End If

                m_tblResultExcel = m_tblResult.Copy
                m_tblResultExcel.Columns.Remove("Selected")
                m_tblResultExcel.Columns.Remove("Erdat")
                m_tblResultExcel.Columns.Remove("Zzkunnr_Zs")
                m_tblResultExcel.Columns(0).ColumnName = "Name"
                m_tblResultExcel.Columns(1).ColumnName = "Fahrgestellnummer"
                m_tblResultExcel.Columns(6).ColumnName = "Bezahlkennzeichen"
                m_tblResultExcel.Columns(7).ColumnName = "Versandsperre"
                m_tblResultExcel.Columns.RemoveAt(8)
                m_tblResultExcel.Columns.RemoveAt(5)
                m_tblResultExcel.Columns.RemoveAt(4)
                m_tblResultExcel.Columns.RemoveAt(3)
                m_tblResultExcel.Columns.RemoveAt(2)

                WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c), Nothing)
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -1402
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If

                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ",   " & Replace(m_strMessage, "<br>", " "), Nothing)

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If

            End Try

        End If

    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, _
                                ByRef page As Page)

        m_strClassAndMethod = "Versandfreigabe.Change"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try
            m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Versandfreigabe_Freigabe", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_VERSANDFREIGABE_FREIGABE", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            S.AP.Init("Z_M_VERSANDFREIGABE_FREIGABE", "I_KUNNR_AG", m_strKUNNR)

            Dim sapImportTable As DataTable = S.AP.GetImportTable("IT_FAHRZG") 'myProxy.getImportTable("IT_FAHRZG")

            For intLoop = m_tblResult.Rows.Count - 1 To 0 Step -1
                If Not CBool(m_tblResult.Rows(intLoop)("Selected")) Then
                    m_tblResult.Rows.RemoveAt(intLoop)
                Else
                    m_tblResult.Rows(intLoop)("Zzvsfreigabe") = "X"
                End If
            Next

            If m_tblResult.Rows.Count = 0 Then
                Throw New Exception("Es sind keine Daten zum Speichern vorhanden.")
            End If

            Dim intLoop2 As Integer
            Dim rowSAP As DataRow

            For intLoop = 0 To m_tblResult.Rows.Count - 1
                rowSAP = sapImportTable.NewRow
                For intLoop2 = 0 To m_tblResult.Columns.Count - 3
                    rowSAP.Item(intLoop2) = m_tblResult.Rows(intLoop)(intLoop2)
                Next
                sapImportTable.Rows.Add(rowSAP)
            Next

            'myProxy.callBapi()
            S.AP.Execute()

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
            End If

            m_tblResultExcel = m_tblResult.Copy
            m_tblResultExcel.Columns.Remove("Selected")
            m_tblResultExcel.Columns.Remove("Erdat")
            m_tblResultExcel.Columns.Remove("Zzkunnr_Zs")
            m_tblResultExcel.Columns(0).ColumnName = "Name"
            m_tblResultExcel.Columns(1).ColumnName = "Fahrgestellnummer"
            m_tblResultExcel.Columns(6).ColumnName = "Bezahlkennzeichen"
            m_tblResultExcel.Columns(7).ColumnName = "Versandsperre"
            m_tblResultExcel.Columns.RemoveAt(8)
            m_tblResultExcel.Columns.RemoveAt(5)
            m_tblResultExcel.Columns.RemoveAt(4)
            m_tblResultExcel.Columns.RemoveAt(3)
            m_tblResultExcel.Columns.RemoveAt(2)


        Catch ex As Exception
            m_tblResult = Nothing
            m_intStatus = -9999

            Select Case ex.Message
                Case "UPDATE_ERROR"
                    m_strMessage = "Fehler beim Speichern der Daten."
                Case Else
                    m_strMessage = "Beim der Übertragung ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            If m_intIDSAP > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
            End If
        End Try

    End Sub

#End Region

End Class
' ************************************************
' $History: Versandfreigabe.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:53
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' Warnungen
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.03.09   Time: 14:22
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA: 2671
' 
' ************************************************
