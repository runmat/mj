Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Public Class Klaerfaelle
    Inherits Base.Business.BankBase

#Region "Declarations"
    Private m_strCarportID As String
    Private m_strModus As String
    Private m_strSelectedKennz As String
    Private m_strKennz As String
    Private m_strLiefernr As String
    Private m_strFahrgestnr As String
    Private m_strE_SUBRC As String
    Private m_strE_MESSAGE As String
    Private m_strKlaerfalltext As String
    Private m_tblResultPDIs As DataTable
    Private m_Kunden As DataTable
    Private m_Kundennummer As String = ""

#End Region

#Region "Properties"


    Public Property ResultPDIs() As DataTable
        Get
            Return m_tblResultPDIs
        End Get
        Set(ByVal Value As DataTable)
            m_tblResultPDIs = Value
        End Set
    End Property

    Public Property CarportID() As String
        Get
            Return m_strCarportID
        End Get
        Set(ByVal Value As String)
            m_strCarportID = Value
        End Set
    End Property

    Public Property Modus() As String
        Get
            Return m_strModus
        End Get
        Set(ByVal Value As String)
            m_strModus = Value
        End Set
    End Property

    Public Property E_SUBRC() As String
        Get
            Return m_strE_SUBRC
        End Get
        Set(ByVal Value As String)
            m_strE_SUBRC = Value
        End Set
    End Property
    Public Property SelectedKennz() As String
        Get
            Return m_strSelectedKennz
        End Get
        Set(ByVal Value As String)
            m_strSelectedKennz = Value
        End Set
    End Property
    Public Property Klaerfalltext() As String
        Get
            Return m_strKlaerfalltext
        End Get
        Set(ByVal Value As String)
            m_strKlaerfalltext = Value
        End Set
    End Property
    Public Property E_MESSAGE() As String
        Get
            Return m_strE_MESSAGE
        End Get
        Set(ByVal Value As String)
            m_strE_MESSAGE = Value
        End Set
    End Property
    Public Property Kennz() As String
        Get
            Return m_strKennz
        End Get
        Set(ByVal Value As String)
            m_strKennz = Value
        End Set
    End Property
    Public Property Fahrgestnr() As String
        Get
            Return m_strFahrgestnr
        End Get
        Set(ByVal Value As String)
            m_strFahrgestnr = Value
        End Set
    End Property
    Public Property Liefernr() As String
        Get
            Return m_strLiefernr
        End Get
        Set(ByVal Value As String)
            m_strLiefernr = Value
        End Set
    End Property

    Public Property Kunden() As DataTable
        Get
            Return m_Kunden
        End Get
        Set(ByVal Value As DataTable)
            m_Kunden = Value
        End Set
    End Property

    Public Property Kundennummer() As String
        Get
            Return m_Kundennummer
        End Get
        Set(ByVal Value As String)
            m_Kundennummer = Value
        End Set
    End Property

#End Region

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Klaerfaelle.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_strE_SUBRC = ""
        m_strE_MESSAGE = ""
        m_intStatus = 0

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_KLAERFALL_CARPORT", m_objApp, m_objUser, page)


                If Kundennummer.Length > 0 Then
                    myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & Kundennummer, 10))
                Else
                    myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                End If


                myProxy.setImportParameter("I_MODUS", m_strModus)
                myProxy.setImportParameter("I_CARPORT", m_strCarportID)
                If m_strModus = "E" Then
                    myProxy.setImportParameter("I_LICENSE_NUM", m_strKennz)
                    myProxy.setImportParameter("I_CHASSIS_NUM", m_strFahrgestnr)
                    myProxy.setImportParameter("I_LIEFNR", m_strLiefernr)
                End If
                myProxy.callBapi()

                m_tblResultPDIs = myProxy.getExportTable("GT_WEB")

                m_strE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                m_strE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

                'CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", I_CARPORT=" & m_strE_MESSAGE & ", I_MODUS=" & m_strModus, m_tblResult)
            Catch ex As Exception
                m_intStatus = -3333
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", I_CARPORT=" & m_strE_MESSAGE & ", I_MODUS=" & m_strModus, m_tblResult)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Klaerfaelle.Change"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_strE_SUBRC = ""
        m_strE_MESSAGE = ""
        m_intStatus = 0

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_KLAERFALL_CARPORT", m_objApp, m_objUser, page)


                If Kundennummer.Length > 0 Then
                    myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & Kundennummer, 10))
                Else
                    myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                End If

                myProxy.setImportParameter("I_MODUS", m_strModus)
                myProxy.setImportParameter("I_CARPORT", m_strCarportID)
                If m_strModus = "S" Then
                    Dim tmpTable As DataTable = myProxy.getImportTable("GT_WEB")
                    Dim dataRow() As DataRow = ResultPDIs.Select("LICENSE_NUM = '" & SelectedKennz & "'")
                    If dataRow.Length = 1 Then

                        Dim NewRow As DataRow = tmpTable.NewRow

                        NewRow("CARPORT_ID") = dataRow(0)("CARPORT_ID").ToString
                        NewRow("LIEFNR") = dataRow(0)("LIEFNR").ToString
                        NewRow("CHASSIS_NUM") = dataRow(0)("CHASSIS_NUM").ToString
                        NewRow("LICENSE_NUM") = dataRow(0)("LICENSE_NUM").ToString
                        If IsDate(dataRow(0)("DAT_IMP").ToString) Then
                            NewRow("DAT_IMP") = dataRow(0)("DAT_IMP").ToString
                        End If
                        If IsDate(dataRow(0)("DAT_DEMONT").ToString) Then
                            NewRow("DAT_DEMONT") = dataRow(0)("DAT_DEMONT").ToString
                        End If
                        NewRow("ANZ_KENNZ_CPL") = dataRow(0)("ANZ_KENNZ_CPL").ToString
                        NewRow("VORLAGE_ZB1_CPL") = dataRow(0)("VORLAGE_ZB1_CPL").ToString
                        If IsDate(dataRow(0)("DAT_ANLAGE_ABW").ToString) Then
                            NewRow("DAT_ANLAGE_ABW") = dataRow(0)("DAT_ANLAGE_ABW").ToString
                        End If
                        NewRow("TEXT_KLAERF_DAD") = dataRow(0)("TEXT_KLAERF_CARP").ToString

                        NewRow("BEARB_KLAER_CARP") = Now.ToString


                        NewRow("TEXT_KLAERF_CARP") = m_strKlaerfalltext
                        If IsDate(dataRow(0)("ABSCHL_KLAER_DAD").ToString) Then
                            NewRow("ABSCHL_KLAER_DAD") = dataRow(0)("ABSCHL_KLAER_DAD").ToString
                        End If


                        tmpTable.Rows.Add(NewRow)
                    Else
                        m_intStatus = -9999
                        m_strE_MESSAGE = "Fehler beim Speichern der Daten!"
                        WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", I_CARPORT=" & m_strCarportID & ", I_MODUS=" & m_strModus, m_tblResult)
                        Exit Sub
                    End If
                End If

                myProxy.callBapi()

                m_strE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                m_strE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

                'CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", I_CARPORT=" & m_strCarportID & ", I_MODUS=" & m_strModus, m_tblResult)
            Catch ex As Exception
                m_intStatus = -3333
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", I_CARPORT=" & m_strCarportID & ", I_MODUS=" & m_strModus, m_tblResult)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()

    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Sub FillCustomerToPDI(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Klaerfaelle.FillCustomerToPDI"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_strE_SUBRC = ""
        m_strE_MESSAGE = ""
        m_intStatus = 0

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_GET_PDI_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_DADPDI", m_strCarportID)
                
                myProxy.callBapi()

                m_Kunden = myProxy.getExportTable("GT_PDI")

              
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_Kunden)
            Catch ex As Exception
                m_intStatus = -3333
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Es wurden keine Kunden gefunden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", I_CARPORT=" & m_strE_MESSAGE, m_Kunden)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub


End Class