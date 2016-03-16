Imports CKG.Base.Kernel.Security
Imports System.Web.UI
Imports CKG.Base.Common

Namespace Business

    Public Class KVP
        Inherits BankBase

#Region "Declarations"

        Private mAktuelleKVPId As String
        Private mKostenstelle As String
        Private mBenutzer As String
        Private mBenutzername As String
        Private Const mAbteilung As String = "ZUL"
        Private mFunktion As String
        Private mStandort As String
        Private mVorgesetzter As String
        Private mKurzbeschreibung As String
        Private mSituationText As String
        Private mVeraenderungText As String
        Private mVorteilText As String
        Private mStatus As Integer = 0

        Private mBewertungsfrist As String
        Private mBewertungPositiv As Boolean
        Private mBewertungNegativ As Boolean

        Private mZuBewertendeKVPs As Integer = 0
        Private mGeparkteKVPId As String

        Private mVorschlagsliste As DataTable

#End Region

#Region "Properties"

        Public ReadOnly Property AktuelleKVPId() As String
            Get
                Return mAktuelleKVPId
            End Get
        End Property

        Public ReadOnly Property Kostenstelle() As String
            Get
                Return mKostenstelle
            End Get
        End Property

        Public ReadOnly Property Benutzer() As String
            Get
                Return mBenutzer
            End Get
        End Property

        Public ReadOnly Property Benutzername() As String
            Get
                Return mBenutzername
            End Get
        End Property

        Public ReadOnly Property Abteilung() As String
            Get
                Return mAbteilung
            End Get
        End Property

        Public Property Funktion() As String
            Get
                Return mFunktion
            End Get
            Set(value As String)
                mFunktion = value
            End Set
        End Property

        Public ReadOnly Property Standort() As String
            Get
                Return mStandort
            End Get
        End Property

        Public ReadOnly Property Vorgesetzter() As String
            Get
                Return mVorgesetzter
            End Get
        End Property

        Public Property Kurzbeschreibung() As String
            Get
                Return mKurzbeschreibung
            End Get
            Set(value As String)
                mKurzbeschreibung = value
            End Set
        End Property

        Public Property SituationText() As String
            Get
                Return mSituationText
            End Get
            Set(value As String)
                mSituationText = value
            End Set
        End Property

        Public Property VeraenderungText() As String
            Get
                Return mVeraenderungText
            End Get
            Set(value As String)
                mVeraenderungText = value
            End Set
        End Property

        Public Property VorteilText() As String
            Get
                Return mVorteilText
            End Get
            Set(value As String)
                mVorteilText = value
            End Set
        End Property

        Public ReadOnly Property Status() As Integer
            Get
                Return mStatus
            End Get
        End Property

        Public ReadOnly Property Bewertungsfrist() As String
            Get
                Return mBewertungsfrist
            End Get
        End Property

        Public Property BewertungPositiv() As Boolean
            Get
                Return mBewertungPositiv
            End Get
            Set(value As Boolean)
                mBewertungPositiv = value
            End Set
        End Property

        Public Property BewertungNegativ() As Boolean
            Get
                Return mBewertungNegativ
            End Get
            Set(value As Boolean)
                mBewertungNegativ = value
            End Set
        End Property

        Public ReadOnly Property ZuBewertendeKVPs() As Integer
            Get
                Return mZuBewertendeKVPs
            End Get
        End Property

        Public ReadOnly Property GeparkteKVPId() As String
            Get
                Return mGeparkteKVPId
            End Get
        End Property

        Public ReadOnly Property Vorschlagsliste() As DataTable
            Get
                Return mVorschlagsliste
            End Get
        End Property

        Public ReadOnly Property HasError() As Boolean
            Get
                Return (Not String.IsNullOrEmpty(m_strMessage))
            End Get
        End Property

#End Region

        Public Sub New(ByRef objUser As User, ByRef objApp As App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
            MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
        End Sub

        Public Sub KVPLogin(ByVal kst As String, ByVal user As String, ByVal username As String, ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
            m_strClassAndMethod = "KVP.KVPLogin"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            m_intStatus = 0
            m_strMessage = String.Empty

            If Not m_blnGestartet Then
                m_blnGestartet = True

                mKostenstelle = kst
                mBenutzer = user
                mBenutzername = username

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_KVP_ANMELDUNG", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_KOSTL", mKostenstelle)
                    myProxy.setImportParameter("I_UNAME", mBenutzer)
                    myProxy.setImportParameter("I_ABTEILUNG", mAbteilung)

                    myProxy.callBapi()

                    mStandort = myProxy.getExportParameter("E_STANDORT")
                    mVorgesetzter = myProxy.getExportParameter("E_VORGESETZTER")
                    mZuBewertendeKVPs = Int32.Parse(myProxy.getExportParameter("E_ANZ_KVP_BW"))
                    mGeparkteKVPId = myProxy.getExportParameter("E_KVPID")

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE")

                Catch ex As Exception
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                    End Select
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub LoadKVP(ByVal kvpId As String, ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page, Optional ByVal preserveBewertungsfrist As Boolean = False)
            m_strClassAndMethod = "KVP.LoadKVP"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            m_intStatus = 0
            m_strMessage = String.Empty

            If Not m_blnGestartet Then
                m_blnGestartet = True

                ClearKVP(preserveBewertungsfrist)
                mAktuelleKVPId = kvpId

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_KVP_READ", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_KVPID", mAktuelleKVPId)

                    myProxy.callBapi()

                    mSituationText = myProxy.getExportParameter("E_LTEXT_WIE")
                    mVeraenderungText = myProxy.getExportParameter("E_LTEXT_WAS")
                    mVorteilText = myProxy.getExportParameter("E_LTEXT_WEM")

                    Dim tblTemp As DataTable = myProxy.getExportTable("ES_VORGANG")

                    If tblTemp IsNot Nothing AndAlso tblTemp.Rows.Count > 0 Then
                        mKurzbeschreibung = tblTemp.Rows(0)("KTEXT").ToString()
                        mStatus = Int32.Parse(tblTemp.Rows(0)("STATUS").ToString())
                    End If

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE")

                Catch ex As Exception
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                    End Select
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub SaveKVP(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page, Optional ByVal nurParken As Boolean = False)
            m_strClassAndMethod = "KVP.SaveKVP"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            m_intStatus = 0
            m_strMessage = String.Empty

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_KVP_SAVE", m_objApp, m_objUser, page)

                    Dim tblTemp As DataTable = myProxy.getImportTable("IS_VORGANG")
                    Dim newRow As DataRow = tblTemp.NewRow()
                    newRow("KVPID") = mAktuelleKVPId
                    newRow("KOSTL") = mKostenstelle
                    newRow("KTEXT") = mKurzbeschreibung
                    newRow("UNAME") = mBenutzer
                    newRow("NAME") = mBenutzername
                    newRow("ABTEILUNG") = mAbteilung
                    newRow("STANDORT") = mStandort
                    newRow("FUNKTION") = mFunktion
                    newRow("VORGESETZTER") = mVorgesetzter
                    newRow("STATUS") = IIf(nurParken, 1, 2)
                    tblTemp.Rows.Add(newRow)

                    myProxy.setImportParameter("I_LTEXT_WIE", mSituationText)
                    myProxy.setImportParameter("I_LTEXT_WAS", mVeraenderungText)
                    myProxy.setImportParameter("I_LTEXT_WEM", mVorteilText)

                    myProxy.callBapi()

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE")

                    If Not HasError Then
                        mAktuelleKVPId = myProxy.getExportParameter("E_KVPID")
                        If Not nurParken Then
                            ClearKVP()
                        End If
                    End If
                Catch ex As Exception
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                    End Select
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub DeleteKVP(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
            m_strClassAndMethod = "KVP.DeleteKVP"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            m_intStatus = 0
            m_strMessage = String.Empty

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_KVP_DELETE", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_KVPID", mAktuelleKVPId)

                    myProxy.callBapi()

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE")

                    If Not HasError Then
                        ClearKVP()
                    End If
                Catch Ex As Exception
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(Ex.Message)
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(Ex.Message) & ")"
                    End Select
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Private Sub ClearKVP(Optional ByVal preserveBewertungsfrist As Boolean = False)
            mAktuelleKVPId = ""
            mKurzbeschreibung = ""
            mSituationText = ""
            mVeraenderungText = ""
            mVorteilText = ""
            mStatus = 0
            mBewertungPositiv = False
            mBewertungNegativ = False
            If Not preserveBewertungsfrist Then
                mBewertungsfrist = ""
            End If
        End Sub

        Public Sub LoadBewertungen(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
            m_strClassAndMethod = "KVP.LoadBewertungen"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            m_intStatus = 0
            m_strMessage = String.Empty

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_KVP_GET_BEWERT", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_UNAME", mBenutzer)
                    myProxy.setImportParameter("I_ABTEILUNG", mAbteilung)

                    myProxy.callBapi()

                    mVorschlagsliste = myProxy.getExportTable("ET_BW_VORGANG")

                    If mVorschlagsliste IsNot Nothing Then
                        mVorschlagsliste.Columns.Add("RESTTAGE", GetType(Integer))
                        For Each dRow As DataRow In mVorschlagsliste.Rows
                            dRow("RESTTAGE") = CInt((CDate(dRow("BW_FRIST")) - DateTime.Now).TotalDays)
                        Next
                    End If

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE")
                Catch ex As Exception
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                    End Select
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub SaveBewertung(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
            m_strClassAndMethod = "KVP.SaveBewertung"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            m_intStatus = 0
            m_strMessage = String.Empty

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Dim bewertung As String = ""
                If mBewertungPositiv Then
                    bewertung = "P"
                Else
                    bewertung = "N"
                End If

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_KVP_SAVE_BEWERT", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_KVPID", mAktuelleKVPId)
                    myProxy.setImportParameter("I_UNAME", mBenutzer)
                    myProxy.setImportParameter("I_BEWERT", bewertung)

                    myProxy.callBapi()

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE")

                    If Not HasError Then
                        ClearKVP()
                    End If
                Catch ex As Exception
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                    End Select
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub SelectKVPForBewertung(ByVal kvpId As String)
            ClearKVP()
            mAktuelleKVPId = kvpId
            Dim rows As DataRow() = mVorschlagsliste.Select("KVPID='" & kvpId & "'")
            If rows.Length > 0 Then
                mBewertungsfrist = CDate(rows(0)("BW_FRIST")).ToString("dd.MM.yyyy")
            End If
        End Sub

        Public Overrides Sub Show()
        End Sub

        Public Overrides Sub Change()
        End Sub

    End Class

End Namespace

