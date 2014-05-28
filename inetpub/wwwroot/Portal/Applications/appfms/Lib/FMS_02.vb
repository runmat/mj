Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class FMS_02
    REM § Status-Report, Kunde: FMS, BAPI: Z_M_VERSBRIEFE,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datAbmeldedatumVon As DateTime
    Private m_datAbmeldedatumBis As DateTime
    Private m_strABC_KZ As String
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
        m_strClassAndMethod = "FMS_02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                S.AP.Init("Z_M_Versbriefe")

                S.AP.SetImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_DATVON", m_datAbmeldedatumVon.ToShortDateString)
                S.AP.SetImportParameter("I_DATBIS", m_datAbmeldedatumBis.ToShortDateString)
                S.AP.SetImportParameter("I_VERSART", m_strABC_KZ)

                S.AP.Execute()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("T_RETURN")
                tblTemp2.Columns.Add("ADRESSE")
                If tblTemp2.Rows.Count > 0 Then
                    Dim row As DataRow
                    For Each row In tblTemp2.Rows
                        Dim strTemp As String = CStr(row("NAME1")) & ", " & CStr(row("NAME2")) & ", " & CStr(row("STRAS")) & " " & CStr(row("HNR")) & ", " & CStr(row("PLZ")) & " " & CStr(row("ORT"))
                        row("ADRESSE") = Replace(strTemp, ", ,  ,  ", "")
                        If CStr(row("VERSART")) = "1" Then
                            row("VERSART") = "Temporär"
                        Else
                            row("VERSART") = "Endgültig"
                        End If
                    Next
                End If

                CreateOutPut(tblTemp2, strAppID)
            Catch ex As Exception

                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "ERR_NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Fahrzeuge im vorgegebenen Zeitraum gefunden."
                    Case Else
                        m_intStatus = -9999
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
' $History: FMS_02.vb $
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 9.03.10    Time: 17:57
' Updated in $/CKAG/Applications/appfms/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 15:58
' Updated in $/CKAG/Applications/appfms/Lib
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:44
' Created in $/CKAG/Applications/appfms/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 17:52
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 13:11
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' 
' ******************************************
