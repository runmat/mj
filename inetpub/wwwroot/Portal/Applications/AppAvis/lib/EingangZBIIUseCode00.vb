Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

'#################################################################
' Klasse für das Lesen der Avis-Grundaten#
' Reports : EingangZBIIUseCode00 (Report03)
'#################################################################
Public Class EingangZBIIUseCode00
    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    '----------------------------------------------------------------------
    ' Methode: Fill
    ' Autor: J.Jung
    ' Beschreibung: füllen der Ausgabetabelle für Eingang Fahrzeugbriefe (Report03)
    ' Erstellt am: 31.10.2008
    ' ITA: 2330
    '----------------------------------------------------------------------

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String)

        m_strClassAndMethod = "EingangZBIIUseCode00.Fill"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            Dim tblTemp2 As DataTable = S.AP.GetExportTableWithInitExecute("Z_M_READ_GRUNDDAT_004.GT_WEB",
                                                         "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

            CreateOutPut(tblTemp2, m_strAppID)

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Daten vorhanden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Finally
            m_blnGestartet = False
        End Try
    End Sub
#End Region

End Class

' ************************************************
' $History: EingangZBIIUseCode00.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.11.08   Time: 9:35
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2421 testfertig
'
' ************************************************
