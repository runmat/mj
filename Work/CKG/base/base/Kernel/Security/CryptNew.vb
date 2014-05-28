Imports System.Text
Imports System.Collections.Specialized
Imports System.Security.Cryptography


Namespace Kernel.Security
    <Serializable()> Public Class CryptNew
        Private lbtVector() As Byte = {240, 3, 45, 29, 0, 76, 173, 59}
        Private lscryptoKey As String = "PassCrypt"


        Public Function psDecrypt(ByVal sQueryString As String) As String

            Dim buffer() As Byte
            Dim loCryptoClass As New TripleDESCryptoServiceProvider
            Dim loCryptoProvider As New MD5CryptoServiceProvider

            Try

                buffer = Convert.FromBase64String(sQueryString)
                loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lscryptoKey))
                loCryptoClass.IV = lbtVector
                Return Encoding.ASCII.GetString(loCryptoClass.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length()))
            Catch ex As Exception

            Finally
                loCryptoClass.Clear()
                loCryptoProvider.Clear()
                loCryptoClass = Nothing
                loCryptoProvider = Nothing
            End Try


        End Function

        Public Function psEncrypt(ByVal sInputVal As String) As String

            Dim loCryptoClass As New TripleDESCryptoServiceProvider
            Dim loCryptoProvider As New MD5CryptoServiceProvider
            Dim lbtBuffer() As Byte

            Try
                lbtBuffer = System.Text.Encoding.ASCII.GetBytes(sInputVal)
                loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lscryptoKey))
                loCryptoClass.IV = lbtVector
                sInputVal = Convert.ToBase64String(loCryptoClass.CreateEncryptor().TransformFinalBlock(lbtBuffer, 0, lbtBuffer.Length()))
                psEncrypt = sInputVal
            Catch ex As CryptographicException
                Throw ex
            Catch ex As FormatException
                Throw ex
            Catch ex As Exception
                Throw ex
            Finally
                loCryptoClass.Clear()
                loCryptoProvider.Clear()
                loCryptoClass = Nothing
                loCryptoProvider = Nothing
            End Try
        End Function

    End Class
End Namespace
' ************************************************
' $History: CryptNew.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.10.09    Time: 16:31
' Created in $/CKAG/Base/Kernel/Security
' 