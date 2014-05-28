Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports System.Threading
Imports System.Globalization
Imports Microsoft.Data.SAPClient
'Liefert Übersichtsdaten für Mofa-Kennzeichen und Addressdaten der Vermittler
Public Class VFS02

    Inherits Base.Business.DatenimportBase

    Public Shared ReadOnly CONST_NO_ADDRESS_DATA As String = "Für die Vermittlernummer wurde keine Adresse gefunden."

#Region "Declarations"

    Private m_OrgNr As String
    Private m_Versicherungsjahr As Int32
    Private m_DatumVon As String
    Private m_DatumBis As String
    Private m_Kennzeichen As String
    Private m_AddressTable As DataTable
    Private m_DetailTable As DataTable
    Private m_KennzTable As DataTable
    Private m_OverviewTable As DataTable
#End Region



#Region "Properties"

    Public Property OrgNr() As String
        Get
            Return m_OrgNr
        End Get
        Set(ByVal Value As String)
            m_OrgNr = Value
        End Set
    End Property

    Public Property Versicherungsjahr() As Int32
        Get
            Return m_Versicherungsjahr
        End Get
        Set(ByVal Value As Int32)
            m_Versicherungsjahr = Value
        End Set
    End Property

    Public Property DatumVon() As String
        Get
            Return m_DatumVon
        End Get
        Set(ByVal Value As String)
            m_DatumVon = Value
        End Set
    End Property

    Public Property DatumBis() As String
        Get
            Return m_DatumBis
        End Get
        Set(ByVal Value As String)
            m_DatumBis = Value
        End Set
    End Property

    Public Property Kennzeichen() As String
        Get
            Return m_Kennzeichen
        End Get
        Set(ByVal Value As String)
            m_Kennzeichen = Value
        End Set
    End Property

    Public Property AddressTable() As DataTable
        Get
            Return m_AddressTable
        End Get
        Set(ByVal Value As DataTable)
            m_AddressTable = Value
        End Set
    End Property

    Public Property DetailTable() As DataTable
        Get
            Return m_DetailTable
        End Get
        Set(ByVal Value As DataTable)
            m_DetailTable = Value
        End Set
    End Property
    Public Property KennzTable() As DataTable
        Get
            Return m_KennzTable
        End Get
        Set(ByVal Value As DataTable)
            m_KennzTable = Value
        End Set
    End Property
    Public Property OverviewTable() As DataTable
        Get
            Return m_OverviewTable
        End Get
        Set(ByVal Value As DataTable)
            m_OverviewTable = Value
        End Set
    End Property

#End Region



    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)

    End Sub

    Public Sub GiveData(ByVal pAppID As String, ByVal pSessionID As String)
        ReadData(pAppID, _
                 pSessionID, _
                 m_DatumVon, _
                 m_DatumBis, _
                 Right("0000000000" & m_objUser.KUNNR, 10), _
                 m_Kennzeichen, _
                 m_OrgNr, _
                 m_Versicherungsjahr.ToString())
    End Sub

    Public Sub GiveDataByKennzeichen(ByVal pAppID As String, ByVal pSessionID As String)
        ReadData(pAppID, _
                 pSessionID, _
                 String.Empty, _
                 String.Empty, _
                 Right("0000000000" & m_objUser.KUNNR, 10), _
                 m_Kennzeichen, _
                 String.Empty, _
                 m_Versicherungsjahr.ToString())
    End Sub

    Public Sub GiveDataByOrgNrAndKennzeichen(ByVal pAppId As String, _
                                             ByVal pSessionId As String)
        m_strAppID = pAppId
        m_strSessionID = pSessionId

        Dim objSAP As New SAPProxy_VFS.SAPProxy_VFS()
        Dim tblSAPDetail As New SAPProxy_VFS.ZDAD_M_WEB_MOFA_DETAILS_001Table()
        Dim tblSAPDetailRow As New SAPProxy_VFS.ZDAD_M_WEB_MOFA_DETAILS_001()

        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        objSAP.Connection.Open()

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try

            intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Mofa_Details_Vers", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

            objSAP.Z_M_Mofa_Details_Vers("", _
                                         "", _
                                         Right("0000000000" & m_objUser.KUNNR, 10), _
                                         m_Kennzeichen, _
                                         m_OrgNr, _
                                         m_Versicherungsjahr.ToString(), _
                                         tblSAPDetail)
            objSAP.CommitWork()
            If intID > -1 Then
                m_objlogApp.WriteEndDataAccessSAP(intID, True)
            End If


        Catch ex As Exception
            Select Case ex.Message
                Case "NO_DATA"
                    m_intStatus = -3331
                    m_strMessage = "Keine Daten gefunden."
                Case "NO_MATERIAL"
                    m_intStatus = -3332
                    m_strMessage = "Dem Kunden ist kein Material im VJAHR zugeordnet."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
            If intID > -1 Then
                m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If

        Finally
            If intID > -1 Then
                m_objlogApp.WriteStandardDataAccessSAP(intID)
            End If

            objSAP.Connection.Close()
            objSAP.Dispose()

        End Try

        Dim tblDetail As DataTable

        tblDetail = New DataTable()

        With tblDetail.Columns
            .Add("Kennzeichen", System.Type.GetType("System.String"))
            .Add("Vertragsnummer", System.Type.GetType("System.String"))
            .Add("Versand am", System.Type.GetType("System.DateTime"))
            .Add("Verkauf am", System.Type.GetType("System.DateTime"))
            .Add("Rücklauf am", System.Type.GetType("System.DateTime"))
            .Add("Verlust am", System.Type.GetType("System.DateTime"))
            .Add("Stueckzahl", System.Type.GetType("System.Int32"))
            .Add("VD-Bezirk", System.Type.GetType("System.Int64"))
            .Add("Name1", System.Type.GetType("System.String"))
            .Add("Name2", System.Type.GetType("System.String"))
        End With

        For Each tblSAPDetailRow In tblSAPDetail
            Dim rowDetail As DataRow

            rowDetail = tblDetail.NewRow
            rowDetail("VD-Bezirk") = CType(tblSAPDetailRow.Eikto_Vm, Int64)
            rowDetail("Name1") = tblSAPDetailRow.Name1_Sb
            rowDetail("Name2") = tblSAPDetailRow.Name2_Sb
            rowDetail("Vertragsnummer") = tblSAPDetailRow.Zzreferenz1
            rowDetail("Stueckzahl") = CInt(tblSAPDetailRow.Lifmeng)
            rowDetail("Kennzeichen") = tblSAPDetailRow.Sernr
            Dim sTemp As String = (Right(tblSAPDetailRow.Datum, 2) & "." & Mid(tblSAPDetailRow.Datum, 5, 2) & "." & Left(tblSAPDetailRow.Datum, 4))
            If IsDate(sTemp) Then rowDetail("Versand am") = sTemp
            rowDetail("Kennzeichen") = tblSAPDetailRow.Sernr
            sTemp = (Right(tblSAPDetailRow.Dat_Verk, 2) & "." & Mid(tblSAPDetailRow.Dat_Verk, 5, 2) & "." & Left(tblSAPDetailRow.Dat_Verk, 4))
            If IsDate(sTemp) Then rowDetail("Verkauf am") = sTemp
            sTemp = (Right(tblSAPDetailRow.Dat_Rueck, 2) & "." & Mid(tblSAPDetailRow.Dat_Rueck, 5, 2) & "." & Left(tblSAPDetailRow.Dat_Rueck, 4))
            If IsDate(sTemp) Then rowDetail("Rücklauf am") = sTemp
            sTemp = (Right(tblSAPDetailRow.Dat_Verl, 2) & "." & Mid(tblSAPDetailRow.Dat_Verl, 5, 2) & "." & Left(tblSAPDetailRow.Dat_Verl, 4))
            If IsDate(sTemp) Then rowDetail("Verlust am") = sTemp


            tblDetail.Rows.Add(rowDetail)
        Next
        m_DetailTable = tblDetail

        Dim qryDataHelper As QueryDataHelper = New QueryDataHelper()

        m_OverviewTable = qryDataHelper.SelectGroupByInto("TMP", m_DetailTable, "VD-Bezirk, Name2, Name1", , "VD-Bezirk, Name2")
    End Sub

    Private Sub ReadData(ByVal pAppId As String, _
                         ByVal pSessionId As String, _
                         ByVal pDatumVon As String, _
                         ByVal pDatumBis As String, _
                         ByVal pKUNNR As String, _
                         ByVal pKennzeichen As String, _
                         ByVal pOrgNr As String, _
                         ByVal pVersicherungsjahr As String)

        m_strAppID = pAppId
        m_strSessionID = pSessionId

        Dim objSAP As New SAPProxy_VFS.SAPProxy_VFS()
        Dim tblSAPDetail As New SAPProxy_VFS.ZDAD_M_WEB_MOFA_DETAILS_001Table()
        Dim tblSAPDetailRow As New SAPProxy_VFS.ZDAD_M_WEB_MOFA_DETAILS_001()

        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        objSAP.Connection.Open()

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If
        Dim strDatZulVon As String
        If IsDate(pDatumVon) Then
            strDatZulVon = HelpProcedures.MakeDateSAP(pDatumVon)
            If strDatZulVon = "10101" Then
                strDatZulVon = "|"
            End If
        Else
            strDatZulVon = "|"
        End If
        Dim strDatZulBis As String
        If IsDate(pDatumBis) Then
            strDatZulBis = HelpProcedures.MakeDateSAP(pDatumBis)
            If strDatZulBis = "10101" Then
                strDatZulBis = "|"
            End If
        Else
            strDatZulBis = "|"
        End If
        Dim intID As Int32 = -1

        Try

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Mofa_Details_Vers", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

            objSAP.Z_M_Mofa_Details_Vers(strDatZulBis, _
                                         strDatZulVon, _
                                         pKUNNR, _
                                         pKennzeichen, _
                                         pOrgNr, _
                                         pVersicherungsjahr, _
                                         tblSAPDetail)

            objSAP.CommitWork()
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If


        Catch ex As Exception
            Select Case ex.Message
                Case "NO_DATA"
                    m_intStatus = -3331
                    m_strMessage = "Keine Daten gefunden."
                Case "NO_MATERIAL"
                    m_intStatus = -3332
                    m_strMessage = "Dem Kunden ist kein Material im VJAHR zugeordnet."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If

        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If

            objSAP.Connection.Close()
            objSAP.Dispose()

        End Try

        Dim tblDetail As DataTable

        tblDetail = New DataTable()

        With tblDetail.Columns
            .Add("Kennzeichen", System.Type.GetType("System.String"))
            .Add("Auftragsnummer", System.Type.GetType("System.Int64"))
            .Add("Vertragsnummer", System.Type.GetType("System.String"))
            .Add("Versand am", System.Type.GetType("System.DateTime"))
            .Add("Verkauf am", System.Type.GetType("System.DateTime"))
            .Add("Rücklauf am", System.Type.GetType("System.DateTime"))
            .Add("Verlust am", System.Type.GetType("System.DateTime"))
            .Add("Stueckzahl", System.Type.GetType("System.Int32"))
            .Add("VD-Bezirk", System.Type.GetType("System.Int64"))
            .Add("Name1", System.Type.GetType("System.String"))
            .Add("Name2", System.Type.GetType("System.String"))
        End With

        For Each tblSAPDetailRow In tblSAPDetail
            Dim rowDetail As DataRow

            rowDetail = tblDetail.NewRow
            rowDetail("VD-Bezirk") = CType(tblSAPDetailRow.Eikto_Vm, Int64)
            rowDetail("Name1") = tblSAPDetailRow.Name1_Sb
            rowDetail("Name2") = tblSAPDetailRow.Name2_Sb
            rowDetail("Vertragsnummer") = tblSAPDetailRow.Zzreferenz1
            rowDetail("Auftragsnummer") = CType(tblSAPDetailRow.Vgbel, Int64)
            rowDetail("Stueckzahl") = CInt(tblSAPDetailRow.Lifmeng)
            rowDetail("Kennzeichen") = tblSAPDetailRow.Sernr
            Dim sTemp As String = (Right(tblSAPDetailRow.Datum, 2) & "." & Mid(tblSAPDetailRow.Datum, 5, 2) & "." & Left(tblSAPDetailRow.Datum, 4))
            If IsDate(sTemp) Then rowDetail("Versand am") = sTemp
            rowDetail("Kennzeichen") = tblSAPDetailRow.Sernr
            sTemp = (Right(tblSAPDetailRow.Dat_Verk, 2) & "." & Mid(tblSAPDetailRow.Dat_Verk, 5, 2) & "." & Left(tblSAPDetailRow.Dat_Verk, 4))
            If IsDate(sTemp) Then rowDetail("Verkauf am") = sTemp
            sTemp = (Right(tblSAPDetailRow.Dat_Rueck, 2) & "." & Mid(tblSAPDetailRow.Dat_Rueck, 5, 2) & "." & Left(tblSAPDetailRow.Dat_Rueck, 4))
            If IsDate(sTemp) Then rowDetail("Rücklauf am") = sTemp
            sTemp = (Right(tblSAPDetailRow.Dat_Verl, 2) & "." & Mid(tblSAPDetailRow.Dat_Verl, 5, 2) & "." & Left(tblSAPDetailRow.Dat_Verl, 4))
            If IsDate(sTemp) Then rowDetail("Verlust am") = sTemp


            tblDetail.Rows.Add(rowDetail)
        Next

        m_DetailTable = tblDetail

        Dim qryDataHelper As QueryDataHelper = New QueryDataHelper()

        m_OverviewTable = qryDataHelper.SelectGroupByInto("TMP", m_DetailTable, "VD-Bezirk, Name1, Name2, Auftragsnummer, Versand am, Stueckzahl", , " VD-Bezirk, Name1, Name2, Auftragsnummer, Versand am, Stueckzahl")
    End Sub


    'Liefert die Adressdaten der selektierten Vermittlernummern
    Public Sub GiveAddressData(ByVal pListeVermittlernummer As System.Collections.ArrayList, _
                               ByVal strAppID As String, _
                               ByVal strSessionID As String)
        If pListeVermittlernummer.Count = 0 Then Exit Sub

        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Dim tblAdressen As DataTable = New DataTable()

        With tblAdressen.Columns
            .Add("VD-Bezirk", System.Type.GetType("System.Int64"))
            .Add("Name 1", System.Type.GetType("System.String"))
            .Add("Name 2", System.Type.GetType("System.String"))
            .Add("PLZ", System.Type.GetType("System.String"))
            .Add("Ort", System.Type.GetType("System.String"))
            .Add("Strasse", System.Type.GetType("System.String"))
            .Add("Telefon", System.Type.GetType("System.String"))
            .Add("E-Mail", System.Type.GetType("System.String"))
        End With


        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1
        Dim tmpVermittlernummer As String

        Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        con.Open()


        Dim cmd As New SAPCommand()
        cmd.Connection = con

        Dim strCom As String

        strCom = "EXEC Z_M_Mofa_Adressdaten_Vm "
        strCom = strCom & "@I_KUNNR_AG=@pI_KUNNR_AG,@I_KUN_EXT_VM=@pI_KUN_EXT_VM,@GS_WEB=@pE_GS_WEB OUTPUT OPTION 'disabledatavalidation'"

        cmd.CommandText = strCom


        'importparameter
        Dim pI_KUNNR_AG As New SAPParameter("@pI_KUNNR_AG", ParameterDirection.Input)
        Dim pI_KUN_EXT_VM As New SAPParameter("@pI_KUN_EXT_VM", ParameterDirection.Input)

        'exportParameter
        Dim pE_GS_WEB As New SAPParameter("@pE_GS_WEB", ParameterDirection.Output)

        'Importparameter hinzufügen
        cmd.Parameters.Add(pI_KUNNR_AG)
        cmd.Parameters.Add(pI_KUN_EXT_VM)

        'exportparameter hinzugfügen
        cmd.Parameters.Add(pE_GS_WEB)

        'befüllen der Importparameter
        pI_KUNNR_AG.Value = Right("0000000000" & m_objUser.KUNNR, 10)


        For Each tmpVermittlernummer In pListeVermittlernummer
            Try
                Dim tmpNrArray() As String
                Dim bFlag As Boolean = False
                tmpNrArray = Split(tmpVermittlernummer, "#")
                Dim tmpVerNr As String = tmpNrArray(0)

                pI_KUN_EXT_VM.Value = tmpVerNr
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Mofa_Adressdaten_Vm", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                cmd.ExecuteNonQuery()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim rowAdresse As DataRow
                Dim i As Integer
                For i = 0 To tblAdressen.Rows.Count - 1
                    Dim sTemprow As DataRow
                    sTemprow = tblAdressen.Rows(i)
                    Dim sTempNr As String = sTemprow.Item("VD-Bezirk").ToString

                    If sTempNr = tmpVerNr Then
                        bFlag = True
                    End If
                Next

                Dim tblSAPDetail As DataTable
                If Not pE_GS_WEB.Value Is DBNull.Value Then

                    tblSAPDetail = DirectCast(pE_GS_WEB.Value, DataTable)

                    If Not bFlag Then
                        rowAdresse = tblAdressen.NewRow()
                        With rowAdresse
                            .Item("VD-Bezirk") = Int64.Parse(tmpVerNr)
                            .Item("Name 1") = tblSAPDetail.Rows(0)("Name1").ToString
                            .Item("Name 2") = tblSAPDetail.Rows(0)("Name2").ToString
                            .Item("PLZ") = tblSAPDetail.Rows(0)("Pstlz").ToString
                            .Item("Ort") = tblSAPDetail.Rows(0)("Ort01").ToString
                            .Item("Strasse") = tblSAPDetail.Rows(0)("Stras").ToString
                            .Item("Telefon") = tblSAPDetail.Rows(0)("Telf1").ToString
                            .Item("E-Mail") = tblSAPDetail.Rows(0)("SMTP_ADDR").ToString
                        End With

                        tblAdressen.Rows.Add(rowAdresse)
                    End If
                End If
                pE_GS_WEB.Value = Nothing

        Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        Dim rowAdresse As DataRow = tblAdressen.NewRow()
                        With rowAdresse
                            .Item("VD-Bezirk") = Convert.ToInt64(tmpVermittlernummer.Split("#"c)(0))
                            .Item("Name 1") = String.Empty
                            .Item("Name 2") = String.Empty
                            .Item("PLZ") = String.Empty
                            .Item("Ort") = CONST_NO_ADDRESS_DATA
                            .Item("Strasse") = String.Empty
                            .Item("Telefon") = String.Empty
                            .Item("E-Mail") = String.Empty
                        End With
                        tblAdressen.Rows.Add(rowAdresse)
                        m_intStatus = -3331
                        m_strMessage = CONST_NO_ADDRESS_DATA
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
            End Try
  Next


        If intID > -1 Then
            m_objLogApp.WriteStandardDataAccessSAP(intID)
        End If
        con.Close()

        AddressTable = tblAdressen

    End Sub

End Class

' ************************************************
' $History: VFS02.vb $
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 4.12.08    Time: 10:37
' Updated in $/CKAG/Applications/appvfs/Lib
' ita 2377 nachbesserung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 4.12.08    Time: 10:12
' Updated in $/CKAG/Applications/appvfs/Lib
' ITA 2377 testfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 27.11.08   Time: 14:35
' Updated in $/CKAG/Applications/appvfs/Lib
' ITA 2317 Testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 26.11.08   Time: 17:11
' Updated in $/CKAG/Applications/appvfs/Lib
' ITA 2317 unfertig
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:43
' Created in $/CKAG/Applications/appvfs/Lib
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 19.07.07   Time: 16:59
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 19.07.07   Time: 13:45
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA: 1140
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 18.07.07   Time: 16:20
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA: 1140
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 17.07.07   Time: 16:23
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA: 1140 SAPProxy_VFS neu generiert
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 3.07.07    Time: 9:17
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 3.07.07    Time: 8:48
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 2.07.07    Time: 16:01
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' 
' *****************  Version 2  *****************
' User: Uha          Date: 21.06.07   Time: 18:45
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' Bugfixing VFS 2
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 19:07
' Created in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
