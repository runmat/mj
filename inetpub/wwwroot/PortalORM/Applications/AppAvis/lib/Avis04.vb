Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts


Public Class Avis04
 

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
        Fill(m_strAppID, m_strSessionID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        Dim tblTemp2 As DataTable
        Dim tblHerst As DataTable
        Dim intCounter As Integer

        m_strClassAndMethod = "Avis04.FILL"
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

                S.AP.InitExecute("Z_M_SCHLUESSELDIFFERENZEN", "I_KUNNR", m_objUser.KUNNR.ToSapKunnr())

                'strCom = "EXEC Z_M_SCHLUESSELDIFFERENZEN @I_KUNNR='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
                '        & "@GT_WEB_OUT_TUETE=@SAPTableDummy OUTPUT," & _
                '          "@GT_WEB_OUT_BRIEFE=@SAPTableBriefe OUTPUT," & _
                '          "@GT_WEB_OUT_HERST=@SAPTableHerst OUTPUT OPTION 'disabledatavalidation'"

                'cmd.Connection = con
                'cmd.CommandText = strCom

                ''Exportparameter
                'Dim SAPTableDummy As New SAPParameter("@SAPTableDummy", ParameterDirection.Output)
                'cmd.Parameters.Add(SAPTableDummy)

                'Dim SAPTableBriefe As New SAPParameter("@SAPTableBriefe", ParameterDirection.Output)
                'cmd.Parameters.Add(SAPTableBriefe)

                'Dim SAPTableHerst As New SAPParameter("@SAPTableHerst", ParameterDirection.Output)
                'cmd.Parameters.Add(SAPTableHerst)


                'cmd.ExecuteNonQuery()


                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                'tblTemp2 = New DataTable()
                'tblTemp2 = DirectCast(SAPTableBriefe.Value, DataTable)
                'tblHerst = DirectCast(SAPTableHerst.Value, DataTable)

                tblTemp2 = S.AP.GetExportTable("GT_WEB_OUT_BRIEFE")
                tblHerst = S.AP.GetExportTable("GT_WEB_OUT_HERST")

                Dim row As DataRow()

                tblTemp2.Columns.Add("Hersteller", GetType(System.String))  '§§§ JVE 27.09.2006: Hersteller.
                tblTemp2.Columns.Add("Delete", GetType(System.Boolean))     '§§§ JVE 16.10.2006: Löschen.
                tblTemp2.Columns.Add("Status", GetType(System.String))     '§§§ JVE 16.10.2006: Status.

                For intCounter = tblTemp2.Rows.Count - 1 To 0 Step -1


                    tblTemp2.Rows(intCounter)("Delete") = False             '§§§ JVE 16.10.2006: Löschen.
                    tblTemp2.Rows(intCounter)("Status") = String.Empty              '§§§ JVE 16.10.2006: Status.

                    '§§§ JVE 27.09.2006: Hersteller einfügen...
                    row = tblHerst.Select("CHASSIS_NUM='" & CStr(tblTemp2.Rows(intCounter)("CHASSIS_NUM")) & "'")
                    If Not (row.Length = 0) Then
                        tblTemp2.Rows(intCounter)("Hersteller") = CStr(row(0)("HERST_T"))
                    Else
                        tblTemp2.Rows(intCounter)("Hersteller") = String.Empty
                    End If
                Next

                tblTemp2.AcceptChanges()

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                'con.Close()
                'con.Dispose()

                'cmd.Dispose()
                m_blnGestartet = False
            End Try
        End If
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
                Dim strKUNNR As String = m_objUser.KUNNR.ToSapKunnr()

                Dim SAPTable As DataTable = S.AP.GetImportTableWithInit("Z_M_Schluesselverloren", "I_KUNNR", strKUNNR)

                'Dim strKUNNR As String = Right("0000000000" & m_objUser.KUNNR, 10)

                'Dim SAPTable As New DataTable


                'With SAPTable
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
' $History: Avis04.vb $
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 14.01.10   Time: 12:45
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 28.04.09   Time: 17:09
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 28.01.09   Time: 11:24
' Updated in $/CKAG/Applications/AppAvis/lib
' ita 2431 nachbesserung
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 9.12.08    Time: 11:26
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2419 21 Tage-WebFilter entfernt
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 3.12.08    Time: 10:46
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2419 testfertig
'
'
' ************************************************