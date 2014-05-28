Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class ECAN_02
    REM § Status-Report, Kunde: ECAN, BAPI: Z_M_BRIEF_EINGANG,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_datAbmeldedatumVon As DateTime
    Private m_datAbmeldedatumBis As DateTime
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

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    'Public Overloads Overrides Sub Fill()
    '    Fill(m_strAppID, m_strSessionID)
    'End Sub
    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "ECAN_02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1
            Dim strDatTempVon As String = m_datAbmeldedatumVon.ToShortDateString
            Dim strDatTempBis As String = m_datAbmeldedatumBis.ToShortDateString

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BRIEF_EINGANG", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("I_VON_ERDAT", strDatTempVon)
                myProxy.setImportParameter("I_BIS_ERDAT", strDatTempBis)

                myProxy.callBapi()

                Dim tblTemp2 As DataTable = myProxy.getExportTable("OT_BRIEFE")


                tblTemp2.Columns.Add("XXXXX", System.Type.GetType("System.String"))

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Vorgänge im vorgegebenen Zeitraum gefunden."
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
    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
   
    End Sub
#End Region
End Class

' ************************************************
' $History: ECAN_02.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.02.10    Time: 16:49
' Updated in $/CKAG/Applications/appecan/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 10:57
' Updated in $/CKAG/Applications/appecan/Lib
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:07
' Created in $/CKAG/Applications/appecan/Lib
' 
' *****************  Version 2  *****************
' User: Uha          Date: 11.07.07   Time: 12:37
' Updated in $/CKG/Applications/AppECAN/AppECANWeb/Lib
' Bug in Parameterliste von Z_M_Brief_Eingang gefixt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 11.07.07   Time: 11:10
' Created in $/CKG/Applications/AppECAN/AppECANWeb/Lib
' Report "Täglicher Eingang Fahrzeugbriefe" hinzugefügt
' 
' ************************************************
