Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class FFE_Bank_Haendlerfinanzierungen
    REM § Status-Report, Kunde: FFE, BAPI: Z_M_HaendlerFinanzierung,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Protected m_strFahrzeugtyp As String
#End Region

#Region " Properties"
    Public Property Fahrzeugtyp() As String
        Get
            Return m_strFahrzeugtyp
        End Get
        Set(ByVal Value As String)
            m_strFahrzeugtyp = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFahrzeugtyp As String, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_strFahrzeugtyp = strFahrzeugtyp
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "FFD_Bank_Haendlerfinanzierungen.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            Dim intID As Int32 = -1
            Dim strKONZS As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try
                Dim SAPTable As New SAPProxy_FFE.ZDAD_M_WEB_HAENDLERFINANZTable()

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Haendlerfinanzierung", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                Dim strNeuwagen As String = " "
                Dim strVorfuehrwagen As String = " "
                Dim strDirekt As String = " "
                Select Case m_strFahrzeugtyp
                    Case "FA11"
                        strNeuwagen = "X"
                    Case "FA12"
                        strVorfuehrwagen = "X"
                    Case "FA14"
                        strDirekt = "X"
                End Select


                objSAP.Z_M_Haendlerfinanzierung(strDirekt, strKONZS, strNeuwagen, strVorfuehrwagen, SAPTable)
                objSAP.CommitWork()
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = SAPTable.ToADODataTable
                Dim tmpRows As DataRow
                Dim tmpStr As String = ""
                For Each tmpRows In tblTemp2.Rows
                    tmpStr = tmpRows("KUNNR_ZF").ToString
                    If tmpStr.Length > 5 Then

                        tmpRows("KUNNR_ZF") = Right(tmpStr, 5)

                    End If

                Next
                tblTemp2.AcceptChanges()
                CreateOutPut(tblTemp2, strAppID)



                WriteLogEntry(True, "Fahrzeugtyp=" & m_strFahrzeugtyp, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -9999
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten gefunden."
                    Case "PARAMETER"
                        m_strMessage = "Es können nicht Neu- und Vorführwagen gleichzeitig ausgewählt werden."
                    Case "NO_PARAMETER"
                        m_strMessage = "Es ist weder Neu- noch Vorführwagen ausgewählt worden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "Fahrzeugtyp=" & m_strFahrzeugtyp & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
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
' $History: FFE_Bank_Haendlerfinanzierungen.vb $
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugef�gt
' 
' ************************************************
