Public Class CurrentPageHelper
    Public Shared Function GetCurrentPage() As Page
        If HttpContext.Current Is Nothing OrElse HttpContext.Current.Handler Is Nothing Then Return Nothing
        Return TryCast(HttpContext.Current.Handler, Page)
    End Function
End Class
