Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class CSC_InaktiveVertraege
    REM § Status-Report, Kunde: CNC, BAPI: ZZ_CSC_INAKTIVE_VERTRAEGE,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_strFahrgestellnummer As String
    Private m_strKontonummer As String
    Private m_strKennzeichen As String
    Private m_strKunnr As String
#End Region

#Region " Properties"

#End Region

#Region " Methods"

    Public Sub New(ByVal strKunnr As String, ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFahrgestellnummer As String, ByVal strKontonummer As String, ByVal strKennzeichen As String, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_strFahrgestellnummer = strFahrgestellnummer
        m_strKontonummer = strKontonummer
        m_strKennzeichen = strKennzeichen
        m_strKunnr = strKunnr
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "CSC_InaktiveVertraege.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try
                S.AP.Init("Zz_Csc_Inaktive_Vertraege")

                S.AP.SetImportParameter("I_KONZS", strKUNNR)
                S.AP.SetImportParameter("I_CHASSIS_NUM", m_strFahrgestellnummer)
                S.AP.SetImportParameter("I_LICENSE_NUM", m_strKennzeichen)
                S.AP.SetImportParameter("I_LIZNR", m_strKontonummer)
                S.AP.SetImportParameter("I_VKORG", "1510")

                S.AP.Execute()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")

                CreateOutPut(tblTemp2, strAppID)

                Dim rowTemp As DataRow
                For Each rowTemp In m_tblResult.Rows
                    Select Case CStr(rowTemp("Versendet"))
                        Case "9999"
                            rowTemp("Versendet") = "X"
                        Case Else
                            rowTemp("Versendet") = ""
                    End Select
                Next

            Catch ex As Exception
                m_intStatus = -1111

                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -2222
                        m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                    Case "NO_WEB"
                        m_intStatus = -2223
                        m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                    Case Else
                        m_intStatus = -2224
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

                End Select

                If ex.Message = "NO_DATA" Or ex.Message = "NO_WEB" Then
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End If

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: CSC_InaktiveVertraege.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.03.10   Time: 13:34
' Updated in $/CKAG/Applications/appcsc/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:37
' Created in $/CKAG/Applications/appcsc/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 16:46
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 9:28
' Updated in $/CKG/Applications/AppCSC/AppCSCWeb/Lib
' 
' ************************************************
