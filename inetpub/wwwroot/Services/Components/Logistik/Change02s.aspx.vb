Imports CKG.Base.Kernel.Security
Imports CKG.Base.Kernel.Common
Imports System.Drawing

Partial Public Class Change02s
    Inherits Page
    Implements ITransferPage
    Private _app As App
    Private _user As User

    Const ToggleScript As String = "function OnStepChanged(index) {{" &
                                   "$(""img[id^='toggleheader_']"").attr('src', '{0}');" &
                                   "$(""#toggleheader_"" + index).attr('src', '{1}');}}"

    Public ReadOnly Property CSKUser As User Implements ITransferPage.CSKUser
        Get
            Return _user
        End Get
    End Property

    Public ReadOnly Property CSKApp As App Implements ITransferPage.CSKApp
        Get
            Return _app
        End Get
    End Property

    Public ReadOnly Property Transfer As Transfer Implements ITransferPage.Transfer
        Get
            Dim result = DirectCast(Session("Transfer"), Transfer)
            If result Is Nothing Then
                InitClass()
                result = DirectCast(Session("Transfer"), Transfer)
            End If
            Return result
        End Get
    End Property

    Public ReadOnly Property Dal As TransferDal Implements ITransferPage.Dal
        Get
            Dim sDal = DirectCast(Session("TransferDal"), TransferDal)

            If sDal Is Nothing Then
                sDal = New TransferDal(Transfer)
                Session("TransferDal") = sDal
            End If

            Return sDal
        End Get
    End Property

    Protected Overrides Sub OnInit(e As EventArgs)
        MyBase.OnInit(e)

        ' Initialize all wizard steps
        InitWizardSteps()

        ' Register client scripts 
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ClientScripts",
                                                    String.Format(ToggleScript,
                                                                  Page.ResolveClientUrl(
                                                                      "~/Images/Zulassung/toggleDown.png"),
                                                                  Page.ResolveClientUrl(
                                                                      "~/Images/Zulassung/toggleUp.png")), True)
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        MyBase.OnLoad(e)

        ' Remove Session-Object, if of wrong type
        If Not Session("Transfer") Is Nothing AndAlso Not TypeOf Session("Transfer") Is Transfer Then
            Session.Remove("Transfer")
            Session.Remove("TransferDal")
        End If

        _user = Common.GetUser(Me)
        Common.FormAuth(Me, _user)
        _app = New App(_user)
        Common.GetAppIDFromQueryString(Me)

        Dim keepAdress = Session("KeepAdress") IsNot Nothing AndAlso Session("KeepAdress") = "1"
        Session.Remove("KeepAdress")

        If Not keepAdress AndAlso Not IsPostBack Then
            Session("Transfer") = Nothing
            Session("TransferDal") = Nothing
            InitClass()
            FillTables()
        Else
            Dim fahrten = TryCast(wizardControl.CurrentStep.Content, WSFahrten)
            If (fahrten IsNot Nothing) Then
                Dim requestParams = Request.Params.AllKeys
                ' Check whether ibKM ("Berechnung der Entfernungskilometer.") was clicked
                If _
                    (requestParams.Any(
                        Function(p) _
                                          Not String.IsNullOrEmpty(p) AndAlso
                                          (p.EndsWith("ibKM.x") OrElse p.EndsWith("ibKM.y")))) Then
                    fahrten.Save()
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        MyBase.OnPreRender(e)

        Common.SetEndASPXAccess(Me)
    End Sub

    Private Sub InitClass()

        Dim ct As New Transfer(_user, _app, "", "", "")

        ct.InitTables(_user, Page)
        ct.FillPartner(_user, Page, GetKundennummer())

        Session.Add("Transfer", ct)
    End Sub

    Private Sub InitWizardSteps()
        For Each [step] In wizardControl.Steps
            Dim wizardStep = TryCast([step].Content, IWizardStep)

            If wizardStep IsNot Nothing Then
                AddHandler wizardStep.Completed, AddressOf OnWizardStepCompleted
                AddHandler wizardStep.NavigateBack, AddressOf OnWizardNavigateBack
            End If
        Next
    End Sub

    Private Sub FillTables()
        Dim ct = Transfer

        ct.FillAdressen(_user, Page, "Z_WEB_UEB_LAND", "", "", "", GetKundennummer())
        ct.getProtokollarten(_user, Page, GetKundennummer())
        ct.getLaender(Page)
        ct.FillTables(_user, Page, GetKundennummer())
    End Sub

    Private Sub OnWizardNavigateBack(sender As Object, e As EventArgs)
        ' Step backward
        wizardControl.SelectedIndex = wizardControl.SelectedIndex - 1
    End Sub

    Private Sub OnWizardStepCompleted(sender As Object, e As EventArgs)
        If wizardControl.SelectedIndex = wizardControl.StepCount - 1 Then
            lblError.Visible = True

            'Prüfen, ob alle hochgeladenen Dateien da sind
            If Not Dal.ValidateUploads() Then
                lblError.Text = "Der Upload einer oder mehrerer Dateien war nicht erfolgreich. Bitte laden Sie diese ggf. erneut hoch."
                Exit Sub
            End If

            Dim errorMessages = Dal.Submit(CSKUser, Me, GetKundennummer())
            If errorMessages.Count = 0 Then
                Dim alleProtokolle As New List(Of Protokoll)
                Dim kunnr = GetKundennummer()

                If Dal.Fzg1.Protokolle IsNot Nothing Then
                    alleProtokolle.AddRange(Dal.Fzg1.Protokolle)
                End If
                If Dal.Fzg2 IsNot Nothing AndAlso Dal.Fzg2.Protokolle IsNot Nothing Then
                    alleProtokolle.AddRange(Dal.Fzg2.Protokolle)
                End If

                For Each r As DataRow In Transfer.ReturnTable.Rows
                    Dim vbeln = CStr(r("VBELN"))
                    Dim fahrt = CStr(r("Fahrt"))

                    Dim protokolle = alleProtokolle.Where(Function(p) p.Fahrt = fahrt).ToList
                    protokolle.ForEach(Sub(p) p.Transfer(vbeln, kunnr))
                Next
                lblError.Text = "Ihr Auftrag wurde erfolgreich übermittelt."
                lblError.ForeColor = Color.Green

                wizardControl.IsDone = True
                Dim final = DirectCast(wizardControl.Steps.Last().Content, WSÜbersicht)
                If Not final Is Nothing Then
                    final.IsWizardComplete = True
                End If
            Else
                lblError.Text = "Ihr Auftrag konnte nicht gespeichert werden.<br />" &
                                String.Join("<br />", errorMessages.ToArray)
            End If
        Else
            ' Step forward
            wizardControl.SelectedIndex = wizardControl.SelectedIndex + 1
        End If
    End Sub

    Private Function GetKundennummer() As String Implements ITransferPage.GetKundennummer
        Dim kundennummer As String = _user.KUNNR

        If _user.Store = "AUTOHAUS" AndAlso _user.Reference.Trim(" "c).Length > 0 AndAlso _user.KUNNR = "261510" Then
            kundennummer = _user.Reference
        End If

        Return kundennummer
    End Function
End Class