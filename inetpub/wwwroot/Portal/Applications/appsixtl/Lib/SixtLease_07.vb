Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class SixtLease_07
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


    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "SixtLease_07.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_KLAERFAELLEVW", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_KLAERFAELLEVW", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")
                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")

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

                    'If tmpString.Trim(" "c).Length > tblTemp2.Columns("TDLINE1").MaxLength Then
                    '    tmpRow("TDLINE1") = Left(tmpString.Trim(" "c), tblTemp2.Columns("TDLINE1").MaxLength)
                    'Else
                    tmpRow("TDLINE1") = tmpString.Trim(" "c)
                    'End If
                Next
                CreateOutPut(tblTemp2, strAppID)
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
' $History: SixtLease_07.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 4.05.09    Time: 10:47
' Updated in $/CKAG/Applications/appsixtl/Lib
' ITA:2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:27
' Created in $/CKAG/Applications/appsixtl/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 3.07.07    Time: 9:34
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 8.03.07    Time: 13:42
' Updated in $/CKG/Applications/AppSIXTL/AppSIXTLWeb/Lib
' 
' ************************************************
