Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
'Imports Microsoft.Data.SAPClient
Imports CKG.Base.Common

Public Class Sixt_B11
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Schluesseldifferenzen,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datErfassungsdatumVon As DateTime
    Private m_datErfassungsdatumBis As DateTime
    Private m_tblHistory As DataTable
    Private m_strCHASSIS_NUM As String
    Private m_strEQUNR As String
    Private m_blnUnvollstaendigeTuete As Boolean
#End Region

#Region " Properties"
    Public Property UnvollstaendigeTuete() As Boolean
        Get
            Return m_blnUnvollstaendigeTuete
        End Get
        Set(ByVal Value As Boolean)
            m_blnUnvollstaendigeTuete = Value
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

    Public Property CHASSIS_NUM() As String
        Get
            Return m_strCHASSIS_NUM
        End Get
        Set(ByVal Value As String)
            m_strCHASSIS_NUM = Value
        End Set
    End Property

    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Sixt_B11.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1

            Try


                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_SCHLUESSELDIFFERENZEN", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))


                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_SCHLUESSELDIFFERENZEN", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                Dim tblTemp2 As DataTable

                'If m_blnUnvollstaendigeTuete Then
                '    tblTemp2 = myProxy.getExportTable("GT_WEB_OUT_TUETE")
                'Else
                '    tblTemp2 = myProxy.getExportTable("GT_WEB_OUT_BRIEFE")
                'End If

                If m_blnUnvollstaendigeTuete Then
                    tblTemp2 = S.AP.GetExportTable("GT_WEB_OUT_TUETE")
                Else
                    tblTemp2 = S.AP.GetExportTable("GT_WEB_OUT_BRIEFE")
                End If

                CreateOutPut(tblTemp2, strAppID)

                Dim row As DataRow

                m_tblResult.Columns.Add("Zulassungsdatum", GetType(System.DateTime))
                For Each row In m_tblResult.Rows
                    If Not (TypeOf row("Erstzulassungsdatum") Is System.DBNull) Then
                        row("Zulassungsdatum") = CDate(row("Erstzulassungsdatum"))
                    End If
                Next
                m_tblResult.AcceptChanges()

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)


            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Clear(ByVal strAppID As String, ByVal strSessionID As String, ByRef myWebTable As DataTable)
        m_strClassAndMethod = "Sixt_B11.Clear"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

            'con.Open()
            'Dim cmd As New SAPCommand()
            'Dim strCom As String


            Dim intID As Int32 = -1
            Try
                Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)

                'Dim SAPTable As New DataTable


                'With SAPTable
                '    .Columns.Add("MANDT", System.Type.GetType("System.String"))
                '    .Columns.Add("KUNNR", System.Type.GetType("System.String"))
                '    .Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
                '    .Columns.Add("EQUNR", System.Type.GetType("System.String"))
                '    .Columns.Add("EQTYP", System.Type.GetType("System.String"))
                '    .Columns.Add("EQFNR", System.Type.GetType("System.String"))
                '    .Columns.Add("LICENSE_NUM", System.Type.GetType("System.String"))
                '    .Columns.Add("REPLA_DATE", System.Type.GetType("System.String"))
                '    .Columns.Add("TIDNR", System.Type.GetType("System.String"))
                '    .Columns.Add("LIZNR", System.Type.GetType("System.String"))
                '    .Columns.Add("ABCKZ", System.Type.GetType("System.String"))
                '    .Columns.Add("KUNPDI", System.Type.GetType("System.String"))
                '    .Columns.Add("FLAG", System.Type.GetType("System.String"))
                '    .Columns.Add("ERSSCHLUESSEL", System.Type.GetType("System.String"))
                '    .Columns.Add("CARPASS", System.Type.GetType("System.String"))
                '    .Columns.Add("RADIOCODEKARTE", System.Type.GetType("System.String"))
                '    .Columns.Add("NAVICD", System.Type.GetType("System.String"))
                '    .Columns.Add("CHIPKARTE", System.Type.GetType("System.String"))
                '    .Columns.Add("COCBESCH", System.Type.GetType("System.String"))
                '    .Columns.Add("NAVICODEKARTE", System.Type.GetType("System.String"))
                '    .Columns.Add("WFSCODEKARTE", System.Type.GetType("System.String"))
                '    .Columns.Add("SH_ERS_FB", System.Type.GetType("System.String"))
                '    .Columns.Add("PRUEFBUCH_LKW", System.Type.GetType("System.String"))
                '    .Columns.Add("EMPTY", System.Type.GetType("System.String"))
                '    .Columns.Add("OBJNR", System.Type.GetType("System.String"))
                '    .Columns.Add("AUSNAHME", System.Type.GetType("System.String"))
                '    .Columns.Add("MELDDAT", System.Type.GetType("System.String"))
                '    .Columns.Add("MAHN_1", System.Type.GetType("System.String"))
                '    .Columns.Add("MAHN_2", System.Type.GetType("System.String"))
                '    .Columns.Add("ZZREFERENZ1", System.Type.GetType("System.String"))
                'End With

                myWebTable.Columns.Add("Status", GetType(System.String))
                myWebTable.AcceptChanges()

                S.AP.Init("Z_M_Schluesselverloren", "I_KUNNR", strKUNNR)

                Dim SAPTable As DataTable = S.AP.GetImportTable("GT_WEB_IN")

                For Each tmpRow As DataRow In myWebTable.Rows

                    If CBool(tmpRow("Delete")) Then
                        Dim Row As DataRow = SAPTable.NewRow
                        Row("KUNNR") = strKUNNR
                        Row("Chassis_Num") = CStr(tmpRow("Fahrg-Nr"))
                        Row("Equnr") = CStr(tmpRow("Equipmentnummer"))
                        Row("Flag") = "X"
                        SAPTable.Rows.Add(Row)
                        tmpRow("Status") = "OK"
                    End If

                Next
                myWebTable.AcceptChanges()
                SAPTable.AcceptChanges()


                'intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Schluesselverloren", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)


                'strCom = "EXEC Z_M_Schluesselverloren @GT_WEB_IN=@pSAPTable,@I_KUNNR='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'" _
                '        & " OPTION 'disabledatavalidation'"

                'cmd.Connection = con
                'cmd.CommandText = strCom


                ''Importparameter
                'Dim pSAPTable As New SAPParameter("@pSAPTable", SAPTable)
                'cmd.Parameters.Add(pSAPTable)


                'cmd.ExecuteNonQuery()

                S.AP.Execute()

                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                'End If
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", CHASSIS_NUM=" & m_strCHASSIS_NUM & ", EQUNR=" & m_strEQUNR, m_tblResult, True)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case Else
                        m_strMessage = ex.Message
                End Select
                'If intID > -1 Then
                '    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                'End If
                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", CHASSIS_NUM=" & m_strCHASSIS_NUM & ", EQUNR=" & m_strEQUNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, True)
            Finally
                'con.Close()
                'con.Dispose()

                'cmd.Dispose()

                m_blnGestartet = False
            End Try
        End If
    End Sub


#End Region
End Class

' ************************************************
' $History: Sixt_B11.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 7.01.11    Time: 8:49
' Updated in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 16.02.10   Time: 11:40
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 28.01.09   Time: 11:32
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA 2431 nachbesserungen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 3.12.08    Time: 13:16
' Updated in $/CKAG/Applications/appsixt/Lib
' ITA 2431 fertigstellung
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:37
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 12  *****************
' User: Uha          Date: 3.07.07    Time: 9:25
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 11  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
