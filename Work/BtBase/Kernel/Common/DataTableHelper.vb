Option Strict On

Namespace Kernel.Common

    Public Class DataTableHelper

        '------------------
        'Erzeugt ein neues DataSet und übersetzt dabei die Spalten
        'Ggf. werden Datumsfelder geparst und als DateTime gespeichert
        '------------------
        Public Shared Function TranslateDataColumns(ByVal app As Base.Kernel.Security.App, ByVal sourceTable As DataTable, ByVal appIdForColumnTranslations As String) As DataTable

            Dim resultTable As DataTable

            Dim datatablerow As DataRow
            Dim tbTranslation As New DataTable()

            tbTranslation = app.ColumnTranslation(appIdForColumnTranslations)
            Dim rowsTranslation As DataRow()
            rowsTranslation = tbTranslation.Select("", "DisplayOrder")

            resultTable = New DataTable("ResultsSAP")   'm_tblResult kommt aus der Klasse ReportBase...
            For Each datatablerow In rowsTranslation
                Dim datatablecolumn As DataColumn
                For Each datatablecolumn In sourceTable.Columns
                    If datatablecolumn.ColumnName.ToUpper = datatablerow("OrgName").ToString.ToUpper Then
                        If CType(datatablerow("istdatum"), System.Boolean) = True Then
                            resultTable.Columns.Add(Replace(datatablerow("newname").ToString, ".", ""), GetType(DateTime))
                        Else
                            resultTable.Columns.Add(Replace(datatablerow("newname").ToString, ".", ""), datatablecolumn.datatype)
                        End If
                        resultTable.Columns(resultTable.Columns.Count - 1).ExtendedProperties.Add("Alignment", datatablerow("Alignment").ToString)
                        resultTable.Columns(resultTable.Columns.Count - 1).ExtendedProperties.Add("HeadLine", datatablerow("NewName").ToString)
                    End If
                Next
            Next

            Dim rowResult As DataRow
            For Each rowResult In sourceTable.Rows
                Dim rowNew As DataRow
                rowNew = resultTable.NewRow

                For Each datatablerow In rowsTranslation
                    Dim datatablecolumn As DataColumn
                    For Each datatablecolumn In sourceTable.Columns
                        If datatablecolumn.ColumnName.ToUpper = datatablerow("OrgName").ToString.ToUpper Then
                            If CType(datatablerow("NullenEntfernen"), System.Boolean) = True Then
                                rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = rowResult(datatablerow("OrgName").ToString).ToString.TrimStart("0"c)
                            ElseIf CType(datatablerow("TextBereinigen"), System.Boolean) = True Then
                                rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = Replace(rowResult(datatablerow("OrgName").ToString).ToString, "<(>&<)>", "und")
                            ElseIf CType(datatablerow("IstDatum"), System.Boolean) = True Then
                                '----
                                'Datum
                                '----
                                Dim strTempDate As String = Right(rowResult(datatablerow("OrgName").ToString).ToString, 2) & "." & Mid(rowResult(datatablerow("OrgName").ToString).ToString, 5, 2) & "." & Left(rowResult(datatablerow("OrgName").ToString).ToString, 4)
                                If IsDate(strTempDate) Then
                                    rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = Date.Parse(strTempDate)
                                Else
                                    rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = Date.MinValue
                                End If

                            ElseIf CType(datatablerow("ABEDaten"), System.Boolean) = True Then
                                rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = "<a href=""../Shared/Change06_3.aspx?EqNr=" & rowResult(datatablerow("OrgName").ToString).ToString & """ Target=""_blank"">Anzeige</a>"
                            Else
                                If TypeOf rowResult(datatablerow("OrgName").ToString) Is String Then
                                    rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = rowResult(datatablerow("OrgName").ToString).ToString.Trim(" "c)
                                Else
                                    rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = rowResult(datatablerow("OrgName").ToString)
                                End If
                            End If
                        End If
                    Next
                Next
                resultTable.Rows.Add(rowNew)

            Next

            Return resultTable

        End Function

        ''' <summary>
        ''' Übersetzt die Spaltenüberschriften und Spaltentypen einer Tabelle
        ''' </summary>
        ''' <param name="table">Datentabelle die übersetzt werden soll</param>
        ''' <param name="AppId">ID der Anwendung unter der die zuverwendende Übersetzung gepflegt ist</param>
        ''' <param name="app">Anwendungsobjekt das die ColumnTranslation bereitstellt</param>
        Public Shared Function TranslateDataColumns(ByVal table As DataTable, ByVal AppId As String, ByVal app As Base.Kernel.Security.App) As DataTable
            Dim tbTranslation As DataTable = app.ColumnTranslation(AppId)
            Dim rowsTranslation As DataRow() = tbTranslation.Select("", "DisplayOrder")

            table.TableName = "ResultsSAP"

            For Each tblRow As DataRow In rowsTranslation
                For Each tblColumn As DataColumn In table.Columns
                    If tblColumn.ColumnName.ToUpper = tblRow("OrgName").ToString.ToUpper Then
                        If CType(tblRow("IstDatum"), System.Boolean) = True Then
                            tblColumn.DataType = Type.GetType("System.DateTime")
                        ElseIf CType(tblRow("IstZeit"), System.Boolean) = True Then
                            tblColumn.DataType = Type.GetType("System.String")
                        End If
                        tblColumn.ColumnName = Replace(tblRow("NewName").ToString, ".", "")

                        tblColumn.ExtendedProperties.Add("Alignment", tblRow("Alignment").ToString)
                        tblColumn.ExtendedProperties.Add("HeadLine", tblRow("NewName").ToString)
                        Exit For
                    End If
                Next
            Next

            Return table
        End Function

        '------------------
        'Wandelt ein Object in ein DataTable um
        'Alle öffentlichen Member werden gelesen
        '------------------
        Public Shared Function ObjectToDataTable(ByVal obj As Object) As DataTable

            Dim result As New DataTable()
            result.Rows.Add(result.NewRow()) 'Leere Row anlegen, wird später gefüllt

            Dim pi As Reflection.PropertyInfo
            For Each pi In obj.GetType.GetProperties()

                If pi.CanRead AndAlso (pi.PropertyType.IsPrimitive OrElse pi.PropertyType Is GetType(String) OrElse pi.PropertyType Is GetType(DateTime)) Then

                    'Spalte anlegen
                    result.Columns.Add(pi.Name, pi.PropertyType)

                    'Wert setzen
                    result.Rows(0)(pi.Name) = pi.GetValue(obj, Nothing)

                End If
            Next

            Return result
        End Function

    End Class

End Namespace

' ************************************************
' $History: DataTableHelper.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Common
' 
' *****************  Version 2  *****************
' User: Uha          Date: 15.05.07   Time: 15:29
' Updated in $/CKG/Base/Base/Kernel/Common
' Änderungen aus StartApplication vom 11.05.2007
' 
' ************************************************
