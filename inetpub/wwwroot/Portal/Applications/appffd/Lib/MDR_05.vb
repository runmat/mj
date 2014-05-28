Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class MDR_05
    REM § Status-Report, Kunde: MDR, BAPI: Z_Dad_Cs_Mdr_Briefe_Ohne_Kfz,
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
        m_strClassAndMethod = "MDR_05.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Dad_Cs_Mdr_Briefe_Ohne_Kfz", m_objApp, m_objUser, page)

                Try
                    'myProxy.callBapi()
                    S.AP.InitExecute("Z_Dad_Cs_Mdr_Briefe_Ohne_Kfz")
                Catch ex As Exception
                    Select Case ex.Message.Replace("Execution failed", "").Trim()
                        Case "NO_DATA"
                            'nodata Ignorieren, da noch 2. Bapi hinzukommt
                        Case Else
                            Throw ex
                    End Select
                End Try

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_BRIEF_OHNE_KFZ") 'myProxy.getExportTable("GT_BRIEF_OHNE_KFZ")


                'myProxy = DynSapProxy.getProxy("Z_DAD_CS_MDR_BRIEFE_OHNE_KFZ_2", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & "314582", 10))


                Try
                    'myProxy.callBapi()
                    S.AP.InitExecute("Z_DAD_CS_MDR_BRIEFE_OHNE_KFZ_2", "I_KUNNR", Right("0000000000" & "314582", 10))
                Catch ex As Exception
                    Select Case ex.Message.Replace("Execution failed", "").Trim()
                        Case "NO_DATA"
                            'wenn tbltemp2 auch nicht ist, dann wirkliche nodata ausgeben
                            If tblTemp2 Is Nothing Then
                                Throw ex
                            End If
                        Case Else
                            Throw ex
                    End Select
                End Try

                Dim tblTemp3 As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

                If Not tblTemp2 Is Nothing Then
                    For Each tmpRow As DataRow In tblTemp3.Rows
                        tblTemp2.ImportRow(tmpRow)
                    Next
                Else
                    tblTemp2 = tblTemp3
                End If

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception

                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Fahrzeuge gefunden."
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

#End Region
End Class

' ************************************************
' $History: MDR_05.vb $
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 21.07.09   Time: 10:12
' Updated in $/CKAG/Applications/appffd/Lib
' ITA 2977 Testfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 16.07.09   Time: 15:37
' Updated in $/CKAG/Applications/appffd/Lib
' ITa 2977 nachbesserungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 16.07.09   Time: 15:13
' Updated in $/CKAG/Applications/appffd/Lib
' ITA 2977
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 1  *****************
' User: Uha          Date: 16.08.07   Time: 11:37
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITAs 1162, 1223 und 1161 werden jetzt über Report11.aspx abgewickelt.
' Report14 wieder komplett gelöscht.
' 
' ************************************************
