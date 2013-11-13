
Partial Class LeftDEMainMasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Session("Click") = "BasicData" Then
            BasicData.ForeColor = Drawing.Color.Blue
            BasicData.BackColor = Drawing.Color.White
            Me.Session("Click") = "DEM"
            Me.Session("View") = Nothing
        ElseIf Me.Session("Click") = "AddDevices" Then
            AddDevices.ForeColor = Drawing.Color.Blue
            AddDevices.BackColor = Drawing.Color.White
            Me.Session("Click") = "DEM"
            Me.Session("View") = Nothing
        ElseIf Me.Session("Click") = "SearchAAssetNo" Then
            SearchAAssetNo.ForeColor = Drawing.Color.Blue
            SearchAAssetNo.BackColor = Drawing.Color.White
            Me.Session("Click") = "DEM"
            Me.Session("View") = Nothing
        ElseIf Me.Session("Click") = "View" Then
            View.ForeColor = Drawing.Color.Blue
            View.BackColor = Drawing.Color.White
            Me.Session("Click") = "DEM"
            Me.Session.Add("View", "canUpdate")
        End If
        lblJob.Text = Me.Session("JDesc")
        lblCostCenter.Text = Me.Session("JCode")
        lblHO.Text = Me.Session("ZCode")
    End Sub


    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Me.Session("JDesc") <> Nothing Then
            If Me.Session("JDesc").ToString().Contains("MMD") Then
                Me.viewBranch.Visible = True
            End If
        End If
    End Sub
End Class

