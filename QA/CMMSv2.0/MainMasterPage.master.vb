Imports INI_DLL
Imports MYSQLDB_DLL
Imports MySql.Data
Imports MySql.Data.MySqlClient

Imports System.Collections.Generic
Imports System.Web.Services
Imports System.IO
Imports System.Data

Partial Class MainMasterPage
    Inherits System.Web.UI.MasterPage
    Dim sPageName As String = ""
    Dim sPageName2 As String = ""

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Request.UserAgent.IndexOf("AppleWebKit") > 0 Then
            Request.Browser.Adapters.Clear()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        sPageName = GetPageName()
        NavigationMenu.Visible = False
        lblWorkLabel.Visible = False
        If Not IsPostBack Then
            If Me.Session("JDesc") = "MMD-STAFF" Then
                Dim sURL As String = Request.Url.ToString().ToLower()
                If (sURL.EndsWith("cabasicdata.aspx")) Then

                    mnuChild.Visible = False
                    NavigationMenu.Visible = True
                    lblCategory.Text = "Computer Asset"

                ElseIf (sURL.EndsWith("caadddevice.aspx")) Then

                    NavigationMenu.Visible = True
                    lblCategory.Text = "Computer Asset"

                ElseIf (sURL.Contains("/view/") OrElse sURL.Contains("/report/") OrElse sURL.Contains("/workorder/")) Then
                    If (sURL.Contains("/report/")) Then
                        lblCategory.Text = "Report Asset"
                    End If
                    If (sURL.Contains("/workorder/")) Then
                        lblCategory.Text = "Work Order Menu"
                        If (sURL.EndsWith("receiveddetail.aspx")) Then
                            lblCategory.Visible = False
                        End If
                    End If
                    smdsChild.StartingNodeOffset = -1
                End If
            ElseIf divisionUsers(Me.Session("JDesc")) = True Then
                Dim sURL As String = Request.Url.ToString().ToLower()
                If (sURL.Contains("/workorder/")) Then
                    NavigationMenu.Visible = False
                    lblCategory.Text = "Work Order Menu"
                    If (sURL.EndsWith("receiveddetail.aspx")) Then
                        lblCategory.Visible = False
                    End If
                End If
                smdsChild.StartingNodeOffset = -1
            ElseIf clientUsers(Me.Session("JDesc")) = True Then
                Dim sURL As String = Request.Url.ToString().ToLower()
                If (sURL.Contains("/workorder/")) Then
                    NavigationMenu.Visible = False
                    lblCategory.Text = "Work Order Menu"
                    If (sURL.EndsWith("receiveddetail.aspx")) Then
                        lblCategory.Visible = False
                    End If
                End If
                smdsChild.StartingNodeOffset = -1
            End If
        End If
        Dim fName As String = StrConv(Me.Session("fName"), VbStrConv.ProperCase)
        Dim wee As String = "Welcome, " & fName
        lblLoginUser.Text = wee
        lblJob.Text = Me.Session("JDesc")
        lblCostCenter.Text = Me.Session("JCode")
        lblHO.Text = Me.Session("HO")

    End Sub

    Public Function GetPageName() As String
        Dim fi As String = Request.Url.Segments(2).ToString.Replace("/", "")
        Return fi
    End Function
    Public Function GetPageName2() As String
        Dim path As String = System.Web.HttpContext.Current.Request.Url.AbsolutePath
        Dim fi As System.IO.FileInfo = New System.IO.FileInfo(path)
        GetPageName2 = fi.ToString
    End Function

    Public Sub DisplayDataFromMaster(ByVal message As String)
        lblLoginUser.Text = message
    End Sub

    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        HttpContext.Current.Session.Abandon()
        Response.Redirect("~/login.aspx")
    End Sub

    <WebMethod()> _
    Public Sub KillSession()
        HttpContext.Current.Session.Abandon()
    End Sub

    Protected Sub Menu1_MenuItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemDataBound

        If Me.Session("JDesc") = "MMD-STAFF" Then
            If (e.Item.Text.ToUpper = sPageName.ToUpper) Then

                e.Item.Selected = True

            End If
            If (e.Item.Text.ToUpper = "DATA ENTRY") Then
                sPageName2 = GetPageName2().ToLower

                If (sPageName2.EndsWith("cabasicdata.aspx")) Then
                    NavigationMenu.Items(0).Selected = True

                ElseIf (sPageName2.EndsWith("caadddevice.aspx")) Then
                    NavigationMenu.Items(1).Selected = True
                    Menu1.Items(0).Selected = True

                End If
            End If
        ElseIf divisionUsers(Me.Session("JDesc")) = True Then
            Menu1.Visible = False
            lblWorkLabel.Visible = True
            lblWorkLabel.Text = "Work Order"
        ElseIf clientUsers(Me.Session("JDesc")) = True Then
            Menu1.Visible = False
            lblWorkLabel.Visible = True
            lblWorkLabel.Text = "Work Order"
        End If
    End Sub

    Private Function divisionUsers(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from cmms_users where task like '%BOS-CONT%' or task like '%DIVMAN%' or task like '%DEPTMAN%';"
        ds = Execute_DataSet(mySqlDes)
        If ds IsNot Nothing Then
            For x = 0 To ds.Tables(0).Rows.Count - 1
                If jobDesc = ds.Tables(0).Rows(x)(0) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Function clientUsers(ByVal jobDesc As String) As Boolean
        Dim mySqlDes As String
        Dim ds As DataSet
        mySqlDes = "select distinct task from cmms_users where task like '%/BM-R/%' or task like '%LPT-A%' or task like '%RCT-A%';"
        ds = Execute_DataSet(mySqlDes)
        If ds IsNot Nothing Then
            For x = 0 To ds.Tables(0).Rows.Count - 1
                If jobDesc = ds.Tables(0).Rows(x)(0) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Public Function Execute_DataSet(ByVal as_mysql As String) As DataSet
        Dim Con As New MySqlConnection        

        Dim Com As New MySqlCommand
        Dim sqlAdapter As MySqlDataAdapter
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
            sqlAdapter = New MySqlDataAdapter(as_mysql, Con)
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
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
            Con.Close()
            Com.Dispose()
        End Try
    End Function
End Class
