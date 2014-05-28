Option Explicit On
Option Strict On

Imports CKG
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

'#################################################################
' Klasse für die Zulassung
' Change : Zulassung (Change01)
'#################################################################

Public Class Zulassung
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private mFahrzeuge As DataTable
    Private mFilter As String = ""
    Private mUserHistorie As DataTable
    Private mSelectionStep As Int32 = 0
    Private mWriteUserSlection As Boolean = False
    Private mUebernahmeAusschluss As Boolean = True
    Private mLeasingnehmer As DataTable
    Private mDeckungsKarten As DataTable
    Private mBrandings As DataTable
    Private mAbwScheinSchilderAdr As DataTable
    Private mAdrFilter As String
    Private mZulassungKba As String
    Private mZulassungVerwZweck As String

#End Region

#Region " Properties"
    Public ReadOnly Property Fahrzeuge() As DataTable
        Get
            Return mFahrzeuge
        End Get
    End Property

    Public ReadOnly Property Leasingnehmer() As DataTable
        Get
            If mLeasingnehmer Is Nothing Then
                mLeasingnehmer = New DataTable
                mLeasingnehmer.Columns.Add("NameOrt", String.Empty.GetType)
                mLeasingnehmer.Columns.Add("ID", String.Empty.GetType)
            End If
            Return mLeasingnehmer
        End Get
    End Property

    Public ReadOnly Property Deckungskarten() As DataTable
        Get
            Return mDeckungsKarten
        End Get
    End Property

    Public WriteOnly Property Filterstring() As String
        Set(ByVal value As String)

            mAdrFilter = value
        End Set
    End Property

    Public ReadOnly Property Brandings() As DataTable
        Get
            Return mBrandings
        End Get
    End Property

    Public ReadOnly Property AbwScheinSchilderAdrDV() As DataView
        Get
            Dim tmpDV As New DataView(mAbwScheinSchilderAdr)

            If Not mAdrFilter Is Nothing Then
                If Not mAdrFilter.Length < 4 Then 'anfags and entfernen
                    mAdrFilter = mAdrFilter.Remove(0, 4)
                End If
            End If

            tmpDV.RowFilter = mAdrFilter



            Return tmpDV
        End Get
    End Property

    Public ReadOnly Property AbwScheinSchilderAdr() As DataTable
        Get
            Return mAbwScheinSchilderAdr
        End Get
    End Property

    Public ReadOnly Property Filter() As String
        Get
            If UebernahmeAusschluss Then
                If mFilter.Contains(" AND Uebernommen<>'X'") = False Then
                    mFilter = mFilter & " AND Uebernommen<>'X'"
                End If
            End If

            If mFilter.Length < 4 Then 'anfags and entfernen
                Return mFilter
            Else
                Return mFilter.Remove(0, 4)
            End If
        End Get
    End Property

    Public Property SelektionStep() As Int32
        Get
            Return mSelectionStep
        End Get
        Set(ByVal value As Int32)
            mSelectionStep = value
        End Set
    End Property

    Public Property writeUserSelektion() As Boolean
        Get
            Return mWriteUserSlection
        End Get
        Set(ByVal value As Boolean)
            mWriteUserSlection = value
        End Set
    End Property

    Public Property UebernahmeAusschluss() As Boolean
        Get
            Return mUebernahmeAusschluss
        End Get
        Set(ByVal value As Boolean)
            mUebernahmeAusschluss = value
        End Set
    End Property

    Public ReadOnly Property UserHistorie() As DataTable
        Get
            If mUserHistorie Is Nothing Then
                mUserHistorie = New DataTable
                With mUserHistorie

                    .Columns.Add("Step", String.Empty.GetType)
                    .Columns.Add("ControlID", String.Empty.GetType)
                    .Columns.Add("SelectedValue", String.Empty.GetType)
                End With
                mUserHistorie.AcceptChanges()
            End If
            Return mUserHistorie
        End Get
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Sub nextUserStep()
        mSelectionStep += 1
        mWriteUserSlection = True

        For Each tmpRow As DataRow In UserHistorie.Select("Step='" & mSelectionStep & "'")
            'wenn in diesem userstep noch gespeicherte daten vorhanden sind, diese löschen!
            UserHistorie.Rows.Remove(tmpRow)
        Next

    End Sub

    Public Sub SelectionStepZurueck()
        mSelectionStep -= 1
        mWriteUserSlection = False
    End Sub

    Public Sub fillKbaNummer(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            S.AP.InitExecute("Z_M_IMP_AUFTRDAT_007", "I_KUNNR, I_KENNUNG", Right("0000000000" & m_objUser.KUNNR, 10), "KBA")

            Dim tmpTable As DataTable = S.AP.GetExportTable("GT_WEB")

            If tmpTable IsNot Nothing AndAlso tmpTable.Rows.Count > 0 Then
                mZulassungKba = tmpTable.Rows(0)("POS_KURZTEXT").ToString()
            End If

        Catch ex As Exception
            m_intStatus = -1111
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try
    End Sub

    Public Sub fillVerwendungszweck(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            S.AP.InitExecute("Z_M_IMP_AUFTRDAT_007", "I_KUNNR, I_KENNUNG", Right("0000000000" & m_objUser.KUNNR, 10), "VERWENDUNGSZWECK")

            Dim tmpTable As DataTable = S.AP.GetExportTable("GT_WEB")

            If tmpTable IsNot Nothing AndAlso tmpTable.Rows.Count > 0 Then
                mZulassungVerwZweck = tmpTable.Rows(0)("POS_KURZTEXT").ToString()
            End If

        Catch ex As Exception
            m_intStatus = -1111
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try
    End Sub

    Public Sub Zulassen(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)
        '----------------------------------------------------------------------
        ' Methode: Zulassen
        ' Autor: JJU
        ' Beschreibung: Zulassen(change01_2)
        ' Erstellt am: 10.02.2009
        ' ITA: 2535
        '----------------------------------------------------------------------

        m_strClassAndMethod = "Zulassung.Zulassen"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            S.AP.Init("Z_M_WEB_DEZZUL_001", "I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim sapZulassungsTable As DataTable = S.AP.GetImportTable("GT_WEB") 'myProxy.getImportTable("GT_WEB")

            For Each row As DataRow In Fahrzeuge.Select("Uebernommen='X'")
                Dim newZulassung As DataRow = sapZulassungsTable.NewRow
                With newZulassung
                    .Item("I_ZH") = Right("0000000000" & m_objUser.KUNNR, 10)
                    .Item("I_ZC") = row("LizensnehmerNR")

                    'versicherernummer laut evbNr filtern
                    .Item("I_ZV") = mDeckungsKarten.Select("ZVSNR='" & row("EVBNR").ToString & "'")(0)("KUNNR_ZV").ToString

                    .Item("I_ZULST") = mZulassungKba
                    .Item("I_Verwzweck") = mZulassungVerwZweck

                    .Item("I_ZS") = "0000000001" 'Immer DAD?

                    If Not row("AbwScheinSchilderNR").ToString = "-" AndAlso Not row("AbwScheinSchilderNR").ToString.Trim(" "c) = "" Then
                        .Item("DADPDI") = row("AbwScheinSchilderNR")
                    Else
                        .Item("DADPDI") = row("StandortNR")
                    End If

                    .Item("I_VSNR") = row("EVBNR")
                    .Item("I_FAHRG") = row("Fahrgestellnummer")
                    .Item("I_ZULDAT") = row("Zuldat")
                    .Item("I_ZZLABEL") = row("BrandingNR")
                    .Item("I_ZLAUFZEIT") = row("Laufzeit")
                End With
                sapZulassungsTable.Rows.Add(newZulassung)
            Next

            'myProxy.callBapi()
            S.AP.Execute()

            Dim SapResultTable As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            Dim SapResultMessagetable As DataTable = S.AP.GetExportTable("GT_MESSAGE") 'myProxy.getExportTable("GT_MESSAGE")
            For Each tmpRow As DataRow In SapResultTable.Rows
                Dim tmpFahrzeug As DataRow = mFahrzeuge.Select("Fahrgestellnummer='" & tmpRow.Item("I_FAHRG").ToString & "'")(0)
                'das kennzeichen der zulassung
                tmpFahrzeug.Item("Zulassungskennzeichen") = tmpRow("O_LICENSE_NUM")
                'msg Tabelle
                If Not SapResultMessagetable.Select("CHASSIS_NUM='" & tmpRow.Item("I_FAHRG").ToString & "'").Count = 0 Then
                    tmpFahrzeug.Item("Status") = "Fehler: " & SapResultMessagetable.Select("CHASSIS_NUM='" & tmpRow.Item("I_FAHRG").ToString & "'")(0)("MSG_TEXT").ToString
                Else
                    tmpFahrzeug.Item("Status") = "" '- entfernen 
                End If
            Next
            mFahrzeuge.AcceptChanges()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -1111
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)
        '----------------------------------------------------------------------
        ' Methode: Fill
        ' Autor: JJU
        ' Beschreibung: füllen der Selektiondaten(Report01)
        ' Erstellt am: 22.01.2009
        ' ITA: 2535
        '----------------------------------------------------------------------

        m_strClassAndMethod = "Zulassung.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            S.AP.InitExecute("Z_M_READ_FZGPOOL_ZUL_FZG_001", "I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tmpDT As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            tmpDT.Columns.Add("Standort", System.Type.GetType("System.String"))
            tmpDT.Columns.Add("Absender", System.Type.GetType("System.String"))
            tmpDT.Columns.Add("Ausgewaehlt", System.Type.GetType("System.String"))
            tmpDT.Columns.Add("Uebernommen", System.Type.GetType("System.String"))
            tmpDT.Columns.Add("Zuldat", System.Type.GetType("System.String"))
            With tmpDT
                .Columns.Add("Laufzeit", System.Type.GetType("System.String"))
                .Columns.Add("BrandingTEXT", System.Type.GetType("System.String"))
                .Columns.Add("BrandingNR", System.Type.GetType("System.String"))
                .Columns.Add("EVBTEXT", System.Type.GetType("System.String"))
                .Columns.Add("EVBNR", System.Type.GetType("System.String"))
                .Columns.Add("LizensnehmerTEXT", System.Type.GetType("System.String"))
                .Columns.Add("LizensnehmerNR", System.Type.GetType("System.String"))
                .Columns.Add("AbwScheinSchilderNR", System.Type.GetType("System.String"))
                .Columns.Add("AbwScheinSchilderTEXT", System.Type.GetType("System.String"))
                .Columns.Add("Zulassungskennzeichen", System.Type.GetType("System.String"))
                .Columns.Add("Status", System.Type.GetType("System.String"))
                .Columns.Add("Sicherungsscheinpflichtig", System.Type.GetType("System.String"))
            End With


            For Each row As DataRow In tmpDT.Rows
                row("Standort") = row("PSTLZ_PDI").ToString & " " & row("ORT01_PDI").ToString & " " & row("NAME1_PDI").ToString & " " & row("NAME2_PDI").ToString
                row("Absender") = row("PSTLZ_ZP").ToString & " " & row("ORT01_ZP").ToString & " " & row("NAME1_ZP").ToString & " " & row("NAME2_ZP").ToString
                row("Sicherungsscheinpflichtig") = "X"
                For i As Int32 = 0 To row.ItemArray.Length - 1
                    If row(i).ToString.Trim(" "c) = "" Then
                        row(i) = "-" 'weil man nicht mehrer leere Strings mit dem rowfilter abfragen kann
                    End If
                Next

            Next
            tmpDT.AcceptChanges()
            CreateOutPut(tmpDT, strAppID)


            mFahrzeuge = m_tblResult

            mFahrzeuge.CaseSensitive = True


            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -1111
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Public Sub fillAbwScheinSchilderAdr(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)

        '----------------------------------------------------------------------
        ' Methode: fillAbwScheinSchilderAdr
        ' Autor: JJU
        ' Beschreibung: füllt die Adressen für abweichende Schein u Schilderversendungen
        ' Erstellt am: 10.02.2009
        ' ITA: 2535
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try


            S.AP.InitExecute("Z_M_GET_KUNDEN_PDI", "I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tmpDT As DataTable = S.AP.GetExportTable("GT_PDI") 'myProxy.getExportTable("GT_PDI")

            tmpDT.Columns.Add("AdressAnzeige", System.Type.GetType("System.String"))
            tmpDT.Columns.Add("NAME", System.Type.GetType("System.String"))

            For Each row As DataRow In tmpDT.Rows
                row("AdressAnzeige") = row("PSTLZ").ToString & " " & row("STRAS1").ToString & " " & row("ORT01").ToString & " " & row("NAME1").ToString & " " & row("NAME2").ToString
                row("NAME") = row("NAME1").ToString & " " & row("NAME2").ToString
            Next

            tmpDT.AcceptChanges()
            mAbwScheinSchilderAdr = tmpDT

        Catch ex As Exception
            m_intStatus = -1111
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try
    End Sub

    Public Sub FILLLeasingnehmerUndBranding(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)
        '----------------------------------------------------------------------
        ' Methode: Fill
        ' Autor: JJU
        ' Beschreibung: füllt die Leasingnehmer /Deckungskarten und Branding Tabelle
        ' Erstellt am: 22.01.2009
        ' ITA: 2535
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Leasingnehmer/Deckungskarten

            S.AP.InitExecute("Z_M_LIZNEHM_001", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tmpDT As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            tmpDT.Columns.Add("DeckungsKarteAnzeige", System.Type.GetType("System.String"))


            For Each row As DataRow In tmpDT.Rows
                row("DeckungsKarteAnzeige") = row("ZVSNR").ToString & " " & row("NAME1_ZV").ToString
            Next
            tmpDT.AcceptChanges()

            tmpDT.CaseSensitive = True


            'alle unterschiedlichen Leasingnehmer herrausfinden
            Dim dv As New DataView(tmpDT)
            dv.Sort = "KUNNR_LZN"
            Dim e As Int32 = 0
            Dim tmpRow As DataRow
            Leasingnehmer.Clear()

            Do While e < dv.Count
                If Not Leasingnehmer.Rows.Count = 0 Then
                    If Leasingnehmer.Rows(Leasingnehmer.Rows.Count - 1).ToString <> dv.Item(e)("KUNNR_LZN").ToString Then
                        tmpRow = Leasingnehmer.NewRow
                        tmpRow("NameOrt") = dv.Item(e)("NAME1_LZN").ToString & " " & dv.Item(e)("ORT_LZN").ToString
                        tmpRow("ID") = dv.Item(e)("KUNNR_LZN").ToString
                        Leasingnehmer.Rows.Add(tmpRow)
                    End If
                Else
                    tmpRow = Leasingnehmer.NewRow
                    tmpRow("NameOrt") = dv.Item(e)("NAME1_LZN").ToString & " " & dv.Item(e)("ORT_LZN").ToString
                    tmpRow("ID") = dv.Item(e)("KUNNR_LZN").ToString
                    Leasingnehmer.Rows.Add(tmpRow)
                End If


                e += 1
            Loop
            mDeckungsKarten = tmpDT



            'Branding

            S.AP.InitExecute("Z_V_ZDAD_LABEL_001", "I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            mBrandings = S.AP.GetExportTable("GT_WEB") 'myProxy2.getExportTable("GT_WEB")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -1111
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

    Public Sub FillControls(ByRef ctr As ListControl, ByVal FilterColumn As String)
        '----------------------------------------------------------------------
        'Methode:       FillControls
        'Autor:         Julian Jung
        'Beschreibung:  füllt die übergebene ddl mit den unterschiedlichen werten der übergebenen spalte
        'Erstellt am:   26.01.2009
        '----------------------------------------------------------------------


        Dim tmpLBX As New ListBox
        Dim tmpRBL As New RadioButtonList
        Dim tmpDDL As New DropDownList


        If mWriteUserSlection Then
            writeCurrentSelektionStep(ctr)
        End If

        ctr.Enabled = True

        Select Case ctr.GetType.ToString
            Case tmpLBX.GetType.ToString
                tmpLBX = CType(ctr, ListBox)
                tmpLBX.Items.Clear()

                For Each tmpItem As String In getDifferentItems(FilterColumn)

                    tmpLBX.Items.Add(New ListItem(tmpItem))
                    If IsDate(tmpItem) Then
                        tmpLBX.Items(tmpLBX.Items.Count - 1).Text = CDate(tmpLBX.Items(tmpLBX.Items.Count - 1).Text).ToShortDateString
                    End If
                Next

                If tmpLBX.Items.Count = 1 Then
                    tmpLBX.SelectedIndex = 0
                    tmpLBX.Enabled = False
                End If

            Case tmpRBL.GetType.ToString
                tmpRBL = CType(ctr, RadioButtonList)
                'nur initial befüllen
                If tmpRBL.Items.Count = 0 Then
                    For Each tmpItem As String In getDifferentItems(FilterColumn)
                        tmpRBL.Items.Add(New ListItem(tmpItem))
                        If IsDate(tmpItem) Then
                            tmpRBL.Items(tmpRBL.Items.Count - 1).Text = CDate(tmpRBL.Items(tmpRBL.Items.Count - 1).Text).ToShortDateString
                        End If
                    Next
                End If

                If getDifferentItems(FilterColumn).Count = 1 Then
                    tmpRBL.Items.FindByValue(getDifferentItems(FilterColumn)(0).ToString).Selected = True
                    tmpRBL.Enabled = False
                Else
                    If getDifferentItems(FilterColumn).Count < tmpRBL.Items.Count Then
                        For Each tmpItem As ListItem In tmpRBL.Items
                            tmpItem.Enabled = False
                        Next

                        For Each tmpItem As String In getDifferentItems(FilterColumn)
                            tmpRBL.Items.FindByValue(tmpItem.ToString).Enabled = True
                        Next
                    Else 'es sind noch alle möglichkeiten vorhanden
                        For Each tmpItem As ListItem In tmpRBL.Items
                            tmpItem.Enabled = True
                        Next
                    End If
                End If

            Case tmpDDL.GetType.ToString
                tmpDDL = CType(ctr, DropDownList)
                tmpDDL.Items.Clear()
                tmpDDL.Items.Add(New ListItem("-keine Auwahl-", "-1"))
                For Each tmpItem As String In getDifferentItems(FilterColumn)

                    tmpDDL.Items.Add(New ListItem(tmpItem))
                    If IsDate(tmpItem) Then
                        tmpDDL.Items(tmpDDL.Items.Count - 1).Text = CDate(tmpDDL.Items(tmpDDL.Items.Count - 1).Text).ToShortDateString
                    End If
                Next

                If tmpDDL.Items.Count = 2 Then
                    tmpDDL.SelectedIndex = 1
                    tmpDDL.Enabled = False
                End If

        End Select


        If ctr.Enabled Then
            getSavedValue(ctr)

            Select Case ctr.GetType.ToString
                Case tmpLBX.GetType.ToString
                    tmpLBX.BackColor = Drawing.Color.White
                Case tmpDDL.GetType.ToString
                    tmpDDL.BackColor = Drawing.Color.White
                Case tmpRBL.GetType.ToString
            End Select
        Else
            Select Case ctr.GetType.ToString
                Case tmpLBX.GetType.ToString
                    tmpLBX.Enabled = True
                    tmpLBX.BackColor = Drawing.Color.FromArgb(255, 255, 204)
                Case tmpDDL.GetType.ToString
                    tmpDDL.BackColor = Drawing.Color.FromArgb(255, 255, 204)
                Case tmpRBL.GetType.ToString

            End Select

        End If
    End Sub

    Public Sub GenerateFilter(ByVal FilterColumn As String, ByVal FilterValue As String)
        mFilter = mFilter & " AND " & FilterColumn & "='" & FilterValue & "'"
    End Sub

    Public Sub killFilter()
        mFilter = ""
    End Sub

    Private Sub getSavedValue(ByRef ctr As ListControl)

        Dim tmpRows() As DataRow

        tmpRows = UserHistorie.Select("Step='" & mSelectionStep & "' AND ControlID='" & ctr.ID & "'")

        If tmpRows.Length = 0 Then
            ctr.SelectedIndex = -1
        ElseIf tmpRows.Length = 1 Then
            If Not ctr.Items.FindByValue(tmpRows(0)("SelectedValue").ToString) Is Nothing Then
                ctr.Items.FindByValue(tmpRows(0)("SelectedValue").ToString).Selected = True
            Else
                ctr.SelectedIndex = -1
            End If
        Else
            Throw New Exception("ein Control existiert mehrmals in einem Userstep")
        End If

    End Sub

    Private Sub writeCurrentSelektionStep(ByRef ctr As ListControl)

        Dim tmpRow As DataRow = UserHistorie.NewRow

        With tmpRow
            .Item("Step") = mSelectionStep
            .Item("ControlID") = ctr.ID
            .Item("SelectedValue") = ctr.SelectedValue
        End With

        UserHistorie.Rows.Add(tmpRow)
        UserHistorie.AcceptChanges()

    End Sub

    Private Function getDifferentItems(ByVal Column As String) As ArrayList

        '----------------------------------------------------------------------
        'Methode:       getDifferentItems
        'Autor:         Julian Jung
        'Beschreibung:  ermittelt alle unterschiedlichen werte aus dem übergebenen spaltennamen
        'Erstellt am:   26.01.2009
        '----------------------------------------------------------------------

        Dim dv As New DataView(mFahrzeuge)
        dv.RowFilter = Filter()
        dv.Sort = Column
        Dim e As Int32 = 0
        getDifferentItems = New ArrayList
        Do While e < dv.Count
            If Not getDifferentItems.Count = 0 Then
                If getDifferentItems.Item(getDifferentItems.Count - 1).ToString <> dv.Item(e)(Column).ToString Then
                    getDifferentItems.Add(dv.Item(e)(Column).ToString)
                End If
            Else
                getDifferentItems.Add(dv.Item(e)(Column).ToString)
            End If
            e += 1
        Loop
        Return getDifferentItems
    End Function


#End Region

End Class
' ************************************************
' $History: Zulassung.vb $
' 
' *****************  Version 20  *****************
' User: Fassbenders  Date: 29.03.11   Time: 13:40
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' 
' *****************  Version 19  *****************
' User: Fassbenders  Date: 29.03.11   Time: 12:52
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 4749
' 
' *****************  Version 18  *****************
' User: Fassbenders  Date: 1.04.10    Time: 10:13
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' 
' *****************  Version 17  *****************
' User: Fassbenders  Date: 31.03.10   Time: 14:01
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' 
' *****************  Version 16  *****************
' User: Fassbenders  Date: 30.03.10   Time: 13:39
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA: 3553
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 16.06.09   Time: 13:43
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 24.02.09   Time: 13:17
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' nachbesserungen
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 12.02.09   Time: 10:56
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2537
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 12.02.09   Time: 10:51
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2537
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 11.02.09   Time: 17:31
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ita 2537
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 10.02.09   Time: 17:32
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2537
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 10.02.09   Time: 16:22
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2537
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 4.02.09    Time: 19:04
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ita 2537
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 3.02.09    Time: 18:25
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITa 2537
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 30.01.09   Time: 15:20
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2537
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 29.01.09   Time: 17:29
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2537
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 28.01.09   Time: 17:00
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2537
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 28.01.09   Time: 11:11
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2537
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 27.01.09   Time: 17:31
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2537
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 26.01.09   Time: 18:00
' Created in $/CKAG/Applications/AppCommonCarRent/Lib
' ita 2537
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 26.01.09   Time: 11:27
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ita 2535 fertig
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 26.01.09   Time: 11:21
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2535
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 23.01.09   Time: 17:12
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2535
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 22.01.09   Time: 17:11
' Created in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2535
' 
' ************************************************
