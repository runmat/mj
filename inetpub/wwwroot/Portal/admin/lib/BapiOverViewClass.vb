Public Class BapiOverViewClass
    Inherits CKG.Base.Common.DynSapProxy


#Region "Deklarations"

    Private mDBProxys As DBProxys
    Private mRamProxys As DataTable
    Private mUpdateDBProxys As UpdateDBProxys

    Private mRamFilter As String = "%"
    Private mDBFilter As String = "%"
    Private mUpdateDBFilter As String = "%"
    Private mTestSap As Boolean
#End Region

#Region "Properties"

    Public WriteOnly Property RamFilter() As String
        Set(ByVal value As String)
            mRamFilter = value.Replace("*", "%")
        End Set
    End Property

    Public WriteOnly Property DBFilter() As String
        Set(ByVal value As String)
            mDBFilter = value.Replace("*", "%")
        End Set
    End Property

    Public WriteOnly Property UpdateDBFilter() As String
        Set(ByVal value As String)
            mUpdateDBFilter = value.Replace("*", "%")
        End Set
    End Property

    Public ReadOnly Property RamProxys() As DataTable
        Get
            If Not mRamProxys Is Nothing Then
                mRamProxys.RejectChanges()
                For Each tmpRow As DataRow In mRamProxys.Select("BapiName LIKE '" & mRamFilter & "'")
                    tmpRow.SetModified()
                Next
            End If
            Return mRamProxys
        End Get
    End Property

    Public ReadOnly Property Speicherbedarf() As Double
        Get
            Return getUsedMemory()
        End Get
    End Property

    Public ReadOnly Property DBProxys() As DataTable
        Get
            If Not mDBProxys Is Nothing Then
                mDBProxys.RejectChanges()
                For Each tmpRow As DataRow In mDBProxys.Select("BapiName LIKE '" & mDBFilter & "'")
                    tmpRow.SetModified()
                Next
            End If
            Return mDBProxys
        End Get
    End Property

    Public ReadOnly Property UpdateDBProxys() As DataTable
        Get
            If Not mUpdateDBProxys Is Nothing Then
                mUpdateDBProxys.RejectChanges()
                For Each tmpRow As DataRow In mUpdateDBProxys.Select("BapiName LIKE '" & mUpdateDBFilter & "'")
                    tmpRow.SetModified()
                Next
            End If
            Return mUpdateDBProxys
        End Get
    End Property

    Public ReadOnly Property TestSap() As Boolean
        Get
            Return mTestSap
        End Get
    End Property

#End Region

#Region "Methodes"


    Public Sub New(ByVal testSap As Boolean)
        mTestSap = testSap
        mRamProxys = GetProxys()
        mDBProxys = New DBProxys(ConfigurationManager.AppSettings("Connectionstring"), mTestSap)
        mUpdateDBProxys = New UpdateDBProxys(ConfigurationManager.AppSettings("Connectionstring"), mTestSap)
    End Sub

    Public Sub fillRamProxys()
        mRamProxys = GetProxys()
    End Sub

    Public Sub fillDBProxys()
        mDBProxys.refill()
    End Sub

    Public Sub fillUpdateDBProxys()
        mUpdateDBProxys.refill()
    End Sub


    Public Sub deleteBapiFrom(ByVal BapiName As String, ByVal from As String)
        If Not removeBapiFrom(BapiName, mTestSap, from) Then
            Throw New Exception("Es wurde kein Datensatz entfernt zu BAPI: " & BapiName)
        End If

    End Sub

    Public Overloads Sub insertBapiIntoUpdateDB(ByVal BapiName As String, ByVal webUserName As String)
        MyBase.insertBapiIntoUpdateDB(BapiName, webUserName, mTestSap)
    End Sub

#End Region
End Class
