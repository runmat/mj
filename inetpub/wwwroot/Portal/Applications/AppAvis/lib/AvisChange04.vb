Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports System.Globalization

Imports SapORM.Contracts

Public Class AvisChange04

    Inherits CKG.Base.Business.DatenimportBase

#Region "Declarations"

    Private m_Action As Int32

    Private mCarport As String
    Private mFarbe As String
    Private mLiefermonat As String
    Private mHersteller As String
    Private mModellgruppe As String
    Private mKraftstoff As String
    Private mNAVI As String
    Private mReifenart As String
    Private mAufbauart As String
    Private mSperreAb As String
    Private mSperreBis As String
    Private mAnzFzge As String
    Private mSperrvermerk As String
    Private mTyp As String
    Private mEinbaufirma As String
    Private mAusruestung As String
    Private mUserRegel As String
    Private mHaendlernr As String
    Private mDropDownTable As DataTable

#End Region


#Region "Properties"
    Public Property Aktion() As Int32
        Get
            Return m_Action
        End Get
        Set(ByVal Value As Int32)
            m_Action = Value
        End Set
    End Property

    Public Property DropDownTable() As DataTable
        Get
            Return mDropDownTable
        End Get
        Set(ByVal value As DataTable)
            mDropDownTable = value
        End Set
    End Property

    Public Property Carport() As String
        Get
            Return mCarport
        End Get
        Set(ByVal value As String)
            mCarport = value
        End Set
    End Property

    Public Property Farbe() As String
        Get
            Return mFarbe
        End Get
        Set(ByVal value As String)
            mFarbe = value
        End Set
    End Property

    Public Property Liefermonat() As String
        Get
            Return mLiefermonat
        End Get
        Set(ByVal value As String)
            mLiefermonat = value
        End Set
    End Property

    Public Property Hersteller() As String
        Get
            Return mHersteller
        End Get
        Set(ByVal value As String)
            mHersteller = value
        End Set
    End Property

    Public Property Modellgruppe() As String
        Get
            Return mModellgruppe
        End Get
        Set(ByVal value As String)
            mModellgruppe = value
        End Set
    End Property

    Public Property Kraftstoff() As String
        Get
            Return mKraftstoff
        End Get
        Set(ByVal value As String)
            mKraftstoff = value
        End Set
    End Property

    Public Property NAVI() As String
        Get
            Return mNAVI
        End Get
        Set(ByVal value As String)
            mNAVI = value
        End Set
    End Property

    Public Property Reifenart() As String
        Get
            Return mReifenart
        End Get
        Set(ByVal value As String)
            mReifenart = value
        End Set
    End Property

    Public Property Aufbauart() As String
        Get
            Return mAufbauart
        End Get
        Set(ByVal value As String)
            mAufbauart = value
        End Set
    End Property

    Public Property SperreAb() As String
        Get
            Return mSperreAb
        End Get
        Set(ByVal value As String)
            mSperreAb = value
        End Set
    End Property

    Public Property SperreBis() As String
        Get
            Return mSperreBis
        End Get
        Set(ByVal value As String)
            mSperreBis = value
        End Set
    End Property

    Public Property AnzFzge() As String
        Get
            Return mAnzFzge
        End Get
        Set(ByVal value As String)
            mAnzFzge = value
        End Set
    End Property

    Public Property Sperrvermerk() As String
        Get
            Return mSperrvermerk
        End Get
        Set(ByVal value As String)
            mSperrvermerk = value
        End Set
    End Property

    Public Property Typ() As String
        Get
            Return mTyp
        End Get
        Set(ByVal value As String)
            mTyp = value
        End Set
    End Property

    Public Property Einbaufirma() As String
        Get
            Return mEinbaufirma
        End Get
        Set(ByVal value As String)
            mEinbaufirma = value
        End Set
    End Property

    Public Property Ausruestung() As String
        Get
            Return mAusruestung
        End Get
        Set(ByVal value As String)
            mAusruestung = value
        End Set
    End Property

    Public Property UserRegel() As String
        Get
            Return mUserRegel
        End Get
        Set(ByVal value As String)
            mUserRegel = value
        End Set
    End Property

    Public Property Haendlernr() As String
        Get
            Return mHaendlernr
        End Get
        Set(ByVal value As String)
            mHaendlernr = value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Sub FillDropdown()

        Try

            m_tblResult = S.AP.GetExportTableWithInitExecute("Z_M_READ_LISTBOX_BR_006.GT_WEB", "I_KUNNR", m_objUser.KUNNR.ToSapKunnr)

            Dim row As DataRow

            For Each row In m_tblResult.Rows
                If row("KENNUNG").ToString = "EINBAUFIRMA" Then
                    row("POS_TEXT") = row("POS_KURZTEXT").ToString & " - " & row("POS_TEXT").ToString
                End If
            Next

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Abfragen der Leasingnehmerdaten.<br>(" & ex.Message & ")"
            End Select

            m_tblResult = Nothing

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

        End Try

        DropDownTable = m_tblResult


    End Sub

    Public Sub SaveRegel()

        Try

            S.AP.Init("Z_M_CHANGE_BLOCKREG_001", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

            Dim SAPTable As DataTable = S.AP.GetImportTable("GT_WEB")

            Dim Row As DataRow = SAPTable.NewRow

            Row("ID_BLOCK_RG") = "0000000000"
            Row("FARBE_DE") = Farbe
            Row("CARPORT") = Carport
            Row("GEPL_LIEFTERMIN") = Liefermonat
            Row("HERST_NUMMER") = Hersteller
            Row("MODELLGRUPPE") = Modellgruppe
            Row("KRAFTSTOFF") = Kraftstoff
            Row("NAVIGATION") = NAVI
            Row("REIFENART") = Reifenart
            Row("AUFBAUART") = Aufbauart
            Row("TYP") = Typ
            Dim datSperreAb As DateTime
            If Not String.IsNullOrEmpty(SperreAb) AndAlso DateTime.TryParseExact(SperreAb, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, datSperreAb) Then
                Row("DAT_SPERR_AB") = datSperreAb
            End If
            Dim datSperreBis As DateTime
            If Not String.IsNullOrEmpty(SperreBis) AndAlso DateTime.TryParseExact(SperreBis, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, datSperreBis) Then
                Row("DAT_SPERR_BIS") = datSperreBis
            End If
            Row("SPERRVERMERK") = Sperrvermerk
            Row("ANZ_FZG") = AnzFzge
            Row("WEB_USER") = m_objUser.UserName
            Row("RETUR_BEM") = ""
            Row("LIEFERANT") = mHaendlernr

            SAPTable.Rows.Add(Row)

            S.AP.Execute()

            Dim SAPTableEx As DataTable = S.AP.GetExportTable("GT_WEB")

            If SAPTableEx.Rows.Count > 0 Then
                If Len(SAPTableEx.Rows(0)("RETUR_BEM").ToString) > 0 Then
                    Throw New Exception()
                End If
            End If

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Speichern."
            End Select

            m_tblResult = Nothing

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Throw New Exception(m_strMessage)

        End Try

    End Sub


    Public Function GetSaveTable() As DataTable

        Return S.AP.GetImportTableWithInit("Z_M_CHANGE_BLOCKREG_001.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

    End Function


    Public Function ChangeRegel(ByVal SAPTable As DataTable) As DataTable

        Try
            S.AP.Execute()

            m_tblResult = S.AP.GetExportTable("GT_WEB")

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Speichern.<br>(" & ex.Message & ")"
            End Select

            m_tblResult = Nothing

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

        End Try

        Return m_tblResult

    End Function


    Public Function SearchGesperrteFahrzeuge() As DataTable

        Try

            m_tblResult = S.AP.GetExportTableWithInitExecute("Z_M_READ_GESP_FZG_001.GT_WEB",
                                                                "I_KUNNR_AG, I_USER_ANL_BREG, I_CARPORT, I_HERST_NUMMER, I_MODELLGRUPPE, I_KRAFTSTOFF, I_NAVIGATION, I_REIFENART, I_AUFBAUART, I_TYP, I_LIEFERANT",
                                                                m_objUser.KUNNR.ToSapKunnr(), "", Carport, Hersteller, Modellgruppe, Kraftstoff, NAVI, Reifenart, Aufbauart, Typ, mHaendlernr)

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Abfragen der Daten.<br>(" & ex.Message & ")"
            End Select

            m_tblResult = Nothing

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

        End Try

        Return m_tblResult

    End Function

    Public Function SearchRegeln() As DataTable

        Try

            Dim SAPTable As DataTable = S.AP.GetExportTableWithInitExecute("Z_M_READ_BLOCKREG_001.GT_WEB",
                                                                     "I_KUNNR_AG, I_USER_ANL_BREG, I_CARPORT, I_HERST_NUMMER, I_MODELLGRUPPE, I_KRAFTSTOFF, I_NAVIGATION, I_REIFENART, I_AUFBAUART, I_TYP, I_LIEFERANT",
                                                                     m_objUser.KUNNR.ToSapKunnr(), "", Carport, Hersteller, Modellgruppe, Kraftstoff, NAVI, Reifenart, Aufbauart, Typ, mHaendlernr)


            'Datum umwandeln
            Dim Row As DataRow

            Dim SAPTableFormat As DataTable = SAPTable.Clone()

            SAPTableFormat.Columns("DAT_SPERR_AB").DataType = GetType(System.String)
            SAPTableFormat.Columns("DAT_SPERR_BIS").DataType = GetType(System.String)

            Dim newRow As DataRow


            If SAPTable.Rows.Count > 0 Then

                For Each Row In SAPTable.Rows

                    newRow = SAPTableFormat.NewRow()

                    For Each dc As DataColumn In SAPTable.Columns

                        If dc.ColumnName = "DAT_SPERR_AB" Then
                            If IsDBNull(Row("DAT_SPERR_AB")) OrElse Not IsDate(Row("DAT_SPERR_AB")) OrElse CType(Row("DAT_SPERR_AB"), DateTime).Year < 1901 Then
                                newRow("DAT_SPERR_AB") = ""
                            Else
                                newRow("DAT_SPERR_AB") = CType(Row("DAT_SPERR_AB"), DateTime).ToShortDateString()
                            End If
                        ElseIf dc.ColumnName = "DAT_SPERR_BIS" Then
                            If IsDBNull(Row("DAT_SPERR_BIS")) OrElse Not IsDate(Row("DAT_SPERR_BIS")) OrElse CType(Row("DAT_SPERR_BIS"), DateTime).Year < 1901 Then
                                newRow("DAT_SPERR_BIS") = ""
                            Else
                                newRow("DAT_SPERR_BIS") = CType(Row("DAT_SPERR_BIS"), DateTime).ToShortDateString()
                            End If
                        Else
                            newRow(dc.ColumnName) = Row(dc.ColumnName)
                        End If

                    Next

                    SAPTableFormat.Rows.Add(newRow)

                Next

            End If

            SAPTable = SAPTableFormat

            m_tblResult = SAPTable

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Abfragen der Daten.<br>(" & ex.Message & ")"
            End Select

            m_tblResult = Nothing

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

        End Try

        Return m_tblResult

    End Function

    Public Function GetFreigabenSaveTable() As DataTable

        Return S.AP.GetImportTableWithInit("Z_M_GESP_FZG_FREIG_001.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

    End Function

    Public Sub SaveFreigaben()

        Try

            S.AP.Execute()

            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555

            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Speichern.<br>(" & ex.Message & ")"
            End Select

            m_tblResult = Nothing

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

        End Try
    End Sub

#End Region

End Class
