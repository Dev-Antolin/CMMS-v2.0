
Partial Class LeftRPTMainMasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Session("JDesc").ToString.Contains("LPTL") Then
            DP.Visible = False
            AM.Visible = False
            Me.Session("Click") = "BP"
        End If
        If Me.Session("Click") = "DP" Then
            DP.ForeColor = Drawing.Color.Blue
            DP.BackColor = Drawing.Color.White
            Me.Session("Click") = "RAF"
        ElseIf Me.Session("Click") = "BP" Then
            BP.ForeColor = Drawing.Color.Blue
            BP.BackColor = Drawing.Color.White
            Me.Session("Click") = "RAF"
        ElseIf Me.Session("Click") = "AP" Then
            AP.ForeColor = Drawing.Color.Blue
            AP.BackColor = Drawing.Color.White
            Me.Session("Click") = "RAF"
        ElseIf Me.Session("Click") = "RP" Then
            RP.ForeColor = Drawing.Color.Blue
            RP.BackColor = Drawing.Color.White
            Me.Session("Click") = "RAF"
        ElseIf Me.Session("Click") = "AM" Then
            AM.ForeColor = Drawing.Color.Blue
            AM.BackColor = Drawing.Color.White
            Me.Session("Click") = "RAF"
        End If
        lblJob.Text = Me.Session("JDesc")
        lblCostCenter.Text = Me.Session("JCode")
        lblHO.Text = Me.Session("ZCode")
    End Sub
End Class

