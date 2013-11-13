Imports INI_DLL
Imports MYSQLDB_DLL
Imports MySql.Data.MySqlClient
Imports System
Imports System.Data
Imports System.Data.SqlClient

Partial Class Login
    Inherits System.Web.UI.Page

    Protected Sub btnLogIn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogIn.Click
        If txtPassword.Text = "" And txtUserName.Text = "" Then
            lblError.Text = "Fill up username and password!"
            txtUserName.Focus()
            Exit Sub
        ElseIf txtPassword.Text = "" And txtUserName.Text = txtUserName.Text Then
            lblError.Text = "Please fill up password!"
            txtPassword.Focus()
            Exit Sub
        ElseIf txtUserName.Text = "" And txtPassword.Text = txtPassword.Text Then
            lblError.Text = "Please fill up username!"
            txtUserName.Focus()
            Exit Sub
        Else
            Try                
                CheckUser(Trim(Me.txtUserName.Text), Trim(Me.txtPassword.Text))
            Catch ex As SqlException
                If ex.Number = -2 Then
                    Me.lblError.Text = "Log-in Failed! Connection error."
                Else
                    Me.lblError.Text = ex.Message
                End If
            Catch ex As Exception                
                lblError.Text = ex.Message
            End Try
            Me.txtPassword.Text = Nothing
            Me.txtUserName.Text = Nothing

        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            Response.Buffer = True
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
            Response.Expires = -1500
            Response.CacheControl = "no-cache"            
            Me.Session.Add("strConf", ConfigurationManager.ConnectionStrings("WebProject").ConnectionString)
            Me.Session.Add("strCon", ConfigurationManager.ConnectionStrings("CMMS").ConnectionString)
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            HttpContext.Current.Response.Cache.SetNoServerCaching()
            HttpContext.Current.Response.Cache.SetNoStore()
        End If
    End Sub

    Private Sub CheckUser(ByVal usr As String, ByVal pwd As String)
        Try
            Dim link As String = Nothing
            Dim zCode As String = UserZoneCode(usr, pwd)
            If zCode = "True" Then
                If Me.Session("ZComp") = "001" Or Me.Session("ZComp").ToString = "002" Then
                    Dim LoginMsg As String = getUserDivision(usr, pwd, Me.Session("ZCode").ToString, Me.Session("ZComp").ToString)
                    If LoginMsg = "True" Then
                        If Me.Session("JDesc") = "MMD-STAFF" Then
                            Response.Redirect("~/DataEntry/Default.aspx", False)
                        ElseIf DivUsersVismin(Me.Session("JDesc").ToString, Me.Session("strConf").ToString, Me.Session("ZCode").ToString, Me.Session("ZComp").ToString, usr) Or Me.Session("JDesc") = "BOS-CONT" Then
                            Response.Redirect("~/WorkOrder/Default.aspx", False)
                        Else
                            lblError.Text = "User is not registered! Please contact your administrator."                          
                        End If
                    ElseIf LoginMsg = "You're not authorized" Then
                        lblError.Text = "Access Denied! Please check your username and password."
                    End If
                Else
                    Dim LoginMsg As String = getUserBranch(usr, pwd, Me.Session("ZCode").ToString)
                    If LoginMsg = "True" Then
                        link = BranchUsers(Me.Session("strConf").ToString, Me.Session("JDesc").ToString, Me.Session("ZCode").ToString, usr)
                        If link <> Nothing Then
                            Response.Redirect(link)
                        Else
                            lblError.Text = "User is not registered! Please contact your administrator."                            
                        End If
                    ElseIf LoginMsg = "You're not authorized" Then
                        lblError.Text = "Access Denied! Please check your username and password."                        
                    End If
                End If
            ElseIf zCode = "You're not authorized" Then
                lblError.Text = "Access Denied! Please check your username and password."
                txtUserName.Text = ""
            ElseIf zCode = "No connection" Then
                lblError.Text = "Log-in Failed! Connection error."
                txtUserName.Text = ""
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function getUserDivision(ByVal usr As String, ByVal pwd As String, ByVal Zone As String, ByVal Computer As String) As String
        Dim sql As New StringBuilder()
        Dim result As String = Nothing
        Using con As New SqlConnection(Me.Session("strConf"))
            sql.Append("SELECT")
            sql.Append("    UPPER(RTRIM(wa.fullname)) AS fullname, ")
            sql.Append("    wa.res_id, ")
            sql.Append("    RTRIM(wa.job_title) AS job_title, ")
            sql.Append("    RTRIM(wa.task) AS task, ")
            sql.Append("    RTRIM(ird.divisionacro) AS divisionacro, ")
            sql.Append("    RTRIM(ird.division) AS division, ")
            sql.Append("    RTRIM(ird.divisioncode) AS divisioncode, ")
            sql.Append("    wb.class_03 ")
            sql.Append("FROM")
            sql.Append("    WebAccounts AS wa ")
            sql.Append("INNER JOIN")
            sql.Append("    WebBranches AS wb ")
            sql.Append("ON")
            sql.Append("    wa.comp = wb.bedrnr ")
            sql.Append("AND wa.zonecode = wb.zonecode ")
            sql.Append("INNER JOIN")
            sql.Append("    irdivision AS ird ")
            sql.Append("ON")
            sql.Append("    ird.costcenter = wa.costcenter ")
            sql.Append("AND ird.zonecode = wa.zonecode ")
            sql.Append("WHERE")
            sql.Append("    wa.usr_id = @usr ")
            sql.Append("AND wa.res_id = @pwd ")
            sql.Append("AND wa.ZoneCode = @Zone ")
            sql.Append("AND wa.Comp = @comp; ")
            Using cmd As New SqlCommand(sql.ToString(), con)
                cmd.Parameters.Add("usr", SqlDbType.VarChar, 20).Value = usr
                cmd.Parameters.Add("pwd", SqlDbType.BigInt, 15).Value = pwd
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 10).Value = Zone
                cmd.Parameters.Add("Comp", SqlDbType.VarChar, 4).Value = Computer
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                        If DataRead.HasRows Then
                            If DataRead.Read() Then
                                Me.Session.Add("fName", DataRead.GetString(DataRead.GetOrdinal("fullname")))
                                Me.Session.Add("res_id", DataRead.GetInt32(DataRead.GetOrdinal("res_id")))
                                Me.Session.Add("JPos", DataRead.GetString(DataRead.GetOrdinal("job_title")))
                                Me.Session.Add("JDesc", DataRead.GetString(DataRead.GetOrdinal("task")))
                                Me.Session.Add("JCode", DataRead.GetString(DataRead.GetOrdinal("divisionacro")))
                                Me.Session.Add("JCodeName", DataRead.GetString(DataRead.GetOrdinal("division")))
                                Me.Session.Add("JCodeNo", DataRead.GetString(DataRead.GetOrdinal("divisioncode")))
                                Me.Session.Add("JRegion", DataRead.GetString(DataRead.GetOrdinal("class_03")))
                                DataRead.Close()
                                result = True
                            End If
                        Else
                            result = "You're not authorized"
                        End If
                    End Using
                Catch ex As Exception
                    Throw
                Finally
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                End Try
            End Using
        End Using
        Return result
    End Function

    Public Function getUserBranch(ByVal usr As String, ByVal pwd As String, ByVal Zone As String) As String
        Dim sql As New StringBuilder
        Dim result As String = Nothing
        Using con As New SqlConnection(Me.Session("strConf"))
            sql.Append("SELECT")
            sql.Append("    UPPER(RTRIM(wa.fullname)) AS fullname, ")
            sql.Append("    wa.res_id, ")
            sql.Append("    RTRIM(wa.job_title) AS job_title, ")
            sql.Append("    RTRIM(wa.task) AS task, ")
            sql.Append("    RTRIM(wb.bedrnr) AS bedrnr, ")
            sql.Append("    RTRIM(wb.bedrnm) AS bedrnm, ")
            sql.Append("    RTRIM(irr.regionid) AS regionid, ")
            sql.Append("    RTRIM(wb.class_03) AS class_03 ")
            sql.Append("FROM")
            sql.Append("    dbo.WebAccounts AS wa ")
            sql.Append("INNER JOIN")
            sql.Append("    dbo.WebBranches AS wb ")
            sql.Append("ON ")
            sql.Append("    wa.comp = wb.bedrnr ")
            sql.Append("AND wa.zonecode = wb.zonecode ")
            sql.Append("INNER JOIN")
            sql.Append("    dbo.irregioncode AS irr ")
            sql.Append("ON")
            sql.Append("    irr.region = wb.class_03 ")
            sql.Append("AND irr.zonecode = wb.zonecode ")
            sql.Append("WHERE")
            sql.Append("    wa.usr_id = @usr ")
            sql.Append("AND wa.res_id = @pwd ")
            sql.Append("AND wa.ZoneCode = @Zone ")
            sql.Append("AND wa.Comp NOT IN ('001','002')")
            Using cmd As New SqlCommand(sql.ToString, con)
                cmd.Parameters.Add("usr", SqlDbType.VarChar, 20).Value = usr
                cmd.Parameters.Add("pwd", SqlDbType.BigInt, 15).Value = pwd
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 20).Value = Zone
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                        If DataRead.HasRows Then
                            If DataRead.Read Then
                                Me.Session.Add("fName", DataRead.GetString(DataRead.GetOrdinal("fullname")))
                                Me.Session.Add("res_id", DataRead.GetInt32(DataRead.GetOrdinal("res_id")))
                                Me.Session.Add("JPos", DataRead.GetString(DataRead.GetOrdinal("job_title")))
                                Me.Session.Add("JDesc", DataRead.GetString(DataRead.GetOrdinal("task")))
                                Me.Session.Add("JCode", DataRead.GetString(DataRead.GetOrdinal("bedrnr")))
                                Me.Session.Add("JCodeName", DataRead.GetString(DataRead.GetOrdinal("bedrnm")))
                                Me.Session.Add("JCodeNo", DataRead.GetString(DataRead.GetOrdinal("regionid")))
                                Me.Session.Add("JRegion", DataRead.GetString(DataRead.GetOrdinal("class_03")))
                                DataRead.Close()
                                result = True
                            End If
                        Else
                            result = "You're not authorized"
                        End If
                    End Using
                Catch ex As Exception
                    Throw
                End Try
            End Using
            Return result
        End Using
    End Function

    Public Function UserZoneCode(ByVal user As String, ByVal Password As String) As String
        Dim sql As String        
        Dim result As String = Nothing
        Using con As New SqlConnection(Me.Session("strConf"))
            sql = "SELECT  zonecode, comp FROM WebAccounts WHERE usr_id = @usr AND res_id = @pw "
            Using Usercmd As New SqlCommand(sql, con)
                Usercmd.Parameters.Add("usr", SqlDbType.VarChar, 20).Value = user
                Usercmd.Parameters.Add("pw", SqlDbType.BigInt, 15).Value = Password
                Usercmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using Reader As SqlDataReader = Usercmd.ExecuteReader(CommandBehavior.SingleRow)
                        If Reader.HasRows() Then
                            Reader.Read()
                            Me.Session.Add("ZCode", Reader.GetString(Reader.GetOrdinal("zonecode")))
                            Me.Session.Add("ZComp", Reader.GetString(Reader.GetOrdinal("comp")))
                            Reader.Close()
                            result = True
                        Else
                            result = "You're not authorized"
                        End If
                    End Using                
                Catch ex As Exception
                    Throw
                Finally
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                End Try
                Return result
            End Using
        End Using
    End Function

    Private Function DivUsersVismin(ByVal jobDesc As String, ByVal Connection As String, ByVal Zone As String, ByVal Comp As String, ByVal Usr As String) As Boolean
        Using con As New SqlConnection(Connection)
            Dim sql As New StringBuilder()
            sql.Append("SELECT DISTINCT task FROM webaccounts ")
            sql.Append("WHERE (task LIKE '%DIVMAN%' OR task LIKE '%DEPTMAN%' OR comp = @Comp ) AND task = @task AND ZoneCode = @Zone AND usr_id = @usr; ")
            'sql.Append("SELECT DISTINCT task FROM webaccounts WHERE task LIKE '%DIVMAN%' AND task = @task AND ZoneCode = @Zone; ")
            'sql.Append("SELECT DISTINCT task FROM webaccounts WHERE task LIKE '%DEPTMAN%' AND task = @task AND ZoneCode = @Zone; ")            
            'sql.Append("SELECT DISTINCT task FROM webaccounts WHERE comp = @Comp AND task = @task AND ZoneCode = @Zone; ")
            Using cmd As New SqlCommand(sql.ToString, con)
                cmd.Parameters.Add("task", SqlDbType.VarChar, 50).Value = jobDesc
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 10).Value = Zone
                cmd.Parameters.Add("Comp", SqlDbType.VarChar, 4).Value = Comp
                cmd.Parameters.Add("usr", SqlDbType.VarChar, 20).Value = Usr
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataReader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataReader.HasRows Then
                            Me.Session.Add("isDivision", True)
                            Return True
                        End If
                    End Using
                Catch ex As Exception
                    Throw
                Finally
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                End Try
            End Using
        End Using
        Return Nothing
    End Function

    Private Function BranchUsers(ByVal Connection As String, ByVal task As String, ByVal Zone As String, ByVal user As String) As String
        Using con As New SqlConnection(Connection)
            Dim sql As New StringBuilder()
            sql.Append("SELECT DISTINCT task FROM irlptl WHERE task = @task AND zonecode = @Zone AND usr_id = @usr ; ")
            sql.Append("SELECT DISTINCT task FROM")
            sql.Append("    (SELECT DISTINCT task,zonecode,usr_id FROM irrcts ")
            sql.Append("UNION ALL")
            sql.Append("    SELECT DISTINCT task,zonecode,usr_id FROM irregionalmanagers ")
            sql.Append("UNION ALL")
            sql.Append("    SELECT DISTINCT task,zonecode,usr_id FROM irareamanagers ")
            sql.Append("UNION ALL")
            sql.Append("    SELECT DISTINCT task,zonecode,usr_id FROM irbranchmanager ")
            sql.Append("UNION ALL")
            sql.Append("    SELECT DISTINCT task,zonecode,usr_id FROM weblpts) AS x ")
            sql.Append("WHERE")
            sql.Append("    task = @task ")
            sql.Append("AND zonecode = @Zone ")
            sql.Append("AND usr_id = @usr; ")            
            Using cmd As New SqlCommand(sql.ToString, con)
                cmd.Parameters.Add("task", SqlDbType.VarChar, 50).Value = task
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 10).Value = Zone
                cmd.Parameters.Add("usr", SqlDbType.VarChar, 20).Value = user
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows Then
                            Me.Session.Add("isDivision", False)
                            Return "~/WorkOrder/Default.aspx"
                        End If
                        DataRead.NextResult()
                        If DataRead.HasRows Then
                            Me.Session.Add("isDivision", False)
                            Return "~/WorkOrderRABLR/Default.aspx"
                        End If
                    End Using
                Catch ex As Exception
                    Throw
                Finally
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                End Try
            End Using
        End Using
        Return Nothing
    End Function

    'Private Function DivMan(ByVal jobDesc As String) As Boolean
    '    Dim mySqlDes As String
    '    Dim ds As DataSet
    '    mySqlDes = "select distinct task from webaccounts where task like '%DIVMAN%';"
    '    ds = Execute_DataSet(mySqlDes)
    '    If Not ds Is Nothing Then
    '        For x = 0 To ds.Tables(0).Rows.Count - 1
    '            If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.ToUpper.Trim Then
    '                Return True
    '            End If
    '        Next
    '    End If
    '    Return False
    'End Function

    'Private Function DeptMan(ByVal jobDesc As String) As Boolean
    '    Dim mySqlDes As String
    '    Dim ds As DataSet
    '    mySqlDes = "select distinct task from webaccounts where task like '%DEPTMAN%';"
    '    ds = Execute_DataSet(mySqlDes)
    '    If Not ds Is Nothing Then
    '        For x = 0 To ds.Tables(0).Rows.Count - 1
    '            If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.ToUpper.Trim Then
    '                Return True
    '            End If
    '        Next
    '    End If
    '    Return False
    'End Function

    'Private Function DivUsers(ByVal jobDesc As String) As Boolean 'Elycode
    '    Dim mySqlDes As String
    '    Dim ds As DataSet
    '    mySqlDes = "select distinct task from webaccounts where comp = '001';"
    '    ds = Execute_DataSet(mySqlDes)
    '    If Not ds Is Nothing Then
    '        For x = 0 To ds.Tables(0).Rows.Count - 1
    '            If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.ToUpper.Trim Then
    '                Return True
    '            End If
    '        Next
    '    End If
    '    Return False
    'End Function

    'Private Function LPTL(ByVal jobDesc As String) As Boolean
    '    Dim mySqlDes As String
    '    Dim ds As DataSet
    '    mySqlDes = "select distinct task from irlptl;"
    '    ds = Execute_DataSet(mySqlDes)
    '    If Not ds Is Nothing Then
    '        For x = 0 To ds.Tables(0).Rows.Count - 1
    '            If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.ToUpper.Trim Then
    '                Return True
    '            End If
    '        Next
    '    End If
    '    Return False
    'End Function

    'Private Function Branch(ByVal jobDesc As String) As Boolean
    '    Dim mySqlDes As String
    '    Dim ds As DataSet
    '    mySqlDes = "select distinct task from irbranchmanager where task = 'BM/BOSMAN';"
    '    ds = Execute_DataSet(mySqlDes)
    '    If Not ds Is Nothing Then
    '        For x = 0 To ds.Tables(0).Rows.Count - 1
    '            If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.ToUpper.Trim Then
    '                Return True
    '            End If
    '        Next
    '    End If
    '    Return False
    'End Function

    'Private Function LPT(ByVal jobDesc As String) As Boolean
    '    Dim mySqlDes As String
    '    Dim ds As DataSet
    '    mySqlDes = "select distinct task from weblpts where task <> 'LPTL/BM/LPT/BOSMAN' and task <> 'ABM/LPTL/LPT';"
    '    ds = Execute_DataSet(mySqlDes)
    '    If Not ds Is Nothing Then
    '        For x = 0 To ds.Tables(0).Rows.Count - 1
    '            If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.ToUpper.Trim Then
    '                Return True
    '            End If
    '        Next
    '    End If
    '    Return False
    'End Function

    'Private Function RCT(ByVal jobDesc As String) As Boolean
    '    Dim mySqlDes As String
    '    Dim ds As DataSet
    '    mySqlDes = "select distinct task from irrcts;"
    '    ds = Execute_DataSet(mySqlDes)
    '    If Not ds Is Nothing Then
    '        For x = 0 To ds.Tables(0).Rows.Count - 1
    '            If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.ToUpper.Trim Then
    '                Return True
    '            End If
    '        Next
    '    End If
    '    Return False
    'End Function

    'Private Function Regional(ByVal jobDesc As String) As Boolean
    '    Dim mySqlDes As String
    '    Dim ds As DataSet
    '    mySqlDes = "select distinct task from irregionalmanagers;"
    '    ds = Execute_DataSet(mySqlDes)
    '    If Not ds Is Nothing Then
    '        For x = 0 To ds.Tables(0).Rows.Count - 1
    '            If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.ToUpper.Trim Then
    '                Return True
    '            End If
    '        Next
    '    End If
    '    Return False
    'End Function

    'Private Function Area(ByVal jobDesc As String) As Boolean
    '    Dim mySqlDes As String
    '    Dim ds As DataSet
    '    mySqlDes = "select distinct task from irareamanagers;"
    '    ds = Execute_DataSet(mySqlDes)
    '    If Not ds Is Nothing Then
    '        For x = 0 To ds.Tables(0).Rows.Count - 1
    '            If jobDesc.ToUpper = ds.Tables(0).Rows(x)(0).ToString.ToUpper.Trim Then
    '                Return True
    '            End If
    '        Next
    '    End If
    '    Return False
    'End Function

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
    '        'MsgBox(ex.Message)
    '        Throw
    '        If Con.State = ConnectionState.Open Then
    '            Con.Close()
    '            Con.Dispose()
    '        End If            
    '    End Try
    'End Function

    'Private Sub MFWEBIniSetup()
    '    Dim ini_Path As String = AppDomain.CurrentDomain.BaseDirectory + "CMMS.ini"
    '    Dim server As String
    '    Dim db As String
    '    Dim uName As String
    '    Dim pWord As String
    '    Dim rdr As New ReadWriteINI
    '    Dim strConf As String

    '    server = rdr.readINI("MF SERVER VISMIN", "SERVER", False, ini_Path)
    '    db = rdr.readINI("MF SERVER VISMIN", "DBNAME", False, ini_Path)
    '    uName = rdr.readINI("MF SERVER VISMIN", "USERNAME", False, ini_Path)
    '    pWord = rdr.readINI("MF SERVER VISMIN", "PASSWORD", False, ini_Path)
    '    strConf = "user id=" & uName & ";password=" & pWord & ";data source=" & server & ";persist security info=False;initial catalog=" & db & " ;pooling=false "
    '    HttpContext.Current.Session("HO") = CStr("VISMIN")

    '    HttpContext.Current.Session("server") = CStr(server)
    '    HttpContext.Current.Session("db") = CStr(db)
    '    HttpContext.Current.Session("pWord") = CStr(pWord)
    '    HttpContext.Current.Session("uName") = CStr(uName)
    '    HttpContext.Current.Session("strConf") = CStr(strConf)
    'End Sub

    'Private Sub CMMSIniSet()
    '    Dim ini_Path As String = AppDomain.CurrentDomain.BaseDirectory + "CMMS.ini"
    '    Dim server As String
    '    Dim db As String
    '    Dim uName As String
    '    Dim pWord As String
    '    Dim rdr As New ReadWriteINI
    '    Dim strCon As String

    '    server = rdr.readINI("CMMS SERVER VISMIN", "SERVER", False, ini_Path)
    '    db = rdr.readINI("CMMS SERVER VISMIN", "DBNAME", False, ini_Path)
    '    uName = rdr.readINI("CMMS SERVER VISMIN", "USERNAME", False, ini_Path)
    '    pWord = rdr.readINI("CMMS SERVER VISMIN", "PASSWORD", False, ini_Path)
    '    strCon = "user id=" & uName & ";password=" & pWord & ";data source=" & server & ";persist security info=False;initial catalog=" & db & " ;pooling=false;Allow User Variables = True "
    '    HttpContext.Current.Session("HO") = CStr("VISMIN")
    '    HttpContext.Current.Session("server") = CStr(server)
    '    HttpContext.Current.Session("db") = CStr(db)
    '    HttpContext.Current.Session("pWord") = CStr(pWord)
    '    HttpContext.Current.Session("uName") = CStr(uName)
    '    HttpContext.Current.Session("strCon") = CStr(strCon)
    'End Sub

    'Private Sub WebProjectsIniSetup()
    '    Dim ini_Path As String = AppDomain.CurrentDomain.BaseDirectory + "CMMS.ini"
    '    Dim server As String
    '    Dim db As String
    '    Dim uName As String
    '    Dim pWord As String
    '    Dim rdr As New ReadWriteINI
    '    Dim strConf As String

    '    server = rdr.readINI("WEBPROJECTS SERVER", "SERVER", False, ini_Path)
    '    db = rdr.readINI("WEBPROJECTS SERVER", "DBNAME", False, ini_Path)
    '    uName = rdr.readINI("WEBPROJECTS SERVER", "USERNAME", False, ini_Path)
    '    pWord = rdr.readINI("WEBPROJECTS SERVER", "PASSWORD", False, ini_Path)
    '    strConf = "user id=" & uName & ";password=" & pWord & ";data source=" & server & ";persist security info=False;initial catalog=" & db & " ;pooling=false "
    '    HttpContext.Current.Session("HO") = CStr("VISMIN")

    '    HttpContext.Current.Session("server") = CStr(server)
    '    HttpContext.Current.Session("db") = CStr(db)
    '    HttpContext.Current.Session("pWord") = CStr(pWord)
    '    HttpContext.Current.Session("uName") = CStr(uName)
    '    HttpContext.Current.Session("strConf") = CStr(strConf)
    'End Sub
    'If Me.Session("ZCode") = "VISMIN" Then
    '    'If Me.Session("ZComp") = "001" Then
    '    '    Dim LoginMsg As String = getUserDivision()
    '    '    If LoginMsg = "True" Then
    '    '        If Me.Session("JDesc") = "MMD-STAFF" Then
    '    '            Response.Redirect("~/DataEntry/Default.aspx")
    '    '            'ElseIf Me.Session("JDesc") = "BOS-CONT" Then
    '    '            'Response.Redirect("~/WorkOrder/Default.aspx")
    '    '        ElseIf DivUsersVismin(Me.Session("JDesc").ToString, Me.Session("strConf").ToString, Me.Session("ZCode").ToString) Or Me.Session("JDesc") = "BOS-CONT" Then
    '    '            Response.Redirect("~/WorkOrder/Default.aspx", False)
    '    '        Else
    '    '            lblError.Text = "User is not registered! Please contact your administrator."
    '    '            txtUserName.Text = Nothing
    '    '            txtPassword.Text = Nothing
    '    '        End If
    '    '    ElseIf LoginMsg = "You're not authorized" Then
    '    '        lblError.Text = "Access Denied! Please check your username and password."
    '    '        txtUserName.Text = Nothing
    '    '        txtPassword.Text = Nothing
    '    '    End If
    '    'Else
    '    '    Dim LoginMsg As String = getUserBranch()
    '    '    If LoginMsg = "True" Then
    '    '        link = BranchUsers(Me.Session("strConf").ToString, Me.Session("JDesc").ToString, Me.Session("ZCode").ToString)
    '    '        If link <> Nothing Then
    '    '            Response.Redirect(link)
    '    '        Else
    '    '            lblError.Text = "User is not registered! Please contact your administrator."
    '    '            txtUserName.Text = Nothing
    '    '            txtPassword.Text = Nothing
    '    '        End If
    '    '    ElseIf LoginMsg = "You're not authorized" Then
    '    '        lblError.Text = "Access Denied! Please check your username and password."
    '    '        txtUserName.Text = Nothing
    '    '        txtPassword.Text = Nothing
    '    '    End If
    '    'End If
    'Else
    '    If Me.Session("ZComp") = "002" Then
    '        Dim LoginMsg As String = getUserDivision()
    '        If LoginMsg = "True" Then
    '            If Me.Session("JDesc") = "MMD-STAFF" Then
    '                Response.Redirect("~/DataEntry/Default.aspx")
    '                'ElseIf Me.Session("JDesc") = "BOS-CONT" Then
    '                'Response.Redirect("~/WorkOrder/Default.aspx")
    '            ElseIf DivUsersVismin(Me.Session("JDesc").ToString, Me.Session("strConf").ToString, Me.Session("ZCode").ToString, Me.Session("ZComp").Timeout) Or Me.Session("JDesc") = "BOS-CONT" Then
    '                Response.Redirect("~/WorkOrder/Default.aspx", False)
    '                'ElseIf DivMan(Me.Session("JDesc")) = True Then
    '                '    Response.Redirect("~/WorkOrder/Default.aspx")
    '                'ElseIf DeptMan(Me.Session("JDesc")) = True Then
    '                '    Response.Redirect("~/WorkOrder/Default.aspx")
    '                'ElseIf DivUsers(Me.Session("JDesc")) = True Then
    '                '    Response.Redirect("~/WorkOrder/Default.aspx")
    '            Else
    '                lblError.Text = "User is not registered! Please contact your administrator."
    '                txtUserName.Text = ""
    '            End If
    '        ElseIf LoginMsg = "You're not authorized" Then
    '            lblError.Text = "Access Denied! Please check your username and password."
    '            txtUserName.Text = ""
    '        End If
    '    Else
    '        Dim LoginMsg As String = getUserBranch()
    '        If LoginMsg = "True" Then
    '            link = BranchUsers(Me.Session("strConf").ToString, Me.Session("JDesc").ToString, Me.Session("ZCode").ToString)
    '            If link <> Nothing Then
    '                Response.Redirect(link, False)
    '            Else
    '                lblError.Text = "User is not registered! Please contact your administrator."
    '                Me.txtUserName.Text = Nothing
    '                Me.txtPassword.Text = Nothing
    '            End If
    '            'If LPTL(Me.Session("JDesc")) = True Then
    '            '    Response.Redirect("~/WorkOrder/Default.aspx")
    '            'ElseIf Branch(Me.Session("JDesc")) = True Then
    '            '    Response.Redirect("~/WorkOrderRABLR/Default.aspx")
    '            'ElseIf LPT(Me.Session("JDesc")) = True Then
    '            '    Response.Redirect("~/WorkOrderRABLR/Default.aspx")
    '            'ElseIf RCT(Me.Session("JDesc")) = True Then
    '            '    Response.Redirect("~/WorkOrderRABLR/Default.aspx")
    '            'ElseIf Regional(Me.Session("JDesc")) = True Then
    '            '    Response.Redirect("~/WorkOrderRABLR/Default.aspx")
    '            'ElseIf Area(Me.Session("JDesc")) = True Then
    '            '    Response.Redirect("~/WorkOrderRABLR/Default.aspx")
    '            'Else
    '            '    lblError.Text = "User is not registered! Please contact your administrator."
    '            '    txtUserName.Text = ""
    '            'End If
    '        ElseIf LoginMsg = "You're not authorized" Then
    '            lblError.Text = "Access Denied! Please check your username and password."
    '            txtUserName.Text = ""
    '        End If
    '    End If
    'End If
End Class