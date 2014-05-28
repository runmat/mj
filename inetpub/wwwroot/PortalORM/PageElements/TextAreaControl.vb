Imports System.Web.UI.WebControls


Namespace PageElements

    Public Class TextAreaControl
        Inherits TextBox

        Protected Overrides Sub OnPreRender(ByVal e As EventArgs)

            If Me.MaxLength > 0 AndAlso TextMode = TextBoxMode.MultiLine Then

                'Add javascript handlers for paste and keypress
                Attributes.Add("onkeypress", Me.ID + "_doKeypress(this);")
                Attributes.Add("onbeforepaste", Me.ID + "_doBeforePaste(this);")
                Attributes.Add("onpaste", Me.ID + "_doPaste(this);")

                'Add attribute for access of maxlength property on client-side
                Attributes.Add("maxLength", Me.MaxLength.ToString())

                MyBase.OnPreRender(e)
            End If

        End Sub


        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

            'JavaScript einbauen
            'UH: 31.05.2007
            'Beim Gesamterstellen mit vbc.exe hatte ich Probleme textArea.js als eingebettete Ressource zu bekommen
            '=> Verweis auf den Pfad der Datei.
            'Dim script As String = New IO.StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".textArea.js")).ReadToEnd
            Dim script As String = New IO.StreamReader(Page.Request.PhysicalApplicationPath & "PageElements\JavaScript\textArea.js").ReadToEnd
            writer.WriteLine(script.Replace("%CONTROL_ID%", Me.ID))

            MyBase.Render(writer)
        End Sub

    End Class

End Namespace

' ************************************************
' $History: TextAreaControl.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:19
' Created in $/CKAG/PortalORM/PageElements
' 
' *****************  Version 3  *****************
' User: Uha          Date: 31.05.07   Time: 13:43
' Updated in $/CKG/PortalORM/PageElements
' ITA 0844 Architektur
' ITA 1077 Doppeltes Login
' 
' *****************  Version 2  *****************
' User: Uha          Date: 3.05.07    Time: 15:16
' Updated in $/CKG/PortalORM/PageElements
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 13:23
' Created in $/CKG/PortalORM/PageElements
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************