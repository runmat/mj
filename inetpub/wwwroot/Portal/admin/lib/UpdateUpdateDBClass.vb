'Public Class UpdateUpdateDBClass
'    Inherits CKG.Base.Common.DynSapProxy



'#Region "Deklarations"

'    Private mDBProxys As DBProxys
'    Private mUpdatedBapisProxys As DataTable
'    Private mLastUpdater As String
'    Private mLastUpdate As Date
'    Private mTestSap As Boolean

'#End Region

'#Region "Properties"

'    Public ReadOnly Property updatedBapis() As DataTable
'        Get
'            Return mUpdatedBapisProxys
'        End Get
'    End Property


'    Public ReadOnly Property LastUpdate() As Date
'        Get
'            Return mLastUpdate
'        End Get
'    End Property

'    Public ReadOnly Property LastUpdateUser() As String
'        Get
'            Return mLastUpdater
'        End Get
'    End Property


'#End Region

'#Region "Methodes"

'    Public Sub New(ByVal testSap As Boolean)
'        mTestSap = testSap
'    End Sub

'    Public Sub fillUpdatedBapisProxys(ByVal mObjWebUser As CKG.Base.Kernel.Security.User, ByVal appId As String)
'        Dim tmpSchwachsinn As CKG.Base.Business.BankBaseCredit
'        tmpSchwachsinn = New CKG.Base.Business.BankBaseCredit(mObjWebUser, mObjWebUser.App, "", mObjWebUser.SessionID, "")
'        mUpdatedBapisProxys = tmpSchwachsinn.CreateOutPut(writeUpdatedBapisIntoUpdateDB(callUpdatedBapiBapi(mLastUpdate), mObjWebUser.UserName, mTestSap), appId)
'    End Sub


'    Public Sub getLastUpdateRun()
'        Dim tmpDT As DataTable = New UpdateDBProxys(ConfigurationManager.AppSettings("Connectionstring"), mTestSap, True)
'        mLastUpdate = tmpDT.Select("BapiName='WebUpdateRun'")(0)("BapiLastChangeSap")
'        mLastUpdater = tmpDT.Select("BapiName='WebUpdateRun'")(0)("LastDBChangeWebBy")
'    End Sub
'#End Region

'End Class
