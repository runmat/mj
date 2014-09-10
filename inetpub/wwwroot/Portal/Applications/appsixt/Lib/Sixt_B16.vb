Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Common

'---------------------------
'Zugriff auf BAPI Z_M_BRIEFEINGANG_MELDUNG
'---------------------------
Public Class Sixt_B16
    Inherits Base.Business.DatenimportBase

#Region " Declarations"

    Private m_chassisNummern As DataTable

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String, ByVal chassisNummern As DataTable)
        MyBase.New(objUser, objApp, strFilename)
        Me.m_chassisNummern = chassisNummern
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strClassAndMethod = "Sixt_B16.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)

            Try

                'Importparameter erstellen

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BRIEFEINGANG_MELDUNG", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                S.AP.Init("Z_M_BRIEFEINGANG_MELDUNG", "I_KUNNR", strKUNNR)

                Dim chassisTable As DataTable
                'chassisTable = myProxy.getImportTable("IT_CHASSIS_NUM")
                chassisTable = S.AP.GetImportTable("IT_CHASSIS_NUM")

                Dim nr As DataRow
                For Each nr In Me.m_chassisNummern.Rows
                    Dim row As DataRow = chassisTable.NewRow
                    row("ZZFAHRG") = nr(0).ToString()
                    chassisTable.Rows.Add(row)
                Next

                'myProxy.callBapi()

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")

                Dim tblTemp2 As DataTable = S.AP.GetExportTableWithExecute("GT_WEB")

                CreateOutPut(tblTemp2, strAppID)
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Sixt_B16.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 9.03.09    Time: 14:10
' Updated in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 2  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 1  *****************
' User: Uha          Date: 15.05.07   Time: 17:40
' Created in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Änderungen aus StartApplication vom 11.05.2007
' 
' ************************************************
