Public Class BapiOverViewClass
    Inherits CKG.Base.Common.DynSapProxy


#Region "Deklarations"

    Private mDBProxys As DBProxys
    Private mDBFilter As String = "%"
    Private mTestSap As Boolean
#End Region

#Region "Properties"

    Public WriteOnly Property DBFilter() As String
        Set(ByVal value As String)
            mDBFilter = value.Replace("*", "%")
        End Set
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

    Public ReadOnly Property TestSap() As Boolean
        Get
            Return mTestSap
        End Get
    End Property

#End Region

#Region "Methodes"

    Public Sub New(ByVal testSap As Boolean)
        mTestSap = testSap
        mDBProxys = New DBProxys(ConfigurationManager.AppSettings("Connectionstring"), mTestSap)
    End Sub

    Public Sub fillDBProxys()
        mDBProxys.refill()
    End Sub

    Public Sub deleteBapi(ByVal BapiName As String)
        If Not BapiName.Contains(","c) Then
            Throw New Exception("Es wurde kein Datensatz entfernt zu BAPI: " & BapiName)
        End If

        Dim bName As String = BapiName.Substring(0, BapiName.IndexOf(","c))
        Dim bModul As String = BapiName.Substring(BapiName.IndexOf(","c) + 1)

        If Not removeBapiFromDB(bName, mTestSap, bModul) Then
            Throw New Exception("Es wurde kein Datensatz entfernt zu BAPI: " & BapiName)
        End If

    End Sub

    Public Sub deleteAllBapis()
        MyBase.clearBapiStrukturenInSQLDB()
    End Sub

#End Region
End Class
