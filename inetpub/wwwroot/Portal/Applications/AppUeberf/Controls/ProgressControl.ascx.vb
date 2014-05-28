Namespace Controls

    Public MustInherit Class ProgressControl
        Inherits System.Web.UI.UserControl
        Protected WithEvents lblOrtLabel As System.Web.UI.WebControls.Label
        Protected WithEvents lblLeasingnehmerOrt As System.Web.UI.WebControls.Label
        Protected WithEvents lblLeasingnehmerName As System.Web.UI.WebControls.Label
        Protected WithEvents lblNameLabel As System.Web.UI.WebControls.Label
        Protected WithEvents lblReferenz As System.Web.UI.WebControls.Label
        Protected WithEvents lblFahrzeugtyp As System.Web.UI.WebControls.Label
        Protected WithEvents lblTypLabel As System.Web.UI.WebControls.Label
        Protected WithEvents lblRefLabel As System.Web.UI.WebControls.Label
        Protected WithEvents lblText As System.Web.UI.WebControls.Label

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

        Public Enum Source
            Unknown
            Zul01
            Ueber01
            Ueber02
            Ueber03
            Ueber04
            ZulBest
            ZulUebBest
        End Enum

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            ' Hier Benutzercode zur Seiteninitialisierung einfügen
        End Sub

        Private Function GetCurrent(ByVal source As Source, ByVal daten As Ueberf_01) As Integer
            Select Case source
                Case source.Zul01
                    Return 1
                Case source.Ueber01
                    Select Case daten.Beauftragung
                        Case Ueberf_01.Beauftragungsart.OffeneUeberfuehrung, Ueberf_01.Beauftragungsart.ReineUeberfuehrung, Ueberf_01.Beauftragungsart.UeberfuehrungKCL
                            Return 1
                        Case Else
                            Return 2
                    End Select
                Case source.Ueber02
                    Select Case daten.Beauftragung
                        Case Ueberf_01.Beauftragungsart.OffeneUeberfuehrung, Ueberf_01.Beauftragungsart.ReineUeberfuehrung, Ueberf_01.Beauftragungsart.UeberfuehrungKCL
                            Return 2
                        Case Else
                            Return 3
                    End Select
                Case source.Ueber03
                    Select Case daten.Beauftragung
                        Case Ueberf_01.Beauftragungsart.OffeneUeberfuehrung, Ueberf_01.Beauftragungsart.ReineUeberfuehrung, Ueberf_01.Beauftragungsart.UeberfuehrungKCL
                            Return 3
                        Case Else
                            Return 4
                    End Select
                Case source.Ueber04
                    Select Case daten.Beauftragung
                        Case Ueberf_01.Beauftragungsart.OffeneUeberfuehrung, Ueberf_01.Beauftragungsart.ReineUeberfuehrung, Ueberf_01.Beauftragungsart.UeberfuehrungKCL
                            Return 3 + IIf(daten.Anschluss, 1, 0)
                        Case Else
                            Return 4 + IIf(daten.Anschluss, 1, 0)
                    End Select
                Case source.ZulUebBest
                    Return 4 + IIf(daten.Anschluss, 1, 0)
                Case source.ZulBest
                    Return 2
                Case Else
                    Throw New NotSupportedException("Unbekannte Quelle:" + source.ToString())
            End Select
        End Function

        Private Function GetMax(ByVal daten As Ueberf_01) As Integer
            Select Case daten.Beauftragung
                Case Ueberf_01.Beauftragungsart.ZulassungKCL
                    Return 2
                Case Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrung, Ueberf_01.Beauftragungsart.ZulassungUndUeberfuehrungKCL
                    Return 4 + IIf(daten.Anschluss, 1, 0)
                Case Ueberf_01.Beauftragungsart.OffeneUeberfuehrung, Ueberf_01.Beauftragungsart.ReineUeberfuehrung, Ueberf_01.Beauftragungsart.UeberfuehrungKCL
                    Return 3 + IIf(daten.Anschluss, 1, 0)
                Case Else
                    Throw New NotSupportedException("Unbekannte Quelle:" + daten.Beauftragung.ToString())
            End Select
        End Function


        Public Sub Fill(ByVal source As Source, ByVal daten As Ueberf_01)

            Me.lblLeasingnehmerName.Text = daten.Leasingnehmer
            Me.lblLeasingnehmerOrt.Text = daten.LeasingnehmerOrt
            Me.lblReferenz.Text = daten.Ref
            Me.lblFahrzeugtyp.Text = daten.Herst

            Me.lblText.Text = "Schritt " + Me.GetCurrent(source, daten).ToString() + " von " + Me.GetMax(daten).ToString() + ""

        End Sub

    End Class

End Namespace

' ************************************************
' $History: ProgressControl.ascx.vb $
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 16.04.08   Time: 11:53
' Updated in $/CKAG/Applications/AppUeberf/Controls
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 16:06
' Created in $/CKAG/Applications/AppUeberf/Controls
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Created in $/CKG/Applications/AppUeberf/AppUeberfWeb/Controls
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************