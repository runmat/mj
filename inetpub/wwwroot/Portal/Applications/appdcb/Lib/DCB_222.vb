Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class DCB_222
    REM § Status-Report, Kunde: DCB, BAPI: Z_M_Abm_Equi_Abmdat,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Dim m_strAbdatum As String
    Dim m_strBisdatum As String
#End Region

#Region " Properties"
    Property datAb() As String
        Get
            Return m_strAbdatum
        End Get
        Set(ByVal Value As String)
            m_strAbdatum = Value
        End Set
    End Property

    Property datBis() As String
        Get
            Return m_strBisdatum
        End Get
        Set(ByVal Value As String)
            m_strBisdatum = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "DCB_222.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                Dim m_strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

                S.AP.InitExecute("Z_M_ABM_EQUI_ABMDAT", "I_KUNNR,I_EXPIRY_DATE_VON,I_EXPIRY_DATE_BIS", m_strKUNNR, m_strAbdatum, m_strBisdatum)
                

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ABM_EQUI_ABMDAT", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", m_strKUNNR)
                'myProxy.setImportParameter("I_EXPIRY_DATE_VON", m_strAbdatum)
                'myProxy.setImportParameter("I_EXPIRY_DATE_BIS", m_strBisdatum)

                'myProxy.callBapi()

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_DATEN") 'myProxy.getExportTable("GT_DATEN")
                'Versandart im Klartext...

                Dim row As DataRow
                If tblTemp2.Rows.Count > 0 Then
                    tblTemp2.Columns("ABCKZ").MaxLength = 5
                End If
                For Each row In tblTemp2.Rows

                    If Not (TypeOf row("ABCKZ") Is DBNull) Then
                        If CType(row("ABCKZ"), String) = "1" Then
                            row("ABCKZ") = "temp."
                        End If
                        If CType(row("ABCKZ"), String) = "2" Then
                            row("ABCKZ") = "endg."
                        End If
                    End If
                    tblTemp2.AcceptChanges()
                Next

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Keine Fahrzeuge gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If

    End Sub

#End Region

End Class

' ************************************************
' $History: DCB_222.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 4.02.10    Time: 13:56
' Updated in $/CKAG/Applications/appdcb/Lib
' ITA: 2918
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 8:39
' Updated in $/CKAG/Applications/appdcb/Lib
' ITA 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 11:52
' Created in $/CKAG/Applications/appdcb/Lib
' 
' *****************  Version 6  *****************
' User: Uha          Date: 2.07.07    Time: 16:50
' Updated in $/CKG/Applications/AppDCB/AppDCBWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 5  *****************
' User: Uha          Date: 7.03.07    Time: 9:53
' Updated in $/CKG/Applications/AppDCB/AppDCBWeb/Lib
' 
' ************************************************
