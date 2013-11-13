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

Partial Class UserControl_Branch
    Inherits System.Web.UI.UserControl

    Public BranchSelected As BranchDelegate
    Public BranchToFind As SearchDelegate


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblNoRec.Visible = False
        If Me.Session("isDivision") Then
            Exit Sub
        End If
        If Me.Session("JDesc").ToString.Contains("HELPDESK") OrElse _
           Me.Session("JDesc").ToString.Contains("MMD") Then
            If Me.Session("View") = "canUpdate" Then
                SearchByDivisionBranch()
            ElseIf Me.Session("View") Is Nothing Then
                Branches()
            End If
        ElseIf Me.Session("JDesc").ToString.Contains("LPTL") Then
            If Me.Session("View") = "canUpdate" Then
                SearchByBranch(Me.Session("JRegion").ToString())
            ElseIf Me.Session("View") Is Nothing Then
                Branches()
            End If
        Else
            Branches()
        End If
    End Sub

    Protected Sub Select_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        Dim currentrow As GridViewRow = TryCast(DirectCast(sender, Control).Parent.NamingContainer, GridViewRow)
        If currentrow IsNot Nothing Then            
            If BranchSelected IsNot Nothing Then
                BranchSelected.Invoke(currentrow.Cells(1).Text, currentrow.Cells(2).Text)
                Me.Session("SBCode") = Nothing
                Me.Session("SBName") = Nothing                
            Else
                Throw New ArgumentNullException("BranchSelected Event not registered")
            End If
        End If
    End Sub

    Protected Sub btnSearchBranch_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        If BranchToFind IsNot Nothing Then
            BranchToFind.Invoke("Search")
        Else
            Throw New ArgumentNullException("JobTitleToFind Event not registered")
        End If

    End Sub

    Public Sub Branches()
        Dim sql As New StringBuilder()
        Dim Branch As New DataTable("bedrnr")
        If Me.Session("strConf").ToString = String.Empty Then Exit Sub

        If Me.Session("SBName") <> Nothing Or Me.Session("SBCode") <> Nothing Then
            txtSearchBCode.Text = Me.Session("SBCode")
            txtSearchBName.Text = Me.Session("SBName")
        End If
        Using con As New SqlConnection(Me.Session("strConf").ToString)
            con.Open()
            'sql.Append("SELECT  DISTINCT")
            'sql.Append("    wb.bedrnr, ")
            'sql.Append("    RTRIM(wb.bedrnm) AS bedrnm ")
            'sql.Append("FROM ")
            'sql.Append("	WebProject.dbo.WebAccounts wa ")
            'sql.Append("INNER JOIN ")
            'sql.Append("    WebProject.dbo.WebBranches wb ")
            'sql.Append("ON ")
            'sql.Append("    wa.comp = wb.bedrnr AND ")
            'sql.Append("	wa.zonecode = wb.zonecode ")
            'sql.Append("WHERE ")
            'sql.Append("    wa.zonecode = @Zone ")
            'sql.Append("	AND wb.bedrnr NOT IN ('001','002') ")
            'If Trim(txtSearchBName.Text) <> Nothing Then sql.Append("AND LTRIM(RTRIM(bedrnm)) LIKE @Bname ")
            'If Trim(txtSearchBCode.Text) <> Nothing Then sql.Append("AND LTRIM(RTRIM(bedrnr)) = @Bcode ")
            'sql.Append("ORDER BY ")
            'sql.Append("	wb.bedrnr; ")
 
            sql.Append("SELECT DISTINCT LTRIM(RTRIM(bedrnr)) AS bedrnr , LTRIM(RTRIM(bedrnm)) AS bedrnm ")
            sql.Append("FROM dbo.WebBranches ")
            sql.Append("WHERE LTRIM(RTRIM(bedrnr)) NOT IN ('001','002') ")
            If Trim(txtSearchBName.Text) <> Nothing Then
                sql.Append("AND LTRIM(RTRIM(bedrnm)) LIKE @Bname ")
            End If
            If Trim(txtSearchBCode.Text) <> Nothing Then
                sql.Append("AND LTRIM(RTRIM(bedrnr)) = @Bcode ")
            End If            
            sql.Append("AND zonecode = @Zone ")            
            sql.Append("ORDER BY bedrnm ASC; ")
            Using cmd As New SqlCommand(sql.ToString(), con)
                If Trim(Me.txtSearchBName.Text) <> Nothing Then
                    cmd.Parameters.Add(New SqlParameter("Bname", SqlDbType.VarChar, 60)).Value = "%" & Trim(Me.txtSearchBName.Text) & "%"
                End If
                If Trim(Me.txtSearchBCode.Text) <> Nothing Then
                    cmd.Parameters.Add(New SqlParameter("Bcode", SqlDbType.VarChar, 6)).Value = Trim(Me.txtSearchBCode.Text)
                End If
                cmd.Parameters.AddWithValue("Zone", Trim(Me.Session("ZCode")))
                cmd.CommandType = CommandType.Text
                Using DataRead As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    If DataRead.HasRows Then
                        Branch.Load(DataRead)
                        gvBranch.Visible = True
                        gvBranch.DataSource = Branch
                        gvBranch.DataBind()
                        Me.Session.Add("SBCode", Me.txtSearchBCode.Text)
                        Me.Session.Add("SBName", Me.txtSearchBName.Text)
                        Me.txtSearchBName.Text = String.Empty
                        Me.txtSearchBCode.Text = String.Empty
                    Else
                        Me.Session("SBCode") = Nothing
                        Me.Session("SBName") = Nothing
                        Me.txtSearchBName.Text = String.Empty
                        Me.txtSearchBCode.Text = String.Empty
                        gvBranch.Visible = False
                        lblNoRec.Visible = True
                    End If
                    DataRead.Close()
                End Using
            End Using
        End Using
    End Sub

    Private Function MSBranch(ByVal zone As String, ByVal region As String) As String
        Dim str As New StringBuilder()
        Dim sql As New StringBuilder()        
        Using con As New SqlConnection(Me.Session("strConf").ToString)
            con.Open()
            sql.Append("SELECT bedrnm FROM webbranches ")
            sql.Append("WHERE zonecode = @Zone AND Class_03 = @Region ")
            sql.Append("AND bedrnr NOT IN ('001','002')  ORDER BY bedrnm; ")
            'sql.Append("AND (bedrnr <> '001' AND bedrnr <> '002') ORDER BY bedrnm; ")
            Using cmd As New SqlCommand(sql.ToString(), con)
                cmd.Parameters.AddWithValue("Region", Trim(region))
                cmd.Parameters.AddWithValue("Zone", Trim(zone))
                'cmd.Parameters.Add(New SqlParameter("Region", SqlDbType.VarChar)).Value = Trim(region)
                'cmd.Parameters.Add(New SqlParameter("Zone", SqlDbType.VarChar)).Value = Trim(zone)
                cmd.CommandType = CommandType.Text
                Using DataRead As SqlDataReader = cmd.ExecuteReader()
                    If DataRead.HasRows Then
                        While DataRead.Read
                            str.Append("'")
                            str.Append(Trim(DataRead(0).ToString()))
                            str.Append("',")
                        End While
                        str.Remove(str.Length - 1, 1)               
                    End If
                End Using
            End Using
            con.Close()
        End Using
        Return str.ToString()
    End Function

    Public Sub SearchByBranch(ByVal region As String)
        Dim str As String = MSBranch(Me.Session("ZCode").ToString(), Me.Session("JRegion").ToString)
        Dim sql As New StringBuilder()
        Dim Branch As New DataTable("bedrnr")
        If Me.Session("strCon").ToString = String.Empty Then Exit Sub

        If Me.Session("SBName") <> Nothing Or Me.Session("SBCode") <> Nothing Then
            txtSearchBCode.Text = Me.Session("SBCode")
            txtSearchBName.Text = Me.Session("SBName")
        End If

        Using con As New MySqlConnection(Me.Session("strCon").ToString)
            con.Open()
            sql.Append("SELECT DISTINCT TRIM(Bc_Code) AS bedrnr, TRIM(Bc_Name) AS bedrnm ,Zone_Code FROM cmms.cmms_entry_masterheader ")
            sql.Append("WHERE IsNumeric(Bc_Code) AND Bc_Name IN (" & str & ") ")
            If Trim(txtSearchBName.Text) <> Nothing Then
                sql.Append("AND TRIM(Bc_Name) LIKE @Bname ")
            End If
            If Trim(txtSearchBCode.Text) <> Nothing Then
                sql.Append("AND TRIM(Bc_Code) = @Bcode ")
            End If
            sql.Append("AND Item_Code IN ('500','501','502') ")
            sql.Append("AND Zone_Code = @Zone ")
            sql.Append("ORDER BY Bc_Name ASC; ")

            Using cmd As New MySqlCommand(sql.ToString(), con)
                If Trim(Me.txtSearchBName.Text) <> Nothing Then
                    cmd.Parameters.Add(New MySqlParameter("Bname", MySqlDbType.VarString, 100)).Value = "%" & Trim(Me.txtSearchBName.Text) & "%"
                End If
                If Trim(Me.txtSearchBCode.Text) <> Nothing Then
                    cmd.Parameters.Add(New MySqlParameter("Bcode", MySqlDbType.VarString, 3)).Value = Trim(Me.txtSearchBCode.Text)
                End If
                cmd.Parameters.AddWithValue("Zone", Trim(Me.Session("ZCode")))
                'cmd.Parameters.Add(New MySqlParameter("brm", MySqlDbType.Text)).Value = str
                'cmd.Parameters.Add(New MySqlParameter("Zone", MySqlDbType.VarString, 10)).Value() = Trim(Me.Session("ZCode"))
                cmd.CommandType = CommandType.Text
                Using DataRead As MySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    If DataRead.HasRows Then
                        Branch.Load(DataRead)
                        gvBranch.Visible = True
                        gvBranch.DataSource = Branch
                        gvBranch.DataBind()
                        Me.Session.Add("SBCode", Trim(Me.txtSearchBCode.Text))
                        Me.Session.Add("SBName", Trim(Me.txtSearchBName.Text))
                        Me.txtSearchBName.Text = String.Empty
                        Me.txtSearchBCode.Text = String.Empty
                    Else
                        Me.Session("SBCode") = Nothing
                        Me.Session("SBName") = Nothing
                        Me.txtSearchBName.Text = String.Empty
                        Me.txtSearchBCode.Text = String.Empty
                        gvBranch.Visible = False
                        lblNoRec.Visible = True
                    End If
                    DataRead.Close()
                End Using
            End Using
        End Using
    End Sub

    Public Sub SearchByDivisionBranch()
        If Me.Session("SBName") <> Nothing Or Me.Session("SBCode") <> Nothing Then
            txtSearchBCode.Text = Me.Session("SBCode")
            txtSearchBName.Text = Me.Session("SBName")
        End If

        Dim Branch As New DataTable("bedrnr")
        Dim sql As New StringBuilder()

        If Me.Session("strCon").ToString = String.Empty Then Exit Sub
        Using con As New MySqlConnection(Me.Session("strCon").ToString)
            con.Open()
            sql.Append("SELECT DISTINCT TRIM(Bc_Code) AS bedrnr, TRIM(Bc_Name) AS bedrnm ,Zone_Code FROM cmms.cmms_entry_masterheader ")
            sql.Append("WHERE IsNumeric(Bc_Code) ")
            If Trim(txtSearchBName.Text) <> Nothing Then
                sql.Append("AND TRIM(Bc_Name) LIKE @Bname ")
            End If
            If Trim(txtSearchBCode.Text) <> Nothing Then
                sql.Append("AND TRIM(Bc_Code) = @Bcode ")
            End If
            sql.Append("AND Item_Code IN ('500','501','502') ")
            sql.Append("AND Bc_Name <> '' ")
            sql.Append("AND zone_Code = @Zone ")
            sql.Append("ORDER BY Bc_Name ASC; ")

            Using cmd As New MySqlCommand(sql.ToString(), con)
                If Trim(Me.txtSearchBName.Text) <> Nothing Then
                    cmd.Parameters.Add(New MySqlParameter("Bname", MySqlDbType.VarString, 100)).Value = "%" & Trim(Me.txtSearchBName.Text) & "%"
                End If
                If Trim(Me.txtSearchBCode.Text) <> Nothing Then
                    cmd.Parameters.Add(New MySqlParameter("Bcode", MySqlDbType.VarString, 3)).Value = Trim(Me.txtSearchBCode.Text)
                End If
                cmd.Parameters.Add(New MySqlParameter("Zone", MySqlDbType.VarString, 10)).Value() = Trim(Me.Session("ZCode"))
                cmd.CommandType = CommandType.Text
                Using DataRead As MySqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    If DataRead.HasRows Then
                        Branch.Load(DataRead)
                        gvBranch.Visible = True
                        gvBranch.DataSource = Branch
                        gvBranch.DataBind()
                        Me.Session.Add("SBCode", Me.txtSearchBCode.Text)
                        Me.Session.Add("SBName", Me.txtSearchBName.Text)
                        Me.txtSearchBName.Text = String.Empty
                        Me.txtSearchBCode.Text = String.Empty
                    Else
                        Me.Session("SBCode") = Nothing
                        Me.Session("SBName") = Nothing
                        Me.txtSearchBName.Text = String.Empty
                        Me.txtSearchBCode.Text = String.Empty
                        gvBranch.Visible = False
                        lblNoRec.Visible = True
                    End If
                    DataRead.Close()
                End Using
            End Using
        End Using
    End Sub



    Protected Sub txtSearchBCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchBCode.TextChanged
        'If Me.Session("SBName") <> Nothing Or Me.Session("SBCode") <> Nothing Then
        '    Me.Session("SBCode") = Nothing
        '    Me.Session("SBName") = Nothing
        'End If
    End Sub


    Protected Sub txtSearchBName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchBName.TextChanged
        'If Me.Session("SBName") <> Nothing Or Me.Session("SBCode") <> Nothing Then
        '    Me.Session("SBCode") = Nothing
        '    Me.Session("SBName") = Nothing
        'End If
    End Sub
End Class
