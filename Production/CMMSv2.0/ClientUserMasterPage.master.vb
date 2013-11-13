Imports INI_DLL
Imports MYSQLDB_DLL
Imports MySql.Data.MySqlClient

Imports System.Collections.Generic
Imports System.Web.Services
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Partial Class ClientUserMasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Request.UserAgent.IndexOf("AppleWebKit") > 0 Then
            Request.Browser.Adapters.Clear()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Session("click") = "WOMRABLR" Then
            WOMRABLR.ForeColor = Drawing.Color.Orange
            WOMRABLR.BackColor = Drawing.Color.Transparent
        End If
        Dim fName As String = StrConv(Me.Session("fName"), VbStrConv.ProperCase)
        Dim wee As String = "Welcome, " & fName.ToUpper
        lblLoginUser.Text = wee
    End Sub

    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        HttpContext.Current.Session.Abandon()
        Response.Redirect("~/login.aspx")
    End Sub

    <WebMethod()> _
    Public Sub KillSession()
        HttpContext.Current.Session.Abandon()
    End Sub
End Class

