Imports CKG.Base.Kernel

Public Enum AdressPflegeModus
    Stationen
    Spediteure
End Enum

Public Class Adresspflege
    Inherits Base.Common.SapError

    Private I_KUNNR_AG As String = ""
    Private I_WEBUSER As String = ""
    Private I_ADDRTYP As String = "ZSTO"
    Private _adressen As DataTable
    Private _modus As AdressPflegeModus = AdressPflegeModus.Stationen

    Public ReadOnly Property Adressen As DataTable
        Get
            Return _adressen
        End Get
    End Property

    Public Property Modus() As AdressPflegeModus
        Get
            Return _modus
        End Get
        Set(value As AdressPflegeModus)
            _modus = value
        End Set
    End Property

    Public Sub New(ByVal kundennummer As String, ByVal webuser As String)
        I_KUNNR_AG = kundennummer.PadLeft(10, "0"c)

        If webuser.Length > 12 Then
            I_WEBUSER = webuser.Substring(0, 12)
        Else
            I_WEBUSER = webuser
        End If
    End Sub

    Private Sub SendAdresse(type As String, stationscode As String, Optional name1 As String = "", Optional name2 As String = "",
                                 Optional strasse As String = "", Optional hausnr As String = "", Optional plz As String = "",
                                 Optional stadt As String = "", Optional land As String = "", Optional telefon As String = "", Optional fax As String = "",
                                 Optional email As String = "", Optional dadPdi As String = "")
        ClearError()

        Try
            If _modus = AdressPflegeModus.Stationen Then

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

                _adressen = S.AP.GetExportTable("GT_OUT")

                If CInt(S.AP.GetExportParameter("E_SUBRC")) <> 0 Then
                    RaiseError(S.AP.GetExportParameter("E_SUBRC").ToString, S.AP.GetExportParameter("E_MESSAGE").ToString())
                End If

            Else

                S.AP.Init("Z_DPM_CHG_SPEDITEUR_001")

                S.AP.SetImportParameter("I_KUNNR", I_KUNNR_AG)
                S.AP.SetImportParameter("I_TYPE", type)
                S.AP.SetImportParameter("I_KUNPDI", stationscode)

                Dim tblSap As DataTable = S.AP.GetImportTable("GT_WEB")

                Dim newRow As DataRow = tblSap.NewRow()
                newRow("SMTP_ADDR") = email
                newRow("DADPDI") = dadPdi
                tblSap.Rows.Add(newRow)

                S.AP.Execute()

                _adressen = S.AP.GetExportTable("GT_WEB")

            End If

        Catch ex As Exception
            RaiseError("9999", ex.Message)
        End Try
    End Sub

    Public Sub InsertAdresse(stationscode As String, name1 As String, name2 As String, strasse As String, hausnr As String,
                                 plz As String, stadt As String, land As String, telefon As String, fax As String, email As String)
        SendAdresse("1", stationscode, name1, name2, strasse, hausnr, plz, stadt, land, telefon, fax, email)
    End Sub

    Public Sub ChangeAdresse(stationscode As String, name1 As String, name2 As String, strasse As String, hausnr As String,
                                 plz As String, stadt As String, land As String, telefon As String, fax As String, email As String, dadPdi As String)
        SendAdresse("2", stationscode, name1, name2, strasse, hausnr, plz, stadt, land, telefon, fax, email, dadPdi)
    End Sub

    Public Function CheckStationExists(stationscode As String) As Boolean
        SendAdresse("1", stationscode)

        Return (ErrorCode = "101" OrElse (Modus = AdressPflegeModus.Spediteure AndAlso _adressen.Rows.Count > 0))
    End Function

End Class
