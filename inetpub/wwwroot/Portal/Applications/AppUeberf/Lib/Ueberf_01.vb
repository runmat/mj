Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel

Public Class Ueberf_01
    Inherits Base.Business.DatenimportBase


#Region "Deklarationen"

    'Key für Speicherort des Objektes in der Session
    Public Const CONST_SESSION_UEBERFUEHRUNG As String = "Ueberf"

    'Je nach Beauftragungsart sollen im Frontend entsprechende Funktionalitäten gegeben sein.
    Public Enum Beauftragungsart
        ReineUeberfuehrung = 1
        ZulassungUndUeberfuehrung = 2
        OffeneUeberfuehrung = 3
        UeberfuehrungKCL = 4
        ZulassungKCL = 5
        ZulassungUndUeberfuehrungKCL = 6
    End Enum

    Public Enum AdressStatus
        Frei
        Gesperrt
        KopieVonLeasingnehmer
    End Enum


#End Region

#Region "Konstruktor"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

#End Region

    Public Function Save() As DataTable

        Dim SAPTable As DataTable
        Dim SAPTableRow As DataRow
        Dim SAPTableRet As DataTable = Nothing

        Dim strEinweisung As String = ""
        Dim strTanken As String = ""
        Dim strZugelassen As String = ""
        Dim strWaesche As String = ""
        Dim strMatnr As String = ""
        Dim strKennzeichen As String = Kenn1 & "-" & Kenn2
        Dim strLnKunnr As String = ""
        Dim strAbKunnr As String = ""
        Dim strAnKunnr As String = ""
        Dim strSoWi As String = ""
        Dim strReSoWi As String = ""
        Dim strReZugelassen As String = ""
        Dim strReKennzeichen As String = ""
        Dim strKunnr As String = Right("0000000000" & m_objUser.KUNNR, 10)
        Dim datUeberf As Date
        Dim strRotKenn As String = ""
        Dim strFix As String = ""
        Dim strReReifenUnbekannt As String = ""

        Try
            ' Parameter füllen
            ' ------------------------------------------------------
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Order_Ueberfuehrung_Leas", m_objApp, m_objUser, page)

            If Beauftragung = Beauftragungsart.ReineUeberfuehrung Then
                strVkorg = "1510"
            Else
                strVkorg = "1510"
            End If

            If booFzgEinweisung = True Then
                strEinweisung = "x"
            End If

            If booTanken = True Then
                strTanken = "x"
            End If

            If booWaesche = True Then
                strWaesche = "x"
            End If

            If booRotKenn = True Then
                strRotKenn = "x"
            End If

            If FixDatumUeberfuehrung Then
                strFix = "x"
            End If

            If FzgZugelassen = "Nein" Then
                strZugelassen = "x"
            End If

            If SomWin = "Winter" Then
                strSoWi = "X"
            ElseIf SomWin = "Ganzjahresreifen" Then
                strSoWi = "G"
            End If

            If Anschluss = True Then
                strMatnr = "000000000000002340"
            Else
                strMatnr = "000000000000001911"
            End If

            If DatumUeberf <> Date.MinValue Then
                datUeberf = DatumUeberf
            End If

            If LeasingnehmerKundennummer = "" Then
                strLnKunnr = strKunnr
            Else
                strLnKunnr = LeasingnehmerKundennummer
            End If

            If AdressStatusAbholung = AdressStatus.Frei OrElse AbKundennummer = "" Then
                strAbKunnr = strKunnr
            Else
                strAbKunnr = AbKundennummer
            End If

            If AdressStatusAnlieferung = AdressStatus.Frei OrElse AnKundennummer = "" Then
                strAnKunnr = strKunnr
            Else
                strAnKunnr = AnKundennummer
            End If

            If ReSomWin = "Winter" Then
                strReSoWi = "X"
            ElseIf ReSomWin = "Ganzjahresreifen" Then
                strReSoWi = "G"
            ElseIf ReSomWin = "Unbekannt" Then
                strReSoWi = "U"
                strReReifenUnbekannt = "Fahrzeug (Rücktour) Reifen unbekannt"
            End If

            If ReFzgZugelassen = "Nein" Then
                strReZugelassen = "x"
            End If

            If Anschluss = True Then
                strReKennzeichen = ReKenn1 & "-" & ReKenn2
            End If

            If Beauftragung = Beauftragungsart.ZulassungUndUeberfuehrung Or Beauftragung = Beauftragungsart.ZulassungUndUeberfuehrungKCL Then
                ZulAuftrag = "x"
            End If

            ' ------------------------------------------------------


            ' Tabellen füllen
            ' ------------------------------------------------------

            SAPTable = S.AP.GetImportTableWithInit("Z_M_Order_Ueberfuehrung_Leas.PARTNER")

            'Auftraggeber

            SAPTableRow = SAPTable.NewRow()

            SAPTableRow("PARTN_ROLE") = "AG"
            SAPTableRow("Itm_Number") = "000000"
            SAPTableRow("Partn_Numb") = strKunnr

            SAPTable.Rows.Add(SAPTableRow)

            ' ZL

            SAPTableRow = SAPTable.NewRow()

            SAPTableRow("PARTN_ROLE") = "ZL"
            SAPTableRow("Itm_Number") = "000000"
            SAPTableRow("Partn_Numb") = strLnKunnr
            SAPTableRow("Name") = LeasingnehmerAnsprechpartner
            SAPTableRow("Name_2") = LeasingnehmerAnsprechpartner
            SAPTableRow("Street") = LeasingnehmerStrasse & " " & LeasingnehmerHausnummer
            SAPTableRow("Postl_Code") = LeasingnehmerPLZ
            SAPTableRow("City") = LeasingnehmerOrt
            SAPTableRow("Telephone") = LeasingnehmerTelefon1
            SAPTableRow("Telephone2") = LeasingnehmerTelefon2

            SAPTable.Rows.Add(SAPTableRow)

            'Warenempfänger

            SAPTableRow = SAPTable.NewRow()

            SAPTableRow("PARTN_ROLE") = "WE"
            SAPTableRow("Itm_Number") = "000000"
            SAPTableRow("Partn_Numb") = strAnKunnr
            SAPTableRow("Name") = AnName
            SAPTableRow("Name_2") = AnAnsprechpartner
            SAPTableRow("Street") = AnStrasse & " " & AnNr
            SAPTableRow("Postl_Code") = AnPlz
            SAPTableRow("City") = AnOrt
            SAPTableRow("Telephone") = AnTelefon1
            SAPTableRow("Telephone2") = AnTelefon2

            SAPTable.Rows.Add(SAPTableRow)

            'Abholung

            SAPTableRow = SAPTable.NewRow()

            SAPTableRow("PARTN_ROLE") = "ZB"
            SAPTableRow("Itm_Number") = "000000"
            SAPTableRow("Partn_Numb") = strAbKunnr
            SAPTableRow("Name") = AbName
            SAPTableRow("Name_2") = AbAnsprechpartner
            SAPTableRow("Street") = AbStrasse & " " & AbNr
            SAPTableRow("Postl_Code") = AbPlz
            SAPTableRow("City") = AbOrt
            SAPTableRow("Telephone") = AbTelefon1
            SAPTableRow("Telephone2") = AbTelefon2

            SAPTable.Rows.Add(SAPTableRow)

            If Anschluss = True Then
                'Retour

                SAPTableRow = SAPTable.NewRow()

                SAPTableRow("PARTN_ROLE") = "ZR"
                SAPTableRow("Itm_Number") = "000000"
                SAPTableRow("Partn_Numb") = strKunnr
                SAPTableRow("Name") = ReName
                SAPTableRow("Name_2") = ReAnsprechpartner
                SAPTableRow("Street") = ReStrasse & " " & ReNr
                SAPTableRow("Postl_Code") = RePlz
                SAPTableRow("City") = ReOrt
                SAPTableRow("Telephone") = ReTelefon1
                SAPTableRow("Telephone2") = ReTelefon2

                SAPTable.Rows.Add(SAPTableRow)
            End If

            SAPTable.AcceptChanges()

            ' ------------------------------------------------------


            ' SAP Aufruf

            S.AP.SetImportParameter("KUNNR", strKunnr)
            S.AP.SetImportParameter("VKORG", strVkorg)
            S.AP.SetImportParameter("MATNR", strMatnr)
            S.AP.SetImportParameter("ZZBRIEF", Herst)
            S.AP.SetImportParameter("ZZKENN", strKennzeichen)
            S.AP.SetImportParameter("ZZREFNR", Ref)
            S.AP.SetImportParameter("ZZFAHRG", Vin)
            S.AP.SetImportParameter("VDATU", datUeberf)
            S.AP.SetImportParameter("ZULGE", strZugelassen)
            S.AP.SetImportParameter("WASCHEN", strWaesche)
            S.AP.SetImportParameter("TANKE", strTanken)
            S.AP.SetImportParameter("EINW", strEinweisung)
            S.AP.SetImportParameter("SOWIHIN", strSoWi)
            S.AP.SetImportParameter("ROTKENN", strRotKenn)
            S.AP.SetImportParameter("ZZBEZEI", ReHerst)
            S.AP.SetImportParameter("ZZKENNRUCK", strReKennzeichen)
            S.AP.SetImportParameter("ZZFAHRGRUCK", ReVin)
            S.AP.SetImportParameter("ZZREFNRRUCK", ReRef)
            S.AP.SetImportParameter("ZULGRUCK", strReZugelassen)
            S.AP.SetImportParameter("SOWIRUCK", strReSoWi)
            S.AP.SetImportParameter("BEMERKUNG", "")
            S.AP.SetImportParameter("AUGRU", SelFahrzeugwert)
            S.AP.SetImportParameter("EQUNR", Equipment)
            S.AP.SetImportParameter("I_VBELN", Auftragsnummer)
            S.AP.SetImportParameter("USER", m_objUser.UserName)
            S.AP.SetImportParameter("LFSPERR", ZulAuftrag)
            S.AP.SetImportParameter("BEMERKUNG02", Bemerkung)
            S.AP.SetImportParameter("BEMERKUNG03", ReBemerkung)
            S.AP.SetImportParameter("WINTER", WinterText)
            S.AP.SetImportParameter("FIX", strFix)
            S.AP.SetImportParameter("UNBKRUCK", strReReifenUnbekannt)
            S.AP.SetImportParameter("ZZFAHRZGTYP", Herst)
            S.AP.SetImportParameter("ZFZGKAT", "")
            S.AP.SetImportParameter("ZFZGKATRUCK", "")

            'myProxy.callBapi()
            S.AP.Execute()

            Vbeln = S.AP.GetExportParameter("VBELN")
            Vbeln1510 = S.AP.GetExportParameter("VBELN_1510")
            Qmnum = S.AP.GetExportParameter("QMNUM")

            'SAPTable = myProxy.getExportTable("PARTNER")
            SAPTableRet = S.AP.GetExportTable("RETURN")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        End Try

        Return SAPTableRet

    End Function

    Public Function getPartner(ByVal KunNr As String) As DataSets.AddressDataSet.ADDRESSEDataTable

        Dim intID As Int32 = -1
        Dim SAPTable As New DataTable

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Partner_Aus_Knvp_Lesen", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("KUNNR", Right("0000000000" & KunNr, 10))

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Partner_Aus_Knvp_Lesen", "KUNNR", Right("0000000000" & KunNr, 10))

            SAPTable = S.AP.GetExportTable("AUSGABE")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

        Dim res As New DataSets.AddressDataSet.ADDRESSEDataTable()

        For Each row As DataRow In SAPTable.Rows
            res.AddADDRESSERow(row("Name1").ToString(), row("Post_Code1").ToString(), row("City1").ToString(), row("Street").ToString(), _
                               row("Name1").ToString() + "_" + row("Post_Code1").ToString() + "_" + row("City1").ToString(), _
                               row("House_Num1").ToString(), row("Tel_Number").ToString(), "", row("Name2").ToString(), row("Parvw").ToString(), row("Kunnr").ToString(), "")
        Next

        Return res

    End Function

    Public Function InsertUeberfuehrung() As Boolean
        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ZULAUFTR_INSERT", m_objApp, m_objUser, page)

            Dim sapTable As DataTable = S.AP.GetImportTableWithInit("Z_M_ZULAUFTR_INSERT.GT_WEB")
            Dim dtRow As DataRow = sapTable.NewRow()


            dtRow("KUNNR") = Right("0000000000" & m_objUser.KUNNR, 10)
            dtRow("CHASSIS_NUM") = Vin
            dtRow("EQUNR") = Equipment
            dtRow("ZZREFERENZ") = Ref
            dtRow("VBELN") = Auftragsnummer

            sapTable.Rows.Add(dtRow)
            sapTable.AcceptChanges()

            S.AP.SetImportParameter("I_BAPI", "Z_M_ZULAUFTR_INSERT")
            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            S.AP.SetImportParameter("I_WEB_REPORT", "Ueberf01.aspx")

            'myProxy.callBapi()
            S.AP.Execute()

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Return True

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Return False

        End Try

    End Function

    Public Sub SetVehicleData(ByVal KunNr As String, ByVal Kennzeichen As String, ByVal ref As String)

        Dim intID As Int32 = -1

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Fahrzeugdaten", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_Fahrzeugdaten")

            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & KunNr, 10))
            S.AP.SetImportParameter("I_LICENSE_NUM", Kennzeichen)
            S.AP.SetImportParameter("I_LIZNR", ref)

            'myProxy.callBapi()
            S.AP.Execute()

            Dim sapStruc As DataTable = S.AP.GetExportTable("GF_WEB")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Dim StrucRow As DataRow = sapStruc.Rows(0)

            If Trim(StrucRow("License_Num").ToString) <> String.Empty Then
                Dim arKenn As String() = sapStruc.Rows(0)("License_Num").ToString.Split("-"c)
                If arKenn.GetLength(0) = 2 Then
                    Kenn1 = arKenn(0)
                    Kenn2 = arKenn(1)
                End If
            End If

            Herst = StrucRow("ZZHERST_TEXT").ToString & " " & StrucRow("ZZKLARTEXT_TYP").ToString
            'ref = StrucRow("LIZNR").ToString
            Vin = StrucRow("CHASSIS_NUM").ToString
            Equipment = StrucRow("EQUNR").ToString

            FahrzeugVorhanden = True

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

    End Sub

    Public Function GetUeberfuehrungsbuffer(ByVal kunNr As String) As DataTable

        Dim sapTable As DataTable = Nothing

        Try
            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_ZULAUFTR_LIST", m_objApp, m_objUser, page)

            S.AP.Init("Z_M_ZULAUFTR_LIST")

            S.AP.SetImportParameter("I_BAPI", "Z_M_ZULAUFTR_LIST")
            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & kunNr, 10))
            S.AP.SetImportParameter("I_WEB_REPORT", "Ueberf01.aspx")

            'myProxy.callBapi()
            S.AP.Execute()

            sapTable = S.AP.GetExportTable("GT_WEB")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

        Return sapTable

    End Function

    Public Sub getSTVA(ByVal strPLZ As String)

        Dim SAPTableSTVA As DataTable = Nothing
        Dim SAPTablePLZ As DataTable = Nothing

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_Get_Zulst_By_Plz", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_PLZ", strPLZ)
            'myProxy.setImportParameter("I_ORT", "")

            'myProxy.callBapi()

            S.AP.InitExecute("Z_Get_Zulst_By_Plz", "I_PLZ,I_ORT", strPLZ, "")

            SAPTablePLZ = S.AP.GetExportTable("T_ORTE")
            SAPTableSTVA = S.AP.GetExportTable("T_ZULST")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

        tblKreis = SAPTableSTVA

    End Sub

    Public Sub getVKBuero(ByVal KunNr As String)

        Dim sapTable As DataTable

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Adresse_Vk_Buero", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_Adresse_Vk_Buero")

            S.AP.SetImportParameter("I_BAPI", "Z_M_ADRESSE_VK_BUERO")
            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & KunNr, 10))
            S.AP.SetImportParameter("I_KUNNR", "ZUL01.aspx")
            S.AP.SetImportParameter("I_AKTION", "")

            'myProxy.callBapi()
            S.AP.Execute()

            VB3100 = S.AP.GetExportParameter("NE_3100")
            sapTable = S.AP.GetExportTable("E_ADRESSE")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            If Not sapTable Is Nothing Then
                With sapTable.Rows(0)
                    VBName1 = ("Name1")
                    VBName2 = ("Name2")
                    VBStrasse = ("Street") & " " & ("House_Num1")
                    VBPlzOrt = ("Post_Code1") & " " & ("City1")
                    VBTel = ("Tel_Number")
                    VBMail = ("Smtp_Addr")
                End With
            End If

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        End Try

    End Sub

    Public Sub getVKBueroFooter(ByVal KunNr As String)

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Adresse_Vk_Buero", m_objApp, m_objUser, page)
            S.AP.Init("Z_M_Adresse_Vk_Buero")

            S.AP.SetImportParameter("I_BAPI", "Z_M_ADRESSE_VK_BUERO")
            S.AP.SetImportParameter("I_KUNNR", Right("0000000000" & KunNr, 10))
            S.AP.SetImportParameter("I_WEB_REPORT", "ZUL01.aspx")
            S.AP.SetImportParameter("I_AKTION", "A")

            'myProxy.callBapi()
            S.AP.Execute()

            Dim sapTable As DataTable = S.AP.GetExportTable("E_ADRESSE")

            If Not sapTable Is Nothing Then

                Dim dr As DataRow = sapTable.Rows(0)

                FooVBName1 = dr("Name1").ToString
                FooVBName2 = dr("Name2").ToString
                FooVBStrasse = dr("Street").ToString & " " & dr("House_Num1").ToString
                FooVBPlzOrt = dr("Post_Code1").ToString & " " & dr("City1").ToString
                FooVBTel = "Telefon " & dr("Tel_Number").ToString
                FooVBFax = "Fax " & dr("Fax_Number").ToString
                FooVBMail = dr("Smtp_Addr").ToString
            End If

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_PARAMETER"
                    m_strMessage = "Eingabedaten nicht ausreichend."
                Case "NO_ASL"
                    m_strMessage = "Falsche Kundennr."
                Case "NO_LANGTEXT"
                    m_strMessage = ""
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
            
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            
        End Try

    End Sub
    
#Region "Vars"

    Private strKundeName1 As String
    Private strKundeStrasse As String
    Private strKundeOrt As String
    Private strKundeAnsprechpartner As String
    Private strKundeTelefon As String
    Private strAbKundennummer As String
    Private strAbName As String
    Private strAbStrasse As String
    Private strAbNr As String
    Private strAbPlz As String
    Private strAbOrt As String
    Private strAbTelefon1 As String
    Private strAbTelefon2 As String
    Private strAbAnsprechpartner As String
    Private strAnKundennummer As String
    Private strAnName As String
    Private strAnStrasse As String
    Private strAnNr As String
    Private strAnPlz As String
    Private strAnOrt As String
    Private strAnTelefon1 As String
    Private strAnTelefon2 As String
    Private strAnAnsprechpartner As String
    Private strHerst As String
    Private strKenn1 As String
    Private strKenn2 As String
    Private strWunschKennzeichen1 As String
    Private strWunschKennzeichen2 As String
    Private strWunschKennzeichen3 As String
    Private strFzgZugelassen As String
    Private strSomWin As String
    Private strVin As String
    Private strRef As String
    Private booAnschluss As Boolean
    Private dteDatumUeberf As Date
    Private booFixDatum As Boolean
    Private strZulHaltername As String
    Private strZulPLZ As String
    Private strZulOrt As String
    Private booTanken As Boolean
    Private booWaesche As Boolean
    Private booFzgEinweisung As Boolean
    Private booRotKenn As Boolean
    Private strBemerkung As String
    Private strReName As String
    Private strReStrasse As String
    Private strReNr As String
    Private strReKundennummer As String
    Private strRePlz As String
    Private strReOrt As String
    Private strReTelefon1 As String
    Private strReTelefon2 As String
    Private strReAnsprechpartner As String
    Private strReHerst As String
    Private strReKenn1 As String
    Private strReKenn2 As String
    Private strReFzgZugelassen As String
    Private strReSomWin As String
    Private strReVin As String
    Private strReRef As String
    Private strReBemerkung As String
    Public AdressStatusAbholung As AdressStatus
    Public AdressStatusAnlieferung As AdressStatus
    Public AdressStatusRuecklieferung As AdressStatus
    Private strSelRetour As String
    Private strSelFahrzeugwert As String
    Private strVbeln As String
    Private strFahrzeugwertTxt As String
    Private lngBeauftragungsart As Beauftragungsart
    Private booFahrzeugVorhanden As Boolean
    Private strEqui As String
    Private strQmnum As String
    Private strVbeln1510 As String
    Private strVkorg As String
    Private nModus As Int16
    Private strAuftragsnummer As String
    Private strVBName1 As String
    Private strVBName2 As String
    Private strVBStrasse As String
    Private strVBPlzOrt As String
    Private strVBTel As String
    Private strVBMail As String
    Private strVB3100 As String

    Private strFooVBName1 As String
    Private strFooVBName2 As String
    Private strFooVBStrasse As String
    Private strFooVBPlzOrt As String
    Private strFooVBTel As String
    Private strFooVBFax As String
    Private strFooVBMail As String

    Private dteZulassungsdatum As Date
    Private strZulAuftrag As String
    Public strClassTrace As String

    Private strLeasingnehmerName As String
    Private strLeasingnehmerPLZ As String
    Private strLeasingnehmerOrt As String
    Private strLeasingnehmerTelefon1 As String
    Private strLeasingnehmerTelefon2 As String
    Private strLeasingnehmerAnsprechpartner As String
    Private strLeasingnehmerStrasse As String
    Private strLeasingnehmerHausnummer As String
    Private strLeasingnehmerKundennummer As String
    Private strVersicherer As String
    Private strKfzSteuer As String
    Private strVersicherungsnehmer As String
    Private strBemerkungLease As String
    Public tblKreis As DataTable
    Public HalterAdressStatus As AdressStatus
    Public istLeasingnehmerGesperrt As Boolean
    Public selVersicherer As String
    Public zulModus As Int16
    Private strSchildversandName As String
    Private strSchildversandStrasseHausnr As String
    Private strSchildversandPLZOrt As String
    Private booWinterHandling As Boolean
    Private strWinterText As String
    Private strWinterFelgen As String
    Private strWinterRadkappen As String
    Private strWinterZweiterRadsatz As String
    Private strWinterReifenquelle As String
    Private booWinterGroesser As Boolean
    Private strWinterGroesse As String

#End Region

#Region "Properties"

    Public Property Beauftragung() As Beauftragungsart
        Get
            Return lngBeauftragungsart
        End Get
        Set(ByVal Value As Beauftragungsart)
            lngBeauftragungsart = Value
        End Set
    End Property

    Public Property Equipment() As String
        Get
            Return strEqui
        End Get
        Set(ByVal Value As String)
            strEqui = Value
        End Set
    End Property

    Public Property Qmnum() As String
        Get
            Return strQmnum
        End Get
        Set(ByVal Value As String)
            strQmnum = Value
        End Set
    End Property

    Public Property Vbeln1510() As String
        Get
            Return strVbeln1510
        End Get
        Set(ByVal Value As String)
            strVbeln1510 = Value
        End Set
    End Property


    Public Property KundeName() As String
        Get
            Return strKundeName1
        End Get
        Set(ByVal Value As String)
            strKundeName1 = Value
        End Set
    End Property

    Public Property KundeStrasse() As String
        Get
            Return strKundeStrasse
        End Get
        Set(ByVal Value As String)
            strKundeStrasse = Value
        End Set
    End Property

    Public Property KundeOrt() As String
        Get
            Return strKundeOrt
        End Get
        Set(ByVal Value As String)
            strKundeOrt = Value
        End Set
    End Property

    Public Property KundeAnsprechpartner() As String
        Get
            Return strKundeAnsprechpartner
        End Get
        Set(ByVal Value As String)
            strKundeAnsprechpartner = Value
        End Set
    End Property

    Public Property KundeTelefon() As String
        Get
            Return strKundeTelefon
        End Get
        Set(ByVal Value As String)
            strKundeTelefon = Value
        End Set
    End Property

    Public Property AbKundennummer() As String
        Get
            Return strAbKundennummer
        End Get
        Set(ByVal Value As String)
            strAbKundennummer = Value
        End Set
    End Property

    Public Property AbName() As String
        Get
            Return strAbName
        End Get
        Set(ByVal Value As String)
            strAbName = Value
        End Set
    End Property

    Public Property AbStrasse() As String
        Get
            Return strAbStrasse
        End Get
        Set(ByVal Value As String)
            strAbStrasse = Value
        End Set
    End Property

    Public Property AbNr() As String
        Get
            Return strAbNr
        End Get
        Set(ByVal Value As String)
            strAbNr = Value
        End Set
    End Property

    Public Property AbPlz() As String
        Get
            Return strAbPlz
        End Get
        Set(ByVal Value As String)
            strAbPlz = Value
        End Set
    End Property

    Public Property AbOrt() As String
        Get
            Return strAbOrt
        End Get
        Set(ByVal Value As String)
            strAbOrt = Value
        End Set
    End Property

    Public Property AbTelefon1() As String
        Get
            Return strAbTelefon1
        End Get
        Set(ByVal Value As String)
            strAbTelefon1 = Value
        End Set
    End Property

    Public Property AbTelefon2() As String
        Get
            Return strAbTelefon2
        End Get
        Set(ByVal Value As String)
            strAbTelefon2 = Value
        End Set
    End Property

    Public Property AbAnsprechpartner() As String
        Get
            Return strAbAnsprechpartner
        End Get
        Set(ByVal Value As String)
            strAbAnsprechpartner = Value
        End Set
    End Property

    Public Property AnKundennummer() As String
        Get
            Return strAnKundennummer
        End Get
        Set(ByVal Value As String)
            strAnKundennummer = Value
        End Set
    End Property

    Public Property AnName() As String
        Get
            Return strAnName
        End Get
        Set(ByVal Value As String)
            strAnName = Value
        End Set
    End Property

    Public Property AnStrasse() As String
        Get
            Return strAnStrasse
        End Get
        Set(ByVal Value As String)
            strAnStrasse = Value
        End Set
    End Property

    Public Property AnNr() As String
        Get
            Return strAnNr
        End Get
        Set(ByVal Value As String)
            strAnNr = Value
        End Set
    End Property

    Public Property AnPlz() As String
        Get
            Return strAnPlz
        End Get
        Set(ByVal Value As String)
            strAnPlz = Value
        End Set
    End Property

    Public Property AnOrt() As String
        Get
            Return strAnOrt
        End Get
        Set(ByVal Value As String)
            strAnOrt = Value
        End Set
    End Property

    Public Property AnTelefon1() As String
        Get
            Return strAnTelefon1
        End Get
        Set(ByVal Value As String)
            strAnTelefon1 = Value
        End Set
    End Property

    Public Property AnTelefon2() As String
        Get
            Return strAnTelefon2
        End Get
        Set(ByVal Value As String)
            strAnTelefon2 = Value
        End Set
    End Property

    Public Property AnAnsprechpartner() As String
        Get
            Return strAnAnsprechpartner
        End Get
        Set(ByVal Value As String)
            strAnAnsprechpartner = Value
        End Set
    End Property

    Public Property Herst() As String
        Get
            Return strHerst
        End Get
        Set(ByVal Value As String)
            strHerst = Value
        End Set
    End Property

    Public Property Kenn1() As String
        Get
            Return strKenn1
        End Get
        Set(ByVal Value As String)
            strKenn1 = Value
        End Set
    End Property

    Public Property Kenn2() As String
        Get
            Return strKenn2
        End Get
        Set(ByVal Value As String)
            strKenn2 = Value
        End Set
    End Property

    Public Property Wunschkennzeichen1() As String
        Get
            Return strWunschKennzeichen1
        End Get
        Set(ByVal Value As String)
            strWunschKennzeichen1 = Value
        End Set
    End Property

    Public Property Wunschkennzeichen2() As String
        Get
            Return strWunschKennzeichen2
        End Get
        Set(ByVal Value As String)
            strWunschKennzeichen2 = Value
        End Set
    End Property

    Public Property Wunschkennzeichen3() As String
        Get
            Return strWunschKennzeichen3
        End Get
        Set(ByVal Value As String)
            strWunschKennzeichen3 = Value
        End Set
    End Property


    Public Property FzgZugelassen() As String
        Get
            Return strFzgZugelassen
        End Get
        Set(ByVal Value As String)
            strFzgZugelassen = Value
        End Set
    End Property

    Public Property SomWin() As String
        Get
            Return strSomWin
        End Get
        Set(ByVal Value As String)
            strSomWin = Value
        End Set
    End Property

    Public Property Vin() As String
        Get
            Return strVin
        End Get
        Set(ByVal Value As String)
            strVin = Value
        End Set
    End Property

    Public Property Ref() As String
        Get
            Return strRef
        End Get
        Set(ByVal Value As String)
            strRef = Value
        End Set
    End Property

    Public Property Anschluss() As Boolean
        Get
            Return booAnschluss
        End Get
        Set(ByVal Value As Boolean)
            booAnschluss = Value
        End Set
    End Property

    Public Property DatumUeberf() As Date
        Get
            Return dteDatumUeberf
        End Get
        Set(ByVal Value As Date)
            dteDatumUeberf = Value
        End Set
    End Property

    Public Property FixDatumUeberfuehrung() As Boolean
        Get
            Return booFixDatum
        End Get
        Set(ByVal Value As Boolean)
            booFixDatum = Value
        End Set
    End Property

    Public ReadOnly Property FixDatumUeberfuehrungText() As String
        Get
            If FixDatumUeberfuehrung Then
                Return "Fix"
            Else
                Return ""
            End If
        End Get
    End Property

    Public Property Tanken() As Boolean
        Get
            Return booTanken
        End Get
        Set(ByVal Value As Boolean)
            booTanken = Value
        End Set
    End Property

    Public Property Waesche() As Boolean
        Get
            Return booWaesche
        End Get
        Set(ByVal Value As Boolean)
            booWaesche = Value
        End Set
    End Property

    Public Property FzgEinweisung() As Boolean
        Get
            Return booFzgEinweisung
        End Get
        Set(ByVal Value As Boolean)
            booFzgEinweisung = Value
        End Set
    End Property

    Public Property RotKenn() As Boolean
        Get
            Return booRotKenn
        End Get
        Set(ByVal Value As Boolean)
            booRotKenn = Value
        End Set
    End Property

    Public Property Bemerkung() As String
        Get
            Return strBemerkung
        End Get
        Set(ByVal Value As String)
            strBemerkung = Value
        End Set
    End Property
    Public Property ZulHaltername() As String
        Get
            Return strZulHaltername
        End Get
        Set(ByVal Value As String)
            strZulHaltername = Value
        End Set
    End Property

    Public Property ZulPLZ() As String
        Get
            Return strZulPLZ
        End Get
        Set(ByVal Value As String)
            strZulPLZ = Value
        End Set
    End Property

    Public Property ZulOrt() As String
        Get
            Return strZulOrt
        End Get
        Set(ByVal Value As String)
            strZulOrt = Value
        End Set
    End Property


    'Neue Props
    Public Property ReName() As String
        Get
            Return strReName
        End Get
        Set(ByVal Value As String)
            strReName = Value
        End Set
    End Property

    Public Property ReStrasse() As String
        Get
            Return strReStrasse
        End Get
        Set(ByVal Value As String)
            strReStrasse = Value
        End Set
    End Property

    Public Property ReNr() As String
        Get
            Return strReNr
        End Get
        Set(ByVal Value As String)
            strReNr = Value
        End Set
    End Property

    Public Property ReKundennummer() As String
        Get
            Return strReKundennummer
        End Get
        Set(ByVal Value As String)
            strReKundennummer = Value
        End Set
    End Property

    Public Property RePlz() As String
        Get
            Return strRePlz
        End Get
        Set(ByVal Value As String)
            strRePlz = Value
        End Set
    End Property

    Public Property ReOrt() As String
        Get
            Return strReOrt
        End Get
        Set(ByVal Value As String)
            strReOrt = Value
        End Set
    End Property

    Public Property ReTelefon1() As String
        Get
            Return strReTelefon1
        End Get
        Set(ByVal Value As String)
            strReTelefon1 = Value
        End Set
    End Property

    Public Property ReTelefon2() As String
        Get
            Return strReTelefon2
        End Get
        Set(ByVal Value As String)
            strReTelefon2 = Value
        End Set
    End Property

    Public Property ReAnsprechpartner() As String
        Get
            Return strReAnsprechpartner
        End Get
        Set(ByVal Value As String)
            strReAnsprechpartner = Value
        End Set
    End Property


    Public Property SelRetour() As String
        Get
            Return strSelRetour
        End Get
        Set(ByVal Value As String)
            strSelRetour = Value
        End Set
    End Property


    Public Property ReHerst() As String
        Get
            Return strReHerst
        End Get
        Set(ByVal Value As String)
            strReHerst = Value
        End Set
    End Property

    Public Property ReKenn1() As String
        Get
            Return strReKenn1
        End Get
        Set(ByVal Value As String)
            strReKenn1 = Value
        End Set
    End Property

    Public Property ReKenn2() As String
        Get
            Return strReKenn2
        End Get
        Set(ByVal Value As String)
            strReKenn2 = Value
        End Set
    End Property

    Public Property ReFzgZugelassen() As String
        Get
            Return strReFzgZugelassen
        End Get
        Set(ByVal Value As String)
            strReFzgZugelassen = Value
        End Set
    End Property

    Public Property ReSomWin() As String
        Get
            Return strReSomWin
        End Get
        Set(ByVal Value As String)
            strReSomWin = Value
        End Set
    End Property

    Public Property ReVin() As String
        Get
            Return strReVin
        End Get
        Set(ByVal Value As String)
            strReVin = Value
        End Set
    End Property

    Public Property ReRef() As String
        Get
            Return strReRef
        End Get
        Set(ByVal Value As String)
            strReRef = Value
        End Set
    End Property

    Public Property ReBemerkung() As String
        Get
            Return strReBemerkung
        End Get
        Set(ByVal Value As String)
            strReBemerkung = Value
        End Set
    End Property


    Public Property Vbeln() As String
        Get
            Return strVbeln
        End Get
        Set(ByVal Value As String)
            strVbeln = Value
        End Set
    End Property

    Public Property Modus() As Int16
        Get
            Return nModus
        End Get
        Set(ByVal Value As Int16)
            nModus = Value
        End Set
    End Property

    Public Property FahrzeugwertTxt() As String
        Get
            Return strFahrzeugwertTxt
        End Get
        Set(ByVal Value As String)
            strFahrzeugwertTxt = Value
        End Set
    End Property

    Public Property FahrzeugVorhanden() As Boolean
        Get
            Return booFahrzeugVorhanden
        End Get
        Set(ByVal Value As Boolean)
            booFahrzeugVorhanden = Value
        End Set
    End Property

    Public Property Auftragsnummer() As String
        Get
            Return strAuftragsnummer
        End Get
        Set(ByVal Value As String)
            strAuftragsnummer = Value
        End Set
    End Property

    Public Property VBName1() As String
        Get
            Return strVBName1
        End Get
        Set(ByVal Value As String)
            strVBName1 = Value
        End Set
    End Property

    Public Property VBName2() As String
        Get
            Return strVBName2
        End Get
        Set(ByVal Value As String)
            strVBName2 = Value
        End Set
    End Property


    Public Property VBStrasse() As String
        Get
            Return strVBStrasse
        End Get
        Set(ByVal Value As String)
            strVBStrasse = Value
        End Set
    End Property

    Public Property VBPlzOrt() As String
        Get
            Return strVBPlzOrt
        End Get
        Set(ByVal Value As String)
            strVBPlzOrt = Value
        End Set
    End Property

    Public Property VBTel() As String
        Get
            Return strVBTel
        End Get
        Set(ByVal Value As String)
            strVBTel = Value
        End Set
    End Property

    Public Property VBMail() As String
        Get
            Return strVBMail
        End Get
        Set(ByVal Value As String)
            strVBMail = Value
        End Set
    End Property

    Public Property VB3100() As String
        Get
            Return strVB3100
        End Get
        Set(ByVal Value As String)
            strVB3100 = Value
        End Set
    End Property


    Public Property FooVBName1() As String
        Get
            Return strFooVBName1
        End Get
        Set(ByVal Value As String)
            strFooVBName1 = Value
        End Set
    End Property

    Public Property FooVBName2() As String
        Get
            Return strFooVBName2
        End Get
        Set(ByVal Value As String)
            strFooVBName2 = Value
        End Set
    End Property


    Public Property FooVBStrasse() As String
        Get
            Return strFooVBStrasse
        End Get
        Set(ByVal Value As String)
            strFooVBStrasse = Value
        End Set
    End Property

    Public Property FooVBPlzOrt() As String
        Get
            Return strFooVBPlzOrt
        End Get
        Set(ByVal Value As String)
            strFooVBPlzOrt = Value
        End Set
    End Property

    Public Property FooVBTel() As String
        Get
            Return strFooVBTel
        End Get
        Set(ByVal Value As String)
            strFooVBTel = Value
        End Set
    End Property

    Public Property FooVBFax() As String
        Get
            Return strFooVBFax
        End Get
        Set(ByVal Value As String)
            strFooVBFax = Value
        End Set
    End Property


    Public Property FooVBMail() As String
        Get
            Return strFooVBMail
        End Get
        Set(ByVal Value As String)
            strFooVBMail = Value
        End Set
    End Property



    Public Property Zulassungsdatum() As Date
        Get
            Return dteZulassungsdatum
        End Get
        Set(ByVal value As Date)
            dteZulassungsdatum = value
        End Set
    End Property

    Public Property ZulAuftrag() As String
        Get
            Return strZulAuftrag
        End Get
        Set(ByVal value As String)
            strZulAuftrag = value
        End Set
    End Property

    Public Property Leasingnehmer() As String
        Get
            Return strLeasingnehmerName
        End Get
        Set(ByVal Value As String)
            strLeasingnehmerName = Value
        End Set
    End Property

    Public Property LeasingnehmerPLZ() As String
        Get
            Return strLeasingnehmerPLZ
        End Get
        Set(ByVal Value As String)
            strLeasingnehmerPLZ = Value
        End Set
    End Property

    Public Property LeasingnehmerOrt() As String
        Get
            Return strLeasingnehmerOrt
        End Get
        Set(ByVal Value As String)
            strLeasingnehmerOrt = Value
        End Set
    End Property

    Public Property LeasingnehmerTelefon1() As String
        Get
            Return strLeasingnehmerTelefon1
        End Get
        Set(ByVal Value As String)
            strLeasingnehmerTelefon1 = Value
        End Set
    End Property

    Public Property LeasingnehmerTelefon2() As String
        Get
            Return strLeasingnehmerTelefon2
        End Get
        Set(ByVal Value As String)
            strLeasingnehmerTelefon2 = Value
        End Set
    End Property

    Public Property LeasingnehmerAnsprechpartner() As String
        Get
            Return strLeasingnehmerAnsprechpartner
        End Get
        Set(ByVal Value As String)
            strLeasingnehmerAnsprechpartner = Value
        End Set
    End Property

    Public Property LeasingnehmerStrasse() As String
        Get
            Return strLeasingnehmerStrasse
        End Get
        Set(ByVal Value As String)
            strLeasingnehmerStrasse = Value
        End Set
    End Property

    Public Property LeasingnehmerHausnummer() As String
        Get
            Return strLeasingnehmerHausnummer
        End Get
        Set(ByVal Value As String)
            strLeasingnehmerHausnummer = Value
        End Set
    End Property

    Public Property LeasingnehmerKundennummer() As String
        Get
            Return strLeasingnehmerKundennummer
        End Get
        Set(ByVal Value As String)
            strLeasingnehmerKundennummer = Value
        End Set
    End Property

    Public Property Versicherer() As String
        Get
            Return strVersicherer
        End Get
        Set(ByVal Value As String)
            strVersicherer = Value
        End Set
    End Property

    Public Property Versicherungsnehmer() As String
        Get
            Return strVersicherungsnehmer
        End Get
        Set(ByVal Value As String)
            strVersicherungsnehmer = Value
        End Set
    End Property

    Public Property KfzSteuer() As String
        Get
            Return strKfzSteuer
        End Get
        Set(ByVal Value As String)
            strKfzSteuer = Value
        End Set
    End Property
    Public Property BemerkungLease() As String
        Get
            Return strBemerkungLease
        End Get
        Set(ByVal Value As String)
            strBemerkungLease = Value
        End Set
    End Property

    Public Property SchildversandName() As String
        Get
            Return strSchildversandName
        End Get
        Set(ByVal Value As String)
            strSchildversandName = Value
        End Set
    End Property

    Public Property SchildversandStrasseHausnr() As String
        Get
            Return strSchildversandStrasseHausnr
        End Get
        Set(ByVal Value As String)
            strSchildversandStrasseHausnr = Value
        End Set
    End Property

    Public Property SchildversandPLZOrt() As String
        Get
            Return strSchildversandPLZOrt
        End Get
        Set(ByVal Value As String)
            strSchildversandPLZOrt = Value
        End Set
    End Property

    Public Property SelFahrzeugwert() As String
        Get
            Return strSelFahrzeugwert
        End Get
        Set(ByVal Value As String)
            strSelFahrzeugwert = Value
        End Set
    End Property

    Public Property WinterText() As String
        Get
            Return strWinterText
        End Get
        Set(ByVal Value As String)
            strWinterText = Value
        End Set
    End Property

    Public Property WinterFelgen() As String
        Get
            Return strWinterFelgen
        End Get
        Set(ByVal Value As String)
            strWinterFelgen = Value
        End Set
    End Property

    Public Property WinterRadkappen() As String
        Get
            Return strWinterRadkappen
        End Get
        Set(ByVal Value As String)
            strWinterRadkappen = Value
        End Set
    End Property

    Public Property WinterZweiterRadsatz() As String
        Get
            Return strWinterZweiterRadsatz
        End Get
        Set(ByVal Value As String)
            strWinterZweiterRadsatz = Value
        End Set
    End Property

    Public Property WinterReifenquelle() As String
        Get
            Return strWinterReifenquelle
        End Get
        Set(ByVal Value As String)
            strWinterReifenquelle = Value
        End Set
    End Property

    Public Property WinterGroesse() As String
        Get
            Return strWinterGroesse
        End Get
        Set(ByVal Value As String)
            strWinterGroesse = Value
        End Set
    End Property

    Public Property WinterGroesser() As Boolean
        Get
            Return booWinterGroesser
        End Get
        Set(ByVal Value As Boolean)
            booWinterGroesser = Value
        End Set
    End Property

    Public Property WinterHandling() As Boolean
        Get
            Return booWinterHandling
        End Get
        Set(ByVal Value As Boolean)
            booWinterHandling = Value
        End Set
    End Property

    Public ReadOnly Property StrasenverkehrsamtText() As String
        Get
            If Not tblKreis Is Nothing AndAlso tblKreis.Rows.Count > 0 Then

                Return "Ermittelter Zulassungskreis: " & tblKreis.Rows(0)("ZKFZKZ").ToString() & "/" & _
                                    tblKreis.Rows(0)("ZKREIS").ToString()

            Else

                Return ""

            End If

        End Get
    End Property
#End Region

End Class

' ************************************************
' $History: Ueberf_01.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 18.12.08   Time: 11:35
' Updated in $/CKAG/Applications/AppUeberf/Lib
' ITA 2490
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 4.06.08    Time: 16:42
' Updated in $/CKAG/Applications/AppUeberf/Lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Lib
' 
' *****************  Version 14  *****************
' User: Uha          Date: 2.07.07    Time: 16:18
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 13  *****************
' User: Fassbenders  Date: 21.06.07   Time: 16:27
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' 
' *****************  Version 12  *****************
' User: Fassbenders  Date: 12.06.07   Time: 17:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' 
' *****************  Version 11  *****************
' User: Uha          Date: 4.06.07    Time: 14:21
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' Ungereimtheiten bzgl. PARVW->TYP, KUNNR->KUNDENNUMMER etc. zwischen
' Dataset und Code der *.aspx.vb-Dateien beseitigt. Richtige BAPI in lib
' Ueberf_01.vb eingetragen.
' 
' *****************  Version 10  *****************
' User: Uha          Date: 29.05.07   Time: 10:13
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' LeasingnehmerKundennummer wird im Fall einer Freitexteingabe nicht mehr
' leer übergeben.
' 
' *****************  Version 9  *****************
' User: Uha          Date: 21.05.07   Time: 11:47
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' Änderungen im Vergleich zur Startapplikation zum Stand 11.05.2007
' 
' *****************  Version 8  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Lib
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
