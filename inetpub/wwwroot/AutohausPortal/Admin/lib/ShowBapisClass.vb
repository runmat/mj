Imports CKG.Base.Common
Imports System.Linq

Public Class ShowBapisClass
    Inherits CKG.Base.Common.DynSapProxy

#Region "Deklarations"

    Private mDBProxys As DBProxys

    Private mObjDynSapProxyObjWEB As DynSapProxyObj
    Private mObjDynSapProxyObjSAP As DynSapProxyObj

    'webTabellen
    Private mWEBImportParameter As DataTable
    Private mWEBImportTabellen() As DataTable
    Private mWEBExportParameter As DataTable
    Private mWEBExportTabellen() As DataTable
    Private mWEBTabellen() As DataTable
    Private mWEBTabellenOverview As DataTable

    'sapTabellen
    Private mSAPImportParameter As DataTable
    Private mSAPImportTabellen() As DataTable
    Private mSAPExportParameter As DataTable
    Private mSAPExportTabellen() As DataTable
    Private mSAPTabellen() As DataTable
    Private mSAPTabellenOverview As DataTable

    Private mDBFilter As String = "%"

#End Region

#Region "Properties"


    Public WriteOnly Property DBFilter() As String
        Set(ByVal value As String)
            mDBFilter = value.Replace("*", "%")
        End Set
    End Property


    Private ReadOnly Property getAStrukturTable() As DataTable
        Get
            Dim tmpNewDataTable As New DataTable
            tmpNewDataTable.Columns.Add("SpaltenName", String.Empty.GetType)
            tmpNewDataTable.Columns.Add("SpaltenTyp", String.Empty.GetType)
            tmpNewDataTable.Columns.Add("Laenge", String.Empty.GetType)
            tmpNewDataTable.AcceptChanges()
            Return tmpNewDataTable
        End Get

    End Property


    Public ReadOnly Property WebBapiName() As String
        Get
            If Not mObjDynSapProxyObjWEB Is Nothing Then
                Return mObjDynSapProxyObjWEB.BapiName
            Else
                Return Nothing
            End If

        End Get
    End Property



    Public ReadOnly Property SapBapiName() As String
        Get
            If Not mObjDynSapProxyObjSAP Is Nothing Then
                Return mObjDynSapProxyObjSAP.BapiName
            Else
                Return Nothing
            End If

        End Get
    End Property


    Public ReadOnly Property SapBapiDatum() As Date
        Get
            If Not mObjDynSapProxyObjSAP Is Nothing Then
                Return mObjDynSapProxyObjSAP.BapiDate
            Else
                Return Nothing
            End If

        End Get
    End Property


    Public ReadOnly Property WebBapiDatum() As Date
        Get
            If Not mObjDynSapProxyObjWEB Is Nothing Then
                Return mObjDynSapProxyObjWEB.BapiDate
            Else
                Return Nothing
            End If

        End Get
    End Property


    Public ReadOnly Property WEBImportParameter() As DataTable
        Get
            Return mWEBImportParameter
        End Get
    End Property


    Public ReadOnly Property WEBExportParameter() As DataTable
        Get
            Return mWEBExportParameter
        End Get
    End Property


    Public ReadOnly Property SAPImportParameter() As DataTable
        Get
            Return mSAPImportParameter
        End Get
    End Property


    Public ReadOnly Property SAPExportParameter() As DataTable
        Get
            Return mSAPExportParameter
        End Get
    End Property


    Public ReadOnly Property WEBTabellen() As DataTable
        Get
            Return mWEBTabellenOverview
        End Get
    End Property



    Private ReadOnly Property WEBTabellenOverview() As DataTable
        Get
            If mWEBTabellenOverview Is Nothing Then
                mWEBTabellenOverview = New DataTable
                mWEBTabellenOverview.Columns.Add("TabellenName", String.Empty.GetType)
                mWEBTabellenOverview.Columns.Add("Tabellengroesse", String.Empty.GetType)
                mWEBTabellenOverview.AcceptChanges()
            End If
            Return mWEBTabellenOverview
        End Get
    End Property

    Public ReadOnly Property SAPTabellen() As DataTable
        Get
            Return mSAPTabellenOverview
        End Get
    End Property

    Private ReadOnly Property SAPTabellenOverview() As DataTable
        Get
            If mSAPTabellenOverview Is Nothing Then
                mSAPTabellenOverview = New DataTable
                mSAPTabellenOverview.Columns.Add("TabellenName", String.Empty.GetType)
                mSAPTabellenOverview.Columns.Add("Tabellengroesse", String.Empty.GetType)
                mSAPTabellenOverview.AcceptChanges()
            End If
            Return mSAPTabellenOverview
        End Get
    End Property

    Public ReadOnly Property DBProxys() As DataTable
        Get
            If Not mDBProxys Is Nothing Then
                mDBProxys.RejectChanges()
                For Each tmpRow As DataRow In mDBProxys.Select("BapiName LIKE '" & mDBFilter & "'")
                    tmpRow.SetModified()
                Next
            End If
            Return mDBProxys
        End Get
    End Property

#End Region

#Region "Methodes"

    Public Sub New(ByVal testSap As Boolean)
        mDBProxys = New DBProxys(ConfigurationManager.AppSettings("Connectionstring"), testSap)
    End Sub

    Public Function getSAPImportTabelle(ByVal TabellenName As String) As DataTable
        For Each tmpTable As DataTable In mSAPImportTabellen
            If tmpTable.TableName = TabellenName Then
                Return tmpTable
            End If
        Next
        Return Nothing
    End Function


    Public Function getSAPExportTabelle(ByVal TabellenName As String) As DataTable
        For Each tmpTable As DataTable In mSAPExportTabellen
            If tmpTable.TableName = TabellenName Then
                Return tmpTable
            End If
        Next
        Return Nothing
    End Function

    Public Function getSAPTabellenTabelle(ByVal TabellenName As String) As DataTable
        For Each tmpTable As DataTable In mSAPTabellen
            If tmpTable.TableName = TabellenName Then
                Return tmpTable
            End If
        Next
        Return Nothing
    End Function

    Public Function getWEBImportTabelle(ByVal TabellenName As String) As DataTable
        For Each tmpTable As DataTable In mWEBImportTabellen
            If tmpTable.TableName = TabellenName Then
                Return tmpTable
            End If
        Next
        Return Nothing
    End Function


    Public Function getWEBExportTabelle(ByVal TabellenName As String) As DataTable
        For Each tmpTable As DataTable In mWEBExportTabellen
            If tmpTable.TableName = TabellenName Then
                Return tmpTable
            End If
        Next
        Return Nothing
    End Function

    Public Function getWEBTabellenTabelle(ByVal TabellenName As String) As DataTable
        For Each tmpTable As DataTable In mWEBTabellen
            If tmpTable.TableName = TabellenName Then
                Return tmpTable
            End If
        Next
        Return Nothing
    End Function

    Public Sub fillDBProxys()
        mDBProxys.refill()
    End Sub

    Public Sub getWebStruktur(ByVal BapiName As String, ByRef mObjApp As CKG.Base.Kernel.Security.App, ByRef mObjUser As CKG.Base.Kernel.Security.User, ByRef mObjPage As Web.UI.Page)
        mObjDynSapProxyObjWEB = DynSapProxy.getProxy(BapiName, mObjApp, mObjUser, mObjPage)
        Dim tmpRow As DataRow
        Dim i As Int32 = 0
        'alles erstmal clearen, da bei mehrfachem bapi aufruf die tabellen vom alten noch gefüllt sind
        mWEBExportParameter = Nothing
        mWEBImportParameter = Nothing
        ReDim mWEBExportTabellen(0)
        ReDim mWEBImportTabellen(0)
        ReDim mWEBTabellen(0)
        mWEBTabellenOverview = Nothing

        'holen aller Import/Export tabellen aus der struktur 
        Dim tmpImportTableRows() As DataRow = mObjDynSapProxyObjWEB.Import.Select("ElementCode='TABLE'")
        Dim tmpExportTableRows() As DataRow = mObjDynSapProxyObjWEB.Export.Select("ElementCode='TABLE'")
        mWEBImportParameter = CType(mObjDynSapProxyObjWEB.Import.Select("ElementCode='PARA'")(0)(0), DataTable).Copy
        mWEBExportParameter = CType(mObjDynSapProxyObjWEB.Export.Select("ElementCode='PARA'")(0)(0), DataTable).Copy

        ReDim mWEBImportTabellen(tmpImportTableRows.Count - 1)
        ReDim mWEBExportTabellen(tmpExportTableRows.Count - 1)
        ReDim mWEBTabellen(tmpExportTableRows.Count + tmpImportTableRows.Count)

        'Jede tabelle ins Tabellen Array schreiben.
        i = 0
        For Each tmpRow In tmpImportTableRows
            mWEBImportTabellen(i) = getAStrukturTable
            mWEBImportTabellen(i).TableName = CType(tmpRow(0), DataTable).TableName
            For Each tmpColum As DataColumn In CType(tmpRow(0), DataTable).Columns
                mWEBImportTabellen(i).Rows.Add(tmpColum.ColumnName.ToString, tmpColum.DataType.ToString, tmpColum.MaxLength.ToString)
                mWEBImportTabellen(i).AcceptChanges()
            Next
            i += 1
        Next
        i = 0
        For Each tmpRow In tmpExportTableRows
            mWEBExportTabellen(i) = getAStrukturTable
            mWEBExportTabellen(i).TableName = CType(tmpRow(0), DataTable).TableName
            For Each tmpColum As DataColumn In CType(tmpRow(0), DataTable).Columns
                mWEBExportTabellen(i).Rows.Add(tmpColum.ColumnName.ToString, tmpColum.DataType.ToString, tmpColum.MaxLength.ToString)
                mWEBExportTabellen(i).AcceptChanges()
            Next
            i += 1
        Next

        i = 0

        For Each tmpDatatable As DataTable In If(mWEBExportTabellen.Length > mWEBImportTabellen.Length, mWEBExportTabellen, mWEBImportTabellen)
            If Not tmpDatatable Is Nothing Then
                For Each tmpDatatable2 As DataTable In If(mWEBImportTabellen.Contains(tmpDatatable), mWEBExportTabellen, mWEBImportTabellen)
                    If Not tmpDatatable2 Is Nothing Then
                        If tmpDatatable2.TableName = tmpDatatable.TableName Then
                            WEBTabellenOverview.Rows.Add(tmpDatatable.TableName, tmpDatatable.Rows.Count.ToString)
                            mWEBTabellen(i) = tmpDatatable.Copy

                            'hier prüfen woher das objekt kommt und dann direkt die position im array entfernen, das objekt alleine auf nothing zu setzen bringt nix, 
                            'ist schon irgendwie doof 
                            If Not Array.IndexOf(mWEBExportTabellen, tmpDatatable) = -1 Then
                                mWEBExportTabellen(Array.IndexOf(mWEBExportTabellen, tmpDatatable)) = Nothing
                            End If
                            If Not Array.IndexOf(mWEBExportTabellen, tmpDatatable2) = -1 Then
                                mWEBExportTabellen(Array.IndexOf(mWEBExportTabellen, tmpDatatable2)) = Nothing
                            End If
                            If Not Array.IndexOf(mWEBImportTabellen, tmpDatatable) = -1 Then
                                mWEBImportTabellen(Array.IndexOf(mWEBImportTabellen, tmpDatatable)) = Nothing
                            End If
                            If Not Array.IndexOf(mWEBImportTabellen, tmpDatatable2) = -1 Then
                                mWEBImportTabellen(Array.IndexOf(mWEBImportTabellen, tmpDatatable2)) = Nothing
                            End If
                            i += 1
                        End If
                    End If

                Next
            End If
        Next


        i = 0
        For Each tmpDataTable As DataTable In mWEBExportTabellen
            If Not tmpDataTable Is Nothing Then
                mWEBExportParameter.Rows.Add(tmpDataTable.TableName, "Tabelle", tmpDataTable.Rows.Count.ToString)
                mWEBExportTabellen(i) = tmpDataTable
                i += 1
            End If
        Next
        ReDim Preserve mWEBExportTabellen(i - 1)

        i = 0
        For Each tmpDataTable As DataTable In mWEBImportTabellen
            If Not tmpDataTable Is Nothing Then
                mWEBImportParameter.Rows.Add(tmpDataTable.TableName, "Tabelle", tmpDataTable.Rows.Count.ToString)
                mWEBImportTabellen(i) = tmpDataTable
                i += 1
            End If
        Next
        ReDim Preserve mWEBImportTabellen(i - 1)


        i = 0
        For Each tmpDataTable As DataTable In mWEBTabellen
            If Not tmpDataTable Is Nothing Then
                mWEBTabellen(i) = tmpDataTable
                i += 1
            End If
        Next
        ReDim Preserve mWEBTabellen(i - 1)



    End Sub


    Public Sub getSAPStruktur(ByVal BapiName As String)
        mObjDynSapProxyObjSAP = getDirektSapStruktur(BapiName)
        Dim tmpRow As DataRow
        Dim i As Int32 = 0
        mSAPExportParameter = Nothing
        mSAPImportParameter = Nothing
        ReDim mSAPExportTabellen(0)
        ReDim mSAPImportTabellen(0)
        ReDim mSAPTabellen(0)
        mSAPTabellenOverview = Nothing

        'holen aller Import/Export tabellen aus der struktur 
        Dim tmpImportTableRows() As DataRow = mObjDynSapProxyObjSAP.Import.Select("ElementCode='TABLE'")
        Dim tmpExportTableRows() As DataRow = mObjDynSapProxyObjSAP.Export.Select("ElementCode='TABLE'")
        mSAPImportParameter = CType(mObjDynSapProxyObjSAP.Import.Select("ElementCode='PARA'")(0)(0), DataTable).Copy
        mSAPExportParameter = CType(mObjDynSapProxyObjSAP.Export.Select("ElementCode='PARA'")(0)(0), DataTable).Copy

        ReDim mSAPImportTabellen(tmpImportTableRows.Count - 1)
        ReDim mSAPExportTabellen(tmpExportTableRows.Count - 1)
        ReDim mSAPTabellen(tmpExportTableRows.Count + tmpImportTableRows.Count)

        'Jede tabelle ins Tabellen Array schreiben.
        i = 0
        For Each tmpRow In tmpImportTableRows
            mSAPImportTabellen(i) = getAStrukturTable
            mSAPImportTabellen(i).TableName = CType(tmpRow(0), DataTable).TableName
            For Each tmpColum As DataColumn In CType(tmpRow(0), DataTable).Columns
                mSAPImportTabellen(i).Rows.Add(tmpColum.ColumnName.ToString, tmpColum.DataType.ToString, tmpColum.MaxLength.ToString)
                mSAPImportTabellen(i).AcceptChanges()
            Next
            i += 1
        Next
        i = 0
        For Each tmpRow In tmpExportTableRows
            mSAPExportTabellen(i) = getAStrukturTable
            mSAPExportTabellen(i).TableName = CType(tmpRow(0), DataTable).TableName
            For Each tmpColum As DataColumn In CType(tmpRow(0), DataTable).Columns
                mSAPExportTabellen(i).Rows.Add(tmpColum.ColumnName.ToString, tmpColum.DataType.ToString, tmpColum.MaxLength.ToString)
                mSAPExportTabellen(i).AcceptChanges()
            Next
            i += 1
        Next

        i = 0
        For Each tmpDatatable As DataTable In If(mSAPExportTabellen.Length > mSAPImportTabellen.Length, mSAPExportTabellen, mSAPImportTabellen)
            If Not tmpDatatable Is Nothing Then
                For Each tmpDatatable2 As DataTable In If(mSAPImportTabellen.Contains(tmpDatatable), mSAPExportTabellen, mSAPImportTabellen)
                    If Not tmpDatatable2 Is Nothing Then
                        If tmpDatatable2.TableName = tmpDatatable.TableName Then
                            SAPTabellenOverview.Rows.Add(tmpDatatable.TableName, tmpDatatable.Rows.Count.ToString)
                            mSAPTabellen(i) = tmpDatatable.Copy

                            If Not Array.IndexOf(mSAPExportTabellen, tmpDatatable) = -1 Then
                                mSAPExportTabellen(Array.IndexOf(mSAPExportTabellen, tmpDatatable)) = Nothing
                            End If
                            If Not Array.IndexOf(mSAPExportTabellen, tmpDatatable2) = -1 Then
                                mSAPExportTabellen(Array.IndexOf(mSAPExportTabellen, tmpDatatable2)) = Nothing
                            End If
                            If Not Array.IndexOf(mSAPImportTabellen, tmpDatatable) = -1 Then
                                mSAPImportTabellen(Array.IndexOf(mSAPImportTabellen, tmpDatatable)) = Nothing
                            End If
                            If Not Array.IndexOf(mSAPImportTabellen, tmpDatatable2) = -1 Then
                                mSAPImportTabellen(Array.IndexOf(mSAPImportTabellen, tmpDatatable2)) = Nothing
                            End If
                            i += 1
                        End If
                    End If

                Next
            End If
        Next


        i = 0
        For Each tmpDataTable As DataTable In mSAPExportTabellen
            If Not tmpDataTable Is Nothing Then
                mSAPExportParameter.Rows.Add(tmpDataTable.TableName, "Tabelle", tmpDataTable.Rows.Count.ToString)
                mSAPExportTabellen(i) = tmpDataTable
                i += 1
            End If
        Next
        ReDim Preserve mSAPExportTabellen(i - 1)

        i = 0
        For Each tmpDataTable As DataTable In mSAPImportTabellen
            If Not tmpDataTable Is Nothing Then
                mSAPImportParameter.Rows.Add(tmpDataTable.TableName, "Tabelle", tmpDataTable.Rows.Count.ToString)
                mSAPImportTabellen(i) = tmpDataTable
                i += 1
            End If
        Next
        ReDim Preserve mSAPImportTabellen(i - 1)


        i = 0
        For Each tmpDataTable As DataTable In mSAPTabellen
            If Not tmpDataTable Is Nothing Then
                mSAPTabellen(i) = tmpDataTable
                i += 1
            End If
        Next
        ReDim Preserve mSAPTabellen(i - 1)
    End Sub

#End Region
End Class
