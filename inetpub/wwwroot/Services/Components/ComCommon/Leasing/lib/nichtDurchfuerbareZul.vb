Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Common

Public Class nichtdurchfuerbareZull
    Inherits Base.Business.DatenimportBase

#Region " Declarations"

#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, _
                            ByVal strSessionID As String, _
                            ByVal page As Web.UI.Page)
        m_strClassAndMethod = "nichtdurchfuerbareZull.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_KLAERFAELLE", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR", strKUNNR)

                myProxy.callBapi()

                Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")

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
                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: nichtDurchfuerbareZul.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 12.08.10   Time: 14:20
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing/lib
' ITA 4042 testfertig
' 
'
' ************************************************