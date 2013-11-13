Imports INI_DLL
Imports MYSQLDB_DLL
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System
Imports System.Data
Partial Class Report_MaintenanceHistory2
    Inherits System.Web.UI.Page
    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            CheckLogin()
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            CheckUser()
            CheckLogin()
            Divisions()
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Disable Cache
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        CheckUser()
        If Not Page.IsPostBack Then
            Me.Session("Click") = "AM"
            txtDateFrom.Text = Format(Date.Now, "yyyy-MM-dd")
            txtDateTo.Text = Format(Date.Now, "yyyy-MM-dd")
        Else
            txtDateFrom.Text = Request.Form(txtDateFrom.UniqueID)
            txtDateTo.Text = Request.Form(txtDateTo.UniqueID)
        End If
        lblErrorReport.Text = ""
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

    Protected Sub btnGeneRptHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGeneRptHistory.Click
        Dim frmAll As String
        Dim toAll As String
        frmAll = Request.Form(txtDateFrom.UniqueID)
        toAll = Request.Form(txtDateTo.UniqueID)

        'ely code 10-24-2011
        If CDate(frmAll) > CDate(toAll) Then
            lblErrorReport.Visible = True
            lblErrorReport.Text = "Date range error."
            Exit Sub
        End If
        'ely code 10-24-2011


        If frmAll = "" OrElse toAll = "" Then
            lblErrorReport.Text = "No date to query."
            Exit Sub
        End If
        If ddlDivName.Text = "0" Then
            Me.Session.Add("FromAll", frmAll)
            Me.Session.Add("ToAll", toAll)
            Response.Redirect("MaintenanceHistory2Rpt.aspx")
        Else
            Me.Session.Add("FromAll", frmAll)
            Me.Session.Add("ToAll", toAll)
            Me.Session.Add("ByDiv", ddlDivName.Text)
            Me.Session.Add("ByManName", txtDivManName.Text)
            Response.Redirect("MaintenanceHistory2RptAll.aspx")
        End If
    End Sub

    Private Sub Divisions()
        Dim ds As DataSet
        Dim myDiv As String
        Try
            If Me.Session("ZCode") = "VISMIN" Then
                myDiv = "select distinct division from irdivision where zonecode = 'vismin' order by division desc;"
            Else
                myDiv = "select distinct division from irdivision where zonecode = 'luzon' order by division desc;"
            End If
            ds = Execute_DataSet(myDiv)

            Dim RText As String = "DIVISIONS"
            Dim i As Integer
            ddlDivName.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlDivName.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlDivName.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DivManName()
        Dim ds As DataSet
        Dim myMan As String

        Try
            If Me.Session("ZCode") = "VISMIN" Then
                myMan = "select distinct upper(fullname) as fullname from webaccounts wa " _
                      & "inner join irdivision ird on wa.costcenter = ird.costcenter " _
                      & "where ird.division = '" & ddlDivName.Text.Trim & "' and wa.task like '%DIVMAN%' and wa.zonecode = 'vismin'"
            Else
                myMan = "select distinct upper(fullname) as fullname from webaccounts wa " _
                      & "inner join irdivision ird on wa.costcenter = ird.costcenter " _
                      & "where ird.division = '" & ddlDivName.Text.Trim & "' and wa.task like '%DIVMAN%' and wa.zonecode = 'luzon'"
            End If
            ds = Execute_DataSet(myMan)
            txtDivManName.Text = ds.Tables(0).Rows(0).Item(0).ToString
        Catch ex As Exception
            lblErrorReport.Text = "No Division Manager assigned in this division"
            txtDivManName.Text = ""
        End Try
    End Sub

    Public Function Execute_DataSet(ByVal as_mysql As String) As DataSet
        Dim Con As New SqlConnection
        Dim Com As New SqlCommand
        Dim sqlAdapter As SqlDataAdapter
        Dim sqlDataset As New DataSet

        Execute_DataSet = Nothing
        Try
            Try
                Con.ConnectionString = Me.Session("strConf")
                If Con.State = ConnectionState.Closed Then
                    Con.Open()
                End If
            Catch
            End Try
            sqlAdapter = New SqlDataAdapter(as_mysql, Con)
            sqlAdapter.Fill(sqlDataset)
            If Not sqlDataset Is Nothing Then
                If sqlDataset.Tables(0).Rows.Count <> 0 Then
                    Execute_DataSet = sqlDataset
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

    Protected Sub ddlDivName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDivName.SelectedIndexChanged
        If ddlDivName.Text = "0" Then
            txtDivManName.Text = ""
        Else
            DivManName()
        End If
    End Sub
End Class
