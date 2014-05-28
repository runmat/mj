Option Explicit On
Option Strict On
Imports CKG
Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Public Class F1_Bank_Haendlerfinanzierungen
    REM § Status-Report, Kunde: FFE, BAPI: Z_M_HaendlerFinanzierung,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Protected m_strFahrzeugtyp As String
#End Region

#Region " Properties"
    Public Property Fahrzeugtyp() As String
        Get
            Return m_strFahrzeugtyp
        End Get
        Set(ByVal Value As String)
            m_strFahrzeugtyp = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFahrzeugtyp As String, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_strFahrzeugtyp = strFahrzeugtyp
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        '----------------------------------------------------------------------
        ' Methode: FILL
        ' Autor: JJU
        ' Beschreibung: ruft das BAPI Z_M_Haendlerfinanzierung auf
        ' Erstellt am: 12.03.2009
        ' ITA: 2685
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try
            Dim strNeuwagen As String = " "
            Dim strVorfuehrwagen As String = " "
            Dim strDirekt As String = " "
            Select Case m_strFahrzeugtyp
                Case "FA11"
                    strNeuwagen = "X"
                Case "FA12"
                    strVorfuehrwagen = "X"
                Case "FA14"
                    strDirekt = "X"
            End Select


            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Haendlerfinanzierung", m_objApp, m_objUser, Page)

            'myProxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_NEU", strNeuwagen)
            'myProxy.setImportParameter("I_VOR", strVorfuehrwagen)
            'myProxy.setImportParameter("I_DIREKT", strDirekt)


            'myProxy.callBapi()

            'CreateOutPut(myProxy.getExportTable("GT_WEB"), strAppID)

            S.AP.InitExecute("Z_M_Haendlerfinanzierung", "I_KONZS,I_NEU,I_VOR,I_DIREKT", Right("0000000000" & m_objUser.KUNNR, 10), strNeuwagen, strVorfuehrwagen, strDirekt)
            CreateOutPut(S.AP.GetExportTable("GT_WEB"), strAppID)

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten gefunden."
                Case "PARAMETER"
                    m_strMessage = "Es können nicht Neu- und Vorführwagen gleichzeitig ausgewählt werden."
                Case "NO_PARAMETER"
                    m_strMessage = "Es ist weder Neu- noch Vorführwagen ausgewählt worden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try

    End Sub

#End Region

End Class
' ************************************************
' $History: F1_Bank_Haendlerfinanzierungen.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 11.03.10   Time: 13:51
' Updated in $/CKAG/Applications/AppF1/lib
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 12.03.09   Time: 14:24
' Created in $/CKAG/Applications/AppF1/lib
' ITA 2685 unfertig
' 
' ************************************************
