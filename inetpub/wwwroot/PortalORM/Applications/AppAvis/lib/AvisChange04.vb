Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts

Public Class AvisChange04

    Inherits CKG.Base.Business.DatenimportBase

#Region "Declarations"
    Private Enum Blocken
        Anlegen = 1
        FreigebenBeauftragen = 2
        Bearbeiten = 3
    End Enum

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

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        Dim intID As Int32 = -1

        Try
            'strCom = "EXEC Z_M_READ_LISTBOX_BR_006 @I_KUNNR='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '            & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom

            ''Exportparameter
            'Dim pSAPTable As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTable)


            'cmd.ExecuteNonQuery()

            'Dim SAPTable As DataTable = DirectCast(pSAPTable.Value, DataTable)
            Dim SAPTable As DataTable = S.AP.GetExportTableWithInitExecute("Z_M_READ_LISTBOX_BR_006.GT_WEB", "I_KUNNR", m_objUser.KUNNR.ToSapKunnr)



            m_tblResult = SAPTable

            Dim row As DataRow


            For Each row In m_tblResult.Rows

                If row("KENNUNG").ToString = "EINBAUFIRMA" Then
                    row("POS_TEXT") = row("POS_KURZTEXT").ToString & " - " & row("POS_TEXT").ToString

                End If

            Next

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, _
                                                        "Z_M_READ_LISTBOX_BR_006", m_strAppID, m_strSessionID, _
                                                        m_objUser.CurrentLogAccessASPXID)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

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

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            'Throw New Exception(m_strMessage)

        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'con.Close()
            'con.Dispose()

            'cmd.Dispose()

        End Try

        DropDownTable = m_tblResult


    End Sub

    Public Sub SaveRegel()

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        Dim intID As Int32 = -1

        Try
            'strCom = "EXEC Z_M_CHANGE_BLOCKREG_001 @I_KUNNR_AG='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '            & "@GT_WEB=@pSAPTable,@GT_WEB=@pSAPTableEx OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom

            'Dim SAPTable As New DataTable

            S.AP.Init("Z_M_CHANGE_BLOCKREG_001", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

            Dim SAPTable As DataTable = S.AP.GetImportTable("GT_WEB")



            'With SAPTable

            '    .Columns.Add("ID_BLOCK_RG", System.Type.GetType("System.String"))
            '    .Columns.Add("DAT_STOR_BREG", System.Type.GetType("System.String"))
            '    .Columns.Add("UZEIT_STOR_BREG", System.Type.GetType("System.String"))
            '    .Columns.Add("FARBE_DE", System.Type.GetType("System.String"))
            '    .Columns.Add("CARPORT", System.Type.GetType("System.String"))
            '    .Columns.Add("GEPL_LIEFTERMIN", System.Type.GetType("System.String"))
            '    .Columns.Add("HERST_NUMMER", System.Type.GetType("System.String"))
            '    .Columns.Add("MODELLGRUPPE", System.Type.GetType("System.String"))
            '    .Columns.Add("KRAFTSTOFF", System.Type.GetType("System.String"))
            '    .Columns.Add("NAVIGATION", System.Type.GetType("System.String"))
            '    .Columns.Add("REIFENART", System.Type.GetType("System.String"))
            '    .Columns.Add("AUFBAUART", System.Type.GetType("System.String"))
            '    .Columns.Add("TYP", System.Type.GetType("System.String"))
            '    .Columns.Add("DAT_SPERR_AB", System.Type.GetType("System.String"))
            '    .Columns.Add("DAT_SPERR_BIS", System.Type.GetType("System.String"))
            '    .Columns.Add("SPERRVERMERK", System.Type.GetType("System.String"))
            '    .Columns.Add("ANZ_FZG", System.Type.GetType("System.String"))
            '    .Columns.Add("WEB_USER", System.Type.GetType("System.String"))
            '    .Columns.Add("RETUR_BEM", System.Type.GetType("System.String"))
            '    .Columns.Add("LIEFERANT", System.Type.GetType("System.String"))
            'End With

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
            Row("DAT_SPERR_AB") = ChangeDate(SperreAb)
            Row("DAT_SPERR_BIS") = ChangeDate(SperreBis)
            Row("SPERRVERMERK") = Sperrvermerk
            Row("ANZ_FZG") = AnzFzge
            Row("WEB_USER") = m_objUser.UserName
            Row("RETUR_BEM") = ""
            Row("LIEFERANT") = mHaendlernr

            SAPTable.Rows.Add(Row)


            ''Importparameter
            'Dim pSAPTable As New SAPParameter("@pSAPTable", SAPTable)
            'cmd.Parameters.Add(pSAPTable)


            ''Exportparameter
            'Dim pSAPTableEx As New SAPParameter("@pSAPTableEx", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTableEx)





            'cmd.ExecuteNonQuery()
            S.AP.Execute()


            'Dim SAPTableEx As DataTable = DirectCast(pSAPTableEx.Value, DataTable)
            Dim SAPTableEx As DataTable = S.AP.GetExportTable("GT_WEB")

            If SAPTableEx.Rows.Count > 0 Then
                If Len(SAPTableEx.Rows(0)("RETUR_BEM").ToString) > 0 Then
                    Throw New Exception()
                End If
            End If


            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, _
                                                        "Z_M_CHANGE_BLOCKREG_001", m_strAppID, m_strSessionID, _
                                                        m_objUser.CurrentLogAccessASPXID)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
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

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Throw New Exception(m_strMessage)

        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'con.Close()
            'con.Dispose()

            'cmd.Dispose()

        End Try



    End Sub


    Public Function GetSaveTable() As DataTable

        Return S.AP.GetImportTableWithInit("Z_M_CHANGE_BLOCKREG_001.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

    End Function


    Public Function ChangeRegel(ByVal SAPTable As DataTable) As DataTable

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        Dim intID As Int32 = -1

        Try
            'strCom = "EXEC Z_M_CHANGE_BLOCKREG_001 @I_KUNNR_AG='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '            & "@GT_WEB=@pSAPTable,@GT_WEB=@SAPTableExport OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom


            ''Importparameter
            'Dim pSAPTable As New SAPParameter("@pSAPTable", SAPTable)
            'cmd.Parameters.Add(pSAPTable)

            ''Exportparameter
            'Dim SAPTableExport As New SAPParameter("@SAPTableExport", ParameterDirection.Output)
            'cmd.Parameters.Add(SAPTableExport)

            'cmd.ExecuteNonQuery()

            S.AP.Execute()

            'Dim SAPTableEx As DataTable = DirectCast(SAPTableExport.Value, DataTable)
            Dim SAPTableEx As DataTable = S.AP.GetExportTable("GT_WEB")

            m_tblResult = SAPTableEx

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, _
                                                        "Z_M_CHANGE_BLOCKREG_001", m_strAppID, m_strSessionID, _
                                                        m_objUser.CurrentLogAccessASPXID)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

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

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            'Throw New Exception(m_strMessage)

        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'con.Close()
            'con.Dispose()

            'cmd.Dispose()

        End Try

        Return m_tblResult

    End Function


    Public Function SearchGesperrteFahrzeuge() As DataTable

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        Dim intID As Int32 = -1

        Try
            'strCom = "EXEC Z_M_READ_GESP_FZG_001 @I_KUNNR_AG='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '        & "@I_USER_ANL_BREG=''," _
            '        & "@I_CARPORT='" & Carport & "'," _
            '        & "@I_HERST_NUMMER='" & Hersteller & "'," _
            '        & "@I_MODELLGRUPPE='" & Modellgruppe & "'," _
            '        & "@I_KRAFTSTOFF='" & Kraftstoff & "'," _
            '        & "@I_NAVIGATION='" & NAVI & "'," _
            '        & "@I_REIFENART='" & Reifenart & "'," _
            '        & "@I_AUFBAUART='" & Aufbauart & "'," _
            '        & "@I_TYP='" & Typ & "'," _
            '        & "@I_LIEFERANT='" & mHaendlernr & "'," _
            '        & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom

            ''Exportparameter
            'Dim pSAPTable As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTable)


            'cmd.ExecuteNonQuery()

            'Dim SAPTable As DataTable = DirectCast(pSAPTable.Value, DataTable)
            Dim SAPTable As DataTable = S.AP.GetExportTableWithInitExecute("Z_M_READ_GESP_FZG_001.GT_WEB",
                                                                     "I_KUNNR_AG, I_USER_ANL_BREG, I_CARPORT, I_HERST_NUMMER, I_MODELLGRUPPE, I_KRAFTSTOFF, I_NAVIGATION, I_REIFENART, I_AUFBAUART, I_TYP, I_LIEFERANT",
                                                                     m_objUser.KUNNR.ToSapKunnr(), "", Carport, Hersteller, Modellgruppe, Kraftstoff, NAVI, Reifenart, Aufbauart, Typ, mHaendlernr)

            Dim Row As DataRow

            If SAPTable.Rows.Count > 0 Then

                For Each Row In SAPTable.Rows

                    Row("ZZDAT_EIN") = MakeStandardDate(Row("ZZDAT_EIN").ToString)
                    Row("DAT_EING_ZBII") = MakeStandardDate(Row("DAT_EING_ZBII").ToString)
                    Row("DAT_SPERR_ANL") = MakeStandardDate(Row("DAT_SPERR_ANL").ToString)
                    Row("DAT_BEAUFTR") = MakeStandardDate(Row("DAT_BEAUFTR").ToString)

                Next

            End If


            m_tblResult = SAPTable

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, _
                                                        "Z_M_READ_GESP_FZG_001", m_strAppID, m_strSessionID, _
                                                        m_objUser.CurrentLogAccessASPXID)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

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

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            'Throw New Exception(m_strMessage)

        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'con.Close()
            'con.Dispose()

            'cmd.Dispose()

        End Try

        Return m_tblResult

    End Function

    Public Function SearchRegeln() As DataTable

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        Dim intID As Int32 = -1

        Try
            'strCom = "EXEC Z_M_READ_BLOCKREG_001 @I_KUNNR_AG='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '        & "@I_USER_ANL_BREG=''," _
            '        & "@I_CARPORT='" & Carport & "'," _
            '        & "@I_HERST_NUMMER='" & Hersteller & "'," _
            '        & "@I_MODELLGRUPPE='" & Modellgruppe & "'," _
            '        & "@I_KRAFTSTOFF='" & Kraftstoff & "'," _
            '        & "@I_NAVIGATION='" & NAVI & "'," _
            '        & "@I_REIFENART='" & Reifenart & "'," _
            '        & "@I_AUFBAUART='" & Aufbauart & "'," _
            '        & "@I_TYP='" & Typ & "'," _
            '        & "@GT_WEB=@pSAPTable OUTPUT OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom

            ''Exportparameter
            'Dim pSAPTable As New SAPParameter("@pSAPTable", ParameterDirection.Output)
            'cmd.Parameters.Add(pSAPTable)


            'cmd.ExecuteNonQuery()

            'Dim SAPTable As DataTable = DirectCast(pSAPTable.Value, DataTable)
            Dim SAPTable As DataTable = S.AP.GetExportTableWithInitExecute("Z_M_READ_BLOCKREG_001.GT_WEB",
                                                                     "I_KUNNR_AG, I_USER_ANL_BREG, I_CARPORT, I_HERST_NUMMER, I_MODELLGRUPPE, I_KRAFTSTOFF, I_NAVIGATION, I_REIFENART, I_AUFBAUART, I_TYP, I_LIEFERANT",
                                                                     m_objUser.KUNNR.ToSapKunnr(), "", Carport, Hersteller, Modellgruppe, Kraftstoff, NAVI, Reifenart, Aufbauart, Typ, mHaendlernr)


            'Datum umwandeln
            Dim Row As DataRow

            If SAPTable.Rows.Count > 0 Then

                For Each Row In SAPTable.Rows

                    Row("DAT_SPERR_AB") = MakeStandardDate(Row("DAT_SPERR_AB").ToString)
                    Row("DAT_SPERR_BIS") = MakeStandardDate(Row("DAT_SPERR_BIS").ToString)

                Next

            End If





            m_tblResult = SAPTable

            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, _
                                                        "Z_M_READ_BLOCKREG_001", m_strAppID, m_strSessionID, _
                                                        m_objUser.CurrentLogAccessASPXID)

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

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

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            'Throw New Exception(m_strMessage)

        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'con.Close()
            'con.Dispose()

            'cmd.Dispose()

        End Try

        Return m_tblResult

    End Function

    Public Function GetFreigabenSaveTable() As DataTable

        Return S.AP.GetImportTableWithInit("Z_M_GESP_FZG_FREIG_001.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())

    End Function

    Public Sub SaveFreigaben()
        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)

        'con.Open()
        'Dim cmd As New SAPCommand()
        'Dim strCom As String


        Dim intID As Int32 = -1

        Try
            'strCom = "EXEC Z_M_GESP_FZG_FREIG_001 @I_KUNNR_AG='" & Right("0000000000" & m_objUser.KUNNR, 10) & "'," _
            '            & "@GT_WEB=@pSAPTable OPTION 'disabledatavalidation'"

            'cmd.Connection = con
            'cmd.CommandText = strCom


            ''Importparameter
            'Dim pSAPTable As New SAPParameter("@pSAPTable", SAPTable)
            'cmd.Parameters.Add(pSAPTable)


            intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, _
                                                        "Z_M_GESP_FZG_FREIG_001", m_strAppID, m_strSessionID, _
                                                        m_objUser.CurrentLogAccessASPXID)

            'cmd.ExecuteNonQuery()
            S.AP.Execute()


            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, True)
            End If

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

            If intID > -1 Then
                m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
            End If

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            'Throw New Exception(m_strMessage)

        Finally
            If intID > -1 Then
                m_objLogApp.WriteStandardDataAccessSAP(intID)
            End If
            'con.Close()
            'con.Dispose()

            'cmd.Dispose()

        End Try
    End Sub


    Private Function MakeStandardDate(ByVal strInput As String) As String
        REM § Formt String-Input im SAP-Format in Standard-Date um. Gibt "01.01.1900" zurück, wenn Umwandlung nicht möglich ist.
        Dim strTemp As String = Right(strInput, 2) & "." & Mid(strInput, 5, 2) & "." & Left(strInput, 4)

        If IsDate(strTemp) Then
            Return strTemp
        Else
            Return String.Empty
        End If
    End Function

    Private Function ChangeDate(ByVal Datum As String) As Object
        'Dim Temp As String = String.Empty
        'If Datum = Nothing Then
        '    Temp = "00000000"
        'Else
        '    If Datum.Length = 10 Then
        '        Temp = Datum.Replace(".", "")
        '        Temp = Right(Temp, 4) & Mid(Temp, 3, 2) & Left(Temp, 2)
        '    Else
        '        Temp = "00000000"
        '    End If
        'End If

        'Return Temp

        Return Datum.NotEmptyOrDbNull

    End Function


#End Region





End Class
