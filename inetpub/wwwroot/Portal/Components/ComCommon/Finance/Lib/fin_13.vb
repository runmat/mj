Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class fin_13
    REM § Lese-/Schreibfunktion, Kunde: Banken,
    REM § Show - BAPI: Z_M_AUFTRAGSDATEN_001,
    REM § Change - BAPI: Z_M_Offeneanfor_Storno_001
    Inherits Base.Business.BankBase

#Region " Declarations"
    Private m_tblAuftraege As DataTable
    Private m_tblRaw As DataTable
    Private m_strHaendler As String
    Private m_strEQUNR As String
    Private m_strVBELN As String
    Private m_strStornoHaendler As String
    Private m_strFahrgestellnummer As String

#End Region

#Region " Properties"
    Public ReadOnly Property Auftraege() As DataTable
        Get
            Return m_tblAuftraege
        End Get
    End Property


    Public Property Haendler() As String
        Get
            Return m_strHaendler
        End Get
        Set(ByVal Value As String)
            m_strHaendler = Value
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


    Public Property StornoHaendler() As String
        Get
            Return m_strStornoHaendler
        End Get
        Set(ByVal Value As String)
            m_strStornoHaendler = Value
        End Set
    End Property

    Public Property EQUNR() As String
        Get
            Return m_strEQUNR
        End Get
        Set(ByVal Value As String)
            m_strEQUNR = Value
        End Set
    End Property


    Public Property VBELN() As String
        Get
            Return m_strVBELN
        End Get
        Set(ByVal Value As String)
            m_strVBELN = Value
        End Set
    End Property


#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String,
                    ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overloads Overrides Sub show()
        'nur wegen bankbase
    End Sub

    Public Overloads Sub Show(ByVal HEZ As String, ByVal strAppID As String, ByVal strSessionID As String)
        Dim rowTemp As DataRow

        Try
            ClearError()

            S.AP.InitExecute("Z_M_Offene_Anforderungen_001", "I_AG, I_HAENDLER, I_VKORG, I_HEZKZ",
                             Right("0000000000" & m_objUser.KUNNR, 10), m_strHaendler, "1510", HEZ)

            m_tblRaw = S.AP.GetExportTable("GT_WEB")
            m_tblRaw.Columns.Add("Adresse", System.Type.GetType("System.String"))
            m_tblRaw.Columns("BSTZD").MaxLength = 25
            m_tblRaw.AcceptChanges()

            For Each rowTemp In m_tblRaw.Rows
                Select Case rowTemp("BSTZD").ToString
                    Case "0001"
                        rowTemp("BSTZD") = "Standard temporär"
                    Case "0002"
                        rowTemp("BSTZD") = "Standard endgültig"
                    Case "0005"
                        rowTemp("BSTZD") = "Händlereigene Zulassung"
                End Select
                If CStr(rowTemp("CMGST")) = "B" Then
                    rowTemp("CMGST") = "X"
                Else
                    rowTemp("CMGST") = ""
                End If

                rowTemp("Adresse") = CStr(rowTemp("Name1_ZF")) & "<br>" & CStr(rowTemp("Name2_ZF")) & "<br>" & rowTemp("ORT01_ZF").ToString

            Next

            m_tblRaw.AcceptChanges()
            m_tblAuftraege = CreateOutPut(m_tblRaw, m_strAppID)
            m_tblResultExcel = m_tblAuftraege.Copy
            m_tblResultExcel.Columns.Remove("VBELN")
            m_tblResultExcel.Columns.Remove("EQUNR")
            m_tblResultExcel.Columns.Remove("KVGR3")

            If m_tblAuftraege.Rows.Count = 0 Then
                RaiseError("0", "Keine Daten gefunden.")
            End If

            If Not m_strHaendler Is Nothing AndAlso m_strHaendler.Trim.Length > 0 Then
                Haendler = m_strHaendler
            End If
        Catch ex As Exception
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("NO_DATA") Then
                If m_hez = True Then
                    RaiseError("-2501", "Keine Daten gefunden.")
                Else
                    RaiseError("0", "Keine Daten gefunden.")
                End If
            Else
                RaiseError("-9999", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            End If
            
        End Try
    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String)

        Try
            ClearError()

            S.AP.InitExecute("Z_M_Offeneanfor_Storno_001", "I_AG, I_HAENDLER, I_EQUNR, I_VBELN, I_ERNAM",
                             Right("0000000000" & m_objUser.KUNNR, 10), Right("0000000000" & StornoHaendler, 10), m_strEQUNR, m_strVBELN, Left(m_objUser.UserName, 12))

        Catch ex As Exception
            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("NO_UPDATE") Then
                RaiseError("-3501", "Kein EQUI-UPDATE.")
            ElseIf errormessage.Contains("NO_ZDADVERSAND") Then
                RaiseError("-3502", "Kein ZDADVERSAND-STORNO.")
            ElseIf errormessage.Contains("NO_UPDATE_SALESDOCUMENT") Then
                RaiseError("-3503", "Keine Auftragsänderung.")
            ElseIf errormessage.Contains("ZVERSAND_SPERRE") Then
                RaiseError("-3504", "ZVERSAND vom DAD gesperrt.")
            ElseIf errormessage.Contains("NO_PICKLISTE") Then
                RaiseError("-3505", "Kein Picklisteneintrag gefunden.")
            ElseIf errormessage.Contains("NO_ZCREDITCONTROL") Then
                RaiseError("-3506", "Kein Creditcontroleintrag gefunden.")
            Else
                RaiseError("-9999", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
            End If
        End Try
    End Sub

    Public Overrides Sub Change()
    End Sub    
  
#End Region
End Class

' ************************************************
' $History: fin_13.vb $
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 23.06.09   Time: 9:13
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ita 2918 Z_M_OFFENEANFOR_STORNO_001, Z_M_OFFENEANFOR_001
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 6.06.08    Time: 10:31
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 1954
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 28.04.08   Time: 14:18
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 1811
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance/Lib
' 
' *****************  Version 16  *****************
' User: Jungj        Date: 13.03.08   Time: 10:42
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' AKF Anpassungen Offene Anforderungen Bank/offene Anforderungen Händler
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 5.03.08    Time: 18:08
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' AKF Änderungen 1733
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 1.03.08    Time: 13:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Änderung der Händlernummer auf 10 stellen mit führenden 0 
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 26.02.08   Time: 16:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1733
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 25.02.08   Time: 15:51
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ita 1733
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 25.02.08   Time: 15:05
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ita 1733
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 22.01.08   Time: 9:25
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 8  *****************
' User: Uha          Date: 8.01.08    Time: 17:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Bugfix RTFS
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 20.12.07   Time: 13:08
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Testfertig
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 20.12.07   Time: 10:59
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Testfertig
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 20.12.07   Time: 9:09
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 19.12.07   Time: 17:46
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' kompilierfähig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 19.12.07   Time: 13:24
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' auf Testdaten wartend
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 19.12.07   Time: 11:09
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1513/1491 change46/fin_13 torso hinzugefügt
' 
' ************************************************