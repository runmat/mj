Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Common
Imports CKG.Base.Kernel.Common
Imports SapORM.Contracts

Namespace Business

    Public MustInherit Class SapOrmBusinessBase
        Implements ISapError

#Region " Declarations"

        Protected m_intErrorCode As Integer
        Protected m_strErrorMessage As String
        Protected m_blnErrorOccured As Boolean
        Protected m_blnGestartet As Boolean

#End Region

#Region " Properties"

        Public VKORG As String
        Public VKBUR As String

        Public ReadOnly Property ErrorOccured() As Boolean Implements ISapError.ErrorOccured
            Get
                Return m_blnErrorOccured
            End Get
        End Property

        Public ReadOnly Property ErrorCode() As Integer Implements ISapError.ErrorCode
            Get
                Return m_intErrorCode
            End Get
        End Property

        Public ReadOnly Property Message() As String Implements ISapError.ErrorMessage
            Get
                Return m_strErrorMessage
            End Get
        End Property

        Protected ReadOnly Property SAP() As ISapDataService
            Get
                Return S.AP
            End Get
        End Property

#End Region

#Region " Methods"

        Public Sub New()
            ClearError()
        End Sub

        Protected Overridable Sub ClearError() Implements ISapError.ClearError
            m_blnErrorOccured = False
            m_intErrorCode = 0
            m_strErrorMessage = ""
        End Sub

        Protected Overridable Sub RaiseError(errCode As Integer, msg As String) Implements ISapError.RaiseError
            m_blnErrorOccured = True
            m_intErrorCode = errCode
            m_strErrorMessage = msg
        End Sub

        Protected Sub CallBapi()
            SAP.Execute()

            If SAP.ResultCode <> 0 Then
                RaiseError(SAP.ResultCode, SAP.ResultMessage)
            End If
        End Sub

        Protected Sub ExecuteSapZugriff(ByVal sapZugriffsCode As Action)

            If Not m_blnGestartet Then

                m_blnGestartet = True

                ClearError()

                Try
                    sapZugriffsCode()

                Catch ex As Exception
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case "NO_DATA"
                            RaiseError(-5555, "Keine Daten gefunden.")
                        Case "NO_LIEF"
                            RaiseError(103, "Kein Lieferanten/Zulassungsdienst gepflegt! Keine Versandzulassung möglich!")
                        Case Else
                            RaiseError(-9999, "Beim SAP-Zugriff ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")")
                    End Select

                Finally
                    m_blnGestartet = False

                End Try

            End If

        End Sub

#End Region

    End Class

End Namespace
