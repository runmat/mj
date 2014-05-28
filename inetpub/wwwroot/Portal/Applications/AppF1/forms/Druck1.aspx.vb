Imports CKG.Base.Business
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG

Partial Public Class Druck1
    Inherits System.Web.UI.Page

    Private m_tblKontingente As DataTable
    Private objFDDBank2 As F1_Bank_2
    Private objFDDBank As F1_BankBase
    Private objSuche As Search
    Private m_User As Base.Kernel.Security.User

    Public Property Kontingente() As DataTable
        Get
            Return m_tblKontingente
        End Get
        Set(ByVal Value As DataTable)
            Dim strTemp As String = ""

            m_tblKontingente = Value
            If CType(Session("AppShowNot"), Boolean) = True Then
                DataGrid2.Visible = False
            Else

                If (Not (m_tblKontingente Is Nothing)) AndAlso (Not (m_tblKontingente.Rows.Count = 0)) Then

                    DataGrid2.DataSource = m_tblKontingente
                    DataGrid2.DataBind()

                    '            Dim intKreditlimit As Int32
                    '            Dim intAusschoepfung As Int32
                    '            Dim blnGesperrt As Boolean

                    '            Dim item As DataGridItem
                    '            Dim cell As TableCell
                    '            Dim chkBox As CheckBox
                    '            Dim label As label
                    '            Dim control As control

                    '            For Each item In DataGrid1.Items                    'Zeilen des DataGrids durchgehen...
                    '                Dim blnZeigeKontingent As Boolean
                    '                cell = item.Cells(0)                            'Erste Spalte holen = CheckBox (nicht sichtbar)
                    '                For Each control In cell.Controls               'bestimmen, ob die jew. Kontingentart gezeigt werden soll...
                    '                    If TypeOf control Is CheckBox Then
                    '                        chkBox = CType(control, CheckBox)
                    '                        blnZeigeKontingent = chkBox.Checked     'Die checkBox wird bereits durch DataBinding gesetzt, in Abhängigkeit des Wertes in "ZeigeKontingentArt"!
                    '                    End If
                    '                Next
                    '                cell = item.Cells(2)                            'Spalte Kontingent
                    '                For Each control In cell.Controls
                    '                    If TypeOf control Is label Then
                    '                        label = CType(control, label)
                    '                        If label.ID = "Label1" And blnZeigeKontingent Then      'Label1 = nimmt Wert für Kontingent_Alt auf
                    '                            label.Visible = True
                    '                            intKreditlimit = CInt(label.Text)
                    '                        Else
                    '                            label.Visible = False
                    '                            If label.ID = "Label2" And (Not blnZeigeKontingent) Then    'Label2 = nimmt Wert für Richtwert_Alt auf
                    '                                intKreditlimit = CInt(label.Text)
                    '                                If UserReferenz.Length = 0 Then
                    '                                    label.Visible = True
                    '                                Else
                    '                                    label.Visible = False
                    '                                    If intKreditlimit = 0 Then
                    '                                        item.Visible = False
                    '                                    End If
                    '                                End If
                    '                            Else
                    '                                label.Visible = False
                    '                            End If
                    '                        End If
                    '                    End If
                    '                Next
                    '                intAusschoepfung = CInt(item.Cells(3).Text)
                    '                cell = item.Cells(4)
                    '                For Each control In cell.Controls
                    '                    If TypeOf control Is label Then
                    '                        label = CType(control, label)
                    '                        If label.ID = "Label3" And blnZeigeKontingent Then
                    '                            label.Visible = True
                    '                        Else
                    '                            label.Visible = False
                    '                        End If
                    '                    End If
                    '                Next

                    '                cell = item.Cells(5)
                    '                For Each control In cell.Controls
                    '                    If TypeOf control Is CheckBox Then
                    '                        chkBox = CType(control, CheckBox)
                    '                        blnGesperrt = chkBox.Checked
                    '                        If blnZeigeKontingent Then
                    '                            chkBox.Visible = True
                    '                        Else
                    '                            chkBox.Visible = False
                    '                        End If
                    '                    End If
                    '                Next

                    '                If blnGesperrt Or (intAusschoepfung > intKreditlimit) And blnZeigeKontingent Then
                    '                    For Each cell In item.Cells
                    '                        cell.ForeColor = System.Drawing.Color.Red
                    '                    Next
                    '                End If
                    '            Next
                    '        End If

                    '        lblMessage.Text = m_strMessage
                    '        If Not strTemp.Length = 0 Then
                    '            If Not lblMessage.Text.Length = 0 Then
                    '                lblMessage.Text &= "<br>"
                    '            End If
                    '            lblMessage.Text &= strTemp
                End If
            End If
        End Set
    End Property



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Hier Benutzercode zur Seiteninitialisierung einfügen
        m_User = GetUser(Me)
        FormAuth(Me, m_User)

        objFDDBank2 = CType(Session("objFDDBank2"), F1_Bank_2)
        objFDDBank = CType(Session("objFDDBank"), F1_BankBase)
        objSuche = CType(Session("objSuche"), Search)

        Kontingente = objSuche.Kontingente



        'txtA001.Text = objFDDBank.Kontingente.Rows(0)("RECART").ToString
        'txtA002.Text = objFDDBank.Kontingente.Rows(1)("KLIMK").ToString
        'txtK001.Text = objFDDBank.Kontingente.Rows(0)("SKFOR").ToString
        'txtK002.Text = objFDDBank.Kontingente.Rows(1)("FREIKONTI").ToString
        'txtI001.Text = objFDDBank.Kontingente.Rows(0)("Ausschoepfung").ToString
        'txtI002.Text = objFDDBank.Kontingente.Rows(1)("Ausschoepfung").ToString
        'txtF001.Text = objFDDBank.Kontingente.Rows(0)("Frei").ToString
        'txtF002.Text = objFDDBank.Kontingente.Rows(1)("Frei").ToString

        'txtA002.Text = objFDDBank.Kontingente.Rows(1)("Kontingentart").ToString
        'txtK001.Text = objFDDBank.Kontingente.Rows(0)("Kontingent_Neu").ToString
        'txtK002.Text = objFDDBank.Kontingente.Rows(1)("Kontingent_Neu").ToString
        'txtI001.Text = objFDDBank.Kontingente.Rows(0)("Ausschoepfung").ToString
        'txtI002.Text = objFDDBank.Kontingente.Rows(1)("Ausschoepfung").ToString
        'txtF001.Text = objFDDBank.Kontingente.Rows(0)("Frei").ToString
        'txtF002.Text = objFDDBank.Kontingente.Rows(1)("Frei").ToString

        'If objFDDBank.Kontingente.Rows.Count = 4 Then
        '    If objFDDBank.Kontingente.Rows(3)("Kreditkontrollbereich").ToString = "0004" Then
        '        txtF004.Text = objFDDBank.Kontingente.Rows(3)("Frei").ToString
        '        txtI004.Text = objFDDBank.Kontingente.Rows(3)("Ausschoepfung").ToString
        '        txtK004.Text = objFDDBank.Kontingente.Rows(3)("Richtwert_Neu").ToString
        '        txtA004.Text = objFDDBank.Kontingente.Rows(3)("Kontingentart").ToString
        '    End If
        '    'HEZ
        '    If objFDDBank.Kontingente.Rows(3)("Kreditkontrollbereich").ToString = "0005" Then
        '        txtF005.Text = objFDDBank.Kontingente.Rows(3)("Frei").ToString
        '        txtI005.Text = objFDDBank.Kontingente.Rows(3)("Ausschoepfung").ToString
        '        txtK005.Text = objFDDBank.Kontingente.Rows(3)("Richtwert_Neu").ToString
        '        txtA005.Text = objFDDBank.Kontingente.Rows(3)("Kontingentart").ToString
        '    End If
        'End If
        'If objFDDBank.Kontingente.Rows.Count = 5 Then
        '    'DP
        '    txtF004.Text = objFDDBank.Kontingente.Rows(3)("Frei").ToString
        '    txtI004.Text = objFDDBank.Kontingente.Rows(3)("Ausschoepfung").ToString
        '    txtK004.Text = objFDDBank.Kontingente.Rows(3)("Richtwert_Neu").ToString
        '    txtA004.Text = objFDDBank.Kontingente.Rows(3)("Kontingentart").ToString
        '    'HEZ
        '    txtI005.Text = objFDDBank.Kontingente.Rows(4)("Ausschoepfung").ToString
        '    txtF005.Text = objFDDBank.Kontingente.Rows(4)("Frei").ToString
        '    txtK005.Text = objFDDBank.Kontingente.Rows(4)("Richtwert_Neu").ToString
        '    txtA005.Text = objFDDBank.Kontingente.Rows(4)("Kontingentart").ToString
        'End If

        'If CType(objFDDBank.Kontingente.Rows(2)("ZeigeKontingentart"), Boolean) Then
        '    trDP.Visible = True
        'End If

        'If CType(objFDDBank.Kontingente.Rows(3)("ZeigeKontingentart"), Boolean) Then
        '    trHEZ.Visible = True
        'End If


        txtUser.Text = m_User.UserName.ToString
        txtNr.Text = objSuche.REFERENZ
        txtName.Text = objSuche.NAME
        txtAdresse.Text = objSuche.STREET & ", " & objSuche.POSTL_CODE & " " & objSuche.CITY

        Dim view As DataView
        view = objFDDBank2.Auftraege.DefaultView ' CType(Session("ResultTableRaw"), DataView)
        'view.RowFilter = "Initiator <> ''"

        'For Each row In view.Table.Rows
        '    row("ZZFAHRG") = "-" & Right(row("ZZFAHRG").ToString.Trim, 5).ToString
        'Next

        DataGrid1.DataSource = view
        DataGrid1.DataBind()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.PreRender
        SetEndASPXAccess(Me)
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
        SetEndASPXAccess(Me)
    End Sub
End Class
' ************************************************
' $History: Druck1.aspx.vb $
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 26.03.09   Time: 11:53
' Created in $/CKAG/Applications/AppF1/forms
' ITA 2675 anpassungen
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 15.05.08   Time: 14:33
' Updated in $/CKAG/Applications/AppFFE/forms
' ITA:1865
' 
' ************************************************
