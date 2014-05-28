Imports System
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

'§§§ JVE 12.10.2005 <>
'Funktionen Encrypt + Decrypt werden z.Z. nicht genutzt. Nur zu Lern- und Testzwecken!
'§§§ JVE 12.10.2005 <>

Namespace Kernel.Security
    Public Class Crypto
        Private key() As Byte = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24}
        Private iv() As Byte = {65, 110, 68, 26, 69, 178, 200, 219}

        'As you can see, our encryption key consists of 24 bytes and the initialization vector consists of 8 bytes. Feel free to replace the above values with numbers of your choosing. 
        'Now we can begin to build the encryption function. Paste the following code into your TripleDES class. Once the code is in place we can go through it line by line, examining how it works. 

        Public Function Encrypt(ByVal plainText As String) As Byte()
            ' Declare a UTF8Encoding object so we may use the GetByte 
            ' method to transform the plainText into a Byte array. 
            Dim utf8encoder As UTF8Encoding = New UTF8Encoding()
            Dim inputInBytes() As Byte = utf8encoder.GetBytes(plainText)

            ' Create a new TripleDES service provider 
            Dim tdesProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()

            ' The ICryptTransform interface uses the TripleDES 
            ' crypt provider along with encryption key and init vector 
            ' information 
            Dim cryptoTransform As ICryptoTransform = tdesProvider.CreateEncryptor(Me.key, Me.iv)

            ' All cryptographic functions need a stream to output the 
            ' encrypted information. Here we declare a memory stream 
            ' for this purpose. 
            Dim encryptedStream As MemoryStream = New MemoryStream()
            Dim cryptStream As CryptoStream = New CryptoStream(encryptedStream, cryptoTransform, CryptoStreamMode.Write)

            ' Write the encrypted information to the stream. Flush the information 
            ' when done to ensure everything is out of the buffer. 
            cryptStream.Write(inputInBytes, 0, inputInBytes.Length)
            cryptStream.FlushFinalBlock()
            encryptedStream.Position = 0

            ' Read the stream back into a Byte array and return it to the calling 
            ' method. 
            Dim result(encryptedStream.Length - 1) As Byte
            encryptedStream.Read(result, 0, encryptedStream.Length)
            cryptStream.Close()
            Return result
        End Function

        Public Function Decrypt(ByVal inputInBytes() As Byte) As String
            ' UTFEncoding is used to transform the decrypted Byte Array 
            ' information back into a string. 
            Dim utf8encoder As UTF8Encoding = New UTF8Encoding()
            Dim tdesProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()

            ' As before we must provide the encryption/decryption key along with 
            ' the init vector. 
            Dim cryptoTransform As ICryptoTransform = tdesProvider.CreateDecryptor(Me.key, Me.iv)

            ' Provide a memory stream to decrypt information into 
            Dim decryptedStream As MemoryStream = New MemoryStream()
            Dim cryptStream As CryptoStream = New CryptoStream(decryptedStream, cryptoTransform, CryptoStreamMode.Write)
            cryptStream.Write(inputInBytes, 0, inputInBytes.Length)
            cryptStream.FlushFinalBlock()
            decryptedStream.Position = 0

            ' Read the memory stream and convert it back into a string 
            Dim result(decryptedStream.Length - 1) As Byte
            decryptedStream.Read(result, 0, decryptedStream.Length)
            cryptStream.Close()
            Dim myutf As UTF8Encoding = New UTF8Encoding()
            Return myutf.GetString(result)
        End Function
        '----------------------------------------------------------------------------------------------------------
        Public Shared Function RandomPassword(ByVal minLength As Integer, ByVal minNumeric As Integer, ByVal minCapital As Integer, ByVal minSpecial As Integer, ByRef status As String) As String
            '§§§JVE 11.10.2005 <>
            'Erzeugt ein "kryptografisch sicheres" Passwort.
            'Zunächst wird eine GUID erzeugt und dann die notwendigen Großbuchstaben, Zaheln und Sonderzeichen angehängt
            '§§§JVE 11.10.2005 <>
            Dim aChar As String
            Dim aNum As String
            Dim aSpec As String
            Dim randomArray(15) As Byte
            Dim pw As String = ""
            Dim uid As Guid
            Dim sbuild As New StringBuilder()
            Dim random As New RNGCryptoServiceProvider()
            Dim nArray() As Char = {"0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c}
            Dim cArray() As Char = {"!"c, "§"c, "$"c, "%"c, "&"c, "/"c, "("c, "_"c, "="c, "*"c, "#"c, "?"c}
            Dim sArray() As Char = {"A"c, "B"c, "C"c, "D"c, "E"c, "F"c, "G"c, "H"c, "I"c, "J"c, "K"c, "L"c, "M"c, "N"c, "O"c, "P"c, "Q"c, "R"c, "S"c, "T"c, "U"c, "V"c, "W"c, "X"c, "Y"c, "Z"c}
            Dim rnd As New Random()
            Dim index As Integer
            Dim i As Integer

            status = String.Empty
            Try
                random.GetBytes(randomArray)
                uid = New Guid(randomArray)

                For i = 1 To minCapital
                    index = rnd.Next(0, 25)         'Großbuchstaben
                    aChar = sArray.GetValue(index)
                    sbuild.Append(aChar, 1)
                Next

                For i = 1 To minSpecial
                    index = rnd.Next(0, 11)         'Sonderzeichen
                    aSpec = cArray.GetValue(index)
                    sbuild.Append(aSpec, 1)
                Next

                For i = 1 To minNumeric
                    index = rnd.Next(0, 8)          'Zahlen
                    aNum = nArray.GetValue(index)
                    sbuild.Append(aNum, 1)
                Next

                pw = Left(uid.ToString, minLength) & sbuild.ToString

            Catch ex As Exception
                status = ex.Message ' "Fehler beim Generieren des Passwortes!"
            End Try

            Return pw

        End Function
    End Class
End Namespace

' ************************************************
' $History: Crypto.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 11.04.08   Time: 11:57
' Updated in $/CKAG/Base/Kernel/Security
' Migration OR
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Kernel/Security
' 
' *****************  Version 6  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Kernel/Security
' 
' ************************************************