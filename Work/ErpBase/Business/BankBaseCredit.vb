Option Explicit On
Option Infer On
Option Strict On

Imports System
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Common

Namespace Business
    <Serializable()> Public Class BankBaseCredit
        REM § Lese-/Schreibfunktion, Kunde: FFD, 
        REM § Show - BAPI: Z_M_Creditlimit_Detail,
        REM § Change - BAPI: Zzcreditlimit_Change.

        Inherits BankBase

#Region " Declarations"
        Private m_tblKontingente As DataTable
#End Region

#Region " Properties"
        Public Property Kontingente() As DataTable
            Get
                Return m_tblKontingente
            End Get
            Set(ByVal Value As DataTable)
                m_tblKontingente = Value
            End Set
        End Property
#End Region

#Region " Methods"
        Public Sub New(ByRef objUser As Kernel.Security.User, ByRef objApp As Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
            MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
            m_hez = hez
        End Sub

        Public Overrides Sub Show()
            m_strClassAndMethod = "BankBaseCredit.Show"
            If Not m_blnGestartet Then
                m_blnGestartet = True

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                Try
                    m_tblKontingente = New DataTable()
                    With m_tblKontingente.Columns
                        .Add("Kreditkontrollbereich", GetType(String))
                        .Add("ZeigeKontingentart", GetType(Boolean))
                        .Add("Kontingentart", GetType(String))
                        .Add("Kontingent_Alt", GetType(Int32))
                        .Add("Kontingent_Neu", GetType(Int32))
                        .Add("Ausschoepfung", GetType(Int32))
                        .Add("Frei", GetType(Int32))
                        .Add("Gesperrt_Alt", GetType(Boolean))
                        .Add("Gesperrt_Neu", GetType(Boolean))
                        .Add("Richtwert_Alt", GetType(Int32))
                        .Add("Richtwert_Neu", GetType(Int32))
                        .Add("Lastschrift", GetType(Boolean))
                    End With
                    m_intStatus = 0
                    m_strMessage = ""

                    If CheckCustomerData() Then

                        m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Creditlimit_Detail", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                        Dim proxy = DynSapProxy.getProxy("Z_M_CREDITLIMIT_DETAIL", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                        proxy.setImportParameter("I_KNRZE", Left(m_objUser.Reference, 2))
                        proxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.KUNNR, 10))
                        proxy.setImportParameter("I_KUNNR", Right(m_strCustomer, 5))
                        proxy.setImportParameter("I_VKORG", "1510")

                        proxy.callBapi()

                        If m_intIDSAP > -1 Then
                            m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                        End If

                        Dim creditAccountDetailTable = proxy.getExportTable("GT_WEB")

                        If creditAccountDetailTable.Rows.Count > 0 Then
                            Dim rowNew As DataRow
                            For Each creditAccountDetail As DataRow In creditAccountDetailTable.Rows
                                Dim kkber = CStr(creditAccountDetail("KKBER"))
                                If Not String.IsNullOrEmpty(kkber) Then
                                    rowNew = m_tblKontingente.NewRow
                                    rowNew("Kreditkontrollbereich") = kkber
                                    Select Case kkber
                                        Case "0001"
                                            rowNew("Kontingentart") = "Standard temporär"
                                            rowNew("ZeigeKontingentart") = True
                                        Case "0002"
                                            rowNew("Kontingentart") = "Standard endgültig"
                                            rowNew("ZeigeKontingentart") = True
                                        Case "0003"
                                            rowNew("Kontingentart") = "Retail"
                                            rowNew("ZeigeKontingentart") = False
                                            If CStr(creditAccountDetail("Zzfrist")) <> "000" Then
                                                rowNew("Lastschrift") = True
                                            End If
                                        Case "0004"
                                            rowNew("Kontingentart") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                                            rowNew("ZeigeKontingentart") = False
                                        Case "0005"
                                            rowNew("Kontingentart") = "HEZ (in standard temporär enthalten)"
                                            rowNew("ZeigeKontingentart") = False
                                        Case Else
                                            m_intStatus = -2203
                                            m_strMessage = "Fehler: Unbekannte Kontingentart (" & kkber & ")."
                                            Exit Try
                                    End Select
                                    rowNew("Richtwert_Alt") = CInt(creditAccountDetail("Zzrwert"))
                                    rowNew("Richtwert_Neu") = rowNew("Richtwert_Alt")
                                    rowNew("Kontingent_Alt") = CInt(creditAccountDetail("Klimk"))
                                    rowNew("Kontingent_Neu") = rowNew("Kontingent_Alt")
                                    If IsNumeric(creditAccountDetail("Skfor").ToString.Trim(" "c)) Then
                                        rowNew("Ausschoepfung") = CInt(creditAccountDetail("Skfor"))
                                    Else
                                        rowNew("Ausschoepfung") = 0
                                    End If
                                    rowNew("Frei") = CInt(rowNew("Kontingent_Alt")) - CInt(rowNew("Ausschoepfung"))
                                    If CStr(creditAccountDetail("Crblb")) = "X" Then
                                        rowNew("Gesperrt_Alt") = True
                                    Else
                                        rowNew("Gesperrt_Alt") = False
                                    End If
                                    rowNew("Gesperrt_Neu") = rowNew("Gesperrt_Alt")
                                    m_tblKontingente.Rows.Add(rowNew)
                                End If
                            Next
                        Else
                            m_intStatus = -2202
                            m_strMessage = "Fehler: Keine Kontingentinformationen vorhanden."
                        End If

                        Dim col As DataColumn
                        For Each col In m_tblKontingente.Columns
                            col.ReadOnly = False
                        Next
                        WriteLogEntry(True, "KUNNR=" & Right(m_strCustomer, 5), m_tblKontingente)
                    End If
                Catch ex As Exception
                    Select Case ex.Message
                        Case "NO_DATA"
                            m_intStatus = -2201
                            m_strMessage = "Es konnten keine Kontingente ermittelt werden."
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = ex.Message
                    End Select
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    End If
                    WriteLogEntry(False, "KUNNR=" & Right(m_strCustomer, 5) & " , " & Replace(m_strMessage, "<br>", " "), m_tblKontingente)
                Finally
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                    End If

                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub ShowAll(ByVal haendler As DataView, Optional ByVal zeigeAlle As Boolean = False)
            '§§§JVE 07.09.05 <begin>
            'Zeigt alle Händler an
            m_strClassAndMethod = "BankBaseCredit.ShowAll"
            If Not m_blnGestartet Then
                m_blnGestartet = True

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                Try
                    m_tblKontingente = New DataTable()
                    With m_tblKontingente.Columns
                        .Add("Kunnr", GetType(String))
                        .Add("Kreditkontrollbereich", GetType(String))
                        .Add("ZeigeKontingentart", GetType(Boolean))
                        .Add("Standard_Endg", GetType(String))
                        .Add("Standard_Temp", GetType(String))
                        .Add("Retail", GetType(String))
                        .Add("DP", GetType(String))
                        .Add("HEZ", GetType(String))
                    End With
                    m_intStatus = 0
                    m_strMessage = ""

                    m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Creditlimit_Detail", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)


                    Dim proxy = DynSapProxy.getProxy("Z_M_CREDITLIMIT_DETAIL", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                    proxy.setImportParameter("I_KNRZE", Right("00" & m_objUser.Organization.OrganizationReference, 2))
                    proxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.KUNNR, 10))
                    proxy.setImportParameter("I_KUNNR", "")
                    proxy.setImportParameter("I_VKORG", "1510")

                    proxy.callBapi()

                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                    End If

                    Dim creditAccountDetailTable = proxy.getExportTable("GT_WEB")

                    If creditAccountDetailTable.Rows.Count > 0 Then
                        For Each creditAccountDetail As DataRow In creditAccountDetailTable.Rows
                            Dim insert As Boolean = False

                            Dim kunnr = CStr(creditAccountDetail("KUNNR"))
                            Dim kkber = CStr(creditAccountDetail("KKBER"))
                            Dim skfor = Integer.Parse(CStr(creditAccountDetail("SKFOR")))
                            Dim klimk = Integer.Parse(CStr(creditAccountDetail("KLIMK")))
                            Dim zzrwert = Integer.Parse(CStr(creditAccountDetail("ZZRWERT")))

                            If Not zeigeAlle Then       'ggf. fremde Händler entfernen...
                                For Each row As DataRow In haendler.Table.Rows         'Nur eigene Händler nehmen
                                    If Right("0000000000" & ("60" & CType(row("REFERENZ"), String)), 10) = kunnr Then
                                        insert = True
                                    End If
                                Next
                            Else
                                insert = True
                            End If

                            If (kkber.Length > 0) AndAlso (skfor <= klimk) Then   'Nur wenn Inanspruchnahme > Kontingent
                                insert = False
                            End If

                            If (kkber.Length > 0) AndAlso (skfor <= zzrwert) Then 'oder Inanspruchnahme > Richtwert
                                insert = False
                            End If

                            If (kkber.Length > 0) AndAlso (insert = True) Then
                                Dim filiale As String = Right(kunnr, 5)

                                Dim rowNew As DataRow = m_tblKontingente.NewRow
                                rowNew("Kunnr") = filiale
                                rowNew("Kreditkontrollbereich") = kkber
                                Select Case kkber
                                    Case "0001"
                                        rowNew("Standard_Temp") = (klimk - skfor).ToString()
                                        rowNew("ZeigeKontingentart") = True
                                    Case "0002"
                                        rowNew("Standard_Endg") = (klimk - skfor).ToString()
                                        rowNew("ZeigeKontingentart") = True
                                    Case "0003"
                                        rowNew("Retail") = (zzrwert - skfor).ToString()
                                        rowNew("ZeigeKontingentart") = False
                                    Case "0004"
                                        rowNew("DP") = (zzrwert - skfor).ToString()
                                        rowNew("ZeigeKontingentart") = False
                                    Case "0005"
                                        rowNew("HEZ") = (zzrwert - skfor).ToString()
                                        rowNew("ZeigeKontingentart") = False
                                    Case Else
                                        m_intStatus = -2203
                                        m_strMessage = "Fehler: Unbekannte Kontingentart (" & kkber & ")."
                                        Exit Try
                                End Select
                                m_tblKontingente.Rows.Add(rowNew)
                            End If
                        Next

                        Dim col As DataColumn
                        For Each col In m_tblKontingente.Columns
                            col.ReadOnly = False
                        Next
                        WriteLogEntry(True, "KUNNR=" & Right(m_strCustomer, 5), m_tblKontingente)
                    End If
                Catch ex As Exception
                    Select Case ex.Message
                        Case "NO_DATA"
                            m_intStatus = -2201
                            m_strMessage = "Es konnten keine Kontingente ermittelt werden."
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = ex.Message
                    End Select
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    End If
                    WriteLogEntry(False, "KUNNR=" & Right(m_strCustomer, 5) & " , " & Replace(m_strMessage, "<br>", " "), m_tblKontingente)
                Finally
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                    End If

                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Overrides Sub Change()
            If Not m_blnGestartet Then
                m_blnGestartet = True

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1
                Dim strKontingentart As String = ""

                Try
                    m_intStatus = 0
                    m_strMessage = ""

                    If CheckCustomerData() Then

                        Dim tmpRow As DataRow
                        For Each tmpRow In m_tblKontingente.Rows

                            Dim decKreditlimit_Alt As Decimal = CType(tmpRow("Kontingent_Alt"), Decimal)
                            Dim decKreditlimit_Neu As Decimal = CType(tmpRow("Kontingent_Neu"), Decimal)
                            Dim decRichtwert_Alt As Decimal = CType(tmpRow("Richtwert_Alt"), Decimal)
                            Dim decRichtwert_Neu As Decimal = CType(tmpRow("Richtwert_Neu"), Decimal)
                            Dim blnGesperrt_Alt As Boolean = CType(tmpRow("Gesperrt_Alt"), Boolean)
                            Dim blnGesperrt_Neu As Boolean = CType(tmpRow("Gesperrt_Neu"), Boolean)
                            strKontingentart = CType(tmpRow("Kontingentart"), String)

                            If Not ((decKreditlimit_Alt = decKreditlimit_Neu) And (decRichtwert_Alt = decRichtwert_Neu) And (blnGesperrt_Alt = blnGesperrt_Neu)) Then
                                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Creditlimit_Change", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                                m_hez = False
                                If (CType(tmpRow("Kreditkontrollbereich"), String) = "0005") Then
                                    m_hez = True
                                End If

                                Dim proxy = DynSapProxy.getProxy("Z_M_CREDITLIMIT_CHANGE", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                                proxy.setImportParameter("I_ERNAM", m_strInternetUser)
                                proxy.setImportParameter("I_CRBLB", If(blnGesperrt_Neu, "X", " "))
                                proxy.setImportParameter("I_HEZKZ", If(m_hez, "X", " "))
                                proxy.setImportParameter("I_KKBER", CStr(tmpRow("Kreditkontrollbereich")))
                                proxy.setImportParameter("I_KLIMK", decKreditlimit_Neu.ToString)
                                proxy.setImportParameter("I_KONZS", m_strKUNNR)
                                proxy.setImportParameter("I_KUNNR", m_strCustomer)
                                proxy.setImportParameter("I_ZZRWERT", decRichtwert_Neu.ToString)

                                proxy.callBapi()

                                If m_intIDSAP > -1 Then
                                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                                End If

                                Dim tblAuftraege = proxy.getExportTable("GT_WEB")
                            End If
                        Next
                    End If
                Catch ex As Exception
                    Select Case ex.Message
                        Case "NO_DATA"
                            m_intStatus = -2221
                            m_strMessage = "Kontingentart """ & strKontingentart & """ nicht gefunden."
                        Case "NO_ZCREDITCONTROL"
                            m_intStatus = -2222
                            m_strMessage = "Kontingentart """ & strKontingentart & """ konnte nicht geändert werden. (Update-Fehler ZCREDITCONTROL)"
                        Case "NO_ZDADVERSAND"
                            m_intStatus = -2223
                            m_strMessage = "Kontingentart """ & strKontingentart & """ konnte nicht geändert werden. (Insert-Fehler ZDADVERSAND)"
                        Case "ZCREDITCONTROL_SPERRE"
                            m_intStatus = -2224
                            m_strMessage = "Kontingentart """ & strKontingentart & """ konnte nicht geändert werden. (ZCREDITCONTROL vom DAD gesperrt)"
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = ex.Message
                    End Select
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    End If
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub ShowStandard()
            '§§§ JVE 17.07.2006: Neues BAPI (zunächst nur für Porsche, soll später Standard werden).
            m_strClassAndMethod = "BankBaseCredit.ShowStandard"
            If Not m_blnGestartet Then
                m_blnGestartet = True

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                Try
                    m_tblKontingente = New DataTable()
                    With m_tblKontingente.Columns
                        .Add("Kreditkontrollbereich", GetType(String))
                        .Add("ZeigeKontingentart", GetType(Boolean))
                        .Add("Kontingentart", GetType(String))
                        .Add("Kontingent_Alt", GetType(Int32))
                        .Add("Kontingent_Neu", GetType(Int32))
                        .Add("Ausschoepfung", GetType(Int32))
                        .Add("Frei", GetType(Int32))
                        .Add("Gesperrt_Alt", GetType(Boolean))
                        .Add("Gesperrt_Neu", GetType(Boolean))
                        .Add("Richtwert_Alt", GetType(Int32))
                        .Add("Richtwert_Neu", GetType(Int32))
                    End With
                    m_intStatus = 0
                    m_strMessage = ""

                    If CheckCustomerData() Then

                        m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Creditlimit", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                        Dim proxy = DynSapProxy.getProxy("Z_M_CREDITLIMIT", m_objApp, m_objUser, PageHelper.GetCurrentPage())

                        proxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
                        proxy.setImportParameter("I_KUNNR_ZF", Right("0000000000" & m_strCustomer, 10))
                        proxy.setImportParameter("I_WEB_REPORT", "Change04.aspx")

                        proxy.callBapi()

                        If m_intIDSAP > -1 Then
                            m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                        End If

                        Dim creditAccountDetailTable = proxy.getExportTable("GT_WEB")
                        If creditAccountDetailTable.Rows.Count > 0 Then
                            Dim rowNew As DataRow
                            For Each creditAccountDetail As DataRow In creditAccountDetailTable.Rows
                                Dim kkber = CStr(creditAccountDetail("KKBER"))
                                If kkber.Length > 0 Then
                                    rowNew = m_tblKontingente.NewRow
                                    rowNew("Kreditkontrollbereich") = kkber
                                    Select Case kkber
                                        Case "0001"
                                            rowNew("Kontingentart") = "Standard temporär"
                                            rowNew("ZeigeKontingentart") = True
                                        Case "0002"
                                            rowNew("Kontingentart") = "Standard endgültig"
                                            rowNew("ZeigeKontingentart") = True
                                        Case "0003"
                                            rowNew("Kontingentart") = "Retail"
                                            rowNew("ZeigeKontingentart") = False
                                        Case "0004"
                                            rowNew("Kontingentart") = "Erweitertes Zahlungsziel (Delayed Payment) endgültig"
                                            rowNew("ZeigeKontingentart") = False
                                        Case "0005"
                                            rowNew("Kontingentart") = "HEZ (in standard temporär enthalten)"
                                            rowNew("ZeigeKontingentart") = False
                                        Case Else
                                            m_intStatus = -2203
                                            m_strMessage = "Fehler: Unbekannte Kontingentart (" & kkber & ")."
                                            Exit Try
                                    End Select
                                    'rowNew("Richtwert_Alt") = CInt(CreditAccountDetail.Zzrwert)
                                    'rowNew("Richtwert_Neu") = rowNew("Richtwert_Alt")
                                    rowNew("Kontingent_Alt") = CInt(creditAccountDetail("KLIMK"))
                                    rowNew("Kontingent_Neu") = rowNew("Kontingent_Alt")
                                    If IsNumeric(CStr(creditAccountDetail("SKFOR")).Trim(" "c)) Then
                                        rowNew("Ausschoepfung") = CInt(CStr(creditAccountDetail("SKFOR")).Trim(" "c))
                                    Else
                                        rowNew("Ausschoepfung") = 0
                                    End If
                                    rowNew("Frei") = CInt(rowNew("Kontingent_Alt")) - CInt(rowNew("Ausschoepfung"))
                                    If CStr(creditAccountDetail("CRBLB")) = "X" Then
                                        rowNew("Gesperrt_Alt") = True
                                    Else
                                        rowNew("Gesperrt_Alt") = False
                                    End If
                                    rowNew("Gesperrt_Neu") = rowNew("Gesperrt_Alt")
                                    m_tblKontingente.Rows.Add(rowNew)
                                End If
                            Next
                        Else
                            m_intStatus = -2202
                            m_strMessage = "Fehler: Keine Kontingentinformationen vorhanden."
                        End If

                        Dim col As DataColumn
                        For Each col In m_tblKontingente.Columns
                            col.ReadOnly = False
                        Next
                        WriteLogEntry(True, "KUNNR=" & Right(m_strCustomer, 5), m_tblKontingente)
                    End If
                Catch ex As Exception
                    Select Case ex.Message
                        Case "NO_DATA"
                            m_intStatus = -2201
                            m_strMessage = "Es konnten keine Kontingente ermittelt werden."
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = ex.Message
                    End Select
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    End If
                    WriteLogEntry(False, "KUNNR=" & Right(m_strCustomer, 5) & " , " & Replace(m_strMessage, "<br>", " "), m_tblKontingente)
                Finally
                    If m_intIDSAP > -1 Then
                        m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                    End If

                    m_blnGestartet = False
                End Try
            End If
        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: BankBaseCredit.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Business
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 15  *****************
' User: Rudolpho     Date: 20.03.08   Time: 9:13
' Updated in $/CKG/Base/Base/Business
' ITA: 1372
' 
' *****************  Version 14  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Base/Base/Business
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 13  *****************
' User: Rudolpho     Date: 15.06.07   Time: 9:28
' Updated in $/CKG/Base/Base/Business
' 
' *****************  Version 12  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Base/Base/Business
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 11  *****************
' User: Uha          Date: 21.05.07   Time: 14:22
' Updated in $/CKG/Base/Base/Business
' Änderungen im Vergleich zur Startapplikation zum Stand 21.05.2007
' 
' *****************  Version 10  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Business
' 
' ************************************************
