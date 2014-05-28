Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel

Public Class Arval_06
    REM § Status-Report, Kunde: Arval, BAPI: Z_M_Versendete_Equipments,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal kennz As String, ByVal lvnr As String)
        m_strClassAndMethod = "Arval_06.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True
            
            Try
                S.AP.InitExecute("Z_M_Versendete_Equipments", "I_KONZS,I_VKORG,I_LICENSE_NUM,I_LIZNR", Right("0000000000" & m_objUser.KUNNR, 10), "1510", kennz, lvnr)

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Versendete_Equipments", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.KUNNR, 10))
                'myProxy.setImportParameter("I_VKORG", "1510")
                'myProxy.setImportParameter("I_LICENSE_NUM", kennz)
                'myProxy.setImportParameter("I_LIZNR", lvnr)

                'myProxy.callBapi()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")

                Dim rowTemp As DataRow
                tblTemp2.AcceptChanges()
                tblTemp2.Columns("ABCKZ").MaxLength = 5
                Dim strTemp As String

                For Each rowTemp In tblTemp2.Rows
                    Select Case CStr(rowTemp("ABCKZ"))
                        Case "1"
                            strTemp = "TEMP"
                            rowTemp("Name1") = String.Empty
                            rowTemp("City1") = String.Empty
                            rowTemp("Post_Code1") = String.Empty
                            rowTemp("Street") = String.Empty
                            rowTemp("House_Num1") = String.Empty
                        Case "2"
                            strTemp = "ENDG"
                        Case Else
                            strTemp = "-"
                    End Select
                    rowTemp("ABCKZ") = strTemp
                Next
                tblTemp2.AcceptChanges()

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ",KENNZ=" & kennz, m_tblResult, False)
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = -3332
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ",KENNZ=" & kennz & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: Arval_06.vb $
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
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' 
' ************************************************
