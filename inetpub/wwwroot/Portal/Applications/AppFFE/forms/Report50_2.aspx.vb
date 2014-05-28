Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Business

Partial Public Class Report50_2
    Inherits System.Web.UI.Page

    Private m_User As Base.Kernel.Security.User
    Private m_App As Base.Kernel.Security.App
    Private objSuche As FFE_Search
    Private objReport50_objFDDBank As FFE_BankBase
    Private objReport50_objFDDBank3 As FFE_Bank_3
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        m_User = GetUser(Me)
        ucHeader.InitUser(m_User)
        FormAuth(Me, m_User)

        lnkKreditlimit.NavigateUrl = "Report50.aspx?AppID=" & Session("AppID").ToString & "&Back=1"
        Try
            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text
            m_App = New Base.Kernel.Security.App(m_User)

            If Not IsPostBack Then
                If CType(Session("App_Gesamt"), System.Boolean) = True Then
                    Kopfdaten1.Visible = False
                Else
                    objSuche = Session("objSuche")
                    objReport50_objFDDBank = New FFE_BankBase(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, Session("SelectedDealer").ToString)
                    objReport50_objFDDBank.Customer = Session("SelectedDealer").ToString
                    objReport50_objFDDBank.CreditControlArea = "ZDAD"
                    objReport50_objFDDBank.Show()
                    If objReport50_objFDDBank.Status = 0 Then
                        Kopfdaten1.Kontingente = objReport50_objFDDBank.Kontingente
                        Kopfdaten1.UserReferenz = m_User.Reference
                        Kopfdaten1.HaendlerNummer = objSuche.REFERENZ
                        Dim strTemp As String = objSuche.NAME
                        If objSuche.NAME_2.Length > 0 Then
                            strTemp &= "<br>" & objSuche.NAME_2
                        End If
                        Kopfdaten1.HaendlerName = strTemp
                        Kopfdaten1.Adresse = objSuche.COUNTRYISO & " - " & objSuche.POSTL_CODE & " " & objSuche.CITY & "<br>" & objSuche.STREET
                        Session("objReport50_objFDDBank") = objReport50_objFDDBank
                        cmdSearch.Enabled = True
                    Else
                        cmdSearch.Enabled = False
                    End If
                End If

            Else
                If Not Session("objReport50_objFDDBank") Is Nothing Then
                    objReport50_objFDDBank = CType(Session("objReport50_objFDDBank"), FFE_BankBase)
                    If objReport50_objFDDBank.Status = 0 Then
                        'Kopfdaten1.Kontingente = objReport50_objFDDBank.Kontingente
                        'Session("objReport50_objFDDBank") = objReport50_objFDDBank
                        cmdSearch.Enabled = True
                    Else
                        cmdSearch.Enabled = False
                    End If
                Else
                    'Response.Redirect("../../../Start/Selection.aspx")
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Beim Laden der Seite ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
        End Try
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        DoSubmit()
    End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        DoSubmit()
    End Sub

    Private Sub DoSubmit()
        Session("lnkExcel") = ""
        lblError.Text = ""
        lblError.Visible = False
        lblNoData.Text = ""
        Dim Status As Integer
        If CType(Session("App_Gesamt"), System.Boolean) = True Then
            Status = 0
        Else : Status = objReport50_objFDDBank.Status
        End If
        If Status = 0 Then


            Dim strFileName As String = Format(Now, "yyyyMMdd_HHmmss_") & m_User.UserName & ".xls"
            objReport50_objFDDBank3 = New FFE_Bank_3(m_User, m_App, Session("AppID").ToString, Session.SessionID.ToString, strFileName)

            If CType(Session("App_Gesamt"), System.Boolean) = True Then
                objReport50_objFDDBank3.GesamtBestand = True
                objReport50_objFDDBank3.Customer = ""
            Else
                objReport50_objFDDBank3.GesamtBestand = False
                objReport50_objFDDBank3.Customer = "60" & Session("SelectedDealer").ToString
                Kopfdaten1.Kontingente = objReport50_objFDDBank.Kontingente
            End If
            objReport50_objFDDBank3.CreditControlArea = "ZDAD"
            Dim strTemp As String = Replace(txtFahrgestellnummer.Text, "%", "*")
            If strTemp Is Nothing Then
                strTemp = ""
            End If
            If strTemp.Length = 5 Then
                objReport50_objFDDBank3.FahrgestellnummerSuche = "*" & strTemp
            Else
                objReport50_objFDDBank3.FahrgestellnummerSuche = strTemp
            End If
            objReport50_objFDDBank3.Ordernummer = txtOrdernummer.Text
            objReport50_objFDDBank3.Vertragsnummer = txtVertragsnummer.Text
            If chkAlle.Checked Then
                objReport50_objFDDBank3.Vorgaenge = "alle"
            End If
            If chkAngefordert.Checked Then
                objReport50_objFDDBank3.Vorgaenge = "angefordert"
            End If
            If chkNichtAngefordert.Checked Then
                objReport50_objFDDBank3.Vorgaenge = "nichtangefordert"
            End If
            objReport50_objFDDBank3.Report()
            If Not objReport50_objFDDBank3.Fahrzeuge Is Nothing AndAlso objReport50_objFDDBank3.Fahrzeuge.Rows.Count > 0 Then
                Session("objReport50_objFDDBank3") = objReport50_objFDDBank3
                Dim objExcelExport As New Excel.ExcelExport()
                Try
                    Excel.ExcelExport.WriteExcel(objReport50_objFDDBank3.FahrzeugeExcel, ConfigurationManager.AppSettings("ExcelPath") & strFileName)
                    Session("lnkExcel") = "/Portal/Temp/Excel/" & strFileName
                Catch
                End Try
                Response.Redirect("Report50_3.aspx?AppID=" & Session("AppID").ToString)


            Else
                lblError.Text = "Zu den gewählten Kriterien wurden keine unbezahlten Fahrzeuge gefunden."
                lblError.Visible = True
            End If
        Else
            lblError.Text = objReport50_objFDDBank.Message
            lblError.Visible = True
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub

End Class