Option Strict On
Option Explicit On

Namespace SilverDAT
    Friend NotInheritable Class DatConnector
        Implements IDisposable

        Private NotInheritable Class AsyncResult(Of T)
            Implements IAsyncResult

            Private ReadOnly _result As T
            Private ReadOnly _asyncState As Object

            Public Sub New(result As T, asyncState As Object)
                Me._result = result
                Me._asyncState = asyncState
            End Sub

            Public ReadOnly Property Result As T
                Get
                    Return Me._result
                End Get
            End Property

            Public ReadOnly Property AsyncState As Object Implements System.IAsyncResult.AsyncState
                Get
                    Return Me._asyncState
                End Get
            End Property

            Public ReadOnly Property AsyncWaitHandle As System.Threading.WaitHandle Implements System.IAsyncResult.AsyncWaitHandle
                Get
                    Return Nothing
                End Get
            End Property

            Public ReadOnly Property CompletedSynchronously As Boolean Implements System.IAsyncResult.CompletedSynchronously
                Get
                    Return True
                End Get
            End Property

            Public ReadOnly Property IsCompleted As Boolean Implements System.IAsyncResult.IsCompleted
                Get
                    Return True
                End Get
            End Property
        End Class

        Private NotInheritable Class AsyncResultWrapper(Of T)
            Implements IAsyncResult

            Private ReadOnly _asyncResult As IAsyncResult
            Private ReadOnly _keyComponents As T()

            Public Sub New(asnycResult As IAsyncResult, ParamArray keyComponents As T())
                Me._asyncResult = asnycResult
                Me._keyComponents = keyComponents
            End Sub

            Public ReadOnly Property KeyComponents As T()
                Get
                    Return Me._keyComponents
                End Get
            End Property

            Public ReadOnly Property OriginalResult As IAsyncResult
                Get
                    Return Me._asyncResult
                End Get
            End Property

            Public ReadOnly Property AsyncState As Object Implements System.IAsyncResult.AsyncState
                Get
                    Return Me._asyncResult.AsyncState
                End Get
            End Property

            Public ReadOnly Property AsyncWaitHandle As System.Threading.WaitHandle Implements System.IAsyncResult.AsyncWaitHandle
                Get
                    Return Me._asyncResult.AsyncWaitHandle
                End Get
            End Property

            Public ReadOnly Property CompletedSynchronously As Boolean Implements System.IAsyncResult.CompletedSynchronously
                Get
                    Return Me._asyncResult.CompletedSynchronously
                End Get
            End Property

            Public ReadOnly Property IsCompleted As Boolean Implements System.IAsyncResult.IsCompleted
                Get
                    Return Me._asyncResult.IsCompleted
                End Get
            End Property
        End Class

        Private Shared Function ComposeKey(Of T)(keyRoot As String, ParamArray keyParts As T()) As String
            Dim sb As New StringBuilder(keyRoot, keyRoot.Length + keyParts.Length * 2)

            For Each part In keyParts
                sb.Append(part)
            Next

            Return sb.ToString()
        End Function

        Private ReadOnly client As SDOSelectVehicleExtPortTypeClient
        Private Shared ReadOnly MaxConstructionDate As Integer = (DateTime.Now.Year - 1969) * 120

        Public Sub New()
            Me.client = New SDOSelectVehicleExtPortTypeClient()
        End Sub

        Private Const VersionCacheKey As String = "DatVersion"

        Public Function BeginGetDatVersion(credentials As DatCredentials, cb As AsyncCallback, state As Object) As IAsyncResult
            Dim result As IAsyncResult
            Dim version As String = DirectCast(HttpRuntime.Cache(DatConnector.VersionCacheKey), String)

            If String.IsNullOrEmpty(version) Then
                Dim request As New getVersionRequest()
                request.datCustomerNumber = credentials.CustomerNumber
                request.userName = credentials.UserName
                request.password = credentials.Password

                result = Me.client.BegingetVersion(request, cb, state)
            Else
                result = New AsyncResult(Of String)(version, state)

                If cb IsNot Nothing Then
                    cb(result)
                End If
            End If

            Return result
        End Function

        Public Function EndGetDatVersion(asyncResult As IAsyncResult) As String
            Dim version As String
            Dim result As AsyncResult(Of String) = TryCast(asyncResult, AsyncResult(Of String))

            If result Is Nothing Then
                Dim response As getVersionResponse = Me.client.EndgetVersion(asyncResult)
                version = response.getVersionReturn
                HttpRuntime.Cache.Insert(DatConnector.VersionCacheKey, version)
            Else
                version = result.Result
            End If

            Return version
        End Function

        Private Const VehicleTypesCacheKeyRoot As String = "DatVehicleTypesFor"

        Public Function BeginGetVehicleTypes(credentials As DatCredentials, cb As AsyncCallback, state As Object) As IAsyncResult
            Return Me.BeginGetVehicleTypes(credentials, 0, cb, state)
        End Function

        Public Function BeginGetVehicleTypes(credentials As DatCredentials, restriction As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Dim result As IAsyncResult
            Dim vehicleTypes As IDictionary(Of Integer, String) = DirectCast(HttpRuntime.Cache(DatConnector.ComposeKey(DatConnector.VehicleTypesCacheKeyRoot, restriction)), IDictionary(Of Integer, String))

            If vehicleTypes Is Nothing Then
                Dim request As New getVehicleTypesRequest()
                request.datCustomerNumber = credentials.CustomerNumber
                request.userName = credentials.UserName
                request.password = credentials.Password
                request.restriction = restriction

                result = Me.client.BegingetVehicleTypes(request, cb, state)
                result = New AsyncResultWrapper(Of Integer)(result, restriction)
            Else
                result = New AsyncResult(Of IDictionary(Of Integer, String))(vehicleTypes, state)

                If cb IsNot Nothing Then
                    cb(result)
                End If
            End If

            Return result
        End Function

        Public Function EndGetVehicleTypes(asyncResult As IAsyncResult) As IDictionary(Of Integer, String)
            Dim vehicleTypes As IDictionary(Of Integer, String)
            Dim result As AsyncResult(Of IDictionary(Of Integer, String)) = TryCast(asyncResult, AsyncResult(Of IDictionary(Of Integer, String)))

            If result Is Nothing Then
                Dim wrapper As AsyncResultWrapper(Of Integer) = DirectCast(asyncResult, AsyncResultWrapper(Of Integer))

                Try
                    Dim response As getVehicleTypesResponse = Me.client.EndgetVehicleTypes(wrapper.OriginalResult)
                    ' Omnibus und Caravan ausfiltern mit Where Klausel
                    vehicleTypes = response.getVehicleTypesReturn.Where(Function(i) i.key < 5).ToDictionary(Function(i) i.key, Function(i) i.value)
                Catch ex As ServiceModel.FaultException
                    Dim de As DatException = DatException.FromFault(ex)

                    If de IsNot Nothing Then
                        Throw de
                    Else
                        Throw
                    End If
                End Try

                HttpRuntime.Cache.Insert(DatConnector.ComposeKey(DatConnector.VehicleTypesCacheKeyRoot, wrapper.KeyComponents), vehicleTypes)
            Else
                vehicleTypes = result.Result
            End If

            Return vehicleTypes
        End Function

        Private Const ManufacturersCacheKeyRoot As String = "DatManufacturesFor"

        Public Function BeginGetManufacturers(credentials As DatCredentials, vehicleType As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Return Me.BeginGetManufacturers(credentials, vehicleType, 0, cb, state)
        End Function

        Public Function BeginGetManufacturers(credentials As DatCredentials, vehicleType As Integer, restriction As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Dim result As IAsyncResult
            Dim manufacturers As IDictionary(Of Integer, String) = DirectCast(HttpRuntime.Cache(DatConnector.ComposeKey(DatConnector.ManufacturersCacheKeyRoot, vehicleType, restriction)), IDictionary(Of Integer, String))

            If manufacturers Is Nothing Then
                Dim request As New getManufacturersRequest()
                request.datCustomerNumber = credentials.CustomerNumber
                request.userName = credentials.UserName
                request.password = credentials.Password
                request.vehicleType = vehicleType
                request.restriction = restriction

                result = Me.client.BegingetManufacturers(request, cb, state)
                result = New AsyncResultWrapper(Of Integer)(result, vehicleType, restriction)
            Else
                result = New AsyncResult(Of IDictionary(Of Integer, String))(manufacturers, state)

                If cb IsNot Nothing Then
                    cb(result)
                End If
            End If

            Return result
        End Function

        Public Function EndGetManufacturers(asyncResult As IAsyncResult) As IDictionary(Of Integer, String)
            Dim manufacturers As IDictionary(Of Integer, String)
            Dim result As AsyncResult(Of IDictionary(Of Integer, String)) = TryCast(asyncResult, AsyncResult(Of IDictionary(Of Integer, String)))

            If result Is Nothing Then
                Dim wrapper As AsyncResultWrapper(Of Integer) = DirectCast(asyncResult, AsyncResultWrapper(Of Integer))

                Try
                    Dim response As getManufacturersResponse = Me.client.EndgetManufacturers(wrapper.OriginalResult)
                    manufacturers = response.getManufacturersReturn.ToDictionary(Function(i) i.key, Function(i) i.value)
                Catch ex As ServiceModel.FaultException
                    Dim de As DatException = DatException.FromFault(ex)

                    If de IsNot Nothing Then
                        Throw de
                    Else
                        Throw
                    End If
                End Try

                HttpRuntime.Cache.Insert(DatConnector.ComposeKey(DatConnector.ManufacturersCacheKeyRoot, wrapper.KeyComponents), manufacturers)
            Else
                manufacturers = result.Result
            End If

            Return manufacturers
        End Function

        Private Const BaseModelsCacheKeyRoot As String = "DatBaseModelsFor"

        Public Function BeginGetBaseModels(credentials As DatCredentials, vehicleType As Integer, manufacturer As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Return Me.BeginGetBaseModels(credentials, vehicleType, manufacturer, 0, cb, state)
        End Function

        Public Function BeginGetBaseModels(credentials As DatCredentials, vehicleType As Integer, manufacturer As Integer, restriction As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Return Me.BeginGetBaseModels(credentials, vehicleType, manufacturer, restriction, 10, cb, state)
        End Function

        Public Function BeginGetBaseModels(credentials As DatCredentials, vehicleType As Integer, manufacturer As Integer, restriction As Integer, constructionDateFrom As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Return Me.BeginGetBaseModels(credentials, vehicleType, manufacturer, restriction, constructionDateFrom, DatConnector.MaxConstructionDate, cb, state)
        End Function

        Public Function BeginGetBaseModels(credentials As DatCredentials, vehicleType As Integer, manufacturer As Integer, restriction As Integer, constructionDateFrom As Integer, constructionDateTo As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Dim result As IAsyncResult
            Dim baseModels As IDictionary(Of Integer, String) = DirectCast(HttpRuntime.Cache(DatConnector.ComposeKey(DatConnector.BaseModelsCacheKeyRoot, vehicleType, manufacturer, restriction, constructionDateFrom, constructionDateTo)), IDictionary(Of Integer, String))

            If baseModels Is Nothing Then
                Dim request As New getBaseModelsRequest()
                request.datCustomerNumber = credentials.CustomerNumber
                request.userName = credentials.UserName
                request.password = credentials.Password
                request.vehicleType = vehicleType
                request.manufacturer = manufacturer
                request.restriction = restriction
                request.constructionTimeFrom = constructionDateFrom
                request.constructionTimeTo = constructionDateTo

                result = Me.client.BegingetBaseModels(request, cb, state)
                result = New AsyncResultWrapper(Of Integer)(result, vehicleType, manufacturer, restriction, constructionDateFrom, constructionDateTo)
            Else
                result = New AsyncResult(Of IDictionary(Of Integer, String))(baseModels, state)

                If cb IsNot Nothing Then
                    cb(result)
                End If
            End If

            Return result
        End Function

        Public Function EndGetBaseModels(asyncResult As IAsyncResult) As IDictionary(Of Integer, String)
            Dim baseModels As IDictionary(Of Integer, String)
            Dim result As AsyncResult(Of IDictionary(Of Integer, String)) = TryCast(asyncResult, AsyncResult(Of IDictionary(Of Integer, String)))

            If result Is Nothing Then
                Dim wrapper As AsyncResultWrapper(Of Integer) = DirectCast(asyncResult, AsyncResultWrapper(Of Integer))

                Try
                    Dim response As getBaseModelsResponse = Me.client.EndgetBaseModels(wrapper.OriginalResult)
                    baseModels = response.getBaseModelsReturn.OrderBy(Function(i) i.value).ToDictionary(Function(i) i.key, Function(i) i.value)
                Catch ex As ServiceModel.FaultException
                    Dim de As DatException = DatException.FromFault(ex)

                    If de IsNot Nothing Then
                        Throw de
                    Else
                        Throw
                    End If
                End Try

                HttpRuntime.Cache.Insert(DatConnector.ComposeKey(DatConnector.BaseModelsCacheKeyRoot, wrapper.KeyComponents), baseModels)
            Else
                baseModels = result.Result
            End If

            Return baseModels
        End Function

        Private Const SubModelsCacheKeyRoot As String = "DatSubModelsFor"

        Public Function BeginGetSubModels(credentials As DatCredentials, vehicleType As Integer, manufacturer As Integer, baseModel As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Return Me.BeginGetSubModels(credentials, vehicleType, manufacturer, baseModel, 0, cb, state)
        End Function

        Public Function BeginGetSubModels(credentials As DatCredentials, vehicleType As Integer, manufacturer As Integer, baseModel As Integer, restriction As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Return Me.BeginGetSubModels(credentials, vehicleType, manufacturer, baseModel, restriction, 10, cb, state)
        End Function

        Public Function BeginGetSubModels(credentials As DatCredentials, vehicleType As Integer, manufacturer As Integer, baseModel As Integer, restriction As Integer, constructionDateFrom As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Return Me.BeginGetSubModels(credentials, vehicleType, manufacturer, baseModel, restriction, constructionDateFrom, DatConnector.MaxConstructionDate, cb, state)
        End Function

        Public Function BeginGetSubModels(credentials As DatCredentials, vehicleType As Integer, manufacturer As Integer, baseModel As Integer, restriction As Integer, constructionDateFrom As Integer, constructionDateTo As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Dim result As IAsyncResult
            Dim subModels As IDictionary(Of Integer, String) = DirectCast(HttpRuntime.Cache(DatConnector.ComposeKey(DatConnector.SubModelsCacheKeyRoot, vehicleType, manufacturer, baseModel, restriction, constructionDateFrom, constructionDateTo)), IDictionary(Of Integer, String))

            If subModels Is Nothing Then
                Dim request As New getSubModelsRequest()
                request.datCustomerNumber = credentials.CustomerNumber
                request.userName = credentials.UserName
                request.password = credentials.Password
                request.vehicleType = vehicleType
                request.manufacturer = manufacturer
                request.baseModel = baseModel
                request.restriction = restriction
                request.constructionTimeFrom = constructionDateFrom
                request.constructionTimeTo = constructionDateTo

                result = Me.client.BegingetSubModels(request, cb, state)
                result = New AsyncResultWrapper(Of Integer)(result, vehicleType, manufacturer, baseModel, restriction, constructionDateFrom, constructionDateTo)
            Else
                result = New AsyncResult(Of IDictionary(Of Integer, String))(subModels, state)

                If cb IsNot Nothing Then
                    cb(result)
                End If
            End If

            Return result
        End Function

        Public Function EndGetSubModels(asyncResult As IAsyncResult) As IDictionary(Of Integer, String)
            Dim subModels As IDictionary(Of Integer, String)
            Dim result As AsyncResult(Of IDictionary(Of Integer, String)) = TryCast(asyncResult, AsyncResult(Of IDictionary(Of Integer, String)))

            If result Is Nothing Then
                Dim wrapper As AsyncResultWrapper(Of Integer) = DirectCast(asyncResult, AsyncResultWrapper(Of Integer))

                Try
                    Dim response As getSubModelsResponse = Me.client.EndgetSubModels(wrapper.OriginalResult)
                    subModels = response.getSubModelsReturn.OrderBy(Function(i) i.value).ToDictionary(Function(i) i.key, Function(i) i.value)
                Catch ex As ServiceModel.FaultException
                    Dim de As DatException = DatException.FromFault(ex)

                    If de IsNot Nothing Then
                        Throw de
                    Else
                        Throw
                    End If
                End Try

                HttpRuntime.Cache.Insert(DatConnector.ComposeKey(DatConnector.SubModelsCacheKeyRoot, wrapper.KeyComponents), subModels)
            Else
                subModels = result.Result
            End If

            Return subModels
        End Function

        Private Const DatECodeCacheKeyRoot As String = "DatECodeFor"

        Public Function BeginCompileDatECode(credentials As DatCredentials, vehicleType As Integer, manufacturer As Integer, baseModel As Integer, subModel As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Return Me.BeginCompileDatECode(credentials, vehicleType, manufacturer, baseModel, subModel, New Integer() {}, cb, state)
        End Function

        Public Function BeginCompileDatECode(credentials As DatCredentials, vehicleType As Integer, manufacturer As Integer, baseModel As Integer, subModel As Integer, availableOptions As ICollection(Of Integer), cb As AsyncCallback, state As Object) As IAsyncResult
            Dim result As IAsyncResult

            Dim keyParts(availableOptions.Count + 3) As Integer
            keyParts(0) = vehicleType
            keyParts(1) = manufacturer
            keyParts(2) = baseModel
            keyParts(3) = subModel
            availableOptions.CopyTo(keyParts, 4)
            Dim datECode As String = DirectCast(HttpRuntime.Cache(DatConnector.ComposeKey(DatConnector.DatECodeCacheKeyRoot, keyParts)), String)

            If datECode Is Nothing Then
                Dim request As New compileDatECodeRequest()
                request.datCustomerNumber = credentials.CustomerNumber
                request.userName = credentials.UserName
                request.password = credentials.Password
                request.vehicleType = vehicleType
                request.manufacturer = manufacturer
                request.baseModel = baseModel
                request.subModel = subModel
                request.availableOptions = availableOptions.ToArray()

                result = Me.client.BegincompileDatECode(request, cb, state)
                result = New AsyncResultWrapper(Of Integer)(result, keyParts)
            Else
                result = New AsyncResult(Of String)(datECode, state)

                If cb IsNot Nothing Then
                    cb(result)
                End If
            End If

            Return result
        End Function

        Public Function EndCompileDatECode(asyncResult As IAsyncResult) As String
            Dim datECode As String
            Dim result As AsyncResult(Of String) = TryCast(asyncResult, AsyncResult(Of String))

            If result Is Nothing Then
                Dim wrapper As AsyncResultWrapper(Of Integer) = DirectCast(asyncResult, AsyncResultWrapper(Of Integer))

                Try
                    Dim response As compileDatECodeResponse = Me.client.EndcompileDatECode(wrapper.OriginalResult)
                    datECode = response.compileDatECodeReturn
                Catch ex As ServiceModel.FaultException
                    Dim de As DatException = DatException.FromFault(ex)

                    If de IsNot Nothing Then
                        Throw de
                    Else
                        Throw
                    End If
                End Try

                HttpRuntime.Cache.Insert(DatConnector.ComposeKey(DatConnector.SubModelsCacheKeyRoot, wrapper.KeyComponents), datECode)
            Else
                datECode = result.Result
            End If

            Return datECode
        End Function

        Private Const EngineOptionsRoot As String = "DatEngineOptionsFor"

        Public Function BeginGetEngineOptions(credentials As DatCredentials, vehicleType As Integer, manufacturer As Integer, baseModel As Integer, subModel As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Return Me.BeginGetEngineOptions(credentials, vehicleType, manufacturer, baseModel, subModel, 0, cb, state)
        End Function

        Public Function BeginGetEngineOptions(credentials As DatCredentials, vehicleType As Integer, manufacturer As Integer, baseModel As Integer, subModel As Integer, restriction As Integer, cb As AsyncCallback, state As Object) As IAsyncResult
            Return Me.BeginGetEngineOptions(credentials, vehicleType, manufacturer, baseModel, subModel, 0, New Integer() {}, cb, state)
        End Function

        Public Function BeginGetEngineOptions(credentials As DatCredentials, vehicleType As Integer, manufacturer As Integer, baseModel As Integer, subModel As Integer, restriction As Integer, availableOptions As ICollection(Of Integer), cb As AsyncCallback, state As Object) As IAsyncResult
            Dim result As IAsyncResult

            Dim keyParts(availableOptions.Count + 4) As Integer
            keyParts(0) = vehicleType
            keyParts(1) = manufacturer
            keyParts(2) = baseModel
            keyParts(3) = subModel
            keyParts(4) = restriction
            availableOptions.CopyTo(keyParts, 5)
            Dim engineOptions As IDictionary(Of Integer, String) = DirectCast(HttpRuntime.Cache(DatConnector.ComposeKey(DatConnector.EngineOptionsRoot, keyParts)), IDictionary(Of Integer, String))

            If engineOptions Is Nothing Then
                Dim request As New getEngineOptionsRequest()
                request.datCustomerNumber = credentials.CustomerNumber
                request.userName = credentials.UserName
                request.password = credentials.Password
                request.vehicleType = vehicleType
                request.manufacturer = manufacturer
                request.baseModel = baseModel
                request.subModel = subModel
                request.availableOptions = availableOptions.ToArray()

                result = Me.client.BegingetEngineOptions(request, cb, state)
                result = New AsyncResultWrapper(Of Integer)(result, keyParts)
            Else
                result = New AsyncResult(Of IDictionary(Of Integer, String))(engineOptions, state)

                If cb IsNot Nothing Then
                    cb(result)
                End If
            End If

            Return result
        End Function

        Public Function EndGetEngineOptions(asyncResult As IAsyncResult) As IDictionary(Of Integer, String)
            Dim engineOptions As IDictionary(Of Integer, String)
            Dim result As AsyncResult(Of IDictionary(Of Integer, String)) = TryCast(asyncResult, AsyncResult(Of IDictionary(Of Integer, String)))

            If result Is Nothing Then
                Dim wrapper As AsyncResultWrapper(Of Integer) = DirectCast(asyncResult, AsyncResultWrapper(Of Integer))

                Try
                    Dim response As getEngineOptionsResponse = Me.client.EndgetEngineOptions(wrapper.OriginalResult)
                    engineOptions = response.getEngineOptionsReturn.OrderBy(Function(i) i.value).ToDictionary(Function(i) i.key, Function(i) i.value)
                Catch ex As ServiceModel.FaultException
                    Dim de As DatException = DatException.FromFault(ex)

                    If de IsNot Nothing Then
                        Throw de
                    Else
                        Throw
                    End If
                End Try

                HttpRuntime.Cache.Insert(DatConnector.ComposeKey(DatConnector.EngineOptionsRoot, wrapper.KeyComponents), engineOptions)
            Else
                engineOptions = result.Result
            End If

            Return engineOptions
        End Function

        Private Const PriceDiscoveryConstructionYearsFor As String = ""

        Public Function BeginGetPriceDiscoveryConstructionYears(credentials As DatCredentials, datECode As String, cb As AsyncCallback, state As Object) As IAsyncResult
            Dim result As IAsyncResult
            Dim priceDiscoveryConstructionYears As ICollection(Of Integer) = DirectCast(HttpRuntime.Cache(DatConnector.ComposeKey(DatConnector.PriceDiscoveryConstructionYearsFor, datECode)), ICollection(Of Integer))

            If priceDiscoveryConstructionYears Is Nothing Then
                Dim request As New getPriceDiscoveryConstructionYearsRequest()
                request.datCustomerNumber = credentials.CustomerNumber
                request.userName = credentials.UserName
                request.password = credentials.Password
                request.datECode = datECode

                result = Me.client.BegingetPriceDiscoveryConstructionYears(request, cb, state)
                result = New AsyncResultWrapper(Of String)(result, datECode)
            Else
                result = New AsyncResult(Of ICollection(Of Integer))(priceDiscoveryConstructionYears, state)

                If cb IsNot Nothing Then
                    cb(result)
                End If
            End If

            Return result
        End Function

        Public Function EndGetPriceDiscoveryConstructionYears(asyncResult As IAsyncResult) As ICollection(Of Integer)
            Dim priceDiscoveryConstructionYears As ICollection(Of Integer)
            Dim result As AsyncResult(Of ICollection(Of Integer)) = TryCast(asyncResult, AsyncResult(Of ICollection(Of Integer)))

            If result Is Nothing Then
                Dim wrapper As AsyncResultWrapper(Of String) = DirectCast(asyncResult, AsyncResultWrapper(Of String))

                Try
                    Dim response As getPriceDiscoveryConstructionYearsResponse = Me.client.EndgetPriceDiscoveryConstructionYears(wrapper.OriginalResult)
                    priceDiscoveryConstructionYears = response.getPriceDiscoveryConstructionYearsReturn
                Catch ex As ServiceModel.FaultException
                    Dim de As DatException = DatException.FromFault(ex)

                    If de IsNot Nothing Then
                        Throw de
                    Else
                        Throw
                    End If
                End Try

                HttpRuntime.Cache.Insert(DatConnector.ComposeKey(DatConnector.PriceDiscoveryConstructionYearsFor, wrapper.KeyComponents), priceDiscoveryConstructionYears)
            Else
                priceDiscoveryConstructionYears = result.Result
            End If

            Return priceDiscoveryConstructionYears
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
            DirectCast(Me.client, IDisposable).Dispose()
            GC.SuppressFinalize(Me)
        End Sub
    End Class
End Namespace