
Public Class ComCommon

#Region "Declarations"

    Private mE_SUBRC As Integer
    Private mE_MESSAGE As String
    Private m_Laender As DataTable
    Private m_Funktion As DataTable
    Private m_Branchen As DataTable
    Private m_MitarbeiterNr As String
    Private m_Abruftyp As String
    Private m_EinzugEr As String
    Private m_Anrede As String
    Private m_Branche As String
    Private m_BrancheFreitext As String
    Private m_Name1 As String
    Private m_Name2 As String
    Private m_Strasse As String
    Private m_HausNr As String
    Private m_PLZ As String
    Private m_Ort As String
    Private m_Land As String
    Private m_UIDNummer As String
    Private m_ASPName As String
    Private m_ASPVorname As String
    Private m_strFunktion As String
    Private m_Telefon As String
    Private m_Mobil As String
    Private m_Fax As String
    Private m_Mail As String
    Private mMyKasse As Kasse
    Private m_NeueKUNNR As String

    Private mtblMailings As DataTable

    Private m_strMailBody As String = ""
    Private m_strMailAdress As String = ""
    Private m_strMailAdressCC As String = ""
    Private m_strBetreff As String = ""

    Private mListeAusparken As DataTable

    Protected m_intStatus As Int32
    Protected m_strMessage As String
    Dim SAPExc As SAPExecutor.SAPExecutor

#End Region

#Region "Properties"

    ReadOnly Property Laender() As DataTable
        Get
            Return m_Laender
        End Get
    End Property

    ReadOnly Property Funktionen() As DataTable
        Get
            Return m_Funktion
        End Get
    End Property

    ReadOnly Property Branchen() As DataTable
        Get
            Return m_Branchen
        End Get
    End Property

    Public Property E_SUBRC() As Integer
        Get
            Return mE_SUBRC
        End Get
        Set(ByVal Value As Integer)
            mE_SUBRC = Value
        End Set
    End Property

    Public Property E_MESSAGE() As String
        Get
            Return mE_MESSAGE
        End Get
        Set(ByVal Value As String)
            mE_MESSAGE = Value
        End Set
    End Property

    Public Property MitarbeiterNr() As String
        Get
            Return m_MitarbeiterNr
        End Get
        Set(ByVal Value As String)
            m_MitarbeiterNr = Value
        End Set
    End Property

    Public Property Abruftyp() As String
        Get
            Return m_Abruftyp
        End Get
        Set(ByVal Value As String)
            m_Abruftyp = Value
        End Set
    End Property

    Public Property EinzugEr() As String
        Get
            Return m_EinzugEr
        End Get
        Set(ByVal Value As String)
            m_EinzugEr = Value
        End Set
    End Property

    Public Property Anrede() As String
        Get
            Return m_Anrede
        End Get
        Set(ByVal Value As String)
            m_Anrede = Value
        End Set
    End Property

    Public Property Branche() As String
        Get
            Return m_Branche
        End Get
        Set(ByVal Value As String)
            m_Branche = Value
        End Set
    End Property

    Public Property BrancheFreitext() As String
        Get
            Return m_BrancheFreitext
        End Get
        Set(ByVal Value As String)
            m_BrancheFreitext = Value
        End Set
    End Property

    Public Property Name1() As String
        Get
            Return m_Name1
        End Get
        Set(ByVal Value As String)
            m_Name1 = Value
        End Set
    End Property

    Public Property Name2() As String
        Get
            Return m_Name2
        End Get
        Set(ByVal Value As String)
            m_Name2 = Value
        End Set
    End Property

    Public Property Strasse() As String
        Get
            Return m_Strasse
        End Get
        Set(ByVal Value As String)
            m_Strasse = Value
        End Set
    End Property

    Public Property PLZ() As String
        Get
            Return m_PLZ
        End Get
        Set(ByVal Value As String)
            m_PLZ = Value
        End Set
    End Property

    Public Property HausNr() As String
        Get
            Return m_HausNr
        End Get
        Set(ByVal Value As String)
            m_HausNr = Value
        End Set
    End Property

    Public Property Ort() As String
        Get
            Return m_Ort
        End Get
        Set(ByVal Value As String)
            m_Ort = Value
        End Set
    End Property

    Public Property Land() As String
        Get
            Return m_Land
        End Get
        Set(ByVal Value As String)
            m_Land = Value
        End Set
    End Property

    Public Property UIDNummer() As String
        Get
            Return m_UIDNummer
        End Get
        Set(ByVal Value As String)
            m_UIDNummer = Value
        End Set
    End Property

    Public Property ASPName() As String
        Get
            Return m_ASPName
        End Get
        Set(ByVal Value As String)
            m_ASPName = Value
        End Set
    End Property

    Public Property ASPVorname() As String
        Get
            Return m_ASPVorname
        End Get
        Set(ByVal Value As String)
            m_ASPVorname = Value
        End Set
    End Property

    Public Property Funktion() As String
        Get
            Return m_strFunktion
        End Get
        Set(ByVal Value As String)
            m_strFunktion = Value
        End Set
    End Property

    Public Property Telefon() As String
        Get
            Return m_Telefon
        End Get
        Set(ByVal Value As String)
            m_Telefon = Value
        End Set
    End Property

    Public Property Mail() As String
        Get
            Return m_Mail
        End Get
        Set(ByVal Value As String)
            m_Mail = Value
        End Set
    End Property

    Public Property Mobil() As String
        Get
            Return m_Mobil
        End Get
        Set(ByVal Value As String)
            m_Mobil = Value
        End Set
    End Property

    Public Property Fax() As String
        Get
            Return m_Fax
        End Get
        Set(ByVal Value As String)
            m_Fax = Value
        End Set
    End Property

    Public Sub New(ByRef Kasse As Kasse)
        mMyKasse = Kasse
    End Sub

    Public Property Mailings() As DataTable
        Get
            Return mtblMailings
        End Get
        Set(ByVal Value As DataTable)
            mtblMailings = Value
        End Set
    End Property

    Public Property MailBody() As String
        Get
            Return m_strMailBody
        End Get
        Set(ByVal Value As String)
            m_strMailBody = Value
        End Set
    End Property

    Public Property MailAdress() As String
        Get
            Return m_strMailAdress
        End Get
        Set(ByVal Value As String)
            m_strMailAdress = Value
        End Set
    End Property

    Public Property MailAdressCC() As String
        Get
            Return m_strMailAdressCC
        End Get
        Set(ByVal Value As String)
            m_strMailAdressCC = Value
        End Set
    End Property

    Public Property Betreff() As String
        Get
            Return m_strBetreff
        End Get
        Set(ByVal Value As String)
            m_strBetreff = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Int32
        Get
            Return m_intStatus
        End Get
    End Property

    Public ReadOnly Property ListeAusparken() As DataTable
        Get
            Return mListeAusparken
        End Get
    End Property

    Public Property PKennzeichen As String
    Public Property Dokumente As DataTable

#End Region

    Private Function Create_FillInputTab() As DataTable
        Dim tblSAP As New DataTable()
        tblSAP.Columns.Add("BEDIEN", String.Empty.GetType)
        tblSAP.Columns.Add("BUKRS", String.Empty.GetType)
        tblSAP.Columns.Add("VKORG", String.Empty.GetType)
        tblSAP.Columns.Add("VKBUR", String.Empty.GetType)
        tblSAP.Columns.Add("KALKS", String.Empty.GetType)
        tblSAP.Columns.Add("EZERM", String.Empty.GetType)
        tblSAP.Columns.Add("TITLE", String.Empty.GetType)
        tblSAP.Columns.Add("BRSCH", String.Empty.GetType)
        tblSAP.Columns.Add("BRSCH_FREITXT", String.Empty.GetType)
        tblSAP.Columns.Add("NAME1", String.Empty.GetType)
        tblSAP.Columns.Add("NAME2", String.Empty.GetType)
        tblSAP.Columns.Add("NAME3", String.Empty.GetType)
        tblSAP.Columns.Add("NAME4", String.Empty.GetType)
        tblSAP.Columns.Add("STREET", String.Empty.GetType)
        tblSAP.Columns.Add("HOUSE_NUM1", String.Empty.GetType)
        tblSAP.Columns.Add("CITY1", String.Empty.GetType)
        tblSAP.Columns.Add("POST_CODE1", String.Empty.GetType)
        tblSAP.Columns.Add("LAND1", String.Empty.GetType)
        tblSAP.Columns.Add("STCEG", String.Empty.GetType)
        tblSAP.Columns.Add("AP_NAMEV", String.Empty.GetType)
        tblSAP.Columns.Add("AP_NAME1", String.Empty.GetType)
        tblSAP.Columns.Add("AP_PAFKT", String.Empty.GetType)
        tblSAP.Columns.Add("AP_TEL_NUMBER", String.Empty.GetType)
        tblSAP.Columns.Add("AP_MOB_NUMBER", String.Empty.GetType)
        tblSAP.Columns.Add("AP_FAX_NUMBER", String.Empty.GetType)
        tblSAP.Columns.Add("AP_SMTP_ADDR", String.Empty.GetType)
        tblSAP.Columns.Add("QUELLE", String.Empty.GetType)
        tblSAP.Columns.Add("ERNAM", String.Empty.GetType)

        tblSAP.Columns.Add("BANKS", String.Empty.GetType)
        tblSAP.Columns.Add("BANKL", String.Empty.GetType)
        tblSAP.Columns.Add("BNKLZ", String.Empty.GetType)
        tblSAP.Columns.Add("BANKN", String.Empty.GetType)
        tblSAP.Columns.Add("IBAN", String.Empty.GetType)
        tblSAP.Columns.Add("SWIFT", String.Empty.GetType)
        tblSAP.Columns.Add("GRUPPE_T", String.Empty.GetType)
        tblSAP.Columns.Add("UMS_P_MON", String.Empty.GetType)
        tblSAP.Columns.Add("GEB_M_UST", String.Empty.GetType)
        tblSAP.Columns.Add("KREDITVS", String.Empty.GetType)
        tblSAP.Columns.Add("AUSKUNFT", String.Empty.GetType)
        tblSAP.Columns.Add("BEMERKUNG", String.Empty.GetType)

        Dim SapRow As DataRow = tblSAP.NewRow

        SapRow("BEDIEN") = m_MitarbeiterNr
        SapRow("BUKRS") = mMyKasse.Werk
        SapRow("VKORG") = mMyKasse.Werk
        SapRow("VKBUR") = mMyKasse.Lagerort
        SapRow("KALKS") = m_Abruftyp
        SapRow("EZERM") = m_EinzugEr
        SapRow("TITLE") = m_Anrede
        SapRow("BRSCH") = m_Branche
        SapRow("BRSCH_FREITXT") = m_BrancheFreitext
        SapRow("NAME1") = Name1
        SapRow("NAME2") = Name2
        SapRow("NAME3") = ""
        SapRow("NAME4") = ""
        SapRow("STREET") = m_Strasse
        SapRow("CITY1") = m_Ort
        SapRow("HOUSE_NUM1") = m_HausNr
        SapRow("POST_CODE1") = m_PLZ
        SapRow("LAND1") = m_Land
        SapRow("STCEG") = m_UIDNummer
        SapRow("AP_NAMEV") = m_ASPVorname
        SapRow("AP_NAME1") = m_ASPName
        SapRow("AP_PAFKT") = m_strFunktion
        SapRow("AP_TEL_NUMBER") = m_Telefon
        SapRow("AP_MOB_NUMBER") = m_Mobil
        SapRow("AP_SMTP_ADDR") = m_Mail
        SapRow("AP_FAX_NUMBER") = m_Fax
        SapRow("QUELLE") = "EFA-Neu"

        tblSAP.Rows.Add(SapRow)
        Return tblSAP
    End Function

    Public Sub LeseMailTexte(ByVal InputVorgang As String)

        Dim strTempVorgang As String = InputVorgang

        m_intStatus = 0
        m_strMessage = ""
        mtblMailings = New DataTable

        Try
            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            cn.Open()

            Dim da As New SqlClient.SqlDataAdapter("SELECT * FROM vwGetMailTexte WHERE KundenID=@KundenID " & _
                                                                                "AND Vorgangsnummer Like @Vorgangsnummer " & _
                                                                                "AND Aktiv=1", cn)

            da.SelectCommand.Parameters.AddWithValue("@KundenID", mMyKasse.CustomerID)
            da.SelectCommand.Parameters.AddWithValue("@Vorgangsnummer", strTempVorgang)

            da.Fill(mtblMailings)

            If mtblMailings.Rows.Count > 0 Then
                Dim dRow As DataRow

                For Each dRow In mtblMailings.Rows
                    m_strMailBody = dRow("Text").ToString
                    m_strBetreff = dRow("Betreff").ToString
                    If CBool(dRow("CC")) Then
                        If m_strMailAdressCC.Length = 0 Then
                            m_strMailAdressCC = dRow("EmailAdresse").ToString
                        Else
                            m_strMailAdressCC &= ";" & dRow("EmailAdresse").ToString
                        End If
                    ElseIf m_strMailAdress.Length = 0 Then
                        m_strMailAdress = dRow("EmailAdresse").ToString
                    Else
                        m_strMailAdress &= ";" & dRow("EmailAdresse").ToString
                    End If
                Next
            Else
                m_intStatus = -9999
            End If

            cn.Close()
        Catch ex As Exception
            m_strMessage = "Keine Mailvorlagen für diesen Kunden <br>(" & ex.Message & ")."
            m_intStatus = -9999
        End Try
    End Sub

    Public Sub LeseMailAdressCC(ByVal InputVorgang As String)

        Dim strTempVorgang As String = InputVorgang

        Dim intReturn As Int32

        Try
            Dim cn As New SqlClient.SqlConnection(KBS_BASE.SAPConnectionString)
            cn.Open()

            Dim da As New SqlClient.SqlDataAdapter("SELECT * FROM vwGetMailTexte WHERE KundenID=@KundenID " & _
                                                                                "AND Vorgangsnummer=@Vorgangsnummer " & _
                                                                                "AND CC=0", cn)
            da.SelectCommand.Parameters.AddWithValue("@KundenID", mMyKasse.CustomerID)
            da.SelectCommand.Parameters.AddWithValue("@Vorgangsnummer", strTempVorgang)
            da.Fill(mtblMailings)

            If mtblMailings.Rows.Count > 0 Then
                Dim dRow As DataRow

                For Each dRow In mtblMailings.Rows

                Next

            End If

            cn.Close()
            intReturn = mtblMailings.Rows.Count
        Catch ex As Exception
            m_strMessage = "Keine Filialen für diesen Kunden <br>(" & ex.Message & ")."
            intReturn = 0
        End Try
    End Sub

    Public Sub FillERP()

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            dt.Rows.Add(New Object() {"GT_T005", True})
            dt.Rows.Add(New Object() {"GT_T016", True})
            dt.Rows.Add(New Object() {"GT_TPFK", True})


            SAPExc.ExecuteERP("Z_ALL_DEBI_CHECK_TABLES", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='GT_T005'")(0)
                If Not retRows Is Nothing Then
                    m_Laender = DirectCast(retRows("Data"), DataTable)
                End If
                retRows = dt.Select("Fieldname='GT_T016'")(0)
                If Not retRows Is Nothing Then
                    m_Branchen = DirectCast(retRows("Data"), DataTable)
                End If
                retRows = dt.Select("Fieldname='GT_TPFK'")(0)
                If Not retRows Is Nothing Then
                    m_Funktion = DirectCast(retRows("Data"), DataTable)
                End If

            End If

        Catch ex As Exception

        Finally

        End Try

    End Sub

    Public Sub ChangeERP()

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            'befüllen der Importparameter
            Dim tblSAP As DataTable = Create_FillInputTab()
            dt.Rows.Add(New Object() {"GS_IN", False, tblSAP})
            dt.Rows.Add(New Object() {"E_VKUNNR", True})

            SAPExc.ExecuteERP("Z_ALL_DEBI_VORERFASSUNG_WEB", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='E_VKUNNR'")(0)
                If Not retRows Is Nothing Then
                    m_NeueKUNNR = retRows("Data").ToString()
                End If

            End If

        Catch ex As Exception
            m_strMessage = ex.Message
        Finally

        End Try
    End Sub

    Public Function GetListeAusparkenERP() As DataTable
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            mListeAusparken = New DataTable
            dt.Rows.Add(New Object() {"I_VKBUR", False, Right("0000" & mMyKasse.Lagerort, 4), 4})
            dt.Rows.Add(New Object() {"GT_LISTE", True})

            SAPExc.ExecuteERP("Z_ALL_DEBI_PARK_LIST", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC

                Select Case E_SUBRC
                    Case 101
                        E_MESSAGE = ""
                    Case Else
                        E_MESSAGE = SAPExc.E_MESSAGE
                End Select

            End If
            Dim retRows As DataRow = dt.Select("Fieldname='GT_LISTE'")(0)
            If Not retRows Is Nothing Then
                mListeAusparken = DirectCast(retRows("Data"), DataTable)
            End If

        Catch ex As Exception

        End Try

        Return mListeAusparken

    End Function

    Public Sub ParkenERP()
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            Dim tblNewKunde As DataTable = Create_FillInputTab()

            dt.Rows.Add(New Object() {"GS_IN", False, tblNewKunde})
            dt.Rows.Add(New Object() {"E_VKUNNR", True})

            SAPExc.ExecuteERP("Z_ALL_DEBI_PARK_SAVE", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='E_VKUNNR'")(0)
                If Not retRows Is Nothing Then
                    m_NeueKUNNR = retRows("Data").ToString()
                End If

            End If

        Catch ex As Exception
            m_strMessage = ex.Message
        End Try

    End Sub

    Public Sub AusparkenERP(ByVal VKunNr As String)

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_VKUNNR", False, Right("0000000000" & VKunNr, 10), 10})
            dt.Rows.Add(New Object() {"GS_OUT", True})
            SAPExc.ExecuteERP("Z_ALL_DEBI_PARK_READ", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='GS_OUT'")(0)
                If Not retRows Is Nothing Then
                    Dim tblTemp As DataTable = DirectCast(retRows("Data"), DataTable)
                    If tblTemp.Rows.Count > 0 Then
                        Dim SapRow As DataRow = tblTemp.Rows(0)

                        m_MitarbeiterNr = SapRow("BEDIEN").ToString
                        m_Abruftyp = SapRow("KALKS").ToString
                        m_EinzugEr = SapRow("EZERM").ToString
                        m_Anrede = SapRow("TITLE").ToString
                        m_Branche = SapRow("BRSCH").ToString
                        m_BrancheFreitext = SapRow("BRSCH_FREITXT").ToString
                        Name1 = SapRow("NAME1").ToString
                        Name2 = SapRow("NAME2").ToString
                        m_Strasse = SapRow("STREET").ToString
                        m_Ort = SapRow("CITY1").ToString
                        m_HausNr = SapRow("HOUSE_NUM1").ToString
                        m_PLZ = SapRow("POST_CODE1").ToString
                        m_Land = SapRow("LAND1").ToString
                        m_UIDNummer = SapRow("STCEG").ToString
                        m_ASPVorname = SapRow("AP_NAMEV").ToString
                        m_ASPName = SapRow("AP_NAME1").ToString

                        m_strFunktion = SapRow("AP_PAFKT").ToString
                        m_Telefon = SapRow("AP_TEL_NUMBER").ToString
                        m_Mobil = SapRow("AP_MOB_NUMBER").ToString
                        m_Mail = SapRow("AP_SMTP_ADDR").ToString
                        m_Fax = SapRow("AP_FAX_NUMBER").ToString

                    End If
                End If
            End If

        Catch ex As Exception
            m_strMessage = ex.Message
        End Try

    End Sub

    Public Sub FillDokumenteERP()
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)
        E_MESSAGE = ""
        E_SUBRC = 0

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_ZKBA1", False, "", 5})
            dt.Rows.Add(New Object() {"I_ZKBA2", False, "", 5})
            dt.Rows.Add(New Object() {"I_ZKFZKZ", False, PKennzeichen})
            dt.Rows.Add(New Object() {"I_AUSWAHL", False, "", 1})

            dt.Rows.Add(New Object() {"GT_WEB", True})
            SAPExc.ExecuteERP("Z_M_ZGBS_BEN_ZULASSUNGSUNT", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            Else
                Dim retRows As DataRow = dt.Select("Fieldname='GT_WEB'")(0)
                If Not retRows Is Nothing Then
                    Dokumente = DirectCast(retRows("Data"), DataTable)
                End If
            End If

        Catch ex As Exception
            m_strMessage = ex.Message
        End Try

    End Sub

End Class
