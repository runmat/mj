Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports Microsoft.Data.SAPClient
Imports CKG.Base.Business

Public Class VFS03
    REM § Lese-/Schreibfunktion, Kunde: VFS,
    REM § Show - BAPI: --keine--,
    REM § Change - BAPI: z_M_Offeneanforderungen_storno

    Inherits Base.Business.BankBase

#Region " Declarations"
    Private m_strVorname As String
    Private m_strName As String
    Private m_strStrasse As String
    Private m_strTelefon As String
    Private m_strHausnummer As String
    Private m_strPostleitzahl As String
    Private m_strOrt As String
    Private m_strAgenturnummer As String
    Private m_intAnzahlKennzeichen As Integer
    Private m_blnExpress As Boolean
    Private m_blnConfirm As Boolean
    Private m_blnComplete As Boolean
    Private m_strEmailAdresse As String
    Private m_blnKeineEmailAdresse As Boolean
    Private mAnrede As String
    Private mMehrfachBestellung As Boolean = False

    Private mWEAnrede As String = ""
    Private m_strWEVorname As String = ""
    Private m_strWEName As String = ""
    Private m_strWEStrasse As String = ""
    Private m_strWETelefon As String = ""
    Private m_strWEHausnummer As String = ""
    Private m_strWEPostleitzahl As String = ""
    Private m_strWEOrt As String = ""

#End Region

#Region " Properties"
    Public Property KeineEmailAdresse() As Boolean
        Get
            Return m_blnKeineEmailAdresse
        End Get
        Set(ByVal Value As Boolean)
            m_blnKeineEmailAdresse = Value
        End Set
    End Property

    Public Property EmailAdresse() As String
        Get
            Return m_strEmailAdresse
        End Get
        Set(ByVal Value As String)
            m_strEmailAdresse = Value
        End Set
    End Property

    Public Property Mehrfachbestellung() As Boolean
        Get
            Return mMehrfachBestellung
        End Get
        Set(ByVal value As Boolean)
            mMehrfachBestellung = value
        End Set
    End Property

    Public Property Anrede() As String
        Get
            Return mAnrede
        End Get
        Set(ByVal Value As String)
            mAnrede = Value
        End Set
    End Property

    Public Property Confirm() As Boolean
        Get
            Return m_blnConfirm
        End Get
        Set(ByVal Value As Boolean)
            m_blnConfirm = Value
        End Set
    End Property

    Public Property Complete() As Boolean
        Get
            Return m_blnComplete
        End Get
        Set(ByVal Value As Boolean)
            m_blnComplete = Value
        End Set
    End Property

    Public Property Vorname() As String
        Get
            Return m_strVorname
        End Get
        Set(ByVal Value As String)
            m_strVorname = Value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return m_strName
        End Get
        Set(ByVal Value As String)
            m_strName = Value
        End Set
    End Property

    Public Property Strasse() As String
        Get
            Return m_strStrasse
        End Get
        Set(ByVal Value As String)
            m_strStrasse = Value
        End Set
    End Property

    Public Property Telefon() As String
        Get
            Return m_strTelefon
        End Get
        Set(ByVal Value As String)
            m_strTelefon = Value
        End Set
    End Property

    Public Property Hausnummer() As String
        Get
            Return m_strHausnummer
        End Get
        Set(ByVal Value As String)
            m_strHausnummer = Value
        End Set
    End Property

    Public Property Postleitzahl() As String
        Get
            Return m_strPostleitzahl
        End Get
        Set(ByVal Value As String)
            m_strPostleitzahl = Value
        End Set
    End Property

    Public Property Ort() As String
        Get
            Return m_strOrt
        End Get
        Set(ByVal Value As String)
            m_strOrt = Value
        End Set
    End Property

    Public Property Agenturnummer() As String
        Get
            Return m_strAgenturnummer
        End Get
        Set(ByVal Value As String)
            m_strAgenturnummer = Value
        End Set
    End Property

    Public Property AnzahlKennzeichen() As Integer
        Get
            Return m_intAnzahlKennzeichen
        End Get
        Set(ByVal Value As Integer)
            m_intAnzahlKennzeichen = Value
        End Set
    End Property

    Public Property Express() As Boolean
        Get
            Return m_blnExpress
        End Get
        Set(ByVal Value As Boolean)
            m_blnExpress = Value
        End Set
    End Property

    Public Property WEAnrede() As String
        Get
            Return mWEAnrede
        End Get
        Set(ByVal Value As String)
            mWEAnrede = Value
        End Set
    End Property

    Public Property WEVorname() As String
        Get
            Return m_strWEVorname
        End Get
        Set(ByVal Value As String)
            m_strWEVorname = Value
        End Set
    End Property

    Public Property WEName() As String
        Get
            Return m_strWEName
        End Get
        Set(ByVal Value As String)
            m_strWEName = Value
        End Set
    End Property

    Public Property WEStrasse() As String
        Get
            Return m_strWEStrasse
        End Get
        Set(ByVal Value As String)
            m_strWEStrasse = Value
        End Set
    End Property

    Public Property WETelefon() As String
        Get
            Return m_strWETelefon
        End Get
        Set(ByVal Value As String)
            m_strWETelefon = Value
        End Set
    End Property

    Public Property WEHausnummer() As String
        Get
            Return m_strWEHausnummer
        End Get
        Set(ByVal Value As String)
            m_strWEHausnummer = Value
        End Set
    End Property

    Public Property WEPostleitzahl() As String
        Get
            Return m_strWEPostleitzahl
        End Get
        Set(ByVal Value As String)
            m_strWEPostleitzahl = Value
        End Set
    End Property

    Public Property WEOrt() As String
        Get
            Return m_strWEOrt
        End Get
        Set(ByVal Value As String)
            m_strWEOrt = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

        m_strVorname = ""
        m_strName = ""
        m_strStrasse = ""
        m_strHausnummer = ""
        m_strPostleitzahl = ""
        m_strOrt = ""
        m_strAgenturnummer = ""
        m_intAnzahlKennzeichen = 5
        m_blnExpress = False
        m_blnConfirm = False
        m_blnComplete = False
        m_strEmailAdresse = ""
        m_blnKeineEmailAdresse = False
    End Sub

    Public Overloads Overrides Sub show()
        'nur wegen bankbase
    End Sub

    Public Overloads Sub Show(ByVal HEZ As String)
        'GIBT'S HIER NICHT!!!

        'm_strClassAndMethod = "VFS03.Show"
        'If Not m_blnGestartet Then
        '    m_blnGestartet = True

        '    Dim objSAP As New SAPProxy_ComCommon.SAPProxy_ComCommon()

        '    Dim tblAuftraegeSAP As New SAPProxy_ComCommon.ZDAD_M_WEB_AUFTRAEGETable()

        '    MakeDestination()
        '    objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
        '    objSAP.Connection.Open()

        '    If m_objLogApp Is Nothing Then
        '        m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        '    End If
        '    m_intIDSAP = -1
        '    Dim i As Int32
        '    Dim rowTemp As DataRow

        '    Try
        '        m_intStatus = 0
        '        m_strMessage = ""
        '        Dim strKKBER As String

        '        m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Offene_Anforderungen_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

        '        objSAP.Z_M_Offene_Anforderungen_001(Right("0000000000" & m_objUser.Customer.KUNNR, 10), m_strHaendler, HEZ, "", "1510", tblAuftraegeSAP)

        '        objSAP.CommitWork()
        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
        '        End If

        '        m_tblRaw = tblAuftraegeSAP.ToADODataTable
        '        m_tblRaw.AcceptChanges()
        '        For Each rowTemp In m_tblRaw.Rows
        '            Select Case rowTemp("BSTZD")
        '                Case "0001"
        '                    rowTemp("BSTZD") = "Standard temporär"
        '                Case "0002"
        '                    rowTemp("BSTZD") = "Standard endgültig"
        '                Case "0005"
        '                    rowTemp("BSTZD") = "Händlereigene Zulassung"
        '            End Select
        '            If CStr(rowTemp("CMGST")) = "B" Then
        '                rowTemp("CMGST") = "X"
        '            Else
        '                rowTemp("CMGST") = ""
        '            End If
        '        Next
        '        m_tblRaw.AcceptChanges()
        '        m_tblAuftraege = CreateOutPut(m_tblRaw, m_strAppID)
        '        m_tblResultExcel = m_tblAuftraege.Copy
        '        m_tblResultExcel.Columns.Remove("VBELN")
        '        m_tblResultExcel.Columns.Remove("EQUNR")


        '        If m_tblAuftraege.Rows.Count = 0 Then
        '            m_intStatus = 0
        '            m_strMessage = "Keine Daten gefunden."
        '        End If

        '        If m_strHaendler.Length > 0 Then
        '            Haendler = m_strHaendler
        '        End If
        '    Catch ex As Exception
        '        Select Case ex.Message
        '            Case "NO_DATA"
        '                m_intStatus = 0
        '                If m_hez = True Then
        '                    m_intStatus = -2501
        '                End If
        '                m_strMessage = "Keine Daten gefunden."
        '            Case Else
        '                m_intStatus = -9999
        '                m_strMessage = ex.Message
        '        End Select
        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
        '        End If
        '    Finally
        '        If m_intIDsap > -1 Then
        '            m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
        '        End If

        '        objSAP.Connection.Close()
        '        objSAP.Dispose()

        '        m_blnGestartet = False
        '    End Try
        'End If
    End Sub


    Public Function checkVertriebsdirektion(ByVal code As String) As Boolean
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_VFS.SAPProxy_VFS()

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""
                Dim strResult As String = String.Empty

                m_intIDsap = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_MOFA_VDPRUEFUNG_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                objSAP.Z_M_Mofa_Vdpruefung_001(Right("0000000000" & m_objUser.KUNNR, 10), code, strResult)
                objSAP.CommitWork()

                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, True)
                End If

                If Not strResult = "X" Then
                    Return False
                Else
                    Return True
                End If

            Catch ex As Exception
                Select Case ex.Message
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                        Return False
                End Select
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
            Finally
                objSAP.Connection.Close()
                objSAP.Dispose()
                m_blnGestartet = False
            End Try
        End If
    End Function


    Public Overrides Sub Change()

        If Not m_blnGestartet Then
            m_blnGestartet = True
            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            con.Open()
            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim strVersand As String
                If m_blnExpress Then
                    strVersand = " "
                Else
                    strVersand = "X"
                End If


                Dim cmd As New SAPCommand()
                cmd.Connection = con

                Dim strCom As String

                strCom = "EXEC Z_M_Mofa_Anford_001 @I_KUNNR_AG=@pI_KUNNR_AG,@I_ANREDE=@pI_ANREDE,@I_NAME1=@pI_NAME1,@I_NAME2=@pI_NAME2,"
                strCom = strCom & "@I_STREET=@pI_STREET,@I_HOUSE_NUM1=@pI_HOUSE_NUM1,@I_POST_CODE1=@pI_POST_CODE1,@I_CITY1=@pI_CITY1,"
                strCom = strCom & "@I_KUN_VD=@pI_KUN_VD,@I_AGENTUR=@pI_AGENTUR,@I_ANZAHL=@pI_ANZAHL,@I_VERSART=@pI_VERSART,"
                strCom = strCom & "@I_SMTP_ADDR=@pI_SMTP_ADDR,@I_TELF1=@pI_TELF1,@I_BEST_ANL=@pI_BEST_ANL, "
                strCom = strCom & "@WE_ANREDE=@pI_WE_ANREDE,@WE_NAME1=@pI_WE_NAME1,@WE_NAME2='" & m_strWEName.ToString & "', "
                strCom = strCom & "@WE_STREET=@pI_WE_STREET,@WE_HOUSE_NUM1=@pI_WE_HOUSE_NUM1,@WE_POST_CODE1=@pI_WE_POST_CODE1, "
                strCom = strCom & "@WE_CITY1=@pI_WE_CITY1,@WE_TELF1=@pI_WE_TELF1, @E_BEST_VORH=@pE_BEST_VORH OUTPUT OPTION 'disabledatavalidation'"

                cmd.CommandText = strCom

                'importparameter
                Dim pI_KUNNR_AG As New SAPParameter("@pI_KUNNR_AG", ParameterDirection.Input)
                Dim pI_ANREDE As New SAPParameter("@pI_ANREDE", ParameterDirection.Input)
                Dim pI_NAME1 As New SAPParameter("@pI_NAME1", ParameterDirection.Input)
                Dim pI_NAME2 As New SAPParameter("@pI_NAME2", ParameterDirection.Input)

                Dim pI_STREET As New SAPParameter("@pI_STREET", ParameterDirection.Input)
                Dim pI_HOUSE_NUM1 As New SAPParameter("@pI_HOUSE_NUM1", ParameterDirection.Input)
                Dim pI_POST_CODE1 As New SAPParameter("@pI_POST_CODE1", ParameterDirection.Input)
                Dim pI_CITY1 As New SAPParameter("@pI_CITY1", ParameterDirection.Input)

                Dim pI_KUN_VD As New SAPParameter("@pI_KUN_VD", ParameterDirection.Input)
                Dim pI_AGENTUR As New SAPParameter("@pI_AGENTUR", ParameterDirection.Input)
                Dim pI_ANZAHL As New SAPParameter("@pI_ANZAHL", ParameterDirection.Input)
                Dim pI_VERSART As New SAPParameter("@pI_VERSART", ParameterDirection.Input)

                Dim pI_SMTP_ADDR As New SAPParameter("@pI_SMTP_ADDR", ParameterDirection.Input)
                Dim pI_TELF1 As New SAPParameter("@pI_TELF1", ParameterDirection.Input)
                Dim pI_BEST_ANL As New SAPParameter("@pI_BEST_ANL", ParameterDirection.Input)

                Dim pI_WE_ANREDE As New SAPParameter("@pI_WE_ANREDE", ParameterDirection.Input)
                Dim pI_WE_NAME1 As New SAPParameter("@pI_WE_NAME1", ParameterDirection.Input)
                ' Dim pI_WE_NAME2 As New SAPParameter("@pI_WE_NAME2 ", ParameterDirection.Input)
                Dim pI_WE_STREET As New SAPParameter("@pI_WE_STREET", ParameterDirection.Input)

                Dim pI_WE_HOUSE_NUM1 As New SAPParameter("@pI_WE_HOUSE_NUM1", ParameterDirection.Input)
                Dim pI_WE_POST_CODE1 As New SAPParameter("@pI_WE_POST_CODE1", ParameterDirection.Input)
                Dim pI_WE_CITY1 As New SAPParameter("@pI_WE_CITY1", ParameterDirection.Input)
                Dim pI_WE_TELF1 As New SAPParameter("@pI_WE_TELF1", ParameterDirection.Input)

                'exportparameter
                Dim pE_BEST_VORH As New SAPParameter("@pE_BEST_VORH", ParameterDirection.Output)

                'Importparameter hinzufügen
                cmd.Parameters.Add(pI_KUNNR_AG)
                cmd.Parameters.Add(pI_ANREDE)
                cmd.Parameters.Add(pI_NAME1)
                cmd.Parameters.Add(pI_NAME2)

                cmd.Parameters.Add(pI_STREET)
                cmd.Parameters.Add(pI_HOUSE_NUM1)
                cmd.Parameters.Add(pI_POST_CODE1)
                cmd.Parameters.Add(pI_CITY1)

                cmd.Parameters.Add(pI_KUN_VD)
                cmd.Parameters.Add(pI_AGENTUR)
                cmd.Parameters.Add(pI_ANZAHL)
                cmd.Parameters.Add(pI_VERSART)

                cmd.Parameters.Add(pI_SMTP_ADDR)
                cmd.Parameters.Add(pI_TELF1)
                cmd.Parameters.Add(pI_BEST_ANL)

                cmd.Parameters.Add(pI_WE_ANREDE)
                cmd.Parameters.Add(pI_WE_NAME1)
                ' cmd.Parameters.Add(pI_WE_NAME2)
                cmd.Parameters.Add(pI_WE_STREET)
                cmd.Parameters.Add(pI_WE_HOUSE_NUM1)
                cmd.Parameters.Add(pI_WE_POST_CODE1)
                cmd.Parameters.Add(pI_WE_CITY1)
                cmd.Parameters.Add(pI_WE_TELF1)

                'exportparameter hinzufügen
                cmd.Parameters.Add(pE_BEST_VORH)


                'befüllen der Importparameter
                pI_KUNNR_AG.Value = Right("0000000000" & m_objUser.KUNNR, 10)
                pI_ANREDE.Value = mAnrede
                pI_NAME1.Value = m_strVorname
                pI_NAME2.Value = m_strName
                pI_STREET.Value = m_strStrasse
                pI_HOUSE_NUM1.Value = m_strHausnummer
                pI_POST_CODE1.Value = m_strPostleitzahl
                pI_CITY1.Value = m_strOrt
                pI_KUN_VD.Value = ""
                pI_AGENTUR.Value = m_strAgenturnummer
                pI_ANZAHL.Value = m_intAnzahlKennzeichen.ToString
                pI_VERSART.Value = strVersand
                pI_SMTP_ADDR.Value = m_strEmailAdresse
                pI_TELF1.Value = m_strTelefon
                pI_WE_ANREDE.Value = mWEAnrede.ToString
                pI_WE_NAME1.Value = m_strWEVorname.ToString
                ' pI_WE_NAME2.Value = m_strWEName
                pI_WE_STREET.Value = m_strWEStrasse.ToString
                pI_WE_HOUSE_NUM1.Value = m_strWEHausnummer.ToString
                pI_WE_POST_CODE1.Value = m_strWEPostleitzahl.ToString
                pI_WE_CITY1.Value = m_strWEOrt.ToString
                pI_WE_TELF1.Value = m_strWETelefon.ToString
                If Mehrfachbestellung Then
                    pI_BEST_ANL.Value = "X"
                Else
                    pI_BEST_ANL.Value = ""
                End If

                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Mofa_Anford_001", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                cmd.ExecuteNonQuery()
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                If Not pE_BEST_VORH.Value Is Nothing Then
                    If Not pE_BEST_VORH.Value.ToString = "0" And Not pI_BEST_ANL.Value.ToString = "X" Then
                        m_intStatus = -2222
                        m_strMessage = "heutige Bestellvorgänge: " & pE_BEST_VORH.Value.ToString
                        Exit Sub
                    End If
                End If
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        ' m_intStatus = -9999
                        m_intStatus = -2222
                        m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
            Finally
                con.Close()
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class
' ************************************************
' $History: VFS03.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 19.01.09   Time: 14:19
' Updated in $/CKAG/Applications/appvfs/Lib
' ITA: 2498
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 9.12.08    Time: 11:06
' Updated in $/CKAG/Applications/appvfs/Lib
' ITA 2433 testfertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 22.08.08   Time: 9:22
' Updated in $/CKAG/Applications/appvfs/Lib
' ITA 2086 SapConnector Part entfernt
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 22.08.08   Time: 9:15
' Updated in $/CKAG/Applications/appvfs/Lib
' ITA 2086 fertig
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 17:43
' Created in $/CKAG/Applications/appvfs/Lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 10.03.08   Time: 10:15
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 21.02.08   Time: 10:29
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA:1727
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 31.01.08   Time: 8:52
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA 1466
' 
' *****************  Version 4  *****************
' User: Uha          Date: 24.01.08   Time: 13:07
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA 1644: Oberflächenkosmetik (Vertriebsdirektion entfernt) - BAPI nach
' wie vor funktionslos
' 
' *****************  Version 3  *****************
' User: Uha          Date: 23.01.08   Time: 12:36
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA 1644: Formular mit Prüfung (BAPI immer noch funktionslos)
' 
' *****************  Version 2  *****************
' User: Uha          Date: 22.01.08   Time: 14:52
' Updated in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA 1644: Change01 und VFS03 - Vorversion, da BAPI derzeit nur Hülle
' 
' *****************  Version 1  *****************
' User: Uha          Date: 22.01.08   Time: 13:16
' Created in $/CKG/Applications/AppVFS/AppVFSWeb/Lib
' ITA 1644: Change01 und VFS03 (funktionslos) hinzugefügt
' 
' ************************************************