Imports System.Text
Imports System.Security.Cryptography




Public Class Security
    Private ReadOnly _lbtVector() As Byte = {240, 3, 45, 29, 0, 76, 173, 59}
    Private ReadOnly _lscryptoKey As String = "PassCrypt"



    Public Function PsDecrypt(ByVal sQueryString As String) As String

        Dim buffer() As Byte
        Dim loCryptoClass As New TripleDESCryptoServiceProvider
        Dim loCryptoProvider As New MD5CryptoServiceProvider

        Try

            buffer = Convert.FromBase64String(sQueryString)
            loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(_lscryptoKey))
            loCryptoClass.IV = _lbtVector
            Return Encoding.ASCII.GetString(loCryptoClass.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length()))
        Catch ex As Exception
            Throw
        Finally
            loCryptoClass.Clear()
            loCryptoProvider.Clear()

        End Try


    End Function

    Public Function PsEncrypt(ByVal sInputVal As String) As String

        Dim loCryptoClass As New TripleDESCryptoServiceProvider
        Dim loCryptoProvider As New MD5CryptoServiceProvider
        Dim lbtBuffer() As Byte

        Try
            lbtBuffer = Encoding.ASCII.GetBytes(sInputVal)
            loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(_lscryptoKey))
            loCryptoClass.IV = _lbtVector
            sInputVal = Convert.ToBase64String(loCryptoClass.CreateEncryptor().TransformFinalBlock(lbtBuffer, 0, lbtBuffer.Length()))
            psEncrypt = sInputVal
        Catch ex As CryptographicException
            Throw
        Catch ex As FormatException
            Throw
        Catch ex As Exception
            Throw
        Finally
            loCryptoClass.Clear()
            loCryptoProvider.Clear()
        End Try
    End Function

End Class
