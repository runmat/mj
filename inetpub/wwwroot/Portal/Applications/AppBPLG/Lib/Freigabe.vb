Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

<Serializable()> Public Class Freigabe
    REM §  BAPI: Z_M_Gesperrte_Auftraege_002
    REM §  BAPI:Z_M_Freigeben_Auftrag_002
    Inherits Base.Business.BankBase

#Region " Declarations"

    Dim strKontingentart As String
    Dim strEquinr As String
    Dim strVBELN As String
    Dim strStorno As String
    Dim mSucheFahrgestellnummer As String = ""
    Dim mFahrgestellnummer As String = ""
    Dim strStornoGrund1 As String = ""
    Dim strStornoGrund2 As String = ""
    Dim strStornoGrund3 As String = ""
    Dim strStornoGrund4 As String = ""
    Dim mFreigabeUser As String
    Dim mFreigabeDatum As String
    Dim mFreigabeUhrzeit As String
    Dim mSucheLiznr As String = ""
    Dim mSucheEndkundennummer As String = ""
    Dim mSucheHaendlernummer As String = ""

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

    Public Property SucheFahrgestellnr() As String
        Get
            Return mSucheFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            mSucheFahrgestellnummer = Value
        End Set
    End Property

    Public Property Fahrgestellnr() As String
        Get
            Return mFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            mFahrgestellnummer = Value
        End Set
    End Property

    Public Property SucheLIZNR() As String
        Get
            Return mSucheLiznr
        End Get
        Set(ByVal Value As String)
            mSucheLiznr = Value
        End Set
    End Property

    Public Property SucheEndkundennummer() As String
        Get
            Return mSucheEndkundennummer
        End Get
        Set(ByVal Value As String)
            mSucheEndkundennummer = Value
        End Set
    End Property

    Public Property SucheHaendlernummer() As String
        Get
            Return mSucheHaendlernummer
        End Get
        Set(ByVal Value As String)
            mSucheHaendlernummer = Value
        End Set
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overloads Overrides Sub show()
        show(AppID, SessionID)
    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "Freigabe.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                S.AP.Init("Z_M_GESPERRTE_AUFTRAEGE_002")

                S.AP.SetImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_HAENDLER", SucheHaendlernummer)
                S.AP.SetImportParameter("I_KKBER", "")
                S.AP.SetImportParameter("I_VKORG", "1510")
                S.AP.SetImportParameter("I_ZZFAHRG", SucheFahrgestellnr)
                S.AP.SetImportParameter("I_INTERN", "")
                S.AP.SetImportParameter("I_LIZNR", SucheLIZNR)
                S.AP.SetImportParameter("I_ENDKUNDE", SucheEndkundennummer)

                S.AP.Execute()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")

                'X für die column Translation 
                tblTemp2.Columns.Add("StornoGrundX", GetType(System.String))
                tblTemp2.Columns.Add("AbrufGrundX", GetType(System.String))
                tblTemp2.Columns.Add("VersandadresseX", GetType(System.String))
                tblTemp2.Columns.Add("MassenfreigabeX", GetType(System.String))

                Dim rowTemp As DataRow
                For Each rowTemp In tblTemp2.Rows
                    'im sap gibt es 4 x 27 Felder für den StornoGrund, beschissen aber wahr 

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
                    rowTemp("VersandadresseX") = CStr(rowTemp("NAME1")) & " " & CStr(rowTemp("NAME2")) & " - " &
                        CStr(rowTemp("STREET")) & " " & CStr(rowTemp("HOUSE_NUM1")) & " - " & CStr(rowTemp("POST_CODE1")) & " " &
                        CStr(rowTemp("CITY1")) & " - " & CStr(rowTemp("COUNTRY"))

                    If TypeOf rowTemp("MassenfreigabeX") Is DBNull Then
                        rowTemp("MassenfreigabeX") = ""
                    End If
                Next

                'entfernen der rows die in sich in autorisation befinden
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
                    m_objApp.CheckForPendingAuthorization(CInt(m_strAppID), m_objUser.Organization.OrganizationId, CStr(rowTemp("Endkundennummer")),
                                                          CStr(rowTemp("Fahrgestellnummer")), m_objUser.IsTestUser, strInitiator, intAutID)
                    If intAutID > 0 Then
                        m_tblResult.Rows.RemoveAt(i)
                    Else
                        i = i + 1
                    End If
                Next
                m_tblResult.AcceptChanges()
                '------------------------------

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case "NO_WEB"
                        m_strMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_HAENDLER"
                        m_strMessage = "Händler nicht vorhanden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

    Private Function getAbrufgrund(ByVal kuerzel As String) As String
        Dim sReturn As String = ""
        Dim cn As SqlClient.SqlConnection
        Dim cmdAg As SqlClient.SqlCommand
        Dim dsAg As DataSet
        Dim adAg As SqlClient.SqlDataAdapter
        Dim dr As SqlClient.SqlDataReader
        cn = New SqlClient.SqlConnection

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
                    sReturn = String.Empty
                Else
                    sReturn = CStr(dr.Item("WebBezeichnung"))
                End If
            End If
        Catch ex As Exception
            Throw New Exception(HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
        Finally
            cn.Close()
        End Try

        Return sReturn
    End Function

    Public Overrides Sub change()
        StornoOrderFreigabe(Storno)
    End Sub

    Public Sub StornoOrderFreigabe(ByVal strStorno As String)
        m_strClassAndMethod = "Freigabe.StornoOrderFreigabe.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            ClearError()

            Try

                S.AP.Init("Z_M_FREIGEBEN_AUFTRAG_002")

                S.AP.SetImportParameter("I_EQUNR", strEquinr)
                S.AP.SetImportParameter("I_ERNAM", Left(m_objUser.UserName, 12))
                S.AP.SetImportParameter("I_KKBER", strKontingentart)
                S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_MATNR", "")
                S.AP.SetImportParameter("I_STORNO", strStorno)
                S.AP.SetImportParameter("I_KUNNR_ZF", "")
                S.AP.SetImportParameter("I_VBELN", strVBELN)
                S.AP.SetImportParameter("I_WEB_REPORT", m_objUser.Applications.Select("AppID = '" & m_strAppID & "'")(0)("AppFriendlyName").ToString)
                S.AP.SetImportParameter("I_STORNOGRUND1", strStornoGrund1)
                S.AP.SetImportParameter("I_STORNOGRUND2", strStornoGrund2)
                S.AP.SetImportParameter("I_STORNOGRUND3", strStornoGrund3)
                S.AP.SetImportParameter("I_STORNOGRUND4", strStornoGrund4)

                S.AP.Execute()

                'auswerten der exportparameter
                If S.AP.GetExportParameter("E_FLAG_FREIG").ToUpper() = "X" Then
                    mFreigabeDatum = S.AP.GetExportParameter("E_ERDAT")
                    mFreigabeUhrzeit = S.AP.GetExportParameter("E_ERTIM").Substring(0, 2) & ":" &
                        S.AP.GetExportParameter("E_ERTIM").Substring(2, 2) & ":" &
                        S.AP.GetExportParameter("E_ERTIM").Substring(4, 2)
                    mFreigabeUser = S.AP.GetExportParameter("E_ERNAM")
                    m_intStatus = -1111
                End If

            Catch ex As Exception
                m_intStatus = -9999

                m_strMessage = "Beim stornieren/sreigeben des Auftrages ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"

            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

#End Region

End Class
' ************************************************
' $History: Freigabe.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 29.04.09   Time: 16:09
' Updated in $/CKAG/Applications/AppBPLG/Lib
' Warnungen
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 22.08.08   Time: 12:39
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ITA 2189 fertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 29.07.08   Time: 14:29
' Updated in $/CKAG/Applications/AppBPLG/Lib
' BPLG Test Nachbesserungen
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 28.07.08   Time: 8:53
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ita 2100 testfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 25.07.08   Time: 16:57
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ITA 2100 TestFertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.07.08   Time: 10:31
' Updated in $/CKAG/Applications/AppBPLG/Lib
' ITA 2100 ungetestet
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 23.07.08   Time: 13:56
' Created in $/CKAG/Applications/AppBPLG/Lib
' ITA 2100 erstellung Rohversion
' 
' ************************************************