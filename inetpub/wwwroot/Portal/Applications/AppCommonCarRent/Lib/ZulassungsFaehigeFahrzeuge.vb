Option Explicit On
Option Strict On

Imports CKG
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

'#################################################################
' Klasse für das anzeigen zulassungsfähiger Fahrzeuge
' Report : zulassungsfähige Fahrzeuge (Report01)
'#################################################################

Public Class ZulassungsFaehigeFahrzeuge
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private mFahrzeuge As DataTable
    Private mFilter As String = ""
    Private mUserHistorie As New ArrayList

#End Region

#Region " Properties"
    Public ReadOnly Property Fahrzeuge() As DataTable
        Get
            Return mFahrzeuge
        End Get
    End Property

    Public ReadOnly Property Filter() As String
        Get
            If mFilter.Length < 4 Then 'anfags and entfernen
                Return mFilter
            Else
                Return mFilter.Remove(0, 4)
            End If
        End Get
    End Property

    Public ReadOnly Property UserHistorie() As ArrayList
        Get
            Return mUserHistorie
        End Get
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    '----------------------------------------------------------------------
    ' Methode: Fill
    ' Autor: JJU
    ' Beschreibung: alle Fahrzeuge die zugelassen werden können+ alle spalten die noch benötigt werden hinzufügen
    ' Erstellt am: 22.01.2009
    ' ITA: 2537
    '----------------------------------------------------------------------

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_READ_FZGPOOL_ZUL_FZG_001", m_objApp, m_objUser, page)


            'myProxy.setImportParameter("I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_READ_FZGPOOL_ZUL_FZG_001", "I_KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tmpDT As DataTable = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            tmpDT.Columns.Add("Standort", System.Type.GetType("System.String"))
            tmpDT.Columns.Add("Absender", System.Type.GetType("System.String"))

            For Each row As DataRow In tmpDT.Rows
                row("Standort") = row("PSTLZ_PDI").ToString & " " & row("ORT01_PDI").ToString & " " & row("NAME1_PDI").ToString & " " & row("NAME2_PDI").ToString & " [" & row("KUNPDI").ToString & "]"
                row("Absender") = row("PSTLZ_ZP").ToString & " " & row("ORT01_ZP").ToString & " " & row("NAME1_ZP").ToString & " " & row("NAME2_ZP").ToString


                For i As Int32 = 0 To row.ItemArray.Length - 1
                    If row(i).ToString.Trim(" "c) = "" Then
                        row(i) = "-" 'weil man nicht mehrer leere Strings mit dem rowfilter abfragen kann
                    End If
                Next

            Next
            tmpDT.AcceptChanges()
            CreateOutPut(tmpDT, strAppID)

            mFahrzeuge = m_tblResult

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

    Public Sub FillDDls(ByRef ddl As DropDownList, ByVal FilterColumn As String)
        '----------------------------------------------------------------------
        'Methode:       FillDDls
        'Autor:         Julian Jung
        'Beschreibung:  füllt die übergebene ddl mit den unterschiedlichen werten der übergebenen spalte
        'Erstellt am:   26.01.2009
        '----------------------------------------------------------------------


        Dim tmpSavedValue As String = Nothing

        If Not ddl.SelectedIndex = -1 Then
            If ddl.Enabled Then
                tmpSavedValue = ddl.SelectedValue
            End If
        End If


        ddl.Items.Clear()
        ddl.Items.Add(New ListItem("-keine Auwahl-", "-1"))
        For Each tmpItem As String In getDifferentItems(FilterColumn)

            ddl.Items.Add(New ListItem(tmpItem))
            If IsDate(tmpItem) Then
                ddl.Items(ddl.Items.Count - 1).Text = CDate(ddl.Items(ddl.Items.Count - 1).Text).ToShortDateString
            End If

        Next

        If Not tmpSavedValue Is Nothing Then
            ddl.Items.FindByValue(tmpSavedValue).Selected = True
        End If

        ddl.Enabled = True
        If ddl.Items.Count = 1 Then
            ddl.Enabled = False
        Else
            If ddl.Items.Count = 2 And Not wasUserSelected(ddl.ID) Then 'wenn nur ein möglichkeit besteht, ddl deaktivieren außer es war eine User-selektion
                ddl.SelectedIndex = 1
                ddl.Enabled = False
            End If
        End If

        If ddl.Enabled Then
            ddl.BackColor = Drawing.Color.White
        Else
            ddl.BackColor = Drawing.Color.FromArgb(255, 255, 204)
        End If

    End Sub

    Public Function wasUserSelected(ByVal ddlID As String) As Boolean
        'es wird die selektionstabelle durchlaufen um festzustellen ob die Selektion einer DDl von einem User Vorgenommen wurde
        'weil wenn der User diese Selektion getätigt hat, dann muss der filter wieder gesetzt werden, sonst nicht

        If UserHistorie.Contains(ddlID) Then
            Return True
        Else
            Return False
        End If


    End Function

    Public Sub GenerateFilter(ByVal FilterColumn As String, ByVal FilterValue As String)
        mFilter = mFilter & " AND " & FilterColumn & "='" & FilterValue & "'"
    End Sub

    Public Sub killFilter()
        mFilter = ""
    End Sub

    Private Function getDifferentItems(ByVal Column As String) As ArrayList

        '----------------------------------------------------------------------
        'Methode:       getDifferentItems
        'Autor:         Julian Jung
        'Beschreibung:  ermittelt alle unterschiedlichen werte aus dem übergebenen spaltennamen
        'Erstellt am:   26.01.2009
        '----------------------------------------------------------------------


        Dim dv As New DataView(mFahrzeuge)
        dv.RowFilter = Filter
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
' $History: ZulassungsFaehigeFahrzeuge.vb $
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 26.01.09   Time: 18:00
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
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
