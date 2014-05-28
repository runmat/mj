Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Common
Imports CKG.Base.Kernel

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

                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_M_Creditlimit_Detail", m_objApp, m_objUser)

                    m_tblKontingente = New DataTable()
                    With m_tblKontingente.Columns
                        .Add("Kreditkontrollbereich", System.Type.GetType("System.String"))
                        .Add("ZeigeKontingentart", System.Type.GetType("System.Boolean"))
                        .Add("Kontingentart", System.Type.GetType("System.String"))
                        .Add("Kontingent_Alt", System.Type.GetType("System.Int32"))
                        .Add("Kontingent_Neu", System.Type.GetType("System.Int32"))
                        .Add("Ausschoepfung", System.Type.GetType("System.Int32"))
                        .Add("Frei", System.Type.GetType("System.Int32"))
                        .Add("Gesperrt_Alt", System.Type.GetType("System.Boolean"))
                        .Add("Gesperrt_Neu", System.Type.GetType("System.Boolean"))
                        .Add("Richtwert_Alt", System.Type.GetType("System.Int32"))
                        .Add("Richtwert_Neu", System.Type.GetType("System.Int32"))
                        .Add("Lastschrift", System.Type.GetType("System.Boolean"))
                    End With

                    ClearError()

                    If CheckCustomerData() Then

                        'm_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Creditlimit_Detail", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                        'myProxy.setImportParameter("I_KNRZE", Left(m_objUser.Reference, 2))
                        'myProxy.setImportParameter("I_KONZS", m_strKUNNR)
                        'myProxy.setImportParameter("I_KUNNR", Right(m_strCustomer, 5))
                        'myProxy.setImportParameter("I_VKORG", "1510")

                        'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                        S.AP.InitExecute("Z_M_Creditlimit_Detail", "I_KNRZE, I_KONZS, I_KUNNR, I_VKORG", Left(m_objUser.Reference, 2), KUNNR, Right(m_strCustomer, 5), "1510")

                        'Dim CreditAccountDetailTable As DataTable = myProxy.getExportTable("GT_WEB")
                        Dim CreditAccountDetailTable As DataTable = S.AP.GetExportTable("GT_WEB")

                        ' SAP-Performance Logging
                        'If m_intIDSAP > -1 Then
                        '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                        'End If

                        If CreditAccountDetailTable.Rows.Count > 0 Then
                            Dim rowNew As DataRow
                            For Each dr As DataRow In CreditAccountDetailTable.Rows
                                If dr("Kkber").ToString.Length > 0 Then
                                    rowNew = m_tblKontingente.NewRow
                                    rowNew("Kreditkontrollbereich") = dr("Kkber")
                                    Select Case dr("Kkber").ToString
                                        Case "0001"
                                            rowNew("Kontingentart") = "Standard temporär"
                                            rowNew("ZeigeKontingentart") = True
                                        Case "0002"
                                            rowNew("Kontingentart") = "Standard endgültig"
                                            rowNew("ZeigeKontingentart") = True
                                        Case "0003"
                                            rowNew("Kontingentart") = "Retail"
                                            rowNew("ZeigeKontingentart") = False
                                            If dr("Zzfrist").ToString <> "000" Then
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
                                            m_strMessage = "Fehler: Unbekannte Kontingentart (" & dr("Kkber").ToString & ")."
                                            Exit Try
                                    End Select
                                    rowNew("Richtwert_Alt") = CInt(dr("Zzrwert"))
                                    rowNew("Richtwert_Neu") = rowNew("Richtwert_Alt")
                                    rowNew("Kontingent_Alt") = CInt(dr("Klimk"))
                                    rowNew("Kontingent_Neu") = rowNew("Kontingent_Alt")
                                    If IsNumeric(dr("Skfor").ToString.Trim(" "c)) Then
                                        rowNew("Ausschoepfung") = CInt(dr("Skfor"))
                                    Else
                                        rowNew("Ausschoepfung") = 0
                                    End If
                                    rowNew("Frei") = CInt(rowNew("Kontingent_Alt")) - CInt(rowNew("Ausschoepfung"))
                                    If dr("Crblb").ToString = "X" Then
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

                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    'End If

                    ' Fehler-Logging
                    Dim strLog As String = ""

                    If m_strCustomer <> String.Empty Then
                        strLog = "KUNNR=" & Right(m_strCustomer, 5) & " , "
                    End If

                    WriteLogEntry(False, strLog & Replace(m_strMessage, "<br>", " "))
                Finally
                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                    'End If

                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub ShowAll(ByVal haendler As DataView, Optional ByVal zeigeAlle As Boolean = False)
            'Zeigt alle Händler an
            m_strClassAndMethod = "BankBaseCredit.ShowAll"
            If Not m_blnGestartet Then
                m_blnGestartet = True

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                Try
                    'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_M_Creditlimit_Detail", m_objApp, m_objUser)

                    m_tblKontingente = New DataTable()
                    With m_tblKontingente.Columns
                        .Add("Kunnr", System.Type.GetType("System.String"))
                        .Add("Kreditkontrollbereich", System.Type.GetType("System.String"))
                        .Add("ZeigeKontingentart", System.Type.GetType("System.Boolean"))
                        .Add("Standard_Endg", System.Type.GetType("System.String"))
                        .Add("Standard_Temp", System.Type.GetType("System.String"))
                        .Add("Retail", System.Type.GetType("System.String"))
                        .Add("DP", System.Type.GetType("System.String"))
                        .Add("HEZ", System.Type.GetType("System.String"))
                    End With
                    m_intStatus = 0
                    m_strMessage = ""

                    'm_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Creditlimit_Detail", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                    'myProxy.setImportParameter("I_KNRZE", Right("00" & m_objUser.Organization.OrganizationReference, 2))
                    'myProxy.setImportParameter("I_KONZS", m_strKUNNR)
                    'myProxy.setImportParameter("I_KUNNR", "")
                    'myProxy.setImportParameter("I_VKORG", "1510")

                    'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)

                    S.AP.InitExecute("Z_M_Creditlimit_Detail", "I_KNRZE, I_KONZS, I_KUNNR, I_VKORG",
                                     Right("00" & m_objUser.Organization.OrganizationReference, 2), KUNNR, "", "1510")

                    'Dim CreditAccountDetailTable As DataTable = myProxy.getExportTable("GT_WEB")
                    Dim CreditAccountDetailTable As DataTable = S.AP.GetExportTable("GT_WEB")

                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                    'End If

                    Dim row As DataRow
                    Dim insert As Boolean
                    Dim filiale As String

                    If CreditAccountDetailTable.Rows.Count > 0 Then
                        Dim rowNew As DataRow
                        For Each dr As DataRow In CreditAccountDetailTable.Rows

                            insert = False

                            If Not zeigeAlle Then       'ggf. fremde Händler entfernen...
                                For Each row In haendler.Table.Rows         'Nur eigene Händler nehmen
                                    If Right("0000000000" & ("60" & CType(row("REFERENZ"), String)), 10) = dr("Kunnr").ToString Then
                                        insert = True
                                    End If
                                Next
                            Else
                                insert = True
                            End If

                            If (dr("Kkber").ToString.Length > 0) AndAlso (CInt(dr("Skfor")) <= CInt(dr("Klimk"))) Then   'Nur wenn Inanspruchnahme > Kontingent
                                insert = False
                            End If

                            If (dr("Kkber").ToString.Length > 0) AndAlso (CInt(dr("Skfor")) <= CInt(dr("Zzrwert"))) Then 'oder Inanspruchnahme > Richtwert
                                insert = False
                            End If

                            If (dr("Kkber").ToString.Length > 0) AndAlso (insert = True) Then
                                filiale = Right(CType(dr("Kunnr"), String), 5)

                                rowNew = m_tblKontingente.NewRow
                                rowNew("Kunnr") = filiale
                                rowNew("Kreditkontrollbereich") = dr("Kkber").ToString
                                Select Case dr("Kkber").ToString
                                    Case "0001"
                                        rowNew("Standard_Temp") = CType(CInt(dr("Klimk")) - CInt(dr("Skfor")), String)
                                        rowNew("ZeigeKontingentart") = True
                                    Case "0002"
                                        rowNew("Standard_Endg") = CType(CInt(dr("Klimk")) - CInt(dr("Skfor")), String)
                                        rowNew("ZeigeKontingentart") = True
                                    Case "0003"
                                        rowNew("Retail") = CType(CInt(dr("Zzrwert")) - CInt(dr("Skfor")), String)
                                        rowNew("ZeigeKontingentart") = False
                                    Case "0004"
                                        rowNew("DP") = CType(CInt(dr("Zzrwert")) - CInt(dr("Skfor")), String)
                                        rowNew("ZeigeKontingentart") = False
                                    Case "0005"
                                        rowNew("HEZ") = CType(CInt(dr("Zzrwert")) - CInt(dr("Skfor")), String)
                                        rowNew("ZeigeKontingentart") = False
                                    Case Else
                                        m_intStatus = -2203
                                        m_strMessage = "Fehler: Unbekannte Kontingentart (" & dr("Kkber").ToString & ")."
                                        Exit Try
                                End Select
                                m_tblKontingente.Rows.Add(rowNew)
                            End If
                        Next

                        Dim col As DataColumn
                        For Each col In m_tblKontingente.Columns
                            col.ReadOnly = False
                        Next
                        
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

                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    'End If

                    ' Fehler-Logging
                    Dim strLog As String = ""

                    If m_strCustomer <> String.Empty Then
                        strLog = "KUNNR=" & Right(m_strCustomer, 5) & " , "
                    End If

                    WriteLogEntry(False, strLog & Replace(m_strMessage, "<br>", " "))

                Finally
                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                    'End If

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

                                'm_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Creditlimit_Change", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                                m_Hez = False
                                If (CType(tmpRow("Kreditkontrollbereich"), String) = "0005") Then
                                    m_Hez = True
                                End If

                                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxyNoPage("Z_M_Creditlimit_Change", m_objApp, m_objUser)

                                'myProxy.setImportParameter("I_ERNAM", m_strInternetUser)
                                'myProxy.setImportParameter("I_KKBER", CType(tmpRow("Kreditkontrollbereich"), String))
                                'myProxy.setImportParameter("I_KLIMK", CStr(decKreditlimit_Neu))
                                'myProxy.setImportParameter("I_ZZRWERT", CStr(decRichtwert_Neu))
                                'myProxy.setImportParameter("I_KONZS", m_strKUNNR)
                                'myProxy.setImportParameter("I_KUNNR", m_strCustomer)

                                S.AP.Init("Z_M_Creditlimit_Change", "I_ERNAM, I_KKBER, I_KLIMK, I_ZZRWERT, I_KONZS, I_KUNNR",
                                          m_strInternetUser, CType(tmpRow("Kreditkontrollbereich"), String), CStr(decKreditlimit_Neu), CStr(decRichtwert_Neu), KUNNR, m_strCustomer)

                                'If blnGesperrt_Neu Then
                                '    myProxy.setImportParameter("I_CRBLB", "X")
                                '    If Not m_hez Then
                                '        myProxy.setImportParameter("I_HEZKZ", " ")
                                '    Else
                                '        myProxy.setImportParameter("I_HEZKZ", "X")
                                '    End If
                                'Else
                                '    myProxy.setImportParameter("I_CRBLB", " ")
                                '    If Not m_hez Then
                                '        myProxy.setImportParameter("I_HEZKZ", " ")
                                '    Else
                                '        myProxy.setImportParameter("I_HEZKZ", "X")
                                '    End If
                                'End If

                                If blnGesperrt_Neu Then
                                    S.AP.SetImportParameter("I_CRBLB", "X")
                                    If Not m_hez Then
                                        S.AP.SetImportParameter("I_HEZKZ", " ")
                                    Else
                                        S.AP.SetImportParameter("I_HEZKZ", "X")
                                    End If
                                Else
                                    S.AP.SetImportParameter("I_CRBLB", " ")
                                    If Not m_hez Then
                                        S.AP.SetImportParameter("I_HEZKZ", " ")
                                    Else
                                        S.AP.SetImportParameter("I_HEZKZ", "X")
                                    End If
                                End If

                                'myProxy.callBapiNoPage(m_strAppID, m_strSessionID)
                                S.AP.Execute()

                                ' SAP-Performance Logging
                                'If m_intIDSAP > -1 Then
                                '    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                                'End If
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
                    ' SAP-Performance Logging
                    'If m_intIDSAP > -1 Then
                    '    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    'End If
                Finally
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
