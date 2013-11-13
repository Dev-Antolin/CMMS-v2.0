Imports CrystalDecisions.CrystalReports.Engine
Imports System.Data
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Partial Class Report_MaintenanceHistoryRpt
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        GenerateReport()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Disable Cache
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        CheckLogin()
        If Not Page.IsPostBack Then
            Me.Session("Click") = "AM"
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

    Private Sub GenerateReport()
        Dim myReport As New ReportDocument
        Dim ds As DataSet
        Dim dt As New DataTable
        Dim myRPT As String
        Dim DateFrom As String
        Dim DateTo As String
        Dim ByBra As String
        Dim ByBraName As String
        DateFrom = Me.Session("BraDateFrom")
        DateTo = Me.Session("BraDateTo")
        ByBra = Me.Session("ByBra")
        ByBraName = Me.Session("ByBraName")

        Try
            myRPT = "CALL SPBraMainHistory('" & DateFrom & "','" & DateTo & "','" & ByBra & "','" & ByBraName & "');"
            ds = Execute_DataSetCMMS(myRPT)
            If Not ds Is Nothing Then
                dt = ds.Tables(0)
            End If
            myReport.Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\BranchMaintenHis.rpt")
            myReport.SetDataSource(dt)
            rptViewer.ReportSource = myReport
            myReport.SetParameterValue("DateFrom", CDate(DateFrom).ToString("yyyy-MM-dd"))
            myReport.SetParameterValue("DateTo", CDate(DateTo).ToString("yyyy-MM-dd"))
            myReport.SetParameterValue("Branch", ByBra)
            myReport.SetParameterValue("EmpName", ByBraName)
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try
    End Sub

    Public Function Execute_DataSetCMMS(ByVal as_mysql As String) As DataSet
        Dim Con As New MySqlConnection
        Dim Com As New MySqlCommand
        Dim sqlAdapter As MySqlDataAdapter
        Dim sqlDataset As New DataSet

        Execute_DataSetCMMS = Nothing
        Try
            Try
                Con.ConnectionString = Me.Session("strCon")
                If Con.State = ConnectionState.Closed Then
                    Con.Open()
                End If
            Catch
            End Try
            sqlAdapter = New MySqlDataAdapter(as_mysql, Con)
            sqlAdapter.Fill(sqlDataset)
            If Not sqlDataset Is Nothing Then
                If sqlDataset.Tables(0).Rows.Count <> 0 Then
                    Execute_DataSetCMMS = sqlDataset
                    sqlDataset.Dispose()
                    sqlAdapter.Dispose()
                End If
            End If
            Con.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            Con.Close()
            Com.Dispose()
        End Try
    End Function

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            CheckLogin()
            CheckLogin()
        End If
    End Sub
End Class
