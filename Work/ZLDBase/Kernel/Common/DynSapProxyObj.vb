Option Explicit On
Option Strict Off

Imports System

Imports CKG.Base.Kernel
Imports CKG.Base.Business
'Imports Microsoft.Data.SAPClient
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports System.Collections.Generic
Imports System.Text
Imports System.Linq
Imports ERPConnect
Imports System.Configuration
Imports GeneralTools.Services

Namespace Common

    <Serializable()> Public Class DynSapProxyObj

#Region "Declarations"

        Private mBapiName As String
        Private mBapiDate As Date
        Private mBapiLoadet As Date
        Private mImport As DataTable
        Private mExport As DataTable
        'Private mbizTalkImportParameter As New Generic.List(Of SAPParameter)
        'Private mbizTalkExportParameter As New Generic.List(Of SAPParameter)
        Private mCommandText As String = ""
        Private mClearMe As Boolean = False
        Private mObjLogApp As CKG.Base.Kernel.Logging.Trace
        Private mObjMyApplication As CKG.Base.Kernel.Security.App
        Private mObjMyUser As CKG.Base.Kernel.Security.User
        Private mObjMyPage As Web.UI.Page
        Private Shared mSourceModuleString As String = ""

#End Region

#Region "Properties"
        Private ReadOnly Property commandtext() As String
            Get
                Return mCommandText
            End Get
        End Property

        Private ReadOnly Property SapConnectionString() As String
            Get
                Dim conStr As String = ""
                conStr = "ASHOST=" & mObjMyApplication.SAPAppServerHost & _
                                        ";CLIENT=" & mObjMyApplication.SAPClient & _
                                        ";SYSNR=" & mObjMyApplication.SAPSystemNumber & _
                                        ";USER=" & mObjMyApplication.SAPUsername & _
                                        ";PASSWD=" & mObjMyApplication.SAPPassword & _
                                        ";LANG=DE"
                Return conStr
            End Get
        End Property

        Public ReadOnly Property BapiName() As String
            Get
                Return mBapiName
            End Get
        End Property

        Public ReadOnly Property BapiDate() As Date
            Get
                Return mBapiDate
            End Get
        End Property

        Protected Friend ReadOnly Property BapiLoadet() As Date
            Get
                Return mBapiLoadet
            End Get
        End Property


        Public ReadOnly Property Import() As DataTable
            Get
                Return mImport
            End Get
        End Property

        Public ReadOnly Property Export() As DataTable
            Get
                Return mExport
            End Get
        End Property

        Public Shared ReadOnly Property SourceModuleString() As String
            Get
                If String.IsNullOrEmpty(mSourceModuleString) Then
                    Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
                    Dim myFileVersion As FileVersionInfo = FileVersionInfo.GetVersionInfo(myAssembly.Location)
                    mSourceModuleString = myAssembly.GetName().Name & "," & myFileVersion.FileVersion & ",.NET " & myAssembly.ImageRuntimeVersion
                End If

                Return mSourceModuleString
            End Get
        End Property
#End Region

#Region "Methods"

        Protected Sub New()
        End Sub


        Protected Friend Sub New(ByVal BapiName As String, ByVal SapDatum As Date, ByVal impStruktur As DataTable, ByVal expStruktur As DataTable)
            mBapiName = BapiName
            mBapiDate = SapDatum
            mBapiLoadet = Now
            mImport = GenerateANewDataTableCopy(impStruktur)
            mExport = GenerateANewDataTableCopy(expStruktur)
            'vll für spätere überprüfung der Strukturen falls sie beim BapiAufruf modifiziert werden
            '---------------------------------------------------------------
            '  mOriginalExportStruktur = GenerateANewDataTableCopy(expStruktur) 
            '  mOriginalImportStruktur = GenerateANewDataTableCopy(impStruktur) 
            '---------------------------------------------------------------
        End Sub


        Protected Friend Sub settingCalledSource(ByRef mObjApp As CKG.Base.Kernel.Security.App, ByRef mObjUser As CKG.Base.Kernel.Security.User, ByRef mObjPage As Web.UI.Page)
            mObjMyApplication = mObjApp
            mObjMyPage = mObjPage
            mObjMyUser = mObjUser
        End Sub


        Private Function GenerateANewDataTableCopy(ByVal originalDataTable As DataTable) As DataTable
            '----------------------------------------------------------------------
            'Methode:       GenerateANewDataTableCopy
            'Autor:         Julian Jung
            'Beschreibung:  erstellt eine Kopie der Import/Export Struktur
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------
            Dim newdataTable As New DataTable(originalDataTable.TableName)
            newdataTable = originalDataTable.Clone
            For Each tmpRow As DataRow In originalDataTable.Rows
                Dim newRow As DataRow = newdataTable.NewRow
                Dim i As Int32
                For i = 0 To tmpRow.ItemArray.Length - 1 Step 1
                    If TypeOf (tmpRow.Item(i)) Is DataTable Then
                        newRow.Item(i) = GenerateANewDataTableCopy(CType(tmpRow.Item(i), DataTable))
                    Else
                        newRow.Item(i) = tmpRow.Item(i)
                    End If
                Next
                newdataTable.Rows.Add(newRow)
                newdataTable.AcceptChanges()
            Next
            Return newdataTable
        End Function



        Public Function callBapi() As Boolean
            '----------------------------------------------------------------------
            'Methode:       callBapi
            'Autor:         Julian Jung
            'Beschreibung:  ruft das aktuelle BAPI im SAP auf
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------

            Dim itemsCollect As New RFCTableCollection
            Dim items As RFCTable
            Dim item As RFCStructure
            Dim tempTable As DataTable

            Dim con As New R3Connection(mObjMyApplication.SAPAppServerHost, _
                                        mObjMyApplication.SAPSystemNumber, _
                                        mObjMyApplication.SAPUsername, _
                                        mObjMyApplication.SAPPassword, _
                                        "DE", mObjMyApplication.SAPClient.ToString)

            Dim stoppuhr As Stopwatch = Nothing

            Try
                If mClearMe Then
                    clearAllValues()
                End If


                ERPConnect.LIC.SetLic(ConfigurationManager.AppSettings("ErpConnectLicense"))
                'ERPConnect.LIC.SetLic("5DVZ5588DC-25444")
                con.Open(False)


                Dim func As ERPConnect.RFCFunction = con.CreateFunction(mBapiName)

                Try
                    For Each impRow As DataRow In mImport.Rows
                        If impRow.Item(1).ToString = "PARA" Then
                            Dim paraTabelle As DataTable = CType(impRow.Item(0), DataTable)
                            For Each paraRow As DataRow In paraTabelle.Rows
                                If Not paraRow(1).ToString = "DATE" Then

                                    If IsDBNull(paraRow(2)) = False Then
                                        func.Exports(paraRow(0).ToString).ParamValue = paraRow(2)
                                    ElseIf func.Exports(paraRow(0).ToString).Type = RFCTYPE.CHAR Then
                                        If func.Exports(paraRow(0).ToString).ParamValue.ToString.Length > 0 Then
                                            func.Exports(paraRow(0).ToString).ParamValue = ""
                                        End If

                                    End If
                                Else
                                    If IsDBNull(paraRow(2)) = False Then
                                        If Not paraRow(2).ToString = "00000000" And Not paraRow(2).ToString = "" Then
                                            func.Exports(paraRow(0).ToString).ParamValue = ERPConnect.ConversionUtils.NetDate2SAPDate(CDate(paraRow(2)))
                                        End If
                                    End If
                                End If
                            Next
                        End If

                        If impRow.Item(1).ToString = "TABLE" Then

                            items = New RFCTable

                            Dim rfcTable As New RFCTable
                            Dim booFound As Boolean = False

                            'Schauen, ob die Importtabelle Bestandteil der SAP-Tabellen ist.
                            For Each rfcT As RFCTable In func.Tables

                                If rfcT.Name = CType(impRow(0), DataTable).TableName Then
                                    booFound = True
                                    Exit For
                                End If

                            Next

                            If booFound = True Then
                                items = func.Tables(CType(impRow(0), DataTable).TableName)
                                item = New RFCStructure(items.Columns)
                            Else 'wenn nein dann schauen ob die Tabelle eine Importstruktur ist
                                For Each SapStructure As RFCParameter In func.Exports
                                    If SapStructure.IsStructure Then

                                        If SapStructure.Name = CType(impRow(0), DataTable).TableName Then
                                            item = func.Exports(CType(impRow(0), DataTable).TableName).ToStructure

                                            tempTable = CType(impRow(0), DataTable)
                                            For Each dr As DataRow In tempTable.Rows
                                                For Each col As RFCTableColumn In item.Columns
                                                    Select Case col.Type
                                                        Case RFCTYPE.NUM
                                                            If Not dr(col.Name) Is System.DBNull.Value Then
                                                                If IsNumeric(dr(col.Name).ToString) Then
                                                                    item(col.Name) = dr(col.Name).ToString
                                                                End If

                                                            End If
                                                        Case RFCTYPE.CHAR
                                                            item(col.Name) = dr(col.Name).ToString
                                                        Case RFCTYPE.BCD
                                                            If Not dr(col.Name) Is System.DBNull.Value Then
                                                                If IsNumeric(dr(col.Name).ToString) Then
                                                                    item(col.Name) = dr(col.Name).ToString
                                                                End If
                                                            End If
                                                        Case RFCTYPE.DATE
                                                            If dr(col.Name).ToString = "00000000" Or dr(col.Name).ToString = "" Then
                                                                item(col.Name) = ""
                                                            Else
                                                                item(col.Name) = ERPConnect.ConversionUtils.NetDate2SAPDate(dr(col.Name).ToString)
                                                            End If
                                                        Case RFCTYPE.TIME
                                                            If dr(col.Name).ToString = "000000" Or dr(col.Name).ToString = "" Then
                                                                item(col.Name) = ""
                                                            Else
                                                                item(col.Name) = dr(col.Name).ToString
                                                            End If

                                                    End Select
                                                Next
                                            Next
                                            func.Exports(SapStructure.Name).ParamValue = item
                                        End If
                                    End If
                                Next

                            End If

                            If booFound = True Then
                                tempTable = CType(impRow(0), DataTable)
                                For Each dr As DataRow In tempTable.Rows
                                    item = items.AddRow()
                                    For Each col As RFCTableColumn In item.Columns

                                        Select Case col.Type
                                            Case RFCTYPE.NUM
                                                If Not dr(col.Name) Is System.DBNull.Value Then
                                                    If IsNumeric(dr(col.Name).ToString) Then
                                                        item(col.Name) = dr(col.Name).ToString
                                                    End If
                                                End If
                                            Case RFCTYPE.CHAR
                                                item(col.Name) = dr(col.Name).ToString
                                            Case RFCTYPE.BCD
                                                If Not dr(col.Name) Is System.DBNull.Value Then
                                                    If IsNumeric(dr(col.Name).ToString) Then
                                                        item(col.Name) = CDec(dr(col.Name).ToString)
                                                    End If
                                                End If
                                            Case RFCTYPE.DATE
                                                If dr(col.Name).ToString = "00000000" Or dr(col.Name).ToString = "" Then
                                                    item(col.Name) = ""
                                                Else
                                                    item(col.Name) = ERPConnect.ConversionUtils.NetDate2SAPDate(dr(col.Name).ToString)
                                                End If
                                            Case RFCTYPE.TIME
                                                If dr(col.Name).ToString = "000000" Or dr(col.Name).ToString = "" Then
                                                    item(col.Name) = ""
                                                Else
                                                    item(col.Name) = dr(col.Name).ToString
                                                End If

                                        End Select
                                    Next
                                Next

                                itemsCollect.Add(items)
                            Else

                            End If

                        End If
                    Next
                Catch ex As Exception
                End Try
                'import

                stoppuhr = Stopwatch.StartNew()

                func.Execute()

                stoppuhr.Stop()

                'Export-Tabellen
                For Each tmpRow As DataRow In Export.Select("ElementCode='TABLE'")
                    ' Exportparameter Struktur oder Tabelle
                    For Each it As RFCParameter In func.Imports
                        If it.IsStructure Then
                            If it.Name = CType(tmpRow(0), DataTable).TableName Then
                                item = it.ToStructure()
                                Dim tblTemp As New DataTable
                                tblTemp.TableName = it.Name
                                For Each col As RFCTableColumn In item.Columns
                                    tblTemp.Columns.Add(col.Name, GetType(System.String))
                                Next
                                Dim Row As DataRow
                                Row = tblTemp.NewRow
                                For Each col As RFCTableColumn In item.Columns

                                    Select Case col.Type
                                        Case RFCTYPE.NUM
                                            Row(col.Name) = item.Item(col.Name).ToString
                                        Case RFCTYPE.CHAR
                                            Row(col.Name) = item.Item(col.Name).ToString
                                        Case RFCTYPE.BCD
                                            If item.Item(col.Name) Is System.DBNull.Value Then
                                                Row(col.Name) = ""
                                            End If
                                        Case RFCTYPE.DATE
                                            If item.Item(col.Name).ToString = "00000000" Or item.Item(col.Name).ToString = "" Then
                                                Row(col.Name) = ""
                                            Else
                                                Row(col.Name) = ERPConnect.ConversionUtils.SAPDate2NetDate(item.Item(col.Name).ToString)
                                            End If
                                        Case RFCTYPE.TIME
                                            If item.Item(col.Name).ToString = "000000" Or item.Item(col.Name).ToString = "" Then
                                                Row(col.Name) = ""
                                            Else
                                                Row(col.Name) = item.Item(col.Name).ToString
                                            End If
                                        Case RFCTYPE.BYTE, RFCTYPE.ITAB

                                    End Select
                                Next
                                tblTemp.Rows.Add(Row)
                                tblTemp.AcceptChanges()
                                tmpRow(0) = tblTemp
                            ElseIf it.IsTable Then
                                Dim rfcT As RFCTable
                                rfcT = it.ToTable
                                If rfcT.Name = CType(tmpRow(0), DataTable).TableName Then

                                    Dim tblTemp As DataTable = rfcT.ToADOTableLocaleDe

                                    For Each col As RFCTableColumn In rfcT.Columns

                                        For Each Row As DataRow In tblTemp.Rows
                                            Select Case col.Type

                                                Case RFCTYPE.NUM
                                                    Row(col.Name) = Row(col.Name).ToString

                                                Case RFCTYPE.CHAR
                                                    Row(col.Name) = Row(col.Name).ToString
                                                Case RFCTYPE.BCD
                                                    If Row(col.Name) Is System.DBNull.Value Then
                                                        Row(col.Name) = ""
                                                    End If
                                                Case RFCTYPE.DATE
                                                    If Row(col.Name).ToString = "00000000" Or Row(col.Name).ToString = "" Then
                                                        Row(col.Name) = ""
                                                    Else
                                                        Row(col.Name) = ERPConnect.ConversionUtils.SAPDate2NetDate(Row(col.Name).ToString)
                                                    End If
                                                Case RFCTYPE.BYTE, RFCTYPE.ITAB

                                                Case RFCTYPE.TIME
                                                    If Row(col.Name).ToString = "000000" Or Row(col.Name).ToString = "" Then
                                                        Row(col.Name) = ""
                                                    Else
                                                        Row(col.Name) = Row(col.Name).ToString
                                                    End If
                                            End Select

                                        Next

                                    Next

                                    tblTemp.AcceptChanges()

                                    tmpRow(0) = tblTemp

                                End If

                            End If
                        End If
                    Next

                    For Each rfcT As RFCTable In func.Tables

                        If rfcT.Name = CType(tmpRow(0), DataTable).TableName Then


                            Dim tblTemp As DataTable = rfcT.ToADOTableLocaleDe
                            Dim tblTempResult As DataTable = rfcT.ToADOTableLocaleDe.Clone
                            Dim NewRow As DataRow

                            'Datentyp auf DateTime ändern
                            For Each col As RFCTableColumn In rfcT.Columns
                                If col.Type = RFCTYPE.DATE Then
                                    tblTempResult.Columns(col.Name).DataType = GetType(System.DateTime)
                                    tblTempResult.AcceptChanges()
                                End If
                            Next

                            'Vorhandene Werte die nicht DateTime sind in die neue Tabelle schreiben.
                            For Each dr As DataRow In tblTemp.Rows

                                NewRow = tblTempResult.NewRow

                                For Each col As RFCTableColumn In rfcT.Columns
                                    If Not col.Type = RFCTYPE.DATE Then
                                        NewRow(col.Name) = dr(col.Name)
                                    End If
                                Next
                                tblTempResult.Rows.Add(NewRow)
                            Next
                            tblTempResult.AcceptChanges()


                            For Each col As RFCTableColumn In rfcT.Columns

                                For i As Integer = 0 To tblTempResult.Rows.Count - 1

                                    Dim Row As DataRow = tblTempResult.Rows(i)

                                    Select Case col.Type

                                        Case RFCTYPE.NUM
                                            Row(col.Name) = Row(col.Name).ToString

                                        Case RFCTYPE.CHAR
                                            Row(col.Name) = Row(col.Name).ToString
                                        Case RFCTYPE.BCD
                                            Row(col.Name) = Row(col.Name).ToString
                                        Case RFCTYPE.DATE
                                            If rfcT.Rows(i)(col.Name).ToString = "00000000" Or rfcT.Rows(i)(col.Name).ToString = "" Then
                                                Row(col.Name) = System.DBNull.Value
                                            ElseIf IsDate(ERPConnect.ConversionUtils.SAPDate2NetDate(rfcT.Rows(i)(col.Name))) Then
                                                Row(col.Name) = ERPConnect.ConversionUtils.SAPDate2NetDate(rfcT.Rows(i)(col.Name))
                                            End If
                                        Case RFCTYPE.TIME
                                            If Row(col.Name).ToString = "000000" Or Row(col.Name).ToString = "" Then
                                                Row(col.Name) = ""
                                            Else
                                                Row(col.Name) = Row(col.Name).ToString
                                            End If
                                        Case RFCTYPE.BYTE, RFCTYPE.ITAB

                                    End Select

                                Next

                            Next

                            tblTempResult.AcceptChanges()

                            tmpRow(0) = tblTempResult

                        End If

                    Next

                Next

                For Each tmpRow As DataRow In Export.Select("ElementCode='PARA'")
                    Dim paraTabelle As DataTable = CType(tmpRow.Item(0), DataTable)
                    For Each paraRow As DataRow In paraTabelle.Rows
                        If Not paraRow(1).ToString = "DATE" Then
                            For Each it As RFCParameter In func.Imports
                                If paraRow("Parameter").ToString = it.Name Then
                                    paraRow("ParameterValue") = it.ParamValue
                                End If
                            Next
                        Else
                            For Each it As RFCParameter In func.Imports
                                If paraRow("Parameter").ToString = it.Name Then
                                    paraRow("ParameterValue") = ERPConnect.ConversionUtils.SAPDate2NetDate(it.ParamValue.ToString)
                                End If
                            Next
                        End If
                    Next
                Next

                Dim logger As New LogService(String.Empty, String.Empty)
                logger.LogSapCall(BapiName, String.Empty, Import, Export, True, stoppuhr.Elapsed.TotalSeconds)

                Return True
            Catch ex As ERPException

                Dim callduration As Double = -1

                If (stoppuhr IsNot Nothing AndAlso stoppuhr.IsRunning) Then
                    stoppuhr.Stop()
                    callduration = stoppuhr.Elapsed.TotalSeconds
                End If

                PreserveStackTrace(ex)

                Dim logger As New LogService(String.Empty, String.Empty)
                logger.LogSapCall(BapiName, String.Empty, Import, Export, False, callduration)

                If ex.Source.Contains("ERPConnect35") Then
                    If ex.ABAPException.Length > 0 Then
                        Throw New Exception(ex.ABAPException)
                    ElseIf ex.Message.Contains("Connect to SAP gateway failed") Then
                        Throw New Exception("Connect to SAP gateway failed")
                    Else
                        Throw New Exception("Fehler in DynSapProxyObj.callBapi: " & ex.Message)
                    End If
                Else
                    '' Ausnahme erneut werfen.  Ausnahme landet dann bei der Standard Ausnahme Behandlung
                    Throw
                End If

            Finally

                If Not con Is Nothing Then
                    con.Close()
                    con.Dispose()
                End If
                mClearMe = True
            End Try
        End Function

        Public Function getImportTable(ByVal Name As String) As DataTable
            '----------------------------------------------------------------------
            'Methode:       getImportTable
            'Autor:         Julian Jung
            'Beschreibung:  liefert eine benannte Importtabelle zurück
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------
            Try
                If mClearMe Then
                    clearAllValues()
                End If
                For Each tmpRow As DataRow In Import.Select("ElementCode='TABLE'")
                    If CType(tmpRow(0), DataTable).TableName = Name Then
                        Return CType(tmpRow(0), DataTable)
                    End If
                Next
                Throw New Exception
            Catch ex As Exception
                Throw New Exception("Importtabelle mit dem Namen: " & Name & " nicht vorhanden!")
            End Try
        End Function

        Public Function getImportTables() As IEnumerable(Of DataTable)
            '----------------------------------------------------------------------
            'Methode:       getExportTables
            'Autor:         Matthias Jenzen
            'Beschreibung:  liefert alle Importtabellen zurück
            'Erstellt am:   10.12.2012
            '----------------------------------------------------------------------

            Dim list As List(Of DataTable)

            list = New List(Of DataTable)

            Try
                For Each tmpRow As DataRow In Import.Select("ElementCode='TABLE'")
                    If Not tmpRow(0) Is DBNull.Value Then

                        list.Add(CType(tmpRow(0), DataTable).Copy)

                    End If
                Next
                Throw New Exception
            Catch ex As Exception
            End Try

            Return list
        End Function

        Public Sub setImportParameter(ByVal Name As String, ByVal Wert As String)
            Try
                If mClearMe Then
                    clearAllValues()
                End If
                CType(Import.Select("ElementCode='PARA'")(0)(0), DataTable).Select("PARAMETER='" & Name & "'")(0)(2) = Wert
            Catch ex As Exception
                Throw New Exception("ImportParameter mit dem Namen: " & Name & " und dem Wert: " & Wert & " konnte nicht gesetzt werden!")
            End Try
        End Sub

        Public Function getExportParameter(ByVal Name As String) As String
            Try
                If Not CType(Export.Select("ElementCode='PARA'")(0)(0), DataTable).Select("PARAMETER='" & Name & "'")(0)(2) Is DBNull.Value Then
                    Return CType(Export.Select("ElementCode='PARA'")(0)(0), DataTable).Select("PARAMETER='" & Name & "'")(0)(2).ToString.Trim
                Else
                    Return ""
                End If
            Catch ex As Exception
                Throw New Exception("ExportParameter mit dem Namen: " & Name & " nicht vorhanden!")
            End Try
        End Function
        
        Public Function getExportParameterByte(ByVal Name As String) As Byte()
            Dim buffer As Byte()
            Try
                If (Not DirectCast(Me.Export.Select("ElementCode='PARA'")(0).Item(0), DataTable).Select(("PARAMETER='" & Name & "'"))(0).Item(2) Is DBNull.Value) Then
                    If (DirectCast(Me.Export.Select("ElementCode='PARA'")(0).Item(0), DataTable).Select(("PARAMETER='" & Name & "'"))(0).Item(2).GetType Is Type.GetType("System.Byte[]")) Then
                        Return DirectCast(DirectCast(Me.Export.Select("ElementCode='PARA'")(0).Item(0), DataTable).Select(("PARAMETER='" & Name & "'"))(0).Item(2), Byte())
                    End If
                    Return Nothing
                End If
                buffer = Nothing
            Catch ex As Exception
                Throw New Exception(("ExportParameter mit dem Namen: " & Name & " nicht vorhanden!"))
            End Try
            Return buffer
        End Function

        Public Function getExportTable(ByVal Name As String) As DataTable
            '----------------------------------------------------------------------
            'Methode:       getExportTable
            'Autor:         Julian Jung
            'Beschreibung:  liefert eine benannte Exporttabelle zurück
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------
            Try
                For Each tmpRow As DataRow In Export.Select("ElementCode='TABLE'")
                    If Not tmpRow(0) Is DBNull.Value Then
                        If CType(tmpRow(0), DataTable).TableName = Name Then
                            Return CType(tmpRow(0), DataTable).Copy
                        End If
                    End If
                Next
                Throw New Exception
            Catch ex As Exception
                Throw New Exception("Exportabelle mit dem Namen: " & Name & " nicht vorhanden!")
            End Try
        End Function

        Public Function getExportTables() As IEnumerable(Of DataTable)
            '----------------------------------------------------------------------
            'Methode:       getExportTables
            'Autor:         Matthias Jenzen
            'Beschreibung:  liefert alle Exporttabellen zurück
            'Erstellt am:   10.12.2012
            '----------------------------------------------------------------------

            Dim list As List(Of DataTable)

            list = New List(Of DataTable)

            Try
                For Each tmpRow As DataRow In Export.Select("ElementCode='TABLE'")
                    If Not tmpRow(0) Is DBNull.Value Then

                        list.Add(CType(tmpRow(0), DataTable).Copy)

                    End If
                Next
                Throw New Exception
            Catch ex As Exception
            End Try

            Return list
        End Function

        Private Function clearAllValues() As Boolean
            '----------------------------------------------------------------------
            'Methode:       clearAllValues
            'Autor:         Julian Jung
            'Beschreibung:  löscht alle Werte aus der Import/Export-Struktur
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------
            Try
                For Each impRow As DataRow In mImport.Rows
                    If impRow.Item(1).ToString = "PARA" Then
                        Dim paraTabelle As DataTable = CType(impRow.Item(0), DataTable)
                        For Each paraRow As DataRow In paraTabelle.Rows
                            paraRow.Item(2) = ""
                        Next
                        paraTabelle.AcceptChanges()
                    End If
                    If impRow.Item(1).ToString = "TABLE" OrElse impRow.Item(1).ToString = "SAPTABLE" Then
                        CType(impRow.Item(0), DataTable).Rows.Clear()
                        CType(impRow.Item(0), DataTable).Clear()
                        CType(impRow.Item(0), DataTable).AcceptChanges()
                    End If
                Next
                For Each expRow As DataRow In Export.Rows
                    If expRow.Item(1).ToString = "PARA" Then
                        Dim paraTabelle As DataTable = CType(expRow.Item(0), DataTable)
                        For Each paraRow As DataRow In paraTabelle.Rows
                            paraRow.Item(2) = ""
                        Next
                        paraTabelle.AcceptChanges()
                    End If

                    If expRow.Item(1).ToString = "TABLE" Then
                        CType(expRow.Item(0), DataTable).Rows.Clear()
                        CType(expRow.Item(0), DataTable).Clear()
                        CType(expRow.Item(0), DataTable).AcceptChanges()
                    End If
                Next
                'mbizTalkExportParameter.Clear()
                'mbizTalkImportParameter.Clear()
                mClearMe = False
                Return True
            Catch ex As Exception
                Throw New Exception("Fehler in DynSapProxyObj.clearAllValues: " & ex.Message)
                Return False
            End Try
        End Function

        Private Function ConformTableForSAP(ByVal webTable As DataTable) As DataTable
            '----------------------------------------------------------------------
            'Methode:       ConformTableForSAP
            'Autor:         Julian Jung
            'Beschreibung:  macht eine Tabelle SAP-Conform
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------
            Dim SAPTable As DataTable
            Dim dateArray() As Int32 = {-1}
            For Each tmpRow As DataRow In Import.Select("ElementCode='SAPTABLE'")
                If Not tmpRow(0) Is Nothing Then
                    SAPTable = CType(tmpRow(0), DataTable)
                    If SAPTable.TableName = webTable.TableName Then
                        For Each tmpColumn As DataColumn In webTable.Columns
                            If tmpColumn.DataType Is System.Type.GetType("System.DateTime") Then
                                ReDim Preserve dateArray(dateArray.Length)
                                dateArray(dateArray.Length - 1) = webTable.Columns.IndexOf(tmpColumn)
                            End If
                        Next
                        If Not dateArray.Length = 1 Then
                            Dim itemCounter As Int32 = 0
                            Dim newRow As DataRow
                            Dim maxcount As Int32 = 0
                            'sicherheit für die erweiterung von saptabellen
                            If webTable.Columns.Count > SAPTable.Columns.Count Then
                                maxcount = SAPTable.Columns.Count
                            Else
                                maxcount = webTable.Columns.Count
                            End If
                            For Each tmpRow2 As DataRow In webTable.Rows
                                newRow = SAPTable.NewRow
                                While itemCounter < maxcount
                                    If dateArray.Contains(itemCounter) Then
                                        newRow.Item(itemCounter) = ConvertToSapDate(tmpRow2.Item(itemCounter).ToString)
                                    Else
                                        newRow.Item(itemCounter) = tmpRow2.Item(itemCounter).ToString
                                    End If
                                    itemCounter += 1
                                End While
                                SAPTable.Rows.Add(newRow)
                                SAPTable.AcceptChanges()
                                itemCounter = 0
                            Next
                            Return SAPTable
                        Else
                            Return webTable
                        End If
                    End If
                End If
            Next
            Return webTable
        End Function

        Private Function ConformTableForWeb(ByVal SapTable As DataTable) As DataTable
            '----------------------------------------------------------------------
            'Methode:       ConformTableForWeb
            'Autor:         Julian Jung
            'Beschreibung:  macht eine Tabelle WEB-Conform
            'Erstellt am:   09.12.2008
            '----------------------------------------------------------------------
            Dim WebTable As DataTable
            Dim dateArray() As Int32 = {-1}
            HelpProcedures.killAllDBNullValuesInDataTable(SapTable)
            For Each tmpRow As DataRow In Export.Select("ElementCode='Table'")
                If Not tmpRow(0) Is Nothing Then
                    WebTable = CType(tmpRow(0), DataTable)
                    If SapTable.TableName = WebTable.TableName Then
                        For Each tmpColumn As DataColumn In WebTable.Columns
                            If tmpColumn.DataType Is System.Type.GetType("System.DateTime") Then
                                ReDim Preserve dateArray(dateArray.Length)
                                dateArray(dateArray.Length - 1) = WebTable.Columns.IndexOf(tmpColumn)
                            End If
                        Next
                        If Not dateArray.Length = 1 Then
                            Dim itemCounter As Int32 = 0
                            Dim newRow As DataRow
                            'sicherheit für die erweiterung von saptabellen
                            Dim maxcount As Int32 = 0
                            If WebTable.Columns.Count > SapTable.Columns.Count Then
                                maxcount = SapTable.Columns.Count
                            Else
                                maxcount = WebTable.Columns.Count
                            End If
                            For Each tmpRow2 As DataRow In SapTable.Rows
                                newRow = WebTable.NewRow
                                While itemCounter < maxcount

                                    'wenn ein Feld in einer SAP tabelle hinzukommt, 
                                    'entsteht hier ein fehler, da die Webtabelle noch nciht auf dem Stand ist wie das BAPi die tabelle zurückliefert
                                    'nämlich mit ein paar Feldern mehr
                                    If dateArray.Contains(itemCounter) Then
                                        newRow.Item(itemCounter) = IIf(ConvertFromSapDate(tmpRow2.Item(itemCounter).ToString) = Nothing, DBNull.Value, ConvertFromSapDate(tmpRow2.Item(itemCounter).ToString))
                                    Else
                                        newRow.Item(itemCounter) = tmpRow2.Item(itemCounter).ToString
                                    End If
                                    itemCounter += 1
                                End While
                                WebTable.Rows.Add(newRow)
                                WebTable.AcceptChanges()
                                itemCounter = 0
                            Next
                        Else
                            Return SapTable 'es existier eine Tabelle die keine Datewerte Enthält aber trotzdem befüllt werden muss
                        End If
                        Return WebTable
                    End If
                End If
            Next
            Return SapTable
        End Function

        Private Function ConvertToSapDate(ByVal datInput As Object) As String
            If datInput Is Nothing OrElse datInput Is DBNull.Value OrElse datInput.ToString = New Date().ToString OrElse datInput.ToString = "" OrElse datInput.ToString = New Date().ToShortDateString Then
                Return "00000000"
            ElseIf IsDate(datInput) Then
                Dim myDate As Date
                myDate = CDate(datInput)
                Return Year(myDate) & Right("0" & Month(myDate), 2) & Right("0" & Day(myDate), 2)
            Else
                Return ""
            End If
        End Function

        Private Function ConvertFromSapDate(ByVal datInput As String) As Date
            If Not datInput = "" AndAlso Not datInput.Length < 8 AndAlso Not datInput = "00000000" Then
                Dim strTemp As String = Right(datInput, 2) & "." & Mid(datInput, 5, 2) & "." & Left(datInput, 4)
                If IsDate(strTemp) Then
                    Return CDate(strTemp)
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        End Function

        '''
        ''' Wenn eine Exception in einem try/catch Block abgefangen wird und wieder geschmißen werden soll, kann es vorkommen dass der Stacktrace abgeschnitten wird
        ''' und der Stacktrace beginnt mit der throw statt mit der Zeile wo die Ausnahme tatsächlich verursacht wurde
        ''' Aufruf bewahrt das Stacktrace so dass die Stelle an der die Ausnahme auftrat auch gespeichert wird
        '''
        Private Shared Sub PreserveStackTrace(e As Exception)
            Dim ctx As StreamingContext = New StreamingContext(StreamingContextStates.CrossAppDomain)
            Dim mgr As ObjectManager = New ObjectManager(Nothing, ctx)
            Dim si As SerializationInfo = New SerializationInfo(e.[GetType](), New FormatterConverter())

            e.GetObjectData(si, ctx)
            mgr.RegisterObject(e, 1, si)
            mgr.DoFixups()
        End Sub
#End Region

    End Class
End Namespace
' ************************************************
' $History: DynSapProxyObj.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 6.08.10    Time: 13:13
' Updated in $/CKAG/Base/Save
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 14.05.10   Time: 17:22
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 14  *****************
' User: Rudolpho     Date: 12.05.10   Time: 18:21
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 9.07.09    Time: 17:17
' Updated in $/CKAG/Base/Kernel/Common
' nachbesserung im dateconverter
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 18.06.09   Time: 8:34
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 28.04.09   Time: 16:36
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 6.03.09    Time: 8:40
' Updated in $/CKAG/Base/Kernel/Common
' SapTabellen erweiterung ist nun egal, auer imports nat�rlich
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 5.03.09    Time: 17:26
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 5.03.09    Time: 17:22
' Updated in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 5.03.09    Time: 10:18
' Updated in $/CKAG/Base/Kernel/Common
' Bugfix bei Input/Output table, falsche direction 
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 3.02.09    Time: 14:29
' Updated in $/CKAG/Base/Kernel/Common
' Dyn Proxy Bugfix
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 26.01.09   Time: 10:00
' Updated in $/CKAG/Base/Kernel/Common
' dynProxy Bugfix 
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 16.12.08   Time: 11:49
' Updated in $/CKAG/Base/Kernel/Common
' dp anpassung
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