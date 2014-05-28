Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Portal.PageElements
Imports CKG.Base.Kernel.Common.Common

Namespace Controls

    '----------------------------
    'Stellt Eingabemaske für Suche nach Adressen bereit
    'Liefert gefundene Adressen über Event
    '----------------------------
    Public MustInherit Class AddressSearchInputControl
        Inherits System.Web.UI.UserControl
        Protected WithEvents txtName As System.Web.UI.WebControls.TextBox
        Protected WithEvents Label1 As System.Web.UI.WebControls.Label
        Protected WithEvents Label2 As System.Web.UI.WebControls.Label
        Protected WithEvents Label4 As System.Web.UI.WebControls.Label
        Protected WithEvents btnSuche As System.Web.UI.WebControls.Button
        Protected WithEvents Label5 As System.Web.UI.WebControls.Label
        Protected WithEvents txtPLZ As System.Web.UI.WebControls.TextBox
        Protected WithEvents txtOrt As System.Web.UI.WebControls.TextBox
        Protected WithEvents lblError As System.Web.UI.WebControls.Label
        Protected WithEvents Label3 As System.Web.UI.WebControls.Label

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

#Region "Deklarationen"

        Public Event ErrorOccured As Helper.Delegates.ProcessException

        Private _ddlAdressen As DropDownList

#End Region

#Region "Properties"

        '----
        'Dropdownlist in der die Ergebnisse angezeigt werden sollen
        '----
        Public Property ResultDropdownList() As DropDownList
            Get
                Return _ddlAdressen
            End Get
            Set(ByVal Value As DropDownList)
                _ddlAdressen = Value
            End Set
        End Property

#End Region

#Region "Events"

        '----------
        'Laden des Controls
        '----------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                lblError.Text = ""

            Catch ex As Exception
                RaiseEvent ErrorOccured(ex)
            End Try
        End Sub

        '----------
        'Führt die Suche nach den Adressen durch
        '----------
        Private Sub btnSuche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuche.Click
            Try

                PerformSearch()

            Catch mex As Exceptions.MandatoryDataMissingException
                lblError.Text = mex.Message

            Catch ex As Exception
                RaiseEvent ErrorOccured(ex)
            End Try
        End Sub

#End Region

#Region "Methods"

        '---------
        'Führt die Suche durch
        '---------
        Public Sub PerformSearch()

            If txtOrt.Text = "" AndAlso txtPLZ.Text = "" AndAlso txtName.Text = "" Then
                Throw New Exceptions.MandatoryDataMissingException("Bitte geben Sie wenigstens ein Suchkriterium an.")
            End If

            Dim addressAccess As New Adressen()

            'Adressen suchen und Ergebnis über Event bekannt geben
            Dim objUser As Base.Kernel.Security.User = CType(Session("objUser"), Base.Kernel.Security.User)
            ProcessAddresses(addressAccess.GetData(objUser, New Base.Kernel.Security.App(objUser), txtOrt.Text, txtPLZ.Text, txtName.Text))

        End Sub

        '-------
        'Verarbeitet die Adressen
        '-------
        Private Sub ProcessAddresses(ByVal dt As DataSets.AddressDataSet.ADDRESSEDataTable)

            If Not _ddlAdressen Is Nothing Then
                _ddlAdressen.Items.Clear()
                _ddlAdressen.Items.Add(New ListItem("Bitte Auswahl treffen", "0"))
                Dim row As DataSets.AddressDataSet.ADDRESSERow
                Dim maxLen As Integer = 5 'Minimum 5 breit
                For Each row In dt.Rows
                    Dim li As New ListItem(row.NAME + ", " + row.ORT, row.ID)
                    _ddlAdressen.Items.Add(li)

                    If li.Text.Length > maxLen Then
                        maxLen = li.Text.Length
                    End If
                Next

                _ddlAdressen.SelectedIndex = 0

                'Daten speichern
                Session(_ddlAdressen.ID) = dt

            End If

        End Sub

#End Region

    End Class

End Namespace

' ************************************************
' $History: AddressSearchInputControl.ascx.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Controls
' 
' *****************  Version 2  *****************
' User: Uha          Date: 21.05.07   Time: 11:46
' Updated in $/CKG/Applications/AppUeberf/AppUeberfWeb/Controls
' Änderungen im Vergleich zur Startapplikation zum Stand 11.05.2007
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Controls
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************