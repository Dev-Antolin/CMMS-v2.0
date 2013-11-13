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
Partial Class UserControl_UsersIDD
    Inherits System.Web.UI.UserControl
    Public UsersIDDSelected As UsersIDDDelegate
    Public UsersIDDToFind As SearchDelegate
    Dim Status As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblNoRec.Visible = False
        If Not Me.Session("isDivision") Then
            Exit Sub
        End If
        If Me.Session("JDesc").ToString.Contains("HELPDESK") OrElse _
          Me.Session("JDesc").ToString.Contains("MMD") Then
            If Me.Session("View") = "canUpdate" Then
                getDivisionEmployee()
            ElseIf Me.Session("View") Is Nothing Then
                getDivisionID()
            End If
        Else
            getDivisionID()
        End If        
    End Sub

    Protected Sub Select_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentrow As GridViewRow = TryCast(DirectCast(sender, Control).Parent.NamingContainer, GridViewRow)
        If currentrow IsNot Nothing Then
            If UsersIDDSelected IsNot Nothing Then
                UsersIDDSelected.Invoke(currentrow.Cells(1).Text, currentrow.Cells(2).Text)
                Me.Session("_txtEName") = Nothing
                Me.Session("_txtEID") = Nothing
                Me.Session("flag") = Nothing
                Me.txtSearchID.Text = String.Empty
                Me.txtSearchEName.Text = String.Empty
            Else
                Throw New ArgumentNullException("UsersIDSelected Event not registered")
            End If
        End If
    End Sub

    Protected Sub btnSearchBranch_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        If UsersIDDToFind IsNot Nothing Then
            UsersIDDToFind.Invoke("Search")
        Else
            Throw New ArgumentNullException("JobTitleToFind Event not registered")
        End If
    End Sub

    Private Sub getDivisionID()
        If Me.Session("_txtEName") <> Nothing Or Me.Session("_txtEID") <> Nothing Then
            txtSearchID.Text = Me.Session("_txtEID")
            txtSearchEName.Text = Me.Session("_txtEName")
            Me.Session.Add("flag", 1)
        End If
        Dim Div As New DataTable("wa.res_id")
        Dim sql As New StringBuilder()
        sql.Append("SELECT LTRIM(RTRIM(wa.res_id)) AS res_id, LTRIM(RTRIM(UPPER(wa.fullname))) AS fullname ")
        sql.Append("FROM dbo.WebAccounts AS wa INNER JOIN irdivision AS ird ON wa.costcenter = ird.costcenter ")
        sql.Append("WHERE wa.zonecode = @Zone ")
        If Me.Session("Code") <> Nothing Then
            sql.Append("AND ird.divisionacro = @DivCode ")
        End If
        If Trim(Me.txtSearchID.Text) <> Nothing Then
            sql.Append("AND wa.res_id LIKE RTRIM(@EmpID) ")
            Me.Session.Add("flag", 1)
        End If
        If Trim(Me.txtSearchEName.Text) <> Nothing Then
            sql.Append("AND wa.fullname LIKE RTRIM(@EmpName) ")
            Me.Session.Add("flag", 1)
        End If
        sql.Append("ORDER BY wa.fullname; ")

        Try
            Using con As New SqlConnection(Me.Session("strConf").ToString())
                con.Open()
                Using cmd As New SqlCommand(sql.ToString(), con)
                    Dim a As String = Me.Session("Code")
                    cmd.Parameters.Add(New SqlParameter("Zone", SqlDbType.VarChar, 6)).Value = Trim(Me.Session("ZCode"))
                    If Me.Session("Code") <> Nothing Then
                        cmd.Parameters.Add(New SqlParameter("DivCode", SqlDbType.VarChar, 3)).Value() = Trim(Me.Session("Code").ToString())
                    End If
                    If Trim(Me.txtSearchID.Text) <> Nothing Then
                        cmd.Parameters.Add(New SqlParameter("EmpID", SqlDbType.VarChar, 10)).Value() = "%" & Trim(Me.txtSearchID.Text) & "%"
                    End If
                    If Trim(Me.txtSearchEName.Text) <> Nothing Then
                        cmd.Parameters.Add(New SqlParameter("EmpName", SqlDbType.VarChar, 64)).Value() = "%" & Trim(Me.txtSearchEName.Text) & "%"
                    End If
                    cmd.CommandType = CommandType.Text
                    Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                        If DataRead.HasRows() Then
                            Div.Load(DataRead)
                            gvBranch.Visible = True
                            gvBranch.DataSource = Div
                            gvBranch.DataBind()
                            If Me.txtSearchEName.Text = Nothing Or Me.txtSearchID.Text = Nothing Then
                                Me.Session.Add("isEmpty", "empty")
                            End If
                            Me.Session.Add("_txtEName", txtSearchEName.Text)
                            Me.Session.Add("_txtEID", txtSearchID.Text)
                            Me.txtSearchID.Text = String.Empty
                            Me.txtSearchEName.Text = String.Empty
                        Else
                            Me.Session("_txtEName") = Nothing
                            Me.Session("_txtEID") = Nothing
                            Me.txtSearchID.Text = String.Empty
                            Me.txtSearchEName.Text = String.Empty
                            gvBranch.Visible = False
                            lblNoRec.Visible = True
                        End If
                        DataRead.Close()
                    End Using
                End Using
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Private Sub getDivisionEmployee()

        If Me.Session("_txtEName") <> Nothing Or Me.Session("_txtEID") <> Nothing Then
            txtSearchID.Text = Me.Session("_txtEID")
            txtSearchEName.Text = Me.Session("_txtEName")
            Me.Session.Add("flag", 1)
        End If

        Dim Div As New DataTable("wa.res_id")
        Dim sql As New StringBuilder()
        If Me.Session("strCon").ToString = String.Empty Then Exit Sub
        Using con As New MySqlConnection(Me.Session("strCon").ToString)
            con.Open()
            sql.Append("SELECT DISTINCT Res_ID_Assigned AS res_id,Emp_Name_Assigned AS fullname FROM cmms.cmms_entry_masterheader ")
            sql.Append("WHERE Bc_Code = @Bcode ")
            sql.Append("AND Item_Code IN ('500','501','502') ")
            If Trim(Me.txtSearchID.Text) <> Nothing Then
                sql.Append("AND Res_Id_Assigned LIKE @res_id ")
                Me.Session.Add("flag", 1)
            End If
            If Trim(Me.txtSearchEName.Text) <> Nothing Then
                sql.Append("AND Emp_Name_Assigned LIKE @res_name ")
                Me.Session.Add("flag", 1)
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
                    cmd.Parameters.Add(New MySqlParameter("res_id", MySqlDbType.VarString, 50)).Value() = "%" & Trim(Me.txtSearchID.Text) & "%"
                End If
                If Trim(Me.txtSearchEName.Text) <> Nothing Then
                    cmd.Parameters.Add(New MySqlParameter("res_name", MySqlDbType.VarString, 50)).Value() = "%" & Trim(Me.txtSearchEName.Text) & "%"
                End If
                cmd.CommandType() = CommandType.Text
                Using DataRead As MySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    If DataRead.HasRows() Then
                        Div.Load(DataRead)
                        gvBranch.Visible = True
                        gvBranch.DataSource = Div
                        gvBranch.DataBind()
                        Me.Session.Add("_txtEName", txtSearchEName.Text)
                        Me.Session.Add("_txtEID", txtSearchID.Text)
                        Me.txtSearchID.Text = String.Empty
                        Me.txtSearchEName.Text = String.Empty
                    Else
                        Me.Session("_txtEName") = Nothing
                        Me.Session("_txtEID") = Nothing
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
    '    If Me.Session("_txtEName") <> Nothing Or Me.Session("_txtEID") <> Nothing Then
    '        Me.Session("_txtEName") = Nothing
    '        Me.Session("_txtEID") = Nothing
    '    End If
    'End Sub

    'Protected Sub txtSearchID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchID.TextChanged
    '    If Me.Session("_txtEName") <> Nothing Or Me.Session("_txtEID") <> Nothing Then
    '        Me.Session("_txtEName") = Nothing
    '        Me.Session("_txtEID") = Nothing
    '    End If
    'End Sub
End Class
