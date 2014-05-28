Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class fw_02

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"

    Private m_datAbDatum As Date
    Private m_datBisDatum As Date
#End Region

#Region " Properties"

   

    Public Property datVON() As Date
        Get
            Return m_datAbDatum
        End Get
        Set(ByVal Value As Date)
            m_datAbDatum = Value
        End Set
    End Property

    Public Property datBIS() As Date
        Get
            Return m_datBisDatum
        End Get
        Set(ByVal Value As Date)
            m_datBisDatum = Value
        End Set
    End Property

    Public ReadOnly Property FileName() As String
        Get
            Return m_strFileName
        End Get
    End Property

#End Region

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal datAbDatum As Date, ByVal datBisDatum As Date, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)

        m_datAbDatum = datAbDatum
        m_datBisDatum = datBisDatum


    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strsessionid)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "fw_02.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_strMessage = ""
        m_intStatus = 0
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim objSAP As New SAPProxy_FW.SAPProxy_FW()
            'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            'objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            Dim intID As Int32 = -1

            Try
                'Dim SAPTable As New SAPProxy_FW.ZDAD_M_WEB_LT_VERSENDETE_ZB2Table()

                intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Versendete_Zb2_Endg_L", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)

                'objSAP.Z_M_Versendete_Zb2_Endg_Lt("", "", Right("0000000000" & m_objUser.KUNNR, 10), "", "", "1510", HelpProcedures.MakeDateSAP(m_datBisDatum), HelpProcedures.MakeDateSAP(m_datAbDatum), SAPTable)
                'objSAP.CommitWork()

                S.AP.InitExecute("Z_M_VERSENDETE_ZB2_ENDG_LT", "I_KUNNR_AG,I_VKORG,I_CHASSIS_NUM,I_LICENSE_NUM,I_LIZNR,I_ZZTMPDT_VON,I_ZZTMPDT_BIS,I_KONZS_ZK",
                                                                Right("0000000000" & m_objUser.KUNNR, 10),
                                                                "1510",
                                                                "",
                                                                "",
                                                                "",
                                                                HelpProcedures.MakeDateSAP(m_datAbDatum),
                                                                HelpProcedures.MakeDateSAP(m_datBisDatum),
                                                                "")


                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")
                Dim rowTemp As DataRow
                For Each rowTemp In tblTemp2.Rows
                    Select Case CStr(rowTemp("ABCKZ"))
                        Case ""
                            rowTemp("ABCKZ") = "DAD"
                        Case "1"
                            rowTemp("ABCKZ") = "temporär"
                        Case "2"
                            rowTemp("ABCKZ") = "endgültig"
                        Case "5"
                            rowTemp("ABCKZ") = "Händler Zulassung"
                    End Select
                Next

                'For Each rowTemp In tblTemp2.Rows
                '    Dim strgrund As String
                '    If CStr(rowTemp("ZZVGRUND")).Length > 0 Then
                '        strgrund = Abrufgrund(CStr(rowTemp("ZZVGRUND")))
                '    Else
                '        strgrund = "keine Auswahl"
                '    End If
                '    rowTemp("ZZVGRUND") = strgrund
                'Next
                'tblTemp2.Columns.Add("Adresse", System.Type.GetType("System.String"))
                'For Each rowTemp In tblTemp2.Rows
                '    rowTemp("Adresse") = rowTemp("Name1").ToString & ", " & rowTemp("Post_Code1").ToString & " " & rowTemp("City1").ToString & ", " & rowTemp("Street").ToString & " " & rowTemp("House_NUM1").ToString
                'Next


                CreateOutPut(tblTemp2, strAppID)


                WriteLogEntry(True, "DATANF=" & m_datAbDatum.ToShortDateString & ", DATEND=" & m_datBisDatum.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception

                Select Case (ex.Message)
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "keine Ergebnisse gefunden"
                    Case Else
                        m_intStatus = -2222
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

                If intID > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "DATANF=" & m_datAbDatum.ToShortDateString & " DATEND=" & m_datBisDatum.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(intID)
                End If

                'objSAP.Connection.Close()
                'objSAP.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub
End Class
