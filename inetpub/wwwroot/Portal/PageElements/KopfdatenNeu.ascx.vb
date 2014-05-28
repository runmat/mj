Imports CKG.Base.Kernel

Namespace PageElements
    Public MustInherit Class KopfdatenNeu
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

        Protected WithEvents lblAdresse As System.Web.UI.WebControls.Label
        Protected WithEvents lblHaendlerName As System.Web.UI.WebControls.Label
        Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
        Protected WithEvents lblHaendlerNummer As System.Web.UI.WebControls.Label
        Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
        Protected WithEvents lbl_HaendlerNummer As System.Web.UI.WebControls.Label
        Protected WithEvents DataGrid1 As System.Web.UI.WebControls.DataGrid

#Region " Vom Web Form Designer generierter Code "

        'Dieser Aufruf ist für den Web Form-Designer erforderlich.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: Diese Methode ist für den Web Form-Designer erforderlich
            'Verwenden Sie nicht den Code-Editor zur Bearbeitung.
            InitializeComponent()
        End Sub

#End Region

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

        Public Property Message() As String
            Get
                Return m_strMessage
            End Get
            Set(ByVal Value As String)
                m_strMessage = Value
                lblMessage.Text = m_strMessage & "<br>"
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

        Public Property Kontingente() As DataTable
            Get
                Return m_tblKontingente
            End Get
            Set(ByVal Value As DataTable)
                Dim strTemp As String = ""

                m_tblKontingente = Value

                If (Not (m_tblKontingente Is Nothing)) AndAlso (Not (m_tblKontingente.Rows.Count = 0)) Then
                    lblMessage.CssClass = "LabelExtraLarge"

                    DataGrid1.DataSource = m_tblKontingente
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

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Dim strTemp As String = ""

            'If (m_strHaendlerNummer Is Nothing) OrElse (m_strHaendlerNummer.Length = 0) Then
            '    strTemp = "Fehler: Es konnten keine Händlerdaten ermittelt werden."
            '    lblMessage.CssClass = "TextError"
            'Else
            '    lblMessage.CssClass = "LabelExtraLarge"

            '    If (m_tblKontingente Is Nothing) OrElse (m_tblKontingente.Rows.Count = 0) Then
            '        'If strTemp.Length = 0 Then
            '        '    strTemp = "Fehler: Es konnten keine Kontingentdaten ermittelt werden."
            '        'Else
            '        '    strTemp &= "<br>Fehler: Es konnten keine Kontingentdaten ermittelt werden."
            '        'End If
            '        'lblMessage.CssClass = "TextError"
            '    Else
            '        lblMessage.CssClass = "LabelExtraLarge"

            '        DataGrid1.DataSource = m_tblKontingente
            '        DataGrid1.DataBind()

            '        Dim intKreditlimit As Int32
            '        Dim intAusschoepfung As Int32
            '        Dim blnGesperrt As Boolean

            '        Dim item As DataGridItem
            '        Dim cell As TableCell
            '        Dim chkBox As CheckBox
            '        Dim label As Label
            '        Dim control As Control

            '        For Each item In DataGrid1.Items
            '            Dim blnZeigeKontingent As Boolean
            '            cell = item.Cells(0)
            '            For Each control In cell.Controls
            '                If TypeOf control Is CheckBox Then
            '                    chkBox = CType(control, CheckBox)
            '                    blnZeigeKontingent = chkBox.Checked
            '                End If
            '            Next
            '            cell = item.Cells(2)
            '            For Each control In cell.Controls
            '                If TypeOf control Is Label Then
            '                    label = CType(control, Label)
            '                    If label.ID = "Label1" And blnZeigeKontingent Then
            '                        label.Visible = True
            '                        intKreditlimit = CInt(label.Text)
            '                    Else
            '                        label.Visible = False
            '                        If label.ID = "Label2" And (Not blnZeigeKontingent) Then
            '                            intKreditlimit = CInt(label.Text)
            '                            If UserReferenz.Length = 0 Then
            '                                label.Visible = True
            '                            Else
            '                                label.Visible = False
            '                                If intKreditlimit = 0 Then
            '                                    item.Visible = False
            '                                End If
            '                            End If
            '                        Else
            '                            label.Visible = False
            '                        End If
            '                    End If
            '                End If
            '            Next
            '            intAusschoepfung = CInt(item.Cells(3).Text)
            '            cell = item.Cells(4)
            '            For Each control In cell.Controls
            '                If TypeOf control Is Label Then
            '                    label = CType(control, Label)
            '                    If label.ID = "Label3" And blnZeigeKontingent Then
            '                        label.Visible = True
            '                    Else
            '                        label.Visible = False
            '                    End If
            '                End If
            '            Next

            '            cell = item.Cells(5)
            '            For Each control In cell.Controls
            '                If TypeOf control Is CheckBox Then
            '                    chkBox = CType(control, CheckBox)
            '                    blnGesperrt = chkBox.Checked
            '                    If blnZeigeKontingent Then
            '                        chkBox.Visible = True
            '                    Else
            '                        chkBox.Visible = False
            '                    End If
            '                End If
            '            Next

            '            If blnGesperrt Or (intAusschoepfung > intKreditlimit) And blnZeigeKontingent Then
            '                For Each cell In item.Cells
            '                    cell.ForeColor = System.Drawing.Color.Red
            '                Next
            '            End If
            '        Next
            '    End If
            'End If

            'lblMessage.Text = m_strMessage
            'If Not strTemp.Length = 0 Then
            '    If Not lblMessage.Text.Length = 0 Then
            '        lblMessage.Text &= "<br>"
            '    End If
            '    lblMessage.Text &= strTemp
            'End If
        End Sub
    End Class
End Namespace
