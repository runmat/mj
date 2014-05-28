Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Public Class SperrPruefung
    Inherits Base.Business.DatenimportBase

    Enum UploadTableStatus
        Ok
        Warnung
        Fehler
    End Enum

#Region "Declarations"

    Private m_Auftraggeber As String = String.Empty
    Private m_Treuhandgeber As String = String.Empty

    Private m_EQUI_KEY As String = String.Empty
    Private m_ERNAM As String = String.Empty
    Private m_ERDAT As String = String.Empty
    Private m_SPERRDAT As String = String.Empty
    Private m_ZZREFERENZ2 As String = String.Empty
    Private m_Action As String = String.Empty
    Private m_isSperren As Boolean = False

#End Region

#Region "Properties"

    Public Property Treuhandgeber() As String
        Get
            Return m_Treuhandgeber
        End Get
        Set(ByVal Value As String)
            m_Treuhandgeber = Value
        End Set
    End Property

    Public Property Auftraggeber() As String
        Get
            Return m_Auftraggeber
        End Get
        Set(ByVal Value As String)
            m_Auftraggeber = Value
        End Set
    End Property

    Public Property IsSperren As Boolean
        Get
            Return m_isSperren
        End Get
        Set(ByVal Value As Boolean)
            m_isSperren = Value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Function CheckAll(ByVal page As Page, ByRef table As DataTable) As Dictionary(Of String, String())

        Dim tabImport As DataTable
        Dim outTable As DataTable

        Dim dicRet = New Dictionary(Of String, String())

        Try
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_CHECK_EQUI_01", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", Right("0000000000" & m_Auftraggeber, 10))
            myProxy.setImportParameter("I_TREU", Right("0000000000" & m_Treuhandgeber, 10))

            If IsSperren Then
                myProxy.setImportParameter("I_TREUH_VGA", "S")
            Else
                myProxy.setImportParameter("I_TREUH_VGA", "F")
            End If

            tabImport = myProxy.getImportTable("GT_IN")
            For Each row As DataRow In table.Rows

                If row("SELECT").Equals("99") Then
                    Me.m_SPERRDAT = row("SPERRDAT").ToString
                    Me.m_EQUI_KEY = row("EQUI_KEY").ToString
                    Me.m_ERDAT = row("ERDAT").ToString
                    Me.m_ZZREFERENZ2 = row("ZZREFERENZ2").ToString
                    Me.m_ERNAM = row("ERNAM").ToString
                    tabImport.Rows.Add(New String() {m_EQUI_KEY, m_ERNAM, m_ERDAT, m_SPERRDAT, m_ZZREFERENZ2})
                End If

            Next

            myProxy.callBapi()

            outTable = myProxy.getExportTable("GT_OUT")

            Dim returnString As String = String.Empty
            Dim equKey As String
            Dim errTyp As String
            Dim errTxt As String
            Dim err As String

            'ergebnis auslesen
            For Each row As DataRow In outTable.Rows
                equKey = row("EQUI_KEY").ToString

                err = row("ERROR").ToString
                errTyp = row("ERROR_TYP").ToString
                errTxt = row("ERROR_TEXT").ToString

                If dicRet.ContainsKey(equKey) Then
                    dicRet(equKey).SetValue("F", 0)
                    dicRet(equKey).SetValue(equKey + " ist mehrfach im Upload vorhanden.", 1)
                Else
                    dicRet.Add(equKey, New String() {errTyp, errTxt, err})
                End If

            Next

            Return dicRet

        Catch ex As Exception

            Return Nothing

        End Try

    End Function

    Public Function CheckAllTable(ByVal page As Page, ByRef table As DataTable) As UploadTableStatus

        Dim erg As UploadTableStatus = UploadTableStatus.Ok
        Dim tabImport As DataTable
        Dim outTable As DataTable

        Dim lstEquis As New List(Of String)

        Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_CHECK_TH_CODE", m_objApp, m_objUser, page)

        myProxy.setImportParameter("I_AG", Right("0000000000" & m_Auftraggeber, 10))
        myProxy.setImportParameter("I_TREU", Right("0000000000" & m_Treuhandgeber, 10))

        If IsSperren Then
            myProxy.setImportParameter("I_TREUH_VGA", "S")
        Else
            myProxy.setImportParameter("I_TREUH_VGA", "F")
        End If

        tabImport = myProxy.getImportTable("GT_IN")
        For Each row As DataRow In table.Rows

            If row("SELECT").Equals("99") Then
                Me.m_SPERRDAT = row("SPERRDAT").ToString
                Me.m_EQUI_KEY = row("EQUI_KEY").ToString
                Me.m_ERDAT = row("ERDAT").ToString
                Me.m_ZZREFERENZ2 = row("ZZREFERENZ2").ToString
                Me.m_ERNAM = row("ERNAM").ToString
                tabImport.Rows.Add(New String() {m_EQUI_KEY, m_ERNAM, m_ERDAT, m_SPERRDAT, m_ZZREFERENZ2})
            End If

        Next

        myProxy.callBapi()

        outTable = myProxy.getExportTable("GT_OUT")

        Dim equKey As String
        Dim errTyp As String
        Dim errTxt As String
        Dim err As String

        'ergebnis auslesen
        For Each row As DataRow In table.Rows
            row("ERROR") = ""

            equKey = row("EQUI_KEY").ToString

            Dim rows As DataRow() = outTable.Select("EQUI_KEY='" & equKey & "'")

            If rows.Length > 0 Then
                err = rows(0)("ERROR").ToString
                errTyp = rows(0)("ERROR_TYP").ToString
                errTxt = rows(0)("ERROR_TEXT").ToString

                If lstEquis.Contains(equKey) Then
                    row("ERROR") = equKey + " ist mehrfach im Upload vorhanden."
                    erg = UploadTableStatus.Fehler
                Else
                    lstEquis.Add(equKey)
                End If

                'Fehler 12 nur beim Sperren berücksichtigen
                If IsSperren OrElse Not err = "12" Then
                    If errTyp = "F" Then
                        row("ERROR") = "FEHLER: " & errTxt
                        erg = UploadTableStatus.Fehler
                    ElseIf errTyp = "W" Then
                        row("ERROR") = "WARNUNG: " & errTxt
                        If Not erg = UploadTableStatus.Fehler Then
                            erg = UploadTableStatus.Warnung
                        End If
                    Else
                        row("ERROR") = errTxt
                    End If
                End If
            End If
        Next

        Return erg

    End Function

    Public Function CheckAll(ByVal page As Page, ByRef GridView1 As GridView) As Dictionary(Of String, String())

        Dim tabImport As DataTable
        Dim outTable As DataTable

        Dim dicRet = New Dictionary(Of String, String())

        Try
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_CHECK_EQUI_01", m_objApp, m_objUser, page)


            myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            myProxy.setImportParameter("I_TREU", Right("0000000000" & m_Treuhandgeber, 10))


            tabImport = myProxy.getImportTable("GT_IN")

            Dim chbox As CheckBox

            For Each Row As GridViewRow In GridView1.Rows

                chbox = CType(Row.Cells(0).FindControl("chkAnfordern"), CheckBox)

                If chbox.Checked Then
                    Me.m_SPERRDAT = CType(Row.FindControl("lblSPERRDAT"), Label).Text
                    Me.m_EQUI_KEY = CType(Row.FindControl("lblEQUI_KEY"), Label).Text
                    Me.m_ERDAT = CType(Row.FindControl("lblERDAT"), Label).Text
                    Me.m_ZZREFERENZ2 = CType(Row.FindControl("lblZZREFERENZ2"), Label).Text
                    Me.m_ERNAM = CType(Row.FindControl("lblERNAM"), Label).Text
                    tabImport.Rows.Add(New String() {m_EQUI_KEY, m_ERNAM, m_ERDAT, m_SPERRDAT, m_ZZREFERENZ2})
                End If

            Next

            myProxy.callBapi()

            outTable = myProxy.getExportTable("GT_OUT")

            Dim returnString As String = String.Empty
            Dim equKey As String
            Dim errTyp As String
            Dim errTxt As String
            Dim err As String

            'Dim ag As String
            'Dim treu As String
            'Dim errNam As String
            'Dim errDat As String
            'Dim sperrDat As String
            'Dim ref2 As String


            'ergebnis auslesen
            For Each row As DataRow In outTable.Rows
                equKey = row("EQUI_KEY").ToString
                If String.IsNullOrEmpty(equKey) Then
                    Continue For
                End If

                'ag = row("AG").ToString
                'treu = row("TREU").ToString
                'errNam = row("ERNAM").ToString
                'errDat = row("ERDAT").ToString
                'sperrDat = row("SPERRDAT").ToString
                'ref2 = row("ZZREFERENZ2").ToString
                err = row("ERROR").ToString
                errTyp = row("ERROR_TYP").ToString
                errTxt = row("ERROR_TEXT").ToString

                If dicRet.ContainsKey(equKey) Then
                    dicRet(equKey).SetValue("F", 0)
                    dicRet(equKey).SetValue(equKey + " ist mehrfach im Upload vorhanden.", 1)
                Else
                    dicRet.Add(equKey, New String() {errTyp, errTxt, err})
                End If

            Next

            Return dicRet

        Catch ex As Exception

            Return Nothing

        End Try

    End Function


#End Region

End Class
