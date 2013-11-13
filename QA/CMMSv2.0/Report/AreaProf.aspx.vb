Imports INI_DLL
Imports MYSQLDB_DLL
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Partial Class Report_AreaProf
    Inherits System.Web.UI.Page    
    Dim Area As New AreaReports

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then            
            CheckUser()
            CheckLogin()
            Try
                Me.ddlRegion.DataSource = Area.getRegions(Me.Session("strConf"), Me.Session("ZCode"))
                Me.ddlRegion.DataBind()
                Me.ddlRegion.Enabled = Me.ddlRegion.Items.Count > 0
                Me.ddlArea.Enabled = False
                Me.Session("AreaAssetP") = Nothing
                Me.btnGeneRptArea.Visible = False
            Catch ex As Exception
                Me.lblErrorReport.Text = ex.Message
            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        CheckLogin()
        If Not Page.IsPostBack Then
            Me.Session("Click") = "AP"            
        End If
        Me.lblErrorReport.Text = Nothing
    End Sub

    Private Sub CheckLogin()
        If Me.Session("fName") = "" Then
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Sub CheckUser()
        If Me.Session("JDesc") = "MMD-STAFF" Then
            Exit Sub
        Else
            Response.Redirect("~/Unauthorized.aspx")
        End If
    End Sub

    Protected Sub ddlRegion_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRegion.SelectedIndexChanged        
        If Me.ddlRegion.SelectedIndex = 0 Then
            Me.btnGeneRptArea.Visible = False
            Me.ddlArea.Items.Clear()
            Me.ddlArea.Enabled = False
            Me.txtAreaManager.Text = String.Empty
            Me.btnGeneRptArea.Visible = False
        Else
            Try
                Me.ddlArea.DataSource = Area.getArea(Me.Session("ZCode"), Me.Session("strConf"), Me.ddlRegion.SelectedItem.Text)
            Catch ex As Exception
                Me.lblErrorReport.Text = ex.Message
            End Try
            Me.ddlArea.DataBind()
            Me.ddlArea.Enabled = True
            Me.btnGeneRptArea.Visible = False
            Me.txtAreaManager.Text = String.Empty
        End If
    End Sub

    Protected Sub ddlArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlArea.SelectedIndexChanged        
        If Me.ddlArea.SelectedIndex <> 0 Then            
            Try
                Me.txtAreaManager.Text = Area.getAreaManager(Me.Session("ZCode").ToString, Me.ddlArea.SelectedItem.Text, Me.Session("strConf"))
            Catch ex As Exception
                Me.lblErrorReport.Text = ex.Message
            End Try
            Me.btnGeneRptArea.Visible = True
        Else
            Me.btnGeneRptArea.Visible = False
            Me.txtAreaManager.Text = String.Empty
        End If
    End Sub

    Protected Sub btnGeneRptArea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGeneRptArea.Click        
        Dim AreaMan As String = Request.Form(txtAreaManager.UniqueID)
        If Me.ddlRegion.SelectedIndex <> 0 And Me.ddlArea.SelectedIndex <> 0 Then
            Try
                Area.GenerateAreaRpt(Me.ddlRegion.SelectedItem.Text, Me.ddlArea.SelectedItem.Text, Me.Session("ZCode").ToString, Me.Session("strConf").ToString, Me.txtAreaManager.Text)
                If Me.Session("AreaAssetP") Is Nothing Then
                    Me.lblErrorReport.Text = "NO DATA FOUND"
                Else
                    Response.Redirect("AreaRPT.aspx")
                End If
            Catch ex As Exception
                Me.lblErrorReport.Text = ex.Message
            End Try
        End If
    End Sub
End Class
