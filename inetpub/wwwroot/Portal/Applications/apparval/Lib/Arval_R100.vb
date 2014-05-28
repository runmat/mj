Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class Arval_R100
    REM § Status-Report, Kunde: Sixt Leasing, BAPI: Z_M_KLAERFAELLEVW,
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

    Public Overloads Sub FILL(ByVal strAppId As String, ByVal strSessionId As String)
        m_strClassAndMethod = "Arval_R100.FILL"
        m_strAppID = strAppId
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True
            
            Try

                S.AP.InitExecute("Z_M_Klaerfaelle", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Klaerfaelle", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'myProxy.callBapi()


                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

                Dim tmpRow As DataRow
                Dim tmpString As String = String.Empty

                For Each tmpRow In tblTemp2.Rows
                    If TypeOf tmpRow("TDLINE1") Is System.String Then
                        tmpString = Trim(CStr(tmpRow("TDLINE1")))
                    End If
                    If TypeOf tmpRow("TDLINE2") Is System.String Then
                        tmpString &= " " & Trim(CStr(tmpRow("TDLINE2")))
                    End If
                    If TypeOf tmpRow("TDLINE3") Is System.String Then
                        tmpString &= " " & Trim(CStr(tmpRow("TDLINE3")))
                    End If
                    If TypeOf tmpRow("TDLINE4") Is System.String Then
                        tmpString &= " " & Trim(CStr(tmpRow("TDLINE4")))
                    End If
                    If TypeOf tmpRow("TDLINE5") Is System.String Then
                        tmpString &= " " & Trim(CStr(tmpRow("TDLINE5")))
                    End If
                    tmpRow("TDLINE1") = tmpString.Trim(" "c)
                Next

                CreateOutPut(tblTemp2, strAppId)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                Select Case ex.Message
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
' $History: Arval_R100.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 30.06.09   Time: 13:03
' Updated in $/CKAG/Applications/apparval/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' 
' ************************************************
