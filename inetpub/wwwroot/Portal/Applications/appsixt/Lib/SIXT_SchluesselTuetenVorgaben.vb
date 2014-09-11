Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class SIXT_SchluesselTuetenVorgaben
    REM § Lese-/Schreibfunktion, Kunde: SIXT, 
    REM § Show - BAPI: Z_M_Schluesselweb,
    REM § Change - BAPI: Z_M_Schluesselfuellen.

    Inherits Base.Business.BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private m_tblVorgaben As DataTable
    Private m_intVorgabeID As Int32
    Private m_blnDelete As Boolean
#End Region

#Region " Properties"
    Public Property Delete() As Boolean
        Get
            Return m_blnDelete
        End Get
        Set(ByVal Value As Boolean)
            m_blnDelete = Value
        End Set
    End Property

    Public Property VorgabeID() As Int32
        Get
            Return m_intVorgabeID
        End Get
        Set(ByVal Value As Int32)
            m_intVorgabeID = Value
        End Set
    End Property

    Public Property Vorgaben() As DataTable
        Get
            Return m_tblVorgaben
        End Get
        Set(ByVal Value As DataTable)
            m_tblVorgaben = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overrides Sub Show()
 
    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "SIXT_SchluesselTuetenVorgaben.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_tblVorgaben = New DataTable()
                m_tblVorgaben.Columns.Add("VorgabeID", System.Type.GetType("System.Int32"))
                m_tblVorgaben.Columns.Add("Fahrgestellnummer von", System.Type.GetType("System.String"))
                m_tblVorgaben.Columns.Add("Fahrgestellnummer bis", System.Type.GetType("System.String"))
                m_tblVorgaben.Columns.Add("Hersteller", System.Type.GetType("System.String"))
                m_tblVorgaben.Columns.Add("Modell", System.Type.GetType("System.String"))
                m_tblVorgaben.Columns.Add("Ersatzschlüssel", System.Type.GetType("System.Int32"))
                m_tblVorgaben.Columns.Add("Carpass", System.Type.GetType("System.Int32"))
                m_tblVorgaben.Columns.Add("Radio Codekarte", System.Type.GetType("System.Int32"))
                m_tblVorgaben.Columns.Add("CD-Navigationssystem", System.Type.GetType("System.Int32"))
                m_tblVorgaben.Columns.Add("Chipkarte", System.Type.GetType("System.Int32"))
                m_tblVorgaben.Columns.Add("COC-Papier", System.Type.GetType("System.Int32"))
                m_tblVorgaben.Columns.Add("Navigationssystem Codekarte", System.Type.GetType("System.Int32"))
                m_tblVorgaben.Columns.Add("Codekarte Wegfahrsperre", System.Type.GetType("System.Int32"))
                m_tblVorgaben.Columns.Add("Ersatzfernbedienung Standheizung", System.Type.GetType("System.Int32"))
                m_tblVorgaben.Columns.Add("Prüfbuch bei LKW", System.Type.GetType("System.Int32"))

                m_intStatus = 0
                m_strMessage = ""

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_SCHLUESSELWEB", m_objApp, m_objUser, page)

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_SCHLUESSELWEB")

                'Dim CarDetailTable As DataTable = myProxy.getExportTable("GT_WEB")
                Dim CarDetailTable As DataTable = S.AP.GetExportTable("GT_WEB")

                If CarDetailTable.Rows.Count > 0 Then
                    Dim rowNew As DataRow
                    Dim intVorgabeID As Int32 = 1
                    For Each CarDetail As DataRow In CarDetailTable.Rows
                        rowNew = m_tblVorgaben.NewRow
                        rowNew("VorgabeID") = CarDetail("Autowert")
                        rowNew("Fahrgestellnummer von") = CarDetail("Chassis_Num")
                        rowNew("Fahrgestellnummer bis") = CarDetail("Chassis_Num_Bis")
                        rowNew("Modell") = CarDetail("Modell")
                        rowNew("Hersteller") = CarDetail("Hersteller")
                        If IsNumeric(CarDetail("Ersschluessel")) Then
                            rowNew("Ersatzschlüssel") = CInt(CarDetail("Ersschluessel"))
                        Else
                            rowNew("Ersatzschlüssel") = 0
                        End If
                        If IsNumeric(CarDetail("Carpass")) Then
                            rowNew("Carpass") = CInt(CarDetail("Carpass"))
                        Else
                            rowNew("Carpass") = 0
                        End If
                        If IsNumeric(CarDetail("Radiocodekarte")) Then
                            rowNew("Radio Codekarte") = CInt(CarDetail("Radiocodekarte"))
                        Else
                            rowNew("Radio Codekarte") = 0
                        End If
                        If IsNumeric(CarDetail("Navicd")) Then
                            rowNew("CD-Navigationssystem") = CInt(CarDetail("Navicd"))
                        Else
                            rowNew("CD-Navigationssystem") = 0
                        End If
                        If IsNumeric(CarDetail("Chipkarte")) Then
                            rowNew("Chipkarte") = CInt(CarDetail("Chipkarte"))
                        Else
                            rowNew("Chipkarte") = 0
                        End If
                        If IsNumeric(CarDetail("Cocbesch")) Then
                            rowNew("COC-Papier") = CInt(CarDetail("Cocbesch"))
                        Else
                            rowNew("COC-Papier") = 0
                        End If
                        If IsNumeric(CarDetail("Navicodekarte")) Then
                            rowNew("Navigationssystem Codekarte") = CInt(CarDetail("Navicodekarte"))
                        Else
                            rowNew("Navigationssystem Codekarte") = 0
                        End If
                        If IsNumeric(CarDetail("Wfscodekarte")) Then
                            rowNew("Codekarte Wegfahrsperre") = CInt(CarDetail("Wfscodekarte"))
                        Else
                            rowNew("Codekarte Wegfahrsperre") = 0
                        End If
                        If IsNumeric(CarDetail("Sh_Ers_Fb")) Then
                            rowNew("Ersatzfernbedienung Standheizung") = CInt(CarDetail("Sh_Ers_Fb"))
                        Else
                            rowNew("Ersatzfernbedienung Standheizung") = 0
                        End If
                        If IsNumeric(CarDetail("Pruefbuch_Lkw")) Then
                            rowNew("Prüfbuch bei LKW") = CInt(CarDetail("Pruefbuch_Lkw"))
                        Else
                            rowNew("Prüfbuch bei LKW") = 0
                        End If
                        m_tblVorgaben.Rows.Add(rowNew)
                        intVorgabeID += 1
                    Next
                    WriteLogEntry(True, "", m_tblVorgaben)
                End If
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
                WriteLogEntry(False, m_strMessage, m_tblVorgaben)
            Finally
                m_blnGestartet = False
            End Try

            If Not m_intStatus = 0 Then
                Throw New System.Exception(m_strMessage)
            End If
        End If
    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        If Not m_blnGestartet Then
            m_blnGestartet = True
            m_intStatus = 0
            m_strMessage = ""

            Dim tmpDataView As DataView
            tmpDataView = m_tblVorgaben.DefaultView

            tmpDataView.RowFilter = "VorgabeID = " & m_intVorgabeID.ToString

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_SCHLUESSELFUELLEN", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", m_strCustomer)

                S.AP.Init("Z_M_SCHLUESSELFUELLEN", "I_KUNNR", m_strCustomer)

                'Dim CarDetailTable As DataTable = myProxy.getImportTable("GS_WEB_IN")
                Dim CarDetailTable As DataTable = S.AP.GetImportTable("GS_WEB_IN")

                Dim CarDetail As DataRow
                CarDetail = CarDetailTable.NewRow

                CarDetail("Kunnr") = m_strCustomer
                If Not m_intVorgabeID = -1 Then
                    CarDetail("Autowert") = m_intVorgabeID
                End If
                If m_blnDelete Then
                    CarDetail("Flag") = "X"
                Else
                    CarDetail("Flag") = " "
                End If
                CarDetail("Chassis_Num") = CType(tmpDataView.Item(0)("Fahrgestellnummer von"), String)
                CarDetail("Chassis_Num_Bis") = CType(tmpDataView.Item(0)("Fahrgestellnummer bis"), String)
                CarDetail("Modell") = CType(tmpDataView.Item(0)("Modell"), String)
                CarDetail("Hersteller") = CType(tmpDataView.Item(0)("Hersteller"), String)
                CarDetail("Ersschluessel") = CType(tmpDataView.Item(0)("Ersatzschlüssel"), String)
                CarDetail("Carpass") = CType(tmpDataView.Item(0)("Carpass"), String)
                CarDetail("Radiocodekarte") = CType(tmpDataView.Item(0)("Radio Codekarte"), String)
                CarDetail("Navicd") = CType(tmpDataView.Item(0)("CD-Navigationssystem"), String)
                CarDetail("Chipkarte") = CType(tmpDataView.Item(0)("Chipkarte"), String)
                CarDetail("Cocbesch") = CType(tmpDataView.Item(0)("COC-Papier"), String)
                CarDetail("Navicodekarte") = CType(tmpDataView.Item(0)("Navigationssystem Codekarte"), String)
                CarDetail("Wfscodekarte") = CType(tmpDataView.Item(0)("Codekarte Wegfahrsperre"), String)
                CarDetail("Sh_Ers_Fb") = CType(tmpDataView.Item(0)("Ersatzfernbedienung Standheizung"), String)
                CarDetail("Pruefbuch_Lkw") = CType(tmpDataView.Item(0)("Prüfbuch bei LKW"), String)

                CarDetailTable.Rows.Add(CarDetail)

                'myProxy.callBapi()
                S.AP.Execute()

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_KUNNR"
                        m_intStatus = -2271
                        m_strMessage = "Fehler: Kundennummer nicht korrekt."
                    Case "NO_KORREKTRELATION"
                        m_intStatus = -2272
                        m_strMessage = "Fehler: Fahrgestellnummer VON ist größer Fahrgestellnummer BIS."
                    Case "NO_DATENSATZ"
                        m_intStatus = -2273
                        m_strMessage = "Fehler: Kein Datensatz vorhanden."
                    Case "NO_UEBERSCHNEIDUNGEN"
                        m_intStatus = -2274
                        m_strMessage = "Fehler: Es liegt eine Überschneidung mit existierenden Daten vor."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Fehler bei der Speicherung der Vorgänge (" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try

            If Not m_intStatus = 0 Then
                Throw New System.Exception(m_strMessage)
            End If
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: SIXT_SchluesselTuetenVorgaben.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA: 2918
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:38
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 8  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
