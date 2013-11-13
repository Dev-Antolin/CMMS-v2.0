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

Partial Class UserControl_UsersID
    Inherits System.Web.UI.UserControl

    Public UsersIDSelected As UsersIDDelegate
    Public UsersIDToFind As SearchDelegate

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblNoRec.Visible = False
        If Me.Session("isDivision") Then
            Exit Sub
        End If
        If Me.Session("JDesc").ToString.Contains("HELPDESK") OrElse _
           Me.Session("JDesc").ToString.Contains("MMD") OrElse _
           Me.Session("JDesc").ToString.Contains("LPTL") Then
            If Me.Session("View") = "canUpdate" Then
                getBranchEmployee()
            ElseIf Me.Session("View") Is Nothing Then
                getBranchID()
            End If
        Else
            getBranchID()            
        End If
    End Sub

    Protected Sub Select_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentrow As GridViewRow = TryCast(DirectCast(sender, Control).Parent.NamingContainer, GridViewRow)        
        If currentrow IsNot Nothing Then
            If UsersIDSelected IsNot Nothing Then
                UsersIDSelected.Invoke(currentrow.Cells(1).Text, currentrow.Cells(2).Text)
                Me.Session("_txtBEName") = Nothing
                Me.Session("_txtBEID") = Nothing
                Me.Session("bflag") = Nothing
            Else
                Throw New ArgumentNullException("UsersIDSelected Event not registered")
            End If
        End If
    End Sub

    Protected Sub btnSearchBranch_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        If UsersIDToFind IsNot Nothing Then
            UsersIDToFind.Invoke("Search")
        Else
            Throw New ArgumentNullException("JobTitleToFind Event not registered")
        End If
    End Sub

    Protected Friend Sub getBranchID()
        Try

            If Me.Session("_txtBEName") <> Nothing Or Me.Session("_txtBEID") <> Nothing Then
                txtSearchID.Text = Me.Session("_txtBEID")
                txtSearchEName.Text = Me.Session("_txtBEName")
                Me.Session.Add("bflag", 1)
            End If

            Using con As New SqlConnection(Me.Session("strConf").ToString())
                con.Open()
                Dim sql As New StringBuilder()
                Dim Branch As New DataTable("res_id")

                sql.Append("SELECT DISTINCT ")
                sql.Append("    wa.res_id AS res_id , ")
                sql.Append("    RTRIM(UPPER(wa.fullname)) AS fullname ")
                sql.Append("FROM ")
                sql.Append("    dbo.WebAccounts AS wa ")
                sql.Append("INNER JOIN ")
                sql.Append("    dbo.WebBranches AS wb ")
                sql.Append("ON ")
                sql.Append("    wa.comp = wb.bedrnr AND ")
                sql.Append("    wa.zonecode=wb.zonecode ")
                sql.Append("WHERE ")
                sql.Append("	wa.zonecode = @Zone AND ")
                sql.Append("    wb.bedrnr NOT IN ('001','002') ")
                If Trim(Me.txtSearchID.Text) <> Nothing Then
                    sql.Append("AND wa.res_id LIKE @EmpID ")
                    Me.Session.Add("bflag", 1)
                End If
                If Trim(Me.txtSearchEName.Text) <> Nothing Then
                    sql.Append("AND wa.fullname LIKE @EmpName ")
                    Me.Session.Add("bflag", 1)
                End If
                If Me.Session("Code") <> Nothing Then
                    sql.Append("AND wb.bedrnr = @Bcode ")
                End If
                sql.Append("ORDER BY ")
                sql.Append("    wa.fullname; ")

                'sql.Append("FROM WebProject.dbo.WebAccounts AS wa INNER JOIN WebProject.dbo.WebBranches AS wb ON wa.comp = wb.bedrnr ")
                'sql.Append("WHERE wa.zonecode = @Zone AND wb.bedrnr NOT IN ('001','002') ")
                'If Trim(Me.txtSearchID.Text) <> Nothing Then sql.Append("AND wa.res_id LIKE @EmpID ")
                'If Trim(Me.txtSearchEName.Text) <> Nothing Then sql.Append("AND wa.fullname LIKE @EmpName ")
                'If Me.Session("Code") <> Nothing Then sql.Append("AND wb.bedrnr = @Bcode ")
                'sql.Append("ORDER BY wa.fullname; ")

                Using cmd As New SqlCommand(sql.ToString(), con)
                    cmd.Parameters.Add(New SqlParameter("Zone", SqlDbType.VarChar, 8)).Value() = Me.Session("ZCode").ToString
                    If Me.Session("Code") <> Nothing Then
                        cmd.Parameters.Add(New SqlParameter("Bcode", SqlDbType.VarChar, 3)).Value() = Me.Session("Code")
                    End If
                    If Trim(Me.txtSearchID.Text) <> Nothing Then
                        cmd.Parameters.Add(New SqlParameter("EmpID", SqlDbType.VarChar, 15)).Value() = Trim(Me.txtSearchID.Text)
                    End If
                    If Trim(Me.txtSearchEName.Text) <> Nothing Then
                        cmd.Parameters.Add(New SqlParameter("EmpName", SqlDbType.VarChar, 20)).Value() = "%" & Trim(Me.txtSearchEName.Text) & "%"
                    End If
                    cmd.CommandType = CommandType.Text
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows() Then
                            Branch.Load(DataRead)
                            gvBranch.Visible = True
                            gvBranch.DataSource = Branch
                            gvBranch.DataBind()
                            Me.Session.Add("_txtBEName", txtSearchEName.Text)
                            Me.Session.Add("_txtBEID", txtSearchID.Text)
                            Me.txtSearchEName.Text = Nothing
                            Me.txtSearchID.Text = Nothing
                        Else
                            Me.Session("_txtBEName") = Nothing
                            Me.Session("_txtBEID") = Nothing
                            'Me.Session("bflag") = Nothing
                            Me.txtSearchEName.Text = Nothing
                            Me.txtSearchID.Text = Nothing
                            gvBranch.Visible = False
                            lblNoRec.Visible = True
                        End If
                        DataRead.Close()
                    End Using
                End Using
                con.Close()
            End Using
        Catch ex As Exception
            Dim a As String = ex.ToString()
        End Try
    End Sub

    Public Sub getBranchEmployee()
        If Me.Session("SelectedBname") = Nothing Then Exit Sub
        If Me.Session("_txtBEName") <> Nothing Or Me.Session("_txtBEID") <> Nothing Then
            txtSearchID.Text = Me.Session("_txtBEID")
            txtSearchEName.Text = Me.Session("_txtBEName")
            Me.Session.Add("bflag", 1)
        End If
        Dim Branch As New DataTable("res_id")
        Dim sql As New StringBuilder()
        If Me.Session("strCon").ToString = String.Empty Then Exit Sub
        Using con As New MySqlConnection(Me.Session("strCon").ToString)
            con.Open()

            sql.Append("SELECT DISTINCT Res_ID_Assigned AS res_id,Emp_Name_Assigned AS fullname FROM cmms.cmms_entry_masterheader ")
            sql.Append("WHERE Item_Code IN ('500','501','502') ")
            sql.Append("AND Bc_Code = @Bcode ")
            If Trim(Me.txtSearchID.Text) <> Nothing Then
                sql.Append("AND Res_Id_Assigned LIKE @res_id ")
                Me.Session.Add("bflag", 1)
            End If
            If Trim(Me.txtSearchEName.Text) <> Nothing Then
                sql.Append("AND Emp_Name_Assigned LIKE @res_name ")
                Me.Session.Add("bflag", 1)
            End If
            If Not String.IsNullOrEmpty(Me.Session("SelectedBname").ToString) Then
                sql.Append("AND Bc_Name = @Bname ")
            End If
            sql.Append("ORDER BY Emp_Name_Assigned ASC; ")

            Using cmd As New MySqlCommand(sql.ToString(), con)
                cmd.Parameters.Add(New MySqlParameter("Bcode", MySqlDbType.String)).Value() = Trim(Me.Session("Code").ToString())
                If Not String.IsNullOrEmpty(Me.Session("SelectedBname").ToString) Then
                    cmd.Parameters.Add(New MySqlParameter("Bname", MySqlDbType.String)).Value() = Trim(Me.Session("SelectedBname").ToString())
                End If
                If Trim(Me.txtSearchID.Text) <> Nothing Then
                    cmd.Parameters.Add(New MySqlParameter("res_id", MySqlDbType.VarString, 50)).Value() = "%" & Trim(Me.txtSearchID.Text)
                End If
                If Trim(Me.txtSearchEName.Text) <> Nothing Then
                    cmd.Parameters.Add(New MySqlParameter("res_name", MySqlDbType.VarString, 50)).Value() = "%" & Trim(Me.txtSearchEName.Text) & "%"
                End If
                cmd.CommandType() = CommandType.Text

                Using DataRead As MySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    If DataRead.HasRows() Then
                        Branch.Load(DataRead)
                        gvBranch.Visible = True
                        gvBranch.DataSource = Branch
                        gvBranch.DataBind()
                        Me.Session.Add("_txtBEName", txtSearchEName.Text)
                        Me.Session.Add("_txtBEID", txtSearchID.Text)
                        Me.txtSearchID.Text = String.Empty
                        Me.txtSearchEName.Text = String.Empty
                    Else
                        Me.Session("_txtBEName") = Nothing
                        Me.Session("_txtBEID") = Nothing
                        'Me.Session("bflag") = Nothing
                        Me.txtSearchID.Text = String.Empty
                        Me.txtSearchEName.Text = String.Empty
                        gvBranch.Visible = False
                        lblNoRec.Visible = True
                    End If
                    DataRead.Close()
                End Using
            End Using
        End Using
    End Sub

    'Protected Sub txtSearchEName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchEName.TextChanged
    '    If Me.Session("_txtBEName") <> Nothing Or Me.Session("_txtBEID") <> Nothing Then
    '        Me.Session("_txtBEID") = Nothing
    '        Me.Session("_txtBEName") = Nothing            
    '    End If
    'End Sub

    'Protected Sub txtSearchID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchID.TextChanged
    '    If Me.Session("_txtBEName") <> Nothing Or Me.Session("_txtBEID") <> Nothing Then
    '        Me.Session("_txtBEID") = Nothing
    '        Me.Session("_txtBEName") = Nothing
    '    End If
    'End Sub
End Class