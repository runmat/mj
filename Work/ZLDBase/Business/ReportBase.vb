Option Explicit On
Option Strict On

Imports System

Namespace Business
    Public MustInherit Class ReportBase
        REM § Basisklasse aller nur lesenden Reports, Fill-Methoden jeweils individuell,
        REM § Ergebnisse in Datatable-Property.
        Implements Base.Common.ISapError

#Region " Declarations"

        Protected m_tblResult As DataTable
        Protected m_tableResult As DataTable
        Protected m_objUser As Base.Kernel.Security.User
        Protected m_objApp As Base.Kernel.Security.App
        Protected m_intStatus As Int32
        Protected m_strMessage As String
        Protected m_blnErrorOccured As Boolean
        Protected m_blnGestartet As Boolean
        Protected m_strAppID As String
        Protected m_strSessionID As String
        Protected m_strFileName As String
        Protected m_strClassAndMethod As String

#End Region

#Region " Properties"

        Public Property ResultTable() As DataTable
            Get
                Return m_tableResult
            End Get
            Set(ByVal Value As DataTable)
                m_tableResult = Value
            End Set
        End Property

        Public ReadOnly Property Result() As DataTable
            Get
                Return m_tblResult
            End Get
        End Property

        Public ReadOnly Property Status() As Integer
            Get
                Return m_intStatus
            End Get
        End Property

        Public ReadOnly Property ErrorCode() As Integer Implements Common.ISapError.ErrorCode
            Get
                Return m_intStatus
            End Get
        End Property

        Public ReadOnly Property Message() As String Implements Common.ISapError.ErrorMessage
            Get
                Return m_strMessage
            End Get
        End Property

        Public ReadOnly Property ErrorOccured() As Boolean Implements Common.ISapError.ErrorOccured
            Get
                Return m_blnErrorOccured
            End Get
        End Property

        Public ReadOnly Property Gestartet() As Boolean
            Get
                Return m_blnGestartet
            End Get
        End Property

        Public Property AppID() As String
            Get
                Return m_strAppID
            End Get
            Set(ByVal Value As String)
                m_strAppID = Value
            End Set
        End Property

        Public Property SessionID() As String
            Get
                Return m_strSessionID
            End Get
            Set(ByVal Value As String)
                m_strSessionID = Value
            End Set
        End Property

#End Region

#Region " Methods"

        Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFileName As String)
            REM § Constructor. Übernimmt User und Applikationsobjekt und Verbindungsobjekt (SAPDestination).
            m_objUser = objUser
            m_objApp = objApp
            m_strFileName = strFileName

            m_intStatus = 0
            m_strMessage = ""
        End Sub

        Public MustOverride Sub Fill()

        '''<summary>
        ''' Setzt den Fehlerzustand der Klasse zurück
        '''</summary>
        Protected Overridable Sub ClearError() Implements Common.ISapError.ClearError
            m_blnErrorOccured = False
            m_intStatus = 0
            m_strMessage = ""
        End Sub

        '''<summary>
        ''' Löst ein Fehlerereignis mit Fehlercode und Fehlermeldung aus
        '''</summary>
        Protected Overridable Sub RaiseError(errorcode As Integer, message As String) Implements Common.ISapError.RaiseError
            m_blnErrorOccured = True
            m_intStatus = errorcode
            m_strMessage = message
        End Sub

#End Region

#Region "Obsolete Functions"

        <Obsolete("Diese Funktion ist veraltet! Wenn nötig ist die Funktion Business.HelpProcedures.MakeDateSAP() zu verwenden!", False)>
        Public Function MakeDateSAP(ByVal datInput As Date) As String
            REM $ Formt Date-Input in String YYYYMMDD um
            Return Year(datInput) & Right("0" & Month(datInput), 2) & Right("0" & Day(datInput), 2)
        End Function

        <Obsolete("Diese Funktion ist veraltet! Wenn nötig ist die Funktion Business.HelpProcedures.MakeDateSAP() zu verwenden!", False)>
        Public Function MakeDateSAP(ByVal datInput As String) As String
            REM $ Formt Date-Input in String YYYYMMDD um
            Dim dat As Date

            dat = CType(datInput, Date)
            Return Year(dat) & Right("0" & Month(dat), 2) & Right("0" & Day(dat), 2)
        End Function

        <Obsolete("Diese Funktion ist veraltet! Wenn nötig ist die Funktion Business.HelpProcedures.MakeDateStandard() zu verwenden!", False)>
        Public Function MakeDateStandard(ByVal strInput As String) As Date
            REM § Formt String-Input im SAP-Format in Standard-Date um. Gibt "01.01.1900" zurück, wenn Umwandlung nicht möglich ist.
            Dim strTemp As String = Right(strInput, 2) & "." & Mid(strInput, 5, 2) & "." & Left(strInput, 4)
            If IsDate(strTemp) Then
                Return CDate(strTemp)
            Else
                Return CDate("01.01.1900")
            End If
        End Function

#End Region

    End Class
End Namespace

' ************************************************
' $History: ReportBase.vb $
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 27.07.09   Time: 9:25
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 4.06.08    Time: 16:42
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 5  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Base/Base/Business
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 4  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Business
' 
' ************************************************