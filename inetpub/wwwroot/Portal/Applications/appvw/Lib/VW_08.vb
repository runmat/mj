Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

Public Class VW_08

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_tblRechnungsPruefung As DataTable
    Private m_intResultCount As Integer
    Private m_tblHistory As DataTable
#End Region

#Region " Properties"
    Public ReadOnly Property ResultCount() As Integer
        Get
            Return m_intResultCount
        End Get
    End Property

    Public ReadOnly Property rechnungsPruefungTbl() As DataTable
        Get
            Return m_tblRechnungsPruefung
        End Get
    End Property
#End Region


#Region " Constructor"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub
#End Region

#Region " Methods"

    Public Function fillRechnungsPruefungsGrid(ByRef alSelectionCriteria As ArrayList) As DataTable

        'With alSelectionCriteria
        '    .Add(txtVorhabennummer.Text)   0
        '    .Add(txtIKZNummer.Text)        1
        '    .Add(txtUebergabedatumVon.Text)2
        '    .Add(txtUebergabedatumBis.Text)3
        '    .Add(txtRechnungsnummern.Text) 4
        'End With


        Dim aCriteria(alSelectionCriteria.Count) As String
        Dim strTmpElement As String
        Dim i As Int32 = 0

        'Krücke, da in asp1.1 keine generischen Arrays möglich sind JJ 2007.09.25
        For Each strTmpElement In alSelectionCriteria
            aCriteria(i) = strTmpElement
            i = i + 1
        Next
        'Dim objSAP As New SAPProxy_VW.SAPProxy_VW()
        Try

            Dim m_tblRechnungsPruefung As DataTable = S.AP.GetExportTableWithInitExecute("Z_DAD_WEB_RGPRUEFUNG.GT_RECHNUNGEN",
                                                                                         "I_KUNNR, I_ZZREFERENZ1, I_LIZNR, I_AUSLIDAT_VON, I_AUSLIDAT_BIS, I_RECHNUNGSNUMMERN",
                                                                                         m_objUser.KUNNR.ToSapKunnr,
                                                                                         aCriteria(0), aCriteria(1), DateTime.Parse(aCriteria(2)), DateTime.Parse(aCriteria(3)), aCriteria(4))

            'Dim sapReturnTable As New SAPProxy_VW.ZDAD_RECHNUNGENTable()

            'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            'objSAP.Connection.Open()


            'objSAP.Z_Dad_Web_Rgpruefung(checkDate(aCriteria(3)), checkDate(aCriteria(2)), Right("0000000000" & m_objUser.KUNNR, 10), aCriteria(1), aCriteria(4), aCriteria(0), sapReturnTable)

            'm_tblRechnungsPruefung = sapReturnTable.ToADODataTable()
            m_strMessage = S.AP.ResultMessage

            'm_strMessage = S.AP.SapConnection.SAPUsername + " - " + S.AP.SapConnection.SAPPassword + " - " + S.AP.SapConnection.SAPAppServerHost + " - " + S.AP.SapConnection.ProdSAP.ToString + " - "

            Return m_tblRechnungsPruefung
        Catch ex As Exception
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten vorhanden."
                Case "NRF"
                    m_strMessage = "Keine Daten vorhanden."
                Case "FALSCHER_STATUS"
                    m_strMessage = "Falscher Status"
                Case Else
                    m_strMessage = ex.Message
            End Select
        Finally
            'objSAP.Connection.Close()
            'objSAP.Dispose()
        End Try
        Return m_tblRechnungsPruefung
    End Function

    Private Function checkDate(ByVal datum As String) As String
        Try
            datum = HelpProcedures.MakeDateSAP(CDate(datum).ToShortDateString)
        Catch
            datum = "00000000"
        End Try
        checkDate = datum
    End Function

#End Region
End Class

' ************************************************
' $History: VW_08.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:14
' Updated in $/CKAG/Applications/appvw/Lib
' ITA: 2837
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.07.08   Time: 14:52
' Updated in $/CKAG/Applications/appvw/Lib
' SAP connection Closed verbessert/hinzugefügt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:49
' Created in $/CKAG/Applications/appvw/Lib
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 4.10.07    Time: 11:51
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 28.09.07   Time: 15:50
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 27.09.07   Time: 17:52
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 26.09.07   Time: 17:47
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 26.09.07   Time: 16:01
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 26.09.07   Time: 15:57
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 26.09.07   Time: 15:45
' Updated in $/CKG/Applications/AppVW/AppVWWeb/Lib
' 
