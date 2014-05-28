Option Explicit On
Option Strict On

Imports System.Runtime.CompilerServices

Friend Module Extensions
    <Extension()>
    Public Function Chunk(ByVal s As String, size As Integer) As IEnumerable(Of String)
        Dim result As New List(Of String)()
        For i As Integer = 0 To s.Length - 1 Step size
            result.Add(s.Substring(i, Math.Min(size, s.Length - i)))
        Next
        Return result.AsReadOnly()
    End Function

    <Extension()>
    Public Function SetFields(dr As DataRow, ParamArray fieldsValues As Object()) As DataRow
        If fieldsValues.Length Mod 2 <> 0 Then
            Throw New InvalidOperationException()
        End If

        For i As Integer = 0 To fieldsValues.Length - 1 Step 2
            If TypeOf fieldsValues(i) Is String Then
                dr.SetField(DirectCast(fieldsValues(i), String), fieldsValues(i + 1))
            ElseIf TypeOf fieldsValues(i) Is Integer Then
                dr.SetField(DirectCast(fieldsValues(i), Integer), fieldsValues(i + 1))
            Else
                Throw New InvalidOperationException()
            End If
        Next

        Return dr
    End Function
End Module

Friend Class CarportBeauftragung
    Private ReadOnly user As CKG.Base.Kernel.Security.User
    Private _kunnr As String
    Private _referenz As String
    Private ReadOnly app As CKG.Base.Kernel.Security.App
    Private ReadOnly page As Web.UI.Page

    Private ReadOnly Property Kunnr As String
        Get
            If String.IsNullOrEmpty(Me._kunnr) Then
                Me._kunnr = Me.user.Customer.KUNNR.PadLeft(10, "0"c)
            End If

            Return Me._kunnr
        End Get
    End Property

    Private ReadOnly Property Referenz As String
        Get
            If String.IsNullOrEmpty(Me._referenz) Then
#If DEBUG Then
                Me._referenz = "1234" 'Referenz?
#Else
                Me._referenz = Me.user.Reference
#End If
            End If

            Return Me._referenz
        End Get
    End Property

    'Private ReadOnly Property Leistungsart As String
    '    Get
    '        Return "UAFZAV"
    '    End Get
    'End Property

    Private ReadOnly Property Eingangsleistungsart As String
        Get
            Return "UAFZEI"
        End Get
    End Property

    Private ReadOnly Property Bereitmeldungsleistungsart As String
        Get
            Return "UAFZBE"
        End Get
    End Property

    Friend Sub New(user As CKG.Base.Kernel.Security.User, _
                   app As CKG.Base.Kernel.Security.App, _
                   page As Web.UI.Page)
        Me.user = user
        Me.app = app
        Me.page = page
    End Sub

    Private Function GetPlangruppenzähler() As String
        Dim auftrdatProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", Me.app, Me.user, Me.page)
        auftrdatProxy.setImportParameter("I_KUNNR", Me.Kunnr)
        auftrdatProxy.setImportParameter("I_KENNUNG", "PROZESSNUMMER")
        auftrdatProxy.setImportParameter("I_POS_KURZTEXT", Me.Referenz)

        auftrdatProxy.callBapi()

        Dim table As DataTable = auftrdatProxy.getExportTable("GT_WEB")

        Return (From row In table.AsEnumerable() _
                Select row.Field(Of String)("POS_TEXT")).SingleOrDefault()
    End Function

    Private Function GetAuftragsabschluss() As String
        Try
            Dim auftrdatProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", Me.app, Me.user, Me.page)
            auftrdatProxy.setImportParameter("I_KUNNR", Me.Kunnr)
            auftrdatProxy.setImportParameter("I_KENNUNG", "CARPORT_ABSCHLUSS")

            auftrdatProxy.callBapi()

            Dim table As DataTable = auftrdatProxy.getExportTable("GT_WEB")

            Return (From row In table.AsEnumerable() _
                    Select row.Field(Of String)("POS_KURZTEXT")).SingleOrDefault()
        Catch e As Exception
            If Not e.Message.Equals("NO_DATA", StringComparison.OrdinalIgnoreCase) Then
                Throw
            End If
        End Try

        Return Nothing
    End Function

    Public Function GetWunschlieferdatum() As String
        Try
            Dim auftrdatProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", Me.app, Me.user, Me.page)
            auftrdatProxy.setImportParameter("I_KUNNR", Me.Kunnr)
            auftrdatProxy.setImportParameter("I_KENNUNG", "WUNSCHLIEFERDATUM")

            auftrdatProxy.callBapi()

            Dim table As DataTable = auftrdatProxy.getExportTable("GT_WEB")

            Return (From row In table.AsEnumerable() _
                    Select row.Field(Of String)("POS_KURZTEXT")).SingleOrDefault()
        Catch e As Exception
            If Not e.Message.Equals("NO_DATA", StringComparison.OrdinalIgnoreCase) Then
                Throw
            End If
        End Try

        Return Nothing
    End Function

    Public Function GetFahrzeugavisierung() As String
        Try
            Dim auftrdatProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", Me.app, Me.user, Me.page)
            auftrdatProxy.setImportParameter("I_KUNNR", Me.Kunnr)
            auftrdatProxy.setImportParameter("I_KENNUNG", "FAHRZEUGAVISIERUNG")

            auftrdatProxy.callBapi()

            Dim table As DataTable = auftrdatProxy.getExportTable("GT_WEB")

            Return (From row In table.AsEnumerable() _
                    Select row.Field(Of String)("POS_KURZTEXT")).SingleOrDefault()
        Catch e As Exception
            If Not e.Message.Equals("NO_DATA", StringComparison.OrdinalIgnoreCase) Then
                Throw
            End If
        End Try

        Return Nothing
    End Function

    Private Function GetCarport() As String
        Dim auftrdatProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", Me.app, Me.user, Me.page)
        auftrdatProxy.setImportParameter("I_KUNNR", Me.Kunnr)
        auftrdatProxy.setImportParameter("I_KENNUNG", "CARPORT")
        auftrdatProxy.setImportParameter("I_POS_KURZTEXT", Me.Referenz)

        auftrdatProxy.callBapi()

        Dim table As DataTable = auftrdatProxy.getExportTable("GT_WEB")

        Return (From row In table.AsEnumerable() _
                Select row.Field(Of String)("POS_TEXT")).SingleOrDefault()
    End Function

    Public Function GetServices() As IEnumerable(Of ListItem)
        Try
            Dim serviceProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_DPM_READ_LV_001", Me.app, Me.user, Me.page)

            serviceProxy.setImportParameter("I_VWAG", "X")

            Dim itable As DataTable = serviceProxy.getImportTable("GT_IN_AG")

            Dim irow As DataRow = itable.NewRow()
            irow.SetField("AG", Me.Kunnr)
            itable.Rows.Add(irow)
            itable.AcceptChanges()

            itable = serviceProxy.getImportTable("GT_IN_PROZESS")

            irow = itable.NewRow()
            irow.SetField("SORT1", 22)
            itable.Rows.Add(irow)
            itable.AcceptChanges()

            serviceProxy.callBapi()

            Dim table As DataTable = serviceProxy.getExportTable("GT_OUT_DL")

            Return From row As DataRow In table.AsEnumerable() _
                              Where Not String.IsNullOrEmpty(row.Field(Of String)("ASNUM")) _
                              Select New ListItem(row.Field(Of String)("ASKTX"), row.Field(Of String)("ASNUM"))
        Catch e As Exception
            If Not e.Message.Equals("NO_DATA", StringComparison.OrdinalIgnoreCase) Then
                Throw
            End If
        End Try

        Return New ListItem() {}
    End Function

    Public Function GetCarports() As IEnumerable(Of ListItem)
        Try
            Dim carportProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_DPM_READ_CARPORTS_02", Me.app, Me.user, Me.page)

            carportProxy.setImportParameter("I_AG", Me.Kunnr)
            carportProxy.callBapi()

            Dim table As DataTable = carportProxy.getExportTable("GT_PDI")

            Return From row As DataRow In table.AsEnumerable() _
                              Select New ListItem(String.Format("{0}{1} - ({2})", row.Field(Of String)("NAME1"), row.Field(Of String)("NAME2"), row.Field(Of String)("KUNPDI")), row.Field(Of String)("KUNPDI"))
        Catch e As Exception
            If Not e.Message.Equals("NO_DATA", StringComparison.OrdinalIgnoreCase) Then
                Throw
            End If
        End Try

        Return New ListItem() {}
    End Function

    Public Function GetLogistikPartner() As IEnumerable(Of ListItem)
        Try
            Dim auftrdatProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", Me.app, Me.user, Me.page)
            auftrdatProxy.setImportParameter("I_KUNNR", Me.Kunnr)
            auftrdatProxy.setImportParameter("I_KENNUNG", "LOGISTIKPARTNER")

            auftrdatProxy.callBapi()

            Dim table As DataTable = auftrdatProxy.getExportTable("GT_WEB")

            Return From row In table.AsEnumerable() _
                    Select New ListItem(row.Field(Of String)("POS_TEXT"))
        Catch e As Exception
            If Not e.Message.Equals("NO_DATA", StringComparison.OrdinalIgnoreCase) Then
                Throw
            End If
        End Try

        Return New ListItem() {}
    End Function

    Public Function GetAuftragsstatus(fahrgestellnummer As IEnumerable(Of String)) As IEnumerable(Of String)
        Try
            Dim aufposProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_DPM_COIH_READ_AUF_POS_02", Me.app, Me.user, Me.page)
            aufposProxy.setImportParameter("I_KUNNR_AG", Me.Kunnr)
            aufposProxy.setImportParameter("I_PLNAL", Me.GetPlangruppenzähler())

            Dim itable As DataTable = aufposProxy.getImportTable("GT_LSTART")
            Dim irow As DataRow = itable.NewRow()

            irow.SetField("LSTAR", GetFahrzeugavisierung())

            itable.Rows.Add(irow)
            itable.AcceptChanges()

            itable = aufposProxy.getImportTable("GT_AUF")

            For Each nummer As String In fahrgestellnummer
                irow = itable.NewRow()

                irow.SetField("CHASSIS_NUM", nummer)

                itable.Rows.Add(irow)
            Next

            itable.AcceptChanges()

            aufposProxy.callBapi()

            Dim table As DataTable = aufposProxy.getExportTable("GT_VORG")

            Dim statuses As String() = (From row In table.AsEnumerable() _
                    Select row.Field(Of String)("ANLZU")).ToArray()

            For i As Integer = 0 To statuses.Length - 1
                If String.IsNullOrEmpty(statuses(i)) Then
                    statuses(i) = "Neu"
                End If
            Next

            Return statuses
        Catch e As Exception
            If Not e.Message.Equals("NO_DATA", StringComparison.OrdinalIgnoreCase) Then
                Throw
            End If
        End Try

        Return Nothing
    End Function



    Private Function GetAuftragszustände() As DataTable
        Dim auftragProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_DPM_COIH_READ_AUFTRAG_001", Me.app, Me.user, Me.page)

        Dim importtable As DataTable = auftragProxy.getImportTable("GS_IN")

        Dim dr As DataRow = importtable.NewRow

        dr("KUNNR_AG") = Me.Kunnr
        dr("AUTYP") = "30"
        ' Geht noch nicht!
        'dr("PLNAL") = pn

        importtable.Rows.Add(dr)
        importtable.AcceptChanges()

        auftragProxy.callBapi()

        Return auftragProxy.getExportTable("GT_AUF_RUECK")
    End Function

    Public Function GetAuftrag(fahrgestellnummer As String) As DataRow
        Dim auftraege = Me.GetAufträge(Nothing, Nothing, "*", fahrgestellnummer, String.Empty, Nothing, Nothing).Rows.Cast(Of DataRow).ToList()

        If auftraege.Count > 1 Then Throw New Exception("Es existieren mehrere Aufträge zur Fahrgestellnummer '" + fahrgestellnummer + "'")

        Return auftraege.SingleOrDefault()
    End Function

    Public Function GetAufträge(datumVon As DateTime?, datumBis As DateTime?, carport As String, _
                                fahrgestellnummer As String, kennzeichen As String, status As String, leistungsart As String) As DataTable
        Try
            Dim aufposProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_DPM_COIH_READ_AUF_POS_02", Me.app, Me.user, Me.page)
            aufposProxy.setImportParameter("I_KUNNR_AG", Me.Kunnr)
            aufposProxy.setImportParameter("I_PLNAL", Me.GetPlangruppenzähler())

            If String.IsNullOrEmpty(carport) Then
                carport = Me.GetCarport()
            End If

            If Not carport.Equals("*", StringComparison.Ordinal) Then
                aufposProxy.setImportParameter("I_ETEXT1", carport)
            End If

            If Not String.IsNullOrEmpty(kennzeichen) Then
                aufposProxy.setImportParameter("I_ETEXT4", kennzeichen)
            End If

            Dim itable As DataTable = aufposProxy.getImportTable("GT_LSTART")
            Dim irow As DataRow = itable.NewRow()

            irow.SetField("LSTAR", GetFahrzeugavisierung())

            itable.Rows.Add(irow)
            itable.AcceptChanges()

            If Not String.IsNullOrEmpty(fahrgestellnummer) Then
                itable = aufposProxy.getImportTable("GT_AUF")
                irow = itable.NewRow()

                irow.SetField("CHASSIS_NUM", fahrgestellnummer)

                itable.Rows.Add(irow)
                itable.AcceptChanges()
            End If

            aufposProxy.callBapi()

            Dim table As DataTable = aufposProxy.getExportTable("GT_VORG")
            table.Columns.Add("TEXTE", GetType(String))
            table.Columns.Add("DIENSTLEISTUNGEN", GetType(String))
            table.Columns.Add("EINGANG", GetType(DateTime))
            table.Columns.Add("BEREITMELDUNG", GetType(DateTime))

            Dim temp As IEnumerable(Of DataRow) = (From dr In table.AsEnumerable() _
                        Where (Not datumVon.HasValue OrElse datumVon.Value <= dr.Field(Of DateTime?)("TERMIN_ANFANG")) _
                        AndAlso (Not datumBis.HasValue OrElse dr.Field(Of DateTime?)("TERMIN_ANFANG") <= datumBis.Value) _
                        Select dr)

            table = aufposProxy.getExportTable("GT_TEXTE")
            Dim table2 = aufposProxy.getExportTable("GT_LEISTUNGEN")
            Dim table3 = Me.GetAuftragszustände()

            temp = From dr In temp _
                   Group Join dr2 In table.AsEnumerable() _
                   On dr.Field(Of String)("AUFNR") Equals dr2.Field(Of String)("AUFNR") And _
                      dr.Field(Of String)("VORNR") Equals dr2.Field(Of String)("VORNR") Into g = Group _
                   Group Join dr3 In table2.AsEnumerable() _
                   On dr.Field(Of String)("AUFNR") Equals dr3.Field(Of String)("AUFNR") And _
                      dr.Field(Of String)("VORNR") Equals dr3.Field(Of String)("VORNR") Into g2 = Group _
                   Group Join dr4 In table3.AsEnumerable().Where(Function(r) r.Field(Of String)("AUERU").Equals("X", StringComparison.Ordinal)) _
                   On dr.Field(Of String)("AUFNR") Equals dr4.Field(Of String)("AUFNR") Into g3 = Group _
                   Let b = g.Aggregate(String.Empty, _
                                       Function(i As String, j As DataRow) As String
                                           Return String.Concat(i, j.Field(Of String)("TDLINE"))
                                       End Function) _
                   Let c = g2.Aggregate(String.Empty, _
                                       Function(i As String, j As DataRow) As String
                                           Return String.Concat(i, If(String.IsNullOrEmpty(i), String.Empty, ", "), j.Field(Of String)("KTEXT1"))
                                       End Function) _
                   Let o = g3.Aggregate(DirectCast(DBNull.Value, Object), _
                                        Function(i As Object, j As DataRow) As Object
                                            Return If(TypeOf i Is DateTime, i, _
                                                      If(j.Field(Of String)("LEARR").Equals(Me.Eingangsleistungsart, StringComparison.Ordinal), _
                                                         DirectCast(j.Field(Of DateTime)("IEDD"), Object), DBNull.Value))
                                        End Function) _
                   Let p = g3.Aggregate(DirectCast(DBNull.Value, Object), _
                                        Function(i As Object, j As DataRow) As Object
                                            Return If(TypeOf i Is DateTime, i, _
                                                      If(j.Field(Of String)("LEARR").Equals(Me.Bereitmeldungsleistungsart, StringComparison.Ordinal), _
                                                         DirectCast(j.Field(Of DateTime)("IEDD"), Object), DBNull.Value))
                                        End Function) _
                   Select dr.SetFields("TEXTE", b, "DIENSTLEISTUNGEN", c, "EINGANG", o, "BEREITMELDUNG", p)

            Return If(temp.Any(), temp.CopyToDataTable(), New DataTable())
        Catch e As Exception
            If Not e.Message.Equals("NO_DATA", StringComparison.OrdinalIgnoreCase) Then
                Throw
            End If
        End Try

        Return New DataTable()
    End Function

    Private Sub ChangeAuftrag(auftragsnummer As String, _
                              fahrgestellnummer As String, _
                              kennzeichen As String, _
                              carport As String, _
                              logistikPartner As String, _
                              bereitstellungsdatum As DateTime?, _
                              bemerkung As String, _
                              dienstleistungen As IEnumerable(Of String))

        Dim changeProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_DPM_COIH_CHANGE_AUF_POS_02", Me.app, Me.user, Me.page)
        changeProxy.setImportParameter("I_KUNNR_AG", Me.Kunnr)
        changeProxy.setImportParameter("I_PLNAL", Me.GetPlangruppenzähler())

        Dim itable As DataTable = changeProxy.getImportTable("GT_VORG")
        Dim irow As DataRow = itable.NewRow()

        irow.SetField("AUFNR", auftragsnummer)
        irow.SetField("CHASSIS_NUM", fahrgestellnummer)
        irow.SetField("LSTAR", GetFahrzeugavisierung())
        irow.SetField("ETEXT1", carport)

        If Not String.IsNullOrEmpty(logistikPartner) Then
            irow.SetField("ETEXT3", logistikPartner)
        End If

        If Not String.IsNullOrEmpty(kennzeichen) Then
            irow.SetField("ETEXT4", kennzeichen)
        End If

        If bereitstellungsdatum.HasValue Then
            irow.SetField("TERMIN_ANFANG", bereitstellungsdatum.Value)
            irow.SetField("TERMIN_ENDE", bereitstellungsdatum.Value)
        End If

        irow.SetField("ANLZU", "B")

        itable.Rows.Add(irow)
        itable.AcceptChanges()

        If Not String.IsNullOrEmpty(bemerkung) Then
            itable = changeProxy.getImportTable("GT_TEXTE")

            For Each chunk As String In bemerkung.Chunk(132)
                irow = itable.NewRow()

                irow.SetField("AUFNR", auftragsnummer)
                irow.SetField("CHASSIS_NUM", fahrgestellnummer)
                irow.SetField("LSTAR", GetFahrzeugavisierung())
                irow.SetField("TDLINE", chunk)

                itable.Rows.Add(irow)
            Next

            itable.AcceptChanges()
        End If

        itable = changeProxy.getImportTable("GT_LEISTUNGEN")

        For Each dienstleistung As String In dienstleistungen
            irow = itable.NewRow()

            irow.SetField("AUFNR", auftragsnummer)
            irow.SetField("CHASSIS_NUM", fahrgestellnummer)
            irow.SetField("LSTAR", GetFahrzeugavisierung())
            irow.SetField("ASNUM", dienstleistung)

            itable.Rows.Add(irow)
        Next

        itable.AcceptChanges()

        changeProxy.callBapi()

        Dim table As DataTable = changeProxy.getExportTable("GT_RETURN")
        Dim e As Integer = table.Rows.Count
    End Sub

    Public Sub SaveRückmeldung(fahrgestellnummer As String, _
                               kennzeichen As String, _
                               carport As String, _
                               logistikPartner As String, _
                               bereitstellungsdatum As DateTime?, _
                               bemerkung As String, _
                               dienstleistungen As IEnumerable(Of String))
        Try
            Dim saveProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_DPM_COIH_SAVE_RUECK_001", Me.app, Me.user, Me.page)

            Dim itable As DataTable = saveProxy.getImportTable("GT_IN_RUECK")
            Dim irow As DataRow = itable.NewRow()

            irow.SetField("KUNNR_AG", Me.Kunnr)
            irow.SetField("PLNAL", Me.GetPlangruppenzähler())
            irow.SetField("LARNT", Me.GetFahrzeugavisierung())
            irow.SetField("CHASSIS_NUM", fahrgestellnummer)
            irow.SetField("AUERU", "X")

            itable.Rows.Add(irow)
            itable.AcceptChanges()

            saveProxy.callBapi()

            Dim table As DataTable = saveProxy.getExportTable("GT_IN_RUECK")
            Dim auftragsnummer As String = (From row In table.AsEnumerable() _
                                            Select row.Field(Of String)("AUFNR")).Single()

            Me.ChangeAuftrag(auftragsnummer, _
                             fahrgestellnummer, _
                             kennzeichen, _
                             carport, _
                             logistikPartner, _
                             bereitstellungsdatum, _
                             bemerkung, _
                             dienstleistungen)

            If GetWunschlieferdatum() IsNot Nothing Then
                itable = saveProxy.getImportTable("GT_IN_RUECK")
                itable.NewRow()

                irow.SetField("KUNNR_AG", Me.Kunnr)
                irow.SetField("PLNAL", Me.GetPlangruppenzähler())
                irow.SetField("LARNT", GetWunschlieferdatum())
                irow.SetField("PEDD", bereitstellungsdatum)
                irow.SetField("CHASSIS_NUM", fahrgestellnummer)

                itable.Rows.Add(irow)
                itable.AcceptChanges()

                saveProxy.callBapi()

            End If


        Catch e As Exception
            If Not e.Message.Equals("NO_DATA", StringComparison.OrdinalIgnoreCase) Then
                Throw
            End If
        End Try
    End Sub

    Public Sub Eingang(auftragsnummer As IEnumerable(Of Pair))
        Me.ChangeLeistungsart(auftragsnummer, Me.Eingangsleistungsart)
    End Sub

    Public Sub Bereitmeldung(auftragsnummer As IEnumerable(Of Pair))
        Me.ChangeLeistungsart(auftragsnummer, Me.Bereitmeldungsleistungsart)
    End Sub

    Public Sub Ausgang(auftragsnummer As IEnumerable(Of Pair))
        Me.ChangeLeistungsart(auftragsnummer, "UAFZAU")
    End Sub

    Private Sub ChangeLeistungsart(auftragsnummer As IEnumerable(Of Pair), newLeistungsart As String)
        Try
            Dim saveProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_DPM_COIH_SAVE_RUECK_001", Me.app, Me.user, Me.page)
            Dim abschlussProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_DPM_COIH_TECH_ABSCHL_001", Me.app, Me.user, Me.page)

            Dim importtable As DataTable = saveProxy.getImportTable("GT_IN_RUECK")
            Dim importtable2 As DataTable = abschlussProxy.getImportTable("GT_IN")

            Dim pn As String = Me.GetPlangruppenzähler()
            Dim aa As Boolean = newLeistungsart.Equals(Me.GetAuftragsabschluss(), StringComparison.Ordinal)

            For Each nummer In auftragsnummer
                Dim irow As DataRow = importtable.NewRow()

                irow.SetField("KUNNR_AG", Me.Kunnr)
                irow.SetField("PLNAL", pn)
                irow.SetField("LARNT", newLeistungsart)
                irow.SetField("AUFNR", nummer.First)
                irow.SetField("AUERU", "X")

                importtable.Rows.Add(irow)

                If aa Then
                    Dim importrow2 As DataRow = importtable2.NewRow()

                    importrow2.SetField("KUNNR_AG", Me.Kunnr)
                    importrow2.SetField("CHASSIS_NUM", nummer.Second)

                    importtable2.Rows.Add(importrow2)
                End If
            Next

            importtable.AcceptChanges()

            saveProxy.callBapi()

            If importtable2.Rows.Count > 0 Then
                importtable2.AcceptChanges()

                abschlussProxy.setImportParameter("I_REPID", "ZABSCHLUSS")
                abschlussProxy.setImportParameter("I_AUART", "ZS02")
                abschlussProxy.setImportParameter("I_PLNAL", pn)
                abschlussProxy.setImportParameter("I_TECHN_AB", "X")

                abschlussProxy.callBapi()

                Dim resulttable2 As DataTable = abschlussProxy.getExportTable("GT_RET")
            End If
        Catch e As Exception
            If Not e.Message.Equals("NO_DATA", StringComparison.OrdinalIgnoreCase) Then
                Throw
            End If
        End Try
    End Sub
End Class
