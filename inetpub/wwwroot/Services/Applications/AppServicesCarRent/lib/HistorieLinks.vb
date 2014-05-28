Option Explicit On
Option Strict On
Option Infer On

Imports QuickEasy

Public Class HistorieLinks
    'Inherits DatenimportBase

    ReadOnly _fahrgestellNr As String
    ReadOnly _lagerort As String
    ReadOnly _archivName As String
    Dim _hasDocument As Boolean
    Private _filelist As String(,)

    Public Sub New(ByVal fahrgestellNr As String, ByVal archivName As String, ByVal lagerort As String)
        _fahrgestellNr = fahrgestellNr
        _lagerort = lagerort
        _archivName = archivName
        _hasDocument = False
    End Sub

    Public ReadOnly Property HasDokument As Boolean
        Get
            Return _hasDocument
        End Get
    End Property

    Public ReadOnly Property FileList As String(,)
        Get
            Return _filelist
        End Get
    End Property

    ''' <summary>
    ''' Holt eine Liste aller verfügbaren Dokumente
    ''' </summary>
    Public Function GetFileList() As String(,)

        Dim qe = New Documents(".1001=" + _fahrgestellNr,
              ConfigurationManager.AppSettings("EasyRemoteHosts").ToString(),
              60, ConfigurationManager.AppSettings("EasySessionId"),
              ConfigurationManager.AppSettings("ExcelPath").ToString(),
              "C:\\TEMP",
              "SYSTEM",
              ConfigurationManager.AppSettings("EasyPwdClear").ToString(),
              "C:\\TEMP",
              If(String.IsNullOrEmpty(_lagerort), "DCBANK", _lagerort),
              _archivName,
              "SGW")


        _filelist = qe.GetFileList()

        ' Status OK und Dateien vorhanden?
        If qe.ReturnStatus = 2 Then
            _hasDocument = True
        Else
            _hasDocument = False
        End If

        Return _filelist
    End Function

    Public Sub DownloadSingleFile(ByVal destinationLiteral As Literal, ByVal Extension As String, ByVal strDoc_id As String, ByVal strDoc_ver As String)
        Dim qe = New Documents(".1001=" + _fahrgestellNr,
              ConfigurationManager.AppSettings("EasyRemoteHosts").ToString(),
              60, ConfigurationManager.AppSettings("EasySessionId"),
              ConfigurationManager.AppSettings("ExcelPath").ToString(),
              "C:\\TEMP",
              "SYSTEM",
              ConfigurationManager.AppSettings("EasyPwdClear").ToString(),
              "C:\\TEMP",
              If(String.IsNullOrEmpty(_lagerort), "DCBANK", _lagerort),
              _archivName,
              "SGW")

        Dim path = qe.DownloadSingleFile(Extension, strDoc_id, strDoc_ver)

        Dim sb = New StringBuilder()
        sb.AppendLine(" <script language=""Javascript"">")
        sb.AppendLine(" <!-- //")

        ' Status OK und ZBII vorhanden?
        If qe.ReturnStatus = 2 Then
            _hasDocument = True
            
            HttpContext.Current.Session("App_Filepath") = path
            sb.AppendFormat(" window.open(""Report05_Formular.aspx?AppID={0}"", ""_blank"", ""left=0,top=0,resizable=YES,scrollbars=YES"");" + Environment.NewLine, HttpContext.Current.Session("AppID"))
        Else
            sb.AppendLine(" alert('Das Dokument konnte nicht gefunden werden.');")
        End If

        sb.AppendLine(" //-->")
        sb.AppendLine(" </script>")
        destinationLiteral.Text = sb.ToString()
        destinationLiteral.Visible = True
    End Sub
End Class