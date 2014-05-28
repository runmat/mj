Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Public Class CarportErfassung
    Inherits Base.Business.BankBase


#Region "Declarations"
    Private m_tblPositionen As DataTable
    Private m_Kunden As DataTable
    Private m_Kundennummer As String = ""
    Private m_Kundenname As String = ""
    Private m_strE_SUBRC As String
    Private m_strE_MESSAGE As String
#End Region

#Region "Properties"
    Public Property Positionen() As DataTable
        Get
            Return m_tblPositionen
        End Get
        Set(ByVal Value As DataTable)
            m_tblPositionen = Value
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

    Public Property Kundenname() As String
        Get
            Return m_Kundenname
        End Get
        Set(ByVal Value As String)
            m_Kundenname = Value
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

    Public Property E_MESSAGE() As String
        Get
            Return m_strE_MESSAGE
        End Get
        Set(ByVal Value As String)
            m_strE_MESSAGE = Value
        End Set
    End Property

#End Region

#Region "Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "CarportErfassung.Change"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1


            Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)

            If Kundennummer.Length > 0 Then
                strKUNNR = Kundennummer.PadLeft(10, "0"c)
            End If



            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_CARPORT_MELDUNG", m_objApp, m_objUser, page)

                Dim tblTemp As DataTable = myProxy.getImportTable("GT_WEB")

                Dim rowTemp As DataRow
                Dim i As Integer = 0

                For Each rowTemp In m_tblPositionen.Rows
                    Dim NewRow As DataRow = tblTemp.NewRow



                    NewRow("KUNNR_AG") = strKUNNR
                    NewRow("LSNUMMER") = ""
                    NewRow("CARPORT_ID") = m_objUser.Reference
                    NewRow("LICENSE_NUM") = rowTemp("LICENSE_NUM")
                    NewRow("CHASSIS_NUM") = rowTemp("CHASSIS_NUM")
                    NewRow("ZZFABRIKNAME") = rowTemp("ZZFABRIKNAME")
                    NewRow("ZZB1_IN_VERW_DAD") = rowTemp("ZZB1_IN_VERW_DAD")
                    NewRow("DAT_DEMONT") = rowTemp("DAT_DEMONT")
                    NewRow("ANZ_KENNZ_CPL") = rowTemp("ANZ_KENNZ_CPL")
                    NewRow("VORLAGE_ZB1_CPL") = rowTemp("VORLAGE_ZB1_CPL")
                    NewRow("WEB_USER") = rowTemp("WEB_USER")
                    NewRow("LIZNR") = rowTemp("LIZNR")

                    i += 1

                    tblTemp.Rows.Add(NewRow)
                Next

                myProxy.callBapi()

                Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")

                tblTemp2.Columns.Add("VORLAGE_ZB1_CPL_Text", GetType(System.String))
                For Each rowTemp In tblTemp2.Rows

                    Select Case rowTemp("VORLAGE_ZB1_CPL").ToString
                        Case "0"
                            rowTemp("VORLAGE_ZB1_CPL_Text") = "Nein"
                        Case "1"
                            rowTemp("VORLAGE_ZB1_CPL_Text") = "Ja"
                        Case "2"
                            rowTemp("VORLAGE_ZB1_CPL_Text") = "mit Kopie"
                        Case Else

                    End Select
                    tblTemp2.AcceptChanges()

                Next

                m_tblPositionen = New DataTable
                m_tblPositionen = tblTemp2.Copy

                WriteLogEntry(True, "KUNNR=" & strKUNNR, m_tblPositionen)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblPositionen)

            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

    End Sub

    Public Sub CreatePosTable()
        m_tblPositionen = New DataTable
        m_tblPositionen.Columns.Add("ID", GetType(System.String))
        m_tblPositionen.Columns.Add("KUNNR_AG", GetType(System.String))
        m_tblPositionen.Columns.Add("LSNUMMER", GetType(System.String))
        m_tblPositionen.Columns.Add("CARPORT_ID", GetType(System.String))
        m_tblPositionen.Columns.Add("LICENSE_NUM", GetType(System.String))
        m_tblPositionen.Columns.Add("CHASSIS_NUM", GetType(System.String))
        m_tblPositionen.Columns.Add("ZZFABRIKNAME", GetType(System.String))
        m_tblPositionen.Columns.Add("ZZB1_IN_VERW_DAD", GetType(System.String))
        m_tblPositionen.Columns.Add("DAT_DEMONT", GetType(System.String))
        m_tblPositionen.Columns.Add("ANZ_KENNZ_CPL", GetType(System.String))
        m_tblPositionen.Columns.Add("VORLAGE_ZB1_CPL", GetType(System.String))
        m_tblPositionen.Columns.Add("VORLAGE_ZB1_CPL_Text", GetType(System.String))
        m_tblPositionen.Columns.Add("WEB_USER", GetType(System.String))
        m_tblPositionen.Columns.Add("FLAG_FOUND", GetType(System.String))
        m_tblPositionen.Columns.Add("LIZNR", GetType(System.String))
        m_tblPositionen.Columns.Add("FEHLERTEXT", GetType(System.String))

    End Sub

    Public Sub GetData(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "CarportErfassung.GetData"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)

            If Kundennummer.Length > 0 Then
                strKUNNR = Kundennummer.PadLeft(10, "0"c)
            End If


            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_CARPORT_MELDUNG", m_objApp, m_objUser, page)

                Dim tblTemp As DataTable = myProxy.getImportTable("GT_WEB")

                Dim rowTemp As DataRow


                For Each rowTemp In m_tblPositionen.Rows
                    Dim NewRow As DataRow = tblTemp.NewRow

                    NewRow("KUNNR_AG") = strKUNNR
                    NewRow("LSNUMMER") = ""
                    NewRow("CARPORT_ID") = ""
                    NewRow("LICENSE_NUM") = rowTemp("LICENSE_NUM")
                    NewRow("CHASSIS_NUM") = rowTemp("CHASSIS_NUM")
                    NewRow("ZZFABRIKNAME") = ""
                    NewRow("ZZB1_IN_VERW_DAD") = ""
                    NewRow("DAT_DEMONT") = rowTemp("DAT_DEMONT")
                    NewRow("ANZ_KENNZ_CPL") = rowTemp("ANZ_KENNZ_CPL")
                    NewRow("VORLAGE_ZB1_CPL") = rowTemp("VORLAGE_ZB1_CPL")
                    NewRow("WEB_USER") = m_objUser.UserName
                    'NewRow("FLAG_FOUND") = rowTemp("FLAG_FOUND")


                    tblTemp.Rows.Add(NewRow)
                Next

                myProxy.callBapi()

                Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")

                tblTemp2.Columns.Add("VORLAGE_ZB1_CPL_Text", GetType(System.String))
                tblTemp2.Columns.Add("ID", GetType(System.String))

                Dim i As Integer = 0

                For Each rowTemp In tblTemp2.Rows

                    Select Case rowTemp("VORLAGE_ZB1_CPL").ToString
                        Case "0"
                            rowTemp("VORLAGE_ZB1_CPL_Text") = "Nein"
                        Case "1"
                            rowTemp("VORLAGE_ZB1_CPL_Text") = "Ja"
                        Case "2"
                            rowTemp("VORLAGE_ZB1_CPL_Text") = "mit Kopie"
                        Case Else

                    End Select


                    rowTemp("ID") = i


                    tblTemp2.AcceptChanges()
                    i += 1
                Next

                m_tblPositionen = New DataTable
                m_tblPositionen = tblTemp2.Copy

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblPositionen)

            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblPositionen)

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
        m_strClassAndMethod = "CarportErfassung.FillCustomerToPDI"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_strE_SUBRC = ""
        m_strE_MESSAGE = ""

        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_GET_PDI_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_DADPDI", m_objUser.Reference)

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


#End Region

End Class
