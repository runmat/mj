Option Strict On
Option Explicit On

Public Class Vertragsbestand
    Inherits CKG.Base.Business.DatenimportBase

    Public Property Kontonummer As String
    Public Property CIN As String
    Public Property PaidNummer As String
    Public Property Name As String
    Public Property Vertragsart As String

    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, objApp As CKG.Base.Kernel.Security.App, strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Sub GetVertragsbestand(strAppID As String, strSessionID As String, page As Page, Optional reaktivierung As Boolean = False)
        m_strClassAndMethod = "Vertragsbestand.GetVertragsbestand"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = String.Empty

        Try
            Dim myProxy As CKG.Base.Common.DynSapProxyObj = CKG.Base.Common.DynSapProxy.getProxy("Z_DPM_VERTRAGSBESTAND_01", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, "0"c))
            Dim findAll As Boolean = True

            If Not String.IsNullOrEmpty(Me.Kontonummer) Then
                findAll = False
                myProxy.setImportParameter("I_KONTONR", Me.Kontonummer)
            End If

            If Not String.IsNullOrEmpty(Me.CIN) Then
                findAll = False
                myProxy.setImportParameter("I_CIN", Me.CIN)
            End If

            If Not String.IsNullOrEmpty(Me.PaidNummer) Then
                findAll = False
                myProxy.setImportParameter("I_PAID", Me.PaidNummer)
            End If

            If Not String.IsNullOrEmpty(Me.Name) Then
                myProxy.setImportParameter("I_NAME", "*" & Me.Name & "*")
            ElseIf findAll Then
                myProxy.setImportParameter("I_NAME", "*")
            End If

            If Not String.IsNullOrEmpty(Me.Vertragsart) AndAlso Not Me.Vertragsart.ToUpper = "ALLE" Then
                myProxy.setImportParameter("I_VERT_ART", Me.Vertragsart)
            End If

            myProxy.callBapi()

            ResultTable = myProxy.getExportTable("GT_OUT")

            If reaktivierung Then

                'ResultTable.DefaultView.RowFilter = "(ISNULL(DAT_VERTR_REAKT, '01.01.0001') = '01.01.0001') AND NOT (ISNULL(ENDG_VERS, '01.01.0001') = '01.01.0001')"
                ' MJE, 31.03.2013, ITA 6198: 
                '  Ummappen von "ENDG_VERS" auf "DAT_VERTR_ENDE"
                ResultTable.DefaultView.RowFilter = "(ISNULL(DAT_VERTR_REAKT, '01.01.0001') = '01.01.0001') AND NOT (ISNULL(DAT_VERTR_ENDE, '01.01.0001') = '01.01.0001')"

                ResultTable = ResultTable.DefaultView.ToTable()
            End If

            ResultTable.Columns.Add("Fahrzeugwertverlauf", GetType(DataTable))
            ResultTable.Columns.Add("MahnstufeCoC", GetType(Integer))
            ResultTable.Columns.Add("MahndatumCoC", GetType(DateTime))
            ResultTable.Columns.Add("MahnstufeZBII", GetType(Integer))
            ResultTable.Columns.Add("MahndatumZBII", GetType(DateTime))
            ResultTable.Columns.Add("MahnstufeSÜ", GetType(Integer))
            ResultTable.Columns.Add("MahndatumSÜ", GetType(DateTime))

            ResultTable.Columns.Add("NotizAvailable", GetType(Boolean))
            ResultTable.Columns.Add("Notizen", GetType(DataTable))
            'ResultTable.Columns.Add("Notiz", GetType(String))
            'ResultTable.Columns.Add("NotizDatum", GetType(DateTime))
            'ResultTable.Columns.Add("NotizUser", GetType(String))
            'ResultTable.Columns.Add("NotizHerkunft", GetType(String))
            'ResultTable.Columns.Add("NotizPartner", GetType(String))

            FormatTableData(ResultTable)
            FormatNotiz(ResultTable, myProxy.getExportTable("GT_NOTIZ"))

            ResultTable.AcceptChanges()

            WriteLogEntry(True, "KUNNR=" + m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -1111
            Me.ResultTable = New DataTable()
            m_strMessage = "Keine Informationen gefunden."
        End Try
    End Sub

    Private Sub FormatNotiz(dt As DataTable, dtNotiz As DataTable)
        For Each tmprow As DataRow In dt.Rows

            Dim tempTable As New DataTable
            tempTable.Columns.Add("Datum", GetType(DateTime))
            tempTable.Columns.Add("User", GetType(String))
            tempTable.Columns.Add("Herkunft", GetType(String))
            tempTable.Columns.Add("Gespraechspartner", GetType(String))
            tempTable.Columns.Add("Notiz", GetType(String))

            Dim notizen As DataRow() = dtNotiz.Select("KONTONR = '" & tmprow("KONTONR").ToString() & "' AND PAID = '" & tmprow("PAID").ToString() & "' AND CIN = '" & tmprow("CIN").ToString() & "'")

            If (notizen.Count = 0) Then
                tmprow("NotizAvailable") = False
            Else
                tmprow("NotizAvailable") = True

                For Each notizRow As DataRow In notizen
                    Dim newNotizRow As DataRow = tempTable.NewRow
                    Dim dtDatum As DateTime = CType(notizRow("DATUM"), DateTime)
                    Dim strUhrzeit As String = notizRow("UHRZEIT").ToString()
                    If Not String.IsNullOrEmpty(strUhrzeit) AndAlso strUhrzeit.Length = 6 Then
                        dtDatum = dtDatum.AddHours(Double.Parse(strUhrzeit.Substring(0, 2)))
                        dtDatum = dtDatum.AddMinutes(Double.Parse(strUhrzeit.Substring(2, 2)))
                        dtDatum = dtDatum.AddSeconds(Double.Parse(strUhrzeit.Substring(4, 2)))
                    End If
                    newNotizRow("Datum") = dtDatum
                    newNotizRow("User") = notizRow("USERNAME").ToString()
                    newNotizRow("Herkunft") = notizRow("HERKUNFT_D_NOTIZ").ToString()
                    newNotizRow("Gespraechspartner") = notizRow("GESPRAECHSPARTNER").ToString()
                    newNotizRow("Notiz") = notizRow("GESPRAECHSNOTIZ").ToString()
                    tempTable.Rows.Add(newNotizRow)
                Next
            End If

            tmprow("Notizen") = tempTable
        Next
    End Sub

    'Private Sub FormatNotiz(dt As DataTable, dtNotiz As DataTable)

    '    For Each tmprow As DataRow In dt.Rows

    '        If (dtNotiz.Rows.Count = 0) Then
    '            tmprow("NotizAvailable") = False
    '            tmprow("Notiz") = ""
    '        Else

    '            Dim notiz As String = ""
    '            For Each notizRow As DataRow In dtNotiz.Rows
    '                tmprow("NotizAvailable") = True
    '                notiz = notiz & " " & notizRow("GESPRAECHSNOTIZ").ToString

    '                tmprow("NotizDatum") = notizRow("DATUM")
    '                tmprow("NotizUser") = notizRow("USERNAME").ToString
    '                tmprow("NotizHerkunft") = notizRow("HERKUNFT_D_NOTIZ").ToString
    '                tmprow("NotizPartner") = notizRow("GESPRAECHSPARTNER").ToString
    '            Next
    '            tmprow("Notiz") = notiz

    '        End If
    '    Next
    'End Sub

    Private Sub FormatTableData(dt As DataTable)
        For Each tmprow As DataRow In dt.Rows

            Select Case tmprow("GRUND_ABWEICH").ToString
                Case "00"
                    tmprow("GRUND_ABWEICH") = ""
                Case Else
            End Select

            'tmprow("Notiz") = "Walter Zabel<br /><br />ist <i>ganz</i> ok!!"

        Next
    End Sub

    Public Sub GetWertverlauf(strAppID As String, strSessionID As String, page As Page, intIndex As Integer)
        m_strClassAndMethod = "Vertragsbestand.GetWertverlauf"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = String.Empty

        Dim r As DataRow = ResultTable.Rows(intIndex)

        If DBNull.Value.Equals(r("Fahrzeugwertverlauf")) Then
            Try
                Dim myProxy As CKG.Base.Common.DynSapProxyObj = CKG.Base.Common.DynSapProxy.getProxy("Z_DPM_READ_RETAIL_POS_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, "0"c))
                myProxy.setImportParameter("I_KONTONR", r.Field(Of String)("KONTONR"))
                myProxy.setImportParameter("I_CIN", r.Field(Of String)("CIN"))
                myProxy.setImportParameter("I_PAID", r.Field(Of String)("PAID"))

                myProxy.callBapi()

                Dim tmpTable As DataTable = myProxy.getExportTable("GT_OUT")

                r.SetField("Fahrzeugwertverlauf", tmpTable)
                ResultTable.AcceptChanges()

                WriteLogEntry(True, "KUNNR=" + m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -1111
                r.SetField("Fahrzeugwertverlauf", New DataTable())
                m_strMessage = "Keine Informationen gefunden."
            End Try
        End If
    End Sub

    Public Sub GetMahnstufen(strAppID As String, strSessionID As String, page As Page, intIndex As Integer)
        m_strClassAndMethod = "Vertragsbestand.GetMahnstufen"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = String.Empty

        Dim r As DataRow = ResultTable.Rows(intIndex)

        If DBNull.Value.Equals(r("MahnstufeCoC")) Then
            Try
                Dim myProxy As CKG.Base.Common.DynSapProxyObj = CKG.Base.Common.DynSapProxy.getProxy("Z_DPM_READ_MAHN_EQSTL_01", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, "0"c))
                myProxy.setImportParameter("I_VORG_ART", "D")
                myProxy.setImportParameter("I_CHASSIS_NUM", r.Field(Of String)("PAID"))
                myProxy.setImportParameter("I_NUR_OFFENE", "X")

                myProxy.callBapi()

                Dim tmpTable As DataTable = myProxy.getExportTable("GT_OUT")

                Dim pair As Pair = Vertragsbestand.ExtractMahnstufe(tmpTable, 720)
                r.SetField("MahnstufeCoC", pair.First)
                r.SetField("MahndatumCoC", pair.Second)
                pair = Vertragsbestand.ExtractMahnstufe(tmpTable, 722)
                r.SetField("MahnstufeZBII", pair.First)
                r.SetField("MahndatumZBII", pair.Second)
                pair = Vertragsbestand.ExtractMahnstufe(tmpTable, 725)
                r.SetField("MahnstufeSÜ", pair.First)
                r.SetField("MahndatumSÜ", pair.Second)

                ResultTable.AcceptChanges()

                WriteLogEntry(True, "KUNNR=" + m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -1111
                r.SetField("MahnstufeCoC", 0)
                r.SetField(Of Object)("MahndatumCoC", Nothing)
                r.SetField("MahnstufeZBII", 0)
                r.SetField(Of Object)("MahndatumZBII", Nothing)
                r.SetField("MahnstufeSÜ", 0)
                r.SetField(Of Object)("MahndatumSÜ", Nothing)
                m_strMessage = "Keine Informationen gefunden."
            End Try
        End If
    End Sub

    Private Function GetKundenLänder(page As Page) As DataTable
        Try
            Dim sapProxy = CKG.Base.Common.DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", m_objApp, m_objUser, page)
            sapProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, "0"c).ToString())
            sapProxy.setImportParameter("I_KENNUNG", "Z_WEB_UEB_LAND")

            sapProxy.callBapi()

            Dim result = sapProxy.getExportTable("GT_WEB")
            Return result
        Catch ex As Exception
            If Not CKG.Base.Business.HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Equals("NO_DATA", StringComparison.Ordinal) Then
                m_strMessage = "Unbekannter Fehler."
                m_intStatus = -9999
            End If
            End Try
        Return Nothing
    End Function

    Public Function GetLänder(page As Page) As DataTable
        Try
            Dim sapProxy = CKG.Base.Common.DynSapProxy.getProxy("Z_M_LAND_PLZ_001", m_objApp, m_objUser, page)
            sapProxy.callBapi()

            Dim result = sapProxy.getExportTable("GT_WEB")

            ' Get customer specific countries
            Dim customerCountries = Me.GetKundenLänder(page)

            If (customerCountries IsNot Nothing AndAlso customerCountries.Rows.Count > 0) Then
                Dim newResult = result.Clone()

                For Each row As DataRow In customerCountries.Rows
                    Dim countryRows = result.Select(String.Format("LAND1 = '{0}'", row.Field(Of String)("POS_KURZTEXT")))

                    If (countryRows.Length > 0) Then
                        Dim countryRow = countryRows(0)
                        Dim newRow = newResult.NewRow()

                        newRow.SetField(0, countryRow.Field(Of String)(0))
                        newRow.SetField(1, countryRow.Field(Of String)(1))
                        newRow.SetField(2, countryRow.Field(Of String)(2))
                        newRow.SetField(3, countryRow.Field(Of String)(3))

                        newResult.Rows.Add(newRow)
                    End If
                Next

                newResult.AcceptChanges()
                result = newResult
            End If

            Return result
        Catch ex As Exception
            If (CKG.Base.Business.HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Equals("ERR_INV_PLZ", StringComparison.Ordinal)) Then
                m_strMessage = "Ungültige Postleitzahl."
                m_intStatus = -1118
            Else
                m_strMessage = "Unbekannter Fehler."
                m_intStatus = -9999
            End If
        End Try

        Return Nothing
    End Function

    Private Shared Function ExtractMahnstufe(mahntabelle As DataTable, matnr As Integer) As Pair
        Dim dr As DataRow = mahntabelle.Select(String.Format("MATNR = '000000000000000{0}'", matnr)).FirstOrDefault()
        Dim mahnstufe As Integer
        Dim mahndatum As DateTime? = Nothing

        If dr Is Nothing Then
            mahnstufe = 0
        Else
            mahnstufe = CInt(dr.Field(Of String)("ZZMAHNS"))

            Select Case mahnstufe
                Case 1
                    mahndatum = dr.Field(Of DateTime)("ZZMADAT_1")
                    Exit Select
                Case 2
                    mahndatum = dr.Field(Of DateTime)("ZZMADAT_2")
                    Exit Select
                Case 3
                    mahndatum = dr.Field(Of DateTime)("ZZMADAT_3")
                    Exit Select
            End Select
        End If

        Return New Pair(mahnstufe, mahndatum)
    End Function

    Public Sub SetReaktivierung(strAppID As String, strSessionID As String, page As Page, intIndex As Integer)
        m_strClassAndMethod = "Vertragsbestand.SetReaktivierung"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = String.Empty

        Dim r As DataRow = ResultTable.Rows(intIndex)

        Try
            Dim myProxy As CKG.Base.Common.DynSapProxyObj = CKG.Base.Common.DynSapProxy.getProxy("Z_DPM_SAVE_REAKTIVIERG_01", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, "0"c))
            myProxy.setImportParameter("I_WEB_USER", m_objUser.UserName)
            Dim table As DataTable = myProxy.getImportTable("GT_IN")

            Dim row As DataRow = table.NewRow()
            row.SetField("KONTONR", r.Field(Of String)("KONTONR"))
            row.SetField("CIN", r.Field(Of String)("CIN"))
            row.SetField("PAID", r.Field(Of String)("PAID"))
            table.Rows.Add(row)
            table.AcceptChanges()

            myProxy.callBapi()

            WriteLogEntry(True, "KUNNR=" + m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -1111
            m_strMessage = "Reaktivierung fehlgeschlagen."
        End Try

    End Sub

    Public Sub SetAbweichendeAdresse(strAppID As String, strSessionID As String, page As Page, intIndex As Integer, _
                                     title As String, name As String, street As String, postcode As String, city As String, country As String)
        m_strClassAndMethod = "Vertragsbestand.SetAbweichendeAdresse"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        m_strMessage = String.Empty

        Dim r As DataRow = ResultTable.Rows(intIndex)

        Try
            Dim myProxy As CKG.Base.Common.DynSapProxyObj = CKG.Base.Common.DynSapProxy.getProxy("Z_DPM_IMP_ADR_01", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, "0"c))
            Dim table As DataTable = myProxy.getImportTable("GT_IN")

            Dim row As DataRow = table.NewRow()
            row.SetField("KONTONR", r.Field(Of String)("KONTONR"))
            row.SetField("CIN", r.Field(Of String)("CIN"))
            row.SetField("PAID", r.Field(Of String)("PAID"))
            row.SetField("ADR_ART", "ABV")
            row.SetField("ANREDE", title)
            row.SetField("NAME", name)
            row.SetField("STREET", street)
            row.SetField("POST_CODE1", postcode)
            row.SetField("CITY1", city)
            row.SetField("COUNTRY", country)
            table.Rows.Add(row)
            table.AcceptChanges()

            myProxy.callBapi()

            WriteLogEntry(True, "KUNNR=" + m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -1111
            m_strMessage = "Adressesetzen fehlgeschlagen."
        End Try

    End Sub
End Class

