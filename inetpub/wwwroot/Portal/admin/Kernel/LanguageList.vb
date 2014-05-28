Namespace Kernel
Public Class LanguageList
        REM § Objekt zum Auflisten von Browsersprachen für die Administration

    Inherits DataTable

#Region " Constructor "
    Public Sub New(ByVal strConnectionString As String, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
        Me.New(New SqlClient.SqlConnection(strConnectionString), blnAll, blnNone)
    End Sub
    Public Sub New(ByVal cn As SqlClient.SqlConnection, Optional ByVal blnAll As Boolean = False, Optional ByVal blnNone As Boolean = False)
        If cn.State = ConnectionState.Closed Then
            cn.Open()
        End If
            Dim daLanguages As New SqlClient.SqlDataAdapter( _
                        "SELECT " & _
                        "[LanguageID], " & _
                        "[BrowserLanguage] + ' - ' + [LanguageDiscription] AS [Language] " & _
                        "FROM [Language] ORDER BY LanguageDiscription", cn)
            daLanguages.Fill(Me)
        AddAllNone(blnAll, blnNone)
    End Sub
#End Region

#Region " Functions "
        Public Sub AddAllNone(ByVal blnAll As Boolean, ByVal blnNone As Boolean)
            Dim dr As DataRow
            If blnAll Then
                dr = NewRow()
                dr("LanguageID") = 0
                dr("Language") = " - alle - "
                Rows.Add(dr)
            End If
            If blnNone Then
                dr = NewRow()
                dr("LanguageID") = -1
                dr("Language") = " - keine - "
                Rows.Add(dr)
            End If
        End Sub
#End Region

End Class
End Namespace

' ************************************************
' $History: LanguageList.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 14:47
' Created in $/CKAG/admin/Kernel
' 
' *****************  Version 1  *****************
' User: Uha          Date: 12.09.07   Time: 15:17
' Created in $/CKG/Admin/AdminWeb/Kernel
' ITA 1263: Pflege der Feldübersetzungen
' 
' ************************************************