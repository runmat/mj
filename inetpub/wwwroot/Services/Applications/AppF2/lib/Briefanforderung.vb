Imports CKG.Base.Business
Imports CKG.Base.Kernel.Security
Imports CKG.Base.Common

Public Class Briefanforderung
    Inherits BankBase

    Private Const BAPI_Briefanforderung As String = "Z_M_BRIEFANFORDERUNG_STD"

    Sub New(ByVal user As User, ByVal app As App, ByVal appId As String, ByVal sessionID As String)
        MyBase.New(user, app, appId, sessionID, "")

        KUNNR = user.KUNNR
    End Sub

    Property DeliveryValue As String

    Property DeliveryText As String

    Property Kbanr As String

    Property NewAdress As Boolean

    Property Name1 As String

    Property Name2 As String

    Property Street As String

    Property HouseNum As String

    Property PostCode As String

    Property City As String

    Property VersandText As String

    Property VersandMaterialNummer As String

    Property VersandDienstleister As String

    Property Haendler_Ex As String

    Public Sub Anfordern(ByRef page As Page, ByVal row As DataRow, ByVal text50 As String)
        Dim mi = System.Reflection.MethodInfo.GetCurrentMethod

        m_intStatus = 0
        m_strClassAndMethod = mi.DeclaringType.Name & "." & mi.Name

        Try
            Customer = Haendler_Ex
            CreditControlArea = "ZDAD"

            If Not CheckCustomerData() Then Return

            Dim proxy = DynSapProxy.getProxy(BAPI_Briefanforderung, m_objApp, m_objUser, page)

            Dim impTable = proxy.getImportTable("IMP")
            Dim impRow = impTable.NewRow()
            impRow("AG") = KUNNR
            impRow("HAENDLER_EX") = Haendler_Ex
            If Not NewAdress AndAlso Not String.IsNullOrEmpty(DeliveryValue) Then
                impRow("KUNNR_ZS") = DeliveryValue.PadLeft(10, "0"c)
            End If
            impRow("EQUNR") = row("EQUNR")
            impRow("ERNAM") = m_objUser.UserName.PadLeft(12)
            impRow("ZZLABEL") = row("ZZLABEL")
            impRow("ZZKKBER") = "0002"
            impRow("MATNR") = VersandMaterialNummer
            impRow("TEXT50") = text50
            impRow("PREIS") = 0
            'impRow("KUNNR_ZH") = "??"
            impRow("KUNNR_ZE") = String.Empty
            impRow("DATUM") = DateTime.Today
            impRow("ZULST") = Kbanr
            'impRow("KVGR3") = "??"
            impRow("TXT_KOPF") = row("Subject")
            'impRow("TXT_POS") = "??"
            impRow("KDGRP") = IIf(m_objUser.Reference.Trim.Length = 0, String.Empty, "X")
            impRow("ZZDIEN1") = String.Empty
            impRow("AUGRU") = row("AUGRU")
            impRow("ZZLAUFZEIT") = row("ZZLAUFZEIT")
            'impRow("ANFNR") = "??"
            impRow("NAME1") = Name1
            impRow("NAME2") = Name2
            impRow("NAME3") = String.Empty
            impRow("STREET") = Street
            impRow("HOUSE_NUM1") = HouseNum
            impRow("POST_CODE1") = PostCode
            impRow("CITY1") = City
            'impRow("COUNTRY") = "??"
            impRow("VERSANDWEG") = VersandDienstleister

            impTable.Rows.Add(impRow)

            proxy.callBapi()


            Dim auftragsnummer = proxy.getExportParameter("E_VBELN").TrimStart("0"c)
            Dim auftragsstatus = proxy.getExportParameter("E_CMGST").TrimStart("0"c)

            Select Case auftragsstatus.ToUpper
                Case ""
                    auftragsstatus = "Kreditprüfung nicht durchgeführt"
                Case "A"
                    auftragsstatus = "Vorgang OK"
                Case "B"
                    auftragsstatus = "Zahlung offen" 'cro für GMAC 16.4.2009
                Case "C"
                    auftragsstatus = "Zahlung offen" 'cro für GMAC 16.4.2009 ' "In Bearbeitung " & m_objUser.CustomerName
                Case "D"
                    auftragsstatus = "Freigegeben"
                Case Else
                    auftragsstatus = "Unbekannt"
            End Select

            If auftragsnummer.Length = 0 Then
                m_intStatus = -2100
                m_strMessage = "Ihre Anforderung konnte im System nicht erstellt werden."
            End If

            row("COMMENT") = auftragsstatus
            row("VBELN") = auftragsnummer

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "ZCREDITCONTROL_ENTRY_LOCKED"
                    m_strMessage = "System ausgelastet. Bitte klicken Sie erneut auf ""Absenden""."
                    m_intStatus = -1111
                Case "NO_UPDATE_EQUI"
                    m_strMessage = "Fehler bei der Datenspeicherung (EQUI-UPDATE)"
                    m_intStatus = -1112
                Case "NO_AUFTRAG"
                    m_strMessage = "Kein Auftrag angelegt"
                    m_intStatus = -1113
                Case "NO_ZDADVERSAND"
                    m_strMessage = "Keine Einträge für die Versanddatei erstellt"
                    m_intStatus = -1114
                Case "NO_MODIFY_ILOA"
                    m_strMessage = "ILOA-MODIFY-Fehler"
                    m_intStatus = -1115
                Case "NO_BRIEFANFORDERUNG"
                    m_strMessage = "Brief bereits angefordert"
                    m_intStatus = -1116
                Case "NO_EQUZ"
                    m_strMessage = "Kein Brief vorhanden (EQUZ)"
                    m_intStatus = -1117
                Case "NO_ILOA"
                    m_strMessage = "Kein Brief vorhanden (ILOA)"
                    m_intStatus = -1118
                Case "NO_ANF_ABRUFGRUND"
                    m_strMessage = "Abrufgrund ungültig."
                    m_intStatus = -1119
                Case Else
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    m_intStatus = -9999
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & m_strMessage.Replace("<br>", " "), Nothing)
        End Try
    End Sub

    Public Overrides Sub Change()
        Throw New NotImplementedException
    End Sub

    Public Overrides Sub Show()
        Throw New NotImplementedException
    End Sub
End Class
