Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class ec_10
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Schluesseldifferenzen,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

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



    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        Dim tblTemp2 As DataTable
        Dim tblHerst As DataTable
        Dim intCounter As Integer
        Dim datZulassung As Date
        Dim datGrenze As Date

        m_strClassAndMethod = "EC_10.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        Try
            Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Schluesseldifferenzen", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", strKUNNR)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Schluesseldifferenzen", "I_KUNNR", strKUNNR)

            tblTemp2 = New DataTable()
            tblTemp2 = S.AP.GetExportTable("GT_WEB_OUT_BRIEFE") 'myProxy.getExportTable("GT_WEB_OUT_BRIEFE")
            tblHerst = S.AP.GetExportTable("GT_WEB_OUT_HERST") 'myProxy.getExportTable("GT_WEB_OUT_HERST")

            Dim row As DataRow()

            tblTemp2.Columns.Add("Hersteller", GetType(System.String))
            tblTemp2.Columns.Add("Delete", GetType(System.Boolean))
            tblTemp2.Columns.Add("Status", GetType(System.String))

            For intCounter = tblTemp2.Rows.Count - 1 To 0 Step -1
                If IsDate(tblTemp2.Rows(intCounter)("REPLA_DATE")) Then datZulassung = CDate(tblTemp2.Rows(intCounter)("REPLA_DATE"))

                datGrenze = Now.Date.Subtract(New TimeSpan(21, 0, 0, 0))

                tblTemp2.Rows(intCounter)("Delete") = False
                tblTemp2.Rows(intCounter)("Status") = String.Empty

                If (datZulassung > datGrenze) Then
                    tblTemp2.Rows.Remove(tblTemp2.Rows(intCounter))
                Else
                   row = tblHerst.Select("CHASSIS_NUM='" & CStr(tblTemp2.Rows(intCounter)("CHASSIS_NUM")) & "'")

                    If Not (row.Length = 0) Then
                        tblTemp2.Rows(intCounter)("Hersteller") = CStr(row(0)("HERST_T"))
                    Else
                        tblTemp2.Rows(intCounter)("Hersteller") = String.Empty
                    End If
                End If
            Next

            tblTemp2.AcceptChanges()

            CreateOutPut(tblTemp2, strAppID)

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
          
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Public Sub Clear(ByVal strAppID As String, ByVal strSessionID As String, ByRef myWebTable As DataTable)
        m_strClassAndMethod = "Avis04.Clear"
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

                S.AP.Init("Z_M_Schluesselverloren", "I_KUNNR", strKUNNR)
                Dim SAPTable As DataTable = S.AP.GetImportTable("GT_WEB_IN")

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


                For Each tmpRow As DataRow In myWebTable.Rows

                    If CBool(tmpRow("Delete")) Then
                        Dim Row As DataRow = SAPTable.NewRow
                        Row("KUNNR") = strKUNNR
                        Row("Chassis_Num") = CStr(tmpRow("Fahrgestellnummer"))
                        Row("Equnr") = CStr(tmpRow("Equipmentnummer"))
                        Row("Flag") = "X"
                        SAPTable.Rows.Add(Row)
                        tmpRow("Status") = "OK"
                    End If

                Next
                myWebTable.AcceptChanges()
                SAPTable.AcceptChanges()


                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Schluesselverloren", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)


                'strCom = "EXEC Z_M_Schluesselverloren @GT_WEB_IN=@pSAPTable,@I_KUNNR='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'" _
                '        & " OPTION 'disabledatavalidation'"

                'cmd.Connection = con
                'cmd.CommandText = strCom


                ''Importparameter
                'Dim pSAPTable As New SAPParameter("@pSAPTable", SAPTable)
                'cmd.Parameters.Add(pSAPTable)


                'cmd.ExecuteNonQuery()

                S.AP.Execute()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", CHASSIS_NUM=" & m_strCHASSIS_NUM & ", EQUNR=" & m_strEQUNR, m_tblResult, True)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case Else
                        m_strMessage = ex.Message
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
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
' $History: ec_10.vb $
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 14.01.10   Time: 12:44
' Updated in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 21.08.09   Time: 12:20
' Updated in $/CKAG/Applications/appec/Lib
' ITA: 2918
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 8.07.09    Time: 16:24
' Updated in $/CKAG/Applications/appec/Lib
' nachbesserung dyproxy umstellung
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 26.06.09   Time: 14:53
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918, Z_M_Ec_Avm_Zulauf, Z_V_FAHRZEUG_STATUS_001,
' Z_M_EC_AVM_STATUS_BESTAND, Z_M_ABMBEREIT_LAUFAEN,
' Z_M_ABMBEREIT_LAUFZEIT, Z_M_Brief_Temp_Vers_Mahn_001,
' Z_M_SCHLUE_SET_MAHNSP_001, Z_M_SCHLUESSELDIFFERENZEN,
' Z_M_SCHLUESSELVERLOREN, Z_M_SCHLUE_TEMP_VERS_MAHN_001,
' Z_M_Ec_Avm_Status_Zul,  Z_M_ECA_TAB_BESTAND
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 28.01.09   Time: 11:30
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2431 nachbesserung
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 4.12.08    Time: 8:38
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2431 testfertig
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 10  *****************
' User: Uha          Date: 2.07.07    Time: 17:23
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 9  *****************
' User: Uha          Date: 3.05.07    Time: 18:42
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
