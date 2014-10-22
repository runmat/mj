Option Explicit On 
Option Strict On

Imports System
Imports CKG
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

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

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page, ByVal strHalterNr As String, ByVal vollst As String)
        m_strClassAndMethod = "Arval_04.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try
                Dim proxy = DynSapProxy.getProxy("Z_M_Zuldokumente_Arval", m_objApp, m_objUser, Page)

                proxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                proxy.setImportParameter("I_UNVOLLSTAENDIG", vollst)
                proxy.setImportParameter("I_ZHNAME", strHalterNr.ToUpper())

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Zuldokumente_Arval", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                proxy.callBapi()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = proxy.getExportTable("T_ZULDOKUMENTE")
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
                Select Case ex.Message
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
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "HALTER=" & strHalterNr & ", Art=" & vollst & ", KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                m_blnGestartet = False
            End Try
        End If
    End Sub
    Public Sub Update_EVB(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page, ByVal strHalterNr As String)
        m_strClassAndMethod = "Arval_04.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try
                Dim proxy = DynSapProxy.getProxy("Z_M_Zuldokumente_Evb", m_objApp, m_objUser, Page)

                proxy.setImportParameter("I_EVB_BIS", m_strEVBbis)
                proxy.setImportParameter("I_EVB_NR", m_strEVBNr)
                proxy.setImportParameter("I_EVB_VON", m_strEVBvon)
                proxy.setImportParameter("I_HALTER", Right("0000000000" & strHalterNr, 10))
                proxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Zuldokumente_Evb", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                proxy.callBapi()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                WriteLogEntry(True, "HALTER=" & strHalterNr & ", ab=" & m_strEVBvon & ", bis=" & m_strEVBbis & " Nr.=" & m_strEVBNr & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_UPDATE"
                        m_intStatus = -3331
                        m_strMessage = "EVB-Nummer konnte nicht gespeichert werden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "HALTER=" & strHalterNr & ", ab=" & m_strEVBvon & ", bis=" & m_strEVBbis & " Nr.=" & m_strEVBNr & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
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
