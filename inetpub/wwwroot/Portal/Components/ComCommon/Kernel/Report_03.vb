Option Explicit On 
Option Strict On
Option Compare Binary

Imports System
Imports CKG.Base.Kernel

Public Class Report_03
    REM § Lese-/Schreibfunktion, Kunde: Übergreifend
    REM § Show - BAPI: Z_M_Flc_Anz_Auftr_001
    REM § Change - BAPI: XXXXXXXX_Offen

    Inherits Base.Business.BankBase

#Region " Declarations"
    Private m_tblAuftraege As DataTable
    Private m_tblRaw As DataTable
    Private m_blnChangeMemo As Boolean
    Private m_i_Kunnr As String
    Private m_datVon As DateTime
    Private m_datBis As DateTime
    Private m_strVbeln As String
    Private m_strBemerkung As String
#End Region

#Region " Properties"
    Public Property Bemerkung() As String
        Get
            Return m_strBemerkung
        End Get
        Set(ByVal Value As String)
            m_strBemerkung = Value
        End Set
    End Property

    Public Property Vbeln() As String
        Get
            Return m_strVbeln
        End Get
        Set(ByVal Value As String)
            m_strVbeln = Right("0000000000" & Value, 10)
        End Set
    End Property

    Public Property DatumVon() As DateTime
        Get
            Return m_datVon
        End Get
        Set(ByVal Value As DateTime)
            m_datVon = Value
        End Set
    End Property

    Public Property DatumBis() As DateTime
        Get
            Return m_datBis
        End Get
        Set(ByVal Value As DateTime)
            m_datBis = Value
        End Set
    End Property

    Public ReadOnly Property Auftraege() As DataTable
        Get
            Return m_tblAuftraege
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        m_blnChangeMemo = False
        m_i_Kunnr = objUser.KUNNR.PadLeft(10, "0"c)
    End Sub

    Public Overrides Sub Show()
        m_strClassAndMethod = "Report_03.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_ComCommon.SAPProxy_ComCommon()

            Dim tblAuftraegeSAP As New SAPProxy_ComCommon.ZDAD_M_WEB_FLC_AUFTR_001Table()
            Dim tblTexteSAP As New SAPProxy_ComCommon.ZDAD_M_FLC_TEXT_001Table()
            Dim tblRawTexte As DataTable

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1
            Dim i As Int32
            Dim rowTemp As DataRow

            Try
                m_intStatus = 0
                m_strMessage = ""


                m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Flc_Anz_Auftr_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_M_Flc_Anz_Auftr_001(m_i_Kunnr, MakeDateSAP(m_datBis), MakeDateSAP(m_datVon), tblTexteSAP, tblAuftraegeSAP)
                objSAP.CommitWork()
                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                m_tblRaw = tblAuftraegeSAP.ToADODataTable
                tblRawTexte = tblTexteSAP.ToADODataTable

                m_tblAuftraege = CreateOutPut(m_tblRaw, m_strAppID)
                m_tblAuftraege.Columns.Add("Bemerkung", System.Type.GetType("System.String"))
                m_tblAuftraege.Columns.Add("ButtonText", System.Type.GetType("System.String"))
                Dim rowTemp2() As DataRow
                For Each rowTemp In m_tblAuftraege.Rows
                    rowTemp("Bemerkung") = ""
                    rowTemp("ButtonText") = "Erfassen"

                    If Not tblRawTexte Is Nothing AndAlso tblRawTexte.Rows.Count > 0 Then
                        rowTemp2 = tblRawTexte.Select("VBELN = '" & CStr(rowTemp("Vertriebsbelegnummer")) & "'")
                        If rowTemp2.Length > 0 Then
                            rowTemp("ButtonText") = "Ändern"
                            For i = 0 To rowTemp2.Length - 1
                                rowTemp("Bemerkung") = CStr(rowTemp("Bemerkung")) & CStr(rowTemp2(i)("Tdline"))
                            Next
                        End If

                        rowTemp("Bemerkung") = CStr(rowTemp("Bemerkung")).TrimStart(" "c)
                    End If
                    rowTemp("Vertriebsbelegnummer") = CStr(rowTemp("Vertriebsbelegnummer")).TrimStart("0"c)
                Next

                m_tblResultExcel = m_tblAuftraege.Copy
                m_tblResultExcel.Columns.Remove("Vertriebsbelegnummer")
                m_tblResultExcel.Columns.Remove("ButtonText")
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -1
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
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
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim objSAP As New SAPProxy_ComCommon.SAPProxy_ComCommon()
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                MakeDestination()
                objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                objSAP.Connection.Open()

                Dim tblText As New SAPProxy_ComCommon.ZDAD_M_FLC_TEXT_002Table()
                Dim lineText As SAPProxy_ComCommon.ZDAD_M_FLC_TEXT_002
                Dim strTemp As String = m_strBemerkung
                Do While strTemp.Length > 0
                    lineText = New SAPProxy_ComCommon.ZDAD_M_FLC_TEXT_002()
                    lineText.Vbeln = m_strVbeln
                    lineText.Tdline = Left(strTemp, 132)
                    tblText.Add(lineText)
                    If strTemp.Length < 132 Then
                        strTemp = ""
                    Else
                        strTemp = Right(strTemp, strTemp.Length - 132)
                    End If
                Loop

                m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "XXXXXXXX_Offen", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                objSAP.Z_M_Flc_Change_Text_001(m_i_Kunnr, tblText)
                objSAP.CommitWork()

                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = ex.Message
                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Report_03.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Kernel
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Kernel
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Kernel
' 
' *****************  Version 4  *****************
' User: Uha          Date: 20.09.07   Time: 17:19
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' ITA 1181 Testversion
' 
' *****************  Version 3  *****************
' User: Uha          Date: 19.09.07   Time: 17:29
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' ITA 1181: Funktionslose Rohversion
' 
' *****************  Version 2  *****************
' User: Uha          Date: 17.09.07   Time: 14:40
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' ITA 1237: Bugfixing; Bemerkung: BAPI zum Schreiben der Bemerkung steht
' noch aus
' 
' *****************  Version 1  *****************
' User: Uha          Date: 5.09.07    Time: 17:21
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' ITA 1237 - Report "Geplante Floorchecks": Testversion fertig (keine
' Testdaten im CKQ)
' 
' ************************************************
