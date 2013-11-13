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
Partial Class UserControl_RCTDesignation
    Inherits System.Web.UI.UserControl
    Public DesignationRCT As RCTDesigDelegate
    Public DesigToFindT As SearchDelegate
    Dim SelectedCode As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblNoRec.Visible = False
        findRCT()
    End Sub

    Private Sub findRCT()
        If Me.Session("strConf") = "" OrElse Me.Session("Desig").ToString = "0" Then
            Exit Sub
        End If

        Dim strCon As String = Me.Session("strConf")
        Dim sql As String
        Dim code As String = txtSearchRCT.Text
        Dim cn As SqlConnection = New SqlConnection(strCon)
        SelectedCode = Me.Session("Desig")
        Dim URegion As String = Me.Session("JRegion")

        If SelectedCode = "0" Then
            Exit Sub
        Else
            If txtSearchRCT.Text = "" Then
                sql = "select distinct upper(fullname) as fullname from irrcts where class_03 = '" & URegion.Trim & "' order by fullname asc;"
            Else
                sql = "select distinct upper(fullname) as fullname from irrcts where class_03 = '" & URegion.Trim & "' and fullname like '" & code.Trim & "%' order by fullname asc;"
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
            If DesignationRCT IsNot Nothing Then
                DesignationRCT.Invoke(currentrow.Cells(1).Text)
            Else
                Throw New ArgumentNullException("DivisionSelected Event not registered")
            End If
        End If
    End Sub

    Protected Sub btnSearchBranch_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        If DesigToFindT IsNot Nothing Then
            DesigToFindT.Invoke("Search")
        Else
            Throw New ArgumentNullException("JobTitleToFind Event not registered")
        End If
    End Sub
End Class
