Option Explicit On
Option Strict On

Imports KBSBase

Public NotInheritable Class LongStringToSap
    Inherits ErrorHandlingClass

    Dim mLText As String = ""
    Dim mLTextID As String = ""
    Dim mLTextNr As String = ""
    Dim mUName As String = ""

#Region "Properties"

    Public Property UName() As String
        Get
            Return mUName
        End Get
        Set(ByVal value As String)
            mUName = value
        End Set
    End Property

    Public Property LText() As String
        Get
            Return mLText
        End Get
        Set(ByVal value As String)
            mLText = value
        End Set
    End Property

    Public Property LTextID() As String
        Get
            Return mLTextID
        End Get
        Set(ByVal value As String)
            mLTextID = value
        End Set
    End Property

    Public Property LTextNr() As String
        Get
            Return mLTextNr
        End Get
        Set(ByVal value As String)
            mLTextNr = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Sub UpdateStringERP(ByVal Text As String, ByVal ltnr As String, Optional ByVal username As String = "")
        ClearErrorState()

        mLText = Text
        mLTextNr = ltnr

        Try
            S.AP.Init("Z_BC_LTEXT_UPDATE")

            S.AP.SetImportParameter("I_LTEXT_NR", mLTextNr)
            S.AP.SetImportParameter("I_STRING", mLText)
            S.AP.SetImportParameter("I_UNAME", username)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub DeleteStringERP(ByVal ltnr As String)
        ClearErrorState()

        mLTextNr = ltnr

        Try
            S.AP.Init("Z_BC_LTEXT_DELETE", "I_LTEXT_NR", mLTextNr)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

#End Region

#Region "Functions"

    Public Function ReadStringERP(ByVal ltnr As String) As String
        ClearErrorState()

        mLTextNr = ltnr
        mLText = ""

        Try
            S.AP.Init("Z_BC_LTEXT_READ", "I_LTEXT_NR", mLTextNr)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            mLText = S.AP.GetExportParameter("E_STRING")
            mLTextID = S.AP.GetExportParameter("E_LTEXT_ID")

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return mLText
    End Function

    Public Function InsertStringERP(ByVal Text As String, ByVal TextID As String, Optional ByVal username As String = "") As String
        ClearErrorState()

        mLText = Text

        Try
            S.AP.Init("Z_BC_LTEXT_INSERT")

            S.AP.SetImportParameter("I_STRING", mLText)
            S.AP.SetImportParameter("I_LTEXT_ID", TextID)
            S.AP.SetImportParameter("I_UNAME", username)

            S.AP.Execute()

            If S.AP.ResultCode <> 0 Then
                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage)
            End If

            mLTextNr = S.AP.GetExportParameter("E_LTEXT_NR")

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return mLTextNr
    End Function

#End Region

End Class
