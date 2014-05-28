Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

<Serializable()> Public Class fin_11
    REM §  BAPI: Z_M_Gesperrte_Auftraege_001
    REM §  BAPI:Z_M_Freigeben_Auftrag_001
    Inherits Base.Business.BankBase

#Region " Declarations"
    Dim strKontingentart As String
    Dim strEquinr As String
    Dim strVBELN As String
    Dim strStorno As String
    Dim strHaendler As String
    Dim strFahrgestellnr As String
    Dim strStornoGrund1 As String = ""
    Dim strStornoGrund2 As String = ""
    Dim strStornoGrund3 As String = ""
    Dim strStornoGrund4 As String = ""
    Dim mFreigabeUser As String
    Dim mFreigabeDatum As String
    Dim mFreigabeUhrzeit As String
    Dim mAutorisierungUser As String = ""
#End Region

#Region " Properties"
    Public Property Kontingentart() As String
        Get
            Return strKontingentart
        End Get
        Set(ByVal Value As String)
            strKontingentart = Value
        End Set
    End Property


    Public Property VBELN() As String
        Get
            Return strVBELN
        End Get
        Set(ByVal Value As String)
            strVBELN = Value
        End Set
    End Property
    Public ReadOnly Property FreigabeUser() As String
        Get
            Return mFreigabeUser
        End Get

    End Property
    Public Property AutorisierungUser() As String
        Get
            Return mAutorisierungUser
        End Get
        Set(ByVal Value As String)
            mAutorisierungUser = Value
        End Set
    End Property
    Public ReadOnly Property FreigabeDatum() As String
        Get
            Return mFreigabeDatum
        End Get

    End Property
    Public ReadOnly Property FreigabeUhrzeit() As String
        Get
            Return mFreigabeUhrzeit
        End Get

    End Property


   

    Public Property Equinr() As String
        Get
            Return strEquinr
        End Get
        Set(ByVal Value As String)
            strEquinr = Value
        End Set
    End Property

    Public Property Storno() As String
        Get
            Return strStorno
        End Get
        Set(ByVal Value As String)
            strStorno = Value
        End Set
    End Property

    Public Property StornoGrund1() As String
        Get
            Return strStornoGrund1
        End Get
        Set(ByVal Value As String)
            strStornoGrund1 = Value
        End Set
    End Property
    Public Property StornoGrund2() As String
        Get
            Return strStornoGrund2
        End Get
        Set(ByVal Value As String)
            strStornoGrund2 = Value
        End Set
    End Property
    Public Property StornoGrund3() As String
        Get
            Return strStornoGrund3
        End Get
        Set(ByVal Value As String)
            strStornoGrund3 = Value
        End Set
    End Property
    Public Property StornoGrund4() As String
        Get
            Return strStornoGrund4
        End Get
        Set(ByVal Value As String)
            strStornoGrund4 = Value
        End Set
    End Property



    Public Property Haendlernummer() As String
        Get
            Return strHaendler
        End Get
        Set(ByVal Value As String)
            strHaendler = Value
        End Set
    End Property

    Public Property Fahrgestellnr() As String
        Get
            Return strFahrgestellnr
        End Get
        Set(ByVal Value As String)
            strFahrgestellnr = Value
        End Set
    End Property


#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overloads Overrides Sub show()

    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "fin_11.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        m_intStatus = 0
        Try
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            S.AP.InitExecute("Z_M_Gesperrte_Auftraege_001   ", "I_AG, I_HAENDLER, I_VKORG, I_ZZFAHRG",
                             Right("0000000000" & m_objUser.KUNNR, 10), strHaendler, "1510", strFahrgestellnr)

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")
            tblTemp2.Columns("BSTZD").MaxLength = 25
            tblTemp2.AcceptChanges()

            'X für die column Translation 
            tblTemp2.Columns.Add("StornoGrundX", GetType(System.String))
            tblTemp2.Columns.Add("AbrufGrundX", GetType(System.String))
            tblTemp2.Columns.Add("VersandadresseX", GetType(System.String))

            Dim rowTemp As DataRow

            For Each rowTemp In tblTemp2.Rows
                'im sap gibt es 4 x 27 Felder für den StornoGrund, beschissen aber wahr 

                'wenn dbNull dann auf String.empty Setzetn, sonst Exception
                '------------------------------------------------
                If rowTemp("ZBE01") Is DBNull.Value Then
                    rowTemp("ZBE01") = String.Empty
                End If

                If rowTemp("ZBE02") Is DBNull.Value Then
                    rowTemp("ZBE02") = String.Empty
                End If

                If rowTemp("ZBE03") Is DBNull.Value Then
                    rowTemp("ZBE03") = String.Empty
                End If

                If rowTemp("ZBE04") Is DBNull.Value Then
                    rowTemp("ZBE04") = String.Empty
                End If

                If rowTemp("AUGRU") Is DBNull.Value Then
                    rowTemp("AUGRU") = String.Empty
                End If

                If rowTemp("NAME1") Is DBNull.Value Then
                    rowTemp("NAME1") = String.Empty
                End If

                If rowTemp("NAME2") Is DBNull.Value Then
                    rowTemp("NAME2") = String.Empty
                End If

                If rowTemp("CITY1") Is DBNull.Value Then
                    rowTemp("CITY1") = String.Empty
                End If

                If rowTemp("POST_CODE1") Is DBNull.Value Then
                    rowTemp("POST_CODE1") = String.Empty
                End If

                If rowTemp("STREET") Is DBNull.Value Then
                    rowTemp("STREET") = String.Empty
                End If

                If rowTemp("HOUSE_NUM1") Is DBNull.Value Then
                    rowTemp("HOUSE_NUM1") = String.Empty
                End If

                If rowTemp("COUNTRY") Is DBNull.Value Then
                    rowTemp("COUNTRY") = String.Empty
                End If

                '------------------------------------------------

                rowTemp("StornoGrundX") = CStr(rowTemp("ZBE01")) & CStr(rowTemp("ZBE02")) & CStr(rowTemp("ZBE03")) & CStr(rowTemp("ZBE04"))
                'abrufgrund aus sqlDatenbank laut SAP Kürzel abrufen
                rowTemp("AbrufGrundX") = getAbrufgrund(CStr(rowTemp("AUGRU")))

                Select Case CStr(rowTemp("BSTZD"))
                    Case "0001"
                        rowTemp("BSTZD") = "Standard temporär"
                    Case "0002"
                        rowTemp("BSTZD") = "Standard endgültig"
                    Case "0005"
                        rowTemp("BSTZD") = "Händler Zulassung"
                End Select
                'versandadresse Zusammenbauen
                rowTemp("VersandadresseX") = CStr(rowTemp("NAME1")) & " " & CStr(rowTemp("NAME2")) & " - " & CStr(rowTemp("STREET")) & " " & CStr(rowTemp("HOUSE_NUM1")) & " - " & CStr(rowTemp("POST_CODE1")) & " " & CStr(rowTemp("CITY1")) & " - " & CStr(rowTemp("COUNTRY"))

            Next

            'entfernen der rows die in sich in authorisation befinden
            '------------------------------
            Dim tmpDt As DataTable
            Dim i As Int32 = 0
            tmpDt = CreateOutPut(tblTemp2, strAppID)
            tmpDt.Columns.Add("Status", Type.GetType("System.String"))

            'initialbelegung der Statusspalte mit string.empty

            For Each rowTemp In tmpDt.Rows
                rowTemp("Status") = ""
            Next
            m_tblResult = tmpDt.Copy
            For Each rowTemp In tmpDt.Rows
                Dim intAutID As Int32 = 0
                Dim strInitiator As String = ""
                m_objApp.CheckForPendingAuthorization(CInt(m_strAppID), m_objUser.Organization.OrganizationId, CStr(rowTemp("Haendlernummer")), CStr(rowTemp("Fahrgestellnummer")), m_objUser.IsTestUser, strInitiator, intAutID)
                If intAutID > 0 Then
                    m_tblResult.Rows.RemoveAt(i)
                Else
                    i = i + 1
                End If
            Next
            '------------------------------

        Catch ex As Exception
            m_intStatus = -9999

            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            If errormessage.Contains("NO_DATA") Then
                m_strMessage = "Keine Daten vorhanden."
            ElseIf errormessage.Contains("NO_WEB") Then
                m_strMessage = "Keine Web-Tabelle erstellt."
            ElseIf errormessage.Contains("NO_HAENDLER") Then
                m_strMessage = "Händler nicht vorhanden."
            Else
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End If

        End Try
    End Sub

    Private Function getAbrufgrund(ByVal kuerzel As String) As String
        Dim cn As New SqlClient.SqlConnection
        Dim cmdAg As SqlClient.SqlCommand
        Dim dsAg As DataSet
        Dim adAg As SqlClient.SqlDataAdapter
        Dim dr As SqlClient.SqlDataReader
        Dim sAbrufgrund As String = ""
        Try

            cn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            cn.Open()
            dsAg = New DataSet()
            adAg = New SqlClient.SqlDataAdapter()
            cmdAg = New SqlClient.SqlCommand("SELECT " & _
                                            "[WebBezeichnung]" & _
                                            "FROM CustomerAbrufgruende " & _
                                            "WHERE " & _
                                            "CustomerID =' " & m_objUser.Customer.CustomerId.ToString & "' AND " & _
                                            "SapWert='" & kuerzel & "'" & _
                                            " AND GroupID = " & m_objUser.GroupID.ToString _
                                               , cn)
            cmdAg.CommandType = CommandType.Text
            dr = cmdAg.ExecuteReader()

            If dr.Read() = True Then
                If dr.IsDBNull(0) Then
                    sAbrufgrund = String.Empty
                Else
                    sAbrufgrund = CStr(dr.Item("WebBezeichnung"))
                End If
            End If
        Catch ex As Exception

            Throw ex

        Finally
            cn.Close()
        End Try
        Return sAbrufgrund
    End Function

    Public Overrides Sub change()

    End Sub

    Public Sub stornoorderfreigabe(ByVal strAppID As String, ByVal strSessionID As String, ByVal strStorno As String, Optional ByVal Autorisierung As String = "")
        m_strClassAndMethod = "fin_11.stornoorderfreigabe"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        m_strMessage = ""
        m_intStatus = 0

        Try
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            S.AP.Init("Z_M_FREIGEBEN_AUFTRAG_001")
            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            S.AP.SetImportParameter("I_WEB_REPORT", m_objUser.Applications.Select("AppID = '" & m_strAppID & "'")(0)("AppFriendlyName").ToString)
            S.AP.SetImportParameter("I_KUNNR_ZF", strHaendler)
            S.AP.SetImportParameter("I_VBELN", strVBELN)
            S.AP.SetImportParameter("I_ERNAM", Right("0000000000" & m_objUser.KUNNR, 10))
            S.AP.SetImportParameter("I_EQUNR", strEquinr)
            S.AP.SetImportParameter("I_KKBER", strKontingentart)
            S.AP.SetImportParameter("I_STORNO", strStorno)
            S.AP.SetImportParameter("I_STORNOGRUND1", strStornoGrund1)
            S.AP.SetImportParameter("I_STORNOGRUND2", strStornoGrund2)
            S.AP.SetImportParameter("I_STORNOGRUND3", strStornoGrund3)
            S.AP.SetImportParameter("I_STORNOGRUND4", strStornoGrund4)

            If Autorisierung = "X" Then
                S.AP.SetImportParameter("I_USER_AUTOR", Left(mAutorisierungUser, 12))
            Else
                S.AP.SetImportParameter("I_USER_AUTOR", Left(m_objUser.UserName, 12))
            End If
            S.AP.SetImportParameter("I_DATUM_AUTOR", Now.ToShortDateString)
            S.AP.SetImportParameter("I_UZEIT_AUTOR", Now.Hour.ToString.PadLeft(2, CChar("0")) & Now.Minute.ToString.PadLeft(2, CChar("0")) & Now.Second.ToString.PadLeft(2, CChar("0")))

            S.AP.Execute()

            Dim FlagFreigabe As String = S.AP.GetExportParameter("E_FLAG_FREIG")

            mFreigabeDatum = ""
            mFreigabeUhrzeit = ""
            mFreigabeUser = ""
            mFreigabeUser = ""

            If Not FlagFreigabe Is String.Empty Then
                If FlagFreigabe = "X" Then
                    'auftrag wurde während der bearbeitung bereits freigegeben.
                    mFreigabeDatum = S.AP.GetExportParameter("E_ERDAT")
                    Dim strTempTime As String = S.AP.GetExportParameter("E_ERTIM")
                    If strTempTime.Length = 6 Then
                        mFreigabeUhrzeit = strTempTime.Substring(0, 2) & ":" & strTempTime.Substring(2, 2) & ":" & strTempTime.Substring(4, 2)
                    End If
                    mFreigabeUser = S.AP.GetExportParameter("E_ERNAM")
                    m_intStatus = -1111
                End If
            End If

        Catch ex As Exception
            m_intStatus = -9999
            m_strMessage = "Beim Stornieren/Freigeben des Auftrages ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
        End Try
    End Sub

#End Region

End Class

' ************************************************
' $History: fin_11.vb $
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 24.05.11   Time: 9:12
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 3.03.10    Time: 13:52
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 10.07.09   Time: 17:09
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 22.06.09   Time: 16:58
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918  Z_M_Gesperrte_Auftraege_001
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 4.05.09    Time: 11:50
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 23.07.08   Time: 10:41
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' Bugfix 2062
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 23.07.08   Time: 10:35
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' SapProxyVerweise neue hinzugefügt
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 14.07.08   Time: 13:54
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2026
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 10.07.08   Time: 11:02
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2062
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 13.06.08   Time: 14:43
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2005
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 17.04.08   Time: 12:42
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' AKf freigabe BugFix
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 16.04.08   Time: 14:45
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 21  *****************
' User: Jungj        Date: 11.04.08   Time: 9:12
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1842
' 
' *****************  Version 20  *****************
' User: Jungj        Date: 11.04.08   Time: 8:17
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1842
' 
' *****************  Version 19  *****************
' User: Jungj        Date: 11.03.08   Time: 16:42
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1766
' 
' *****************  Version 18  *****************
' User: Jungj        Date: 11.03.08   Time: 14:48
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1765
' 
' *****************  Version 17  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 16  *****************
' User: Jungj        Date: 2.02.08    Time: 13:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' akf Änderungen
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 2.02.08    Time: 9:21
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 22.01.08   Time: 9:06
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' doku
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 9.01.08    Time: 9:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
'  bugfix Sammelautorisierung bei Freigabe gesperrter Aufträge
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 8.01.08    Time: 18:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Autorisierung
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 8.01.08    Time: 16:19
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Einzelautorisierung bei der Freigabe Briefanforderungen
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 8.01.08    Time: 12:49
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Authorisierung Freigabe gesperrter Aufträge
' 
' ************************************************