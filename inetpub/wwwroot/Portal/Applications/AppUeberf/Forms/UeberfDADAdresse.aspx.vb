Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Partial Public Class UeberfDADAdresse
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private mUeberfDAD As UeberfDAD

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App

#End Region

#Region "Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        GetAppIDFromQueryString(Me)

        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Protected Sub btnSuchen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSuchen.Click

        lblError.Text = String.Empty

        If Replace(txtName.Text, "*", "") = "" AndAlso _
            Replace(txtPLZ.Text, "*", "") = "" AndAlso _
            Replace(txtOrt.Text, "*", "") = "" Then

            lblError.Text = "Bitte geben Sie wenigstens ein Suchkriterium an."
            Exit Sub
        End If


        If mUeberfDAD Is Nothing Then
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
        End If

        Try
            mUeberfDAD.Adresssuche(Request.QueryString("Art"), txtName.Text, _
                                               txtPLZ.Text, _
                                               txtOrt.Text)

            drpAdresse.DataSource = mUeberfDAD.Adressen

            drpAdresse.DataTextField = "Adresse"
            drpAdresse.DataValueField = "POS_KURZTEXT"

            drpAdresse.DataBind()

        Catch ex As Exception
            lblError.Text = ex.Message
        End Try




    End Sub

    Protected Sub lbGet_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbGet.Click

        If drpAdresse.SelectedItem.Text = "Auswahl" Then
            lblError.Text = "Bitte führen Sie zuerst eine Suche aus."
            Exit Sub
        End If


        If mUeberfDAD Is Nothing Then
            mUeberfDAD = DirectCast(Session("UeberfData"), UeberfDAD)
        End If

        Dim Kennung As String
        Kennung = drpAdresse.SelectedItem.Value



        Dim AdressRows() As DataRow = mUeberfDAD.Adressen.Select("POS_KURZTEXT = '" & Kennung & "'")

        Dim AdressRow As DataRow = AdressRows(0)

        With mUeberfDAD


            Select Case Request.QueryString("Art")
                Case "LNEHMER"

                    If Request.QueryString("SRC") = "RUECK" Then
                        .RAbName = AdressRow("Name1")
                        .RAbStrasse = AdressRow("STRAS")
                        .RAbPLZ = AdressRow("PSTLZ")
                        .RAbOrt = AdressRow("ORT01")
                        .RAbAnsprechpartner = AdressRow("Name2")
                        .RAbTelefon = AdressRow("TELNR")
                        .RAbMail = AdressRow("EMAIL")
                        Response.Redirect("UeberfDADEinRueck.aspx?AppID=" & Session("AppID").ToString)
                    ElseIf Request.QueryString("SRC") = "AUS" Then
                        .Leasingnehmernummer = AdressRow("POS_TEXT")
                        .Leasingnehmer = AdressRow("Name1")
                        .LeasingnehmerStrasse = AdressRow("STRAS")
                        .LeasingnehmerPLZ = AdressRow("PSTLZ")
                        .LeasingnehmerOrt = AdressRow("ORT01")
                        Response.Redirect("UeberfDADAus.aspx?AppID=" & Session("AppID").ToString)
                    Else
                        .Leasingnehmernummer = AdressRow("POS_TEXT")
                        .Leasingnehmer = AdressRow("Name1")
                        .LeasingnehmerStrasse = AdressRow("STRAS")
                        .LeasingnehmerPLZ = AdressRow("PSTLZ")
                        .LeasingnehmerOrt = AdressRow("ORT01")
                        Response.Redirect("UeberfDADZul.aspx?AppID=" & Session("AppID").ToString)
                    End If


                Case "HALTER"
                    .Halter = AdressRow("Name1")
                    .HalterStrasse = AdressRow("STRAS")
                    .HalterPLZ = AdressRow("PSTLZ")
                    .HalterOrt = AdressRow("ORT01")
                    Response.Redirect("UeberfDADZul.aspx?AppID=" & Session("AppID").ToString)
                Case "HAENDLER"

                    If Request.QueryString("SRC") = "RUECK" Then
                        .RAnName = AdressRow("Name1")
                        .RAnStrasse = AdressRow("STRAS")
                        .RAnPLZ = AdressRow("PSTLZ")
                        .RAnOrt = AdressRow("ORT01")
                        .RAnAnsprechpartner = AdressRow("Name2")
                        .RAnTelefon = AdressRow("TELNR")
                        .RAnMail = AdressRow("EMAIL")
                        Response.Redirect("UeberfDADEinRueck.aspx?AppID=" & Session("AppID").ToString)
                    Else

                        .HaendlerName1 = AdressRow("Name1")
                        .HaendlerAnsprech = AdressRow("Name2")
                        .HaendlerStrasse = AdressRow("STRAS")
                        .HaendlerPLZ = AdressRow("PSTLZ")
                        .HaendlerOrt = AdressRow("ORT01")
                        .HaendlerTelefon = AdressRow("TELNR")
                        .HaendlerMail = AdressRow("EMAIL")

                        If Request.QueryString("SRC") = "VSS" Then
                            .VssName = AdressRow("Name1")
                            .VssName = AdressRow("Name2")
                            .VssStrasse = AdressRow("STRAS")
                            .VssPLZ = AdressRow("PSTLZ")
                            .VssOrt = AdressRow("ORT01")
                            Response.Redirect("UeberfDADVss.aspx?AppID=" & Session("AppID").ToString)
                        Else
                            Response.Redirect("UeberfDADAus.aspx?AppID=" & Session("AppID").ToString)
                        End If
                    End If




                Case "AUSLIEFERUNG"
                    .EmpfName = AdressRow("Name1")
                    .EmpfAnsprechpartner = AdressRow("Name2")
                    .EmpfStrasse = AdressRow("STRAS")
                    .EmpfPLZ = AdressRow("PSTLZ")
                    .EmpfOrt = AdressRow("ORT01")
                    .EmpfTelefon = AdressRow("TELNR")
                    .EmpfMail = AdressRow("EMAIL")

                    Response.Redirect("UeberfDADAus.aspx?AppID=" & Session("AppID").ToString)

                Case "SONSTIGES"
                    .RAnName = AdressRow("Name1")
                    .RAnStrasse = AdressRow("STRAS")
                    .RAnPLZ = AdressRow("PSTLZ")
                    .RAnOrt = AdressRow("ORT01")
                    .RAnAnsprechpartner = AdressRow("Name2")
                    .RAnTelefon = AdressRow("TELNR")
                    .RAnMail = AdressRow("EMAIL")
                    Response.Redirect("UeberfDADEinRueck.aspx?AppID=" & Session("AppID").ToString)
            End Select


        End With


    End Sub

    Protected Sub lbCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCancel.Click
        Select Case Request.QueryString("Art")
            Case "LNEHMER"

                If Request.QueryString("SRC") = "RUECK" Then
                    Response.Redirect("UeberfDADEinRueck.aspx?AppID=" & Session("AppID").ToString)
                ElseIf Request.QueryString("SRC") = "AUS" Then
                    Response.Redirect("UeberfDADAus.aspx?AppID=" & Session("AppID").ToString)
                Else
                    Response.Redirect("UeberfDADZul.aspx?AppID=" & Session("AppID").ToString)
                End If

            Case "HALTER"
                Response.Redirect("UeberfDADZul.aspx?AppID=" & Session("AppID").ToString)
            Case "HAENDLER"

                If Request.QueryString("SRC") = "RUECK" Then
                    Response.Redirect("UeberfDADEinRueck.aspx?AppID=" & Session("AppID").ToString)
                Else
                    If Request.QueryString("SRC") = "VSS" Then
                        Response.Redirect("UeberfDADVss.aspx?AppID=" & Session("AppID").ToString)
                    Else
                        Response.Redirect("UeberfDADAus.aspx?AppID=" & Session("AppID").ToString)
                    End If
                End If

            Case "AUSLIEFERUNG"
                Response.Redirect("UeberfDADAus.aspx?AppID=" & Session("AppID").ToString)

            Case "SONSTIGES"
                Response.Redirect("UeberfDADEinRueck.aspx?AppID=" & Session("AppID").ToString)
        End Select
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
#End Region
End Class

' ************************************************
' $History: UeberfDADAdresse.aspx.vb $