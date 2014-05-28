Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common



Public Class Unfallmeldung

    Inherits Base.Business.DatenimportBase

#Region " Declarations"

    Private m_tblResultFahrzeuge As DataTable
    Private m_Stationen As DataTable
    Private m_EquiNr As String
    Private m_Unfallnummer As String
    Private m_Mahnstufe As String
    Private m_Station As String
    Private m_WebUser As String
    Private m_AnlagedatVon As String
    Private m_AnlagedatBis As String
    Private m_AbmeldedatVon As String
    Private m_AbmeldedatBis As String
    Private m_OhneAbmeld As String
    Private m_MitAbmeld As String
    Private m_tblUnfallMeldung As DataTable
    Private m_tblUFMeldungExcel As DataTable
    Private m_E_SUBRC As String
    Private m_E_MESSAGE As String
    Private m_Stornierte As String
    Private m_StornoBem As String
    Private m_MeldeTyp As String
#End Region

#Region " Properties"
    Public Property ResultFahrzeuge() As DataTable
        Get
            Return m_tblResultFahrzeuge
        End Get
        Set(ByVal value As DataTable)
            m_tblResultFahrzeuge = value
        End Set
    End Property
    Public Property Stationen() As DataTable
        Get
            Return m_Stationen
        End Get
        Set(ByVal value As DataTable)
            m_Stationen = value

        End Set
    End Property
    Public Property EquiNr() As String
        Get
            Return m_EquiNr
        End Get
        Set(ByVal value As String)
            m_EquiNr = value
        End Set
    End Property

    Public Property Unfallnummer() As String
        Get
            Return m_Unfallnummer
        End Get
        Set(ByVal value As String)
            m_Unfallnummer = value
        End Set
    End Property
    Public Property Mahnstufe() As String
        Get
            Return m_Mahnstufe
        End Get
        Set(ByVal value As String)
            m_Mahnstufe = value
        End Set
    End Property

    Public Property WebUser() As String
        Get
            Return m_WebUser
        End Get
        Set(ByVal value As String)
            m_WebUser = value
        End Set

    End Property

    Public Property Station() As String
        Get
            Return m_Station
        End Get
        Set(ByVal value As String)
            m_Station = value
        End Set
    End Property
    Public Property UFMeldungExcel() As DataTable
        Get
            Return m_tblUFMeldungExcel
        End Get
        Set(ByVal value As DataTable)
            m_tblUFMeldungExcel = value
        End Set
    End Property
    Public Property UnfallMeldungen() As DataTable
        Get
            Return m_tblUnfallMeldung
        End Get
        Set(ByVal value As DataTable)
            m_tblUnfallMeldung = value
        End Set
    End Property
    Public Property AnlagedatVon() As String
        Get
            Return m_AnlagedatVon
        End Get
        Set(ByVal value As String)
            m_AnlagedatVon = value
        End Set
    End Property
    Public Property AnlagedatBis() As String
        Get
            Return m_AnlagedatBis
        End Get
        Set(ByVal value As String)
            m_AnlagedatBis = value
        End Set
    End Property

    Public Property AbmeldedatVon() As String
        Get
            Return m_AbmeldedatVon
        End Get
        Set(ByVal value As String)
            m_AbmeldedatVon = value
        End Set
    End Property
    Public Property AbmeldedatBis() As String
        Get
            Return m_AbmeldedatBis
        End Get
        Set(ByVal value As String)
            m_AbmeldedatBis = value
        End Set
    End Property

    Public Property OhneAbmeld() As String
        Get
            Return m_OhneAbmeld
        End Get
        Set(ByVal value As String)
            m_OhneAbmeld = value
        End Set
    End Property
    Public Property MitAbmeld() As String
        Get
            Return m_MitAbmeld
        End Get
        Set(ByVal value As String)
            m_MitAbmeld = value
        End Set
    End Property
    Public Property StornoBem() As String
        Get
            Return m_StornoBem
        End Get
        Set(ByVal value As String)
            m_StornoBem = value
        End Set
    End Property
    Public Property Stornierte() As String
        Get
            Return m_Stornierte
        End Get
        Set(ByVal value As String)
            m_Stornierte = value
        End Set
    End Property
    Public Property MeldeTyp() As String
        Get
            Return m_MeldeTyp
        End Get
        Set(ByVal value As String)
            m_MeldeTyp = value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub


    Public Sub FillUnfall(ByVal strAppID As String, ByVal strSessionID As String, ByVal Kennzeichen As String, ByVal Fahrgestellnummer As String, ByVal Briefnummer As String, ByVal Unitnummer As String, ByVal page As Page)
        m_strClassAndMethod = "Unfallmeldung.FillUnfall"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_UF_EQUI_SUCHE", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_LICENSE_NUM", UCase(Kennzeichen))
            'myProxy.setImportParameter("I_CHASSIS_NUM", UCase(Fahrgestellnummer))
            'myProxy.setImportParameter("I_TIDNR", UCase(Briefnummer))
            'myProxy.setImportParameter("I_ZZREFERENZ1", UCase(Unitnummer))
            'myProxy.setImportParameter("I_VORG_ART", UCase(MeldeTyp))


            'myProxy.callBapi()


            'm_tblResultFahrzeuge = myProxy.getExportTable("GT_EQUIS")

            S.AP.InitExecute("Z_DPM_UF_EQUI_SUCHE", "I_AG,I_LICENSE_NUM,I_CHASSIS_NUM,I_TIDNR,I_ZZREFERENZ1,I_VORG_ART", _
                             Right("0000000000" & m_objUser.KUNNR, 10), UCase(Kennzeichen), UCase(Fahrgestellnummer), UCase(Briefnummer), UCase(Unitnummer), UCase(MeldeTyp))

            m_tblResultFahrzeuge = S.AP.GetExportTable("GT_EQUIS")

            WriteLogEntry(True, "I_AG=" & m_objUser.KUNNR & ", I_TIDNR=" & UCase(Briefnummer) & ", I_CHASSIS_NUM=" & UCase(Fahrgestellnummer) & ", I_ZZREFERENZ1=" & UCase(Unitnummer) & ", I_LICENSE_NUM=" & UCase(Kennzeichen), m_tblResultFahrzeuge, False)

        Catch ex As Exception
            m_intStatus = -2222
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case "NO_WEB"
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case Else
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select

            WriteLogEntry(False, "I_AG=" & m_objUser.KUNNR & ", I_TIDNR=" & UCase(Briefnummer) & ", I_CHASSIS_NUM=" & UCase(Fahrgestellnummer) & ", ZZREF1=" & UCase(Unitnummer) & ", I_LICENSE_NUM=" & UCase(Kennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblResultFahrzeuge, False)
        End Try
    End Sub

    Public Sub GetStation(ByVal strAppID As String, ByVal strSessionID As String, ByVal Station As String, ByVal page As Page)
        m_strClassAndMethod = "Unfallmeldung.GetStation"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_UF_STATION_LESEN", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_STATION", Station)

            'myProxy.callBapi()


            'm_Stationen = myProxy.getExportTable("ES_STATION")

            S.AP.InitExecute("Z_DPM_UF_STATION_LESEN", "I_AG,I_STATION", Right("0000000000" & m_objUser.KUNNR, 10), Station)

            m_Stationen = S.AP.GetExportTable("ES_STATION")

        Catch ex As Exception
            m_intStatus = -2222
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case "NO_WEB"
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case Else
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select

        End Try
    End Sub

    Public Sub SetMeldung(ByVal strAppID As String, ByVal strSessionID As String, ByVal Station As String, ByVal Standort As String, ByVal page As Page)
        m_strClassAndMethod = "Unfallmeldung.SetMeldung"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            m_Unfallnummer = ""

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_UF_CREATE", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_EQUNR", EquiNr)
            'myProxy.setImportParameter("I_ERNAM", m_objUser.UserName)
            'myProxy.setImportParameter("I_STATION", Station)
            'myProxy.setImportParameter("I_STANDORT", Standort)
            'myProxy.setImportParameter("I_VORG_ART", MeldeTyp)


            'myProxy.callBapi()


            'm_Unfallnummer = myProxy.getExportParameter("E_UNFALL_NR")


            'm_E_SUBRC = myProxy.getExportParameter("E_SUBRC")
            'm_E_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

            S.AP.InitExecute("Z_DPM_UF_CREATE", "I_AG,I_EQUNR,I_ERNAM,I_STATION,I_STANDORT,I_VORG_ART", _
                             Right("0000000000" & m_objUser.KUNNR, 10), EquiNr, m_objUser.UserName, Station, Standort, MeldeTyp)

            m_Unfallnummer = S.AP.GetExportParameter("E_UNFALL_NR")

            ' Fehlerhandling
            m_E_SUBRC = S.AP.GetExportParameter("E_SUBRC")
            m_E_MESSAGE = S.AP.GetExportParameter("E_MESSAGE")

            If m_E_SUBRC <> "0" Then
                m_intStatus = CInt(m_E_SUBRC)
                m_strMessage = m_E_MESSAGE
            End If

        Catch ex As Exception
            m_intStatus = -2222
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case "NO_WEB"
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case Else
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
        End Try
    End Sub

    Public Sub FillUnfallMeldungen(ByVal strAppID As String, ByVal strSessionID As String, ByVal Kennzeichen As String, ByVal Fahrgestellnummer As String, ByVal page As Page)
        m_strClassAndMethod = "Unfallmeldung.FillUnfallMeldungen"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_strMessage = ""
        m_intStatus = 0
        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_UF_MELDUNGS_SUCHE", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_LICENSE_NUM", UCase(Kennzeichen))
            'myProxy.setImportParameter("I_CHASSIS_NUM", UCase(Fahrgestellnummer))
            'myProxy.setImportParameter("I_ERDAT_VON", m_AnlagedatVon)
            'myProxy.setImportParameter("I_ERDAT_BIS", m_AnlagedatBis)
            'myProxy.setImportParameter("I_ERNAM", m_WebUser)
            'myProxy.setImportParameter("I_OHNE_ABM", m_OhneAbmeld)
            'myProxy.setImportParameter("I_MIT_ABM", m_MitAbmeld)
            'myProxy.setImportParameter("I_ABMDT_VON", m_AbmeldedatVon)
            'myProxy.setImportParameter("I_ABMDT_BIS", m_AbmeldedatBis)
            'myProxy.setImportParameter("I_MAHNSTUFE", m_Mahnstufe)
            'myProxy.setImportParameter("I_STATION", m_Station)
            'myProxy.setImportParameter("I_STORNO", m_Stornierte)

            'myProxy.callBapi()

            'Dim SAPTable As DataTable
            'SAPTable = myProxy.getExportTable("GT_UF")

            S.AP.InitExecute("Z_DPM_UF_MELDUNGS_SUCHE", "I_AG,I_LICENSE_NUM,I_CHASSIS_NUM,I_ERDAT_VON,I_ERDAT_BIS,I_ERNAM,I_OHNE_ABM,I_MIT_ABM,I_ABMDT_VON,I_ABMDT_BIS,I_MAHNSTUFE,I_STATION,I_STORNO", _
                              Right("0000000000" & m_objUser.KUNNR, 10), UCase(Kennzeichen), UCase(Fahrgestellnummer), m_AnlagedatVon, m_AnlagedatBis, m_WebUser, m_OhneAbmeld, m_MitAbmeld, _
                              m_AbmeldedatVon, m_AbmeldedatBis, m_Mahnstufe, m_Station, m_Stornierte)

            Dim SAPTable As DataTable = S.AP.GetExportTable("GT_UF")

            CreateOutPut(SAPTable, strAppID)

            m_tblUnfallMeldung = Result

            m_tblUnfallMeldung.Columns.Add("Selected", GetType(System.String))
            m_tblUnfallMeldung.Columns.Add("Status", GetType(System.String))
            m_tblUnfallMeldung.Columns.Add("Storniert", GetType(System.String))
            For Each sapRow As DataRow In m_tblUnfallMeldung.Rows
                If sapRow("Stornodatum").ToString.Trim.Length > 0 Then
                    sapRow("Status") = "S"
                    sapRow("Storniert") = "Storniert!"
                ElseIf sapRow("Kennzeicheneingang").ToString.Trim.Length > 0 Then
                    sapRow("Status") = "X"
                Else
                    sapRow("Status") = ""
                End If
            Next
            m_tblUFMeldungExcel = New DataTable

            m_tblUFMeldungExcel.Columns.Add("Anlagedatum", GetType(System.String))
            m_tblUFMeldungExcel.Columns.Add("Webuser", GetType(System.String))
            m_tblUFMeldungExcel.Columns.Add("Kennzeichen", GetType(System.String))
            m_tblUFMeldungExcel.Columns.Add("Fahrgestellnummer", GetType(System.String))
            m_tblUFMeldungExcel.Columns.Add("Erstzulassungsdatum", GetType(System.String))
            m_tblUFMeldungExcel.Columns.Add("Kennzeicheneingang", GetType(System.String))
            m_tblUFMeldungExcel.Columns.Add("Stilllegung", GetType(System.String))
            m_tblUFMeldungExcel.Columns.Add("Station", GetType(System.String))
            m_tblUFMeldungExcel.Columns.Add("Mahnstufe", GetType(System.String))

            For Each ExcelRow As DataRow In m_tblUnfallMeldung.Rows
                Dim NewExcelRow As DataRow = m_tblUFMeldungExcel.NewRow
                NewExcelRow("Anlagedatum") = IIf(IsDate(ExcelRow("Anlagedatum").ToString), Left(ExcelRow("Anlagedatum").ToString, 10), "")
                NewExcelRow("Webuser") = ExcelRow("Webuser").ToString
                NewExcelRow("Kennzeichen") = ExcelRow("Kennzeichen").ToString
                NewExcelRow("Fahrgestellnummer") = ExcelRow("Fahrgestellnummer").ToString
                NewExcelRow("Erstzulassungsdatum") = IIf(IsDate(ExcelRow("Erstzulassungsdatum").ToString), Left(ExcelRow("Erstzulassungsdatum").ToString, 10), "")
                NewExcelRow("Kennzeicheneingang") = IIf(IsDate(ExcelRow("Kennzeicheneingang").ToString), Left(ExcelRow("Kennzeicheneingang").ToString, 10), "")
                NewExcelRow("Stilllegung") = IIf(IsDate(ExcelRow("Stilllegung").ToString), Left(ExcelRow("Stilllegung").ToString, 10), "")
                NewExcelRow("Station") = ExcelRow("Station").ToString
                NewExcelRow("Mahnstufe") = ExcelRow("Mahnstufe").ToString
                m_tblUFMeldungExcel.Rows.Add(NewExcelRow)
            Next

            'Fehlerhandling

            'm_E_SUBRC = myProxy.getExportParameter("E_SUBRC")
            'm_E_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

            m_E_SUBRC = S.AP.GetExportParameter("E_SUBRC")
            m_E_MESSAGE = S.AP.GetExportParameter("E_MESSAGE")

            If m_E_SUBRC <> "0" Then
                m_intStatus = CInt(m_E_SUBRC)
                m_strMessage = m_E_MESSAGE
                WriteLogEntry(False, "I_AG=" & m_objUser.KUNNR & ", I_CHASSIS_NUM=" & UCase(Fahrgestellnummer) & ", I_LICENSE_NUM=" & UCase(Kennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblUnfallMeldung, False)
            End If

            WriteLogEntry(True, "I_AG=" & m_objUser.KUNNR & ", I_CHASSIS_NUM=" & UCase(Fahrgestellnummer) & ", I_LICENSE_NUM=" & UCase(Kennzeichen), m_tblUnfallMeldung, False)

        Catch ex As Exception
            m_intStatus = -2222
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case "NO_WEB"
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case Else
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select

            WriteLogEntry(False, "I_AG=" & m_objUser.KUNNR & ", I_CHASSIS_NUM=" & UCase(Fahrgestellnummer) & ", I_LICENSE_NUM=" & UCase(Kennzeichen) & ", " & Replace(m_strMessage, "<br>", " "), m_tblUnfallMeldung, False)
        End Try
    End Sub


    Public Sub SetStornoMeldung(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Unfallmeldung.SetStornoMeldung"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_strMessage = ""
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_UF_STORNO", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_UNFALL_NR", m_Unfallnummer)
            'myProxy.setImportParameter("I_STORNONAM", m_objUser.UserName)
            'myProxy.setImportParameter("I_STORNOBEM", m_StornoBem)

            'myProxy.callBapi()

            'm_E_SUBRC = myProxy.getExportParameter("E_SUBRC")
            'm_E_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

            S.AP.InitExecute("Z_DPM_UF_STORNO", "I_AG,I_UNFALL_NR,I_STORNONAM,I_STORNOBEM", Right("0000000000" & m_objUser.KUNNR, 10), m_Unfallnummer, m_objUser.UserName, m_StornoBem)

            m_E_SUBRC = S.AP.GetExportParameter("E_SUBRC")
            m_E_MESSAGE = S.AP.GetExportParameter("E_MESSAGE")

            If m_E_SUBRC <> "0" Then
                m_intStatus = CInt(m_E_SUBRC)
                m_strMessage = m_E_MESSAGE
                WriteLogEntry(False, "I_AG=" & m_objUser.KUNNR & ", I_UNFALL_NR=" & UCase(m_Unfallnummer) & ", I_STORNONAM=" & m_objUser.UserName & ", " & ", I_STORNOBEM=" & m_StornoBem & ", " & Replace(m_strMessage, "<br>", " "), m_tblUnfallMeldung, False)
            End If

            WriteLogEntry(True, "I_AG=" & m_objUser.KUNNR & ", I_UNFALL_NR=" & UCase(m_Unfallnummer) & ", I_STORNONAM=" & m_objUser.UserName & ", " & ", I_STORNOBEM=" & m_StornoBem & ", " & Replace(m_strMessage, "<br>", " "), m_tblUnfallMeldung, False)


        Catch ex As Exception
            m_intStatus = -2222
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Eingabedaten vorhanden."
                Case "NO_WEB"
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case Else
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
            WriteLogEntry(False, "I_AG=" & m_objUser.KUNNR & ", I_UNFALL_NR=" & UCase(m_Unfallnummer) & ", I_STORNONAM=" & m_objUser.UserName & ", " & ", I_STORNOBEM=" & m_StornoBem & ", " & Replace(m_strMessage, "<br>", " "), m_tblUnfallMeldung, False)
        End Try
    End Sub
#End Region



End Class
