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
Imports System.Configuration
Imports ERPConnect

Namespace Common

    Public Class DynSapProxy
        Inherits DynSapProxyObj

#Region "Methods"

        Private Shared ReadOnly Property ErpConnection() As R3Connection

            Get
                Dim con As R3Connection

                If ConfigurationManager.AppSettings("ProdSAP").ToString.ToUpper = "TRUE" Then
                    con = New R3Connection(ConfigurationManager.AppSettings("SAPAppServerHost"), _
                              CShort(ConfigurationManager.AppSettings("SAPSystemNumber")), _
                              ConfigurationManager.AppSettings("SAPUsername"), _
                              ConfigurationManager.AppSettings("SAPPassword"), _
                             "DE", CShort(ConfigurationManager.AppSettings("SAPClient")))
                Else
                    con = New R3Connection(ConfigurationManager.AppSettings("TESTSAPAppServerHost"), _
                             CShort(ConfigurationManager.AppSettings("TESTSAPSystemNumber")), _
                             ConfigurationManager.AppSettings("TESTSAPUsername"), _
                             ConfigurationManager.AppSettings("TESTSAPPassword"), _
                            "DE", CShort(ConfigurationManager.AppSettings("TESTSAPClient")))
                End If

                Return con
            End Get

        End Property

        Public Shared Function getProxyWrapper(ByVal bapiName As String, mObjApp As CKG.Base.Kernel.Security.App, mObjUser As CKG.Base.Kernel.Security.User, mObjPage As Web.UI.Page) As DynSapProxyObj

            Dim mObjApp2 As CKG.Base.Kernel.Security.App
            Dim mObjUser2 As CKG.Base.Kernel.Security.User
            Dim mObjPage2 As Web.UI.Page

            mObjApp2 = mObjApp
            mObjUser2 = mObjUser
            mObjPage2 = mObjPage

            Return getProxy(bapiName, mObjApp2, mObjUser2, mObjPage2)

        End Function

        Public Shared Function getProxy(ByVal bapiName As String, ByRef mObjApp As CKG.Base.Kernel.Security.App, ByRef mObjUser As CKG.Base.Kernel.Security.User, ByRef mObjPage As Web.UI.Page) As DynSapProxyObj
            '----------------------------------------------------------------------
            'Methode:       getProxy
            'Autor:         Julian Jung
            'Beschreibung:  erstellt ein neues DynProxyObj-Objekt aus der Vorlage im Proxyarray
            'Erstellt am:   05.12.2008
            '----------------------------------------------------------------------

            Dim blnTestSap As Boolean = mObjUser.IsTestUser

            If Not bapiName.Trim(" "c) = "" AndAlso Not mObjPage Is Nothing AndAlso Not mObjApp Is Nothing AndAlso Not mObjUser Is Nothing Then
                If Not checkBapiInDB(bapiName.ToUpper, blnTestSap) Then
                    Dim tmpProxyObj As DynSapProxyObj = generateNewProxy(bapiName.ToUpper)
                    If Not tmpProxyObj Is Nothing Then
                        writeStrukturIntoSQLDB(tmpProxyObj.Import, tmpProxyObj.Export, tmpProxyObj.BapiDate, tmpProxyObj.BapiName, blnTestSap)
                    End If
                End If
                Dim tmpObj As DynSapProxyObj
                tmpObj = getBapiStrukturFromSQLDB(bapiName.ToUpper(), blnTestSap)
                tmpObj.settingCalledSource(mObjApp, mObjUser, mObjPage)
                Return tmpObj
            Else
                Throw New Exception("Fehlende Parameter zum Abfragen eines ProxyObj")
            End If
        End Function

        Protected Function getDirektSapStruktur(ByVal BapiName As String) As DynSapProxyObj
            getDirektSapStruktur = generateNewProxy(BapiName)
        End Function

        Protected Function removeBapiFromDB(ByVal bapiName As String, ByVal testSap As Boolean, ByVal srcModule As String) As Boolean
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
                For Each xDynProx As DynSapProxyObj In Proxys
                    If xDynProx.BapiName = bapiName Then
                        Proxys.Remove(xDynProx)
                        Exit For
                    End If
                Next

                sqlQuery = "Delete FROM BapiStruktur WHERE BapiName=@BapiName AND ISNULL(TestSap,0)=@TestSap AND SourceModule=@SourceModule;"
                cmd.Parameters.AddWithValue("@BapiName", bapiName)
                cmd.Parameters.AddWithValue("@TestSap", testSap)
                cmd.Parameters.AddWithValue("@SourceModule", srcModule)

                cmd.CommandText = sqlQuery
                tmpInt = cmd.ExecuteNonQuery

                If Not tmpInt = 1 Then
                    status = False
                Else
                    status = True
                End If

                Return status
            Catch ex As Exception
                Throw New Exception("Fehler beim löschen von: BAPINAME= " & bapiName & " \ " & ex.Message & " \ " & ex.StackTrace)
            Finally
                cn.Close()
            End Try
        End Function


        Private Shared Function checkBapiInDB(ByVal bapiName As String, ByVal testSap As Boolean) As Boolean
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

                sqlQuery = "Select * FROM BapiStruktur WHERE BapiName=@BapiName AND ISNULL(TestSap,0)=@TestSap AND SourceModule=@SourceModule;"
                cmd.Parameters.AddWithValue("@BapiName", bapiName)
                cmd.Parameters.AddWithValue("@TestSap", testSap)
                cmd.Parameters.AddWithValue("@SourceModule", SourceModuleString)

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

                func.Exports("I_FUNCNAME").ParamValue = bapiName.Trim(" "c).ToUpper

                func.Execute()

                Dim pE_FUNCNAME As RFCParameter
                Dim pE_UDATE As RFCParameter

                pE_FUNCNAME = func.Imports("E_FUNCNAME")
                pE_UDATE = func.Imports("E_UDATE")

                If pE_FUNCNAME.ParamValue Is DBNull.Value OrElse Not pE_FUNCNAME.ParamValue.ToString.Trim(" "c) = bapiName.Trim(" "c).ToUpper Then
                    Throw New Exception("angefragtes Bapi:" & bapiName.Trim(" "c) & " <> struktur von: " & pE_FUNCNAME.ParamValue.ToString)
                End If

                'auswerten der exportparameter
                If func.Tables("EXP_TAB").RowCount > 0 Then
                    mExportTabelle = func.Tables("EXP_TAB").ToADOTableLocaleDe
                    HelpProcedures.killAllDBNullValuesInDataTable(mExportTabelle)
                    mExportTabelle.AcceptChanges()
                End If

                If func.Tables("IMP_TAB").RowCount > 0 Then
                    mImportTabelle = func.Tables("IMP_TAB").ToADOTableLocaleDe
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
                ImportStruktur.Columns.Add(New DataColumn("ElementCode", GetType(String)))
                ImportStruktur.AcceptChanges()

                'exportStruktur Definieren 
                ExportStruktur.Columns.Add(New DataColumn("Element", GetType(DataTable)))
                ExportStruktur.Columns.Add(New DataColumn("ElementCode", GetType(String)))
                ExportStruktur.AcceptChanges()

                'import
                Dim parameterTabelle As New DataTable("ParameterTabelle")
                parameterTabelle.Columns.Add(New DataColumn("PARAMETER", GetType(String)))
                parameterTabelle.Columns.Add(New DataColumn("ParameterDATATYPE", GetType(String)))
                parameterTabelle.Columns.Add(New DataColumn("ParameterValue"))
                parameterTabelle.Columns.Add(New DataColumn("ParameterLength", GetType(Int32)))
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
                                        tmpDatatable.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, GetType(String)))
                                        tmpDatatable.Columns(tmpDatatable.Columns.Count - 1).MaxLength = CInt(tmprowX("LENGTH"))
                                    ElseIf tmprowX("DATATYPE").ToString = "DATE" Then
                                        tmpDatatable.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, GetType(DateTime)))
                                    End If

                                    sapConformImportTable.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, GetType(String)))
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
                                SapTabelle.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, GetType(String)))
                                SapTabelle.Columns(SapTabelle.Columns.Count - 1).MaxLength = CInt(tmprowX("LENGTH"))
                            ElseIf tmprowX("DATATYPE").ToString = "DATE" Then
                                SapTabelle.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, GetType(DateTime)))
                            End If

                            sapConformImportTable.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, GetType(String)))
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

                parameterTabelle2.Columns.Add(New DataColumn("PARAMETER", GetType(String)))
                parameterTabelle2.Columns.Add(New DataColumn("ParameterDATATYPE", GetType(String)))
                parameterTabelle2.Columns.Add(New DataColumn("ParameterValue", GetType(Object)))
                parameterTabelle2.Columns.Add(New DataColumn("ParameterLength", GetType(Int32)))
                parameterTabelle2.AcceptChanges()

                Dim sapTabellen2(0) As DataTable
                Dim found2 As Boolean = False
                For Each tmprowX As DataRow In expTable.Rows
                    'Trace.WriteLine(String.Join(", ", tmprowX.ItemArray))
                    If Not tmprowX("STRUCTURE") Is DBNull.Value AndAlso Not tmprowX("STRUCTURE").ToString.Trim(" "c) = "" Then
                        found2 = False
                        For Each tmpDatatable As DataTable In sapTabellen2
                            If Not tmpDatatable Is Nothing Then
                                If tmpDatatable.TableName = tmprowX("STRUCTURE").ToString Then
                                    found2 = True

                                    If tmprowX("DATATYPE").ToString = "STRING" Then
                                        tmpDatatable.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, GetType(String)))
                                        tmpDatatable.Columns(tmpDatatable.Columns.Count - 1).MaxLength = CInt(tmprowX("LENGTH"))
                                    ElseIf tmprowX("DATATYPE").ToString = "DATE" Then
                                        tmpDatatable.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, GetType(DateTime)))
                                    End If

                                    tmpDatatable.AcceptChanges()
                                    Exit For
                                End If
                            End If
                        Next
                        If Not found2 Then
                            Dim SapTabelle As New DataTable(tmprowX("STRUCTURE").ToString)
                            sapTabellen2(sapTabellen2.Length - 1) = SapTabelle
                            ReDim Preserve sapTabellen2(sapTabellen2.Length)

                            If tmprowX("DATATYPE").ToString = "STRING" Then
                                SapTabelle.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, GetType(String)))
                                SapTabelle.Columns(SapTabelle.Columns.Count - 1).MaxLength = CInt(tmprowX("LENGTH"))
                            ElseIf tmprowX("DATATYPE").ToString = "DATE" Then
                                SapTabelle.Columns.Add(New DataColumn(tmprowX("PARAMETER").ToString, GetType(DateTime)))
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


        Private Shared Function getBapiStrukturFromSQLDB(ByVal BapiName As String, ByVal testSap As Boolean) As DynSapProxyObj
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
                Dim cmd As New SqlClient.SqlCommand("SELECT * FROM BapiStruktur WHERE BapiName=@BapiName AND ISNULL(TestSap,0)=@TestSap AND SourceModule=@SourceModule", cn)
                cmd.Parameters.AddWithValue("@BapiName", BapiName)
                cmd.Parameters.AddWithValue("@TestSap", testSap)
                cmd.Parameters.AddWithValue("@SourceModule", SourceModuleString)
                Dim myDataReader As SqlClient.SqlDataReader = cmd.ExecuteReader

                If Not myDataReader Is Nothing AndAlso myDataReader.Read Then
                    Return New DynSapProxyObj(BapiName, CDate(myDataReader.GetValue(1)),
                                              CType(myFormatter.Deserialize(New MemoryStream(DirectCast(myDataReader.GetValue(2), Byte()))), DataTable),
                                              DirectCast(myFormatter.Deserialize(New MemoryStream(DirectCast(myDataReader.GetValue(3), Byte()))), DataTable))
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Return Nothing
            Finally
                cn.Close()
            End Try
        End Function

        Private Shared Sub writeStrukturIntoSQLDB(ByVal ImportStruktur As DataTable, ByVal ExportStruktur As DataTable, ByVal bapiDate As Date, ByVal bapiname As String, ByVal testSap As Boolean)
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
                If Not checkBapiInDB(bapiname, testSap) Then
                    SqlQuery = "INSERT INTO [BapiStruktur] (BapiName, BapiDate, TestSap, SourceModule, ImportStruktur, ExportStruktur) VALUES (@BapiName, @BapiDate, @TestSap, @SourceModule, @ImportStruktur, @ExportStruktur);"
                    With cmd
                        .Parameters.AddWithValue("@BapiName", bapiname)
                        .Parameters.AddWithValue("@BapiDate", bapiDate)
                        .Parameters.AddWithValue("@TestSap", testSap)
                        .Parameters.AddWithValue("@SourceModule", SourceModuleString)
                        .Parameters.AddWithValue("@ImportStruktur", serializedI)
                        .Parameters.AddWithValue("@ExportStruktur", serializedE)
                    End With
                Else
                    SqlQuery = "UPDATE BapiStruktur " & _
                     "SET ImportStruktur=@ImportStruktur," & _
                     "ExportStruktur=@ExportStruktur," & _
                     "BapiDate=@BapiDate, " & _
                     "updated=@updated " & _
                     "WHERE BapiName=@BapiName AND ISNULL(TestSap,0)=@TestSap AND SourceModule=@SourceModule;"
                    With cmd
                        .Parameters.AddWithValue("@ImportStruktur", serializedI)
                        .Parameters.AddWithValue("@ExportStruktur", serializedE)
                        .Parameters.AddWithValue("@BapiDate", bapiDate)
                        .Parameters.AddWithValue("@updated", Now)
                        .Parameters.AddWithValue("@BapiName", bapiname)
                        .Parameters.AddWithValue("@TestSap", testSap)
                        .Parameters.AddWithValue("@SourceModule", SourceModuleString)
                    End With
                End If
                cmd.CommandText = SqlQuery
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception("FEHLER BEI DEM SCHREIBEN/UPDATE EINES PROXY OBJ IN DIE SQL DB: BAPINAME= " & bapiname & " \ " & ex.Message & " \ " & ex.StackTrace)
            Finally
                cn.Close()
            End Try
        End Sub

        Protected Function clearBapiStrukturenInSQLDB() As Integer
            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            Dim cmd As New SqlClient.SqlCommand
            Dim sqlQuery As String
            Dim tmpInt As Integer = 0
            cmd.Connection = cn
            cn.Open()

            Try
                sqlQuery = "Delete FROM BapiStruktur;"
                cmd.CommandText = sqlQuery
                tmpInt = cmd.ExecuteNonQuery()

                Return tmpInt
            Catch ex As Exception
                Throw (New Exception("Fehler beim löschen der Bapistrukturen \ " & ex.Message & " \ " & ex.StackTrace))
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