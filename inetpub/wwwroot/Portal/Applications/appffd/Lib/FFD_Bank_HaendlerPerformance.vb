Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Public Class FDD_Bank_HaendlerPerformance
    REM § Status-Report, Kunde: FFD, BAPI: Z_M_Haendler_Performance,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Dim gJahr As String
#End Region

#Region " Properties"
    Property Geschaeftsjahr() As String
        Get
            Return gJahr
        End Get
        Set(ByVal Value As String)
            gJahr = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        Try

            m_intStatus = 0
            m_strMessage = ""
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Haendler_Performance", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_ZZKUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_ZZKUNNR_ZF_VON", "")
            'myProxy.setImportParameter("I_ZZKUNNR_ZF_BIS", "")
            'myProxy.setImportParameter("I_GJAHR", gJahr)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Haendler_Performance", "I_ZZKUNNR_AG,I_ZZKUNNR_ZF_VON,I_ZZKUNNR_ZF_BIS,I_GJAHR", Right("0000000000" & m_objUser.KUNNR, 10), "", "", gJahr)

            m_tableResult = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            Dim row As DataRow

            For Each row In m_tableResult.Rows
                If CType(row("FLAG"), String) = String.Empty Then
                    row("MONAT") = "01-" & CType(row("MONAT"), String)
                End If
            Next

            With m_tableResult
                .Columns.Remove("ZZBEZAHLT")
                .Columns.Remove("ZZKUNNR_AG")
                .Columns.Remove("FLAG")
                .Columns.Remove("GESAMTANZ")

                .Columns("ZZKUNNR_ZF").ColumnName = "Haendler"
                .Columns("GJAHR").ColumnName = "Jahr"
                .Columns("ANZ01").ColumnName = "Anz.Versendungen"
                .Columns("ZZKKBER").ColumnName = "Versandart"
                .Columns("DUR01").ColumnName = "Durchschn.Zeitraum"
                .Columns("ANZ_UNBEZ").ColumnName = "Anz.Unbezahlt"
                .Columns("DIF01").ColumnName = "Faelligkeit"
                .Columns("PROZ_UNBEZ").ColumnName = "Proz.Unbezahlt"
                .Columns("BEZ01").ColumnName = "Bez_01Tag"
                .Columns("BEZ02").ColumnName = "Bez_02Tag"
                .Columns("BEZ03").ColumnName = "Bez_03Tag"
                .Columns("BEZ04").ColumnName = "Bez_04Tag"
                .Columns("BEZ05").ColumnName = "Bez_05Tag"
                .Columns("BEZ06").ColumnName = "Bez_06Tag"
                .Columns("BEZ07").ColumnName = "Bez_07Tag"
                .Columns("BEZ08").ColumnName = "Bez_08Tag"
                .Columns("BEZ09").ColumnName = "Bez_09Tag"
                .Columns("BEZ10").ColumnName = "Bez_10Tag"
                .Columns("BEZ11").ColumnName = "Bez_11Tag"

                .Columns("PRZ01").ColumnName = "Proz_01"
                .Columns("PRZ02").ColumnName = "Proz_02"
                .Columns("PRZ03").ColumnName = "Proz_03"
                .Columns("PRZ04").ColumnName = "Proz_04"
                .Columns("PRZ05").ColumnName = "Proz_05"
                .Columns("PRZ06").ColumnName = "Proz_06"
                .Columns("PRZ07").ColumnName = "Proz_07"
                .Columns("PRZ08").ColumnName = "Proz_08"
                .Columns("PRZ09").ColumnName = "Proz_09"
                .Columns("PRZ10").ColumnName = "Proz_10"
                .Columns("PRZ11").ColumnName = "Proz_11"
                .Columns("KNRZE").ColumnName = "Filiale"
            End With


            For Each row In m_tableResult.Rows
                row("Haendler") = Right(row("Haendler").ToString, 5)
            Next
            m_tableResult.AcceptChanges()

            WriteLogEntry(True, "GJAHR=" & gJahr & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            WriteLogEntry(False, "GJAHR=" & gJahr & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        End Try
    End Sub

#End Region

End Class

' ************************************************
' $History: FFD_Bank_HaendlerPerformance.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.06.09   Time: 11:35
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.03.07    Time: 12:51
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' ************************************************
