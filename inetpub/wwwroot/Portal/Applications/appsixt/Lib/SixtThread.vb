Imports System.Threading

Public Class SixtThread
    Private running As Boolean
    Private finished As Boolean
    Private obj_work As SIXT_PDI
    Private newThread As System.Threading.Thread

    Public ReadOnly Property objThread() As Thread
        Get
            Return newThread
        End Get
    End Property

    Public ReadOnly Property IsRunning() As Boolean
        Get
            Return running
        End Get
    End Property

    Public Property IsFinished() As Boolean
        Get
            Return finished
        End Get
        Set(ByVal Value As Boolean)
            finished = Value
        End Set
    End Property

    Public Property WorkObject() As SIXT_PDI
        Get
            Return obj_work
        End Get
        Set(ByVal Value As SIXT_PDI)
            obj_work = Value
        End Set
    End Property

    Public Sub New()
        finished = False
        running = False
        obj_work = Nothing
    End Sub

    Public Sub DoChange()
        running = True
        obj_work.Change()
        running = False
        finished = True
    End Sub

    Public Sub start()
        If Not obj_work Is Nothing Then
            newThread = New System.Threading.Thread(AddressOf DoChange)
            '//set the thread priority to something
            '//that is acceptable, warning setting the 
            '//priority to normal or higher will 
            '//potentially lock up your server,
            '//or make your asp.net pages respond very 
            '//slowly. The best priority to use is lowest
            newThread.Priority = ThreadPriority.Lowest
            newThread.Start()

        Else
            Throw New Exception("Kein Arbeitsobjekt festgelegt.")
        End If
    End Sub
End Class

' ************************************************
' $History: SixtThread.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 8.04.08    Time: 13:38
' Created in $/CKAG/Applications/appsixt/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 8.03.07    Time: 13:15
' Updated in $/CKG/Applications/AppSIXT/AppSIXTWeb/Lib
' 
' ************************************************
