Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Kernel.DocumentGeneration
Imports System.Net.Mail
Imports System.Drawing


Partial Public Class Report01s_03
    Inherits Page

#Region "Declarations"

    Private m_App As App
    Private m_User As User

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        m_User = Common.GetUser(Me)
        Common.FormAuth(Me, m_User)
        Common.GetAppIDFromQueryString(Me)
        m_App = New App(m_User)

        Try
            m_App = New App(m_User)

            If IsPostBack = False Then
                fillFields()
            End If
        Catch ex As Exception
            'lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("Report01s.aspx?AppID=" & Session("AppID").ToString)
    End Sub

    Protected Sub lbCreatePDF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreatePDF.Click
        CreatePDF("")
    End Sub

    Protected Sub lbCreateMail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCreateMail.Click
        CreatePDF("Mail")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.PreRender
        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Unload
        Common.SetEndASPXAccess(Me)
    End Sub

#End Region

#Region "Methods"

    Private Function GetAufnr() As String
        Dim sAufnr = Page.Request.Params("AUFNR").Trim()

        If Regex.IsMatch(sAufnr, "^\d+$") Then Return sAufnr

        Return String.Empty
    End Function

    Private Sub CreatePDF(ByVal art As String)
        Dim aufnr = GetAufnr()

        Dim mObjBestand = CType(Session("mObjBestand"), Bestand)
        Dim tmpRow = mObjBestand.Gesamt.Select("Auftragsnummer='" & aufnr & "'").First()
        mObjBestand.Fin = tmpRow("Fahrgestellnummer").ToString
        mObjBestand.Mitarbeiternr = tmpRow("Mitarbeiternummer").ToString
        mObjBestand.Mail = tmpRow("Email").ToString
        mObjBestand.Haltername = Replace(tmpRow("Haltername").ToString, "*X*", "")
        mObjBestand.PlzOrt = tmpRow("PLZ").ToString & "/" & tmpRow("Ort").ToString
        mObjBestand.Strasse = tmpRow("Strasse").ToString
        mObjBestand.Telefon = tmpRow("Telefon").ToString

        If tmpRow("KM_Hin_Differenz").ToString.Length > 0 Then
            mObjBestand.HinDifferenz = tmpRow("KM_Hin_Differenz").ToString
        Else
            mObjBestand.HinDifferenz = ""
        End If

        If tmpRow("KM_Rueck_Differenz").ToString.Length > 0 Then
            mObjBestand.RueckDifferenz = tmpRow("KM_Rueck_Differenz").ToString
        Else
            mObjBestand.RueckDifferenz = ""
        End If

        tmpRow = mObjBestand.Kopfdaten.Select("AUFNR='" & aufnr & "'")(0)

        mObjBestand.LvNummer = tmpRow("DEVICEID").ToString
        mObjBestand.Kennzeichen = tmpRow("LICENSE_NUM").ToString

        Dim tblKmStand = mObjBestand.GetKMStand(lblFahrgestellnummer.Text, m_User, m_App, Me)

        If tblKmStand.Rows.Count > 0 Then
            mObjBestand.HinAb = tblKmStand.Rows(0)("ZABKM_H").ToString
            mObjBestand.HinAn = tblKmStand.Rows(0)("ZANKM_H").ToString
            mObjBestand.RueckAb = tblKmStand.Rows(0)("ZABKM_R").ToString
            mObjBestand.RueckAn = tblKmStand.Rows(0)("ZANKM_R").ToString
        End If

        Dim tblTemp = FillRueckTable(aufnr)

        mObjBestand.DienstCount = lblDienstleistungen.Text

        Dim exportTable = New DataTable
        exportTable.Columns.Add("Dienstleister", GetType(String))
        exportTable.Columns.Add("Leistungsart", GetType(String))
        exportTable.Columns.Add("Art", GetType(String))
        exportTable.Columns.Add("Datum", GetType(String))
        exportTable.Columns.Add("Prognosedatum", GetType(String))
        exportTable.Columns.Add("Rückmeldetext", GetType(String))
        exportTable.AcceptChanges()

        Dim leistungsart = ""
        For Each dRow As DataRow In tblTemp.Rows

            Dim exportRow As DataRow = exportTable.NewRow

            Dim lArt As String = ""
            Dim dienstleister As String = ""

            If leistungsart <> (dRow("Dienstleister").ToString & dRow("Leistungsart").ToString) Then

                lArt = dRow("Leistungsart").ToString
                dienstleister = dRow("Dienstleister").ToString

            End If

            exportRow("Dienstleister") = dienstleister
            exportRow("Leistungsart") = lArt
            exportRow("Art") = dRow("Art").ToString
            exportRow("Datum") = CDate(dRow("RueckDatum")).ToShortDateString
            If Not DBNull.Value.Equals(dRow("Prognosedatum")) Then
                If IsDate(dRow("Prognosedatum")) Then
                    exportRow("Prognosedatum") = CDate(dRow("Prognosedatum")).ToShortDateString
                End If
            End If


            exportRow("Rückmeldetext") = dRow("RueckText").ToString

            leistungsart = dRow("Dienstleister").ToString & dRow("Leistungsart").ToString

            exportTable.Rows.Add(exportRow)

        Next

        exportTable.AcceptChanges()

        Dim dataTables(0) As DataTable

        dataTables(0) = exportTable

        Dim tblData As DataTable = DataTableHelper.ObjectToDataTable(mObjBestand)
        Dim imageHt As New Hashtable()
        Try
            imageHt.Add("Logo", m_User.Customer.LogoImage)
        Catch ex As Exception
            ' LogoPath am Customer nicht (korrekt) gepflegt - hier: ignorieren
        End Try
        Dim docFactory As New WordDocumentFactory(tblData, imageHt)

        If art = "Mail" Then
            Dim path As String = ConfigurationManager.AppSettings("ExcelPath") & "Details_" &
                                 Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName
            docFactory.CreateDocumentAndSave(path, Me, "\Components\ComCommon\documents\Details.doc", "Tabelle",
                                             dataTables)
            SendMail(path)
        Else
            docFactory.CreateDocument("Details_" & Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName, Me,
                                      "\Components\ComCommon\documents\Details.doc", "Tabelle", dataTables)
        End If
    End Sub

    Private Sub SendMail(ByVal anhang As String)
        Dim _
            clsMail As _
                New GetMailTexte(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)

        If clsMail.Status = 0 Then
            Try
                clsMail.LeseMailTexte("1")

                Dim smtpMailSender = ConfigurationManager.AppSettings("SmtpMailSender")
                Dim mailsender As New MailAddress(smtpMailSender)

                clsMail.MailBody = clsMail.MailBody.Replace("#Name#", m_User.FirstName & " " & m_User.LastName)
                clsMail.MailBody = clsMail.MailBody.Replace("#MailAdresse#", m_User.Email)

                Dim mail = New MailMessage() _
                        With {.Sender = mailsender, .From = mailsender, .Body = clsMail.MailBody, .IsBodyHtml = True,
                        .Subject = clsMail.Betreff}
                mail.To.Add(lblHalterEMail.Text)
                mail.Attachments.Add(New Attachment(anhang & ".pdf"))

                Dim smtpMailServer = ConfigurationManager.AppSettings("SmtpMailServer")
                Dim client = New SmtpClient(smtpMailServer)
                client.Send(mail)

                lblInfo.Text = "E-Mailversand erfolgreich."
            Catch ex As Exception
                lblError.Text = "Fehler beim Versenden der E-Mail."
            End Try
        End If
    End Sub


    Private Sub FillFields()
        Dim aufnr = GetAufnr()

        Dim mObjBestand As Bestand = CType(Session("mObjBestand"), Bestand)
        Dim tmpRow = mObjBestand.Gesamt.Select(String.Format("Auftragsnummer='{0}'", aufnr)).First()

        lblFahrgestellnummer.Text = tmpRow("Fahrgestellnummer").ToString
        lblKundenstatus.Text = IIf(tmpRow("ANLZU").ToString <> "0", tmpRow("ANLZU").ToString, "")

        lblMitarbeiternr.Text = tmpRow("Mitarbeiternummer").ToString
        lblHalterEMail.Text = tmpRow("Email").ToString
        lblHaltername.Text = Replace(tmpRow("Haltername").ToString, "*X*", "")
        lblHalterOrt.Text = tmpRow("Ort").ToString
        lblHalterStrasse.Text = tmpRow("Strasse").ToString
        lblHalterPLZ.Text = tmpRow("PLZ").ToString
        lblHalterTelefon.Text = tmpRow("Telefon").ToString

        tmpRow = mObjBestand.Kopfdaten.Select(String.Format("AUFNR='{0}'", aufnr))(0)

        lblLvNummer.Text = tmpRow("DEVICEID").ToString
        lblKennzeichen.Text = tmpRow("LICENSE_NUM").ToString

        Dim tblKmStand As DataTable = mObjBestand.GetKMStand(lblFahrgestellnummer.Text, m_User, m_App, Me)

        If tblKmStand.Rows.Count > 0 Then
            lblHinAbfahrt.Text = tblKmStand.Rows(0)("ZABKM_H").ToString
            lblHinAnkunft.Text = tblKmStand.Rows(0)("ZANKM_H").ToString
            lblRueckAbfahrt.Text = tblKmStand.Rows(0)("ZABKM_R").ToString
            lblRueckAnkunft.Text = tblKmStand.Rows(0)("ZANKM_R").ToString

            If lblHinAbfahrt.Text.Length > 0 AndAlso lblHinAnkunft.Text.Length > 0 Then
                lblDiffHin.Text = (CInt(lblHinAnkunft.Text) - CInt(lblHinAbfahrt.Text)).ToString
            End If

            If lblRueckAbfahrt.Text.Length > 0 AndAlso lblRueckAnkunft.Text.Length > 0 Then
                lblDiffRueck.Text = (CInt(lblRueckAnkunft.Text) - CInt(lblRueckAbfahrt.Text)).ToString
            End If

        End If

        Dim tblTemp = FillRueckTable(aufnr)

        Const tagStartTable As String = "<table style=""width: 100%;padding-top:10px;padding-bottom:10px"">"
        Const tagEndTable As String = "</table>"
        Const tagCellDienstleister As String = "<td width=""150px"">"
        Const tagCellLeistungart As String = "<td width=""150px"">"
        Const tagCellArt As String = "<td width=""120px"" style=""text-align:left"">"
        Const tagCellUrsache As String =
                  "<td style=""text-decoration: underline; font-weight: 700; color: #0000FF"" Title=""#Title#"" width=""60px"">"
        Const tagCellDatum As String = "<td width=""80px"">"
        Const tagCellPrognoseDatum As String = "<td width=""100px"">"
        Const tagCellRuecktext As String = "<td width=""160px"" style=""padding-right:20px"">"
        Const tagCellEnd As String = "</td>"
        Const tagRowStart As String = "<tr>"
        Const tagRowEnd As String = "</tr>"

        Dim ausgabeString = ""
        Dim leistungsart = ""
        Dim i As Integer = 1

        If tblTemp.Rows.Count > 6 Then
            divAusgabe.Style.Add("overflow-y", "scroll")
            divAusgabe.Style.Add("overflow-x", "hidden")
        End If

        For Each dRow As DataRow In tblTemp.Rows
            Dim lArt As String = ""
            Dim dienstleister As String = ""

            'Neue Tabelle
            If leistungsart <> (dRow("Dienstleister").ToString & dRow("Leistungsart").ToString) Then
                lArt = dRow("Leistungsart").ToString
                dienstleister = dRow("Dienstleister").ToString

                ausgabeString &= tagStartTable
            End If

            ausgabeString &= tagRowStart & vbCrLf
            ausgabeString &= tagCellDienstleister & dienstleister & tagCellEnd & vbCrLf
            ausgabeString &= tagCellLeistungart & lArt & tagCellEnd & vbCrLf
            ausgabeString &= tagCellArt & dRow("Art").ToString & tagCellEnd & vbCrLf

            If dRow("UrsacheText").ToString.Length > 0 Then
                ausgabeString &= tagCellUrsache.Replace("#Title#", dRow("UrsacheText").ToString)
            Else
                ausgabeString &= tagCellUrsache.Replace("#Title#", "")
            End If

            ausgabeString &= dRow("UrsacheGrund").ToString & tagCellEnd & vbCrLf
            ausgabeString &= tagCellDatum & CDate(dRow("RueckDatum")).ToShortDateString & tagCellEnd & vbCrLf
            If Not DBNull.Value.Equals(dRow("Prognosedatum")) Then

                If IsDate(dRow("Prognosedatum")) Then
                    ausgabeString &= tagCellPrognoseDatum & CDate(dRow("Prognosedatum")).ToShortDateString & tagCellEnd &
                                     vbCrLf
                Else
                    ausgabeString &= tagCellPrognoseDatum & " " & tagCellEnd & vbCrLf
                End If

            Else
                ausgabeString &= tagCellPrognoseDatum & " " & tagCellEnd & vbCrLf
            End If

            ausgabeString &= tagCellRuecktext & dRow("RueckText").ToString & tagCellEnd & vbCrLf
            ausgabeString &= tagRowEnd & vbCrLf

            If i < tblTemp.Rows.Count Then
                If (dRow("Dienstleister").ToString & dRow("Leistungsart").ToString) <>
                   (tblTemp.Rows(i)("Dienstleister").ToString & tblTemp.Rows(i)("Leistungsart").ToString) Then
                    ausgabeString &= tagEndTable & vbCrLf
                    If i <> tblTemp.Rows.Count Then
                        ausgabeString &= "<hr />" & vbCrLf
                    End If
                End If
            Else
                ausgabeString &= tagEndTable & vbCrLf
                If i <> tblTemp.Rows.Count Then
                    ausgabeString &= "<hr />" & vbCrLf
                End If
            End If

            leistungsart = dRow("Dienstleister").ToString & dRow("Leistungsart").ToString

            i += 1
        Next

        litRueck.Text = ausgabeString
    End Sub

    Private Function FillRueckTable(aufnr As String) As DataTable
        Dim mObjBestand As Bestand = CType(Session("mObjBestand"), Bestand)

        Dim tblTemp As New DataTable
        tblTemp.Columns.Add("Zaehl", GetType(Int16))
        tblTemp.Columns.Add("Dienstleister", GetType(String))
        tblTemp.Columns.Add("Leistungsart", GetType(String))
        tblTemp.Columns.Add("Art", GetType(String))
        tblTemp.Columns.Add("UrsacheGrund", GetType(String))
        tblTemp.Columns.Add("UrsacheText", GetType(String))
        tblTemp.Columns.Add("RueckDatum", GetType(String))
        tblTemp.Columns.Add("Prognosedatum", GetType(String))
        tblTemp.Columns.Add("RueckText", GetType(String))

        Dim countErledigt As Integer = 0
        Dim strSelectionArbeitsplatz As String = ""

        If Not mObjBestand.ArbeitsplatzUser = "showAll" Then
            strSelectionArbeitsplatz = "AND ARBPL='" & mObjBestand.ArbeitsplatzUser & "'"
        End If


        For Each dRow As DataRow In mObjBestand.ArbeitsplanDatumswerte.Select("AUFNR='" & aufnr & "'")
            Dim grund As String = "X"
            If dRow("GRUND").ToString.Length > 0 Then
                grund = dRow("GRUND")
            End If

            Dim checkRow =
                    mObjBestand.Leistungsart.Select(
                        "LARNT= '" & dRow("LEARR").ToString & "' " & strSelectionArbeitsplatz)
            If checkRow.Length > 0 Then
                Dim tempRow As DataRow = tblTemp.NewRow
                Dim apRow As DataRow =
                        mObjBestand.Leistungsart.Select(
                            "LARNT='" & dRow("LEARR").ToString & "' " & strSelectionArbeitsplatz)(0)
                Dim rowUrsache As DataRow() = mObjBestand.tblAbweichendeUrsachen.Select("GRUND='" & grund & "'")

                tempRow("Zaehl") = CInt(apRow("ZAEHL"))
                tempRow("Dienstleister") = apRow("KTEXT_AP")
                tempRow("Leistungsart") = apRow("LTXA1")

                If rowUrsache.Length > 0 Then
                    tempRow("UrsacheGrund") = rowUrsache(0)("GRUND")
                    tempRow("UrsacheText") = rowUrsache(0)("GRDTX")
                End If

                tempRow("RueckDatum") = dRow("IEDD")
                If Not DBNull.Value.Equals(dRow("PEDD")) Then
                    tempRow("Prognosedatum") = dRow("PEDD")
                End If

                tempRow("RueckText") = dRow("LTXA1")

                If dRow("AUERU").ToString.ToUpper = "X" Then
                    tempRow("Art") = "Erledigt"
                Else
                    tempRow("Art") = "Teilmeldung"
                End If

                tblTemp.Rows.Add(tempRow)
                tblTemp.AcceptChanges()
            End If

            If dRow("AUERU").ToString.ToUpper = "X" Then
                countErledigt += 1
            End If
        Next

        tblTemp.DefaultView.Sort = "ZAEHL ASC, RueckDatum ASC"

        Dim tblAusgabe = tblTemp.DefaultView.ToTable

        lblDienstleistungen.Text = String.Format("{0} von {1} Dienstleistungen erledigt.", countErledigt,
                                                 mObjBestand.Leistungsart.Rows.Count)

        Dim percentResult = CInt(((countErledigt * 100) / mObjBestand.Leistungsart.Rows.Count))

        If percentResult < 60 Then
            lblDienstleistungen.ForeColor = Color.Red
        ElseIf percentResult >= 60 AndAlso percentResult < 100 Then
            lblDienstleistungen.ForeColor = Color.Orange
        Else
            lblDienstleistungen.ForeColor = Color.Green
        End If

        Return tblAusgabe
    End Function

#End Region
End Class


' ************************************************
' $History: Report01s_03.aspx.vb $
' 
' *****************  Version 13  *****************
' User: Fassbenders  Date: 13.05.11   Time: 16:10
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 12  *****************
' User: Fassbenders  Date: 13.05.11   Time: 10:14
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 23.07.10   Time: 16:20
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 15.07.10   Time: 13:56
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 15.07.10   Time: 12:49
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 8  *****************
' User: Fassbenders  Date: 14.07.10   Time: 16:24
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 7  *****************
' User: Fassbenders  Date: 13.07.10   Time: 13:54
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 6  *****************
' User: Fassbenders  Date: 9.07.10    Time: 11:23
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 8.07.10    Time: 13:21
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 5.07.10    Time: 17:06
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 16.06.10   Time: 14:03
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 15.06.10   Time: 12:59
' Updated in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA: 3829
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 11.06.10   Time: 15:13
' Created in $/CKAG2/Services/Components/ComCommon/Leasing
' ITA: 3829
' 