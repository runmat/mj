Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Namespace Treuhand
    Partial Public Class Change02Aut
        Inherits System.Web.UI.Page
#Region "Declarations"
        Private m_User As Base.Kernel.Security.User
        Private m_App As Base.Kernel.Security.App
        Private CustomerObject As SperreFreigabe
        Private logApp As Base.Kernel.Logging.Trace
#End Region


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            m_User = GetUser(Me) ' füllen Form.Session("objUser"), rückgabe eines UserObjekte
            FormAuth(Me, m_User)
            m_App = New Base.Kernel.Security.App(m_User) 'erzeugt ein App_objekt 
            ucHeader.InitUser(m_User)
            GetAppIDFromQueryString(Me) ' füllen page.Session("AppID")

            lblHead.Text = m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString
            ucStyles.TitleText = lblHead.Text

            Dim sAut As String = ""
            If Not Request.QueryString("Aut") = Nothing Then
                If Request.QueryString("Aut").ToString = "@!" Then
                    sAut = Request.QueryString("Aut").ToString()
                End If
            End If
            If Not IsPostBack Then
                FillAutData()
            End If
        End Sub
        Private Sub FillAutData()

            Dim OutPutStream As System.IO.MemoryStream
            Dim formatter As New BinaryFormatter()

            'cmdDelete.Visible = False
            cmdSave.Visible = False
            If (Not Session("Authorization") Is Nothing) AndAlso CBool(Session("Authorization")) AndAlso _
                (Not Session("AuthorizationID") Is Nothing) AndAlso IsNumeric(Session("AuthorizationID")) Then
                OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "CustomerObject")
                If OutPutStream Is Nothing Then
                    lblError.Text = "Keine Daten für den Vorgang vorhanden."
                Else


                    OutPutStream = GiveAuthorizationDetails(m_App.Connectionstring, CInt(Session("AuthorizationID")), "CustomerObject")
                    If OutPutStream Is Nothing Then
                        lblError.Text = "Keine Daten für den Vorgang vorhanden."
                    Else
                        cmdSave.Visible = True
                        formatter = New BinaryFormatter()
                        CustomerObject = New SperreFreigabe(m_User, m_App, Session.SessionID.ToString, Session("AppID").ToString, "")
                        CustomerObject = DirectCast(formatter.Deserialize(OutPutStream), SperreFreigabe)

                        lblRefShow.Text = CustomerObject.ReferenceforAut
                        Dim dRows() As DataRow
                        dRows = CustomerObject.tblUpload.Select("EQUI_KEY = '" & CustomerObject.ReferenceforAut & "'")
                        If dRows.Length > 0 Then
                            For Each ResultRow As DataRow In dRows
                                lblTreunehmShow.Text = CustomerObject.Treunehmer
                                lblRefShow.Text = CustomerObject.ReferenceforAut
                                lblDatumShow.Text = ResultRow("ERDAT").ToString
                                lblSachbShow.Text = ResultRow("ERNAM").ToString
                                lbl_SperrDatShow.Text = ResultRow("SPERRDAT").ToString
                                If ResultRow("TREUH_VGA").ToString = "S" Then
                                    lblAktionShow.Text = "Sperren"
                                Else
                                    lblAktionShow.Text = "Entsperren"
                                End If

                            Next
                        End If
                        Session("CustomerObject") = CustomerObject
                    End If
                End If
            Else
                lblError.Text = "Fehler bei der Datenübergabe."
            End If
        End Sub

        Private Sub WriteLog(ByVal strMessage As String, ByVal strHaendler As String, Optional ByVal strType As String = "APP")
            logApp.WriteEntry(strType, m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, Right(strHaendler, 5), strMessage, m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 10)
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
            SetEndASPXAccess(Me)
        End Sub

        Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            SetEndASPXAccess(Me)
        End Sub

        Protected Sub cmdSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSave.Click
            If Not Session("CustomerObject") Is Nothing Then
                CustomerObject = CType(Session("CustomerObject"), SperreFreigabe)
                CustomerObject.AutorizeCar(Session("AppID").ToString, Session.SessionID.ToString)
                If CustomerObject.Status < 0 Then
                    lblError.Text = CustomerObject.Message
                Else
                    Dim logApp As New Base.Kernel.Logging.Trace(m_App.Connectionstring, m_App.SaveLogAccessSAP, m_App.LogLevel)

                    logApp.CollectDetails("Vertrags. - / Fahrgestellnr.", CType(lblBELNR.Text, Object), True)
                    logApp.CollectDetails("Treunehmer", CType(lblTreunehmShow.Text, Object))
                    logApp.CollectDetails("Sachbearbeiter", CType(lblSachbShow.Text, Object))
                    logApp.CollectDetails("Sperrdatum", CType(lbl_SperrDatShow.Text, Object))
                    logApp.CollectDetails("Datum", CType(lblDatumShow.Text, Object))
                    If lblAktionShow.Text = "Sperren" Then
                        logApp.CollectDetails("Sperren", "X")
                        logApp.CollectDetails("Entsperren", "")
                    Else
                        logApp.CollectDetails("Sperren", "")
                        logApp.CollectDetails("Entsperren", "X")
                    End If
                    If lblAktionShow.Text = "Sperren" Then
                        logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, lblTreunehmShow.Text, "Sperrung für Treunehmer " & lblTreunehmShow.Text & " erfolgreich autorisiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                    Else
                        logApp.WriteEntry("APP", m_User.UserName, Session.SessionID, CInt(Session("AppID")), m_User.Applications.Select("AppID = '" & Session("AppID").ToString & "'")(0)("AppFriendlyName").ToString, lblTreunehmShow.Text, "Entsperrung für Treunehmer " & lblTreunehmShow.Text & " erfolgreich autorisiert.", m_User.CustomerName, m_User.Customer.CustomerId, m_User.IsTestUser, 0, logApp.InputDetails)
                    End If

                    cmdSave.Visible = False
                    Session("CustomerObject") = CustomerObject
                    DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
                    cmdSave.Enabled = False
                    cmdDel.Enabled = False
                    lblNoData.Visible = True
                    Dim ErrCount As Integer = 0
                    ErrCount = CustomerObject.Fahrzeuge.Select("SUBRC <> 0").Length

                    If ErrCount > 0 Then
                        lblNoData.Text &= "<br /> " & ErrCount & " Fahrzeuge mit Fehler!"
                    Else
                        lblNoData.Text = "Der Vorgang wurde erfolgreich autorisiert!"
                    End If
                    lblNoData.Visible = True
                    Session("CustomerObject") = CustomerObject
                End If
            End If
            lb_zurueck.Visible = True
        End Sub

        Protected Sub lb_zurueck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lb_zurueck.Click
            Response.Redirect("Autorisierung.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Autorisierung'")(0).Item("AppID")) & "&Aut=@!", False)
        End Sub

        Protected Sub cmdDel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDel.Click
            Try
                DeleteAuthorizationEntry(m_App.Connectionstring, CInt(Session("AuthorizationID")))
                lblNoData.Text = "<b>Daten wurden aus Autorisierung entfernt.</b><br>&nbsp;"
                Session("Authorization") = Nothing
                Session("AuthorizationID") = Nothing
                cmdSave.Visible = False
                cmdDel.Visible = False
                lb_zurueck.Visible = False

                'zurueck zur Liste oder Hauptmenue
                Dim strLastRecord As String = CStr(Request.QueryString("LastRecord"))
                If Not strLastRecord = "True" Then
                    Response.Redirect("Autorisierung.aspx?AppID=" & CStr(m_User.Applications.Select("AppName='Autorisierung'")(0).Item("AppID")) & "&Aut=@!", False)
                Else
                    Response.Redirect("../../../Start/Selection.aspx", False)
                End If
            Catch ex As Exception
                WriteLog("Fehler bei der Löschung des Vorgangs für", lblTreunehmShow.Text & "/" & lblRefShow.Text, "ERR")
                lblError.Text = "Beim Speichern Ihrer Daten ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"

            End Try
        End Sub
    End Class
End Namespace
