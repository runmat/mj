Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class CommonService
    Inherits System.Web.Services.WebService

    <WebMethod(EnableSession:=True)> _
    Public Function GetCustomerList(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim NewArr(0) As String
        Dim BaseArr() As String
        BaseArr = CType(Session("Kunden"), System.String())
        Try

            If prefixText.Length < 4 Then

                For i = 0 To BaseArr.Length - 1
                    If BaseArr(i).ToUpper.StartsWith(prefixText.ToUpper) Then

                        If NewArr(0) Is Nothing Then
                            NewArr(0) = BaseArr(i)
                        Else

                            ReDim Preserve NewArr(NewArr.Length)

                            NewArr(NewArr.Length - 1) = BaseArr(i)

                        End If

                    End If
                Next
            Else

                For i = 0 To BaseArr.Length - 1
                    If BaseArr(i).ToUpper.Contains(prefixText.ToUpper) Then

                        If NewArr(0) Is Nothing Then
                            NewArr(0) = BaseArr(i)
                        Else

                            ReDim Preserve NewArr(NewArr.Length)

                            NewArr(NewArr.Length - 1) = BaseArr(i)

                        End If

                    End If
                Next
            End If

            If NewArr(0) Is Nothing Then
                NewArr = Nothing
            End If

        Catch ex As Exception
            NewArr = Nothing
            Return NewArr
        End Try


        Return NewArr

    End Function


    <WebMethod(EnableSession:=True)> _
 Public Function GetKreise(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim NewArr(0) As String
        Dim BaseArr() As String
        BaseArr = CType(Session("Kreise"), System.String())
        Try

            If prefixText.Length < 4 Then

                For i = 0 To BaseArr.Length - 1
                    If BaseArr(i).ToUpper.StartsWith(prefixText.ToUpper) Then

                        If NewArr(0) Is Nothing Then
                            NewArr(0) = BaseArr(i)
                        Else

                            ReDim Preserve NewArr(NewArr.Length)

                            NewArr(NewArr.Length - 1) = BaseArr(i)

                        End If

                    End If
                Next


            Else
                For i = 0 To BaseArr.Length - 1
                    If BaseArr(i).ToUpper.Contains(prefixText.ToUpper) Then

                        If NewArr(0) Is Nothing Then
                            NewArr(0) = BaseArr(i)
                        Else

                            ReDim Preserve NewArr(NewArr.Length)

                            NewArr(NewArr.Length - 1) = BaseArr(i)

                        End If

                    End If
                Next
            End If




            If NewArr(0) Is Nothing Then
                NewArr = Nothing
            End If

        Catch ex As Exception
            NewArr = Nothing
            Return NewArr
        End Try


        Return NewArr

    End Function


End Class