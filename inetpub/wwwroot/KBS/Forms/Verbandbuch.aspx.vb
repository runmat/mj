
Public Class Verbandbuch
    Inherits Page

    Private mObjKasse As Kasse

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not Session("mKasse") Is Nothing Then
            mObjKasse = Session("mKasse")
        End If

        verbandbuchErf.NavigateUrl = ConfigurationManager.AppSettings("VerbandbuchHostAdress") &
                                        "Erfassung?vkbur=" & mObjKasse.Lagerort.ToString() &
                                        "&ra=" & ConfigurationManager.AppSettings("VerbandbuchRemoteKey") &
                                        "&rb=" & ConfigurationManager.AppSettings("VerbandbuchTimeStamp")

        verbandbuchRep.NavigateUrl = ConfigurationManager.AppSettings("VerbandbuchHostAdress") &
                                        "Report?vkbur=" & mObjKasse.Lagerort.ToString() &
                                        "&ra=" & ConfigurationManager.AppSettings("VerbandbuchRemoteKey") &
                                        "&rb=" & ConfigurationManager.AppSettings("VerbandbuchTimeStamp")


    End Sub

    Protected Sub lb_zurueck_Click(sender As Object, e As EventArgs) Handles lb_zurueck.Click
        Session("ObjKVP") = Nothing
        Response.Redirect("../Selection.aspx")
    End Sub

End Class