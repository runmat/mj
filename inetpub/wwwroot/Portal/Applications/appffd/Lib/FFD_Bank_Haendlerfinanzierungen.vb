Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class FFD_Bank_Haendlerfinanzierungen
    REM § Status-Report, Kunde: FFD, BAPI: Z_M_HaendlerFinanzierung,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Protected m_strFahrzeugtyp As String
#End Region

#Region " Properties"
    Public Property Fahrzeugtyp() As String
        Get
            Return m_strFahrzeugtyp
        End Get
        Set(ByVal Value As String)
            m_strFahrzeugtyp = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFahrzeugtyp As String, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_strFahrzeugtyp = strFahrzeugtyp
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        Dim intID As Int32 = -1
        Dim strKONZS As String = "0000324562"


        Try

            m_intStatus = 0
            m_strMessage = ""
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Haendlerfinanzierung", m_objApp, m_objUser, page)

            Dim strNeuwagen As String = " "
            Dim strVorfuehrwagen As String = " "

            If m_strFahrzeugtyp = "Neuwagen" Then
                strNeuwagen = "X"
            Else
                strVorfuehrwagen = "X"
            End If

            'myProxy.setImportParameter("I_KONZS", strKONZS)
            'myProxy.setImportParameter("I_NEU", strNeuwagen)
            'myProxy.setImportParameter("I_VOR", strVorfuehrwagen)
            'myProxy.setImportParameter("I_DIREKT", " ")
            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Haendlerfinanzierung", "I_KONZS,I_NEU,I_VOR,I_DIREKT", strKONZS, strNeuwagen, strVorfuehrwagen, " ")

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            CreateOutPut(tblTemp2, strAppID)
            Dim column As DataColumn
            Dim blnMitHaendlern As Boolean = False

            For Each column In m_tblResult.Columns
                If column.ColumnName = "Händler" Then
                    blnMitHaendlern = True
                End If
            Next

            If blnMitHaendlern Then
                Dim row As DataRow
                For Each row In m_tblResult.Rows
                    row("Händler") = Right(CStr(row("Händler")), Len(CStr(row("Händler"))) - 2)
                Next
            End If

            Dim tmpRow As DataRow

            For Each tmpRow In m_tblResult.Rows
                tmpRow("Distrikt") = getDistriktBezeichnung(CStr(tmpRow("Distrikt")))
            Next

            WriteLogEntry(True, "Fahrzeugtyp=" & m_strFahrzeugtyp, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -9999
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten gefunden."
                Case "PARAMETER"
                    m_strMessage = "Es können nicht Neu- und Vorführwagen gleichzeitig ausgewählt werden."
                Case "NO_PARAMETER"
                    m_strMessage = "Es ist weder Neu- noch Vorführwagen ausgewählt worden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "Fahrzeugtyp=" & m_strFahrzeugtyp & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Private Function getDistriktBezeichnung(ByVal nr As String) As String

        Dim dt As New DataTable()
        Dim strTemp As String = "0"
        Dim DistrictID As Integer = 0

        Dim da As New SqlClient.SqlDataAdapter( _
          "SELECT OrganizationName FROM ORGANIZATION" & _
          " WHERE CustomerID='" & m_objUser.Customer.CustomerId & "'" & _
          " AND OrganizationReference='" & nr & "'", _
          m_objApp.Connectionstring)

        da.Fill(dt)

        If Not dt.Rows.Count = 1 Then
            Throw (New Exception("Es konnte keine eindeutiger Distriktname für die ID " & nr & " ermittelt werden"))
        Else
            Return CStr(dt.Rows(0)(0))
        End If

    End Function

#End Region

End Class

' ************************************************
' $History: FFD_Bank_Haendlerfinanzierungen.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 16.06.09   Time: 11:35
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2837
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 19.05.08   Time: 11:21
' Updated in $/CKAG/Applications/appffd/Lib
' ITA 1907
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' ************************************************
