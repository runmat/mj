Option Explicit On
Option Strict On

Imports System.Configuration

Public Class Common

    Public Shared ReadOnly Property ProdSAP() As Boolean
        Get
            Return (Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("ProdSAP")) AndAlso ConfigurationManager.AppSettings("ProdSAP").ToUpper() = "TRUE")
        End Get
    End Property

    ''' <summary>
    ''' Prüft ob der mitgegebene String ein numerischer Wert ist.
    ''' </summary>
    ''' <param name="val">numerischer String</param>
    ''' <returns>true bei numerisch, false bei nicht numerisch</returns>
    Public Shared Function IsNumeric(ByVal val As String) As Boolean
        Try
            Convert.ToInt32(val)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Prüft ob der mitgegebene String dezimal-Wert ist.
    ''' </summary>
    ''' <param name="val">String dezimal</param>
    ''' <returns>true bei dezimal, false bei nicht dezimal</returns>
    Public Shared Function IsDecimal(ByVal val As String) As Boolean
        Try
            Convert.ToDecimal(val)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Prüft ob der mitgegebene String ein Datum ist
    ''' </summary>
    ''' <param name="val">Date String</param>
    ''' <returns>true bei Date, false bei nicht Date</returns>
    Public Shared Function IsDate(ByVal val As String) As Boolean
        Try
            Dim tmpDate As DateTime
            Return DateTime.TryParse(val, tmpDate)
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Wandelt Boolean-Wert in X oder "" für die
    ''' Datenaufbereitung SAP um.
    ''' </summary>
    ''' <param name="val">Boolean-Wert</param>
    ''' <returns>X bei true, "" bei false</returns>
    Public Shared Function BoolToX(ByVal val As Nullable(Of Boolean)) As String
        If val.HasValue AndAlso val.Value Then Return "X"
        Return ""
    End Function

    ''' <summary>
    ''' Wandelt Boolean-Wert in X oder "" für die
    ''' Datenaufbereitung SAP um.
    ''' </summary>
    ''' <param name="val">Boolean-Wert</param>
    ''' <returns>X bei true, "" bei false</returns>
    Public Shared Function BoolToX(ByVal val As Boolean) As String
        If val Then Return "X"
        Return ""
    End Function

    ''' <summary>
    ''' Wandelt X bzw. " " in boolean-Wert um
    ''' </summary>
    ''' <param name="val"></param>
    ''' <returns></returns>
    Public Shared Function XToBool(ByVal val As String) As Boolean
        Return (val = "X")
    End Function

    ''' <summary>
    ''' Helper.
    ''' wandelt das benutzte Kurzdatum z.B. 010112 in 01.01.2012
    ''' </summary>
    ''' <param name="dat">Kurzdatum</param>
    ''' <returns>norm. Datum</returns>
    Public Shared Function toShortDateStr(ByVal dat As String) As String
        Dim datum As DateTime

        Try
            datum = Convert.ToDateTime(dat.Substring(0, 2) & "." & dat.Substring(2, 2) & "." & DateTime.Now.Year.ToString().Substring(0, 2) & dat.Substring(4, 2))
        Catch ex As Exception
            Return ""
        End Try

        Return datum.ToShortDateString()
    End Function

End Class
