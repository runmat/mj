Imports CKG.Base.Kernel

Namespace PageElements
    Public MustInherit Class Kopfdaten
        Inherits System.Web.UI.UserControl
        Private objUser As Security.User
        Private objApp As Security.App

        Private m_strHaendlerNummer As String
        Private m_strHaendlerName As String
        Private m_strAdresse As String
        Private m_tblKontingente As DataTable
        Private m_strMessage As String
        Private m_blnGesperrt As Boolean
        Private m_strUserReferenz As String

        Public Property UserReferenz() As String
            Get
                If Not m_strUserReferenz Is Nothing Then
                    Return m_strUserReferenz
                Else
                    Return ""
                End If
            End Get
            Set(ByVal Value As String)
                m_strUserReferenz = Value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return m_strMessage
            End Get
            Set(ByVal Value As String)
                m_strMessage = Value
                Me.lblMessage.Text = m_strMessage & "<br>"
                lblMessage.CssClass = "TextNormal"      'HEZ?
            End Set
        End Property

        Public Property MessageError() As String
            Get
                Return m_strMessage
            End Get
            Set(ByVal Value As String)
                m_strMessage = Value
                lblMessage.Text = m_strMessage & "<br>"
                lblMessage.CssClass = "TextError"
            End Set
        End Property

        Public Property HaendlerName() As String
            Get
                Return m_strHaendlerName
            End Get
            Set(ByVal Value As String)
                m_strHaendlerName = Value
                lblHaendlerName.Text = m_strHaendlerName
            End Set
        End Property

        Public Property Adresse() As String
            Get
                Return m_strAdresse
            End Get
            Set(ByVal Value As String)
                m_strAdresse = Value
                lblAdresse.Text = m_strAdresse
            End Set
        End Property

        Public Property HaendlerNummer() As String
            Get
                If Not m_strHaendlerNummer Is Nothing Then
                    Return m_strHaendlerNummer
                Else
                    Return ""
                End If
            End Get
            Set(ByVal Value As String)
                m_strHaendlerNummer = Value
                lblHaendlerNummer.Text = m_strHaendlerNummer
            End Set
        End Property

        Public Property Kontingente() As DataTable
            Get
                Return m_tblKontingente
            End Get
            Set(ByVal Value As DataTable)
                Dim strTemp As String = ""
                m_tblKontingente = Value
                m_tblKontingente.DefaultView.RowFilter = "Kreditkontrollbereich='0001'"

                If (Not (m_tblKontingente Is Nothing)) AndAlso (Not (m_tblKontingente.Rows.Count = 0)) Then
                    lblMessage.CssClass = "LabelExtraLarge"

                    DataGrid1.DataSource = m_tblKontingente.DefaultView
                    DataGrid1.DataBind()

                    Dim intKreditlimit As Int32
                    Dim intAusschoepfung As Int32
                    Dim blnGesperrt As Boolean

                    Dim item As DataGridItem
                    Dim cell As TableCell
                    Dim chkBox As CheckBox
                    Dim label As label
                    Dim control As control

                    For Each item In DataGrid1.Items                    'Zeilen des DataGrids durchgehen...
                        Dim blnZeigeKontingent As Boolean
                        cell = item.Cells(0)                            'Erste Spalte holen = CheckBox (nicht sichtbar)
                        For Each control In cell.Controls               'bestimmen, ob die jew. Kontingentart gezeigt werden soll...
                            If TypeOf control Is CheckBox Then
                                chkBox = CType(control, CheckBox)
                                blnZeigeKontingent = chkBox.Checked     'Die checkBox wird bereits durch DataBinding gesetzt, in Abhängigkeit des Wertes in "ZeigeKontingentArt"!
                            End If
                        Next
                        cell = item.Cells(2)                            'Spalte Kontingent
                        For Each control In cell.Controls
                            If TypeOf control Is label Then
                                label = CType(control, label)
                                If label.ID = "Label1" And blnZeigeKontingent Then      'Label1 = nimmt Wert für Kontingent_Alt auf
                                    label.Visible = True
                                    intKreditlimit = CInt(label.Text)
                                Else
                                    label.Visible = False
                                    If label.ID = "Label2" And (Not blnZeigeKontingent) Then    'Label2 = nimmt Wert für Richtwert_Alt auf
                                        intKreditlimit = CInt(label.Text)
                                        If UserReferenz.Length = 0 Then
                                            label.Visible = True
                                        Else
                                            label.Visible = False
                                            If intKreditlimit = 0 Then
                                                item.Visible = False
                                            End If
                                        End If
                                    Else
                                        label.Visible = False
                                    End If
                                End If
                            End If
                        Next
                        intAusschoepfung = CInt(item.Cells(3).Text)
                        cell = item.Cells(4)
                        For Each control In cell.Controls
                            If TypeOf control Is label Then
                                label = CType(control, label)
                                If label.ID = "Label3" And blnZeigeKontingent Then
                                    label.Visible = True
                                Else
                                    label.Visible = False
                                End If
                            End If
                        Next

                        cell = item.Cells(5)
                        For Each control In cell.Controls
                            If TypeOf control Is CheckBox Then
                                chkBox = CType(control, CheckBox)
                                blnGesperrt = chkBox.Checked
                                If blnZeigeKontingent Then
                                    chkBox.Visible = True
                                Else
                                    chkBox.Visible = False
                                End If
                            End If
                        Next

                        If blnGesperrt Or (intAusschoepfung > intKreditlimit) And blnZeigeKontingent Then
                            For Each cell In item.Cells
                                cell.ForeColor = System.Drawing.Color.Red
                            Next
                        End If
                    Next
                End If

                lblMessage.Text = m_strMessage
                If Not strTemp.Length = 0 Then
                    If Not lblMessage.Text.Length = 0 Then
                        lblMessage.Text &= "<br>"
                    End If
                    lblMessage.Text &= strTemp
                End If
            End Set
        End Property
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        Private Sub DataGrid1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
            Dim datCell As TableCell
            Dim chkBox As CheckBox

            For Each datCell In e.Item.Cells
                chkBox = CType(datCell.FindControl("Checkbox1"), CheckBox)
                If Not chkBox Is Nothing Then
                    chkBox.InputAttributes.Add("disabled", "disabled")
                End If

            Next
        End Sub
    End Class
End Namespace