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
Partial Class UserControl_LPTDesignation
    Inherits System.Web.UI.UserControl
    Public DesignationLPT As LPTDesigDelegate
    Public DesigToFindL As SearchDelegate
    Dim SelectedCode As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblNoRec.Visible = False
        findLPT()
    End Sub

    Private Sub findLPT()
        If Me.Session("strConf") = "" OrElse Me.Session("Desig").ToString = "0" Then
            Exit Sub
        End If

        'Dim strCon As String = Me.Session("strConf")
        'Dim sql As String
        Dim code As String = Trim(txtSearchLPT.Text)
        'Dim cn As SqlConnection = New SqlConnection(strCon)
        'SelectedCode = Me.Session("Desig")
        Dim URegion As String = Me.Session("JRegion")


        Using con As New SqlConnection(Me.Session("strConf"))
            Dim sql As New StringBuilder()
            sql.Append("SELECT DISTINCT")
            sql.Append("    UPPER(RTRIM(fullname)) AS fullname ")
            sql.Append("FROM")
            sql.Append("    WebLpts ")
            sql.Append("WHERE")
            sql.Append("    Class_03 = @Region ")
            If code <> Nothing Then
                sql.Append("AND fullname LIKE @fName ")
            End If
            sql.Append("ORDER BY")
            sql.Append("    fullname ASC; ")
            Using cmd As New SqlCommand(sql.ToString, con)
                cmd.Parameters.Add("Region", SqlDbType.VarChar, 30).Value = URegion
                If code <> Nothing Then
                    cmd.Parameters.Add("fName", SqlDbType.VarChar, 64).Value = code
                End If
                Try
                    con.Open()
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows Then
                            Dim dt As New DataTable("fullname")
                            dt.Load(DataRead)
                            gvBranch.Visible = True
                            gvBranch.DataSource = dt
                            gvBranch.DataBind()
                            Me.txtSearchLPT.Text = Nothing
                        Else
                            Me.txtSearchLPT.Text = Nothing
                            lblNoRec.Visible = True
                            gvBranch.Visible = False
                        End If
                    End Using
                Catch ex As Exception
                    If con.State = ConnectionState.Open Then
                        con.Dispose()
                    End If
                    Throw ex
                Finally
                    If con.State = ConnectionState.Open Then
                        con.Dispose()
                    End If
                End Try
            End Using
        End Using
        'If SelectedCode = "0" Then
        '    Exit Sub
        'Else
        '    If txtSearchLPT.Text = "" Then
        '        sql = "select distinct upper(fullname) as fullname from weblpts where class_03 = '" & URegion.Trim & "' order by fullname asc;"
        '    Else
        '        sql = "select distinct upper(fullname) as fullname from weblpts where class_03 = '" & URegion.Trim & "' and fullname like '" & code.Trim & "%' order by fullname asc;"
        '    End If
        'End If

        'Try
        '    Dim cmd As SqlCommand = New SqlCommand(sql, cn)
        '    cn.Open()

        '    Dim reader As SqlDataReader = cmd.ExecuteReader
        '    Dim dt As New DataTable("fullname")
        '    dt.Load(reader)

        '    If dt.Rows.Count < 1 Then
        '        lblNoRec.Visible = True
        '        gvBranch.Visible = False                
        '    Else
        '        gvBranch.Visible = True
        '        gvBranch.DataSource = dt
        '        gvBranch.DataBind()
        '    End If

        '    reader.Close()
        'Catch ex As Exception
        '    Throw New Exception(ex.Message.ToString)
        'Finally
        '    cn.Close()
        '    cn.Dispose()
        'End Try

    End Sub

    Protected Sub Select_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentrow As GridViewRow = TryCast(DirectCast(sender, Control).Parent.NamingContainer, GridViewRow)
        If currentrow IsNot Nothing Then
            If DesignationLPT IsNot Nothing Then
                DesignationLPT.Invoke(currentrow.Cells(1).Text)
            Else
                Throw New ArgumentNullException("DivisionSelected Event not registered")
            End If
        End If
    End Sub

    Protected Sub btnSearchBranch_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        If DesigToFindL IsNot Nothing Then
            DesigToFindL.Invoke("Search")
        Else
            Throw New ArgumentNullException("JobTitleToFind Event not registered")
        End If
    End Sub
End Class
