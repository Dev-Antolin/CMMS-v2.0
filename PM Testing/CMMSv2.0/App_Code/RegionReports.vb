Option Strict On
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports CrystalDecisions.CrystalReports.Engine

Public Class RegionReports
    Implements IRegion, IRegionRpt

    Public Function getRegions(ByVal constr As String, ByVal zone As String) As System.Collections.Generic.List(Of String) Implements IRegion.getRegions
        Dim sql As New StringBuilder()
        Dim ListOfRegion As New List(Of String)
        sql.Append("SELECT")
        sql.Append("    DISTINCT wb.Class_03 ")
        sql.Append("FROM")
        sql.Append("    OPENQUERY(cmms,'SELECT Bc_Code,Zone_Code FROM cmms.cmms_entry_masterheader; ') AS header ")
        sql.Append("INNER JOIN")
        sql.Append("    WebBranches AS wb ")
        sql.Append("ON")
        sql.Append("    header.Bc_Code = wb.bedrnr ")
        sql.Append("AND header.Zone_Code = wb.zonecode ")
        sql.Append("WHERE")
        sql.Append("    header.Zone_Code = @Zone ")
        sql.Append("AND wb.Class_03 NOT IN ( 'HO' , '<none>') ")
        sql.Append("ORDER BY")
        sql.Append("    wb.Class_03 ASC; ")
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(sql.ToString, con)
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 10).Value = zone
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows Then
                            ListOfRegion.Add("")
                            Dim region As Int32 = DataRead.GetOrdinal("Class_03")
                            While DataRead.Read
                                ListOfRegion.Add(DataRead.GetString(region))
                            End While
                            DataRead.Close()
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
        Return ListOfRegion
    End Function

    Public Sub GenerateRegionRpt(ByVal Region As String, ByVal Manager As String, ByVal Connection As String, ByVal Zone As String) Implements IRegionRpt.GenerateRegionRpt
        Dim dt As New DataTable
        Dim ds As New DataSet
        Using con As New SqlConnection(Connection)
            Using cmd As New SqlCommand("SPRegAssProf", con)
                cmd.Parameters.Add("region", SqlDbType.VarChar, 50).Value = Region
                cmd.Parameters.Add("zonecode", SqlDbType.VarChar, 10).Value = Zone
                cmd.CommandType = CommandType.StoredProcedure
                Try
                    con.Open()
                    Using DataAdapter As New SqlDataAdapter(cmd)
                        DataAdapter.Fill(ds)
                        If ds.Tables.Count > 0 Then
                            Dim myReport As New ReportDocument
                            myReport.Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\RegionAssetProfile.rpt")
                            myReport.SetDataSource(ds.Tables(0))
                            myReport.SetParameterValue("DateFrom", ds.Tables(1).Rows(0)(0))
                            myReport.SetParameterValue("DateTo", ds.Tables(1).Rows(0)(1))
                            myReport.SetParameterValue("RegName", Region)
                            myReport.SetParameterValue("ManName", Manager)
                            HttpContext.Current.Session.Add("RegionAssetP", myReport)
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
    End Sub

    Public Function getRegionalManager(ByVal Connection As String, ByVal Zone As String, ByVal Region As String) As String Implements IRegionRpt.getRegionalManager
        Dim result As String = Nothing
        Dim sql As New StringBuilder()
        sql.Append("SELECT DISTINCT")
        sql.Append("    UPPER(RTRIM(wa.fullname)) AS fullname ")
        sql.Append("FROM")
        sql.Append("    WebAccounts AS wa ")
        sql.Append("INNER JOIN")
        sql.Append("    WebBranches AS wb ")
        sql.Append("ON")
        sql.Append("    wa.comp = wb.bedrnr ")
        sql.Append("WHERE")
        sql.Append("    wa.zonecode = @Zone ")
        sql.Append("AND wb.Class_03 = @Region ")
        sql.Append("AND wa.task = 'Regional Man' ")
        sql.Append("ORDER BY")
        sql.Append("    wa.fullname DESC; ")
        Using con As New SqlConnection(Connection)
            Using cmd As New SqlCommand(sql.ToString, con)
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 10).Value = Zone
                cmd.Parameters.Add("Region", SqlDbType.VarChar, 30).Value = Region
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    result = CStr(cmd.ExecuteScalar)
                    If result = Nothing Then
                        result = "No Regional Manager assigned in this region."
                    End If
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

End Class

Public Class BranchReports
    Implements IBranch

    Public Function getArea(ByVal zone As String, ByVal constr As String, ByVal Region As String) As System.Collections.Generic.List(Of String) Implements IArea.getArea
        Dim ListOfArea As New List(Of String)
        Dim sql As New StringBuilder()
        sql.Append("SELECT DISTINCT")
        sql.Append("    wb.Class_04 ")
        sql.Append("FROM")
        sql.Append("    OPENQUERY(cmms,'SELECT Bc_Code,Bc_Name,Zone_Code FROM cmms.cmms_entry_masterheader; ') AS Header ")
        sql.Append("INNER JOIN")
        sql.Append("    WebBranches AS wb ")
        sql.Append("ON")
        sql.Append("    Header.Bc_Code = wb.bedrnr ")
        sql.Append("AND Header.Zone_Code= wb.ZoneCode ")
        sql.Append("WHERE")
        sql.Append("    Header.Zone_Code = @Zone ")
        sql.Append("AND wb.Class_03 NOT IN ('HO','<none>') ")
        sql.Append("AND wb.Class_03 = @Region ")
        sql.Append("ORDER BY")
        sql.Append("    wb.Class_04 ASC; ")
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(sql.ToString, con)
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 6).Value = zone
                cmd.Parameters.Add("Region", SqlDbType.VarChar, 30).Value = Region
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows Then
                            ListOfArea.Add("")
                            Dim Area As Int32 = DataRead.GetOrdinal("Class_04")
                            While DataRead.Read
                                ListOfArea.Add(DataRead.GetString(Area))
                            End While
                            DataRead.Close()
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
        Return ListOfArea
    End Function

    Public Function getRegions(ByVal constr As String, ByVal zone As String) As System.Collections.Generic.List(Of String) Implements IRegion.getRegions
        Dim sql As New StringBuilder()
        Dim ListOfRegion As New List(Of String)
        sql.Append("SELECT DISTINCT")
        sql.Append("    UPPER(RTRIM(wb.Class_03)) AS Class_03 ")
        sql.Append("FROM")
        sql.Append("    OPENQUERY(cmms,'SELECT Bc_Code,Zone_Code FROM cmms.cmms_entry_masterheader; ') AS header ")
        sql.Append("INNER JOIN")
        sql.Append("    WebBranches AS wb ")
        sql.Append("ON")
        sql.Append("    header.Bc_Code = wb.bedrnr ")
        sql.Append("AND header.Zone_Code = wb.zonecode ")
        sql.Append("WHERE")
        sql.Append("    header.Zone_Code = @Zone ")
        sql.Append("AND wb.Class_03 NOT IN ( 'HO' , '<none>') ")
        sql.Append("ORDER BY")
        sql.Append("    wb.Class_03 ASC; ")
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand(sql.ToString, con)
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 10).Value = zone
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows Then
                            ListOfRegion.Add("")
                            Dim Region As Int32 = DataRead.GetOrdinal("Class_03")
                            While DataRead.Read
                                ListOfRegion.Add(DataRead.GetString(Region))
                            End While
                            DataRead.Close()
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
        Return ListOfRegion
    End Function

    Public Function getBranches(ByVal Zone As String, ByVal Region As String, ByVal Area As String, ByVal Connection As String) As System.Collections.Generic.List(Of String) Implements IBranch.getBranches
        Dim ListOfBranch As New List(Of String)
        Dim sql As New StringBuilder
        sql.Append("SELECT DISTINCT")
        sql.Append("    UPPER(RTRIM(wb.bedrnm)) AS bedrnm ")
        sql.Append("FROM")
        sql.Append("    OPENQUERY(cmms,'SELECT Bc_Code,Bc_Name,Zone_Code FROM cmms.cmms_entry_masterheader; ') AS Header ")
        sql.Append("INNER JOIN")
        sql.Append("    WebBranches AS wb ")
        sql.Append("ON")
        sql.Append("    Header.Bc_Code = wb.bedrnr ")
        sql.Append("AND Header.Zone_Code= wb.ZoneCode ")
        sql.Append("WHERE")
        sql.Append("    Header.Zone_Code = @Zone ")
        sql.Append("AND wb.Class_03 NOT IN ('HO','<none>') ")
        sql.Append("AND wb.Class_03 = @Region ")
        sql.Append("AND wb.Class_04 = @Area ")
        sql.Append("ORDER BY")
        sql.Append("    wb.bedrnm ASC; ")
        Using con As New SqlConnection(Connection)
            Using cmd As New SqlCommand(sql.ToString, con)
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 6).Value = Zone
                cmd.Parameters.Add("Region", SqlDbType.VarChar, 30).Value = Region
                cmd.Parameters.Add("Area", SqlDbType.VarChar, 30).Value = Area
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows Then
                            ListOfBranch.Add("")
                            Dim branch As Int32 = DataRead.GetOrdinal("bedrnm")
                            While DataRead.Read
                                ListOfBranch.Add(DataRead.GetString(branch))
                            End While
                            DataRead.Close()
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
        Return ListOfBranch
    End Function

    Public Function getBranchEmployee(ByVal Zone As String, ByVal Branch As String, ByVal Connection As String) As System.Collections.Generic.List(Of String) Implements IBranch.getBranchEmployee
        Dim sql As New StringBuilder()
        Dim ListOfEmployee As New List(Of String)
        sql.Append("SELECT DISTINCT")
        sql.Append("    UPPER(RTRIM(wa.fullname)) AS FullName ")
        sql.Append("FROM OPENQUERY(cmms,'SELECT Bc_Code,Bc_Name,Emp_Name_Assigned FROM cmms.cmms_entry_masterheader; ') AS Header ")
        sql.Append("INNER JOIN")
        sql.Append("    WebAccounts wa ")
        sql.Append("ON")
        sql.Append("    wa.fullname = Header.Emp_Name_Assigned ")
        sql.Append("INNER JOIN")
        sql.Append("    WebBranches wb ")
        sql.Append("ON")
        sql.Append("    wa.comp = wb.bedrnr ")
        sql.Append("WHERE")
        sql.Append("    wa.zonecode = @Zone ")
        sql.Append("AND wb.bedrnm = @Bc_Name ")
        sql.Append("ORDER BY")
        sql.Append("    wa.fullname ASC; ")

        Using con As New SqlConnection(Connection)            
            Using cmd As New SqlCommand(sql.ToString, con)
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 6).Value = Zone
                cmd.Parameters.Add("Bc_Name", SqlDbType.VarChar, 60).Value = Branch
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows Then
                            ListOfEmployee.Add("")
                            Dim Employee As Int32 = DataRead.GetOrdinal("FullName")
                            While DataRead.Read
                                ListOfEmployee.Add(DataRead.GetString(Employee))
                            End While
                            DataRead.Close()
                            ListOfEmployee.Add("ALL EMPLOYEE")
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
        Return ListOfEmployee
    End Function

    Public Sub getAllBrachEmployeeReport(ByVal Bc_Name As String, ByVal Connection As String) Implements IBranch.getAllBrachEmployeeReport
        Dim ds As New DataSet
        Using con As New MySqlConnection(Connection)
            Using cmd As New MySqlCommand("SPBraAllAssProf", con)
                cmd.Parameters.Add("ByBra", MySqlDbType.VarChar, 60).Value = Bc_Name
                cmd.CommandType = CommandType.StoredProcedure
                Try
                    con.Open()
                    Using DataAdap As New MySqlDataAdapter(cmd)
                        DataAdap.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim myReport As New ReportDocument
                            With myReport
                                .Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\BranchAllAssetProfile.rpt")
                                .SetDataSource(ds.Tables(0))
                                .SetParameterValue("DateFrom", ds.Tables(1).Rows(0)(0))
                                .SetParameterValue("DateTo", ds.Tables(1).Rows(0)(1))
                                .SetParameterValue("Branch", Bc_Name)
                            End With
                            System.Web.HttpContext.Current.Session.Add("AllBranchAssetP", myReport)
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
    End Sub

    Public Sub getBranchEmployeeRerpot(ByVal Bc_Name As String, ByVal Bc_Emp As String, ByVal Connection As String) Implements IBranch.getBranchEmployeeRerpot
        Dim dt As New DataTable
        Dim ds As New DataSet
        Using con As New MySqlConnection(Connection)
            Using cmd As New MySqlCommand("SPBraAssProf", con)
                cmd.Parameters.Add("ByBra", MySqlDbType.VarChar, 60).Value = Bc_Name
                cmd.Parameters.Add("ByBraName", MySqlDbType.VarChar, 64).Value = Bc_Emp
                cmd.CommandType = CommandType.StoredProcedure
                Try
                    con.Open()
                    Using DataAdapter As New MySqlDataAdapter(cmd)
                        DataAdapter.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim myReport As New ReportDocument
                            With myReport
                                .Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\BranchAssetProfile.rpt")
                                .SetDataSource(ds.Tables(0))
                                .SetParameterValue("DateFrom", ds.Tables(1).Rows(0)(0))
                                .SetParameterValue("DateTo", ds.Tables(1).Rows(0)(1))
                                .SetParameterValue("Branch", Bc_Name)
                                .SetParameterValue("EmpName", Bc_Emp)
                            End With
                            System.Web.HttpContext.Current.Session.Add("BranchAssetP", myReport)
                        End If                        
                    End Using
                    'Using DataRead As MySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    '    If DataRead.HasRows() Then
                    '        dt.Load(DataRead)
                    '        Dim myReport As New ReportDocument
                    '        With myReport
                    '            .Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\BranchAssetProfile.rpt")
                    '            .SetDataSource(dt)
                    '            .SetParameterValue("DateFrom", "2013-01-01")
                    '            .SetParameterValue("DateTo", "2013-01-01")
                    '            .SetParameterValue("Branch", Bc_Name)
                    '            .SetParameterValue("EmpName", Bc_Emp)
                    '        End With
                    '        System.Web.HttpContext.Current.Session.Add("BranchAssetP", myReport)
                    '    End If
                    'End Using
                Catch ex As Exception
                    Throw
                Finally
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                End Try
            End Using
        End Using
    End Sub

End Class

Public Class DivisionReports
    Implements IDivision

    Public Function getDivisions(ByVal Zone As String, ByVal Connection As String) As System.Collections.Generic.List(Of String) Implements IDivision.getDivisions
        Dim ListOfDivision As New List(Of String)
        Using con As New MySqlConnection(Connection)
            Dim sql As New StringBuilder()
            sql.Append("SELECT DISTINCT Bc_Name FROM cmms.cmms_entry_masterheader ")
            sql.Append("WHERE NOT IsNumeric(Bc_Code) ")
            sql.Append("AND zone_Code = @Zone ")
            sql.Append("ORDER BY Bc_Name ASC; ")
            Using cmd As New MySqlCommand(sql.ToString, con)
                cmd.Parameters.Add("Zone", MySqlDbType.VarChar, 10).Value = Zone
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataReader As MySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataReader.HasRows Then
                            ListOfDivision.Add("")
                            Dim Div As Int32 = DataReader.GetOrdinal("Bc_Name")
                            While DataReader.Read
                                ListOfDivision.Add(DataReader.GetString(Div))
                            End While
                            DataReader.Close()
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
        Return ListOfDivision
    End Function

    Public Function DivisionManager(ByVal Division As String, ByVal Zone As String, ByVal Connection As String) As String Implements IDivision.DivisionManager
        Dim sql As New StringBuilder()
        Using con As New SqlConnection(Connection)
            sql.Append("SELECT DISTINCT")
            sql.Append("    RTRIM(fullname) AS fullname ")
            sql.Append("FROM")
            sql.Append("    WebAccounts wa ")
            sql.Append("INNER JOIN")
            sql.Append("    irdivision ird ")
            sql.Append("ON")
            sql.Append("    wa.costcenter = ird.costcenter ")
            sql.Append("WHERE")
            sql.Append("    ird.division = @Division ")
            If Division = "GMO" Then
                sql.Append("AND wa.task LIKE '%GENMAN%' ")
            Else
                sql.Append("AND wa.task LIKE '%DIVMAN%' ")
            End If
            sql.Append("AND wa.zonecode = @Zone; ")

            Using cmd As New SqlCommand(sql.ToString, con)
                cmd.Parameters.Add("Division", SqlDbType.VarChar, 10).Value = Division
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 10).Value = Zone
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Dim DivMan As String = CStr(cmd.ExecuteScalar())
                    If DivMan = Nothing Then
                        DivMan = Nothing
                    End If
                    Return DivMan
                Catch ex As Exception
                    Throw
                Finally
                    If con.State = ConnectionState.Open Then
                        con.Close()
                    End If
                End Try
            End Using
        End Using
    End Function

    Public Function AllDivManRpt(ByVal Connection As String) As CrystalDecisions.CrystalReports.Engine.ReportDocument Implements IDivision.AllDivManRpt
        Dim ds As New DataSet
        Dim Report As New ReportDocument
        Using con As New MySqlConnection(Connection)
            Using cmd As New MySqlCommand("SPDivAssProf", con)
                cmd.CommandType = CommandType.StoredProcedure
                Try
                    con.Open()
                    Using DataAdapter As New MySqlDataAdapter(cmd)
                        DataAdapter.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            With Report
                                .Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\DivisionAssetProfile.rpt")
                                .SetDataSource(ds.Tables(0))
                                .SetParameterValue("DateFrom", ds.Tables(1).Rows(0)(0))
                                .SetParameterValue("DateTo", ds.Tables(1).Rows(0)(1))
                            End With
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
        Return Report
    End Function

    Public Function ByDivManRpt(ByVal Connection As String, ByVal Division As String, ByVal DivisionManager As String) As CrystalDecisions.CrystalReports.Engine.ReportDocument Implements IDivision.ByDivManRpt
        Dim ds As New DataSet
        Dim Report As New ReportDocument
        Using con As New MySqlConnection(Connection)
            Using cmd As New MySqlCommand("SPBYDivAssProf", con)
                cmd.Parameters.Add("ByDiv", MySqlDbType.VarChar, 5, CStr(ParameterDirection.Input)).Value = Division
                cmd.CommandType = CommandType.StoredProcedure
                Try
                    con.Open()
                    Using DataAdapter As New MySqlDataAdapter(cmd)
                        DataAdapter.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            With Report
                                .Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\AssetProfilebyDivision.rpt")
                                .SetDataSource(ds.Tables(0))
                                .SetParameterValue("DateFrom", ds.Tables(1).Rows(0)(0))
                                .SetParameterValue("DateTo", ds.Tables(1).Rows(0)(1))
                                .SetParameterValue("DivName", Division)
                                .SetParameterValue("ManName", DivisionManager)
                            End With
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
        Return Report
    End Function

    Public Sub CheckAttachment(ByVal AssetNo As String, ByVal AAssetNo As String, ByVal Connection As String) Implements IDivision.CheckAttachment
        Using con As New MySqlConnection(Connection)
            Dim sql As New StringBuilder()
            sql.Append("DELETE FROM cmms_entry_attachfiles WHERE Asset_Inv_No = @AssetNo")
            sql.Append("SELECT Asset_Inv_No FROM cmms_entry_attachfiles WHERE Asset_Inv_No = @AssetNo;")
            sql.Append("DELETE FROM cmms_entry_attachfiles WHERE Asset_Inv_No = @AssetNo")
            sql.Append("SELECT Asset_Inv_No FROM cmms_entry_attachfiles WHERE Asset_Inv_No = @AssetNo;")
            Using cmd As New MySqlCommand(sql.ToString, con)
                cmd.Parameters.Add("AssetNo", MySqlDbType.VarChar, 20)
                cmd.CommandType = CommandType.Text
                Try
                    Dim i As Int32 = cmd.ExecuteNonQuery
                Catch ex As Exception
                    Throw
                End Try
            End Using
        End Using
    End Sub
End Class