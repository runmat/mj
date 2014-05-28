Namespace Kernel

    Public Class AbrufgruendeDT
        Inherits DataTable

#Region " Constructor "
        Public Sub New(ByVal cn As SqlClient.SqlConnection, ByVal customerID As String, ByVal GroupID As String, ByVal filtered As Boolean, ByVal abrufgrundtyp As String)
            If filtered Then
                Try
                    Me.Clear()
                    If cn.State = ConnectionState.Closed Then
                        cn.Open()
                    End If

                    Dim sql As String = "SELECT dbo.CustomerAbrufgruende.*" & _
                    " FROM dbo.CustomerAbrufgruende" & _
                    " WHERE dbo.CustomerAbrufgruende.CustomerID = " & customerID & " and" & _
                    " CustomerAbrufgruende.abruftyp = '" & abrufgrundtyp & "' and SapWert <> 000 And" & _
                    " dbo.CustomerAbrufgruende.SAPWert not in" & _
                    " (SELECT dbo.CustomerAbrufgruende.SAPWert From dbo.CustomerAbrufgruende" & _
                    " WHERE (dbo.CustomerAbrufgruende.CustomerID = " & customerID & ")" & _
                    " AND (dbo.CustomerAbrufgruende.GroupID = " & GroupID & ") and CustomerAbrufgruende.abruftyp = '" & abrufgrundtyp & "')  AND (GroupID = 0)"

                    Dim daApp As New SqlClient.SqlDataAdapter(sql, cn)
                    daApp.Fill(Me)
                Finally
                    If cn.State <> ConnectionState.Closed Then
                        cn.Close()
                    End If
                End Try
            Else
                Try
                    Me.Clear()
                    If cn.State = ConnectionState.Closed Then
                        cn.Open()
                    End If
                    Dim daApp As New SqlClient.SqlDataAdapter("SELECT *" & _
                                                                "FROM CustomerAbrufgruende WHERE CustomerID='" & customerID & "' AND GroupID='" & GroupID & "' AND AbrufTyp='" & abrufgrundtyp & "' AND SapWert <> 000", cn)

                    daApp.Fill(Me)
                Finally
                    If cn.State <> ConnectionState.Closed Then
                        cn.Close()
                    End If
                End Try
            End If
        End Sub



        Public Sub New()

        End Sub

#End Region
    End Class
End Namespace