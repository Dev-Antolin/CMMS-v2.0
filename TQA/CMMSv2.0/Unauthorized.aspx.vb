
Partial Class Unauthorized
    Inherits System.Web.UI.Page

    Protected Sub lbLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLogin.Click
        Response.Redirect("~/Login.aspx")
    End Sub
End Class
