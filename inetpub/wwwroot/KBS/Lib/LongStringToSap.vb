Option Explicit On
Option Strict On

Public NotInheritable Class LongStringToSap
    Inherits ErrorHandlingClass

    Dim mLText As String = ""
    Dim mLTextID As String = ""
    Dim mLTextNr As String = ""
    Dim mUName As String = ""
    Dim SAPExc As SAPExecutor.SAPExecutor

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

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_LTEXT_NR", False, mLTextNr})
            dt.Rows.Add(New Object() {"I_STRING", False, mLText})
            dt.Rows.Add(New Object() {"I_UNAME", False, username})

            SAPExc.ExecuteERP("Z_BC_LTEXT_UPDATE", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub DeleteStringERP(ByVal ltnr As String)
        ClearErrorState()

        mLTextNr = ltnr
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_LTEXT_NR", False, mLTextNr})
            SAPExc.ExecuteERP("Z_BC_LTEXT_DELETE", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
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
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_LTEXT_NR", False, mLTextNr})
            'ExportParameter
            dt.Rows.Add(New Object() {"E_STRING"})
            dt.Rows.Add(New Object() {"E_LTEXT_ID"})

            SAPExc.ExecuteERP("Z_BC_LTEXT_READ", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            End If
            Dim retRows = dt.Select("Fieldname='E_STRING'")(0)
            If Not retRows Is Nothing Then
                mLText = retRows("Data").ToString
            End If
            retRows = dt.Select("Fieldname='E_LTEXT_ID'")(0)
            If Not retRows Is Nothing Then
                mLTextID = retRows("Data").ToString
            End If
        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return mLText
    End Function

    Public Function InsertStringERP(ByVal Text As String, ByVal TextID As String, Optional ByVal username As String = "") As String
        ClearErrorState()

        mLText = Text
        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try
            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()
            'befüllen der Importparameter
            dt.Rows.Add(New Object() {"I_STRING", False, mLText})
            dt.Rows.Add(New Object() {"I_LTEXT_ID", False, TextID}) '"UMLT"
            dt.Rows.Add(New Object() {"I_UNAME", False, username})

            dt.Rows.Add(New Object() {"E_LTEXT_NR", True})
            SAPExc.ExecuteERP("Z_BC_LTEXT_INSERT", dt)

            If (SAPExc.ErrorOccured) Then
                RaiseError(SAPExc.E_SUBRC, SAPExc.E_MESSAGE)
            End If
            Dim retRows = dt.Select("Fieldname='E_LTEXT_NR'")(0)
            If Not retRows Is Nothing Then
                mLTextNr = retRows("Data").ToString
            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return mLTextNr
    End Function

#End Region

End Class
