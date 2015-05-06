Imports System
Imports KBS.KBS_BASE
Imports GeneralTools.Models

Public Class Zulassungsdienstsuche_2
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        FormAuth(Me)
        lblError.Text = ""

        Try
            If Session("ResultTable") Is Nothing Then
                lblError.Text = "Fehler: Die Seite wurde ohne Kontext aufgerufen."
            Else
                Dim tblResultTable As DataTable = CType(Session("ResultTable"), DataTable)
                If tblResultTable IsNot Nothing Then
                    If Request.QueryString("ID") Is Nothing OrElse Request.QueryString("ID").Length = 0 Then
                        lblError.Text = "Fehler: Die Seite wurde ohne Angaben zum Zulassungsdienst aufgerufen."
                    Else
                        Dim row As DataRow = tblResultTable.Select("ID = " & Request.QueryString("ID"))(0)
                        Label1.Text = row("NAME1").ToString()
                        Label2.Text = row("ANSPRECHPARTNER").ToString()
                        Label3.Text = row("NAME1").ToString()
                        Label4.Text = row("NAME2").ToString()
                        Label5.Text = row("STREET").ToString()
                        Label6.Text = row("HOUSE_NUM1").ToString()
                        Label7.Text = row("POST_CODE1").ToString()
                        Label8.Text = row("CITY1").ToString()
                        Label9.Text = row("TELE1").ToString()
                        Label10.Text = row("TELE2").ToString()
                        Label11.Text = row("TELE3").ToString()
                        Label12.Text = row("FAX_NUMBER").ToString()
                        Label13.Text = row("SMTP_ADDR").ToString()
                        Label14.Text = row("ZTXT1").ToString()
                        Label15.Text = row("ZTXT2").ToString()
                        Label16.Text = row("ZTXT3").ToString()
                        Label17.Text = row("BEMERKUNG").ToString()
                        Dim bln48h As Boolean = row("Z48H").ToString().XToBool()
                        Label18.Text = IIf(bln48h, "Ja", "Nein")
                        If bln48h Then
                            trAbwAdresse.Visible = True
                            Label21.Text = row("Z48H_NAME1").ToString()
                            Label22.Text = row("Z48H_NAME2").ToString()
                            Label23.Text = row("Z48H_STREET").ToString()
                            Label24.Text = row("Z48H_POST_CODE1").ToString()
                            Label25.Text = row("Z48H_CITY1").ToString()
                        Else
                            trAbwAdresse.Visible = False
                        End If
                        Dim lifZeit As String = row("LIFUHRBIS").ToString()
                        If Not String.IsNullOrEmpty(lifZeit) AndAlso lifZeit.Length > 3 Then
                            lifZeit = String.Format("{0}:{1}", lifZeit.Substring(0, 2), lifZeit.Substring(2, 2))
                        End If
                        Label19.Text = lifZeit
                        Dim blnNachreich As Boolean = row("NACHREICH").ToString().XToBool()
                        Label20.Text = IIf(blnNachreich, "Ja", "Nein")
                    End If
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

End Class