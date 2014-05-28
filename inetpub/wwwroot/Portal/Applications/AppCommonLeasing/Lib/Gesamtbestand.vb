Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
'Imports Microsoft.Data.SAPClient
Imports CKG.Base.Business
Imports CKG.Base.Business.DatenimportBase

Public Class Gesamtbestand
    Inherits CKG.Base.Business.DatenimportBase

#Region " Declarations"
    Private mFileName As String
#End Region

#Region " Properties"

    Public ReadOnly Property FileName() As String
        Get
            Return mFileName
        End Get
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strFileName)
        mFileName = strFileName
        AppID = strAppID
        SessionID = strSessionID
    End Sub

    Public Overrides Sub Fill()
        m_strClassAndMethod = "Gesamtbestand.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            m_intStatus = 0


            'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            'con.Open()
            Try
                S.AP.InitExecute("Z_KD_BESTAND_LESEN", "I_KUNNR,I_VINKULIER", Right("0000000000" & m_objUser.KUNNR, 10), "X")

                'Dim cmd As New SAPCommand()
                'cmd.Connection = con

                'Dim strCom As String

                'strCom = "EXEC Z_KD_BESTAND_LESEN @I_KUNNR=@pI_KUNNR,@I_VINKULIER=@pI_VINKULIER,@GT_WEB=@pE_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                'cmd.CommandText = strCom


                ''importparameter
                'Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)
                'Dim pI_VINKULIER As New SAPParameter("@pI_VINKULIER", ParameterDirection.Input)

                ''exportParameter
                'Dim pE_GT_WEB As New SAPParameter("@pE_GT_WEB", ParameterDirection.Output)

                ''Importparameter hinzufügen
                'cmd.Parameters.Add(pI_KUNNR)
                'cmd.Parameters.Add(pI_VINKULIER)


                ''exportparameter hinzugfügen
                'cmd.Parameters.Add(pE_GT_WEB)

                ''befüllen der Importparameter
                'pI_KUNNR.Value = Right("0000000000" & m_objUser.KUNNR, 10)
                'pI_VINKULIER.Value = "X" 'Wenn Vinkulierung mit selektiert werden soll, Ösis

                'If m_objLogApp Is Nothing Then
                '    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                'End If

                'intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_KD_BESTAND_LESEN", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                'cmd.ExecuteNonQuery()

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                'End If

                'auswerten der exportparameter
                'If Not pE_GT_WEB.Value Is DBNull.Value Then


                ResultTable = S.AP.GetExportTable("GT_WEB") 'DirectCast(pE_GT_WEB.Value, DataTable)

                ResultTable.Columns.Add("Versandadresse", String.Empty.GetType)

                'HelpProcedures.killAllDBNullValuesInDataTable(ResultTable)

                For Each tmpRow As DataRow In ResultTable.Rows
                    'versandadresse zusammenbauen
                    tmpRow("Versandadresse") = CStr(tmpRow("NAME1")) & "<br>" & CStr(tmpRow("STREET")) & "<br>" & CStr(tmpRow("COUNTRY")) & " " & CStr(tmpRow("POST_CODE1")) & " " & CStr(tmpRow("CITY1"))

                    'standort
                    Select Case tmpRow("ABCKZ").ToString
                        Case "1"
                            tmpRow("ABCKZ") = "temporär versendet"
                        Case "2"
                            tmpRow("ABCKZ") = "endgültig versendet"
                        Case Else
                            tmpRow("ABCKZ") = "eingelagert"
                    End Select

                    'Von Sap Boolean to Sprache 
                    If tmpRow("ZZCOCKZ").ToString.ToUpper = "X" Then
                        tmpRow("ZZCOCKZ") = "Ja"
                    Else
                        tmpRow("ZZCOCKZ") = "Nein"
                    End If
                Next

                ResultTable.AcceptChanges()
                CreateOutPut(ResultTable, AppID)
                'End If
            Catch ex As Exception
                ResultTable = Nothing
                m_intStatus = -9999

                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                'End If
            Finally
                'If intID > -1 Then
                '    m_objLogApp.WriteStandardDataAccessSAP(intID)
                'End If

                'con.Close()

                m_blnGestartet = False
            End Try
        End If

    End Sub


#End Region

End Class

' ************************************************
' $History: Gesamtbestand.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 6.11.08    Time: 13:34
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2365,2367,2362
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 31.10.08   Time: 13:42
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2289 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 30.10.08   Time: 16:47
' Updated in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2289 warte auf bapi
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 14.10.08   Time: 15:14
' Created in $/CKAG/Applications/AppCommonLeasing/Lib
' ITA 2289 unfertig
'
' ************************************************