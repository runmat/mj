Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG

Public Class Porsche_06c
    REM § Lese-/Schreibfunktion, Kunde: FFD, 
    REM § GiveCars - BAPI: Z_M_Unangefordert,
    REM § Anfordern - BAPI: Z_M_Briefanforderung.

    Inherits Base.Business.BankBaseCredit

#Region " Declarations"
    Private m_tblKontingente As DataTable
#End Region

#Region " Properties"
    Public Property PKontingente() As DataTable
        Get
            Return m_tblKontingente
        End Get
        Set(ByVal Value As DataTable)
            m_tblKontingente = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overrides Sub Change()
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_PORSCHE.SAPProxy_PORSCHE()

            MakeDestination()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1
            Dim strKontingentart As String = ""

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim tmpRow As DataRow
                For Each tmpRow In m_tblKontingente.Rows

                    Dim decKreditlimit_Alt As Decimal = CType(tmpRow("Kontingent_Alt"), Decimal)
                    Dim decKreditlimit_Neu As Decimal = CType(tmpRow("Kontingent_Neu"), Decimal)

                    Dim blnGesperrt_Alt As Boolean = CType(tmpRow("Gesperrt_Alt"), Boolean)
                    Dim blnGesperrt_Neu As Boolean = CType(tmpRow("Gesperrt_Neu"), Boolean)
                    strKontingentart = CType(tmpRow("Kontingentart"), String)

                    m_strKUNNR = Right("0000000000" & m_objUser.KUNNR, 10)

                    If (Not (blnGesperrt_Alt = blnGesperrt_Neu)) Then
                        Dim tblAuftraege As SAPProxy_PORSCHE.ZDAD_M_WEB_AUFTRAEGETable
                        m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Creditlimit_Change", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                        If blnGesperrt_Neu Then
                            objSAP.Z_M_Creditlimit_Change_Porsche("X", m_strInternetUser, CType(tmpRow("Kreditkontrollbereich"), String), decKreditlimit_Neu, m_strKUNNR, m_strCustomer, "Change06.aspx", tblAuftraege)
                        Else
                            objSAP.Z_M_Creditlimit_Change_Porsche(" ", m_strInternetUser, CType(tmpRow("Kreditkontrollbereich"), String), decKreditlimit_Neu, m_strKUNNR, m_strCustomer, "Change06.aspx", tblAuftraege)
                        End If
                    End If
                    objSAP.CommitWork()
                    If m_intIDSAP > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                    End If
                Next
                'End If
            Catch ex As Exception
                Select Case ex.Message
                    Case "NO_DATA"
                        m_intStatus = -2221
                        m_strMessage = "Kontingentart """ & strKontingentart & """ nicht gefunden."
                    Case "NO_ZCREDITCONTROL"
                        m_intStatus = -2222
                        m_strMessage = "Kontingentart """ & strKontingentart & """ konnte nicht geändert werden. (Update-Fehler ZCREDITCONTROL)"
                    Case "NO_ZDADVERSAND"
                        m_intStatus = -2223
                        m_strMessage = "Kontingentart """ & strKontingentart & """ konnte nicht geändert werden. (Insert-Fehler ZDADVERSAND)"
                    Case "ZCREDITCONTROL_SPERRE"
                        m_intStatus = -2224
                        m_strMessage = "Kontingentart """ & strKontingentart & """ konnte nicht geändert werden. (ZCREDITCONTROL vom DAD gesperrt)"
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
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
' $History: Porsche_06c.vb $
' 
' *****************  Version 1  *****************
' User: Rudolpho     Date: 7.04.09    Time: 15:20
' Created in $/CKAG2/Applications/AppPorsche/lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 11:28
' Created in $/CKAG/Applications/AppPorsche/Lib
' 
' *****************  Version 5  *****************
' User: Uha          Date: 2.07.07    Time: 18:24
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 4  *****************
' User: Uha          Date: 5.03.07    Time: 12:50
' Updated in $/CKG/Applications/AppPorsche/AppPorscheWeb/Lib
' 
' ************************************************