Option Explicit On
Option Infer On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel.Common

'Liefert Übersichtsdaten für Mofa-Kennzeichen und Addressdaten der Vermittler
Public Class VFS01

    Inherits DatenimportBase

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


        'hart codierter wert laut rau ITA 2317 JJU20081126
        Get
            Try
                Select Case m_Versicherungsjahr
                    Case "2010"
                        Return "91700"
                    Case Else
                        Throw New Exception("nicht gepflegtes Versicherungsjahr")
                        Return ""
                End Select
            Catch
                Return ""
            End Try
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



    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByVal objApp As CKG.Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)

    End Sub

    Public Sub GiveDataDyn(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1
            Dim tblSAPDetail As DataTable

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Mofa_Uebersicht", m_objApp, m_objUser, page)

                Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                myProxy.setImportParameter("I_VJAHR", m_Versicherungsjahr.ToString())
                myProxy.setImportParameter("I_VERM", m_OrgNr)


                myProxy.callBapi()

                RuecklaeuferAnzahl = myProxy.getExportParameter("ANZ_RUECK")
                UnverkaufteAnzahl = myProxy.getExportParameter("ANZ_UNVERK")
                VerkaufteAnzahl = myProxy.getExportParameter("ANZ_VERK")
                VerloreneAnzahl = myProxy.getExportParameter("ANZ_VERL")
                VermittlerAnzahl = myProxy.getExportParameter("ANZ_VERM")
                LagerAnzahl = myProxy.getExportParameter("ANZ_ZENTLAGER")
                Erdat = myProxy.getExportParameter("ERDAT")
                If IsDate(Erdat) Then Erdat = CDate(Erdat).ToShortDateString
                tblSAPDetail = myProxy.getExportTable("GT_WEB")

                For Each RowSap As DataRow In tblSAPDetail.Rows
                    RowSap("KUN_EXT_VM") = Left(RowSap("KUN_EXT_VM").ToString, 4) & "-" & Mid(RowSap("KUN_EXT_VM").ToString, 5, 4) & "-" & Right(RowSap("KUN_EXT_VM").ToString, 1)
                    tblSAPDetail.AcceptChanges()
                Next
                DetailTable = tblSAPDetail

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_intStatus = -3331
                        m_strMessage = "Keine Daten gefunden."
                    Case "NO_MATERIAL"
                        m_intStatus = -3332
                        m_strMessage = "Dem Kunden ist kein Material im VJAHR zugeordnet."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select

            Finally
                m_blnGestartet = False
            End Try
        End If




    End Sub

    Public Sub GiveAddressData(ByVal pListeVermittlernummer As ArrayList, _
                            ByVal strAppID As String, _
                            ByVal strSessionID As String)
        If pListeVermittlernummer.Count = 0 Then Exit Sub

        m_strAppID = strAppID
        m_strSessionID = strSessionID


        Dim tblAdressen As DataTable = New DataTable()

        With tblAdressen.Columns
            .Add("VD-Bezirk", GetType(Int64))
            .Add("Name 1", GetType(String))
            .Add("Name 2", GetType(String))
            .Add("PLZ", GetType(String))
            .Add("Ort", GetType(String))
            .Add("Strasse", GetType(String))
            .Add("Telefon", GetType(String))
            .Add("E-Mail", GetType(String))
        End With



        If m_objLogApp Is Nothing Then
            m_objLogApp = New CKG.Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Dim intID As Int32 = -1
        Dim tmpVermittlernummer As String

        For Each tmpVermittlernummer In pListeVermittlernummer
            Try

                Dim tmpNrArray() As String
                Dim bFlag As Boolean = False
                tmpNrArray = Split(tmpVermittlernummer, "#")
                Dim tmpVerNr As String = tmpNrArray(0)


                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Mofa_Adressdaten_Vm", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim proxy = DynSapProxy.getProxy("Z_M_Mofa_Adressdaten_Vm", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                'befüllen der Importparameter
                proxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                proxy.setImportParameter("I_KUN_EXT_VM", tmpVerNr)

                proxy.callBapi()

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

                Dim tblSAPDetail = proxy.getExportTable("GS_WEB")
                If Not tblSAPDetail Is Nothing AndAlso tblSAPDetail.Rows.Count > 0 Then
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

        AddressTable = tblAdressen

    End Sub

End Class

' ************************************************
' $History: vfs01.vb $
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 15.03.10   Time: 13:01
' Updated in $/CKAG2/Applications/AppInsurance/lib
' ITA: 2918
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 26.01.10   Time: 16:12
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 20.01.10   Time: 15:54
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 26.11.09   Time: 17:03
' Updated in $/CKAG2/Applications/AppInsurance/lib
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 26.10.09   Time: 14:30
' Updated in $/CKAG2/Applications/AppInsurance/lib
' ITA: 3249, 3206
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 28.04.09   Time: 13:58
' Updated in $/CKAG2/Applications/AppGenerali/lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 23.01.09   Time: 13:43
' Updated in $/CKAG2/Applications/AppGenerali/lib
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 12.01.09   Time: 13:40
' Created in $/CKAG2/Applications/AppGenerali/lib
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 12.01.09   Time: 13:28
' Created in $/CKG/Services/Applications/AppGenerali/lib
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
