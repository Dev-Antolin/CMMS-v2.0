Imports CrystalDecisions.CrystalReports.Engine
'Imports System.Data
'Imports MySql.Data.MySqlClient
Partial Class Report_BranchRPT
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Me.Session("BranchAssetP") Is Nothing Then
            Response.Redirect("BranchProf.aspx")
        Else
            rptViewer.ReportSource = Me.Session("BranchAssetP")
        End If
        'GenerateReport()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Disable Cache
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        CheckLogin()
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

    'Private Sub GenerateReport()

    '    'Dim ds As DataSet
    '    'Dim dt As New DataTable
    '    'Dim myRPT As String
    '    'Dim DateFrom As String
    '    'Dim DateTo As String
    '    'Dim ByBra As String
    '    'Dim ByBraName As String
    '    'DateFrom = Me.Session("BraDateFrom")
    '    'DateTo = Me.Session("BraDateTo")
    '    'ByBra = Me.Session("ByBra")
    '    'ByBraName = Me.Session("ByBraName")
    '    rptViewer.ReportSource = Me.Session("BranchAssetRPT")
    '    Try
    '        'Dim myReport As New ReportDocument
    '        'myReport.Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\BranchAssetProfile.rpt")
    '        'myReport.SetDataSource(Me.Session("BranchAsset"))
    '        'rptViewer.ReportSource = myReport
    '        'myReport.SetParameterValue("DateFrom", Me.Session("BraDateFrom")) ' CDate(DateFrom).ToString("yyyy-MM-dd"))
    '        'myReport.SetParameterValue("DateTo", Me.Session("BraDateTo")) 'CDate(DateTo).ToString("yyyy-MM-dd"))
    '        'myReport.SetParameterValue("Branch", Me.Session("ByBra")) ' ByBra)
    '        'myReport.SetParameterValue("EmpName", Me.Session("ByBraName")) ' ByBraName)

    '        'myRPT = "call spbraassprof('" & DateFrom & "','" & DateTo & "','" & ByBra & "','" & ByBraName & "');"
    '        'ds = Execute_DataSetCMMS(myRPT)
    '        'If Not ds Is Nothing Then

    '        '    dt = ds.Tables(0)
    '        '    myReport.Load(AppDomain.CurrentDomain.BaseDirectory + "CrystalRPT\BranchAssetProfile.rpt")
    '        '    myReport.SetDataSource(Me.Session("BranchAsset"))
    '        '    rptViewer.ReportSource = myReport
    '        '    myReport.SetParameterValue("DateFrom", Me.Session("BraDateFrom")) ' CDate(DateFrom).ToString("yyyy-MM-dd"))
    '        '    myReport.SetParameterValue("DateTo", Me.Session("BraDateTo")) 'CDate(DateTo).ToString("yyyy-MM-dd"))
    '        '    myReport.SetParameterValue("Branch", Me.Session("ByBra")) ' ByBra)
    '        '    myReport.SetParameterValue("EmpName", Me.Session("ByBraName")) ' ByBraName)
    '        'Else
    '        '    Exit Sub
    '        'End If
    '    Catch ex As Exception
    '        Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
    '    End Try
    'End Sub

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
            CheckLogin()
            CheckLogin()
        End If
    End Sub
End Class
