Imports SapORM.Contracts
Imports KBSBase

Public Class ComCommon
    Inherits ErrorHandlingClass

#Region "Declarations"

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

    Public ReadOnly Property ListeAusparken() As DataTable
        Get
            Return mListeAusparken
        End Get
    End Property

    Public Property PKennzeichen As String
    Public Property Dokumente As DataTable

#End Region

    Private Sub FillInputTab(ByRef tblSAP As DataTable)
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
    End Sub

    Public Sub LeseMailTexte(ByVal InputVorgang As String)
        ClearErrorState()

        mtblMailings = New DataTable

        Try
            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            cn.Open()

            Dim da As New SqlClient.SqlDataAdapter("SELECT * FROM vwGetMailTexte WHERE KundenID=@KundenID " & _
                                                                                "AND Vorgangsnummer Like @Vorgangsnummer " & _
                                                                                "AND Aktiv=1", cn)

            da.SelectCommand.Parameters.AddWithValue("@KundenID", mMyKasse.CustomerID)
            da.SelectCommand.Parameters.AddWithValue("@Vorgangsnummer", InputVorgang)

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
                RaiseError("9999", "Keine Mailvorlagen für diesen Kunden")
            End If

            cn.Close()

        Catch ex As Exception
            RaiseError("9999", "Keine Mailvorlagen für diesen Kunden <br>(" & ex.Message & ").")
        End Try
    End Sub

    Public Sub LeseMailAdressCC(ByVal InputVorgang As String)
        ClearErrorState()

        Try
            Dim cn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
            cn.Open()

            Dim da As New SqlClient.SqlDataAdapter("SELECT * FROM vwGetMailTexte WHERE KundenID=@KundenID " & _
                                                                                "AND Vorgangsnummer=@Vorgangsnummer " & _
                                                                                "AND CC=0", cn)
            da.SelectCommand.Parameters.AddWithValue("@KundenID", mMyKasse.CustomerID)
            da.SelectCommand.Parameters.AddWithValue("@Vorgangsnummer", InputVorgang)
            da.Fill(mtblMailings)

            cn.Close()

        Catch ex As Exception
            RaiseError("9999", "Keine Filialen für diesen Kunden <br>(" & ex.Message & ").")
        End Try
    End Sub

    Public Sub FillERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_ALL_DEBI_CHECK_TABLES")

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                m_Laender = S.AP.GetExportTable("GT_T005")
                m_Branchen = S.AP.GetExportTable("GT_T016")
                m_Funktion = S.AP.GetExportTable("GT_TPFK")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub ChangeERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_ALL_DEBI_VORERFASSUNG_WEB")

            Dim tblSAP As DataTable = S.AP.GetImportTable("GS_IN")
            FillInputTab(tblSAP)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                m_NeueKUNNR = S.AP.GetExportParameter("E_VKUNNR")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Function GetListeAusparkenERP() As DataTable
        ClearErrorState()

        mListeAusparken = New DataTable

        Try
            S.AP.Init("Z_ALL_DEBI_PARK_LIST", "I_VKBUR", mMyKasse.Lagerort.PadLeft(4, "0"c))

            S.AP.Execute()

            If S.AP.ResultCode = 0 OrElse S.AP.ResultCode = 101 Then
                mListeAusparken = S.AP.GetExportTable("GT_LISTE")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return mListeAusparken
    End Function

    Public Sub ParkenERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_ALL_DEBI_PARK_SAVE")

            Dim tblNewKunde As DataTable = S.AP.GetImportTable("GS_IN")
            FillInputTab(tblNewKunde)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                m_NeueKUNNR = S.AP.GetExportParameter("E_VKUNNR")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub AusparkenERP(ByVal VKunNr As String)
        ClearErrorState()

        Try
            S.AP.Init("Z_ALL_DEBI_PARK_READ", "I_VKUNNR", VKunNr.ToSapKunnr())

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                Dim tblTemp As DataTable = S.AP.GetExportTable("GS_OUT")
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

            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub FillDokumenteERP()
        ClearErrorState()

        Try
            S.AP.Init("Z_M_ZGBS_BEN_ZULASSUNGSUNT", "I_ZKFZKZ", PKennzeichen)

            S.AP.Execute()

            If S.AP.ResultCode = 0 Then
                Dokumente = S.AP.GetExportTable("GT_WEB")
            Else
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

End Class
