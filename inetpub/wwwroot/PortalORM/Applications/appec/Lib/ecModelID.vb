Imports CKG.Base.Common
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class ecModelID
    Inherits Base.Business.DatenimportBase


#Region " Declarations"
    Private m_tblFahrzeuge As DataTable
    Private mModelID As String
    Private mModelName As String
    Private mSippCode As String
    Private mLaufzeit As String
    Private mLZBindung As String

    Private mNeuModelName As String
    Private mNeuSippCode As String
    Private mNeuLaufzeit As String
    Private mNeuLZBindung As String
    Private mGesamt As String

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
    Public Property Gesamt() As String
        Get
            Return mGesamt
        End Get
        Set(ByVal Value As String)
            mGesamt = Value
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
    Public Property LZBindung() As String
        Get
            Return mLZBindung
        End Get
        Set(ByVal Value As String)
            mLZBindung = Value
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
    Public Property NeuLZBindung() As String
        Get
            Return mNeuLZBindung
        End Get
        Set(ByVal Value As String)
            mNeuLZBindung = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub


    Public Overloads Sub Show(ByVal page As Page)
        m_strClassAndMethod = "ecModelID.Show"

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If
        Try
            m_intStatus = 0
            m_strMessage = ""


            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_MODELID", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_MODELID", mModelID)
            'myProxy.callBapi()

            'mModelName = myProxy.getExportParameter("ZZBEZEI").ToString()
            'mSippCode = myProxy.getExportParameter("ZSIPP_CODE").ToString()
            'mLaufzeit = myProxy.getExportParameter("ZLAUFZEIT").ToString()
            'mLZBindung = myProxy.getExportParameter("ZLZBINDUNG").ToString()
            'm_E_SUBRC = myProxy.getExportParameter("E_SUBRC").ToString()
            'm_E_MESSAGE = myProxy.getExportParameter("E_MESSAGE").ToString()

            S.AP.InitExecute("Z_DPM_READ_MODELID", "I_KUNNR,I_MODELID", Right("0000000000" & m_objUser.KUNNR, 10), mModelID)

            mModelName = S.AP.GetExportParameter("ZZBEZEI").ToString()
            mSippCode = S.AP.GetExportParameter("ZSIPP_CODE").ToString()
            mLaufzeit = S.AP.GetExportParameter("ZLAUFZEIT").ToString()
            mLZBindung = S.AP.GetExportParameter("ZLZBINDUNG").ToString()
            m_E_SUBRC = S.AP.GetExportParameter("E_SUBRC").ToString()
            m_E_MESSAGE = S.AP.GetExportParameter("E_MESSAGE").ToString()

            If m_E_SUBRC <> "00" Then
                m_intStatus = -9999
            End If
            m_strMessage = m_E_MESSAGE



            WriteLogEntry(True, "ModelID=" & mModelID, Nothing)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Unbekannter Fehler."
            End Select
            WriteLogEntry(False, "ModelID=" & mModelID & " , " & m_strMessage, Nothing)
        End Try
    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        m_strClassAndMethod = "ecModelID.Change"

        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If
        Try
            m_intStatus = 0
            m_strMessage = ""


            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_CHANGE_MODELID", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_MODELID", mModelID)
            'myProxy.setImportParameter("I_ZSIPP_CODE", mNeuSippCode)
            'myProxy.setImportParameter("I_ZZBEZEI", mNeuModelName)
            'myProxy.setImportParameter("I_ZLAUFZEIT", mNeuLaufzeit)
            'myProxy.setImportParameter("I_GESAMT", mGesamt)
            'myProxy.setImportParameter("I_ZLZBINDUNG", mNeuLZBindung)
            'myProxy.setImportParameter("I_UNAME", IIf(m_objUser.UserName.Length > 12, Left(m_objUser.UserName, 12), m_objUser.UserName))

            'myProxy.callBapi()

            'm_E_SUBRC = myProxy.getExportParameter("E_SUBRC").ToString()
            'm_E_MESSAGE = myProxy.getExportParameter("E_MESSAGE").ToString()

            S.AP.InitExecute("Z_DPM_CHANGE_MODELID", "I_KUNNR,I_MODELID,I_ZSIPP_CODE,I_ZZBEZEI,I_ZLAUFZEIT,I_GESAMT,I_ZLZBINDUNG,I_UNAME", _
                             Right("0000000000" & m_objUser.KUNNR, 10), mModelID, mNeuSippCode, mNeuModelName, mNeuLaufzeit, mGesamt, mNeuLZBindung, _
                             IIf(m_objUser.UserName.Length > 12, Left(m_objUser.UserName, 12), m_objUser.UserName))

            m_E_SUBRC = S.AP.GetExportParameter("E_SUBRC").ToString()
            m_E_MESSAGE = S.AP.GetExportParameter("E_MESSAGE").ToString()

            If m_E_SUBRC <> "00" Then
                m_intStatus = -9999
                m_strMessage = m_E_MESSAGE
            End If


            WriteLogEntry(True, "ModelID=" & mModelID, Nothing)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Unbekannter Fehler."
            End Select
            WriteLogEntry(False, "ModelID=" & mModelID & " , " & m_strMessage, Nothing)
        End Try
    End Sub

#End Region
End Class
