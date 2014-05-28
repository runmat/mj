Option Explicit On 
Option Strict On

Imports CKG.Base.Common
Imports CKG
Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
'Imports Microsoft.Data.SAPClient

Public Class AbgemeldeteFahrzeuge
    Inherits Base.Business.DatenimportBase

    Private mDatBis As String
    Private mDatVon As String

    Public Property DatumVon() As String
        Get
            Return mDatVon
        End Get
        Set(ByVal value As String)
            mDatVon = value
        End Set
    End Property

    Public Property DatumBis() As String
        Get
            Return mDatBis
        End Get
        Set(ByVal value As String)
            mDatBis = value
        End Set
    End Property



    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)
        '----------------------------------------------------------------------
        ' Methode: callBapiForAdressen
        ' Autor: JJU
        ' Beschreibung: fragt die Versandadressen im SAP ab
        ' Erstellt am: 12.02.2009
        ' ITA: 2596
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABM_ABGEMELDETE_010", m_objApp, m_objUser, Page)

            'myProxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("PICKDATAB", mDatVon)
            'myProxy.setImportParameter("PICKDATBI", mDatBis)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_ABM_ABGEMELDETE_010", "KUNNR,PICKDATAB,PICKDATBI", Right("0000000000" & m_objUser.KUNNR, 10), mDatVon, mDatBis)

            CreateOutPut(S.AP.GetExportTable("AUSGABE"), m_strAppID) 'myProxy.getExportTable("AUSGABE"), m_strAppID)

        Catch ex As Exception
            m_intStatus = -9999
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Fahrzeuge vorhanden. "
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try

    End Sub
End Class

' ************************************************
' $History: AbgemeldeteFahrzeuge.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 13.02.09   Time: 13:51
' Created in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2596 / 2589
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 11.11.08   Time: 10:14
' Created in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2370,2367 (Nachbesserungen)
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 11.11.08   Time: 8:52
' Created in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2372 + 2367 (Nachbesserung)
' 
' ************************************************
