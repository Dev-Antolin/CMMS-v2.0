Imports INI_DLL
Imports MYSQLDB_DLL
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System
Imports System.Data
Partial Class Report_MaintenanceHistory
    Inherits System.Web.UI.Page

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            CheckUser()
            CheckLogin()
            Region()
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
            Me.Session("Click") = "AM"
            txtDateFrom.Text = Format(Date.Now, "yyyy-MM-dd")
            txtDateTo.Text = Format(Date.Now, "yyyy-MM-dd")
        Else
            txtDateFrom.Text = Request.Form(txtDateFrom.UniqueID)
            txtDateTo.Text = Request.Form(txtDateTo.UniqueID)
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
        Else
            If ddlBraMem.Text = Me.Session("ALLEmp") Then
                Me.Session.Add("BraDateFrom", txtDateFrom.Text)
                Me.Session.Add("BraDateTo", txtDateTo.Text)
                Me.Session.Add("ByBra", ddlBranch.Text)
                Response.Redirect("MaintenanceHistoryRptAll.aspx")
            Else
                Me.Session.Add("BraDateFrom", txtDateFrom.Text)
                Me.Session.Add("BraDateTo", txtDateTo.Text)
                Me.Session.Add("ByBra", ddlBranch.Text)
                Me.Session.Add("ByBraName", ddlBraMem.Text)
                Response.Redirect("MaintenanceHistoryRpt.aspx")
            End If
        End If
    End Sub

    Protected Sub ddlRegion_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRegion.SelectedIndexChanged
        Area()
    End Sub

    Protected Sub ddlArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlArea.SelectedIndexChanged
        Branch()
    End Sub

    Protected Sub ddlBranch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBranch.SelectedIndexChanged
        BranchManName()
    End Sub

    Private Sub Region()
        Dim ds As DataSet
        Dim myReg As String
        Try
            If Me.Session("ZCode") = "VISMIN" Then
                myReg = "select distinct upper(class_03) as class_03 from webbranches where zonecode = 'vismin' and class_03 <> 'HO' order by class_03 desc;"
            Else
                myReg = "select distinct upper(class_03)as class_03 from webbranches where zonecode = 'luzon' and class_03 <> 'HO' order by class_03 desc;"
            End If
            ds = Execute_DataSet(myReg)

            Dim RText As String = ""
            Dim i As Integer
            ddlRegion.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlRegion.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString.Trim))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlRegion.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Area()
        Dim ds As DataSet
        Dim myAre As String
        Try
            If Me.Session("ZCode") = "VISMIN" Then
                myAre = "select distinct upper(class_04) as class_04 " _
                      & "from webbranches " _
                      & "where zonecode = 'vismin' and class_03 = '" & ddlRegion.Text & "' " _
                      & "order by class_04 desc;"
            Else
                myAre = "select distinct upper(class_04) as class_04 " _
                      & "from webbranches " _
                      & "where zonecode = 'luzon' and class_03 = '" & ddlRegion.Text & "' " _
                      & "order by class_04 desc;"
            End If
            ds = Execute_DataSet(myAre)

            Dim RText As String = ""
            Dim i As Integer
            ddlArea.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlArea.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString.Trim))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlArea.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Branch()
        Dim ds As DataSet
        Dim myBra As String
        Try
            If Me.Session("ZCode") = "VISMIN" Then
                myBra = "select distinct upper(bedrnm)as bedrnm from webbranches " _
                      & "where zonecode = 'vismin' and class_04 = '" & ddlArea.Text & "' order by bedrnm desc;"
            Else
                myBra = "select distinct upper(bedrnm)as bedrnm from webbranches " _
                      & "where zonecode = 'luzon' and class_04 = '" & ddlArea.Text & "' order by bedrnm desc;"
            End If
            ds = Execute_DataSet(myBra)

            Dim RText As String = ""
            Dim i As Integer
            ddlBranch.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlBranch.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString.Trim))
                Next
            Else
                RText = "Nothing Available"
            End If
            ddlBranch.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub BranchManName()
        Dim ds As DataSet
        Dim myBraMan As String

        Try
            If Me.Session("ZCode") = "VISMIN" Then
                myBraMan = "select distinct upper(wa.fullname) as fullname from webaccounts as wa " _
                         & "inner join webbranches as wb on wa.comp = wb.bedrnr " _
                         & "where wa.zonecode = 'vismin' and wb.bedrnm = '" & ddlBranch.Text & "' order by wa.fullname desc;"
            Else
                myBraMan = "select distinct upper(wa.fullname) as fullname from webaccounts as wa " _
                         & "inner join webbranches as wb on wa.comp = wb.bedrnr " _
                         & "where wa.zonecode = 'luzon' and wb.bedrnm = '" & ddlBranch.Text & "' order by wa.fullname desc;"
            End If
            ds = Execute_DataSet(myBraMan)

            Dim RText As String = ""
            Dim AllEmp As String = "ALL EMPLOYEE"
            Dim i As Integer
            Dim s As String
            ddlBraMem.Items.Clear()
            If ds.Tables(0).Rows.Count > 0 Then
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    ddlBraMem.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString.Trim))
                Next
                s = i + 1
                ddlBraMem.Items.Insert(i, New ListItem(AllEmp, s))
                Me.Session.Add("ALLEmp", s)
            Else
                RText = "Nothing Available"
            End If
            ddlBraMem.Items.Insert(0, New ListItem(RText, "0"))

        Catch ex As Exception
            MsgBox(ex.Message)
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
End Class
