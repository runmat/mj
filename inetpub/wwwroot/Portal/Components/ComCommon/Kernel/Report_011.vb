Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Public Class Report_011
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Briefeohnefzg2 (Fill),
    REM § BAPI: Z_M_Brieflebenslauf (FillHistory),
    REM § Ausgabetabelle per Zuordnung in Web-DB (Fill), Direkte Rückgabe (FillHistory).
    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_tblHistory As DataTable
    Private m_tblResultModelle As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property ResultModelle() As DataTable
        Get
            Return m_tblResultModelle
        End Get
    End Property

    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Sub FillHistory(ByRef page As Web.UI.Page, ByVal strAppID As String, ByVal strSessionID As String, ByVal strAmtlKennzeichen As String, ByVal strFahrgestellnummer As String, ByVal strBriefnummer As String, ByVal strOrdernummer As String)
        m_strClassAndMethod = "Report_011.FillHistory"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                S.AP.Init("Z_M_Brieflebenslauf")
                S.AP.SetImportParameter("I_VKORG", "1510")
                S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_ZZKENN", UCase(strAmtlKennzeichen))
                S.AP.SetImportParameter("I_ZZFAHRG", UCase(strFahrgestellnummer))
                S.AP.SetImportParameter("I_ZZBRIEF", UCase(strBriefnummer))
                S.AP.SetImportParameter("I_ZZREF1", UCase(strOrdernummer))
                S.AP.Execute()

                Dim SAPTable As DataTable = S.AP.GetExportTable("GT_WEB")

                m_tblHistory = SAPTable.Copy

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten vorhanden."
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case Else
                        m_strMessage = ex.Message
                End Select
            Finally
                m_blnGestartet = False
            End Try

        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Report_011.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 8.03.10    Time: 14:22
' Updated in $/CKAG/Components/ComCommon/Kernel
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Kernel
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Kernel
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 5.10.07    Time: 10:46
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' 
