
Partial Class LeftWORABLRMainMasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Session("JDesc") = "BM/BOSMAN" OrElse Me.Session("JDesc") = "REGIONAL MAN" OrElse Me.Session("JDesc") = "AREA MANAGER" Then
            RWORABLR.Visible = False
            RWORABLRLI.Visible = False
        Else
            RWORABLR.Visible = True
            RWORABLRLI.Visible = True
        End If
        If Me.Session("Click") = "CWORABLR" Then
            CWORABLR.ForeColor = Drawing.Color.Blue
            CWORABLR.BackColor = Drawing.Color.White
            Me.Session("Click") = "WOMRABLR"
        ElseIf Me.Session("Click") = "RWORABLR" Then
            RWORABLR.ForeColor = Drawing.Color.Blue
            RWORABLR.BackColor = Drawing.Color.White
            Me.Session("Click") = "WOMRABLR"
        ElseIf Me.Session("Click") = "OWORABLR" Then
            OWORABLR.ForeColor = Drawing.Color.Blue
            OWORABLR.BackColor = Drawing.Color.White
            Me.Session("Click") = "WOMRABLR"
        ElseIf Me.Session("Click") = "XWORABLR" Then
            XWORABLR.ForeColor = Drawing.Color.Blue
            XWORABLR.BackColor = Drawing.Color.White
            Me.Session("Click") = "WOMRABLR"
        End If
        lblJob.Text = Me.Session("JDesc")
        lblCostCenter.Text = Me.Session("JCode")
        lblHO.Text = Me.Session("ZCode")
    End Sub
End Class

