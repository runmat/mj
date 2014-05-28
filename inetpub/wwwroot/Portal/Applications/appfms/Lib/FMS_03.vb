Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class FMS_03
    REM § Status-Report, Kunde: FMS, BAPI: Z_M_ABM_FEHLENDE_UNTERL_001,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "FMS_03.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                S.AP.InitExecute("Z_M_ABM_FEHLENDE_UNTERL", "KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("AUSGABE")

                CreateOutPut(tblTemp2, strAppID)

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: FMS_03.vb $
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
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 17:52
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.03.07    Time: 13:11
' Updated in $/CKG/Applications/AppFMS/AppFMSWeb/Lib
' 
' ******************************************
