Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports Microsoft.Data.SAPClient
'Liefert Übersichtsdaten für Mofa-Kennzeichen und Addressdaten der Vermittler
Public Class VFS01

    Inherits Base.Business.DatenimportBase

    Public Shared ReadOnly CONST_NO_ADDRESS_DATA As String = "Für die Vermittlernummer wurde keine Adresse gefunden."

#Region "Declarations"

    Private m_DetailTable As DataTable
    Private m_AddressTable As DataTable
    Private m_OrgNr As String
    Private m_Versicherungsjahr As String
    Private m_Erdat As String
    Private m_VermittlerAnzahl As String
    Private m_KennzeichenAnzahl As String
    Private m_UnverkaufteAnzahl As String
    Private m_VerkaufteAnzahl As String
    Private m_VerloreneAnzahl As String
    Private m_RuecklaeuferAnzahl As String
    Private m_LagerAnzahl As String
#End Region



#Region "Properties"

    Public Property Erdat() As String
        Get
            Return m_Erdat
        End Get
        Set(ByVal Value As String)
            m_Erdat = Value
        End Set
    End Property

    Public Property VermittlerAnzahl() As String
        Get
            Return m_VermittlerAnzahl
        End Get
        Set(ByVal Value As String)
            m_VermittlerAnzahl = Value
        End Set
    End Property

    Public ReadOnly Property KennzeichenGesamtbestand() As String
        Get
            'hart codierter wert laut rau ITA 2317 JJU20081126
            Select Case m_Versicherungsjahr
                Case "2007"
                    Return "36900"
                Case "2008"
                    Return "36900"
                Case "2009"
                    Return "34200"
                Case Else
                    Throw New Exception("nicht gepflegtes versicherungsjahr")
            End Select
        End Get
    End Property

    Public Property UnverkaufteAnzahl() As String
        Get
            Return m_UnverkaufteAnzahl
        End Get
        Set(ByVal Value As String)
            m_UnverkaufteAnzahl = Value
        End Set
    End Property

    Public Property VerkaufteAnzahl() As String
        Get
            Return m_VerkaufteAnzahl
        End Get
        Set(ByVal Value As String)
            m_VerkaufteAnzahl = Value
        End Set
    End Property

    Public Property VerloreneAnzahl() As String
        Get
            Return m_VerloreneAnzahl
        End Get
        Set(ByVal Value As String)
            m_VerloreneAnzahl = Value
        End Set
    End Property

    Public Property RuecklaeuferAnzahl() As String
        Get
            Return m_RuecklaeuferAnzahl
        End Get
        Set(ByVal Value As String)
            m_RuecklaeuferAnzahl = Value
        End Set
    End Property

    Public Property LagerAnzahl() As String
        Get
            Return m_LagerAnzahl
        End Get
        Set(ByVal Value As String)
            m_LagerAnzahl = Value
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

    Public Property AddressTable() As DataTable
        Get
            Return m_AddressTable
        End Get
        Set(ByVal Value As DataTable)
            m_AddressTable = Value
        End Set
    End Property

    Public Property OrgNr() As String
        Get
            Return m_OrgNr
        End Get
        Set(ByVal Value As String)
            m_OrgNr = Value
        End Set
    End Property

    Public Property Versicherungsjahr() As String
        Get
            Return m_Versicherungsjahr
        End Get
        Set(ByVal Value As String)
            m_Versicherungsjahr = Value
        End Set
    End Property

#End Region



    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)

    End Sub

    Public Sub GiveData(ByVal strAppID As String, ByVal strSessionID As String)

        m_strAppID = strAppID
        m_strSessionID = strSessionID


        Dim objSAP As New SAPProxy_VFS.SAPProxy_VFS()
        Dim tblSAPDetail As New SAPProxy_VFS.ZDAD_MOFA_UEBERS_WEBTable()
        Dim tblSAPDetailRow As New SAPProxy_VFS.ZDAD_MOFA_UEBERS_WEB()


        objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        objSAP.Connection.Open()

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1

        Try
            intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Mofa_Uebersicht", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

            objSAP.Z_M_Mofa_Uebersicht(Right("0000000000" & m_objUser.KUNNR, 10), OrgNr, Versicherungsjahr, _
                                       "", RuecklaeuferAnzahl, UnverkaufteAnzahl, VerkaufteAnzahl, _
                                       VerloreneAnzahl, VermittlerAnzahl, LagerAnzahl, Erdat, tblSAPDetail)
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

        Dim tblFahrzeuge As DataTable

        tblFahrzeuge = New DataTable()

        With tblFahrzeuge.Columns
            .Add("KUN_EXT_VM", System.Type.GetType("System.String"))
            .Add("NAME2_VM", System.Type.GetType("System.String"))
            .Add("NAME1_VM", System.Type.GetType("System.String"))
            .Add("ANZ_GES", System.Type.GetType("System.Int32"))
            .Add("ANZ_VERK", System.Type.GetType("System.Int32"))
            .Add("ANZ_UNVERK", System.Type.GetType("System.Int32"))
            .Add("ANZ_VERL", System.Type.GetType("System.Int32"))
            .Add("ANZ_RUECK", System.Type.GetType("System.Int32"))
        End With

        For Each tblSAPDetailRow In tblSAPDetail

            Dim rowFahrzeug As DataRow
            rowFahrzeug = tblFahrzeuge.NewRow
            rowFahrzeug("KUN_EXT_VM") = tblSAPDetailRow.Kun_Ext_Vm
            rowFahrzeug("NAME2_VM") = tblSAPDetailRow.Name2_Vm
            rowFahrzeug("NAME1_VM") = tblSAPDetailRow.Name1_Vm
            rowFahrzeug("ANZ_GES") = CInt(tblSAPDetailRow.Anz_Ges)
            rowFahrzeug("ANZ_UNVERK") = CInt(tblSAPDetailRow.Anz_Unverk)
            rowFahrzeug("ANZ_VERK") = CInt(tblSAPDetailRow.Anz_Verk)
            rowFahrzeug("ANZ_VERL") = CInt(tblSAPDetailRow.Anz_Verl)
            rowFahrzeug("ANZ_RUECK") = CInt(tblSAPDetailRow.Anz_Rueck)
            tblFahrzeuge.Rows.Add(rowFahrzeug)
        Next

        DetailTable = tblFahrzeuge


    End Sub


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
                            .Item("VD-Bezirk") = Int64.Parse(tmpVermittlernummer)
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
' $History: VFS01.vb $
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 19.02.09   Time: 11:03
' Updated in $/CKAG/Applications/appvfs/Lib
' pflege Versicherungsjahr 2009 Steentoft
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 4.12.08    Time: 10:37
' Updated in $/CKAG/Applications/appvfs/Lib
' ita 2377 nachbesserung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 27.11.08   Time: 14:35
' Updated in $/CKAG/Applications/appvfs/Lib
' ITA 2317 Testfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 26.11.08   Time: 17:11
' Updated in $/CKAG/Applications/appvfs/Lib
' ITA 2317 unfertig
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 14.05.08   Time: 15:21
' Updated in $/CKAG/Applications/appvfs/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:43
' Created in $/CKAG/Applications/appvfs/Lib
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 3.07.07    Time: 9:17
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 9  *****************
' User: Uha          Date: 21.06.07   Time: 18:45
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' Bugfixing VFS 2
' 
' *****************  Version 8  *****************
' User: Uha          Date: 3.05.07    Time: 19:07
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
