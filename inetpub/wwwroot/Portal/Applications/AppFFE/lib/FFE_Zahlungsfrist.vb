Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports Microsoft.Data.SAPClient
Imports CKG.Base.Business

<Serializable()> _
Public Class FFE_Zahlungsfrist
    Inherits FFE_BankBasis

#Region "Declarations"

    Public Const COLUMN_KONTINGENTART As String = "Kontingentart"
    Public Const COLUMN_KONTINGENTID As String = "KontingentID"
    Public Const COLUMN_ZAHLUNGSFRIST_ALT As String = "Alte Zahlungsfrist"
    Public Const COLUMN_ZAHLUNGSFRIST_NEU As String = "Neue Zahlungsfrist"

    Private m_dtZahlungsfrist As DataTable
    Private m_Haendler As DataTable
    Private m_RowFaelligkeit As Integer
    Private m_Kunnr As String
    Private mFaelligkeitenAnzeigeTable As DataTable
    Private mSapTable As DataTable


    Private objFDDBank As FFE_BankBase
#End Region

#Region "Properties"

    Public Property Zahlungsfristen() As DataTable
        Get
            Return Me.m_dtZahlungsfrist
        End Get
        Set(ByVal Value As DataTable)
            Me.m_dtZahlungsfrist = Value
        End Set
    End Property

    Public ReadOnly Property SapTabelle() As DataTable
        Get
            Return mSapTable
        End Get
    End Property


    Public ReadOnly Property FaelligkeitenAnzeigeTable() As DataTable
        Get
            Return mFaelligkeitenAnzeigeTable
        End Get
    End Property


    Public Property RowFaelligkeit() As Integer
        Get
            Return Me.m_RowFaelligkeit
        End Get
        Set(ByVal Value As Integer)
            Me.m_RowFaelligkeit = Value
        End Set
    End Property

    Public Property KunnrFrist() As String
        Get
            Return m_Kunnr
        End Get
        Set(ByVal Value As String)
            m_Kunnr = Right("0000000000" & Value, 10)
        End Set
    End Property
#End Region

#Region "Constructor"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

        'DataTable anlegen
        Me.m_dtZahlungsfrist = New DataTable()
        With Me.m_dtZahlungsfrist.Columns
            .Add(COLUMN_KONTINGENTID)
            .Add(COLUMN_KONTINGENTART)
            .Add(COLUMN_ZAHLUNGSFRIST_ALT)
            .Add(COLUMN_ZAHLUNGSFRIST_NEU)
        End With

    End Sub

#End Region

#Region "Methods"

    Public Overrides Sub Show()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            MakeDestination()
            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1


            Try
                m_intStatus = 0
                m_strMessage = ""

                m_intIDSAP = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_V_Zahlungsfrist_Get", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                Dim out_zahlungsfrist As String = ""
                'DataTable leeren (falls gefüllt)
                Me.m_dtZahlungsfrist.Clear()
                'Neue Row hinzufügen
                Dim i As Integer
                Dim sKKBer As String


                Dim row As DataRow = Me.m_dtZahlungsfrist.NewRow()

                For i = 1 To 4
                    Try
                        sKKBer = "000" & i
                        objSAP.Z_M_Zahlungsfrist_Get_Fce(KUNNR, sKKBer, m_strCustomer, out_zahlungsfrist)
                        objSAP.CommitWork()

                        row = Me.m_dtZahlungsfrist.NewRow()
                        Select Case i
                            Case 1
                                row(COLUMN_KONTINGENTART) = "Standard Temporär"
                                row(COLUMN_KONTINGENTID) = "0001"
                                row(COLUMN_ZAHLUNGSFRIST_ALT) = out_zahlungsfrist
                                Me.m_dtZahlungsfrist.Rows.Add(row)
                            Case 2
                                row(COLUMN_KONTINGENTART) = "Standard endgültig"
                                row(COLUMN_KONTINGENTID) = "0002"
                                row(COLUMN_ZAHLUNGSFRIST_ALT) = out_zahlungsfrist
                                Me.m_dtZahlungsfrist.Rows.Add(row)
                            Case 3
                                row(COLUMN_KONTINGENTART) = "Retail"
                                row(COLUMN_KONTINGENTID) = "0003"
                                row(COLUMN_ZAHLUNGSFRIST_ALT) = out_zahlungsfrist
                                Me.m_dtZahlungsfrist.Rows.Add(row)
                            Case 4
                                row(COLUMN_KONTINGENTART) = "Delayed payment"
                                row(COLUMN_KONTINGENTID) = "0004"
                                row(COLUMN_ZAHLUNGSFRIST_ALT) = out_zahlungsfrist
                                Me.m_dtZahlungsfrist.Rows.Add(row)
                                'Case 5
                                '    row(COLUMN_KONTINGENTART) = "KF/KL"
                                '    row(COLUMN_KONTINGENTID) = "0006"
                                '    row(COLUMN_ZAHLUNGSFRIST_ALT) = out_zahlungsfrist
                                '    Me.m_dtZahlungsfrist.Rows.Add(row)
                        End Select

                        If i = 4 Then
                            sKKBer = "000" & 6 ' für KF/KL
                            objSAP.Z_M_Zahlungsfrist_Get_Fce(KUNNR, sKKBer, m_strCustomer, out_zahlungsfrist)
                            objSAP.CommitWork()

                            row = Me.m_dtZahlungsfrist.NewRow()
                            row(COLUMN_KONTINGENTART) = "KF/KL"
                            row(COLUMN_KONTINGENTID) = "0006"
                            row(COLUMN_ZAHLUNGSFRIST_ALT) = out_zahlungsfrist
                            Me.m_dtZahlungsfrist.Rows.Add(row)
                        End If


                    Catch ex As Exception
                        Select Case ex.Message
                            Case "NRF"
                                row = Me.m_dtZahlungsfrist.NewRow()
                                Select Case i
                                    Case 1
                                        row(COLUMN_KONTINGENTART) = "Standard Temporär"
                                        row(COLUMN_KONTINGENTID) = "0001"
                                    Case 2
                                        row(COLUMN_KONTINGENTART) = "Standard endgültig"
                                        row(COLUMN_KONTINGENTID) = "0002"
                                    Case 3
                                        row(COLUMN_KONTINGENTART) = "Retail"
                                        row(COLUMN_KONTINGENTID) = "0003"
                                    Case 4
                                        row(COLUMN_KONTINGENTART) = "Delayed payment"
                                        row(COLUMN_KONTINGENTID) = "0004"
                                    Case 5
                                        row(COLUMN_KONTINGENTART) = "KF/KL"
                                        row(COLUMN_KONTINGENTID) = "0006"

                                End Select
                                row(COLUMN_ZAHLUNGSFRIST_ALT) = "000"
                                Me.m_dtZahlungsfrist.Rows.Add(row)
                            Case Else
                                m_intStatus = -9999
                                m_strMessage = ex.Message
                        End Select
                    End Try
                Next



                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                WriteLogEntry(True, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=", Nothing)
            Catch ex As Exception
                Select Case ex.Message
                    Case "NRF"
                        m_intStatus = -1402
                        m_strMessage = "Keine Daten gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDsap > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDsap, False, m_strMessage)
                End If
                WriteLogEntry(False, "KONZS=" & m_strCustomer.TrimStart("0"c) & ", KNRZE=" & m_strFiliale & ", KUNNR=, KKBER=" & " , " & Replace(m_strMessage, "<br>", " "), Nothing)

                If m_intIDsap > -1 Then
                    m_objlogApp.WriteStandardDataAccessSAP(m_intIDsap)
                End If

            Finally
                objSAP.Connection.Close()
                objSAP.Dispose()
                m_blnGestartet = False
            End Try

        End If

    End Sub

    Public Overrides Sub Change()

        If Not m_blnGestartet Then
            m_blnGestartet = True

            MakeDestination()
            Dim objSAP As New SAPProxy_FFE.SAPProxy_FFE()
            objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
            objSAP.Connection.Open()

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            Try
                m_intStatus = 0
                m_strMessage = ""

                If Me.m_dtZahlungsfrist.Rows.Count = 0 Then
                    Throw New Exception("Es sind keine Daten zum Speichern vorhanden.")
                End If

                'objSAP.Z_V_Zahlungsfrist_Put("0002", Me.m_strCustomer, Me.m_dtZahlungsfrist.Rows(0)(COLUMN_ZAHLUNGSFRIST_NEU).ToString())

                Dim sKKBER As String = Me.m_dtZahlungsfrist.Rows(Me.m_RowFaelligkeit)(COLUMN_KONTINGENTID).ToString()
                Dim sZfrist As String = Me.m_dtZahlungsfrist.Rows(Me.m_RowFaelligkeit)(COLUMN_ZAHLUNGSFRIST_NEU).ToString()
                'Wert auch umkopieren neu -> alt
                Me.m_dtZahlungsfrist.Rows(Me.m_RowFaelligkeit)(COLUMN_ZAHLUNGSFRIST_ALT) = Me.m_dtZahlungsfrist.Rows(Me.m_RowFaelligkeit)(COLUMN_ZAHLUNGSFRIST_NEU).ToString()

                objSAP.Z_M_Zahlungsfrist_Put_Fce(Me.m_strKUNNR, Me.m_strCustomer, sKKBER, sZfrist)

                objSAP.CommitWork()

                If m_intIDSAP > -1 Then
                    m_objlogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If



            Catch ex As Exception
                Select Case ex.Message
                    Case "NRF"
                        m_intStatus = -1402
                        m_strMessage = "Keine Daten gefunden."
                    Case "WERROR"
                        m_intStatus = -9998
                        m_strMessage = "Fehler beim Speichern"
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

    Public Sub ShowKontingentDetails()
        '----------------------------------------------------------------------
        ' Methode: ShowKontingentDetails
        ' Autor: JJU
        ' Beschreibung: Ruft das Bapi Z_M_GET_CREDIT_GRUND auf 
        ' Erstellt am: 2008.09.11
        ' ITA: 2216
        '----------------------------------------------------------------------

        m_strClassAndMethod = "FFE_ZAHLUNGSFRIST.ShowKontingentDetails"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intStatus = 0
            Dim intID As Int32 = -1
            Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            con.Open()

            Try
                Dim cmd As New SAPCommand()
                cmd.Connection = con

                Dim strCom As String

                strCom = "EXEC Z_M_GET_CREDIT_GRUND @I_AG=@pI_AG,@I_HAENDLER=@pI_HAENDLER,"
                strCom = strCom & "@GT_CREDITGRUND=@pI_GT_CREDITGRUND,"
                strCom = strCom & "@GT_CREDITGRUND=@pE_GT_CREDITGRUND OUTPUT OPTION 'disabledatavalidation'"

                cmd.CommandText = strCom

                'importparameter
                Dim pI_AG As New SAPParameter("@pI_AG", ParameterDirection.Input)
                Dim pI_HAENDLER As New SAPParameter("@pI_HAENDLER", ParameterDirection.Input)
                Dim pI_GT_CREDITGRUND As New SAPParameter("@pI_GT_CREDITGRUND", New DataTable)

                'exportParameter
                Dim pE_GT_CREDITGRUND As New SAPParameter("@pE_GT_CREDITGRUND", ParameterDirection.Output)

                'Importparameter hinzufügen
                cmd.Parameters.Add(pI_AG)
                cmd.Parameters.Add(pI_HAENDLER)
                cmd.Parameters.Add(pI_GT_CREDITGRUND)

                'exportparameter hinzugfügen
                cmd.Parameters.Add(pE_GT_CREDITGRUND)

                'befüllen der Importparameter
                pI_AG.Value = Right("0000000000" & m_objUser.KUNNR, 10)
                pI_HAENDLER.Value = Right("0000000000" & Customer, 10)

                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_BRIEFANF_ERROR_LIST", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                cmd.ExecuteNonQuery()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

                Dim tblTemp As DataTable = DirectCast(pE_GT_CREDITGRUND.Value, DataTable)
                HelpProcedures.killAllDBNullValuesInDataTable(tblTemp)
                mSapTable = tblTemp.Clone 'ist die tabelle die auch wieder ins sap zurück geschrieben wird

                mFaelligkeitenAnzeigeTable = tblTemp

                mFaelligkeitenAnzeigeTable.Columns.Add("NeueZFProAG", System.Type.GetType("System.String"))
                mFaelligkeitenAnzeigeTable.Columns.Add("AbrufGrundText", System.Type.GetType("System.String"))
                HelpProcedures.killAllDBNullValuesInDataTable(mFaelligkeitenAnzeigeTable)

                For Each tmprow As DataRow In mFaelligkeitenAnzeigeTable.Rows

                    Dim AbrufTyp As String
                    Select Case tmprow("KKBER").ToString
                        Case "0001"
                            AbrufTyp = "temp"
                        Case "0002"
                            AbrufTyp = "endg"
                        Case Else
                            AbrufTyp = "unbekannt"
                    End Select
                    tmprow("AbrufGrundText") = getAbrufgrund(tmprow("ZZVGRUND").ToString, AbrufTyp)
                Next
                mFaelligkeitenAnzeigeTable.AcceptChanges()
            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "HAENDLER_NOT_FOUND"
                        m_strMessage = "Händler nicht vorhanden"
                    Case "AG_NOT_FOUND"
                        m_strMessage = "Keine Kundennummer"
                    Case "NO_DATA"
                        m_intStatus = -1111
                        m_strMessage = "Es wurden keine Daten gefunden"
                    Case "AG_NO_CREDIT_CHECK"
                        m_strMessage = "AG hate die Kreditlimitprüfung nicht aktiv"
                    Case "AG_CUST_NOT_FOUND"
                        m_strMessage = "AG nicht im Bankencustomizing angelegt"
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

                con.Close()
                m_blnGestartet = False
            End Try
        End If

    End Sub

    Public Sub changeFristDetails()

        '----------------------------------------------------------------------
        ' Methode: changeFristDetails
        ' Autor: JJU
        ' Beschreibung: Ruft das Bapi Z_M_PUT_CREDIT_GRUND auf 
        ' Erstellt am: 2008.09.15
        ' ITA: 2216
        '----------------------------------------------------------------------

        m_strClassAndMethod = "FFE_ZAHLUNGSFRIST.changeFristDetails"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intStatus = 0
            Dim intID As Int32 = -1
            Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            con.Open()

            Try
                Dim cmd As New SAPCommand()
                cmd.Connection = con

                Dim strCom As String

                strCom = "EXEC Z_M_PUT_CREDIT_GRUND "
                strCom = strCom & "@GT_CREDITGRUND=@pI_GT_CREDITGRUND,"
                strCom = strCom & "@GT_CREDITGRUND=@pE_GT_CREDITGRUND OUTPUT OPTION 'disabledatavalidation'"

                cmd.CommandText = strCom

                'importparameter
                Dim pI_GT_CREDITGRUND As New SAPParameter("@pI_GT_CREDITGRUND", mSapTable)

                'exportParameter
                Dim pE_GT_CREDITGRUND As New SAPParameter("@pE_GT_CREDITGRUND", ParameterDirection.Output)

                'Importparameter hinzufügen
                cmd.Parameters.Add(pI_GT_CREDITGRUND)

                'exportparameter hinzugfügen
                cmd.Parameters.Add(pE_GT_CREDITGRUND)


                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If
                intID = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_BRIEFANF_ERROR_LIST", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                cmd.ExecuteNonQuery()

                If intID > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(intID, True)
                End If

            Catch ex As Exception
                m_intStatus = -9999
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "HAENDLER_NOT_FOUND"
                        m_strMessage = "Händler nicht vorhanden"
                    Case "AG_NOT_FOUND"
                        m_strMessage = "Keine Kundennummer"
                    Case "UPDATE_ERROR"
                        m_intStatus = -1112
                        m_strMessage = "Beim Ändern der Daten ist ein Fehler aufgetreten"
                    Case "HAENDLER_NO_CREDITLIMIT"
                        m_strMessage = "Händler hat keine Eintrag in der Kreditlimittabelle"
                    Case "TABLE_LOCK"
                        m_strMessage = "Tabelle zur Zeit gesperrt"
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

                con.Close()
                m_blnGestartet = False
            End Try
        End If

    End Sub

    Private Function getAbrufgrund(ByVal kuerzel As String, ByVal AbrufTyp As String) As String
        Dim cn As New SqlClient.SqlConnection
        Dim cmdAg As SqlClient.SqlCommand
        Dim dsAg As DataSet
        Dim adAg As SqlClient.SqlDataAdapter
        Dim dr As SqlClient.SqlDataReader

        Try

            cn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            cn.Open()
            dsAg = New DataSet()
            adAg = New SqlClient.SqlDataAdapter()

            cmdAg = New SqlClient.SqlCommand("SELECT " & _
                                            "[WebBezeichnung]" & _
                                            "FROM CustomerAbrufgruende " & _
                                            "WHERE " & _
                                            "CustomerID =' " & m_objUser.Customer.CustomerId.ToString & "' AND " & _
                                            "SapWert='" & kuerzel & "'" & _
                                            " AND AbrufTyp ='" & AbrufTyp & "'" _
                                               , cn)
            cmdAg.CommandType = CommandType.Text
            dr = cmdAg.ExecuteReader()

            If dr.Read() = True Then
                If dr.IsDBNull(0) Then
                    Return String.Empty
                Else
                    Return CStr(dr.Item("WebBezeichnung"))
                End If
            End If
            Return Nothing
        Catch ex As Exception

            Throw ex
            Return Nothing
        Finally
            cn.Close()

        End Try
    End Function


#End Region
End Class
' ************************************************
' $History: FFE_Zahlungsfrist.vb $
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 30.04.09   Time: 15:42
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2837
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 16.09.08   Time: 16:44
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2216 testfertig
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 15.09.08   Time: 10:48
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2216 unfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 12.09.08   Time: 15:31
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA 2216 unfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 7.07.08    Time: 10:40
' Updated in $/CKAG/Applications/AppFFE/lib
' SSV Historien hinzugefgt
' 
' ************************************************
