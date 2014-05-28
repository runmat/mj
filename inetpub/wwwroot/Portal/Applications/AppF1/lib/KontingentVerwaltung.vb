Option Explicit On
Option Strict On

Imports CKG
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

<Serializable()> Public Class KontingentVerwaltung
    Inherits F1_BankBase

#Region " Definitions"

    Private mGruppenHaendler As DataTable
    Private m_strHaendlerReferenzNummer As String
    Private m_strSapInterneHaendlerReferenzNummer As String
    Private m_strHaendlerName As String
    Private m_strHaendlerOrt As String
    Private m_strHaendlerFiliale As String
    Private m_tblHaendler As DataTable
    Private m_tblFilialen As DataTable
    Private m_districtTable As DataTable
    Private m_strErrorMessage As String
    Private m_strNAME As String
    Private m_strNAME_2 As String
    Private m_strCOUNTRYISO As String
    Private m_strPOSTL_CODE As String
    Private m_strCITY As String
    Private m_strSTREET As String
    Private m_strREFERENZ As String
    Private m_tblSearchResult As DataTable

#End Region


#Region " Public Properties"

    Public Property GruppenHaendler() As DataTable
        Get
            Return mGruppenHaendler
        End Get
        Set(ByVal value As DataTable)
            mGruppenHaendler = value
        End Set
    End Property

#End Region

#Region " Public Methods"

    Public Sub New(ByRef objApp As Base.Kernel.Security.App, ByRef objUser As Base.Kernel.Security.User, ByVal strSessionID As String, ByVal strAppID As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, "")

        m_objApp = objApp
        m_objUser = objUser

        m_strHaendlerReferenzNummer = ""
        m_strHaendlerName = ""
        m_strHaendlerOrt = ""
        m_strHaendlerFiliale = ""
        m_strSapInterneHaendlerReferenzNummer = ""
    End Sub

    Public Sub writeKontingente(ByVal strAppID As String, ByVal strSessionID As String)
        '----------------------------------------------------------------------
        ' Methode: writeKontingente
        ' Autor: JJU
        ' Beschreibung: überträgt die Änderungen der Kontingente ins sap
        ' Erstellt am: 11.03.2009
        ' ITA: 2677
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_CREDITLIMIT_CHANGE_STD", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_ERNAM", Right(m_objUser.UserName, 12))

            S.AP.Init("Z_M_CREDITLIMIT_CHANGE_STD", "I_AG,I_ERNAM", m_strKUNNR, Right(m_objUser.UserName, 12))

            Dim SapImportTable As DataTable = S.AP.GetImportTable("GT_LIMIT") 'myProxy.getImportTable("GT_LIMIT")
            Dim sapRow As DataRow

            For Each tmpRow As DataRow In m_tblKontingente.Rows
                sapRow = SapImportTable.NewRow
                sapRow("KUNNR_EX") = tmpRow("KUNNR_EX")
                sapRow("AG") = tmpRow("AG")
                sapRow("KUNNR") = tmpRow("KUNNR")
                sapRow("KUNDENART") = tmpRow("KUNDENART")
                sapRow("KKBER") = tmpRow("KKBER")
                sapRow("KLIMK") = CInt(tmpRow("EingabeKontingent"))
                sapRow("SKFOR") = CInt(tmpRow("SKFOR"))
                sapRow("ZZRWERT") = CInt(tmpRow("ZZRWERT"))
                sapRow("FREIKONTI") = CInt(tmpRow("FREIKONTI"))
                sapRow("CRBLB") = tmpRow("EingabeGesperrt")
                sapRow("ZZFRIST") = tmpRow("ZZFRIST")
                SapImportTable.Rows.Add(sapRow)
            Next

            'myProxy.callBapi()
            S.AP.Execute()

        Catch ex As Exception
            m_intStatus = -9999
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case Else
                    m_strMessage = ex.Message
            End Select
        End Try

    End Sub

    Public Sub fillKontingent(ByVal strAppID As String, ByVal strSessionID As String, ByVal Haendlernummer As String)
        '----------------------------------------------------------------------
        ' Methode: fillHaendlerData
        ' Autor: JJU
        ' Beschreibung: gibt das Händler oder Gruppenkontinget eines Händler zurück
        ' Erstellt am: 11.03.2009
        ' ITA: 2677
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_CREDITLIMIT_DETAIL_STD", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_HAENDLER_EX", Haendlernummer)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_CREDITLIMIT_DETAIL_STD", "I_AG,I_HAENDLER_EX", m_strKUNNR, Haendlernummer)

            m_tblKontingente = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            mGruppenHaendler = S.AP.GetExportTable("GT_HAENDLER") 'myProxy.getExportTable("GT_HAENDLER")

            m_tblKontingente.Columns.Add("EingabeKontingent", Type.GetType("System.Decimal"))
            m_tblKontingente.Columns.Add("EingabeGesperrt", String.Empty.GetType)
            m_tblKontingente.Columns.Add("KontingentArt", String.Empty.GetType)

            mGruppenHaendler.Columns.Add("HaendlerAdresse", String.Empty.GetType)


            For Each tmpRow As DataRow In m_tblKontingente.Rows
                tmpRow("EingabeKontingent") = tmpRow("KLIMK")
                tmpRow("EingabeGesperrt") = tmpRow("CRBLB").ToString

                Select Case tmpRow.Item("KUNDENART").ToString
                    Case "G"
                        tmpRow.Item("KontingentArt") = "Gruppe"
                    Case "H"
                        tmpRow.Item("KontingentArt") = "Händler"
                    Case Else
                        tmpRow.Item("") = "unbekannte Kontingentart"
                End Select
            Next

            For Each tmpRow As DataRow In mGruppenHaendler.Rows
                tmpRow("HaendlerAdresse") = tmpRow("NAME1").ToString & " " & tmpRow("NAME2").ToString & " " & tmpRow("STRAS").ToString & " " & tmpRow("LAND1").ToString & " - " & tmpRow("PSTLZ").ToString & " " & tmpRow("ORT01").ToString
            Next

            m_tblKontingente.AcceptChanges()
            mGruppenHaendler.AcceptChanges()

        Catch ex As Exception
            m_intStatus = -9999
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case Else
                    m_strMessage = ex.Message
            End Select
        End Try
    End Sub

    'Private Function stringZeichenEntfernen(ByVal zeichenkette As String) As String
    '    If zeichenkette Is Nothing OrElse zeichenkette Is String.Empty Then
    '        zeichenkette = "-"
    '    Else
    '        If Not zeichenkette.IndexOf("'") = -1 Then
    '            zeichenkette = zeichenkette.Replace("'", "")
    '        End If
    '        If Not zeichenkette.IndexOf("""") = -1 Then
    '            zeichenkette = zeichenkette.Replace("""", "")
    '        End If
    '    End If
    '    Return zeichenkette
    'End Function


#End Region

End Class

' ************************************************
' $History: KontingentVerwaltung.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 30.04.09   Time: 11:39
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2837
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 12.03.09   Time: 9:10
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2678,2677 testfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 11.03.09   Time: 17:01
' Updated in $/CKAG/Applications/AppF1/lib
' ITa 2677 unfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 11.03.09   Time: 15:49
' Updated in $/CKAG/Applications/AppF1/lib
' ITa 2678 unfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 11.03.09   Time: 11:34
' Updated in $/CKAG/Applications/AppF1/lib
' unfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 11.03.09   Time: 11:26
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2677 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 11.03.09   Time: 10:28
' Created in $/CKAG/Applications/AppF1/lib
' ITA 2677&2678
' 
' ************************************************
