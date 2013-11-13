
Partial Class EntryData_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.Session("Click") = "DEM"
        End If
        Response.Redirect("cabasicdata.aspx")
    End Sub
End Class
