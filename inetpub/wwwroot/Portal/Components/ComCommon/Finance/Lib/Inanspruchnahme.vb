Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class Inanspruchnahme
    Inherits Base.Business.BankBase

#Region " Declarations"
    Private mEquis As DataTable
    Private m_strHaendler As String
    Private m_strEQUNR As String
    Private m_strVBELN As String
    Private m_strStornoHaendler As String
    Private m_strFahrgestellnummer As String
    Private mAktionCodes As DataTable
    Private mGTWeb As DataTable

#End Region

#Region " Properties"
    Public ReadOnly Property Equipments() As DataTable
        Get
            Return mEquis
        End Get
    End Property

    Public ReadOnly Property GTWeb() As DataTable
        Get
            Return mGTWeb
        End Get
    End Property


    Public Property Haendler() As String
        Get
            Return m_strHaendler
        End Get
        Set(ByVal Value As String)
            m_strHaendler = Value
        End Set
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return m_strFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummer = Value
        End Set
    End Property

    Public ReadOnly Property AktionCodes() As DataView
        Get
            If mAktionCodes Is Nothing Then
                mAktionCodes = New DataTable
                mAktionCodes.Columns.Add("Text")
                mAktionCodes.Columns.Add("Wert")
                mAktionCodes.AcceptChanges()
                Dim bla2() As Object = {"bezahlt ohne Lastschrift", "BOL"}
                Dim bla1() As Object = {"- keine Auswahl -", ""}
                Dim bla3() As Object = {"bezahlt mit Lastschrift", "BML"}
                Dim bla4() As Object = {"Umwandlung in Vorführwagen", "VFW"}
                mAktionCodes.Rows.Add(bla1)
                mAktionCodes.Rows.Add(bla2)
                mAktionCodes.Rows.Add(bla3)
                mAktionCodes.Rows.Add(bla4)
            End If

            Return mAktionCodes.DefaultView
        End Get
    End Property

    Public Property StornoHaendler() As String
        Get
            Return m_strStornoHaendler
        End Get
        Set(ByVal Value As String)
            m_strStornoHaendler = Value
        End Set
    End Property

    Public Property EQUNR() As String
        Get
            Return m_strEQUNR
        End Get
        Set(ByVal Value As String)
            m_strEQUNR = Value
        End Set
    End Property


    Public Property VBELN() As String
        Get
            Return m_strVBELN
        End Get
        Set(ByVal Value As String)
            m_strVBELN = Value
        End Set
    End Property


#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
     
    End Sub

    Public Overloads Overrides Sub Show()

        m_strClassAndMethod = "Inanspruchnahme.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intStatus = 0

            Try
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                S.AP.InitExecute("Z_M_GET_INANSPRUCHNAHME", "I_AG, I_HAENDLER",
                                 Right("0000000000" & m_objUser.KUNNR, 10), m_strHaendler)

                Dim tblGTWEB As DataTable = S.AP.GetExportTable("GT_WEB")

                'auswerten der exportparameter
                If tblGTWEB IsNot Nothing Then
                    mEquis = tblGTWEB
                    mEquis.Columns.Add("Aktion", GetType(String))
                    HelpProcedures.killAllDBNullValuesInDataTable(mEquis)

                    For Each tmpRow As DataRow In mEquis.Rows
                        With tmpRow
                            Select Case .Item("ZZKKBER").ToString
                                Case "0001"
                                    .Item("ZZKKBER") = "temporär"
                                Case "0002"
                                    .Item("ZZKKBER") = "endgültig"
                                Case Else
                                    .Item("ZZKKBER") = "unbekannt"
                            End Select
                        End With
                    Next

                    mEquis = CreateOutPut(mEquis, AppID)
                End If

            Catch ex As Exception
                mEquis = Nothing
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
        
    End Sub

    Public Overrides Sub Change()
        m_strClassAndMethod = "Inanspruchnahme.Change"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intStatus = 0

            If mGTWeb IsNot Nothing Then
                mGTWeb.Clear()
            End If

            Try
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                S.AP.Init("Z_M_SET_ACTION", "I_AG, I_HAENDLER",
                          Right("0000000000" & m_objUser.KUNNR, 10), m_strHaendler)

                mGTWeb = S.AP.GetImportTable("GT_WEB")

                Dim tmpRows() As DataRow = mEquis.Select("Aktion<>''")
                For Each tmpRow As DataRow In tmpRows
                    Dim tmpNewRow As DataRow = mGTWeb.NewRow

                    tmpNewRow.Item("EQUNR") = tmpRow.Item("EQUNR").ToString
                    tmpNewRow.Item("ERNAM") = Left(m_objUser.UserName, 12)
                    tmpNewRow.Item("ACTION") = tmpRow.Item("Aktion").ToString

                    mGTWeb.Rows.Add(tmpNewRow)
                    mGTWeb.AcceptChanges()
                Next

                S.AP.Execute()

                mGTWeb = S.AP.GetExportTable("GT_WEB")

                HelpProcedures.killAllDBNullValuesInDataTable(mGTWeb)

            Catch ex As Exception
                mEquis = Nothing
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Inanspruchnahme.vb $
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 16.07.09   Time: 17:04
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' bugfix ernam Länge Inanspruchnahme
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 22.06.09   Time: 17:12
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 Z_M_LAND_PLZ_001
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 24.10.08   Time: 14:25
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2246 Ok-Code anpassung
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 23.10.08   Time: 14:19
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' Problem bei übertragung ins sap 
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 10.10.08   Time: 11:48
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2246 unfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 9.10.08    Time: 14:51
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2246 unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 8.10.08    Time: 10:18
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2246 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 7.10.08    Time: 16:44
' Created in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2246 Torso
' 
' ************************************************