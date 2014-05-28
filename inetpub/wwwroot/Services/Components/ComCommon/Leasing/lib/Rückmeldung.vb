Option Explicit On
Option Strict On

Public NotInheritable Class Leistungsart
    Private ReadOnly _externeKennung As String
    Private ReadOnly _interneKennung As String
    Private ReadOnly _leistungsart As String

    Public ReadOnly Property ExterneKennung As String
        Get
            Return Me._externeKennung
        End Get
    End Property

    Public ReadOnly Property InterneKennung As String
        Get
            Return Me._interneKennung
        End Get
    End Property

    Public ReadOnly Property Leistungsart As String
        Get
            Return Me._leistungsart
        End Get
    End Property

    Public Sub New(externeKennung As String, interneKennung As String, leistungsart As String)
        Me._externeKennung = externeKennung
        Me._interneKennung = interneKennung
        Me._leistungsart = leistungsart
    End Sub
End Class

Public NotInheritable Class Rückmeldungsdetails
    Private ReadOnly _fahrgestellnummer As String
    Private ReadOnly _abgeschlossen As Boolean
    Private ReadOnly _leistungsartIntern As String
    Private ReadOnly _endrückmeldung As Boolean

    Public ReadOnly Property Fahrgestellnummer As String
        Get
            Return Me._fahrgestellnummer
        End Get
    End Property

    Public ReadOnly Property Abgeschlossen As Boolean
        Get
            Return Me._abgeschlossen
        End Get
    End Property

    Public ReadOnly Property LeistungsartIntern As String
        Get
            Return Me._leistungsartIntern
        End Get
    End Property

    Public ReadOnly Property Endrückmeldung As Boolean
        Get
            Return Me._endrückmeldung
        End Get
    End Property

    Public Sub New(fahrgestellnummer As String, abgeschlossen As Boolean, leistungsartIntern As String, endrückmeldung As Boolean)
        Me._fahrgestellnummer = fahrgestellnummer
        Me._abgeschlossen = abgeschlossen
        Me._leistungsartIntern = leistungsartIntern
        Me._endrückmeldung = endrückmeldung
    End Sub
End Class

Public NotInheritable Class Rückmeldungsfehler
    Private ReadOnly _fahrgestellnummer As String
    Private ReadOnly _leistungsartIntern As String
    Private ReadOnly _meldungstyp As String
    Private ReadOnly _meldung As String

    Public ReadOnly Property Fahrgestellnummer As String
        Get
            Return Me._fahrgestellnummer
        End Get
    End Property

    Public ReadOnly Property LeistungsartIntern As String
        Get
            Return Me._leistungsartIntern
        End Get
    End Property

    Public ReadOnly Property Meldungstyp As String
        Get
            Return Me._meldungstyp
        End Get
    End Property

    Public ReadOnly Property Meldung As String
        Get
            Return Me._meldung
        End Get
    End Property

    Public Sub New(fahrgestellnummer As String, leistungsartIntern As String, meldungstyp As String, meldung As String)
        Me._fahrgestellnummer = fahrgestellnummer
        Me._leistungsartIntern = leistungsartIntern
        Me._meldungstyp = meldungstyp
        Me._meldung = meldung
    End Sub

End Class

Friend Enum Status
    [New]
    [Error]
    Ready
    Saved
End Enum

Friend Class Rückmeldung
    Private ReadOnly user As CKG.Base.Kernel.Security.User
    Private _kunnr As String
    Private _referenz As String
    Private ReadOnly app As CKG.Base.Kernel.Security.App
    Private ReadOnly page As Web.UI.Page

    Private ReadOnly Property Kunnr As String
        Get
            If String.IsNullOrEmpty(Me._kunnr) Then
                Me._kunnr = Right("0000000000" & Me.user.Customer.KUNNR, 10)
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
            auftrdatProxy.setImportParameter("I_KENNUNG", "LA_AUFTRAGSABSCHLUSS")

            auftrdatProxy.callBapi()

            Dim table As DataTable = auftrdatProxy.getExportTable("GT_WEB")

            Return (From row In table.AsEnumerable() _
                    Where row.Field(Of String)("POS_TEXT") = Me.Referenz _
                    Select row.Field(Of String)("POS_KURZTEXT")).SingleOrDefault()
        Catch e As Exception
            If Not e.Message.Equals("NO_DATA", StringComparison.OrdinalIgnoreCase) Then
                Throw
            End If
        End Try

        Return Nothing
    End Function

    Friend Function GetBerechtigteLeistungsarten() As IEnumerable(Of Leistungsart)
        Try
            Dim auftrdatProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", Me.app, Me.user, Me.page)

            auftrdatProxy.setImportParameter("I_KUNNR", Me.Kunnr)
            auftrdatProxy.setImportParameter("I_KENNUNG", "LEISTUNGSARTEN")

            auftrdatProxy.callBapi()

            Dim table As DataTable = auftrdatProxy.getExportTable("GT_WEB")
            table.DefaultView.RowFilter = "POS_TEXT = " & Me.Referenz
            table = table.DefaultView.ToTable()

            Dim pn As String = Me.GetPlangruppenzähler()

            If Not String.IsNullOrEmpty(pn) Then
                Dim auftragProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_DPM_COIH_READ_AUFTRAG_001", Me.app, Me.user, Me.page)

                Dim importtable As DataTable = auftragProxy.getImportTable("GS_IN")

                Dim dr As DataRow = importtable.NewRow

                dr.SetField("KUNNR_AG", Me.Kunnr)
                dr.SetField("AUTYP", "30")
                dr.SetField("NUR_AP", "X")
                ' Geht noch nicht!
                'dr.SetField("PLNAL", pn)

                importtable.Rows.Add(dr)
                importtable.AcceptChanges()

                auftragProxy.callBapi()

                Dim ap As DataTable = auftragProxy.getExportTable("GT_AP")

                Dim f = From t In table.AsEnumerable() _
                        Join a In ap.AsEnumerable() On t.Field(Of String)("POS_KURZTEXT") Equals a.Field(Of String)("LARNT") _
                        Select New Leistungsart(t.Field(Of String)("NAME1"), _
                                                t.Field(Of String)("POS_KURZTEXT"), _
                                                a.Field(Of String)("LTXA1"))

                Return f.ToList().AsReadOnly()
            End If
        Catch e As Exception
        End Try

        Return Enumerable.Empty(Of Leistungsart)()
    End Function

    Friend Function SaveRückmeldungen(daten As IEnumerable(Of DataRow)) As IEnumerable(Of Rückmeldungsfehler)
        Try
            Dim saveProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_DPM_COIH_SAVE_RUECK_001", Me.app, Me.user, Me.page)
            Dim abschlussProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_DPM_COIH_TECH_ABSCHL_001", Me.app, Me.user, Me.page)

            Dim importtable As DataTable = saveProxy.getImportTable("GT_IN_RUECK")
            Dim importtable2 As DataTable = abschlussProxy.getImportTable("GT_IN")

            Dim pn As String = Me.GetPlangruppenzähler()
            Dim aa As String = Me.GetAuftragsabschluss()

            For Each dr As DataRow In daten
                Dim importrow As DataRow = importtable.NewRow()

                importrow.SetField("KUNNR_AG", Me.Kunnr)
                importrow.SetField("AUART", "ZS02")
                importrow.SetField("PLNAL", pn)

                Dim fn As String = dr.Field(Of String)("Fahrgestellnummer")
                importrow.SetField("CHASSIS_NUM", fn)

                Dim la As String = dr.Field(Of String)("LeistungsartIntern")
                importrow.SetField("LARNT", la)

                importrow.SetField("IEDD", DateTime.Today.ToShortDateString())
                importrow.SetField("LTXA1", dr.Field(Of String)("Bemerkungstext"))

                Dim aueru As String = dr.Field(Of String)("Endrückmeldung")
                aueru = If(aueru = "E", "X", Nothing)
                importrow.SetField("AUERU", aueru)
                importrow.SetField("PEDD", dr.Field(Of DateTime?)("PrognoseArbeitsende"))

                importtable.Rows.Add(importrow)

                If la.Equals(aa, StringComparison.Ordinal) Then
                    Dim importrow2 As DataRow = importtable2.NewRow()

                    importrow2.SetField("KUNNR_AG", Me.Kunnr)
                    importrow2.SetField("CHASSIS_NUM", fn)

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

            Dim resulttable As DataTable = saveProxy.getExportTable("GT_RET")

            Dim x As IEnumerable(Of Rückmeldungsfehler) = From r In resulttable.AsEnumerable() _
                                    Select New Rückmeldungsfehler( _
                                        r.Field(Of String)("CHASSIS_NUM"), _
                                        r.Field(Of String)("LARNT"), _
                                        r.Field(Of String)("TYPE"), _
                                        r.Field(Of String)("BEMERKUNG"))

            Return x.ToList().AsReadOnly()
        Catch e As Exception
        End Try

        Return Enumerable.Empty(Of Rückmeldungsfehler)()
    End Function

    Friend Function GetRückmeldungsdetails(idents As IEnumerable(Of String)) As IEnumerable(Of Rückmeldungsdetails)
        Dim auftragProxy As Base.Common.DynSapProxyObj = Base.Common.DynSapProxy.getProxy("Z_DPM_COIH_READ_AUFTRAG_001", Me.app, Me.user, Me.page)

        Dim importtable As DataTable = auftragProxy.getImportTable("GS_IN")

        Dim dr As DataRow = importtable.NewRow

        dr("KUNNR_AG") = Me.Kunnr
        dr("AUTYP") = "30"
        ' Geht noch nicht!
        'dr("PLNAL") = pn

        importtable.Rows.Add(dr)
        importtable.AcceptChanges()

        importtable = auftragProxy.getImportTable("GT_IN_IDENTS")

        For Each ident As String In idents
            dr = importtable.NewRow()
            dr.SetField("CHASSIS_NUM", ident)
            importtable.Rows.Add(dr)
        Next

        importtable.AcceptChanges()

        auftragProxy.callBapi()

        Dim kopf As DataTable = auftragProxy.getExportTable("GT_AUF_KOPF")
        Dim rückmeldung As DataTable = auftragProxy.getExportTable("GT_AUF_RUECK")

        Dim s = From k In kopf.AsEnumerable() _
                Join r In rückmeldung.AsEnumerable() _
                On k.Field(Of String)("AUFNR") Equals r.Field(Of String)("AUFNR") _
                Select New Rückmeldungsdetails(k.Field(Of String)("KTEXT"), _
                    k.Field(Of String)("ABGESCHLOSSEN") = "X", _
                    r.Field(Of String)("LEARR"), _
                    r.Field(Of String)("AUERU") = "X")

        Return s.ToList().AsReadOnly()
    End Function
End Class
