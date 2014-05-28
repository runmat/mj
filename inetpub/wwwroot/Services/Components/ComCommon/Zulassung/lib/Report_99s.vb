Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common


Public Class Report_99s
    REM § Status-Report, Kunde: Übergreifend, BAPI: Z_M_Zgbs_Ben_Zulassungsunt,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private strKennzeichen As String
#End Region

#Region " Properties"
    Property PKennzeichen() As String
        Get
            Return strKennzeichen
        End Get
        Set(ByVal Value As String)
            strKennzeichen = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal page As Web.UI.Page, ByVal strAppID As String, ByVal strSessionID As String)

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ZGBS_BEN_ZULASSUNGSUNT", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_ZKBA1", "")
                myProxy.setImportParameter("I_ZKBA2", "")
                myProxy.setImportParameter("I_ZKFZKZ", strKennzeichen)
                myProxy.setImportParameter("I_AUSWAHL", "A")

                myProxy.callBapi()

                m_tblResult = myProxy.getExportTable("GT_WEB")


            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = -5555
                        m_strMessage = "Keine Daten gefunden."
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
' $History: Report_99s.vb $
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 19.01.11   Time: 14:14
' Created in $/CKAG2/Services/Components/ComCommon/Zulassung/lib
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 9.03.10    Time: 10:34
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
' *****************  Version 8  *****************
' User: Rudolpho     Date: 23.01.08   Time: 13:08
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' ITA: 1371
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 18.12.07   Time: 16:54
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' 
' *****************  Version 6  *****************
' User: Uha          Date: 11.07.07   Time: 15:11
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' Nur Codepflege
' 
' *****************  Version 5  *****************
' User: Uha          Date: 3.07.07    Time: 9:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 17:00
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' 
' ************************************************