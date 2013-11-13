Option Strict On
Partial Class LeftWOMainMasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Session("Click").ToString = "CWO" Then
            CWO.ForeColor = Drawing.Color.Blue
            CWO.BackColor = Drawing.Color.White
            Me.Session("Click") = "WOM"
            Me.Session("View") = Nothing
        ElseIf Me.Session("Click").ToString = "RWO" Then
            RWO.ForeColor = Drawing.Color.Blue
            RWO.BackColor = Drawing.Color.White
            Me.Session("Click") = "WOM"
            Me.Session("View") = Nothing
        ElseIf Me.Session("Click").ToString = "OWO" Then
            OWO.ForeColor = Drawing.Color.Blue
            OWO.BackColor = Drawing.Color.White
            Me.Session("Click") = "WOM"
            Me.Session("View") = Nothing
        ElseIf Me.Session("Click").ToString = "XWO" Then
            XWO.ForeColor = Drawing.Color.Blue
            XWO.BackColor = Drawing.Color.White
            Me.Session("Click") = "WOM"
            Me.Session("View") = Nothing
        ElseIf Me.Session("Click").ToString = "View" Then
            View.ForeColor = Drawing.Color.Blue
            View.BackColor = Drawing.Color.White
            Me.Session("Click") = "WOM"
            Me.Session.Add("View", "canUpdate")
        End If
        lblJob.Text = Me.Session("JDesc").ToString
        lblCostCenter.Text = Me.Session("JCode").ToString
        lblHO.Text = Me.Session("ZCode").ToString
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender        
        If Me.Session("JDesc").ToString().Contains("HELPDESK") OrElse Me.Session("JDesc").ToString().Contains("LPTL") Then
            Me.ViewBranch.Visible = True
        End If
    End Sub
End Class

