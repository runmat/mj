Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class AppDokVerw_04
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_datAbmeldedatumVon As DateTime
    Private m_datAbmeldedatumBis As DateTime
    Private m_strABC_KZ As String
    Private configurationAppSettings As System.Configuration.AppSettingsReader
#End Region

#Region " Properties"
    Public Property AbmeldedatumVon() As DateTime
        Get
            Return m_datAbmeldedatumVon
        End Get
        Set(ByVal Value As DateTime)
            m_datAbmeldedatumVon = Value
        End Set
    End Property

    Public Property AbmeldedatumBis() As DateTime
        Get
            Return m_datAbmeldedatumBis
        End Get
        Set(ByVal Value As DateTime)
            m_datAbmeldedatumBis = Value
        End Set
    End Property

    Public Property ABC_KZ() As String
        Get
            Return m_strABC_KZ
        End Get
        Set(ByVal Value As String)
            m_strABC_KZ = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "ALD_02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            configurationAppSettings = New System.Configuration.AppSettingsReader()

            Try
                S.AP.InitExecute("Z_M_Versbriefe", "I_AG, I_DATVON, I_DATBIS, I_VERSART",
                                 Right("0000000000" & m_objUser.KUNNR, 10), m_datAbmeldedatumVon, m_datAbmeldedatumBis, m_strABC_KZ)

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("T_RETURN")

                tblTemp2.Columns.Add("ADRESSE")
                If tblTemp2.Rows.Count > 0 Then
                    Dim row As DataRow
                    For Each row In tblTemp2.Rows
                        Dim strTemp As String = CStr(row("NAME1")) & ", " & row("NAME2").ToString & ", " & CStr(row("STRAS")) & " " & row("HNR").ToString & ", " & CStr(row("PLZ")) & " " & CStr(row("ORT"))
                        row("ADRESSE") = Replace(strTemp, ", ,  ,  ", "")
                        If CStr(row("VERSART")) = "1" Then
                            row("VERSART") = "Temporär"
                        Else
                            row("VERSART") = "Endgültig"
                        End If
                    Next
                End If
                tblTemp2.Columns.Remove("NAME1")
                tblTemp2.Columns.Remove("NAME2")
                tblTemp2.Columns.Remove("STRAS")
                tblTemp2.Columns.Remove("HNR")
                tblTemp2.Columns.Remove("PLZ")
                tblTemp2.Columns.Remove("Ort")
                ResultTable = tblTemp2
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "ERR_NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Fahrzeuge im vorgegebenen Zeitraum gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region

End Class

' ************************************************
' $History: AppDokVerw_04.vb $
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 15.07.08   Time: 14:20
' Created in $/CKAG/Components/ComCommon/AppDokVerw/Lib
' ITA: 2081
' 
' ************************************************
