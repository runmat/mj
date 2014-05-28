Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Imports SapORM.Contracts

Public Class UploadZulassung

    Inherits Base.Business.BankBase

#Region " Declarations"
    Private mZulassungsTabelle As DataTable
    Private mSAPTabelle As DataTable
    Private mSAPTabelleZulassung As DataTable
    Private m_DezZul As Boolean
#End Region

#Region " Properties"

    Public ReadOnly Property SAPTabelle() As DataTable
        Get
            If mSAPTabelle Is Nothing Then

                Dim pI_I_DEZ As String
                If DezZul Then
                    pI_I_DEZ = "X"
                Else
                    pI_I_DEZ = " "
                End If

                mSAPTabelle = S.AP.GetImportTableWithInit("Z_M_FAHRGNR_READ_FZGPOOL_006.GT_WEB",
                                                          "I_KUNNR_AG, I_DEZ",
                                                          m_objUser.KUNNR.ToSapKunnr(), pI_I_DEZ)

                'mSAPTabelle = New DataTable
                'With mSAPTabelle
                '    .Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
                '    .Columns.Add("MVA_NUMMER", System.Type.GetType("System.String"))
                '    .Columns.Add("ZULDAT", System.Type.GetType("System.String"))

                '    .Columns.Add("HERST_NUMMER", System.Type.GetType("System.String"))
                '    .Columns.Add("MAKE_CODE", System.Type.GetType("System.String"))
                '    .Columns.Add("ZZMODELL", System.Type.GetType("System.String"))

                '    .Columns.Add("ZZBEZEI", System.Type.GetType("System.String"))
                '    .Columns.Add("GEPL_LIEFTERMIN", System.Type.GetType("System.String"))
                '    .Columns.Add("REIFENART", System.Type.GetType("System.String"))

                '    .Columns.Add("BEZAHLTKENNZ", System.Type.GetType("System.String"))
                '    .Columns.Add("ZULASSUNGSORT", System.Type.GetType("System.String"))
                '    .Columns.Add("VERWENDUNGSZWECK", System.Type.GetType("System.String"))

                '    .Columns.Add("DAT_SPERRE", System.Type.GetType("System.String"))
                '    .Columns.Add("SPERRVERMERK", System.Type.GetType("System.String"))
                '    .Columns.Add("EQUNR", System.Type.GetType("System.String"))

                '    .Columns.Add("QMNUM", System.Type.GetType("System.String"))
                '    .Columns.Add("ZZCARPORT", System.Type.GetType("System.String"))
                '    .Columns.Add("STATUS", System.Type.GetType("System.String"))
                'End With
                'mSAPTabelle.AcceptChanges()
                '    Return mSAPTabelle
                'Else
                '    Return mSAPTabelle
            End If

            Return mSAPTabelle
        End Get
    End Property

    Public ReadOnly Property SAPTabelleZulassung() As DataTable
        Get
            If mSAPTabelleZulassung Is Nothing Then

                mSAPTabelleZulassung = S.AP.GetImportTableWithInit("Z_M_MASSENZULASSUNG_006.GT_WEB",
                                                                   "I_KUNNR_AG, I_WEB_USER",
                                                                   m_objUser.KUNNR.ToSapKunnr(), m_objUser.UserName.Left(40))

                'mSAPTabelleZulassung = New DataTable
                'With mSAPTabelleZulassung
                '    .Columns.Add("AKTION", System.Type.GetType("System.String"))
                '    .Columns.Add("CHASSIS_NUM", System.Type.GetType("System.String"))
                '    .Columns.Add("LICENSE_NUM", System.Type.GetType("System.String"))

                '    .Columns.Add("ZULASSUNGSORT", System.Type.GetType("System.String"))
                '    .Columns.Add("VERWENDUNGSZWECK", System.Type.GetType("System.String"))
                '    .Columns.Add("REIFENART", System.Type.GetType("System.String"))

                '    .Columns.Add("ZZCARPORT", System.Type.GetType("System.String"))
                '    .Columns.Add("EQUNR", System.Type.GetType("System.String"))
                '    .Columns.Add("QMNUM", System.Type.GetType("System.String"))

                '    .Columns.Add("ZULDAT", System.Type.GetType("System.String"))
                '    .Columns.Add("DAT_SPERRE", System.Type.GetType("System.String"))
                '    .Columns.Add("SPERRVERMERK", System.Type.GetType("System.String"))

                '    .Columns.Add("WEB_USER", System.Type.GetType("System.String"))
                '    .Columns.Add("FEHLER", System.Type.GetType("System.String"))

                'End With
                'mSAPTabelleZulassung.AcceptChanges()

                '    Return mSAPTabelleZulassung
                'Else
                '    Return mSAPTabelleZulassung
            End If

            Return mSAPTabelleZulassung
        End Get
    End Property

    Public ReadOnly Property ZulassungsTabelle() As DataTable
        Get
            If mZulassungsTabelle Is Nothing Then
                mZulassungsTabelle = New DataTable
                With mZulassungsTabelle
                    .Columns.Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                    .Columns.Add("MVANummer", System.Type.GetType("System.String"))
                    .Columns.Add("Zulassungsdatum", System.Type.GetType("System.String"))
                    .Columns.Add("Herstellernummer", System.Type.GetType("System.String"))
                    .Columns.Add("Modell", System.Type.GetType("System.String"))
                    .Columns.Add("Modellbezeichnung", System.Type.GetType("System.String"))
                    .Columns.Add("geplanterLiefertermin", System.Type.GetType("System.String"))
                    .Columns.Add("istbezahlt", System.Type.GetType("System.String"))
                    .Columns.Add("Sperrdatum", System.Type.GetType("System.String"))
                    .Columns.Add("Sperrvermerk", System.Type.GetType("System.String"))
                    .Columns.Add("STATUS", System.Type.GetType("System.String"))
                End With
                mZulassungsTabelle.AcceptChanges()
                '    Return mZulassungsTabelle
                'Else
                '    Return mZulassungsTabelle
            End If

            Return mZulassungsTabelle
        End Get
    End Property

    Public Property DezZul() As Boolean
        Get
            Return m_DezZul
        End Get
        Set(ByVal value As Boolean)
            m_DezZul = value
        End Set
    End Property


#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overloads Overrides Sub change()
        m_strClassAndMethod = "UploadZulassung.Change"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            m_intStatus = 0


            'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            'con.Open()
            Try

                'Dim cmd As New SAPCommand()
                'cmd.Connection = con

                'Dim strCom As String

                'strCom = "EXEC Z_M_MASSENZULASSUNG_006 @I_KUNNR_AG=@pI_KUNNR_AG,@I_WEB_USER=@pI_WEB_USER,"
                'strCom = strCom & "@GT_WEB=@pI_GT_WEB,@GT_WEB=@pE_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                'cmd.CommandText = strCom


                ''importparameter
                'Dim pI_KUNNR_AG As New SAPParameter("@pI_KUNNR_AG", ParameterDirection.Input)
                'Dim pI_WEB_USER As New SAPParameter("@pI_WEB_USER", ParameterDirection.Input)
                'Dim pI_GT_WEB As New SAPParameter("@pI_GT_WEB", SAPTabelleZulassung)

                ''exportParameter
                'Dim pE_GT_WEB As New SAPParameter("@pE_GT_WEB", ParameterDirection.Output)

                'Importparameter hinzufügen
                'cmd.Parameters.Add(pI_KUNNR_AG)
                'cmd.Parameters.Add(pI_WEB_USER)
                'cmd.Parameters.Add(pI_GT_WEB)


                ''exportparameter hinzugfügen
                'cmd.Parameters.Add(pE_GT_WEB)

                ''befüllen der Importparameter
                'pI_KUNNR_AG.Value = Right("0000000000" & m_objUser.KUNNR, 10)
                'pI_WEB_USER.Value = Left(m_objUser.UserName, 40)
                'pI_GT_WEB.Value = SAPTabelleZulassung

                If Not SAPTabelleZulassung Is Nothing Then
                    SAPTabelleZulassung.Clear()
                End If

                Dim tmpSAPRow As DataRow
                Dim tmpNewRowZulassung As DataRow
                'For Each tmpRow As DataRow In mZulassungsTabelle.Select("STATUS='Für Zulassung bereit'")
                For Each tmpRow As DataRow In ZulassungsTabelle.Select("STATUS='Für Zulassung bereit'")
                    tmpNewRowZulassung = SAPTabelleZulassung.NewRow
                    'tmpSAPRow = mSAPTabelle.Select("CHASSIS_NUM='" & tmpRow("Fahrgestellnummer").ToString & "'")(0)
                    tmpSAPRow = SAPTabelle.Select("CHASSIS_NUM='" & tmpRow("Fahrgestellnummer").ToString & "'")(0)

                    tmpNewRowZulassung("CHASSIS_NUM") = tmpSAPRow("CHASSIS_NUM").ToString
                    'tmpNewRowZulassung("ZULDAT") = tmpSAPRow("ZULDAT").ToString
                    tmpNewRowZulassung("ZULDAT") = tmpSAPRow("ZULDAT").ToString.NotEmptyOrDbNull
                    tmpNewRowZulassung("REIFENART") = tmpSAPRow("REIFENART").ToString
                    tmpNewRowZulassung("ZULASSUNGSORT") = tmpSAPRow("ZULASSUNGSORT").ToString
                    tmpNewRowZulassung("VERWENDUNGSZWECK") = tmpSAPRow("VERWENDUNGSZWECK").ToString
                    'tmpNewRowZulassung("DAT_SPERRE") = tmpSAPRow("DAT_SPERRE").ToString
                    tmpNewRowZulassung("DAT_SPERRE") = tmpSAPRow("DAT_SPERRE").ToString.NotEmptyOrDbNull
                    tmpNewRowZulassung("SPERRVERMERK") = tmpSAPRow("SPERRVERMERK").ToString
                    tmpNewRowZulassung("EQUNR") = tmpSAPRow("EQUNR").ToString
                    tmpNewRowZulassung("QMNUM") = tmpSAPRow("QMNUM").ToString
                    tmpNewRowZulassung("ZZCARPORT") = tmpSAPRow("ZZCARPORT").ToString

                    tmpNewRowZulassung("AKTION") = "Z" 'Zulassung
                    tmpNewRowZulassung("WEB_USER") = Left(m_objUser.UserName, 15)

                    SAPTabelleZulassung.Rows.Add(tmpNewRowZulassung)
                Next
                SAPTabelleZulassung.AcceptChanges()
                'HelpProcedures.killAllDBNullValuesInDataTable(SAPTabelleZulassung)

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_MASSENZULASSUNG_006", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                'cmd.ExecuteNonQuery()
                S.AP.Execute()

                'mSAPTabelleZulassung = DirectCast(pE_GT_WEB.Value, DataTable)
                'HelpProcedures.killAllDBNullValuesInDataTable(mSAPTabelleZulassung)
                mSAPTabelleZulassung = S.AP.GetExportTable("GT_WEB")


                'nur fehlerhafte sätze wurden zurückgeliefert
                'jetzt müssen alle stadien die wirklich übertragen worden waren, ermal den status ok bekommen
                For Each tmpRow As DataRow In ZulassungsTabelle.Select("STATUS='Für Zulassung bereit'")
                    tmpRow("STATUS") = "Zulassung erfolgreich beauftragt."
                Next

                ''alle fehlerhaften Datensätze werden zurückgeliefert, status dort Ändern
                'For Each tmpRow As DataRow In mSAPTabelleZulassung.Rows
                '    mZulassungsTabelle.Select("Fahrgestellnummer='" & tmpRow("CHASSIS_NUM").ToString & "'")(0)("STATUS") = "Zulassungsbeauftragung fehlgeschlagen: " & tmpRow("FEHLER").ToString
                'Next

                'mZulassungsTabelle.AcceptChanges()

                'alle fehlerhaften Datensätze werden zurückgeliefert, status dort Ändern
                For Each tmpRow As DataRow In SAPTabelleZulassung.Rows
                    ZulassungsTabelle.Select("Fahrgestellnummer='" & tmpRow("CHASSIS_NUM").ToString & "'")(0)("STATUS") = "Zulassungsbeauftragung fehlgeschlagen: " & tmpRow("FEHLER").ToString
                Next

                ZulassungsTabelle.AcceptChanges()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If
            Catch ex As Exception
                mZulassungsTabelle = Nothing
                m_intStatus = -9999
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                'con.Close()

                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub SaveDataDez(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        'Dim cmd As New SAPCommand()
        'Dim strKunnr As String = ""
        'Dim sAktion As String = ""
        'm_strClassAndMethod = "UploadZulassung.SaveDataDez"
        'm_strAppID = strAppID
        'm_strSessionID = strSessionID
        'strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Dim intID As Int32 = -1

        Try

            m_intStatus = 0
            m_strMessage = ""


            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_DEZ_ZUL_001", m_objApp, m_objUser, page)
            'myProxy.setImportParameter("I_KUNNR_AG", strKunnr)

            'mSAPTabelleZulassung = myProxy.getImportTable("GT_WEB")

            mSAPTabelleZulassung = S.AP.GetImportTableWithInit("Z_DPM_DEZ_ZUL_001.GT_WEB", "I_KUNNR_AG", m_objUser.KUNNR.ToSapKunnr())


            Dim tmpSAPRow As DataRow
            Dim tmpNewRowZulassung As DataRow
            'For Each tmpRow As DataRow In mZulassungsTabelle.Select("STATUS='Für Zulassung bereit'")
            For Each tmpRow As DataRow In ZulassungsTabelle.Select("STATUS='Für Zulassung bereit'")
                tmpNewRowZulassung = SAPTabelleZulassung.NewRow
                'tmpSAPRow = mSAPTabelle.Select("CHASSIS_NUM='" & tmpRow("Fahrgestellnummer").ToString & "'")(0)
                tmpSAPRow = SAPTabelle.Select("CHASSIS_NUM='" & tmpRow("Fahrgestellnummer").ToString & "'")(0)

                tmpNewRowZulassung("CHASSIS_NUM") = tmpSAPRow("CHASSIS_NUM").ToString
                tmpNewRowZulassung("ZULDAT") = MakeDateStandard(tmpSAPRow("ZULDAT").ToString)
                tmpNewRowZulassung("REIFENART") = tmpSAPRow("REIFENART").ToString
                tmpNewRowZulassung("ZULASSUNGSORT") = tmpSAPRow("ZULASSUNGSORT").ToString
                tmpNewRowZulassung("VERWENDUNGSZWECK") = tmpSAPRow("VERWENDUNGSZWECK").ToString
                tmpNewRowZulassung("DAT_SPERRE") = MakeDateStandard(tmpSAPRow("DAT_SPERRE").ToString)
                tmpNewRowZulassung("SPERRVERMERK") = tmpSAPRow("SPERRVERMERK").ToString
                tmpNewRowZulassung("EQUNR") = tmpSAPRow("EQUNR").ToString
                tmpNewRowZulassung("QMNUM") = tmpSAPRow("QMNUM").ToString
                tmpNewRowZulassung("ZZCARPORT") = tmpSAPRow("ZZCARPORT").ToString

                tmpNewRowZulassung("AKTION") = "Z" 'Zulassung
                tmpNewRowZulassung("WEB_USER") = Left(m_objUser.UserName, 15)


                SAPTabelleZulassung.Rows.Add(tmpNewRowZulassung)
            Next
            SAPTabelleZulassung.AcceptChanges()

            'myProxy.callBapi()
            S.AP.Execute()

            'mSAPTabelleZulassung = myProxy.getExportTable("GT_WEB")
            'HelpProcedures.killAllDBNullValuesInDataTable(mSAPTabelleZulassung)
            mSAPTabelleZulassung = S.AP.GetExportTable("GT_WEB")


            'nur fehlerhafte sätze wurden zurückgeliefert
            'jetzt müssen alle stadien die wirklich übertragen worden waren, ermal den status ok bekommen
            'For Each tmpRow As DataRow In mZulassungsTabelle.Select("STATUS='Für Zulassung bereit'")
            For Each tmpRow As DataRow In ZulassungsTabelle.Select("STATUS='Für Zulassung bereit'")
                tmpRow("STATUS") = "Zulassung erfolgreich beauftragt."
            Next

            'alle fehlerhaften Datensätze werden zurückgeliefert, status dort Ändern
            'For Each tmpRow As DataRow In mSAPTabelleZulassung.Rows
            For Each tmpRow As DataRow In SAPTabelleZulassung.Rows
                'mZulassungsTabelle.Select("Fahrgestellnummer='" & tmpRow("CHASSIS_NUM").ToString & "'")(0)("STATUS") = "Zulassungsbeauftragung fehlgeschlagen: " & tmpRow("FEHLER").ToString
                ZulassungsTabelle.Select("Fahrgestellnummer='" & tmpRow("CHASSIS_NUM").ToString & "'")(0)("STATUS") = "Zulassungsbeauftragung fehlgeschlagen: " & tmpRow("FEHLER").ToString
            Next

            'mSAPTabelleZulassung.AcceptChanges()
            SAPTabelleZulassung.AcceptChanges()

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        Finally
            m_blnGestartet = False
        End Try
    End Sub

    Public Sub generateZulassungsTable(ByVal tmpZulassungsTable As DataTable)
        Dim tmpNewRow As DataRow

        If Not ZulassungsTabelle Is Nothing Then
            ZulassungsTabelle.Clear()
        End If

        For Each tmpRow As DataRow In tmpZulassungsTable.Rows
            tmpNewRow = ZulassungsTabelle.NewRow


            Dim fehlerHafteRow As Boolean = False
            If Not tmpZulassungsTable.Rows(0) Is tmpRow Then
                'nicht die überschriftsspalte

                If tmpRow(0).ToString.Length > 0 OrElse tmpRow(1).ToString.Length > 0 Then

                    If tmpRow(0).ToString.Length = 17 Then
                        tmpNewRow("Fahrgestellnummer") = tmpRow(0).ToString
                        tmpNewRow("MVANummer") = tmpRow(1).ToString
                    ElseIf tmpRow(1).ToString.Length = 8 Then
                        tmpNewRow("MVANummer") = tmpRow(1).ToString
                        'muss trotzdem hinzugefügt werden, da auf die fgsn ein select getätigt wird
                        tmpNewRow("Fahrgestellnummer") = tmpRow(0).ToString
                    Else
                        'muss trotzdem hinzugefügt werden, da auf die fgsn ein select getätigt wird
                        tmpNewRow("Fahrgestellnummer") = tmpRow(0).ToString
                        fehlerHafteRow = True
                    End If


                    If tmpRow(2).ToString.Length > 0 AndAlso IsDate(tmpRow(2).ToString) Then
                        tmpNewRow("Zulassungsdatum") = CDate(tmpRow(2).ToString).ToShortDateString
                    Else
                        fehlerHafteRow = True
                    End If

                    If fehlerHafteRow Then
                        tmpNewRow("Status") = "Fehlerhafte Uploaddatei, Fahrzeug wird ignoriert"
                        ZulassungsTabelle.Rows.Add(tmpNewRow)
                    Else
                        ZulassungsTabelle.Rows.Add(tmpNewRow)
                    End If
                Else
                    'nichts tun
                End If
            End If

        Next
        ZulassungsTabelle.AcceptChanges()
        HelpProcedures.killAllDBNullValuesInDataTable(ZulassungsTabelle)
    End Sub

    Public Overrides Sub Show()
        m_strClassAndMethod = "UploadZulassung.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Dim intID As Int32 = -1
            m_intStatus = 0


            'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            'con.Open()
            Try

                'Dim cmd As New SAPCommand()
                'cmd.Connection = con

                'Dim strCom As String

                'strCom = "EXEC Z_M_FAHRGNR_READ_FZGPOOL_006 @I_KUNNR_AG=@pI_KUNNR_AG,@I_DEZ=@pI_I_DEZ, "
                'strCom = strCom & "@GT_WEB=@pI_GT_WEB,@GT_WEB=@pE_GT_WEB OUTPUT OPTION 'disabledatavalidation'"

                'cmd.CommandText = strCom


                ''importparameter
                'Dim pI_KUNNR_AG As New SAPParameter("@pI_KUNNR_AG", ParameterDirection.Input)
                'Dim pI_I_DEZ As New SAPParameter("@pI_I_DEZ", ParameterDirection.Input)

                'Dim pI_GT_WEB As New SAPParameter("@pI_GT_WEB", SAPTabelle)

                ''exportParameter
                'Dim pE_GT_WEB As New SAPParameter("@pE_GT_WEB", ParameterDirection.Output)

                ''Importparameter hinzufügen
                'cmd.Parameters.Add(pI_KUNNR_AG)

                'cmd.Parameters.Add(pI_I_DEZ)
                'cmd.Parameters.Add(pI_GT_WEB)


                ''exportparameter hinzugfügen
                'cmd.Parameters.Add(pE_GT_WEB)

                ''befüllen der Importparameter
                'pI_KUNNR_AG.Value = Right("0000000000" & m_objUser.KUNNR, 10)
                'If DezZul Then
                '    pI_I_DEZ.Value = "X"
                'Else
                '    pI_I_DEZ.Value = " "
                'End If
                'pI_GT_WEB.Value = SAPTabelle

                If Not SAPTabelle Is Nothing Then
                    SAPTabelle.Clear()
                End If


                Dim tmpNewRow As DataRow
                'For Each tmpRow As DataRow In mZulassungsTabelle.Select("STATUS<>'Fehlerhafte Uploaddatei, Fahrzeug wird ignoriert'")
                For Each tmpRow As DataRow In ZulassungsTabelle.Select("STATUS<>'Fehlerhafte Uploaddatei, Fahrzeug wird ignoriert'")
                    tmpNewRow = SAPTabelle.NewRow


                    'tmpNewRow("ZULDAT") = MakeDateSAP(tmpRow("Zulassungsdatum").ToString)
                    tmpNewRow("ZULDAT") = tmpRow("Zulassungsdatum").ToString.NotEmptyOrDbNull
                    tmpNewRow("CHASSIS_NUM") = tmpRow("Fahrgestellnummer").ToString
                    tmpNewRow("MVA_NUMMER") = tmpRow("MVANummer").ToString
                    SAPTabelle.Rows.Add(tmpNewRow)

                Next
                SAPTabelle.AcceptChanges()
                'HelpProcedures.killAllDBNullValuesInDataTable(SAPTabelle)


                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_FAHRGNR_READ_FZGPOOL_006", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                'cmd.ExecuteNonQuery()
                S.AP.Execute()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                'mSAPTabelle = DirectCast(pE_GT_WEB.Value, DataTable)
                'HelpProcedures.killAllDBNullValuesInDataTable(mSAPTabelle)
                mSAPTabelle = S.AP.GetExportTable("GT_WEB")

                Dim tmpRowSAP As DataRow
                'For Each tmpRow As DataRow In mZulassungsTabelle.Select("STATUS<>'Fehlerhafte Uploaddatei, Fahrzeug wird ignoriert'")
                For Each tmpRow As DataRow In ZulassungsTabelle.Select("STATUS<>'Fehlerhafte Uploaddatei, Fahrzeug wird ignoriert'")

                    'entweder nach MVA-Nummer oder FAhrgestellnummer zuordnen, je nach mit was sie die Daten ergänzt haben

                    If SAPTabelle.Select("CHASSIS_NUM='" & tmpRow("Fahrgestellnummer").ToString & "'").Length = 1 Then
                        tmpRowSAP = SAPTabelle.Select("CHASSIS_NUM='" & tmpRow("Fahrgestellnummer").ToString & "'")(0)
                    ElseIf SAPTabelle.Select("MVA_NUMMER='" & tmpRow("MVANummer").ToString & "'").Length = 1 Then
                        tmpRowSAP = SAPTabelle.Select("MVA_NUMMER='" & tmpRow("MVANummer").ToString & "'")(0)
                    Else
                        Throw New Exception("eines der Angefragen Fahrzeuge konnte nicht eindeutig zugeordnet werden")
                    End If

                    If tmpRowSAP("BEZAHLTKENNZ").ToString = "X" Then
                        tmpRow("istbezahlt") = "Ja"
                    Else
                        tmpRow("istbezahlt") = "Nein"
                    End If

                    Select Case tmpRowSAP("STATUS").ToString
                        Case "1"
                            tmpRow("STATUS") = "Für Zulassung bereit"
                        Case "2"
                            tmpRow("STATUS") = "Fahrzeug bereits zugelassen"
                        Case "3"
                            tmpRow("STATUS") = "Fahrzeug gesperrt"
                        Case "4"
                            tmpRow("STATUS") = "Fahrzeug nicht zulassungbreit"
                        Case "5"
                            tmpRow("STATUS") = "Dezentrale Zulassung"
                        Case "6"
                            tmpRow("STATUS") = "Navi CD liegt nicht vor"
                        Case "7"
                            tmpRow("STATUS") = "Zentrale Zulassung"
                        Case Else
                            tmpRow("STATUS") = "Unbekannter Status"
                    End Select

                    tmpRow("Fahrgestellnummer") = tmpRowSAP("CHASSIS_NUM").ToString
                    tmpRow("MVANummer") = tmpRowSAP("MVA_NUMMER").ToString
                    tmpRow("Sperrdatum") = If(MakeDateStandard(tmpRowSAP("DAT_SPERRE").ToString).ToShortDateString = "01.01.1900", "", MakeDateStandard(tmpRowSAP("DAT_SPERRE").ToString).ToShortDateString)
                    tmpRow("Zulassungsdatum") = If(MakeDateStandard(tmpRowSAP("ZULDAT").ToString).ToShortDateString = "01.01.1900", "", MakeDateStandard(tmpRowSAP("ZULDAT").ToString).ToShortDateString)
                    tmpRow("Herstellernummer") = tmpRowSAP("HERST_NUMMER").ToString
                    tmpRow("Modell") = tmpRowSAP("ZZMODELL").ToString
                    tmpRow("Modellbezeichnung") = tmpRowSAP("ZZBEZEI").ToString
                    tmpRow("geplanterLiefertermin") = tmpRowSAP("GEPL_LIEFTERMIN").ToString
                    If tmpRowSAP("SPERRVERMERK").ToString = "X" Then
                        tmpRow("Sperrvermerk") = "Ja"
                    Else
                        tmpRow("Sperrvermerk") = "Nein"
                    End If
                Next
                'mZulassungsTabelle.AcceptChanges()
                ZulassungsTabelle.AcceptChanges()


            Catch ex As Exception
                mZulassungsTabelle = Nothing
                m_intStatus = -9999
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                End If
            Finally
                If intID > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(intID)
                End If

                'con.Close()

                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region

End Class

' ************************************************
' $History: UploadZulassung.vb $
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 6.05.10    Time: 14:15
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA: 3696
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 25.11.09   Time: 11:10
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 30.10.09   Time: 11:14
' Updated in $/CKAG/Applications/AppAvis/lib
' ita´s: 3216, 3155
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 3.04.09    Time: 13:04
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2758 testfertig
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 31.03.09   Time: 11:48
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2758
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 26.03.09   Time: 9:08
' Updated in $/CKAG/Applications/AppAvis/lib
' BugFix
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 23.03.09   Time: 8:34
' Updated in $/CKAG/Applications/AppAvis/lib
' ita 2739
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 20.03.09   Time: 15:33
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2739 unfertig
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 15.01.09   Time: 11:21
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2457
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 21.11.08   Time: 14:54
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2412 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 21.11.08   Time: 9:59
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2412 testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 20.11.08   Time: 14:08
' Created in $/CKAG/Applications/AppAvis/lib
' ITa 2412 torso
' 
' ************************************************