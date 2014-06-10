Imports System.Web.Services
Imports System.ComponentModel

<WebService(Namespace:="http://kroschke.de/")> _
Public Class ServiceData
    Inherits WebService

    Friend WithEvents EventLog1 As EventLog

    <WebMethod()> Public Function WMInsertFreisetzung_Zul(ByVal User As String, ByVal Password As String, ByVal VehicleRegistrations As VehicleRegs_Zul) As Errors
        Dim VehErrors As New Errors()

        Try
            'Login überprüfen
            If Not CheckLogin(User, Password) Then
                Throw New Exception("WMInsertFreisetzung_Zul, User oder Password nicht korrekt.")
            End If

            Dim SetData As New SapInterface()

            VehErrors = SetData.InsertFreisetzung_Zul(VehicleRegistrations)

        Catch ex As Exception
            Try
                EventLog.WriteEntry("SixtServiceLeas", "WMInsertFreisetzung_Zul: " & ex.Message, EventLogEntryType.Warning)
            Catch
                'Fehlgeschlagener Eventlog-Eintrag darf nicht zum Abbruch der Anwendung führen
            End Try

        End Try

        Return VehErrors
    End Function

    <WebMethod()> Public Function WMInsertFreisetzung_SonstDL(ByVal User As String, ByVal Password As String, ByVal VehicleRegistrations As VehicleRegs_Sonst) As Errors
        Dim VehErrors As New Errors()

        Try
            'Login überprüfen
            If Not CheckLogin(User, Password) Then
                Throw New Exception("WMInsertFreisetzung_SonstDL, User oder Password nicht korrekt.")
            End If

            Dim SetData As New SapInterface()

            VehErrors = SetData.InsertFreisetzung_SonstDL(VehicleRegistrations)

        Catch ex As Exception
            Try
                EventLog.WriteEntry("SixtServiceLeas", "WMInsertFreisetzung_SonstDL: " & ex.Message, EventLogEntryType.Warning)
            Catch
                'Fehlgeschlagener Eventlog-Eintrag darf nicht zum Abbruch der Anwendung führen
            End Try

        End Try

        Return VehErrors
    End Function

    <WebMethod()> Public Function WMInsertFreisetzung_EndgVers(ByVal User As String, ByVal Password As String, ByVal VehicleRegistrations As VehicleRegs_EndgVers) As Errors
        Dim VehErrors As New Errors()

        Try
            'Login überprüfen
            If Not CheckLogin(User, Password) Then
                Throw New Exception("WMInsertFreisetzung_EndgVers, User oder Password nicht korrekt.")
            End If

            Dim SetData As New SapInterface()

            VehErrors = SetData.InsertFreisetzung_EndgVers(VehicleRegistrations)

        Catch ex As Exception
            Try
                EventLog.WriteEntry("SixtServiceLeas", "WMInsertFreisetzung_EndgVers: " & ex.Message, EventLogEntryType.Warning)
            Catch
                'Fehlgeschlagener Eventlog-Eintrag darf nicht zum Abbruch der Anwendung führen
            End Try

        End Try

        Return VehErrors
    End Function

    <WebMethod()> Public Function WMInsertFreisetzung_TempVers(ByVal User As String, ByVal Password As String, ByVal VehicleRegistrations As VehicleRegs_TempVers) As Errors
        Dim VehErrors As New Errors()

        Try
            'Login überprüfen
            If Not CheckLogin(User, Password) Then
                Throw New Exception("WMInsertFreisetzung_TempVers, User oder Password nicht korrekt.")
            End If

            Dim SetData As New SapInterface()

            VehErrors = SetData.InsertFreisetzung_TempVers(VehicleRegistrations)

        Catch ex As Exception
            Try
                EventLog.WriteEntry("SixtServiceLeas", "WMInsertFreisetzung_TempVers: " & ex.Message, EventLogEntryType.Warning)
            Catch
                'Fehlgeschlagener Eventlog-Eintrag darf nicht zum Abbruch der Anwendung führen
            End Try

        End Try

        Return VehErrors
    End Function

    Private Sub InitializeComponent()
        Try
            EventLog1 = New EventLog
            CType(EventLog1, ISupportInitialize).BeginInit()

            EventLog1.Log = "Application"
            CType(EventLog1, ISupportInitialize).EndInit()
        Catch
            'Fehlgeschlagener Eventlog-Eintrag darf nicht zum Abbruch der Anwendung führen
        End Try

    End Sub

    Private Function CheckLogin(ByVal User As String, ByVal Password As String) As Boolean

        If User <> ConfigurationManager.AppSettings("Username") OrElse Password <> ConfigurationManager.AppSettings("Password") Then
            Return False
        Else
            Return True
        End If

    End Function

    <WebMethod()> Public Function WMGetFreisetzung_Status(ByVal User As String, ByVal Password As String) As String
        Dim strXml As String

        Try

            'Login überprüfen
            If Not CheckLogin(User, Password) Then
                Throw New Exception("WMGetFreisetzung_Status, User oder Password nicht korrekt.")
            End If

            Dim SetData As SapInterface
            SetData = New SapInterface()
            strXml = SetData.WMGetFreisetzungStatus()

        Catch ex As Exception
            Try
                'Error in das Eventlog schreiben
                EventLog.WriteEntry("SixtServiceLeas", "WMGetFreisetzung_Status: " & ex.Message, EventLogEntryType.Warning)
            Catch
                'Fehlgeschlagener Eventlog-Eintrag darf nicht zum Abbruch der Anwendung führen
            End Try

            Throw
        End Try

        Return strXml
    End Function

    Public Sub New()
        MyBase.New()

        'Dieser Aufruf ist für den Webdienst-Designer erforderlich.
        InitializeComponent()
        ' Fügen Sie Ihren eigenen Initialisierungscode hinter dem InitializeComponent()-Aufruf ein

    End Sub

End Class