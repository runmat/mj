Option Explicit On
Option Infer On
Option Strict On

Imports CKG
Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel.Common

Public Class Fristablauf
    Inherits Base.Business.DatenimportBase

#Region " Declarations"


    Private m_strFilename2 As String



#End Region

#Region " Properties"
    Public ReadOnly Property FileName() As String
        Get
            Return m_strFilename2
        End Get
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_strFilename2 = strFilename
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID)

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "Fristablauf.Fill"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            m_intStatus = 0

            Try
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_FEHLENDE_COC ", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                Dim proxy = DynSapProxy.getProxy("Z_M_BRIEF_1MAL_GEMAHNT", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                proxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

                proxy.callBapi()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                'auswerten der exportparameter
                Dim tblTemp As DataTable = proxy.getExportTable("E_EXP_BRIEFE")

                tblTemp.Columns.Add("Versandadresse", GetType(String))

                For Each row As DataRow In tblTemp.Rows
                    Dim strTemp As String = CStr(row("Name1"))
                    If Not TypeOf row("Name2") Is DBNull Then
                        strTemp &= " " & CStr(row("Name2"))
                    End If
                    If Not TypeOf row("Name3") Is DBNull Then
                        strTemp &= " " & CStr(row("Name3"))
                    End If
                    If Not TypeOf row("Street") Is DBNull Then
                        strTemp &= ", " & CStr(row("Street"))
                    End If
                    If Not TypeOf row("House_Num1") Is DBNull Then
                        strTemp &= " " & CStr(row("House_Num1"))
                    End If
                    If Not TypeOf row("Post_Code1") Is DBNull Then
                        strTemp &= ", " & CStr(row("Post_Code1"))
                    End If
                    If Not TypeOf row("City1") Is DBNull Then
                        strTemp &= " " & CStr(row("City1"))
                    End If
                    row("Versandadresse") = strTemp
                Next

                CreateOutPut(tblTemp, strAppID)
                HelpProcedures.killAllDBNullValuesInDataTable(Result)

                WriteLogEntry(True, "", Result)
            Catch ex As Exception
                ResultTable = Nothing
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
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

' ************************************************
' $History: Fristablauf.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 15.04.09   Time: 11:01
' Created in $/CKAG2/Applications/AppAkf/lib
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 18.11.08   Time: 9:12
' Updated in $/CKAG/Applications/appakf/Lib
' ITA 2384 testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 13.11.08   Time: 15:29
' Created in $/CKAG/Applications/appakf/Lib
' ITA 2384
' 
' ************************************************
