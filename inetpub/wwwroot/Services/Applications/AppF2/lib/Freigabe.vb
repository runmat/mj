Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common

<Serializable()> Public Class Freigabe

    Inherits CKG.Base.Business.BankBase

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
    Dim mKontingentSteuerung As String
    Dim mLiznr As String
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

    Public Property Liznr() As String
        Get
            Return mLiznr
        End Get
        Set(ByVal Value As String)
            mLiznr = Value
        End Set
    End Property

    Public Property KontingentSteuerung() As String
        Get
            Return Me.mKontingentSteuerung
        End Get
        Set(ByVal value As String)
            Me.mKontingentSteuerung = value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub


    Public Overloads Overrides Sub show()

    End Sub


    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Freigabe.Show"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Dim intID As Int32 = -1
        Dim strKUNNR As String = m_objUser.Reference ' Right("0000000000" & m_objUser.KUNNR, 10)
        m_intStatus = 0
        Try
            If m_objLogApp Is Nothing Then
                m_objLogApp = New CKG.Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Gesperrte_Auftraege_003", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            myProxy.setImportParameter("I_HAENDLER", strHaendler)
            myProxy.setImportParameter("I_VKORG", "1510")
            'myProxy.setImportParameter("I_KKBER", "")
            myProxy.setImportParameter("I_ZZFAHRG", strFahrgestellnr)
            myProxy.setImportParameter("I_LIZNR", mLiznr)
            myProxy.setImportParameter("I_SPERRE", mKontingentSteuerung)

            myProxy.callBapi()


            Dim tblTemp2 As DataTable = myProxy.getExportTable("GT_WEB")
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
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Daten vorhanden. "
                Case "NO_WEB"
                    m_strMessage = "Keine Web-Tabelle erstellt."
                Case "NO_HAENDLER"
                    m_strMessage = "Händler nicht vorhanden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
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
        stornoorderfreigabe(Storno)
    End Sub


    Public Sub stornoorderfreigabe(ByVal strStorno As String)

        m_strClassAndMethod = "Freigabe.StornoOrderFreigabe.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            m_intStatus = 0

            Try

              

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New CKG.Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Freigeben_Auftrag_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim proxy = DynSapProxy.getProxy("Z_M_FREIGEBEN_AUFTRAG_001", m_objApp, m_objUser, CurrentPageHelper.GetCurrentPage())

                'befüllen der Importparameter
                proxy.setImportParameter("I_EQUNR", strEquinr)
                proxy.setImportParameter("I_ERNAM", Left(m_objUser.UserName, 12))
                proxy.setImportParameter("I_KKBER", strKontingentart)
                proxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                proxy.setImportParameter("I_MATNR", "")
                proxy.setImportParameter("I_STORNO", strStorno)
                proxy.setImportParameter("I_KUNNR_ZF", strHaendler)
                proxy.setImportParameter("I_VBELN", strVBELN)
                proxy.setImportParameter("I_WEB_REPORT", m_objUser.Applications.Select("AppID = '" & m_strAppID & "'")(0)("AppFriendlyName").ToString)
                proxy.setImportParameter("I_STORNOGRUND1", strStornoGrund1)
                proxy.setImportParameter("I_STORNOGRUND2", strStornoGrund2)
                proxy.setImportParameter("I_STORNOGRUND3", strStornoGrund3)
                proxy.setImportParameter("I_STORNOGRUND4", strStornoGrund4)

                proxy.callBapi()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim FLAG_FREIG = proxy.getExportParameter("E_FLAG_FREIG")
                Dim ERNAM = proxy.getExportParameter("E_ERNAM")
                Dim ERDAT = proxy.getExportParameter("E_ERDAT")
                Dim ERTIM = proxy.getExportParameter("E_ERTIM")


                'auswerten der exportparameter
                If FLAG_FREIG = "X" Then
                    'auftrag wurde während der bearbeitung bereits freigegeben.
                    mFreigabeDatum = MakeDateStandard(ERDAT).ToShortDateString
                    mFreigabeUhrzeit = ERTIM.Substring(0, 2) & ":" & ERTIM.Substring(2, 2) & ":" & ERTIM.Substring(4, 2)
                    mFreigabeUser = ERNAM
                    m_intStatus = -1111
                End If
            Catch ex As Exception
                m_intStatus = -9999


                m_strMessage = "Beim stornieren/freigeben des Auftrages ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If

            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region


End Class
' ************************************************
' $History: Freigabe.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 8.09.09    Time: 13:01
' Updated in $/CKAG2/Applications/AppF2/lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 4.09.09    Time: 11:46
' Updated in $/CKAG2/Applications/AppF2/lib
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 2.09.09    Time: 14:06
' Updated in $/CKAG2/Applications/AppF2/lib
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 17.08.09   Time: 10:01
' Updated in $/CKAG2/Applications/AppF2/lib
' ITA: 3071
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 16.08.09   Time: 9:34
' Updated in $/CKAG2/Applications/AppF2/lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 13.08.09   Time: 17:36
' Created in $/CKAG2/Applications/AppF2/lib
' 
