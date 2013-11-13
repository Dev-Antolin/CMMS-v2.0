﻿'Imports CrystalDecisions.CrystalReports.Engine
'Imports System.Data
'Imports System.Data.SqlClient
'Imports MySql.Data.MySqlClient
Partial Class Report_RegionRPT
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Me.Session("RegionAssetP") Is Nothing Then
            Response.Redirect("RegionProf.aspx")
        Else
            rptViewer.ReportSource = Me.Session("RegionAssetP")
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

    'Private Sub GenerateReport()
    '    Dim myReport As New ReportDocument        
    '    myReport.Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\RegionAssetProfile.rpt")
    '    myReport.SetDataSource(Me.Session("RegionAsset"))
    '    rptViewer.ReportSource = myReport
    '    myReport.SetParameterValue("DateFrom", Me.Session("ByDateFrom")) 'CDate(ByDateFrom).ToString("yyyy-MM-dd"))
    '    myReport.SetParameterValue("DateTo", Me.Session("ByDateTo")) 'CDate(ByDateTo).ToString("yyyy-MM-dd"))
    '    myReport.SetParameterValue("RegName", Me.Session("ByReg")) ' ByReg)
    '    myReport.SetParameterValue("ManName", Me.Session("ByRegMan")) 'ByRegMan)
    '    Dim ds As DataSet
    '    Dim dt As New DataTable
    '    Dim myRPT As String
    '    Dim ByDateFrom As String
    '    Dim ByDateTo As String
    '    Dim ByReg As String
    '    Dim ByRegMan As String
    '    ByDateFrom = Me.Session("ByDateFrom")
    '    ByDateTo = Me.Session("ByDateTo")
    '    ByReg = Me.Session("ByReg")
    '    ByRegMan = Me.Session("ByRegMan")
    '    Try
    '        'myRPT = "exec SPRegAssProf '" & ByDateFrom.Trim & " 00:00:00','" & ByDateTo.Trim & " 23:59:59','" & ByReg.Trim & "'"
    '        '/-------------------------------------REGION RPT
    '        'Dim xx As String = Me.Session("ZCode") 'LATEST CODE ADDED ELY 5-4-2012
    '        'myRPT = "exec SPRegAssProf '" & ByDateFrom.Trim & " 00:00:00','" & ByDateTo.Trim & " 23:59:59','" & ByReg.Trim & "'" ','" + xx + "'"  'LATEST CODE ADDED ELY 5-4-2012
    '        ''\-------------------------------------REGION RPT         
    '        'ds = Execute_DataSet(myRPT)
    '        'If Not ds Is Nothing Then
    '        '    dt = ds.Tables(0)
    '        'End If
    '        myReport.Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\RegionAssetProfile.rpt")
    '        myReport.SetDataSource(Me.Session("RegionAsset"))
    '        rptViewer.ReportSource = myReport
    '        myReport.SetParameterValue("DateFrom", Me.Session("ByDateFrom")) 'CDate(ByDateFrom).ToString("yyyy-MM-dd"))
    '        myReport.SetParameterValue("DateTo", Me.Session("ByDateTo")) 'CDate(ByDateTo).ToString("yyyy-MM-dd"))
    '        myReport.SetParameterValue("RegName", Me.Session("ByReg")) ' ByReg)
    '        myReport.SetParameterValue("ManName", Me.Session("ByRegMan")) 'ByRegMan)
    '    Catch ex As Exception
    '        Me.Session.Add("ErrorReport", ex.Message)
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

    'Public Function Execute_DataSetCMMS(ByVal as_mysql As String) As DataSet
    '    Dim Con As New MySqlConnection
    '    Dim Com As New MySqlCommand
    '    Dim sqlAdapter As MySqlDataAdapter
    '    Dim sqlDataset As New DataSet

    '    Execute_DataSetCMMS = Nothing
    '    Try
    '        Try
    '            Con.ConnectionString = Me.Session("strCon")
    '            If Con.State = ConnectionState.Closed Then
    '                Con.Open()
    '            End If
    '        Catch
    '        End Try
    '        sqlAdapter = New MySqlDataAdapter(as_mysql, Con)
    '        sqlAdapter.Fill(sqlDataset)
    '        If Not sqlDataset Is Nothing Then
    '            If sqlDataset.Tables(0).Rows.Count <> 0 Then
    '                Execute_DataSetCMMS = sqlDataset
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

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            CheckUser()
            CheckLogin()
        End If
    End Sub
End Class
