Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class LT_11
    REM § Status-Report, Kunde: LeaseTrend, BAPI: Z_M_Versendete_Equipments,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"

    Private m_datErfassungsdatumVon As String
    Private m_datErfassungsdatumBis As String
    Private m_strLvNr As String

#End Region

#Region " Properties"

    Public Property PErfassungsDatumVon() As String
        Get
            Return m_datErfassungsdatumVon
        End Get
        Set(ByVal Value As String)
            m_datErfassungsdatumVon = Value
        End Set
    End Property

    Public Property PErfassungsDatumBis() As String
        Get
            Return m_datErfassungsdatumBis
        End Get
        Set(ByVal Value As String)
            m_datErfassungsdatumBis = Value
        End Set
    End Property

    Public Property PLVnr() As String
        Get
            Return m_strLvNr
        End Get
        Set(ByVal Value As String)
            m_strLvNr = Value
        End Set
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "LT_11.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            S.AP.Init("Z_M_Versendete_Zb2_Temp_Lt")

            S.AP.SetImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            S.AP.SetImportParameter("I_KONZS_ZK", m_objUser.Reference)
            S.AP.SetImportParameter("I_LIZNR", m_strLvNr)
            S.AP.SetImportParameter("I_ZZTMPDT_BIS", m_datErfassungsdatumBis)
            S.AP.SetImportParameter("I_ZZTMPDT_VON", m_datErfassungsdatumVon)

            S.AP.Execute()

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")

            CreateOutPut(tblTemp2, strAppID)
            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & m_datErfassungsdatumVon & ", ERDAT_HIGH=" & m_datErfassungsdatumBis, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", ERDAT_LOW=" & m_datErfassungsdatumVon & ", ERDAT_HIGH=" & m_datErfassungsdatumBis & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

#End Region

End Class

' ************************************************
' $History: LT_11.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 30.06.09   Time: 14:58
' Updated in $/CKAG/Applications/AppLeaseTrend/Lib
' ITA 2918  Z_M_FEHLENDE_VERTRAGSNUMMERN, Z_M_BRIEFLEBENSLAUF_LT,
' Z_M_BRIEFERSTEINGANG_LEASETR, Z_M_BRIEFANFORDERUNG_ALLG,
' Z_M_UNANGEFORDERT_L, Z_M_KUNDENDATEN_LT, Z_M_VERSENDETE_ZB2_ENDG_LT,
' Z_M_VERSENDETE_ZB2_TEMP_LT, Z_M_BRIEF_TEMP_VERS_MAHN_002,
' Z_M_DATEN_OHNE_ZB2_LT
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 15:07
' Created in $/CKAG/Applications/AppLeaseTrend/Lib
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 18:14
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 8.03.07    Time: 10:46
' Updated in $/CKG/Applications/AppLeaseTrend/AppLeaseTrendWeb/Lib
' 
' ************************************************
