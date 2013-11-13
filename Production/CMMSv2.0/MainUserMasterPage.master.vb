Imports INI_DLL
Imports MYSQLDB_DLL
Imports MySql.Data.MySqlClient

Imports System.Collections.Generic
Imports System.Web.Services
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Partial Class MainUserMasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Request.UserAgent.IndexOf("AppleWebKit") > 0 Then
            Request.Browser.Adapters.Clear()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load        
        Select Case Me.Session("click")
            Case "DEM"
                Me.DEM.ForeColor = Drawing.Color.Orange
                Me.DEM.BackColor = Drawing.Color.Transparent
                Exit Select
            Case "WOM"
                Me.WOM.ForeColor = Drawing.Color.Orange
                Me.WOM.BackColor = Drawing.Color.Transparent
                Exit Select
            Case "RAF"
                Me.RAF.ForeColor = Drawing.Color.Orange
                Me.RAF.BackColor = Drawing.Color.Transparent
                Exit Select
        End Select
        If Me.Session("JDesc") = "MMD-STAFF" Then
            Me.RAF.Visible = True
            Me.DEM.Visible = True
            Me.lblDEM.Visible = True
            Me.lblRAF.Visible = True
        ElseIf CMMSuser(Me.Session("JDesc").ToString, Me.Session("strConf").ToString) Then
            If Me.Session("JDesc").ToString.Contains("LPTL") Or Me.Session("JDesc").ToString = "MIS-HELPDESK" Then
                Me.RAF.Visible = True
            Else
                Me.RAF.Visible = False
            End If
            Me.DEM.Visible = False
            Me.lblDEM.Visible = False
            Me.lblRAF.Visible = False
        End If
        lblLoginUser.Text = "Welcome, " & StrConv(Me.Session("fName"), VbStrConv.ProperCase).ToUpper
    End Sub

    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        HttpContext.Current.Session.Abandon()
        Response.Redirect("~/login.aspx")
    End Sub

    <WebMethod()> _
    Public Sub KillSession()
        HttpContext.Current.Session.Abandon()
    End Sub

    Private Function CMMSuser(ByVal JobDesc As String, ByVal Connection As String) As Boolean
        Dim sql As New StringBuilder
        sql.Append("select distinct task from webaccounts where task like '%BOS-CONT%' ")
        sql.Append("or task like '%DIVMAN%' or task like '%DEPTMAN%' AND task = @task; ")
        sql.Append("SELECT DISTINCT task FROM webaccounts ")
        sql.Append("WHERE task LIKE '%/BM/%' OR task LIKE '%Regional%' OR task LIKE '%Area%' ")
        sql.Append("OR task LIKE '%LPT%' OR task LIKE '%RCT-A%' OR task LIKE '%BM/BOSMAN%' AND task = @task; ")
        sql.Append("SELECT DISTINCT task FROM webaccounts WHERE comp IN ('001','002') AND task = @task; ")
        Using con As New SqlConnection(Connection)
            Using cmd As New SqlCommand(sql.ToString, con)
                cmd.Parameters.Add("task", SqlDbType.VarChar, 20).Value = JobDesc
                Try
                    con.Open()
                    Using DataReader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        Do While DataReader.NextResult
                            If DataReader.HasRows Then
                                Return True
                            End If
                        Loop
                    End Using
                Catch ex As Exception
                    Throw
                End Try
            End Using
        End Using
        Return False
    End Function

End Class