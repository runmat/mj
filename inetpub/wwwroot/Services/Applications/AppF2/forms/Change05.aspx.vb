Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel.Security

Partial Public Class Change05
    Inherits System.Web.UI.Page

#Region "Declarations"
    Private m_App As App
    Private m_User As User
    Private objHaendler As Haendler
    Private objSuche As AppF2.Search
    Private m_change As Freigabe
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmdSearch.Visible = True
        m_User = GetUser(Me)
        FormAuth(Me, m_User, True)
        GetAppIDFromQueryString(Me)

        lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
        m_App = New App(m_User)
        'If Request.

        If IsPostBack = False Then
            If objHaendler Is Nothing Then
                If Session.Item("AppHaendler") Is Nothing Then
                    objHaendler = New Haendler(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.Reference, m_User.KUNNR)
                    Session.Add("AppHaendler", objHaendler)
                Else
                    If TypeOf Session.Item("AppHaendler") Is Haendler Then
                        objHaendler = CType(Session("AppHaendler"), Haendler)
                    Else
                        objHaendler = New Haendler(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, "", m_User.Reference, m_User.KUNNR)
                        Session("AppHaendler") = objHaendler
                    End If
                End If
            End If
            If objSuche Is Nothing Then
                searchHaendlerInitial()
            End If

            m_change = New Freigabe(m_User, m_App, CStr(Session("AppID")), Me.Session.SessionID, "")
            m_change.SessionID = Me.Session.SessionID
            m_change.AppID = CStr(Session("AppID"))
            Session.Add("m_change", m_change)

            'kontingenttabelle ausblenden wenn Parameter
            If Request.QueryString("HDL") = 1 Then
                Session("AppShowNot") = True
            End If
            If Request.QueryString("ART") = 1 Then
                m_change.KontingentSteuerung = "1"
            Else
                m_change.KontingentSteuerung = "0"
            End If

        Else 'wenn Postback
            If objHaendler Is Nothing Then
                objHaendler = CType(Session("AppHaendler"), Haendler)
            End If

            If m_change Is Nothing Then
                m_change = CType(Session("m_change"), Freigabe)
            End If

        End If
    End Sub

    Protected Sub cmdSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSearch.Click
        m_change.Haendlernummer = lblHaendlerDetailsNR.Text
        m_change.Fahrgestellnr = txtFahrgestellNr.Text
        m_change.Liznr = txt_LIZNR.Text
        'Kein Händler ausgewählt
        'If lblHaendlerDetailsNR.Text.Length = 0 Then

        m_change.show(Session("AppID").ToString, Session.SessionID, Me)
        If m_change.Status = 0 AndAlso Not m_change.Result Is Nothing Then
            Session("m_change") = m_change
            Response.Redirect("Change05_2.aspx?AppID=" & Session("AppID").ToString)
        ElseIf m_change.Status < 0 Then
            lblError.Text = m_change.Message
        ElseIf m_change.Result.Rows.Count <= 0 Then
            lblError.Text = "keine Daten vorhanden"
        End If



        ' Else
        '    lbSelektionZurueckSetzen.Visible = True

        '    Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
        '    m_change = New OffeneAnforderungen(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)
        '    m_change.AppID = Session("AppID").ToString
        '    m_change.CreditControlArea = "ZDAD"

        '    m_change.Haendler = Right("0000000000" & lblHaendlerDetailsNR.Text, 10)
        '    Dim strTemp As String = ""
        '    CKG.Base.Business.HelpProcedures.getAppParameters(Session("AppID").ToString, strTemp, System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
        '    Session.Add("m_change", m_change)
        '    objSuche = New AppF2.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
        '    Session("objSuche") = objSuche

        '    m_change.show(Me.Request.QueryString.Item("HEZ"), Session("AppID").ToString, Session.SessionID, Me)
        '    If IsNothing(m_change.Auftraege) OrElse m_change.Auftraege.Rows.Count = 0 Then
        '        lblError.Text = "Keine Daten zur Anzeige gefunden."
        '    Else
        '        Response.Redirect("Change03_2.aspx?AppID=" & Session("AppID").ToString & strTemp)
        '    End If


        'End If





        'If SucheHaendler1.isInUse = False Then
        '    m_change.show(Session("AppID").ToString, Session.SessionID, Me)
        '    If m_change.Status = 0 AndAlso Not m_change.Result Is Nothing Then
        '        Response.Redirect("Change45_1.aspx?AppID=" & Session("AppID").ToString)
        '    ElseIf m_change.Status < 0 Then
        '        lblError.Text = m_change.Message
        '    ElseIf m_change.Result.Rows.Count <= 0 Then
        '        lblError.Text = "keine Daten vorhanden"
        '    End If
        'ElseIf SucheHaendler1.NotInUseButinBorder Then
        '    Dim tmpHaendlernummer As String = ""
        '    tmpHaendlernummer = SucheHaendler1.giveHaendlernummer
        '    lbSelektionZurueckSetzen.Visible = True

        '    If Not tmpHaendlernummer Is Nothing Then
        '        m_change.Haendlernummer = tmpHaendlernummer
        '        m_change.show(Session("AppID").ToString, Session.SessionID, Me)
        '        If m_change.Status = 0 AndAlso Not m_change.Result Is Nothing Then
        '            Response.Redirect("Change45_1.aspx?AppID=" & Session("AppID").ToString)
        '        ElseIf m_change.Status < 0 Then
        '            lblError.Text = m_change.Message
        '        ElseIf m_change.Result.Rows.Count <= 0 Then
        '            lblError.Text = "keine Daten vorhanden"
        '        End If
        '    Else
        '        m_change.show(Session("AppID").ToString, Session.SessionID, Me)
        '        If m_change.Status = 0 AndAlso Not m_change.Result Is Nothing Then
        '            Response.Redirect("Change45_1.aspx?AppID=" & Session("AppID").ToString)
        '        ElseIf m_change.Status < 0 Then
        '            lblError.Text = m_change.Message
        '        ElseIf m_change.Result.Rows.Count <= 0 Then
        '            lblError.Text = "keine Daten vorhanden"
        '        End If
        '    End If


        'Else
        '    Dim tmpHaendlernummer As String = ""
        '    tmpHaendlernummer = SucheHaendler1.giveHaendlernummer
        '    lbSelektionZurueckSetzen.Visible = True

        '    If Not tmpHaendlernummer Is Nothing Then
        '        m_change.Haendlernummer = tmpHaendlernummer
        '        m_change.show(Session("AppID").ToString, Session.SessionID, Me)
        '        If m_change.Status = 0 AndAlso Not m_change.Result Is Nothing Then
        '            Response.Redirect("Change45_1.aspx?AppID=" & Session("AppID").ToString)
        '        ElseIf m_change.Status < 0 Then
        '            lblError.Text = m_change.Message
        '        ElseIf m_change.Result.Rows.Count <= 0 Then
        '            lblError.Text = "keine Daten vorhanden"
        '        End If

        '    End If
        'End If
    End Sub

    Protected Sub lbSelektionZurueckSetzen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbSelektionZurueckSetzen.Click

        objSuche = Nothing
        Session("objNewHaendlerSuche") = Nothing
        lbSelektionZurueckSetzen.Visible = False

        lbHaendler.Items.Clear()

        lblHaendlerDetailsName1.Text = String.Empty
        lblHaendlerDetailsName2.Text = String.Empty
        lblHaendlerDetailsNR.Text = String.Empty
        lblHaendlerDetailsOrt.Text = String.Empty
        lblHaendlerDetailsPLZ.Text = String.Empty
        lblHaendlerDetailsStrasse.Text = String.Empty


        searchHaendlerInitial()

    End Sub

    Protected Sub lbHaendler_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lbHaendler.SelectedIndexChanged
        objSuche = Session("objNewHaendlerSuche")

        objSuche.Haendler.RowFilter = "REFERENZ = '" & lbHaendler.SelectedValue & "'"
        lblHaendlerDetailsNR.Text = objSuche.Haendler.Item(0).Item("REFERENZ")
        lblHaendlerDetailsName1.Text = objSuche.Haendler.Item(0).Item("Name")
        lblHaendlerDetailsName2.Text = objSuche.Haendler.Item(0).Item("NAME_2")
        lblHaendlerDetailsStrasse.Text = objSuche.Haendler.Item(0).Item("STREET")
        lblHaendlerDetailsPLZ.Text = objSuche.Haendler.Item(0).Item("POSTL_CODE")
        lblHaendlerDetailsOrt.Text = objSuche.Haendler.Item(0).Item("CITY")
    End Sub


    Protected Sub txtName1_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtName1.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtName1.Text, "*", "%")
        Search("NAME LIKE '%" & strFilter & "%'")
    End Sub



    Private Sub txtPLZ_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPLZ.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtPLZ.Text, "*", "%")
        Search("POSTL_CODE LIKE '%" & strFilter & "%'")
    End Sub

    Protected Sub txtNummer_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtNummer.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtNummer.Text, "*", "%")
        Search("REFERENZ LIKE '%" & strFilter & "%'")
    End Sub

    Protected Sub txtName2_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtName2.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtName2.Text, "*", "%")
        Search("NAME_2 LIKE '%" & strFilter & "%'")
    End Sub

    Protected Sub txtOrt_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtOrt.TextChanged
        Dim strFilter As String
        strFilter = Replace(txtOrt.Text, "*", "%")
        Search("CITY LIKE '" & strFilter & "'")
    End Sub

    Protected Sub lbBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbBack.Click
        Response.Redirect("/Services/(S(" + Session.SessionID + "))/Start/Selection.aspx")
    End Sub


    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub


#End Region

#Region "Methods"
    Private Sub searchHaendlerInitial()


        objSuche = New AppF2.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)


        With objSuche
            .sucheHaendlerNr = txtNummer.Text
            .sucheName1 = txtName1.Text
            .sucheName2 = txtName2.Text
            .sucheOrt = txtOrt.Text
            .suchePLZ = txtPLZ.Text
        End With

        Searchhaendler()
        If lblError.Text = "" AndAlso objSuche.anzahlHaendlerTreffer > 0 AndAlso objSuche.anzahlHaendlerTreffer <= 200 Then


            'Name 2 Textbox wieder aktivieren da ja jetzt von name1 sapseitig auch in name2 gesucht wird, clientseitig nicht möglich! JJ2008.04.23
            txtName2.Enabled = True
            txtName2.BackColor = Nothing



            'werte in SearchHistoryLabels Schreiben
            lblSHistoryName1.Text = txtName1.Text
            lblSHistoryName2.Text = txtName1.Text
            '------------------------------------
            'nein weil jetzt sap seitig in name 1 und 2 gesucht wird bei eingabe in Name 1
            'lblSHistoryName2.Text = txtName2.Text
            '-----------------------------------
            lblSHistoryNR.Text = txtNummer.Text
            lblSHistoryOrt.Text = txtOrt.Text
            lblSHistoryPLZ.Text = txtPLZ.Text



            'wenn nicht * am ende dann in historyLabel Schreiben sonst * entfernen
            If Not txtNummer.Text.Trim(" "c).Length = 0 AndAlso txtNummer.Text.IndexOf("*") < txtNummer.Text.Length - 1 Then
                txtNummer.Text = ""
            Else
                txtNummer.Text = txtNummer.Text.Replace("*", "")
            End If

            If Not txtName1.Text.Trim(" "c).Length = 0 AndAlso txtName1.Text.IndexOf("*") < txtName1.Text.Length - 1 Then
                txtName1.Text = ""
            Else
                '---------------------------------------------------------------
                'der Bapi sucht bei einer übergabe von Name1 auch in Name 2, daher muss immer die Eingabe gelöscht werden, da sonst die clientseitige Selektion nicht greift, 
                'dort geht nur die suche eine Eingabe-> ein Sucharray, da ja alle nicht zutreffenden rausgeschmissen werden. JJ2008.4.23
                'txtName1.Text = txtName1.Text.Replace("*", "")
                '---------------------------------------------------------------
                txtName1.Text = ""
            End If

            If Not txtName2.Text.Trim(" "c).Length = 0 AndAlso txtName2.Text.IndexOf("*") < txtName2.Text.Length - 1 Then
                txtName2.Text = ""
            Else
                txtName2.Text = txtName2.Text.Replace("*", "")
            End If

            If Not txtPLZ.Text.Trim(" "c).Length = 0 AndAlso txtPLZ.Text.IndexOf("*") < txtPLZ.Text.Length - 1 Then
                txtPLZ.Text = ""
            Else
                txtPLZ.Text = txtPLZ.Text.Replace("*", "")
            End If


            If Not txtOrt.Text.Trim(" "c).Length = 0 AndAlso txtOrt.Text.IndexOf("*") < txtOrt.Text.Length - 1 Then
                txtOrt.Text = ""
            Else
                txtOrt.Text = txtOrt.Text.Replace("*", "")
            End If



        End If
        Session("objNewHaendlerSuche") = objSuche
    End Sub


    Private Sub Searchhaendler()
        'neue Klassse erstellen für BAPI Aufruf

        Try

            lblError.Text = ""
            lblMessage.Text = ""
            Dim intStatus As Int32
            intStatus = objSuche.LeseHaendlerForSucheHaendlerControl(Session("AppID").ToString, Session.SessionID.ToString, Me.Page)

            If Not intStatus = 0 Then
                lblError.Text = objSuche.ErrorMessage
                Exit Sub
            End If

            lbHaendler.DataSource = objSuche.Haendler
            lbHaendler.DataTextField = "DISPLAY"
            lbHaendler.DataValueField = "REFERENZ"
            lbHaendler.DataBind()

            If objSuche.anzahlHaendlerTreffer = 0 Then
                lblMessage.Text = "keine Ergebnisse gefunden"

                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False
                lbl_Info.Visible = True
            ElseIf Not objSuche.anzahlHaendlerTreffer = 0 AndAlso objSuche.Haendler Is Nothing OrElse objSuche.Haendler.Count = 0 Then
                If anySearchEntrys() = True Then
                    lblMessage.Text = "zu viele Ergebnisse gefunden (max 200 Treffer), weitere Einschränkungen benötigt "
                End If


                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbl_Info.Visible = True
            Else
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                tr_HaendlerAuswahl.Visible = True
                txtName2.Enabled = True
                txtName2.BackColor = Nothing
                'lbl_Info.Visible = False
                lblMessage.Text = ""
            End If

        Catch ex As Exception
            lblError.Text = "Es ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub


    Private Sub Search(ByVal Filter As String)

        If Session("objNewHaendlerSuche") Is Nothing Then
            objSuche = New AppF2.Search(m_App, m_User, Session.SessionID.ToString, Session("AppID").ToString)
            Session("objNewHaendlerSuche") = objSuche
            searchHaendlerInitial()
        Else
            objSuche = Session("objNewHaendlerSuche")
            If objSuche.Haendler Is Nothing Then
                searchHaendlerInitial()
            End If

        End If

        Try
            If objSuche.anzahlHaendlerTreffer = 0 Then
                lblMessage.Text = "keine Ergebnisse gefunden"

                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False

                lbl_Info.Visible = True
            ElseIf Not objSuche.anzahlHaendlerTreffer = 0 AndAlso objSuche.Haendler Is Nothing OrElse objSuche.Haendler.Count = 0 Then
                If anySearchEntrys() = True Then
                    lblMessage.Text = "zu viele Ergebnisse gefunden (max 200 Treffer), weitere Einschränkungen benötigt "
                End If


                lbHaendler.Items.Clear()
                tr_HaendlerAuswahl.Visible = False
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                lbl_Info.Visible = True
            Else
                lblErgebnissAnzahl.Text = objSuche.anzahlHaendlerTreffer
                tr_HaendlerAuswahl.Visible = True
                txtName2.Enabled = True
                txtName2.BackColor = Nothing
                'lbl_Info.Visible = False
                lblMessage.Text = ""
                objSuche.Haendler.RowFilter = Filter
                lbHaendler.DataSource = objSuche.Haendler
                lbHaendler.DataTextField = "DISPLAY"
                lbHaendler.DataValueField = "REFERENZ"
                lbHaendler.DataBind()
            End If

        Catch ex As Exception
            lblError.Text = "Es ist ein Fehler aufgetreten: " & ex.Message
        End Try
    End Sub

    Private Function anySearchEntrys() As Boolean

        If Not txtName1.Text.Trim(" "c) = "" OrElse Not txtName1.Text.Trim(" "c) = "" OrElse Not txtName2.Text.Trim(" "c) = "" OrElse Not txtOrt.Text.Trim(" "c) = "" OrElse Not txtPLZ.Text.Trim(" "c) = "" OrElse Not txtNummer.Text.Trim(" "c) = "" Then
            Return True
        Else
            Return False
        End If


    End Function

#End Region



End Class
' ************************************************
' $History: Change05.aspx.vb $
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 3.09.09    Time: 11:27
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 2.09.09    Time: 14:06
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 16.08.09   Time: 9:34
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 13.08.09   Time: 17:36
' Updated in $/CKAG2/Applications/AppF2/forms
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 13.08.09   Time: 12:58
' Created in $/CKAG2/Applications/AppF2/forms
' ITA: 3071
' 