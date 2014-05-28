Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class ANC_1
    REM § Lese-/Schreibfunktion, Kunde: ANC, 
    REM § Show - BAPI: Z_M_Fahrzeugeingaenge,
    REM § Change - BAPI: .

    Inherits BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private m_tblBriefe As DataTable
    Private m_strBriefnummer As String
    Private m_strPDINummer As String
    Private m_strFahrgestellnummer As String
    Private m_strKennzeichen As String
#End Region

#Region " Properties"
    Public ReadOnly Property Briefe() As DataTable
        Get
            Return m_tblBriefe
        End Get
    End Property

    Public Property Kennzeichen() As String
        Get
            Return m_strKennzeichen
        End Get
        Set(ByVal Value As String)
            m_strKennzeichen = Value
        End Set
    End Property

    Public Property PDINummer() As String
        Get
            Return m_strPDINummer
        End Get
        Set(ByVal Value As String)
            m_strPDINummer = Value
        End Set
    End Property

    Public Property Briefnummer() As String
        Get
            Return m_strBriefnummer
        End Get
        Set(ByVal Value As String)
            m_strBriefnummer = Value
        End Set
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return m_strFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummer = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_strBriefnummer = ""
        m_strFahrgestellnummer = ""
        m_strKennzeichen = ""
    End Sub

    Public Overrides Sub Show()
        m_strClassAndMethod = "ANC_1.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ANC.SAPProxy_ANC()

            Dim Brief As SAPProxy_ANC.ZDAD_S_BRIEFEINGANG_VANGUARD
            Dim BriefTable As New SAPProxy_ANC.ZDAD_S_BRIEFEINGANG_VANGUARDTable()

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_tblBriefe = New DataTable()
                m_tblBriefe.Columns.Add("Eingangsdatum", System.Type.GetType("System.String"))
                m_tblBriefe.Columns.Add("Hersteller", System.Type.GetType("System.String"))
                m_tblBriefe.Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                m_tblBriefe.Columns.Add("Briefnummer", System.Type.GetType("System.String"))
                m_tblBriefe.Columns.Add("Bezeichnung / PDI-Meldung", System.Type.GetType("System.String"))
                m_tblBriefe.Columns.Add("Absender Fahrzeugbrief", System.Type.GetType("System.String"))

                m_intStatus = 0
                m_strMessage = ""

                If CheckCustomerData() Then

                    m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Briefeingang_Vanguard", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                    objSAP.Z_M_Briefeingang_Vanguard(m_strCustomer, BriefTable)
                    objSAP.CommitWork()
                    If m_intIDsap > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                    End If

                    If BriefTable.Count > 0 Then
                        Dim rowNew As DataRow
                        For Each Brief In BriefTable
                            rowNew = m_tblBriefe.NewRow
                            Dim datTemp As DateTime = MakeDateStandard(Brief.Erdat)
                            If Not (datTemp.ToShortDateString = "01.01.1900") Then
                                rowNew("Eingangsdatum") = Format(datTemp, "dd.MM.yyyy")
                            End If
                            rowNew("Hersteller") = Brief.Herst
                            rowNew("Fahrgestellnummer") = Brief.Chassis_Num
                            rowNew("Briefnummer") = Brief.Tidnr
                            rowNew("Bezeichnung / PDI-Meldung") = Brief.Zzbezei
                            rowNew("Absender Fahrzeugbrief") = Brief.Name1_Zp
                            m_tblBriefe.Rows.Add(rowNew)
                        Next
                    Else
                        m_intStatus = -2202
                        m_strMessage = "Fehler: Keine Briefinformationen vorhanden."
                    End If

                    Dim col As DataColumn
                    For Each col In m_tblBriefe.Columns
                        col.ReadOnly = False
                    Next
                End If

                WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c), m_tblBriefe)
                'WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZBRIEF=" & m_strBriefnummer & ", ZZFAHRG=" & m_strFahrgestellnummer & ", ZZKENN=" & m_strKennzeichen, m_tblBriefe)
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_WEB"
                        m_intStatus = -2201
                        m_strMessage = "Keine Eingabedaten vorhanden."
                    Case "NO_WEB"
                        m_intStatus = -2202
                        m_strMessage = "Keine Web-Tabelle erstellt"
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c), m_tblBriefe)
                'WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & ", ZZBRIEF=" & m_strBriefnummer & ", ZZFAHRG=" & m_strFahrgestellnummer & ", ZZKENN=" & m_strKennzeichen & " , " & Replace(m_strMessage, "<br>", " "), m_tblBriefe)
            Finally
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Change()

    End Sub
#End Region
End Class

' ************************************************
' $History: ANC_1.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 29.04.09   Time: 15:28
' Updated in $/CKAG/Applications/appanc/Lib
' Warnungen entfernt.
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:41
' Created in $/CKAG/Applications/appanc/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 15:47
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 6.03.07    Time: 13:45
' Updated in $/CKG/Applications/AppANC/AppANCWeb/Lib
' 
' ************************************************
