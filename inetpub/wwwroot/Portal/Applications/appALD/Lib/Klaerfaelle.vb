Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class Klaerfaelle
    REM § Status-Report, Kunde: ALD, BAPI: Z_Bapi_Dad_Klaerfaelle,
    REM § Ausgabetabelle per individueller Erzeugung.

    Inherits ReportBase

#Region " Declarations"
    Private m_datVonDatum As Date
    Private m_datBisDatum As Date
    Private m_blnAll As Boolean
#End Region

#Region " Properties"

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        FILL(m_datBisDatum, m_datVonDatum, m_strAppID, m_strSessionID, m_blnAll)
    End Sub

    Public Overloads Sub FILL(ByVal datBisDatum As Date, ByVal datVonDatum As Date, ByVal strAppID As String, ByVal strSessionID As String, ByVal blnAll As Boolean)
        m_strClassAndMethod = "Klaerfaelle.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ALD.SAPProxy_ALD()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1

            Try
                Dim SAPTable As New SAPProxy_ALD.ZKLAERFALLTable()

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_Bapi_Dad_Klaerfaelle", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                If blnAll = True Then
                    objSAP.Z_Bapi_Dad_Klaerfaelle(MakeDateSAP(datBisDatum), Right("0000000000" & m_objUser.KUNNR, 10), MakeDateSAP(datVonDatum), "", SAPTable)
                Else
                    objSAP.Z_Bapi_Dad_Klaerfaelle(MakeDateSAP(datBisDatum), Right("0000000000" & m_objUser.KUNNR, 10), MakeDateSAP(datVonDatum), "MAIL", SAPTable)
                End If
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, True)
                End If

                m_tblResult = SAPTable.ToADODataTable

                m_tblResult.Columns.Remove("AUART")
                m_tblResult.Columns.Remove("VBELN")

                Dim datatablerow As DataRow
                For Each datatablerow In m_tblResult.Rows
                    datatablerow.Item("Text") = Replace(datatablerow.Item("Text").ToString, "<(>&<)>", "und")
                Next
                Dim tbTranslation As New DataTable()
                tbTranslation = m_objApp.ColumnTranslation(strAppID)
                For Each datatablerow In tbTranslation.Rows
                    Dim datatablecolumn As DataColumn
                    For Each datatablecolumn In m_tblResult.Columns
                        If datatablecolumn.ColumnName.ToUpper = datatablerow("OrgName").ToString.ToUpper Then
                            datatablecolumn.ColumnName = datatablerow("NewName").ToString
                        End If
                    Next
                Next
                WriteLogEntry(True, "VONDATUM=" & datVonDatum.ToShortDateString & ", BISDATUM=" & datBisDatum.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = ex.Message
                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "VONDATUM=" & datVonDatum.ToShortDateString & ", BISDATUM=" & datBisDatum.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", " & m_strMessage, m_tblResult, False)
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
' $History: Klaerfaelle.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 17.04.08   Time: 15:15
' Created in $/CKAG/Applications/appald/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 13.03.07   Time: 15:07
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Lib
' Inherits-Eintrag angepasst
' 
' *****************  Version 3  *****************
' User: Uha          Date: 5.03.07    Time: 16:59
' Updated in $/CKG/Applications/AppALD/AppALDWeb/Lib
' 
' ************************************************
