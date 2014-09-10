Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class Sixt_B13
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Schluesselweb,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datErfassungsdatumVon As DateTime
    Private m_datErfassungsdatumBis As DateTime
    Private m_tblHistory As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_strClassAndMethod = "Sixt_B13.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABM_FEHLENDE_UNTERL_001", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_ABM_FEHLENDE_UNTERL_001", "KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'Dim tblTemp2 As DataTable = myProxy.getExportTable("AUSGABE")
                Dim tblTemp2 As DataTable = S.AP.GetExportTable("AUSGABE")

                Dim tblTemp As DataTable
                tblTemp = tblTemp2.Clone
                tblTemp.Columns.Remove("TREUH")
                tblTemp.Columns.Remove("DATVERS")

                tblTemp.Columns.Add("TREUH", GetType(System.String))
                tblTemp.Columns.Add("DATVERS", GetType(System.String))

                Dim rowTemp As DataRow
                For Each rowTemp In tblTemp2.Rows
                    Dim NewRow As DataRow = tblTemp.NewRow
                    For i As Integer = 0 To tblTemp2.Columns.Count - 1
                        NewRow(i) = rowTemp(i)
                    Next
                    tblTemp.Rows.Add(NewRow)
                Next
                For Each rowTemp In tblTemp.Rows
                    If Not rowTemp("TREUH").ToString.Trim(" "c).Length = 0 Then
                        rowTemp("TREUH") = "X"
                    End If
                    If Not rowTemp("DATVERS").ToString.Trim(" "c).Length = 0 Then
                        rowTemp("DATVERS") = "X"
                    End If
                Next
                CreateOutPut(tblTemp, strAppID)
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
' $History: Sixt_B13.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
