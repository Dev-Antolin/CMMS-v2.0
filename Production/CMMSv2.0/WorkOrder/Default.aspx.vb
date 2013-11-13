
Partial Class WorkOrder_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.Session("Click") = "WOM"
            Response.Redirect("CreateWO.aspx")
        End If
    End Sub
End Class
