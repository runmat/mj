Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel

Public Class Arval_04
    REM § Status-Report, Kunde: Arval, BAPI: Z_M_Zuldokumente_Arval,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_strEVBNr As String
    Private m_strEVBvon As String
    Private m_strEVBbis As String
#End Region

#Region " Properties"
    Public Property EVBNr() As String
        Get
            Return m_strEVBNr
        End Get
        Set(ByVal Value As String)
            m_strEVBNr = Value
        End Set
    End Property

    Public Property EVBvon() As String
        Get
            Return m_strEVBvon
        End Get
        Set(ByVal Value As String)
            m_strEVBvon = Value
        End Set
    End Property

    Public Property EVBbis() As String
        Get
            Return m_strEVBbis
        End Get
        Set(ByVal Value As String)
            m_strEVBbis = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal strHalterNr As String, ByVal vollst As String)
        m_strClassAndMethod = "Arval_04.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                S.AP.Init("Z_M_Zuldokumente_Arval")
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Zuldokumente_Arval", m_objApp, m_objUser, Page)

                S.AP.SetImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_ZHNAME", strHalterNr.ToUpper)
                S.AP.SetImportParameter("I_ZHPLZ", "")
                S.AP.SetImportParameter("I_UNVOLLSTAENDIG", vollst)

                S.AP.Execute() 'myProxy.callBapi()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("T_ZULDOKUMENTE")

                Dim row As DataRow

                For Each row In tblTemp2.Rows
                    If CType(row("Karte"), String) = "000" Then
                        row("KARTE") = ""
                    Else
                        row("KARTE") = "X"
                    End If
                Next

                CreateOutPut(tblTemp2, strAppID)
                tblTemp2.AcceptChanges()

                WriteLogEntry(True, "HALTER=" & strHalterNr & ", Art=" & vollst & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "ERR_INV_AG"
                        m_intStatus = -3331
                        m_strMessage = "Ungültige Kundennummer."
                    Case "ERR_NO_DATA"
                        m_intStatus = -3332
                        m_strMessage = "Keine Daten gefunden."
                    Case "ERR_NO_PARAMETER"
                        m_intStatus = -3333
                        m_strMessage = "Unzureichende Parameter."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                WriteLogEntry(False, "HALTER=" & strHalterNr & ", Art=" & vollst & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Update_EVB(ByVal strAppID As String, ByVal strSessionID As String, ByVal strHalterNr As String)
        m_strClassAndMethod = "Arval_04.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                S.AP.Init("Z_M_Zuldokumente_Evb")
                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Zuldokumente_Evb", m_objApp, m_objUser, page)

                S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("I_HALTER", Right("0000000000" & strHalterNr, 10))
                S.AP.SetImportParameter("I_STANDORT", "")
                S.AP.SetImportParameter("I_EVB_NR", m_strEVBNr)
                S.AP.SetImportParameter("I_EVB_VON", m_strEVBvon)
                S.AP.SetImportParameter("I_EVB_BIS", m_strEVBbis)

                S.AP.Execute() 'myProxy.callBapi()

                WriteLogEntry(True, "HALTER=" & strHalterNr & ", ab=" & m_strEVBvon & ", bis=" & m_strEVBbis &
                              " Nr.=" & m_strEVBNr & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_UPDATE"
                        m_intStatus = -3331
                        m_strMessage = "EVB-Nummer konnte nicht gespeichert werden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                WriteLogEntry(False, "HALTER=" & strHalterNr & ", ab=" & m_strEVBvon & ", bis=" & m_strEVBbis & " Nr.=" & m_strEVBNr & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: Arval_04.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 30.06.09   Time: 13:03
' Updated in $/CKAG/Applications/apparval/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:18
' Created in $/CKAG/Applications/apparval/Lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 19.03.08   Time: 14:35
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 10.03.08   Time: 10:15
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 3  *****************
' User: Uha          Date: 6.03.07    Time: 14:48
' Updated in $/CKG/Applications/AppARVAL/AppARVALWeb/Lib
' 
' ************************************************
