Imports CKG.Base.Kernel

Public Class Adresspflege
    Inherits CKG.Base.Common.SapError

    Private I_KUNNR_AG As String = ""
    Private I_WEBUSER As String = ""
    Private I_ADDRTYP As String = "ZSTO"
    Private _adressen As DataTable

    ReadOnly Property Adressen As DataTable
        Get
            Return _Adressen
        End Get
    End Property

    Public Sub New(ByVal kundennummer As String, ByVal webuser As String)
        I_KUNNR_AG = Right("0000000000" & kundennummer, 10)

        If Webuser.Length > 12 Then
            I_WEBUSER = Webuser.Substring(0, 12)
        Else
            I_WEBUSER = Webuser
        End If
    End Sub

    Private Function SendAdresse(type As String, stationscode As String, Optional name1 As String = "", Optional name2 As String = "",
                                 Optional strasse As String = "", Optional hausnr As String = "", Optional plz As String = "",
                                 Optional stadt As String = "", Optional land As String = "", Optional telefon As String = "", Optional fax As String = "",
                                 Optional email As String = "") As DataTable
        ClearError()

        Try
            S.AP.Init("Z_DPM_CHANGE_ADDR002_001")

            S.AP.SetImportParameter("I_KUNNR_AG", I_KUNNR_AG)
            S.AP.SetImportParameter("I_WEBUSER", I_WEBUSER)
            S.AP.SetImportParameter("I_TYPE", type)
            S.AP.SetImportParameter("I_ADDRTYP", I_ADDRTYP)
            S.AP.SetImportParameter("I_EX_KUNNR", stationscode)
            S.AP.SetImportParameter("I_NAME1", name1)
            S.AP.SetImportParameter("I_NAME2", name2)
            S.AP.SetImportParameter("I_STREET", strasse)
            S.AP.SetImportParameter("I_HOUSENUM1", hausnr)
            S.AP.SetImportParameter("I_POSTCODE1", plz)
            S.AP.SetImportParameter("I_CITY1", stadt)
            S.AP.SetImportParameter("I_COUNTRY", land)
            S.AP.SetImportParameter("I_TELNUMBER", telefon)
            S.AP.SetImportParameter("I_FAXNUMBER", fax)
            S.AP.SetImportParameter("I_SMTPADDR", email)

            S.AP.Execute()

            If CInt(S.AP.GetExportParameter("E_SUBRC")) <> 0 Then
                RaiseError(S.AP.GetExportParameter("E_SUBRC").ToString, S.AP.GetExportParameter("E_MESSAGE"))
            End If

            _adressen = S.AP.GetExportTable("GT_OUT")

            Return Adressen
        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try

        Return Nothing
    End Function

    Public Function InsertAdresse(stationscode As String, name1 As String, name2 As String, strasse As String, hausnr As String,
                                 plz As String, stadt As String, land As String, telefon As String, fax As String, email As String) As DataTable
        Return SendAdresse("1", stationscode, name1, name2, strasse, hausnr, plz, stadt, land, telefon, fax, email)
    End Function

    Public Function ChangeAdresse(stationscode As String, name1 As String, name2 As String, strasse As String, hausnr As String,
                                 plz As String, stadt As String, land As String, telefon As String, fax As String, email As String) As DataTable
        Return SendAdresse("2", stationscode, name1, name2, strasse, hausnr, plz, stadt, land, telefon, fax, email)
    End Function

    Public Function CheckStationExists(stationscode As String) As Boolean
        SendAdresse("1", stationscode)

        If ErrorCode = "101" Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
