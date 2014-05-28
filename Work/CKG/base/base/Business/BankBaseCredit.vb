Option Explicit On 
Option Strict On

Imports System

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

                Dim objSAP As New SAPProxy_Base.SAPProxy_Base()

                Dim CreditAccountDetail As SAPProxy_Base.ZCREDITCONTROL
                Dim CreditAccountDetailTable As New SAPProxy_Base.ZCREDITCONTROLTable()

                MakeDestination()
                objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                objSAP.Connection.Open()

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                Try
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
                    m_intStatus = 0
                    m_strMessage = ""

                    If CheckCustomerData() Then

                        m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Creditlimit_Detail", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                        objSAP.Z_M_Creditlimit_Detail(Left(m_objUser.Reference, 2), Right("0000000000" & m_objUser.KUNNR, 10), Right(m_strCustomer, 5), "1510", CreditAccountDetailTable)  'HEZ: Parameterliste angepasst am 09.05.2005
                        objSAP.CommitWork()
                        If m_intIDSAP > -1 Then
                            m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                        End If

                        If CreditAccountDetailTable.Count > 0 Then
                            Dim rowNew As DataRow
                            For Each CreditAccountDetail In CreditAccountDetailTable
                                If CreditAccountDetail.Kkber.Length > 0 Then
                                    rowNew = m_tblKontingente.NewRow
                                    rowNew("Kreditkontrollbereich") = CreditAccountDetail.Kkber
                                    Select Case CreditAccountDetail.Kkber
                                        Case "0001"
                                            rowNew("Kontingentart") = "Standard temporär"
                                            rowNew("ZeigeKontingentart") = True
                                        Case "0002"
                                            rowNew("Kontingentart") = "Standard endgültig"
                                            rowNew("ZeigeKontingentart") = True
                                        Case "0003"
                                            rowNew("Kontingentart") = "Retail"
                                            rowNew("ZeigeKontingentart") = False
                                            If CreditAccountDetail.Zzfrist.ToString <> "000" Then
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
                                            m_strMessage = "Fehler: Unbekannte Kontingentart (" & CreditAccountDetail.Kkber & ")."
                                            Exit Try
                                    End Select
                                    rowNew("Richtwert_Alt") = CInt(CreditAccountDetail.Zzrwert)
                                    rowNew("Richtwert_Neu") = rowNew("Richtwert_Alt")
                                    rowNew("Kontingent_Alt") = CInt(CreditAccountDetail.Klimk)
                                    rowNew("Kontingent_Neu") = rowNew("Kontingent_Alt")
                                    If IsNumeric(CreditAccountDetail.Skfor.ToString.Trim(" "c)) Then
                                        rowNew("Ausschoepfung") = CInt(CreditAccountDetail.Skfor)
                                    Else
                                        rowNew("Ausschoepfung") = 0
                                    End If
                                    rowNew("Frei") = CInt(rowNew("Kontingent_Alt")) - CInt(rowNew("Ausschoepfung"))
                                    If CreditAccountDetail.Crblb = "X" Then
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
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    End If
                    WriteLogEntry(False, "KUNNR=" & Right(m_strCustomer, 5) & " , " & Replace(m_strMessage, "<br>", " "), m_tblKontingente)
                Finally
                    If m_intIDsap > -1 Then
                        m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
                    End If

                    objSAP.Connection.Close()
                    objSAP.Dispose()

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

                Dim objSAP As New SAPProxy_Base.SAPProxy_Base()

                Dim CreditAccountDetail As SAPProxy_Base.ZCREDITCONTROL
                Dim CreditAccountDetailTable As New SAPProxy_Base.ZCREDITCONTROLTable()

                MakeDestination()
                objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                objSAP.Connection.Open()

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                Try
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

                    m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Creditlimit_Detail", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                    objSAP.Z_M_Creditlimit_Detail(Right("00" & m_objUser.Organization.OrganizationReference, 2), Right("0000000000" & m_objUser.KUNNR, 10), "", "1510", CreditAccountDetailTable)  'HEZ: Parameterliste angepasst am 09.05.2005
                    objSAP.CommitWork()
                    If m_intIDSAP > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                    End If

                    Dim row As DataRow
                    Dim insert As Boolean
                    Dim filiale As String

                    If CreditAccountDetailTable.Count > 0 Then
                        Dim rowNew As DataRow
                        For Each CreditAccountDetail In CreditAccountDetailTable

                            insert = False

                            If Not zeigeAlle Then       'ggf. fremde Händler entfernen...
                                For Each row In haendler.Table.Rows         'Nur eigene Händler nehmen
                                    If Right("0000000000" & ("60" & CType(row("REFERENZ"), String)), 10) = CreditAccountDetail.Kunnr Then
                                        insert = True
                                    End If
                                Next
                            Else
                                insert = True
                            End If

                            If (CreditAccountDetail.Kkber.Length > 0) AndAlso (CInt(CreditAccountDetail.Skfor) <= CInt(CreditAccountDetail.Klimk)) Then   'Nur wenn Inanspruchnahme > Kontingent
                                insert = False
                            End If

                            If (CreditAccountDetail.Kkber.Length > 0) AndAlso (CInt(CreditAccountDetail.Skfor) <= CInt(CreditAccountDetail.Zzrwert)) Then 'oder Inanspruchnahme > Richtwert
                                insert = False
                            End If

                            If (CreditAccountDetail.Kkber.Length > 0) AndAlso (insert = True) Then
                                filiale = Right(CType(CreditAccountDetail.Kunnr, String), 5)

                                rowNew = m_tblKontingente.NewRow
                                rowNew("Kunnr") = filiale
                                rowNew("Kreditkontrollbereich") = CreditAccountDetail.Kkber
                                Select Case CreditAccountDetail.Kkber
                                    Case "0001"
                                        rowNew("Standard_Temp") = CType(CInt(CreditAccountDetail.Klimk) - CInt(CreditAccountDetail.Skfor), String)
                                        rowNew("ZeigeKontingentart") = True
                                    Case "0002"
                                        rowNew("Standard_Endg") = CType(CInt(CreditAccountDetail.Klimk) - CInt(CreditAccountDetail.Skfor), String)
                                        rowNew("ZeigeKontingentart") = True
                                    Case "0003"
                                        rowNew("Retail") = CType(CInt(CreditAccountDetail.Zzrwert) - CInt(CreditAccountDetail.Skfor), String)
                                        rowNew("ZeigeKontingentart") = False
                                    Case "0004"
                                        rowNew("DP") = CType(CInt(CreditAccountDetail.Zzrwert) - CInt(CreditAccountDetail.Skfor), String)
                                        rowNew("ZeigeKontingentart") = False
                                    Case "0005"
                                        rowNew("HEZ") = CType(CInt(CreditAccountDetail.Zzrwert) - CInt(CreditAccountDetail.Skfor), String)
                                        rowNew("ZeigeKontingentart") = False
                                    Case Else
                                        m_intStatus = -2203
                                        m_strMessage = "Fehler: Unbekannte Kontingentart (" & CreditAccountDetail.Kkber & ")."
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
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    End If
                    WriteLogEntry(False, "KUNNR=" & Right(m_strCustomer, 5) & " , " & Replace(m_strMessage, "<br>", " "), m_tblKontingente)
                Finally
                    If m_intIDsap > -1 Then
                        m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
                    End If

                    objSAP.Connection.Close()
                    objSAP.Dispose()

                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Overrides Sub Change()
            If Not m_blnGestartet Then
                m_blnGestartet = True

                Dim objSAP As New SAPProxy_Base.SAPProxy_Base()

                MakeDestination()
                objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                objSAP.Connection.Open()

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
                                Dim tblAuftraege As New SAPProxy_Base.ZDAD_M_WEB_AUFTRAEGETable
                                m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Creditlimit_Change", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                                m_Hez = False
                                If (CType(tmpRow("Kreditkontrollbereich"), String) = "0005") Then
                                    m_Hez = True
                                End If

                                If blnGesperrt_Neu Then
                                    If Not m_Hez Then
                                        objSAP.Z_M_Creditlimit_Change("X", m_strInternetUser, " ", CType(tmpRow("Kreditkontrollbereich"), String), decKreditlimit_Neu, m_strKUNNR, m_strCustomer, decRichtwert_Neu, tblAuftraege)    '01.06.2005 HEZ
                                    Else
                                        objSAP.Z_M_Creditlimit_Change("X", m_strInternetUser, "X", CType(tmpRow("Kreditkontrollbereich"), String), decKreditlimit_Neu, m_strKUNNR, m_strCustomer, decRichtwert_Neu, tblAuftraege)   '01.06.2005 HEZ
                                    End If
                                Else
                                    If Not m_Hez Then
                                        objSAP.Z_M_Creditlimit_Change(" ", m_strInternetUser, " ", CType(tmpRow("Kreditkontrollbereich"), String), decKreditlimit_Neu, m_strKUNNR, m_strCustomer, decRichtwert_Neu, tblAuftraege)    '01.06.2005 HEZ
                                    Else
                                        objSAP.Z_M_Creditlimit_Change(" ", m_strInternetUser, "X", CType(tmpRow("Kreditkontrollbereich"), String), decKreditlimit_Neu, m_strKUNNR, m_strCustomer, decRichtwert_Neu, tblAuftraege)   '01.06.2005 HEZ
                                    End If
                                End If
                                objSAP.CommitWork()
                                If m_intIDSAP > -1 Then
                                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                                End If
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
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    End If
                Finally
                    objSAP.Connection.Close()
                    objSAP.Dispose()

                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub ShowStandard()
            '§§§ JVE 17.07.2006: Neues BAPI (zunächst nur für Porsche, soll später Standard werden).
            m_strClassAndMethod = "BankBaseCredit.ShowStandard"
            If Not m_blnGestartet Then
                m_blnGestartet = True

                Dim objSAP As New SAPProxy_Base.SAPProxy_Base()

                Dim CreditAccountDetail As SAPProxy_Base.ZDAD_M_WEB_CREDITLIMIT ' .ZCREDITCONTROL
                Dim CreditAccountDetailTable As New SAPProxy_Base.ZDAD_M_WEB_CREDITLIMITTable() ' .ZCREDITCONTROLTable()

                MakeDestination()
                objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                objSAP.Connection.Open()

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                m_intIDSAP = -1

                Try
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
                    End With
                    m_intStatus = 0
                    m_strMessage = ""

                    If CheckCustomerData() Then

                        m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Creditlimit", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                        '§§§ JVE 18.07.2006: Customer 10 - Stellig (mit "60" und führenden Nullen).
                        objSAP.Z_M_Creditlimit(Right("0000000000" & m_objUser.KUNNR, 10), Right("0000000000" & m_strCustomer, 10), "Change04.aspx", CreditAccountDetailTable)  'HEZ: Parameterliste angepasst am 09.05.2005
                        objSAP.CommitWork()
                        If m_intIDSAP > -1 Then
                            m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                        End If

                        If CreditAccountDetailTable.Count > 0 Then
                            Dim rowNew As DataRow
                            For Each CreditAccountDetail In CreditAccountDetailTable
                                If CreditAccountDetail.Kkber.Length > 0 Then
                                    rowNew = m_tblKontingente.NewRow
                                    rowNew("Kreditkontrollbereich") = CreditAccountDetail.Kkber
                                    Select Case CreditAccountDetail.Kkber
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
                                            m_strMessage = "Fehler: Unbekannte Kontingentart (" & CreditAccountDetail.Kkber & ")."
                                            Exit Try
                                    End Select
                                    'rowNew("Richtwert_Alt") = CInt(CreditAccountDetail.Zzrwert)
                                    'rowNew("Richtwert_Neu") = rowNew("Richtwert_Alt")
                                    rowNew("Kontingent_Alt") = CInt(CreditAccountDetail.Klimk)
                                    rowNew("Kontingent_Neu") = rowNew("Kontingent_Alt")
                                    If IsNumeric(CreditAccountDetail.Skfor.ToString.Trim(" "c)) Then
                                        rowNew("Ausschoepfung") = CInt(CreditAccountDetail.Skfor)
                                    Else
                                        rowNew("Ausschoepfung") = 0
                                    End If
                                    rowNew("Frei") = CInt(rowNew("Kontingent_Alt")) - CInt(rowNew("Ausschoepfung"))
                                    If CreditAccountDetail.Crblb = "X" Then
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
                        m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                    End If
                    WriteLogEntry(False, "KUNNR=" & Right(m_strCustomer, 5) & " , " & Replace(m_strMessage, "<br>", " "), m_tblKontingente)
                Finally
                    If m_intIDsap > -1 Then
                        m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
                    End If

                    objSAP.Connection.Close()
                    objSAP.Dispose()

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
