Option Strict On
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Imports MySql.Data.MySqlClient
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Xml.Linq

Partial Class UserControl_DivisionDesignation
    Inherits System.Web.UI.UserControl
    Public DesignationDiv As DivDesigDelegate
    Public DesigToFind As SearchDelegate
    Dim SelectedCode As String
    Dim RecWorkOrderAuthor As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblNoRec.Visible = False
        findManagers()
    End Sub

    Private Sub findManagers()
        If Me.Session("strConf").ToString = "" Then
            Exit Sub
        End If

        Dim strCon As String = Me.Session("strConf").ToString
        Dim sql As String
        Dim code As String = txtSearchBName.Text
        Dim CurrentUser As String = Me.Session("FName").ToString
        RecWorkOrderAuthor = Me.Session("RecAuthor").ToString
        Dim cn As SqlConnection = New SqlConnection(strCon)
        SelectedCode = Me.Session("Designation").ToString

        'Using con As New SqlConnection(Me.Session("strConf").ToString)
        '    Dim sql As New StringBuilder()
        '    sql.Append("SELECT")
        '    sql.Append("    UPPER(RTRIM(fullname)) AS fullname ")
        '    sql.Append("FROM")
        '    sql.Append("    WebAccounts AS wa ")
        '    sql.Append("INNER JOIN")
        '    sql.Append("    irdivision AS ird ")
        '    sql.Append("ON")
        '    sql.Append("    ird.costcenter = wa.costcenter ")
        '    sql.Append("WHERE")
        '    sql.Append("    ird.division = @div ")
        '    sql.Append("AND ")
        '    sql.Append("")
        '    sql.Append("")
        '    sql.Append("")
        '    sql.Append("")
        '    sql.Append("")
        '    sql.Append("")
        'End Using


        If Me.Session("ZCode").ToString = "VISMIN" Then
            If SelectedCode = "" Then
                Exit Sub
            Else
                If txtSearchBName.Text = "" AndAlso RecWorkOrderAuthor = "" Then
                    sql = "select upper(wa.fullname) as fullname from webaccounts as wa " _
                        & "inner join irdivision as ird on ird.costcenter = wa.costcenter " _
                        & "where ird.division = '" & SelectedCode.Trim & "' and ird.division = '" & SelectedCode.Trim & "' " _
                        & "and wa.fullname <> '" & CurrentUser.Trim & "' and wa.zonecode = 'vismin' order by wa.fullname asc;"
                Else
                    sql = "select upper(wa.fullname) as fullname from webaccounts as wa " _
                        & "inner join irdivision as ird on ird.costcenter = wa.costcenter " _
                        & "where wa.fullname like '" & code.Trim & "%' and ird.division = '" & SelectedCode.Trim & "' " _
                        & "and wa.fullname <> '" & CurrentUser.Trim & "' and wa.fullname <> '" & RecWorkOrderAuthor.Trim & "' and wa.zonecode = 'vismin' order by wa.fullname asc;"
                End If
            End If
        Else
            If SelectedCode = "" Then
                Exit Sub
            Else
                If txtSearchBName.Text = "" Then
                    sql = "select upper(wa.fullname) as fullname from webaccounts as wa " _
                        & "inner join irdivision as ird on ird.costcenter = wa.costcenter " _
                        & "where ird.division = '" & SelectedCode.Trim & "' and wa.task like '%" & SelectedCode.Trim & "%' " _
                        & "and wa.zonecode = 'luzon' order by wa.fullname desc;"
                Else
                    sql = "select upper(wa.fullname) as fullname  from webaccounts as wa " _
                       & "inner join irdivision as ird on ird.costcenter = wa.costcenter " _
                       & "where ird.division = '" & code.Trim & "' and wa.task like '%" & SelectedCode.Trim & "%' " _
                       & "and wa.zonecode = 'luzon' order by wa.fullname desc;"
                End If
            End If
        End If

        Try
            Dim cmd As SqlCommand = New SqlCommand(sql, cn)
            cn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader
            Dim dt As New DataTable("fullname")
            dt.Load(reader)

            If dt.Rows.Count < 1 Then
                lblNoRec.Visible = True
                gvBranch.Visible = False
            Else
                gvBranch.Visible = True
                gvBranch.DataSource = dt
                gvBranch.DataBind()
            End If

            reader.Close()

        Catch ex As Exception
            Throw New Exception(ex.Message.ToString)
        Finally
            cn.Close()
            cn.Dispose()
        End Try

    End Sub

    Protected Sub Select_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentrow As GridViewRow = TryCast(DirectCast(sender, Control).Parent.NamingContainer, GridViewRow)
        If currentrow IsNot Nothing Then
            If DesignationDiv IsNot Nothing Then
                DesignationDiv.Invoke(currentrow.Cells(1).Text)
            Else
                Throw New ArgumentNullException("DivisionSelected Event not registered")
            End If
        End If
    End Sub

    Protected Sub btnSearchBranch_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        If DesigToFind IsNot Nothing Then
            DesigToFind.Invoke("Search")
        Else
            Throw New ArgumentNullException("JobTitleToFind Event not registered")
        End If
    End Sub
End Class
