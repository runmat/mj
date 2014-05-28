Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel

Namespace Kernel
    Public Class Monatslisten
        REM § Status-Report, Kunde: ALD, BAPI: Z_V_Ueberf_Auftr_Kund_Port,
        REM § Ausgabetabelle per Zuordnung in Web-DB.
        Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
        Private day_first As String
        Private day_last As String
#End Region

#Region " Properties"
        Property day_start() As String
            Get
                Return day_first
            End Get
            Set(ByVal val As String)
                day_first = val
            End Set
        End Property

        Property day_end() As String
            Get
                Return day_last
            End Get
            Set(ByVal val As String)
                day_last = val
            End Set
        End Property
#End Region

#Region " Methods"
        Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
            MyBase.New(objUser, objApp, strFilename)
        End Sub

        Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
            m_strClassAndMethod = "Kernel.Monatslisten.FILL"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    S.AP.Init("Z_V_Ueberf_Auftr_Kund_Port")

                    Dim SAPTable As DataTable = S.AP.GetImportTable("T_SELECT")
                    Dim SAPTableRow As DataRow = SAPTable.NewRow

                    SAPTableRow("VDATU") = day_first
                    SAPTableRow("VDATU_BIS") = day_last
                    SAPTableRow("KUNNR_AG") = Right("0000000000" & m_objUser.KUNNR, 10)
                    SAPTableRow("WBSTK") = "D"

                    SAPTable.Rows.Add(SAPTableRow)   'Zeile hinzufügen

                    S.AP.Execute()

                    Dim table As DataTable = S.AP.GetExportTable("T_AUFTRAEGE")

                    CreateOutPut(table, strAppID)   'Spaltenübersetzungen....
                    'Report-Logeintrag (ok)
                    WriteLogEntry(True, "VDATU=" & day_first & ", VDATU_BIS=" & day_last & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
                Catch ex As Exception

                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

                    WriteLogEntry(False, "VDATU=" & day_first & ", VDATU_BIS=" & day_last & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub
#End Region

    End Class
End Namespace

' ************************************************
' $History: Monatslisten.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Kernel
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Kernel
' 
' *****************  Version 4  *****************
' User: Uha          Date: 3.07.07    Time: 9:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 1.03.07    Time: 17:00
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' 
' ************************************************