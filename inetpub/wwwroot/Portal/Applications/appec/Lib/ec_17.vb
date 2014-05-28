Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Configuration
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common
Imports System.Data.OleDb

Public Class ec_17
    REM § Lese-/Schreibfunktion, Kunde: Arval, 
    REM § Show - BAPI: Z_M_ZULF_FZGE_ARVAL,
    REM § Change - BAPI: ?.

    Inherits BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private strUploadFile As String
    Private mModel As DataTable
#End Region

#Region " Properties"

    Public Property PUploadfile() As String
        Get
            Return strUploadFile
        End Get
        Set(ByVal Value As String)
            strUploadFile = Value
        End Set
    End Property

    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblResult
        End Get
        Set(ByVal Value As DataTable)
            m_tblResult = Value
        End Set
    End Property

    Public Property Model() As DataTable
        Get
            Return mModel
        End Get
        Set(ByVal Value As DataTable)
            mModel = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overrides Sub Show()
    End Sub

    Public Overrides Sub Change()
    End Sub

    Public Sub GiveCars(ByVal tblFahrzeuge As DataTable, ByVal page As Page)
        Dim tableFahrzeuge As New DataTable()
        Dim tblTemp As DataTable

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Status_Greenway", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            S.AP.Init("Z_M_Ec_Avm_Status_Greenway", "I_KUNNR", m_strKUNNR)

            Dim TempTable As DataTable = S.AP.GetImportTable("GT_WEB") 'myProxy.getImportTable("GT_WEB")
            Dim tmpNewRow As DataRow

            For Each tmpRow As DataRow In tblFahrzeuge.Rows
                tmpNewRow = TempTable.NewRow
                tmpNewRow("CHASSIS_NUM") = tmpRow("CHASSIS_NUM")
                tmpNewRow("LACKIERUNG") = tmpRow("LACKIERUNG")
                tmpNewRow("RECHNUNGSDATUM") = tmpRow("RECHNUNGSDATUM")
                tmpNewRow("MODEL_ID") = tmpRow("MODEL_ID")
                tmpNewRow("DATAB") = tmpRow("DATAB")
                tmpNewRow("ZZDAT_EIN") = tmpRow("ZZDAT_EIN")
                tmpNewRow("ZZCARPORT") = tmpRow("ZZCARPORT")
                tmpNewRow("ZZREFERENZ1") = tmpRow("ZZREFERENZ1")
                tmpNewRow("TIDNR") = tmpRow("TIDNR")
                TempTable.Rows.Add(tmpNewRow)
            Next

            'myProxy.callBapi()
            S.AP.Execute()

            tblTemp = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
  
            m_tblResult = CreateOutPut(tblTemp, m_strAppID)

            m_intStatus = 0

            If (m_tblResult Is Nothing) OrElse (m_tblResult.Rows.Count = 0) Then
                m_intStatus = -3331
                m_strMessage = "Keine Daten gefunden."
            End If

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -3331
                    m_strMessage = "Keine Daten gefunden."
                Case "NO_BAPI"
                    m_intStatus = -3332
                    m_strMessage = "Keine Eintrag in Prüftabelle vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub


    Public Sub SetAvis(ByVal page As Page)
        Dim tblImp As DataTable

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_IMP_MODELL_ID_01", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_WEB_USER", m_objUser.UserName)

            S.AP.Init("Z_DPM_IMP_MODELL_ID_01", "I_AG,I_WEB_USER", m_strKUNNR, m_objUser.UserName)

            tblImp = S.AP.GetImportTable("GT_IN") 'myProxy.getImportTable("GT_IN")

            Dim NewRow As DataRow

            For Each dr As DataRow In Model.Rows

                NewRow = tblImp.NewRow

                NewRow("CHASSIS_NUM") = dr("Fahrgestellnummer")
                NewRow("ZMODELL") = dr("Model")

                tblImp.Rows.Add(NewRow)

            Next

            tblImp.AcceptChanges()


            'myProxy.callBapi()
            S.AP.Execute()

            m_tblResult = S.AP.GetExportTable("GT_OUT") 'myProxy.getExportTable("GT_OUT")


            m_intStatus = 0

            

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -3331
                    m_strMessage = "Keine Daten gefunden."
                Case "NO_BAPI"
                    m_intStatus = -3332
                    m_strMessage = "Keine Eintrag in Prüftabelle vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub
    


    Public Sub setMatch(ByVal tblName As String, ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_EC_AVM_STATUS_GREENWAY", m_objApp, m_objUser, page)

        Dim tblFahrzeugUpload As DataTable
        Dim rowFahrzeuge As DataRow
        Dim rowData As DataRow
        Dim strChassisNum As String             'Spalte 1 : Fahrgestellnummer
        Dim strLack As String = ""          'Spalte 2 : Lackierung
        Dim strRechnungsdatum As String = ""   'Spalte 3 : Rechnungsnummer
        Dim strModell As String = ""     'Spalte 4 : Modell-ID

        Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                 "Data Source=" & strUploadFile & ";" & _
                 "Extended Properties=""Excel 8.0;HDR=NO;"""

        Dim objConn As New OleDbConnection(sConnectionString)
        objConn.Open()

        Dim objCmdSelect As New OleDbCommand("SELECT * FROM  [" & tblName & "$]", objConn)

        Dim objAdapter1 As New OleDbDataAdapter()
        objAdapter1.SelectCommand = objCmdSelect

        Dim objDataset1 As New DataSet()
        objAdapter1.Fill(objDataset1, "XLData")

        Dim logApp As New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        logApp.InitEntry(m_objUser.UserName, m_strSessionID, CInt(m_strAppID), m_objUser.Applications.Select("AppID = '" & m_strAppID & "'")(0)("AppFriendlyName").ToString, m_objUser.CustomerName, m_objUser.Customer.CustomerId, m_objUser.IsTestUser, 0)



        Dim i As Long

        i = 1

        tblFahrzeugUpload = S.AP.GetImportTableWithInit("Z_M_EC_AVM_STATUS_GREENWAY.GT_WEB", "") 'myProxy.getImportTable("GT_WEB")


        For Each rowData In objDataset1.Tables(0).Rows

            If Not (TypeOf rowData(0) Is System.DBNull) Then

                If i > 1 Then
                    strChassisNum = CStr(rowData(0))
                    If Not (TypeOf rowData(1) Is System.DBNull) Then
                        strLack = CStr(rowData(1))
                    End If
                    If Not (TypeOf rowData(2) Is System.DBNull) Then
                        strRechnungsdatum = CStr(rowData(2))
                    End If
                    If Not (TypeOf rowData(3) Is System.DBNull) Then
                        strModell = CStr(rowData(3))
                    End If
                    rowFahrzeuge = tblFahrzeugUpload.NewRow

                    rowFahrzeuge("Chassis_Num") = strChassisNum
                    rowFahrzeuge("Lackierung") = strLack
                    rowFahrzeuge("Rechnungsdatum") = strRechnungsdatum
                    rowFahrzeuge("Model_Id") = strModell

                    tblFahrzeugUpload.Rows.Add(rowFahrzeuge)
                End If
            End If

            i = i + 1
        Next
        objConn.Close()
        GiveCars(tblFahrzeugUpload, New Page)
    End Sub

    Public Sub setData(ByVal tblName As String)
        Dim rowData As DataRow

        Dim strUnitnum As String                'Spalte 1 : Unitnumber
        Dim strChassisNum As String             'Spalte 2 : Fahrgestellnummer
        Dim strModellID As String = ""               'Spalte 3 : Modell-ID
        Dim strModell As String = ""                'Spalte 4 : Modell-Bez
        Dim strPDIEingang As String = ""           'Spalte 5 : PDI-Eingang
        Dim strPDIBereit As String = ""            'Spalte 6 : PDI-Bereit
        Dim strKennz As String = ""             'Spalte 7 : Kennzeichen
        Dim strZuldate As String = ""             'Spalte 8 : Zulassungsdatum

        Dim sConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                 "Data Source=" & strUploadFile & ";" & _
                 "Extended Properties=""Excel 8.0;HDR=NO;"""

        Dim objConn As New OleDbConnection(sConnectionString)
        objConn.Open()

        Dim objCmdSelect As New OleDbCommand("SELECT * FROM  [" & tblName & "$]", objConn)

        Dim objAdapter1 As New OleDbDataAdapter()
        objAdapter1.SelectCommand = objCmdSelect

        Dim objDataset1 As New DataSet()
        objAdapter1.Fill(objDataset1, "XLData")

        Dim logApp As New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        logApp.InitEntry(m_objUser.UserName, m_strSessionID, CInt(m_strAppID), m_objUser.Applications.Select("AppID = '" & m_strAppID & "'")(0)("AppFriendlyName").ToString, m_objUser.CustomerName, m_objUser.Customer.CustomerId, m_objUser.IsTestUser, 0)

        Dim i As Long

        i = 1

        For Each rowData In objDataset1.Tables(0).Rows

            If Not (TypeOf rowData(0) Is System.DBNull) Then

                If i > 1 Then
                    strUnitnum = CStr(rowData(0))
                    strChassisNum = CStr(rowData(1))
                    If Not (TypeOf rowData(2) Is System.DBNull) Then
                        strModellID = CStr(rowData(2))
                    End If
                    If Not (TypeOf rowData(3) Is System.DBNull) Then
                        strModell = CStr(rowData(3))
                    End If

                End If
            End If

            i = i + 1
        Next
        objConn.Close()
        LoadCars(New Page)
    End Sub


    Public Sub LoadCars(ByVal page As Page)
        Dim tableFahrzeuge As New DataTable()
        Dim tblTemp As DataTable

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_FAHRZEUG_STATUS_001", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()

            S.AP.InitExecute("Z_V_FAHRZEUG_STATUS_001", "I_KUNNR", m_strKUNNR)

            tblTemp = S.AP.GetExportTable("DATEN_TAB") 'myProxy.getExportTable("DATEN_TAB")

            m_tblResult = CreateOutPut(tblTemp, m_strAppID)

            m_intStatus = 0

            If (m_tblResult Is Nothing) OrElse (m_tblResult.Rows.Count = 0) Then
                m_intStatus = -3331
                m_strMessage = "Keine Daten gefunden."
            End If

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = -3331
                    m_strMessage = "Keine Daten gefunden."
                Case "NO_BAPI"
                    m_intStatus = -3332
                    m_strMessage = "Keine Eintrag in Prüftabelle vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)
        End Try
    End Sub

   
#End Region
End Class

' ************************************************
' $History: ec_17.vb $
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 24.05.11   Time: 20:04
' Updated in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 29.07.09   Time: 11:20
' Updated in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 29.06.09   Time: 9:23
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918 Z_M_Fehlend_Unfall_001, Z_M_Schlue_Set_Mahnsp_001
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 26.06.09   Time: 15:28
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918 unfertig, nicht kompilierbar
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 26.06.09   Time: 14:53
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918, Z_M_Ec_Avm_Zulauf, Z_V_FAHRZEUG_STATUS_001,
' Z_M_EC_AVM_STATUS_BESTAND, Z_M_ABMBEREIT_LAUFAEN,
' Z_M_ABMBEREIT_LAUFZEIT, Z_M_Brief_Temp_Vers_Mahn_001,
' Z_M_SCHLUE_SET_MAHNSP_001, Z_M_SCHLUESSELDIFFERENZEN,
' Z_M_SCHLUESSELVERLOREN, Z_M_SCHLUE_TEMP_VERS_MAHN_001,
' Z_M_Ec_Avm_Status_Zul,  Z_M_ECA_TAB_BESTAND
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 26.06.09   Time: 11:29
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918
' Z_M_EC_AVM_BRIEFLEBENSLAUF,Z_M_Ec_Avm_Fzg_M_Dfs_O_Zul,Z_M_EC_AVM_FZG_OH
' NE_BRIEF,Z_M_Ec_Avm_Fzg_Ohne_Unitnr,Z_M_Ec_Avm_Nur_Brief_Vorh,
' Z_M_EC_AVM_OFFENE_ZAHLUNGEN,  Z_M_EC_AVM_PDI_BESTAND,
' Z_M_EC_AVM_STATUS_EINSTEUERUNG,  Z_M_EC_AVM_STATUS_GREENWAY,
' Z_M_Ec_Avm_Status_Zul, Z_M_EC_AVM_ZULASSUNGEN, Z_M_Ec_Avm_Zulassungen_2
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Lib
' Warnungen entfernt!
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 10.08.07   Time: 11:26
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Bugfix: Zugriff auf ExcelTabelle geändert AppEC- ec_17
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 17:23
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 21.06.07   Time: 8:50
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' ITA: 1050
' 
' *****************  Version 3  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' ************************************************
