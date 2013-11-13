Imports INI_DLL
Imports MYSQLDB_DLL
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System
Imports System.Data
Partial Class Report_DivisionProf
    Inherits System.Web.UI.Page
    Dim Div As New DivisionReports

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            CheckLogin()
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            CheckUser()
            CheckLogin()
            Try
                Me.ddlDivName.DataSource = Div.getDivisions(Me.Session("ZCode").ToString, Me.Session("strCon").ToString)
                Me.ddlDivName.DataBind()
            Catch ex As Exception
                Me.ddlDivName.Enabled = False
                Me.lblErrorReport.Text = ex.Message
            End Try           
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Disable Cache
        Response.Buffer = True
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1D)
        Response.Expires = -1500
        Response.CacheControl = "no-cache"
        CheckUser()
        If CheckAttachA() = True Then
            Dim AN As String = Me.Session("txtAAsstInvNo")
            If AN = Me.Session("NewAddAssetNo") Then
                Dim DelSql As String
                DelSql = "delete from cmms_entry_attachfiles where Asset_Inv_No ='" & AN & "';"
                Execute_Delete(DelSql)
            End If
        End If
        If CheckAttachB() = True Then
            Dim AN As String = Me.Session("txtAsstInvNo")
            If AN = Me.Session("NewBasicAssetNo") Then
                Dim DelSql As String
                DelSql = "delete from cmms_entry_attachfiles where Asset_Inv_No ='" & AN & "';"
                Execute_Delete(DelSql)
            End If
        End If
        If Not Page.IsPostBack Then
            Me.Session("Click") = "DP"            
        End If
        lblErrorReport.Text = Nothing
    End Sub

    Private Sub CheckLogin()        
        If Me.Session("fName") = "" Then
            Response.Redirect("~/login.aspx")
        End If
    End Sub

    Private Sub CheckUser()
        If Me.Session("JDesc").ToString = "MMD-STAFF" Or Me.Session("JDesc").ToString = "MIS-HELPDESK" Or Me.Session("JDesc").ToString.Contains("LPTL") Then
            Exit Sub
        Else
            Response.Redirect("~/Unauthorized.aspx")
        End If
    End Sub

    Private Function CheckAttachA() As Boolean
        Dim AssetNo As String = Me.Session("txtAAsstInvNo")
        Dim CheckSql As String
        Dim ds As DataSet

        CheckSql = "select Asset_Inv_No from cmms_entry_attachfiles where Asset_Inv_No = '" & AssetNo & "';"

        ds = Execute_DataSetAttach(CheckSql)
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function CheckAttachB() As Boolean
        Dim AssetNo As String = Me.Session("txtAsstInvNo")
        Dim CheckSql As String
        Dim ds As DataSet

        CheckSql = "select Asset_Inv_No from cmms_entry_attachfiles where Asset_Inv_No = '" & AssetNo & "';"

        ds = Execute_DataSetAttach(CheckSql)
        If Not ds Is Nothing AndAlso ds.Tables(0).Rows(0).Item(0).ToString <> "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function Execute_Delete(ByVal mySql As String) As Boolean
        Dim con As New MySqlConnection
        Dim com As New MySqlCommand
        Dim oTran As MySqlTransaction

        con.ConnectionString = Me.Session("strCon")
        If con.State = ConnectionState.Closed Then
            con.Open()
            oTran = con.BeginTransaction()
            com = con.CreateCommand
            com.CommandTimeout = 0
            com.Transaction = oTran
            Try
                com.CommandText = mySql
                com.ExecuteNonQuery()
                oTran.Commit()
                com.Dispose()
                Return True
            Catch ex As Exception
                Me.Session.Add("InsertError", ex.Message)
                oTran.Rollback()
                con.Close()
                com.Dispose()
                Return False
            End Try
        End If
    End Function

    Protected Sub ddlDivName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDivName.SelectedIndexChanged
        If Me.ddlDivName.SelectedIndex = 0 Then
            Me.txtDivManager.Text = Nothing
        Else            
            Me.txtDivManager.Text = Div.DivisionManager(Me.ddlDivName.SelectedItem.Text, Me.Session("ZCode").ToString, Me.Session("strConf").ToString)
        End If
    End Sub

    Protected Sub btnGeneRptAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGeneRptAll.Click
        Try
            If Me.ddlDivName.SelectedIndex = 0 Then
                Me.Session.Add("AllDivisionRpt", Div.AllDivManRpt(Me.Session("strCon").ToString))
                If Me.Session("AllDivisionRpt").ToString Is Nothing Then
                    Me.lblErrorReport.Text = "NO RECORD FOUND"
                Else
                    Me.Response.Redirect("DivisionRPT.aspx", False)
                End If
            Else
                Me.Session.Add("ByDivManRpt", Div.ByDivManRpt(Me.Session("strCon").ToString, Me.ddlDivName.SelectedItem.Text, Me.txtDivManager.Text))
                If Me.Session("ByDivManRpt").ToString = Nothing Then
                    Me.lblErrorReport.Text = "NO RECORD FOUND"
                Else
                    Me.Response.Redirect("ByDivisionRPT.aspx", False)
                End If
            End If
        Catch ex As MySqlException
            If ex.Number = -2 Then
                Me.lblErrorReport.Text = "Connection Timeout"
            Else
                Me.lblErrorReport.Text = ex.Message
            End If
        Catch ex As Exception
            Me.lblErrorReport.Text = ex.Message
        End Try
        
    End Sub

    '    Private Sub Divisions()
    '        Dim ds As DataSet
    '        Dim myDiv As String
    '        Try
    '            If Me.Session("ZCode") = "VISMIN" Then
    '                myDiv = "select distinct division from irdivision where zonecode = 'vismin' order by division desc;"
    '            Else
    '                myDiv = "select distinct division from irdivision where zonecode = 'luzon' order by division desc;"
    '            End If
    '            ds = Execute_DataSet(myDiv)

    '            Dim RText As String = "DIVISIONS"
    '            Dim i As Integer
    '            ddlDivName.Items.Clear()
    '            If ds.Tables(0).Rows.Count > 0 Then
    '                For i = 0 To ds.Tables(0).Rows.Count - 1
    '                    ddlDivName.Items.Insert(0, New ListItem(ds.Tables(0).Rows(i).Item(0).ToString, ds.Tables(0).Rows(i).Item(0).ToString))
    '                Next
    '            Else
    '                RText = "Nothing Available"
    '            End If
    '            ddlDivName.Items.Insert(0, New ListItem(RText, "0"))

    '        Catch ex As Exception
    '            lblErrorReport.Text = ex.Message
    '        End Try
    '    End Sub

    '    Private Sub DivManName()
    '        Dim ds As DataSet
    '        Dim myMan As String

    '        Try
    '            '/update by ely 12-12-2011
    '            If ddlDivName.Text.Trim = "GMO" Then
    '                If Me.Session("ZCode") = "VISMIN" Then
    '                    myMan = "select distinct fullname from webaccounts wa " _
    '                          & "inner join irdivision ird on wa.costcenter = ird.costcenter " _
    '                          & "where ird.division = '" & ddlDivName.Text.Trim & "' and wa.task like '%GENMAN%' and wa.zonecode = 'vismin'"
    '                Else
    '                    myMan = "select distinct fullname from webaccounts wa " _
    '                          & "inner join irdivision ird on wa.costcenter = ird.costcenter " _
    '                          & "where ird.division = '" & ddlDivName.Text.Trim & "' and wa.task like '%GENMAN%' and wa.zonecode = 'luzon'"
    '                End If
    '                ds = Execute_DataSet(myMan)
    '                txtDivManager.Text = ds.Tables(0).Rows(0).Item(0).ToString
    '                GoTo Bypass
    '            End If
    '            '\update by ely 12-12-2011

    '            If Me.Session("ZCode") = "VISMIN" Then
    '                myMan = "select distinct fullname from webaccounts wa " _
    '                      & "inner join irdivision ird on wa.costcenter = ird.costcenter " _
    '                      & "where ird.division = '" & ddlDivName.Text.Trim & "' and wa.task like '%DIVMAN%' and wa.zonecode = 'vismin'"
    '            Else
    '                myMan = "select distinct fullname from webaccounts wa " _
    '                      & "inner join irdivision ird on wa.costcenter = ird.costcenter " _
    '                      & "where ird.division = '" & ddlDivName.Text.Trim & "' and wa.task like '%DIVMAN%' and wa.zonecode = 'luzon'"
    '            End If
    '            ds = Execute_DataSet(myMan)
    '            txtDivManager.Text = ds.Tables(0).Rows(0).Item(0).ToString
    'Bypass:     '<----- code by ely 12/12/2011
    '        Catch ex As Exception
    '            lblErrorReport.Text = "No Division Manager assigned in this division."
    '            txtDivManager.Text = ""
    '        End Try
    '    End Sub

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

    Public Function Execute_DataSetAttach(ByVal as_mysql As String) As DataSet
        Dim Con As New MySqlConnection
        Dim Com As New MySqlCommand
        Dim sqlAdapter As MySqlDataAdapter
        Dim sqlDataset As New DataSet

        Execute_DataSetAttach = Nothing
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
                    Execute_DataSetAttach = sqlDataset
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
