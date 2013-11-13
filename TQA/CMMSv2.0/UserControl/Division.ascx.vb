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

Partial Class usercontrol_Division
    Inherits System.Web.UI.UserControl
    Public DivisionSelected As DivisionDelegate
    Public DivisionToFind As SearchDelegate

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load        
        lblNoRec.Visible = False
        If Not Me.Session("isDivision") Then
            Exit Sub
        End If
        Me.gvBranch.DataSource = Nothing
        Me.gvBranch.DataBind()        
        If Me.Session("JDesc").ToString.Contains("HELPDESK") OrElse Me.Session("JDesc").ToString.Contains("MMD") Then
            If Me.Session("View") Is Nothing Then
                Division()
            ElseIf Me.Session("View") = "canUpdate" Then
                getDivisionBranch()
            End If
        Else
            Division()
        End If
    End Sub

    Private Sub Division()

        If Me.Session("_txtdName") <> Nothing Or Me.Session("_txtdCode") <> Nothing Then
            txtSearchDCode.Text = Me.Session("_txtdCode")
            txtSearchDName.Text = Me.Session("_txtdName")
        End If

        Dim sql As New StringBuilder()
        Dim Div As New DataTable("DivisionAcro")
        Try
            Using con As New SqlConnection(Me.Session("strConf").ToString())
                con.Open()
                sql.Append("SELECT DISTINCT LTRIM(RTRIM(DivisionAcro)) AS DivisionAcro ,LTRIM(RTRIM(Division)) AS Division FROM irdivision ")
                sql.Append("WHERE zonecode = @Zone ")
                If Trim(Me.txtSearchDName.Text) <> Nothing Then
                    sql.Append("AND LTRIM(RTRIM(Division)) = @Bname ")
                End If
                If Trim(Me.txtSearchDCode.Text) <> Nothing Then
                    sql.Append("AND LTRIM(RTRIM(DivisionAcro)) = @Bcode ")
                End If
                sql.Append("ORDER BY Division; ")
                Using cmd As New SqlCommand(sql.ToString(), con)
                    If Trim(Me.txtSearchDName.Text) <> Nothing Then
                        cmd.Parameters.Add(New SqlParameter("Bname", SqlDbType.VarChar, 60)).Value = Trim(Me.txtSearchDName.Text)
                    End If
                    If Trim(Me.txtSearchDCode.Text) <> Nothing Then
                        cmd.Parameters.Add(New SqlParameter("Bcode", SqlDbType.VarChar, 6)).Value = Trim(Me.txtSearchDCode.Text)
                    End If
                    cmd.Parameters.AddWithValue("Zone", Trim(Me.Session("ZCode")))
                    cmd.CommandType = CommandType.Text
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows Then
                            Div.Load(DataRead)
                            gvBranch.Visible = True
                            gvBranch.DataSource = Div
                            gvBranch.DataBind()
                            Me.Session.Add("_txtdName", txtSearchDName.Text)
                            Me.Session.Add("_txtdCode", txtSearchDCode.Text)
                            Me.txtSearchDCode.Text = String.Empty
                            Me.txtSearchDName.Text = String.Empty
                        Else
                            Me.Session("_txtdName") = Nothing
                            Me.Session("_txtdCode") = Nothing
                            Me.txtSearchDCode.Text = String.Empty
                            Me.txtSearchDName.Text = String.Empty
                            gvBranch.Visible = False
                            lblNoRec.Visible = True
                        End If
                        DataRead.Close()
                    End Using
                End Using
            End Using
        Catch ex As SqlException
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
        End Try
    End Sub

    Protected Sub Select_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentrow As GridViewRow = TryCast(DirectCast(sender, Control).Parent.NamingContainer, GridViewRow)
        Dim a As String = currentrow.Cells(2).Text
        If currentrow IsNot Nothing Then
            If DivisionSelected IsNot Nothing Then
                DivisionSelected.Invoke(currentrow.Cells(1).Text, currentrow.Cells(2).Text)
                Me.Session("_txtdCode") = Nothing
                Me.Session("_txtdName") = Nothing
                'Me.txtSearchDCode.Text = String.Empty
                'Me.txtSearchDName.Text = String.Empty                
            Else
                Throw New ArgumentNullException("DivisionSelected Event not registered")
            End If
        End If
    End Sub

    Protected Sub btnSearchBranch_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        If DivisionToFind IsNot Nothing Then
            DivisionToFind.Invoke("Search")
        Else
            Throw New ArgumentNullException("JobTitleToFind Event not registered")
        End If
    End Sub

    Public Sub getDivisionBranch()
        If Me.Session("_txtdName") <> Nothing Or Me.Session("_txtdCode") <> Nothing Then
            txtSearchDCode.Text = Me.Session("_txtdCode")
            txtSearchDName.Text = Me.Session("_txtdName")
        End If
        Dim Div As New DataTable("DivisionAcro")
        Dim sql As New StringBuilder()
        If Me.Session("strCon").ToString = String.Empty Then Exit Sub
        Try
            Using con As New MySqlConnection(Me.Session("strCon").ToString)
                con.Open()
                sql.Append("SELECT DISTINCT TRIM(Bc_Code) AS DivisionAcro,TRIM(Bc_Name) AS Division ,Zone_Code FROM cmms.cmms_entry_masterheader ")
                sql.Append("WHERE NOT IsNumeric(Bc_Code) ")
                If Trim(Me.txtSearchDName.Text) <> Nothing Then
                    sql.Append("AND TRIM(Bc_Name) = @Bname ")
                End If
                If Trim(Me.txtSearchDCode.Text) <> Nothing Then
                    sql.Append("AND TRIM(Bc_Code) = @Bcode ")
                End If
                sql.Append("AND Item_Code IN ('500','501','502') ")
                sql.Append("AND zone_Code = @Zone ")
                sql.Append("ORDER BY Bc_Name ASC; ")

                Using cmd As New MySqlCommand(sql.ToString(), con)
                    If Trim(Me.txtSearchDName.Text) <> Nothing Then
                        cmd.Parameters.Add(New MySqlParameter("Bname", MySqlDbType.VarString, 100)).Value = Trim(Me.txtSearchDName.Text)
                    End If
                    If Trim(Me.txtSearchDCode.Text) <> Nothing Then
                        cmd.Parameters.Add(New MySqlParameter("Bcode", MySqlDbType.VarString, 3)).Value = Trim(Me.txtSearchDCode.Text)
                    End If
                    cmd.Parameters.Add(New MySqlParameter("Zone", MySqlDbType.VarString, 10)).Value() = Trim(Me.Session("ZCode"))
                    cmd.CommandType = CommandType.Text
                    Using DataRead As MySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows Then
                            Div.Load(DataRead)
                            gvBranch.Visible = True
                            gvBranch.DataSource = Div
                            gvBranch.DataBind()
                            Me.Session.Add("_txtdName", txtSearchDName.Text)
                            Me.Session.Add("_txtdCode", txtSearchDCode.Text)
                            Me.txtSearchDName.Text = String.Empty
                            Me.txtSearchDCode.Text = String.Empty
                        Else
                            Me.Session("_txtdCode") = Nothing
                            Me.Session("_txtdName") = Nothing
                            Me.txtSearchDName.Text = String.Empty
                            Me.txtSearchDCode.Text = String.Empty
                            gvBranch.Visible = False
                            lblNoRec.Visible = True
                        End If
                        DataRead.Close()
                    End Using
                End Using
            End Using
        Catch ex As MySqlException
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
        Catch ex As Exception
            Response.Write("<script language=javascript>alert('" & ex.Message & "')</script>")
        End Try
    End Sub

    'Protected Sub txtSearchDCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchDCode.TextChanged
    '    If Me.Session("_txtdName") <> Nothing Or Me.Session("_txtdCode") <> Nothing Then
    '        Me.Session("_txtdCode") = Nothing
    '        Me.Session("_txtdName") = Nothing
    '    End If
    'End Sub

    'Protected Sub txtSearchDName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchDName.TextChanged
    '    If Me.Session("_txtdName") <> Nothing Or Me.Session("_txtdCode") <> Nothing Then
    '        Me.Session("_txtdCode") = Nothing
    '        Me.Session("_txtdName") = Nothing
    '    End If
    'End Sub

    'Protected Sub gvBranch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvBranch.SelectedIndexChanged

    'End Sub
End Class
