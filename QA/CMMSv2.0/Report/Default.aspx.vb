
Partial Class Reports_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.Session("Click") = "RAF"
        End If
        If Me.Session("JDesc").ToString.Contains("LPTL") Then
            Response.Redirect("BranchProf.aspx")
        Else
            Response.Redirect("DivisionProf.aspx")
        End If
    End Sub
End Class
