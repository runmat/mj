Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class ecModelID
    Inherits Base.Business.DatenimportBase

#Region " Declarations"

    Private mHerstellerId As String

    Private mTblHerstellerIds As DataTable

    Private mModelID As String
    Private mModelName As String
    Private mSippCode As String
    Private mLaufzeit As String
    Private mLZBindung As Boolean
    Private mFzggruppeLkw As Boolean
    Private mWinterreifen As Boolean
    Private mAhk As Boolean
    Private mNavi As Boolean
    Private mSecuFleet As Boolean
    Private mLeasing As Boolean

    Private mNeuModelID As String
    Private mNeuModelName As String
    Private mNeuSippCode As String
    Private mNeuLaufzeit As String
    Private mNeuLZBindung As Boolean
    Private mNeuFzggruppeLkw As Boolean
    Private mNeuWinterreifen As Boolean
    Private mNeuAhk As Boolean
    Private mNeuNavi As Boolean
    Private mNeuSecuFleet As Boolean
    Private mNeuLeasing As Boolean

    Private mGesamt As Boolean
    Private mVerarbeitungsKz As String

    Private m_E_SUBRC As String
    Private m_E_MESSAGE As String

#End Region

#Region " Properties"

    Public Property ModelID() As String
        Get
            Return mModelID
        End Get
        Set(ByVal Value As String)
            mModelID = Value
        End Set
    End Property

    Public Property HerstellerID() As String
        Get
            Return mHerstellerId
        End Get
        Set(ByVal Value As String)
            mHerstellerId = Value
        End Set
    End Property

    Public Property TblHerstellerIds() As DataTable
        Get
            Return mTblHerstellerIds
        End Get
        Set(ByVal Value As DataTable)
            mTblHerstellerIds = Value
        End Set
    End Property

    Public Property ModelName() As String
        Get
            Return mModelName
        End Get
        Set(ByVal Value As String)
            mModelName = Value
        End Set
    End Property

    Public Property SippCode() As String
        Get
            Return mSippCode
        End Get
        Set(ByVal Value As String)
            mSippCode = Value
        End Set
    End Property

    Public Property Laufzeit() As String
        Get
            Return mLaufzeit
        End Get
        Set(ByVal Value As String)
            mLaufzeit = Value
        End Set
    End Property

    Public Property LZBindung() As Boolean
        Get
            Return mLZBindung
        End Get
        Set(ByVal Value As Boolean)
            mLZBindung = Value
        End Set
    End Property

    Public Property FzggruppeLkw() As Boolean
        Get
            Return mFzggruppeLkw
        End Get
        Set(ByVal Value As Boolean)
            mFzggruppeLkw = Value
        End Set
    End Property

    Public Property Winterreifen() As Boolean
        Get
            Return mWinterreifen
        End Get
        Set(ByVal Value As Boolean)
            mWinterreifen = Value
        End Set
    End Property

    Public Property Ahk() As Boolean
        Get
            Return mAhk
        End Get
        Set(ByVal Value As Boolean)
            mAhk = Value
        End Set
    End Property

    Public Property Navi() As Boolean
        Get
            Return mNavi
        End Get
        Set(ByVal Value As Boolean)
            mNavi = Value
        End Set
    End Property

    Public Property SecuFleet() As Boolean
        Get
            Return mSecuFleet
        End Get
        Set(ByVal Value As Boolean)
            mSecuFleet = Value
        End Set
    End Property

    Public Property Leasing() As Boolean
        Get
            Return mLeasing
        End Get
        Set(ByVal Value As Boolean)
            mLeasing = Value
        End Set
    End Property

    Public Property NeuModelID() As String
        Get
            Return mNeuModelID
        End Get
        Set(ByVal Value As String)
            mNeuModelID = Value
        End Set
    End Property

    Public Property NeuModelName() As String
        Get
            Return mNeuModelName
        End Get
        Set(ByVal Value As String)
            mNeuModelName = Value
        End Set
    End Property

    Public Property NeuSippCode() As String
        Get
            Return mNeuSippCode
        End Get
        Set(ByVal Value As String)
            mNeuSippCode = Value
        End Set
    End Property

    Public Property NeuLaufzeit() As String
        Get
            Return mNeuLaufzeit
        End Get
        Set(ByVal Value As String)
            mNeuLaufzeit = Value
        End Set
    End Property

    Public Property NeuLZBindung() As Boolean
        Get
            Return mNeuLZBindung
        End Get
        Set(ByVal Value As Boolean)
            mNeuLZBindung = Value
        End Set
    End Property

    Public Property NeuFzggruppeLkw() As Boolean
        Get
            Return mNeuFzggruppeLkw
        End Get
        Set(ByVal Value As Boolean)
            mNeuFzggruppeLkw = Value
        End Set
    End Property

    Public Property NeuWinterreifen() As Boolean
        Get
            Return mNeuWinterreifen
        End Get
        Set(ByVal Value As Boolean)
            mNeuWinterreifen = Value
        End Set
    End Property

    Public Property NeuAhk() As Boolean
        Get
            Return mNeuAhk
        End Get
        Set(ByVal Value As Boolean)
            mNeuAhk = Value
        End Set
    End Property

    Public Property NeuNavi() As Boolean
        Get
            Return mNeuNavi
        End Get
        Set(ByVal Value As Boolean)
            mNeuNavi = Value
        End Set
    End Property

    Public Property NeuSecuFleet() As Boolean
        Get
            Return mNeuSecuFleet
        End Get
        Set(ByVal Value As Boolean)
            mNeuSecuFleet = Value
        End Set
    End Property

    Public Property NeuLeasing() As Boolean
        Get
            Return mNeuLeasing
        End Get
        Set(ByVal Value As Boolean)
            mNeuLeasing = Value
        End Set
    End Property

    Public Property Gesamt() As Boolean
        Get
            Return mGesamt
        End Get
        Set(ByVal Value As Boolean)
            mGesamt = Value
        End Set
    End Property

    Public Property VerarbeitungsKz() As String
        Get
            Return mVerarbeitungsKz
        End Get
        Set(ByVal Value As String)
            mVerarbeitungsKz = Value
        End Set
    End Property

    Public ReadOnly Property IsChanged() As Boolean
        Get
            Return (NeuModelName <> ModelName) _
                OrElse (NeuSippCode <> SippCode) _
                OrElse (NeuLaufzeit <> Laufzeit) _
                OrElse (NeuLZBindung <> LZBindung) _
                OrElse (NeuFzggruppeLkw <> FzggruppeLkw) _
                OrElse (NeuWinterreifen <> Winterreifen) _
                OrElse (NeuAhk <> Ahk) _
                OrElse (NeuNavi <> Navi) _
                OrElse (NeuSecuFleet <> SecuFleet) _
                OrElse (NeuLeasing <> Leasing)
        End Get
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub Show(ByVal page As Page)
        m_strClassAndMethod = "ecModelID.Show"

        Try
            m_intStatus = 0
            m_strMessage = ""

            S.AP.InitExecute("Z_DPM_READ_MODELID", "I_KUNNR, I_MODELID", Right("0000000000" & m_objUser.KUNNR, 10), mModelID)

            mModelName = S.AP.GetExportParameter("ZZBEZEI")
            mSippCode = S.AP.GetExportParameter("ZSIPP_CODE")
            mLaufzeit = S.AP.GetExportParameter("ZLAUFZEIT")
            mLZBindung = (S.AP.GetExportParameter("ZLZBINDUNG") = "X")
            mFzggruppeLkw = (S.AP.GetExportParameter("LKW") = "X")
            mWinterreifen = (S.AP.GetExportParameter("WINTERREIFEN") = "X")
            mAhk = (S.AP.GetExportParameter("AHK") = "X")
            mNavi = (S.AP.GetExportParameter("NAVI_VORH") = "X")
            mSecuFleet = (S.AP.GetExportParameter("SECU_FLEET") = "X")
            mLeasing = (S.AP.GetExportParameter("LEASING") = "X")
            m_E_SUBRC = S.AP.GetExportParameter("E_SUBRC")
            m_E_MESSAGE = S.AP.GetExportParameter("E_MESSAGE")

            If m_E_SUBRC <> "00" Then
                m_intStatus = -9999
            End If
            m_strMessage = m_E_MESSAGE

            If m_strMessage.ToUpper() = "NO DATA" Then
                m_strMessage = "Zur angegebenen Model-ID konnten keine Daten ermittelt werden!"
            End If

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler: " & ex.Message
            End Select
        End Try
    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        m_strClassAndMethod = "ecModelID.Change"

        Try
            m_intStatus = 0
            m_strMessage = ""

            S.AP.Init("Z_DPM_CHANGE_MODELID", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            S.AP.SetImportParameter("I_VERKZ", mVerarbeitungsKz)
            S.AP.SetImportParameter("I_MODELID", mNeuModelID)
            S.AP.SetImportParameter("I_ZZBEZEI", mNeuModelName)
            S.AP.SetImportParameter("I_ZSIPP_CODE", mNeuSippCode)
            S.AP.SetImportParameter("I_ZLAUFZEIT", mNeuLaufzeit)
            S.AP.SetImportParameter("I_ZLZBINDUNG", IIf(mNeuLZBindung, "X", ""))
            S.AP.SetImportParameter("I_GESAMT", IIf(mGesamt, "X", ""))
            S.AP.SetImportParameter("I_UNAME", IIf(m_objUser.UserName.Length > 12, Left(m_objUser.UserName, 12), m_objUser.UserName))
            S.AP.SetImportParameter("I_HERST", mHerstellerId)
            S.AP.SetImportParameter("I_LKW", IIf(mNeuFzggruppeLkw, "X", ""))
            S.AP.SetImportParameter("I_WINTERREIFEN", IIf(mNeuWinterreifen, "X", ""))
            S.AP.SetImportParameter("I_AHK", IIf(mNeuAhk, "X", ""))
            S.AP.SetImportParameter("I_NAVI_VORH", IIf(mNeuNavi, "X", ""))
            S.AP.SetImportParameter("I_SECU_FLEET", IIf(mNeuSecuFleet, "X", ""))
            S.AP.SetImportParameter("I_LEASING", IIf(mNeuLeasing, "X", ""))

            S.AP.Execute()

            m_E_SUBRC = S.AP.GetExportParameter("E_SUBRC").ToString()
            m_E_MESSAGE = S.AP.GetExportParameter("E_MESSAGE").ToString()

            If m_E_SUBRC <> "00" Then
                m_intStatus = -9999
                m_strMessage = m_E_MESSAGE
            End If

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler: " & ex.Message
            End Select
        End Try
    End Sub

    Public Sub LoadHerstellerIds()
        m_strClassAndMethod = "ecModelID.LoadHerstellerIds"

        Try
            m_intStatus = 0
            m_strMessage = ""

            S.AP.InitExecute("Z_M_HERSTELLERGROUP", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            mTblHerstellerIds = S.AP.GetExportTable("T_HERST")

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler: " & ex.Message
            End Select
        End Try
    End Sub

#End Region

End Class
