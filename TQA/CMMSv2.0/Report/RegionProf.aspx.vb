Imports INI_DLL
Imports MYSQLDB_DLL
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Partial Class Report_RegionProf
    Inherits System.Web.UI.Page
    Dim Region As New RegionReports

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            CheckUser()
            CheckLogin()
            Try
                Me.ddlRegion.DataSource = Region.getRegions(Me.Session("strConf"), Me.Session("ZCode"))
                Me.ddlRegion.DataBind()
                Me.btnGeneRptRegion.Visible = False
            Catch ex As Exception
                Me.lblErrorReport.Text = ex.Message
            End Try            
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Disable Cache
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        CheckLogin()
        If Not Page.IsPostBack Then
            Me.Session("Click") = "RP"            
        Else
            Me.lblErrorReport.Text = Nothing
        End If

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
        If Me.ddlRegion.SelectedIndex <> 0 Then            
            Try                
                Me.txtRegMan.Text = Region.getRegionalManager(Me.Session("strConf").ToString, Me.Session("ZCode").ToString, Me.ddlRegion.SelectedItem.Text)
            Catch ex As Exception
                Me.lblErrorReport.Text = ex.Message
            End Try
            Me.btnGeneRptRegion.Visible = True
        Else
            Me.txtRegMan.Text = Nothing
            Me.btnGeneRptRegion.Visible = False            
        End If

    End Sub

    Protected Sub btnGeneRptRegion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGeneRptRegion.Click        
        If Me.ddlRegion.SelectedIndex <> 0 Then
            Try
                Region.GenerateRegionRpt(Me.ddlRegion.SelectedItem.Text, Me.txtRegMan.Text, Me.Session("strConf"), Me.Session("ZCode").ToString)                
                If Me.Session("RegionAssetP") Is Nothing Then
                    Me.lblErrorReport.Text = "NO RECORD FOUND"                    
                Else                                   
                    Response.Redirect("RegionRPT.aspx")
                End If
            Catch ex As Exception
                Me.lblErrorReport.Text = ex.Message
            End Try
        End If
    End Sub

    'Private Function RegionalManager(ByVal Connection As String, ByVal Zone As String, ByVal Region As String) As String
    '    Dim result As String = Nothing
    '    Dim sql As New StringBuilder()
    '    sql.Append("SELECT DISTINCT")
    '    sql.Append("    UPPER(RTRIM(wa.fullname)) AS fullname ")
    '    sql.Append("FROM")
    '    sql.Append("    WebAccounts AS wa ")
    '    sql.Append("INNER JOIN")
    '    sql.Append("    WebBranches AS wb ")
    '    sql.Append("ON")
    '    sql.Append("    wa.comp = wb.bedrnr ")
    '    sql.Append("WHERE")
    '    sql.Append("    wa.zonecode = @Zone ")
    '    sql.Append("AND wb.Class_03 = @Region ")
    '    sql.Append("AND wa.task = 'Regional Man' ")
    '    sql.Append("ORDER BY")
    '    sql.Append("    wa.fullname DESC; ")
    '    Using con As New SqlConnection(Connection)            
    '        Using cmd As New SqlCommand(sql.ToString(), con)
    '            cmd.Parameters.Add("Zone", SqlDbType.VarChar, 10).Value = Zone
    '            cmd.Parameters.Add("Region", SqlDbType.VarChar, 30).Value = Region
    '            cmd.CommandType = CommandType.Text
    '            Try
    '                con.Open()
    '                result = cmd.ExecuteScalar()
    '                If result = Nothing Then
    '                    result = "No Regional Manager assigned in this region."
    '                End If
    '            Catch ex As Exception
    '                Throw
    '            End Try                
    '        End Using
    '    End Using
    '    Return result
    'End Function

    'Private Sub getRegionRPT(ByVal DateFrom As String, ByVal DateTo As String, ByVal Region As String, ByVal Manager As String, ByVal Connection As String)
    '    Dim dt As New DataTable
    '    Using con As New SqlConnection(Connection)
    '        Using cmd As New SqlCommand("SPRegAssProf", con)
    '            cmd.Parameters.Add("datefrom", SqlDbType.DateTime).Value = DateFrom
    '            cmd.Parameters.Add("dateto", SqlDbType.DateTime).Value = DateTo
    '            cmd.Parameters.Add("region", SqlDbType.VarChar, 50).Value = Region
    '            cmd.CommandType = CommandType.StoredProcedure
    '            Try
    '                con.Open()
    '                Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
    '                    If DataRead.HasRows Then
    '                        dt.Load(DataRead)
    '                        Dim myReport As New ReportDocument
    '                        myReport.Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\RegionAssetProfile.rpt")
    '                        myReport.SetDataSource(dt)
    '                        myReport.SetParameterValue("DateFrom", DateFrom)
    '                        myReport.SetParameterValue("DateTo", DateTo)
    '                        myReport.SetParameterValue("RegName", Region)
    '                        myReport.SetParameterValue("ManName", Manager)
    '                        Me.Session.Add("RegionAssetP", myReport)
    '                    End If
    '                End Using
    '            Catch ex As Exception
    '                Throw
    '            End Try
    '        End Using
    '    End Using
    'End Sub

End Class
