Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel


Public Class FFE_AbgleichRetail
    Inherits Base.Business.DatenimportBase



    '#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)

    End Sub


#Region "Declarations"

    Private m_dteBaseDate As Date

#End Region

#Region "Methods"

    Public Overloads Overrides Sub Fill()
        FILL(m_strAppID, m_strSessionID, m_dteBaseDate)
    End Sub


    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal dteBaseDate As Date)
        Dim intID As Int32 = -1

        'Kundennummer ermitteln und in SAP-Format wandeln
        Dim strKUNNR As String = Right("0000000000" & Me.m_objUser.Customer.KUNNR, 10)

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Try
                Dim SAPTable As New SAPProxy_FFE.ZDAD_ABGLEICH_RETAIL_S001Table()

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_V_Abgleich_Retail", Me.m_strAppID, Me.m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                objSAP.Z_V_Abgleich_Retail("0000324616", strKUNNR, SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & strKUNNR, m_tblResult, False)

            Catch ex As Exception
                Select Case ex.Message
                    Case "NRF"
                        m_intStatus = -1402
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -5555
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & strKUNNR & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                objSAP.Connection.Close()
                objSAP.Dispose()

                m_blnGestartet = False
            End Try

        End If

    End Sub

#End Region


End Class
' ************************************************
' $History: FFE_AbgleichRetail.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugef�gt
' 
' ************************************************
