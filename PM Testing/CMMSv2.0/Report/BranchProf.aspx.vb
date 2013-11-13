Imports INI_DLL
Imports MYSQLDB_DLL
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine

Partial Class Report_BranchProf
    Inherits System.Web.UI.Page    
    Dim Branches As New BranchReports    

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            CheckUser()
            CheckLogin()
            Try
                Me.ddlRegion.DataSource = Branches.getRegions(Me.Session("strConf"), Me.Session("ZCode"))
                Me.ddlRegion.DataBind()
                If Me.ddlRegion.Items.Count > 0 Then
                    Me.ddlRegion.Enabled = True
                End If
                Me.ddlArea.Enabled = False
                Me.ddlBranch.Enabled = False
                Me.ddlBraMem.Enabled = False
                Me.btnGeneRptBranch.Visible = False
            Catch ex As Exception
                lblErrorReport.Text = ex.Message
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
        Me.Session("BranchAssetP") = Nothing
        Me.Session("AllBranchAssetP") = Nothing
        Me.lblErrorReport.Text = Nothing
        If Not Page.IsPostBack Then
            Me.Session("Click") = "BP"
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
        If Me.ddlRegion.SelectedIndex = 0 Then            
            Me.ddlArea.Items.Clear()
            Me.ddlArea.Enabled = False

            Me.ddlBranch.Items.Clear()
            Me.ddlBranch.Enabled = False

            Me.ddlBraMem.Items.Clear()
            Me.ddlBraMem.Enabled = False

            Me.btnGeneRptBranch.Visible = False
        Else
            Try
                Me.ddlArea.DataSource = Branches.getArea(Me.Session("ZCode"), Me.Session("strConf"), Me.ddlRegion.SelectedItem.Text)
                Me.ddlArea.DataBind()
                Me.ddlArea.Enabled = True

                Me.ddlBranch.Items.Clear()
                Me.ddlBranch.Enabled = False

                Me.ddlBraMem.Items.Clear()
                Me.ddlBraMem.Enabled = False

                Me.ddlBraMem.Items.Clear()
                Me.ddlBraMem.Enabled = False
                Me.btnGeneRptBranch.Visible = False
            Catch ex As Exception
                Me.lblErrorReport.Text = ex.Message
            End Try                    
        End If
    End Sub

    Protected Sub ddlArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlArea.SelectedIndexChanged
        If Me.ddlArea.SelectedIndex = 0 Then
            Me.ddlBranch.Items.Clear()
            Me.ddlBranch.Enabled = False

            Me.ddlBraMem.Items.Clear()
            Me.ddlBraMem.Enabled = False

            Me.btnGeneRptBranch.Visible = False
        Else
            Try
                Me.ddlBranch.DataSource = Branches.getBranches(Me.Session("ZCode"), Me.ddlRegion.SelectedItem.Text, Me.ddlArea.SelectedItem.Text, Me.Session("strConf"))
                Me.ddlBranch.DataBind()
                Me.ddlBranch.Enabled = True

                Me.ddlBraMem.Items.Clear()
                Me.ddlBraMem.Enabled = False

                Me.btnGeneRptBranch.Visible = False
            Catch ex As Exception
                Me.lblErrorReport.Text = ex.Message
            End Try
        End If
    End Sub

    Protected Sub ddlBranch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBranch.SelectedIndexChanged
        If Me.ddlBranch.SelectedIndex = 0 Then
            Me.ddlBraMem.Items.Clear()
            Me.ddlBraMem.Enabled = False

            Me.btnGeneRptBranch.Visible = False
        Else
            Try
                Me.ddlBraMem.DataSource = Branches.getBranchEmployee(Me.Session("ZCode"), Me.ddlBranch.SelectedItem.Text, Me.Session("strConf"))
                Me.ddlBraMem.DataBind()
                Me.ddlBraMem.Enabled = True

                Me.btnGeneRptBranch.Visible = False
            Catch ex As Exception
                Me.lblErrorReport.Text = ex.Message
            End Try            
        End If
    End Sub

    Protected Sub btnGeneRptBranch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGeneRptBranch.Click
        Me.lblErrorReport.Text = Nothing
        Try
            If ddlBraMem.Text = "ALL EMPLOYEE" Then
                Branches.getAllBrachEmployeeReport(Me.ddlBranch.SelectedItem.Text, Me.Session("strCon"))
                If Me.Session("AllBranchAssetP") Is Nothing Then
                    Me.lblErrorReport.Text = "NO RECORD FOUND"
                Else
                    Response.Redirect("BranchAllRPT.aspx")
                End If
            Else
                'If Me.ddlRegion.SelectedIndex <> 0 And Me.ddlArea.SelectedIndex <> 0 And Me.ddlBranch.SelectedIndex <> 0 And Me.ddlBraMem.SelectedIndex <> 0 Then
                Branches.getBranchEmployeeRerpot(Me.ddlBranch.SelectedItem.Text, Me.ddlBraMem.SelectedItem.Text, Me.Session("strCon"))
                If Me.Session("BranchAssetP") Is Nothing Then                    
                    Me.lblErrorReport.Text = "NO RECORD FOUND"
                Else
                    Response.Redirect("BranchRPT.aspx")
                End If
                'End If
            End If
        Catch ex As Exception
            Me.lblErrorReport.Text = ex.Message
        End Try
        
    End Sub

    'Private Sub Region()
    '    Try
    '        Dim sql As New StringBuilder()
    '        sql.Append("SELECT")
    '        sql.Append("    DISTINCT wb.Class_03 ")
    '        sql.Append("FROM")
    '        sql.Append("    OPENQUERY(cmms,'SELECT Bc_Code,Zone_Code FROM cmms.cmms_entry_masterheader; ') AS header ")
    '        sql.Append("INNER JOIN")
    '        sql.Append("    WebProject.dbo.WebBranches AS wb ")
    '        sql.Append("ON")
    '        sql.Append("    header.Bc_Code = wb.bedrnr ")
    '        sql.Append("AND header.Zone_Code = wb.zonecode ")
    '        sql.Append("WHERE")
    '        sql.Append("    header.Zone_Code = @Zone ")
    '        sql.Append("AND wb.Class_03 NOT IN ( 'HO' , '<none>') ")
    '        sql.Append("ORDER BY")
    '        sql.Append("    wb.Class_03 ASC; ")

    '        Using con As New SqlConnection(Me.Session("strConf").ToString())
    '            con.Open()
    '            Using cmd As New SqlCommand(sql.ToString(), con)
    '                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 10).Value() = Me.Session("ZCode")
    '                cmd.CommandType = CommandType.Text
    '                Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
    '                    If DataRead.HasRows() Then
    '                        ddlRegion.Items.Clear()
    '                        ddlRegion.Items.Add("")
    '                        While DataRead.Read()
    '                            ddlRegion.Items.Add(DataRead(0).ToString())
    '                        End While
    '                        DataRead.Close()
    '                    End If
    '                End Using
    '            End Using
    '        End Using
    '        Dim ds As DataSet
    '        Dim myReg As String
    '        If Me.Session("ZCode") = "VISMIN" Then
    '            myReg = "select distinct UPPER(class_03) AS CLASS_03 from webbranches where zonecode = 'vismin' and class_03 <> 'HO' order by class_03 desc;"
    '        Else
    '            myReg = "select distinct UPPER(class_03) AS CLASS_03 from webbranches where zonecode = 'luzon' and class_03 <> 'HO' order by class_03 desc;"
    '        End If
    '        ds = Execute_DataSet(myReg)

    '        Dim RText As String = ""
    '        Dim i As Integer
    '        ddlRegion.Items.Clear()
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            For i = 0 To ds.Tables(0).Rows.Count - 1
    '                ddlRegion.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString.Trim))
    '            Next
    '        Else
    '            RText = "Nothing Available"
    '        End If
    '        ddlRegion.Items.Insert(0, New ListItem(RText, "0"))

    '    Catch ex As Exception
    '        Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
    '    End Try
    'End Sub

    'Private Sub Area()
    '    Try
    '        Dim sql As New StringBuilder
    '        sql.Append("SELECT DISTINCT")
    '        sql.Append("    wb.Class_04 ")
    '        sql.Append("FROM")
    '        sql.Append("    OPENQUERY(cmms,'SELECT Bc_Code,Bc_Name,Zone_Code FROM cmms.cmms_entry_masterheader; ') AS Header ")
    '        sql.Append("INNER JOIN")
    '        sql.Append("    WebProject.dbo.WebBranches AS wb ")
    '        sql.Append("ON")
    '        sql.Append("    Header.Bc_Code = wb.bedrnr ")
    '        sql.Append("AND Header.Zone_Code= wb.ZoneCode ")
    '        sql.Append("WHERE")
    '        sql.Append("    Header.Zone_Code = @Zone ")
    '        sql.Append("AND wb.Class_03 NOT IN ('HO','<none>') ")
    '        sql.Append("AND wb.Class_03 = @Region ")
    '        sql.Append("ORDER BY")
    '        sql.Append("    wb.Class_04 ASC; ")
    '        Using con As New SqlConnection(MsConstr)
    '            con.Open()
    '            Using cmd As New SqlCommand(sql.ToString(), con)
    '                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 6).Value() = Me.Session("ZCode")
    '                cmd.Parameters.Add("Region", SqlDbType.VarChar, 30).Value() = Me.ddlRegion.SelectedItem.Text
    '                cmd.CommandType = CommandType.Text
    '                Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
    '                    If DataRead.HasRows() Then
    '                        Me.ddlArea.Items.Clear()
    '                        Me.ddlArea.Items.Add("")
    '                        While DataRead.Read()
    '                            Me.ddlArea.Items.Add(DataRead(0).ToString)
    '                        End While
    '                        DataRead.Close()
    '                    End If
    '                End Using
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
    '    End Try
    'End Sub

    'Private Sub Branch()
    '    Try
    '        Dim sql As New StringBuilder
    '        sql.Append("SELECT DISTINCT")
    '        sql.Append("    wb.bedrnm ")
    '        sql.Append("FROM")
    '        sql.Append("    OPENQUERY(cmms,'SELECT Bc_Code,Bc_Name,Zone_Code FROM cmms.cmms_entry_masterheader; ') AS Header ")
    '        sql.Append("INNER JOIN")
    '        sql.Append("    WebProject.dbo.WebBranches AS wb ")
    '        sql.Append("ON")
    '        sql.Append("    Header.Bc_Code = wb.bedrnr ")
    '        sql.Append("AND Header.Zone_Code= wb.ZoneCode ")
    '        sql.Append("WHERE")
    '        sql.Append("    Header.Zone_Code = @Zone ")
    '        sql.Append("AND wb.Class_03 NOT IN ('HO','<none>') ")
    '        sql.Append("AND wb.Class_03 = @Region ")
    '        sql.Append("AND wb.Class_04 = @Area ")
    '        sql.Append("ORDER BY")
    '        sql.Append("    wb.bedrnm ASC; ")
    '        Using con As New SqlConnection(MsConstr)
    '            con.Open()
    '            Using cmd As New SqlCommand(sql.ToString(), con)
    '                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 6).Value() = Me.Session("ZCode")
    '                cmd.Parameters.Add("Region", SqlDbType.VarChar, 30).Value() = Me.ddlRegion.SelectedItem.Text
    '                cmd.Parameters.Add("Area", SqlDbType.VarChar, 30).Value() = Me.ddlArea.SelectedItem.Text
    '                cmd.CommandType = CommandType.Text
    '                Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
    '                    If DataRead.HasRows() Then
    '                        Me.ddlBranch.Items.Clear()
    '                        Me.ddlBranch.Items.Add("")
    '                        While DataRead.Read()
    '                            Me.ddlBranch.Items.Add(DataRead(0).ToString)
    '                        End While
    '                        DataRead.Close()
    '                    End If
    '                End Using
    '            End Using
    '        End Using
    '        Dim ds As DataSet
    '        Dim myBra As String
    '        If Me.Session("ZCode") = "VISMIN" Then
    '            myBra = "select distinct UPPER(bedrnm) AS BEDRNM from webbranches " _
    '                  & "where zonecode = 'vismin' and class_04 = '" & ddlArea.Text & "' order by bedrnm desc;"
    '        Else
    '            myBra = "select distinct UPPER(bedrnm) AS BEDRNM from webbranches " _
    '                  & "where zonecode = 'luzon' and class_04 = '" & ddlArea.Text & "' order by bedrnm desc;"
    '        End If
    '        ds = Execute_DataSet(myBra)
    '        Dim RText As String = ""
    '        Dim i As Integer
    '        ddlBranch.Items.Clear()
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            For i = 0 To ds.Tables(0).Rows.Count - 1
    '                ddlBranch.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString.Trim))
    '            Next
    '        Else
    '            RText = "Nothing Available"
    '        End If
    '        ddlBranch.Items.Insert(0, New ListItem(RText, "0"))

    '    Catch ex As Exception
    '        Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
    '    End Try
    'End Sub

    'Private Sub BranchManName()
    '    Try
    '        Dim sql As New StringBuilder()
    '        sql.Append("SELECT DISTINCT")
    '        sql.Append("    UPPER(wa.fullname) AS FullName ")
    '        sql.Append("FROM OPENQUERY(cmms,'SELECT Bc_Code,Bc_Name,Emp_Name_Assigned FROM cmms.cmms_entry_masterheader; ') AS Header ")
    '        sql.Append("INNER JOIN")
    '        sql.Append("    WebProject.dbo.WebAccounts wa ")
    '        sql.Append("ON")
    '        sql.Append("    wa.fullname = Header.Emp_Name_Assigned ")
    '        sql.Append("INNER JOIN")
    '        sql.Append("    WebProject.dbo.WebBranches wb ")
    '        sql.Append("ON")
    '        sql.Append("    wa.comp = wb.bedrnr ")
    '        sql.Append("WHERE")
    '        sql.Append("    wa.zonecode = @Zone ")
    '        sql.Append("AND wb.bedrnm = @Bc_Name ")
    '        sql.Append("ORDER BY")
    '        sql.Append("    wa.fullname ASC; ")

    '        Using con As New SqlConnection(MsConstr)
    '            con.Open()
    '            Using cmd As New SqlCommand(sql.ToString(), con)
    '                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 6).Value() = Me.Session("ZCode")
    '                cmd.Parameters.Add("Bc_Name", SqlDbType.VarChar, 60).Value() = Me.ddlBranch.SelectedItem.Text
    '                cmd.CommandType = CommandType.Text
    '                Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
    '                    If DataRead.HasRows() Then
    '                        Me.ddlBraMem.Items.Clear()
    '                        Me.ddlBraMem.Items.Add("")
    '                        While DataRead.Read()
    '                            Me.ddlBraMem.Items.Add(DataRead(0).ToString)
    '                        End While
    '                        DataRead.Close()
    '                        Me.ddlBraMem.Items.Add("ALL EMPLOYEE")
    '                    End If
    '                End Using
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        Throw ex
    '        Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
    '    End Try
    'End Sub

    'Public Function Execute_DataSet(ByVal as_mysql As String) As DataSet
    '    Dim Con As New SqlConnection
    '    Dim Com As New SqlCommand
    '    Dim sqlAdapter As SqlDataAdapter
    '    Dim sqlDataset As New DataSet

    '    Execute_DataSet = Nothing
    '    Try
    '        Try
    '            Con.ConnectionString = Me.Session("strConf")
    '            If Con.State = ConnectionState.Closed Then
    '                Con.Open()
    '            End If
    '        Catch
    '        End Try
    '        sqlAdapter = New SqlDataAdapter(as_mysql, Con)
    '        sqlAdapter.Fill(sqlDataset)
    '        If Not sqlDataset Is Nothing Then
    '            If sqlDataset.Tables(0).Rows.Count <> 0 Then
    '                Execute_DataSet = sqlDataset
    '                sqlDataset.Dispose()
    '                sqlAdapter.Dispose()
    '            End If
    '        End If
    '        Con.Close()
    '    Catch ex As Exception
    '        Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
    '        Con.Close()
    '        Com.Dispose()
    '    End Try
    'End Function

    'Private Sub getBranch(ByVal DateFrom As String, ByVal DateTo As String, ByVal Bc_Name As String, ByVal Bc_Emp As String, ByVal Connection As String)
    '    Dim dt As New DataTable
    '    Using con As New MySqlConnection(Connection)
    '        Using cmd As New MySqlCommand("SPBraAssProf", con)
    '            cmd.Parameters.Add("DateFrom", MySqlDbType.VarChar, 20).Value = DateFrom
    '            cmd.Parameters.Add("DateTo", MySqlDbType.VarChar, 20).Value = DateTo
    '            cmd.Parameters.Add("ByBra", MySqlDbType.VarChar, 30).Value = Bc_Name
    '            cmd.Parameters.Add("ByBraName", MySqlDbType.VarChar, 30).Value = Bc_Emp
    '            cmd.CommandType = CommandType.StoredProcedure
    '            Try
    '                con.Open()
    '                Using DataRead As MySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
    '                    If DataRead.HasRows() Then
    '                        dt.Load(DataRead)
    '                        Dim myReport As New ReportDocument
    '                        With myReport
    '                            .Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\BranchAssetProfile.rpt")
    '                            .SetDataSource(dt)
    '                            .SetParameterValue("DateFrom", DateFrom)
    '                            .SetParameterValue("DateTo", DateTo)
    '                            .SetParameterValue("Branch", Bc_Name)
    '                            .SetParameterValue("EmpName", Bc_Emp)
    '                        End With
    '                        Me.Session.Add("BranchAssetP", myReport)
    '                    End If
    '                End Using
    '            Catch ex As Exception
    '                Throw
    '            End Try
    '        End Using
    '    End Using
    'End Sub

    'Private Sub getAllBranch(ByVal DateFrom As String, ByVal DateTo As String, ByVal Bc_Name As String, ByVal Connection As String)
    '    Dim dt As New DataTable
    '    Using con As New MySqlConnection(Connection)
    '        Using cmd As New MySqlCommand("SPBraAllAssProf", con)
    '            cmd.Parameters.Add("DateFrom", MySqlDbType.VarChar, 20).Value = DateFrom
    '            cmd.Parameters.Add("DateTo", MySqlDbType.VarChar, 20).Value = DateTo
    '            cmd.Parameters.Add("ByBra", MySqlDbType.VarChar, 30).Value = Bc_Name
    '            cmd.CommandType = CommandType.StoredProcedure
    '            Try
    '                con.Open()
    '                Using DataRead As MySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
    '                    If DataRead.HasRows() Then
    '                        dt.Load(DataRead)
    '                        Dim myReport As New ReportDocument
    '                        With myReport
    '                            .Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\BranchAllAssetProfile.rpt")
    '                            .SetDataSource(dt)
    '                            .SetParameterValue("DateFrom", DateFrom)
    '                            .SetParameterValue("DateTo", DateTo)
    '                            .SetParameterValue("Branch", Bc_Name)
    '                        End With
    '                        Me.Session.Add("AllBranchAssetP", myReport)
    '                    End If
    '                End Using
    '            Catch ex As Exception
    '                Throw
    '            End Try
    '        End Using
    '    End Using
    'End Sub

    Protected Sub ddlBraMem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBraMem.SelectedIndexChanged
        If Me.ddlBraMem.SelectedIndex = 0 Then
            Me.btnGeneRptBranch.Visible = False
        Else
            Me.btnGeneRptBranch.Visible = True
        End If
    End Sub
End Class
