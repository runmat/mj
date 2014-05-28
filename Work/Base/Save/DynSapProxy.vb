Option Explicit Off
Option Strict Off

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
'Imports Microsoft.Data.SAPClient
Imports System.Configuration
Imports ERPConnect

Namespace Common

    Public Class DynSapProxy
        Inherits DynSapProxyObj
        Private Shared Proxys As New Generic.List(Of DynSapProxyObj)
        Private mObjMyApplication As CKG.Base.Kernel.Security.App

#Region "Methods"

        Private Shared ReadOnly Property ErpConnection() As R3Connection

            Get
                Dim con As R3Connection

                If ConfigurationManager.AppSettings("Connectionstring").Contains("192.168.10.09") OrElse ConfigurationManager.AppSettings("Connectionstring").Contains("192.168.10.9") Then
                    con = New R3Connection(ConfigurationManager.AppSettings("SAPAppServerHost"), _
                             CShort(ConfigurationManager.AppSettings("SAPSystemNumber")), _
                             ConfigurationManager.AppSettings("SAPUsername"), _
                             ConfigurationManager.AppSettings("SAPPassword"), _
                            "DE", CShort(ConfigurationManager.AppSettings("SAPClient")))
                ElseIf ConfigurationManager.AppSettings("Connectionstring").Contains("AHS009") OrElse ConfigurationManager.AppSettings("Connectionstring").Contains("HHSRVW2KBCK019") OrElse ConfigurationManager.AppSettings("Connectionstring").Contains("CKGPortalTest") OrElse ConfigurationManager.AppSettings("Connectionstring").Contains("192.168.11.21") Then
                    con = New R3Connection(ConfigurationManager.AppSettings("TESTSAPAppServerHost"), _
                             CShort(ConfigurationManager.AppSettings("TESTSAPSystemNumber")), _
                             ConfigurationManager.AppSettings("TESTSAPUsername"), _
                             ConfigurationManager.AppSettings("TESTSAPPassword"), _
                            "DE", CShort(ConfigurationManager.AppSettings("TESTSAPClient")))
                Else
                    Throw New Exception("DynProxy kann SQL-ConnectionString nicht erkennen")

                End If

                Return con
            End Get




        End Property


        Private Shared ReadOnly Property SAPConnectionString() As String
            Get
                Dim conStr As String = ""
                If ConfigurationManager.AppSettings("Connectionstring").Contains("192.168.10.09") OrElse ConfigurationManager.AppSettings("Connectionstring").Contains("192.168.10.9") Then
                    'prod
                    conStr = "ASHOST=" & ConfigurationManager.AppSettings("SAPAppServerHost") & _
                                            ";CLIENT=" & CShort(ConfigurationManager.AppSettings("SAPClient")) & _
                                            ";SYSNR=" & CShort(ConfigurationManager.AppSettings("SAPSystemNumber")) & _
                                            ";USER=" & ConfigurationManager.AppSettings("SAPUsername") & _
                                            ";PASSWD=" & ConfigurationManager.AppSettings("SAPPassword") & _
                                            ";LANG=DE"
                ElseIf ConfigurationManager.AppSettings("Connectionstring").Contains("AHS009") OrElse ConfigurationManager.AppSettings("Connectionstring").Contains("HHSRVW2KBCK019") OrElse ConfigurationManager.AppSettings("Connectionstring").Contains("CKGPortalTest") OrElse ConfigurationManager.AppSettings("Connectionstring").Contains("192.168.11.21") Then
                    'vm,test,entwicklung
                    conStr = "ASHOST=" & ConfigurationManager.AppSettings("TESTSAPAppServerHost") & _
                                        ";CLIENT=" & CShort(ConfigurationManager.AppSettings("TESTSAPClient")) & _
                                        ";SYSNR=" & CShort(ConfigurationManager.AppSettings("TESTSAPSystemNumber")) & _
                                        ";USER=" & ConfigurationManager.AppSettings("TESTSAPUsername") & _
                                        ";PASSWD=" & ConfigurationManager.AppSettings("TESTSAPPassword") & _
                                        ";LANG=DE"
                Else
                    Throw New Exception("DynProxy kann SQL-ConnectionString nicht erkennen")
                End If
                Return conStr
            End Get
        End Property



        Public Shared Function getProxy(ByVal bapiName As String, ByRef mObjApp As CKG.Base.Kernel.Security.App, ByRef mObjUser As CKG.Base.Kernel.Security.User, ByRef mObjPage As Web.UI.Page) As DynSapProxyObj
            '----------------------------------------------------------------------
            'Methode:       getProxy
            'Autor:         Julian Jung
            'Beschreibung:  erstellt ein neues DynProxyObj-Objekt aus der Vorlage im Proxyarray
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------

            If Not bapiName.Trim(" "c) = "" AndAlso Not mObjPage Is Nothing AndAlso Not mObjApp Is Nothing AndAlso Not mObjUser Is Nothing Then
                If neuererBapiVorhanden(bapiName.ToUpper) Then
                    Dim tmpProxyObj As DynSapProxyObj = generateNewProxy(bapiName.ToUpper)
                    If Not tmpProxyObj Is Nothing Then
                        writeStrukturIntoSQLDB(tmpProxyObj.Import, tmpProxyObj.Export, tmpProxyObj.BapiDate, tmpProxyObj.BapiName)
                        Proxys.Add(tmpProxyObj)
                    End If
                End If
                Dim tmpObj As DynSapProxyObj
                tmpObj = retrieveObj(bapiName.ToUpper)
                tmpObj.settingCalledSource(mObjApp, mObjUser, mObjPage)
                Return tmpObj
            Else
                Throw New Exception("Fehlende Parameter zum Abfragen eines ProxyObj")
            End If
        End Function


        Protected Function GetProxys() As DataTable
            Dim proxysToShow As New DataTable
            With proxysToShow.Columns
                .Add("BapiName", String.Empty.GetType)
                .Add("BapiDate", Type.GetType("System.DateTime"))
                .Add("BapiLoadet", Type.GetType("System.DateTime"))
            End With

            For Each tmpProxy As DynSapProxyObj In Proxys
                Dim tmpRow As DataRow = proxysToShow.NewRow
                tmpRow.Item("BapiName") = tmpProxy.BapiName
                tmpRow.Item("BapiDate") = tmpProxy.BapiDate
                tmpRow.Item("BapiLoadet") = tmpProxy.BapiLoadet
                proxysToShow.Rows.Add(tmpRow)
            Next
            proxysToShow.AcceptChanges()
            Return proxysToShow
        End Function


        Protected Function getUsedMemory() As Double
            Dim returnValue As Long = GC.GetTotalMemory(True)
            Proxys.Clear()
            Return (returnValue - GC.GetTotalMemory(True)) / (1024 * 1024)
        End Function


        Private Shared Function neuererBapiVorhanden(ByVal BapiName As String) As Boolean
            '----------------------------------------------------------------------
            'Methode:       neuererBapiVorhanden
            'Autor:         Julian Jung
            'Beschreibung:  prüft im Proxyarray auf das angefragte Bapi auch in anbetracht des BAPI-Datums
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------

            Dim existInRam As Boolean = False
            If Not checkUpdateDB(BapiName) Then
                For Each DynSapProxyObj As DynSapProxyObj In Proxys
                    If DynSapProxyObj.BapiName = BapiName Then
                        If checkBapiInDB(BapiName, DynSapProxyObj.BapiDate) Then
                            Return False
                        Else
                            Throw New Exception("BAPI: " & BapiName & " Bapi Datum stimmt im RAM und in der SQL-DB nicht überein!")
                        End If
                        Exit For
                    End If
                Next
                If checkBapiInDB(BapiName) Then
                    Proxys.Add(getBapiStrukturFromSQLDB(BapiName))
                    Return False
                Else
                    Return True
                End If
            Else
                For Each DynSapProxyObj As DynSapProxyObj In Proxys
                    If DynSapProxyObj.BapiName = BapiName Then
                        Proxys.Remove(DynSapProxyObj)
                        Exit For
                    End If
                Next
                If removeBapiFromUpdateDB(BapiName) Then
                    Return True
                Else
                    Throw New Exception("BAPI: " & BapiName & " wurde nicht aus BAPIUPDATE Tabelle gelöscht")
                End If
            End If
        End Function


        Protected Sub insertBapiIntoUpdateDB(ByVal BapiName As String, ByVal webUserName As String)
            '----------------------------------------------------------------------
            'Methode:       insertBapiIntoUpdateDB
            'Autor:         Julian Jung
            'Beschreibung:  schreibt einen Eintrag für den übergebnen BapiNamen in die UpdateDB (manueller Eintrag)
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------

            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand
            Try
                cn.Open()
                cmd.Connection = cn
                cmd.CommandType = CommandType.Text
                Dim SqlQuery As String

                SqlQuery = "INSERT INTO [BapiUpdate] (BapiName, BapiLastChangeSap, LastSapDeveloper, LastDBChangeWebBy) VALUES (@BapiName, @BapiLastChangeSap, @LastSapDeveloper, @LastDBChangeWebBy);"
                With cmd
                    .Parameters.AddWithValue("@BapiName", BapiName.ToUpper)
                    .Parameters.AddWithValue("@BapiLastChangeSap", Today.ToShortDateString)
                    .Parameters.AddWithValue("@LastSapDeveloper", "web-manuell")
                    .Parameters.AddWithValue("@LastDBChangeWebBy", webUserName)
                End With
                cmd.CommandText = SqlQuery
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("SCHREIBEN EINES EINTRAGES IN DIE DB: TABELLENNAME=  BAPIUPDATE  \ " & ex.Message & " \ " & ex.StackTrace)
            Finally
                cn.Close()
            End Try
        End Sub

        Protected Function writeUpdatedBapisIntoUpdateDB(ByRef UpdatedBapis As DataTable, ByVal webUserName As String) As DataTable
            '----------------------------------------------------------------------
            'Methode:       writeUpdatedBapisIntoUpdateDB
            'Autor:         Julian Jung
            'Beschreibung:  schreibt für alle BapiNamen in der DT einen BapiEintrag in die UpdateDB und aktullisiert "webRun"-Eintrag (automatisches Update)
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------

            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand
            Dim BapiDate As Date
            BapiDate = Nothing
            Try
                UpdatedBapis.Columns.Add("RESULT", GetType(String))
                UpdatedBapis.AcceptChanges()
                cn.Open()
                cmd.Connection = cn
                cmd.CommandType = CommandType.Text
                Dim SqlQuery As String
                For Each tmpRow As DataRow In UpdatedBapis.Rows
                    If Not checkUpdateDB(tmpRow("NAME").ToString) Then
                        SqlQuery = "INSERT INTO [BapiUpdate] (BapiName, BapiLastChangeSap, LastSapDeveloper, LastDBChangeWebBy) VALUES (@BapiName, @BapiLastChangeSap, @LastSapDeveloper, @LastDBChangeWebBy);"
                        With cmd
                            .Parameters.AddWithValue("@BapiName", tmpRow("NAME"))
                            .Parameters.AddWithValue("@BapiLastChangeSap", HelpProcedures.MakeDateStandard(tmpRow("UDAT").ToString))
                            .Parameters.AddWithValue("@LastSapDeveloper", tmpRow("UNAM"))
                            .Parameters.AddWithValue("@LastDBChangeWebBy", webUserName)
                        End With
                        tmpRow("RESULT") = "wurde der UpdateTabelle hinzugefügt."
                    Else
                        If Not checkUpdateDB(tmpRow("NAME").ToString, HelpProcedures.MakeDateStandard(tmpRow("UDAT").ToString)) Then

                            SqlQuery = "UPDATE BapiUpdate " & _
                       "SET BapiLastChangeSap=@BapiLastChangeSap," & _
                       "LastSapDeveloper=@LastSapDeveloper," & _
                       "LastDBChangeWebBy=@LastDBChangeWebBy, " & _
                       "updated=@updated " & _
                       "WHERE BapiName='" & tmpRow("NAME").ToString & "';"
                            With cmd
                                .Parameters.AddWithValue("@BapiLastChangeSap", HelpProcedures.MakeDateStandard(tmpRow("UDAT").ToString))
                                .Parameters.AddWithValue("@LastSapDeveloper", tmpRow("UNAM"))
                                .Parameters.AddWithValue("@LastDBChangeWebBy", webUserName)
                                .Parameters.AddWithValue("@updated", Now)
                            End With
                            tmpRow("RESULT") = "Bapieintrag wurde in der UpdateTabelle geupdated"
                        Else
                            SqlQuery = String.Empty
                            tmpRow("RESULT") = "Bapieintrag ist bereits in der UpdateTabelle enthalten."
                        End If
                    End If
                    If Not SqlQuery Is String.Empty Then
                        cmd.CommandText = SqlQuery
                        cmd.ExecuteNonQuery()
                        cmd.Parameters.Clear()
                        SqlQuery = String.Empty
                    End If
                Next
                SqlQuery = "UPDATE BapiUpdate " & _
                "SET BapiLastChangeSap=@BapiLastChangeSap," & _
                "LastSapDeveloper=@LastSapDeveloper," & _
                "LastDBChangeWebBy=@LastDBChangeWebBy, " & _
                "updated=@updated " & _
                "WHERE BapiName='WebUpdateRun';"
                With cmd
                    .Parameters.AddWithValue("@BapiLastChangeSap", Now)
                    .Parameters.AddWithValue("@LastSapDeveloper", "WebUpdateRun")
                    .Parameters.AddWithValue("@LastDBChangeWebBy", webUserName)
                    .Parameters.AddWithValue("@updated", Now)
                End With
                cmd.CommandText = SqlQuery
                cmd.ExecuteNonQuery()
                UpdatedBapis.AcceptChanges()
                Return UpdatedBapis
            Catch ex As Exception
                Throw New Exception("FEHLER BEI DEM SCHREIBEN/UPDATE EINES EINTRAGES IN  DIE SQL DB: TABELLENNAME=  BAPIUPDATE  \ " & ex.Message & " \ " & ex.StackTrace)
            Finally
                cn.Close()
            End Try
        End Function


        Protected Function callUpdatedBapiBapi(ByVal ChangesFrom As System.DateTime) As DataTable
            '----------------------------------------------------------------------
            'Methode:       callUpdatedBapiBapi
            'Autor:         Julian Jung
            'Beschreibung:  ruft das BAPI auf,dass alle BAPIS zurückliefert die Änderungen seit dem Zeitpunkt X enthalten
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------

            If Not ChangesFrom = Nothing Then

                Dim con As R3Connection = ErpConnection()

                ERPConnect.LIC.SetLic(ConfigurationManager.AppSettings("ErpConnectLicense"))

                Try
                    con.Open(False)



                    Dim func As ERPConnect.RFCFunction = con.CreateFunction("Z_S_GET_BAPI_INTERFACE_CHANGED")

                    'Dim cmd As New SAPCommand()
                    'cmd.Connection = con
                    'Dim strCom As String

                    'strCom = "EXEC Z_S_GET_BAPI_INTERFACE_CHANGED @I_ABDAT=@pI_ABDAT,"
                    'strCom = strCom & "@GT_BAPIS=@pE_GT_BAPIS OUTPUT OPTION 'disabledatavalidation'"
                    'cmd.CommandText = strCom

                    'importparameter
                    'Dim pI_ABDAT As New SAPParameter("@pI_ABDAT", ParameterDirection.Input)

                    func.Exports("I_ABDAT").ParamValue = Year(ChangesFrom) & Right("0" & Month(ChangesFrom), 2) & Right("0" & Day(ChangesFrom), 2)

                    func.Execute()

                    'exportParameter
                    'Dim pE_GT_BAPIS As New SAPParameter("@pE_GT_BAPIS", ParameterDirection.Output)
                    Dim pE_GT_BAPIS As RFCParameter


                    pE_GT_BAPIS = func.Imports("GT_BAPIS")


                    'Importparameter hinzufügen
                    'cmd.Parameters.Add(pI_ABDAT)

                    'exportparameter hinzugfügen
                    'cmd.Parameters.Add(pE_GT_BAPIS)

                    'befüllen der Importparameter
                    'pI_ABDAT.Value = Year(ChangesFrom) & Right("0" & Month(ChangesFrom), 2) & Right("0" & Day(ChangesFrom), 2)




                    'cmd.ExecuteNonQuery()

                    If Not pE_GT_BAPIS.ParamValue Is DBNull.Value Then
                        Dim tmpDt As DataTable
                        tmpDt = DirectCast(pE_GT_BAPIS.ParamValue, DataTable)
                        HelpProcedures.killAllDBNullValuesInDataTable(tmpDt)
                        Return tmpDt
                    Else
                        Return Nothing
                    End If
                Catch ex As Exception
                    Throw New Exception("FEHLER BEIM AUFRUF DES BAPIS FÜR DAS UPDATE DER BAPISTRUKTUREN: BAPINAME= Z_S_GET_BAPI_INTERFACE_CHANGED  \ " & ex.Message & " \ " & ex.StackTrace)
                    Return Nothing
                Finally
                    con.Close()
                    con.Dispose()
                End Try
            Else
                Return Nothing
            End If
        End Function


        Protected Function getDirektSapStruktur(ByVal BapiName As String) As DynSapProxyObj
            getDirektSapStruktur = generateNewProxy(BapiName)
        End Function


        Private Shared Function checkUpdateDB(ByVal bapiName As String, Optional ByVal sapBapiDate As Date = Nothing) As Boolean
            '----------------------------------------------------------------------
            'Methode:       checkUpdateDB
            'Autor:         Julian Jung
            'Beschreibung:  prüft ob sich ein bestimmtes BAPI in der UpdateDB befindet, also geändert wurde
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------

            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand
            Dim sqlQuery As String
            Dim obj As Object
            Try
                cmd.Connection = cn
                cn.Open()

                If sapBapiDate = Nothing Then
                    sqlQuery = "Select * FROM BapiUpdate WHERE BapiName=@BapiName;"
                    cmd.Parameters.AddWithValue("@BapiName", bapiName)
                Else
                    sqlQuery = "Select * FROM BapiUpdate WHERE BapiName='" & bapiName & "' AND BapiLastChangeSap=@BapiLastChangeSap;"
                    cmd.Parameters.AddWithValue("@BapiLastChangeSap", sapBapiDate)
                End If
                cmd.CommandText = sqlQuery
                obj = cmd.ExecuteScalar()
                If obj Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                Throw New Exception("FEHLER BEI BAPIUPDATETABELLE EXISTENZABFRAGE: BAPINAME= " & bapiName & " \ " & ex.Message & " \ " & ex.StackTrace)
            Finally
                cn.Close()
            End Try
        End Function

        Private Shared Function removeBapiFromUpdateDB(ByVal bapiName As String) As Boolean
            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand
            Dim sqlQuery As String
            Dim tmpInt As Integer = 0
            Try
                cmd.Connection = cn
                cn.Open()

                sqlQuery = "Delete FROM BapiUpdate WHERE BapiName=@BapiName;"
                cmd.Parameters.AddWithValue("@BapiName", bapiName)


                cmd.CommandText = sqlQuery
                tmpInt = cmd.ExecuteNonQuery

                If Not tmpInt = 1 Then
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                Throw New Exception("FEHLER BEI LÖSCHEN EINES EINTRAGES AUS DER BAPIUPDATE TABELLE: BAPINAME= " & bapiName & " \ " & ex.Message & " \ " & ex.StackTrace)
            Finally
                cn.Close()
            End Try
        End Function


        Protected Function removeBapiFrom(ByVal bapiName As String, ByVal from As String) As Boolean
            '----------------------------------------------------------------------
            'Methode:       removeBapiFrom
            'Autor:         Julian Jung
            'Beschreibung:  entfernt ein BAPI aus RAM/DB/UpdateDB je nach Parameter
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------

            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand
            Dim sqlQuery As String
            Dim tmpInt As Integer = 0
            Dim status As Boolean = False
            cmd.Connection = cn
            cn.Open()
            Try
                Select Case from
                    Case "updateDB"
                        sqlQuery = "Delete FROM BapiUpdate WHERE BapiName=@BapiName;"
                        cmd.Parameters.AddWithValue("@BapiName", bapiName)

                        cmd.CommandText = sqlQuery
                        tmpInt = cmd.ExecuteNonQuery

                        If Not tmpInt = 1 Then
                            status = False
                        Else
                            status = True
                        End If
                    Case "DB"
                        For Each xDynProx As DynSapProxyObj In Proxys
                            If xDynProx.BapiName = bapiName Then
                                Proxys.Remove(xDynProx)
                                Exit For
                            End If
                        Next

                        sqlQuery = "Delete FROM BapiStruktur WHERE BapiName=@BapiName;"
                        cmd.Parameters.AddWithValue("@BapiName", bapiName)

                        cmd.CommandText = sqlQuery
                        tmpInt = cmd.ExecuteNonQuery

                        If Not tmpInt = 1 Then
                            status = False
                        Else
                            status = True
                        End If
                    Case "RAM"
                        For Each xDynProx As DynSapProxyObj In Proxys
                            If xDynProx.BapiName = bapiName Then
                                Proxys.Remove(xDynProx)
                                status = True
                                Exit For
                            End If
                        Next
                End Select
                Return status
            Catch ex As Exception
                Throw New Exception("Fehler beim löschen von: BAPINAME= " & bapiName & " \ " & ex.Message & " \ " & ex.StackTrace)
            Finally
                cn.Close()
            End Try
        End Function


        Private Shared Function checkBapiInDB(ByVal bapiName As String, Optional ByVal bapiDate As Date = Nothing) As Boolean
            '----------------------------------------------------------------------
            'Methode:       checkBapiInDB
            'Autor:         Julian Jung
            'Beschreibung:  prüft ein BAPI in der DB auf seine Aktuallität u. vorhanden sein
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------

            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand
            Dim sqlQuery As String
            Dim obj As Object
            Try
                cmd.Connection = cn
                cn.Open()
                If bapiDate = Nothing Then
                    sqlQuery = "Select * FROM BapiStruktur WHERE BapiName=@BapiName;"
                    cmd.Parameters.AddWithValue("@BapiName", bapiName)
                Else
                    sqlQuery = "Select * FROM BapiStruktur WHERE BapiName='" & bapiName & "' AND BapiDate=@BapiDate;"
                    cmd.Parameters.AddWithValue("@BapiDate", bapiDate)
                End If
                cmd.CommandText = sqlQuery
                obj = cmd.ExecuteScalar()
                If obj Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                Throw New Exception("FEHLER BEI PROXY EXISTENZABFRAGE: BAPINAME= " & bapiName & " \ " & ex.Message & " \ " & ex.StackTrace)
            Finally
                cn.Close()
            End Try
        End Function


        Private Shared Function retrieveObj(ByVal BapiName As String) As DynSapProxyObj
            '----------------------------------------------------------------------
            'Methode:       checkBapiInDB
            'Autor:         Julian Jung
            'Beschreibung:  erstellt ein neues BAPI aus dem BapiArray
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------

            For Each xDynProx As DynSapProxyObj In Proxys
                If xDynProx.BapiName = BapiName Then
                    Return New DynSapProxyObj(xDynProx.BapiName, xDynProx.BapiDate, xDynProx.Import, xDynProx.Export)
                End If
            Next
            Return Nothing
        End Function


        Private Shared Function CallbapiForBapi(ByRef mExportTabelle As DataTable, ByRef mImportTabelle As DataTable, ByVal bapiName As String) As Date
            '----------------------------------------------------------------------
            'Methode:       CallbapiForBapi
            'Autor:         Julian Jung
            'Beschreibung:  ruft das BAPI für die BAPI-Strukturen auf und füllt die Import/Export Tabellen
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------


            Dim con As R3Connection = ErpConnection()

            ERPConnect.LIC.SetLic(ConfigurationManager.AppSettings("ErpConnectLicense"))

            Try

                con.Open(False)



                Dim func As ERPConnect.RFCFunction = con.CreateFunction("Z_S_GET_BAPI_INTERFACE")


                'Dim cmd As New SAPCommand()
                'cmd.Connection = con
                'Dim strCom As String
                'strCom = "EXEC Z_S_GET_BAPI_INTERFACE @I_FUNCNAME=@pI_FUNCNAME,@IMP_TAB=@pE_IMP_TAB OUTPUT,@E_UDATE=@pE_UDATE OUTPUT,@E_FUNCNAME=@pE_FUNCNAME OUTPUT,"
                'strCom = strCom & "@EXP_TAB=@pE_EXP_TAB OUTPUT OPTION 'disabledatavalidation'"

                'cmd.CommandText = strCom

                'importparameter
                'Dim pI_FUNCNAME As New SAPParameter("@pI_FUNCNAME", ParameterDirection.Input)

                func.Exports("I_FUNCNAME").ParamValue = bapiName.Trim(" "c).ToUpper

                'exportParameter
                'Dim pE_EXP_TAB As New SAPParameter("@pE_EXP_TAB", ParameterDirection.Output)
                'Dim pE_IMP_TAB As New SAPParameter("@pE_IMP_TAB", ParameterDirection.Output)
                'Dim pE_FUNCNAME As New SAPParameter("@pE_FUNCNAME", ParameterDirection.Output)
                'Dim pE_UDATE As New SAPParameter("@pE_UDATE", ParameterDirection.Output)

                func.Execute()

                'Dim pE_EXP_TAB As RFCParameter
                'Dim pE_IMP_TAB As RFCParameter
                Dim pE_FUNCNAME As RFCParameter
                Dim pE_UDATE As RFCParameter

                'pE_EXP_TAB = func.Imports("EXP_TAB")
                'pE_IMP_TAB = func.Imports("IMP_TAB")
                pE_FUNCNAME = func.Imports("E_FUNCNAME")
                pE_UDATE = func.Imports("E_UDATE")



                'Importparameter hinzufügen
                'cmd.Parameters.Add(pI_FUNCNAME)

                'exportparameter hinzugfügen
                'cmd.Parameters.Add(pE_EXP_TAB)
                'cmd.Parameters.Add(pE_IMP_TAB)
                'cmd.Parameters.Add(pE_FUNCNAME)
                'cmd.Parameters.Add(pE_UDATE)

                'befüllen der Importparameter
                'pI_FUNCNAME.Value = bapiName.Trim(" "c).ToUpper

                'cmd.ExecuteNonQuery()

                If pE_FUNCNAME.ParamValue Is DBNull.Value OrElse Not pE_FUNCNAME.ParamValue.ToString.Trim(" "c) = bapiName.Trim(" "c).ToUpper Then
                    Throw New Exception("angefragtes Bapi:" & bapiName.Trim(" "c) & " <> struktur von: " & pE_FUNCNAME.ParamValue.ToString)
                End If

                'auswerten der exportparameter
                If func.Tables("EXP_TAB").RowCount > 0 Then
                    mExportTabelle = func.Tables("EXP_TAB").ToADOTable
                    HelpProcedures.killAllDBNullValuesInDataTable(mExportTabelle)
                    mExportTabelle.AcceptChanges()
                End If

                If func.Tables("IMP_TAB").RowCount > 0 Then
                    mImportTabelle = func.Tables("IMP_TAB").ToADOTable
                    HelpProcedures.killAllDBNullValuesInDataTable(mImportTabelle)
                    mImportTabelle.AcceptChanges()
                End If

                If pE_UDATE.ParamValue Is DBNull.Value Then
                    Throw New Exception("angefragtes Bapi:" & bapiName.Trim(" "c) & " enhält kein Änderungsdatum")
                Else
                    Dim strTemp As String = Right(pE_UDATE.ParamValue.ToString, 2) & "." & Mid(pE_UDATE.ParamValue.ToString, 5, 2) & "." & Left(pE_UDATE.ParamValue.ToString, 4)

                    If Not IsDate(strTemp) Then
                        Throw New Exception("angefragtes Bapi:" & bapiName.Trim(" "c) & " enhält kein Änderungsdatum")
                    Else
                        Return CDate(strTemp)
                    End If
                End If
            Catch ex As Exception
                Throw New Exception("FEHLER BEIM AUFRUF DES BAPIS FÜR DIE BAPISTRUKTUR: BAPINAME= " & bapiName & " \ " & ex.Message & " \ " & ex.StackTrace)
                Return Nothing
            Finally
                con.Close()
            End Try
        End Function


        Private Shared Function generateNewProxy(ByVal bapiName As String) As DynSapProxyObj
            '----------------------------------------------------------------------
            'Methode:       generateNewProxy
            'Autor:         Julian Jung
            'Beschreibung:  generiert ein neues Proxyobjekt aus der Import/Export Tabelle und gibt es zurück
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------

            Dim ExportStruktur As New DataTable
            Dim ImportStruktur As New DataTable
            Dim impTable As New DataTable
            Dim expTable As New DataTable
            Dim BapiSapDate As Date
            BapiSapDate = CallbapiForBapi(expTable, impTable, bapiName)
            Try
                'importStruktur Definieren 
                ImportStruktur.Columns.Add(New DataColumn("Element", GetType(DataTable)))
                ImportStruktur.Columns.Add(New DataColumn("ElementCode", System.Type.GetType("System.String")))
                ImportStruktur.AcceptChanges()

                'exportStruktur Definieren 
                ExportStruktur.Columns.Add(New DataColumn("Element", GetType(DataTable)))
                ExportStruktur.Columns.Add(New DataColumn("ElementCode", System.Type.GetType("System.String")))
                ExportStruktur.AcceptChanges()

                'import
                Dim parameterTabelle As New DataTable("ParameterTabelle")
                parameterTabelle.Columns.Add(New DataColumn("PARAMETER", System.Type.GetType("System.String")))
                parameterTabelle.Columns.Add(New DataColumn("ParameterDATATYPE", System.Type.GetType("System.String")))
                parameterTabelle.Columns.Add(New DataColumn("ParameterValue"))
                parameterTabelle.Columns.Add(New DataColumn("ParameterLength", System.Type.GetType("System.Int32")))
                parameterTabelle.AcceptChanges()

                Dim sapConformCount As Int32 = 0
                Dim found As Boolean = False
                Dim sapTabellen(0) As DataTable
                Dim sapConformImportTable As New DataTable
                Dim sapConformImportTabellen(0) As DataTable

                For Each tmprowX As DataRow In impTable.Rows
                    If Not tmprowX("STRUCTURE") Is DBNull.Value AndAlso Not tmprowX("STRUCTURE").ToString.Trim(" "c) = "" Then
                        found = False
                        sapConformCount = 0
                        For Each tmpDatatable As DataTable In sapTabellen
                            sapConformCount += 1
                            If Not tmpDatatable Is Nothing Then
                                If tmpDatatable.TableName = tmprowX("STRUCTURE").ToString Then
                                    found = True

                                    sapConformImportTable = sapConformImportTabellen(sapConformCount - 1)

                                    If tmprowX("DATATYPE").ToString = "STRING" Then
                                        tmpDatatable.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, System.Type.GetType("System.String")))
                                        tmpDatatable.Columns(tmpDatatable.Columns.Count - 1).MaxLength = CInt(tmprowX("LENGTH"))
                                    ElseIf tmprowX("DATATYPE").ToString = "DATE" Then
                                        tmpDatatable.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, System.Type.GetType("System.DateTime")))
                                    End If

                                    sapConformImportTable.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, System.Type.GetType("System.String")))
                                    sapConformImportTable.Columns(sapConformImportTable.Columns.Count - 1).MaxLength = CInt(tmprowX("LENGTH"))
                                    tmpDatatable.AcceptChanges()
                                    sapConformImportTable.AcceptChanges()
                                    Exit For
                                End If
                            End If
                        Next
                        If found = False Then
                            Dim SapTabelle As New DataTable(tmprowX("STRUCTURE").ToString)
                            sapTabellen(sapTabellen.Length - 1) = SapTabelle
                            ReDim Preserve sapTabellen(sapTabellen.Length)
                            sapConformImportTable = New DataTable(tmprowX("STRUCTURE").ToString)
                            sapConformImportTabellen(sapConformImportTabellen.Length - 1) = sapConformImportTable
                            ReDim Preserve sapConformImportTabellen(sapConformImportTabellen.Length)

                            If tmprowX("DATATYPE").ToString = "STRING" Then
                                SapTabelle.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, System.Type.GetType("System.String")))
                                SapTabelle.Columns(SapTabelle.Columns.Count - 1).MaxLength = CInt(tmprowX("LENGTH"))
                            ElseIf tmprowX("DATATYPE").ToString = "DATE" Then
                                SapTabelle.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, System.Type.GetType("System.DateTime")))
                            End If

                            sapConformImportTable.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, System.Type.GetType("System.String")))
                            sapConformImportTable.Columns(sapConformImportTable.Columns.Count - 1).MaxLength = CInt(tmprowX("LENGTH"))
                            SapTabelle.AcceptChanges()
                            sapConformImportTable.AcceptChanges()
                        End If
                    Else
                        Dim rowX As DataRow = parameterTabelle.NewRow
                        rowX("PARAMETER") = tmprowX("PARAMETER").ToString
                        rowX("ParameterDATATYPE") = tmprowX("DATATYPE").ToString
                        rowX("ParameterValue") = Nothing
                        rowX("ParameterLength") = CInt(tmprowX("LENGTH"))
                        parameterTabelle.Rows.Add(rowX)
                        parameterTabelle.AcceptChanges()
                    End If
                Next

                Dim strukturRow As DataRow
                strukturRow = ImportStruktur.NewRow
                strukturRow("Element") = parameterTabelle
                strukturRow("ElementCode") = "PARA"
                ImportStruktur.Rows.Add(strukturRow)
                ImportStruktur.AcceptChanges()
                strukturRow = Nothing
                For Each DataTable As DataTable In sapTabellen
                    If Not DataTable Is Nothing Then
                        strukturRow = ImportStruktur.NewRow
                        strukturRow("Element") = DataTable
                        strukturRow("ElementCode") = "TABLE"
                        ImportStruktur.Rows.Add(strukturRow)
                        ImportStruktur.AcceptChanges()
                        strukturRow = Nothing
                    End If
                Next
                For Each DataTable As DataTable In sapConformImportTabellen
                    If Not DataTable Is Nothing Then
                        strukturRow = ImportStruktur.NewRow
                        strukturRow("Element") = DataTable
                        strukturRow("ElementCode") = "SAPTABLE"
                        ImportStruktur.Rows.Add(strukturRow)
                        ImportStruktur.AcceptChanges()
                        strukturRow = Nothing
                    End If
                Next

                'export
                Dim parameterTabelle2 As New DataTable("parameterTabelle")

                parameterTabelle2.Columns.Add(New DataColumn("PARAMETER", System.Type.GetType("System.String")))
                parameterTabelle2.Columns.Add(New DataColumn("ParameterDATATYPE", System.Type.GetType("System.String")))
                parameterTabelle2.Columns.Add(New DataColumn("ParameterValue"))
                parameterTabelle2.Columns.Add(New DataColumn("ParameterLength", System.Type.GetType("System.Int32")))
                parameterTabelle2.AcceptChanges()

                Dim sapTabellen2(0) As DataTable
                Dim found2 As Boolean = False

                For Each tmprowX As DataRow In expTable.Rows
                    If Not tmprowX("STRUCTURE") Is DBNull.Value AndAlso Not tmprowX("STRUCTURE").ToString.Trim(" "c) = "" Then
                        found2 = False
                        For Each tmpDatatable As DataTable In sapTabellen2
                            If Not tmpDatatable Is Nothing Then
                                If tmpDatatable.TableName = tmprowX("STRUCTURE").ToString Then
                                    found2 = True

                                    If tmprowX("DATATYPE").ToString = "STRING" Then
                                        tmpDatatable.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, System.Type.GetType("System.String")))
                                        tmpDatatable.Columns(tmpDatatable.Columns.Count - 1).MaxLength = CInt(tmprowX("LENGTH"))
                                    ElseIf tmprowX("DATATYPE").ToString = "DATE" Then
                                        tmpDatatable.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, System.Type.GetType("System.DateTime")))
                                    End If

                                    tmpDatatable.AcceptChanges()
                                    Exit For
                                End If
                            End If
                        Next
                        If found2 = False Then
                            Dim SapTabelle As New DataTable(tmprowX("STRUCTURE").ToString)
                            sapTabellen2(sapTabellen2.Length - 1) = SapTabelle
                            ReDim Preserve sapTabellen2(sapTabellen2.Length)

                            If tmprowX("DATATYPE").ToString = "STRING" Then
                                SapTabelle.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, System.Type.GetType("System.String")))
                                SapTabelle.Columns(SapTabelle.Columns.Count - 1).MaxLength = CInt(tmprowX("LENGTH"))
                            ElseIf tmprowX("DATATYPE").ToString = "DATE" Then
                                SapTabelle.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, System.Type.GetType("System.DateTime")))
                            End If

                            SapTabelle.AcceptChanges()
                        End If
                    Else
                        Dim rowX As DataRow = parameterTabelle2.NewRow
                        rowX("PARAMETER") = tmprowX("PARAMETER").ToString
                        rowX("ParameterDATATYPE") = tmprowX("DATATYPE").ToString
                        rowX("ParameterValue") = Nothing
                        rowX("ParameterLength") = CInt(tmprowX("LENGTH"))
                        parameterTabelle2.Rows.Add(rowX)
                        parameterTabelle2.AcceptChanges()
                    End If
                Next

                strukturRow = ExportStruktur.NewRow
                strukturRow("Element") = parameterTabelle2
                strukturRow("ElementCode") = "PARA"
                ExportStruktur.Rows.Add(strukturRow)
                ExportStruktur.AcceptChanges()
                strukturRow = Nothing
                For Each DataTable As DataTable In sapTabellen2
                    If Not DataTable Is Nothing Then
                        strukturRow = ExportStruktur.NewRow
                        strukturRow("Element") = DataTable
                        strukturRow("ElementCode") = "TABLE"
                        ExportStruktur.Rows.Add(strukturRow)
                        ExportStruktur.AcceptChanges()
                        strukturRow = Nothing
                    End If
                Next

                Return New DynSapProxyObj(bapiName, BapiSapDate, ImportStruktur, ExportStruktur)
            Catch ex As Exception
                Throw New Exception("Fehler bei der Erstellung des Proxys für BAPI: " & bapiName & " / " & ex.Message)
                Return Nothing
            End Try
        End Function


        Private Shared Function getBapiStrukturFromSQLDB(ByVal BapiName As String) As DynSapProxyObj
            '----------------------------------------------------------------------
            'Methode:       getBapiStrukturFromSQLDB
            'Autor:         Julian Jung
            'Beschreibung:  holt ein bestimmtes Proxy objekt aus der ProxySQLDB und gibt ihn zurück
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------

            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim myFormatter As New BinaryFormatter()
            Try
                cn.Open()
                Dim cmd As New SqlClient.SqlCommand("SELECT * FROM BapiStruktur WHERE BapiName ='" & BapiName & "'", cn)
                Dim myDataReader As SqlClient.SqlDataReader = cmd.ExecuteReader

                If Not myDataReader Is Nothing AndAlso myDataReader.Read Then
                    Return New DynSapProxyObj(BapiName, CDate(myDataReader.GetValue(1)), CType(myFormatter.Deserialize(New MemoryStream(DirectCast(myDataReader.GetValue(2), Byte()))), DataTable), DirectCast(myFormatter.Deserialize(New MemoryStream(DirectCast(myDataReader.GetValue(3), Byte()))), DataTable))
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Return Nothing
                Throw ex
            Finally
                cn.Close()
            End Try
        End Function


        Private Shared Function writeStrukturIntoSQLDB(ByVal ImportStruktur As DataTable, ByVal ExportStruktur As DataTable, ByVal bapiDate As Date, ByVal bapiname As String) As Boolean
            '----------------------------------------------------------------------
            'Methode:       getBapiStrukturFromSQLDB
            'Autor:         Julian Jung
            'Beschreibung:  schreibt die BAPI-Strukturen für ein Proxyobjekt in die ProxySQLDB 
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------

            Dim msI As New MemoryStream
            Dim msE As New MemoryStream
            Dim myFormatterImportS As New BinaryFormatter()
            Dim myFormatterExportS As New BinaryFormatter()
            Dim serializedI As Byte()
            Dim serializedE As Byte()
            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand

            myFormatterImportS.Serialize(msI, ImportStruktur)
            myFormatterExportS.Serialize(msE, ExportStruktur)
            msI.Close()
            msE.Close()
            serializedE = msE.ToArray
            serializedI = msI.ToArray

            Try
                cn.Open()
                cmd.Connection = cn
                cmd.CommandType = CommandType.Text
                Dim SqlQuery As String
                If Not checkBapiInDB(bapiname) Then
                    SqlQuery = "INSERT INTO [BapiStruktur] (BapiName, BapiDate, ImportStruktur, ExportStruktur) VALUES (@BapiName, @BapiDate, @ImportStruktur, @ExportStruktur);"
                    With cmd
                        .Parameters.AddWithValue("@BapiName", bapiname)
                        .Parameters.AddWithValue("@BapiDate", bapiDate)
                        .Parameters.AddWithValue("@ImportStruktur", serializedI)
                        .Parameters.AddWithValue("@ExportStruktur", serializedE)
                    End With
                Else
                    SqlQuery = "UPDATE BapiStruktur " & _
                     "SET ImportStruktur=@ImportStruktur," & _
                     "ExportStruktur=@ExportStruktur," & _
                     "BapiDate=@BapiDate, " & _
                     "updated=@updated " & _
                     "WHERE BapiName='" & bapiname & "';"
                    With cmd
                        .Parameters.AddWithValue("@ImportStruktur", serializedI)
                        .Parameters.AddWithValue("@ExportStruktur", serializedE)
                        .Parameters.AddWithValue("@BapiDate", bapiDate)
                        .Parameters.AddWithValue("@updated", Now)
                    End With
                End If
                cmd.CommandText = SqlQuery
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("FEHLER BEI DEM SCHREIBEN/UPDATE EINES PROXY OBJ IN DIE SQL DB: BAPINAME= " & bapiname & " \ " & ex.Message & " \ " & ex.StackTrace)
            Finally
                cn.Close()
            End Try
        End Function
#End Region
    End Class

End Namespace
' ************************************************
' $History: DynSapProxy.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 6.08.10    Time: 13:09
' Created in $/CKAG/Base/Save
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 19.05.10   Time: 11:46
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 12.05.10   Time: 18:21
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 28.04.09   Time: 16:36
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 10.03.09   Time: 8:07
' Updated in $/CKAG/Base/Kernel/Common
' sql connection string erweitern
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 12.01.09   Time: 11:51
' Updated in $/CKAG/Base/Kernel/Common
' DynProxy weitere webconfig mglichkeiten f�r SAP-System zugriff
' hinzugef�gt
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 16.12.08   Time: 10:20
' Updated in $/CKAG/Base/Kernel/Common
' DynProxy Anpassungen
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 15.12.08   Time: 18:00
' Updated in $/CKAG/Base/Kernel/Common
' DynProxy Anpassungen
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 15.12.08   Time: 17:37
' Created in $/CKAG/Base/Kernel/Common
' Dyn Proxy integriert
' 
' ************************************************