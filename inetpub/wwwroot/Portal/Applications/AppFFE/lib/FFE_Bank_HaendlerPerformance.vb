Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class FFE_Bank_HaendlerPerformance

    REM § Status-Report, Kunde: FFD, BAPI: Z_M_Haendler_Performance,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

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

    'Public Overloads Overrides Sub Fill()
    '    FILL(m_strAppID, m_strSessionID)
    'End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "FFD_Bank_HaendlerPerformance.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_FFE.ZDADPERFORM_SORTTable() ' .ZDADPERFORMANCETable()

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Haendler_Performance", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Haendler_Performance(gJahr, Right("0000000000" & m_objUser.KUNNR, 10), "", "", SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable
                Dim row As DataRow

                m_tableResult = SAPTable.ToADODataTable
                'Überflüssige Spalten entfernen

                For Each row In m_tableresult.Rows 'Vormonate (Inhalt = "01-X")
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
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Eingabedaten vorhanden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "GJAHR=" & gJahr & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class
' ************************************************
' $History: FFE_Bank_HaendlerPerformance.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugef�gt
' 
' ************************************************
