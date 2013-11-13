Option Strict On
Imports System
Imports System.Data
Imports System.Data.SqlClient
Public Class AreaReports
    Implements IArea, IAreaRpt    

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
            Using cmd As New SqlCommand(sql.ToString(), con)
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 10).Value() = zone
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows() Then
                            ListOfRegion.Add("")
                            Dim Region As Int32 = DataRead.GetOrdinal("Class_03")
                            While DataRead.Read()
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
            Using cmd As New SqlCommand(sql.ToString(), con)
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 6).Value() = zone
                cmd.Parameters.Add("Region", SqlDbType.VarChar, 30).Value() = Region
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows() Then
                            ListOfArea.Add("")
                            Dim Area As Int32 = DataRead.GetOrdinal("Class_04")
                            While DataRead.Read()
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

    Public Function getAreaManager(ByVal Zone As String, ByVal Area As String, ByVal MsConstr As String) As String Implements IAreaRpt.getAreaManager
        Dim sql As New StringBuilder()
        Dim result As String = Nothing
        sql.Append("SELECT DISTINCT")
        sql.Append("    UPPER(RTRIM(wa.fullname)) AS fullname ")
        sql.Append("FROM")
        sql.Append("    WebAccounts wa ")
        sql.Append("INNER JOIN")
        sql.Append("    WebBranches wb ")
        sql.Append("ON")
        sql.Append("    wa.comp = wb.bedrnr ")
        sql.Append("WHERE")
        sql.Append("    wa.zonecode = @Zone ")
        sql.Append("AND wb.Class_04 = @Area ")
        sql.Append("AND wa.task = 'AREA Manager' ")
        sql.Append("ORDER BY")
        sql.Append("    wa.fullname DESC; ")
        Using con As New SqlConnection(MsConstr)
            Using cmd As New SqlCommand(sql.ToString(), con)
                cmd.Parameters.Add("Zone", SqlDbType.VarChar, 10, CStr(ParameterDirection.Input)).Value() = Zone
                cmd.Parameters.Add("Area", SqlDbType.VarChar, 30, CStr(ParameterDirection.Input)).Value() = Area
                cmd.CommandType = CommandType.Text
                Try
                    con.Open()
                    result = CStr(cmd.ExecuteScalar())
                    If result = Nothing Then
                        Return "No Area Manager assigend in this region."
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

    Public Sub GenerateAreaRpt(ByVal Region As String, ByVal Area As String, ByVal Zone As String, ByVal Connection As String, Optional ByVal AreaMan As String = Nothing) Implements IAreaRpt.GenerateAreaRpt
        Dim ds As New DataSet
        Using con As New SqlConnection(Connection)
            Using cmd As New SqlCommand("SPAreAssProf", con)
                cmd.Parameters.Add("region", SqlDbType.VarChar, 30, CStr(ParameterDirection.Input)).Value = Region
                cmd.Parameters.Add("area", SqlDbType.VarChar, 30, CStr(ParameterDirection.Input)).Value = Area
                cmd.Parameters.Add("zonecode", SqlDbType.VarChar, 10, CStr(ParameterDirection.Input)).Value = Zone
                cmd.CommandType = CommandType.StoredProcedure
                Try
                    con.Open()
                    Using DataAdapter As New SqlDataAdapter(cmd)
                        DataAdapter.Fill(ds)
                        If ds.Tables(0).Rows.Count > 0 Then
                            Dim myReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                            With myReport
                                .Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\AreaAssetProfile.rpt")
                                .SetDataSource(ds.Tables(0))
                                .SetParameterValue("DateFrom", ds.Tables(1).Rows(0)(0))
                                .SetParameterValue("DateTo", ds.Tables(1).Rows(0)(1))                                
                                .SetParameterValue("AreaName", Area)
                                .SetParameterValue("RegName", Region)
                                .SetParameterValue("ManName", AreaMan)
                            End With
                            HttpContext.Current.Session.Add("AreaAssetP", myReport)
                        End If
                    End Using
                    'Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    '    If DataRead.HasRows Then
                    '        dt.Load(DataRead)
                    '        Dim myReport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
                    '        With myReport
                    '            .Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\AreaAssetProfile.rpt")
                    '            .SetDataSource(dt)
                    '            .SetParameterValue("DateFrom", "2013-01-01")
                    '            .SetParameterValue("DateTo", "2013-01-01")
                    '            .SetParameterValue("AreaName", Area)
                    '            .SetParameterValue("RegName", Region)
                    '            .SetParameterValue("ManName", AreaMan)
                    '        End With                                             
                    '        HttpContext.Current.Session.Add("AreaAssetP", myReport)
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
